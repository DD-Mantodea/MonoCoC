using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<MinotaurPregnancyDelivery>("coc:delivery_minotaur")]
    public class MinotaurPregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            switch (context.Incubation)
            {
                case 336:
                    context.Write("Pregnancy.Mouse.BellySlightlyLarger".Localized());

                    displayed = true;

                    break;

                case 280:
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
                    context.Write("Pregnancy.Minotaur.StrongKick".Localized());

                    displayed = true;

                    break;

                case 120:
                    context.Write("Pregnancy.Mouse.EverGrowing".Localized());

                    displayed = true;

                    break;

                case 72:
                {
                    var text = "Pregnancy.Minotaur.PainfullyDistended".Localized() + CorBranch(
                        "Pregnancy.Mouse.HardToFunction".Localized(),

                        "Pregnancy.Mouse.WonderHowLong".Localized(),

                        "Pregnancy.Mouse.EagerToBirth".Localized()) + "[pg]";

                    context.Write(text);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 4f));

                    displayed = true;

                    break;
                }

                case 48:
                {
                    var text = "Pregnancy.Minotaur.RubBelly".Localized() + CorBranch(
                        "Pregnancy.Mouse.Disgusted".Localized(),

                        "Pregnancy.Mouse.FewDays".Localized(),

                        "Pregnancy.Minotaur.Daydream".Localized()) + "[pg]";

                    context.Write(text);

                    displayed = true;

                    break;
                }
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

            Scenes.Scene.ShowImage("birth-minotaur");

            PregnancyUtils.CreateVaginaIfMissing(context);

            player.BoostLactation(0.01f);

            context.Write("Pregnancy.Minotaur.BirthPart1".Localized());

            context.Write("Pregnancy.Minotaur.BirthPart2".Localized());

            var average = player.AverageLactation();

            if (average > 0 && average < 5)
            {
                context.Write("Pregnancy.Centaur.MilkDribblesShort".Localized());

                player.BoostLactation(1f);
            }

            player.CuntChange(120, true, true, false);

            if (player.Vaginas.Count > 0 && player.Vaginas[0].Wetness <= 0)
                player.Vaginas[0].Wetness += 1;

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Strength, -1f),

                new DynStat(GameStats.Toughness, -2f),

                new DynStat(GameStats.Speed, 3f),

                new DynStat(GameStats.Libido, 1f),

                new DynStat(GameStats.Sensitivity, 0.5f));

            var butt = player.Body.Get<ButtPart>();

            if (butt.Rating < 12 && Random.Shared.Next(2) == 0)
            {
                butt.Rating += 1;

                context.Write("Pregnancy.Mouse.ButtGrew".Localized(player.ButtDescript()));
            }
            else
            {
                var hips = player.Body.Get<HipsPart>();

                if (hips.Rating < 15)
                {
                    hips.Rating += 1;

                    context.Write("Pregnancy.Imp.HipsWider".Localized(player.ArmorName, player.HipDescript()));
                }
            }

            context.Write("[pg]");

            var flags = CoC.Instance.Flags;

            flags.Set(GameFlagIds.MinotaurSonsPending, flags.Get(GameFlagIds.MinotaurSonsPending) + 1);

            if (flags.Get(GameFlagIds.MinotaurSonsGrowupCounter) == 0)
                flags.Set(GameFlagIds.MinotaurSonsGrowupCounter, 150);
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