using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Materials;
using MonoCoC.Graphics.Shaders;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Data;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;
using System.IO;

using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using MonoCoC.UI.Animations;
using MonoCoC.UI.Structures;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class ChoiceWheel : Component, IFromHjson<ChoiceWheel> {
        private sealed class Entry
        {
            public string Id = "";
            public string LabelKey = "";
            public string ElderLabelKey = "";
            public string DescriptionKey = "";
            public string ElderDescriptionKey = "";
            public string Condition = "";
            public string LockedReasonKey = "";
            public string RequiresGender = "";
            public string RequiresRace = "";
        }

        private readonly List<Entry> _entries = [];
        private readonly List<Button> _buttons = [];

        private readonly List<Button> _lockedButtons = [];
        private int _selected = -1;
        private float _scrollPosition;
        private bool _hasFocus;
        private int _previousWheel;

        public bool Fade { get; set; } = true;

        public Color FadeColor { get; set; } = UIParseUtils.ColorOf("#E6D7BDFF");

        public float FadeTop { get; set; } = 0.2f;

        public float FadeBottom { get; set; } = 0.2f;

        public ChoiceWheel()
        {
            BackgroundColor = Color.Transparent;

            Scissor = true;
        }

        public int VisibleItems { get; set; } = 1;
        public float FadeMin { get; set; } = 1f;
        public float AnimationSpeed { get; set; } = 1f;
        public int FontSize { get; set; } = 1;
        public int CenterFontSize { get; set; } = 1;
        public string Direction { get; set; } = "vertical";

        private bool IsHorizontal => string.Equals(Direction, "horizontal", StringComparison.OrdinalIgnoreCase);

        public UIStateStyle? OptionStyle { get; set; }
        public UIStateStyle? LockedStyle { get; set; }
        public Color LockColor { get; set; } = UIParseUtils.ColorOf("#8A857CFF");
        public string LockIcon { get; set; } = "";

        public void PrewarmIcon()
        {
            if (string.IsNullOrWhiteSpace(LockIcon))
                return;

            foreach (var size in new[] { FontSize, CenterFontSize, 16, 20, 26 })
            {
                SvgTextureCache.GetFile(
                    CoC.Instance.GraphicsDevice,
                    LockIcon,
                    Math.Clamp(size, 8, 32),
                    new SkiaSharp.SKColor(LockColor.R, LockColor.G, LockColor.B, LockColor.A));
            }
        }
        public Func<string, bool>? Available { get; set; }
        public bool ShowLocked { get; set; }
        public string CurrentGender { get; set; } = "";
        public string CurrentAge { get; set; } = "";
        public string ResolveText(string text) => ChoiceTextResolver.Resolve(text, CurrentAge, CurrentGender);
        public string ExplainText(string text) => ChoiceTextResolver.Explain(text, CurrentAge, CurrentGender);

        public string FormatDescription(string text)
        {
            var body = ExplainText(text);

            if (!SelectedLocked)
                return body;

            return "Views.CharCreation.Lock.Locked".Localized() + "\n" +
                   SelectedLockedReason + "\n\n" +
                   body;
        }        public string CurrentRace { get; set; } = "";
        public Action<string>? SelectionChanged;
        public bool Elder { get; set; }
        public string? SelectedId => _selected >= 0 && _selected < _entries.Count ? _entries[_selected].Id : null;
        public bool SelectedLocked => _selected >= 0 && _selected < _entries.Count && !IsAvailable(_selected);
        public string SelectedLockedReasonKey => _selected < 0 ? "" : _entries[_selected].LockedReasonKey;
        public string SelectedDescriptionKey => _selected < 0 ? "" : (Elder && _entries[_selected].ElderDescriptionKey.Length > 0 ? _entries[_selected].ElderDescriptionKey : _entries[_selected].DescriptionKey);

        public void Load(string source)
        {
            _entries.Clear();

            var path = Path.Combine(Paths.Assets, "Data", "Creation", source + ".hjson");

            if (!File.Exists(path))
                return;

            if (MonoCoC.HjsonParser.Parse(File.ReadAllText(path)) is not JsonObject root)
                return;

            if (!root.TryGetValue("options", out var value) || value is not JsonArray options)
                return;

            foreach (var option in options)
            {
                if (option is not JsonObject obj)
                    continue;

                _entries.Add(new Entry
                {
                    Id = obj.Str("id"),
                    LabelKey = obj.Str("labelKey"),
                    ElderLabelKey = obj.Str("elderLabelKey"),
                    DescriptionKey = obj.Str("descriptionKey"),
                    ElderDescriptionKey = obj.Str("elderDescriptionKey"),
                    Condition = obj.Str("condition"),
                    LockedReasonKey = obj.Str("lockedReasonKey"),
                    RequiresGender = obj.Str("requiresGender"),
                    RequiresRace = obj.Str("requiresRace")
                });
            }

            Rebuild();
        }

        public void SetLabels(IEnumerable<string> labels)
        {
            _entries.Clear();

            foreach (var label in labels)
                _entries.Add(new Entry { LabelKey = label });

            Rebuild();
        }

        public void SetItems(IEnumerable<(string Id, string LabelKey)> items)
        {
            _entries.Clear();

            foreach (var item in items)
                _entries.Add(new Entry { Id = item.Id, LabelKey = item.LabelKey });

            Rebuild();
        }

        public void SelectAt(int index) => Select(index, false);

        public void SetElder(bool elder)        {
            if (Elder == elder)
                return;

            Elder = elder;
            Rebuild();
        }

        public void Refresh()
        {
            UpdateItems();
        }

        private void Rebuild()
        {
            foreach (var button in _buttons)
                RemoveChild(button);

            _buttons.Clear();

            for (var index = 0; index < _entries.Count; index++)
            {
                var optionIndex = index;
                var button = new Button("", LabelFor(_entries[index]), "coc:menu")
                {
                    LayoutData = { LayoutType = LayoutType.Relative }
                };

                button.Click(() =>
                {
                    _hasFocus = true;
                    Select(optionIndex);
                });

                AddChild(button);
                _buttons.Add(button);
            }

            var first = _entries.FindIndex(entry => entry.Condition.Length == 0 || Available?.Invoke(entry.Condition) != false);
            _selected = first >= 0 ? first : -1;
            _scrollPosition = _selected;
            UpdateItems();
        }

        private string LabelFor(Entry entry) =>
            (Elder && entry.ElderLabelKey.Length > 0 ? entry.ElderLabelKey : entry.LabelKey).Localized();

        private bool IsAvailable(int index) =>
            index >= 0 && index < _entries.Count &&
            (string.IsNullOrEmpty(_entries[index].RequiresGender) || string.Equals(_entries[index].RequiresGender, CurrentGender, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(_entries[index].RequiresRace) || string.Equals(_entries[index].RequiresRace, CurrentRace, StringComparison.OrdinalIgnoreCase)) &&
            (_entries[index].Condition.Length == 0 || Available?.Invoke(_entries[index].Condition) != false);

        public string SelectedLockedReason
        {
            get
            {
                if (_selected < 0 || _selected >= _entries.Count)
                    return "";

                var entry = _entries[_selected];

                if (!string.IsNullOrEmpty(entry.RequiresGender))
                    return "Views.CharCreation.Lock.Gender".Localized(entry.RequiresGender);

                if (!string.IsNullOrEmpty(entry.RequiresRace))
                    return "Views.CharCreation.Lock.Race".Localized(entry.RequiresRace);

                if (entry.Condition == "cock")
                    return "Views.CharCreation.Lock.RequiresCock".Localized();

                if (entry.Condition == "vagina")
                    return "Views.CharCreation.Lock.RequiresVagina".Localized();

                return entry.LockedReasonKey.Length > 0
                    ? entry.LockedReasonKey.Localized()
                    : "Views.CharCreation.Lock.Required".Localized();
            }
        }

        private static int ShortestDelta(int from, int to, int count)
        {
            if (count == 0) return 0;
            var delta = (to - from + count) % count;
            if (delta > count / 2) delta -= count;
            return delta;
        }

        private static float ShortestDeltaFloat(float from, float to, int count)
        {
            if (count == 0) return 0;
            var delta = to - from;
            delta -= count * (float)Math.Round(delta / count);
            return delta;
        }

        private void Select(int index, bool notify = true)
        {
            if (_entries.Count == 0) return;

            _selected = Math.Clamp(index, 0, _entries.Count - 1);

            if (notify)
                SelectionChanged?.Invoke(_entries[_selected].Id);
        }

        private void EnsureSelectionAvailable()
        {
            if (_entries.Count == 0)
                return;

            if (_selected >= 0 && IsAvailable(_selected))
                return;

            var fallback = _entries.FindIndex(entry => entry.Condition.Length == 0 || Available?.Invoke(entry.Condition) != false);

            if (fallback >= 0)
                _selected = fallback;
        }

        private void Move(int delta)
        {
            if (_entries.Count == 0) return;

            var candidate = _selected + delta;

            if (candidate < 0 || candidate >= _entries.Count)
                return;

            Select(candidate);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsHovering)
                _hasFocus = true;

            var wheel = UserInput.CurrentMouseState.ScrollWheelValue;
            if (IsHovering && wheel != _previousWheel)
                Move(wheel > _previousWheel ? -1 : 1);
            _previousWheel = wheel;

            if (_hasFocus)
            {
                if (IsHorizontal)
                {
                    if (IsKeyPressed(Keys.Left)) Move(-1);
                    if (IsKeyPressed(Keys.Right)) Move(1);
                }
                else
                {
                    if (IsKeyPressed(Keys.Up)) Move(-1);
                    if (IsKeyPressed(Keys.Down)) Move(1);
                }
            }

            if (_selected >= 0 && _entries.Count > 0)
            {
                var delta = _selected - _scrollPosition;

                _scrollPosition += delta * AnimationSpeed;

                if (Math.Abs(delta) < 0.01f)
                    _scrollPosition = _selected;
            }

        }

        public override void PerformLayout(GameTime gameTime)
        {
            base.PerformLayout(gameTime);

            UpdateItems();
        }

        private static bool IsKeyPressed(Keys key) =>
            UserInput.PreviousKeyState.IsKeyUp(key) && UserInput.CurrentKeyState.IsKeyDown(key);

        private void UpdateItems()
        {
            var half = VisibleItems / 2;
            var rowHeight = Height / (float)VisibleItems;
            var columnWidth = Width / (float)VisibleItems;
            var centerY = (Height - rowHeight) / 2f;
            var centerX = (Width - columnWidth) / 2f;

            for (var index = 0; index < _buttons.Count; index++)
                _buttons[index].Visible = false;

            _lockedButtons.Clear();

            if (_entries.Count == 0)
                return;

            var shown = new HashSet<int>();

            for (var index = 0; index < _buttons.Count; index++)
            {
                var delta = index - _scrollPosition;

                if (Math.Abs(delta) > half + 1)
                    continue;

                var dataIndex = (index + _entries.Count) % _entries.Count;

                if (!shown.Add(dataIndex))
                    continue;

                var available = IsAvailable(dataIndex);

                if (!available && !ShowLocked)
                    continue;

                var button = _buttons[index];
                var animatedSlot = delta;
                var centerAmount = dataIndex == _selected ? 1f : Math.Clamp(1f - Math.Abs(animatedSlot), 0f, 1f);
                var alpha = Fade
                    ? Math.Clamp(FadeMin + (1f - FadeMin) * (1f - Math.Abs(animatedSlot) / (half + 1)), 0f, 1f)
                    : 1f;

                button.Visible = true;

                if (IsHorizontal)
                {
                    button.LayoutData.Size = new Vector2(columnWidth, Height);
                    button.LayoutData.Position = new Vector2(centerX + animatedSlot * columnWidth, 0);
                }
                else
                {
                    button.LayoutData.Size = new Vector2(Width, rowHeight);
                    button.LayoutData.Position = new Vector2(0, centerY + animatedSlot * rowHeight);
                }

                button.StateStyle = OptionStyle;
                button.Enabled = true;

                if (!available)
                    _lockedButtons.Add(button);

                button.SetLabel(LabelFor(_entries[dataIndex]));
                button.Selected = dataIndex == _selected;
                button.FontSizeOverride = (int)Math.Round(FontSize + (CenterFontSize - FontSize) * centerAmount);
                button.LayoutData.Alpha = alpha;
                button.Label.LayoutData.Alpha = alpha;
                button.Background?.LayoutData.Alpha = alpha;
                button.RefreshVisual();
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.PostDraw(spriteBatch, gameTime);

            if (string.IsNullOrWhiteSpace(LockIcon))
                return;

            foreach (var button in _lockedButtons)
            {
                var size = Math.Clamp((int)((button.FontSizeOverride ?? FontSize) * 0.62f), 8, 18);

                Texture2D? texture;

                try
                {
                    texture = SvgTextureCache.GetFile(
                        CoC.Instance.GraphicsDevice,
                        LockIcon,
                        size,
                        new SkiaSharp.SKColor(LockColor.R, LockColor.G, LockColor.B, LockColor.A));
                }
                catch (Exception ex)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] svg icon error: {ex.Message}");

                    continue;
                }

                if (texture is null)
                {
                    MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                        $"[UI] lock icon render failed: icon={LockIcon}, size={size}");

                    continue;
                }

                var rect = new Rectangle(
                    button.Rectangle.X - size - 4,
                    button.Rectangle.Y + (button.Rectangle.Height - size) / 2,
                    size,
                    size);

                spriteBatch.Draw(texture, rect, Color.White * (button.LayoutData.Alpha));
            }
        }
    
        public static string TypeId => "coc:choice_wheel";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var wheel = new ChoiceWheel
            {
                VisibleItems = node.Int("visibleItems", 5),
                Direction = node.Str("direction", "vertical"),
                FadeMin = node.Float("fadeMin", 0.35f),
                AnimationSpeed = node.Float("animationSpeed", 0.18f),
                FontSize = node.Int("fontSize", 20),
                CenterFontSize = node.Int("centerFontSize", 26),
                Fade = node.Bool("fade", true),
                FadeColor = UIParseUtils.ColorOf(node.Str("fadeColor", "#E6D7BDFF")),
                FadeTop = node.Float("fadeTop", 0.2f),
                FadeBottom = node.Float("fadeBottom", 0.2f),
                ShowLocked = node.Bool("showLocked", false),
                LockColor = UIParseUtils.ColorOf(node.Str("lockColor", "#8A857CFF")),
                LockIcon = node.Str("lockIcon")
            };

            var thickness = node.Int("borderThickness", 0);

            if (node.TryGetValue("states", out var stateValue) && stateValue is JsonObject states)
            {
                var style = new UIStateStyle { BorderThickness = thickness };

                ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
                ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
                ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
                ApplyState(states, "selected", out style.SelectedBackground, out style.SelectedBorder, out style.SelectedText);

                ApplyStateThickness(states, "normal", ref style.NormalBorderThickness);
                ApplyStateThickness(states, "hover", ref style.HoverBorderThickness);
                ApplyStateThickness(states, "pressed", ref style.PressedBorderThickness);
                ApplyStateThickness(states, "selected", ref style.SelectedBorderThickness);

                InheritStateFallbacks(states, style);

                wheel.OptionStyle = style;

                if (HasStateField(states, "locked", "backgroundColor") ||
                    HasStateField(states, "locked", "textColor"))
                {
                    var locked = new UIStateStyle { BorderThickness = thickness };

                    ApplyState(states, "locked", out locked.NormalBackground, out locked.NormalBorder, out locked.NormalText);

                    locked.HoverBackground = locked.NormalBackground;
                    locked.HoverBorder = locked.NormalBorder;
                    locked.HoverText = locked.NormalText;
                    locked.PressedBackground = locked.NormalBackground;
                    locked.PressedBorder = locked.NormalBorder;
                    locked.PressedText = locked.NormalText;
                    locked.SelectedBackground = locked.NormalBackground;
                    locked.SelectedBorder = locked.NormalBorder;
                    locked.SelectedText = locked.NormalText;
                    locked.NormalBorderThickness = thickness;
                    locked.HoverBorderThickness = thickness;
                    locked.PressedBorderThickness = thickness;
                    locked.SelectedBorderThickness = thickness;

                    wheel.LockedStyle = locked;
                }
            }

            wheel.Load(node.Str("source"));

            return wheel;
        }


        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<ChoiceWheel>();
    
}
}
