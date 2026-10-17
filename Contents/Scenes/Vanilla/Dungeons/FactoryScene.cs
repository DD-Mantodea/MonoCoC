using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Utils;
using MonoCoC.Contents.Dungeons;

namespace MonoCoC.Contents.Scenes.Vanilla.Factory
{
    [Register<FactoryScene>("coc:factory")]
    public class FactoryScene : Scene
    {

        public static string checkIronDoor => Player.HasKeyItem("Iron Key") ? "NESW" : "LNNESW";

        public static string checkPremiumDoor => Player.HasKeyItem("Supervisor's Key") ? "ES" : "LSSE";

        public static void RunFunc() => DungeonScene.ActiveContext?.Refresh();

        public override void Entry()
        {
            EnterDungeon();
        }

        public void EnterDungeon()
        {
            ClearOutput();

            ShowImage("dungeon-entrance-factory");

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                OutputText("Scenes.Factory.EnterDungeon.RoundingBend".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 1)
                    OutputText("Scenes.Factory.EnterDungeon.RoundingBendAgain".Localized());
                else
                    OutputText("Scenes.Factory.EnterDungeon.RoundingBendThird".Localized());

            OutputText("Scenes.Factory.EnterDungeon.NoWindows".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 0)
                OutputText("Scenes.Factory.EnterDungeon.GoInside".Localized());

            OutputText("Scenes.Factory.EnterDungeon.EnterFactoryQuestion".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryFound) < 1)
            {
                OutputText("Scenes.Factory.EnterDungeon.FactoryAccessibleBanner".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.FactoryFound, 1);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Enter".Localized(), StartDungeon);

            AddButton(14, "Common.Button.Leave".Localized(), ExitDungeon);
        }

        public void StartDungeon()
        {

DungeonScene.Enter(DungeonIds.Factory);
        }

        public void ExitDungeon()
        {
            ClearOutput();

            DungeonSession.End();

            ShowImage("dungeon-entrance-factory");

            OutputText("Scenes.Factory.ExitDungeon.SlipsOut".Localized());

            ReturnToCamp(1);
        }

        public void TakeIronKey()
        {
            ClearOutput();

            ShowImage("item-key-iron");

            OutputText("Scenes.Factory.TakeIronKey.TakesIronKey".Localized());

            Player.CreateKeyItem("Iron Key", 0, 0, 0, 0);

            RunFunc();

            RunFunc();
        }

        public void TakeSupervisorKey()
        {
            ClearOutput();

            ShowImage("item-key-silver");

            OutputText("Scenes.Factory.TakeSupervisorKey.SearchesDesk".Localized());

            OutputText("Scenes.Factory.TakeSupervisorKey.TakesSupervisorKey".Localized());

            Player.CreateKeyItem("Supervisor's Key", 0, 0, 0, 0);

            RunFunc();

            RunFunc();
        }

