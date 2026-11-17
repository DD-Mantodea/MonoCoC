using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.UI.Animations;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components
{
    public class SegmentedControl : Component
    {
        private readonly List<Button> _buttons = [];

        private readonly Component _highlight = new() { CanHit = false };

        private Color _highlightColor = Color.Transparent;

        private float _visualIndex;

        private readonly AnimationPlayer _animator = new();

        private int _selected;

        public float AnimationDuration { get; set; } = 0.22f;

        public Func<float, float> AnimationEasing { get; set; } = Easing.EaseOutCubic;

        public Action<int>? ValueChanged;

        public int OptionCount => _buttons.Count;

        public UIStateStyle? OptionStyle;

        public SegmentedControl()
        {
            BackgroundColor = Color.Transparent;

            _highlight.LayoutData.LayoutType = LayoutType.Relative;

            AddChild(_highlight);
        }

        public int SelectedIndex
        {
            get => _selected;

            set
            {
                if (_buttons.Count == 0)
                    return;

                _selected = Math.Clamp(value, 0, _buttons.Count - 1);

                _animator.Play(new FloatAnimation(
                    _visualIndex,
                    _selected,
                    value => _visualIndex = value,
                    AnimationDuration)
                {
                    Easing = AnimationEasing
                });

                for (var index = 0; index < _buttons.Count; index++)
                {
                    var button = _buttons[index];

                    var selected = index == _selected;

                    button.Selected = selected;

                    button.RefreshVisual();
                }
            }
        }

        public void SetOptionLabels(IReadOnlyList<string> options)
        {
            var count = Math.Min(options.Count, _buttons.Count);

            for (var index = 0; index < count; index++)
            {
                _buttons[index].SetLabel(options[index]);
                _buttons[index].RefreshVisual();
            }
        }

        public void SetOptions(IEnumerable<string> options, int selected = 0)
        {
            foreach (var button in _buttons)
                RemoveChild(button);

            _buttons.Clear();

            if (OptionStyle is not null)
            {
                if (_highlightColor == Color.Transparent)
                    _highlightColor = OptionStyle.SelectedBackground;

                OptionStyle.SelectedBackground = Color.Transparent;
                OptionStyle.SelectedBorder = Color.Transparent;
            }

            _highlight.BackgroundColor = _highlightColor;

            foreach (var option in options)
            {
                var index = _buttons.Count;

                var button = new Button("", option, "coc:menu")
                {
                    StateStyle = OptionStyle,
                    LayoutData = { LayoutType = LayoutType.Relative }
                };

                button.Click(() =>
                {
                    SelectedIndex = index;

                    ValueChanged?.Invoke(index);
                });

                AddChild(button);

                _buttons.Add(button);
            }

            _visualIndex = selected;

            SelectedIndex = selected;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_buttons.Count == 0)
                return;

            _animator.Update(gameTime);

        }

        public override void PerformLayout(GameTime gameTime)
        {
            base.PerformLayout(gameTime);

            if (_buttons.Count == 0)
                return;

            var width = Width / _buttons.Count;

            for (var index = 0; index < _buttons.Count; index++)
            {
                var button = _buttons[index];

                button.LayoutData.Size = new Vector2(width - 4, Height);

                button.LayoutData.Position = new Vector2(index * width + 2, 0);

                button.FontSizeOverride = Math.Clamp((int)(Height * 0.42f), 14, 20);
            }

            _highlight.LayoutData.Size = new Vector2(width - 4, Height);
            _highlight.LayoutData.Position = new Vector2(_visualIndex * width + 2, 0);
            _highlight.BackgroundColor = _highlightColor;
        }
    }
}
