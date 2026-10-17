using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Seasonal
{
    [Register<ValentinesScene>("coc:valentines")]
    public class ValentinesScene : Scene
    {

        public override void Entry()
        {
            CrazyVDayShenanigansByVenithil();
        }

        public void CrazyVDayShenanigansByVenithil()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.ValentinesEventYear, DateTime.Now.Year);

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.StreetsOfTelAdre".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.StallNextTavern".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.RecognizeAbylon".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.RaisingEyebrow".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.PointsStall".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.NodsToAbby".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.HopedToSeeYou".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.PleasantThought".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.DeserveRecognition".Localized());
                else
                    OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.NowThatsThought".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.LiftingFinger".Localized());

            OutputText("Scenes.Valentines.CrazyVDayShenanigansByVenithil.MoreImportant".Localized());

            AwardAchievement(AchievementIds.HolidayValentine);

            ClearButtons();

            AddButton(0, "Common.Button.HelpThem".Localized(), HelpValentinesDay);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveValentinesDayForever);
        }

        public void HelpValentinesDay()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.HelpValentinesDay.OverallAppreciation".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), HelpValentinesDayII);
        }

        public void LeaveValentinesDayForever()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.LeaveValentinesDayForever.NotWorthTime".Localized());

            ReturnToCamp(1);
        }

        public void HelpValentinesDayII()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.HelpValentinesDayII.DecidingHelp".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.PawnShop".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.FindsOswald".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.VictoriaRoses".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.ExplainsIdea".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.YvonneBlushes".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.YaraEnthusiastic".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.BakeryWeaponShop".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayII.SmallCrowd".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), HelpValentinesDayIII);
        }

        public void HelpValentinesDayIII()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.HelpValentinesDayIII.RestOfEvening".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayIII.SunSetting".Localized());

            OutputText("Scenes.Valentines.HelpValentinesDayIII.HowSpendEvening".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Scylla".Localized(), GoVisitScyllaVday);

            AddButton(1, "Common.Button.Abby".Localized(), GoVisitAbbyVday);

            if (Player.HasCockThatFits(28) || Player.HasVagina())
                AddButton(2, "Common.Button.Pastie".Localized(), GoVisitPastyVDay);

            AddButton(14, "Common.Button.Leave".Localized(), GoHomeFromVDay);
        }

        public void GoHomeFromVDay()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoHomeFromVDay.WorkIsDone".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ReturnToCamp(2);
        }

        public void GoVisitScyllaVday()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoVisitScyllaVday.GoodbyesAbby".Localized());

            OutputText("Scenes.Valentines.GoVisitScyllaVday.MoreThanWilling".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GoVisitScyllaVdayII);
        }

        public void GoVisitScyllaVdayII()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoVisitScyllaVdayII.LongerWalk".Localized());

            OutputText("Scenes.Valentines.GoVisitScyllaVdayII.SimplyListen".Localized());

            OutputText("Scenes.Valentines.GoVisitScyllaVdayII.ExplainsResting".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GoVisitScyllaVdayIII);
        }

        public void GoVisitScyllaVdayIII()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoVisitScyllaVdayIII.GuardsRecognize".Localized());

            OutputText("Scenes.Valentines.GoVisitScyllaVdayIII.AnotherDoor".Localized());

            OutputText("Scenes.Valentines.GoVisitScyllaVdayIII.HooksArm".Localized());

            OutputText("Scenes.Valentines.GoVisitScyllaVdayIII.PonderResponse".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), CuddleWithScyllaVDay);

            AddButton(1, "Common.Button.MakeOut".Localized(), MakeOutWithScyllaVDay);

            AddButton(2, "Common.Button.FeedHer".Localized(), FeedScyllaVDay);
        }

        public void CuddleWithScyllaVDay()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.CuddleWithScyllaVDay.SpecialMoment".Localized());

            OutputText("Scenes.Valentines.CuddleWithScyllaVDay.CuddleMore".Localized());

            OutputText("Scenes.Valentines.CuddleWithScyllaVDay.CuddledTime".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Valentines.CuddleWithScyllaVDay.VaginaMoist".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valentines.CuddleWithScyllaVDay.DoneDeal".Localized());

            OutputText("Scenes.Valentines.CuddleWithScyllaVDay.LustArousal".Localized());

            OutputText("Scenes.Valentines.CuddleWithScyllaVDay.EyesSparkle".Localized());

            OutputText("Scenes.Valentines.CuddleWithScyllaVDay.LaterDesert".Localized());

            OutputText("Scenes.Valentines.CuddleWithScyllaVDay.PureLovingPerk".Localized());

            Player.CreatePerk(PerkIds.PureAndLoving, 0, 0, 0, 0);

            ReturnToCamp(2);
        }

        public void MakeOutWithScyllaVDay()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.BeforeLong".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.NotContentPart".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.TongueUnwraps".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.CompletelyHard".Localized());
            else
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.EachCockHard".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.SentencePeriod".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.NoticesPresses".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.ProdigiousChest".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.BreakKiss".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.RefuseLady".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDay.StealFirstOrgasm".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MakeOutWithScyllaVDayII);
        }

        public void MakeOutWithScyllaVDayII()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.KissesForehead".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.EagerlyMakeOut".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.FiresInsideTit".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.EasyToLose".Localized());
            else
                if (Player.CumQ() < 1500)
                    OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.BlessedCumMilker".Localized());
                else
                    OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.TitThirsts".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.LiftsOff".Localized());

            OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.HourLater".Localized());

            if (!Player.HasPerk(PerkIds.SensualLover))
            {
                OutputText("Scenes.Valentines.MakeOutWithScyllaVDayII.SensualLoverPerk".Localized());

                Player.CreatePerk(PerkIds.SensualLover, 0, 0, 0, 0);
            }

            ReturnToCamp(2);
        }

        public void FeedScyllaVDay()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.FeedScyllaVDay.GallonOfJizz".Localized());

            Player.Orgasm("Dick");

            OutputText("Scenes.Valentines.FeedScyllaVDay.OneTrackMind".Localized());

            Player.CreatePerk(PerkIds.OneTrackMind, 0, 0, 0, 0);

            ReturnToCamp(2);
        }

        public void GoVisitAbbyVday()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoVisitAbbyVday.WavesScylla".Localized());

            OutputText("Scenes.Valentines.GoVisitAbbyVday.AsksCompany".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GoVisitAbby);
        }

        public void GoVisitAbby()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoVisitAbby.YouKiddingRight".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.IfSheWasntWrong".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.MakesSenseGuess".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.MusesGoblin".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.BreaksSilence".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.KnownScyllaFor".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.HeroSavingNun".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.FirstbornDaughter".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.AndAGirl".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.LessCocky".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.DeeplyFlushes".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.SmilesCoyly".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.PicksUpPace".Localized());

            OutputText("Scenes.Valentines.GoVisitAbby.MakeYourselfHome".Localized());

            ClearButtons();

            if (Player.HasCock())
            {
                var x = Player.CockThatFits(46);

                if (Player.CockArea(x) < 46)
                {
                    OutputText("Scenes.Valentines.GoVisitAbby.GoblinCuntDip".Localized());

                    AddButton(1, "Common.Button.FuckHer".Localized(), FuckAbbyVDay);
                }
                else
                    OutputText("Scenes.Valentines.GoVisitAbby.LookOfFright".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Valentines.GoVisitAbby.CertainDoubts".Localized());

            AddButton(0, "Common.Button.Pleasureher".Localized(), PleasureAbbyVDay);
        }

        public void FuckAbbyVDay()
        {
            ClearOutput();

            var x = Player.CockThatFits(46);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Valentines.FuckAbbyVDay.GoblinFeltOutPlace".Localized());

            OutputText("Scenes.Valentines.FuckAbbyVDay.TeasesLonger".Localized());

            OutputText("Scenes.Valentines.FuckAbbyVDay.WetAndWaiting".Localized());

            OutputText("Scenes.Valentines.FuckAbbyVDay.FeedsInch".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Valentines.FuckAbbyVDay.PicksUpPace".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Valentines.FuckAbbyVDay.SatedDisentangle".Localized());

            if (!Player.HasPerk(PerkIds.SensualLover))
            {
                OutputText("Scenes.Valentines.FuckAbbyVDay.SensualLoverPerk".Localized());

                Player.CreatePerk(PerkIds.SensualLover, 0, 0, 0, 0);
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            ReturnToCamp(2);
        }

        public void PleasureAbbyVDay()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.PleasureAbbyVDay.WillingComply".Localized());

            OutputText("Scenes.Valentines.PleasureAbbyVDay.FingersPumping".Localized());

            OutputText("Scenes.Valentines.PleasureAbbyVDay.PussyDrips".Localized());

            OutputText("Scenes.Valentines.PleasureAbbyVDay.PrettySureAbylon".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Valentines.PleasureAbbyVDay.FingersIntoHer".Localized());
            else
                if (Player.HasLongTongue())
                    OutputText("Scenes.Valentines.PleasureAbbyVDay.ShakesExcitement".Localized());
                else
                    OutputText("Scenes.Valentines.PleasureAbbyVDay.HookingFingers".Localized());

            OutputText("Scenes.Valentines.PleasureAbbyVDay.ComesDown".Localized());

            OutputText("Scenes.Valentines.PleasureAbbyVDay.BlushesWholeTime".Localized());

            OutputText("Scenes.Valentines.PleasureAbbyVDay.AllInAll".Localized());

            OutputText("Scenes.Valentines.PleasureAbbyVDay.PureLovingPerk".Localized());

            Player.CreatePerk(PerkIds.PureAndLoving, 0, 0, 0, 0);

            Player.ApplyStats(new DynStat(GameStats.Lust, 80f));

            ReturnToCamp(2);
        }

        public void GoVisitPastyVDay()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoVisitPastyVDay.LeanOverTable".Localized());

            OutputText("Scenes.Valentines.GoVisitPastyVDay.LiveningUp".Localized());

            if (Player.HasItem(StatusEffectIds.BimboChampagne) || Player.HasItem(ItemRegistry.Create(ConsumableIds.BCBeer)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.GoblinAle)) || Player.HasVagina())
            {
                ClearButtons();

                if (Player.HasItem(StatusEffectIds.BimboChampagne))
                    AddButton(0, "Common.Button.Bimbocham".Localized(), () => PastieValentineIntro(StatusEffectIds.BimboChampagne));

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BCBeer)))
                    AddButton(1, "Common.Button.BCatBeer".Localized(), () => PastieValentineIntro(ConsumableIds.BCBeer));

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.GoblinAle)))
                    AddButton(2, "Common.Button.GobAle".Localized(), () => PastieValentineIntro(ConsumableIds.GoblinAle));

                if (Player.HasVagina())
                    AddButton(3, "Common.Button.Pussy".Localized(), () => PastieValentineIntro("vag"));
            }
            else
            {
                OutputText("Scenes.Valentines.GoVisitPastyVDay.OnlyHaveWhat".Localized());

                ReturnToCamp(2);
            }
        }

        public void PastieValentineIntro(string choice = "")
        {
            ClearOutput();

            if (choice == "vag")
            {
                OutputText("Scenes.Valentines.PastieValentineIntro.CouldGetFix".Localized());

                OutputText("Scenes.Valentines.PastieValentineIntro.BackAlley".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), GoForAPushayDivePasty);
            }
            else
            {
                var itype = ItemRegistry.Create(choice);

                Player.ConsumeItem(itype, 1);

                OutputText("Scenes.Valentines.PastieValentineIntro.PresentsDrink".Localized());

                ValentineDrinkPastie(choice);
            }
        }

        public void ValentineDrinkPastie(Identifier itype)
        {
            ClearOutput();

            OutputText("Scenes.Valentines.ValentineDrinkPastie.AwayPryingEyes".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Valentines.ValentineDrinkPastie.ProbablyIsnt".Localized());
            else
                if (Player.Corruption < 75)
                    OutputText("Scenes.Valentines.ValentineDrinkPastie.AsLongFine".Localized());
                else
                    OutputText("Scenes.Valentines.ValentineDrinkPastie.MoodDrink".Localized());

            OutputText("Scenes.Valentines.ValentineDrinkPastie.GimmeGimme".Localized());

            OutputText("Scenes.Valentines.ValentineDrinkPastie.WithABurp".Localized());

            ValentineDayGetPastieDrink(itype);
        }

        public void ValentineDayGetPastieDrink(Identifier itype)
        {

            if (itype == ConsumableIds.BCBeer)
            {
                OutputText("Scenes.Valentines.ValentineDayGetPastieDrink.HandsAlongBody".Localized());

                OutputText("Scenes.Valentines.ValentineDayGetPastieDrink.ExpectedEffect".Localized());
            }
            else
                if (itype == StatusEffectIds.BimboChampagne)
                {
                    OutputText("Scenes.Valentines.ValentineDayGetPastieDrink.GigglesBody".Localized());

                    OutputText("Scenes.Valentines.ValentineDayGetPastieDrink.UsualDrink".Localized());

                    OutputText("Scenes.Valentines.ValentineDayGetPastieDrink.SluttificationSpread".Localized());
                }
                else
                    if (itype == ConsumableIds.GoblinAle)
                    {
                        OutputText("Scenes.Valentines.ValentineDayGetPastieDrink.FeelsWeird".Localized());

                        OutputText("Scenes.Valentines.ValentineDayGetPastieDrink.StillDiminutive".Localized());
                    }

            ClearButtons();

            if (Player.HasCockThatFits(8))
                AddButton(0, "Common.Button.Fuck".Localized(), FuckPastieForVDay);

            if (Player.HasCock())
                AddButton(1, "Common.Button.Rubondick".Localized(), RubPastieOnYourWangDawg);

            if (Player.HasVagina())
                AddButton(2, "Common.Button.GoInPussy".Localized(), GoForAPushayDivePasty);
        }

        public void FuckPastieForVDay()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Valentines.FuckPastieForVDay.SemiSadistic".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Valentines.FuckPastieForVDay.GrabsButt".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Valentines.FuckPastieForVDay.FruitlessPressing".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Valentines.FuckPastieForVDay.TightConfines".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Valentines.FuckPastieForVDay.TightSnatch".Localized());

            OutputText("Scenes.Valentines.FuckPastieForVDay.NotCaring".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Valentines.FuckPastieForVDay.AverageLoad".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Valentines.FuckPastieForVDay.CumshotsBig".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Valentines.FuckPastieForVDay.YellsFirstWave".Localized());
                    else
                        if (Player.CumQ() < 2000)
                            OutputText("Scenes.Valentines.FuckPastieForVDay.YellsFirstWaveAgain".Localized());
                        else
                            OutputText("Scenes.Valentines.FuckPastieForVDay.MindBreaks".Localized());

            OutputText("Scenes.Valentines.FuckPastieForVDay.GottenOffWell".Localized());

            OutputText("Scenes.Valentines.FuckPastieForVDay.GrinsLicks".Localized());

            OutputText("Scenes.Valentines.FuckPastieForVDay.OneTrackMind".Localized());

            Player.CreatePerk(PerkIds.OneTrackMind, 0, 0, 0, 0);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(2);
        }

        public void RubPastieOnYourWangDawg()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.SomeAmusement".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.PicksTheDrunk".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.BringsHand".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.GivesUp".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.PolishingMast".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.BallsLurch".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.AssClenches".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.DrenchesFaerie".Localized());
            else
                if (Player.CumQ() < 750)
                    OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.ArmDripping".Localized());
                else
                    OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.BlastsHand".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.SpunkPlastered".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.GottenYourselfOff".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.WhistlesTune".Localized());

            OutputText("Scenes.Valentines.RubPastieOnYourWangDawg.OneTrackMind".Localized());

            Player.CreatePerk(PerkIds.OneTrackMind, 0, 0, 0, 0);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(2);
        }

        public void GoForAPushayDivePasty()
        {
            ClearOutput();

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.ShedsArmor".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.HandToChest".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valentines.GoForAPushayDivePasty.TugsCock".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.AnticipatingWriggle".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.DoesntDisappoint".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valentines.GoForAPushayDivePasty.JerksOff".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.FullDive".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.ApproachingPeak".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.CanTakeNoMore".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Valentines.GoForAPushayDivePasty.DepositsLoad".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Valentines.GoForAPushayDivePasty.MuchToSay".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.FliesWall".Localized());

            OutputText("Scenes.Valentines.GoForAPushayDivePasty.NotRomantic".Localized());

            if (!Player.HasPerk(PerkIds.SensualLover))
            {
                OutputText("Scenes.Valentines.GoForAPushayDivePasty.SensualLoverPerk".Localized());

                Player.CreatePerk(PerkIds.SensualLover, 0, 0, 0, 0);
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(2);
        }

    }
}