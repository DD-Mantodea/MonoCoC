using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:6808-6813 freezeTime
    public static class PlayerFreezeTimeHandlers
    {
        private static readonly Identifier ClockworkShield = new("coc", "clockwork_shield");

        [Register("coc:has_clockwork_shield")]
        public static bool HasClockworkShield(CombatContext context, CombatState state) =>

            context.Player.Shield != null && context.Player.Shield.Id == ClockworkShield;

        [Register("coc:freeze_time_disabled")]
        public static bool FreezeTimeDisabled(CombatContext context, CombatState state) =>

            context.Player.Shield?.Save.GetBool("used") == true || state.Round < 3;

        [Register("coc:special_freeze_time")]
        public static void FreezeTime(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Special.FreezeTime.Text".Localized());

            player.CreateStatusEffect(StatusEffectIds.TimeFrozen, 0, 0, 0, 0, 3);

            player.Shield?.Save.SetBool("used", true);
        }
    }
}