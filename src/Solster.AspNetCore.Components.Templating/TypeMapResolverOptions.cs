namespace Solster.AspNetCore.Components.Templating;

/// <summary>
/// Options for string-to-type and type-to-string mapping.
/// </summary>
public class TypeMapResolverOptions
{
    public IList<ITypeProvider> TypeProviders { get; } = [];
}

