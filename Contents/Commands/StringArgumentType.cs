using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class StringArgumentType : ArgumentType<string>
    {
        public override string UsageText => "<string>";

        public override string ParseValue(StringReader reader) => reader.ReadString();

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder) => [];
    }
}
