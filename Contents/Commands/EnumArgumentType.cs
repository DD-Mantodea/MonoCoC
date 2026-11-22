using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public sealed class EnumArgumentType<T> : ArgumentType<T> where T : struct, Enum
    {
        public override string UsageText => "<" + string.Join("|", Enum.GetNames<T>()) + ">";

        public override T ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            if (Enum.TryParse<T>(text, true, out var value) && Enum.IsDefined(value))
                return value;

            throw new CommandSyntaxException("Commands.Error.ExpectedEnum", [text], reader.Cursor);
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder) =>
            Enum.GetNames<T>().Where(name => name.StartsWith(builder.Remaining, StringComparison.OrdinalIgnoreCase));
    }
}
