using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Contents.Items;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Structures;
using MonoCoC.UI.Styles;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public class ItemGrid : Component, IFromHjson<ItemGrid> {
        public int Columns = 2;

        public int SlotHeight = 84;

        public int SlotWidth;

        public int Spacing = 8;

        public int HeaderHeight;

        public ItemSlotStyle ItemStyle = new();

        public Action<ItemStack>? SelectionChanged;

        public Action<ItemStack>? ItemRightClicked;

        public Func<IReadOnlyList<ItemStack>>? StacksProvider;

        public int SelectedIndex = -1;

        public bool HighlightAll;

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
            IReadOnlyList<ItemStack> stacks = StacksProvider?.Invoke() ?? CoC.Instance.Player.Inventory.Slots;

            var visible = 0;

            for (var index = 0; index < stacks.Count; index++)
            {
                if (!stacks[index].Unlocked)
                    continue;

                while (_slots.Count <= visible)
                {
                    var slot = new ItemSlot
                    {
                        Style = ItemStyle,
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
                child.Style = ItemStyle;
                child.LayoutData.Size = new Vector2(CardWidth, SlotHeight);
                child.LayoutData.Position = new Vector2(
                    visible % Columns * (CardWidth + Spacing),
                    HeaderHeight + visible / Columns * (SlotHeight + Spacing) - _scroll);
                child.Visible = true;
                child.Selected = HighlightAll || index == SelectedIndex;
                visible++;
            }

            for (var index = visible; index < _slots.Count; index++)
                _slots[index].Visible = false;

            var rows = Math.Max(1, (visible + Columns - 1) / Columns);

            var contentHeight = HeaderHeight + LayoutData.Padding.Top + LayoutData.Padding.Bottom + rows * SlotHeight + Math.Max(0, rows - 1) * Spacing;

            var maxScroll = Math.Max(0, contentHeight - Height);

            var wheel = UserInput.GetDeltaWheelValue();

            if (wheel != 0 && Rectangle.Contains(UserInput.GetMousePos()))
                _scroll = Math.Clamp(_scroll - Math.Sign(wheel) * 48, 0, maxScroll);

            _scroll = Math.Clamp(_scroll, 0, maxScroll);

            base.Update(gameTime);
        }
    
        public static string TypeId => "coc:item_grid";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var style = new ItemSlotStyle();

            if (node.TryGetValue("itemStyle", out var styleValue))
            {
                JsonObject? template = null;

                if (styleValue is JsonObject inline)
                    template = inline;
                else if (styleValue is not JsonObject &&
                         services.Document.Styles.TryGetValue(styleValue.ToString().Trim('"'), out var named))
                    template = named;

                if (template is not null)
                {
                    style.Height = template.Int("height", style.Height);
                    style.IconSize = template.Int("iconSize", style.IconSize);
                    style.EmptyIcon = template.Str("emptyIcon", style.EmptyIcon);
                    style.Padding = template.Int("padding", style.Padding);
                    style.Spacing = template.Int("spacing", style.Spacing);
                    style.ShowName = template.Bool("showName", true);
                    style.ShowType = template.Bool("showType", true);
                    style.ShowQuantity = template.Bool("showQuantity", true);

                    if (template.TryGetValue("states", out var statesValue) && statesValue is JsonObject states)
                        style.States = BuildStateStyle(states, template.Int("borderThickness", 0));
                }
            }

            return new ItemGrid
            {
                Columns = node.Int("columns", 2),
                SlotWidth = node.Int("slotWidth", 0),
                SlotHeight = style.Height,
                Spacing = node.Int("spacing", 8),
                HeaderHeight = node.Int("headerHeight", 0),
                ItemStyle = style
            };
        }


        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<ItemGrid>();
    
}
}
