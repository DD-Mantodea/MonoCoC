using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using SkiaSharp;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MonoCoC.UI.Components
{
    public static class SvgTextureCache
    {
        private static readonly Dictionary<string, Texture2D> Cache = [];

        private static readonly Dictionary<string, string> Files = [];

        public static Texture2D? GetFile(GraphicsDevice device, string relativePath, int size, SKColor color)
        {
            if (!Files.TryGetValue(relativePath, out var svgText))
            {
                var path = Path.Combine(Paths.AssetPath("Textures"), relativePath);

                if (!File.Exists(path))
                    return null;

                svgText = File.ReadAllText(path);
                Files[relativePath] = svgText;
            }

            return Get(device, svgText, size, color);
        }

        public static Texture2D? Get(GraphicsDevice device, string svgText, int size, SKColor color)
        {
            if (string.IsNullOrWhiteSpace(svgText) || size <= 0)
                return null;

            var key = $"{svgText.GetHashCode()}@{size}@{color}";

            if (Cache.TryGetValue(key, out var hit))
                return hit;

            try
            {
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(svgText));

                var svg = new SKSvg();

                var picture = svg.Load(stream);

                if (picture is null)
                    return null;

                using var bitmap = new SKBitmap(size, size);

                using var canvas = new SKCanvas(bitmap);

                canvas.Clear(SKColors.Transparent);

                var bounds = picture.CullRect;

                var scale = size / Math.Max(1f, Math.Max(bounds.Width, bounds.Height));

                canvas.Save();
                canvas.Scale(scale);
                canvas.Translate(-bounds.Left, -bounds.Top);

                var paint = new SKPaint
                {
                    ColorFilter = SKColorFilter.CreateBlendMode(color, SKBlendMode.SrcIn)
                };

                canvas.DrawPicture(picture, paint);
                canvas.Restore();
                canvas.Flush();

                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);
                using var png = data.AsStream();

                var texture = Texture2D.FromStream(device, png);

                Cache[key] = texture;

                return texture;
            }
            catch (Exception ex)
            {
                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[SVG] render failed: {ex.Message}");

                return null;
            }
        }
    }
}
