
// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components.Web;

public static class HtmlRendererCssExtensions
{
    extension(HtmlRenderer renderer)
    {
        /// <summary>
        /// Renders the component identified by <paramref name="componentType"/> with the given <paramref name="model"/> to an HTML string and optionally inlines CSS.
        /// </summary>
        public async Task<String> RenderAsync<TModel>(Type componentType, TModel model, Boolean inlineCss)
        {
            var html = await renderer.RenderAsync(componentType, model);
            return inlineCss ? InlineCss(html) : html;
        }
        
        /// <summary>
        /// Renders the component identified by <paramref name="componentType"/> with the given <paramref name="parameters"/> dictionary to an HTML string and optionally inlines CSS.
        /// </summary>
        public async Task<String> RenderAsync(Type componentType, Dictionary<String, Object?> parameters, Boolean inlineCss)
        {
            var html = await renderer.RenderAsync(componentType, parameters);
            return inlineCss ? InlineCss(html) : html;
        }

        /// <summary>
        /// Renders the component identified by <paramref name="componentType"/> with no parameters and optionally inlines CSS.
        /// </summary>
        public async Task<String> RenderAsync(Type componentType, Boolean inlineCss)
        {
            var html = await renderer.RenderAsync(componentType);
            return inlineCss ? InlineCss(html) : html;
        }
    }

    private static String InlineCss(String html)
    {
        var result = new PreMailer.Net.PreMailer(html)
            .MoveCssInline(removeComments: true);
        
        return result.Html;
    }
}


