using static MonoCoC.Contents.Scenes.Scene;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;
using System.Linq;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Items
{
    public static class InventoryFlow
    {
        public static Action? _returnTo;

        public static Action? _itemUsed;

        private static MonoCoC.UI.Components.View? _returnView;

        public static void Open(Action? returnTo = null, Action? itemUsed = null)
        {
            MonoCoC.UI.Components.Combat.PlayerStats.HiddenByMenu = false;

            var root = MonoCoC.UI.Framework.UIManager.Instance?.Root;

            if (root is not null && root.View is not MonoCoC.UI.Components.Views.InventoryScreen)
                _returnView = root.View;

            root?.SwitchView(new InventoryScreen());

            InventoryMenu(returnTo, itemUsed);
        }

        public static void InventoryMenu() => InventoryMenu(null, null);

        public static void InventoryMenu(Action? returnTo, Action? itemUsed = null)
        {
            _returnTo = returnTo;

            _itemUsed = itemUsed;

            Player.UpdateItemStacks();

            ClearOutput();

            ClearButtons();

            var foundItem = false;

            for (var index = 0; index < Player.Inventory.Slots.Length; index++)
            {
                if (!Player.Inventory.Slots[index].Unlocked)
                    continue;

                if (!Player.Inventory.Slots[index].IsEmpty)
                    foundItem = true;
            }

            if (!foundItem)
            {
                OutputText("Scenes.Inventory.NoItems".Localized());

            }
            else
            {
            }
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
            _itemUsed = null;

            if (_returnTo != null)
            {
                var returnTo = _returnTo;

                _returnTo = null;

                returnTo();

                return;
            }

            var root = MonoCoC.UI.Framework.UIManager.Instance?.Root;

            var returnView = _returnView;

            _returnView = null;

            if (returnView is not null && root is not null)
            {
                root.SwitchView(returnView);

                return;
            }

            MonoCoC.UI.Components.Views.GameView.SwitchTo<CampView>();
        }
    }
}
