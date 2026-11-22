using Microsoft.Xna.Framework;
using MonoCoC.UI.Common;
using MonoCoC.UI.Framework;
using System;

namespace MonoCoC.UI.Common
{
    // 滚动条显隐动画：平时隐藏，鼠标靠近右边缘（或正在拖动）时渐显，离开后渐隐。
    // 每个自己画滚动条的组件持有一个实例。
    public sealed class ScrollBarFade
    {
        private float _alpha;

        private float _flashRemaining;

        // 拖动中 / 强制显示（例如正在滚轮滚动）。
        public bool Pinned;

        public float Alpha => _alpha;

        public bool Visible => _alpha > 0.01f;

        // 滚轮滚动时"脉冲"显示一小段时间，避免鼠标不在右边缘时看不出滚动位置。
        public void Flash(float seconds = 0.7f) => _flashRemaining = Math.Max(_flashRemaining, seconds);

        public void Update(GameTime gameTime, Rectangle area, bool needed)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_flashRemaining > 0)
                _flashRemaining = Math.Max(0, _flashRemaining - delta);

            var reveal = needed && (Pinned || _flashRemaining > 0 || NearRightEdge(area));

            var speed = reveal
                ? 8f
                : 4f;

            _alpha = reveal
                ? Math.Min(1f, _alpha + speed * delta)
                : Math.Max(0f, _alpha - speed * delta);
        }

        // 只改 alpha，不缩放 RGB：UI 用的是 NonPremultiplied 混合，
        // 乘色会让滑块在淡入过程中先变深/发黑再消失。
        public Color Fade(Color color) => new(color.R, color.G, color.B, (byte)(color.A * Math.Clamp(_alpha, 0f, 1f) * 255f / 255f));

        // 鼠标在右边缘一条竖带里（含滚动条左右各 revealDistance）。
        private static bool NearRightEdge(Rectangle area)
        {
            var mouse = UserInput.GetMousePos();

            var distance = Math.Max(8, 28);

            return mouse.Y >= area.Y && mouse.Y <= area.Bottom &&
                   mouse.X >= area.Right - distance && mouse.X <= area.Right + distance;
        }
    }
}
