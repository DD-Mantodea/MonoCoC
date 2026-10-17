using MonoCoC.Attributes;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<RaphaelScene>("coc:raphael")]
    public class RaphaelScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Raphael);

        public bool dressGiven { get => CoC.Instance.Flags.Get(GameFlagIds.RaphaelDressGiven); set => CoC.Instance.Flags.Set(GameFlagIds.RaphaelDressGiven, value); }

        public bool rejected { get => CoC.Instance.Flags.Get(GameFlagIds.RejectedRaphael); set => CoC.Instance.Flags.Set(GameFlagIds.RejectedRaphael, value); }

        public int countdownTimer { get => CoC.Instance.Flags.Get(GameFlagIds.RaphealCountdownTimer); set => CoC.Instance.Flags.Set(GameFlagIds.RaphealCountdownTimer, value); }

        public bool bountyReceived { get => CoC.Instance.Flags.Get(GameFlagIds.UrtaPaidOutRaphaelsBounty); set => CoC.Instance.Flags.Set(GameFlagIds.UrtaPaidOutRaphaelsBounty, value); }

        public bool raphaelMet { get => CoC.Instance.Flags.Get(GameFlagIds.RaphaelMet); set => CoC.Instance.Flags.Set(GameFlagIds.RaphaelMet, value); }

        public bool raphaelDisgusted { get => CoC.Instance.Flags.Get(GameFlagIds.RaphaelDisgustedByPcAppearance); set => CoC.Instance.Flags.Set(GameFlagIds.RaphaelDisgustedByPcAppearance, value); }

        public bool retreivedPendant { get => CoC.Instance.Flags.Get(GameFlagIds.FuckOffTheivingRaphael); set => CoC.Instance.Flags.Set(GameFlagIds.FuckOffTheivingRaphael, value); }

        public bool hadSex { get => CoC.Instance.Flags.Get(GameFlagIds.RaphaelBoned); set => CoC.Instance.Flags.Set(GameFlagIds.RaphaelBoned, value); }

        public int timesVistedOrphanage { get => CoC.Instance.Flags.Get(GameFlagIds.TimesOrphanagedWithRaphael); set => CoC.Instance.Flags.Set(GameFlagIds.TimesOrphanagedWithRaphael, value); }

        public int rapierTraining { get => CoC.Instance.Flags.Get(GameFlagIds.RaphaelRapierTraning); set => CoC.Instance.Flags.Set(GameFlagIds.RaphaelRapierTraning, value); }

        public int intelligenceTraining { get => CoC.Instance.Flags.Get(GameFlagIds.RaphaelIntelligenceTraining); set => CoC.Instance.Flags.Set(GameFlagIds.RaphaelIntelligenceTraining, value); }

        public override void Entry()
        {
            // Raphael.hx:33 timeChangeLarge
            if (ReadyForFirstMeeting())
            {
                StartEvent(MeetRaphael);

                return;
            }

            if (ReadyForDress())
            {
                StartEvent(RaphaelDress);

                return;
            }

            if (ReadyForMeetWithDress())
            {
                StartEvent(RaphaelEncounterIIDressFollowup);

                return;
            }

            if (ReadyForFinale())
            {
                StartEvent(QuiksilverFawkesEndGame);

                return;
            }

            Go<CampScene>();
        }

        public bool TimeChange()
        {
            Save.SetInt("checkedRussetRogue", 0);

            return false;
        }

        public bool TimeChangeLarge()
        {
            var previous = Save.GetInt("checkedRussetRogue", 0);

            Save.SetInt("checkedRussetRogue", previous + 1);

            if (previous == 0 && View.Time.Hours == 6)
            {
                if (ReadyForFirstMeeting())
                {
                    StartEvent(MeetRaphael);

                    return true;
                }

                if (ReadyForDress())
                {
                    StartEvent(RaphaelDress);

                    return true;
                }

                if (ReadyForMeetWithDress())
                {
                    StartEvent(RaphaelEncounterIIDressFollowup);

                    return true;
                }

                if (ReadyForFinale())
                {
                    StartEvent(QuiksilverFawkesEndGame);

                    return true;
                }

                if (CountdownStarted())
                    countdownTimer--;
            }

            return false;
        }

        public void StartEvent(Action scene)
        {

            OutputText("Scenes.Raphael.StartEvent.SomethingUnusualHappens".Localized());

            scene();
        }

        public bool ReadyForFirstMeeting()
        {

            if (!raphaelMet && CampScene.HasChest() && Player.Gems >= 5 && Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1 && (RaphaelLikes() || (Random.Shared.Next(100) < (10) && View.Time.Days >= 15)))
                return true;
            else
                return false;
        }

        public bool ReadyForDress()
        {

            if (raphaelMet && !dressGiven && !rejected && RaphaelLikes())
                return true;
            else
                return false;
        }

        public bool ReadyForMeetWithDress()
        {

            if (!rejected && Player.Armor.Id == ArmorIds.R_BDYST && dressGiven && countdownTimer == 0 && (RaphaelLikes() || !raphaelDisgusted))
                return true;
            else
                return false;
        }

        public bool ReadyForFinale()
        {

            return countdownTimer == 1 && RaphaelLikes();
        }

        public bool CountdownStarted()
        {

            return countdownTimer > 0;
        }

        public bool FinaleBetrayed(bool markDone = false)
        {

            if (markDone)
                CoC.Instance.Flags.Set(GameFlagIds.RaphealCountdownTimer, -1);

            return CoC.Instance.Flags.Get(GameFlagIds.RaphealCountdownTimer) == -1;
        }

        public bool FinaleCovered(bool markDone = false)
        {

            if (markDone)
                CoC.Instance.Flags.Set(GameFlagIds.RaphealCountdownTimer, -2);

            return CoC.Instance.Flags.Get(GameFlagIds.RaphealCountdownTimer) == -2;
        }

        public bool RaphaelLikes()
        {

            if (((OptionStore.Instance.Get("lowStandards", 0) > 0)) && (Player.HasVagina()))
                    return true;

            if (!Player.IsBiped())
                return false;

            if (Player.BiggestBreastSize() >= BreastCups.FBig)
                return false;

            if (Player.BiggestBreastSize() < 2)
                return false;

            if (Player.Body.Hips < HipsPart.RatingAmple)
                return false;

            if (Player.Body.Butt >= ButtPart.RatingHuge)
                return false;

            if (Player.Tallness > 72)
                return false;

            if (Player.Tallness < 48)
                return false;

            if (Player.HasBalls() || Player.HasCock())
                return false;

            if (!Player.HasVagina())
                return false;

            return true;
        }

        public void MeetRaphael()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.MeetRaphael.StirInYourSleep".Localized());

            OutputText("Scenes.Raphael.MeetRaphael.SitUpStraight".Localized());

            OutputText("Scenes.Raphael.MeetRaphael.RushOutOfBed".Localized());

            OutputText("Scenes.Raphael.MeetRaphael.CallOutSleepy".Localized());

            OutputText("Scenes.Raphael.MeetRaphael.ExclaimsBoastfully".Localized());

            ShowImage("raphael-talk");

            OutputText("Scenes.Raphael.MeetRaphael.RubYourEyesLook".Localized());

            raphaelMet = true;

            MeetRaphaelPtII();
        }

        public void MeetRaphaelPtII()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.MeetRaphaelPtII.FeaturesGrowSoft".Localized());

            OutputText("Scenes.Raphael.MeetRaphaelPtII.MaraeCursedMe".Localized());

            OutputText("Scenes.Raphael.MeetRaphaelPtII.SmileAtFlourish".Localized());

            ShowImage("raphael-talk");

            OutputText("Scenes.Raphael.MeetRaphaelPtII.SearchedEntireCamp".Localized());

            if (RaphaelLikes())
                OutputText("Scenes.Raphael.MeetRaphaelPtII.KneelsGreatestJewel".Localized());
            else
                OutputText("Scenes.Raphael.MeetRaphaelPtII.ApologiesNotALady".Localized());

            OutputText("Scenes.Raphael.MeetRaphaelPtII.WhatDoYouDo".Localized());

            ClearButtons();

            if (RaphaelLikes())
            {
                AddButton(0, "Common.Button.Talk".Localized(), RaphaelFirstMeetingTALK);

                AddButton(2, "Common.Button.Swoon".Localized(), RaphaelFirstMeetingSWOON);
            }
            else
            {
                AddButton(0, "Common.Button.LetHimGo".Localized(), LetRaphaelGoFirstMeeting);

                AddButton(2, "Common.Button.Swoon".Localized(), null);
            }

            AddButton(1, "Common.Button.Slap".Localized(), RaphaelFirstMeetingSLAP);

            if (Player.Lust >= 33 && Player.HasCock() && (Player.IsCorruptEnough(60) || Player.HasPerk(PerkIds.Pervert)))
                AddButton(3, "Common.Button.Rape".Localized(), RapeRaphael);
            else
                AddButton(3, "Common.Button.Rape".Localized(), null);
        }

        public void RaphaelFirstMeetingSLAP()
        {
            ClearOutput();

            ShowImage("raphael-slap");

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.OffendedScowlSlap".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.TakesHimBySurprise".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.NoIntentionTainting".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.WrestleHandBackhand".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.TwiceTheBlindFool".Localized());

            if (RaphaelLikes())
                OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.AdviseYouGainWeight".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.CurtsiesTipsHat".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSLAP.ResolveToWashHand".Localized());

            rejected = true;

            Go<CampScene>();
        }

        public void RaphaelFirstMeetingSWOON()
        {
            ClearOutput();

            ShowImage("raphael-swoon");

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.SnickerBlush".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.NormallyRobMerchants".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.AskWhoHeIs".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.WhyIAmRaphael".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.FaintSoundHeard".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.ShiningJewel".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.BlinkJumpUpWall".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingSWOON.HoldHandHeTouched".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Speed, 3f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 25f));

            Go<CampScene>();
        }

        public void RaphaelFirstMeetingTALK()
        {
            ClearOutput();

            ShowImage("raphael-talk");

            OutputText("Scenes.Raphael.RaphaelFirstMeetingTALK.SquintWrestleArm".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingTALK.MereSouvenirs".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingTALK.InsistReturnAll".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingTALK.TiringOfCharades".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingTALK.ArentWeFrisky".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingTALK.InspectTheseGoods".Localized());

            OutputText("Scenes.Raphael.RaphaelFirstMeetingTALK.BlinkJumpUpWall".Localized());

            Player.LoseGems(5);

            Go<CampScene>();
        }

        public void LetRaphaelGoFirstMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.LetRaphaelGoFirstMeeting.TellHimLetGo".Localized());

            OutputText("Scenes.Raphael.LetRaphaelGoFirstMeeting.ThankYouFriend".Localized());

            Player.LoseGems(5);

            Go<CampScene>();
        }

        public void RapeRaphael()
        {
            ClearOutput();

            rejected = true;

            OutputText("Scenes.Raphael.RapeRaphael.FoxMustBePunished".Localized());

            if (Player.Strength + Random.Shared.Next(30) < 50)
            {
                OutputText("Scenes.Raphael.RapeRaphael.WigglesOutRunsAway".Localized());

                Player.GainGems(Utils.Utils.RandBetween(10, 30));

                Go<CampScene>();

                return ;
            }

            ShowImage("raphael");

            OutputText("Scenes.Raphael.RapeRaphael.PinHimUnderWeight".Localized(
                Player.ClothedOrNakedLower(", remove your [armor]", "")
            ));

            OutputText("Scenes.Raphael.RapeRaphael.GaspsAsCockMeets".Localized());

            OutputText("Scenes.Raphael.RapeRaphael.YellsNotNormal".Localized());

            OutputText("Scenes.Raphael.RapeRaphael.ShoveHimOnGround".Localized());

            if (Player.CountCocksOfType("TENTACLE") > 0)
                OutputText("Scenes.Raphael.RapeRaphael.PushTentacleCockIn".Localized());

            OutputText("Scenes.Raphael.RapeRaphael.HoldBackNoMore".Localized());

            Player.GainGems(Utils.Utils.RandBetween(100, 120));

            Player.Orgasm("Dick");

            ShowImage("item-rRapier");

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.RaphaelsRapier), 1);

            RapeRaphaelII();
        }

        public void RapeRaphaelII()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RapeRaphaelII.CleanAndDragHim".Localized(
                Player.ClothedOrNakedLower(", redress in your [armor]", "")
            ));

            Go<CampScene>();
        }

        public void RaphaelDress()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelDress.PebbleHitsGround".Localized());

            OutputText("Scenes.Raphael.RaphaelDress.RubbingEyesGetUp".Localized());

            OutputText("Scenes.Raphael.RaphaelDress.CheckStorageChest".Localized());

            OutputText("Scenes.Raphael.RaphaelDress.HappenedAcrossEnsemble".Localized());

            OutputText("Scenes.Raphael.RaphaelDress.YoursFaithfully".Localized());

            OutputText("Scenes.Raphael.RaphaelDress.PostscriptGift".Localized());

            OutputText("Scenes.Raphael.RaphaelDress.HoldRubyPendant".Localized());

            OutputText("Scenes.Raphael.RaphaelDress.PutItDown".Localized());

            RaphaelDressPtII();
        }

        public void RaphaelDressPtII()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelDressPtII.OnePieceSuit".Localized());

            OutputText("Scenes.Raphael.RaphaelDressPtII.GossamerDensity".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Raphael.RaphaelDressPtII.TailPeeksOut".Localized());

            OutputText("Scenes.Raphael.RaphaelDressPtII.GenerousGlimpse".Localized(
                Player.BreastDescript(0)
            ));

            OutputText("Scenes.Raphael.RaphaelDressPtII.BlushAsWindBreezes".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Raphael.RaphaelDressPtII.StandUpLookOver".Localized());

            dressGiven = true;

            ShowImage("item-rBdyst");

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.R_BDYST), 1);

            PlayerMenu();
        }

        public void RaphaelEncounterIIDressFollowup()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.AwakeToPatter".Localized());

            OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.RushOutSpotRed".Localized());

            ShowImage("raphael-talk");

            OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.AmbleTowardsRogue".Localized());

            if (RaphaelLikes())
            {
                raphaelDisgusted = false;

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.HeSmirksBreakfast".Localized());

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.InspectRestOfBody".Localized());

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.PriceOfAdmission".Localized());

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.WhatDoYouDo".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Reject".Localized(), RaphaelChooseReject);

                AddButton(1, "Common.Button.Frisk".Localized(), RaphaelChooseFrisk);

                AddButton(2, "Common.Button.Date".Localized(), RaphaelSelectDate);
            }
            else
            {
                raphaelDisgusted = true;

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.CatchesSightSpurts".Localized());

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.MonDieuShock".Localized());

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.WhatTerribleTragedy".Localized());

                if (Player.HasTailInsteadOfLegs() || Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Goo || Player.IsTaur() || Player.IsDrider())
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.MissingHalfBody".Localized());

                if (Player.BiggestBreastSize() >= 7)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.UddersInsteadOfCurves".Localized());

                if (Player.BiggestBreastSize() < 3)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.FemaleCurvesGone".Localized());

                if (Player.Body.Hips < 6)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.FineHourglassFigure".Localized());

                if (Player.Body.Butt >= 13)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.LookAtYourButt".Localized());

                if (Player.Tallness > 72)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.RollsEyesGiantBody".Localized());

                if (Player.Tallness < 48)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.SquintsDiminutive".Localized());

                if (Player.Balls > 0 || Player.CockTotal() > 0)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.InspectsYourGroin".Localized());

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.SomethingDifferentSmell".Localized());

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.LooksDisappointed".Localized());

                OutputText("Scenes.Raphael.RaphaelEncounterIIDressFollowup.ClenchJawOffended".Localized());

                Go<CampScene>();
            }
        }

        public void RaphaelSelectDate()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelSelectDate.SmileBashfully".Localized());

            OutputText("Scenes.Raphael.RaphaelSelectDate.HopsToFeetGloats".Localized());

            OutputText("Scenes.Raphael.RaphaelSelectDate.FeelsLikeConsenting".Localized());

            RaphaelPicnic();
        }

        public void RaphaelChooseReject()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelChooseReject.SighAndShakeHead".Localized());

            OutputText("Scenes.Raphael.RaphaelChooseReject.FlicksTailVanish".Localized());

            OutputText("Scenes.Raphael.RaphaelChooseReject.RussetRogueNoThief".Localized());

            OutputText("Scenes.Raphael.RaphaelChooseReject.FlowerRefusedToBloom".Localized());

            OutputText("Scenes.Raphael.RaphaelChooseReject.TwoSecondsVanish".Localized());

            rejected = true;

            Go<CampScene>();
        }

        public void RaphaelChooseFrisk()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelChooseFrisk.FlashPlayfulSmile".Localized());

            OutputText("Scenes.Raphael.RaphaelChooseFrisk.HopsFromPerch".Localized());

            OutputText("Scenes.Raphael.RaphaelChooseFrisk.LeavesMoreRoom".Localized());

            if ((Player.Intelligence < 24 && Player.Speed < 24) && Player.Corruption < 19)
            {
                OutputText("Scenes.Raphael.RaphaelChooseFrisk.FrownBefuddled".Localized());

                OutputText("Scenes.Raphael.RaphaelChooseFrisk.ArmsAroundHisNeck".Localized());

                OutputText("Scenes.Raphael.RaphaelChooseFrisk.ForgiveMeGrowls".Localized());

                OutputText("Scenes.Raphael.RaphaelChooseFrisk.DayIsYoung".Localized());

                RaphaelPicnic();
            }
            else
                if ((Player.Intelligence < 36 && Player.Speed < 36) && Player.Corruption < 36)
                {
                    OutputText("Scenes.Raphael.RaphaelChooseFrisk.NoticeMovingTail".Localized());

                    if (Player.Corruption <= 15)
                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.UrgeToGrabBulge".Localized());
                    else
                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.GrabHimByBulge".Localized());

                    OutputText("Scenes.Raphael.RaphaelChooseFrisk.ArentWeFrisky".Localized());

                    OutputText("Scenes.Raphael.RaphaelChooseFrisk.DayYoungAgain".Localized());

                    RaphaelPicnic();
                }
                else
                    if (Player.Corruption < 35)
                    {
                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.FigureOutMovingTail".Localized());

                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.NiceTryRetrieve".Localized());

                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.GraciousLoserBeams".Localized());

                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.HowDoYouRespond".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Reject".Localized(), FriskRejectChoice);

                        AddButton(1, "Common.Button.Accept".Localized(), FriskAcceptChoice);
                    }
                    else
                    {
                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.DontManageToFollow".Localized());

                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.EagerLittleThing".Localized());

                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.HappyToSeeYou".Localized());

                        OutputText("Scenes.Raphael.RaphaelChooseFrisk.WhatDoYouDo".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Squeeze".Localized(), FriskSqueezeChoice);

                        AddButton(1, "Common.Button.Fondle".Localized(), FriskFondleChoice);
                    }
        }

        public void FriskAcceptChoice()
        {
            ClearOutput();

            retreivedPendant = true;

            OutputText("Scenes.Raphael.FriskAcceptChoice.ComeOnShootsUp".Localized());

            OutputText("Scenes.Raphael.FriskAcceptChoice.WaiveHandWalkAround".Localized());

            RaphaelPicnic();
        }

        public void FriskRejectChoice()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.FriskRejectChoice.ScoffCrossArms".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.AsYouSayRose".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.StartleBangChest".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.NoTeachingLesserStock".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.EvenWithoutABelt".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.BoltsLookOfSurprise".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.YelpsLosesHold".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.RegainMyHonor".Localized());

            OutputText("Scenes.Raphael.FriskRejectChoice.LaughAsHeBlushes".Localized());

            rejected = true;

            Go<CampScene>();
        }

        public void FriskFondleChoice()
        {
            ClearOutput();

            ShowImage("raphael");

            OutputText("Scenes.Raphael.FriskFondleChoice.MassageLeatheryPackage".Localized());

            OutputText("Scenes.Raphael.FriskFondleChoice.SuchAGoodGirl".Localized());

            OutputText("Scenes.Raphael.FriskFondleChoice.RemoveFoggedStones".Localized());

            OutputText("Scenes.Raphael.FriskFondleChoice.WillRetrieveThose".Localized());

            OutputText("Scenes.Raphael.FriskFondleChoice.DayYoungShootsUp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Speed, 3f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 25f));

            RaphaelPicnic();
        }

        public void FriskSqueezeChoice()
        {
            ClearOutput();

            ShowImage("raphael");

            OutputText("Scenes.Raphael.FriskSqueezeChoice.DigIntoSqueeze".Localized());

            OutputText("Scenes.Raphael.FriskSqueezeChoice.SomeBallsStealing".Localized());

            OutputText("Scenes.Raphael.FriskSqueezeChoice.ShakesHeadTear".Localized());

            OutputText("Scenes.Raphael.FriskSqueezeChoice.ChuckleLookAround".Localized());

            OutputText("Scenes.Raphael.FriskSqueezeChoice.RepayTheFavor".Localized());

            OutputText("Scenes.Raphael.FriskSqueezeChoice.SingleFingerGroans".Localized());

            rejected = true;

            Go<CampScene>();
        }

        public void RaphaelPicnic()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelPicnic.LeadsYouByHand".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnic.TrekAnotherHalfHour".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnic.BreakfastIsServed".Localized());

            ShowImage("raphael-picnic");

            OutputText("Scenes.Raphael.RaphaelPicnic.FlowerFlankedArrangement".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnic.OnlyWhenHeEats".Localized());

            RaphaelPicnicII();
        }

        public void RaphaelPicnicII()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelPicnicII.ClearThroatKnowingly".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnicII.AskAboutMareth".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fencing".Localized(), RaphaelPicnicSkill);

            AddButton(1, "Common.Button.Thieving".Localized(), () => RaphaelPicnicChooseThieving());
        }

        public void RaphaelPicnicEnd()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelPicnicEnd.ThiefMoreThanGems".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnicEnd.GladlySpendEternity".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnicEnd.KeepingUpAppearances".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnicEnd.WinkAndBow".Localized());

            OutputText("Scenes.Raphael.RaphaelPicnicEnd.SeemsSoHasty".Localized());

            if (!retreivedPendant)
            {
                OutputText("Scenes.Raphael.RaphaelPicnicEnd.MinuteOfSilenceThrows".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicEnd.CatchReturnedPendant".Localized());
            }
            else
            {
                retreivedPendant = false;

                OutputText("Scenes.Raphael.RaphaelPicnicEnd.MinuteOfSilenceMakes".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicEnd.RecollectAndHeadBack".Localized());
            }

            Go<CampScene>();
        }

        public void RaphaelPicnicSkill()
        {
            ClearOutput();

            if (rapierTraining == 0)
            {
                OutputText("Scenes.Raphael.RaphaelPicnicSkill.DistractedByOpulence".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.EyeCaughtByRapier".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.AskIfSmallWeapon".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.MomentOfAwkwardSilence".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.MansWeaponBeams".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.GleamsSensually".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.PointBetweenBreasts".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.CouldLearnToUse".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.DontFlinchStare".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.AskIfThatsWeird".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.ShallWeJumpsUp".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.ArmorInsteadOfSilk".Localized());
            }

            if (Player.Speed < 30 || rapierTraining == 0)
            {
                OutputText("Scenes.Raphael.RaphaelPicnicSkill.AmbleToMossyField".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.ThrowsWeaponDown".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.ArentWeUsingWeapon".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.SoEagerSnarls".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.TryToExplain".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.ObserveMotions".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.FirstThingMoveFeet".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.RetreatsWildly".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.SubtleMovementFingers".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.FallsIntoRepertoire".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.EnergyLeftFlourish".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.JoinMeFineFae".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.MumbleDance".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.DanceSnarlsArm".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.TriesToLeadTango".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.ForcedToStopPractice".Localized());

                ShowImage("raphael-dancin");

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.ReassureHim".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.MostBeautifulThing".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.SmileReturnsRaphael".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicSkill.AgainExtendsArm".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Speed, 3f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 25f));

                rapierTraining = 1;
            }
            else
                if (Player.Speed < 39 || rapierTraining == 1)
                {
                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.GrabHimInField".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.PartOfDance".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.VamanosLeadsDance".Localized());

                    ShowImage("raphael-dancin");

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.ConcentrateOnFeet".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.ShiftsFromSteps".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.PicksYouBackUp".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.DigHeelsStopHim".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.DoNotFearBlade".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.GaspForbidsYou".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.TreatBladeLikeLovers".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.PushesYouAway".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.StandStillFeelWeapon".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicSkill.MagnefiqueComments".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Speed, 3f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 25f));

                    rapierTraining = 2;
                }
                else
                    if (Player.Speed < 49 || rapierTraining == 2)
                    {
                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.FancyFootworkGranted".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.BeratesAllWrong".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.HadItComing".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.GrabsArmSqueezes".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.FaceBesideYours".Localized(
                            Player.ButtDescript()
                        ));

                        ShowImage("raphael-hug");

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.RelishTheBlade".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.ProperPostureNow".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.RememberHowThisFeels".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.FirmerBrushRemoves".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Speed, 3f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Lust, 60f));

                        rapierTraining = 3;
                    }
                    else
                    {
                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.DrawnToRapierAgain".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.WeaponLyingBetween".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.LikeToOwnIt".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.SmileCautiousOffer".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.SparOverIt".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicSkill.WhatDoYouDo".Localized());

                        rapierTraining = 4;
