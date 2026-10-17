using System;
using System.Collections.Generic;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Allows a mod (or a content manager adapter) to consume Hjson files
    /// from the mod package without knowing whether they come from a
    /// directory or a .cocmod container.
    /// </summary>
    public sealed class ModDataLoaderRegistry : IDisposable
    {
        private readonly List<(string Folder, string Pattern, Action<IDataFileSource, string> Loader)> _loaders = [];
        private bool _disposed;

        public void Register(string folder, string pattern, Action<IDataFileSource, string> loader)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ModDataLoaderRegistry));

            _loaders.Add((folder, pattern, loader));
        }

        internal void LoadAll(IDataFileSource source)
        {
            foreach (var (folder, pattern, loader) in _loaders)
                foreach (var path in source.Enumerate(folder, pattern))
                    loader(source, path);
        }

        public void Dispose()
        {
            _disposed = true;
            _loaders.Clear();
        }
    }
}
