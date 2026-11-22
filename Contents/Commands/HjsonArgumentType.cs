using Hjson;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class HjsonArgumentType : ArgumentType<JsonObject>
    {
        public override string UsageText => "<hjson>";

        public override JsonObject ParseValue(StringReader reader)
        {
            reader.SkipWhitespace();

            if (reader.Peek() != '{')
                throw new CommandSyntaxException("Commands.Error.ExpectedCharacter", ["{"], reader.Cursor);

            var start = reader.Cursor;

            var depth = 0;

            var inString = false;

            var escape = false;

            while (reader.CanRead)
            {
                var character = reader.Read();

                if (inString)
                {
                    if (escape)
                        escape = false;
                    else if (character == '\\')
                        escape = true;
                    else if (character == '"')
                        inString = false;

                    continue;
                }

                switch (character)
                {
                    case '"':
                        inString = true;

                        break;
                    case '{':
                        depth++;

                        break;
                    case '}':
                        depth--;

                        if (depth == 0)
                        {
                            var text = reader.Input[start..reader.Cursor];

                            try
                            {
                                if (HjsonParser.Parse(text) is JsonObject value)
                                    return value;
                            }
                            catch (Exception exception)
                            {
                                throw new CommandSyntaxException("Commands.Error.InvalidHjson",
                                    [exception.Message], start);
                            }

                            throw new CommandSyntaxException("Commands.Error.InvalidHjson",
                                ["not an object"], start);
                        }

                        break;
                }
            }

            throw new CommandSyntaxException("Commands.Error.ExpectedCharacter", ["}"], start);
        }

        public override IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder)
        {
            if (!builder.Remaining.StartsWith('{'))
                return ["{"];

            return [];
        }
    }
}
