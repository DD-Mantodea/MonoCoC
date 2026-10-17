using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Bog
{
    [Register<ChameleonGirlScene>("coc:chameleon_girl")]
    public class ChameleonGirlScene : Scene
    {

        public override void Entry()
        {

            CurrentMonster = EnemyRegistry.Create(EnemyIds.ChameleonGirl);

            EncounterChameleon();
        }

        public void EncounterChameleon()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMetChameleon) == 0)
            {
                OutputText("Scenes.Chameleongirlscene.EncounterChameleon.WorksThrough".Localized());

                OutputText("Scenes.Chameleongirlscene.EncounterChameleon.RespiteDisturbed".Localized(
                     CurrentMonster.Body.Get<SkinPart>().Adj
                ));

                OutputText("Scenes.Chameleongirlscene.EncounterChameleon.AfterFewMoments".Localized());

                if (Player.Tallness > 76)
                    OutputText("Scenes.Chameleongirlscene.EncounterChameleon.MuchShorter".Localized());

                OutputText("Scenes.Chameleongirlscene.EncounterChameleon.ChucklesDisplay".Localized());

                OutputText("Scenes.Chameleongirlscene.EncounterChameleon.FightingChameleon".Localized());
            }
            else
            {
                OutputText("Scenes.Chameleongirlscene.EncounterChameleon.WorksThroughAgain".Localized());

                OutputText("Scenes.Chameleongirlscene.EncounterChameleon.SuddenScrape".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesMetChameleon, CoC.Instance.Flags.Get(GameFlagIds.TimesMetChameleon) + 1);

            CombatScene.Start(EnemyRegistry.Create(EnemyIds.ChameleonGirl), () => Go<CampScene>());
        }

        public void LoseToChameleonGirl()
        {
            ClearOutput();

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Chameleongirlscene.LoseToChameleonGirl.LosingControl".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.LoseToChameleonGirl.TooWeakFight".Localized());

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
                        OutputText("Scenes.Chameleongirlscene.LoseToChameleonGirl.GirlGives".Localized());

                        Player.HealthChange(-20);

                        CombatScene.CleanupAfterCombat();
                    }
        }

        public void LoseToChameleonWithCockAnBallsAnCunt()
        {

            var x = Player.BiggestCockIndex();

            ShowImage("chameleon-herm-lose-footjob-taildick");

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.HaughtyLaughs".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.CommandsStrip".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.SetsLastBit".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.EyesLocked".Localized());

            Player.CuntChange(8, true, true, false);

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.KeepsPushing".Localized());

            Player.CuntChange(25, true, true, false);

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.GaspsStretched".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.WeightLifts".Localized());

            if (Player.CockArea(x) < 40)
                OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.LiftingIt".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.BlushesWatches".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChameleonWithCockAnBallsAnCunt.FeelingsFilled".Localized(
                 Player.CockDescript(x)
            ));

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void DudesLoseToChamChams()
        {

            var x = Random.Shared.Next(Player.Cocks.Count);

            ShowImage("chameleon-male-lose-facesit-vagsex");

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.RecognizingNoDanger".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.OverwhelmedLust".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.BatteredWord".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.RipBinding".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.ShortWork".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.GripsTree".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.DrippingGrinds".Localized(
                 Player.CockDescript(x),

                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.TeasesHead".Localized());

            if (Player.CockArea(x) <= CombatController.Instance!.Context.Enemy.VaginalCapacity())
                OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.BaseOfShaft".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.AsFarAsCan".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.EyesGoWide".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.NearingOrgasm".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() > 500)
                OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.FillingWithCum".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.PaceSlows".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.SofteningSlidesOut".Localized());

            OutputText("Scenes.Chameleongirlscene.DudesLoseToChamChams.CatchesHerself".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToChamChamWithPCCunt()
        {

            ShowImage("chameleon-female-lose-facesit-tailfuck");

            OutputText("Scenes.Chameleongirlscene.LoseToChamChamWithPCCunt.StalksSwaying".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChamChamWithPCCunt.GrabsHips".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChamChamWithPCCunt.TurningAttention".Localized());

            OutputText("Scenes.Chameleongirlscene.LoseToChamChamWithPCCunt.EnjoysIt".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Chameleongirlscene.LoseToChamChamWithPCCunt.MoansInsistent".Localized(
                 Player.VaginaDescript(0)
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

            Player.Lust = Player.MaxLust;

            CombatScene.CleanupAfterCombat();
        }

        public void DefeatChameleonGirl()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
            {
                OutputText("Scenes.Chameleongirlscene.DefeatChameleonGirl.UnableControl".Localized());

                if (Player.Lust >= 33)
                    OutputText("Scenes.Chameleongirlscene.DefeatChameleonGirl.HelpHornyQuestion".Localized());
            }
            else
            {
                OutputText("Scenes.Chameleongirlscene.DefeatChameleonGirl.TooWeakContinue".Localized());

                if (Player.Lust >= 33)
                    OutputText("Scenes.Chameleongirlscene.DefeatChameleonGirl.UseGirlQuestion".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.UseDick".Localized(), null);

            AddButton(1, "Common.Button.UsePussy".Localized(), null);

            AddButton(2, "Common.Button.HermStyle".Localized(), null);

            AddButton(3, "Common.Button.SuccMilk".Localized(), null);

            AddButton(4, "Common.Button.Lustnsensd".Localized(), null);

            if (Player.Lust >= 33 && !Player.IsGenderless())
            {
                if (Player.HasCock())
                    AddButton(0, "Common.Button.UseDick".Localized(), ManFucksChameleonWithBiggishWang);

                if (Player.HasVagina())
                    AddButton(1, "Common.Button.UsePussy".Localized(), FemaleHasWinSexWithChamCham);

                if (Player.IsHerm())
                    AddButton(2, "Common.Button.HermStyle".Localized(), FuckDatChameleonAsACoolGuyGirlHerm);

                if ((Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk))) && Player.HasCock())
                    AddButton(3, "Common.Button.SuccMilk".Localized(), GiveTheChameleonASuccubiMilk);

                if ((Player.HasItem(ItemRegistry.Create(ConsumableIds.SensitivityDraft)) && Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft))) || Player.HasItem(ItemRegistry.Create(ConsumableIds.FuckDraft)))
                    AddButton(4, "Common.Button.Lustnsensd".Localized(), DoseDatChameleonWithLustAndSensitivityDrafts);
            }

            if (Player.HasMultiTails())
                AddNextButton("Common.Button.ForceFluff".Localized(), () => SceneInstance<Forest.KitsuneScene>.Get().KitsuneGenericFluff());

            AddButton(14, "Common.Button.Leave".Localized(), CombatScene.CleanupAfterCombat);
        }

        public void FuckDatChameleonAsACoolGuyGirlHerm()
        {
            ClearOutput();

            int x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            ShowImage("chameleon-herm-win-tailjob-handjob");

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.ShedsArmor".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.TakesNipple".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.PlaysAlong".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.NotPrefix".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.OneToBeSelfish".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.QuiteSelfish".Localized());

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.RemovesArm".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.SpendingWhile".Localized());

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.TwoSlowDown".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.AndGuiltily".Localized());

            OutputText("Scenes.Chameleongirlscene.FuckDatChameleonAsACoolGuyGirlHerm.RollHerBack".Localized());

            Player.Orgasm("Generic");

            CombatScene.CleanupAfterCombat();
        }

        public void ManFucksChameleonWithBiggishWang()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            ShowImage("chameleon-male-win-get-blowjob");

            OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.ApproachesFallen".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.MasturbatingFuriously".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.StartsFingers".Localized());

            OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.QuickJerk".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.DropsPreCum".Localized());

            OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.SensingLoosens".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.BallsClench".Localized());

            OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.SemenErupts".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CumQ() > 750)
                OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.TorrentBursting".Localized());

            OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.OrgasmEnds".Localized());

            if (Player.CumQ() > 750)
                OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.MurkyWater".Localized());

            OutputText("Scenes.Chameleongirlscene.ManFucksChameleonWithBiggishWang.PutsBack".Localized());

            Player.Orgasm("Dick");

            CombatScene.CleanupAfterCombat();
        }

        public void FemaleHasWinSexWithChamCham()
        {
            ClearOutput();

            ShowImage("chameleon-female-win-fingering");

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.ShedsArmor".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.MoansGrowing".Localized());

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.LooksUpDown".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.CoosLustily".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.EyesWiden".Localized());

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.HandReachesSilk".Localized());

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.TakesNipple".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.NotPrefix".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.OneToBeSelfish".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.QuiteSelfish".Localized());

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.RemovesArm".Localized(
                 Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.SpendingWhile".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.SomewhatGuiltily".Localized());

            OutputText("Scenes.Chameleongirlscene.FemaleHasWinSexWithChamCham.RollHerBack".Localized());

            Player.Orgasm("Vaginal");

            CombatScene.CleanupAfterCombat();
        }

        public void GiveTheChameleonASuccubiMilk()
        {
            ClearOutput();

            ShowImage("chameleon-male-win-succumilk-titfuck");

            OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.FindsBottle".Localized());

            OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.ContainerStride".Localized());

            OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.NothingHappens".Localized());

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.LooksBackUp".Localized());

            if (Player.HasLongTail())
                OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.WithTail".Localized());
            else
                OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.ByStraddling".Localized());

            OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.CockAimed".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.PoorResistance".Localized());

            OutputText("Scenes.Chameleongirlscene.GiveTheChameleonASuccubiMilk.DespiteInexperience".Localized());

            Player.Orgasm("Dick");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));
            else
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));

                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));
            }

            CombatScene.CleanupAfterCombat();
        }

        public void DoseDatChameleonWithLustAndSensitivityDrafts()
        {
            ClearOutput();

            OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.DigsBags".Localized());

            OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.EyesWidenShock".Localized());

            OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.TooTurnedOn".Localized());

            OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.WorriedManage".Localized());

            OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.PushesFurther".Localized());

            OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.KeepsGoingLeg".Localized());

            if (!Player.HasCock() || Player.CockArea(Player.SmallestCockIndex()) > CombatController.Instance!.Context.Enemy.VaginalCapacity())
            {
                ShowImage("chameleon-male-win-fingering");

                OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.ReachesThong".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 30f));
            }
            else
            {
                var x = Player.CockThatFits(CombatController.Instance!.Context.Enemy.VaginalCapacity());

                if (x < 0)
                {
                    OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.CockError".Localized());

                    CombatScene.CleanupAfterCombat();

                    return ;
                }

                ShowImage("chameleon-male-win-vagsex");

                OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.TugsKnot".Localized(
                     Player.CockDescript(x)
                ));

                OutputText("Scenes.Chameleongirlscene.DoseDatChameleonWithLustAndSensitivityDrafts.TracesFinger".Localized(
                     Player.CockDescript(x),

                     Player.CockDescript(x)
                ));

                Player.Orgasm("Generic");

                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));
            }

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SensitivityDraft));

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));
            else
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.FuckDraft));

            CombatScene.CleanupAfterCombat();
        }

    }
}