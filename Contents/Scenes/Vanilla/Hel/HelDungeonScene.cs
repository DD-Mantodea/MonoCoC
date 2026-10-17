using MonoCoC.Attributes;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Hel
{
    [Register<HelDungeonScene>("coc:hel_dungeon")]
    public class HelDungeonScene : Scene
    {
        public new static void ReturnToCamp(int hours)
        {
            if (hours > 0)
                View.Time.PassTime(hours);

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());
        }

        public static Action? _pendingRoom;

        public override void Entry()
        {
            DungeonSession.InDungeon = true;

            if (_pendingRoom != null)
            {
                var room = _pendingRoom;

                _pendingRoom = null;

                room();

                return;
            }

            RoomGuardHall();
        }

        public void HeliaDiscovery()
        {

            OutputText("Scenes.Heldungeon.HeliaDiscovery.BeforeBeddingDown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelIsabellaThreesomeEnabled) == 0)
            {
                OutputText("Scenes.Heldungeon.HeliaDiscovery.SalamanderShouts".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Heldungeon.HeliaDiscovery.RushOverStopHel".Localized());
                else
                    OutputText("Scenes.Heldungeon.HeliaDiscovery.LazilyPointOutTraps".Localized());

                OutputText("Scenes.Heldungeon.HeliaDiscovery.GuideHerToCamp".Localized());
            }
            else
                OutputText("Scenes.Heldungeon.HeliaDiscovery.SalamanderCalls".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.SitHerDown".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Heldungeon.HeliaDiscovery.HandOnYourThigh".Localized());
            else
                OutputText("Scenes.Heldungeon.HeliaDiscovery.SheStrokesYourFlank".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.AskIfThatsTrue".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.AskAFavor".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.BelchesThunderously".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.AskWhyPiqued".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.FeatherBitches".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.GetRestOfInfo".Localized());

            OutputText("Scenes.Heldungeon.HeliaDiscovery.DoYouAgreeQuestion".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), AgreeToHelpHeliaDungeon);

            AddButton(1, "Common.Button.No".Localized(), NoDungeon);
        }

        public void NoDungeon()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.NoDungeon.ConsiderRefuse".Localized());

            OutputText("Scenes.Heldungeon.NoDungeon.HelStammers".Localized());

            OutputText("Scenes.Heldungeon.NoDungeon.TryToExplain".Localized());

            OutputText("Scenes.Heldungeon.NoDungeon.WellFuckYouAnyway".Localized());

            OutputText("Scenes.Heldungeon.NoDungeon.HelRunsAway".Localized());

            OutputText("Scenes.Heldungeon.NoDungeon.WellThen".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelReducedEncounterRate, 1);

            CoC.Instance.Flags.Set(GameFlagIds.HelFuckbuddy, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelAffection, Math.Max(0, CoC.Instance.Flags.Get(GameFlagIds.HelAffection) - 70));

            DoNext(PlayerMenu);
        }

        public void AgreeToHelpHeliaDungeon()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.MullProposition".Localized());

            OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.LeapsIntoHug".Localized());

            if (!Player.IsTaur())
                OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.CuddlingInLap".Localized());

            OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.NuzzlesWhispersThanks".Localized());

            OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.HandThroughHair".Localized());

            OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.CurlUpWithHelia".Localized());
            // 原版 HelDungeon.hx:123：Marble 在营地且开启 silly 模式时，Marble 会撞见这一幕
            if (SceneInstance<Npcs.MarbleScene>.Get().MarbleAtCamp() && silly)
            {
                OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.CloppingHooves".Localized());

                OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.MarbleAghast".Localized());

                OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.ThisCouldBeBad".Localized());

                OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.BackOffCowSlut".Localized());

                if (silly)

                    OutputText("Scenes.Heldungeon.AgreeToHelpHeliaDungeon.MarbleStammers".Localized(
                        SceneInstance<Npcs.KihaFollowerScene>.Get().FollowerKiha()

                            ? " As Hel settles back into your arms, you're almost certain you hear a rather draconic scream of rage in the distance."

                            : "")); // 原版 HelDungeon.hx:132：Kiha 是跟随者时追加一句远处的怒吼
            }

            CoC.Instance.Flags.Set(GameFlagIds.HelFollowerLevel, -1);

            DoNext(PlayerMenu);

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesOrgasmed) > 0)
                Player.Orgasm("Generic");
        }

        public void MorningAfterHeliaDungeonAgreements()
        {

            OutputText("Scenes.Heldungeon.MorningAfterHeliaDungeonAgreements.EyesFlickerOpen".Localized());

            if (Player.BiggestBreastSize() > 3)
                OutputText("Scenes.Heldungeon.MorningAfterHeliaDungeonAgreements.HeadNestled".Localized());

            OutputText("Scenes.Heldungeon.MorningAfterHeliaDungeonAgreements.SnoringBoorishly".Localized((CoC.Instance.Flags.Get(GameFlagIds.TimesOrgasmed) > 0 ? " and sex" : "")));

            OutputText("Scenes.Heldungeon.MorningAfterHeliaDungeonAgreements.HuhWhaGroans".Localized());

            OutputText("Scenes.Heldungeon.MorningAfterHeliaDungeonAgreements.WhatsThePlan".Localized());

            OutputText("Scenes.Heldungeon.MorningAfterHeliaDungeonAgreements.NeedAffairsInOrder".Localized());

            OutputText("Scenes.Heldungeon.MorningAfterHeliaDungeonAgreements.HeliaAddedToLovers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelFollowerLevel, 1);

            DoNext(PlayerMenu);
        }

        public void NotYet()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.NotYet.OnlyCheckingOnHer".Localized());

            DoNext(PlayerMenu);
        }

        public void GoToHeliaDungeon()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.GoToHeliaDungeon.ReadyAsEver".Localized());

            DoNext(() => GoToHeliaDungeon2());
        }

        public void GoToHeliaDungeon2()
        {
            ClearOutput();

            ShowImage("dungeon-entrance-phoenixtower");

            OutputText("Scenes.Heldungeon.GoToHeliaDungeon2.WithinTheHourHike".Localized());

            OutputText("Scenes.Heldungeon.GoToHeliaDungeon2.CrestOfTower".Localized());

            OutputText("Scenes.Heldungeon.GoToHeliaDungeon2.QuietSheHisses".Localized());

            OutputText("Scenes.Heldungeon.GoToHeliaDungeon2.QuietlyNod".Localized());

            OutputText("Scenes.Heldungeon.GoToHeliaDungeon2.SafeFromHarpies".Localized());

            OutputText("Scenes.Heldungeon.GoToHeliaDungeon2.BoostHerUpWall".Localized());

            DungeonSession.InDungeon = true;

            Go<HelDungeonScene>();
        }

        public void RetryDungeonFromBadEndPrompt()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.RetryDungeonFromBadEndPrompt.LikeToRetry".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), ReallyRetry);

            AddButton(1, "Common.Button.No".Localized(), DeclineRetry);
        }

        public void ReallyRetry()
        {

            Player.ApplyStats(new DynStat(GameStats.Lust, -100f));

            Player.Fatigue = 0;

            Player.Health = Player.MaxHealth;

            CoC.Instance.Flags.Set(GameFlagIds.HelHarpiesDefeated, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelPhoenixesDefeated, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelHarpyQueenDefeated, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelBrigidDefeated, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelKnowsAboutHakon, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HarpyQueenExecuted, 0);

            CoC.Instance.Flags.Set(GameFlagIds.TookQueenStaff, 0);

            GoToHeliaDungeon();
        }

        public void DeclineRetry()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.DeclineRetry.LoadLatestSave".Localized());

            GameOver();

            RemoveButton(1);
        }

        public void ReturnToHeliaDungeon()
        {
            ClearOutput();

            ShowImage("dungeon-entrance-phoenixtower");

            OutputText("Scenes.Heldungeon.ReturnToHeliaDungeon.HikeUpAgain".Localized());

            DungeonSession.InDungeon = true;

            Go<HelDungeonScene>();
        }

        public void ExitHelTower()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.ExitHelTower.SlipOutTheDoor".Localized());

            DungeonSession.InDungeon = false;

            DoNext(() => ReturnToCamp(1));
        }

        public void TakeGodMead()
        {

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.GodMead), 1);

