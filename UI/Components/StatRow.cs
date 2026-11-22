using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using System;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;

using Hjson;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class StatRow : Container, IFromHjson<StatRow> {
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

        public UIStateStyle? Style;

        public void ApplyStyle(UIStateStyle style)
        {
            Style = style;

            LayoutData.Padding = style.Padding;

            var font = UIFonts.GetAt(UIFonts.Family(style.FontRole), style.FontSize);

            var labelFont = UIFonts.GetAt(
                UIFonts.Family(style.LabelFontRole),
                style.LabelFontSize > 0 ? style.LabelFontSize : style.FontSize);

            Value.RichTextLayout.Font = font;

            Label.RichTextLayout.Font = labelFont;

            Label.VerticalAlignment = style.TextVerticalAlign;

            Value.VerticalAlignment = style.TextVerticalAlign;

            Label.Text(Label.RichTextLayout.Text ?? "");

            Value.Text(Value.RichTextLayout.Text ?? "");
        }

        public override void Update(GameTime gameTime)
        {
            if (ValueBinding?.Resolve(DataContext) is { } value)
                Value.Text(value.ToString() ?? "");

            var padding = LayoutData.Padding;

            var width = MathF.Max(0, Width - padding.Left - padding.Right);

            var height = MathF.Max(0, Height - padding.Top - padding.Bottom);

            Label.LayoutData.Position = new Vector2(0, Style?.TextOffsetY ?? 0);

            Value.LayoutData.Position = new Vector2(0, Style?.TextOffsetY ?? 0);

            Label.LayoutData.Size = new Vector2(width, height);

            Value.LayoutData.Size = new Vector2(width, height);

            if (Style is not null)
            {
                var hovered = IsHovering;

                BackgroundColor = Style.BackgroundFor(Enabled, hovered, IsPressed, Selected);

                Label.TextColor = Style.LabelFor(Enabled, hovered, IsPressed, Selected)
                    ?? Style.TextFor(Enabled, hovered, IsPressed, Selected)
                    ?? Label.TextColor;

                Value.TextColor = Style.ValueFor(Enabled, hovered, IsPressed, Selected)
                    ?? Style.TextFor(Enabled, hovered, IsPressed, Selected)
                    ?? Value.TextColor;

                MouseCursor = hovered && OnClick is not null
                    ? Microsoft.Xna.Framework.Input.MouseCursor.Hand
                    : null;
            }

            base.Update(gameTime);
        }
    
        public static string TypeId => "coc:stat_row";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var row = new StatRow(UIFonts.Resolve(node, "coc:menu"));

            row.Label.Text(UILocalizedText.Resolve(node, "label"));

            row.ValueBinding = UIParseUtils.ReadBinding(node, "value");

            if (node.TryGetValue("labelColor", out _))
                row.Label.TextColor = UIParseUtils.ColorOf(node.Str("labelColor"));

            if (node.TryGetValue("valueColor", out _))
                row.Value.TextColor = UIParseUtils.ColorOf(node.Str("valueColor"));

            return row;
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<StatRow>();
    
}
}

