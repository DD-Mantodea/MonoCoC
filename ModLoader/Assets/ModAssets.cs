using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Per-mod asset repository. Every created resource is disposed on unload.
    /// </summary>
    public sealed class ModAssets : IDisposable
    {
        private readonly ModFiles _files;
        private readonly Dictionary<string, Texture2D> _textures = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, SoundEffect> _sounds = new(StringComparer.OrdinalIgnoreCase);
        private bool _disposed;

        internal ModAssets(ModFiles files)
        {
            _files = files;
        }

        public string ModId => _files.ModId;

        public bool HasAsset(string path) => Resolve(path) is not null;

        public T Request<T>(string path) where T : class
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ModAssets));

            var resolved = Resolve(path)
                ?? throw new FileNotFoundException($"{ModId}: asset not found: {path}");

            if (typeof(T) == typeof(Texture2D))
                return (T)(object)LoadTexture(resolved);

            if (typeof(T) == typeof(SoundEffect))
                return (T)(object)LoadSound(resolved);

            throw new NotSupportedException($"{ModId}: unsupported asset type {typeof(T).Name}");
        }

        public bool RequestIfExists<T>(string path, out T? asset) where T : class
        {
            if (!HasAsset(path))
            {
                asset = null;
                return false;
            }

            asset = Request<T>(path);
            return true;
        }

        private Texture2D LoadTexture(string path)
        {
            if (_textures.TryGetValue(path, out var texture))
                return texture;

            using var stream = _files.OpenRead(path);

            texture = Texture2D.FromStream(CoC.Instance.GraphicsDevice, stream);

            _textures[path] = texture;

            return texture;
        }

        private SoundEffect LoadSound(string path)
        {
            if (_sounds.TryGetValue(path, out var sound))
                return sound;

            if (path.EndsWith(".ogg", StringComparison.OrdinalIgnoreCase))
                throw new NotSupportedException($"{ModId}: OGG playback is not implemented yet: {path}");

            using var stream = _files.OpenRead(path);

            sound = SoundEffect.FromStream(stream);

            _sounds[path] = sound;

            return sound;
        }

        private string? Resolve(string path)
        {
            var normalized = path.Replace('\\', '/').TrimStart('/');

            if (_files.Exists(normalized))
                return normalized;

            foreach (var extension in new[] { ".png", ".wav" })
                if (_files.Exists(normalized + extension))
                    return normalized + extension;

            return null;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            foreach (var texture in _textures.Values)
                texture.Dispose();

            foreach (var sound in _sounds.Values)
                sound.Dispose();

            _textures.Clear();
            _sounds.Clear();
        }
    }
}
