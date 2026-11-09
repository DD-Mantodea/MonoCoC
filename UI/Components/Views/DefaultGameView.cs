using MonoCoC.UI.Data;

namespace MonoCoC.UI.Components.Views
{
    public sealed class DefaultGameView : GameView
    {
        protected override string ScreenFile => "Screens/GameView.hjson";

        protected override UIViewContract Contract => GameViewContract.Value;
    }
}
