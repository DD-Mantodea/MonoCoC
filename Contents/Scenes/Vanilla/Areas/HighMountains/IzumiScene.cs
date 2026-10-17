using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains
{
    [Register<IzumiScene>("coc:izumi")]
    public class IzumiScene : Scene
    {

        public float contestStage = 0.0f;

        public static SaveContent Save => SaveContent(SceneIds.HighMountains.Izumi);

        public string debugName => "Izumi";

        public string debugHint => "";

        public override void Entry()
        {
            Encounter();
        }

        public void Reset()
        {

            Save.SetFloat("contestStage", 0);
        }

        public string HeightDesc(bool doName = true)
        {

            if (Random.Shared.Next(4) != 0)
                return "kid";
            else
            {
                if (Player.Tallness < 60)
                    return "pint-size";

                if (Player.Tallness < 72)
                    return "short-stuff";

                if (Player.Tallness < 84)
                    return "shorty";

                if (Player.Tallness < 96)
                    return "kid";
                else
                    if (doName)
                        return Player.ShortName;
                    else
                        return "kid";
            }
        }

        public string HeightDesc()
        {
            var str = this.HeightDesc(true);

            return char.ToUpperInvariant(str[0]) + str.Substring(1);
        }

        public bool ActsLikeACow()
        {

            return Player.InHeat || Player.InRut || Player.HasStatusEffect(StatusEffectIds.Dysfunction);
        }

        public void Encounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzumiMet) == 0)
            {
                OutputText("Scenes.Izumiscene.Encounter.WanderFindCaveEntrance".Localized());

                OutputText("Scenes.Izumiscene.Encounter.PressStoneHearHumming".Localized());

                OutputText("Scenes.Izumiscene.Encounter.FindGapSeeCampfire".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Enter".Localized(), EnterFirstTime);

                AddButton(1, "Common.Button.Leave".Localized(), Abortabortleavethefukkencave);
            }
            else
            {
                OutputText("Scenes.Izumiscene.Encounter.ReturnToCaveHumming".Localized());

                OutputText("Scenes.Izumiscene.Encounter.HesitateEnterOrLeave".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Enter".Localized(), EnterAfterMet);

                AddButton(1, "Common.Button.Leave".Localized(), NopeLeavePlz);
            }
        }

        public void EnterAfterMet()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.IzumiLastEncounter, 2);

            if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 1)
            {
                OutputText("Scenes.Izumiscene.EnterAfterMet.LastVisitOutcome".Localized());

                OutputText("Scenes.Izumiscene.EnterAfterMet.IzumiRibsGreeting".Localized(
                     this.HeightDesc()
                ));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 2)
                {
                    OutputText("Scenes.Izumiscene.EnterAfterMet.RevisitCaveMouth".Localized());

                    OutputText("Scenes.Izumiscene.EnterAfterMet.CheerfulReturnCall".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 3)
                    {
                        OutputText("Scenes.Izumiscene.EnterAfterMet.BackAtIzumiCamp".Localized());

                        OutputText("Scenes.Izumiscene.EnterAfterMet.BlushingApologyTease".Localized());
                    }
                    else
                    {

                    }

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheFuckhugeOni);

            AddButton(1, "Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ButtonSurrender".Localized(), FuckhugeOniGetsWhatSheWants);

            AddButton(2, "Common.Button.Flee".Localized(), FuckinChegIt);

            if (Save.GetFloat("contestStage") == 0)
                AddNextButton("Common.Button.Contest".Localized(), IzumiContest);
            else
                if (Save.GetFloat("contestStage") < 3)
                    AddNextButton("Common.Button.Rematch".Localized(), IzumiRematch);
        }

        public void Abortabortleavethefukkencave()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.Abortabortleavethefukkencave.RandomCaveWarning".Localized());

            OutputText("Scenes.Izumiscene.Abortabortleavethefukkencave.ReturnCampEmpty".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NopeLeavePlz()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.NopeLeavePlz.DiscretionOverValor".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void EnterFirstTime()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IzumiMet, 1);

            if (Player.Tallness >= 108)
            {
                if (Player.Strength >= 75)
                    OutputText("Scenes.Izumiscene.EnterFirstTime.ShoulderBoulderPush".Localized());
                else
                    OutputText("Scenes.Izumiscene.EnterFirstTime.ShoulderBoulderPushAgain".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.HummingStopsHand".Localized());

                if (Player.Strength < 75)
                    OutputText("Scenes.Izumiscene.EnterFirstTime.BoulderShoveOpen".Localized());
                else
                    OutputText("Scenes.Izumiscene.EnterFirstTime.ParagraphBreak".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.SuspicionStudy".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.EyesBackToFace".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.SyrupySeductiveDrawl".Localized());

                if (Player.Strength >= 75)
                    OutputText("Scenes.Izumiscene.EnterFirstTime.ForcingPrefix".Localized());
                else
                    OutputText("Scenes.Izumiscene.EnterFirstTime.TryingToForcePrefix".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.IzumiDoorScoldPrefix".Localized());

                if (Player.Horns.Id != BodyPartTypes.CowMinotaur)
                {
                    OutputText("Scenes.Izumiscene.EnterFirstTime.BullHornsAside".Localized());

                    if (this.ActsLikeACow() == true)
                        OutputText("Scenes.Izumiscene.EnterFirstTime.TemperamentAside".Localized());
                    else
                        OutputText("Scenes.Izumiscene.EnterFirstTime.Comma".Localized());
                }
                else
                    if (this.ActsLikeACow() == true)
                        OutputText("Scenes.Izumiscene.EnterFirstTime.TemperamentPhysiqueAside".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.RhetoricalFollowAsk".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.TurnsBackIntoGloom".Localized());
            }
            else
                OutputText("Scenes.Izumiscene.EnterFirstTime.SqueezeThroughGap".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTime.BraziersDimInterior".Localized());

            if (Player.Toughness <= 75)
                OutputText("Scenes.Izumiscene.EnterFirstTime.EnduranceSappingHeat".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTime.PeriodParagraphBreak".Localized());

            if (Player.Tallness < 108)
            {
                OutputText("Scenes.Izumiscene.EnterFirstTime.CreepTowardHumming".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.ShadowGruntsSpeaks".Localized());

                if (Player.MinoScore() > 3)
                    OutputText("Scenes.Izumiscene.EnterFirstTime.RightHeadThinking".Localized());
                else
                    if (Player.DogScore() > 3)
                        OutputText("Scenes.Izumiscene.EnterFirstTime.NotChasingStick".Localized());
                    else
                        if (Player.GoblinScore() > 3)
                            OutputText("Scenes.Izumiscene.EnterFirstTime.NotInsufferablePraise".Localized());
                        else
                            if (Player.DemonScore() > 3)
                                OutputText("Scenes.Izumiscene.EnterFirstTime.NoCorruptionRant".Localized());
                            else
                                if (Player.HarpyScore() > 3)
                                    OutputText("Scenes.Izumiscene.EnterFirstTime.NoScreechingHarpy".Localized());
                                else
                                    if (Player.LizardScore() > 3)
                                        OutputText("Scenes.Izumiscene.EnterFirstTime.NoCreepyEyes".Localized());
                                    else
                                        OutputText("Scenes.Izumiscene.EnterFirstTime.NotTroglodyteLook".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTime.SitDownInvitation".Localized());
            }

            OutputText("Scenes.Izumiscene.EnterFirstTime.EdgeTowardFire".Localized());

            if (Player.Tallness >= 108)
                OutputText("Scenes.Izumiscene.EnterFirstTime.ALetter".Localized());
            else
                OutputText("Scenes.Izumiscene.EnterFirstTime.TheWord".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTime.ShadowBecomesVisible".Localized());

            if (Player.Tallness < 108)
                OutputText("Scenes.Izumiscene.EnterFirstTime.StunnedByHumanLook".Localized());
            else
                OutputText("Scenes.Izumiscene.EnterFirstTime.StunnedStudyingWoman".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTime.SecondFactorHorn".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTime.CaughtStaringHorn".Localized());

            if (Player.Toughness < 60)
                OutputText("Scenes.Izumiscene.EnterFirstTime.CoughFromSmoke".Localized());
            else
                OutputText("Scenes.Izumiscene.EnterFirstTime.VoiceStolenByBurn".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTime.ConfusedThenGrin".Localized());

            if (Player.Toughness < 60)
                OutputText("Scenes.Izumiscene.EnterFirstTime.KidLetFragment".Localized());
            else
                OutputText("Scenes.Izumiscene.EnterFirstTime.LetTagFragment".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTime.GetAirOffer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), EnterFirstTimePartII);
        }

        public void EnterFirstTimePartII()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.StillWord".Localized());

            if (Player.Toughness < 60)
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.StrugglingWithPrefix".Localized());
            else
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.DistractedByPrefix".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.SmokeTurnToBoulder".Localized());

            if (Player.Strength <= 75)
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.AmazedBoulderMoves".Localized());
            else
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.BoulderWelcomeAir".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.CaveMouthHalfOpen".Localized());

            if (Player.Tallness < 108)
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.SilkenKimono".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.KneeRaisedPose".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.UncomfortableThigh".Localized());
            else
                if (Player.Corruption > 33 && Player.Corruption < 66)
                    OutputText("Scenes.Izumiscene.EnterFirstTimePartII.TantalizingThigh".Localized());
                else
                    OutputText("Scenes.Izumiscene.EnterFirstTimePartII.DeliciousThigh".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.DoesntSeemToCare".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.BetterSmokeApology".Localized(
                 this.HeightDesc(false)
            ));

            if (Player.Tallness <= 96)
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.LittleImposingAside".Localized());
            else
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.Comma".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.NameAndBackstory".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.HeroicTypeSigh".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.FontOfStrengthName".Localized());

            if (Player.HumanScore() > 3)
            {
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.HumansGamesBackHome".Localized());

                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.PowerfulPipeDrag".Localized());
            }

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.JustWantingFun".Localized());

            if (Player.Strength <= 75 || Player.Tallness < 96)
                OutputText("Scenes.Izumiscene.EnterFirstTimePartII.WolfStaringDown".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.NoFunNoChallenges".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.UnperturbedByLust".Localized());

            OutputText("Scenes.Izumiscene.EnterFirstTimePartII.AnimatedHourTalk".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Smoke".Localized(), SmokeThePipe);

            AddButton(1, "Common.Button.Nosmoke".Localized(), DontSmokeThePipe);
        }

        public void SmokeEffect(bool smokedPipe)
        {

            var lustMod = smokedPipe ? 14 : 7;

            Player.CreateStatusEffect(StatusEffectIds.IzumisPipeSmoke, 0, 0, 0, smokedPipe ? 1 : 0);

            Player.ApplyStats(new DynStat(GameStats.Lust, lustMod));
        }

        public void SmokeThePipe()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.SmokeThePipe.ThickCloyingSmoke".Localized());

            if (Player.Toughness <= 60)
                OutputText("Scenes.Izumiscene.SmokeThePipe.CoughSplutterReaction".Localized());
            else
                if (Player.Toughness < 75)
                    OutputText("Scenes.Izumiscene.SmokeThePipe.FlinchHoldCough".Localized());
                else
                    OutputText("Scenes.Izumiscene.SmokeThePipe.EyesWater".Localized());

            OutputText("Scenes.Izumiscene.SmokeThePipe.MutualAmusementTalk".Localized());

            if (Player.Toughness <= 80)
                OutputText("Scenes.Izumiscene.SmokeThePipe.TooWeakToReach".Localized());
            else
                OutputText("Scenes.Izumiscene.SmokeThePipe.ParagraphBreak".Localized());

            SmokeEffect(true);

            SmokeJoins();
        }

        public void DontSmokeThePipe()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.DontSmokeThePipe.PolitePipeRefusal".Localized());

            SmokeEffect(false);

            SmokeJoins();
        }

        public void SmokeJoins()
        {

            OutputText("Scenes.Izumiscene.SmokeJoins.ConversationDiesDown".Localized());

            OutputText("Scenes.Izumiscene.SmokeJoins.IzumiGigglesCloser".Localized());

            if (Player.Strength <= 75 || Player.Toughness <= 75 || Player.Tallness <= 96)
                OutputText("Scenes.Izumiscene.SmokeJoins.SwallowNervously".Localized());

            OutputText("Scenes.Izumiscene.SmokeJoins.LargerUpClose".Localized());

            if (Player.Tallness < 108)
            {
                OutputText("Scenes.Izumiscene.SmokeJoins.CuteRemarkLeaningIn".Localized());

                OutputText("Scenes.Izumiscene.SmokeJoins.DemandRepayment".Localized());

                OutputText("Scenes.Izumiscene.SmokeJoins.SlurredMakeItUp".Localized());
            }
            else
                OutputText("Scenes.Izumiscene.SmokeJoins.HomesickCompliment".Localized());

            OutputText("Scenes.Izumiscene.SmokeJoins.WeighHerAdvance".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightTheFuckhugeOni);

            AddButton(1, "Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ButtonSurrender".Localized(), FuckhugeOniGetsWhatSheWants);

            AddButton(2, "Common.Button.Flee".Localized(), FuckinChegIt);

            if (Save.GetFloat("contestStage") == 0)
                AddNextButton("Common.Button.Contest".Localized(), IzumiContest);
            else
                if (Save.GetFloat("contestStage") < 3)
                    AddNextButton("Common.Button.Rematch".Localized(), IzumiRematch);
        }

        public void FuckinChegIt()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IzumiLastEncounter, 2);

            OutputText("Scenes.Izumiscene.FuckinChegIt.HastyRetreat".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void FightTheFuckhugeOni()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 0)
            {
                OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.RefuseToSubmit".Localized());

                OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.TauntChallenge".Localized(
                     this.HeightDesc()
                ));

                if (Player.Tone <= 75)
                    OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.OfferHandicap".Localized());

                OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.SmirkWager".Localized());

                if (Player.Tone <= 75)
                    OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.OneHandedInsult".Localized());

                OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.RiseToStance".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 3)
                {
                    OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.RefuseApology".Localized());

                    OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.StanceNoHandicap".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 1)
                    {
                        OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.RefuseHerDominance".Localized());

                        OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.OfferHandicapAgain".Localized());

                        OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.NodIntoStance".Localized());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IzumiLastEncounter) == 2)
                        {
                            OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.FaceOniNow".Localized());

                            OutputText("Scenes.Izumiscene.FightTheFuckhugeOni.NoMoreRunning".Localized());
                        }
                        else
                        {

        }
        }

        public void FuckhugeOniGetsWhatSheWants()
        {
            ClearOutput();

            if (Player.HasCock())
            {
                OutputText("Scenes.Izumiscene.FuckhugeOniGetsWhatSheWants.SwallowPride".Localized());

                OutputText("Scenes.Izumiscene.FuckhugeOniGetsWhatSheWants.SmirkObedience".Localized());

                OutputText("Scenes.Izumiscene.FuckhugeOniGetsWhatSheWants.SuddenLungeGrab".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Izumiscene.FuckhugeOniGetsWhatSheWants.FlankPinSideways".Localized());
                else
                    OutputText("Scenes.Izumiscene.FuckhugeOniGetsWhatSheWants.ChestPinBackward".Localized());

                OutputText("Scenes.Izumiscene.FuckhugeOniGetsWhatSheWants.OneArmRestraint".Localized());

                if (Player.BiggestCockLength() <= 4)
                    SurrenderSmallCock();
                else
                    if (Player.BiggestCockLength() <= 10)
                        SurrenderMediumCock();
                    else
                        SurrenderLargeCock();
            }
            else
                if (Player.HasVagina())
                    SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp();
                else
                    NoDickNoVagNoService();

            CoC.Instance.Flags.Set(GameFlagIds.IzumiTimesSubmitted, CoC.Instance.Flags.Get(GameFlagIds.IzumiTimesSubmitted) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.IzumiLastEncounter, 1);

            CoC.Instance.Flags.Set(GameFlagIds.IzumiSeenPcGender, (int)Player.Gender);
        }

        public void SurrenderSmallCock()
        {

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.FirstSightPurr".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.BigWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.SmallWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.NotWhatExpected".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.LookAwayBlushing".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.FlankWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.ChestWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.GripChinLockEyes".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.SeatOnYourFlank".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.LieBesideYou".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.ReleaseChinPullIn".Localized());

            if (Player.BiggestBreastSize() >= 1 && Player.BiggestBreastSize() < 3)
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.ChestDwarfed".Localized());
            else
                if (Player.BiggestBreastSize() >= 3 && Player.BiggestBreastSize() < 5)
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.ChestMolding".Localized());
                else
                    if (Player.BiggestBreastSize() >= 5 && Player.BiggestBreastSize() <= 8)
                        OutputText("Scenes.Izumiscene.SurrenderSmallCock.ChestCompressing".Localized());
                    else
                        OutputText("Scenes.Izumiscene.SurrenderSmallCock.ChestDwarfing".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.CheekStrokeReassure".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.PinnedCoaxErection".Localized());

            if (Player.Strength <= 75)
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.PowerlessToResist".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.StruggleToDeny".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.ShameOverHandjob".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.MindSwitchesOff".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.AsksAboutShame".Localized(
                 this.HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.MocksTinySize".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.ArousalDripping".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.PauseHandLingers".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.GuessSizeRemark".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.RegularPhrasePrefix".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.AWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.GirlThenHandBottoms".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.StrokeBalls".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.StrokeVag".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.SentencePeriod".Localized());

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.MinotaurWhisper".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) >= 1)
            {
                OutputText("Scenes.Izumiscene.SurrenderSmallCock.MinotaurTrigger".Localized());

                OutputText("Scenes.Izumiscene.SurrenderSmallCock.DirtyBoyTease".Localized());

                OutputText("Scenes.Izumiscene.SurrenderSmallCock.CalledItRight".Localized());
            }
            else
            {
                if (!Player.IsTaur())
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.PushToSeated".Localized());

                OutputText("Scenes.Izumiscene.SurrenderSmallCock.WantsToWatch".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.HandjobOtherSide".Localized());

                OutputText("Scenes.Izumiscene.SurrenderSmallCock.StaresAtCock".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.AlongYourFlank".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.PumpHarder".Localized());

                OutputText("Scenes.Izumiscene.SurrenderSmallCock.CombinedSensation".Localized());

                if (Player.CumQ() <= 100)
                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.DribbleHotMess".Localized());
                else
                    if (Player.CumQ() > 100 && Player.CumQ() <= 250)
                        OutputText("Scenes.Izumiscene.SurrenderSmallCock.SpurtsHotMess".Localized());
                    else
                        if (Player.CumQ() > 250)
                        {
                            OutputText("Scenes.Izumiscene.SurrenderSmallCock.ShootsRopesCum".Localized());

                            if (Player.CumQ() > 500)
                            {
                                OutputText("Scenes.Izumiscene.SurrenderSmallCock.PuddleForms".Localized());

                                if (Player.CumQ() > 1000)
                                    OutputText("Scenes.Izumiscene.SurrenderSmallCock.LakeSpreads".Localized());
                            }
                            else
                                OutputText("Scenes.Izumiscene.SurrenderSmallCock.ParagraphBreak".Localized());
                        }

                OutputText("Scenes.Izumiscene.SurrenderSmallCock.CrowsAtSpasms".Localized(
                     this.HeightDesc()
                ));
            }

            OutputText("Scenes.Izumiscene.SurrenderSmallCock.DustsOffPalms".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void SurrenderMediumCock()
        {

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.ImpressedBySize".Localized(
                 this.HeightDesc()
            ));

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.BiggerWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.SmallerWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.TeasesGiftedRemark".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.RapidFullErection".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.YourVaginaSuffix".Localized());

                if (Player.Wetness() == 0)
                    OutputText("Scenes.Izumiscene.SurrenderMediumCock.MoisteningWord".Localized());
                else
                    if (Player.Wetness() == 1)
                        OutputText("Scenes.Izumiscene.SurrenderMediumCock.DrippingArousalFluid".Localized());
                    else
                        if (Player.Wetness() == 2)
                            OutputText("Scenes.Izumiscene.SurrenderMediumCock.OngoingFeminineArousal".Localized());
                        else
                            OutputText("Scenes.Izumiscene.SurrenderMediumCock.SoppyFemSlimeLake".Localized());

                OutputText("Scenes.Izumiscene.SurrenderMediumCock.LustFueledHarmony".Localized());
            }
            else
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.SentencePeriod".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.IndulgentSmileDown".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.ReadyToStartLine".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.SeatsOnYourFlank".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.LiesBesideYou".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.SheReleasesPrefix".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.FlankWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.ChestWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.LeansOnElbow".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.GlaciallySlowPace".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.SuddenHipSpasm".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderMediumCock.UpwardsWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.ReflexiveHipBuck".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCock.PlayfulThreatPurr".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Obey".Localized(), SurrenderMediumCockObey);

            AddButton(1, "Common.Button.Refuse".Localized(), SurrenderMediumCockRefuse);
        }

        public void SurrenderMediumCockObey()
        {

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.ForcesSelfCalm".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.YLetter".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.LoweringHipsPrefix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.RelaxAndWaitSuffix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.ApprovesThenTrailsOff".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.OrgasmRisesFast".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.CasualCumPermit".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.BallsWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.CockWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.LastDropSuffix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.OrgasmFinallySubsides".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.MixedPuddlePrefix".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.OwnPuddlePrefix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockObey.PantingAfterSuffix".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void SurrenderMediumCockRefuse()
        {

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.DefiesHerControl".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.UnconcernedDismissal".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.FutileStruggle".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.MocksYourEffort".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.OverwhelmingArousal".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.DeniedOrgasmAnger".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.SympathySpasmSuffix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.SensesImpendingOrgasm".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.FeignedHelpOffer".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.BarelyEnoughTouch".Localized());

            if (Player.CumQ() <= 250)
                OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.SpurtsOverYourself".Localized());
            else
                if (Player.CumQ() <= 500)
                    OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.GlobsShowerDown".Localized());
                else
                    if (Player.CumQ() <= 1000)
                        OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.FertileSpunkTorrent".Localized());
                    else
                        OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.ArcingRopesOfCum".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.GentlePatAfter".Localized());

            OutputText("Scenes.Izumiscene.SurrenderMediumCockRefuse.LessonThenLeaves".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void SurrenderLargeCock()
        {

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.SurprisedByGirth".Localized(
                 this.HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.FrowningInspection".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.ReleasesFromFloor".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderLargeCock.HaunchesRaisedSuffix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.SettlesBetweenLegs".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.CountdownSixGrab".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.CountdownFiveGrip".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.CountdownFourFeet".Localized());

            if (Player.Balls > 0 && !Player.HasVagina())
                OutputText("Scenes.Izumiscene.SurrenderLargeCock.BallsMassageSuffix".Localized());
            else
                if (Player.Balls == 0 && Player.HasVagina())
                    OutputText("Scenes.Izumiscene.SurrenderLargeCock.VaginaTeaseSuffix".Localized());
                else
                    if (Player.Balls > 0 && Player.HasVagina())
                        OutputText("Scenes.Izumiscene.SurrenderLargeCock.AlternatingTeaseSuffix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.ParagraphBreak".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.QuiverFromSensation".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.CountdownThreeMoves".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.VoiceBreaksFocus".Localized(
                 this.HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.CountdownTwoShift".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.HelplessGruntShiver".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Izumiscene.SurrenderLargeCock.AchingBallsRealize".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.WarnedTechniqueRealized".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.CountdownOneDemand".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Izumiscene.SurrenderLargeCock.EndlessMoaningWait".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.OrgasmSubsidesCollapse".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderLargeCock.SidewaysWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderLargeCock.BackwardsWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.StaringAtRoofSuffix".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.HalfApologyGiggle".Localized());

            OutputText("Scenes.Izumiscene.SurrenderLargeCock.LeavesYouForPipe".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp()
        {

            OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ResignToUndress".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IzumiTimesSubmitted) == 0)
            {
                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiGrabsWrist".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiWantsToSavor".Localized());

                if (Player.Corruption <= 33)
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.CripplingEmbarrassment".Localized());
                else
                    if (Player.Corruption >= 66)
                        OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.FamiliarHeatBuilds".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ParagraphBreak".Localized());

                if (Player.BiggestBreastSize() >= 15)
                {
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiShockedByBust".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.AsksIfBreastsReal".Localized());

                    if (Player.Corruption < 50)
                    {
                        OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiGropesForProof".Localized());

                        OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiMuttersUnbelievable".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ProudlyNoddingAtChest".Localized());

                        OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiScowlsAndPinches".Localized());

                        OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.TeasingShrugProvokes".Localized());
                    }

                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiCallsShameless".Localized());
                }
            }
            else
                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiTakesOverUndressing".Localized());
            // 原版 IzumiScene.hx:850：该分支的触发条件
            if (SceneInstance<Npcs.CeraphScene>.Get().HasExhibition())
            {
                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiRemovesArmor".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.SmugIzumiSurveys".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiSurprisedByWetness".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.CockThickensUnderGaze".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ShiftInPlace".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.WriggleAtopLegs".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.SubtlyPartingLegs".Localized());

                if (Player.IsTaur())
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.RollsButtUpward".Localized());
                else
                {

                }

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ShowsOffForIzumi".Localized());

                if (!Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.RubThighsInDelight".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.WriggleAndWrithe".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.EnjoyingBeingWatched".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.SlithersDeepInside".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ShiftsWeightBetweenFeet".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.BeingWatchedFeelsGood".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiReassuresYou".Localized(
                     this.HeightDesc()
                ));

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.CumsMeetingHerGaze".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiObliviousToCum".Localized());
            }

            if (!Player.IsTaur())
            {
                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiLungesGrabbing".Localized());

                if (!Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ForcedDownOnKnees".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.PressedDownToGround".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ParagraphBreakAgain".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.GripsChinFromBehind".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.SpacedLegTag".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.ThighsWord".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.PeriodParagraphBreak".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.MurmursIntoEar".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.StrugglesAgainstOni".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.YankedBackByIzumi".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.RealizesCantEscape".Localized());
            }
            else
            {
                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiLeapsAtYou".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.GripsChinWithHand".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.MurmursThenJerksYou".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.StrugglesInWristGrip".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.IzumiLaysAlongYou".Localized());

                OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.EscapeSeemsHopeless".Localized());
            }

            OutputText("Scenes.Izumiscene.SurrenderOhGodTheFemaleSurrenderContentIsFuckingHugeSendHelp.WhispersNoticingWetness".Localized());
            // 原版 IzumiScene.hx:924 — 解锁 Ceraph 露出剧情后才给「ShowMeOff/HideMe」
            if (SceneInstance<Npcs.CeraphScene>.Get().HasExhibition())
            {
                ClearButtons();

                AddButton(0, "Common.Button.Showmeoff".Localized(), SurrenderFemaleExhibitionVariant);

                AddButton(1, "Common.Button.Hideme".Localized(), SurrenderFemaleNonExhibitionVariant);
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SurrenderFemaleNonExhibitionVariant);
            }
        }

        public void SurrenderFemaleNonExhibitionVariant()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SquirmingUnderLeers".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.YellsAtVultures".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SittingUprightOnBack".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.HurlsRockAtCrowd".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.ReassuredDespiteGawking".Localized());

            if (Player.HasMuzzle())
                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.MawWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.MouthWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.StunnedByFrenchKiss".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.ResumesCuppingBreast".Localized());

            if (Player.BiggestBreastSize() <= 2)
            {
                if (Player.IsChild())
                {
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.WhispersCutePraise".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SaysYouMightGrow".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.HissesAboutOlderMen".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.StammeringConfusedReply".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SoftlyReassuresYou".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.GuidesHandToNethers".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.PeriodParagraphBreak".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.KeepsYouHere".Localized());
                }
                else
                {
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.WhispersCutePraiseAgain".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SaysItsPrettySad".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.HissesNoWayAnyone".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.StammeringConfusedReplyAgain".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SoftlyReassuresYouAgain".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.GuidesHandDownAgain".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.PeriodParagraphBreakAgain".Localized());

                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.KeepsYouHereAgain".Localized());
                }

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.GroansThenAcquiesces".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.AddledArousalBuilds".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.StrangledScreamCum".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.GoodGirlPraise".Localized(
                     this.HeightDesc()
                ));

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.BlownKissFarewell".Localized());

                if (Player.IsChild())
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.DesiresArentNormal".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SchoolgirlOutfitKinky".Localized());
            }
            else
            {
                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.GruntsApprovingly".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SatisfiedHandSlides".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.OverYourHip".Localized());
                else
                    if (Player.IsDrider())
                        OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.BetweenSpindlyLegs".Localized());
                    else
                        OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.BetweenYourThighs".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.BreastsPressBack".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.AlongWord".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.BetweenWord".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.FeatherLightTease".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.TeasingActuallyWorks".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.LaughsCallsYouDyke".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.FingerLikeCock".Localized());

                if (Player.Vaginas[0].Looseness <= 1)
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.PushesInSlowly".Localized());
                else
                    if (Player.Vaginas[0].Looseness >= 3)
                        OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.FingerBottomsOut".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.TeasesAboutBeingBusy".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.OneHandGropesChest".Localized(
                     this.HeightDesc()
                ));

                if (Player.LactationQ() > 0)
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.MoansAsMilkSpurts".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.BreastsMilked) && (Player.LactationQ() > 750))
                {
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.LeansBackIntoBreasts".Localized());

                    if (Player.Corruption < 40)
                        OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.HowDidThisHappen".Localized());
                    else
                        if (Player.Corruption >= 40 && Player.Corruption <= 90)
                            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SurprisinglyPleasant".Localized());
                        else
                            OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.GoodToBeMilked".Localized());
                }

                Player.Orgasm("Tits");

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.AddsToArousal".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.EnjoyableLittleFantasy".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.LostInMilkingFantasy".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.RealizesSheFinished".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.ToWord".Localized());
                else
                    OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.BetweenWordAgain".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.ReturnsAttentionToGroin".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SkilledDespiteClaims".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.KissWhileGasping".Localized());

                OutputText("Scenes.Izumiscene.SurrenderFemaleNonExhibitionVariant.SalivaTrailAfterKiss".Localized());
            }

            Player.Orgasm("Vaginal");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void SurrenderFemaleExhibitionVariant()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.SurrenderFemaleExhibitionVariant.HottestThoughtEver".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Izumiscene.SurrenderFemaleExhibitionVariant.SpreadLegsWider".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleExhibitionVariant.YouWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleExhibitionVariant.ArchBackForAudience".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleExhibitionVariant.TeasesExhibitionKink".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut);

            AddButton(1, "Common.Button.No".Localized(), SurrenderFemaleNonExhibitionVariant);
        }

        public void SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut()
        {

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.NodsAtAudienceIdea".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.ReleasesYouAndStands".Localized(
                 this.HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.AnnouncesToCrowd".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.DeathlySilenceThenImp".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.OffersFirstImp".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.ThighsWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.LegsWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.RevealsRagingErection".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.HesitantlyTakesCock".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.KneelsBeforeImp".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.SuckingImpCock".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.AsksAboutGivingShow".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.FeralWhoreDemand".Localized());

            if (Player.HasMuzzle())
                OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.MuzzleWord".Localized());
            else
                OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.FaceWord".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.TearsOfExertion".Localized(
                 this.HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.ImpPullsFree".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.ReadyForRoundTwo".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.EndlessParadeOfHarpies".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.ArchPerformanceReview".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.DraggedIntoCave".Localized());

            OutputText("Scenes.Izumiscene.SurrenderFemaleLookitYouTheCenterOfAttentionYouSlut.WakesWithAStart".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckedUpByAFuckhugeOni(bool titLoss = false)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IzumiLastEncounter, 1);

            CoC.Instance.Flags.Set(GameFlagIds.IzumiSeenPcGender, (int)Player.Gender);

            if (Player.Health <= 0)
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.KnockedDownByStrength".Localized());
            else
                if (Player.Lust >= Player.MaxLust)
                    OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.SwooningFromArousal".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.VictoryMeansPlaytime".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.ReflexiveLeadIn".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.SlitherWord".Localized());
            else
                if (Player.IsDrider())
                    OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.ScuttleWord".Localized());
                else
                    OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.ScurryWord".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.BackwardsEscapeInstinct".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.PuzzledAtYourShudder".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.ArousedYetHesitant".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.OniLooksHurt".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.FeelsRejectedByYou".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.OffersYouLeave".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.DoubtingHerSincerity".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.SlitherAgain".Localized());
            else
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.WalkWord".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.ExitsCaveSmoothly".Localized());

            if (Player.LustPercent >= 75)
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.TemptingSexProspect".Localized());
            else
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.FLetter".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.KimonoClingsTight".Localized());

            if (Player.LustPercent >= 75)
                OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.WonderingAboutSliding".Localized());

            OutputText("Scenes.Izumiscene.FuckedUpByAFuckhugeOni.ParagraphBreak".Localized());

            ClearButtons();

            if (Player.HasCock())
            {
                AddButton(0, "Common.Button.Anal".Localized(), CowedIntoPuttingItInIntoHerGiantOniButt);

                AddButton(1, "Common.Button.Vaginal".Localized(), FuckhugeOniWantsYourBabiesOrSomeShit);
            }

            if (Player.HasVagina() && !Player.HasCock())
                AddButton(2, "69", LittleChampLittleChampFuckhugeOniIsCominTaEatcha);

            if (!Player.HasCock() && !Player.HasVagina())
                AddButton(0, "Common.Button.Next".Localized(), NoDickNoVagNoService);

            if (OptionStore.Instance.Get("watersports", 0) > 0)
                AddButton(3, "Common.Button.Taunt".Localized(), GetPissedOn);

            AddButton(4, "Common.Button.Hug".Localized(), HugTheOni);

            if (Player.HasVagina() || Player.HasCock())
                AddButton(14, "Common.Button.Leave".Localized(), LossSceneLeave);
        }

        public void LossSceneLeave()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.LossSceneLeave.ExcusedRefusal".Localized());

            OutputText("Scenes.Izumiscene.LossSceneLeave.InvitesYouBack".Localized());

            ClearButtons();

            CombatScene.CleanupAfterCombat();
        }

        public void LossSceneStay()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.LossSceneStay.GropingHerBreast".Localized());

            OutputText("Scenes.Izumiscene.LossSceneStay.SurprisedYelp".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Izumiscene.LossSceneStay.SeductiveWhisper".Localized());
        }

        public void DeathBySnuSnuIMeanGiantOniTits()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IzumiLastEncounter, 1);

            if (Player.IsTaur() || Player.IsDrider() || !Player.HasCock())
            {
                FuckedUpByAFuckhugeOni(true);

                return ;
            }

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.CannotBreakFree".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.PullingHipsBack".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.UniqueHardSensation".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.MurmurBackToReality".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.TooArousedToCare".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.ChidesYouGreedy".Localized());

            if (Player.Tallness <= 60)
            {
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.ArmSlidesUnderYou".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SLetter".Localized());

                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SuckingAtHerTeat".Localized());

                if (Player.BiggestCockLength() <= 5)
                {
                    OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.CuppingBallsTeasing".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.OneOfYourPhrase".Localized());
                    else
                        OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.YourWord".Localized());

                    OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.ManhoodFragment".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SAgain".Localized());

                    OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.BackstrokeShiver".Localized());
                }
            }

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.ThoughtOfHerArousal".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SThird".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.HottestPointBetween".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.HalfStandingPosition".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.OminousPressureBuildup".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.AsksIfYoureClose".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.ExplodesInEcstasy".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.EachOfPhrase".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.IronHardCock".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SFourthLetter".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SplattersInsideArmor".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SFifthLetter".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.HumpingToClimax".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SSixthLetter".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.SoaksYourUnderwear".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.DecoratedUnderwear".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.RelentSuffix".Localized());
            else
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.RelentsWord".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.DeflatingPhrase".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.TheirWord".Localized());
            else
                OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.ItsWord".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.PantingRaiseHead".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.GripsYourBulge".Localized());

            OutputText("Scenes.Izumiscene.DeathBySnuSnuIMeanGiantOniTits.AdmitsUnexpectedFight".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void CowedIntoPuttingItInIntoHerGiantOniButt()
        {

            LossSceneStay();

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.IzumiWhispersDirtyIdea".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.GentlyLaidOnBack".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.SLetter".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.StripsWithGrin".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.MulticockWord".Localized());
            else
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.BiggestCockWord".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.SurveyingHerBody".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.SLetterAgain".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.TwitchAtSight".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.TheyWord".Localized());
            else
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.ItWord".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.CaughtStaringLaugh".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.TauntingReadyRemark".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.AsksEmbarrassed".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.SilentFullPlunge".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.SheepishSelfTalk".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.BucksHipsUpward".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.ArousalCloudsGaze".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.BreathyGruntsEcho".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.HeatKeepsRising".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.PantingBetweenLaughs".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.MisreadsYourSilence".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.ClimaxWithRoar".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.TheCockClause".Localized());

                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.SLetterThird".Localized());

                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.ShallowCocksErupt".Localized());
            }
            else
                OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.SentencePeriod".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.ExtendedJackhammer".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.FinallyReleasesYou".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.HandsOnHipsPose".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.CatchesYouLeaving".Localized());

            OutputText("Scenes.Izumiscene.CowedIntoPuttingItInIntoHerGiantOniButt.ReassureThenLeave".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void FuckhugeOniWantsYourBabiesOrSomeShit()
        {

            LossSceneStay();

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.StraddlesLeaningIn".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.SharedHissJoy".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.AwkwardQuestion".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.RisingToMeetHer".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.SofterThanExpected".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.GenuineSmileNow".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.DispelsGloomyNotion".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.MatchesHerPace".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.BallsWord".Localized());
            else
                OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.LoadWord".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.TwitchingEntry".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.SaggingForBreath".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.AbashedAdmission".Localized());

            OutputText("Scenes.Izumiscene.FuckhugeOniWantsYourBabiesOrSomeShit.HastyFarewellNod".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void NoDickNoVagNoService()
        {

            LossSceneStay();

            CoC.Instance.Flags.Set(GameFlagIds.IzumiSeenPcGender, (int)Player.Gender);

            OutputText("Scenes.Izumiscene.NoDickNoVagNoService.ProbingGroinFrown".Localized());

            OutputText("Scenes.Izumiscene.NoDickNoVagNoService.AbackAtMissingDick".Localized());

            OutputText("Scenes.Izumiscene.NoDickNoVagNoService.HoistedUpsideDown".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Izumiscene.NoDickNoVagNoService.TailWord".Localized());
            else
                OutputText("Scenes.Izumiscene.NoDickNoVagNoService.AnkleWord".Localized());

            OutputText("Scenes.Izumiscene.NoDickNoVagNoService.FeralGrinUndresses".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Izumiscene.NoDickNoVagNoService.GrabsSecondAnkle".Localized());

            OutputText("Scenes.Izumiscene.NoDickNoVagNoService.PeriodThenBreak".Localized());

            OutputText("Scenes.Izumiscene.NoDickNoVagNoService.ConfusedAtYourBody".Localized(
                 this.HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.NoDickNoVagNoService.SetsYouGentlyDown".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void LittleChampLittleChampFuckhugeOniIsCominTaEatcha()
        {

            LossSceneStay();

            if (CoC.Instance.Flags.Get(GameFlagIds.IzumiSeenPcGender) != 2 && !Player.HasCock())
            {
                OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.ProbingSlitFrown".Localized());

                OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.AbackAtMissingCock".Localized());

                if (Player.HasTailInsteadOfLegs())
                    OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.TailWord".Localized());
                else
                    OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.AnkleWord".Localized());

                OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.IronGripTaunt".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.IzumiSeenPcGender, (int)Player.Gender);

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.SpunUpsideDown".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.TailWordAgain".Localized());
            else
                OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.AnkleWordAgain".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.FeralGrinStripsYou".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.LegsSpreadWide".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.PeriodThenBreak".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.IgnoresProtests".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.WrithingPanicJoy".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.ClingingShudders".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.ContinuesDevouring".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.PaceNeverSlows".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.MouthPullsAway".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.BoastsWhilePanting".Localized());

            OutputText("Scenes.Izumiscene.LittleChampLittleChampFuckhugeOniIsCominTaEatcha.GuiltLetsYouRest".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void HugTheOni()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.HugTheOni.CalmExplanation".Localized());

            OutputText("Scenes.Izumiscene.HugTheOni.AwkwardHugIdea".Localized());

            OutputText("Scenes.Izumiscene.HugTheOni.ImmenseArmsWrap".Localized());

            OutputText("Scenes.Izumiscene.HugTheOni.OnlySlightlyWinded".Localized());

            OutputText("Scenes.Izumiscene.HugTheOni.AcceptsHugWarmly".Localized());

            OutputText("Scenes.Izumiscene.HugTheOni.PoliteDeparture".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void GetPissedOn()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.GetPissedOn.IndignantRant".Localized());

            OutputText("Scenes.Izumiscene.GetPissedOn.OniSnapsInRage".Localized((!Player.IsNaked() ? "Izumi pushes you around aggressively as she hurriedly strips you of your gear, bringing you bare naked." : "")));

            OutputText("Scenes.Izumiscene.GetPissedOn.BaresForEmphasis".Localized());

            OutputText("Scenes.Izumiscene.GetPissedOn.TensionFinallyDrains".Localized());

            OutputText("Scenes.Izumiscene.GetPissedOn.UntiesAndReturns".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void TouchThatFluffyHorn()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IzumiLastEncounter, 3);

            CoC.Instance.Flags.Set(GameFlagIds.IzumiTimesGrabbedTheHorn, CoC.Instance.Flags.Get(GameFlagIds.IzumiTimesGrabbedTheHorn) + 1);

            OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.IzumiKneelsForPause".Localized());

            if ((Player.Health / Player.MaxHealth) <= 0.1f)
                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.BriefRecoveryPause".Localized());
            else
                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.OniCheeksTurnRosy".Localized());
                else
                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.StaggeredByOnslaught".Localized());

            OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.YellsHoldIt".Localized());

            OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.OffersADraw".Localized());

            OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.DeclinesHerOffer".Localized());

            OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.IzumiAsksYourLuck".Localized());

            if (silly)
            {
                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.IzumiStompsGround".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.RoarGrowsLouder".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.EnergyWavesBuild".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.DashesAtIzumi".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.BlushDeepens".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.AuraDissipates".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.QuietlyAsksRelease".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.SwallowsHardStaring".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.ThumbRollsHorn".Localized());
            }
            else
            {
                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.IzumiStompsGroundAgain".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.RoarGrowsLouderAgain".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.StrongerThanClaimed".Localized());

                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                {
                    CombatController.Instance!.Resolver.PlayerTease();

                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.IgnoresDistraction".Localized());
                }
                else
                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.CatchHerOffGuard".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.CrazedEyesLockOn".Localized());

                if (Player.Speed >= 90)
                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.UppercutGrabDodged".Localized());
                else
                    if (Player.Strength >= 90)
                        OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.GrabbedByThroat".Localized());
                    else
                        if (Player.Intelligence >= 75)
                            OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.WrenchedOffFloor".Localized());
                        else
                            if (Player.Toughness >= 90)
                                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.LiftedOffFloor".Localized());
                            else
                                if (Player.Libido >= 40 && (Player.BiggestBreastSize() >= 2 || Player.BiggestCockLength() >= 5))
                                {
                                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.WrenchedOffFloorAgain".Localized());

                                    if (Player.BiggestBreastSize() > 2)
                                        OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.SmotherWithChest".Localized());
                                    else
                                        OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.BaresCockQuickly".Localized());

                                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.ForceGazeToAssets".Localized());
                                }
                                else
                                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.SwingsWhileChoked".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.ExpressionChanges".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.VisageDrops".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.QuietlySaysUm".Localized());

                if (Player.Speed < 90 || Player.Strength < 90)
                {
                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.SinksToHerKnees".Localized());

                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.PleaseLetGo".Localized());
                }
                else
                    OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.PleaseReleaseGrip".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.WondersAtChange".Localized());

                OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.ThumbRollsHornAgain".Localized());
            }

            OutputText("Scenes.Izumiscene.TouchThatFluffyHorn.CouldLetHerGo".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Tribhorn".Localized(), null);

            AddButton(2, "Common.Button.Letsfuck".Localized(), null);

            AddButton(3, "Common.Button.Revengeanal".Localized(), null);

            AddButton(4, "Common.Button.Revengeoral".Localized(), null);

            AddButton(5, "Common.Button.Tentacruel".Localized(), null);

            AddButton(0, "Common.Button.LetGo".Localized(), LetGoAndLeaveYouSillyFuck);

            CoC.Instance.Flags.Set(GameFlagIds.IzumiSeenPcGender, (int)Player.Gender);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.Tribhorn".Localized(), TribThatRockyHornGirl);

            if (Player.HasCock())
            {
                AddButton(2, "Common.Button.Letsfuck".Localized(), LetGoAndFuck);

                AddButton(3, "Common.Button.Revengeanal".Localized(), GonnaGetMeSomeRevengeButtsexin);

                AddButton(4, "Common.Button.Revengeoral".Localized(), SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt);

                if (Player.CountCocksOfType("TENTACLE") >= 8)
                    AddButton(5, "Common.Button.Tentacruel".Localized(), TentacruelRevengeFux);
            }
        }

        public void LetGoAndLeaveYouSillyFuck()
        {
            ClearOutput();

            if (Player.Libido >= 50)
                OutputText("Scenes.Izumiscene.LetGoAndLeaveYouSillyFuck.AgainstLibido".Localized());
            else
                OutputText("Scenes.Izumiscene.LetGoAndLeaveYouSillyFuck.YLetter".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndLeaveYouSillyFuck.LetIzumiGoPrefix".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndLeaveYouSillyFuck.GivesGemsHurriedly".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndLeaveYouSillyFuck.StowsGemsAway".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndLeaveYouSillyFuck.BitesLipFrowns".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndLeaveYouSillyFuck.VanishesInSmoke".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void LetGoAndFuck()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.LetGoAndFuck.HornReleaseTruce".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.GrabsSleeveInterrupt".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.AdmitsYouWon".Localized());

            if (Player.Tallness < 80)
                OutputText("Scenes.Izumiscene.LetGoAndFuck.DownWord".Localized());
            else
                if (Player.Tallness > 108)
                    OutputText("Scenes.Izumiscene.LetGoAndFuck.UpWord".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.HandsOnHipsTaunt".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.IndulgentEyeRoll".Localized());

            if (Player.Tallness < 80)
                OutputText("Scenes.Izumiscene.LetGoAndFuck.FromAboveSuffix".Localized());
            else
                if (Player.Tallness > 108)
                    OutputText("Scenes.Izumiscene.LetGoAndFuck.FromBelowSuffix".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.NippleToMouth".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.ShowsBothBreasts".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.DetachesFromBreasts".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.SuggestsNewPosition".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.LasciviousTaunt".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.AdvancesOnIzumi".Localized(
                 this.HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.LetGoAndFuck.FirstThrustInside".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.ReclinesUnderYou".Localized());

            if (Player.BiggestBreastSize() >= 1 && Player.BiggestBreastSize() < 3)
                OutputText("Scenes.Izumiscene.LetGoAndFuck.SmothersChestWithPair".Localized());
            else
                if (Player.BiggestBreastSize() >= 3 && Player.BiggestBreastSize() < 5)
                    OutputText("Scenes.Izumiscene.LetGoAndFuck.ChestMoldsAgainstOni".Localized());
                else
                    if (Player.BiggestBreastSize() >= 5 && Player.BiggestBreastSize() <= 8)
                        OutputText("Scenes.Izumiscene.LetGoAndFuck.ChestCreatesValley".Localized());
                    else
                        if (Player.BiggestBreastSize() > 8)
                            OutputText("Scenes.Izumiscene.LetGoAndFuck.WinsSpaceBattle".Localized());
                        else
                            OutputText("Scenes.Izumiscene.LetGoAndFuck.PressedIntoHerChest".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.UrgesFasterPace".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.HoldsBackTeasing".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.InsistsOnFaster".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.MoansSweepingHair".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.LongSlowRhythm".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.QuietHornRequest".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.BitesLipHornAsk".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.OverwhelmedByHorn".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.HornTouchClench".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Izumiscene.LetGoAndFuck.AgainstYourOwnSuffix".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.GaspsForAirSuffix".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.WarnsOfClimax".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.CumsInsideHer".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.AfterglowCollapse".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.RegretfulClimax".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.CheersHerUp".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.StammeredGratitude".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.DropsTheSubject".Localized());

            OutputText("Scenes.Izumiscene.LetGoAndFuck.WinkedThankYou".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void GonnaGetMeSomeRevengeButtsexin()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.OrderHerToDisrobe".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.StripsWhileProtesting".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.FlusteredAndNaked".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.NoteHerFineBody".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.TestHerCompliance".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ScowlThenConcede".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.OrderArmsAgainstWall".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.CaressHerHorn".Localized());

            OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.QuietThenYields".Localized());

            if (Player.BiggestCockLength() <= 4)
            {
                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.BareYourselfToHer".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.DoubtsYourIntent".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.PoisedBehindHer".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheFeelsItEnter".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.PoundInFury".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.NoiseFuelsAnger".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.BoredSighStokesAnger".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheWarnsHimOff".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.StrikeHerHead".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.OutrageAmusesYou".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.DemandSizePraise".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ObjectCutBySlap".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheAdmitsItsBig".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.PressForDetails".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ThreatenWithHand".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SootheAndUrge".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.GrudgingAcquiescence".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.FakeGroansAmuse".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.MonotoneMockery".Localized());

                if (Player.Corruption >= 50)
                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ApathyArousesYou".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.DrapeOverHerBack".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.PoundFromAbove".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheMocksStamina".Localized());

                if (Player.CumQ() >= 200)
                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.LargeOrgasmContrast".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ParagraphBreak".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SensesTheSurge".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.StaggerBackSpent".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheDeniesItHappened".Localized());

                OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.GrabGemPurse".Localized());
            }
            else
                if (Player.BiggestCockLength() > 4 && Player.BiggestCockLength() <= 12)
                {
                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ContemptuousGrunt".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ProtestsWrongHole".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.IgnoreHerAndPush".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SurprisedAtFullDepth".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.BallsWord".Localized());
                    else
                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.HiltWord".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.EnduresDeepEntry".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.EstablishSlowPace".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.MarvelAtHerBack".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheRejectsRomance".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SeizeHerMane".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ChokesInSurprise".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SweatBeadsForm".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SinkToTheDeepest".Localized());

                    if (Player.CumQ() < 1500)
                    {
                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.RelaxAfterOrgasm".Localized());

                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.UnashamedDefense".Localized());

                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.TooTiredToArgue".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.EndlessSpunkFlow".Localized());

                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.AlarmedQuaver".Localized());

                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.RefuseToPullOut".Localized());

                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.WithdrawAndGush".Localized());

                        OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.AbandonHerSpent".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.FaceFallsAtSight".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.DeniesWhatSheSees".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ShePanicsAtSize".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.LineUpForEntry".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.PleadsThenYelps".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.PityHerStrain".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheNearlyCries".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.PuzzleAtSmallClaim".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.GratefulSniffle".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SavourTheSqueeze".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.GentlenessWinsHer".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SheAsksForFaster".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ReachForHerBreasts".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SnapHipsForward".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.HerProtestsFail".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.ButtMusclesClamp".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.MilkedInHerTrap".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SpentAndWithdraw".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.SleepyMurmur".Localized());

                    OutputText("Scenes.Izumiscene.GonnaGetMeSomeRevengeButtsexin.FindHerAsleep".Localized());
                }

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void TentacruelRevengeFux()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.ReleaseGripOnHorn".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.StammerAtWrithingCrotch".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.YellPanicStraining".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.StiffenEyesWideSqueak".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.EyesRollBackGrunting".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.PullLegsApartExpose".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.PipeUpForcedJoviality".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.AdmitAppealDecide".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.DangerAvertedEnjoyFuck".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.NotSatisfiedMainEvent".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.RelaxFlushedFogged".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 10)
                OutputText("Scenes.Izumiscene.TentacruelRevengeFux.TestTheorySnakePalms".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.OneHoleLeftUnfinished".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.PlungeHardEyeShut".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.PoundRelentlessly".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.ArchBackFloodHoles".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.OrgasmSubsidesCummyMess".Localized());

            OutputText("Scenes.Izumiscene.TentacruelRevengeFux.FindGemsInJunk".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.EvilIdeaYankHorn".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.DirectKneelGrowl".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.StrokeHornOnWhim".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.FullyErectLockedGaze".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.OrderKissItFlicker".Localized());

            var cockIndex = Player.BiggestCockIndex();

            if (Player.Cocks[cockIndex].Id == BodyPartTypes.Horse)
                OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.UnsureBrushLipsCenter".Localized());
            else
                if (new[] { BodyPartTypes.Dog, BodyPartTypes.Wolf, BodyPartTypes.Fox }.Contains(Player.Cocks[cockIndex].Id))
                    OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.DartsForwardTakeTip".Localized());
                else
                    OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.FeelSoftWarmLips".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.ParagraphBreak".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.PhaseThreeYankThrust".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.CommandSuckItTears".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.LatchLikeVacuumWeak".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.YankForwardBuryDeep".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.OrderStopConfusion".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.OralAssaultFalters".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.SubservientEnoughResume".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.StammerHazilyJerkHorn".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.BetterAngleDeeperThroat".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.AmusingPlatformBonus".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.PositionSecuredSlam".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.MinorTwitchVisiblyShaking".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.LeanBackSurprised".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.BullishOniHumpingAir".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.SurpriseDeepensFlooding".Localized());

            if (Player.CumQ() > 100)
            {
                OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.HipsShootUpMilk".Localized());

                OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.PaintLeonineFeatures".Localized());
            }

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.BothDoneRemain".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.MutterClenchHands".Localized());

            OutputText("Scenes.Izumiscene.SayThatsAPrettyFaceBeAShameIfSomebodyJizzedAllOverIt.NoticeLeatherBag".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void TribThatRockyHornGirl()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.RespondToBribeRequest".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.MuttersBeforePushedDown".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.StammerWinceRockyHorn".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.YammersWaitTerror".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.SmirkAtMisplacedTerror".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.ExtricateScootForward".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.BumpyHornAppreciativeHiss".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.OtherConcernsNewToy".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.FeelShiftBreathyGrunt".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.AmusedLaughMuscleGirl".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.AskHavingFunBlush".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.StammerBlameTouching".Localized());

            if (Player.Corruption > 80)
                OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.ShootBackLittleSlut".Localized());
            else
                OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.IgnoreProtestsReturn".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.SomethingMissingToying".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.ShiftLeanBumpyWarmth".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.FrigInEarnest".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.ProudOniWantonThought".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.OrgasmBeforeExperiment".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.HipsDropFinalThump".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.AskCameTrembles".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.CroakNearTears".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.AnnounceBagOfGems".Localized());

            OutputText("Scenes.Izumiscene.TribThatRockyHornGirl.CampWhistleMood".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void IzumiContest()
        {
            ClearOutput();

            Player.ChangeFatigue(30);

            if (Save.GetFloat("contestStage") == 0)
            {
                OutputText("Scenes.Izumiscene.IzumiContest.HoldHandSuggestIdea".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.SmirkBeatMeHere".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.ProposeLiftingContest".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.HeartyLaughNotBetter".Localized());
            }

            if (Save.GetFloat("contestStage") % 1 == 0)
            {
                OutputText("Scenes.Izumiscene.IzumiContest.ScanCaveForBoulder".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.GripBoulderHeave".Localized());
            }

            if (Player.Strength < 40)
            {
                OutputText("Scenes.Izumiscene.IzumiContest.RockImmobilePremonition".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.PantingLoudSlapBack".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.SnortsPatsBack".Localized(
                     HeightDesc()
                ));

                OutputText("Scenes.Izumiscene.IzumiContest.TenderOrPatronizing".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.LastSnickerByFire".Localized());

                Save.SetFloat("contestStage", .5f);

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Izumiscene.IzumiContest.LiftWithAllStrength".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.MockHunkEyeRoll".Localized());

                OutputText("Scenes.Izumiscene.IzumiContest.AskSoWhatNow".Localized());

                Save.SetFloat("contestStage", 1);

                ClearButtons();

                AddNextButton("Scenes.Kitsune.LoseToBlackHairLatexWhileMilky.WordAnother".Localized(), IzumiSecondRock);

                AddNextButton("Common.Button.Stop".Localized(), IzumiRockStop);
            }
        }

        public void IzumiRockStop()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.IzumiRockStop.TellEnoughForNow".Localized());

            OutputText("Scenes.Izumiscene.IzumiRockStop.BlinkGrinsPathetic".Localized());

            OutputText("Scenes.Izumiscene.IzumiRockStop.ApproachImpishFire".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Fight".Localized(), FightTheFuckhugeOni);

            AddNextButton("Scenes.ErlKing.RepeatWildHuntAWinnerIsYou.ButtonSurrender".Localized(), FuckhugeOniGetsWhatSheWants);

            AddNextButton("Common.Button.Flee".Localized(), FuckinChegIt);
        }

        public void IzumiSecondRock()
        {
            ClearOutput();

            if (Save.GetFloat("contestStage") % 1 == 0)
            {
                OutputText("Scenes.Izumiscene.IzumiSecondRock.UndeterredResolve".Localized());

                OutputText("Scenes.Izumiscene.IzumiSecondRock.SuitableBoulderFound".Localized());
            }

            OutputText("Scenes.Izumiscene.IzumiSecondRock.InvitationToPerform".Localized());

            Player.ChangeFatigue(30);

            if (Player.Strength < 70)
            {
                OutputText("Scenes.Izumiscene.IzumiSecondRock.StrainingEffort".Localized());

                OutputText("Scenes.Izumiscene.IzumiSecondRock.MusclesFail".Localized());

                OutputText("Scenes.Izumiscene.IzumiSecondRock.SmugSatisfaction".Localized());

                Save.SetFloat("contestStage", 1.5f);

                DoNext(() => FuckhugeOniGetsWhatSheWants());
            }
            else
            {
                OutputText("Scenes.Izumiscene.IzumiSecondRock.EffortTakingHold".Localized());

                OutputText("Scenes.Izumiscene.IzumiSecondRock.FocusUnderLoad".Localized());

                OutputText("Scenes.Izumiscene.IzumiSecondRock.HiddenImpression".Localized());

                OutputText("Scenes.Izumiscene.IzumiSecondRock.EffortlessThrow".Localized());

                OutputText("Scenes.Izumiscene.IzumiSecondRock.EagerForNext".Localized(
                     HeightDesc()
                ));

                Save.SetFloat("contestStage", 2);

                ClearButtons();

                AddNextButton("Common.Button.OneMore".Localized(), () => IzumiThirdRock());

                AddNextButton("Common.Button.Stop".Localized(), IzumiRockStop);
            }
        }

        public void IzumiThirdRock(bool first = true)
        {
            ClearOutput();

            if (Save.GetFloat("contestStage") % 1 == 0)
            {
                OutputText("Scenes.Izumiscene.IzumiThirdRock.RisingAmbition".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.ProvocativeGrin".Localized());
            }

            Player.ChangeFatigue(30);

            if (Player.Strength < 95)
            {
                OutputText("Scenes.Izumiscene.IzumiThirdRock.MaximumExertion".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.SurpassingLimits".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.ImpressedConcern".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.BreathBeforeLift".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.PantingGaze".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.FlirtatiousApproach".Localized());

                Save.SetFloat("contestStage", 2.5f);

                DoNext(() => FuckhugeOniGetsWhatSheWants());
            }
            else
            {
                OutputText("Scenes.Izumiscene.IzumiThirdRock.ColossalRock".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.ImmovableWeight".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.CrushingPressure".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.SavedByEmbrace".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.FlusteredBlush".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.BoulderEmbrace".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.GradualAscent".Localized());

                OutputText("Scenes.Izumiscene.IzumiThirdRock.CatchingBreath".Localized());

                Save.SetFloat("contestStage", 3);

                ClearButtons();

                AddNextButton("Common.Button.LiftHer".Localized(), IzumiLift);

                AddNextButton("Common.Button.ItsADraw".Localized(), IzumiDraw);
            }
        }

        public void IzumiDraw()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.IzumiDraw.MutualRespect".Localized());

            OutputText("Scenes.Izumiscene.IzumiDraw.UnexpectedFeat".Localized(
                 HeightDesc()
            ));

            OutputText("Scenes.Izumiscene.IzumiDraw.FiresideRelaxation".Localized());

            OutputText("Scenes.Izumiscene.IzumiDraw.PartingFarewell".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void IzumiLift()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.IzumiLift.AssessingOpponent".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.SearchingForChallenge".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.UnexpectedLift".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.DodgingSwings".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.FuriousBark".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.AskingPrize".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.DenyingVictory".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.RhetoricalTrap".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.StunnedDefeat".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.GrudgingConcession".Localized());

            OutputText("Scenes.Izumiscene.IzumiLift.CherishedMemory".Localized());

            Player.ChangeFatigue(30);

            CoC.Instance.Flags.Set(GameFlagIds.IzumiSeenPcGender, (int)Player.Gender);

            ClearButtons();

            if (Player.HasCock())
            {
                AddButton(0, "Common.Button.Anal".Localized(), CowedIntoPuttingItInIntoHerGiantOniButt);

                AddButton(1, "Common.Button.Vaginal".Localized(), FuckhugeOniWantsYourBabiesOrSomeShit);
            }

            if (Player.HasVagina() && !Player.HasCock())
                AddButton(2, "69", LittleChampLittleChampFuckhugeOniIsCominTaEatcha);

            if (!Player.HasCock() && !Player.HasVagina())
                AddButton(0, "Common.Button.Next".Localized(), NoDickNoVagNoService);

            if (OptionStore.Instance.Get("watersports", 0) > 0)
                AddButton(3, "Common.Button.Taunt".Localized(), GetPissedOn);

            AddButton(4, "Common.Button.Hug".Localized(), HugTheOni);

            if (Player.HasVagina() || Player.HasCock())
                AddButton(14, "Common.Button.Leave".Localized(), LossSceneLeave);
        }

        public void IzumiRematch()
        {
            ClearOutput();

            OutputText("Scenes.Izumiscene.IzumiRematch.DemandingRematch".Localized());

            OutputText("Scenes.Izumiscene.IzumiRematch.RivalPrideTaunt".Localized());

            if (Save.GetFloat("contestStage") % 1 == .5f)
                OutputText("Scenes.Izumiscene.IzumiRematch.SilentThanks".Localized());

            switch ((float)(int)Math.Floor(Save.GetFloat("contestStage")))
            {
                case 0:
                {
                DoNext(() => IzumiContest());
                }

                break;

                case 1:
                {
                DoNext(() => IzumiSecondRock());
                }

                break;

                case 2:
                {
                DoNext(() => IzumiThirdRock());
                }

                break;
            }
        }

    }
}