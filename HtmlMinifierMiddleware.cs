using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;

namespace Arex388.AspNetCore.Http {
	public sealed class HtmlMinifierMiddleware {
		private RequestDelegate Next { get; }

		public HtmlMinifierMiddleware(
			RequestDelegate next) => Next = next;

		public async Task InvokeAsync(
			HttpContext context) {
			var response = context.Response;
			var stream = response.Body;

			using (var memoryStream = new MemoryStream()) {
				response.Body = memoryStream;

				await Next(context);

				memoryStream.Seek(0, SeekOrigin.Begin);

				if (response.ContentType.Contains("text/html")) {
					var document = new HtmlDocument();

					document.Load(memoryStream);

					TrimWhitespace(document.DocumentNode);

					document.Save(stream);
				}

				await memoryStream.CopyToAsync(stream);

				response.Body = stream;
			}
		}

		private static void TrimWhitespace(
			HtmlNode document) {
			var textNodes = document.SelectNodesAsList("//text()").Where(
				n => string.IsNullOrWhiteSpace(n.InnerHtml));

			foreach (var textNode in textNodes) {
				textNode.Remove();
			}

			var commentNodes = document.SelectNodesAsList("//comment()").Where(
				n => n.InnerHtml != "<!DOCTYPE html>");

			foreach (var commentNode in commentNodes) {
				commentNode.Remove();
			}
		}
	}
}