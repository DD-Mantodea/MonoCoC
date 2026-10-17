using MonoCoC.Assets;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Discovers, loads and unloads .cocmod packages.
    /// </summary>
    public sealed class ModHost
    {
        public const int ApiVersion = 1;

        private static readonly Lazy<ModHost> LazyInstance = new(() => new ModHost());

        private readonly List<ModCandidate> _installed = [];
        private readonly List<LoadedMod> _loaded = [];
        private readonly List<WeakReference<ModLoadContext>> _retiredContexts = [];
        private readonly List<string> _errors = [];

        public static ModHost Instance => LazyInstance.Value;

        public string ModDirectory => Path.Combine(Paths.Root, "Mods");

        public string EnabledFile => Path.Combine(ModDirectory, "enabled.json");

        public IReadOnlyList<ModCandidate> Installed => _installed;

        public IReadOnlyList<LoadedMod> Loaded => _loaded;

        public IReadOnlyList<string> Errors => _errors;

        public IReadOnlyList<string> GetLiveRetiredContexts()
        {
            var names = new List<string>();

            foreach (var reference in _retiredContexts)
                if (reference.TryGetTarget(out var context))
                    names.Add(context.Name ?? "unknown");

            return names;
        }

        public void Scan()
        {
            _installed.Clear();
            Directory.CreateDirectory(ModDirectory);

            foreach (var path in Directory.EnumerateFiles(ModDirectory, "*.cocmod", SearchOption.TopDirectoryOnly).OrderBy(path => path, StringComparer.OrdinalIgnoreCase))
            {
                try
                {
                    using var package = CocModPackage.Open(path);

                    var manifest = package.ReadManifest();

                    _installed.Add(new ModCandidate { PackagePath = path, Manifest = manifest });
                }
                catch (Exception exception)
                {
                    _installed.Add(new ModCandidate { PackagePath = path, Error = exception.Message });
                }
            }
        }

        public IReadOnlyList<string> ReadEnabled()
        {
            try
            {
                if (!File.Exists(EnabledFile))
                    return [];

                return JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(EnabledFile)) ?? [];
            }
            catch
            {
                return [];
            }
        }

        public void SaveEnabled(IEnumerable<string> ids)
        {
            Directory.CreateDirectory(ModDirectory);

            File.WriteAllText(EnabledFile, JsonConvert.SerializeObject(ids.ToArray(), Formatting.Indented));
        }

        public ModResolveResult Resolve(IEnumerable<string>? enabledIds = null)
        {
            if (_installed.Count == 0)
                Scan();

            return ModResolver.Resolve(_installed, enabledIds ?? ReadEnabled());
        }

        public void LoadEnabled()
        {
            var result = Resolve();

            if (result.Errors.Count > 0)
                throw new ModResolveException(result.Errors);

            UnloadAll();

            try
            {
                foreach (var candidate in result.Mods)
                    Load(candidate);

                SaveEnabled(result.EnabledIds);
            }
            catch
            {
                UnloadAll();
                throw;
            }
        }

        public void Reload()
        {
            var enabled = ReadEnabled();

            UnloadAll();
            Scan();

            var result = Resolve(enabled);

            if (result.Errors.Count > 0)
                throw new ModResolveException(result.Errors);

            foreach (var candidate in result.Mods)
                Load(candidate);

            SaveEnabled(result.EnabledIds);
        }

        public LoadedMod Load(ModCandidate candidate)
        {
            if (candidate.Manifest is null)
                throw new InvalidDataException(candidate.Error ?? $"invalid mod: {candidate.PackagePath}");

            if (_loaded.Any(loaded => string.Equals(loaded.Mod.Name, candidate.Manifest.Id, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"mod is already loaded: {candidate.Manifest.Id}");

            CocModPackage? package = null;
            ModLoadContext? loadContext = null;
            ModContext? context = null;
            Mod? mod = null;

            try
            {
                package = CocModPackage.Open(candidate.PackagePath);

                var manifest = package.ReadManifest();

                if (manifest.ApiVersion != ApiVersion)
                    throw new InvalidDataException($"{manifest.Id}: requires mod API {manifest.ApiVersion}, this loader supports {ApiVersion}");

                loadContext = new ModLoadContext(manifest.Id, package);

                var assembly = loadContext.LoadMainAssembly(manifest.Assembly);

                ModPreJit.Prepare(
                    assembly,
                    manifest.Id,
                    id => string.Equals(id, manifest.Id, StringComparison.OrdinalIgnoreCase) ||
                        _loaded.Any(loaded => string.Equals(loaded.Mod.Name, id, StringComparison.OrdinalIgnoreCase)));

                var entryType = assembly.GetType(manifest.Entry, throwOnError: false)
                    ?? throw new InvalidDataException($"{manifest.Id}: entry type '{manifest.Entry}' was not found");

                if (entryType.IsAbstract || !typeof(Mod).IsAssignableFrom(entryType))
                    throw new InvalidDataException($"{manifest.Id}: '{manifest.Entry}' must inherit Mod");

                mod = (Mod)Activator.CreateInstance(entryType)!;

                context = new ModContext(manifest, package, loadContext, new ModContentRegistry(manifest.Id))
                {
                    Instance = mod
                };

                mod.Context = context;

                var assets = context.Assets;

                TextureRegistry.Instance.RegisterProvider(manifest.Id, path =>
                    assets.RequestIfExists<Microsoft.Xna.Framework.Graphics.Texture2D>(path, out var texture) ? texture : null);

                using (context.Enter())
                {
                    mod.Load();

                    ModDataManagers.EnsureLoaded();

                    AssetManagerRegistry.LoadModData(context);

                    context.DataLoaders.LoadAll(context.Data);

                    context.Content.Scan(assembly);

                    context.Content.SetupContent();

                    mod.SetupContent();

                    mod.PostSetupContent();
                }

                context.IsLoaded = true;

                var loaded = new LoadedMod { Mod = mod, Context = context };

                _loaded.Add(loaded);

                return loaded;
            }
            catch (Exception exception)
            {
                _errors.Add($"{candidate.Id}: {exception.Message}");

                if (mod is not null && context is not null)
                {
                    try
                    {
                        using (context.Enter())
                            mod.Unload();
                    }
                    catch
                    {
                    }
                }

                if (context is not null)
                    AssetManagerRegistry.RemoveModData(context.Manifest.Id);

                if (context is not null)
                    TextureRegistry.Instance.Unregister(context.Manifest.Id);

                context?.Dispose();

                loadContext?.Unload();

                if (context is null)
                    package?.Dispose();

                mod = null;
                context = null;
                loadContext = null;

                throw;
            }
        }

        public bool Unload(string id)
        {
            var loaded = _loaded.FirstOrDefault(item => string.Equals(item.Mod.Name, id, StringComparison.OrdinalIgnoreCase));

            if (loaded is null)
                return false;

            Unload(loaded);

            return true;
        }

        public void UnloadAll()
        {
            for (var index = _loaded.Count - 1; index >= 0; index--)
                Unload(_loaded[index]);

            _loaded.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public bool TryGetMod(string id, out Mod? mod)
        {
            mod = _loaded.FirstOrDefault(item => string.Equals(item.Mod.Name, id, StringComparison.OrdinalIgnoreCase))?.Mod;

            return mod is not null;
        }

        public void WriteErrors(TextWriter writer)
        {
            foreach (var error in _errors)
                writer.WriteLine(error);
        }

        private void Unload(LoadedMod loaded)
        {
            try
            {
                using (loaded.Context.Enter())
                    loaded.Mod.Unload();
            }
            catch (Exception exception)
            {
                _errors.Add($"{loaded.Mod.Name}: unload failed: {exception.Message}");
            }
            finally
            {
                var loadContext = loaded.Context.LoadContext;

                TextureRegistry.Instance.Unregister(loaded.Context.Manifest.Id);

                loaded.Context.Dispose();

                AssetManagerRegistry.RemoveModData(loaded.Mod.Name);

                if (loadContext is not null)
                {
                    _retiredContexts.Add(new WeakReference<ModLoadContext>(loadContext));

                    loadContext.BeginUnload();
                }

                _loaded.Remove(loaded);
            }
        }
    }
}
