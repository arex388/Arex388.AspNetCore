using System.Collections.Generic;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc.ModelBinding {
    public static class ModelStateDictionaryExtensions {
        public static IDictionary<string, string> GetDictionary(
            this ModelStateDictionary dictionary) => dictionary.Where(
            _ => _.Value.Errors.Count > 0).ToDictionary(
            _ => _.Key,
            _ => _.Value.Errors.Select(
                e => e.ErrorMessage).StringJoin("; "));
    }
}