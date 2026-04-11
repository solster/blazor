namespace Solster.Blazor.Templating;

/// <summary>
/// Resolves a template name to the corresponding Blazor component <see cref="Type"/>.
/// </summary>
public interface ITemplateResolver
{
    /// <summary>
    /// Returns the <see cref="Type"/> registered under <paramref name="templateName"/>.
    /// </summary>
    /// <param name="templateName">The case-insensitive name of the template to resolve.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no template with the given name is found.
    /// </exception>
    Type Resolve(String templateName);
}
