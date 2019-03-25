using System;
using System.Linq;

namespace Arex388.AspNetCore {
	public sealed class TokenProvider {
		private const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";

		private Random Random { get; }

		public TokenProvider(
			Random random) => Random = random;

		public string Create(
			int length) {
			var characters = Enumerable.Repeat(Characters, length).Select(
				s => s[Random.Next(62)]).ToArray();

			return new string(characters);
		}
	}
}