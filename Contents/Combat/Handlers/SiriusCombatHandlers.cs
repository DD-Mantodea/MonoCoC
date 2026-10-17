using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class SiriusCombatHandlers
    {

        [Register("coc:sirius_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Sirius.EAttack.SiriusReadiesHis".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Sirius.OutputPlayerDodged.WithYourTrained".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:sirius_man_naga_tease")]
        public static void ManNagaTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Sirius.ManNagaTease.TheSnakeMan".Localized());

            if (context.Random.Next(10) == 0)
                context.Append("Combat.Sirius.ManNagaTease.YouBlinkAnd".Localized());

            float lust;

            if (enemy.HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Append("Combat.Sirius.ManNagaTease.ThoughYourVision".Localized());

                lust = 5 + player.Libido / 10f - player.Intelligence / 20f;
            }
            else
            {
                context.Append("Combat.Sirius.ManNagaTease.ThosePoolsOf".Localized());

                lust = 10 + player.Libido / 7f - player.Intelligence / 20f;
            }

            player.Lust = Math.Clamp(player.Lust + lust, 0, player.MaxLust);
        }

        [Register("coc:sirius_naga_spit_attack")]
        public static void NagaSpitAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Sirius.NagaSpitAttack.HissingLoudlySirius".Localized());

            if (enemy.Speed / 20f + context.Random.Next(20) + 1 > player.Speed / 20f + 10)
            {
                context.Append("Combat.Sirius.NagaSpitAttack.TheVileSpray".Localized());

                if (!player.HasStatusEffect(StatusEffectIds.Blind))
                    player.CreateStatusEffect(StatusEffectIds.Blind, 3, 0, 0, 0);
            }
            else
                context.Append("Combat.Sirius.NagaSpitAttack.YouQuicklyLean".Localized());
        }

        [Register("coc:sirius_poison_bite")]
        public static void PoisonBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Sirius.PoisonBite.WithALoud".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,

                Defender = player,

                DoDodge = true,

                DoParry = false,

                DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
                context.Append("Combat.Sirius.PoisonBite.YouDodgeJust".Localized());

            context.Append("Combat.Sirius.PoisonBite.TheSnakeMan".Localized());

            player.HealthChange(-(40 + context.Random.Next(20)));
        }
    }
}