using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<InfestedChameleonGirlScene>("coc:infested_chameleon_girl")]
    public class InfestedChameleonGirlScene : Scene
    {

        public readonly System.Collections.Generic.List<Func<int>> dreams;

        public InfestedChameleonGirlScene()
        {
            dreams =

            [
                MinotaurDream,

                MouseDream,

                ImpDream,

                AnemoneDream,

                TentacleBeastDream
            ];
        }

        public override void Entry() => EncounterChameleon();

        public bool ParasiteUpdate()
        {

            var retorno = false;

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteEel))
            {
                if (!Player.HasVagina())
                {
                    OutputText("Scenes.Infestedchameleongirlscene.ParasiteUpdate.NoWomb".Localized());

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteEel);

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteEelNeedCum);

                    if (Player.HasPerk(PerkIds.ParasiteQueen))
                        Player.RemovePerk(PerkIds.ParasiteQueen);

                    retorno = true;
                }

                var parasiteamount = Player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0);

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteEelNeedCum, 2) / parasiteamount >= 4)
                {
                    retorno = true;

                    OutputText("Scenes.Infestedchameleongirlscene.ParasiteUpdate.NearlyCrippled".Localized());

                    Player.RemoveStatusEffect(StatusEffectIds.ParasiteEelNeedCum);

                    Player.ApplyStats(new DynStat(GameStats.Speed, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

                    Player.ChangeStatusValue(StatusEffectIds.ParasiteEel, 0, -1);

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0) <= 0)
                    {
                        Player.RemoveStatusEffect(StatusEffectIds.ParasiteEel);

                        OutputText("Scenes.Infestedchameleongirlscene.ParasiteUpdate.HardToCount".Localized());

                        if (Player.HasPerk(PerkIds.ParasiteQueen))
                            Player.RemovePerk(PerkIds.ParasiteQueen);
                    }
                }

                if (Player.StatusEffectValue(StatusEffectIds.ParasiteEel, 1) >= 2)
                {
                    retorno = true;

                    OutputText("Scenes.Infestedchameleongirlscene.ParasiteUpdate.FeelsSquirming".Localized());

                    Player.ChangeStatusValue(StatusEffectIds.ParasiteEel, 0, 1);

                    if (Player.StatusEffectValue(StatusEffectIds.ParasiteEel, 0) == 5)
                    {
                        OutputText("Scenes.Infestedchameleongirlscene.ParasiteUpdate.VeritableBroodmother".Localized());

                        OutputText("Scenes.Infestedchameleongirlscene.ParasiteUpdate.PerkAddedBanner".Localized());

                        Player.CreatePerk(PerkIds.ParasiteQueen, 0, 0, 0, 0);
                    }

                    Player.ChangeStatusValue(StatusEffectIds.ParasiteEel, 1, -3);
                }

                if (View.Time.Days % 3 == 0 && CoC.Instance.Flags.Get(GameFlagIds.ParasiteEelDaydone) != View.Time.Days)
                    if (!Player.HasStatusEffect(StatusEffectIds.ParasiteEelNeedCum))
                    {
                        retorno = true;

                        CoC.Instance.Flags.Set(GameFlagIds.ParasiteEelDaydone, View.Time.Days);

                        int currDream = dreams[Random.Shared.Next(dreams.Count)]();

                        Player.CreateStatusEffect(StatusEffectIds.ParasiteEelNeedCum, 0, currDream, parasiteamount, 0);
                    }
                    else
                        if (View.Time.Days != CoC.Instance.Flags.Get(GameFlagIds.ParasiteEelDaydone))
                        {
                            retorno = true;

                            OutputText("Scenes.Infestedchameleongirlscene.ParasiteUpdate.ParasitesGrow".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Speed, -1f), new DynStat(GameStats.Toughness, -1f), new DynStat(GameStats.Strength, -1f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

                            Player.ChangeStatusValue(StatusEffectIds.ParasiteEelNeedCum, 2, parasiteamount);

                            CoC.Instance.Flags.Set(GameFlagIds.ParasiteEelDaydone, View.Time.Days);
                        }

                return retorno;
            }

            return retorno;
        }

        public int MinotaurDream()
        {

            if (Player.HasPerk(PerkIds.ParasiteQueen))
            {
                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.TookEffort".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.CreatureCollapses".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.StillAttempts".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.SlidesCock".Localized());
            }
            else
            {
                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.BestEfforts".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.KnowsWhyWalked".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.PointsCock".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.PresentsSelf".Localized());
            }

            OutputText("Scenes.Infestedchameleongirlscene.MinotaurDream.SnapsOut".Localized());

            return 2; 
        }

        public int MouseDream()
        {

            OutputText("Scenes.Infestedchameleongirlscene.MouseDream.MouseSeduceDream".Localized());

            return 4; 
        }

        public int ImpDream()
        {

            if (Player.HasPerk(PerkIds.ParasiteQueen))
            {
                OutputText("Scenes.Infestedchameleongirlscene.ImpDream.TakesDownImp".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.ImpDream.GrabsCock".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.ImpDream.LowersMoaning".Localized());
            }
            else
            {
                OutputText("Scenes.Infestedchameleongirlscene.ImpDream.TakenDown".Localized());

                OutputText("Scenes.Infestedchameleongirlscene.ImpDream.ApproachReveal".Localized());
            }

            OutputText("Scenes.Infestedchameleongirlscene.ImpDream.SnapsOut".Localized());

            return 1; 
        }

        public int AnemoneDream()
        {

            OutputText("Scenes.Infestedchameleongirlscene.AnemoneDream.AnemoneLoveDream".Localized());

            return 10; 
        }

        public int TentacleBeastDream()
        {

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.BeastCrushes".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.FallsGround".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.ThoughtsInvade".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.TendrilWorms".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.BucksUnwittingly".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.TentacleWraps".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.GivesWhatWanted".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.TentacleBeastDream.SnapsOut".Localized());

            return 39; 
        }

        public void EncounterChameleon()
        {
            ClearOutput();

            OutputText("Scenes.Infestedchameleongirlscene.EncounterChameleon.WorksThrough".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.EncounterChameleon.SuddenScrape".Localized());

            if (Player.Gender == Gender.Male || Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Infestedchameleongirlscene.EncounterChameleon.SemenSmell".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Infestedchameleongirlscene.EncounterChameleon.FreshSmell".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Infestedchameleongirlscene.EncounterChameleon.InvadesTerritory".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.EncounterChameleon.RaisesWeapon".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetChameleon, CoC.Instance.Flags.Get(GameFlagIds.TimesMetChameleon) + 1);

            var enemy = EnemyRegistry.Create(EnemyIds.InfestedChameleonGirl);

            enemy.OnDefeated = _ => DefeatChameleonGirl();

            enemy.OnWon = _ => LoseToChameleonGirl();

            CombatScene.Start(enemy, () => Go<CampScene>());
        }

        public void LoseToChameleonGirl()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonGirl.LosingControl".Localized());
            else
                OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonGirl.TooWeak".Localized());

            if (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0)
                LoseToChameleonWithCockAnBallsAnCunt();
            else
                if (Player.Gender == Gender.Male || (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0))
                    DudesLoseToChamChams();
                else
                    if (Player.HasVagina())
                        LoseToChamChamWithPCCunt();
                    else
                    {
                        OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonGirl.GivesOnceOver".Localized());

                        Player.HealthChange(-20);

                        CombatScene.CleanupAfterCombat();
                    }
        }

        public void EelInfest()
        {

            Player.CreateStatusEffect(StatusEffectIds.ParasiteEel, 1, 0, 0, 0);

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteNephila))
            {
                OutputText("Scenes.Infestedchameleongirlscene.EelInfest.SlimesInside".Localized());

                Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephila);

                Player.RemoveStatusEffect(StatusEffectIds.ParasiteNephilaNeedCum);

                CoC.Instance.Flags.Set(GameFlagIds.NephilaCovenQueenCrowned, 0);

                Player.RemovePerk(PerkIds.NephilaQueen);

                Player.RemovePerk(PerkIds.NephilaArchQueen);
            }
        }

        public void LoseToChameleonWithCockAnBallsAnCunt()
        {

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.HaughtyLaughs".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.CommandsStrip".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.SetsLastBit".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.EyesLocked".Localized());

            Player.CuntChange(25, true, true, false);

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.StimulationArouses".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.GaspsTentacle".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.KeptInPlace".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.FilledSensation".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.GetsUpSmiling".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.LeavesThrashing".Localized());

            EelInfest();

            Player.Orgasm("Vaginal");

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void DudesLoseToChamChams()
        {

            var x = Random.Shared.Next(Player.Cocks.Count);

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.NoDanger".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.OverwhelmedLust".Localized());
            else
                OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.BatteredWord".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.CantRip".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.ShortWork".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.GripsTree".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.CuntGrinds".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.TeasesHead".Localized());

            if (Player.CockArea(x) <= CombatController.Instance!.Context.Enemy.VaginalCapacity())
                OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.ToTheBase".Localized());
            else
                OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.AsFarAsCan".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.EyesNarrow".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.NearingOrgasm".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() > 1500)
                OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.FillingCum".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.SurprisedFeeling".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.SofteningOut".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.DudesLoseToChamChams.SemenLeaks".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToChamChamWithPCCunt()
        {

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.HaughtyLaughs".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.CommandsStrip".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChamChamWithPCCunt.SetsLastBit".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.EyesLocked".Localized());

            Player.CuntChange(25, true, true, false);

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChamChamWithPCCunt.StimulationArouses".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChamChamWithPCCunt.GaspsTentacle".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChamChamWithPCCunt.FilledFeelings".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.GetsUpSmiling".Localized());

            OutputText("Scenes.Infestedchameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.LeavesThrashing".Localized());

            EelInfest();

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void DefeatChameleonGirl()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
            {
                OutputText("Scenes.Infestedchameleongirlscene.DefeatChameleonGirl.UnableControl".Localized());

                if (Player.Lust >= 33)
                    OutputText("Scenes.Infestedchameleongirlscene.DefeatChameleonGirl.ConsiderHelping".Localized());
            }
            else
            {
                OutputText("Scenes.Infestedchameleongirlscene.DefeatChameleonGirl.TooWeakFight".Localized());

                if (Player.Lust >= 33)
                    OutputText("Scenes.Infestedchameleongirlscene.DefeatChameleonGirl.ConsiderUsing".Localized());
            }

            if (Player.Lust < 33 || Player.Gender == Gender.Genderless)
            {
                CombatScene.CleanupAfterCombat();

                return ;
            }

            AddButton(0, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

    }
}