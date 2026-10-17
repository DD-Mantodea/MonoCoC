using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<ScyllaScene>("coc:scylla")]
    public class ScyllaScene : Scene
    {

        public const int SCYLLA_NOT_PRESENT = 0;

        public const int SCYLLA_ACTION_FIRST_TALK = 1;

        public const int SCYLLA_ACTION_ROUND_TWO = 2;

        public const int SCYLLA_ACTION_ROUND_THREE = 3;

        public const int SCYLLA_ACTION_ROUND_FOUR = 4;

        public const int SCYLLA_ACTION_MEET_CATS = 5;

        public const int SCYLLA_ACTION_ADICTS_ANON = 6;

        public const int SCYLLA_ACTION_FLYING_SOLO = 7;

        public const int SCYLLA_ACTION_FUCKING_URTA = 8;

        public const int SCYLLA_ACTION_FURRY_FOURSOME = 9;

        public int scyllaAction = 0;

        public int scyllaLastActionSelectionTime = 0;

        public int action => scyllaAction;

        public override void Entry()
        {
            ScyllaSprite();
        }

        public void ScyllaSprite()
        {

        }

        public void ScyllaBarSelectAction()
        {

            if (View.Time.TotalTime == scyllaLastActionSelectionTime)
                return ;

            scyllaLastActionSelectionTime = (int)(View.Time.TotalTime);

            scyllaAction = SCYLLA_NOT_PRESENT;

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                return ;

            if ((!Player.HasCock() || Player.BiggestCockLength() < 12) && CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) == 0)
            {
                scyllaAction = SCYLLA_ACTION_FIRST_TALK;

                return ;
            }

            if (Player.Cocks.Count > 0)
            {
                if (Player.BiggestCockLength() >= 12)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) == 0)
                    {
                        scyllaAction = SCYLLA_ACTION_FIRST_TALK;

                        return ;
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) == 1 && Random.Shared.Next(5) == 0)
                    {
                        scyllaAction = SCYLLA_ACTION_ROUND_TWO;

                        return ;
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) == 2 && Random.Shared.Next(5) == 0)
                    {
                        scyllaAction = SCYLLA_ACTION_ROUND_THREE;

                        return ;
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) == 3 && Random.Shared.Next(5) == 0)
                    {
                        scyllaAction = SCYLLA_ACTION_ROUND_FOUR;

                        return ;
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) == 5 && Random.Shared.Next(5) == 0)
                    {
                        scyllaAction = SCYLLA_ACTION_MEET_CATS;

                        return ;
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) >= 4 && (View.Time.Hours == 18 || View.Time.Hours == 19))
                    {
                        scyllaAction = SCYLLA_ACTION_ADICTS_ANON;

                        return ;
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) >= 2 && CoC.Instance.Flags.Get(GameFlagIds.FedScyllaToday) == 0 && View.Time.Hours >= 7 && View.Time.Hours <= 11)
                    {
                        scyllaAction = SCYLLA_ACTION_FLYING_SOLO;

                        return ;
                    }
                }

                if (OptionStore.Instance.Get("urtaDisabled", 0) > 0)
                    return ;

                if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) < 3)
                    return ;

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaBannedFromScylla) == 1 && CoC.Instance.Flags.Get(GameFlagIds.UrtaConfrontedScylla) == 1)
                    return ;
                // 原版 ScyllaScene.hx：Urta 在酒吧或仍在生 PC 的气时不触发
                if (SceneInstance<UrtaScene>.Get().UrtaAtBar()
                    || CoC.Instance.Flags.Get(GameFlagIds.UrtaAngryAtPcCountdown) > 0)

                    return ;

                if (CoC.Instance.Flags.Get(GameFlagIds.UrtaTimeSinceLastCame) > 0 || CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) <= 2 || CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedUrta) == 0)
                    return ;

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesCaughtUrtaWithScylla) == 0)
                {
                    if (SceneInstance<UrtaScene>.Get().UrtaDrunk())
                        return ;
                }
                else
                    if (SceneInstance<UrtaScene>.Get().UrtaDrunk() && Player.Balls == 0)
                        return ;

                if (SceneInstance<KatherineScene>.Get().NeedIntroductionFromScylla())
                    return ;

                if (Random.Shared.Next(3) == 0)
                    scyllaAction = SCYLLA_ACTION_FUCKING_URTA;

                if (Random.Shared.Next(2) == 0 && CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) >= 3 && !Player.HasKeyItem("Opal Ring") && SceneInstance<UrtaScene>.Get().UrtaAtBar() && Player.BiggestCockLength() >= 8 && CoC.Instance.Flags.Get(GameFlagIds.TimesCaughtUrtaWithScylla) > 0)
                    scyllaAction = SCYLLA_ACTION_FURRY_FOURSOME;
            }
        }

        public void TalkToScylla()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-first-meeting");

            OutputText("Scenes.Scylla.TalkToScylla.ApproachBustyStranger".Localized());

            if (!Player.HasCock() || Player.BiggestCockLength() < 12)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaSmallcockIntro) == 0)
                {
                    OutputText("Scenes.Scylla.TalkToScylla.StepUpIntroduceHat".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.ScyllaSmallcockIntro, 1);
                }
                else
                    OutputText("Scenes.Scylla.TalkToScylla.NunGivenUpPrayer".Localized());

                ReturnToCamp(1);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaSmallcockIntro) == 0)
                OutputText("Scenes.Scylla.TalkToScylla.IntroduceSelfNoticeHat".Localized());
            else
                OutputText("Scenes.Scylla.TalkToScylla.ApproachNunFullAttention".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                OutputText("Scenes.Scylla.TalkToScylla.WasHolyWomanOnce".Localized());
            else
                OutputText("Scenes.Scylla.TalkToScylla.OnceHolyWoman".Localized());

            OutputText("Scenes.Scylla.TalkToScylla.FingertipsAlongHorns".Localized());

            OutputText("Scenes.Scylla.TalkToScylla.SureKnowWhatDrank".Localized());

            OutputText("Scenes.Scylla.TalkToScylla.ClearlyAddictedSemen".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), HelpScylla);

            AddButton(1, "Common.Button.No".Localized(), DontHelpScylla);
        }

        public void HelpScylla()
        {
            ClearOutput();

            ScyllaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.NumberOfTimesMetScylla, CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) + 1);

            var x = Player.BiggestCockIndex();

            ShowImage("scylla-first-help");

            OutputText("Scenes.Scylla.HelpScylla.ExplainRunIntoBefore".Localized());

            OutputText("Scenes.Scylla.HelpScylla.SqueaksCrossesLegs".Localized());

            OutputText("Scenes.Scylla.HelpScylla.ReluctantlyKneels".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Scylla.HelpScylla.BodyTensesFingersDig".Localized());

            OutputText("Scenes.Scylla.HelpScylla.SwallowsMoreInches".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.HelpScylla.BallsTightening".Localized());

            OutputText("Scenes.Scylla.HelpScylla.TwitchingTiltsHeadSwallows".Localized());

            OutputText("Scenes.Scylla.HelpScylla.NoTimeOrgasmBurst".Localized());

            OutputText("Scenes.Scylla.HelpScylla.SpurtsSlowLockEyes".Localized(
                Player.Balls > 0 ? "is immediately apparent as your balls expand as if filling with helium. You are obliged to slide off the bed a bit as they droop down and rest against the ground, as large as barrels. Scylla's lips inflate with a whorish sucking noise and now block your sight of every part of her face but the desperate, hungry blue eyes. Her tongue must've grown in her mouth, because you can feel it wrapping around your dick like a coiling serpent, pulling tighter and tighter before loosening in a milking motion. Your overstuffed balls immediately respond. A bulge of cum as large as a baseball works its way past her suckling lips and down her long, distended neck. The sensation of the cum spraying out of your " + Player.CockHead() + " causes your toes to curl and makes you clench your fist so hard that blood leaks between your fingers." : "seems to flow into you with a hellish warmth. The subtle changes of her hungering magic worms its way into your prostate and the swelling sensation obliges you to slide to the edge of the bed, a shuddering need for release wrapping iron bands around your lungs. Scylla's lips inflate with a whorish sucking noise and now block your sight of every part of her face but the desperate, hungry blue eyes. Her tongue must've grown in her mouth, because you can feel it wrapping around your dick like a coiling serpent, pulling tighter and tighter before loosening in a milking motion. Your entranced body immediately responds. A bulge of cum as large as a baseball works its way past her suckling lips and down her long, distended neck. The sensation of the cum spraying out of your " + Player.CockHead() + " causes your toes to curl and makes you clench your fist so hard that blood leaks between your fingers."
            ));

            OutputText("Scenes.Scylla.HelpScylla.MilkingGlobAfterGlob".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.HelpScylla.BallsQuiver".Localized());
            else
                OutputText("Scenes.Scylla.HelpScylla.BodyQuivers".Localized());

            OutputText("Scenes.Scylla.HelpScylla.MindShattersGrabHorns".Localized());

            OutputText("Scenes.Scylla.HelpScylla.ImpossiblyEmptyPassOut".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 4f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 2f));

            Player.CumMultiplier += 2;

            ReturnToCamp(1);
        }

        public void DontHelpScylla()
        {
            ClearOutput();

            ScyllaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.NumberOfTimesMetScylla, CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) - 1);

            OutputText("Scenes.Scylla.DontHelpScylla.TellAddictedTurnAway".Localized());

            OutputText("Scenes.Scylla.DontHelpScylla.WrestleDecisionWalkBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -2f), new DynStat(GameStats.Lust, -99f));

            ReturnToCamp(1);
        }

        public void ScyllaRoundII()
        {
            ClearOutput();

            ScyllaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.FedScyllaToday, 1);

            CoC.Instance.Flags.Set(GameFlagIds.NumberOfTimesMetScylla, CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) + 1);

            ShowImage("scylla-help-round-two");

            OutputText("Scenes.Scylla.ScyllaRoundII.WetBitchParticularlyBusy".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundII.DropGemsBuyWine".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundII.LiftsHeadInnocentEyes".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundII.CheeksFlushReadNote".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundII.DespiteToneWroteHours".Localized());

            ScyllaRoundIIPartII();
        }

        public void ScyllaRoundIIPartII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-two-part-two");

            OutputText("Scenes.Scylla.ScyllaRoundIIPartII.StrokeHairLingerEars".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIIPartII.TremblingHandsLiftsShaft".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIIPartII.CavernousWaistHeadRibs".Localized());

            if (Player.HasKnot(0))
                OutputText("Scenes.Scylla.ScyllaRoundIIPartII.UnableHoldKnotBalloons".Localized());
            else
                if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Scylla.ScyllaRoundIIPartII.HissHorseCockFlares".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIIPartII.FeelOrgasmBuildingClench".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIIPartII.SomethingWrongCantCum".Localized());

            if (Player.HasKnot(0))
                OutputText("Scenes.Scylla.ScyllaRoundIIPartII.PullBackBloatedKnot".Localized());
            else
                OutputText("Scenes.Scylla.ScyllaRoundIIPartII.TryPullRingOff".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIIPartII.WithoutReleaseSealed".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 100f));

            ScyllaRoundIIPartIII();
        }

        public void ScyllaRoundIIPartIII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-two-part-three");

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.ScyllaRoundIIPartIII.FamishedNunWhimpersSwallows".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundIIPartIII.LoseTrackOfTime".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundIIPartIII.RollOverPumpingBalls".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaRoundIIPartIII.FamishedNunWhimpersAgain".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundIIPartIII.LoseTrackTime".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundIIPartIII.SummonStrengthRollOver".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 4f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 2f));

            Player.CumMultiplier += 2;

            if (View.Time.Hours > 5)
                View.Time.Days += 1;

            View.Time.Hours = 5;

            ReturnToCamp(1);
        }

        public void ScyllaRoundThreeCUM()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-two-jizz");

            CoC.Instance.Flags.Set(GameFlagIds.NumberOfTimesMetScylla, CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) + 1);

            if (!Player.HasKeyItem("Opal Ring") || CoC.Instance.Flags.Get(GameFlagIds.FedScyllaToday) == 1)
            {
                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.WetBitchSilence".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.NunInsideSmoothingHabit".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.PatsEmptyTummyDarn".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.QuestionsForHer".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Belly".Localized(), ScyllaIIIBellyChat);

                AddButton(1, "Common.Button.Breathing".Localized(), ScyllaIIIHOWZUBREETH);
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.WetBitchRelax".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.InsideDaintilySmoothing".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.PatsTummySecondRumble".Localized());

                OutputText("Scenes.Scylla.ScyllaRoundThreeCUM.QuestionsForHerAgain".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Belly".Localized(), ScyllaIIIBellyChat);

                AddButton(1, "Common.Button.Breathing".Localized(), ScyllaIIIHOWZUBREETH);
            }
        }

        public void ScyllaIIIBellyChat()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaIIIBellyChat.AskHowThirstBeen".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIBellyChat.StomachMoansBlushes".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIBellyChat.ShakesHeadNotSame".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIBellyChat.LookCloserGuessProtein".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIBellyChat.TurnSubjectBreasts".Localized());

            ScyllaIIIHOWZUBREETH();
        }

        public void ScyllaIIIHOWZUBREETH()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.BotheringYouPhrase".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.HandOnKneeYelps".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.NipplesLargerChanged".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.SqueaksShallowPenetration".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.PushNunBackStrip".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.PluralS".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.SlapDownOnBelly".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.AWord".Localized());
            else
                OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.YourWord".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.DickBetweenSlickMounds".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.WillpowerHoldBackWithdraw".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.IntrusionSwollenTitLips".Localized());

                OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.SurprisedTitsGrowing".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.SwollenTitLipsAgain".Localized());

                OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.SurprisedTitsGrowingAgain".Localized());
            }

            OutputText("Scenes.Scylla.ScyllaIIIHOWZUBREETH.CollapseAtopMammothTits".Localized());

            ScyllaLevelIIIRoundIIIFIGHT();
        }

        public void ScyllaLevelIIIRoundIIIFIGHT()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-three");

            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.RichnessReinvigorated".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.ThroesAnotherOrgasmBalls".Localized());

                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.TitsOneLastThrust".Localized());
                }
                else
                {
                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.ThroesAnotherOrgasmGut".Localized());

                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.TitsLastThrustAgain".Localized());
                }

                OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.StrangledNoiseSwallowsLoad".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.NurseTitsArmsWrap".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.TwoWord".Localized());
                else
                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.BothWord".Localized());

                OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.DicksAgainstNippleLips".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.BallsFillingGritTeeth".Localized());

                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.BallsEruptShaftsVibrate".Localized());
                }
                else
                {
                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.ProstateFillingGritTeeth".Localized());

                    OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.ShaftsVibrateCumRushes".Localized());
                }

                OutputText("Scenes.Scylla.ScyllaLevelIIIRoundIIIFIGHT.StillCummingCollapse".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 4f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 2f));

            Player.CumMultiplier += 2;

            if (Player.Balls > 0)
                Player.BallSize += 2;

            ScyllaIIIFinisher();
        }

        public void ScyllaIIIFinisher()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaIIIFinisher.RouseFromSlumberUnderstand".Localized());

            OutputText("Scenes.Scylla.ScyllaIIIFinisher.GushesThankYouRescue".Localized());

            ReturnToCamp(4);
        }

        public void ScyllaRoundIVGo()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-four");

            CoC.Instance.Flags.Set(GameFlagIds.NumberOfTimesMetScylla, CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) + 1);

            OutputText("Scenes.Scylla.ScyllaRoundIVGo.TavernDecentCrowd".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVGo.ShouldHaveOrderedFood".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVGo.ConsciousnessReturnsStoneRoom".Localized());

            ScyllaRoundIVPtII();
        }

        public void ScyllaRoundIVPtII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-four-pt-two");

            OutputText("Scenes.Scylla.ScyllaRoundIVPtII.CallAnsweredScraping".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtII.GlanceWarilyMountainsForest".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtII.HazeBurnedAwayLook".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtII.ReachHumbleQuartersCot".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            ScyllaRoundIVPtIII();
        }

        public void ScyllaRoundIVPtIII()
        {
            ClearOutput();

            ScyllaSprite();

            HideUpDown();

            ShowImage("scylla-help-round-four-pt-three");

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIII.ContactBrokenBrimstone".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIII.TauntsWeakMindedChampion".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIII.ScyllaInterposesHerself".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIII.ImpLordCoylyPlays".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIII.StungObservationSpineStiffens".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIII.SitsHeavilyOnStool".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Watch".Localized(), ScyllaRoundIVPtIVWATCH);

            AddButton(1, "Common.Button.KissHer".Localized(), ScyllaPtIVKissPtI);
        }

        public void ScyllaRoundIVPtIVWATCH()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIVWATCH.LowersKneesHoversImp".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIVWATCH.TremblingNeedDemonicCock".Localized());

            OutputText("Scenes.Scylla.ScyllaRoundIVPtIVWATCH.ImpFaceGrimaceGrunts".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ScyllaWatchSecondPartPoorImp();
        }

        public void ScyllaWatchSecondPartPoorImp()
        {
            ClearOutput();

            ScyllaSprite();

            HideUpDown();

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            OutputText("Scenes.Scylla.ScyllaWatchSecondPartPoorImp.ImpRisesPullMember".Localized());

            OutputText("Scenes.Scylla.ScyllaWatchSecondPartPoorImp.ImpCursesSmartWhore".Localized());

            OutputText("Scenes.Scylla.ScyllaWatchSecondPartPoorImp.ImpCantControlThrusts".Localized());

            OutputText("Scenes.Scylla.ScyllaWatchSecondPartPoorImp.CriesBeginningsOfPanic".Localized());

            ScyllaWatchThirdPartOhShitSon();
        }

        public void ScyllaWatchThirdPartOhShitSon()
        {
            ClearOutput();

            ScyllaSprite();

            HideUpDown();

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Lust, 10f), new DynStat(GameStats.Corruption, -5f));

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.ScyllaWatchThirdPartOhShitSon.LooksUpEyeLevel".Localized());
            else
                OutputText("Scenes.Scylla.ScyllaWatchThirdPartOhShitSon.LooksUpLevelAgain".Localized());

            OutputText("Scenes.Scylla.ScyllaWatchThirdPartOhShitSon.ImpCantHoldOverstuffed".Localized());

            OutputText("Scenes.Scylla.ScyllaWatchThirdPartOhShitSon.CumOozesThreeMouths".Localized());

            OutputText("Scenes.Scylla.ScyllaWatchThirdPartOhShitSon.SpermFixSenseReturns".Localized());

            ScyllaPtIVEpilogue();
        }

        public void ScyllaPtIVKissPtI()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtI.StepUpNunKiss".Localized());

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtI.StepBackAppreciateScale".Localized());

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtI.RubCheekAgainstBoob".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            ScyllaPtIVKissPtII();
        }

        public void ScyllaPtIVKissPtII()
        {
            ClearOutput();

            ScyllaSprite();

            HideUpDown();

            Player.Orgasm("Dick");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtII.BreakSecondKissEntrapped".Localized(
                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtII.StrokesPalmsAndCheeks".Localized());

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtII.TongueCrimsonHungryDrool".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtII.LightHeadedBloodTrapped".Localized(
                Player.SMultiCockDesc()
            ));

            if (Player.Cocks.Count > 1)
                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Scylla.ScyllaPtIVKissPtII.OtherCockNoMouth".Localized());
                else
                    OutputText("Scenes.Scylla.ScyllaPtIVKissPtII.OtherCocksNoMouths".Localized());

            ScyllaPtIVKissPtIII();
        }

        public void ScyllaPtIVKissPtIII()
        {
            ClearOutput();

            ScyllaSprite();

            HideUpDown();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Scylla.ScyllaPtIVKissPtIII.ScrambleOutOfCocoon".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.ScyllaPtIVKissPtIII.LongTongueSeeksSack".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtIVKissPtIII.BallsPumpLastCum".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaPtIVKissPtIII.TongueSeeksShaftBase".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtIVKissPtIII.LoinsPumpLastCum".Localized());
            }

            ScyllaPtIVEpilogue();
        }

        public void ScyllaPtIVEpilogue()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaPtIVEpilogue.ThankPrepareTelAdre".Localized());

            ReturnToCamp(4);
        }

        public void ScyllaAdictsAnonV()
        {

            ScyllaSprite();

            ReturnToCamp(1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetScyllaInAddictionGroup) == 0)
                ScyllaPtVFirstTime();
            else
                ScyllaPtVRepeat();

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) < 5)
                CoC.Instance.Flags.Set(GameFlagIds.NumberOfTimesMetScylla, 5);
        }

        public void ScyllaPtVFirstTime()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-firt-time");

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.SpotAcrossCommonRoom".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.PeeringThroughCrackDrinks".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.AlmostDontNoticeGirl".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.SoftEnthusiasmUnfazed".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.FaerieShakesHeadLoud".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.TurnsGazeToGoblin".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.HandOverBulbousLips".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTime.GoblinWavesBlehSisterhood".Localized());

            ScyllaPtVFirstTimeII();
        }

        public void ScyllaPtVFirstTimeII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-firt-time-pt-two");

            OutputText("Scenes.Scylla.ScyllaPtVFirstTimeII.SaysHappilyIgnoringInsult".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTimeII.PastieSighsGoNext".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTimeII.HucksGlassOverShoulder".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTimeII.WeAllHaveChallenges".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVFirstTimeII.FeelShittySpying".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Share".Localized(), ScyllaPtVShare);

            AddButton(1, "Common.Button.Advantage".Localized(), ScyllaPtVTakeAdvantage);

            AddButton(14, "Common.Button.Leave".Localized(), ScyllaPtVLeave);
        }

        public void ScyllaPtVRepeat()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaPtVRepeat.AtBarFillingTray".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Share".Localized(), ScyllaPtVShare);

            AddButton(1, "Common.Button.Advantage".Localized(), ScyllaPtVTakeAdvantage);

            AddButton(14, "Common.Button.Leave".Localized(), ScyllaPtVLeave);
        }

        public void ScyllaPtVLeave()
        {
            ClearOutput();

            ScyllaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetScyllaInAddictionGroup, CoC.Instance.Flags.Get(GameFlagIds.TimesMetScyllaInAddictionGroup) + 1);

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f));

            if (Player.Corruption > 30)
                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));

            OutputText("Scenes.Scylla.ScyllaPtVLeave.TakeHighRoad".Localized());

            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void ScyllaPtVTakeAdvantage()
        {
            ClearOutput();

            ScyllaSprite();

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan) && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 1) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesMetScyllaInAddictionGroup, CoC.Instance.Flags.Get(GameFlagIds.TimesMetScyllaInAddictionGroup) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.TimesScyllaAddictGroupExploited, CoC.Instance.Flags.Get(GameFlagIds.TimesScyllaAddictGroupExploited) + 1);

                ScyllaVTakeAdvantageWithExgartuan();

                return ;
            }

            if (Player.Cocks.Count >= 3)
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesMetScyllaInAddictionGroup, CoC.Instance.Flags.Get(GameFlagIds.TimesMetScyllaInAddictionGroup) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.TimesScyllaAddictGroupExploited, CoC.Instance.Flags.Get(GameFlagIds.TimesScyllaAddictGroupExploited) + 1);

                AddictionAdvantageMultiDickGreatTimeParty();

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetScyllaInAddictionGroup, CoC.Instance.Flags.Get(GameFlagIds.TimesMetScyllaInAddictionGroup) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesScyllaAddictGroupExploited, CoC.Instance.Flags.Get(GameFlagIds.TimesScyllaAddictGroupExploited) + 1);

            ShowImage("scylla-help-round-five-take-advantage");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            if (Player.Corruption < 30)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            if (Player.Corruption < 60)
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.TooGoodDuckRoom".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.OpensDoorWhatPleasure".Localized(
                    Player.ShortName,

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.NoSensePlayingCoy".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.ReachesForShaftWorshipfully".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.NunPleadsHandsTightening".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.AbbyShootsBackGrip".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.PastieFiguresOutFlies".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.GivenNameInfuriates".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.DoorPleasureAgain".Localized(
                    Player.ShortName,

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.NoSenseBothHands".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.ReachesOutShaftPresents".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.NunPleadsTighteningAgain".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.AbbyShootsBack".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.PastieFiguresFliesAgain".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVTakeAdvantage.GivenNameInfuriatesAgain".Localized());
            }

            Player.HealthChange(-1);

            Player.ApplyStats(new DynStat(GameStats.Lust, -99f));

            ReturnToCamp(1);
        }

        public void ScyllaVTakeAdvantageWithExgartuan()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-take-advantage-with-exgartuan-one");

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan.HandOnDoorknobReady".Localized());

            if (Player.CockTotal() < 3)
                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan.ExgartuanHavingNone".Localized());
            else
                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan.ExgartuanHavingNoneAgain".Localized());

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan.GirlsStareBlankly".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            ScyllaVTakeAdvantageWithExgartuan2();
        }

        public void ScyllaVTakeAdvantageWithExgartuan2()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-take-advantage-with-exgartuan-two");

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan2.AbbyFirstMoveSeize".Localized());

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan2.ScyllaSuccumbsNext".Localized());

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan2.PastieIntoxicatedAssessment".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan2.AllThreeAttachedFiend".Localized());
            else
                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan2.AllThreeAttachedAgain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ScyllaVTakeAdvantageWithExgartuan3();
        }

        public void ScyllaVTakeAdvantageWithExgartuan3()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-take-advantage-with-exgartuan-three");

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan3.SlowBreathingHoldBack".Localized());

                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan3.FaerieGrindingTooMuch".Localized());

                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan3.PressureSwollenBallsForces".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan3.SlowBreathingHoldAgain".Localized());

                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan3.GrindingTooMuchAgain".Localized());

                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan3.PressureAbdomenForces".Localized());
            }

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan3.LongPastPointFluid".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            ScyllaVTakeAdvantageWithExgartuan4();
        }

        public void ScyllaVTakeAdvantageWithExgartuan4()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-take-advantage-with-exgartuan-four");

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan4.MindSwitchesOffInflating".Localized());

            if (Player.CockTotal() < 3)
                OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan4.DemonWithdrawnReforming".Localized());

            OutputText("Scenes.Scylla.ScyllaVTakeAdvantageWithExgartuan4.GatherStrengthWinceRise".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.CumMultiplier += 2;

            Player.ChangeStatusValue(StatusEffectIds.Exgartuan, 2, 24);

            ReturnToCamp(1);
        }

        public void ScyllaPtVShare()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-share");

            if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaTimesSharedInAddictGroup) == 0)
            {
                OutputText("Scenes.Scylla.ScyllaPtVShare.BlushPushDoorOpen".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVShare.GirlsSurprisedWelcoming".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaPtVShare.KnockPokeHead".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVShare.ResolveBestBehavior".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.ScyllaTimesSharedInAddictGroup, CoC.Instance.Flags.Get(GameFlagIds.ScyllaTimesSharedInAddictGroup) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetScyllaInAddictionGroup, CoC.Instance.Flags.Get(GameFlagIds.TimesMetScyllaInAddictionGroup) + 1);

            OutputText("Scenes.Scylla.ScyllaPtVShare.WhatAddictionDiscuss".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionTracker) >= 50 || Player.HasPerk(PerkIds.MinotaurCumAddict))
                AddButton(0, "Common.Button.Cum".Localized(), ScyllaPtVCum);
            else
                AddButton(0, "Common.Button.Cum".Localized(), null);

            if ((Player.HasPerk(PerkIds.MarblesMilk) || SceneInstance<MarbleScene>.Get().KnowAddiction > 0) && !Player.HasPerk(PerkIds.MarbleResistant))
                AddButton(1, "Common.Button.Milk".Localized(), ScyllaPtVMilk);
            else
                AddButton(1, "Common.Button.Milk".Localized(), null);

            if (Player.LibidoPercent >= 85 || Player.MinLibido >= 20)
                AddButton(2, "Common.Button.Sex".Localized(), ScyllaPtVSex);
            else
                AddButton(2, "Common.Button.Sex".Localized(), null);

            if (Player.CountCocksOfType("TENTACLE") >= 2)
                AddButton(3, "Scenes.Kitsune.DefeatTheKitsunes.ButtonTentacles".Localized(), ShareTentaclesWithScylla);
            else
                AddButton(3, "Scenes.Kitsune.DefeatTheKitsunes.ButtonTentacles".Localized(), null);

            AddButton(4, "Common.Button.None".Localized(), ScyllaPtVNoDiscussion);
        }

        public void ScyllaPtVNoDiscussion()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaPtVNoDiscussion.FailComeUpExcuse".Localized());

            ReturnToCamp(1);
        }

        public void ScyllaPtVMilk()
        {
            ClearOutput();

            ScyllaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.ScyllaMilkTherapyTimes, CoC.Instance.Flags.Get(GameFlagIds.ScyllaMilkTherapyTimes) + 1);

            ShowImage("scylla-help-round-five-with-milk-one");

            OutputText("Scenes.Scylla.ScyllaPtVMilk.TellAboutWhitneyFarm".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilk.AudienceEngrossedRollingLocks".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilk.RisesMushyHug".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilk.TryConsiderWordsHeat".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            ScyllaPtVMilkII();
        }

        public void ScyllaPtVMilkII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-with-milk-two");

            OutputText("Scenes.Scylla.ScyllaPtVMilkII.ReachHandCollarBone".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkII.SpitsBeerTryBra".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkII.LeanHotBreathGrin".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkII.BottomLipBloatsInflating".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkII.KneelCushionRightBreast".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            ScyllaPtVMilkIII();
        }

        public void ScyllaPtVMilkIII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-with-milk-three");

            OutputText("Scenes.Scylla.ScyllaPtVMilkIII.ClenchesEyesGroansMelons".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkIII.FaerieDartsPlungesStream".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkIII.GoblinDifficultyBreathing".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkIII.AsksAllRightWorry".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkIII.OrgasmImmediateMoan".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkIII.AbbyMistakesThroesPain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            ScyllaPtVMilkIV();
        }

        public void ScyllaPtVMilkIV()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-with-milk-four");

            OutputText("Scenes.Scylla.ScyllaPtVMilkIV.StomachFullPauseGlance".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkIV.RememberPastieTurnBack".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkIV.AbylonPushesOutCleavage".Localized());

            ScyllaPtVMilkV();
        }

        public void ScyllaPtVMilkV()
        {
            ClearOutput();

            ShowImage("scylla-help-round-five-with-milk-five");

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaPtVMilkV.EmbarrassedOutburstApologize".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVMilkV.PleasedWithSelfSkip".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -1f));

            ReturnToCamp(1);
        }

        public void ScyllaPtVCum()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-jizz-pt-one");

            CoC.Instance.Flags.Set(GameFlagIds.ScyllaCumTherapyTimes, CoC.Instance.Flags.Get(GameFlagIds.ScyllaCumTherapyTimes) + 1);

            OutputText("Scenes.Scylla.ScyllaPtVCum.CollectThoughtsBegin".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVCum.BuryFaceHandsAdmit".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVCum.AbbyPullingPlates".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVCum.CutsInMadSmile".Localized());

            ScyllaPtVCumII();
        }

        public void ScyllaPtVCumII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-jizz-pt-two");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Scylla.ScyllaPtVCumII.HopsDownRoleplay".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVCumII.HangingKicksOffBoots".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Scylla.ScyllaPtVCumII.LiftGoblinOverHead".Localized(
                Player.CockDescript(x)
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            ScyllaPtVCumIII();
        }

        public void ScyllaPtVCumIII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-jizz-pt-three");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Scylla.ScyllaPtVCumIII.BeforeAnotherKick".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.ScyllaPtVCumIII.RaggedDeliciousShudder".Localized());
            else
                OutputText("Scenes.Scylla.ScyllaPtVCumIII.RaggedDeliciousShudderAgain".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVCumIII.NailsUpWaist".Localized(
                Player.CockDescript(x)
            ));

            ScyllaPtVCumIV();
        }

        public void ScyllaPtVCumIV()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-jizz-pt-four");

            var x = Player.BiggestCockIndex();

            if (Player.Cocks.Count >= 2)
                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.ScyllaPtVCumIV.UnableToKeepAway".Localized(
                        Player.BallsDescriptLight(),

                        Player.CockDescript(1),

                        Player.CockDescript(x),

                        Player.CockDescript(1)
                    ));
                else
                    OutputText("Scenes.Scylla.ScyllaPtVCumIV.UnableKeepAwayAgain".Localized(
                        Player.CockDescript(1),

                        Player.CockDescript(x),

                        Player.CockDescript(1)
                    ));

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.ScyllaPtVCumIV.DarknessCreepsBallsQuiver".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Scylla.ScyllaPtVCumIV.DarknessCreepsHipsQuiver".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.Cocks.Count >= 2)
                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.ScyllaPtVCumIV.FiresRootOfCock".Localized(
                        Player.CockDescript(1),

                        Player.BallsDescriptLight()
                    ));
                else
                    OutputText("Scenes.Scylla.ScyllaPtVCumIV.FiresRootCockAgain".Localized(
                        Player.CockDescript(1)
                    ));

            OutputText("Scenes.Scylla.ScyllaPtVCumIV.LegLockKeepsRooted".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Scylla.ScyllaPtVCumIV.NunMurmursDefeatedMinotaur".Localized());

            Player.Orgasm("Dick");

            ScyllaPtVCumV();
        }

        public void ScyllaPtVCumV()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-jizz-pt-five");

            if (Player.SensitivityPercent > 10)
                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            OutputText("Scenes.Scylla.ScyllaPtVCumV.WeaklyProtestsTheft".Localized());

            ReturnToCamp(1);
        }

        public void ScyllaPtVSex()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-sex-pt-one");

            CoC.Instance.Flags.Set(GameFlagIds.ScyllaSexTherapyTimes, CoC.Instance.Flags.Get(GameFlagIds.ScyllaSexTherapyTimes) + 1);

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            OutputText("Scenes.Scylla.ScyllaPtVSex.UncomfortableTalkingInFront".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVSex.RetchingNoiseFingerThroat".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVSex.SmilesNotBestJudge".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVSex.GrumblesTooBig".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVSex.HedgesAvoidingNotBest".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVSex.HiccupsLaughsDontCare".Localized());

            ScyllaPtVSexII();
        }

        public void ScyllaPtVSexII()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-sex-pt-two");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Scylla.ScyllaPtVSexII.GaspsAlarmPastieStop".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVSexII.StuttersWringingHands".Localized());

            OutputText("Scenes.Scylla.ScyllaPtVSexII.ScreamsIntoGagOrgasm".Localized(
                Player.CockDescript(x)
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            ScyllaPtVSexIII();
        }

        public void ScyllaPtVSexIII()
        {
            ClearOutput();

            ShowImage("scylla-help-round-five-sex-pt-three");

            ScyllaSprite();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Scylla.ScyllaPtVSexIII.SetFeetDown".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.ScyllaPtVSexIII.WindUpOneMore".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVSexIII.AbylonSpasmsDown".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVSexIII.StartlingActionLoseControl".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVSexIII.ReleasesTesticlesRisesNurse".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.ScyllaPtVSexIII.WindUpStrikeAgain".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVSexIII.AbylonSpasmsDown".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Scylla.ScyllaPtVSexIII.StartlingActionPressesForward".Localized());

                OutputText("Scenes.Scylla.ScyllaPtVSexIII.ReleasesVacuumSuction".Localized());
            }

            OutputText("Scenes.Scylla.ScyllaPtVSexIII.PleaseStopCallingFat".Localized());

            Player.Orgasm("Dick");

            ScyllaPtVSexIV();
        }

        public void ScyllaPtVSexIV()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-sex-pt-four");

            OutputText("Scenes.Scylla.ScyllaPtVSexIV.FourReadyLeaveHug".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            ReturnToCamp(1);
        }

        public void AddictionAdvantageMultiDickGreatTimeParty()
        {
            ClearOutput();

            ShowImage("scylla-help-round-five-multi-cock-one");

            ScyllaSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesAdvantagedScyllaMulticocks) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesAdvantagedScyllaMulticocks, CoC.Instance.Flags.Get(GameFlagIds.TimesAdvantagedScyllaMulticocks) + 1);

                OutputText("Scenes.Scylla.AddictionAdvantageMultiDickGreatTimeParty.TooGoodPassKnock".Localized());

                OutputText("Scenes.Scylla.AddictionAdvantageMultiDickGreatTimeParty.CoyBothHands".Localized());

                OutputText("Scenes.Scylla.AddictionAdvantageMultiDickGreatTimeParty.SinksKneesReachesCock".Localized());

                AddictionScyllaTakeAdvantageDicksII();
            }
            else
            {
                OutputText("Scenes.Scylla.AddictionAdvantageMultiDickGreatTimeParty.WalkingInTheyreExpecting".Localized());

                AddictionScyllaTakeAdvantageDicksII();
            }
        }

        public void AddictionScyllaTakeAdvantageDicksII()
        {
            ClearOutput();

            ShowImage("scylla-help-round-five-multi-cock-two");

            ScyllaSprite();

            OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksII.SnortsAgeBeforeBeauty".Localized(
                Player.CockDescript(1),

                Player.CockDescript(2)
            ));

            OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksII.FingerOverNostrilLowers".Localized(
                Player.CockDescript(1),

                Player.CockDescript(2)
            ));

            OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksII.CumRopeNextBottle".Localized(
                Player.CockDescript(2),

                Player.CockDescript(2)
            ));

            OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksII.LooksAtSpunkLust".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)), new DynStat(GameStats.Corruption, 1f));

            AddictionScyllaTakeAdvantageDicksIII();
        }

        public void AddictionScyllaTakeAdvantageDicksIII()
        {
            ClearOutput();

            ShowImage("scylla-help-round-five-multi-cock-three");

            ScyllaSprite();

            OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksIII.CheckedOutMentionCum".Localized());

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksIII.BallsQuiverBuildingLoad".Localized(
                    Player.CockDescript(1),

                    Player.CockDescript(1)
                ));

                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksIII.CastsHabitDemonicNubs".Localized(
                    Player.CockDescript(1)
                ));
            }
            else
            {
                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksIII.HipsQuiverBuildingLoad".Localized(
                    Player.CockDescript(1),

                    Player.CockDescript(1)
                ));

                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksIII.CastsHabitNubsAgain".Localized(
                    Player.CockDescript(1)
                ));
            }

            OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksIII.PulsesUnsatisfiedStoopGrab".Localized(
                Player.CockDescript(2),

                Player.CockDescript(2),

                Player.CockDescript(2)
            ));

            Player.Orgasm("Dick");

            AddictionScyllaTakeAdvantageDicksBonusAndEpilogue();
        }

        public void AddictionScyllaTakeAdvantageDicksBonusAndEpilogue()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-help-round-five-multi-cock-four");

            var blah = new List<int>();

            if (Player.Cocks.Count >= 4)
                blah.Add(4);

            if (Player.CountCocksOfType("TENTACLE") > 0)
                blah.Add(5);

            if (Player.CountCocksOfType("DEMON") > 0)
                blah.Add(6);

            float select = blah[Random.Shared.Next(blah.Count)];

            if (select == 4)
            {
                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.PicksHerselfUpGiggling".Localized(
                    Player.CockDescript(3),

                    Player.CockDescript(2),

                    Player.CockDescript(1)
                ));

                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.GoofyTitteringLeapBackward".Localized());
            }

            if (select == 5)
            {
                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.CatchBreathNoticeBottle".Localized());

                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.StrokeTentacleCockSmile".Localized());

                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.PumpTipOnPastie".Localized());
            }

            if (select == 6)
            {
                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.SatisfiedTurnToLeave".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.UnfamiliarTinglingBase".Localized());
                else
                    OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.UnfamiliarTinglingBaseAgain".Localized());

                OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.CrescendoEndsLostControl".Localized());

                var temp = Player.Cocks.Count;

                while (temp > 0)
                {
                    temp--;

                    if (Player.Cocks[0].Id == BodyPartTypes.Demon)
                    {
                        Player.Cocks[0].Id = BodyPartTypes.Human;

                        Player.ApplyStats(new DynStat(GameStats.Corruption, -3f));

                        break;
                    }
                }
            }

            OutputText("Scenes.Scylla.AddictionScyllaTakeAdvantageDicksBonusAndEpilogue.RoomSlimyMess".Localized());

            ReturnToCamp(1);
        }

        public void Scylla6()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.NumberOfTimesMetScylla) < 6)
                CoC.Instance.Flags.Set(GameFlagIds.NumberOfTimesMetScylla, 6);

            OutputText("Scenes.Scylla.Scylla6.ForOnceNotCrowded".Localized());

            OutputText("Scenes.Scylla.Scylla6.StepOutTelAdre".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.Scylla6.GuidesPoliteDirections".Localized());
            else
                OutputText("Scenes.Scylla.Scylla6.GuidesPoliteDirectionsAgain".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));

            OutputText("Scenes.Scylla.Scylla6.GuidesDownAlleyBunny".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), Scylla6SLOW);

            if (Player.Speed + Player.Intelligence > 100)
                AddButton(1, "Common.Button.Duck".Localized(), Scylla6DUCK);
        }

        public void Scylla6DUCK()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6DUCK.TurnInTimeDrop".Localized());

            OutputText("Scenes.Scylla.Scylla6DUCK.CatsScatterInTerror".Localized());

            ReturnToCamp(1);
        }

        public void Scylla6SLOW()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6SLOW.TooLatePickpocketsDistraction".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) < 1)
                OutputText("Scenes.Scylla.Scylla6SLOW.LastThingSmallBell".Localized());

            OutputText("Scenes.Scylla.Scylla6SLOW.PeriodBreak".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));

            if (Player.BiggestLactation() >= 1 && Random.Shared.Next(2) == 0)
            {
                OutputText("Scenes.Scylla.Scylla6SLOW.AwakenLimbsUnresponsive".Localized());

                OutputText("Scenes.Scylla.Scylla6SLOW.StrippedBareBreasts".Localized());

                OutputText("Scenes.Scylla.Scylla6SLOW.CatNappedCompromising".Localized());

                OutputText("Scenes.Scylla.Scylla6SLOW.CatsJitteryMill".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yell".Localized(), Scylla6YellForHelp);

                AddButton(1, "Common.Button.Wait".Localized(), Scylla6MilkyWait);

                if (Player.Strength >= 85 || Player.HasStatusEffect(StatusEffectIds.KnowsMight) && Player.Strength + 10 * Player.SpellMod() >= 85)
                    AddButton(2, "Common.Button.KickAss".Localized(), LactateAndKickAss);
                else
                    AddButton(2, "Common.Button.KickAss".Localized(), null);
            }
            else
            {
                OutputText("Scenes.Scylla.Scylla6SLOW.AttackersLeftYou".Localized());

                OutputText("Scenes.Scylla.Scylla6SLOW.CatNappedCompromisingAgain".Localized());

                OutputText("Scenes.Scylla.Scylla6SLOW.CatsMillingColossalChest".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.GetHelp".Localized(), Scylla6NoMilkGetHelp);

                if (CoC.Instance.Flags.Get(GameFlagIds.KatherineUnlocked) < 1)
                {
                    if (Player.HasVagina())
                        AddButton(1, "Common.Button.Submit".Localized(), Scylla6NoMilkSubmit);
                    else
                        AddButton(1, "Common.Button.Submit".Localized(), null);

                    AddButton(2, "Common.Button.Rape".Localized(), Scylla6NoMilkRAPETHEWORLD);
                }
                else
                {
                    AddButton(1, "Common.Button.Submit".Localized(), null);

                    AddButton(2, "Common.Button.Rape".Localized(), null);
                }
            }
        }

        public void LactateAndKickAss()
        {
            ClearOutput();

            if (Player.Strength < 85)
                OutputText("Scenes.Scylla.LactateAndKickAss.FlushDrawOnDesires".Localized());
            else
                OutputText("Scenes.Scylla.LactateAndKickAss.CrackNeckEndNow".Localized());

            OutputText("Scenes.Scylla.LactateAndKickAss.UnholyStrengthBreakCollar".Localized());

            OutputText("Scenes.Scylla.LactateAndKickAss.CatsScatterInTerror".Localized());

            ReturnToCamp(1);
        }

        public void Scylla6YellForHelp()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6YellForHelp.FillLungsScreamHelp".Localized());

            OutputText("Scenes.Scylla.Scylla6YellForHelp.GuardStoopsPullGag".Localized());

            OutputText("Scenes.Scylla.Scylla6YellForHelp.PausesMilkFreaks".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Truth".Localized(), Scylla6AfterYellTruth);

            AddButton(1, "Common.Button.Lie".Localized(), Scylla6AfterYellLie);
        }

        public void Scylla6AfterYellTruth()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.Scylla6AfterYellTruth.SpitInDisgust".Localized());

            OutputText("Scenes.Scylla.Scylla6AfterYellTruth.GladlyTailWagging".Localized());

            ReturnToCamp(1);
        }

        public void Scylla6AfterYellLie()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.Scylla6AfterYellLie.PullAsideExplainJoke".Localized());

            OutputText("Scenes.Scylla.Scylla6AfterYellLie.BarksFineCollectsGear".Localized());

            Player.Gems -= 10;

            ReturnToCamp(1);
        }

        public void Scylla6MilkyWait()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6MilkyWait.DontRiskAggravatingJunkies".Localized());

            OutputText("Scenes.Scylla.Scylla6MilkyWait.OthersMadeUpMinds".Localized());

            if (Player.BreastRows.Count == 1)
                OutputText("Scenes.Scylla.Scylla6MilkyWait.LickLipsShakeHead".Localized());
            else
                OutputText("Scenes.Scylla.Scylla6MilkyWait.EndowmentsKeepCatsBusy".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)((20 + Player.Libido / 10 + Player.Sensitivity / 10))));

            Scylla6MilkyWaitII();
        }

        public void Scylla6MilkyWaitII()
        {
            ClearOutput();

            if (Player.LactationQ() < 500)
                OutputText("Scenes.Scylla.Scylla6MilkyWaitII.SuckShudderingOrgasm".Localized());
            else
                if (Player.LactationQ() < 3000)
                    OutputText("Scenes.Scylla.Scylla6MilkyWaitII.TitsCumFirst".Localized());
                else
                {
                    OutputText("Scenes.Scylla.Scylla6MilkyWaitII.BreastsChurnAching".Localized());

                    if (Player.BreastRows.Count == 1)
                        OutputText("Scenes.Scylla.Scylla6MilkyWaitII.ThreeShareOneNipple".Localized());
                    else
                        OutputText("Scenes.Scylla.Scylla6MilkyWaitII.FelinesDrinkJets".Localized());

                    OutputText("Scenes.Scylla.Scylla6MilkyWaitII.MilkPoolingCobblestones".Localized());
                }

            Player.BoostLactation(1);

            OutputText("Scenes.Scylla.Scylla6MilkyWaitII.TunnelVisionRecedes".Localized());

            Player.Orgasm("Tits");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 2f));

            Scylla6MilkyWaitIII();
        }

        public void Scylla6MilkyWaitIII()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6MilkyWaitIII.LastFelineDrunkFill".Localized());

            Player.Gems += 25;

            ReturnToCamp(1);
        }

        public void Scylla6NoMilkGetHelp()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.Scylla6NoMilkGetHelp.FillLungsScreamHelp".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkGetHelp.GuardLoomsGrowling".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkGetHelp.PausesMilkFreaks".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Truth".Localized(), Scylla6NoMilkTruth);

            AddButton(1, "Common.Button.Lie".Localized(), Scylla6NoMilkLie);
        }

        public void Scylla6NoMilkTruth()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.Scylla6NoMilkTruth.SpitInDisgust".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkTruth.GladlyTailWagging".Localized());

            ReturnToCamp(1);
        }

        public void Scylla6NoMilkLie()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.Scylla6NoMilkLie.PullAsideExplainJoke".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkLie.BarksFineCollectsGear".Localized());

            Player.Gems -= 10;

            ReturnToCamp(1);
        }

        public void Scylla6NoMilkSubmit()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmit.AroundCornerReadyHeat".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmit.VainProtestPullArmor".Localized());

            if (Player.BiggestBreastSize() >= 1 && Player.BiggestLactation() < 1)
            {
                OutputText("Scenes.Scylla.Scylla6NoMilkSubmit.FelinesCupTitsNuzzle".Localized());

                Player.BoostLactation(.25f);
            }

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmit.MoistExcitementFemales".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmit.CarryYouToScylla".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmit.SkinSweetTingling".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmit.BeforeSecondGulpPulled".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 75f));

            Scylla6NoMilkSubmitII();
        }

        public void Scylla6NoMilkSubmitII()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitII.BlackCatBoundHands".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitII.PresenceBehindMeatySlap".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitII.PullsBackKnottedCock".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitII.FelinePullsBackReluctant".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitII.EightInchesDogCock".Localized());

            Player.CuntChange(36, true);

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Lust, 80f), new DynStat(GameStats.Corruption, 1f));

            Scylla6NoMilkSubmitIII();
        }

        public void Scylla6NoMilkSubmitIII()
        {
            ClearOutput();

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.MiddleOfSandwichBalls".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSpermSlurps".Localized());
                else
                    if (Player.HasKnot(0))
                        OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSlurpsAgain".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Demon || Player.Cocks[0].Id == BodyPartTypes.Cat)
                            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSlurpsThird".Localized());
                        else
                            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSlurpsFourth".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.MiddleOfSandwichThighs".Localized());

                if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                    OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSlurpsFifth".Localized());
                else
                    if (Player.HasKnot(0))
                        OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSlurpsSixth".Localized());
                    else
                        if (Player.Cocks[0].Id == BodyPartTypes.Demon || Player.Cocks[0].Id == BodyPartTypes.Cat)
                            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSlurpsSeventh".Localized());
                        else
                            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIII.BellyBulgesSlurpsEighth".Localized());
            }

            Scylla6NoMilkSubmitIV();

            Player.Orgasm("Dick");
        }

        public void Scylla6NoMilkSubmitIV()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIV.HadntRealizedFellAsleep".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkSubmitIV.OccursSaccharinMood".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineMetScylla, 1);

            Player.CreateKeyItem("Silver Kitty-Bell", 0, 0, 0, 0);

            Player.Gems += 50;

            ReturnToCamp(1);
        }

        public void Scylla6NoMilkRAPETHEWORLD()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6NoMilkRAPETHEWORLD.DecideStayHidden".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRAPETHEWORLD.SuckableLipsPaleFluid".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRAPETHEWORLD.GirlHermaphroditeSurprise".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRAPETHEWORLD.MoaningProvidesCover".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 75f), new DynStat(GameStats.Corruption, 1f));

            Scylla6NoMilkRapeII();
        }

        public void Scylla6NoMilkRapeII()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeII.CatFreezesBackArched".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeII.SoothesPoorDear".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.Scylla6NoMilkRapeII.NotSureWhosEnjoying".Localized());
            else
                OutputText("Scenes.Scylla.Scylla6NoMilkRapeII.NotSureWhoEnjoys".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeII.MastiffCockedBitesTongue".Localized());

            Scylla6NoMilkRapeIII();
        }

        public void Scylla6NoMilkRapeIII()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeIII.SavagePumpKittyKnot".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeIII.VergeCummingPullOut".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeIII.BlackCatYelpSpunk".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Scylla.Scylla6NoMilkRapeIII.SackEmptiesHardnessSlacken".Localized());
            else
                OutputText("Scenes.Scylla.Scylla6NoMilkRapeIII.BodyEmptiesHardnessSlacken".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -1f));

            Scylla6NoMilkRapeIV();
        }

        public void Scylla6NoMilkRapeIV()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeIV.PullDownCutRestraints".Localized());

            OutputText("Scenes.Scylla.Scylla6NoMilkRapeIV.GirlApologizesHardTell".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KatherineMetScylla, 1);

            Player.CreateKeyItem("Silver Kitty-Bell", 0, 0, 0, 0);

            ReturnToCamp(1);
        }

        public void ShareTentaclesWithScylla()
        {
            ClearOutput();

            ShowImage("scylla-share-tentacle-addiction-one");

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla.GirlsPaleTentaclesSnake".Localized());

            if (silly)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla.ThrowHandsLasciviousGrin".Localized());
            else
                if (Player.Corruption < 33)
                    OutputText("Scenes.Scylla.ShareTentaclesWithScylla.AskGirlsForHelp".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Scylla.ShareTentaclesWithScylla.MildlyInviteJoin".Localized());
                    else
                        OutputText("Scenes.Scylla.ShareTentaclesWithScylla.InvitePrepareThemselves".Localized());

            if (Player.BiggestCockLength() < 60)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla.MeetsGazeSapphireEyes".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ShareTentaclesWithScylla2);
        }

        public void ShareTentaclesWithScylla2()
        {
            ClearOutput();

            ShowImage("scylla-share-tentacle-addiction-two");

            if (Player.CountCocksOfType("TENTACLE") <= 2)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.ShakesOutLuridTrance".Localized());
            else
                if (Player.CountCocksOfType("TENTACLE") <= 4)
                    OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.NunGoblinMoveCloser".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.DeepBreathHandsHips".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 4)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.TwoMoreFindAbylon".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 6)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.AnotherEncirclesPastie".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.ShriekTentaclesTightenThighs".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 4)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.WrapsTentacleNeck".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 6)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.FaerieWrithesCocooned".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.SlippingTowardCleavage".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.FallsKneesBlissfulCry".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 4)
            {
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.AbylonCursesKneesShaking".Localized());

                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.VoiceBoxGaspsInsults".Localized());
            }

            if (Player.CountCocksOfType("TENTACLE") >= 6)
            {
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.DrugLikeCumFight".Localized());

                OutputText("Scenes.Scylla.ShareTentaclesWithScylla2.ShiftCockWrappingPastie".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Lust, 100f), new DynStat(GameStats.Corruption, .25f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ShareTentaclesWithScylla3);
        }

        public void ShareTentaclesWithScylla3()
        {
            ClearOutput();

            ShowImage("scylla-share-tentacle-addiction-three");

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.MembersInPlaceDrink".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 4)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.AbylonHipsRollStruggles".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 6)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.PastieWrigglingTwinSheathes".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.WrithingHostIgnited".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.NunTightlyBoundHeaves".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 4)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.MovingAsOnePiston".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 6)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.TentaclesRippleWavelike".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.ConsumingThrillsFeedLust".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.EggLikeBulgesSlow".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 4)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.HeavyBouncingStretchedPussy".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 6)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla3.CockSwallowingPastiePulses".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, .5f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), ShareTentaclesWithScylla4);
        }

        public void ShareTentaclesWithScylla4()
        {
            ClearOutput();

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla4.OrgasmWanesCatchBreath".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 4)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla4.LowerGoblinIntoCleavage".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 6)
                OutputText("Scenes.Scylla.ShareTentaclesWithScylla4.ScoopPastieOntoTable".Localized());

            OutputText("Scenes.Scylla.ShareTentaclesWithScylla4.TakeLeaveRestlessHeat".Localized());

            if (Player.CumQ() < 28000)
                Player.CumMultiplier += 2;

            ReturnToCamp(1);
        }

        public void ScyllasFlyingSolo()
        {
            ClearOutput();

            ScyllaSprite();

            ShowImage("scylla-feed-bar-intro");

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSoloFedNun) == 0)
            {
                OutputText("Scenes.Scylla.ScyllasFlyingSolo.SpyStatuesqueNunAlone".Localized());

                OutputText("Scenes.Scylla.ScyllasFlyingSolo.ForgiveMeDistracted".Localized());

                OutputText("Scenes.Scylla.ScyllasFlyingSolo.HungryButInControl".Localized());

                OutputText("Scenes.Scylla.ScyllasFlyingSolo.InquireHowMuchNeeds".Localized());

                OutputText("Scenes.Scylla.ScyllasFlyingSolo.ConversationStirsThirst".Localized());
            }
            else
                OutputText("Scenes.Scylla.ScyllasFlyingSolo.WalkingTableToTable".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Feed".Localized(), FeedScyllaSomeJizzDatJunkieNeedsIt);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void FeedScyllaSomeJizzDatJunkieNeedsIt()
        {
            ClearOutput();

            ShowImage("scylla-feed-jizz-solo-one");

            ScyllaSprite();

            CoC.Instance.Flags.Set(GameFlagIds.TimesSoloFedNun, CoC.Instance.Flags.Get(GameFlagIds.TimesSoloFedNun) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.FedScyllaToday, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSoloFedNun) == 1)
            {
                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.CantJustLeaveHer".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.SeethingLoinsEmptyBalls".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.BestPrivateSooner".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.ScrotumInflatesArmorUnbearable".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.HastilyStripGuideBed".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.OrbsSwingingContinueGrowth".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.StandingLiftCock".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.ChurningBurdenOffBalance".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.BeforeRealizeGushes".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.SuggestRetireToRoom".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.LoinsEmptyBallsAgain".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.BreakKissStrokeCheek".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.ScrotumInflatesDelicious".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.HelpsStripGuidesBed".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.OrbsSwingingEveryMotion".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.StandingLiftAgain".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.BurdenOffBalanceAgain".Localized());

                OutputText("Scenes.Scylla.FeedScyllaSomeJizzDatJunkieNeedsIt.ThickLumpPreCum".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), FeedingScyllaCumStepTwo);
        }

        public void FeedingScyllaCumStepTwo()
        {
            ClearOutput();

            ShowImage("scylla-feed-jizz-solo-two");

            ScyllaSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSoloFedNun) == 1)
            {
                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.PunchStomachVirileMagic".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.CorpulentSuffocating".Localized());

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.AvoidWasteSlidingDeeper".Localized());

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.DilatedSwellingForcedDown".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.SpooLacqueredFaceTrembling".Localized());

                    if (Player.Balls == 2)
                        OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.TwinWord".Localized());
                    else
                        if (Player.Balls == 4)
                            OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.FourfoldWord".Localized());
                        else
                            OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.ManyWord".Localized());

                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.DeitiesHungFromLoins".Localized());
                }

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.RapidShallowThrustsHeat".Localized());

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.DelugeSemenGlobularTorrents".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.DischargingPressurizedBounty".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.BolsteringSurgeVirileMagic".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.CorpulentTremendous".Localized());

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.NeedingNoCoaxingDeeper".Localized());

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.SwellingForcedDownAgain".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.SpooLacqueredFaceAgain".Localized());

                    if (Player.Balls == 2)
                        OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.TwinWordAgain".Localized());
                    else
                        if (Player.Balls == 4)
                            OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.FourfoldWordAgain".Localized());

                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.DeitiesHungLoinsAgain".Localized());
                }

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.ShallowThrustsHeatAgain".Localized());

                OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.SemenGlobularTorrentsAgain".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Scylla.FeedingScyllaCumStepTwo.PressurizedBountyAgain".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -1f));

            AddButton(0, "Common.Button.Next".Localized(), CumFeedScyllaShesACoolGirl);
        }

        public void CumFeedScyllaShesACoolGirl()
        {
            ClearOutput();

            Player.ChangeFatigue(10);

            ScyllaSprite();

            ShowImage("scylla-feed-jizz-solo-three");

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesSoloFedNun) == 1)
            {
                if (Player.CumQ() <= 50000)
                {
                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.ReservoirDrySeparate".Localized());

                    Player.CumMultiplier += 1 + Random.Shared.Next(5);

                    if (Player.Balls > 0)
                        Player.BallSize += 1;

                    ReturnToCamp(1);
                }
                else
                {
                    if (Player.Balls > 0)
                        Player.BallSize += 1;

                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.SeethingIssueGeyser".Localized());

                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.AmazedRecoveredWits".Localized());

                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.ReservoirDryLastCondom".Localized());

                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.OfferPersonalPurveyor".Localized());

                    Player.CumMultiplier += 1 + Random.Shared.Next(5);

                    if (Player.Balls > 0)
                        Player.BallSize += 1;

                    ClearButtons();

                    AddButton(0, "Common.Button.Offer".Localized(), BeScyllasPersonSemenSquirtingMilkMaid);

                    AddButton(14, "Common.Button.Leave".Localized(), DeclineToBeASpunkPumpintJizztrocity);
                }
            }
            else
                if (Player.CumQ() <= 50000)
                {
                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.ReservoirDrySeparateAgain".Localized());

                    Player.CumMultiplier += 1 + Random.Shared.Next(5);

                    if (Player.Balls > 0)
                        Player.BallSize += 1;

                    ReturnToCamp(1);
                }
                else
                {
                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.SeethingIssueGeyserAgain".Localized());

                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.BodyPinnedByGirth".Localized());

                    OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.ReservoirDryCondomAgain".Localized());

                    if (!Player.HasKeyItem("Opal Ring"))
                    {
                        OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.ThoughExhaustingCouldProvide".Localized());

                        OutputText("Scenes.Scylla.CumFeedScyllaShesACoolGirl.OfferPersonalPurveyorAgain".Localized());

                        Player.CumMultiplier += 1 + Random.Shared.Next(5);

                        if (Player.Balls > 0)
                            Player.BallSize += 1;

                        ClearButtons();

                        AddButton(0, "Common.Button.Offer".Localized(), BeScyllasPersonSemenSquirtingMilkMaid);

                        AddButton(14, "Common.Button.Leave".Localized(), DeclineToBeASpunkPumpintJizztrocity);
                    }
                    else
                    {
                        Player.CumMultiplier += 1 + Random.Shared.Next(5);

                        if (Player.Balls > 0)
                            Player.BallSize += 1;

                        ReturnToCamp(1);
                    }
                }
        }

        public void DeclineToBeASpunkPumpintJizztrocity()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.DeclineToBeASpunkPumpintJizztrocity.RetrospectThirstWillGrow".Localized());

            ReturnToCamp(1);
        }

        public void BeScyllasPersonSemenSquirtingMilkMaid()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.BeScyllasPersonSemenSquirtingMilkMaid.ExplainPropositionSatedNun".Localized());

            OutputText("Scenes.Scylla.BeScyllasPersonSemenSquirtingMilkMaid.SmallNodAgrees".Localized());

            OutputText("Scenes.Scylla.BeScyllasPersonSemenSquirtingMilkMaid.RingInHandCamp".Localized());

            Player.CreateKeyItem("Opal Ring", 0, 0, 0, 0);

            OutputText("Scenes.Scylla.BeScyllasPersonSemenSquirtingMilkMaid.GainedOpalRing".Localized());

            ReturnToCamp(1);
        }

        public void OpenTheDoorToFoursomeWivScyllaAndFurries()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.DoorCreaksSlipInside".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.FormLeftMouseGirl".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.OnRightImposingWoman".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.FlutteringEyesSpotYou".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.ShedArmorHalfwayApology".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.CompanionsReactShoulder".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaFurryFoursomeCount) == 0)
            {
                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.GreetsBetweenGrunts".Localized());

                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.NodStepCloserTongue".Localized());

                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SnowCoosSwattingTail".Localized());

                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.HumsEyesClosed".Localized());
            }
            else
            {
                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.CockyRodentHelloAgain".Localized());

                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.StepCloserAllowCaress".Localized());

                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.AlbinoMouseTwinkle".Localized());

                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.NodThrustFirstInches".Localized());
            }

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SnowBeamsCirclingAsshole".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.WinterWhimpersTalk".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.MouseSnickersLookDown".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.HaplessDogWomanTrembling".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.HeldOnEdgeGrow".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SnowGigglesPoundingRod".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaFurryFoursomeCount) == 0)
                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.BustsNutsLikeChamp".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaFurryFoursomeCount) <= 1)
                    OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.FillsCondomsBalloonMachine".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaFurryFoursomeCount) <= 2)
                        OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.BustsCondomsEveryOther".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.ScyllaFurryFoursomeCount) <= 3)
                            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.BustsCondomsThinks".Localized());
                        else
                            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.CanFillBathtubNowadays".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SayEnd".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.FeelStartedCummingVigorously".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.PearlCheeksColorRed".Localized());

            if (Player.BiggestCockLength() <= 12)
                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.AFootWord".Localized());
            else
                if (Player.BiggestCockLength() < 24)
                    OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.OverAFootWord".Localized());
                else
                    OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.AFewFeetWord".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.OfCockChokingThroat".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SnowMischievousDrawsMost".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.NoOutwardSignsHeard".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.ProtestHeldFastPriestess".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SnowPushesHeadHalfway".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.AbruptlySquealsRamsHard".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.EyesOpenScleraDarken".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SnowGaspsArchingPlunge".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SmallStaturedTrue".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.NotSureBlackMagic".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.NowhereNearAsStrange".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.DickGoesWord".Localized());
                else
                    OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.DicksGoWord".Localized());

                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.LimpUnableWasteDrop".Localized());
            }

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.TailSlipsProstate".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SomethingSoftBrushesLeg".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.WinterPleadsLetCum".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.ShakesHeadLosingLuster".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SnowFirstToWhimper".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.NotFarBehind".Localized());

            if (Player.Libido >= 80 || Player.MinLibido > 50)
                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SlappingBellyStillHard".Localized());
            else
                OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.StartingToGoLimp".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.FeelsLikeCummingHours".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.StillStandingWinterReleased".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.WhereLayingLipsSealed".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.CriesEachTitExpanding".Localized());

            OutputText("Scenes.Scylla.OpenTheDoorToFoursomeWivScyllaAndFurries.SublimeReliefWinterFace".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -1f));

            ScyllaFurryFoursomePartDues();
        }

        public void ScyllaFurryFoursomePartDues()
        {
            ClearOutput();

            ScyllaSprite();

            OutputText("Scenes.Scylla.ScyllaFurryFoursomePartDues.StrongHandSlapsChest".Localized());

            OutputText("Scenes.Scylla.ScyllaFurryFoursomePartDues.WinterCommandsWakeUp".Localized());

            OutputText("Scenes.Scylla.ScyllaFurryFoursomePartDues.NodStaggerDonArmor".Localized());

            OutputText("Scenes.Scylla.ScyllaFurryFoursomePartDues.AcceptApologyThankWarning".Localized());

            OutputText("Scenes.Scylla.ScyllaFurryFoursomePartDues.DoorClicksGroinTingling".Localized());

            if (Player.CumMultiplier < 19990)
                Player.CumMultiplier += 1 + Random.Shared.Next(5);

            if (Player.Balls > 0 && Player.BallSize < 30)
                Player.BallSize += 1;

            CoC.Instance.Flags.Set(GameFlagIds.ScyllaFurryFoursomeCount, CoC.Instance.Flags.Get(GameFlagIds.ScyllaFurryFoursomeCount) + 1);

            ReturnToCamp(1);
        }

    }
}