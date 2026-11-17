using MonoCoC.Contents.Items;
using MonoCoC.Registry;
using System;

namespace MonoCoC.UI.Components.Views
{
    public partial class InventoryView
    {
        public static void Open(Action? returnTo = null, Action? itemUsed = null) =>
            InventoryFlow.Open(returnTo, itemUsed);

        public static void InventoryMenu() => InventoryFlow.InventoryMenu();

        public static void InventoryMenu(Action? returnTo, Action? itemUsed = null) =>
            InventoryFlow.InventoryMenu(returnTo, itemUsed);

        public static void TakeItem(Item item, Action? next = null, Action? abandon = null,
            ItemStack? source = null, bool display = true) =>
            InventoryFlow.TakeItem(item, next, abandon, source, display);

        public static void TakeItem(Identifier id, Action? next = null, Action? abandon = null,
            ItemStack? source = null, bool display = true) =>
            InventoryFlow.TakeItem(id, next, abandon, source, display);

        public static void TakeItemNoNext(Item item, Action? next = null, Action? abandon = null,
            ItemStack? source = null, bool display = true) =>
            InventoryFlow.TakeItemNoNext(item, next, abandon, source, display);

        public static void TakeItems(Item[] items, Action? next = null, Action? abandon = null,
            bool display = true) =>
            InventoryFlow.TakeItems(items, next, abandon, display);

        public static void Exit() => InventoryFlow.Exit();
    }
}
