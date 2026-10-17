using System;
using System.Linq;
using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Bog;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.Contents.Scenes.Vanilla.Areas.HighMountains;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain;
using MonoCoC.Contents.Scenes.Vanilla.Areas.VolcanicCrag;
using MonoCoC.Contents.Scenes.Vanilla.Factory;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Hel;
using MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar;
using MonoCoC.Contents.Scenes.Vanilla.Places.Lake;
using MonoCoC.Extensions;
using TelAdreScene = MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre;

namespace MonoCoC.Contents.Scenes.Vanilla.Places
{
    [Register<PlacesScene>("coc:places")]
    public class PlacesScene : Scene
    {
        private readonly PagedMenu<(string Label, Action Action, string Hint)> _pager = new();

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            ShowImage("camp-pathfinder");

            OutputText("Scenes.Places.Intro".Localized());

            var items = new System.Collections.Generic.List<(string Label, Action Action, string Hint)>
            {
                ("Common.Button.Bazaar".Localized(),

                    () => SceneInstance<BazaarScene>.Get().EnterTheBazaar(),

                    "Visit the Bizarre Bazaar, where demons and corrupted beings hang out.")
            };

            if (!Player.HasStatusEffect(StatusEffectIds.BoatDiscovery))
                items.RemoveAt(0);

            if (Player.HasStatusEffect(StatusEffectIds.BoatDiscovery))
                items.Add(("Common.Button.Boat".Localized(),

                    () => SceneInstance<BoatScene>.Get().BoatExplore(),

                    "Get on the boat and explore the lake.[pg]Recommended level: 4"));

            if (Flags.Get(GameFlagIds.BazaarEntered) <= 0)
                items.RemoveAll(item => item.Label == "Common.Button.Bazaar".Localized());

            if (Flags.Get(GameFlagIds.FoundCathedral) > 0)
                items.Add(("Cathedral",

                    () => SceneInstance<GargoyleScene>.Get().GargoyleStarterMenu(),

                    "Visit the ruined cathedral where a gargoyle resides."));

            if (DungeonFound())
                items.Add(("Common.Button.Dungeons".Localized(), DungeonsMenu, "Delve into dungeons."));

            if (SceneInstance<FarmScene>.Get().FarmEnabled())
                items.Add(("Common.Button.Farm".Localized(),

                    () => SceneInstance<FarmScene>.Get().FarmExploreEncounter(),

                    "Visit Whitney's farm near the lake."));

            if (Flags.Get(GameFlagIds.AikoTimesMet) > 3)
                items.Add(("Common.Button.GreatTree".Localized(),

                    () => SceneInstance<AikoScene>.Get().EncounterAiko(),

                    "Visit the Great Tree in the Deep Woods where Aiko lives."));

            if (Flags.Get(GameFlagIds.LumiMet) > 0)
                items.Add(("Lumi's Lab", () => SceneInstance<LumiScene>.Get().LumiEncounter(),

                    "Visit the goblin alchemist Lumi at her lab."));

            if (SceneInstance<SylviaScene>.Get().SylviaProg >= 4
                && !(MothCave.MothCaveScene.DoloresProg == 7 && MothCave.MothCaveScene.DoloresTime < 6))

                items.Add(("Common.Button.MothCave".Localized(),

                    () => SceneInstance<MothCave.MothCaveScene>.Get().EncounterCave(),

                    "Visit Sylvia's home."));

            if (Flags.Get(GameFlagIds.OwcaUnlocked) == 1)
            {
                var sacrificeDisabled = Flags.Get(GameFlagIds.OwcaSacrificeDisabled) > 0;

                items.Add(("Common.Button.Owca".Localized(),

                    () => SceneInstance<OwcaScene>.Get().GangbangVillageStuff(),

                    "Visit the sheep village of Owca, where the villagers "

                    + (sacrificeDisabled ? "were" : "are")

                    + " regularly forced to offer up a sacrfice to the demons."));
            }

            if (Flags.Get(GameFlagIds.MetMinerva) >= 4)
                items.Add(("Common.Button.OasisTower".Localized(),

                    () => SceneInstance<MinervaScene>.Get().EncounterMinerva(),

                    "Visit the ruined tower in the high mountains where Minerva resides."));

            if (Player.HasStatusEffect(StatusEffectIds.HairdresserMeeting))
                items.Add(("Common.Button.Salon".Localized(),

                    () => SceneInstance<Salon>.Get().SalonGreeting(),

                    "Visit the goblin-run hair salon for some service."));

            if (Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1)
                items.Add(("Common.Button.Teladre".Localized(), () => Go<TelAdreScene>(),

                    "Visit the city of Tel'Adre in the desert, easily recognized by the massive tower."));

            if (Flags.Get(GameFlagIds.AmilyVillageAccessible) > 0)
                items.Add(("Common.Button.TownRuins".Localized(),

                    () => SceneInstance<TownRuinsScene>.Get().ExploreVillageRuin(),

                    "Visit the village ruins."));

            var nephila = SceneInstance<NephilaCovenFollowerScene>.Get();

