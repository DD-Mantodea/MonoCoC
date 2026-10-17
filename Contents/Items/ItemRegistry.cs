using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Attributes;
using MonoCoC.Registry;
using MonoCoC.Saving.Legacy;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items
{
    public static class ItemRegistry
    {
        public static Registry<ItemDefinition> Definitions => ManagerInstance<ItemManager>.Instance.Registry;

        public static readonly Dictionary<string, Func<ItemDefinition, Item>> Types = new(StringComparer.OrdinalIgnoreCase);

        static ItemRegistry() => RegistrationScanner.OnTypes(RegisterTypes);

        public static IReadOnlyCollection<Identifier> Ids => Definitions.Ids;

        public static void RegisterDefinition(ItemDefinition definition)
        {
            ManagerInstance<ItemManager>.Instance.Registry.Register(definition.Id, () => definition);

            if (definition.LegacyId != "")
            {
                ItemIdMap.Register(definition.LegacyId, definition.Id);

                var trimmed = definition.LegacyId.Trim();

                if (trimmed != definition.LegacyId)
                    ItemIdMap.Register(trimmed, definition.Id);
            }
        }

        public static void RegisterType(string type, Func<ItemDefinition, Item> factory) => Types[type] = factory;

        internal static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || !typeof(Item).IsAssignableFrom(type))
                    continue;

                var attributes = type.GetCustomAttributes(typeof(RegisterAttribute), false);

                if (attributes.Length == 0)
                    continue;

                var ctor = type.GetConstructor(Type.EmptyTypes)

                    ?? throw new InvalidOperationException($"[Register] {type.Name} 缺少空构造");

                foreach (RegisterAttribute attribute in attributes)
                {
                    Types[attribute.Id] = _ => (Item)ctor.Invoke(null);

                    count++;
                }
            }

            return count;
        }

        public static ItemDefinition? GetDefinition(Identifier id) =>

            ManagerInstance<ItemManager>.Instance.Dict.TryGetValue(id.ToString(), out var definition) ? definition : null;

        public static Item Create(Identifier id)
        {
            var definition = GetDefinition(id);

            if (definition is null)
                return CreateUnknown(id);

            return Create(definition);
        }

        public static Item Create(ItemDefinition definition)
        {
            var type = string.IsNullOrWhiteSpace(definition.Type) ? "default" : definition.Type;

            var factory = Types.TryGetValue(type, out var registered) ? registered : Types["default"];

            var item = factory(definition);

            item.Configure(definition);

            return item;
        }

        public static Item Resolve(string saved)
        {
            if (string.IsNullOrWhiteSpace(saved))
                return Item.Nothing;

            if (!ItemIdMap.Resolve(saved, out var id))
                return Item.Nothing;

            if (id == Item.Nothing.Id)
                return Item.Nothing;

            if (id == Weapon.Fists.Id)
                return Weapon.Fists;

            if (id == Weapon.Claws.Id)
                return Weapon.Claws;

            return Create(id);
        }

        public static bool TryCreate(string saved, out Item item)
        {
            item = Resolve(saved);

            return item != Item.Nothing;
        }

        public static Item CreateUnknown(Identifier id) => new()
        {
            Id = id,

            Name = id.Path,

            LongName = id.Path
        };
    }
}