using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Enemies.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep
{
    [Register<MinotaurKingScenes>("coc:minotaur_king_scenes")]
    public class MinotaurKingScenes : Scene
    {

        public override void Entry()
        {
        }

        public void EncounterMinotaurKing()
        {
            ClearOutput();

            ShowImage("monster-minotaurlord");

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.StepsAway".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.BovineBrute".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.BrutePromises".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.VisionOfCurves".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.IsabellaFollowerAccepted) != 0)
                OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.IsabellaStruggle".Localized());
            else

                if (SceneInstance<MarbleScene>.Get().MarbleFollower())
                    OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.MarbleStruggle".Localized());
                else if (SceneInstance<HelScene>.Get().FollowerHel())
                    OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.HeliaStruggle".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.PoorCowSlut".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.GleamingShaft".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.FeminineVoice".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.EarsTwitching".Localized());

            OutputText("Scenes.Minotaurkingscenes.EncounterMinotaurKing.MetricFuckton".Localized());

            Player.CreateStatusEffect(MinotaurKingMuskId, 0, 0, 0, 0);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "minotaur_king")), RoomDungeon.ResumeFromFight);
        }

        public void TheKingIsDeadLongLiveTheKing(bool hpVictory)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingDefeated, 1);

            OutputText("Scenes.Minotaurkingscenes.TheKingIsDeadLongLiveTheKing.AxeClatters".Localized());

            OutputText("Scenes.Minotaurkingscenes.TheKingIsDeadLongLiveTheKing.SlowClap".Localized());

            OutputText("Scenes.Minotaurkingscenes.TheKingIsDeadLongLiveTheKing.IfShePreyed".Localized());

            if (Player.Corruption <= 33)
                OutputText("Scenes.Minotaurkingscenes.TheKingIsDeadLongLiveTheKing.PoorThingsCorrupted".Localized());

            ClearButtons();

            AddButtonDisabled(1, "Docking", "This scene requires you to have a fitting cock.");

            AddButtonDisabled(2, "Buttfuck", "This scene requires you to have a fitting cock.");

            AddButtonDisabled(3, "Titfuck", "This scene requires you to have a cock.");

            AddButtonDisabled(4, "SloppySeconds", "This scene requires you to have a cock.");

            AddButtonDisabled(5, "Ride Him", "This scene requires you to have a vagina.");

            AddButton(0, "Common.Button.KillThem".Localized(), Murderhobo);

            if (Player.HasCock())
            {
                if (Player.SmallestCockArea() <= 50)
                    AddButton(1, "Common.Button.Docking".Localized(), () => Dockucocku(Player.SmallestCockIndex()));

                if (Player.HasCockThatFits(200))
                    AddButton(2, "Common.Button.ButtFuck".Localized(), Buttufucku);

                AddButton(3, "Common.Button.Titfuck".Localized(), TitfuckCowslut);

                AddButton(4, "Common.Button.Sloppyseconds".Localized(), SloppySeconds);
            }

            if (Player.HasVagina())
                AddButton(5, "Scenes.Akbal.TakeAdvantageOfAkbitch.ButtonRideHim".Localized(), Mechanicalbullhue);

            AddNextButton("Common.Button.LetGo".Localized(), LetGo);
        }

        public void LetGo()
        {
            ClearOutput();

            OutputText("Scenes.Minotaurkingscenes.LetGo.NoInterest".Localized());

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void Murderhobo()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingKilled, 1);

            OutputText("Scenes.Minotaurkingscenes.Murderhobo.MakeItQuick".Localized());

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void Dockucocku(int cockIdx)
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingFucked, 1);

            ShowImage("monster-minotaurlord");

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.DickThatBig".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.WildlyFlaringTip".Localized());

            if (Player.CountCocksOfType("Horse") > 0)
                OutputText("Scenes.Minotaurkingscenes.Dockucocku.HorseDickLikeYours".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.Dockucocku.AverageEquinePrick".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.DiscardsThought".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.OneDoing".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.GrabsMonarch".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.BulgingFlare".Localized(
                cockIdx + 1,

                cockIdx + 1
            ));

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.HoldsBack".Localized(
                cockIdx + 1
            ));

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.WaversBathing".Localized(
                cockIdx + 1
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Minotaurkingscenes.Dockucocku.AndBallsWord".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.SteadyPre".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.TightInside".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.FeelingGenerous".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.MaybeJustSome".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucocku.InhalesDeeply".Localized());

            if (Player.CumQ() <= 500)
                OutputText("Scenes.Minotaurkingscenes.Dockucocku.CumsOneMoment".Localized());
            else
                if (Player.CumQ() <= 2500)
                    OutputText("Scenes.Minotaurkingscenes.Dockucocku.ImpregnateMildTerm".Localized());
                else
                    OutputText("Scenes.Minotaurkingscenes.Dockucocku.FirehoseSpraying".Localized());

            if (!Player.HasKnot(cockIdx) || Player.Cocks[cockIdx].KnotMultiplier * Player.Cocks[cockIdx].Thickness <= 2)
            {
                OutputText("Scenes.Minotaurkingscenes.Dockucocku.PullsOutSatisfied".Localized());

                OutputText("Scenes.Minotaurkingscenes.Dockucocku.PowerfulShaggy".Localized());

                OutputText("Scenes.Minotaurkingscenes.Dockucocku.NeitherStanding".Localized());

                CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
            }
            else
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => Dockucockuknotu(cockIdx));
            }
        }

        public void Dockucockuknotu(int cockIdx)
        {
            ClearOutput();

            OutputText("Scenes.Minotaurkingscenes.Dockucockuknotu.TriesPullOut".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucockuknotu.KingMoaning".Localized(
                cockIdx + 1
            ));

            ShowImage("badend-minotaurlord-knot");

            OutputText("Scenes.Minotaurkingscenes.Dockucockuknotu.PressureReversed".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Minotaurkingscenes.Dockucockuknotu.FatWithCream".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.Dockucockuknotu.SwellsLarger".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucockuknotu.ShouldBeAlarmed".Localized());

            OutputText("Scenes.Minotaurkingscenes.Dockucockuknotu.GreatFuzzy".Localized());

            GameOver();
        }

        public void Buttufucku()
        {
            ClearOutput();

            var x = Player.CockThatFits(200);

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingFucked, 1);

            ShowImage("monster-minotaurlord");

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.WhySpoiledTart".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.PerfectIntent".Localized());

            if (Player.Cocks.Count == 1)
                OutputText("Scenes.Minotaurkingscenes.Buttufucku.PluralS".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.GrowingHard".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.KicksKing".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.ShesWatching".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.PainedMoo".Localized());

            if (Player.BiggestCockLength() >= 24)
                OutputText("Scenes.Minotaurkingscenes.Buttufucku.BellyBulges".Localized(
                    Player.CockDescript(x)
                ));
            else
                if (Player.BiggestCockLength() >= 12)
                {
                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.FedHisAss".Localized());

                    if (Player.HasKnot())
                        OutputText("Scenes.Minotaurkingscenes.Buttufucku.KnotAndAll".Localized());

                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.VisibleBulge".Localized());
                }
                else
                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.BuriedEntirety".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.FirstClass".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.WasteGood".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.CockStandsStraight".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.WrigglesTwists".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Minotaurkingscenes.Buttufucku.OtherMember".Localized());

                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.PluralSAgain".Localized());

                OutputText("Scenes.Minotaurkingscenes.Buttufucku.SlippingSliding".Localized());
            }

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.BetterThings".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.GivesLittle".Localized());

            if (Player.HasKnot())
                OutputText("Scenes.Minotaurkingscenes.Buttufucku.KnotBalloons".Localized());
            else
                if (Player.CountCocksOfType("Horse") > 0)
                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.FlareExpands".Localized());
                else
                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.FlareBlooms".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.EruptsSplitSecond".Localized());

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.LandsInMouth".Localized());

            if (Player.CumQ() > 2500)
            {
                OutputText("Scenes.Minotaurkingscenes.Buttufucku.BellySwells".Localized());

                if (Player.BiggestCockLength() >= 18)
                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.EvenBigger".Localized());

                if (Player.CumQ() > 5000)
                {
                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.FarFromDone".Localized());

                    if (Player.Balls > 0)
                        OutputText("Scenes.Minotaurkingscenes.Buttufucku.YourBalls".Localized());
                    else
                        OutputText("Scenes.Minotaurkingscenes.Buttufucku.YourCrotch".Localized());

                    OutputText("Scenes.Minotaurkingscenes.Buttufucku.StickyPuddle".Localized());
                }

                OutputText("Scenes.Minotaurkingscenes.Buttufucku.SentencePeriod".Localized());
            }

            OutputText("Scenes.Minotaurkingscenes.Buttufucku.SquirmsWrithes".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void Mechanicalbullhue()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingFucked, 1);

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.HandedOpportunity".Localized());

            if (AverageVaginalLooseness() <= 3)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.ProbablyWord".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.SentencePeriod".Localized());

            if (AverageVaginalLooseness() == 4)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.PerksOfHaving".Localized());
            else
                if (AverageVaginalLooseness() < 4)
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.NotExperienced".Localized());
                else
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.BigThingWould".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.AllYouHaveTo".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.StillSmellsLike".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.GushesDrenching".Localized());
            else
                if (Player.Wetness() >= 3)
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.DroolsRivulets".Localized());
                else
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.DrenchesAnew".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TemptedToDrop".Localized());

            if (!Player.HasTailInsteadOfLegs() && !Player.IsGoo())
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.YourKnees".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TheGround".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TakeIntoMouth".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TasteWontDo".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.YourDick".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.PluralS".Localized());

                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TwitchWord".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.EsSuffix".Localized());

                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.EagerBeats".Localized(
                    Player.CockHead(Player.BiggestCockIndex())
                ));
            }

            if (AverageVaginalLooseness() >= 5)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.EasingTremendous".Localized());
            else
                if (AverageVaginalLooseness() >= 1)
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.FirstAttemptGuide".Localized());
                else
                {
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.FirstAttemptForce".Localized());

                    if (Player.Vaginas.Count == 0 && Player.Vaginas[0].Virgin)
                        OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.LookingSuffix".Localized());

                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.GatesMeets".Localized());

                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.RepeatsAction".Localized());

                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.NeedfulVacuum".Localized());
                }

            ShowImage("minotaurlord-vag");

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.ThreeRings".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.StimulatingBase".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.ReversesDirection".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.WarmPreCum".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.HardToThink".Localized());

            if (Player.LustPercent >= 80)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.NeededMuchHelp".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.PotentFluids".Localized());

            if (Player.IsBiped() || Player.IsTaur())
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.BetweenLegs".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.InYourLoins".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TooDemanding".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.QuietPop".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.BullKingGroaning".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.InItsEntirety".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.HipsLift".Localized());

            if (Player.Tallness <= 50 && Player.Thickness <= 4)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.SkinShines".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.SentencePeriodAgain".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.HandOnYour".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.SlapsFingertips".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.NoRespite".Localized());

            if (Player.IsLactating())
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TricknipplesSpraying".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.DenyingChance".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.GivenItselfOver".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.SwollenFeel".Localized());

            if (Player.Pregnancy.Incubation != 0)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.PastCervix".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TearOpenWomb".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.InsidesStretch".Localized());

            if (AverageVaginalLooseness() >= 5)
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.IsntFarTruth".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.AccommodateSwelling".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.FeelItSurging".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.PluralSAgain".Localized());

                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.SentencePeriodThird".Localized());
            }

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.CumUnbelievable".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.TouchesFall".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.NearlyDontMakeIt".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.AkimboWord".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.ArtificialEuphoria".Localized());

            OutputText("Scenes.Minotaurkingscenes.Mechanicalbullhue.FantasyInMind".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Lust, 5f));

            Player.CreateStatusEffect(LethiceEnemy.MinotaurKingsTouchId, 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingsTouch, 1);

            Player.MinoCumAddiction(20);

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation - 48);

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void TitfuckCowslut()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingsslutFucked, 1);

            ShowImage("minotaurlord-cow");

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.ForgetBeefcake".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.BarelyConscious".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.QuiveringCowMoans".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.MilkNotPerfect".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.TwitchesVelvety".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.SlutLoving".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.NoMore".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.GoldLips".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.DesireWells".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.GrabsHold".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.DoesntMatter".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.LosesTrack".Localized());

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.GoesOffSlam".Localized());

            if (Player.CumQ() <= 500)
                OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.MostFallShort".Localized());
            else
                if (Player.CumQ() <= 1500)
                    OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.FallOnExcellia".Localized());
                else
                {
                    OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.TongueNeverStood".Localized());

                    if (Player.CumQ() >= 5000)
                        OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.BreastsBasically".Localized());
                }

            OutputText("Scenes.Minotaurkingscenes.TitfuckCowslut.GotWorkToDo".Localized());

            Player.Orgasm("Dick");

            Player.Health = Player.MaxHealth;

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void SloppySeconds()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingsslutFucked, 1);

            var x = Player.BiggestCockIndex() + 1;

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.WhyResort".Localized());

            if (Player.HasPerk(PerkIds.MinotaurCumAddict))
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.WonderfulWord".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.SortOfNice".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.MightHaveSecond".Localized(
                x
            ));

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.DreamilyCoos".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.BallsFullOfCum".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.BackedUpLoad".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.MuchPoint".Localized(
                x
            ));

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.ExcelliaMoans".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.SavageTugTail".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.BallWord".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.AssWord".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.SlappingImpact".Localized());

            if (Player.BiggestCockLength() < 18)
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.DontThinkShe".Localized());
            else
                if (Player.BiggestCockLength() <= 30)
                    OutputText("Scenes.Minotaurkingscenes.SloppySeconds.DidntThinkShed".Localized());
                else
                    OutputText("Scenes.Minotaurkingscenes.SloppySeconds.DontThinkSheAgain".Localized());

            ShowImage("minotaurlord-excelia-sex");

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.SeeWhyOvergrown".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.QuiveringCunt".Localized(
                x
            ));

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.CouldLoveAnyone".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.ElectricBliss".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.WantsToStop".Localized(
                x
            ));

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.GetOffMore".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.LightShimmers".Localized());

            if (Player.Cocks.Count == 2)
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.OtherPenisSliding".Localized());
            else
                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Minotaurkingscenes.SloppySeconds.OtherPenisesSliding".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.IsntDesire".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.MoosLoudly".Localized());

            if (Player.HasKnot())
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.NotMuchPoint".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.YourWord".Localized());

            if (Player.IsBiped() || Player.IsTaur())
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.WobbleWord".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.WobblesWord".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.WeaklyBeneath".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.ParagraphBreak".Localized());

            if (Player.HasKnot() && Player.CumQ() >= 5000)
                OutputText("Scenes.Minotaurkingscenes.SloppySeconds.BellyBulgesInto".Localized());
            else
                if (Player.CumQ() >= 5000 && !Player.HasKnot())
                    OutputText("Scenes.Minotaurkingscenes.SloppySeconds.MuscularControl".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.PivotingPrick".Localized(
                x
            ));

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.NotSureHowMany".Localized());

            OutputText("Scenes.Minotaurkingscenes.SloppySeconds.LethiceBored".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public void HailToTheKingBaby(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            ShowImage("monster-minotaurlord");

            var mk = ((MinotaurKingEnemy)CurrentMonster);

            if (!hpVictory)
            {
                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.DeterminationBurns".Localized());

                if (mk.Orgasms > 0)
                    OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.SoWetJuices".Localized());

                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.FillsVision".Localized());

                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.GrabsHold".Localized());
            }
            else
            {
                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.BeatenStayingUpright".Localized());

                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.GrabsHoldAgain".Localized());
            }

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.DragsUp".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.WantedToFight".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.CockPastTemple".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.PrecumSweatSkin".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.OnlyOneWay".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.PushesBack".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.TriumphantCroons".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.HardlyDisagree".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.LeaningForward".Localized());

            if (Player.HasPerk(PerkIds.BimboBrains) || Player.Corruption >= 66)
                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.LongPast".Localized());
            else
                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.GagReflex".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.FlaringNostrils".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.HeadOfMinotaur".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.NoLongerPressure".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.NotLeftWaiting".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.LurchesForward".Localized());

            if (Player.BiggestBreastSize() >= 5)
                OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.JigglingTits".Localized());
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.OozingHalfHard".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.PluralS".Localized());

                    OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.FlailComically".Localized());
                }
                else
                    OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.ShakingWholeBody".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.FlareGrowsFatter".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.OrgasmRocks".Localized());

            OutputText("Scenes.Minotaurkingscenes.HailToTheKingBaby.EverythingHazy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), Gurglegurgle);
        }

        public void Gurglegurgle()
        {
            ClearOutput();

            ShowImage("badend-minotaurlord-gurgle");

            OutputText("Scenes.Minotaurkingscenes.Gurglegurgle.GratefulFor".Localized());

            OutputText("Scenes.Minotaurkingscenes.Gurglegurgle.WonderfulEvenWhen".Localized());

            OutputText("Scenes.Minotaurkingscenes.Gurglegurgle.SoWhatIf".Localized());

            OutputText("Scenes.Minotaurkingscenes.Gurglegurgle.RequestsSomeone".Localized());

            GameOver();
        }

        public void Leavethem()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.MinotaurkingAlive, 1);

            OutputText("Scenes.Minotaurkingscenes.LeaveThem.SquaresOff".Localized());

            if (!Player.Weapon.IsUnarmed())
                OutputText("Scenes.Minotaurkingscenes.LeaveThem.WeaponInHand".Localized());
            else
                if (Player.Weapon.IsFist())
                    OutputText("Scenes.Minotaurkingscenes.LeaveThem.BalledFists".Localized());
                else
                    OutputText("Scenes.Minotaurkingscenes.LeaveThem.WeaponRaised".Localized());

            OutputText("Scenes.Minotaurkingscenes.LeaveThem.RejectsOffer".Localized());

            if (Player.HasPerk(PerkIds.BimboBrains))
                OutputText("Scenes.Minotaurkingscenes.LeaveThem.HornyBesides".Localized());
            else
                if (Player.Corruption >= 66)
                    OutputText("Scenes.Minotaurkingscenes.LeaveThem.OhComeOn".Localized());
                else
                    if (Player.Corruption <= 20)
                        OutputText("Scenes.Minotaurkingscenes.LeaveThem.WhyDebase".Localized());
                    else
                        OutputText("Scenes.Minotaurkingscenes.LeaveThem.NotEnslaved".Localized());

            CombatScene.CleanupAfterCombat();

            RoomDungeon.ResumeFromFight();
        }

        public static readonly Identifier MinotaurKingMuskId = new("coc", "minotaur_king_musk");

        public static float AverageVaginalLooseness()
        {
            var total = 0f;

            foreach (var vagina in Player.Vaginas)
                total += vagina.Looseness;

            return Player.Vaginas.Count == 0 ? 0f : total / Player.Vaginas.Count;
        }

        [Register("coc:keep_hook_minotaurking")]
        public static void KeepHookMinotaurKing() =>

            SceneInstance<MinotaurKingScenes>.Get().EncounterMinotaurKing();
    }
}