using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arex388.AspNetCore {
    public sealed class HtmlMinifierMiddleware {
        private static readonly string[] ContentTypes = {
            "text/html; charset=utf-8",
            "text/xml"
        };

        private readonly RequestDelegate _next;

        public HtmlMinifierMiddleware(
            RequestDelegate next) => _next = next ?? throw new ArgumentNullException(nameof(next));

        public async Task InvokeAsync(
            HttpContext context) {
            var response = context.Response;
            var stream = response.Body;

            try {
                await using var memoryStream = new MemoryStream();

                response.Body = memoryStream;

                await _next(context);

                memoryStream.Seek(0, SeekOrigin.Begin);

                if (!ContentTypes.Contains(response.ContentType)) {
                    await memoryStream.CopyToAsync(stream);

                    return;
                }

                var document = new HtmlDocument();

                document.Load(memoryStream, Encoding.UTF8);
                document.DocumentNode.TrimWhitespace();

                var html = document.DocumentNode.OuterHtml;
                var htmlBytes = Encoding.UTF8.GetBytes(html);

                await stream.WriteAsync(htmlBytes);
            } finally {
                response.Body = stream;
            }
        }
    }
}