using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Collectible load context for one mod and its private dependencies.
    /// </summary>
    public sealed class ModLoadContext : AssemblyLoadContext
    {
        private readonly CocModPackage _package;
        private readonly Dictionary<string, Assembly> _assemblies = new(StringComparer.OrdinalIgnoreCase);

        public ModLoadContext(string name, CocModPackage package) : base(name, isCollectible: true)
        {
            _package = package;
        }

        public Assembly LoadMainAssembly(string path)
        {
            var assembly = LoadFromStream(new MemoryStream(_package.ReadAllBytes(path), false));

            _assemblies[assembly.GetName().Name ?? path] = assembly;

            return assembly;
        }

        public void BeginUnload()
        {
            // The assembly dictionary keeps a strong reference to the mod assembly
            // through the collectible ALC; clear it before Unload so the ALC can die.
            _assemblies.Clear();

            Unload();
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name is null)
                return null;

            if (_assemblies.TryGetValue(assemblyName.Name, out var existing))
                return existing;

            if (Default.Assemblies.Any(assembly => assembly.GetName().Name == assemblyName.Name))
                return null;

            var dependencyPath = "lib/" + assemblyName.Name + ".dll";

            if (!_package.Contains(dependencyPath))
                return null;

            var loaded = LoadFromStream(new MemoryStream(_package.ReadAllBytes(dependencyPath), false));

            _assemblies[assemblyName.Name] = loaded;

            return loaded;
        }
    }
}
