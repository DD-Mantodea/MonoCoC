using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class MimicCombatHandlers
    {

        [Register("coc:mimic_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            CombatController.Instance!.Resolver.EnemyBasicAttack();
        }

        [Register("coc:mimic_bite")]
        public static void Bite(CombatContext context, CombatAbilityDefinition ability)
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