// 原版 Achievements.hx:223「Fencer」：完成 Raphael 的刺剑训练
            AwardAchievement(new Identifier("coc", "general_fencer"));

                        ClearButtons();

                        AddButton(0, "Common.Button.Fence".Localized(), FenceRaphaelSexily);

                        AddButton(1, "Common.Button.Discuss".Localized(), FenceOfferChangeToDiscuss);

                        return ;
                    }

            RaphaelPicnicEnd();
        }

        public void FenceOfferChangeToDiscuss()
        {

            OutputText("Scenes.Raphael.FenceOfferChangeToDiscuss.BlushChangeSubject".Localized());

            RaphaelPicnicChooseThieving(false);
        }

        public void FenceRaphaelSexily()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.FenceRaphaelSexily.MostBeautifulMorning".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexily.TallFieryFox".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexily.DrawYourWeapon".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexily.EagerSeeHimSweat".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexily.SheathSwipesSideways".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexily.EnGarde".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexily.DanceOnceAgain".Localized());

            ShowImage("raphael-fence");

            OutputText("Scenes.Raphael.FenceRaphaelSexily.ManagesToCounterattack".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexily.SoftMetallicScrape".Localized());

            FenceRaphaelSexilyPtII();
        }

        public void FenceRaphaelSexilyPtII()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.LittleEmbarrassed".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.TrulyMagnificent".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.StammerNotSure".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.NodsReassuringly".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.AskWhatLesson".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.OpponentWasMe".Localized());

            OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.AskWhatHeMeans".Localized(
                Player.VaginaDescript(0)
            ));

            if (intelligenceTraining == 4)
            {
                OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.GrownWiseHisWays".Localized());

                OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.GuideHimAway".Localized());
            }

            if (Player.Intelligence > 49)
            {
                OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.StillFightingSpirit".Localized());

                OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.NeverMetWomanSkills".Localized());

                OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.SolicitingMisterFox".Localized());

                OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.GamesMeantToLose".Localized());

                OutputText("Scenes.Raphael.FenceRaphaelSexilyPtII.DoYouLetHim".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), RaphaelPostFenceSex);

                AddButton(1, "Common.Button.No".Localized(), DeclinePuttingOutForRogues);
            }
            else
                RaphaelPostFenceSex();
        }

        public void RaphaelPostFenceSex()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.TooLateSayNo".Localized());

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.WhiskersSmoothSoft".Localized(
                Player.BreastDescript(0)
            ));

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.GrabsRightLeg".Localized());

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.TimeSeemsRight".Localized(
                Player.VaginaDescript(0)
            ));

            ShowImage("raphael-sex-woods");

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.TrembleAsHeShifts".Localized());

            Player.CuntChange(12, true);

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.SlowlyOscillates".Localized());

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.BearItNoMore".Localized());

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.VictoriousGlintRolls".Localized());

            OutputText("Scenes.Raphael.RaphaelPostFenceSex.SpendRestOfMorning".Localized());

            Player.Orgasm("Vaginal");

            PostRaphaelCoitus();
        }

        public void PostRaphaelCoitus()
        {
            ClearOutput();

            hadSex = true;

            OutputText("Scenes.Raphael.PostRaphaelCoitus.WakeOnMoss".Localized());

            if (rapierTraining == 4)
            {
                OutputText("Scenes.Raphael.PostRaphaelCoitus.RapierLeftBehind".Localized());

                ShowImage("item-rRapier");

                Player.Inventory.Add(ItemRegistry.Create(ItemIds.RaphaelsRapier), 1);

                PlayerMenu();

                if (!Player.HasPerk(PerkIds.RapierTraining))
                    Player.CreatePerkIfNotHasPerk(PerkIds.RapierTraining, 0, 0, 0, 0);

                OutputText("Scenes.Raphael.PostRaphaelCoitus.GainedRapierTraining".Localized());
            }

            if (intelligenceTraining == 4)
            {
                OutputText("Scenes.Raphael.PostRaphaelCoitus.LeftMoreThanMemories".Localized());

                if (!Player.HasPerk(PerkIds.Misdirection))
                    Player.CreatePerkIfNotHasPerk(PerkIds.Misdirection, 0, 0, 0, 0);
            }

            OutputText("Scenes.Raphael.PostRaphaelCoitus.ReturnToCampCleaned".Localized());

            countdownTimer = 7;

            if (rapierTraining != 4)
                Go<CampScene>();
        }

        public void DeclinePuttingOutForRogues()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.ShakeHeadNo".Localized());

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.FoxFrownsShield".Localized());

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.MagicStillThere".Localized());

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.GetsOffBody".Localized());

            ShowImage("raphael");

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.MumblesRollsOff".Localized());

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.SmileSorryForHim".Localized());

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.FoxFinallySpeaks".Localized());

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.TurnAroundHeVanished".Localized());

            if (rapierTraining == 4)
            {
                OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.RapierLeftBehind".Localized());

                ShowImage("item-rRapier");

                Player.Inventory.Add(ItemRegistry.Create(ItemIds.RaphaelsRapier), 1);

                PlayerMenu();

                Player.CreatePerkIfNotHasPerk(PerkIds.RapierTraining, 0, 0, 0, 0);

                OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.GainedRapierTraining".Localized());
            }

            if (intelligenceTraining == 4)
            {
                OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.MoreThanMemories".Localized());

                Player.CreatePerkIfNotHasPerk(PerkIds.Misdirection, 0, 0, 0, 0);
            }

            OutputText("Scenes.Raphael.DeclinePuttingOutForRogues.ReturnToCampCleaned".Localized());

            countdownTimer = 7;

            rejected = true;

            if (rapierTraining != 4)
                Go<CampScene>();
        }

        public void RaphaelPicnicChooseThieving(bool newl = true)
        {
            ClearOutput();

            if (intelligenceTraining == 0)
            {
                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.FacedByRussetRogue".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.CertainlyNotGrins".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.AsksWhatDoingNow".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.CuteForPickpocket".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.WhatElseRevealed".Localized());
            }

            if (Player.Intelligence < 30 || intelligenceTraining == 0)
            {
                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.HowDoPickpockets".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.SpareribNibble".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.BlushStaringBack".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.AskWhatsTheMatter".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.TryToCrackSmile".Localized());

                ShowImage("raphael-hug");

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.HearZipperPull".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.RemainsAmicable".Localized());

                OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.EmbarrassedUhHuh".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Intelligence, 3f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 40f));

                intelligenceTraining = 1;
            }
            else
                if (Player.Intelligence < 39 || intelligenceTraining == 1)
                {
                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.AreRoguesBorn".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.DependsOnRogue".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.CouldLearnIt".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.CouldTeachIt".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.NotWhatYouMeant".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.LeansForwardsStaring".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.MovesLikeOnesNow".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.ConversationHardlyMatters".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.GameOfChess".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.TwoCanPlay".Localized());

                    ShowImage("raphael-hug");

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.TooCaughtUpPlaying".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.NotBadDisconnects".Localized());

                    OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.AskWhatMeans".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Intelligence, 3f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 40f));

                    intelligenceTraining = 2;
                }
                else
                    if (Player.Intelligence < 39 || intelligenceTraining == 2)
                    {
                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.AlwaysThisCharmer".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.WearySmileNatural".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.WhatForRelaxation".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.ImagineHimHomely".Localized());

                        ShowImage("raphael-hug");

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.CornerOfEyeApproach".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.RealizeWhatHeMeant".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.HumorHimClose".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.PlummetsToBlanket".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.DoesntGetAngry".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.CrackUpLaugh".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Intelligence, 3f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 20f));

                        intelligenceTraining = 3;
                    }
                    else
                    {
                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.GottenUsedToRogue".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.FoxDoingSameThing".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.ReadyForAnotherChat".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.PreparingForLongHaul".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.OccursToChangeSubject".Localized());

                        OutputText("Scenes.Raphael.RaphaelPicnicChooseThieving.WhatDoYouDo".Localized());

                        intelligenceTraining = 4;

                        ClearButtons();

                        AddButton(0, "Common.Button.Fencing".Localized(), RaphaelPicnicSkill);

                        AddButton(1, "Common.Button.Flirt".Localized(), ThieveryEnding);

                        return ;
                    }

            RaphaelPicnicEnd();
        }

        public void ThieveryEnding()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.ThieveryEnding.BeautifulMorningMeadows".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.RemainsSilentNoNeed".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.BreakSilenceAsk".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.EntireWorldRides".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.SnapDecisionIndulge".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.HoldBreathHeCloses".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.StruggleNotMeetLips".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.KissesAcrossLeg".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Raphael.ThieveryEnding.PricelessBrooch".Localized());

            if (rapierTraining != 4)
                OutputText("Scenes.Raphael.ThieveryEnding.TryToReachOrnament".Localized());
            else
                OutputText("Scenes.Raphael.ThieveryEnding.FondleOrnament".Localized());

            ShowImage("raphael-hug");

            OutputText("Scenes.Raphael.ThieveryEnding.RealizingDistracted".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.EnjoyingYourself".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.NeverMetWomanInsight".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.SolicitingMisterFox".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.GamesMeantToLose".Localized());

            OutputText("Scenes.Raphael.ThieveryEnding.DoYou".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            AddButton(0, "Common.Button.Yes".Localized(), RaphaelThieverySmex);

            AddButton(1, "Common.Button.No".Localized(), DeclinePuttingOutForRogues);
        }

        public void RaphaelThieverySmex()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelThieverySmex.NeedNoWords".Localized());

            OutputText("Scenes.Raphael.RaphaelThieverySmex.FragileTensionBreaks".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Raphael.RaphaelThieverySmex.GaspAsHeSpreads".Localized(
                Player.VaginaDescript(0)
            ));

            ShowImage("raphael-sex-woods");

            OutputText("Scenes.Raphael.RaphaelThieverySmex.LowersHimselfPosition".Localized());

            Player.CuntChange(12, true);

            OutputText("Scenes.Raphael.RaphaelThieverySmex.SlowlyOscillates".Localized());

            OutputText("Scenes.Raphael.RaphaelThieverySmex.BearItNoMore".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Raphael.RaphaelThieverySmex.VictoriousGlintSash".Localized());

            OutputText("Scenes.Raphael.RaphaelThieverySmex.SpendRestOfMorning".Localized());

            RaphaelThieverySmexPtII();
        }

        public void RaphaelThieverySmexPtII()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelThieverySmexPtII.WakeOnMoss".Localized());

            if (rapierTraining == 4)
            {
                OutputText("Scenes.Raphael.RaphaelThieverySmexPtII.RapierLeftBehind".Localized());

                ShowImage("item-rRapier");

                Player.Inventory.Add(ItemRegistry.Create(ItemIds.RaphaelsRapier), 1);

                PlayerMenu();

                Player.CreatePerkIfNotHasPerk(PerkIds.RapierTraining, 0, 0, 0, 0);

                OutputText("Scenes.Raphael.RaphaelThieverySmexPtII.GainedRapierTraining".Localized());
            }

            if (intelligenceTraining == 4)
            {
                OutputText("Scenes.Raphael.RaphaelThieverySmexPtII.LeftMoreThanMemories".Localized());

                Player.CreatePerkIfNotHasPerk(PerkIds.Misdirection, 0, 0, 0, 0);

                OutputText("Scenes.Raphael.RaphaelThieverySmexPtII.GainedMisdirection".Localized());
            }

            OutputText("Scenes.Raphael.RaphaelThieverySmexPtII.ReturnToCampCleaned".Localized());

            countdownTimer = 7;

            if (rapierTraining != 4)
                Go<CampScene>();
        }

        public void QuiksilverFawkesEndGame()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.WakeToOminousCry".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.EerilyMistyMorning".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.BegsYouMustHelp".Localized());

            if (rejected)
                OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.ConfusedAskWhatsUp".Localized());

            if (hadSex)
                OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.MiffedAtFox".Localized());

            ShowImage("raphael");

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.HoundNightAndDay".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.NoChanceToRespond".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.HazyDistanceShadow".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.SmelledOfElderberries".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.HatiIdenticalVoice".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.TeethVanishEyesSoft".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesEndGame.RemovesItselfFromYou".Localized());

            QuiksilverFawkesPtII();
        }

        public void QuiksilverFawkesPtII()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.PairOfWolves".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.JumpUpNudgesNose".Localized());

            ShowImage("raphael-chase");

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.HatiManners".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.SpecialDivisionTaskForce".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.TrackingRedFox".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.AskWhatCrimes".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.MustHaveGoneRomp".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.SetStandards".Localized());

            if (rejected)
                OutputText("Scenes.Raphael.QuiksilverFawkesPtII.SmellTracesOfHim".Localized());
            else
                OutputText("Scenes.Raphael.QuiksilverFawkesPtII.SheMustKnow".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.HeartJumpsPileClothes".Localized());

            if (Player.ArmorName == "red, high-society bodysuit")
                OutputText("Scenes.Raphael.QuiksilverFawkesPtII.EyesEarsPerk".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.ParagraphBreak".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.SkollReprovesHati".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.NotACitizen".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.GlanceAtWaterbarrel".Localized());

            OutputText("Scenes.Raphael.QuiksilverFawkesPtII.SellOutOrCover".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Cover".Localized(), CoverForRaphael);

            AddButton(1, "Common.Button.SellOut".Localized(), BetrayRaphael);
        }

        public void CoverForRaphael()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.CoverForRaphael.StareBackAtWolf".Localized());

            if (Player.Corruption < 30)
                OutputText("Scenes.Raphael.CoverForRaphael.FeelBadForLying".Localized());

            OutputText("Scenes.Raphael.CoverForRaphael.CuteAnswer".Localized());

            OutputText("Scenes.Raphael.CoverForRaphael.HairStandsUpright".Localized());

            OutputText("Scenes.Raphael.CoverForRaphael.HatiEnough".Localized());

            if (Player.Corruption < 30)
                OutputText("Scenes.Raphael.CoverForRaphael.ShakeHeadTortured".Localized());

            OutputText("Scenes.Raphael.CoverForRaphael.ParagraphBreak".Localized());

            OutputText("Scenes.Raphael.CoverForRaphael.TrailStrongestMeadows".Localized());

            OutputText("Scenes.Raphael.CoverForRaphael.HopeYouLetKnow".Localized());

            OutputText("Scenes.Raphael.CoverForRaphael.PicksUpClothesDeparts".Localized());

            CoverForRaphaelII();
        }

        public void CoverForRaphaelII()
        {
            ClearOutput();

            ShowImage("raphael-talk");

            OutputText("Scenes.Raphael.CoverForRaphaelII.MinuteToAppear".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.HasntLostHisWays".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.MerciMademoiselle".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.UpsetForAskingLie".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.SpluttersNotLikeThat".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.ConsiderDubiousness".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.RiseToOneKnee".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.RushesBackToSpot".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.OweGreatBoon".Localized());

            OutputText("Scenes.Raphael.CoverForRaphaelII.BlinkAsHeFades".Localized());

            FinaleCovered(true);

            Go<CampScene>();
        }

        public void BetrayRaphael()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.BetrayRaphael.AdmitTheBarrel".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.CarefulBareTeeth".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.TwinsLookGrin".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.BurstsFromWater".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.TellItToJudge".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.LiftsBarrelShoulder".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.OtherRegardsYou".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.CaughtInRecordTime".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.OneMoreAmicableGlance".Localized());

            OutputText("Scenes.Raphael.BetrayRaphael.BlinkAsTheyFade".Localized());

            FinaleBetrayed(true);

            Go<CampScene>();
        }

        public void OrphanageIntro()
        {
            ClearOutput();

            if (RaphaelLikes())
                if (timesVistedOrphanage == 0)
                {
                    OutputText("Scenes.Raphael.OrphanageIntro.SearchTelAdreOrphanage".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.BeggarsAndGuards".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.SparseDirections".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.AtLeastOrphansHere".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.WearyOpenDoor".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.CallOutThrice".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.DoorUnlockedMinute".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.TwoSecondGlimpse".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.FinallyYouMadeIt".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.OrphanageUtter".Localized());

                    ShowImage("raphael-talk");

                    OutputText("Scenes.Raphael.OrphanageIntro.FakeLookOfInnocence".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.ChildrensPartyBlink".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.ParentsYouSee".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.ParentsStammer".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.ProspectiveParents".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.MasterAnotherFox".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.EyesFlashOpen".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.CareToTryAgain".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.NotEntirelyHonest".Localized());

                    OutputText("Scenes.Raphael.OrphanageIntro.StillWaitingGrabsRope".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), RaphaelIntroPageTwo);

                    timesVistedOrphanage += 1;
                }
                else
                    if (View.Time.Hours >= 19 && View.Time.Hours <= 21)
                    {
                        OutputText("Scenes.Raphael.OrphanageIntro.WalkIntoAlley".Localized());

                        timesVistedOrphanage += 1;

                        if (Player.Speed < 33)
                        {
                            OutputText("Scenes.Raphael.OrphanageIntro.ElegantEntranceFails".Localized());

                            OutputText("Scenes.Raphael.OrphanageIntro.HoHoPricelessTreasure".Localized());
                        }
                        else
                            if (Player.Speed < 66)
                            {
                                OutputText("Scenes.Raphael.OrphanageIntro.GettingHangOfIt".Localized());

                                OutputText("Scenes.Raphael.OrphanageIntro.GettingBetterMoth".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Raphael.OrphanageIntro.HoistComesUpShort".Localized());

                                OutputText("Scenes.Raphael.OrphanageIntro.ThinkingOfYou".Localized());
                            }

                        ShowImage("raphael-talk");

                        OutputText("Scenes.Raphael.OrphanageIntro.GladYouCouldCome".Localized());

                        OutputText("Scenes.Raphael.OrphanageIntro.GesturesToTheBed".Localized());

                        OutputText("Scenes.Raphael.OrphanageIntro.SitOnRicketyBed".Localized());

                        OutputText("Scenes.Raphael.OrphanageIntro.OnlyThingKeepsAlert".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Talk".Localized(), TalkWithRedFoxLooksOutHesASpy);

                        AddButton(1, "Common.Button.Sex".Localized(), RaphaelOrphanageSexMenu);

                        AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                    }
                    else
                    {
                        OutputText("Scenes.Raphael.OrphanageIntro.WalkIntoAlleyAgain".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
                    }
        }

        public void RaphaelIntroPageTwo()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.RaphaelIntroPageTwo.NotReallyAnOrphanage".Localized());

            OutputText("Scenes.Raphael.RaphaelIntroPageTwo.CalledByCity".Localized());

            OutputText("Scenes.Raphael.RaphaelIntroPageTwo.WhatOtherFunctions".Localized());

            OutputText("Scenes.Raphael.RaphaelIntroPageTwo.PublicEstablishment".Localized());

            OutputText("Scenes.Raphael.RaphaelIntroPageTwo.EqualizeYourPossessions".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), EvenMoreRaphaelIntro);
        }

        public void EvenMoreRaphaelIntro()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.DriftAwayFromRogue".Localized());

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.GardenUpHere".Localized());

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.LittlePastimeGarden".Localized());

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.CrossArmsSmirk".Localized());

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.RosesDoBloom".Localized());

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.RemindedOfRedBodysuit".Localized());

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.ThinkingOfThisMoment".Localized());

            OutputText("Scenes.Raphael.EvenMoreRaphaelIntro.FirmSwerveTurnsYou".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Kiss".Localized(), KissRaphaelFirstTimeOrphanage);

            AddNextButton("Common.Button.Leave".Localized(), GetTheFuckOutOfDodge);
        }

        public void GetTheFuckOutOfDodge()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.GetTheFuckOutOfDodge.PolitelyInformGo".Localized());

            OutputText("Scenes.Raphael.GetTheFuckOutOfDodge.WellThatWentWell".Localized());

            ReturnToCamp(1);
        }

        public void KissRaphaelFirstTimeOrphanage()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.KissRaphaelFirstTimeOrphanage.DriftIntoRaphael".Localized());

            OutputText("Scenes.Raphael.KissRaphaelFirstTimeOrphanage.MovesBackGarden".Localized());

            OutputText("Scenes.Raphael.KissRaphaelFirstTimeOrphanage.BriefMomentVulnerability".Localized());

            Player.CuntChange(5, true, true, false);

            ShowImage("raphael-sex-home");

            OutputText("Scenes.Raphael.KissRaphaelFirstTimeOrphanage.SomethingToBeSaid".Localized());

            OutputText("Scenes.Raphael.KissRaphaelFirstTimeOrphanage.NotMostEndowed".Localized());

            OutputText("Scenes.Raphael.KissRaphaelFirstTimeOrphanage.SmoothTipCervix".Localized());

            OutputText("Scenes.Raphael.KissRaphaelFirstTimeOrphanage.BegHimToStop".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FollowupToFirstTimeOrphanageRaphSex);
        }

        public void FollowupToFirstTimeOrphanageRaphSex()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.FollowupToFirstTimeOrphanageRaphSex.WakeUpStartled".Localized());

            OutputText("Scenes.Raphael.FollowupToFirstTimeOrphanageRaphSex.CracksInFloorboards".Localized());

            OutputText("Scenes.Raphael.FollowupToFirstTimeOrphanageRaphSex.FeelLikeWatched".Localized());

            OutputText("Scenes.Raphael.FollowupToFirstTimeOrphanageRaphSex.BeforeYouAskWinks".Localized());

            OutputText("Scenes.Raphael.FollowupToFirstTimeOrphanageRaphSex.RememberWhichRope".Localized());

            OutputText("Scenes.Raphael.FollowupToFirstTimeOrphanageRaphSex.WinkDropsFromWindow".Localized());

            ReturnToCamp(1);
        }

        public void CunnilingusWithRaphael()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.CunnilingusWithRaphael.KissesYouOnLips".Localized());

            OutputText("Scenes.Raphael.CunnilingusWithRaphael.SilverTongue".Localized());

            ShowImage("raphael-sex-cunny");

            OutputText("Scenes.Raphael.CunnilingusWithRaphael.RaisesUpButt".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Raphael.CunnilingusWithRaphael.GoodAtIt".Localized());

            OutputText("Scenes.Raphael.CunnilingusWithRaphael.AfterFiveMinutes".Localized());

            if (Player.Wetness() < 3)
                OutputText("Scenes.Raphael.CunnilingusWithRaphael.CuntFluttering".Localized());
            else
                if (Player.Wetness() < 4)
                    OutputText("Scenes.Raphael.CunnilingusWithRaphael.SmallSpurtFemcum".Localized());
                else
                    OutputText("Scenes.Raphael.CunnilingusWithRaphael.GushingTorrentFemcum".Localized());

            OutputText("Scenes.Raphael.CunnilingusWithRaphael.BacksOffAfterwards".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void GirlOnTopOfRedFoxesOhMy()
        {
            ClearOutput();

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.FoxNotFar".Localized());

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.NotHardToFigure".Localized());

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.UnoMomento".Localized());

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.SwipeTheCondom".Localized(
                Player.VaginaDescript(0)
            ));

            ShowImage("raphael-sex-home");

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.PointTipTowardsFurls".Localized());

            Player.CuntChange(5, true, true, false);

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.CockIsSomethingElse".Localized());

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.DesperateForMore".Localized());

            OutputText("Scenes.Raphael.GirlOnTopOfRedFoxesOhMy.LookingHimInEyes".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void TalkWithRedFoxLooksOutHesASpy()
        {
            ClearOutput();

            ShowImage("raphael-talk");

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.HardToDistract".Localized());

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.TheyDontBotherMe".Localized());

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.JokinglyReferIncubus".Localized());

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.LikeISaid".Localized());

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.AskWhatAbout".Localized());

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.WouldntRecommend".Localized());

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.ValueInModesty".Localized());

            OutputText("Scenes.Raphael.TalkWithRedFoxLooksOutHesASpy.LeansOnOneArm".Localized());

            ClearButtons();

            RaphaelOrphanageSexMenu();

            SetExitButton("Common.Button.Back".Localized());
        }

        public void RaphaelOrphanageSexMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Cunnilingus".Localized(), CunnilingusWithRaphael);

            AddButton(1, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonRideHim".Localized(), GirlOnTopOfRedFoxesOhMy);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

    }
}