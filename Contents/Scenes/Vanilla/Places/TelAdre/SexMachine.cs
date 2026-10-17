using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<SexMachine>("coc:sex_machine")]
    public class SexMachine : Scene
    {

        public override void Entry()
        {
            ExploreShowers();
        }

        public void ExploreShowers()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GymMachineStatus) <= 1 && CoC.Instance.Flags.Get(GameFlagIds.DisabledSexMachine) <= 0)
            {
                OutputText("Scenes.Sexmachine.ExploreShowers.WorkedBody".Localized());

                OutputText("Scenes.Sexmachine.ExploreShowers.WalksBackHalls".Localized());

                OutputText("Scenes.Sexmachine.ExploreShowers.ShoutsAfter".Localized());

                OutputText("Scenes.Sexmachine.ExploreShowers.PolishedSteel".Localized());

                OutputText("Scenes.Sexmachine.ExploreShowers.UseItQuestion".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => UseTheSexMachine()); AddButton(1, "No", () => LeaveShowers());

                AddButton(2, "Common.Button.FuckNo".Localized(), DisableMachineForGood);

                CoC.Instance.Flags.Set(GameFlagIds.GymMachineStatus, 1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BrookeMet) == 0)
                    SceneInstance<BrookeScene>.Get().MeetBrookeFirstTime();
                else
                    UseTheSexMachine();
        }

        public void LeaveShowers()
        {
            ClearOutput();

            OutputText("Scenes.Sexmachine.LeaveShowers.ShakesHeadLeaves".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DisableMachineForGood()
        {
            ClearOutput();

            OutputText("Scenes.Sexmachine.DisableMachineForGood.NoWayUse".Localized());

            OutputText("Scenes.Sexmachine.DisableMachineForGood.NoEncounterBanner".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DisabledSexMachine, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void UseTheSexMachine()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.GymMachineStatus, 2);

            if (Player.Gender == Gender.Male)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsMale) == 0)
                {
                    OutputText("Scenes.Sexmachine.UseTheSexMachine.SitsPaddedBench".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesAsTold".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Sexmachine.UseTheSexMachine.PrimarySuffix".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.TighteningAround".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.TakesLittleTime".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.ShakesHead".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesntTakeLong".Localized());

                    Player.ButtChange(30, true, true, false);

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.ScreenStates".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.AnalPhallus".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Sexmachine.UseTheSexMachine.BallsPullingUp".Localized());
                    else
                        OutputText("Scenes.Sexmachine.UseTheSexMachine.ProstatePulsing".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.PumpsSeed".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.SurprisedMachine".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.GoblinEmerges".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.CriesOutNotices".Localized());
                }
                else
                {
                    OutputText("Scenes.Sexmachine.UseTheSexMachine.SlySmirk".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.WalksBackHalls".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.PlacesLegs".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.RockHard".Localized(
                         Player.CockHead()
                    ));

                    Player.ButtChange(30, true, true, false);

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.ParagraphBreak".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.BroughtToEdge".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.HalfHourTorment".Localized());

                    OutputText("Scenes.Sexmachine.UseTheSexMachine.PrepareToLeave".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.TimesUsedSexMachineAsMale, CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsMale) + 1);
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsFemale) == 0)
                    {
                        OutputText("Scenes.Sexmachine.UseTheSexMachine.SitsGently".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.MachineLifts".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.MachineBegins".Localized(
                             Appearance.AllBreastsDescript(Player)
                        ));

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.ContinuingMolestations".Localized());

                        Player.CuntChange(30, true, true, false);

                        Player.ButtChange(30, true, true, false);

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.ParagraphBreakTwo".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.GaspsPleasure".Localized());

                        if (Player.BiggestBreastSize() >= 2)
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.FourthCock".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.ParagraphBreakThree".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesntTakeLongUntil".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.MachineSlows".Localized());

                        if (Player.BiggestBreastSize() >= 2)
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.BreastsCovered".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.JizzLeaking".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.GoblinReturns".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.SheTurnsBack".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sexmachine.UseTheSexMachine.GrinsWalking".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.SitsBench".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.CoolAir".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.MoansLustfully".Localized());

                        Player.CuntChange(30, true, true, false);

                        Player.ButtChange(30, true, true, false);

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.BeginStroking".Localized());

                        if (Player.BiggestBreastSize() >= 2)
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesntTakeLongSuffix".Localized());
                        else
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.MachineRotates".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.SentencePeriodBreak".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.LegsLifted".Localized());

                        OutputText("Scenes.Sexmachine.UseTheSexMachine.PantsLetDown".Localized());
                    }

                    CoC.Instance.Flags.Set(GameFlagIds.TimesUsedSexMachineAsFemale, CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsFemale) + 1);
                }
                else
                    if (Player.Gender == Gender.Genderless)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsEunuch) == 0)
                        {
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.SitsDown".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.RaisesEyebrow".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.WhirsConfused".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.GaspsBounce".Localized());

                            Player.ButtChange(30, true, false, true);

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.ForcesForwards".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesntTakeLongAgain".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.PullsOutSlowly".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.CollectsYourself".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.WalksHallsAfter".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.StripsNaked".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.ShrugsItOff".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.LooksBack".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesntTakeLongThird".Localized());

                            Player.ButtChange(30, true, false, true);

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.MoansEyesClosed".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.MachineKnows".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.CantTakeIt".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.TenMinutesPass".Localized());
                        }

                        CoC.Instance.Flags.Set(GameFlagIds.TimesUsedSexMachineAsEunuch, CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsEunuch) + 1);
                    }
                    else
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsHerm) == 0)
                        {
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.SighsRelaxation".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.EyebrowButDo".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.MoansGently".Localized());

                            if (Player.CockTotal() > 1)
                                OutputText("Scenes.Sexmachine.UseTheSexMachine.PrimaryCockSuffix".Localized());
                            else
                                OutputText(Player.CockDescript(0));

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.StrokedBySheath".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.ImpossibleToStay".Localized());

                            Player.CuntChange(30, true, true, false);

                            Player.ButtChange(30, true, true, false);

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.GirlyJuices".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.MoansAroundCock".Localized());

                            if (Player.BiggestBreastSize() >= 2)
                                OutputText("Scenes.Sexmachine.UseTheSexMachine.NotEveryPart".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.ParagraphBreakFive".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesntTakeLongFourth".Localized());

                            if (Player.BiggestBreastSize() >= 2)
                                OutputText("Scenes.Sexmachine.UseTheSexMachine.ChestCockPumps".Localized());
                            else
                                OutputText("Scenes.Sexmachine.UseTheSexMachine.YLetter".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.MilkedOfAll".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.KegAppears".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.SnatchesKeg".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Sexmachine.UseTheSexMachine.WipesOff".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.MetallicClick".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.ThoughtStirs".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.LatexTentacles".Localized());

                            Player.CuntChange(30, true, true, false);

                            Player.ButtChange(30, true, true, false);

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.ParagraphBreakSix".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.SucksFirmly".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.TentacleInAss".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.DoesntTakeLongFifth".Localized());

                            OutputText("Scenes.Sexmachine.UseTheSexMachine.PantsLetDownAgain".Localized());
                        }

                        CoC.Instance.Flags.Set(GameFlagIds.TimesUsedSexMachineAsHerm, CoC.Instance.Flags.Get(GameFlagIds.TimesUsedSexMachineAsHerm) + 1);
                    }

            if (Player.HasVagina())
            {
                var temp = Random.Shared.Next(6);

                switch (temp)
                {
                    case 0:
                    {
                    Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);
                    }

                    break;

                    case 1:
                    {
                    Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation);
                    }

                    break;

                    case 2:
                    {
                    Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation);
                    }

                    break;
                }

                if (true )
                    Player.KnockUp(PregnancyIds.Centaur, PregnancyIds.CentaurIncubation);
            }

            Player.ChangeFatigue(10);

            if (Player.HasVagina())
                Player.Orgasm("Vaginal");

            Player.Orgasm("Anal");

            DoNext(() => ReturnToCamp(1));
        }

    }
}