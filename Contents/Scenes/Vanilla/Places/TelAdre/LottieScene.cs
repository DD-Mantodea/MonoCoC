using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<LottieScene>("coc:lottie")]
    public class LottieScene : Scene
    {

        public override void Entry()
        {

        }

        public Action? LottieAppearance(bool display = true)
        {

            if (!Player.HasCock())
                return null;

            if (CoC.Instance.Flags.Get(GameFlagIds.MetMarae) <= 0)
                return null;

            if (View.Time.Hours >= 15 && View.Time.Hours <= 23)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) == 0)
                {
                    if (display)
                        OutputText("Scenes.Lottie.LottieAppearance.PigLadyEmergesPool".Localized());

                    return EncounterLottieCharge;
                }
                else
                {
                    if (display)
                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) < 30 && Random.Shared.Next(6) == 0)
                        {
                            OutputText("Scenes.Lottie.LottieAppearance.CryingInSauna".Localized());

                            if (silly)
                                OutputText("Scenes.Lottie.LottieAppearance.LoveMakingBacon".Localized());
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) < 30 && Random.Shared.Next(5) == 0)
                            {
                                OutputText("Scenes.Lottie.LottieAppearance.CryingAtCafe".Localized());

                                if (silly)
                                    OutputText("Scenes.Lottie.LottieAppearance.BestPorkBuns".Localized());
                            }
                            else
                                if (Random.Shared.Next(2) == 0)
                                {
                                    OutputText("Scenes.Lottie.LottieAppearance.ExitingYogaRoom".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 33)
                                        OutputText("Scenes.Lottie.LottieAppearance.Voluptuous".Localized());
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 66)
                                            OutputText("Scenes.Lottie.LottieAppearance.Plump".Localized());
                                        else
                                            OutputText("Scenes.Lottie.LottieAppearance.Curvy".Localized());

                                    OutputText("Scenes.Lottie.LottieAppearance.JogsJigglingBreasts".Localized(
                                        LottieCup()
                                    ));

                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 33)
                                        OutputText("Scenes.Lottie.LottieAppearance.Bountiful".Localized());
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 66)
                                            OutputText("Scenes.Lottie.LottieAppearance.ChubbyAgain".Localized());
                                        else
                                            OutputText("Scenes.Lottie.LottieAppearance.CuteAgain".Localized());

                                    OutputText("Scenes.Lottie.LottieAppearance.RumpAgain".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Lottie.LottieAppearance.LeavingWeightRoom".Localized(
                                        LottieCup()
                                    ));

                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 33)
                                        OutputText("Scenes.Lottie.LottieAppearance.Voluminous".Localized());
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 66)
                                            OutputText("Scenes.Lottie.LottieAppearance.ChubbyAgain".Localized());
                                        else
                                            OutputText("Scenes.Lottie.LottieAppearance.CuteAgain".Localized());

                                    OutputText("Scenes.Lottie.LottieAppearance.RumpAgain".Localized());
                                }

                    return EncounterLottieCharge;
                }
            }
            else
                return null;
        }

        public string LottieCup()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) >= 75)
                return "D";
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) >= 50)
                    return "E";
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) >= 25)
                        return "F";
                    else
                        return "G";
        }

        public void EncounterLottieCharge()
        {

            EncounterLottie(true);
        }

        public void EncounterLottie(bool charge = false)
        {
            ClearOutput();

            ShowImage("lottie-encounter");

            if ((charge) && (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0))
            {
                OutputText("Scenes.Lottie.EncounterLottie.DoorFeeTenGems".Localized());

                Player.Gems -= 10;

            }

            CoC.Instance.Flags.Set(GameFlagIds.LottieEncounterCounter, CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.LottieFigure, 50);

                CoC.Instance.Flags.Set(GameFlagIds.LottieConfidence, 50);

                CoC.Instance.Flags.Set(GameFlagIds.LottieLastEncouragementState, 50);

                CoC.Instance.Flags.Set(GameFlagIds.LottieWeightConclusion, 50);

                OutputText("Scenes.Lottie.EncounterLottie.ApproachTowelWrappedGirl".Localized());

                OutputText("Scenes.Lottie.EncounterLottie.BlurtsHello".Localized());

                OutputText("Scenes.Lottie.EncounterLottie.RamblesPastAttempts".Localized());

                if (!Player.HasTailInsteadOfLegs() && !Player.IsTaur() && Player.Tone >= 50)
                {
                    OutputText("Scenes.Lottie.EncounterLottie.BlushesLooksDown".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), AcceptBeingLottiesHamSandwich);

                    AddButton(1, "Common.Button.No".Localized(), DeclineBeingLottiesGymHamsandwich);
                }
                else
                {
                    OutputText("Scenes.Lottie.EncounterLottie.GrinYogaClassWait".Localized());

                    OutputText("Scenes.Lottie.EncounterLottie.DecidesReturnHome".Localized());

                    ReturnToCamp(1);
                }

                return;
            }
            else
            {
                OutputText("Scenes.Lottie.EncounterLottie.ApproachPigGirl".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) >= 50)
                    OutputText("Scenes.Lottie.EncounterLottie.Relaxing".Localized());
                else
                    OutputText("Scenes.Lottie.EncounterLottie.SittingTimidly".Localized());

                OutputText("Scenes.Lottie.EncounterLottie.LowCutBootyShorts".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 50)
                    OutputText("Scenes.Lottie.EncounterLottie.Plump".Localized());
                else
                    OutputText("Scenes.Lottie.EncounterLottie.Curvy".Localized());

                OutputText("Scenes.Lottie.EncounterLottie.Figure".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieLovesHerBody) == 0)
                    OutputText("Scenes.Lottie.EncounterLottie.LightlySipping".Localized());
                else
                    OutputText("Scenes.Lottie.EncounterLottie.SlurpingDown".Localized());

                OutputText("Scenes.Lottie.EncounterLottie.AppearsToBe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieLovesHerBody) == 0)
                    OutputText("Scenes.Lottie.EncounterLottie.SomeJuice".Localized());
                else
                    OutputText("Scenes.Lottie.EncounterLottie.Milkshake".Localized());

                OutputText("Scenes.Lottie.EncounterLottie.SpotsYouAndWaves".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) >= 70)
                    OutputText("Scenes.Lottie.EncounterLottie.Excitedly".Localized());

                OutputText("Scenes.Lottie.EncounterLottie.SitsAndListens".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieTrainer) == 0 && (Player.HasTailInsteadOfLegs() || Player.IsTaur() || Player.Tone < 50))
                {
                    OutputText("Scenes.Lottie.EncounterLottie.GladTalkedAboutHerself".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Leave".Localized(), SceneInstance<GymScene>.Get().GymDesc);

                    return;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieTrainer) == 0)
                    {
                        OutputText("Scenes.Lottie.EncounterLottie.AtLossOfWords".Localized(
                            Player.Mf("big guy", "babe")
                        ));

                        OutputText("Scenes.Lottie.EncounterLottie.WellAreYou".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Yes".Localized(), GotInShapeAcceptTrainingOffer);

                        AddButton(1, "Common.Button.No".Localized(), GotInShapeButTurnedDownLottie);

                        if (silly && (Player.IsCorruptEnough(80) || Player.HasPerk(PerkIds.Sadist)))
                            AddButton(2, "Common.Button.GodNo".Localized(), GotInShapeComedyDeclineLottie);

                        return;
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) == 2 && CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) > 0)
                            OutputText("Scenes.Lottie.EncounterLottie.EnjoyedWhatWeDid".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) == 2)
                                OutputText("Scenes.Lottie.EncounterLottie.GladDoingThis".Localized());
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) == 4 && CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) == 0)
                                {
                                    OutputText("Scenes.Lottie.EncounterLottie.NiceSeeYouAgain".Localized());

                                    OutputText("Scenes.Lottie.EncounterLottie.TakeHerUp".Localized());

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Sex".Localized(), FourthMeetingChooseSexWithLottie);

                                    AddButton(1, "Common.Button.Buddies".Localized(), StayLottieBuds4Evah);

                                    return;
                                }
                                else
                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncouragementThreshold3130Down) == 0 && CoC.Instance.Flags.Get(GameFlagIds.LottieLastEncouragementState) > 30 && CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) <= 30)
                                    {
                                        OutputText("Scenes.Lottie.EncounterLottie.WhatsGottenIntoYou".Localized());

                                        CoC.Instance.Flags.Set(GameFlagIds.LottieEncouragementThreshold3130Down, 1);

                                        CoC.Instance.Flags.Set(GameFlagIds.LottieLastEncouragementState, CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence));
                                    }
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncouragementThreshold3130Down) > 0 && CoC.Instance.Flags.Get(GameFlagIds.LottieLastEncouragementState) > 30 && CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) <= 30)
                                        {
                                            OutputText("Scenes.Lottie.EncounterLottie.NoticedYourActing".Localized());

                                            CoC.Instance.Flags.Set(GameFlagIds.LottieLastEncouragementState, CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence));
                                        }
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieLastEncouragementState) <= 30 && CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) > 30)
                                            {
                                                CoC.Instance.Flags.Set(GameFlagIds.LottieLastEncouragementState, CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence));

                                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncouragementThreshold3031Up) == 0)
                                                {
                                                    OutputText("Scenes.Lottie.EncounterLottie.GladBeingNiceAgain".Localized());

                                                    CoC.Instance.Flags.Set(GameFlagIds.LottieEncouragementThreshold3031Up, 1);
                                                }
                                                else
                                                    OutputText("Scenes.Lottie.EncounterLottie.ThanksBrighteningUp".Localized());
                                            }
                                            else
                                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieLastEncouragementState) > 80 && CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) <= 80)
                                                {
                                                    CoC.Instance.Flags.Set(GameFlagIds.LottieLastEncouragementState, CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence));

                                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncouragementThreshold8079Down) == 0)
                                                    {
                                                        CoC.Instance.Flags.Set(GameFlagIds.LottieEncouragementThreshold8079Down, 1);

                                                        OutputText("Scenes.Lottie.EncounterLottie.SomethingWrong".Localized());
                                                    }
                                                    else
                                                        OutputText("Scenes.Lottie.EncounterLottie.StopActingLikeThat".Localized());
                                                }
                                                else
                                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieLastEncouragementState) <= 80 && CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) > 80)
                                                    {
                                                        CoC.Instance.Flags.Set(GameFlagIds.LottieLastEncouragementState, CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence));

                                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncouragementThreshold7980Up) == 0)
                                                        {
                                                            CoC.Instance.Flags.Set(GameFlagIds.LottieEncouragementThreshold7980Up, 1);

                                                            OutputText("Scenes.Lottie.EncounterLottie.NervousFidgeting".Localized(
                                                                Player.ShortName,

                                                                Player.ShortName
                                                            ));

                                                            OutputText("Scenes.Lottie.EncounterLottie.DontKnowAnswer".Localized());
                                                        }
                                                        else
                                                            OutputText("Scenes.Lottie.EncounterLottie.ThanksForBeingNicer".Localized());
                                                    }
                                                    else
                                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) >= 60)
                                                        {
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 20 && CoC.Instance.Flags.Get(GameFlagIds.LottieLovesHerBody) == 0)
                                                                OutputText("Scenes.Lottie.EncounterLottie.LetsDoOurBest".Localized());
                                                            else
                                                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 20)
                                                                    OutputText("Scenes.Lottie.EncounterLottie.GrinsAndGrabsHerself".Localized(
                                                                        Player.Mf("stud", "cutie")
                                                                    ));
                                                                else
                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 40 && CoC.Instance.Flags.Get(GameFlagIds.LottieLovesHerBody) == 0)
                                                                        OutputText("Scenes.Lottie.EncounterLottie.GetStartedSoon".Localized());
                                                                    else
                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 40)
                                                                            OutputText("Scenes.Lottie.EncounterLottie.GrinsPunchesArm".Localized(
                                                                                Player.Mf("stud", "cutie")
                                                                            ));
                                                                        else
                                                                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 60)
                                                                                OutputText("Scenes.Lottie.EncounterLottie.WasLookingForYou".Localized());
                                                                            else
                                                                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 80)
                                                                                    OutputText("Scenes.Lottie.EncounterLottie.LeansOverCreamyTits".Localized(
                                                                                        Player.Mf("big guy", "babe")
                                                                                    ));
                                                                                else
                                                                                    OutputText("Scenes.Lottie.EncounterLottie.WinksPunches".Localized(
                                                                                        Player.Mf("stud", "cutie")
                                                                                    ));
                                                        }
                                                        else
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) >= 30)
                                                            {
                                                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 20)
                                                                    OutputText("Scenes.Lottie.EncounterLottie.ShouldTryToday".Localized());
                                                                else
                                                                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 40)
                                                                        OutputText("Scenes.Lottie.EncounterLottie.GetStartedSoonAgain".Localized());
                                                                    else
                                                                        if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 60)
                                                                            OutputText("Scenes.Lottie.EncounterLottie.AtEaseNow".Localized());
                                                                        else
                                                                            OutputText("Scenes.Lottie.EncounterLottie.BeenWaitingForYou".Localized());
                                                            }
                                                            else
                                                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) <= 70)
                                                                    OutputText("Scenes.Lottie.EncounterLottie.HaventBeenTrying".Localized());
                                                                else
                                                                    OutputText("Scenes.Lottie.EncounterLottie.DontLikeLookingMe".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), LotteryAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), TalkToPigSlut);

            AddButton(2, "Common.Button.Exercise".Localized(), LottieExercise);

            AddButton(3, "Common.Button.GiveItem".Localized(), GiveLottieAnItem);

            if (Player.Lust >= 33)
                AddButton(4, "Common.Button.Sex".Localized(), LottieFuck);
            else
                AddButton(4, "Common.Button.Sex".Localized(), null);

            AddButton(5, "Common.Button.Hug".Localized(), HugTheShitOutOfYourHam);

            AddButton(14, "Common.Button.Leave".Localized(), SceneInstance<GymScene>.Get().GymDesc);

            CoC.Instance.Flags.Set(GameFlagIds.LottieLastEncouragementState, LottieMorale());

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.LottieWeightConclusion, LottieTone());
        }

        public void FourthMeetingChooseSexWithLottie()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.FourthMeetingChooseSexWithLottie.FineFollowYourLead".Localized(
                Player.CockDescript(x),

                Player.Mf("guy", "lady"),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.FourthMeetingChooseSexWithLottie.DecidedVigorousWorkout".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), GetSuckedOffByAHamSandwich);

            AddButton(1, "Common.Button.Anal".Localized(), AnalLottieMilkshake);
        }

        public void StayLottieBuds4Evah()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.StayLottieBuds4Evah.StayBuddiesInstead".Localized());

            OutputText("Scenes.Lottie.StayLottieBuds4Evah.SitAtGymEntrance".Localized(
                Player.Mf("big guy", "babe")
            ));

            ReturnToCamp(1);
        }

        public void DeclineBeingLottiesGymHamsandwich()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.DeclineBeingLottiesGymHamsandwich.ChucklesNervouslyUnderstands".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieDisabled, 1);

            ReturnToCamp(1);
        }

        public void AcceptBeingLottiesHamSandwich()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LottieTrainer, 1);

            OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.AssureHerSerious".Localized(
                Player.Mf("big guy", "babe"),

                Player.ShortName
            ));

            OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.GoodLookAtLottie".Localized());

            if (Player.Tallness > 60)
                OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.ShorterThanYou".Localized());
            else
                if (Player.Tallness > 53)
                    OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.SameHeightAsYou".Localized());
                else
                    OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.TallerThanYou".Localized());

            OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.ChubbierThanMostGirls".Localized());

            OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.BoldEllipsisParagraph".Localized());

            OutputText("Scenes.Lottie.AcceptBeingLottiesHamSandwich.HourLaterRunningTrack".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Water".Localized(), DrinkWaterWithYourHamhock);

            AddButton(1, "Common.Button.Shake".Localized(), DrinkLustWithYourHamhock);
        }

        public void GotInShapeButTurnedDownLottie()
        {

            OutputText("Scenes.Lottie.GotInShapeButTurnedDownLottie.ChucklesNervouslyUnderstands".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieDisabled, 1);

            ReturnToCamp(1);
        }

        public void GotInShapeComedyDeclineLottie()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.GotInShapeComedyDeclineLottie.OfCourseFuckingArent".Localized(
                Player.CockDescript(Player.BiggestCockIndex()),

                Player.CockDescript(Player.BiggestCockIndex())
            ));

            CoC.Instance.Flags.Set(GameFlagIds.LottieDisabled, 1);

            ReturnToCamp(1);
        }

        public void GotInShapeAcceptTrainingOffer()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.LottieTrainer, 1);

            OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.AssureHerSerious".Localized(
                Player.Mf("big guy", "babe")
            ));

            OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.GoodLookAtLottie".Localized());

            if (Player.Tallness > 60)
                OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.ShorterThanYou".Localized());
            else
                if (Player.Tallness > 53)
                    OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.SameHeightAsYou".Localized());
                else
                    OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.TallerThanYou".Localized());

            OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.ChubbierThanMostGirls".Localized());

            OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.EllipsisParagraph".Localized());

            OutputText("Scenes.Lottie.GotInShapeAcceptTrainingOffer.HalfLaterRunningTrack".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Water".Localized(), DrinkWaterWithYourHamhock);

            AddButton(1, "Common.Button.Shake".Localized(), DrinkLustWithYourHamhock);
        }

        public void DrinkWaterWithYourHamhock()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.DrinkWaterWithYourHamhock.DrinkingConcoctionBadIdea".Localized());

            OutputText("Scenes.Lottie.DrinkWaterWithYourHamhock.BoldEllipsisParagraph".Localized());

            OutputText("Scenes.Lottie.DrinkWaterWithYourHamhock.SitBackRelaxCafe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesHadYoga) == 0)
                OutputText("Scenes.Lottie.DrinkWaterWithYourHamhock.NeverHadYogaExperience".Localized());
            else
                OutputText("Scenes.Lottie.DrinkWaterWithYourHamhock.HadYogaExperience".Localized());

            OutputText("Scenes.Lottie.DrinkWaterWithYourHamhock.GreatWayMeetPeople".Localized());

            OutputText("Scenes.Lottie.DrinkWaterWithYourHamhock.LoosensUpAroundResponse".Localized());

            ReturnToCamp(1);
        }

        public void DrinkLustWithYourHamhock()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.DrinkLustWithYourHamhock.DownsCupPinkLiquid".Localized(
                Player.SMultiCockDesc(),

                Player.ArmorName,

                Player.ArmorName
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Lottie.DrinkLustWithYourHamhock.FingersTraceAroundBalls".Localized());

            OutputText("Scenes.Lottie.DrinkLustWithYourHamhock.StopsHerForNow".Localized());

            OutputText("Scenes.Lottie.DrinkLustWithYourHamhock.LeadsHerEmptySauna".Localized(
                Player.Mf("guy", "lady")
            ));

            OutputText("Scenes.Lottie.DrinkLustWithYourHamhock.DecidedVigorousWorkout".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 99f));

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            ClearButtons();

            AddButton(0, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), GetSuckedOffByAHamSandwich);

            AddButton(1, "Common.Button.Anal".Localized(), AnalLottieMilkshake);
        }

        public void AnalLottieMilkshake()
        {
            ClearOutput();

            ShowImage("lottie-fuck-her-in-teh-butzor");

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.AnalLottieMilkshake.GripOffFitGirth".Localized());

            if (Player.Cocks.Count == 2)
                OutputText("Scenes.Lottie.AnalLottieMilkshake.SqueezeInOtherCock".Localized());
            else
                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.RestDicksPokingFlesh".Localized());

            OutputText("Scenes.Lottie.AnalLottieMilkshake.GigglesLikeMyButt".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.AnalLottieMilkshake.CompleteMessGaspingConvulsing".Localized());

            OutputText("Scenes.Lottie.AnalLottieMilkshake.OinksNoisilyEyesRolling".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Lottie.AnalLottieMilkshake.SoakedPussyRubbingBalls".Localized());

            OutputText("Scenes.Lottie.AnalLottieMilkshake.SentencePeriodParagraph".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.AnalLottieMilkshake.ArentDoneBouncyRear".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.YourOtherCock".Localized());
                else
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.OneOfYourCocks".Localized());

                OutputText("Scenes.Lottie.AnalLottieMilkshake.PushAgainstPuckeredHole".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.YourTwo".Localized());
                else
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.TwoOfYour".Localized());

                OutputText("Scenes.Lottie.AnalLottieMilkshake.ThrobbingCocksBuriedRectum".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.PluralS".Localized());

                OutputText("Scenes.Lottie.AnalLottieMilkshake.SentencePeriodParagraphAgain".Localized());
            }

            if (Player.Balls > 0)
                OutputText("Scenes.Lottie.AnalLottieMilkshake.SackBeginsToSwell".Localized());
            else
                OutputText("Scenes.Lottie.AnalLottieMilkshake.AboutToExplode".Localized());

            OutputText("Scenes.Lottie.AnalLottieMilkshake.LargeAmountOfCum".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Lottie.AnalLottieMilkshake.Overflowing".Localized());
            else
                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.Dripping".Localized());
                else
                    OutputText("Scenes.Lottie.AnalLottieMilkshake.Filled".Localized());

            OutputText("Scenes.Lottie.AnalLottieMilkshake.HotSpunkSquealsOrgasm".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Lottie.AnalLottieMilkshake.HeavyAmountLeakingAss".Localized());

            OutputText("Scenes.Lottie.AnalLottieMilkshake.PullWipeAgainstButt".Localized());

            if (silly)
                OutputText("Scenes.Lottie.AnalLottieMilkshake.Bacon".Localized());

            OutputText("Scenes.Lottie.AnalLottieMilkshake.ShakeOnWayOut".Localized());

            LottieTone(1);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkAnal) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.LottieNeedsToTalkAnal, 1);

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

        public void GetSuckedOffByAHamSandwich()
        {
            ClearOutput();

            ShowImage("lottie-gives-you-a-blowjob");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.GrabShouldersMoveCrotch".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length <= 4)
            {
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.StaresThenUp".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.WhileYourOtherCock".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.Pokes".Localized());
                    else
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.PluralSPoke".Localized());

                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.AgainstHerChin".Localized());
                }

                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.SentencePeriodParagraph".Localized());
            }
            else
                if (Player.Cocks[x].Length <= 12)
                {
                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.StaresIntentlyHungerEyes".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.BiggestHeadLightly".Localized());
                    else
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.HeadLightly".Localized());

                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.SalivaLeaksQuickLick".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.JerkingOffBothHands".Localized(
                            Player.CockDescript(Player.BiggestCockIndex2())
                        ));

                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.SmallLicksTipDevours".Localized());
                }
                else
                {
                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.GaspsYouSlapFace".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.BiggestErection".Localized());
                    else
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.Erection".Localized());

                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.DroolEscapesLusciousLips".Localized(
                        Player.CockDescript(x)
                    ));

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.HandsMassageLeftover".Localized(
                            Player.CockDescript(Player.BiggestCockIndex2())
                        ));

                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.SmallLicksTipStuffs".Localized(
                        Player.CockDescript(x)
                    ));
                }

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.FellatingIncredibleRate".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.SackSlappingAgainstChin".Localized());
            else
                if (Player.BiggestBreastSize() > 2)
                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.WhileYouPlayYour".Localized(
                        Appearance.BiggestBreastSizeDescript(Player)
                    ));

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.RemarkHungryLittlePiggy".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ExtraFlailsSlappingCheeks".Localized());
                else
                    if (Player.CockTotal() > 2)
                        OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ExtraFlailSlappingCheeks".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.SlickTongueWrapsLength".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.StopsRubbingYourTo".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ReachesDrippingTwoFingers".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.KneadYour".Localized());
            else
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ReachAroundSqueezeYour".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.GrabHeadPullFurther".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.TakeOutMouthPop".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length > 12)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.SensitiveBrushingEagerMouth".Localized());
            else
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.RestingCushionyFuckpillows".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.WrapsTitsAroundLength".Localized());

            if (Player.Cocks[x].Length > 12)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.LicksCockheadSquishesBreasts".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.DecideMakeUseOf".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.Your".Localized());
                else
                    OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.An".Localized());

                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ExtraBetweenPoutyLips".Localized(
                    Player.CockDescript(Player.BiggestCockIndex2()),

                    Player.CockDescript(x)
                ));
            }

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.RaisingBodyUp".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ChokesOnYourMeat".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ThrobAgainstFleshBubbling".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ThickRopesSplatteringFace".Localized());
            else
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.MassiveSplatteringPaintingTits".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ContinuesMoveUpDetermined".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.IncredibleAmountsOf".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.CumPaintedGulpingAll".Localized(
                Player.Mf("big guy", "babe")
            ));

            if (silly)
                OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.Bacon".Localized());

            OutputText("Scenes.Lottie.GetSuckedOffByAHamSandwich.ShakeOnWayOut".Localized());

            LottieTone(-1);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkOral) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.LottieNeedsToTalkOral, 1);

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

        public void LotteryAppearance()
        {
            ClearOutput();

            ShowImage("lottie-examine-appearance");

            if (LottieTone() <= 20)
            {
                OutputText("Scenes.Lottie.LotteryAppearance.StandsShortOnAverage".Localized());

                if (Player.Tallness > 60)
                    OutputText("Scenes.Lottie.LotteryAppearance.ShorterThanYou".Localized());
                else
                    if (Player.Tallness > 53)
                        OutputText("Scenes.Lottie.LotteryAppearance.SameHeightAsYou".Localized());
                    else
                        OutputText("Scenes.Lottie.LotteryAppearance.TallerThanYouFifth".Localized());

                OutputText("Scenes.Lottie.LotteryAppearance.VeryChubbyBuild".Localized());
            }
            else
                if (LottieTone() <= 40)
                {
                    OutputText("Scenes.Lottie.LotteryAppearance.StandsShortAverageAgain".Localized());

                    if (Player.Tallness > 60)
                        OutputText("Scenes.Lottie.LotteryAppearance.ShorterThanYouAgain".Localized());
                    else
                        if (Player.Tallness > 53)
                            OutputText("Scenes.Lottie.LotteryAppearance.SameHeightYouAgain".Localized());
                        else
                            OutputText("Scenes.Lottie.LotteryAppearance.TallerThanYouAgain".Localized());

                    OutputText("Scenes.Lottie.LotteryAppearance.DefinitelyChubbyBuild".Localized());
                }
                else
                    if (LottieTone() <= 60)
                    {
                        OutputText("Scenes.Lottie.LotteryAppearance.StandsShortAverageThird".Localized());

                        if (Player.Tallness > 60)
                            OutputText("Scenes.Lottie.LotteryAppearance.ShorterThanYouThird".Localized());
                        else
                            if (Player.Tallness > 53)
                                OutputText("Scenes.Lottie.LotteryAppearance.SameHeightYouThird".Localized());
                            else
                                OutputText("Scenes.Lottie.LotteryAppearance.TallerThanYouThird".Localized());

                        OutputText("Scenes.Lottie.LotteryAppearance.BitChubbierBuild".Localized());
                    }
                    else
                        if (LottieTone() <= 80)
                        {
                            OutputText("Scenes.Lottie.LotteryAppearance.StandsShortAverageFourth".Localized());

                            if (Player.Tallness > 60)
                                OutputText("Scenes.Lottie.LotteryAppearance.ShorterThanYouFourth".Localized());
                            else
                                if (Player.Tallness > 53)
                                    OutputText("Scenes.Lottie.LotteryAppearance.SameHeightYouFourth".Localized());
                                else
                                    OutputText("Scenes.Lottie.LotteryAppearance.TallerThanYouFourth".Localized());

                            OutputText("Scenes.Lottie.LotteryAppearance.HasSomeChubBuild".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Lottie.LotteryAppearance.StandsShortAverageFifth".Localized());

                            if (Player.Tallness > 60)
                                OutputText("Scenes.Lottie.LotteryAppearance.ShorterThanYouFifth".Localized());
                            else
                                if (Player.Tallness > 53)
                                    OutputText("Scenes.Lottie.LotteryAppearance.SameHeightYouFifth".Localized());
                                else
                                    OutputText("Scenes.Lottie.LotteryAppearance.TallerThanYouFifth".Localized());

                            OutputText("Scenes.Lottie.LotteryAppearance.ExcellentShapeBuild".Localized());
                        }

            EncounterLottie();
        }

        public int LottieTone(int dif = 0)
        {

            if (dif == 0)
                return CoC.Instance.Flags.Get(GameFlagIds.LottieFigure);

            if (Debug)
                OutputText("Scenes.Lottie.LottieTone.DebugToneChanged".Localized(
                    dif
                ));

            CoC.Instance.Flags.Set(GameFlagIds.LottieFigure, CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) + dif);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.LottieFigure, 100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LottieFigure) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.LottieFigure, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkOral) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.LottieNeedsToTalkOral, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkAnal) == 1)
                CoC.Instance.Flags.Set(GameFlagIds.LottieNeedsToTalkAnal, 0);

            return CoC.Instance.Flags.Get(GameFlagIds.LottieFigure);
        }

        public int LottieMorale(int dif = 0)
        {

            if (dif == 0)
                return CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence);

            if (Debug)
                OutputText("Scenes.Lottie.LottieMorale.DebugMoraleChanged".Localized(
                    dif
                ));

            CoC.Instance.Flags.Set(GameFlagIds.LottieConfidence, CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) + dif);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.LottieConfidence, 100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.LottieConfidence, 0);

            return CoC.Instance.Flags.Get(GameFlagIds.LottieConfidence);
        }

        public void TalkToPigSlut()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkOral) == 1)
            {
                CoC.Instance.Flags.Set(GameFlagIds.LottieNeedsToTalkOral, 2);

                OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkUpGloomy".Localized(
                    Player.Mf("big guy", "babe")
                ));

                ClearButtons();

                AddButton(0, "Common.Button.Encourage".Localized(), FirstOralReactionChooseEncourage);

                AddButton(1, "Common.Button.Abuse".Localized(), FirstOralReactionChooseAbuse);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkAnal) == 1)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.LottieNeedsToTalkAnal, 2);

                    OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkUpExcited".Localized(
                        Player.Mf("big guy", "babe")
                    ));

                    ClearButtons();

                    AddButton(0, "Common.Button.Encourage".Localized(), FirstAnalReactionChooseEncourage);

                    AddButton(1, "Common.Button.Abuse".Localized(), FirstAnalReactionChooseAbuse);
                }
                else
                    if (LottieMorale() <= 30 && CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) - 5 > LottieTone())
                    {
                        OutputText("Scenes.Lottie.TalkToPigSlut.WantsToTalkWinces".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Encourage".Localized(), LottieLowMoraleWeightGainEncourage);

                        AddButton(1, "Common.Button.Abuse".Localized(), LottieLowMorealeWeightGainAbuse);
                    }
                    else
                        if (LottieMorale() <= 70 && CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) - 5 > LottieTone())
                        {
                            OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkGloomyAgain".Localized(
                                Player.Mf("big guy", "babe")
                            ));

                            ClearButtons();

                            AddButton(0, "Common.Button.Encourage".Localized(), LottieMediumMoraleWeightGainEncourage);

                            AddButton(1, "Common.Button.Abuse".Localized(), LottieMediumMorealeWeightGainAbuse);
                        }
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) - 5 > LottieTone() && CoC.Instance.Flags.Get(GameFlagIds.LottieLovesHerBody) == 1)
                            {
                                OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkUpDistracted".Localized(
                                    Player.Mf("big guy", "babe")
                                ));

                                ClearButtons();

                                AddButton(0, "Common.Button.Encourage".Localized(), LottieHighMoraleWeightGainFATTYLOVESIT);

                                AddButton(1, "Common.Button.Abuse".Localized(), LottieHighMoraleWeightGainFATTYLOVESABUSE);
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) - 5 > LottieTone())
                                {
                                    OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkDistractedAgain".Localized(
                                        Player.Mf("big guy", "babe")
                                    ));

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Encourage".Localized(), LottieHighMoraleWeightGainEncourage);

                                    AddButton(1, "Common.Button.Abuse".Localized(), LottieHighMoraleWeightGainAbuse);
                                }
                                else
                                    if (LottieMorale() <= 30 && CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) < LottieTone() - 5)
                                    {
                                        OutputText("Scenes.Lottie.TalkToPigSlut.WantsTalkWincesAgain".Localized());

                                        ClearButtons();

                                        AddButton(0, "Common.Button.Encourage".Localized(), LottieLowMoraleWeightLossEncourage);

                                        AddButton(1, "Common.Button.Abuse".Localized(), LottieLowMoraleWeightLossAbuse);
                                    }
                                    else
                                        if (LottieMorale() <= 70 && CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) < LottieTone() - 5)
                                        {
                                            OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkExcitedAgain".Localized());

                                            ClearButtons();

                                            AddButton(0, "Common.Button.Encourage".Localized(), LottieMediumMoraleWeightLossEncourage);

                                            AddButton(1, "Common.Button.Abuse".Localized(), LottieMediumMoraleWeightLossAbuse);
                                        }
                                        else
                                            if (CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) < LottieTone() - 5 && CoC.Instance.Flags.Get(GameFlagIds.LottieLovesHerBody) == 1)
                                            {
                                                OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkUpConcerned".Localized());

                                                ClearButtons();

                                                AddButton(0, "Common.Button.Encourage".Localized(), LottieHighMoraleWeightLossFATTYLOVESIT);

                                                AddButton(1, "Common.Button.Abuse".Localized(), LottieHighMoraleWeightLossFATTYLOVESABUSE);
                                            }
                                            else
                                                if (CoC.Instance.Flags.Get(GameFlagIds.LottieWeightConclusion) < LottieTone() - 5)
                                                {
                                                    OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkExcitedThird".Localized());

                                                    ClearButtons();

                                                    AddButton(0, "Common.Button.Encourage".Localized(), LottieHighMoraleWeightLossEncourage);

                                                    AddButton(1, "Common.Button.Abuse".Localized(), LottieHighMoraleWeightLossAbuse);
                                                }
                                                else
                                                    if (LottieMorale() == 0 && LottieTone() == 0 && CoC.Instance.Flags.Get(GameFlagIds.LottieFatassLowEsteem) == 0)
                                                    {
                                                        CoC.Instance.Flags.Set(GameFlagIds.LottieFatassLowEsteem, 1);

                                                        OutputText("Scenes.Lottie.TalkToPigSlut.WantsToTalkImpatient".Localized());

                                                        ClearButtons();

                                                        AddButton(0, "Common.Button.ToFuck".Localized(), LottieFatLoserCulminationFuckChoice);

                                                        AddButton(1, "Common.Button.ToHumiliate".Localized(), LottieFatLoserCulminationHumiliationChoice);

                                                        if (silly)
                                                            AddButton(2, "Common.Button.ToLeave".Localized(), LottieFatLoserCulminationLeaveComedyChoice);
                                                    }
                                                    else
                                                        if (LottieMorale() == 100 && LottieTone() == 0 && CoC.Instance.Flags.Get(GameFlagIds.LottieLovingFatBitchEvent) == 0)
                                                        {
                                                            CoC.Instance.Flags.Set(GameFlagIds.LottieLovingFatBitchEvent, 1);

                                                            OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkIncrediblyDistracted".Localized());

                                                            ClearButtons();

                                                            AddButton(0, "Common.Button.LoveSelf".Localized(), LottieHighMoraleFatLoveSelfOneTimeEvent);

                                                            AddButton(1, "Common.Button.Stickwithit".Localized(), LottieHighMoraleFatYoullGetThereOneTimeEvent);

                                                            if (silly)
                                                                AddButton(2, "Common.Button.Ewlottieno".Localized(), EwLottieNo);
                                                        }
                                                        else
                                                            if (LottieMorale() == 0 && LottieTone() == 100 && CoC.Instance.Flags.Get(GameFlagIds.LottieCharlottesWebEvent) == 0)
                                                            {
                                                                CoC.Instance.Flags.Set(GameFlagIds.LottieCharlottesWebEvent, 1);

                                                                OutputText("Scenes.Lottie.TalkToPigSlut.WantsToTalkFidgeting".Localized());

                                                                OutputText("Scenes.Lottie.TalkToPigSlut.SpiderGirlNoticesGaze".Localized());

                                                                OutputText("Scenes.Lottie.TalkToPigSlut.EmbraceNewGroupMember".Localized());

                                                                ClearButtons();

                                                                AddButton(0, "Common.Button.Threesome".Localized(), LottieElleThreesomeFirstTime);

                                                                AddButton(1, "Common.Button.JustElle".Localized(), FuckElleInsteadOfThatFatSlutFirstTime);

                                                                if (silly)
                                                                    AddButton(2, "Common.Button.OhIgetit".Localized(), CharlottesWebComedyBullshit);
                                                            }
                                                            else
                                                                if (LottieMorale() == 100 && LottieTone() == 100 && CoC.Instance.Flags.Get(GameFlagIds.LottieMaxGoalLoveReached) == 0)
                                                                {
                                                                    CoC.Instance.Flags.Set(GameFlagIds.LottieMaxGoalLoveReached, 1);

                                                                    OutputText("Scenes.Lottie.TalkToPigSlut.WantsTalkTooDistracted".Localized());

                                                                    ClearButtons();

                                                                    AddButton(0, "Common.Button.YogaRoom".Localized(), LottieSuccessOneShot);

                                                                    AddButton(1, "Common.Button.Leave".Localized(), LeaveLottieInsteadOfYogaSex);
                                                                }
                                                                else
                                                                    if (LottieMorale() <= 30)
                                                                    {
                                                                        OutputText("Scenes.Lottie.TalkToPigSlut.WantsTalkWincesThird".Localized());

                                                                        OutputText("Scenes.Lottie.TalkToPigSlut.NotMuchToGo".Localized());

                                                                        ReturnToCamp(1);
                                                                    }
                                                                    else
                                                                        if (LottieMorale() <= 70)
                                                                        {
                                                                            OutputText("Scenes.Lottie.TalkToPigSlut.EarsPerkNothingMind".Localized());

                                                                            Player.ApplyStats(new DynStat(GameStats.Lust, 2f));

                                                                            OutputText("Scenes.Lottie.TalkToPigSlut.NotMuchOnAgain".Localized());

                                                                            ReturnToCamp(1);
                                                                        }
                                                                        else
                                                                            if (LottieMorale() <= 100000)
                                                                            {
                                                                                OutputText("Scenes.Lottie.TalkToPigSlut.EarsNothingMindAgain".Localized());

                                                                                Player.ApplyStats(new DynStat(GameStats.Lust, 2f));

                                                                                OutputText("Scenes.Lottie.TalkToPigSlut.NotMuchOnThird".Localized());

                                                                                ReturnToCamp(1);
                                                                            }
                                                                            else
                                                                            {
                                                                                OutputText("Scenes.Lottie.TalkToPigSlut.BuggedOutSon".Localized());

                                                                                ReturnToCamp(1);
                                                                            }

            CoC.Instance.Flags.Set(GameFlagIds.LottieWeightConclusion, LottieTone());
        }

        public void FirstOralReactionChooseEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.FirstOralReactionChooseEncourage.NotHerFaultResults".Localized());

            OutputText("Scenes.Lottie.FirstOralReactionChooseEncourage.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.FirstOralReactionChooseEncourage.AfterWorkoutReturnCamp".Localized());

            LottieMorale(3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void FirstOralReactionChooseAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.FirstOralReactionChooseAbuse.MethodsNeverFail".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void FirstAnalReactionChooseEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.FirstAnalReactionChooseEncourage.CongratulateHerSuccess".Localized());

            OutputText("Scenes.Lottie.FirstAnalReactionChooseEncourage.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.FirstAnalReactionChooseEncourage.AfterWorkoutReturnCamp".Localized());

            LottieMorale(3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void FirstAnalReactionChooseAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.FirstAnalReactionChooseAbuse.SneerNowhereNearGoal".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieLowMoraleWeightGainEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieLowMoraleWeightGainEncourage.ShouldntCryThingsHappen".Localized());

            OutputText("Scenes.Lottie.LottieLowMoraleWeightGainEncourage.BoldEllipsisAfterWorkout".Localized());

            LottieMorale(3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieLowMorealeWeightGainAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieLowMorealeWeightGainAbuse.IncreaseGripGrowlSqueals".Localized());

            OutputText("Scenes.Lottie.LottieLowMorealeWeightGainAbuse.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieLowMorealeWeightGainAbuse.AfterWorkoutReturnCamp".Localized());

            LottieMorale(-10);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieMediumMoraleWeightGainEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieMediumMoraleWeightGainEncourage.NotFaultThingsHappen".Localized());

            OutputText("Scenes.Lottie.LottieMediumMoraleWeightGainEncourage.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieMediumMoraleWeightGainEncourage.AfterWorkoutReturnCamp".Localized());

            LottieMorale(3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieMediumMorealeWeightGainAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieMediumMorealeWeightGainAbuse.MethodsNeverFail".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightGainEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainEncourage.NeverFaultFineFeel".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainEncourage.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainEncourage.AfterWorkoutReturnCamp".Localized());

            LottieMorale(3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightGainAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainAbuse.FrownTighteningGripShoulder".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainAbuse.DecidingDoneReturnCamp".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightGainFATTYLOVESIT()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainFATTYLOVESIT.NeverFaultEvenThough".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainFATTYLOVESIT.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainFATTYLOVESIT.AfterWorkoutReturnCamp".Localized());

            LottieMorale(3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightGainFATTYLOVESABUSE()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainFATTYLOVESABUSE.FrownTighteningGripShoulder".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightGainFATTYLOVESABUSE.DecidingDoneReturnCamp".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieLowMoraleWeightLossEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieLowMoraleWeightLossEncourage.CongratulateAlmostHappy".Localized());

            OutputText("Scenes.Lottie.LottieLowMoraleWeightLossEncourage.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieLowMoraleWeightLossEncourage.AfterWorkoutReturnCamp".Localized());

            LottieMorale(3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieLowMoraleWeightLossAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieLowMoraleWeightLossAbuse.SneerExcitedBitLoss".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieMediumMoraleWeightLossEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieMediumMoraleWeightLossEncourage.CongratulateResultsQuick".Localized());

            OutputText("Scenes.Lottie.LottieMediumMoraleWeightLossEncourage.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieMediumMoraleWeightLossEncourage.AfterWorkoutReturnCamp".Localized());

            LottieMorale(4);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieMediumMoraleWeightLossAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieMediumMoraleWeightLossAbuse.SneerNowhereNearGoal".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightLossEncourage()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossEncourage.CongratulateGettingUsedTo".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossEncourage.BoldEllipsisAfterWorkout".Localized());

            LottieMorale(4);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightLossAbuse()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossAbuse.FrownEagerReaction".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossAbuse.DecidingDoneReturnCamp".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightLossFATTYLOVESIT()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossFATTYLOVESIT.RubShoulderNoShame".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossFATTYLOVESIT.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossFATTYLOVESIT.AfterWorkoutReturnCamp".Localized());

            LottieMorale(4);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleWeightLossFATTYLOVESABUSE()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossFATTYLOVESABUSE.FrownUncomfortableSuddenLoss".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossFATTYLOVESABUSE.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleWeightLossFATTYLOVESABUSE.AfterWorkoutReturnCamp".Localized());

            LottieMorale(-10);

            ReturnToCamp(1);
        }

        public void LottieFatLoserCulminationFuckChoice()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.OnlyWorthPurposeNow".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.GroanTightWallsScreaming".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.FamiliarSensationDriveFaster".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.GiveLeaveSeedLeaking".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.ShakeOnWayOut".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.FuckingLoveBaconShakes".Localized());

            OutputText("Scenes.Lottie.LottieFatLoserCulminationFuckChoice.DoggyStyleUnlocked".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Corruption, 3f));

            CoC.Instance.Flags.Set(GameFlagIds.LottieDoggystyleUnlocked, 1);

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            ReturnToCamp(1);
        }

        public void LottieFatLoserCulminationHumiliationChoice()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.TakenOutsideBackGym".Localized());

            OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.TimidlyRemovesTopBends".Localized());

            if (OptionStore.Instance.Get("urtaDisabled", 0) > 0)
            {
                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.WalkFamiliarBackAlley".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.BaconBaconGottaHave".Localized());
                else
                    OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.HeyyyBuddyShexSlave".Localized());

                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.DecideRightPushLottie".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.LeaveVagrantJerksOff".Localized());
            }
            else
            {
                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.WalkFamiliarAlleyAgain".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.BaconGottaHaveAgain".Localized());
                else
                    OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.HeyyyBuddySlaveAgain".Localized());

                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.DecideRightPushAgain".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.LeaveUrtaJerksOff".Localized());
            }

            if (silly)
                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.BaconAgain".Localized());

            OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.LoveSpitRoasting".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.BaconAgain".Localized());

            OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.Shake".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieWalkieUnlocked, 1);

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            OutputText("Scenes.Lottie.LottieFatLoserCulminationHumiliationChoice.WalkiesUnlocked".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 5f));

            ReturnToCamp(1);
        }

        public void LottieFatLoserCulminationLeaveComedyChoice()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieFatLoserCulminationLeaveComedyChoice.OneLookTellRequest".Localized());

            OutputText("Scenes.Lottie.LottieFatLoserCulminationLeaveComedyChoice.DiscoverPregnantMiserable".Localized());

            OutputText("Scenes.Lottie.LottieFatLoserCulminationLeaveComedyChoice.NoLongerEncounterable".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieDisabled, 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            Player.CumMultiplier += 100;

            ReturnToCamp(1);
        }

        public void LottieHighMoraleFatLoveSelfOneTimeEvent()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.BeginAboutYourStory".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.RosyLoverBouncesRoom".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 40)
                OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.AmpleCheeksRestingGroin".Localized());
            else
                OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.OnlyReachFarFilled".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.TakeMattersOwnHands".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.AboutExplodeTightGrip".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.YourOtherCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.Does".Localized());
                else
                    OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.PluralSDoes".Localized());

                OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.NotDisappointSpurtingEssence".Localized());
            }

            OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.BothLayMatPanting".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.WhileLaterSaidGoodbyes".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.BeforeLeaveBaconShake".Localized());
            else
                OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.ParagraphBreak".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatLoveSelfOneTimeEvent.CowgirlUnlocked".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Corruption, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.LottieCowgirlUnlocked, 1);

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            ReturnToCamp(1);
        }

        public void LottieHighMoraleFatYoullGetThereOneTimeEvent()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.TellShouldntAffectFeelings".Localized(
                Player.ShortName
            ));

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.FollowOutPoolArea".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.NudeGirlStandsBlushing".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.SecondsFlatRemoveClothes".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.YourOtherCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.Twitches".Localized());
                else
                    OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.PluralSTwitch".Localized());

                OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.InEcstasyReleasingButt".Localized());
            }

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.ParagraphBreak".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.SweatyPantingLiesFloor".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.ShakeOnWayOut".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.FuckingDelicious".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Lottie.LottieHighMoraleFatYoullGetThereOneTimeEvent.QuickieUnlocked".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieQuickieUnlocked, 1);

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Corruption, -1f));

            ReturnToCamp(1);
        }

        public void EwLottieNo()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.EwLottieNo.BeginAboutYourStory".Localized());

            OutputText("Scenes.Lottie.EwLottieNo.HoursTurnsPumpingSeed".Localized());

            OutputText("Scenes.Lottie.EwLottieNo.ReallyNeedGlasses".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieDisabled, 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            ReturnToCamp(1);
        }

        public void LottieElleThreesomeFirstTime()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieElleThreesomeFirstTime.ChuckleDevilishlyUndressHarem".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Lottie.LottieElleThreesomeFirstTime.OfOne".Localized());

            OutputText("Scenes.Lottie.LottieElleThreesomeFirstTime.IntoFullLipsCircling".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (silly)
                OutputText("Scenes.Lottie.LottieElleThreesomeFirstTime.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieElleThreesomeFirstTime.ShakeOnWayOut".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieElleThreesomeFirstTime.OnlyNeedOne".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            OutputText("Scenes.Lottie.LottieElleThreesomeFirstTime.ThreesomeUnlocked".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieThreesomeUnlocked, 1);

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            ReturnToCamp(1);
        }

        public void FuckElleInsteadOfThatFatSlutFirstTime()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.ChuckleDevilishlyUndress".Localized());

            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.CaressesWithTwoHands".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.MassagingBalls".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.MassagingPussy".Localized());
                    else
                        OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.MassagingAss".Localized());
            }
            else
                OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.UsesAllFourHands".Localized());

            OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.LottieStandsSobbingFingering".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.YourExtraCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.ThrobsSpillingEssenceFloor".Localized());
                else
                    OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.PluralThrobSpillingEssence".Localized());
            }

            OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.ElleFollowsSuitPeak".Localized(
                Player.CockDescript(x)
            ));

            if (silly)
                OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.GrabBaconShakeWay".Localized());

            OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.SentencePeriod".Localized());

            OutputText("Scenes.Lottie.FuckElleInsteadOfThatFatSlutFirstTime.ElleUnlocked".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieElleNtrUnlocked, 1);

            CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 10f));

            ReturnToCamp(1);
        }

        public void CharlottesWebComedyBullshit()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.CharlottesWebComedyBullshit.SlapGleefullyPointGirls".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.CharlottesWebComedyBullshit.BoldEllipsis".Localized());

            OutputText("Scenes.Lottie.CharlottesWebComedyBullshit.WakeLaterMissingGems".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LottieDisabled, 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            ReturnToCamp(1);
        }

        public void LottieSuccessOneShot()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            if (Player.Tone > 30 || Player.Thickness < 70)
            {
                OutputText("Scenes.Lottie.LottieSuccessOneShot.WalkYogaAreaSmell".Localized());

                OutputText("Scenes.Lottie.LottieSuccessOneShot.GentlyAsksLieDown".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Lottie.LottieSuccessOneShot.ReleaseApproachingFinalPush".Localized(
                    Player.CockDescript(x)
                ));

                if (silly)
                    OutputText("Scenes.Lottie.LottieSuccessOneShot.BaconAgain".Localized());

                OutputText("Scenes.Lottie.LottieSuccessOneShot.ShakeFromCafeFantastic".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LottieReverseCowgirlUnlocked, 1);

                CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

                OutputText("Scenes.Lottie.LottieSuccessOneShot.ReverseCowgirlUnlocked".Localized());
            }
            else
            {
                OutputText("Scenes.Lottie.LottieSuccessOneShot.WalkYogaAreaAgain".Localized());

                OutputText("Scenes.Lottie.LottieSuccessOneShot.WakeDazedForeheadThrobbing".Localized());

                OutputText("Scenes.Lottie.LottieSuccessOneShot.SlapLeanPigGirl".Localized());

                if (Player.BiggestBreastSize() >= 2)
                    OutputText("Scenes.Lottie.LottieSuccessOneShot.Tits".Localized());
                else
                    OutputText("Scenes.Lottie.LottieSuccessOneShot.Belly".Localized());

                OutputText("Scenes.Lottie.LottieSuccessOneShot.SqueezingPuttyReachedGoal".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Lottie.LottieSuccessOneShot.VelvetGlovedHandsTease".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CumQ() < 500)
                {
                    OutputText("Scenes.Lottie.LottieSuccessOneShot.NodFeverishlyCum".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                    if (silly)
                        OutputText("Scenes.Lottie.LottieSuccessOneShot.BuyShakeHeadHome".Localized());
                    else
                        OutputText("Scenes.Lottie.LottieSuccessOneShot.ParagraphBreak".Localized());
                }
                else
                    OutputText("Scenes.Lottie.LottieSuccessOneShot.TooMuchBuiltThrust".Localized(
                        Player.CockDescript(x)
                    ));

                if (silly)
                    OutputText("Scenes.Lottie.LottieSuccessOneShot.BaconAgain".Localized());

                OutputText("Scenes.Lottie.LottieSuccessOneShot.ShakeBeforeHeadHome".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.LottieSuccessOneShot.FuckingLoveBaconShakes".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LottieFemdomUnlocked, 1);

                CoC.Instance.Flags.Set(GameFlagIds.LottiePorkedCount, CoC.Instance.Flags.Get(GameFlagIds.LottiePorkedCount) + 1);

                OutputText("Scenes.Lottie.LottieSuccessOneShot.FemdomUnlocked".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LeaveLottieInsteadOfYogaSex()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LeaveLottieInsteadOfYogaSex.MindTellingBlissfulIgnorance".Localized());

            OutputText("Scenes.Lottie.LeaveLottieInsteadOfYogaSex.StrangestFeelingMissedSex".Localized());

            ReturnToCamp(1);
        }

        public void LottieExercise()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieExercise.TimeToHitGym".Localized());

            if (LottieMorale() >= 66)
                OutputText("Scenes.Lottie.LottieExercise.EnthusiasticNod".Localized());
            else
                if (LottieMorale() >= 33)
                    OutputText("Scenes.Lottie.LottieExercise.NodInAgreement".Localized());
                else
                    OutputText("Scenes.Lottie.LottieExercise.SlowlyLookingUpNodding".Localized());

            OutputText("Scenes.Lottie.LottieExercise.WhereYouWannaGo".Localized(
                Player.Mf("guy", "ma'am")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Weights".Localized(), LottieWeights);

            AddButton(1, "Common.Button.Jogging".Localized(), GoJoggingWithLottie);

            AddButton(2, "Common.Button.Yoga".Localized(), LottieYoga);

            AddButton(3, "Common.Button.Pool".Localized(), LottiePool);
        }

        public void LottieWeights()
        {
            ClearOutput();

            if (LottieTone() <= 70)
            {
                OutputText("Scenes.Lottie.LottieWeights.LooksNervouslyLiftingToday".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Boast".Localized(), BoastAtLottie);

                AddButton(1, "Common.Button.Train".Localized(), TrainWithLottie);

                if (LottieMorale() <= 30 && CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedIfrisBlowjob) > 0 && Player.Strength >= 75 && Player.Tone >= 60 && Player.CumQ() >= 1000)
                    AddButton(2, "Common.Button.NtrLift".Localized(), IfrisAndLottieGetsNTRedLiftingWeights);
                else
                    AddButton(2, "Common.Button.NtrLift".Localized(), null);
            }
            else
            {
                OutputText("Scenes.Lottie.LottieWeights.PumpsFistBitLifting".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Boast".Localized(), BoastAtFitLottie);

                AddButton(1, "Common.Button.Train".Localized(), TrainWithFitLottie);
            }
        }

        public void BoastAtLottie()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.BoastAtLottie.LaughPickLargerPump".Localized());

            if (silly)
                OutputText("Scenes.Lottie.BoastAtLottie.LargePomf".Localized());
            else
                OutputText("Scenes.Lottie.BoastAtLottie.LoudClonk".Localized());

            OutputText("Scenes.Lottie.BoastAtLottie.LooksUpSnifflesGrunt".Localized());

            OutputText("Scenes.Lottie.BoastAtLottie.BucklesUnderPressureGives".Localized());

            Player.ChangeFatigue(10);

            Player.ApplyStats(new DynStat(GameStats.Strength, .25f));

            LottieMorale(-3);

            ReturnToCamp(1);
        }

        public void TrainWithLottie()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.TrainWithLottie.ChucklePatheticSmallOnes".Localized());

            OutputText("Scenes.Lottie.TrainWithLottie.BothDecideEnough".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, .1f));

            LottieMorale(2);

            LottieTone(2);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void BoastAtFitLottie()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.BoastAtFitLottie.LaughPickLargerPump".Localized());

            OutputText("Scenes.Lottie.BoastAtFitLottie.BucklesUnderPressureGives".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, .25f));

            LottieMorale(-3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void TrainWithFitLottie()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.TrainWithFitLottie.SmileWorkPullWeight".Localized());

            OutputText("Scenes.Lottie.TrainWithFitLottie.BothDecideEnough".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, .1f));

            LottieMorale(2);

            LottieTone(2);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void GoJoggingWithLottie()
        {
            ClearOutput();

            if (LottieTone() <= 70)
            {
                OutputText("Scenes.Lottie.GoJoggingWithLottie.DecidedRunningNotStrong".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Sprint".Localized(), SprintAndLeaveLottieBehind);

                AddButton(1, "Common.Button.Jog".Localized(), JogWithLottie);
            }
            else
            {
                OutputText("Scenes.Lottie.GoJoggingWithLottie.DecidedJoggingHappilyAgrees".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Sprint".Localized(), SprintAndLeaveFitLottieBehind);

                AddButton(1, "Common.Button.Jog".Localized(), JogWithFitLottie);
            }
        }

        public void SprintAndLeaveLottieBehind()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.SprintAndLeaveLottieBehind.DontWantLoseGroove".Localized());

            OutputText("Scenes.Lottie.SprintAndLeaveLottieBehind.ExhaustedReturnHome".Localized());

            Player.ApplyStats(new DynStat(GameStats.Toughness, .2f), new DynStat(GameStats.Speed, .25f));

            LottieMorale(-3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void JogWithLottie()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.JogWithLottie.SprintingWouldntGetAnywhere".Localized());

            Player.ApplyStats(new DynStat(GameStats.Toughness, .1f), new DynStat(GameStats.Speed, .1f));

            LottieMorale(2);

            LottieTone(2);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void SprintAndLeaveFitLottieBehind()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.SprintAndLeaveFitLottieBehind.DontWantLoseGroove".Localized());

            OutputText("Scenes.Lottie.SprintAndLeaveFitLottieBehind.FewRoundsExhaustedNotice".Localized());

            Player.ApplyStats(new DynStat(GameStats.Toughness, .2f), new DynStat(GameStats.Speed, .25f));

            LottieMorale(-3);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void JogWithFitLottie()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.JogWithFitLottie.SprintingWasteOfTime".Localized());

            LottieMorale(2);

            LottieTone(2);

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void LottieYoga()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieYoga.KnowsAffinityYogaEnter".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Alone".Localized(), LottieYogaAlone);

            AddButton(1, "Common.Button.Together".Localized(), TogetherLottieYoga);
        }

        public void LottieYogaAlone()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieYogaAlone.TakeMatWalkTrainer".Localized());

            OutputText("Scenes.Lottie.LottieYogaAlone.ContinueSexualPositions".Localized());

            OutputText(Player.ModThickness(30, 1));

            LottieMorale(-2);

            Player.ChangeFatigue(5);

            ReturnToCamp(1);
        }

        public void TogetherLottieYoga()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.TogetherLottieYoga.TakeMatPigGirl".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Lottie.TogetherLottieYoga.PluralS".Localized());

            OutputText("Scenes.Lottie.TogetherLottieYoga.SecretlyRelishBackClass".Localized());

            OutputText("Scenes.Lottie.TogetherLottieYoga.ClassEndsTeaseWalk".Localized());

            LottieMorale(4);

            LottieTone(1);

            Player.ChangeFatigue(5);

            Player.ApplyStats(new DynStat(GameStats.Lust, 80f));

            ReturnToCamp(1);
        }

        public void LottiePool()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottiePool.OftenFindNearPool".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Jump".Localized(), JumpInPoolBeforeLottieSeesYourWingWang);

            AddButton(1, "Common.Button.Wait".Localized(), WaitAndShowLottieYourPoolBoner);
        }

        public void JumpInPoolBeforeLottieSeesYourWingWang()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.JumpInPoolBeforeLottieSeesYourWingWang.CantPlungeFreezingWater".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Lottie.JumpInPoolBeforeLottieSeesYourWingWang.PluralS".Localized());

            OutputText("Scenes.Lottie.JumpInPoolBeforeLottieSeesYourWingWang.DecidedHelloSwimAlone".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            LottieTone(2);

            LottieMorale(-1);

            Player.ChangeFatigue(5);

            ReturnToCamp(1);
        }

        public void WaitAndShowLottieYourPoolBoner()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.WaitAndShowLottieYourPoolBoner.FigureCantDoHarm".Localized());

            if (Player.BiggestCockArea() >= 15)
                OutputText("Scenes.Lottie.WaitAndShowLottieYourPoolBoner.PrideAndJoy".Localized());
            else
                if (Player.BiggestCockArea() < 5)
                    OutputText("Scenes.Lottie.WaitAndShowLottieYourPoolBoner.SecretShame".Localized());
                else
                    OutputText("Scenes.Lottie.WaitAndShowLottieYourPoolBoner.PainfullyAveragePrick".Localized());

            OutputText("Scenes.Lottie.WaitAndShowLottieYourPoolBoner.AlmostTingeOfRegret".Localized(
                Player.Mf("big guy", "babe")
            ));

            OutputText("Scenes.Lottie.WaitAndShowLottieYourPoolBoner.WhileLaterFinishedLaps".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 80f));

            LottieTone(2);

            LottieMorale(2);

            Player.ChangeFatigue(5);

            ReturnToCamp(1);
        }

        public void GiveLottieAnItem()
        {
            ClearOutput();

            ClearButtons();

            AddButton(0, "Common.Button.Cafe".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LaBova)))
                AddButton(1, "Items.Labova.Short".Localized(), FeedLottieLaBova);
            else
                AddButton(1, "Items.Labova.Short".Localized(), null);

            if (Player.HasItem(ItemIds.GroPlus) && CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) > 0)
                AddButton(2, "Items.GroPlus.Short".Localized(), GiveLottieGroPlus);
            else
                AddButton(2, "Items.GroPlus.Short".Localized(), null);

            if (Player.HasItem(ItemIds.Reducto))
                AddButton(3, "Items.Reducto.Short".Localized(), GiveLottieReducto);
            else
                AddButton(3, "Items.Reducto.Short".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieTimesGivenItems) == 0)
                OutputText("Scenes.Lottie.GiveLottieAnItem.ResearchWeightLossAids".Localized());
            else
            {
                OutputText("Scenes.Lottie.GiveLottieAnItem.MightHaveOtherItems".Localized());

                AddButton(0, "Common.Button.Cafe".Localized(), FeedLottieAWholeCafe);

                if (silly)
                    AddButton(4, "Common.Button.Hamsandwich".Localized(), HamSammitchTimeBIATCH);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => EncounterLottie());
        }

        public void FeedLottieAWholeCafe()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.FeedLottieAWholeCafe.BetterFindingCafeFood".Localized());

            OutputText("Scenes.Lottie.FeedLottieAWholeCafe.SecretlyKnowItWill".Localized());

            LottieTone(-5);

            CoC.Instance.Flags.Set(GameFlagIds.LottieTimesGivenItems, CoC.Instance.Flags.Get(GameFlagIds.LottieTimesGivenItems) + 1);

            ReturnToCamp(1);
        }

        public void FeedLottieLaBova()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieLabovaCountdown) == 0)
                OutputText("Scenes.Lottie.FeedLottieLaBova.LaBovaRemoveLabel".Localized());
            else
                OutputText("Scenes.Lottie.FeedLottieLaBova.LaBovaPourContainer".Localized());

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LaBova));

            LottieTone(-10);

            CoC.Instance.Flags.Set(GameFlagIds.LottieLabovaCountdown, 40);

            CoC.Instance.Flags.Set(GameFlagIds.LottieTimesGivenItems, CoC.Instance.Flags.Get(GameFlagIds.LottieTimesGivenItems) + 1);

            ReturnToCamp(1);
        }

        public void GiveLottieGroPlus()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.GiveLottieGroPlus.TakeNeedleFlickBubbles".Localized());

            if (Player.Intelligence >= 80)
            {
                OutputText("Scenes.Lottie.GiveLottieGroPlus.TakeHandSuaveMovements".Localized());

                LottieTone(-20);

                LottieMorale(-1);

                Player.ConsumeItem(ItemIds.GroPlus);
            }
            else
            {
                OutputText("Scenes.Lottie.GiveLottieGroPlus.GrabShoulderBrushes".Localized());

                LottieMorale(-5);
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.LottieTimesGivenItems, CoC.Instance.Flags.Get(GameFlagIds.LottieTimesGivenItems) + 1);

            ReturnToCamp(1);
        }

        public void GiveLottieReducto()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieReductoUsed) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.LottieReductoUsed, CoC.Instance.Flags.Get(GameFlagIds.LottieReductoUsed) + 1);

                OutputText("Scenes.Lottie.GiveLottieReducto.TakeTubeNoticesReducto".Localized());

                OutputText("Scenes.Lottie.GiveLottieReducto.SitsStillUnsureGlad".Localized());
            }
            else
            {
                OutputText("Scenes.Lottie.GiveLottieReducto.TakeTubeRecognizesReducto".Localized());

                OutputText("Scenes.Lottie.GiveLottieReducto.SitsStillGladAgain".Localized());
            }

            LottieTone(10);

            Player.ConsumeItem(ItemIds.Reducto);

            CoC.Instance.Flags.Set(GameFlagIds.LottieTimesGivenItems, CoC.Instance.Flags.Get(GameFlagIds.LottieTimesGivenItems) + 1);

            ReturnToCamp(1);
        }

        public void HamSammitchTimeBIATCH()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LottieHamSammich) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.LottieHamSammich, CoC.Instance.Flags.Get(GameFlagIds.LottieHamSammich) + 1);

                OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.WalkCafeOrderSandwich".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.MumblesLifetimeFreeFood".Localized());

                OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.ExchangeSmileHandSandwich".Localized());

                OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.ReachForHighFive".Localized());
            }
            else
                if (Player.IsCorruptEnough(75))
                {
                    OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.WalkOrderSandwich".Localized());

                    OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.ReachHighFiveAgain".Localized());

                    Player.Orgasm("Dick");
                }
                else
                {
                    OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.WalkOrderSandwichThird".Localized());

                    OutputText("Scenes.Lottie.HamSammitchTimeBIATCH.ReachHighFiveThird".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.LottieTimesGivenItems, CoC.Instance.Flags.Get(GameFlagIds.LottieTimesGivenItems) + 1);

            ReturnToCamp(1);
        }

        public void LottieFuck()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            if (LottieMorale() <= 40)
            {
                OutputText("Scenes.Lottie.LottieFuck.TakeHandFlinchTouch".Localized(
                    Player.ShortName,

                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Lottie.LottieFuck.TurnDecidedVigorousWorkout".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Anal".Localized(), AnalLottieMilkshake);

                AddButton(1, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), GetSuckedOffByAHamSandwich);

                if (Player.CountCocksOfType("TENTACLE") > 0 && CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkOral) == 2)
                    AddButton(2, "Common.Button.Tentacle".Localized(), TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK);
                else
                    AddButton(2, "Common.Button.Tentacle".Localized(), null);

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieDoggystyleUnlocked) > 0)
                    AddButton(5, "Common.Button.Doggystyle".Localized(), LottieRepeatDoggieStyle);
                else
                    AddButton(5, "Common.Button.Doggystyle".Localized(), null);

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieWalkieUnlocked) > 0)
                    AddButton(6, "Common.Button.Walkies".Localized(), LottieRepeatWalkies);
                else
                    AddButton(6, "Common.Button.Walkies".Localized(), null);

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieThreesomeUnlocked) > 0)
                    AddButton(7, "Common.Button.Threesome".Localized(), LottieRepeatThreesome);
                else
                    AddButton(7, "Common.Button.Threesome".Localized(), null);

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieElleNtrUnlocked) > 0)
                    AddButton(8, "Common.Button.Netorare".Localized(), LottieRepeatSUPERNEATORARES);
                else
                    AddButton(8, "Common.Button.Netorare".Localized(), null);

                if (silly && Player.HasCock())
                    AddButton(9, "Common.Button.Minute".Localized(), LottiesThirtyMinutePigGasm);
            }
            else
                if (LottieMorale() <= 60)
                {
                    OutputText("Scenes.Lottie.LottieFuck.AskWorkoutGiggle".Localized(
                        Player.Mf("big guy", "babe")
                    ));

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Lottie.LottieFuck.PluralS".Localized());

                    OutputText("Scenes.Lottie.LottieFuck.TakeHandLeadCafe".Localized());

                    if (silly)
                        OutputText("Scenes.Lottie.LottieFuck.Street".Localized());

                    OutputText("Scenes.Lottie.LottieFuck.SharkNotDifficultUndress".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Lottie.LottieFuck.NotUnsatisfiedVigorousWorkout".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Anal".Localized(), AnalLottieMilkshake);

                    AddButton(1, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), GetSuckedOffByAHamSandwich);

                    if (Player.CountCocksOfType("TENTACLE") > 0 && CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkOral) == 2)
                        AddButton(2, "Common.Button.Tentacle".Localized(), TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK);
                    else
                        AddButton(2, "Common.Button.Tentacle".Localized(), null);

                    if (silly && Player.HasCock())
                        AddButton(3, "Common.Button.Minute".Localized(), LottiesThirtyMinutePigGasm);
                }
                else
                {
                    OutputText("Scenes.Lottie.LottieFuck.TakeHandSmallBlush".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Lottie.LottieFuck.PluralSAgain".Localized());

                    OutputText("Scenes.Lottie.LottieFuck.AgainstPlumpAsscheeksMoans".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Lottie.LottieFuck.WhimpersBetweenHotBreaths".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Lottie.LottieFuck.TurnVigorousWorkout".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Anal".Localized(), AnalLottieMilkshake);

                    AddButton(1, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), GetSuckedOffByAHamSandwich);

                    if (Player.CountCocksOfType("TENTACLE") > 0 && CoC.Instance.Flags.Get(GameFlagIds.LottieNeedsToTalkOral) == 2)
                        AddButton(2, "Common.Button.Tentacle".Localized(), TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK);
                    else
                        AddButton(2, "Common.Button.Tentacle".Localized(), null);

                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieQuickieUnlocked) > 0)
                        AddButton(3, "Common.Button.Quickie".Localized(), LottieRepeatQuickie);
                    else
                        AddButton(3, "Common.Button.Quickie".Localized(), null);

                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieCowgirlUnlocked) > 0)
                        AddButton(4, "Common.Button.CowGirl".Localized(), LottieRepeatCowGirl);
                    else
                        AddButton(4, "Common.Button.CowGirl".Localized(), null);

                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieReverseCowgirlUnlocked) > 0)
                        AddButton(5, "Common.Button.CowGirl".Localized(), LottieRepeatBackwardsAssCowgirl);
                    else
                        AddButton(5, "Common.Button.CowGirl".Localized(), null);

                    if (CoC.Instance.Flags.Get(GameFlagIds.LottieFemdomUnlocked) > 0)
                        AddButton(6, "Common.Button.Femdom".Localized(), LottieRepeatFEMDOMFEMFDOM);
                    else
                        AddButton(6, "Common.Button.Femdom".Localized(), null);

                    if (silly && Player.HasCock())
                        AddButton(7, "Common.Button.Minute".Localized(), LottiesThirtyMinutePigGasm);
                }

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));
        }

        public void LottieRepeatDoggieStyle()
        {
            ClearOutput();

            ShowImage("lottie-fuck-her-doggie-style-repeat");

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatDoggieStyle.WhoreToWashroomsMirrors".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieRepeatDoggieStyle.GroanTightWallsScreaming".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieRepeatDoggieStyle.FamiliarSensationDriveFaster".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieRepeatDoggieStyle.GiveLeaveSeedLeaking".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatDoggieStyle.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieRepeatDoggieStyle.ShakeOnWayOut".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatDoggieStyle.FuckingLoveBaconShakes".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LottieRepeatWalkies()
        {
            ClearOutput();

            ShowImage("lottie-fuck-her-in-public-repeat");

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatWalkies.WhoreOutsideBackGym".Localized());

            OutputText("Scenes.Lottie.LottieRepeatWalkies.LookBackToShed".Localized());

            if (OptionStore.Instance.Get("urtaDisabled", 0) > 0)
            {
                OutputText("Scenes.Lottie.LottieRepeatWalkies.WalkFamiliarBackAlley".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.LottieRepeatWalkies.BaconBaconGottaHave".Localized());
                else
                    OutputText("Scenes.Lottie.LottieRepeatWalkies.HeyyyBuddyShexSlave".Localized());

                OutputText("Scenes.Lottie.LottieRepeatWalkies.GiveDrunkTreat".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Lottie.LottieRepeatWalkies.LeaveVagrantJerksOff".Localized());
            }
            else
            {
                OutputText("Scenes.Lottie.LottieRepeatWalkies.WalkFamiliarAlleyAgain".Localized());

                if (!silly)
                    OutputText("Scenes.Lottie.LottieRepeatWalkies.HeyyyShexSlaveShumthin".Localized());
                else
                    OutputText("Scenes.Lottie.LottieRepeatWalkies.HeyyyShmellBacon".Localized());

                OutputText("Scenes.Lottie.LottieRepeatWalkies.GiveDrunkVixenTreat".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Lottie.LottieRepeatWalkies.LeaveUrtaJerksOff".Localized());
            }

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatWalkies.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieRepeatWalkies.Shake".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatWalkies.SureLoveSpitRoasting".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 1f));

            ReturnToCamp(1);
        }

        public void LottieRepeatCowGirl()
        {
            ClearOutput();

            ShowImage("lottie-fuck-her-cowgirl-repeat");

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatCowGirl.BothYogaRoomsMat".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 40)
                OutputText("Scenes.Lottie.LottieRepeatCowGirl.AmpleCheeksRestingGroin".Localized());
            else
                OutputText("Scenes.Lottie.LottieRepeatCowGirl.OnlyReachFarFilled".Localized());

            OutputText("Scenes.Lottie.LottieRepeatCowGirl.TakeMattersOwnHands".Localized());

            OutputText("Scenes.Lottie.LottieRepeatCowGirl.AboutExplodeTightGrip".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.LottieRepeatCowGirl.YourOtherCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.LottieRepeatCowGirl.Does".Localized());
                else
                    OutputText("Scenes.Lottie.LottieRepeatCowGirl.PluralSDoes".Localized());

                OutputText("Scenes.Lottie.LottieRepeatCowGirl.NotDisappointSpurtingEssence".Localized());
            }

            OutputText("Scenes.Lottie.LottieRepeatCowGirl.BothLayMatPanting".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieRepeatCowGirl.WhileLaterSaidGoodbyes".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatCowGirl.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieRepeatCowGirl.Shake".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatCowGirl.LoveBaconShakes".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LottieRepeatQuickie()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatQuickie.FollowOutPoolArea".Localized());

            OutputText("Scenes.Lottie.LottieRepeatQuickie.NudeGirlStandsBlushing".Localized());

            OutputText("Scenes.Lottie.LottieRepeatQuickie.MomentsFlatRemoveClothes".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.LottieRepeatQuickie.YourOtherCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.LottieRepeatQuickie.Twitches".Localized());
                else
                    OutputText("Scenes.Lottie.LottieRepeatQuickie.PluralSTwitch".Localized());

                OutputText("Scenes.Lottie.LottieRepeatQuickie.InEcstasyReleasingButt".Localized());
            }

            OutputText("Scenes.Lottie.LottieRepeatQuickie.ParagraphBreak".Localized());

            OutputText("Scenes.Lottie.LottieRepeatQuickie.SweatyPantingLiesFloor".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatQuickie.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieRepeatQuickie.ShakeOnWayOut".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatQuickie.FuckingDelicious".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LottieRepeatThreesome()
        {
            ClearOutput();

            ShowImage("lottie-fuck-her-threesome-repeat");

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatThreesome.BothYogaRoomsElle".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatThreesome.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieRepeatThreesome.ShakeOnWayOut".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatThreesome.OnlyNeedOne".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LottieRepeatSUPERNEATORARES()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.BothYogaRoomsElle".Localized());

            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.CaressesWithTwoHands".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Balls > 0)
                    OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.Balls".Localized());
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.Pussy".Localized());
                    else
                        OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.Ass".Localized());
            }
            else
                OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.UsesAllFourHands".Localized());

            OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.LottieStandsSobbingFingering".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.YourExtraCock".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.ThrobsSpillingEssenceFloor".Localized());
                else
                    OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.PluralThrobSpillingEssence".Localized());
            }

            OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.ElleFollowsSuitPeak".Localized(
                Player.CockDescript(x)
            ));

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.ShakeOnWayOut".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatSUPERNEATORARES.OnlyNeedOne".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 2f));

            ReturnToCamp(1);
        }

        public void LottieRepeatBackwardsAssCowgirl()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatBackwardsAssCowgirl.BothYogaAreaSmell".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieRepeatBackwardsAssCowgirl.ReleaseApproachingFinalPush".Localized(
                Player.CockDescript(x)
            ));

            if (silly)
                OutputText("Scenes.Lottie.LottieRepeatBackwardsAssCowgirl.Bacon".Localized());

            OutputText("Scenes.Lottie.LottieRepeatBackwardsAssCowgirl.ShakeFromCafeFantastic".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void LottieRepeatFEMDOMFEMFDOM()
        {
            ClearOutput();

            int x = Player.CockThatFits(40);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.BothYogaAreaSmell".Localized());

            OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.WakeDazedForeheadThrobbing".Localized());

            OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.SlapLeanPigGirl".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.Tits".Localized());
            else
                OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.Belly".Localized());

            OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.SqueezingPuttyReachedGoal".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.VelvetGlovedHandsTease".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() <= 500)
            {
                OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.NodFeverishlyCum".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                if (silly)
                    OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.BaconAgain".Localized());

                OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.ShakeBeforeHeadHome".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.FuckingLoveBaconShakes".Localized());
            }
            else
            {
                OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.TooMuchBuiltThrust".Localized(
                    Player.CockDescript(x)
                ));

                if (silly)
                    OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.BaconAgain".Localized());

                OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.ShakeBeforeHomeAgain".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.LottieRepeatFEMDOMFEMFDOM.FuckingBaconShakesAgain".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void HugTheShitOutOfYourHam()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.HugTheShitOutOfYourHam.StandUpTellCome".Localized());

            if (LottieMorale() < 70)
                LottieMorale(1);

            if (LottieMorale() <= 30 && !Player.IsPureEnough(75))
            {
                OutputText("Scenes.Lottie.HugTheShitOutOfYourHam.TellWakeSlapConsciousness".Localized());

                OutputText("Scenes.Lottie.HugTheShitOutOfYourHam.WishesLifeWillChange".Localized());

                LottieMorale(-4);
            }

            ReturnToCamp(1);
        }

        public void TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK()
        {
            ClearOutput();

            ShowImage("lottie-fuck-her-with-tentacle-dick");

            int x = -1;

            int y = -1;

            int z = -1;

            int zz = -1;

            int zzz = -1;

            int zzzz = -1;

            int zzzzz = -1;

            int zzzzzz = -1;

            int zzzzzzz = -1;

            int zzzzzzzz = -1;

            int counter = 0;

            while (counter < Player.CockTotal())
            {
                if (Player.Cocks[counter].Id == new Identifier("coc", "tentacle"))
                    if (x == -1)
                        x = counter;
                    else
                        if (y == -1)
                            y = counter;
                        else
                            if (z == -1)
                                z = counter;
                            else
                                if (zz == -1)
                                    zz = counter;
                                else
                                    if (zzz == -1)
                                        zzz = counter;
                                    else
                                        if (zzzz == -1)
                                            zzzz = counter;
                                        else
                                            if (zzzzz == -1)
                                                zzzzz = counter;
                                            else
                                                if (zzzzzz == -1)
                                                    zzzzzz = counter;
                                                else
                                                    if (zzzzzzz == -1)
                                                        zzzzzzz = counter;
                                                    else
                                                        if (zzzzzzzz == -1)
                                                            zzzzzzzz = counter;

                counter += 1;
            }

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.NotLongTakenShed".Localized());

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.PorcineGirlTakesInitiative".Localized());

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.RemoveArmorHangVine".Localized(
                Player.MultiCockDescriptLight(),

                Player.Mf("big guy", "hon")
            ));

            if (LottieMorale() <= 30)
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.GloomyStarePlungesHead".Localized());
            else
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.RaisesEyebrowDoesntProbe".Localized());

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.SentencePeriodParagraph".Localized());

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.GraspBaseCaressFace".Localized(
                Player.CockDescript(x),

                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.FirmlySqueezeYour".Localized());

            if (Player.CountCocksOfType("TENTACLE") > 1)
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.First".Localized());

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.WrithingMemberSnakingProbing".Localized());

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.GroansPleasureLikeCock".Localized());

            if (y >= 0)
            {
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.ExtraLeftVacantHoles".Localized(
                    Player.CockDescript(y)
                ));

                if (Player.CockArea(y) <= 80)
                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.EntiretyOfYour".Localized(
                        Player.CockDescript(y)
                    ));
                else
                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.AsManyInchesYour".Localized(
                        Player.CockDescript(y)
                    ));

                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.TearingMammothMakesRoom".Localized());

                if (z >= 0)
                {
                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.ConstantScreamingReminds".Localized(
                        Player.CockDescript(z),

                        Player.CockDescript(z)
                    ));

                    if (LottieMorale() < 40)
                        OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.PigSlutLovesUsed".Localized());
                    else
                        OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.PigLovesWhatDoing".Localized());

                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.BunchTightFuckHoles".Localized());

                    if (zz >= 0)
                    {
                        OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.TwatHaveRoomLeft".Localized(
                            Player.CockDescript(zz)
                        ));

                        if (zzz >= 0)
                        {
                            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.DrippingExtraDickBackside".Localized(
                                Player.CockDescript(zzz),

                                Player.CockDescript(y),

                                Player.CockDescript(zzz)
                            ));

                            if (zzzz >= 0)
                            {
                                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.NoMatterFitMore".Localized(
                                    Player.CockDescript(zzzz),

                                    Player.CockDescript(zzzz),

                                    Player.CockDescript(zzzz)
                                ));

                                if (zzzzz >= 0)
                                {
                                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.IdlyCountExtraWays".Localized(
                                        Player.CockDescript(zzzzz)
                                    ));

                                    if (zzzzzz >= 0)
                                        OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.SeeingEffectRemainingHand".Localized(
                                            Player.CockDescript(zzzzzz)
                                        ));

                                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.KeepsStrokingCompulsiveWay".Localized());

                                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.ParagraphBreak".Localized());

                                    if (zzzzzzz >= 0)
                                        OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.AnotherCockLeftUnsatisfied".Localized());
                                }
                            }
                        }
                    }
                }
            }

            OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.ContinueShoveBrokenSlut".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.WarmFloodCumPlug".Localized());
            else
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.StandTremblingAfterEffects".Localized(
                    Player.CockDescript(x)
                ));

            if (LottieMorale() >= 99)
            {
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.LaughSheTriesMove".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.BeforeLeaveBaconShake".Localized());

                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.ParagraphBreakAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.TurnSeeSlutSnorting".Localized());

                if (silly)
                    OutputText("Scenes.Lottie.TentaDickLottieButtFuckbuttFuckButtFUCKBUTTFUCKBUTTFUCK.UnfortunatelyForSpiderClerk".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void IfrisAndLottieGetsNTRedLiftingWeights()
        {
            ClearOutput();

            ShowImage("lottie-ifris-threesome");

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.ReplySwaggerFlabAway".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.IfrisSwaysUsualPosition".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.TrimWomanReturnsGaze".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.StartSwiftlyClearHead".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.AskStopSomeoneHandle".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.FinallyRestraintsLiftsAir".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.IfrisWorkedUpEyes".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.EarlierLubricationGlideThroat".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.SoundSpeedsGirlishMoans".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.NeedySubjectWatchingFinish".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.StandBenchHoldingIfris".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.ObeysBlushingLook".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.GagsNoisilySmallShrieks".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.PaceDoesntBallsSmack".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.DerangedChuckleIfrisThrown".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.SweatyGonadsColliding".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.LottieCryingOutEcstasy".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.LegsWeakArmsSlime".Localized());

            OutputText("Scenes.Lottie.IfrisAndLottieGetsNTRedLiftingWeights.SwayingFewSecondsBlack".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Corruption, 5f));

            LottieAndIfrisNTREpilogue();
        }

        public void LottieAndIfrisNTREpilogue()
        {
            ClearOutput();

            OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.DryWindRousesOutside".Localized());

            OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.DearNameSleepWell".Localized());

            if (SceneInstance<UrtaScene>.Get().UrtaLove())
            {
                OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.FinePublicIndecency".Localized());

                OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.CaptainOfTheGuard".Localized());

                if (Player.Gems > 0)
                {
                    Player.Gems -= 200;

                    if (Player.Gems < 0)
                        Player.Gems = 0;
                }
            }
            else
                OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.ShowOfFining".Localized());

            OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.SayEndParagraph".Localized());

            OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.SatisfiedDressedStrutCamp".Localized());

            if (silly)
                OutputText("Scenes.Lottie.LottieAndIfrisNTREpilogue.DamnSureBaconShake".Localized());

            ReturnToCamp(1);
        }

        public void LottiesThirtyMinutePigGasm()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Lottie.LottiesThirtyMinutePigGasm.GreetsUsualCheerfulFashion".Localized());

            OutputText("Scenes.Lottie.LottiesThirtyMinutePigGasm.SmilingBackWarmGreeting".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottiesThirtyMinutePigGasm.ObliviousOglingPlumpRump".Localized());

            OutputText("Scenes.Lottie.LottiesThirtyMinutePigGasm.TimeToPorkBitch".Localized());

            OutputText("Scenes.Lottie.LottiesThirtyMinutePigGasm.StopStrokingWickedLook".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Lottie.LottiesThirtyMinutePigGasm.StandingVictoriousCantPull".Localized());

            OutputText("Scenes.Lottie.LottiesThirtyMinutePigGasm.HalfHourOrgasmJealous".Localized());

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

    }
}