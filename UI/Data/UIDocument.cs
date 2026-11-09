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

        public IReadOnlyDictionary<string, JsonObject> ControlTemplates { get; }

        private UIDocument(
            string id,
            string source,
            JsonObject root,
            IReadOnlyDictionary<string, JsonObject> templates,
            IReadOnlyDictionary<string, JsonObject> controlTemplates)
        {
            Id = id;
            Source = source;
            Root = root;
            Templates = templates;
            ControlTemplates = controlTemplates;
        }

        public static UIDocument Load(string path)
        {
            var text = ReadAllTextShared(path);

            var root = HjsonValue.Parse(text) as JsonObject
                ?? throw new InvalidDataException($"{path}: UI document must be an object");

            var id = root.Str("id");

            if (id.Length == 0)
                throw new InvalidDataException($"{path}: UI document is missing id");

            var templates = new Dictionary<string, JsonObject>(StringComparer.OrdinalIgnoreCase);

            var controlTemplates = new Dictionary<string, JsonObject>(StringComparer.OrdinalIgnoreCase);

            var globalPath = UIResourceLocator.ResolveDocument("GlobalStyles.hjson");

            if (File.Exists(globalPath))
            {
                var globalRoot = HjsonValue.Parse(ReadAllTextShared(globalPath)) as JsonObject;

                if (globalRoot is not null)
                    ReadTables(globalRoot, templates, controlTemplates);
            }

            ReadTables(root, templates, controlTemplates);

            return new UIDocument(id, path, root, templates, controlTemplates);
        }

        private static void ReadTables(
            JsonObject root,
            IDictionary<string, JsonObject> templates,
            IDictionary<string, JsonObject> controlTemplates)
        {
            if (root.TryGetValue("templates", out var value) && value is JsonObject table)
                foreach (var pair in table)
                    if (pair.Value is JsonObject definition)
                        templates[pair.Key] = definition;

            if (root.TryGetValue("controlTemplates", out var controlValue) && controlValue is JsonObject controlTable)
                foreach (var pair in controlTable)
                    if (pair.Value is JsonObject definition)
                        controlTemplates[pair.Key] = definition;
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
