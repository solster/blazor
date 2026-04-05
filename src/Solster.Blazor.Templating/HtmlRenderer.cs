using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging.Abstractions;
using Renderer = Microsoft.AspNetCore.Components.Web.HtmlRenderer;

namespace Solster.Blazor.Templating;

public sealed class HtmlRenderer(IServiceProvider serviceProvider) : IHtmlRenderer
{
    public async Task<String> RenderAsync<TComponent, TModel>(TModel model, bool inlineCss = false)
        where TComponent : IHtmlTemplate<TModel>
    {
        await using var renderer = new Renderer(serviceProvider, NullLoggerFactory.Instance);

        var parameters = ParameterView.FromDictionary(new Dictionary<String, Object?>
        {
            [nameof(IHtmlTemplate<TModel>.Model)] = model
        });

        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await renderer.RenderComponentAsync<TComponent>(parameters);
            return output.ToHtmlString();
        });
    }

    public async Task<String> RenderAsync<TComponent>(bool inlineCss = false)
        where TComponent : IComponent
    {
        await using var renderer = new Renderer(serviceProvider, NullLoggerFactory.Instance);

        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await renderer.RenderComponentAsync<TComponent>(ParameterView.Empty);
            return output.ToHtmlString();
        });
    }
}
