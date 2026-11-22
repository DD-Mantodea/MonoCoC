using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    public class ExplorationView : GameView<ExplorationView>, ISceneHostView
    {
        public bool CanHost(Scene scene) => scene is ExplorationScene;
        public ExplorationView() => Initialize();

        protected override string ScreenFile => "Views\\ExplorationView.hjson";

        protected override void OnPartsBound()
        {
            Buttons = new Button[SlotCount];

            for (var index = 0; index < ButtonGrid.LayoutData.Children.Count && index < Buttons.Length; index++)
                if (ButtonGrid.LayoutData.Children[index] is Button button)
                    Buttons[index] = button;

            Scene.ReapplyActions();
        }

        [UIRequired("game.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("game.input")]
        private void BindInput(TextBox part) => Input = part;

        [UIRequired("game.output")]
        private void BindOutput(TextArea part) => Output = part;

        [UIRequired("game.playerStats")]
        private void BindPlayerStats(PlayerStats part) => PlayerStats = part;

        [UIRequired("game.buttons")]
        private void BindButtons(Grid part) => ButtonGrid = part;
    }
}
