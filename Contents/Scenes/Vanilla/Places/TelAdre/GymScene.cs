using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<GymScene>("coc:gym")]
    public class GymScene : Scene
    {
        public override void Entry()
        {
            GymDesc();
        }

        public void GymDesc()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.PcIsAGoodCottonDad) + CoC.Instance.Flags.Get(GameFlagIds.PcIsADeadbeatCottonDad) == 0 && Player.IsPregnant())
            {
                SceneInstance<CottonScene>.Get().CottonPregnantAlert();

                return;
            }

            OutputText("Scenes.Gym.GymDesc.IngnamLead".Localized());

            if (Player.Corruption < 35)
                OutputText("Scenes.Gym.GymDesc.ThankfullyWord".Localized());
            else
                if (SceneInstance<CeraphScene>.Get().HasExhibition() || Player.Corruption > 80)
                    OutputText("Scenes.Gym.GymDesc.UnfortunatelyWord".Localized());

            OutputText("Scenes.Gym.GymDesc.FullSizedWalls".Localized());

            OutputText("Scenes.Gym.GymDesc.HeavyUse".Localized());

            OutputText("Scenes.Gym.GymDesc.CentauressTankTop".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                OutputText("Scenes.Gym.GymDesc.HandsOnHips".Localized());
            else
                OutputText("Scenes.Gym.GymDesc.NiceWorkout".Localized());

            if (Player.Gems < 10 && CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
            {
                OutputText("Scenes.Gym.GymDesc.ReachPockets".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return;
            }

            SceneInstance<LottieScene>.Get().LottieAppearance()?.Invoke();

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeMet) > 0 && CoC.Instance.Flags.Get(GameFlagIds.LoppeDisabled) == 0)
                OutputText("Scenes.Gym.GymDesc.SpotLoppe".Localized());

            if (View.Time.Hours > 9 && View.Time.Hours <= 15)
                SceneInstance<HeckelScene>.Get().HeckelAppearance();

            GymMenu();
        }

        public void GymMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Changeroom".Localized(), SceneInstance<Jasun>.Get().ChangingRoom);

            AddButton(1, "Common.Button.Jog".Localized(), GoJogging);

            AddButton(2, "Common.Button.Liftweights".Localized(), WeightLifting);

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0 && Player.Gems >= 500)
                AddButton(4, "Common.Button.LifeMember".Localized(), BuyGymLifeTimeMembership);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) > 0)
                    AddButton(4, "Common.Button.LifeMember".Localized(), null);
                else
                    AddButton(4, "Common.Button.LifeMember".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.PcIsADeadbeatCottonDad) == 0 && SceneInstance<CottonScene>.Get().CottonsIntro())
                AddButton(5, CoC.Instance.Flags.Get(GameFlagIds.CottonMetFucked) > 0 ? "Npcs.Cotton.Name".Localized() : "Common.Text.Horsegirl".Localized(), SceneInstance<CottonScene>.Get().CottonGreeting);

            if (View.Time.Hours > 9 && View.Time.Hours <= 15)
                AddButton(6, CoC.Instance.Flags.Get(GameFlagIds.MetHeckel) > 0 ? "Common.Button.Heckel".Localized() : "Common.Text.Hyena".Localized(), SceneInstance<HeckelScene>.Get().GreetHeckel);

            if (SceneInstance<IfrisScene>.Get().IfrisIntro())
                AddButton(7, CoC.Instance.Flags.Get(GameFlagIds.MetIfris) > 0 ? "Common.Text.Ifris".Localized() : "Common.Text.DemonGirl".Localized(), SceneInstance<IfrisScene>.Get().ApproachIfris);

            AddButton(8, CoC.Instance.Flags.Get(GameFlagIds.LottieEncounterCounter) > 0 ? "Common.Button.Lottie".Localized() : "Common.Text.PigGirl".Localized(), () => SceneInstance<LottieScene>.Get().LottieAppearance(false)?.Invoke());

            if (CoC.Instance.Flags.Get(GameFlagIds.LoppeMet) > 0 && CoC.Instance.Flags.Get(GameFlagIds.LoppeDisabled) == 0)
                AddButton(9, "Common.Button.Loppe".Localized(), SceneInstance<LoppeScene>.Get().LoppeGenericMeetings);

            if (SceneInstance<PabloScene>.Get().PabloIntro() && CoC.Instance.Flags.Get(GameFlagIds.PabloFreakedOutOverWorms) != 1)
                AddButton(10, CoC.Instance.Flags.Get(GameFlagIds.PabloMet) > 0 ? "Common.Text.Pablo".Localized() : "Common.Text.Imp".Localized(), SceneInstance<PabloScene>.Get().ApproachPablo);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void BuyGymLifeTimeMembership()
        {
            ClearOutput();

            if (silly)
                OutputText("Scenes.Gym.BuyGymLifeTimeMembership.ShutUpTakeGems".Localized());

            OutputText("Scenes.Gym.BuyGymLifeTimeMembership.PaysFiveHundred".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Gym.BuyGymLifeTimeMembership.FlushFace".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));
            }

            OutputText("Scenes.Gym.BuyGymLifeTimeMembership.MembershipGained".Localized());

            Player.CreateKeyItem("Gym Membership", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.LifetimeGymMember, 1);

            Player.Gems -= 500;

            GymMenu();
        }

        public void WeightLifting()
        {
            ClearOutput();

            if (Player.Fatigue > Player.MaxFatigue() - 25)
            {
                OutputText("Scenes.Gym.WeightLifting.CantExerciseBanner".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                    OutputText("Scenes.Gym.WeightLifting.SaveMoney".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                Player.Gems -= 10;

            Player.ChangeFatigue(25);

            OutputText("Scenes.Gym.WeightLifting.WalksToWeights".Localized());

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 25)
                OutputText("Scenes.Gym.WeightLifting.SmallerWeights".Localized());
            else
                if ((100 * Player.Strength / Player.GetMaxStats("str")) < 40)
                    OutputText("Scenes.Gym.WeightLifting.HeftsFew".Localized());
                else
                    if ((100 * Player.Strength / Player.GetMaxStats("str")) < 60)
                        OutputText("Scenes.Gym.WeightLifting.GripsHeavier".Localized());
                    else
                        if ((100 * Player.Strength / Player.GetMaxStats("str")) < 80)
                            OutputText("Scenes.Gym.WeightLifting.HeaviestDumbbells".Localized());
                        else
                            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                                OutputText("Scenes.Gym.WeightLifting.GrabHeaviest".Localized());
                            else
                                OutputText("Scenes.Gym.WeightLifting.NothingLeftChallenge".Localized());

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                Player.ApplyStats(new DynStat(GameStats.Strength, .5f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 40)
                Player.ApplyStats(new DynStat(GameStats.Toughness, .3f));

            PromptShowers();
        }

        public void GoJogging()
        {
            ClearOutput();

            if (Player.Fatigue > Player.MaxFatigue() - 30)
            {
                OutputText("Scenes.Gym.GoJogging.CantExerciseBanner".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                    OutputText("Scenes.Gym.GoJogging.SaveMoney".Localized());

                Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());

                return;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.LifetimeGymMember) == 0)
                Player.Gems -= 10;

            Player.ChangeFatigue(30);

            OutputText("Scenes.Gym.GoJogging.HitsTrack".Localized());

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 25)
                OutputText("Scenes.Gym.GoJogging.GetsWinded".Localized());
            else
                if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 40)
                    OutputText("Scenes.Gym.GoJogging.PerformancePoor".Localized());
                else
                    if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 60)
                        OutputText("Scenes.Gym.GoJogging.DoesQuiteWell".Localized());
                    else
                        if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 80)
                            OutputText("Scenes.Gym.GoJogging.DoesntFaze".Localized());
                        else
                            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 90)
                                OutputText("Scenes.Gym.GoJogging.TerrificTime".Localized());
                            else
                                OutputText("Scenes.Gym.GoJogging.BarelyChallenges".Localized());

            if ((100 * Player.Speed / Player.GetMaxStats("spe")) < 40)
                Player.ApplyStats(new DynStat(GameStats.Speed, .3f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 90)
                Player.ApplyStats(new DynStat(GameStats.Toughness, .5f));

            if (Player.Body.Butt >= 15)
            {
                OutputText("Scenes.Gym.GoJogging.RunningDoneGood".Localized());

                Player.Body.Butt--;
            }
            else
                if (Player.Body.Butt >= 10 && Random.Shared.Next(3) == 0)
                {
                    OutputText("Scenes.Gym.GoJogging.TrimmedUp".Localized());

                    Player.Body.Butt--;
                }
                else
                    if (Player.Body.Butt >= 5 && Random.Shared.Next(3) == 0)
                    {
                        OutputText("Scenes.Gym.GoJogging.AssGottenLittle".Localized());

                        Player.Body.Butt--;
                    }
                    else
                        if (Player.Body.Butt > 1 && Random.Shared.Next(4) == 0)
                        {
                            OutputText("Scenes.Gym.GoJogging.AssGottenLittleAgain".Localized());

                            Player.Body.Butt--;
                        }

            if (Player.Body.Hips >= 15)
            {
                OutputText("Scenes.Gym.GoJogging.HipsShed".Localized());

                Player.Body.Hips--;
            }
            else
                if (Player.Body.Hips >= 10 && Random.Shared.Next(3) == 0)
                {
                    OutputText("Scenes.Gym.GoJogging.HipsShedTwo".Localized());

                    Player.Body.Hips--;
                }
                else
                    if (Player.Body.Hips >= 5 && Random.Shared.Next(3) == 0)
                    {
                        OutputText("Scenes.Gym.GoJogging.HipsShedThree".Localized());

                        Player.Body.Hips--;
                    }
                    else
                        if (Player.Body.Hips > 1 && Random.Shared.Next(4) == 0)
                        {
                            OutputText("Scenes.Gym.GoJogging.HipsShedFour".Localized());

                            Player.Body.Hips--;
                        }

            OutputText(Player.ModThickness(1, 5 + Random.Shared.Next(2)));

            PromptShowers();
        }

        public void PromptShowers()
        {

            OutputText("Scenes.Gym.PromptShowers.HitTheShowers".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BrookeMet) == 1)
            {
                ClearButtons();

                if (CoC.Instance.Flags.Get(GameFlagIds.DisabledSexMachine) == 0)
                {
                    AddButton(0, "Common.Text.ShowersQuoted".Localized(), SceneInstance<SexMachine>.Get().ExploreShowers);

                    AddButton(1, "Common.Button.Showers".Localized(), SceneInstance<BrookeScene>.Get().RepeatChooseShower);

                    SetExitButton("Common.Button.Back".Localized());
                }
                else
                {
                    AddButton(0, "Common.Button.Yes".Localized(), SceneInstance<BrookeScene>.Get().RepeatChooseShower);

                    AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
                }
            }
            else
            {
                AddButton(0, "Common.Button.Yes".Localized(), SceneInstance<SexMachine>.Get().ExploreShowers);

                AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
            }
        }

    }
}