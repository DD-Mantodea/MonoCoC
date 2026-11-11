using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.World;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Components.Views;
using Microsoft.Xna.Framework;

namespace MonoCoC.UI.Components.Combat
{
    public class PlayerStats : HjsonView
    {
        public static bool HiddenByMenu;

        private static readonly Time FallbackTime = new();

        private readonly string _screenFile;

        public PlayerStats()
            : this("Views\\PlayerStats.hjson")
        {
        }

        public PlayerStats(string screenFile)
            : base(screenFile)
        {
            _screenFile = screenFile;

            Initialize(new UIEventRegistry(), new UIWidgetRegistry());

            if (LayoutData.Size == Vector2.Zero && Content is not null)
                LayoutData.Size = new Vector2(Content.Rectangle.Width, Content.Rectangle.Height);

            DataContext = new PlayerStatsDataContext(
                new PlayerStatsModel(() => CoC.Instance?.Player, () => GameView.Instance?.Time ?? FallbackTime));

            Content?.ApplyDataContext(DataContext);
        }

        protected override string ScreenFile => _screenFile;

        public override void Update(GameTime gameTime)
        {
            if (Content is not null && !ReferenceEquals(Content.DataContext, DataContext))
                Content.ApplyDataContext(DataContext);

            Visible = !HiddenByMenu;

            base.Update(gameTime);
        }
    }
}
