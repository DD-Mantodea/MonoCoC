using MonoCoC.Contents.Items;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Stats.ModifierSources
{
    public record EquipmentSource(EquipSlot Slot, Identifier ItemId, string Name, int Instance = 0) : IModifierSource
    {
        public string DisplayName => Name;
    }
}