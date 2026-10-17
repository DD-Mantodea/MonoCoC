using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Followers;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<JackFrostScene>("coc:jack_frost")]
    public class JackFrostScene : Scene
    {

        public static string Pluralize(int count, string singular, string? plural = null) => count == 1 ? singular : plural ?? singular + "s";

        public override void Entry()
        {
            MeetJackFrostInTheMountains();
        }

        public void MeetJackFrostInTheMountains()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.ExploreMountainousHeights".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.HikingUpMountain".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.SlinkCloser".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.InAPanicClaw".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.BeforeYouHandAround".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.IgnoresYouStroking".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.TakesAWhileFor".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.ApologizeForInterrupting".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.ScratchesNeckNods".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.NodInUnfeigned".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.SighLongingly".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.ShakesHeadThen".Localized());

            OutputText("Scenes.Xmasjackfrost.MeetJackFrostInTheMountains.WhatDoYouSay".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), JizzSnowOnMyCampPlease);

            AddButton(1, "Common.Button.No".Localized(), NoJizzingInMyCampPlease);

            AddButton(2, "Common.Button.Later".Localized(), NoJizzingInMyCampRightNow);
        }

        public void NoJizzingInMyCampRightNow()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.NoJizzingInMyCampRightNow.ThankTheGiant".Localized());

            OutputText("Scenes.Xmasjackfrost.NoJizzingInMyCampRightNow.ResumesFuriousMasturbation".Localized());

            OutputText("Scenes.Xmasjackfrost.NoJizzingInMyCampRightNow.NoReasonToLinger".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NoJizzingInMyCampPlease()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.NoJizzingInMyCampPlease.ThankTheGiant".Localized());

            OutputText("Scenes.Xmasjackfrost.NoJizzingInMyCampPlease.ResumesFuriousMasturbation".Localized());

            OutputText("Scenes.Xmasjackfrost.NoJizzingInMyCampPlease.NoReasonToLinger".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.JackFrostYear, DateTime.Now.Year);

            DoNext(() => ReturnToCamp(1));
        }

        public void JizzSnowOnMyCampPlease()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.JizzSnowOnMyCampPlease.LookAroundThenFind".Localized());

            OutputText("Scenes.Xmasjackfrost.JizzSnowOnMyCampPlease.ThankHimSurprisingly".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.JackFrostYear, DateTime.Now.Year);

            CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void ProcessJackFrostEvent()
        {
            ClearOutput();

            HideUpDown();

            if (CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) == 1)
            {
                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ApproachYourCamp".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 2);

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);
            }
            else
                if (FollowerRegistry.All.Count == 0)
                {
                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SighRemindsYou".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 0);

                    Player.HealthChange(Player.MaxHealth);

                    Player.ChangeFatigue(-100);

                    ReturnToCamp(1);
                }
                else

                    if (MarbleScene.FollowerRathazul() && CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) == 2)
                    {
                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.RathazulApproaches".Localized());

                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TellRathazulMet".Localized());

                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.RathazulSmiles".Localized());

                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AskIfHedLike".Localized());

                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.BeginGatheringSnow".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);

                        CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 3);

                        return ;
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1 && CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 3)
                        {
                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WonderWhereIzma".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MoundExplodes".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TellShesA".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TigersharkLaughsLeaps".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.GladShellLike".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.IzmaSighsDisappointment".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.NowAboutAlphaBeta".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.GladToDemonstrate".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.RunHandsAlong".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ClickTongueShaking".Localized((noFur ? "girl" : "morph")));

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.BeginTicklingBreasts".Localized());

                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PromiseHerTheyWont".Localized());

                            AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);

                            CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 4);

                            return ;
                        }
                        else

                            if ((SceneInstance<JojoScene>.Get().JojoFollower() || SceneInstance<JojoScene>.Get().CampCorruptJojo()) && CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 4)
                            {

                                if (SceneInstance<JojoScene>.Get().JojoFollower())
                                {
                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.JojoSittingUsual".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SoIsHeEnjoying".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MouseSaysCatching".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.OutOfMonksSight".Localized((noFur ? "" : "white-furred ")));

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SnowballExplodes".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CantResistLaughing".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FasterThanExpected".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CalmlyWipeSnow".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.JojoStatesBouncing".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LaughterEchoesTwoOf".Localized());

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);

                                    CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 5);
                                }
                                else
                                {
                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.OutInOutskirts".Localized());

                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CasuallyApproachBehind".Localized());

                                    ClearButtons();

                                    if (Player.HasCock())
                                        AddButton(0, "Common.Button.PenAss".Localized(), PenetradeCorruptJojoAss);

                                    if (Player.HasVagina())
                                        AddButton(1, "Common.Button.RideCock".Localized(), RideCorruptJojoCock);

                                    AddButton(2, "Common.Button.SpankNFinger".Localized(), SpankNFingerCorruptJojoAss);

                                    AddButton(4, "Common.Button.LeaveHim".Localized(), SkipCorruptJojo);
                                }

                                return ;
                            }
                            else

                                if (SceneInstance<AmilyScene>.Get().AmilyFollower() && CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 5)
                                {
                                    CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 6);

                                    if (!SceneInstance<AmilyScene>.Get().AmilyCorrupt())
                                    {
                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AmilyWarilyProwling".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ApproachingAskWhats".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AmilyHisses".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CantHelpButLaugh".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AsksInBafflement".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.NodYouMetSomeone".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MouseSmilesScampers".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LaughAndHugBack".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.GrinThenEnvelop".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.NotReallyPossible".Localized());

                                        ClearButtons();

                                        if (Player.HasCock())
                                            AddButton(0, "Common.Button.Penetrate".Localized(), PenetradePureMouseWaifu);

                                        AddButton(1, "Common.Button.RideCock".Localized(), MakeOutWithPureMouseWaifu);

                                        return ;
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.OutInOutskirtsWhere".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ApproachHerWithSmile".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LookingUpKneeling".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LaughTellingEnjoy".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.GaspsLoyalCumGuzzling".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PatHerOnHead".Localized());

                                        if ((int)Player.Gender > 0)
                                        {
                                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.StoppingToScratchChin".Localized());

                                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SmilesWithGlee".Localized());

                                            if (Player.HasCock())
                                            {
                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.StripOffLowerGarments".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AmilyBowsRespectfully".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PromptlyTakesHoldOf".Localized());

                                                if (Player.Balls > 0)
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.EvenTicklingYour".Localized());
                                                else
                                                    if (Player.HasVagina())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.EvenTicklingYourAdded".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SentencePeriod".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FeelingCloseYouPull".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ThankYouThanks".Localized());
                                            }
                                            else
                                            {
                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.StripGarmentsAgain".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.BowsThenSlides".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FeelingClosePullAgain".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TellCallingAmilyAttention".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ThankYouAgain".Localized());
                                            }
                                        }
                                    }

                                    ClearButtons();

                                    AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);

                                    return ;
                                }
                                else

                                    if (SceneInstance<EmberScene>.Get().FollowerEmber() && CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 6)
                                    {
                                        CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 7);

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.DragonWanderingCamp".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.HeardQuestionApproach".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.RepeatsCuriousAwed".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AskFirstTime".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.EmberNotesDepressed".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PerhapsLikeToPlay".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.EmberProtests".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WhoSaidJustFor".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.EmberSaysBlowing".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SmileBeginExplaining".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ByTimeYoureDone".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AgreesEagerKid".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ExplainRulesAnd".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SlippierThanRabbits".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.RatherThanComplaining".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.DragonPromptlyDrops".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LargeSnowBallFace".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PairOfClawFingered".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CoughingUpSnow".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.EmberBragsThenLowers".Localized());

                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WaveAwayBeingBeaten".Localized());

                                        ClearButtons();

                                        AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);
                                    }
                                    else

                                        if (CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 7 && SceneInstance<MarbleScene>.Get().MarbleAtCamp())
                                        {
                                            CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 8);

                                            int kids = CoC.Instance.Flags.Get(GameFlagIds.MarbleKids);

                                            if (kids > 0 && CoC.Instance.Flags.Get(GameFlagIds.MarbleNurseryConstruction) >= 100)
                                            {
                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FindMarbleStaring".Localized(
                                                    kids > 1 ? "are" : "is",

                                                    Pluralize(kids, "kid"),

                                                    kids > 1 ? "they are" : "she is",

                                                    Pluralize(kids, "girl"),

                                                    kids > 1 ? " themselves." : "s herself."
                                                ));

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MarbleLiftsHand".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.KnowWhatThis".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LeftAtALoss".Localized(
                                                    Pluralize(kids, "child", "children"),

                                                    Pluralize(kids, "kid")
                                                ));

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CertainlyDontMindStaying".Localized(
                                                    Pluralize(kids, "kid"),

                                                    kids > 1 ? "They listen" : "She listens"
                                                ));

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WhenMarbleReturns".Localized(
                                                    Pluralize(kids, "one")
                                                ));

                                                if (Player.Strength >= 85)
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WouldSeemYourStrength".Localized((kids > 1 ? "the girls" : "your daughter")));
                                                else
                                                    if (Player.Strength >= 60)
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SoonYouAndMarble".Localized((kids > 1 ? "the girls" : "your daughter")));
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.OfCourseGreatStrength".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AfterSledRidesEnd".Localized());
                                            }
                                            else
                                            {
                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FindMarbleStaringAgain".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.KnowWhatThisAgain".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LeftAtLossAgain".Localized());

                                                if (Player.Tallness > 75)
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MarbleLeansAgainst".Localized());
                                                else
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MarblePullsOnYou".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.StartToSpeakSlowly".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FinishAndSilence".Localized());
                                            }

                                            ClearButtons();

                                            AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);

                                            return ;
                                        }
                                        else

                                            if (CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 9 && (SceneInstance<HelScene>.Get().FuckBuddyHel() || SceneInstance<HelScene>.Get().FollowerHel()))
                                            {

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.HeliaCallsSees".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.GreetSalamanderLover".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FireyLizardAsks".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ExplainToHerIts".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.HeliaCheersSteam".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.HeliaDoesntListen".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CriesDoingHerBest".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.EnjoyBattleWithHelia".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SighsDisappointedComplies".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ColdNotWilling".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.RatherSurprisedWhen".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WipeSnowOff".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WithoutWordsFiery".Localized());

                                                if (Player.Speed / 20 + Random.Shared.Next(20) > 11)
                                                {
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SurprisedBySudden".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LaughsAsSurfaces".Localized());
                                                }
                                                else
                                                {
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.YelpInSurprise".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LaughsNotIfYou".Localized());
                                                }

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TwoOfYouToss".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.NotToSayHelia".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WipeSomeMoisture".Localized());

                                                if ((int)Player.Gender > 0)
                                                {
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AsEvidencedByWord".Localized());

                                                    if (Player.HasCock())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.YourErectCocks".Localized());

                                                    if (Player.Gender == Gender.Hermaphrodite)
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AndWord".Localized());
                                                    else
                                                        if (Player.HasVagina())
                                                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.YourWord".Localized());

                                                    if (Player.HasVagina())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.DrippingVagina".Localized());
                                                }

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SentencePeriodAgain".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.HeliaLaughsTurns".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.KissHerBack".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WhenYoureWithA".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.UnwillingToEngage".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SalamanderCriesOut".Localized());

                                                if (Player.HasCock())
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TorrentOfSeedSplashes".Localized());

                                                if (Player.HasVagina())
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.QuiversAndSpillJuices".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FinallyFinishCumming".Localized());

                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.InsinuatesGentlyTapping".Localized());

                                                CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 10);

                                                ClearButtons();

                                                AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);

                                                return ;
                                            }
                                            else

                                                if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower() && CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 10)
                                                {
                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.BovineIsabellaStaring".Localized());
                                                    // 原版 XmasJackFrost.hx:409 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.DaNameIAm".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.YesNameIAm".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SheRepliesWord".Localized());
                                                    // 原版 XmasJackFrost.hx:415 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.BackHomeMineLand".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.BackHomeOldLand".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.YesYouAgree".Localized());
                                                    // 原版 XmasJackFrost.hx:422 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SheMusesButThen".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MusesThenAgain".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.YochalotYouAsk".Localized());
                                                    // 原版 XmasJackFrost.hx:428 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.MakesShowRubbing".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ShowRubbingAgain".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.YochalotSoundsLikeChocolate".Localized());
                                                    // 原版 XmasJackFrost.hx:436 — Isabella 有口音时才说德国腔台词
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AtThisLooksCrestfallen".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LooksCrestfallenAgain".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PityButThenAgain".Localized());
                                                    // 原版 XmasJackFrost.hx:442 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LooksSurprisedGrins".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LooksSurprisedGrinsAgain".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SeeingAsYoureAlready".Localized());
                                                    // 原版 XmasJackFrost.hx:449 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SmilesAndCuddles".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SmilesAndCuddlesAgain".Localized());
                                                    // 原版 XmasJackFrost.hx:455 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LaughsBreastsJiggling".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.LaughsBreastsJigglingAgain".Localized());

                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SoundsLikeAPlan".Localized());
                                                    // 原版 XmasJackFrost.hx:462 — Isabella 有口音时换一种说法
                                                    if (SceneInstance<IsabellaFollowerScene>.Get().IsabellaAccent())
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SmilesAndReleasesArm".Localized());
                                                    else
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SmilesReleasesArm".Localized());

                                                    CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 11);

                                                    ClearButtons();

                                                    AddButton(0, "Common.Button.Next".Localized(), ProcessJackFrostEvent);
                                                }
                                                else

                                                    if (SceneInstance<KihaFollowerScene>.Get().FollowerKiha() && CoC.Instance.Flags.Get(GameFlagIds.JackFrostProgress) <= 11)
                                                    {

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.AhAhAhchoo".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SayToKiha".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.KihaSnifflesWipes".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.ExplainToKiha".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SneezesAgainSparks".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WellAsideFromGames".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.DemandsBlushing".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SwallowsHardHear".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SeeingThatYouSuggest".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.WasteNoTimeLeading".Localized());

                                                        CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 12);

                                                        ClearButtons();

                                                        if (Player.HasCockThatFits(67))
                                                            AddButton(0, "Common.Button.FuckHer".Localized(), KihaXmasFuck);

                                                        AddButton(1, "Common.Button.FingerHer".Localized(), KihaXmasFingering);
                                                    }
                                                    else
                                                    {
                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.SmallHolidayOfYours".Localized());

                                                        OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.OnlyThingLeft".Localized());

                                                        if (CampScene.LoversCount() + CampScene.SlavesCount() == 1)
                                                            OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TakeCarePreparations".Localized());
                                                        else
                                                            if (CampScene.LoversCount() + CampScene.SlavesCount() == 0)
                                                            {
                                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TakeCarePreparationsAgain".Localized());

                                                                if (FollowerRegistry.All.Count > 1)
                                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PluralS".Localized());

                                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.CanTellYouWere".Localized());

                                                                if (FollowerRegistry.All.Count > 1)
                                                                    OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.PluralSAgain".Localized());

                                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.GoodNightRetire".Localized());
                                                            }
                                                            else
                                                            {
                                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.TakeCarePreparationsThird".Localized());

                                                                OutputText("Scenes.Xmasjackfrost.ProcessJackFrostEvent.FeastProgresses".Localized());
                                                            }

                                                        Player.HealthChange(Player.MaxHealth);

                                                        Player.Hunger = 100;

                                                        Player.ChangeFatigue(-100);

                                                        CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 0);

                                                        ReturnToCamp(1);
                                                    } // 原版 XmasJackFrost.hx:130 Rathazul 跟随者       
        }

        public void KihaXmasFuck()
        {
            ClearOutput();

            SceneInstance<KihaFollowerScene>.Get().KihaSprite(true);

            var x = Player.CockThatFits(67);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.StrippingOffYour".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.StalksOverTailSwaying".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.LaughAndTellHer".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.WrySmileYouAsk".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.KihaDeclaresTeasing".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.ImpatientStartingToFeel".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.NotInterestedInDelaying".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.SmileAtDragonGirl".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.ImCloseKihaGasps".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFuck.OnceOrgasmSubsided".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ProcessJackFrostEvent());
        }

        public void KihaXmasFingering()
        {
            ClearOutput();

            SceneInstance<KihaFollowerScene>.Get().KihaSprite(true);

            OutputText("Scenes.Xmasjackfrost.KihaXmasFingering.ExamineKihaSnowflake".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFingering.TellHerNotWorry".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFingering.PayNoHeed".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFingering.ChuckleSendingVibrations".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFingering.BeginPistoningFingers".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFingering.ClickTongueMocking".Localized());

            OutputText("Scenes.Xmasjackfrost.KihaXmasFingering.CroonsSnugglingTighter".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Sensitivity / 20 + Player.Libido / 20)));

            DoNext(() => ProcessJackFrostEvent());
        }

        public void PenetradePureMouseWaifu()
        {
            ClearOutput();

            var x = Player.CockThatFits(50);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.StrippingDownIndicating".Localized(
                Player.Mf("man", "herm")
            ));

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.AmilyNotesDespite".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.NotToMentionDick".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.SentencePeriod".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.SittingDownBedroll".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.HerOwnJuttingOut".Localized(
                    ""
                ));

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.WrigglingIntoArms".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.SimplyGrinBeginBucking".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.GuideHandsDown".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.AskHerToTell".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.AndPaintingBellyWhite".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.SentencePeriodAgain".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.TellAmilyHugging".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradePureMouseWaifu.AgreeWithHerEnjoy".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            DoNext(() => ProcessJackFrostEvent());
        }

        public void MakeOutWithPureMouseWaifu()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.StripDownAndSuggest".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.SayWithAllThat".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.SmilesWithJoy".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.SillyMouseRemark".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.OpenArmsInA".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyWangLength) > 0)
                OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.SmilingSheepishlyTell".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.PromptlyWalksOver".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.GigglesWrigglingBack".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.SighsSoftlyIFeel".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.PutNoseAgainst".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.TellHerFeelsPerfect".Localized());

            OutputText("Scenes.Xmasjackfrost.MakeOutWithPureMouseWaifu.ChuckleEvenThough".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(30 + Player.Sensitivity / 10)));

            DoNext(() => ProcessJackFrostEvent());
        }

        public void PenetradeCorruptJojoAss()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.RoughlyPullMouseSlut".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.FeelingJustWasting".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.SlapHisAss".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.VoicesLoudMoaning".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.FeelingCloseRedouble".Localized());

            if (Player.CumQ() >= 700)
                OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.AllOverCumEngorged".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.OntoSnowyEarth".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.LaughingAtReaction".Localized());

            OutputText("Scenes.Xmasjackfrost.PenetradeCorruptJojoAss.GoodNowHeShould".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, -3f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ProcessJackFrostEvent());

            CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 5);
        }

        public void RideCorruptJojoCock()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.FlourishDiscard".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.MeeklyDeclaresUnable".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.TakeHisShaft".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.NowNowCantHave".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.ThatsBetterReleaseHim".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.ThatsWhatYouLike".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.GoesBerserkBabbling".Localized());

            Player.CuntChange(20, true, true, false);

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.ThrustBuckHeave".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.OwnFutaCockSpewing".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.OrgasmTriggersJojo".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.WaitUntilSure".Localized());

            OutputText("Scenes.Xmasjackfrost.RideCorruptJojoCock.JustStandAndWatch".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, -3f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ProcessJackFrostEvent());

            CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 5);

            Player.KnockUp(PregnancyIds.Jojo, PregnancyIds.MouseIncubation + 82);
        }

        public void SpankNFingerCorruptJojoAss()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.NotBotheringToUndress".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.PleaseSpankMe".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.WigglesBareAss".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.AdministerEchoingSlap".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.CantResistLaughingPointing".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.ReleaseHisTail".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.NoReasonToDeny".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.MoansAndGroansLustily".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.GaspAndAHowl".Localized());

            OutputText("Scenes.Xmasjackfrost.SpankNFingerCorruptJojoAss.LaughAtMouseSlut".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 5);

            DoNext(() => ProcessJackFrostEvent());
        }

        public void SkipCorruptJojo()
        {
            ClearOutput();

            OutputText("Scenes.Xmasjackfrost.SkipCorruptJojo.ShakeHeadAndLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.JackFrostProgress, 5);

            DoNext(() => ProcessJackFrostEvent());
        }

    }
}