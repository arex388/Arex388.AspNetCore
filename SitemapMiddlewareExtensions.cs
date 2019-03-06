﻿using System;
using Microsoft.AspNetCore.Builder;

namespace Arex388.AspNetCore {
	public static class SitemapMiddlewareExtensions {
		public static IApplicationBuilder UseSitemap<TMiddleware>(
			this IApplicationBuilder builder)
			where TMiddleware : SitemapMiddlewareBase {
			if (builder is null) {
				throw new ArgumentNullException(nameof(builder));
			}

			return builder.UseMiddleware<TMiddleware>();
		}
	}
}