        public void TakeGroPlus()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FactoryTakenGroplus, CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenGroplus) + 1);

            ShowImage("item-gro-plus");

            InventoryScene.TakeItem(ItemIds.GroPlus, RunFunc);
        }

        public void TakeLactaid()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.FactoryTakenLactaid, CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenLactaid) + 1);

            ShowImage("item-lactaid");

            InventoryScene.TakeItem(ConsumableIds.Lactaid, RunFunc);
        }

        public void DrinkCoffee()
        {
            ClearOutput();

            ShowImage("item-coffee");

            OutputText("Scenes.Factory.DrinkCoffee.TakesSip".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 1f));

            Player.HealthChange(35);

            Player.RefillHunger(10);

            DoNext(() => RunFunc());
        }

        public void BuildCockMilker()
        {
            ClearOutput();

            ShowImage("factory-milking-cock");

            OutputText("Scenes.Factory.BuildCockMilker.PuzzlesOut".Localized());

            OutputText("Scenes.Factory.BuildCockMilker.GainedCockMilker".Localized());

            OutputText("Scenes.Factory.BuildCockMilker.NeedLittleHelp".Localized());

            Player.CreateKeyItem("Cock Milker", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.FactoryMilkerBuilt, 1);

            DoNext(() => RunFunc());
        }

        public void BuildBreastMilker()
        {
            ClearOutput();

            ShowImage("factory-milking-breasts");

            OutputText("Scenes.Factory.BuildBreastMilker.PuzzlesOut".Localized());

            OutputText("Scenes.Factory.BuildBreastMilker.GainedBreastMilker".Localized());

            OutputText("Scenes.Factory.BuildBreastMilker.NeedLittleHelp".Localized());

            Player.CreateKeyItem("Breast Milker", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.FactoryMilkerBuilt, 1);

            DoNext(() => RunFunc());
        }

        public void DoTensionRelease()
        {
            ClearOutput();

            int temp = 0;

            if (!Player.HasStatusEffect(StatusEffectIds.TensionReleased))
            {
                if (Player.HasVagina() && Player.HasCock())
                    ShowImage("factory-tension-herm");
                else
                    if (Player.HasVagina() && !Player.HasCock())
                        ShowImage("factory-tension-female");
                    else
                        ShowImage("factory-tension-male");

                OutputText("Scenes.Factory.DoTensionRelease.NodsSteps".Localized());

                if (Player.BiggestBreastSize() < 10)
                    Player.GrowTits(1, (2 + Random.Shared.Next(3)), true, 1);

                OutputText("Scenes.Factory.DoTensionRelease.GlancesPile".Localized());

                Player.Orgasm("Tits");

                Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f), new DynStat(GameStats.Libido, 4f), new DynStat(GameStats.Corruption, 4f));

                Player.SlimeFeed();

                Player.CreateStatusEffect(StatusEffectIds.TensionReleased, 0, 0, 0, 0);
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.TensionReleased, 0) == 0)
                {
                    if (Player.HasVagina() && Player.HasCock())
                        ShowImage("factory-tension-herm");
                    else
                        if (Player.HasVagina() && !Player.HasCock())
                            ShowImage("factory-tension-female");
                        else
                            ShowImage("factory-tension-male");

                    OutputText("Scenes.Factory.DoTensionRelease.PutsOnModified".Localized());

                    if (Player.Cocks.Count > 0)
                    {
                        Player.LengthChange(Player.IncreaseCock(0, 5), Player.Cocks.Count);

                        if (Player.AverageCockLength() >= 9 && Player.AverageCockThickness() < 2)
                        {
                            OutputText("Scenes.Factory.DoTensionRelease.GainsThickness".Localized());

                            temp = Player.Cocks.Count;

                            while (temp > 0)
                            {
                                temp--;

                                if (Player.Cocks[temp].Thickness < 2)
                                    Player.Cocks[temp].Thickness += 1;
                            }
                        }
                        else
                            if (Player.AverageCockLength() >= 15 && Player.AverageCockThickness() < 3)
                            {
                                OutputText("Scenes.Factory.DoTensionRelease.GainsThicknessAgain".Localized());

                                temp = Player.Cocks.Count;

                                while (temp > 0)
                                {
                                    temp--;

                                    if (Player.Cocks[temp].Thickness < 3)
                                        Player.Cocks[temp].Thickness += 1;
                                }
                            }
                    }

                    if (Player.BreastRows.Count == 0)
                    {
                        Player.CreateBreastRow();

                        OutputText("Scenes.Factory.DoTensionRelease.ChestTingles".Localized());
                    }

                    Player.GrowTits(1, (2 + Random.Shared.Next(3)), true, 1);

                    OutputText("Scenes.Factory.DoTensionRelease.YourNipplesPrefix".Localized());

                    if (Player.Cocks.Count > 0)
                        OutputText("Scenes.Factory.DoTensionRelease.AndPrefix".Localized(
                            Player.MultiCockDescript()
                        ));

                    OutputText("Scenes.Factory.DoTensionRelease.BecomeRockHard".Localized());

                    if (Player.Cocks.Count > 0 && Player.Vaginas.Count == 0)
                        OutputText("Scenes.Factory.DoTensionRelease.GlancingSea".Localized());

                    if (Player.Vaginas.Count > 0 && Player.Cocks.Count == 0)
                    {
                        OutputText("Scenes.Factory.DoTensionRelease.EntersCrazed".Localized(
                            Player.VaginaDescript(0)
                        ));

                        Player.CuntChange(150, true);
                    }

                    if (Player.Vaginas.Count > 0 && Player.Cocks.Count > 0)
                        OutputText("Scenes.Factory.DoTensionRelease.GettingMilked".Localized(
                            Player.MultiCockDescript()
                        ));

                    OutputText("Scenes.Factory.DoTensionRelease.EightHoursLost".Localized());

                    Player.Orgasm("Generic");

                    Player.ApplyStats(new DynStat(GameStats.Intelligence, -2f), new DynStat(GameStats.Libido, 4f), new DynStat(GameStats.Corruption, 4f));

                    Player.CreateStatusEffect(StatusEffectIds.TensionReleased, 0, 0, 0, 0);

                    Player.ChangeStatusValue(StatusEffectIds.TensionReleased, 0, 1);

                    Player.SlimeFeed();
                }
                else
                {
                    DoBadEndTension();

                    return ;
                }

            DoNext(() => RunFunc());
        }

        public void FactoryShutdown()
        {
            ClearOutput();

            ShowImage("factory-closed-shutdown");

            OutputText("Scenes.Factory.FactoryShutDown.ResolveShutDown".Localized());

            OutputText("Scenes.Factory.FactoryShutDown.RememberDestroy".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FactoryShutdown, 1);

            DoNext(() => RunFunc());
        }

        public void FactoryOverload()
        {
            ClearOutput();

            ShowImage("factory-closed-overload");

            OutputText("Scenes.Factory.FactoryOverload.ResolveShutDown".Localized());

            OutputText("Scenes.Factory.FactoryOverload.RememberDestroy".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FactoryShutdown, 2);

            DoNext(() => RunFunc());
        }

        public void DoBadEndGeneric()
        {
            ClearOutput();

            OutputText("Scenes.Factory.DoBadEndGeneric.CracksEyes".Localized());

            if (Player.Cocks.Count == 0 || Player.BiggestBreastSize() <= 1)
            {
                OutputText("Scenes.Factory.DoBadEndGeneric.VoicePipesUp".Localized());

                if (Player.Cocks.Count == 0)
                {
                    OutputText("Scenes.Factory.DoBadEndGeneric.AnythingGroin".Localized());

                    if (Player.BiggestBreastSize() <= 1)
                        OutputText("Scenes.Factory.DoBadEndGeneric.OrFragment".Localized());
                }

                if (Player.BiggestBreastSize() <= 1)
                    OutputText("Scenes.Factory.DoBadEndGeneric.ChestAdornments".Localized());

                OutputText("Scenes.Factory.DoBadEndGeneric.VoiceTrailsOff".Localized());
            }
            else
                OutputText("Scenes.Factory.DoBadEndGeneric.VoiceTries".Localized());

            OutputText("Scenes.Factory.DoBadEndGeneric.RealizeVoice".Localized());

            OutputText("Scenes.Factory.DoBadEndGeneric.SuchHorny".Localized());

            if (Player.BreastRows.Count() == 2)
                OutputText("Scenes.Factory.DoBadEndGeneric.PairOfWord".Localized());
            else
                OutputText("Scenes.Factory.DoBadEndGeneric.MultitudeOfWord".Localized());

            OutputText("Scenes.Factory.DoBadEndGeneric.ClearHoses".Localized());

            if (Player.BiggestLactation() <= 1.5f)
                OutputText("Scenes.Factory.DoBadEndGeneric.CreamyMilk".Localized());
            else
                OutputText("Scenes.Factory.DoBadEndGeneric.HosesBulge".Localized());

            ShowImage("badend-factory");

            OutputText("Scenes.Factory.DoBadEndGeneric.WantonMoans".Localized());

            OutputText("Scenes.Factory.DoBadEndGeneric.SnapEchoes".Localized());

            OutputText("Scenes.Factory.DoBadEndGeneric.GirlSquirms".Localized());

            OutputText("Scenes.Factory.DoBadEndGeneric.TubeLining".Localized());
            // 原版 Factory.hx:355 — AikoScene.hx:1889 / FactoryScene.hx 等：Marble 是跟随者
            if (SceneInstance<Npcs.MarbleScene>.Get().MarbleFollower())
                OutputText("Scenes.Factory.DoBadEndGeneric.ReadingThoughts".Localized());
            else
                OutputText("Scenes.Factory.DoBadEndGeneric.ReadingThoughtsAgain".Localized());

            OutputText("Scenes.Factory.DoBadEndGeneric.TreatmentContinues".Localized());

            GameOver();
        }

        public void DoBadEndTension()
        {

            if (Player.HasVagina() && Player.HasCock())
                ShowImage("factory-tension-herm");
            else
                if (Player.HasVagina() && !Player.HasCock())
                    ShowImage("factory-tension-female");
                else
                    ShowImage("factory-tension-male");

            OutputText("Scenes.Factory.DoBadEndTension.DesperateMore".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Factory.DoBadEndTension.FillingYour".Localized());

                OutputText("Scenes.Factory.DoBadEndTension.WithSensation".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Factory.DoBadEndTension.SWord".Localized());

                OutputText("Scenes.Factory.DoBadEndTension.AsFragment".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Factory.DoBadEndTension.TheyWord".Localized());
                else
                    OutputText("Scenes.Factory.DoBadEndTension.ItWord".Localized());

                OutputText("Scenes.Factory.DoBadEndTension.GrowWord".Localized());

                if (Player.Cocks.Count == 1)
                    OutputText("Scenes.Factory.DoBadEndTension.SWordAgain".Localized());

                OutputText("Scenes.Factory.DoBadEndTension.MassiveEngorged".Localized());
            }
            else
                OutputText("Scenes.Factory.DoBadEndTension.SentencePeriod".Localized());

            OutputText("Scenes.Factory.DoBadEndTension.ThrobBecoming".Localized());

            if (Player.Vaginas.Count > 0)
                OutputText("Scenes.Factory.DoBadEndTension.PussySoaked".Localized());

            OutputText("Scenes.Factory.DoBadEndTension.DesireForMore".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -25f), new DynStat(GameStats.Lust, -75f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => BadEndTension());
        }

        public void BadEndTension()
        {
            ClearOutput();

            ShowImage("badend-tension");

            OutputText("Scenes.Factory.BadEndTension.InTimeWake".Localized());

            if (Player.BiggestLactation() >= 3)
                OutputText("Scenes.Factory.BadEndTension.MooWord".Localized());
            else
                OutputText("Scenes.Factory.BadEndTension.CryWord".Localized());

            OutputText("Scenes.Factory.BadEndTension.WithHappiness".Localized());

            Player.ApplyStats(new DynStat(GameStats.Intelligence, -75f), new DynStat(GameStats.Libido, 100f), new DynStat(GameStats.Corruption, 3f));

            GameOver();
        }

        public void RoomLobby()
        {
            ClearOutput();

            ShowImage("factory-foyer");

            OutputText("Scenes.Factory.RoomLobby.FoyerBanner".Localized());

            OutputText("Scenes.Factory.RoomLobby.DoorSwingsShut".Localized());

            AddButton(11, "Common.Button.Leave".Localized(), ExitDungeon);
        }

        public void RoomBreakRoom()
        {
            ClearOutput();

            ShowImage("factory-break-room");

            OutputText("Scenes.Factory.RoomBreakRoom.BreakRoomBanner".Localized());

            OutputText("Scenes.Factory.RoomBreakRoom.SteppingThrough".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactorySuccubusDefeated) > 0)
            {
                if (!Player.HasKeyItem("Iron Key"))
                {
                    OutputText("Scenes.Factory.RoomBreakRoom.OpponentDropped".Localized());

                    AddButton(0, "Common.Button.IronKey".Localized(), TakeIronKey);
                }

                AddButton(1, "Common.Button.Coffee".Localized(), DrinkCoffee);
            }
            else
                SceneInstance<SecretarialSuccubusScene>.Get().EncounterSuccubus();
        }

        public void RoomPumpRoom()
        {
            ClearOutput();

            ShowImage("factory-pump-room");

            OutputText("Scenes.Factory.RoomPumpRoom.PumpRoomBanner".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) < 1)
                OutputText("Scenes.Factory.RoomPumpRoom.StepsThroughIron".Localized());
            else
                OutputText("Scenes.Factory.RoomPumpRoom.StepsThroughIronAgain".Localized());
        }

        public void RoomFurnaceRoom()
        {
            ClearOutput();

            ShowImage("factory-furnace-room");

            OutputText("Scenes.Factory.RoomFurnaceRoom.FurnaceRoomBanner".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                OutputText("Scenes.Factory.RoomFurnaceRoom.AirInside".Localized());
            else
                OutputText("Scenes.Factory.RoomFurnaceRoom.MachineryShutDown".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryIncubusDefeated) <= 0 && CoC.Instance.Flags.Get(GameFlagIds.D3Discovered) == 0)
            {

            }
        }

        public void RoomRepairCloset()
        {
            ClearOutput();

            ShowImage("factory-repair-closet");

            OutputText("Scenes.Factory.RoomRepairCloset.RepairClosetBanner".Localized());

            OutputText("Scenes.Factory.RoomRepairCloset.SlipsInside".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryMilkerBuilt) > 0)
                OutputText("Scenes.Factory.RoomRepairCloset.ShelvesEmpty".Localized());
            else
            {
                OutputText("Scenes.Factory.RoomRepairCloset.CabinetHolds".Localized());

                if (Player.Intelligence >= 40)
                {
                    OutputText("Scenes.Factory.RoomRepairCloset.EnoughPieces".Localized());

                    if (Player.HasKeyItem("Cock Milker"))
                        OutputText("Scenes.Factory.RoomRepairCloset.AlreadyHaveCock".Localized());
                    else
                        AddButton(1, "Common.Button.CockMilker".Localized(), BuildCockMilker);

                    if (Player.HasKeyItem("Breast Milker"))
                        OutputText("Scenes.Factory.RoomRepairCloset.AlreadyHaveBreast".Localized());
                    else
                        AddButton(0, "Common.Button.BreastMilker".Localized(), BuildBreastMilker);
                }
            }

            OutputText("Scenes.Factory.RoomRepairCloset.OnlyExitBack".Localized());
        }

        public void RoomMainChamber()
        {
            ClearOutput();

            ShowImage("factory-main-chamber");

            OutputText("Scenes.Factory.RoomMainChamber.MainChamberBanner".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
            {
                OutputText("Scenes.Factory.RoomMainChamber.CavernousChamber".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Factory.RoomMainChamber.WishCouldFree".Localized());
                else
                    OutputText("Scenes.Factory.RoomMainChamber.WishHadMachinery".Localized());

                OutputText("Scenes.Factory.RoomMainChamber.DoorwayTo".Localized());
            }
            else
            {
                OutputText("Scenes.Factory.RoomMainChamber.SignificantlyEmptier".Localized());

                OutputText("Scenes.Factory.RoomMainChamber.LeatherCladLadies".Localized());

                AddButton(0, "Common.Button.Tension".Localized(), DoTensionRelease);
            }

        }

        public void RoomForemanOffice()
        {
            ClearOutput();

            OutputText("Scenes.Factory.RoomForemanOffice.ForemanOfficeBanner".Localized());

            ShowImage("factory-foreman-office");

            OutputText("Scenes.Factory.RoomForemanOffice.ExcellentView".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryOmnibusDefeated) <= 0)
                SceneInstance<OmnibusOverseerScene>.Get().EncounterOmnibus();
            else
                if (!Player.HasKeyItem("Supervisor's Key"))
                    AddButton(0, "Cabin.Furniture.Desk.Name".Localized(), TakeSupervisorKey);
        }

        public void RoomControlRoom()
        {
            ClearOutput();

            ShowImage("factory-control-room");

            OutputText("Scenes.Factory.RoomControlRoom.PumpControlBanner".Localized());

            OutputText("Scenes.Factory.RoomControlRoom.LittleMoreThan".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
            {
                OutputText("Scenes.Factory.RoomControlRoom.SpendsMoment".Localized());

                OutputText("Scenes.Factory.RoomControlRoom.CloseStorageVent".Localized());

                OutputText("Scenes.Factory.RoomControlRoom.SystemShutdown".Localized());

                OutputText("Scenes.Factory.RoomControlRoom.LeaveEquipment".Localized());
            }
            else
                OutputText("Scenes.Factory.RoomControlRoom.ControlsInoperable".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
            {
                AddButton(0, "Common.Button.Valves".Localized(), FactoryOverload);

                AddButton(1, "Common.Button.Shutdown".Localized(), FactoryShutdown);
            }
        }

        public void RoomPremiumStorage()
        {
            ClearOutput();

            ShowImage("factory-storage");

            OutputText("Scenes.Factory.RoomPremiumStorage.PremiumProductsBanner".Localized());

            OutputText("Scenes.Factory.RoomPremiumStorage.StoreRoomFilled".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenLactaid) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenLactaid) < 5)
                {
                    OutputText("Scenes.Factory.RoomPremiumStorage.CrateBottles".Localized(
                        NumberUtils.Num2Text(5 - CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenLactaid))
                    ));

                    AddButton(0, "Items.Lactaid.Short".Localized(), TakeLactaid);
                }
            }
            else
            {
                OutputText("Scenes.Factory.RoomPremiumStorage.UnopenedCrate".Localized());

                AddButton(0, "Items.Lactaid.Short".Localized(), TakeLactaid);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenGroplus) > 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenGroplus) < 5)
                {
                    OutputText("Scenes.Factory.RoomPremiumStorage.CrateBottlesAgain".Localized(
                        NumberUtils.Num2Text(5 - CoC.Instance.Flags.Get(GameFlagIds.FactoryTakenGroplus))
                    ));

                    AddButton(1, "Common.Button.Groplus".Localized(), TakeGroPlus);
                }
            }
            else
            {
                OutputText("Scenes.Factory.RoomPremiumStorage.UnopenedCrateAgain".Localized());

                AddButton(1, "Items.GroPlus.Short".Localized(), TakeGroPlus);
            }
        }

        public void RoomBathroom()
        {
            ClearOutput();

            ShowImage("factory-washroom");

            OutputText("Scenes.Factory.RoomBathroom.WashroomBanner".Localized());

            OutputText("Scenes.Factory.RoomBathroom.FairlyClean".Localized());
        }

    }
}