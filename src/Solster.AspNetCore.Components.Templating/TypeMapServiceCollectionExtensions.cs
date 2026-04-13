using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Solster.AspNetCore.Components.Templating;

public static class TypeMapServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Registers a default (non-keyed) type map resolver.
        /// </summary>
        public IServiceCollection AddTypeMapResolver(Action<TypeMapResolverOptions> configureOptions)
        {
            services.AddOptions<TypeMapResolverOptions>().Configure(configureOptions);
            services.TryAddSingleton<ITypeMapResolver>(sp =>
                new TypeMapResolver(sp.GetRequiredService<IOptions<TypeMapResolverOptions>>()));
            return services;
        }

        /// <summary>
        /// Registers a default (non-keyed) type map resolver using all types from <paramref name="assembly"/>.
        /// </summary>
        public IServiceCollection AddTypeMapResolver(Assembly assembly)
            => services.AddTypeMapResolver(options => options.TypeProviders.Add(new AssemblyTypeProvider(assembly)));

        /// <summary>
        /// Registers a keyed type map resolver.
        /// </summary>
        public IServiceCollection AddKeyedTypeMapResolver(Object serviceKey, Action<TypeMapResolverOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(serviceKey);

            var optionsName = GetOptionsName(serviceKey);
            services.AddOptions<TypeMapResolverOptions>(optionsName).Configure(configureOptions);
            services.AddKeyedSingleton<ITypeMapResolver>(serviceKey, (sp, key) =>
            {
                var monitor = sp.GetRequiredService<IOptionsMonitor<TypeMapResolverOptions>>();
                return new TypeMapResolver(monitor.Get(GetOptionsName(key ?? serviceKey)));
            });
            return services;
        }

        /// <summary>
        /// Registers a keyed type map resolver using all types from <paramref name="assembly"/>.
        /// </summary>
        public IServiceCollection AddKeyedTypeMapResolver(Object serviceKey, Assembly assembly)
            => services.AddKeyedTypeMapResolver(serviceKey,
                options => options.TypeProviders.Add(new AssemblyTypeProvider(assembly)));
    }

    private static String GetOptionsName(Object serviceKey)
        => $"TypeMapResolver::{serviceKey}";
}