RoomCellar();

            CoC.Instance.Flags.Set(GameFlagIds.HelDungeonMeadLooted, CoC.Instance.Flags.Get(GameFlagIds.HelDungeonMeadLooted) + 1);
        }

        public void TakeWhip()
        {

Player.Inventory.Add(ItemRegistry.Create(ItemIds.SuccubusWhip), 1); RoomDungeon();

            CoC.Instance.Flags.Set(GameFlagIds.HelDungeonTakenWhip, 1);
        }

        public void TakeStraps()
        {

Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BondageStraps), 1); RoomDungeon();

            CoC.Instance.Flags.Set(GameFlagIds.HelDungeonTakenStraps, 1);
        }

        public void TakeDagger()
        {

Player.Inventory.Add(ItemRegistry.Create(new Identifier("coc", "lust_dagger")), 1); RoomDungeon();

            CoC.Instance.Flags.Set(GameFlagIds.HelDungeonTakenDagger, 1);
        }

        public void TakeGooArmor()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.TakeGooArmor.ArmorRackSuit".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TakeArmor".Localized(), TakeGooArmor4Realz);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void TakeGooArmor4Realz()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.TakeGooArmor4Realz.ReachOutToGrab".Localized());

            OutputText("Scenes.Heldungeon.TakeGooArmor4Realz.ArmoredGooGrowls".Localized());

            OutputText("Scenes.Heldungeon.TakeGooArmor4Realz.WhatTheFuckFight".Localized());

            StartHelCombat("goo_armor", RoomGuardHall);
        }

        public void GooArmorBeatsUpPC()
        {

            OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.CollapseVagina".Localized());

            OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.RealizeMistakeVagina".Localized());

            if (Player.HasVagina() && !Player.HasCock())
            {
                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.PeelArmorVagina".Localized());

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.ShapeCuntVagina".Localized());

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.LikeThatVagina".Localized());

                Player.CuntChange(25, true, true, false);

                if (Player.BiggestBreastSize() > 1)
                {
                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.HammersCunny".Localized());

                    if (Player.LactationQ() > 0)
                        OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.SprayOfMilk".Localized());
                }

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.PoundingYourCunt".Localized());

                if (Player.Tallness < 70)
                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.LittleWord".Localized());
                else
                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.BigWord".Localized());

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.GirlReadyToCum".Localized());

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.EruptFemspunk".Localized());

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.PattingFullBelly".Localized());

                if (Player.BiggestBreastSize() > 1 && Player.LactationQ() > 0)
                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.AndMilkWord".Localized());

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.SwirlingInsideHer".Localized());

                OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.BeforeYouRecoverVagina".Localized());
            }
            else
                if (Player.HasCock())
                {
                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.PeelArmorCock".Localized());

                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.PluralSCocks".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.ShowOfLickingLips".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.StrokeYourCock".Localized(
                         Player.MultiCockDescriptLight()
                    ));

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.JerkingYouOff".Localized());

                    if (Player.Tallness < 70)
                        OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.LittleWordAgain".Localized());
                    else
                        OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.BigWordAgain".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.BoyReadyToCum".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.ClimaxRopesOfJizz".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.PattingFullBellyCock".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.BeforeYouRecoverCock".Localized());
                }
                else
                {
                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.CollapseGenderless".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.RealizeMistakeGenderless".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.WhatJustWhat".Localized(
                         Player.Mf("Dude", "Babe")
                    ));

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.StruggleWeakly".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.FuckItWhatever".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.ShrugsAndSaunters".Localized());

                    OutputText("Scenes.Heldungeon.GooArmorBeatsUpPC.SorePickYourselfUp".Localized());
                }

            Player.HealthChange(1000);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, 3f));

            CombatScene.CleanupAfterCombat();

            DoNext(PlayerMenu);

            CoC.Instance.Flags.Set(GameFlagIds.LostGooArmorFight, 1);
        }

        public void BeatUpGooArmor()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.BeatUpGooArmor.SuccumbingToYour".Localized());

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
                OutputText("Scenes.Heldungeon.BeatUpGooArmor.EroticAbilitiesWord".Localized());
            else
                OutputText("Scenes.Heldungeon.BeatUpGooArmor.SkillInBattleWord".Localized());

            OutputText("Scenes.Heldungeon.BeatUpGooArmor.ArmoredGooSlumps".Localized());

            OutputText("Scenes.Heldungeon.BeatUpGooArmor.GooGaspsWaving".Localized());

            OutputText("Scenes.Heldungeon.BeatUpGooArmor.ScowlAtArmorGoo".Localized());

            OutputText("Scenes.Heldungeon.BeatUpGooArmor.RaiseAnEyebrow".Localized());

            OutputText("Scenes.Heldungeon.BeatUpGooArmor.RapsGooeyKnuckles".Localized());

            OutputText("Scenes.Heldungeon.BeatUpGooArmor.InterestingOffer".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Tamani.TamaniMaleRepeatEncounter.ButtonTakeHer".Localized(), TakeGooArmorAndWearIt);

            AddButton(1, "Common.Button.RefuseHer".Localized(), RefuseGooArmorOffer);

            AddButton(2, "Common.Button.RefusePolite".Localized(), RefuseGooArmorOfferPolitely);

            CoC.Instance.Flags.Set(GameFlagIds.WonGooArmorFight, 1);
        }

        public void RefuseGooArmorOffer()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.ValeriaFoundInGlacialRift) == 0)
            {
                OutputText("Scenes.Heldungeon.RefuseGooArmorOffer.TellHerFuckOff".Localized());

                OutputText("Scenes.Heldungeon.RefuseGooArmorOffer.HuffsIndignantly".Localized());

                Player.HealthChange(1000);

                CombatScene.CleanupAfterCombat();

                DoNext(PlayerMenu);
            }
            else
            {
                OutputText("Scenes.Heldungeon.RefuseGooArmorOffer.TellFuckOffAgain".Localized());

                OutputText("Scenes.Heldungeon.RefuseGooArmorOffer.HuffsIndignantlyAgain".Localized());

                Player.HealthChange(Player.MaxHealth);

                CombatScene.CleanupAfterCombat();

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void RefuseGooArmorOfferPolitely()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.ValeriaFoundInGlacialRift) == 0)
            {
                OutputText("Scenes.Heldungeon.RefuseGooArmorOfferPolitely.NoThanksNotNow".Localized());

                OutputText("Scenes.Heldungeon.RefuseGooArmorOfferPolitely.HuffsIndignantly".Localized());
            }
            else
            {
                OutputText("Scenes.Heldungeon.RefuseGooArmorOfferPolitely.NotNowAgain".Localized());

                OutputText("Scenes.Heldungeon.RefuseGooArmorOfferPolitely.HuffsIndignantlyAgain".Localized());
            }

            Player.HealthChange(Player.MaxHealth);

            CombatScene.CleanupAfterCombat();

            DoNext(PlayerMenu);
        }

        public void TakeGooArmorAndWearIt()
        {
            ClearOutput();

            var gooArmor = (Armor)ItemRegistry.Create(ArmorIds.GooArmor);

            gooArmor.UseText();

            Player.Armor.RemoveText();

            CombatScene.CleanupAfterCombat();

            OutputText("Scenes.Heldungeon.TakeGooArmorAndWearIt.FeelInvigorated".Localized(((CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) != 0 && CoC.Instance.Flags.Get(GameFlagIds.HelHarpiesDefeated) > 0 && CoC.Instance.Flags.Get(GameFlagIds.HelPhoenixesDefeated) > 0 && CoC.Instance.Flags.Get(GameFlagIds.HelBrigidDefeated) > 0) ? "Common.Text.Adventures".Localized() : "Common.Text.Dungeon2".Localized())));

            CoC.Instance.Flags.Set(GameFlagIds.MetValeria, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ValeriaFluids, 80);

            Player.HealthChange(Player.MaxHealth);

            CoC.Instance.Flags.Set(GameFlagIds.TookGooArmor, 1);

            var item = Player.SetArmor(gooArmor);

            if (item == null)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ValeriaFoundInGlacialRift) == 0)
                    RoomGuardHall();
                else
                    ReturnToCamp(1);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.ValeriaFoundInGlacialRift) == 0)
                {
                Player.Inventory.Add(item, 1);

                    RoomGuardHall();
                }
                else
                {
                Player.Inventory.Add(item, 1);

                    ReturnToCamp(1);
                }
        }

        public void PcLosesToHarpyHorde()
        {

            OutputText("Scenes.Heldungeon.PcLosesToHarpyHorde.UnableToWithstand".Localized());

            if (Player.Health < 1)
                OutputText("Scenes.Heldungeon.PcLosesToHarpyHorde.BrutalAssaultWord".Localized());
            else
                OutputText("Scenes.Heldungeon.PcLosesToHarpyHorde.RawSexualityWord".Localized());

            OutputText("Scenes.Heldungeon.PcLosesToHarpyHorde.CollapseAtTheirMercy".Localized());

            OutputText("Scenes.Heldungeon.PcLosesToHarpyHorde.DraggedUpstairs".Localized());

            DoNext(() => HarpyQueenBeatsUpPCBadEnd());
        }

        public void PcDefeatsHarpyHorde()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HelHarpiesDefeated, 1);

            OutputText("Scenes.Heldungeon.PcDefeatsHarpyHorde.HarpiesCollapsePile".Localized());

            OutputText("Scenes.Heldungeon.PcDefeatsHarpyHorde.GoddamnOneArmy".Localized(
                 Player.Mf("dude", "lady")
            ));

            OutputText("Scenes.Heldungeon.PcDefeatsHarpyHorde.AskWhoSheIs".Localized());

            OutputText("Scenes.Heldungeon.PcDefeatsHarpyHorde.HerInformer".Localized());

            OutputText("Scenes.Heldungeon.PcDefeatsHarpyHorde.AskMeAnything".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void KiriInteraction()
        {

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), TalkToKiri);

            AddButton(1, "Common.Button.Sex".Localized(), KiriSexIntro);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void TalkToKiri()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.TalkToKiri.AskShareInfo".Localized());

            OutputText("Scenes.Heldungeon.TalkToKiri.OfCoursePleasantly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Hel".Localized(), AskKirkAboutHel);

            AddButton(1, "Common.Button.Harpies".Localized(), AskKiriAboutHarpies);

            AddButton(2, "Codex.Entry.Salamanders.Name".Localized(), AskKiriAboutSalamander);

            AddButton(3, "Common.Button.Kiri".Localized(), AskKiriAboutKiri);

            AddButton(14, "Common.Button.NeverMind".Localized(), () => KiriInteraction());
        }

        public void AskKirkAboutHel()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.AskKirkAboutHel.HowSheKnowsHel".Localized());

            OutputText("Scenes.Heldungeon.AskKirkAboutHel.StartsNervously".Localized());

            OutputText("Scenes.Heldungeon.AskKirkAboutHel.AskIfJustFriends".Localized());

            OutputText("Scenes.Heldungeon.AskKirkAboutHel.StammersAghast".Localized());

            DoNext(() => TalkToKiri());
        }

        public void AskKiriAboutHarpies()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.AskKiriAboutHarpies.AskAboutHarpies".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutHarpies.ScoutedAround".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutHarpies.PhoenixesQuestion".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutHarpies.HalfBreedsMother".Localized());

            DoNext(() => TalkToKiri());
        }

        public void AskKiriAboutSalamander()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.AskAboutSalamander".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.AboutThat".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.UhOh".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.HesFineSortOf".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.WellShit".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.HelDoesntKnowYet".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.HowFreeHim".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutSalamander.MotherKeepsKey".Localized());

            DoNext(() => TalkToKiri());
        }

        public void AskKiriAboutKiri()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.AskKiriAboutKiri.AskAboutHerself".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutKiri.NobodySpecial".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutKiri.UrgeHerAnyway".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutKiri.HalfBreedOfSorts".Localized());

            OutputText("Scenes.Heldungeon.AskKiriAboutKiri.ShrugCarryOn".Localized());

            DoNext(() => TalkToKiri());
        }

        public void KiriSexIntro()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Heldungeon.KiriSexIntro.AskBlowOffSteam".Localized());

            OutputText("Scenes.Heldungeon.KiriSexIntro.WhaWhatYelps".Localized());

            OutputText("Scenes.Heldungeon.KiriSexIntro.RemindHerMareth".Localized());

            OutputText("Scenes.Heldungeon.KiriSexIntro.HangsHerHead".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Heldungeon.KiriSexIntro.NothingSheCanDo".Localized());

            if (Player.HasCock())
                AddButton(0, "Common.Button.Anal".Localized(), KiriSexAnal);

            if (Player.HasVagina())
                AddButton(1, "Common.Button.GetLicked".Localized(), KiriSexGetLicked);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void KiriSexAnal()
        {
            ClearOutput();

            int x = Player.CockThatFits(60);

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = x + 1;

            OutputText("Scenes.Heldungeon.KiriSexAnal.WhipCockOut".Localized(
                 y
            ));

            OutputText("Scenes.Heldungeon.KiriSexAnal.ShrieksPullOut".Localized(
                 y
            ));

            OutputText("Scenes.Heldungeon.KiriSexAnal.TakesSomeDoing".Localized(
                 y
            ));

            if (Player.CockArea(x) > 60)
                OutputText("Scenes.Heldungeon.KiriSexAnal.CanFitNoMore".Localized());
            else
                OutputText("Scenes.Heldungeon.KiriSexAnal.BuriedToTheHilt".Localized());

            OutputText("Scenes.Heldungeon.KiriSexAnal.WrithesPainedPleasure".Localized());

            OutputText("Scenes.Heldungeon.KiriSexAnal.SinkHandsIntoButt".Localized());

            OutputText("Scenes.Heldungeon.KiriSexAnal.CumQuicklyGrunting".Localized());

            if (Player.CockArea(x) > 60)
                OutputText("Scenes.Heldungeon.KiriSexAnal.FarInAsCan".Localized());
            else
                OutputText("Scenes.Heldungeon.KiriSexAnal.FillingHerCompletely".Localized());

            OutputText("Scenes.Heldungeon.KiriSexAnal.SquirtsThickLoad".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Heldungeon.KiriSexAnal.CreamSquelchesOut".Localized());

            OutputText("Scenes.Heldungeon.KiriSexAnal.ScreamOfDelight".Localized(
                 y
            ));

            OutputText("Scenes.Heldungeon.KiriSexAnal.PullOutWithPop".Localized());

            Player.Orgasm("Dick");

            View.Time.PassMinutes((int)Math.Round((1.0 / 3) * 60));

            DoNext(PlayerMenu);
        }

        public void KiriSexGetLicked()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.KiriSexGetLicked.AskKiriEatYou".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Heldungeon.KiriSexGetLicked.AndCocksWord".Localized());

            OutputText("Scenes.Heldungeon.KiriSexGetLicked.WordOfEncouragement".Localized());

            OutputText("Scenes.Heldungeon.KiriSexGetLicked.TongueSurprisinglySkilled".Localized());

            OutputText("Scenes.Heldungeon.KiriSexGetLicked.SlipsTongueIn".Localized());

            OutputText("Scenes.Heldungeon.KiriSexGetLicked.GrindYourSlit".Localized());

            OutputText("Scenes.Heldungeon.KiriSexGetLicked.CannotResistTongue".Localized());

            OutputText("Scenes.Heldungeon.KiriSexGetLicked.UtterlySatisfied".Localized());

            Player.Orgasm("Generic");

            View.Time.PassMinutes((int)Math.Round((1.0 / 3) * 60));

            DoNext(PlayerMenu);
        }

        public void TalkToValeria()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.TalkToValeria.AskWhatSheThinks".Localized());

            OutputText("Scenes.Heldungeon.TalkToValeria.OhHiLaughs".Localized());

            OutputText("Scenes.Heldungeon.TalkToValeria.WellHeyThereCutie".Localized());

            OutputText("Scenes.Heldungeon.TalkToValeria.ClearThroatRepeat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0)
                OutputText("Scenes.Heldungeon.TalkToValeria.BroodmotherSerious".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelBrigidDefeated) == 0)
                OutputText("Scenes.Heldungeon.TalkToValeria.BrigidTheJailer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelPhoenixesDefeated) == 0)
                OutputText("Scenes.Heldungeon.TalkToValeria.FreakyHalfBreeds".Localized());

            DoNext(PlayerMenu);
        }

        public void TortureGear()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Heldungeon.TortureGear.WalkUpToRack".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenWhip) == 0 || CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenStraps) == 0 || CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenDagger) == 0)
            {
                OutputText("Scenes.Heldungeon.TortureGear.RackContains".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenWhip) == 0)
                {
                    OutputText("Scenes.Heldungeon.TortureGear.WhipWord".Localized());

                    AddButton(0, "Common.Button.Sucwhip".Localized(), TakeWhip);
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenStraps) == 0)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenWhip) == 0)
                        OutputText("Scenes.Heldungeon.TortureGear.CommaSpace".Localized());

                    OutputText("Scenes.Heldungeon.TortureGear.LeatherStrapsWord".Localized());

                    AddButton(1, "Common.Button.Bonstrp".Localized(), TakeStraps);
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenDagger) == 0)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenStraps) == 0 || CoC.Instance.Flags.Get(GameFlagIds.HelDungeonTakenWhip) == 0)
                        OutputText("Scenes.Heldungeon.TortureGear.CommaSpaceAgain".Localized());

                    OutputText("Scenes.Heldungeon.TortureGear.LustDraftDagger".Localized());

                    AddButton(2, "Common.Button.LDaggr".Localized(), TakeDagger);
                }

                OutputText("Scenes.Heldungeon.TortureGear.PeriodSpace".Localized());
            }
            else
                OutputText("Scenes.Heldungeon.TortureGear.RackIsEmpty".Localized());

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void HelDungeonPrisonerTalk()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.HelPcTalkedWithHakon) == 0)
            {
                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.ApproachSalamander".Localized());

                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.SightForSoreEyes".Localized());

                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.HereToRescueYou".Localized());

                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.NotComplaining".Localized());

                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.HeliaSentMe".Localized());

                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.MyLittleHel".Localized());

                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.NotFarAway".Localized());

                OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.YouBroughtHerHere".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HelPcTalkedWithHakon, 1);
            }
            else
                if (Player.HasKeyItem("Harpy Key A") && Player.HasKeyItem("Harpy Key B"))
                {
                    OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.SmileApproachHakon".Localized());

                    DoNext(() => TowerOutro());

                    return ;
                }
                else
                    OutputText("Scenes.Heldungeon.HelDungeonPrisonerTalk.ApproachHakon".Localized());

            DoNext(PlayerMenu);
        }

        public void PhoenixPlatoonMurdersPC()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.PhoenixPlatoonMurdersPC.CollapseToo".Localized());

            if (Player.Lust >= Player.MaxLust)
                OutputText("Scenes.Heldungeon.PhoenixPlatoonMurdersPC.TurnedOnWord".Localized());
            else
                OutputText("Scenes.Heldungeon.PhoenixPlatoonMurdersPC.BadlyInjuredWord".Localized());

            OutputText("Scenes.Heldungeon.PhoenixPlatoonMurdersPC.ToContinueTheFight".Localized());

            OutputText("Scenes.Heldungeon.PhoenixPlatoonMurdersPC.GonerBringToMom".Localized());

            DoNext(() => HarpyQueenBeatsUpPCBadEnd());
        }

        public void PhoenixPlatoonLosesToPC()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.PhoenixPlatoonLosesToPC.FinalGruntCollapses".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelPhoenixesDefeated, CoC.Instance.Flags.Get(GameFlagIds.HelPhoenixesDefeated) + 1);

            CombatScene.CleanupAfterCombat();
        }

        public void CheckOutDemBirdBitches()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Heldungeon.CheckOutDemBirdBitches.LoomOverHeavyInfantry".Localized());

            if (Player.Lust > 33)
            {
                OutputText("Scenes.Heldungeon.CheckOutDemBirdBitches.UseOneGetOff".Localized());

                if (Player.HasCock())
                {
                    if (Player.HasCockThatFits(80))
                        AddButton(0, "Scenes.Aiko.AikoConsSex.ButtonMissionary".Localized(), PhoenixMissionary);
                    else
                        OutputText("Scenes.Heldungeon.CheckOutDemBirdBitches.TooBigToFuck".Localized());

                    AddButton(1, "Common.Button.GetWanked".Localized(), PhoenixWanking);
                }

                if (Player.HasVagina())
                    AddButton(2, "Common.Button.RideVaginal".Localized(), PhoenixAginal);

                AddButton(3, "Common.Button.RideAnal".Localized(), GitButtRoadPhoenix);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void PhoenixMissionary()
        {
            ClearOutput();

            int x = Player.CockThatFits(80);

            if (x < 0)
                x = Player.SmallestCockIndex();

            var y = x + 1;

            OutputText("Scenes.Heldungeon.PhoenixMissionary.GrabHealthiestPhoenix".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.ForceLegsApart".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.ChideWhySoWet".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.IAmNotNatural".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.OhReallySure".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.SheAdmits".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.ExposeHerCunt".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.AndTightRing".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.GraspWideFlanks".Localized(
                 y
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.AndExtraBoner".Localized());
            else
                OutputText("Scenes.Heldungeon.PhoenixMissionary.UpWithHerHole".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.PushInPenetrating".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.AndAssWord".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.SlidingIntoChannel".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.PluralSChannels".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.SentencePeriod".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.SquirmsAsYouPush".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.PluralSCocksDrive".Localized());
            else
                OutputText("Scenes.Heldungeon.PhoenixMissionary.DrivesWord".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.RollYourHips".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.AndAssWordAgain".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.SlammingHipsIntoHers".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.SheCumsFirst".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.DoublePenetrationTooMuch".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.GripsShouldersTight".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.AndTighterSpasm".Localized());
            else
                OutputText("Scenes.Heldungeon.PhoenixMissionary.SpasmsWord".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.AroundYourDick".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.PluralSDicks".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.SquirmingInEmbrace".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.AndHotAsshole".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.CumIntoHer".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.ReleaseFromEmbrace".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.PluralSCocksOut".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.OutOfRavagedHole".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Heldungeon.PhoenixMissionary.PluralSHoles".Localized());

            OutputText("Scenes.Heldungeon.PhoenixMissionary.AndGatherYourGear".Localized());

            Player.Orgasm("Dick");

            DoNext(PlayerMenu);
        }

        public void PhoenixWanking()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            var y = x + 1;

            OutputText("Scenes.Heldungeon.PhoenixWanking.GrabHealthiestPhoenix".Localized());

            OutputText("Scenes.Heldungeon.PhoenixWanking.GroanNods".Localized(
                 y,

                 y
            ));

            OutputText("Scenes.Heldungeon.PhoenixWanking.FeelsHeavenly".Localized(
                 y,

                 y,

                 y
            ));

            OutputText("Scenes.Heldungeon.PhoenixWanking.FingeringHerself".Localized(
                 y
            ));

            OutputText("Scenes.Heldungeon.PhoenixWanking.ExplodesThickLoad".Localized(
                 y,

                 y
            ));

            Player.Orgasm("Dick");

            DoNext(PlayerMenu);
        }

        public void GitButtRoadPhoenix()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.GrabHealthiestPhoenix".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.StaresEagerCaution".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.YouNod".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.UhWellOkayThen".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.DiscardArmorSquat".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.CompletelyRigid".Localized());

            Player.ButtChange(30, true, true, false);

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.BottomsOut".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.ShiftsHandsToShoulders".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.HelplessSurpriseAttack".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.BiteNippleCum".Localized());

            OutputText("Scenes.Heldungeon.GitButtRoadPhoenix.PhoenixAsleep".Localized());

            Player.Orgasm("Anal");

            DoNext(PlayerMenu);
        }

        public void PhoenixAginal()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.PhoenixAginal.GrabHealthiestPhoenix".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.HeeeeyWhines".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.MaintainGrasp".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.BitOfPhoenixSeed".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.ParagraphBreak".Localized());

            if (Player.HasPerk(PerkIds.BroodMother))
                OutputText("Scenes.Heldungeon.PhoenixAginal.AssureSheWillBe".Localized());
            else
                OutputText("Scenes.Heldungeon.PhoenixAginal.GrinAtHer".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.StripOutOfArmor".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.NiceAndHard".Localized());

            Player.CuntChange(12, true, true, false);

            OutputText("Scenes.Heldungeon.PhoenixAginal.RecoveredFromAttack".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.GiveHerHandUp".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.CumulativePleasure".Localized());

            OutputText("Scenes.Heldungeon.PhoenixAginal.BowLeggedStand".Localized());

            Player.KnockUp(PregnancyIds.OvielixirEggs, 120, 100);

            Player.CreateStatusEffect(StatusEffectIds.Eggs, Random.Shared.Next(6), 0, (5 + Random.Shared.Next(3)), 0);

            Player.Orgasm("Vaginal");

            DoNext(PlayerMenu);
        }

        public void HarpyQueenBeatsUpPCBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.CollapseBeforeQueen".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.WellWhatHaveWe".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.InterloperWanders".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.MotherHarpyCalls".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.AnotherSalamander".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.CouldUseAnother".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.SpermBankWord".Localized());
            else
                OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.IncubatorWord".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.InMyHarem".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.SixMonthsLater".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.WristsChafing".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.NewSensationsBrokeMind".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.SlutGetsHerFill".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.AnotherOrgasmPlows".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.SubterraneanBreeding".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.HelFullBodyOrgasm".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.QueenStretchesWings".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.GoodStudSince".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenBeatsUpPCBadEnd.AnotherOrgasmWashes".Localized());

            GameOver();

            if (!(OptionStore.Instance.Get("hardcore", 0) > 0))
                AddButton(1, "Common.Button.Retry".Localized(), RetryDungeonFromBadEndPrompt);
        }

        public void HarpyQueenDefeatedByPC()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HarpyQueenDefeatedByPC.FinalLoudGasp".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenDefeatedByPC.RuinEverything".Localized());

            Player.CreateKeyItem("Harpy Key B", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.HelHarpyQueenDefeated, 1);

            CombatScene.CleanupAfterCombat();
        }

        public void HeliaThroneRoom()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Heldungeon.HeliaThroneRoom.TurnToSalamander".Localized());

            OutputText("Scenes.Heldungeon.HeliaThroneRoom.HeyGrabArmor".Localized());

            OutputText("Scenes.Heldungeon.HeliaThroneRoom.PatHerOnHead".Localized());

            OutputText("Scenes.Heldungeon.HeliaThroneRoom.WhatsThePlan".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelPcTalkedWithHakon) > 0)
                AddButton(0, "Common.Button.Hakon".Localized(), HeliaHakonTalk);

            AddButton(1, "Common.Button.Kiri".Localized(), HeliaKiriTalk);

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) == 0)
                AddButton(2, "Common.Button.Queen".Localized(), HeliaQueenTalk);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void HeliaHakonTalk()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HeliaHakonTalk.TellHelWhosDownstairs".Localized());

            OutputText("Scenes.Heldungeon.HeliaHakonTalk.ThatCantBe".Localized());

            OutputText("Scenes.Heldungeon.HeliaHakonTalk.AssureItsTrue".Localized());

            OutputText("Scenes.Heldungeon.HeliaHakonTalk.DontBelieveIt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) == 0)
                OutputText("Scenes.Heldungeon.HeliaHakonTalk.PayForWhatDid".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HelKnowsAboutHakon, 1);

            DoNext(PlayerMenu);
        }

        public void HeliaKiriTalk()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.AskAboutKiri".Localized());

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.MetKiri".Localized());

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.SmackFaceConcrete".Localized());

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.MetHerYearsBack".Localized());

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.BoppedTheBull".Localized());

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.AskMoreThanFriends".Localized());

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.GiveMeSomeCredit".Localized(
                 Player.Mf("Dude", "Babe")
            ));

            OutputText("Scenes.Heldungeon.HeliaKiriTalk.RollEyesAndLaugh".Localized());

            DoNext(PlayerMenu);
        }

        public void HeliaQueenTalk()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HeliaQueenTalk.QueenCuntHerself".Localized());

            OutputText("Scenes.Heldungeon.HeliaQueenTalk.AskWhatToDo".Localized());

            OutputText("Scenes.Heldungeon.HeliaQueenTalk.TailUpHerTwat".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelKnowsAboutHakon) == 1)
                OutputText("Scenes.Heldungeon.HeliaQueenTalk.SnapHerNeck".Localized());

            DoNext(PlayerMenu);
        }

        public void HarpyQueenAdvantage()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Heldungeon.HarpyQueenAdvantage.LoomOverQueen".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenAdvantage.KillMeBeDone".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenAdvantage.WhatWord".Localized());

            ClearButtons();

            if (Player.Lust > 33 && Player.HasCock())
                AddButton(0, "Common.Button.FuckHer".Localized(), FuckHarpyQueen);

            AddButton(1, "Common.Button.Interrogate".Localized(), HarpyQueenInterrogate);

            AddButton(2, "Scenes.Tamani.KillTamaniChoice.ButtonKillHer".Localized(), KillHarpyQueen);

            AddButton(3, "Scenes.Aiko.YamataLoses.ButtonLetHerGo".Localized(), LetHarpyQueenGo);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void LetHarpyQueenGo()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.TellHelLetUp".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.WhatDeadpan".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.TellHerMoveFoot".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.MercyWhy".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.NoDemonLovingBastard".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.JustLikeThatQuestion".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.JustLikeThatWord".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.NodForHelOff".Localized());

            OutputText("Scenes.Heldungeon.LetHarpyQueenGo.YoureAFool".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelKnowsAboutHakon) == 1)
            {
                OutputText("Scenes.Heldungeon.LetHarpyQueenGo.HarpiesBeatWings".Localized());

                OutputText("Scenes.Heldungeon.LetHarpyQueenGo.TryYellTooLate".Localized());

                OutputText("Scenes.Heldungeon.LetHarpyQueenGo.BullshitSnapsNeck".Localized());

                AwardAchievement(AchievementIds.Accomplice);

                ClearButtons();

                AddButton(0, "Common.Button.Forgive".Localized(), HarpyQueenLetHerGoForgive);

                AddButton(1, "Common.Button.Berate".Localized(), HarpyQueenLetHerGoBerate);

                CoC.Instance.Flags.Set(GameFlagIds.HarpyQueenExecuted, 1);
            }
            else
            {
                OutputText("Scenes.Heldungeon.LetHarpyQueenGo.StandAsideWatch".Localized());

                OutputText("Scenes.Heldungeon.LetHarpyQueenGo.WeWillMeetAgain".Localized());

                OutputText("Scenes.Heldungeon.LetHarpyQueenGo.HarpiesTakeFlight".Localized());

                DoNext(PlayerMenu);

                CoC.Instance.Flags.Set(GameFlagIds.HarpyQueenExecuted, -1);
            }

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));
        }

        public void HarpyQueenLetHerGoForgive()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.StareEachOtherDown".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.ConsideringEmotions".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.EyesLightUp".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.YouNod".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.TightHugThanks".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.TellHarpiesGetLost".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.AlrightYouveGotKey".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelKnowsAboutHakon) == 1)
                OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.DadWord".Localized());
            else
                OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.ThatPoorSalamander".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.OutOfTheDungeon".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoForgive.HelTrotsOut".Localized());

            DoNext(PlayerMenu);
        }

        public void HarpyQueenLetHerGoBerate()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoBerate.StareEachOtherDown".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoBerate.SlamFootDemand".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoBerate.OfCourseYouWouldnt".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoBerate.RunsOutTheDoor".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenLetHerGoBerate.NoticeQueensStaff".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FoughtWithHelInDungeon, 1);

            DoNext(PlayerMenu);
        }

        public void KillHarpyQueen()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.KillHarpyQueen.LookHelToQueen".Localized());

            OutputText("Scenes.Heldungeon.KillHarpyQueen.SnapHerNeck".Localized());

            OutputText("Scenes.Heldungeon.KillHarpyQueen.HelNodsApprovingly".Localized());

            OutputText("Scenes.Heldungeon.KillHarpyQueen.ThatsThatThen".Localized());

            OutputText("Scenes.Heldungeon.KillHarpyQueen.NodYourAgreement".Localized());

            OutputText("Scenes.Heldungeon.KillHarpyQueen.AlrightYouveGotKey".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelKnowsAboutHakon) == 1)
                OutputText("Scenes.Heldungeon.KillHarpyQueen.DadWord".Localized());
            else
                OutputText("Scenes.Heldungeon.KillHarpyQueen.ThatPoorSalamander".Localized());

            OutputText("Scenes.Heldungeon.KillHarpyQueen.OutOfTheDungeon".Localized());

            OutputText("Scenes.Heldungeon.KillHarpyQueen.HelTrotsOut".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HarpyQueenExecuted, 1);

            DoNext(PlayerMenu);
        }

        public void TakeQueensStaff()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.TakeQueensStaff.PickUpQueensStaff".Localized());

