using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:4209-4226 testResolve / testResolve2
    public static class PlayerTestResolveHandlers
    {
        [Register("coc:has_family_talisman")]
        public static bool HasFamilyTalisman(CombatContext context, CombatState state) =>

            context.Player.HasKeyItem("Family Talisman");

        [Register("coc:resolve_active")]
        public static bool ResolveActive(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.Resolve);

        [Register("coc:special_test_resolve")]
        public static void TestResolve(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.Special.TestResolve.Intro".Localized());

            context.Append("Combat.Special.TestResolve.Testing".Localized());

            Scenes.SceneInstance<Dungeons.ManorScene>.Get().TestResolve(false);
        }
    }
}