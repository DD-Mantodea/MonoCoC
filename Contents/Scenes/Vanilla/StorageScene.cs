using MonoCoC.UI.Components.Storage;
using MonoCoC.Contents.Storage;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World;
using Microsoft.Xna.Framework;
using MonoCoC.Contents.Items;
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
        public static StoragePane? _pane;

        public static bool _dragArmed;

        public static bool _dragStarted;

        public static bool _suppressClick;

        public static bool _dragFromInventory;

        public static Vector2 _dragOrigin;

        public static StorageArea? _dragSection;

        public static int _dragIndex;

        public static Label? _dragGhost;

        public override void Entry() => StorageMenu();

        public override void OnLeave()
        {
            CancelDrag();

            HidePane();
        }

        public static void StorageMenu()
        {
            Player.FixStorage();

            ClearOutput();

            ClearButtons();

            BuildPane();

            for (var index = 0; index < Player.ItemStacks.Length; index++)
            {
                var slot = Player.ItemStacks[index];

                if (!slot.Unlocked)
                    continue;

                if (slot.IsEmpty)
                    AddButtonDisabled(index, "Scenes.Inventory.Nothing".Localized());
                else
                {
                    var position = index;

                    var button = AddButton(index, slot.InvLabel, () =>
                    {
                        if (!_suppressClick)
                            Deposit(position);
                    }, slot.TooltipText, slot.TooltipHeader);

                    if (button != null)
                    {
                        button.OnMouseDown = () => BeginDragFromInventory(position);

                        button.OnMouseMove = MoveDrag;

                        button.OnMouseUp = EndDrag;
                    }
                }
            }

            AddButton(10, (Player.StorageMoveAll ? "Scenes.Storage.MoveAllOn" : "Scenes.Storage.MoveAllOff").Localized(), ToggleMoveAll, "Scenes.Storage.MoveAllHint".Localized());

            AddButton(14, "Common.Back".Localized(), Exit);
        }

        public static void ToggleMoveAll()
        {
            Player.StorageMoveAll = !Player.StorageMoveAll;

            StorageMenu();
        }

        public static System.Collections.Generic.List<StorageArea> Sections()
        {
            return Player.Storage.Values
                .Where(area => area.Definition.IsAvailable(Player))
                .OrderBy(area => area.Definition.Order)
                .ToList();
        }

        public static void BuildPane()
        {
            HidePane();

            _pane = new StoragePane(TextZoneX, TextZoneY, TextZoneWidth, TextZoneHeight)
                .Of(View);

            _pane.OnDragStart = BeginDragFromStorage;

            _pane.OnDragMove = MoveDrag;

            _pane.OnDragEnd = EndDrag;

            _pane.Build(
                Sections(),

                "Scenes.Storage.Title".Localized(),

                ChestDescription(),

                Withdraw,

                SectionDescription);
        }

        public static void HidePane()
        {
            if (_pane == null)
                return;

            _pane.LayoutData.Parent?.LayoutData.Children.Remove(_pane);

            _pane = null;
        }

        public static void Exit()
        {
            CancelDrag();

            HidePane();

            Go<CampScene>();
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

            // 原版 scenes/Inventory.hx:1003：住小屋时是「inside your cabin」，否则「near the portal entrance」
            var key = CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) > 0

                ? "Scenes.Storage.ChestDescriptionCabin"

                : "Scenes.Storage.ChestDescription";

            return key.Localized(MonoCoC.Utils.Utils.FormatStringArray(chests));
        }

        // 原版 scenes/Inventory.hx:1012 describe(storage) —— 各储物区自己的说明
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

        // 原版 scenes/Inventory.hx:1018 jewelryLocation()
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

        public static void Deposit(int position)
        {
            if (_suppressClick)
                return;

            var slot = Player.ItemStacks[position];

            if (slot.IsEmpty)
                return;

            StorageArea? target = null;

            var storageIndex = -1;

            var areas = Sections();

            foreach (var section in areas.Where(area => !area.Definition.Fallback).Concat(areas.Where(area => area.Definition.Fallback)))
            {
                if (!section.AcceptsItem(slot.Item))
                    continue;

                storageIndex = section.FirstEmptySlot();

                if (storageIndex < 0)
                    continue;

                target = section;

                break;
            }

            if (target == null)
            {
                Message("Scenes.Storage.NoSpace", StorageMenu);

                return;
            }

            var quantity = Player.StorageMoveAll ? slot.Quantity : 1;

            target.Slots[storageIndex].SetItemAndQty(slot.Item, quantity);

            for (var index = 0; index < quantity; index++)
                slot.RemoveOneItem();

            BuildPane();
        }

        public static void Withdraw(StorageArea section, int storageIndex)
        {
            if (_suppressClick)
                return;

            if (storageIndex < 0 || storageIndex >= section.Slots.Count)
                return;

            var slot = section.Slots[storageIndex];

            if (slot.IsEmpty)
                return;

            var item = slot.Item;

            var quantity = Player.StorageMoveAll ? slot.Quantity : 1;

            var moved = 0;

            while (moved < quantity && !slot.IsEmpty)
            {
                if (!Player.AddItem(item, 1))
                    break;

                slot.RemoveOneItem();

                moved++;
            }

            if (moved == 0)
            {
                Message("Scenes.Storage.NoRoom", StorageMenu);

                return;
            }

            BuildPane();
        }

        public static void BeginDragFromInventory(int index) => BeginDrag(true, null, index);

        public static void BeginDragFromStorage(StorageArea section, int index) => BeginDrag(false, section, index);

        public static void BeginDrag(bool fromInventory, StorageArea? section, int index)
        {
            _dragArmed = true;

            _dragStarted = false;

            _suppressClick = false;

            _dragFromInventory = fromInventory;

            _dragSection = section;

            _dragIndex = index;

            _dragOrigin = UserInput.GetMousePos();
        }

        public static void MoveDrag(Vector2 position)
        {
            if (!_dragArmed)
                return;

            if (!_dragStarted)
            {
                if (Vector2.Distance(position, _dragOrigin) < 8)
                    return;

                var slot = _dragFromInventory

                    ? Player.ItemStacks[_dragIndex]

                    : _dragSection!.Slots[_dragIndex];

                if (slot.IsEmpty)
                {
                    _dragArmed = false;

                    return;
                }

                _dragStarted = true;

                _dragGhost = new Label(UIFonts.Get("settingsBody"))
                    .Text(slot.InvLabel)
                    .CanHit(false)
                    .LayoutType(LayoutType.Absolute)
                    .Of(View);

                _dragGhost.TextColor = Color.Black;
            }

            _dragGhost?.LayoutData.Position = new Vector2(position.X + 12, position.Y + 12);
        }

        public static void EndDrag(Vector2 position)
        {
            if (!_dragArmed)
                return;

            if (!_dragStarted)
            {
                _dragArmed = false;

                _dragSection = null;

                return;
            }

            _suppressClick = true;

            RemoveGhost();

            if (_dragFromInventory)
                CompleteInventoryDrop(position);
            else
                CompleteStorageDrop(position);

            _dragArmed = false;

            _dragStarted = false;

            _dragSection = null;
        }

        public static void CompleteInventoryDrop(Vector2 position)
        {
            var source = Player.ItemStacks[_dragIndex];

            if (source.IsEmpty || _pane == null)
                return;

            if (!_pane.TryHitStorage(position, out var section, out var index))
                return;

            var target = section.Slots[index];

            if (!target.IsEmpty || !section.AcceptsItem(source.Item))
                return;

            var quantity = Player.StorageMoveAll ? source.Quantity : 1;

            target.SetItemAndQty(source.Item, quantity);

            for (var count = 0; count < quantity; count++)
                source.RemoveOneItem();

            BuildPane();
        }

        public static void CompleteStorageDrop(Vector2 position)
        {
            if (_dragSection == null)
                return;

            var source = _dragSection.Slots[_dragIndex];

            if (source.IsEmpty)
                return;

            if (_pane != null && _pane.TryHitStorage(position, out var section, out var index))
            {
                if (section == _dragSection && index == _dragIndex)
                    return;

                var target = section.Slots[index];

                if (!target.IsEmpty || !section.AcceptsItem(source.Item))
                    return;

                var quantity = Player.StorageMoveAll ? source.Quantity : 1;

                target.SetItemAndQty(source.Item, quantity);

                for (var count = 0; count < quantity; count++)
                    source.RemoveOneItem();

                BuildPane();

                return;
            }

            if (_pane != null && _pane.Rectangle.Contains(position))
                return;

            var item = source.Item;

            var move = Player.StorageMoveAll ? source.Quantity : 1;

            var moved = 0;

            while (moved < move && !source.IsEmpty)
            {
                if (!Player.AddItem(item, 1))
                    break;

                source.RemoveOneItem();

                moved++;
            }

            if (moved > 0)
                BuildPane();
        }

        public static void RemoveGhost()
        {
            if (_dragGhost == null)
                return;

            _dragGhost.LayoutData.Parent?.LayoutData.Children.Remove(_dragGhost);

            _dragGhost = null;
        }

        public static void CancelDrag()
        {
            RemoveGhost();

            _dragArmed = false;

            _dragStarted = false;

            _dragSection = null;
        }

        public static void Message(string key, Action next)
        {
            HidePane();

            ClearOutput();

            ClearButtons();

            OutputText(key.Localized());

            AddButton(0, "Common.OK".Localized(), next);
        }
    }
}
