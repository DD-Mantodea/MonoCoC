using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoCoC.Configuration
{
    public static class LanguageService
    {
        private static FileSystemWatcher? _watcher;

        private static volatile bool _reloadRequested;

        private static DateTime _reloadAt;

        public static int Revision { get; private set; }
        public static void ApplySaved()
        {
            var store = OptionStore.Instance;

            store.Initialize();

            var setting = store.Panes.SelectMany(pane => pane.Settings).FirstOrDefault(candidate => candidate.Apply == "language");

            if (setting == null)
            {
                EnsureWatcher();

                return;
            }

            var values = Choices();

            var value = store.Get(setting.Id, setting.DefaultValue);

            var choice = values.FirstOrDefault(candidate => candidate.Value == value);

            if (choice != null)
                Use(choice.Data);

            EnsureWatcher();
        }

        public static List<OptionValueDefinition> Choices()
        {
            var result = new List<OptionValueDefinition>();

            var manager = ManagerInstance<LocalizationManager>.Instance;

            for (var index = 0; index < manager.Languages.Count; index++)
            {
                var language = manager.Languages[index];

                result.Add(new OptionValueDefinition
                {
                    Value = index,

                    LabelKey = language.Name,

                    Data = language.Code
                });
            }

            return result;
        }

        public static void Use(string language)
        {
            if (string.IsNullOrEmpty(language))
                return;

            try
            {
                ManagerInstance<LocalizationManager>.Instance.Use(language);

                UI.UIFonts.ApplyLanguage(language);

                UI.UIFonts.ApplyToParser(Contents.Scenes.Scene.ScriptContext);
            }
            catch
            {
                // Unknown language: keep the current one.
            }
        }

        public static void Update()
        {
            if (!_reloadRequested || DateTime.UtcNow < _reloadAt)
                return;

            _reloadRequested = false;

            var manager = ManagerInstance<LocalizationManager>.Instance;

            var current = manager.Language;

            try
            {
                manager.Load();

                Use(current);

                Revision++;
            }
            catch (Exception exception)
            {
                MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                    $"[Localization] reload failed: {exception.Message}");
            }
        }

        private static void EnsureWatcher()
        {
            if (_watcher is not null)
                return;

            var root = LocalizationManager.RootPath;

            if (!Directory.Exists(root))
                return;

            _watcher = new FileSystemWatcher(root, "*.hjson")
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.Renamed += OnChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object sender, FileSystemEventArgs args)
        {
            _reloadAt = DateTime.UtcNow.AddMilliseconds(150);

            _reloadRequested = true;
        }
    }
}