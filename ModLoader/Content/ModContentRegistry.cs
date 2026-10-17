using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Owns all code content instantiated by one mod.
    /// </summary>
    public sealed class ModContentRegistry : IDisposable
    {
        private readonly List<IModContent> _content = [];

        private readonly string _modId;

        private bool _disposed;

        internal ModContentRegistry(string modId)
        {
            _modId = modId;
        }

        public IReadOnlyList<IModContent> Content => _content;

        public T Register<T>(T content) where T : IModContent
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ModContentRegistry));

            content.Load();

            _content.Add(content);

            return content;
        }

        public void Scan(Assembly assembly)
        {
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition)
                    continue;

                if (!typeof(IModContent).IsAssignableFrom(type))
                    continue;

                if (type.GetConstructor(Type.EmptyTypes) is null)
                    continue;

                Register((IModContent)Activator.CreateInstance(type)!);
            }
        }

        public void SetupContent()
        {
            foreach (var content in _content)
                content.SetupContent();
        }

        public IEnumerable<T> GetContent<T>() where T : class, IModContent =>
            _content.OfType<T>();

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            for (var index = _content.Count - 1; index >= 0; index--)
                _content[index].Unload();

            _content.Clear();
        }
    }
}
