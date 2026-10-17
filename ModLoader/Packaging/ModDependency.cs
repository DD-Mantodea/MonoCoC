namespace MonoCoC.ModLoader
{
    public sealed class ModDependency
    {
        public required string Id { get; init; }

        public ModVersionRange Range { get; init; } = ModVersionRange.Any;

        public bool Weak { get; init; }

        public override string ToString() => Weak ? $"{Id} (weak) {Range}" : $"{Id} {Range}";
    }
}
