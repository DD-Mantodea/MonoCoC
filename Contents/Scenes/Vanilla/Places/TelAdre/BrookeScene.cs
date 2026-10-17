using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<BrookeScene>("coc:brooke")]
    public class BrookeScene : Scene
    {

        public override void Entry()
        {
            BrookeThirdEncounter();
        }

        public int BrookeAffection(int arg = 0)
        {

            CoC.Instance.Flags.Set(GameFlagIds.BrookeAffection, CoC.Instance.Flags.Get(GameFlagIds.BrookeAffection) + arg);

            if (CoC.Instance.Flags.Get(GameFlagIds.BrookeAffection) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.BrookeAffection, 100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BrookeAffection) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.BrookeAffection, 0);

            if ((Player.IsTaur() || CoC.Instance.Flags.Get(GameFlagIds.BrookeShoweredWith) == 0) && CoC.Instance.Flags.Get(GameFlagIds.BrookeAffection) > 20)
                CoC.Instance.Flags.Set(GameFlagIds.BrookeAffection, 20);

            return CoC.Instance.Flags.Get(GameFlagIds.BrookeAffection);
        }

        public float BrookeCapacity()
        {

            return 60;
        }

        public void MeetBrookeFirstTime()
        {
            ClearOutput();

            ShowImage("brooke-first-meeting");

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.HeadDownHallways".Localized());

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.FamiliarLayoutShowers".Localized());

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.StallOccupiedRunning".Localized((noFur ? "dog-eared person" : "canine morph"), (noFur ? "elf" : " fur")));

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.CurvesHipsSwell".Localized((noFur ? "The coloration of her tail and ears resemble one of the Shepherd breeds you had back at Ingnam, a mix of tan and black" : "Her coloration makes her look like one of the Shepherd breeds you had back at Ingnam: mostly tan and black, with a thin streak of white going straight down her front."), (noFur ? "" : " -- or is it more fur? --"), (noFur ? "" : " and the fur on her muzzle is totally black"), (noFur ? "" : "although it's difficult to gauge underneath her heavy, wet fur, ")));

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.StareFarLonger".Localized());

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.BlinkProcessingAskedQuestion".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Brooke.MeetBrookeFirstTime.CoilTailNervouslyUnder".Localized());
            else
                OutputText("Scenes.Brooke.MeetBrookeFirstTime.ShuffleYourFeet".Localized());

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.JustAboutToShower".Localized());

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.TurnToFaceMention".Localized());

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.EyesUpDown".Localized());

            if (Player.Tone <= 20)
                OutputText("Scenes.Brooke.MeetBrookeFirstTime.TwistsMouthStraightFace".Localized());
            else
                if (Player.Tone <= 50)
                    OutputText("Scenes.Brooke.MeetBrookeFirstTime.LooksBitNoddingApprovingly".Localized());
                else
                    if (Player.Tone <= 80)
                        OutputText("Scenes.Brooke.MeetBrookeFirstTime.SmilesToHerself".Localized());
                    else
                        OutputText("Scenes.Brooke.MeetBrookeFirstTime.EyesRoving".Localized());

            OutputText("Scenes.Brooke.MeetBrookeFirstTime.WatchWalkDownHallway".Localized());

            BrookeAffection(5);

            CoC.Instance.Flags.Set(GameFlagIds.BrookeMetToday, 1);

            CoC.Instance.Flags.Set(GameFlagIds.BrookeMet, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void RepeatChooseShower()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) >= 60 && SceneInstance<PabloScene>.Get().PabloIntro(false) && Random.Shared.Next(2) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.PabloSecretLearned) == 0)
                {
                    SceneInstance<PabloScene>.Get().PabloSecret();

                    return ;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) >= 100)
                    {
                        SceneInstance<PabloScene>.Get().PabloShowerSexIntro();

                        return ;
                    }

                if (CoC.Instance.Flags.Get(GameFlagIds.PabloFreakedOutOverWorms) > 0 && !Player.HasStatusEffect(StatusEffectIds.Infested))
                {
                    SceneInstance<PabloScene>.Get().PabloComesBackAfterWormCure();

                    return ;
                }
            }

            if (View.Time.Hours < 16 || View.Time.Hours > 18 || Player.Tone < 30)
            {
                if (BrookeAffection() <= 19)
                {
                    OutputText("Scenes.Brooke.RepeatChooseShower.ShowersSweetAche".Localized());

                    OutputText("Scenes.Brooke.RepeatChooseShower.ShowerUneventfullyCleanWhistle".Localized());

                    OutputText("Scenes.Brooke.RepeatChooseShower.BumpIntoHerLater".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BrookeMediumScene) == 0)
                    {
                        OutputText("Scenes.Brooke.RepeatChooseShower.ShowersSweetAcheAgain".Localized());

                        OutputText("Scenes.Brooke.RepeatChooseShower.CleanWhistleAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Brooke.RepeatChooseShower.ShowersThird".Localized());

                        OutputText("Scenes.Brooke.RepeatChooseShower.ShowerUneventfullyRogue".Localized());
                    }

                DoNext(() => ReturnToCamp(1));

                Player.ChangeFatigue(-5);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BrookeGrumpsAboutTa) == 0 && Player.Tone >= 30)
                    BrookeThirdEncounter();
                else
                    if (BrookeAffection() <= 19)
                        LowAffectionBrookeMeeting();
                    else
                        if (BrookeAffection() <= 39 && CoC.Instance.Flags.Get(GameFlagIds.BrookeShoweredWith) == 0)
                            MediumLowBrookeAffectionOneTime();
                        else
                            if (BrookeAffection() >= 40 && CoC.Instance.Flags.Get(GameFlagIds.BrookeMediumScene) > 0)
                                MediumBrookeAffectionMeetingAfterSex();
                            else
                                if (BrookeAffection() >= 40 && CoC.Instance.Flags.Get(GameFlagIds.BrookeMediumScene) == 0 && Player.Tone >= 50 && (Player.HasVagina() || Player.HasCockThatFits(BrookeCapacity())))
                                    MediumAffectionOneTimeEvent();
                                else
                                    if (BrookeAffection() != 0)
                                        MediumLowBrookeAffection();
        }

        public void BrookeThirdEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.BrookeThirdEncounter.LockersStoringArmor".Localized());

            OutputText("Scenes.Brooke.BrookeThirdEncounter.HearingStallGoing".Localized((noFur ? "her hair" : "at her fur")));

            OutputText("Scenes.Brooke.BrookeThirdEncounter.CasuallyRubbingWater".Localized((noFur ? "through her hair" : "underneath her fur")));

            OutputText("Scenes.Brooke.BrookeThirdEncounter.AskAboutHerself".Localized());

            OutputText("Scenes.Brooke.BrookeThirdEncounter.LittleOleMe".Localized());

            OutputText("Scenes.Brooke.BrookeThirdEncounter.TwoAndTwoWatch".Localized());

            OutputText("Scenes.Brooke.BrookeThirdEncounter.RespondsScrubHarderWas".Localized());

            OutputText("Scenes.Brooke.BrookeThirdEncounter.ShowerColdUneasy".Localized());

            BrookeAffection(5);

            CoC.Instance.Flags.Set(GameFlagIds.BrookeGrumpsAboutTa, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void LowAffectionBrookeMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.LowAffectionBrookeMeeting.ShowersSweetAche".Localized());

            OutputText("Scenes.Brooke.LowAffectionBrookeMeeting.ShowerSmoothlyTalkingIdly".Localized());

            OutputText("Scenes.Brooke.LowAffectionBrookeMeeting.ShowerUneventfullyCleanWhistle".Localized());

            DoNext(() => ReturnToCamp(1));

            BrookeAffection(5);
        }

        public void MediumLowBrookeAffectionOneTime()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.MediumLowBrookeAffectionOneTime.GoodWorkoutFineSheen".Localized());

            OutputText("Scenes.Brooke.MediumLowBrookeAffectionOneTime.SureEnoughLookingMelancholy".Localized());

            if (Player.IsTaur())
            {
                OutputText("Scenes.Brooke.MediumLowBrookeAffectionOneTime.EverythingAllRight".Localized());

                OutputText("Scenes.Brooke.MediumLowBrookeAffectionOneTime.ProceedsNormallyCant".Localized());

                OutputText("Scenes.Brooke.MediumLowBrookeAffectionOneTime.SomethingPersonal".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Brooke.MediumLowBrookeAffectionOneTime.EverythingAllRightAgain".Localized());

                OutputText("Scenes.Brooke.MediumLowBrookeAffectionOneTime.NotSoBadWashing".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Faerie.GladesScene.ButtonHelp".Localized(), HelpBrookeOut);

                AddButton(1, "Common.Button.DontHelp".Localized(), DontHelpBrookeShower);
            }
        }

        public void DontHelpBrookeShower()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.DontHelpBrookeShower.DontKnowEachOther".Localized((noFur ? "her body" : "hard at her fur")));

            OutputText("Scenes.Brooke.DontHelpBrookeShower.ShowerUneventfullyCleanWhistle".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BrookeAffection, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void HelpBrookeOut()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.HelpBrookeOut.HelpNotBigDeal".Localized((noFur ? "soft skin" : "moist fur")));

            OutputText("Scenes.Brooke.HelpBrookeOut.GoSlowlyFirstHands".Localized((noFur ? "" : " with Brooke's fur")));

            if (Player.HasCock())
                OutputText("Scenes.Brooke.HelpBrookeOut.DifficultKeepCool".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.SentencePeriod".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.SilentGetToWork".Localized((noFur ? "" : " getting your fingers underneath her fur. It's not very long, but it's not very short either; it takes some work getting the soap in underneath her fur, but not nearly as hard as she frequently made it out to be")));

            OutputText("Scenes.Brooke.HelpBrookeOut.FirstStartConversation".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.GentlySpeaksFeelsGood".Localized((noFur ? "eared" : "morph")));

            OutputText("Scenes.Brooke.HelpBrookeOut.DaylightCivvyClothes".Localized((noFur ? "washing her back" : "working the soap into her fur")));

            OutputText("Scenes.Brooke.HelpBrookeOut.LastLongLuckyHit".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.BackStillToYou".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.WoundUpGettingAway".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.WashingBackWhileAgo".Localized());
            // 原版 Brooke.hx:242：(urtaDisabled ? "the captain" : "Captain Urta")
            OutputText("Scenes.Brooke.HelpBrookeOut.LaterStretchesArms".Localized(GameSettings.UrtaDisabled ? "the captain" : "Captain Urta"));

            OutputText("Scenes.Brooke.HelpBrookeOut.SpinsBothAround".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.GrabbingTowel".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Brooke.HelpBrookeOut.StirringEachCock".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Brooke.HelpBrookeOut.FeelingFireStokeVagina".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.HelpBrookeOut.LoinsBeginningStir".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.ShepherdGirlAss".Localized());

            OutputText("Scenes.Brooke.HelpBrookeOut.LongGoneWaterCooling".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(15 + Player.Libido / 20)));

            CoC.Instance.Flags.Set(GameFlagIds.BrookeShoweredWith, 1);

            BrookeAffection(4);

            DoNext(() => ReturnToCamp(1));
        }

        public void MediumLowBrookeAffection()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.MediumLowBrookeAffection.WorkoutHeadBackShop".Localized((noFur ? "her body" : "at her fur")));

            OutputText("Scenes.Brooke.MediumLowBrookeAffection.StepIntoStallLathering".Localized());

            OutputText("Scenes.Brooke.MediumLowBrookeAffection.WhatTalkAbout".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.WorkingOut".Localized(), TalkWithBrookeAboutWorkingOut);

            AddButton(1, "Common.Button.GymMembers".Localized(), TalkToBrookeAboutGymFolks);

            AddButton(2, "Common.Button.HerBod".Localized(), BrookesRockinBod);

            if (BrookeAffection() >= 40 && BrookeAffection() <= 70 && CoC.Instance.Flags.Get(GameFlagIds.BrookeMediumScene) > 0)
                AddButton(3, "Common.Button.Grope".Localized(), GropeDatBrooke);
        }

        public void MediumBrookeAffectionMeetingAfterSex()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.MediumBrookeAffectionMeetingAfterSex.WorkoutHeadBackShops".Localized((noFur ? "her body" : "at her fur")));

            OutputText("Scenes.Brooke.MediumBrookeAffectionMeetingAfterSex.StepIntoHerStall".Localized());

            OutputText("Scenes.Brooke.MediumBrookeAffectionMeetingAfterSex.RemainsQuietMassageShoulders".Localized());

            OutputText("Scenes.Brooke.MediumBrookeAffectionMeetingAfterSex.WhatTalkAbout".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.WorkingOut".Localized(), TalkWithBrookeAboutWorkingOut);

            AddButton(1, "Common.Button.GymMembers".Localized(), TalkToBrookeAboutGymFolks);

            AddButton(2, "Common.Button.HerBod".Localized(), BrookesRockinBod);

            if (BrookeAffection() >= 40 && CoC.Instance.Flags.Get(GameFlagIds.BrookeMediumScene) > 0)
                AddButton(3, "Common.Button.Grope".Localized(), GropeDatBrooke);
        }

        public void TalkWithBrookeAboutWorkingOut()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.ManagesWorkOut".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.SaysJoviallyThanksCompliment".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.FaceShowerheadFirst".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.ListingWaysRegime".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.DoesAnybodyNeedReason".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.OneMoreQuestion".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.WaterRunsColdClean".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.AssureFineBothClean".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.EachCockWord".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.YourVaginaWord".Localized());
                else
                    OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.EachCockWordAgain".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.SentencePeriod".Localized());

            OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.GlancingBackHotStuff".Localized());

            if (BrookeAffection() <= 39)
                OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.NakedGirlUnusual".Localized());
            else
                OutputText("Scenes.Brooke.TalkWithBrookeAboutWorkingOut.KnowsButtonsWeaknesses".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            if (Player.Intelligence < 50)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f));

            BrookeAffection(4);

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkToBrookeAboutGymFolks()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.OtherGymMembers".Localized());

            OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.PersonalityWiseType".Localized());

            OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.OwnsPlaceAllRight".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) > 0)
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.CottonHorseMorphCute".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BrookeAndHeckel3some) > 0 && CoC.Instance.Flags.Get(GameFlagIds.BrookeAndHeckel3some) < 4)
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.HeckelStillCompetitiveWarmups".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BrookeAndHeckel3some) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.BrookeAndHeckel3some) < 7)
                    OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.TakesGettingUsedTo".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BrookeAndHeckel3some) >= 7)
                        OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.ActuallyPrettySweet".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.MetHeckel) > 0)
                            OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.WayTooCompetitive".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetIfris) > 0)
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.IfrisCreeperHangsAround".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.JasunMet) > 0)
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.JasunHardAsRock".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeMet) > 0)
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.BunnyGirlNiceGal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) > 0)
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.LottiePigletBody".Localized());

            OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.AllMembersOffTop".Localized());

            OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.WaterRunsColdTowel".Localized());

            if (BrookeAffection() <= 39)
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.NakedGirlLittleUnusual".Localized());
            else
                OutputText("Scenes.Brooke.TalkToBrookeAboutGymFolks.KnowsButtonsWeaknesses".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            BrookeAffection(4);

            DoNext(() => ReturnToCamp(1));
        }

        public void BrookesRockinBod()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.BrookesRockinBod.WantingBuffYoungAge".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.EyesDriftingKeepTalking".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.IfRememberDumbbellBirthday".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.SadlyDownAtHands".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.ThenPubertyHappenedDifferent".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.AdulthoodRealizedTooLate".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.AnswerGuysAndGals".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.EyesLockGesturesApproach".Localized());

            OutputText("Scenes.Brooke.BrookesRockinBod.LingersPullingAway".Localized());

            if (BrookeAffection() <= 39)
                OutputText("Scenes.Brooke.BrookesRockinBod.NakedGirlLittleUnusual".Localized());
            else
                OutputText("Scenes.Brooke.BrookesRockinBod.KnowsButtonsWeaknesses".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            BrookeAffection(15);

            DoNext(() => ReturnToCamp(1));
        }

        public void GropeDatBrooke()
        {
            ClearOutput();

            ShowImage("brooke-grope-her");

            OutputText("Scenes.Brooke.GropeDatBrooke.BodyKnowsFlaunt".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GropeDatBrooke.CockBetweenCheeks".Localized());

            OutputText("Scenes.Brooke.GropeDatBrooke.TensionResolveWavers".Localized());

            OutputText("Scenes.Brooke.GropeDatBrooke.ShouldersEasingWay".Localized());

            OutputText("Scenes.Brooke.GropeDatBrooke.FeelPlushnessLeansBack".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GropeDatBrooke.EachCockWord".Localized());
            else
            {
                OutputText("Scenes.Brooke.GropeDatBrooke.YourPelvisWord".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Brooke.GropeDatBrooke.WetTailThroughVulva".Localized());
            }

            OutputText("Scenes.Brooke.GropeDatBrooke.ReadyNeedLaps".Localized());

            OutputText("Scenes.Brooke.GropeDatBrooke.DeterminedlyAcceptChallenge".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            OutputText("Scenes.Brooke.GropeDatBrooke.WeaponOfChoice".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(BrookeCapacity()))
                AddButton(0, "Common.Button.Penetrate".Localized(), PenetrateBrooke);
            else
                AddButton(0, "Common.Button.Penetrate".Localized(), null);

            if (Player.HasCock())
                AddButton(1, "Common.Button.Anal".Localized(), BrookeAnal);
            else
                AddButton(1, "Common.Button.Anal".Localized(), null);

            if (Player.HasVagina())
                AddButton(2, "Common.Button.Tribadism".Localized(), Tribadism);
            else
                AddButton(2, "Common.Button.Tribadism".Localized(), null);

            AddButton(3, "Common.Button.GoDownOn".Localized(), GoDownOnBrooke);

            if (Player.HasCockThatFits(BrookeCapacity()))
                AddButton(4, "Common.Button.GetLaid".Localized(), GetLaidByBrooke);
            else
                AddButton(4, "Common.Button.GetLaid".Localized(), null);

            if (Player.CockTotal() >= 2)
                AddButton(5, "Common.Button.DoubleDick".Localized(), DoubleDicked);
            else
                AddButton(5, "Common.Button.DoubleDick".Localized(), null);
        }

        public void PenetrateBrooke()
        {
            ClearOutput();

            var x = Player.CockThatFits(BrookeCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = Player.CockThatFits2(BrookeCapacity());

            ShowImage("brooke-gym-male-penetratebrooke");

            OutputText("Scenes.Brooke.PenetrateBrooke.CleftTonedAss".Localized(Player.CockDescript(x), (noFur ? "they" : "the water in her fur makes them")));

            OutputText("Scenes.Brooke.PenetrateBrooke.PressingLipsThrough".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.RisesEngorgingLust".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.PullMouthReposition".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.LeansArmsOnStall".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.WrappingTailTorso".Localized());

            if (y >= 2)
                OutputText("Scenes.Brooke.PenetrateBrooke.UpDownCrackLamely".Localized(
                    Player.CockDescript(y)
                ));

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.PenetrateBrooke.VaginaPulsesJealously".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.MoansAppreciativelyLovingFeel".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Brooke.PenetrateBrooke.RequestsGrabBoobsAgain".Localized((noFur ? "" : "through her wet fur, ")));

            OutputText("Scenes.Brooke.PenetrateBrooke.LovingWayTreatBody".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Brooke.PenetrateBrooke.AcrossSackAnd".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.ThighsWetEnough".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.JerksIntoHands".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Brooke.PenetrateBrooke.RBallsWord".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.FewMorePumps".Localized());

            if (y >= 0)
                OutputText("Scenes.Brooke.PenetrateBrooke.LoadOnLowerBack".Localized(
                    Player.CockDescript(y)
                ));

            if (Player.CockTotal() > 1 && y >= 0)
                OutputText("Scenes.Brooke.PenetrateBrooke.JizzOverFloorWalls".Localized());

            if (Player.CockTotal() == 2 && y >= 0)
                OutputText("Scenes.Brooke.PenetrateBrooke.SentencePeriod".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.PenetrateBrooke.FemcumDownLegs".Localized());

            OutputText("Scenes.Brooke.PenetrateBrooke.StandingColdNozzle".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Brooke.PenetrateBrooke.MinutesKeepingWarm".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            BrookeAffection(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void BrookeAnal()
        {
            ClearOutput();

            var x = Player.CockThatFits(BrookeCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            if (Player.CockArea(x) >= 61)
            {
                OutputText("Scenes.Brooke.BrookeAnal.AskOpinionOnAnal".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                ClearButtons();

                AddButton(14, "Common.Button.Back".Localized(), GropeDatBrooke);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) == 0)
                OutputText("Scenes.Brooke.BrookeAnal.OpinionOnAnalAgain".Localized());
            else
                OutputText("Scenes.Brooke.BrookeAnal.LastTimeHesitant".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.TrappingCockAsscheeks".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Brooke.BrookeAnal.DraggingCockCrack".Localized(
                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) == 0)
                OutputText("Scenes.Brooke.BrookeAnal.WonderFirstTimeAnal".Localized());
            else
                OutputText("Scenes.Brooke.BrookeAnal.TightAsVice".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.RubsAgainstRosebud".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Brooke.BrookeAnal.EntertainsBetweenThighs".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.ClenchesMomentRelax".Localized());

            ShowImage("brooke-gym-male-analbrooke");

            OutputText("Scenes.Brooke.BrookeAnal.TakeItEasyRealign".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Brooke.BrookeAnal.StayStillHeatTakes".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Brooke.BrookeAnal.HalfwayWaterTight".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.RightEasyPace".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.GaspInvitationHarder".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.LittleHarderAlready".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Brooke.BrookeAnal.BallsWord".Localized());
            else
                OutputText("Scenes.Brooke.BrookeAnal.SheathWord".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.VoraciousAssClimbs".Localized());

            if (Player.Corruption >= 45)
                OutputText("Scenes.Brooke.BrookeAnal.SomeoneComeInvestigate".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.HarderThrustingAssBack".Localized());

            if (Player.CockTotal() >= 2)
                OutputText("Scenes.Brooke.BrookeAnal.DicksBatteringPainfully".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.ViciouslyHarderKeepGoing".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.AssRippleTightenSnapping".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Brooke.BrookeAnal.InYourBalls".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.CockStiffens".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.BrookeAnal.PussyClenchesAtNothing".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.CumIntoHerBlasting".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Brooke.BrookeAnal.SecondDickFlailsErupts".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Brooke.BrookeAnal.BlastsDifferentDirection".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Brooke.BrookeAnal.RigorousSexCollapse".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) == 0)
                OutputText("Scenes.Brooke.BrookeAnal.NeverKnewThrill".Localized());
            else
                OutputText("Scenes.Brooke.BrookeAnal.YourFaultBossyBitch".Localized());

            OutputText("Scenes.Brooke.BrookeAnal.LetsGoSpentFlaccid".Localized(
                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ChangeFatigue(15);

            CoC.Instance.Flags.Set(GameFlagIds.TimesInBrookesButt, CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) + 1);

            BrookeAffection(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void Tribadism()
        {
            ClearOutput();

            ShowImage("brooke-gym-female-tribadism");

            OutputText("Scenes.Brooke.Tribadism.FullBreastsWetHeavy".Localized((noFur ? "" : " -- trying to avoid getting any fur sticking to your tongue")));

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.Tribadism.WishingPutToUse".Localized());
            else
                OutputText("Scenes.Brooke.Tribadism.ToneOfAssWish".Localized());

            OutputText("Scenes.Brooke.Tribadism.DownStomachAbs".Localized());

            OutputText("Scenes.Brooke.Tribadism.ThrustSecondKnuckle".Localized());

            OutputText("Scenes.Brooke.Tribadism.TeaseIndexThrusts".Localized());

            OutputText("Scenes.Brooke.Tribadism.RemovingHandAlmostHurt".Localized());

            OutputText("Scenes.Brooke.Tribadism.PressingChestBrooke".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.Tribadism.BaseStrainingCock".Localized());

            OutputText("Scenes.Brooke.Tribadism.SentencePeriod".Localized());

            OutputText("Scenes.Brooke.Tribadism.SnapsHumpRightBack".Localized());

            if (Player.Tone >= 50)
                OutputText("Scenes.Brooke.Tribadism.GrippingPullingAsscheeks".Localized());

            OutputText("Scenes.Brooke.Tribadism.FuckAgainstHerEarnest".Localized(
                Player.Mf("woman", "'woman'")
            ));

            OutputText("Scenes.Brooke.Tribadism.SpurringHarderLift".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Brooke.Tribadism.UpToTitsGroping".Localized());

            if (!Player.HasCock() && Player.LactationQ() >= 200)
                OutputText("Scenes.Brooke.Tribadism.MilkWeakPressure".Localized());

            if (!Player.HasCock() && Player.HasFuckableNipples())
                OutputText("Scenes.Brooke.Tribadism.NipplesSnapsOut".Localized());

            OutputText("Scenes.Brooke.Tribadism.SentencePeriodAgain".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.Tribadism.OtherHandOtherCock".Localized());

            OutputText("Scenes.Brooke.Tribadism.WholeBodyHeaving".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.Tribadism.ExplodesAlongSister".Localized());
            else
                OutputText("Scenes.Brooke.Tribadism.DriveAgainstMauls".Localized());

            OutputText("Scenes.Brooke.Tribadism.RiseClimbClimaxes".Localized());

            OutputText("Scenes.Brooke.Tribadism.ThrustWindDown".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.Tribadism.EachCockGoesLimp".Localized());

            OutputText("Scenes.Brooke.Tribadism.ExcitementShuddersMiniature".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.Tribadism.StrokesWaningErection".Localized());

            OutputText("Scenes.Brooke.Tribadism.SentencePeriodThird".Localized());

            OutputText("Scenes.Brooke.Tribadism.WhatsLeftWaterClean".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            Player.ChangeFatigue(15);

            BrookeAffection(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void GoDownOnBrooke()
        {
            ClearOutput();

            ShowImage("brooke-gym-goDown");

            OutputText("Scenes.Brooke.GoDownOnBrooke.FondleBreastKneading".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.ConfusedTurnsHead".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.DownPelvisStroke".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.SighsMeltsIntoBody".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GoDownOnBrooke.BeginsFillDesire".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.BarelySlippingFinger".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.QuitItHandCrotch".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.DropKneesDive".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.PullTongueCollarbone".Localized((noFur ? "" : " (and her wet fur)")));

            OutputText("Scenes.Brooke.GoDownOnBrooke.KneesThudFlatTongue".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.PlantLipsVulva".Localized((noFur ? "gasps" : "yips")));

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GoDownOnBrooke.PullsForwardMagnetic".Localized());
            else
                OutputText("Scenes.Brooke.GoDownOnBrooke.SalivatingViableSpecimen".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.RocksBodyOwnRhythm".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.EnoughBreakDarts".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.DownLowTipTongue".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.DrillBackWiggling".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.FlopsOutGaspsLust".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.LongerDropsHandSkull".Localized((noFur ? "sighs" : "yips")));

            OutputText("Scenes.Brooke.GoDownOnBrooke.ClenchUnclenchChewsLip".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.EnoughBodyGoesSlack".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.AllTongueCouldAsk".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.WaterWentColdNeeded".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GoDownOnBrooke.HardlyGoneUnnoticed".Localized());
            else
                OutputText("Scenes.Brooke.GoDownOnBrooke.StandsOutPuffyNeedy".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.OffersReturnFavourDecline".Localized());

            OutputText("Scenes.Brooke.GoDownOnBrooke.DryingWithoutIncident".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            BrookeAffection(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void GetLaidByBrooke()
        {
            ClearOutput();

            var x = Player.CockThatFits(BrookeCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Brooke.GetLaidByBrooke.WrappingArmTorso".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.HipsIntoTonedAss".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GetLaidByBrooke.SpearingCockBetweenCheeks".Localized(Player.CockDescript(x), (noFur ? "" : "her soft fur and ")));
            else
                OutputText("Scenes.Brooke.GetLaidByBrooke.WiggleTailBetweenLegs".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.NeckKnowsLikes".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.HandsWorkMagic".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.LoveMakingFeelSpecial".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GetLaidByBrooke.ThrustSpearLuscious".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Brooke.GetLaidByBrooke.StokesEachSwish".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.GrinningBackActions".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.GoDownReadsMind".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GetLaidByBrooke.HorndogWord".Localized());
            else
                OutputText("Scenes.Brooke.GetLaidByBrooke.WhoreWord".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.TurningAroundReachesDown".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GetLaidByBrooke.EachCockHissingInwardly".Localized());
            else
                OutputText("Scenes.Brooke.GetLaidByBrooke.VaginaGrinningLowerLips".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.AsksInnocentlyFingersWorking".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.QuickFierceKiss".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GetLaidByBrooke.FullyErectAttention".Localized());
            else
                OutputText("Scenes.Brooke.GetLaidByBrooke.ResistFuckingHips".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.DroolingLipsLeansIn".Localized());

            if (Player.HasCock())
            {
                ShowImage("brooke-gym-male-getLaid");

                OutputText("Scenes.Brooke.GetLaidByBrooke.BreathesHotlyHesitating".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.KeepingSturdy".Localized());

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.SureWhereBegin".Localized(
                        Player.CockDescript(x)
                    ));

                if (Player.CockTotal() >= 4)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.TearEyesFromRest".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Brooke.GetLaidByBrooke.FullyAwareFeminine".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Brooke.GetLaidByBrooke.HandsOnHeadExpertly".Localized());

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.OverRestOfHer".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.SentencePeriod".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.PushesDeeperSwallowing".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() <= 2)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.LipsCanineLoverSlide".Localized());
                else
                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Brooke.GetLaidByBrooke.TriesDamnedestEvery".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.SentencePeriodAgain".Localized());

                if (Player.Gender != Gender.Hermaphrodite)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.ParagraphBreak".Localized());
                else
                {
                    OutputText("Scenes.Brooke.GetLaidByBrooke.PullsAwayCockCold".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Brooke.GetLaidByBrooke.BuryingMouthUnderneathSack".Localized());

                    OutputText("Scenes.Brooke.GetLaidByBrooke.TongueWorkVaginaGasp".Localized(Player.CockDescript(x), (noFur ? "" : " the fine fur on"), Player.CockDescript(x), Player.CockDescript(x)));
                }

                OutputText("Scenes.Brooke.GetLaidByBrooke.OutLoudPressingFarther".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockHead(x)
                ));

                OutputText("Scenes.Brooke.GetLaidByBrooke.PicksUpPace".Localized());

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.BlurUpAndDown".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Brooke.GetLaidByBrooke.PistoningThumbFrigs".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.NotEntirelyPositive".Localized());

                if (Player.CumQ() <= 349)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.SuckingMouthLicks".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    if (Player.CumQ() <= 999)
                        OutputText("Scenes.Brooke.GetLaidByBrooke.HungryMouthHungry".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        OutputText("Scenes.Brooke.GetLaidByBrooke.WorkedHardSeedCute".Localized(
                            Player.CockDescript(x)
                        ));

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.CockOrgasmSprays".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.RewardedDiligence".Localized());
            }
            else
            {
                ShowImage("brooke-gym-female-getLaid");

                OutputText("Scenes.Brooke.GetLaidByBrooke.WasteAnyTimeWork".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.FaceIntoCrotch".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.TongueWhimperPutsBack".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.BaskWarmWater".Localized());

                if (Player.HasFuckableNipples())
                    OutputText("Scenes.Brooke.GetLaidByBrooke.RemindsAlternativeOutlets".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.MuffledTongueVibrates".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.WiderBendsSpearing".Localized());

                if (Player.Corruption <= 49)
                    OutputText("Scenes.Brooke.GetLaidByBrooke.EmbarrassingTurnOff".Localized());
                else
                    OutputText("Scenes.Brooke.GetLaidByBrooke.AllForYourselfSharing".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.MouthfulQuimThrusts".Localized());

                OutputText("Scenes.Brooke.GetLaidByBrooke.HipsRockMouthStimulation".Localized());
            }

            OutputText("Scenes.Brooke.GetLaidByBrooke.ExplosiveOrgasmEyesCrossed".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Brooke.GetLaidByBrooke.LettingExcessWash".Localized((noFur ? "" : " fur")));

            OutputText("Scenes.Brooke.GetLaidByBrooke.MomentsPassUndecipherableBlur".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.KissBackConvince".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.GetLaidByBrooke.SnowballingHasntSwallowed".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.CradlesStrongArms".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.WaterFairlyCold".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.KeepStealingGlances".Localized());

            OutputText("Scenes.Brooke.GetLaidByBrooke.WhistleArmorFarewell".Localized());

            BrookeAffection(10);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void DoubleDicked()
        {
            ClearOutput();

            var x = Player.CockThatFits(BrookeCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = Player.CockThatFits2(BrookeCapacity());

            if (y < 0 || Player.CockThatFits(x) >= BrookeCapacity())
            {
                OutputText("Scenes.Brooke.DoubleDicked.AgainstWarmWetFeel".Localized((noFur ? "skin" : "fur")));

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), GropeDatBrooke);

                return ;
            }

            ShowImage("brooke-gym-male-doubledicked");

            OutputText("Scenes.Brooke.DoubleDicked.SayingMuchRubAss".Localized((noFur ? "her wet, firm" : "the wet, matted fur of her"), Player.CockDescript(x), Player.CockDescript(y)));

            OutputText("Scenes.Brooke.DoubleDicked.LettingJerkOff".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y)
            ));

            if (Player.IsNaga())
                OutputText("Scenes.Brooke.DoubleDicked.ReptilesBetterOrWorse".Localized());
            else
                OutputText("Scenes.Brooke.DoubleDicked.ExoticSetSweetThing".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.CasuallyMasturbate".Localized());

            if (Player.IsNaga())
                OutputText("Scenes.Brooke.DoubleDicked.DickTwiceTheFun".Localized());
            else
                OutputText("Scenes.Brooke.DoubleDicked.HalfSnakeSportingMore".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.BrushingRingAnus".Localized(
                Player.CockDescript(y),

                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) > 0)
                OutputText("Scenes.Brooke.DoubleDicked.TurnButtfuckingFreak".Localized());
            else
                OutputText("Scenes.Brooke.DoubleDicked.JustBitHeatAsshole".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.DualPenisesRubbingHoles".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y)
            ));

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Brooke.DoubleDicked.OtherDicksRiseJealous".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.PalmsEveryMuscle".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.TogetherArousingPlunge".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y)
            ));

            OutputText("Scenes.Brooke.DoubleDicked.ReadyDontRush".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.SlidesTightlyPressing".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y),

                Player.CockDescript(y)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) == 0)
                OutputText("Scenes.Brooke.DoubleDicked.CertainlyNotWorkoutAccustomed".Localized());
            else
                OutputText("Scenes.Brooke.DoubleDicked.LovesGettingFucked".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.AssTakesAnotherInch".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.FeelGlutesRigid".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Brooke.DoubleDicked.AbdomenAbsFlexes".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Brooke.DoubleDicked.LamentablyStandsLeaking".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.DoubleDicked.ClenchesInTime".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.ContortsConcentrationFidgets".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.LeastCanDoReturn".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y)
            ));

            OutputText("Scenes.Brooke.DoubleDicked.KeepsSteadyAndSlow".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) == 0)
                OutputText("Scenes.Brooke.DoubleDicked.SlowlyStartsEvenOut".Localized());
            else
                OutputText("Scenes.Brooke.DoubleDicked.BothCocksAlternating".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.JerksEveryThrust".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Brooke.DoubleDicked.DownToYourBalls".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.PracticallyFeelTwoDicks".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.GruntsTurnPleasuredMoans".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.WallToButtGrabbing".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Brooke.DoubleDicked.PainfullyErectSquashes".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.MoreForcefullyNotFaster".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.TurnsRaggedShudders".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.PressureHarderGoing".Localized());

            if (Player.CumQ() <= 349)
                OutputText("Scenes.Brooke.DoubleDicked.BlastsIntoWomb".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(y)
                ));
            else
                if (Player.CumQ() <= 999)
                    OutputText("Scenes.Brooke.DoubleDicked.PotentSpunkThirstyCunt".Localized());
                else
                    OutputText("Scenes.Brooke.DoubleDicked.PowerfullyOvertakes".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(y)
                    ));

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Brooke.DoubleDicked.SpraysFromBetween".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.DoubleDicked.PulsesSprayingDownLegs".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.StayLikeThatAfterglow".Localized());

            if (Player.CumQ() >= 10000)
                OutputText("Scenes.Brooke.DoubleDicked.ProbablyHaveTrouble".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.PullsAwayLookEye".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.StayLikeThatDeflated".Localized((noFur ? "" : " under her fur")));

            OutputText("Scenes.Brooke.DoubleDicked.WhistleBothLockers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBrookesButt) == 0)
                OutputText("Scenes.Brooke.DoubleDicked.BreaksSilenceHotDamn".Localized());
            else
                OutputText("Scenes.Brooke.DoubleDicked.NoWordsTowel".Localized());

            OutputText("Scenes.Brooke.DoubleDicked.DryingWithoutIncident".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));

            BrookeAffection(10);
        }

        public void MediumAffectionOneTimeEvent()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.IntoShowersLockers".Localized((noFur ? "" : " at her fur")));

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.FirstStepEarsPerk".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.InviteWashBack".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.TowelPlacingWallStep".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.WhereGoingNotExciting".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.RightAwayShoulders".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.CockWakingProximity".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.UpperBackFollowSuit".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.TravelLowerBody".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.RisingCockPretending".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.TeaseFirstTime".Localized());

            OutputText("Scenes.Brooke.MediumAffectionOneTimeEvent.DownButtSqueeze".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BrookeSpecialMediumSceneContinued);
        }

        public void BrookeSpecialMediumSceneContinued()
        {
            ClearOutput();

            var x = Player.CockThatFits(BrookeCapacity());

            if (x < 0 && Player.HasCock())
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.BrookeMediumScene, 1);

            if (x >= 0 && !Player.HasVagina())
            {
                ShowImage("brooke-gym-male-specialscene");

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.GripNotStrongShoulders".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BreaksKissPullingBack".Localized(
                    Player.CockDescript(x)
                ));

                var y = Player.CockThatFits2(BrookeCapacity());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BrushesBetweenAbs".Localized((noFur ? "" : " through the wet fur on her stomach")));

                if (y >= 0 && Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.FindsDrivingHigher".Localized((noFur ? "" : "through the wet fur on her stomach and ")));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.HeatNeedGrind".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.NeedsItBadly".Localized(
                    Player.CockHead(x)
                ));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.WetHeatContractions".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RestingAgainstCrotch".Localized());
                else
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.CrotchAgainstHers".Localized());

                if (!noFur)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.FurLightlyTinglingSkin".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SentencePeriod".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.MomentEmbracing".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.WishingSayThat".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BitesLipComposure".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SlappingPussyThrust".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SwingsFreelyCold".Localized((noFur ? "skin" : "fur")));

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.DicksSwingFreelyCold".Localized((noFur ? "skin" : "fur")));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.MoansExcitedGrunts".Localized());

                if (Player.Body.Get<HairPart>().Length >= 1)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RunningFingersThroughHair".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.GiveHardDick".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.ProbablyHeardVisitors".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RemindsComingQuickly".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BringingReleaseMuscles".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BallsBeginToChurn".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RacesCockTightens".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CumQ() <= 250)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.PaintWallsSeed".Localized());
                else
                    if (Player.CumQ() <= 750)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BlastHotTunnel".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        if (Player.CumQ() <= 1500)
                            OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.EverPunchingSeed".Localized(
                                Player.CockDescript(x)
                            ));
                        else
                            if (Player.CumQ() <= 4000)
                                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RushesBlastingWalls".Localized());
                            else
                                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BurstFillingCapacity".Localized(
                                    Player.CockDescript(x)
                                ));

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.TensesAlongWithFirst".Localized());

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SpurtsAlongFirst".Localized((noFur ? "" : " and into her fur")));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.PantPhysicallySpent".Localized((noFur ? "" : " under the wet fur")));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SayHolyFuck".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.ArmsTireRagged".Localized());

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.LetsGoNeverFucked".Localized());

                if (Player.CockTotal() >= 2)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.LooksDownNoticingMess".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.CourtesyShuttingWater".Localized((noFur ? "my hair" : "out fur")));

                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.JustBeforeWrap".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 0 - 2));

                Player.ChangeFatigue(10);
            }
            else
                if (Player.HasVagina() && x < 0)
                {
                    ShowImage("brooke-gym-female-specialscene");

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.KeepUpWantonDesire".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.StrokesUpDownHips".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.PullsMouthAway".Localized());

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BeginsLickingSlows".Localized());

                    if (Player.LactationQ() >= 200)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.MilkDripNurses".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.EverythingBrookeHump".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.NippleStandingUpright".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SuddenDemandNotArgue".Localized((noFur ? "" : " fur"), (noFur ? "Her crotch is shaved bare" : "The fur around her crotch is thin")));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.DigIntoFlesh".Localized((noFur ? "" : " All you feel is wet fur and all you smell is wet dog, but neither of these things deter you from your duty.")));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RunningUpperLip".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.KeepAtItMoments".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.GraspingPullingButt".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.OnslaughtBreakKiss".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.ThrashTooExcited".Localized());

                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.EyesCrossMindFuzzes".Localized((noFur ? "squishiness of Brooke's breasts to the warmth of" : "heavy drag of Brooke's breast fur to the fine, ticklish fur on")));
                    else
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.QuakeOrgasmOvertake".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SpentCrashesOnto".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.CalmMomentMakeOut".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.FuckedHarderThan".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.CloseAlmostPredatorily".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.OnceMoreDeepBreath".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.OfferDoubtPromise".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

                    Player.ChangeFatigue(10);
                }
                else
                {
                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.NoTimeFeelingGroping".Localized((noFur ? "skin" : "short fur"), (noFur ? "taut belly" : " fine belly fur")));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.OwnHandsReturning".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.EveryExcitedMovement".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.TimeWithFatAss".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.PinnedAgainstStall".Localized());

                    if (Player.HasFuckableNipples())
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.TongueCaressesNipple".Localized());

                    if (Player.LactationQ() >= 200)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.MilkSloshChest".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.DropsHandsWrapping".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.GrindHipsOnHands".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.ContainHerselfTongue".Localized(
                            Player.CockDescript(x)
                        ));

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.AlmostEcstaticSeeing".Localized(
                            Player.CockDescript(x),

                            Player.CockDescript(x)
                        ));

                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.WideEyedLostEntranced".Localized(
                            Player.CockDescript(x)
                        ));

                    if (Player.Balls > 0)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.HeadDipsSuckling".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.WhimperApproachingEdge".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.TakeInvitationGrip".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RecomposeHotEnough".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RequestNotForgotten".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RaggedRisingPitch".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.EveryOnceFuckYes".Localized());

                    if (Player.Balls > 0)
                        if (!Player.HasStatusEffect(StatusEffectIds.Uniball))
                            OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.EveryPumpSlapping".Localized((Configuration.OptionStore.Instance.Get("furry", 0) <= 0 ? "" : " You feel no pain, and in fact the smoothness of her fur around her genitals tickles the cum out of your [balls] all the faster.")));
                        else
                            OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.TightSmoothSlaps".Localized((Configuration.OptionStore.Instance.Get("furry", 0) <= 0 ? "" : " You feel no pain, and in fact the smoothness of her fur around her genitals tickles the cum out of your [balls] all the faster.")));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.PussyEverythingBurns".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.SwingsColdAlone".Localized(
                            Player.CockDescript(x)
                        ));

                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.MuchAsBothWant".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.CeilingConvolvesContain".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CumQ() <= 250)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.ShepherdGirlPumping".Localized());

                    if (Player.CumQ() <= 500)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.HungryCoochAbandon".Localized(
                            Player.CockDescript(x)
                        ));
                    else
                        if (Player.CumQ() <= 1000)
                            OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.FloodsIntoHeartbeat".Localized());
                        else
                            if (Player.CumQ() <= 3000)
                                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.RippingThroughCock".Localized(
                                    Player.CockDescript(x)
                                ));
                            else
                                OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.InitialBlastWomb".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.EruptsInTimeBrother".Localized((noFur ? "her skin" : " into her fine fur, blending in with the white strip of fur running down her torso")));

                    if (Player.CockTotal() >= 3)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.NotInsideSprays".Localized());

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.LonelyPussyErupts".Localized());
                    else
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.LeftOfEnergyNeglected".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.BuckAgainstWindDown".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.HungTitsInSight".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.LegsSlightlyHandsUp".Localized());

                    if (Player.CockTotal() >= 2)
                        OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.WanderUnfocusedDazed".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.LeaningOppositeWall".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.LaughApproachingSlapThigh".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.APromiseLaughs".Localized());

                    OutputText("Scenes.Brooke.BrookeSpecialMediumSceneContinued.ColdWaterOff".Localized(
                        Player.CockDescript(x)
                    ));

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

                    Player.ChangeFatigue(10);
                }

            BrookeAffection(10);

            DoNext(() => ReturnToCamp(1));
        }

        public void SpecialHeckelAndBrookeEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.SpecialHeckelAndBrookeEncounter.GymLookingHeckel".Localized());

            OutputText("Scenes.Brooke.SpecialHeckelAndBrookeEncounter.CornersCommotion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Butterfly".Localized(), ButterflyMachinesGooooo);

            AddButton(1, "Common.Button.LegPress".Localized(), LegPressInsteadOfThreesome);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveHeckelEncounter);
        }

        public void LeaveHeckelEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.LeaveHeckelEncounter.CameLookingHeckel".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void ButterflyMachinesGooooo()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BrookeAndHeckel3some, CoC.Instance.Flags.Get(GameFlagIds.BrookeAndHeckel3some) + 1);

            OutputText("Scenes.Brooke.ButterflyMachinesGooooo.AroundButterflyMachines".Localized((noFur ? "Animal-eared people" : "Anthromorphic animals")));

            OutputText("Scenes.Brooke.ButterflyMachinesGooooo.EachMachineHeckel".Localized());

            OutputText("Scenes.Brooke.ButterflyMachinesGooooo.ConfidentGrinExhaling".Localized());

            OutputText("Scenes.Brooke.ButterflyMachinesGooooo.ArmsNeverStopping".Localized());

            OutputText("Scenes.Brooke.ButterflyMachinesGooooo.HighFivingTopDog".Localized());

            OutputText("Scenes.Brooke.ButterflyMachinesGooooo.DragsBrookeBehindCrowd".Localized());

            OutputText("Scenes.Brooke.ButterflyMachinesGooooo.HandGrabCollarArmor".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BrookeAndHeckelStuffPartTwo);
        }

        public void BrookeAndHeckelStuffPartTwo()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartTwo.KicksOpenDoor".Localized());

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartTwo.DownFlatOnBack".Localized((noFur ? "" : " only straight enough to not drag through Brooke's fur,")));

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartTwo.HurryItUpWatching".Localized());

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartTwo.DoYouComply".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), SubmitHeckelXBrookeThreesome);

            AddButton(1, "Common.Button.No".Localized(), NopeOutofBroke);
        }

        public void NopeOutofBroke()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.NopeOutofBroke.NotSecondFiddle".Localized());

            OutputText("Scenes.Brooke.NopeOutofBroke.MuscledHermShrugs".Localized());

            OutputText("Scenes.Brooke.NopeOutofBroke.ExactlyPleasedGetInto".Localized());

            OutputText("Scenes.Brooke.NopeOutofBroke.ShesSureSaveYourself".Localized());

            OutputText("Scenes.Brooke.NopeOutofBroke.ChasedOutLewdSlurps".Localized());

            BrookeAffection(1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SubmitHeckelXBrookeThreesome()
        {
            ClearOutput();

            ShowImage("brooke-gym-heckelbrooke3some");

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.HastilyRemovingArmor".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.TenInchDogDick".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.ArmsLengthGrabsWrist".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SwiftnessFerocityTakeAback".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.BitLongerStartUp".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.HyenaFaceSixPack".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SimplySwitchBoth".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SmellMuskNewVantage".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.JustAsBusy".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.StopBothComply".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.GrippingHemShorts".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.CommandsAllFours".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.LightningReflexes".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.HardCockWord".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.PluralS".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.WetWaitingVaginaWord".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SuddenPenetration".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.ForearmsOnBench".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.IntoWarmBusyMouth".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SecondDickFreeHand".Localized());

            if (Player.CockTotal() >= 3)
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.DoesBestToPlease".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SamePaceEager".Localized());

            if (!Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.VaginaDiggingSearching".Localized((noFur ? "face" : "muzzle")));

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.HyenaBehindPounds".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.ReleasesCockDipsLower".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.PoundingIntoBrooke".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.CalmlyThrustsNeverSlow".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.MightTakeOffence".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SlowingJustBitLean".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.OneLastPushLunges".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.CumDirectlyIntoThroat".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.SprayingFeminineJuices".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.ContinuesCumAgain".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.KnowCumHungryBitch".Localized());

            OutputText("Scenes.Brooke.SubmitHeckelXBrookeThreesome.BeginsWindDown".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BrookeAndHeckelStuffPartThree);
        }

        public void BrookeAndHeckelStuffPartThree()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartThree.EyesSeeingStars".Localized());

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartThree.StartledHuhWha".Localized());

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartThree.DoesntReplySnarky".Localized());

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartThree.KissForeheadStand".Localized());

            OutputText("Scenes.Brooke.BrookeAndHeckelStuffPartThree.LeaveBeforeWakes".Localized());

            BrookeAffection(5);

            DoNext(() => ReturnToCamp(1));
        }

        public void LegPressInsteadOfThreesome()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.BrookeAndHeckel3some, CoC.Instance.Flags.Get(GameFlagIds.BrookeAndHeckel3some) + 1);

            OutputText("Scenes.Brooke.LegPressInsteadOfThreesome.GroupAroundLegPress".Localized((noFur ? "Animal-eared people" : "Anthromorphic animals")));

            OutputText("Scenes.Brooke.LegPressInsteadOfThreesome.EachMachineBrooke".Localized((noFur ? "-eared man" : "")));

            OutputText("Scenes.Brooke.LegPressInsteadOfThreesome.CocksureGrinExhaling".Localized());

            OutputText("Scenes.Brooke.LegPressInsteadOfThreesome.LookingTowardsHeckel".Localized());

            OutputText("Scenes.Brooke.LegPressInsteadOfThreesome.PalmsPrettyFunBrave".Localized());

            OutputText("Scenes.Brooke.LegPressInsteadOfThreesome.HeckelBehindCrowd".Localized());

            OutputText("Scenes.Brooke.LegPressInsteadOfThreesome.HeckelRightArmShoulders".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BrookeVHeckelBrookeWins2);
        }

        public void BrookeVHeckelBrookeWins2()
        {
            ClearOutput();

            ShowImage("brooke-gym-heckelbrookedominance");

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.IntoLockerRoomClose".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.JustInTimeThrow".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.OntoBenchPresses".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.BodySculptedMakeOut".Localized((noFur ? "skin" : "short fur")));

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.BetweenShakingLegs".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.MuskAmplifiedNoShower".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.GripsLockersAbuse".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.OwnTaskSuckingCock".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.NotInPositionArgue".Localized((noFur ? "gasps" : "yips")));

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.RemissTesticles".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.JerkyMovementsWhining".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.HumpsLetsGoLockers".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.PartnerInCrime".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.GripsBaseCockTightly".Localized());

            var x = Player.CockThatFits(BrookeCapacity());

            if (x >= 0)
            {
                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.SteppingBackStandBehind".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.JubilationProperSex".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.RemindedEmptyPussy".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.TrailingHandsBrooke".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.TakeChanceFuckProperly".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelBlowjob) > 0 || CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelAnal) > 0)
                {
                    var bj = CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelBlowjob) > 0;

                    var anal = CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelAnal) > 0;

                    if (bj && anal)
                        OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.MetHeckelWentDown".Localized());
                    else
                        if (bj && !anal)
                            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.WentDownReturn".Localized());
                        else
                            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.MetHeckelTakeDick".Localized());
                }
                else
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.YWord".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.TightVelvetTunnel".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() >= 2)
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.FlopsLamelyHotdogging".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.GaspsMoreOften".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.HerPartThreesome".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.EveryRidgeMuscles".Localized((noFur ? "gasps" : "yips")));

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.BackBeginStrain".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.KeepingEyeContact".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.ForcefulPushLands".Localized((noFur ? "" : ", running coarse short fur against thick dog fur"), (noFur ? "moaning" : "woofing")));

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.ShortlyAfterFlooding".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.HasKnot(x))
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.PoundKnotAgainst".Localized(
                        Player.CockDescript(x)
                    ));

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.ThrustingHardDrench".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.PulsesSprayingJuices".Localized(
                        Player.CockDescript(x)
                    ));

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.FlailsPulsesEjaculating".Localized());

                if (Player.CockTotal() >= 3)
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.PumpsYetMoreJism".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.SentencePeriod".Localized());
            }
            else
            {
                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.OnTheFloorDog".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.LeaningInClose".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.SharpWhineOverwhelmed".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.MostActiveMostWork".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.ExperienceTongueWanting".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.FallsForwardLayingFlat".Localized((noFur ? "" : " through her short fur")));

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.FollowSuitLeaning".Localized((noFur ? "" : " the fur of")));

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.DelightedPetRises".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.AfterSomeTime".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.KnowingListening".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.ForcefulPushAgain".Localized());

                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.TakeNoMoreHowls".Localized());

                if (Player.Wetness() >= 4)
                    OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.TooMuchSplashesOut".Localized());
            }

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins2.WindsDownPanting".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), BrookeVHeckelBrookeWins3);
        }

        public void BrookeVHeckelBrookeWins3()
        {
            ClearOutput();

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.NotReallyDazed".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.StartledByMotion".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.SurprisedDoesntReplySnarky".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.StillPluggingLips".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.AtSoundBlameHer".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.DisengageYourselfFromHeckel".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.GentlyLayDown".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.BowLeggedMakeWay".Localized());

            OutputText("Scenes.Brooke.BrookeVHeckelBrookeWins3.LeaveBeforeChangesMind".Localized());

            DoNext(() => ReturnToCamp(1));

            BrookeAffection(5);
        }

    }
}