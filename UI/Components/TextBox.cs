using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.Text.RegularExpressions;

namespace MonoCoC.UI.Components
{
    public class TextBox : Component, IUIStatefulPart
    {
        private sealed record SnapshotState(string Text, int ScrollX, bool Active, int CursorIndex, int SelectBegin);

        public TextBox(SpriteFontBase font)
        {
            Font = font;

            Cursor = new(Color.White, Color.White)
            {
                TextBox = this
            };

            LayoutData.Padding = 5;

            MouseCursor = MouseCursor.IBeam;
        }

        public string Text = "";

        public SpriteFontBase Font;

        public float CharacterSpacing = 0;

        public TextBoxState State;

        public Cursor Cursor;

        public Color TextColor = Color.Black;

        public Color BorderColor = Color.Black;

        public bool ShowBorder = true;

        public object? CaptureState() =>
            new SnapshotState(Text, ScrollX, Active, Cursor.CursorIndex, Cursor.SelectBegin);

        public void RestoreState(object value)
        {
            if (value is not SnapshotState state)
                return;

            Text = state.Text;

            ScrollX = state.ScrollX;

            Cursor.CursorIndex = state.CursorIndex;

            Cursor.SelectBegin = state.SelectBegin;

            Active = state.Active;

            OnTextChanged?.Invoke(Text);
        }

        public int KeepPressDelay = 6;

        public int ScrollX;

        public Action<string> OnTextChanged;

        private bool _active;

        private bool _dragged;

        private Vector2 _pressStart;

        private float _backTimer;

        private float _leftTimer;

        private float _rightTimer;

        public bool Active
        {
            get => _active;

            set
            {
                _active = value;

                if (value)
                {
                    UIManager.Instance?.FocusedComponent = this;

                    return;
                }

                if (ReferenceEquals(UIManager.Instance?.FocusedComponent, this))
                    UIManager.Instance!.FocusedComponent = null;
            }
        }

        public Vector2 TextSize(string text)
        {
            if (text.Length == 0)
                return Vector2.Zero;

            return Font.MeasureString(text, characterSpacing: CharacterSpacing);
        }

        public int GetCursorIndex(Vector2 mousePos)
        {
            for (var i = 1; i <= Text.Length; i++)
                if (TextSize(Text[..i]).X + Position.X - ScrollX > mousePos.X)
                    return i - 1;

            return Text.Length;
        }

        public void CursorTo(int index)
        {
            Cursor.SelectBegin = index;

            Cursor.CursorIndex = index;
        }

        public void ClearText()
        {
            Text = "";

            CursorTo(0);
        }

        public void AppendCharacter(char c)
        {
            Cursor.DeleteSelection();

            Text = Text.Insert(Cursor.CursorIndex, c.ToString());

            Cursor.CursorIndex++;

            Cursor.SelectBegin = Cursor.CursorIndex;

            OnTextChanged?.Invoke(Text);
        }

        public void AppendString(string str)
        {
            Cursor.DeleteSelection();

            AppendAt(str, Cursor.CursorIndex);

            Cursor.CursorIndex += str.Length;

            Cursor.SelectBegin = Cursor.CursorIndex;
        }

        public void AppendAt(string str, int index)
        {
            if (str == "")
                return;

            Text = Text.Insert(index, str);

            OnTextChanged?.Invoke(Text);
        }

        public void ReplaceRange(int start, int length, string str)
        {
            start = start.Clamp(0, Text.Length);

            length = length.Clamp(0, Text.Length - start);

            Text = Text.Remove(start, length).Insert(start, str ?? "");

            CursorTo(start + (str?.Length ?? 0));

            OnTextChanged?.Invoke(Text);
        }

        public void DeleteCharacter()
        {
            if (Cursor.DeleteSelection() && Cursor.CursorIndex > 0)
            {
                Text = Text.Remove(Cursor.CursorIndex - 1, 1);

                Cursor.SelectBegin--;

                Cursor.CursorIndex--;

                OnTextChanged?.Invoke(Text);
            }
        }

        private void CharPressed(char character)
        {
            if (UserInput.Ctrl)
                return;

            // 控制字符（含 ESC）不应进入文本框
            if (char.IsControl(character))
                return;

            if (character.Equals('\r') || character.Equals('\n'))
                return;

            if (State.HasFlag(TextBoxState.OnlyNumber) && (character < 48 || character > 57))
                return;

            AppendCharacter(character);
        }

