using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components
{
    public class Selector : Component
    {
        private readonly Button _previous;

        private readonly Button _next;

        private readonly Label _value;

        private readonly List<string> _options = [];

        private int _index;

        public Selector()
        {
            BackgroundColor = UIComponentStyles.Selector.Background;

            _previous = new Button("", "<", "coc:menu")
            {
                BackgroundColor = Color.Transparent,
                FontSizeOverride = 22,
                TextColor = UIComponentStyles.Selector.Text
            };

            _previous.Click(() => Move(-1));

            _previous.LayoutData.LayoutType = LayoutType.Relative;

            _next = new Button("", ">", "coc:menu")
            {
                BackgroundColor = Color.Transparent,
                FontSizeOverride = 22,
                TextColor = UIComponentStyles.Selector.Text
            };

            _next.Click(() => Move(1));

            _next.LayoutData.LayoutType = LayoutType.Relative;

            _value = new Label(UIFonts.GetAt(UIFonts.Family("coc:menu"), 20))
            {
                TextColor = UIComponentStyles.Selector.Text,
                CanHit = false,
                AutoSize = false
            };

            _value.Alignment(TextHorizontalAlignment.Center);

            _value.VerticalAlign(TextVerticalAlignment.Top);

            _value.LayoutData.LayoutType = LayoutType.Relative;

            AddChild(_previous);

            AddChild(_value);

            AddChild(_next);
        }

        public Action<int>? ValueChanged;

        public IReadOnlyList<string> Options => _options;

        public int SelectedIndex
        {
            get => _index;

            set
            {
                if (_options.Count == 0)
                {
                    _index = 0;

                    return;
                }

                _index = Math.Clamp(value, 0, _options.Count - 1);

                _value.Text(_options[_index]);
            }
        }

        public void SetOptions(IEnumerable<string> options, int selected = 0)
        {
            _options.Clear();

            _options.AddRange(options);

            SelectedIndex = selected;
        }

        private void Move(int direction)
        {
            if (_options.Count == 0)
                return;

            SelectedIndex = (_index + direction + _options.Count) % _options.Count;

            ValueChanged?.Invoke(_index);
        }

        public override void PerformLayout(GameTime gameTime)
        {
            base.PerformLayout(gameTime);

            var buttonWidth = Math.Min(48, Math.Max(32, Width / 6));

            var fontSize = Math.Clamp((int)(Height * 0.45f), 16, 22);

            _previous.FontSizeOverride = fontSize;

            _next.FontSizeOverride = fontSize;

            if (_value.RichTextLayout.Font.FontSize != fontSize)
                _value.RichTextLayout.Font = UIFonts.GetAt(UIFonts.Family("coc:menu"), fontSize);

            _previous.Size(buttonWidth, Height);

            _previous.Position(0, 0);

            _next.Size(buttonWidth, Height);

            _next.Position(Width - buttonWidth, 0);

            _value.Size(Width, Height);

            var text = _options.Count == 0 ? "" : _options[_index];

            var textHeight = _value.RichTextLayout.Font.MeasureString(text).Y;

            _value.Position(0, (int)Math.Max(0, (Height - textHeight) / 2f));
        }
    }
}
