namespace Solster.AspNetCore.Components.Templating;

/// <summary>
/// Overrides the default type key (type name) used by <see cref="ITypeMapResolver"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class JsonTypeKeyAttribute(String key) : Attribute
{
    public String Key { get; } = key;
}

