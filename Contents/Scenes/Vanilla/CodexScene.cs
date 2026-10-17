using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Attributes;
using MonoCoC.Contents.Codex;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla
{

    [Register<CodexScene>("coc:codex")]
    public class CodexScene : Scene
    {
        public const int PageSize = 12;

        public string _category = "";

        public int _page;

        public static List<CodexEntry> AllEntries =>

            ManagerInstance<CodexManager>.Instance.Dict.Values
                .OrderBy(e => e.Category)
                .ThenBy(e => e.Order)
                .ThenBy(e => e.Id.ToString(), StringComparer.Ordinal)
                .ToList();

        public static bool IsUnlocked(CodexEntry entry) =>

            entry.Unlock == null || CoC.Instance.Flags.Get(entry.Unlock.Value) > 0;

        public override void Entry()
        {
            _category = "";

            _page = 0;

            // 原版 accessCodexMenu() 进代码库时先检查全收集成就
            CheckUnlocked();

            ShowCategories();
        }

        // 原版 Codex.hx checkUnlocked(returnMax = false)
        public static int CheckUnlocked(bool returnMax = false)
        {
            var num = 0;

            var max = 0;

            foreach (var entry in AllEntries)
            {
                max += 1;

                if (IsUnlocked(entry))
                    num += 1;
            }

            if (num == max && max > 0)
                AwardAchievement(new MonoCoC.Registry.Identifier("coc", "general_scholar"));

            return returnMax ? max : num;
        }

        public void ShowCategories()
        {
            ClearOutput();

            OutputText("Codex.Menu.Title".Localized());

            ClearButtons();

            var categories = AllEntries
                .Select(e => e.Category)
                .Where(c => !string.IsNullOrEmpty(c))
                .Distinct()
                .OrderBy(c => c, StringComparer.Ordinal)
                .ToList();

            if (categories.Count == 0)
            {
                OutputText("Codex.Menu.Empty".Localized());

                SetExitButton("Common.Button.Back".Localized());

                return;
            }

            for (var i = 0; i < categories.Count && i < PageSize; i++)
            {
                var category = categories[i];

                var button = AddButton(i, category.Localized(), () => ShowCategory(category));

                button?.Hint("Codex.Menu.CategoryHint".Localized());
            }

            SetExitButton("Common.Button.Back".Localized());
        }

        public void ShowCategory(string category)
        {
            _category = category;

            _page = 0;

            RenderCategory();
        }

        public void RenderCategory()
        {
            ClearOutput();

            OutputText("Codex.Menu.Title".Localized());

            OutputText(_category.Localized());

            ClearButtons();

            var entries = AllEntries.Where(e => e.Category == _category).ToList();

            var unlocked = entries.Count(IsUnlocked);

            OutputText("Codex.Menu.Progress".Localized(unlocked, entries.Count));

            var page = entries.Skip(_page * PageSize).Take(PageSize).ToList();

            for (var i = 0; i < page.Count; i++)
            {
                var entry = page[i];

                var isUnlocked = IsUnlocked(entry);

                // 原版 addCodexButton()：未解锁的条目显示 "???"，不能直接暴露名字
                var button = AddButton(i, isUnlocked ? entry.NameKey.Localized() : "Codex.Menu.LockedName".Localized(),

                    isUnlocked ? () => ShowEntry(entry) : null);

                button?.Hint(isUnlocked ? "Codex.Menu.EntryHint".Localized() : "Codex.Menu.LockedHint".Localized());
            }

            if (_page > 0)
                AddButton(12, "Codex.Menu.PrevPage".Localized(), () => { _page--; RenderCategory(); });

            if ((_page + 1) * PageSize < entries.Count)
                AddButton(13, "Codex.Menu.NextPage".Localized(), () => { _page++; RenderCategory(); });

            SetExitButton("Common.Button.Back".Localized(), ShowCategories);
        }

        public void ShowEntry(CodexEntry entry)
        {
            ClearOutput();

            OutputText(entry.NameKey.Localized());

            // 原版 Codex.hx 各 codexEntryXxx() 会连续输出多行；TextKeys 非空时按序拼接
            if (entry.TextKeys.Count > 0)
                foreach (var key in entry.TextKeys)
                    OutputText(key.Localized());
            else
                OutputText(entry.TextKey.Localized());

            AddNextButton("Common.Button.Back".Localized(), RenderCategory);
        }
    }
}
