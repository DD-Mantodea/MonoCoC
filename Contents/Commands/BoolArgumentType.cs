using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class BoolArgumentType : ArgumentType<bool>
    {
        public override string UsageText => "<true|false>";

        public override bool ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            if (bool.TryParse(text, out var value))
                return value;

            if (text is "1" or "yes" or "on")
                return true;

            if (text is "0" or "no" or "off")
                return false;

            throw new CommandSyntaxException("Commands.Error.ExpectedBoolean", [text], reader.Cursor);
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder)
        {
            if ("true".StartsWith(builder.Remaining, System.StringComparison.OrdinalIgnoreCase))
                builder.Suggest("true");

            if ("false".StartsWith(builder.Remaining, System.StringComparison.OrdinalIgnoreCase))
                builder.Suggest("false");

            return [];
        }
    }
}
