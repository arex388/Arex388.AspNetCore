using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;

namespace Arex388.AspNetCore {
    public sealed class SimpleSlugifyParameterTransformer :
        IOutboundParameterTransformer {
        public string TransformOutbound(
            object? value) {
            if (value is not string @string) {
                return value?.ToString() ?? string.Empty;
            }

            var chars = new List<char>();

            foreach (var c in @string) {
                if (!char.IsUpper(c)) {
                    chars.Add(c);

                    continue;
                }

                chars.Add('-');
                chars.Add(c);
            }

            return new string(chars.ToArray()).Trim('-');
        }
    }
}