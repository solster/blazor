
// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components.Web;

public static class HtmlRendererOfTCssExtensions
{
    extension(HtmlRenderer renderer)
    {
        /// <summary>
        /// Renders <typeparamref name="TComponent"/> with the given <paramref name="model"/> to an HTML string and optionally inlines CSS.
        /// </summary>
        public Task<String> RenderAsync<TComponent, TModel>(TModel model, Boolean inlineCss)
            => renderer.RenderAsync(typeof(TComponent), model, inlineCss);

        /// <summary>
        /// Renders <typeparamref name="TComponent"/> with the given <paramref name="parameters"/> dictionary to an HTML string and optionally inlines CSS.
        /// </summary>
        public Task<String> RenderAsync<TComponent>(Dictionary<String, Object?> parameters, Boolean inlineCss)
            => renderer.RenderAsync(typeof(TComponent), parameters, inlineCss);

        /// <summary>
        /// Renders <typeparamref name="TComponent"/> with no parameters and optionally inlines CSS.
        /// </summary>
        public Task<String> RenderAsync<TComponent>(Boolean inlineCss)
            => renderer.RenderAsync(typeof(TComponent), inlineCss);
    }
}


