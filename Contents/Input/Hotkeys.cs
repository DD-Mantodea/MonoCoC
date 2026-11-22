using Microsoft.Xna.Framework.Input;
using MonoCoC.Configuration;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Saving;
using MonoCoC.UI.Components.Views;
using System;

namespace MonoCoC.Contents.Input
{
    public static class Hotkeys
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered)
                return;

            _registered = true;

            Register("show_stats", "Show Stats", "Show the stats pane when available", Keys.S, action: ToggleStats, canExecute: CanUseGameMenu);

            Register("level_up", "Level Up", "Show the level up page when available", Keys.L, action: () => Scene.Go(SceneInstance<LevelUpScene>.Instance), canExecute: CanUseGameMenu);

            Register("show_menu", "Show Menu", "Show the main menu", Keys.Back, action: ShowMainMenu, canExecute: CanUseGameMenu);

            Register("options", "Options", "Open the options menu", Keys.O, action: OpenOptions, canExecute: CanUseGameMenu);

            Register("data_menu", "Data Menu", "Show the save/load menu", Keys.D, action: OpenData, canExecute: CanUseGameMenu);

            Register("appearance_page", "Appearance Page", "Open the appearance page", Keys.A, action: () => Scene.Go(SceneInstance<PlayerAppearance>.Instance), canExecute: CanUseGameMenu);

            Register("no", "No", "Press the No button", Keys.N, action: () => PressYesNo(false), canExecute: CanUseGameMenu);

            Register("yes", "Yes", "Press the Yes button", Keys.Y, action: () => PressYesNo(true), canExecute: CanUseGameMenu);

            Register("show_perks", "Show Perks", "Show the perks page", Keys.P, action: () => Scene.Go(SceneInstance<PerksScene>.Instance), canExecute: CanUseGameMenu);

            Register("continue", "Continue", "Press the Continue button", Keys.Enter, Keys.Space, PressContinue, CanUseGameMenu);

            Register("cycle_background", "Cycle Background", "Cycle the background image", Keys.Home, action: CycleBackground, canExecute: CanUseGameMenu);

            Register("button_1", "Button 1", "Press button 1", Keys.D1, action: () => PressButton(0));

            Register("button_2", "Button 2", "Press button 2", Keys.D2, action: () => PressButton(1));

            Register("button_3", "Button 3", "Press button 3", Keys.D3, action: () => PressButton(2));

            Register("button_4", "Button 4", "Press button 4", Keys.D4, action: () => PressButton(3));

            Register("button_5", "Button 5", "Press button 5", Keys.D5, action: () => PressButton(4));

            Register("button_6", "Button 6", "Press button 6", Keys.D6, Keys.Q, () => PressButton(5));

            Register("button_7", "Button 7", "Press button 7", Keys.D7, Keys.W, () => PressButton(6));

            Register("button_8", "Button 8", "Press button 8", Keys.D8, Keys.E, () => PressButton(7));

            Register("button_9", "Button 9", "Press button 9", Keys.D9, Keys.R, () => PressButton(8));

            Register("button_10", "Button 10", "Press button 10", Keys.D0, Keys.T, () => PressButton(9));

            Register("button_11", "Button 11", "Press button 11", Keys.None, Keys.A, () => PressButton(10));

            Register("button_12", "Button 12", "Press button 12", Keys.None, Keys.S, () => PressButton(11));

            Register("button_13", "Button 13", "Press button 13", Keys.None, Keys.D, () => PressButton(12));

            Register("button_14", "Button 14", "Press button 14", Keys.None, Keys.F, () => PressButton(13));

            Register("button_15", "Button 15", "Press button 15", Keys.None, Keys.G, () => PressButton(14));

            HotkeyRegistry.Load();
        }

        private static void Register(string id, string name, string description, Keys primary,

            Keys secondary = Keys.None, Action? action = null, Func<bool>? canExecute = null) =>

            HotkeyRegistry.Register("coc:" + id, name, description, primary, secondary, action, canExecute);

        private static void ToggleStats()
        {
            var hidden = !UI.Components.Combat.PlayerStats.HiddenByMenu;

            UI.Components.Combat.PlayerStats.HiddenByMenu = hidden;

            UI.Components.Views.GameView.SetPlayerStatsVisible(!hidden);
        }

        private static int _backgroundIndex = 1;

        private static void CycleBackground()
        {
            var view = UI.Components.Views.GameView.Current;

            if (view == null)
                return;

            _backgroundIndex = _backgroundIndex % 4 + 1;

            if (view.Background is UI.Components.UIImage image)
                image.Texture = Assets.ManagerInstance<Assets.AssetManagers.TextureManager>.Instance["background" + _backgroundIndex];
        }

        private static bool CanUseGameMenu() =>
            CoC.Instance?.Player is { Loaded: true } && Scene.Current is not null;

        private static void ShowMainMenu() => MenuBarView.Toggle();

        private static void OpenOptions()
        {
            var options = new OptionsView();

            ViewNavigator.Open(options);

            OptionsView.Open();
        }

        private static void OpenData()
        {
            if (UI.Framework.UIManager.Instance.Root == null || Scene.Current == null)
                return;

            ViewNavigator.Open(new SaveLoadView());
        }

        private static void PressContinue()
        {
            foreach (var role in new[] { SceneActionRole.Continue, SceneActionRole.Exit })
            {
                foreach (var action in Scene.Actions)
                {
                    if (action.Role != role || !action.Visible || !action.Enabled || action.OnClick is null)
                        continue;

                    action.OnClick();

                    return;
                }
            }

            var fallback = Scene.ButtonAt(14);

            if (fallback is { Visible: true, Enabled: true })
                fallback.OnClick?.Invoke();
        }

        private static void PressYesNo(bool yes)
        {
            var button = Scene.ButtonAt(yes ? 0 : 1);

            if (button is { Visible: true, Enabled: true })
                button.OnClick?.Invoke();
        }

        private static void PressButton(int slot)
        {
            var button = Scene.ButtonAt(slot);

            if (button is { Visible: true, Enabled: true })
                button.OnClick?.Invoke();
        }

        private static void QuickSave(int slot)
        {
            if (!GameView.CanQuickSave)
                return;

            var name = QuickSlot(slot);

            if (OptionStore.Instance.Get("quicksaveConfirm", 1) == 0)
            {
                DoQuickSave(slot, name);

                return;
            }

            Scene.ClearButtons();

            Scene.ClearOutput();

            Scene.OutputText("Scenes.SaveLoad.QuickSaveConfirm".Localized(name));

            Scene.AddButton(0, "Common.Button.No".Localized(), Scene.PlayerMenu);

            Scene.AddButton(1, "Common.Button.Yes".Localized(), () => DoQuickSave(slot, name));
        }

        private static void QuickLoad(int slot)
        {
            var name = QuickSlot(slot);

            if (!SaveData.TryRead(name, out _))
                return;

            if (!GameView.CanQuickLoad)
                return;

            if (OptionStore.Instance.Get("quickloadConfirm", 1) == 0)
            {
                DoQuickLoad(name);

                return;
            }

            Scene.ClearButtons();

            Scene.ClearOutput();

            Scene.OutputText("Scenes.SaveLoad.QuickLoadConfirm".Localized(slot));

            Scene.AddButton(0, "Common.Button.No".Localized(), Scene.PlayerMenu);

            Scene.AddButton(1, "Common.Button.Yes".Localized(), () => DoQuickLoad(name));
        }

        private static string QuickSlot(int slot)
        {
            var hardcore = OptionStore.Instance.Get("hardcore", 0) > 0;

            var hardcoreSlot = OptionStore.Instance.Get("hardcoreSlot", 0);

            return hardcore && hardcoreSlot > 0 ? $"CoC_{hardcoreSlot}" : $"QuickSave{slot}";
        }

        private static void DoQuickSave(int slot, string name)
        {
            SaveData.Capture(CoC.Instance.Player, name).Write(name);

            Scene.ClearButtons();

            Scene.ClearOutput();

            Scene.OutputText("Scenes.SaveLoad.QuickSaved".Localized(name));

            Scene.AddNextButton("Common.Button.Next".Localized(), Scene.PlayerMenu);
        }

        private static void DoQuickLoad(string name)
        {
            if (!SaveData.TryRead(name, out var data))
                return;

            data.Apply(CoC.Instance.Player);

            Scene.Go(SceneInstance<CampScene>.Get());

            Scene.ClearOutput();

            Scene.OutputText("Scenes.SaveLoad.SlotLoaded".Localized(name));

            Scene.AddNextButton("Common.Button.Next".Localized(), Scene.PlayerMenu);
        }
    }
}
