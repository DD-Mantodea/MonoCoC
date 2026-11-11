using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    public class CampView : GameView
    {
        protected override string ScreenFile => "Views\\CampView.hjson";

        protected override UIViewContract Contract => CampContract.Value;

        protected override void OnBuilt(View root, UIDocument document)
        {
            base.OnBuilt(root, document);

            Scene.ReapplyActions();
        }

        protected override void BindParts()
        {
            Background = RequirePart<Component>("game.background");

            Input = RequirePart<TextBox>("game.input");

            Output = RequirePart<TextArea>("game.output");

            PlayerStats = RequirePart<PlayerStats>("game.playerStats");

            ButtonGrid = RequirePart<Grid>("game.buttons");

            Buttons = new Button[SlotCount];

            for (var index = 0; index < ButtonGrid.LayoutData.Children.Count && index < Buttons.Length; index++)
                if (ButtonGrid.LayoutData.Children[index] is Button button)
                    Buttons[index] = button;
        }
    }

    internal static class CampContract
    {
        public static readonly UIViewContract Value = new(
            "CampView",
            [
                new("game.background", typeof(Component)),
                new("game.input", typeof(TextBox)),
                new("game.output", typeof(TextArea)),
                new("game.playerStats", typeof(PlayerStats)),
                new("game.buttons", typeof(Grid))
            ]);
    }
}
