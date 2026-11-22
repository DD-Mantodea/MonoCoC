using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Contents.Input;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.Text.RegularExpressions;

using FontStashSharp.RichText;
using Hjson;
using MonoCoC.UI.Animations;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Structures;
using MonoCoC.UI.Styles;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class TextBox : Component, IUIStatefulPart, IFromHjson<TextBox> {
        private record SnapshotState(string Text, int ScrollX, int CursorIndex, int SelectBegin);

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

        public bool EnableChineseInput = true;

        public Func<InputEventArgs, bool>? KeyInterceptor;

        public string Text = "";

        public SpriteFontBase Font;

        public float CharacterSpacing = 0;

        public TextBoxState State;

        public Cursor Cursor;

        public Color TextColor = Color.Black;

        public Color BorderColor = Color.Black;

        public bool ShowBorder = true;

        public Color HintColor = Color.Transparent;

        public object? CaptureState() =>
            new SnapshotState(Text, ScrollX, Cursor.CursorIndex, Cursor.SelectBegin);

        public void RestoreState(object value)
        {
            if (value is not SnapshotState state)
                return;

            Text = state.Text;

            ScrollX = state.ScrollX;

            Cursor.CursorIndex = state.CursorIndex;

            Cursor.SelectBegin = state.SelectBegin;

            OnTextChanged?.Invoke(Text);
        }

        public int KeepPressDelay = 3;

        public int ScrollX;

        public Action<string> OnTextChanged;

        private bool _active;

        private bool _dragged;

        private bool _imeClickCommitted;

        private Vector2 _pressStart;

        private float _backTimer;

        private Keys _imeRepeatKey;

        private int _imeRepeatFrames;

        private string _imeCaretPreedit = "";

        private string _imeText = "";

        private int _imeCaretIndex;
        private float _leftTimer;

        private float _rightTimer;

        public bool Active
        {
            get => _active;

            set
            {
                var wasActive = _active;

                _active = value;

                if (value)
                {
                    RimeIme.ResetComposition();

                    UIManager.Instance?.FocusedComponent = this;

                    return;
                }

                if (ReferenceEquals(UIManager.Instance?.FocusedComponent, this))
                    UIManager.Instance!.FocusedComponent = null;

                if (wasActive && EnableChineseInput && RimeIme.Available && RimeIme.HasComposition)
                    CommitImeRaw();

                RimeIme.ResetComposition();

                UIManager.Instance?.HideIme();
            }
        }

        public string ImePreedit =>
            Active && EnableChineseInput && RimeIme.Available
                ? _imeText.Length > 0 ? _imeText : RimeIme.Preedit
                : "";

        private void SyncImeText()
        {
            if (!EnableChineseInput || !RimeIme.Available || !RimeIme.HasComposition)
            {
                _imeText = "";

                return;
            }

            if (_imeText.Length > 0)
                return;

            _imeText = RimeIme.Preedit;

            _imeCaretPreedit = _imeText;

            _imeCaretIndex = _imeText.Length;
        }

        public float ImePreeditWidth =>
            ImePreedit.Length == 0 ? 0 : Font.MeasureString(ImePreedit, characterSpacing: CharacterSpacing).X;

        public float ImeCaretOffset
        {
            get
            {
                var preedit = ImePreedit;

                if (preedit.Length == 0)
                    return 0;

                if (_imeCaretPreedit != preedit)
                {
                    _imeCaretPreedit = preedit;

                    _imeCaretIndex = Math.Min(_imeCaretIndex, preedit.Length);
                }

                var position = Math.Clamp(_imeCaretIndex, 0, preedit.Length);

                return Font.MeasureString(preedit[..position], characterSpacing: CharacterSpacing).X;
            }
        }

        private static string NormalizeImeText(string text)
        {
            while (text.Contains("''"))
                text = text.Replace("''", "'");

            if (text.StartsWith('\''))
                text = text[1..];

            if (text.EndsWith('\''))
                text = text[..^1];

            return text;
        }

        private static int SnapImeCaretBack(string text, int index)
        {
            index = Math.Clamp(index, 0, text.Length);

            if (text.Length == 0)
                return 0;

            var separator = text.LastIndexOf('\'', Math.Min(index, text.Length - 1));

            return separator >= 0 ? separator : index;
        }

        private static int SnapImeCaret(string text, int index)
        {
            index = Math.Clamp(index, 0, text.Length);

            while (index < text.Length && text[index] != '\'')
                index++;

            return index;
        }

        private void InsertImeText(string text)
        {
            SyncImeText();

            var position = Math.Clamp(_imeCaretIndex, 0, _imeText.Length);

            _imeText = NormalizeImeText(_imeText.Insert(position, text));

            RimeIme.RebuildComposition(_imeText);

            _imeText = RimeIme.Preedit;

            _imeCaretPreedit = _imeText;

            _imeCaretIndex = SnapImeCaret(_imeText, position + text.Length);
        }

        private bool DeleteImeAtCaret()
        {
            SyncImeText();

            if (_imeText.Length == 0)
                return false;

            if (_imeCaretIndex <= 0)
                return true;

            var position = Math.Clamp(_imeCaretIndex, 1, _imeText.Length);

            var removeIndex = position - 1;

            _imeText = NormalizeImeText(_imeText.Remove(removeIndex, 1));

            RimeIme.RebuildComposition(_imeText);

            _imeText = RimeIme.Preedit;

            _imeCaretPreedit = _imeText;

            _imeCaretIndex = Math.Clamp(removeIndex, 0, _imeText.Length);

            return true;
        }

        private void MoveImeCaret(int direction)
        {
            var preedit = ImePreedit;

            if (preedit.Length == 0)
                return;

            if (_imeCaretPreedit != preedit)
            {
                _imeCaretPreedit = preedit;

                _imeCaretIndex = preedit.Length;
            }

            var position = Math.Clamp(_imeCaretIndex, 0, preedit.Length);

            if (direction < 0)
            {
                position--;

                while (position > 0 && preedit[position] == '\'')
                    position--;
            }
            else
            {
                position++;

                while (position < preedit.Length && preedit[position] == '\'')
                    position++;
            }

            _imeCaretIndex = Math.Clamp(position, 0, preedit.Length);
        }

        public Rectangle ImeAnchor
        {
            get
            {
                var index = Math.Clamp(Cursor.CursorIndex, 0, Text.Length);

                var x = Rectangle.X + LayoutData.Padding.Left - ScrollX + (int)TextSize(Text[..index]).X;

                return new Rectangle(x, Rectangle.Y, Math.Max(1, Rectangle.Width), Rectangle.Height);
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
            // DeleteSelection 在没有选区时也会返回 true（remove 0 个字符），
            // 所以必须先显式判断选区是否存在，否则正常退格会被吞掉。
            if (Cursor.SelectBegin != Cursor.CursorIndex)
            {
                Cursor.DeleteSelection();

                OnTextChanged?.Invoke(Text);

                return;
            }

            if (Cursor.CursorIndex <= 0)
                return;

            Text = Text.Remove(Cursor.CursorIndex - 1, 1);

            Cursor.SelectBegin--;

            Cursor.CursorIndex--;

            OnTextChanged?.Invoke(Text);
        }
        public void DeleteForward()
        {
            if (Cursor.SelectBegin != Cursor.CursorIndex)
            {
                Cursor.DeleteSelection();

                OnTextChanged?.Invoke(Text);

                return;
            }

            if (Cursor.CursorIndex >= Text.Length)
                return;

            Text = Text.Remove(Cursor.CursorIndex, 1);

            Cursor.SelectBegin = Cursor.CursorIndex;

            OnTextChanged?.Invoke(Text);
        }
        private void CharPressed(char character)
        {
            if (UserInput.Ctrl)
                return;

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

                case Keys.Delete:
                    DeleteForward();

                    break;
            }
        }

        private bool HandleImeRepeat(Keys key)
        {
            if (!EnableChineseInput || !RimeIme.Available || !RimeIme.HasComposition)
                return false;

            if (key is not ((>= Keys.A and <= Keys.Z) or Keys.Back or Keys.Left or Keys.Right or Keys.Up or Keys.Down or Keys.PageUp or Keys.PageDown))
                return false;

            if (key != _imeRepeatKey)
            {
                _imeRepeatKey = key;

                _imeRepeatFrames = 0;
            }

            _imeRepeatFrames++;

            var initial = key switch
            {
                Keys.Back => 10,
                Keys.Up or Keys.Down or Keys.PageUp or Keys.PageDown => 12,
                _ => 12
            };

            var interval = key switch
            {
                Keys.Back => 3,
                Keys.Up or Keys.Down or Keys.PageUp or Keys.PageDown => 5,
                _ => 2
            };

            if (_imeRepeatFrames < initial)
                return true;

            if ((_imeRepeatFrames - initial) % interval != 0)
                return true;

            if (key is Keys.Left or Keys.Right)
            {
                MoveImeCaret(key == Keys.Left ? -1 : 1);

                return true;
            }

            if (key is >= Keys.A and <= Keys.Z)
            {
                SyncImeText();

                if (_imeCaretIndex >= _imeText.Length)
                {
                    RimeIme.ProcessKey(key, out var typed);

                    if (typed.Length > 0)
                        AppendString(typed);

                    _imeText = RimeIme.Preedit;

                    _imeCaretPreedit = _imeText;

                    _imeCaretIndex = _imeText.Length;
                }
                else
                    InsertImeText(((char)('a' + (key - Keys.A))).ToString());

                return true;
            }

            if (key == Keys.Back && DeleteImeAtCaret())
                return true;

            RimeIme.ProcessKey(key, out var commit);

            if (commit.Length > 0)
                AppendString(commit);

            return true;
        }

        private void KeyKeepPress(Keys key)
        {
            if (HandleImeRepeat(key))
                return;

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

            CommitImeWhenWindowLosesFocus();

            RefreshImeOverlay();
        }

        public void CommitImeLetters()
        {
            var raw = ImePreedit.Replace("'", "");

            if (raw.Length > 0)
                AppendString(raw);

            RimeIme.ResetComposition();

            _imeText = "";

            _imeCaretPreedit = "";

            _imeCaretIndex = 0;
        }

        private void CommitImeRaw()
        {
            var raw = RimeIme.Preedit;

            if (raw.Length > 0)
                AppendString(raw);

            RimeIme.ResetComposition();

            _imeText = "";

            _imeCaretPreedit = "";

            _imeCaretIndex = 0;
        }

        private void CommitImeWhenWindowLosesFocus()
        {
            if (!Active || !EnableChineseInput || !RimeIme.Available)
                return;

            if (CoC.Instance.IsActive)
                return;

            var preedit = RimeIme.Preedit;

            if (preedit.Length == 0)
                return;

            CommitImeRaw();

            UIManager.Instance?.HideIme();
        }

        private void RefreshImeOverlay()
        {
            if (!EnableChineseInput || !Active || !RimeIme.Available)
                return;

            var preedit = RimeIme.Preedit;

            var candidates = RimeIme.Candidates;

            if (preedit.Length == 0 && candidates.Count == 0)
            {
                UIManager.Instance?.HideIme();

                return;
            }

            UIManager.Instance?.ShowIme(ImeAnchor, candidates, RimeIme.HighlightedIndex);
        }

        private void UpdateScrollX()
        {
            var available = Rectangle.Width - LayoutData.Padding.Left - LayoutData.Padding.Right;

            if (available <= 0)
                return;

            var caret = (int)(TextSize(Text[..Cursor.CursorIndex]).X + ImePreeditWidth);

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

                    if (EnableChineseInput && RimeIme.Available && RimeIme.HasComposition)
                    {
                        CommitImeRaw();

                        _imeClickCommitted = true;

                        return InputResult.Consumed;
                    }

                    break;

                case InputType.MouseLeftClick:
                    if (_imeClickCommitted)
                    {
                        _imeClickCommitted = false;

                        return InputResult.Consumed;
                    }

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

                    if (_imeClickCommitted)
                        return InputResult.Consumed;

                    _dragged = true;

                    Cursor.CursorIndex = GetCursorIndex(args.MousePosition);

                    Cursor.SelectBegin = GetCursorIndex(_pressStart);

                    return InputResult.Consumed;

                case InputType.TextInput:
                    if (!Active)
                        break;

                    if (EnableChineseInput && RimeIme.Available && !RimeIme.IsAsciiMode &&
                        !(args.Character == ' ' && !RimeIme.HasComposition))
                        return InputResult.Consumed;

                    CharPressed(args.Character);

                    return InputResult.Consumed;

                case InputType.KeyDown:
                    if (!Active)
                        break;

                    SyncImeText();

                    if (KeyInterceptor?.Invoke(args) == true)
                        return InputResult.Consumed;

                    _imeRepeatKey = Keys.None;

                    _imeRepeatFrames = 0;

                    if (EnableChineseInput && RimeIme.Available && UserInput.Ctrl && args.Key == Keys.Space)
                    {
                        if (RimeIme.HasComposition)
                        {
                            CommitImeLetters();

                            RimeIme.ToggleLanguage();

                            return InputResult.Consumed;
                        }

                        RimeIme.ToggleLanguage();

                        return InputResult.Consumed;
                    }

                    if (EnableChineseInput && RimeIme.Available && RimeIme.HasComposition && args.Key == Keys.Enter)
                    {
                        CommitImeLetters();

                        return InputResult.Consumed;
                    }

                    if (EnableChineseInput && RimeIme.Available && args.Key == Keys.Escape && RimeIme.HasComposition)
                    {
                        RimeIme.ResetComposition();

                        _imeText = "";

                        _imeCaretPreedit = "";

                        _imeCaretIndex = 0;

                        return InputResult.Consumed;
                    }

                    if (UserInput.Ctrl || UserInput.Alt)
                    {
                        KeyJustPress(args.Key);

                        return InputResult.Consumed;
                    }

                    if (EnableChineseInput && RimeIme.Available && RimeIme.HasComposition && args.Key is Keys.Left or Keys.Right)
                    {
                        MoveImeCaret(args.Key == Keys.Left ? -1 : 1);

                        return InputResult.Consumed;
                    }

                    if (EnableChineseInput && RimeIme.Available && RimeIme.HasComposition && args.Key == Keys.Back && DeleteImeAtCaret())
                        return InputResult.Consumed;

                    if (EnableChineseInput && RimeIme.Available && RimeIme.HasComposition &&
                        args.Key is >= Keys.A and <= Keys.Z)
                    {
                        if (_imeCaretIndex >= _imeText.Length)
                        {
                            RimeIme.ProcessKey(args.Key, out var typed);

                            if (typed.Length > 0)
                                AppendString(typed);

                            _imeText = RimeIme.Preedit;

                            _imeCaretPreedit = _imeText;

                            _imeCaretIndex = _imeText.Length;
                        }
                        else
                            InsertImeText(((char)('a' + (args.Key - Keys.A))).ToString());

                        return InputResult.Consumed;
                    }

                    if (EnableChineseInput && RimeIme.Available && RimeIme.HasComposition && args.Key == Keys.OemQuotes)
                    {
                        if (_imeCaretIndex >= _imeText.Length)
                        {
                            RimeIme.ProcessKey(args.Key, out _);

                            _imeText = RimeIme.Preedit;

                            _imeCaretPreedit = _imeText;

                            _imeCaretIndex = _imeText.Length;
                        }
                        else
                            InsertImeText("'");

                        return InputResult.Consumed;
                    }

                    if (EnableChineseInput && RimeIme.Available && Cursor.SelectBegin != Cursor.CursorIndex)
                    {
                        Cursor.DeleteSelection();

                        OnTextChanged?.Invoke(Text);
                    }

                    if (EnableChineseInput && RimeIme.Available && RimeIme.ProcessKey(args.Key, out var commit))
                    {
                        if (commit.Length > 0)
                            AppendString(commit);

                        if (!RimeIme.HasComposition)
                        {
                            _imeText = "";

                            _imeCaretPreedit = "";

                            _imeCaretIndex = 0;
                        }
                        else if (_imeText.Length == 0)
                        {
                            _imeText = RimeIme.Preedit;

                            _imeCaretPreedit = _imeText;

                            _imeCaretIndex = _imeText.Length;
                        }

                        return InputResult.Consumed;
                    }

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

                    if (KeyInterceptor?.Invoke(args) == true)
                        return InputResult.Consumed;

                    KeyKeepPress(args.Key);

                    return InputResult.Consumed;
            }

            return InputResult.Continue;
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            spriteBatch.End();

            UIManager.Instance.ScissorStack.Push(Rectangle);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, UIManager.ScissorRasterizer);

            try
            {
                if (Active)
                    Cursor.DrawSelection(spriteBatch);

                if (!string.IsNullOrEmpty(Text) || ImePreedit.Length > 0)
                {
                    var x = Rectangle.X + LayoutData.Padding.Left - ScrollX;

                    var y = Rectangle.Y + Rectangle.Height / 2 - Font.LineHeight / 2 + 2;

                    var preedit = ImePreedit;

                    if (preedit.Length > 0)
                    {
                        var index = Math.Clamp(Cursor.CursorIndex, 0, Text.Length);

                        var before = Text[..index];

                        var after = Text[index..];

                        var beforeWidth = before.Length > 0 ? Font.MeasureString(before, characterSpacing: CharacterSpacing).X : 0;

                        var preeditWidth = Font.MeasureString(preedit, characterSpacing: CharacterSpacing).X;

                        if (before.Length > 0)
                            Font.DrawText(spriteBatch, before, new Vector2(x, y), TextColor * Alpha, characterSpacing: CharacterSpacing);

                        Font.DrawText(spriteBatch, preedit, new Vector2(x + beforeWidth, y), TextColor * Alpha, characterSpacing: CharacterSpacing);

                        if (after.Length > 0)
                            Font.DrawText(spriteBatch, after, new Vector2(x + beforeWidth + preeditWidth, y), TextColor * Alpha, characterSpacing: CharacterSpacing);

                        for (var dot = 0f; dot < preeditWidth; dot += 2f)
                            spriteBatch.DrawPixel(new Rectangle((int)(x + beforeWidth + dot), (int)(y + Font.LineHeight - 3), 1, 1), null, TextColor * Alpha, 0, Vector2.Zero, SpriteEffects.None, 1);
                    }
                    else
                    {
                        Font.DrawText(spriteBatch, Text, new Vector2(x, y), TextColor * Alpha, characterSpacing: CharacterSpacing);
                    }
                }

                if (Active)
                    Cursor.Draw(spriteBatch);
            }
            finally
            {
                spriteBatch.End();

                UIManager.Instance.ScissorStack.Pop();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, null, null, UIManager.ScissorRasterizer);
            }
        }

        public static string TypeId => "coc:text_box";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var box = new TextBox(UIFonts.Resolve(node, "coc:input"));

            if (node.TryGetValue("textColor", out _))
                box.TextColor = UIParseUtils.ColorOf(node.Str("textColor"));

            if (node.TryGetValue("borderColor", out _))
                box.BorderColor = UIParseUtils.ColorOf(node.Str("borderColor"));

            if (node.TryGetValue("showBorder", out _))
                box.ShowBorder = node.Bool("showBorder", box.ShowBorder);

            if (node.TryGetValue("chineseInput", out _))
                box.EnableChineseInput = node.Bool("chineseInput", true);

            if (node.TryGetValue("states", out var boxStateValue) && boxStateValue is JsonObject boxStates)
            {
                var boxBaseThickness = node.TryGetValue("borderThickness", out var boxBorderValue) && boxBorderValue is not JsonArray
                    ? UIParseUtils.Int(boxBorderValue)
                    : 1;

                var style = BuildStateStyle(boxStates, boxBaseThickness);

                if (node.TryGetValue("borderThickness", out var boxThickness))
                {
                    var sides = ReadSides(boxThickness);
                    if (sides is not null)
                    {
                        style.NormalBorderSides = sides;
                        style.HoverBorderSides = sides;
                        style.PressedBorderSides = sides;
                        style.SelectedBorderSides = sides;
                    }
                }

                box.StateStyle = style;
            }
            else if (node.TryGetValue("backgroundColor", out _) ||
                     node.TryGetValue("borderColor", out _) ||
                     node.TryGetValue("borderThickness", out _))
            {
                var style = new UIStateStyle
                {
                    NormalBackground = UIParseUtils.ColorOf(node.Str("backgroundColor", "#00000000")),
                    NormalBorder = box.BorderColor
                };

                if (node.TryGetValue("borderThickness", out var boxThickness))
                {
                    var sides = ReadSides(boxThickness);

                    if (sides is not null)
                        style.NormalBorderSides = sides;
                    else
                        style.NormalBorderThickness = UIParseUtils.Int(boxThickness);
                }

                box.StateStyle = style;
            }

            if (box.StateStyle is not null && node.TryGetValue("backgroundColor", out _))
            {
                var background = UIParseUtils.ColorOf(node.Str("backgroundColor"));

                box.StateStyle.NormalBackground = background;

                if (box.StateStyle.HoverBackground == Color.Transparent)
                    box.StateStyle.HoverBackground = background;

                if (box.StateStyle.PressedBackground == Color.Transparent)
                    box.StateStyle.PressedBackground = background;

                if (box.StateStyle.SelectedBackground == Color.Transparent)
                    box.StateStyle.SelectedBackground = background;
            }

            if (!box.ShowBorder && box.StateStyle is not null)
                box.StateStyle.NormalBorder = Color.Transparent;

            if (node.TryGetValue("characterSpacing", out var characterSpacing))
                box.CharacterSpacing = UIParseUtils.Float(characterSpacing);

            if (node.TryGetValue("keepPressDelay", out var keepPressDelay))
                box.KeepPressDelay = UIParseUtils.Int(keepPressDelay);

            var cursor = box.Cursor;

            if (node.TryGetValue("cursorColor", out _))
                cursor.CursorColor = UIParseUtils.ColorOf(node.Str("cursorColor"));


            if (node.TryGetValue("selectionColor", out _))
                cursor.SelectionColor = UIParseUtils.ColorOf(node.Str("selectionColor"));

            if (node.TryGetValue("cursorBlinkTime", out var blinkTime))
                cursor.BlinkTime = UIParseUtils.Int(blinkTime);

            box.Cursor = cursor;


            return box;
        }



        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<TextBox>();
    
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

            cursorPos.X += TextBox.ImeCaretOffset;

            var x = (int)(cursorPos.X + TextBox.Position.X) + TextBox.LayoutData.Padding.Left - TextBox.ScrollX;

            var y = (int)((TextBox.Height - TextBox.Font.FontSize) / 2 + TextBox.Position.Y);

            spriteBatch.DrawPixel(new(x, y, 1, (int)TextBox.Font.FontSize), null, CursorColor, 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public void DrawSelection(SpriteBatch spriteBatch)
        {
            if (TextBox.ImePreeditWidth > 0)
                return;

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

