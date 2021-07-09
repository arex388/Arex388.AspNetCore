using System.Diagnostics.CodeAnalysis;

namespace System {
    public static class StringExtensions {
        public static bool HasValue(
            [NotNullWhen(true)]
            this string? value) => !string.IsNullOrEmpty(value);
    }
}