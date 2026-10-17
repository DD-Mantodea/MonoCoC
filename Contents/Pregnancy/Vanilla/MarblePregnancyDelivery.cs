using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Pregnancy.Vanilla
{

    [Register<MarblePregnancyDelivery>("coc:delivery_marble")]
    public class MarblePregnancyDelivery : IPregnancyDelivery
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
                    var text = "Pregnancy.Marble.BellyOverswollen".Localized()

                        + CorBranch(
                            "Pregnancy.Mouse.HardToFunction".Localized(),

                            "Pregnancy.Mouse.WonderHowLong".Localized(),

                            "Pregnancy.Marble.EagerToBirth".Localized())

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
                    context.Write("Pregnancy.Marble.RubBelly".Localized());

                    displayed = true;

                    break;

                case 85:
                    WriteLatePregnancy(context, player);

                    displayed = true;

                    break;
            }

            if (context.Incubation is 32 or 150)
            {
                displayed = true;

                LactationTick(context, player);
            }

            return displayed;
        }

        public static void WriteLatePregnancy(PregnancyContext context, Player player)
        {
            var text = "Pregnancy.Marble.LatePrefix".Localized();

            var biggestTit = player.BiggestBreastSize();

            var row = player.Body.Get<BreastRowPart>();

            if (biggestTit <= 0)
            {
                text += "Pregnancy.Marble.LateFlat".Localized();

                row.BreastRating = 3;
            }
            else if (biggestTit <= 1 && player.Mf("m", "f") == "f")
            {
                text += "Pregnancy.Marble.LateSmallFemale".Localized();

                row.BreastRating = 3;
            }
            else if (biggestTit <= 10)
            {
                text += "Pregnancy.Marble.LateGrowing".Localized();

                row.BreastRating += 1;
            }
            else
                text += "Pregnancy.Marble.LateOdd".Localized();

            if (player.BiggestLactation() < 1)
            {
                text += "Pregnancy.Marble.LateStartLactating".Localized();

                player.BoostLactation(player.BreastRows.Count);
            }
            else
            {
                text += "Pregnancy.Marble.LateMoreMilk".Localized();

                player.BoostLactation(player.BreastRows.Count * 0.25f);
            }

            context.Write(text + "</b>[pg]");
        }

        public void Deliver(PregnancyContext context)
        {
            var player = context.Player;

            var flags = CoC.Instance.Flags;

            Scenes.Scene.ShowImage("birth-cowgirl");

            player.BoostLactation(0.01f);

            PregnancyUtils.CreateVaginaIfMissing(context);

            if (flags.Get(GameFlagIds.MarbleNurseryConstruction) < 100)
            {

                context.Write("Pregnancy.Marble.BirthNoNursery1".Localized());

                context.Write("Pregnancy.Marble.BirthNoNursery2".Localized(player.ChestDesc(), player.NippleDescript(0)));

                player.CuntChange(20, true, true, false);

                context.Write("Pregnancy.Marble.BirthNoNursery3".Localized());

                context.Write("Pregnancy.Marble.BirthNoNursery4".Localized());

                context.Write("[pg]");

                return;
            }

            context.Write("Pregnancy.Marble.BirthHasNursery".Localized(player.VaginaDescript(0)));

            var isBoy = flags.Get(GameFlagIds.MarblePurified) > 0 && Random.Shared.Next(2) == 0;

            if (isBoy)
            {
                context.Write("Pregnancy.Marble.BoyHead".Localized());

                context.Write("Pregnancy.Marble.BoyBody".Localized());

                context.Write("Pregnancy.Marble.BoySuckle".Localized());

                context.Write("Pregnancy.Marble.BoyNursery".Localized());

                context.Write("Pregnancy.Marble.BoyGrows".Localized());

                var hipsBoy = player.Body.Get<HipsPart>();

                if (hipsBoy.Rating < 10)
                {
                    hipsBoy.Rating += 1;

                    context.Write("Pregnancy.Imp.HipsWider".Localized(player.ArmorName, player.HipDescript()));
                }

                if (flags.Get(GameFlagIds.MarbleBoys) == 0)
                    context.Write("Pregnancy.Marble.BoyFirstTime".Localized());

                flags.Set(GameFlagIds.MarbleKids, flags.Get(GameFlagIds.MarbleKids) + 1);

                flags.Set(GameFlagIds.MarbleBoys, flags.Get(GameFlagIds.MarbleBoys) + 1);
            }
            else
            {
                player.CuntChange(20, true, true, false);

                context.Write("Pregnancy.Marble.GirlHead".Localized());

                context.Write("Pregnancy.Marble.GirlBody".Localized());

                if (player.StatusEffectValue(StatusEffectIds.Marble, 3) >= 20)
                    context.Write("Pregnancy.Marble.GirlAddicted".Localized(player.NippleDescript(0)));
                else
                    context.Write("Pregnancy.Marble.GirlSuckle".Localized());

                context.Write("Pregnancy.Marble.GirlGrows".Localized());

                flags.Set(GameFlagIds.MarbleKids, flags.Get(GameFlagIds.MarbleKids) + 1);

                var hipsGirl = player.Body.Get<HipsPart>();

                if (hipsGirl.Rating < 10)
                {
                    hipsGirl.Rating += 1;

                    context.Write("Pregnancy.Imp.HipsWider".Localized(player.ArmorName, player.HipDescript()));
                }
            }

            context.Write("[pg]");
        }

        public static void LactationTick(PregnancyContext context, Player player)
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

        public static string CorBranch(string low, string mid, string high)
        {
            var cor = CoC.Instance.Player.Corruption;

            if (cor < 40)
                return low;

            return cor < 75 ? mid : high;
        }
    }
}