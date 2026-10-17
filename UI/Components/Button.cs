using FontStashSharp.RichText;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Extensions;
using MonoCoC.UI.Common;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components
{
    public class Button : Component
    {
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            FitLabel();
        }

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

            Label.Position(width / 2, labelY);
        }

        public UIImage Background;

        public Label Label;

        public new Action OnClick;

        public Action? OnMouseDown;

        public Action<Vector2>? OnMouseMove;

        public Action<Vector2>? OnMouseUp;

        public string Hint = "";

        public string HintHeader = "";

        public override string ToolTipText => Enabled ? Hint : (string.IsNullOrEmpty(DisabledReason) ? Hint : DisabledReason);

        public override string ToolTipHeader => string.IsNullOrEmpty(HintHeader) ? Label.RichTextLayout.Text : HintHeader;

        public string DisabledReason = "";

        public override InputResult OnInput(InputEventArgs args)
        {
            if (!Visible || !Enabled)
                return InputResult.Continue;

            if (args.Type == InputType.MouseLeftDown)
            {
                OnMouseDown?.Invoke();

                return InputResult.Continue;
            }

            if (args.Type == InputType.MouseMove && OnMouseMove != null)
            {
                OnMouseMove(args.MousePosition);

                return InputResult.Continue;
            }

            if (args.Type == InputType.MouseLeftUp && OnMouseUp != null)
                OnMouseUp(args.MousePosition);

            if (args.Type == InputType.MouseLeftClick)
            {
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

            Hint = "";

            HintHeader = "";

            return RefreshVisual();
        }

        public Button RefreshVisual()
        {
            Background?.LayoutData.Alpha = !Enabled ? 0.4f : IsHovering ? 0.5f : 1f;

            Label?.TextColor = Enabled ? TextColor : UIColors.ButtonTextDisabled;

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
    }
}
