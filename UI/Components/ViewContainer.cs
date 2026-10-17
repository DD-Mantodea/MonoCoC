using MonoCoC.Extensions;
using MonoCoC.UI.Components.Views;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public class ViewContainer : Component
    {
        public View Scene;

        public ViewContainer()
        {
            Scene = new MainView().Of(this);
        }

        public void SwitchView(View scene)
        {
            RemoveAllChild();

            Scene = scene.Of(this);

            UIManager.Instance?.MarkSceneChanged();
        }
    }
}