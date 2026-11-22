using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using System;
using System.Collections.Generic;

using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using MonoCoC.UI.Data;
using MonoCoC.UI.Structures;
using MonoCoC.UI.Styles;
using System.Runtime.CompilerServices;
namespace MonoCoC.UI.Components
{
    public sealed class EquipmentArea
    {
        public string Id = "";
        public int X;
        public int Y;
        public int Width;
        public int Height;
    }

    public sealed class EquipmentSlotSpec
    {
        public string Id = "";
        public string Kind = "weapon";
        public string LabelKey = "";
        public string SlotIcon = "";
        public string Area = "";
        public int Order;
        public bool IsExtra;
        public string FallbackAttack = "";
        public List<string> Accepts = [];
        public Dictionary<string, int> Capacity = [];
        public List<string> RequiresTags = [];
    }

    public class EquipmentGrid : Component, IFromHjson<EquipmentGrid>
    {
        public static string TypeId => "coc:equipment_grid";

        public static Component FromHjson(JsonObject node, UIBuildContext services)
        {
            var grid = new EquipmentGrid
            {
                Columns = node.Int("columns", 1),
                SlotWidth = node.Int("slotWidth", 0),
                SlotHeight = node.Int("slotHeight", 84),
                Spacing = node.Int("spacing", 8),
                ItemStyle = ItemSlotStyleResolver.Resolve(node, services.Document)
            };

            if (node.Array("areas") is { } areas)
            {
                foreach (var value in areas)
                {
                    if (value is not JsonObject area)
                        continue;

                    grid.Areas.Add(new EquipmentArea
                    {
                        Id = area.Str("id"),
                        X = area.Int("x", 0),
                        Y = area.Int("y", 0),
                        Width = area.Int("width", 0),
                        Height = area.Int("height", 0)
                    });
                }
            }

            if (node.Array("slots") is { } slots)
            {
                foreach (var value in slots)
                {
                    if (value is not JsonObject slot)
                        continue;

                    grid.Slots.Add(new EquipmentSlotSpec
                    {
                        Kind = slot.Str("kind", "weapon"),
                        LabelKey = UILocalizedText.Resolve(slot, "label"),
                        SlotIcon = slot.Str("slotIcon"),
                        Area = slot.Str("area"),
                        Order = slot.Int("order", 0)
                    });
                }
            }

            return grid;
        }

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<EquipmentGrid>();

        public readonly List<EquipmentArea> Areas = [];

        public readonly List<EquipmentSlotSpec> Slots = [];

        public Action<Item?>? SelectionChanged;

        public Action<string, string>? SlotActivated;

        public Action<Item?, string, string>? RightClicked;

        public ItemSlotStyle? ItemStyle;

        public string? SelectedSlotId;

        public void ClearSelection()
        {
            SelectedSlotId = null;

            foreach (var child in _children)
                child.Selected = false;
        }

        public int SlotWidth;

        public int SlotHeight = 84;

        public int Columns = 1;

        public int Spacing = 8;

        private const int HeaderHeight = 24;

        public Color SlotColor = UIParseUtils.ColorOf("#F7EFE0CC");

        public Color BorderColor = UIParseUtils.ColorOf("#B79A70AA");

        public Color TextColor = UIParseUtils.ColorOf("#35291FFF");

        private readonly List<Label> _headerLabels = [];

        private readonly List<EquipmentSlot> _children = [];

        private int _scroll;

        private int CardWidth => SlotWidth > 0
            ? SlotWidth
            : Math.Max(1, (Width - LayoutData.Padding.Left - LayoutData.Padding.Right - Spacing * (Columns - 1)) / Columns);

        public EquipmentGrid()
        {
            Scissor = true;

            MonoCoC.Contents.Equipment.EquipmentCatalog.EnsureLoaded();

            if (Areas.Count == 0)
                Areas.AddRange(MonoCoC.Contents.Equipment.EquipmentCatalog.Areas);
        }

