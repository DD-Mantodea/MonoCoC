using MonoCoC.Contents.Items;
using Hjson;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Storage
{
    public class StorageArea
    {
        public StorageDefinition Definition;

        public List<ItemStack> Slots = [];

        public int Capacity => Slots.Count;

        public StorageArea(StorageDefinition definition)
        {
            Definition = definition;

            Resize(definition.SlotCount);
        }

        public void Resize(int count)
        {
            count = Math.Max(0, count);

            while (Slots.Count < count)

                Slots.Add(new ItemStack());

            while (Slots.Count > count)

                Slots.RemoveAt(Slots.Count - 1);
        }

        public int OccupiedSlots()
        {
            var count = 0;

            foreach (var slot in Slots)
                if (!slot.IsEmpty)
                    count++;

            return count;
        }

        public int FirstEmptySlot()
        {
            for (var index = 0; index < Slots.Count; index++)
                if (Slots[index].IsEmpty)
                    return index;

            return -1;
        }

        public bool AcceptsItem(Item item) => item != null && item != Item.Nothing && Definition.Accepts(item);

        public void Save(JsonObject json)
        {
            var slotsJson = new JsonArray();

            foreach (var slot in Slots)
            {
                var slotJson = new JsonObject();

                slot.Save(slotJson);

                slotsJson.Add(slotJson);
            }

            json["slots"] = slotsJson;
        }

        public void Load(JsonObject json)
        {
            Slots.Clear();

            if (json.TryGetValue("slots", out var slotsValue) && slotsValue is JsonArray slotsArray)
                foreach (var slotValue in slotsArray)
                {
                    var slot = new ItemStack();

                    if (slotValue is JsonObject slotJson)
                        slot.Load(slotJson);

                    Slots.Add(slot);
                }

            Resize(Math.Max(Definition.SlotCount, Slots.Count));
        }

        public void LoadSlots(JsonArray slotsArray)
        {
            Slots.Clear();

            foreach (var slotValue in slotsArray)
            {
                var slot = new ItemStack();

                if (slotValue is JsonObject slotJson)
                    slot.Load(slotJson);

                Slots.Add(slot);
            }

            Resize(Math.Max(Definition.SlotCount, Slots.Count));
        }
    }
}