using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Arex388.AspNetCore {
	public static class SimpleSlugifyParameterTransformerExtensions {
		public static IServiceCollection ConfigureSimpleSlugifyParameterTransformer(
			this IServiceCollection services) {
			return services.Configure<RouteOptions>(
				o => {
					if (o.ConstraintMap.ContainsKey("slugify")) {
						throw new InvalidOperationException("An IOutboundParameterTransformer with a key of 'slugify' is already registered.");
					}

					o.ConstraintMap["slugify"] = typeof(SimpleSlugifyParameterTransformer);
				});
		}
	}
}