using Microsoft.AspNetCore.Components;

namespace Solster.Blazor.Templating;

/// <summary>
/// Marker interface for Blazor components used as HTML email templates.
/// Implement this on your .razor component and expose a single Model parameter.
/// </summary>
public interface IHtmlTemplate<TModel> : IComponent
{
    TModel Model { get; set; }
}
