using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class AmarokCombatHandlers
    {

        [Register("coc:amarok_attack")]
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

        [Register("coc:amarok_claw")]
        public static void Claw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasStatusEffect(StatusEffectIds.Blind))
            {
                context.Append("Combat.Amarok.Claw.Blind".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Amarok.Claw.Miss".Localized());

                return;
            }

            context.Append("Combat.Amarok.Claw.Hit".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength + 50 + context.Random.Next(100)));
        }

        [Register("coc:amarok_tail")]
        public static void Tail(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Amarok.Tail.Lead".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.Amarok.Tail.Miss".Localized());

                return;
            }

            context.Append("Combat.Amarok.Tail.HitLead".Localized());

            context.Append(player.Stun(0, 33)

                ? "Combat.Amarok.Tail.Stunned".Localized()

                : "Combat.Amarok.Tail.NotStunned".Localized());

            player.HealthChange(-CombatCalculator.ReduceDamage(context, player, enemy.Strength + 100 + context.Random.Next(75)));
        }
    }
}