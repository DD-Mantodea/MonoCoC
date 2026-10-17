using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 ImaginaryMiffix 的招式（ImaginaryMiffix.hx:51-57）。
    public static class ImaginaryMiffixCombatHandlers
    {
        // 原版 onepunch()：三句台词 + 99999 伤害 + 强制 HP 归零。
        [Register("coc:imaginary_miffix_onepunch")]
        private static void Onepunch(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.ImaginaryMiffix.Onepunch.MiffixLooksAt".Localized());
            context.Append("Combat.ImaginaryMiffix.Onepunch.YouReadyYourself".Localized());

            player.TakeDamage(99999);

            player.Health = 0;

            context.Append("Combat.ImaginaryMiffix.Onepunch.YouCollapseOn".Localized());
        }
    }
}
