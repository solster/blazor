using System.Reflection;
using Microsoft.Extensions.Options;

namespace Solster.AspNetCore.Components.Templating;

internal sealed class TypeMapResolver : ITypeMapResolver
{
    private readonly IReadOnlyDictionary<String, Type> _nameToType;
    private readonly IReadOnlyDictionary<Type, String> _typeToName;

    public TypeMapResolver(IOptions<TypeMapResolverOptions> options)
        : this(options.Value)
    {
    }

    public TypeMapResolver(TypeMapResolverOptions options)
    {
        var context = new TypeProviderContext();

        foreach (var provider in options.TypeProviders)
        {
            provider.OnDiscovering(context);
        }

        var nameToType = new Dictionary<String, Type>(StringComparer.OrdinalIgnoreCase);
        var typeToName = new Dictionary<Type, String>();

        foreach (var type in context.Types.Distinct())
        {
            var name = ResolveName(type);

            if (nameToType.TryGetValue(name, out var existingType) && existingType != type)
            {
                throw new TypeMapResolverException($"Duplicate type key '{name}'.");
            }

            nameToType[name] = type;
            typeToName[type] = name;
        }

        _nameToType = nameToType;
        _typeToName = typeToName;
    }

    public Type Resolve(String typeName)
    {
        if (TryResolve(typeName, out var type))
        {
            return type;
        }

        throw new TypeMapResolverException(
            $"No type found for '{typeName}'. Available: {String.Join(", ", _nameToType.Keys)}");
    }

    public Boolean TryResolve(String typeName, out Type type)
        => _nameToType.TryGetValue(typeName, out type!);

    public Boolean TryGetName(Type type, out String name)
        => _typeToName.TryGetValue(type, out name!);

    private static String ResolveName(Type type)
        => type.GetCustomAttribute<JsonTypeKeyAttribute>()?.Key ?? type.Name;
}


