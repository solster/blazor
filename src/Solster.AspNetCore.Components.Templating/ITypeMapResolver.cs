namespace Solster.AspNetCore.Components.Templating;

/// <summary>
/// Resolves a string type name to a CLR <see cref="Type"/> and supports reverse lookup.
/// </summary>
public interface ITypeMapResolver
{
    /// <summary>
    /// Returns the mapped <see cref="Type"/> for <paramref name="typeName"/>.
    /// </summary>
    Type Resolve(String typeName);

    /// <summary>
    /// Attempts to resolve <paramref name="typeName"/> to a CLR type.
    /// </summary>
    Boolean TryResolve(String typeName, out Type type);

    /// <summary>
    /// Attempts to resolve a CLR type to its configured string key.
    /// </summary>
    Boolean TryGetName(Type type, out String name);
}

