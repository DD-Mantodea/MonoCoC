using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ChameleonGirlCombatHandlers
    {

        [Register("coc:chameleon_girl_chameleon_tongue_attack")]
        public static void ChameleonTongueAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var enemy = context.Enemy;

            enemy.WeaponName = "tongue";

            enemy.WeaponVerb = "tongue-slap";

            enemy.WeaponAttack = 10;

            SneakAttack(context);

            enemy.WeaponAttack = 30;

            enemy.WeaponName = "claws";

            enemy.WeaponVerb = "claw";
        }

        [Register("coc:chameleon_girl_chameleon_claws")]
        public static void ChameleonClaws(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) < 1)
            {
                context.Append("Combat.ChameleonGirl.Claws.Blind".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = true, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.ChameleonGirl.Claws.Miss".Localized());

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            if (damage > 0)
            {
                context.Append("Combat.ChameleonGirl.Claws.Hit".Localized());

                player.HealthChange(-damage);
            }
            else
                context.Append("Combat.ChameleonGirl.Claws.Absorbed".Localized());
        }

        [Register("coc:chameleon_girl_roll_kick_claw_what_the_fuck_combo_is_this_shit")]
        public static void RollKickClaw(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(3) < 1)
            {
                context.Append("Combat.ChameleonGirl.RollKick.Blind".Localized());

                return;
            }

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy, Defender = player, DoDodge = true, DoParry = false, DoBlock = false
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.Append("Combat.ChameleonGirl.RollKick.Dodged".Localized());

                player.HealthChange(-(1 + context.Random.Next(10)));

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack) + 25;

            if (damage > 0)
            {
                context.Append("Combat.ChameleonGirl.RollKick.Hit".Localized());

                player.HealthChange(-damage);
            }
            else
                context.Append("Combat.ChameleonGirl.RollKick.Absorbed".Localized());
        }

        [Register("coc:chameleon_girl_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) => SneakAttack(context);

        public static void SneakAttack(CombatContext context)
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