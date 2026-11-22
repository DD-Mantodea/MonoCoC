using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Storage;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.UI.Components.Views
{
    public class StorageView : GameView<StorageView>
    {
        public static StorageView? Active;

        private ItemGrid _inventoryGrid = null!;

        private ItemGrid _warehouseGrid = null!;

        private ChoiceWheel _tabs = null!;

        private ContextMenu _menu = null!;

        private Label _detailName = null!;

        private Label _detailType = null!;

        private Label _detailDescription = null!;

        private Label _errorLabel = null!;

        private ItemInteractionContext? _menuContext;

        private readonly List<ItemStack> _warehouseStacks = [];

        private readonly List<(StorageArea Area, int Index)> _warehouseRefs = [];

        private Identifier? _selectedSection;

        private int _replaceInventoryIndex = -1;

        private StorageArea? _replaceStorageArea;

        private int _replaceStorageIndex = -1;

        public StorageView()
        {
            Active = this;

            Initialize();
        }

        protected override string ScreenFile => "Views\\StorageView.hjson";

        [UIRequired("storage.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("storage.inventory")]
        private void BindInventory(ItemGrid part)
        {
            _inventoryGrid = part;

            part.SelectionChanged = stack =>
            {
                if (_replaceStorageArea is not null)
                {
                    CompleteReplaceWithdraw(stack);

                    return;
                }

                _warehouseGrid?.ClearSelection();

                ShowDetail(stack);
            };

            part.ItemRightClicked = stack => OpenMenu(new ItemInteractionContext
            {
                Item = stack.Item,
                Stack = stack,
                Page = InventoryPage.Storage,
                StorageArea = CurrentSection()
            });
        }

        [UIRequired("storage.warehouse")]
        private void BindWarehouse(ItemGrid part)
        {
            _warehouseGrid = part;

            part.StacksProvider = WarehouseStacks;

            part.SelectionChanged = stack =>
            {
                if (_replaceInventoryIndex >= 0)
                {
                    CompleteReplaceDeposit(stack);

                    return;
                }

                _inventoryGrid?.ClearSelection();

                ShowDetail(stack);
            };

            part.ItemRightClicked = stack =>
            {
                var index = _warehouseStacks.IndexOf(stack);

                if (index < 0 || index >= _warehouseRefs.Count)
                    return;

                var reference = _warehouseRefs[index];

                OpenMenu(new ItemInteractionContext
                {
                    Item = stack.Item,
                    Stack = stack,
                    Page = InventoryPage.Storage,
                    StorageArea = reference.Area,
                    StorageIndex = reference.Index
                });
            };
        }

        [UIRequired("storage.tabs")]
        private void BindTabs(ChoiceWheel part) => _tabs = part;

        protected override void OnPartsBound()
        {
            base.OnPartsBound();

            try
            {
                RefreshTabs();
            }
            catch
            {
            }
        }

        public void RefreshTabs()
        {
            if (_tabs is null)
                return;

            var sections = StorageScene.Sections();

            _tabs.VisibleItems = Math.Max(1, sections.Count);

            _tabs.SetItems(sections.Select(section => (
                section.Definition.Id.Path,
                section.Definition.NameKey)));

            _selectedSection = sections.FirstOrDefault()?.Definition.Id;

            _tabs.SelectionChanged = id =>
            {
                _selectedSection = new Identifier("coc", id);

                ClearReplaceState();

                _warehouseGrid?.ClearSelection();
            };

            _tabs.SelectAt(0);
        }

        [UIRequired("storage.menu")]
        private void BindMenu(ContextMenu part)
        {
            _menu = part;

            _menu.Chosen = OnMenuChosen;

            _menu.Dismissed = () => _menuContext = null;
        }

        [UIRequired("storage.detail.name")]
        private void BindDetailName(Label part) => _detailName = part;

        [UIRequired("storage.detail.type")]
        private void BindDetailType(Label part) => _detailType = part;

        [UIRequired("storage.detail.description")]
        private void BindDetailDescription(Label part) => _detailDescription = part;

        [UIRequired("storage.error")]
        private void BindError(Label part) => _errorLabel = part;

        [UIEvent("storage.back")]
        private void Back() => StorageScene.Exit();

        private IReadOnlyList<ItemStack> WarehouseStacks()
        {
            _warehouseStacks.Clear();

            _warehouseRefs.Clear();

            foreach (var area in StorageScene.Sections())
            {
                if (_selectedSection is not null && area.Definition.Id != _selectedSection)
                    continue;

                for (var index = 0; index < area.Slots.Count; index++)
                {
                    _warehouseStacks.Add(area.Slots[index]);

                    _warehouseRefs.Add((area, index));
                }
            }

            return _warehouseStacks;
        }

        private void OpenMenu(ItemInteractionContext context)
        {
            _menuContext = context;

            var entries = new List<ContextMenuEntry>();

            foreach (var interaction in ItemInteractions.BuildStorage(context))
                entries.Add(new ContextMenuEntry
                {
                    LabelKey = interaction.LabelKey,
                    Order = interaction.Order,
                    Invoke = interaction.Id == "store"
                        ? () => { OpenTransfer(context, true); return null; }
                        : interaction.Id == "take_out"
                            ? () => { OpenTransfer(context, false); return null; }
                            : () => interaction.Invoke(context)
                });

            _menu.Open(UserInput.GetMousePos(), entries);
        }

        private void OnMenuChosen(ContextMenuEntry entry)
        {
            var context = _menuContext;

            if (context is null)
                return;

            string? message = null;

            try
            {
                message = entry.Invoke?.Invoke();
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }

            _menuContext = null;

            _inventoryGrid.ClearSelection();

            _warehouseGrid.ClearSelection();

            if (!string.IsNullOrEmpty(message))
                ShowError(message);
        }

        private StorageArea? CurrentSection()
        {
            if (_selectedSection is null)
                return null;

            return StorageScene.Sections().FirstOrDefault(area => area.Definition.Id == _selectedSection);
        }

        public static bool BeginReplaceDeposit(int inventoryIndex)
        {
            if (Active is null)
                return false;

            Active._replaceInventoryIndex = inventoryIndex;

            Active._warehouseGrid.HighlightAll = true;

            Active.ShowError("Storage is full. Click a warehouse slot to replace it.");

            return true;
        }

        public static bool BeginReplaceWithdraw(StorageArea area, int storageIndex)
        {
            if (Active is null)
                return false;

            Active._replaceStorageArea = area;

            Active._replaceStorageIndex = storageIndex;

            Active._inventoryGrid.HighlightAll = true;

            Active.ShowError("Backpack is full. Click an inventory slot to replace it.");

            return true;
        }

        private void CompleteReplaceDeposit(ItemStack target)
        {
            var index = _warehouseStacks.IndexOf(target);

            if (index < 0 || index >= _warehouseRefs.Count || _replaceInventoryIndex < 0)
            {
                ClearReplaceState();

                return;
            }

            var source = CoC.Instance.Player.Inventory.Slots[_replaceInventoryIndex];

            var reference = _warehouseRefs[index];

            if (source.IsEmpty)
            {
                ClearReplaceState();

                return;
            }

            if (!reference.Area.AcceptsItem(source.Item))
            {
                ShowError("This storage cannot hold that item.");

                return;
            }

            var targetStack = reference.Area.Slots[reference.Index];

            var oldItem = targetStack.Item;

            var oldQuantity = targetStack.Quantity;

            targetStack.SetItemAndQty(source.Item, source.Quantity);

            source.SetItemAndQty(oldItem, oldQuantity);

            _errorLabel.Text("");

            ClearReplaceState();

            _inventoryGrid.ClearSelection();

            _warehouseGrid.ClearSelection();

            ShowDetailText("Item swapped into storage.");
        }

        private void CompleteReplaceWithdraw(ItemStack target)
        {
            if (_replaceStorageArea is null || _replaceStorageIndex < 0)
            {
                ClearReplaceState();

                return;
            }

            var index = Array.IndexOf(CoC.Instance.Player.Inventory.Slots, target);

            if (index < 0)
            {
                ClearReplaceState();

                return;
            }

            var source = _replaceStorageArea.Slots[_replaceStorageIndex];

            if (source.IsEmpty)
            {
                ClearReplaceState();

                return;
            }

            var targetSlot = CoC.Instance.Player.Inventory.Slots[index];

            var oldItem = targetSlot.Item;

            var oldQuantity = targetSlot.Quantity;

            targetSlot.SetItemAndQty(source.Item, source.Quantity);

            source.SetItemAndQty(oldItem, oldQuantity);

            _errorLabel.Text("");

            ClearReplaceState();

            _inventoryGrid.ClearSelection();

            _warehouseGrid.ClearSelection();

            ShowDetailText("Item swapped into backpack.");
        }

        private void ClearReplaceState()
        {
            _replaceInventoryIndex = -1;

            _replaceStorageArea = null;

            _replaceStorageIndex = -1;

            if (_inventoryGrid is not null)
                _inventoryGrid.HighlightAll = false;

            if (_warehouseGrid is not null)
                _warehouseGrid.HighlightAll = false;
        }

        private void OpenTransfer(ItemInteractionContext context, bool store)
        {
            if (store)
            {
                if (context.StorageArea is null || !context.StorageArea.AcceptsItem(context.Item))
                {
                    ShowError("Scenes.Storage.Error.TypeRejected".Localized());

                    return;
                }

                var index = Array.IndexOf(CoC.Instance.Player.Inventory.Slots, context.Stack);

                if (index < 0)
                    return;

                if (context.StorageArea.FirstEmptySlot() < 0)
                {
                    StorageScene.DepositTo(index, context.StorageArea);

                    return;
                }

                if (context.Stack!.Quantity <= 1)
                {
                    StorageScene.DepositTo(index, context.StorageArea, 1);

                    return;
                }

                TransferBarOverlay.Show(
                    "Scenes.Storage.Store".Localized() + ": " + context.Item.Name,
                    context.Stack!.Quantity,
                    amount => StorageScene.DepositTo(index, context.StorageArea, amount));

                return;
            }

            if (context.StorageArea is null || context.StorageIndex < 0 || context.Stack is null)
                return;

            if (!CoC.Instance.Player.Inventory.CanAdd(context.Item))
            {
                BeginReplaceWithdraw(context.StorageArea, context.StorageIndex);

                return;
            }

            if (context.Stack.Quantity <= 1)
            {
                StorageScene.Withdraw(context.StorageArea, context.StorageIndex, 1);

                return;
            }

            TransferBarOverlay.Show(
                "Scenes.Storage.TakeOut".Localized() + ": " + context.Item.Name,
                context.Stack.Quantity,
                amount => StorageScene.Withdraw(context.StorageArea, context.StorageIndex, amount));
        }

        private void ShowDetail(ItemStack stack)
        {
            _errorLabel.Text("");

            var detail = stack.Item.GetSlotDetail();

            _detailName.Text(detail.Name);

            _detailType.Text(detail.Type);

            _detailDescription.Text(detail.Description);
        }

        private void ShowError(string text)
        {
            _errorLabel.Text(text);

            _detailName.Text("");

            _detailType.Text("");

            _detailDescription.Text("");
        }

        private void ShowDetailText(string text)
        {
            _detailName.Text("");

            _detailType.Text("");

            _detailDescription.Text(text);
        }
    }
}
