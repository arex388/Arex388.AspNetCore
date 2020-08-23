using Arex388.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddArex388(
            this IServiceCollection services,
            Action<Arex388Options> configurer) {
            if (services is null) {
                throw new ArgumentNullException(nameof(services));
            }

            if (configurer is null) {
                throw new ArgumentNullException(nameof(configurer));
            }

            var options = new Arex388Options();

            configurer(options);

            ConfigureFeatures(services, options);
            ConfigureIdentityProvider(services, options);
            ConfigureSimpleSlugifyParameterTransformer(services, options);

            return services;
        }

        private static void ConfigureFeatures(
            IServiceCollection services,
            Arex388Options options) {
            if (!options.UseFeatures) {
                return;
            }

            services.Configure<RazorViewEngineOptions>(
                o => {
                    o.ViewLocationExpanders.Clear();
                    o.ViewLocationExpanders.Add(new FeaturesViewLocationExpander());
                });
        }

        private static void ConfigureIdentityProvider(
            IServiceCollection services,
            Arex388Options options) {
            if (!options.UseIdentityProvider) {
                return;
            }

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IdentityProvider>();
        }

        private static void ConfigureSimpleSlugifyParameterTransformer(
            IServiceCollection services,
            Arex388Options options) {
            if (!options.UseSimpleSlugifyParameterTransformer) {
                return;
            }

            services.Configure<RouteOptions>(
                o => {
                    if (o.ConstraintMap.ContainsKey("slugify")) {
                        throw new InvalidOperationException("An IOutboundParameterTransformer with a key of 'slugify' is already registered.");
                    }

                    o.ConstraintMap["slugify"] = typeof(SimpleSlugifyParameterTransformer);
                });
        }
    }
}