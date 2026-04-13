using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Solster.AspNetCore.Components.Templating;

/// <summary>
/// Applies envelope body binding for types that contain a property annotated with <see cref="JsonBodyAttribute"/>.
/// </summary>
public sealed class JsonBodyConverterFactory(IServiceProvider serviceProvider) : JsonConverterFactory
{
    public override Boolean CanConvert(Type typeToConvert)
        => GetBodyProperty(typeToConvert) is not null;

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var converterType = typeof(JsonBodyConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType, serviceProvider, this, options)!;
    }

    private static PropertyInfo? GetBodyProperty(Type type)
        => type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .FirstOrDefault(p => p.GetCustomAttribute<JsonBodyAttribute>() is not null);

    private sealed class JsonBodyConverter<TEnvelope> : JsonConverter<TEnvelope>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly JsonSerializerOptions _fallbackOptions;
        private readonly PropertyInfo _bodyProperty;
        private readonly JsonBodyAttribute _attribute;
        private readonly PropertyInfo _typeProperty;
        private readonly String _bodyJsonName;
        private readonly String _typeJsonName;

        public JsonBodyConverter(IServiceProvider serviceProvider, JsonBodyConverterFactory owner, JsonSerializerOptions options)
        {
            _serviceProvider = serviceProvider;
            _fallbackOptions = BuildFallbackOptions(options, owner);
            _bodyProperty = ResolveBodyProperty();
            _attribute = ResolveBodyAttribute();
            _typeProperty = ResolveTypeProperty();
            _bodyJsonName = ResolveJsonPropertyName(_bodyProperty, options);
            _typeJsonName = ResolveJsonPropertyName(_typeProperty, options);
        }

        public override TEnvelope? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions serializerOptions)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var root = document.RootElement;
            var json = root.GetRawText();

            var envelope = JsonSerializer.Deserialize<TEnvelope>(json, _fallbackOptions);
            if (envelope is null)
            {
                return default;
            }

            if (!root.TryGetProperty(_bodyJsonName, out var bodyElement))
            {
                return envelope;
            }

            var typeName = _typeProperty.GetValue(envelope) as String;
            var resolver = GetResolver();
            if (!String.IsNullOrWhiteSpace(typeName) && resolver is not null && resolver.TryResolve(typeName, out var bodyType))
            {
                var typedBody = JsonSerializer.Deserialize(bodyElement.GetRawText(), bodyType, _fallbackOptions);
                AssignBody(envelope, typedBody);
                return envelope;
            }

            AssignBody(envelope, bodyElement.Clone());
            return envelope;
        }

        public override void Write(Utf8JsonWriter writer, TEnvelope value, JsonSerializerOptions serializerOptions)
        {
            var node = JsonSerializer.SerializeToNode(value, _fallbackOptions) as JsonObject ?? new JsonObject();
            var bodyValue = _bodyProperty.GetValue(value);
            var typeName = _typeProperty.GetValue(value) as String;
            var resolver = GetResolver();

            if (String.IsNullOrWhiteSpace(typeName) && bodyValue is not null && resolver is not null &&
                resolver.TryGetName(bodyValue.GetType(), out var resolvedName))
            {
                typeName = resolvedName;
                if (_typeProperty.CanWrite)
                {
                    _typeProperty.SetValue(value, typeName);
                }
            }

            if (!String.IsNullOrWhiteSpace(typeName))
            {
                node[_typeJsonName] = typeName;
            }

            if (bodyValue is not null)
            {
                var bodyType = bodyValue.GetType();
                node[_bodyJsonName] = JsonSerializer.SerializeToNode(bodyValue, bodyType, _fallbackOptions);
            }

            node.WriteTo(writer, serializerOptions);
        }

        private ITypeMapResolver? GetResolver()
        {
            if (String.IsNullOrWhiteSpace(_attribute.ResolverKey))
            {
                return _serviceProvider.GetService<ITypeMapResolver>();
            }

            return _serviceProvider.GetKeyedService<ITypeMapResolver>(_attribute.ResolverKey);
        }

        private void AssignBody(TEnvelope envelope, Object? bodyValue)
        {
            if (!_bodyProperty.CanWrite)
            {
                return;
            }

            if (bodyValue is null)
            {
                _bodyProperty.SetValue(envelope, null);
                return;
            }

            if (_bodyProperty.PropertyType.IsInstanceOfType(bodyValue))
            {
                _bodyProperty.SetValue(envelope, bodyValue);
                return;
            }

            if (bodyValue is JsonElement jsonElement && _bodyProperty.PropertyType == typeof(Object))
            {
                _bodyProperty.SetValue(envelope, jsonElement);
            }
        }

        private static PropertyInfo ResolveBodyProperty()
            => typeof(TEnvelope).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .FirstOrDefault(p => p.GetCustomAttribute<JsonBodyAttribute>() is not null)
               ?? throw new InvalidOperationException(
                   $"Type '{typeof(TEnvelope)}' must contain a property marked with {nameof(JsonBodyAttribute)}.");

        private JsonBodyAttribute ResolveBodyAttribute()
            => _bodyProperty.GetCustomAttribute<JsonBodyAttribute>()
               ?? throw new InvalidOperationException(
                   $"Type '{typeof(TEnvelope)}' must contain a property marked with {nameof(JsonBodyAttribute)}.");

        private PropertyInfo ResolveTypeProperty()
            => typeof(TEnvelope).GetProperty(_attribute.TypePropertyName, BindingFlags.Instance | BindingFlags.Public)
               ?? throw new InvalidOperationException(
                   $"Type '{typeof(TEnvelope)}' does not contain the type property '{_attribute.TypePropertyName}'.");

        private static String ResolveJsonPropertyName(PropertyInfo property, JsonSerializerOptions options)
            => property.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name
               ?? options.PropertyNamingPolicy?.ConvertName(property.Name)
               ?? property.Name;

        private static JsonSerializerOptions BuildFallbackOptions(JsonSerializerOptions source, JsonConverterFactory owner)
        {
            var options = new JsonSerializerOptions(source);
            var existing = options.Converters.FirstOrDefault(c => ReferenceEquals(c, owner));
            if (existing is not null)
            {
                options.Converters.Remove(existing);
            }

            return options;
        }
    }
}


