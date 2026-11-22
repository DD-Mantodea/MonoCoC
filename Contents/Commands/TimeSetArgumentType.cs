using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class TimeSetArgumentType : ArgumentType<int>
    {
        private static readonly Dictionary<string, int> Named = new(StringComparer.OrdinalIgnoreCase)
        {
            ["sunrise"] = 5,
            ["day"] = 6,
            ["noon"] = 12,
            ["sunset"] = 18,
            ["night"] = 20,
            ["midnight"] = 0
        };

        public override string UsageText => "<day|sunrise|noon|sunset|night|midnight|0..23>";

        public override int ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            if (Named.TryGetValue(text, out var named))
                return named;

            if (int.TryParse(text, out var hour) && hour is >= 0 and <= 23)
                return hour;

            throw new CommandSyntaxException("Commands.Error.ExpectedTime", [text], reader.Cursor);
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder)
        {
            foreach (var name in Named.Keys)
                if (name.StartsWith(builder.Remaining, StringComparison.OrdinalIgnoreCase))
                    builder.Suggest(name);

            return [];
        }
    }
}
