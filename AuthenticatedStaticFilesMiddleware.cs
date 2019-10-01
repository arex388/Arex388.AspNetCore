//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Options;

//namespace Arex388.AspNetCore {
//	public sealed class AuthenticatedStaticFilesMiddleware {
//		private AuthenticatedStaticFileOptions Options { get; }
//		private RequestDelegate Next { get; }

//		public AuthenticatedStaticFilesMiddleware(
//			RequestDelegate next,
//			IOptions<AuthenticatedStaticFileOptions> options) {
//			Next = next ?? throw new ArgumentNullException(nameof(next));
//			Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
//		}

//		public async Task InvokeAsync(
//			HttpContext context) {
//			if (Options.Paths == null) {
//				await Next.Invoke(context);

//				return;
//			}

//			var path = context.Request.Path.Value;
//			var matches = Options.Paths.Any(
//				p => path.Contains(p));

//			if (!context.User.Identity.IsAuthenticated
//			    && matches) {
//				context.Response.StatusCode = 404;

//				return;
//			}

//			await Next.Invoke(context);
//		}
//	}
//}