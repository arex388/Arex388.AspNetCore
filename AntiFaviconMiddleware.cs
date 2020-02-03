using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Arex388.AspNetCore {
	public sealed class AntiFaviconMiddleware {
		private RequestDelegate Next { get; }

		public AntiFaviconMiddleware(
			RequestDelegate next) => Next = next ?? throw new ArgumentNullException(nameof(next));

		public async Task InvokeAsync(
			HttpContext context) {
			var request = context.Request;
			var response = context.Response;

			if (request.Path.Value == "/favicon.ico") {
				response.StatusCode = (int)HttpStatusCode.NotFound;

				return;
			}

			await Next(context);
		}
	}
}