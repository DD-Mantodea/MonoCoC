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
using MonoCoC;

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

        public Action? BackHandler { get; set; }

        public bool ActionsRegistered { get; set; }

        protected override int ActiveTabIndex => PageIndex;

        public OptionsView()
        {
            Current = this;

            OptionStore.Instance.Initialize();

            _pageRenderers["controls"] = CreateControlsPage;

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
            var controls = Pane is not null && ReferenceEquals(Pane, _controlsPane);

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
            Contents.Input.HotkeyRegistry.Reset();

            RefreshControls();
        }

        [UIEvent("options.controls.clear")]
        private static void ClearControls()
        {
            Contents.Input.HotkeyRegistry.Clear();

            RefreshControls();
        }


        [UIEvent("options.back")]
        private void Back() => HandleBack();

        private SettingPane? _controlsPane;

        private readonly Dictionary<string, Func<OptionPaneDefinition, SettingPane>> _pageRenderers = new(StringComparer.OrdinalIgnoreCase);

        public void OpenDetail(Action page)
        {
            SearchText = "";

            PushSubPage(page);

            ShowSearch();
        }

        public void CloseDetail()
        {
            SearchText = "";

            PopSubPage();

            ShowSearch();
        }

        public static void Open() => Current?.OpenPage();

        public static void RefreshControls() => Current?.RefreshControlsPage();

        public void OpenPage()
        {
            RegisterActions();

            OptionStore.Instance.Initialize();

            SelectPage(InitialPage > 0 && PageIndex == 0 ? InitialPage : PageIndex);

            ShowSearch();
        }

        public void HandleBack()
        {
            if (SubPageOpen)
            {
                CloseDetail();

                return;
            }

            Close();

            if (ViewNavigator.Back(this))
                return;

            var handler = BackHandler;

            BackHandler = null;

            handler?.Invoke();
        }

        protected override void OnPageSelecting(int index, bool fromTab)
        {
            if (!fromTab)
                return;

            ClearSubPages();

            SearchText = "";
        }

        protected override void OnPageAttached(int index, Component? page)
        {
            Pane = page as SettingPane;
        }

        protected override void OnPageSelected(int index, bool fromTab) => ShowSearch();

        protected override Component? CreatePage(int index)
        {
            UIManager.TraceEvent("createPage " + index);

            var panes = OptionStore.Instance.Panes;

            if (panes.Count == 0)
                return null;

            index = Math.Clamp(index, 0, panes.Count - 1);

            var definition = panes[index];

            var factory = _pageRenderers.TryGetValue(definition.Kind, out var registered)
                ? registered
                : CreateSettingsPage;

            Pane = factory(definition);

            if (!SubPageOpen)
                Pane.ApplyFilter(SearchText);

            return Pane;
        }

        private SettingPane CreateSettingsPage(OptionPaneDefinition definition)
        {
            if (!SubPageOpen && !CoC.Instance.Player.Loaded && !CoC.Instance.Player.CharCreation && !definition.Global)
            {
                var missing = new SettingPane(330, 70, 940, 700).Of(Current);

                missing.Header("SAVE NOT LOADED",

                    "The settings in this tab are local settings, saved to your character. Load a save or start a new game to change them.",

                    "");

                return missing;
            }

            var pane = new SettingPane(330, 70, 940, 700).Of(Current);

            Pane = pane;

            var note = definition.Global ? "View.Options.Note.Global" : "View.Options.Note.Local";

            pane.Header(definition.TitleKey.Localized(), definition.DescriptionKey.Localized(), note.Localized());

            if (SubPageOpen)
            {
                ShowSub();

                return pane;
            }

            RenderPane(definition);

            return pane;
        }

        private SettingPane CreateControlsPage(OptionPaneDefinition definition)
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

                RefreshControlsPage();
            }

            return _controlsPane;
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

        public void ShowSub() => CurrentSubPage?.Invoke();

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
            if (Pane is null || !ReferenceEquals(Pane, _controlsPane))
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
    }
}
