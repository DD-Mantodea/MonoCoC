using MonoCoC.Scripting;
using System;
using System.Collections.Generic;

namespace MonoCoC.Macros
{

    public class MacroScope : IDisposable
    {
        public readonly Dictionary<string, Func<ScriptValue?>> _factories = new(StringComparer.OrdinalIgnoreCase);

        public bool _disposed;

        internal MacroScope(object owner) => Owner = owner;

        public object Owner { get; }

        public void Set(string name, Func<ScriptValue?> factory)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(MacroScope), $"作用域已结束，不能再注册标签 '{name}'");

            _factories[name] = factory;
        }

        internal bool TryResolve(string name, out ScriptValue value)
        {
            if (_factories.TryGetValue(name, out var factory))
            {
                var resolved = factory();

                if (resolved != null)
                {
                    value = resolved.Value;

                    return true;
                }
            }

            value = default;

            return false;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _factories.Clear();

            MacroRegistry.EndScope(this);
        }
    }
}