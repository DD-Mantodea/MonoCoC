using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Attributes;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<InventoryScene>("coc:inventory")]
    public class InventoryScene : Scene
    {
        public static Action? _returnTo;

        public static Action? _itemUsed;

        public override void Entry() => InventoryMenu();

        public static void InventoryMenu() => InventoryMenu(null, null);

        public static void InventoryMenu(Action? returnTo, Action? itemUsed = null)
        {
            _returnTo = returnTo;

            _itemUsed = itemUsed;

            Player.UpdateItemStacks();

            ClearOutput();

            DisplayHeader("Scenes.Inventory.Title".Localized());

            OutputText("Scenes.Inventory.Equipment".Localized() + "\n");

            OutputText("Scenes.Inventory.Weapon".Localized(ItemName(Player.Weapon), Player.WeaponAttack) + "\n");

            OutputText("Scenes.Inventory.Shield".Localized(ItemName(Player.Shield), Player.Shield.Block) + "\n");

            OutputText("Scenes.Inventory.Armor".Localized(ItemName(Player.Armor), Player.ArmorDefense) + "\n");

            OutputText("Scenes.Inventory.UpperGarment".Localized(ItemName(Player.UpperGarment)) + "\n");

            OutputText("Scenes.Inventory.LowerGarment".Localized(ItemName(Player.LowerGarment)) + "\n");

            OutputText("Scenes.Inventory.Accessory".Localized(ItemName(Player.Jewelry)) + "\n");

            if (Player.KeyItems.Count > 0)
            {
                OutputText("Scenes.Inventory.KeyItems".Localized() + "\n");

                foreach (var keyItem in Player.KeyItems.Values.OrderByDescending(item => item.DisplayName))
                    OutputText("Scenes.Inventory.KeyItem".Localized(keyItem.DisplayName) + "\n");
            }

            ClearButtons();

            var foundItem = false;

            for (var index = 0; index < Player.Inventory.Slots.Length; index++)
            {
                var slotIndex = index;

                var slot = Player.Inventory.Slots[index];

                if (!slot.Unlocked)
                    continue;

                if (!slot.IsEmpty)
                {
                    AddButton(index, slot.InvLabel, () => UseSlot(slotIndex), slot.TooltipText, slot.TooltipHeader);

                    foundItem = true;
                }
                else
                    AddButtonDisabled(index, "Scenes.Inventory.Nothing".Localized());
            }

            if (!foundItem)
            {
                OutputText("Scenes.Inventory.NoItems".Localized());

                AddButton(14, "Common.Back".Localized(), Exit);
            }
            else
            {
                AddButton(10, "Scenes.Inventory.Unequip".Localized(), ManageEquipment);

                AddButton(11, DeleteLabel().Localized(), ToggleDelete, DeleteHint().Localized());

                OutputText("Scenes.Inventory.Capacity".Localized(Player.Inventory.OccupiedSlots, Player.Inventory.MaxSlots) + "\n");

                AddButton(14, "Common.Back".Localized(), Exit);
            }
        }

        private static string ItemName(Item item) =>

            item.ShortNameKey.Length > 0 ? item.ShortNameKey.Localized() : item.Name;

        public static string DeleteLabel() => Flags.Get(GameFlagIds.DeleteItems) switch
        {
            1 => "Scenes.Inventory.DeleteOne",

            2 => "Scenes.Inventory.DeleteAll",

            _ => "Scenes.Inventory.DeleteOff"
        };

        public static string DeleteHint() => Flags.Get(GameFlagIds.DeleteItems) switch
        {
            1 => "Scenes.Inventory.DeleteOneHint",

            2 => "Scenes.Inventory.DeleteAllHint",

            _ => "Scenes.Inventory.DeleteOffHint"
        };

        public static void ToggleDelete()
        {
            Flags.Set(GameFlagIds.DeleteItems, (Flags.Get(GameFlagIds.DeleteItems) + 1) % 3);

            InventoryMenu();
        }

        public static void UseSlot(int index)
        {
            var slot = Player.Inventory.Slots[index];

            if (slot.IsEmpty)
                return;

            var item = slot.Item;

            var mode = Flags.Get(GameFlagIds.DeleteItems);

            if (mode == 1)
            {
                DeletePrompt(item, slot, false);

                return;
            }

            if (mode == 2)
            {
                DeletePrompt(item, slot, true);

                return;
            }

            if (item is Armor || item is Weapon || item is Shield || item is Jewelry || item is Undergarment)
            {
                Equip(slot);

                return;
            }

            if (item is Useable || item is Consumable)
            {
                ClearOutput();

                ClearButtons();

                var used = Player.Inventory.Use(slot, text => OutputText(text), Random.Shared);

                if (used && _itemUsed != null)
                {
                    var itemUsed = _itemUsed;

                    _itemUsed = null;

                    _returnTo = null;

                    itemUsed();

                    return;
                }

                if (!used)
                    OutputText("Scenes.Inventory.CannotUse".Localized(item.LongName));

                AddButton(0, "Common.OK".Localized(), InventoryMenu);

                return;
            }

            ClearOutput();

            OutputText("Scenes.Inventory.CannotUse".Localized(item.LongName));

            AddButton(0, "Common.OK".Localized(), InventoryMenu);
        }

        public static void Equip(ItemStack slot)
        {
            var previous = Player.Inventory.Equip(slot);

            if (previous != null && previous != Item.Nothing &&
                previous != Weapon.Fists && previous != Weapon.Claws &&

                !Player.Inventory.HasItem(previous))
            {
                ClearOutput();

                OutputText("Scenes.Inventory.NoRoom".Localized(previous.LongName));

                AddButton(0, "Common.OK".Localized(), InventoryMenu);

                return;
            }

            InventoryMenu();
        }

        public static void DeletePrompt(Item item, ItemStack slot, bool stack)
        {
            ClearOutput();

            if (stack)
                OutputText("Scenes.Inventory.DeleteAllPrompt".Localized(slot.Quantity, item.LongName, slot.Quantity == 1 ? "it" : "them"));
            else
                OutputText("Scenes.Inventory.DeleteOnePrompt".Localized(item.LongName));

            ClearButtons();

            AddButton(0, "Scenes.Inventory.Yes".Localized(), () => Delete(slot, stack));

            AddButton(1, "Scenes.Inventory.No".Localized(), InventoryMenu);
        }

        public static void Delete(ItemStack slot, bool stack)
        {
            var item = slot.Item;

            var quantity = Player.Inventory.Delete(slot, stack);

            ClearOutput();

            if (stack)
                OutputText("Scenes.Inventory.DeletedAll".Localized(quantity, item.LongName));
            else
                OutputText("Scenes.Inventory.DeletedOne".Localized(item.LongName));

            AddButton(0, "Common.OK".Localized(), InventoryMenu);
        }

        public static void ManageEquipment()
        {
            ClearOutput();

            OutputText("Scenes.Inventory.WhichUnequip".Localized() + "\n");

            ClearButtons();

            AddButton(0, "Scenes.Inventory.EquipmentWeapon".Localized(), UnequipWeapon)
                .DisableIf(Player.Weapon.IsUnarmed(), "Scenes.Inventory.NothingToUnequip".Localized());

            AddButton(1, "Scenes.Inventory.EquipmentShield".Localized(), UnequipShield)
                .DisableIf(Player.Shield == Shield.Nothing, "Scenes.Inventory.NothingToUnequip".Localized());

            AddButton(2, "Scenes.Inventory.EquipmentAccessory".Localized(), UnequipJewelry)
                .DisableIf(Player.Jewelry == Jewelry.Nothing, "Scenes.Inventory.NothingToUnequip".Localized());

            AddButton(5, "Scenes.Inventory.EquipmentArmor".Localized(), UnequipArmor)
                .DisableIf(Player.Armor == Armor.Nothing, "Scenes.Inventory.NothingToUnequip".Localized());

            AddButton(6, "Scenes.Inventory.EquipmentUpper".Localized(), UnequipUpperGarment)
                .DisableIf(Player.UpperGarment == Undergarment.Nothing, "Scenes.Inventory.NothingToUnequip".Localized());

            AddButton(7, "Scenes.Inventory.EquipmentLower".Localized(), UnequipLowerGarment)
                .DisableIf(Player.LowerGarment == Undergarment.Nothing, "Scenes.Inventory.NothingToUnequip".Localized());

            AddButton(14, "Common.Back".Localized(), InventoryMenu);
        }

        public static void UnequipWeapon() => ReturnItem(Player.SetUnarmed());

        public static void UnequipShield() => ReturnItem(Player.SetShield(Shield.Nothing));

        public static void UnequipJewelry() => ReturnItem(Player.SetJewelry(Jewelry.Nothing));

        public static void UnequipArmor() => ReturnItem(Player.SetArmor(Armor.Nothing));

        public static void UnequipUpperGarment() => ReturnItem(Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeUpperwear));

        public static void UnequipLowerGarment() => ReturnItem(Player.SetUndergarment(Undergarment.Nothing, Undergarment.TypeLowerwear));

        public static void ReturnItem(Item item)
        {
            if (item == null || item == Item.Nothing || item == Weapon.Fists || item == Weapon.Claws)
            {
                InventoryMenu();

                return;
            }

            if (!Player.AddItem(item))
            {
                ClearOutput();

                OutputText("Scenes.Inventory.NoRoom".Localized(item.LongName));

                AddButton(0, "Common.OK".Localized(), InventoryMenu);

                return;
            }

            InventoryMenu();
        }

        public static void TakeItem(
            Item item,

            Action? next = null,

            Action? abandon = null,

            ItemStack? source = null,

            bool display = true) =>

            TakeItem(item, next, abandon, source, display, true);

        public static void TakeItem(
            Identifier id,

            Action? next = null,

            Action? abandon = null,

            ItemStack? source = null,

            bool display = true) =>

            TakeItem(ItemRegistry.Create(id), next, abandon, source, display, true);

        public static void TakeItemNoNext(
            Item item,

            Action? next = null,

            Action? abandon = null,

            ItemStack? source = null,

            bool display = true) =>

            TakeItem(item, next, abandon, source, display, false);

        public static void TakeItems(
            Item[] items,

            Action? next = null,

            Action? abandon = null,

            bool display = true)
        {
            next ??= InventoryMenu;

            if (items.Length == 0)
            {
                next();

                return;
            }

            if (items.Length == 1)
            {
                TakeItem(items[0], next, abandon, null, display);

                return;
            }

            TakeItemNoNext(
                items[0],

                () => TakeItems(items[1..], next, abandon, display),

                abandon,

                null,

                display);
        }

        public static void TakeItem(
            Item item,

            Action? next,

            Action? abandon,

            ItemStack? source,

            bool display,

            bool showNext)
        {
            next ??= InventoryMenu;

            var result = Player.Inventory.Take(item, source);

            if (result.Success)
            {
                if (display)
                    OutputText(result.Kind == InventoryTakeKind.Stack

                        ? "Scenes.Inventory.TakeStack".Localized(item.LongName, Inventory.SlotName(result.Slot), result.Quantity)

                        : "Scenes.Inventory.TakeEmpty".Localized(item.LongName, Inventory.SlotName(result.Slot)));

                if (showNext)
                    AddButton(0, "Common.Continue".Localized(), next);
                else
                    next();

                return;
            }

            ClearButtons();

            OutputText("Scenes.Inventory.TakeFull".Localized(item.LongName));

            for (var index = 0; index < Player.Inventory.Slots.Length; index++)
            {
                var slot = Player.Inventory.Slots[index];

                if (!slot.Unlocked || slot.IsEmpty)
                    continue;

                AddButton(index, slot.InvLabel, () => Replace(index, item, next, source), slot.TooltipText, slot.TooltipHeader);
            }

            if ((item is Useable || item is Consumable) && !item.InvUseOnly && item.CanUse())
                AddButton(13, "Scenes.Inventory.UseNow".Localized(), () => UseNow(item, next));

            AddButton(14, "Scenes.Inventory.Abandon".Localized(), abandon ?? next);
        }

        public static void UseNow(Item item, Action next)
        {
            ClearOutput();

            ClearButtons();

            var used = Player.Inventory.Use(item, text => OutputText(text), Random.Shared);

            if (!used)
            {
                OutputText("Scenes.Inventory.CannotUse".Localized(item.LongName));

                AddButton(0, "Common.OK".Localized(), next);

                return;
            }

            AddButton(0, "Common.Continue".Localized(), next);
        }

        public static void Replace(int index, Item item, Action next, ItemStack? source)
        {
            ClearOutput();

            ClearButtons();

            Player.Inventory.Replace(index, item, source);

            OutputText("Scenes.Inventory.TakeReplace".Localized(item.LongName, index + 1));

            AddButton(0, "Common.Continue".Localized(), next);
        }

        public static void Exit()
        {
            Flags.Set(GameFlagIds.DeleteItems, 0);

            _itemUsed = null;

            if (_returnTo != null)
            {
                var returnTo = _returnTo;

                _returnTo = null;

                returnTo();

                return;
            }

            Go<CampScene>();
        }
    }
}
