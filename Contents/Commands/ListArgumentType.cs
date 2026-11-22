using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class ListArgumentType<T> : ArgumentType<List<object>> where T : ArgumentType, new()
    {
        private readonly T _element = new();

        public ListArgumentType() { }

        public ListArgumentType(T element) => _element = element;

        public override string UsageText => "[<" + _element.UsageText + ">,...]";

        public override List<object> ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            if (!text.StartsWith('[') || !text.EndsWith(']'))
                throw new CommandSyntaxException("Commands.Error.ExpectedCharacter", ["["], reader.Cursor);

            var body = text[1..^1];

            var result = new List<object>();

            if (body.Length == 0)
                return result;

            foreach (var part in body.Split(','))
            {
                var elementReader = new StringReader(part.Trim());

                try
                {
                    result.Add(_element.Parse(elementReader));
                }
                catch (CommandSyntaxException exception)
                {
                    throw new CommandSyntaxException(exception.Key, exception.Args, reader.Cursor);
                }

                elementReader.SkipWhitespace();

                if (elementReader.CanRead)
                    throw new CommandSyntaxException("Commands.Error.InvalidListElement", [part], reader.Cursor);
            }

            return result;
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder)
        {
            if (!builder.Remaining.StartsWith('['))
                return ["["];

            return [];
        }
    }
}
