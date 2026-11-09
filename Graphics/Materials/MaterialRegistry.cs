using Hjson;
using Microsoft.Xna.Framework;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MonoCoC.Graphics.Materials
{
    /// <summary>
    /// Registered material resources for Hjson nodes:
    ///   material: "coc:paper"
    ///   materialConfig: { ... }
    /// </summary>
    public sealed class MaterialRegistry
    {
        private readonly Dictionary<string, Func<JsonObject?, Material?>> _materials =
            new(StringComparer.OrdinalIgnoreCase);

        public static MaterialRegistry Instance { get; } = new();

        static MaterialRegistry()
        {
            Instance.Register("coc:paper", CreatePaper);
        }

        public void Register(string id, Func<JsonObject?, Material?> factory)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Material id is required", nameof(id));

            _materials[id] = factory;
        }

        public bool TryCreate(string id, JsonObject? config, out Material? material)
        {
            if (_materials.TryGetValue(id, out var factory))
            {
                material = factory(config);

                if (material is null)
                {
                    UI.Console.ConsoleOverlay.WriteLine(
                        $"[Material] {id} unavailable: {ShaderManager.Instance.LastError ?? "no effect"}");
                }

                return true;
            }

            UI.Console.ConsoleOverlay.WriteLine($"[Material] Unknown material '{id}'");

            material = null;
            return false;
        }

        private static Material? CreatePaper(JsonObject? config)
        {
            var effect = ShaderManager.Instance.Load("Paper.fx");

            if (effect is null)
                return null;

            var settings = PaperSettings.Default;

            if (config is not null)
            {
                var preset = config.Str("preset").Trim().ToLowerInvariant();

                settings = preset switch
                {
                    "clean" => PaperSettings.Clean,
                    "aged" => PaperSettings.Aged,
                    _ => settings
                };

                if (config.TryGetValue("centerColor", out _))
                    settings.CenterColor = ColorOf(config.Str("centerColor"));

                if (config.TryGetValue("edgeColor", out _))
                    settings.EdgeColor = ColorOf(config.Str("edgeColor"));

                if (config.TryGetValue("edgeStrength", out var edge))
                    settings.EdgeStrength = Float(edge);

                if (config.TryGetValue("lowNoiseStrength", out var noise))
                    settings.LowNoiseStrength = Float(noise);

                if (config.TryGetValue("fiberStrength", out var fiber))
                    settings.FiberStrength = Float(fiber);
            }

            return new PaperMaterial(CoC.Instance.GraphicsDevice, effect)
            {
                Settings = settings
            };
        }

        private static float Float(object value) =>
            float.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                ? result
                : 0f;

        private static Color ColorOf(string value)
        {
            value = value.Trim();

            if (value.Length == 0 || value[0] != '#')
                return Color.Transparent;

            var hex = value[1..];

            if (hex.Length == 6)
                hex += "FF";

            if (hex.Length != 8 ||
                !uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgba))
                return Color.Transparent;

            return new Color(
                (byte)((rgba >> 24) & 0xFF),
                (byte)((rgba >> 16) & 0xFF),
                (byte)((rgba >> 8) & 0xFF),
                (byte)(rgba & 0xFF));
        }
    }
}
