using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<HellhoundPregnancyDelivery>("coc:delivery_hellhound")]
    public class HellhoundPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            switch (context.Incubation)
            {
                case 290:
                    context.Write("Pregnancy.Mouse.BellySlightlyLarger".Localized());

                    displayed = true;

                    break;

                case 240:
                    context.Write("Pregnancy.Mouse.BellyDistended".Localized());

                    displayed = true;

                    break;

                case 216:
                {
                    var text = "Pregnancy.Mouse.BellyUnmistakable".Localized()

                        + CorBranch(
                            "Pregnancy.Mouse.JojoLowCor".Localized(),

                            "Pregnancy.Mouse.JojoMidCor".Localized(),

                            "Pregnancy.Mouse.JojoHighCor".Localized())

                        + "[pg]";

                    context.Write(text);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 2f));

                    displayed = true;

                    break;
                }

                case 180:
                    context.Write("Pregnancy.Hellhound.StrangeHeat".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, -1f));

                    displayed = true;

                    break;

                case 120:
                    context.Write("Pregnancy.Hellhound.HeatGrowing".Localized());

                    player.ApplyStats(new DynStat(GameStats.Toughness, -1f));

                    displayed = true;

                    break;

                case 72:
                    context.Write("Pregnancy.Hellhound.OddStrength".Localized());

                    player.ApplyStats(
                        new DynStat(GameStats.Strength, 1f),

                        new DynStat(GameStats.Toughness, 1f));

                    displayed = true;

                    break;

                case 48:
                {
                    var text = "Pregnancy.Hellhound.TwoLumps".Localized()

                        + CorBranch(
                            "Pregnancy.Mouse.HardToFunction".Localized(),

                            "Pregnancy.Mouse.WonderHowLong".Localized(),

                            "Pregnancy.Mouse.EagerToBirth".Localized())

                        + "[pg]";

                    context.Write(text);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 4f));

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

            var noFur = !player.HasFur();

            context.Write("Pregnancy.Hellhound.BirthWakes".Localized());

            PregnancyUtils.CreateVaginaIfMissing(context);

            context.Write("Pregnancy.Hellhound.BirthSteam".Localized());

            context.Write("Pregnancy.Hellhound.BirthHeads".Localized(
                noFur ? "squealing" : "yipping",

                noFur

                    ? ", and the rest of its body resembles an ordinary newborn puppy, aside from the tiny double dicks"

                    : ", but the rest of its body looks entirely human except for the double dicks, and it even has your skin color"));

            context.Write("Pregnancy.Hellhound.BirthGrows".Localized(
                noFur ? "" : ", dropping down onto four legs just before disappearing from view"));

            player.BoostLactation(0.01f);

            var average = player.AverageLactation();

            if (average > 0 && average < 5)
            {
                context.Write("Pregnancy.Hellhound.MilkDribbles".Localized());

                player.BoostLactation(0.5f);
            }

            player.CuntChange(60, true);

            if (player.Vaginas.Count > 0 && player.Vaginas[0].Wetness <= 0)
                player.Vaginas[0].Wetness += 1;

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Strength, -1f),

                new DynStat(GameStats.Toughness, -1f),

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

        public static string CorBranch(string low, string mid, string high)
        {
            var cor = CoC.Instance.Player.Corruption;

            if (cor < 40)
                return low;

            return cor < 75 ? mid : high;
        }
    }
}