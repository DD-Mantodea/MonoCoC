using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.UI.Components.Settings
{
    public class SettingPane : Container
    {
        public const int RowHeight = 55;

        public const int RowGap = 4;

        public const int ButtonHeight = 40;

        public const int LabelMargin = 10;

        public const int LabelReservedWidth = 304;

        private readonly Container _content;

        private readonly List<Row> _rows = [];

        private int _headerHeight;

        private int _scroll;

        private int _contentHeight;

        private bool _draggingBar;

        public SettingPane(int x, int y, int width, int height)
        {
            Scissor = true;

            CanHit = true;

            LayoutData.Size = new Vector2(width, height);

            LayoutData.Position = new Vector2(x, y);

            _content = new Container()
                .Size(width, 0)
                .Position(0, 0)
                .LayoutType(LayoutType.Relative)
                .Of(this);
        }

                public SpriteFontBase TextFont => UIFonts.Get("settingsBody");

        private static string BoldTag => "/f[" + UIFonts.Family("settingsBold") + "," + UIFonts.Size("settingsBold") + "]";

        private static string SmallTag => "/f[" + UIFonts.Family("settingsSmall") + "," + UIFonts.Size("settingsSmall") + "]";

        public void Header(string title, string description, string note)
        {
            _content.LayoutData.Children.Clear();

            _rows.Clear();

            _contentHeight = 0;

            _scroll = 0;

            _content.LayoutData.Position = new Vector2(0, 0);

            var y = 10;

            var titleFont = UIFonts.Get("settingsTitle");

            var titleLabel = new Label(titleFont)
                .Text(title)
                .Position(LabelMargin, y)
                .WrapWidth(Width - 40 - ScrollBar.Thickness)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            y += titleLabel.RichTextLayout.Measure(Width - 40).Y + 2;

            if (description != "")
            {
                var descLabel = new Label(TextFont)
                    .Text(description)
                    .Position(LabelMargin, y)
                    .WrapWidth(Width - 40 - ScrollBar.Thickness)
                    .LayoutType(LayoutType.Relative)
                    .Of(_content);

                y += descLabel.RichTextLayout.Measure(Width - 40).Y + 12;
            }

            if (note != "")
            {
                var noteLabel = new Label(TextFont)
                    .Text(note)
                    .Position(LabelMargin, y)
                    .WrapWidth(Width - 40 - ScrollBar.Thickness)
                    .LayoutType(LayoutType.Relative)
                    .Of(_content);

                y += noteLabel.RichTextLayout.Measure(Width - 40).Y + 12;
            }

            _headerHeight = y + 4;

            Relayout();
        }

        public void Message(string text)
        {
            var label = new Label(TextFont)
                .Text(text)
                .Position(LabelMargin, 0)
                .WrapWidth(Width - 40 - ScrollBar.Thickness)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            _rows.Add(new Row
            {
                Box = label,

                Height = label.RichTextLayout.Measure(Width - 40).Y + RowGap,

                Text = text
            });

            Relayout();
        }

        public void BindRow(string label, string primary, string secondary, Action primaryClick, Action secondaryClick)
        {
            var rowWidth = Width - 20 - ScrollBar.Thickness;

            // 原版 BindDisplay：左侧动作名，右侧 Primary / Secondary 两个按钮
            var labelWidth = rowWidth - 300;

            var box = new Container()
                .Size(rowWidth, RowHeight)
                .Position(LabelMargin, 0)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            new Label(TextFont)
                .Text(BoldTag + label + "/fd")
                .Position(0, 0)
                .WrapWidth(labelWidth)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var buttonWidth = 140;

            var left = new Button("button0", primary)
                .Size(buttonWidth, ButtonHeight)
                .Position(labelWidth + 4, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            left.OnClick = primaryClick;

            var right = new Button("button1", secondary)
                .Size(buttonWidth, ButtonHeight)
                .Position(labelWidth + 4 + buttonWidth + 4, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            right.OnClick = secondaryClick;

            _rows.Add(new Row
            {
                Box = box,

                Height = RowHeight + RowGap,

                Text = label.ToLowerInvariant()
            });

            Relayout();
        }

        public void Switcher(string label, string description, string value, Action previous, Action next)
        {
            var rowWidth = Width - 20 - ScrollBar.Thickness;

            var labelWidth = rowWidth - LabelReservedWidth;

            var head = BoldTag + label + "/fd";

            if (description != "")
                head += "\n" + SmallTag + description + "/fd";

            var box = new Container()
                .Size(rowWidth, RowHeight)
                .Position(LabelMargin, 0)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            new Label(TextFont)
                .Text(head)
                .Position(0, 0)
                .WrapWidth(labelWidth)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var areaStart = labelWidth + 4;

            var areaWidth = rowWidth - areaStart;

            var widgetWidth = 208;

            var startX = areaStart + Math.Max(0, (areaWidth - widgetWidth) / 2);

            var left = new Button("buttonSmall", "<", "switcher")
                .Size(40, 38)
                .Position(startX, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            left.OnClick = previous;

            var valueFont = UIFonts.Get("switcher");

            var valueWidth = (int)valueFont.MeasureString(value).X;

            new Label(valueFont)
                .Text(value)
                .Position(startX + 104 - valueWidth / 2, 9)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var right = new Button("buttonSmall", ">", "switcher")
                .Size(40, 38)
                .Position(startX + 168, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            right.OnClick = next;

            _rows.Add(new Row
            {
                Box = box,

                Height = RowHeight + RowGap,

                Text = (label + " " + description).ToLowerInvariant()
            });

            Relayout();
        }

        public void Toggle(string label, string current, string description, bool overridesLabel, params (string Name, bool Current, Action OnClick)[] options)
        {
            var rowWidth = Width - 20 - ScrollBar.Thickness;

            var labelWidth = rowWidth - LabelReservedWidth;

            var head = BoldTag + label;

            if (current != "")
                head += ": " + Colored(current);

            head += "/fd";

            if (description != "")
                head += "\n" + SmallTag + description + "/fd";

            var box = new Container()
                .Size(rowWidth, RowHeight)
                .Position(LabelMargin, 0)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            new Label(TextFont)
                .Text(head)
                .Position(0, 0)
                .WrapWidth(labelWidth)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var buttonWidth = options.Length <= 2 ? 150 : 98;

            for (var index = 0; index < options.Length; index++)
            {
                var option = options[index];

                var texture = options.Length <= 2 ? "button" + index : "buttonMedium" + index;

                var button = new Button(texture, option.Name)
                    .Size(buttonWidth, ButtonHeight)
                    .Position(labelWidth + 4 + index * (buttonWidth + 4), 0)
                    .LayoutType(LayoutType.Relative)
                    .Of(box);

                button.LabelPadding = options.Length <= 2 ? 10 : 4;

                button.OnClick = option.OnClick;

                button.Enabled = !option.Current;

                button.RefreshVisual();
            }

            _rows.Add(new Row
            {
                Box = box,

                Height = RowHeight + RowGap,

                Text = (label + " " + current + " " + description).ToLowerInvariant()
            });

            Relayout();
        }

        public void ApplyFilter(string query)
        {
            var words = query.ToLowerInvariant().Split([' ', '\t', '\n'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in _rows)
                row.Box.Visible = words.Length == 0 || words.Any(word => row.Text.Contains(word));

            Relayout();
        }

        public override void DrawChildren(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawChildren(spriteBatch, gameTime);

            if (!ScrollBar.Needed(_contentHeight, Height))
                return;

            var track = ScrollBar.Track(Rectangle);

            var thumb = ScrollBar.Thumb(Rectangle, _contentHeight, Height, _scroll);

            spriteBatch.DrawRectangle(track, UIColors.ScrollTrack);

            spriteBatch.DrawBorder(track, 1, UIColors.ScrollTrackBorder);

            spriteBatch.DrawRectangle(thumb, UIColors.ScrollThumb);

            spriteBatch.DrawBorder(thumb, 1, UIColors.ScrollThumbBorder);

            if (thumb.Height > 6)
                spriteBatch.DrawRectangle(new Rectangle(thumb.X + 2, thumb.Y + 2, Math.Max(1, thumb.Width - 4), 1), UIColors.ScrollThumbHighlight);
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            switch (args.Type)
            {
                case InputType.MouseWheel:
                    SetScroll(_scroll + (args.WheelDelta > 0 ? -40 : 40));

                    return InputResult.Consumed;

                case InputType.MouseLeftDown:
                    if (!ScrollBar.Contains(Rectangle, args.MousePosition, _contentHeight, Height))
                        break;

                    _draggingBar = true;

                    SetScroll(ScrollBar.ScrollFrom(Rectangle, _contentHeight, Height, args.MousePosition.Y));

                    return InputResult.Consumed;

                case InputType.MouseMove:
                    if (!_draggingBar)
                        break;

                    SetScroll(ScrollBar.ScrollFrom(Rectangle, _contentHeight, Height, args.MousePosition.Y));

                    return InputResult.Consumed;

                case InputType.MouseLeftUp:
                    if (!_draggingBar)
                        break;

                    _draggingBar = false;

                    return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        private void Relayout()
        {
            var y = _headerHeight;

            foreach (var row in _rows.Where(row => row.Box.Visible))
            {
                row.Box.LayoutData.Position = new Vector2(LabelMargin, y);

                y += row.Height;
            }

            _contentHeight = y;

            _content.LayoutData.Size = new Vector2(Width, _contentHeight);

            _scroll = Math.Clamp(_scroll, 0, Math.Max(0, _contentHeight - Height));

            _content.LayoutData.Position = new Vector2(0, -_scroll);
        }

        private void SetScroll(int value)
        {
            _scroll = Math.Clamp(value, 0, Math.Max(0, _contentHeight - Height));

            _content.LayoutData.Position = new Vector2(0, -_scroll);
        }

        private static string Colored(string value) => value.ToLowerInvariant() switch
        {
            "on" or "new" or "old" or "enabled" => "/c[#008000FF]" + value + "/cd",

            "off" or "disabled" => "/c[#800000FF]" + value + "/cd",

            "choose" or "enable" => "",

            _ => value
        };

        private class Row
        {
            public Component Box = null!;

            public int Height;

            public string Text = "";
        }
    }
}
