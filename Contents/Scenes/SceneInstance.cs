using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes
{

    public static class SceneInstance<T> where T : Scene, new()
    {
        public static readonly object Sync = new();

        public static T? _instance;

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

        public static T Get(bool fresh = false)
        {
            if (fresh)
                lock (Sync)

                    _instance = new T();

            return Instance;
        }

        public static void Reset()
        {
            lock (Sync)

                _instance = null;
        }
    }

    public static class SceneInstance
    {
        public static readonly Dictionary<string, Scene> Instances = [];

        public static readonly object Sync = new();

        public static T Of<T>(string id) where T : Scene
        {
            var key = id;

            lock (Sync)
            {
                if (Instances.TryGetValue(key, out var cached) && cached is T typed)
                    return typed;

                var created = (T)SceneRegistry.CreateOrDefault(id);

                Instances[key] = created;

                return created;
            }
        }

        public static void Reset()
        {
            lock (Sync)

                Instances.Clear();
        }
    }
}