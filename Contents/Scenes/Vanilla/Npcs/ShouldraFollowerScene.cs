using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Followers;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<ShouldraFollowerScene>("coc:shouldra_follower")]
    public class ShouldraFollowerScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.ShouldraFollower);

        public string debugName => "Shouldra";

        public string debugHint => "";

        public override void Entry()
        {
            FollowerShouldra();
        }

        public void Reset()
        {

            Save.SetBool("mastIntro", false);

            Save.SetBool("frustrationKnown", false);
        }

        public bool FollowerShouldra()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) == 1;
        }

        public bool ShouldraSleeping(int changes = 0, bool Reset = false)
        {

            if (Reset)
                CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, changes);
            else
                CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer) + changes);

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer) < -216)
                CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, -216);

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer) > 48)
                CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, 48);

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer) > 0)
                return true;

            return false;
        }

        public bool GhostLoli()
        {

            return SceneInstance<ShouldraScene>.Get().GhostLoli();
        }

        public void InitialShouldersRecruitment()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.InitialShouldersRecruitment.EarlyGhostArrival".Localized());

            OutputText("Scenes.Shouldrafollower.InitialShouldersRecruitment.AnxiousAboutAnswer".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), RecruitShouldraForCampSexyTimes);

            AddButton(1, "Common.Button.No".Localized(), NoFollowerShoulders);
        }

        public void NoFollowerShoulders()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.NoFollowerShoulders.BluntRefusal".Localized());

            OutputText("Scenes.Shouldrafollower.NoFollowerShoulders.ForcedSmileDeparture".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RecruitShouldraForCampSexyTimes()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.RecruitShouldraForCampSexyTimes.WelcomeCompanion".Localized());

            OutputText("Scenes.Shouldrafollower.RecruitShouldraForCampSexyTimes.CheerfulTrekToCamp".Localized());

            OutputText("Scenes.Shouldrafollower.RecruitShouldraForCampSexyTimes.ResignedWalkToCamp".Localized());

            OutputText("Scenes.Shouldrafollower.RecruitShouldraForCampSexyTimes.ForgottenSomething".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraFollowerState, .5f);

            DoNext(() => ReturnToCamp(1));
        }

        public void MorningShouldraAlert()
        {

            OutputText("Scenes.Shouldrafollower.MorningShouldraAlert.MorningWakeUpCall".Localized());

            OutputText("Scenes.Shouldrafollower.MorningShouldraAlert.ReadyToSetOut".Localized());

            OutputText("Scenes.Shouldrafollower.MorningShouldraAlert.ShouldraFollowerGained".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraFollowerState, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPlotCountdown, 7);

            DoNext(PlayerMenu);
        }

        public void ShouldraAndValaGetBigger()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraMetVala) < 1)
                CoC.Instance.Flags.Set(GameFlagIds.ShouldraMetVala, 1);

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.TavernIntroduction".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.AdorableFaerieRemark".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.ShouldraTakesOver".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.NervousProtest".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.AnnoyedAtResistance".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.RequestForSilence".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.PulledIntoKiss".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.FaerieHoldsOn".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.ExaminingNewBody".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.UprightSpell".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.SpellMishap".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.DevilishGrin".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.PlayfulResumption".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.ClumsyUndressing".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.UndressRequest".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndValaGetBigger.OverpoweredBySize".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), PureMacroValaCont);

            AddButton(1, "Common.Button.BeAToy".Localized(), PureValaAndShouldraMacroMax);
        }

        public void PureMacroValaCont()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.CloseInspection".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.DirtyFingerTease".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.TastingTheMess".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.GrinningAdmission".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.DesperateHeadShake".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.FullBodyLick".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.MilkingMotion".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.WakingOutsideCamp".Localized());
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.ChestPressure".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.SpecterInspection".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.EagerNodding".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.InstinctiveLicking".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.QuickLickGasp".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.ProlongedOrgasm".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.LosingCount".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.FadingConsciousness".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.BriefAwakening".Localized());

                OutputText("Scenes.Shouldrafollower.PureMacroValaCont.WakingBackAtCamp".Localized());
            }

            ShouldraSleeping(10 + Random.Shared.Next(15), true);

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void PureValaAndShouldraMacroMax()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.UnusualSuggestion".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.PraisingTheIdea".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.GlisteningView".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.EngulfedInPink".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.EngulfedToLegs".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.BodyResponds".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.EagerPistoning".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.SlickWetness".Localized());
            }

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.AbruptExtraction".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.EagerReturn".Localized());

            if (Player.HasCock())
                OutputText(Player.MultiCockDescriptLight());
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript());
                else
                    OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.ButtToken".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.SlickWallPush".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.GSpotHit".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.SuddenOrgasm".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.MatchingHerOrgasm".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.IrresistibleAllure".Localized());

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.JoyfulSquirt".Localized());

                    OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.SentencePeriod".Localized());
                }

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.ConcludingOrgasm".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.SomehowWord".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.VanishingGiggle".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.LustDrivenClimb".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.SwayingGait".Localized());

            OutputText("Scenes.Shouldrafollower.PureValaAndShouldraMacroMax.WakingInCamp".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraMeetsCorruptVala()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.SuddenIdea".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.UnchainingTheFaerie".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.AmusedChuckle".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.ConfusedReaction".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.PeelingArmor".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.TransformationEffect".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.SashayingApproach".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.SilencedProtests".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.PausingMassage".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.DelightedSqueal".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.SlippingPastFolds".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.GrippedAtChest".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.ShiveringRhythm".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMeetsCorruptVala.SharedGaze".Localized());

            Player.Orgasm("Generic");

            ShouldraSleeping(10 + Random.Shared.Next(15), true);

            DoNext(PlayerMenu);
        }

        public void SandWitchGetsGhostly()
        {
            ClearOutput();

            if (Player.HasCock() && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
            {
                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SpecterAppearsAfterVictory".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SorceressScreamsInSand".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SpiritDeniesInvolvement".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.BraMeasuringSmile".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.WitchPushesYouDown".Localized());

                if (Player.BiggestCockLength() < 18)
                    OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.TotalSmothering".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.BreastfleshPressure".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.MilkFormulaSuccess".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.FourHandsFourBoobs".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.EffortsContinue".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.TooSpentToPiston".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SecondWindEscape".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.BeachAWhaleJeer".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.PartingGlanceAtWitch".Localized());
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.MenacingHandRub".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.GhostGirlMocks".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SummonerRemark".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SpellRecital".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SandWitchReclaimsFace".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.MidPerilStruggle".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.PreoccupyingHands".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.MeltingShouldra".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SpheresRiseFromDirt".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.SexualEnablersCharge".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.DeterminedFight".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.DeterminationWinsOut".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.HelpingSeductressUp".Localized());

                Player.CuntChange(20, true, true, false);

                Player.ButtChange(20, true, true, false);

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.BeetRedBlush".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.FaceMimicry".Localized());

                if (!Player.HasFuckableNipples())
                    OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.ChestTreatTease".Localized());

                OutputText("Scenes.Shouldrafollower.SandWitchGetsGhostly.ExasperatedRetreat".Localized());
            }

            ShouldraSleeping(10, true);

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void ShouldraFappinTimes()
        {

            if (Player.HasCock() && Player.HasStatusEffect(StatusEffectIds.Infested))
                ShouldraAndWormsYoureGonnaHaveABadTime();
            else
                if (Player.Gender == Gender.Genderless)
                    GenderlessShouldraMasturbation();
                else
                    if (Player.HasCock() && (!Player.HasVagina() || Random.Shared.Next(2) == 0))
                        MaleMasturbationProper();
                    else
                        if (Random.Shared.Next(2) == 0)
                            GirlyMasturbationWithGhosts();
                        else
                            ShouldraTongueLicksPCs();
        }

        public void GenderlessShouldraMasturbation()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.FingersTraceForm".Localized());

            if (Player.Femininity < 50)
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.MasculineWord".Localized());
            else
                if (Player.Femininity > 50)
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.FeminineWord".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.GlowingEyesMulling".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) == 0)
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.LackingOptions".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) == 0)
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ContemplativePose".Localized());
                else
                {
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.AnotherRoundQuestion".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.NoFavorableAnswer".Localized());
                    else
                        OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.WouldntHaveAsked".Localized());
                }

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ArmorDiscarded".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ClearingInWoods".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.AmnesiaLeastConcern".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) == 0)
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.RusticChest".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) == 0)
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ClenchingReveal".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ToyTrove".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ContainerUnveiled".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.MidsectionStirring".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0)
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.WhatElsePacked".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.SpellCutShort".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 || CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) > 0)
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.FamiliarWord".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.DemonVibratorGrip".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.AdjustmentWarning".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.SpeakingThroughRear".Localized());

            if (Player.Anal.Looseness < 3)
            {
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.NervesScreechInPain".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.GrowingWorry".Localized());

                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.QuickAnusPrep".Localized());
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.PlentyActionJest".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ConfusingClause".Localized());

                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.SentencePeriod".Localized());
            }

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.PieceDeResistance".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.SlimyProtrusion".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetsukoMet) > 0)
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.KitsuneInspired".Localized());
            else
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.CaughtUpInMachination".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ObjectCirclesSphincter".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.PreparationComplete".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.PoeticPreamble".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.SomberShiftRuse".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.FarFromSatisfied".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.QuiveringShafts".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.PayloadYankedFree".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.NotThroughYet".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 || CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) > 0)
                OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.HorseCockRecognition".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0)
                    OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.HorrifyingCatch".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.DildoOfDoomSmack".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.BracingForImpact".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ForestShudders".Localized());

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.WakingAtCamp".Localized());

            Player.Anal.Looseness = 5;

            OutputText("Scenes.Shouldrafollower.GenderlessShouldraMasturbation.ParagraphBreak".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ShouldraSleeping((10 + Random.Shared.Next(26)), true);

            CoC.Instance.Flags.Set(GameFlagIds.GenderlessMasturbationWithGhostCount, CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) + 1);

            InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
        }

        public void NongenderlessAnalShouldraMasturbation()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.ShouldraRunsYourFingers".Localized());

            if (Player.Femininity < 50)
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.MasculineWord".Localized());
            else
                if (Player.Femininity > 50)
                    OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.FeminineWord".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.FormWhileProcessing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) == 0)
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.SpectralVisitorCracksKnuckles".Localized());
            else
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.SheAsksBoxingStance".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.GhostGirlDiscardsArmor".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.LeadsYouToClearing".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.PhantomBoutOfAmnesia".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0 && CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) == 0)
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.RusticWellBuiltChest".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) == 0)
                    OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.LightRevealsContents".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.GhostTreasureTrove".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.PullContainerFromHiding".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.UnusualStirringWithin".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0)
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.WhatElseShePacked".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.EndsSpellQuickly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 || CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) > 0)
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.FamiliarWord".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.DemonVibratorGrasp".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.SentencePeriod".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.SpeaksThroughButthole".Localized());

            if (Player.Anal.Looseness < 3)
            {
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.NervesScreechInPain".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.OnlyWorriesYouFurther".Localized());

                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.AnusPreparedQuickly".Localized());
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.RearRevenantJests".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.OnlyServesToConfuse".Localized());

                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.SentencePeriodAgain".Localized());
            }

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.FinaleAnnouncement".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.MoreSorceryIncoming".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ZetsukoMet) > 0)
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.ShouldraTeases".Localized());
            else
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.CaughtUpInMachination".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.ObjectCirclesSphincter".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.EagerSpiritActs".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.ProfoundWordsRequest".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.PerplexedBySomberShift".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.FarFromSatisfied".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.BizarreShaftsOverwhelm".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.DoublesDownOnPayload".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.SpiritNotThroughYet".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 || CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) > 0)
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.RecognizesHorseCock".Localized());
            else
                OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.HoldsCatchUpClear".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.SmacksDildoOnBoulder".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.TooTiredToObject".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.ForestShuddersWithScream".Localized());

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.WakesBackAtCamp".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            Player.Anal.Looseness = 5;

            ShouldraSleeping((10 + Random.Shared.Next(26)), true);

            OutputText("Scenes.Shouldrafollower.NongenderlessAnalShouldraMasturbation.ParagraphBreak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GenderlessMasturbationWithGhostCount, CoC.Instance.Flags.Get(GameFlagIds.GenderlessMasturbationWithGhostCount) + 1);

            InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
        }

        public void MaleMasturbationProper()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.OverwhelmedByDelight".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.ShouldraDroolsAtMeat".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.SmileWidensAtFlower".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.SunflowerReachesLight".Localized());

            if (Player.CockArea(0) < 200)
                OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.ShouldraLaughsAtSize".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.CrazedLaughterPeals".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.RelinquishesHold".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.CrawlToFlowerEntrance".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.PokeAtSoakingFolds".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.PlantGoesToWork".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.YourSackWord".Localized());
            else
                OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.YourProstateWord".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.ShuddersAndChurns".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.ToeCurlingCompression".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.MonstrousFlowerTips".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.FamiliarGhostRises".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.TakesHandStandUp".Localized());

            if (Player.CockArea(0) < 200)
                OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.StaggersAtJunkHeft".Localized());

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.TossesEctoplasmBottle".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            OutputText("Scenes.Shouldrafollower.MaleMasturbationProper.ParagraphBreak".Localized());

            ShouldraSleeping((10 + Random.Shared.Next(26)), true);

            InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
        }

        public void GirlyMasturbationWithGhosts()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.ShouldraTakesControl".Localized());

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.WarmthSpreadsOut".Localized());

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.PseudoTortureAgony".Localized());

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.SayStart".Localized());

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.VibrationBeginsTeasing".Localized());

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.PalmPressesEntrance".Localized());

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.ClutchesThighsInDefiance".Localized());

            OutputText("Scenes.Shouldrafollower.GirlyMasturbationWithGhosts.ChillSignalsExit".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ShouldraSleeping((10 + Random.Shared.Next(26)), true);

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraTongueLicksPCs()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.GhostDiscardsArmor".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.LeadsToClearing".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 || CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.ForgetfulJourney".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.AmnesiaLeastConcern".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) == 0 && CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.RusticChest".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraGenderlessFuckCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.TreasuredPossessions".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.ToyTrove".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.PullHiddenContainer".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.MusesAtChoices".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
            {
                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SighsAllTheSame".Localized());

                if (Player.Body.Get<TonguePart>().Id.Path != "human")
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SomeOfWord".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.TongueOutExamining".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.HostsTongueReach".Localized());
            }
            else
                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SmilesWondersAloud".Localized());

            if (Player.HasPerk(PerkIds.Flexibility))
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
                {
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.PulseConfirmation".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SitsDownWithDildos".Localized(
                        Player.VaginaDescript(0)
                    ));
                }
                else
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.EnthusiasticSuggestion".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.TestingLicks".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.MuttersNeedMore".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
                {
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.WrestControlBack".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.MomentToAdjust".Localized());
                }
                else
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.HappilyContinues".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.FingersAlongTongue".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.GripsTongueMidsection".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.PossessorEnjoyment".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.BreathyApproval".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.HelplessOnlooker".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.OneInEachHand".Localized());
            }
            else
                if (Player.HasLongTongue())
                {
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.FriendSeesHerself".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SitsDownThenLicks".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.NeedMoreChant".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
                        OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.ShockedWrestControl".Localized());
                    else
                        OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.ContinuesHappily".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SlidesFingersFurther".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.HoldsTongueMiddle".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SharedSensation".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.BreathyDetermination".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.RideOutTheStorm".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.BothShakingHands".Localized());
                }
                else
                {
                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.RefusePolitely".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SitsDownToCheck".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SticksTongueOut".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) == 0)
                        OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.AlmostWrestControl".Localized());
                    else
                        OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.StillHappilyContinues".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.FootOfTongue".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.MidsectionRubbing".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.SheFeelsItToo".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.BreathlessVerdict".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.TrappedObserving".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.GrippingBothToys".Localized());
                }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, .25f), new DynStat(GameStats.Sensitivity, -2f));

            OutputText("Scenes.Shouldrafollower.ShouldraTongueLicksPCs.WakeBackAtCamp".Localized());

            ShouldraSleeping((10 + Random.Shared.Next(26)), true);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraTongueLicksTimes, CoC.Instance.Flags.Get(GameFlagIds.ShouldraTongueLicksTimes) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void GhostBlowjob()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.GhostBlowjob.NovelBlowjobIdea".Localized());

            OutputText("Scenes.Shouldrafollower.GhostBlowjob.SkepticalSpecter".Localized());

            DoNext(() => ShouldraSexMenu());
        }

        public void ShouldraMutualMast()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast.EroticIdeasSpawn".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast.SpecterChimesIn".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast.FurrowsBrow".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast.TrailsOffGesturing".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast.GhostHandToChin".Localized());

            Save.SetBool("mastIntro", true);

            DoNext(() => ShouldraMutualMast2());
        }

        public void ShouldraMutualMast2()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast2.CommandSpiritOut".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast2.OpensEyesSlowly".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast2.WhistlesAndSnaps".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast2.ReasonForSummon".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast2.MayHaveAPoint".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Striptease".Localized(), ShouldraStriptease);

            AddButton(1, "Common.Button.Fadeaway".Localized(), ShouldraFadeAway);
        }

        public void ShouldraStriptease()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            OutputText("Scenes.Shouldrafollower.ShouldraStriptease.MysticStriptease".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraStriptease.BreechesSlideDown".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraStriptease.SwaysAndHovers".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraStriptease.PantiesAtKnees".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraStriptease.LiftsHerShirt".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraStriptease.MentallyConnected".Localized());

            DoNext(() => ShouldraMutualMast3());
        }

        public void ShouldraFadeAway()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraFadeAway.RelentToGhost".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraFadeAway.TwirlsAndFades".Localized());

            ShouldraMutualMast3(false);
        }

        public void ShouldraMutualMast3(bool doClear = true)
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.InvisibleSeat".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.PresentationRiles".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.UnconsciousRhythm".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.WorkHarderForHer".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.TinglingSpark".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.DesperatePleas".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.TakeABreather".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.ExhaledConfession".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraMutualMast3.DozesWithinYou".Localized());

            Player.Orgasm("All");

            ShouldraSleeping((10 + Random.Shared.Next(26)), true);

            InventoryScene.TakeItem(ConsumableIds.Ectoplasm, () => ReturnToCamp(1));
        }

        public void ShouldraDream1()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) == 0)
            {
                OutputText("Scenes.Shouldrafollower.ShouldraDream1.DaysElapsedReflection".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.LostSimplePleasures".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.HollowSelfTouch".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.ResolveDespiteSordidActs".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.FigureEmerges".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.BetterIdeaForms".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.ShoulderVantage".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.WorthTheRisk".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.FirstStepSuccess".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.ReturnToTheTree".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.HostVoiceShout".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.PleadingWomanDemand".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.PleadingWomanVoice".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.RefuseAttachment".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.BeyondAnyMemory".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) == 1)
                {
                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.MidsectionStrike".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.BlueCollarOutburst".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.FamiliarDarknessShroud".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.AnnouncingToNothing".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.FingersGuidedToClit".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.SnatchScentedHand".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.OminousPromise".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.SpookedFarmhandIgnored".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.OccultKnowledgeSearch".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.CupComparison".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.FondlingInterrupted".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraDream1.ProwessSideEffect".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) == 2)
                    {
                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.RelaxingWalkHome".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.GoblinRummaging".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.QuietStalking".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.UnseenPursuit".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.AccessoryCuriosity".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.NipplePiercingOutrage".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.GoblinAstonished".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.SecondBreastRetort".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.RealitySinksIn".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.RightBreastOutburst".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.LeftBreastOutburst".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.RightBreastInterjects".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.InterruptedProtest".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.PregnancyUltimatum".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.HaltingExcuse".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.DesperateHandRequest".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.LeftBreastQuestion".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.MeekTongueGuess".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.IndignantCorrection".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.SuddenSwelling".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.MasculineTwatQuery".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.FearTurnsToAnger".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.ResolveWeakens".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.VirginGoblinSkepticism".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.SexualTroublesSpill".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.BelligerentBoxYells".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.MotherDefended".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.LeftBreastChiding".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.HandTugResistance".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.HarshDaughterVerdict".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.EagerBoxSuggestion".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.IncestRefusal".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.AnnoyedFleshOutburst".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.MotherAbandonmentTaunt".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.SpeechlessDefense".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.CalmDickDemand".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.WearyConcession".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.ImpatientBreastInterrupt".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.LonelyAssholeWhine".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.SurpriseLeftCross".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.NeglectComplaint".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.AgitatedBreastScolding".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.GapingAssholePlea".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.DismissiveSlurp".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.LifeFading".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.JealousAccusation".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.LiteralAssholeInsult".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.ForgottenRage".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.DemandForQuiet".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.EscalatingRetort".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.CacophonyOfAnger".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.BreakingPoint".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.AwestruckTerror".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraDream1.TriumphantWispExit".Localized());
                    }

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) == 3)
            {
                OutputText("Scenes.Shouldrafollower.ShouldraDream1.ScreamAwakening".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.WallPleaRefused".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.ResignedResolve".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.NervousMilitiaWatch".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.OverrunHopeless".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.NearlyEscaped".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.ClubToTemple".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.DazedAwakening".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.OrcNoticesYou".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.SilentDefiance".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.FunnelDiscovery".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.ChokingCreamFlow".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.LongerNextDay".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.DaysBlurTogether".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.WeeksOfCaptivity".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.OrcBeckonedOver".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.NearlyThere".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.SpellcastingFocus".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.FrenziedCasting".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.YourWrathUnleashed".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraDream1.UrgentAwakening".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPlotLevel, CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraTalkNeeded, 1);

            DoNext(PlayerMenu);
        }

        public void ShouldraYappin()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraTalkNeeded) == 1)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) == 1)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.ShouldraTalkNeeded, 0);

                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) < 4)
                        CoC.Instance.Flags.Set(GameFlagIds.ShouldraPlotCountdown, 7);

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.AskAboutStrangeDream".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.HandLiftsToLip".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.GhostDreamSurprise".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.ConceptualizeGhostDream".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.CoyAdmission".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.WatchSpiritFace".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.SenseHerReluctance".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.TangentDissolves".Localized());

                    if (Player.HasPerk(PerkIds.Incorporeality))
                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraBumpsYou".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraScootchesCloser".Localized());

                    if (Player.HasPerk(PerkIds.Incorporeality))
                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.ReallyPossessSomeone".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.LearnHowMuchMore".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.MischievousSmile".Localized());

                    if (Player.Gender == Gender.Genderless)
                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.AssholeInlineTag".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.HandLiftsBackUp".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.AwareOfMonologue".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) == 2)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.ShouldraTalkNeeded, 0);

                        if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) < 4)
                            CoC.Instance.Flags.Set(GameFlagIds.ShouldraPlotCountdown, 7);

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraInterrupts".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.VeerGhostAway".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.WhySheTookMagic".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.SexGetsBoring".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.SelfMadeMagicExpert".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.GhostGirlWalksUp".Localized());

                        if ((int)Player.Gender > 0)
                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.GenitalsSwellUp".Localized());
                        else
                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.FleshyBulgeBegins".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.HandsSlipInside".Localized());

                        if (Player.Gender == Gender.Genderless)
                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.DickWord".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.MarchOfMagnification".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.GenitalsPulseEmphasis".Localized());

                        if (Player.Gender == Gender.Genderless)
                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.BurgeoningDickflesh".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.GumptionToSpeak".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.PointOutDemons".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.SpellsNeverStick".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.KnockedOffYourFeet".Localized());

                        if (Player.Gender == Gender.Genderless)
                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.ErectDickLeaking".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.IncreasedWetness".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.HandsSlowBackDown".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldraYappin.GhostClimbsBackIn".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) == 3)
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.ShouldraTalkNeeded, 0);

                            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) < 4)
                                CoC.Instance.Flags.Set(GameFlagIds.ShouldraPlotCountdown, 7);

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.StareAtShouldra".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShakeHeadConfess".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.GrinAtCandid".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraNoticesConcern".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraReminisces".Localized());

                            if (Player.HasCock())
                                OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraEyesYourCock".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.HalfMillenniumScarcity".Localized());

                            if (Player.BiggestCockArea() >= 200)
                                OutputText("Scenes.Shouldrafollower.ShouldraYappin.OnesLikeYours".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.JustInHeaven".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.SnickerAtFetishes".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.WraithLaughsSlyly".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.InterruptedByHerHand".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraLaughsHand".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.TakeHerTrophy".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.FaceGoesCoy".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.TigerSharkShock".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.PalmFaceShakeHead".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.StillParalyzed".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.RecentFavoriteSport".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.GhostTakesDeepBreath".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraSlidesBackIn".Localized());
                        }
                        else
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.ShouldraTalkNeeded, 0);

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.StepsOutOfYou".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.SitInSilence".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.DrawsKneesToChest".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.AskWhyRepulsed".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.RamblingAndVenting".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.NoApprenticeRegret".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.HoursOfVenting".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.EyesWaverThenBolt".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldraYappin.StepsForwardReenters".Localized());
                        }
            }
            else
            {

                var choices = new List<int> { 1, 2 };

                if (Save.GetBool("frustrationKnown"))
                    choices.Add(3);

                switch (choices[Random.Shared.Next(choices.Count)])
                {
                    case 1:
                    {
                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.WhimChatWithGhost".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.TapsFinger".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.SmilingToHerself".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraPausesGazing".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.CantBelieveIt".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShrugTrailsOffFrowning".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.SwoopsBackInside".Localized());
                    }

                    break;

                    case 2:
                    {
                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.QuietDuringSex".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.ShouldraWavers".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.MakesSomeSense".Localized());
                    }

                    break;

                    case 3:
                    {
                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.RuffleBrowQuestion".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.SighExplainsNuance".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.SoundsLikeBother".Localized());

                    OutputText("Scenes.Shouldrafollower.ShouldraYappin.GhostScoffs".Localized());
                    }

                    break;
                }
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraFollowerScreen()
        {
            ClearOutput();

            if (!ShouldraSleeping())
                OutputText("Scenes.Shouldrafollower.ShouldraFollowerScreen.PulseDesireContact".Localized());
            else
            {
                OutputText("Scenes.Shouldrafollower.ShouldraFollowerScreen.ContactSleepingGhost".Localized());

                DoNext(PlayerMenu);

                return ;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), ShouldraAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), ShouldraYappin);

            if (Player.Lust >= 33)
                AddButton(2, "Common.Button.Sex".Localized(), ShouldraSexMenu);
            else
                AddButton(2, "Common.Button.Sex".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) >= 2)
            {
                AddButton(5, "Common.Button.GrowSomething".Localized(), ShouldraGroPlus);

                AddButton(6, "Common.Button.ShrinkSomething".Localized(), ShouldraReductoMenu);

                OutputText("Scenes.Shouldrafollower.ShouldraFollowerScreen.PermanentBodyChange".Localized());
            }

            AddButton(10, "Common.Button.GoAway".Localized(), KickFollowerShouldraOutPrompt);

            if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !SceneInstance<Areas.Swamp.AlrauneScene>.Get().AskedShouldra)
                AddButton(11, "Common.Button.Vines".Localized(), () => SceneInstance<Areas.Swamp.AlrauneScene>.Get().AskShouldra());

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void ShouldraSexMenu()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraSexMenu.ShouldraStirsExcited".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Ghostmasturbate".Localized(), ShouldraFappinTimes);

            if ((int)Player.Gender > 0 && Player.Lust >= 33)
            {
                AddButton(1, "Common.Button.AnalMasturb".Localized(), NongenderlessAnalShouldraMasturbation);

                OutputText("Scenes.Shouldrafollower.ShouldraSexMenu.AnalFunOffer".Localized());
            }
            else
                AddButton(1, "Common.Button.AnalMasturb".Localized(), null);

            AddNextButton("Common.Button.Mutualmast".Localized(), Save.GetBool("mastIntro")

                ? ShouldraMutualMast2

                : ShouldraMutualMast);

            if (CoC.Instance.Flags.Get(GameFlagIds.FreedVala) != 0 && (View.Time.Hours >= 12 && View.Time.Hours <= 21))
            {
                OutputText("Scenes.Shouldrafollower.ShouldraSexMenu.IntroduceValaToShouldra".Localized());

                AddNextButton("Common.Button.VisitVala".Localized(), ShouldraAndValaGetBigger);
            }

            if (silly && Player.HasCock())
                AddNextButton("Common.Button.Ghostblowjob".Localized(), GhostBlowjob);

            AddButton(14, "Common.Button.Back".Localized(), ShouldraFollowerScreen);
        }

        public void ShouldraAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraAppearance.PaleIntangibleBody".Localized(
                MeasurementsUtils.BriefHeight(GhostLoli() ? 57 : 65)
            ));

            OutputText("Scenes.Shouldrafollower.ShouldraAppearance.BreastsAndButt".Localized());

            DoNext(() => ShouldraFollowerScreen());
        }

        public void ShouldraGroPlus()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraGroPlus.AcceptBodyEnhanceOffer".Localized());

            ClearButtons();

            if (Player.Balls > 0)
                AddButton(0, "Scenes.PlayerAppearance.Label.Balls".Localized(), GroBallsBiggaGHOSTYSTYLE);
            else
                AddButton(0, "Scenes.PlayerAppearance.Label.Balls".Localized(), null);

            AddButton(1, "Common.Button.Breasts".Localized(), ShouldraGrowsYoTits);

            if (Player.HasVagina())
                AddButton(2, "Scenes.PlayerAppearance.Label.Clit".Localized(), ShouldraGrowsYoClit);
            else
                AddButton(2, "Scenes.PlayerAppearance.Label.Clit".Localized(), null);

            if (Player.HasCock())
                AddButton(3, "Common.Button.Cock".Localized(), ShouldraCockBloating101);
            else
                AddButton(3, "Common.Button.Cock".Localized(), null);

            AddButton(4, "Common.Button.Nipples".Localized(), ShouldraGivesYaSomeFukkinTeats);

            AddButton(5, "Scenes.PlayerAppearance.Label.Butt".Localized(), ShouldrasButtBigginator);

            AddButton(14, "Common.Button.Back".Localized(), ShouldraFollowerScreen);
        }

        public void GroBallsBiggaGHOSTYSTYLE()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.GroBallsBiggaGHOSTYSTYLE.BiggerBallsackOffer".Localized());

            if (Random.Shared.Next(5) == 0)
            {
                Player.BallSize += 2 + Random.Shared.Next(4);

                OutputText("Scenes.Shouldrafollower.GroBallsBiggaGHOSTYSTYLE.TesticlesRumbleBigger".Localized());
            }
            else
            {
                Player.BallSize += 1 + Random.Shared.Next(2);

                OutputText("Scenes.Shouldrafollower.GroBallsBiggaGHOSTYSTYLE.TinglingScrotum".Localized());
            }

            if (Player.BallSize > 10)
                OutputText("Scenes.Shouldrafollower.GroBallsBiggaGHOSTYSTYLE.SecondGuessWalking".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraGrowsYoTits()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShouldraGrowsYoTits.TitSlaveBoast".Localized());

            if (Player.BreastRows.Count == 1)
                Player.GrowTits((1 + Random.Shared.Next(5)), 1, true, 1);
            else
                Player.GrowTits(1 + Random.Shared.Next(2), Player.BreastRows.Count, true, 1);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraGrowsYoClit()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShouldraGrowsYoClit.JoyBuzzerTease".Localized());

            Player.Vaginas[0].ClitLength += 1;

            OutputText("Scenes.Shouldrafollower.ShouldraGrowsYoClit.WelcomesNewInch".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f), new DynStat(GameStats.Lust, 15f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraCockBloating101()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShouldraCockBloating101.SatisfiedGrinDevelops".Localized());

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Shouldrafollower.ShouldraCockBloating101.MoreCocksEmerge".Localized());

                if (!Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Shouldrafollower.ShouldraCockBloating101.CrotchWord".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.ShouldraCockBloating101.SheathWord".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraCockBloating101.ShouldraGraspsEagerly".Localized());

                Player.IncreaseCock(0, 4);

                Player.Cocks[0].Length += 1;

                Player.Cocks[0].Thickness += .25f;
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.ShouldraCockBloating101.CockBouquetThickens".Localized());

                for (var i = 0; i < Player.Cocks.Count; i++)
                {
                    Player.IncreaseCock(i, 2);

                    Player.Cocks[i].Length += 1;

                    Player.Cocks[i].Thickness += .25f;
                }
            }

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 20f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraGivesYaSomeFukkinTeats()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShouldraGivesYaSomeFukkinTeats.ShouldraRubsHands".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraGivesYaSomeFukkinTeats.NipplesPressArmor".Localized());

            Player.NippleLength += (Random.Shared.Next(2) + 3) / 10;

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            float temp2 = 0;

            if (!Player.HasFuckableNipples() && Random.Shared.Next(4) == 0)
            {
                var temp = Player.BreastRows.Count;

                while (temp > 0)
                {
                    temp--;

                    if (!Player.BreastRows[temp].Fuckable && Player.NippleLength >= 2)
                    {
                        Player.BreastRows[temp].Fuckable = true;

                        temp2 += 1;
                    }
                }

                if (temp2 > 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraGivesYaSomeFukkinTeats.FingersSlipInside".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldrasButtBigginator()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShouldrasButtBigginator.HauntingLaughEscapes".Localized());

            Player.Body.Butt += 1 + Random.Shared.Next(3);

            OutputText("Scenes.Shouldrafollower.ShouldrasButtBigginator.CheeksQuakeMagic".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraReductoMenu()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraReductoMenu.ShrinkOfferAccepted".Localized());

            ClearButtons();

            if (Player.Balls > 0 && Player.BallSize > 1)
                AddButton(0, "Scenes.PlayerAppearance.Label.Balls".Localized(), () => ShouldraReductosYourBallsUpInsideYa());
            else
                AddButton(0, "Scenes.PlayerAppearance.Label.Balls".Localized(), null);

            if (Player.BiggestBreastSize() >= 1)
                AddButton(1, "Common.Button.Breasts".Localized(), ShouldraReductosYourTits);
            else
                AddButton(1, "Common.Button.Breasts".Localized(), null);

            if (Player.HasVagina() && Player.GetClitLength() > .25f)
                AddButton(2, "Scenes.PlayerAppearance.Label.Clit".Localized(), ClittyVanishingActShouldra);
            else
                AddButton(2, "Scenes.PlayerAppearance.Label.Clit".Localized(), null);

            if (Player.HasCock() && Player.BiggestCockLength() > 4)
                AddButton(3, "Common.Button.Cock".Localized(), ShouldraMakesCocksDisappear);
            else
                AddButton(3, "Common.Button.Cock".Localized(), null);

            if (Player.NippleLength > .25f)
                AddButton(4, "Common.Button.Nipples".Localized(), ShrinkDemNipplzForYoGhost);
            else
                AddButton(4, "Common.Button.Nipples".Localized(), null);

            if (Player.Body.Butt >= 2)
                AddButton(5, "Scenes.PlayerAppearance.Label.Butt".Localized(), ShrinkDatBootyForYoGhost);
            else
                AddButton(5, "Scenes.PlayerAppearance.Label.Butt".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), ShouldraFollowerScreen);
        }

        public void ShouldraReductosYourBallsUpInsideYa(bool rescue = false)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            if (rescue)
            {
                OutputText("Scenes.Shouldrafollower.ShouldraReductosYourBallsUpInsideYa.PunsAndTeasesFail".Localized());

                if (Player.BallSize > 18 + (Player.Strength / 2) + (Player.Tallness / 4))
                    Player.BallSize = 16 + (Player.Strength / 2) + (Player.Tallness / 4);
            }
            else
                OutputText("Scenes.Shouldrafollower.ShouldraReductosYourBallsUpInsideYa.PunsAloneFail".Localized());

            Player.BallSize -= (2 + Random.Shared.Next(4));

            if (Player.BallSize < 1)
                Player.BallSize = 1;

            OutputText("Scenes.Shouldrafollower.ShouldraReductosYourBallsUpInsideYa.ScrotumSquirmsInward".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraReductosYourTits()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShouldraReductosYourTits.ArmsClaspChest".Localized());

            Player.ShrinkTits();

            OutputText("Scenes.Shouldrafollower.ShouldraReductosYourTits.ExploreNewChestSize".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void ClittyVanishingActShouldra()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ClittyVanishingActShouldra.NodReassertsDesire".Localized());

            Player.SetClitLength(Player.GetClitLength() / 1.7f);

            Player.SetClitLength((int)(Player.GetClitLength() * 100) / 100);

            OutputText("Scenes.Shouldrafollower.ClittyVanishingActShouldra.SoftMoanShrinks".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraMakesCocksDisappear()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.HandsDartToProtect".Localized());

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.PenisVibratesMagic".Localized());

                float choice = Random.Shared.Next(10);

                if (choice == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.MurdererInsult".Localized());
                else
                    if (choice == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.BastardInsult".Localized());
                    else
                        if (choice == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.JerkInsult".Localized());
                        else
                            if (choice == 3)
                                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.MonsterInsult".Localized());
                            else
                                if (choice == 4)
                                    OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.CretinInsult".Localized());
                                else
                                    if (choice == 5)
                                        OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.FiendInsult".Localized());
                                    else
                                        if (choice == 6)
                                            OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.AssholeInsult".Localized());
                                        else
                                            if (choice == 7)
                                                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.VillainInsult".Localized());
                                            else
                                                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.DickInsult".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.TinyVoiceFades".Localized());

                Player.Cocks[0].Length *= 2f / 3f;

                Player.Cocks[0].Thickness *= 2f / 3f;
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.DicksVibrateMagic".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.BiggestCockPleads".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.SmallestCockBegs".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.AnotherCockYells".Localized());

                if (Player.CockTotal() > 3)
                    OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.OneCockAsksWhy".Localized());

                if (Player.CockTotal() > 4)
                    OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.AnotherDickRages".Localized());

                if (Player.CockTotal() > 5)
                    OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.YetAnotherPleads".Localized());

                if (Player.CockTotal() > 6)
                    OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.RemainderCryBeg".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraMakesCocksDisappear.CacophonySubsides".Localized());

                float ii = 0;

                while (ii < Player.Cocks.Count)
                {
                    Player.Cocks[(int)(ii)].Length *= 2f / 3f;

                    Player.Cocks[(int)(ii)].Thickness *= 2f / 3f;

                    ii += 1;
                }
            }

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Lust, -10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShrinkDemNipplzForYoGhost()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShrinkDemNipplzForYoGhost.CutGhostArgument".Localized());

            if (Player.NippleLength / 2 < .25f)
            {
                OutputText("Scenes.Shouldrafollower.ShrinkDemNipplzForYoGhost.NipplesQuarterInch".Localized());

                Player.NippleLength = .25f;
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.ShrinkDemNipplzForYoGhost.NipplesHalfSize".Localized());

                Player.NippleLength /= 2;
            }

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Lust, -5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ShrinkDatBootyForYoGhost()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraMagicCooldown, 72);

            OutputText("Scenes.Shouldrafollower.ShrinkDatBootyForYoGhost.HandsGraspButt".Localized());

            if (Player.Body.Butt >= 15)
            {
                Player.Body.Butt -= 3 + (int)(Player.Body.Butt / 3);

                OutputText("Scenes.Shouldrafollower.ShrinkDatBootyForYoGhost.CheeksFeelLighter".Localized());
            }
            else
                if (Player.Body.Butt >= 10)
                {
                    Player.Body.Butt -= 3;

                    OutputText("Scenes.Shouldrafollower.ShrinkDatBootyForYoGhost.AssJigglesInward".Localized());
                }
                else
                {
                    Player.Body.Butt -= 1 + Random.Shared.Next(3);

                    if (Player.Body.Butt < 1)
                        Player.Body.Butt = 1;

                    OutputText("Scenes.Shouldrafollower.ShrinkDatBootyForYoGhost.ButtSettlesTiny".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Lust, -10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void KickFollowerShouldraOutPrompt()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KickFollowerShouldraOutPrompt.ConfirmBootOut".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), KickFollowerShouldraOut);

            AddButton(1, "Common.Button.No".Localized(), ShouldraFollowerScreen);
        }

        public void KickFollowerShouldraOut()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraPlotLevel) < 4 || CoC.Instance.Flags.Get(GameFlagIds.ShouldraTalkNeeded) > 0)
            {
                OutputText("Scenes.Shouldrafollower.KickFollowerShouldraOut.ShouldraTumblesOut".Localized());

                OutputText("Scenes.Shouldrafollower.KickFollowerShouldraOut.HairDarkensBrown".Localized());

                OutputText("Scenes.Shouldrafollower.KickFollowerShouldraOut.TurnsAndLeaves".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.ShouldraFollowerState, 0);

                CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, 0);

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.KickFollowerShouldraOut.ConfirmAskLeave".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), DefinitelyKickOutCrazyGhosts);

                AddButton(1, "Common.Button.No".Localized(), ShouldraFollowerScreen);
            }
        }

        public void DefinitelyKickOutCrazyGhosts()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.DefinitelyKickOutCrazyGhosts.SayWhatReply".Localized());

            OutputText("Scenes.Shouldrafollower.DefinitelyKickOutCrazyGhosts.RepeatLeaveCommand".Localized());

            OutputText("Scenes.Shouldrafollower.DefinitelyKickOutCrazyGhosts.ShouldraStompsOff".Localized());

            OutputText("Scenes.Shouldrafollower.DefinitelyKickOutCrazyGhosts.StandAndListen".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraFollowerState, -1);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public bool ShouldersWarnings()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer) == -96)
            {
                if (Random.Shared.Next(3) == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldersWarnings.BuiltUpLustWarning".Localized());
                else
                    if (Random.Shared.Next(2) == 0)
                        OutputText("Scenes.Shouldrafollower.ShouldersWarnings.HandsRubImpatiently".Localized());
                    else
                        OutputText("Scenes.Shouldrafollower.ShouldersWarnings.ShiverDownSpine".Localized());

                Save.SetBool("frustrationKnown", true);

                OutputText("Scenes.Shouldrafollower.ShouldersWarnings.ParagraphBreak".Localized());

                return true;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer) == -168)
                {
                    if (Random.Shared.Next(3) == 0)
                    {
                        OutputText("Scenes.Shouldrafollower.ShouldersWarnings.YourArousalFragment".Localized());

                        if (Player.Gender == Gender.Genderless)
                            OutputText("Scenes.Shouldrafollower.ShouldersWarnings.LibidoGoingHaywire".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldersWarnings.GhostLustBleedsIn".Localized());

                        OutputText("Scenes.Shouldrafollower.ShouldersWarnings.MinLustRaised".Localized());
                    }
                    else
                        if (Random.Shared.Next(2) == 0)
                        {
                            OutputText("Scenes.Shouldrafollower.ShouldersWarnings.ShouldraKeepsRubbing".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldersWarnings.MinLustRaisedAgain".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Shouldrafollower.ShouldersWarnings.LegDancesFervently".Localized());

                            OutputText("Scenes.Shouldrafollower.ShouldersWarnings.MinLustRaisedThird".Localized());
                        }

                    Player.ApplyStats(new DynStat(GameStats.Lust, .1f));

                    OutputText("Scenes.Shouldrafollower.ShouldersWarnings.ParagraphBreakAgain".Localized());

                    return true;
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraSleepTimer) == -216)
            {
                if (Random.Shared.Next(3) == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldersWarnings.NodUnsureHow".Localized());
                else
                    if (Random.Shared.Next(2) == 0)
                        OutputText("Scenes.Shouldrafollower.ShouldersWarnings.HandsClingDesperately".Localized());
                    else
                        OutputText("Scenes.Shouldrafollower.ShouldersWarnings.HunchAboutShouldra".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, .1f));

                OutputText("Scenes.Shouldrafollower.ShouldersWarnings.ParagraphBreakThird".Localized());

                return true;
            }

            return false;
        }

        public void NightTimeShouldraRapesThePC()
        {

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, 0);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 4f));

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ChillingGustRouses".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraTimesNightRapedPc) == 0)
            {
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ShouldraSoftTone".Localized());

                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.NotManyPeopleBrought".Localized());
            }
            else
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ShouldraSparesRhetoric".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ShouldraBreaksGlare".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.VindictiveDrySpell".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.FailingToStand".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ShadeWorksOver".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ShadeBendsDown".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.GroggyDazePanic".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.AlitYellowTinge".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.FlabbyFingersUndulate".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.HarpyAssReveal".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.LeftHandExaminesBouquet".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.BreathingPaceQuickens".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.CirclingEntireLengths".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.EcstaticTourFeeling".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ShouldraPartsFingers".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.SwappingBetweenDicks".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.FingerPayloads".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.GhostPersonalHeaven".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.SatisfiedSpiritSpeaks".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ManhoodToken".Localized());
            else
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.WomanhoodToken".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.SofteningThePrefix".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.MagicallyEnlargedMember".Localized());
            else
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.MagicallyGapedSnatch".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.LessCraggyComplexion".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.VacantlyRubbingYou".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.FlailingCock".Localized());
            else
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.MoistVagina".Localized());

            OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.SentencePeriod".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.SurrenderToGame".Localized());

                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.ManicStrokingCommences".Localized());
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.KowtowAdrenalineRush".Localized());

                OutputText("Scenes.Shouldrafollower.NightTimeShouldraRapesThePC.FiveDicksStrong".Localized());
            }

            DoNext(() => EdwardShouldraDickHandsPartII());
        }

        public void EdwardShouldraDickHandsPartII()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.EdwardShouldraDickHandsPartII.GaspAwakeColdSweat".Localized());

            OutputText("Scenes.Shouldrafollower.EdwardShouldraDickHandsPartII.StoneFacedGhoul".Localized());

            OutputText("Scenes.Shouldrafollower.EdwardShouldraDickHandsPartII.DreamAllTooReal".Localized());

            OutputText("Scenes.Shouldrafollower.EdwardShouldraDickHandsPartII.MinimumLustReduced".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraTimesNightRapedPc, CoC.Instance.Flags.Get(GameFlagIds.ShouldraTimesNightRapedPc) + 1);

            DoNext(PlayerMenu);
        }

        public void ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark()
        {

            var choices = new List<int>();

            int select = 0;

            if (Player.HasCock() && Player.BiggestCockArea() < 200)
            {
                choices.Add(1);

                choices.Add(1);

                choices.Add(1);

                choices.Add(1);
            }
            else
                if (Player.HasCock())
                {
                    choices.Add(2);

                    choices.Add(2);

                    choices.Add(2);

                    choices.Add(2);
                }

            if (Player.Balls > 0)
            {
                choices.Add(3);

                choices.Add(3);

                choices.Add(3);

                choices.Add(3);
            }

            if (Player.BiggestBreastSize() >= 2)
            {
                choices.Add(4);

                choices.Add(4);

                choices.Add(4);

                choices.Add(4);
            }

            if (Player.HasVagina())
            {
                choices.Add(5);

                choices.Add(5);

                choices.Add(5);

                choices.Add(5);
            }

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1 && 9999 == 9999)
            {
                choices.Add(6);

                choices.Add(6);

                choices.Add(6);

                choices.Add(6);
            }

            choices.Add(7);

            choices.Add(7);

            choices.Add(7);

            choices.Add(7);

            if (Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1)
                choices.Add(8);

            if (Player.Pregnancy.Incubation > 0)
            {
                choices.Add(9);

                choices.Add(10);
            }

            if (Player.ButtPregnancy.Incubation > 0)
                choices.Add(11);

            if (Player.CanFly())
                choices.Add(12);

            if (Player.HasTailInsteadOfLegs())
                choices.Add(13);
            // 原版 ShouldraFollower.hx:1808：Amily 是纯爱跟随者时追加一个选项
            if (SceneInstance<AmilyScene>.Get().AmilyFollowerPure())
                choices.Add(14);

            if (FollowerRegistry.HasFollower(FollowerIds.JojoFollower))
                choices.Add(15);

            if (Player.HasPerk(PerkIds.MaraesGiftStud) || Player.HasPerk(PerkIds.MaraesGiftFertility) || Player.HasPerk(PerkIds.MaraesGiftProfractory) || Player.HasPerk(PerkIds.MaraesGiftButtslut))
                choices.Add(16);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) > 0)
                choices.Add(17);

            if (Player.HasPerk(PerkIds.Incorporeality))
                choices.Add(18);

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 2)
                choices.Add(19);

            if (Player.HasSpells())
                choices.Add(20);

            if (Player.CountCocksOfType("ANEMONE") > 0)
                choices.Add(21);

            if (Holidays.IsHalloween(true))
                choices.Add(22);

            select = choices[Random.Shared.Next(choices.Count)];

            var subSelect = 0;

            switch (select)
            {
                case 1:
                {
                subSelect = Random.Shared.Next(5);

                if (subSelect == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.TinglesAndTighteningArmor".Localized());
                else
                    if (subSelect == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.UncomfortableNetherStirring".Localized());
                    else
                        if (subSelect == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.DistractedByGrowingCock".Localized());
                        else
                            if (subSelect == 3)
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ArmsSeizedMidStride".Localized());
                            else
                                if (subSelect == 4 && Player.CountCocksOfType("Horse") == 0)
                                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SuddenTwistingPain".Localized());
                                else
                                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.HandPlaysWithCock".Localized());
                }

                break;

                case 2:
                {
                subSelect = Random.Shared.Next(5);

                if (subSelect == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.CockWarmAndDense".Localized());
                else
                    if (subSelect == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.WetFeelingInArmor".Localized());
                    else
                        if (subSelect == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.HandsDiveIntoArmor".Localized());
                        else
                            if (subSelect == 3)
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.KnockedBreathlessByDesire".Localized());
                            else
                            {
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.LightheadedFromEngorgedCock".Localized());

                                Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 10f));
                            }
                }

                break;

                case 3:
                {
                subSelect = Random.Shared.Next(5);

                if (subSelect == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.BallsGrowingHot".Localized());
                else
                    if (subSelect == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ArmorTightnessRevealsBalls".Localized());
                    else
                        if (subSelect == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.FallsAsBallsThrash".Localized());
                        else
                            if (subSelect == 3 && Player.HasCock())
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.BallsSwellThenTighten".Localized());
                            else
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ShouldraWhatIsWrong".Localized());
                }

                break;

                case 4:
                {
                subSelect = Random.Shared.Next(5);

                if (subSelect == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ChestArmorTooTight".Localized());
                else
                    if (subSelect == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.StretchingChestSensation".Localized());
                    else
                        if (subSelect == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.NipplesLactateProfusely".Localized());
                        else
                            if (subSelect == 3)
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.LifeNearlySpookedOut".Localized());
                            else
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.MischievousVoiceWhispers".Localized());
                }

                break;

                case 5:
                {
                subSelect = Random.Shared.Next(5);

                if (subSelect == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.FlowerRustlingBucklesKnees".Localized());
                else
                    if (subSelect == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.VaginaPuffsAndNips".Localized());
                    else
                        if (subSelect == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SneakyHandFingering".Localized());
                        else
                            if (subSelect == 3)
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.VaginaGushesFluids".Localized());
                            else
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SensesShouldraLaughing".Localized());
                }

                break;

                case 6:
                {
                subSelect = Random.Shared.Next(6);

                if (subSelect == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.WhisperingFromCock".Localized());
                else
                    if (subSelect == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.CockWigglesInsideArmor".Localized());
                    else
                        if (subSelect == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ExgartuanAsksFromArmor".Localized());
                        else
                            if (subSelect == 3)
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.HandsExploreCockNodules".Localized());
                            else
                                if (subSelect == 4)
                                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ExgartuanAsksBluntly".Localized());
                                else
                                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ArmorVanishesButtNaked".Localized());
                }

                break;

                case 7:
                {
                subSelect = Random.Shared.Next(6);

                if (subSelect == 0)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ShouldraMoansTakesControl".Localized());
                else
                    if (subSelect == 1)
                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ColossalYawnBursts".Localized());
                    else
                        if (subSelect == 2)
                            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.QuietAndSuspicious".Localized());
                        else
                            if (subSelect == 3)
                                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.MutteringFromYourPerson".Localized());
                            else
                                if (subSelect == 4)
                                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.FootFetishQuestion".Localized());
                                else
                                    if (subSelect == 5)
                                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.FamiliarVoiceSurfaces".Localized());
                                    else
                                        OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ComplimentsTheRacks".Localized());
                }

                break;

                case 8:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ShouldraSiftsMemories".Localized());
                }

                break;

                case 9:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.HandsCircleBelly".Localized());
                }

                break;

                case 10:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.CommentsOnWigglingBelly".Localized());
                }

                break;

                case 11:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.RearEndedPregnancyJoke".Localized());
                }

                break;

                case 12:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.WingsFlapByThemselves".Localized());
                }

                break;

                case 13:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.TailWigglesByItself".Localized());
                }

                break;

                case 14:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.FingersSignalStirring".Localized());
                }

                break;

                case 15:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SilencePromptsQuestion".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ShrugsOffHolyMouse".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.MocksSpiritQuote".Localized());
                }

                break;

                case 16:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.CommentsOnCorruptedBlessings".Localized());
                }

                break;

                case 17:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SayStart".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenSharkgirls) + CoC.Instance.Flags.Get(GameFlagIds.IzmaChildrenTigersharks) == 1)
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SharkDaughterFragment".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SharkDaughtersFragment".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.SharkDaughtersPraise".Localized());
                }

                break;

                case 18:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.LaughsAboutEctoplasm".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.BestToStopWondering".Localized());
                }

                break;

                case 19:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ChestFidgetsMuffledVoice".Localized());
                }

                break;

                case 20:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ChimesAboutMagic".Localized());

                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.MindReadingWish".Localized());
                }

                break;

                case 21:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.AnemoneDickRevealed".Localized());
                }

                break;

                case 22:
                {
                OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ShouldraTakesControl".Localized());
                }

                break;
            }

            OutputText("Scenes.Shouldrafollower.ShouldraWakesUpOrPokesPCsForShitsAndGigglesIdunnoHowLongCanIMakeThisFunctionNameQuestionMark.ParagraphBreak".Localized());
        }

        public void ShouldraAndWormsYoureGonnaHaveABadTime()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.EyesGlowYellow".Localized());

            if (Player.Cocks[0].Length < 48)
                OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.CutsToTheChase".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.DelightedDickHug".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.StrangeSensationInLiquid".Localized());

            if (View.Time.Hours <= 12)
                OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.MorningWord".Localized());
            else
                if (View.Time.Hours <= 5)
                    OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.AfternoonWord".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.EveningWord".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.UrethraIntroductionToEidolon".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.UnearthlyScream".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraWormSceneCounter) > 0)
                OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.SpiritForgetsWorms".Localized());
            else
                OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.MouthUttersInfested".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndWormsYoureGonnaHaveABadTime.SitInSilence".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.KeepShouldra".Localized(), KickOutWormiesForYourGhostPalPAL);

            AddButton(1, "Common.Button.KeepWorms".Localized(), KickOutShouldra4YoWormyBuddies);
        }

        public void KickOutShouldra4YoWormyBuddies()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KickOutShouldra4YoWormyBuddies.NoAllyAmongCreepies".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutShouldra4YoWormyBuddies.SensesYourFirmness".Localized());

            if (Player.Cocks[0].Length < 48)
                OutputText("Scenes.Shouldrafollower.KickOutShouldra4YoWormyBuddies.LittleOrdealDealtWith".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutShouldra4YoWormyBuddies.HopesWormsAppreciate".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraFollowerState, 0);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void KickOutWormiesForYourGhostPalPAL()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KickOutWormiesForYourGhostPalPAL.ConcedesToParanormalPlaintiff".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutWormiesForYourGhostPalPAL.SpiritProclaimsThroughMouth".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutWormiesForYourGhostPalPAL.PanickedSpiritShouts".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutWormiesForYourGhostPalPAL.ShouldraCallsYouAsshole".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutWormiesForYourGhostPalPAL.SizzlingBallsFuelGeyser".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutWormiesForYourGhostPalPAL.SpiritFallsYouBack".Localized());

            OutputText("Scenes.Shouldrafollower.KickOutWormiesForYourGhostPalPAL.DemandsBathFirst".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.Infested);

            DoNext(() => ReturnToCamp(1));
        }

        public void ShouldraAndExgartumonFightGottaCatchEmAll()
        {

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.BedLooksComfortable".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.WorkWord".Localized());
            else
                OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.FuckingWord".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.CockErectsAtBedtime".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.PuzzlesOverNewBehavior".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.RepliesInNewVoice".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.ExgartuanMeatSackRant".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.ShouldraPackageDealRetort".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.SensationOwnershipClaim".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.RelinquishControlRequest".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndExgartumonFightGottaCatchEmAll.TuckAndLieProne".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartudrama, 1);

            DoNext(PlayerMenu);
        }

        public void ExgartumonAndShouldraFightPartII()
        {

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartII.SuddenBuildupBurst".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartII.MindBendingEjaculation".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartII.ExgartuanLaughsHard".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartudrama, 2);
        }

        public void ExgartumonAndShouldraFightPartIII()
        {

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartIII.ShouldraBlurtOutburst".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartIII.ClenchFistsSting".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartIII.ShoutAtEquestrian".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartIII.DieTwiceThreat".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartumonAndShouldraFightPartIII.EscalationRefusal".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartudrama, 3);
        }

        public void ExgartuMonAndShouldraShowdown()
        {

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.MorningWoodAwakening".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.NotDaybreakGreeting".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.StayOutDemand".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.StopHoggingCock".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.StageJumperRage".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.ThrustForcesShouldraOut".Localized(
                Player.Mf("guy", "gal")
            ));

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.UntangleFromSheets".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.SickOfRepeating".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.GroupDelusionMockery".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.DickHatredRemark".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.GrindingTeethSteam".Localized());

            if (silly)
                OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.BodilyRightDeclaration".Localized());

            OutputText("Scenes.Shouldrafollower.ExgartuMonAndShouldraShowdown.ShoutToSilenceThem".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.KeepExgartuan".Localized(), KeepExgartuanInsteadOfShouldra);

            AddButton(1, "Common.Button.KeepShouldra".Localized(), KeepShouldraAndKickOutExgartuan);

            AddButton(2, "Common.Button.KeepBoth".Localized(), KeepAllTheGhosts);
        }

        public void KeepExgartuanInsteadOfShouldra()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.SideWithDemonAnnouncement".Localized());

            OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.KickingMeOutProtest".Localized());

            OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.ClingyGirlfriendTaunt".Localized());

            OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.GirlfriendSlipStop".Localized());

            if (Player.Corruption < 10)
                OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.PurificationExcuse".Localized());
            else
                if (Player.Corruption < 60)
                    OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.BrushAwayClaim".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.PraiseExgartuanProwess".Localized());

            OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.DisappointedSideChoice".Localized());

            OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.BlahBlahInterruption".Localized());

            OutputText("Scenes.Shouldrafollower.KeepExgartuanInsteadOfShouldra.StinkEyeDeparture".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartudrama, -1);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraFollowerState, -1);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraSleepTimer, 0);

            DoNext(PlayerMenu);
        }

        public void KeepShouldraAndKickOutExgartuan()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.SideWithShouldraDecision".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.RidOfMeTaunt".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.LaughAndShrugTaunt".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.GrudgeRevelation".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.CockIdentityRetort".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.BallooningArrogance".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.PushingDemonButtons".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.QuestioningMyGreatness".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.SmugPissSlitGlance".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.PlainGreeting".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.NothingSpecialRebuke".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.DevilOfDickingsRage".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.StingingPainSurge".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.HumanComplexionReturns".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.LastPushRequest".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.LimpRelief".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.DemonicPossessionClaim".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.ActualLoadReassurance".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraAndKickOutExgartuan.NewToyRambling".Localized());

            Player.Cocks[0].Id = BodyPartTypes.Human;

            Player.RemoveStatusEffect(StatusEffectIds.Exgartuan);

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartudrama, -0.5f);

            DoNext(PlayerMenu);
        }

        public void KeepShouldraPartIIExgartumonsUndeatH()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIExgartumonsUndeatH.BedChillDread".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIExgartumonsUndeatH.NotSoEasyTaunt".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIExgartumonsUndeatH.BedSpeaksAloud".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIExgartumonsUndeatH.SheetsClosingIn".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartudrama, 0);

            DoNext(() => KeepShouldraPartIIIExgartumonsUndeatH());
        }

        public void KeepShouldraPartIIIExgartumonsUndeatH()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIIExgartumonsUndeatH.PlayfulBoo".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIIExgartumonsUndeatH.HeadFromCovers".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIIExgartumonsUndeatH.FabricPhantomLaugh".Localized());

            OutputText("Scenes.Shouldrafollower.KeepShouldraPartIIIExgartumonsUndeatH.PeacefulNightsSleep".Localized());

            DoNext(PlayerMenu);
        }

        public void KeepAllTheGhosts()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.SharingCockProposal".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.ChampionDisbelief".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.SilverTongueFailure".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.AdjustAnyHoleBoast".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.HoleSizeIndignation".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.ParlorTricksDismissal".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.GreatnessChallenge".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.DraggedToClearing".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.FistClenchStomp".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.GhostGlowsBrightly".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.CreationShowcase".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.FullMastApproval".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.PlayRoughApproval".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.MagnificentCreationOffer".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.ShareTheFunOffer".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.IgnoringSnark".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.DeadSilenceFalls".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.GolemFinishesTask".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.ObscuredByDirtGlobes".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.SwallowedByMounds".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.HoldingEarthenHips".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.SoftMoanEnjoyment".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.WideEyedRelease".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.CumTrailSuffix".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.SentencePeriod".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.GripQuaversAway".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.MutualContentment".Localized());

            OutputText("Scenes.Shouldrafollower.KeepAllTheGhosts.SunriseCleanup".Localized());

            ShouldraSleeping(15, true);

            Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 2, (12 + Random.Shared.Next(7)));

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraExgartudrama, 4);

            DoNext(PlayerMenu);
        }

        public void ShouldraBakeryIntro()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraBakeryIntro.BakeryApproach".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraBakeryIntro.BakeryExcitement".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraBakeryTimes) == 0)
                OutputText("Scenes.Shouldrafollower.ShouldraBakeryIntro.GhostHungerSkepticism".Localized());
            else
                OutputText("Scenes.Shouldrafollower.ShouldraBakeryIntro.PriorBakeryMeal".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraBakeryIntro.SnackEnthusiasm".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Continue".Localized(), FeedShouldraACake);

            AddButton(1, "Common.Button.BailOut".Localized(), BailOut);
        }

        public void BailOut()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.BailOut.RetreatFromBakery".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FeedShouldraACake()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.FeedShouldraACake.NoColdFeet".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraBakeryTimes) > 0)
                OutputText("Scenes.Shouldrafollower.FeedShouldraACake.NothingPermanent".Localized());

            OutputText("Scenes.Shouldrafollower.FeedShouldraACake.RampToBakery".Localized());

            OutputText("Scenes.Shouldrafollower.FeedShouldraACake.MaddieAtCounter".Localized());

            OutputText("Scenes.Shouldrafollower.FeedShouldraACake.ShouldraGetsAntsy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Maddie".Localized(), ShouldraAndMaddieSittingInATree);
        }

        public void ShouldraAndMaddieSittingInATree()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.ReachQueueFront".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.SpookRattlesOffOrder".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.MaddieScribblesOrder".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.ShouldraTakesBooth".Localized());

            if (Player.Tone >= 60)
                OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.WellToned".Localized());
            else
                if (Player.Thickness < 50)
                    OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.Trim".Localized());
                else
                    OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.Curvaceous".Localized());

            OutputText("Scenes.Shouldrafollower.ShouldraAndMaddieSittingInATree.Figure".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GoAlong".Localized(), GoAlongWIthShouldrasEatingSpree);

            AddButton(1, "Common.Button.Resist".Localized(), ResistBeingAFatass);
        }

        public void GoAlongWIthShouldrasEatingSpree()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.ShouldraBakeryTimes) > 0)
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ImpactOnFigure".Localized());

            OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.GiveInToHunger".Localized());

            OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.FinishTheTorte".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.NotUnwelcome".Localized());

            OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.BlushIntoCorner".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
            {
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.BloodRushesDown".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.DrowsyMember".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.And".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.TremblingNetherLips".Localized());

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.Groin".Localized());
            }

            OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.NobodyNotices".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.KinkOfYours".Localized());

            OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.SomethingExtra".Localized());

            if (Player.Femininity <= 50)
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ButtonedShirtJeans".Localized());
            else
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.TankTopAndSkirt".Localized());

            OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.AdornYourPerson".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ExamineNewClothes".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.GrabAnEclair".Localized());

                if (silly)
                    OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.GoodEclair".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.CreamTreatsStuffed".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.GluttonousHaze".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.BumpUnderGut".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.PastryLicksLips".Localized());

                if (Player.Cocks[0].Length < 8 || Player.Cocks[0].Length > 25)
                    OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ArcaneSounds".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.IcingLipsWrap".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.StruggleForDonuts".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.BloatedOrbTatters".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.FeedingRealization".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.CreamyOrgasm".Localized());

                if (Player.CumQ() < 200)
                    OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ModestSpurts".Localized());
                else
                    if (Player.CumQ() <= 400)
                        OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ThickSpurts".Localized());
                    else
                        if (Player.CumQ() <= 1000)
                            OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.PearlJetFlood".Localized());
                        else
                            if (Player.CumQ() <= 5000)
                                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ImmenseLoadBurst".Localized());
                            else
                                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.PotentJizmWad".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ShouldraSqueezesOut".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.NoPermanentDamage".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.LeaveBakery".Localized());

                Player.Orgasm("Dick");

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.ExamineNewClothesAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.GrabAnEclairAgain".Localized());

                if (silly)
                    OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.GoodEclairAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.CreamTreatsStuffedAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.GluttonousHazeAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.MoistnessUnderGut".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.PastryOffersCunt".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.StruggleForDonutsAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.FeedingRealizationAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.CreamyOrgasmAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.NoPermanentDamageAgain".Localized());

                OutputText("Scenes.Shouldrafollower.GoAlongWIthShouldrasEatingSpree.LeaveBakeryAgain".Localized());

                Player.Orgasm("Vaginal");

                DoNext(() => ReturnToCamp(1));
            }

            CoC.Instance.Flags.Set(GameFlagIds.ShouldraBakeryTimes, CoC.Instance.Flags.Get(GameFlagIds.ShouldraBakeryTimes) + 1);

            Player.SlimeFeed();
        }

        public void ResistBeingAFatass()
        {
            ClearOutput();

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.WeightWarning".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.VacantStomach".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.ShouldraSmirks".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.GirlReactionsShift".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.MaddieApproaches".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.SampleOffer".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.BuffetQuestion".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.GirlBlushesHard".Localized());

            if (silly)
                OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.ShouldraSighs".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.StammersPastryBeams".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.ShyGirlFidgets".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.ShouldraSitsDown".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.GirlGasps".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.FatteningTreatFeast".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.GrabsFleshyDome".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.MilkingSound".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.GenerousChallenge".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.InaudibleMouthing".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.FeedeeYellsYes".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.GirlGrabsClothes".Localized());

            OutputText("Scenes.Shouldrafollower.ResistBeingAFatass.NoOneNoticed".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}