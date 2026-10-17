using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class LethiciteCrystalCombatHandlers
    {

        [Register("coc:lethicite_crystal_pulse")]
        public static void Pulse(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Append("Combat.LethiciteCrystal.Pulse.TheCrystalPulses".Localized());

            switch (context.Random.Next(3))
            {
                case 0:
                    context.Append("Combat.LethiciteCrystal.Pulse.ABeautifulGirl".Localized());

                    context.Append("Combat.LethiciteCrystal.Pulse.SheGrindsHard".Localized());

                    context.Append("Combat.LethiciteCrystal.Pulse.YouCumToo".Localized());

                    break;

                case 1:
                    context.Append("Combat.LethiciteCrystal.Pulse.YouReSurprised".Localized());

                    context.Append("Combat.LethiciteCrystal.Pulse.MinutesLaterShe".Localized());

                    context.Append("Combat.LethiciteCrystal.Pulse.YouHaveNo".Localized());

                    break;

                case 2:
                    context.Append("Combat.LethiciteCrystal.Pulse.YouSmellSomething".Localized());

                    context.Append("Combat.LethiciteCrystal.Pulse.YouEjaculateFor".Localized());

                    break;

                default:

                    context.Append("Combat.LethiciteCrystal.Pulse.YouLeaveThe".Localized());

                    context.Append("Combat.LethiciteCrystal.Pulse.SuddenlyYouNotice".Localized());

                    context.Append("Combat.LethiciteCrystal.Pulse.YouStartTurning".Localized());

                    break;
            }

            context.Append("Combat.LethiciteCrystal.Pulse.YouShakeYour".Localized());

            context.Player.TakeLustDamage(20 + context.Random.Next(20), true, true);

            context.Player.TakeLustDamage(5 + context.Random.Next(3), true, false);
        }
    }
}