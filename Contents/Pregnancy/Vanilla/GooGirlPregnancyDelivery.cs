using MonoCoC.Attributes;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<GooGirlPregnancyDelivery>("coc:delivery_goo_girl")]
    public class GooGirlPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            if (context.Incubation == 72)
            {

                var text = "Pregnancy.GooGirl.BellySquirms".Localized();

                if (player.Corruption < 50)
                    text += "Pregnancy.GooGirl.ShakesHead".Localized();

                context.Write(text + "</b>[pg]");

                displayed = true;
            }

            if (context.Incubation is 16 or 32 or 64 or 82)
            {
                var biggestTit = player.BiggestBreastSize();

                var rows = player.MostBreastsPerRow();

                var lactation = player.BiggestLactation();

                if (biggestTit >= 3 && rows > 1 && lactation is >= 1 and < 2)
                {
                    context.Write("Pregnancy.Common.SwollenWithMilk".Localized());

                    player.BoostLactation(0.5f);

                    displayed = true;
                }

                if (biggestTit >= 3 && rows > 1 && lactation > 0 && lactation < 1)
                {
                    context.Write("Pregnancy.Common.DropsOfMilk".Localized());

                    player.BoostLactation(0.5f);

                    displayed = true;
                }

                if (biggestTit >= 3 && rows > 1 && lactation == 0)
                {
                    context.Write("Pregnancy.Common.FullAndLactating".Localized());

                    player.BoostLactation(1f);

                    displayed = true;
                }

                if (biggestTit == 2 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToCCups".Localized());

                    player.GrowTits(1, 1, false, 3);

                    displayed = true;
                }

                if (biggestTit == 1 && rows > 1)
                {
                    context.Write("Pregnancy.Common.GrewToBCups".Localized());

                    player.GrowTits(1, 1, false, 3);

                    displayed = true;
                }
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Lake.GooGirlScene>.Get().GooPregVagBirth();
        }
    }
}