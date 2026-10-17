using MonoCoC.ModLoader;
using System.Collections.Generic;

namespace MonoCoC.Assets
{
    /// <summary>
    /// Registry of all constructed asset/content managers. Used to fan out
    /// mod data loading and unload without a global text-injection shim.
    /// </summary>
    public static class AssetManagerRegistry
    {
        private static readonly List<IAssetManager> _managers = [];

        public static IReadOnlyList<IAssetManager> Managers => _managers;

        internal static void Register(IAssetManager manager) => _managers.Add(manager);

        public static void LoadModData(ModContext context)
        {
            foreach (var manager in _managers)
                manager.LoadFromSource(context.DataSource, context.Manifest.Id);
        }

        public static void RemoveModData(string owner)
        {
            foreach (var manager in _managers)
                manager.RemoveModData(owner);
        }
    }

    public static class ManagerInstance<T> where T : IAssetManager, new()
    {
        public static T Instance = new();
    }
}
