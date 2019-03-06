using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace Arex388.AspNetCore {
	public static class AuthenticatedStaticFilesMiddlewareExtensions {
		public static IApplicationBuilder UseAuthenticatedStaticFiles(
			this IApplicationBuilder builder) {
			if (builder is null) {
				throw new ArgumentNullException(nameof(builder));
			}

			return builder.UseMiddleware<AuthenticatedStaticFilesMiddleware>();
		}

		public static IApplicationBuilder UseAuthenticatedStaticFiles(
			this IApplicationBuilder builder,
			AuthenticatedStaticFileOptions options) {
			if (builder is null) {
				throw new ArgumentNullException(nameof(builder));
			}

			if (options is null) {
				throw new ArgumentNullException(nameof(options));
			}

			return builder.UseMiddleware<AuthenticatedStaticFilesMiddleware>(Options.Create(options));
		}
	}
}