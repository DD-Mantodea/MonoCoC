using Hjson;
using MonoCoC.Contents.Tags;
using MonoCoC.Registry;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class TagRuleManager : ContentManager<TagRule>
    {
        public override string Folder => "Data/TagRules";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, TagRule> entries)
        {
            var value = HjsonValue.Parse(text);

            if (value is not JsonObject root || !root.TryGetValue("rules", out var rules) || rules is not JsonArray list)
                return;

            foreach (var entry in list)
            {
                if (entry is not JsonObject json)
                    continue;

                var rule = new TagRule
                {
                    Id = new Identifier("coc", (string)json["id"]),
                };

                ReadList(json, "any", rule.Any);

                ReadList(json, "always", rule.Always);

                ReadList(json, "never", rule.Never);

                ReadList(json, "all", rule.All);

                entries[rule.Id.ToString()] = rule;
            }
        }

        private static void ReadList(JsonObject json, string key, List<string> target)
        {
            if (!json.TryGetValue(key, out var value) || value is not JsonArray array)
                return;

            foreach (var item in array)
                target.Add(TagSet.Normalize((string)item));
        }
    }
}