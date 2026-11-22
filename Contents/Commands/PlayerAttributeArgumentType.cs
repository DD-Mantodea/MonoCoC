using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public sealed class PlayerAttributeArgumentType : ArgumentType<Identifier>
    {
        public static readonly string[] Attributes =
        [
            "coc:hp",
            "coc:lust",
            "coc:fatigue",
            "coc:corruption",
            "coc:level",
            "coc:xp",
            "coc:gems",
            "coc:day",
            "coc:hour"
        ];

        public override string UsageText => "<namespace:path>";

        public override Identifier ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            if (!text.Contains(':') || !Identifier.TryParse(text, out var identifier))
                throw new CommandSyntaxException("Commands.Error.ExpectedIdentifier", [text], reader.Cursor);

            if (!Attributes.Contains(identifier.ToString(), StringComparer.OrdinalIgnoreCase))
                throw new CommandSyntaxException("Commands.Error.UnknownAttribute", [text], reader.Cursor);

            return identifier;
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder) =>
            Attributes.Where(attribute => attribute.StartsWith(builder.Remaining, StringComparison.OrdinalIgnoreCase));
    }
}
