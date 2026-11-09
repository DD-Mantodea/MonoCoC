using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Contents.Commands;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Console
{
    public class ConsoleOverlay : Container
    {
        private const int PanelPadding = 12;

        private const int InputHeight = 36;

        private static readonly List<string> Lines = [];

        private static readonly List<string> InputHistory = [];

        private static ConsoleOverlay? _instance;

        private readonly TextArea _output;

        private readonly TextBox _input;

        private readonly ConsolePanel _panel;

        private readonly CommandCompletionList _completionList;

        private readonly List<string> _suggestions = [];

        private bool _applyingSuggestion;

        private const float CompletionRepeatDelay = 0.30f;

        private const float CompletionRepeatInterval = 0.05f;

        private Keys? _repeatKey;

        private float _repeatDelay;

        private int _inputHistoryIndex;

        private ConsoleOverlay()
        {
            LayoutData.Size = new Vector2(ScreenWidth, ScreenHeight);

            CanHit = true;

            var panelHeight = ScreenHeight / 2;

            _panel = new ConsolePanel()
                .Size(ScreenWidth, panelHeight)
                .Position(0, ScreenHeight - panelHeight)
                .Of(this);

            _panel.BackgroundColor = new Color(28, 28, 28, 220);

            var outputHeight = panelHeight - InputHeight - PanelPadding * 3 - 26;

            _output = new TextArea(UIFonts.Get("console"))
                .Size(ScreenWidth - PanelPadding * 2, outputHeight)
                .Position(PanelPadding, PanelPadding)
                .LayoutType(LayoutType.Relative)
                .Of(_panel);

            _output.Text.TextColor = Color.White;

            _output.ScrollStep = 48;

            var prompt = new Label(UIFonts.Get("console"))
                .Text(">")
                .Position(PanelPadding, panelHeight - InputHeight - PanelPadding + 6)
                .CanHit(false)
                .LayoutType(LayoutType.Relative)
                .Of(_panel);

            prompt.TextColor = Color.White;

            _completionList = new CommandCompletionList()
                .Position(PanelPadding, 0)
                .LayoutType(LayoutType.Relative)
                .Of(_panel);

            _input = new TextBox(UIFonts.Get("console"))
                .Size(ScreenWidth - PanelPadding * 2 - 18, InputHeight)
                .Position(PanelPadding + 18, panelHeight - InputHeight - PanelPadding)
                .LayoutType(LayoutType.Relative)
                .Of(_panel);

            _input.TextColor = Color.White;

            _input.ShowBorder = false;

            _input.Cursor = new Cursor(Color.White, UIColors.InputSelection)
            {
                TextBox = _input
            };

            _input.Active = true;

            _input.OnTextChanged = _ =>
            {
                if (_applyingSuggestion)
                    return;

                RefreshSuggestions();
            };

            UIManager.Instance!.InputBlock = this;

            Refresh();

            if (Lines.Count == 0)
                Append("MonoCoC console. Type \"help\" for commands.");

            _inputHistoryIndex = InputHistory.Count;
        }

        private void Refresh()
        {
            _output.Clear();

            foreach (var line in Lines)
                _output.Append(line + "\n");

            _output.ScrollToEnd();
        }

        private void Append(string text)
        {
            Lines.Add(text);

            if (Lines.Count > 500)
                Lines.RemoveAt(0);

            _output.Append(text + "\n");

            _output.ScrollToEnd();
        }

        /// <summary>
        /// Writes a diagnostic line to the in-game console. Messages emitted
        /// before the overlay is opened are buffered and appear on first open.
        /// </summary>
        public static void WriteLine(string text)
        {
            WriteLine(text, null);
        }

        public static void WriteLine(string text, Color? color)
        {
            if (color is { } tint)
                text = $"/c[#{tint.R:X2}{tint.G:X2}{tint.B:X2}FF]{text}/cd";

            if (_instance is not null)
            {
                _instance.Append(text);

                return;
            }

            Lines.Add(text);

            if (Lines.Count > 500)
                Lines.RemoveAt(0);
        }

        public static void WriteError(string text) =>
            WriteLine(text, new Color(255, 90, 90));

        public static void WriteWarning(string text) =>
            WriteLine(text, new Color(255, 205, 100));

        private void ClearLines()
        {
            Lines.Clear();

            _output.Clear();
        }

        private void Submit()
        {
            var line = _input.Text.Trim();

            _input.ClearText();

            if (line == "")
                return;

            InputHistory.Add(line);

            _inputHistoryIndex = InputHistory.Count;

            Append("> " + line);

            var context = new CommandContext
            {
                Output = Append,

                Clear = ClearLines,

                Close = Close
            };

            if (!CommandRegistry.TryExecute(line, context, out var error) && error != "")
                Append(error);
        }

        private void ClearSuggestions()
        {
            _suggestions.Clear();

            _completionList.Clear();
        }

        private void RefreshSuggestions()
        {
            _suggestions.Clear();

            if (_input.Text == "")
            {
                _completionList.Clear();

                return;
            }

            _suggestions.AddRange(CommandRegistry.Complete(_input.Text, _input.Cursor.CursorIndex));

            _completionList.SetSuggestions(_suggestions);

            _completionList.LayoutData.Position = new Vector2(PanelPadding, ScreenHeight / 2 - InputHeight - PanelPadding - _completionList.Height - 4);
        }

        private void Complete()
        {
            if (_suggestions.Count == 0)
                RefreshSuggestions();

            if (_suggestions.Count == 0)
                return;

            var selected = _completionList.SelectedSuggestion;

            if (selected == null)
                return;

            ApplySuggestion(selected);

            RefreshSuggestions();
        }

        private void ApplySuggestion(string value)
        {
            var cursor = _input.Cursor.CursorIndex;

            var start = cursor;

            while (start > 0 && !char.IsWhiteSpace(_input.Text[start - 1]))

                start--;

            _applyingSuggestion = true;

            _input.ReplaceRange(start, cursor - start, value + (_suggestions.Count == 1 ? " " : ""));

            _applyingSuggestion = false;
        }

        private void HistoryStep(int direction)
        {
            if (InputHistory.Count == 0)
                return;

            _inputHistoryIndex = Math.Clamp(_inputHistoryIndex + direction, 0, InputHistory.Count - 1);

            _input.ClearText();

            _input.AppendString(InputHistory[_inputHistoryIndex]);

            _input.CursorTo(_input.Text.Length);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsOpen)
                return;

            if (UserInput.IsJustPress(Keys.Escape))
            {
                Close();

                return;
            }

            if (UserInput.IsJustPress(Keys.Enter))
            {
                ClearSuggestions();

                Submit();

                return;
            }

            if (UserInput.IsJustPress(Keys.Tab))
            {
                Complete();

                return;
            }

            HandleCompletionRepeat(Keys.Up, -1, gameTime);

            HandleCompletionRepeat(Keys.Down, 1, gameTime);
        }

        private void HandleCompletionRepeat(Keys key, int direction, GameTime gameTime)
        {
            if (UserInput.IsJustPress(key))
            {
                if (_completionList.Visible)
                {
                    _completionList.Select(direction);

                    _repeatKey = key;

                    _repeatDelay = CompletionRepeatDelay;
                }
                else
                {
                    HistoryStep(direction);

                    _repeatKey = null;
                }

                return;
            }

            if (_repeatKey != key)
                return;

            if (!UserInput.IsKeyDown(key) || !_completionList.Visible)
            {
                _repeatKey = null;

                return;
            }

            _repeatDelay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_repeatDelay > 0)
                return;

            _completionList.Select(direction);

            _repeatDelay = CompletionRepeatInterval;
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (args.Type == InputType.MouseLeftClick && !_panel.Rectangle.Contains(args.MousePosition))
            {
                Close();

                return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        public static bool IsOpen => _instance != null && _instance.LayoutData.Parent != null;

        public static void Toggle()
        {
            if (IsOpen)
            {
                _instance!.Close();

                return;
            }

            var root = UIManager.Instance?.Root;

            if (root == null)
                return;

            _instance = new ConsoleOverlay();

            _instance.Of(root);
        }

        private void Close()
        {
            _input.Active = false;

            if (ReferenceEquals(UIManager.Instance?.InputBlock, this))
                UIManager.Instance!.InputBlock = null;

            LayoutData.Parent?.LayoutData.Children.Remove(this);

            if (ReferenceEquals(_instance, this))
                _instance = null;
        }
    }

    public class ConsolePanel : Container
    {
        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            spriteBatch.DrawBorder(Rectangle, 2, new Color(130, 130, 130));

            spriteBatch.DrawRectangle(new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, 2), new Color(180, 180, 180));
        }
    }
}
