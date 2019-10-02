using System;
using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore {
	public static class AntiFaviconMiddlewareExtensions {
		public static IApplicationBuilder UseAntiFavicon(
			this IApplicationBuilder builder) {
			if (builder is null) {
				throw new ArgumentNullException(nameof(builder));
			}

			return builder.UseMiddleware<AntiFaviconMiddleware>();
		}
	}
}