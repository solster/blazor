using Microsoft.Extensions.DependencyInjection;
using Solster.Blazor.Templating.Tests.Components;

namespace Solster.Blazor.Templating.Tests;

public sealed class PreMailerHtmlRendererTests
{
    private static readonly Uri DummyBaseUri = new("https://example.com/");

    private static IServiceProvider BuildServiceProvider() =>
        new ServiceCollection().BuildServiceProvider();

    [Fact]
    public async Task RenderAsync_TypedComponent_InlinesCssWhenInlineCssTrue()
    {
        var sp = BuildServiceProvider();
        var inner = new HtmlRenderer(sp);
        var renderer = new PreMailerHtmlRenderer(inner, DummyBaseUri);

        var html = await renderer.RenderAsync<StyledComponent, CssModel>(new CssModel("My Title"), inlineCss: true);

        html.Should().Contain("My Title");
        // CSS inlined — styles are applied as inline attributes on elements
        html.Should().Contain("style=");
    }

    [Fact]
    public async Task RenderAsync_TypedComponent_SkipsCssWhenInlineCssFalse()
    {
        var sp = BuildServiceProvider();
        var inner = new HtmlRenderer(sp);
        var renderer = new PreMailerHtmlRenderer(inner, DummyBaseUri);

        var html = await renderer.RenderAsync<StyledComponent, CssModel>(new CssModel("My Title"), inlineCss: false);

        html.Should().Contain("My Title");
        // CSS not inlined, so no inline style attribute on elements
        html.Should().NotContain("<h1 style=");
    }

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_DoesNotInlineCssByDefault()
    {
        var sp = BuildServiceProvider();
        var inner = new HtmlRenderer(sp);
        var renderer = new PreMailerHtmlRenderer(inner, DummyBaseUri);

        var html = await renderer.RenderAsync<SimpleComponent>();

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public async Task RenderAsync_ParameterlessComponent_SkipsCssWhenInlineCssFalse()
    {
        var sp = BuildServiceProvider();
        var inner = new HtmlRenderer(sp);
        var renderer = new PreMailerHtmlRenderer(inner, DummyBaseUri);

        var html = await renderer.RenderAsync<SimpleComponent>(inlineCss: false);

        html.Should().Contain("Hello, world!");
    }

    [Fact]
    public void AddHtmlRenderer_RegistersPreMailerRenderer()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer(DummyBaseUri);
        var sp = services.BuildServiceProvider();

        var renderer = sp.GetRequiredService<IHtmlRenderer>();

        renderer.Should().BeOfType<PreMailerHtmlRenderer>();
    }

    [Fact]
    public void AddHtmlRenderer_ReturnsSameInstance_WhenResolvedTwice()
    {
        var services = new ServiceCollection();
        services.AddHtmlRenderer(DummyBaseUri);
        var sp = services.BuildServiceProvider();

        var renderer1 = sp.GetRequiredService<IHtmlRenderer>();
        var renderer2 = sp.GetRequiredService<IHtmlRenderer>();

        renderer1.Should().BeSameAs(renderer2);
    }
}
