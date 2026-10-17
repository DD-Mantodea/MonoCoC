using Hjson;
using MonoCoC.Contents.Combat;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class DamageTypeManager : ContentManager<DamageTypeDefinition>
    {
        public override string Folder => "Data/Combat/DamageTypes";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, DamageTypeDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var idText = root.TryGetValue("id", out var idValue) ? Unquote(idValue.ToString()) : "";

            if (!Identifier.TryParse(idText, out var identifier))
                throw new FormatException($"Invalid damage type id '{idText}' in '{path}'");

            var definition = new DamageTypeDefinition
            {
                Id = identifier,

                NameKey = root.TryGetValue("nameKey", out var name) ? Unquote(name.ToString()) : ""
            };

            if (root.TryGetValue("handler", out var handler) && Identifier.TryParse(Unquote(handler.ToString()), out var handlerId))
                definition.Handler = handlerId;

            entries[identifier.ToString()] = definition;
        }

        private static string Unquote(string text) =>

            text.Length > 1 && text[0] == '"' && text[^1] == '"' ? text[1..^1] : text;
    }
}