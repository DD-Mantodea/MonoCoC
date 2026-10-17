using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<PabloScene>("coc:pablo")]
    public class PabloScene : Scene
    {

        public override void Entry()
        {
            PabloIntro();
        }

        public bool PabloIntro(bool textOutput = true)
        {

            if (View.Time.Hours > 14 && View.Time.Hours <= 20)
            {
                if (textOutput)
                    if (CoC.Instance.Flags.Get(GameFlagIds.PabloMet) == 0)
                        OutputText("Scenes.Pabloscene.PabloIntro.ShortImp".Localized());
                    else
                        OutputText("Scenes.Pabloscene.PabloIntro.PabloBack".Localized());

                return true;
            }
            else
                return false;
        }

        public int PabloAffection(int mod = 0)
        {

            CoC.Instance.Flags.Set(GameFlagIds.PabloAffection, CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) + mod);

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) < 0)
                CoC.Instance.Flags.Set(GameFlagIds.PabloAffection, 0);

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.PabloAffection, 100);

            return CoC.Instance.Flags.Get(GameFlagIds.PabloAffection);
        }

        public void CheckPablosVirginity(bool anal)
        {

            if (anal)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.PabloGotDickedAnally) == 0)
                    OutputText("Scenes.Pabloscene.CheckPablosVirginity.ClaimedAnal".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PabloGotDickedVaginally) == 0)
                    OutputText("Scenes.Pabloscene.CheckPablosVirginity.ClaimedVaginal".Localized());
        }

        public void ApproachPablo()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloMet) == 0)
            {
                OutputText("Scenes.Pabloscene.ApproachPablo.ApproachesImp".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.HelloThere".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.YourName".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.RudeToAsk".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.AreYou".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.HesitatesMoment".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.AnImp".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.LooksLikeImp".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.LoweringHead".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.WhatWasHe".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.HumanOnce".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.EyesWiden".Localized((Player.Race == "human" ? "are" : "were")));

                OutputText("Scenes.Pabloscene.ApproachPablo.ReallyHungry".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.ListensStory".Localized());

                OutputText("Scenes.Pabloscene.ApproachPablo.HairColor".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.PabloMet, 1);

                PabloAffection(10);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) < 80)
                {
                    OutputText("Scenes.Pabloscene.ApproachPablo.ApproachPablo".Localized());

                    OutputText("Scenes.Pabloscene.ApproachPablo.AnotherWorkout".Localized());
                }
                else
                {
                    OutputText("Scenes.Pabloscene.ApproachPablo.GladToSee".Localized());

                    OutputText("Scenes.Pabloscene.ApproachPablo.LookWhosHere".Localized());
                }

            ClearButtons();

            AddNextButton("Common.Button.Yes".Localized(), WorkoutWithPablo);

            AddNextButton("Common.Button.No".Localized(), DeclineWorkout);
        }

        public void WorkoutWithPablo()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloWorkoutCounter) == 0)
            {
                OutputText("Scenes.Pabloscene.WorkoutWithPablo.NodsFauxImp".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                    OutputText("Scenes.Pabloscene.WorkoutWithPablo.CentauressCollects".Localized());

                OutputText("Scenes.Pabloscene.WorkoutWithPablo.RespondsGrin".Localized());

                OutputText("Scenes.Pabloscene.WorkoutWithPablo.FlutterWings".Localized());

                OutputText("Scenes.Pabloscene.WorkoutWithPablo.ParagraphBreak".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) < 80)
            {
                OutputText("Scenes.Pabloscene.WorkoutWithPablo.WorksOutHour".Localized());

                OutputText("Scenes.Pabloscene.WorkoutWithPablo.QuickShower".Localized());
            }
            else
            {
                OutputText("Scenes.Pabloscene.WorkoutWithPablo.AcceptsEagerly".Localized());

                OutputText("Scenes.Pabloscene.WorkoutWithPablo.EnjoyWorkouts".Localized());

                OutputText("Scenes.Pabloscene.WorkoutWithPablo.HopUpBar".Localized());

                OutputText("Scenes.Pabloscene.WorkoutWithPablo.HourIsUp".Localized());
            }

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 40)
                Player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 60)
                Player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 80)
                Player.ApplyStats(new DynStat(GameStats.Strength, 0.5f));

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                Player.ApplyStats(new DynStat(GameStats.Strength, 0.4f));

            Player.ApplyStats(new DynStat(GameStats.Strength, 0.1f));

            Player.ModTone(90);

            Player.ChangeFatigue(20);

            CoC.Instance.Flags.Set(GameFlagIds.PabloWorkoutCounter, CoC.Instance.Flags.Get(GameFlagIds.PabloWorkoutCounter) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) < 60)
                PabloAffection(5);

            PabloAffection(5);

            DoNext(() => ReturnToCamp(1));
        }

        public void DeclineWorkout()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.PabloWorkoutCounter) == 0)
            {
                OutputText("Scenes.Pabloscene.DeclineWorkout.PolitelyDecline".Localized());

                OutputText("Scenes.Pabloscene.DeclineWorkout.ItsAlright".Localized());

                OutputText("Scenes.Pabloscene.DeclineWorkout.MakesLeave".Localized());

                PabloAffection(5);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PabloAffection) < 80)
                {
                    OutputText("Scenes.Pabloscene.DeclineWorkout.DroppedBy".Localized());

                    OutputText("Scenes.Pabloscene.DeclineWorkout.SeemsFlattered".Localized());

                    OutputText("Scenes.Pabloscene.DeclineWorkout.NiceOfYou".Localized());
                }
                else
                {
                    OutputText("Scenes.Pabloscene.DeclineWorkout.SmilesFriend".Localized());

                    OutputText("Scenes.Pabloscene.DeclineWorkout.WellThatsNice".Localized());

                    OutputText("Scenes.Pabloscene.DeclineWorkout.BeamingPride".Localized());

                    OutputText("Scenes.Pabloscene.DeclineWorkout.LookForwardToIt".Localized());
                }

            Go<CampScene>();
        }

        public void PabloSecret()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloSecret.SmellsArmpit".Localized(
                Player.ClothedOrNaked("There, you remove your " + Player.ArmorDescript() + ", stripping naked before grabbing", "There, being naked already, you grab")
            ));

            OutputText("Scenes.Pabloscene.PabloSecret.HumanCock".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.StaredEnough".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.GreetsEnter".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.JumpsBack".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.NoDamnItNo".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.LooksConfused".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.RemovesHands".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.WhyEmbarrass".Localized((Player.Gender == Gender.Hermaphrodite ? ", and you happen to be one yourself." : ".")));

            OutputText("Scenes.Pabloscene.PabloSecret.LooksToSide".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.ActuallyInterested".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.OfCourseInterested".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.StandsSilent".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.LeansStall".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.FrownCrosses".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.FaceLightsUp".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.ReallyGreat".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.SighRelief".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.ItWasNothing".Localized());

            OutputText("Scenes.Pabloscene.PabloSecret.ShutsWater".Localized(
                Player.ClothedOrNaked("re-equip your [Armor] and ")
            ));

            CoC.Instance.Flags.Set(GameFlagIds.PabloSecretLearned, 1);

            PabloAffection(20);

            Player.ChangeFatigue(-10);

            DoNext(() => ReturnToCamp(1));
        }

        public void PabloShowerSexIntro()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloShowerSexIntro.LongWorkout".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerSexIntro.LockerRoom".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerSexIntro.HeyThere".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerSexIntro.TurnsOnWater".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerSexIntro.HeGulps".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerSexIntro.MouthAgape".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerSexIntro.ForgetIt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LetHimGo".Localized(), PabloShowerLetGo);

            AddButton(1, "Common.Button.StopHim".Localized(), PabloShowerStopHim);
        }

        public void PabloShowerLetGo()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloShowerLetGo.ResolvesLetGo".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerLetGo.LooksAtGround".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerLetGo.TurnsOffWater".Localized());

            Player.ChangeFatigue(-10);

            DoNext(() => ReturnToCamp(1));
        }

        public void PabloShowerStopHim()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.SwingOpenDoor".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.ButWhy".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.FindAttractive".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.SoftKiss".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.BreaksKiss".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.HaveIdea".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.AdmiresFigure".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.TooStraightforward".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.WouldntSaySo".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.ReallyWantTo".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.OfCourseYouDo".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.NoddingFuriously".Localized());

            OutputText("Scenes.Pabloscene.PabloShowerStopHim.ToTheStall".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                PabloGetsGrossedOutByWorms();

                return ;
            }

            PabloSexMenu();
        }

        public void PabloSexMenu()
        {

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.UseDick".Localized(), PabloDickingTimeSubMenus);
            else
                AddButton(0, "Common.Button.UseDick".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.UsePussy".Localized(), PabloPussPussSubMenus);
            else
                AddButton(1, "Common.Button.UsePussy".Localized(), null);

            AddButton(2, "Common.Button.UseAss".Localized(), PabloButtsecksSubMenus);
        }

        public void PabloDickingTimeSubMenus()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloDickingTimeSubMenus.HowDickUsed".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakechargeVag".Localized(), () => PabloDickingTime(true, false));

            AddButton(1, "Common.Button.TakechargeAnal".Localized(), null);

            AddButton(2, "Common.Button.PabloleadsVag".Localized(), () => PabloDickingTime(false, false));

            AddButton(3, "Common.Button.PabloleadsAnal".Localized(), null);
        }

        public void PabloDickingTime(bool inCharge = false, bool anal = false)
        {
            ClearOutput();

            if (anal)
            {
                if (inCharge)
                {
                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreak".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakThird".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakFourth".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakFifth".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakSixth".Localized());

                    CheckPablosVirginity(true);
                }
                else
                {
                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakSeventh".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakEighth".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakNinth".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakTenth".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakEleventh".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ParagraphBreakTwelfth".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.PabloGotDickedAnally, CoC.Instance.Flags.Get(GameFlagIds.PabloGotDickedAnally) + 1);
            }
            else
            {
                if (inCharge)
                {
                    if (Player.Tallness >= 60 && !Player.IsTaur())
                    {
                        OutputText("Scenes.Pabloscene.PabloDickingTime.LooksDownImp".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.ScoopsUp".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.TakeMePlease".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.NotKeepWaiting".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.UmWait".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.WhatsWrong".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.HesitatesAnswer".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.OfCourseYouCan".Localized());

                        CheckPablosVirginity(false);

                        OutputText("Scenes.Pabloscene.PabloDickingTime.WhinesSoftly".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.PracticallySqueals".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.FinallySpeaks".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.ReturnsGround".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.ThankYouLover".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Pabloscene.PabloDickingTime.LooksAtImpFriend".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.GoEasyOnMe".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.OfCourseCanAgain".Localized());

                        CheckPablosVirginity(false);

                        OutputText("Scenes.Pabloscene.PabloDickingTime.PracticallySquealsAgain".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.FinallySpeaksAgain".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.GetsUp".Localized());

                        OutputText("Scenes.Pabloscene.PabloDickingTime.ThankYouLoverAgain".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Pabloscene.PabloDickingTime.EyeballsConfusion".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.WantMeLead".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.HoveringOver".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.MightBeStranger".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.GivesNod".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.LicksLips".Localized());

                    CheckPablosVirginity(false);

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ImFine".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.GivesTime".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ImpMoans".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.HalfBadJob".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ReadingMind".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ComesDownHigh".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ThatWasGreat".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.WhatTalkingAbout".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.GuessYoureRight".Localized());

                    OutputText("Scenes.Pabloscene.PabloDickingTime.ImpLeaves".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.PabloGotDickedVaginally, CoC.Instance.Flags.Get(GameFlagIds.PabloGotDickedVaginally) + 1);
            }

            PabloAffection(5);

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void PabloPussPussSubMenus()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloPussPussSubMenus.HowVaginaUsed".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeCharge".Localized(), () => PabloPussPuss(true));

            AddButton(1, "Common.Button.PabloLeads".Localized(), () => PabloPussPuss(false));
        }

        public void PabloPussPuss(bool inCharge = false)
        {
            ClearOutput();

            if (inCharge)
            {
                if (Player.Tallness >= 60 && !Player.IsTaur())
                {
                    OutputText("Scenes.Pabloscene.PabloPussPuss.LooksDownImp".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.ScoopsUp".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.TakeMePlease".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.NotKeepWaiting".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.StopTeasing".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.GrinningEarToEar".Localized());

                    Player.CuntChange(20, true, true, true);

                    OutputText("Scenes.Pabloscene.PabloPussPuss.StartsSlow".Localized(
                        Player.HasCock() ? " while your " + Player.CockDescript() + " shoots jism all over his belly" : ""
                    ));

                    OutputText("Scenes.Pabloscene.PabloPussPuss.FinallySpeaks".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.ReturnsGround".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.ThankYouLover".Localized());
                }
                else
                {
                    OutputText("Scenes.Pabloscene.PabloPussPuss.LooksAtImpFriend".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.SeeWhereGoing".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.HoveringAbove".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.NoSenseTeasing".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.IsntWrong".Localized());

                    Player.CuntChange(20, true, true, true);

                    OutputText("Scenes.Pabloscene.PabloPussPuss.StartsSlowAgain".Localized(
                        Player.HasCock() ? " while your " + Player.CockDescript() + " shoots jism all over his belly" : ""
                    ));

                    OutputText("Scenes.Pabloscene.PabloPussPuss.FinallySpeaksAgain".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.ReturnsGroundAgain".Localized());

                    OutputText("Scenes.Pabloscene.PabloPussPuss.ThankYouLoverAgain".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Pabloscene.PabloPussPuss.EyeballsConfusion".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.WantMeLead".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.HoveringOver".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.MightBeStranger".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.ChuckleConcern".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.IfYouSaySo".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.GoInHeDoes".Localized());

                Player.CuntChange(20, true, true, true);

                OutputText("Scenes.Pabloscene.PabloPussPuss.ImpStartsSlow".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.ThatWasGreat".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.WhatTalkingAbout".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.GuessYoureRight".Localized());

                OutputText("Scenes.Pabloscene.PabloPussPuss.ImpLeaves".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.PabloUsedYourPussy, CoC.Instance.Flags.Get(GameFlagIds.PabloUsedYourPussy) + 1);

            PabloAffection(5);

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void PabloButtsecksSubMenus()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloButtsecksSubMenus.HowAssUsed".Localized(
                Player.AssDescript()
            ));

            ClearButtons();

            AddButton(0, "Common.Button.TakeCharge".Localized(), () => PabloButtSecks(true));

            AddButton(1, "Common.Button.PabloLeads".Localized(), () => PabloButtSecks(false));
        }

        public void PabloButtSecks(bool inCharge)
        {
            ClearOutput();

            if (inCharge)
            {
                OutputText("Scenes.Pabloscene.PabloButtSecks.EyeballsConfusion".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.PutItThere".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ChuckleOwnComment".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.OkayIfYouSay".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.TipPresses".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.WhatAreYouDoing".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.WarnedHim".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.BothKnow".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.SaidPlease".Localized());

                Player.ButtChange(20, true, true, true);

                OutputText("Scenes.Pabloscene.PabloButtSecks.HardAsMustBe".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ImpStartsSlow".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ThatWasGreat".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.WhatTalkingAbout".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.GuessYoureRight".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ImpLeaves".Localized());
            }
            else
            {
                OutputText("Scenes.Pabloscene.PabloButtSecks.EyeballsConfusionAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.PutItThereAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ChuckleOwnCommentAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.PrepareAnus".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.SlipsPastRing".Localized());

                Player.ButtChange(20, true, true, true);

                OutputText("Scenes.Pabloscene.PabloButtSecks.HardForHimAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ImpStartsSlowAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ThatWasGreatAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.WhatTalkingAboutAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.GuessYoureRightAgain".Localized());

                OutputText("Scenes.Pabloscene.PabloButtSecks.ImpLeavesAgain".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.PabloUsedYourPussy, CoC.Instance.Flags.Get(GameFlagIds.PabloUsedYourPussy) + 1);

            PabloAffection(5);

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void PabloGetsGrossedOutByWorms()
        {

            OutputText("Scenes.Pabloscene.PabloGetsGrossedOutByWorms.ContemplatesFucking".Localized());

            OutputText("Scenes.Pabloscene.PabloGetsGrossedOutByWorms.WormsExclaims".Localized());

            OutputText("Scenes.Pabloscene.PabloGetsGrossedOutByWorms.MakesLeave".Localized());

            PabloAffection(-10);

            CoC.Instance.Flags.Set(GameFlagIds.PabloFreakedOutOverWorms, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void PabloComesBackAfterWormCure()
        {
            ClearOutput();

            OutputText("Scenes.Pabloscene.PabloComesBackAfterWormCure.EyesLightUp".Localized());

            OutputText("Scenes.Pabloscene.PabloComesBackAfterWormCure.GreetsTailLimp".Localized());

            OutputText("Scenes.Pabloscene.PabloComesBackAfterWormCure.CheerfulGleam".Localized());

            OutputText("Scenes.Pabloscene.PabloComesBackAfterWormCure.ThatsGreat".Localized());

            OutputText("Scenes.Pabloscene.PabloComesBackAfterWormCure.EagerOne".Localized());

            PabloAffection(10);

            CoC.Instance.Flags.Set(GameFlagIds.PabloFreakedOutOverWorms, 0);

            PabloSexMenu();
        }

    }
}