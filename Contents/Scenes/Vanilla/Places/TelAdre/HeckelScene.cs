using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<HeckelScene>("coc:heckel")]
    public class HeckelScene : Scene
    {

        public override void Entry()
        {
            HeckelAppearance();
        }

        public void HeckelAppearance()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MetHeckel) == 0)
                OutputText("Scenes.Heckel.HeckelAppearance.BrownHyena".Localized());
            else
                OutputText("Scenes.Heckel.HeckelAppearance.RunningLaps".Localized());
        }

        public void GreetHeckel()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.MetHeckel) == 0)
            {
                OutputText("Scenes.Heckel.GreetHeckel.ApproachesTrack".Localized());

                OutputText("Scenes.Heckel.GreetHeckel.DrawsUp".Localized());

                OutputText("Scenes.Heckel.GreetHeckel.WhatCanIDo".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MetHeckel, CoC.Instance.Flags.Get(GameFlagIds.MetHeckel) + 1);
            }
            else
                if (SceneInstance<BrookeScene>.Get().BrookeAffection() >= 70 && CoC.Instance.Flags.Get(GameFlagIds.BrookeMediumScene) > 0 && CoC.Instance.Flags.Get(GameFlagIds.MetHeckel) > 0 && View.Time.Hours >= 13 && View.Time.Hours <= 15 && (Player.HasVagina() || Player.HasCockThatFits(SceneInstance<BrookeScene>.Get().BrookeCapacity())))
                {
                    SceneInstance<BrookeScene>.Get().SpecialHeckelAndBrookeEncounter();

                    return ;
                }
                else
                    OutputText("Scenes.Heckel.GreetHeckel.CheckingPulse".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Training".Localized(), HeckelTraining);

            AddButton(14, "Common.Button.Leave".Localized(), SceneInstance<GymScene>.Get().GymDesc);
        }

        public void HeckelTraining()
        {
            ClearOutput();

            if (Player.Tone < 60)
            {
                OutputText("Scenes.Heckel.HeckelTraining.TrainingPartner".Localized());

                OutputText("Scenes.Heckel.HeckelTraining.SheStarts".Localized());

                SceneInstance<GymScene>.Get().GymDesc();

                return ;
            }
            else
                if (Player.Gender == Gender.Genderless || Player.IsTaur())
                {
                    OutputText("Scenes.Heckel.HeckelTraining.TrainingPartnerAgain".Localized());

                    OutputText("Scenes.Heckel.HeckelTraining.AllShapes".Localized());

                    SceneInstance<GymScene>.Get().GymDesc();

                    return ;
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelBlowjob) + CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelAnal) == 0)
            {
                OutputText("Scenes.Heckel.HeckelTraining.TrainingPartnerThird".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                {
                    OutputText("Scenes.Heckel.HeckelTraining.CollectsFee".Localized());

                    Player.Gems -= 10;

                }

                OutputText("Scenes.Heckel.HeckelTraining.ChangingRooms".Localized());

                OutputText("Scenes.Heckel.HeckelTraining.WandersAisles".Localized());

                OutputText("Scenes.Heckel.HeckelTraining.SitsOnBench".Localized());

                OutputText("Scenes.Heckel.HeckelTraining.HalfErect".Localized());

                OutputText("Scenes.Heckel.HeckelTraining.LikeWhatYouSee".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), HeckelOrallyFixated);

                if (Player.Strength > 50 && Player.HasCock())
                {
                    OutputText("Scenes.Heckel.HeckelTraining.FeelsStrong".Localized());

                    AddButton(1, "Common.Button.BeTopDog".Localized(), DominateHeckel);
                }
                else
                    AddButton(1, "Common.Button.BeTopDog".Localized(), null);

                SetExitButton("Common.Button.Back".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                {
                    OutputText("Scenes.Heckel.HeckelTraining.CollectsFeeAgain".Localized());

                    Player.Gems -= 10;

                }

                OutputText("Scenes.Heckel.HeckelTraining.KnowsDrill".Localized());

                OutputText("Scenes.Heckel.HeckelTraining.SheComesBack".Localized());

                OutputText("Scenes.Heckel.HeckelTraining.MinceWords".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Faerie.SexMenu.ButtonOral".Localized(), HeckelOrallyFixated);

                AddButton(1, "Common.Button.Anal".Localized(), () => HeckelLovesDaAnal());

                if (Player.Strength > 50 && Player.HasCock())
                {
                    OutputText("Scenes.Heckel.HeckelTraining.FeelsStrongAgain".Localized());

                    AddButton(2, "Common.Button.BeTopDog".Localized(), DominateHeckel);
                }
                else
                    AddButton(2, "Common.Button.BeTopDog".Localized(), null);

                SetExitButton("Common.Button.Back".Localized());
            }
        }

        public void HeckelOrallyFixated()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelBlowjob) == 0)
            {
                OutputText("Scenes.Heckel.HeckelOrallyFixated.FallsToKnees".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.FeelsBloodFlow".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.GaspsFeminine".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.HitsThroat".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.AnotherInch".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.GrazeKnot".Localized());

                HeckelOralFixationII();
            }
            else
            {
                OutputText("Scenes.Heckel.HeckelOrallyFixated.LicksLips".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.MoreRoom".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.TakesMouthful".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.SeesHerFace".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.IfIToldYou".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.GruntsAbove".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Heckel.HeckelOrallyFixated.FondlesBalls".Localized(
                        Player.CockDescript(0)
                    ));
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Heckel.HeckelOrallyFixated.FondlesBallsAgain".Localized(
                            Player.VaginaDescript(0)
                        ));

                OutputText("Scenes.Heckel.HeckelOrallyFixated.KnotGrows".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.GrowingAlarm".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.KnotLarge".Localized());

                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Heckel.HeckelOrallyFixated.GrowlRises".Localized());
                else
                    OutputText("Scenes.Heckel.HeckelOrallyFixated.GrowlRisesAgain".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.PullsFromThroat".Localized());

                OutputText("Scenes.Heckel.HeckelOrallyFixated.QuiteTheMouth".Localized());

                Player.Orgasm("Lips");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

                ReturnToCamp(1);
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedHeckelBlowjob, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelBlowjob) + 1);
        }

        public void HeckelOralFixationII()
        {
            ClearOutput();

            OutputText("Scenes.Heckel.HeckelOralFixationII.ScowlingFace".Localized());

            OutputText("Scenes.Heckel.HeckelOralFixationII.FocusNotGagging".Localized());

            OutputText("Scenes.Heckel.HeckelOralFixationII.NoWayOut".Localized());

            OutputText("Scenes.Heckel.HeckelOralFixationII.EmptiedOnFace".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 4f), new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10 + Player.Sensitivity / 10)));

            ReturnToCamp(1);
        }

        public void HeckelLovesDaAnal(bool loss = false)
        {
            ClearOutput();

            if (Player.AnalCapacity() <= 20 && !loss)
            {
                OutputText("Scenes.Heckel.HeckelLovesDaAnal.BarkingLaugh".Localized(
                    Player.AssDescript()
                ));

                ReturnToCamp(1);

                return ;
            }

            OutputText("Scenes.Heckel.HeckelLovesDaAnal.GrinSplits".Localized());

            OutputText("Scenes.Heckel.HeckelLovesDaAnal.HalfExpect".Localized());

            if (Player.Tone >= 80)
                OutputText("Scenes.Heckel.HeckelLovesDaAnal.HellOfAss".Localized());
            else
                if (Player.Tone >= 60)
                    OutputText("Scenes.Heckel.HeckelLovesDaAnal.NotBad".Localized());

            OutputText("Scenes.Heckel.HeckelLovesDaAnal.ParagraphBreak".Localized());

            OutputText("Scenes.Heckel.HeckelLovesDaAnal.OneHandReleases".Localized(
                Player.ButtDescript()
            ));

            OutputText("Scenes.Heckel.HeckelLovesDaAnal.StrokingStops".Localized());

            OutputText("Scenes.Heckel.HeckelLovesDaAnal.TongueSandpaper".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Heckel.HeckelLovesDaAnal.TrappedBetween".Localized(
                    Player.SMultiCockDesc(),

                    Player.VaginaDescript(0)
                ));
            else
                if (Player.HasCock())
                    OutputText("Scenes.Heckel.HeckelLovesDaAnal.TrappedBetweenAgain".Localized(
                        Player.SMultiCockDesc()
                    ));
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Heckel.HeckelLovesDaAnal.SnatchAches".Localized());

            OutputText("Scenes.Heckel.HeckelLovesDaAnal.FewInches".Localized());

            HeckelLovesAnal2();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedHeckelAnal, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedHeckelAnal) + 1);
        }

        public void HeckelLovesAnal2()
        {
            ClearOutput();

            OutputText("Scenes.Heckel.HeckelLovesAnal2.LegsBump".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.OpenToGasp".Localized());

            Player.ButtChange(28, true, true, false);

            OutputText("Scenes.Heckel.HeckelLovesAnal2.ParagraphBreak".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.MoreDoggieDick".Localized());

            Player.ButtChange(35, true, true, false);

            OutputText("Scenes.Heckel.HeckelLovesAnal2.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.StingFades".Localized());

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Heckel.HeckelLovesAnal2.TrappedBeneath".Localized(
                    Player.VaginaDescript(0)
                ));
            else
                if (Player.HasCock() && Player.Balls > 0)
                    OutputText("Scenes.Heckel.HeckelLovesAnal2.PulsesBeneath".Localized(
                        Player.SMultiCockDesc()
                    ));
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Heckel.HeckelLovesAnal2.BallsSlap".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.DoesntProtest".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Heckel.HeckelLovesAnal2.FuckYourAss".Localized());
            else
                OutputText("Scenes.Heckel.HeckelLovesAnal2.RideYourAss".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.WhispersEar".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.FasterShorter".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.FreezesBites".Localized());

            Player.ButtChange(45, true, true, false);

            OutputText("Scenes.Heckel.HeckelLovesAnal2.ParagraphBreakThird".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Heckel.HeckelLovesAnal2.WastesNoTime".Localized(
                    Player.VaginaDescript(0)
                ));
            else
                if (Player.HasCock())
                    OutputText("Scenes.Heckel.HeckelLovesAnal2.WastesNoTimeAgain".Localized(
                        Player.BallsDescriptLight(),

                        Appearance.CockNoun(BodyPartTypes.Human)
                    ));
                else
                    if (Player.HasVagina())
                        OutputText("Scenes.Heckel.HeckelLovesAnal2.WastesNoTimeThird".Localized(
                            Player.VaginaDescript(0)
                        ));

            OutputText("Scenes.Heckel.HeckelLovesAnal2.OrgasmFades".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.AwakensHoursLater".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.LaysOnBench".Localized());

            OutputText("Scenes.Heckel.HeckelLovesAnal2.HeadsShower".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 4f));

            ReturnToCamp(4);
        }

        public void DominateHeckel()
        {
            ClearOutput();

            OutputText("Scenes.Heckel.DominateHeckel.BendOver".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesDommedHeckel) == 0)
                OutputText("Scenes.Heckel.DominateHeckel.NewTopDog".Localized());
            else
                OutputText("Scenes.Heckel.DominateHeckel.BackForTaste".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.NiceTry".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesDommedHeckel) > 0)
                OutputText("Scenes.Heckel.DominateHeckel.LastTimeFluke".Localized());
            else
                OutputText("Scenes.Heckel.DominateHeckel.AlwaysOnTop".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.WetBitch".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.StepsCloser".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.GonnaBetYours".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesLostHeckelDomChallenge) > 0)
                OutputText("Scenes.Heckel.DominateHeckel.AgainWord".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.SlamsPlexus".Localized());

            if (Player.Toughness < 60)
                OutputText("Scenes.Heckel.DominateHeckel.PushesBack".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.SentencePeriod".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.RiskingIt".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.YoureOn".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.CockyGrin".Localized());

            OutputText("Scenes.Heckel.DominateHeckel.NeverWord".Localized());

            if (Player.Strength < 75)
            {
                OutputText("Scenes.Heckel.DominateHeckel.CannotOvercome".Localized());

                OutputText("Scenes.Heckel.DominateHeckel.YourAssIsMine".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => HeckelLovesDaAnal(true));

                CoC.Instance.Flags.Set(GameFlagIds.TimesLostHeckelDomChallenge, CoC.Instance.Flags.Get(GameFlagIds.TimesLostHeckelDomChallenge) + 1);
            }
            else
            {
                OutputText("Scenes.Heckel.DominateHeckel.TurnsTide".Localized());

                OutputText("Scenes.Heckel.DominateHeckel.LetsGoPalm".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesDommedHeckel) > 0)
                    OutputText("Scenes.Heckel.DominateHeckel.AgainWordSecond".Localized());

                OutputText("Scenes.Heckel.DominateHeckel.EllipsisClose".Localized());

                OutputText("Scenes.Heckel.DominateHeckel.ClaimPrize".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, .5f));

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), DominateHeckelConclusion);

                CoC.Instance.Flags.Set(GameFlagIds.TimesDommedHeckel, CoC.Instance.Flags.Get(GameFlagIds.TimesDommedHeckel) + 1);
            }
        }

        public void DominateHeckelConclusion()
        {
            ClearOutput();

            var x = Player.CockThatFits(70);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Heckel.DominateHeckelConclusion.CirclesTable".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.HeckelProtests".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.FlicksBall".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Heckel.DominateHeckelConclusion.BlushesHotly".Localized());
            else
                if (Player.Corruption < 33)
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.BlushesBit".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Heckel.DominateHeckelConclusion.SmirksLittle".Localized());
                    else
                        OutputText("Scenes.Heckel.DominateHeckelConclusion.GrinsWidely".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.ProbingMotions".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.StepsAside".Localized());

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Heckel.DominateHeckelConclusion.FansNeck".Localized());
            else
                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.DropsOverEyes".Localized());
                else
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.DrapedPenis".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.WhimpersRub".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heckel.DominateHeckelConclusion.ThrobsThicken".Localized());
            else
                OutputText("Scenes.Heckel.DominateHeckelConclusion.ThrobsThickens".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.TooTurnedOn".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.HotWetCavern".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Heckel.DominateHeckelConclusion.SingleDrop".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.SteadyDrip".Localized());
                else
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.NeverEndingFlow".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.PreCumTongue".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.PullsFingersOut".Localized());

            if (Player.BiggestCockArea() > 40)
                OutputText("Scenes.Heckel.DominateHeckelConclusion.AsMuchPossible".Localized());
            else
                OutputText("Scenes.Heckel.DominateHeckelConclusion.ItWord".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.EyesBulge".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.SavorSensation".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.TriesLurch".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.YouAnswer".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.PleadingWhimpers".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Heckel.DominateHeckelConclusion.BastesBalls".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.BastesVagina".Localized());
                else
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.BastesSkin".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.KnowingSmile".Localized(
                Player.Mf("man", "'woman'"),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Heckel.DominateHeckelConclusion.SlowGlide".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Heckel.DominateHeckelConclusion.GropesTits".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.HighPitchedWhine".Localized());

            OutputText("Scenes.Heckel.DominateHeckelConclusion.PumpsLastTime".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() > 500)
            {
                OutputText("Scenes.Heckel.DominateHeckelConclusion.PotentVirility".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.KeepsBlasting".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.WithoutEnd".Localized());

                if (Player.CumQ() >= 3000)
                {
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.PuddleDrips".Localized());

                    if (Player.CumQ() >= 4000)
                    {
                        OutputText("Scenes.Heckel.DominateHeckelConclusion.AtLeast".Localized());

                        if (Player.CumQ() < 10000)
                            OutputText("Scenes.Heckel.DominateHeckelConclusion.FootDeep".Localized());
                        else
                            if (Player.CumQ() < 15000)
                                OutputText("Scenes.Heckel.DominateHeckelConclusion.KneeHigh".Localized());
                            else
                                if (Player.CumQ() < 20000)
                                    OutputText("Scenes.Heckel.DominateHeckelConclusion.WaistHigh".Localized());
                                else
                                    OutputText("Scenes.Heckel.DominateHeckelConclusion.HighAsTable".Localized());
                    }

                    OutputText("Scenes.Heckel.DominateHeckelConclusion.ThankGodsDrains".Localized());
                }
            }

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Heckel.DominateHeckelConclusion.OtherMember".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.SContribute".Localized());
                else
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.Contributes".Localized());

                OutputText("Scenes.Heckel.DominateHeckelConclusion.WonderlandOf".Localized());
            }

            OutputText("Scenes.Heckel.DominateHeckelConclusion.FullySated".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Heckel.DominateHeckelConclusion.MoansAgain".Localized());

            if (silly)
            {
                OutputText("Scenes.Heckel.DominateHeckelConclusion.BaconShake".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) > 0)
                    OutputText("Scenes.Heckel.DominateHeckelConclusion.HighFiveLottie".Localized());

                OutputText("Scenes.Heckel.DominateHeckelConclusion.LoveBacon".Localized());
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

    }
}