using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.UI.Data
{
    public class HjsonView : View
    {
        private FileSystemWatcher? _watcher;

        private FileSystemWatcher? _styleWatcher;

        private string _documentPath;

        private string _screenFile;

        private UIEventRegistry? _events;

        private UIWidgetRegistry? _widgets;

        private View? _content;

        private IUIDataContext? _dataContext;

        public IUIDataContext? DataContext
        {
            get => _dataContext;
            set
            {
                _dataContext = value;

                _content?.ApplyDataContext(value);
            }
        }

        private volatile bool _reloadRequested;

        private DateTime _reloadAt;

        private int _languageRevision = -1;

        protected HjsonView()
        {
            _screenFile = ScreenFile;

            _documentPath = UIResourceLocator.ResolveDocument(_screenFile);

            CreateWatcher();
        }

        protected HjsonView(string screenFile)
        {
            _screenFile = screenFile;

            _documentPath = UIResourceLocator.ResolveDocument(_screenFile);

            CreateWatcher();
        }

        protected virtual string ScreenFile => "";

        protected virtual UIViewContract Contract => UIViewContract.Empty;

        public void SetScreen(string screenFile)
        {
            if (string.Equals(_screenFile, screenFile, StringComparison.OrdinalIgnoreCase))
                return;

            _screenFile = screenFile;

            _documentPath = UIResourceLocator.ResolveDocument(screenFile);

            CreateWatcher();

            TryBuild();
        }

        private void CreateWatcher()
        {
            _watcher?.Dispose();

            _watcher = null;

            _styleWatcher?.Dispose();

            _styleWatcher = null;

            var directory = Path.GetDirectoryName(_documentPath);

            if (directory is not null && Directory.Exists(directory))
            {
                _watcher = new FileSystemWatcher(directory, Path.GetFileName(_documentPath))
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
                };

                _watcher.Changed += OnDocumentChanged;

                _watcher.Created += OnDocumentChanged;

                _watcher.Deleted += OnDocumentChanged;

                _watcher.Renamed += OnDocumentChanged;

                _watcher.EnableRaisingEvents = true;
            }

            var stylePath = UIResourceLocator.ResolveDocument("GlobalStyles.hjson");

            var styleDirectory = Path.GetDirectoryName(stylePath);

            if (styleDirectory is not null && Directory.Exists(styleDirectory))
            {
                _styleWatcher = new FileSystemWatcher(styleDirectory, Path.GetFileName(stylePath))
                {
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.FileName
                };

                _styleWatcher.Changed += OnDocumentChanged;

                _styleWatcher.Created += OnDocumentChanged;

                _styleWatcher.Deleted += OnDocumentChanged;

                _styleWatcher.Renamed += OnDocumentChanged;

                _styleWatcher.EnableRaisingEvents = true;
            }
        }

        public string DocumentPath => _documentPath;

        public UIDocument? Document { get; private set; }

        public View? Content => _content;

        protected void Initialize(UIEventRegistry events, UIWidgetRegistry widgets)
        {
            _events = events;

            _widgets = widgets;

            TryBuild();

            _languageRevision = MonoCoC.Configuration.LanguageService.Revision;
        }

        public override void Update(GameTime gameTime)
        {
            if (_reloadRequested && DateTime.UtcNow >= _reloadAt)
            {
                _reloadRequested = false;

                TryBuild();
            }

            if (_languageRevision != MonoCoC.Configuration.LanguageService.Revision)
            {
                _languageRevision = MonoCoC.Configuration.LanguageService.Revision;

                TryBuild();
            }

            base.Update(gameTime);
        }

        public T? Part<T>(string name) where T : Component
        {
            return _content is null ? null : FindPart<T>(_content, name);
        }

        public T RequirePart<T>(string name) where T : Component =>
            Part<T>(name) ?? throw new UIViewContractException(
                $"View '{Contract.Id}' is missing required part '{name}' ({typeof(T).Name})");

        protected virtual void OnBuilt(View root, UIDocument document) => BindParts();

        protected virtual void BindParts()
        {
        }

        private void OnDocumentChanged(object sender, FileSystemEventArgs args)
        {
            _reloadAt = DateTime.UtcNow.AddMilliseconds(150);

            _reloadRequested = true;
        }

        private void TryBuild()
        {
            if (string.IsNullOrEmpty(ScreenFile))
                return;

            var previous = _content;

            try
            {
                var focused = UIManager.Instance?.FocusedComponent;

                var focusedInContent = focused is not null &&
                                       previous is not null &&
                                       IsDescendant(previous, focused);

                var focusedPart = focused?.PartName;

                var state = CaptureState(previous);

                var document = UIDocument.Load(_documentPath);

                if (_events is null || _widgets is null)
                    throw new InvalidOperationException("HjsonView.Initialize was not called");

                var content = new UIScreenBuilder(_events, _widgets).Build(document);

                Contract.Validate(content);

                _content = content;

                content.ApplyDataContext(DataContext);

                OnBuilt(content, document);

                RestoreState(content, state);

                if (previous is not null)
                    RemoveChild(previous);

                AddChild(content);

                Document = document;

                if (focusedInContent)
                    RestoreFocus(content, focusedPart);
            }
            catch (Exception exception)
            {
                _content = previous;

                MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                    $"[UI] {GetType().Name} load failed [{_documentPath}]: " +
                    $"{exception.GetType().Name}: {exception.Message}");
            }
        }

        private static Dictionary<string, object> CaptureState(Component? root)
        {
            var result = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

            if (root is not null)
                CaptureState(root, result);

            return result;
        }

        private static void CaptureState(Component component, Dictionary<string, object> result)
        {
            if (component is IUIStatefulPart stateful && component.PartName.Length > 0)
            {
                var state = stateful.CaptureState();

                if (state is not null)
                    result[component.PartName] = state;
            }

            foreach (var child in component.LayoutData.Children)
                CaptureState(child, result);
        }

        private static void RestoreState(Component component, IReadOnlyDictionary<string, object> state)
        {
            if (component is IUIStatefulPart stateful &&
                component.PartName.Length > 0 &&
                state.TryGetValue(component.PartName, out var partState))
                stateful.RestoreState(partState);

            foreach (var child in component.LayoutData.Children)
                RestoreState(child, state);
        }

        private static bool IsDescendant(Component root, Component target)
        {
            for (var current = target; current is not null; current = current.LayoutData.Parent)
                if (ReferenceEquals(current, root))
                    return true;

            return false;
        }

        private static void RestoreFocus(Component content, string? partName)
        {
            if (UIManager.Instance is null)
                return;

            if (string.IsNullOrEmpty(partName))
            {
                UIManager.Instance.FocusedComponent = null;

                return;
            }

            var target = FindPart<Component>(content, partName);

            if (target is TextBox textBox)
                textBox.Active = true;
            else
                UIManager.Instance.FocusedComponent = target;
        }

        private static T? FindPart<T>(Component component, string name) where T : Component
        {
            if (string.Equals(component.PartName, name, StringComparison.OrdinalIgnoreCase) &&
                component is T typed)
                return typed;

            foreach (var child in component.LayoutData.Children)
            {
                var result = FindPart<T>(child, name);

                if (result is not null)
                    return result;
            }

            return null;
        }
    }
}
