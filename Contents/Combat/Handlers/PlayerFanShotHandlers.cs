using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4484-4505 fanShot
    public static class PlayerFanShotHandlers
    {
        private static readonly Identifier BowMastery = new("coc", "Bow");

        [Register("coc:can_fan_shot")]
        public static bool CanFanShot(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.HasPerk(PerkIds.FanShot)

                && (player.HasKeyItem("Bow") || player.HasKeyItem("Kelt's Bow"));
        }

        [Register("coc:special_fan_shot")]
        public static void FanShot(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (player.HasStatusEffect(StatusEffectIds.BowDisabled))
            {
                context.Append("Combat.Special.FireBow.Disabled".Localized());

                return;
            }

            context.Append("Combat.Special.FanShot.Intro".Localized());

            var hitChance = PlayerFireBowHandlers.FireBowChance(context);

            if (!enemy.Plural)
                hitChance = (int)(hitChance / 1.75f);

            for (var i = 0; i < 6; i++)
                PlayerFireBowHandlers.ShootArrow(context, hitChance, true);

            player.MasteryXP(BowMastery, 5 + context.Random.Next(11));
        }
    }
}