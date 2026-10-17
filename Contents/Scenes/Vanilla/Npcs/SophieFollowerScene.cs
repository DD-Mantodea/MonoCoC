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
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Places;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<SophieFollowerScene>("coc:sophie_follower")]
    public class SophieFollowerScene : Scene
    {

        public static readonly string[] eggColors = ["Black", "Blue", "Brown", "Pink", "Purple", "White"];

        public bool pregnancy => SceneInstance<SophieScene>.Get().Pregnancy.IsPregnant;

        public override void Entry()
        {
            SophieFollower();
        }

        public bool SophieFollower()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) > 0)
                return false;

            if (SceneInstance<SophieBimboScene>.Get().BimboSophie())
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieDebimboed) > 0)
                return true;

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieRecruitedPure) > 0)
                return true;

            return false;
        }

        public bool SophieFollowerAtCamp()
        {

            return SophieFollower() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0;
        }

        public void SophieSprite()
        {

            SceneInstance<SophieBimboScene>.Get().SophieSprite();
        }

        public void UnbimboSophie()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.UnbimboSophie.ConsiderGivingPotion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), YesDebimboSophie);

            AddButton(1, "Common.Button.No".Localized(), NoDontDebimbo);
        }

        public void NoDontDebimbo()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.NoDontDebimbo.PutPotionBack".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => SceneInstance<SophieBimboScene>.Get().ApproachBimboSophieInCamp());
        }

        public void YesDebimboSophie()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.DeBimbo));

            OutputText("Scenes.Sophiefollowerscene.YesDebimboSophie.GrabAirHeadedHarpy".Localized());

            OutputText("Scenes.Sophiefollowerscene.YesDebimboSophie.EwTotallyNotBooze".Localized());

            OutputText("Scenes.Sophiefollowerscene.YesDebimboSophie.WhatDidYouDo".Localized(
                Player.Mf("bastard", "bitch")
            ));

            OutputText("Scenes.Sophiefollowerscene.YesDebimboSophie.RecoilAtOutburst".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SophieDebimboed, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) > 0)
                CoC.Instance.Flags.Set(GameFlagIds.SophiesDaughtersDebimboed, 1);

            ClearButtons();

            AddButton(0, "Common.Button.BeatHer".Localized(), BeatSophieAroundYouMonster);

            if (Player.Intelligence > 30)
            {
                AddButton(1, "Common.Button.Apologize".Localized(), ApologizeToDebimboSophie);

                AddButton(2, "Common.Button.Whyididit".Localized(), WhyIDidItToDebimboSophie);
            }

            AddButton(3, "Scenes.Aiko.YamataLoses.ButtonLetHerGo".Localized(), LetDebimboSophieGo);

            if (Player.HasItem(ItemIds.BimboLiqueur))
                AddButton(4, "Common.Button.BimboAgain".Localized(), BimboSophieAgain);
        }

        public void BimboSophieAgain()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.BimboSophieAgain.LungeWithBimboPotion".Localized());

            OutputText("Scenes.Sophiefollowerscene.BimboSophieAgain.BabeSheAsks".Localized());

            OutputText("Scenes.Sophiefollowerscene.BimboSophieAgain.CrisisAverted".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SophieDebimboed, 0);

            CoC.Instance.Flags.Set(GameFlagIds.SophiesDaughtersDebimboed, 0);

            Player.ConsumeItem(ItemIds.BimboLiqueur);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                SceneInstance<SophieBimboScene>.Get().ApproachBimboSophieInCamp(false);
        }

        public void BeatSophieAroundYouMonster()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.BeatSophieAroundYouMonster.ReadyYourWeapon".Localized());

            OutputText("Scenes.Sophiefollowerscene.BeatSophieAroundYouMonster.GonnaMakeYouPay".Localized());

            var soph = EnemyRegistry.Create(new Identifier("coc", "sophie"));

            if (soph is SophieEnemy sophie)
                sophie.BimboBrawl = true;

            soph.OnDefeated = (hpVictory) => BeatUpDebimboSophie(hpVictory);

            soph.OnWon = (hpVictory) => DebimboSophieBeatsYouUp(hpVictory);

            soph.CreateStatusEffect(StatusEffectIds.GenericRunDisabled, 0, 0, 0, 0);

            CombatScene.Start(soph, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void BeatUpDebimboSophie(bool hpVictory)
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.BeatUpDebimboSophie.SophieStumblesBack".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Apologize".Localized(), ApologizeToDebimboSophie);

            AddButton(2, "Common.Button.Whyididit".Localized(), WhyIDidItToDebimboSophie);

            AddButton(3, "Scenes.Aiko.YamataLoses.ButtonLetHerGo".Localized(), LetDebimboSophieGo);

            if (Player.HasItem(ItemIds.BimboLiqueur))
                AddButton(4, "Common.Button.BimboAgain".Localized(), BimboSophieAgain);
        }

        public void DebimboSophieBeatsYouUp(bool hpVictory, bool pcCameWorms = false)
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.DebimboSophieBeatsYouUp.FallBackwards".Localized());

            OutputText("Scenes.Sophiefollowerscene.DebimboSophieBeatsYouUp.SophieCries".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Apologize".Localized(), ApologizeToDebimboSophie);

            AddButton(2, "Common.Button.Whyididit".Localized(), WhyIDidItToDebimboSophie);

            AddButton(3, "Scenes.Aiko.YamataLoses.ButtonLetHerGo".Localized(), LetDebimboSophieGo);
        }

        public void LetDebimboSophieGo()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.LetSophieGo".Localized());

            OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.MaybeSeeHerAgain".Localized());

            OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.WillSheGetBack".Localized());

            if (Player.Libido >= 50)
            {
                OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.ThoughtsDrift".Localized());

                if (Player.Corruption < 40)
                {
                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.BlinkClearHead".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 10f));
                }
                else
                {
                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.LetFantasiesRunWild".Localized());

                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.PheromonalScent".Localized());

                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.MinotaurBellows".Localized());

                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.TwoRemainingMinotaurs".Localized());

                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.VisionGrowsDim".Localized());

                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.VictoriousMinotaur".Localized());

                    OutputText("Scenes.Sophiefollowerscene.LetDebimboSophieGo.SmilePerversely".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 25f));
                }
            }

            CoC.Instance.Flags.Set(GameFlagIds.SophieDisabledForever, 1);

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void WhyIDidItToDebimboSophie()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.ExplainYourselfGently".Localized());

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.LongMomentOfSilence".Localized());

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.WhatQuestion".Localized());

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.JustLookAtMe".Localized());

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.WryChuckle".Localized());

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.SetUpNest".Localized());

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.MakeYourselfAtHome".Localized());

            OutputText("Scenes.Sophiefollowerscene.WhyIDidItToDebimboSophie.SophieMovedToFollowers".Localized());

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void ApologizeToDebimboSophie()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.ApologizeToSophie".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.ListensSilently".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.LongMomentOfSilence".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.WhatQuestion".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.JustLookAtMe".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.WryChuckle".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.SetUpNest".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.MakeYourselfAtHome".Localized());

            OutputText("Scenes.Sophiefollowerscene.ApologizeToDebimboSophie.SophieMovedToFollowers".Localized());

            if (CombatController.InCombat)
                CombatScene.CleanupAfterCombat();
            else
                ReturnToCamp(1);
        }

        public void SophieDaughterDebimboUpdate()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 1)
                OutputText("Scenes.Sophiefollowerscene.SophieDaughterDebimboUpdate.OneDaughterCured".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) == 2)
                    OutputText("Scenes.Sophiefollowerscene.SophieDaughterDebimboUpdate.TwoDaughtersCured".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieDaughterDebimboUpdate.AllDaughtersCured".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SophiesDaughtersDebimboed, 0);
        }

        public void FollowerSophieMainScreen()
        {
            ClearOutput();

            SceneInstance<SophieBimboScene>.Get().SophieSprite();

            if (SceneInstance<SophieBimboScene>.Get().SophieIsInSeason() && Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
            {
                OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.SoftBodyJiggles".Localized());

                OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.HandTravelsDown".Localized());

                if (SceneInstance<SophieBimboScene>.Get().BimboSophie())
                    OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.SkilledAtItAgain".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.SkilledAtIt".Localized());

                OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.GiveItToMe".Localized());

                OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.NoWaySheAccepts".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

                ClearButtons();

                AddButton(0, "Common.Button.Appearance".Localized(), SophieAppearance);

                if (Player.HasCock())
                {
                    if (Player.HasCockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity()))
                    {
                        AddButton(1, "Common.Button.Vaginal".Localized(), FuckFollowerSophie);

                        AddButton(2, "Common.Button.Special".Localized(), SophieSpecial);
                    }
                    else
                        OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.TooBigToFit".Localized());
                }
                else
                    OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.NeedPenisToFuck".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Sophie")
                {
                    AddButton(8, "Common.Button.Nosleepwith".Localized(), SleepWithSophieToggle);

                    OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.SharingBedNoCock".Localized());
                }

                AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));

                return;
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                        OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.LeansBackFCups".Localized(
                            Player.Mf("handsome", "cutey")
                        ));
                    else
                        OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.DdCupsOnDisplay".Localized(
                            Player.Mf("handsome", "cutey")
                        ));
                }
                else
                {
                    OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.WhenYouCall".Localized());

                    OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.SheCoos".Localized());
                }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), SophieAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), SophieTalkMenu);

            AddButton(2, "Common.Button.Sex".Localized(), SexWithFollowerSophie);

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieCampEggCountdown) > 0 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
                OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.SophieEggSitting".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0)
                AddButton(5, "Common.Button.Daughter".Localized(), SceneInstance<SophieBimboScene>.Get().DaughterCheckup);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
                AddButton(9, "Common.Button.SleepWith".Localized(), SleepWithSophieToggle);

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Sophie" && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
            {
                AddButton(9, "Common.Button.SleepAlone".Localized(), SleepWithSophieToggle);

                OutputText("Scenes.Sophiefollowerscene.FollowerSophieMainScreen.SharingBedMainScreen".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                AddButton(10, "Common.Button.FarmWork".Localized(), SendToFarm);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 1)
                AddButton(10, "Common.Button.GoCamp".Localized(), BackToCamp);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionSophie) == 0)
                AddButton(11, "Common.Button.HarvestEggs".Localized(), HarvestEggs);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionSophie) == 1)
            {
                AddButton(11, "Common.Button.ChangeEggs".Localized(), ChangeEggs);

                AddButton(12, "Common.Button.StopHarvest".Localized(), StopHarvest);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmSophie) == 0)
                AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
            else
                AddButton(14, "Common.Button.Back".Localized(), null);
        }

        public void SendToFarm()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiefollowerscene.SendToFarm.HeadTowardsLake".Localized());

            OutputText("Scenes.Sophiefollowerscene.SendToFarm.NotTillSomeFarm".Localized());

            OutputText("Scenes.Sophiefollowerscene.SendToFarm.WhiningLongAfter".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmSophie, 1);

            ReturnToCamp(1);
        }

        public void BackToCamp()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiefollowerscene.BackToCamp.WelcomeBackToCamp".Localized());

            OutputText("Scenes.Sophiefollowerscene.BackToCamp.ThankTheGods".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmSophie, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void HarvestEggs()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiefollowerscene.HarvestEggs.ProduceCertainColor".Localized());

            OutputText("Scenes.Sophiefollowerscene.HarvestEggs.UsingLakeMagic".Localized());

            EggSelector();
        }

        public void EggSelector()
        {

            var i = 0;

            while (i < eggColors.Length)
            {
                AddButton(i, eggColors[i], () => PostEggSelector(eggColors[i]));

                i += 1;
            }
        }

        public void PostEggSelector(string selected)
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiefollowerscene.PostEggSelector.NiceEggsForYou".Localized(
                selected.ToLower()
            ));

            CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionSophie, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerProductionSophieColorchoice) != selected)
            {
                CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionSophieColorchoice, selected);

                CoC.Instance.Flags.Set(GameFlagIds.FarmEggStored, 0);

                CoC.Instance.Flags.Set(GameFlagIds.FarmEggCountdown, 7);
            }

            DoNext(FarmCorruption.RootScene);
        }

        public void StopHarvest()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiefollowerscene.StopHarvest.StopGivingEggs".Localized());

            OutputText("Scenes.Sophiefollowerscene.StopHarvest.CouldntYouFuckReal".Localized());

            OutputText("Scenes.Sophiefollowerscene.StopHarvest.NegativeEarnsPout".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerProductionSophie, 0);

            DoNext(FarmCorruption.RootScene);
        }

        public void ChangeEggs()
        {
            ClearOutput();

            SophieSprite();

            OutputText("Scenes.Sophiefollowerscene.ChangeEggs.ChangeEggType".Localized());

            OutputText("Scenes.Sophiefollowerscene.ChangeEggs.PreferFuckingMe".Localized());

            EggSelector();
        }

        public void SexWithFollowerSophie()
        {
            ClearOutput();

            if (!Player.HasCock())
            {
                OutputText("Scenes.Sophiefollowerscene.SexWithFollowerSophie.NotEquippedForThat".Localized(
                    Player.Mf("dickless boys", "girls")
                ));

                OutputText("Scenes.Sophiefollowerscene.SexWithFollowerSophie.WhatsSoBad".Localized());

                OutputText("Scenes.Sophiefollowerscene.SexWithFollowerSophie.DontLeaveBecause".Localized());

                OutputText("Scenes.Sophiefollowerscene.SexWithFollowerSophie.WellReasoned".Localized());

                FollowerSophieMainScreen();

                return;
            }

            ClearButtons();

            OutputText("Scenes.Sophiefollowerscene.SexWithFollowerSophie.HopingForATaste".Localized());

            if (Player.HasCock())
            {
                if (Player.HasCockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity()))
                    AddButton(0, "Common.Button.Vaginal".Localized(), FuckFollowerSophie);

                AddButton(1, "Scenes.Dullahan.AskDullSex.ButtonBlowjob".Localized(), SophieFollowerGivesBlowjobs);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieFamilyIncest) > 0 && Player.HasCockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity()))
                AddButton(8, "Common.Button.Daughterfuck".Localized(), SophieIncestInHerCooterOrSomethingIDunno);

            AddButton(14, "Common.Button.Back".Localized(), FollowerSophieMainScreen);
        }

        public void SophieAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieAppearance.BigBuxomHarpy".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieAppearance.VeryWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieAppearance.LongWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieAppearance.PlatinumBlond".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieAppearance.PinkWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieAppearance.HairDanglesShoulders".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieAppearance.SuperSizedWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieAppearance.LipsAtAllTimes".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieAppearance.SophiesBreastsAre".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieAppearance.SoImmenseDragHer".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieAppearance.UnusuallyLarge".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieAppearance.GlitteringMoistPussy".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieAppearance.ArmsCoveredInFeathers".Localized());

            if (FarmCorruption.hasTattoo("sophie"))
            {
                OutputText("Scenes.Sophiefollowerscene.SophieAppearance.ParagraphBreak".Localized());

                if (FarmCorruption.sophieFullTribalTats())
                    OutputText("Scenes.Sophiefollowerscene.SophieAppearance.TribalTattoos".Localized());
                else
                {
                    if (FarmCorruption.numTattoos("sophie") > 1)
                        OutputText("Scenes.Sophiefollowerscene.SophieAppearance.FollowingTattoos".Localized());
                    else
                        OutputText("Scenes.Sophiefollowerscene.SophieAppearance.SheHasWord".Localized());

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooCollarbone))
                        OutputText("Scenes.Sophiefollowerscene.SophieAppearance.TattooCollarbone".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooCollarbone)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooShoulders))
                        OutputText("Scenes.Sophiefollowerscene.SophieAppearance.TattooShoulders".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooShoulders)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooLowerback))
                        OutputText("Scenes.Sophiefollowerscene.SophieAppearance.TattooLowerBack".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooLowerback)
                        ));

                    if (CoC.Instance.Flags.Has(GameFlagIds.SophieTattooButt))
                        OutputText("Scenes.Sophiefollowerscene.SophieAppearance.TattooButt".Localized(
                            CoC.Instance.Flags.Get(GameFlagIds.SophieTattooButt)
                        ));
                }
            }

            FollowerSophieMainScreen();
        }

        public void SophieTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMenu.WhatDoYouAsk".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Harpies".Localized(), SophieTalkHarpies);

            AddNextButton("Common.Button.Love".Localized(), SophieTalkLove);

            AddNextButton("Common.Button.Motherhood".Localized(), SophieTalkMotherhood);

            AddNextButton("Common.Button.Mareth".Localized(), SophieTalkMareth);

            AddButton(14, "Common.Button.Back".Localized(), () => FollowerSophieMainScreen());
        }

        public void SophieTalkHarpies()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieTalkHarpies.MovedInWithYou".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkHarpies.ThoseDivas".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkHarpies.CompanyNotBest".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkHarpies.AirsBitNicer".Localized());

            SophieTalkMenu();
        }

        public void SophieTalkLove()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieTalkLove.TouchySubject".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkLove.WantMeToSay".Localized(
                CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) + CoC.Instance.Flags.Get(GameFlagIds.SophieDaughterMaturityCounter) > 0 ? "we even have " + (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) != 0 ? "children" : "a kid") + " here together" : "I hope you're not calling it quits there"
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieTalkLove.EncouragingButVague".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkLove.MoreThanEnoughMan".Localized());

            SophieTalkMenu();
        }

        public void SophieTalkMotherhood()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMotherhood.SoMuchInteraction".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMotherhood.MatronlySurprised".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMotherhood.RubbingHerBelly".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMotherhood.LipPurses".Localized());

            SophieTalkMenu();
        }

        public void SophieTalkMareth()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMareth.FairlyUnfazed".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMareth.DontThinkOften".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMareth.AllTheBreeder".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieTalkMareth.ThighsGrindTogether".Localized());

            SophieTalkMenu();
        }

        public void SleepWithSophieToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "Sophie")
            {
                OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.AskToSleepTogether".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                {
                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.FirstBimboNowThis".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.YouWord".Localized());

                    if (Player.Corruption < 33)
                        OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.SheepishlyWord".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.SnuggleAnswer".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.MatronSmiles".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.SeeYouTonight".Localized());

                    if (Player.SmallestCockArea() <= 5 && Player.HasCock())
                        OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.DontBeSurprised".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.SheCallsWord".Localized());

                    if (Player.SmallestCockArea() <= 5 && Player.HasCock())
                        OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.TellStopTeasing".Localized());
                }
                else
                {
                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.SmilesRadiantly".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.SeparatesWithSmile".Localized());

                    if (Player.SmallestCockArea() <= 5 && Player.HasCock())
                        OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.CuteLittleDick".Localized());
                }

                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Sophie");
            }
            else
            {
                OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.RatherNotSleep".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) == 0)
                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.WellItsYourBed".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SleepWithSophieToggle.DontFretMomma".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "");
            }

            FollowerSophieMainScreen();
        }

        public void FuckFollowerSophie()
        {
            ClearOutput();

            var x = Player.CockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = Player.CockThatFits2(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (SceneInstance<SophieBimboScene>.Get().SophieIsInSeason())
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.FeatheryEmbrace".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.SmirksAsSheSaunters".Localized(
                    Player.Mf("man", "'man'")
                ));

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.StripOutOfArmor".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 5)
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.GirlishMember".Localized(
                    Player.Mf("I thought you were a man, but that looks like a little girl-clit!", "I know you're a herm, but maybe you should just turn that little thing into a proper clit!")
                ));
            else
                if (Player.CockArea(x) <= SceneInstance<SophieBimboScene>.Get().SophieCapacity() / 2)
                    OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.NotThatImpressive".Localized(
                        Player.CockDescript(x)
                    ));
                else
                    OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.BigFatCock".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.AppreciativeSwat".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.ProwlForward".Localized(
                Player.CockDescript(x)
            ));

            if (SceneInstance<SophieBimboScene>.Get().SophieIsInSeason())
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.GushingSpermHungry".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.SlipperyWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.MoanOutLoad".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.WingTipsFlutter".Localized());

            if (Player.Intelligence < 50)
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.AbruptlyWord".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.EvenThoughYouKnow".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.HerWordsStillAffect".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.BeginsToGrunt".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.FuckItWord".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.FingerMyAssholeCommand".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.WhileIFuckYou".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.ShiftToBringUp".Localized(
                    Player.CockDescript(y)
                ));
            else
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.ThumbAtAsshole".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.SqueezeTighter".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.HardTimeFocusing".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.SqueezesHerTits".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.ShudderHeadToToe".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.AndAnalWalls".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.FingersDigIn".Localized());

            if (y >= 0)
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.PackIntestines".Localized());

            if (SceneInstance<SophieBimboScene>.Get().SophieIsInSeason())
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.PussyDrinksDeposit".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.BodySlacksOver".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.SophiesBelly".Localized());

                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.HasBegunToInflate".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.InflatedAbsurdDegree".Localized());

                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.DueToAllSpooge".Localized());

                if (Player.CumQ() >= 4000)
                    OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.StillGrowingLarger".Localized());

                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.SentencePeriod".Localized());
            }

            if (Player.CumQ() >= 5000 || (Player.CumQ() >= 500 && SceneInstance<SophieBimboScene>.Get().SophieIsInSeason()))
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.HugeWashOfWhite".Localized());

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.ThenYouBlackOut".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
                OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.AndWord".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Sophiefollowerscene.FuckFollowerSophie.StillPulsingEcstasy".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            if (SceneInstance<SophieBimboScene>.Get().SophieIsInSeason())
                SceneInstance<SophieBimboScene>.Get().SophiePregChance();

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SophieVagFollowerFollowup);
        }

        public void SophieVagFollowerFollowup()
        {
            ClearOutput();

            var x = Player.CockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Sophiefollowerscene.SophieVagFollowerFollowup.RousedBySmilingSophie".Localized());

            if (SceneInstance<SophieBimboScene>.Get().SophieIsInSeason())
                OutputText("Scenes.Sophiefollowerscene.SophieVagFollowerFollowup.IfThatDidntTake".Localized());
            else
                if (Player.CockArea(x) <= 5)
                    OutputText("Scenes.Sophiefollowerscene.SophieVagFollowerFollowup.NotBadGirlDick".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieVagFollowerFollowup.BetterWhenYouWait".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieVagFollowerFollowup.SnarkyReply".Localized());

            ReturnToCamp(1);
        }

        public void SophieFollowerGivesBlowjobs()
        {
            ClearOutput();

            var x = Player.CockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.WannaFeelLips".Localized());

            if (Player.CockArea(x) <= 5)
                OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.TinyClitWord".Localized());
            else
                if (Player.CockArea(x) <= SceneInstance<SophieBimboScene>.Get().SophieCapacity() / 2)
                    OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.NeedyBoyCock".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.OverswollenEngorgement".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.SauntersHipsSwaying".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.RightBetweenYour".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.TrailsUpToChin".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.BimboBodyInsatiable".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.SeducedMeIntoCamp".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerGivesBlowjobs.BlowsYouAKiss".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.ForceHer".Localized(), ForceSophieBlowjob);

            AddButton(1, "Common.Button.Sixtynine".Localized(), SophieBlowsSixtyNine);
        }

        public void SophieBlowsSixtyNine()
        {
            ClearOutput();

            var x = Player.CockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.NodEagerly".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.FewLicksOnTwat".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.CockAnEyebrow".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.SimultaneousEnsnarement".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.PluralS".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ThatWord".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.AreWord".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.IsWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.MountingHigherAndHigher".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.SearchingForTarget".Localized(
                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.AffectionateSqueeze".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ExhalesOnIt".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.OralEndowment".Localized());

            if (BodyPartTypes.TongueIndex(Player.Body.Get<TonguePart>().Id) > BodyPartTypes.TongueHuman && BodyPartTypes.TongueIndex(Player.Body.Get<TonguePart>().Id) <= BodyPartTypes.TongueDraconic)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.LongerAndLonger".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.TongueSlidesIn".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ThighsSqueezeTighter".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.HerTongueAndYours".Localized(
                Player.CockDescript(x)
            ));

            if (Player.HasVagina())
            {
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.SheTenderlyWord".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ReachesUnderBalls".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ExploresVagina".Localized());
            }
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.CaressesSack".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.StrokesLongNail".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.FeatheryHairDangles".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.HumsHappily".Localized(
                Player.CockDescript(x),

                Player.Mf("king", "breeder-queen")
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.AnsweringCryOfPleasure".Localized());

            if (Player.CockArea(x) <= 60)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ToTheSheath".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.FarAsSheCan".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ThighsBeginToTremble".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.TwoHumsOfDelight".Localized());

            if (Player.CumQ() >= 1000 || Player.CockTotal() > 1)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.WarmGooCovers".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.GivesUpSwallowing".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.CumQ() >= 3000)
                OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.BigWhitePuddle".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.ShakesFewMore".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieBlowsSixtyNine.YoudGetOffHarder".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerIrritation, 0);

            Player.SlimeFeed();

            SceneInstance<SophieScene>.Get().LuststickApplication(8);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ReturnToCamp(1);
        }

        public void ForceSophieBlowjob()
        {
            ClearOutput();

            var x = Player.CockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            CoC.Instance.Flags.Set(GameFlagIds.SophieFollowerIrritation, CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerIrritation) + 1);

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.PushDownOnKnees".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.SophieGrumbles".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.FondlingYourBalls".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.RubbingYourLength".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.DontMakeAHabit".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.ImpatientFingers".Localized(
                Player.CockHead(x),

                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 60)
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.FarAsCanGo".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.WholeWayToBase".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.SentencePeriod".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.SnugGoldLips".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockTotal() == 2)
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.ExtraCockFlops".Localized());
            else
                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.ExtraCocksFlop".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.GrabsOwnTits".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.BallsFeelTaut".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.ItFeelsTight".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.AllOverHarpysFace".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.SlobberAndDrool".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.YourSackAndOver".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.LegsSophiesEyesRoll".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.CompressesCockPillows".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            if (Player.HasKnot(x))
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.InflatesBehindLips".Localized(
                    Player.KnotWord(x)
                ));

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.SemenJetsUrethra".Localized());

            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.GurglesMidriffBulges".Localized());

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.JustTheBeginning".Localized());

                if (Player.CumQ() >= 4000)
                    OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.ThenCumSomeMore".Localized());

                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.SentencePeriodAgain".Localized());
            }

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.AlmostRegretfullyWord".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.PullAgainstCockLocked".Localized(
                    Player.KnotWord(x),

                    Player.CockDescript(x)
                ));
            else
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.SlipSpasmingBoner".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.HarpyBlushesUp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerIrritation) == 1)
                OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.NotAsMuchReciprocate".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerIrritation) < 4)
                    OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.DontKnowHowMany".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerIrritation) < 8)
                        OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.EverGoingToReciprocate".Localized());
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.SophieFollowerIrritation) == 8)
                            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.HadEnoughOfThis".Localized());
                        else
                            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.MightLeaveYou".Localized());

            OutputText("Scenes.Sophiefollowerscene.ForceSophieBlowjob.GetDressedSelfSatisfied".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            ReturnToCamp(1);
        }

        public void SophieSixtyNine()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieSixtyNine.SapphicDelightQuestion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), SleepWithSophieToggle);
        }

        public void SophieSmallDongTeases()
        {

            var x = Player.SmallestCockIndex();

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.WakingUpHeading".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.TinglyWarmthEnvelops".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.AndWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.LavishingSlickness".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.ErectionRemoved".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Strength >= 70)
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.CouldForceHerOff".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.FluffyBehindCrashes".Localized());

            if (Player.BreastRows[0].BreastRating >= 1)
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.SqueezesBreast".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.TalonsDigIn".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.LicksShiningLips".Localized(
                Player.Mf("-boy", "y-girl")
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.ShakeHeadDenying".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesMorningSophieFemdommed) > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.BackOfYourMind".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.BackOfMindAgain".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.PlantsKiss".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.ChecksHerNails".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.BirdWomanExaggerates".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.CatchYourselfWondering".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.TittersSmearsWet".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.PullsHerselfUp".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.WarmthDoubles".Localized());

            if (Player.Sensitivity / 5 + Random.Shared.Next(20) + 1 <= 20)
            {
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.ClimaxEludes".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.SlidesHipsCloser".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.NearlyCryingPentUp".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.RubbingHerChin".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.BiteLipAndNod".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0)
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.GoldenWord".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.PinkWord".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.HairedMistressLooks".Localized());

                if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.PregnantWord".Localized());
                else
                    if (Player.Tone >= 70)
                        OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.TonedWord".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.BellyDeflecting".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.MoansAndDivesHand".Localized());

                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.CumSoMuchBoth".Localized());

                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.HugeWetSpots".Localized());
                    else
                        OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.SoakedWaistDown".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.FeelsSoGoodNeither".Localized());
                }

                if (Player.CumQ() >= 2000)
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.BedReflectiveWhite".Localized());
            }
            else
            {
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.WaveOfPleasure".Localized());

                if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 150)
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.RollingOffDome".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.SentencePeriod".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.CumForHours".Localized());

                if (Player.CumQ() >= 1000)
                    OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.PuddleOfJizz".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.AboutEmptySophieSpeaks".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.OnlyAfterOrgasm".Localized());
            }

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.OlderHarpyGiggles".Localized(
                Player.Mf("lady-boys", "girls")
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.LeansDownToHug".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.GetUpShaky".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.HugeWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSmallDongTeases.MessYouMade".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesMorningSophieFemdommed, CoC.Instance.Flags.Get(GameFlagIds.TimesMorningSophieFemdommed) + 1);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, 3f));

            Go<CampScene>();
        }

        public void SophieFollowerGoesIntoSeas()
        {

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerGoesIntoSeas.SeeksAttention".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerGoesIntoSeas.SophieWantsToBreed".Localized());
        }

        public void SophieFertilityExpired()
        {

            OutputText("Scenes.Sophiefollowerscene.SophieFertilityExpired.LostFertilityInterest".Localized());
        }

        public void SophieFertilityKnockedUpExpired()
        {

            OutputText("Scenes.Sophiefollowerscene.SophieFertilityKnockedUpExpired.RubsAbdomenSmirks".Localized());
        }

        public void SophieSpecial()
        {
            ClearOutput();

            var x = Player.CockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (x < 0)
                Player.SmallestCockIndex();

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.WantSpecialTreatment".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SinksToKnees".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.GoldenLipsWrap".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.BallsWaitingToUnload".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SensesItAndChuckles".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 5)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SquirtingClittyWord".Localized());
            else
                if (Player.CockArea(x) <= 30)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.NeedyBoyCock".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.GiantMeatRod".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.CantFitInMouth".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.PushedAndPulled".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.CommandBehindWords".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.AndWord".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.InsideMilkingPassage".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() < 250)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.MilksYouForAll".Localized());
            else
                if (Player.CumQ() < 750)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.WhimpersAtAmount".Localized());
                else
                    if (Player.CumQ() <= 1500)
                        OutputText("Scenes.Sophiefollowerscene.SophieSpecial.CumGushesPuddle".Localized());
                    else
                        OutputText("Scenes.Sophiefollowerscene.SophieSpecial.AssPushedOut".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.GoodBoyObedient".Localized());

            if (Player.CockArea(x) <= 5)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.ClittyWord".Localized());
            else
                if (Player.CockArea(x) <= 30)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.PrettyCock".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.HumongousCuntStretcher".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.CantGoSoft".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.RisesFromCock".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CumQ() < 250)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.PussyDrippingCum".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SemenDrowning".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.TurnsJigglingTits".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.WetPussyLikeHeaven".Localized());

            if (Player.CockArea(x) <= 5)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.DaintyFemCock".Localized());
            else
                if (Player.CockArea(x) <= 30)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SweetCumSquirter".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.ImmenseBeastCock".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.UntilAlmostBroken".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.WithholdFromCommand".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SchlickingVagina".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x),

                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            if (Player.CumQ() < 250)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SeveralRopes".Localized());
            else
                if (Player.CumQ() < 750)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.LargeChaoticStream".Localized());
                else
                    if (Player.CumQ() < 2000)
                        OutputText("Scenes.Sophiefollowerscene.SophieSpecial.WaveAfterWave".Localized());
                    else
                        OutputText("Scenes.Sophiefollowerscene.SophieSpecial.MassiveAmounts".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.OfCumIntoHer".Localized());

            if (Player.CumQ() < 500)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.MoanInSatisfaction".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.GroanCunnyCantHold".Localized());
                else
                    if (Player.CumQ() < 2000)
                        OutputText("Scenes.Sophiefollowerscene.SophieSpecial.StretchAtopOfYou".Localized());
                    else
                        OutputText("Scenes.Sophiefollowerscene.SophieSpecial.EruptCumViolently".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SentencePeriod".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.DefinitelyAMomma".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.SquealUnsure".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.DrowningInLiquid".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.LipsOnYours".Localized());

            if (Player.CockArea(x) <= 5)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.ClittyWordAgain".Localized());
            else
                if (Player.CockArea(x) <= 30)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.PrettyCockAgain".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.HumongousCuntStretcherAgain".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.ReadyToBlowAgain".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.NodDesperately".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.ForcesToSuckNipples".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.EveryLiftTorment".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.AfterAnEternity".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Sophiefollowerscene.SophieSpecial.MeagerFewSquirts".Localized());
            else
                if (Player.CumQ() < 750)
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.ProductionCantKeepUp".Localized());
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieSpecial.InhumanCumFactories".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.ThrashAndMoan".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.IntensityOfHerCunt".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieSpecial.MayNeedRest".Localized());

            Player.Orgasm("Dick");

            if (SceneInstance<SophieBimboScene>.Get().SophieIsInSeason())
                SceneInstance<SophieBimboScene>.Get().SophiePregChance();

            Player.ChangeFatigue(15);

            ReturnToCamp(1);
        }

        public void SophieFollowerIntro()
        {

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerIntro.UnusualMorningHeading".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerIntro.WarmRiverDream".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerIntro.FuckMeHardDarling".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerIntro.SnapBackToReality".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieFollowerIntro.HardlyWhatYouThink".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 10f));

            ClearButtons();

            AddButton(0, "Common.Button.Kisssophie".Localized(), KissSophieRecruitment);

            AddButton(1, "Common.Button.GetOut".Localized(), GetOutSophieRecruitment);
        }

        public void GetOutSophieRecruitment()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.GetOutSophieRecruitment.InvadingCampOverstep".Localized());

            OutputText("Scenes.Sophiefollowerscene.GetOutSophieRecruitment.NeverSpeakAgain".Localized());

            OutputText("Scenes.Sophiefollowerscene.GetOutSophieRecruitment.DartsAboveCamp".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, -5f));

            CoC.Instance.Flags.Set(GameFlagIds.NoPureSophieRecruitment, 1);

            Go<CampScene>();
        }

        public void KissSophieRecruitment()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.ShutHerWithKiss".Localized());

            OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.HoldMotherlyHips".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.SecondSlidesBetween".Localized());

            if (Player.CockTotal() > 3)
                OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.OthersPressAgainst".Localized());

            OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.CriesOutPleasure".Localized());

            OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.RavenousGlans".Localized());

            OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.SlumpBackExhausted".Localized());

            OutputText("Scenes.Sophiefollowerscene.KissSophieRecruitment.CuteSetUpHere".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            ClearButtons();

            AddButton(0, "Common.Button.Sure".Localized(), SophieRecruitmentFinale);

            AddButton(1, "Common.Button.No".Localized(), GetOutSophieRecruitment);
        }

        public void SophieRecruitmentFinale()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.PeckOnCheek".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.GladFeltYoung".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.MilfyMatronLifts".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.BusyChampioning".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.WelcomeToCamp".Localized());

            if (CampScene.CompanionsCount() >= 3)
                OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.IntroduceToOthers".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.SentencePeriod".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieRecruitmentFinale.SophieAddedToFollowers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SophieRecruitedPure, 1);

            Go<CampScene>();
        }

        public void SophieIncestInHerCooterOrSomethingIDunno()
        {
            ClearOutput();

            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.CallOutForDaughter".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount) > 1)
                OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.ScuffleForAttention".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.DaughterComesWillingly".Localized());

            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.CurvyGirlSmiles".Localized());

            int daughterCap = CoC.Instance.Flags.Get(GameFlagIds.SophieAdultKidCount);

            if (daughterCap > 4)
                daughterCap = 4;

            daughterCap = Random.Shared.Next(daughterCap);

            if (daughterCap == 0)
            {
                OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.BustyBimboPoses".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.DaughterOneBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0)
                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.HowDoILook".Localized(
                        Player.Mf("Daddy", "Mommy"),

                        Player.Mf("Daddy", "Mommy")
                    ));
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.LikeMyBreasts".Localized(
                        Player.Mf("Daddy", "Mommy"),

                        Player.Mf("Daddy", "Mommy")
                    ));

                OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.SoftBreastsPressing".Localized());

                OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.UnableToResistNipples".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.DaughterOneBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0)
                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.CantTakeTeasing".Localized(
                        Player.Mf("D-Daddy", "M-Mommy"),

                        Player.Mf("Dad", "Mom")
                    ));
                else
                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.CantTakeTeasingPlease".Localized(
                        Player.Mf("D-Daddy", "M-Mommy"),

                        Player.Mf("Daddy", "Mommy")
                    ));
            }
            else
                if (daughterCap == 1)
                {
                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.HugeAssedDaughterPoses".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.DaughterTwoBimbo) > 0)
                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.BimboWord".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.LipsBlowsAKiss".Localized());

                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.BiteLipAtDisplay".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.DaughterTwoBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0)
                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.PoundMySexyAss".Localized(
                            Player.Mf("Daddy", "Mommy")
                        ));
                    else
                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.TouchMeSpankMe".Localized(
                            Player.Mf("Daddy", "Mommy"),

                            Player.Mf("Daddy", "Mommy")
                        ));

                    OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.SpanksHerAss".Localized());
                }
                else
                    if (daughterCap == 2)
                    {
                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.MoreAverageDaughter".Localized());

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.WarmsYourHeart".Localized());

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.ClapAndApplaud".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.DaughterThreeBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0)
                            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.IDoGoodJob".Localized(
                                Player.Mf("Daddy", "Mommy"),

                                Player.Mf("Daddy", "Mommy")
                            ));
                        else
                            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.WhatDidYouThink".Localized(
                                Player.Mf("Dad", "Mommy")
                            ));

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.WantingToPlease".Localized());

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.SmirkAndGrab".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.UnfurlsWings".Localized());

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.SwingsHipsApproaching".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.DaughterFourBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0)
                            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.SeeHowHot".Localized(
                                Player.Mf("Daddy", "Mommy")
                            ));
                        else
                            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.JustLookAtMe".Localized(
                                Player.Mf("Daddy", "Mommy"),

                                Player.Mf("Daddy", "Mommy")
                            ));

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.HornyBirdgirlLicks".Localized());

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.GrabFeatheryDaughter".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.DaughterFourBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0)
                            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.YouLikeMyButt".Localized(
                                Player.Mf("Daddy", "Mommy")
                            ));
                        else
                            OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.TouchMeAndPlay".Localized(
                                Player.Mf("Daddy", "Mommy")
                            ));

                        OutputText("Scenes.Sophiefollowerscene.SophieIncestInHerCooterOrSomethingIDunno.CoosIntoTabooEar".Localized());
                    }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => PhaseTwoOfIncest(daughterCap));
        }

        public void PhaseTwoOfIncest(int daughter)
        {
            ClearOutput();

            var bimbo = false;

            var x = Player.CockThatFits(SceneInstance<SophieBimboScene>.Get().SophieCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            daughter += 1;

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.PushHerDown".Localized());

            if (daughter == 1)
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.MassivePillowyTits".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.TenderPerkyBreasts".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.FingersSinkIntoFlesh".Localized(
                Player.Mf("Daddy", "Mommy")
            ));

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.RunHandsUpDown".Localized());

            if (daughter == 2)
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.MassiveSpankableAss".Localized());
            else
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.GropableSoftAss".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.GrabHerHard".Localized());

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.ChestFreeOfGear".Localized());

                if (Player.BiggestBreastSize() < 3)
                    OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.PerkUpCoolAir".Localized());
                else
                    if (Player.BiggestBreastSize() < 10)
                        OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.BouncePerkily".Localized());
                    else
                        OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.SpringForthBustiness".Localized());
            }

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.CockFlopsFree".Localized(
                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.DaughterFourBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0)
                bimbo = (((daughter == 4 && CoC.Instance.Flags.Get(GameFlagIds.DaughterFourBimbo) > 0) || (daughter == 3 && CoC.Instance.Flags.Get(GameFlagIds.DaughterThreeBimbo) > 0) || (daughter == 2 && CoC.Instance.Flags.Get(GameFlagIds.DaughterTwoBimbo) > 0) || (daughter == 1 && CoC.Instance.Flags.Get(GameFlagIds.DaughterOneBimbo) > 0)) && CoC.Instance.Flags.Get(GameFlagIds.SophiesDaughtersDebimboed) == 0);

            if (bimbo)
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.FuckMyBrainsOut".Localized(
                    Player.Mf("Daddy", "Mommy")
                ));
            else
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.WantedThisSoBadly".Localized(
                    Player.Mf("Daddy", "Mommy")
                ));

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.WordsDrippingNeed".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.TakeCockInHand".Localized(
                Player.CockDescript(x),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.LeanDownToKiss".Localized());

            if (daughter == 1)
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.LipsPressingSensation".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.EagerlyKissesBack".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.ForceTongueAgainst".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.ClimaxFallsUpon".Localized());

            if (Player.Balls > 0 && Player.BallSize < 8)
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.SeedSwollenBalls".Localized());
            else
                if (Player.Balls > 0)
                    OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.BreederBallsSlap".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.DistendedCumvein".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.HoldBodiesClose".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.SquirtingOverAndOver".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.GushingOverAndOver".Localized());
                else
                    if (Player.CumQ() < 2000)
                        OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.GushAfterGush".Localized());
                    else
                        OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.SeedFloodsCunt".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.MomentOfLucidity".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.ComeDownFromHigh".Localized());

            if (bimbo)
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.SoGoodHotSexy".Localized(
                    Player.Mf("Daddy", "Mommy"),

                    Player.Mf("Daddy", "Mommy"),

                    Player.Mf("Daddy", "Mommy"),

                    Player.Mf("stud", "herm"),

                    Player.Mf("Daddy", "Mommy")
                ));
            else
                OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.CouldGetAddicted".Localized(
                    Player.Mf("Daddy", "Mommy"),

                    Player.Mf("Daddy", "Mommy"),

                    Player.Mf("stud", "perfect breeder"),

                    Player.Mf("Daddy", "Mommy")
                ));

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.KissesWithSoftLips".Localized());

            OutputText("Scenes.Sophiefollowerscene.PhaseTwoOfIncest.NearlyAnHourCuddling".Localized());

            Player.Orgasm("Generic");

            ReturnToCamp(1);
        }

    }
}