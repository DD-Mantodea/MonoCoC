using MonoCoC.Attributes;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Collections.Generic;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla.Dungeons
{
    [Register<LiddelliumEventDungeonScene>("coc:forest_liddellium_event_dungeon")]
    public class LiddelliumEventDungeonScene : Scene
    {
        private bool _beatenCamp;

        private int _impFoodTaken;

        private bool _takenAxe;

        private int _spearsTaken;

        private int _swordsTaken;

        private int _shieldsTaken;

        private bool _takenPhial;

        private int _whipsLeft = 3;

        private bool _takenBStrap;

        private int _iDraftLeft = 5;

        private int _sMilkLeft = 5;

        private bool _takenLPop;

        public override void Entry() => EncounterImps();

        public void EncounterImps()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.EncounterImps.HaltsEarsPerk".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.EncounterImps.JustImps".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Fight".Localized(), FightImps);

            AddButton(1, "Common.Button.Sneak".Localized(), SneakImps);

            AddButton(2, "Common.Button.Leave".Localized(), () => ReturnToCamp(1));
        }

        public void FightImps()
        {
            OutputText("Scenes.Liddelliumeventdungeon.FightImps.ToHellWithThem".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.FightImps.ImpsShriek".Localized());

            if (Flags.Get(GameFlagIds.LethiceDefeated) > 0)
            {
                OutputText("Scenes.Liddelliumeventdungeon.FightImps.ChampionShout".Localized());

                OutputText("Scenes.Liddelliumeventdungeon.FightImps.GroupScatters".Localized());

                DoNext(DemonCampScene);

                return;
            }

            OutputText("Scenes.Liddelliumeventdungeon.FightImps.AssemblesReady".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "imp_horde")), DemonCampScene);
        }

        public void SneakImps()
        {
            OutputText("Scenes.Liddelliumeventdungeon.SneakImps.NotWorthIt".Localized());

            DoNext(DemonCampScene);
        }

        public void DemonCampScene()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.DemonCampScene.GuardedCamp".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.DemonCampScene.AboutResidents".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Massacre".Localized(), Massacre);

            AddButton(1, "Common.Button.Thievery".Localized(), CampThievery);

            AddButton(2, "Common.Button.Leave".Localized(), CampLeave);
        }

        public void CampLeave()
        {
            OutputText("Scenes.Liddelliumeventdungeon.CampLeave.ManyOpportunities".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void CampThievery()
        {
            ClearOutput();

            var text = "Scenes.Liddelliumeventdungeon.CampThievery.NeverKnow".Localized();

            if (Player.HasPerk(PerkIds.Incorporeality))
                text += "Scenes.Liddelliumeventdungeon.CampThievery.LikeAGhost".Localized();

            if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
            {
                // 原版 LiddelliumEventDungeon.hx:73-78：先播固定台词，再按 silly 开关追加一句
                text += "Scenes.Liddelliumeventdungeon.CampThievery.PossessedByGhost".Localized();

                text += (Silly

                    ? "Scenes.Liddelliumeventdungeon.CampThievery.PossessedByGhostSilly"

                    : "Scenes.Liddelliumeventdungeon.CampThievery.PossessedByGhostNormal").Localized();
            }

            text += "Scenes.Liddelliumeventdungeon.CampThievery.MainEntrance".Localized();

            if (Player.HasTailInsteadOfLegs())
                if (Player.HasPerk(PerkIds.Incorporeality))
                    text += "Scenes.Liddelliumeventdungeon.CampThievery.EnoughSimiles".Localized();
                else
                    text += "Scenes.Liddelliumeventdungeon.CampThievery.LikeASnake".Localized();

            text += "Scenes.Liddelliumeventdungeon.CampThievery.WindowOpens".Localized();

            OutputText(text);

            DoNext(ImpRoom);
        }

        public void ImpRoom()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.ImpRoom.ImpBedroom".Localized());

            ClearButtons();

            if (_impFoodTaken < 30)
                AddButton(0, "Items.ImpFood.Short".Localized(), TakeImpFood);
            else
                AddButtonDisabled(0, "Imp Food", "The imps kept rations of food stored here. Monster.");

            AddButton(1, "Common.Button.NextBuilding".Localized(), _beatenCamp ? () => CampMenu("ImpRoom") : NextBuilding);
        }

        public void TakeImpFood()
        {
            ClearOutput();

            var taken = _impFoodTaken++;

            switch (taken)
            {
                case 0:
                    OutputText("Scenes.Liddelliumeventdungeon.TakeImpFood.PlentyRations".Localized());

                    break;

                case 1:
                    OutputText("Scenes.Liddelliumeventdungeon.TakeImpFood.JustHungry".Localized());

                    break;

                case 2:
                    OutputText("Scenes.Liddelliumeventdungeon.TakeImpFood.ThieveryFetish".Localized());

                    break;

                case 3:
                    OutputText("Scenes.Liddelliumeventdungeon.TakeImpFood.ImpsStarve".Localized());

                    break;

                default:
                    if (_impFoodTaken > 3 && _impFoodTaken < 30)
                        OutputText("Scenes.Liddelliumeventdungeon.TakeImpFood.LavishGarbage".Localized());
                    else
                        OutputText("Scenes.Liddelliumeventdungeon.TakeImpFood.DeprivedImps".Localized());

                    break;
            }

            DoNext(ImpRoom);
        }

        public void NextBuilding()
        {
            OutputText("Scenes.Liddelliumeventdungeon.NextBuilding.PeersOutDoorway".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.DemonLordsHouse".Localized(), DemonLordHouse);

            AddButton(1, "Common.Button.Leave".Localized(), QuitWhileAhead);
        }

        public void QuitWhileAhead()
        {
            OutputText("Scenes.Liddelliumeventdungeon.QuitWhileAhead.RuinedDay".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DemonLordHouse()
        {
            ClearOutput();

            var text = "Scenes.Liddelliumeventdungeon.DemonLordHouse.Nonchalant".Localized();

            if (Player.Tallness > 86 || Player.Thickness > 70)
            {
                text += "Scenes.Liddelliumeventdungeon.DemonLordHouse.RushDucking".Localized();

                text += Player.Tallness > 86

                    ? "Scenes.Liddelliumeventdungeon.DemonLordHouse.Giants".Localized()

                    : "Scenes.Liddelliumeventdungeon.DemonLordHouse.Chubbies".Localized();

                text += "Scenes.Liddelliumeventdungeon.DemonLordHouse.DontMakeRogues".Localized();

                OutputText(text);

                DoNext(DemonCampBattle);

                return;
            }

            text += "Scenes.Liddelliumeventdungeon.DemonLordHouse.DashThrough".Localized();

            OutputText(text);

            DoNext(HaremHouse);
        }

        public void HaremHouse()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.HaremHouse.HaremHouse".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.HaremHouse.PeopleFucking".Localized());

            Player.TakeLustDamage(15);

            ClearButtons();

            AddButton(0, "Common.Button.Steal".Localized(), HaremHouseSteal);

            AddButton(1, "Common.Button.Fight".Localized(), () => HaremHouseBattle(true));
        }

        public void HaremHouseSteal()
        {
            var text = "Scenes.Liddelliumeventdungeon.HaremHouseSteal.SidlingThrough".Localized();

            if (Player.Tallness > 86 || Player.Thickness > 70)
            {
                text += "Scenes.Liddelliumeventdungeon.HaremHouseSteal.PlanFails".Localized();

                OutputText(text);

                DoNext(() => HaremHouseBattle());

                return;
            }

            text += "Scenes.Liddelliumeventdungeon.HaremHouseSteal.SaunteringPast".Localized();

            if (Player.Tallness < 60)
                text += "Scenes.Liddelliumeventdungeon.HaremHouseSteal.HandFallsShort".Localized();

            text += "Scenes.Liddelliumeventdungeon.HaremHouseSteal.LargeMetalObjects".Localized();

            if (Player.Tallness > 59)
            {
                text += "Scenes.Liddelliumeventdungeon.HaremHouseSteal.ScreechPierces".Localized();

                OutputText(text);

                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Liddellium), 1);

                DoNext(() => HaremHouseBattle());

                return;
            }

            text += "Scenes.Liddelliumeventdungeon.HaremHouseSteal.ReachWeaponry".Localized();

            OutputText(text);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Liddellium), 1);

            DoNext(HaremHouseWeaponRack);
        }

        public void HaremHouseWeaponRack()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.HaremHouseWeaponRack.WeaponRack".Localized());

            if (_takenAxe && _spearsTaken == 2 && _swordsTaken == 4 && _shieldsTaken == 4)
                OutputText("Scenes.Liddelliumeventdungeon.HaremHouseWeaponRack.ClearedRack".Localized());

            ClearButtons();

            if (_takenAxe)
                AddButtonDisabled(0, "Large Axe", "You already took it!");
            else
                AddButton(0, "Items.LargeAxe.Short".Localized(), TakeAxe);

            if (_spearsTaken >= 2)
                AddButtonDisabled(1, "Spear", "You've already taken the spears!");
            else
                AddButton(1, "Items.Spear.Short".Localized(), TakeSpear);

            if (_swordsTaken >= 4)
                AddButtonDisabled(2, "Sword", "You've taken all four katanas.");
            else
                AddButton(2, "Common.Button.Sword".Localized(), TakeSword);

            if (_shieldsTaken >= 4)
                AddButtonDisabled(3, "Shield", "There are no more shields on display.");
            else
                AddButton(3, "Scenes.Inventory.EquipmentShield".Localized(), TakeShield);

            AddButton(4, "Common.Button.MoveAlong".Localized(), MoveAlong);
        }

        public void TakeAxe()
        {
            OutputText("Scenes.Liddelliumeventdungeon.TakeAxe.CouldDemonsUse".Localized());

            if (Player.Tallness > 78 || Player.Strength >= 90)
            {
                OutputText("Scenes.Liddelliumeventdungeon.TakeAxe.AddsAxe".Localized());

                _takenAxe = true;

                Player.Inventory.Add(ItemRegistry.Create(WeaponIds.LargeAxe), 1);

                DoNext(HaremHouseWeaponRack);

                return;
            }

            OutputText("Scenes.Liddelliumeventdungeon.TakeAxe.HeavesWeapon".Localized());

            DoNext(HaremHouseWeaponRack);
        }

        public void TakeSpear()
        {
            OutputText("Scenes.Liddelliumeventdungeon.TakeSpear.FinelyCrafted".Localized());

            _spearsTaken += 1;

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Spear), 1);

            DoNext(HaremHouseWeaponRack);
        }

        public void TakeSword()
        {
            OutputText(_swordsTaken == 0

                ? "Scenes.Liddelliumeventdungeon.TakeSword.FourKatanas".Localized()

                : "Scenes.Liddelliumeventdungeon.TakeSword.TakesKatana".Localized());

            _swordsTaken += 1;

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Katana), 1);

            DoNext(HaremHouseWeaponRack);
        }

        public void TakeShield()
        {
            OutputText("Scenes.Liddelliumeventdungeon.TakeShield.GrabsKiteShield".Localized());

            _shieldsTaken += 1;

            Player.Inventory.Add(ItemRegistry.Create(new Identifier("coc", "kite_shield")), 1);

            DoNext(HaremHouseWeaponRack);
        }

        public void MoveAlong()
        {
            OutputText("Scenes.Liddelliumeventdungeon.MoveAlong.TalesOfYou".Localized());

            Flags.Set(GameFlagIds.LiddelliumDungeonFlag, 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void HaremHouseBattle(bool delay = false)
        {
            if (!delay)
            {
                StartHaremBattle();

                return;
            }

            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.HaremHouseBattle.TimeToDismantle".Localized());

            StartHaremBattle();
        }

        private void StartHaremBattle()
        {
            var enemies = new List<Enemy>
            {
                EnemyRegistry.Create(new Identifier("coc", "random_succubus")),

                EnemyRegistry.Create(new Identifier("coc", "random_succubus")),
            };

            CombatScene.StartMultiple(enemies, HaremBattleWin);
        }

        public void HaremBattleWin()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.HaremBattleWin.DemonicPairFall".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.WeaponRack".Localized(), HaremHouseWeaponRack);

            if (!_takenPhial)
                AddButton(1, "Common.Button.Phial".Localized(), GetPhial);

            AddButton(2, "Common.Button.Leave".Localized(), HaremLeave);
        }

        public void GetPhial()
        {
            var text = "Scenes.Liddelliumeventdungeon.GetPhial.PhialOnShelf".Localized();

            if (Player.Tallness < 60)
                text += "Scenes.Liddelliumeventdungeon.GetPhial.HandFallsShort".Localized();

            OutputText(text);

            _takenPhial = true;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Liddellium), 1);

            DoNext(HaremBattleWin);
        }

        public void HaremLeave()
        {
            OutputText("Scenes.Liddelliumeventdungeon.HaremLeave.DenHoldsNothing".Localized());

            Flags.Set(GameFlagIds.LiddelliumDungeonFlag, 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void HaremBattleLose()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.HaremBattleLose.PairTooMuch".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.HaremBattleLose.SuccubusCackles".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.HaremBattleLose.PullsHerself".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.HaremBattleLose.UncorksBottle".Localized());

            DoNext(HaremBattleLose2);
        }

        public void HaremBattleLose2()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.HaremBattleLose2.AwakesWooden".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.HaremBattleLose2.BodyWeak".Localized());

            GameOver();
        }

        public void Massacre()
        {
            OutputText("Scenes.Liddelliumeventdungeon.Massacre.LeapsIntoAction".Localized());

            DemonCampBattle();
        }

        public void DemonCampBattle()
        {
            var enemies = new List<Enemy>
            {
                EnemyRegistry.Create(new Identifier("coc", "random_succubus")),

                EnemyRegistry.Create(new Identifier("coc", "random_succubus")),

                EnemyRegistry.Create(new Identifier("coc", "random_succubus")),

                EnemyRegistry.Create(new Identifier("coc", "imp_horde")),
            };

            CombatScene.StartMultiple(enemies, CampVictory);
        }

        public void CampLoss()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.CampLoss.KnewWhatGetting".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.CampLoss.SuccubusSmug".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.CampLoss.SuccubiBanter".Localized());

            GameOver();
        }

        public void CampVictory()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.CampVictory.LastMenace".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.CampVictory.PoorShed".Localized());

            OutputText("Scenes.Liddelliumeventdungeon.CampVictory.LargerSide".Localized());

            _beatenCamp = true;

            CampMenu();
        }

        public void CampMenu(string from = "")
        {
            if (from != "")
            {
                ClearOutput();

                switch (from)
                {
                    case "Quarters":
                        OutputText("Scenes.Liddelliumeventdungeon.CampMenu.SeenAllNeed".Localized());

                        break;

                    case "ImpRoom":
                        OutputText("Scenes.Liddelliumeventdungeon.CampMenu.NothingValuable".Localized());

                        break;

                    case "Lab":
                        OutputText("Scenes.Liddelliumeventdungeon.CampMenu.WhatShame".Localized());

                        break;

                    case "Workshop":
                        OutputText("Scenes.Liddelliumeventdungeon.CampMenu.PointlessHeartwarming".Localized());

                        break;
                }
            }

            ClearButtons();

            AddButton(0, "Common.Button.LeadersHome".Localized(), HaremHouse);

            AddButton(1, "Common.Button.ImpBarracks".Localized(), ImpRoom);

            AddButton(2, "Common.Button.Lab".Localized(), Lab);

            AddButton(3, "Common.Button.Workshop".Localized(), Workshop);

            AddButton(4, "Common.Button.Quarters".Localized(), Quarters);
        }

        public void Quarters()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.Quarters.LikeBuildings".Localized());

            ClearButtons();

            if (_whipsLeft > 0)
                AddButton(0, "Common.Button.LeatherWhip".Localized(), TakeWhip);

            if (!_takenBStrap)
                AddButton(1, "Common.Button.BondageStrap".Localized(), TakeBStrap);

            AddButton(2, "Common.Button.Leave".Localized(), () => CampMenu("Quarters"));
        }

        public void TakeWhip()
        {
            _whipsLeft--;

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Whip), 1);

            DoNext(Quarters);
        }

        public void TakeBStrap()
        {
            _takenBStrap = true;

            OutputText("Scenes.Liddelliumeventdungeon.TakeBStrap.TakesBondageStrap".Localized());

            Player.Inventory.Add(ItemRegistry.Create(ArmorIds.BondageStraps), 1);

            DoNext(Quarters);
        }

        public void Lab()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.Lab.UnkemptLab".Localized());

            ClearButtons();

            if (_iDraftLeft > 0)
                AddButton(0, "Common.Button.IncubusDraft".Localized(), TakeIDraft);

            if (_sMilkLeft > 0)
                AddButton(1, "Common.Button.SuccubusMilk".Localized(), TakeSMilk);

            if (!_takenLPop)
                AddButton(2, "Items.Lolipop.Short".Localized(), TakeLPop);

            AddButton(3, "Common.Button.Leave".Localized(), () => CampMenu("Lab"));
        }

        public void TakeLPop()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.TakeLPop.GlossySheen".Localized());

            _takenLPop = true;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Lollipop), 1);

            DoNext(Lab);
        }

        public void TakeIDraft()
        {
            _iDraftLeft--;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.IncubiDraft), 1);

            DoNext(Lab);
        }

        public void TakeSMilk()
        {
            _sMilkLeft--;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.SuccubiMilk), 1);

            DoNext(Lab);
        }

        public void Workshop()
        {
            ClearOutput();

            OutputText("Scenes.Liddelliumeventdungeon.Workshop.NoFrontWall".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Leave".Localized(), () => CampMenu("Workshop"));
        }
    }
}