using Microsoft.AspNetCore.Http;

namespace Arex388.AspNetCore {
	public class IdentityProvider {
		protected IHttpContextAccessor Accessor { get; }

		public IdentityProvider(
			IHttpContextAccessor accessor) => Accessor = accessor;

		public virtual bool IsAuthenticated => Accessor.HttpContext.User.Identity.IsAuthenticated;
		public virtual int? UserId => Accessor.HttpContext.User.GetUserId();
	}
}