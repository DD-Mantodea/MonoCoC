using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:6665-6685 vineTrip
    public static class PlayerVineTripHandlers
    {
        private static readonly Identifier VineArmor = new("coc", "vine_armor");

        [Register("coc:has_vine_trip")]
        public static bool HasVineTrip(CombatContext context, CombatState state)
        {
            var armor = context.Player.Armor;

            return armor != null && armor.Id == VineArmor && armor.Stage > 3;
        }

        [Register("coc:special_vine_trip")]
        public static void VineTrip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.VineTrip.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, -10),
            });

            if (avoid.AttackHit)
            {
                context.Append("Combat.Special.VineTrip.Hit".Localized());

                if (!enemy.HasStatusEffect(StatusEffectIds.Tripped))
                {
                    enemy.CreateStatusEffect(StatusEffectIds.Tripped, 0, 0, 0, 0, 2);

                    CombatCalculator.AddCombatBuff(enemy, GameStats.Speed, -enemy.Speed * 0.5f, "Tripped", StatusEffectIds.Tripped);
                }

                enemy.Bleed(player);
            }
            else
            {
                context.Append("Combat.Special.VineTrip.Miss".Localized());

                CombatRangeData.MoveMonsterDistant(enemy);
            }
        }
    }
}