Player.Inventory.Add(ItemRegistry.Create(new Identifier("coc", "eldritch_staff")), 1); RoomThroneRoom();

            CoC.Instance.Flags.Set(GameFlagIds.TookQueenStaff, 1);
        }

        public void FuckHarpyQueen()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.FuckHarpyQueen.DecideTeachLesson".Localized());

            OutputText("Scenes.Heldungeon.FuckHarpyQueen.HopingYoudSayThat".Localized());

            ClearButtons();

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Heldungeon.FuckHarpyQueen.LackEndowments".Localized());

                if (silly)
                    OutputText("Scenes.Heldungeon.FuckHarpyQueen.AnotherReasonGenderless".Localized());
            }

            if (Player.HasCock())
            {
                AddButton(1, "Common.Button.Anal".Localized(), HarpyQueenSexAnal);

                AddButton(2, "Common.Button.Vaginal".Localized(), VaginalHarpyQueenSex);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public void HarpyQueenSexAnal()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.HelTossesQueen".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.PositionBehindBroodmother".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.ThrustIntoHerCrack".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.WinkAtHel".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.PillowyCheeksAbsorb".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.SwatAgainShutUp".Localized());

            if (Player.CockArea(0) < 100)
                OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.UntilHipsSink".Localized());
            else
                OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.UntilMassiveCock".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.SunkInFar".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.SwatAndSwat".Localized(
                 Player.CockDescript(0)
            ));

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.SqueezesDownHard".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenSexAnal.SpentPullOut".Localized());

            Player.Orgasm("Dick");

            View.Time.PassMinutes((int)Math.Round((1.0 / 3) * 60));

            DoNext(PlayerMenu);
        }

        public void VaginalHarpyQueenSex()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.TossQueenFloor".Localized());

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.HelPlantsOverFace".Localized());

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.StrugglesGetsSlap".Localized());

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.KneelBetweenLegs".Localized());

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.SlickWallsAround".Localized());

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.WickedGrinGrabTail".Localized());

            if (Player.CockArea(0) < 48)
                OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.NotToBeOutdone".Localized());

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.SecondShaft".Localized());

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.HarpyGetsOff".Localized());

            if (Player.CumQ() < 300)
            {
                OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.SlamHipsEjaculate".Localized());

                OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.CumAndCumFilling".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.BallsFeelWord".Localized());
                else
                    OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.CrotchFeelsWord".Localized());

                OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.HollowEmptyShuddering".Localized());
            }
            else
            {
                OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.SlamHipsEjaculateAgain".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.BallsFeelWordAgain".Localized());
                else
                    OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.CrotchFeelsWordAgain".Localized());

                OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.HollowEmptySlowWithdraw".Localized());
            }

            OutputText("Scenes.Heldungeon.VaginalHarpyQueenSex.AfterFuckLikeThat".Localized());

            Player.Orgasm("Generic");

            View.Time.PassMinutes((int)Math.Round((1.0 / 3) * 60));

            DoNext(PlayerMenu);
        }

        public void HarpyQueenInterrogate()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.LeaningOverQueen".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.QueenSpitsAtYou".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.HadThatOneComing".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.GoblinAlchemy".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.WeSeeAboutThat".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.RuleTheWorldFool".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.OhReallyQuestion".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.ItIsTheTruth".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.WaitWhat".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.BreedArmyTruth".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.YouBitchSnaps".Localized());

            OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.ThisOneDidNot".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelKnowsAboutHakon) == 1)
                OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.HeWasMarriedBitch".Localized());
            else
                OutputText("Scenes.Heldungeon.HarpyQueenInterrogate.ScowlsSaysNothing".Localized());

            DoNext(PlayerMenu);
        }

        public void TowerOutro()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.HarpyQueenExecuted, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelKnowsAboutHakon) == 0)
            {
                OutputText("Scenes.Heldungeon.TowerOutro.HelpHakonOutSurprise".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.HelAndKiriTurn".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.KiriTakesHand".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.OldScalesWeakGrin".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.ThisIsOurDad".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.OldScalesIsIt".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.LeapsIntoHisArms".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.ReallyYouDad".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.NeverLeavingYouSurprise".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.StoopSurprise".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.TimeGoThanksSurprise".Localized(
                     Player.Mf("Son", "Sweetheart")
                ));

                OutputText("Scenes.Heldungeon.TowerOutro.YouDidAllRight".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.LoverMineChuckles".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.WellShit".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.LaughDownMountain".Localized());
            }
            else
            {
                OutputText("Scenes.Heldungeon.TowerOutro.HelpHakonOutKnows".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.DaddyGleefulCry".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.HakonTurnsHelLeaps".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.HelAsksCrying".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.NeverLeavingYouKnows".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.StoopKnows".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.TimeGoThanksKnows".Localized(
                     Player.Mf("Son", "Sweetheart")
                ));

                OutputText("Scenes.Heldungeon.TowerOutro.YouDidAlrightKnows".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.LoverMineChucklesKnows".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.WellShitKnows".Localized());

                OutputText("Scenes.Heldungeon.TowerOutro.LaughDownMountainKnows".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.ClearedHelTower, 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void RoomGuardHall()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.RoomGuardHall.GuardHallHeading".Localized());

            OutputText("Scenes.Heldungeon.RoomGuardHall.GuardRoomRansacked".Localized());

AddButton(6, "Common.Button.North".Localized(), RoomStairwell);

            AddButton(6, "Common.Button.North".Localized(), RoomStairwell);

            if (CoC.Instance.Flags.Get(GameFlagIds.WonGooArmorFight) + CoC.Instance.Flags.Get(GameFlagIds.LostGooArmorFight) == 0)
                if (CoC.Instance.Flags.Get(GameFlagIds.ClearedHelTower) == 0)
                {
                    OutputText("Scenes.Heldungeon.RoomGuardHall.SuitOfHalfPlate".Localized());

                    AddButton(0, "Common.Button.Armor".Localized(), TakeGooArmor);
                }
                else
                    OutputText("Scenes.Heldungeon.RoomGuardHall.RackAppearsEmpty".Localized());

            OutputText("Scenes.Heldungeon.RoomGuardHall.HeavyIronDoors".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.ClearedHelTower) > 0)
            {
                OutputText("Scenes.Heldungeon.RoomGuardHall.AlreadyClearedTower".Localized());

                AddButton(11, "Common.Button.Leave".Localized(), ExitHelTower);
            }

            AddButton(7, "Common.Button.Trapdoor".Localized(), RoomCellar);
        }

        public void RoomCellar()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.RoomCellar.WineCellarHeading".Localized());

            OutputText("Scenes.Heldungeon.RoomCellar.DroppedIntoHideyHole".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Heldungeon.RoomCellar.JustEnoughRoom".Localized());
            else
                OutputText("Scenes.Heldungeon.RoomCellar.CeilingSoLow".Localized());

            OutputText("Scenes.Heldungeon.RoomCellar.NothingHorrifying".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelDungeonMeadLooted) < 5)
            {
                OutputText("Scenes.Heldungeon.RoomCellar.FewHornsOfMead".Localized());

                var remainingMead = 5 - CoC.Instance.Flags.Get(GameFlagIds.HelDungeonMeadLooted);

                OutputText("Scenes.Heldungeon.RoomCellar.GodsMeadHere".Localized(
                    remainingMead == 1 ? "Common.Text.Is".Localized() : "Common.Text.Are".Localized(),

                    NumberUtils.Num2Text(remainingMead),

                    remainingMead == 1 ? "Common.Text.Horn".Localized() : "Common.Text.Horns".Localized()
                ));

                AddButton(0, "Common.Button.Godmead".Localized(), TakeGodMead);
            }
            else
                OutputText("Scenes.Heldungeon.RoomCellar.WineRackEmpty".Localized());

            AddButton(5, "Common.Button.ClimbUp".Localized(), RoomGuardHall);
        }

        public void RoomStairwell()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.RoomStairwell.StairWellHeading".Localized());

