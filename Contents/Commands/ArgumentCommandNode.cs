using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class ArgumentCommandNode : CommandNode
    {
        public ArgumentType Argument { get; }

        public ArgumentCommandNode(string name, ArgumentType argument)
        {
            Name = name;

            Argument = argument;
        }

        public override string UsageText => Argument.UsageText;

        public override ParsedArgument? Consume(StringReader reader, CommandContext context,
            Dictionary<string, object> values)
        {
            reader.SkipWhitespace();

            if (!reader.CanRead)
                throw new CommandSyntaxException("Commands.Error.ExpectedArgument", [UsageText], reader.Cursor);

            var start = reader.Cursor;

            var value = Argument.Parse(reader);

            values[Name] = value;

            return new ParsedArgument(Name, StringRange.Between(start, reader.Cursor), value);
        }
    }
}
