using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    public class DungeonView : GameView<DungeonView>, ISceneHostView
    {
        public bool CanHost(Scene scene) => scene is DungeonScene;
        private DungeonContext? _context;

        public DungeonMapPanel? DungeonMap;

        public DungeonMiniMapPanel? DungeonMiniMap;

        public DungeonView() => Initialize();

        protected override string ScreenFile => "Views\\DungeonView.hjson";

        [UIRequired("game.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("game.input")]
        private void BindInput(TextBox part) => Input = part;

        [UIRequired("game.output")]
        private void BindOutput(TextArea part) => Output = part;

        [UIRequired("game.playerStats")]
        private void BindPlayerStats(PlayerStats part) => PlayerStats = part;

        [UIRequired("game.buttons")]
        private void BindButtons(Grid part) => ButtonGrid = part;

        [UIRequired("game.miniMap")]
        private void BindMiniMap(DungeonMiniMapPanel part) => DungeonMiniMap = part;

        [UIRequired("game.dungeonMap")]
        private void BindDungeonMap(DungeonMapPanel part) => DungeonMap = part;

        protected override void OnPartsBound()
        {
            Buttons = new Button[SlotCount];

            for (var index = 0; index < ButtonGrid.LayoutData.Children.Count && index < Buttons.Length; index++)
                if (ButtonGrid.LayoutData.Children[index] is Button button)
                    Buttons[index] = button;

            Scene.ReapplyActions();
        }

        public override void ApplyContext(Context<DungeonView> context)
        {
            if (context is not DungeonContext dungeon)
                return;

            _context = dungeon;

            StartWith(dungeon);
        }

        public void StartWith(DungeonContext context)
        {
            DungeonMiniMap?.Show(context);

            DungeonMap?.Show(context);
        }

        public void RefreshMap()
        {
            if (_context is null)
                return;

            DungeonMiniMap?.Hide();

            DungeonMiniMap?.Show(_context);

            DungeonMap?.Hide();

            DungeonMap?.Show(_context);
        }
    }
}
