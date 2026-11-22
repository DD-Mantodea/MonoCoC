using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    public class MenuPageView : GameView<MenuPageView>
    {
        public MenuPageView() => Initialize();

        protected override string ScreenFile => "Views\\MenuPageView.hjson";

        protected override void OnPartsBound()
        {
            Buttons = new Button[SlotCount];

            for (var index = 0; index < ButtonGrid.LayoutData.Children.Count && index < Buttons.Length; index++)
                if (ButtonGrid.LayoutData.Children[index] is Button button)
                    Buttons[index] = button;

            Scene.ReapplyActions();
        }

        [UIRequired("page.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("page.output")]
        private void BindOutput(TextArea part) => Output = part;

        [UIRequired("page.buttons")]
        private void BindButtons(Grid part) => ButtonGrid = part;
    }
}
