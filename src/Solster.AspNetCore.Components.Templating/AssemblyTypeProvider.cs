using System.Reflection;

namespace Solster.AspNetCore.Components.Templating;

public sealed class AssemblyTypeProvider(Assembly assembly) : ITypeProvider
{
    public void OnDiscovering(TypeProviderContext context)
    {
        try
        {
            context.Types.AddRange(assembly.GetTypes());
        }
        catch (ReflectionTypeLoadException ex)
        {
            context.Types.AddRange(ex.Types.OfType<Type>());
        }
    }
}

