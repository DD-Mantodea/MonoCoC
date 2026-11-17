using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    public class CampView : GameView
    {
        public CampView() => Initialize();

        protected override string ScreenFile => "Views\\CampView.hjson";

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
