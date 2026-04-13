namespace Solster.AspNetCore.Components.Templating;

/// <summary>
/// Marks an envelope body property that should be materialized based on a sibling root-level type property.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class JsonBodyAttribute(String typePropertyName, String? resolverKey = null) : Attribute
{
    public String TypePropertyName { get; } = typePropertyName;

    public String? ResolverKey { get; } = resolverKey;
}

