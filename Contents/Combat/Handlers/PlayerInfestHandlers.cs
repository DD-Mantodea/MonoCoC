using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:1762-1776 Infest → WormsScene.playerInfest
    public static class PlayerInfestHandlers
    {
        [Register("coc:can_infest")]
        public static bool CanInfest(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.HasStatusEffect(StatusEffectIds.Infested)

                && player.StatusEffectValue(StatusEffectIds.Infested, 0) == 5

                && player.HasCock();
        }

        [Register("coc:special_infest")]
        public static void Infest(CombatContext context, CombatAbilityDefinition ability) =>

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Mountain.WormsScene>.Get().PlayerInfest();
    }
}