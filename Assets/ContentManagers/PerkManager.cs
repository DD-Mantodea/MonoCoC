using Hjson;
using MonoCoC.Contents.Perks;
using MonoCoC.Registry;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Assets.ContentManagers
{
    public class PerkManager : ContentManager<PerkType>
    {
        public override string Folder => "Data/Perks";

        public override string Pattern => "*.hjson";

        public override void FromText(string path, string text, IDictionary<string, PerkType> entries)
        {
            var root = (JsonObject)HjsonValue.Parse(text);

            // 同一个 Data/Perks 目录里可以放可购买定义（purchasable 数组），交给现有的专长目录系统
            PerkPurchaseCatalog.LoadFromJson(root);

            var perks = new List<PerkType>();

            if (!root.TryGetValue("perks", out var value) || value is not JsonArray array)
                return;

            foreach (var entry in array)
            {
                if (entry is not JsonObject json)
                    continue;

                var id = Str(json, "id");

                if (Identifier.TryParse(id, out var identifier))
                    perks.Add(new PerkType { Id = identifier });
            }

            foreach (var perk in perks)
                entries[perk.Id.ToString()] = perk;
        }

        protected override void Store(string key, PerkType value)
        {
            if (Registry.Has(value.Id))
                return;

            base.Store(key, value);
        }

        private static string Str(JsonObject root, string key, string fallback = "")
        {
            if (!root.TryGetValue(key, out var value) || value == null)
                return fallback;

            return value.ToString().Trim('"');
        }
    }
}