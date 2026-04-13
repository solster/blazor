
// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components.Web;

public static class HtmlRendererExtensions
{
    private const String ModelParameterName = "Model";
    
    extension(HtmlRenderer renderer)
    {
        /// <summary>
        /// Renders the component identified by <paramref name="componentType"/> with the given
        /// <paramref name="model"/> to an HTML string.
        /// </summary>
        public Task<String> RenderAsync<TModel>(Type componentType, TModel model)
        {
            var parameters = new Dictionary<String, Object?>
            {
                [ModelParameterName] = model
            };

            return renderer.RenderAsync(componentType, parameters);
        }
        
        /// <summary>
        /// Renders the component identified by <paramref name="componentType"/> with the given
        /// <paramref name="parameters"/> dictionary to an HTML string.
        /// </summary>
        public Task<String> RenderAsync(Type componentType, Dictionary<String, Object?> parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            return RenderComponentAsync(renderer, componentType, ParameterView.FromDictionary(parameters));
        }

        /// <summary>
        /// Renders the component identified by <paramref name="componentType"/> with no parameters.
        /// </summary>
        public Task<String> RenderAsync(Type componentType)
        {
            return RenderComponentAsync(renderer, componentType, ParameterView.Empty);
        }
    }

    private static async Task<String> RenderComponentAsync(HtmlRenderer renderer, Type componentType, ParameterView parameters)
    {
        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            var output = await renderer.RenderComponentAsync(componentType, parameters);
            return output.ToHtmlString();
        });
    }
}



