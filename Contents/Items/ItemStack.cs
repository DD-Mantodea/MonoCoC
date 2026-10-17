using Hjson;
using MonoCoC.Extensions;
using System.Globalization;

namespace MonoCoC.Contents.Items
{
    public class ItemStack
    {

        public Item Item = Item.Nothing;

        public int Quantity;

        public int Damage;

        public bool Unlocked;

        public bool IsEmpty => Item is null || Item.Id.Equals(Item.Nothing.Id) || Quantity <= 0;

        public string InvLabel => IsEmpty ? "" : Item.MaxStackSize > 1

            ? DisplayName + " x" + Quantity

            : DisplayName;

        private string DisplayName => Item.ShortNameKey.Length > 0

            ? Item.ShortNameKey.Localized()

            : Item.Name;

        public string TooltipText
        {
            get
            {
                if (IsEmpty)
                    return "";

                var type = Item switch
                {
                    Weapon => "Weapon",

                    Armor => "Armor",

                    Shield => "Shield",

                    Undergarment => "Undergarment",

                    Jewelry => "Jewelry",

                    Consumable => "Consumable",

                    Useable => "Consumable",

                    _ => "Misc",
                };

                var typeName = ("Items.Type." + type).LocalizedOr(type);

                return Item.TooltipText

                    + "\n\n" + "Items.Tooltip.Type".Localized(typeName)

                    + "\n" + "Items.Tooltip.BaseValue".Localized(
                        Item.Value.ToString("0.##", CultureInfo.InvariantCulture));
            }
        }

        public string TooltipHeader => IsEmpty ? "" : Item.TooltipHeader;

        public void SetItemAndQty(Item item, int quantity)
        {
            item ??= Item.Nothing;

            if (quantity == 0 && item == Item.Nothing)
            {
                Clear();

                return;
            }

            if (quantity < 0 || (quantity == 0 && item != Item.Nothing) || (quantity > 0 && item == Item.Nothing))
            {
                quantity = 0;

                item = Item.Nothing;
            }

            Quantity = quantity;

            Item = item;
        }

        public void Clear()
        {
            Quantity = 0;

            Item = Item.Nothing;
        }

        public void RemoveOneItem()
        {
            if (Quantity <= 0)
                return;

            Quantity -= 1;

            if (Quantity == 0)
                Item = Item.Nothing;
        }

        public void Save(JsonObject json)
        {
            json["item"] = Item == null ? "" : Item.Id.ToString();

            json["quantity"] = Quantity;

            json["damage"] = Damage;

            json["unlocked"] = Unlocked;
        }

        public void Load(JsonObject json)
        {
            var saved = json.TryGetValue("item", out var itemValue) ? (string)itemValue : "";

            Unlocked = json.TryGetValue("unlocked", out var unlocked) && (bool)unlocked;

            Item = ItemRegistry.Resolve(saved);

            if (Item == Item.Nothing)
            {
                Quantity = 0;

                Damage = 0;

                return;
            }

            Quantity = json.TryGetValue("quantity", out var quantity) ? (int)quantity : 0;

            Damage = json.TryGetValue("damage", out var damage) ? (int)damage : 0;
        }
    }
}