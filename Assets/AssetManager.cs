using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets
{
    public abstract class AssetManager<T> : IAssetManager where T : class
    {
        public AssetManager()
        {
            AssetManagerRegistry.Register(this);

            Load();
        }

        public Dictionary<string, T> Dict = [];

        public abstract string Folder { get; }

        public abstract string Pattern { get; }

        public virtual bool SupportsDataSource => false;

        public virtual void Load()
        {
            foreach (var file in Paths.GetAssets(Folder, Pattern))
            {
                var entries = new Dictionary<string, T>();

                FromText(file, File.ReadAllText(file), entries);

                foreach (var pair in entries)
                    Store(pair.Key, pair.Value, null);
            }
        }

        public virtual void FromText(string path, string text, IDictionary<string, T> entries) =>
            FromFile(path, entries);

        public virtual void FromFile(string path, IDictionary<string, T> entries) =>
            throw new NotSupportedException($"{GetType().Name} does not implement FromText/FromFile");

        public virtual void LoadFromSource(IDataFileSource source, string owner)
        {
            if (!SupportsDataSource)
                return;

            var folder = Folder.Trim('/');

            var dataFolder = folder.StartsWith("Data/", StringComparison.OrdinalIgnoreCase)
                ? folder[5..]
                : folder;

            var prefix = dataFolder.Trim('/') + "/";

            foreach (var path in source.Enumerate(folder, Pattern))
            {
                var relative = path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                    ? path[prefix.Length..]
                    : Path.GetFileName(path);

                var virtualPath = Path.Combine(Paths.AssetPath(Folder), relative);

                var entries = new Dictionary<string, T>();

                FromText(virtualPath, source.ReadAllText(path), entries);

                foreach (var pair in entries)
                    Store(pair.Key, pair.Value, owner);
            }
        }

        public virtual void RemoveModData(string owner)
        {
        }

        protected virtual void Store(string key, T value) => Dict[key] = value;

        protected virtual void Store(string key, T value, string? owner) => Store(key, value);

        public T? this[string index] => Dict.TryGetValue(index, out var value) ? value : null;
    }

    public interface IAssetManager
    {
        bool SupportsDataSource { get; }

        void LoadFromSource(IDataFileSource source, string owner);

        void RemoveModData(string owner);
    }
}
