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
    Task<String> RenderAsync<TComponent, TModel>(TModel model)
        where TComponent : IHtmlTemplate<TModel>;

    /// <summary>
    /// Renders <typeparamref name="TComponent"/> with no model (for parameter-less templates).
    /// </summary>
    Task<String> RenderAsync<TComponent>()
        where TComponent : IComponent;
}
