using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class LiteralCommandNode : CommandNode
    {
        public string Literal { get; }

        public LiteralCommandNode(string literal)
        {
            Literal = literal;

            Name = literal;
        }

        public override string UsageText => Literal;

        public override ParsedArgument? Consume(StringReader reader, CommandContext context,
            Dictionary<string, object> values)
        {
            reader.SkipWhitespace();

            if (!reader.CanRead)
                throw new CommandSyntaxException("Commands.Error.ExpectedLiteral", [Literal], reader.Cursor);

            var word = reader.ReadString();

            if (!string.Equals(word, Literal, StringComparison.OrdinalIgnoreCase))
                throw new CommandSyntaxException("Commands.Error.ExpectedLiteral", [Literal], reader.Cursor);

            return null;
        }
    }
}
