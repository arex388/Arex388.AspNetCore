using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Arex388.AspNetCore {
    public sealed class AntiFaviconMiddleware {
        private const string FaviconPath = "/favicon.ico";

        private readonly RequestDelegate _next;

        public AntiFaviconMiddleware(
            RequestDelegate next) => _next = next ?? throw new ArgumentNullException(nameof(next));

        public async Task InvokeAsync(
            HttpContext context) {
            var request = context.Request;
            var response = context.Response;

            if (request.Path.Value != FaviconPath) {
                await _next(context);

                return;
            }

            response.StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}