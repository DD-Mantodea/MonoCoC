using Hjson;
using MonoCoC.UI.Framework;
using System;

namespace MonoCoC.UI.Data
{
    public sealed class UIBuildContext(UIEventRegistry events)
    {
        public UIDocument? Document { get; set; }

        public UIEventRegistry Events { get; } = events;

        public Func<JsonObject, UIDocument, Component?>? TemplateBuilder { get; set; }

        public JsonObject? ResolveStyle(JsonObject node, string key)
        {
            if (!node.TryGetValue(key, out var value))
                return null;

            if (value is JsonObject inline)
                return inline;

            var name = value.ToString().Trim('"');

            return Document is not null && Document.Styles.TryGetValue(name, out var named) ? named : null;
        }
        public Component? BuildTemplate(JsonObject node) =>
            TemplateBuilder?.Invoke(node, Document!);
    }
}