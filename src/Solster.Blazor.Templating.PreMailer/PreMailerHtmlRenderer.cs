using Microsoft.AspNetCore.Components;
using Solster.Blazor.Templating;

namespace Solster.Blazor.Templating.PreMailer;

/// <summary>
/// Decorates <see cref="IHtmlRenderer"/> to inline CSS using PreMailer.Net.
/// </summary>
public sealed class PreMailerHtmlRenderer(IHtmlRenderer inner, Uri cssBaseUri) : IHtmlRenderer
{
    public async Task<String> RenderAsync<TComponent, TModel>(TModel model)
        where TComponent : IHtmlTemplate<TModel>
    {
        var html = await inner.RenderAsync<TComponent, TModel>(model);
        return InlineCss(html);
    }

    public async Task<String> RenderAsync<TComponent>()
        where TComponent : IComponent
    {
        var html = await inner.RenderAsync<TComponent>();
        return InlineCss(html);
    }

    private String InlineCss(String html)
    {
        var result = new global::PreMailer.Net.PreMailer(html, cssBaseUri).MoveCssInline(removeComments: true);
        return result.Html;
    }
}
