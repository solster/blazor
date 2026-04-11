using Microsoft.Extensions.Options;

namespace Solster.Blazor.Templating;

/// <summary>
/// Resolves template names to Blazor component types by scanning the configured <see cref="TemplateResolverOptions.TemplateAssembly"/>.
/// </summary>
public sealed class TemplateResolver : ITemplateResolver
{
    private readonly IReadOnlyDictionary<String, Type> _templates;

    /// <summary>
    /// Initialises a new instance of <see cref="TemplateResolver"/> using the assembly specified in <paramref name="options"/>.
    /// </summary>
    public TemplateResolver(IOptions<TemplateResolverOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options.Value.TemplateAssembly, nameof(options));

        _templates = GetLoadableTypes(options.Value.TemplateAssembly)
            .Where(t => t is { IsAbstract: false, IsInterface: false } &&
                        t.GetInterfaces().Any(i => i.IsGenericType &&
                                                   i.GetGenericTypeDefinition() == typeof(IHtmlTemplate<>)))
            .ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
    }

    private static IEnumerable<Type> GetLoadableTypes(System.Reflection.Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (System.Reflection.ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t is not null)!;
        }
    }

    /// <inheritdoc />
    public Type Resolve(String templateName)
    {
        if (_templates.TryGetValue(templateName, out var type))
        {
            return type;
        }

        throw new InvalidOperationException(
            $"No template found for '{templateName}'. Available: {String.Join(", ", _templates.Keys)}");
    }
}
