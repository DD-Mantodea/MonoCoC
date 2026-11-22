using Microsoft.Xna.Framework;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System.Collections.Generic;

using Hjson;

namespace MonoCoC.UI.Components
{
    public class ScrollPanel : Container, IFromHjson<ScrollPanel>
    {
        private readonly Dictionary<Component, Vector2> _basePositions = [];

        private bool _captured;

        public int ScrollOffset;

        public int ScrollStep = 48;

        public int ContentHeight;

        public ScrollPanel()
        {
            CanHit = true;

            Scissor = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!_captured)
            {
                _captured = true;

                foreach (var child in LayoutData.Children)
                    _basePositions[child] = child.LayoutData.Position;
            }

            var height = ContentHeight;

            foreach (var child in LayoutData.Children)
                height = System.Math.Max(height, (int)child.LayoutData.Position.Y + (int)child.LayoutData.Size.Y);

            var maxScroll = System.Math.Max(0, height - Height);

            var wheel = UserInput.GetDeltaWheelValue();

            if (wheel != 0 && Rectangle.Contains(UserInput.GetMousePos()))
                ScrollOffset = System.Math.Clamp(ScrollOffset - System.Math.Sign(wheel) * ScrollStep, 0, maxScroll);

            ScrollOffset = System.Math.Clamp(ScrollOffset, 0, maxScroll);

            foreach (var child in LayoutData.Children)
                if (_basePositions.TryGetValue(child, out var basePosition))
                    child.LayoutData.Position = new Vector2(basePosition.X, basePosition.Y - ScrollOffset);

            base.Update(gameTime);
        }

        public static string TypeId => "coc:scroll_panel";

        public static Component FromHjson(JsonObject node, UIBuildContext services) =>
            new ScrollPanel
            {
                ScrollStep = node.Int("scrollStep", 48),
                ContentHeight = node.Int("contentHeight", 0)
            };

        [System.Runtime.CompilerServices.ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<ScrollPanel>();
    }
}
