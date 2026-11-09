using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Achievements;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Desert
{
    [Register<AntsScene>("coc:ants")]
    public class AntsScene : Scene
    {

        public bool PlayingHero = false;

        public override void Entry()
        {
            PhyllaWaifu();
        }

        public bool PhyllaWaifu()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.AntWaifu) > 0;
        }

        public void AntColonyEncounter()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) - CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) >= 2 && CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) >= 4 && (int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.PhyllaGemsHuntedToday) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaStayHome) > 0)
                    BumpIntoTheAntColonyAfterStayHomePhylla();
                else
                    AntGirlGoodEnd();
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PcReadyForAntColonyChallenge) == 1)
                    AntColonyChallenge();
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaSaved) == 1)
                        EnterTheColony();
                    else
                        FirstAntColonyEncounter();
        }

        public float PhyllaCapacity()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.PhyllaCapacity);
        }

        public void FirstAntColonyEncounter()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.WastelandWander".Localized());

            if (Player.Corruption > 75 && Player.LustPercent > 50)
                OutputText("Scenes.Antsscene.FirstAntColonyEncounter.SlakeUnquenchableLust".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.AbandonedMerchantCart".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.CartCrashStartle".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.AntClimbsFromCart".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.FourArmedRevelation".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.LeanMuscularFrame".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.BobbingAbdomen".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.BlockedChestView".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.TangledSandyHair".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.DemonsCrestDune".Localized());

            OutputText("Scenes.Antsscene.FirstAntColonyEncounter.WatchOrIntervene".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.PlayHero".Localized(), PlayHero);

            AddButton(1, "Common.Button.KeepHidden".Localized(), KeepHidden);
        }

        public void KeepHidden()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.AntColonyKeptHidden, 1);

            if ((Player.LibidoPercent < 41 && Player.LustPercent < 50) || !Player.IsCorruptEnough(66) || Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Antsscene.KeepHidden.DecisionToHide".Localized());

                OutputText("Scenes.Antsscene.KeepHidden.FleeWhileDistracted".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }
            else
                if (Player.HasCock())
                {
                    ShowImage("ants-desert-male-firstencounter");

                    OutputText("Scenes.Antsscene.KeepHidden.LustfulAnticipation".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.StrokeWhileObserving".Localized());

                    var x = Player.BiggestCockIndex();

                    if (Player.Cocks[x].Length >= 20)
                        OutputText("Scenes.Antsscene.KeepHidden.LickCockHead".Localized(
                             x + 1
                        ));
                    else
                        if (Player.CockArea(x) < 6)
                            OutputText("Scenes.Antsscene.KeepHidden.ModestCockBoast".Localized());
                        else
                            if (Player.CockArea(x) < 12)
                                OutputText("Scenes.Antsscene.KeepHidden.SwordHiltGrip".Localized());
                            else
                                if (Player.CockArea(x) < 30)
                                    OutputText("Scenes.Antsscene.KeepHidden.FearOfDiscovery".Localized());
                                else
                                    OutputText("Scenes.Antsscene.KeepHidden.BetrayingErection".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.AntGirlResists".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.HandsomeDemonApproaches".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.ForcedFaceEntry".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.SilencedScreams".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.BlowjobRoarWithdrawal".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.EnviousDemonGriping".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.StrokeOnOutcrop".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.FlippedBeforeClimax".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.LeaderTeasesSlit".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.HeldFirmWrithing".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.HermaphroditeInterrupts".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.HermCrammingStruggle".Localized(
                         Player.SMultiCockDesc()
                    ));

                    if (Player.CumQ() < 100)
                        OutputText("Scenes.Antsscene.KeepHidden.SizableWord".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Antsscene.KeepHidden.HugeWord".Localized());
                        else
                            if (Player.CumQ() < 10000)
                                OutputText("Scenes.Antsscene.KeepHidden.MassiveWord".Localized());
                            else
                                OutputText("Scenes.Antsscene.KeepHidden.TitanicWord".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.RefocusAfterClimax".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.SatisfiedSlumber".Localized());

                    Player.Orgasm("Dick");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 3f));
                }
                else
                {
                    ShowImage("ants-desert-female-firstencounter");

                    OutputText("Scenes.Antsscene.KeepHidden.MoistenWhileWatching".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.QuietlyStripArmor".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.CaressBreastsQuietly".Localized());

                    if (Player.LactationQ() > 100)
                        OutputText("Scenes.Antsscene.KeepHidden.WarmMilkFlow".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Antsscene.KeepHidden.ThrobbingBetweenLegs".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.FingersOnVulva".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.ForcedThroatFuck".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.CumDumpsterDoggy".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.DevilishRealization".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.FasterHermPumping".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.VirginityRealization".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.NextDemonFingering".Localized());

                    OutputText("Scenes.Antsscene.KeepHidden.AwakeFromDoze".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 3f));
                }

            DoNext(() => ReturnToCamp(1));
        }

        public void PlayHero()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.PlayHero.BurstFromHiding".Localized());

            OutputText("Scenes.Antsscene.PlayHero.FightingDemons".Localized());

            var pack = EnemyRegistry.Create(EnemyIds.DemonPack);

            pack.OnDefeated = (hpVictory) => DemonPackDefeated(hpVictory);

            PlayingHero = true;

            CombatScene.Start(pack, () => Go<CampScene>());
        }

        public void DemonPackDefeated(dynamic hpVictory)
        {
            ClearOutput();

            PlayingHero = false;

            if (hpVictory)
            {
                OutputText("Scenes.Antsscene.DemonPackDefeated.LastDemonFalls".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 1f));
            }
            else
                OutputText("Scenes.Antsscene.DemonPackDefeated.DemonsTouchAdmiringly".Localized());

            DoNext(() => ConsolePhylla());
        }

        public void ConsolePhylla()
        {
            ClearOutput();

            ShowImage("ant-desesrt-console-phylla");

            OutputText("Scenes.Antsscene.ConsolePhylla.DemonsFleeSurvey".Localized());

            if (Player.Libido >= 50)
                OutputText("Scenes.Antsscene.ConsolePhylla.EnticingPussyGlimpse".Localized());
            else
                OutputText("Scenes.Antsscene.ConsolePhylla.YLetter".Localized());

            OutputText("Scenes.Antsscene.ConsolePhylla.TapShakingAbdomen".Localized());

            OutputText("Scenes.Antsscene.ConsolePhylla.PhyllaCrawlsOut".Localized());

            OutputText("Scenes.Antsscene.ConsolePhylla.AttemptedIntroduction".Localized());

            OutputText("Scenes.Antsscene.ConsolePhylla.BorrowedMemories".Localized());

            OutputText("Scenes.Antsscene.ConsolePhylla.StaggeringCatch".Localized());

            OutputText("Scenes.Antsscene.ConsolePhylla.LoudSighSurprise".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaSaved, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void EnterTheColony()
        {
            ClearOutput();

            ShowImage("ant-desert-colony");

            OutputText("Scenes.Antsscene.EnterTheColony.MemoryGuidedMarch".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.AmbushAtCrest".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.CrepuscularCaveDescent".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.QueenOnThrone".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.QueenToneSpeech".Localized());

            if (Player.IsPureEnough(40))
                OutputText("Scenes.Antsscene.EnterTheColony.ChampionMissionSpeech".Localized());
            else
                OutputText("Scenes.Antsscene.EnterTheColony.TaintAndValor".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.SavedDaughterRecognition".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.DaggerGlareAtDaughter".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.EggSpillsDuringSpeech".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.ChyllaStandsUpright".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.CapturedBeastsSpeech".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.PauseToDeter".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.ArenaSendOff".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.DismissiveWave".Localized());

            OutputText("Scenes.Antsscene.EnterTheColony.OminousReturnGuide".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PcReadyForAntColonyChallenge, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AntColonyChallenge()
        {
            ClearOutput();

            ShowImage("ant-desert-colony");

            if (CoC.Instance.Flags.Get(GameFlagIds.MetAntArena) == 0)
            {
                OutputText("Scenes.Antsscene.AntColonyChallenge.UnbiddenFeetReturn".Localized());

                OutputText("Scenes.Antsscene.AntColonyChallenge.ShieldedEyesTurn".Localized());

                OutputText("Scenes.Antsscene.AntColonyChallenge.MutteringSoldier".Localized());

                if (silly)
                    OutputText("Scenes.Antsscene.AntColonyChallenge.MissingFromList".Localized());
                else
                    if (Player.Strength > 50)
                        OutputText("Scenes.Antsscene.AntColonyChallenge.LooksThePartRemark".Localized());
                    else
                        if (Player.Intelligence > 50)
                            OutputText("Scenes.Antsscene.AntColonyChallenge.WittyPuppyEyesQuip".Localized());

                OutputText("Scenes.Antsscene.AntColonyChallenge.ChartScribble".Localized());

                OutputText("Scenes.Antsscene.AntColonyChallenge.ReadyToBeginRemark".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MetAntArena, CoC.Instance.Flags.Get(GameFlagIds.MetAntArena) + 1);
            }
            else
            {
                OutputText("Scenes.Antsscene.AntColonyChallenge.ReturnToColony".Localized());

                OutputText("Scenes.Antsscene.AntColonyChallenge.CowardAccusation".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), AntColiseumFight);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveAntColony);
        }

        public void LeaveAntColony()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.LeaveAntColony.BetterPrepareLater".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void AntColiseumFight()
        {
            ClearOutput();

            ShowImage("ant-desert-coliseum");

            if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) + CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) == 0)
            {
                OutputText("Scenes.Antsscene.AntColiseumFight.ReadyAnnouncement".Localized());

                OutputText("Scenes.Antsscene.AntColiseumFight.ShamblingInterrupt".Localized());

                OutputText("Scenes.Antsscene.AntColiseumFight.TentacleBeastFight".Localized());

                var tenty = new TentacleBeastEnemy
                {
                    OnDefeated = (hpVictory) => PhyllaTentacleDefeat(hpVictory),

                    OnWon = (hpVictory) => PhyllaTentaclePCLoss(hpVictory)
                };

                CombatScene.Start(tenty, () => Go<CampScene>());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) + CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) == 1)
                {
                    OutputText("Scenes.Antsscene.AntColiseumFight.ColosseumArrival".Localized());

                    OutputText("Scenes.Antsscene.AntColiseumFight.MinotaurAxeFight".Localized());

                    var mino = new MinotaurEnemy
                    {
                        OnDefeated = (hpVictory) => PhyllaBeatAMino(hpVictory),

                        OnWon = (hpVictory) => PhyllaPCLostToMino(hpVictory)
                    };

                    CombatScene.Start(mino, () => Go<CampScene>());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.MetAntArenaGnoll) == 0)
                        OutputText("Scenes.Antsscene.AntColiseumFight.StagingAreaLead".Localized());
                    else
                        OutputText("Scenes.Antsscene.AntColiseumFight.TunnelNetworkReturn".Localized());

                    OutputText("Scenes.Antsscene.AntColiseumFight.GnollFight".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.MetAntArenaGnoll, CoC.Instance.Flags.Get(GameFlagIds.MetAntArenaGnoll) + 1);

                    var gnoll = EnemyRegistry.Create(EnemyIds.Gnoll);

                    gnoll.OnDefeated = (hpVictory) => PhyllaPCBeatsGnoll(hpVictory);

                    gnoll.OnWon = (hpVictory) => PhyllaGnollBeatsPC(hpVictory);

                    CombatScene.Start(gnoll, () => Go<CampScene>());
                }

            CurrentMonster.CreateStatusEffect(StatusEffectIds.NoLoot, 0, 0, 0, 0);
        }

        public void PhyllaTentacleDefeat(bool hpVictory)
        {
            ClearOutput();

            if (hpVictory)
            {
                OutputText("Scenes.Antsscene.PhyllaTentacleDefeat.CreatureDeathScreech".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.TentacleBeastsKilled, CoC.Instance.Flags.Get(GameFlagIds.TentacleBeastsKilled) + 1);
            }
            else
                OutputText("Scenes.Antsscene.PhyllaTentacleDefeat.QuiveringTentacleMass".Localized());

            OutputText("Scenes.Antsscene.PhyllaTentacleDefeat.ArenaExitGreeting".Localized());

            OutputText("Scenes.Antsscene.PhyllaTentacleDefeat.ShyCongratulations".Localized());

            OutputText("Scenes.Antsscene.PhyllaTentacleDefeat.SheDressesWounds".Localized());

            OutputText("Scenes.Antsscene.PhyllaTentacleDefeat.RecognizedGuideGuard".Localized());

            Player.ChangeFatigue(10);

            CoC.Instance.Flags.Set(GameFlagIds.AntArenaWins, CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void PhyllaTentaclePCLoss(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            if (hpVictory)
            {
                OutputText("Scenes.Antsscene.PhyllaTentaclePCLoss.WoundedDesperateRun".Localized());

                OutputText("Scenes.Antsscene.PhyllaTentaclePCLoss.ReachNearbyTunnel".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllaTentaclePCLoss.SurrenderToArousal".Localized());

                OutputText("Scenes.Antsscene.PhyllaTentaclePCLoss.VoiceRousesEscape".Localized());
            }

            OutputText("Scenes.Antsscene.PhyllaTentaclePCLoss.ArenaDefeatStopped".Localized());

            OutputText("Scenes.Antsscene.PhyllaTentaclePCLoss.LostDrinkCommand".Localized());

            OutputText("Scenes.Antsscene.PhyllaTentaclePCLoss.VialInquisitiveLook".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AntArenaLosses, CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) > 3)
            {
                AntastrophyBadEnd();

                return;
            }

            CombatScene.CleanupAfterCombat();
        }

        public void PhyllaBeatAMino(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.PhyllaBeatAMino.ArenaExitPhyllaMeet".Localized());

            OutputText("Scenes.Antsscene.PhyllaBeatAMino.HeadRaiseKissNear".Localized());

            OutputText("Scenes.Antsscene.PhyllaBeatAMino.GuideClearsThroat".Localized());

            Player.ChangeFatigue(10);

            CoC.Instance.Flags.Set(GameFlagIds.AntArenaWins, CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void PhyllaPCLostToMino(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.PhyllaPCLostToMino.DefeatGuardsApproach".Localized());

            OutputText("Scenes.Antsscene.PhyllaPCLostToMino.AntCommandIndifference".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AntArenaLosses, CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) > 3)
            {
                AntastrophyBadEnd();

                return;
            }

            CombatScene.CleanupAfterCombat();
        }

        public void PhyllaPCBeatsGnoll(bool hpVictory)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.AntsPcBeatGnoll) == 0)
            {
                OutputText("Scenes.Antsscene.PhyllaPCBeatsGnoll.FoeDefeatedCrowdAware".Localized());

                OutputText("Scenes.Antsscene.PhyllaPCBeatsGnoll.ChampionGirlPraise".Localized());

                OutputText("Scenes.Antsscene.PhyllaPCBeatsGnoll.FingerLipsShush".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.AntsPcBeatGnoll, CoC.Instance.Flags.Get(GameFlagIds.AntsPcBeatGnoll) + 1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) - CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) >= 2 && CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) >= 3 && (int)Player.Gender > 0)
                {
                    CombatController.InCombat = false;

                    CoC.Instance.Flags.Set(GameFlagIds.AntArenaWins, CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) + 1);

                    CombatScene.CleanupAfterCombat();

                    return;
                }
                else
                {
                    OutputText("Scenes.Antsscene.PhyllaPCBeatsGnoll.RepeatVictoryCrowd".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) - CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) >= 2 && CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) >= 3)
                    {
                        OutputText("Scenes.Antsscene.PhyllaPCBeatsGnoll.FeatsEarnFavor".Localized());

                        OutputText("Scenes.Antsscene.PhyllaPCBeatsGnoll.GenitalLackHesitation".Localized());
                    }
                }

            CoC.Instance.Flags.Set(GameFlagIds.AntArenaWins, CoC.Instance.Flags.Get(GameFlagIds.AntArenaWins) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void PhyllaGnollBeatsPC(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.AntsPcLostToGnoll) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.AntsPcLostToGnoll, CoC.Instance.Flags.Get(GameFlagIds.AntsPcLostToGnoll) + 1);

                OutputText("Scenes.Antsscene.PhyllaGnollBeatsPC.ArenaFloorBooed".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllaGnollBeatsPC.CrowdBooFourGuards".Localized());

                OutputText("Scenes.Antsscene.PhyllaGnollBeatsPC.GrabVialDrink".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.AntArenaLosses, CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AntArenaLosses) > 3)
            {
                AntastrophyBadEnd();

                return;
            }

            CombatScene.CleanupAfterCombat();
        }

        public void AntastrophyBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.ChyllaBodyguardsApproach".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.ChyllaJoinColonyOrder".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.QueenDryVerdict".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.ChooseJoinOverFight".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.DraggedIntoColony".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.NewHomeMealRules".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.RespondInterrupted".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.LeaveThreatThenExit".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.AntBringsClayCup".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.SameRoutineDays".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.EpilogueHeader".Localized());

            OutputText("Scenes.Antsscene.AntastrophyBadEnd.BlissfulColonyLife".Localized());

            GameOver();
        }

        public void AntGirlGoodEnd()
        {
            ClearOutput();

            ShowImage("ant-desert-kissing");

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaCapacity, 50);

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.CrowdSilentPhyllaDescends".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.TunnelDragToQueen".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.QueenPraiseWisdom".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.PrivateChamberRetreat".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.CushionSeating".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.NoddingAgreement".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.HugeSmileGrows".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.BashfulLookAway".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.UncertainRealization".Localized());

            OutputText("Scenes.Antsscene.AntGirlGoodEnd.EyesLockKiss".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.UsePenis".Localized(), GigititigitigitigitigityAntGirl);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.UseVagina".Localized(), FemalePhyllaFirstFuckGooooo);

            AddButton(3, "Common.Button.NotNow".Localized(), DeclineAntSexForNow);

            AddButton(4, "Common.Button.Reject".Localized(), RejectAntSex);
        }

        public void DeclineAntSexForNow()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.DeclineAntSexForNow.PolitelyDeclineToday".Localized());

            OutputText("Scenes.Antsscene.DeclineAntSexForNow.UnderstandQuietSmile".Localized());

            OutputText("Scenes.Antsscene.DeclineAntSexForNow.ReassuringKissDeparture".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaGemsHuntedToday, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void RejectAntSex()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.RejectAntSex.DoorDashRejection".Localized());

            OutputText("Scenes.Antsscene.RejectAntSex.TearsAtRefusal".Localized());

            OutputText("Scenes.Antsscene.RejectAntSex.PartingForeboding".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AntsPcFailedPhylla, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void GigititigitigitigitigityAntGirl()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.ObviousTentDesire".Localized());

            var x = Player.CockThatFits(PhyllaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            if (Player.CockArea(x) < 36)
            {
                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.DelicateFaceCuriosity".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.CannotHandleOne".Localized());
            }
            else
                if (Player.CockArea(x) < 72)
                {
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.TwitchingUnnoticed".Localized());

                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.FourArmsFreeing".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.CannotHandleOneAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.FabricCannotContain".Localized());

                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.QuickSeriousGlance".Localized());
                }

            if (Player.CockArea(x) >= 72)
            {
                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.WarningThenBurst".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.SLetter".Localized());

                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.SentencePeriod".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.OverwhelmedBySight".Localized());

                if (Player.CountCocksOfType("TENTACLE") > 0)
                {
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.WhimCommandTentacle".Localized());

                    if (Player.CountCocksOfType("TENTACLE") > 1)
                        OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.SLetterAgain".Localized());

                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.WrapAndTugClose".Localized());
                }

                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.TeaseReleaseCock".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.SLetterThird".Localized());

                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.HangBeforeHerFace".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.ItPronoun".Localized());
                else
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.ThemTheyPronoun".Localized());

                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.SlightlyIntimidated".Localized());

                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.PeekAroundShock".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.SLetterFourth".Localized());

                OutputText("Scenes.Antsscene.GigititigitigitigitigityAntGirl.SizeableSearchFear".Localized());
            }

            ClearButtons();

            if (Player.CockArea(x) <= PhyllaCapacity())
                AddButton(0, "Common.Button.Next".Localized(), () => MalePhyllaContinuation(x));
            else
                AddButton(0, "Common.Button.Next".Localized(), CuntmuffinLingusPhyllaDickBig);
        }

        public void MalePhyllaContinuation(int x)
        {
            ClearOutput();

            ShowImage("ant-desert-first-male-vagsex");

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.FirstTimeNerves".Localized());

            if (Player.IsPureEnough(75))
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.AssureFirstTime".Localized());
            else
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.PrivateTightExpectation".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.TugPrefix".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.PlayfullyWord".Localized());
            else
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.ForcefullyWord".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.RoyalDressRemoved".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.ClothesTossGuide".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.PresentingBehindHer".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.SlimyLustTraces".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.SentencePeriod".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.VaginaTopTension".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.BetweenMoansPlea".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.HesitantReadiness".Localized());

            if (Player.Corruption >= 75)
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.WetEagerReaping".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.CushionBedGlance".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.TremblingDripping".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.NipplesAndPrefix".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.WetSignsTimidness".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.EntranceDragMoan".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Corruption < 75)
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.GentleForcePrefix".Localized());
            else
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.BrutishForcePrefix".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.TightOpeningBreach".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.VirginityTakenHeader".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.PauseAskWellbeing".Localized());
            else
                OutputText("Scenes.Antsscene.MalePhyllaContinuation.UndeterredDeeperBreach".Localized());

            OutputText("Scenes.Antsscene.MalePhyllaContinuation.StammeredReassurance".Localized());

            ClearButtons();

            if (Player.Corruption < 75)
                AddButton(0, "Common.Button.Next".Localized(), PhyllaFirstTimePureBabiesFuckEnding);
            else
                AddButton(0, "Common.Button.Next".Localized(), PhyllaCorruptMascEnding);
        }

        public void PhyllaFirstTimePureBabiesFuckEnding()
        {
            ClearOutput();

            if (!Player.CanOvipositSpider())
                ShowImage("ant-desert-male-first-pure-vagsex");

            var x = Player.CockThatFits(PhyllaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.AllowPhyllaAdapt".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.SmallerHandsReach".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.BallsToken".Localized());
            else
                OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.SensitiveTaint".Localized());

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.MoansEscapeLips".Localized());

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.PhyllaMoaningMimics".Localized());

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.PhyllaFierceKiss".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.TempoUnfamiliarWarnings".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            if (Player.CumQ() < 50)
                OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.ThinWord".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.HeftyWord".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.ThickWord".Localized());

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.WombEjaculateResponds".Localized());

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.LastStrengthPullOut".Localized());

            if (Player.CumQ() >= 100 && Player.CumQ() < 1000)
                OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.AmazingLoadsBulge".Localized());
            else
                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.NethersMystify".Localized());

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.SpentHeadDrop".Localized());

            if (Player.CanOvipositSpider())
            {
                OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.MaternalFeelingIdea".Localized());

                OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.ExplainEggsHost".Localized());

                if (Player.Intelligence / 20 + Random.Shared.Next(20) + 1 < 16)
                {
                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.PhyllaShakesHead".Localized());

                    if (Player.IsPregnant())
                        OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.CantHoldMore".Localized());

                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.MaybeLater".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.BroodMotherConsider".Localized());

                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.FingerLipsRelax".Localized());

                    if (Player.Corruption >= 75)
                        OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.PhyllaBabyMaker".Localized());

                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.PhyllaHeedsDirections".Localized());

                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.ArmsCompressLubricate".Localized());

                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.SquintsEggBrace".Localized());

                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.PainCriesOut".Localized());

                    OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.ComfortFewMore".Localized());

                    Player.DumpEggs();

                    if (Player.Pregnancy.Incubation == 0)
                    {
                        Player.Pregnancy.Type = Pregnancy.PregnancyIds.DriderEggs;

                        Player.Pregnancy.Incubation = 8 * 24;
                    }
                }
            }

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.LinkFadesApproval".Localized());

            if (CampScene.LoversCount() > 1)
                OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.OtherLoversFeel".Localized());

            OutputText("Scenes.Antsscene.PhyllaFirstTimePureBabiesFuckEnding.DriftOffSleep".Localized());

            Player.Orgasm("Generic");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WaifuQuestOver);
        }

        public void PhyllaCorruptMascEnding()
        {
            ClearOutput();

            ShowImage("ant-desert-male-first-corrupt-vagsex");

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PumpHardRump".Localized());

            if (Player.Tallness >= 72)
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.ChestWord".Localized());
            else
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.BellyWord".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.BraceRigidAbdomen".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.SheAsksNervously".Localized());

            var x = Player.CockThatFits(PhyllaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PlungeSurpriseYell".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Balls == 0)
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.ThighsWord".Localized());
            else
                OutputText(Player.BallsDescriptLight());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.SlapRumpWail".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PoorDearTender".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PhyllaGraspCushions".Localized());

            var y = Player.CockThatFits2(PhyllaCapacity());

            if (Player.CockTotal() < 2)
                y = -1;

            if (y >= 0)
            {
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.OwnPleasureIntent".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.OneFreeCock".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.YourFreeCock".Localized());

                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.TightAssIntruder".Localized());

                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.ProtestBetweenMoans".Localized());

                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PhyllaAnalVirginity".Localized());

                if (Player.BiggestCockArea() > PhyllaCapacity())
                    OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.HandsMouthUse".Localized());
            }

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.ViceLikePressure".Localized());

            if (y >= 0)
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.SLetter".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.DeepPowerControl".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.MinutesHardRemember".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.RoarGodsExplode".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.TwitchLetterS".Localized());
            else
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.TwitchesWord".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.CorruptedSeedWomb".Localized());

            if (y >= 0)
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.AndHerAss".Localized());
            else
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.OntoBedspread".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PulsingCuntSuck".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.BallsWord".Localized());
            else
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.ShaftWord".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PhyllaOwnOrgasm".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.PhyllaWailingMuffled".Localized());

            if (silly)
                OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.OldSayingMonsters".Localized());

            OutputText("Scenes.Antsscene.PhyllaCorruptMascEnding.CorruptingBestRoute".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WaifuQuestOver);
        }

        public void CuntmuffinLingusPhyllaDickBig()
        {
            ClearOutput();

            ShowImage("ant-desert-male-first-cunning");

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.TooLargePenetrate".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.SLetter".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.WontWorkTears".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.VirginNightBust".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.SheUpsetTraditional".Localized());

            if (Player.BiggestCockLength() < 69)
            {
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.AngleHipsCeiling".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.ItPointsCeiling".Localized());
                else
                    OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.TheyPointCeiling".Localized());

                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.CeilingOutOfWay".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.AngleHipsDoorway".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.ItIsDoorway".Localized());
                else
                    OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.TheyAreDoorway".Localized());

                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.DoorwayAimedOut".Localized());
            }

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.BeckonHookedFinger".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.PhyllaGigglesRelief".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.LargerHandsShoulders".Localized());

            if (silly)
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.WhisperSimba".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.GesturesEnormousCock".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.TeasingNoseBridge".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.SwitchesAndBobsPlural".Localized());
            else
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.SwitchAndBobSingular".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.LoudlyBrushing".Localized());

            if (Player.BiggestCockLength() < 69)
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.CeilingWord".Localized());
            else
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.HallwayWall".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.SentencePeriod".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.PhyllaWhispersEar".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.HandsAwayNod".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.FaceGrindThought".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.ClosedEyesMoan".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.TongueRemoveSpread".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Snake)
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.ForkedTongueClit".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.MuffledMoanSpot".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.AttemptToSpeak".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.GrindClitFaster".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.NearPeakFinish".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.TumblingTearsFree".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.CatchBreathHand".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.SquirterVolumeForce".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.SLetterAgain".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.PantsAfterTriathlon".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.ThisMonsterSingular".Localized());
            else
                OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.TheseMonstersPlural".Localized());

            OutputText("Scenes.Antsscene.CuntmuffinLingusPhyllaDickBig.ShylyRemarks".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            CoC.Instance.Flags.Set(GameFlagIds.DidntFuckPhyllaOnRecruitment, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WaifuQuestOver);
        }

        public void FemalePhyllaFirstFuckGooooo()
        {
            ClearOutput();

            ShowImage("ant-desert-female-first-boobsucking");

            OutputText("Scenes.Antsscene.FemalePhyllaFirstFuckGooooo.PhyllaUndressOrder".Localized());

            if (Player.LactationQ() > 250)
                OutputText("Scenes.Antsscene.FemalePhyllaFirstFuckGooooo.MilkDrinkingSensation".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstFuckGooooo.NippleTonguePlay".Localized());

            if (Player.IsGoo())
                OutputText("Scenes.Antsscene.FemalePhyllaFirstFuckGooooo.SlimeMilkQuestion".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstFuckGooooo.SuddenMindSuggestion".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstFuckGooooo.UpsideDownGaze".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstFuckGooooo.PatienceRunsOut".Localized());

            ClearButtons();

            if (!Player.HasCock())
                AddButton(0, "Common.Button.Next".Localized(), GirlFiller);
            else
                AddButton(0, "Common.Button.Next".Localized(), FemalePhyllaFirstTimePlusCock);
        }

        public void GirlFiller()
        {
            ClearOutput();

            ShowImage("ant-desert-female-first-scissoring");

            OutputText("Scenes.Antsscene.GirlFiller.OralAnticipationCoos".Localized());

            OutputText("Scenes.Antsscene.GirlFiller.InvoluntaryWrithing".Localized());

            OutputText("Scenes.Antsscene.GirlFiller.ClitLengthGuess".Localized());

            OutputText("Scenes.Antsscene.GirlFiller.SucklingClitLicks".Localized());

            OutputText("Scenes.Antsscene.GirlFiller.ShockImageTension".Localized());

            OutputText("Scenes.Antsscene.GirlFiller.FaceCleanComplaint".Localized());

            OutputText("Scenes.Antsscene.GirlFiller.StammeredFirstTime".Localized());

            OutputText("Scenes.Antsscene.GirlFiller.ArchingMoanReaction".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => ScissorContinue(true));
        }

        public void FemalePhyllaFirstTimePlusCock()
        {
            ClearOutput();

            if (Player.SmallestCockArea() > PhyllaCapacity())
                ShowImage("ant-desert-female-first-scissoring-with-bigcock");
            else
                ShowImage("ant-desert-female-first-scissoring-with-cock");

            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.OralAnticipationCoos".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.TakenHoldOfFragment".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.YourWord".Localized());
            else
                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.LuckyMemberBasking".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.InvoluntaryWrithing".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.HeadBetweenLegs".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.SucklingClitLicks".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.ShockImageTension".Localized());

            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.EuphoricRollOff".Localized());

            if (Player.SmallestCockArea() > PhyllaCapacity())
            {
                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.PainfulScissorAttempt".Localized(
                     Player.MultiCockDescriptLight(),

                     Player.MultiCockDescriptLight()
                ));

                if (Player.IsTaur())
                    OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.FourArmScramble".Localized());
                else
                    if (Player.IsDrider())
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.CarapaceClitBorder".Localized());
                    else
                        if (Player.IsGoo())
                            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.GoopyMassConnection".Localized());
                        else
                            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.LegCurlFlip".Localized());

                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.ScissorPositioning".Localized());

                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.SizeAweTease".Localized());
            }
            else
                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.PressedCuntScissor".Localized());

            if (Player.SmallestCockArea() > PhyllaCapacity())
                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.PussyKissFluids".Localized());
            else
            {
                OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.CockAttentionHunger".Localized());

                if (Player.CountCocksOfType("TENTACLE") > 0)
                {
                    OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.TentacleDickFragment".Localized());

                    if (Player.CountCocksOfType("TENTACLE") > 1)
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.SLetter".Localized());

                    OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.FirstSightShock".Localized());

                    if (Player.CountCocksOfType("TENTACLE") > 1)
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.SLetterAgain".Localized());

                    OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.BodyCaressTrail".Localized());

                    if (Player.CountCocksOfType("TENTACLE") > 1)
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.SLetterThird".Localized());

                    OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.MotionToPrick".Localized());

                    if (Player.CountCocksOfType("TENTACLE") > 1)
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.WiggleThemPlural".Localized());
                    else
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.WiggleItSingular".Localized());

                    OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.WormBirdTease".Localized());

                    if (Player.CountCocksOfType("TENTACLE") == 1)
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.CockIntoMouth".Localized());
                    else
                        if (Player.CountCocksOfType("TENTACLE") == 2)
                            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.CocksMouthBreasts".Localized());
                        else
                            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.BodyCockWrap".Localized());
                }
                else
                    if (Player.CockTotal() == 2 && (Player.CockArea(Player.SmallestCockIndex2()) + Player.CockArea(Player.SmallestCockIndex()) <= PhyllaCapacity()))
                        OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.ApologeticStroking".Localized());
                    else
                        if (Player.Cocks[Player.SmallestCockIndex()].Length < 4)
                            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.ApologeticStrokingSingular".Localized(
                                 Player.CockDescript(Player.SmallestCockIndex())
                            ));
                        else
                            OutputText("Scenes.Antsscene.FemalePhyllaFirstTimePlusCock.GlansGripStruggle".Localized(
                                 Player.CockDescript(Player.SmallestCockIndex()),

                                 Player.CockDescript(Player.BiggestCockIndex())
                            ));
            }

            ScissorContinue(false);
        }

        public void ScissorContinue(bool clear)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Antsscene.ScissorContinue.ParagraphBreak".Localized());

            OutputText("Scenes.Antsscene.ScissorContinue.TempoForceIncrease".Localized());

            OutputText("Scenes.Antsscene.ScissorContinue.PleasureCollapse".Localized());

            Player.Orgasm("Vaginal");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), WaifuQuestOver);
        }

        public void WaifuQuestOver()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.WaifuQuestOver.WakingUnsureTime".Localized());

            OutputText("Scenes.Antsscene.WaifuQuestOver.FetalSleepNotice".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Come2camp".Localized(), GetAntWaifuYoShit);

            AddButton(1, "Common.Button.StayHere".Localized(), TellPhyllaToStayTheFuckAtHomeThatCunt);
        }

        public void GetAntWaifuYoShit()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.GetAntWaifuYoShit.CampJoinInvite".Localized());

            OutputText("Scenes.Antsscene.GetAntWaifuYoShit.LoversMenuNotice".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AntWaifu, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void TellPhyllaToStayTheFuckAtHomeThatCunt()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.TellPhyllaToStayTheFuckAtHomeThatCunt.CampSafetyWarning".Localized());

            OutputText("Scenes.Antsscene.TellPhyllaToStayTheFuckAtHomeThatCunt.TurnToLeaveOffer".Localized());

            OutputText("Scenes.Antsscene.TellPhyllaToStayTheFuckAtHomeThatCunt.SilentNodDeparture".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaStayHome, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BumpIntoTheAntColonyAfterStayHomePhylla()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.BumpIntoTheAntColonyAfterStayHomePhylla.FamiliarPathVisit".Localized());

            OutputText("Scenes.Antsscene.BumpIntoTheAntColonyAfterStayHomePhylla.StartledEeepClick".Localized());

            OutputText("Scenes.Antsscene.BumpIntoTheAntColonyAfterStayHomePhylla.FlusteredWelcome".Localized());

            OutputText("Scenes.Antsscene.BumpIntoTheAntColonyAfterStayHomePhylla.SpaceUndergroundPlea".Localized());

            OutputText("Scenes.Antsscene.BumpIntoTheAntColonyAfterStayHomePhylla.QuietConsideration".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Come2camp".Localized(), GetAntWaifuYoShit);

            AddButton(1, "Common.Button.StayHere".Localized(), TellPhyllaToStayTheFuckAtHomeThatCunt);
        }

        public void PhyllaSprite(bool nude = false)
        {

        }

        public void IntroductionToPhyllaFollower(bool output = true)
        {
            ClearOutput();

            if (output)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaCapacity) < 50)
                    CoC.Instance.Flags.Set(GameFlagIds.PhyllaCapacity, 50);

                ShowImage("ant-camp-follower-intro");

                PhyllaSprite();

                if (Player.IsPregnant() && Player.Pregnancy.Incubation == 0)
                {
                    PhyllaLaysSomeDriderEggs();

                    return;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaCampVisits) == 0)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.PhyllaCampVisits, CoC.Instance.Flags.Get(GameFlagIds.PhyllaCampVisits) + 1);

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.AnthillSighting".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.TunnelNappingFind".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.JoyfulArrival".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.RoomGazingTrailOff".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.RoomNervousApproval".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.CutOffFunnyLook".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.ConfessionPreamble".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.NodToListen".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.PregnancyReveal".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.ThoughtsGrindHalt".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.FourArmWaving".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.ChildChoiceDenial".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.ConfusedDisbelief".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.CoveredMouthFluster".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.PregnancyExplanation".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.DidntFuckPhyllaOnRecruitment) == 1 && Player.HasCock())
                    {
                        OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.RaisedBrowQuestion".Localized());

                        OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.DeepBreathContinues".Localized());

                        OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.FluidLeakBlush".Localized());
                    }
                    else
                        if (!Player.HasCock())
                        {
                            OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.PenislessCuriosity".Localized());

                            OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.DeepBreathContinuesAgain".Localized());

                            OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.SweetFluidMemory".Localized());
                        }

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.KnowingLookNudge".Localized());

                    DoNext(() => PostFirstIntro());

                    return;
                }
                else
                {
                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.AnthillJourney".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.DaysPhyllaInCamp) < 10)
                        OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.UnimpressiveCaveMap".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.DaysPhyllaInCamp) < 30)
                            OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.ImpressiveCaveMap".Localized());
                        else
                            OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.DizzyingCarving".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.MindTwitchRecognition".Localized());

                    OutputText("Scenes.Antsscene.IntroductionToPhyllaFollower.ReturnWelcomePrompt".Localized());
                }
            }

            PhyllaCampMenu();
        }

        public void PhyllaCampMenu()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), PhyllaPearance);

            AddButton(1, "Common.Button.Talk".Localized(), PhyllaTalkChoices);

            if (Player.Lust >= 33)
                AddButton(2, "Common.Button.Sex".Localized(), PhyllaSexMenu);
            else
                AddButton(2, "Common.Button.Sex".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
                AddButton(3, "Common.Button.LayEggs".Localized(), PhyllaLaysEggsToggle);
            else
                AddButton(3, "Common.Button.NoEggs".Localized(), PhyllaLaysEggsToggle);

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 0)
                AddButton(5, "Common.Button.Children".Localized(), PhyllasKidsChildren);

            AddButton(1, "Common.Button.FindGems".Localized(), PhyllaDigsForGems);

            AddButton(1, "Common.Button.FindStones".Localized(), PhyllaStones);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void PostFirstIntro()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.PostFirstIntro.EmbarrassedGratitude".Localized());

            PhyllaCampMenu();
        }

        public void PhyllaSexMenu()
        {

            ClearButtons();

            PhyllaSprite(true);

            AddButton(0, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), null);

            AddButton(1, "Common.Button.FuckHer".Localized(), null);

            AddButton(2, "Common.Button.LesbianSex".Localized(), null);

            AddButton(3, "Common.Button.OrgyMale".Localized(), null);

            AddButton(4, "Common.Button.OrgyFemale".Localized(), null);

            AddButton(5, "Common.Button.Oviposit".Localized(), null);

            if (Player.HasCock())
            {
                AddButton(0, "Scenes.Kitsune.DefeatTheKitsunes.ButtonGetBJ".Localized(), PhyllaBeeeJays);

                AddButton(1, "Common.Button.FuckHer".Localized(), DickPhylla);

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AntKids) >= 10)
                    AddButton(1, "Common.Button.FuckHer".Localized(), DudesFuckEggLayingBitches);

                if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 10 && Player.IsCorruptEnough(75))
                    AddButton(3, "Common.Button.OrgyMale".Localized(), OrgyWithDatColonyCorruptDudes);
            }

            if (Player.HasVagina())
            {
                AddButton(2, "Common.Button.LesbianSex".Localized(), LesbianFisting);

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AntKids) >= 10)
                    AddButton(2, "Common.Button.LesbianSex".Localized(), BirfingSexWithAntsForDasLadies);

                if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 10 && Player.IsCorruptEnough(75))
                    AddButton(4, "Common.Button.OrgyFemale".Localized(), AntColonyOrgy4Ladies);
            }

            if (Player.CanOvipositSpider())
                AddButton(5, "Common.Button.Oviposit".Localized(), EggDatBitch);

            AddButton(14, "Common.Button.Back".Localized(), () => IntroductionToPhyllaFollower());
        }

        public void PhyllaTalkChoices()
        {

            ClearButtons();

            AddButton(0, "Common.Button.History".Localized(), TalkAboutAntHistory);

            AddButton(1, "Common.Button.Mating".Localized(), TalkAboutAntMatingAndRituals);

            AddButton(2, "Common.Button.HerLife".Localized(), PhyllasLifePastAndFuture);

            AddButton(14, "Common.Button.Back".Localized(), () => IntroductionToPhyllaFollower());
        }

        public void TalkAboutAntHistory()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TalkedWithPhyllaAboutHistory, 1);

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.YouRelaxAskHistory".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.SheLooksDownTunnel".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.SuperColonyExplanation".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.ImagineSuperColonies".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.HerVoiceTrailsOff".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.AfterDemonsInquiry".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.ImploreHerContinue".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.OrgyConfessionDemure".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.QueenLustDecline".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.OnVergeOfTears".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntHistory.TearsThenSmile".Localized());

            if (Player.Corruption < 70)
            {
                OutputText("Scenes.Antsscene.TalkAboutAntHistory.InterruptWithKiss".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntHistory.FirstMeetingVisions".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntHistory.HappyMemoriesFinish".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntHistory.StandAlmostFall".Localized());
            }
            else
                if (Player.Corruption < 80)
                {
                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.InterruptLovingSpeech".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.BetterThingsToDo".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.TimidApologyRecover".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.SheThoughtWrong".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.NoTimeHeadBack".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.DisgustedScoffPush".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.HarshBeratingSpeech".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.ShamingHurtsHer".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.NotAliveProtest".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.CorruptionArousalThought".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                        OutputText("Scenes.Antsscene.TalkAboutAntHistory.EveryYearSacrifice".Localized());
                    else
                        OutputText("Scenes.Antsscene.TalkAboutAntHistory.KnowingPriorFates".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.ShockedPlaceExists".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.BeggingToStay".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.StopReadingIntoThings".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntHistory.LeaveHearCrying".Localized());
                }

            DoNext(() => ReturnToCamp(1));
        }

        public void TalkAboutAntMatingAndRituals()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.WonderMindProjection".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.InheritedKnowledgeTalk".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.CuteStutterSmile".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.PlayfulKnowingFace".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 0)
                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.ShareWithChildren".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaTimesDriderEggLayed) > 0)
                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.NonAntChildren".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 0)
                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.SenseAllChildren".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.CountNumbersAnalogy".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.GigglingConfusesHer".Localized());

            OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.UnderstandingStruggle".Localized());

            if (Player.Corruption < 60)
            {
                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.KissSarcasticQuip".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.PoutyReiteration".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.ThankHerCovertTouch".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.GrinBackToCamp".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.TortureDescribingBody".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.FeignInnocenceAsk".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.SoftWhisperStart".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.LeanInTooSoft".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.StartsAgainLouder".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.BreastsUnimpressive".Localized());

                if (Player.BiggestBreastSize() >= 4)
                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.YoursAreNicer".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.CupsBothBreasts".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.NipplesGetHard".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.SensitiveMilkTalk".Localized());
                else
                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.MilkForChildren".Localized());

                OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.QuipKeepInMind".Localized());

                if (Player.Corruption >= 80)
                {
                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.RemovesUpperHands".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.StutteringVWord".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.CoaxHerWord".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.WhispersVagina".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.PretendNotHear".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.FinallyStatesVagina".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.CorrectToCunt".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.DefeatedCuntRepeat".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.ClitVisibleLips".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                        OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.MilkDripping".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.ExplainNoBirth".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.ClitorisHypersensitive".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.HoldsPussyOpen".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                        OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.JetsOfMilk".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.LoudMoanOrgasm".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.SoakedHandsLook".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.GlazedEyesRoll".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.TimePassesBreak".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.LostCountFifteen".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.PleadsTooMuch".Localized());

                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.SilentPoolSecretion".Localized());

                    View.Time.Hours += 1;

                    Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
                }
                else
                {
                    OutputText("Scenes.Antsscene.TalkAboutAntMatingAndRituals.WinkThankShow".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 15f));
                }
            }

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaInheritedKnowledge, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void PhyllasLifePastAndFuture()
        {
            ClearOutput();

            // 原版 AntsScene.hx:1551：Izma 是跟随者且尚未谈过 Phylla 的往事
            if (SceneInstance<Npcs.IzmaScene>.Get().IzmaFollower()
                && CoC.Instance.Flags.Get(GameFlagIds.PhyllaIzmaTalk) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.PhyllaIzmaTalk, 1);

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaDistant".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaTapsHead".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaDistressed".Localized());

                if (Player.Corruption < 40)
                {
                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.ExplainIzma".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaUnconvinced".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.FoughtQuestion".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.CiteIzmaIdea".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaShyHope".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaPerks".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.RaiseEyebrow".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaInheritedKnowledge) == 1)
                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.QuestionControl".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.TryPhylla".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaEmbrace".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.MemoryOfCrying".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaCalmer".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.BreakKiss".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaApology".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.TellUnderstandable".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.ChuckleClap".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaThanks".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.ReturnToCamp".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaKissAttack".Localized());

                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.MindMeldStress".Localized());

                    if (Player.Corruption >= 80)
                    {
                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.HowDareShe".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PushPhyllaAway".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaStammers".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.HeadPounding".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.InsultBarrage".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaFlinches".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.DominateUrge".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.GrabHair".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.ForceKiss".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.DepravedMemory".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.MindProbing".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.LaughDepart".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.CounterProbe".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.AwkwardPause".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaRealization".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaWeeps".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.IzmaMatters".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.ChastiseTone".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.BothMatter".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.WipeTears".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.MeanItQuery".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.YouNod".Localized());

                        OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.LongSilence".Localized());
                    }
                }
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.CushionCuddle".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.AskHerPast".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.LeanOnChest".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaShudders".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PrincessMate".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.SqueezeArm".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) == 0)
                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.PhyllaHesitates".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.ChildrenScurry".Localized());

                OutputText("Scenes.Antsscene.PhyllasLifePastAndFuture.MullOver".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void PhyllaBeeeJays()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaBlowjobs, CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) + 1);

            ShowImage("ant-camp-give-bj");

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) == 1)
            {
                OutputText("Scenes.Antsscene.PhyllaBeeeJays.SeductiveLook".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.LoudCough".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.PhyllaExcuse".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.StomachCaress".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.ShyWaver".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.PhyllaAgrees".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllaBeeeJays.LeanOnWall".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.AskNicely".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.PluralEsSuffix".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.SheTeases".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
                OutputText("Scenes.Antsscene.PhyllaBeeeJays.PhyllaGrins".Localized());
            else
                OutputText("Scenes.Antsscene.PhyllaBeeeJays.PhyllaLooksStuck".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) == 1)
                OutputText("Scenes.Antsscene.PhyllaBeeeJays.MealDelivered".Localized());
            else
                OutputText("Scenes.Antsscene.PhyllaBeeeJays.CoosPlayfully".Localized());

            var x = Player.SmallestCockIndex();

            if (Player.CockArea(x) < 60)
            {
                OutputText("Scenes.Antsscene.PhyllaBeeeJays.FreeCocks".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.ItsPronoun".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.TheirPronoun".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.UndoStraps".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.PullUndergarments".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.AllWord".Localized());

                OutputText("Scenes.Antsscene.PhyllaBeeeJays.BobbingUp".Localized());

                if (Player.Cocks[x].Length < 5)
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.TallAndProud".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.RockHardReady".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) == 1)
                {
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.TugFreeMember".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.PluralSLetter".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.DoesntRealizeSize".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.ThisPronoun".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.ThesePronoun".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.BatteringRam".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.PluralSLetterAgain".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.ChuckleIgnorance".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.ChidingRemark".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.StripClothing".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.ItPronoun".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.ThemPronoun".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.FullErection".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.PhyllaAwed".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.FleshySpears".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.SputterLaugh".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.BeCareful".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) == 2)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.LastTime".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.OurFirstTime".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.HelpFreeIt".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.MonsterSingular".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.MonsterPlural".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.CarefulValuables".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.NoAccidents".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.SingularIsVerb".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.PluralAreVerb".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.FreedAndReady".Localized());

                    if (Player.Cocks[x].Length < 16)
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.ShaftBaseWord".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaBeeeJays.ShaftMiddleWord".Localized());

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.TongueOnShaft".Localized(
                         Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Antsscene.PhyllaBeeeJays.ChucklePullAway".Localized(
                         Player.CockDescript(x)
                    ));
                }

            ClearButtons();

            if (Player.Corruption < 75)
                AddButton(0, "Common.Button.Next".Localized(), PurePhyllaBJOver);
            else
                AddButton(0, "Common.Button.Next".Localized(), CorruptPhyllaEndings);
        }

        public void PurePhyllaBJOver()
        {
            ClearOutput();

            ShowImage("ant-camp-give-pure-bj-cum");

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.SoftlyReassureHer".Localized());

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.HerEagerPromise".Localized());

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.SecureSheLowersDown".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.InstructOpenMouth".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.LanguidTongueLick".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Antsscene.PurePhyllaBJOver.BriefGaspRequest".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Antsscene.PurePhyllaBJOver.YourOtherDick".Localized());
                else
                    OutputText("Scenes.Antsscene.PurePhyllaBJOver.RestOfYourDicks".Localized());

                OutputText("Scenes.Antsscene.PurePhyllaBJOver.WithGustoHandsRise".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Antsscene.PurePhyllaBJOver.FirmGripsPlural".Localized());
                else
                    OutputText("Scenes.Antsscene.PurePhyllaBJOver.FirmGripsSingular".Localized());

                OutputText("Scenes.Antsscene.PurePhyllaBJOver.MumblesThenRealizes".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Antsscene.PurePhyllaBJOver.AskForWomanhoodCare".Localized());

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.BrushHairGuideHead".Localized());

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.MinutesFlyTeaching".Localized());

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.SheNeedsNoGuidance".Localized());

            OutputText("Scenes.Antsscene.PurePhyllaBJOver.SheAdmitsHeat".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Sure".Localized(), SurePhyllaLetsFuck);

            AddButton(1, "Common.Button.Nope".Localized(), NopeNotOnMouthOrWhateverFuckThisNoise);
        }

        public void SurePhyllaLetsFuck()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Antsscene.SurePhyllaLetsFuck.CutOffWithKiss".Localized());

            OutputText("Scenes.Antsscene.SurePhyllaLetsFuck.PleasuresTidalWave".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.SurePhyllaLetsFuck.EyesOpenOnGaze".Localized());

            if (Player.CanAutoFellate())
                OutputText("Scenes.Antsscene.SurePhyllaLetsFuck.FamiliarYetDifferent".Localized());

            OutputText("Scenes.Antsscene.SurePhyllaLetsFuck.BeseechMindAndBody".Localized());

            PureBJEnding(true);
        }

        public void NopeNotOnMouthOrWhateverFuckThisNoise()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.NopeNotOnMouthOrWhateverFuckThisNoise.DeclineHerOffer".Localized());

            if (!Player.CanAutoFellate())
                OutputText("Scenes.Antsscene.NopeNotOnMouthOrWhateverFuckThisNoise.OrSimilarSensation".Localized());
            else
                OutputText("Scenes.Antsscene.NopeNotOnMouthOrWhateverFuckThisNoise.LinkedMindFeeling".Localized());

            OutputText("Scenes.Antsscene.NopeNotOnMouthOrWhateverFuckThisNoise.WhinesRespectsWishes".Localized());

            OutputText("Scenes.Antsscene.NopeNotOnMouthOrWhateverFuckThisNoise.EyesOpenLongingGaze".Localized());

            PureBJEnding(false);
        }

        public void PureBJEnding(bool linked = true)
        {

            ShowImage("ant-camp-give-pure-bj-cum");

            OutputText("Scenes.Antsscene.PureBJEnding.FullyEmbracedTalent".Localized());

            if (!linked)
                OutputText("Scenes.Antsscene.PureBJEnding.SentencePeriod".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesLinkedBjSuck) == 0)
                {
                    OutputText("Scenes.Antsscene.PureBJEnding.CommaSpace".Localized());

                    OutputText("Scenes.Antsscene.PureBJEnding.JestThroughLink".Localized());
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesLinkedBjSuck) > 0)
                OutputText("Scenes.Antsscene.PureBJEnding.RelyingLessOnLink".Localized());

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Antsscene.PureBJEnding.GigglesWhileBobbing".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() < 250)
                OutputText("Scenes.Antsscene.PureBJEnding.ThinWord".Localized());
            else
                if (Player.CumQ() > 1000)
                    OutputText("Scenes.Antsscene.PureBJEnding.ThickWord".Localized());

            OutputText("Scenes.Antsscene.PureBJEnding.StrandsHangBetween".Localized());

            if (Player.CumQ() > 1000)
                OutputText("Scenes.Antsscene.PureBJEnding.MouthfulNotFinished".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Antsscene.PureBJEnding.YourOtherCock".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Antsscene.PureBJEnding.PossessiveSPrefix".Localized());
                else
                    OutputText("Scenes.Antsscene.PureBJEnding.ShowersPlural".Localized());

                OutputText("Scenes.Antsscene.PureBJEnding.HandsBodyInGoo".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Antsscene.PureBJEnding.EachOfThemPhrase".Localized());

                OutputText("Scenes.Antsscene.PureBJEnding.ThemLovinglyPhrase".Localized());
            }

            OutputText("Scenes.Antsscene.PureBJEnding.HeavySighLighter".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Antsscene.PureBJEnding.BallsWord".Localized());
            else
                OutputText("Scenes.Antsscene.PureBJEnding.InThePelvis".Localized());

            OutputText("Scenes.Antsscene.PureBJEnding.RubHairPlayfully".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) != 1)
                OutputText("Scenes.Antsscene.PureBJEnding.SentencePeriodAgain".Localized());
            else
                OutputText("Scenes.Antsscene.PureBJEnding.CongratulateFirstTime".Localized());

            OutputText("Scenes.Antsscene.PureBJEnding.PuffedCheeksNoticed".Localized());

            Player.Orgasm("Dick");

            ClearButtons();

            AddButton(0, "Common.Button.SwallowIt".Localized(), SwallowDatJismPhylla);

            AddButton(1, "Common.Button.Spititout".Localized(), SpitItOutYouCunt);
        }

        public void SwallowDatJismPhylla()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.SwallowDatJismPhylla.HandsOnHipsSmirk".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SpitItOutYouCunt()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.SpitItOutYouCunt.ArmsCrossedDemandSpit".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CorruptPhyllaEndings()
        {
            ClearOutput();

            var x = Player.SmallestCockIndex();

            ShowImage("ant-camp-give-corrupt-bj-cum");

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.PonderWhileRubbing".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.ForceOpenWide".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) == 1)
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.DesiredEffectAchieved".Localized());
            else
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.ImprovedSinceLastTime".Localized());

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.GrabHeadBrutishly".Localized());

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.GroansAtRoughness".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.MuffledMoansEscape".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.BriefGaspRequest".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Antsscene.CorruptPhyllaEndings.OtherDickPhrase".Localized());
                else
                    OutputText("Scenes.Antsscene.CorruptPhyllaEndings.RestOfYourDicks".Localized());

                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.WithGustoHandsRise".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.AskForWomanhoodCare".Localized());

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.PraiseHastyHeeding".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.CheekyLoudYell".Localized());

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.LowerOtherArm".Localized());

            if (Player.CumQ() < 10)
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.TinyWord".Localized());
            else
                if (Player.CumQ() < 100)
                    OutputText("Scenes.Antsscene.CorruptPhyllaEndings.RespectableWord".Localized());
                else
                    OutputText("Scenes.Antsscene.CorruptPhyllaEndings.MassiveWord".Localized());

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.FacialCoversFeatures".Localized());

            if (silly)
            {
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.StreamDoesNotStop".Localized());

                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.SheDoesntMindBathing".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Antsscene.CorruptPhyllaEndings.BallsToken".Localized());
                else
                    OutputText(Player.CockDescript(x));

                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.FortitudeRollSuccess".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.BeggingForExtra".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.ForgotAboutClit".Localized());
            }

            if (silly)
                OutputText("Scenes.Antsscene.CorruptPhyllaEndings.WakeFromSlumber".Localized());

            OutputText("Scenes.Antsscene.CorruptPhyllaEndings.MuseCanYouAgain".Localized());

            Player.Orgasm("Generic");

            DoNext(() => ReturnToCamp(1));
        }

        public void DickPhylla()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            ShowImage("ant-camp-vagsex");

            OutputText("Scenes.Antsscene.DickPhylla.GivePhyllaDeviousLook".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaBlowjobs) == 0)
            {
                OutputText("Scenes.Antsscene.DickPhylla.SuddenNudeRealization".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.DickPhylla.SLetter".Localized());

                OutputText("Scenes.Antsscene.DickPhylla.SentencePeriod".Localized());

                OutputText("Scenes.Antsscene.DickPhylla.PhyllaHesitantAsk".Localized());

                OutputText("Scenes.Antsscene.DickPhylla.GentlyGrabHerHead".Localized());
            }
            else
                OutputText("Scenes.Antsscene.DickPhylla.SheScampersToKnees".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.PushedOntoBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
                OutputText("Scenes.Antsscene.DickPhylla.AbdomenShaftRidge".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.LowerHandPumps".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaFucks) >= 3)
                OutputText("Scenes.Antsscene.DickPhylla.PlayfulQuestion".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaFucks) == 0)
                    OutputText("Scenes.Antsscene.DickPhylla.QuickNod".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaFucks) > 0)
                OutputText("Scenes.Antsscene.DickPhylla.MischievousAskBack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaFucks) >= 3)
                OutputText("Scenes.Antsscene.DickPhylla.LosingShyness".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.GuidingCockInside".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.EyesWidenSurprise".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Antsscene.DickPhylla.AndBallsClause".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.SexualLiquidsClause".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.RidingWithoutPause".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.FutileSentenceAttempts".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.PinchLoveKnob".Localized());

            if (Player.CockTotal() >= 2 && Player.CockTotal() <= 5)
                if (Player.HasVagina())
                    OutputText("Scenes.Antsscene.DickPhylla.AttendLadyParts".Localized());
                else
                {
                    OutputText("Scenes.Antsscene.DickPhylla.ShrugUnsure".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Antsscene.DickPhylla.ItPronoun".Localized());
                    else
                        OutputText("Scenes.Antsscene.DickPhylla.ThemPronoun".Localized());

                    OutputText("Scenes.Antsscene.DickPhylla.SlowlyWord".Localized());
                }

            if (Player.CockTotal() > 5)
                if (Player.HasVagina())
                    OutputText("Scenes.Antsscene.DickPhylla.AllHandsPleasure".Localized());
                else
                    OutputText("Scenes.Antsscene.DickPhylla.AllHandsPleasureAgain".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Antsscene.DickPhylla.PlayfulAggressiveWhisper".Localized());

            if ((int)Player.Gender >= 3)
                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaFucks) == 0)
                    OutputText("Scenes.Antsscene.DickPhylla.SpitFixNeglect".Localized());
                else
                    OutputText("Scenes.Antsscene.DickPhylla.TeasingBetweenMoans".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.HardPinchLoveButton".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.TongueMindSync".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.HipsSlowToStop".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.TurnAwayAllSixes".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.TakeHintSlamDeep".Localized());

            if (Player.CountCocksOfType("TENTACLE") == 2)
                OutputText("Scenes.Antsscene.DickPhylla.ContentBodySucksCock".Localized());
            else
                if (Player.CountCocksOfType("TENTACLE") > 2)
                    OutputText("Scenes.Antsscene.DickPhylla.TentaclesToHerHands".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.HumpUsingAbdomen".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.FinalDeepSlam".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.CollapseStickyHeap".Localized());

            OutputText("Scenes.Antsscene.DickPhylla.PhyllaStirsInvite".Localized());

            Player.Orgasm("Dick");

            DoNext(() => ReturnToCamp(1));
        }

        public void LesbianFisting()
        {
            ClearOutput();

            ShowImage("ant-camp-female-fisting");

            OutputText("Scenes.Antsscene.LesbianFisting.SeductiveSmileArmor".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.UncrossLegsForHer".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.ShyBlushQuestion".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.SilentBeckonHookedFinger".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Antsscene.LesbianFisting.PhyllaPumpsCock".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.EarthSweatSexAroma".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.FingersReachDeeper".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.MouthFingersWithdraw".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.KissUpYourBody".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.MindSyncWarmth".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.ThrustingNippleRub".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.MindEyeSpearFingers".Localized());

            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin || Player.VaginalCapacity() <= 10)
                OutputText("Scenes.Antsscene.LesbianFisting.FingersPartialInsert".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.ThrustHitsRightSpot".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.WakeHourLater".Localized());

            OutputText("Scenes.Antsscene.LesbianFisting.ThinkAboutItWink".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void DudesFuckEggLayingBitches()
        {
            ClearOutput();

            ShowImage("ant-desert-male-preg-vagsex");

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.TakenAbackByRequest".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.OffersToBeSmaller".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.ContractionMidSentence".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.ChildScoopsEgg".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.RecoversThenRemembers".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.RamblingPersuasion".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.DisregardAppearance".Localized());
            else
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.IgnorePregnantForm".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.ApproachHerAnd".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.LovinglyWord".Localized());
            else
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.ApatheticallyWord".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.PullHandsFromBreasts".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.BreastsDoubledMilk".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.SqueezeMilkJet".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.CommentSoreBack".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.StammeredReply".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.SqueezeMilkCutOff".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.LatchOnLikeCobra".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.LowerArmsGripHead".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.DetachCaresNoMore".Localized());

            if (silly)
            {
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.BelchClearsStomach".Localized());

                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.ArousalStillReady".Localized());
            }

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.HandWelcomedGuest".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.VoiceStopsTooLate".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.VaginaCrushesFingers".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.EggTipEmerges".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.DreadingRepeat".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.MagnetsLinkMinds".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.FeelEggStage".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.FourArmsUndressYou".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.ClitDragsCockTop".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.TwoCanPlayGame".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.BodiesShareInfo".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.BallsSlapPelvis".Localized());
            else
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.KneesGrindThroneEdge".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.FeelBirthDescend".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.LiftedOffFeet".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.PussyWallsWarp".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.CuntFinalPulse".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.ContractionStagesPass".Localized());

            OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.GaspsCollapses".Localized());

            if (Player.Corruption < 75)
            {
                if (Player.Strength >= 70)
                    OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.CarryHerToBed".Localized());
                else
                    OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.DragExhaustedPhylla".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.SighLiftToFeet".Localized());

                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.CommandMarchHumiliate".Localized());

                OutputText("Scenes.Antsscene.DudesFuckEggLayingBitches.StaggeringWalkFlop".Localized());
            }

            Player.Orgasm("Generic");

            Player.RefillHunger(20);

            DoNext(() => ReturnToCamp(1));
        }

        public void BirfingSexWithAntsForDasLadies()
        {
            ClearOutput();

            ShowImage("ant-camp-female-preg-");

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.TakenAbackByRequest".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.GlancesBehindHerself".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.StopsMidSentence".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.UndressesBeforeHer".Localized());

            if (Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.SlithersWhorishly".Localized());
            else
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.StrutsWhorishly".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.SensualHipSwivel".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.DidntMeanJustThought".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.FingerToLips".Localized());
            else
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.WantsToSeeTits".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.MilkAlreadyDribbling".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.NeedToFeedChildren".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.SqueezesHerBreast".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.BitesHerNipple".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.SpreadsHerLegs".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.WondersAtHerWarmth".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.AnotherEggComing".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.LiftsFaceToLink".Localized());

            if (Player.Corruption < 75)
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.PullsHerIntoKiss".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.LinkHitsHard".Localized());

                if (Player.Pregnancy.Incubation > 0)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.PregnanciesTransferred".Localized());
                else
                    if (Player.StatusEffectValue(StatusEffectIds.Birthed, 0) > 0)
                        OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.DifferentThanBefore".Localized());
                    else
                        OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.EveryStageAtOnce".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.WantsToFuckForever".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.TeachesDisappointment".Localized());
            }

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.YouWord".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.LovinglyWord".Localized());
            else
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.DismissivelyWord".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.FingersHerRelentlessly".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.DivesIntoHerScent".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.GroanOfPleasure".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.MakesBestOfIt".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.WillMakeHerPay".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.SecondRoundSensation".Localized());

            if (Player.Corruption <= 50)
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.HerMindPleads".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ClawsAtYourHead".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.NotReadyToCum".Localized());

            if (Player.Corruption <= 50)
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ProbesHerMind".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.LickMeRightThere".Localized());

            if (Player.Corruption <= 50)
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.AllowsHerSilence".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ContinuePussyAssault".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.AsksHowToPlease".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.BreaksHerGrip".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.HandsOutOfWay".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.WrestlesFree".Localized());

                if (Player.Vaginas[0].Looseness >= 3)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.JuicesDripOnFace".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.BidsHerLickPussy".Localized());
            }

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.StarvingForYourTaste".Localized());

            if (Player.CockTotal() > 0 && Player.HasCockThatFits(42))
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.CocksBegForTouch".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ThemThey".Localized());
                else
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ItIt".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.StrokingWord".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ItItAgain".Localized());
                else
                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ThemTheyAgain".Localized());
                    else
                        OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ManyAsPossible".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.FiercelyAsBodyAllows".Localized());
            }

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ClitGlidesAlongLips".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.MustFinishQuickly".Localized());
            else
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.EggBeforeFinished".Localized(
                     Player.Mf("Daddy", "Momma")
                ));

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.TongueAndFingersMagic".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.LeansDownToWhisper".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ClitRestsOnLips".Localized());

            if (Player.CockTotal() > 0 && Player.HasCockThatFits(42))
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.AlternatesLickAndSuck".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ReachesForAbdomenTip".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ClimaxSeizesBoth".Localized());

            if (Player.CockTotal() > 0 && Player.HasCockThatFits(42))
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.PopsTwoCocksIn".Localized());

                if (Player.CockThatFits2(42) >= 0)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.TwoOfWord".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.CocksIntoHerMouth".Localized());

                if (Player.CockThatFits2(42) >= 0)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ThemTheyThird".Localized());
                else
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ItItThird".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.AgainstHerThroat".Localized());
            }
            else
                if (Player.HasCock())
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.RollsOffToErupt".Localized());

            if (Player.BiggestCockArea() >= 500)
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.TwitchesWithRelease".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.SLetter".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.HeavyHeaveLaunch".Localized());
            }

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.FloodedByGirlCum".Localized());

            OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.NatureRuinsMoment".Localized());

            if (Player.Corruption < 75)
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.FlipsHerOver".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ThanksBetweenKisses".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.PatterUpHallway".Localized());

                if (Player.Stats.Get(GameStats.TeaseDamage) <= 0)
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ChildNoticesNeed".Localized());
                else
                    OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.ModestyFails".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.DriftOffToSleep".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.CouldntCareLess".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.GroanOfBirth".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.CutOffWithLook".Localized());

                OutputText("Scenes.Antsscene.BirfingSexWithAntsForDasLadies.TimeToSleep".Localized());
            }

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void OrgyWithDatColonyCorruptDudes()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesCorruptMaleAntOrgy, CoC.Instance.Flags.Get(GameFlagIds.TimesCorruptMaleAntOrgy) + 1);

            ShowImage("ant-camp-mle-orgy");

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SummonStrongestBrood".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCorruptMaleAntOrgy) == 1)
            {
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.ConfusedAboutRoomFit".Localized());

                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SheIsProbablyRight".Localized());
            }
            else
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.MeekNodUnderstanding".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.FiveSonsArrive".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.LineUpInspection".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.DemandFullNudity".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCorruptMaleAntOrgy) == 1)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.InsistOnFullView".Localized());
            else
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PhyllaGivesIn".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PhyllaEntersTrance".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PhyllaLooksAway".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.StrokeLongCock".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.FourAntsYearn".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.TeaseAboutLust".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.InstructOtherFour".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.LaysDownUnderHer".Localized());
            else
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.LiftsAbdomenToFace".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PhyllaLosesHerself".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.AntThrustsInHand".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SlapAssEncourage".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.ChargesAtMother".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.LargeLactating".Localized());
            else
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SmallWord".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.OrbsWord".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.FaceSlappedByBalls".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.KissStolenGrabsBrother".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.RealizeStillClothed".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PluralSuffix".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.StiffLongingForOrgy".Localized());

            if (Player.CockTotal() > 1 && Player.CockTotal() <= 3)
            {
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.EveryCockAttended".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.IsWord".Localized());
                else
                    OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SAreFragment".Localized());

                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SuckedOffByTwo".Localized());
            }

            if (Player.CockTotal() >= 4 && Player.CockTotal() <= 6)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.StandOverPhylla".Localized());

            if (silly)
            {
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.HeartfeltSpeech".Localized());

                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.RousingBattleSpeech".Localized());
            }

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.LimbsCocksPile".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.MouthBeggingFilled".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.FourHandsWelcome".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCorruptMaleAntOrgy) == 1)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.LinkSeemsStrained".Localized());
            else
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.MomentLongAwaited".Localized());

            var x = Player.CockThatFits(PhyllaCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.MindFullySynced".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.EveryMindWinces".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.VaginaPulsations".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.BroodLicksClit".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockTotal() > 2)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.MoreCocksNeededPlural".Localized());
            else
                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.MoreCocksNeededSingular".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.EverythingBlursTogether".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SynchronizedRelease".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.YourOwnBalls".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.CoatPhyllaInCum".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PhyllaSitsUpLicks".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.MassiveOrgyHours".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PluralSuffixAgain".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.SuckedStrokedInserted".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.WakeUpExhausted".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.PhyllaWeakVoice".Localized());

            OutputText("Scenes.Antsscene.OrgyWithDatColonyCorruptDudes.TimeToLeave".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void AntColonyOrgy4Ladies()
        {
            ClearOutput();

            ShowImage("ant-camp=female-orgy");

            CoC.Instance.Flags.Set(GameFlagIds.TimesCorruptFemaleAntOrgy, CoC.Instance.Flags.Get(GameFlagIds.TimesCorruptFemaleAntOrgy) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesCorruptFemaleAntOrgy) == 1)
            {
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.ConfusedButObeys".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FinestWarriorsComing".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.HopeForFineCocks".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.EachCockHarden".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FiveMalesEnter".Localized());
            }
            else
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.WeakNodConsent".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.BestSpecimens".Localized());

            if (Player.Femininity >= 80)
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.AllFiveErect".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.NailsScratchChest".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.GlanceDownTheLine".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.BallsFullOfPrecum".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.MentionNeedRelease".Localized());

            if (silly)
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.GreaterGoodEcho".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PhyllaAvertsGaze".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.ScoldForFairness".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.TerrifiedByOffspring".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FootOnHisShoulder".Localized());
            else
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PerfectHeightOffer".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FaceIntoYourCunt".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.NearOrgasmFromOral".Localized());

            if (Player.IsBiped())
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.BetweenYourLegs".Localized());
            else
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FromYourCunt".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.BeckonHimFollow".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.GlazedBlankExpression".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.InterruptHerPlea".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.DemandSwallowSeed".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PhyllaSwallows".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.BeggingForMore".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.KissWhatSheWanted".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.MindEmotionsSync".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.LoinsAfire".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PlayfulPussyTouch".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.MaleWedgesBetween".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.SandwichHisCock".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.YearningGivesIdea".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.CommandDoggystyle".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.EggFilledAbdomen".Localized());
            else
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.ScramblesIntoPlace".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.AdjacentPosition".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.AssesInTheAir".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.HandsOnYourShaft".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PluralSuffix".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.SpurtPreOnFloor".Localized());
            }

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.MindScreamsPenetration".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.MentalGoAhead".Localized());

            Player.CuntChange(20, true);

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.UnsureWhatFeelsBest".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.MusterFortitude".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.MountingAntGrabs".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PluralSuffixAgain".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.SentencePeriod".Localized());
            }

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.SlowerHarderThrusts".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PleasureCrashesIn".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.ReachesCrescendo".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FinalClimaxThrust".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.OrgasmRocksBody".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.RoarInBliss".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.WishSomeoneUnder".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PluralSuffixThird".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FeelYourLoad".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.PluralSuffixFourth".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.SplashOnChest".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.ItPronoun".Localized());
                else
                    OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.TheyPronoun".Localized());

                OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.UnloadWord".Localized());
            }

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.WarmthFillsWomb".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.SaltyPayloadErupts".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.FinishingHerMeal".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.HeadClearingUp".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.StillNeedsMore".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.CrawlingToDoorway".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.ProudOfGoodSlut".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.WakeUpAching".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.WeakVoiceSterile".Localized());

            OutputText("Scenes.Antsscene.AntColonyOrgy4Ladies.CollectThingsLeave".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void PhyllaLaysEggsToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.PhyllaEggLaying, 1);

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.PhyllaGazeConfusion".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TalkedWithPhyllaAboutHistory) > 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.TellColonyWish".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.EyesWidenShock".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.LickItRequest".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.LickThat".Localized(), LickThatAntButt);

                AddButton(1, "Common.Button.DontLick".Localized(), DontLickAntButt);
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.PhyllaEggLaying, 0);

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.SighAboutTalk".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.AstonishedAgreement".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.AbdomenPulseEgg".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.AskAbdomenNormal".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysEggsToggle.ThankAndLeave".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void LickThatAntButt()
        {
            ClearOutput();

            PhyllaSprite();

            ShowImage("ant-camp-lay-egg-licked");

            OutputText("Scenes.Antsscene.LickThatAntButt.TiltHeadDoubt".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.FasterKickstartPlea".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.BeggingArousalRush".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.BeggingMoansAction".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.GrabAbdomenDive".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.RavageBirthCanal".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.HourLaterRelease".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.CollapseEyesWiden".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.MinuteThenEgg".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AntsBirthedFromLicking) == 0)
            {
                OutputText("Scenes.Antsscene.LickThatAntButt.ShelllessClearEgg".Localized());

                OutputText("Scenes.Antsscene.LickThatAntButt.CradleNewLife".Localized());

                OutputText("Scenes.Antsscene.LickThatAntButt.ComplimentInterrupted".Localized());

                OutputText("Scenes.Antsscene.LickThatAntButt.SurprisedFastRate".Localized());

                OutputText("Scenes.Antsscene.LickThatAntButt.RecoveredGaspReply".Localized());

                OutputText("Scenes.Antsscene.LickThatAntButt.SmallContractionsGasp".Localized());
            }
            else
                OutputText("Scenes.Antsscene.LickThatAntButt.UnwrapAndScoop".Localized());

            OutputText("Scenes.Antsscene.LickThatAntButt.ContractionSeepEgg".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaCooldown, 12);

            CoC.Instance.Flags.Set(GameFlagIds.AntsBirthedFromLicking, CoC.Instance.Flags.Get(GameFlagIds.AntsBirthedFromLicking) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) < 5000)
                CoC.Instance.Flags.Set(GameFlagIds.AntKids, CoC.Instance.Flags.Get(GameFlagIds.AntKids) + 5);

            DoNext(() => ReturnToCamp(1));
        }

        public void DontLickAntButt()
        {
            ClearOutput();

            PhyllaSprite();

            ShowImage("ant-camp-lay-egg-not-licked");

            OutputText("Scenes.Antsscene.DontLickAntButt.HesitantDiscomfort".Localized());

            OutputText("Scenes.Antsscene.DontLickAntButt.WhisperedThanks".Localized());

            OutputText("Scenes.Antsscene.DontLickAntButt.SimulationMinutes".Localized());

            OutputText("Scenes.Antsscene.DontLickAntButt.EmbraceAfterHour".Localized());

            OutputText("Scenes.Antsscene.DontLickAntButt.ConfideNoTrouble".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 1)
                OutputText("Scenes.Antsscene.DontLickAntButt.ChildrenTendEggs".Localized());

            OutputText("Scenes.Antsscene.DontLickAntButt.DutiesInviteLater".Localized());

            OutputText("Scenes.Antsscene.DontLickAntButt.RemarkAndWink".Localized((CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 2 ? ", passing several of your children as they scoop up the bundle of eggs that lie huddled together on the floor" : "")));

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaCooldown, 6);

            CoC.Instance.Flags.Set(GameFlagIds.AntKids, CoC.Instance.Flags.Get(GameFlagIds.AntKids) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void PhyllaLaysSomeDriderEggs()
        {
            ClearOutput();

            PhyllaSprite();

            Player.Pregnancy.Type = default;

            Player.Pregnancy.Incubation = 0;

            ShowImage("ant-desert-lay-drideregg");

            OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.BedchamberEeep".Localized());

            if (Player.Corruption < 75)
            {
                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.KneelTakeHand".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.MoanOnShoulder".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.WordsCutSpasm".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.HalfExclaimMoan".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.BothEggsDrop".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.KnowingThatPrefix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.BothEggsFine".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.SingleEggFine".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.RedirectFocusSuffix".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaTimesDriderEggLayed) == 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.BetweenGaspsAir".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.RubbingEmptyBelly".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.HatchingInterrupts".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.CorruptBroodBirth".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.FirstEggEmerges".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.HerOwnOffspring".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaTimesDriderEggLayed) == 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.NotedNewTrick".Localized());

                OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.LastHeaveExpel".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) > 0)
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.AbdomenFewLeft".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 45f));

                if (Player.CanOvipositSpider())
                {
                    OutputText("Scenes.Antsscene.PhyllaLaysSomeDriderEggs.MountUrgeDesire".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Letherrecover".Localized(), LetPhyllaRecover);

                    AddButton(1, "Common.Button.Mount".Localized(), DriderDoublePhllaMount);
                }
                else
                {
                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), LetPhyllaRecover);
                }
            }

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaTimesDriderEggLayed, CoC.Instance.Flags.Get(GameFlagIds.PhyllaTimesDriderEggLayed) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaDriderBabiesCount, CoC.Instance.Flags.Get(GameFlagIds.PhyllaDriderBabiesCount) + 5 + Random.Shared.Next(4));
        }

        public void DriderDoublePhllaMount()
        {
            ClearOutput();

            ShowImage("ant-camp-ant-mounted-by-drider");

            OutputText("Scenes.Antsscene.DriderDoublePhllaMount.RipArmorCharge".Localized());

            OutputText("Scenes.Antsscene.DriderDoublePhllaMount.WhisperIgnitesDesire".Localized());

            OutputText("Scenes.Antsscene.DriderDoublePhllaMount.EggAppendageEntry".Localized());

            OutputText("Scenes.Antsscene.DriderDoublePhllaMount.FuriousHowlFill".Localized());

            OutputText("Scenes.Antsscene.DriderDoublePhllaMount.EggSupplySpent".Localized());

            Player.Orgasm("Ovi");

            Player.DumpEggs();

            if (Player.Pregnancy.Incubation == 0)
            {
                Player.Pregnancy.Type = Pregnancy.PregnancyIds.DriderEggs;

                Player.Pregnancy.Incubation = 8 * 24;
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void LetPhyllaRecover()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.LetPhyllaRecover.SelfControlRest".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PhyllasKidsChildren()
        {
            ClearOutput();

            OutputText("Scenes.Antsscene.PhyllasKidsChildren.SurprisedInterest".Localized());

            OutputText("Scenes.Antsscene.PhyllasKidsChildren.TimidNudge".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DaysPhyllaHasSpentBirthing) < 10)
                OutputText("Scenes.Antsscene.PhyllasKidsChildren.CorrectingSmile".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.DaysPhyllaHasSpentBirthing) < 40)
                    OutputText("Scenes.Antsscene.PhyllasKidsChildren.ConsiderableProgress".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllasKidsChildren.PridefulColonyTone".Localized());

            OutputText("Scenes.Antsscene.PhyllasKidsChildren.NoArenaReassure".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaTimesDriderEggLayed) > 0)
                OutputText("Scenes.Antsscene.PhyllasKidsChildren.InquireDriderKids".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaTimesDriderEggLayed) > 10)
                OutputText("Scenes.Antsscene.PhyllasKidsChildren.SighForlornDrider".Localized());

            OutputText("Scenes.Antsscene.PhyllasKidsChildren.SurveyColonyPride".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void PhyllaPearance()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
            {
                OutputText("Scenes.Antsscene.PhyllaPearance.HeightAndFrame".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.BreastBCup".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.TightPussyDetail".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.AssholeBetweenCheeks".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.BirthingSlitDetail".Localized());
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllaPearance.EggEngorgedAbdomen".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.MilkLadenBreasts".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.LubedPussyDetail".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.AssholeBetweenCheeksAgain".Localized());

                OutputText("Scenes.Antsscene.PhyllaPearance.BirthingSlitSpurts".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => IntroductionToPhyllaFollower());
        }

        public void PhyllaDigsForGems()
        {
            ClearOutput();

            var kidsMod = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 10)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 50)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 150)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 300)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 600)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 1000)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 2000)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaGemsHuntedToday) == 0 && Random.Shared.Next(20) + kidsMod > 10)
            {
                var gems = 0;

                gems = 10 + Random.Shared.Next(10) + kidsMod * 2;

                Player.Gems += gems;

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
                {
                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.AskFoundGems".Localized());

                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.HairRuffledGems".Localized());

                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.GainGems".Localized(
                         gems
                    ));

                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.AsksExcitedly".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.AskChildrenGems".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Antsscene.PhyllaDigsForGems.PatChildHead".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaDigsForGems.CountGemsFrown".Localized());

                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.AnythingElsePrompt".Localized());

                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.GainGemsAgain".Localized(
                         gems
                    ));
                }
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllaDigsForGems.AskMoreGemsSorry".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.AllRightLater".Localized());
                else
                    OutputText("Scenes.Antsscene.PhyllaDigsForGems.SighWeakPat".Localized());

                OutputText("Scenes.Antsscene.PhyllaDigsForGems.InquiresExcitedly".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.PhyllaGemsHuntedToday, 1);

            IntroductionToPhyllaFollower(false);
        }

        public void PhyllaStones()
        {
            ClearOutput();

            var kidsMod = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 10)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 50)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 150)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 300)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 600)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 1000)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.AntKids) > 2000)
                kidsMod += 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaStonesHuntedToday) == 0)
            {
                var stones = 2 + Random.Shared.Next(10) + (kidsMod * 2);

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinStoneResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) + stones);

                CoC.Instance.Flags.Set(GameFlagIds.AchievementProgressAntworks, CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressAntworks) + stones);

                if (CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressAntworks) >= 200)
                    AwardAchievement(AchievementIds.AntWorks);

                if (CoC.Instance.Flags.Get(GameFlagIds.PhyllaEggLaying) == 0)
                {
                    OutputText("Scenes.Antsscene.PhyllaStones.AskSpareStones".Localized());

                    OutputText("Scenes.Antsscene.PhyllaStones.MessHairStones".Localized((CoC.Instance.Flags.Get(GameFlagIds.AntKids) < 1 ? "I'm" : "we are")));

                    OutputText("Scenes.Antsscene.PhyllaStones.GainStones".Localized(
                         stones
                    ));

                    OutputText("Scenes.Antsscene.PhyllaStones.ExcitedAskStones".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.PhyllaStones.AskTakeStones".Localized());

                    if (Player.Corruption < 50 || stones >= 10)
                        OutputText("Scenes.Antsscene.PhyllaStones.PatChildHead".Localized());
                    else
                        OutputText("Scenes.Antsscene.PhyllaStones.CountStonesFrown".Localized());

                    OutputText("Scenes.Antsscene.PhyllaStones.AnythingElsePrompt".Localized());

                    OutputText("Scenes.Antsscene.PhyllaStones.GainStonesAgain".Localized(
                         stones
                    ));
                }

                CoC.Instance.Flags.Set(GameFlagIds.PhyllaStonesHuntedToday, 1);
            }
            else
            {
                OutputText("Scenes.Antsscene.PhyllaStones.AskStonesSorry".Localized());

                OutputText("Scenes.Antsscene.PhyllaStones.AnythingElsePromptAgain".Localized());
            }

            IntroductionToPhyllaFollower(false);
        }

        public void EggDatBitch()
        {
            ClearOutput();

            if (Player.CanOvipositSpider())
            {
                ShowImage("ant-camp-ant-mounted-by-drider");

                OutputText("Scenes.Antsscene.EggDatBitch.SatedAbdomenNeed".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEggImpregnatingPhylla) == 0)
            {
                OutputText("Scenes.Antsscene.EggDatBitch.BroodMotherOffer".Localized());

                OutputText("Scenes.Antsscene.EggDatBitch.ExplainEggHost".Localized());
            }
            else
                OutputText("Scenes.Antsscene.EggDatBitch.KnowingLookThrust".Localized());

            if (Random.Shared.Next(20) + 1 + Player.Intelligence / 20 < 10)
            {
                OutputText("Scenes.Antsscene.EggDatBitch.PondersRefuses".Localized());

                if (Player.IsPregnant())
                    OutputText("Scenes.Antsscene.EggDatBitch.NoRoomInside".Localized());
                else
                    OutputText("Scenes.Antsscene.EggDatBitch.NotComfortableLater".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Antsscene.EggDatBitch.BroodingConsideration".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.TimesEggImpregnatingPhylla) == 0)
                {
                    OutputText("Scenes.Antsscene.EggDatBitch.HowProceedAsk".Localized());

                    OutputText("Scenes.Antsscene.EggDatBitch.FingerToLips".Localized());
                }
                else
                {
                    OutputText("Scenes.Antsscene.EggDatBitch.UncertainPosition".Localized());

                    if (Player.Corruption > 75)
                        OutputText("Scenes.Antsscene.EggDatBitch.UnknowingBroodUse".Localized());
                }

                OutputText("Scenes.Antsscene.EggDatBitch.PoseHerRight".Localized());

                OutputText("Scenes.Antsscene.EggDatBitch.TongueSeeksKiss".Localized());

                OutputText("Scenes.Antsscene.EggDatBitch.BraceForEgg".Localized());

                OutputText("Scenes.Antsscene.EggDatBitch.MaternalCryHurt".Localized());

                OutputText("Scenes.Antsscene.EggDatBitch.EggsSlideSwell".Localized());

                if (Player.Corruption >= 75)
                    OutputText("Scenes.Antsscene.EggDatBitch.LinkAsksStop".Localized());

                OutputText("Scenes.Antsscene.EggDatBitch.RetractOvipositor".Localized());

                if (Player.Pregnancy.Incubation == 0)
                {
                    Player.Pregnancy.Type = Pregnancy.PregnancyIds.DriderEggs;

                    Player.Pregnancy.Incubation = 8 * 24;
                }

                CoC.Instance.Flags.Set(GameFlagIds.TimesEggImpregnatingPhylla, CoC.Instance.Flags.Get(GameFlagIds.TimesEggImpregnatingPhylla) + 1);

                Player.Orgasm("Ovi");

                Player.DumpEggs();
            }

            DoNext(() => ReturnToCamp(1));
        }

    }
}