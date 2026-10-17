using MonoCoC.Registry;

namespace MonoCoC.Contents.Stats.ModifierSources
{
    public record PerkSource(Identifier PerkId, string Name) : IModifierSource
    {
        public string DisplayName => Name;

        public virtual bool Equals(PerkSource other) => other is not null && other.PerkId == PerkId;

        public override int GetHashCode() => PerkId.GetHashCode();
    }
}