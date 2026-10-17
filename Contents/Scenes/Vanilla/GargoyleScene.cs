using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<GargoyleScene>("coc:gargoyle")]
    public class GargoyleScene : Scene
    {

        public override void Entry()
        {
            GarLoli();
        }

        public bool GarLoli()
        {

            return OptionStore.Instance.Get("gargoyleChild", 0) > 0 && false;
        }

        public int GargoyleConfidence(int arg = 0)
        {

            if (arg != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GarConfidence, CoC.Instance.Flags.Get(GameFlagIds.GarConfidence) + arg);

                if (CoC.Instance.Flags.Get(GameFlagIds.GarConfidence) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.GarConfidence, 100);
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.GarConfidence) < 0)
                        CoC.Instance.Flags.Set(GameFlagIds.GarConfidence, 0);
            }

            return CoC.Instance.Flags.Get(GameFlagIds.GarConfidence);
        }

        public void GargoyleSprite()
        {

        }

        public void GargoylesTheShowNowOnWBNetwork()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GargoylesTheShowNowOnWBNetwork.SetOffInSearch".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesExploredDesert) > 0)
                OutputText("Scenes.Gargoyle.GargoylesTheShowNowOnWBNetwork.EvenDesertSeemsHealthy".Localized());

            OutputText("Scenes.Gargoyle.GargoylesTheShowNowOnWBNetwork.TripSoonBeginsSeem".Localized());

            OutputText("Scenes.Gargoyle.GargoylesTheShowNowOnWBNetwork.CatchFaintestGlimmer".Localized());

            if (SceneInstance<TelAdre>.Get().IsAllowedInto())
                OutputText("Scenes.Gargoyle.GargoylesTheShowNowOnWBNetwork.RuinsOfOneOf".Localized());

            OutputText("Scenes.Gargoyle.GargoylesTheShowNowOnWBNetwork.EvenSomeDemonEnclave".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), GargoyleMeeting2);

            View.Time.PassMinutes((int)Math.Round((double)((1) * 60)));
        }

        public void GargoyleMeeting2()
        {
            ClearOutput();

            GargoyleSprite();

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.FinallyCloseDistance".Localized());

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.ArriveAtGrounds".Localized());

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.QuiteDarkInside".Localized());

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.WhatCatchesYourEye".Localized());

            ShowImage("gargoyle-cathedral");

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.AllButOneIn".Localized());

            if (Player.Libido >= 40)
                OutputText("Scenes.Gargoyle.GargoyleMeeting2.CouldTakeThisWith".Localized());

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.AttentionSoonDrawn".Localized());

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.BreakBondsToTame".Localized());

            OutputText("Scenes.Gargoyle.GargoyleMeeting2.SupposeCouldBreakChains".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FoundCathedral) == 0)
                OutputText("Scenes.Gargoyle.GargoyleMeeting2.CathedralAddedToPlaces".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FoundCathedral, 1);

            ClearButtons();

            AddButton(0, "Common.Button.BreakChains".Localized(), BreakZeChains);

            AddButton(1, "Common.Button.DontBreak".Localized(), DontBreakThatShit);
        }

        public void DontBreakThatShit()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.DontBreakThatShit.NoNotDoingIt".Localized());

            OutputText("Scenes.Gargoyle.DontBreakThatShit.IfChangeYourMind".Localized());

            ReturnToCamp(1);
        }

        public void BreakZeChains()
        {
            ClearOutput();

            ShowImage("gargoyle-cathedral");

            OutputText("Scenes.Gargoyle.BreakZeChains.SwingYourUpOver".Localized());

            if (Player.Weapon.IsUnarmed())
                OutputText("Scenes.Gargoyle.BreakZeChains.OwDammitThatHurt".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.EarSplittingISching".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.GargoyleSteppedDown".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.MasterMyMaster".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.GlanceOverShoulder".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.AsksLookingUp".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.CautiousAskHerWhat".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.SeemsToPause".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.RaisedMoreQuestions".Localized());

            OutputText("Scenes.Gargoyle.BreakZeChains.ISimplyAm".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), NameZeGargoyle);
        }

        public void NameZeGargoyle()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.NameZeGargoyle.WhatNameWillGive".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SubmitName);

        }

        public void SubmitName()
        {
            ClearOutput();

            var input = "" ;

            if (input == "")
            {
                OutputText("Scenes.Gargoyle.SubmitName.YouMustNameHer".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), SubmitName);

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.GarName, input);

            OutputText("Scenes.Gargoyle.SubmitName.MasterHasInterestingTaste".Localized());

            OutputText("Scenes.Gargoyle.SubmitName.ContinuesToKneel".Localized());

            GargoyleStarterMenu();
        }

        public void GargoyleStarterMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.FunnyOrder".Localized(), GiveGargoyleAFunnyOrder);

            AddButton(1, "Common.Button.CarnalOrder".Localized(), CarnalOrder);

            AddButton(2, "Common.Button.Talk".Localized(), FirstGargoyleTalk);
        }

        public void GiveGargoyleAFunnyOrder()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GiveGargoyleAFunnyOrder.IfShesSoIntent".Localized());

            OutputText("Scenes.Gargoyle.GiveGargoyleAFunnyOrder.HesitatesOnlyFor".Localized());

            GargoyleStarterMenu();
        }

        public void CarnalOrder()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.CarnalOrder.WellWellWillingFollow".Localized());

            OutputText("Scenes.Gargoyle.CarnalOrder.QuietlyAlmostColdly".Localized());

            if (Player.Corruption < 40)
                OutputText("Scenes.Gargoyle.CarnalOrder.FlusteredWord".Localized());
            else
                OutputText("Scenes.Gargoyle.CarnalOrder.ExcitedWord".Localized());

            OutputText("Scenes.Gargoyle.CarnalOrder.HeartbeatWatchesYou".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            GargoyleStarterMenu();
        }

        public void FirstGargoyleTalk()
        {
            ClearOutput();

            ShowImage("gargoyle-cathedral");

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.DecideAgainstOrder".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.FinallyAskGlancing".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.FoundToyGrewTired".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.YourLastMaster".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.ThatIsCorrect".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.WhoWasShe".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.SheWasMaster".Localized());

            if (silly)
                OutputText("Scenes.Gargoyle.FirstGargoyleTalk.MasterIsMasculine".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.SighApparentlyDoesnt".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.BreaksStareAndLooks".Localized());

            OutputText("Scenes.Gargoyle.FirstGargoyleTalk.EventsThatLed".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Berate".Localized(), BerateTheGargoyle);

            AddButton(1, "Common.Button.Reassure".Localized(), ReassureTheGargoyle);
        }

        public void BerateTheGargoyle()
        {
            ClearOutput();

            GargoyleConfidence(-10);

            ShowImage("gargoyle-cathedral");

            OutputText("Scenes.Gargoyle.BerateTheGargoyle.PillarOfRageBoils".Localized());

            OutputText("Scenes.Gargoyle.BerateTheGargoyle.RecoilsFlinching".Localized());

            OutputText("Scenes.Gargoyle.BerateTheGargoyle.StormOutOfCathedral".Localized());

            ReturnToCamp(1);
        }

        public void ReassureTheGargoyle()
        {
            ClearOutput();

            GargoyleConfidence(10);

            ShowImage("gargoyle-cathedral");

            OutputText("Scenes.Gargoyle.ReassureTheGargoyle.CantHelpButFeel".Localized());

            OutputText("Scenes.Gargoyle.ReassureTheGargoyle.LooksUpHerRuby".Localized());

            OutputText("Scenes.Gargoyle.ReassureTheGargoyle.SaysAgainLookingAway".Localized());

            OutputText("Scenes.Gargoyle.ReassureTheGargoyle.SlowlyNodAndAsk".Localized());

            OutputText("Scenes.Gargoyle.ReassureTheGargoyle.RustleHairAndAssure".Localized());

            ReturnToCamp(1);
        }

        public void ReturnToCathedral(bool woken = false)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FoundCathedral, 1);

            GargoyleSprite();

            RegisterMacro("gargconfidence", GargoyleConfidence());

            RegisterMacro("garloli", GarLoli());

            if (!woken)
                if (Random.Shared.Next(3) == 0)
                    OutputText("Scenes.Gargoyle.ReturnToCathedral.MakeYourWayBack".Localized());
                else
                    if (Random.Shared.Next(2) == 0)
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.SpotTheCathedral".Localized());
                    else
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.WanderIntoGrounds".Localized());

            ClearButtons();

            if (View.Time.Hours <= 9 || View.Time.Hours >= 18 || woken)
            {
                if (!woken)
                {
                    ShowImage("gargoyle-cathedral");

                    OutputText("Scenes.Gargoyle.ReturnToCathedral.InTheDim".Localized());

                    if (View.Time.Hours <= 9)
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.EarlyMorning".Localized());
                    else
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.LateEvening".Localized());

                    OutputText("Scenes.Gargoyle.ReturnToCathedral.LightStreamingIn".Localized());

                    if (GargoyleConfidence() < 70)
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.KneelsHeadBowedLow".Localized());
                    else
                        if (GargoyleConfidence() < 99)
                            OutputText("Scenes.Gargoyle.ReturnToCathedral.BowsLowASmall".Localized());
                        else
                            OutputText("Scenes.Gargoyle.ReturnToCathedral.YouAndLaysSmall".Localized());

                    OutputText("Scenes.Gargoyle.ReturnToCathedral.SaysWord".Localized());

                    if (GargoyleConfidence() > 50)
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.HappilyWord".Localized());
                    else
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.SolemnlyWord".Localized());

                    OutputText("Scenes.Gargoyle.ReturnToCathedral.WhatWouldYouHave".Localized());
                }

                AddButton(0, "Common.Button.Appearance".Localized(), GargoyleAppearance);

                if (CoC.Instance.Flags.Get(GameFlagIds.GarHistory) < 3 || CoC.Instance.Flags.Get(GameFlagIds.GarCathedral) < 3 || CoC.Instance.Flags.Get(GameFlagIds.GarNameTalks) < 3 || (GargoyleConfidence() > 99 && CoC.Instance.Flags.Get(GameFlagIds.GarLoverTalks) < 1))
                    AddButton(1, "Common.Button.Talk".Localized(), () => TalkToGargoyle());
                else
                    AddButton(1, "Common.Button.Talk".Localized(), null);

                if (Player.Lust >= 33)
                    AddButton(2, "Common.Button.Sex".Localized(), GargoyleSexMenu);
                else
                    AddButton(2, "Common.Button.Sex".Localized(), null);

                AddButton(5, "Common.Button.Rituals".Localized(), RitualGargoyle);

                if (GargoyleConfidence() >= 70 && CoC.Instance.Flags.Get(GameFlagIds.RitualIntro) > 0)
                    AddButton(6, "Common.Button.KinkyRituals".Localized(), GargoyleKinkyRituals);
                else
                    AddButton(6, "Common.Button.KinkyRituals".Localized(), null);

                AddRowButton(1, "Wash", Wash);
            }
            else
            {
                if (!woken)
                {
                    OutputText("Scenes.Gargoyle.ReturnToCathedral.WithinTheRuins".Localized());

                    if (Player.Lust >= 33)
                        OutputText("Scenes.Gargoyle.ReturnToCathedral.CouldUseHerTo".Localized());

                    OutputText("Scenes.Gargoyle.ReturnToCathedral.SentencePeriod".Localized());
                }

                AddButton(0, "Common.Button.WakeHer".Localized(), () => ReturnToCathedral(true));

                if (Player.Lust >= 33)
                {
                    if (Player.HasCock())
                        AddButton(1, "Common.Button.UseHer".Localized(), UseGargoyleMaleHerm);
                    else
                        if (Player.HasVagina())
                            AddButton(1, "Common.Button.UseHer".Localized(), UseHerGargoyleFemale);
                        else
                            AddButton(1, "Common.Button.UseHer".Localized(), null);
                }
                else
                    AddButton(1, "Common.Button.UseHer".Localized(), null);
            }

            SetExitButton("Common.Button.Back".Localized());
        }

        public void GargoyleSexMenu()
        {
            ClearOutput();

            ShowImage("gargoyle-cathedral");

            OutputText("Scenes.Gargoyle.GargoyleSexMenu.ApproachAndTell".Localized());

            if (GargoyleConfidence() <= 69)
                OutputText("Scenes.Gargoyle.GargoyleSexMenu.GargoyleBowsLow".Localized());
            else
                OutputText("Scenes.Gargoyle.GargoyleSexMenu.RubyRedEyesLock".Localized());

            ClearButtons();

            if (Player.HasCock())
            {
                AddButton(0, "Common.Button.Vaginal".Localized(), GargoyleCoochiiGetsPlowed);

                AddButton(1, "Common.Button.Anal".Localized(), GargoyleAnal);

                AddButton(2, "Common.Text.Titfuck".Localized() + (GarLoli() ? "?" : ""), TitFuckTheGargoyle);

                if (OptionStore.Instance.Get("watersports", 0) > 0)
                    AddButton(5, "Common.Button.Watersports".Localized(), GargoyleWatersports);
            }
            else
            {
                AddButton(0, "Common.Button.Vaginal".Localized(), null);

                AddButton(1, "Common.Button.Anal".Localized(), null);

                AddButton(2, "Common.Button.Titfuck".Localized(), null);
            }

            AddButton(3, "Common.Button.StrapOn".Localized(), StrapOnGargoyle);

            if (Player.HasVagina())
                AddButton(4, "Common.Button.TailFuck".Localized(), TailFuckGargoyleScene);
            else
                AddButton(4, "Common.Button.TailFuck".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), () => ReturnToCathedral());
        }

        public void GargoyleWatersports()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GargoyleWatersports.DrawStatuesqueLover".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.OneByOneArticles".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.BreathesSheRepeats".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.PussyTightAndFamiliar".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.StammersGrowingInsensate".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.NothingLastsForever".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.MoansWithEachExhale".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.CockSofteningButStill".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.StillFeelAnotherUrge".Localized());

            OutputText("Scenes.Gargoyle.GargoyleWatersports.LastOfYourStream".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), WatersportsLeave);

            AddButton(1, "Common.Button.StayPlay".Localized(), WatersportsStay);
        }

        public void WatersportsLeave()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.WatersportsLeave.RedressAndGatherGear".Localized());

            ReturnToCamp(1);
        }

        public void WatersportsStay()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.WatersportsStay.AsLiftAFoot".Localized());

            OutputText("Scenes.Gargoyle.WatersportsStay.EaseDownHerBack".Localized());

            OutputText("Scenes.Gargoyle.WatersportsStay.ThreeMoreMouthfuls".Localized());

            OutputText("Scenes.Gargoyle.WatersportsStay.AwakenLaterAndRemove".Localized());

            ReturnToCamp(1);
        }

        public void GargoyleAnal()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GargoyleAnal.TellGetOnAll".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAnal.SharpLittleGasp".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAnal.GottenHerMore".Localized());

            if (Player.Cocks[Player.BiggestCockIndex()].Length >= (GarLoli() ? 30 : 50))
            {
                OutputText("Scenes.Gargoyle.GargoyleAnal.KnowingLivingStatue".Localized());

                OutputText("Scenes.Gargoyle.GargoyleAnal.GustoHammeringAway".Localized());

                OutputText("Scenes.Gargoyle.GargoyleAnal.FuckingStraightThrough".Localized());

                OutputText("Scenes.Gargoyle.GargoyleAnal.NowSatedPutHands".Localized());
            }
            else
            {
                OutputText("Scenes.Gargoyle.GargoyleAnal.SpendFewMinutesMaking".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Gargoyle.GargoyleAnal.BallsSlappingHerButt".Localized());

                OutputText("Scenes.Gargoyle.GargoyleAnal.PantsChestHeaving".Localized());

                OutputText("Scenes.Gargoyle.GargoyleAnal.SoonMoaningAgain".Localized());

                OutputText("Scenes.Gargoyle.GargoyleAnal.UtterlySatedPushOff".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void GargoyleCoochiiGetsPlowed()
        {
            ClearOutput();

            var x = Player.CockThatFits(GarLoli() ? 20 : 60);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.GentlyPushOntoBack".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.RewardedForEfforts".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.PullYourFromYour".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.MoreAndMoreDickmeat".Localized());

            if (Player.CockArea(x) > (GarLoli() ? 20 : 60))
                OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.AsFarCanTake".Localized());
            else
                OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.UpToTheHilt".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.NowYoureSoDeep".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.StartToThrust".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.ToYourDismay".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.TwoLayThere".Localized(
                Player.CockDescript(x)
            ));

            if (GargoyleConfidence() >= 70)
                OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.BeforeYouCanStand".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.SaysRunningHandThrough".Localized());

            OutputText("Scenes.Gargoyle.GargoyleCoochiiGetsPlowed.GiveHerAPat".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void TitFuckTheGargoyle()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.SituateOnAPew".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.OnceSatisfiedAtYour".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.LittleEncouragementStarts".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.OverwhelmingWetness".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.NoReasonHoldBack".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.GrabPushHerDown".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.ByTimeYoureDone".Localized());

            if (GargoyleConfidence() >= 70)
                OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.SurpriseSoDoesShe".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.SentencePeriod".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.AsksInnocently".Localized());

            OutputText("Scenes.Gargoyle.TitFuckTheGargoyle.GiveLittlePat".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void StrapOnGargoyle()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.StrapOnGargoyle.QuickLookAroundThe".Localized());

            OutputText("Scenes.Gargoyle.StrapOnGargoyle.AsksConfusedYouRoll".Localized());

            OutputText("Scenes.Gargoyle.StrapOnGargoyle.LaysDownYouAtop".Localized());

            OutputText("Scenes.Gargoyle.StrapOnGargoyle.TeaseAndSheFreezes".Localized());

            if (!Player.IsTaur() && !Player.IsGoo() && !Player.IsDrider() && !Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Gargoyle.StrapOnGargoyle.RevealYourLegFirst".Localized());
            else
                OutputText("Scenes.Gargoyle.StrapOnGargoyle.GiverGoodShow".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Gargoyle.StrapOnGargoyle.PantingWithLustBy".Localized());

                OutputText("Scenes.Gargoyle.StrapOnGargoyle.WithoutFurtherAdo".Localized());

                Player.CuntChange(15, true, true, false);

                OutputText("Scenes.Gargoyle.StrapOnGargoyle.SoonBothMoaning".Localized());

                OutputText("Scenes.Gargoyle.StrapOnGargoyle.ByTimeShesDone".Localized());
            }
            else
            {
                OutputText("Scenes.Gargoyle.StrapOnGargoyle.PantingWithLustAgain".Localized());

                OutputText("Scenes.Gargoyle.StrapOnGargoyle.WithoutFurtherAdoAgain".Localized());

                Player.ButtChange(15, true, true, false);

                OutputText("Scenes.Gargoyle.StrapOnGargoyle.SoonBothMoaningAgain".Localized());

                OutputText("Scenes.Gargoyle.StrapOnGargoyle.StillPantingFromOrdeal".Localized());
            }

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void TailFuckGargoyleScene()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.WithoutWarningGrabTail".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.LittleBalancingHelp".Localized());

            Player.CuntChange(15, true, true, true);

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.ShudderAsYouBottom".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.FirmlyInsideDepths".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.JustAsYouFeel".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.BeforeCanGetUsed".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.WhenFinishedCarefully".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.WasMasterSatisfied".Localized());

            if (GargoyleConfidence() > 70)
                OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.HintOfASmile".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.SentencePeriod".Localized());

            OutputText("Scenes.Gargoyle.TailFuckGargoyleScene.RubHeadAndTell".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void UseGargoyleMaleHerm()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.UseGargoyleMaleHerm.ApproachMotionlessStatue".Localized());

            OutputText("Scenes.Gargoyle.UseGargoyleMaleHerm.SurpriseGargoyleDoes".Localized());

            OutputText("Scenes.Gargoyle.UseGargoyleMaleHerm.ArentQuiteSatisfied".Localized());

            OutputText("Scenes.Gargoyle.UseGargoyleMaleHerm.ReturnToThrusting".Localized());

            OutputText("Scenes.Gargoyle.UseGargoyleMaleHerm.GaspsFullBodyShudder".Localized());

            OutputText("Scenes.Gargoyle.UseGargoyleMaleHerm.GiveGargoyleALittle".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void UseHerGargoyleFemale()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.UseHerGargoyleFemale.CasuallyStripOut".Localized());

            OutputText("Scenes.Gargoyle.UseHerGargoyleFemale.HarshSqueezeHeavy".Localized());

            Player.CuntChange(15, true, true, true);

            OutputText("Scenes.Gargoyle.UseHerGargoyleFemale.InsensateRemainsLow".Localized());

            OutputText("Scenes.Gargoyle.UseHerGargoyleFemale.PleasedToNoteThat".Localized());

            OutputText("Scenes.Gargoyle.UseHerGargoyleFemale.ContentedSpendMinutes".Localized());

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void RitualGargoyle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.RitualIntro) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.RitualIntro, 1);

                OutputText("Scenes.Gargoyle.RitualGargoyle.NoteSanctuarys".Localized());

                OutputText("Scenes.Gargoyle.RitualGargoyle.LongBeforeMasterCame".Localized());

                if (GargoyleConfidence() < 50)
                    OutputText("Scenes.Gargoyle.RitualGargoyle.ForgiveMyImpertinence".Localized());

                OutputText("Scenes.Gargoyle.RitualGargoyle.ThoughtPerhapsMaster".Localized());
            }
            else
                OutputText("Scenes.Gargoyle.RitualGargoyle.WalkToTheChurchs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Body".Localized(), BodyRitual);

            AddButton(1, "Common.Button.Mind".Localized(), MindGargoyleRitual);

            if (Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                AddButton(2, "Common.Button.Banish".Localized(), BanishmentGargoyleRitual);
            else
                AddButton(2, "Common.Button.Banish".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), () => ReturnToCathedral());
        }

        public void BodyRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.BodyRitual.TellCouldStand".Localized());

            OutputText("Scenes.Gargoyle.BodyRitual.SaysSoQuietly".Localized());

            OutputText("Scenes.Gargoyle.BodyRitual.RaggedPainedBreathing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -.5f), new DynStat(GameStats.Sensitivity, -2f));

            Player.HealthChange(-(float)(Player.MaxHealth / 2));

            ClearButtons();

            AddButton(0, "Common.Button.Nothing".Localized(), NoReactionToBodyRitual);

            AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), GargoyleCuddleAfterBodyRitual);

            AddButton(2, "Common.Button.Revenge".Localized(), GargoyleRevengeAfterBodyRitual);
        }

        public void NoReactionToBodyRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.NoReactionToBodyRitual.SimplyStillAndTry".Localized());

            ReturnToCamp(1);
        }

        public void GargoyleRevengeAfterBodyRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GargoyleRevengeAfterBodyRitual.StandingOverGloating".Localized());

            GargoyleConfidence(-5);

            ReturnToCamp(1);
        }

        public void GargoyleCuddleAfterBodyRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GargoyleCuddleAfterBodyRitual.DespiteThePain".Localized());

            GargoyleConfidence(5);

            ReturnToCamp(1);
        }

        public void MindGargoyleRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.MindGargoyleRitual.TellYouveBeen".Localized());

            OutputText("Scenes.Gargoyle.MindGargoyleRitual.WhoSeeksPurity".Localized());

            OutputText("Scenes.Gargoyle.MindGargoyleRitual.InvocationQuietClose".Localized());

            OutputText("Scenes.Gargoyle.MindGargoyleRitual.DoSoTakingLong".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -10f), new DynStat(GameStats.Corruption, -.5f));

            Player.ChangeFatigue(50);

            ReturnToCamp(1);
        }

        public void BanishmentGargoyleRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.TalkingAboutRituals".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.AHitchhiker".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.SomeHitchhikers".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.InYourTravels".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.ThisBastard".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.TheseBastards".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.OutOfYourSystem".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.BeneathAltarWithdraws".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.TryBestToRelax".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.SaysWord".Localized());

            if (GargoyleConfidence() >= 50)
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.SoothinglyFreeHand".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.PainIsNecessary".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.StillCantHelpBut".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.BoomingFromWithin".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.GratifyinglyYouCanFeel".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.HolyRodJammedUp".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.CreatureWithinHas".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.CreaturesWithinHave".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.AbsorbedAndPurged".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.PossessedWord".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.InfestedWord".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.AgainWord".Localized());

            OutputText("Scenes.Gargoyle.BanishmentGargoyleRitual.StillPantingAndGasping".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                Player.RemoveStatusEffect(StatusEffectIds.Exgartuan);
            else
                Player.RemoveStatusEffect(StatusEffectIds.Infested);

            Player.HealthChange(-(float)(Player.MaxHealth / 3));

            Player.ChangeFatigue(10);

            AddButton(0, "Common.Button.Nothing".Localized(), DontFreakOutAfterBanishment);

            AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonCuddle".Localized(), CuddleForBanishments);

            AddButton(2, "Common.Button.Revenge".Localized(), GetRevengeForBanishments);
        }

        public void DontFreakOutAfterBanishment()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.DontFreakOutAfterBanishment.SimplyStillAndTry".Localized());

            ReturnToCamp(1);
        }

        public void GetRevengeForBanishments()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GetRevengeForBanishments.StandingOverGloating".Localized());

            GargoyleConfidence(-5);

            ReturnToCamp(1);
        }

        public void CuddleForBanishments()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.CuddleForBanishments.DespiteThePain".Localized());

            GargoyleConfidence(5);

            ReturnToCamp(1);
        }

        public void GargoyleKinkyRituals()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KinkyRitualsSpoken) == 0)
            {
                OutputText("Scenes.Gargoyle.GargoyleKinkyRituals.BroachSubjectOfRituals".Localized());

                OutputText("Scenes.Gargoyle.GargoyleKinkyRituals.AskExactlyWhatShe".Localized());

                OutputText("Scenes.Gargoyle.GargoyleKinkyRituals.LastMasterObsessed".Localized());
            }
            else
                OutputText("Scenes.Gargoyle.GargoyleKinkyRituals.WalkToTheChurchs".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Body".Localized(), KinkyBodyRitual);

            AddButton(1, "Common.Button.Mind".Localized(), MindRitualPervy);

            if (Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                AddButton(2, "Common.Button.Banish".Localized(), BanishPervRitual);
            else
                AddButton(2, "Common.Button.Banish".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), () => ReturnToCathedral());
        }

        public void KinkyBodyRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.TellCouldStand".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.WatchOverYourShoulder".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Gargoyle.KinkyBodyRitual.CuppingInColdStone".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.YourGoRockHard".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.BodySoWarped".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.AsIfReadingThoughts".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.PlaysEasilyWithin".Localized());

            if (Player.HasCock())
                OutputText(Player.CockDescript(0));
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript());
                else
                    OutputText("Scenes.Gargoyle.KinkyBodyRitual.BarrenCrotch".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.BreaksKissAndSteps".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.GuidesYouOverTo".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Gargoyle.KinkyBodyRitual.OnYourChest".Localized());
            else
                OutputText("Scenes.Gargoyle.KinkyBodyRitual.BetweenYourChest".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.FeelMoreThickSkinned".Localized());

            OutputText("Scenes.Gargoyle.KinkyBodyRitual.SpendFewMinutesCuddling".Localized());

            Player.ApplyStats(new DynStat(GameStats.Toughness, .5f), new DynStat(GameStats.Libido, -.5f), new DynStat(GameStats.Sensitivity, -2f));

            GargoyleConfidence(2);

            Player.HealthChange(-(float)(Player.MaxHealth / 2));

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void MindRitualPervy()
        {
            ClearOutput();

            GargoyleConfidence(2);

            OutputText("Scenes.Gargoyle.MindRitualPervy.TellYouveBeen".Localized());

            OutputText("Scenes.Gargoyle.MindRitualPervy.LiftsToPerchAtop".Localized());

            OutputText("Scenes.Gargoyle.MindRitualPervy.SlipsSmallToesInto".Localized());

            OutputText("Scenes.Gargoyle.MindRitualPervy.DoAsSheObviously".Localized());

            OutputText("Scenes.Gargoyle.MindRitualPervy.SoonClutchingBreasts".Localized());

            OutputText("Scenes.Gargoyle.MindRitualPervy.ScreamingArchesBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, .5f), new DynStat(GameStats.Lust, -10f), new DynStat(GameStats.Corruption, -.5f));

            Player.ChangeFatigue(50);

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void BanishPervRitual()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.BanishPervRitual.ExplainThatYouSeem".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishPervRitual.AHitchhiker".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishPervRitual.SomeHitchhikers".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.InYourTravelsTired".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishPervRitual.ItWord".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishPervRitual.ThemWord".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.UnderstandingNod".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.GivesShaftOfThe".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.GaspsPushingBack".Localized());

            if (Player.Anal.Wetness > 0)
                OutputText("Scenes.Gargoyle.BanishPervRitual.SlipsInEasily".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishPervRitual.SlowlyButSurelyCoaxes".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.SentencePeriod".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.RodFirmlyInsideYou".Localized());

            if (!Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishPervRitual.FeelHerCockRam".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.FuckYouInEarnest".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishPervRitual.HearTheDemonicVoice".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Gargoyle.BanishPervRitual.ReleasesYourHips".Localized());
            else
            {
                OutputText("Scenes.Gargoyle.BanishPervRitual.PeakOfHerSpeed".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                    OutputText("Scenes.Gargoyle.BanishPervRitual.ExgartuanIs".Localized());
                else
                    OutputText("Scenes.Gargoyle.BanishPervRitual.TheWormsAre".Localized());

                OutputText("Scenes.Gargoyle.BanishPervRitual.SuckedFromYourBody".Localized());
            }

            OutputText("Scenes.Gargoyle.BanishPervRitual.HolyRodJammedUp".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                OutputText("Scenes.Gargoyle.BanishPervRitual.WithinYouHas".Localized());
            else
                OutputText("Scenes.Gargoyle.BanishPervRitual.SWithinYouHave".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.AbsorbedAndPurged".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.StillPantingAndGasping".Localized());

            OutputText("Scenes.Gargoyle.BanishPervRitual.EventuallyYouKnow".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan))
                Player.RemoveStatusEffect(StatusEffectIds.Exgartuan);
            else
                Player.RemoveStatusEffect(StatusEffectIds.Infested);

            Player.HealthChange(-(float)(Player.MaxHealth / 3));

            Player.ChangeFatigue(10);

            GargoyleConfidence(2);

            CoC.Instance.Flags.Set(GameFlagIds.LewdedTheGargoyle, 1);

            ReturnToCamp(1);
        }

        public void TalkToGargoyle(bool output = true)
        {
            ClearOutput();

            var loverTalkTime = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.GarHistory) >= 3 && CoC.Instance.Flags.Get(GameFlagIds.GarCathedral) >= 3 && CoC.Instance.Flags.Get(GameFlagIds.GarNameTalks) >= 3 && (GargoyleConfidence() > 99 && CoC.Instance.Flags.Get(GameFlagIds.GarLoverTalks) < 1))
                loverTalkTime = true;

            if (output)
            {
                ShowImage("gargoyle-cathedral");

                OutputText("Scenes.Gargoyle.TalkToGargoyle.TakeSeatOnOne".Localized());

                if (GargoyleConfidence() < 50)
                    OutputText("Scenes.Gargoyle.TalkToGargoyle.MeeklyApproachesMaster".Localized());
                else
                    if (GargoyleConfidence() < 70)
                        OutputText("Scenes.Gargoyle.TalkToGargoyle.NearlySkipsToFollow".Localized());
                    else
                        OutputText("Scenes.Gargoyle.TalkToGargoyle.HappilyTakesASeat".Localized());

                if (loverTalkTime)
                    OutputText("Scenes.Gargoyle.TalkToGargoyle.SomethingWantingToAsk".Localized());
                else
                    OutputText("Scenes.Gargoyle.TalkToGargoyle.TellHerYoudLike".Localized());
            }

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.GarHistory) < 3)
                AddButton(0, "Common.Button.History".Localized(), HistoryGo);
            else
                AddButton(0, "Common.Button.History".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.GarCathedral) < 3)
                AddButton(1, "Common.Button.Cathedral".Localized(), CathedralTalks);
            else
                AddButton(1, "Common.Button.Cathedral".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.GarNameTalks) < 3)
                AddButton(2, "Common.Button.Her".Localized(), TalkAboutGarName);
            else
                AddButton(2, "Common.Button.Her".Localized(), null);

            if (loverTalkTime)
                AddButton(3, "Common.Button.Relationship".Localized(), RelationshipTalk);
            else
                AddButtonDisabled(3, "Relationship", (CoC.Instance.Flags.Get(GameFlagIds.GarLoverTalks) < 1) ? "You should get closer to " + CoC.Instance.Flags.Get(GameFlagIds.GarName) + "Common.Text.First".Localized() : "");

            AddButton(14, "Common.Button.Back".Localized(), () => ReturnToCathedral(true));
        }

        public void HistoryGo()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.GarHistory) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GarHistory, 1);

                TalkToGargoyleHistoryA();
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GarHistory) == 1)
                {
                    HistoryOfGargoylesB();

                    CoC.Instance.Flags.Set(GameFlagIds.GarHistory, 2);
                }
                else
                {
                    CoC.Instance.Flags.Set(GameFlagIds.GarHistory, 3);

                    GargoyleHistoryC();
                }
        }

        public void TalkToGargoyleHistoryA()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.TalkToGargoyleHistoryA.AskCanTell".Localized());

            OutputText("Scenes.Gargoyle.TalkToGargoyleHistoryA.TellHerSureYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Berate".Localized(), BerateGargoyleForBeingDumb);

            AddButton(1, "Common.Button.Comfort".Localized(), ComfortGargoyleDumbness);
        }

        public void BerateGargoyleForBeingDumb()
        {
            ClearOutput();

            GargoyleConfidence(-5);

            OutputText("Scenes.Gargoyle.BerateGargoyleForBeingDumb.AttemptToGiveNone".Localized());

            ReturnToCamp(1);
        }

        public void ComfortGargoyleDumbness()
        {
            ClearOutput();

            GargoyleConfidence(10);

            OutputText("Scenes.Gargoyle.ComfortGargoyleDumbness.SighAndReachOver".Localized());

            if (GargoyleConfidence() < 50)
                OutputText("Scenes.Gargoyle.ComfortGargoyleDumbness.SheRecoilsBut".Localized());

            OutputText("Scenes.Gargoyle.ComfortGargoyleDumbness.CupCheekAndTurn".Localized());

            TalkToGargoyle(false);
        }

        public void HistoryOfGargoylesB()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.HistoryOfGargoylesB.AskToTellYou".Localized());

            if (GargoyleConfidence() >= 70)
                OutputText("Scenes.Gargoyle.HistoryOfGargoylesB.IfShesUpIt".Localized());

            OutputText("Scenes.Gargoyle.HistoryOfGargoylesB.PeriodSpace".Localized());

            OutputText("Scenes.Gargoyle.HistoryOfGargoylesB.HesitantlyNodsAssent".Localized());

            if (GargoyleConfidence() > 70)
                OutputText("Scenes.Gargoyle.HistoryOfGargoylesB.DespiseThemWithEvery".Localized());

            OutputText("Scenes.Gargoyle.HistoryOfGargoylesB.MayNotBeMost".Localized());

            OutputText("Scenes.Gargoyle.HistoryOfGargoylesB.TellHerThatsFine".Localized());

            GargoyleConfidence(10);

            TalkToGargoyle(false);
        }

        public void GargoyleHistoryC()
        {
            ClearOutput();

            if (SceneInstance<IzmaScene>.Get().IzmaFollower() && SceneInstance<IsabellaFollowerScene>.Get().IsabellaFollower())
            {
                OutputText("Scenes.Gargoyle.GargoyleHistoryC.AskToTellYou".Localized());

                OutputText("Scenes.Gargoyle.GargoyleHistoryC.WhoaThereStopExplain".Localized());

                if (GargoyleConfidence() > 50)
                    OutputText("Scenes.Gargoyle.GargoyleHistoryC.JustLikeHer".Localized());

                OutputText("Scenes.Gargoyle.GargoyleHistoryC.IfCouldYoureSure".Localized());

                OutputText("Scenes.Gargoyle.GargoyleHistoryC.GivingHerAPat".Localized());

                OutputText("Scenes.Gargoyle.GargoyleHistoryC.OhShitWeGo".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Dodge".Localized(), IzmaAndIsabellaDodge);

                AddButton(1, "Common.Button.Slave".Localized(), GargoylesAreSlavesYo);

                AddButton(2, "Common.Button.Girlfriend".Localized(), GargoylesAreGirlfriends);
            }
            else
                if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                {
                    OutputText("Scenes.Gargoyle.GargoyleHistoryC.AskTellAAgain".Localized());

                    OutputText("Scenes.Gargoyle.GargoyleHistoryC.CutOffRightThere".Localized());

                    OutputText("Scenes.Gargoyle.GargoyleHistoryC.WellDoYouSay".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.NotExclusive".Localized(), MarbleAintExclusiveBiatch);

                    AddButton(1, "Common.Button.Doesntcount".Localized(), GargoyleDoesntCountAsAWaifu);
                }
                else
                    if (SceneInstance<IzmaScene>.Get().IzmaFollower())
                    {
                        OutputText("Scenes.Gargoyle.GargoyleHistoryC.AskTellAThird".Localized());

                        OutputText("Scenes.Gargoyle.GargoyleHistoryC.WellDoSayAgain".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.NotExclusive".Localized(), SharkgirlsArentExclusiveBiatch);

                        AddButton(1, "Common.Button.Doesntcount".Localized(), GargoylesDontCountAsSharkWaifus);
                    }
                    else
                    {
                        OutputText("Scenes.Gargoyle.GargoyleHistoryC.AskTellAFourth".Localized());

                        GargoyleConfidence(5);

                        ReturnToCathedral(true);
                    }
        }

        public void MarbleAintExclusiveBiatch()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.MarbleAintExclusiveBiatch.RunHandThroughHair".Localized());

            ReturnToCathedral(true);
        }

        public void GargoyleDoesntCountAsAWaifu()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GargoyleDoesntCountAsAWaifu.ScoffTellHerNot".Localized());

            GargoyleConfidence(-5);

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverChoice, 1);

            ReturnToCathedral(true);
        }

        public void SharkgirlsArentExclusiveBiatch()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.SharkgirlsArentExclusiveBiatch.RunHandThroughHair".Localized());

            ReturnToCathedral(true);
        }

        public void GargoylesDontCountAsSharkWaifus()
        {
            ClearOutput();

            GargoyleConfidence(-5);

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverChoice, 1);

            OutputText("Scenes.Gargoyle.GargoylesDontCountAsSharkWaifus.ScoffTellHerNot".Localized());

            ReturnToCathedral(true);
        }

        public void IzmaAndIsabellaDodge()
        {
            ClearOutput();

            GargoyleConfidence(-5);

            OutputText("Scenes.Gargoyle.IzmaAndIsabellaDodge.MakeAShittyJoke".Localized());

            ReturnToCathedral(true);
        }

        public void GargoylesAreSlavesYo()
        {
            ClearOutput();

            GargoyleConfidence(-15);

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverChoice, 1);

            OutputText("Scenes.Gargoyle.GargoylesAreSlavesYo.WhatIsSheBut".Localized());

            OutputText("Scenes.Gargoyle.GargoylesAreSlavesYo.TemporarilyBrokenThe".Localized());

            ReturnToCamp(1);
        }

        public void GargoylesAreGirlfriends()
        {
            ClearOutput();

            GargoyleConfidence(15);

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverChoice, 2);

            OutputText("Scenes.Gargoyle.GargoylesAreGirlfriends.SmileAndTussleHer".Localized());

            OutputText("Scenes.Gargoyle.GargoylesAreGirlfriends.SoonSurprisedWhen".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.Vaginal".Localized(), GargoyleCoochiiGetsPlowed);
            else
                AddButton(1, "Common.Button.StrapOn".Localized(), StrapOnGargoyle);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void CathedralTalks()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.GarCathedral) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GarCathedral, CoC.Instance.Flags.Get(GameFlagIds.GarCathedral) + 1);

                TalkCathedralA();
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GarCathedral) == 1)
                {
                    CathedralBTalk();

                    CoC.Instance.Flags.Set(GameFlagIds.GarCathedral, 2);
                }
                else
                {
                    CathedralC();

                    CoC.Instance.Flags.Set(GameFlagIds.GarCathedral, 3);
                }
        }

        public void TalkCathedralA()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.TalkCathedralA.AskToTellYou".Localized());

            OutputText("Scenes.Gargoyle.TalkCathedralA.AsksLookingYouIn".Localized());

            OutputText("Scenes.Gargoyle.TalkCathedralA.CouldTellHerIts".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Pointless".Localized(), PointlessGargoylesArePointless);

            AddButton(1, "Common.Button.Encourage".Localized(), EncourageGargoyleWaifuToDoSomething);
        }

        public void PointlessGargoylesArePointless()
        {
            ClearOutput();

            GargoyleConfidence(-5);

            OutputText("Scenes.Gargoyle.PointlessGargoylesArePointless.RollEyesAndTry".Localized());

            OutputText("Scenes.Gargoyle.PointlessGargoylesArePointless.HowSillyOfMe".Localized());

            OutputText("Scenes.Gargoyle.PointlessGargoylesArePointless.NodAndChangeSubject".Localized());

            TalkToGargoyle(false);
        }

        // Haxe EncourageGargoyleWaifuToDoSomething()   outputText 2 
        public void EncourageGargoyleWaifuToDoSomething()
        {
            ClearOutput();

            GargoyleConfidence(10);

            OutputText("Scenes.Gargoyle.EncourageGargoyleWaifuToDoSomething.GiveAPat".Localized());

            OutputText("Scenes.Gargoyle.EncourageGargoyleWaifuToDoSomething.WithinTheHourTwo".Localized());

            Player.ChangeFatigue(10);

            ReturnToCamp(1);
        }

        public void CathedralBTalk()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.CathedralBTalk.AskToTellYou".Localized());

            OutputText("Scenes.Gargoyle.CathedralBTalk.AskIfSheMeans".Localized());

            OutputText("Scenes.Gargoyle.CathedralBTalk.FallsIntoMelancholy".Localized());

            GargoyleConfidence(10);

            TalkToGargoyle(false);
        }

        public void CathedralC()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.CathedralC.SitDownWithTo".Localized());

            OutputText("Scenes.Gargoyle.CathedralC.NowLookingStraightIn".Localized());

            if (GargoyleConfidence() > 69)
                OutputText("Scenes.Gargoyle.CathedralC.UnlessItWereWith".Localized());

            OutputText("Scenes.Gargoyle.CathedralC.BoundToThisPlace".Localized());

            OutputText("Scenes.Gargoyle.CathedralC.AskIfAnyOf".Localized());

            OutputText("Scenes.Gargoyle.CathedralC.SheWhimpers".Localized());

            OutputText("Scenes.Gargoyle.CathedralC.AllButHerQuick".Localized());

            OutputText("Scenes.Gargoyle.CathedralC.CouldComfortGargoyle".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Berate".Localized(), BerateDatGargoyle4SomeSavin);

            AddButton(1, "Common.Button.Comfort".Localized(), ComfortGargoyle);
        }

        public void ComfortGargoyle()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.ComfortGargoyle.PickUpAndWrap".Localized());

            OutputText("Scenes.Gargoyle.ComfortGargoyle.WhispersIntoYourEar".Localized());

            OutputText("Scenes.Gargoyle.ComfortGargoyle.SpendFewMoreMinutes".Localized());

            OutputText("Scenes.Gargoyle.ComfortGargoyle.SurpriseFollowsRunning".Localized());

            OutputText("Scenes.Gargoyle.ComfortGargoyle.RealizeThisIsFirst".Localized());

            GargoyleConfidence(10);

            ReturnToCamp(1);
        }

        public void BerateDatGargoyle4SomeSavin()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.BerateDatGargoyle4SomeSavin.BecomingLivingFootstool".Localized());

            GargoyleConfidence(-10);

            ReturnToCamp(1);
        }

        public void TalkAboutGarName()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.GarNameTalks) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.GarNameTalks, 1);

                GarNameA();
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GarNameTalks) == 1)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.GarNameTalks, 2);

                    GarNameB();
                }
                else
                {
                    CoC.Instance.Flags.Set(GameFlagIds.GarNameTalks, 3);

                    GarNameC();
                }
        }

        public void GarNameA()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GarNameA.CuriousAboutGargoyle".Localized());

            OutputText("Scenes.Gargoyle.GarNameA.UrgeHerOnAdding".Localized());

            if (Player.Race == "human")
                OutputText("Scenes.Gargoyle.GarNameA.AreWord".Localized());
            else
                OutputText("Scenes.Gargoyle.GarNameA.OnceWere".Localized());

            OutputText("Scenes.Gargoyle.GarNameA.MasterWeWere".Localized());

            OutputText("Scenes.Gargoyle.GarNameA.WhenDemonsCame".Localized());

            OutputText("Scenes.Gargoyle.GarNameA.AskTellYou".Localized());

            if (GargoyleConfidence() > 69)
                OutputText("Scenes.Gargoyle.GarNameA.ContinuesWithALook".Localized());

            OutputText("Scenes.Gargoyle.GarNameA.LastMasterFoundMe".Localized());

            if (GargoyleConfidence() <= 69)
                OutputText("Scenes.Gargoyle.GarNameA.AfterWhatSheDid".Localized());
            else
                OutputText("Scenes.Gargoyle.GarNameA.AfterDidAgain".Localized());

            OutputText("Scenes.Gargoyle.GarNameA.CantHelpButAsk".Localized());

            if (GargoyleConfidence() <= 69)
                OutputText("Scenes.Gargoyle.GarNameA.CantHurtsTooMuch".Localized());
            else
                OutputText("Scenes.Gargoyle.GarNameA.DontWantToTalk".Localized());

            OutputText("Scenes.Gargoyle.GarNameA.ConsiderPressingIssue".Localized());

            GargoyleConfidence(10);

            ReturnToCamp(1);
        }

        public void GarNameB()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GarNameB.SettlingInForWhat".Localized());

            OutputText("Scenes.Gargoyle.GarNameB.VoiceHardeningInto".Localized());

            OutputText("Scenes.Gargoyle.GarNameB.GentlyShiftConversation".Localized());

            OutputText("Scenes.Gargoyle.GarNameB.ConfusedHaveToAsk".Localized());

            OutputText("Scenes.Gargoyle.GarNameB.DontNeedToPrompt".Localized());

            OutputText("Scenes.Gargoyle.GarNameB.WhileStillHasntAnswered".Localized());

            GargoyleConfidence(5);

            TalkToGargoyle(false);
        }

        public void GarNameC()
        {
            ClearOutput();

            GargoyleConfidence(5);

            OutputText("Scenes.Gargoyle.GarNameC.AskIfShesFinally".Localized());

            OutputText("Scenes.Gargoyle.GarNameC.SheContinues".Localized());

            OutputText("Scenes.Gargoyle.GarNameC.PausesSnifflingAnd".Localized());

            OutputText("Scenes.Gargoyle.GarNameC.SheContinuesAgain".Localized());

            OutputText("Scenes.Gargoyle.GarNameC.FallsSilentHeaving".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Comfort".Localized(), ComfortGarNameC);

            AddButton(1, "Common.Button.Berate".Localized(), BerateGargoyleC);
        }

        public void ComfortGarNameC()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.ComfortGarNameC.PullOntoYourLap".Localized());

            OutputText("Scenes.Gargoyle.ComfortGarNameC.LittleGargoyleCurls".Localized());

            OutputText("Scenes.Gargoyle.ComfortGarNameC.CutHerOffRight".Localized());

            OutputText("Scenes.Gargoyle.ComfortGarNameC.SmilesWeaklyRests".Localized());

            OutputText("Scenes.Gargoyle.ComfortGarNameC.StammersConfused".Localized());

            OutputText("Scenes.Gargoyle.ComfortGarNameC.SighingPatOnHead".Localized());

            GargoyleConfidence(15);

            ReturnToCathedral(true);
        }

        public void BerateGargoyleC()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.BerateGargoyleC.LaughAndTellExactly".Localized());

            OutputText("Scenes.Gargoyle.BerateGargoyleC.WhenDoneExplaining".Localized());

            OutputText("Scenes.Gargoyle.BerateGargoyleC.SaysEvenlyAsShe".Localized());

            OutputText("Scenes.Gargoyle.BerateGargoyleC.NoddingAppreciatively".Localized());

            GargoyleConfidence(-10);

            ReturnToCathedral(true);
        }

        public void RelationshipTalk()
        {

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverTalks, 1);

            OutputText("Scenes.Gargoyle.RelationshipTalk.PattingHerHead".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GarLoverChoice) == 1)
                OutputText("Scenes.Gargoyle.RelationshipTalk.NothingATool".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.GarLoverChoice) == 2)
                    OutputText("Scenes.Gargoyle.RelationshipTalk.EvenIfGirlfriend".Localized());
                else
                    OutputText("Scenes.Gargoyle.RelationshipTalk.GottenSoCloseAnd".Localized());

            OutputText("Scenes.Gargoyle.RelationshipTalk.FidgetsBesideYou".Localized());

            OutputText("Scenes.Gargoyle.RelationshipTalk.LooksNervousAsWatches".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Urtascene.UrtaDiscussesAlcholism.LoverWord".Localized(), GargoyleWaifu);

            AddButton(1, "Common.Button.Family".Localized(), GargoyleDaughter);

            AddButton(2, "Common.Button.Slave".Localized(), GargoylePet);
        }

        public void GargoyleWaifu()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.GargoyleWaifu.SmileAndStrokeHair".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverChoice, 2);

            ReturnToCamp(1);
        }

        public void GargoyleDaughter()
        {
            ClearOutput();

            var familyType = "";

            if (Player.IsElder() && GarLoli())
                familyType = "granddaughter";
            else
                if (Player.IsChild() || Player.IsTeen())
                    familyType = "sister";
                else
                    familyType = "daughter";

            OutputText("Scenes.Gargoyle.GargoyleDaughter.ThinkAboutIt".Localized(
                familyType
            ));

            OutputText("Scenes.Gargoyle.GargoyleDaughter.BlinksLookOfSurprise".Localized());

            OutputText("Scenes.Gargoyle.GargoyleDaughter.GentlyStrokeHerHead".Localized());

            OutputText("Scenes.Gargoyle.GargoyleDaughter.LaughAndRuffleHair".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.LewdedTheGargoyle) == 1)
                OutputText("Scenes.Gargoyle.GargoyleDaughter.CanCloseFamily".Localized());
            else
                OutputText("Scenes.Gargoyle.GargoyleDaughter.YoullThinkAboutIt".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverChoice, 3);

            ReturnToCamp(1);
        }

        public void GargoylePet()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GarLoverChoice) == 1)
                OutputText("Scenes.Gargoyle.GargoylePet.ExplainThatNothing".Localized());
            else
                OutputText("Scenes.Gargoyle.GargoylePet.TellHerBluntly".Localized());

            OutputText("Scenes.Gargoyle.GargoylePet.GirlsEyesGoWide".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GarLoverChoice, 1);

            GargoyleConfidence(-60);

            ReturnToCamp(1);
        }

        public void GargoyleAppearance()
        {
            ClearOutput();

            ShowImage("gargoyle-cathedral");

            OutputText("Scenes.Gargoyle.GargoyleAppearance.GrayMarbleStatue".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAppearance.StandsRoughlyFeetTall".Localized());

            if (GargoyleConfidence() < 70)
                OutputText("Scenes.Gargoyle.GargoyleAppearance.GlowTimidlyReflecting".Localized());
            else
                OutputText("Scenes.Gargoyle.GargoyleAppearance.CoolFireInThem".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAppearance.SkinColdAndSmooth".Localized());

            if (GargoyleConfidence() < 70)
                OutputText("Scenes.Gargoyle.GargoyleAppearance.TimidlyWord".Localized());
            else
                OutputText("Scenes.Gargoyle.GargoyleAppearance.PlayfullyWord".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAppearance.AboutHerThighsTwo".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAppearance.HasBreastsWithoutNipples".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAppearance.SmallYetEasily".Localized());

            OutputText("Scenes.Gargoyle.GargoyleAppearance.TightAssholeBetween".Localized());

            ReturnToCathedral(true);
        }

        public void Wash()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.Wash.WhileGraySkin".Localized());

            OutputText("Scenes.Gargoyle.Wash.AskIfThereIs".Localized());

            OutputText("Scenes.Gargoyle.Wash.SheReplies".Localized());

            OutputText("Scenes.Gargoyle.Wash.PointsAFinger".Localized());

            OutputText("Scenes.Gargoyle.Wash.ChimesInFromBehind".Localized());

            OutputText("Scenes.Gargoyle.Wash.InAnyCaseSuitable".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Gently".Localized(), WashGently);

            AddNextButton("Common.Button.Roughly".Localized(), WashRoughly);

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), () => WashNevermind());
        }

        public void WashGently()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.WashGently.BeckonGirlCloser".Localized());

            OutputText("Scenes.Gargoyle.WashGently.SimplySaysSheSlips".Localized());

            OutputText("Scenes.Gargoyle.WashGently.AtHerDirection".Localized());

            OutputText("Scenes.Gargoyle.WashGently.MoodOnlyImproves".Localized());

            OutputText("Scenes.Gargoyle.WashGently.ContinueToWashHer".Localized());

            OutputText("Scenes.Gargoyle.WashGently.AfterSeveralMinutes".Localized());

            OutputText("Scenes.Gargoyle.WashGently.EventuallyYouReallyHave".Localized());

            OutputText("Scenes.Gargoyle.WashGently.SeemsSadButGives".Localized());

            OutputText("Scenes.Gargoyle.WashGently.WarmFeelingInYour".Localized());

            GargoyleConfidence(5);

            ReturnToCamp(1);
        }

        public void WashRoughly()
        {
            ClearOutput();

            OutputText("Scenes.Gargoyle.WashRoughly.CommandGirlToPresent".Localized());

            OutputText("Scenes.Gargoyle.WashRoughly.OnceThereSwingHer".Localized());

            OutputText("Scenes.Gargoyle.WashRoughly.LooksUncertainBut".Localized());

            OutputText("Scenes.Gargoyle.WashRoughly.GaspsAndSputters".Localized());

            OutputText("Scenes.Gargoyle.WashRoughly.SeeProcessThrough".Localized());

            OutputText("Scenes.Gargoyle.WashRoughly.ThatsAboutAll".Localized());

            GargoyleConfidence(-5);

            ReturnToCamp(1);
        }

        public void WashNevermind()
        {
            ClearOutput();

            ReturnToCathedral(true);

            OutputText("Scenes.Gargoyle.WashNevermind.OnSecondThought".Localized());

        }

    }
}