using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Shaders;
using MonoCoC.UI.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MonoCoC.Graphics.Materials
{
    /// <summary>
    /// Base for full-screen procedural materials. Owns the reusable
    /// white-pixel quad, SpriteBatch setup, Effect parameter cache and
    /// hot-reload rebinding. The Effect itself is externally owned.
    /// </summary>
    public abstract class Material(GraphicsDevice graphicsDevice, Effect effect) : IDisposable
    {
        private readonly Dictionary<string, EffectParameter?> _parameters = new(StringComparer.Ordinal);

        private string? _shaderName;

        private bool _disposed;

        protected GraphicsDevice GraphicsDevice { get; } = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));

        protected Effect Effect { get; private set; } = effect ?? throw new ArgumentNullException(nameof(effect));

        /// <summary>
        /// Replace the effect after a shader hot reload. Parameters are
        /// re-resolved lazily against the new effect.
        /// </summary>
        public void SetEffect(Effect effect)
        {
            Effect = effect ?? throw new ArgumentNullException(nameof(effect));

            _parameters.Clear();

            OnEffectChanged();
        }

        protected virtual void OnEffectChanged()
        {
        }

        protected void ListenShader(string shaderName)
        {
            _shaderName = shaderName;

            ShaderManager.Instance.EffectReloaded += OnShaderReloaded;
        }

        private void OnShaderReloaded(string name, Effect effect)
        {
            if (string.Equals(name, _shaderName, StringComparison.OrdinalIgnoreCase))
                SetEffect(effect);
        }

        protected EffectParameter? Parameter(string name)
        {
            if (_parameters.TryGetValue(name, out var parameter))
                return parameter;

            parameter = Effect.Parameters[name];

            if (parameter is null)
            {
                ConsoleOverlay.WriteLine(
                    $"[Material] {_shaderName ?? "shader"} is missing parameter '{name}'");

                ConsoleOverlay.WriteLine(new StackTrace(true).ToString());
            }

            _parameters[name] = parameter;

            return parameter;
        }

        protected abstract void ApplyParameters(Rectangle bounds);

        /// <summary>
        /// Draws a rectangle with this material. Must be called outside an
        /// active SpriteBatch.Begin/End block.
        /// </summary>
        public virtual void Draw(SpriteBatch spriteBatch, Rectangle bounds, Color? tint = null)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            ApplyParameters(bounds);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque,
                SamplerState.LinearClamp,
                DepthStencilState.None,
                RasterizerState.CullNone,
                Effect);

            OnSpriteBatchBegun(spriteBatch, bounds);

            spriteBatch.Draw(SpriteBatchExt.Pixel, bounds, tint ?? Color.White);

            spriteBatch.End();
        }

        protected virtual void OnSpriteBatchBegun(SpriteBatch spriteBatch, Rectangle bounds)
        {
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _parameters.Clear();

            if (_shaderName is not null)
                ShaderManager.Instance.EffectReloaded -= OnShaderReloaded;
        }
    }
}
