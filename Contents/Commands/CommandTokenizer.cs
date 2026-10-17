using System.Collections.Generic;
using System.Text;

namespace MonoCoC.Contents.Commands
{
    public readonly struct CommandToken
    {
        public CommandToken(string raw, string text)
        {
            Raw = raw;

            Text = text;
        }

        public readonly string Raw;

        public readonly string Text;
    }

    public static class CommandTokenizer
    {
        public const char Escape = '\\';

        public static List<CommandToken> Tokenize(string input)
        {
            var tokens = new List<CommandToken>();

            if (string.IsNullOrEmpty(input))
                return tokens;

            var index = 0;

            while (index < input.Length)
            {
                if (char.IsWhiteSpace(input[index]))
                {
                    index++;

                    continue;
                }

                tokens.Add(ReadToken(input, ref index));
            }

            return tokens;
        }

        public static CommandToken ReadToken(string input, ref int index)
        {
            var start = index;

            var builder = new StringBuilder();

            var quote = '\0';

            while (index < input.Length)
            {
                var character = input[index];

                if (character == Escape && index + 1 < input.Length)
                {
                    builder.Append(Unescape(input[index + 1]));

                    index += 2;

                    continue;
                }

                if (quote != '\0')
                {
                    if (character == quote)
                        quote = '\0';
                    else
                        builder.Append(character);

                    index++;

                    continue;
                }

                if (character is '"' or '\'')
                {
                    quote = character;

                    index++;

                    continue;
                }

                if (char.IsWhiteSpace(character))
                    break;

                builder.Append(character);

                index++;
            }

            return new CommandToken(input[start..index], builder.ToString());
        }

        public static string Unescape(char character) => character switch
        {
            'n' => "\n",

            'r' => "\r",

            't' => "\t",

            '0' => "\0",

            '\\' => "\\",

            '"' => "\"",

            '\'' => "'",

            _ => Escape + character.ToString()
        };
    }
}