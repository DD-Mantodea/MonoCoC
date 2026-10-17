using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GreenSlimeCombatHandlers
    {

        [Register("coc:green_slime_lust_attack")]
        public static void LustAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.GreenSlime.LustAttack.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + player.Libido / 10 + 8);
        }

        [Register("coc:green_slime_lust_reduction")]
        public static void LustReduction(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            context.Append("Combat.GreenSlime.LustReduction.Lead".Localized());

            enemy.Lust = Math.Max(0, enemy.Lust - 13);
        }

        [Register("coc:green_slime_attack")]
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