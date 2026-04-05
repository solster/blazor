using Microsoft.AspNetCore.Components;

namespace Solster.Blazor.Templating;

/// <summary>
/// Decorates <see cref="IHtmlRenderer"/> to inline CSS using PreMailer.Net.
/// </summary>
public sealed class PreMailerHtmlRenderer(IHtmlRenderer inner, Uri cssBaseUri) : IHtmlRenderer
{
    public async Task<String> RenderAsync<TComponent, TModel>(TModel model, bool inlineCss = false)
        where TComponent : IHtmlTemplate<TModel>
    {
        var html = await inner.RenderAsync<TComponent, TModel>(model);
        return inlineCss ? InlineCss(html) : html;
    }

    public async Task<String> RenderAsync<TComponent>(bool inlineCss = false)
        where TComponent : IComponent
    {
        var html = await inner.RenderAsync<TComponent>();
        return inlineCss ? InlineCss(html) : html;
    }

    private String InlineCss(String html)
    {
        var result = new PreMailer.Net.PreMailer(html, cssBaseUri).MoveCssInline(removeComments: true);
        return result.Html;
    }
}
