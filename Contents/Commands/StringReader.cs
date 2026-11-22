using System;
using System.Globalization;
using System.Text;

namespace MonoCoC.Contents.Commands
{
    public sealed class StringReader(string input)
    {
        public string Input { get; } = input;

        public int Cursor { get; set; }

        public bool CanRead => Cursor < Input.Length;

        public string Remaining => Input[Cursor..];

        public StringReader Clone() => new(Input) { Cursor = Cursor };

        public char Peek() => CanRead ? Input[Cursor] : '\0';

        public char Read()
        {
            if (!CanRead)
                throw new CommandSyntaxException("Commands.Error.UnexpectedEnd", Array.Empty<object>(), Cursor);

            return Input[Cursor++];
        }

        public bool ReadIf(char expected)
        {
            if (Peek() != expected)
                return false;

            Cursor++;

            return true;
        }

        public void Expect(char expected)
        {
            if (Peek() != expected)
                throw new CommandSyntaxException("Commands.Error.ExpectedCharacter", [expected.ToString()], Cursor);

            Cursor++;
        }

        public void SkipWhitespace()
        {
            while (CanRead && char.IsWhiteSpace(Peek()))
                Cursor++;
        }

        public string ReadUnquoted()
        {
            var start = Cursor;

            while (CanRead && !char.IsWhiteSpace(Peek()))
                Cursor++;

            return Input[start..Cursor];
        }

        public string ReadQuoted()
        {
            var quote = Read();

            var builder = new StringBuilder();

            while (CanRead && Peek() != quote)
                builder.Append(Read());

            if (!CanRead)
                throw new CommandSyntaxException("Commands.Error.UnclosedQuote", Array.Empty<object>(), Cursor);

            Cursor++;

            return builder.ToString();
        }

        public string ReadString()
        {
            SkipWhitespace();

            if (!CanRead)
                throw new CommandSyntaxException("Commands.Error.ExpectedArgument", Array.Empty<object>(), Cursor);

            return Peek() is '"' or '\'' ? ReadQuoted() : ReadUnquoted();
        }

        public int ReadInt()
        {
            var text = ReadString();

            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                throw new CommandSyntaxException("Commands.Error.ExpectedInteger", [text], Cursor);

            return value;
        }

        public float ReadFloat()
        {
            var text = ReadString();

            if (!float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                throw new CommandSyntaxException("Commands.Error.ExpectedNumber", [text], Cursor);

            return value;
        }

        public float ReadFloatOr(string fallback)
        {
            var text = ReadString();

            return float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : float.Parse(fallback, CultureInfo.InvariantCulture);
        }
    }
}