AddButton(11, "Common.Button.South".Localized(), RoomGuardHall);

            AddButton(11, "Common.Button.South".Localized(), RoomGuardHall);

            if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpiesDefeated) == 0)
            {
                OutputText("Scenes.Heldungeon.RoomStairwell.OpenHeavyDoors".Localized());

                OutputText("Scenes.Heldungeon.RoomStairwell.ReadyWeaponWing".Localized());

                OutputText("Scenes.Heldungeon.RoomStairwell.ItsAFight".Localized());

                ClearButtons();

                StartHelCombat("harpy_mob", RoomStairwell);

                return ;
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0)
                {
                    OutputText("Scenes.Heldungeon.RoomStairwell.PileOfDruggedHarpies".Localized());

                            if (Player.ArmorName == "goo armor")
                        AddButton(2, "Common.Button.Valeria".Localized(), TalkToValeria);

                    AddButton(1, "Common.Button.Kiri".Localized(), KiriInteraction);
                }
                else
                    if ((CoC.Instance.Flags.Get(GameFlagIds.HelPhoenixesDefeated) > 0 && CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) > 0))
                        OutputText("Scenes.Heldungeon.RoomStairwell.LargeRoomStairwell".Localized());
                    else
                        OutputText("Scenes.Heldungeon.RoomStairwell.DruggedHarpiesAgain".Localized());

