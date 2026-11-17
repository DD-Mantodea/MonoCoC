using MonoCoC.Configuration;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.UI.Components.Settings;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;
using MonoCoC.Contents.Input;
using System.Linq;

namespace MonoCoC.UI.Components.Views
{
    public class OptionsView : TabPageView
    {
        private Button _resetControls = null!;

        private Button _clearControls = null!;

        private Label? _controlsNote;

        private double _controlsNoteTimer;

        protected override string ScreenFile => "Views\\OptionsView.hjson";

        public static OptionsView? Current { get; private set; }

        public string SearchText { get; set; } = "";

        public int InitialPage { get; set; }

        public SettingPane? Pane { get; private set; }

        public SearchBar? Search { get; set; }

        public Action? OnStart { get; set; }

        public Action? OnBack { get; set; }

        public bool ActionsRegistered { get; set; }

        protected override int ActiveTabIndex => PageIndex;

        public OptionsView()
        {
            Current = this;

            OptionStore.Instance.Initialize();

            Initialize();
        }

        protected override void OnPartsBound()
        {

            var panes = OptionStore.Instance.Panes;

            var pages = new List<TabPageDefinition>(panes.Count);

            for (var index = 0; index < panes.Count; index++)
                pages.Add(new TabPageDefinition(panes[index].Id, panes[index].ButtonKey, "", panes[index].Order));

            SetupTabPages(pages, selectFirst: false);

        }


        [UIRequired("options.controls.reset")]
        private void BindResetControls(Button part) => _resetControls = part;

        [UIRequired("options.controls.clear")]
        private void BindClearControls(Button part) => _clearControls = part;

        [UIRequired("options.controls.note")]
        private void BindControlsNote(Label part) => _controlsNote = part;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            var controls = ControlsOpen;

            _resetControls.Visible = controls;

            _clearControls.Visible = controls;

            if (!controls && _controlsNote is not null)
                _controlsNote.Visible = false;

