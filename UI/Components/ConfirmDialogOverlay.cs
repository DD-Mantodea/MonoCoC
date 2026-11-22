using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components
{
    public sealed record ConfirmDialogOptions(
        string Title,
        string Message,
        string ConfirmText = "Yes",
        string CancelText = "No",
        int TitleFontSize = 34,
        int ButtonFontSize = 24,
        int BoxWidth = 640,
        int BoxHeight = 300);

    public sealed class ConfirmDialogOverlay : Container
    {
        private static ConfirmDialogOverlay? _active;

        private Action? _confirm;

        private Action? _cancel;

        private Component? _previousBlock;

        private Component? _previousFocus;

        private ConfirmDialogOverlay()
        {
            CanHit = true;

            LayoutData.LayoutType = LayoutType.Absolute;

            LayoutData.Size = new Vector2(LayoutConstants.ScreenWidth, LayoutConstants.ScreenHeight);
        }

        public static void Show(string title, string message, Action confirm, Action? cancel = null) =>
            Show(new ConfirmDialogOptions(title, message), confirm, cancel);

        public static void Show(ConfirmDialogOptions options, Action confirm, Action? cancel = null)
        {
            var root = UIManager.Instance?.Root;

            if (root is null)
                return;

            _active?.Close(false);

            var overlay = new ConfirmDialogOverlay
            {
                _confirm = confirm,
                _cancel = cancel,
                _previousBlock = UIManager.Instance?.InputBlock,
                _previousFocus = UIManager.Instance?.FocusedComponent
            };

            overlay.Build(options);

            overlay.Of(root);

            if (UIManager.Instance is not null)
                UIManager.Instance.InputBlock = overlay;

            _active = overlay;
        }

        private void Build(ConfirmDialogOptions options)
        {
            var dim = new Container
            {
                CanHit = true,
                BackgroundColor = Color.Black * 0.55f,
                LayoutData = { LayoutType = LayoutType.Relative }
            }.Size(LayoutConstants.ScreenWidth, LayoutConstants.ScreenHeight).Of(this);

            var box = new Container
            {
                CanHit = true,
                BackgroundColor = ParseColor("#F2E5C7FF"),
                LayoutData = { LayoutType = LayoutType.Absolute }
            }.Size(options.BoxWidth, options.BoxHeight)
                .Position(
                    (LayoutConstants.ScreenWidth - options.BoxWidth) / 2,
                    (LayoutConstants.ScreenHeight - options.BoxHeight) / 2)
                .Of(this);

            var contentWidth = options.BoxWidth - 48;

            var titleLabel = new Label(UIFonts.GetAt("coc:menu", options.TitleFontSize))
                .Text(options.Title)
                .Position(24, 24)
                .Size(contentWidth, 48)
                .LayoutType(LayoutType.Relative)
                .CanHit(false)
                .Of(box);

            titleLabel.TextColor = ParseColor("#35291FFF");

            var bodyLabel = new Label(UIFonts.Get("coc:body"))
                .Text(Scene.Parser.Parse(options.Message))
                .Position(24, 84)
                .Size(contentWidth, options.BoxHeight - 180)
                .WrapWidth(contentWidth)
                .LayoutType(LayoutType.Relative)
                .CanHit(false)
                .Of(box);

            bodyLabel.TextColor = ParseColor("#35291FFF");

            var buttonY = options.BoxHeight - 64;

            var buttonWidth = (contentWidth - 20) / 2;

            var yes = new Button("", options.ConfirmText, "coc:menu")
                .Position(24, buttonY)
                .Size(buttonWidth, 44)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            yes.FontSizeOverride = options.ButtonFontSize;

            yes.TextColor = ParseColor("#7C6B58CC");

            yes.Click(() => Close(true));

            var no = new Button("", options.CancelText, "coc:menu")
                .Position(44 + buttonWidth, buttonY)
                .Size(buttonWidth, 44)
                .LayoutType(LayoutType.Relative)
                .Of(box);

            no.FontSizeOverride = options.ButtonFontSize;

            no.TextColor = ParseColor("#7C6B58CC");

            no.Click(() => Close(false));
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (args.Type == InputType.KeyDown && args.Key == Keys.Escape)
            {
                Close(false);

                return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        private void Close(bool confirmed)
        {
            var callback = confirmed ? _confirm : _cancel;

            _confirm = null;

            _cancel = null;

            if (LayoutData.Parent is { } parent)
                parent.RemoveChild(this);

            if (UIManager.Instance is not null)
            {
                if (ReferenceEquals(UIManager.Instance.InputBlock, this))
                    UIManager.Instance.InputBlock = _previousBlock;

                if (ReferenceEquals(UIManager.Instance.FocusedComponent, this))
                    UIManager.Instance.FocusedComponent = _previousFocus;
            }

            if (ReferenceEquals(_active, this))
                _active = null;

            callback?.Invoke();
        }

        private static Color ParseColor(string value) => UIParseUtils.ColorOf(value);
    }
}
