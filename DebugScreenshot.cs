using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using System;
using System.IO;
using System.Linq;

namespace MonoCoC
{
    public static class DebugScreenshot
    {
        private static bool _requested;

        public static string DirectoryPath => Path.Combine(Paths.Root, "Tasks", "ui-captures");

        public static void Request() => _requested = true;

        public static void TryCapture(GraphicsDevice device)
        {
            if (!_requested)
                return;

            _requested = false;

            try
            {
                var width = device.PresentationParameters.BackBufferWidth;

                var height = device.PresentationParameters.BackBufferHeight;

                var data = new Color[width * height];

                device.GetBackBufferData(data);

                using var texture = new Texture2D(device, width, height);

                texture.SetData(data);

                System.IO.Directory.CreateDirectory(DirectoryPath);

                var path = Path.Combine(
                    DirectoryPath,
                    $"ui-{DateTime.Now:yyyyMMdd-HHmmss-fff}.png");

                using var stream = File.Create(path);

                texture.SaveAsPng(stream, width, height);

                Cleanup();
            }
            catch (Exception exception)
            {
                UI.Console.ConsoleOverlay.WriteError($"[DebugScreenshot] {exception.Message}");
            }
        }

        private static void Cleanup()
        {
            var files = new DirectoryInfo(DirectoryPath)
                .GetFiles("ui-*.png")
                .OrderByDescending(file => file.CreationTimeUtc)
                .Skip(20)
                .ToArray();

            foreach (var file in files)
                file.Delete();
        }
    }
}
