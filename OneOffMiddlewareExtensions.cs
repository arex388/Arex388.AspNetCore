using System;
using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore {
	public static class OneOffMiddlewareExtensions {
		public static IApplicationBuilder UseOneOff<TMiddleware>(
			this IApplicationBuilder builder)
			where TMiddleware : OneOffMiddlewareBase {
			if (builder is null) {
				throw new ArgumentNullException(nameof(builder));
			}

			return builder.UseMiddleware<TMiddleware>();
		}
	}
}