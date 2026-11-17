using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Saving;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.Linq;

namespace MonoCoC.UI.Components.Views
{
    public class MainView : HjsonView
    {
        protected override string ScreenFile => "Views\\MainView.hjson";

        public MainView()
        {
            Initialize();
        }

        [UIEvent("main.options")]
        private static void OpenOptions()
        {
            var view = new OptionsView
            {
                OnBack = () => UIManager.Instance.Root.SwitchView(new MainView())
            };

            UIManager.Instance.Root.SwitchView(view);

            OptionsView.Open();
        }

        [UIEvent("main.new_game")]
        private static void NewGame()
        {
            Combat.PlayerStats.HiddenByMenu = false;

            var view = new CharCreationView();

            if (view.Content is null)
                return;

            UIManager.Instance.Root.SwitchView(view);

            //Scene.Go(SceneInstance<CharCreation>.Instance);
        }

        [UIEvent("main.continue")]
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

        [UIEvent("main.load_game")]
        private static void OpenData()
        {
            SaveLoad.FromMainMenu = true;

            UIManager.Instance.Root.SwitchView(new SaveLoadView());

            Scene.Go(SceneInstance<SaveLoad>.Instance);
        }

        [UIEvent("main.achievements")]
        private static void OpenAchievements()
        {
            UIManager.Instance.Root.SwitchView(new AchievementsView());

            Scene.Go(SceneInstance<AchievementsScene>.Instance);
        }

        [UIEvent("main.instructions")]
        private static void OpenInstructions() =>
            UIManager.Instance.Root.SwitchView(new InstructionsView());

        [UIEvent("main.credits")]
        private static void OpenCredits() =>
            UIManager.Instance.Root.SwitchView(new CreditView());

        [UIEvent("main.quit")]
        private static void QuitGame() => CoC.Instance.Exit();
    }
}
