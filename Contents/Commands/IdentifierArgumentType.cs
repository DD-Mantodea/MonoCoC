using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public sealed class IdentifierArgumentType : ArgumentType<Identifier>
    {
        private readonly Func<IEnumerable<string>>? _suggestions;

        private readonly Func<Identifier, bool>? _exists;

        public IdentifierArgumentType() { }

        public IdentifierArgumentType(Func<IEnumerable<string>>? suggestions = null,
            Func<Identifier, bool>? exists = null)
        {
            _suggestions = suggestions;

            _exists = exists;
        }

        public override string UsageText => "<namespace:path>";

        public override Identifier ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            if (!text.Contains(':') || !Identifier.TryParse(text, out var identifier))
                throw new CommandSyntaxException("Commands.Error.ExpectedIdentifier", [text], reader.Cursor);

            if (_exists is not null && !_exists(identifier))
                throw new CommandSyntaxException("Commands.Error.UnknownIdentifier", [text], reader.Cursor);

            return identifier;
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder) =>
            _suggestions?.Invoke().Where(value =>
                value.StartsWith(builder.Remaining, StringComparison.OrdinalIgnoreCase)) ?? [];
    }
}
