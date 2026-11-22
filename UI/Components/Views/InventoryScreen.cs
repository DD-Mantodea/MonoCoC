using MonoCoC.Contents.Items;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

using MonoCoC.Registry;
namespace MonoCoC.UI.Components.Views
{
    public class InventoryScreen : GameView<InventoryScreen>
    {
        public InventoryScreen()
        {
            Initialize();

            SetDataContext(new PlayerStatsDataContext(
                new PlayerStatsModel(() => Player, () => Time)));
        }

        protected override string ScreenFile => "Views\\InventoryView.hjson";

        [UIRequired("inventory.background")]
        private void BindBackground(Component part) => Background = part;

        [UIRequired("inventory.stats.combat")]
        private void BindCombatStats(DetailStats part) => part.DetailSelected = ShowStatDetail;

        [UIRequired("inventory.grid")]
        private void BindItemGrid(ItemGrid part)
        {
            _itemGrid = part;

            part.SelectionChanged = stack =>
            {
                _equipmentGrid?.ClearSelection();

                _selectedStack = stack;

                ShowItemDetail(stack.Item);
            };

            // 右键：菜单条目完全来自 ItemInteractions 注册表，视图不认识任何物品类别。
            part.ItemRightClicked = stack =>
            {
                _menuContext = new ItemInteractionContext { Item = stack.Item, Stack = stack };

                OpenItemMenu(ItemInteractions.Build(_menuContext));
            };
        }

        [UIRequired("inventory.equipment")]
        private void BindEquipmentGrid(EquipmentGrid part)
        {
            _equipmentGrid = part;

            part.SelectionChanged = stack =>
            {
                _itemGrid?.ClearSelection();

                if (stack is null)
                {
                    ClearDetail();

                    return;
                }

                ShowItemDetail(stack);
            };

            part.SlotActivated = (_, _) => _itemGrid?.ClearSelection();

            // 右键已装备物品：Unequip 也来自注册表（equipmentSlotId 在上下文里）。
            part.RightClicked = (item, slotId, _) =>
            {
                if (item is null)
                    return;

                _menuContext = new ItemInteractionContext { Item = item, EquipmentSlotId = slotId };

                OpenItemMenu(ItemInteractions.Build(_menuContext));
            };
        }

        [UIRequired("inventory.detail.name")]
        private void BindDetailName(Label part) => _detailName = part;

        [UIRequired("inventory.detail.type")]
        private void BindDetailType(Label part) => _detailType = part;

        [UIRequired("inventory.detail.description")]
        private void BindDetailDescription(Label part) => _detailDescription = part;

        [UIRequired("inventory.detail.stats")]
        private void BindDetailStats(Label part) => _detailStats = part;

        [UIRequired("inventory.notice")]
        private void BindNotice(Label part) => _notice = part;

        [UIRequired("inventory.keyItems")]
        private void BindKeyItems(TextArea part)
        {
            _keyItems = part;

            _keyItems.AutoScroll = false;
        }

        [UIRequired("inventory.resources")]
        private void BindResources(TextArea part)
        {
            _resources = part;

            _resources.AutoScroll = false;
        }

        protected override void OnPartsBound()
        {
            base.OnPartsBound();

            RefreshKeyItems();
        }

        private void RefreshKeyItems()
        {
            if (_keyItems is null)
                return;

            var text = new System.Text.StringBuilder();

            foreach (var keyItem in Player.KeyItems.Values)
                text.AppendLine("- " + keyItem.DisplayName);

            _keyItems.SetText(text.ToString());

            var resources = new System.Text.StringBuilder();

            if (Player.HasKeyItem(KeyItemIds.CarpentersToolbox))
            {
                resources.AppendLine("Wood: " + CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources));

                resources.AppendLine("Nails: " + Player.KeyItemValue("Carpenter's Toolbox", 1));

                resources.AppendLine("Stone: " + CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources));
            }
            else
            {
                resources.Append("—");
            }

            _resources.SetText(resources.ToString());
        }

        private void ShowStatDetail(StatsDetailScene.StatDetail detail)
        {
            _detailName.Text(detail.Label);
            _detailType.Text("");
            _detailDescription.Text(System.Text.RegularExpressions.Regex.Replace(detail.Text(), "<[^>]+>", ""));
            _detailStats.Text("");
        }

        private void ShowItemDetail(Item item)
        {
            var detail = item.GetSlotDetail();

            _detailName.Text(detail.Name);
            _detailType.Text(detail.Type);
            _detailDescription.Text(detail.Description);
            _detailStats.Text("");
        }

        private void ClearDetail()
        {
            _detailName.Text("");
            _detailType.Text("");
            _detailDescription.Text("");
            _detailStats.Text("");
        }

        // 动作提示单独一行，不覆盖物品详情。
        public void ShowNotice(string text, double seconds = 4)
        {
            if (_notice is null)
                return;

            _notice.Text(text);

            _notice.Visible = text.Length > 0;

            _noticeTimer = _notice.Visible ? seconds : 0;
        }

        private void TickNotice(double delta)
        {
            if (_notice is null || !_notice.Visible)
                return;

            _noticeTimer -= delta;

            if (_noticeTimer > 0)
                return;

            _notice.Visible = false;

            _notice.Text("");
        }

        // 菜单是 HJSON 声明的 part（coc:context_menu），这里只接回调和样式外的逻辑。
        [UIRequired("inventory.menu")]
        private void BindMenu(ContextMenu part)
        {
            _menu = part;

            _menu.Chosen = OnMenuChosen;

            _menu.Dismissed = () => _menuContext = null;
        }

        // 物品交互 → 通用菜单条目（菜单组件不认识物品，只认条目）。
        private void OpenItemMenu(IReadOnlyList<ItemInteraction> interactions)
        {
            if (_menuContext is null || _menu is null)
                return;

            var context = _menuContext;

            var entries = new List<ContextMenuEntry>(interactions.Count);

            foreach (var interaction in interactions)
                entries.Add(new ContextMenuEntry
                {
                    LabelKey = interaction.LabelKey,

                    Order = interaction.Order,

                    Invoke = () => interaction.Invoke(context)
                });

            _menu.Open(UserInput.GetMousePos(), entries);
        }

        // 菜单条目点击：执行条目的 Invoke，返回值当作提示显示。
        private void OnMenuChosen(ContextMenuEntry entry)
        {
            if (_menuContext is null)
                return;

            string? message;

            try
            {
                message = entry.Invoke?.Invoke();
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }

            if (!string.IsNullOrEmpty(message))
                ShowNotice(message);

            _menuContext = null;

            _itemGrid?.ClearSelection();

            _equipmentGrid?.ClearSelection();

            _selectedStack = null;

            ClearDetail();
        }

        public override void Update(GameTime gameTime)
        {
            TickNotice(gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        private static Contents.Creatures.Player Player => MonoCoC.CoC.Instance.Player;

        private Label? _notice;

        private TextArea? _keyItems;

        private TextArea? _resources;

        private ContextMenu? _menu;

        private ItemInteractionContext? _menuContext;

        private double _noticeTimer;

        private Label _detailName = null!;

        private Label _detailType = null!;

        private Label _detailDescription = null!;

        private Label _detailStats = null!;

        private ItemStack? _selectedStack;

        private ItemGrid? _itemGrid;

        private EquipmentGrid? _equipmentGrid;

        [UIEvent("inventory.back")]
        private void Back()
        {
            _menu?.Close(false);

            InventoryFlow.Exit();
        }
    }
}
