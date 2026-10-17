using MonoCoC.Registry;

namespace MonoCoC.Contents.Stats.ModifierSources
{
    public record StatusSource(Identifier StatusId, string Name) : IModifierSource
    {
        public string DisplayName => Name;

        public virtual bool Equals(StatusSource other) => other is not null && other.StatusId == StatusId;

        public override int GetHashCode() => StatusId.GetHashCode();
    }
}