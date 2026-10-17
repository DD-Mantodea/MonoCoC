using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class FrogGirlPinkCombatHandlers
    {

        [Register("coc:frog_girl_pink_drug")]
        public static void Drug(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.FrogGirlPink.Drug.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.FrogGirlPink.Drug.Miss".Localized());

                return;
            }

            context.Append("Combat.FrogGirlPink.Drug.Hit".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength / 2 + enemy.WeaponAttack + context.Random.Next(5)));

            player.CreateStatusEffect(StatusEffectIds.FrogPoison);
        }

        [Register("coc:frog_girl_pink_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.FrogGirlPink.Tease.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 10 + context.Random.Next(8));
        }

        [Register("coc:frog_girl_pink_attack")]
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