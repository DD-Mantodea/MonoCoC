using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:6815-6835 skirmish
    public static class PlayerSkirmishHandlers
    {
        [Register("coc:can_skirmish")]
        public static bool CanSkirmish(CombatContext context, CombatState state) =>

            context.Player.Weapon.IsRanged();

        [Register("coc:special_skirmish")]
        public static void Skirmish(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.Skirmish.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = true, DoBlock = true,

                ToHitChance = player.StandardDodgeFunc(enemy, -15),
            });

            if (avoid.AttackHit)
            {
                var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * 0.7f;

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Ranged);

                context.Append("Combat.Special.Skirmish.Damage".Localized((int)damage));
            }

            if (enemy.Distance != CombatDistance.Distant)
                if (context.Random.NextDouble() * 100 < player.MovementChance(enemy)
                    || !enemy.ShouldMove(CombatDistance.Melee, true))
                {
                    context.Append("Combat.Special.Skirmish.Distance".Localized());

                    CombatRangeData.MovePlayerDistant(enemy);
                }
                else
                    context.Append("Combat.Special.Skirmish.CloseIn".Localized());
        }
    }
}