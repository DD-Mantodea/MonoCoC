using MonoCoC.Registry;

namespace MonoCoC.Contents.Datas.Vanilla
{
    public static class CombatAbilityIds
    {

        // 原版 CombatAbilities.hx:2744 Shielding 已由 ArianCombatHandlers（coc:arian_shield）实现；
        // 原版 CombatAbilities.hx:5287 TimesBashed 已由 TailSlamCombatHandlers（coc:tail_slam）实现。
        public static readonly Identifier Tease = new("coc", "tease");

        public static readonly Identifier Fantasize = new("coc", "fantasize");
    }
}