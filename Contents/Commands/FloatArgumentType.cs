namespace MonoCoC.Contents.Commands
{
    public sealed class FloatArgumentType : ArgumentType<float>
    {
        private readonly float? _min;

        private readonly float? _max;

        public FloatArgumentType(float? min = null, float? max = null)
        {
            _min = min;

            _max = max;
        }

        public override string UsageText => "<number>";

        public override float ParseValue(StringReader reader)
        {
            var value = reader.ReadFloat();

            if ((_min is { } min && value < min) || (_max is { } max && value > max))
                throw new CommandSyntaxException("Commands.Error.OutOfRange", [value.ToString(), _min?.ToString() ?? "", _max?.ToString() ?? ""], reader.Cursor);

            return value;
        }
    }
}
