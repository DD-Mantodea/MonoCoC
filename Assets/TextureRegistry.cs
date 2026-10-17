using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MonoCoC.Assets
{
    /// <summary>
    /// Resolves logical texture IDs:
    ///   coc:&lt;path&gt;       vanilla textures
    ///   &lt;modId&gt;:&lt;path&gt;   a loaded mod's textures
    ///
    /// Providers are registered per namespace. Mod providers must be removed
    /// before ModAssets disposes its textures.
    /// </summary>
    public sealed class TextureRegistry
    {
        private sealed class Provider
        {
            public required Func<string, Texture2D?> Resolve { get; init; }
        }

        private readonly Dictionary<string, Provider> _providers = new(StringComparer.OrdinalIgnoreCase);

        public static TextureRegistry Instance { get; } = new();

        public void RegisterProvider(string namespaceId, Func<string, Texture2D?> provider)
        {
            if (string.IsNullOrWhiteSpace(namespaceId))
                throw new ArgumentException("Texture namespace is required", nameof(namespaceId));

            if (namespaceId.Equals("coc", StringComparison.OrdinalIgnoreCase) && _providers.ContainsKey("coc"))
                throw new InvalidOperationException("The 'coc' texture namespace is reserved for vanilla assets");

            _providers[namespaceId] = new Provider { Resolve = provider };
        }

        public void Unregister(string namespaceId) => _providers.Remove(namespaceId);

        public Texture2D? Resolve(string? logicalId)
        {
            if (string.IsNullOrWhiteSpace(logicalId))
                return null;

            var separator = logicalId.IndexOf(':');

            if (separator < 0)
                return ResolvePath("coc", logicalId);

            if (separator == 0 || separator == logicalId.Length - 1)
                return null;

            var namespaceId = logicalId[..separator];
            var path = logicalId[(separator + 1)..].Replace('\\', '/').Trim('/');

            if (path.Length == 0 || path.Contains("..", StringComparison.Ordinal) || path.StartsWith("/", StringComparison.Ordinal))
                return null;

            return ResolvePath(namespaceId, path);
        }

        private Texture2D? ResolvePath(string namespaceId, string path) =>
            _providers.TryGetValue(namespaceId, out var provider) ? provider.Resolve(path) : null;
    }
}
