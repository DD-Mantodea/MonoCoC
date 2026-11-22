using Hjson;
using MonoCoC.Contents.Creatures;
using MonoCoC.Configuration;
using MonoCoC.UI.Components.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Saving
{
    public class SaveData
    {
        public const int AutoSaveLimit = 10;

        public string Version = GameInfo.SaveVersion;

        public string Slot = "";

        public string Name = "";

        public string Notes = "";

        public long SaveTime;

        public JsonObject Flags = [];

        public JsonObject Body = [];

        public JsonObject Player = [];

        public JsonObject Time = [];

        public string Location = "";

        public double Playtime;

        public JsonObject ModData = [];

        public JsonObject PlayerOptions = [];

        // 场景私有存档（原版每个场景的 saveContent）
        public JsonObject Scenes = [];

        public JsonObject SelfSaving = [];

        // 已解锁成就 id 列表
        public JsonArray Achievements = [];

        public string SaveTimeText => DateTimeOffset.FromUnixTimeMilliseconds(SaveTime).ToLocalTime().ToString("yyyy-MM-dd HH:mm");

        private static int _cacheRevision = -1;

        private static SaveData[] _cache = [];

        public static SaveData Capture(Player player, string slot)
        {
            var data = new SaveData
            {
                Slot = slot,

                Name = player.ShortName,

                SaveTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),

                Flags = CoC.Instance.Flags.Save()
            };

            player.Body.Save(data.Body);

            player.Save(data.Player);

            var time = GameView.Current?.Time;

            data.Time = new JsonObject
            {
                ["days"] = time?.Days ?? 1,

                ["hours"] = time?.Hours ?? 6,

                ["minutes"] = time?.Minutes ?? 0
            };

            data.PlayerOptions = OptionStore.LocalToJson();

            data.Location = player.Location;

            data.Playtime = GameView.Current?.Time.TotalTime ?? 0;

            data.ModData = ModSaveData.Capture();

            data.Scenes = Contents.Scenes.SceneSaveRegistry.Save();

            data.Achievements = [];

            foreach (var id in Assets.ManagerInstance<Assets.ContentManagers.AchievementManager>.Instance.Unlocked)
                data.Achievements.Add(id.ToString());

            return data;
        }

        public JsonObject ToJson() => new()
        {
            ["version"] = Version,

            ["slot"] = Slot,

            ["name"] = Name,

            ["notes"] = Notes,

            ["saveTime"] = SaveTime,

            ["flags"] = Flags,

            ["body"] = Body,

            ["player"] = Player,

            ["time"] = Time,

            ["location"] = Location,

            ["playtime"] = Playtime,

            ["moddata"] = ModData,

            ["playeroptions"] = PlayerOptions,

            ["scenes"] = Scenes,

            ["selfSaving"] = SelfSaving,

            ["achievements"] = Achievements
        };

        public static SaveData FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            var data = new SaveData
            {
                Version = json.TryGetValue("version", out var version) ? (string)version : "",

                Slot = json.TryGetValue("slot", out var slot) ? (string)slot : "",

                Name = json.TryGetValue("name", out var name) ? (string)name : "",

            Notes = json.TryGetValue("notes", out var notes) ? (string)notes : "",

                SaveTime = json.TryGetValue("saveTime", out var saveTime) ? (long)saveTime : 0,

                Flags = json.TryGetValue("flags", out var flags) && flags is JsonObject flagsObject ? flagsObject : [],

                Body = json.TryGetValue("body", out var body) && body is JsonObject bodyObject ? bodyObject : [],

                Player = json.TryGetValue("player", out var player) && player is JsonObject playerObject ? playerObject : [],

                Time = json.TryGetValue("time", out var time) && time is JsonObject timeObject ? timeObject : [],

                Location = json.TryGetValue("location", out var location) ? (string)location : "",

                Playtime = json.TryGetValue("playtime", out var playtime) ? (double)playtime : 0,

                ModData = json.TryGetValue("moddata", out var modData) && modData is JsonObject modObject ? modObject : [],

                  PlayerOptions = json.TryGetValue("playeroptions", out var playerOptions) && playerOptions is JsonObject optionsObject ? optionsObject : [],

                Scenes = json.TryGetValue("scenes", out var scenes) && scenes is JsonObject scenesObject ? scenesObject : [],

                SelfSaving = json.TryGetValue("selfSaving", out var selfSaving) && selfSaving is JsonObject selfSavingObject ? selfSavingObject : [],

                Achievements = json.TryGetValue("achievements", out var achievements) && achievements is JsonArray achievementsArray ? achievementsArray : []
            };

            MigrateLegacyInventory(json, data.Player);

            return data;
        }

        private static void MigrateLegacyInventory(JsonObject root, JsonObject player)
        {
            if (player.ContainsKey("itemSlots"))
                return;

            if (!root.TryGetValue("itemStorage", out var value) || value is not JsonArray slots)
                return;

            var converted = new JsonArray();

            var unlocked = 0;

            foreach (var entry in slots)
            {
                var slot = new JsonObject();

                if (entry is JsonObject saved)
                {
                    var item = saved.TryGetValue("id", out var id) ? id.ToString() : "";

                    if (item.Length == 0 && saved.TryGetValue("shortName", out var shortName))
                        item = shortName.ToString();

                    var isUnlocked = saved.TryGetValue("unlocked", out var unlockedValue) &&
                                      unlockedValue.ToString().Equals("true", StringComparison.OrdinalIgnoreCase);

                    slot["item"] = item;

                    slot["quantity"] = saved.TryGetValue("quantity", out var quantity) ? (int)quantity : 0;

                    slot["damage"] = saved.TryGetValue("damage", out var damage) ? (int)damage : 0;

                    slot["unlocked"] = isUnlocked;

                    if (isUnlocked)
                        unlocked++;
                }
                else
                {
                    slot["item"] = "";

                    slot["quantity"] = 0;

                    slot["damage"] = 0;

                    slot["unlocked"] = false;
                }

                converted.Add(slot);
            }

            player["itemSlots"] = converted;

            player["unlockedItemStacks"] = unlocked;
        }

        public void Apply(Player player)
        {
            player.SlotName = Slot;

            CoC.Instance.Flags.Load(Flags);

            // 旧存档里打包的 flag 拆分成独立字段
            LegacyFlagsMigration.Apply();

            // 还原成就
            Assets.ManagerInstance<Assets.ContentManagers.AchievementManager>.Instance.ClearUnlocked();

            foreach (var entry in Achievements)
                if (Registry.Identifier.TryParse((string)entry, out var achievementId))
                    Assets.ManagerInstance<Assets.ContentManagers.AchievementManager>.Instance.Unlock(achievementId);

            player.Body.Load(Body);

            player.Load(Player);

            ApplyLegacySelfSaving();

            // 原版读档后 lust 也受 minLust 下限约束
            player.ClampLust();

            if (GameView.Current != null)
            {
                GameView.Current.Time.Days = Time.TryGetValue("days", out var days) ? (int)days : 1;

                GameView.Current.Time.Hours = Time.TryGetValue("hours", out var hours) ? (int)hours : 6;

                GameView.Current.Time.Minutes = Time.TryGetValue("minutes", out var minutes) ? (int)minutes : 0;
            }

            OptionStore.ApplyLocal(PlayerOptions);

            Contents.Scenes.SceneSaveRegistry.Load(Scenes);

            ModSaveData.Load(ModData);

            // 读档：小时派发注册表与已注册场景单例都要重建（NewGame 在 GameView 构造里做同样的事）。
            Contents.World.TimeAwareRegistry.ReloadVanilla();
        }

        private void ApplyLegacySelfSaving()
        {
            if (SelfSaving.TryGetValue("sylvia", out var sylviaValue) && sylviaValue is JsonObject sylviaScene &&
                sylviaScene.TryGetValue("data", out var sylviaDataValue) && sylviaDataValue is JsonObject sylviaData)
            {
                MonoCoC.Contents.Scenes.Vanilla.Areas.Bog.SylviaScene.Save.SetInt("sylviaAffection", LegacyInt(sylviaData, "sylviaAffection"));
                MonoCoC.Contents.Scenes.Vanilla.Areas.Bog.SylviaScene.Save.SetInt("sylviaDominance", LegacyInt(sylviaData, "sylviaDominance"));
                MonoCoC.Contents.Scenes.Vanilla.Areas.Bog.SylviaScene.Save.SetInt("sylviaProgress", LegacyInt(sylviaData, "sylviaProgress"));
            }

            if (SelfSaving.TryGetValue("rathazul", out var rathazulValue) && rathazulValue is JsonObject rathazulScene &&
                rathazulScene.TryGetValue("data", out var rathazulDataValue) && rathazulDataValue is JsonObject rathazulData &&
                rathazulData.TryGetValue("campFollower", out var follower))
                MonoCoC.Contents.Scenes.Vanilla.Npcs.RathazulScene.Save.SetBool("campFollower", (bool)follower);

            if (SelfSaving.TryGetValue("camp", out var campValue) && campValue is JsonObject campScene &&
                campScene.TryGetValue("data", out var campDataValue) && campDataValue is JsonObject campData)
            {
                MonoCoC.Contents.Scenes.Vanilla.TrainingDummyCamp.DummyBuilt =
                    campData.TryGetValue("dummyBuilt", out var built) && (bool)built;
                MonoCoC.Contents.Scenes.Vanilla.TrainingDummyCamp.DummyName = LegacyString(campData, "dummyName");
                MonoCoC.Contents.Scenes.Vanilla.TrainingDummyCamp.DummyGender = LegacyInt(campData, "dummyGender");
            }

            if (CoC.Instance.Player.HasStatusEffect(Contents.Datas.Vanilla.StatusEffectIds.ExploredDeepwoods))
            {
                var area = Contents.World.Areas.AreaRegistry.Get(Contents.Datas.Vanilla.AreaIds.Deepwoods);

                if (area is not null)
                    Contents.World.Areas.AreaRegistry.Discover(area);
            }
        }

        private static int LegacyInt(JsonObject json, string key) =>
            json.TryGetValue(key, out var value) ? (int)value : 0;

        private static string LegacyString(JsonObject json, string key) =>
            json.TryGetValue(key, out var value) ? value.ToString() : "";

        public void Write(string slot)
        {
            var target = string.IsNullOrEmpty(slot) ? Slot : slot;

            Slot = target;

            SaveManager.Save(target, ToJson());
        }

        public static SaveData Read(string slot) => FromJson(SaveManager.Load(slot));

        public static bool TryRead(string slot, out SaveData data)
        {
            data = null;

            try
            {
                data = Read(slot);
            }
            catch
            {
                return false;
            }

            return IsUsable(data);
        }

        public static bool IsUsable(SaveData data) =>

            data != null &&

            data.Version == GameInfo.SaveVersion &&

            data.Player != null &&

            data.Player.Count > 0 &&

            data.Body != null &&

            data.Body.ContainsKey("coc:skin") &&

            data.Body.ContainsKey("coc:face");

        public static bool HasAny() => List().Length > 0;

        public static SaveData[] List()
        {
            if (_cacheRevision == SaveManager.Revision)
                return _cache;

            var data = new List<SaveData>();

            foreach (var slot in SaveManager.Slots())
                if (TryRead(slot, out var entry))
                    data.Add(entry);

            data.Sort((left, right) => right.SaveTime.CompareTo(left.SaveTime));

            _cache = [.. data];

            _cacheRevision = SaveManager.Revision;

            return _cache;
        }

        public static string AutoSaveSlot(Player player, int index) => $"AutoSave{index}_{SafeName(player)}";

        private static string SafeName(Player player)
        {
            var name = player.ShortName;

            if (string.IsNullOrWhiteSpace(name))
                return "Player";

            return SaveManager.Sanitize(name);
        }

        public static void AutoSave(Player player)
        {
            if (!player.AutoSave || player.GameOver)
                return;

            var slots = SaveManager.Slots()
                .Select(slot => (slot, index: ParseAutoSaveIndex(slot, player.ShortName)))
                .Where(entry => entry.index >= 0)
                .OrderBy(entry => entry.index)
                .ToList();

            if (slots.Count < AutoSaveLimit)
            {
                var next = slots.Count == 0 ? 0 : slots[^1].index + 1;

                var target = AutoSaveSlot(player, next);

                Capture(player, target).Write(target);

                return;
            }

            SaveManager.Delete(slots[0].slot);

            for (var offset = 1; offset < slots.Count; offset++)
                SaveManager.Rename(slots[offset].slot, AutoSaveSlot(player, offset - 1));

            var newest = AutoSaveSlot(player, AutoSaveLimit - 1);

            Capture(player, newest).Write(newest);
        }

        public static bool TryAutoSave(Player player)
        {
            try
            {
                AutoSave(player);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static int ParseAutoSaveIndex(string slot, string name)
        {
            var suffix = "_" + SaveManager.Sanitize(string.IsNullOrWhiteSpace(name) ? "Player" : name);

            if (!slot.StartsWith("AutoSave") || !slot.EndsWith(suffix))
                return -1;

            var number = slot["AutoSave".Length..^suffix.Length];

            return int.TryParse(number, out var index) && index >= 0 && index < AutoSaveLimit ? index : -1;
        }
    }
}
