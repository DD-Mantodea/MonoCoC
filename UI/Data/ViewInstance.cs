using MonoCoC.UI.Components;
using System;

namespace MonoCoC.UI.Data
{
    public static class ViewInstance<T> where T : View, new()
    {
        private static readonly object Sync = new();

        private static T? _instance;

        public static T Instance
        {
            get
            {
                if (_instance is null)
                    lock (Sync)
                        _instance ??= new T();

                return _instance;
            }
        }

        public static T Get() => Instance;

        public static void Reset()
        {
            lock (Sync)
                _instance = null;
        }
    }
}