        private void KeyJustPress(Keys key)
        {
            switch (key)
            {
                case Keys.A:
                    if ((UserInput.Ctrl) && (Text.Length > 0))
                            Cursor.SelectFrom(0, Text.Length);

                    break;

                case Keys.C:
                    if (UserInput.Ctrl)
                        CopySelection();

                    break;

                case Keys.V:
                    if (!UserInput.Ctrl)
                        break;

                    if (Clipboard.GetDataFormat() != ClipboardDataFormat.Text)
                        break;

                    Cursor.DeleteSelection();

                    var clip = Clipboard.GetClipboardText() ?? "";

                    if (State.HasFlag(TextBoxState.OnlyNumber))
                        clip = new Regex("^[0-9]").Replace(clip, "");

                    Text = Text.Insert(Cursor.CursorIndex, clip);

                    Cursor.CursorIndex += clip.Length;

                    Cursor.SelectBegin = Cursor.CursorIndex;

                    OnTextChanged?.Invoke(Text);

                    break;

                case Keys.X:
                    if (!UserInput.Ctrl)
                        break;

                    CopySelection();

                    Cursor.DeleteSelection();

                    OnTextChanged?.Invoke(Text);

                    break;

                case Keys.Left:
                    if (UserInput.Shift)
                        Cursor.CursorIndex--;
                    else
                    {
                        Cursor.CursorIndex--;

                        Cursor.SelectBegin = Cursor.CursorIndex;
                    }

                    break;

                case Keys.Right:
                    if (UserInput.Shift)
                        Cursor.CursorIndex++;
                    else
                    {
                        Cursor.CursorIndex++;

                        Cursor.SelectBegin = Cursor.CursorIndex;
                    }

                    break;

                case Keys.Back:
                    DeleteCharacter();

                    break;
            }
        }

        private void KeyKeepPress(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    _backTimer++;

                    if (_backTimer < KeepPressDelay)
                        return;

                    _backTimer = 0;

                    Cursor.ShouldBlink = false;

                    Cursor._blinkTimer = 0;

                    Cursor.Visible = true;

                    DeleteCharacter();

                    break;

                case Keys.Left:
                    _leftTimer++;

                    if (_leftTimer < KeepPressDelay)
                        return;

                    _leftTimer = 0;

                    Cursor.ShouldBlink = false;

                    Cursor._blinkTimer = 0;

                    Cursor.Visible = true;

                    if (UserInput.Shift)
                        Cursor.CursorIndex--;
                    else
                    {
                        Cursor.CursorIndex--;

                        Cursor.SelectBegin = Cursor.CursorIndex;
                    }

                    break;

                case Keys.Right:
                    _rightTimer++;

                    if (_rightTimer < KeepPressDelay)
                        return;

                    _rightTimer = 0;

                    Cursor.ShouldBlink = false;

                    Cursor._blinkTimer = 0;

                    Cursor.Visible = true;

                    if (UserInput.Shift)
                        Cursor.CursorIndex++;
                    else
                    {
                        Cursor.CursorIndex++;

                        Cursor.SelectBegin = Cursor.CursorIndex;
                    }

                    break;
            }
        }

        private void CopySelection()
        {
            if (Cursor.SelectBegin < Cursor.CursorIndex)
                Clipboard.SetClipboardText(Text.Substring(Cursor.SelectBegin, Cursor.CursorIndex - Cursor.SelectBegin));

            if (Cursor.SelectBegin > Cursor.CursorIndex)
                Clipboard.SetClipboardText(Text.Substring(Cursor.CursorIndex, Cursor.SelectBegin - Cursor.CursorIndex));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Cursor.Update(gameTime);

            UpdateScrollX();
        }

