using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class FrogGirlRedCombatHandlers
    {

        [Register("coc:frog_girl_red_poison")]
        public static void PoisonAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.FrogGirlRed.Poison.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.FrogGirlRed.Poison.Miss".Localized());

                return;
            }

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength * 0.75f + enemy.WeaponAttack + context.Random.Next(10)));

            context.Append("Combat.FrogGirlRed.Poison.Hit".Localized());

            StatusEffects.EffectApplication.Apply(
                player, StatusEffectIds.Poison, value2: 1);
        }

        [Register("coc:frog_girl_red_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.FrogGirlRed.Tease.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + context.Random.Next(5));
        }

        [Register("coc:frog_girl_red_attack")]
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
    }
}