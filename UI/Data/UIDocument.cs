using Hjson;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.UI.Data
{
    /// <summary>
    /// One Hjson screen definition plus its local template table.
    /// </summary>
    public sealed class UIDocument
    {
        public string Id { get; }

        public string Source { get; }

        public JsonObject Root { get; }

        public IReadOnlyDictionary<string, JsonObject> Templates { get; }

        private UIDocument(string id, string source, JsonObject root, IReadOnlyDictionary<string, JsonObject> templates)
        {
            Id = id;
            Source = source;
            Root = root;
            Templates = templates;
        }

        public static UIDocument Load(string path)
        {
            var text = File.ReadAllText(path);

            var root = HjsonValue.Parse(text) as JsonObject
                ?? throw new InvalidDataException($"{path}: UI document must be an object");

            var id = root.Str("id");

            if (id.Length == 0)
                throw new InvalidDataException($"{path}: UI document is missing id");

            var templates = new Dictionary<string, JsonObject>(StringComparer.OrdinalIgnoreCase);

            if (root.TryGetValue("templates", out var value) && value is JsonObject table)
                foreach (var pair in table)
                    if (pair.Value is JsonObject definition)
                        templates[pair.Key] = definition;

            return new UIDocument(id, path, root, templates);
        }

        /// <summary>
        /// Expands template references before the component tree is built.
        /// Instance fields override template fields; arrays are replaced.
        /// </summary>
        public JsonObject Expand(JsonObject node)
        {
            return Expand(node, []);
        }

        private JsonObject Expand(JsonObject node, HashSet<string> stack)
        {
            var templateId = node.Str("template");

            if (templateId.Length == 0)
                return Clone(node);

            if (!Templates.TryGetValue(templateId, out var template))
                throw new InvalidDataException($"Unknown UI template '{templateId}' in {Source}");

            if (!stack.Add(templateId))
                throw new InvalidDataException($"UI template cycle detected at '{templateId}' in {Source}");

            var baseNode = Expand(template, stack);

            stack.Remove(templateId);

            var result = Merge(baseNode, node);

            result.Remove("template");

            return result;
        }

        private static JsonObject Merge(JsonObject baseNode, JsonObject overlay)
        {
            var result = Clone(baseNode);

            foreach (var pair in overlay)
            {
                if (string.Equals(pair.Key, "template", StringComparison.Ordinal))
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
            (JsonObject)HjsonValue.Parse(source.ToString());

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
