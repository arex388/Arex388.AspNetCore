using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Arex388.AspNetCore {
	public abstract class OneOffMiddlewareBase {
		protected static bool HasProcessed { get; set; }

		protected RequestDelegate Next { get; }

		protected OneOffMiddlewareBase(
			RequestDelegate next) => Next = next ?? throw new ArgumentNullException(nameof(next));

		public async Task InvokeAsync(
			HttpContext context,
			IOneOffServices services) {
			if (HasProcessed) {
				await Next(context);

				return;
			}

			await InvokeInternalAsync(services);

			HasProcessed = true;

			await Next(context);
		}

		protected abstract Task InvokeInternalAsync(
			IOneOffServices services);
	}
}