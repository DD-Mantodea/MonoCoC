using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<NephilaCovenScene>("coc:nephila_coven")]
    public class NephilaCovenScene : Scene
    {

        public override void Entry()
        {

        }

        public void NephilaCovenFirstTime()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenscene.NephilaCovenFirstTime.SlimeGirlAppears".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenFirstTime.OozesEmbrace".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenFirstTime.MightBeTrouble".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenFirstTime.NephilaAttacks".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryNephila);

            CoC.Instance.Flags.Set(GameFlagIds.PcMetNephilaCoven, 1);

            var enemy = EnemyRegistry.Create(new Identifier("coc", "nephila_coven"));

            enemy.OnDefeated = _ => WinRapeChoices();

            enemy.OnWon = _ => NephilaCovenRapesYou();

            CombatScene.Start(enemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void NephilaCovenFirstTimeRepeat()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenscene.NephilaCovenFirstTimeRepeat.AngelicSlime".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenFirstTimeRepeat.UncurlsTentacles".Localized());

            var enemy = EnemyRegistry.Create(new Identifier("coc", "nephila_coven"));

            enemy.OnDefeated = _ => WinRapeChoices();

            enemy.OnWon = _ => NephilaCovenRapesYou();

            CombatScene.Start(enemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void EncounterNephilaCoven()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.PcMetNephilaCoven) == 0)
            {
                NephilaCovenFirstTime();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.PcMetNephilaCoven) == 1)
            {
                NephilaCovenFirstTimeRepeat();

                return ;
            }
        }

        public void WinRapeChoices()
        {
            ClearOutput();

            Action leave = () => CombatScene.CleanupAfterCombat();

            if (!CombatController.InCombat)
            {
                CurrentMonster = EnemyRegistry.Create(SceneIds.Npcs.NephilaCoven);

                leave = () => Go<CampScene>();

                CombatController.Instance!.Context.Enemy.Lust = 100;

                CombatController.Instance!.Context.Enemy.Health = 2;

                if (Player.Lust < 34)
                    Player.Lust = 34;
            }

            if (CombatController.Instance!.Context.Enemy.Health < 1 && Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10 && !Player.HasPerk(PerkIds.NephilaArchQueen))
                OutputText("Scenes.Nephilacovenscene.WinRapeChoices.CollapsesBeaten".Localized());
            else
                if (CombatController.Instance!.Context.Enemy.Health < 1 && !Player.HasPerk(PerkIds.NephilaArchQueen))
                    OutputText("Scenes.Nephilacovenscene.WinRapeChoices.CollapsesBeatenAgain".Localized());
                else
                    if (CombatController.Instance!.Context.Enemy.Health < 1)
                        OutputText("Scenes.Nephilacovenscene.WinRapeChoices.DaughterFalls".Localized());
                    else
                        if (!Player.HasPerk(PerkIds.NephilaArchQueen) && Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                            OutputText("Scenes.Nephilacovenscene.WinRapeChoices.CollapsesGround".Localized());
                        else
                            if (!Player.HasPerk(PerkIds.NephilaArchQueen))
                                OutputText("Scenes.Nephilacovenscene.WinRapeChoices.CollapsesGroundAgain".Localized());
                            else
                                OutputText("Scenes.Nephilacovenscene.WinRapeChoices.DaughterFallsAgain".Localized());

            ClearButtons();

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                AddButton(0, "Common.Button.AcceptOffer".Localized(), null);

            AddButton(1, "Common.Button.Cunnilingus".Localized(), null);

            AddButton(2, "Common.Button.UnbirthNephila".Localized(), null);

            AddButton(1, "Common.Button.Cunnilingus".Localized(), NephilaCovenCunnilingus);

            if (!Player.HasPerk(PerkIds.NephilaArchQueen))
            {
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                    AddButton(0, "Common.Button.AcceptOffer".Localized(), () => Go<CampScene>());

                AddButton(2, "Common.Button.UnbirthNephila".Localized(), NephilaCovenUnbirth);
            }

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<CampScene>());
        }

        public void NephilaCovenRapesYou()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.BowsHead".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.SlumpsDown".Localized());
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.GivesUpMasturbating".Localized());

            if (!Player.HasPerk(PerkIds.NephilaArchQueen))
                OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.SaddenedMother".Localized());
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.OooohSorry".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.GooPresses".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.QuickGrope".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.WhatToDo".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.PushesGut".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.ConfusedMoment".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.SkyBlocked".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.StopsRestraining".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.InchByInch".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.StomachExplodes".Localized(
                Player.SkinFurScales()
            ));

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.BlacksOut".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.PerverseSexuality".Localized());

            if (!Player.HasPerk(PerkIds.NephilaArchQueen))
                OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.WhispersHunt".Localized());
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenRapesYou.DaughtersLazing".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 200f));

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                DoNext(() => ReturnToCamp(1));
        }

        public void NephilaCovenCunnilingus()
        {
            ClearOutput();

            Player.SlimeFeed();

            if (!Player.HasPerk(PerkIds.NephilaArchQueen))
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.LaughsTaunt".Localized());
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.WavesCrowd".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.BeatenProne".Localized());
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.ProneMasturbating".Localized());

            if (!Player.HasPerk(PerkIds.NephilaArchQueen))
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.StillThinks".Localized());
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.ArenaFloor".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.GroansClutches".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.GrinSlipsOff".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.WhatsThatBitch".Localized());

            if (!Player.HasPerk(PerkIds.NephilaArchQueen))
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.GooMoans".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.RollsForward".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.LowersCrotch".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.CollapsesDownward".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.GrindsAgainst".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.WhorishHands".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.BodyWord".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.RidesUpDown".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenCunnilingus.ReachesOrgasm".Localized());

            CombatScene.CleanupAfterCombat();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));
        }

        public void NephilaCovenUnbirth()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.LaughsTaunt".Localized());

            if (CombatController.Instance!.Context.Enemy.Health < 1)
                OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.BeatenProne".Localized());
            else
                OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.ProneMasturbating".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.StillThinks".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.GroansImpact".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.LickMe".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.ThatsIt".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.SlimeEndeavors".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.PancakedNephila".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.BellyBulges".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) <= 19)
            {
                OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.CriesSurprise".Localized());

                if (Player.Tone >= 60)
                    OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.MusclyWord".Localized());
                else
                    if (Player.Thickness < 50)
                        OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.SlenderWord".Localized());
                    else
                        OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.VoluptuousWord".Localized());

                OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.BodyGained".Localized());

                ClearButtons();

                Player.Orgasm("Vaginal");

                Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 1f));

                Player.Thickness += 4;

                Player.ChangeStatusValue(StatusEffectIds.ParasiteNephila, 0, 1);

                if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum))
                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 3)
                {
                    OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.VeritableBroodmother".Localized());

                    Player.Vaginas[0].Wetness = 5;
                }

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 5)
                {
                    OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.HugsMonstrous".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Speed, -5f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

                    Player.Vaginas[0].Wetness = 5;
                }

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 10)
                {
                    OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.HeavesBelly".Localized());

                    OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.PerkAddedBanner".Localized());

                    Player.Vaginas[0].Looseness = 5;

                    Player.ApplyStats(new DynStat(GameStats.Speed, 15f), new DynStat(GameStats.Toughness, 10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 100f));

                    Player.CreatePerk(PerkIds.NephilaQueen, 0, 0, 0, 0);

                    Player.Vaginas[0].Wetness = 4;
                }

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) == 15)
                {
                    OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.SmilesRealizing".Localized());

                    Player.Vaginas[0].Looseness = 5;

                    Player.ApplyStats(new DynStat(GameStats.Speed, 15f), new DynStat(GameStats.Toughness, 10f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 100f));

                    Player.Vaginas[0].Wetness = 4;
                }
                else
                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 11)
                    {
                        OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirth.GainsReminder".Localized());

                        Player.Vaginas[0].Looseness = 5;

                        Player.ApplyStats(new DynStat(GameStats.Speed, -5f), new DynStat(GameStats.Intelligence, 5f), new DynStat(GameStats.Corruption, 100f));

                        Player.Vaginas[0].Wetness = 4;
                    }

                Player.Body.Hips += 1 + Random.Shared.Next(3);

                RemoveButton(0);

                RemoveButton(1);

                RemoveButton(2);

                if (CombatController.InCombat)
                    CombatScene.CleanupAfterCombat();
                else
                    DoNext(() => ReturnToCamp(1));
            }
            else
                DoNext(() => NephilaCovenUnbirthBadEnd());
        }

        public void NephilaCovenUnbirthBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirthBadEnd.CriesSurprise".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirthBadEnd.BellySeemingly".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirthBadEnd.LastThought".Localized());

            OutputText("Scenes.Nephilacovenscene.NephilaCovenUnbirthBadEnd.StomachDetonates".Localized());

            GameOver();
        }

        public void StartAFightWithNephilaCoven()
        {

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "nephila_coven")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false);
        }

    }
}