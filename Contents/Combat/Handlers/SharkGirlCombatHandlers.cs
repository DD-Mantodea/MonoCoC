using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class SharkGirlCombatHandlers
    {

        [Register("coc:shark_girl_shark_tease")]
        public static void SharkTease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            if (context.Random.Next(2) == 0)
            {
                context.Append("Combat.SharkGirl.SharkTease.YouChargeAt".Localized());

                player.HealthChange(-(4 + context.Random.Next(4)));

                player.TakeLustDamage(10 + player.Libido / 20f);
            }
            else
            {
                context.Append("Combat.SharkGirl.SharkTease.YouPullYour".Localized());

                player.HealthChange(-(4 + context.Random.Next(4)));

                player.TakeLustDamage(5 + player.Libido / 10f);
            }
        }

        [Register("coc:shark_girl_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

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
                enemy.OutputAttack(context, 0);

                return;
            }

            var damage = CombatCalculator.ReduceDamage(context, player, enemy.Strength + enemy.WeaponAttack);

            player.HealthChange(-damage);

            enemy.OutputAttack(context, damage);
        }
    }
}