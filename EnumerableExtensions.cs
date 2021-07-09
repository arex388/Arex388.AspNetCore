namespace System.Collections.Generic {
    public static class EnumerableExtensions {
        public static string StringJoin<T>(
            this IEnumerable<T> values,
            string? separator = null) => string.Join(separator, values);
    }
}