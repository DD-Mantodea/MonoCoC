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

        public JsonObject PlayerOptions = [];

        // 场景私有存档（原版每个场景的 saveContent）
        public JsonObject Scenes = [];

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

            var time = GameView.Instance?.Time;

            data.Time = new JsonObject
            {
                ["days"] = time?.Days ?? 1,

                ["hours"] = time?.Hours ?? 6,

                ["minutes"] = time?.Minutes ?? 0
            };

            data.PlayerOptions = OptionStore.LocalToJson();

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

            ["playeroptions"] = PlayerOptions,

            ["scenes"] = Scenes,

            ["achievements"] = Achievements
        };

        public static SaveData FromJson(JsonObject json)
        {
            if (json == null)
                return null;

            return new SaveData
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

                  PlayerOptions = json.TryGetValue("playeroptions", out var playerOptions) && playerOptions is JsonObject optionsObject ? optionsObject : [],

                Scenes = json.TryGetValue("scenes", out var scenes) && scenes is JsonObject scenesObject ? scenesObject : [],

                Achievements = json.TryGetValue("achievements", out var achievements) && achievements is JsonArray achievementsArray ? achievementsArray : []
            };
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

            // 原版读档后 lust 也受 minLust 下限约束
            player.ClampLust();

            if (GameView.Instance != null)
            {
                GameView.Instance.Time.Days = Time.TryGetValue("days", out var days) ? (int)days : 1;

                GameView.Instance.Time.Hours = Time.TryGetValue("hours", out var hours) ? (int)hours : 6;

                GameView.Instance.Time.Minutes = Time.TryGetValue("minutes", out var minutes) ? (int)minutes : 0;
            }

            OptionStore.ApplyLocal(PlayerOptions);

            Contents.Scenes.SceneSaveRegistry.Load(Scenes);

            // 读档：小时派发注册表与已注册场景单例都要重建（NewGame 在 GameView 构造里做同样的事）。
            Contents.World.TimeAwareRegistry.ReloadVanilla();
        }

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