        public override void Update(GameTime gameTime)
        {
            Slots.Clear();
            Slots.AddRange(MonoCoC.Contents.Equipment.EquipmentCatalog.Compose(CoC.Instance.Player.EquipmentTags));

            Slots.Sort((a, b) =>
            {
                var order = TypeOrder(a.Kind).CompareTo(TypeOrder(b.Kind));

                return order != 0 ? order : a.Order.CompareTo(b.Order);
            });

            while (_children.Count < Slots.Count)
            {
                var child = new EquipmentSlot
                {
                    LayoutData = { LayoutType = LayoutType.Relative }
                };

                child.Clicked = item => SelectionChanged?.Invoke(item);                child.Activated = (id, kind) =>
                {
                    SelectedSlotId = id;

                    SlotActivated?.Invoke(id, kind);
                };

                child.RightClicked = (item, id, kind) => RightClicked?.Invoke(item, id, kind);

                AddChild(child);
                _children.Add(child);
            }

            for (var index = 0; index < _children.Count; index++)
                _children[index].Visible = index < Slots.Count;

            var headerIndex = 0;
            var cursor = 0;
            var slotIndex = 0;

            while (slotIndex < Slots.Count)
            {
                var kind = Slots[slotIndex].Kind;
                var header = Slots[slotIndex].LabelKey.LocalizedOr(kind);

                while (_headerLabels.Count <= headerIndex)
                {
                    var label = new Label(UIFonts.GetAt(UIFonts.Family("coc:menu"), 16))
                    {
                        LayoutData = { LayoutType = LayoutType.Relative }
                    };

                    AddChild(label);
                    _headerLabels.Add(label);
                }

                var headerLabel = _headerLabels[headerIndex];
                headerLabel.Text(header);
                headerLabel.TextColor = TextColor * 0.75f;
                headerLabel.LayoutData.Position = new Vector2(0, cursor - _scroll);
                headerLabel.Visible = true;
                headerIndex++;

                cursor += HeaderHeight;

                var groupStart = slotIndex;

                // 组内布局基线：手牌两列一行，行数 = ceil(手数 / 2)。
                var groupTop = cursor;

                // 手牌槽的占用完全由容量推算：一件物品消耗 Uses(item)["hand"] 格，
                // 它后面被占用的格子置灰（四只手时双手武器只占两格，其余照常可用）。
                var blockedHands = 0;

                while (slotIndex < Slots.Count && Slots[slotIndex].Kind == kind)
                {
                    var spec = Slots[slotIndex];
                    var child = _children[slotIndex];

                    child.Kind = spec.Kind;
                    child.SlotId = spec.Id;
                    child.IsExtra = spec.IsExtra;
                    child.LabelKey = spec.LabelKey;
                    child.SlotIcon = spec.SlotIcon;
                    child.ShowLabel = false;
                    child.Style = ItemStyle;
                    child.Selected = spec.Id == SelectedSlotId;
                    child.Disabled = false;
                    child.SlotColor = SlotColor;
                    child.BorderColor = BorderColor;
                    child.TextColor = TextColor;

                    if (spec.Kind == "hand")
                    {
                        var offset = slotIndex - groupStart;

                        var handIndex = offset + 1;

                        if (blockedHands > 0)
                        {
                            blockedHands--;

                            child.Disabled = true;
                        }
                        else if (CoC.Instance.Player.GetHand(handIndex) is { } item)
                        {
                            blockedHands = Math.Max(0,
                                (MonoCoC.Contents.Equipment.EquipmentRules.Uses(item)
                                    .TryGetValue("hand", out var uses) ? uses : 1) - 1);
                        }

                        // 列数读配置（InventoryView.hjson 的 columns），不写死 2。
                        var columns = Math.Max(1, Columns);

                        var column = offset % columns;

                        var row = offset / columns;

                        child.LayoutData.Size = new Vector2(CardWidth, SlotHeight);

                        child.LayoutData.Position = new Vector2(
                            column * (CardWidth + Spacing),
                            groupTop - _scroll + row * (SlotHeight + Spacing));

                        slotIndex++;

                        continue;
                    }

                    child.LayoutData.Size = new Vector2(CardWidth, SlotHeight);
                    child.LayoutData.Position = new Vector2(0, cursor - _scroll);

                    cursor += SlotHeight + Spacing;
                    slotIndex++;
                }

                if (kind == "hand")
                {
                    var handCount = slotIndex - groupStart;

                    var rows = (handCount + Math.Max(1, Columns) - 1) / Math.Max(1, Columns);

                    cursor = groupTop + rows * (SlotHeight + Spacing);
                }
            }

            for (var index = headerIndex; index < _headerLabels.Count; index++)
                _headerLabels[index].Visible = false;

            var contentHeight = LayoutData.Padding.Top + LayoutData.Padding.Bottom + cursor;

            var maxScroll = Math.Max(0, contentHeight - Height);

            var wheel = UserInput.GetDeltaWheelValue();

            if (wheel != 0 && Rectangle.Contains(UserInput.GetMousePos()))
                _scroll = Math.Clamp(_scroll - Math.Sign(wheel) * 48, 0, maxScroll);

            _scroll = Math.Clamp(_scroll, 0, maxScroll);

            base.Update(gameTime);
        }

        private static int TypeOrder(string kind) => kind switch
        {
            "hand" => 0,
            "weapon" => 1,
            "shield" => 2,
            "armor" => 3,
            "upper" => 4,
            "lower" => 5,
            "jewelry" => 6,
            _ => 7
        };

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);
        }
    }
}
