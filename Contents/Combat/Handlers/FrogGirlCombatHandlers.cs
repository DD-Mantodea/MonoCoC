using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class FrogGirlCombatHandlers
    {

        [Register("coc:frog_girl_kick")]
        public static void Kick(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.FrogGirl.Kick.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = true
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Block:
                    context.Append("Combat.FrogGirl.Kick.Blocked".Localized());

                    return;

                case AvoidOutcome.Parry:
                    context.Append("Combat.FrogGirl.Kick.Parry".Localized());

                    return;

                case AvoidOutcome.Dodge:
                    context.Append("Combat.FrogGirl.Kick.Dodged".Localized());

                    return;
            }

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength * 1.5f + context.Random.Next(30)));

            if (player.Stun(1, 50))
                context.Append("Combat.FrogGirl.Kick.Stunned".Localized());
        }

        [Register("coc:frog_girl_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.FrogGirl.Tease.Lead".Localized());

            player.Lust = Math.Min(player.MaxLust, player.Lust + 13 + context.Random.Next(10));
        }

        [Register("coc:frog_girl_attack")]
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