using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Pregnancy;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<HellHoundScene>("coc:hellhound")]
    public class HellHoundScene : Scene
    {

        public override void Entry()
        {
            HellhoundEncounter();
        }

        public void HellhoundEncounter()
        {
            ClearOutput();

            ShowImage("monster-hellhound");

            OutputText("Scenes.Hellhoundscene.HellhoundEncounter.FieryHowl".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryHellhounds);

            var hound = EnemyRegistry.Create(EnemyIds.HellHound);

            hound.OnDefeated = hpVictory =>
            {
                if (hpVictory)
                    CombatScene.CleanupAfterCombat();
                else
                    HellHoundGetsRaped();
            };

            hound.OnWon = hpVictory => HellhoundRapesPlayer();

            CombatScene.Start(hound, () => Go<CampScene>());
        }

        public void HellhoundRapesPlayer()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.OvercomeDesire".Localized());
            else
                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.TooWeak".Localized());

            OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.GazesAtYou".Localized());

            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Hoofed)
                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.HocksAndKnees".Localized());
            else
                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.HandsAndKnees".Localized());

            OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.PositionsBehind".Localized());

            if (Player.HasVagina() && !Player.HasTailInsteadOfLegs())
            {
                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.TwinDicks".Localized(
                     Player.VaginaDescript(0)
                ));

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true);
            }
            else
                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.LowerDick".Localized());

            Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, false);

            if (Player.HasVagina() && !Player.HasTailInsteadOfLegs())
            {
                ShowImage("hellhound-loss-vag");

                if (Player.VaginalCapacity() < CombatController.Instance!.Context.Enemy.CockArea(0))
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.IntenseMixture".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.LowerBodyExplodes".Localized(
                         Player.VaginaDescript(0)
                    ));

                if (Player.Vaginas[0].Virgin)
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.NoLongerVirgin".Localized());

                if (Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), false))
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.BeastHowls".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.PantsFew".Localized());

                if (Player.Vaginas[0].Wetness < 2)
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.PauseGivesTime".Localized(
                         Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.StartsPumping".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.ReachesClimax".Localized());

                if (Player.Corruption < 40)
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.MoansInsides".Localized(
                         Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.FlamingSeed".Localized(
                         Player.VaginaDescript(0)
                    ));

                Player.KnockUp(PregnancyIds.HellHound, PregnancyIds.HellHoundIncubation, 101);

                Player.Orgasm("Vaginal", false);
            }
            else
            {
                ShowImage("hellhound-loss-butt");

                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.LowerBodyExplodesAgain".Localized(
                     Player.AssholeDescript()
                ));

                OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.ReachesClimaxAgain".Localized());

                if (Player.Corruption < 40)
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.MoansTrying".Localized());
                else
                    OutputText("Scenes.Hellhoundscene.HellhoundRapesPlayer.FlamingSeedAgain".Localized());
            }

            Player.SlimeFeed();

            Player.Orgasm("Anal");

            if (Player.Corruption < 40)
                Player.ApplyStats(new DynStat(GameStats.Toughness, -2f), new DynStat(GameStats.Corruption, 1f));
            else
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1.5f));

            CombatScene.CleanupAfterCombat();
        }

        public void HellHoundGetsRaped()
        {
            ClearOutput();

            if (Player.Gender == Gender.Hermaphrodite)
                ShowImage("hellhound-win-herm");
            else
                if (Player.Gender == Gender.Male)
                    ShowImage("hellhound-win-male");
                else
                    ShowImage("hellhound-win-female");

            if (Player.Cocks.Count == 1 && Player.Vaginas.Count == 0)
                OutputText("Scenes.Hellhoundscene.HellHoundGetsRaped.TwinHeads".Localized(
                     Player.CockDescript(0)
                ));

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Hellhoundscene.HellHoundGetsRaped.TwinHeadsAgain".Localized(
                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

            if (Player.Gender == Gender.Hermaphrodite && Player.Cocks.Count == 1)
                OutputText("Scenes.Hellhoundscene.HellHoundGetsRaped.HeadStartsEagerly".Localized(
                     Player.CockDescript(0),

                     Player.VaginaDescript(0),

                     Player.VaginaDescript(0)
                ));

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Hellhoundscene.HellHoundGetsRaped.HeadStartsEagerlyAgain".Localized(
                     Player.CockDescript(0),

                     Player.CockDescript(1),

                     Player.CockDescript(1)
                ));

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Hellhoundscene.HellHoundGetsRaped.TwoHeadsStare".Localized());
            else
            {
                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Corruption, 0.5f));
            }

            CombatScene.CleanupAfterCombat();
        }

        public void HellHoundMasterEncounter()
        {
            ClearOutput();

            ShowImage("area-mountain");

            if (CoC.Instance.Flags.Get(GameFlagIds.HellhoundMasterProgress) == 0)
            {
                OutputText("Scenes.Hellhoundscene.HellHoundMasterEncounter.WanderingMountains".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => AcceptCrazyPresences()); AddButton(1, "No", () => DeclineCrazyPresences());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.HellhoundMasterProgress) == 1)
                {
                    OutputText("Scenes.Hellhoundscene.HellHoundMasterEncounter.WanderingMountainsAgain".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), () => GiveUpLethicite()); AddButton(1, "No", () => DeclineGivingAwayLethicite());
                }
        }

        public void DeclineCrazyPresences()
        {
            ClearOutput();

            OutputText("Scenes.Hellhoundscene.DeclineCrazyPresences.ForcesPresenceOut".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AcceptCrazyPresences()
        {
            ClearOutput();

            ShowImage("hellhound-crazyPresences");

            OutputText("Scenes.Hellhoundscene.AcceptCrazyPresences.RelaxesMind".Localized());

            OutputText("Scenes.Hellhoundscene.AcceptCrazyPresences.SmilesSight".Localized());

            OutputText("Scenes.Hellhoundscene.AcceptCrazyPresences.ContinuesTrek".Localized());

            DoNext(() => AcceptCrazyPresencesII());
        }

        public void AcceptCrazyPresencesII()
        {
            ClearOutput();

            OutputText("Scenes.Hellhoundscene.AcceptCrazyPresencesII.FortInterior".Localized());

            ShowImage("hellhound-master");

            OutputText("Scenes.Hellhoundscene.AcceptCrazyPresencesII.MovesThroughKennels".Localized());

            OutputText("Scenes.Hellhoundscene.AcceptCrazyPresencesII.GoodLook".Localized());

            OutputText("Scenes.Hellhoundscene.AcceptCrazyPresencesII.SomeTimeLater".Localized());

            if (Player.HasKeyItem("Marae's Lethicite") && Player.KeyItemV2("Marae's Lethicite") < 3)
                OutputText("Scenes.Hellhoundscene.AcceptCrazyPresencesII.ExtractsLethicite".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HellhoundMasterProgress, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DeclineGivingAwayLethicite()
        {
            ClearOutput();

            OutputText("Scenes.Hellhoundscene.DeclineGivingAwayLethicite.PushesPresenceOut".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveUpLethicite()
        {
            ClearOutput();

            OutputText("Scenes.Hellhoundscene.GiveUpLethicite.AllowsMaster".Localized());

            OutputText("Scenes.Hellhoundscene.GiveUpLethicite.SuccubusGuard".Localized());

            ShowImage("hellhound-master");

            OutputText("Scenes.Hellhoundscene.GiveUpLethicite.BarelySpareThought".Localized());

            ClearButtons();

            if (Player.KeyItemV2("Marae's Lethicite") == 0)
                AddButton(0, "Common.Button.GiveAll".Localized(), GiveALLTHELETHICITES);
            else
                AddButton(0, "Common.Button.GiveAll".Localized(), null);

            AddButton(1, "Common.Button.GivePart".Localized(), GiveLethicitePiece);
        }

        public void GiveLethicitePiece()
        {
            ClearOutput();

            OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.ProducesCrystal".Localized());

            if (Player.KeyItemV1("Marae's Lethicite") == 2)
            {
                OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.LooksAtCrystal".Localized());

                Player.AddKeyValue("Marae's Lethicite", 1, -1);
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.MaraeLethicite, CoC.Instance.Flags.Get(GameFlagIds.MaraeLethicite) - 1);

                Player.AddKeyValue("Marae's Lethicite", 1, -1);
            }

            OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.RollsCrystal".Localized());

            OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.ExtractsCup".Localized());

            OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.QuiteThePool".Localized());

            OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.PantsWaits".Localized());

            ShowImage("hellhound-hellfire");

            OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.NoticesWhat".Localized());

            OutputText("Scenes.Hellhoundscene.GiveLethicitePiece.PowerBanner".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Lust, 30f), new DynStat(GameStats.Corruption, 6f));

            if (!Player.HasPerk(PerkIds.Hellfire))
                Player.CreatePerk(PerkIds.Hellfire, 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HellhoundMasterProgress, CoC.Instance.Flags.Get(GameFlagIds.HellhoundMasterProgress) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void GiveALLTHELETHICITES()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.PullsGiantCrystal".Localized());

            OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.ChucklesFlares".Localized());

            if (Player.DogCocks() >= 2)
            {
                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.HoundClearly".Localized());

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.CremeraTurns".Localized());

                if (Player.Cocks.Count > 2 || Player.CockThatFits(60) < 0)
                {
                    OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.ReachesWaist".Localized());

                    Player.Cocks[0].Id = BodyPartTypes.Dog;

                    Player.Cocks[1].Id = BodyPartTypes.Dog;

                    Player.Cocks[0].Thickness = 3;

                    Player.Cocks[1].Thickness = 3;

                    Player.Cocks[0].Length = 14;

                    Player.Cocks[1].Length = 14;

                    while (Player.Cocks.Count > 2)
                    {
                        Player.RemoveCock(2, 1);
                    }
                }

                ShowImage("badend-hellhound-male");

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.PouncesWithoutHesitation".Localized());

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.MatterOfMoments".Localized(
                     Player.AssDescript()
                ));

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.FeelsLikeHours".Localized());

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.VoiceCalls".Localized());
            }
            else
            {
                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.HoundClearlyAgain".Localized());

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.CimerMoves".Localized(
                     Player.VaginaDescript(0)
                ));

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.StepsBehind".Localized(
                     Player.VaginaDescript(0)
                ));

                ShowImage("badend-hellhound-female");

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.BarelyRegister".Localized());

                if (Player.VaginalCapacity() < 45)
                {
                    if (Player.Vaginas[0].Virgin)
                        OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.RelievingHoles".Localized());
                    else
                        OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.YLetter".Localized());

                    Player.Vaginas[0].Looseness = 3;

                    OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.TwoCuntsStretch".Localized());
                }

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.MoanOfPleasure".Localized());

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.FeelsCimer".Localized(
                     Player.ButtDescript()
                ));

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.HoursOfPleasure".Localized());

                OutputText("Scenes.Hellhoundscene.GiveALLTHELETHICITES.VoiceCallsAgain".Localized(
                     Player.VaginaDescript(0)
                ));
            }

            DoNext(() => BadEndPartTwo());
        }

        public void BadEndPartTwo()
        {
            ClearOutput();

            ShowImage("badend-hellhound-cerberus");

            OutputText("Scenes.Hellhoundscene.BadEndPartTwo.InclinedToAgree".Localized());

            OutputText("Scenes.Hellhoundscene.BadEndPartTwo.PowerfulDemon".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 50f), new DynStat(GameStats.Corruption, 100f));

            GameOver();
        }

        public void HellHoundPropahRape()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.FlamesDim".Localized());
            else
                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.UnableBearHurt".Localized());

            OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.CaressesPair".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.SpringsToLife".Localized());
            else
                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.LeakPlenty".Localized());

            if (!Player.IsCorruptEnough(20))
            {
                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.JuiceBurns".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 0f));

                CombatScene.CleanupAfterCombat();

                return ;
            }
            else
            {
                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.MuskyScent".Localized(
                     Player.VaginaDescript(0)
                ));

                Player.CuntChange(CombatController.Instance!.Context.Enemy.CockArea(0), true, false, true);

                Player.ButtChange(CombatController.Instance!.Context.Enemy.CockArea(1), true, false, true);

                if (!Player.IsCorruptEnough(40) && !Player.HasPerk(PerkIds.Masochist))
                {
                    OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.BottomsOut".Localized());

                    Player.HealthChange(-20);

                    Player.ApplyStats(new DynStat(GameStats.Lust, -99f));

                    CombatScene.CleanupAfterCombat();

                    return ;
                }

                ShowImage("hellhound-win-vag");

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.MagicalFlames".Localized());

                if (Player.BiggestBreastSize() > 1)
                    OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.ChestJiggle".Localized());

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.RidingHound".Localized(
                     Player.VaginaDescript(0)
                ));

                if (Player.HasCock())
                {
                    OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.CanineInAnus".Localized());

                    if (Player.Cocks.Count == 1)
                        OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.CockReacts".Localized());
                    else
                        OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.EachCockReacts".Localized());
                }

                if (Player.Balls > 0)
                    OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.WheneverLower".Localized());

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.ParagraphBreak".Localized());

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.WayTooEarly".Localized());

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.SlowlySteadily".Localized());

                if (Player.Cocks.Count > 0)
                {
                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.CockTwitches".Localized());
                    else
                        OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.CocksTwitch".Localized());

                    OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.KeepsMotions".Localized());

                    if (Player.CumQ() > 700)
                        OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.OrgasmDrags".Localized());
                }

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.ParagraphBreakTwo".Localized());

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.BusyWithOrgasm".Localized());

                OutputText("Scenes.Hellhoundscene.HellHoundPropahRape.FinallyFinished".Localized());

                Player.Orgasm("Vaginal");

                Player.Orgasm("Anal", false);

                if (!Player.IsCorruptEnough(40))
                    Player.ApplyStats(new DynStat(GameStats.Toughness, -2f), new DynStat(GameStats.Corruption, 1f));
                else
                    Player.ApplyStats(new DynStat(GameStats.Corruption, 1.5f));

                Player.KnockUp(PregnancyIds.HellHound, PregnancyIds.HellHoundIncubation, 101);
            }

            CombatScene.CleanupAfterCombat();
        }

    }
}