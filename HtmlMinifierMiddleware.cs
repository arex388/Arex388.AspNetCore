using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;

namespace Arex388.AspNetCore {
	public sealed class HtmlMinifierMiddleware {
		private RequestDelegate Next { get; }

		public HtmlMinifierMiddleware(
			RequestDelegate next) => Next = next ?? throw new ArgumentNullException(nameof(next));

		public async Task InvokeAsync(
			HttpContext context) {
			var request = context.Request;

			if (request.Path.Value == "/favicon.ico") {
				await Next(context);

				return;
			}

			var response = context.Response;
			var stream = response.Body;

			using (var memoryStream = new MemoryStream()) {
				response.Body = memoryStream;

				try {
					await Next(context);
				} catch {
					response.Body = stream;

					throw;
				}

				memoryStream.Seek(0, SeekOrigin.Begin);

				if (response.StatusCode == 200
					&& !string.IsNullOrEmpty(response.ContentType)
					&& response.ContentType.Contains("text/html")) {
					var document = new HtmlDocument();

					document.Load(memoryStream, Encoding.UTF8);
					document.DocumentNode.TrimWhitespace();
					document.Save(stream);
				}

				await memoryStream.CopyToAsync(stream);

				response.Body = stream;
			}
		}
	}
}