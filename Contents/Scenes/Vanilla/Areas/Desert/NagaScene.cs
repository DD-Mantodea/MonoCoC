using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<NagaScene>("coc:naga")]
    public class NagaScene : Scene
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
            NagaEncounter();
        }

        public void NagaEncounter()
        {
            ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.Naga))
                Player.CreateStatusEffect(StatusEffectIds.Naga, 0, 0, 0, 0);

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Naga)
            {
                Player.ChangeStatusValue(StatusEffectIds.Naga, 1, 1);

                if (Player.StatusEffectValue(StatusEffectIds.Naga, 1) == 0)
                {
                    Player.ChangeStatusValue(StatusEffectIds.Naga, 2, 1);

                    OutputText("Scenes.Nagascene.NagaEncounter.WanderIntoDesertNoting".Localized());

                    OutputText("Scenes.Nagascene.NagaEncounter.AsYouGetCloser".Localized());

                    Player.UnlockCodexEntry(GameFlagIds.CodexEntryNagas, false, true);

                    OutputText("Scenes.Nagascene.NagaEncounter.WonderHowItIs".Localized());

                    OutputText("Scenes.Nagascene.NagaEncounter.RelaxALittle".Localized());
                }
                else
                {
                    OutputText("Scenes.Nagascene.NagaEncounter.SlideOverHotSand".Localized());

                    Player.UnlockCodexEntry(GameFlagIds.CodexEntryNagas, false, true);
                }

                OutputText("Scenes.Nagascene.NagaEncounter.LetsOutSoftMoan".Localized());

                if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs)
                    OutputText("Scenes.Nagascene.NagaEncounter.YourOwnWord".Localized());
                else
                    OutputText("Scenes.Nagascene.NagaEncounter.InsideOfYourMouth".Localized());

                OutputText("Scenes.Nagascene.NagaEncounter.ParagraphBreak".Localized());

                if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                {
                    if (Player.CockArea(0) > 35)
                    {
                        ShowImage("naga-desert-male-naga-get-bj");

                        OutputText("Scenes.Nagascene.NagaEncounter.KissContinuesBothOf".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.WrapsBoth".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.HissOfPleasure".Localized(
                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaEncounter.MoanInPleasure".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.AtYourLimit".Localized());

                        if (Player.CumQ() > 250)
                            OutputText("Scenes.Nagascene.NagaEncounter.CheeksBulgeWithVolume".Localized());

                        if (Player.CumQ() > 1000)
                            OutputText("Scenes.Nagascene.NagaEncounter.CheeksBulgeOutWith".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.CollapseOntoSand".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.OneLastKiss".Localized());
                    }
                    else
                    {
                        ShowImage("naga-desert-male-naga-vagsex");

                        OutputText("Scenes.Nagascene.NagaEncounter.KissContinuesAndBoth".Localized());

                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Nagascene.NagaEncounter.ItStartsWord".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaEncounter.TheyStartWord".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.GrowHardAgainst".Localized());

                        if (Player.Cocks.Count == 1)
                            OutputText("Scenes.Nagascene.NagaEncounter.GentlyStartsToStroke".Localized());

                        if (Player.Cocks.Count == 2)
                            OutputText("Scenes.Nagascene.NagaEncounter.GentlyStrokesBoth".Localized());

                        if (Player.Cocks.Count >= 3)
                            OutputText("Scenes.Nagascene.NagaEncounter.StrokesBothSlowly".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.HissOfPleasureAgain".Localized());

                        if (Player.Cocks.Count == 1)
                            OutputText("Scenes.Nagascene.NagaEncounter.CarefullyLinesItUp".Localized());

                        if (Player.Cocks.Count == 2)
                            OutputText("Scenes.Nagascene.NagaEncounter.LinesUpBoth".Localized());

                        if (Player.Cocks.Count >= 3)
                            OutputText("Scenes.Nagascene.NagaEncounter.LinesUpOneMember".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.PullsHerHipsBack".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.EveryThrustBringsDeeper".Localized());

                        if (Player.CumQ() > 250)
                            OutputText("Scenes.Nagascene.NagaEncounter.QuicklyFillHerWith".Localized());
                        else
                            if (Player.CumQ() > 1000)
                                OutputText("Scenes.Nagascene.NagaEncounter.StomachQuicklySwells".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.TwoOfYouLay".Localized());
                    }

                    Player.Orgasm("Dick");
                }
                else
                    if (Player.HasVagina())
                    {
                        ShowImage("naga-desert-female-naga-tailsex");

                        OutputText("Scenes.Nagascene.NagaEncounter.KissContinuesYouCan".Localized());

                        if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs)
                            OutputText("Scenes.Nagascene.NagaEncounter.CarefullyNibblingAtIt".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaEncounter.NibblingGentlyAtIt".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.TravelingDownPause".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.HandsMoveDownAgain".Localized());

                        if (Player.StatusEffectValue(StatusEffectIds.Naga, 1) == 0)
                            OutputText("Scenes.Nagascene.NagaEncounter.StartToUnveilHer".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.TwistFingersDeeper".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.SlideFingersOutOf".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.PlaceAFingerOnto".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.UntangleEndOfYour".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.TailTipSlides".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.SimplyRunTongueAround".Localized(
                             Player.ClitDescript()
                        ));

                        OutputText("Scenes.Nagascene.NagaEncounter.SheReturnsYourKiss".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        Player.CuntChange(30, true, false, true);

                        Player.Orgasm("Vaginal");

                        OutputText("Scenes.Nagascene.NagaEncounter.DesertMoreOften".Localized());

                        DoNext(() => ReturnToCamp(4));

                        return ;
                    }
                    else
                    {
                        ShowImage("naga-desert-genderless-give-cunning");

                        OutputText("Scenes.Nagascene.NagaEncounter.NagaLooksDownSuddenly".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.NagaReachesOutPlaces".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.EnjoyingNewShift".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.MoveTongueFasterAgain".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.GripsYourHeadTightly".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.UncoilAndSlowlyGather".Localized());

                        OutputText("Scenes.Nagascene.NagaEncounter.BeforeYouHeadOff".Localized());
                    }

                OutputText("Scenes.Nagascene.NagaEncounter.DesertMoreOftenAgain".Localized());

                DoNext(() => ReturnToCamp(1));

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.Libido / 5)));

                return ;
            }

            if (Player.StatusEffectValue(StatusEffectIds.Naga, 0) == 0)
            {
                if (!Player.HasStatusEffect(StatusEffectIds.Naga))
                    Player.CreateStatusEffect(StatusEffectIds.Naga, 0, 0, 0, 0);

                OutputText("Scenes.Nagascene.NagaEncounter.WalkingThroughShifting".Localized());

                OutputText("Scenes.Nagascene.NagaEncounter.SurpriseWhatYouSee".Localized());
            }

            if (Player.StatusEffectValue(StatusEffectIds.Naga, 0) > 0)
            {
                Player.ChangeStatusValue(StatusEffectIds.Naga, 1, 0);

                OutputText("Scenes.Nagascene.NagaEncounter.WalkInDesertFor".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryNagas, true, true);

            var enemy = EnemyRegistry.Create(EnemyIds.Naga);

            enemy.OnDefeated = hpVictory => NagaRapeChoice();

            enemy.OnWon = hpVictory => NagaFUCKSJOOOOOO();

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void GooNagaRape()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
                ShowImage("naga-desert-male-goo-win-rape");
            else
                if (Player.Gender == Gender.Female)
                    ShowImage("naga-desert-female-goo-win-rape");
                else
                    ShowImage("naga-desert-herm-goo-win-rape");

            OutputText("Scenes.Nagascene.GooNagaRape.LookOverAtProne".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 0)
                OutputText("Scenes.Nagascene.GooNagaRape.WeakWord".Localized());
            else
                OutputText("Scenes.Nagascene.GooNagaRape.FlushedWord".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.OnlyServingToArouse".Localized());

            if (Player.Corruption > 50)
            {
                if (Player.Gender == Gender.Male)
                    OutputText("Scenes.Nagascene.GooNagaRape.GrowingHarderAtThought".Localized());
                else
                    if (Player.Gender == Gender.Female)
                        OutputText("Scenes.Nagascene.GooNagaRape.CuntGrowsMoist".Localized());
                    else
                        if (Player.Gender == Gender.Hermaphrodite)
                            OutputText("Scenes.Nagascene.GooNagaRape.CockTwitchesPussy".Localized());
                        else
                            OutputText("Scenes.Nagascene.GooNagaRape.FeelATingleAt".Localized());
            }
            else
                OutputText("Scenes.Nagascene.GooNagaRape.AlmostTooHorrible".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.SlideGooeyFormOver".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.LookingDownSeeScale".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.DecideYouvePlayed".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.FeelYourselfHitA".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Nagascene.GooNagaRape.ImmediatelyAfterDecide".Localized());
            else
                OutputText("Scenes.Nagascene.GooNagaRape.NaggingVoiceAtThe".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.GrabNagaShoulders".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.PauseMomentEnjoying".Localized());

            if (Player.Cocks.Count > 0)
            {
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Nagascene.GooNagaRape.YourCockWord".Localized());
                else
                    OutputText("Scenes.Nagascene.GooNagaRape.OneOfYourCocks".Localized());

                OutputText("Scenes.Nagascene.GooNagaRape.HittingPleasureBuzzer".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Nagascene.GooNagaRape.ClitRubbingAgainst".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Nagascene.GooNagaRape.YourCockWordAgain".Localized());
                        else
                            OutputText("Scenes.Nagascene.GooNagaRape.YourCocksWord".Localized());

                        OutputText("Scenes.Nagascene.GooNagaRape.AndClitRubbingAgainst".Localized());
                    }
                    else
                        OutputText("Scenes.Nagascene.GooNagaRape.BareCrotchHitting".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.RelentlesslyPushYourself".Localized());

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Nagascene.GooNagaRape.LetOutAYell".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Nagascene.GooNagaRape.ComeAllOverNaga".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Nagascene.GooNagaRape.ComeOverNagaAgain".Localized());
                    else
                        OutputText("Scenes.Nagascene.GooNagaRape.ComeViolentlyCovering".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    OutputText("Scenes.Nagascene.GooNagaRape.ShriekInDelight".Localized());

                    if (Player.Vaginas[0].Wetness == 5)
                        OutputText("Scenes.Nagascene.GooNagaRape.GirlcumGushingOut".Localized());

                    OutputText("Scenes.Nagascene.GooNagaRape.SentencePeriod".Localized());
                }

            if (Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Nagascene.GooNagaRape.ScreamInEcstasy".Localized());

                if (Player.Vaginas[0].Wetness < 5)
                    OutputText("Scenes.Nagascene.GooNagaRape.LeakingOutWord".Localized());
                else
                    OutputText("Scenes.Nagascene.GooNagaRape.GushingOutFormPool".Localized());

                OutputText("Scenes.Nagascene.GooNagaRape.AndYourCocks".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Nagascene.GooNagaRape.SpurtingOntoStomach".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Nagascene.GooNagaRape.SpurtingStomachAndFace".Localized());
                    else
                        OutputText("Scenes.Nagascene.GooNagaRape.CompletelyCoveringNaga".Localized());
            }

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Nagascene.GooNagaRape.CryOutAndShudder".Localized());

            OutputText("Scenes.Nagascene.GooNagaRape.MomentYouJustSit".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void NagaVictoryMale()
        {
            ClearOutput();

            if (Player.Cocks.Count > 0)
                if (Player.IsTaur())
                {
                    ShowImage("naga-male-taur-win-vagsex");

                    OutputText("Scenes.Nagascene.NagaVictoryMale.MassiveBulkLoomsOver".Localized(
                         Player.CockDescript(0),

                         Player.CockDescript(0),

                         Player.CockDescript(0)
                    ));

                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Nagascene.NagaVictoryMale.CumInsideHer".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Nagascene.NagaVictoryMale.FillHerInsidesHuge".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaVictoryMale.ExplodeWithFountains".Localized());

                    OutputText("Scenes.Nagascene.NagaVictoryMale.SqueezesYouTightly".Localized());

                    OutputText("Scenes.Nagascene.NagaVictoryMale.ExhaustedFromGymnastics".Localized());
                }
                else
                    if (Player.Corruption < 50)
                    {
                        OutputText("Scenes.Nagascene.NagaVictoryMale.AlmostFeelPity".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryMale.SlowlyBeginMovingFingers".Localized());

                        if (Player.CockArea(0) <= CombatController.Instance!.Context.Enemy.VaginalCapacity())
                        {
                            ShowImage("naga-male-win-vagsex");

                            OutputText("Scenes.Nagascene.NagaVictoryMale.ChillsRunDownSpine".Localized());

                            if (Player.CumQ() < 250)
                                OutputText("Scenes.Nagascene.NagaVictoryMale.ThickRopesOfSperm".Localized());
                            else
                                if (Player.CumQ() < 500)
                                    OutputText("Scenes.Nagascene.NagaVictoryMale.PumpingLargeAmounts".Localized());
                                else
                                    OutputText("Scenes.Nagascene.NagaVictoryMale.BlastingViolentTorrent".Localized());
                        }
                        else
                        {
                            ShowImage("naga-male-win-get-bj");

                            OutputText("Scenes.Nagascene.NagaVictoryMale.DrawAwayFromLips".Localized(
                                 Player.CockDescript(0),

                                 Player.CockDescript(0),

                                 Player.CockDescript(0),

                                 Player.CockHead(),

                                 Player.CockHead()
                            ));

                            OutputText("Scenes.Nagascene.NagaVictoryMale.LeanBackAndSigh".Localized());

                            if (Player.CumQ() < 250)
                                OutputText("Scenes.Nagascene.NagaVictoryMale.FillingHerMouthWarm".Localized());
                            else
                                if (Player.CumQ() < 500)
                                    OutputText("Scenes.Nagascene.NagaVictoryMale.PumpingMouthFull".Localized());
                                else
                                    OutputText("Scenes.Nagascene.NagaVictoryMale.FloodingHerMouth".Localized());

                            OutputText("Scenes.Nagascene.NagaVictoryMale.NagaRemovesYour".Localized());

                            if (Player.CumQ() > 500)
                                OutputText("Scenes.Nagascene.NagaVictoryMale.LastStickyBlasts".Localized());

                            OutputText("Scenes.Nagascene.NagaVictoryMale.LooksToYouInstruction".Localized());

                            if (Player.CumQ() < 250)
                                OutputText("Scenes.Nagascene.NagaVictoryMale.GulpingDownWarmSperm".Localized());
                            else
                                OutputText("Scenes.Nagascene.NagaVictoryMale.BestSheCan".Localized());
                        }

                        OutputText("Scenes.Nagascene.NagaVictoryMale.SlowlyUntangleYourself".Localized());
                    }
                    else
                    {
                        Player.ChangeFatigue(20);

                        ShowImage("naga-desert-male-win-corrupted-sex");

                        OutputText("Scenes.Nagascene.NagaVictoryMale.MindFillsObscene".Localized());

                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Nagascene.NagaVictoryMale.PauseJustLongEnough".Localized(
                                 Player.CockDescript(1)
                            ));
                        else
                            OutputText("Scenes.Nagascene.NagaVictoryMale.ShoveAsMuchOf".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryMale.TryToMaintainFocus".Localized());

                        if (Player.CumQ() > 250)
                            OutputText("Scenes.Nagascene.NagaVictoryMale.SpoogeExplodesFromNaga".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaVictoryMale.ParagraphBreak".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryMale.AwakenNagaGone".Localized());

                        if (Player.CumQ() > 250)
                            OutputText("Scenes.Nagascene.NagaVictoryMale.WithTracesOfSemen".Localized());
                        else
                            if (Player.CumQ() > 2000)
                                OutputText("Scenes.Nagascene.NagaVictoryMale.ThatsFilledWithThick".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryMale.PeriodBreak".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryMale.RetrieveYourClothes".Localized());
                    }

            OutputText("Scenes.Nagascene.NagaVictoryMale.DesertMoreOften".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();

            return ;
        }

        public void NagaVictoryFemale()
        {
            ClearOutput();

            if (Player.HasVagina())
                if (Player.IsTaur())
                {
                    if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    {
                        ShowImage("naga-female-taur-win-fingering");

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.PreyFallsToThe".Localized());

                        if (Player.Gender == Gender.Hermaphrodite)
                            OutputText("Scenes.Nagascene.NagaVictoryFemale.TailGentlySqueezes".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.AddedSupportYouAre".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.BeginsToGentlyToy".Localized(
                             Player.VaginaDescript(0)
                        ));

                        Player.CuntChange(20, true, false, true);

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.RemainEntwined".Localized());

                        if (Player.Gender == Gender.Hermaphrodite)
                            OutputText("Scenes.Nagascene.NagaVictoryFemale.ConstantlyStimulatedBy".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.AccelerateFingerThrusts".Localized(
                             Player.VaginaDescript(0)
                        ));
                    }
                    else
                        OutputText("Scenes.Nagascene.NagaVictoryFemale.NoMatterHowHard".Localized());
                }
                else
                    if (Player.Corruption < 50)
                    {
                        ShowImage("naga-desert-female-win-get-tailsex");

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.LustWithinBuilds".Localized());

                        Player.CuntChange(20, true, false, true);

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.GrindingBodyAgainst".Localized());
                    }
                    else
                    {
                        if (!Player.HasStatusEffect(StatusEffectIds.MeanToNaga))
                            Player.CreateStatusEffect(StatusEffectIds.MeanToNaga, 0, 0, 0, 0);

                        ShowImage("naga-female-win-69");

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.WatchTheWrithingForm".Localized(
                             Player.ArmorDescript() != "gear" ? "begin to unfasten the lower part of your " + Player.ArmorDescript() : "present your naked body to the naga.",

                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.FaceTurningRed".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.KneelOverHerFace".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.StopAfterThirdClimax".Localized(
                             Player.VaginaDescript(0),

                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.ItDoesnTTake".Localized(
                             Player.VaginaDescript(0)
                        ));

                        if (Player.Strength > 50)
                        {
                            OutputText("Scenes.Nagascene.NagaVictoryFemale.BeginToPushHarder".Localized());

                            if (Player.Corruption > 80)
                                OutputText("Scenes.Nagascene.NagaVictoryFemale.PartingGiftYouKick".Localized());
                            else
                                OutputText("Scenes.Nagascene.NagaVictoryFemale.ThinkToYourselfBut".Localized());
                        }

                        OutputText("Scenes.Nagascene.NagaVictoryFemale.ReturnToCampSatisfied".Localized());
                    }

            OutputText("Scenes.Nagascene.NagaVictoryFemale.DesertMoreOften".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();

            return ;
        }

        public void NagaVictoryGenderless()
        {
            ClearOutput();

            if (Player.IsTaur())
            {
                ShowImage("naga-desert-genderless-win-give-cunning");

                OutputText("Scenes.Nagascene.NagaVictoryGenderless.PranceOverToProne".Localized());

                OutputText("Scenes.Nagascene.NagaVictoryGenderless.PositionOverUppermost".Localized());

                OutputText("Scenes.Nagascene.NagaVictoryGenderless.HissOfPleasureEscapes".Localized());

                OutputText("Scenes.Nagascene.NagaVictoryGenderless.SpeedUpYourLicking".Localized());

                OutputText("Scenes.Nagascene.NagaVictoryGenderless.TailOfNagaSlides".Localized());
            }
            else
                if (Player.Corruption < 50)
                {
                    ShowImage("naga-desert-genderless-win-corrupted-scissoring");

                    OutputText("Scenes.Nagascene.NagaVictoryGenderless.LookDownAtCollapsed".Localized());

                    OutputText("Scenes.Nagascene.NagaVictoryGenderless.StraddlePlacingYour".Localized());

                    OutputText("Scenes.Nagascene.NagaVictoryGenderless.NagaPussyStartsTo".Localized());

                    OutputText("Scenes.Nagascene.NagaVictoryGenderless.StandUpOverHer".Localized());
                }
                else
                {
                    if (!Player.HasStatusEffect(StatusEffectIds.MeanToNaga))
                        Player.CreateStatusEffect(StatusEffectIds.MeanToNaga, 0, 0, 0, 0);

                    if (Player.Body.Get<FacePart>().Id == BodyPartTypes.SnakeFangs)
                        ShowImage("naga-desert-genderless-win-fisting");
                    else
                        ShowImage("naga-desert-genderless-win-cunning");

                    OutputText("Scenes.Nagascene.NagaVictoryGenderless.EvilGrinCrossesFace".Localized());

                    OutputText("Scenes.Nagascene.NagaVictoryGenderless.CantGiveUp".Localized());

                    OutputText("Scenes.Nagascene.NagaVictoryGenderless.ThrustYourselfOnTop".Localized());

                    if (Player.Body.Get<FacePart>().Id != BodyPartTypes.SnakeFangs)
                    {
                        OutputText("Scenes.Nagascene.NagaVictoryGenderless.SheStartsThrusting".Localized());

                        OutputText("Scenes.Nagascene.NagaVictoryGenderless.QuicklySlithersOut".Localized());
                    }
                    else
                        OutputText("Scenes.Nagascene.NagaVictoryGenderless.ReachUpPullingHead".Localized());
                }

            OutputText("Scenes.Nagascene.NagaVictoryGenderless.DesertMoreOften".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();

            return ;
        }

        public void NagaFUCKSJOOOOOO()
        {
            ClearOutput();

            if ((Player.HasPerk(PerkIds.BimboBrains) || Player.HasPerk(PerkIds.FutaFaculties)) && Player.HasVagina())
            {
                ShowImage("nage-desert-female-bimbo-lose-tailsex");

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.FallToKneesUsual".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SlowlyApproachesYouWith".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SlowlyMoveTongueAround".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ContinueAssaultOnHer".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.AfterTheNagaManages".Localized(
                     Appearance.AllBreastsDescript(Player),

                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.DoYourVeryBest".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ThenLiesFlatOn".Localized(
                     Appearance.AllBreastsDescript(Player),

                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TailBeginsToPenetrate".Localized());

                Player.CuntChange(1, true, true, false);

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ScalySkinRubsAt".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.LactationQ() >= 50 && Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.WarmMilkLeaking".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.NagaLiftsYouFrom".Localized(
                     Player.VaginaDescript(0),

                     Appearance.AllBreastsDescript(Player)
                ));

                if (Player.LactationQ() >= 50 && Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.MouthSucksAndPlays".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TailWorksInside".Localized());

                if (Player.LactationQ() >= 50 && Player.BiggestLactation() >= 1)
                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SharesAMouthful".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TipOfHerTail".Localized());

                Player.CuntChange((Player.VaginalCapacity() + 5), true, true, false);

                Player.Orgasm("Vaginal");

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ParagraphBreak".Localized());

                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.LikeYourWishComes".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));
            }
            else
                if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                {
                    if (Player.Lust >= Player.MaxLust)
                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.FallToKneesShaking".Localized());
                    else
                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.FallToKneesBattered".Localized());

                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.HearTheHissOf".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ClampingLegsToYour".Localized());

                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.StruggleWeaklyAgainst".Localized());

                    if (Player.Lust >= Player.MaxLust)
                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.AllYourResistance".Localized());
                    else
                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TooWeakToDo".Localized());

                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.LeansTowardsGripping".Localized());

                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.OnlyThingRunningThrough".Localized());

                    if (Player.Corruption > 75)
                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ThoughtMakesYouThrob".Localized());

                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.CloseEyesAndReady".Localized(
                         Player.CockDescript(0),

                         Player.CockHead()
                    ));

                    if (Player.CockArea(0) <= CombatController.Instance!.Context.Enemy.VaginalCapacity())
                    {
                        ShowImage("naga-desert-male-lose-vagsex");

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SheStopsSuddenlyAnd".Localized(
                             Player.CockHead(),

                             Player.CockDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.FeelHandsOfThe".Localized(
                             Player.CockDescript(0),

                             Player.CockDescript(0),

                             Player.CockDescript(0),

                             Player.CockDescript(0),

                             Player.CockDescript(0)
                        ));

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.PoursWord".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SpurtsWord".Localized());
                            else
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.EruptsWord".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.IntoHerBodyShe".Localized());

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.MoansAndPantsWord".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SquealsWithJoyWord".Localized());
                            else
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ScreamsInDelightWord".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.HotGooeySperm".Localized());

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.CoatsHerWombWord".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.CompletelyFillsHerWord".Localized());
                            else
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.EngorgesHerWithExcess".Localized());
                    }
                    else
                    {
                        ShowImage("naga-male-lose-get-bj");

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ContinuesToStrokeYour".Localized(
                             Player.CockDescript(0),

                             Player.CockDescript(0)
                        ));

                        if (Player.Balls > 0)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ShiftsHandToBalls".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TongueContinuesToSlide".Localized(
                             Player.CockHead(),

                             Player.CockDescript(0),

                             Player.CockDescript(0),

                             Player.CockDescript(0),

                             Player.CockHead()
                        ));

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ViolatedAsYouAre".Localized());

                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ManagesToHoldThe".Localized());
                        else
                            if (Player.CumQ() < 1000)
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.NagaCheeksBalloon".Localized());
                            else
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.HotSemenBlasts".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.PullsAwayHerTongue".Localized());

                        if (Player.HasStatusEffect(StatusEffectIds.MeanToNaga))
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.GulpDownAMouthful".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.EndUpEnjoyingWhole".Localized());
                    }

                    Player.Orgasm("Dick");
                }
                else
                    if (Player.HasVagina())
                    {
                        if (Player.Lust >= Player.MaxLust)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.KneesShakingAgain".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.KneesBatteredAgain".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SlowlyApproachesYouAgain".Localized());

                        if (Player.Corruption > 50)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SheerThoughtOfIt".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ProbablyWontLike".Localized(
                                 Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.NowAstrideTheBase".Localized());

                        if (Player.Lust >= Player.MaxLust)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.NeedBurnsInside".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.YoureTooWeakTo".Localized());

                        ShowImage("naga-desert-female-lose-tailsex-cunning");

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.WithoutComplaintsPull".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SlowlyMoveTongueAgain".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ContinueLovingAssault".Localized());

                        if (Player.Corruption > 80)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.BiteAtHerClit".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.AttemptToPullAway".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.FinallyPullAway".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.AfterNagaCalmAgain".Localized());

                        if (Player.Corruption > 50)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.CloseEyesAndImagine".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.FollowNagaExample".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.EventuallyItAppears".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.NagaLiesFlat".Localized(
                             Appearance.BiggestBreastSizeDescript(Player),

                             Player.VaginaDescript(0),

                             Player.ClitDescript(),

                             Player.ClitDescript(),

                             Player.VaginaDescript(0)
                        ));

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TailPenetratesAgain".Localized(
                             Player.VaginaDescript(0),

                             Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        Player.CuntChange(40, true);

                        if (Player.HasFuckableNipples())
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.HandsRubAcrossYour".Localized(
                                 Appearance.BiggestBreastSizeDescript(Player)
                            ));

                        if (Player.BiggestLactation() > 1)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.WarmMilkLeakingAgain".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ParagraphBreakThird".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.NagaLiftsYouAgain".Localized(
                             Player.VaginaDescript(0),

                             Appearance.BiggestBreastSizeDescript(Player)
                        ));

                        if (Player.HasFuckableNipples())
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.IsntLongBeforeShe".Localized(
                                 Player.NippleDescript(0)
                            ));

                        if (Player.BiggestLactation() > 1)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.MouthSucksAtAnd".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.ParagraphBreakFourth".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TailWorksInsideAgain".Localized());

                        if (Player.BiggestLactation() > 1)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SharingSomeOfYour".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SentencePeriod".Localized());

                        if (Player.Pregnancy.Incubation > 0)
                        {
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.FinallyReachYourNext".Localized());

                            if (Player.HasStatusEffect(StatusEffectIds.MeanToNaga))
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.SlipperyRopeSheDrops".Localized(
                                     Player.VaginaDescript(0),

                                     Player.VaginaDescript(0)
                                ));
                            else
                                OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TicklesBackOfYour".Localized(
                                     Appearance.BiggestBreastSizeDescript(Player),

                                     Player.VaginaDescript(0),

                                     Player.VaginaDescript(0)
                                ));
                        }
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TailTipCervix".Localized(
                                 Player.VaginaDescript(0)
                            ));

                        Player.Orgasm("Vaginal");
                    }
                    else
                    {
                        if (Player.Lust >= Player.MaxLust)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.KneesShakingThird".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.KneesBatteredThird".Localized());

                        ShowImage("naga-desert-genderless-lose-give-cunning");

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.GrinStretchesAcross".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.AfterMomentOfChuckling".Localized());

                        if (Player.Corruption < 50)
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TurnHeadAway".Localized());
                        else
                            OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.EagerlyBringMouth".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.StartToWorkTongue".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.TailStartsToSqueeze".Localized());

                        OutputText("Scenes.Nagascene.NagaFUCKSJOOOOOO.BringMouthBackTo".Localized());

                        Player.Orgasm("Lips");
                    }

            CombatScene.CleanupAfterCombat();
        }

        public void NagaRapeChoice()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Nagascene.NagaRapeChoice.YouveDefeatedTheNaga".Localized());
            else
                OutputText("Scenes.Nagascene.NagaRapeChoice.NagaWrithesInSand".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.MaleRape".Localized(), null);

            AddButton(1, "Common.Button.FemaleRape".Localized(), null);

            AddButton(2, "Common.Button.Genderless".Localized(), null);

            AddButton(3, "Common.Button.GooeyRape".Localized(), null);

            AddButton(4, "Common.Button.LayEggs".Localized(), null);

            if (Player.Lust >= 33)
            {
                OutputText("Scenes.Nagascene.NagaRapeChoice.BodyAchesForSatisfaction".Localized());

                if (Player.HasCock())
                    AddButton(0, "Common.Button.MaleRape".Localized(), NagaVictoryMale);

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.FemaleRape".Localized(), NagaVictoryFemale);

                if (Player.IsGenderless())
                    AddButton(2, "Common.Button.Genderless".Localized(), NagaVictoryGenderless);

                if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo)
                    AddButton(3, "Common.Button.GooeyRape".Localized(), GooNagaRape);
            }

            if (Player.CanOvipositSpider())
                AddButton(4, "Common.Button.LayEggs".Localized(), EggUpANagaSpiderLike);

            if (Player.CanOvipositBee() && !Player.IsGenderless())
                AddButton(4, "Common.Button.LayEggs".Localized(), BeePositANagaPlease);

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), () => CombatScene.CleanupAfterCombat());
        }

        public void EggUpANagaSpiderLike()
        {
            ClearOutput();

            ShowImage("naga-desert-drider-win-lay-egg");

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.ShinyScaledGirlCollapses".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.RaisesHerselfOnto".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.HavingNoneOfThat".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.JustBelowHumanSkin".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.NothingButSand".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.TightSensationOfSnake".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.SquirmingSlowsSheBegins".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.MusclesDeepInSpider".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.AboveHerGroanComplete".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.CommaSpace".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.JizzWord".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.AndWord".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.FemcumWord".Localized());

                OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.RainingDownOnNaga".Localized());
            }

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.CrumpleIntoAHeap".Localized());

            OutputText("Scenes.Nagascene.EggUpANagaSpiderLike.WhileLaterAwakenAlone".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

        public void BeePositANagaPlease()
        {
            ClearOutput();

            ShowImage("naga-desert-bee-win-lay-egg");

            OutputText("Scenes.Nagascene.BeePositANagaPlease.AdvanceOnSnakeWoman".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.GingerlyCaressSinuous".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.WincesGivingYouA".Localized());

            Player.Body.Get<TailPart>().Venom = 0;

            OutputText("Scenes.Nagascene.BeePositANagaPlease.PressFingersIn".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.WaveringBackAndForth".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.LowerInsectoidAbdomen".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Nagascene.BeePositANagaPlease.NothingButEnjoy".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Nagascene.BeePositANagaPlease.MayAsWellEnjoy".Localized());
                else
                    OutputText("Scenes.Nagascene.BeePositANagaPlease.ReluctantlyKissingBack".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.RelaxAgainstTwoBodies".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.FirstEggExits".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.BodyReactingMounting".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Nagascene.BeePositANagaPlease.EruptsToSplatterWhite".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Nagascene.BeePositANagaPlease.AndY".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Nagascene.BeePositANagaPlease.YLetter".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Nagascene.BeePositANagaPlease.OurVaginaWord".Localized());

                if (Player.Wetness() < 3)
                    OutputText("Scenes.Nagascene.BeePositANagaPlease.DroolsWord".Localized());
                else
                    if (Player.Wetness() < 5)
                        OutputText("Scenes.Nagascene.BeePositANagaPlease.GushesWord".Localized());
                    else
                        OutputText("Scenes.Nagascene.BeePositANagaPlease.SplattersWord".Localized());

                OutputText("Scenes.Nagascene.BeePositANagaPlease.ClearFemcumOverNaga".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Nagascene.BeePositANagaPlease.AsWellWord".Localized());
            }

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Nagascene.BeePositANagaPlease.SentencePeriod".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.PantingSoftlyAddled".Localized());

            if (Player.Eggs() > 30)
                OutputText("Scenes.Nagascene.BeePositANagaPlease.FromOutsideAtLeast".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.IdlyRunsHerHands".Localized());

            if (Player.FertilizedEggs() == 0)
                OutputText("Scenes.Nagascene.BeePositANagaPlease.EggBearerWord".Localized());
            else
                OutputText("Scenes.Nagascene.BeePositANagaPlease.MotherWord".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.PartingStingHerMoans".Localized());

            OutputText("Scenes.Nagascene.BeePositANagaPlease.ShouldComeToThe".Localized());

            Player.DumpEggs();

            Player.Orgasm("Ovi");

            CombatScene.CleanupAfterCombat();
        }

    }
}