AddButton(7, "Common.Button.Upstairs".Localized(), RoomMezzanine);

AddButton(8, "Common.Button.Downstairs".Localized(), RoomDungeon);

                AddButton(7, "Common.Button.Upstairs".Localized(), RoomMezzanine);

                AddButton(8, "Common.Button.Downstairs".Localized(), RoomDungeon);
            }
        }

        public void RoomDungeon()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.RoomDungeon.DungeonHeading".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelBrigidDefeated) == 0)
            {
                OutputText("Scenes.Heldungeon.RoomDungeon.DownstairsSmokyRoom".Localized());

                OutputText("Scenes.Heldungeon.RoomDungeon.TallestHarpyBrigid".Localized());

                OutputText("Scenes.Heldungeon.RoomDungeon.BoutTimeMadeIt".Localized(
                     Player.Mf("bastard", "bitch")
                ));

                OutputText("Scenes.Heldungeon.RoomDungeon.ReadyWeaponTakeKeys".Localized());

                StartHelCombat("brigid", RoomDungeon);

                return ;
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ClearedHelTower) == 0)
                {
                    OutputText("Scenes.Heldungeon.RoomDungeon.DungeonRoomPrisoner".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.HelPcTalkedWithHakon) == 0)
                    {
                        OutputText("Scenes.Heldungeon.RoomDungeon.OnWhichPrisonerLies".Localized());

                        AddButton(0, "Common.Button.Prisoner".Localized(), HelDungeonPrisonerTalk);
                    }
                    else
                    {
                        OutputText("Scenes.Heldungeon.RoomDungeon.OnWhichHakonLies".Localized());

                        AddButton(0, "Common.Button.Hakon".Localized(), HelDungeonPrisonerTalk);
                    }

                    OutputText("Scenes.Heldungeon.RoomDungeon.SentencePeriod".Localized());

                    if (Player.HasKeyItem("Harpy Key A") && Player.HasKeyItem("Harpy Key B"))
                        OutputText("Scenes.Heldungeon.RoomDungeon.HaveTheKeys".Localized());
                }
                else
                    OutputText("Scenes.Heldungeon.RoomDungeon.DungeonRoomEmpty".Localized());

