using System;
using System.Globalization;

namespace MonoCoC.Contents.Commands
{
    public sealed class TimeAddArgumentType : ArgumentType<int>
    {
        public override string UsageText => "<duration[hd]>";

        public override int ParseValue(StringReader reader)
        {
            var text = reader.ReadString();

            var multiplier = 1;

            if (text.EndsWith('h'))
            {
                text = text[..^1];
            }
            else if (text.EndsWith('d'))
            {
                text = text[..^1];

                multiplier = 24;
            }

            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) || value < 0)
                throw new CommandSyntaxException("Commands.Error.ExpectedDuration", [text], reader.Cursor);

            return value * multiplier;
        }
    }
}
