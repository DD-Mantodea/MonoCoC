using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using static MonoCoC.UI.Layout.LayoutConstants;

using FontStashSharp;
using Hjson;
using MonoCoC.UI.Styles;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class Button : Component, IUIStatefulPart, IFromHjson<Button> {
        private record Snapshot(
            string LabelText,
            string HintText,
            string HintHeaderText,
            bool IsVisible,
            bool IsEnabled,
            Action? ClickAction,
            Color TextColorValue);

        public string FontRole = "button";

        public Button(string textureID = "", string label = "", string fontRole = "button")
        {
            FontRole = fontRole;

            var texture = TextureRegistry.Instance.Resolve(textureID);

            Background = texture is null ? null : new UIImage(texture)
                .SizeType(SizeType.Fill)
                .LayoutType(LayoutType.Relative)
                .CanHit(false)
                .Of(this);

            Label = new Label(UIFonts.Get(FontRole))
                .Text(label)
                .Alignment(TextHorizontalAlignment.Center)
                .LayoutType(LayoutType.Relative)
                .Position(ButtonLabelX, ButtonLabelY)
                .CanHit(false)
                .Of(this);

            MouseCursor = MouseCursor.Hand;
        }

        public int LabelPadding = 10;

        public TextHorizontalAlignment LabelAlignment = TextHorizontalAlignment.Center;

        public Color TextColor = Color.Black;

        public Color IconColor = Color.White;

        public Color IconHoverColor = Color.White;

        public UIImage? Icon;

        public int? FontSizeOverride;

        public bool FitLabelEnabled = true;

        private int _fittedWidth = -1;

        private int _fittedPadding = -1;

        private TextHorizontalAlignment _fittedAlignment = TextHorizontalAlignment.Center;

        private string _fittedText = "";

        private int _fittedRevision = -1;

        private int _fittedFontSize = -1;

        public void RefreshLabelFit() => FitLabel();

        private void FitLabel()
        {
            if (!FitLabelEnabled)
            {
                var fixedText = Label.RichTextLayout.Text ?? "";

                var fixedInset = LabelAlignment == TextHorizontalAlignment.Center ? 0 : Math.Max(0, LabelPadding);

                var fixedInnerWidth = Math.Max(1, Width - fixedInset * 2);

                Label.Alignment(LabelAlignment);

                Label.WrapWidth(fixedInnerWidth);

                Label.LayoutData.Size = new Vector2(fixedInnerWidth, Label.Height);

                var fixedLineHeight = (int)Label.RichTextLayout.Font.MeasureString(fixedText).Y;

                Label.Position(fixedInset, fixedLineHeight > 0 ? Math.Max(0, (Height - fixedLineHeight) / 2) : ButtonLabelY);

                return;
            }

            var text = Label.RichTextLayout.Text ?? "";

            var width = Width;

            var size = FontSizeOverride ?? UIFonts.Size(FontRole);

            if (width <= 0 || (width == _fittedWidth && text == _fittedText &&
                               LabelPadding == _fittedPadding &&
                               LabelAlignment == _fittedAlignment &&
                               UIFonts.Revision == _fittedRevision &&
                               size == _fittedFontSize))
                return;

            _fittedWidth = width;

            _fittedText = text;

            _fittedPadding = LabelPadding;

            _fittedAlignment = LabelAlignment;

            _fittedRevision = UIFonts.Revision;

            _fittedFontSize = size;

            var family = UIFonts.Family(FontRole);

            var font = UIFonts.GetAt(family, size);

            while (size > 12 && font.MeasureString(text).X > width - LabelPadding)
            {
                size--;

                font = UIFonts.GetAt(family, size);
            }

            Label.RichTextLayout.Font = font;

            var lineHeight = (int)font.MeasureString(text).Y;

            var labelY = lineHeight > 0 ? Math.Max(0, (Height - lineHeight) / 2) : ButtonLabelY;

            Label.Alignment(LabelAlignment);

            var inset = LabelAlignment == TextHorizontalAlignment.Center ? 0 : Math.Max(0, LabelPadding);

            var innerWidth = Math.Max(1, width - inset * 2);

            Label.WrapWidth(innerWidth);

            Label.LayoutData.Size = new Vector2(innerWidth, Label.Height);

            Label.Position(inset, labelY);
        }

        public UIImage Background;

        public Label Label;

        public new Action OnClick;

        public Action? OnMouseDown;

        public Action<Vector2>? OnMouseMove;

        public Action<Vector2>? OnMouseUp;

        public string Hint = "";

        public object? CaptureState() => new Snapshot(
            Label.RichTextLayout.Text ?? "",
            Hint,
            HintHeader,
            Visible,
            Enabled,
            OnClick,
            TextColor);

        public void RestoreState(object value)
        {
            if (value is not Snapshot state)
                return;

            Label.Text(state.LabelText);

            Hint = state.HintText;

            HintHeader = state.HintHeaderText;

            Visible = state.IsVisible;

            Enabled = state.IsEnabled;

            OnClick = state.ClickAction;

            TextColor = state.TextColorValue;

            RefreshVisual();
        }

        public string HintHeader = "";

        public HintStyle? HintSettings;

        public HintView? HintView;

        public override string ToolTipText => Enabled ? Hint : (string.IsNullOrEmpty(DisabledReason) ? Hint : DisabledReason);

        public override string ToolTipHeader => string.IsNullOrEmpty(HintHeader) ? Label.RichTextLayout.Text : HintHeader;

        public string DisabledReason = "";

        public string DisableIf = "";

        private bool _conditionDisabled;

        private bool _enabledBeforeCondition;

        private double _conditionTimer;

        private string _lastConditionError = "";

        public override void Update(GameTime gameTime)
        {
            UpdateCondition(gameTime);

            base.Update(gameTime);

            FitLabel();
        }

        private void UpdateCondition(GameTime gameTime)
        {
            if (DisableIf.Length == 0)
            {
                if (_conditionDisabled)
                {
                    _conditionDisabled = false;

                    Enabled = _enabledBeforeCondition;

                    RefreshVisual();
                }

                return;
            }

            _conditionTimer -= gameTime.ElapsedGameTime.TotalSeconds;

            if (_conditionTimer > 0)
                return;

            _conditionTimer = 0.25;

            if (!MonoCoC.Scripting.ConditionEvaluator.TryEvaluate(DisableIf, out var disabled, out var error))
            {
                if (error != _lastConditionError)
                {
                    _lastConditionError = error;

                    MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] disableIf error: {error}");
                }

                return;
            }

            _lastConditionError = "";

            if (disabled == _conditionDisabled)
                return;

            _conditionDisabled = disabled;

            if (disabled)
            {
                _enabledBeforeCondition = Enabled;

                Enabled = false;
            }
            else
            {
                Enabled = _enabledBeforeCondition;
            }

            RefreshVisual();
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (!Visible || !Enabled)
                return InputResult.Continue;

            if (args.Type == InputType.MouseLeftDown)
            {
                IsPressed = true;

                OnMouseDown?.Invoke();

                return InputResult.Consumed;
            }

            if (args.Type == InputType.MouseMove && OnMouseMove != null)
            {
                OnMouseMove(args.MousePosition);

                return InputResult.Continue;
            }

            if (args.Type == InputType.MouseLeftUp)
            {
                IsPressed = false;

                OnMouseUp?.Invoke(args.MousePosition);
            }

            if (args.Type == InputType.MouseLeftClick)
            {
                IsPressed = false;

                UIManager.Instance?.HideHint();

                OnClick?.Invoke();

                return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        public Button Reset(bool clearLabel = true)
        {
            if (clearLabel)
                Label.Text("");

            OnClick = null;

            Enabled = true;

            Visible = false;

            DisabledReason = "";

            DisableIf = "";

            _conditionDisabled = false;

            _enabledBeforeCondition = true;

            Hint = "";

            HintHeader = "";

            return RefreshVisual();
        }

        public Button RefreshVisual()
        {
            Background?.LayoutData.Alpha = !Enabled ? 0.4f : IsHovering ? 0.5f : 1f;

            if (Icon is not null)
                Icon.Color = !Enabled ? IconColor * 0.4f : IsHovering ? IconHoverColor : IconColor;

            var stateText = StateStyle?.TextFor(Enabled, IsHovering, IsPressed, Selected);

            Label?.TextColor = stateText ?? TextColor;

            return this;
        }

        public override void OnMouseEnter()
        {
            base.OnMouseEnter();

            RefreshVisual();
        }

        public override void OnMouseLeave()
        {
            base.OnMouseLeave();

            IsPressed = false;

            RefreshVisual();
        }

        public Button SetLabel(string text)
        {
            Label.Text(text);

            FitLabel();

            return this;
        }

        public Button SetIcon(string relativePath, int size, int x, int y)
        {
            var texture = SvgTextureCache.GetFile(CoC.Instance.GraphicsDevice, relativePath, size, SkiaSharp.SKColors.White);

            if (texture is null)
                return this;

            Icon = new UIImage(texture)
                .Size(size, size)
                .Position(x, y)
                .LayoutType(LayoutType.Relative)
                .CanHit(false)
                .Of(this);

            RefreshVisual();

            return this;
        }

        public Button Click(Action onClick)
        {
            OnClick = onClick;

            return this;
        }
    
        public static string TypeId => "coc:button";
        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var background = node.TryGetValue("background", out var backgroundValue)
                ? backgroundValue.ToString().Trim('"')
                : node.Str("texture");

            var fontRole = node.TryGetValue("font", out var fontValue) && fontValue is JsonObject fontNode
                ? fontNode.Str("id", "coc:button")
                : node.Str("font", "coc:button");

            var button = new Button(background, "", fontRole);

            if (node.TryGetValue("font", out var sizeValue) && sizeValue is JsonObject sizeNode &&
                sizeNode.TryGetValue("size", out var size))
            {
                button.FontSizeOverride = UIParseUtils.Int(size);
            }

            button.SetLabel(UILocalizedText.Resolve(node, "label"));

            var labelAlign = node.Str("textHorizontalAlign", node.Str("textAlign", ""));

            if (labelAlign.Length > 0)
                button.LabelAlignment = labelAlign.Trim().ToLowerInvariant() switch
                {
                    "left" => TextHorizontalAlignment.Left,
                    "right" => TextHorizontalAlignment.Right,
                    _ => TextHorizontalAlignment.Center
                };

            if (node.TryGetValue("labelPadding", out var labelPadding))
                button.LabelPadding = UIParseUtils.Int(labelPadding);

            button.Label.RichTextLayout.Font = UIFonts.Resolve(node, "coc:button");

            var command = node.Str("command");

            if (command.Length > 0)
                button.Click(() => services.Events.Raise(command));

            if (node.TryGetValue("hint", out var hintValue) && hintValue is JsonObject hintNode)
                button.ApplyHint(hintNode, services);

            if (node.TryGetValue("enabled", out _))
                button.Enabled = node.Bool("enabled", true);

            if (node.TryGetValue("fitLabel", out _))
                button.FitLabelEnabled = node.Bool("fitLabel", true);

            if (node.TryGetValue("disableIf", out var disableIf))
            {
                var disableText = disableIf.ToString().Trim('"');

                if (bool.TryParse(disableText, out var disabledNow))
                    button.Enabled = !disabledNow;
                else
                    button.DisableIf = disableText;
            }

            var backgroundColor = node.Str("backgroundColor");

            if (backgroundColor.Length > 0)
            {
                var tint = UIParseUtils.ColorOf(backgroundColor);

                if (button.Background is not null)
                    button.Background.Color = tint;
                else
                    button.BackgroundColor = tint;
            }

            var textColor = node.Str("textColor");

            if (textColor.Length > 0)
                button.TextColor = UIParseUtils.ColorOf(textColor);

            var iconPath = node.Str("icon");

            if (iconPath.Length > 0)
            {
                var iconSize = node.Int("iconSize", 24);

                button.IconColor = UIParseUtils.ColorOf(node.Str("iconColor", "#35291FFF"));

                button.IconHoverColor = UIParseUtils.ColorOf(node.Str("iconHoverColor", "#8B6B42FF"));

                button.SetIcon(
                    iconPath,
                    iconSize,
                    node.Int("iconX", 20),
                    node.Int("iconY", Math.Max(0, (node.Int("height", 48) - iconSize) / 2)));
            }

            button.RefreshVisual();

            return button;
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<Button>();
    

        public void ApplyHint(JsonObject node, UIBuildContext services)
        {
            var style = new HintStyle
            {
                Padding = HintStyle.Default.Padding,
                HeaderColor = HintStyle.Default.HeaderColor,
                BodyColor = HintStyle.Default.BodyColor,
                SeparatorColor = HintStyle.Default.SeparatorColor
            };

            if (node.TryGetValue("width", out var width))
                style.Width = UIParseUtils.Int(width);

            if (node.TryGetValue("minHeight", out var minHeight))
                style.MinHeight = UIParseUtils.Int(minHeight);

            if (node.TryGetValue("padding", out var padding))
                style.Padding = UIParseUtils.Int(padding);

            if (node.TryGetValue("headerY", out var headerY))
                style.HeaderY = UIParseUtils.Int(headerY);

            if (node.TryGetValue("headerHeight", out var headerHeight))
                style.HeaderHeight = UIParseUtils.Int(headerHeight);

            if (node.TryGetValue("separatorY", out var separatorY))
                style.SeparatorY = UIParseUtils.Int(separatorY);

            if (node.TryGetValue("bodyY", out var bodyY))
                style.BodyY = UIParseUtils.Int(bodyY);

            if (node.TryGetValue("separatorWidth", out var separatorWidth))
                style.SeparatorWidth = UIParseUtils.Int(separatorWidth);

            style.Direction = node.Str("direction", "auto");

            if (node.TryGetValue("distance", out var distance))
                style.Distance = UIParseUtils.Int(distance);

            if (node.TryGetValue("offsetX", out var offsetX))
                style.OffsetX = UIParseUtils.Int(offsetX);

            if (node.TryGetValue("offsetY", out var offsetY))
                style.OffsetY = UIParseUtils.Int(offsetY);

            if (node.TryGetValue("offset", out var offsetValue) && offsetValue is JsonObject offset)
            {
                style.OffsetX = offset.Int("x", style.OffsetX);

                style.OffsetY = offset.Int("y", style.OffsetY);
            }

            style.HeaderFont = node.Str("headerFont", style.HeaderFont);

            style.BodyFont = node.Str("bodyFont", style.BodyFont);

            style.BackgroundTexture = node.Str("backgroundTexture");

            if (node.TryGetValue("backgroundColor", out _))
                style.BackgroundColor = UIParseUtils.ColorOf(node.Str("backgroundColor"));

            if (node.TryGetValue("alpha", out var alpha))
                style.Alpha = UIParseUtils.Float(alpha);

            if (node.TryGetValue("borderColor", out _))
                style.BorderColor = UIParseUtils.ColorOf(node.Str("borderColor"));

            if (node.TryGetValue("borderThickness", out var borderThickness))
                style.BorderThickness = UIParseUtils.Int(borderThickness);

            if (node.TryGetValue("headerColor", out _))
                style.HeaderColor = UIParseUtils.ColorOf(node.Str("headerColor"));

            if (node.TryGetValue("bodyColor", out _))
                style.BodyColor = UIParseUtils.ColorOf(node.Str("bodyColor"));

            if (node.TryGetValue("separatorColor", out _))
                style.SeparatorColor = UIParseUtils.ColorOf(node.Str("separatorColor"));

            var hintText = UILocalizedText.Resolve(node, "text");

            if (hintText.Length > 0)
                this.Hint = hintText;

            var hintHeader = UILocalizedText.Resolve(node, "header");

            if (hintHeader.Length > 0)
                this.HintHeader = hintHeader;

            var disabledHint = UILocalizedText.Resolve(node, "disabledHint");

            if (disabledHint.Length > 0)
                this.DisabledReason = disabledHint;

            this.HintSettings = style;

            var templateId = node.Str("componentTemplate");

            if (templateId.Length == 0 || services.Document is null)
                return;

            if (!services.Document.ComponentTemplates.TryGetValue(templateId, out var template))
            {
                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] Unknown hint component template '{templateId}'");

                return;
            }

            var root = services.BuildTemplate(template);

            if (root is not null)
                this.HintView = new HintView(root, style);
        }
    
}
}
