using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<CorruptedWitchPregnancyDelivery>("coc:delivery_corrupted_witch")]
    public class CorruptedWitchPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            if (context.Incubation == 168)
            {
                context.Write("Pregnancy.CorruptedWitch.BellySlightlyLarger".Localized());

                displayed = true;
            }

            if (context.Incubation == 96)
            {
                context.Write("Pregnancy.CorruptedWitch.BellyGrown".Localized());

                displayed = true;
            }

            if (context.Incubation == 24)
            {
                context.Write("Pregnancy.CorruptedWitch.BellyFull".Localized());

                displayed = true;
            }

            if (context.Incubation is 168 or 96)
            {
                var biggestTit = player.BiggestBreastSize();

                var rows = player.MostBreastsPerRow();

                var lactation = player.BiggestLactation();

                if (biggestTit >= 3 && rows > 1 && lactation is >= 1 and < 2)
                {
                    context.Write("Pregnancy.CorruptedWitch.SwollenWithMilk".Localized());

                    player.BoostLactation(0.5f);
                }

                if (biggestTit >= 3 && rows > 1 && lactation > 0 && lactation < 1)
                {
                    context.Write("Pregnancy.CorruptedWitch.DropsOfMilk".Localized());

                    player.BoostLactation(0.5f);
                }

                if (biggestTit >= 3 && rows > 1 && lactation == 0)
                {
                    context.Write("Pregnancy.CorruptedWitch.FullAndLactating".Localized());

                    player.BoostLactation(1f);
                }

                if (biggestTit == 2 && rows > 1)
                {
                    context.Write("Pregnancy.CorruptedWitch.GrewToCCups".Localized());

                    player.GrowTits(1, 1, false, 3);
                }

                if (biggestTit == 1 && rows > 1)
                {
                    context.Write("Pregnancy.CorruptedWitch.GrewToBCups".Localized());

                    player.GrowTits(1, 1, false, 3);
                }
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            PregnancyUtils.CreateVaginaIfMissing(context);

            Scenes.SceneInstance<CorruptedWitchScene>.Get().GiveBirthToWitches();

            var hips = player.Body.Get<HipsPart>();

            if (hips.Rating < 10)
            {
                hips.Rating += 1;

                context.Write("Pregnancy.CorruptedWitch.HipsWider".Localized(player.ArmorName, player.HipDescript()));
            }

            context.Write("[pg]");
        }
    }
}