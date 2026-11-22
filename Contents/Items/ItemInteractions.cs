using MonoCoC.Contents.Equipment;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Storage;
using MonoCoC.Extensions;
using MonoCoC.UI.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Items
{
    public enum InventoryPage
    {
        Backpack,
        Equipment,
        Storage
    }

    public sealed class ItemInteractionContext
    {
        public required Item Item;

        public ItemStack? Stack;

        public string? EquipmentSlotId;

        public InventoryPage Page = InventoryPage.Backpack;

        public StorageArea? StorageArea;

        public int StorageIndex = -1;
    }

    // 一条交互：由物品类型在自己的 RegisterInteractions 里 Add 进来。
    public sealed class ItemInteraction
    {
        public required string Id;

        public required string LabelKey;

        public Func<ItemInteractionContext, string?> Invoke = _ => null;

        public int Order;
    }

    // 物品类型往这里注册自己的交互；UI 只按顺序生成菜单。
    public sealed class ItemInteractionMenu
    {
        private readonly List<ItemInteraction> _entries = [];

        public void Add(string id, string labelKey, Func<ItemInteractionContext, string?> invoke, int order = 0) =>
            _entries.Add(new ItemInteraction
            {
                Id = id,

                LabelKey = labelKey,

                Invoke = invoke,

                Order = order
            });

        public List<ItemInteraction> Ordered() => _entries.OrderBy(entry => entry.Order).ToList();
    }

    // 公共动作实现 + 建菜单入口。物品类型自己决定"我有哪些交互"，这里不按类别判断。
    public static class ItemInteractions
    {
        public static List<ItemInteraction> Build(ItemInteractionContext context)
        {
            var menu = new ItemInteractionMenu();

            context.Item.RegisterInteractions(menu, context);

            return menu.Ordered();
        }

        public static List<ItemInteraction> BuildStorage(ItemInteractionContext context)
        {
            var menu = new ItemInteractionMenu();

            if (context.Stack is not null)
            {
                if (context.StorageArea is not null && context.StorageIndex >= 0)
                    menu.Add("take_out", "Scenes.Storage.TakeOut", TakeOut, order: 0);
                else
                    menu.Add("store", "Scenes.Storage.Store", Store, order: 0);
            }

            return menu.Ordered();
        }

        public static string? Store(ItemInteractionContext context)
        {
            var index = Array.IndexOf(Player.Inventory.Slots, context.Stack);

            if (index < 0)
                return null;

            if (context.StorageArea is null)
                return "Scenes.Storage.Error.NoSection".Localized();

            if (!context.StorageArea.AcceptsItem(context.Item))
                return "Scenes.Storage.Error.TypeRejected".Localized();

            StorageScene.DepositTo(index, context.StorageArea);

            return null;
        }

        public static string? TakeOut(ItemInteractionContext context)
        {
            if (context.StorageArea is null || context.StorageIndex < 0)
                return null;

            StorageScene.Withdraw(context.StorageArea, context.StorageIndex);

            return null;
        }

        // 可装备类（Weapon/Shield/Armor/Jewelry/Undergarment）在自己的 RegisterInteractions 里调这个。
        public static void RegisterEquip(ItemInteractionMenu menu, ItemInteractionContext context)
        {
            if (context.Stack is not null)
                menu.Add("equip", "Scenes.Inventory.Equip", Equip, order: 0);
        }

        // 可使用类（Consumable/Useable）在自己的 RegisterInteractions 里调这个。
        public static void RegisterUse(ItemInteractionMenu menu, ItemInteractionContext context)
        {
            if (context.Stack is not null && context.Item.CanUse())
                menu.Add("use", "Scenes.Inventory.Use", Use, order: 0);
        }

        public static string? Use(ItemInteractionContext context)
        {
            var message = "";

            var used = Player.Inventory.Use(context.Stack!, text => message = text, Random.Shared);

            return used ? message : "Scenes.Inventory.CannotUse".Localized(context.Item.LongName);
        }

        public static string? Equip(ItemInteractionContext context)
        {
            Player.Inventory.Equip(context.Stack!);

            return Player.Inventory.LastEquipError.Length > 0
                ? Player.Inventory.LastEquipError.Localized()
                : "Scenes.Inventory.Equipped".Localized(context.Item.LongName);
        }

        public static string? Delete(ItemInteractionContext context)
        {
            var stack = context.Stack;

            if (stack is null)
                return null;

            ConfirmDialogOverlay.Show(
                "Scenes.Inventory.Delete".Localized(),
                "Scenes.Inventory.DeleteConfirm".Localized(stack.InvLabel),
                () => Player.Inventory.Delete(stack, true));

            return null;
        }

        public static string? Drop(ItemInteractionContext context)
        {
            var deleted = Player.Inventory.Delete(context.Stack!, false);

            return deleted > 0 ? "Scenes.Inventory.DeletedOne".Localized(context.Item.LongName) : null;
        }

        public static string? Unequip(ItemInteractionContext context)
        {
            if (context.EquipmentSlotId is not { } slotId)
                return null;

            var spec = EquipmentCatalog.Compose(Player.EquipmentTags).FirstOrDefault(candidate => candidate.Id == slotId);

            Item? removed = null;

            if (spec is { IsExtra: true })
            {
                Player.ExtraEquipment.Remove(slotId, out removed);

                Player.SyncHandDerived();
            }
            else
                removed = spec?.Kind switch
                {
                    "weapon" => Player.SetWeapon(null),
                    "shield" => Player.SetShield(null),
                    "armor" => Player.SetArmor(null),
                    "jewelry" => Player.SetJewelry(null),
                    "upper" => Player.SetUndergarment(null, Undergarment.TypeUpperwear),
                    "lower" => Player.SetUndergarment(null, Undergarment.TypeLowerwear),
                    _ => null
                };

            if (removed is null || removed == Item.Nothing)
                return "Scenes.Inventory.NothingEquipped".Localized();

            Player.Inventory.Add(removed);

            return "Scenes.Inventory.Unequipped".Localized(removed.LongName);
        }

        private static Creatures.Player Player => MonoCoC.CoC.Instance.Player;
    }
}
