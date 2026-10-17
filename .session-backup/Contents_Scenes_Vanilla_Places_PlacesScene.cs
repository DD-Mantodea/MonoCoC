// 原版 classes/scenes/Camp.hx:2739-2880 的 PLACES MENU（placesCount / places）。
// 菜单按原版 places() 的 ButtonDataList 语义动态排列：条件不满足的地点**直接不显示**（原版是 if 内的 buttons.add），
// 只有「条件满足但暂时不可进」的情况才置灰（Moth Cave 的 Dolores 冷却、Tel'Adre 通缉状态）。
// 各地点状态分类：
//   - 已接线：Bazaar / Boat(+Swim) / Farm / Great Tree(Aiko) / Moth Cave / Owca / Oasis Tower(Minerva) /
//             Salon / Tel'Adre / Town Ruins
//   - 场景类不存在：Cathedral（无 Gargoyle/Cathedral 类）、Lumi's Lab（无 Lumi 类）→ 条件满足时置灰说明
//   - 子菜单未移植：Dungeons（DungeonScene 类存在，但 Camp.hx:2801-2803 的 dungeons 子菜单没有）
//   - 类存在但本批未接：Nephila Coven（NephilaCovenScene.EncounterNephilaCoven 存在）
// 入口链：Camp.cs 的「Places」按钮 → PlacesScene.Entry()。
using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag;
using MonoCoC.Contents.Scenes.Vanilla.Factory;
using MonoCoC.Contents.Scenes.Vanilla.Hel;
using MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar;
using MonoCoC.Contents.Scenes.Vanilla.Places.Lake;
using MonoCoC.Extensions;
using TelAdreScene = MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre;

namespace MonoCoC.Contents.Scenes.Vanilla.Places
{
    [Register("coc:places")]
    public class PlacesScene : Scene
    {
        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            // 原版 Camp.hx:2782 images.showImage("camp-pathfinder")
            ShowImage("camp-pathfinder");

            // 原版 Camp.hx:2783
            OutputText("Scenes.Places.Intro".Localized());

            // 原版 places() 用 ButtonDataList 顺序 add；条件不满足的不进列表，所以这里用动态槽位压缩排列。
            var slot = 0;

            // 按钮：Bazaar（原版 Camp.hx:2788-2790；when: BAZAAR_ENTERED > 0）
            if (Flags.Get(VanillaFlags.BazaarEntered) > 0)
                AddButton(slot++, "Bazaar", () => SceneInstance<BazaarScene>.Get().EnterTheBazaar(),
                    "Visit the Bizarre Bazaar, where demons and corrupted beings hang out.");

            // 按钮：Boat（原版 Camp.hx:2791-2793；when: BoatDiscovery）
            // Camp 的 Places 按钮本身就要求 BoatDiscovery（Camp.cs），这里保持原版的隐藏语义。
            if (Player.HasStatusEffect(VanillaStatusEffects.BoatDiscovery))
                AddButton(slot++, "Boat", () => SceneInstance<BoatScene>.Get().BoatExplore(),
                    "Get on the boat and explore the lake.[pg]Recommended level: 4");

            // 按钮：Cathedral（原版 Camp.hx:2794-2800；when: FOUND_CATHEDRAL > 0）
            // 场景类不存在：条件满足时置灰说明，不隐藏（否则玩家不知道那里有内容）。
            if (Flags.Get(VanillaFlags.FoundCathedral) > 0)
                AddButtonDisabled(slot++, "Cathedral",
                    "Scene class not ported: original Camp.hx:2794-2800 (game.gargoyle.*); no Gargoyle/Cathedral scene class exists in C#.");

            // 按钮：Dungeons（原版 Camp.hx:2801-2803；when: dungeonFound() || debug）
            // 子菜单本体照原版 dungeons()（Camp.hx:2862-2889）落地，见 DungeonsMenu()。
            if (DungeonFound())
                AddButton(slot++, "Dungeons", DungeonsMenu, "Delve into dungeons.");

            // 按钮：Farm（原版 Camp.hx:2804-2806；when: farmFound()）
            // farmFound 的「腐化追随者」分支依赖未移植的 FarmCorruption（原版 Camp.hx:2710-2714）；正常发现路径用 FarmEnabled()。
            if (SceneInstance<FarmScene>.Get().FarmEnabled())
                AddButton(slot++, "Farm", () => SceneInstance<FarmScene>.Get().FarmExploreEncounter(),
                    "Visit Whitney's farm near the lake.");

            // 按钮：Great Tree（原版 Camp.hx:2807-2809；when: AIKO_TIMES_MET > 3）
            if (Flags.Get(VanillaFlags.AikoTimesMet) > 3)
                AddButton(slot++, "Great Tree", () => SceneInstance<AikoScene>.Get().EncounterAiko(),
                    "Visit the Great Tree in the Deep Woods where Aiko lives.");

