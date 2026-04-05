using Microsoft.AspNetCore.Components;

namespace Solster.Blazor.Templating;

/// <summary>
/// Renders a Blazor component to an HTML string.
/// </summary>
public interface IHtmlRenderer
{
    /// <summary>
    /// Renders <typeparamref name="TComponent"/> with the given <paramref name="model"/> to an HTML string.
    /// </summary>
    /// <param name="inlineCss">
    /// When <see langword="true"/>, CSS is inlined into the HTML if the renderer supports it.
    /// Defaults to <see langword="false"/>.
    /// </param>
    Task<String> RenderAsync<TComponent, TModel>(TModel model, bool inlineCss = false)
        where TComponent : IHtmlTemplate<TModel>;

    /// <summary>
    /// Renders <typeparamref name="TComponent"/> with the given <paramref name="parameters"/> dictionary to an HTML string.
    /// </summary>
    /// <param name="parameters">
    /// A dictionary of component parameter names and their values, passed directly to the component.
    /// </param>
    /// <param name="inlineCss">
    /// When <see langword="true"/>, CSS is inlined into the HTML if the renderer supports it.
    /// Defaults to <see langword="false"/>.
    /// </param>
    Task<String> RenderAsync<TComponent>(Dictionary<String, Object?> parameters, bool inlineCss = false)
        where TComponent : IComponent;

    /// <summary>
    /// Renders <typeparamref name="TComponent"/> with no model (for parameter-less templates).
    /// </summary>
    /// <param name="inlineCss">
    /// When <see langword="true"/>, CSS is inlined into the HTML if the renderer supports it.
    /// Defaults to <see langword="false"/>.
    /// </param>
    Task<String> RenderAsync<TComponent>(bool inlineCss = false)
        where TComponent : IComponent;
}
