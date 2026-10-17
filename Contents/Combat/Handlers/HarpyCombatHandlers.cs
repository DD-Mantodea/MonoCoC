using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class HarpyCombatHandlers
    {

        [Register("coc:harpy_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }

        [Register("coc:harpy_uber_charge")]
        public static void UberCharge(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (!enemy.HasStatusEffect(StatusEffectIds.Uber))
            {
                enemy.CreateStatusEffect(StatusEffectIds.Uber);

                context.Append("Combat.Harpy.Uber.Charge".Localized());

                return;
            }

            if (!context.PlayerWaited)
            {
                context.Append("Combat.Harpy.Uber.Hit".Localized());

                var damage = 160 + context.Random.Next(20);

                player.HealthChange(-damage);

                enemy.RemoveStatusEffect(StatusEffectIds.Uber);

                return;
            }

            context.Append("Combat.Harpy.Uber.Dodged".Localized());

            player.HealthChange(-(10 + context.Random.Next(10)));

            enemy.RemoveStatusEffect(StatusEffectIds.Uber);

            enemy.HealthChange(-20);
        }

        [Register("coc:harpy_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Harpy.Tease.Lead".Localized());

            var lust = 12 + context.Random.Next(Math.Max(1, (int)(player.Sensitivity / 5)));

            player.Lust = Math.Min(player.MaxLust, player.Lust + lust);
        }
    }
}