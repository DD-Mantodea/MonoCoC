using System;

namespace MonoCoC.UI.Animations
{
    public static class Easing
    {
        public static float Linear(float t) => t;

        public static float EaseInQuad(float t) => t * t;

        public static float EaseOutQuad(float t) => t * (2f - t);

        public static float EaseInOutQuad(float t) =>
            t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;

        public static float EaseOutCubic(float t) => 1f - (1f - t) * (1f - t) * (1f - t);

        public static float EaseOutBack(float t)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;

            return 1f + c3 * (t - 1f) * (t - 1f) * (t - 1f) + c1 * (t - 1f) * (t - 1f);
        }

        public static float SmoothStep(float t) => t * t * (3f - 2f * t);

        public static Func<float, float> FromName(string? name) => name switch
        {
            "easeInQuad" => EaseInQuad,
            "easeOutQuad" => EaseOutQuad,
            "easeInOutQuad" => EaseInOutQuad,
            "easeOutCubic" => EaseOutCubic,
            "easeOutBack" => EaseOutBack,
            "smoothStep" => SmoothStep,
            _ => Linear
        };
    }
}