            // 按钮：Lumi's Lab（原版 Camp.hx:2810-2812；when: LUMI_MET > 0）
            // 场景类不存在：条件满足时置灰说明。
            if (Flags.Get(VanillaFlags.LumiMet) > 0)
                AddButtonDisabled(slot++, "Lumi's Lab",
                    "Scene class not ported: original Camp.hx:2810-2812 (game.lumi.lumiEncounter); no Lumi scene class exists in C#.");

            // 按钮：Moth Cave（原版 Camp.hx:2813-2819；when: sylviaProg >= 4）
            // 条件满足后，Dolores 刚产卵（doloresProg==7 && doloresTime<6）时置灰。
            if (SceneInstance<SylviaScene>.Get().SylviaProg >= 4)
            {
                if (MonoCoC.Contents.Scenes.Vanilla.Places.MothCave.MothCaveScene.DoloresProg == 7
                    && MonoCoC.Contents.Scenes.Vanilla.Places.MothCave.MothCaveScene.DoloresTime < 6)
                    AddButtonDisabled(slot++, "Moth Cave", "You should probably give them some time.", "Moth Cave");
                else
                    AddButton(slot++, "Moth Cave",
                        () => SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.MothCave.MothCaveScene>.Get().EncounterCave(),
                        "Visit Sylvia's home.");
            }

            // 按钮：Owca（原版 Camp.hx:2820-2822；when: OWCA_UNLOCKED == 1）
            // hint 里按 OWCA_SACRIFICE_DISABLED 选 "are/were"。
            if (Flags.Get(VanillaFlags.OwcaUnlocked) == 1)
            {
                var sacrificeDisabled = Flags.Get(VanillaFlags.OwcaSacrificeDisabled) > 0;

                AddButton(slot++, "Owca", () => SceneInstance<OwcaScene>.Get().GangbangVillageStuff(),
                    "Visit the sheep village of Owca, where the villagers "
                    + (sacrificeDisabled ? "were" : "are")
                    + " regularly forced to offer up a sacrfice to the demons.");
            }

            // 按钮：Oasis Tower（原版 Camp.hx:2823-2825；when: MET_MINERVA >= 4）
            if (Flags.Get(VanillaFlags.MetMinerva) >= 4)
                AddButton(slot++, "Oasis Tower", () => SceneInstance<MinervaScene>.Get().EncounterMinerva(),
                    "Visit the ruined tower in the high mountains where Minerva resides.");

            // 按钮：Salon（原版 Camp.hx:2826-2828；when: HairdresserMeeting 状态）
            if (Player.HasStatusEffect(VanillaStatusEffects.HairdresserMeeting))
                AddButton(slot++, "Salon", () => SceneInstance<Salon>.Get().SalonGreeting(),
                    "Visit the goblin-run hair salon for some service.");

            // 按钮：Tel'Adre（原版 Camp.hx:2829-2835；when: TelAdre 状态 v1 >= 1）
            // C# TelAdre 没有单独 telAdreMenu()，入口是 Entry()（TelAdre.cs:13），所以用 Go<TelAdreScene>()。
            // 原版通缉状态（time.days < kittens.disabledDate）分支依赖 C# Kittens 未实现的 disabledDate，暂不置灰。
            if (Player.StatusEffectValue(VanillaStatusEffects.TelAdre, 0) >= 1)
                AddButton(slot++, "Tel'Adre", () => Go<TelAdreScene>(),
                    "Visit the city of Tel'Adre in the desert, easily recognized by the massive tower.");

            // 按钮：Town Ruins（原版 Camp.hx:2836-2838；when: AMILY_VILLAGE_ACCESSIBLE > 0）
            if (Flags.Get(VanillaFlags.AmilyVillageAccessible) > 0)
                AddButton(slot++, "Town Ruins", () => SceneInstance<TownRuinsScene>.Get().ExploreVillageRuin(),
                    "Visit the village ruins.");

            // 按钮：Nephila Coven（原版 Camp.hx:2839-2842；when: nephilaCovenIsFollower() = NEPHILA_COVEN_QUEEN_CROWNED > 0）
            // 场景类存在（NephilaCovenScene.EncounterNephilaCoven），但不在本批 8 个内，入口未接 → 条件满足时置灰。
            if (Flags.Get(VanillaFlags.NephilaCovenQueenCrowned) > 0)
                AddButtonDisabled(slot++, "Nephila Coven",
                    "Entry not wired yet: original Camp.hx:2839-2842 (nephilaCovenFollowerScene.nephilaCovenFollowerEncounter); C# NephilaCovenScene.EncounterNephilaCoven exists but was not part of this batch.");

            // 按钮 14：Back（原版 Camp.hx:2877 addButton(14, "Back", places)，这里回营地）
            AddButton(14, "Back", () => Go<Camp>());
        }

