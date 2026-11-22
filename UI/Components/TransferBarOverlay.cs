using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;

namespace MonoCoC.UI.Components
{
    public sealed class TransferBarOverlay : HjsonView
    {
        private static TransferBarOverlay? _active;

        private const int TargetY = 760;

        private const float OpenDuration = 0.28f;

        private const float CloseDuration = 0.22f;

        private int _maximum = 1;

        private Slider _slider = null!;

        private Label _title = null!;

        private Label _valueLabel = null!;

        private Container _panel = null!;

        private Action<int>? _confirm;

        private Component? _previousBlock;

        private bool _closing;

        private bool _confirmed;

        private float _animationTime;

        private float _animationFrom;

        private float _animationTo;

        private float _panelY;

        public TransferBarOverlay()
        {
            Initialize();
        }

        protected override string ScreenFile => "Views\\TransferBarView.hjson";

        public static void Show(string title, int maximum, Action<int> confirm)
        {
            var root = UIManager.Instance?.Root;

            if (root is null || maximum < 1)
                return;

            _active?.CloseImmediate();

            var overlay = new TransferBarOverlay
            {
                _confirm = confirm,
                _maximum = maximum
            };

            overlay.Of(root);

            overlay._previousBlock = UIManager.Instance!.InputBlock;

            UIManager.Instance.InputBlock = overlay;

            _active = overlay;

            overlay.Configure(title);

            overlay.BeginOpen();
        }

        [UIRequired("transfer.panel")]
        private void BindPanel(Container part) => _panel = part;

        [UIRequired("transfer.title")]
        private void BindTitle(Label part) => _title = part;

        [UIRequired("transfer.value")]
        private void BindValue(Label part) => _valueLabel = part;

        [UIRequired("transfer.slider")]
        private void BindSlider(Slider part) => _slider = part;

        private void Configure(string title)
        {
            _title.Text(title);

            _slider.Min = 1;

            _slider.Max = _maximum;

            _slider.Step = 1;

            _slider.Value = 1;

            _slider.ValueChanged = value =>
            {
                _valueLabel.Text((int)Math.Round(value) + " / " + _maximum);
            };

            _valueLabel.Text("1 / " + _maximum);
        }

        private void BeginOpen()
        {
            _panelY = LayoutConstants.ScreenHeight;

            _animationFrom = _panelY;

            _animationTo = TargetY;

            _animationTime = 0;

            _closing = false;

            UpdatePanelPosition();
        }

        public override void Update(GameTime gameTime)
        {
            if (_animationTime >= 0)
            {
                _animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                var duration = _closing ? CloseDuration : OpenDuration;

                var progress = Math.Clamp(_animationTime / duration, 0f, 1f);

                var eased = _closing
                    ? progress * progress * progress
                    : 1f - MathF.Pow(1f - progress, 3f);

                _panelY = _animationFrom + (_animationTo - _animationFrom) * eased;

                UpdatePanelPosition();

                if (progress >= 1f)
                {
                    _animationTime = -1;

                    if (_closing)
                        FinalizeClose();
                }
            }

            base.Update(gameTime);
        }

        private void UpdatePanelPosition()
        {
            _panel.LayoutData.Position = new Vector2(500, _panelY);
        }

        public override InputResult OnInput(InputEventArgs args)
        {
            if (_closing)
                return InputResult.Consumed;

            if (args.Type == InputType.KeyDown && args.Key == Keys.Escape)
            {
                Cancel();

                return InputResult.Consumed;
            }

            if (args.Type == InputType.MouseLeftDown && !_panel.Rectangle.Contains(args.MousePosition))
            {
                Cancel();

                return InputResult.Consumed;
            }

            return base.OnInput(args);
        }

        [UIEvent("transfer.confirm")]
        private void Confirm()
        {
            if (_closing)
                return;

            _confirmed = true;

            BeginClose();
        }

        [UIEvent("transfer.cancel")]
        private void Cancel()
        {
            if (_closing)
                return;

            _confirmed = false;

            BeginClose();
        }

        private void BeginClose()
        {
            if (_closing)
                return;

            _closing = true;

            _animationTime = 0;

            _animationFrom = _panelY;

            _animationTo = LayoutConstants.ScreenHeight;

            if (!_confirmed)
                _confirm = null;
        }

        private void CloseImmediate()
        {
            if (LayoutData.Parent is { } parent)
                parent.RemoveChild(this);

            if (UIManager.Instance is not null && ReferenceEquals(UIManager.Instance.InputBlock, this))
                UIManager.Instance.InputBlock = _previousBlock;

            if (ReferenceEquals(_active, this))
                _active = null;
        }

        private void FinalizeClose()
        {
            var callback = _confirmed ? _confirm : null;

            var value = (int)Math.Round(_slider.Value);

            _confirm = null;

            CloseImmediate();

            callback?.Invoke(value);
        }
    }
}
