using Microsoft.Extensions.DependencyInjection;

namespace Solster.Blazor.Templating;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> for rendering Blazor components to HTML strings.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(this IServiceCollection services)
    {
        services.AddSingleton<IHtmlRenderer, HtmlRenderer>();
        return services;
    }
}