AddButton(7, "Common.Button.Upstairs".Localized(), RoomStairwell);

                AddButton(7, "Common.Button.Upstairs".Localized(), RoomStairwell);

                AddButton(1, "Common.Button.TortureGear".Localized(), TortureGear);
            }
        }

        public void RoomMezzanine()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.RoomMezzanine.MezzanineHeading".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelPhoenixesDefeated) == 0)
            {
                OutputText("Scenes.Heldungeon.RoomMezzanine.AscendStepsPhoenixes".Localized());

                ClearButtons();

                StartHelCombatMultiple(["phoenix_commander", "phoenix_grenadier", "phoenix_pyro", "phoenix_sapper"], RoomMezzanine);

                return ;
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ClearedHelTower) == 0)
                    OutputText("Scenes.Heldungeon.RoomMezzanine.MezzaninePhoenixesPile".Localized());
                else
                    OutputText("Scenes.Heldungeon.RoomMezzanine.MezzanineEmpty".Localized());

AddButton(7, "Common.Button.Upstairs".Localized(), RoomThroneRoom);

AddButton(8, "Common.Button.Downstairs".Localized(), RoomStairwell);

                AddButton(7, "Common.Button.Upstairs".Localized(), RoomThroneRoom);

                AddButton(8, "Common.Button.Downstairs".Localized(), RoomStairwell);

                if (CoC.Instance.Flags.Get(GameFlagIds.ClearedHelTower) == 0)
                    AddButton(1, "Common.Button.Phoenixes".Localized(), CheckOutDemBirdBitches);
            }
        }

        public void RoomThroneRoom()
        {
            ClearOutput();

            OutputText("Scenes.Heldungeon.RoomThroneRoom.ThroneRoomHeading".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0)
            {
                OutputText("Scenes.Heldungeon.RoomThroneRoom.AscendingStairsDoors".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.HelVsBroodmother".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.HelSpinsBesideYou".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.TookYourSweetTime".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.ReassuringNod".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.YouFoolsHisses".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.IgnoreHerPosition".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.OhWellFuckMe".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.TakePieceOfAction".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.HelGrabsTwoHarpies".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.IdiotSheSneers".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.WhitewoodStaffFire".Localized());

                OutputText("Scenes.Heldungeon.RoomThroneRoom.UnseenForceSlamsDoor".Localized());

                StartHelCombat("harpy_queen", RoomThroneRoom);

                return ;
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.ClearedHelTower) == 0)
                    OutputText("Scenes.Heldungeon.RoomThroneRoom.ThroneRoomHarpiesCower".Localized());
                else
                    OutputText("Scenes.Heldungeon.RoomThroneRoom.ThroneRoomEmpty".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) == 0)
                {
                    OutputText("Scenes.Heldungeon.RoomThroneRoom.QueenSlumpsInsensate".Localized());

                    AddButton(1, "Npcs.HarpyQueen.Name".Localized(), HarpyQueenAdvantage);

                    AddButton(0, "Common.Button.Helia".Localized(), HeliaThroneRoom);
                }

                if (CoC.Instance.Flags.Get(GameFlagIds.HarpyQueenExecuted) == 1 && CoC.Instance.Flags.Get(GameFlagIds.TookQueenStaff) == 0)
                    AddButton(1, "Common.Button.TakeStaff".Localized(), TakeQueensStaff);

