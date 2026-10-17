using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Achievements;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<HelFollowerScene>("coc:hel_follower")]
    public class HelFollowerScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.Npcs.HelFollower);

        public string debugName => "Helia";

        public string debugHint => "";

        public override void Entry()
        {

        }

        public void Reset()
        {

            Save.SetInt("lastTimeSex", 0);
        }

        public float HelCapacity()
        {

            return 85;
        }

        public float HelAnalCapacity()
        {

            var anal = 85;

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) >= 1)
                anal += 100;

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) >= 2)
                anal += 300;

            return anal;
        }

        public float HeliaCapacity()
        {

            return HelCapacity();
        }

        public float HeliaAnalCapacity()
        {

            return HelAnalCapacity();
        }

        public int HelAffection(int diff = 0)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.HelAffectionFollower) > 70 && CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.HelAffectionFollower, 70);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelAffectionFollower) < 100 || (CoC.Instance.Flags.Get(GameFlagIds.HelBonusPoints) == 0 && diff < 0))
            {
                CoC.Instance.Flags.Set(GameFlagIds.HelAffectionFollower, CoC.Instance.Flags.Get(GameFlagIds.HelAffectionFollower) + diff);

                if (CoC.Instance.Flags.Get(GameFlagIds.HelAffectionFollower) >= 100)
                    CoC.Instance.Flags.Set(GameFlagIds.HelAffectionFollower, 100);

                if (CoC.Instance.Flags.Get(GameFlagIds.HelAffectionFollower) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.HelAffectionFollower, 0);
            }
            else
                if (SceneInstance<HelScene>.Get().FollowerHel())
                {
                    CoC.Instance.Flags.Set(GameFlagIds.HelAffectionFollower, 100);

                    CoC.Instance.Flags.Set(GameFlagIds.HelBonusPoints, CoC.Instance.Flags.Get(GameFlagIds.HelBonusPoints) + diff * 3);

                    if (diff > 0)
                        if (CoC.Instance.Flags.Get(GameFlagIds.HelBonusPoints) > 150)
                            CoC.Instance.Flags.Set(GameFlagIds.HelBonusPoints, 150);
                        else
                            if ((diff < 0) && (CoC.Instance.Flags.Get(GameFlagIds.HelBonusPoints) < 0))
                                    CoC.Instance.Flags.Set(GameFlagIds.HelBonusPoints, 0);
                }

            return CoC.Instance.Flags.Get(GameFlagIds.HelAffectionFollower);
        }

        public bool IsHeliaBirthday()
        {

            return DateTime.Now.Month == 7 && DateTime.Now.Day >= 24;
        }

        public void HeliaFollowerIntro()
        {

            ShowImage("helia-follower-intro");

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.WakingShake".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.HoodedReveal".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.GroggyQuestion".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.PrivateTalkRequest".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.RisingFromBed".Localized());

            if (Player.Tallness > 84)
                OutputText("Scenes.Helfollower.HeliaFollowerIntro.WaistFragment".Localized());
            else
                OutputText("Scenes.Helfollower.HeliaFollowerIntro.ShouldersFragment".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.LeadsBeyondCamp".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.CrumblingWallRest".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.HeliaConfides".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) > 0)
                OutputText("Scenes.Helfollower.HeliaFollowerIntro.KillWord".Localized());
            else
                OutputText("Scenes.Helfollower.HeliaFollowerIntro.BringDownWord".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.SisterAndHarpyQueen".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.GemSavingsPitch".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerIntro.StartledByOffer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Come2camp".Localized(), ComeToCampHeliaIWantTailInButt);

            AddButton(1, "Common.Button.ICant".Localized(), ICantLetFireButtsRapeMyCampsButt);

            AddButton(2, "Common.Button.Justfriends".Localized(), JustFriendsWithAnalTailWaifu);
        }

        public void ICantLetFireButtsRapeMyCampsButt()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.ICantLetFireButtsRapeMyCampsButt.HeavyHeartRefusal".Localized());

            OutputText("Scenes.Helfollower.ICantLetFireButtsRapeMyCampsButt.WeakSmileReply".Localized());

            OutputText("Scenes.Helfollower.ICantLetFireButtsRapeMyCampsButt.PlayfulShoulderPunch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFoxyFoursomeWarned) > 0 || CoC.Instance.Flags.Get(GameFlagIds.HelEdrynOffer) > 0)
                OutputText("Scenes.Helfollower.ICantLetFireButtsRapeMyCampsButt.UsualFragment".Localized());

            OutputText("Scenes.Helfollower.ICantLetFireButtsRapeMyCampsButt.QualityTimeOffer".Localized());

            OutputText("Scenes.Helfollower.ICantLetFireButtsRapeMyCampsButt.FieryLoverPromise".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaFollowerDisabled, 1);

            Go<CampScene>();
        }

        public void ComeToCampHeliaIWantTailInButt()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.ComeToCampHeliaIWantTailInButt.ChampionDutyRefusal".Localized());

            OutputText("Scenes.Helfollower.ComeToCampHeliaIWantTailInButt.WideEyedShock".Localized());

            OutputText("Scenes.Helfollower.ComeToCampHeliaIWantTailInButt.AllayConcerns".Localized());

            OutputText("Scenes.Helfollower.ComeToCampHeliaIWantTailInButt.ReluctantAgreement".Localized());

            OutputText("Scenes.Helfollower.ComeToCampHeliaIWantTailInButt.AffectionateKiss".Localized());

            OutputText("Scenes.Helfollower.ComeToCampHeliaIWantTailInButt.KissBreakPromise".Localized());

            ClearButtons();

            View.Time.Hours += 1;

            AddButton(0, "Common.Button.Next".Localized(), AfterMoveInBoningAnalFireTail);
        }

        public void AfterMoveInBoningAnalFireTail()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.AfterMoveInBoningAnalFireTail.HourLaterMarker".Localized());

            OutputText("Scenes.Helfollower.AfterMoveInBoningAnalFireTail.TeaseWhileDressing".Localized());

            OutputText("Scenes.Helfollower.AfterMoveInBoningAnalFireTail.WinkAndGrin".Localized());

            OutputText("Scenes.Helfollower.AfterMoveInBoningAnalFireTail.PlayfulSwatFarewell".Localized());

            OutputText("Scenes.Helfollower.AfterMoveInBoningAnalFireTail.LoversMenuNotice".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelFollowerLevel, 2);

            Go<CampScene>();
        }

        public void JustFriendsWithAnalTailWaifu()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.AwkwardLetdown".Localized());

            OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.GraciousAcceptance".Localized());

            OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.MaybeVisitPromise".Localized());

            if (Player.Tallness > 84)
                OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.UpFragment".Localized());
            else
                if (Player.Tallness < 60)
                    OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.DownFragment".Localized());
                else
                    OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.OverFragment".Localized());

            OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.CheekPeck".Localized());

            OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.SureWillReply".Localized());

            OutputText("Scenes.Helfollower.JustFriendsWithAnalTailWaifu.FarewellWave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaFollowerDisabled, .5f);

            ReturnToCamp(1);
        }

        public void HelFollowersIntro()
        {
            ClearOutput();

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha() && CoC.Instance.Flags.Get(GameFlagIds.HelIntrosLevel) < 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.HelIntrosLevel, 1);

                OutputText("Scenes.Helfollower.HelFollowersIntro.AwaitCompanionReturn".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.KihaSmirks".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.AdmitExpectingSomeone".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.TeaseWhoIsIt".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.LookOverShoulder".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.HelArrivesWink".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.KihaDemands".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.PolyamoryTaunt".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.HandsOffDemand".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.HelRollsEyes".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.KihaTakenAback".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.HelOffersHand".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.DragonessYelps".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.SurpriseHug".Localized());

                OutputText("Scenes.Helfollower.HelFollowersIntro.GigglingFarewell".Localized());
            }
            else
                if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower() && CoC.Instance.Flags.Get(GameFlagIds.HelIntrosLevel) < 2)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.HelIntrosLevel, 2);

                    if (CoC.Instance.Flags.Get(GameFlagIds.HelIsabellaThreesomeEnabled) > 0)
                    {
                        OutputText("Scenes.Helfollower.HelFollowersIntro.ShowHelAround".Localized());

                        if (!(CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaGreetsPlain".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaGreetsAccented".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelLeapsOnLap".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.AnnounceHelMovingIn".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Helfollower.HelFollowersIntro.HeliaZisTrueAccented".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.HeliaZisTruePlain".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.SalamanderNods".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Helfollower.HelFollowersIntro.CowWishesMeinHel".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.CowWishesHelPlain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Helfollower.HelFollowersIntro.ShowHelAroundAgain".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.RecallDisdain".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelDrawsScimitar".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaStammersAccented".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaStammersPlain".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.LeapBetweenThem".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.BothBlurtSurprised".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.DemandExplanation".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.BandanaAccusation".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaHuffs".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelDemandsHere".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.CowBrowFurrows".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.ExplainEachMeeting".Localized());

                        if (Player.Corruption < 50)
                            OutputText("Scenes.Helfollower.HelFollowersIntro.GirlsFallSilent".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.GlowerBalefully".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaAsksLoversAccented".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaAsksLoversPlain".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelScoffsApproval".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.DopeySmileShake".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Helfollower.HelFollowersIntro.MilkShakeBowAccented".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.MilkShakeBowPlain".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelSheathesSword".Localized());

                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaFarewellAccent".Localized());
                        else
                            OutputText("Scenes.Helfollower.HelFollowersIntro.IsabellaFarewellPlain".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.HelIsabellaThreesomeEnabled, 1);
                    }
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.HelIntrosLevel) < 3 && SceneInstance<RathazulScene>.Get().FollowerRathazul())
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.HelIntrosLevel, 3);

                        OutputText("Scenes.Helfollower.HelFollowersIntro.VisitRathLab".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.RathYelpsStartled".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelOffersRathHand".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.RathStrugglesUp".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelWinksMovesIn".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.RathCollectRemark".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelBimboMoment".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.RathShoosAway".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.HelRecoilsAway".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.RathAgreesToStill".Localized());

                        OutputText("Scenes.Helfollower.HelFollowersIntro.LeadHelHome".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.HelIntrosLevel) < 4 && SceneInstance<SophieBimboScene>.Get().BimboSophieAtCamp())
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.HelIntrosLevel, 4);

                            OutputText("Scenes.Helfollower.HelFollowersIntro.StringUpHammock".Localized());

                            OutputText("Scenes.Helfollower.HelFollowersIntro.SentencePeriod".Localized());

                            OutputText("Scenes.Helfollower.HelFollowersIntro.SophieGreetsCutie".Localized(
                                Player.Mf("hunk", "babe")
                            ));

                            OutputText("Scenes.Helfollower.HelFollowersIntro.SophieYankedOff".Localized());

                            OutputText("Scenes.Helfollower.HelFollowersIntro.SnapBreakItUp".Localized());

                            OutputText("Scenes.Helfollower.HelFollowersIntro.HelPointsAccusing".Localized());

                            OutputText("Scenes.Helfollower.HelFollowersIntro.SophieOverwhelmed".Localized());

                            OutputText("Scenes.Helfollower.HelFollowersIntro.WeighOptions".Localized());

                            if (Player.Intelligence >= 40 || Player.Intelligence / 5 + Random.Shared.Next(20) + 1 > 10)
                            {
                                OutputText("Scenes.Helfollower.HelFollowersIntro.MaybeMakeItWork".Localized());

                                ClearButtons();

                                AddButton(0, "Common.Button.BootSophie".Localized(), BimboSophieGetsBooted4Firebutt);

                                AddButton(1, "Common.Button.BootHel".Localized(), BootHelOutForBimboSophie);

                                AddButton(2, "Common.Button.WorkItOut".Localized(), WorkItOutWithSophieAndFireTits);

                                return ;
                            }

                            ClearButtons();

                            AddButton(0, "Common.Button.BootSophie".Localized(), BimboSophieGetsBooted4Firebutt);

                            AddButton(1, "Common.Button.BootHel".Localized(), BootHelOutForBimboSophie);

                            return ;
                        }
                        else
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.HelIntrosLevel, 9001);

                            Go<CampScene>();

                            return ;
                        }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), HelFollowersIntro);
        }

        public void BimboSophieGetsBooted4Firebutt()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.LiftSophieUp".Localized());

            OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.CarrySophieOut".Localized());

            OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.SophieWhinesConfused".Localized());

            OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.ExplainToLeave".Localized());

            OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.OrderLeaveNow".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AdultMinotaurOffsprings) >= 3)
            {
                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.MinotaurSonsArrive".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.SonAsksPresent".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.RealizeIdea".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.OfferSophieToBoys".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.BoysGrabSophie".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.WarnTakeCare".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MinoSonsHaveSophie, 1);
            }
            else
            {
                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.SayGoodbye".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.SophieLeapsBack".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.StayCommand".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.PuzzledQuestion".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.StayShout".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.ButProtest".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.StayShoutAgain".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.GestureStayRetreat".Localized());

                OutputText("Scenes.Helfollower.BimboSophieGetsBooted4Firebutt.CurseHel".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.SophieDisabledForever, 1);

            ReturnToCamp(1);
        }

        public void BootHelOutForBimboSophie()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.RefuseScowl".Localized());

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.DeadpanWhat".Localized());

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.OrderGetOut".Localized());

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.ChoseBimboOverMe".Localized());

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.ConfirmGetOut".Localized());

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.BitterFarewell".Localized());

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.HelLeavesCamp".Localized());

            OutputText("Scenes.Helfollower.BootHelOutForBimboSophie.SophieWavesBye".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaFollowerDisabled, 1);

            ReturnToCamp(1);
        }

        public void WorkItOutWithSophieAndFireTits()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.InsistHelDeals".Localized());

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.HarpiesAreEvil".Localized());

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.DefendSophieStupid".Localized());

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.SophieRushesHel".Localized());

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.HelThrowsArms".Localized());

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.RecallBimbofication".Localized());

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.HelSighsResigned".Localized());

            OutputText("Scenes.Helfollower.WorkItOutWithSophieAndFireTits.MaybeTheyGetAlong".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KeepHeliaAndSophie, 1);

            ReturnToCamp(1);
        }

        public void AngryHelAndIzzyCampHelHereFirst()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.ShowingIsabellaTour".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.RememberDisdainMoment".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.HelYellsDropsWhetstone".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaStammersRecovers".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.VLetter".Localized());
            else
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.WLetter".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.WhatDoingHereShout".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.LeapBetweenThem".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.TheyBlurtSurprised".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.InsistOnExplanation".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.HelSaysStoleBandana".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaIndignantHuff".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.HelDemandsAnswer".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.CowGirlBrowFurrows".Localized());
            else
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.CowBrowFurrowsAgain".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.ExplainHowMetEach".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.FallSilentUnderGazes".Localized());
            else
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.GlowerBalefullyChallenge".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaAsksLovers".Localized());
            else
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaAsksLoversAgain".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.HelScoffsAgrees".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.DopeySmileHandshake".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaOffersBlueBow".Localized());
            else
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaOffersBowAgain".Localized());

            OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.HelSheathesSwordThanks".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaSaysVelcome".Localized());
            else
                OutputText("Scenes.Helfollower.AngryHelAndIzzyCampHelHereFirst.IsabellaSaysWelcomeAgain".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelIsabellaThreesomeEnabled, 1);

            ReturnToCamp(1);
        }

        public void HeliaFollowerMenu(bool display = true)
        {
            ClearOutput();

            if (display)
                SceneInstance<HelScene>.Get().SpriteChooser();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) == 2)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTrainingOffered) == 0 && display && Player.BiggestCockArea() > HeliaAnalCapacity())
                {
                    HeliaAnalTrainingPrompt();

                    return ;
                }

                if (display)
                    OutputText("Scenes.Helfollower.HeliaFollowerMenu.CallHelLoverOver".Localized());

                ClearButtons();

                Player.UnlockCodexEntry(GameFlagIds.CodexEntrySalamanders);

                AddButton(0, "Common.Button.Appearance".Localized(), SceneInstance<HelSpawnScene>.Get().HeliasAppearanceScreen);

                AddButton(1, "Common.Button.Talk".Localized(), HeliaOptions);

                AddButton(2, "Common.Button.Sex".Localized(), () => HeliaRoughSex());

                if (!SceneInstance<HelSpawnScene>.Get().HelPregnant())
                    AddButton(3, "Common.Button.Spar".Localized(), SparWithHeliaFirebuttsAreHot);
                else
                    OutputText("Scenes.Helfollower.HeliaFollowerMenu.HeliaNoSparPregnant".Localized());

                if (!CampGuards.IsGuard("Helia"))
                    AddButton(4, "Common.Button.GuardCamp".Localized(), HelGuardToggle);
                else
                    AddButton(4, "Common.Button.StopGuarding".Localized(), HelGuardToggle);

                AddButton(5, "Common.Button.Bathe".Localized(), TakeABath);

                AddButton(6, "Common.Button.Hug".Localized(), HugASmokeyTail);

                AddButton(7, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), HugASmokeyTail);

                if (!SceneInstance<HelSpawnScene>.Get().HelPregnant())
                    AddButton(8, "Common.Button.Box".Localized(), BoxWithInCampHel);

                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helia")
                    AddButton(9, "Common.Button.SleepAlone".Localized(), DontSleepWithHelia);
                else
                    AddButton(9, CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "salamanders" ? "Common.Text.SleepWithout".Localized() : "Common.Button.SleepWith".Localized(), HeliaSleepWith);

                if ((CoC.Instance.Flags.Get(GameFlagIds.HelLove) == 1 || CoC.Instance.Flags.Get(GameFlagIds.HelLove) == -1) && (Player.HasCockThatFits(HeliaCapacity()) && Player.Lust >= 33 && !SceneInstance<HelSpawnScene>.Get().HelPregnant() && CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) == 0))
                        AddRowButton(2, "Have a Kid", SceneInstance<HelSpawnScene>.Get().HaveAKid);

                if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) == 1)
                    AddRowButton(2, SceneInstance<HelSpawnScene>.Get().helspawnName, SceneInstance<HelSpawnScene>.Get().PlayWithYourKid);

                if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTrainingOffered) > 0 && CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) < 2 && Player.BiggestCockArea() > HeliaAnalCapacity() && Player.HasItem(ItemRegistry.Create(ConsumableIds.GoblinAle), 1))
                    AddRowButton(2, "Anal Train", HeliaGapeSceneChoices);

                AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) == 1)
                    if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 1)
                    {
                        if (display)
                            OutputText("Scenes.Helfollower.HeliaFollowerMenu.StorylineCulmination".Localized());
                    }
                    else
                    {
                        if (display)
                        {
                            OutputText("Scenes.Helfollower.HeliaFollowerMenu.ApproachPacingHel".Localized());

                            OutputText("Scenes.Helfollower.HeliaFollowerMenu.ReadyHitTheRoad".Localized());
                        }

                        if ((OptionStore.Instance.Get("hardcore", 0) > 0) && (display))
                                OutputText("Scenes.Helfollower.HeliaFollowerMenu.HardcoreModeWarning".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Dungeon".Localized(), null );

                        AddButton(1, "Common.Button.NotYet".Localized(), null );
                    }
        }

        public void HeliaOptions()
        {

            if (SceneInstance<HelScene>.Get().Pregnancy.Event >= 3 && CoC.Instance.Flags.Get(GameFlagIds.HeliaTalkSeven) == 0)
            {
                SceneInstance<HelSpawnScene>.Get().HeliaTalkSeven();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) == 1 && CoC.Instance.Flags.Get(GameFlagIds.HelTalkEight) == 0)
            {
                SceneInstance<HelSpawnScene>.Get().HeliaTalkEight();

                return ;
            }

            TalkToHel();
        }

        public void DontSleepWithHelia()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.DontSleepWithHelia.DeclineSleepingHelia".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "");

            HeliaFollowerMenu(false);
        }

        public void SparWithHeliaFirebuttsAreHot()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.SparWithHeliaFirebuttsAreHot.PlayfulPunchBattleRequest".Localized());

            OutputText("Scenes.Helfollower.SparWithHeliaFirebuttsAreHot.SparWarningAccepted".Localized());

            OutputText("Scenes.Helfollower.SparWithHeliaFirebuttsAreHot.ReadyWeaponForBattle".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "hel")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

            CurrentMonster.CreateStatusEffect(StatusEffectIds.Sparring, 0, 0, 0, 0);

            CombatController.Instance!.Context.Enemy.Gems = 0;
        }

        public void LoseToSparringHeliaLikeAButtRapedChump()
        {
            ClearOutput();

            if (Player.Health < 1)
                OutputText("Scenes.Helfollower.LoseToSparringHeliaLikeAButtRapedChump.CollapseOverwhelmedPain".Localized());
            else
                OutputText("Scenes.Helfollower.LoseToSparringHeliaLikeAButtRapedChump.ArousalDesperateRelease".Localized());

            OutputText("Scenes.Helfollower.LoseToSparringHeliaLikeAButtRapedChump.HelLoomsRemovingBikini".Localized());

            OutputText("Scenes.Helfollower.LoseToSparringHeliaLikeAButtRapedChump.HelplessLookUpSigh".Localized());

            OutputText("Scenes.Helfollower.LoseToSparringHeliaLikeAButtRapedChump.VictorTakesSpoils".Localized());

            OutputText("Scenes.Helfollower.LoseToSparringHeliaLikeAButtRapedChump.MightAsWellEnjoy".Localized());

            HeliaRoughSex(false);
        }

        public void PCBeatsUpSalamanderSparring()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Helfollower.PCBeatsUpSalamanderSparring.SalamanderCollapsesKnee".Localized());
            else
                OutputText("Scenes.Helfollower.PCBeatsUpSalamanderSparring.PantingKneesCollapse".Localized());

            OutputText("Scenes.Helfollower.PCBeatsUpSalamanderSparring.HelGroansSwaying".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Helfollower.PCBeatsUpSalamanderSparring.KickHarderTaunt".Localized());
            else
                OutputText("Scenes.Helfollower.PCBeatsUpSalamanderSparring.TurnMeOnTaunt".Localized());

            OutputText("Scenes.Helfollower.PCBeatsUpSalamanderSparring.ChuckleOfferHandUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaSparVictories, CoC.Instance.Flags.Get(GameFlagIds.HeliaSparVictories) + 1);

            HeliaRoughSex(false);
        }

        public void TalkToHel()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) == 0)
            {
                OutputText("Scenes.Helfollower.TalkToHel.HairStrokeInvite".Localized());

                OutputText("Scenes.Helfollower.TalkToHel.GrinsToRock".Localized(
                    Player.Mf("bud", "babe")
                ));

                OutputText("Scenes.Helfollower.TalkToHel.SettlingInQuestion".Localized());

                if (SceneInstance<RathazulScene>.Get().FollowerRathazul())
                    OutputText("Scenes.Helfollower.TalkToHel.OldManStillReveal".Localized());
                else
                    OutputText("Scenes.Helfollower.TalkToHel.HomemadeStillReveal".Localized());

                OutputText("Scenes.Helfollower.TalkToHel.FoulSteinPour".Localized());

                OutputText("Scenes.Helfollower.TalkToHel.HardLiquorGulp".Localized());

                if (Player.Toughness < 50)
                    OutputText("Scenes.Helfollower.TalkToHel.BoozeBurnCough".Localized());
                else
                    OutputText("Scenes.Helfollower.TalkToHel.EnduranceGulp".Localized());

                OutputText("Scenes.Helfollower.TalkToHel.ArmAroundBoast".Localized());

                OutputText("Scenes.Helfollower.TalkToHel.HeadNestle".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) == 1)
                {
                    OutputText("Scenes.Helfollower.TalkToHel.SitDownStoryAsk".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.GoodStoriesDeflect".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.FoodRaidTale".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.WallClimbPlan".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.LarderAmbush".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.EscapeGoesWrong".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.DemandStoryFinish".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.SpookyGhostTaunt".Localized());

                    if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                        OutputText("Scenes.Helfollower.TalkToHel.ShouldraInterrupts".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.ShoulderPunch".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.HorrorStoryResumes".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.GnollPackedWalls".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.WhatReaction".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.AlphaGnollQueen".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.QueenSpeaks".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.NakedGnollSurround".Localized());

                    OutputText("Scenes.Helfollower.TalkToHel.KnowWhereThisGoes".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Listen".Localized(), ListenToHelTalkAboutGnolls);

                    AddButton(1, "Common.Button.ShutUp".Localized(), ShutUpHelTalks);

                    return ;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) == 2)
                    {
                        OutputText("Scenes.Helfollower.TalkToHel.SuddenEmbrace".Localized());

                        OutputText("Scenes.Helfollower.TalkToHel.PurringMine".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) == 3)
                        {
                            OutputText("Scenes.Helfollower.TalkToHel.AskForMinutes".Localized());

                            OutputText("Scenes.Helfollower.TalkToHel.GnollHuntOffer".Localized());

                            OutputText("Scenes.Helfollower.TalkToHel.SwordOutStroll".Localized());

                            OutputText("Scenes.Helfollower.TalkToHel.PlainsEmbrace".Localized());

                            OutputText("Scenes.Helfollower.TalkToHel.WaistKiss".Localized());

                            OutputText("Scenes.Helfollower.TalkToHel.NiceWhisper".Localized());

                            OutputText("Scenes.Helfollower.TalkToHel.FaceOverYours".Localized());

                            OutputText("Scenes.Helfollower.TalkToHel.AnotherDeepKiss".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) == 4)
                            {
                                OutputText("Scenes.Helfollower.TalkToHel.ComeHitherOffer".Localized());

                                OutputText("Scenes.Helfollower.TalkToHel.FollowToStill".Localized());

                                OutputText("Scenes.Helfollower.TalkToHel.PourTwoTankards".Localized());

                                if (SceneInstance<HelSpawnScene>.Get().HelPregnant())
                                    OutputText("Scenes.Helfollower.TalkToHel.BeerWord".Localized());
                                else
                                    OutputText("Scenes.Helfollower.TalkToHel.FizzyBeerWord".Localized());

                                OutputText("Scenes.Helfollower.TalkToHel.LapToast".Localized());

                                if (!SceneInstance<HelSpawnScene>.Get().HelPregnant())
                                    OutputText("Scenes.Helfollower.TalkToHel.HerEyesCrossing".Localized());
                                else
                                    OutputText("Scenes.Helfollower.TalkToHel.SentencePeriod".Localized());

                                OutputText("Scenes.Helfollower.TalkToHel.HelLaughs".Localized());

                                if (!SceneInstance<HelSpawnScene>.Get().HelPregnant())
                                    OutputText("Scenes.Helfollower.TalkToHel.DrunkenlyWord".Localized());
                                else
                                    OutputText("Scenes.Helfollower.TalkToHel.HappilyWord".Localized());

                                if (!SceneInstance<HelSpawnScene>.Get().HelPregnant())
                                {
                                    OutputText("Scenes.Helfollower.TalkToHel.RefillAndHop".Localized());

                                    if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                                        OutputText("Scenes.Helfollower.TalkToHel.DistantShout".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.FireballGiggle".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.EncouragedGulp".Localized());

                                    if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                                        OutputText("Scenes.Helfollower.TalkToHel.KihaRockThrow".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.BeaconFlame".Localized());
                                }
                                else
                                    OutputText("Scenes.Helfollower.TalkToHel.LewdJestExchange".Localized());

                                OutputText("Scenes.Helfollower.TalkToHel.LaughPullsYou".Localized());

                                if (Player.Tallness < 60)
                                    OutputText("Scenes.Helfollower.TalkToHel.UpWord".Localized());
                                else
                                    OutputText("Scenes.Helfollower.TalkToHel.DownWord".Localized());

                                OutputText("Scenes.Helfollower.TalkToHel.BosomHug".Localized());

                                OutputText("Scenes.Helfollower.TalkToHel.BrowKiss".Localized());
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) == 5 && Player.Gems >= 1 && SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower() && SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                                {
                                    ShowImage("helia-follower-poker");

                                    var gems = 0;

                                    OutputText("Scenes.Helfollower.TalkToHel.PokerInvitation".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.AskWhatGame".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.PolkaGuess".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.SuggestPoker".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.EagerAccept".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.ShrugFollow".Localized());

                                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                        OutputText("Scenes.Helfollower.TalkToHel.GermanPokerRules".Localized());
                                    else
                                        OutputText("Scenes.Helfollower.TalkToHel.PlainPokerRules".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.KihaConfusedCards".Localized());

                                    if (!(CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                        OutputText("Scenes.Helfollower.TalkToHel.SomewhatInterested".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.SentencePeriodAgain".Localized());

                                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                        OutputText("Scenes.Helfollower.TalkToHel.GermanSillySnap".Localized());
                                    else
                                        OutputText("Scenes.Helfollower.TalkToHel.PlainSillySnap".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.SillyGirlOutrage".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.RumShotCalm".Localized());

                                    OutputText("Scenes.Helfollower.TalkToHel.RedheadsSettled".Localized());

                                    if (Random.Shared.Next(20) + 1 + Player.Intelligence / 5 >= 15)
                                    {
                                        OutputText("Scenes.Helfollower.TalkToHel.GemPilePride".Localized());

                                        OutputText("Scenes.Helfollower.TalkToHel.HelCursesLoss".Localized());

                                        if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                            OutputText("Scenes.Helfollower.TalkToHel.GermanBeginnerMoan".Localized());
                                        else
                                            OutputText("Scenes.Helfollower.TalkToHel.PlainBeginnerMoan".Localized());

                                        OutputText("Scenes.Helfollower.TalkToHel.ShamelessKiha".Localized());

                                        gems = 40 + Random.Shared.Next(40);
                                    }
                                    else
                                        if (Random.Shared.Next(3) == 0)
                                        {
                                            OutputText("Scenes.Helfollower.TalkToHel.BooyahWin".Localized());

                                            OutputText("Scenes.Helfollower.TalkToHel.CheatAccusation".Localized());

                                            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                                OutputText("Scenes.Helfollower.TalkToHel.GermanLuckDraw".Localized());
                                            else
                                                OutputText("Scenes.Helfollower.TalkToHel.PlainLuckDraw".Localized());

                                            OutputText("Scenes.Helfollower.TalkToHel.LiterallyAdd".Localized());

                                            gems = -5 - Random.Shared.Next(10);
                                        }
                                        else
                                            if (Random.Shared.Next(2) == 0)
                                            {
                                                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                                {
                                                    OutputText("Scenes.Helfollower.TalkToHel.GermanMamaIsabella".Localized());

                                                    OutputText("Scenes.Helfollower.TalkToHel.GermanNewSkirt".Localized());
                                                }
                                                else
                                                {
                                                    OutputText("Scenes.Helfollower.TalkToHel.PlainMamaIsabella".Localized());

                                                    OutputText("Scenes.Helfollower.TalkToHel.PlainNewSkirt".Localized());
                                                }

                                                OutputText("Scenes.Helfollower.TalkToHel.MilkmaidScoff".Localized());

                                                OutputText("Scenes.Helfollower.TalkToHel.KihaWhinesFair".Localized());

                                                OutputText("Scenes.Helfollower.TalkToHel.DrinkAwaySorrow".Localized());

                                                gems = -5 - Random.Shared.Next(10);
                                            }
                                            else
                                            {
                                                OutputText("Scenes.Helfollower.TalkToHel.KihaWonDoubt".Localized());

                                                OutputText("Scenes.Helfollower.TalkToHel.WaxOnGrumbles".Localized());

                                                if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                                                    OutputText("Scenes.Helfollower.TalkToHel.GermanRhymeHuff".Localized());
                                                else
                                                    OutputText("Scenes.Helfollower.TalkToHel.PlainRhymeHuff".Localized());

                                                OutputText("Scenes.Helfollower.TalkToHel.KihaDepartsHumming".Localized());

                                                gems = -5 - Random.Shared.Next(10);
                                            }

                                    if (Player.Gems + gems < 0)
                                        gems = Player.Gems;

                                    if (gems < 0)
                                        OutputText("Scenes.Helfollower.TalkToHel.LostGemsMessage".Localized(
                                            gems
                                        ));
                                    else
                                        OutputText("Scenes.Helfollower.TalkToHel.GainedGemsMessage".Localized(
                                            gems
                                        ));

                                    Player.Gems += gems;

                                }
                                else
                                {
                                    CoC.Instance.Flags.Set(GameFlagIds.FollowerHelTalks, 1);

                                    TalkToHel();

                                    return ;
                                }

            CoC.Instance.Flags.Set(GameFlagIds.FollowerHelTalks, CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) + 1);

            ReturnToCamp(1);
        }

        public void ShutUpHelTalks()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.ShutUpHelTalks.HastyInterjection".Localized());

            OutputText("Scenes.Helfollower.ShutUpHelTalks.SmugAgreement".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Helfollower.ShutUpHelTalks.BardApology".Localized());

                OutputText("Scenes.Helfollower.ShutUpHelTalks.LapStraddle".Localized());
            }
            else
                OutputText("Scenes.Helfollower.ShutUpHelTalks.GuiltyExcuse".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerHelTalks, CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) + 1);

            ReturnToCamp(1);
        }

        public void ListenToHelTalkAboutGnolls()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.GnollTaleResumes".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.TankardRefill".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.QueenClitRub".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.FirstScepterEntry".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.ScepterTaunt".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.ShatteredSob".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.TasteBoysOffer".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.SobbingBeg".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.SlutOrgyHours".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.SecondTimeScoff".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.RuefulLaugh".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.DrunkenStoryFinish".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.SlickThighStare".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.IndignantDefense".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.ArousalPuzzle".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.NightmareConfession".Localized());

            OutputText("Scenes.Helfollower.ListenToHelTalkAboutGnolls.WetKissDistraction".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerHelTalks, CoC.Instance.Flags.Get(GameFlagIds.FollowerHelTalks) + 1);

            HeliaRoughSex(false);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void HugASmokeyTail()
        {
            ClearOutput();

            if (View.Time.Hours >= 21)
            {
                OutputText("Scenes.Helfollower.HugASmokeyTail.SunsetHammock".Localized());

                OutputText("Scenes.Helfollower.HugASmokeyTail.EmbraceTurn".Localized());

                OutputText("Scenes.Helfollower.HugASmokeyTail.NippingKisses".Localized());

                if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                    OutputText("Scenes.Helfollower.HugASmokeyTail.LetterS".Localized());

                OutputText("Scenes.Helfollower.HugASmokeyTail.IntertwinedLaugh".Localized());

                OutputText("Scenes.Helfollower.HugASmokeyTail.StolenKiss".Localized());

                OutputText("Scenes.Helfollower.HugASmokeyTail.SweetNightVow".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Helia");

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));

                return ;
            }
            else
                if (Player.Tallness >= 96)
                {
                    OutputText("Scenes.Helfollower.HugASmokeyTail.BridalLift".Localized());

                    OutputText("Scenes.Helfollower.HugASmokeyTail.LanguidSigh".Localized());
                }
                else
                    if (Player.Tallness >= 72)
                    {
                        OutputText("Scenes.Helfollower.HugASmokeyTail.SalamanderLift".Localized());

                        OutputText("Scenes.Helfollower.HugASmokeyTail.TightHugPraise".Localized(
                            Player.Mf("handsome", "beautiful")
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Helfollower.HugASmokeyTail.ReversedSweep".Localized());

                        OutputText("Scenes.Helfollower.HugASmokeyTail.StorybookDip".Localized());

                        OutputText("Scenes.Helfollower.HugASmokeyTail.BosomPraise".Localized());
                    }

            ReturnToCamp(1);
        }

        public void TalkAboutFuta()
        {
            ClearOutput();

        }

        public void SleepyNightMareHel()
        {

            OutputText("Scenes.Helfollower.SleepyNightMareHel.SweatWaking".Localized());

            OutputText("Scenes.Helfollower.SleepyNightMareHel.NightmareMutter".Localized());

            OutputText("Scenes.Helfollower.SleepyNightMareHel.CheekComfort".Localized());
        }

        public void HeliaRoughSex(bool output = true)
        {
            ClearOutput();

            if (output)
                OutputText("Scenes.Helfollower.HeliaRoughSex.BikiniUndo".Localized());

            var buttons = 0;

            ClearButtons();

            if (Player.HasCock() && Player.Lust >= 33)
            {
                if (Player.HasCockThatFits(HeliaCapacity()) && buttons < 14)
                {
                    AddButton(buttons, "Common.Button.Fuckvag".Localized(), SceneInstance<HelScene>.Get().BeatUpHelAndStealHerWalletFromHerVagina);

                    buttons += 1;
                }

                if (Player.HasCockThatFits(HeliaAnalCapacity()) && buttons < 14)
                {
                    AddButton(buttons, "Common.Button.Anal".Localized(), SceneInstance<HelScene>.Get().FuckHelsAss);

                    buttons += 1;
                }

                if (buttons < 14)
                {
                    AddButton(buttons, "Common.Button.GetBlown".Localized(), SceneInstance<HelScene>.Get().HelBlowsYou);

                    buttons += 1;
                }

                if (Player.HasCockThatFits(HeliaCapacity()) && Player.CockThatFits2(HeliaCapacity()) >= 0 && buttons < 14)
                {
                    AddButton(buttons, "Common.Button.Doublepen".Localized(), SceneInstance<HelScene>.Get().DpHel);

                    buttons += 1;
                }

                if (buttons < 14)
                {
                    AddButton(buttons, "Common.Button.TailWank".Localized(), SceneInstance<HelScene>.Get().HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora);

                    buttons += 1;
                }
            }

            if (Player.HasVagina() && Player.Lust >= 33 && buttons < 14)
            {
                AddButton(buttons, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetLicked".Localized(), SceneInstance<HelScene>.Get().GetLickedByHel);

                buttons += 1;
            }

            if (Player.Lust >= 33 && buttons < 14)
            {
                AddButton(buttons, "Common.Button.Tailpeg".Localized(), () => SceneInstance<HelScene>.Instance.HelTailPegging());

                buttons += 1;
            }

            if (Player.Lust >= 33 && Player.IsTaur())
            {
                if ((Player.HasCock()) && (Player.HasCockThatFits(HeliaCapacity()) && buttons < 14))
                    {
                        AddButton(buttons, "Common.Button.MountHer".Localized(), CentaurMountsCampHel);

                        buttons += 1;
                    }

                if (Player.HasKeyItem("Centaur Pole") && Player.HasVagina() && buttons < 14)
                {
                    AddButton(buttons, "Common.Button.Centaurtoy".Localized(), FemtaurPlusCampHel);

                    buttons += 1;
                }
            }

            if (Player.Lust >= 33 && Player.HasPerk(PerkIds.Incorporeality) && SceneInstance<IzmaScene>.Get().IzmaFollower() && CoC.Instance.Flags.Get(GameFlagIds.IzmaNoCock) == 0 && buttons < 14)
            {
                AddButton(buttons, "Common.Button.Possess".Localized(), HeliaCampPossession);

                buttons += 1;
            }

            if (Player.Lust >= 33 && Player.CountCocksOfType("TENTACLE") > 6)
            {
                AddButton(buttons, "Scenes.Kitsune.DefeatTheKitsunes.ButtonTentacles".Localized(), HeliaFollowerTentafuck);

                buttons += 1;
            }

            if (Player.Lust >= 33 && Player.IsNaga())
            {
                if (Player.HasVagina() && buttons < 14)
                {
                    AddButton(buttons, "Common.Button.Nagacoilf".Localized(), NagaCoilForHelCampWithGirls);

                    buttons += 1;
                }

                if ((Player.HasCock() && buttons < 14) && (Player.HasCockThatFits(HeliaCapacity())))
                    {
                        AddButton(buttons, "Common.Button.Nagacoilm".Localized(), InCampHelNagaLuv);

                        buttons += 1;
                    }
            }

            if (Player.HasStatusEffect(StatusEffectIds.Heat) || Player.HasStatusEffect(StatusEffectIds.Rut))
                AddNextButton("Common.Button.LustFuck".Localized(), HeliaLustFuck);

            if (Player.Lust < 33)
                OutputText("Scenes.Helfollower.HeliaRoughSex.ArousalTooLow".Localized());

            if (SceneInstance<HelScene>.Get().FollowerHel())
                AddNextButton("Common.Button.Threesomes".Localized(), HeliaThreesomes);

            if (CombatController.InCombat)
                AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
            else
                AddButton(14, "Common.Button.Back".Localized(), () => HeliaFollowerMenu());
        }

        public void HeliaCampPossession()
        {
            ClearOutput();

            ShowImage("helia-follower-sex-possession");

            OutputText("Scenes.Helfollower.HeliaCampPossession.GhostlyRequest".Localized());

            OutputText("Scenes.Helfollower.HeliaCampPossession.PossessionYelp".Localized());

            OutputText("Scenes.Helfollower.HeliaCampPossession.OverheardComplaint".Localized());

            OutputText("Scenes.Helfollower.HeliaCampPossession.Apology".Localized());

            OutputText("Scenes.Helfollower.HeliaCampPossession.CampPlaymateSearch".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Izma".Localized(), PossessIzma);
        }

        public void PossessIzma()
        {
            ClearOutput();

            ShowImage("helia-follower-sex-possession-izma");

            OutputText("Scenes.Helfollower.PossessIzma.StreamStroll".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.LightCough".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.TigersharkStartle".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.HipSwishApproach".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.WolfishTease".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.StammeringWorry".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.NervousConsent".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.GroaningBeg".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.FirmSqueezeOrder".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.LickCommand".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.ConfusedSputter".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.SilverHairGrab".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.GentleHairStroke".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.InnerGroanPraise".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.PlayfulSlap".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.DesperateBeg".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.FeignedDeafness".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.LoudPlea".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.FeignedDoubt".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.HorrifiedGasp".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.LessonDecided".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.HardSpankImpale".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.QuietDismay".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.StandingSlam".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.ShudderingClimax".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.DefiantDemand".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.CleavageGrip".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.RidingFinish".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.ResumedRiding".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.PantingQuestion".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.CrossEyedThreat".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.MergedReveal".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.AghastGasp".Localized());

            OutputText("Scenes.Helfollower.PossessIzma.TailPushRebuke".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void InCampHelNagaLuv()
        {
            ClearOutput();

            ShowImage("helia-follower-sex-nagamale");

            var x = Player.CockThatFits(HeliaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.SerpentCircle".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.NagaHugBoast".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.WarmEmbrace".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.GenitalReveal".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.SingleThrustFlip".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.TailHugIdea".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.ChestPullRoll".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.HelplessSqueeze".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.EasyClimax".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Helfollower.InCampHelNagaLuv.BallsToken".Localized());
            else
                OutputText("Scenes.Helfollower.InCampHelNagaLuv.LoadWord".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.WombTailSpasm".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.SweetPeck".Localized());

            OutputText("Scenes.Helfollower.InCampHelNagaLuv.DriftingAsleep".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void NagaCoilForHelCampWithGirls()
        {
            ClearOutput();

            ShowImage("helia-follower-sex-nagafemale");

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.CoilingApproach".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.NagaHugPraise".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.CoilChestKiss".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.ImpatientGroan".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.MercyDecision".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.SilentAgreement".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.CervixTailTip".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.WildTailOrgasm".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.CuddledAftermath".Localized());

            OutputText("Scenes.Helfollower.NagaCoilForHelCampWithGirls.SleepyEmbrace".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void CentaurMountsCampHel()
        {
            ClearOutput();

            var x = Player.CockThatFits(HeliaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            ShowImage("helia-follower-sex-mount");

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.TrotAroundLover".Localized());

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.NudgeTowardCamp".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.PatFlank".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (Player.BiggestBreastSize() < 3)
                OutputText("Scenes.Helfollower.CentaurMountsCampHel.BodyTooBig".Localized());
            else
                OutputText("Scenes.Helfollower.CentaurMountsCampHel.TeaseYourself".Localized());

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.SentencePeriod".Localized());

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.SalamanderHandJob".Localized(
                Player.CockHead(x),

                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.BlowjobHalts".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.GuideIntoHer".Localized());

            if (Player.CockArea(x) > HeliaCapacity())
                OutputText("Scenes.Helfollower.CentaurMountsCampHel.CramTheRest".Localized());
            else
                OutputText("Scenes.Helfollower.CentaurMountsCampHel.OfferHer".Localized());

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.SettleDown".Localized());

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.ThwackSound".Localized());

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.RoughTailSlap".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.GritTeethTrot".Localized());

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.HighShriekClimax".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.CentaurMountsCampHel.GaspBreathingHard".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void FemtaurPlusCampHel()
        {
            ClearOutput();

            ShowImage("helia-follower-sex-femtaur");

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.TrotAroundLover".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.SetUpImpPole".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.SmallGasp".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.PauseReveling".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.SultryWhisper".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.ImpGraspHips".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.SecondPlastid".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.DesperateCurse".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.ArchedBackOrgasm".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.HourBeforeCollapse".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.PantEyesRolled".Localized());

            OutputText("Scenes.Helfollower.FemtaurPlusCampHel.PatHerCheek".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void HeliaFollowerTentafuck()
        {
            ClearOutput();

            ShowImage("helia-follower-sex-tentacles");

            OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.FlourishArmorOff".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.GrowlAroundPrick".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.GroanAroundCock".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.FiveCocksAssault".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.BallsToken".Localized());
            else
                OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.WrigglingTentacles".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.EmptyWhiteSlush".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.ContentedYankOut".Localized());

            OutputText("Scenes.Helfollower.HeliaFollowerTentafuck.EyeRollCleanup".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void BoxWithInCampHel()
        {
            ClearOutput();

            ShowImage("helia-follower-boxing");

            OutputText("Scenes.Helfollower.BoxWithInCampHel.AskCombatPractice".Localized());

            OutputText("Scenes.Helfollower.BoxWithInCampHel.PlayfulGrin".Localized());

            OutputText("Scenes.Helfollower.BoxWithInCampHel.BattleStance".Localized());

            Player.ChangeFatigue(5 + Random.Shared.Next(20));

            if (Player.Intelligence / 20 + 1 + Random.Shared.Next(20) >= 13)
            {
                OutputText("Scenes.Helfollower.BoxWithInCampHel.SeeOpening".Localized());

                OutputText("Scenes.Helfollower.BoxWithInCampHel.SeizeVictory".Localized());

                OutputText("Scenes.Helfollower.BoxWithInCampHel.SettleBeneathYou".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, 1f), new DynStat(GameStats.Toughness, 1f));

                HeliaRoughSex(false);

                SetExitButton("Common.Button.Back".Localized());
            }
            else
                if (Player.Strength / 20 + 1 + Random.Shared.Next(20) >= 13)
                {
                    OutputText("Scenes.Helfollower.BoxWithInCampHel.TireOfBattle".Localized());

                    OutputText("Scenes.Helfollower.BoxWithInCampHel.YouWinThisTime".Localized());

                    OutputText("Scenes.Helfollower.BoxWithInCampHel.ClawedHandGrab".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Strength, 2f));

                    HeliaRoughSex(false);

                    SetExitButton("Common.Button.Back".Localized());
                }
                else
                {
                    OutputText("Scenes.Helfollower.BoxWithInCampHel.FendOffBlows".Localized());

                    OutputText("Scenes.Helfollower.BoxWithInCampHel.GoodLoverTaunt".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Toughness, 2f));

                    if ((Player.Lust >= 33 && Player.HasCock() && Random.Shared.Next(2) == 0) && (Player.HasCockThatFits(HelCapacity())))
                        {
                            MuddyLizardFeet();

                            return ;
                        }

                    HeliaRoughSex(false);

                    SetExitButton("Common.Button.Back".Localized());
                }
        }

        public void MuddyLizardFeet()
        {
            ClearOutput();

            ShowImage("helia-follower-muddylizardfeet");

            OutputText("Scenes.Helfollower.MuddyLizardFeet.SuddenLunge".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.SoleOnChest".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.ShiftWeightDown".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.ClutchBetweenToes".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Helfollower.MuddyLizardFeet.HeelCaressSack".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Helfollower.MuddyLizardFeet.HeelBrushNub".Localized());
                else
                    OutputText("Scenes.Helfollower.MuddyLizardFeet.HeelBrushTaint".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.GroanLustily".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.AlmostBust".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.MoanSqueezeChest".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.LegsWrapAround".Localized());

            if (Player.BiggestCockArea() > HeliaCapacity())
                OutputText("Scenes.Helfollower.MuddyLizardFeet.AgainstCervixLips".Localized());
            else
                OutputText("Scenes.Helfollower.MuddyLizardFeet.GroinMeets".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.BeforeWithdraw".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.StrokeHerCheek".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.RougherPace".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.GripRollOver".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.WinkOverShoulder".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.HandsToBreasts".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.TitFuckCry".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.FaceDownMud".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.FingersThroughHair".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.NotYetGroan".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.GrinDown".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.DaintyKiss".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.LoveThisCock".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.StillNotYet".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.MudStainedTresses".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.TonguesEntwined".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.SpeakYourName".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.HoldTightlySeed".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.OrgasmSubsides".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.GentleQuickKiss".Localized());

            OutputText("Scenes.Helfollower.MuddyLizardFeet.LovingEmbrace".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void TakeABath()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelCanSwim) == 0)
            {
                OutputText("Scenes.Helfollower.TakeABath.SmellsOfBeer".Localized());

                OutputText("Scenes.Helfollower.TakeABath.SlumpShoulders".Localized());

                OutputText("Scenes.Helfollower.TakeABath.DontFeelLikeIt".Localized());

                OutputText("Scenes.Helfollower.TakeABath.DropOrPush".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.DropIt".Localized(), () => HeliaFollowerMenu());

                AddButton(1, "Common.Button.PushIt".Localized(), PushHelToSwim);
            }
            else
            {
                ShowImage("helia-swim");

                OutputText("Scenes.Helfollower.TakeABath.RuffleHair".Localized());

                OutputText("Scenes.Helfollower.TakeABath.BrightSmileAlways".Localized());

                OutputText("Scenes.Helfollower.TakeABath.ArriveAtRiverbank".Localized());

                Player.HasGottenWashed();

                OutputText("Scenes.Helfollower.TakeABath.PlayfulSplashing".Localized());

                OutputText("Scenes.Helfollower.TakeABath.NuzzleAgainstYou".Localized());

                if (Player.Armor.LongName == "goo armor" && Player.Lust >= 33 && 9999 == 0)
                {
                    OutputText("Scenes.Helfollower.TakeABath.CuddleInRiver".Localized());

                    OutputText("Scenes.Helfollower.TakeABath.LeapToFeet".Localized());

                    OutputText("Scenes.Helfollower.TakeABath.SinkBackInWater".Localized());

                    OutputText("Scenes.Helfollower.TakeABath.ArmorGooReplies".Localized());

                    OutputText("Scenes.Helfollower.TakeABath.CupGooBreasts".Localized());

                    OutputText("Scenes.Helfollower.TakeABath.BothGirlsHere".Localized());

                    ClearButtons();
                }
                else
                {
                    OutputText("Scenes.Helfollower.TakeABath.CuddleEventually".Localized());

                    ReturnToCamp(1);
                }
            }
        }

        public void PushHelToSwim()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.PushHelToSwim.FishForAnswers".Localized());

            OutputText("Scenes.Helfollower.PushHelToSwim.SnapsAtYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TeachHer".Localized(), TeachHelToSwim);

            AddButton(1, "Common.Button.DropIt".Localized(), () => HeliaFollowerMenu());
        }

        public void TeachHelToSwim()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.TeachHelToSwim.OfferingSwimLesson".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.HelWhinesShrinking".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.CoaxingHerToTry".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.ArrivingAtRiverbed".Localized());

            if (SceneInstance<IzmaScene>.Get().IzmaFollower())
            {
                OutputText("Scenes.Helfollower.TeachHelToSwim.RedFinAppears".Localized());

                OutputText("Scenes.Helfollower.TeachHelToSwim.HelBlushesBrighter".Localized());
            }
            else
                OutputText("Scenes.Helfollower.TeachHelToSwim.FirebuttWorry".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.NudgingHelBack".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.SteadyingHerShoulders".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.HelLaughsWithGrin".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.CongratulatorySplash".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.SplashingEachOther".Localized());

            OutputText("Scenes.Helfollower.TeachHelToSwim.SwimmingUnlocked".Localized());

            Player.HasGottenWashed();

            CoC.Instance.Flags.Set(GameFlagIds.HelCanSwim, 1);

            ReturnToCamp(1);
        }

        public void HeliaThreesomes()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaThreesomes.ArmAroundHelWaist".Localized());

            OutputText("Scenes.Helfollower.HeliaThreesomes.SerpentineTongueFlick".Localized());

            ClearButtons();

            if (SceneInstance<VapulaScene>.Get().VapulaSlave() && Player.Lust >= 33 && Player.HasCock())
                if (Player.HasCockThatFits(HeliaCapacity()))
                    AddButton(1, "Enemies.VapulaFight.Name".Localized(), HeliaAndVapula);
                else
                    OutputText("Scenes.Helfollower.HeliaThreesomes.CannotFitCock".Localized());

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha() && Player.Lust >= 33 && (int)Player.Gender > 0)
                AddButton(0, "Common.Button.Kiha".Localized(), CampHelAndKihaThreeSome);

            if (SceneInstance<ValeriaScene>.Get().ValeriaFollower() && Player.Lust >= 33)
                AddButton(2, "Common.Button.Valeria".Localized(), HelAndValeriaCampThreesomes);

            if (Player.Lust >= 33 && SceneInstance<SophieBimboScene>.Get().BimboSophie())
                AddButton(3, "Common.Button.Sophie".Localized(), HelAndSluttyHarpy);

            if (Player.Lust >= 33 && SceneInstance<EmberScene>.Get().FollowerEmber())
                AddNextButton("Enemies.Ember.Name".Localized(), HelEmberThreesome);

            if (Player.Lust < 33)
                OutputText("Scenes.Helfollower.HeliaThreesomes.NotHornyEnough".Localized());

            AddButton(14, "Common.Button.Back".Localized(), () => HeliaRoughSex());
        }

        public void HeliaAndVapula()
        {
            ClearOutput();

            var x = Player.CockThatFits(HeliaCapacity());

            var y = Player.CockThatFits2(HeliaCapacity());

            OutputText("Scenes.Helfollower.HeliaAndVapula.HeadingToSuccubus".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.GreetedByVapula".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.VapulaQuestionsVisit".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.AssSlapJiggles".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.MomentOfDoubt".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.AnotherSlapReminder".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.HandsWanderingBody".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.HeliaAndVapula.WhisperAboutTail".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.TailMovesToPucker".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.VapulaPiercingShriek".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.MightyThrustClaim".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.RuthlessPlowing".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Helfollower.HeliaAndVapula.HandDartingToGroin".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Helfollower.HeliaAndVapula.PoundingHarder".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.OrgyNearingClimax".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 750)
                OutputText("Scenes.Helfollower.HeliaAndVapula.BellySwellsAndSpurts".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.JunkPalpitates".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.ArdentKissEach".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.HeliaAndVapula.ReptileGirlCompliments".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.VapulaStammers".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.PromisedAnalReward".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.HelLiftsTailSlowly".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.CumSmellWakesVapula".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.GirlsEatingEachOther".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Helfollower.HeliaAndVapula.AreToken".Localized());
            else
                OutputText("Scenes.Helfollower.HeliaAndVapula.GroinIsToken".Localized());

            OutputText("Scenes.Helfollower.HeliaAndVapula.FillingWithSpunk".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.HeliaAndVapula.HellGirlAndReptile".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.HeliaAndVapula.AnalEmbraceNoBounds".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.HeliaAndVapula.KeepingCockBuried".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helfollower.HeliaAndVapula.HelpingHerToFeet".Localized());

            if (!SceneInstance<HelScene>.Get().FollowerHel())
            {
                OutputText("Scenes.Helfollower.HeliaAndVapula.KissAndAskReturn".Localized());

                OutputText("Scenes.Helfollower.HeliaAndVapula.AssuresSelfCare".Localized());
            }

            Player.Orgasm("Generic");

            Save.SetInt("lastTimeSex", View.Time.Days);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 1f));

            CombatScene.CleanupAfterCombat();
        }

        public void CampHelAndKihaThreeSome()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.SearchingForKiha".Localized());

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.HelTapsKihaShoulder".Localized());

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.RedArmsGrabKiha".Localized());

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.HelLicksKihaNape".Localized());

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.KihaBreaksFree".Localized());

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.TwoSetsOfScales".Localized());

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.KihaEyebrowArched".Localized());

            OutputText("Scenes.Helfollower.CampHelAndKihaThreeSome.DragonProwessOffer".Localized());

            ClearButtons();

            if (Player.Gender == Gender.Male)
                AddButton(0, "Common.Button.Next".Localized(), DudeHeliaAndKihaThreeSome);
            else
                if (Player.Gender == Gender.Female)
                    AddButton(0, "Common.Button.Next".Localized(), GirlsThreesomeHelAndKiha);
                else
                {
                    AddButton(0, "Scenes.Aiko.AikoE1SexPart.ButtonMale".Localized(), DudeHeliaAndKihaThreeSome);

                    AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), GirlsThreesomeHelAndKiha);
                }
        }

        public void GirlsThreesomeHelAndKiha()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.UndressingInWind".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.KihaGripesAss".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.HelThoughtSaving".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.GrabBothGirlsHair".Localized());

            if (Player.LactationQ() >= 50)
                OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.MilkStartsFlowing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.TailDeviatesToAss".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.SilentNodAgreement".Localized());

            Player.ButtChange(20, true, true, false);

            Player.CuntChange(20, true, true, false);

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.PlayfulElbowBump".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.CervixBreachedScream".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.FemcumTorrent".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.KihaHooksHelNeck".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.HelYelpsLeggo".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.GrabKihaHorns".Localized());

            OutputText("Scenes.Helfollower.GirlsThreesomeHelAndKiha.PullingOutLimpTails".Localized());

            Player.Orgasm("Vaginal");

            Player.Orgasm("Anal", false);

            Save.SetInt("lastTimeSex", View.Time.Days);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CombatScene.CleanupAfterCombat();
        }

        public void DudeHeliaAndKihaThreeSome()
        {
            ClearOutput();

            ShowImage("helia-follower-threesome-kiha");

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.PushingGirlsToKnees".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.TongueCoilsShaft".Localized());
            else
                OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.SecondErectionStrokes".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.SentencePeriod".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.PleasuredGroanEscapes".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.SToken".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.ExpertLickingSucking".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.KihaSnapsAtFirebutt".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.HelLoomsOverYou".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.KihaInsultedLizard".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.HelCursesKiha".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.FootOnCockhead".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.KihaKissesNape".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.HelGroansAtKiss".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.GraveLookSoftens".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.KihaSnapsSaveSome".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.CockGoesLimp".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.NotALizardGrowl".Localized());

            OutputText("Scenes.Helfollower.DudeHeliaAndKihaThreeSome.FirebuttScream".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            CombatScene.CleanupAfterCombat();
        }

        public void HelAndValeriaCampThreesomes()
        {
            ClearOutput();

            if (Player.Armor.LongName == "goo armor")
                OutputText("Scenes.Helfollower.HelAndValeriaCampThreesomes.WinkingAtClothing".Localized());
            else
                OutputText("Scenes.Helfollower.HelAndValeriaCampThreesomes.WhistlingForValeria".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), HelXValeriaFemalePC);
        }

        public void HelXValeriaFemalePC()
        {
            ClearOutput();

            if (Player.HasVagina())
                ShowImage("helia-follower-threesome-valeria-female");
            else
                ShowImage("helia-follower-threesome-valeria-male");

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.BlinkSlitherWrap".Localized());

            var partArray = new List<string>
            {
                "[butt]"
            };

            if (Player.HasCock())
                partArray.Add("your [cock]");

            if (Player.HasVagina())
                partArray.Add("and the slick slit of your cunt");

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.GooHelBendsYou".Localized(
                string.Join(", ", partArray),

                Player.HasVagina() ? "[clit]" : Player.HasCock() ? "Common.Text.Cock".Localized() : "Common.Text.Butt".Localized()
            ));

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.GooCocksApproach".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.CoolTipDemands".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.TeethGritRelax".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.PauseToAdjust".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.KnotArrives".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.KnotStrainsHole".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.FingersDigStone".Localized());

            Player.ButtChange(30, true, true, false);

            if (Player.HasVagina())
                Player.CuntChange(20, true, true, false);

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.WetPopScreams".Localized());

            Player.SlimeFeed();

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.GooFloodsBelly".Localized(
                Player.HasCock() ? (Player.HasVagina() ? "r" : "R") + "opes of jism shoot from your [cock]" : ""
            ));

            var fluidArray = new List<string>
            {
                "goo"
            };

            if (Player.HasCock())
                fluidArray.Add("cum");

            if (Player.HasVagina())
                fluidArray.Add("femcum");

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.OrgasmSubsides".Localized(
                string.Join(", ", fluidArray),

                Player.HasCock() ? (Player.HasVagina() ? "Common.Text.CumStillDripsFromYourCock".Localized() : "Common.Text.YourCumStillDrippingFromYour".Localized()) : ""
            ));

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.HelKnotGroan".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.FullExperience".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.HelRipsGoo".Localized());

            OutputText("Scenes.Helfollower.HelXValeriaFemalePC.AllFoursShift".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            CombatScene.CleanupAfterCombat();
        }

        public void HelEmberThreesome()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HelEmberThreesome.IdeaAskEmber".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.SearchCampDragon".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.ExplainProposal".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.EmberSniffRefusal".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.EmberTurnsAway".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.HelCatchesOn".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.EmberSwoopsIn".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.HandleItAlone".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.EmberNearlyBursts".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome.ShowGoodTime".Localized());

            HelEmberThreesome2();
        }

        public void HelEmberThreesome2()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HelEmberThreesome2.LoversQuarrel".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.NeitherWillBudge".Localized(
                SceneInstance<EmberScene>.Get().EmberMF("of them", "girl")
            ));

            OutputText("Scenes.Helfollower.HelEmberThreesome2.ThighSlideEntry".Localized(
                SceneInstance<EmberScene>.Get().EmberMF("strong", "[if (littleember) {developing|bountiful}]")
            ));

            OutputText("Scenes.Helfollower.HelEmberThreesome2.HeliaDoesMoving".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.EmberGaspsClaws".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.DareLizardTaunt".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.RetortSqueak".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.StimulationRises".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.EmberWorksBase".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.VersatileMassage".Localized(
                SceneInstance<EmberScene>.Get().EmberMF("boy", "girl")
            ));

            OutputText("Scenes.Helfollower.HelEmberThreesome2.FirstSpurtCry".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.HeliaKissPalms".Localized(
                Player.LowMedHighCum("filling", "flooding", "overfilling"),

                Player.LowMedHighCum("leaks out around you", "flows from it in rivulets", "spills out in copious amounts"),

                SceneInstance<EmberScene>.Get().EmberMF("of them", "girl")
            ));

            OutputText("Scenes.Helfollower.HelEmberThreesome2.HeapCollapse".Localized());

            OutputText("Scenes.Helfollower.HelEmberThreesome2.DriftPeaceful".Localized());

            Player.Orgasm("Generic");

            Save.SetInt("lastTimeSex", View.Time.Days);

            CombatScene.CleanupAfterCombat();
        }

        public void HelGuardToggle()
        {
            ClearOutput();

            if (!CampGuards.IsGuard("Helia"))
            {
                OutputText("Scenes.Helfollower.HelGuardToggle.AskWatchNight".Localized());

                OutputText("Scenes.Helfollower.HelGuardToggle.GotYourBack".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HelGuarding, 1);
            }
            else
            {
                OutputText("Scenes.Helfollower.HelGuardToggle.DismissWatch".Localized());

                OutputText("Scenes.Helfollower.HelGuardToggle.PartyAtNight".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HelGuarding, 0);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => HeliaFollowerMenu());
        }

        public void HeliaAnalTrainingPrompt()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HeliaAnalTrainingOffered, 1);

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.DangerousGlintAsk".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.ShrugAgree".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.NymphoSaunters".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.BosomLoomsOver".Localized());
            else
                if (Player.Tallness < 72)
                    OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.ToweringAbove".Localized());
                else
                    if (Player.Tallness < 90)
                        OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.EyeToEye".Localized());
                    else
                        OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.LooksUpConfident".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.BoldReachSqueeze".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.BiggestWord".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.MonsterMassage".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.WinkKnowingly".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPrompt.TrainingUnlocked".Localized());

            HeliaFollowerMenu(false);
        }

        public void GiveHeliaAnalTraining()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.HandGoblinAle".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.BerserkerWink".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.TailUnwraps".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.SighSinksLower".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.LegsSpreadWide".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.DrunkSlurPumps".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.HandsFrenzy".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.LewdLickCall".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.StripToFree".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.SelectHeaviest".Localized());
            else
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.GrabStrokeNear".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.AdmireHeft".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.ExcitementBuilds".Localized());

            if (Player.Tallness < 72)
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.BigWord".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.WorshipFromBelow".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.StretchedThrashes".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.RaggedCheer".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.ChinLather".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.PalmsUrethra".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.WigglesStuffed".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.LiftYourNuts".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.EruptSteamy".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.RunsOutWhite".Localized());
            else
                if (Player.CumQ() < 2000)
                    OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.RopeAfterRope".Localized());
                else
                {
                    OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.IvoryWaves".Localized());

                    if (Player.CumQ() >= 3000)
                        OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.CascadingWaves".Localized());

                    OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.LicksHandsClean".Localized());
                }

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.CheersStands".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaAnalTraining, 1);

            if (Player.BiggestCockArea() < HelAnalCapacity())
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.MaybeHandleYou".Localized(
                    Player.Mf("stud", "breeder")
                ));
            else
                OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.TrySomethingBigger".Localized());

            OutputText("Scenes.Helfollower.GiveHeliaAnalTraining.SayEnd".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoblinAle), 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void HeliaGapeSceneChoices()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) == 0)
                GiveHeliaAnalTraining();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) == 1)
                    HeliaAnalTrainingPartTwo();
        }

        public void HeliaAnalTrainingPartTwo()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoblinAle), 1);

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPartTwo.DangleAle".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPartTwo.ClinkBottles".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPartTwo.ToyFlopsOver".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPartTwo.DoubleTakeDildo".Localized());

            OutputText("Scenes.Helfollower.HeliaAnalTrainingPartTwo.StopHeliaPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.AnalTrain".Localized(), YesMakeHeliaAGapeSlut);

            AddButton(1, "Common.Button.Stop".Localized(), NoGapeSlutForHelia);
        }

        public void NoGapeSlutForHelia()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.NoGapeSlutForHelia.StepUpKiss".Localized());

            OutputText("Scenes.Helfollower.NoGapeSlutForHelia.SkepticalAsk".Localized());

            OutputText("Scenes.Helfollower.NoGapeSlutForHelia.DroppedYourCall".Localized());

            ReturnToCamp(1);
        }

        public void YesMakeHeliaAGapeSlut()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HeliaAnalTraining, 2);

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.HeliaHooksFingerArmor".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.LizardwomanPrancesOnBoulders".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.ScaleyWomanDeclaresMoment".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.HeliaGroansRelaxes".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.HeliaTweaksHerNipples".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.TonedSkinDeformsBone".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.LoverEyelidsSinkLower".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.GoblinContraptionSpinsUp".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.JackOffHungryStrokes".Localized(
                Player.Mf("stud", "'stud'")
            ));

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.StaggeringUpStillStroking".Localized());

            if (Player.Strength < 50)
                OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.StrainTo".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.HeftItUpPressing".Localized(
                NumberUtils.Num2Text(Player.Cocks[Player.BiggestCockIndex()].Length)
            ));

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.SighAsPressureRelieved".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.IgnoringTheSpunkSpill".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.SluttySalamanderPullsOff".Localized());

            if (Player.CumQ() < 2000)
                OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.SqueezesLastDollops".Localized());
            else
                OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.AllowsYouToHose".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.HeliaBurpsShoosAway".Localized());

            OutputText("Scenes.Helfollower.YesMakeHeliaAGapeSlut.HeliaCallsAfterYou".Localized());

            Player.Orgasm("Dick");

            Save.SetInt("lastTimeSex", View.Time.Days);

            ReturnToCamp(1);
        }

        public void HeliasBirthday()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliasBirthday.StrangestThingInCamp".Localized());

            OutputText("Scenes.Helfollower.HeliasBirthday.SlipsIntoYourArms".Localized());

            OutputText("Scenes.Helfollower.HeliasBirthday.LoverBodyHeatPressed".Localized());

            OutputText("Scenes.Helfollower.HeliasBirthday.HelRollsEyesDramatic".Localized());

            OutputText("Scenes.Helfollower.HeliasBirthday.CantRecallInvitation".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaBirthdayOffered, 1);

            ClearButtons();

            AddButton(1, "Common.Button.StayHome".Localized(), StayHomeFromHeliaParty);

            AddButton(0, "Common.Button.GoToParty".Localized(), GoWithHelia);
        }

        public void StayHomeFromHeliaParty()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.StayHomeFromHeliaParty.HelGroansSighsChuckles".Localized());

            Go<CampScene>();
        }

        public void HelPartyMenu()
        {

            AwardAchievement(AchievementIds.HolidayHeliaBirthday, false);

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaBdayDrinks) == 0)
                AddNextButton("Common.Button.Drinks".Localized(), HeliaDrinks);

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaBdayHakonAndKiri) == 0)
                AddNextButton("Common.Button.HakonNKiri".Localized(), HeliaHakonAndKiri);

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaBdayPhoenixes) == 0)
                AddNextButton("Common.Button.Phoenixes".Localized(), HeliaPhoenixes);

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaBdayFoxTwins) == 0)
                AddNextButton("Common.Button.FoxTwins".Localized(), HeliaAndFoxTwins);

            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaBdayFoxTwins) > 0)
                AddNextButton("Common.Button.LeaveWGirls".Localized(), LeaveWithGirls);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveWithoutSex);
        }

        public void GoWithHelia()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.GoWithHelia.OfferArmToHel".Localized());

            if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower() || SceneInstance<KihaFollowerScene>.Get().FollowerKiha() || CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1)
            {
                OutputText("Scenes.Helfollower.GoWithHelia.OverShoulderCampEmpty".Localized());

                if (CampScene.CompanionsCount() > 1)
                    OutputText("Scenes.Helfollower.GoWithHelia.SAreSuffix".Localized());
                else
                    OutputText("Scenes.Helfollower.GoWithHelia.IsWord".Localized());

                OutputText("Scenes.Helfollower.GoWithHelia.OutTooYouShrug".Localized());
            }

            OutputText("Scenes.Helfollower.GoWithHelia.MarethShiftingGeography".Localized());

            if (Player.Tallness < 84)
                OutputText("Scenes.Helfollower.GoWithHelia.SweepingYouOffFeet".Localized());
            else
                OutputText("Scenes.Helfollower.GoWithHelia.HangingOffYou".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.ApproachGreatGates".Localized(
                GameSettings.UrtaDisabled ? "Common.Text.TheCaptainCurledUpUnderA".Localized() : "Common.Text.UrtaTheGray".Localized() + (noFur ? "Common.Text.HairedFoxGirl".Localized() : "Common.Text.CoatedFox".Localized()) + " curled up under a small awning beside the gate with a huge canteen resting beside her. Her centauress partner is noticeably missing, but Urta still gives you a wave and a grin you can't quite read."
            ));

            OutputText("Scenes.Helfollower.GoWithHelia.PushesOpenDoor".Localized(
                GameSettings.UrtaDisabled ? Player.Mf("cutie", "girl") + ",] he says with a wink" : "cutie,] she says with a wink"
            ));

            OutputText("Scenes.Helfollower.GoWithHelia.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.UrtaMetHel) == 1)
                OutputText("Scenes.Helfollower.GoWithHelia.QuickLookSharedHistory".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.GivesSmallNod".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.HelGrinsAtAttention".Localized());

            if (Player.Tallness <= 72)
                OutputText("Scenes.Helfollower.GoWithHelia.HeadRestingOnShoulder".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.QuietThanksSqueezingArm".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.HelStiflesLaugh".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.HeartSkipsABeat".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.HelJumpsIntoArms".Localized());

            var list = new List<string> { "Edryn" };

            if (CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1)
                list.Add("Valeria");

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha())
                list.Add("Kiha");

            if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                list.Add("Isabella");

            list.Add("a handful of her wide-hipped and winged half-sisters, the phoenixes");

            OutputText("Scenes.Helfollower.GoWithHelia.GrinsAtHeliaSurprise".Localized(
                string.Join(", ", list)
            ));

            OutputText("Scenes.Helfollower.GoWithHelia.SalamanderSmiles".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.KiriFluttersOnWings".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) > 0)
                OutputText("Scenes.Helfollower.GoWithHelia.ForgaveQueenDeath".Localized());
            else
                OutputText("Scenes.Helfollower.GoWithHelia.WantedToBeFriends".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.SortOfBecameThing".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.HelSqueezesLittleHarpy".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.KiriSlipsFromCrowd".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) > 0)
                OutputText("Scenes.Helfollower.GoWithHelia.FinallyKillingBitchQueen".Localized());
            else
                OutputText("Scenes.Helfollower.GoWithHelia.SparedQueenForLethice".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.NodsAtHisThanks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FreedVala) == 1)
                OutputText("Scenes.Helfollower.GoWithHelia.WaitressFaerieSuppliedIcing".Localized());
            else
                OutputText("Scenes.Helfollower.GoWithHelia.MinotaurChefSuppliedIcing".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.HelHandedOversizedKnife".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.HakonGivesLittlePush".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.PausesTakingLongMoment".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.ThenCakeExplodes".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.YouAndHelYelp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFoxyFoursomeWarned) == 1)
                OutputText("Scenes.Helfollower.GoWithHelia.LovelyFoxEaredHerms".Localized());
            else
                OutputText("Scenes.Helfollower.GoWithHelia.MikoAndMai".Localized());

            OutputText("Scenes.Helfollower.GoWithHelia.TwinsBurstOutPasties".Localized());

            HelPartyMenu();
        }

        public void HeliaDrinks()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaDrinks.SaunterUpToBar".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Helfollower.HeliaDrinks.JustAsYouDo".Localized());

            OutputText("Scenes.Helfollower.HeliaDrinks.PheromoneCloudMounting".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaBdayDrinks, 1);

            HelPartyMenu();
        }

        public void HeliaHakonAndKiri()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaHakonAndKiri.MeanderToVeteranSalamander".Localized());

            OutputText("Scenes.Helfollower.HeliaHakonAndKiri.HakonClapsShoulder".Localized(
                Player.Mf("son", "kid")
            ));

            OutputText("Scenes.Helfollower.HeliaHakonAndKiri.FalterAtLoversFather".Localized());

            OutputText("Scenes.Helfollower.HeliaHakonAndKiri.KiriBlushesBashfulGlance".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaBdayHakonAndKiri, 1);

            HelPartyMenu();
        }

        public void HeliaPhoenixes()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaPhoenixes.ApproachPhoenixSquad".Localized());

            OutputText("Scenes.Helfollower.HeliaPhoenixes.LeaderSnapsSalute".Localized());

            OutputText("Scenes.Helfollower.HeliaPhoenixes.AskWhyTheyreHere".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) > 0)
                OutputText("Scenes.Helfollower.HeliaPhoenixes.QueenDead".Localized());
            else
                OutputText("Scenes.Helfollower.HeliaPhoenixes.QueenDeposed".Localized());

            OutputText("Scenes.Helfollower.HeliaPhoenixes.ThoughtGoodToKnow".Localized());

            OutputText("Scenes.Helfollower.HeliaPhoenixes.GlancesTowardEdryn".Localized());

            if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha() && SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
                OutputText("Scenes.Helfollower.HeliaPhoenixes.ThenDragonessAndCowgirl".Localized());

            OutputText("Scenes.Helfollower.HeliaPhoenixes.FigureWhatTheyMean".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaBdayPhoenixes, 1);

            HelPartyMenu();
        }

        public void HeliaAndFoxTwins()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaAndFoxTwins.ApproachFoxyStrippers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFoxyFoursomeWarned) == 1)
                OutputText("Scenes.Helfollower.HeliaAndFoxTwins.Mai".Localized());
            else
                OutputText("Scenes.Helfollower.HeliaAndFoxTwins.OneOfTheFoxes".Localized());

            OutputText("Scenes.Helfollower.HeliaAndFoxTwins.HelPlantsSerpentKiss".Localized());

            OutputText("Scenes.Helfollower.HeliaAndFoxTwins.HelLaughsSpreading".Localized());

            OutputText("Scenes.Helfollower.HeliaAndFoxTwins.NodToLover".Localized());

            OutputText("Scenes.Helfollower.HeliaAndFoxTwins.FoxGirlsJealousHuff".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HeliaBdayFoxTwins, 1);

            HelPartyMenu();
        }

        public void LeaveWithoutSex()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.LeaveWithoutSex.DecideToLeaveParty".Localized());

            OutputText("Scenes.Helfollower.LeaveWithoutSex.KnowsWhoSheWants".Localized());

            OutputText("Scenes.Helfollower.LeaveWithoutSex.AnnouncesThanks".Localized());

            OutputText("Scenes.Helfollower.LeaveWithoutSex.MurmurOfApproval".Localized());

            OutputText("Scenes.Helfollower.LeaveWithoutSex.DoorHoldsYouClose".Localized());

            ReturnToCamp(1);
        }

        public void LeaveWithGirls()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.LeaveWithGirls.DecideToLeaveParty".Localized());

            OutputText("Scenes.Helfollower.LeaveWithGirls.KnowsWhoSheWants".Localized());

            OutputText("Scenes.Helfollower.LeaveWithGirls.AnnouncesThanks".Localized());

            OutputText("Scenes.Helfollower.LeaveWithGirls.MurmurOfApproval".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.AsMale".Localized(), SceneInstance<HelScene>.Get().FoxyFluffsFoursomeAsMale);
            else
                AddButton(0, "Common.Button.AsMale".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.AsFemale".Localized(), SceneInstance<HelScene>.Get().FoxyFluffGirlsFuckSex);
            else
                AddButton(1, "Common.Button.AsFemale".Localized(), null);
        }

        public void HelAndSluttyHarpy()
        {

            ShowImage("hel-follower-threesome-sophie");

            if (!Player.HasCock() && !Player.HasVagina())
            {
                HelAndSluttyHarpyGenderless();

                return ;
            }
            else
                if (Player.HasCock() && !Player.HasVagina())
                {
                    HelAndSluttyHarpyMale();

                    return ;
                }
                else
                    if (!Player.HasCock() && Player.HasVagina())
                    {
                        HelAndSluttyHarpyFemale();

                        return ;
                    }

            ClearButtons();

            AddButton(0, "Common.Button.Cock".Localized(), HelAndSluttyHarpyMale);

            AddButton(1, "Common.Button.Pussy".Localized(), HelAndSluttyHarpyFemale);

            AddButton(14, "Common.Button.Back".Localized(), HeliaThreesomes);
        }

        public void HelAndSluttyHarpyMale()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.LeadHeliaThroughCamp".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.FidgetsWithHerTail".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SophieAsksToJoin".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SophieHugsHelTight".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.HeliaGrowlsCutOff".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.InchIntoLoveFurnace".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SophieStammersJiggling".Localized());

            if (Player.BiggestCockLength() >= 8)
                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.NoLongerHilted".Localized());
            else
                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SlipOutOf".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SoddenCuntHelGasp".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.HeliaSaysBetweenBreaths".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SophieSilencesMoans".Localized());

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.BimboPussyNeglected".Localized());

                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.HeliaYellsAtSophie".Localized());

                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.HotAndCoolPressure".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.BallsToken".Localized());
                else
                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.GroinWord".Localized());

                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.JissomBlastsOver".Localized());

                if (Player.Vaginas.Count > 0)
                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.UnusedCumsWithCock".Localized());
            }
            else
                if (Player.Cocks.Count <= 3)
                {
                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.NoticeNeglectedPussy".Localized());

                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.PistonFewMoreTimes".Localized());

                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.WithYourCum".Localized());
                    else
                    {
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.FullOfCum".Localized());

                        if (Player.CumQ() <= 1000)
                        {
                            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.UntilItOverflows".Localized());

                            if (Player.CumQ() >= 5000)
                                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.PoolingAroundKnees".Localized());
                        }
                    }

                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SentencePeriod".Localized());

                    if (Player.Cocks.Count == 3)
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.CockSplattersTwo".Localized());

                    if (Player.Vaginas.Count > 0)
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.UnusedCumsShowOrgasm".Localized());
                }
                else
                {
                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.RealizeManyHoles".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) == 0)
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.TightWord".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) == 1)
                            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.WellTrainedWord".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.HeliaAnalTraining) >= 2)
                                OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.GapedWord".Localized());

                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.AssholeGaspsDoublePen".Localized());

                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.TakesAllYourMight".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.BallsTokenAgain".Localized());
                    else
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.GroinWordAgain".Localized());

                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.TwoGirlsShareProblem".Localized());

                    if (Player.Cocks.Count > 4)
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.ExtraCocksSprayCum".Localized());

                    if (Player.Vaginas.Count > 0)
                        OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.UnusedCumsSympathetic".Localized());
                }

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.HelGraspsFeathers".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyMale.SaysWhenKissBroken".Localized());

            Player.Orgasm("Generic");

            Save.SetInt("lastTimeSex", View.Time.Days);

            ClearButtons();

            CombatScene.CleanupAfterCombat();
        }

        public void HelAndSluttyHarpyFemale()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.GraspHeliaHand".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.ShiftsUncomfortably".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.FindHarpyPreening".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.LustyHarpyAsks".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.AskHelToLie".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.WhisperToSophie".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.SophieCrawlsOver".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.SophieBuriesFace".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.HelGivesInLicking".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.LookOverShoulder".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.HoistBimboAss".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.LickMamaSophie".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.BuryFaceInSophie".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.HeliaTongueExtended".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.SophieSquealsPleasure".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.HeliaOverwhelmed".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.FieryLoverOrgasm".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.UnusedCockSpurts".Localized());
            else
                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.UnusedCocksSpurt".Localized());

            if (Player.Cocks.Count >= 1)
                OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.BlastsSpatterMelons".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.SlideOffTwoGirls".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyFemale.SauntersOffShakily".Localized());

            Player.Orgasm("Generic");

            Save.SetInt("lastTimeSex", View.Time.Days);

            ClearButtons();

            CombatScene.CleanupAfterCombat();
        }

        public void HelAndSluttyHarpyGenderless()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.AskHeliaForTail".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.SeductiveGrinReply".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.DismissStupidHarpy".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.SophieSuggestsSharing".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.CommandThemToKiss".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.ModerateSuccessLicking".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.BodyUnsureReaction".Localized());

            OutputText("Scenes.Helfollower.HelAndSluttyHarpyGenderless.HandSlidesToBreast".Localized());

            Player.Orgasm("Generic");

            Save.SetInt("lastTimeSex", View.Time.Days);

            ClearButtons();

            CombatScene.CleanupAfterCombat();
        }

        public void HeliaSleepWith()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Helspawn")
            {
                OutputText("Scenes.Helfollower.HeliaSleepWith.SalamanderSharesBed".Localized());

                OutputText("Scenes.Helfollower.HeliaSleepWith.SteamyGreatIdea".Localized());

                OutputText("Scenes.Helfollower.HeliaSleepWith.GrinFlickFieryTail".Localized());

                OutputText("Scenes.Helfollower.HeliaSleepWith.SnoreRemark".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "salamanders");
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "salamanders")
                {
                    OutputText("Scenes.Helfollower.HeliaSleepWith.TwoSalamandersTooMuch".Localized());

                    OutputText("Scenes.Helfollower.HeliaSleepWith.GrinningAcceptance".Localized());

                    OutputText("Scenes.Helfollower.HeliaSleepWith.BedInvitation".Localized());

                    OutputText("Scenes.Helfollower.HeliaSleepWith.CertainlyWill".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Helspawn");
                }
                else
                {
                    OutputText("Scenes.Helfollower.HeliaSleepWith.AskToSleepTogether".Localized());

                    OutputText("Scenes.Helfollower.HeliaSleepWith.PattingHammock".Localized());

                    OutputText("Scenes.Helfollower.HeliaSleepWith.LashingTailHint".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Helia");
                }

            HeliaFollowerMenu();
        }

        public void HeliaSleep()
        {

            switch (Random.Shared.Next(4))
            {
                case 0:
                {
                OutputText("Scenes.Helfollower.HeliaSleep.ArriveHearSawing".Localized());

                OutputText("Scenes.Helfollower.HeliaSleep.SnoringFades".Localized(
                    Utils.Utils.NumberOfThings((int)Math.Round(View.Time.LastPassedHours), "hour")
                ));
                }

                break;

                case 1:
                {
                OutputText("Scenes.Helfollower.HeliaSleep.ArriveTogether".Localized());

                OutputText("Scenes.Helfollower.HeliaSleep.BothExhausted".Localized(
                    Utils.Utils.NumberOfThings((int)Math.Round(View.Time.LastPassedHours), "hour")
                ));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Helfollower.HeliaSleep.FindHerSprawled".Localized());

                OutputText("Scenes.Helfollower.HeliaSleep.JoinMeOffer".Localized());

                OutputText("Scenes.Helfollower.HeliaSleep.SlipIntoBed".Localized(
                    Utils.Utils.NumberOfThings((int)Math.Round(View.Time.LastPassedHours), "hour")
                ));
                }

                break;

                case 3:
                {
                OutputText("Scenes.Helfollower.HeliaSleep.OddlyQuiet".Localized());

                OutputText("Scenes.Helfollower.HeliaSleep.HappyDisbelief".Localized());

                OutputText("Scenes.Helfollower.HeliaSleep.ScootsAndHugs".Localized(
                    Utils.Utils.NumberOfThings((int)Math.Round(View.Time.LastPassedHours), "hour")
                ));
                }

                break;
            }

            OutputText("Scenes.Helfollower.HeliaSleep.ParagraphBreak".Localized());
        }

        public void SalamandersSleep()
        {

            switch (Random.Shared.Next(3))
            {
                case 0:
                {
                OutputText("Scenes.Helfollower.SalamandersSleep.BothAlreadyThere".Localized());

                OutputText("Scenes.Helfollower.SalamandersSleep.HoggingComplaint".Localized());

                OutputText("Scenes.Helfollower.SalamandersSleep.PlentyToShare".Localized());

                OutputText("Scenes.Helfollower.SalamandersSleep.PlayfulBanter".Localized(
                    Utils.Utils.NumberOfThings((int)Math.Round(View.Time.LastPassedHours), "hour")
                ));
                }

                break;

                case 1:
                {
                OutputText("Scenes.Helfollower.SalamandersSleep.MeetHelspawn".Localized());

                OutputText("Scenes.Helfollower.SalamandersSleep.SleepDuration".Localized(
                    Utils.Utils.NumberOfThings((int)Math.Round(View.Time.LastPassedHours), "hour")
                ));
                }

                break;

                case 2:
                {
                OutputText("Scenes.Helfollower.SalamandersSleep.BedtimeVisit".Localized());

                OutputText("Scenes.Helfollower.SalamandersSleep.AwkwardInterruption".Localized());

                OutputText("Scenes.Helfollower.SalamandersSleep.HeadSnapsBack".Localized());

                OutputText("Scenes.Helfollower.SalamandersSleep.LiesDownBeside".Localized(
                    Utils.Utils.NumberOfThings((int)Math.Round(View.Time.LastPassedHours), "hour")
                ));
                }

                break;
            }

            OutputText("Scenes.Helfollower.SalamandersSleep.ParagraphBreak".Localized());
        }

        public void HeliaLustFuck()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaLustFuck.HazyRedMist".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.ExactlyWhoYouNeed".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.WorldSpins".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.CasualSmirk".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.ShudderingWords".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.PantingPlea".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.HeliaReciprocates".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.HipsFindGrasp".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.BlessedRelief".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.BrinkTooSoon".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck.ShortLivedOrgasm".Localized());

            Player.Orgasm("Generic");

            HeliaLustFuck2();
        }

        public void HeliaLustFuck2()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaLustFuck2.MindlessThrusting".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck2.SlappingSound".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck2.FirstSpurt".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck2.ClimaxEndures".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck2.LoverScreams".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((2) * 60)));

            Player.Orgasm("Generic");

            HeliaLustFuck3();
        }

        public void HeliaLustFuck3()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaLustFuck3.WhirlsToDevour".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck3.SkilledMouth".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck3.FinishingStroke".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck3.NeededRest".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((4) * 60)));

            Player.Orgasm("Generic");

            HeliaLustFuck4();
        }

        public void HeliaLustFuck4()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaLustFuck4.FadingConsciousness".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck4.SensualDreams".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((2) * 60)));

            Player.Orgasm("Generic");

            HeliaLustFuck5();
        }

        public void HeliaLustFuck5()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaLustFuck5.SnapAwake".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck5.RubbingEyes".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck5.PassionateKiss".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck5.MorningStretches".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck5.HappyToComply".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck5.CannotImagineWaking".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((8) * 60)));

            HeliaLustFuck6();
        }

        public void HeliaLustFuck6()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaLustFuck6.NumbToScent".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck6.ContentToLie".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((6) * 60)));

            HeliaLustFuck7();
        }

        public void HeliaLustFuck7()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaLustFuck7.CompleteWreck".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck7.AskingHowLong".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck7.SlackJawedLook".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck7.NeedRecovery".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck7.DayAfterTomorrow".Localized());

            OutputText("Scenes.Helfollower.HeliaLustFuck7.GoneBeforeReply".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f));

            Player.RemoveStatusEffect(StatusEffectIds.Heat);

            Player.RemoveStatusEffect(StatusEffectIds.Rut);

            Save.SetInt("lastTimeSex", View.Time.Days);

            View.Time.PassMinutes((int)Math.Round((double)((2) * 60)));

            Go<CampScene>();
        }

        public void HeliaNightlyNeeds()
        {
            ClearOutput();

            OutputText("Scenes.Helfollower.HeliaNightlyNeeds.SplayedWelcome".Localized());

            OutputText("Scenes.Helfollower.HeliaNightlyNeeds.IntoHerEmbrace".Localized());

            OutputText("Scenes.Helfollower.HeliaNightlyNeeds.InfectiousIntensity".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.DesperateLust".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.PurringApproval".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.EagerLeap".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.Overstimulated".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.RefusingToStop".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.PantingComply".Localized());

                Player.Orgasm("Vaginal");
            }
            else
            {
                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.SnarlsInDelight".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.ZealousThrusts".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.SavageRhythm".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.NeverSlowing".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.NearingClimax".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.RelentlessPace".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.SharedBrink".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.HoldingHerTight".Localized());

                OutputText("Scenes.Helfollower.HeliaNightlyNeeds.CollapseOnBed".Localized());

                Player.Orgasm("Dick");
            }

            Save.SetInt("lastTimeSex", View.Time.Days-1);

            Go<CampScene>();
        }

    }
}