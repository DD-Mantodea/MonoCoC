using MonoCoC.Attributes;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;

namespace MonoCoC.Contents.Scenes.Vanilla.Seasonal
{
    [Register<ThanksgivingScene>("coc:thanksgiving")]
    public class ThanksgivingScene : Scene
    {

        public override void Entry()
        {
            DatTurkeyRumpMeeting();
        }

        public void DatTurkeyRumpMeeting()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.TurkeyFuckYearDone) > 0)
            {
                TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButt();

                CoC.Instance.Flags.Set(GameFlagIds.TurkeyFuckYearDone, DateTime.Now.Year);

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.TurkeyFuckYearDone, DateTime.Now.Year);

            ShowImage("thanksgiving");

            OutputText("Scenes.Thanksgiving.DatTurkeyRumpMeeting.SitDownByFire".Localized());

            OutputText("Scenes.Thanksgiving.DatTurkeyRumpMeeting.SomethingStirs".Localized());

            DatTurkeyRumpMeetingII();
        }

        public void DatTurkeyRumpMeetingII()
        {
            ClearOutput();

            ShowImage("thanksgiving-encounter");

            OutputText("Scenes.Thanksgiving.DatTurkeyRumpMeetingII.JumpToYourFeet".Localized());

            OutputText("Scenes.Thanksgiving.DatTurkeyRumpMeetingII.AsksQuietlyBlue".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.LetApproach".Localized(), LetZeTurkeyApproach);

            AddButton(1, "Common.Button.ShooAway".Localized(), ShooTurkeyAway);
        }

        public void ShooTurkeyAway()
        {
            ClearOutput();

            ShowImage("thanksgiving-enough");

            OutputText("Scenes.Thanksgiving.ShooTurkeyAway.ScowlWaveArm".Localized());

            OutputText("Scenes.Thanksgiving.ShooTurkeyAway.RepeatsCockingHead".Localized());

            OutputText("Scenes.Thanksgiving.ShooTurkeyAway.RollingEyesYell".Localized());

            OutputText("Scenes.Thanksgiving.ShooTurkeyAway.ShakingHeadSitBack".Localized());

            if (Player.Hunger > 60)
                Player.Hunger = 70;

            Player.RefillHunger(40);

            Go<CampScene>();
        }

        public void LetZeTurkeyApproach()
        {
            ClearOutput();

            if (Player.Gender == Gender.Female || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
            {
                FemaleLetZeTurkeyGalApproach();

                return ;
            }

            ShowImage("thanksgiving-approach");

            OutputText("Scenes.Thanksgiving.LetZeTurkeyApproach.PonderousFormGirl".Localized());

            OutputText("Scenes.Thanksgiving.LetZeTurkeyApproach.RaiseEyebrow".Localized());

            OutputText("Scenes.Thanksgiving.LetZeTurkeyApproach.AsksAgainHand".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.BasteHer".Localized(), BasteThatTurkeyBooty);

            AddButton(1, "Common.Button.NoThanks".Localized(), NoThanksTurkeyGal);
        }

        public void NoThanksTurkeyGal()
        {
            ClearOutput();

            ShowImage("thanksgiving-enough");

            OutputText("Scenes.Thanksgiving.NoThanksTurkeyGal.ReluctantlyPushAway".Localized());

            ReturnToCamp(2);
        }

        public void BasteThatTurkeyBooty()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBooty.LetCockSlipFree".Localized());

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBooty.HugeTitsPopFree".Localized());

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBooty.ShovedFullyInto".Localized());

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBooty.IdeaComesToMind".Localized());

            ShowImage("thanksgiving-baste");

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBooty.GrabTitanicTits".Localized());

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBooty.MoveFasterWilling".Localized());

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBooty.CriesEcstatically".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            BasteThatTurkeyBootyII();
        }

        public void BasteThatTurkeyBootyII()
        {
            ClearOutput();

            ShowImage("thanksgiving-basteII");

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBootyII.GobbleNervous".Localized());

            OutputText("Scenes.Thanksgiving.BasteThatTurkeyBootyII.TurkeyGirlSmiles".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.StuffHer".Localized(), StuffDatTurkeyWithSpoo);

            AddButton(1, "Common.Button.ThatllDo".Localized(), ThatllDoTurkey);
        }

        public void StuffDatTurkeyWithSpoo()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.EvenThoughJustCame".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.CoosFingersSlipping".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.AnchoringFingers".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.AsksPlaintively".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.BeginMoveHips".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.SuggestsPushing".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.SurpriseSomethingComes".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.GravyQuestion".Localized());

            ShowImage("thanksgiving-stuff");

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.GagABit".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.SoftMoansEcho".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.LaughLetGo".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.LetLooseAnother".Localized());
            else
                OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.DistendsAndBulges".Localized());

            OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.SoonBetweenKneading".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFuckbuddy) == 1 && CoC.Instance.Flags.Get(GameFlagIds.HelReducedEncounterRate) == 0)
            {
                OutputText("Scenes.Thanksgiving.StuffDatTurkeyWithSpoo.LayAtopTurkey".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.SpitRoast".Localized(), SpitRoastThatTurkey);

                AddButton(1, "Common.Button.ThatsEnough".Localized(), ThatllDoTurkey);
            }
            else
                ThatllDoTurkey();
        }

        public void SpitRoastThatTurkey()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.ContemplateWhatToDo".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.CallsSharp".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.LaughsScratching".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.BeforeHelCanFinish".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.TurkeySaysMuffled".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.HelMovesToStraddle".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.MumblesLookingOver".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.WhenGivenHerAll".Localized());

            ShowImage("thanksgiving-spit-roast");

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.SquirmsAndWrithes".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.HammerTurkeyGirl".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.RoarGiveThe".Localized());

            OutputText("Scenes.Thanksgiving.SpitRoastThatTurkey.PullOutOfHer".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ThatllDoTurkey();
        }

        public void ThatllDoTurkey()
        {
            ClearOutput();

            ShowImage("thanksgiving-ending");

            OutputText("Scenes.Thanksgiving.ThatllDoTurkey.RunningHandThrough".Localized());

            OutputText("Scenes.Thanksgiving.ThatllDoTurkey.SayPattingTit".Localized());

            if (!Player.HasPerk(PerkIds.PilgrimsBounty))
            {
                Player.CreatePerk(PerkIds.PilgrimsBounty, 0, 0, 0, 0);

                OutputText("Scenes.Thanksgiving.ThatllDoTurkey.PerkPilgrimsBounty".Localized());
            }

            ReturnToCamp(2);
        }

        public void FemaleLetZeTurkeyGalApproach()
        {
            ClearOutput();

            ShowImage("thanksgiving-approach");

            OutputText("Scenes.Thanksgiving.FemaleLetZeTurkeyGalApproach.PonderousFormGirl".Localized());

            OutputText("Scenes.Thanksgiving.FemaleLetZeTurkeyGalApproach.GobbleQuestion".Localized());

            OutputText("Scenes.Thanksgiving.FemaleLetZeTurkeyGalApproach.RaiseEyebrow".Localized());

            OutputText("Scenes.Thanksgiving.FemaleLetZeTurkeyGalApproach.QuestionPunctuated".Localized());

            OutputText("Scenes.Thanksgiving.FemaleLetZeTurkeyGalApproach.FoundYourself".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Appetizer".Localized(), TurkeyAppetizer);

            AddButton(1, "Common.Button.MainCourse".Localized(), FemaleAndTurkeyMainCourse);

            AddButton(2, "Common.Button.Dessert".Localized(), HaveTurkeyGirlDesertInYourCunt);
        }

        public void TurkeyAppetizer()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.BitingBackMoan".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.ShiversAtAttentions".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.GroanIntoGirls".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.ChokedMoanRips".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.SightOfHer".Localized());

            ShowImage("thanksgiving-appetizer");

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.BreathingGrowsRagged".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.FeelingSorry".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.StayAwayFrom".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.CriesOutPenetration".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyAppetizer.WrenchYourself".Localized());

            Player.Orgasm("Generic");

            FemaleAndTurkeyMainCourse();
        }

        public void FemaleAndTurkeyMainCourse()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.FullyDisrobedRuns".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.TonguePlaysNipple".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.GravyDribbles".Localized());

            ShowImage("thanksgiving-main-course");

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.StomachBeginsSwell".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.ContinuesToDeny".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.FingersGrazes".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.HandStillEntwined".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.HandsPullTightly".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.ClimaxBegins".Localized());

            OutputText("Scenes.Thanksgiving.FemaleAndTurkeyMainCourse.UnableToWithstand".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));

            HaveTurkeyGirlDesertInYourCunt();
        }

        public void HaveTurkeyGirlDesertInYourCunt()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.BarelyAbleContain".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.SkinFeelsCool".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.FaceRedAs".Localized());

            ShowImage("thanksgiving-dessert");

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.PantingAtDisplay".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.RecoverFranticAssault".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.OfferLikeThat".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.MoansUnderStrokes".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.FingersSnakeWay".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.GirlMustBeClose".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.IntrusionPushesOver".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.OrgasmStartsFade".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.LooksDownAtYou".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.MouthNipsNeck".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.OneHourLaterBreak".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.EyesCrackOpen".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.LurchingToFeet".Localized());

            OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.HaveToAdmit".Localized());

            if (!Player.HasPerk(PerkIds.Cornucopia))
            {
                OutputText("Scenes.Thanksgiving.HaveTurkeyGirlDesertInYourCunt.PerkCornucopia".Localized());

                Player.CreatePerk(PerkIds.Cornucopia, 0, 0, 0, 0);
            }

            Player.ChangeFatigue(-100);

            Player.HealthChange(3000);

            ReturnToCamp(2);
        }

        public void TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButt()
        {
            ClearOutput();

            ShowImage("thanksgiving");

            OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButt.WanderingThroughCamp".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButt.StartToScrounge".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButt.PangOfHomesickness".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButt.ReachForSome".Localized());

            TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButtII();
        }

        public void TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButtII()
        {
            ClearOutput();

            ShowImage("thanksgiving-encounter");

            OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButtII.JumpToYourFeet".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButtII.AsksQuietlyBlue".Localized());

            AwardAchievement(AchievementIds.HolidayThanksgivingII);

            ClearButtons();

            if (Player.HasCock())
                if (Player.HasKeyItem("Deluxe Dildo") || Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.FuckDraft)))
                {
                    OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButtII.CouldLetHerCome".Localized());

                    AddButton(2, "Common.Button.HelloAgain".Localized(), HelloAgain);

                    CoC.Instance.Flags.Set(GameFlagIds.MoreTurkey, 0);
                }
                else
                {
                    OutputText("Scenes.Thanksgiving.TurkeyGirlTwoTheTurkeningBySavinWhatADickInAButtII.MightBeAbleTo".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MoreTurkey, 1);
                }

            AddButton(0, "Common.Button.ShooOff".Localized(), ShooTurkeyAway);

            AddButton(1, "Common.Button.LetHerCome".Localized(), LetZeTurkeyApproach);
        }

        public void HelloAgain()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.HelloAgain.SaySeasonalLittle".Localized());

            OutputText("Scenes.Thanksgiving.HelloAgain.SeeingNotAngry".Localized());

            OutputText("Scenes.Thanksgiving.HelloAgain.GobblesAsksCheerily".Localized());

            ShowImage("thanksgiving-hello-again");

            OutputText("Scenes.Thanksgiving.HelloAgain.AnswerPushingHead".Localized());

            if (Player.CockArea(0) <= 60)
                OutputText("Scenes.Thanksgiving.HelloAgain.AndDownToBase".Localized());
            else
                OutputText("Scenes.Thanksgiving.HelloAgain.SwallowingShaft".Localized());

            OutputText("Scenes.Thanksgiving.HelloAgain.BobsUpAndDown".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GravyBoatDatTurkturk);
        }

        public void GravyBoatDatTurkturk()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.CupTurkeyChin".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.TeaseReachingBack".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.YelpsGentleCaress".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.LetHerEnjoyIt".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.AsksAgainSilenced".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.SmellAlmostOverwhelming".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.DeclaresTriumphantly".Localized());

            if (Player.Cocks[0].Thickness >= 5)
                OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.StomachBulgingObscenely".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.SentencePeriod".Localized());

            ShowImage("thanksgiving-hello-again");

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.LaughPlantingArms".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.SlowlyStartsMove".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.AsksMidBounce".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.OpenEyeIncredulously".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.GrabTurkeyHead".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.GobblesCries".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.GobbleExclaim".Localized());

            OutputText("Scenes.Thanksgiving.GravyBoatDatTurkturk.LooksLikeWantsFavor".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SeasonHerDatTurkeyBitch);
        }

        public void SeasonHerDatTurkeyBitch()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.PlantHandsOn".Localized());

            ShowImage("thanksgiving-season");

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.CriesDigitsBrush".Localized());

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.TurkeyWithout".Localized());

            if (Player.HasKeyItem("Deluxe Dildo"))
                OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.TamanisDildo".Localized());
            else
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                    OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.BottleLustDraft".Localized());
                else
                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.FuckDraft)))
                        OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.BottleFuckDraft".Localized());
                    else
                        OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.FuckinError".Localized());

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.PullItOut".Localized());

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.GobblesGiddily".Localized());

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.InstantlyHerPale".Localized());

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.CommandPokingFinger".Localized());

            OutputText("Scenes.Thanksgiving.SeasonHerDatTurkeyBitch.AnswersRollingOff".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), TurkeyDesertBitches);
        }

        public void TurkeyDesertBitches()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.WhinesAtMoment".Localized());

            ShowImage("thanksgiving-dessert-bitches");

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.GroanVenomDrips".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.CuppingATit".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.VenomTakesControl".Localized());

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.WithBothCocks".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.WithEveryCock".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.ShockingHerSoMuch".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralS".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.MilkingHardAs".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSSecond".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.DrenchingYourIn".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.SquawkingTurkeySeems".Localized());

            if (Player.CockArea(0) >= 50)
            {
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.StomachBulgingObscenely".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.AndAssWord".Localized());
            }
            else
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.HipsSlappingInto".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.CriesOneWord".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSThird".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.ThrustingYourCock".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSFourth".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.DeepInsideAvian".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSFifth".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.CumsAndCumsPleading".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSSixth".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.InAndOutSlick".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.FeatheredArmsReach".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.AndAssSpaceWord".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.LegsSpurringOn".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSSeventh".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.EveryTimeYouPierce".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSEighth".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.SoExpertlyThat".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.DrawNearerBreaking".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSNinth".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.DeepIntoGobbles".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.AndAssTrailingWord".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.SquirmAroundCock".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSTenth".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.WringingCumFrom".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.AndBowelsWord".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.BackArchesTeats".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.GobblesSighsHappily".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSDeposit".Localized());
            else
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.DepositsLastOfIts".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.CreamIntoFully".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSEleventh".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.WiltingInsideHer".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.AssSpaceWordAgain".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.BeginWord".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.PluralSTwelfth".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.DroolSpunkPooling".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.AgreeWrappingGiddy".Localized());

            OutputText("Scenes.Thanksgiving.TurkeyDesertBitches.SayPattingTit".Localized());

            Player.Orgasm("Dick");

            ReturnToCamp(2);
        }

        public bool PigSlutAllowed()
        {

            if (!Holidays.IsThanksgiving())
                return false;

            int check = CoC.Instance.Flags.Get(GameFlagIds.PigSlutDisabled);

            if (check == -1)
                return false;

            if (Utils.Utils.IsBetween(0, check, View.Time.Days))
                return false;

            if (Utils.Utils.IsBetween(0, check, -DateTime.Now.Year) && !Holidays.IsThanksgiving(true))
                return false;

            return true;
        }

        public void PigSlutRoastingGreet()
        {
            ClearOutput();

            ShowImage("location-teladre-thewetbitch");

            OutputText("Scenes.Thanksgiving.PigSlutRoastingGreet.EnterWetBitch".Localized());

            OutputText("Scenes.Thanksgiving.PigSlutRoastingGreet.ExclaimsCheery".Localized(
                Player.Mf("handsome", "cutie")
            ));

            OutputText("Scenes.Thanksgiving.PigSlutRoastingGreet.MotionsToSparkly".Localized());

            OutputText("Scenes.Thanksgiving.PigSlutRoastingGreet.MovesInCloser".Localized());

            AwardAchievement(AchievementIds.HolidayThanksgivingI);

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            ClearButtons();

            AddButton(0, "Common.Button.ThrowGem".Localized(), GetARoastPiggueOinkOinkOinkMotherfucker);

            AddButton(1, "Common.Button.Nah".Localized(), NotNowPigSlut);

            AddButton(2, "Scenes.Faerie.SexMenu.ButtonNever".Localized(), DisablePigSlut);
        }

        public void NotNowPigSlut()
        {

            CoC.Instance.Flags.Set(GameFlagIds.PigSlutDisabled, View.Time.Days);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void DisablePigSlut()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.DisablePigSlut.PigSlutsNotYour".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PigSlutDisabled, -1);

            SceneInstance<TelAdre>.Get().BarTelAdre();
        }

        public void GetARoastPiggueOinkOinkOinkMotherfucker()
        {
            ClearOutput();

            Player.Gems -= 1;

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.ApproachGooeyBucket".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.WanderAroundBit".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.ComeUponDoor".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.PlayfulCalligraphy".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.OpenDoorSmell".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.DroolsPussyJuice".Localized());
            else
                OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.YWord".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.NostrilsFlareGreat".Localized());

            ShowImage("thanksgiving-pigslut");

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.ThenSeeHerJiggly".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.OinkingCutely".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.FemspunkDribbles".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.BringsThemTo".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.FinallyOpensEyes".Localized());

            OutputText("Scenes.Thanksgiving.GetARoastPiggueOinkOinkOinkMotherfucker.PlumpWhoreEyes".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MorePigRoast);
        }

        public void MorePigRoast()
        {
            ClearOutput();

            ShowImage("thanksgiving-pigslut-more");

            OutputText("Scenes.Thanksgiving.MorePigRoast.ClimbOnTop".Localized());

            OutputText("Scenes.Thanksgiving.MorePigRoast.AsksEagerlyMore".Localized());

            AddButton(0, "Common.Button.Next".Localized(), TenderizeDatPigSloot);
        }

        public void TenderizeDatPigSloot()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.SayRatherPlainly".Localized());

            ShowImage("thanksgiving-pigslut-tenderize");

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.DoesAsCommand".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.PorkRoastExclaims".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.AfflictedAreas".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.MoansLustily".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.RapidFireSpank".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.BringTailFrom".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.NoticePigslutMoving".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.HandsAreBlur".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeDatPigSloot.LookAroundRoom".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ClearButtons();

            AddButton(0, "Common.Button.Softcore".Localized(), HamRoastTenderizeHerSoftcore);

            AddButton(1, "Common.Button.Hardcore".Localized(), TenderizeHerHardcore);
        }

        public void TenderizeHerHardcore()
        {
            ClearOutput();

            ShowImage("thanksgiving-pigslut-tenderize-hardcore");

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.CantHelpMaliciously".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.ReturnToWhore".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.GrabOneWeakened".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.MoansWeakly".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.CrackWhipOn".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.LetsOutHigh".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.PatheticSquealing".Localized());

            OutputText("Scenes.Thanksgiving.TenderizeHerHardcore.LoseYourselfExcitement".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpankEpilogueColonYoureAHorriblePersonForAbusingADumbPigGirl);
        }

        public void HamRoastTenderizeHerSoftcore()
        {
            ClearOutput();

            ShowImage("thanksgiving-pigslut-tenderize-softcore");

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.CantHelpLet".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.ReturnToWhore".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.MoveBackDown".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.WhinesInSurprise".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.SeePussyPractically".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.OinksSquealsMercy".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.PainSlutBucks".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.PatheticSquealing".Localized());

            OutputText("Scenes.Thanksgiving.HamRoastTenderizeHerSoftcore.LoseYourselfExcitement".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpankEpilogueColonYoureAHorriblePersonForAbusingADumbPigGirl);
        }

        public void SpankEpilogueColonYoureAHorriblePersonForAbusingADumbPigGirl()
        {
            ClearOutput();

            OutputText("Scenes.Thanksgiving.SpankEpilogueColonYoureAHorriblePersonForAbusingADumbPigGirl.StopCeaselessRomping".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueColonYoureAHorriblePersonForAbusingADumbPigGirl.GrabHandyBottle".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueColonYoureAHorriblePersonForAbusingADumbPigGirl.SureYouve".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueColonYoureAHorriblePersonForAbusingADumbPigGirl.SlutSighsAppreciatively".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SpankEpilogueForRizzles);
        }

        public void SpankEpilogueForRizzles()
        {
            ClearOutput();

            ShowImage("thanksgiving-pigslut-spank-epilogue");

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.LookAroundRoomOnce".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.RoughlyFlipPigslut".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.FirstThingsFirst".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.DominantlySpreadLegs".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.HappilyExclaimsMonster".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.BringItDown".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.EasilySinksIn".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.VisibleBulgeAppears".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.LoveTunnel".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.NowThatTaking".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.LickingTongueParts".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.TasteRemindsYou".Localized());

            OutputText("Scenes.Thanksgiving.SpankEpilogueForRizzles.TongueHerBuzzer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GetDoneEatingOutPigSlut);
        }

        public void GetDoneEatingOutPigSlut()
        {
            ClearOutput();

            ShowImage("thanksgiving-pigslut-ending");

            OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.RemoveYourself".Localized());

            OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.PattingOwnStomach".Localized());

            OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.WhoahThatWasStrange".Localized());

            var changed = false;

            if (Player.Thickness < 100)
            {
                OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.NoticeSeemA".Localized());

                if (Player.Thickness <= 10)
                    Player.Thickness += 10;

                if (Player.Thickness <= 20)
                    Player.Thickness += 10;

                if (Player.Thickness <= 30)
                    Player.Thickness += 10;

                if (Player.Thickness <= 40)
                    Player.Thickness += 10;

                if (Player.Thickness < 50)
                    Player.Thickness += 5;

                changed = true;
            }

            if (Player.Body.Butt < 35)
            {
                OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.AssFillingOut".Localized());

                if (Player.Body.Butt < 4)
                    Player.Body.Butt += 3;

                if (Player.Body.Butt <= 10)
                    Player.Body.Butt += 2;

                if (Player.Body.Butt <= 15)
                    Player.Body.Butt += 1;

                if (Player.Body.Butt <= 20)
                    Player.Body.Butt += 1;

                if (Player.Body.Butt <= 35)
                    Player.Body.Butt += 1;

                changed = true;
            }

            if (Player.Body.Hips < 35)
            {
                OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.HipsGrowToo".Localized());

                if (Player.Body.Hips < 4)
                    Player.Body.Hips += 3;

                if (Player.Body.Hips < 10)
                    Player.Body.Hips += 2;

                if (Player.Body.Hips < 15)
                    Player.Body.Hips += 1;

                if (Player.Body.Hips < 20)
                    Player.Body.Hips += 1;

                if (Player.Body.Hips < 35)
                    Player.Body.Hips += 1;

                changed = true;
            }

            if (Player.BiggestBreastSize() < 20)
            {
                OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.BoobsAlsoGrow".Localized());

                if (Player.BiggestBreastSize() < 3)
                    Player.GrowTits(3, Player.BreastRows.Count, false, 1);

                if (Player.BiggestBreastSize() < 6)
                    Player.GrowTits(2, Player.BreastRows.Count, false, 1);

                if (Player.BiggestBreastSize() < 12)
                    Player.GrowTits(2, Player.BreastRows.Count, false, 1);

                if (Player.BiggestBreastSize() < 20)
                    Player.GrowTits(2, Player.BreastRows.Count, false, 1);

                changed = true;
            }

            OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.YouSigh".Localized());

            if (changed)
                OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.FeelingBodyExpand".Localized());

            OutputText("Scenes.Thanksgiving.GetDoneEatingOutPigSlut.OnlyOneGem".Localized());

            if (!Holidays.IsThanksgiving(true))
                CoC.Instance.Flags.Set(GameFlagIds.PigSlutDisabled, -DateTime.Now.Year);

            ReturnToCamp(2);
        }

    }
}