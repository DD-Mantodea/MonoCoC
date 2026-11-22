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

        public bool SelectPage(string id)
        {
            var target = IndexOfPage(id);

            if (target < 0)
                return false;

            SelectPage(target);

            return true;
        }

        public int IndexOfPage(string id)
        {
            for (var index = 0; index < _pages.Count; index++)
                if (string.Equals(_pages[index].Id, id, StringComparison.OrdinalIgnoreCase))
                    return index;

            return -1;
        }

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
                _currentPage?.Visible = false;

                _currentPage = page;

                page?.Visible = true;
            }

            OnPageAttached(index, page);

            UIManager.Instance?.MarkSceneChanged();

            OnPageSelected(index, fromTab);
        }

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

        protected virtual Component? CreatePage(int index) => _pagePanels[index];

        private readonly Stack<Action> _subPageStack = [];

        protected bool SubPageOpen => _subPageStack.Count > 0;

        protected Action? CurrentSubPage => _subPageStack.Count > 0 ? _subPageStack.Peek() : null;

        protected void PushSubPage(Action renderer)
        {
            _subPageStack.Push(renderer);

            RefreshPage();
        }

        protected void PopSubPage()
        {
            if (_subPageStack.Count == 0)
                return;

            _subPageStack.Pop();

            RefreshPage();
        }

        protected void ClearSubPages() => _subPageStack.Clear();
        protected virtual void OnPageSelecting(int index, bool fromTab)
        {
        }

        protected virtual void OnPageAttached(int index, Component? page)
        {
        }

        protected virtual void OnPageSelected(int index, bool fromTab)
        {
        }

        protected void BuildTabs(IReadOnlyList<(string LabelKey, Action Selected)> entries)
        {
            var host = Part<Component>("page.tabs");

            Tabs.Clear();

            JsonObject? template = null;

            if (Document?.Styles is { } templates && templates.TryGetValue("TabButton", out var tabTemplate))
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
