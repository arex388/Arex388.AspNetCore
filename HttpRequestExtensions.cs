using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Http {
    public static class HttpRequestExtensions {
        public static string? GetReferrer(
            this HttpRequest request) => request.Headers[HeaderNames.Referer].ToString();
    }
}