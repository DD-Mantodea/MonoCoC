using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5599-5629 grandThrust
    public static class PlayerGrandThrustHandlers
    {
        private static readonly Identifier SpearMastery = new("coc", "Spear");

        [Register("coc:can_grand_thrust")]
        public static bool CanGrandThrust(CombatContext context, CombatState state)
        {
            var weapon = context.Player.Weapon;

            return weapon.IsSpear() && context.Player.MasteryLevel(SpearMastery) >= 2;
        }

        [Register("coc:enemy_not_distant")]
        public static bool EnemyNotDistant(CombatContext context, CombatState state) =>

            context.Enemy.Distance != CombatDistance.Distant;

        [Register("coc:special_grand_thrust")]
        public static void GrandThrust(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.GrandThrust.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, 15),
            });

            if (avoid.AttackFailed)
                context.Append("Combat.Special.GrandThrust.Miss".Localized());
            else
            {
                context.Append("Combat.Special.GrandThrust.Hit".Localized());

                // 完全无视护甲：直接按力量+武器伤害计算，不再走 DamagePercent
                var raw = (player.Strength + player.WeaponAttack

                    + player.StatusEffectValue(StatusEffectIds.ChargeWeapon, 0)) * player.PhysMod();

                var damage = CombatCalculator.GlobalMod(player, raw * 1.30f);

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);
            }

            player.MasteryXP(SpearMastery, 10 + context.Random.Next(11));

            CombatRangeData.CloseDistance(enemy);
        }
    }
}