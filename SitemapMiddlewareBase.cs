using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Arex388.AspNetCore {
	public abstract class SitemapMiddlewareBase {
		protected RequestDelegate Next { get; }

		protected SitemapMiddlewareBase(
			RequestDelegate next) => Next = next ?? throw new ArgumentNullException(nameof(next));

		public async Task InvokeAsync(
			HttpContext context,
			ISitemapServices services) {
			if (!context.Request.Path.Value.Equals("/sitemap.xml", StringComparison.OrdinalIgnoreCase)) {
				await Next(context);

				return;
			}

			var sitemap = await InvokeInternalAsync(services);

			var response = context.Response;
			var stream = response.Body;

			response.ContentType = "application/xml";
			response.StatusCode = 200;

			await using var memoryStream = new MemoryStream();

			var bytes = Encoding.UTF8.GetBytes(sitemap);

			await memoryStream.WriteAsync(bytes, 0, bytes.Length);

			memoryStream.Seek(0, SeekOrigin.Begin);

			await memoryStream.CopyToAsync(stream, bytes.Length);
		}

		protected abstract Task<string> InvokeInternalAsync(
			ISitemapServices services);
	}
}