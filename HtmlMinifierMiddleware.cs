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

			try {
				using (var memoryStream = new MemoryStream()) {
					response.Body = memoryStream;

					await Next(context);

					memoryStream.Seek(0, SeekOrigin.Begin);

					if (response.StatusCode == 200
						&& !string.IsNullOrEmpty(response.ContentType)
						&& response.ContentType.Contains("text/html")) {
						var document = new HtmlDocument();

						document.Load(memoryStream, Encoding.UTF8);
						document.DocumentNode.TrimWhitespace();

						var html = document.DocumentNode.OuterHtml;
						var htmlBytes = Encoding.UTF8.GetBytes(html);

						await stream.WriteAsync(htmlBytes);
					}
				}
			} finally {
				request.Body = stream;
			}
		}
	}
}