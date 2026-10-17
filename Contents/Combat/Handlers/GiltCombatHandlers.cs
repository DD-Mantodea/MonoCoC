using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GiltCombatHandlers
    {

        [Register("coc:gilt_dive")]
        public static void Dive(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Gilt.Dive.Lead".Localized());

            if (context.PlayerWaited)
            {
                context.Append("Combat.Gilt.Dive.Waited".Localized());

                enemy.IsFlying = false;

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = true
            });

            switch (avoid.Outcome)
            {
                case AvoidOutcome.Block:
                    context.Append("Combat.Gilt.Dive.Blocked".Localized());

                    break;

                case AvoidOutcome.Hit:
                case AvoidOutcome.Counter:
                    context.Append("Combat.Gilt.Dive.Hit".Localized());

                    player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack + context.Random.Next(50)));

                    break;

                default:
                    context.Append("Combat.Gilt.Dive.Dodged".Localized());

                    break;
            }

            if (avoid.Outcome is AvoidOutcome.Hit or AvoidOutcome.Block or AvoidOutcome.Counter)
                player.Stun(1);

            enemy.IsFlying = false;
        }

        [Register("coc:gilt_attack")]
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

        [Register("coc:gilt_attack_two")]
        public static void Attack2(CombatContext context, CombatAbilityDefinition ability) =>

            Attack(context, ability);

        [Register("coc:gilt_fly")]
        public static void Fly(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Gilt.Fly.Lead".Localized());

            context.Enemy.IsFlying = true;
        }
    }
}