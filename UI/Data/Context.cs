using MonoCoC.UI.Components.Views;

namespace MonoCoC.UI.Data
{
    public abstract class Context<T> where T : GameView<T>
    {
        public GameView? ReturnView;
    }
}
