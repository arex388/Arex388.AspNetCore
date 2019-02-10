using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore.Http {
	public static class HtmlMinifierMiddlewareExtensions {
		public static IApplicationBuilder UseHtmlMinifierMiddleware(
			this IApplicationBuilder builder) {
			return builder.UseMiddleware<HtmlMinifierMiddleware>();
		}
	}
}