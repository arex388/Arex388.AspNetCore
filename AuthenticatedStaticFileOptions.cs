using System.Collections.Generic;

namespace Arex388.AspNetCore {
	public sealed class AuthenticatedStaticFileOptions {
		public IEnumerable<string> Paths { get; set; }
	}
}