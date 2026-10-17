using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<SpiderPregnancyDelivery>("coc:delivery_spider")]
    public class SpiderPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            switch (context.Incubation)
            {
                case 399:
                    context.Write("Pregnancy.Spider.Fuller".Localized());

                    displayed = true;

                    break;

                case 275:
                    context.Write("Pregnancy.Basilisk.BellyGrumbles".Localized());

                    displayed = true;

                    break;

                case 250:
                {
                    var text = "Pregnancy.Basilisk.BellyPudgy".Localized();

                    if (player.Thickness > 60 && player.Tone < 40)
                        text += "Pregnancy.Basilisk.EvenForYou".Localized();

                    context.Write(text + "Pregnancy.Basilisk.MaybeCutBack".Localized());

                    displayed = true;

                    break;
                }

                case 216:
                    context.Write("Pregnancy.Basilisk.BellyBigger".Localized());

                    displayed = true;

                    break;

                case 180:
                    Scenes.Scene.ShowImage("spidermorph-male-loss-vag");

                    context.Write("Pregnancy.Spider.Fantasy".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 20f));

                    displayed = true;

                    break;

                case 120:
                    context.Write("Pregnancy.Basilisk.BellyNiceAndBig".Localized());

                    displayed = true;

                    break;

                case 72:
                {
                    var text = "Pregnancy.Basilisk.BellyHuge".Localized();

                    if (player.Corruption < 50)
                        text += "Pregnancy.Basilisk.ShakesHead".Localized();

                    context.Write(text + "</b>[pg]");

                    displayed = true;

                    break;
                }
            }

            if (context.Incubation is 32 or 64 or 85 or 150)
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

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Swamp.MaleSpiderMorphScene>.Get().SpiderPregVagBirth();
        }
    }
}