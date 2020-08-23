using System;
using System.Linq;

namespace Arex388.AspNetCore {
    public static class TokenProvider {
        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static readonly Random Random = new Random();

        public static string Create(
            int length) {
            var characters = Enumerable.Repeat(Characters, length).Select(
                s => s[Random.Next(62)]).ToArray();

            return new string(characters);
        }

        public static string Create8() => Create(8);

        public static string Create16() => Create(16);

        public static string Create32() => Create(32);

        public static string Create64() => Create(64);

        public static string Create128() => Create(128);
    }
}