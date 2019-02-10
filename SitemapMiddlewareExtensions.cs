using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore.Http {
	public static class SitemapMiddlewareExtensions {
		public static IApplicationBuilder UseSitemapMiddleware<TMiddleware>(
			this IApplicationBuilder app)
			where TMiddleware : SitemapMiddlewareBase {
			return app.UseMiddleware<TMiddleware>();
		}
	}
}