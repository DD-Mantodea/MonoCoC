using MonoCoC.Contents.Items;
using MonoCoC.Contents.Storage;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World;
using MonoCoC.Extensions;
using MonoCoC.UI;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Linq;
using static MonoCoC.UI.Layout.LayoutConstants;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<StorageScene>("coc:storage")]
    public class StorageScene : Scene
    {
        public override void Entry()
        {
            if (MonoCoC.UI.Framework.UIManager.Instance?.Root?.View is not MonoCoC.UI.Components.Views.StorageView)
                MonoCoC.UI.Components.Views.GameView.SwitchTo<MonoCoC.UI.Components.Views.StorageView>();

            Player.FixStorage();

            MonoCoC.UI.Components.Views.StorageView.Active?.RefreshTabs();
        }

        public static System.Collections.Generic.List<StorageArea> Sections()
        {
            return Player.Storage.Values
                .Where(area => area.Definition.IsAvailable(Player))
                .OrderBy(area => area.Definition.Order)
                .ToList();
        }

        public static void DepositTo(int position, StorageArea section, int request = 1)
        {
            var slot = Player.ItemStacks[position];

            if (slot.IsEmpty || !section.AcceptsItem(slot.Item))
                return;

            var storageIndex = section.FirstEmptySlot();

            if (storageIndex < 0)
            {
                MonoCoC.UI.Components.Views.StorageView.BeginReplaceDeposit(position);

                return;
            }

            var quantity = Math.Clamp(request, 1, slot.Quantity);

            if (Player.StorageMoveAll)
                quantity = slot.Quantity;

            section.Slots[storageIndex].SetItemAndQty(slot.Item, quantity);

            for (var index = 0; index < quantity; index++)
                slot.RemoveOneItem();
        }

        public static void Withdraw(StorageArea section, int storageIndex, int request = 1)
        {
            if (storageIndex < 0 || storageIndex >= section.Slots.Count)
                return;

            var slot = section.Slots[storageIndex];

            if (slot.IsEmpty)
                return;

            var item = slot.Item;

            var quantity = Math.Clamp(request, 1, slot.Quantity);

            if (Player.StorageMoveAll)
                quantity = slot.Quantity;

            var moved = 0;

            while (moved < quantity && !slot.IsEmpty)
            {
                if (!Player.AddItem(item, 1))
                    break;

                slot.RemoveOneItem();

                moved++;
            }

            if (moved == 0)
                MonoCoC.UI.Components.Views.StorageView.BeginReplaceWithdraw(section, storageIndex);
        }

        public static void Exit()
        {
            MonoCoC.UI.Components.Views.GameView.SwitchTo<CampView>();
        }

        public static string ChestDescription()
        {
            var chests = new System.Collections.Generic.List<string>();

            if (Player.HasKeyItem(KeyItemIds.CampChest))
                chests.Add("Scenes.Storage.ChestLarge".Localized());

            if (Player.HasKeyItem(KeyItemIds.CampMurkyChest))
                chests.Add("Scenes.Storage.ChestMurky".Localized());

            if (Player.HasKeyItem(KeyItemIds.CampOrnateChest))
                chests.Add("Scenes.Storage.ChestOrnate".Localized());

            if (chests.Count == 0)
                return "Scenes.Storage.NoChest".Localized();

            var key = CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0
                ? "Scenes.Storage.ChestDescriptionCabin"
                : "Scenes.Storage.ChestDescription";

            return key.Localized(Utils.Utils.FormatStringArray(chests));
        }

        public static string SectionDescription(StorageArea area)
        {
            var id = area.Definition.Id;

            if (id == StorageRegistry.GearJewelryId)
                return "Scenes.Storage.JewelryBoxDescription".Localized(JewelryLocation());

            if (id == StorageRegistry.GearUndergarmentsId)
                return "Scenes.Storage.DresserDescription".Localized();

            if (id == StorageRegistry.GearWeaponsId)
                return "Scenes.Storage.WeaponRackDescription".Localized();

            if (id == StorageRegistry.GearArmorId)
                return "Scenes.Storage.ArmorRackDescription".Localized();

            if (id == StorageRegistry.GearShieldsId)
                return "Scenes.Storage.ShieldRackDescription".Localized();

            return "";
        }

        public static string JewelryLocation()
        {
            if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) <= 0 || !CabinInterior.HasBed)
                return "Scenes.Storage.JewelryLocation.Bedroll".Localized();

            if (CabinInterior.HasDresser)
                return "Scenes.Storage.JewelryLocation.Dresser".Localized();

            if (CabinInterior.HasNightstand)
                return "Scenes.Storage.JewelryLocation.Nightstand".Localized();

            return "Scenes.Storage.JewelryLocation.Bed".Localized();
        }
    }
}
