
// ReSharper disable once CheckNamespace
namespace Microsoft.AspNetCore.Components.Web;

public static class HtmlRendererOfTExtensions
{
    extension(HtmlRenderer renderer)
    {
        /// <summary>
        /// Renders <typeparamref name="TComponent"/> with the given <paramref name="model"/> to an HTML string.
        /// </summary>
        public Task<String> RenderAsync<TComponent, TModel>(TModel model)
            => renderer.RenderAsync(typeof(TComponent), model);

        /// <summary>
        /// Renders <typeparamref name="TComponent"/> with the given <paramref name="parameters"/> dictionary to an HTML string.
        /// </summary>
        public Task<String> RenderAsync<TComponent>(Dictionary<String, Object?> parameters)
            where TComponent : IComponent
            => renderer.RenderAsync(typeof(TComponent), parameters);

        /// <summary>
        /// Renders <typeparamref name="TComponent"/> with no parameters.
        /// </summary>
        public Task<String> RenderAsync<TComponent>()
            where TComponent : IComponent
            => renderer.RenderAsync(typeof(TComponent));
    }
}
