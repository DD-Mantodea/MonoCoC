using MonoCoC.Attributes;
using MonoCoC.Utils;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Contents.Npcs;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<SophieScene>("coc:sophie")]
    public class SophieScene : Scene
    {
        public NpcDefinition Definition => NpcRegistry.Get(NpcIds.Sophie)!;

        public NpcState State => Player.GetNpcState(Definition);

        private PregnancyStore? _pregnancy;

        public PregnancyStore Pregnancy => _pregnancy ??= NpcPregnancy.StoreFor(Definition, State);

        public bool sophieBimbo => CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0;

        public bool sophieFollowerScene => false;

        public override void Entry()
        {

        }

        public bool SophieAtCamp()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) > 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) != 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieRecruitedPure) > 0)
                return true;

            return false;
        }

        public void SophiesDaughterDescript()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) == 325)
                OutputText("Scenes.Sophiescene.SophiesDaughterDescript.CuteLittleHarpyStill".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) == 200)
                OutputText("Scenes.Sophiescene.SophiesDaughterDescript.SweetLittleHarpyStarting".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) == 100)
            {
                OutputText("Scenes.Sophiescene.SophiesDaughterDescript.LookingAroundForDaughter".Localized());

                if (sophieBimbo)
                    OutputText("Scenes.Sophiescene.SophiesDaughterDescript.BoisterousBimbo".Localized());
                else
                    OutputText("Scenes.Sophiescene.SophiesDaughterDescript.MatureHarpy".Localized());

                OutputText("Scenes.Sophiescene.SophiesDaughterDescript.QualityMotherDaughter".Localized());

                if (sophieBimbo)
                    OutputText("Scenes.Sophiescene.SophiesDaughterDescript.BimboWord".Localized());
                else
                    OutputText("Scenes.Sophiescene.SophiesDaughterDescript.AvianGirlfriend".Localized());

                OutputText("Scenes.Sophiescene.SophiesDaughterDescript.MovesHerHair".Localized());

                OutputText("Scenes.Sophiescene.SophiesDaughterDescript.DaughterGrowingUp".Localized());
            }
        }

        public void SophiesPregnancyDescript(int triggeredEvent)
        {

            if (triggeredEvent == 1)
            {
                if (sophieBimbo)
                {
                    OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.SophieSitsByHerself".Localized());

                    OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.NoticingYourGaze".Localized());
                }
                else
                {
                    OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.SophieSitsHerselfAgain".Localized());

                    OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.NoticingYourGazeAgain".Localized());
                }

                Player.ApplyStats(new DynStat(GameStats.Lust, 3f));
            }
            else
                if (triggeredEvent == 2)
                {
                    if (sophieBimbo)
                    {
                        OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.AsUsualLayingOn".Localized());

                        OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.LookYourEgg".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.AsUsualLayingAgain".Localized());

                        OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.LookThatEgg".Localized());
                    }

                    Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                }
                else
                    if (triggeredEvent == 3)
                    {
                        if (sophieBimbo)
                        {
                            OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.PregnantBimboLounges".Localized());

                            OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.AmorousHarpyLeapsOut".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.PregnantHarpyLounges".Localized());

                            OutputText("Scenes.Sophiescene.SophiesPregnancyDescript.AmorousLeapsOutAgain".Localized());
                        }

                        Player.ApplyStats(new DynStat(GameStats.Lust, 5f));
                    }
        }

        public void SophieRouter()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MetSophieCounter) == 0)
                MeetSophie();
            else
                MeetSophieRepeat();
        }

        public void MeetSophie()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            CoC.Instance.Flags.Set(GameFlagIds.MetSophieCounter, CoC.Instance.Flags.Get(GameFlagIds.MetSophieCounter) + 1);

            OutputText("Scenes.Sophiescene.MeetSophie.GruelingTrekThrough".Localized());

            OutputText("Scenes.Sophiescene.MeetSophie.HaulingYourselfUp".Localized());

            if (Player.Tallness <= 48)
                OutputText("Scenes.Sophiescene.MeetSophie.DownWord".Localized());
            else
                if (Player.Tallness >= 72)
                    OutputText("Scenes.Sophiescene.MeetSophie.UpWord".Localized());

            OutputText("Scenes.Sophiescene.MeetSophie.WithHerCuriousAmber".Localized());

            OutputText("Scenes.Sophiescene.MeetSophie.SmilesIngratiatingly".Localized());

            OutputText("Scenes.Sophiescene.MeetSophie.SuggestsShePullsA".Localized(
                Player.Mf("boys", "girls")
            ));

            if (Player.Corruption < 33)
                OutputText("Scenes.Sophiescene.MeetSophie.InInterestOfPoliteness".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Sophiescene.MeetSophie.BarelyRememberingManners".Localized());
                else
                    OutputText("Scenes.Sophiescene.MeetSophie.SpreadYourName".Localized());

            OutputText("Scenes.Sophiescene.MeetSophie.WrenchGazeAway".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryHarpies);

            ClearButtons();

            AddButton(0, "Common.Button.Foraging".Localized(), TellSophieYoureForagingForStuff);

            AddButton(1, "Common.Button.GotLost".Localized(), SophieMeetingGotLost);

            AddButton(2, "Common.Button.Look4demons".Localized(), SophieLookingForDemons);

            AddButton(3, "Common.Button.Sex".Localized(), SophieMeetingChoseSex);
        }

        public void MeetSophieRepeat()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAngryAtPcCounter) > 0)
            {
                OutputText("Scenes.Sophiescene.MeetSophieRepeat.DuringYourExploration".Localized());

                var sophieEnemy = EnemyRegistry.Create(new Identifier("coc", "sophie"));

                sophieEnemy.OnDefeated = _ => SophieWonCombat();

                sophieEnemy.OnWon = _ => SophieLostCombat();

                CombatScene.Start(sophieEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));

                return;
            }

            if (Player.Cocks.Count > 0)
            {
                if (Random.Shared.Next(2) == 0 && !Player.IsPregnant())
                {
                    OutputText("Scenes.Sophiescene.MeetSophieRepeat.DuringYourExplorationAgain".Localized());

                    if (Player.Tallness > 72)
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.StrugglesWithYourWeight".Localized());
                    else
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.FlightThankfullyBrief".Localized());

                    OutputText("Scenes.Sophiescene.MeetSophieRepeat.ReleasesTalonsAndYou".Localized(
                        Player.Mf("Boy", "Girl")
                    ));

                    if (Player.LustPercent < 60 || Random.Shared.Next(3) <= 1)
                    {
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.NeedAmplifiesCompulsion".Localized());

                        ClearButtons();

                        AddButton(0, "Common.Button.Yes".Localized(), ConsensualSexSelector);

                        AddButton(1, "Common.Button.No".Localized(), FightSophie);
                    }
                    else
                    {
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.NeedAmplifiesAnd".Localized());

                        DoNext(() => ConsensualSexSelector());
                    }

                    return;
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) > 0)
                {
                    OutputText("Scenes.Sophiescene.MeetSophieRepeat.DuringYourExplorationThird".Localized());

                    if (Player.Tallness > 72)
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.StrugglesWithWeightAgain".Localized());
                    else
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.FlightBriefAgain".Localized());

                    OutputText("Scenes.Sophiescene.MeetSophieRepeat.ReleasesYouAndFlops".Localized());

                    if (Player.IsPregnant())
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.StillHaventLaidYour".Localized());
                    else
                        OutputText("Scenes.Sophiescene.MeetSophieRepeat.AlreadyLaidLastEgg".Localized());

                    OutputText("Scenes.Sophiescene.MeetSophieRepeat.ThighsSpreadInviting".Localized());
                }
                else
                    OutputText("Scenes.Sophiescene.MeetSophieRepeat.DuringYourExplorationFourth".Localized(
                        Player.Mf("boy", "breeder")
                    ));

                OutputText("Scenes.Sophiescene.MeetSophieRepeat.WordsSinkIntoYou".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), ConsensualSexSelector);

                AddButton(1, "Common.Button.No".Localized(), ShootDownSophieSex);

                AddButton(2, "Common.Button.Dominate".Localized(), FightSophie);

                return;
            }
            else if (Player.BiggestLactation() < 1)
            {
                OutputText("Scenes.Sophiescene.MeetSophieRepeat.ClimbManagesTakeYou".Localized(
                    Player.Mf("neuter", "girl")
                ));

                OutputText("Scenes.Sophiescene.MeetSophieRepeat.SeeTheWisdomOf".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.FightSophie".Localized(), FirstTimeSophieForceSex);

                AddButton(1, "Common.Button.KeepClimbing".Localized(), PCIgnoresSophieAndHarpyIsFought);

                SetExitButton("Common.Button.Back".Localized());

                return;
            }
            else
            {
                OutputText("Scenes.Sophiescene.MeetSophieRepeat.ClimbManagesAgain".Localized());

                OutputText("Scenes.Sophiescene.MeetSophieRepeat.AgreeToBreastfeed".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), CramANippleInIt);

                AddButton(1, "Common.Button.No".Localized(), ShootDownSophieSex);

                AddButton(2, "Common.Button.FightHer".Localized(), FirstTimeSophieForceSex);

                return;
            }

            DoNext(() => ReturnToCamp(1));

            OutputText("Scenes.Sophiescene.MeetSophieRepeat.SomethingScrewyHappened".Localized());

            return;
        }

        public void ConsensualSexSelector()
        {

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            if (Player.CockThatFits(232) < 0)
                ConsensualSophieSexNoFit();
            else
                ConsensualHotSophieDickings();
        }

        public void FightSophie()
        {

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            var sophieEnemy2 = EnemyRegistry.Create(new Identifier("coc", "sophie"));

            sophieEnemy2.OnDefeated = _ => SophieWonCombat();

            sophieEnemy2.OnWon = _ => SophieLostCombat();

            CombatScene.Start(sophieEnemy2, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false);

            CoC.Instance.Flags.Set(GameFlagIds.SophieAngryAtPcCounter, CoC.Instance.Flags.Get(GameFlagIds.SophieAngryAtPcCounter) + Random.Shared.Next(24));
        }

        public void RepeatBreastFeeding()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.RepeatBreastFeeding.AgreeAndClimbRest".Localized());

            DoNext(() => CramANippleInIt());
        }

        public void PCIgnoresSophieAndHarpyIsFought()
        {

            OutputText("Scenes.Sophiescene.PCIgnoresSophieAndHarpyIsFought.HarpyWingsOut".Localized());

            var harpyEnemy = EnemyRegistry.Create(new Identifier("coc", "harpy"));

            harpyEnemy.OnDefeated = _ => PCIgnoresSophieAndHarpyIsFought();

            harpyEnemy.OnWon = _ => CombatScene.CleanupAfterCombat();

            CombatScene.Start(harpyEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void SophieLookingForDemons()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.SophieLookingForDemons.ThrowsHeadBack".Localized());

            if ((Player.IsCorruptEnough(60) || Player.LustPercent > 60 || Player.Libido > 70) && Player.HasCock())
                OutputText("Scenes.Sophiescene.SophieLookingForDemons.InSpiteOfDanger".Localized());
            else
            {
                OutputText("Scenes.Sophiescene.SophieLookingForDemons.GulpAndNod".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            OutputText("Scenes.Sophiescene.SophieLookingForDemons.HaveYouGottenBored".Localized());

            if (Player.Tallness <= 48)
                OutputText("Scenes.Sophiescene.SophieLookingForDemons.LittleWord".Localized());
            else
                if (Player.Tallness >= 72)
                    OutputText("Scenes.Sophiescene.SophieLookingForDemons.BigWord".Localized());

            OutputText("Scenes.Sophiescene.SophieLookingForDemons.SeeSomethingYou".Localized());

            OutputText("Scenes.Sophiescene.SophieLookingForDemons.WordsSinkIntoYou".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), ConsensualSexSelector);

            AddButton(1, "Common.Button.No".Localized(), ShootDownSophieSex);
        }

        public void ShootDownSophieSex()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.ShootDownSophieSex.PoutsForAMoment".Localized());

            DoNext(() => ReturnToCamp(1));

            if (Player.LibidoPercent > 25)
                Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            if (Player.LustPercent > 50)
                Player.ApplyStats(new DynStat(GameStats.Lust, -5f));
        }

        public void SophieMeetingChoseSex()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            if (Player.Cocks.Count == 0)
            {
                OutputText("Scenes.Sophiescene.SophieMeetingChoseSex.LooksUpAndDown".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Sophiescene.SophieMeetingChoseSex.AndInsists".Localized());
                else
                    OutputText("Scenes.Sophiescene.SophieMeetingChoseSex.SentencePeriod".Localized());

                OutputText("Scenes.Sophiescene.SophieMeetingChoseSex.TurnsAndFluffs".Localized());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Sophiescene.SophieMeetingChoseSex.WhatDoYouDo".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.ForceSex".Localized(), FirstTimeSophieForceSex);

                    SetExitButton("Common.Button.Back".Localized());

                    return;
                }

                DoNext(() => ReturnToCamp(1));

                return;
            }
            else
            {
                OutputText("Scenes.Sophiescene.SophieMeetingChoseSex.RetreatsToFarSide".Localized());

                OutputText("Scenes.Sophiescene.SophieMeetingChoseSex.AsCouldDeny".Localized());

                DoNext(() => ConsensualSexSelector());

                return;
            }
        }

        public void FirstTimeSophieForceSex()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.FirstTimeSophieForceSex.YouSay".Localized());

            OutputText("Scenes.Sophiescene.FirstTimeSophieForceSex.LargeEyesWiden".Localized());

            OutputText("Scenes.Sophiescene.FirstTimeSophieForceSex.GoingAFight".Localized());

            var sophieEnemy = EnemyRegistry.Create(new Identifier("coc", "sophie"));

            sophieEnemy.OnDefeated = _ => SophieWonCombat();

            sophieEnemy.OnWon = _ => SophieLostCombat();

            CombatScene.Start(sophieEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void SophieMeetingGotLost()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.SophieMeetingGotLost.ExplainYouWereExploring".Localized());

            OutputText("Scenes.Sophiescene.SophieMeetingGotLost.OlderHarpyReclines".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Sophiescene.SophieMeetingGotLost.StayHelpLonelyMatron".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), ConsensualSexSelector);

                AddButton(1, "Common.Button.No".Localized(), ShootDownSophieSex);
            }
            else
                if (Player.BiggestLactation() >= 1.5f)
                {
                    OutputText("Scenes.Sophiescene.SophieMeetingGotLost.LadenLittleCow".Localized());

                    AddButton(0, "Common.Button.Yes".Localized(), CramANippleInIt);

                    AddButton(1, "Common.Button.No".Localized(), ShootDownSophieSex);
                }
                else
                {
                    OutputText("Scenes.Sophiescene.SophieMeetingGotLost.SexuallyDeprivedPlies".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 4)));

                    if (Player.Intelligence < 50)
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

                    DoNext(() => ReturnToCamp(1));
                }
        }

        public void TellSophieYoureForagingForStuff()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.TellSophieYoureForagingForStuff.ExplainThatYou".Localized());

            OutputText("Scenes.Sophiescene.TellSophieYoureForagingForStuff.TakesALittleWhile".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 4)));

            if (Player.Intelligence < 50)
                Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void CramANippleInIt()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            Player.BoostLactation(.01f);

            if (!CombatController.InCombat)
                OutputText("Scenes.Sophiescene.CramANippleInIt.StepsBackAndDrops".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));
            else
                if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                    OutputText("Scenes.Sophiescene.CramANippleInIt.PantsPullsHerselfUp".Localized());
                else
                    OutputText("Scenes.Sophiescene.CramANippleInIt.PullDazedAndDefeated".Localized(
                        Appearance.AllBreastsDescript(Player)
                    ));

            OutputText("Scenes.Sophiescene.CramANippleInIt.MakeYourselfComfortable".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Sophiescene.CramANippleInIt.TopHyphen".Localized());

            OutputText("Scenes.Sophiescene.CramANippleInIt.LeftmostMouthLatches".Localized());

            if (Player.BiggestLactation() < 2)
            {
                OutputText("Scenes.Sophiescene.CramANippleInIt.SucklesHarderAndHarder".Localized());

                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Sophiescene.CramANippleInIt.BatteredWord".Localized());
                else
                    OutputText("Scenes.Sophiescene.CramANippleInIt.LustyWord".Localized());

                OutputText("Scenes.Sophiescene.CramANippleInIt.VisageOfPureSatisfaction".Localized());

                if (Player.TotalNipples() > 2)
                    OutputText("Scenes.Sophiescene.CramANippleInIt.NextWord".Localized());
                else
                    OutputText("Scenes.Sophiescene.CramANippleInIt.OtherWord".Localized());

                OutputText("Scenes.Sophiescene.CramANippleInIt.ClosingEyesAndRelaxing".Localized());

                if (Player.TotalNipples() > 2)
                {
                    OutputText("Scenes.Sophiescene.CramANippleInIt.ThisNippleDoesntLast".Localized());

                    if (Player.TotalNipples() < 5)
                        OutputText("Scenes.Sophiescene.CramANippleInIt.BeforeFinishingLastOne".Localized());
                    else
                        OutputText("Scenes.Sophiescene.CramANippleInIt.AndTheNextContinuing".Localized());
                }

                OutputText("Scenes.Sophiescene.CramANippleInIt.ParagraphBreak".Localized());
            }
            else
                if (Player.BiggestLactation() < 3.5f)
                {
                    OutputText("Scenes.Sophiescene.CramANippleInIt.SucklesHardButSeems".Localized());

                    if (Player.TotalNipples() == 2)
                        OutputText("Scenes.Sophiescene.CramANippleInIt.OtherWordAgain".Localized());
                    else
                        OutputText("Scenes.Sophiescene.CramANippleInIt.NextWordAgain".Localized());

                    OutputText("Scenes.Sophiescene.CramANippleInIt.SucklingGoesOnNearly".Localized());

                    if (Player.BreastRows.Count() > 2)
                    {
                        if (Player.TotalNipples() > 2)
                            OutputText("Scenes.Sophiescene.CramANippleInIt.DrinksFromEachOf".Localized(
                                Appearance.AllBreastsDescript(Player)
                            ));
                        else
                            OutputText("Scenes.Sophiescene.CramANippleInIt.DrinksFromEachBreast".Localized());
                    }
                    else
                        if (Player.TotalNipples() > 2)
                            OutputText("Scenes.Sophiescene.CramANippleInIt.DrinksFromEachNipple".Localized());

                    OutputText("Scenes.Sophiescene.CramANippleInIt.ParagraphBreakAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Sophiescene.CramANippleInIt.SucklesHardAMoment".Localized());

                    if (Player.TotalNipples() == 2)
                        OutputText("Scenes.Sophiescene.CramANippleInIt.OtherWordThird".Localized());
                    else
                        OutputText("Scenes.Sophiescene.CramANippleInIt.NextWordThird".Localized());

                    OutputText("Scenes.Sophiescene.CramANippleInIt.OneBeginsCycleAnew".Localized());

                    if (Player.TotalNipples() >= 4)
                        OutputText("Scenes.Sophiescene.CramANippleInIt.WithAllYourNipples".Localized());

                    OutputText("Scenes.Sophiescene.CramANippleInIt.ParagraphBreakThird".Localized());
                }

            OutputText("Scenes.Sophiescene.CramANippleInIt.PullsBackWith".Localized());

            if (Player.BiggestLactation() >= 3.5f)
                OutputText("Scenes.Sophiescene.CramANippleInIt.IncrediblyLoudBurp".Localized());
            else
                if (Player.BiggestLactation() >= 2)
                    OutputText("Scenes.Sophiescene.CramANippleInIt.ASatisfiedBurp".Localized());
                else
                    OutputText("Scenes.Sophiescene.CramANippleInIt.ASatisfiedAhhh".Localized());

            OutputText("Scenes.Sophiescene.CramANippleInIt.WipesBitOfMilk".Localized());

            if (CombatController.InCombat)
            {
                if (CombatController.Instance!.Context.Enemy.Health < 1)
                    OutputText("Scenes.Sophiescene.CramANippleInIt.KnowYouDontHave".Localized());
                else
                {
                    OutputText("Scenes.Sophiescene.CramANippleInIt.SureKnowHowTo".Localized());

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Sophiescene.CramANippleInIt.MaybeYoullLetTry".Localized());

                    OutputText("Scenes.Sophiescene.CramANippleInIt.SayEnd".Localized());

                    if (Player.Cocks.Count > 0)
                    {
                        OutputText("Scenes.Sophiescene.CramANippleInIt.LaughsAndCaresses".Localized());

                        Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
                    }
                    else
                        OutputText("Scenes.Sophiescene.CramANippleInIt.ParagraphBreakFourth".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerProgress, 0);
            }
            else
            {
                OutputText("Scenes.Sophiescene.CramANippleInIt.DeliciousBeenSoLong".Localized());

                if (Player.Cocks.Count > 0)
                {
                    OutputText("Scenes.Sophiescene.CramANippleInIt.MaybeYoullTryAgain".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 25f));

                    CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerProgress, CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerProgress) + 1);
                }
                else
                    OutputText("Scenes.Sophiescene.CramANippleInIt.SayEndParagraphBreak".Localized());
            }

            Player.BoostLactation(.1f);

            OutputText("Scenes.Sophiescene.CramANippleInIt.BustyHarpyRubs".Localized());

            if (Player.Corruption > 60)
                OutputText("Scenes.Sophiescene.CramANippleInIt.GiveHerWideAss".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -50f));

            CoC.Instance.Flags.Set(GameFlagIds.BreastfeadSophieCounter, CoC.Instance.Flags.Get(GameFlagIds.BreastfeadSophieCounter) + 1);

            Player.Orgasm("Tits");

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));

            if (Player.HasStatusEffect(StatusEffectIds.Feeder))
            {
                Player.ChangeStatusValue(StatusEffectIds.Feeder, 0, 1);

                Player.ChangeStatusValue(StatusEffectIds.Feeder, 2, 0);
            }
        }

        public void ConsensualHotSophieDickings()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerProgress, CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerProgress) + 1);

            int x = Player.CockThatFits(232);

            if (x < 0)
            {
                CoCSettings.Error("");

                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ErrorNoCockFits".Localized());

                DoNext(PlayerMenu);

                CombatController.InCombat = false;

                return;
            }
            else
                if (x > Player.Cocks.Count - 1)
                {
                    CoCSettings.Error("");

                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ErrorTooManyCocks".Localized());

                    DoNext(PlayerMenu);

                    CombatController.InCombat = false;

                    return;
                }

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.SpreadThighsBeckoning".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.OnYourChest".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BetweenYour".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.TracingUpAndDown".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ReleasesFromBountiful".Localized(
                Player.AssDescript()
            ));

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BreastsRubbingAgainst".Localized());
            else
                if (Player.Tallness <= 48)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BreastsPressedIntoFace".Localized());
                else
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BreastsPressingStomach".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.CommaSpace".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.EachOfAgain".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BeginsPokeAndProd".Localized());

            if (Player.CockArea(x) <= 6)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.FrownsAndAsks".Localized());
            else
                if (Player.CockArea(x) < 150)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.GrinsAndTeases".Localized());
                else
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.EyesWidenInDelight".Localized());

            if (Player.CockTotal() > 1 && Player.BiggestCockArea() > 232)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.HandsPushAwayBloated".Localized(
                    Player.CockDescript(Player.BiggestCockIndex()),

                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ParagraphBreak".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.TeasesAndTauntsSting".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.FeelTerribleAndShamed".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ABitIrritatedAt".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.SoftSkinOfHer".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BallsWord".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.CockWord".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BeenMilkingCuteLike".Localized(
                Player.Mf("studs", "herms")
            ));

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.CoosGiveATaste".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 6)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.PlungingIntoOversized".Localized(
                    Player.CockDescript(x)
                ));
            else
                if (Player.CockArea(x) <= 150)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BuryingDeepIntoMassive".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.SlowlyWorkingIntoMassive".Localized(
                        Player.CockDescript(x)
                    ));

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.WarmThighsCloseAround".Localized((noFur ? "" : ", feathery"), Player.CockDescript(x), Player.CockDescript(x)));

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.FeelsGoodButInability".Localized());

            if (Player.Tallness <= 48)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.LeanIntoHerBreasts".Localized());
            else
                if (Player.Tallness < 72)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.LeanDownAndPull".Localized());
                else
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.YoudLoveToSuck".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.PantsWithPleasure".Localized());

            if (Player.Tallness < 72)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.UpWord".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.DownWord".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.KissesYouRegaining".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ProstateWord".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BallsWordAgain".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ASqueeze".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.PantAndGroan".Localized());

            if (Player.Tallness <= 48)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.HeadBackDownBetween".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BodyTightlyAgainst".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.IncreasesTheTempo".Localized(
                Player.Mf("boy", "my sweet")
            ));

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.CompulsionToObey".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 500)
            {
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.JismStartsToSpurt".Localized());
                else
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.JismSpurtsFrom".Localized());

                if (Player.CumQ() >= 4000)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.NestFilledWithEnough".Localized());
            }

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.SighAsYouSquirt".Localized());

            if (Player.Balls > 0)
                OutputText(Player.BallsDescriptLight());
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.AndProstate".Localized(
                    Player.CockDescript(x)
                ));

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.AreAchingAndSore".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.CompletelySpent".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) == 0)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.DidKnowInOld".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) <= 2)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ReallyWhenI".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) <= 10)
                    {
                        OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.DoYouHaveFetish".Localized());

                        if (Random.Shared.Next(2) == 0)
                            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.OlderWomen".Localized());
                        else
                            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.Harpies".Localized());

                        OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.OrJustFallingIn".Localized(
                            Player.Mf("boys", "sluts")
                        ));
                    }
                    else
                        OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.HookedArentYou".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.LegsSlowlyUnwind".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.IntoSoftSurface".Localized());
            else
                if (Player.CumQ() < 4000)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.IntoCumSlickedSurface".Localized());
                else
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.IntoMassivePuddle".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.StruggleUpToFeet".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.EachOfAgain".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.YourRemainsSophieGiggles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) == 0)
            {
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.SorryCuteyLipGloss".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BallsWordThird".Localized());
                else
                    OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.BodyWord".Localized());

                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.FillBackUpCum".Localized());
            }
            else
                OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.ImSorryJustGot".Localized());

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.SayEndBreakAgain".Localized());

            LuststickApplication(4);

            Player.Orgasm("Dick");

            SophieFucked();

            OutputText("Scenes.Sophiescene.ConsensualHotSophieDickings.DoTakeHerUp".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), PostSophieSexSnuggle);

            AddButton(1, "Common.Button.No".Localized(), PostSexSophieSnuggleTurnedDown);
        }

        public void PostSophieSexSnuggle()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.SprawlOutInNest".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.EachOf".Localized());

            OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.YourCumSlicked".Localized());

            OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.FeathersTickle".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.AllOf".Localized());

            OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.YourCocks".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.AndBalls".Localized());

            OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.HandsStrokeAndSqueeze".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.SquirtOverYourBody".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.SplatterYourBody".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.DrenchYourBody".Localized());
                    else
                        OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.EruptOverYourBody".Localized());

            OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.SkilledHandsFinish".Localized());

            OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.ThankHerAnd".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.NearlyGiveGoodbye".Localized());
            else
                OutputText("Scenes.Sophiescene.PostSophieSexSnuggle.GiveHerAKiss".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.Luststick);

            Player.ApplyStats(new DynStat(GameStats.Libido, (float)(CoC.Instance.Flags.Get(GameFlagIds.LuststickLibidoInitial))));

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            DoNext(() => ReturnToCamp(4));
        }

        public void PostSexSophieSnuggleTurnedDown()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.PostSexSophieSnuggleTurnedDown.TurnDownHerOffer".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ConsensualSophieSexNoFit()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            var x = Player.BiggestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerProgress, CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerProgress) + 1);

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.SpreadThighsBeckoning".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.OnYourChest".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BetweenYour".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.TracingUpAndDown".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.ReleasesFromBountiful".Localized(
                Player.AssDescript()
            ));

            if (Player.Tallness <= 48)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BreastsPressedIntoFace".Localized());
            else
                if (Player.Tallness < 72)
                {
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BreastsRubbingAgainst".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.OwnWord".Localized());
                    else
                        OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.ChestWord".Localized());
                }
                else
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BreastsPressingStomach".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BeginsToGrowAlong".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Corruption < 50)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.TeasesAndTauntsSting".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.TeasesAndTauntsAgain".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.SoftSkinOfHer".Localized(
                Player.Mf("studs", "herms")
            ));

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.WingsCurlForward".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.LeansDownAndGives".Localized(
                Player.CockHead(x)
            ));

            if (Player.CumQ() < 50)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.SmallDollop".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.DollopWord".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.LargeSquirt".Localized());
                    else
                        OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.TorrentialGush".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.OfPreCumCoos".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.CarefulNotToSnag".Localized());
            else
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.ThighsCarefulNotSnag".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.MuscledFleshOfThighs".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BodyTwistsAndSquirms".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.StandsUpAndBegins".Localized(
                Player.CockDescript(x),

                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (Player.CumQ() >= 1000)
            {
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.NoTimeBellyDistends".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.ContinueToSpurt".Localized());

                if (Player.CumQ() >= 4000)
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.WaveOfStuffSlides".Localized());
            }

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.ParagraphBreak".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.GigglesAndGivesYou".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.AndYourBallsChurn".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.FlopsOnHerBack".Localized());

            if (Player.CumQ() >= 2000)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.InSemenSoakedNest".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.RunsHandsOverHer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) == 0)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.DidKnowInOld".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) <= 2)
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.ReallyWhenI".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) <= 10)
                    {
                        OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.DoYouHaveFetish".Localized());

                        if (Random.Shared.Next(2) == 0)
                            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.OlderWomen".Localized());
                        else
                            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.Harpies".Localized());

                        OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.OrJustFallingIn".Localized(
                            Player.Mf("boys", "sluts")
                        ));
                    }
                    else
                        OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.HookedArentYou".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.SayEndParagraphBreak".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.StruggleUpToFeet".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.EachOf".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.YourRemainsSophieGiggles".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) == 0)
            {
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.SorryCuteyLipGloss".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BallsWord".Localized());
                else
                    OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.BodyWord".Localized());

                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.FillBackUpCum".Localized());
            }
            else
                OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.ImSorryJustGot".Localized());

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.SayEndBreakAgain".Localized());

            LuststickApplication(4);

            Player.Orgasm("Dick");

            SophieFucked();

            OutputText("Scenes.Sophiescene.ConsensualSophieSexNoFit.DoTakeHerUp".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), PostSophieSexSnuggle);

            AddButton(1, "Common.Button.No".Localized(), PostSexSophieSnuggleTurnedDown);
        }

        public void SophieFucked(bool dicked = true)
        {

            if (!Player.IsPregnant() && dicked)
                Player.KnockUp(PregnancyIds.PlayerFather, 48 + Random.Shared.Next(48), 0);

            if (!dicked)
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedSophieLesbian, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedSophieLesbian) + 1);

                if (CoC.Instance.Flags.Get(GameFlagIds.SophieAngryAtPcCounter) <= 0)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.SophieAngryAtPcCounter, 72 + Random.Shared.Next(100));

                    CoC.Instance.Flags.Set(GameFlagIds.TimesPissedOffSophieCounter, CoC.Instance.Flags.Get(GameFlagIds.TimesPissedOffSophieCounter) + 1);
                }
                else
                    CoC.Instance.Flags.Set(GameFlagIds.SophieAngryAtPcCounter, CoC.Instance.Flags.Get(GameFlagIds.SophieAngryAtPcCounter) + Random.Shared.Next(72));
            }

            CoC.Instance.Flags.Set(GameFlagIds.FuckedSophieCounter, CoC.Instance.Flags.Get(GameFlagIds.FuckedSophieCounter) + 1);
        }

        public void LuststickApplication(float hours = 4)
        {

            if (Player.HasPerk(PerkIds.LuststickAdapted))
                return;

            CoC.Instance.Flags.Set(GameFlagIds.LuststickResistanceProgress, CoC.Instance.Flags.Get(GameFlagIds.LuststickResistanceProgress) + (int)Math.Floor(hours / 2));

            if (!Player.HasCock())
                return;

            if (hours > 20)
                hours = 20;

            if (Player.HasStatusEffect(StatusEffectIds.Luststick))
            {
                if (!(Player.StatusEffectValue(StatusEffectIds.Luststick, 0) >= 20))
                {
                    if (Player.StatusEffectValue(StatusEffectIds.Luststick, 0) + hours > 20)
                        hours = 20 - Player.StatusEffectValue(StatusEffectIds.Luststick, 0);

                    Player.ChangeStatusValue(StatusEffectIds.Luststick, 0, hours);
                }
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.LuststickLibidoInitial, Player.Libido);

                Player.CreateStatusEffect(StatusEffectIds.Luststick, hours, 0, 0, 0);
            }
        }

        public void SophieLostCombat()
        {

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerProgress, 0);

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sophiescene.SophieLostCombat.TooWoundedToFight".Localized());
            else
                OutputText("Scenes.Sophiescene.SophieLostCombat.TooTurnedOn".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.UseDick".Localized(), null);

            AddButton(1, "Common.Button.Scissor".Localized(), null);

            AddButton(2, "Common.Button.FuckWclit".Localized(), null);

            AddButton(3, "Common.Button.BimboHer".Localized(), null);

            if (Player.Lust >= 33 && Player.HasCock())
                if (Player.CockThatFits(232) == -1)
                    AddButton(0, "Common.Button.UseDick".Localized(), MaleVictorySophieRapeHUGE);
                else
                    AddButton(0, "Common.Button.UseDick".Localized(), MaleVictorySophieRape);

            if (Player.Lust >= 33 && Player.HasVagina())
            {
                AddButton(1, "Common.Button.Scissor".Localized(), SophieVictoryPussyGrind);

                if (Player.GetClitLength() >= 5)
                    AddButton(2, "Common.Button.FuckWclit".Localized(), FuckDatClit);
            }

            if (Player.HasItem(ItemIds.BimboLiqueur))
                AddButton(3, "Common.Button.BimboHer".Localized(), () => { });

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void SophieWonCombat()
        {

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerProgress, 0);

            if (Player.HasCock())
            {
                if (Player.CockThatFits(232) < 0)
                    TooBigForOwnGoodSophieLossRape();
                else
                    if (Player.BiggestCockArea() <= 5)
                        TinyDickSupremeSophieLoss();
                    else
                        NormalLossRapuuuuSophie();
            }
            else
                SophieLossRapeNoDonguuuu();
        }

        public void MaleVictorySophieRape()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            int x = Player.CockThatFits(232);

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.SophieIs".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRape.TooBeatenToResist".Localized());
            else
                OutputText("Scenes.Sophiescene.MaleVictorySophieRape.TooTurnedToResist".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.NotSatisfiedWithVictory".Localized());

            if (Player.LustPercent > 90)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRape.DrippingWord".Localized());
            else
                if (Player.LustPercent > 50)
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRape.HardWord".Localized());
                else
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRape.HardeningWord".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.MemberHarpyDoesnt".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.ForceIntoWaiting".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.SlapOfFlesh".Localized(
                Player.Mf("stud", "sexy bitch")
            ));

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.DefinitelyStartingToEnjoy".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.WarmthBoilsUp".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() < 50)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRape.SpoogeSpurtsUntil".Localized());
            else
                if (Player.CumQ() < 300)
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRape.SpoogeSpurtsAgain".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Sophiescene.MaleVictorySophieRape.SpoogeBurstsUntil".Localized());
                    else
                        OutputText("Scenes.Sophiescene.MaleVictorySophieRape.SpoogeBurstsAgain".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRape.WellFuckedSlitGapes".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            SophieFucked();

            CombatScene.CleanupAfterCombat();
        }

        public void MaleVictorySophieRapeHUGE()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.NotSatisfiedWithVictory".Localized());

            if (Player.LustPercent > 90)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.DrippingWord".Localized());
            else
                if (Player.LustPercent > 50)
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.HardWord".Localized());
                else
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.HardeningWord".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.MemberHarpyDoesnt".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.PressForwardIntent".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.BallingHandIntoFist".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.TearsWellUp".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.Cocks[x].Length < 36)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.EachTimePushesInto".Localized());
            else
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.PushesPastThemTo".Localized());

            if (Player.Cocks[x].Length >= 40)
            {
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.SucksInsideHerMouth".Localized(
                    Player.CockHead(x)
                ));

                if (Player.Cocks[x].Length < 50)
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.BeforeYouPullBack".Localized());
                else
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.BeforeSheLetsOut".Localized());
            }

            if (Player.Cocks[x].Length >= 50)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.TongueDartsOut".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.DullyNoteThough".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.BodyConvulsesUnderneath".Localized());

            if (Player.Cocks[x].Length < 36)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.TitsWord".Localized());
            else
                if (Player.Cocks[x].Length < 50)
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.FaceWord".Localized());
                else
                    if (Player.Cocks[x].Length < 100)
                        OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.NestWord".Localized());
                    else
                        OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.AndMountainSide".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.SentencePeriod".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.ClenchedTightlyRevel".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.Cocks[x].Length >= 50)
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.SNest".Localized());

                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.SentencePeriodAgain".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.CompletelyFloodHer".Localized());
            }

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.SophieWord".Localized());

            if (Player.Cocks[x].Length < 50)
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.CoveredUnderside".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.RunningHerDrugLaced".Localized(
                    Player.CockHead(x)
                ));

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.ParagraphBreak".Localized());

            OutputText("Scenes.Sophiescene.MaleVictorySophieRapeHUGE.LeansBackAndScoops".Localized(
                Player.SMultiCockDesc()
            ));

            LuststickApplication(8);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            SophieFucked();

            CombatScene.CleanupAfterCombat();
        }

        public void SophieVictoryPussyGrind()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.SophieIs".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.TooBeatenToResist".Localized());
            else
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.TooTurnedToResist".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.HarpyNeedsALesson".Localized((noFur ? "" : " feathery")));

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.ReplySophieShudders".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.SlideOneFourFeet".Localized());
            else
                if (Player.IsGoo())
                    OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.EngulfOneLeg".Localized());
                else
                    if (Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.SlipTailBetweenLegs".Localized());
                    else
                        OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.HookLegUnderHers".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.ShiftCloserUntil".Localized(
                Player.VaginaDescript(0)
            ));

            if (Player.GetClitLength() < 2)
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.PuffsUpInsideFolds".Localized(
                    Player.VaginaDescript(0)
                ));
            else
                if (Player.GetClitLength() < 5)
                    OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.PuffsUpProtruding".Localized(
                        Player.ClitDescript(),

                        Player.VaginaDescript(0)
                    ));
                else
                    OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.PuffsUpGrowing".Localized(
                        Player.VaginaDescript(0)
                    ));

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.LoudSquishesFill".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.PinchANipple".Localized());
            else
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.ArchYourBack".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.AndThroatilyQuestion".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.UnflappableHarpyLooks".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.BegsSophieCups".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.GirlCumRunsDown".Localized(
                Player.AssDescript()
            ));

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.ExplodesSplattering".Localized(
                    Player.SMultiCockDesc()
                ));

                if (!(Player.CumQ() < 400))
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.JismRainsUntil".Localized());
                    else
                        OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.JismRainsAgain".Localized());
            }

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.GruntsAndMoans".Localized());

            if (Player.Vaginas[0].Wetness >= 5)
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.JuicesSquirt".Localized());
            else
                if (Player.Vaginas[0].Wetness >= 3)
                    OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.JuicesSoakNesting".Localized());
                else
                    if (Player.Vaginas[0].Wetness >= 2)
                        OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.JuiceTrickles".Localized());
                    else
                        OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.PussySoaksYourOwn".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.FallAwayFromEach".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedSophieLesbian) == 0)
                OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.CuteyKnowYourWay".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedSophieLesbian) < 6)
                    OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.CuteyGetOffHard".Localized());
                else
                    OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.NotSureYouKeep".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.ParagraphBreak".Localized());

            OutputText("Scenes.Sophiescene.SophieVictoryPussyGrind.DeclareGetDressed".Localized((noFur ? "" : " feathery")));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            SophieFucked(false);

            CombatScene.CleanupAfterCombat();
        }

        public void FuckDatClit()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.FuckDatClit.SophieIs".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Sophiescene.FuckDatClit.TooBeatenToResist".Localized());
            else
                OutputText("Scenes.Sophiescene.FuckDatClit.TooTurnedToResist".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.HarpyNeedsALesson".Localized((noFur ? "" : " feathery")));

            OutputText("Scenes.Sophiescene.FuckDatClit.LaughMakeItQuite".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.ThatsMoreLikeIt".Localized(
                Player.ClitDescript()
            ));

            OutputText("Scenes.Sophiescene.FuckDatClit.MoansEnthusiastically".Localized());

            if (Random.Shared.Next(2) == 0)
                OutputText("Scenes.Sophiescene.FuckDatClit.SeeGetsThicker".Localized());
            else
                OutputText("Scenes.Sophiescene.FuckDatClit.FeelYouCumming".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.InnerWallsStartWork".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.MotherlyHarpyGroans".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.QuickUptakeInRhythm".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Sophiescene.FuckDatClit.WhitenessErupts".Localized());

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Sophiescene.FuckDatClit.SmallPuddleOf".Localized());
                else
                    if (Player.CumQ() < 250)
                        OutputText("Scenes.Sophiescene.FuckDatClit.ThickPuddleOf".Localized());
                    else
                        OutputText("Scenes.Sophiescene.FuckDatClit.ThickPuddleOfAgain".Localized());
            }

            OutputText("Scenes.Sophiescene.FuckDatClit.BreathingHardButHardly".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.ShesNotDoneYet".Localized());

            if (Player.Vaginas[0].Wetness >= 4)
                OutputText("Scenes.Sophiescene.FuckDatClit.JuiceDribblesDown".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.ManagedToTakeControl".Localized());

            OutputText("Scenes.Sophiescene.FuckDatClit.SeveralHoursLater".Localized());

            CombatController.Instance!.Context.Enemy.Lust = 98;

            CombatController.Instance!.Context.Enemy.Health = 2;

            Player.Lust = Player.MaxLust;

            CoC.Instance.Flags.Set(GameFlagIds.CombatBonusXpValue, CombatController.Instance!.Context.Enemy.Experience);

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));
        }

        public void TinyDickSupremeSophieLoss()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.LooksDownAtYour".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.DefeatedWord".Localized());
            else
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.MasturbatingWord".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.ChirpsHerTalonsHook".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.WordsStingMaking".Localized(
                Player.Mf("Maybe she'll have no tits and be teased for the rest of her life, just like her trap of a father.", "Maybe she'll have nice big tits like her trap of a father.")
            ));

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.SoEmbarrassedThatEven".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.WoundsWord".Localized());
            else
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.LustWord".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.KeepYouImmobilized".Localized(
                Player.CockDescript(0)
            ));

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.SophieTitters".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.HummingVibrationBehind".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.AsIfReadingYour".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.FluidsFromGash".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.AmilyMet) > 0 && Player.Corruption > 50)
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.ImagineAmilysVoice".Localized());
            else
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.ItDoesntHelp".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.EyesFixatedOnThe".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.TickleOfBodyHeat".Localized(
                Player.CockDescript(0)
            ));

            if (Player.CumQ() < 50)
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.PumpTheLastOf".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.PumpJismIntoFolds".Localized());
                else
                    OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.BlastThickWaves".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.ParagraphBreak".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.OlderHarpyClimbsOff".Localized());

            if (Player.CumQ() >= 400)
                OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.SpoogeSoaked".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.SurfaceOfNest".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.SpreadsThickThighs".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.ArentSureIfYou".Localized());

            OutputText("Scenes.Sophiescene.TinyDickSupremeSophieLoss.QuicklyLoseTrack".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));

            SophieFucked();

            LuststickApplication(8);

            CombatScene.CleanupAfterCombat();
        }

        public void NormalLossRapuuuuSophie()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            int x = Player.CockThatFits(232);

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.WatchesYourLustWracked".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasVagina())
                OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.AndVagina".Localized(
                    Player.VaginaDescript(0)
                ));

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.RelentlessIntensity".Localized());

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.LeansOverBrushing".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText(Appearance.AllBreastsDescript(Player));
            else
                OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.ChestWord".Localized());

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.SquirmAtExtra".Localized());

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.ProclaimsHarpyMatron".Localized(
                Player.Mf("Boy", "Girl")
            ));

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.PraiseSendsTickling".Localized());

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.PleasedBySubmission".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.MoistureTrickles".Localized());

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.GrabHoldOfHips".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.HipsGrindDown".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.Tallness >= 72)
                OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.NeckWord".Localized());
            else
                if (Player.Tallness > 48)
                    OutputText(Player.FaceDescript());
                else
                    OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.HeadWord".Localized());

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.ReallyStartsToGet".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.FightBackByLicking".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.ClutchOntoArms".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.ClutchOntoArmsAgain".Localized());
                else
                {
                    OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.LatchOntoArms".Localized());

                    if (Player.CumQ() >= 1500)
                        OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.SomeOfItPours".Localized());

                    OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.DifficultToStayConscious".Localized());
                }

            OutputText("Scenes.Sophiescene.NormalLossRapuuuuSophie.HipsPullYouIn".Localized(
                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));

            SophieFucked();

            LuststickApplication(8);

            CombatScene.CleanupAfterCombat();
        }

        public void TooBigForOwnGoodSophieLossRape()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Sophiescene.TooBigForOwnGoodSophieLossRape.ReachesWithClawed".Localized(Player.CockDescript(x), (noFur ? "" : " feathery")));

            OutputText("Scenes.Sophiescene.TooBigForOwnGoodSophieLossRape.HoursLaterYouWake".Localized(
                Player.CockDescript(x)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));

            SophieFucked();

            LuststickApplication(16);

            CombatScene.CleanupAfterCombat();
        }

        public void SophieLossRapeNoDonguuuu()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            OutputText("Scenes.Sophiescene.SophieLossRapeNoDonguuuu.UtterlyDefeatedCollapse".Localized());

            OutputText("Scenes.Sophiescene.SophieLossRapeNoDonguuuu.ByTimeWakeUp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Toughness, -1f));

            CombatScene.CleanupAfterCombat();

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAngryAtPcCounter) <= 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.SophieAngryAtPcCounter, 72 + Random.Shared.Next(100));

                CoC.Instance.Flags.Set(GameFlagIds.TimesPissedOffSophieCounter, CoC.Instance.Flags.Get(GameFlagIds.TimesPissedOffSophieCounter) + 1);
            }
            else
                CoC.Instance.Flags.Set(GameFlagIds.SophieAngryAtPcCounter, CoC.Instance.Flags.Get(GameFlagIds.SophieAngryAtPcCounter) + Random.Shared.Next(72));
        }

    }
}