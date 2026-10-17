using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<Jasun>("coc:jasun")]
    public class Jasun : Scene
    {

        public override void Entry()
        {
            ChangingRoom();
        }

        public void ChangingRoom()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
            {
                OutputText("Scenes.Jasun.ChangingRoom.PaysCentaur".Localized());

                Player.Gems -= 10;

            }

            if (CoC.Instance.Flags.Get(GameFlagIds.JasunMet) == 0)
                OutputText("Scenes.Jasun.ChangingRoom.EntersStall".Localized());
            else
                OutputText("Scenes.Jasun.ChangingRoom.ReturnsStall".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.JasunNameLearned) == 0)
                AddButton(0, "Common.Button.LookAround".Localized(), MeetJasun);
            else
                AddButton(0, "Common.Button.Jasun".Localized(), MeetJasun);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void MeetJasun()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.JasunMet) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.JasunMet, CoC.Instance.Flags.Get(GameFlagIds.JasunMet) + 1);

                OutputText("Scenes.Jasun.MeetJasun.SurveysRoom".Localized());

                if (silly)
                    OutputText("Scenes.Jasun.MeetJasun.SixWord".Localized());
                else
                    OutputText("Scenes.Jasun.MeetJasun.ThreeWord".Localized());

                OutputText("Scenes.Jasun.MeetJasun.FeetSuffix".Localized());

                OutputText("Scenes.Jasun.MeetJasun.BlinksTwice".Localized());

                if ((int)Player.Gender <= 1)
                {
                    OutputText("Scenes.Jasun.MeetJasun.EyesToYourself".Localized());

                    OutputText("Scenes.Jasun.MeetJasun.DoubtsNerve".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.JasunMet, 0);

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
                else
                {
                    OutputText("Scenes.Jasun.MeetJasun.NoBetterThan".Localized());

                    OutputText("Scenes.Jasun.MeetJasun.TakenAback".Localized());

                    OutputText("Scenes.Jasun.MeetJasun.ExcuseMeLady".Localized());

                    if (Player.Tone < 75)
                    {
                        OutputText("Scenes.Jasun.MeetJasun.ThrustsPast".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

                        OutputText("Scenes.Jasun.MeetJasun.BooksIt".Localized());

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    OutputText("Scenes.Jasun.MeetJasun.SuggestsSwim".Localized());

                    if (Player.ArmorName != "slutty swimwear")
                    {
                        OutputText("Scenes.Jasun.MeetJasun.JustifiedWalk".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

                        OutputText("Scenes.Jasun.MeetJasun.BooksItAgain".Localized());

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    OutputText("Scenes.Jasun.MeetJasun.MotionsHand".Localized());

                    DoNext(() => MeetJasun2());

                    return ;
                }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.JasunMet) > 0 && CoC.Instance.Flags.Get(GameFlagIds.JasunFuckedCounter) == 0)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.JasunNameLearned) == 0)
                    {
                        OutputText("Scenes.Jasun.MeetJasun.LooksAroundStall".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.JasunNameLearned, CoC.Instance.Flags.Get(GameFlagIds.JasunNameLearned) + 1);
                    }

                    if ((int)Player.Gender <= 1 || (Player.Gender == Gender.Hermaphrodite && Player.Mf("m", "f") == "m"))
                    {
                        OutputText("Scenes.Jasun.MeetJasun.WalksOutStall".Localized());

                        OutputText("Scenes.Jasun.MeetJasun.EyesToYourselfAgain".Localized());

                        OutputText("Scenes.Jasun.MeetJasun.DoubtsNerveAgain".Localized());

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    if (Player.Tone < 75 || Player.ArmorName != "slutty swimwear")
                    {
                        OutputText("Scenes.Jasun.MeetJasun.WalksOutStallAgain".Localized());

                        OutputText("Scenes.Jasun.MeetJasun.StillDontSwim".Localized());

                        if (Player.Tone < 75)
                            OutputText("Scenes.Jasun.MeetJasun.FitEnough".Localized());
                        else
                            if (Player.ArmorName != "slutty swimwear")
                                OutputText("Scenes.Jasun.MeetJasun.ProperlyAttired".Localized());
                            else
                                OutputText("Scenes.Jasun.MeetJasun.MoreCompatible".Localized());

                        OutputText("Scenes.Jasun.MeetJasun.CanGoSwim".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, -15f));

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    OutputText("Scenes.Jasun.MeetJasun.ComesOutStall".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), () => MeetJasun2()); AddButton(1, "No", () => TurnAwayFromAdj());

                    return ;
                }
                else
                {
                    if ((int)Player.Gender <= 1 || (Player.Gender == Gender.Hermaphrodite && Player.Mf("m", "f") == "m"))
                    {
                        OutputText("Scenes.Jasun.MeetJasun.WalksOutStallThird".Localized());

                        OutputText("Scenes.Jasun.MeetJasun.EyesToYourselfThird".Localized());

                        OutputText("Scenes.Jasun.MeetJasun.DoubtsNerveThird".Localized());

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    if (Player.Tone < 75 || Player.ArmorName != "slutty swimwear")
                    {
                        OutputText("Scenes.Jasun.MeetJasun.WalksOutStallFourth".Localized());

                        OutputText("Scenes.Jasun.MeetJasun.ISeeThat".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, -15f));

                        DoNext(() => ReturnToCamp(1));

                        return ;
                    }

                    OutputText("Scenes.Jasun.MeetJasun.StepsOutStall".Localized());

                    DoNext(() => JasunSecks());

                    return ;
                }
        }

        public void MeetJasun2()
        {
            ClearOutput();

            OutputText("Scenes.Jasun.MeetJasun2.WalkTogether".Localized());

            if (Player.Strength < 50)
                OutputText("Scenes.Jasun.MeetJasun2.PrettySure".Localized());

            OutputText("Scenes.Jasun.MeetJasun2.SuddenlyStops".Localized());

            OutputText("Scenes.Jasun.MeetJasun2.MyNameIsJasun".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.JasunNameLearned, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), AcceptJasunsAdvances);

            AddButton(1, "Common.Button.TurnAway".Localized(), TurnAwayFromAdj);
        }

        public void TurnAwayFromAdj()
        {
            ClearOutput();

            OutputText("Scenes.Jasun.TurnAwayFromAdj.UnderstandDejected".Localized());

            OutputText("Scenes.Jasun.TurnAwayFromAdj.BooksItAway".Localized());

            DoNext(() => ReturnToCamp(1));

            return ;
        }

        public void AcceptJasunsAdvances()
        {
            ClearOutput();

            OutputText("Scenes.Jasun.AcceptJasunsAdvances.SmilesWider".Localized());

            Player.HasGottenWashed();

            OutputText("Scenes.Jasun.AcceptJasunsAdvances.CountlessScars".Localized());

            DoNext(() => JasunSecks());
        }

        public void JasunSecks()
        {
            ClearOutput();

            Player.SlimeFeed();

            if (CoC.Instance.Flags.Get(GameFlagIds.JasunFuckedCounter) == 0)
            {
                OutputText("Scenes.Jasun.JasunSecks.GripsBack".Localized());

                OutputText("Scenes.Jasun.JasunSecks.ReachesFolds".Localized());

                OutputText("Scenes.Jasun.JasunSecks.SteadiesHips".Localized());

                Player.CuntChange(60, true, false, true);

                OutputText("Scenes.Jasun.JasunSecks.HalfFits".Localized());

                OutputText("Scenes.Jasun.JasunSecks.YouComeTo".Localized());
            }
            else
            {
                OutputText("Scenes.Jasun.JasunSecks.SwimsOver".Localized());

                OutputText("Scenes.Jasun.JasunSecks.PullsBack".Localized());

                Player.CuntChange(60, true, true, false);

                OutputText("Scenes.Jasun.JasunSecks.BobsWater".Localized());

                OutputText("Scenes.Jasun.JasunSecks.FloatsAwhile".Localized());

                OutputText("Scenes.Jasun.JasunSecks.WakesUp".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.JasunFuckedCounter, CoC.Instance.Flags.Get(GameFlagIds.JasunFuckedCounter) + 1);

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}