using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Graphics.Materials;
using MonoCoC.Graphics.Shaders;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    /// <summary>
    /// Optional parchment material component. The base Component handles the
    /// material drawing; if the shader is unavailable, BackgroundColor is the
    /// normal fallback.
    /// </summary>
    public sealed class PaperBackground : Component
    {
        private PaperMaterial? _paperMaterial;

        private PaperSettings _settings = PaperSettings.Default;

        public PaperBackground()
        {
            CanHit = false;

            BackgroundColor = _settings.CenterColor;

            ShaderManager.Instance.EffectReloaded += OnEffectReloaded;

            BindEffect();
        }

        public PaperSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;

                BackgroundColor = value.CenterColor;

                if (_paperMaterial is not null)
                    _paperMaterial.Settings = value;
            }
        }

        private void OnEffectReloaded(string name, Effect effect)
        {
            if (!string.Equals(name, "Paper.fx", System.StringComparison.OrdinalIgnoreCase))
                return;

            if (_paperMaterial is null)
            {
                _paperMaterial = new PaperMaterial(CoC.Instance.GraphicsDevice, effect)
                {
                    Settings = Settings
                };

                Material = _paperMaterial;
            }
            else
                _paperMaterial.SetEffect(effect);
        }

        private void BindEffect()
        {
            var effect = ShaderManager.Instance.Load("Paper.fx");

            if (effect is null)
                return;

            _paperMaterial = new PaperMaterial(CoC.Instance.GraphicsDevice, effect)
            {
                Settings = Settings
            };

            Material = _paperMaterial;
        }
    }
}
