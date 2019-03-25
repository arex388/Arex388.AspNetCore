using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Arex388.AspNetCore {
	public static class Arex388ServiceCollectionExtensions {
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

			if (options.UseFeatures) {
				services.Configure<RazorViewEngineOptions>(
					o => {
						o.ViewLocationExpanders.Clear();
						o.ViewLocationExpanders.Add(new FeaturesViewLocationExpander());
					});
			}

			if (options.UseIdentityProvider) {
				services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
				services.TryAddScoped<IdentityProvider>();
			}

			if (options.UseSimpleSlugifyParameterTransformer) {
				services.Configure<RouteOptions>(
					o => {
						if (o.ConstraintMap.ContainsKey("slugify")) {
							throw new InvalidOperationException("An IOutboundParameterTransformer with a key of 'slugify' is already registered.");
						}

						o.ConstraintMap["slugify"] = typeof(SimpleSlugifyParameterTransformer);
					});
			}

			if (options.UseTokenProvider) {
				services.TryAddScoped<Random>();
				services.TryAddScoped<TokenProvider>();
			}
			
			return services;
		}
	}
}