using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true)]
    public sealed class ItemCategoryAttribute(string id, string icon, string labelKey) : Attribute
    {
        public string Id { get; } = id;

        public string Icon { get; } = icon;

        public string LabelKey { get; } = labelKey;
    }

    public sealed class ItemCategory(string id, string icon, string labelKey)
    {
        public string Id { get; } = id;

        public string Icon { get; } = icon;

        public string LabelKey { get; } = labelKey;
    }

    public static class ItemCategories
    {
        private static readonly Dictionary<Type, ItemCategory?> Cache = [];

        public static readonly ItemCategory Misc = new("coc:misc", "Icons/inventory_misc.svg", "Items.Type.misc");

        public static ItemCategory Resolve(Item item)
        {
            var type = item.GetType();

            if (Cache.TryGetValue(type, out var cached))
                return cached ?? Misc;

            for (var current = type; current is not null; current = current.BaseType)
            {
                var attribute = (ItemCategoryAttribute?)Attribute.GetCustomAttribute(current, typeof(ItemCategoryAttribute));

                if (attribute is null)
                    continue;

                var category = new ItemCategory(attribute.Id, attribute.Icon, attribute.LabelKey);

                Cache[type] = category;

                return category;
            }

            Cache[type] = null;

            return Misc;
        }
    }
}
