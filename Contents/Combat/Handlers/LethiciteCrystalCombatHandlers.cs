using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class LethiciteCrystalCombatHandlers
    {

        [Register("coc:lethicite_crystal_pulse")]
        public static void Pulse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.AppendLog("Combat.LethiciteCrystal.Pulse.TheCrystalPulses".Localized());

            switch (context.Random.Next(3))
            {
                case 0:
                    context.AppendLog("Combat.LethiciteCrystal.Pulse.ABeautifulGirl".Localized());

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.SheGrindsHard".Localized());

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.YouCumToo".Localized());

                    break;

                case 1:
                    context.AppendLog("Combat.LethiciteCrystal.Pulse.YouReSurprised".Localized());

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.MinutesLaterShe".Localized());

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.YouHaveNo".Localized());

                    break;

                case 2:
                    context.AppendLog("Combat.LethiciteCrystal.Pulse.YouSmellSomething".Localized());

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.YouEjaculateFor".Localized());

                    break;

                default:

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.YouLeaveThe".Localized());

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.SuddenlyYouNotice".Localized());

                    context.AppendLog("Combat.LethiciteCrystal.Pulse.YouStartTurning".Localized());

                    break;
            }

            context.AppendLog("Combat.LethiciteCrystal.Pulse.YouShakeYour".Localized());

            context.Player.TakeLustDamage(20 + context.Random.Next(20), true, true);

            context.Player.TakeLustDamage(5 + context.Random.Next(3), true, false);
        }
    }
}