AddButton(8, "Common.Button.Downstairs".Localized(), RoomMezzanine);
            }
        }

        public void StartHelCombat(string enemyId, Action backToRoom)
        {
            try
            {
                var enemy = EnemyRegistry.Create(new Identifier("coc", enemyId));

                _pendingRoom = backToRoom;

                CombatScene.Start(enemy, () => Go<HelDungeonScene>());
            }
            catch (Exception)
            {
                _pendingRoom = null;

                OutputText("[TODO: enemy '" + enemyId + "Common.Text.IsNotPortedYet".Localized());

                AddButton(11, "Common.Button.Back".Localized(), backToRoom);
            }
        }

        public void StartHelCombatMultiple(string[] enemyIds, Action backToRoom)
        {
            try
            {
                var enemies = new System.Collections.Generic.List<Enemy>();

                foreach (var enemyId in enemyIds)
                    enemies.Add(EnemyRegistry.Create(new Identifier("coc", enemyId)));

                _pendingRoom = () =>
                {
                    if (CombatScene.LastOutcome is CombatOutcome.HpVictory or CombatOutcome.LustVictory)
                        CoC.Instance.Flags.Set(GameFlagIds.HelPhoenixesDefeated, 1);

                    backToRoom();
                };

                CombatScene.StartMultiple(enemies, () => Go<HelDungeonScene>());
            }
            catch (Exception)
            {
                _pendingRoom = null;

                OutputText("Common.Text.TodoPhoenixPlatoonIsNotFully".Localized());

                AddButton(11, "Common.Button.Back".Localized(), backToRoom);
            }
        }
    }
}