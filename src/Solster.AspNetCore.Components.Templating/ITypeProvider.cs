namespace Solster.AspNetCore.Components.Templating;

public interface ITypeProvider
{
    void OnDiscovering(TypeProviderContext context);
}

public sealed class TypeProviderContext
{
    public List<Type> Types { get; } = [];
}

