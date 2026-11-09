using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components.Views
{
    public static class CampViewContract
    {
        public static readonly UIViewContract Value = new(
            "CampView",
            [
                new("game.background", typeof(Component)),
                new("game.output", typeof(TextArea)),
                new("game.buttons", typeof(Grid)),
                new("game.playerStats", typeof(PlayerStats)),
                new("game.input", typeof(TextBox)),
                new("game.topMenu", typeof(Grid)),
                new("game.menuButton", typeof(Button)),
                new("game.navigation", typeof(Component)),
            ]);
    }
}
