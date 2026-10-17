using MonoCoC.Contents.World;
using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Achievements;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<WormsScene>("coc:worms")]
    public class WormsScene : Scene
    {

        public override void Entry()
        {
            WormEncounter();
        }

        public void WormEncounter()
        {
            ClearOutput();

            if (!Player.HasStatusEffect(StatusEffectIds.MetWorms))
            {
                OutputText("Scenes.Wormsscene.WormEncounter.PungentSmell".Localized());

                OutputText("Scenes.Wormsscene.WormEncounter.NeverBeforeSeen".Localized());

                OutputText("Scenes.Wormsscene.WormEncounter.StopsDead".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, -10f));

                Player.CreateStatusEffect(StatusEffectIds.MetWorms, 0, 0, 0, 0);

                DoNext(() => ReturnToCamp(1));
            }
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Wormsscene.WormEncounter.MindingBusiness".Localized());

                    OutputText("Scenes.Wormsscene.WormEncounter.WhatDoPrompt".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Confront".Localized(), WormsConfront);

                    AddButton(1, "Common.Button.DoNothing".Localized(), WormsDoNothing);

                    AddButton(2, "Common.Button.Run".Localized(), WormsRun);
                }
                else
                {
                    OutputText("Scenes.Wormsscene.WormEncounter.StumblesOn".Localized());

                    DoNext(() => ReturnToCamp(1));
                }
        }

        public void WormToggle()
        {
            ClearOutput();

            OutputText("Scenes.Wormsscene.WormToggle.ComesAcrossWorms".Localized());

            OutputText("Scenes.Wormsscene.WormToggle.ParasitePrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Aroused".Localized(), WormsOn);

            AddButton(1, "Common.Button.WhoCares".Localized(), WormsPartiallyOn);

            AddButton(2, "Common.Button.GrossedOut".Localized(), WormsOff);
        }

        public void WormsOn()
        {
            ClearOutput();

            OutputText("Scenes.Wormsscene.WormsOn.ThinksHarmless".Localized());

            OutputText("Scenes.Wormsscene.WormsOn.ChangeMindBanner".Localized());

            OptionStore.Instance.Set("parasites", 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void WormsPartiallyOn()
        {
            ClearOutput();

            OutputText("Scenes.Wormsscene.WormsPartiallyOn.ShrugsWalks".Localized());

            OutputText("Scenes.Wormsscene.WormsPartiallyOn.ChangeMindBanner".Localized());

            OptionStore.Instance.Set("parasites", 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void WormsOff()
        {
            ClearOutput();

            OutputText("Scenes.Wormsscene.WormsOff.ShuddersRevulsion".Localized());

            OutputText("Scenes.Wormsscene.WormsOff.ChangeMindBanner".Localized());

            OptionStore.Instance.Set("parasites", 0);

            DoNext(() => ReturnToCamp(1));
        }

        public void WormsConfront()
        {
            ClearOutput();

            OutputText("Scenes.Wormsscene.WormsConfront.ConfrontsWorms".Localized());

            var worms = EnemyRegistry.Create(EnemyIds.WormMass);

            worms.OnDefeated = hpVictory =>
            {
                CoC.Instance.Flags.Set(GameFlagIds.WormsMassKilled, 1);

                CombatScene.CleanupAfterCombat();
            };

            worms.OnWon = hpVictory => Infest1();

            CombatScene.Start(worms, () => Go<CampScene>());
        }

        public void WormsDoNothing()
        {
            ClearOutput();

            OutputText("Scenes.Wormsscene.WormsDoNothing.DoesNothing".Localized());

            var worms = EnemyRegistry.Create(EnemyIds.WormMass);

            worms.OnDefeated = hpVictory =>
            {
                CoC.Instance.Flags.Set(GameFlagIds.WormsMassKilled, 1);

                CombatScene.CleanupAfterCombat();
            };

            worms.OnWon = hpVictory => Infest1();

            CombatScene.Start(worms, () => Go<CampScene>());
        }

        public void WormsRun()
        {
            ClearOutput();

            if (Player.Speed > Random.Shared.Next(35))
            {
                OutputText("Scenes.Wormsscene.WormsRun.InstinctsOverwhelm".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Wormsscene.WormsRun.TurnsToRun".Localized());

                var worms = EnemyRegistry.Create(EnemyIds.WormMass);

                worms.OnDefeated = hpVictory =>
                {
                    CoC.Instance.Flags.Set(GameFlagIds.WormsMassKilled, 1);

                    CombatScene.CleanupAfterCombat();
                };

                worms.OnWon = hpVictory => Infest1();

                CombatScene.Start(worms, () => Go<CampScene>());
            }
        }

        public void Infest1()
        {

            OutputText("Scenes.Wormsscene.Infest1.TrappedInMass".Localized());

            OutputText("Scenes.Wormsscene.Infest1.CriesShock".Localized());

            OutputText("Scenes.Wormsscene.Infest1.ExcitedFluids".Localized());

            if (Player.Corruption < 25)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

                Player.Corruption = 25;
            }

            if (!Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.EverInfested) < 1)
                    CoC.Instance.Flags.Set(GameFlagIds.EverInfested, 1);

                Player.CreateStatusEffect(StatusEffectIds.Infested, 0, 0, 0, 0);

                if (Player.HasPerk(PerkIds.ParasiteMusk) || Player.HasStatusEffect(StatusEffectIds.ParasiteSlug))
                {
                    OutputText("Scenes.Wormsscene.Infest1.WormsDestroyed".Localized());

                    Player.RemovePerk(PerkIds.ParasiteMusk);

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlugReproduction);
                }

                Player.ApplyStats(new DynStat(GameStats.Corruption, 0f));
            }

            CombatScene.CleanupAfterCombat();
        }

        public void InfestOrgasm()
        {
            ClearOutput();

            OutputText("Scenes.Wormsscene.InfestOrgasm.CeaselessSquirming".Localized());

            Player.Orgasm("Dick");

            if ((Player.HasStatusEffect(StatusEffectIds.Infested)) && (Player.StatusEffectValue(StatusEffectIds.Infested, 0) < 5))
                {
                    Player.ChangeStatusValue(StatusEffectIds.Infested, 0, 1);

                    Player.CumMultiplier += 0.5f;

                    if (Player.StatusEffectValue(StatusEffectIds.Infested, 0) == 5)
                        if (Player.Balls == 0)
                            OutputText("Scenes.Wormsscene.InfestOrgasm.AfterEmptying".Localized());
                        else
                        {
                            OutputText("Scenes.Wormsscene.InfestOrgasm.AwkwardSensation".Localized());

                            Player.BallSize += 2;

                            Player.CumMultiplier += 1;
                        }
                }

            DoNext(() => ReturnToCamp(2));
        }

        public void PlayerInfest()
        {
            ClearOutput();

            if (Player.Fatigue + Player.PhysicalCost(40) > Player.MaxFatigue())
            {
                OutputText("Scenes.Wormsscene.PlayerInfest.TriesSummon".Localized());

                Player.ChangeFatigue(100 - Player.Fatigue);

                CombatController.Instance!.EnemyTurn();

                return ;
            }

            if (CombatController.Instance!.Context.Enemy.ShortName != "Izma")
            {
                Player.ChangeFatigue(40, FatigueType.Magical);

                OutputText("Scenes.Wormsscene.PlayerInfest.GreatSqueeze".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.IzmaEncounterCounter, 0);

                CoC.Instance.Flags.Set(GameFlagIds.IzmaWormsScared, 1);

                CombatController.InCombat = false;

                CombatController.Instance!.ClearStatuses();

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (CombatController.Instance!.Context.Enemy.HasStatusEffect(StatusEffectIds.TwuWuv))
            {
                OutputText("Scenes.Wormsscene.PlayerInfest.ExposesSelf".Localized());

                OutputText("Scenes.Wormsscene.PlayerInfest.DemonTeases".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Sensitivity / 10)));

                CombatController.Instance!.EnemyTurn();

                return ;
            }

            Player.ChangeFatigue(40, FatigueType.Magical);

            OutputText("Scenes.Wormsscene.PlayerInfest.GreatSqueezeAgain".Localized());

            if (CombatController.Instance!.Context.Enemy.ShortName != "minotaur" || CombatController.Instance!.Context.Enemy.ShortName != "coc:imp")
            {
                if (CurrentMonster.LustPercent > 70)
                {
                    OutputText("Scenes.Wormsscene.PlayerInfest.LoadWashes".Localized(
                         CombatController.Instance!.Context.Enemy.ShortName,

                         CombatController.Instance!.Context.Enemy.ShortName,

                         CombatController.Instance!.Context.Enemy.ShortName,

                         CombatController.Instance!.Context.Enemy.CockDescriptShort(0),

                         CombatController.Instance!.Context.Enemy.ShortName,

                         CombatController.Instance!.Context.Enemy.ShortName,

                         CombatController.Instance!.Context.Enemy.ShortName,

                         CombatController.Instance!.Context.Enemy.ShortName
                    ));

                    CombatController.Instance!.Context.Enemy.Lust = 100;

                    CombatScene.CleanupAfterCombat();

                    return ;
                }
                else
                {
                    OutputText("Scenes.Wormsscene.PlayerInfest.MonsterWatches".Localized());

                    CombatController.Instance!.Context.Enemy.Lust += 5;
                }
            }
            else
                if (CombatController.Instance!.Context.Enemy.ShortName != "lizan rogue")
                    OutputText("Scenes.Wormsscene.PlayerInfest.LizanNoDodge".Localized());
                else
                    if (CombatController.Instance!.Context.Enemy.ShortName != "anemone")
                    {
                        OutputText("Scenes.Wormsscene.PlayerInfest.WhiteMess".Localized());

                        CombatController.Instance!.Context.Enemy.Health += 20;
                    }
                    else
                        if (CombatController.Instance!.Context.Enemy.ShortName != "Volcanic Golem")
                            OutputText("Scenes.Wormsscene.PlayerInfest.WormsCharred".Localized());
                        else
                            OutputText("Scenes.Wormsscene.PlayerInfest.FluidsBathe".Localized());

            AwardAchievement(AchievementIds.CumCannon);

            Player.ApplyStats(new DynStat(GameStats.Lust, -20f));

            CombatController.Instance!.EnemyTurn();
        }

        public bool EligibleForWormInfestation()
        {

            var isEligible = true;

            if (!Player.HasCock())
                isEligible = false;

            if (CabinInterior.HasBed && CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "")
                isEligible = false;

            if (CoC.Instance.Flags.Get(GameFlagIds.ArianMorning) > 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberMorning) > 0)
                isEligible = false;

            if (CampScene.CampGuarded())
                isEligible = false;

            return isEligible;
        }

        public void NightTimeInfestation()
        {

            OutputText("Scenes.Wormsscene.NightTimeInfestation.OddBanner".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.DreamsPast".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.MmmAnnelid".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.SnapsAwake".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Wormsscene.NightTimeInfestation.PluralAre".Localized());
            else
                OutputText("Scenes.Wormsscene.NightTimeInfestation.SingularIs".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.OpenVulnerable".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Wormsscene.NightTimeInfestation.PluralSuffix".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.OneAfterAnother".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.GrabsHold".Localized());

            if (Player.Cocks[Player.BiggestCockIndex()].Thickness >= 6)
                OutputText("Scenes.Wormsscene.NightTimeInfestation.TightAroundGirth".Localized());
            else
                OutputText("Scenes.Wormsscene.NightTimeInfestation.TightlyGirth".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.HopingContain".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Wormsscene.NightTimeInfestation.VainEffort".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.PluralEsAre".Localized());
                else
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.SingularIsAgain".Localized());

                OutputText("Scenes.Wormsscene.NightTimeInfestation.AlreadyTurning".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.AnWord".Localized());

                OutputText("Scenes.Wormsscene.NightTimeInfestation.InfestationHighway".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.PluralSuffixAgain".Localized());

                OutputText("Scenes.Wormsscene.NightTimeInfestation.SentencePeriod".Localized());

                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.ItsWord".Localized());
                else
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.TheirWord".Localized());

                OutputText("Scenes.Wormsscene.NightTimeInfestation.InsideWord".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.PluralAreAgain".Localized());
                else
                    OutputText("Scenes.Wormsscene.NightTimeInfestation.SingularIsThird".Localized());

                OutputText("Scenes.Wormsscene.NightTimeInfestation.FilledToBrim".Localized());
            }

            OutputText("Scenes.Wormsscene.NightTimeInfestation.TaperedHeads".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.SurgeBlood".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.MoaningLooksOn".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.PainfulPressure".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Wormsscene.NightTimeInfestation.PluralSuffixThird".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.AbsorbNutrition".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.BigOneBathe".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.RelaxAfterglow".Localized());

            OutputText("Scenes.Wormsscene.NightTimeInfestation.InfestedAgainBanner".Localized());

            if (!Player.HasStatusEffect(StatusEffectIds.Infested))
            {
                Player.CreateStatusEffect(StatusEffectIds.Infested, 0, 0, 0, 0);

                Player.ApplyStats(new DynStat(GameStats.Corruption, 0f));
            }

            if (Player.Corruption < 25)
            {
                Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

                Player.Corruption = 25;
            }

            PlayerMenu();
        }

    }
}