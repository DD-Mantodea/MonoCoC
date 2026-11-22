using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.UI.Animations;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;

using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Structures;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class SegmentedComponent : Component, IFromHjson<SegmentedComponent> {
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

        public SegmentedComponent()
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

                button.FontSizeOverride = Math.Clamp((int)(Height * 0.42f), 14, 28);
            }

            _highlight.LayoutData.Size = new Vector2(width - 4, Height);
            _highlight.LayoutData.Position = new Vector2(_visualIndex * width + 2, 0);
            _highlight.BackgroundColor = _highlightColor;
        }
    
        public static string TypeId => "coc:segmented";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var control = new SegmentedComponent
            {
                AnimationDuration = node.Float("animationDuration", 0.22f),
                AnimationEasing = MonoCoC.UI.Animations.Easing.FromName(node.Str("animationEasing"))
            };

            if (node.TryGetValue("states", out var stateValue) && stateValue is JsonObject states)
            {
                var style = new UIStateStyle
                {
                    BorderThickness = node.Int("borderThickness", 0)
                };

                ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
                ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
                ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
                ApplyState(states, "selected", out style.SelectedBackground, out style.SelectedBorder, out style.SelectedText);

                ApplyStateThickness(states, "normal", ref style.NormalBorderThickness);
                ApplyStateThickness(states, "hover", ref style.HoverBorderThickness);
                ApplyStateThickness(states, "pressed", ref style.PressedBorderThickness);
                ApplyStateThickness(states, "selected", ref style.SelectedBorderThickness);

                InheritStateFallbacks(states, style);

                control.OptionStyle = style;
            }

            var values = node.Array("options");
            var keyValues = node.Array("optionKeys");

            if (keyValues is not null)
            {
                var options = new List<string>();

                foreach (var value in keyValues)
                    if (value is not null)
                        options.Add(new UILocalizedText { Key = value.ToString().Trim('"') }.Resolve());

                control.SetOptions(options, node.Int("value", 0));
            }
            else if (values is not null)
            {
                var options = new List<string>();

                foreach (var value in values)
                    if (value is not null)
                        options.Add(value.ToString().Trim('"'));

                control.SetOptions(options, node.Int("value", 0));
            }

            return control;
        }


        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<SegmentedComponent>();
    
}
}
