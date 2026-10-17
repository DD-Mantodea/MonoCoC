using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 SharkGirl 的招式（src/classes/scenes/places/boat/SharkGirl.hx:9-30）。
    public static class SharkGirlCombatHandlers
    {
        // 原版 sharkTease（SharkGirl.hx:10-23）：rand(2) 两种挑衅，各自带少量伤害与欲望。
        [Register("coc:shark_girl_shark_tease")]
        private static void SharkTease(CombatContext context, CombatAbilityDefinition ability)
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

        // 原版 Monster.eAttack（Monster.hx:940-963）。
        [Register("coc:shark_girl_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
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
