using System.Reflection;

namespace Solster.Blazor.Templating;

/// <summary>
/// Options for <see cref="TemplateResolver"/>, specifying which assembly to scan for <see cref="IHtmlTemplate{TModel}"/> implementations.
/// </summary>
public sealed class TemplateResolverOptions
{
    /// <summary>
    /// The assembly to scan for <see cref="IHtmlTemplate{TModel}"/> implementations.
    /// Defaults to <see cref="Assembly.GetEntryAssembly()"/>.
    /// </summary>
    public Assembly TemplateAssembly { get; set; } = Assembly.GetEntryAssembly()!;
}
