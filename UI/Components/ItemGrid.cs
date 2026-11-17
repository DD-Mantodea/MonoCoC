using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Contents.Items;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Components
{
    public class ItemGrid : Component
    {
        public int Columns = 2;

        public int SlotHeight = 84;

        public int SlotWidth;

        public int Spacing = 8;

        public ItemSlotStyle ItemTemplate = new();

        public Action<ItemStack>? SelectionChanged;

        public Action<ItemStack>? ItemRightClicked;

        public int SelectedIndex = -1;

        public void ClearSelection()
        {
            SelectedIndex = -1;

            foreach (var slot in _slots)
                slot.Selected = false;
        }

        private readonly List<ItemSlot> _slots = [];

        private int _scroll;

        private int CardWidth => SlotWidth > 0 ? SlotWidth : Math.Max(1, (Width - LayoutData.Padding.Left - LayoutData.Padding.Right - Spacing * (Columns - 1)) / Columns);

        public ItemGrid()
        {
            Scissor = true;
        }

        public override void Update(GameTime gameTime)
        {
            var stacks = CoC.Instance.Player.Inventory.Slots;

            var visible = 0;

            for (var index = 0; index < stacks.Length; index++)
            {
                if (!stacks[index].Unlocked)
                    continue;

                while (_slots.Count <= visible)
                {
                    var slot = new ItemSlot
                    {
                        Style = ItemTemplate,
                        LayoutData = { LayoutType = LayoutType.Relative }
                    };

                    slot.Clicked += stack =>
                    {
                        SelectedIndex = slot.Index;
                        SelectionChanged?.Invoke(stack);
                    };

                    slot.RightClicked += stack => ItemRightClicked?.Invoke(stack);

                    AddChild(slot);
                    _slots.Add(slot);
                }

                var child = _slots[visible];

                child.Stack = stacks[index];
                child.Index = index;
                child.Style = ItemTemplate;
                child.LayoutData.Size = new Vector2(CardWidth, SlotHeight);
                child.LayoutData.Position = new Vector2(
                    visible % Columns * (CardWidth + Spacing),
                    visible / Columns * (SlotHeight + Spacing) - _scroll);
                child.Visible = true;
                child.Selected = index == SelectedIndex;
                visible++;
            }

            for (var index = visible; index < _slots.Count; index++)
                _slots[index].Visible = false;

            var rows = Math.Max(1, (visible + Columns - 1) / Columns);

            var contentHeight = LayoutData.Padding.Top + LayoutData.Padding.Bottom + rows * SlotHeight + Math.Max(0, rows - 1) * Spacing;

            var maxScroll = Math.Max(0, contentHeight - Height);

            var wheel = UserInput.GetDeltaWheelValue();

            if (wheel != 0 && Rectangle.Contains(UserInput.GetMousePos()))
                _scroll = Math.Clamp(_scroll - Math.Sign(wheel) * 48, 0, maxScroll);

            _scroll = Math.Clamp(_scroll, 0, maxScroll);

            base.Update(gameTime);
        }
    }
}
