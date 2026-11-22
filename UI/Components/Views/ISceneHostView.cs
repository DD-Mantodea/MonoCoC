using MonoCoC.Contents.Scenes;

namespace MonoCoC.UI.Components.Views
{
    public interface ISceneHostView
    {
        bool CanHost(Scene scene);
    }
}