        private void UpdateScrollX()
        {
            var available = Rectangle.Width - LayoutData.Padding.Left - LayoutData.Padding.Right;

            if (available <= 0)
                return;

            var caret = (int)TextSize(Text[..Cursor.CursorIndex]).X;

            if (caret - ScrollX > available)
                ScrollX = caret - available;

            if (caret - ScrollX < 0)
                ScrollX = caret;

            ScrollX = ScrollX.Clamp(0, Math.Max(0, (int)TextSize(Text).X - available));
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            switch (args.Type)
            {
                case InputType.MouseLeftDown:
                    _pressStart = args.MousePosition;

                    _dragged = false;

                    break;

                case InputType.MouseLeftClick:
                    if (_dragged)
                    {
                        _dragged = false;

                        return InputResult.Consumed;
                    }

                    if (!Active)
                    {
                        Active = true;

                        return InputResult.Consumed;
                    }

                    CursorTo(GetCursorIndex(args.MousePosition));

                    return InputResult.Consumed;

                case InputType.MouseMove:
                    if (!Active || !ReferenceEquals(UIManager.Instance?.LeftPressed, this))
                        break;

                    _dragged = true;

                    Cursor.CursorIndex = GetCursorIndex(args.MousePosition);

                    Cursor.SelectBegin = GetCursorIndex(_pressStart);

                    return InputResult.Consumed;

                case InputType.TextInput:
                    if (!Active)
                        break;

                    CharPressed(args.Character);

                    return InputResult.Consumed;

                case InputType.KeyDown:
                    if (!Active)
                        break;

                    if (args.Key == Keys.Escape)
                    {
                        Active = false;

                        return InputResult.Consumed;
                    }

                    KeyJustPress(args.Key);

                    return InputResult.Consumed;

                case InputType.KeyKeepPress:
                    if (!Active)
                        break;

                    KeyKeepPress(args.Key);

                    return InputResult.Consumed;
            }

            return InputResult.Continue;
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            if (ShowBorder)
                spriteBatch.DrawBorder(Rectangle, 1, BorderColor * Alpha);

            spriteBatch.End();

            UIManager.Instance.ScissorStack.Push(Rectangle);

            spriteBatch.Begin(rasterizerState: UIManager.ScissorRasterizer);

            try
            {
                if (Active)
                    Cursor.DrawSelection(spriteBatch);

                if (!string.IsNullOrEmpty(Text))
                {
                    var x = Rectangle.X + LayoutData.Padding.Left - ScrollX;

                    var y = Rectangle.Y + Rectangle.Height / 2 - Font.LineHeight / 2 + 2;

                    Font.DrawText(spriteBatch, Text, new Vector2(x, y), TextColor * Alpha, characterSpacing: CharacterSpacing);
                }

                if (Active)
                    Cursor.Draw(spriteBatch);
            }
            finally
            {
                spriteBatch.End();

                UIManager.Instance.ScissorStack.Pop();

                spriteBatch.Begin(rasterizerState: UIManager.ScissorRasterizer);
            }
        }
    }

    public struct Cursor(Color cursorColor, Color selectionColor, int blinkTime = 40)
    {
        public Color CursorColor = cursorColor;

        public Color SelectionColor = selectionColor;

        public int BlinkTime = blinkTime;

        internal int _blinkTimer = 0;

        public bool Visible;

        public bool ShouldBlink;

        public TextBox TextBox;

        public bool Active => TextBox.Active;

        public int CursorIndex
        {
            get => _cursorIndex;

            set => _cursorIndex = value.Clamp(0, TextBox.Text.Length);
        }

        private int _cursorIndex;

        public int SelectBegin
        {
            get => _selectBegin;

            set => _selectBegin = value.Clamp(0, TextBox.Text.Length);
        }

        private int _selectBegin;

        public void SelectFrom(int begin, int length)
        {
            SelectBegin = begin;

            CursorIndex = begin + length;
        }

        public bool DeleteSelection()
        {
            if (SelectBegin < CursorIndex)
            {
                TextBox.Text = TextBox.Text.Remove(SelectBegin, CursorIndex - SelectBegin);

                CursorIndex = SelectBegin;
            }
            else
            {
                TextBox.Text = TextBox.Text.Remove(CursorIndex, SelectBegin - CursorIndex);

                SelectBegin = CursorIndex;
            }

            return CursorIndex == SelectBegin;
        }

        public void Update(GameTime gameTime)
        {
            if (!Active)
                return;

            if (ShouldBlink)
                _blinkTimer++;

            if (_blinkTimer >= BlinkTime)
            {
                Visible = !Visible;

                _blinkTimer = 0;
            }

            ShouldBlink = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible)
                return;

            var cursorPos = TextBox.Font.MeasureString(TextBox.Text[..CursorIndex], characterSpacing: TextBox.CharacterSpacing);

            var x = (int)(cursorPos.X + TextBox.Position.X) + TextBox.LayoutData.Padding.Left - TextBox.ScrollX;

            var y = (int)((TextBox.Height - TextBox.Font.FontSize) / 2 + TextBox.Position.Y);

            spriteBatch.DrawPixel(new(x, y, 1, (int)TextBox.Font.FontSize), null, CursorColor, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void DrawSelection(SpriteBatch spriteBatch)
        {
            var cursorPos = TextBox.Font.MeasureString(TextBox.Text[..CursorIndex], characterSpacing: TextBox.CharacterSpacing); ;

            var selectBegin = TextBox.Font.MeasureString(TextBox.Text[..SelectBegin], characterSpacing: TextBox.CharacterSpacing).X;

            var x = (int)(cursorPos.X + TextBox.Position.X) + TextBox.LayoutData.Padding.Left - TextBox.ScrollX;

            var y = (int)(TextBox.Height / 2 - TextBox.Font.FontSize / 2 + TextBox.Position.Y);

            if (selectBegin < cursorPos.X)
                spriteBatch.DrawPixel(new((int)selectBegin + (int)TextBox.Position.X + TextBox.LayoutData.Padding.Left - TextBox.ScrollX, y, (int)cursorPos.X - (int)selectBegin, (int)TextBox.Font.FontSize), null, 0x99C9EF.ToColor() * 0.6f, 0, Vector2.Zero, SpriteEffects.None, 1);
            else if (selectBegin > cursorPos.X)
                spriteBatch.DrawPixel(new(x, y, (int)selectBegin - (int)cursorPos.X, (int)TextBox.Font.FontSize), null, 0x99C9EF.ToColor() * 0.6f, 0, Vector2.Zero, SpriteEffects.None, 1);
        }
    }

    [Flags]
    public enum TextBoxState
    {
        None,

        OnlyNumber
    }
}