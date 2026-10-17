using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 Mimic 的招式（Mimic.hx:24-48）。
    public static class MimicCombatHandlers
    {
        // 原版 eAttack()：普通攻击（Monster.eAttack 的通用结算）。
        [Register("coc:mimic_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        // 原版 mimicBite()（Mimic.hx:24-37）：只可闪避，命中后伤害带 NG+ 缩放且最低 10。
        [Register("coc:mimic_bite")]
        private static void Bite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Mimic.MimicBite.TheCreatureLashes".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = enemy,
                Defender = player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });

            if (avoid.AttackFailed)
            {
                context.Append("Combat.Mimic.MimicBite.ThankfullyYouRe".Localized());

                return;
            }

            context.Append("Combat.Mimic.MimicBite.OwTheMouth".Localized());

            // 原版：(10 + str / 2 + Utils.rand(str / 3)) * (1 + player.newGamePlusMod() * 0.3)
            var damage = (10 + enemy.Strength / 2
                + context.Random.Next(Math.Max(1, (int)(enemy.Strength / 3))))
                * (1 + player.NewGamePlusMod() * 0.3f);

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            if (damage < 10)
                damage = 10;

            player.TakeDamage(damage);
        }
    }
}
