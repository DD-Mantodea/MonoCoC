using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Registry;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<HelScene>("coc:hel")]
    public class HelScene : Scene
    {
        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Hel)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        // 原版 MarbleScene.hx:499 marbleFollower()：以 CampMarble 状态判定
        public static bool marbleFollower() => CoC.Instance.Player.HasStatusEffect(StatusEffectIds.CampMarble);

        public static bool IsabellaAccent() => CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) > 0 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) >= 100;

        public static bool isAprilFools() => DateTime.Now.Month == 4 && DateTime.Now.Day == 1;

        public static double Floor(double value) => Math.Floor(value);

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Hel);

        public override void Entry()
        {

        }

        public void SpriteChooser()
        {

        }

        public bool FollowerHel()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) == 2)
                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 1)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) > 0)
                return true;

            return false;
        }

        public bool FuckBuddyHel()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.HelFuckbuddy) == 1;
        }

        public int HeliaSparIntensity()
        {

            var amount = 0;

            amount += (int)Floor(CoC.Instance.Flags.Get(GameFlagIds.HelBonusPoints) / 5);

            amount += CoC.Instance.Flags.Get(GameFlagIds.HeliaSparVictories);

            if (amount > 100)
                amount = 100;

            return amount;
        }

        public void EncounterAJerkInThePlains()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelTimesEncountered, CoC.Instance.Flags.Get(GameFlagIds.HelTimesEncountered) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelAffection) > 0 && CoC.Instance.Flags.Get(GameFlagIds.HelAffection) % 5 == 0 && CoC.Instance.Flags.Get(GameFlagIds.PcPromisedHelMonogamyFucks) == 0)
            {
                HelMinotaurThreesome();

                return ;
            }

            if (FuckBuddyHel())
            {
                GreetHelAsFuckbuddies();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.HelTimesEncountered) == 1)
            {
                OutputText("Scenes.Helscene.EncounterAJerkInThePlains.TallGrassPlains".Localized());

                OutputText("Scenes.Helscene.EncounterAJerkInThePlains.PiercingCryHill".Localized());

                OutputText("Scenes.Helscene.EncounterAJerkInThePlains.ArriveBattleWindingDown".Localized());

                OutputText("Scenes.Helscene.EncounterAJerkInThePlains.StandingOverAnotherWoman".Localized());
            }
            else
            {
                OutputText("Scenes.Helscene.EncounterAJerkInThePlains.ExploringGnollCampSavagery".Localized());

                OutputText("Scenes.Helscene.EncounterAJerkInThePlains.AceBattleStance".Localized());
            }

            Player.UnlockCodexEntry(GameFlagIds.CodexEntrySalamanders);

            var helEnemy = EnemyRegistry.Create(new Identifier("coc", "hel"));

            helEnemy.OnDefeated = _ => HelDefeatedNormal();

            helEnemy.OnWon = _ => LoseToSalamander();

            CombatScene.Start(helEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void GreetHelAsFuckbuddies()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.GreetHelAsFuckbuddies.WideOpenPlains".Localized());

            OutputText("Scenes.Helscene.GreetHelAsFuckbuddies.HeyThereTightHug".Localized());

            if (Player.Lust < 40)
            {
                Player.Lust = 40;

                Player.ApplyStats(new DynStat(GameStats.Lust, 1f));
            }

            HelFuckMenu();
        }

        public void PostHelFuckBuddyFollowup()
        {
            ClearOutput();

            SpriteChooser();

            if (FollowerHel())
            {
                Save.SetInt("lastTimeSex", View.Time.Days);

                ReturnToCamp(1);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFuckbuddy) == 0)
            {
                ReturnToCamp(1);

                return ;
            }

            OutputText("Scenes.Helscene.PostHelFuckBuddyFollowup.WakeHourLaterSnuggled".Localized());

            OutputText("Scenes.Helscene.PostHelFuckBuddyFollowup.RedressTeasingFlirting".Localized());

            ReturnToCamp(1);
        }

        public void LoseToSalamander()
        {
            ClearOutput();

            SpriteChooser();

            if (Player.Health < 1)
                OutputText("Scenes.Helscene.LoseToSalamander.CollapseFewStepsAway".Localized());
            else
                OutputText("Scenes.Helscene.LoseToSalamander.ArousalTooGreat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelTimesEncountered) == 1)
                OutputText("Scenes.Helscene.LoseToSalamander.LyingFewPacesGnoll".Localized());
            else
                OutputText("Scenes.Helscene.LoseToSalamander.LookUpSalamanderLooming".Localized());

            int x = 0;

            if (Random.Shared.Next(4) == 0 && (int)Player.Gender > 0)
            {
                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => HelTailPegging(true));

                return ;
            }

            if (Player.HasCock() && (Player.Gender == Gender.Male || Random.Shared.Next(4) < 3) && Player.HasCockThatFits(85))
            {
                x = Player.CockThatFits(85);

                OutputText("Scenes.Helscene.LoseToSalamander.PullOffArmorPracticed".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Helscene.LoseToSalamander.RevealingYourCock".Localized(
                        Player.VaginaDescript(0)
                    ));

                OutputText("Scenes.Helscene.LoseToSalamander.LettingFlopFree".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Helscene.LoseToSalamander.MoansLustilyTaste".Localized());

                OutputText("Scenes.Helscene.LoseToSalamander.ReadyBestFuckSun".Localized());

                OutputText("Scenes.Helscene.LoseToSalamander.YouLikeTheHeat".Localized());

                OutputText("Scenes.Helscene.LoseToSalamander.SuckingOnTail".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Helscene.LoseToSalamander.FinallyNoMorePressure".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Helscene.LoseToSalamander.VagSquirtGirlcum".Localized());

                OutputText("Scenes.Helscene.LoseToSalamander.GaspsSensationAnotherShot".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Helscene.LoseToSalamander.CollapsesAtopAwesome".Localized());
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Helscene.LoseToSalamander.AlwaysLovedTumbleWoman".Localized());

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Helscene.LoseToSalamander.NotExactlyAWoman".Localized(
                            Player.CockDescript(x)
                        ));

                    OutputText("Scenes.Helscene.LoseToSalamander.StepForwardKneelsShoulders".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.LiftFaceBetweenLegs".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.FewMinutesBuckHips".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.HellNoGraspingForehead".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.SuddenTickleLips".Localized(
                        Player.VaginaDescript(0),

                        Player.VaginaDescript(0)
                    ));

                    Player.CuntChange(20, true, true, false);

                    OutputText("Scenes.Helscene.LoseToSalamander.ParagraphBreak".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.GoodThingsEnd".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Helscene.LoseToSalamander.CockShootingRopeSpunk".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.PeriodBreak".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.BeforeMindSettledCollapsed".Localized(
                        Player.VaginaDescript(0)
                    ));
                }
                else
                {
                    if (Player.Gender == Gender.Genderless)
                        OutputText("Scenes.Helscene.LoseToSalamander.WhatHaveWeHere".Localized());
                    else
                        if (Player.HasCock())
                            OutputText("Scenes.Helscene.LoseToSalamander.HotDamnImpressive".Localized());
                        else
                            OutputText("Scenes.Helscene.LoseToSalamander.HotDamnImpressiveAgain".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.FaceBetweenLegsAgain".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.MinutesBuckHipsAgain".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.GraspForeheadAgain".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.GaspSuddenPressureRim".Localized(
                        Player.AssholeDescript()
                    ));

                    Player.Orgasm("Anal");

                    Player.ButtChange(20, true, true, false);

                    OutputText("Scenes.Helscene.LoseToSalamander.ParagraphBreakAgain".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.GoodThingsEndAgain".Localized());

                    OutputText("Scenes.Helscene.LoseToSalamander.MindSettlesCollapseAgain".Localized());
                }

            if (Player.HasCock())
                Player.Orgasm("Dick");
            else
                Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            CombatScene.CleanupAfterCombat();
        }

        public void BeatUpHel()
        {
            ClearOutput();

            SpriteChooser();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Helscene.BeatUpHel.UnableToWithstandKnee".Localized());
            else
                OutputText("Scenes.Helscene.BeatUpHel.PantingKneesShakingCollapses".Localized());

            OutputText("Scenes.Helscene.BeatUpHel.ParagraphBreak".Localized());

            if (Player.Corruption < 85)
                HelDefeatedNormal();
            else
                HelDefeatedCorrupt();
        }

        public void HelDefeatedCorrupt()
        {

            SpriteChooser();

            OutputText("Scenes.Helscene.HelDefeatedCorrupt.HuffingPuffingAdvantage".Localized());

            ClearButtons();

            if (Player.Lust < 33)
            {
                if (Player.Lust < 33)
                    OutputText("Scenes.Helscene.HelDefeatedCorrupt.NotTurnedOnEnough".Localized());
            }
            else
            {
                if (Player.HasCock())
                    if (Player.HasCockThatFits(85))
                        AddButton(0, "Common.Button.RapeAss".Localized(), RapingHelsAssMeansYourCorruptCauseAnalIsEvil);
                    else
                        OutputText("Scenes.Helscene.HelDefeatedCorrupt.TooBigForAnal".Localized());

                AddButton(1, "Common.Button.GetRimjob".Localized(), ReceiveCorruptRimjobsFromHel);
            }

            AddButton(4, "Common.Button.Wait".Localized(), () => HelDefeatedNormal(true));
        }

        public void HelDefeatedNormal(bool clear = false)
        {
            ClearOutput();

            SpriteChooser();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelTimesEncountered) == 1)
            {
                OutputText("Scenes.Helscene.HelDefeatedNormal.LooksUpBeamsThanks".Localized());

                OutputText("Scenes.Helscene.HelDefeatedNormal.OhManShakingHead".Localized());

                OutputText("Scenes.Helscene.HelDefeatedNormal.GrinsTossingSword".Localized());
            }
            else
            {
                OutputText("Scenes.Helscene.HelDefeatedNormal.LooksUpBeamsDamn".Localized());

                OutputText("Scenes.Helscene.HelDefeatedNormal.ToTheVictorSpoils".Localized());
            }

            HelFuckMenu();
        }

        public void HelFuckMenu(bool isAmbush = false)
        {

            SpriteChooser();

            Action leave = DeclineHelSexings;

            if (isAmbush)
                leave = PussyOutOfHelSexAmbush;

            ClearButtons();

            AddButton(0, "Common.Button.GetLicked".Localized(), () => { });

            AddButton(1, "Scenes.Kitsune.DefeatTheKitsunes.ButtonFuckHerVag".Localized(), null);

            AddButton(2, "Common.Button.Fuckherass".Localized(), null);

            AddButton(3, "Common.Button.Gettailpegged".Localized(), null);

            AddButton(4, "Common.Button.TailWank".Localized(), null);

            AddButton(5, "Common.Button.Doublepen".Localized(), null);

            AddButton(6, "Common.Button.GetBlown".Localized(), null);

            AddButton(7, "Common.Button.Coilfuck".Localized(), null);

            AddButton(8, "Common.Button.Tailsinbutts".Localized(), null);

            AddButton(9, "Common.Button.MountHer".Localized(), null);

            AddButton(10, "Common.Button.Hanging69".Localized(), null);

            AddButton(11, "Common.Button.Possess".Localized(), null);

            if (Player.Lust < 33)
            {
                OutputText("Scenes.Helscene.HelFuckMenu.NotUpForSex".Localized());

                AddButton(14, "Common.Button.Leave".Localized(), leave);

                return ;
            }

            Action? getLicked = null;

            if (Player.HasVagina())
                getLicked = GetLickedByHel;

            Action? fuckVag = null;

            Action? fuckAss = null;

            Action? tailWank = null;

            Action? dp = null;

            Action? getBlown = null;

            if (isAprilFools() && CoC.Instance.Flags.Get(GameFlagIds.DlcAprilFools) == 0)
            {
                OutputText("Scenes.Helscene.HelFuckMenu.HeliaDlcRequired".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.GetDlc".Localized(), () => { });

                AddButton(14, "Common.Button.Leave".Localized(), DeclineHelSexings);

                return ;
            }

            if (Player.HasCock())
            {
                if (Player.HasCockThatFits(85))
                {
                    fuckVag = BeatUpHelAndStealHerWalletFromHerVagina;

                    fuckAss = FuckHelsAss;
                }
                else
                    OutputText("Scenes.Helscene.HelFuckMenu.TooBigToPenetrate".Localized());

                tailWank = HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora;

                if (Player.CockTotal() > 1 && Player.HasCockThatFits(85) && Player.CockThatFits2(85) >= 0)
                    dp = DpHel;

                getBlown = HelBlowsYou;
            }

            if (Player.IsNaga())
            {
                if (Player.HasCockThatFits(85))
                    AddButton(7, "Common.Button.Coilfuck".Localized(), NagaCoilsUpHel);

                AddButton(8, "Common.Button.Tailsinbutts".Localized(), NagaCoilsUpAnalNaga);
            }

            if (Player.IsTaur())
            {
                if (Player.HasCockThatFits(85))
                    AddButton(9, "Common.Button.MountHer".Localized(), MountHel);

                if (Player.HasVagina())
                    AddButton(10, "Common.Button.Hanging69".Localized(), HelVaginaTaur69);
            }

            if (Player.HasPerk(PerkIds.Incorporeality))
                AddButton(11, "Common.Button.Possess".Localized(), HelPossessionShitPoopCock);

            AddButton(0, "Common.Button.GetLicked".Localized(), getLicked);

            AddButton(1, "Scenes.Kitsune.DefeatTheKitsunes.ButtonFuckHerVag".Localized(), fuckVag);

            AddButton(2, "Common.Button.Fuckherass".Localized(), fuckAss);

            AddButton(3, "Common.Button.Gettailpegged".Localized(), () => HelTailPegging());

            AddButton(4, "Common.Button.TailWank".Localized(), tailWank);

            AddButton(5, "Common.Button.Doublepen".Localized(), dp);

            AddButton(6, "Common.Button.GetBlown".Localized(), getBlown);

            AddButton(14, "Common.Button.Leave".Localized(), leave);
        }

        public void DeclineHelSexings()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.DeclineHelSexings.ThankPolitelyDecline".Localized());

            if (CombatController.InCombat)
                OutputText("Scenes.Helscene.DeclineHelSexings.AwHellNoFun".Localized());

            OutputText("Scenes.Helscene.DeclineHelSexings.CatchYouNextTime".Localized());

            OutputText("Scenes.Helscene.DeclineHelSexings.NodReturnWave".Localized());

            SceneInstance<HelFollowerScene>.Get().HelAffection(1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void BeatUpHelAndStealHerWalletFromHerVagina()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-vagfuck");

            int x = Player.CockThatFits(0);

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.SureBlowOffSteam".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.TipBrushesBurningHot".Localized(
                Player.AssDescript()
            ));

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.GripHipsBeginThrusting".Localized(
                Player.AssDescript()
            ));

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.KeepUpFewMinutes".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.GaspsSuddenChangeWolfishly".Localized());

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.FinallyCumGritTeeth".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.ReleasingBreastsFinger".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.RunHandsThroughHair".Localized());

            OutputText("Scenes.Helscene.BeatUpHelAndStealHerWalletFromHerVagina.ExhaustedCollapseCleavage".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void FuckHelsAss()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-buttfuck");

            int x = Player.CockThatFits(0);

            OutputText("Scenes.Helscene.FuckHelsAss.BlowOffSteam".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.KnowWhatWeWant".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.DropToKneesStroke".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.AllEncouragementLineUp".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.FuckHelsAss.PullOutPressure".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.GruntPleasureThrustBack".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.YeahFuckMyAss".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.MakesOwnOrgasmBetter".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.GrinningEarToEar".Localized());

            OutputText("Scenes.Helscene.FuckHelsAss.StrokesCheekDriftOff".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void HelBlowsYou()
        {
            ClearOutput();

            SpriteChooser();

            var x = Player.BiggestCockIndex();

            ShowImage("helia-getblown");

            OutputText("Scenes.Helscene.HelBlowsYou.AgreePropositionKnees".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelBlowsYou.MouthUnnaturallyHot".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelBlowsYou.TakesTitsInHand".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length >= 6)
                OutputText("Scenes.Helscene.HelBlowsYou.MakingHerDeepthroat".Localized());

            OutputText("Scenes.Helscene.HelBlowsYou.CumFillMouth".Localized());

            if (Player.CumQ() <= 30)
                OutputText("Scenes.Helscene.HelBlowsYou.SquirtsWord".Localized());
            else
                if (Player.CumQ() <= 100)
                    OutputText("Scenes.Helscene.HelBlowsYou.RopesWord".Localized());
                else
                    OutputText("Scenes.Helscene.HelBlowsYou.SpraysWord".Localized());

            OutputText("Scenes.Helscene.HelBlowsYou.JismPullOutSwallows".Localized());

            OutputText("Scenes.Helscene.HelBlowsYou.ExhaustedCollapseSoftTits".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void HelBlowsYouCamp()
        {
            ClearOutput();

            SpriteChooser();

            var x = Player.BiggestCockIndex();

            ShowImage("helia-getblown");

            OutputText("Scenes.Helscene.HelBlowsYouCamp.SomethingHeavyShuffles".Localized());

            OutputText("Scenes.Helscene.HelBlowsYouCamp.LessDelicateShove".Localized());

            OutputText("Scenes.Helscene.HelBlowsYouCamp.StandStillSuckCock".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelBlowsYouCamp.MouthUnnaturallyHotAgain".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelBlowsYouCamp.TakesTitsAgain".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length >= 6)
                OutputText("Scenes.Helscene.HelBlowsYouCamp.MakingHerDeepthroatAgain".Localized());

            OutputText("Scenes.Helscene.HelBlowsYouCamp.CumFillMouthAgain".Localized());

            if (Player.CumQ() <= 30)
                OutputText("Scenes.Helscene.HelBlowsYouCamp.SquirtsWordAgain".Localized());
            else
                if (Player.CumQ() <= 100)
                    OutputText("Scenes.Helscene.HelBlowsYouCamp.RopesWordAgain".Localized());
                else
                    OutputText("Scenes.Helscene.HelBlowsYouCamp.SpraysWordAgain".Localized());

            OutputText("Scenes.Helscene.HelBlowsYouCamp.PullOutSwallows".Localized());

            OutputText("Scenes.Helscene.HelBlowsYouCamp.CollapseSoftTits".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void DpHel()
        {
            ClearOutput();

            SpriteChooser();

            int x = Player.CockThatFits(0);

            var y = Player.CockThatFits2(0);

            ShowImage("helia-doublepenetration");

            OutputText("Scenes.Helscene.DpHel.YesUndoArmor".Localized(
                Player.CockDescript(y)
            ));

            OutputText("Scenes.Helscene.DpHel.SitDownHighGrass".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y)
            ));

            OutputText("Scenes.Helscene.DpHel.RisingUpKneesPositioning".Localized(
                Player.CockDescript(x),

                Player.CockDescript(y)
            ));

            OutputText("Scenes.Helscene.DpHel.GottaAdmitSharpBreath".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesHeliaDoubleDonged) == 0)
                OutputText("Scenes.Helscene.DpHel.FirstMultiCock".Localized());
            else
                OutputText("Scenes.Helscene.DpHel.AmazingNeverGetUsed".Localized());

            OutputText("Scenes.Helscene.DpHel.SlamsHerselfDownGasp".Localized());

            OutputText("Scenes.Helscene.DpHel.YeahFuckMyHoles".Localized());

            OutputText("Scenes.Helscene.DpHel.OwnOrgasmBetterAgain".Localized());

            OutputText("Scenes.Helscene.DpHel.HotDamnGrinningStarts".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.TimesHeliaDoubleDonged, CoC.Instance.Flags.Get(GameFlagIds.TimesHeliaDoubleDonged) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void GetLickedByHel()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-getlicked");

            OutputText("Scenes.Helscene.GetLickedByHel.AgreePropositionSlipOut".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Helscene.GetLickedByHel.GripsAssScalyHands".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Helscene.GetLickedByHel.LickThenPressure".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Helscene.GetLickedByHel.EndureMinuteComeScreaming".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Helscene.GetLickedByHel.WhileEachCockShoots".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Helscene.GetLickedByHel.BathingStreamsJism".Localized());
            }

            OutputText("Scenes.Helscene.GetLickedByHel.ShudderGaspForkedTongue".Localized());

            OutputText("Scenes.Helscene.GetLickedByHel.LittleLaughStrokesCheek".Localized());

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-tailwank");

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Helscene.HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora.ThinkThenSaunterUp".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora.ReclinesTailSnake".Localized(
                Player.CockDescript(x),

                Player.AssDescript(),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora.ShudderGaspTailCoils".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora.CmonNowFriend".Localized());

            OutputText("Scenes.Helscene.HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora.ReadyToCurseSqueeze".Localized());

            OutputText("Scenes.Helscene.HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora.FinallyNoMoreWrings".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.HelTailWanksYourDickBecauseSheLovesYouDesuDesuHoraHora.ReleasesCollapseLap".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void HelTailPegging(bool loss = false)
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-tailpeg");

            if (!loss)
                OutputText("Scenes.Helscene.HelTailPegging.AgreePropositionStripArmor".Localized());
            else
                OutputText("Scenes.Helscene.HelTailPegging.HandsKneesSpreadCheeks".Localized());

            OutputText("Scenes.Helscene.HelTailPegging.DoAsksAllFours".Localized());

            OutputText("Scenes.Helscene.HelTailPegging.GritTeethClenchFists".Localized());

            Player.ButtChange(40, true, true, false);

            OutputText("Scenes.Helscene.HelTailPegging.ParagraphBreak".Localized());

            OutputText("Scenes.Helscene.HelTailPegging.FirmlyDepthsPlantsHands".Localized());

            OutputText("Scenes.Helscene.HelTailPegging.EventuallyNoMoreClimax".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Helscene.HelTailPegging.WithYourCock".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Helscene.HelTailPegging.CockWord".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Helscene.HelTailPegging.AndWord".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Helscene.HelTailPegging.VaginaWord".Localized());

                OutputText("Scenes.Helscene.HelTailPegging.ContractingSquiringLover".Localized());
            }

            OutputText("Scenes.Helscene.HelTailPegging.WrithingTailWithdraws".Localized());

            OutputText("Scenes.Helscene.HelTailPegging.CollapsedWrithingTenderness".Localized());

            Player.Orgasm("Anal");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                PostHelFuckBuddyFollowup();
        }

        public void NagaCoilsUpHel()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-nagacoil");

            int x = Player.CockThatFits(85);

            var y = Player.CockThatFits2(85);

            OutputText("Scenes.Helscene.NagaCoilsUpHel.SlitherCloserWorkOff".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.GetIdeaDiscardArmor".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.NagaCoilsUpHel.LoverAtMercy".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Helscene.NagaCoilsUpHel.AssUpWithCock".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Helscene.NagaCoilsUpHel.StartToLowerShaft".Localized());

            if (y >= 0)
                OutputText("Scenes.Helscene.NagaCoilsUpHel.PluralS".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.GaspsSquirmsDeeper".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.ForceDownSlammingHips".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.RewardPullingHalfwayOff".Localized());

            if (y >= 0)
                OutputText("Scenes.Helscene.NagaCoilsUpHel.PluralSAgain".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.SlammingBackDownTail".Localized());

            if (y >= 0)
                OutputText("Scenes.Helscene.NagaCoilsUpHel.PluralSThird".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.HarderThanEverTear".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.NotQuiteDoneYet".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.NagaCoilsUpHel.SoonFeelOrgasmTitfuck".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.ExhaustedByThe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFuckbuddy) == 0)
                OutputText("Scenes.Helscene.NagaCoilsUpHel.BattleAnd".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpHel.MassiveOrgasmCollapseBack".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => PostHelFuckBuddyFollowup());
        }

        public void NagaCoilsUpAnalNaga()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-nagaanal");

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.WorkOffSteamAgain".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.GetIdeaDiscardBoth".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.BothTailsLubricatedCrawls".Localized());

            Player.ButtChange(40, true, true, false);

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.ParagraphBreak".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.ContinuesForceThickeningTail".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.RiseOnTail".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.RamDownHotTail".Localized());

            OutputText("Scenes.Helscene.NagaCoilsUpAnalNaga.SensesLyingOnBack".Localized());

            Player.Orgasm("Anal");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => PostHelFuckBuddyFollowup());
        }

        public void MountHel()
        {
            ClearOutput();

            SpriteChooser();

            int x = Player.CockThatFits(85);

            if (x < 0)
                x = Player.SmallestCockIndex();

            ShowImage("helia-mount");

            OutputText("Scenes.Helscene.MountHel.TrotUpFantasies".Localized());

            OutputText("Scenes.Helscene.MountHel.UnexpectedButHey".Localized());

            OutputText("Scenes.Helscene.MountHel.GiddyupHuskyWhisper".Localized());

            OutputText("Scenes.Helscene.MountHel.StartTrottingArmsGuide".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.MountHel.ArriveSlipsGracefully".Localized());

            OutputText("Scenes.Helscene.MountHel.StifleChuckleRemind".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.MountHel.NakedBeautifulCurvy".Localized());

            OutputText("Scenes.Helscene.MountHel.LittleNervousBreakingHer".Localized(
                Player.CockDescript(x),

                NumberUtils.Num2Text((int)(Player.Cocks[x].Length))
            ));

            OutputText("Scenes.Helscene.MountHel.ComeOnDammitSheened".Localized());

            OutputText("Scenes.Helscene.MountHel.HammerRelentlessBrutal".Localized());

            OutputText("Scenes.Helscene.MountHel.FuckRutThrustCry".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Helscene.MountHel.BeginsToPourOut".Localized());

            OutputText("Scenes.Helscene.MountHel.PeriodBreak".Localized());

            OutputText("Scenes.Helscene.MountHel.StayLikeThatMinutes".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.MountHel.SureCantConvinceWarhorse".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Helscene.MountHel.DutiesPreventAccepting".Localized());
            else
                OutputText("Scenes.Helscene.MountHel.CantImagineCommitting".Localized());

            OutputText("Scenes.Helscene.MountHel.ComeBackLittlePony".Localized());

            OutputText("Scenes.Helscene.MountHel.ChucklingAssureBack".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => PostHelFuckBuddyFollowup());
        }

        public void HelVaginaTaur69()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.HelVaginaTaur69.TrotUpTellFantasies".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.UnexpectedButWeirder".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.StartTrottingHandsWithers".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Helscene.HelVaginaTaur69.StifleChuckleRemindObligation".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.StartingToThinkHad".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.AfterMomentSettlesStill".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Helscene.HelVaginaTaur69.HardlyPreparedSlamsArm".Localized());

            Player.CuntChange(50, true, true, false);

            OutputText("Scenes.Helscene.HelVaginaTaur69.ScreamBuckThrowingHarness".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Helscene.HelVaginaTaur69.HammersCervixFistFucking".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Helscene.HelVaginaTaur69.CalmerGraspOneCock".Localized());

                if (Player.ShortestCockLength() <= 24)
                    OutputText("Scenes.Helscene.HelVaginaTaur69.BuriedToTheHilt".Localized());

                OutputText("Scenes.Helscene.HelVaginaTaur69.MoanLewdlySuckTime".Localized());
            }

            OutputText("Scenes.Helscene.HelVaginaTaur69.FeelsOneSidedReach".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.WickedSmileSnatchTail".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.HelVaginaTaur69.FromAroundEncapsulated".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.GoodGripRedTail".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.HasCock())
                OutputText("Scenes.Helscene.HelVaginaTaur69.AndSwellingMember".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.PeriodBreak".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.CannotGoOnForever".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.HasCock())
                OutputText("Scenes.Helscene.HelVaginaTaur69.AndShaftWord".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.NotAloneTailFucking".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.HelVaginaTaur69.FillingHerThroatJism".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.PeriodBreakAgain".Localized());

            OutputText("Scenes.Helscene.HelVaginaTaur69.TakesMinuteBodyCalm".Localized());

            Player.Orgasm("Vaginal");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => PostHelFuckBuddyFollowup());
        }

        public void HelPossessionShitPoopCock()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.TakingAdvantageIncorporeal".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.GoingGhostLunge".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.ShockedGaspGhostly".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.WhatTheFuckHost".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.SeesApproachingLeapsTerror".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.LetHerCumMurder".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.MomentContentmentThrown".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.OnlyJustAbleSwitch".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.AloneInHerMind".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.EyesGoWideDistends".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.EggEggGonnaEgg".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.FuckingEggsMindNumbing".Localized());

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.Helscene.HelPossessionShitPoopCock.ShareTheFun".Localized());

                OutputText("Scenes.Helscene.HelPossessionShitPoopCock.EyesRollBackTongue".Localized());
            }
            else
            {
                OutputText("Scenes.Helscene.HelPossessionShitPoopCock.OnlyNodTittyFucks".Localized());

                OutputText("Scenes.Helscene.HelPossessionShitPoopCock.EggsShootOutForce".Localized());
            }

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.OrgasmSubsidedPullOut".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.FuckYouLaughingDollop".Localized());

            OutputText("Scenes.Helscene.HelPossessionShitPoopCock.LaughContentedYawn".Localized());

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => PostHelFuckBuddyFollowup());
        }

        public void RapingHelsAssMeansYourCorruptCauseAnalIsEvil()
        {
            ClearOutput();

            SpriteChooser();

            int x = Player.CockThatFits(85);

            var y = Player.CockThatFits2(85);

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.AlreadyGettingHardCircle".Localized(
                Player.ArmorName
            ));

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.TooExhaustedSkilledGrab".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.CockIntoWaitingVag".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.VulnerableScreamsSurprise".Localized());

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.TightAsSilkenVice".Localized());

            if (y >= 0)
                OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.AndVaginaWord".Localized());

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.PeriodBreak".Localized());

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.PullOutLooking".Localized());

            if (y >= 0)
                OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.PluralS".Localized());

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.MilkingTautMuscles".Localized());

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.FuckMyAssScreams".Localized());

            if (y >= 0)
                OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.AndVagWord".Localized());

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.FillingSeedDrained".Localized());

            OutputText("Scenes.Helscene.RapingHelsAssMeansYourCorruptCauseAnalIsEvil.YankOutCollapsesSide".Localized());

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) - 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(-15);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => PostHelFuckBuddyFollowup());
        }

        public void ReceiveCorruptRimjobsFromHel()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.SightBerserkerKnees".Localized());

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.GetsIdeaForkedTongue".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.TonguePenaltySwat".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.ParagraphBreak".Localized());

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.FewMomentsTongueTraces".Localized());

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.DepthsPlantsHands".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.GropingYourTits".Localized());

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.BuckHipsSmashingGroin".Localized());

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.NoMoreClimaxAgain".Localized());

            OutputText("Scenes.Helscene.ReceiveCorruptRimjobsFromHel.PantingGrabGems".Localized());

            Player.Orgasm("Anal");

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) - 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(-15);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => PostHelFuckBuddyFollowup());
        }

        public void HelMinotaurThreesome()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelTalkedAboutBerserking, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelTalkedAboutAttackingYou, 0);

            ShowImage("helia-threesome-minotaur");

            OutputText("Scenes.Helscene.HelMinotaurThreesome.WanderGrasslandsWipeBrow".Localized());

            OutputText("Scenes.Helscene.HelMinotaurThreesome.ContinueWalkAimlesslyMountains".Localized());

            OutputText("Scenes.Helscene.HelMinotaurThreesome.WellItsAMinotaur".Localized());

            OutputText("Scenes.Helscene.HelMinotaurThreesome.NoticesApproachStopsGroping".Localized());

            OutputText("Scenes.Helscene.HelMinotaurThreesome.CantHelpButLaugh".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.HelMinotaurThreesome.GotAnotherHoleNeeds".Localized());
            else
                OutputText("Scenes.Helscene.HelMinotaurThreesome.TaurToGoAround".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(85))
                AddButton(0, "Common.Button.Fuckherass".Localized(), FuckHerAss);
            else
                AddButton(0, "Common.Button.Fuckherass".Localized(), null);

            AddButton(1, "Common.Button.MinoLick".Localized(), HelMinoThreeSomeLickItsDick);

            AddButton(2, "Common.Button.Minosrbad".Localized(), MinosRBadNoFuck);

            AddButton(4, "Common.Button.Nope".Localized(), LeaveMinotaurHelThreesome);
        }

        public void MinosRBadNoFuck()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.MinosRBadNoFuck.WonderIfUnawareDangers".Localized());

            OutputText("Scenes.Helscene.MinosRBadNoFuck.ApproachAttemptPryApart".Localized());

            OutputText("Scenes.Helscene.MinosRBadNoFuck.TellAddictiveSubstance".Localized());

            OutputText("Scenes.Helscene.MinosRBadNoFuck.HmmCockingEyebrow".Localized());

            if (!Player.HasPerk(PerkIds.HistoryAlchemist))
                OutputText("Scenes.Helscene.MinosRBadNoFuck.JustHowItIs".Localized((marbleFollower() ? ", just like how cow-girls have addictive breastmilk" : "")));
            else
                OutputText("Scenes.Helscene.MinosRBadNoFuck.GuessCorruptionHormone".Localized());

            OutputText("Scenes.Helscene.MinosRBadNoFuck.LightlyScowlsAbsorbs".Localized());

            DoNext(() => MinosRBadNoFuck2());
        }

        public void MinosRBadNoFuck2()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.MinosRBadNoFuck2.DontWannaGetHooked".Localized());

            OutputText("Scenes.Helscene.MinosRBadNoFuck2.SupposeOfferSatisfy".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ItsOkay".Localized(), HelCanFuckMinosWhenever);

            AddButton(1, "Common.Button.SatisfyHer".Localized(), SatisfyHelSoSheStopsMinoFucking);
        }

        public void LeaveMinotaurHelThreesome()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.LeaveMinotaurHelThreesome.ShakeHeadDeprecatingLeave".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckHerAss()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-threesome-minotaur-buttfuck");

            int x = Player.CockThatFits(85);

            if (x < 0)
                x = 0;

            OutputText("Scenes.Helscene.FuckHerAss.ChanceAssSlipBy".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.FuckHerAss.MoveHandsRecoilFurred".Localized());

            OutputText("Scenes.Helscene.FuckHerAss.RoleMostlyPassiveBuck".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.FuckHerAss.ScreamsHighHeavensSpasming".Localized());

            OutputText("Scenes.Helscene.FuckHerAss.SightBullCreamingEdge".Localized());

            OutputText("Scenes.Helscene.FuckHerAss.MomentStillnessTightensRolls".Localized());

            OutputText("Scenes.Helscene.FuckHerAss.BurstsOutLaughingTaur".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            DoNext(() => PostMinoThreesomeDecisionTime());
        }

        public void HelMinoThreeSomeLickItsDick()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-threesome-minotaur-minolick");

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.CertainlyNotPassShed".Localized(
                Player.ArmorName
            ));

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.NastySlapCheek".Localized());

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.HappilyBuckMuzzle".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.CrotchWord".Localized());
            else
                OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.AssWord".Localized());

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.AnnouncingImpendingOrgasm".Localized());

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.BullyForYouHammering".Localized());

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.MightyBellowComes".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.WhileEachCockEjects".Localized());

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.PeriodBreak".Localized());

            OutputText("Scenes.Helscene.HelMinoThreeSomeLickItsDick.OrgasmEndsRollOff".Localized());

            Player.Orgasm("VaginalAnal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) + 1);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.HelFuckCounter) + 1);

            DoNext(() => PostMinoThreesomeDecisionTime());
        }

        public void PostMinoThreesomeDecisionTime()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.PostMinoThreesomeDecisionTime.BreaksKissLeansBack".Localized());

            HelChatMenu();
        }

        public void BugOutAfterHelMinoThreesome()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.BugOutAfterHelMinoThreesome.SecondThoughtKeepSimple".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void BerserkMode()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.BerserkMode.TakeOpportunityAskFighting".Localized());

            OutputText("Scenes.Helscene.BerserkMode.StrikinglyGirlishEverBeen".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelIsabellaThreesomeEnabled) == 0)
                OutputText("Scenes.Helscene.BerserkMode.FatCowIsabella".Localized());

            OutputText("Scenes.Helscene.BerserkMode.JustGoOffLook".Localized());

            OutputText("Scenes.Helscene.BerserkMode.KissesYouAgain".Localized());

            OutputText("Scenes.Helscene.BerserkMode.BetterThanHalfDecent".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelTalkedAboutBerserking, 1);

            HelChatMenu();
        }

        public void AskHelAboutHer()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.AskHelAboutHer.ThinkThenAsk".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.WhoMeSeeminglySurprised".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.LaughExtendHandName".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.ImHeliaOrHel".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.TellRoleChampionVillage".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.RecoverAskStory".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.NothingSoGrandioseNesting".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.EventuallyOnlyOneLeft".Localized());

            OutputText("Scenes.Helscene.AskHelAboutHer.AnotherKissFallsSilent".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelTalkedAboutHer, 1);

            HelChatMenu();
        }

        public void AskHelAboutAttackingYou()
        {
            ClearOutput();

            SpriteChooser();

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckbuddy, 0);

            OutputText("Scenes.Helscene.AskHelAboutAttackingYou.FeistyCalmAskWhy".Localized());

            OutputText("Scenes.Helscene.AskHelAboutAttackingYou.CantControlMyself".Localized());

            OutputText("Scenes.Helscene.AskHelAboutAttackingYou.SupposeAskStopAttacking".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelTalkedAboutAttackingYou, 1);

            ClearButtons();

            AddButton(0, "Common.Button.Stop".Localized(), TelHelSTOPATTACKINGMEYOUBITCH);

            AddButton(1, "Common.Button.SayNothing".Localized(), HelChatMenu);
        }

        public void HelChatMenu()
        {

            SpriteChooser();

            ShowImage("helia-interview");

            if (CoC.Instance.Flags.Get(GameFlagIds.HelTalkedAboutHer) == 0)
            {
                ClearButtons();

                AddButton(0, "Common.Button.AboutHer".Localized(), AskHelAboutHer);

                AddButton(14, "Common.Button.Leave".Localized(), BugOutAfterHelMinoThreesome);
            }
            else
            {
                ClearButtons();

                if (CoC.Instance.Flags.Get(GameFlagIds.HelTalkedAboutBerserking) == 0)
                    AddButton(1, "Common.Button.Berserking".Localized(), BerserkMode);

                if (CoC.Instance.Flags.Get(GameFlagIds.HelTalkedAboutAttackingYou) == 0)
                    AddButton(2, "Common.Button.YAttackMe".Localized(), AskHelAboutAttackingYou);

                AddButton(3, "Common.Button.Seconds".Localized(), AskMommaHelForSecondsAfterDinner);

                AddButton(4, "Common.Button.Minosrbad".Localized(), TelHelToGetOffTheMInoCock);

                AddButton(14, "Common.Button.Leave".Localized(), LeaveHelAfterMinoThreeSomeChat);
            }
        }

        public void TelHelSTOPATTACKINGMEYOUBITCH()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.TelHelSTOPATTACKINGMEYOUBITCH.EvenKnowingAskStop".Localized());

            OutputText("Scenes.Helscene.TelHelSTOPATTACKINGMEYOUBITCH.SighsPushesFaceNeck".Localized());

            OutputText("Scenes.Helscene.TelHelSTOPATTACKINGMEYOUBITCH.CheekPatAllRight".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckbuddy, 1);

            HelChatMenu();
        }

        public void AskMommaHelForSecondsAfterDinner()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.AskMommaHelForSecondsAfterDinner.ReachGrabAssSeconds".Localized());

            OutputText("Scenes.Helscene.AskMommaHelForSecondsAfterDinner.AlwaysLoverMine".Localized());

            if (Player.Lust < 40)
            {
                Player.Lust = 40;

                Player.ApplyStats(new DynStat(GameStats.Lust, 1f));
            }

            HelFuckMenu();
        }

        public void LeaveHelAfterMinoThreeSomeChat()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.LeaveHelAfterMinoThreeSomeChat.SadlyThingsToTend".Localized());

            OutputText("Scenes.Helscene.LeaveHelAfterMinoThreeSomeChat.SeeYouSoonLover".Localized());

            OutputText("Scenes.Helscene.LeaveHelAfterMinoThreeSomeChat.CountOnItBack".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SalamanderXIsabellaPlainsIntro()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-isabella-fight");

            OutputText("Scenes.Helscene.SalamanderXIsabellaPlainsIntro.WayOntoVastExpanse".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaPlainsIntro.BurstIntoCampArmed".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaPlainsIntro.GoingDownUdderBitch".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaPlainsIntro.SwiftDodgeNimbleHooves".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaPlainsIntro.DontWantYourMilk".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaPlainsIntro.HuffsHaughtilyMammariesQuake".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaPlainsIntro.LeapsSwordDown".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Diplomacy".Localized(), SalamanderXIsabellaDiplomacy);

            AddButton(1, "Common.Button.Watch".Localized(), WatchIsabellaAndHelFight);

            AddButton(2, "Common.Button.Leave".Localized(), SkipTownOnIsabellaAndHelsFight);
        }

        public void SalamanderXIsabellaDiplomacy()
        {
            ClearOutput();

            SpriteChooser();

            if (Player.Strength > 60)
            {
                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy.WarriorWomenRecoverCharge".Localized());

                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy.BothBlurtAtOnce".Localized());
            }
            else
            {
                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy.WomenChargeAgain".Localized());

                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy.ReliefHearBothNames".Localized());
            }

            Player.HealthChange(-10);

            DoNext(() => SalamanderXIsabellaDiplomacy2());
        }

        public void SalamanderXIsabellaDiplomacy2()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.TwoRedheadsAttentionInsist".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.GlaringStoleMyBandana".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.IndignantHuffNoseUp".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.HowKnowYourName".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.BrowFurrowsJa".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.SpendMinutesExplainingMet".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.FallSilentConsideringGazes".Localized());
            else
                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.GlowerBalefullyChallenge".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.ParagraphBreak".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.IsabellaFinallySaysBandana".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.HelAnswersLittleScoff".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.DopeySmileOffersHand".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.IsabellaSaysBreakingThe".Localized());

            if (silly)
                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.MilkWord".Localized());
            else
                OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.HandWord".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.ShakePullBlueBow".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.YeahYoureAllRight".Localized());

            OutputText("Scenes.Helscene.SalamanderXIsabellaDiplomacy2.YouAreVelcomeGoodbye".Localized());

            SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(5);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            CoC.Instance.Flags.Set(GameFlagIds.HelIsabellaThreesomeEnabled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void WatchIsabellaAndHelFight()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-isabella-fight");

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.DecidingNotYourBusiness".Localized());

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.OnlyArmedTakesInitiative".Localized());

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.CowGirlPainedMoo".Localized());

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.YouBeetchGrindingHooves".Localized());

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.BringItCowClash".Localized());

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.SpearSlamsGround".Localized());

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.FinishAnotherTime".Localized());

            OutputText("Scenes.Helscene.WatchIsabellaAndHelFight.CountBreakApart".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SkipTownOnIsabellaAndHelsFight()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.SkipTownOnIsabellaAndHelsFight.HellNotInvolved".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelIsabellaThreesomeEnabled, -1);

            DoNext(() => ReturnToCamp(1));
        }

        public void FollowrIzzyxSallyThreesomePretext()
        {

            SpriteChooser();

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.SomethingOddDuringNight".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.AsleepFewHoursThwack".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.OutOfCampLouder".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.PickUpPacePerimeter".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.IsabellaGaspsRelieved".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.TakeZeFightUncouth".Localized());
            else
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.TakeFightUncouthBarbarian".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.RaiseWeaponShadowyVillain".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.HelDemandsAnswer".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.TakenOffGuardIntroduce".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.KnowZeLittleBeech".Localized());
            else
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.KnowLittleBitch".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.HelSneersBetterGive".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.RaisesShieldMenacingKick".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.BothShoutFrustratedDistance".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.UndividedAttentionExplanation".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.StoleMyBandana".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.HuffNoseUpAgain".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.FoundInZeHands".Localized());
            else
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.FoundInHandsGnolls".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.YesYouFuckingDid".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.RollEyesCompanionExplain".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.IsabellaSaysShiftGaze".Localized());
            else
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.IsabellaShiftGazeAgain".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.HelAnswersScoffAgain".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.DopeySmileOffersHand".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.IsabellaSaysBreakingAgain".Localized());

            if (silly)
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.MilkWordAgain".Localized());
            else
                OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.HandWordAgain".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.PullBlueBowAgain".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.YoureAllRightAgain".Localized());

            OutputText("Scenes.Helscene.FollowrIzzyxSallyThreesomePretext.YouAreWelcomeDisappears".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelIsabellaThreesomeEnabled, 1);

            DoNext(PlayerMenu);
        }

        public void IsabellaXHelThreeSomePlainsStart()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-isabella-milkoffer");

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomePlainsStart.WanderingSilenceBroken".Localized());

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomePlainsStart.BurstOutGrass".Localized());

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomePlainsStart.MooGoodToSee".Localized());

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomePlainsStart.CertainlyThirstyEnticing".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), NomOnIzzyTitWithSallyMancer);

            AddButton(1, "Common.Button.Leave".Localized(), LeaveIsabellaSallyBehind);
        }

        public void IsabellaXHelThreeSomeCampStart()
        {

            SpriteChooser();

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomeCampStart.OddDuringNightAgain".Localized());

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomeCampStart.SettleInSleepWandered".Localized());

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomeCampStart.PushScrubAsideRevealing".Localized());

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomeCampStart.MooGoodSeeAgain".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.IsabellaXHelThreeSomeCampStart.PerhapsVouldLikeDrink".Localized());
            else
                OutputText("Scenes.Helscene.IsabellaXHelThreeSomeCampStart.PerhapsWouldLikeDrink".Localized());

            OutputText("Scenes.Helscene.IsabellaXHelThreeSomeCampStart.CertainlyThirstyEnticingAgain".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), NomOnIzzyTitWithSallyMancer);

            AddButton(1, "Common.Button.Leave".Localized(), () => Go<CampScene>());
        }

        public void LeaveIsabellaSallyBehind()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.LeaveIsabellaSallyBehind.DeclineOfferHaveFun".Localized());

            if (View.Time.Hours < 6)
                DoNext(PlayerMenu);
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void NomOnIzzyTitWithSallyMancer()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-isabella-drinkmilk");

            OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.OfferCantRefuse".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.ArmorTentingVisibly".Localized());
            else
                OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.NipplesStiffenExcitement".Localized());

            OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.PeriodBreak".Localized());

            OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.AreolaeMassiveThreeInches".Localized());

            OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.WithholdCommentSmashing".Localized());

            Player.RefillHunger(40);

            OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.SuddenlyRocketingGround".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.VantYouNeedYou".Localized());
            else
                OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.WantYouNeedYou".Localized());

            OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.GetHelOff".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 40f));

            Player.ChangeFatigue(-40);

            ClearButtons();

            AddButton(0, "Common.Button.Dick".Localized(), null);

            AddButton(1, "Common.Button.Vagina".Localized(), null);

            if (Player.HasCock() && Player.HasVagina())
            {
                OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.DecideWhichOrgans".Localized());

                if (Player.HasCockThatFits(85))
                    AddButton(0, "Common.Button.Dick".Localized(), StuffIzzyAndSalamanderWithDicks);
                else
                    OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.TooBigManBits".Localized());

                AddButton(1, "Common.Button.Vagina".Localized(), IzzySallyThreeSomeVagoozlaz);
            }
            else
                if (Player.HasVagina())
                    AddButton(1, "Common.Button.Vagina".Localized(), IzzySallyThreeSomeVagoozlaz);
                else
                    if (Player.HasCock())
                    {
                        if (Player.HasCockThatFits(85))
                            AddButton(0, "Common.Button.Dick".Localized(), StuffIzzyAndSalamanderWithDicks);
                        else
                            OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.TooBigForThem".Localized());
                    }
                    else
                        OutputText("Scenes.Helscene.NomOnIzzyTitWithSallyMancer.UnfortunatelyNotMuchContribute".Localized());

            AddButton(2, "Common.Button.Leave".Localized(), NoThreesomeSexWithSallyAndIssyLastMinute);
        }

        public void NoThreesomeSexWithSallyAndIssyLastMinute()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.NoThreesomeSexWithSallyAndIssyLastMinute.HeavyHeartPass".Localized());

            OutputText("Scenes.Helscene.NoThreesomeSexWithSallyAndIssyLastMinute.HeadBackFullBelly".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Helscene.NoThreesomeSexWithSallyAndIssyLastMinute.BallsBluerThanLake".Localized());

            OutputText("Scenes.Helscene.NoThreesomeSexWithSallyAndIssyLastMinute.SentencePeriod".Localized());

            if (View.Time.Hours < 6)
                DoNext(PlayerMenu);
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void StuffIzzyAndSalamanderWithDicks()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-isabella-threesome-dick");

            int x = -1;

            int y = -1;

            int z = -1;

            int zz = -1;

            var temp = Player.Cocks.Count;

            while (temp > 0)
            {
                temp--;

                if (Player.CockArea(temp) <= 85)
                    if (x == -1)
                        x = temp;
                    else
                        if (y == -1)
                            y = temp;
                        else
                            if (z == -1)
                                z = temp;
                            else
                                if (zz == -1)
                                {
                                    zz = temp;

                                    break;
                                }
            }

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.StrippingPopsFree".Localized(
                Player.SMultiCockDesc()
            ));

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.TwoGirlsTitsPressed".Localized());

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.DropToKnees".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.MeanwhileSlidesIntoPussy".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.GrabHipsPushIn".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.SimilarlyComingToRest".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.PeriodBreak".Localized());

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.NowBottomedOutDeep".Localized());

            if (y == -1)
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.HerWord".Localized());
            else
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.ThemWord".Localized());

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.RockHipsGirlsPlay".Localized());

            if (y == -1)
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.HerWordAgain".Localized());
            else
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.ThemWordAgain".Localized());

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.QuickShortStrokesKeep".Localized());

            if (y == -1)
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.HerWordThird".Localized());
            else
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.ThemWordThird".Localized());

            OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.FilledWithCockmeat".Localized());

            if (y == -1)
            {
                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.FeelingBadNeglectedIsabella".Localized());

                if (Player.Cocks[x].Length > 9)
                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.DoNotPlayVith".Localized());
                    else
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.NormallyDontPlayClumsy".Localized());

                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.StillGraspingSlideInto".Localized(
                    Player.CockDescript(x),

                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.TwoMinutesSlam".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.FewMinutesEternityBliss".Localized());

                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.CombinationCowGirlLips".Localized());

                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.NotHappyJustCumming".Localized());

                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.StillRecoveringShakilyComply".Localized());

                OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.SuddenlyGrinningTongues".Localized());
            }
            else
                if (z == -1 || zz == -1)
                {
                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.FuckingBothAtOnce".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(y)
                    ));

                    if (z > -1)
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.EvenNowSpearingAss".Localized(
                            Player.CockDescript(z)
                        ));

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.PeriodBreakAgain".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.GrinningGrabsUdders".Localized());

                    if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.OhJaGrinningToo".Localized());
                    else
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.OhYesGrinningToo".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.BeforeSayWordPinches".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.CocksStillRiddenHard".Localized());

                    if (z == -1)
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.BothWord".Localized());
                    else
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.AllThreeWord".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.CocksCumFirst".Localized(
                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.DisparateSpeedsOverEdge".Localized());

                    if (z > -1)
                        OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.AndHelsAss".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.BuckHipsUnleash".Localized());
                }
                else
                {
                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.AlreadyFuckingNotEnough".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.HolyShitWhatYou".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.RamAllFourCocks".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(y),

                        Player.CockDescript(z),

                        Player.CockDescript(zz)
                    ));

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.ScreamMoanHipsPressed".Localized());

                    OutputText("Scenes.Helscene.StuffIzzyAndSalamanderWithDicks.FourCocksOverEdge".Localized());
                }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            DoNext(() => IzzySallyThreeSomeFollowup());
        }

        public void IzzySallyThreeSomeFollowup()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.IzzySallyThreeSomeFollowup.SweatCumSoakedCollapse".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeFollowup.ThatWasAmazingSeriously".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.IzzySallyThreeSomeFollowup.MmmJaLongYawn".Localized());
            else
                OutputText("Scenes.Helscene.IzzySallyThreeSomeFollowup.MmmYesLongYawn".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeFollowup.RestingIsGoodYawn".Localized());

            if (View.Time.Hours < 6)
                DoNext(PlayerMenu);
            else
                DoNext(() => ReturnToCamp(4));
        }

        public void IzzySallyThreeSomeVagoozlaz()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-isabella-threesome-vagoo");

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.QuicklyStripAlreadyWet".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.ShudderWideTongueBox".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.BuryFaceCrotchTongue".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.ContinueLickCrawlingSensation".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.MeanwhileBuriedInGroin".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.CombinationFingerAndTongue".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.AndHelJerkingOff".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.LimitTongueFuck".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.WorksLikeCharmClamps".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.AndSalamanderDoubleTeam".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.SendOverEdgeScream".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.CumSoakedCollapseAgain".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.WrappingWarmTailLovingly".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.PeriodBreak".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.AmazingSeriouslyAgain".Localized());

            if ((CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentTrainingPercent) < 100 || CoC.Instance.Flags.Get(GameFlagIds.IsabellaAccentForcedOn) != 0))
                OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.MmmJaLongYawn".Localized());
            else
                OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.MmmYesLongYawn".Localized());

            OutputText("Scenes.Helscene.IzzySallyThreeSomeVagoozlaz.RestingGoodYawnAgain".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -3f));

            Save.SetInt("lastTimeSex", View.Time.Days);

            SceneInstance<IsabellaFollowerScene>.Get().IsabellaAffection(4);

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            if (View.Time.Hours < 6)
                DoNext(PlayerMenu);
            else
                DoNext(() => ReturnToCamp(4));
        }

        public void HeliaPlusFoxyFluffs()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-fox-foursome-intro");

            if (CoC.Instance.Flags.Get(GameFlagIds.HelFoxyFoursomeWarned) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.HelFoxyFoursomeWarned, 1);

                OutputText("Scenes.Helscene.HeliaPlusFoxyFluffs.GiveWinkIntroduceFoxy".Localized());

                OutputText("Scenes.Helscene.HeliaPlusFoxyFluffs.GrinsExtraHardware".Localized());

                OutputText("Scenes.Helscene.HeliaPlusFoxyFluffs.MmmWannaPlaySister".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Foursome".Localized(), HeliasFoxyFourSomeFluffs);

                AddButton(14, "Common.Button.Leave".Localized(), RunAwayFromFoxGirls);
            }
            else
            {
                OutputText("Scenes.Helscene.HeliaPlusFoxyFluffs.WhistleWaveBack".Localized());

                OutputText("Scenes.Helscene.HeliaPlusFoxyFluffs.AmusedGropeMyMy".Localized());

                DoNext(() => HeliasFoxyFourSomeFluffs());
            }
        }

        public void RunAwayFromFoxGirls()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.RunAwayFromFoxGirls.ExcuseForgotSomething".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void HeliasFoxyFourSomeFluffs()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.HeliasFoxyFourSomeFluffs.SlipHandLooseTunic".Localized());

            OutputText("Scenes.Helscene.HeliasFoxyFourSomeFluffs.FourSlideIntoRoom".Localized());

            OutputText("Scenes.Helscene.HeliasFoxyFourSomeFluffs.WhatPartsToUse".Localized());

            ClearButtons();

            if (Player.HasCock())
                AddButton(0, "Common.Button.AsMale".Localized(), FoxyFluffsFoursomeAsMale);
            else
                AddButton(0, "Common.Button.AsMale".Localized(), null);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.AsFemale".Localized(), FoxyFluffGirlsFuckSex);
            else
                AddButton(1, "Common.Button.AsFemale".Localized(), null);
        }

        public void FoxyFluffsFoursomeAsMale()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-fox-foursome-male");

            int x = Player.CockThatFits(85);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.StripGrabGigglingFlops".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.SoppingPushBack".Localized());

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.BehindMaiButtCheeks".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.SeesOpportunityCrawlsImpales".Localized());

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.LeanOverKissTongues".Localized());

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.BreakKissIncreasePace".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.TwinsCumAsOne".Localized());

            OutputText("Scenes.Helscene.FoxyFluffsFoursomeAsMale.ShudderGaspOvertakesBury".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            FoxyFluffOutro();
        }

        public void FoxyFluffGirlsFuckSex()
        {
            ClearOutput();

            SpriteChooser();

            ShowImage("helia-fox-foursome-female");

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.StripPushFoxGirls".Localized());

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.FellateTwinsHands".Localized());

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.WetPullTogether".Localized());

                OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.ThrustDoggyCocks".Localized());

                OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.GirlsFuckRawTits".Localized());
            }

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.TwinsPickTossBed".Localized());

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.GraspingHipsMaiSlides".Localized());

            Player.CuntChange(24, true, true, false);

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.ParagraphBreak".Localized());

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.ScreamPleasureCuntStretched".Localized());

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.LeftBehindThrow".Localized());

            Player.Orgasm("Vaginal");

            OutputText("Scenes.Helscene.FoxyFluffGirlsFuckSex.SuddenlyMikoSharpCry".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            FoxyFluffOutro();
        }

        public void FoxyFluffOutro()
        {
            ClearOutput();

            SpriteChooser();

            OutputText("Scenes.Helscene.FoxyFluffOutro.CumSoakedCollapseBed".Localized());

            OutputText("Scenes.Helscene.FoxyFluffOutro.SmileOpenArmsSnuggle".Localized());

            OutputText("Scenes.Helscene.FoxyFluffOutro.ThreeHoursLater".Localized());

            OutputText("Scenes.Helscene.FoxyFluffOutro.AwakeTuckedIntoBed".Localized());

            SceneInstance<HelFollowerScene>.Get().HelAffection(5);

            Save.SetInt("lastTimeSex", View.Time.Days);

            DoNext(() => ReturnToCamp(4));
        }

        public void TelHelToGetOffTheMInoCock()
        {
            ClearOutput();

            OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.LiePostAfterglowThought".Localized());

            if (Player.MinotaurAddicted())
                OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.YouKnowFirsthand".Localized());
            else
                OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.YouveHeard".Localized());

            OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.TheirCumAddictiveDont".Localized());

            OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.HmmCockingEyebrowAgain".Localized());

            OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.HowItIsAgain".Localized());

            if (marbleFollower())
                OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.LikeCowGirlsMilk".Localized());

            OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.SentencePeriod".Localized());

            OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.GetHookedAgain".Localized());

            OutputText("Scenes.Helscene.TelHelToGetOffTheMInoCock.SupposeOfferSatisfyAgain".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ItsOkay".Localized(), HelCanFuckMinosWhenever);

            AddButton(1, "Common.Button.SatisfyHer".Localized(), SatisfyHelSoSheStopsMinoFucking);
        }

        public void HelCanFuckMinosWhenever()
        {
            ClearOutput();

            OutputText("Scenes.Helscene.HelCanFuckMinosWhenever.DecideNotToDeter".Localized());

            DoNext(() => HelChatMenu());
        }

        public void SatisfyHelSoSheStopsMinoFucking()
        {
            ClearOutput();

            OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.AfterContemplationLustyGrin".Localized());

            OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.HellOfAnOffer".Localized());

            if (Player.Libido >= 50)
            {
                OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.GrabShouldersThrowGround".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.StiffeningReadyAgain".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.AndYourCock".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.AlreadySoakedSecretions".Localized());

                if (Player.Gender == Gender.Genderless)
                    OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.AssReadyToFuck".Localized());

                OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.SentencePeriod".Localized());

                OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.GoodStartPullDown".Localized());
            }
            else
            {
                OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.GrimaceBetterKeepUp".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.AnotherDicksCum".Localized());

                OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.NodAgreeingPleasure".Localized());

                OutputText("Scenes.Helscene.SatisfyHelSoSheStopsMinoFucking.SmilesClawedHandsClutching".Localized());
            }

            if (Player.Lust < 33)
            {
                Player.ApplyStats(new DynStat(GameStats.Lust, 0.1f));

                Player.Lust = 33;
            }

            CoC.Instance.Flags.Set(GameFlagIds.PcPromisedHelMonogamyFucks, 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckbuddy, 1);

            HelFuckMenu();
        }

        public void HelSexualAmbush()
        {
            ClearOutput();

            OutputText("Scenes.Helscene.HelSexualAmbush.FootfallsRapidlyApproaching".Localized());

            OutputText("Scenes.Helscene.HelSexualAmbush.GrowlsThrowingTop".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 20)));

            if (Player.Lust < 33)
                Player.Lust = 33;

            CoC.Instance.Flags.Set(GameFlagIds.HelRapedToday, 1);

            HelFuckMenu(true);
        }

        public void PussyOutOfHelSexAmbush()
        {
            ClearOutput();

            OutputText("Scenes.Helscene.PussyOutOfHelSexAmbush.PushOffNotInterested".Localized());

            OutputText("Scenes.Helscene.PussyOutOfHelSexAmbush.WhatDeadpanWhat".Localized());

            OutputText("Scenes.Helscene.PussyOutOfHelSexAmbush.NoSexForYou".Localized());

            OutputText("Scenes.Helscene.PussyOutOfHelSexAmbush.ButWhatYouSaid".Localized());

            OutputText("Scenes.Helscene.PussyOutOfHelSexAmbush.ShrugHeadBackMasturbating".Localized());

            DoNext(() => ReturnToCamp(1));

            SceneInstance<HelFollowerScene>.Get().HelAffection(-20);
        }

    }
}