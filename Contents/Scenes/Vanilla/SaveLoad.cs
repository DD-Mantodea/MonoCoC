using MonoCoC.Extensions;
using MonoCoC.Saving;
using MonoCoC.Attributes;
using MonoCoC.Contents.Items;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Components.Views;
using System;
using System.IO;
using System.Threading;
using System.Linq;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    // 鍘熺増 Saves.hx:238 saveLoad()锛氫富鑿滃崟 + 瀛?璇?鍒犳。 + 鑷姩瀛樻。 + 鏂囦欢瀵煎叆瀵煎嚭
    [Register<SaveLoad>("coc:save_load")]
    public class SaveLoad : Scene
    {
        public const int PageSize = 10;

        public static bool FromMainMenu;

        private int _page;

        private string? _deleteSlot;

        public override void Entry() => ShowMain();

        private bool CanSave => Player.Loaded && !FromMainMenu;

        private void ShowMain()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.SaveLoad.MainInfo".Localized());

            var save = AddButton(0, "Scenes.SaveLoad.Button.Save".Localized(), ShowSaveList);

            if (!CanSave && save != null)
                save.DisableIf(true, "Scenes.SaveLoad.CannotSave".Localized());

            AddButton(1, "Scenes.SaveLoad.Button.Load".Localized(), ShowLoadList);

            AddButton(2, "Scenes.SaveLoad.Button.Delete".Localized(), ShowDeleteList);

            if (Player.Loaded)
                AddButton(3, "Scenes.SaveLoad.Button.AutoSave".Localized(Player.AutoSave ? "ON" : "OFF"), ToggleAutoSave);

            var saveFile = AddButton(5, "Scenes.SaveLoad.Button.SaveToFile".Localized(), SaveToFile);

            if (!CanSave && saveFile != null)
                saveFile.DisableIf(true, "Scenes.SaveLoad.CannotSave".Localized());

            AddButton(6, "Scenes.SaveLoad.Button.LoadFile".Localized(), LoadFromFile);

            AddButton(7, "Scenes.SaveLoad.Button.Import".Localized(), ImportOriginal);

            AddButton(14, "Common.Back".Localized(), Back);
        }

        private void Back()
        {
            if (FromMainMenu)
            {
                FromMainMenu = false;

                UI.Components.Combat.PlayerStats.HiddenByMenu = false;

                UIManager.Instance.Root.SwitchView(new MainView());

                return;
            }

            UI.Components.Combat.PlayerStats.HiddenByMenu = false;

            Go<CampScene>();
        }

        private void ToggleAutoSave()
        {
            Player.AutoSave = !Player.AutoSave;

            ShowMain();
        }

        private SaveData[] Entries => SaveData.List().Where(entry => !entry.Slot.StartsWith("AutoSave")).ToArray();

        private void ShowSlots(string mode, int page)
        {
            _page = Math.Max(0, page);

            ClearOutput();

            ClearButtons();

            DisplayHeader("Scenes.SaveLoad.Title".Localized());

            if (mode == "delete" && _deleteSlot != null)
            {
                OutputText("Scenes.SaveLoad.ConfirmDelete".Localized(_deleteSlot));

                AddButton(0, "Common.Yes".Localized(), () =>
                {
                    SaveManager.Delete(_deleteSlot!);

                    _deleteSlot = null;

                    ShowSlots("delete", _page);
                });

                AddButton(1, "Common.No".Localized(), () =>
                {
                    _deleteSlot = null;

                    ShowSlots("delete", _page);
                });

                return;
            }

            if (mode == "save")
            {
                OutputText("Scenes.SaveLoad.NotesHint".Localized());

                PromptInput(text: "-");
            }

            var entries = Entries;

            var pages = Math.Max(1, (entries.Length + PageSize - 1) / PageSize);

            _page = Math.Min(_page, pages - 1);

            if (entries.Length == 0)
                OutputText("Scenes.SaveLoad.Empty".Localized());

            for (var offset = 0; offset < PageSize; offset++)
            {
                var index = _page * PageSize + offset;

                if (index >= entries.Length)
                    break;

                var entry = entries[index];

                var slot = entry.Slot;

                var notes = string.IsNullOrWhiteSpace(entry.Notes) ? "" : $" ({entry.Notes})";

                AddButton(offset, $"{offset + 1}. {entry.Name}{notes} - {entry.SaveTimeText}", () =>
                {
                    if (mode == "save")
                        SaveSlot(slot);
                    else if (mode == "load")
                        LoadSlot(slot);
                    else
                    {
                        _deleteSlot = slot;

                        ShowSlots("delete", _page);
                    }
                });
            }

            if (mode == "save")
                AddButton(11, "Scenes.SaveLoad.NewSave".Localized(), () => SaveSlot(NextSlot()));

            if (pages > 1)
            {
                AddButton(9, "Scenes.SaveLoad.Previous".Localized(), () => ShowSlots(mode, _page - 1));

                AddButton(10, "Scenes.SaveLoad.Next".Localized(), () => ShowSlots(mode, _page + 1));
            }

            AddButton(14, "Common.Back".Localized(), ShowMain);
        }

        private void ShowSaveList() => ShowSlots("save", 0);

        private void ShowLoadList() => ShowSlots("load", 0);

        private void ShowDeleteList() => ShowSlots("delete", 0);

        private void SaveSlot(string slot)
        {
            if (!CanSave)
                return;

            var notes = FinishInput();

            if (notes == "-")
                notes = "";

            var data = SaveData.Capture(Player, slot);

            data.Notes = notes;

            data.Write(slot);

            ShowSlots("save", _page);
        }

        private void LoadSlot(string slot)
        {
            if (!SaveData.TryRead(slot, out var data))
                return;

            data.Apply(Player);

            UI.Components.Combat.PlayerStats.HiddenByMenu = false;

            GameView.SetPlayerStatsVisible(true);

            Go<CampScene>();
        }

        public static string NextSlot()
        {
            var name = string.IsNullOrWhiteSpace(Player.ShortName) ? "Player" : SaveManager.Sanitize(Player.ShortName);

            for (var index = 0; index < int.MaxValue; index++)
            {
                var slot = $"Save{index}_{name}";

                if (!SaveManager.Exists(slot))
                    return slot;
            }

            throw new InvalidOperationException("No free save slot");
        }

        private static string? PickOpenFile(string filter)
        {
            string? selected = null;

            var thread = new Thread(() =>
            {
                using var dialog = new System.Windows.Forms.OpenFileDialog { Filter = filter };

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    selected = dialog.FileName;
            });

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();

            thread.Join();

            return selected;
        }

        private static string? PickSaveFile(string filter, string fileName)
        {
            string? selected = null;

            var thread = new Thread(() =>
            {
                using var dialog = new System.Windows.Forms.SaveFileDialog
                {
                    Filter = filter,

                    FileName = fileName,
                };

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    selected = dialog.FileName;
            });

            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();

            thread.Join();

            return selected;
        }

        private void ImportOriginal()
        {
            var path = PickOpenFile("Original CoC save (*.sol;*.coc)|*.sol;*.coc|All files (*.*)|*.*");

            if (path == null)
                return;

            ApplyLegacySave(path);
        }

        private void ApplyLegacySave(string path)
        {
            try
            {
                _ = Assets.ManagerInstance<MonoCoC.Assets.ContentManagers.ItemManager>.Instance;

                _ = Assets.ManagerInstance<MonoCoC.Assets.ContentManagers.PerkManager>.Instance;

                var legacy = Saving.Legacy.SaveLoader.Load(path);

                var root = (Hjson.JsonObject)Hjson.HjsonValue.Parse(legacy.ToString());

                // 鍘熺増瀛樻。鏄墎骞崇殑 Haxe 瀵硅薄锛屽瓧娈电被鍨嬪拰 MonoCoC 涓嶅畬鍏ㄤ竴鑷达紱
                // 鍙彇鑳藉畨鍏ㄦ槧灏勭殑鏍稿績瀛楁锛岄伩鍏?Enum.Parse/寮哄埗杞崲鐩存帴鎶涘紓甯搞€?
                var player = new Hjson.JsonObject();

                Copy(root, player, "lust");

                Copy(root, player, "fatigue");

                Copy(root, player, "level");

                Copy(root, player, "gems");

                Copy(root, player, "tallness");

                Copy(root, player, "femininity");

                Copy(root, player, "tone");

                Copy(root, player, "thickness");

                Copy(root, player, "fertility");

                Copy(root, player, "nippleLength");

                Copy(root, player, "balls");

                Copy(root, player, "ballSize");

                Copy(root, player, "cumMultiplier");

                Copy(root, player, "hoursSinceCum");

                Copy(root, player, "statPoints");

                Copy(root, player, "perkPoints");

                Copy(root, player, "ascensionPerkPoints");

                Copy(root, player, "hunger");

                Copy(root, player, "autoSave");

                player["shortName"] = root.TryGetValue("short", out var shortName) ? shortName : "You";

                player["health"] = root.TryGetValue("HP", out var hp) ? hp : 50;

                player["experience"] = root.TryGetValue("XP", out var xp) ? xp : 0;

                player["cocks"] = BuildLegacyCocks(root);

                player["breastRows"] = BuildLegacyBreastRows(root);

                player["anal"] = BuildLegacyAnal(root);

                var mappedFlags = new Hjson.JsonObject();

                if (root.TryGetValue("flags", out var legacyFlags) && legacyFlags is Hjson.JsonObject flagsObject)
                    foreach (var pair in flagsObject)
                        if (Saving.Legacy.LegacyFlagMap.ByName.TryGetValue(pair.Key, out var flagId))
                            mappedFlags[flagId.ToString()] = pair.Value;

                var json = new Hjson.JsonObject
                {
                    ["version"] = root.TryGetValue("version", out var version) ? version : "legacy",
                    ["name"] = player["shortName"],
                    ["saveTime"] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                    ["flags"] = mappedFlags,
                    ["body"] = BuildLegacyBody(root),
                    ["player"] = player,
                    ["time"] = new Hjson.JsonObject
                    {
                        ["days"] = root.TryGetValue("days", out var days) ? days : 1,
                        ["hours"] = root.TryGetValue("hours", out var hours) ? hours : 6,
                        ["minutes"] = root.TryGetValue("minutes", out var minutes) ? minutes : 0,
                    },

                    ["scenes"] = new Hjson.JsonObject(),
                    ["achievements"] = new Hjson.JsonArray(),
                };

                var data = SaveData.FromJson(json);

                if (data == null)
                {
                    OutputText("Scenes.SaveLoad.ImportFailed".Localized());

                    return;
                }

                UI.Components.Combat.PlayerStats.HiddenByMenu = false;

                var gameView = GameView.CreateDefault();

                data.Apply(Player);

                Player.Loaded = true;

                // 鍘熺増 Marble 濂芥劅淇濆瓨鍦?statusAffects 鐨?Marble status value1
                if (root.TryGetValue("statusAffects", out var statusValue) && statusValue is Hjson.JsonArray statusAffects)
                    foreach (var statusEntry in statusAffects)
                    {
                        if (statusEntry is not Hjson.JsonObject status)
                            continue;

                        var statusName = StringValue(status, "statusAffectName", "");

                        if (statusName == "")
                            continue;

                        var statusId = LegacyStatusIdentifier(statusName);

                        if (Player.HasStatusEffect(statusId))
                            continue;

                        Player.CreateStatusEffect(statusId,

                            FloatValue(status, "value1", 0), FloatValue(status, "value2", 0),

                            FloatValue(status, "value3", 0), FloatValue(status, "value4", 0));
                    }

                // 鑰佸瓨妗ｅ彧鏈?exploredDeepwoods status锛屾病鏈?Area 瑙ｉ攣 flag锛?
                // 琛ュ洖 Deepwoods 鐨勫彂鐜扮姸鎬侊紝Explore 鎵嶄細鍒楀嚭璇ュ尯鍩熴€?
                if (Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.ExploredDeepwoods))
                {
                    var deepwoods = World.Areas.AreaRegistry.Get(
                        Datas.Vanilla.AreaIds.Deepwoods);

                    if (deepwoods != null)
                        World.Areas.AreaRegistry.Discover(deepwoods);
                }

                // 鍘熺増 Sylvia 鐘舵€佷繚瀛樺湪 selfSaving.sylvia.data
                var sylviaData = LegacySelfSavingData(root, "sylvia");

                if (sylviaData != null)
                {
                    Areas.Bog.SylviaScene.Save.SetInt("sylviaAffection", IntValue(sylviaData, "sylviaAffection", 0));

                    Areas.Bog.SylviaScene.Save.SetInt("sylviaDominance", IntValue(sylviaData, "sylviaDominance", 0));

                    Areas.Bog.SylviaScene.Save.SetInt("sylviaProgress", IntValue(sylviaData, "sylviaProgress", 0));
                }

                var rathazulData = LegacySelfSavingData(root, "rathazul");

                if (rathazulData != null
                    && rathazulData.TryGetValue("campFollower", out var rathazulFollower))

                    Npcs.RathazulScene.Save.SetBool("campFollower", (bool)rathazulFollower);

                // 鍘熺増 camp 鍦烘櫙鏁版嵁锛氳缁冨亣浜轰繚瀛樺湪 selfSaving.camp.data
                var campData = LegacySelfSavingData(root, "camp");

                if (campData != null)
                {
                    if (campData.TryGetValue("dummyBuilt", out var dummyBuilt) && (bool)dummyBuilt)
                        TrainingDummyCamp.DummyBuilt = true;

                    if (campData.TryGetValue("dummyName", out _))
                        TrainingDummyCamp.DummyName = StringValue(campData, "dummyName", "");

                    TrainingDummyCamp.DummyGender = IntValue(campData, "dummyGender", 0);
                }

                // 鍘熺増瀛樻。璁板綍鐨勬槸鏈€缁堝睘鎬э細琛ョ瓑鍊?Stat 淇
                ApplyLegacyStat(root, "str", Datas.Vanilla.GameStats.Strength);

                ApplyLegacyStat(root, "tou", Datas.Vanilla.GameStats.Toughness);

                ApplyLegacyStat(root, "spe", Datas.Vanilla.GameStats.Speed);

                ApplyLegacyStat(root, "inte", Datas.Vanilla.GameStats.Intelligence);

                ApplyLegacyStat(root, "lib", Datas.Vanilla.GameStats.Libido);

                ApplyLegacyStat(root, "sens", Datas.Vanilla.GameStats.Sensitivity);

                ApplyLegacyStat(root, "cor", Datas.Vanilla.GameStats.Corruption);

                // 鍘熺増 age: 0=Adult, 1=Child, 2=Teen, 3=Elder
                Player.Age = (MonoCoC.Contents.Creatures.Age)IntValue(root, "age", 0);

                Player.StartingAge = (MonoCoC.Contents.Creatures.Age)IntValue(root, "startingAge", 0);

                ImportLegacyPerks(root);

                ImportLegacyKeyItems(root);

                ImportLegacyMasteries(root);

                ImportLegacyInventory(root);

                // 瑁呭閫愪欢灏濊瘯锛氬崟浠跺け璐ヤ笉褰卞搷鏁翠釜瀵煎叆
                TryEquipLegacy(root, "armor", item => Player.SetArmor((Armor)item));

                TryEquipLegacy(root, "weapon", item => Player.SetWeapon((Weapon)item));

                TryEquipLegacy(root, "shield", item => Player.SetShield((Shield)item));

                if (!TryEquipLegacy(root, "jewelry", item => Player.SetJewelry((Jewelry)item)))
                    ApplyLegacyJewelryHp(root);

                // Perk/瑁呭鍙兘鏀瑰彉 minLust锛涙渶缁堝啀 clamp 涓€娆?
                Player.ClampLust();

                // 灞炴€т慨姝ｅ悗閲嶆柊濂楃敤瀛樻。 HP锛屽苟鎸夋柊鐨勪笂闄愭埅鏂?
                if (root.TryGetValue("HP", out var savedHp))
                    Player.Health = Math.Min((float)savedHp, Player.MaxHealth);

                UI.Components.Combat.PlayerStats.HiddenByMenu = false;

                Go<CampScene>();

                // 鍦烘櫙涓庢暟鎹叏閮ㄥ氨缁悗鍐嶆寕杞芥柊 GameView锛岄伩鍏嶅厛鏄剧ず鍗婂垵濮嬪寲鐣岄潰
                UIManager.Instance.Root.SwitchView(gameView);
            }
            catch (Exception ex)
            {
                OutputText("Scenes.SaveLoad.ImportFailed".Localized());

                OutputText("[pg]" + ex.Message);
            }
        }

                // 鍘熺増 JewelryLib锛氱敓鍛芥垝鎸囩洿鎺ユ彁鍗囨渶澶х敓鍛?
        private static void ApplyLegacyJewelryHp(Hjson.JsonObject root)
        {
            if (!root.TryGetValue("jewelry", out var value))
                return;

            var legacyId = value is Hjson.JsonObject obj && obj.TryGetValue("id", out var idValue)

                ? (string)idValue

                : value.ToString();

            var bonus = legacyId switch
            {
                "LifeRng" => 30f,

                "LifeRn2" => 45f,

                "LifeRn3" => 60f,

                _ => 0f,
            };

            if (bonus > 0)
                Player.Stats.AddModifier(Datas.Vanilla.GameStats.MaxHealth,

                    Stats.ModifierOperation.Addition, () => bonus,

                    new MonoCoC.Contents.Stats.ModifierSources.TemporarySource(Guid.NewGuid()));
        }

        private static void ImportLegacyInventory(Hjson.JsonObject root)
        {
            Player.RebuildStorage();

            ImportLegacyItems(root, "items", item => Player.Inventory.Add(item, 1));

            // 鍘熺増 itemSlots 鏄浐瀹氭Ы浣嶆暟缁勶紱MonoCoC 榛樿鍙В閿?3 鏍硷紝
            // 鎸夊瓨妗ｆ暟缁勯暱搴﹁ˉ鍥炲凡瑙ｉ攣妲戒綅锛屽惁鍒欏悗闈㈢殑鐗╁搧浼氳 UpdateItemStacks 閿佷綇銆?
            if (root.TryGetValue("items", out var itemsValue) && itemsValue is Hjson.JsonArray itemsArray)
            {
                Player.UnlockedSlots = Math.Clamp(
                    Math.Max(Player.UnlockedSlots, itemsArray.Count),

                    Player.UnlockedSlots,

                    Player.ItemStacks.Length);

                Player.UpdateItemStacks();
            }

            if (Player.Storage.TryGetValue(new MonoCoC.Registry.Identifier("coc", "items"), out var area))
            {
                ImportLegacyStorage(root, "itemStorage", area);

                ImportLegacyGearStorage(root, "gearStorage", area);
            }
        }

        private static void ImportLegacyGearStorage(Hjson.JsonObject root, string key,

            MonoCoC.Contents.Storage.StorageArea fallbackArea)
        {
            if (!root.TryGetValue(key, out var value) || value is not Hjson.JsonArray array)
                return;

            // 鍘熺増 gearStorage 鏄?5 涓澶囨灦 脳 9 鏍硷細姝﹀櫒銆佹姢鐢层€佺浘鐗屻€佸唴琛ｃ€侀楗?
            var gearAreas = Player.Storage.Values
                .Where(candidate => !candidate.Definition.Fallback)
                .OrderBy(candidate => candidate.Definition.Order)
                .ToList();

            for (var index = 0; index < array.Count; index++)
            {
                var item = LegacyItem(array[index]);

                if (item == null)
                    continue;

                var target = gearAreas.Count == 0

                    ? fallbackArea

                    : gearAreas[Math.Min(index / 9, gearAreas.Count - 1)];

                var slot = target.FirstEmptySlot();

                if (slot < 0)
                    continue;

                target.Slots[slot] = new ItemStack();

                target.Slots[slot].SetItemAndQty(item, 1);
            }
        }

        private static void ImportLegacyItems(Hjson.JsonObject root, string key, Action<Item> add)
        {
            if (!root.TryGetValue(key, out var value) || value is not Hjson.JsonArray array)
                return;

            var slot = 0;

            foreach (var entry in array)
            {
                var item = LegacyItem(entry);

                if (item == null)
                    continue;

                if (slot >= Player.ItemStacks.Length)
                    break;

                var quantity = entry is Hjson.JsonObject json && json.TryGetValue("quantity", out var qty)

                    ? (int)qty

                    : 1;

                var stack = Player.ItemStacks[slot++];

                stack.Unlocked = true;

                stack.SetItemAndQty(item, Math.Max(1, quantity));
            }
        }

        private static void ImportLegacyStorage(Hjson.JsonObject root, string key, MonoCoC.Contents.Storage.StorageArea area)
        {
            if (!root.TryGetValue(key, out var value) || value is not Hjson.JsonArray array)
                return;

            if (area.Capacity < array.Count)
                area.Resize(array.Count);

            for (var index = 0; index < array.Count; index++)
            {
                var item = LegacyItem(array[index]);

                if (item == null)
                    continue;

                var quantity = array[index] is Hjson.JsonObject json && json.TryGetValue("quantity", out var qty)

                    ? (int)qty

                    : 1;

                area.Slots[index] = new ItemStack();

                area.Slots[index].SetItemAndQty(item, quantity);
            }
        }

        private static Item? LegacyItem(Hjson.JsonValue value)
        {
            var legacyId = value is Hjson.JsonObject json && json.TryGetValue("id", out var idValue)

                ? (string)idValue

                : value.ToString();

            if (string.IsNullOrWhiteSpace(legacyId))
                return null;

            if (!Saving.Legacy.ItemIdMap.Resolve(legacyId, out var id))
                return null;

            try
            {
                return ItemRegistry.Create(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static bool TryEquipLegacy(Hjson.JsonObject root, string key, Action<Item> equip)
        {
            if (!root.TryGetValue(key, out var value))
                return false;

            var legacyId = value is Hjson.JsonObject obj && obj.TryGetValue("id", out var idValue)

                ? (string)idValue

                : value.ToString();

            if (!Saving.Legacy.ItemIdMap.Resolve(legacyId, out var id))
                return false;

            try
            {
                if (ItemRegistry.Create(id) is not { } item)
                    return false;

                equip(item);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void ImportLegacyEquipment(Hjson.JsonObject root, Hjson.JsonObject player, string key)
        {
            if (!root.TryGetValue(key, out var value))
                return;

            var legacyId = value is Hjson.JsonObject obj && obj.TryGetValue("id", out var idValue)

                ? (string)idValue

                : value.ToString();

            if (Saving.Legacy.ItemIdMap.Resolve(legacyId, out var id))
                player[key] = id.ToString();
        }

        private static void ImportLegacyKeyItems(Hjson.JsonObject root)
        {
            if (!root.TryGetValue("keyItems", out var keyItemsValue)
                || keyItemsValue is not Hjson.JsonArray keyItems)

                return;

            foreach (var entry in keyItems)
            {
                if (entry is not Hjson.JsonObject keyItem)
                    continue;

                var name = StringValue(keyItem, "keyName", "");

                if (name == "")
                    continue;

                Player.CreateKeyItem(name,

                    FloatValue(keyItem, "value1", 0), FloatValue(keyItem, "value2", 0),

                    FloatValue(keyItem, "value3", 0), FloatValue(keyItem, "value4", 0));
            }
        }

        private static void ImportLegacyMasteries(Hjson.JsonObject root)
        {
            if (!root.TryGetValue("masteries", out var masteriesValue)
                || masteriesValue is not Hjson.JsonArray masteries)

                return;

            foreach (var entry in masteries)
            {
                if (entry is not Hjson.JsonObject mastery)
                    continue;

                var id = StringValue(mastery, "id", "");

                if (id == "")
                    continue;

                var imported = Player.AddMastery(
                    Registry.Identifier.Parse("coc:" + id),

                    IntValue(mastery, "level", 0));

                imported.XP = IntValue(mastery, "xp", 0);

                imported.Permed = mastery.TryGetValue("isPermed", out var permed) && (bool)permed;
            }
        }

        private static void ImportLegacyPerks(Hjson.JsonObject root)
        {
            if (root.TryGetValue("perks", out var perksValue) && perksValue is Hjson.JsonArray perks)
                foreach (var entry in perks)
                {
                    if (entry is not Hjson.JsonObject perk || !perk.TryGetValue("id", out var idValue))
                        continue;

                    var legacyName = (string)idValue;

                    foreach (var perkId in Perks.PerkRegistry.Registry.Ids)
                    {
                        if (!string.Equals(Perks.PerkRegistry.GetName(perkId), legacyName,
                                StringComparison.OrdinalIgnoreCase)

                            && NormalizePerkName(Perks.PerkRegistry.GetName(perkId))

                                != NormalizePerkName(legacyName))

                            continue;

                        Player.CreatePerkIfNotHasPerk(perkId,

                            Number(perk, "value1"), Number(perk, "value2"), Number(perk, "value3"), Number(perk, "value4"));

                        AddLegacyPerkStat(perkId, Number(perk, "value1"));

                        break;
                    }
                }
        }

        private static void AddLegacyPerkStat(MonoCoC.Registry.Identifier perkId, float value1)
        {
            var fatigue = perkId.Path switch
            {
                "improved_endurance" => 20f,

                "improved_endurance2" => 10f,

                "demon_biology" => 20f,

                "ascension_endurance" => 5f * value1,

                _ => 0f,
            };

            if (fatigue > 0)
                Player.Stats.AddModifier(Datas.Vanilla.GameStats.FatigueMax,

                    Stats.ModifierOperation.Addition, () => fatigue,

                    new MonoCoC.Contents.Stats.ModifierSources.TemporarySource(Guid.NewGuid()));
        }

        private static string NormalizePerkName(string value) =>

            new string(value.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

        private static float Number(Hjson.JsonObject json, string key) =>

            json.TryGetValue(key, out var value) ? (float)(double)value : 0f;

        private static void ApplyLegacyStat(Hjson.JsonObject root, string key, MonoCoC.Contents.Stats.Stat stat)
        {
            if (!root.TryGetValue(key, out var value))
                return;

            var target = (float)(double)value;

            // 鍘熺増瀛樻。閲岀殑 str/spe/tou/inte 鏄熀纭€灞炴€у€硷紝鐩存帴鍐?base锛?
            // 涓嶈兘浣滀负涓存椂 modifier 鍙犲姞锛屽惁鍒欏悗缁崌绾?鐧惧垎姣斿姞鎴愪細绠楅敊銆?
            if (Math.Abs(Player.Stats.GetBase(stat) - target) < 0.01f)
                return;

            Player.Stats.SetBase(stat, target);
        }

        private static Hjson.JsonObject BuildLegacyBody(Hjson.JsonObject root)
        {
            var skinTone = StringValue(root, "skinTone", "albino");

            var furColor = StringValue(root, "furColor", "no");

            var skin = new Hjson.JsonObject
            {
                ["id"] = "coc:plain",
                ["tone"] = skinTone,
                ["adj"] = "",
                ["furColor"] = furColor,
                ["descKey"] = "Skin.Desc.skin",
            };

            return new Hjson.JsonObject
            {
                ["coc:skin"] = skin,
                ["coc:under_body"] = new Hjson.JsonObject { ["id"] = "coc:none", ["skin"] = skin },
                ["coc:hair"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:normal",
                    ["color"] = StringValue(root, "hairColor", "no"),
                    ["adj"] = "",
                    ["flowerColor"] = StringValue(root, "hairFlowerColor", ""),
                    ["length"] = FloatValue(root, "hairLength", 0),
                },

                ["coc:ears"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:human",
                    ["value"] = 0,
                    ["earsPierced"] = IntValue(root, "earsPierced", 0),
                },

                ["coc:face"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:human",
                    ["lipPierced"] = IntValue(root, "lipPierced", 0),
                    ["eyebrowPierced"] = IntValue(root, "eyebrowPierced", 0),
                    ["nosePierced"] = IntValue(root, "nosePierced", 0),
                },

                ["coc:eyes"] = new Hjson.JsonObject { ["id"] = "coc:human", ["count"] = 2 },
                ["coc:tongue"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:human",
                    ["pierced"] = IntValue(root, "tonguePierced", 0),
                },

                ["coc:neck"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:normal",
                    ["len"] = 2,
                    ["pos"] = false,
                    ["color"] = "no",
                },

                ["coc:lower_body"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:human",
                    ["legCount"] = 2,
                    ["incorporeal"] = false,
                },

                ["coc:arms"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:human",
                    ["claws"] = new Hjson.JsonObject { ["id"] = "coc:normal", ["tone"] = "" },
                },

                ["coc:beard"] = new Hjson.JsonObject
                {
                    ["id"] = "coc:normal",
                    ["length"] = FloatValue(root, "beardLength", 0),
                },

                ["coc:horns"] = new Hjson.JsonObject { ["id"] = "coc:none", ["value"] = 0 },
                ["coc:antennae"] = new Hjson.JsonObject { ["id"] = "coc:none" },
                ["coc:gills"] = new Hjson.JsonObject { ["id"] = "coc:none" },
                ["coc:rear_body"] = new Hjson.JsonObject { ["id"] = "coc:none", ["color"] = "" },
                ["coc:wings"] = new Hjson.JsonObject { ["id"] = "coc:none", ["color"] = "", ["color2"] = "" },
                ["coc:tail"] = new Hjson.JsonObject { ["id"] = "coc:none", ["venom"] = 0, ["recharge"] = 5 },
            };
        }

        private static Hjson.JsonArray BuildLegacyCocks(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("cocks", out var value) || value is not Hjson.JsonArray cocks)
                return result;

            foreach (var item in cocks)
            {
                if (item is not Hjson.JsonObject cock)
                    continue;

                result.Add(new Hjson.JsonObject
                {
                    ["id"] = "coc:human",
                    ["length"] = FloatValue(cock, "cockLength", 0),
                    ["thickness"] = FloatValue(cock, "cockThickness", 0),
                    ["knotMultiplier"] = FloatValue(cock, "knotMultiplier", 1),
                    ["pierced"] = IntValue(cock, "pierced", 0),
                    ["piercingShortName"] = StringValue(cock, "pShortDesc", ""),
                    ["piercingLongName"] = StringValue(cock, "pLongDesc", ""),
                    ["sock"] = StringValue(cock, "sock", ""),
                    ["sheathOn"] = false,
                });
            }

            return result;
        }

        private static Hjson.JsonArray BuildLegacyBreastRows(Hjson.JsonObject root)
        {
            var result = new Hjson.JsonArray();

            if (!root.TryGetValue("breastRows", out var value) || value is not Hjson.JsonArray rows)
                return result;

            foreach (var item in rows)
            {
                if (item is not Hjson.JsonObject row)
                    continue;

                result.Add(new Hjson.JsonObject
                {
                    ["id"] = "coc:normal",
                    ["breasts"] = IntValue(row, "breasts", 2),
                    ["breastRating"] = FloatValue(row, "breastRating", 0),
                    ["milkFullness"] = FloatValue(row, "milkFullness", 0),
                    ["nipplesPerBreast"] = IntValue(row, "nipplesPerBreast", 1),
                    ["lactation"] = FloatValue(row, "lactationMultiplier", 0),
                    ["loli"] = false,
                    ["fuckable"] = false,
                    ["nippleCocks"] = false,
                    ["nipplesPierced"] = IntValue(root, "nipplesPierced", 0),
                });
            }

            return result;
        }

        private static Hjson.JsonObject BuildLegacyAnal(Hjson.JsonObject root)
        {
            var looseness = 0d;

            var wetness = 0d;

            var fullness = 0d;

            if (root.TryGetValue("ass", out var value) && value is Hjson.JsonObject ass)
            {
                looseness = FloatValue(ass, "analLooseness", 0);

                wetness = FloatValue(ass, "analWetness", 0);

                fullness = FloatValue(ass, "fullness", 0);
            }

            return new Hjson.JsonObject
            {
                ["id"] = "coc:normal",
                ["wetness"] = wetness,
                ["looseness"] = looseness,
                ["fullness"] = fullness,
                ["virgin"] = false,
            };
        }

        private static Hjson.JsonObject? LegacySelfSavingData(Hjson.JsonObject root, string scene)
        {
            if (!root.TryGetValue("selfSaving", out var selfSaving)
                || selfSaving is not Hjson.JsonObject selfSavingObject

                || !selfSavingObject.TryGetValue(scene, out var sceneValue)

                || sceneValue is not Hjson.JsonObject sceneObject

                || !sceneObject.TryGetValue("data", out var dataValue)

                || dataValue is not Hjson.JsonObject data)

                return null;

            return data;
        }

        private static MonoCoC.Registry.Identifier LegacyStatusIdentifier(string name)
        {
            var target = NormalizeStatusKey(name);

            foreach (var field in typeof(MonoCoC.Contents.Datas.Vanilla.StatusEffectIds)
                         .GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                if (field.FieldType != typeof(MonoCoC.Registry.Identifier))
                    continue;

                var id = (MonoCoC.Registry.Identifier)field.GetValue(null)!;

                if (NormalizeStatusKey(id.Path) == target)
                    return id;
            }

            var builder = new System.Text.StringBuilder();

            for (var index = 0; index < name.Length; index++)
            {
                var character = name[index];

                if (character == ' ' || character == '-' || character == '_' || character == '\'')
                {
                    if (builder.Length > 0 && builder[^1] != '_')
                        builder.Append('_');

                    continue;
                }

                if (char.IsUpper(character) && index > 0 && builder.Length > 0
                    && builder[^1] != '_' && char.IsLower(name[index - 1]))

                    builder.Append('_');

                builder.Append(char.ToLowerInvariant(character));
            }

            return Registry.Identifier.Parse("coc:" + builder.ToString().Trim('_'));
        }

        private static string NormalizeStatusKey(string value) =>

            new(value.Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());

        private static string StringValue(Hjson.JsonObject source, string key, string fallback)
        {
            if (!source.TryGetValue(key, out var value))
                return fallback;

            var text = value.ToString();

            return text.Length >= 2 && text[0] == '"' && text[^1] == '"' ? text[1..^1] : text;
        }

        private static int IntValue(Hjson.JsonObject source, string key, int fallback)

            => source.TryGetValue(key, out var value) && double.TryParse(value.ToString(), out var number) ? (int)number : fallback;

        private static float FloatValue(Hjson.JsonObject source, string key, float fallback)

            => source.TryGetValue(key, out var value) && float.TryParse(value.ToString(), out var number) ? number : fallback;

        private static void Copy(Hjson.JsonObject source, Hjson.JsonObject target, string key)
        {
            if (source.TryGetValue(key, out var value))
                target[key] = value;
        }

        private void SaveToFile()
        {
            if (!CanSave)
                return;

            var path = PickSaveFile("CoC save (*.json)|*.json|All files (*.*)|*.*", $"{Player.ShortName}.json");

            if (path == null)
                return;

            var data = SaveData.Capture(Player, "File");

            File.WriteAllText(path, data.ToJson().ToString());

            ShowMain();
        }

        private void LoadFromFile()
        {
            var path = PickOpenFile("CoC save (*.json)|*.json|All files (*.*)|*.*");

            if (path == null)
                return;

            var json = (Hjson.JsonObject)Hjson.HjsonValue.Parse(File.ReadAllText(path));

            var data = SaveData.FromJson(json);

            if (data == null)
                return;

            data.Apply(Player);

            UI.Components.Combat.PlayerStats.HiddenByMenu = false;

            UIManager.Instance.Root.SwitchView(GameView.CreateDefault());

            Go<CampScene>();
        }
    }
}

