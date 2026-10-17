using MonoCoC.Contents.Input;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using static MonoCoC.UI.Layout.LayoutConstants;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<Options>("coc:options")]
    public class Options : Scene
    {
        public const int NoSub = 0;

        public const int FontSub = 1;

        public const int TextBackgroundSub = 2;

        public const int ThemeSub = 3;

        public const int SurvivalSub = 4;

        public const int RealisticSub = 5;

        public const int HardcoreSub = 6;

        public const int ControlsSub = 7;

        // 从主菜单进入设置时隐藏 Player 属性栏（原版 hideStats）
        public static bool HidePlayerStats;

        public static Action? OnStart;

        public static Action? OnBack;

        public static int _paneIndex;

        public static int _sub;

        public static string _searchText = "";

        public static SettingPane? _pane;

        public static SearchBar? _search;

        public static bool _actionsRegistered;

        public static void SelectPane(int index) => _paneIndex = index;

        public override void Entry()
        {
            RegisterActions();

            OptionStore.Instance.Initialize();

            ClearOutput();

            ClearButtons();

            ShowPane(_paneIndex);

            if (_sub == NoSub)
            {
                ShowTabs();

                ShowSearch();

                if (OnStart != null)
                {
                    AddButton(14, "Common.Continue".Localized(), Start);

                    return;
                }

                if (OnBack != null)
                {
                    AddButton(14, "Common.Back".Localized(), Back);

                    return;
                }

                SetExitButton("Common.Back".Localized(), () =>
                {
                    Close();

                    Go<CampScene>();
                });

                return;
            }

            HideSearch();

            AddButton(14, "Common.Back".Localized(), () => GoSub(NoSub));
        }

        public static void ShowPane(int index)
        {
            HidePane();

            var panes = OptionStore.Instance.Panes;

            if (panes.Count == 0)
                return;

            _paneIndex = Math.Clamp(index, 0, panes.Count - 1);

            var definition = panes[_paneIndex];

            // 原版 GameSettings.hx:575：未加载存档时，本地设置页显示 SAVE NOT LOADED 警告
            if (_sub == NoSub && !Player.Loaded && !Player.CharCreation && !definition.Global)
            {
                _pane = new SettingPane(TextZoneX, TextZoneY, TextZoneWidth, TextZoneHeight)
                    .Of(View);

                _pane.Header("SAVE NOT LOADED",

                    "The settings in this tab are local settings, saved to your character. Load a save or start a new game to change them.",

                    "");

                return;
            }

            _pane = new SettingPane(TextZoneX, TextZoneY, TextZoneWidth, TextZoneHeight)
                .Of(View);

            var note = definition.Global ? "Scenes.Options.Note.Global" : "Scenes.Options.Note.Local";

            _pane.Header(definition.TitleKey.Localized(), definition.DescriptionKey.Localized(), note.Localized());

            if (_sub != NoSub)
            {
                ShowSub();

                return;
            }

            RenderPane(definition);

            _pane.ApplyFilter(_searchText);
        }

        public static void RenderPane(OptionPaneDefinition definition)
        {
            foreach (var setting in definition.Settings)
            {
                var values = setting.Source == "" ? setting.Values : OptionHandlers.ResolveSource(setting.Source);

                var current = OptionStore.Instance.Get(setting.Id, setting.DefaultValue);

                var selected = values.FirstOrDefault(value => value.Value == current);

                var descriptionKey = selected != null && selected.DescriptionKey != "" ? selected.DescriptionKey : setting.DescriptionKey;

                var description = descriptionKey == "" ? "" : descriptionKey.Localized();

                if (setting.Kind == "switcher")
                {
                    var index = Math.Max(0, values.FindIndex(value => value.Value == current));

                    var valueText = selected != null ? selected.LabelKey.Localized() : "";

                    _pane!.Switcher(setting.LabelKey.Localized(), description, valueText,

                        () => Cycle(setting, values, index, -1),

                        () => Cycle(setting, values, index, 1));

                    continue;
                }

                var currentText = setting.Action == "" && selected != null ? selected.LabelKey.Localized() : "";

                var choices = new List<(string, bool, Action)>();

                if (setting.Action != "")
                    choices.Add(("Scenes.Options.Choose".Localized(), false, () => OptionHandlers.InvokeAction(setting.Action)));
                else
                    foreach (var value in values)
                    {
                        var option = value;

                        choices.Add((option.LabelKey.Localized(), option.Value == current, () => Select(setting, option)));
                    }

                _pane!.Toggle(setting.LabelKey.Localized(), currentText, description, setting.OverridesLabel, choices.ToArray());
            }
        }

        public static void Cycle(OptionDefinition setting, List<OptionValueDefinition> values, int index, int direction)
        {
            if (values.Count == 0)
                return;

            var next = (Math.Clamp(index, 0, values.Count - 1) + direction + values.Count) % values.Count;

            Select(setting, values[next]);
        }

        public static void Select(OptionDefinition setting, OptionValueDefinition choice)
        {
            OptionStore.Instance.Set(setting.Id, choice.Value);

            if (setting.Apply != "")
                OptionHandlers.InvokeApplier(setting.Apply, choice.Data);

            Go<Options>();
        }

        public static void ShowSub()
        {
            switch (_sub)
            {
                case FontSub:
                    _pane!.Header("Scenes.Options.Sub.Two.Title".Localized(), "", "");

                    _pane.Message("Scenes.Options.Sub.Font.Current".Localized(OptionStore.Instance.Get("fontSize", 20)));

                    FontRow("Smaller", -1);

                    FontRow("Larger", 1);

                    FontRow("Reset", 0);

                    break;

                case TextBackgroundSub:
                    _pane!.Header("Scenes.Options.Sub.Six.Title".Localized(), "", "");

                    TextBackgroundRow(-1, "Clear");

                    TextBackgroundRow(0, "Theme");

                    TextBackgroundRow(3, "Normal");

                    TextBackgroundRow(1, "White");

                    TextBackgroundRow(2, "Tan");

                    break;

                case ThemeSub:
                    _pane!.Header("Scenes.Options.Sub.Seven.Title".Localized(), "", "");

                    _pane.Message("Scenes.Options.Pane.NotImplemented".Localized());

                    break;

                case SurvivalSub:
                    ConfirmSub(3, "survival", "Scenes.Options.Sub.Survival.Warning");

                    break;

                case RealisticSub:
                    ConfirmSub(4, "realistic", "Scenes.Options.Sub.Realistic.Warning");

                    break;

                case HardcoreSub:
                    HardcorePage();

                    break;

                case ControlsSub:
                    ShowControlsPane();

                    break;
            }
        }

        private static void ShowControlsPane()
        {
            // 原版 BindingPane：说明文字 + 每行 Primary/Secondary 两个键位按钮
            _pane!.Header("Keyboard Control Bindings",

                "Click a button next to the action you wish to bind to a new key, then hit the key you want to bind the selected action to.\n\n"

                + "Custom bindings are stored with your global settings.\n"

                + "Duplicate keys are automatically unbound from their old control action.\n"

                + "Reset Ctrls will reset all of the control bindings to their defaults.\n"

                + "Clear Ctrls will remove all of the current control bindings, leaving everything unbound.",

                "");

            foreach (var binding in HotkeyRegistry.All)
            {
                var primary = HotkeyRegistry.PendingBinding == binding && HotkeyRegistry.PendingPrimary

                    ? "Press a key..."

                    : HotkeyRegistry.KeyName(binding.Primary);

                var secondary = HotkeyRegistry.PendingBinding == binding && !HotkeyRegistry.PendingPrimary

                    ? "Press a key..."

                    : HotkeyRegistry.KeyName(binding.Secondary);

                _pane.BindRow(binding.Name + ":",

                    primary,

                    secondary,

                    () =>
                    {
                        HotkeyRegistry.BeginBind(binding, true);

                        ShowSub();
                    },

                    () =>
                    {
                        HotkeyRegistry.BeginBind(binding, false);

                        ShowSub();
                    });
            }

            AddButton(0, "Reset Ctrls", () =>
            {
                HotkeyRegistry.Reset();

                ShowSub();
            });

            AddButton(1, "Clear Ctrls", () =>
            {
                HotkeyRegistry.Clear();

                ShowSub();
            });
        }

        public static void RefreshControls()
        {
            if (_sub == ControlsSub && _pane != null)
                ShowSub();
        }

        public static void ConfirmSub(int sub, string id, string warningKey)
        {
            _pane!.Header(("Scenes.options.Sub." + sub + ".Title").Localized(), "", "");

            _pane.Message(warningKey.Localized());

            _pane.Toggle("Scenes.Options.Enable".Localized(), "", "", false,

                ("Scenes.Options.Enable".Localized(), false, () =>
                {
                    OptionStore.Instance.Set(id, 1);

                    GoSub(NoSub);
                }));

            _pane.Toggle("Scenes.Options.Cancel".Localized(), "", "", false,

                ("Scenes.Options.Cancel".Localized(), false, () => GoSub(NoSub)));
        }

        public static void HardcorePage()
        {
            _pane!.Header("Scenes.Options.Sub.Five.Title".Localized(), "", "");

            _pane.Message("Scenes.Options.Sub.Hardcore.Warning".Localized());

            for (var slot = 1; slot <= 14; slot++)
            {
                var value = slot;

                _pane.Toggle("Scenes.Options.Slot".Localized(slot), "", "", false,

                    ("Scenes.Options.Enable".Localized(), false, () =>
                    {
                        OptionStore.Instance.Set("hardcore", 1);

                        OptionStore.Instance.Set("hardcoreSlot", value);

                        GoSub(NoSub);
                    }));
            }
        }

        public static void FontRow(string key, int delta)
        {
            _pane!.Toggle(("Scenes.options.Font." + key).Localized(), "", "", false,

                ("Scenes.Options.Choose".Localized(), false, () =>
                {
                    var current = OptionStore.Instance.Get("fontSize", 20);

                    OptionStore.Instance.Set("fontSize", delta == 0 ? 20 : Math.Clamp(current + delta, 14, 32));

                    GoSub(FontSub);
                }));
        }

        public static void TextBackgroundRow(int value, string key)
        {
            _pane!.Toggle(("Scenes.options.TextBackground." + key).Localized(), "", "", false,

                ("Scenes.Options.Choose".Localized(), OptionStore.Instance.Get("textBackground") == value, () =>
                {
                    OptionStore.Instance.Set("textBackground", value);

                    GoSub(NoSub);
                }));
        }

        public static void ShowTabs()
        {
            for (var index = 0; index < OptionStore.Instance.Panes.Count; index++)
            {
                var pane = OptionStore.Instance.Panes[index];

                var target = index;

                var row = index < 3 ? 0 : 1;

                var button = AddRowButton(row, pane.ButtonKey.Localized(), () =>
                {
                    _sub = NoSub;

                    _paneIndex = target;

                    HidePane();

                    Go<Options>();
                });

                button?.DisableIf(index == _paneIndex);

                if (index == 2)
                    AddRowButton(0, "Scenes.Options.Tab.Controls".Localized(), () => GoSub(ControlsSub));
            }
        }

        public static void RegisterActions()
        {
            if (_actionsRegistered)
                return;

            _actionsRegistered = true;

            OptionHandlers.RegisterAction("fontSize", () => GoSub(FontSub));

            OptionHandlers.RegisterAction("textBackground", () => GoSub(TextBackgroundSub));

            OptionHandlers.RegisterAction("theme", () => GoSub(ThemeSub));

            OptionHandlers.RegisterAction("survival", () => GoSub(SurvivalSub));

            OptionHandlers.RegisterAction("realistic", () => GoSub(RealisticSub));

            OptionHandlers.RegisterAction("hardcore", () => GoSub(HardcoreSub));

            OptionHandlers.RegisterApplier("language", LanguageService.Use);

            OptionHandlers.RegisterSource("languages", LanguageService.Choices);
        }

        public static void ShowSearch()
        {
            if (_search != null)
                return;

            _search = new SearchBar(text =>
            {
                _searchText = text;

                _pane?.ApplyFilter(text);
            }, "Scenes.Options.Search".Localized()).Of(View);
        }

        public static void HideSearch()
        {
            if (_search == null)
                return;

            _search.LayoutData.Parent?.LayoutData.Children.Remove(_search);

            _search = null;
        }

        public static void GoSub(int sub)
        {
            _sub = sub;

            _searchText = "";

            Go<Options>();
        }

        public static void HidePane()
        {
            if (_pane == null)
                return;

            _pane.LayoutData.Parent?.LayoutData.Children.Remove(_pane);

            _pane = null;
        }

        public static void Close()
        {
            HidePane();

            HideSearch();

            _searchText = "";

            OptionStore.SaveGlobal();
        }

        public static void Back()
        {
            Close();

            var action = OnBack;

            OnBack = null;

            action?.Invoke();
        }

        public static void Start()
        {
            Close();

            var action = OnStart;

            OnStart = null;

            action?.Invoke();
        }
    }
}
