// 原版 classes/scenes/npcs/MarblePurification.hx（1592 行；57 个函数）。
// 由 Tools/PortKit/scene-port.mjs 生成骨架后人工补齐：缺失依赖都换成 C# 已有 API 或
// 本文件内的旗标判据；原版行号见每个方法头注释。
//
// 依赖说明：
//   marbleScene.xxx → SceneInstance<MarbleScene>.Get().xxx（MarbleScene 为 6 个 partial 组成）
//   AmilyScene / KihaFollowerScene / IsabellaFollowerScene / SophieBimbo 未移植（原版 NPCAwareContent.hx:130-330 的 convenience 方法），
//   对应的 amilyFollower / followerKiha / isabellaFollower / bimboSophie 用旗标公式内联
//   （公式出处见文件尾 helper 的注释）。
//   knowsAddiction 在 C# 是 Marble 状态值[2]（MarbleScene.Core.cs:61-69）。

using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.OptionSystem;
using System;
using System.Linq;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register("coc:marble_purification")]
    public class MarblePurification : Scene
    {



        private static bool noFur => !Player.HasFur();

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        private static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        private static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        private static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1);

        private static void ReturnToCamp(int hours) => Camp.ReturnToCamp(hours);

        //  saveContent
        private static SaveContent Save => SaveContent(new Identifier("coc", "MarblePurification"));

        public override void Entry()
        {
            // no entry method in the original class (battle-branch scene): entered from area encounters
        }

        // New

        // Haxe BLUHBLUH()   outputText 20 
        public void BLUHBLUH()
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite();
            OutputText("Scenes.Marblepurification.BLUHBLUH.ReturnCampMarbleGloomy".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) <= 1)
            {
                OutputText("Scenes.Marblepurification.BLUHBLUH.HumanWord".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.BLUHBLUH.BovineWord".Localized());
            }
            OutputText("Scenes.Marblepurification.BLUHBLUH.HeadInHandsSighs".Localized());
            OutputText("Scenes.Marblepurification.BLUHBLUH.AskWhatsWrong".Localized());
            if (AmilyFollowerPure() || KihaFollower() || CoC.Instance.Flags.Get(VanillaFlags.IzmaFollowerStatus) == 1)
            {
                OutputText("Scenes.Marblepurification.BLUHBLUH.HasSheBeenFighting".Localized());
                if (AmilyFollowerPure())
                {
                    OutputText("Scenes.Marblepurification.BLUHBLUH.AmilyWord".Localized());
                }
                else
                {
                    if (KihaFollower())
                    {
                        OutputText("Scenes.Marblepurification.BLUHBLUH.KihaWord".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.BLUHBLUH.IzmaWord".Localized());
                    }
                }
                OutputText("Scenes.Marblepurification.BLUHBLUH.QuestionMark".Localized());
            }
            OutputText("Scenes.Marblepurification.BLUHBLUH.GlumNoNothing".Localized());
            OutputText("Scenes.Marblepurification.BLUHBLUH.AppreciateStrongAskHelp".Localized());
            OutputText("Scenes.Marblepurification.BLUHBLUH.HesitantlyLooksUpCorruption".Localized());
            OutputText("Scenes.Marblepurification.BLUHBLUH.ContemplateSuggestions".Localized());
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.PureHoney)))
            {
                OutputText("Scenes.Marblepurification.BLUHBLUH.RecallHoneyBottle".Localized());
            }
            if (SceneInstance<JojoScene>.Get().JojoFollower())
            {
                OutputText("Scenes.Marblepurification.BLUHBLUH.RecallJojoPure".Localized());
            }
            OutputText("Scenes.Marblepurification.BLUHBLUH.ShakesHeadFocus".Localized());
            OutputText("Scenes.Marblepurification.BLUHBLUH.PointOutLabovaRare".Localized());
            OutputText("Scenes.Marblepurification.BLUHBLUH.DeterminedReplyJiggles".Localized());
            OutputText("Scenes.Marblepurification.BLUHBLUH.ProclaimMatesAreFor".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarblePurificationStage, 1);
            ReturnToCamp(1);
        }

        // Haxe GiveMarblePureQuestLabova()   outputText 17 
        public void GiveMarblePureQuestLabova()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedLaBova));
            OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.TellManagedPurifiedLabova".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesGivenMarblePureLabova) == 0)
            {
                Player.ChangeStatusValue(VanillaStatusEffects.Marble, 3, -4);
                OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.BovineMeetsEyesVial".Localized());
                OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.DropsBottleSmashes".Localized());
                if (Player.StatusEffectValue(VanillaStatusEffects.MarbleSpecials, 1) == 4)
                {
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.EightWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.TwoWord".Localized());
                }
                OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.NipplesDischargeSoak".Localized());
            }
            else
            {
                if (Player.StatusEffectValue(VanillaStatusEffects.MarbleSpecials, 3) <= 30)
                {
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.PassBottleChugs".Localized());
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.RepliesDontFeelDifferent".Localized());
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.AskAnyIdeas".Localized());
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.NotSureCorruptionWashes".Localized());
                    CoC.Instance.Flags.Set(VanillaFlags.MarblePurificationStage, 2);
                }
                else
                {
                    Player.ChangeStatusValue(VanillaStatusEffects.Marble, 3, -4);
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.BovineMeetsVialAgain".Localized());
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.HurriesRemoveShirt".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) > 1)
                    {
                        OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.FurCoveredWord".Localized());
                    }
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.BreastsDangleStoically".Localized());
                    if (Player.StatusEffectValue(VanillaStatusEffects.MarbleSpecials, 1) == 4)
                    {
                        OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.EightWordAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.TwoWordAgain".Localized());
                    }
                    OutputText("Scenes.Marblepurification.GiveMarblePureQuestLabova.NipplesSprayMilkEverywhere".Localized());
                }
            }
            CoC.Instance.Flags.Set(VanillaFlags.TimesGivenMarblePureLabova, CoC.Instance.Flags.Get(VanillaFlags.TimesGivenMarblePureLabova) + 1);
            ReturnToCamp(1);
        }

        // Haxe VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin()   outputText 8 
        public void VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin()
        {
            ClearOutput();

            if (!RathazulFollower())
            {
                OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.SpotAlchemistCampLake".Localized());
            }
            OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.ApproachRodentineAlchemist".Localized());
            OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.AProblemPotionBrewer".Localized());
            OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.ExplainMarbleRelationship".Localized((noFur ? "" : " furry")));
            OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.SmileThankReturnMarble".Localized());
            if (!RathazulFollower())
            {
                OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.RathazulStopsYou".Localized());
                OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.ConsideringHisHelp".Localized());
                OutputText("Scenes.Marblepurification.VisitRathazulToPurifyMarbleAfterLaBovaStopsWorkin.LittleTimeGatherThings".Localized());
                SaveContent(new Identifier("coc", "Rathazul")).SetBool("campFollower", true);
            }
            ClearButtons();
            AddButton(0, "Next", PageTwoOfVisitingTheRatForMurblesPurification);
        }

        // Haxe PageTwoOfVisitingTheRatForMurblesPurification()   outputText 5 
        public void PageTwoOfVisitingTheRatForMurblesPurification()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PageTwoOfVisitingTheRatForMurblesPurification.FortunatelyMarbleAtCamp".Localized());
            OutputText("Scenes.Marblepurification.PageTwoOfVisitingTheRatForMurblesPurification.NodHeNeedsSample".Localized());
            OutputText("Scenes.Marblepurification.PageTwoOfVisitingTheRatForMurblesPurification.AcceptBottleThankPeck".Localized());
            OutputText("Scenes.Marblepurification.PageTwoOfVisitingTheRatForMurblesPurification.OldLooksUpGrin".Localized());
            OutputText("Scenes.Marblepurification.PageTwoOfVisitingTheRatForMurblesPurification.ThankHimCalmDown".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarblePurificationStage, 3);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleRathazulCounter1, 24);
            ReturnToCamp(1);
        }

        // Haxe RathazulsMurbelReport()   outputText 8 
        public void RathazulsMurbelReport()
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite();
            CoC.Instance.Flags.Set(VanillaFlags.MarbleRathazulCounter1, 0);
            OutputText("Scenes.Marblepurification.RathazulsMurbelReport.WakeJoinMarble".Localized());
            if (CompanionCount() > 2)
            {
                OutputText("Scenes.Marblepurification.RathazulsMurbelReport.AndOtherFollowers".Localized());
            }
            OutputText("Scenes.Marblepurification.RathazulsMurbelReport.BreakfastBlearyRathazul".Localized());
            OutputText("Scenes.Marblepurification.RathazulsMurbelReport.BadNewsSample".Localized());
            OutputText("Scenes.Marblepurification.RathazulsMurbelReport.TwoSistersMilkQuestion".Localized());
            OutputText("Scenes.Marblepurification.RathazulsMurbelReport.RelativeMilkBest".Localized());
            OutputText("Scenes.Marblepurification.RathazulsMurbelReport.ThenThatsSolutionFalls".Localized());
            OutputText("Scenes.Marblepurification.RathazulsMurbelReport.TellOkayNotReady".Localized());
            ClearButtons();
            AddButton(0, "Go", MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether);
            AddButton(1, "Stay", StayHereAndDontGoGetYerSisterShesABitch);
        }

        // Haxe StayHereAndDontGoGetYerSisterShesABitch()   outputText 3 
        public void StayHereAndDontGoGetYerSisterShesABitch()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.StayHereAndDontGoGetYerSisterShesABitch.ShakeStillNeedYou".Localized());
            OutputText("Scenes.Marblepurification.StayHereAndDontGoGetYerSisterShesABitch.SighsNodsAlright".Localized());
            OutputText("Scenes.Marblepurification.StayHereAndDontGoGetYerSisterShesABitch.ThreeFinishMeals".Localized());
            ReturnToCamp(1);
        }

        // Haxe MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether()   outputText 3 
        public void MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether.TellTimeOffFamily".Localized());
            OutputText("Scenes.Marblepurification.MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether.FaceLightsTightHug".Localized());
            OutputText("Scenes.Marblepurification.MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether.GiveHandTravelKit".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarblePurificationStage, 4);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleRathazulCounter2, 240);
            CoC.Instance.Flags.Set(VanillaFlags.SleepWith, "");
            ReturnToCamp(1);
        }

        // Haxe ClaraShowsUpInCampBECAUSESHESACUNT()   outputText 19 
        public void ClaraShowsUpInCampBECAUSESHESACUNT()
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite();
            CoC.Instance.Flags.Set(VanillaFlags.MarbleRathazulCounter2, 0);
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.GetUpWomenChatting".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.OtherWomanClaraRelated".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) <= 1)
            {
                OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.YourMateHas".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.WhatMateUsedHave".Localized());
            }
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.CowLegsEarsHorns".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.FairIdeaChimesSarcastic".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.NoticeDrawsOutOs".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.HadToMeetYou".Localized());
            if (Player.Intelligence >= 60)
            {
                OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.OddSmileHerFace".Localized());
            }
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.MarbleBurstsLaughing".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.IntroduceHerToHim".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.IntonesTakenAback".Localized());
            if (Player.Intelligence >= 60)
            {
                OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.SomewhatUnconvincingly".Localized());
            }
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.SentencePeriod".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.BeforeChanceMarbleSteps".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.ClaraLooksIrked".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.RathazulInterjectsSuckling".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.ClaraGlaresMarbleInforms".Localized());
            OutputText("Scenes.Marblepurification.ClaraShowsUpInCampBECAUSESHESACUNT.TurnBackRathazulConcerned".Localized());
            ClearButtons();
            AddButton(0, "Next", PartTwoOfClaraShowingUpAndBeingACunt);
        }

        // Haxe PartTwoOfClaraShowingUpAndBeingACunt()   outputText 19 
        public void PartTwoOfClaraShowingUpAndBeingACunt()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.BackCampMorningRoutine".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.HeyThereCharmer".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.AcquaintanceClopsShoulder".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.MarbleComesBackAsks".Localized());
            var teaDrinkers = 0;
            if (CompanionCount() >= 3)
            {
                OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.GatherOthersTea".Localized());
                if (AmilyFollowerPure())
                {
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.ApproachAmilyTea".Localized());
                    teaDrinkers += 1;
                }
                if (BimboSophie())
                {
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.NotSureWhySophie".Localized());
                    teaDrinkers += 1;
                }
                if (SceneInstance<HelScene>.Get().FollowerHel())
                {
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.WalkOverHelHammock".Localized());
                    teaDrinkers += 1;
                }
                if (IsabellaFollower())
                {
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.HeadTowardsIsabellaCorner".Localized());
                    if (IsabellaAccent())
                    {
                        OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.IsabellaJumpsTeaJa".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.IsabellaJumpsTeaYes".Localized());
                    }
                    teaDrinkers += 1;
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.IzmaFollowerStatus) == 1)
                {
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.WalkOverStreamIzma".Localized());
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.WaitMinutesNoResurface".Localized());
                }
                if (SceneInstance<JojoScene>.Get().JojoFollower())
                {
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.GetToJojoSpot".Localized());
                    teaDrinkers += 1;
                }
                if (KihaFollower())
                {
                    OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.KihaRollsEyesTea".Localized());
                    teaDrinkers += 1;
                }
                OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.FinallyHeadRathazulWorkspace".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.SuggestAskRathazulTea".Localized());
                OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.HeadOverRathazulWorkspace".Localized());
            }
            CoC.Instance.Flags.Set(VanillaFlags.MurbleTeaDrinkerCount, teaDrinkers);
            OutputText("Scenes.Marblepurification.PartTwoOfClaraShowingUpAndBeingACunt.RathazulSurprisedDeclines".Localized());
            ClearButtons();
            AddButton(0, "Next", DumbshitCowCuntsTryToDrugYouBecauseTheyreCunts);
        }

        // Haxe DumbshitCowCuntsTryToDrugYouBecauseTheyreCunts()   outputText 2 
        public void DumbshitCowCuntsTryToDrugYouBecauseTheyreCunts()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.DumbshitCowCuntsTryToDrugYouBecauseTheyreCunts.ReturnCenterAnnoyedMarble".Localized());
            OutputText("Scenes.Marblepurification.DumbshitCowCuntsTryToDrugYouBecauseTheyreCunts.KettleWhistles".Localized());
            if (Player.Intelligence >= 60)
            {
                AddButton(0, "Next", () => HighIntelligenceOutcomesVsCowCuntDrugs());
            }
            else
            {
                if (Player.Intelligence >= 30)
                {
                    AddButton(0, "Next", MediumIntelligenceReactsToDrugging);
                }
                else
                {
                    AddButton(0, "Next", LowIntelligenceGetsDruggedByCowSluts);
                }
            }
        }

        // Haxe LowIntelligenceGetsDruggedByCowSluts()   outputText 3 
        public void LowIntelligenceGetsDruggedByCowSluts()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.LowIntelligenceGetsDruggedByCowSluts.EveryoneHasCupClara".Localized());
            OutputText("Scenes.Marblepurification.LowIntelligenceGetsDruggedByCowSluts.SipTeaTaste".Localized());
            OutputText("Scenes.Marblepurification.LowIntelligenceGetsDruggedByCowSluts.MarbleHandForeheadDemands".Localized());
            ClearButtons();
            AddButton(0, "Next", PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap);
        }

        // Haxe PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap()   outputText 8 
        public void PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap()
        {
            ClearOutput();

            Player.Fatigue = 40;
            Player.Health = Player.MaxHealth;
            Player.Lust = 50;
            OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.PleasantWarmFeeling".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.ThinkOfMarbleFace".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.NoticingEyesOpenClara".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.UndergroundCellarSickSmile".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.KidnapperIntones".Localized());
            OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.BodyDesperatelyWants".Localized());
            if (!Player.Weapon.IsAttached())
            {
                OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.WeaponIsNearby".Localized());
            }
            OutputText("Scenes.Marblepurification.PartTwoOfLowIntelligencePCsFallingForTheIncrediblyObviousCowCuntsTrap.StillWearingArmor".Localized());
            ClearButtons();
            AddButton(0, "Fight", FightCowCuntAfterBeingTotesMcGoatsDrugged);
            AddButton(1, "Submit", VoluntarilyChooseCowCuntBadEndYouBad);
        }

        // Haxe FightCowCuntAfterBeingTotesMcGoatsDrugged()   outputText 4 
        public void FightCowCuntAfterBeingTotesMcGoatsDrugged()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.FightCowCuntAfterBeingTotesMcGoatsDrugged.SteelYourself".Localized());
            if (Player.Weapon.IsUnarmed())
            {
                OutputText("Scenes.Marblepurification.FightCowCuntAfterBeingTotesMcGoatsDrugged.RaiseYourWeapon".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.FightCowCuntAfterBeingTotesMcGoatsDrugged.GrabYourWeapon".Localized());
            }
            OutputText("Scenes.Marblepurification.FightCowCuntAfterBeingTotesMcGoatsDrugged.ReadyToFightFree".Localized());
            StartCombatWithMarbleSister(false);
        }

        // Haxe StartCombatWithMarbleSister()   outputText 0 
        public void StartCombatWithMarbleSister(bool inCamp = true)
        {

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "clara")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
            Player.CreateStatusEffect(VanillaStatusEffects.NoFlee, 0, 0, 0, 0);
            if (inCamp && !Player.HasStatusEffect(VanillaStatusEffects.ClaraFoughtInCamp))
            {
                Player.CreateStatusEffect(VanillaStatusEffects.ClaraFoughtInCamp, 0, 0, 0, 0);
            }
        }

        // Haxe VoluntarilyChooseCowCuntBadEndYouBad()   outputText 1 
        public void VoluntarilyChooseCowCuntBadEndYouBad()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.VoluntarilyChooseCowCuntBadEndYouBad.GivingIntoAddiction".Localized());
            ClearButtons();
            AddButton(0, "Next", () => LoseToClara(false));
        }

        // Haxe MediumIntelligenceReactsToDrugging()   outputText 3 
        public void MediumIntelligenceReactsToDrugging()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MediumIntelligenceReactsToDrugging.HandingOutCupsStrange".Localized());
            OutputText("Scenes.Marblepurification.MediumIntelligenceReactsToDrugging.EveryoneHasCupClara".Localized());
            OutputText("Scenes.Marblepurification.MediumIntelligenceReactsToDrugging.DrinkTeaAnyway".Localized());
            ClearButtons();
            AddButton(0, "Drink Tea", LowIntelligenceGetsDruggedByCowSluts);
            AddButton(1, "Don't", MediumIntelligenceDoesntDrinkCowCuntsTea);
        }

        // Haxe MediumIntelligenceDoesntDrinkCowCuntsTea()   outputText 7 
        public void MediumIntelligenceDoesntDrinkCowCuntsTea()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MediumIntelligenceDoesntDrinkCowCuntsTea.CantShakeWrongFeeling".Localized());
            if (CompanionCount() > 1)
            {
                OutputText("Scenes.Marblepurification.MediumIntelligenceDoesntDrinkCowCuntsTea.RushToFallenCompanion".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.MediumIntelligenceDoesntDrinkCowCuntsTea.RushToOneCompanion".Localized());
            }
            OutputText("Scenes.Marblepurification.MediumIntelligenceDoesntDrinkCowCuntsTea.PassedOutTeaCup".Localized());
            OutputText("Scenes.Marblepurification.MediumIntelligenceDoesntDrinkCowCuntsTea.GirlSpeaksIgnoring".Localized());
            OutputText("Scenes.Marblepurification.MediumIntelligenceDoesntDrinkCowCuntsTea.GoneWonderWhy".Localized());
            OutputText("Scenes.Marblepurification.MediumIntelligenceDoesntDrinkCowCuntsTea.GiveInOrFight".Localized());
            ClearButtons();
            AddButton(0, "Fight", MediumIntelligencesFightCowCunts);
            AddButton(1, "Submit", MediumIntelligencesSubmitCowCunts);
        }

        // Haxe MediumIntelligencesFightCowCunts()   outputText 1 
        public void MediumIntelligencesFightCowCunts()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MediumIntelligencesFightCowCunts.ReadyBattleMace".Localized());
            StartCombatWithMarbleSister(true);
        }

        // Haxe MediumIntelligencesSubmitCowCunts()   outputText 1 
        public void MediumIntelligencesSubmitCowCunts()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MediumIntelligencesSubmitCowCunts.SighAcceptCup".Localized());
            ClearButtons();
            AddButton(0, "Next", () => LoseToClara(false));
        }

        // Haxe HighIntelligenceOutcomesVsCowCuntDrugs()   outputText 5 
        public void HighIntelligenceOutcomesVsCowCuntDrugs(int teaDrinkers = 0)
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.HighIntelligenceOutcomesVsCowCuntDrugs.ObviousUpToSomething".Localized());
            OutputText("Scenes.Marblepurification.HighIntelligenceOutcomesVsCowCuntDrugs.DecisionTimeCallOut".Localized());
            if (teaDrinkers <= 1)
            {
                OutputText("Scenes.Marblepurification.HighIntelligenceOutcomesVsCowCuntDrugs.MarbleWord".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.HighIntelligenceOutcomesVsCowCuntDrugs.YourFollowersWord".Localized());
            }
            OutputText("Scenes.Marblepurification.HighIntelligenceOutcomesVsCowCuntDrugs.JustNotDrinkDrink".Localized());
            ClearButtons();
            AddButton(0, "Drink Tea", LowIntelligenceGetsDruggedByCowSluts);
            AddButton(1, "Don't Drink", MediumIntelligenceDoesntDrinkCowCuntsTea);
            AddButton(2, "CallHerOut", HighIntelligenceCallClaraOut);
        }

        // Haxe HighIntelligenceCallClaraOut()   outputText 12 
        public void HighIntelligenceCallClaraOut()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.StopClaraAskWhy".Localized());
            OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.ClaraVisiblyShaken".Localized());
            OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.StunnedSilenceMarbleFalls".Localized());
            if (CompanionCount() >= 3)
            {
                OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.ClaraAdvancesMace".Localized());
                if (AmilyFollower())
                {
                    OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.AmilySnarlsSavage".Localized());
                }
                if (SceneInstance<HelScene>.Get().FollowerHel())
                {
                    OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.HelGrowlsScimitar".Localized());
                }
                if (IsabellaFollower())
                {
                    OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.CowSnarlsStandUp".Localized());
                }
                if (SceneInstance<JojoScene>.Get().JojoFollower())
                {
                    OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.JojoRisesGround".Localized());
                }
                if (KihaFollower())
                {
                    OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.KihaSpitsFlames".Localized());
                }
                OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.StandUpReadyWeapon".Localized());
                ClearButtons();
                AddButton(0, "Next", () => DefeatClaraCuntInAFight(true));
            }
            else
            {
                OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.RipsShirtAdvances".Localized());
                OutputText("Scenes.Marblepurification.HighIntelligenceCallClaraOut.MarbleDownYourOwn".Localized());
                StartCombatWithMarbleSister(true);
            }
        }

        // Haxe LoseToClara()   outputText 12 
        public void LoseToClara(bool combat = true)
        {
            ClearOutput();

            if (combat)
            {
                if (Player.Health < 1)
                {
                    OutputText("Scenes.Marblepurification.LoseToClara.BodyGivesOutStrain".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.LoseToClara.CantFightItMasturbate".Localized());
                }
                OutputText("Scenes.Marblepurification.LoseToClara.ParagraphBreak".Localized());
            }
            OutputText("Scenes.Marblepurification.LoseToClara.DreamOddDistorted".Localized());
            OutputText("Scenes.Marblepurification.LoseToClara.GreatEffortOpenEyes".Localized());
            OutputText("Scenes.Marblepurification.LoseToClara.EnclosedStoneCellar".Localized());
            if (Player.Tallness < 70)
            {
                OutputText("Scenes.Marblepurification.LoseToClara.MuchTooBigWord".Localized());
            }
            else
            {
                if (Player.Tallness <= 82)
                {
                    OutputText("Scenes.Marblepurification.LoseToClara.JustYourSizeWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.LoseToClara.WayTooSmallWord".Localized());
                }
            }
            OutputText("Scenes.Marblepurification.LoseToClara.FindNotWearingArmor".Localized());
            if ((Player.HasCock() && Player.SmallestCockArea() <= 40) && Player.BiggestBreastSize() <= 25 && !Player.IsTaur() && !Player.HasTailInsteadOfLegs() && !Player.IsGoo() && !Player.IsDrider())
            {
                OutputText("Scenes.Marblepurification.LoseToClara.PlainSlimTwoPiece".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.LoseToClara.CompletelyNaked".Localized());
            }
            ClearButtons();
            AddButton(0, "Next", LosingToCowCuntsPageII);
        }

        // Haxe LosingToCowCuntsPageII()   outputText 18 
        public void LosingToCowCuntsPageII()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.RealizeFamiliarUrge".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.AsCueCaptor".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.LaughsAgainMilkSlave".Localized());
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 2) /* 原版 marbleScene.knowAddiction；Core.cs:61-69 */ == 1)
            {
                OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.WonderingHowIKnow".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.ConfusedMarbleResearched".Localized());
            }
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.NothingLikeHerSister".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.LeansDownFace".Localized());
            if (Camp.LoversCount() == 1)
            {
                OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.MarbleWord".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.TheOthersWord".Localized());
            }
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.EyesDropToChest".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.OrderingDeaths".Localized(
                Player.Mf("lover-boy", "lover-girl")
            ));
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.HesitateSlapped".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.WhisperILoveYou".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.YellsLouder".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.ILoveOnlyYou".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.YellsLouderHugeSmile".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.ILoveOnlyShout".Localized());
            OutputText("Scenes.Marblepurification.LosingToCowCuntsPageII.MadWomanShrieksYesssss".Localized());
            ClearButtons();
            AddButton(0, "Next", CowCuntBadEndPartIII);
        }

        // Haxe CowCuntBadEndPartIII()   outputText 7 
        public void CowCuntBadEndPartIII()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIII.GazeNotPleasantCoos".Localized());
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIII.FloodgatesRushNipple".Localized());
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIII.DistantVoiceEnthusiasm".Localized());
            if (Player.Body.Get<HairPart>().Length > 0)
            {
                OutputText("Scenes.Marblepurification.CowCuntBadEndPartIII.ThroughYourHair".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.CowCuntBadEndPartIII.AcrossYourScalp".Localized());
            }
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIII.GripDownTightDrink".Localized());
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIII.SuckHardYouCan".Localized(
                Player.Mf("lover-boy", "lover-girl")
            ));
            ClearButtons();
            AddButton(0, "Next", CowCuntBadEndPartIV);
        }

        // Haxe CowCuntBadEndPartIV()   outputText 9 
        public void CowCuntBadEndPartIV()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.ChanceSeeSofterSide".Localized());
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.HandMovesDownBody".Localized());
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.PokingsWindDown".Localized());
            if (Player.HasCock() && !Player.HasVagina())
            {
                OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.CocksRockHardStrokes".Localized());
            }
            else
            {
                if (Player.HasVagina() && !Player.HasCock())
                {
                    OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.WetGivesRubs".Localized());
                }
                else
                {
                    if (Player.HasCock() && Player.HasVagina())
                    {
                        OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.ArousalQuiteApparent".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.RatherLackThereof".Localized());
                    }
                }
            }
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.ApparentWhatSheWants".Localized());
            OutputText("Scenes.Marblepurification.CowCuntBadEndPartIV.OverArousedCumSpot".Localized());
            ClearButtons();
            if (!Player.HasCock())
            {
                AddButton(0, "Next", CocklessPCsGetAbusedByCowCunts);
            }
            else
            {
                if (Player.CockThatFits(20) < 0)
                {
                    AddButton(0, "Next", CockTooBigForCowCuntageBadEndage);
                }
                else
                {
                    AddButton(0, "Next", BadEndDickThatFitsClara);
                }
            }
        }

        // Haxe BadEndDickThatFitsClara()   outputText 8 
        public void BadEndDickThatFitsClara()
        {
            ClearOutput();

            var x = Player.CockThatFits(20);
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            if ((Player.HasCock() && Player.SmallestCockArea() <= 40) && Player.BiggestBreastSize() <= 25 && !Player.IsTaur() && !Player.HasTailInsteadOfLegs() && !Player.IsGoo() && !Player.IsDrider())
            {
                OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.SecondsFreedGarment".Localized());
            }
            OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.StillErectFromNursing".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.NotWastingTime".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.WomanSexToySlowly".Localized());
            OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.StopsTeasingLowers".Localized());
            OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.FearsDispelled".Localized());
            OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.SexBlurSensations".Localized());
            OutputText("Scenes.Marblepurification.BadEndDickThatFitsClara.BothExhaustedCollapses".Localized());
            ClearButtons();
            AddButton(0, "Next", FinalBadEndWithCowCunt);
        }

        // Haxe CockTooBigForCowCuntageBadEndage()   outputText 8 
        public void CockTooBigForCowCuntageBadEndage()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.OneProblemSayStart".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.AllPenisesAre".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.YourPenisIs".Localized());
            }
            OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.TooBigVirginFit".Localized());
            if (Player.HasCockThatFits(SceneInstance<MarbleScene>.Get().MarbleCuntCapacity()))
            {
                OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.NotAFlawFeature".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.MarblesFrustrations".Localized());
            }
            OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.RantsSeveralMinutes".Localized());
            OutputText("Scenes.Marblepurification.CockTooBigForCowCuntageBadEndage.CantFuckSomethingElse".Localized());
            ClearButtons();
            AddButton(0, "Next", CocklessPCsGetAbusedByCowCunts);
        }

        // Haxe CocklessPCsGetAbusedByCowCunts()   outputText 17 
        public void CocklessPCsGetAbusedByCowCunts()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.StraddlesHeadLick".Localized());
            if (Player.Body.Get<TonguePart>().Id != VanillaBodyPartTypes.Human)
            {
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.OrdersLickHole".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.StartAgainJustTip".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.ObeyHigherClit".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.SettlesDownMore".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.SquelchingHerWomanhood".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.TorrentOfFluid".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.ReleasesHeadGrip".Localized(
                    Player.Mf("lover-boy", "lover-girl")
                ));
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.NewSleepingArrangements".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.OrdersLickTentatively".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.DoesntSoundSure".Localized());
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.GirlSecondOrgasm".Localized());
                if (Player.HasMuzzle())
                {
                    OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.TryDoWhatBefore".Localized());
                    OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.DoesntTakeLongFinish".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.ResumeLickingAwkward".Localized());
                    OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.GoingFastBarelyBreath".Localized());
                }
                OutputText("Scenes.Marblepurification.CocklessPCsGetAbusedByCowCunts.RecoverNoticeDisappointed".Localized());
            }
            ClearButtons();
            AddButton(0, "Next", FinalBadEndWithCowCunt);
        }

        // Haxe FinalBadEndWithCowCunt()   outputText 14 
        public void FinalBadEndWithCowCunt()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.YearsLater".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.WalkingForestedPath".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.DestinationClearing".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.FriendlyFoxStrawberries".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.TimidVoiceCalls".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.PatFelineHead".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.SecondFloorLanding".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.StruggleFearfulJoyful".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.ClaraGrownYears".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.CoomeHereMilkSlave".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.MistressGoddessServe".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.FinallyTurnsSatisfaction".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.FinishedDrinkingLieDown".Localized());
            OutputText("Scenes.Marblepurification.FinalBadEndWithCowCunt.LifeReducedMilkSlave".Localized());
            GameOver();
        }

        // Haxe DefeatClaraCuntInAFight()   outputText 42 
        public void DefeatClaraCuntInAFight(bool cheated = false)
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite();
            if (!(cheated))
            {
                if (Player.StatusEffectValue(VanillaStatusEffects.ClaraCombatRounds, 0) >= 10 && Player.HasStatusEffect(VanillaStatusEffects.ClaraFoughtInCamp))
                {
                    OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.LoudBoomHammerStrike".Localized());
                }
                else
                {
                    if (!Player.HasStatusEffect(VanillaStatusEffects.ClaraFoughtInCamp))
                    {
                        OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.WorriedOthersDragClara".Localized());
                    }
                    else
                    {
                        if (CompanionCount() <= 2)
                        {
                            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.IncapacitatedCheckMarble".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.IncapacitatedCheckFollowers".Localized());
                        }
                    }
                }
                OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.ParagraphBreak".Localized());
            }
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.DemandsWhatTryingPull".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.GettingAMate".Localized(
                Player.Mf("lover-boy", "lover-girl")
            ));
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.GettingOutOfSweetie".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.NotAddictedNotYours".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.ShakingGettingWorse".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.DadAddictedToMom".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.DaddyWonderfulPerson".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.YesStupidSis".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.LooksAtHandsHorror".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.ClaraFinallyGettingIt".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.ClenchesHandsSeethes".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.AdmitDoesntCare".Localized(
                Player.Mf("lover-boy", "lover-girl"),
                Player.Mf("him", "her")
            ));
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.CompanionShakesAnger".Localized());
            if (Camp.LoversCount() > 1)
            {
                OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.NotEvenOnlyMate".Localized());
            }
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.FrustrationNotNursing".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.BelongTogether".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.AskFirstHusband".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.LaughsDeserveMore".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.SaysUnderBreath".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.WhatWasThatSis".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.DeepBreathIsntLove".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.ScoffsHaventListened".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.CorruptInstinctsTellUs".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.NurseFromYouAgain".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.ShadowCrossesFace".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.WhyCantHeChange".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.SwallowsAnyIdea".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.BurstsLaughingSaveWorld".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.QuiveringInRage".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.NoDamnWayPathetic".Localized()
                Player.ShortName
            );
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.MuseCowGirlEndurance".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.DoesntStandAChance".Localized());
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.HowPunishClara".Localized());
            if (Player.HasCockThatFits(20) || Player.HasVagina())
            {
                OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.UseUnstableStateThreesome".Localized());
            }
            if (Player.HasCockThatFits(SceneInstance<MarbleScene>.Get().MarbleCuntCapacity()) || Player.HasVagina())
            {
                OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.SexFrontOfClara".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.EggMarbleViolateCock".Localized());
            }
            OutputText("Scenes.Marblepurification.DefeatClaraCuntInAFight.OrCalmMarbleDown".Localized());
            ClearButtons();
            AddButton(0, "Spank", MurbleSpanksCowCunt);
            if (Player.HasCockThatFits(20) || Player.HasVagina())
            {
                AddButton(1, "Threesome", PcAndMurbleDoubleTeamCowButt);
            }
            if (Player.HasCockThatFits(SceneInstance<MarbleScene>.Get().MarbleCuntCapacity()) || Player.HasVagina())
            {
                AddButton(2, "NTR", NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG);
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                AddButton(3, "Marble Cock", FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock);
            }
            AddButton(4, "Calm Down", CalmMurblesFatAssDown);
        }

        // Haxe MurbleSpanksCowCunt()   outputText 13 
        public void MurbleSpanksCowCunt()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.MarbleGoodIdeaBack".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.OlderSisterOverKnee".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.NotUntilYouLearn".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.WhatOoe".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.SuggestMoreEffect".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.WhiteHairedBottomAir".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.LoudSharpSmackNo".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.SayYoureSorryGritted".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.AnotherSmackIsntFair".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.RaisesArmSaySorry".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.DoesntBotherReasoning".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.MentionLookAtSister".Localized());
            OutputText("Scenes.Marblepurification.MurbleSpanksCowCunt.PicksSisterOffGround".Localized());
            ClearButtons();
            AddButton(0, "Next", CalmMurblesFatAssDown);
        }

        // Haxe PcAndMurbleDoubleTeamCowButt()   outputText 42 
        public void PcAndMurbleDoubleTeamCowButt()
        {
            ClearOutput();

            var x = Player.CockThatFits(20);
            OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.SuggestWorkTogetherPunish".Localized());
            OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.DontWantThreesomeSister".Localized());
            OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.OutburstConvincesLover".Localized());
            OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.StripMalefactorConsider".Localized());
            if ((Player.HasCock() && x >= 0) && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
            {
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.PullUpSandwich".Localized());
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.LearnedYourLesson".Localized());
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.FuckNooSweetieMine".Localized());
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.LooksToSideNod".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.SharpGaspPushAss".Localized());
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.WipeSweatContinue".Localized());
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.AnusClampsDown".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.AnusMoreRelaxed".Localized());
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.NotMuchLongerPeaks".Localized());
                OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.WhyNotFrontNever".Localized());
            }
            else
            {
                if ((Player.HasCock() && x >= 0) && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) == 0)
                {
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.PushDownFoursBehind".Localized());
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.ResponseTwistEars".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.InGoesLengthMoop".Localized());
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.NoQuestionFirstClaim".Localized());
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.SavorUnpredictablenessTurn".Localized());
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.ExpressionChangesMoan".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.MuffledLoudMoo".Localized());
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.EnjoyingHerselfSecondTime".Localized());
                    OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.DidntYoouMilkTaste".Localized());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
                    {
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.PushDownFoursFront".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.GrabEarsTug".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.LowSlurpMoop".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.SighSeeHowMarble".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.OwnExperienceUnderwhelmed".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.FeelNaturalPowerTongue".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.ExpressDisappointment".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.EnjoyingCumsSecond".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.LooksAboutToProtest".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.RollBackSitsFace".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.LittleTriangleCanThink".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.MateRespondsGrind".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.LiftsSlightlySuggests".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.SilencedByMmph".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.ExperienceMarblesTongue".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.YoungCowSquirmsStruggles".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.OwnMoanMinistrations".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.SameTimeLoudMoo".Localized());
                        OutputText("Scenes.Marblepurification.PcAndMurbleDoubleTeamCowButt.SweatyBodiesSeparate".Localized());
                    }
                }
            }
            Player.Orgasm("Dick");
            ClearButtons();
            AddButton(0, "Next", CalmMurblesFatAssDown);
        }

        // Haxe NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG()   outputText 13 
        public void NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG()
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite(true);
            OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.YouWord".Localized());
            OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.StepForwardTellMarble".Localized());
            OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.ProperLovemakingNotKissing".Localized());
            OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.LetPartnerLead".Localized());
            OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.RackRidingMainEvent".Localized());
            var x = Player.CockThatFits(SceneInstance<MarbleScene>.Get().MarbleCuntCapacity());
            if (Player.HasCock() && x >= 0)
            {
                OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.DropsOntoYouSince".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.NoHesitationRidden".Localized());
            }
            else
            {
                if (Player.HasVagina() && CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) > 0)
                {
                    OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.DropsOntoGroundLifts".Localized());
                    OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.NoHesitationInvaded".Localized());
                }
                else
                {
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.DropsOntoLegsWomanhoods".Localized());
                        OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.BeastAwakenedInMarble".Localized());
                    }
                }
            }
            OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.RoughSexShortEventful".Localized());
            OutputText("Scenes.Marblepurification.NTRIsClearlyTheWorstFetishWhyWouldYouWriteThisOMG.CauseSadnessVerbalAbuse".Localized());
            Player.Orgasm("Generic");
            ClearButtons();
            AddButton(0, "Next", CalmMurblesFatAssDown);
        }

        // Haxe FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock()   outputText 11 
        public void FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.CallOutUseAppendage".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.WhatNewAppendage".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.OlderSisterHesitates".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.WhatNoWouldI".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.SquatsFrontOfOpening".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.FutaCockMeetsEntrance".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.WishesNotMet".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.ForHerPartEnjoying".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.PacedThrustsFrantic".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.LoudMoooCrescendo".Localized());
            OutputText("Scenes.Marblepurification.FutaMarbleIsAHugeCowToCowCuntAndStuffsCowCuntsCuntFullOfCowCock.FutaPantsAsks".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Lust, 20f));
            ClearButtons();
            AddButton(0, "Next", CalmMurblesFatAssDown);
        }

        // Haxe CalmMurblesFatAssDown()   outputText 6 
        public void CalmMurblesFatAssDown()
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite();
            OutputText("Scenes.Marblepurification.CalmMurblesFatAssDown.EnoughHandOnShoulder".Localized());
            if ((Player.Tallness >= 80 && CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) < 2) || (Player.Tallness >= 86 && CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) >= 2))
            {
                OutputText("Scenes.Marblepurification.CalmMurblesFatAssDown.HugsAgainstBody".Localized());
            }
            else
            {
                if (Player.Tallness >= 60)
                {
                    OutputText("Scenes.Marblepurification.CalmMurblesFatAssDown.ReturnsHugRocking".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.CalmMurblesFatAssDown.ScoopsUpHugTightly".Localized());
                }
            }
            OutputText("Scenes.Marblepurification.CalmMurblesFatAssDown.SeparateSeeHerFace".Localized());
            OutputText("Scenes.Marblepurification.CalmMurblesFatAssDown.SuggestCheckRathazul".Localized());
            ClearButtons();
            AddButton(0, "Next", RathazulFinishingMarblesPotion);
        }

        // Haxe RathazulFinishingMarblesPotion()   outputText 9 
        public void RathazulFinishingMarblesPotion()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.ColorfulEventsUnnoticed".Localized());
            OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.MateAsksReady".Localized());
            OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.NotQuiteHalfHour".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0)
            {
                OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.ThenPurifyYouChild".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.RenWord".Localized());
                }
                OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.SentencePeriod".Localized());
            }
            OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.ComeBackLaterRodent".Localized());
            OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.TurnCowGirlAsk".Localized());
            OutputText("Scenes.Marblepurification.RathazulFinishingMarblesPotion.TimeToKill".Localized());
            ClearButtons();
            AddButton(0, "Next", TheFormulaIsDone);
        }

        // Haxe TheFormulaIsDone()   outputText 26 
        public void TheFormulaIsDone()
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite();
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.FormulaReadyTellMarble".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.FindHerRuinedWall".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.AssureOkayCondition".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.CalmlyTellCertain".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.ForcedSmileFades".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.RecallClaraRevealedParents".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.SenseOfDreadGo".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.EvenIfTrueLove".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.ButSweetieTheThing".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.AnotherBreathSternLook".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.WarmSmileCutestThings".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.UnfortunatelyDidntLast".Localized());
            if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 2) /* 原版 marbleScene.knowAddiction；Core.cs:61-69 */ == 1)
            {
                OutputText("Scenes.Marblepurification.TheFormulaIsDone.WhenSaidWantedRely".Localized());
            }
            else
            {
                OutputText("Scenes.Marblepurification.TheFormulaIsDone.WhenToldWhyCouldnt".Localized());
            }
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.KnowWhatHappenedNext".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.RememberHowItStarted".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.MarbleLaughsBarelyLasted".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.MostTroublingPartMom".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.LooksAtYouPassively".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.ReassureEvenIfTruth".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.BurstsBackBreakingHug".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.JumpDownRemindRedress".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.JustBeforeExpressionDarkens".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.MarbleSnortsPeopleClara".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.CutMonstersClaws".Localized());
            OutputText("Scenes.Marblepurification.TheFormulaIsDone.PurifyHerBrilliant".Localized());
            ClearButtons();
            AddButton(0, "Next", PurifyMarbleComplete);
        }

        // Haxe PurifyMarbleComplete()   outputText 33 
        public void PurifyMarbleComplete()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.ArriveRathazulWorkspace".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.YesOneDose".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.HoldsHandOutCup".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.ConsiderBreastsSeem".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.TopOutReturnCup".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.GrimacesTasteHandsCup".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.GrabsBreastsWeigh".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.CallOutInWorry".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.OneWayKnowTaste".Localized());
            OutputText("Scenes.Marblepurification.PurifyMarbleComplete.RathazulCriesSuccess".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0)
            {
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.NowKnowWorksChild".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.RenWord".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.RightAwayGive".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.HerWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.EachWord".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.TwoDropDoseInstructions".Localized());
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.WasteNoTimeNursery".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.EachOfWord".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.YourChildWord".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.RenWordAgain".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.TwoDropsOfFormula".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.TheyWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.SheWord".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.TakeWord".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.PluralS".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.EasilyNoOutwardEffects".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.YourWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.EachWordAgain".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.ChildReportsSomethingLifted".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.TheirWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblepurification.PurifyMarbleComplete.HerWordAgain".Localized());
                }
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.MindWord".Localized());
                OutputText("Scenes.Marblepurification.PurifyMarbleComplete.LightHeartsReturn".Localized());
            }
            ClearButtons();
            AddButton(0, "Next", DealingWithCowCuntPostPurification);
        }

        // Haxe DealingWithCowCuntPostPurification()   outputText 8 
        public void DealingWithCowCuntPostPurification()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.OneMoreLooseEnd".Localized());
            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.StillLayingTiedUp".Localized());
            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.MarbleRespondsKindThinks".Localized());
            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.ClaraCongratulationsNotLacta".Localized(
                Player.Mf("lover-boy", "lover-girl")
            ));
            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.CantBelieveWhyBothering".Localized());
            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.DismissesFuckOff".Localized());
            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.SighsSpeaksOnceMore".Localized());
            OutputText("Scenes.Marblepurification.DealingWithCowCuntPostPurification.PurifyingFittingPunishment".Localized());
            ClearButtons();
            AddButton(0, "Purify", PurifyMurblesSister);
            AddButton(1, "Don't", DontPurifyClara);
        }

        // Haxe PurifyMurblesSister()   outputText 9 
        public void PurifyMurblesSister()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PurifyMurblesSister.AskForBottleFormula".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.ApproachBoundClara".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.ConsidersBottleCurlsLip".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.MomentsLaterDrank".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.PurifiedLabovaWorst".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.GivenTriedTakeLife".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.ProcessFinishesSilent".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.BuildCageKeepEye".Localized());
            OutputText("Scenes.Marblepurification.PurifyMurblesSister.EspeciallyGenerousLetGo".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.ClaraPurified, 1);
            ClearButtons();
            AddButton(0, "Cage", CageDatCowCunt);
            AddButton(1, "Let Go", LetGoOfMarblesCowCuntSister);
        }

        // Haxe CageDatCowCunt()   outputText 2 
        public void CageDatCowCunt()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.CageDatCowCunt.SayKeepClaraPrisoner".Localized());
            OutputText("Scenes.Marblepurification.CageDatCowCunt.TakesHourBuildCage".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.ClaraImprisoned, 1);
            ClearButtons();
            AddButton(0, "Next", PurificationQuestFinal);
        }

        // Haxe LetGoOfMarblesCowCuntSister()   outputText 2 
        public void LetGoOfMarblesCowCuntSister()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.LetGoOfMarblesCowCuntSister.SayFreeGoPromise".Localized());
            OutputText("Scenes.Marblepurification.LetGoOfMarblesCowCuntSister.UntieGiveThingsEscort".Localized());
            ClearButtons();
            AddButton(0, "Next", PurificationQuestFinal);
        }

        // Haxe DontPurifyClara()   outputText 3 
        public void DontPurifyClara()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.DontPurifyClara.OfferMercyKeepCorruption".Localized());
            OutputText("Scenes.Marblepurification.DontPurifyClara.BuildCageKeepEye".Localized());
            OutputText("Scenes.Marblepurification.DontPurifyClara.EspeciallyGenerousLetGo".Localized());
            ClearButtons();
            AddButton(0, "Cage", StickUnpurifiedClaraInACage);
            AddButton(1, "Let Go", LetCorruptCowCuntGo);
        }

        // Haxe StickUnpurifiedClaraInACage()   outputText 3 
        public void StickUnpurifiedClaraInACage()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.StickUnpurifiedClaraInACage.SayKeepClaraPrisoner".Localized());
            OutputText("Scenes.Marblepurification.StickUnpurifiedClaraInACage.ProtestsCageCantDo".Localized());
            OutputText("Scenes.Marblepurification.StickUnpurifiedClaraInACage.TakesHourBuildCage".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.ClaraImprisoned, 1);
            ClearButtons();
            AddButton(0, "Next", PurificationQuestFinal);
        }

        // Haxe LetCorruptCowCuntGo()   outputText 3 
        public void LetCorruptCowCuntGo()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.LetCorruptCowCuntGo.SayLetPromiseNever".Localized());
            OutputText("Scenes.Marblepurification.LetCorruptCowCuntGo.ReallyLetMeGo".Localized());
            OutputText("Scenes.Marblepurification.LetCorruptCowCuntGo.ReleaseBondsEscortCamp".Localized());
            ClearButtons();
            AddButton(0, "Next", PurificationQuestFinal);
        }

        // Haxe PurificationQuestFinal()   outputText 2 
        public void PurificationQuestFinal()
        {
            ClearOutput();

            SceneInstance<MarbleScene>.Get().MarbleSprite();
            OutputText("Scenes.Marblepurification.PurificationQuestFinal.QuiteTheMorning".Localized());
            OutputText("Scenes.Marblepurification.PurificationQuestFinal.Gained500ExpPurity".Localized());
            Player.Experience += 500;
            CoC.Instance.Flags.Set(VanillaFlags.MarblePurificationStage, 5);
            CoC.Instance.Flags.Set(VanillaFlags.MarblePurified, 1);
            Player.ChangeStatusValue(VanillaStatusEffects.Marble, 3, 10);
            CombatController.InCombat = false;
            Player.ClearStatuses();
            ReturnToCamp(4);
        }

        // Haxe PurifiedSuccubusMilkForPureMarbles()   outputText 1 
        public void PurifiedSuccubusMilkForPureMarbles()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PurifiedSuccubusMilkForPureMarbles.AskGrowBreastsBigger".Localized());
            ClearButtons();
            AddButton(0, "Yes", ActuallyGrowPureMarblesTittiesForFunzies);
            AddButton(1, "No", SceneInstance<MarbleScene>.Get().GiveItem);
        }

        // Haxe ActuallyGrowPureMarblesTittiesForFunzies()   outputText 7 
        public void ActuallyGrowPureMarblesTittiesForFunzies()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.ActuallyGrowPureMarblesTittiesForFunzies.OfferSuccubusMilkGrow".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) < 3)
            {
                OutputText("Scenes.Marblepurification.ActuallyGrowPureMarblesTittiesForFunzies.LiftsBottleDrinks".Localized(SceneInstance<MarbleScene>.Get().MarbleBreastSize()));
                CoC.Instance.Flags.Set(VanillaFlags.MarbleBreastSize, CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) + 1);
                OutputText("Scenes.Marblepurification.ActuallyGrowPureMarblesTittiesForFunzies.MelonsWord".Localized(SceneInstance<MarbleScene>.Get().MarbleBreastSize()));
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) == 1)
                {
                    OutputText("Scenes.Marblepurification.ActuallyGrowPureMarblesTittiesForFunzies.ExaminesLargerBust".Localized());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) == 2)
                    {
                        OutputText("Scenes.Marblepurification.ActuallyGrowPureMarblesTittiesForFunzies.ShakesChestSmiles".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.ActuallyGrowPureMarblesTittiesForFunzies.GripsMassiveMammaries".Localized());
                    }
                }
                Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk));
                CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) + 5);
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) > 100)
                {
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 100);
                }
            }
            else
            {
                OutputText("Scenes.Marblepurification.ActuallyGrowPureMarblesTittiesForFunzies.HandsBackBottleSorry".Localized());
            }
            SceneInstance<MarbleScene>.Get().GiveItem();
        }

        // Haxe PureMurbleUsesReducto()   outputText 1 
        public void PureMurbleUsesReducto()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PureMurbleUsesReducto.AskShrinkBreastsDown".Localized());
            ClearButtons();
            AddButton(0, "Yes", PureMurblePCChoosesYesToShrinkeyTits);
            AddButton(1, "No", SceneInstance<MarbleScene>.Get().GiveItem);
        }

        // Haxe PureMurblePCChoosesYesToShrinkeyTits()   outputText 7 
        public void PureMurblePCChoosesYesToShrinkeyTits()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PureMurblePCChoosesYesToShrinkeyTits.OfferReductoJar".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) > 0)
            {
                OutputText("Scenes.Marblepurification.PureMurblePCChoosesYesToShrinkeyTits.OpensJarLiftsTop".Localized(SceneInstance<MarbleScene>.Get().MarbleBreastSize()));
                CoC.Instance.Flags.Set(VanillaFlags.MarbleBreastSize, CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) - 1);
                OutputText("Scenes.Marblepurification.PureMurblePCChoosesYesToShrinkeyTits.SizedBreastsWord".Localized(SceneInstance<MarbleScene>.Get().MarbleBreastSize()));
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) == 0)
                {
                    OutputText("Scenes.Marblepurification.PureMurblePCChoosesYesToShrinkeyTits.GripsSizableDDs".Localized());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBreastSize) == 1)
                    {
                        OutputText("Scenes.Marblepurification.PureMurblePCChoosesYesToShrinkeyTits.ExperimentalBounce".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.PureMurblePCChoosesYesToShrinkeyTits.RollsShouldersStretches".Localized());
                    }
                }
                Player.ConsumeItem(ItemRegistry.Create(VanillaItems.Reducto));
                CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) - 5);
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 0)
                {
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
                }
            }
            else
            {
                OutputText("Scenes.Marblepurification.PureMurblePCChoosesYesToShrinkeyTits.HandsBackJarTiny".Localized());
            }
            SceneInstance<MarbleScene>.Get().GiveItem();
        }

        // Haxe LactaidForPureMurble()   outputText 1 
        public void LactaidForPureMurble()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.LactaidForPureMurble.HandBottleLactaid".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarbleTimeSinceNursedInHours, CoC.Instance.Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) + 10);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) + 20);
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) < 0)
            {
                CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 0);
            }
            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.Lactaid));
            SceneInstance<MarbleScene>.Get().GiveItem();
        }

        // Haxe NursingFromPureMarble()   outputText 12 
        public void NursingFromPureMarble()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) < 4)
            {
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.ThirstyAskForMilk".Localized());
                SceneInstance<MarbleScene>.Get().InteractWithMarbleAtCamp();
            }
            else
            {
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.ThirstyAskMilkAgain".Localized());
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.ArriveLoverTeasesBreasts".Localized(SceneInstance<MarbleScene>.Get().MarbleBreastSize()));
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.SultrySmileHooksShirt".Localized());
                if (Player.Tallness < 65)
                {
                    OutputText("Scenes.Marblepurification.NursingFromPureMarble.TallCowTakesSeat".Localized());
                }
                else
                {
                    if (Player.Tallness < 84)
                    {
                        OutputText("Scenes.Marblepurification.NursingFromPureMarble.LeanForwardAnd".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblepurification.NursingFromPureMarble.LiftUpCowLady".Localized());
                    }
                }
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.LipsWrappedTeats".Localized());
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.PureMilkNoAddiction".Localized());
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.SavorTasteSpecialAttention".Localized());
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.DrinkAchesWashAway".Localized());
                Player.RefillHunger(30);
                OutputText("Scenes.Marblepurification.NursingFromPureMarble.TenderMomentEnds".Localized());
                Player.HealthChange((float)(Player.MaxHealth * .2f));
                Player.ChangeFatigue(-30);
                Player.ApplyStats(new DynStat(VanillaStats.Lust, 15f));
                CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) + 10);
                CoC.Instance.Flags.Set(VanillaFlags.MarbleTimeSinceNursedInHours, 0);
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) > 0)
                {
                    CoC.Instance.Flags.Set(VanillaFlags.MarbleLust, 100);
                }
                ReturnToCamp(1);
            }
        }

        // Haxe MarbleWarnsPCAboutCorruption()   outputText 2 
        public void MarbleWarnsPCAboutCorruption()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MarbleWarnsPCAboutCorruption.ComesUpWorried".Localized());
            OutputText("Scenes.Marblepurification.MarbleWarnsPCAboutCorruption.RunsOffNotBack".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarbleWarnedAboutCorruption, 1);
            // 原版 MarblePurification.hx:1497 doNext(playerMenu)
            Scene.PlayerMenu();
        }

        // Haxe MarbleLeavesThePCOverCorruption()   outputText 7 
        public void MarbleLeavesThePCOverCorruption()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.MarbleLeavesThePCOverCorruption.ApproachesSomberPacked".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
            {
                OutputText("Scenes.Marblepurification.MarbleLeavesThePCOverCorruption.AndChildrenInTow".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                {
                    OutputText("Scenes.Marblepurification.MarbleLeavesThePCOverCorruption.AndHoldingChildsHand".Localized());
                }
            }
            OutputText("Scenes.Marblepurification.MarbleLeavesThePCOverCorruption.StopsTakesDeepBreath".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0)
            {
                OutputText("Scenes.Marblepurification.MarbleLeavesThePCOverCorruption.MommyWhatsWrong".Localized());
            }
            OutputText("Scenes.Marblepurification.MarbleLeavesThePCOverCorruption.TooCorruptedCantStay".Localized());
            OutputText("Scenes.Marblepurification.MarbleLeavesThePCOverCorruption.WomanHelpedLeavesBehind".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLeftOverCorruption, 1);
            if (CoC.Instance.Flags.Get(VanillaFlags.SleepWith) == "Marble")
            {
                CoC.Instance.Flags.Remove(VanillaFlags.SleepWith);
            }
            Player.RemoveStatusEffect(VanillaStatusEffects.CampMarble);
            ReturnToCamp(1);
        }

        // Haxe PureMarbleDecidesToBeLessOfABitch()   outputText 5 
        public void PureMarbleDecidesToBeLessOfABitch()
        {
            ClearOutput();

            OutputText("Scenes.Marblepurification.PureMarbleDecidesToBeLessOfABitch.CheckingTrapsHearVoice".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0)
            {
                OutputText("Scenes.Marblepurification.PureMarbleDecidesToBeLessOfABitch.AndThatOfChild".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
            {
                OutputText("Scenes.Marblepurification.PureMarbleDecidesToBeLessOfABitch.RenWord".Localized());
            }
            OutputText("Scenes.Marblepurification.PureMarbleDecidesToBeLessOfABitch.JustOutsideFoundOut".Localized());
            OutputText("Scenes.Marblepurification.PureMarbleDecidesToBeLessOfABitch.ExcitedlyRunsHugs".Localized());
            Player.CreateStatusEffect(VanillaStatusEffects.CampMarble, 0, 0, 0, 0);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleLeftOverCorruption, 0);
            CoC.Instance.Flags.Set(VanillaFlags.MarbleWarnedAboutCorruption, 0);
            CoC.Instance.Flags.Set(VanillaFlags.FollowerAtFarmMarble, 0);
            ReturnToCamp(1);
        }

        // Haxe ClaraCampAddition()   outputText 8 
        public void ClaraCampAddition()
        {

            OutputText("Scenes.Marblepurification.ClaraCampAddition.NearbyPortalWoodenCage".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.ClaraPurified) > 0)
            {
                var claraInteractions = new List<string> { "You catch Clara staring at you for a moment, then her gaze once again returns to some distant mountain top. She has not taken her purification well.", "As always, Clara sits within, staring off into the distance. She has not taken her purification well." };
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0)
                {
                    claraInteractions.Add("Clara stares wistfully into the distance, ignoring your kids attempting to get her attention right in front of her.");
                }
                OutputText(claraInteractions[Random.Shared.Next(claraInteractions.Count)]);
            }
            else
            {
                if (View.Time.Hours <= 7 || View.Time.Hours >= 20)
                {
                    OutputText("Scenes.Marblepurification.ClaraCampAddition.LayingSprawledSnoring".Localized());
                }
                else
                {
                    var temp = Random.Shared.Next(CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0 ? 5 : 4);
                    if (temp == 0)
                    {
                        OutputText("Scenes.Marblepurification.ClaraCampAddition.FidgetingClothingRepairs".Localized());
                    }
                    if (temp == 1)
                    {
                        OutputText("Scenes.Marblepurification.ClaraCampAddition.HecklingThrowingInsults".Localized());
                    }
                    if (temp == 2)
                    {
                        OutputText("Scenes.Marblepurification.ClaraCampAddition.TryingToTemptYou".Localized());
                    }
                    if (temp == 3)
                    {
                        OutputText("Scenes.Marblepurification.ClaraCampAddition.WhiningAboutConfinement".Localized());
                    }
                    if (temp == 4)
                    {
                        OutputText("Scenes.Marblepurification.ClaraCampAddition.CursingAtYourChildren".Localized());
                    }
                }
            }
        }


        // ── 跨场景 helper（原版 NPCAwareContent 的 convenience 方法）──
        // 原版 NPCAwareContent.hx:287-289 followerRathazul() → Rathazul.followerRathazul()
        private static bool RathazulFollower() => SceneInstance<RathazulScene>.Get().FollowerRathazul();

        // 原版 Camp.hx:1099-1101 companionsCount() = followersCount + slavesCount + loversCount
        private static int CompanionCount() =>
            Camp.FollowersCount() + Camp.SlavesCount() + Camp.LoversCount();

        // 原版 AmilyScene.hx:253-270 amilyFollower()/amilyCorrupt()/amilyFollowerPure()：
        // AmilyScene 本体未移植（原版 AmilyScene.hx:253-270），公式与 Camp.cs:553-573 相同（AMILY_FOLLOWER / FOLLOWER_AT_FARM_AMILY /
        // AMILY_VISITING_URTA 三个旗标）。
        private static bool AmilyFollower()
        {
            if (CoC.Instance.Flags.Get(VanillaFlags.AmilyFollower) == 0)
                return false;

            if (CoC.Instance.Flags.Get(VanillaFlags.FollowerAtFarmAmily) != 0)
                return false;

            var visitingUrta = CoC.Instance.Flags.Get(VanillaFlags.AmilyVisitingUrta);

            return visitingUrta != 1 && visitingUrta != 2;
        }

        private static bool AmilyFollowerPure() =>
            AmilyFollower() && CoC.Instance.Flags.Get(VanillaFlags.AmilyFollower) != 2;

        // 原版 KihaFollowerScene.hx:190-198 followerKiha()
        private static bool KihaFollower() =>
            CoC.Instance.Flags.Get(VanillaFlags.KihaCorruptionBitch) != 1 &&
            CoC.Instance.Flags.Get(VanillaFlags.KihaFollower) > 0;

        // 原版 IsabellaFollowerScene.hx:109-111 isabellaFollower()
        private static bool IsabellaFollower() =>
            CoC.Instance.Flags.Get(VanillaFlags.IsabellaFollowerAccepted) == 1 &&
            CoC.Instance.Flags.Get(VanillaFlags.IsabellaCampDisabled) == 0;

        // 原版 IsabellaFollowerScene.hx:117-125 isabellaAccent()
        private static bool IsabellaAccent() =>
            CoC.Instance.Flags.Get(VanillaFlags.IsabellaAccentForcedOn) > 0 ||
            CoC.Instance.Flags.Get(VanillaFlags.IsabellaAccentTrainingPercent) >= 100;

        // 原版 SophieBimbo.hx:112-114 bimboSophie()
        private static bool BimboSophie() =>
            CoC.Instance.Flags.Get(VanillaFlags.SophieBimbo) > 0 &&
            CoC.Instance.Flags.Get(VanillaFlags.SophieDisabledForever) == 0 &&
            CoC.Instance.Flags.Get(VanillaFlags.SophieDebimboed) == 0;
    }
}
