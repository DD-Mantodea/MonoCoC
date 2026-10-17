using Hjson;
using MonoCoC.Assets;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Configuration
{
    public class OptionStore
    {
        public static OptionStore Instance { get; } = new();

        public List<OptionPaneDefinition> Panes { get; private set; } = [];

        private readonly Dictionary<string, int> _values = [];

        private readonly Dictionary<string, OptionDefinition> _definitions = [];

        private readonly Dictionary<string, bool> _global = [];

        private bool _initialized;

        public void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;

            Panes = OptionCatalog.Load();

            foreach (var pane in Panes)
                foreach (var setting in pane.Settings)
                {
                    _definitions[setting.Id] = setting;

                    _global[setting.Id] = pane.Global;

                    _values.TryAdd(setting.Id, setting.DefaultValue);
                }
        }

        public int Get(string id, int fallback = 0)
        {
            Initialize();

            return _values.TryGetValue(id, out var value) ? value : fallback;
        }

        public void Set(string id, int value)
        {
            Initialize();

            _values[id] = value;
        }

        public bool IsGlobal(string id)
        {
            Initialize();

            return _global.TryGetValue(id, out var global) && global;
        }

        public JsonObject Save(bool global)
        {
            Initialize();

            var result = new JsonObject();

            foreach (var (id, definition) in _definitions)
            {
                if (IsGlobal(id) != global)
                    continue;

                result[id] = Get(id, definition.DefaultValue);
            }

            return result;
        }

        public void Load(JsonObject json, bool global)
        {
            Initialize();

            if (json == null)
                return;

            foreach (var (id, definition) in _definitions)
            {
                if (IsGlobal(id) != global)
                    continue;

                if (!json.TryGetValue(id, out var value) || value == null)
                    continue;

                if (int.TryParse(value.ToString(), out var number))
                    _values[id] = number;
            }
        }

        public void ResetLocal()
        {
            Initialize();

            foreach (var (id, definition) in _definitions)
                if (!IsGlobal(id))
                    _values[id] = definition.DefaultValue;
        }

        public static string FilePath => Path.Combine(Paths.Root, "options.json");

        public static void LoadGlobal()
        {
            Instance.Initialize();

            if (!File.Exists(FilePath))
                return;

            try
            {
                var root = (JsonObject)HjsonValue.Parse(File.ReadAllText(FilePath));

                Instance.Load(root, true);
            }
            catch
            {
                // Broken options file: keep defaults.
            }
        }

        public static void SaveGlobal()
        {
            Instance.Initialize();

            try
            {
                var temp = FilePath + ".tmp";

                File.WriteAllText(temp, Instance.Save(true).ToString());

                File.Move(temp, FilePath, true);
            }
            catch
            {
                // Options are not critical enough to interrupt the game.
            }
        }

        public static JsonObject LocalToJson()
        {
            Instance.Initialize();

            return Instance.Save(false);
        }

        public static void ApplyLocal(JsonObject json)
        {
            Instance.Initialize();

            if (json == null || json.Count == 0)
            {
                Instance.ResetLocal();

                return;
            }

            Instance.Load(json, false);
        }

        public static void ResetLocalOptions()
        {
            Instance.Initialize();

            Instance.ResetLocal();
        }
    }
}