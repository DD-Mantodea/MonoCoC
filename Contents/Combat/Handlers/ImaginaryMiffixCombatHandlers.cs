using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class ImaginaryMiffixCombatHandlers
    {

        [Register("coc:imaginary_miffix_onepunch")]
        public static void Onepunch(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.AppendLog("Combat.ImaginaryMiffix.Onepunch.MiffixLooksAt".Localized());

            context.AppendLog("Combat.ImaginaryMiffix.Onepunch.YouReadyYourself".Localized());

            player.TakeDamage(99999);

            player.Health = 0;

            context.AppendLog("Combat.ImaginaryMiffix.Onepunch.YouCollapseOn".Localized());
        }
    }
}