            if (nephila.NephilaCovenIsFollower())
                items.Add(("Nephila Coven",

                    () => nephila.NephilaCovenFollowerEncounter(),

                    "Have one of your daughters open a portal to the nephila coven's palace so that you may visit."));

            _pager.Items = items;

            _pager.PageSize = 12;

            _pager.Render(
                (slot, label, action) =>
                {
                    var current = items.FirstOrDefault(item => item.Label == label);

                    if (action == null)
                        AddButtonDisabled(slot, label);
                    else
                        AddButton(slot, label, action, current.Hint);
                },

                item => item.Label,

                item => item.Action,

                () => Go<CampScene>(),

                Entry,

                "Common.Button.Prev".Localized(),

                "Common.Button.Next".Localized(),

                "Common.Button.Back".Localized(),

                0);
        }

        public static bool DungeonFound() =>

            Flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) > 0

            || Flags.Get(GameFlagIds.FactoryFound) > 0

            || Flags.Get(GameFlagIds.DiscoveredWitchDungeon) > 0

            || Flags.Get(GameFlagIds.D3Discovered) > 0

            || PhoenixTowerClear()

            || Flags.Get(GameFlagIds.FoundManor) > 0

            || Flags.Get(GameFlagIds.FoundWizardTower) > 0;

        public static bool PhoenixTowerClear() =>

            Flags.Get(GameFlagIds.HarpyQueenExecuted) != 0

            && Flags.Get(GameFlagIds.HelHarpiesDefeated) > 0

            && Flags.Get(GameFlagIds.HelPhoenixesDefeated) > 0

            && Flags.Get(GameFlagIds.HelBrigidDefeated) > 0;

        public void DungeonsMenu()
        {
            ClearOutput();

            ClearButtons();

            if (Flags.Get(GameFlagIds.FactoryFound) > 0)
            {
                var cleared = Flags.Get(GameFlagIds.FactoryShutdown) > 0;

                AddButton(0, "Common.Button.Factory".Localized(), () => Go<FactoryScene>(),

                    "Visit the demonic factory in the mountains."

                    + (cleared ? "[pg]You've managed to shut down the factory." : "The factory is still running. Marae wants you to shut down the factory!")

                    + (cleared ? "[pg]CLEARED!" : ""));
            }

            if (Flags.Get(GameFlagIds.DiscoveredDungeon2Zetaz) > 0)
                AddButton(1, "Dungeons.DeepCave.Name".Localized(), () => DungeonScene.Enter(new MonoCoC.Registry.Identifier("coc", "deep_cave")),

                    "Visit the cave you've found in the Deepwoods."

                    + (Flags.Get(GameFlagIds.DefeatedZetaz) > 0 ? "[pg]You've defeated Zetaz, your old rival." : ""));

            if (Flags.Get(GameFlagIds.D3Discovered) > 0)
            {

                var strongholdCleared = Flags.Get(GameFlagIds.D3MirrorsShattered) > 0

                    && Flags.Get(GameFlagIds.D3JeanClaudeDefeated) > 0

                    && Flags.Get(GameFlagIds.D3GardenerDefeated) > 0

                    && Flags.Get(GameFlagIds.D3CentaurDefeated) > 0

                    && Flags.Get(GameFlagIds.LethiceDefeated) > 0;

                AddButton(2, "Common.Button.Stronghold".Localized(),

                    () => SceneInstance<LethicesKeepScene>.Get().EnterD3(),

                    "Visit the stronghold in the high mountains that belongs to Lethice, the demon queen."

                    + (strongholdCleared

                        ? "[pg]You have defeated Lethice and put an end to the demonic threats. Congratulations, you've beaten the main story![pg]CLEARED!"

                        : ""));
            }

            if (Flags.Get(GameFlagIds.DiscoveredWitchDungeon) > 0)
                AddButton(5, "Dungeons.DesertCave.Name".Localized(), () => DungeonScene.Enter(new MonoCoC.Registry.Identifier("coc", "desert_cave")),

                    "Visit the cave you've found in the desert.");

            if (PhoenixTowerClear())
                AddButton(6, "Common.Button.PhoenixTower".Localized(), () => SceneInstance<HelDungeonScene>.Get().ReturnToHeliaDungeon(),

                    "Re-visit the tower you went to as part of Helia's quest.[pg]CLEARED!");

            if (Flags.Get(GameFlagIds.FoundManor) > 0)
            {
                var necroCleared = (Flags.Get(GameFlagIds.ManorProgress) & 128) != 0;

                AddButton(7, "Common.Button.OldManor".Localized(), () => SceneInstance<ManorScene>.Get().EnterDungeon(),

                    "Visit the accursed manor overlooking the forested valley."

                    + (necroCleared ? "[pg]You have slain the Necromancer and put an end to his unspeakable transgressions of nature.[pg]CLEARED!" : ""));
            }

            if (Flags.Get(GameFlagIds.FoundWizardTower) > 0)
                AddButton(8, "Common.Button.Towerofdeception".Localized(), () => Go<WizardTowerEntranceScene>(),

                    "Visit the obsidian tower in the Volcanic Crag.");

            AddButton(14, "Common.Button.Back".Localized(), () => Entry());
        }
    }
}