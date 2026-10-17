using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.IO;
using static MonoCoC.UI.Layout.LayoutConstants;

namespace MonoCoC.UI.Components.Views
{
    /// <summary>
    /// Data-driven main menu. The Hjson document drives existing controls;
    /// only this screen is swapped when the document changes.
    /// </summary>
    public class MainView : View
    {
        private readonly UIEventRegistry _events;

        private readonly UIWidgetRegistry _widgets;

        private readonly string _documentPath;

        private readonly FileSystemWatcher? _watcher;

        private View? _content;

        private volatile bool _reloadRequested;

        public MainView()
        {
            LayoutData.Size = new(ScreenWidth, ScreenHeight);

            _events = MainMenuEvents.Create();

            _widgets = new UIWidgetRegistry();

            _documentPath = ResolveDocumentPath();

            Build();

            var directory = Path.GetDirectoryName(_documentPath);

            if (directory is not null && File.Exists(_documentPath))
            {
                _watcher = new FileSystemWatcher(directory, Path.GetFileName(_documentPath))
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
                };

                _watcher.Changed += OnDocumentChanged;

                _watcher.Created += OnDocumentChanged;

                _watcher.Renamed += OnDocumentChanged;

                _watcher.EnableRaisingEvents = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (_reloadRequested)
            {
                _reloadRequested = false;

                Reload();
            }

            base.Update(gameTime);
        }

        private void OnDocumentChanged(object sender, FileSystemEventArgs args) => _reloadRequested = true;

        private void Build()
        {
            var document = UIDocument.Load(_documentPath);

            var content = new UIScreenBuilder(_events, _widgets).Build(document);

            if (_content is not null)
                RemoveChild(_content);

            _content = content;

            AddChild(content);
        }

        private void Reload()
        {
            try
            {
                Build();
            }
            catch (Exception exception)
            {
                MonoCoC.UI.Console.ConsoleOverlay.WriteLine($"UI reload failed: {exception.Message}");
            }
        }

        /// <summary>
        /// Prefer the source Hjson while running from the repository. This lets
        /// an editor save update the running game without a build/copy step.
        /// Fall back to the runtime Assets copy for a deployed build.
        /// </summary>
        private static string ResolveDocumentPath()
        {
            var directory = new DirectoryInfo(AppContext.BaseDirectory);

            while (directory is not null)
            {
                if (File.Exists(Path.Combine(directory.FullName, "MonoCoC.csproj")))
                {
                    var source = Path.Combine(directory.FullName, "Assets", "UI", "Screens", "MainMenu.hjson");

                    if (File.Exists(source))
                        return source;
                }

                directory = directory.Parent;
            }

            return Path.Combine(Paths.AssetPath("UI/Screens"), "MainMenu.hjson");
        }
    }
}
