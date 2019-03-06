using System;
using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore {
	public static class HtmlMinifierMiddlewareExtensions {
		public static IApplicationBuilder UseHtmlMinifier(
			this IApplicationBuilder builder) {
			if (builder is null) {
				throw new ArgumentNullException(nameof(builder));
			}

			return builder.UseMiddleware<HtmlMinifierMiddleware>();
		}
	}
}