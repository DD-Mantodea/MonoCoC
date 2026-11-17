using Microsoft.Xna.Framework;
using Hjson;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.UI.Components.Views
{
    public abstract class TabPageView : HjsonView
    {
        private float[] _tabAnimation = [];

        protected readonly List<Button> Tabs = [];

        protected abstract int ActiveTabIndex { get; }

        public sealed record TabPageDefinition(string Id, string LabelKey, string PanelPart, int Order);

        private IReadOnlyList<TabPageDefinition> _pages = [];

        private Component?[] _pagePanels = [];

        protected IReadOnlyList<TabPageDefinition> Pages => _pages;

        protected IReadOnlyList<Component?> PagePanels => _pagePanels;

        private readonly Dictionary<int, Component> _pageCache = [];

        private Component? _currentPage;

        protected Component? CurrentPage => _currentPage;

        public int PageIndex { get; private set; }

        public int IndexOfPage(string id)
        {
            for (var index = 0; index < _pages.Count; index++)
                if (string.Equals(_pages[index].Id, id, StringComparison.OrdinalIgnoreCase))
                    return index;

            return -1;
        }

        // 基类统一：标签按钮、面板按 part 绑定、显隐切换、当前页索引；子类只实现 OnPageSelected。
        protected void SetupTabPages(IReadOnlyList<TabPageDefinition> pages, bool selectFirst = true)
        {
            _pages = pages;

            _pagePanels = new Component?[pages.Count];

            for (var index = 0; index < pages.Count; index++)
                if (pages[index].PanelPart.Length > 0)
                    _pagePanels[index] = Part<Component>(pages[index].PanelPart);

            var entries = new List<(string LabelKey, Action Selected)>(pages.Count);

            for (var index = 0; index < pages.Count; index++)
            {
                var target = index;

                entries.Add((pages[index].LabelKey, () => SelectPage(target, fromTab: true)));
            }

            BuildTabs(entries);

            if (selectFirst) SelectPage(0);
        }

        public void SelectPage(int index, bool fromTab = false)
        {
            if (index < 0 || index >= _pages.Count)
                return;

            OnPageSelecting(index, fromTab);

            PageIndex = index;

            for (var tab = 0; tab < Tabs.Count; tab++)
            {
                Tabs[tab].Selected = tab == index;

                Tabs[tab].RefreshVisual();
            }

            for (var i = 0; i < _pagePanels.Length; i++)
                if (_pagePanels[i] is not null)
                    _pagePanels[i]!.Visible = i == index;

            var page = GetPage(index);

            if (!ReferenceEquals(_currentPage, page))
            {
                if (_currentPage is not null)
                    _currentPage.Visible = false;

                _currentPage = page;

                if (page is not null)
                    page.Visible = true;
            }

            OnPageAttached(index, page);

            OnPageSelected(index, fromTab);
        }

        // 丢弃指定页的缓存并重新构建（子页打开/关闭、热刷新用）。
        protected void RefreshPage()
        {
            _pageCache.Remove(PageIndex);

            if (_currentPage is not null)
            {
                _currentPage.Visible = false;

                _currentPage = null;
            }

            SelectPage(PageIndex);
        }

        private Component? GetPage(int index)
        {
            if (_pageCache.TryGetValue(index, out var cached))
                return cached;

            var page = CreatePage(index);

            if (page is not null)
                _pageCache[index] = page;

            return page;
        }

        // 默认页内容就是定义里声明的 HJSON panel；动态页面的子类覆盖它。
        protected virtual Component? CreatePage(int index) => _pagePanels[index];

        protected virtual void OnPageSelecting(int index, bool fromTab)
        {
        }

        protected virtual void OnPageAttached(int index, Component? page)
        {
        }

        protected virtual void OnPageSelected(int index, bool fromTab)
        {
        }

        // 绑 Component 而不是 Grid：标签页数量由数据决定时，容器不一定用 Grid 排布。
        // 唯一的标签页实现：条目由子类提供，按钮从 TabButton 模板生成（模板缺省时退化为纯文字按钮）。
        protected void BuildTabs(IReadOnlyList<(string LabelKey, Action Selected)> entries)
        {
            var host = Part<Component>("page.tabs");

            Tabs.Clear();

            JsonObject? template = null;

            if (Document?.Templates is { } templates && templates.TryGetValue("TabButton", out var tabTemplate))
                template = tabTemplate;

            for (var index = 0; index < entries.Count; index++)
            {
                var entry = entries[index];

                var button = template is not null && Builder is not null
                    ? (Button)Builder.BuildTemplate(template!)
                    : new Button("", entry.LabelKey.Localized());

                button.SetLabel(entry.LabelKey.Localized());

                button.Selected = index == ActiveTabIndex;

                button.Click(entry.Selected);

                button.LayoutData.LayoutType = MonoCoC.UI.Layout.LayoutType.Relative;

                button.LayoutData.Size = new Vector2(200, 60);

                button.LayoutData.Position = new Vector2((host.Width > 0 ? host.Width : 300) / 2f - 100f, index * 72f);

                host.AddChild(button);

                Tabs.Add(button);
            }

            _tabAnimation = new float[Tabs.Count];

            // 首次构建时把标签直接放到最终视觉状态，避免入场从居中位置滑过来。
            for (var index = 0; index < Tabs.Count; index++)
            {
                _tabAnimation[index] = index == ActiveTabIndex ? 1f : 0f;

                ApplyTabVisual(Tabs[index], _tabAnimation[index]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (var index = 0; index < Tabs.Count; index++)
            {
                var target = index == ActiveTabIndex ? 1f : 0f;

                _tabAnimation[index] = MathHelper.Lerp(
                    _tabAnimation[index],
                    target,
                    MathHelper.Clamp(delta * 12f, 0f, 1f));

                ApplyTabVisual(Tabs[index], _tabAnimation[index]);
            }

            base.Update(gameTime);
        }

        private static void ApplyTabVisual(Button button, float progress)
        {
            button.FontSizeOverride = (int)Math.Round(20 + 4 * progress);

            button.LayoutData.Position.X = 28 - 20 * progress;

            button.RefreshLabelFit();

            button.RefreshVisual();
        }
    }
}
