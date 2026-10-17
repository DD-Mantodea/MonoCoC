using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Saving;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.Linq;

namespace MonoCoC.UI.Components.Views
{
    /// <summary>
    /// Core main-menu event handlers used by Hjson onClick bindings.
    /// </summary>
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

            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = true;

            Options.OnBack = () =>
            {
                MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = false;

                UIManager.Instance.Root.SwitchView(new MainView());
            };

            UIManager.Instance.Root.SwitchView(new GameView());

            Scene.Go(SceneInstance<Options>.Instance);
        }

        private static void NewGame()
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = false;

            UIManager.Instance.Root.SwitchView(new GameView());

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

            var gameView = new GameView();

            save.Apply(CoC.Instance.Player);

            Scene.Go(SceneInstance<CampScene>.Get());

            UIManager.Instance.Root.SwitchView(gameView);
        }

        private static void OpenData()
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = true;

            SaveLoad.FromMainMenu = true;

            UIManager.Instance.Root.SwitchView(new GameView());

            Scene.Go(SceneInstance<SaveLoad>.Instance);
        }

        private static void OpenAchievements()
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = true;

            UIManager.Instance.Root.SwitchView(new GameView());

            Scene.Go(SceneInstance<AchievementsScene>.Instance);
        }

        private static void OpenInstructions()
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = true;

            UIManager.Instance.Root.SwitchView(new GameView());

            Scene.Go(SceneInstance<InstructionsScene>.Instance);
        }

        private static void OpenCredits()
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = true;

            UIManager.Instance.Root.SwitchView(new GameView());

            Scene.Go(SceneInstance<CreditsScene>.Instance);
        }

        private static void QuitGame() => CoC.Instance.Exit();
    }
}
