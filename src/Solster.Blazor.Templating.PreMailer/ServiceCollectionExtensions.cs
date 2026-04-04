using Microsoft.Extensions.DependencyInjection;
using Solster.Blazor.Templating;

namespace Solster.Blazor.Templating.PreMailer;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> with PreMailer.Net CSS inlining enabled.
    /// </summary>
    public static IServiceCollection AddHtmlRendererWithPreMailer(
        this IServiceCollection services,
        Uri cssBaseUri)
    {
        services.AddSingleton<IHtmlRenderer>(sp =>
            new PreMailerHtmlRenderer(new HtmlRenderer(sp), cssBaseUri));
        return services;
    }
}
