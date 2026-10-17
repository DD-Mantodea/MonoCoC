using Hjson;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.IO;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Tags;
using MonoCoC.Extensions;

namespace MonoCoC.Assets.ContentManagers
{
    public class NpcManager : ContentManager<NpcDefinition>
    {
        public override string Folder => "Data/Npcs";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, NpcDefinition> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            var id = Str(root, "id");

            if (!Identifier.TryParse(id, out var identifier))
                throw new FormatException($"Invalid npc id '{id}' in '{path}'");

            var npc = new NpcDefinition
            {
                Id = identifier,

                NameKey = Str(root, "nameKey"),

                DefaultHostile = Bool(root, "hostile")
            };

            if (Identifier.TryParse(Str(root, "scene"), out var scene))
                npc.SceneId = scene;

            if (Identifier.TryParse(Str(root, "combat"), out var combat))
                npc.CombatId = combat;

            npc.Tags = TagSet.NormalizeAll(root.Strings("tags"));

            entries[identifier.ToString()] = npc;
        }

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            var text = value.ToString();

            if (text.Length > 1 && text[0] == '"' && text[^1] == '"')
                text = text[1..^1];

            return text;
        }

        private static bool Bool(JsonObject root, string key, bool fallback = false)
        {
            var value = Str(root, key);

            return value == "" ? fallback : bool.TryParse(value, out var result) && result;
        }
    }
}