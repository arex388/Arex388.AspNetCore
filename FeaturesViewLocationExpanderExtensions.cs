using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace Arex388.AspNetCore {
	public static class FeaturesViewLocationExpanderExtensions {
		public static void AddFeatures(
			this IServiceCollection services) {
			services.Configure<RazorViewEngineOptions>(
				o => {
					o.ViewLocationExpanders.Clear();
					o.ViewLocationExpanders.Add(new FeaturesViewLocationExpander());
				});
		}
	}
}