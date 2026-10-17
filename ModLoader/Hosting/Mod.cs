using System;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Entry point for a mod. Concrete mods inherit this class.
    /// </summary>
    public abstract class Mod
    {
        public ModContext Context { get; internal set; } = null!;

        public ModManifest Manifest => Context.Manifest;

        public string Name => Manifest.Id;

        public Version Version => Manifest.Version;

        public ModFiles Files => Context.Files;

        public bool HasAsset(string path) => Assets.HasAsset(path);

        public bool RequestAssetIfExists<T>(string path, out T? asset) where T : class =>
            Assets.RequestIfExists(path, out asset);

        public ModDataFiles Data => Context.Data;

        public ModDataLoaderRegistry DataLoaders => Context.DataLoaders;

        public IDataFileSource DataSource => Context.DataSource;

        public ModAssets Assets => Context.Assets;

        public ModHookScope Hooks => Context.Hooks;

        public virtual void Load()
        {
        }

        public virtual void SetupContent()
        {
        }

        public virtual void PostSetupContent()
        {
        }

        public virtual void Unload()
        {
        }

        public virtual void Close()
        {
        }
    }
}
