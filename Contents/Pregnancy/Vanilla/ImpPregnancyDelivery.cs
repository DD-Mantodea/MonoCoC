using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<ImpPregnancyDelivery>("coc:delivery_imp")]
    public class ImpPregnancyDelivery : IPregnancyDelivery
    {

        public const int LoosenessTight = 1;

        public const int LoosenessGapingWide = 4;

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
                    context.Write("Pregnancy.Imp.BellyDistended".Localized());

                    displayed = true;

                    break;

                case 216:
                {
                    var text = "Pregnancy.Imp.BellyUnmistakable".Localized()

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
                    context.Write("Pregnancy.Imp.Kick".Localized());

                    displayed = true;

                    break;

                case 120:
                    context.Write("Pregnancy.Imp.EverGrowing".Localized());

                    displayed = true;

                    break;

                case 72:
                {
                    var text = "Pregnancy.Imp.PainfullyDistended".Localized()

                        + CorBranch(
                            "Pregnancy.Mouse.HardToFunction".Localized(),

                            "Pregnancy.Mouse.WonderHowLong".Localized(),

                            "Pregnancy.Imp.EagerToBirth".Localized())

                        + "[pg]";

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
                    var text = "Pregnancy.Mouse.RubBelly".Localized()

                        + CorBranch(
                            "Pregnancy.Mouse.Disgusted".Localized(),

                            "Pregnancy.Mouse.FewDays".Localized(),

                            "Pregnancy.Imp.Daydream".Localized())

                        + "[pg]";

                    context.Write(text);

                    displayed = true;

                    break;
                }
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            Scenes.Scene.ShowImage("birth-imp");

            if (player.HasStatusEffect(StatusEffectIds.BirthedImps))
                player.ChangeStatusValue(StatusEffectIds.BirthedImps, 0, 1);
            else
                player.CreateStatusEffect(StatusEffectIds.BirthedImps, 1, 0, 0, 0);

            PregnancyUtils.CreateVaginaIfMissing(context);

            context.Write("Pregnancy.Imp.WaterBreaks".Localized()

                + (player.Corruption < 50 ? "Pregnancy.Imp.RueTheDay".Localized() : "")

                + "Pregnancy.Imp.DeliveryPleasure".Localized());

            var vagina = player.Vaginas.Count > 0 ? player.Vaginas[0] : null;

            if (vagina != null)
            {
                if (vagina.Looseness == LoosenessTight)
                    vagina.Looseness += 1;

                if (vagina.Looseness < LoosenessGapingWide && Random.Shared.Next(2) == 0)
                {
                    vagina.Looseness += 1;

                    context.Write("Pregnancy.Imp.PermanentlyEnlarged".Localized());
                }

                if (vagina.Wetness <= 0)
                    vagina.Wetness += 1;
            }

            context.Write("Pregnancy.Imp.WakeTracks".Localized());

            var average = player.AverageLactation();

            if (average > 0 && average < 5)
            {
                context.Write("Pregnancy.Mouse.MilkDribbles".Localized());

                player.BoostLactation(0.5f);
            }

            var biggestTit = player.BiggestBreastSize();

            var rows = player.MostBreastsPerRow();

            if (biggestTit >= 3 && rows > 1 && average == 0)
            {
                context.Write("Pregnancy.Imp.SlowlyLactating".Localized());

                player.BoostLactation(1f);
            }

            player.BoostLactation(0.01f);

            if (biggestTit == 2 && rows > 1)
            {
                context.Write("Pregnancy.Imp.GrewToCCups".Localized());

                player.GrowTits(1, 1, false, 3);
            }

            if (biggestTit == 1 && rows > 1)
            {
                context.Write("Pregnancy.Imp.GrewToBCups".Localized());

                player.GrowTits(1, 1, false, 3);
            }

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Toughness, -2f),

                new DynStat(GameStats.Speed, 2f),

                new DynStat(GameStats.Libido, 1f),

                new DynStat(GameStats.Sensitivity, 0.5f),

                new DynStat(GameStats.Corruption, 7f));

            var butt = player.Body.Get<ButtPart>();

            if (butt.Rating < 10 && Random.Shared.Next(2) == 0)
            {
                butt.Rating += 1;

                context.Write("Pregnancy.Mouse.ButtGrew".Localized(player.ButtDescript()));
            }
            else
            {
                var hips = player.Body.Get<HipsPart>();

                if (hips.Rating < 10)
                {
                    hips.Rating += 1;

                    context.Write("Pregnancy.Imp.HipsWider".Localized(player.ArmorName, player.HipDescript()));
                }
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