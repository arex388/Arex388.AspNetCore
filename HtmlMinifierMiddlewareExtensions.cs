using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore.Http {
	public static class HtmlMinifierMiddlewareExtensions {
		public static IApplicationBuilder UseHtmlMinifier(
			this IApplicationBuilder builder) => builder.UseMiddleware<HtmlMinifierMiddleware>();
	}
}