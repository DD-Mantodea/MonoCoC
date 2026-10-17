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
    [Register<IfrisScene>("coc:ifris")]
    public class IfrisScene : Scene
    {

        public override void Entry()
        {
            IfrisIntro();
        }

        public bool IfrisIntro()
        {

            if (View.Time.Hours >= 15)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.MetIfris) > 0)
                    OutputText("Scenes.Ifris.IfrisIntro.StandingCorner".Localized());
                else
                    OutputText("Scenes.Ifris.IfrisIntro.InCornerRoom".Localized());

                return true;
            }

            return false;
        }

        public void ApproachIfris()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MetIfris) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.MetIfris, 1);

                OutputText("Scenes.Ifris.ApproachIfris.AppraisingGaze".Localized());

                OutputText("Scenes.Ifris.ApproachIfris.TiltsHead".Localized());

                OutputText("Scenes.Ifris.ApproachIfris.GivesName".Localized());

                OutputText("Scenes.Ifris.ApproachIfris.GigglesCutely".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedIfrisBlowjob) + CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedIfrisLicked) == 0)
                {
                    OutputText("Scenes.Ifris.ApproachIfris.GrinsWalksOver".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.IfrisShowedOff) == 0)
                        OutputText("Scenes.Ifris.ApproachIfris.HelloDarling".Localized());
                    else
                        OutputText("Scenes.Ifris.ApproachIfris.HelloDarlingAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Ifris.ApproachIfris.GrinsWalksOverAgain".Localized());

                    OutputText("Scenes.Ifris.ApproachIfris.HelloDarlingThird".Localized());

                    OutputText("Scenes.Ifris.ApproachIfris.BitConfused".Localized());

                    OutputText("Scenes.Ifris.ApproachIfris.HowLongQuestion".Localized());
                }

            OutputText("Scenes.Ifris.ApproachIfris.CouldGoAhead".Localized());

            ClearButtons();

            if (Player.FatigueLeft() >= 30)
            {
                AddButton(0, "Common.Button.WorkOut".Localized(), WorkOutForIfris);

                AddButton(1, "Common.Button.JoinMe".Localized(), AskIfrisToJoinYou);
            }
            else
            {
                AddButton(0, "Common.Button.WorkOut".Localized(), null);

                AddButton(1, "Common.Button.JoinMe".Localized(), null);
            }

            AddButton(14, "Common.Button.Leave".Localized(), SceneInstance<GymScene>.Get().GymDesc);
        }

        public void WorkOutForIfris()
        {
            ClearOutput();

            OutputText("Scenes.Ifris.WorkOutForIfris.SmilesDevil".Localized());

            OutputText("Scenes.Ifris.WorkOutForIfris.GesturesBench".Localized());

            OutputText("Scenes.Ifris.WorkOutForIfris.HopeYouDont".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.WorkOut".Localized(), LiftWhileIfrisWatches);

            AddButton(1, "Common.Button.ShowOff".Localized(), ShowOffForIfris);

            if (Player.Strength > 90)
                AddNextButton("Common.Button.GoAllOut".Localized(), IfrisAllOut);
        }

        public void AskIfrisToJoinYou()
        {
            ClearOutput();

            OutputText("Scenes.Ifris.AskIfrisToJoinYou.AsksIfris".Localized());

            OutputText("Scenes.Ifris.AskIfrisToJoinYou.ThatsSweetOf".Localized());

            OutputText("Scenes.Ifris.AskIfrisToJoinYou.ShrugsResponse".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.WorkOut".Localized(), LiftWhileIfrisWatches);

            AddButton(1, "Common.Button.ShowOff".Localized(), ShowOffForIfris);

            if (Player.Strength > 90)
                AddNextButton("Common.Button.GoAllOut".Localized(), IfrisAllOut);
        }

        public void LiftWhileIfrisWatches()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IfrisShowedOff, CoC.Instance.Flags.Get(GameFlagIds.IfrisShowedOff) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
            {
                OutputText("Scenes.Ifris.LiftWhileIfrisWatches.CentauressWalks".Localized());

                Player.Gems -= 10;

            }

            OutputText("Scenes.Ifris.LiftWhileIfrisWatches.SetsMachine".Localized());

            OutputText("Scenes.Ifris.LiftWhileIfrisWatches.FinishSets".Localized());

            OutputText("Scenes.Ifris.LiftWhileIfrisWatches.GrinsExposing".Localized());

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                Player.ApplyStats(new DynStat(GameStats.Strength, .5f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 40)
                Player.ApplyStats(new DynStat(GameStats.Toughness, .3f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            Player.ChangeFatigue(30);

            ReturnToCamp(1);
        }

        public void ShowOffForIfris()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.IfrisShowedOff, CoC.Instance.Flags.Get(GameFlagIds.IfrisShowedOff) + 1);

            Player.ChangeFatigue(30);

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
            {
                OutputText("Scenes.Ifris.ShowOffForIfris.CentauressWalks".Localized());

                Player.Gems -= 10;

            }

            OutputText("Scenes.Ifris.ShowOffForIfris.LooksSettings".Localized());

            if (Player.Strength < 40)
            {
                OutputText("Scenes.Ifris.ShowOffForIfris.GrippingBars".Localized());

                OutputText("Scenes.Ifris.ShowOffForIfris.RollsEyes".Localized());

                OutputText("Scenes.Ifris.ShowOffForIfris.NoTimeFor".Localized());

                OutputText("Scenes.Ifris.ShowOffForIfris.HumiliatedLeaves".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IfrisShowedOff, 0);

                Player.HealthChange(-10);

                ReturnToCamp(1);

                return ;
            }

            if (Player.Strength < 75)
            {
                OutputText("Scenes.Ifris.ShowOffForIfris.GrippingBarsAgain".Localized());

                OutputText("Scenes.Ifris.ShowOffForIfris.SighsCrestfallen".Localized());

                OutputText("Scenes.Ifris.ShowOffForIfris.AtLeastHad".Localized());

                ReturnToCamp(1);

                CoC.Instance.Flags.Set(GameFlagIds.IfrisShowedOff, 0);

                return ;
            }

            if (Player.IsGenderless() || (Player.Femininity < 40 && !Player.IsPureEnough(75) && Player.Body.Get<FacePart>().Id == BodyPartTypes.CowMinotaur))
            {
                OutputText("Scenes.Ifris.ShowOffForIfris.WatchesMoment".Localized());

                if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                    Player.ApplyStats(new DynStat(GameStats.Strength, .5f));

                if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 40)
                    Player.ApplyStats(new DynStat(GameStats.Toughness, .3f));

                ReturnToCamp(1);

                return ;
            }

            if (Player.HasCock())
            {
                if (Player.CockTotal() > 1 && Random.Shared.Next(2) == 0 && Player.CockThatFits(70) != -1 && Player.CockThatFits2(70) != -1)
                {
                    IfrisDP();

                    return ;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedIfrisBlowjob) == 0)
                {
                    OutputText("Scenes.Ifris.ShowOffForIfris.GrippingBarsThird".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.MovesCloser".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.AllButPurrs".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.SpurredEgo".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.TeasesHusky".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.GroansMinistrations".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.PleasedEffect".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.SuddenlyLoses".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Ifris.ShowOffForIfris.SqueezesSack".Localized());
                    else
                        if (Player.HasVagina())
                            OutputText("Scenes.Ifris.ShowOffForIfris.SqueezesLowerLips".Localized());
                        else
                            OutputText("Scenes.Ifris.ShowOffForIfris.SqueezesTaint".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.CanTakeNoMore".Localized());
                }
                else
                {
                    OutputText("Scenes.Ifris.ShowOffForIfris.AllButSqueals".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.LiftsWeight".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Ifris.ShowOffForIfris.HandCupping".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.SmilingWatches".Localized(
                        Player.CockHead()
                    ));

                    OutputText("Scenes.Ifris.ShowOffForIfris.SexyDevil".Localized());

                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Ifris.ShowOffForIfris.SheathWord".Localized());
                    else
                        OutputText("Scenes.Ifris.ShowOffForIfris.BaseWord".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.InThroatAgain".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.StrengthSpent".Localized());

                    OutputText("Scenes.Ifris.ShowOffForIfris.SlowPainfullyHard".Localized(
                        Player.CockHead()
                    ));
                }

                CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedIfrisBlowjob, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedIfrisBlowjob) + 1);
            }
            else
                if (Player.HasVagina())
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedIfrisLicked) == 0)
                    {
                        OutputText("Scenes.Ifris.ShowOffForIfris.GrippingBarsFourth".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.MovesCloserAgain".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.AllButPurrsAgain".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.SpurredEgoThis".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.LetsWeightsRest".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.WarmthGrows".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.BackArches".Localized());

                        if (Player.HasTailInsteadOfLegs())
                            OutputText("Scenes.Ifris.ShowOffForIfris.TailFlexing".Localized());
                        else
                            OutputText("Scenes.Ifris.ShowOffForIfris.ToesCurling".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.MusclesContract".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Ifris.ShowOffForIfris.AllButSquealsAgain".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.LeansInBefore".Localized());

                        if (Player.BiggestBreastSize() >= 1)
                            OutputText("Scenes.Ifris.ShowOffForIfris.BreastsWord".Localized());
                        else
                            OutputText("Scenes.Ifris.ShowOffForIfris.ChestWord".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.SoftKissesNubs".Localized(
                            Player.Legs()
                        ));

                        OutputText("Scenes.Ifris.ShowOffForIfris.FirstPush".Localized(
                            Player.VaginaDescript()
                        ));

                        OutputText("Scenes.Ifris.ShowOffForIfris.TongueJoins".Localized());

                        OutputText("Scenes.Ifris.ShowOffForIfris.LeavingOnce".Localized());
                    }

                    CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedIfrisLicked, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedIfrisLicked) + 1);
                }

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                Player.ApplyStats(new DynStat(GameStats.Strength, .5f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 40)
                Player.ApplyStats(new DynStat(GameStats.Toughness, .3f));

            Player.Orgasm("Generic");

            ReturnToCamp(1);
        }

        public void IfrisDP()
        {

            OutputText("Scenes.Ifris.IfrisDP.SetsMachine".Localized());

            OutputText("Scenes.Ifris.IfrisDP.FirstSetFinished".Localized(
                Player.ArmorName
            ));

            OutputText("Scenes.Ifris.IfrisDP.SpurredCrimson".Localized());

            OutputText("Scenes.Ifris.IfrisDP.SultryPromises".Localized());

            OutputText("Scenes.Ifris.IfrisDP.FiveBigPushes".Localized());

            OutputText("Scenes.Ifris.IfrisDP.GaspsPleasant".Localized());

            OutputText("Scenes.Ifris.IfrisDP.Tortuous".Localized());

            OutputText("Scenes.Ifris.IfrisDP.LeansTwines".Localized());

            OutputText("Scenes.Ifris.IfrisDP.SetsBar".Localized());

            if (Player.CountKnottedCocks() >= 2)
            {
                OutputText("Scenes.Ifris.IfrisDP.CocksBloat".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Ifris.IfrisDP.GroansMoans".Localized());

                OutputText("Scenes.Ifris.IfrisDP.MassiveRelease".Localized());
            }
            else
                if (Player.CountCocksOfType("DEMON") >= 2)
                {
                    OutputText("Scenes.Ifris.IfrisDP.DemonicExplode".Localized());

                    if (Player.CumQ() >= 1000)
                        OutputText("Scenes.Ifris.IfrisDP.GroansMoansAgain".Localized());

                    OutputText("Scenes.Ifris.IfrisDP.AtSameTimeHands".Localized());
                }
                else
                    if (Player.CountCocksOfType("Horse") >= 2)
                    {
                        OutputText("Scenes.Ifris.IfrisDP.EquineExplode".Localized());

                        if (Player.CumQ() >= 1000)
                            OutputText("Scenes.Ifris.IfrisDP.GroansMoansThird".Localized());

                        OutputText("Scenes.Ifris.IfrisDP.HipsRock".Localized());
                    }
                    else
                        if (Player.CountCocksOfType("TENTACLE") >= 2)
                        {
                            OutputText("Scenes.Ifris.IfrisDP.PlantExplode".Localized());

                            if (Player.CumQ() >= 1000)
                                OutputText("Scenes.Ifris.IfrisDP.GroansMoansFourth".Localized());

                            OutputText("Scenes.Ifris.IfrisDP.AtSameTimeTentacle".Localized());
                        }
                        else
                            if (Player.CountCocksOfType("ANEMONE") >= 2)
                            {
                                OutputText("Scenes.Ifris.IfrisDP.AquaticExplode".Localized());

                                if (Player.CumQ() >= 1000)
                                    OutputText("Scenes.Ifris.IfrisDP.GroansMoansFifth".Localized());

                                OutputText("Scenes.Ifris.IfrisDP.AtSameTimeStinging".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Ifris.IfrisDP.CocksExplodeInside".Localized());

                                if (Player.CockTotal() > 2)
                                {
                                    OutputText("Scenes.Ifris.IfrisDP.ThePrefix".Localized());

                                    if (Player.CockTotal() == 1)
                                        OutputText("Scenes.Ifris.IfrisDP.OneUnboundPrick".Localized());
                                    else
                                        OutputText("Scenes.Ifris.IfrisDP.OtherUnboundPricks".Localized());
                                }
                            }

            OutputText("Scenes.Ifris.IfrisDP.PurrsEar".Localized());

            OutputText("Scenes.Ifris.IfrisDP.LastContractions".Localized());

            if (Player.DogCocks() >= 2)
                OutputText("Scenes.Ifris.IfrisDP.NoisilyPopFree".Localized());
            else
                OutputText("Scenes.Ifris.IfrisDP.SlideWord".Localized());

            OutputText("Scenes.Ifris.IfrisDP.SpermGlazed".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Ifris.IfrisDP.BodyBloated".Localized());
            else
                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Ifris.IfrisDP.BellyBloated".Localized());
                else
                    OutputText("Scenes.Ifris.IfrisDP.DoubleStuffed".Localized());

            OutputText("Scenes.Ifris.IfrisDP.SlumpsBack".Localized());

            OutputText("Scenes.Ifris.IfrisDP.SomeTimeUntil".Localized());

            Player.ChangeFatigue(10);

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                Player.ApplyStats(new DynStat(GameStats.Strength, .75f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 40)
                Player.ApplyStats(new DynStat(GameStats.Toughness, .5f));

            Player.Orgasm("Generic");

            ReturnToCamp(1);
        }

        public void IfrisAllOut()
        {
            ClearOutput();

            OutputText("Scenes.Ifris.IfrisAllOut.ExplainsInFor".Localized());

            OutputText("Scenes.Ifris.IfrisAllOut.WithEaseLifts".Localized());

            OutputText("Scenes.Ifris.IfrisAllOut.SaysSurprise".Localized());

            OutputText("Scenes.Ifris.IfrisAllOut.PushesBarUp".Localized());

            OutputText("Scenes.Ifris.IfrisAllOut.SightTonedBody".Localized());

            if (Player.HasCock())
            {
                ClearButtons();

                AddNextButton("Common.Button.Sex".Localized(), IfrisShowerSex);

                AddNextButton("Common.Button.Nah".Localized(), IfrisThot);
            }
            else
                IfrisThot();
        }

        public void IfrisShowerSex()
        {
            ClearOutput();

            OutputText("Scenes.Ifris.IfrisShowerSex.PushesBarOnto".Localized());

            OutputText("Scenes.Ifris.IfrisShowerSex.DevilHopsOff".Localized());

            OutputText("Scenes.Ifris.IfrisShowerSex.BringsIntoShower".Localized());

            OutputText("Scenes.Ifris.IfrisShowerSex.SlipsBetweenThighs".Localized());

            OutputText("Scenes.Ifris.IfrisShowerSex.PressingStall".Localized(
                Player.BiggestCockLength() < 5 ? "[say: I guess with a few hundred " + (metric ? "kilos" : "pounds") + " of force behind it, even a thumbtack is gonna do some damage,] she teases, grinning" : "She grunts and curls her toes as she accepts the forceful intrusion"
            ));

            OutputText("Scenes.Ifris.IfrisShowerSex.PumpsFaster".Localized());

            OutputText("Scenes.Ifris.IfrisShowerSex.ReAdjusting".Localized());

            OutputText("Scenes.Ifris.IfrisShowerSex.WindsDown".Localized());

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

        public void IfrisThot()
        {
            ClearOutput();

            OutputText("Scenes.Ifris.IfrisThot.SettingBar".Localized());

            OutputText("Scenes.Ifris.IfrisThot.WantsHome".Localized());

            ReturnToCamp(1);
        }

    }
}