        // 原版 Camp.hx:2698-2722 dungeonFound()：
        // DISCOVERED_DUNGEON_2_ZETAZ / FACTORY_FOUND / DISCOVERED_WITCH_DUNGEON / D3_DISCOVERED /
        // checkPhoenixTowerClear() / FOUND_MANOR / FOUND_WIZARD_TOWER。
        private static bool DungeonFound() =>
            Flags.Get(VanillaFlags.DiscoveredDungeon2Zetaz) > 0
            || Flags.Get(VanillaFlags.FactoryFound) > 0
            || Flags.Get(VanillaFlags.DiscoveredWitchDungeon) > 0
            || Flags.Get(VanillaFlags.D3Discovered) > 0
            || PhoenixTowerClear()
            || Flags.Get(VanillaFlags.FoundManor) > 0
            || Flags.Get(VanillaFlags.FoundWizardTower) > 0;

        // 原版 DungeonCore.checkPhoenixTowerClear（DungeonCore.hx:130-132）
        private static bool PhoenixTowerClear() =>
            Flags.Get(VanillaFlags.HarpyQueenExecuted) != 0
            && Flags.Get(VanillaFlags.HelHarpiesDefeated) > 0
            && Flags.Get(VanillaFlags.HelPhoenixesDefeated) > 0
            && Flags.Get(VanillaFlags.HelBrigidDefeated) > 0;

        // 原版 Camp.hx:2862-2889 dungeons()：地牢子菜单。
        // 已接入口：Factory / Deep Cave / Desert Cave / Phoenix Tower / Old Manor / TowerOfDeception。
        public void DungeonsMenu()
        {
            ClearOutput();
            ClearButtons();

            if (Flags.Get(VanillaFlags.FactoryFound) > 0)
            {
                var cleared = Flags.Get(VanillaFlags.FactoryShutdown) > 0;

                AddButton(0, "Factory", () => Go<FactoryScene>(),
                    "Visit the demonic factory in the mountains."
                    + (cleared ? "[pg]You've managed to shut down the factory." : "The factory is still running. Marae wants you to shut down the factory!")
                    + (cleared ? "[pg]CLEARED!" : ""));
            }

            if (Flags.Get(VanillaFlags.DiscoveredDungeon2Zetaz) > 0)
            {
                AddButton(1, "Deep Cave", () => DungeonScene.Enter(new MonoCoC.ModLoader.Identifier("coc", "deep_cave")),
                    "Visit the cave you've found in the Deepwoods."
                    + (Flags.Get(VanillaFlags.DefeatedZetaz) > 0 ? "[pg]You've defeated Zetaz, your old rival." : ""));
            }

            if (Flags.Get(VanillaFlags.D3Discovered) > 0)
            {
                // 原版 Camp.hx:2868 game.lethicesKeep.enterD3()、hint 用 DungeonCore.checkLethiceStrongholdClear()
                var strongholdCleared = Flags.Get(VanillaFlags.D3MirrorsShattered) > 0
                    && Flags.Get(VanillaFlags.D3JeanClaudeDefeated) > 0
                    && Flags.Get(VanillaFlags.D3GardenerDefeated) > 0
                    && Flags.Get(VanillaFlags.D3CentaurDefeated) > 0
                    && Flags.Get(VanillaFlags.LethiceDefeated) > 0;

                AddButton(2, "Stronghold",
                    () => SceneInstance<LethicesKeepScene>.Get().EnterD3(),
                    "Visit the stronghold in the high mountains that belongs to Lethice, the demon queen."
                    + (strongholdCleared
                        ? "[pg]You have defeated Lethice and put an end to the demonic threats. Congratulations, you've beaten the main story![pg]CLEARED!"
                        : ""));
            }

            if (Flags.Get(VanillaFlags.DiscoveredWitchDungeon) > 0)
            {
                AddButton(5, "Desert Cave", () => DungeonScene.Enter(new MonoCoC.ModLoader.Identifier("coc", "desert_cave")),
                    "Visit the cave you've found in the desert.");
            }

            if (PhoenixTowerClear())
            {
                AddButton(6, "Phoenix Tower", () => SceneInstance<HelDungeonScene>.Get().ReturnToHeliaDungeon(),
                    "Re-visit the tower you went to as part of Helia's quest.[pg]CLEARED!");
            }

            if (Flags.Get(VanillaFlags.FoundManor) > 0)
            {
                var necroCleared = (Flags.Get(VanillaFlags.ManorProgress) & 128) != 0;

                AddButton(7, "Old Manor", () => SceneInstance<ManorScene>.Get().EnterDungeon(),
                    "Visit the accursed manor overlooking the forested valley."
                    + (necroCleared ? "[pg]You have slain the Necromancer and put an end to his unspeakable transgressions of nature.[pg]CLEARED!" : ""));
            }

            if (Flags.Get(VanillaFlags.FoundWizardTower) > 0)
            {
                AddButton(8, "TowerOfDeception", () => Go<WizardTowerEntranceScene>(),
                    "Visit the obsidian tower in the Volcanic Crag.");
            }

            AddButton(14, "Back", () => Entry());
        }
    }
}
