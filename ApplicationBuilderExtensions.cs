using Arex388.AspNetCore;

namespace Microsoft.AspNetCore.Builder {
    public static class ApplicationBuilderExtensions {
        public static IApplicationBuilder UseHtmlMinifier(
            this IApplicationBuilder builder) => builder.UseMiddleware<HtmlMinifierMiddleware>();

        public static IApplicationBuilder UsePreCompressedStaticFiles(
            this IApplicationBuilder builder) => builder.UseMiddleware<PreCompressedStaticFilesMiddleware>();

        public static IApplicationBuilder UseSitemap<TMiddleware>(
            this IApplicationBuilder builder)
            where TMiddleware : SitemapMiddlewareBase => builder.UseMiddleware<TMiddleware>();
    }
}