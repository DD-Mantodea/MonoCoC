using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;

namespace MonoCoC.UI.Components
{
    public class StatRow : Container
    {
        public StatRow(SpriteFontBase font)
        {
            Label = new Label(font)
                .Text("")
                .LayoutType(LayoutType.Relative)
                .Of(this);

            Value = new Label(font)
                .Text("")
                .LayoutType(LayoutType.Relative)
                .Of(this);

            Label.VerticalAlignment = TextVerticalAlignment.Center;

            Value.VerticalAlignment = TextVerticalAlignment.Center;

            Value.HorizontalAlignment = TextHorizontalAlignment.Right;
        }

        public Label Label;

        public Label Value;

        public UIBinding? ValueBinding;

        public override void Update(GameTime gameTime)
        {
            if (ValueBinding?.Resolve(DataContext) is { } value)
                Value.Text(value.ToString() ?? "");

            Label.LayoutData.Size = new Vector2(Width, Height);

            Value.LayoutData.Size = new Vector2(Width, Height);

            base.Update(gameTime);
        }
    }
}

