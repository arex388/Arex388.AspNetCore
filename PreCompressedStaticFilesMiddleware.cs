using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Arex388.AspNetCore {
    public class PreCompressedStaticFilesMiddleware {
        private const string ContentTypeCss = "text/css";
        private const string ContentTypeJs = "text/javascript";
        private const string EncodingBr = "br";
        private const string EncodingGzip = "gzip";
        private const string ExtensionCss = ".css";
        private const string ExtensionJs = ".js";

        private readonly RequestDelegate _next;

        public PreCompressedStaticFilesMiddleware(
            RequestDelegate next) => _next = next ?? throw new ArgumentNullException(nameof(next));

        public async Task InvokeAsync(
            HttpContext httpContext,
            IWebHostEnvironment environment) {
            var request = httpContext.Request;
            var path = request.Path.Value;
            var extensionType = GetExtensionType(path);

            if (extensionType == ExtensionType.Unknown) {
                await _next(httpContext).ConfigureAwait(false);

                return;
            }

            var acceptEncoding = request.Headers[HeaderNames.AcceptEncoding].ToString();
            var encodingType = GetEncodingType(acceptEncoding);

            if (encodingType == EncodingType.Unknown) {
                await _next(httpContext).ConfigureAwait(false);

                return;
            }

            //  If CSS
            //  - If Accepts-Encoding has br, then serve .css.br
            //  - If Accepts-Encoding has gzip but doesn't have br, then serve .css.gzip
            //  - Else serve .min.css

            //  If JS
            //  - If Accepts-Encoding has br, then serve .js.br
            //  - If Accepts-Encoding has gzip but doesn't have br, then serve .js.gzip
            //  - Else serve .min.js

            var response = httpContext.Response;

            response.Headers[HeaderNames.ContentType] = GetContentType(extensionType);
            response.Headers[HeaderNames.ContentEncoding] = encodingType;

            if (encodingType == EncodingType.Br) {
                var brPath = $"{environment.WebRootPath}\\{path}.br";

                if (!File.Exists(brPath)) {
                    throw new FileNotFoundException($"Could not find a .br file at: {brPath}");
                }

                var brBytes = await File.ReadAllBytesAsync(brPath).ConfigureAwait(false);

                await response.Body.WriteAsync(brBytes, 0, brBytes.Length).ConfigureAwait(false);

                return;
            }

            var gzipPath = $"{environment.WebRootPath}\\{path}.gzip";

            if (!File.Exists(gzipPath)) {
                throw new FileNotFoundException($"Could not find a .gzip file at: {gzipPath}");
            }

            var gzipBytes = await File.ReadAllBytesAsync(gzipPath).ConfigureAwait(false);

            await response.Body.WriteAsync(gzipBytes, 0, gzipBytes.Length).ConfigureAwait(false);
        }

        //  ========================================================================
        //  Utilities
        //  ========================================================================

        private static string GetContentType(
            ExtensionType extensionType) => extensionType switch
            {
                ExtensionType.Css => ContentTypeCss,
                ExtensionType.Js => ContentTypeJs,
                _ => throw new NotImplementedException()
            };

        private static string GetEncodingType(
            string acceptEncoding) {
            var isBr = acceptEncoding.Contains(EncodingBr, StringComparison.InvariantCulture);

            if (isBr) {
                return EncodingType.Br;
            }

            var isGzip = acceptEncoding.Contains(EncodingGzip, StringComparison.InvariantCulture);

            if (isGzip) {
                return EncodingType.Gzip;
            }

            return EncodingType.Unknown;
        }

        private static ExtensionType GetExtensionType(
            string path) {
            var isCss = path.EndsWith(ExtensionCss, StringComparison.InvariantCulture);

            if (isCss) {
                return ExtensionType.Css;
            }

            var isJs = path.EndsWith(ExtensionJs, StringComparison.InvariantCulture);

            if (isJs) {
                return ExtensionType.Js;
            }

            return ExtensionType.Unknown;
        }

        private static class EncodingType {
            public const string Br = "br";
            public const string Gzip = "gzip";
            public static readonly string Unknown = string.Empty;
        }

        private enum ExtensionType :
            byte {
            Unknown,
            Css,
            Js
        }
    }
}
