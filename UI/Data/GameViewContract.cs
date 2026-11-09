using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Data
{
    public static class GameViewContract
    {
        public static readonly UIViewContract Value = new(
            "GameView",
            [
                new("game.background", typeof(MonoCoC.UI.Framework.Component)),
                new("game.output", typeof(TextArea)),
                new("game.buttons", typeof(Grid)),
                new("game.playerStats", typeof(PlayerStats)),
                new("game.input", typeof(TextBox)),
                new("game.combatStats", typeof(CombatStats)),
                new("game.multiCombatStats", typeof(MultiCombatStats)),
                new("game.sprite", typeof(UIImage)),
                new("game.image", typeof(UIImage)),
                new("game.dungeonMap", typeof(DungeonMapPanel)),
                new("game.miniMap", typeof(DungeonMiniMapPanel)),
                new("game.targetPointer", typeof(UIImage)),
                new("game.gameOver", typeof(Label)),
                new("game.menuButton", typeof(Button)),
                new("game.navigation", typeof(Component)),
            ]);
    }
}
