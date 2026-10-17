using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<CentaurPregnancyDelivery>("coc:delivery_centaur")]
    public class CentaurPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            switch (context.Incubation)
            {
                case 350:
                    context.Write("Pregnancy.Centaur.BellyBigger".Localized());

                    displayed = true;

                    break;

                case 280:
                    context.Write("Pregnancy.Centaur.BellyDistended".Localized());

                    displayed = true;

                    break;

                case 216:
                    context.Write("Pregnancy.Centaur.Unmistakable".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 2f));

                    displayed = true;

                    break;

                case 180:
                    context.Write("Pregnancy.Centaur.MovingFaster".Localized());

                    displayed = true;

                    break;

                case 120:
                    context.Write("Pregnancy.Centaur.PainfullyDistended".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 0.5f),

                        new DynStat(GameStats.Sensitivity, 0.5f),

                        new DynStat(GameStats.Lust, 4f));

                    displayed = true;

                    break;

                case 72:
                    context.Write("Pregnancy.Centaur.BeachBall".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 4f));

                    displayed = true;

                    break;

                case 48:
                    context.Write("Pregnancy.Centaur.Happiest".Localized());

                    displayed = true;

                    break;
            }

            if (context.Incubation is 32 or 64 or 85 or 150)
            {
                displayed = true;

                var biggestTit = player.BiggestBreastSize();

                var rows = player.MostBreastsPerRow();

                var lactation = player.BiggestLactation();

                if (biggestTit >= 3 && rows > 1 && lactation is >= 1 and < 2)
                {
                    context.Write("Pregnancy.Mouse.SwollenWithMilk".Localized());

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

            context.Write("Pregnancy.Centaur.BirthAche".Localized());

            PregnancyUtils.CreateVaginaIfMissing(context);

            player.BoostLactation(0.01f);

            context.Write("Pregnancy.Centaur.BirthCompulsion".Localized());

            context.Write("Pregnancy.Centaur.BirthHeadFirst".Localized());

            context.Write("Pregnancy.Centaur.BirthStands".Localized());

            context.Write("Pregnancy.Centaur.BirthDrinks".Localized());

            context.Write("Pregnancy.Centaur.BirthDash".Localized());

            var average = player.AverageLactation();

            if (average > 0 && average < 5)
            {
                context.Write("Pregnancy.Centaur.MilkDribbles".Localized(player.AllBreastsDescript()));

                player.BoostLactation(0.5f);
            }

            context.Write(" ");

            player.CuntChange(100, true);

            if (player.Vaginas.Count > 0 && player.Vaginas[0].Wetness <= 0)
                player.Vaginas[0].Wetness += 1;

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Strength, -1f),

                new DynStat(GameStats.Toughness, -4f),

                new DynStat(GameStats.Speed, 2f),

                new DynStat(GameStats.Libido, 1f),

                new DynStat(GameStats.Sensitivity, 0.5f));

            var butt = player.Body.Get<ButtPart>();

            if ((butt.Rating < 14 && Random.Shared.Next(2) == 0) && (butt.Rating < 10 || (butt.Rating < 14 && Random.Shared.Next(2) == 0)))
                {
                    butt.Rating += 1;

                    context.Write("Pregnancy.Mouse.ButtGrew".Localized(player.ButtDescript()));
                }

            context.Write("[pg]");
        }
    }
}