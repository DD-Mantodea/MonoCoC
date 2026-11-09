using MonoCoC.UI.Data;

namespace MonoCoC.UI.Components.Views
{
    public sealed class CampView : GameView
    {
        protected override string ScreenFile => "Screens/CampView.hjson";

        protected override UIViewContract Contract => CampViewContract.Value;
    }
}
