using System;
using Microsoft.AspNetCore.Http;

namespace Arex388.AspNetCore {
	public abstract class SitemapMiddlewareBase {
		protected RequestDelegate Next { get; }

		protected SitemapMiddlewareBase(
			RequestDelegate next) {
			Next = next ?? throw new ArgumentNullException(nameof(next));
		}
	}
}