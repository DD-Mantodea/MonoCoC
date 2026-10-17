using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 classes/scenes/areas/desert/Ghoul.hx 的四个招式
    public static class GhoulCombatHandlers
    {
        [Register("coc:ghoul_hyena_claw")]
        public static void HyenaClaw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Append("Scenes.Ghoul.HyenaClaw.BlindClawMiss".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.Ghoul.HyenaClaw.ClawDodged".Localized());

                return;
            }

            context.Append("Scenes.Ghoul.HyenaClaw.ClawRakes".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, context.Random.Next(5) + 5);

            player.HealthChange(-damage);
        }

        [Register("coc:ghoul_hyena_bite")]
        public static void HyenaBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Append("Scenes.Ghoul.HyenaBite.BlindBiteMiss".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.Ghoul.HyenaBite.BiteDodged".Localized());

                return;
            }

            context.Append("Scenes.Ghoul.HyenaBite.TeethSink".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, context.Random.Next(10) + 5);

            player.HealthChange(-damage);
        }

        [Register("coc:ghoul_blind")]
        public static void GhoulBlind(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Scenes.Ghoul.GhoulBlind.BlindingFlash".Localized());

            var roll = Math.Max(1, (int)(player.Intelligence / 5));

            if (context.Random.Next(roll) <= 4)
            {
                context.Append("Scenes.Ghoul.GhoulBlind.BlindedStatus".Localized());

                player.CreateStatusEffect(StatusEffectIds.Blind, 1 + context.Random.Next(3), 0, 0, 0);
            }
            else
                context.Append("Scenes.Ghoul.GhoulBlind.BlinkInTime".Localized());
        }

        [Register("coc:ghoul_magic")]
        public static void GhoulMagic(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Ghoul.GhoulMagic.AlchemicCircle".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Scenes.Ghoul.GhoulMagic.CircleDodged".Localized());

                return;
            }

            context.Append("Scenes.Ghoul.GhoulMagic.FlamesScorch".Localized());

            var damage = CombatCalculator.ReduceDamage(context, player, context.Random.Next(10) + 10);

            player.HealthChange(-damage);
        }
    }
}