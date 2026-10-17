using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class BasiliskCombatHandlers
    {

        [Register("coc:basilisk_compulsion")]
        public static void Compulsion(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Basilisk.Compulsion.Lead".Localized());

            if (player.Intelligence / 5f + context.Random.Next(20) >= 24)
            {
                context.Append("Combat.Basilisk.Compulsion.Resisted".Localized());

                return;
            }

            if (player.HasPerk(PerkIds.BasiliskResistance) || player.CanUseStare() || player.HasKeyItem("Laybans"))
            {
                context.Append("Combat.Basilisk.Compulsion.Immune".Localized());

                return;
            }

            context.Append("Combat.Basilisk.Compulsion.Hit".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 3);

            CombatCalculator.AddCombatBuff(player, GameStats.Speed, -20, "BasiliskSlow", StatusEffectIds.BasiliskSlow);

            player.CreateStatusEffect(StatusEffectIds.BasiliskSlow, 20);

            player.CreateStatusEffect(StatusEffectIds.BasiliskCompulsion);

            CoC.Instance.Flags.Set(GameFlagIds.BasiliskResistanceTracker,

                CoC.Instance.Flags.Get(GameFlagIds.BasiliskResistanceTracker) + 2);
        }

        [Register("coc:basilisk_tail_swipe")]
        public static void TailSwipe(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Basilisk.TailSwipe.Lead".Localized());

            var damage = (int)(enemy.Strength + 20 - context.Random.NextDouble() * (player.Toughness + player.ArmorDefense));

            if (damage < 0)
                damage = 0;

            if (damage == 0)
                context.Append("Combat.Basilisk.TailSwipe.Harmless".Localized());

            player.HealthChange(-damage);
        }
    }
}