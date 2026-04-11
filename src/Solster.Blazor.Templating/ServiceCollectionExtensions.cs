using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Solster.Blazor.Templating;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> for rendering Blazor components to HTML strings.
    /// Optionally configures <see cref="TemplateResolverOptions"/> and registers <see cref="ITemplateResolver"/>.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(
        this IServiceCollection services,
        Action<TemplateResolverOptions>? configureResolver = null)
    {
        services.AddLogging();
        services.AddSingleton<IHtmlRenderer, HtmlRenderer>();
        services.Configure<TemplateResolverOptions>(configureResolver ?? (_ => { }));
        services.TryAddSingleton<ITemplateResolver, TemplateResolver>();
        return services;
    }

    /// <summary>
    /// Registers <see cref="IHtmlRenderer"/> with PreMailer.Net CSS inlining support.
    /// CSS inlining is opt-in per render call via the <c>inlineCss</c> parameter on <see cref="IHtmlRenderer.RenderAsync{TComponent,TModel}"/>.
    /// Optionally configures <see cref="TemplateResolverOptions"/> and registers <see cref="ITemplateResolver"/>.
    /// </summary>
    public static IServiceCollection AddHtmlRenderer(
        this IServiceCollection services,
        Uri cssBaseUri,
        Action<TemplateResolverOptions>? configureResolver = null)
    {
        services.AddLogging();
        services.AddSingleton<IHtmlRenderer>(sp =>
            new HtmlRenderer(sp, sp.GetRequiredService<ILoggerFactory>(), cssBaseUri));
        services.Configure<TemplateResolverOptions>(configureResolver ?? (_ => { }));
        services.TryAddSingleton<ITemplateResolver, TemplateResolver>();
        return services;
    }
}
