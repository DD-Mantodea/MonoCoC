namespace MonoCoC.Contents.Commands
{
    public sealed class IntArgumentType : ArgumentType<int>
    {
        private readonly int? _min;

        private readonly int? _max;

        public IntArgumentType(int? min = null, int? max = null)
        {
            _min = min;

            _max = max;
        }

        public override string UsageText => "<integer>";

        public override int ParseValue(StringReader reader)
        {
            var value = reader.ReadInt();

            if (_min is { } min && value < min || _max is { } max && value > max)
                throw new CommandSyntaxException("Commands.Error.OutOfRange", [value.ToString(), _min?.ToString() ?? "", _max?.ToString() ?? ""], reader.Cursor);

            return value;
        }
    }
}
