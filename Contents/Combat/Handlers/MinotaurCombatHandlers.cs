using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MinotaurCombatHandlers
    {

        [Register("coc:minotaur_mino_pheromones")]
        public static void MinoPheromones(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Minotaur.Pheromones.Lead".Localized());

            if (context.Random.Next(3) == 0)
            {
                context.Append("Combat.Minotaur.Pheromones.Face".Localized());

                if (player.Lust >= player.MaxLust * 0.75f)
                {
                    context.Append("Combat.Minotaur.Pheromones.Swallow".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 10 + player.Libido / 10);
                }
                else
                {
                    context.Append("Combat.Minotaur.Pheromones.Lick".Localized());

                    player.Lust = Math.Min(player.MaxLust, player.Lust + 5 + player.Libido / 20);
                }
            }
            else
                context.Append("Combat.Minotaur.Pheromones.Miss".Localized());

            context.Append("Combat.Minotaur.Pheromones.Scent".Localized());
        }

        [Register("coc:minotaur_attack")]
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