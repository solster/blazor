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

    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> with PreMailer.Net CSS inlining support.
    /// CSS inlining is opt-in per render call via the <c>inlineCss</c> parameter on <see cref="IHtmlRenderer.RenderAsync{TComponent,TModel}"/>.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(this IServiceCollection services, Uri cssBaseUri)
    {
        services.AddSingleton<IHtmlRenderer>(sp =>
            new PreMailerHtmlRenderer(new HtmlRenderer(sp), cssBaseUri));
        return services;
    }
}
