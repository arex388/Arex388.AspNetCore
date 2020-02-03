using Arex388.AspNetCore;
using System;

namespace Microsoft.AspNetCore.Builder {
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