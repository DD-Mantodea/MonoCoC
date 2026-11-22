using MonoCoC.Contents.Dungeons;
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
                BackHandler = () => UIManager.Instance.Root.SwitchView(new MainView())
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
            if (CoC.Instance.Player.Loaded)
            {
                if (GameView.Current is InventoryScreen)
                {
                    SwitchToSceneView();

                    return;
                }

                if (GameView.Current is not null)
                {
                    UIManager.Instance.Root.SwitchView(GameView.Current);

                    return;
                }

                SwitchToSceneView();

                return;
            }

            var save = SaveData.List().OrderByDescending(entry => entry.SaveTime).FirstOrDefault();

            if (save is null)
                return;

            save.Apply(CoC.Instance.Player);

            GameView.SwitchTo<CampView>();

            Scene.Go(SceneInstance<CampScene>.Get());
        }

        private static void SwitchToSceneView()
        {
            var scene = Scene.Current;

            if (scene is DungeonScene)
                GameView.SwitchTo<DungeonView>();
            else if (scene is ExplorationScene)
                GameView.SwitchTo<ExplorationView>();
            else
                GameView.SwitchTo<CampView>();

            if (scene is not null)
            {
                Scene.Go(scene);

                return;
            }

            Scene.Go(SceneInstance<CampScene>.Get());
        }

        [UIEvent("main.load_game")]
        private static void OpenData()
        {
            UIManager.Instance.Root.SwitchView(new SaveLoadView(true));
        }

        [UIEvent("main.achievements")]
        private static void OpenAchievements()
        {
            UIManager.Instance.Root.SwitchView(new AchievementsView());
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
