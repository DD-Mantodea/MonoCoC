using MonoCoC.Attributes;

using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Achievements;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<NephilaSlimeScene>("coc:nephila")]
    public class NephilaSlimeScene : Scene
    {

        public override void Entry()
        {
            CheatFunctiondoNotLook();
        }

        public void CheatFunctiondoNotLook()
        {

            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephila, 0, 1);
        }

        public bool NephilaUpdate()
        {

            var retorno = false;

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephila))
            {
                if (!Player.HasVagina())
                {
                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.NoWomb".Localized());

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephila);

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

                    CoC.Instance.Flags.Set(GameFlagIds.NephilaCovenQueenCrowned, 0);

                    if (Player.HasPerk(PerkIds.NephilaQueen))
                        Player.RemovePerk(PerkIds.NephilaQueen);

                    if (Player.HasPerk(PerkIds.NephilaArchQueen))
                        Player.RemovePerk(PerkIds.NephilaArchQueen);

                    retorno = true;
                }

                var nephilaamount = Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0);

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephilaNeedCum, 2) / nephilaamount >= 4)
                {
                    retorno = true;

                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.NearlyCrippled".Localized());

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

                    Player.ApplyStats(new DynStat(GameStats.Speed, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

                    Player.ChangeStatusValue(StatusEffectIds.ParasiteNephila, 0, -1);

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 0)
                    {
                        Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephila);

                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.HardToCount".Localized());

                        CoC.Instance.Flags.Set(GameFlagIds.NephilaCovenQueenCrowned, 0);

                        if (Player.HasPerk(PerkIds.NephilaQueen))
                            Player.RemovePerk(PerkIds.NephilaQueen);

                        if (Player.HasPerk(PerkIds.NephilaArchQueen))
                            Player.RemovePerk(PerkIds.NephilaArchQueen);
                    }
                }

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 1) >= 2)
                {
                    retorno = true;

                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.FeelsSquirming".Localized());

                    Player.ChangeStatusValue(StatusEffectIds.ParasiteNephila, 0, 1);

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 3)
                    {
                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.VeritableBroodmother".Localized());

                        Player.Body.Hips += 1 + Random.Shared.Next(3);

                        Player.Vaginas[0].Wetness = 5;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 5)
                    {
                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.HugsBelly".Localized());

                        Player.Body.Hips += 1 + Random.Shared.Next(3);

                        Player.ApplyStats(new DynStat(GameStats.Speed, -5f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

                        Player.Vaginas[0].Wetness = 5;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 10)
                    {
                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.HeavesSelf".Localized());

                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.ImagesDeep".Localized());

                        Player.Vaginas[0].Looseness = 5;

                        Player.Body.Hips += 1 + Random.Shared.Next(3);

                        Player.ApplyStats(new DynStat(GameStats.Speed, 15f), new DynStat(GameStats.Toughness, 10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 100f));

                        AwardAchievement(AchievementIds.NephilaQueen);

                        Player.CreatePerk(PerkIds.NephilaQueen, 0, 0, 0, 0);

                        Player.Vaginas[0].Wetness = 4;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 15)
                    {
                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SmilesSelf".Localized());

                        Player.Vaginas[0].Looseness = 5;

                        Player.ApplyStats(new DynStat(GameStats.Speed, 15f), new DynStat(GameStats.Toughness, 10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 100f));

                        Player.Body.Hips += 1 + Random.Shared.Next(3);

                        Player.Vaginas[0].Wetness = 4;
                    }

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 20 && CoC.Instance.Flags.Get(GameFlagIds.NephilaCovenQueenCrowned) > 0)
                    {
                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.AwakesNight".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0)
                            OutputText("Scenes.Nephilaslimescene.NephilaUpdate.FirmlyWedged".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) == 0)
                            OutputText("Scenes.Nephilaslimescene.NephilaUpdate.CompletelyFilling".Localized());

                        OutputText("Scenes.Nephilaslimescene.NephilaUpdate.NephilaAmused".Localized());

                        Player.Vaginas[0].Looseness = 5;

                        Player.Body.Hips += 1 + Random.Shared.Next(3);

                        Player.ApplyStats(new DynStat(GameStats.Speed, 1f), new DynStat(GameStats.Toughness, -10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 10f), new DynStat(GameStats.Corruption, 100f));

                        Player.Vaginas[0].Wetness = 4;
                    }
                    else
                        if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 11)
                        {
                            OutputText("Scenes.Nephilaslimescene.NephilaUpdate.WhateverGains".Localized());

                            Player.Vaginas[0].Looseness = 5;

                            Player.Body.Hips += 1 + Random.Shared.Next(3);

                            Player.ApplyStats(new DynStat(GameStats.Speed, -5f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Corruption, 100f));

                            Player.Vaginas[0].Wetness = 4;
                        }

                    Player.ChangeStatusValue(StatusEffectIds.ParasiteNephila, 1, -3);
                }

                if (View.Time.Days % 3 == 0 && CoC.Instance.Flags.Get(GameFlagIds.ParasiteNephilaDaydone) != View.Time.Days)
                    if (!Player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                    {
                        retorno = true;

                        CoC.Instance.Flags.Set(GameFlagIds.ParasiteNephilaDaydone, View.Time.Days);

                        switch (Random.Shared.Next(4))
                        {
                            case 0:
                            {
                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                            {
                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SpreadsLegs".Localized());

                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.HandmaidenHisses".Localized());

                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.AwakesFromSleep".Localized());
                            }
                            else
                                if (Player.HasPerk(PerkIds.NephilaQueen))
                                {
                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.TakesDownImp".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.GrabsDemonicCock".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.LowersMoaning".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.TakenDownBanner".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.ApproachReveal".Localized((Player.HasTailInsteadOfLegs() ? "slide your fingers over your crotch" : "turn and spread your legs")));
                                }

                            OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SnapsOut".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum, 0, 1, nephilaamount, 0);
                            }

                            break;

                            case 1:
                            {
                            OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SeducingImages".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum, 0, 4, nephilaamount, 0);
                            }

                            break;

                            case 2:
                            {
                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                            {
                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.TookEffort".Localized());

                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.CreatureCollapses".Localized());

                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.AwakesFromSleepAgain".Localized());
                            }
                            else
                                if (Player.HasPerk(PerkIds.NephilaQueen))
                                {
                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.TookEffortAgain".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.CreatureCollapsesAgain".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.StillAttempts".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SlidesCock".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.BestEfforts".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.KnowsWhyWalked".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.PointsCock".Localized());

                                    OutputText("Scenes.Nephilaslimescene.NephilaUpdate.PresentsSelf".Localized());
                                }

                            OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SnapsOutAgain".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum, 0, 2, nephilaamount, 0);
                            }

                            break;

                            case 3:
                            {
                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                            {
                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.BathingBanner".Localized());

                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SpreadsArms".Localized());

                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.OrbitsDistorted".Localized());

                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.AwakesFromSleepThird".Localized());
                            }
                            else
                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.LovingImages".Localized());

                            Player.CreateStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum, 0, 10, nephilaamount, 0);
                            }

                            break;
                        }
                    }
                    else
                        if (View.Time.Days != CoC.Instance.Flags.Get(GameFlagIds.ParasiteNephilaDaydone))
                        {
                            retorno = true;

                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.SquirmingArmy".Localized());
                            else
                                OutputText("Scenes.Nephilaslimescene.NephilaUpdate.NephilaSlimesInside".Localized());

                            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                                Player.ApplyStats(new DynStat(GameStats.Speed, -10f), new DynStat(GameStats.Toughness, -10f), new DynStat(GameStats.Strength, -10f), new DynStat(GameStats.Sensitivity, 15f), new DynStat(GameStats.Corruption, 15f));
                            else
                                Player.ApplyStats(new DynStat(GameStats.Speed, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

                            Player.ChangeStatusValue(StatusEffectIds.ParasiteNephilaNeedCum, 2, nephilaamount);

                            CoC.Instance.Flags.Set(GameFlagIds.ParasiteNephilaDaydone, View.Time.Days);
                        }

                return retorno;
            }

            return retorno;
        }

        public void EncounterNephila()
        {
            ClearOutput();

            OutputText("Scenes.Nephilaslimescene.EncounterNephila.WorksThrough".Localized());

            OutputText("Scenes.Nephilaslimescene.EncounterNephila.SuddenScrape".Localized());

            if (Player.Gender == Gender.Male || Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Nephilaslimescene.EncounterNephila.SemenSmell".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Nephilaslimescene.EncounterNephila.FreshSmell".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Nephilaslimescene.EncounterNephila.InvadesTerritory".Localized());

            OutputText("Scenes.Nephilaslimescene.EncounterNephila.RaisesWeapon".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryNephila);

            var nephila = EnemyRegistry.Create(EnemyIds.NephilaSlime);

            nephila.OnDefeated = hpVictory => DefeatNephilaSlime();

            nephila.OnWon = hpVictory => LoseToNephilaSlime();

            CombatScene.Start(nephila, () => Go<CampScene>());
        }

        public void LoseToNephilaSlime()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Nephilaslimescene.LoseToNephilaSlime.LosingControl".Localized());
            else
                OutputText("Scenes.Nephilaslimescene.LoseToNephilaSlime.TooWeak".Localized());

            if (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0)
                LoseToNephilaWithCockAnBallsAnCunt();
            else
                if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                    DudesLoseToNephila();
                else
                    if (Player.HasVagina())
                        LoseToNephilaWithPCCunt();
                    else
                    {
                        OutputText("Scenes.Nephilaslimescene.LoseToNephilaSlime.GivesOnceOver".Localized());

                        Player.HealthChange(-20);

                        CombatScene.CleanupAfterCombat();
                    }
        }

        public void NephilaInfest()
        {

            Player.CreateStatusEffect(StatusEffectIds.ParasiteNephila, 1, 0, 0, 0);

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteEel))
            {
                OutputText("Scenes.Nephilaslimescene.NephilaInfest.EelsInside".Localized());

                Player.RemoveStatusEffect(StatusEffectIds.ParasiteEel);

                Player.RemoveStatusEffect(StatusEffectIds.ParasiteEelNeedCum);

                Player.RemovePerk(PerkIds.ParasiteQueen);
            }
        }

        public void LoseToNephilaWithCockAnBallsAnCunt()
        {

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.HaughtyLaughs".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.CommandsStrip".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.SetsLastBit".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.EyesLocked".Localized());

            Player.CuntChange(25, true, true, false);

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.StimulationArouses".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.GaspsTentacle".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.KeptInPlace".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.LaughsWatches".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.FilledSensation".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.GetsUpSmiling".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithCockAnBallsAnCunt.LeavesThrashing".Localized());

            NephilaInfest();

            Player.Orgasm("Vaginal");

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void DudesLoseToNephila()
        {

            var x = Random.Shared.Next(Player.Cocks.Count);

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.NoDanger".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.OverwhelmedLust".Localized());
            else
                OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.BatteredWord".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.CantRip".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.ShortWork".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.PullsHerselfUp".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.OozeCuntGrinds".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.TeasesHead".Localized());

            if (Player.CockArea(x) <= CombatController.Instance!.Context.Enemy.VaginalCapacity())
                OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.ToTheBase".Localized());
            else
                OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.AsFarAsCan".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.EyesNarrow".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.NearingOrgasm".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() > 1500)
                OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.FillingCum".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.SurprisedFeeling".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.SofteningOut".Localized());

            OutputText("Scenes.Nephilaslimescene.DudesLoseToNephila.SemenLeaks".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToNephilaWithPCCunt()
        {

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.HaughtyLaughs".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.CommandsStrip".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.SetsLastBit".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.EyesLocked".Localized());

            Player.CuntChange(25, true, true, false);

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.StimulationArouses".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.GaspsTentacle".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.LaughsWatches".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.FilledFeelings".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.GetsUpSmiling".Localized());

            OutputText("Scenes.Nephilaslimescene.LoseToNephilaWithPCCunt.LeavesThrashing".Localized());

            NephilaInfest();

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void DefeatNephilaSlime()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
            {
                OutputText("Scenes.Nephilaslimescene.DefeatNephilaSlime.UnableControl".Localized());

                if (Player.Lust >= 33)
                    OutputText("Scenes.Nephilaslimescene.DefeatNephilaSlime.ConsiderHelping".Localized());
            }
            else
            {
                OutputText("Scenes.Nephilaslimescene.DefeatNephilaSlime.TooWeakFight".Localized());

                if (Player.Lust >= 33)
                    OutputText("Scenes.Nephilaslimescene.DefeatNephilaSlime.ConsiderUsing".Localized());
            }

            if (Player.Lust < 33 || Player.Gender == Gender.Genderless)
            {
                CombatScene.CleanupAfterCombat();

                return ;
            }

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

    }
}