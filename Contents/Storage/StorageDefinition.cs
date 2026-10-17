using MonoCoC.Contents.Items;
using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Storage
{
    public class StorageDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string NameKey;

        public int SlotCount;

        public Func<Item, bool> Accepts;

        public Func<Player, bool> IsAvailable;

        public bool Fallback;

        public int Order;

        public StorageDefinition(
            Identifier id,

            string nameKey,

            int slotCount,

            Func<Item, bool> accepts,

            Func<Player, bool> isAvailable,

            bool fallback,

            int order)
        {
            Id = id;

            NameKey = nameKey;

            SlotCount = slotCount;

            Accepts = accepts;

            IsAvailable = isAvailable;

            Fallback = fallback;

            Order = order;
        }
    }
}