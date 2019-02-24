using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore.Http {
	public static class SitemapMiddlewareExtensions {
		public static IApplicationBuilder UseSitemap<TMiddleware>(
			this IApplicationBuilder app)
			where TMiddleware : SitemapMiddlewareBase => app.UseMiddleware<TMiddleware>();
	}
}