using Hjson;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MonoCoC.UI.Data
{
    public class UIDocument
    {
        public string Id { get; }

        public string Source { get; }

        public JsonObject Root { get; }

        public IReadOnlyDictionary<string, JsonObject> Styles { get; }

        public IReadOnlyDictionary<string, JsonObject> ComponentTemplates { get; }

        private UIDocument(
            string id,
            string source,
            JsonObject root,
            IReadOnlyDictionary<string, JsonObject> styles,
            IReadOnlyDictionary<string, JsonObject> componentTemplates)
        {
            Id = id;
            Source = source;
            Root = root;
            Styles = styles;
            ComponentTemplates = componentTemplates;
        }

        public static UIDocument Load(string path)
        {
            var text = ReadAllTextShared(path);

            var root = MonoCoC.HjsonParser.Parse(text) as JsonObject
                ?? throw new InvalidDataException($"{path}: UI document must be an object");

            var id = root.Str("id");

            if (id.Length == 0)
                throw new InvalidDataException($"{path}: UI document is missing id");

            var styles = new Dictionary<string, JsonObject>(StringComparer.OrdinalIgnoreCase);

            var componentTemplates = new Dictionary<string, JsonObject>(StringComparer.OrdinalIgnoreCase);

            LoadRequires(root, path, styles, componentTemplates, new HashSet<string>(StringComparer.OrdinalIgnoreCase));

            ReadTables(root, styles, componentTemplates);

            return new UIDocument(id, path, root, styles, componentTemplates);
        }

        private sealed record CachedDocument(DateTime Stamp, JsonObject Root);

        private static readonly Dictionary<string, CachedDocument> Cache =
            new(StringComparer.OrdinalIgnoreCase);

        private static JsonObject LoadCached(string path)
        {
            var stamp = File.GetLastWriteTimeUtc(path);

            if (Cache.TryGetValue(path, out var cached) && cached.Stamp == stamp)
                return cached.Root;

            var root = MonoCoC.HjsonParser.Parse(ReadAllTextShared(path)) as JsonObject
                ?? throw new InvalidDataException($"{path}: UI document must be an object");

            Cache[path] = new CachedDocument(stamp, root);

            return root;
        }

        private static void LoadRequires(
            JsonObject root,
            string source,
            IDictionary<string, JsonObject> styles,
            IDictionary<string, JsonObject> componentTemplates,
            HashSet<string> stack)
        {
            if (!root.TryGetValue("require", out var value))
                return;

            var names = value is JsonArray array
                ? array.Where(item => item is not null).Select(item => item!.ToString().Trim('"'))
                : [value.ToString().Trim('"')];

            foreach (var name in names)
            {
                if (name.Length == 0)
                    continue;

                var file = name.EndsWith(".hjson", StringComparison.OrdinalIgnoreCase) ? name : name + ".hjson";

                var requiredPath = UIResourceLocator.ResolveDocument(file);

                if (!File.Exists(requiredPath))
                    throw new InvalidDataException($"{source}: required UI document '{name}' was not found");

                if (!stack.Add(requiredPath))
                    throw new InvalidDataException($"{source}: UI require cycle at '{name}'");

                var requiredRoot = LoadCached(requiredPath);

                LoadRequires(requiredRoot, requiredPath, styles, componentTemplates, stack);

                ReadTables(requiredRoot, styles, componentTemplates);

                stack.Remove(requiredPath);
            }
        }

        private static void ReadTables(
            JsonObject root,
            IDictionary<string, JsonObject> styles,
            IDictionary<string, JsonObject> componentTemplates)
        {
            if (root.TryGetValue("styles", out var value) && value is JsonObject table)
                foreach (var pair in table)
                    if (pair.Value is JsonObject definition)
                        styles[pair.Key] = definition;

            if (root.TryGetValue("componentTemplates", out var componentValue) && componentValue is JsonObject componentTable)
                foreach (var pair in componentTable)
                    if (pair.Value is JsonObject definition)
                        componentTemplates[pair.Key] = definition;
        }

        private static string ReadAllTextShared(string path)
        {
            using var stream = new FileStream(
                path,
                FileMode.Open,
                FileAccess.Read,
                FileShare.ReadWrite | FileShare.Delete);

            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }

        public JsonObject Expand(JsonObject node)
        {
            return Expand(node, []);
        }

        private JsonObject Expand(JsonObject node, HashSet<string> stack)
        {
            var styleId = node.Str("style");

            if (styleId.Length == 0)
                return Clone(node);

            if (!Styles.TryGetValue(styleId, out var template))
                throw new InvalidDataException($"Unknown UI template '{styleId}' in {Source}");

            if (!stack.Add(styleId))
                throw new InvalidDataException($"UI template cycle detected at '{styleId}' in {Source}");

            var baseNode = Expand(template, stack);

            stack.Remove(styleId);

            var result = Merge(baseNode, node);

            result.Remove("style");

            return result;
        }

        private static JsonObject Merge(JsonObject baseNode, JsonObject overlay)
        {
            var result = Clone(baseNode);

            foreach (var pair in overlay)
            {
                if (string.Equals(pair.Key, "style", StringComparison.Ordinal))
                    continue;

                if (result.TryGetValue(pair.Key, out var existing) &&
                    existing is JsonObject baseObject &&
                    pair.Value is JsonObject overlayObject)
                {
                    result[pair.Key] = Merge(baseObject, overlayObject);

                    continue;
                }

                result[pair.Key] = CloneValue(pair.Value);
            }

            return result;
        }

        private static JsonObject Clone(JsonObject source) =>
            (JsonObject)MonoCoC.HjsonParser.Parse(source.ToString());

        private static JsonValue CloneValue(JsonValue value) => value switch
        {
            JsonObject json => Clone(json),
            JsonArray array => CloneArray(array),
            _ => value
        };

        private static JsonArray CloneArray(JsonArray source)
        {
            var clone = new JsonArray();

            foreach (var item in source)
                clone.Add(CloneValue(item));

            return clone;
        }
    }
}
