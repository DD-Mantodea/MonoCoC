using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MonoCoC.Configuration;
using MonoCoC.Contents.Scenes;

namespace MonoCoC.UI.Data
{
    public class HjsonView : View
    {
        private FileSystemWatcher? _watcher;

        private FileSystemWatcher? _styleWatcher;

        private string _documentPath;

        private string _screenFile;

        private UIEventRegistry? _events;

        private UICustomComponentRegistry? _customComponents;

        private View? _content;

        protected void SetDataContext(IUIDataContext? context)
        {
            DataContext = context;

            _content?.ApplyDataContext(context);
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

        protected void Initialize()
        {
            _events = new UIEventRegistry();

            _customComponents = new UICustomComponentRegistry();

            UIAttributeScanner.RegisterEvents(_events, this);

            UIAttributeScanner.RegisterComponents(_customComponents, this);

            TryBuild();

            _languageRevision = LanguageService.Revision;
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

        public T RequirePart<T>(string name) where T : Component
        {
            var part = Part<T>(name);

            if (part is not null)
                return part;

            var message = $"View '{Contract.Id}' is missing required part '{name}' ({typeof(T).Name})";

            MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] {message}");

            throw new UIViewContractException(message);
        }

        protected virtual void OnBuilt(View root, UIDocument document)
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

                if (_events is null || _customComponents is null)
                    throw new InvalidOperationException("HjsonView.Initialize was not called");

                var content = new UIScreenBuilder(_events, _customComponents).Build(document);

                Contract.Validate(content);

                _content = content;

                content.ApplyDataContext(DataContext);

                OnBuilt(content, document);

                BindAttributedParts(content);

                OnPartsBound();

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
                    $"[UI] {GetType().Name} load failed [{_documentPath}]: {exception}");
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

        protected virtual void OnPartsBound()
        {
        }

        private void BindAttributedParts(Component content)
        {
            for (var type = GetType(); type is not null && type != typeof(object); type = type.BaseType)
            {
                foreach (var field in type.GetFields(
                             BindingFlags.DeclaredOnly |
                             BindingFlags.Public |
                             BindingFlags.NonPublic |
                             BindingFlags.Instance))
                    BindAttributedMember(
                        content,
                        field.Name,
                        field.FieldType,
                        field.GetCustomAttribute<UIRequiredAttribute>()?.Part,
                        field.GetCustomAttribute<UIOptionalAttribute>()?.Part,
                        field.IsInitOnly,
                        value => field.SetValue(this, value));

                foreach (var property in type.GetProperties(
                             BindingFlags.DeclaredOnly |
                             BindingFlags.Public |
                             BindingFlags.NonPublic |
                             BindingFlags.Instance))
                    BindAttributedMember(
                        content,
                        property.Name,
                        property.PropertyType,
                        property.GetCustomAttribute<UIRequiredAttribute>()?.Part,
                        property.GetCustomAttribute<UIOptionalAttribute>()?.Part,
                        !property.CanWrite,
                        value => property.SetValue(this, value));

                foreach (var method in type.GetMethods(
                             BindingFlags.DeclaredOnly |
                             BindingFlags.Public |
                             BindingFlags.NonPublic |
                             BindingFlags.Instance))
                    BindAttributedMethod(
                        content,
                        method,
                        method.GetCustomAttribute<UIRequiredAttribute>()?.Part,
                        method.GetCustomAttribute<UIOptionalAttribute>()?.Part);
            }
        }

        private void BindAttributedMethod(
            Component content,
            MethodInfo method,
            string? requiredPart,
            string? optionalPart)
        {
            var part = requiredPart ?? optionalPart;

            if (string.IsNullOrEmpty(part))
                return;

            var parameters = method.GetParameters();

            if (method.ReturnType != typeof(void) || parameters.Length != 1)
                throw new UIViewContractException(
                    $"View '{GetType().Name}' method '{method.Name}' must return void and take one part");

            var memberType = parameters[0].ParameterType;

            var component = FindPart<Component>(content, part);

            if (component is null)
            {
                if (optionalPart is not null)
                    return;

                var message = $"View '{GetType().Name}' method '{method.Name}' requires part '{part}' ({memberType.Name})";

                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] {message}");

                throw new UIViewContractException(message);
            }

            if (!memberType.IsInstanceOfType(component))
            {
                var message = $"View '{GetType().Name}' method '{method.Name}' expects {memberType.Name} " +
                              $"but part '{part}' is {component.GetType().Name}";

                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] {message}");

                if (requiredPart is not null)
                    throw new UIViewContractException(message);

                return;
            }

            method.Invoke(this, [component]);
        }

        private void BindAttributedMember(
            Component content,
            string memberName,
            Type memberType,
            string? requiredPart,
            string? optionalPart,
            bool readOnly,
            Action<object> assign)
        {
            var part = requiredPart ?? optionalPart;

            if (string.IsNullOrEmpty(part))
                return;

            if (readOnly)
                throw new UIViewContractException(
                    $"View '{GetType().Name}' member '{memberName}' is read-only and cannot be bound to '{part}'");

            var component = FindPart<Component>(content, part);

            if (component is null)
            {
                if (optionalPart is not null)
                    return;

                var message = $"View '{GetType().Name}' member '{memberName}' requires part '{part}' ({memberType.Name})";

                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] {message}");

                throw new UIViewContractException(message);
            }

            if (!memberType.IsInstanceOfType(component))
            {
                var message = $"View '{GetType().Name}' member '{memberName}' expects {memberType.Name} " +
                              $"but part '{part}' is {component.GetType().Name}";

                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] {message}");

                if (requiredPart is not null)
                    throw new UIViewContractException(message);

                return;
            }

            assign(component);
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
