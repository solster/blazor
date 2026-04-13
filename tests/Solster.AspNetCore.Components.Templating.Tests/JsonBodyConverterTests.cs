using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Solster.AspNetCore.Components.Templating.Tests;

public sealed class JsonBodyConverterTests
{
    [Fact]
    public void Read_UsesDefaultResolver_WhenNoKeyIsSpecified()
    {
        var services = new ServiceCollection();
        services.AddTypeMapResolver(options => options.TypeProviders.Add(new StaticTypeProvider(typeof(UserCreatedPayload))));
        var serviceProvider = services.BuildServiceProvider();

        var options = BuildSerializerOptions(serviceProvider);
        var json = "{\"Type\":\"user-created\",\"Body\":{\"UserId\":\"u-1\"}}";

        var envelope = JsonSerializer.Deserialize<DefaultEnvelope>(json, options);

        envelope.Should().NotBeNull();
        envelope!.Body.Should().BeOfType<UserCreatedPayload>();
        ((UserCreatedPayload)envelope.Body!).UserId.Should().Be("u-1");
    }

    [Fact]
    public void Read_UsesKeyedResolver_WhenKeyIsSpecifiedOnJsonBody()
    {
        var services = new ServiceCollection();
        services.AddKeyedTypeMapResolver("events", options =>
            options.TypeProviders.Add(new StaticTypeProvider(typeof(OrderPlacedPayload))));
        var serviceProvider = services.BuildServiceProvider();

        var options = BuildSerializerOptions(serviceProvider);
        var json = "{\"Type\":\"order-placed\",\"Body\":{\"OrderId\":\"o-1\"}}";

        var envelope = JsonSerializer.Deserialize<KeyedEnvelope>(json, options);

        envelope.Should().NotBeNull();
        envelope!.Body.Should().BeOfType<OrderPlacedPayload>();
        ((OrderPlacedPayload)envelope.Body!).OrderId.Should().Be("o-1");
    }

    [Fact]
    public void Read_KeyedResolver_MissingTypeForKey_KeepsRawJsonElementInBody()
    {
        var services = new ServiceCollection();
        services.AddTypeMapResolver(options =>
            options.TypeProviders.Add(new StaticTypeProvider(typeof(OrderPlacedPayload))));
        services.AddKeyedTypeMapResolver("events", options =>
            options.TypeProviders.Add(new StaticTypeProvider(typeof(UserCreatedPayload))));
        var serviceProvider = services.BuildServiceProvider();

        var options = BuildSerializerOptions(serviceProvider);
        var json = "{\"Type\":\"order-placed\",\"Body\":{\"OrderId\":\"o-2\"}}";

        var envelope = JsonSerializer.Deserialize<KeyedEnvelope>(json, options);

        envelope.Should().NotBeNull();
        envelope!.Body.Should().BeOfType<JsonElement>();
        ((JsonElement)envelope.Body!).GetProperty("OrderId").GetString().Should().Be("o-2");
    }

    [Fact]
    public void Read_UnknownType_KeepsRawJsonElementInBody()
    {
        var services = new ServiceCollection();
        services.AddTypeMapResolver(options => options.TypeProviders.Add(new StaticTypeProvider(typeof(UserCreatedPayload))));
        var serviceProvider = services.BuildServiceProvider();

        var options = BuildSerializerOptions(serviceProvider);
        var json = "{\"Type\":\"does-not-exist\",\"Body\":{\"x\":1}}";

        var envelope = JsonSerializer.Deserialize<DefaultEnvelope>(json, options);

        envelope.Should().NotBeNull();
        envelope!.Body.Should().BeOfType<JsonElement>();
        ((JsonElement)envelope.Body!).GetProperty("x").GetInt32().Should().Be(1);
    }

    [Fact]
    public void Write_InferTypeFromBody_WhenTypeIsMissing()
    {
        var services = new ServiceCollection();
        services.AddTypeMapResolver(options => options.TypeProviders.Add(new StaticTypeProvider(typeof(UserCreatedPayload))));
        var serviceProvider = services.BuildServiceProvider();

        var options = BuildSerializerOptions(serviceProvider);
        var envelope = new DefaultEnvelope
        {
            Body = new UserCreatedPayload("u-2")
        };

        var json = JsonSerializer.Serialize(envelope, options);

        json.Should().Contain("\"Type\":\"user-created\"");
        json.Should().Contain("\"UserId\":\"u-2\"");
    }

    private static JsonSerializerOptions BuildSerializerOptions(IServiceProvider serviceProvider)
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new JsonBodyConverterFactory(serviceProvider));
        return options;
    }

    private sealed class StaticTypeProvider(params Type[] types) : ITypeProvider
    {
        public void OnDiscovering(TypeProviderContext context)
            => context.Types.AddRange(types);
    }

    private sealed class DefaultEnvelope
    {
        public String? Type { get; set; }

        [JsonBody(nameof(Type))]
        public Object? Body { get; set; }
    }

    private sealed class KeyedEnvelope
    {
        public String? Type { get; set; }

        [JsonBody(nameof(Type), "events")]
        public Object? Body { get; set; }
    }

    [JsonTypeKey("user-created")]
    private sealed record UserCreatedPayload(String UserId);

    [JsonTypeKey("order-placed")]
    private sealed record OrderPlacedPayload(String OrderId);
}

