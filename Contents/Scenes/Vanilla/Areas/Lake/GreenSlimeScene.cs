using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Lake
{
    [Register<GreenSlimeScene>("coc:green_slime")]
    public class GreenSlimeScene : Scene
    {

        public static string FaerieHairColor = "";

        public static string FaerieOutfit = "";

        public static readonly Dictionary<string, string> outfits = new()
{
    ["nude"] = "bare, exposed body",
    ["straps"] = "solid black stockings and leather chest-wraps",
    ["leaf"] = "leafy wrappings",
    ["petal"] = "chest-wrap and tiny skirt of vibrant petals",
};

        public override void Entry()
        {
            ServiceLowCorruption();
        }

        public void ServiceLowCorruption()
        {

            OutputText("Scenes.Greenslimescene.ServiceLowCorruption.UnableToPullYour".Localized());

            OutputText("Scenes.Greenslimescene.ServiceLowCorruption.SitNextToBlob".Localized());

            if (Player.Tallness >= 82)
                OutputText("Scenes.Greenslimescene.ServiceLowCorruption.TryGentlyWrapping".Localized());
            else
                OutputText("Scenes.Greenslimescene.ServiceLowCorruption.TryToWrapOne".Localized());

            OutputText("Scenes.Greenslimescene.ServiceLowCorruption.InSpiteOfAppearance".Localized(
                 CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
            ));

            ShowImage("greenslime-oral");

            if (Player.Tallness >= 82)
                OutputText("Scenes.Greenslimescene.ServiceLowCorruption.ThrobbingOfTheSlime".Localized(
                     CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));
            else
                OutputText("Scenes.Greenslimescene.ServiceLowCorruption.SlimeThrobbingGrows".Localized(
                     CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));

            Player.SlimeFeed();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));
        }

        public void ServiceLowCorruptionHighLust()
        {

            Player.SlimeFeed();

            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.UnableToTearAway".Localized());

            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.StraddleCreatureChest".Localized());

            if (Player.Tallness >= 82)
                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.GripHeadOfCreature".Localized());
            else
                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.RunFingerUp".Localized());

            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.SpongyAndPliable".Localized());

            if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.RubCreatureS".Localized(
                     Player.VaginaDescript(0),

                     CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                ));

                if (!Player.Vaginas[0].Virgin)
                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.FamiliarFullFeeling".Localized());

                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ParanoidAboutAllowing".Localized());

                if (Player.VaginalCapacity() <= 18)
                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.OnTopOfThat".Localized());

                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ParagraphBreak".Localized());
            }

            if (Player.CockTotal() > 0)
            {
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.OwnErectRubs".Localized());
                else
                    if (Player.Cocks[0].Thickness >= 36)
                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.OwnErectRubsAgain".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.FeelYourBeginThrob".Localized());
                else
                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.CocksThrobAgain".Localized());

                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ParagraphBreakAgain".Localized());
            }

            ShowImage("greenslime-oral");

            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ContinueToWorkThe".Localized());

            if (Player.Tallness < 82)
            {
                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.LightGreenFluid".Localized());

                if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                {
                    if (Player.Cocks.Count > 1)
                    {
                        if (Player.Cocks.Count > 0)
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ArousalBecomesUnbearable".Localized(
                                 Player.MultiCockDescriptLight(),

                                 Player.CockDescript(1)
                            ));

                        if (Player.Vaginas.Count > 0)
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.QuiversAndShares".Localized(
                                 Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ReachesItsPeak".Localized());
                    }
                    else
                        if (Player.Cocks[0].Length <= 30)
                        {
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ArousalBecomesUnbearableAgain".Localized(
                                 Player.CockDescript(0)
                            ));

                            if (Player.Vaginas.Count > 0)
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.QuiversAndSharesAgain".Localized(
                                     Player.VaginaDescript(0)
                                ));

                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ReachesItsPeakAgain".Localized());
                        }
                        else
                            if (Player.Cocks[0].Length >= 31)
                            {
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ArousalBecomesUnbearableThird".Localized());

                                if (Player.Vaginas.Count > 0)
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.QuiversAndSharesThird".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.InstinctivelyHoldOn".Localized());
                            }

                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ParagraphBreakThird".Localized());
                }
                else
                    if (Player.Gender == Gender.Female || Player.Gender == Gender.Hermaphrodite)
                    {
                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ArousalBecomesUnbearableFourth".Localized(
                             Player.VaginaDescript(0)
                        ));

                        if (Player.GetClitLength() >= 2)
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.MoanAndLeanBack".Localized(
                                 Player.ClitDescript()
                            ));
                        else
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.MoanLeanBackAgain".Localized());

                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.AtSameTimeYou".Localized());

                        if (Player.BreastRows.Count > 0 && Player.BiggestBreastSize() >= 1)
                        {
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.GaspALittle".Localized(
                                 Appearance.AllBreastsDescript(Player)
                            ));

                            if (Player.BiggestBreastSize() > 8)
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.BarelyContainsThem".Localized());
                            else
                                if (Player.BiggestBreastSize() > 3)
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.GentlyCoversThem".Localized());
                                else
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.EasilySmothersThem".Localized());

                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.FeelThingTweak".Localized());
                        }
                        else
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.GaspALittleAgain".Localized());

                        if (Player.CockTotal() > 0)
                            if (Player.CockTotal() > 1)
                            {
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.FeelYourSurge".Localized());

                                if (Player.BiggestBreastSize() == 0)
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.DelicatelyGraspHead".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.CockSurgesFull".Localized());

                                if (Player.BiggestBreastSize() == 0)
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.DelicatelyGraspHeadAgain".Localized());
                            }

                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ParagraphBreakFourth".Localized());

                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.TsunamiOf".Localized(
                             CombatController.Instance!.Context.Enemy.CockDescriptShort(0)
                        ));

                        if (Player.CockTotal() > 0)
                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.StrangledGasp".Localized(
                                     Player.MultiCockDescript()
                                ));
                            else
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.StrangledGaspAgain".Localized());

                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.OpenYourEyes".Localized());

                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ParagraphBreakFifth".Localized());
                    }

                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.AfterFewMomentsCreature".Localized());
            }
            else
                if (Player.Tallness >= 82)
                {
                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.LightGreenFluidAgain".Localized());

                    if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                    {
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ArousalBecomesUnbearableFifth".Localized(
                                 Player.CockDescript(1)
                            ));
                        else
                        {
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ArousalBecomesUnbearableSixth".Localized(
                                 Player.CockDescript(0)
                            ));

                            if (Player.Cocks[0].Id == BodyPartTypes.Human)
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.NearExactReplica".Localized());
                            else
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.DistinctlyHumanPenis".Localized());
                        }

                        if (Player.Vaginas.Count > 0)
                        {
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.IntenseSensationsRipple".Localized(
                                 Player.VaginaDescript(0)
                            ));

                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.IsntLongBefore".Localized(
                                 Player.MultiCockDescript(),

                                 Player.VaginaDescript(0)
                            ));
                        }
                        else
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.IsntLongBeforeAgain".Localized());
                    }
                    else
                        if (Player.Gender == Gender.Female || Player.Gender == Gender.Hermaphrodite)
                        {
                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ArousalUnbearable".Localized());

                            if (Player.BreastRows.Count > 0 && Player.BiggestBreastSize() >= 1)
                            {
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.BeginToGropeYourself".Localized(
                                     Appearance.AllBreastsDescript(Player),

                                     Player.VaginaDescript(0)
                                ));

                                if (Player.BiggestBreastSize() > 8)
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.HandExpandsStruggling".Localized(
                                         Appearance.AllBreastsDescript(Player)
                                    ));
                                else
                                    if (Player.BiggestBreastSize() > 3)
                                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.HandExpandsGentlyCovers".Localized(
                                             Appearance.AllBreastsDescript(Player)
                                        ));
                                    else
                                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.HandEasilySmothers".Localized(
                                             Appearance.AllBreastsDescript(Player)
                                        ));

                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.StopForAMoment".Localized());

                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.BeadOfRealization".Localized());
                            }
                            else
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.RapidlyMoistening".Localized(
                                     Player.VaginaDescript(0),

                                     Player.VaginaDescript(0)
                                ));

                            if (Player.CockTotal() > 0)
                            {
                                if (Player.BiggestBreastSize() == 0 || Player.BreastRows.Count == 0)
                                {
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.BeginToMoan".Localized(
                                         Player.VaginaDescript(0),

                                         Player.MultiCockDescript()
                                    ));

                                    if (Player.CockTotal() > 1)
                                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.PeniiWord".Localized());
                                    else
                                        OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.PenisWord".Localized());

                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.WhatComesNext".Localized());
                                }

                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.SmoothGelLike".Localized());
                            }
                            else
                            {
                                if (Player.BiggestBreastSize() == 0 || Player.BreastRows.Count == 0)
                                    OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.BeginToMoanAgain".Localized(
                                         Player.VaginaDescript(0)
                                    ));

                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.SmoothGelLikeAgain".Localized());
                            }

                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.ParagraphBreakSixth".Localized());

                            if (Player.CockTotal() > 0)
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.EvenAsItDoes".Localized(
                                     Player.MultiCockDescript()
                                ));
                            else
                                OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.EvenAsDoesAgain".Localized());

                            OutputText("Scenes.Greenslimescene.ServiceLowCorruptionHighLust.BarelyNoticeCreature".Localized());
                        }
                }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));
        }

        public void MaleRapesOoze()
        {

            OutputText("Scenes.Greenslimescene.MaleRapesOoze.NotingOwnErectness".Localized());

            OutputText("Scenes.Greenslimescene.MaleRapesOoze.ApproachAndExamine".Localized());

            if (Random.Shared.Next(2) == 0)
            {
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ShruggingDecideGo".Localized());
                else
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ShruggingDecideGoAgain".Localized());

                if (Player.Balls > 1)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.AnointingSlimeFace".Localized());
                else
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ProbingAtMouth".Localized());

                ShowImage("greenslime-rape-orally");

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.PullBackForMoment".Localized());

                var __sw0_1 = Player.Cocks[0].Id;

                if (__sw0_1 == BodyPartTypes.Human)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.TipWord".Localized());

                if (__sw0_1 == BodyPartTypes.Horse)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.FlaredTip".Localized());

                if (__sw0_1 == BodyPartTypes.Dog)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.PointedTip".Localized());

                if (__sw0_1 == new Identifier("coc", "tentacle"))
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.RoundedTip".Localized());

                if (true )
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.TipWordAgain".Localized());

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.OnInsideOfMouth".Localized());

                if (Player.Cocks[0].Length >= 12)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.DownItsThroat".Localized());

                if (Player.Cocks[0].Length >= 24)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.IntoItsTorso".Localized());

                if (Player.Cocks[0].Length >= 48)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ThroughToHips".Localized());

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.GruntApprovingly".Localized());

                if (Player.Corruption >= 60)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.WonderForMoment".Localized());
                else
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.WonderForMomentAgain".Localized());

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.ParagraphBreak".Localized());

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.ThrustWithMore".Localized());

                if (Player.Balls > 1)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.BallsSlapItsFace".Localized());

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.ArmReachesUp".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.BeginsToGrowWet".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.ShoutingBeforeLong".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.QuicklyFollowedBy".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.PullYourOutOf".Localized());
            }
            else
            {
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ShrugAtLackOf".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Greenslimescene.MaleRapesOoze.LargerWord".Localized());
                    else
                        OutputText("Scenes.Greenslimescene.MaleRapesOoze.LargestWord".Localized());

                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.AsYouGrabSlime".Localized());
                }
                else
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ShrugAtLack".Localized());

                ShowImage("greenslime-rape-anally");

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.PositionMemberAround".Localized());

                if (Player.Cocks[0].Length >= 24)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ByTimeFinallyBottom".Localized());
                else
                    if (Player.Cocks[0].Length >= 48)
                        OutputText("Scenes.Greenslimescene.MaleRapesOoze.FinallyBottomAgain".Localized());
                    else
                        if (Player.Cocks[0].Length >= 60)
                            OutputText("Scenes.Greenslimescene.MaleRapesOoze.PenisKeepsGoingDeeper".Localized());

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.SatisfiedSmileBegin".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.GrowsWetAsWork".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.AfterFewMinutes".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ExplodesIntoOrgasm".Localized());
                else
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.ExplodingInsideIt".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Greenslimescene.MaleRapesOoze.PumpJizzInto".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Greenslimescene.MaleRapesOoze.ClampDown".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));
        }

        public void FemaleRapesOoze()
        {

            Player.SlimeFeed();

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.StirringInsideYour".Localized());

            if (Player.Cocks.Count > 0)
                OutputText("Scenes.Greenslimescene.FemaleRapesOoze.GrowingWetAndGrowing".Localized(
                     Player.VaginaDescript(0)
                ));
            else
                OutputText("Scenes.Greenslimescene.FemaleRapesOoze.WetAsYouImagine".Localized());

            if (Player.BiggestBreastSize() > 8)
                OutputText("Scenes.Greenslimescene.FemaleRapesOoze.GrinAndLeanForward".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));
            else
                if (Player.BiggestBreastSize() > 3)
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.SmileMischievouslyLeanDown".Localized());
                else
                    if (Player.BiggestBreastSize() >= 0)
                        OutputText("Scenes.Greenslimescene.FemaleRapesOoze.PullMemberTight".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.CannotTakeThe".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.Vaginas[0].Virgin)
            {
                if (Player.Corruption <= 20)
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.HesitateForMoment".Localized());
                else
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.SmileAsYouRam".Localized());
            }
            else
                if (Player.VaginalCapacity() >= 18)
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.EnthusiasticallyLower".Localized());
                else
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.HesitateAtSheerSize".Localized());

            ShowImage("greenslime-rape-vagoo");

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.CockThoughRigid".Localized(
                 Player.VaginaDescript(0)
            ));

            if (Player.CockTotal() > 0)
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.GrowsHardAsFucked".Localized());
                else
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.CockGrowsHard".Localized());

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.ParagraphBreak".Localized());

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.SlimePumpsYouHard".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.CollapseEntirelyResting".Localized());

            if (Player.CockTotal() > 0)
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.ExplodeSimultaneous".Localized());

                    OutputText(NumberUtils.Num2Text(Player.Cocks.Count));

                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.ThickStreamsOfSemen".Localized());
                }
                else
                    OutputText("Scenes.Greenslimescene.FemaleRapesOoze.ExplodesIntoOrgasmReleases".Localized());

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.RushOfLiquid".Localized());

            if (!Player.Vaginas[0].Virgin)
                OutputText("Scenes.Greenslimescene.FemaleRapesOoze.NotThickAsSemen".Localized());

            OutputText("Scenes.Greenslimescene.FemaleRapesOoze.FillingYouPastFull".Localized(
                 Player.VaginaDescript(0)
            ));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));
        }

        public void OozeButtRapesYou()
        {

            Player.SlimeFeed();

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.CollapseUnderBeating".Localized());

            if (Player.Vaginas.Count > 0)
                OutputText("Scenes.Greenslimescene.OozeButtRapesYou.VaginaAndAsshole".Localized(
                     Player.VaginaDescript(0)
                ));
            else
                if (Player.Balls >= 2)
                    OutputText("Scenes.Greenslimescene.OozeButtRapesYou.AssholeAndBalls".Localized());
                else
                    OutputText(Player.AssholeDescript());

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.BeforeSettlingOverRear".Localized());

            if (Player.AnalCapacity() < 18 || Player.Anal.Looseness == 0)
                if (Player.Corruption <= 50)
                    OutputText("Scenes.Greenslimescene.OozeButtRapesYou.BitOfFear".Localized());
                else
                    if (Player.Corruption < 100)
                        OutputText("Scenes.Greenslimescene.OozeButtRapesYou.InSpiteOfWorries".Localized());
                    else
                        OutputText("Scenes.Greenslimescene.OozeButtRapesYou.AlmostLickLips".Localized());

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.GrimaceSlightly".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.ParagraphBreak".Localized());

            ShowImage("greenslime-loss-butt");

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.DeliberateSlowness".Localized());

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.BeforeLongMoaning".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Greenslimescene.OozeButtRapesYou.PangsOfPleasure".Localized());

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.DickShiftsContinually".Localized());

            if (Player.CockTotal() > 0)
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Greenslimescene.OozeButtRapesYou.QuicklyGrowErect".Localized());
                else
                    OutputText("Scenes.Greenslimescene.OozeButtRapesYou.YourQuicklyGrowsErect".Localized(
                         Player.MultiCockDescript()
                    ));

            if (Player.Vaginas.Count > 0)
                OutputText("Scenes.Greenslimescene.OozeButtRapesYou.GrowsWetAsCreature".Localized(
                     Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.BeginsToThrobMore".Localized());

            if (Player.Vaginas.Count > 0)
            {
                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Greenslimescene.OozeButtRapesYou.FinallyPlungesPair".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Greenslimescene.OozeButtRapesYou.FinallyPlungesPairAgain".Localized(
                         Player.VaginaDescript(0)
                    ));
            }
            else
                if (Player.CockTotal() > 0)
                    OutputText("Scenes.Greenslimescene.OozeButtRapesYou.AdditionalPressureProstate".Localized());

            OutputText("Scenes.Greenslimescene.OozeButtRapesYou.ShudderBeneathCreature".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));
        }

        public void OozeRapesYouOrally()
        {

            Player.SlimeFeed();

            OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.CollapseUnderBeating".Localized());

            if (Player.Corruption < 20)
                OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.YouBlushAs".Localized());

            OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.LargeErectionShaped".Localized());

            ShowImage("greenslime-oral");

            if (Player.Corruption >= 70)
            {
                OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.LickTheTip".Localized());

                OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.PistonsHeadOfIts".Localized());

                if (Player.CockTotal() > 0)
                    if (Player.LustPercent > 40)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.ErectThrobs".Localized());
                    else
                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.CocksBeginHarden".Localized());

                if (Player.Vaginas.Count > 0)
                {
                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.YourVaginaWord".Localized(
                         Player.VaginaDescript(0)
                    ));

                    if (Player.CockTotal() > 0)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.AlsoWord".Localized());

                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.GrowsWetAsWork".Localized());

                    if (Player.CockTotal() < 1 || Player.LustPercent <= 40)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.TryReachYourself".Localized());
                }

                OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.SlimeLetsItselfRest".Localized());

                OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.FinalStrongThrust".Localized());

                if (Player.LustPercent > 40)
                {
                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.WithdrawsAsYouBegin".Localized());

                    Player.Orgasm("Lips");
                }
                else
                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.WithdrawsWhileYoure".Localized());

                Player.RefillHunger(35);
            }
            else
                if (Player.Corruption >= 20)
                {
                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.MinorAttempts".Localized());

                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.PullsOutJustSuddenly".Localized());

                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.WithoutWarningLeansForward".Localized());

                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.BeastMovesOff".Localized());

                    Player.RefillHunger(15);

                    Player.Orgasm("Lips", false);
                }
                else
                {
                    OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.CloseMouthTightly".Localized());

                    if (Player.Intelligence > Player.Toughness)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.ForceYourselfToKeep".Localized());
                    else
                    {
                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.ForceToKeepAgain".Localized());

                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.AsSoonAsSteadied".Localized());

                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.SquirmUnderSlime".Localized());

                        OutputText("Scenes.Greenslimescene.OozeRapesYouOrally.BeastMovesOffAgain".Localized());
                    }

                    Player.RefillHunger(35);

                    Player.Orgasm("Lips", false);
                }

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 10f));
        }

        public void OozeRapesYouVaginally()
        {

            Player.SlimeFeed();

            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.CollapseUnderBeating".Localized());

            if (Player.Corruption <= 20)
            {
                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.FearQuicklyDisappears".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.BeginToFeelTingle".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Virgin)
                    OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.SlowlyPressesAgainst".Localized());
                else
                    if (Player.VaginalCapacity() < 18)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.SlowlyPressesAgainstAgain".Localized(
                             Player.VaginaDescript(0)
                        ));
                    else
                        if (Player.VaginalCapacity() <= 30)
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.SlowlyPressesAgainstThird".Localized(
                                 Player.VaginaDescript(0),

                                 Player.VaginaDescript(0)
                            ));
                        else
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.SlowlyPressesAgainstFourth".Localized(
                                 Player.VaginaDescript(0),

                                 Player.VaginaDescript(0)
                            ));

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, false, true);

                ShowImage("greenslime-loss-vag");

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.ArchYourBack".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.CockTotal() > 0)
                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.RapidlyGrowsErect".Localized(
                             Player.MultiCockDescript()
                        ));

                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.SwitchingBetweenCocks".Localized());
                        else
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.RapidlyStrokingOne".Localized());
                    }
                    else
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.RapidlyGrowsErectAgain".Localized(
                             Player.CockDescript(0)
                        ));

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.AfterShortTimeCreature".Localized());

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.DoesntTakeMuch".Localized());

                if (Player.CockTotal() > 0)
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.ClimaxesAsWellSending".Localized(
                             Player.MultiCockDescript()
                        ));
                    else
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.MemberClimaxesAsWell".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));
            }
            else
            {
                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.FearQuicklyDisappearsAgain".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.FeelTingleAgain".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.Vaginas[0].Virgin)
                {
                    OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.SlowlyPressesAgainstFifth".Localized());

                    Player.CuntChange(15, true);
                }
                else
                    if (Player.VaginalCapacity() < 18)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.FinallyBeginsToPenetrate".Localized(
                             Player.VaginaDescript(0)
                        ));
                    else
                    {
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.FinallyBeginsPenetrateAnd".Localized());

                        if (Player.VaginalCapacity() >= 30)
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.AtLeastPartly".Localized());

                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.DeepThroatyMoan".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        if (Player.VaginalCapacity() >= 30)
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.AlmostPerfectlyWord".Localized());
                        else
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.PerfectlyWord".Localized());
                    }

                ShowImage("greenslime-loss-vag");

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.ArchBackPleasure".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.CockTotal() > 0)
                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.RapidlyGrowsErectThird".Localized(
                             Player.MultiCockDescript()
                        ));

                        if (Player.CockTotal() > 2)
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.SwitchingBetweenCocksAgain".Localized());
                        else
                            OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.RapidlyStrokingOneAgain".Localized());
                    }
                    else
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.RapidlyGrowsErectFourth".Localized(
                             Player.CockDescript(0)
                        ));

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.AfterShortTime".Localized());

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.DoesntTakeMuchAgain".Localized());

                if (Player.CockTotal() > 0)
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.ClimaxesAsWell".Localized(
                             Player.MultiCockDescript()
                        ));
                    else
                        OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.OwnMemberFinallyReaches".Localized());

                OutputText("Scenes.Greenslimescene.OozeRapesYouVaginally.PantAsCreatureHolds".Localized());

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));
            }
        }

        public void TurnIntoGreenSlime()
        {

            Player.Orgasm("Vaginal");

            OutputText("Scenes.Greenslimescene.TurnIntoGreenSlime.CollapseUnderBeating".Localized());

            OutputText("Scenes.Greenslimescene.TurnIntoGreenSlime.SlimeWrapsArms".Localized());

            OutputText("Scenes.Greenslimescene.TurnIntoGreenSlime.BeginToFeelTingle".Localized(Player.VaginaDescript(0), Player.VaginaDescript(0)));

            OutputText("Scenes.Greenslimescene.TurnIntoGreenSlime.ArchYourBack".Localized());

            OutputText("Scenes.Greenslimescene.TurnIntoGreenSlime.DoesntTakeMuch".Localized());

            OutputText("Scenes.Greenslimescene.TurnIntoGreenSlime.MurkyCumFloods".Localized());

            Player.CreateCock(18, 2, BodyPartTypes.Human);

            Player.Body.Get<SkinPart>().Tone = "green";

            Player.Body.Get<HairPart>().Color = "green";

            Player.RemoveVagina(0, 1);

            Player.ModTone(50, 20);

            Player.ModThickness(50, 20);

            Player.ModFem(30, 100);

            if (Player.BiggestBreastSize() >= 1)
            {
                if (Player.BreastRows.Count > 1)
                    while (Player.BreastRows.Count > 1)
                    {
                        Player.RemoveBreastRow(Player.BreastRows.Count - 1, 1);
                    }

                Player.BreastRows[0].BreastRating = 0;

                Player.BreastRows[0].NipplesPerBreast = 1;

                Player.BreastRows[0].Fuckable = false;

                if (Player.NippleLength > .5f)
                    Player.NippleLength = .25f;

                Player.BreastRows[0].Lactation = 0;

                Player.RemoveStatusEffect(StatusEffectIds.Feeder);

                Player.RemovePerk(PerkIds.Feeder);
            }

            OutputText("Scenes.Greenslimescene.TurnIntoGreenSlime.OverwhelmedByChange".Localized());
        }

        public void RapeOozeWithMilk()
        {
            ClearOutput();

            ShowImage("greenslime-rape-orally");

            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.LookOverOoze".Localized(
                 Player.ClothedOrNaked("removing the top of your [armor] and "),

                 Player.BreastDescript(0),

                 Player.NippleDescript(0)
            ));

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.CaressingYourCocks".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.SlippingFurtherDown".Localized());
                else
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.PeriodSpace".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.ManipulatingYourClit".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.CaressingYourCocksAgain".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.SlippingFurtherDownAgain".Localized());
                        else
                            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.AlmostDrippingDown".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.CaressingBlankSpot".Localized(
                             Player.ClothedOrNaked(" despite your [armor] still being in place")
                        ));

                        OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.ParagraphBreak".Localized());

                        OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.BarrageOfFeelings".Localized());
                    }

            if ((int)Player.Gender < 1)
                OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.GrippingGroundAsSlime".Localized());
            else
                if (Player.Gender == Gender.Male)
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.HandsGripGround".Localized());
                else
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.HandsFlyToYour".Localized(
                         Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.CumManyTimesInto".Localized());

            if (Player.Gender == Gender.Male)
            {
                OutputText(Player.MultiCockDescriptLight());

                if (Player.Balls > 0)
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.AndBallsWord".Localized());

                OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.AndWord".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.ClitAndWord".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        OutputText(Player.MultiCockDescriptLight());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.CommaBallsComma".Localized());

                        OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.AndClitAndWord".Localized());
                    }

            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.BreastsPeriodBreak".Localized());

            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.UnfortunatelySeemToHave".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.FeverishlyJackingOff".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.RapidlyEnteringInOut".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.FeverishlyJackingOffAgain".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));
                    else
                        OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.RubbingItOverYour".Localized());

            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.UsingRestOfIt".Localized());

            OutputText("Scenes.Greenslimescene.RapeOozeWithMilk.NowEmptyLeaveOoze".Localized());

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Libido, .2f));

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

            Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void SlimeVictory()
        {

            OutputText("Scenes.Greenslimescene.SlimeVictory.SmileInSatisfaction".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Feeder))
            {
                if (Player.Lust >= 33 && (int)Player.Gender > 0)
                {
                    OutputText("Scenes.Greenslimescene.SlimeVictory.HornyEnoughToTry".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Breastfeed".Localized(), RapeOozeWithMilk);

                    AddButton(1, "Common.Button.Rape".Localized(), SlimeVictoryRape);

                    if (Player.Armor?.Id == ArmorIds.NaughtyNunsHabit)
                        AddButton(2, "Scenes.Akbal.AkbalDefeated.ButtonWorshipCock".Localized(), () => Contents.Items.Behaviors.Vanilla.NaughtyNunsHabitBehavior.CockWorship());

                    if (Player.HasMultiTails())
                        AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

                    AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
                }
                else
                {
                    OutputText("Scenes.Greenslimescene.SlimeVictory.NipplesAcheWithDesire".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), () => RapeOozeWithMilk()); AddButton(1, "No", () => CombatScene.CleanupAfterCombat());
                }
            }
            else
                if (Player.Lust >= 33 && (int)Player.Gender > 0)
                {
                    OutputText("Scenes.Greenslimescene.SlimeVictory.SadlyRealizeOwnNeeds".Localized());

                    AddButton(0, "Common.Button.FuckIt".Localized(), SlimeVictoryRape);

                    if (Player.Armor?.Id == ArmorIds.NaughtyNunsHabit)
                        AddButton(1, "Scenes.Akbal.AkbalDefeated.ButtonWorshipCock".Localized(), () => Contents.Items.Behaviors.Vanilla.NaughtyNunsHabitBehavior.CockWorship());

                    AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
                }
                else
                    CombatScene.CleanupAfterCombat();
        }

        public void SlimeVictoryRape()
        {
            ClearOutput();

            if (Player.Corruption <= 33)
            {
                if (Player.LustPercent < 60)
                    ServiceLowCorruption();
                else
                    ServiceLowCorruptionHighLust();
            }
            else
                if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                    MaleRapesOoze();
                else
                    if ((int)Player.Gender > 0)
                        FemaleRapesOoze();

            CombatScene.CleanupAfterCombat();

            return ;
        }

        public void SlimeLoss()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.BonusItemAfterCombatId) == ConsumableIds.RubberBall.Path)
                CoC.Instance.Flags.Set(GameFlagIds.BonusItemAfterCombatId, "");

            PlayerMenu();

            int temp = 0;

            if (Player.Gender == Gender.Female && Player.IsGoo())
                TurnIntoGreenSlime();
            else
                if (Player.Gender == Gender.Female || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                {
                    temp = Random.Shared.Next(3);

                    if (temp == 0)
                        OozeRapesYouVaginally();

                    if (temp == 1)
                        OozeRapesYouOrally();

                    if (temp == 2)
                        OozeButtRapesYou();
                }
                else
                {
                    temp = Random.Shared.Next(2);

                    if (temp == 0)
                        OozeRapesYouOrally();

                    if (temp == 1)
                        OozeButtRapesYou();
                }

            CombatScene.CleanupAfterCombat();

            return ;
        }

    }
}