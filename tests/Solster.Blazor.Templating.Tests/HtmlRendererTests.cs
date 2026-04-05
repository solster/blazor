using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Solster.Blazor.Templating.Tests.Components;

namespace Solster.Blazor.Templating.Tests;

public sealed class HtmlRendererTests
{
    private static IServiceProvider BuildServiceProvider() =>
        new ServiceCollection().BuildServiceProvider();

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_ReturnsExpectedHtml()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<SimpleComponent>();

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_RendersModelData()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Alice"));

        html.Should().Contain("Alice");
    }

    [Fact]
    public async Task RenderAsync_DictionaryParameters_RendersModelData()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var parameters = new Dictionary<String, Object?> { ["Model"] = new GreetingModel("Charlie") };
        var html = await renderer.RenderAsync<GreetingComponent>(parameters);

        html.Should().Contain("Charlie");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_DoesNotContainOtherName()
    {
        var sp = BuildServiceProvider();
        var renderer = new HtmlRenderer(sp, NullLoggerFactory.Instance);

        var html = await renderer.RenderAsync<GreetingComponent, GreetingModel>(new GreetingModel("Bob"));

        html.Should().NotContain("Alice");
        html.Should().Contain("Bob");
    }
}
