using Hjson;
using MonoCoC.Assets;
using MonoCoC.Registry;
using System.Collections.Generic;
using System.IO;

namespace MonoCoC.Contents.Shops
{

    public record ShopEntry(Identifier Item, int Stock, int PriceOverride);

    public record ShopDefinition(
        Identifier Id,

        string TitleKey,

        string DescriptionKey,

        float Markup,

        IReadOnlyList<ShopEntry> Items);

    public static class ShopCatalog
    {
        public static Dictionary<Identifier, ShopDefinition>? _shops;

        public static ShopDefinition? Get(Identifier id)
        {
            Load();

            return _shops!.TryGetValue(id, out var shop) ? shop : null;
        }

        public static IReadOnlyCollection<ShopDefinition> All
        {
            get
            {
                Load();

                return _shops!.Values;
            }
        }

        public static void Load()
        {
            if (_shops != null)
                return;

            _shops = [];

            foreach (var file in Paths.GetAssets("Data/Shops", "*.hjson"))
                try
                {
                    var shop = Parse((JsonObject)HjsonValue.Parse(File.ReadAllText(file)));

                    if (shop != null)
                        _shops[shop.Id] = shop;
                }
                catch
                {

                }
        }

        public static ShopDefinition? Parse(JsonObject root)
        {
            var idText = root.TryGetValue("id", out var idValue) ? (string)idValue : null;

            if (string.IsNullOrEmpty(idText))
                return null;

            var title = root.TryGetValue("titleKey", out var t) ? (string)t : "";

            var desc = root.TryGetValue("descriptionKey", out var d) ? (string)d : "";

            var markup = root.TryGetValue("markup", out var m) ? (float)m : 1f;

            var entries = new List<ShopEntry>();

            if (root.TryGetValue("items", out var itemsValue) && itemsValue is JsonArray items)
                foreach (var item in items)
                {
                    if (item is not JsonObject o)
                        continue;

                    if (!o.TryGetValue("item", out var itemId) || itemId is null)
                        continue;

                    var stock = o.TryGetValue("stock", out var s) ? (int)s : 0;

                    var price = o.TryGetValue("price", out var p) ? (int)p : -1;

                    entries.Add(new ShopEntry(Identifier.Parse((string)itemId), stock, price));
                }

            return new ShopDefinition(Identifier.Parse(idText), title, desc, markup, entries);
        }
    }
}