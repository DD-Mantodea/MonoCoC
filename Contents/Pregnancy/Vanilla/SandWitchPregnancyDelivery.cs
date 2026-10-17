using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<SandWitchPregnancyDelivery>("coc:delivery_sand_witch")]
    public class SandWitchPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            switch (context.Incubation)
            {

                case 336:
                    context.Write("Pregnancy.SandWitch.Stage336".Localized());

                    return true;

                case 280:
                {
                    context.Write("Pregnancy.SandWitch.Stage280Prefix".Localized());

                    context.Write(player.BiggestLactation() < 1

                        ? "Pregnancy.SandWitch.StartingToLactate".Localized()

                        : "Pregnancy.SandWitch.LactatingMore".Localized());

                    context.Write("Pregnancy.SandWitch.Stage280Suffix".Localized());

                    foreach (var row in player.BreastRows)
                        if (row.BreastRating < 1)
                            row.BreastRating = 1;

                    BoostLactationUntil(player, 1.5f);

                    return true;
                }

                case 216:
                {
                    context.Write("Pregnancy.SandWitch.Stage216".Localized());

                    foreach (var row in player.BreastRows)
                        if (row.BreastRating < 20)
                            row.BreastRating += 1;
                        else
                            row.BreastRating += 0.5f;

                    BoostLactationUntil(player, 1.5f);

                    return true;
                }

                case 180:
                    context.Write("Pregnancy.SandWitch.Stage180".Localized());

                    BoostLactationUntil(player, 2f);

                    return true;

                case 120:
                    context.Write("Pregnancy.SandWitch.Stage120".Localized());

                    BoostLactationUntil(player, 3f);

                    return true;

                case 72:
                    context.Write("Pregnancy.SandWitch.Stage72".Localized());

                    BoostLactationUntil(player, 4.5f);

                    return true;

                case 48:
                    context.Write("Pregnancy.SandWitch.Stage48".Localized());

                    BoostLactationUntil(player, 5.5f);

                    return true;
            }

            return false;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            context.Write("Pregnancy.SandWitch.BirthBanner".Localized());

            if (player.Vaginas.Count == 0)
            {
                context.Write("Pregnancy.SandWitch.BirthVaginaAppears".Localized());

                player.CreateVagina();
            }

            context.Write("Pregnancy.SandWitch.BirthWaterBreaks".Localized());

            context.Write("Pregnancy.SandWitch.BirthPush".Localized());

            context.Write("Pregnancy.SandWitch.BirthGirl".Localized());

            context.Write("Pregnancy.SandWitch.BirthNurse".Localized());

            context.Write("Pregnancy.SandWitch.BirthDone".Localized());

            context.Write("Pregnancy.SandWitch.BirthNursery".Localized());

            player.ChangeFatigue(40);
        }

        public static void BoostLactationUntil(Player player, float target)
        {
            for (var guard = 0; guard < 64 && player.BiggestLactation() < target; guard++)
                player.BoostLactation(0.5f);
        }
    }
}