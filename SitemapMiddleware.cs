using Microsoft.AspNetCore.Http;

namespace Arex388.AspNetCore.Http {
	public abstract class SitemapMiddlewareBase {
		protected RequestDelegate Next { get; }

		protected SitemapMiddlewareBase(
			RequestDelegate next) => Next = next;
	}
}