using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public static class CommandCompletion
    {
        public static IEnumerable<string> Filter(IEnumerable<string> values, string prefix)
        {
            if (string.IsNullOrEmpty(prefix))
                return values;

            return values
                .Where(value => value.Contains(prefix, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(value => value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .ThenBy(value => value, StringComparer.OrdinalIgnoreCase);
        }

        public static bool Matches(string value, string prefix) =>

            string.IsNullOrEmpty(prefix) || value.Contains(prefix, StringComparison.OrdinalIgnoreCase);
    }
}
