using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<AnemonePregnancyDelivery>("coc:delivery_anemone")]
    public class AnemonePregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context)
        {
            var player = context.Player;

            var displayed = false;

            switch (context.Incubation)
            {
                case 240:
                    context.Write("Pregnancy.Anemone.MightBePregnant".Localized());

                    displayed = true;

                    break;

                case 210:
                    context.Write("Pregnancy.Anemone.Fluttering".Localized());

                    player.ApplyStats(new DynStat(GameStats.Lust, 5f + player.Libido / 20f));

                    displayed = true;

                    break;

                case 185:
                {
                    var text = "Pregnancy.Anemone.Unmistakable".Localized()

                        + CorBranch(
                            "Pregnancy.Mouse.JojoLowCor".Localized(),

                            "Pregnancy.Anemone.MidCor".Localized(),

                            "Pregnancy.Anemone.HighCor".Localized())

                        + "[pg]";

                    context.Write(text);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 2f));

                    displayed = true;

                    break;
                }

                case 154:
                    context.Write("Pregnancy.Anemone.StrongMovement".Localized());

                    displayed = true;

                    break;

                case 120:
                {
                    var text = "Pregnancy.Anemone.SquirmingBelly".Localized();

                    if (player.HasVagina())
                        text += "Pregnancy.Anemone.KeepsAroused".Localized(player.VaginaDescript(0));

                    context.Write(text + ".</b>[pg]");

                    player.ApplyStats(new DynStat(GameStats.Lust, 10f + player.Libido / 20f));

                    displayed = true;

                    break;
                }

                case 72:
                {
                    var text = "Pregnancy.Anemone.BellyDistended".Localized()

                        + CorBranch(
                            "Pregnancy.Anemone.LowCorDistended".Localized(),

                            "Pregnancy.Mouse.WonderHowLong".Localized(),

                            "Pregnancy.Mouse.EagerToBirth".Localized())

                        + "[pg]";

                    context.Write(text);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 5f + player.Libido / 20f));

                    displayed = true;

                    break;
                }

                case 48:
                {
                    var text = "Pregnancy.Mouse.RubBelly".Localized()

                        + CorBranch(
                            "Pregnancy.Anemone.DisgustedButHorny".Localized(),

                            "Pregnancy.Anemone.HopeErotic".Localized(),

                            "Pregnancy.Anemone.Daydream".Localized())

                        + "[pg]";

                    context.Write(text);

                    player.ApplyStats(
                        new DynStat(GameStats.Libido, 1f),

                        new DynStat(GameStats.Sensitivity, 1f),

                        new DynStat(GameStats.Lust, 10f + player.Libido / 20f));

                    displayed = true;

                    break;
                }
            }

            return displayed;
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            var flags = CoC.Instance.Flags;

            Scene.ShowImage("birth-anemone");

            PregnancyUtils.CreateVaginaIfMissing(context);

            context.Write("Pregnancy.Anemone.BirthPart1".Localized(player.ArmorName, player.VaginaDescript(0)));

            context.Write("Pregnancy.Anemone.BirthPart2".Localized(player.VaginaDescript(0)));

            if (flags.Get(GameFlagIds.AnemoneKid) > 0)
            {
                context.Write("Pregnancy.Anemone.BirthKidA".Localized(player.ClitDescript(), player.VaginaDescript(0)));

                player.CuntChange(20, true, true, false);

                context.Write("Pregnancy.Anemone.BirthDoze".Localized());

                player.ClearPregnancy();

                return;
            }

            var anemoneCocks = player.CountCocksOfType("anemone");

            if (flags.Get(GameFlagIds.AnemoneKid) == 0 && anemoneCocks > 0 && player.IsPureEnough(25))
            {
                context.Write("Pregnancy.Anemone.BirthPure".Localized(
                    player.ClitDescript(), player.VaginaDescript(0)));

                context.Write("Pregnancy.Anemone.BirthPure2".Localized());

                player.CuntChange(20, true, true, false);

                context.Write("Common.Text.Pg".Localized());

                player.CreateStatusEffect(StatusEffectIds.CampAnemoneTrigger, 0, 0, 0, 0);

                player.ClearPregnancy();

                return;
            }

            if (player.CockTotal() < 10)
            {
                context.Write("Pregnancy.Anemone.BirthStalk".Localized(
                    player.ClitDescript(), player.VaginaDescript(0)));

                player.CuntChange(20, true, true, false);

                context.Write("Pregnancy.Anemone.BirthAdheres".Localized(
                    player.CockTotal() > 0 ? "Pregnancy.Anemone.BelowCocks".Localized(player.MultiCockDescriptLight()) : ""));

                context.Write("Pregnancy.Anemone.BirthStuck".Localized());

                flags.Set(GameFlagIds.EventParserEscape, 1);

                Scene.AddNextButton("Common.Button.AcceptIt".Localized(), () => Accept(context));

                Scene.AddNextButton("Common.Button.RipIt".Localized(), () => Rip(context));

                player.ClearPregnancy();

                return;
            }

            context.Write("Pregnancy.Anemone.BirthTooManyCocks".Localized(
                player.ClitDescript(), player.VaginaDescript(0)));

            player.CuntChange(20, true, true, false);

            context.Write("Pregnancy.Anemone.BirthCrowdOut".Localized(
                player.VaginaDescript(0), player.SMultiCockDesc(), player.MultiCockDescriptLight()));

            player.Orgasm("Generic");

            player.ApplyStats(
                new DynStat(GameStats.Libido, 2f),

                new DynStat(GameStats.Sensitivity, 5f));

            if (!player.HasStatusEffect(StatusEffectIds.AnemoneArousal))
                player.CreateStatusEffect(StatusEffectIds.AnemoneArousal, 0, 0, 0, 0);

            player.ClearPregnancy();

            context.Write("Pregnancy.Anemone.BirthDoze2".Localized());
        }

        public static void Accept(PregnancyContext context)
        {
            var player = context.Player;

            Scene.ClearOutput();

            context.Write("Pregnancy.Anemone.AcceptPart1".Localized());

            if (player.CockTotal() > 0)
                context.Write("Pregnancy.Anemone.AcceptHasCocks".Localized(player.MultiCockDescriptLight()));
            else
                context.Write("Pregnancy.Anemone.AcceptNoCocks".Localized());

            player.CreateCock(4 + Random.Shared.Next(3), 1.2f, BodyPartTypes.Anemone);

            context.Write("Pregnancy.Anemone.AcceptPart2".Localized(player.SMultiCockDesc()));

            if (player.CockTotal() == 1 && player.Balls == 0)
                context.Write("Pregnancy.Anemone.AcceptPressure".Localized());

            context.Write("Pregnancy.Anemone.AcceptPart3".Localized(
                player.CumQ() < 100

                    ? "Pregnancy.Anemone.Glob".Localized()

                    : player.CumQ() < 500

                        ? "Pregnancy.Anemone.Squirt".Localized()

                        : "Pregnancy.Anemone.Spray".Localized(),

                player.BiggestBreastSize() >= 1 ? player.AllBreastsDescript() : "",

                player.CockTotal() > 1 ? "Pregnancy.Anemone.AcceptMoreSquirts".Localized(player.SMultiCockDesc()) : ""));

            context.Write("Pregnancy.Anemone.AcceptPart4".Localized(
                player.VaginaDescript(0),

                WetnessWord(player.Vaginas.Count > 0 ? player.Vaginas[0].Wetness : 0)));

            player.Orgasm("Vaginal");

            player.ApplyStats(
                new DynStat(GameStats.Libido, 2f),

                new DynStat(GameStats.Sensitivity, 5f));

            player.ClearPregnancy();
        }

        public static void Rip(PregnancyContext context)
        {
            var player = context.Player;

            Scene.ClearOutput();

            context.Write("Pregnancy.Anemone.RipPart1".Localized(
                player.CountCocksOfType("anemone") > 0 ? "already " : ""));

            context.Write("Pregnancy.Anemone.RipPart2".Localized());

            Scene.AddNextButton("Common.Button.LetItDie".Localized(), () => LetItDie(context));

            Scene.AddNextButton("Common.Button.Water".Localized(), () => Water(context));
        }

        public static void LetItDie(PregnancyContext context)
        {
            Scene.ClearOutput();

            context.Write("Pregnancy.Anemone.LetItDie".Localized());

            context.Player.ClearPregnancy();
        }

        public static void Water(PregnancyContext context)
        {
            var player = context.Player;

            Scene.ClearOutput();

            context.Write("Pregnancy.Anemone.WaterPart1".Localized("Pregnancy.Anemone.HasBarrel".Localized()));

            context.Write("Pregnancy.Anemone.WaterPart2".Localized("Pregnancy.Anemone.DumpInBarrel".Localized()));

            player.CuntChange(20, true, true, false);

            player.CreateStatusEffect(StatusEffectIds.CampAnemoneTrigger, 0, 0, 0, 0);

            player.ClearPregnancy();
        }

        public static string WetnessWord(float wetness)
        {

            if (wetness < 3)
                return "Pregnancy.Anemone.WetTrickle".Localized();

            return wetness < 5

                ? "Pregnancy.Anemone.WetSquirt".Localized()

                : "Pregnancy.Anemone.WetCupful".Localized();
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