using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.UI.Components;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Console
{
    public class CommandCompletionList : Container
    {
        public const int RowHeight = 32;

        public const int VisibleRows = 6;

        private readonly List<Label> _rows = [];

        private readonly SpriteFontBase _font = UIFonts.Get("console");

        private int _selected;

        private int _scroll;

        public CommandCompletionList()
        {
            CanHit = false;

            Scissor = true;

            BackgroundColor = new Color(32, 32, 32, 240);

            Visible = false;
        }

        public bool HasSuggestions => _rows.Count > 0;

        public string? SelectedSuggestion => _rows.Count == 0 ? null : _rows[_selected].RichTextLayout.Text;

        public void SetSuggestions(IReadOnlyList<string> suggestions)
        {
            LayoutData.Children.Clear();

            _rows.Clear();

            _selected = 0;

            _scroll = 0;

            var maxTextWidth = 0;

            foreach (var suggestion in suggestions)
                maxTextWidth = Math.Max(maxTextWidth, (int)_font.MeasureString(suggestion).X);

            var minWidth = ScreenWidth / 3;

            var maxWidth = ScreenWidth - 24;

            var width = Math.Clamp(maxTextWidth + 20, minWidth, maxWidth);

            var height = Math.Min(suggestions.Count, VisibleRows) * RowHeight + 8;

            LayoutData.Size = new Vector2(width, height);

            for (var index = 0; index < suggestions.Count; index++)
            {
                var row = new Label(_font)
                    .Text(suggestions[index])
                    .Position(10, 4 + index * RowHeight + (RowHeight - _font.LineHeight) / 2)
                    .CanHit(false)
                    .LayoutType(LayoutType.Relative)
                    .Of(this);

                _rows.Add(row);
            }

            Visible = _rows.Count > 0;

            RefreshSelection();
        }

        public void Clear()
        {
            LayoutData.Children.Clear();

            _rows.Clear();

            _selected = 0;

            _scroll = 0;

            LayoutData.Size = Vector2.Zero;

            Visible = false;
        }

        public void Select(int direction)
        {
            if (_rows.Count == 0)
                return;

            _selected = (_selected + direction + _rows.Count) % _rows.Count;

            EnsureVisible();

            RefreshSelection();
        }

        private void EnsureVisible()
        {
            if (_selected < _scroll)
                _scroll = _selected;

            if (_selected >= _scroll + VisibleRows)
                _scroll = _selected - VisibleRows + 1;
        }

        private void RefreshSelection()
        {
            for (var index = 0; index < _rows.Count; index++)
            {
                var row = _rows[index];

                row.LayoutData.Position = new Vector2(10, 4 + (index - _scroll) * RowHeight + (RowHeight - _font.LineHeight) / 2);

                row.TextColor = index == _selected ? Color.White : new Color(190, 190, 190);

                row.Visible = index >= _scroll && index < _scroll + VisibleRows;
            }
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            spriteBatch.DrawBorder(Rectangle, 1, new Color(120, 120, 120));

            var selected = _selected - _scroll;

            if (selected < 0 || selected >= VisibleRows)
                return;

            spriteBatch.DrawRectangle(new Rectangle(Rectangle.X + 2, Rectangle.Y + 4 + selected * RowHeight, Rectangle.Width - 4, RowHeight), new Color(70, 70, 70));
        }
    }
}
