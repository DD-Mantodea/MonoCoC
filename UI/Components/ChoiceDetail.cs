using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.UI.Components
{
    public class ChoiceDetail : Component
    {
        private sealed class Entry
        {
            public string Id = "";

            public string LabelKey = "";

            public string ElderLabelKey = "";

            public string DescriptionKey = "";

            public string ElderDescriptionKey = "";

            public string Condition = "";
        }

        private readonly List<Entry> _entries = [];

        private readonly List<Button> _buttons = [];

        private readonly Label _detail;

        private readonly Button? _confirm;

        private readonly bool _fade;

        private int _selected = -1;

        private bool _hasFocus;

        private int _previousWheel;

        private float _visualOffset;

        public ChoiceDetail(
            int visibleItems,
            string fontRole = "coc:menu",
            string confirmKey = "",
            string detailColor = "#35291FFF",
            bool fade = true)
        {
            VisibleItems = Math.Max(3, visibleItems);

            _fade = fade;

            _detail = new Label(UIFonts.GetAt(UIFonts.Family(fontRole), 18))
            {
                TextColor = ParseColor(detailColor),
                CanHit = false
            };

            _detail.LayoutData.LayoutType = LayoutType.Relative;

            AddChild(_detail);

            if (confirmKey.Length > 0)
            {
                _confirm = new Button("", confirmKey.Localized(), fontRole)
                {
                    LayoutData = { LayoutType = LayoutType.Relative }
                };

                _confirm.Click(() =>
                {
                    if (SelectedId is not null)
                        Confirmed?.Invoke();
                });

                AddChild(_confirm);
            }

            _previousWheel = UserInput.CurrentMouseState.ScrollWheelValue;
        }

        public int VisibleItems { get; set; }

        public float FadeMin { get; set; } = 0.25f;

        public float AnimationSpeed { get; set; } = 0.18f;

        public int FontSize { get; set; } = 20;

        public int CenterFontSize { get; set; } = 24;

        public UIStateStyle? CenterStyle { get; set; }

        public UIStateStyle? OtherStyle { get; set; }

        public Func<string, bool>? Available { get; set; }

        public Action<string>? SelectionChanged;

        public Action? Confirmed;

        public bool Elder { get; set; }

        public string? SelectedId => _selected >= 0 && _selected < _entries.Count ? _entries[_selected].Id : null;

        public void Load(string source)
        {
            _entries.Clear();

            var path = Path.Combine(Paths.Assets, "Data", "Creation", source + ".hjson");

            if (!File.Exists(path))
                return;

            if (MonoCoC.HjsonParser.Parse(File.ReadAllText(path)) is not Hjson.JsonObject root)
                return;

            if (!root.TryGetValue("options", out var value) || value is not Hjson.JsonArray options)
                return;

            foreach (var option in options)
            {
                if (option is not Hjson.JsonObject obj)
                    continue;

                _entries.Add(new Entry
                {
                    Id = obj.Str("id"),
                    LabelKey = obj.Str("labelKey"),
                    ElderLabelKey = obj.Str("elderLabelKey"),
                    DescriptionKey = obj.Str("descriptionKey"),
                    ElderDescriptionKey = obj.Str("elderDescriptionKey"),
                    Condition = obj.Str("condition")
                });
            }

            Rebuild();
        }

        public void SetElder(bool elder)
        {
            if (Elder == elder)
                return;

            Elder = elder;

            Rebuild();
        }

        public void Refresh()
        {
            Rebuild();
        }

        private void Rebuild()
        {
            foreach (var button in _buttons)
                RemoveChild(button);

            _buttons.Clear();

            for (var index = 0; index < _entries.Count; index++)
            {
                var entry = _entries[index];

                var optionIndex = index;

                var button = new Button("", LabelFor(entry), "coc:menu")
                {
                    LayoutData = { LayoutType = LayoutType.Relative }
                };

                button.Click(() =>
                {
                    _hasFocus = true;

                    _visualOffset = ShortestDelta(_selected, optionIndex);

                    Select(optionIndex);
                });

                AddChild(button);

                _buttons.Add(button);
            }

            var first = _entries.FindIndex(entry => entry.Condition.Length == 0 || Available?.Invoke(entry.Condition) != false);

            _selected = first >= 0 ? first : -1;

            UpdateItems();

            if (_selected >= 0)
                Select(_selected, false);
        }

        private string LabelFor(Entry entry) =>
            (Elder && entry.ElderLabelKey.Length > 0 ? entry.ElderLabelKey : entry.LabelKey).Localized();

        private bool IsAvailable(int index) =>
            index >= 0 && index < _entries.Count &&
            (_entries[index].Condition.Length == 0 || Available?.Invoke(_entries[index].Condition) != false);

        private void Select(int index, bool notify = true)
        {
            if (_entries.Count == 0)
                return;

            var direction = index > _selected ? 1 : -1;

            var candidate = Math.Clamp(index, 0, _entries.Count - 1);

            for (var i = 0; i < _entries.Count; i++)
            {
                candidate = (candidate + _entries.Count) % _entries.Count;

                if (IsAvailable(candidate))
                    break;

                candidate = (candidate + direction + _entries.Count) % _entries.Count;
            }

            _selected = candidate;

            UpdateItems();

            var entry = _entries[_selected];

            var key = Elder && entry.ElderDescriptionKey.Length > 0
                ? entry.ElderDescriptionKey
                : entry.DescriptionKey;

            _detail.Text(key.Localized());

            if (notify)
                SelectionChanged?.Invoke(entry.Id);
        }

        private int ShortestDelta(int from, int to)
        {
            var count = _entries.Count;

            if (count == 0)
                return 0;

            var delta = (to - from + count) % count;

            if (delta > count / 2)
                delta -= count;

            return delta;
        }

        private void Move(int delta)
        {
            if (_entries.Count == 0)
                return;

            var candidate = _selected;

            for (var i = 0; i < _entries.Count; i++)
            {
                candidate = (candidate + delta + _entries.Count) % _entries.Count;

                if (IsAvailable(candidate))
                {
                    _visualOffset = delta;

                    Select(candidate);

                    return;
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsHovering)
                _hasFocus = true;

            var wheel = UserInput.CurrentMouseState.ScrollWheelValue;

            if (IsHovering && wheel != _previousWheel)
            {
                Move(wheel > _previousWheel ? -1 : 1);

                _previousWheel = wheel;
            }
            else
            {
                _previousWheel = wheel;
            }

            if (_hasFocus)
            {
                if (IsKeyPressed(Keys.Up))
                    Move(-1);

                if (IsKeyPressed(Keys.Down))
                    Move(1);
            }

            _visualOffset *= 1f - AnimationSpeed;

            if (Math.Abs(_visualOffset) < 0.001f)
                _visualOffset = 0;

            UpdateItems();
        }

        private static bool IsKeyPressed(Keys key) =>
            UserInput.PreviousKeyState.IsKeyUp(key) && UserInput.CurrentKeyState.IsKeyDown(key);

        private void UpdateItems()
        {
            var half = VisibleItems / 2;

            var rowHeight = Height / (float)VisibleItems;

            var centerY = (Height - rowHeight) / 2f;

            var visible = new List<(int Index, int Slot)>();

            if (_selected >= 0)
            {
                for (var slot = -half; slot <= half; slot++)
                {
                    var index = (_selected + slot + _entries.Count) % _entries.Count;

                    if (IsAvailable(index))
                        visible.Add((index, slot));
                }
            }

            for (var index = 0; index < _buttons.Count; index++)
                _buttons[index].Visible = false;

            foreach (var (index, slot) in visible)
            {
                var button = _buttons[index];

                var animatedSlot = slot + _visualOffset;

                var alpha = _fade
                    ? Math.Clamp(FadeMin + (1f - FadeMin) * (1f - Math.Abs(animatedSlot) / half), 0f, 1f)
                    : 1f;

                button.Visible = true;

                button.LayoutData.Size = new Vector2(Width * 0.36f, rowHeight);

                button.LayoutData.Position = new Vector2(0, centerY + animatedSlot * rowHeight);

                var centered = Math.Abs(animatedSlot) < 0.5f;

                button.StateStyle = centered ? CenterStyle : OtherStyle;

                button.Selected = false;

                var centerAmount = Math.Clamp(1f - Math.Abs(animatedSlot), 0f, 1f);

                button.FontSizeOverride = (int)Math.Round(FontSize + (CenterFontSize - FontSize) * centerAmount);

                button.LayoutData.Alpha = alpha;

                button.Label.LayoutData.Alpha = alpha;

                if (button.Background is not null)
                    button.Background.LayoutData.Alpha = alpha;

                button.RefreshVisual();

                button.LayoutData.Alpha = alpha;

                button.Label.LayoutData.Alpha = alpha;
            }

            var detailX = Width * 0.36f + 24;

            _detail.LayoutData.Size = new Vector2(Width - detailX, Height);

            _detail.LayoutData.Position = new Vector2(detailX, 0);

            _detail.WrapWidth((int)Math.Max(1, Width - detailX));

            if (_confirm is not null)
            {
                _confirm.LayoutData.Size = new Vector2(160, 36);

                _confirm.LayoutData.Position = new Vector2(Width - 160, Height - 36);
            }
        }

        private static Color ParseColor(string value)
        {
            var hex = value.TrimStart('#');

            if (hex.Length == 8 && uint.TryParse(hex, System.Globalization.NumberStyles.HexNumber, null, out var argb))
                return new Color(
                    (byte)((argb >> 16) & 0xFF),
                    (byte)((argb >> 8) & 0xFF),
                    (byte)(argb & 0xFF),
                    (byte)((argb >> 24) & 0xFF));

            return Color.White;
        }
    }
}
