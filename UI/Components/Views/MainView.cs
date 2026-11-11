using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Saving;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.IO;
using System.Linq;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    /// <summary>
    /// Data-driven main menu. The Hjson document drives existing controls;
    /// only this screen is swapped when the document changes.
    /// </summary>
    public class MainView : HjsonView
    {
        protected override string ScreenFile => "Views\\MainView.hjson";

        protected override UIViewContract Contract => MainMenuContract.Value;

        private readonly UIEventRegistry _events;

        private readonly UIWidgetRegistry _widgets;

        public MainView()
        {
            _events = MainMenuEvents.Create();

            _widgets = new UIWidgetRegistry();

            Initialize(_events, _widgets);
        }
    }

    internal static class MainMenuContract
    {
        public static readonly UIViewContract Value = new(
            "MainMenu",
            [
                new("main.buttons", typeof(Grid)),
                new("main.version", typeof(Label)),
            ]);
    }

    public static class MainMenuEvents
    {
        public static UIEventRegistry Create()
        {
            var events = new UIEventRegistry();

            events.RegisterEvent("main.continue", ContinueGame);
            events.RegisterEvent("main.new_game", NewGame);
            events.RegisterEvent("main.load_game", OpenData);
            events.RegisterEvent("main.options", OpenOptions);
            events.RegisterEvent("main.achievements", OpenAchievements);
            events.RegisterEvent("main.instructions", OpenInstructions);
            events.RegisterEvent("main.credits", OpenCredits);
            events.RegisterEvent("main.quit", QuitGame);

            return events;
        }

        private static void OpenOptions()
        {
            Options.OnStart = null;

            Combat.PlayerStats.HiddenByMenu = true;

            Options.OnBack = () =>
            {
                Combat.PlayerStats.HiddenByMenu = false;

                UIManager.Instance.Root.SwitchView(new MainView());
            };

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<Options>.Instance);
        }

        private static void NewGame()
        {
            Combat.PlayerStats.HiddenByMenu = false;

            var view = new CampView();

            if (view.Content is null)
                return;

            UIManager.Instance.Root.SwitchView(view);

            Scene.Go(SceneInstance<CharCreation>.Instance);
        }

        private static void ContinueGame()
        {
            if (CoC.Instance.Player.Loaded && GameView.Instance != null)
            {
                UIManager.Instance.Root.SwitchView(GameView.Instance);

                return;
            }

            var save = SaveData.List().OrderByDescending(entry => entry.SaveTime).FirstOrDefault();

            if (save is null)
                return;

            var gameView = new CampView();

            save.Apply(CoC.Instance.Player);

            Scene.Go(SceneInstance<CampScene>.Get());

            UIManager.Instance.Root.SwitchView(gameView);
        }

        private static void OpenData()
        {
            Combat.PlayerStats.HiddenByMenu = true;

            SaveLoad.FromMainMenu = true;

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<SaveLoad>.Instance);
        }

        private static void OpenAchievements()
        {
            Combat.PlayerStats.HiddenByMenu = true;

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<AchievementsScene>.Instance);
        }

        private static void OpenInstructions()
        {
            Combat.PlayerStats.HiddenByMenu = true;

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<InstructionsScene>.Instance);
        }

        private static void OpenCredits()
        {
            Combat.PlayerStats.HiddenByMenu = true;

            UIManager.Instance.Root.SwitchView(new CampView());

            Scene.Go(SceneInstance<CreditsScene>.Instance);
        }

        private static void QuitGame() => CoC.Instance.Exit();
    }
}


