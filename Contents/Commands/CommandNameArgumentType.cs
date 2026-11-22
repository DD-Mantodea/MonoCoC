using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public sealed class CommandNameArgumentType : ArgumentType<string>
    {
        public override string UsageText => "<command>";

        public override string ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            if (CommandRegistry.Names.Any(name => string.Equals(name, text, StringComparison.OrdinalIgnoreCase)))
                return text;

            throw new CommandSyntaxException("Commands.Error.Unknown", [text], reader.Cursor);
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder) =>
            CommandRegistry.Names.Where(name => name.StartsWith(builder.Remaining, StringComparison.OrdinalIgnoreCase));
    }
}
