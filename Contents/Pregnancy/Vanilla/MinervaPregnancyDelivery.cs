using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<MinervaPregnancyDelivery>("coc:delivery_minerva")]
    public class MinervaPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            switch (context.Incubation)
            {
                case 216:
                    context.Write("Pregnancy.Minerva.BellySlightlyLarger".Localized());

                    displayed = true;

                    break;

                case 144:
                    context.Write("Pregnancy.Minerva.BellyDistended".Localized());

                    displayed = true;

                    break;

                case 72:
                    context.Write("Pregnancy.Minerva.BellyTwins".Localized());

                    displayed = true;

                    break;

                case 24:
                    context.Write("Pregnancy.Minerva.BellyFull".Localized());

                    displayed = true;

                    break;
            }

            if (context.Incubation is 72 or 85 or 144 or 150)
            {
                var biggestTit = player.BiggestBreastSize();

                var rows = player.MostBreastsPerRow();

                var lactation = player.BiggestLactation();

                if (biggestTit >= 3 && rows > 1 && lactation is >= 1 and < 2)
                {
                    context.Write("Pregnancy.Minerva.SwollenWithMilk".Localized());

                    player.BoostLactation(0.5f);
                }

                if (biggestTit >= 3 && rows > 1 && lactation > 0 && lactation < 1)
                {
                    context.Write("Pregnancy.Common.DropsOfMilk".Localized());

                    player.BoostLactation(0.5f);
                }

                if (biggestTit >= 3 && rows > 1 && lactation == 0)
                {
                    context.Write("Pregnancy.Common.FullAndLactating".Localized());

                    player.BoostLactation(1f);
                }

                if (biggestTit == 2 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToCCups".Localized());

                    player.GrowTits(1, 1, false, 3);
                }

                if (biggestTit == 1 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToBCups".Localized());

                    player.GrowTits(1, 1, false, 3);
                }
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            PregnancyUtils.CreateVaginaIfMissing(context);

            Scenes.SceneInstance<MinervaPurificationScene>.Get().PlayerGivesBirth();

            var hips = player.Body.Get<HipsPart>();

            if (hips.Rating < 10)
            {
                hips.Rating += 1;

                context.Write("Pregnancy.Minerva.HipsWider".Localized(player.ArmorName, player.HipDescript()));
            }

            context.Write("[pg]");
        }
    }
}