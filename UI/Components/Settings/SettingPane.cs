using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.UI.Components.Settings
{
    public class SettingPane : Container
    {
        public const int RowHeight = 78;

        public const int RowGap = 10;

        public const int ButtonHeight = 40;

        public const int ScrollOffset = 0;

        public const int LabelMargin = 10;

        public const int LabelReservedWidth = 304;

        // 右侧控件区（两个 key chip）：148 + 4 + 148 = 300，右缘留 4px。
        private const int ChipGap = 4;

        private const int ChipBlockWidth = KeyChip.ChipWidth * 2 + ChipGap;

        private readonly ContentContainer _content;

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

            _content = new ContentContainer()
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

        public void BindRow(string label, string description, string primary, bool primaryFilled,
            string secondary, bool secondaryFilled, Action primaryClick, Action secondaryClick)
        {
            var rowWidth = Width - 20 - ScrollBar.Thickness;

            var labelWidth = rowWidth - LabelReservedWidth;

            var box = new Container()
                .Size(rowWidth, RowHeight)
                .Position(LabelMargin, 0)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            var head = BoldTag + label + "/fd";

            if (description != "")
                head += "\n" + SmallTag + description + "/fd";

            new Label(TextFont)
                .Text(head)
                .Position(0, 0)
                .WrapWidth(labelWidth)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            // 与 Toggle 的 On/Off chip 共用同一条右缘线（最右 chip 结束于 rowWidth - 2）。
            var primaryChip = CreateKeyChip(primary, primaryFilled, primaryClick)
                .Size(KeyChip.ChipWidth, KeyChip.ChipHeight)
                .Position(rowWidth - KeyChip.ChipWidth * 2 - ChipGap - 2, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var secondaryChip = CreateKeyChip(secondary, secondaryFilled, secondaryClick)
                .Size(KeyChip.ChipWidth, KeyChip.ChipHeight)
                .Position(rowWidth - KeyChip.ChipWidth - 2, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            _rows.Add(new Row
            {
                Box = box,

                Label = label,

                Description = description,

                Primary = (KeyChip)primaryChip,

                Secondary = (KeyChip)secondaryChip,

                Height = RowHeight + RowGap,

                Text = (label + " " + description).ToLowerInvariant()
            });

            Relayout();
        }

        public void UpdateBindRow(string label, string primary, bool primaryFilled,
            string secondary, bool secondaryFilled)
        {
            foreach (var row in _rows)
            {
                if (row.Primary is null || row.Label != label)
                    continue;

                row.Primary.SetState(primary, primaryFilled);

                row.Secondary?.SetState(secondary, secondaryFilled);

                return;
            }
        }

        private static KeyChip CreateKeyChip(string text, bool filled, Action clicked)
        {
            var chip = new KeyChip();

            chip.SetState(text, filled);

            chip.Clicked = clicked;

            return chip;
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

            var headLabel = new Label(TextFont)
                .Text(head)
                .Position(0, 0)
                .WrapWidth(labelWidth)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var buttonWidth = options.Length <= 2 ? KeyChip.ChipWidth : 98;

            var segmentWidth = (buttonWidth + 4) * options.Length;

            // 右缘对齐：段控件最右 chip 结束于 rowWidth - 2，与 BindRow 的 key chip 同一条线。
            var segmentX = Math.Max(labelWidth, rowWidth - segmentWidth);

            var segment = new SegmentedControl
            {
                OptionStyle = UIComponentStyles.SettingsChip.Style
            }
                .Size(segmentWidth, ButtonHeight)
                .Position(segmentX, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var selected = 0;

            for (var index = 0; index < options.Length; index++)
                if (options[index].Current)
                    selected = index;

            segment.SetOptions(options.Select(option => option.Name), selected);

            segment.ValueChanged = index => options[index].OnClick();

            _rows.Add(new Row
            {
                Box = box,

                Head = headLabel,

                Label = label,

                Description = description,

                Height = RowHeight + RowGap,

                Text = (label + " " + current + " " + description).ToLowerInvariant()
            });

            Relayout();
        }

        public void ApplyFilter(string query)
        {
            var words = query.ToLowerInvariant().Split([' ', '\t', '\n'], StringSplitOptions.RemoveEmptyEntries);

            foreach (var row in _rows)
            {
                row.Filtered = words.Length == 0 || words.Any(word => row.Text.Contains(word));

                // 清空搜索时必须把行重新显示出来，不能只依赖 Relayout 的可见性分支。
                row.Box?.Visible = row.Filtered;
            }

            Relayout();
        }

        public override void DrawChildren(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawChildren(spriteBatch, gameTime);

            _scrollFade.Pinned = _draggingBar;

            _scrollFade.Update(gameTime, Rectangle, ScrollBar.Needed(_contentHeight, Height));

            if (!ScrollBar.Needed(_contentHeight, Height) || !_scrollFade.Visible)
                return;

            var thumb = ScrollBar.Thumb(Rectangle, _contentHeight, Height, _scroll, ScrollOffset);

            // 只画滑块本身，整体 alpha 渐隐渐显；不画轨道底色，避免出现一条"黑条"在淡入淡出。
            spriteBatch.DrawRectangle(thumb, _scrollFade.Fade(UIComponentStyles.ScrollBar.Thumb));
        }

        private readonly ScrollBarFade _scrollFade = new();

        public override InputResult OnInput(InputEventArgs args)
        {
            switch (args.Type)
            {
                case InputType.MouseWheel:
                    _scrollFade.Flash();

                    SetScroll(_scroll + (args.WheelDelta > 0 ? -40 : 40));

                    return InputResult.Consumed;

                case InputType.MouseLeftDown:
                    if (!ScrollBar.Contains(Rectangle, args.MousePosition, _contentHeight, Height, ScrollOffset))
                        break;

                    _draggingBar = true;

                    SetScroll(ScrollBar.ScrollFrom(Rectangle, _contentHeight, Height, args.MousePosition.Y, ScrollOffset));

                    return InputResult.Consumed;

                case InputType.MouseMove:
                    if (!_draggingBar)
                        break;

                    SetScroll(ScrollBar.ScrollFrom(Rectangle, _contentHeight, Height, args.MousePosition.Y, ScrollOffset));

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

            var viewTop = _scroll;

            var viewBottom = _scroll + Height;

            foreach (var row in _rows)
            {
                if (!row.Filtered)
                {
                    row.Box.Visible = false;

                    continue;
                }

                row.Box.LayoutData.Position = new Vector2(LabelMargin, y);

                // 只让视口内的行参与 Update/Draw。
                // Controls 页有 39 行（约 220 个组件），全量更新会把单核打到 80%+；
                // Component.Draw 自己会跳过 Visible，但 Component.Update 不会，所以配合
                // ContentContainer 一起裁。
                row.Box.Visible = y + row.Height > viewTop - RowHeight && y < viewBottom + RowHeight;

                y += row.Height;
            }

            _contentHeight = y;

            _content.LayoutData.Size = new Vector2(Width, _contentHeight);

            _scroll = Math.Clamp(_scroll, 0, Math.Max(0, _contentHeight - Height));

            _content.LayoutData.Position = new Vector2(0, -_scroll);
        }

        private void SetScroll(int value)
        {
            var clamped = Math.Clamp(value, 0, Math.Max(0, _contentHeight - Height));

            if (clamped == _scroll)
                return;

            _scroll = clamped;

            Relayout();
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

            public Label Head = null!;

            public string Label = "";

            public string Description = "";

            public int Height;

            public string Text = "";

            public bool Filtered = true;

            public KeyChip? Primary;

            public KeyChip? Secondary;
        }

        public void Dropdown(string label, string description, IReadOnlyList<string> options, int selected, Action<int> changed)
        {
            var rowWidth = Width - 20 - ScrollBar.Thickness;

            var labelWidth = rowWidth - LabelReservedWidth;

            var box = new Container()
                .Size(rowWidth, RowHeight)
                .Position(LabelMargin, 0)
                .LayoutType(LayoutType.Relative)
                .Of(_content);

            var head = BoldTag + label + "/fd";

            if (description != "")
                head += "\n" + SmallTag + description + "/fd";

            new Label(TextFont)
                .Text(head)
                .Position(0, 0)
                .WrapWidth(labelWidth)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            var dropdown = new Dropdown()
                .Size(260, 40)
                .Position(labelWidth + 4, 0)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            dropdown.SetOptions(options, selected);

            dropdown.ValueChanged = changed;

            _rows.Add(new Row
            {
                Box = box,
                Height = RowHeight + RowGap,
                Text = (label + " " + description).ToLowerInvariant()
            });
        }

        public void UpdateValue(string label, string current)        {
            foreach (var row in _rows)
            {
                if (row.Label != label)
                    continue;

                var head = BoldTag + label + ": " + Colored(current) + "/fd";

                if (row.Description != "")
                    head += "\n" + SmallTag + row.Description + "/fd";

                row.Head.Text(head);

                return;
            }
        }
        // Component.Update 不检查 Visible：滚出视口的行仍然会走整套布局计算。
        // 这里接管子节点更新，跳过不可见的行（Draw 本身已经会跳过）。
        private sealed class ContentContainer : Container
        {
            public override void UpdateChildren(GameTime gameTime)
            {
                foreach (var child in LayoutData.Children.ToArray())
                    if (child.Visible)
                        child.Update(gameTime);
            }
        }
    }
}
