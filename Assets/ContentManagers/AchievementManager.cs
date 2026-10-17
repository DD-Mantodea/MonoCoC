using Hjson;
using MonoCoC.Contents.Achievements;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using MonoCoC.Extensions;

namespace MonoCoC.Assets.ContentManagers
{
    // 成就定义（原版 Achievements.hx 的 KACHIEVEMENTS 表）。
    // 数据在 Assets/Data/Achievements/*.hjson；代码里只留 Identifier 常量（AchievementIds）。
    public class AchievementManager : ContentManager<Achievement>
    {
        public override string Folder => "Data/Achievements";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, Achievement> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            if (!root.TryGetValue("achievements", out var value) || value is not JsonArray achievements)
                throw new FormatException($"Achievement file '{path}' has no 'achievements' array");

            foreach (var entry in achievements)
            {
                if (entry is not JsonObject json)
                    continue;

                var id = json.Str("id");

                if (!Identifier.TryParse(id, out var identifier))
                    throw new FormatException($"Invalid achievement id '{id}' in '{path}'");

                entries[identifier.ToString()] = new Achievement
                {
                    Id = identifier,

                    TitleKey = json.Str("titleKey"),

                    DescriptionKey = json.Str("descriptionKey")
                };
            }
        }

        // 按 id 取定义；取不到说明数据缺失（调用方决定是抛还是忽略）。
        public Achievement? Find(Identifier id) =>

            Dict.TryGetValue(id.ToString(), out var hit) ? hit : null;

        // 已解锁的成就 id（原版 EngineCore.achievements）。随存档序列化，放在管理器里统一维护。
        public readonly HashSet<Identifier> Unlocked = [];

        public bool IsUnlocked(Identifier id) => Unlocked.Contains(id);

        // 首次解锁返回 true；重复解锁返回 false（原版 awardAchievement 只在第一次显示）。
        public bool Unlock(Identifier id) => Unlocked.Add(id);

        public void ClearUnlocked() => Unlocked.Clear();

        // 成就列表 UI 用：全部定义（含未解锁），按 id 排序保证顺序稳定。
        public IEnumerable<Achievement> All => Dict.Values.OrderBy(a => a.Id.ToString(), StringComparer.Ordinal);

        // 已解锁的条目（定义取自数据，解锁状态取自存档）。
        public IEnumerable<Achievement> UnlockedAchievements =>

            All.Where(a => Unlocked.Contains(a.Id));
    }
}