using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<JewelryBehavior>("coc:jewelry")]
    public class JewelryBehavior : ItemBehavior, IItemEquipBehavior
    {
        public void OnEquip(Player player, Item item)
        {
            if (item is not Jewelry jewelry)
                return;

            var source = new EquipmentSource(EquipSlot.Accessory, jewelry.Id, jewelry.Name);

            switch ((int)jewelry.EffectId)
            {
                case 1:
                    player.Stats.AddModifier(GameStats.MinLibido, ModifierOperation.Addition, () => jewelry.EffectMagnitude, source);

                    break;

                case 2:
                    player.Stats.AddModifier(GameStats.Fertility, ModifierOperation.Addition, () => jewelry.EffectMagnitude, source);

                    break;

                case 3:
                    player.Stats.AddModifier(GameStats.CriticalChance, ModifierOperation.Addition, () => jewelry.EffectMagnitude, source);

                    break;

                case 4:
                    player.Stats.AddModifier(GameStats.HealthRegenPercent, ModifierOperation.Addition, () => jewelry.EffectMagnitude, source);

                    break;

                case 5:
                    player.Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition, () => jewelry.EffectMagnitude, source);

                    break;

                case 6:
                    player.Stats.AddModifier(GameStats.PhysicalDamage, ModifierOperation.Multiplier, () => jewelry.EffectMagnitude / 100f, source);

                    break;

                case 7:
                    player.Stats.AddModifier(GameStats.SpellPower, ModifierOperation.Addition, () => jewelry.EffectMagnitude, source);

                    break;

                case 8:
                    player.Stats.AddModifier(GameStats.MinLibido, ModifierOperation.Addition, () => -jewelry.EffectMagnitude, source);

                    break;

                case 10:
                    player.Stats.AddModifier(GameStats.SpellPower, ModifierOperation.Addition, () => jewelry.EffectMagnitude * 10, source);

                    break;

                case 11:
                    player.Stats.AddModifier(GameStats.Accuracy, ModifierOperation.Addition, () => jewelry.EffectMagnitude, source);

                    break;

                case 12:

                    break;

                case 13:
                    player.Stats.AddModifier(GameStats.Dodge, ModifierOperation.Addition, () => 20, source);

                    player.Stats.AddModifier(GameStats.CriticalChance, ModifierOperation.Addition, () => 15, source);

                    player.Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Multiplier, () => -0.4f, source);

                    break;
            }
        }

        public void OnRemove(Player player, Item item)
        {
            if (item is not Jewelry jewelry)
                return;

            player.Stats.RemoveFrom(new EquipmentSource(EquipSlot.Accessory, jewelry.Id, jewelry.Name));
        }
    }
}