using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class KitsuneCombatHandlers
    {

        [Register("coc:kitsune_fox_fire")]
        public static void FoxFire(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Kitsune.FoxFire.One".Localized());

            context.WriteAction("Combat.Kitsune.FoxFire.Two".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 5f + context.Random.Next(20)));

            AddLust(context, 15f + player.Sensitivity / 10f);
        }

        [Register("coc:kitsune_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var select = context.Random.Next(3);

            context.WriteAction(($"Combat.Kitsune.Tease.{select + 1}").Localized());

            AddLust(context, 5f + player.Sensitivity / 7f);
        }

        [Register("coc:kitsune_seal")]
        public static void Seal(CombatContext context, CombatAbilityDefinition ability) =>

            KitsuneSeals.Seal(context);

        [Register("coc:kitsune_entwine")]
        public static void Entwine(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Kitsune.Entwine.One".Localized());

            context.WriteAction("Combat.Kitsune.Entwine.Two".Localized());

            player.CreateOrFindStatusEffect(StatusEffectIds.PCTailTangle);

            AddLust(context, 10f + player.Sensitivity / 8f);
        }

        [Register("coc:kitsune_illusion")]
        public static void Illusion(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Kitsune.Illusion.One".Localized());

            var resist = KitsuneSeals.CalculateAttackResist(player);

            if (context.Random.Next(100) < resist)
            {
                context.WriteAction("Combat.Kitsune.Illusion.Resisted".Localized());

                player.HealthChange(-CombatCalculator.ReduceDamage(context, player, 5f));

                return;
            }

            context.Enemy.CreateStatusEffect(StatusEffectIds.Illusion);

            var source = new StatusSource(StatusEffectIds.Illusion, "Kitsune Illusion");

            context.Enemy.Stats.AddModifier(GameStats.Speed, ModifierOperation.Addition, () => 20f, source);
        }

        public static void AddLust(CombatContext context, float amount)
        {
            var player = context.Player;

            player.Lust = Math.Min(player.MaxLust, player.Lust + amount);
        }
    }
}