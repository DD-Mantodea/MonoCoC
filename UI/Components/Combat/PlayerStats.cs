using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.World;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Components.Views;
using Microsoft.Xna.Framework;

using Hjson;
using MonoCoC.Extensions;
using System.IO;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components.Combat
{
    public class PlayerStats : HjsonView, IFromHjson<PlayerStats> {
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

            Initialize();

            if (LayoutData.Size == Vector2.Zero && Content is not null)
                LayoutData.Size = new Vector2(Content.Rectangle.Width, Content.Rectangle.Height);

            SetDataContext(new PlayerStatsDataContext(
                new PlayerStatsModel(() => CoC.Instance?.Player, () => GameView.Current?.Time ?? FallbackTime)));

            Content?.ApplyDataContext(DataContext);
        }

        protected override string ScreenFile => _screenFile;

        public override void ApplyDataContext(IUIDataContext? context)
        {
            if (context is null && DataContext is not null)
                return;

            base.ApplyDataContext(context);
        }

        public override void Update(GameTime gameTime)
        {
            if (Content is not null && !ReferenceEquals(Content.DataContext, DataContext))
                Content.ApplyDataContext(DataContext);

            base.Update(gameTime);
        }
    
        public static string TypeId => "coc:player_stats";
        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var view = node.Str("view");

            if (view.Length == 0)
                throw new InvalidDataException("coc:player_stats requires 'view'");

            return new PlayerStats(view);
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<PlayerStats>();
    
}
}
