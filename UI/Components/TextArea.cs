using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.UI.Components
{
    public class TextArea : Component, IUIStatefulPart
    {
        private sealed record State(string Text, int ScrollOffset, bool AutoScroll);

        public TextArea(SpriteFontBase font)
        {
            Text = new RichTextBox(font)
                .LayoutType(LayoutType.Relative)
                .CanHit(false)
                .Of(this);

            Scissor = true;
        }

        public RichTextBox Text;

        public UIVec4 Padding = 0;

        public int ScrollOffset;

        public int ScrollStep = 60;

        public bool AutoScroll = true;

        private int _contentHeight;

        private readonly List<(string Text, Action Action)> _links = [];

        public void AddLink(string text, Action action)
        {
            if (string.IsNullOrEmpty(text))
                return;

            _links.Add((text, action));
        }

        private Action? LinkAt(Vector2 mousePosition)
        {
            if (_links.Count == 0 || Width <= 0 || Height <= 0)
                return null;

            var localY = (int)(mousePosition.Y - Rectangle.Y - Padding.Top + ScrollOffset);

            if (localY < 0)
                return null;

            var line = Text.RichTextLayout.GetLineByY(localY);

            if (line == null)
                return null;

            var text = string.Concat(line.Chunks.OfType<TextChunk>().Select(chunk => chunk.Text));

            foreach (var link in _links)
                if (text.Contains(link.Text, StringComparison.Ordinal))
                    return link.Action;

            return null;
        }

        private bool _dragging;

        private bool _scrollToEnd;

        private bool _needsLayout;

        public int ContentHeight => _contentHeight;

        public int ViewWidth => Math.Max(0, Width - Padding.Left - Padding.Right - ScrollBar.Thickness);

        public int ViewHeight => Math.Max(0, Height - Padding.Top - Padding.Bottom);

        public int MaxScroll => Math.Max(0, _contentHeight - ViewHeight);

        public bool ScrollBarVisible => ScrollBar.Needed(_contentHeight, ViewHeight);

        public override void Update(GameTime gameTime)
        {
            EnsureLayout();

            ApplyScrollOffset();

            base.Update(gameTime);
        }

        public override void PrepareDraw()
        {
            EnsureLayout();

            base.PrepareDraw();
        }

        private void EnsureLayout()
        {
            if (!_needsLayout)
                return;

            Relayout();
        }

        private void Relayout()
        {
            if (Width <= 0 || Height <= 0)
                return;

            Text.RichTextLayout.Width = ViewWidth;

            _contentHeight = Text.RichTextLayout.Measure(ViewWidth).Y;

            Text.LayoutData.Size = new Vector2(ViewWidth, _contentHeight);

            if (_scrollToEnd)
            {
                _scrollToEnd = false;

                ScrollOffset = MaxScroll;
            }

            ScrollOffset = ScrollOffset.Clamp(0, MaxScroll);

            ApplyScrollOffset();

            _needsLayout = false;
        }

        private void ApplyScrollOffset()
        {
            Text.LayoutData.Position = new Vector2(Padding.Left, Padding.Top - ScrollOffset);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            if (!ScrollBarVisible)
                return;

            var track = ScrollBar.Track(Rectangle);

            var thumb = ScrollBar.Thumb(Rectangle, _contentHeight, ViewHeight, ScrollOffset);

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
                    if (!Rectangle.Contains(args.MousePosition))
                        break;

                    EnsureLayout();

                    ScrollOffset = Math.Clamp(
                        ScrollOffset - Math.Sign(args.WheelDelta) * ScrollStep,

                        0,

                        MaxScroll);

                    ApplyScrollOffset();

                    return InputResult.Consumed;

                case InputType.MouseLeftDown:
                    if (!ScrollBar.Contains(Rectangle, args.MousePosition, _contentHeight, ViewHeight))
                        break;

                    _dragging = true;

                    ScrollOffset = ScrollBar.ScrollFrom(Rectangle, _contentHeight, ViewHeight, args.MousePosition.Y);

                    ApplyScrollOffset();

                    return InputResult.Consumed;

                case InputType.MouseMove:
                    if (_dragging)
                    {
                        ScrollOffset = ScrollBar.ScrollFrom(Rectangle, _contentHeight, ViewHeight, args.MousePosition.Y);

                        ApplyScrollOffset();

                        return InputResult.Consumed;
                    }

                    MouseCursor = LinkAt(args.MousePosition) != null ? MouseCursor.Hand : MouseCursor.Arrow;

                    return base.OnInput(args);

                case InputType.MouseLeftClick:
                    if (LinkAt(args.MousePosition) is { } linkAction)
                    {
                        linkAction();

                        return InputResult.Consumed;
                    }

                    break;

                case InputType.MouseLeftUp:
                    if (!_dragging)
                        break;

                    _dragging = false;

                    return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        public TextArea Append(string text)
        {
            Text.RichTextLayout.Append(text);

            _scrollToEnd |= AutoScroll;

            _needsLayout = true;

            return this;
        }

        public object? CaptureState() =>
            new State(Text.RichTextLayout.Text ?? "", ScrollOffset, AutoScroll);

        public void RestoreState(object value)
        {
            if (value is not State state)
                return;

            Text.Text(state.Text);

            _scrollToEnd = false;

            _needsLayout = true;

            AutoScroll = state.AutoScroll;

            ScrollOffset = state.ScrollOffset;
        }

        public TextArea SetText(string text)        {
            Text.RichTextLayout.Clear();

            _links.Clear();

            _needsLayout = true;

            return Append(text);
        }

        public TextArea Clear()
        {
            Text.RichTextLayout.Clear();

            _links.Clear();

            _needsLayout = true;

            ScrollOffset = 0;

            return this;
        }

        public TextArea ScrollToEnd()
        {
            _scrollToEnd = true;

            return this;
        }
    }
}
