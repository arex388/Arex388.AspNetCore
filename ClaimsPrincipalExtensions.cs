﻿using System;
using System.Security.Claims;

namespace Arex388.AspNetCore {
	public static class ClaimsPrincipalExtensions {
		public static int? GetUserId(
			this ClaimsPrincipal principal) {
			var value = principal.GetValue(ClaimTypes.NameIdentifier);

			if (string.IsNullOrEmpty(value)) {
				return null;
			}

			return Convert.ToInt32(value);
		}

		public static string GetValue(
			this ClaimsPrincipal principal,
			string claimType) {
			if (principal is null) {
				throw new ArgumentNullException(nameof(principal));
			}

			return principal.FindFirst(claimType)?.Value;
		}
	}
}