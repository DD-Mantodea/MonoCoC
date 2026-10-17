using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5668-5688 whipTrip
    public static class PlayerWhipTripHandlers
    {
        private static readonly Identifier WhipMastery = new("coc", "Whip");

        [Register("coc:can_whip_trip")]
        public static bool CanWhipTrip(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.Weapon.IsWhip() && player.MasteryLevel(WhipMastery) >= 2;
        }

        [Register("coc:effect_tripped_expire")]
        public static void TrippedExpire(CombatContext context, Creature host, StatusEffect status) =>

            context.Append("Combat.Effect.Tripped.Expire".Localized());

        [Register("coc:special_whip_trip")]
        public static void WhipTrip(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Special.WhipTrip.Intro".Localized());

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = player, Defender = enemy,

                DoDodge = true, DoParry = false, DoBlock = false,

                ToHitChance = player.StandardDodgeFunc(enemy, -10),
            });

            if (avoid.AttackHit)
            {
                context.Append("Combat.Special.WhipTrip.Hit".Localized());

                if (!enemy.HasStatusEffect(StatusEffectIds.Tripped))
                {
                    enemy.CreateStatusEffect(StatusEffectIds.Tripped, 0, 0, 0, 0, 2);

                    CombatCalculator.AddCombatBuff(enemy, GameStats.Speed, -enemy.Speed * 0.5f, "Tripped", StatusEffectIds.Tripped);
                }
            }
            else
            {
                context.Append("Combat.Special.WhipTrip.Miss".Localized());

                CombatRangeData.MoveMonsterDistant(enemy);
            }
        }
    }
}