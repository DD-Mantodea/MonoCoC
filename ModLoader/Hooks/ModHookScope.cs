using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Owns every MonoMod hook registered by one mod.
    /// </summary>
    public sealed class ModHookScope : IDisposable
    {
        private readonly List<IDisposable> _hooks = [];

        private bool _disposed;

        internal ModHookScope(string modId)
        {
            ModId = modId;
        }

        public string ModId { get; }

        public IDisposable Add(MethodBase target, Delegate hook)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ModHookScope));

            var handle = new Hook(target, hook);

            _hooks.Add(handle);

            return handle;
        }

        public IDisposable Modify(MethodBase target, ILContext.Manipulator manipulator)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(ModHookScope));

            var handle = new ILHook(target, manipulator);

            _hooks.Add(handle);

            return handle;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            List<Exception>? errors = null;

            for (var index = _hooks.Count - 1; index >= 0; index--)
            {
                try
                {
                    _hooks[index].Dispose();
                }
                catch (Exception exception)
                {
                    errors ??= [];
                    errors.Add(exception);
                }
            }

            _hooks.Clear();

            if (errors is { Count: > 0 })
                throw new AggregateException($"{ModId}: failed to remove {errors.Count} hook(s)", errors);
        }
    }
}
