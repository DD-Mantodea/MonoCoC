using System;
using System.Threading;

namespace MonoCoC.ModLoader
{
    public sealed class ModContext : IDisposable
    {
        private static readonly AsyncLocal<ModContext?> CurrentContext = new();

        private bool _disposed;

        public static ModContext? Current => CurrentContext.Value;

        public ModManifest Manifest { get; }

        public CocModPackage Package { get; }

        public ModLoadContext? LoadContext { get; internal set; }

        public ModFiles Files { get; }

        public ModDataFiles Data { get; }

        public ModDataLoaderRegistry DataLoaders { get; }

        public IDataFileSource DataSource { get; }

        public ModAssets Assets { get; }

        public ModHookScope Hooks { get; }

        public ModContentRegistry Content { get; }

        public Mod? Instance { get; internal set; }

        public bool IsLoaded { get; internal set; }

        internal ModContext(
            ModManifest manifest,
            CocModPackage package,
            ModLoadContext loadContext,
            ModContentRegistry content)
        {
            Manifest = manifest;
            Package = package;
            LoadContext = loadContext;
            Content = content;
            Files = new ModFiles(manifest.Id, package);
            Data = new ModDataFiles(Files);
            DataLoaders = new ModDataLoaderRegistry();
            DataSource = new PackageDataFileSource(Files, "");
            Assets = new ModAssets(Files);
            Hooks = new ModHookScope(manifest.Id);
        }

        public IDisposable Enter()
        {
            var previous = CurrentContext.Value;

            CurrentContext.Value = this;

            return new Scope(this, previous);
        }

        public bool TryGetMod(string id, out Mod? mod) => ModHost.Instance.TryGetMod(id, out mod);

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            Hooks.Dispose();
            Content.Dispose();
            DataLoaders.Dispose();
            Assets.Dispose();

            if (Instance is not null)
                Instance.Close();

            Instance = null;
            Package.Dispose();
            LoadContext = null;
        }

        private sealed class Scope : IDisposable
        {
            private readonly ModContext _context;
            private readonly ModContext? _previous;
            private bool _disposed;

            public Scope(ModContext context, ModContext? previous)
            {
                _context = context;
                _previous = previous;
            }

            public void Dispose()
            {
                if (_disposed)
                    return;

                _disposed = true;

                if (CurrentContext.Value == _context)
                    CurrentContext.Value = _previous;
            }
        }
    }
}
