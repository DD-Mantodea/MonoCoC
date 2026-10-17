using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Plains
{
    [Register<XmasMiscScene>("coc:xmas_misc")]
    public class XmasMiscScene : Scene
    {

        public override void Entry()
        {
            CandyCaneTrapDiscovery();
        }

        public void CandyCaneTrapDiscovery()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.CandyCaneTrapDiscovery.ThoughtsDrift".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Xmasmisc.CandyCaneTrapDiscovery.NostalgicSmile".Localized());
            else
                OutputText("Scenes.Xmasmisc.CandyCaneTrapDiscovery.HowBlandWorld".Localized());

            OutputText("Scenes.Xmasmisc.CandyCaneTrapDiscovery.SharpMoan".Localized());

            OutputText("Scenes.Xmasmisc.CandyCaneTrapDiscovery.RunTheRisk".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Investigate".Localized(), InvestigateCandyCaneBun);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveBeforeMeetingCandyCaneBun);
        }

        public void LeaveBeforeMeetingCandyCaneBun()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.LeaveBeforeMeetingCandyCaneBun.NotWillingRisk".Localized());

            if (Player.Corruption > 50 || Player.Libido > 50)
                OutputText("Scenes.Xmasmisc.LeaveBeforeMeetingCandyCaneBun.NotRightNow".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void InvestigateCandyCaneBun()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.DecisionFindOut".Localized());

            if (Player.Corruption < 50 && Player.Libido < 50)
                OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.EyesDrawnRabbit".Localized((noFur ? "boy" : "morph")));
            else
                OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.EyesDrawnAgain".Localized((noFur ? "boy" : "morph")));

            OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.HangingBalls".Localized());

            OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.MakesTheMove".Localized());

            if (Player.Corruption < 50 && Player.Libido < 50)
                OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.LeaveWord".Localized());
            else
                OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.ApproachWord".Localized());

            OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.SnapsTwig".Localized((noFur ? "knees" : "thighs")));

            OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.RemarkablyHigh".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.CalmlyInforms".Localized());
            else
                OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.ThoughtDerailed".Localized());

            if (silly)
                OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.AnythingToCalm".Localized());

            OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.SmileLightsUp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.CandyCaneYearMet, DateTime.Now.Year);

            OutputText("Scenes.Xmasmisc.InvestigateCandyCaneBun.HelpHimOut".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), HelpWithTheCandyCane);

            AddButton(1, "Common.Button.No".Localized(), DeclineCandyCaneCawks);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));
        }

        public void DeclineCandyCaneCawks()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.DeclineCandyCaneCawks.TurningAwkwardly".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void HelpWithTheCandyCane()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.LowersEyes".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.RisesRocking".Localized());
            else
                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.CoatedLiberal".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.BothSexesFlare".Localized());
                    else
                        OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.SweatDrips".Localized());

            OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.PayNoMind".Localized());

            OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.BunnyCriesOut".Localized());

            OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.SlenderHandsGrip".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.NotDisturb".Localized());
            else
                OutputText("Scenes.Xmasmisc.HelpWithTheCandyCane.RisingUrge".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 100f), new DynStat(GameStats.Corruption, -5f));

            DoNext(() => ReturnToCamp(1));
        }

        public void GetAChristmasChicken()
        {

            CoC.Instance.Flags.Set(GameFlagIds.XmasChickenYear, DateTime.Now.Year);

            OutputText("Scenes.Xmasmisc.GetAChristmasChicken.SquawkingRouses".Localized());

            OutputText("Scenes.Xmasmisc.GetAChristmasChicken.DeterminedShut".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), TalkToXmasChicken);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveXmasChicken);
        }

        public void LeaveXmasChicken()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.LeaveXmasChicken.TooExasperated".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkToXmasChicken()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.TalkToXmasChicken.TryAsking".Localized());

            OutputText("Scenes.Xmasmisc.TalkToXmasChicken.WatchesLie".Localized());

            OutputText("Scenes.Xmasmisc.TalkToXmasChicken.HarpyLooksUp".Localized());

            OutputText("Scenes.Xmasmisc.TalkToXmasChicken.DespitePlumage".Localized(
                (Player.Gender == Gender.Male ? "How's a blowjob sound to you?" : Player.Gender == Gender.Female ? "How about I eat you out?" : "How about I give you a few minutes to do whatever you want with my mouth?")
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), AcceptChristmasBJs);

            AddButton(4, "Scenes.Lumberjack.Button.Decline".Localized(), DeclineChickenBlowjobs);
        }

        public void DeclineChickenBlowjobs()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.DeclineChickenBlowjobs.SquintsSuspiciously".Localized());

            DoNext(PlayerMenu);
        }

        public void AcceptChristmasBJs()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
                AcceptChristmasManBJ();
            else
                if (Player.Gender == Gender.Female)
                    AcceptChristmasWomanBJ();
                else
                {
                    OutputText("Scenes.Xmasmisc.AcceptChristmasBJs.ServiceQuestion".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.ManParts".Localized(), AcceptChristmasManBJ);

                    AddButton(1, "Common.Button.LadyBits".Localized(), AcceptChristmasWomanBJ);
                }
        }

        public void AcceptChristmasManBJ()
        {
            ClearOutput();

            var x = Player.CockThatFits(80);

            if (x < 0)
                x = Player.BiggestCockIndex();

            if (Player.CockArea(x) >= 80)
            {
                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.FreesCock".Localized(
                    x + 1
                ));

                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.FleesTerrified".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

                DoNext(PlayerMenu);

                return ;
            }
            else
            {
                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.FreesCockAgain".Localized(
                    x + 1
                ));

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.SLetter".Localized());

                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.AlmostHungrily".Localized());

                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.ThroatHeat".Localized(
                    x + 1
                ));

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.GrabsHips".Localized());
                else
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.WorksShaft".Localized());
                    else
                        OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.TwoOtherCocks".Localized());

                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.FellatingTongue".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.LooksContented".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.SemenGushes".Localized());
                    else
                        OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.AtFirstHandles".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.CumSprays".Localized());

                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.LetsHarpyGo".Localized(
                    x + 1
                ));

                OutputText("Scenes.Xmasmisc.AcceptChristmasManBJ.HappyGirlHugs".Localized());
            }

            Player.Orgasm("Dick");

            DoNext(PlayerMenu);
        }

        public void AcceptChristmasWomanBJ()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.AcceptChristmasWomanBJ.NodsAcceptance".Localized());

            OutputText("Scenes.Xmasmisc.AcceptChristmasWomanBJ.NotLongBefore".Localized());

            OutputText("Scenes.Xmasmisc.AcceptChristmasWomanBJ.GetsMessage".Localized());

            OutputText("Scenes.Xmasmisc.AcceptChristmasWomanBJ.SuddenlyEmbraces".Localized());

            Player.Orgasm("Dick");

            DoNext(PlayerMenu);
        }

        public void EncounterKamiTheChristmasRoo()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.KamiEncounter) == 0)
                OutputText("Scenes.Xmasmisc.EncounterKamiTheChristmasRoo.EntersBakery".Localized((noFur ? "girl" : "morph")));
            else
                OutputText("Scenes.Xmasmisc.EncounterKamiTheChristmasRoo.BakeryAdorned".Localized());
        }

        public void ApproachKamiTheChristmasRoo()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.KamiEncounter) == 1)
            {
                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.GivesNod".Localized());

                ClearButtons();

                if (Player.HasCockThatFits(70))
                    AddButton(0, "Common.Button.GiveAnal".Localized(), GiveKamiTheChristmasRooAnal);

                AddButton(1, "Common.Button.ReceiveAnal".Localized(), TakeItRooButtStyle);

                AddButton(2, "Common.Button.GiveBj".Localized(), KamiBlowJob);

                if (Player.CockTotal() > 1 && Player.HasCockThatFits(70) && Player.CockThatFits2(70) >= 0)
                    AddButton(3, "Common.Button.DoubleFuck".Localized(), KamiDoubleDickFuck);
            }
            else
            {
                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.SummonsWaitress".Localized());

                if (Player.Femininity >= 75)
                    OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.FeminineWord".Localized());
                else
                    if (Player.Femininity <= 25)
                        OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.MasculineWord".Localized());

                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.LipsAbruptly".Localized());

                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.StopStayHere".Localized());

                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.KangaRises".Localized());

                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.PlacesPresent".Localized());

                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.NoteReads".Localized(
                    Player.Mf("stud", "babe")
                ));

                if (silly)
                    OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.ScrawledPenis".Localized());

                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.SentencePeriod".Localized());

                OutputText("Scenes.Xmasmisc.ApproachKamiTheChristmasRoo.PrettyBlunt".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LetsGo".Localized(), ChooseLetsGoKami);

                AddButton(14, "Common.Button.Leave".Localized(), SkipOutAfterOpeningKamisBox);
            }
        }

        public void SkipOutAfterOpeningKamisBox()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.SkipOutAfterOpeningKamisBox.PolitelyDecline".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KamiEncounter, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void ChooseLetsGoKami()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.ChooseLetsGoKami.GiveKamiWhat".Localized());

            OutputText("Scenes.Xmasmisc.ChooseLetsGoKami.KamiConfirms".Localized());

            OutputText("Scenes.Xmasmisc.ChooseLetsGoKami.MoansEnticingly".Localized());

            OutputText("Scenes.Xmasmisc.ChooseLetsGoKami.GazeOver".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(70))
                AddButton(0, "Common.Button.GiveAnal".Localized(), GiveKamiTheChristmasRooAnal);

            AddButton(1, "Common.Button.ReceiveAnal".Localized(), TakeItRooButtStyle);

            AddButton(2, "Common.Button.GiveBj".Localized(), KamiBlowJob);

            if (Player.CockTotal() > 1 && Player.HasCockThatFits(70) && Player.CockThatFits2(70) >= 0)
                AddButton(3, "Common.Button.DoubleFuck".Localized(), KamiDoubleDickFuck);
        }

        public void GiveKamiTheChristmasRooAnal()
        {
            ClearOutput();

            var x = Player.CockThatFits(70);

            OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.HeadsBriskly".Localized());

            OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.IntentDominant".Localized());

            OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.SquealsThrusts".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.BallsSlapping".Localized());

            OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.WincesThrust".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.SquealsClimax".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.MoansSubmissively".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.HandFlies".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Xmasmisc.GiveKamiTheChristmasRooAnal.ReplacingClothes".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.KamiEncounter, 1);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeItRooButtStyle()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.WinksWaitress".Localized());

            Player.ButtChange(24, true, true, false);

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.HipsPistoning".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.YourOwn".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.YourPussy".Localized());
                else
                    OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.YourTaint".Localized());

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.FondlesForm".Localized());

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.ParagraphBreak".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.CuntOozes".Localized());

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.FewMoreMinutes".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.ProstateWord".Localized());
            else
                OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.AssWord".Localized());

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.NumbingSensations".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.PentUpCock".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.MeanwhileY".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.YLetter".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.EagerPussyBuckles".Localized());

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.TrulySpent".Localized());

            OutputText("Scenes.Xmasmisc.TakeItRooButtStyle.NeedsMore".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            DoNext(() => ReturnToCamp(1));

            CoC.Instance.Flags.Set(GameFlagIds.KamiEncounter, 1);
        }

        public void KamiBlowJob()
        {
            ClearOutput();

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Xmasmisc.KamiBlowJob.GetsOnKnees".Localized());
            else
                OutputText("Scenes.Xmasmisc.KamiBlowJob.SteersHalf".Localized());

            OutputText("Scenes.Xmasmisc.KamiBlowJob.GrabsRod".Localized());

            OutputText("Scenes.Xmasmisc.KamiBlowJob.ReactionSurprises".Localized());

            OutputText("Scenes.Xmasmisc.KamiBlowJob.GulpsLoad".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmasmisc.KamiBlowJob.MightStillNeed".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 10 + Player.Sensitivity / 10)));

            CoC.Instance.Flags.Set(GameFlagIds.KamiEncounter, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void KamiDoubleDickFuck()
        {
            ClearOutput();

            var x = Player.CockThatFits(70);

            var y = Player.CockThatFits2(70);

            OutputText("Scenes.Xmasmisc.KamiDoubleDickFuck.ChucklesHeartily".Localized());

            OutputText("Scenes.Xmasmisc.KamiDoubleDickFuck.TakesInitiative".Localized());

            OutputText("Scenes.Xmasmisc.KamiDoubleDickFuck.RearsOutSlow".Localized());

            OutputText("Scenes.Xmasmisc.KamiDoubleDickFuck.ScreamsThrashing".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y)
            ));

            OutputText("Scenes.Xmasmisc.KamiDoubleDickFuck.SecondsBehind".Localized());

            OutputText("Scenes.Xmasmisc.KamiDoubleDickFuck.GathersThoughts".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            CoC.Instance.Flags.Set(GameFlagIds.KamiEncounter, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void GetWinterPudding()
        {
            ClearOutput();

            if (Player.Gems < 35)
            {
                OutputText("Scenes.Xmasmisc.GetWinterPudding.PuddingNotFree".Localized());

                return ;
            }

            Player.Gems -= 35;

            OutputText("Scenes.Xmasmisc.GetWinterPudding.OptsSlice".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Faerie.SexMenu.ButtonEat".Localized(), EatWinterPudding);

            AddButton(1, "Common.Button.Take".Localized(), () => InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.WinterPudding), () => Go<CampScene>()));
        }

        public void EatWinterPudding()
        {
            ClearOutput();

        }

        public void PolarPete()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.PolarPeteYearMet, DateTime.Now.Year);

            OutputText("Scenes.Xmasmisc.PolarPete.SomethingOff".Localized());

            OutputText("Scenes.Xmasmisc.PolarPete.CrunchSnow".Localized());

            OutputText("Scenes.Xmasmisc.PolarPete.LargeMan".Localized((noFur ? "particularly his limbs which are" : "as his body is")));

            OutputText("Scenes.Xmasmisc.PolarPete.HappyHoliday".Localized());

            OutputText("Scenes.Xmasmisc.PolarPete.LargeSack".Localized());

            OutputText("Scenes.Xmasmisc.PolarPete.FurredHandCups".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Unwrap".Localized(), UnwrapPolarPete);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveDisFukkinPolarBear);
        }

        public void UnwrapPolarPete()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.TemptationStrong".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.PurrsShine".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.EyesLookUp".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.TongueRunsAlong".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.NestlesHair".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.SmilesHalfLidded".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.BumpsThroat".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.GroinRams".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.StaresLaughing".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.PullsCockFree".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.WinksSlyly".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.EggsYouOn".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.JollyLaugh".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmasmisc.UnwrapPolarPete.StiffSoaksArmor".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Xmasmisc.UnwrapPolarPete.GrowsMoist".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.TwinkleEye".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.SnickersFreeHand".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.ShutsEyesBliss".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.LengthHilting".Localized());

            Player.ButtChange(25, true, true, false);

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.CloudsMist".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Xmasmisc.UnwrapPolarPete.RamsProstate".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Xmasmisc.UnwrapPolarPete.QuiversThrusting".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.WatchesWrithe".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.TimeToGo".Localized());

            OutputText("Scenes.Xmasmisc.UnwrapPolarPete.SlyWink".Localized());

            Player.Orgasm("Anal");

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.MintWhite), () => ReturnToCamp(1));
        }

        public void LeaveDisFukkinPolarBear()
        {
            ClearOutput();

            OutputText("Scenes.Xmasmisc.LeaveDisFukkinPolarBear.DistrustStranger".Localized());

            OutputText("Scenes.Xmasmisc.LeaveDisFukkinPolarBear.BrightSmile".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.MintWhite), () => ReturnToCamp(1));
        }

    }
}