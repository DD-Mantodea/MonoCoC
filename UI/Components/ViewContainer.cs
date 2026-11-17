using MonoCoC.Extensions;
using MonoCoC.UI.Components.Views;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public class ViewContainer : Component
    {
        public View View;

        public ViewContainer()
        {
            View = new MainView().Of(this);
        }

        public void SwitchView(View view)
        {
            var console = LayoutData.Children.Find(child => child is UI.Console.ConsoleOverlay);

            RemoveAllChild();

            View = view.Of(this);

            console?.Of(this);

            UIManager.TraceEvent("switch " + view.GetType().Name);

            UIManager.Instance?.MarkSceneChanged();
        }
    }
}