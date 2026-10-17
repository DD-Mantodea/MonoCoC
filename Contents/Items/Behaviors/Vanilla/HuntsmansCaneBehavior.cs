using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{

    [Register<HuntsmansCaneBehavior>("coc:huntsmans_cane")]
    public class HuntsmansCaneBehavior : ItemBehavior, IItemEquipBehavior
    {
        public static EquipmentSource SourceOf(Item item, EquipSlot slot) => new(slot, item.Id, item.Name);

        public void OnEquip(Player player, Item item)
        {
            var source = SourceOf(item, EquipSlot.MainHand);

            player.Stats.AddModifier(GameStats.LustResistance, ModifierOperation.Factor, () => 1.5f, source);

            player.Stats.AddModifier(GameStats.PhysDamage, ModifierOperation.Factor, () => 0.5f, source);

            player.Stats.AddModifier(GameStats.CorruptionGain, ModifierOperation.Factor, () => 0.25f, source);
        }

        public void OnRemove(Player player, Item item) => player.Stats.RemoveFrom(SourceOf(item, EquipSlot.MainHand));
    }
}