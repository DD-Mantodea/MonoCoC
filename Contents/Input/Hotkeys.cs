using Microsoft.Xna.Framework.Input;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Saving;
using System;

namespace MonoCoC.Contents.Input
{
    // 原版 ControlBindings.hx 的默认键位。mod 不应调用这个类，直接用自己的 Id 调 HotkeyRegistry.Register。
    public static class Hotkeys
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered)
                return;

            _registered = true;

            Register("show_stats", "Show Stats", "Show the stats pane when available", Keys.S, action: ToggleStats);

            Register("level_up", "Level Up", "Show the level up page when available", Keys.L, action: () => Scene.Go(SceneInstance<LevelUpScene>.Instance));

            Register("quicksave_1", "Quicksave 1", "Quicksave the current game to slot 1", Keys.F1, action: () => QuickSave(1));

            Register("quicksave_2", "Quicksave 2", "Quicksave the current game to slot 2", Keys.F2, action: () => QuickSave(2));

            Register("quicksave_3", "Quicksave 3", "Quicksave the current game to slot 3", Keys.F3, action: () => QuickSave(3));

            Register("quicksave_4", "Quicksave 4", "Quicksave the current game to slot 4", Keys.F4, action: () => QuickSave(4));

            Register("quicksave_5", "Quicksave 5", "Quicksave the current game to slot 5", Keys.F5, action: () => QuickSave(5));

            Register("quickload_1", "Quickload 1", "Quickload the current game from slot 1", Keys.F6, action: () => QuickLoad(1));

            Register("quickload_2", "Quickload 2", "Quickload the current game from slot 2", Keys.F7, action: () => QuickLoad(2));

            Register("quickload_3", "Quickload 3", "Quickload the current game from slot 3", Keys.F8, action: () => QuickLoad(3));

            Register("quickload_4", "Quickload 4", "Quickload the current game from slot 4", Keys.F9, action: () => QuickLoad(4));

            Register("quickload_5", "Quickload 5", "Quickload the current game from slot 5", Keys.F10, action: () => QuickLoad(5));

            Register("show_menu", "Show Menu", "Show the main menu", Keys.Back, action: ShowMainMenu);

            Register("options", "Options", "Open the options menu", Keys.O, action: OpenOptions);

            Register("data_menu", "Data Menu", "Show the save/load menu", Keys.D, action: OpenData);

            Register("appearance_page", "Appearance Page", "Open the appearance page", Keys.A, action: () => Scene.Go(SceneInstance<PlayerAppearance>.Instance));

            Register("no", "No", "Press the No button", Keys.N, action: () => PressYesNo(false));

            Register("yes", "Yes", "Press the Yes button", Keys.Y, action: () => PressYesNo(true));

            Register("show_perks", "Show Perks", "Show the perks page", Keys.P, action: () => Scene.Go(SceneInstance<PerksScene>.Instance));

            Register("continue", "Continue", "Press the Continue button", Keys.Enter, Keys.Space, PressContinue);

            Register("cycle_background", "Cycle Background", "Cycle the background image", Keys.Home, action: CycleBackground);

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

            Keys secondary = Keys.None, Action? action = null) =>

            HotkeyRegistry.Register("coc:" + id, name, description, primary, secondary, action);

        private static void ToggleStats()
        {
            var hidden = !UI.Components.Combat.PlayerStats.HiddenByMenu;

            UI.Components.Combat.PlayerStats.HiddenByMenu = hidden;

            UI.Components.Views.GameView.SetPlayerStatsVisible(!hidden);
        }

        private static int _backgroundIndex = 1;

        private static void CycleBackground()
        {
            var view = UI.Components.Views.GameView.Instance;

            if (view == null)
                return;

            _backgroundIndex = _backgroundIndex % 4 + 1;

            if (view.Background is MonoCoC.UI.Components.UIImage image)
                image.Texture = Assets.ManagerInstance<MonoCoC.Assets.AssetManagers.TextureManager>.Instance["background" + _backgroundIndex];
        }

        private static void ShowMainMenu()
        {
            UI.Framework.UIManager.Instance.Root.SwitchView(
                new MonoCoC.UI.Components.Views.MainView());
        }

        private static void OpenOptions() => Scene.Go(SceneInstance<Options>.Instance);

        private static void OpenData()
        {
            // 启动阶段 UI Root/Scene 尚未初始化时触发热键会 NRE
            if (UI.Framework.UIManager.Instance.Root == null || Scene.Current == null)
                return;

            Scene.Go(SceneInstance<SaveLoad>.Instance);
        }

        private static void PressContinue()
        {
            var button = Scene.ButtonAt(14);

            if (button is { Visible: true, Enabled: true })
                button.OnClick?.Invoke();
        }

        private static void PressYesNo(bool yes)
        {
            var button = Scene.ButtonAt(yes ? 1 : 0);

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
            if (CoC.Instance.Player.Loaded)
                SaveData.Capture(CoC.Instance.Player, $"QuickSave{slot}").Write($"QuickSave{slot}");
        }

        private static void QuickLoad(int slot)
        {
            if (SaveData.TryRead($"QuickSave{slot}", out var data))
            {
                data.Apply(CoC.Instance.Player);

                Scene.Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get());
            }
        }
    }
}