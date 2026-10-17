using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Graphics.Shaders
{
    /// <summary>
    /// Loads .fx files at runtime and recompiles them when they change.
    /// Compilation is queued from the watcher thread and applied on Update,
    /// which is the only thread allowed to touch the GraphicsDevice here.
    /// </summary>
    public sealed class ShaderManager
    {
        private readonly Dictionary<string, Effect> _effects = new(StringComparer.OrdinalIgnoreCase);

        private readonly ConcurrentQueue<string> _pending = new();

        private FileSystemWatcher? _watcher;

        private string? _directory;

        public static ShaderManager Instance { get; } = new();

        public event Action<string, Effect>? EffectReloaded;

        public int Version { get; private set; }

        public string? LastError { get; private set; }

        public void Initialize()
        {
            if (_watcher is not null)
                return;

            _directory = ResolveDirectory();

            if (_directory is null)
                return;

            var watcher = new FileSystemWatcher(_directory, "*.fx")
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
            };

            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Renamed += OnChanged;
            watcher.EnableRaisingEvents = true;

            _watcher = watcher;
        }

        public Effect? Load(string name)
        {
            Initialize();

            if (_effects.TryGetValue(name, out var existing))
                return existing;

            return CompileAndStore(name);
        }

        public Effect? Get(string name) =>
            _effects.TryGetValue(name, out var effect) ? effect : null;

        public void Update()
        {
            while (_pending.TryDequeue(out var name))
                Reload(name);
        }

        private void OnChanged(object sender, FileSystemEventArgs args)
        {
            if (Directory.Exists(args.FullPath))
                return;

            var fileName = Path.GetFileName(args.FullPath);

            // Visual Studio and other editors save through temporary files such
            // as "Paper.fx~RF21f2c920.TMP". They are not shaders and disappear
            // before compilation, so never enqueue them.
            if (!fileName.EndsWith(".fx", StringComparison.OrdinalIgnoreCase) ||
                fileName.Contains('~', StringComparison.Ordinal) ||
                fileName.EndsWith(".tmp", StringComparison.OrdinalIgnoreCase))
                return;

            var relative = Path.GetRelativePath(_directory!, args.FullPath).Replace('\\', '/');

            _pending.Enqueue(relative);
        }

        private void Reload(string name)
        {
            var next = Compile(name, out var error);

            if (next is null)
            {
                LastError = error;

                MonoCoC.UI.Console.ConsoleOverlay.WriteLine($"[Shader] {name}: {error}");

                return;
            }

            _effects[name] = next;
            LastError = null;
            Version++;

            EffectReloaded?.Invoke(name, next);
        }

        private Effect? CompileAndStore(string name)
        {
            var effect = Compile(name, out var error);

            if (effect is null)
            {
                LastError = error;

                MonoCoC.UI.Console.ConsoleOverlay.WriteLine($"[Shader] {name}: {error}");

                return null;
            }

            _effects[name] = effect;
            Version++;

            return effect;
        }

        private Effect? Compile(string name, out string? error)
        {
            error = null;

            if (_directory is null)
            {
                error = "Shader directory not found";
                return null;
            }

            var path = Path.Combine(_directory, name.Replace('/', Path.DirectorySeparatorChar));

            if (!File.Exists(path))
            {
                error = $"Shader not found: {path}";
                return null;
            }

            try
            {
                return FxCompiler.Compile(path);
            }
            catch (Exception exception)
            {
                error = exception.Message;
                return null;
            }
        }

        private static string? ResolveDirectory()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "MonoCoC.csproj")))
                {
                    var source = Path.Combine(directory.FullName, "Assets", "Shaders");

                    if (Directory.Exists(source))
                        return source;
                }

                directory = directory.Parent;
            }

            var fallback = Path.Combine(MonoCoC.Assets.Paths.AssetPath("Shaders"));

            return Directory.Exists(fallback) ? fallback : null;
        }
    }
}
