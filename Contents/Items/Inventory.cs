using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items.Effects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Items
{
    public class Inventory(Player owner)
    {
        public Player Owner { get; } = owner;

        public ItemStack[] Slots => Owner.ItemStacks;

        public static string SlotName(int index) =>

            ("Scenes.inventory.Slot." + (index + 1)).Localized();

        public int MaxSlots
        {
            get
            {
                var count = 0;

                foreach (var slot in Slots)
                    if (slot.Unlocked)
                        count++;

                return count;
            }
        }

        public int OccupiedSlots
        {
            get
            {
                var count = 0;

                foreach (var slot in Slots)
                    if (!slot.IsEmpty && slot.Unlocked)
                        count++;

                return count;
            }
        }

        public virtual int FirstEmptySlot()
        {
            for (var index = 0; index < Slots.Length; index++)
                if (Slots[index].IsEmpty && Slots[index].Unlocked)
                    return index;

            return -1;
        }

        public virtual bool Add(Item item, int quantity = 1)
        {
            if (item == null || item == Item.Nothing || quantity <= 0)
                return false;

            var remaining = quantity;

            foreach (var slot in Slots)
            {
                if (slot.IsEmpty || slot.Item.Id != item.Id || slot.Quantity >= item.MaxStackSize)
                    continue;

                var count = Math.Min(remaining, item.MaxStackSize - slot.Quantity);

                slot.Quantity += count;

                remaining -= count;

                if (remaining == 0)
                    return true;
            }

            while (remaining > 0)
            {
                var index = FirstEmptySlot();

                if (index < 0)
                    return false;

                var count = Math.Min(remaining, item.MaxStackSize);

                Slots[index].SetItemAndQty(item, count);

                remaining -= count;
            }

            return true;
        }

        public virtual InventoryTakeResult Take(Item item, ItemStack? source = null)
        {
            if (item == null || item == Item.Nothing)
                return InventoryTakeResult.Failed;

            foreach (var slot in Slots)
            {
                if (slot.IsEmpty || slot.Item.Id != item.Id || slot.Quantity >= item.MaxStackSize)
                    continue;

                slot.Quantity += 1;

                return new InventoryTakeResult(InventoryTakeKind.Stack, Array.IndexOf(Slots, slot), slot.Quantity);
            }

            var index = FirstEmptySlot();

            if (index < 0)
                return InventoryTakeResult.Failed;

            Slots[index].SetItemAndQty(item, 1);

            Slots[index].Damage = source?.Damage ?? 0;

            return new InventoryTakeResult(InventoryTakeKind.EmptySlot, index, 1);
        }

        public virtual bool Replace(int slotIndex, Item item, ItemStack? source = null)
        {
            if (slotIndex < 0 || slotIndex >= Slots.Length || item == null || item == Item.Nothing)
                return false;

            var slot = Slots[slotIndex];

            if (!slot.IsEmpty && slot.Item.Id == item.Id)
                return true;

            slot.SetItemAndQty(item, 1);

            slot.Damage = source?.Damage ?? 0;

            return true;
        }

        public virtual Item Equip(ItemStack slot)
        {
            if (slot.IsEmpty)
                return Item.Nothing;

            var item = slot.Item;

            Item previous = Item.Nothing;

            slot.RemoveOneItem();

            if (item is Armor armor)
                previous = Owner.SetArmor(armor);
            else if (item is Weapon weapon)
                previous = Owner.SetWeapon(weapon);
            else if (item is Shield shield)
                previous = Owner.SetShield(shield);
            else if (item is Jewelry jewelry)
                previous = Owner.SetJewelry(jewelry);
            else if (item is Undergarment undergarment)
                previous = Owner.SetUndergarment(undergarment);

            if (previous != null && previous != Item.Nothing && previous != Weapon.Fists && previous != Weapon.Claws)
                Add(previous);

            return previous;
        }

        public virtual bool Use(ItemStack slot, Action<string> write, Random random)
        {
            if (slot.IsEmpty)
                return false;

            var item = slot.Item;

            if (!item.CanUse())
                return false;

            slot.RemoveOneItem();

            return item is Consumable consumable

                ? consumable.UseItem(new ItemUseContext(item, Owner, write, random))

                : item.UseItem();
        }

        public virtual bool Use(Item item, Action<string> write, Random random)
        {
            if (item == null || item == Item.Nothing || !item.CanUse())
                return false;

            return item is Consumable consumable

                ? consumable.UseItem(new ItemUseContext(item, Owner, write, random))

                : item.UseItem();
        }

        public virtual int Delete(ItemStack slot, bool all)
        {
            if (slot.IsEmpty)
                return 0;

            if (!all)
            {
                slot.RemoveOneItem();

                return 1;
            }

            var quantity = slot.Quantity;

            slot.Clear();

            return quantity;
        }

        public virtual bool Consume(Item item, int amount = 1)
        {
            if (item == null || amount <= 0 || Count(item) < amount)
                return false;

            var remaining = amount;

            foreach (var slot in Slots)
            {
                if (slot.IsEmpty || slot.Item.Id != item.Id)
                    continue;

                var count = Math.Min(remaining, slot.Quantity);

                for (var index = 0; index < count; index++)
                    slot.RemoveOneItem();

                remaining -= count;

                if (remaining == 0)
                    return true;
            }

            return remaining == 0;
        }

        public virtual int Count(Item item) => Count(item.Id);

        public virtual int Count(Identifier id)
        {
            var count = 0;

            foreach (var slot in Slots)
                if (!slot.IsEmpty && slot.Item.Id == id)
                    count += slot.Quantity;

            return count;
        }

        public virtual bool HasItem(Item item, int minimum = 1) => Count(item.Id) >= minimum;

        public virtual bool HasItem(Identifier id, int minimum = 1) => Count(id) >= minimum;

        public virtual ItemStack? SlotAt(int index) => index >= 0 && index < Slots.Length ? Slots[index] : null;
    }

    public enum InventoryTakeKind
    {
        Failed,

        Stack,

        EmptySlot
    }

    public readonly record struct InventoryTakeResult(InventoryTakeKind Kind, int Slot, int Quantity)
    {
        public bool Success => Kind != InventoryTakeKind.Failed;

        public static readonly InventoryTakeResult Failed = new(InventoryTakeKind.Failed, -1, 0);
    }
}