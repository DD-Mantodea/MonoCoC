using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Animations
{
    public abstract class UIAnimation
    {
        public float Delay;

        public bool Loop;

        public bool PingPong;

        public Func<float, float> Easing = Animations.Easing.Linear;

        public bool IsFinished { get; protected set; }

        protected float Elapsed;

        protected bool Reversed;

        public virtual void Reset()
        {
            Elapsed = 0f;
            IsFinished = false;
            Reversed = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (IsFinished)
                return;

            if (Delay > 0f)
            {
                Delay -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                return;
            }

            Elapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var progress = Duration <= 0f ? 1f : Math.Clamp(Elapsed / Duration, 0f, 1f);

            if (PingPong)
            {
                if (progress >= 1f)
                {
                    Reversed = !Reversed;
                    Elapsed = 0f;
                    progress = Reversed ? 1f : 0f;
                }
            }

            Apply(Easing(progress));

            if (progress >= 1f)
            {
                if (Loop)
                {
                    Elapsed = 0f;

                    return;
                }

                IsFinished = true;
                Complete();
            }
        }

        public float Duration { get; set; } = 0.25f;

        protected virtual void Apply(float progress)
        {
        }

        protected virtual void Complete()
        {
        }
    }

    public sealed class FloatAnimation : UIAnimation
    {
        private readonly Action<float> _setter;

        public float From;

        public float To;

        public FloatAnimation(float from, float to, Action<float> setter, float duration = 0.25f)
        {
            From = from;
            To = to;
            _setter = setter;
            Duration = duration;
        }

        protected override void Apply(float progress) =>
            _setter(From + (To - From) * progress);
    }

    public sealed class Vector2Animation : UIAnimation
    {
        private readonly Action<Vector2> _setter;

        public Vector2 From;

        public Vector2 To;

        public Vector2Animation(Vector2 from, Vector2 to, Action<Vector2> setter, float duration = 0.25f)
        {
            From = from;
            To = to;
            _setter = setter;
            Duration = duration;
        }

        protected override void Apply(float progress) =>
            _setter(Vector2.Lerp(From, To, progress));
    }

    public sealed class ColorAnimation : UIAnimation
    {
        private readonly Action<Color> _setter;

        public Color From;

        public Color To;

        public ColorAnimation(Color from, Color to, Action<Color> setter, float duration = 0.25f)
        {
            From = from;
            To = to;
            _setter = setter;
            Duration = duration;
        }

        protected override void Apply(float progress) =>
            _setter(Color.Lerp(From, To, progress));
    }

    public sealed class AnimationPlayer
    {
        private readonly List<UIAnimation> _animations = [];

        public void Play(UIAnimation animation)
        {
            animation.Reset();
            _animations.Add(animation);
        }

        public void StopAll() => _animations.Clear();

        public void Update(GameTime gameTime)
        {
            for (var index = _animations.Count - 1; index >= 0; index--)
            {
                var animation = _animations[index];

                animation.Update(gameTime);

                if (animation.IsFinished)
                    _animations.RemoveAt(index);
            }
        }
    }
}
