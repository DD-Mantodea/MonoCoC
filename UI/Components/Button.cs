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

namespace MonoCoC.UI.Components
{
    public class Button : Component, IUIStatefulPart
    {
        private sealed record Snapshot(
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

        public Color TextColor = Color.Black;

        private int _fittedWidth = -1;

        private int _fittedPadding = -1;

        private string _fittedText = "";

        private int _fittedRevision = -1;

        private void FitLabel()
        {
            var text = Label.RichTextLayout.Text ?? "";

            var width = Width;

            if (width <= 0 || (width == _fittedWidth && text == _fittedText && LabelPadding == _fittedPadding && UIFonts.Revision == _fittedRevision))
                return;

            _fittedWidth = width;

            _fittedText = text;

            _fittedPadding = LabelPadding;

            _fittedRevision = UIFonts.Revision;

            var size = UIFonts.Size(FontRole);

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

            Label.WrapWidth(width);

            Label.LayoutData.Size = new Vector2(width, Label.Height);

            Label.Position(0, labelY);
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
                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] disableIf error: {error}");

                return;
            }

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

                return InputResult.Continue;
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

                UIManager.Instance?.ToolTip.Hide();

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

            HintSettings = null;

            HintView = null;

            return RefreshVisual();
        }

        public Button RefreshVisual()
        {
            Background?.LayoutData.Alpha = !Enabled ? 0.4f : IsHovering ? 0.5f : 1f;

            var stateText = StateStyle?.TextFor(Enabled, IsHovering, IsPressed);

            Label?.TextColor = Enabled ? stateText ?? TextColor : UIColors.ButtonTextDisabled;

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

        public Button Click(Action onClick)
        {
            OnClick = onClick;

            return this;
        }

        /// <summary>
        /// Applies a control template's visual tree. The template's
        /// "content" part receives the button label; all other parts are
        /// ordinary components.
        /// </summary>
        public void ApplyControlTemplate(Component root)
        {
            if (Background is not null)
                Background.Visible = false;

            root.LayoutData.LayoutType = LayoutType.Relative;

            root.LayoutData.Position = Vector2.Zero;

            root.Of(this);
        }
    }
}