            TickControlsNote(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        [UIEvent("options.controls.reset")]
        private static void ResetControls()
        {
            MonoCoC.Contents.Input.HotkeyRegistry.Reset();

            RefreshControls();
        }

        [UIEvent("options.controls.clear")]
        private static void ClearControls()
        {
            MonoCoC.Contents.Input.HotkeyRegistry.Clear();

            RefreshControls();
        }


        [UIEvent("options.back")]
        private static void Back() => BackOrSub();
        // 子页用回调表达（打开哪个子页由选项的 action 决定），不再用枚举常量。
        private Action? _detail;

        // Controls 是顶层页签，必须有自己的面板实例，不能复用数据页的 Pane。
        private SettingPane? _controlsPane;

        // 标签顺序：panes 索引，Controls 用 -1；与 CreateTabList 的构建顺序保持一致。
        public bool DetailOpen => _detail is not null;

        public bool ControlsOpen
        {
            get
            {
                var panes = OptionStore.Instance.Panes;

                return PageIndex >= 0 && PageIndex < panes.Count && panes[PageIndex].Kind == "controls";
            }
        }

        public void OpenDetail(Action page)
        {
            _detail = page;

            SearchText = "";

            RefreshPage();

            ShowSearch();
        }

        public void CloseDetail()
        {
            _detail = null;

            SearchText = "";

            RefreshPage();

            ShowSearch();
        }

        public static void Open() => Current?.OpenPage();

        public static void SwitchPane(int index) => Current?.SelectPage(index);

        public static void OpenControls() => Current?.OpenControlsPage();

        public static void BackOrSub() => Current?.BackOrSubPage();

        public static void RefreshControls() => Current?.RefreshControlsPage();

        public void OpenPage()
        {
            RegisterActions();

            OptionStore.Instance.Initialize();

            SelectPage(InitialPage > 0 && PageIndex == 0 ? InitialPage : PageIndex);

            if (!DetailOpen)
            {
                ShowSearch();

                return;
            }

            ShowSearch();
        }

        public void OpenControlsPage()
        {
            var panes = OptionStore.Instance.Panes;

            for (var index = 0; index < panes.Count; index++)
                if (panes[index].Kind == "controls")
                {
                    SelectPage(index);

                    return;
                }
        }
        public void BackOrSubPage()
        {
            // Controls 是顶层页签（和 Gameplay/Display 同级），Back 应该直接退出 Options，
            // 而不是退回上一个 Settings 页（2026-11-16 反馈）。
            if (DetailOpen)
            {
                CloseDetail();

                return;
            }

            if (Current?.OnStart != null)
            {
                Start();

                return;
            }

            if (Current?.OnBack != null)
            {
                BackPage();

                return;
            }

            Close();

            UIManager.Instance.Root.SwitchView(new CampView());
        }

        protected override void OnPageSelecting(int index, bool fromTab)
        {
            if (!fromTab)
                return;

            _detail = null;

            SearchText = "";
        }

        protected override void OnPageAttached(int index, Component? page)
        {
            Pane = page as SettingPane;
        }

        protected override void OnPageSelected(int index, bool fromTab) => ShowSearch();

        protected override Component? CreatePage(int index)
        {
            Framework.UIManager.TraceEvent("createPage " + index);
            var panes = OptionStore.Instance.Panes;

            if (panes.Count == 0)
                return null;

            index = Math.Clamp(index, 0, panes.Count - 1);

            var definition = panes[index];

            if (definition.Kind == "controls")
            {
                if (_controlsPane is null)
                {
                    _controlsPane = new SettingPane(330, 70, 940, 700).Of(Current);

                    Pane = _controlsPane;

                    ShowControlsPane();
                }
                else
                {
                    Pane = _controlsPane;

                    Pane.Visible = true;

                    // 复用同一实例，只原地刷新行（无闪烁）。
                    RefreshControlsPage();
                }

                Pane.ApplyFilter(SearchText);

                return Pane;
            }

            // 原版 GameSettings.hx:575：未加载存档时，本地设置页显示 SAVE NOT LOADED 警告
            if (!DetailOpen && !MonoCoC.CoC.Instance.Player.Loaded && !MonoCoC.CoC.Instance.Player.CharCreation && !definition.Global)
            {
                Pane = new SettingPane(330, 70, 940, 700).Of(Current);

                Pane.Header("SAVE NOT LOADED",

                    "The settings in this tab are local settings, saved to your character. Load a save or start a new game to change them.",

                    "");

                return Pane;
            }

            Pane = new SettingPane(330, 70, 940, 700).Of(Current);

            var note = definition.Global ? "View.Options.Note.Global" : "View.Options.Note.Local";

            Pane.Header(definition.TitleKey.Localized(), definition.DescriptionKey.Localized(), note.Localized());

            if (DetailOpen)
            {
                ShowSub();

                return Pane;
            }

            RenderPane(definition);

            Pane.ApplyFilter(SearchText);

            return Pane;
        }

        public void RenderPane(OptionPaneDefinition definition)
        {
            foreach (var setting in definition.Settings)
            {

                var values = setting.Source == "" ? setting.Values : OptionHandlers.ResolveSource(setting.Source);

                var current = OptionStore.Instance.Get(setting.Id, setting.DefaultValue);

                var selected = values.FirstOrDefault(value => value.Value == current);

                var descriptionKey = selected != null && selected.DescriptionKey != "" ? selected.DescriptionKey : setting.DescriptionKey;

                var description = descriptionKey == "" ? "" : descriptionKey.Localized();

                if (setting.Control == "dropdown")
                {
                    var languageIndex = Math.Max(0, values.FindIndex(value => value.Value == current));

                    Pane!.Dropdown(
                        setting.LabelKey.Localized(),
                        description,
                        [.. values.Select(value => value.LabelKey.Localized())],
                        languageIndex,
                        index => Select(setting, values[index]));

                    continue;
                }

                if (setting.Kind == "switcher")
                {
                    var index = Math.Max(0, values.FindIndex(value => value.Value == current));

                    var valueText = selected != null ? selected.LabelKey.Localized() : "";

                    Pane!.Switcher(setting.LabelKey.Localized(), description, valueText,

                        () => Cycle(setting, values, index, -1),

                        () => Cycle(setting, values, index, 1));

                    continue;
                }

                var currentText = setting.Action == "" && selected != null ? selected.LabelKey.Localized() : "";

                var choices = new List<(string, bool, Action)>();

                if (setting.Action != "")
                {
                    var action = setting.Action;

                    choices.Add(("View.Options.Choose".Localized(), false,
                        setting.SubPage is not null
                            ? () => OpenDetailFor(setting)
                            : () => OptionHandlers.InvokeAction(action)));
                }
                else
                    foreach (var value in values)
                    {
                        var option = value;

                        choices.Add((option.LabelKey.Localized(), option.Value == current, () => Select(setting, option)));
                    }

                Pane.Toggle(setting.LabelKey.Localized(), currentText, description, setting.OverridesLabel, [.. choices]);
            }
        }

        public void Cycle(OptionDefinition setting, List<OptionValueDefinition> values, int index, int direction)
        {
            if (values.Count == 0)
                return;

            var next = (Math.Clamp(index, 0, values.Count - 1) + direction + values.Count) % values.Count;

            Select(setting, values[next]);
        }

        public void Select(OptionDefinition setting, OptionValueDefinition choice)
        {
            OptionStore.Instance.Set(setting.Id, choice.Value);

            if (setting.Apply != "")
                OptionHandlers.InvokeApplier(setting.Apply, choice.Data);

            Pane?.UpdateValue(setting.LabelKey.Localized(), choice.LabelKey.Localized());
        }

        public void ShowSub() => _detail?.Invoke();

        public void OpenDetailFor(OptionDefinition setting)
        {
            var page = setting.SubPage;

            if (page is null)
                return;

            OpenDetail(() => SubPages.Render(page));
        }
        private OptionsSubPages? _subPages;

        private OptionsSubPages SubPages => _subPages ??= new OptionsSubPages(this);
        private void ShowControlsPane()
        {
            Pane!.Header("View.Options.Controls.Title".Localized(),

                "View.Options.Controls.Description".Localized(),

                "");

            foreach (var binding in HotkeyRegistry.All)
            {
                var definition = binding;

                Pane.BindRow(definition.Name + ":",

                    definition.Description,

                    KeyText(definition, true),

                    KeyFilled(definition, true),

                    KeyText(definition, false),

                    KeyFilled(definition, false),

                    () =>
                    {
                        HotkeyRegistry.BeginBind(definition, true);

                        RefreshControlsPage();
                    },

                    () =>
                    {
                        HotkeyRegistry.BeginBind(definition, false);

                        RefreshControlsPage();
                    });
            }
        }

        private static string KeyText(HotkeyDefinition binding, bool primary) =>
            IsPending(binding, primary)
                ? "View.Options.Controls.PressAKey".Localized()
                : HotkeyRegistry.KeyName(primary ? binding.Primary : binding.Secondary);

        private static bool KeyFilled(HotkeyDefinition binding, bool primary) =>
            IsPending(binding, primary) || (primary ? binding.Primary : binding.Secondary) != Microsoft.Xna.Framework.Input.Keys.None;

        private static bool IsPending(HotkeyDefinition binding, bool primary) =>
            HotkeyRegistry.PendingBinding == binding && HotkeyRegistry.PendingPrimary == primary;

        public void RefreshControlsPage()
        {
            if (!ControlsOpen || Pane == null)
                return;

            foreach (var binding in HotkeyRegistry.All)
                Pane.UpdateBindRow(binding.Name + ":",

                    KeyText(binding, true),

                    KeyFilled(binding, true),

                    KeyText(binding, false),

                    KeyFilled(binding, false));

            var notice = HotkeyRegistry.TakeNotice();

            if (notice.Length > 0)
                ShowControlsNote(notice);
        }

        // 提示条放在 pane 下方（不是 pane 内的浮层）：字号与正文一致，也不会盖住最后一行。
        public void ShowControlsNote(string text, double seconds = 5)
        {
            if (_controlsNote is null)
                return;

            _controlsNote.Text(text);

            _controlsNote.Visible = text.Length > 0;

            _controlsNoteTimer = _controlsNote.Visible ? seconds : 0;
        }

        public void TickControlsNote(double delta)
        {
            if (_controlsNote is null || !_controlsNote.Visible)
                return;

            _controlsNoteTimer -= delta;

            if (_controlsNoteTimer > 0)
                return;

            _controlsNote.Visible = false;

            _controlsNote.Text("");
        }

        public void RegisterActions()
        {
            if (ActionsRegistered)
                return;

            ActionsRegistered = true;

            // 子页现在由选项数据自带 sub 节点驱动，这里不再需要注册 action→页面映射。

            OptionHandlers.RegisterApplier("language", LanguageService.Use);

            OptionHandlers.RegisterSource("languages", LanguageService.Choices);
        }

        public void ShowSearch()
        {
            if (Search != null)
                return;

            Search = new SearchBar(text =>
            {
                SearchText = text;

                Pane?.ApplyFilter(text);
            }, "View.Options.Search".Localized()).Of(Current);
        }

        public void HideSearch()
        {
            if (Search == null)
                return;

            Search.LayoutData.Parent?.LayoutData.Children.Remove(Search);

            Search = null;
        }

        public void Close()
        {
            ShowSearch();

            SearchText = "";

            OptionStore.SaveGlobal();
        }

        public void BackPage()
        {
            Close();

            var action = Current?.OnBack;

            Current?.OnBack = null;

            action?.Invoke();
        }

        public void Start()
        {
            Close();

            var action = Current?.OnStart;

            Current?.OnStart = null;

            action?.Invoke();
        }
    }
}
