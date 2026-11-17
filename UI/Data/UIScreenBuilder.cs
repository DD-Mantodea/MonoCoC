using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Materials;
using MonoCoC.UI.Components;
using MonoCoC.UI.Components.Combat;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Structures;
using MonoCoC.UI.Styles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace MonoCoC.UI.Data
{
    public class UIScreenBuilder
    {
        private readonly UIEventRegistry _events;
        private readonly UICustomComponentRegistry _customComponents;
        private readonly UIComponentRegistry _types = new();

        private UIDocument? _document;

        public UIScreenBuilder(UIEventRegistry events, UICustomComponentRegistry customComponents)
        {
            _events = events;
            _customComponents = customComponents;

            RegisterCoreTypes();
        }

        public UIComponentRegistry Types => _types;

        private void RegisterCoreTypes()
        {
            _types.Register("coc:view", _ => new View());
            _types.Register("coc:image", CreateImage);
            _types.Register("coc:panel", CreatePanel);
            _types.Register("coc:gradient", CreateGradient);
            _types.Register("coc:label", CreateLabel);
            _types.Register("coc:button", CreateButton);
            _types.Register("coc:paper_background", CreatePaperBackground);
            _types.Register("coc:border", CreateBorder);
            _types.Register("coc:corners", CreateCorners);
            _types.Register("coc:symbol", CreateSymbol);
            _types.Register("coc:divider", CreateDivider);
            _types.Register("coc:context_menu", CreateContextMenu);
            
            _types.Register("coc:grid", CreateGrid);
            _types.Register("coc:number", CreateNumber);
            _types.Register("coc:slider", CreateSlider);
            _types.Register("coc:numeric_stepper", CreateNumericStepper);
            _types.Register("coc:option_list", CreateOptionList);
            _types.Register("coc:selector", CreateSelector);
            _types.Register("coc:segmented", CreateSegmented);
            _types.Register("coc:choice_detail", CreateChoiceDetail);
            _types.Register("coc:choice_wheel", CreateChoiceWheel);
            _types.Register("coc:text_area", CreateTextArea);
            _types.Register("coc:text_box", CreateTextBox);
            _types.Register("coc:bar", CreateBar);
            _types.Register("coc:stat_bar", CreateBar);
            _types.Register("coc:stat_row", CreateStatRow);
            _types.Register("coc:player_stats", CreatePlayerStats);

            _types.Register("coc:item_grid", node => CreateItemGrid(node, _document!));

            _types.Register("coc:equipment_grid", node => CreateEquipmentGrid(node, _document!));

            _types.Register("coc:equipment_slot", CreateEquipmentSlot);
            _types.Register("coc:combat_stats", _ => new CombatStats());
            _types.Register("coc:custom_widget", CreateCustomComponent);
            _types.Register("coc:custom_component", CreateCustomComponent);
        }

        public View Build(UIDocument document)
        {
            _document = document;

            var rootNode = document.Root.TryGetValue("root", out var rootValue) && rootValue is JsonObject rootObject
                ? rootObject
                : document.Root;

            rootNode = document.Expand(rootNode);

            var root = new View();

            ApplyLayout(root, rootNode);

            BuildChildren(root, rootNode, document);

            return root;
        }

        private void BuildChildren(Component parent, JsonObject node, UIDocument document)
        {
            if (!node.TryGetValue("children", out var value) || value is not JsonArray children)
                return;

            var sequentialIndex = 0;

            foreach (var childValue in children)
            {
                if (childValue is not JsonObject child)
                    continue;

                child = document.Expand(child);

                var component = Create(child);

                if (component is null)
                    continue;

                if (parent is Grid { AutoIndex: true })
                    component.LayoutData.GridIndex = sequentialIndex;
                else if (child.TryGetValue("gridIndex", out var gridIndex))
                    component.LayoutData.GridIndex = Int(gridIndex);

                sequentialIndex++;

                ApplyMaterial(component, child);

                ApplyLayout(component, child);

                ApplyStateStyle(component, child);

                ApplyDecorationStyle(component, child);

                BindEvents(component, child);

                if (component is Button button)
                    ApplyControlTemplate(button, child, document);

                parent.AddChild(component);

                BuildChildren(component, child, document);

                if (component is Grid grid)
                    grid.ResizeToFit();
            }
        }

        // 供视图在运行期按模板构造单个组件（标签页等），补齐 BuildChildren 里那套样式应用。
        public Component BuildTemplate(JsonObject template)
        {
            var component = Create(template) ?? new Component();

            ApplyLayout(component, template);

            ApplyStateStyle(component, template);

            if (component is Button button && _document is not null)
                ApplyControlTemplate(button, template, _document);

            return component;
        }
        private Component? Create(JsonObject node)
        {
            var type = node.Str("type", "Panel");

            if (!type.Contains(':', StringComparison.Ordinal))
                type = "coc:" + type.ToLowerInvariant();

            if (!_types.TryCreate(type, node, out var component))
                throw new InvalidDataException($"Unknown UI component type '{type}'");

            return component;
        }

        private static Component CreateBorder(JsonObject node)
        {
            var border = new BorderDecoration();

            if (node.TryGetValue("thickness", out var thickness))
                border.Thickness = Int(thickness);

            if (node.TryGetValue("inset", out var inset))
                border.Inset = Int(inset);

            return border;
        }

        private static Component CreateCorners(JsonObject node)
        {
            var corners = new CornerDecoration();

            if (node.TryGetValue("length", out var length))
                corners.Length = Int(length);

            if (node.TryGetValue("thickness", out var thickness))
                corners.Thickness = Int(thickness);

            if (node.TryGetValue("inset", out var inset))
                corners.Inset = Int(inset);

            if (node.TryGetValue("gap", out var gap))
                corners.Gap = Int(gap);

            return corners;
        }

        private static Component CreateSymbol(JsonObject node)
        {
            var symbol = new SymbolDecoration
            {
                Text = UILocalizedText.Resolve(node, "text"),
                TextureId = node.Str("texture"),
                FontRole = node.Str("font", "coc:button")
            };

            if (node.TryGetValue("size", out var size))
                symbol.SymbolSize = Int(size);

            return symbol;
        }

        // 二级菜单：外观来自节点（宽/内边距/行高/间距）与 itemTemplate（条目模板）。
        private Component CreateContextMenu(JsonObject node)
        {
            var menu = new ContextMenu
            {
                // 独立键：通用布局会把 node.padding 记为组件 padding，菜单再自己用一次就会左/上双倍、右/下贴边。
                Padding = node.Int("menuPadding", 4),
                ItemHeight = node.Int("itemHeight", 36),
                Spacing = node.Int("spacing", 2)
            };

            var template = ResolveNamedTemplate(node, "itemTemplate");

            if (template is not null)
                // 走 BuildTemplate：CreateButton 只建对象，states/hover 在后续步骤里应用。
            menu.ItemFactory = () => (Button)BuildTemplate(template);

            return menu;
        }

        // itemTemplate 可以是内联对象，也可以是文档 templates 里的名字（与 item_grid 同一套规则）。
        private JsonObject? ResolveNamedTemplate(JsonObject node, string key)
        {
            if (!node.TryGetValue(key, out var value))
                return null;

            if (value is JsonObject inline)
                return inline;

            var name = value.ToString().Trim('"');

            return _document is not null && _document.Templates.TryGetValue(name, out var named) ? named : null;
        }



        private static Component CreateDivider(JsonObject node)
        {
            var ornamentText = node.TryGetValue("ornamentText", out var ornamentValue)
                ? ornamentValue.ToString().Trim('"')
                : "✦";

            var divider = new DividerDecoration
            {
                Vertical = string.Equals(node.Str("orientation"), "vertical", StringComparison.OrdinalIgnoreCase) ||
                           node.Bool("vertical", false),
                Double = node.Bool("double", false),
                Ornament = node.Bool("ornament", true),
                OrnamentText = ornamentText,
                FontRole = node.Str("font", "coc:button")
            };

            if (node.TryGetValue("thickness", out var thickness))
                divider.Thickness = Int(thickness);

            if (node.TryGetValue("gap", out var gap))
                divider.Gap = Int(gap);

            if (node.TryGetValue("ornamentPadding", out var ornamentPadding))
                divider.OrnamentPadding = Int(ornamentPadding);

            return divider;
        }

        private static void ApplyDecorationStyle(Component component, JsonObject node)
        {
            if (component is not DecorationComponent decoration)
                return;

            var style = new UIDecorationStyle();

            if (node.TryGetValue("states", out var value) && value is JsonObject states)
            {
                ApplyDecorationState(states, "normal", out style.Normal, out style.NormalSecondary);
                ApplyDecorationState(states, "hover", out style.Hover, out style.HoverSecondary);
                ApplyDecorationState(states, "pressed", out style.Pressed, out style.PressedSecondary);
                ApplyDecorationState(states, "disabled", out style.Disabled, out style.DisabledSecondary);
            }

            decoration.Style = style;
        }

        private static void ApplyDecorationState(
            JsonObject states,
            string name,
            out Color primary,
            out Color secondary)
        {
            primary = Color.Transparent;
            secondary = Color.Transparent;

            if (!states.TryGetValue(name, out var value) || value is not JsonObject state)
                return;

            if (state.TryGetValue("color", out _))
                primary = ColorOf(state.Str("color"));

            if (state.TryGetValue("backgroundColor", out _))
                primary = ColorOf(state.Str("backgroundColor"));

            if (state.TryGetValue("secondaryColor", out _))
                secondary = ColorOf(state.Str("secondaryColor"));

            if (state.TryGetValue("borderColor", out _))
                secondary = ColorOf(state.Str("borderColor"));
        }

        private static Component CreateImage(JsonObject node)
        {
            var textureId = node.Str("texture");

            if (textureId.Length == 0)
                return null;

            var texture = TextureRegistry.Instance.Resolve(textureId);

            return texture is null ? null : new UIImage(texture);
        }

        private static Component CreatePanel(JsonObject node) =>
            new()
            {
                CanHit = node.Bool("canHit", false),

                BackgroundColor = ColorOf(node.Str("backgroundColor", "#00000000"))
            };

        private static Component CreateGradient(JsonObject node) =>
            new GradientPanel
            {
                FromColor = ColorOf(node.Str("fromColor", "#00000000")),
                ToColor = ColorOf(node.Str("toColor", "#00000000")),
                CanHit = false
            };

        private static Component CreateLabel(JsonObject node)
        {
            var label = new Label(ResolveFont(node, "coc:menu"));

            var labelBinding = ReadBinding(node, "text");

            if (labelBinding is not null)
                label.BindText(labelBinding);
            else
                label.Text(UILocalizedText.Resolve(node, "text"));

            label.TextColor = ColorOf(node.Str("textColor", "#000000"));

            if (node.TryGetValue("wrapWidth", out var wrap))
                label.WrapWidth(Int(wrap));
            else if (node.TryGetValue("width", out var labelWidth))
                label.WrapWidth(Int(labelWidth));

            if (node.TryGetValue("lineSpacing", out var spacing))
                label.LineSpacing(Int(spacing));

            var textAlign = node.Str("textHorizontalAlign", node.Str("textHAlign", node.Str("textAlign")));

            label.Alignment(textAlign.Trim().ToLowerInvariant() switch
            {
                "center" => TextHorizontalAlignment.Center,
                "right" => TextHorizontalAlignment.Right,
                "left" or "" => TextHorizontalAlignment.Left,
                _ => ReportInvalidTextHorizontal(textAlign)
            });

            var textVertical = node.Str("textVerticalAlign", node.Str("textVAlign"));

            label.VerticalAlign(textVertical.Trim().ToLowerInvariant() switch
            {
                "center" => TextVerticalAlignment.Center,
                "bottom" => TextVerticalAlignment.Bottom,
                "top" or "" => TextVerticalAlignment.Top,
                _ => ReportInvalidTextVertical(textVertical)
            });

            label.AutoSize = node.Bool("autoSize", false);

            return label;
        }

        private Component CreateButton(JsonObject node)
        {
            var background = node.TryGetValue("background", out var backgroundValue)
                ? backgroundValue.ToString().Trim('"')
                : node.Str("texture");

            var fontRole = node.TryGetValue("font", out var fontValue) && fontValue is JsonObject fontNode
                ? fontNode.Str("id", "coc:button")
                : node.Str("font", "coc:button");

            var button = new Button(background, "", fontRole);

            if (node.TryGetValue("font", out var sizeValue) && sizeValue is JsonObject sizeNode &&
                sizeNode.TryGetValue("size", out var size))
            {
                button.FontSizeOverride = Int(size);
            }

            button.SetLabel(UILocalizedText.Resolve(node, "label"));

            var labelAlign = node.Str("textHorizontalAlign", node.Str("textAlign", ""));

            if (labelAlign.Length > 0)
                button.LabelAlignment = labelAlign.Trim().ToLowerInvariant() switch
                {
                    "left" => TextHorizontalAlignment.Left,
                    "right" => TextHorizontalAlignment.Right,
                    _ => TextHorizontalAlignment.Center
                };

            if (node.TryGetValue("labelPadding", out var labelPadding))
                button.LabelPadding = Int(labelPadding);

            button.Label.RichTextLayout.Font = ResolveFont(node, "coc:button");

            var command = node.Str("command");

            if (command.Length > 0)
                button.Click(() => _events.Raise(command));

            if (node.TryGetValue("hint", out var hintValue) && hintValue is JsonObject hintNode)
                ApplyHint(button, hintNode);

            if (node.TryGetValue("enabled", out _))
                button.Enabled = node.Bool("enabled", true);

            if (node.TryGetValue("disableIf", out var disableIf))
            {
                var disableText = disableIf.ToString().Trim('"');

                if (bool.TryParse(disableText, out var disabledNow))
                    button.Enabled = !disabledNow;
                else
                    button.DisableIf = disableText;
            }

            var backgroundColor = node.Str("backgroundColor");

            if (backgroundColor.Length > 0)
            {
                var tint = ColorOf(backgroundColor);

                if (button.Background is not null)
                    button.Background.Color = tint;
                else
                    button.BackgroundColor = tint;
            }

            var textColor = node.Str("textColor");

            if (textColor.Length > 0)
                button.TextColor = ColorOf(textColor);

            button.RefreshVisual();

            return button;
        }

        private void ApplyHint(Button button, JsonObject node)
        {
            // 颜色/内边距默认取全局 ToolTip 模板（ToolTipView 启动时已写入 HintStyle.Default），
            // 节点里的字段再逐项覆盖。
            var style = new HintStyle
            {
                Padding = HintStyle.Default.Padding,
                HeaderColor = HintStyle.Default.HeaderColor,
                BodyColor = HintStyle.Default.BodyColor,
                SeparatorColor = HintStyle.Default.SeparatorColor
            };

            if (node.TryGetValue("width", out var width))
                style.Width = Int(width);

            if (node.TryGetValue("minHeight", out var minHeight))
                style.MinHeight = Int(minHeight);

            if (node.TryGetValue("padding", out var padding))
                style.Padding = Int(padding);

            if (node.TryGetValue("headerY", out var headerY))
                style.HeaderY = Int(headerY);

            if (node.TryGetValue("headerHeight", out var headerHeight))
                style.HeaderHeight = Int(headerHeight);

            if (node.TryGetValue("separatorY", out var separatorY))
                style.SeparatorY = Int(separatorY);

            if (node.TryGetValue("bodyY", out var bodyY))
                style.BodyY = Int(bodyY);

            if (node.TryGetValue("separatorWidth", out var separatorWidth))
                style.SeparatorWidth = Int(separatorWidth);

            style.Direction = node.Str("direction", "auto");

            if (node.TryGetValue("distance", out var distance))
                style.Distance = Int(distance);

            if (node.TryGetValue("offsetX", out var offsetX))
                style.OffsetX = Int(offsetX);

            if (node.TryGetValue("offsetY", out var offsetY))
                style.OffsetY = Int(offsetY);

            if (node.TryGetValue("offset", out var offsetValue) && offsetValue is JsonObject offset)
            {
                style.OffsetX = offset.Int("x", style.OffsetX);

                style.OffsetY = offset.Int("y", style.OffsetY);
            }

            style.HeaderFont = node.Str("headerFont", style.HeaderFont);

            style.BodyFont = node.Str("bodyFont", style.BodyFont);

            style.BackgroundTexture = node.Str("backgroundTexture");

            if (node.TryGetValue("backgroundColor", out _))
                style.BackgroundColor = ColorOf(node.Str("backgroundColor"));

            if (node.TryGetValue("alpha", out var alpha))
                style.Alpha = Float(alpha);

            if (node.TryGetValue("borderColor", out _))
                style.BorderColor = ColorOf(node.Str("borderColor"));

            if (node.TryGetValue("borderThickness", out var borderThickness))
                style.BorderThickness = Int(borderThickness);

            if (node.TryGetValue("headerColor", out _))
                style.HeaderColor = ColorOf(node.Str("headerColor"));

            if (node.TryGetValue("bodyColor", out _))
                style.BodyColor = ColorOf(node.Str("bodyColor"));

            if (node.TryGetValue("separatorColor", out _))
                style.SeparatorColor = ColorOf(node.Str("separatorColor"));

            var hintText = UILocalizedText.Resolve(node, "text");

            if (hintText.Length > 0)
                button.Hint = hintText;

            var hintHeader = UILocalizedText.Resolve(node, "header");

            if (hintHeader.Length > 0)
                button.HintHeader = hintHeader;

            var disabledHint = UILocalizedText.Resolve(node, "disabledHint");

            if (disabledHint.Length > 0)
                button.DisabledReason = disabledHint;

            button.HintSettings = style;

            var templateId = node.Str("controlTemplate");

            if (templateId.Length == 0 || _document is null)
                return;

            if (!_document.ControlTemplates.TryGetValue(templateId, out var template))
            {
                MonoCoC.UI.Console.ConsoleOverlay.WriteError($"[UI] Unknown hint control template '{templateId}'");

                return;
            }

            var root = BuildTemplateNode(template, _document);

            if (root is not null)
                button.HintView = new HintView(root, style);
        }

        private static SpriteFontBase ResolveFont(JsonObject node, string defaultRole)
        {
            var role = defaultRole;

            var size = 0;

            if (node.TryGetValue("font", out var fontValue))
            {
                if (fontValue is JsonObject font)
                {
                    role = font.Str("id", defaultRole);

                    if (font.TryGetValue("size", out var sizeValue))
                        size = Int(sizeValue);
                }
                else
                {
                    role = fontValue.ToString().Trim('"');
                }
            }

            if (size <= 0)
                size = UIFonts.Size(role);

            return UIFonts.GetAt(UIFonts.Family(role), size);
        }

        private static Component CreateTextArea(JsonObject node) =>
            new TextArea(ResolveFont(node, "coc:body"));

        private static Component CreateTextBox(JsonObject node)
        {
            var box = new TextBox(ResolveFont(node, "coc:input"));

            if (node.TryGetValue("textColor", out _))
                box.TextColor = ColorOf(node.Str("textColor"));

            if (node.TryGetValue("borderColor", out _))
                box.BorderColor = ColorOf(node.Str("borderColor"));

            if (node.TryGetValue("showBorder", out _))
                box.ShowBorder = node.Bool("showBorder", box.ShowBorder);

            if (node.TryGetValue("states", out var boxStateValue) && boxStateValue is JsonObject boxStates)
            {
                var boxBaseThickness = node.TryGetValue("borderThickness", out var boxBorderValue) && boxBorderValue is not JsonArray
                    ? Int(boxBorderValue)
                    : 1;

                var style = BuildStateStyle(boxStates, boxBaseThickness);

                if (node.TryGetValue("borderThickness", out var boxThickness))
                {
                    var sides = ReadSides(boxThickness);
                    if (sides is not null)
                    {
                        style.NormalBorderSides = sides;
                        style.HoverBorderSides = sides;
                        style.PressedBorderSides = sides;
                        style.SelectedBorderSides = sides;
                    }
                }

                box.StateStyle = style;
            }
            else if (node.TryGetValue("backgroundColor", out _) ||
                     node.TryGetValue("borderColor", out _) ||
                     node.TryGetValue("borderThickness", out _))
            {
                var style = new UIStateStyle
                {
                    NormalBackground = ColorOf(node.Str("backgroundColor", "#00000000")),
                    NormalBorder = box.BorderColor
                };

                if (node.TryGetValue("borderThickness", out var boxThickness))
                {
                    var sides = ReadSides(boxThickness);

                    if (sides is not null)
                        style.NormalBorderSides = sides;
                    else
                        style.NormalBorderThickness = Int(boxThickness);
                }

                box.StateStyle = style;
            }

            if (box.StateStyle is not null && node.TryGetValue("backgroundColor", out _))
            {
                var background = ColorOf(node.Str("backgroundColor"));

                box.StateStyle.NormalBackground = background;

                if (box.StateStyle.HoverBackground == Color.Transparent)
                    box.StateStyle.HoverBackground = background;

                if (box.StateStyle.PressedBackground == Color.Transparent)
                    box.StateStyle.PressedBackground = background;

                if (box.StateStyle.SelectedBackground == Color.Transparent)
                    box.StateStyle.SelectedBackground = background;
            }

            if (!box.ShowBorder && box.StateStyle is not null)
                box.StateStyle.NormalBorder = Color.Transparent;

            if (node.TryGetValue("characterSpacing", out var characterSpacing))
                box.CharacterSpacing = Float(characterSpacing);

            if (node.TryGetValue("keepPressDelay", out var keepPressDelay))
                box.KeepPressDelay = Int(keepPressDelay);

            var cursor = box.Cursor;

            if (node.TryGetValue("cursorColor", out _))
                cursor.CursorColor = ColorOf(node.Str("cursorColor"));


            if (node.TryGetValue("selectionColor", out _))
                cursor.SelectionColor = ColorOf(node.Str("selectionColor"));

            if (node.TryGetValue("cursorBlinkTime", out var blinkTime))
                cursor.BlinkTime = Int(blinkTime);

            box.Cursor = cursor;


            return box;
        }

        private static UIBinding? ReadBinding(JsonObject node, string name)
        {
            if (!node.TryGetValue(name, out var value) || value is not JsonObject binding)
                return null;

            if (!binding.TryGetValue("bind", out var path))
                return null;

            return new UIBinding(path.ToString().Trim('"'));
        }

        private static Component CreateEquipmentGrid(JsonObject node, UIDocument document)
        {
            var grid = new MonoCoC.UI.Components.EquipmentGrid
            {
                Columns = node.Int("columns", 1),
                SlotWidth = node.Int("slotWidth", 0),
                SlotHeight = node.Int("slotHeight", 84),
                Spacing = node.Int("spacing", 8),
                ItemTemplate = ResolveItemSlotStyle(node, document)
            };

            if (node.Array("areas") is { } areas)
            {
                foreach (var value in areas)
                {
                    if (value is not JsonObject area)
                        continue;

                    grid.Areas.Add(new MonoCoC.UI.Components.EquipmentArea
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

                    grid.Slots.Add(new MonoCoC.UI.Components.EquipmentSlotSpec
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

        private static Component CreateEquipmentSlot(JsonObject node) =>
            new MonoCoC.UI.Components.EquipmentSlot
            {
                Kind = node.Str("kind", "weapon"),
                LabelKey = UILocalizedText.Resolve(node, "label"),
                SlotIcon = node.Str("slotIcon"),
                SlotColor = ColorOf(node.Str("slotColor", "#F7EFE0CC")),
                BorderColor = ColorOf(node.Str("borderColor", "#B79A70AA")),
                TextColor = ColorOf(node.Str("textColor", "#35291FFF")),
                IconSize = node.Int("iconSize", 26)
            };

        private static MonoCoC.UI.Components.ItemSlotStyle? ResolveItemSlotStyle(JsonObject node, UIDocument document)
        {
            if (!node.TryGetValue("itemTemplate", out var templateValue))
                return null;

            JsonObject? template = null;

            if (templateValue is JsonObject inline)
                template = inline;
            else if (templateValue is not JsonObject &&
                     document.Templates.TryGetValue(templateValue.ToString().Trim('"'), out var named))
                template = named;

            if (template is null)
                return null;

            var style = new MonoCoC.UI.Components.ItemSlotStyle
            {
                Height = template.Int("height", 84),
                IconSize = template.Int("iconSize", 36),
                EmptyIcon = template.Str("emptyIcon", "Icons/inventory_empty.svg"),
                Padding = template.Int("padding", 12),
                Spacing = template.Int("spacing", 10),
                ShowName = template.Bool("showName", true),
                ShowType = template.Bool("showType", true),
                ShowQuantity = template.Bool("showQuantity", true)
            };

            if (template.TryGetValue("states", out var statesValue) && statesValue is JsonObject states)
                style.States = BuildStateStyle(states, template.Int("borderThickness", 0));

            return style;
        }

        private static Component CreateItemGrid(JsonObject node, UIDocument document)
        {
            var style = new MonoCoC.UI.Components.ItemSlotStyle();

            if (node.TryGetValue("itemTemplate", out var templateValue))
            {
                JsonObject? template = null;

                if (templateValue is JsonObject inline)
                    template = inline;
                else if (templateValue is not JsonObject &&
                         document.Templates.TryGetValue(templateValue.ToString().Trim('"'), out var named))
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

            return new MonoCoC.UI.Components.ItemGrid
            {
                Columns = node.Int("columns", 2),
                SlotWidth = node.Int("slotWidth", 0),
                SlotHeight = style.Height,
                Spacing = node.Int("spacing", 8),
                ItemTemplate = style
            };
        }

        private static Component CreatePlayerStats(JsonObject node)
        {
            var view = node.Str("view");

            if (view.Length == 0)
                throw new InvalidDataException("coc:player_stats requires 'view'");

            return new PlayerStats(view);
        }

        private static Component CreateStatRow(JsonObject node)
        {
            var row = new StatRow(ResolveFont(node, "coc:menu"));

            row.Label.Text(UILocalizedText.Resolve(node, "label"));

            row.ValueBinding = ReadBinding(node, "value");

            if (node.TryGetValue("labelColor", out _))
                row.Label.TextColor = ColorOf(node.Str("labelColor"));

            if (node.TryGetValue("valueColor", out _))
                row.Value.TextColor = ColorOf(node.Str("valueColor"));

            return row;
        }

        private static Component CreateBar(JsonObject node)
        {
            var bar = new Bar(ResolveFont(node, "coc:body"))
            {
                ValueBinding = ReadBinding(node, "value"),

                MaxBinding = ReadBinding(node, "max"),

                MinBinding = ReadBinding(node, "min"),

                MarkerBinding = ReadBinding(node, "marker")
            };

            if (bar.ValueBinding is null && node.TryGetValue("value", out var value))
                bar.Value = Float(value);

            if (node.TryGetValue("current", out var current))
                bar.Value = Float(current);

            if (bar.MaxBinding is null && node.TryGetValue("max", out var max))
                bar.Max = Float(max);

            if (bar.MinBinding is null && node.TryGetValue("min", out var min))
                bar.Min = Float(min);

            if (bar.MarkerBinding is null && node.TryGetValue("marker", out var marker))
                bar.Marker = Float(marker);

            if (node.TryGetValue("fillColor", out _))
                bar.FillColor = ColorOf(node.Str("fillColor"));

            if (node.TryGetValue("trackColor", out _))
                bar.TrackColor = ColorOf(node.Str("trackColor"));

            if (node.TryGetValue("borderColor", out _))
                bar.BorderColor = ColorOf(node.Str("borderColor"));

            if (node.TryGetValue("textColor", out _))
                bar.TextColor = ColorOf(node.Str("textColor"));

            if (node.TryGetValue("markerColor", out _))
                bar.MarkerColor = ColorOf(node.Str("markerColor"));

            if (node.TryGetValue("borderThickness", out var borderThickness))
                bar.BorderThickness = Int(borderThickness);

            if (node.TryGetValue("textPadding", out var textPadding))
                bar.TextPadding = Int(textPadding);

            bar.Label = UILocalizedText.Resolve(node, "label");

            bar.TextFormat = node.Str("textFormat", bar.TextFormat);

            if (node.TryGetValue("showText", out _))
                bar.ShowText = node.Bool("showText", bar.ShowText);

            return bar;
        }

        private static Component CreateNumber(JsonObject node)
        {
            var field = new NumberField
            {
                Min = node.Float("min", 0),
                Max = node.Float("max", 100),
                Step = node.Float("step", 1),
                Unit = node.Str("unit", "")
            };


            field.SetValue(node.Float("value", field.Min));

            return field;
        }

        private static Component CreateChoiceWheel(JsonObject node)
        {
            var wheel = new ChoiceWheel
            {
                VisibleItems = node.Int("visibleItems", 5),
                FadeMin = node.Float("fadeMin", 0.35f),
                AnimationSpeed = node.Float("animationSpeed", 0.18f),
                FontSize = node.Int("fontSize", 20),
                CenterFontSize = node.Int("centerFontSize", 26),
                Fade = node.Bool("fade", true),
                FadeColor = ColorOf(node.Str("fadeColor", "#E6D7BDFF")),
                FadeTop = node.Float("fadeTop", 0.2f),
                FadeBottom = node.Float("fadeBottom", 0.2f),
                ShowLocked = node.Bool("showLocked", false),
                LockColor = ColorOf(node.Str("lockColor", "#8A857CFF")),
                LockIcon = node.Str("lockIcon")
            };

            var thickness = node.Int("borderThickness", 0);

            if (node.TryGetValue("states", out var stateValue) && stateValue is JsonObject states)
            {
                var style = new UIStateStyle { BorderThickness = thickness };

                ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
                ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
                ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
                ApplyState(states, "selected", out style.SelectedBackground, out style.SelectedBorder, out style.SelectedText);

                ApplyStateThickness(states, "normal", ref style.NormalBorderThickness);
                ApplyStateThickness(states, "hover", ref style.HoverBorderThickness);
                ApplyStateThickness(states, "pressed", ref style.PressedBorderThickness);
                ApplyStateThickness(states, "selected", ref style.SelectedBorderThickness);

                InheritStateFallbacks(states, style);

                wheel.OptionStyle = style;

                if (HasStateField(states, "locked", "backgroundColor") ||
                    HasStateField(states, "locked", "textColor"))
                {
                    var locked = new UIStateStyle { BorderThickness = thickness };

                    ApplyState(states, "locked", out locked.NormalBackground, out locked.NormalBorder, out locked.NormalText);

                    locked.HoverBackground = locked.NormalBackground;
                    locked.HoverBorder = locked.NormalBorder;
                    locked.HoverText = locked.NormalText;
                    locked.PressedBackground = locked.NormalBackground;
                    locked.PressedBorder = locked.NormalBorder;
                    locked.PressedText = locked.NormalText;
                    locked.SelectedBackground = locked.NormalBackground;
                    locked.SelectedBorder = locked.NormalBorder;
                    locked.SelectedText = locked.NormalText;
                    locked.NormalBorderThickness = thickness;
                    locked.HoverBorderThickness = thickness;
                    locked.PressedBorderThickness = thickness;
                    locked.SelectedBorderThickness = thickness;

                    wheel.LockedStyle = locked;
                }
            }

            wheel.Load(node.Str("source"));

            return wheel;
        }

        private static Component CreateChoiceDetail(JsonObject node)
        {
            var detail = new ChoiceDetail(
                node.Int("visibleItems", 5),
                node.Str("font", "coc:menu"),
                node.Str("confirmKey"),
                node.Str("detailColor", "#35291FFF"),
                node.Bool("fade", true));

            detail.FadeMin = node.Float("fadeMin", 0.25f);

            detail.AnimationSpeed = node.Float("animationSpeed", 0.18f);

            detail.FontSize = node.Int("fontSize", 20);

            detail.CenterFontSize = node.Int("centerFontSize", 24);

            var thickness = node.Int("borderThickness", 0);

            if (node.TryGetValue("centerStates", out var centerValue) && centerValue is JsonObject centerStates)
            {
                var style = new UIStateStyle { BorderThickness = thickness };

                ApplyState(centerStates, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
                ApplyState(centerStates, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
                ApplyState(centerStates, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);

                detail.CenterStyle = style;
            }

            if (node.TryGetValue("otherStates", out var otherValue) && otherValue is JsonObject otherStates)
            {
                var style = new UIStateStyle { BorderThickness = thickness };

                ApplyState(otherStates, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
                ApplyState(otherStates, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
                ApplyState(otherStates, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);

                detail.OtherStyle = style;
            }

            detail.Load(node.Str("source"));

            return detail;
        }

        private static Component CreateSegmented(JsonObject node)
        {
            var control = new SegmentedControl
            {
                AnimationDuration = node.Float("animationDuration", 0.22f),
                AnimationEasing = MonoCoC.UI.Animations.Easing.FromName(node.Str("animationEasing"))
            };

            if (node.TryGetValue("states", out var stateValue) && stateValue is JsonObject states)
            {
                var style = new UIStateStyle
                {
                    BorderThickness = node.Int("borderThickness", 0)
                };

                ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
                ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
                ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
                ApplyState(states, "selected", out style.SelectedBackground, out style.SelectedBorder, out style.SelectedText);

                ApplyStateThickness(states, "normal", ref style.NormalBorderThickness);
                ApplyStateThickness(states, "hover", ref style.HoverBorderThickness);
                ApplyStateThickness(states, "pressed", ref style.PressedBorderThickness);
                ApplyStateThickness(states, "selected", ref style.SelectedBorderThickness);

                InheritStateFallbacks(states, style);

                control.OptionStyle = style;
            }

            var values = node.Array("options");
            var keyValues = node.Array("optionKeys");

            if (keyValues is not null)
            {
                var options = new List<string>();

                foreach (var value in keyValues)
                    if (value is not null)
                        options.Add(new UILocalizedText { Key = value.ToString().Trim('"') }.Resolve());

                control.SetOptions(options, node.Int("value", 0));
            }
            else if (values is not null)
            {
                var options = new List<string>();

                foreach (var value in values)
                    if (value is not null)
                        options.Add(value.ToString().Trim('"'));

                control.SetOptions(options, node.Int("value", 0));
            }

            return control;
        }

        private static Component CreateOptionList(JsonObject node)
        {
            var list = new OptionList
            {
                Columns = node.Int("columns", 4),
                Spacing = node.Int("spacing", 8)
            };

            if (node.TryGetValue("options", out var value) && value is JsonArray options)
            {
                var entries = new List<(string Id, string Label)>();

                foreach (var option in options)
                {
                    if (option is not JsonObject entry)
                        continue;

                    var id = entry.Str("id");

                    if (id.Length == 0)
                        continue;

                    entries.Add((id, UILocalizedText.Resolve(entry, "label")));
                }

                list.SetOptions(entries);
            }

            return list;
        }

        private static Component CreateNumericStepper(JsonObject node)
        {
            var stepper = new NumericStepper(
                node.Str("font", "coc:menu"),
                node.Int("fontSize", 18))
            {
                Min = node.Float("min", 0),
                Max = node.Float("max", 100),
                Step = node.Float("step", 1),
                ShiftMultiplier = node.Float("shiftMultiplier", 10),
                Unit = node.Str("unit"),
                ButtonWidth = node.Int("buttonWidth", 44),
                MinusColor = ColorOf(node.Str("minusColor", "#00000000")),
                PlusColor = ColorOf(node.Str("plusColor", "#00000000")),
                ValueColor = ColorOf(node.Str("valueColor", "#00000000")),
                TextColor = ColorOf(node.Str("textColor", "#35291FFF"))
            };

            if (node.Array("snapValues") is { } snapValues)
            {
                var values = new List<float>();

                foreach (var value in snapValues)
                    values.Add(Float(value));

                stepper.SnapValues = [.. values];
            }

            if (node.TryGetValue("states", out var stateValue) && stateValue is JsonObject states)
            {
                var thickness = node.Int("borderThickness", 0);

                stepper.MinusStyle = BuildPartStateStyle(states, "minus", thickness);
                stepper.PlusStyle = BuildPartStateStyle(states, "plus", thickness);
                stepper.ValueStyle = BuildPartStateStyle(states, "value", thickness);
            }

            stepper.ShowRange = node.TryGetValue("min", out _) || node.TryGetValue("max", out _);
            stepper.RangeColor = ColorOf(node.Str("rangeColor", "#5C4A38FF"));
            stepper.RangeFontSize = node.Int("rangeFontSize", 12);

            stepper.SetValue(node.Float("value", stepper.Min));

            return stepper;
        }

        private static Component CreateSlider(JsonObject node)
        {
            var slider = new Slider
            {
                Min = node.Float("min", 0),
                Max = node.Float("max", 100),
                Step = node.Float("step", 1),
                Unit = node.Str("unit", ""),
                TrackColor = ColorOf(node.Str("trackColor", "#00000000")),
                KnobColor = ColorOf(node.Str("knobColor", "#00000000")),
                TextColor = ColorOf(node.Str("textColor", "#00000000")),
                FontSize = node.Int("fontSize", 18)
            };

            slider.SetValue(node.Float("value", slider.Min));

            return slider;
        }

        private static Component CreateSelector(JsonObject node)
        {
            var selector = new Selector();

            var values = node.Array("options");

            if (values is not null)
            {
                var options = new List<string>();

                foreach (var value in values)
                    if (value is not null)
                        options.Add(value.ToString().Trim('"'));

                selector.SetOptions(options, node.Int("value", 0));
            }

            return selector;
        }

        private static Component CreateGrid(JsonObject node)
        {
            var rowDefinitions = ReadGridTracks(node, "rows", "rowDefinitions");

            var columnDefinitions = ReadGridTracks(node, "columns", "columnDefinitions");

            var grid = new Grid(
                rowDefinitions.Count > 0 ? rowDefinitions.Count : Math.Max(1, node.Int("rows", 1)),
                columnDefinitions.Count > 0 ? columnDefinitions.Count : Math.Max(1, node.Int("columns", 1)));

            if (rowDefinitions.Count > 0)
                grid.RowDefinitions = rowDefinitions;

            if (columnDefinitions.Count > 0)
                grid.ColumnDefinitions = columnDefinitions;

            grid.AutoSize = node.Bool("autoSize", false);

            grid.AutoIndex = node.Bool("autoIndex", false);

            if (node.TryGetValue("spacing", out var spacing))
                grid.Spacing = Int(spacing);

            return grid;
        }

        private static List<GridTrack> ReadGridTracks(JsonObject node, string countKey, string definitionsKey)
        {
            var result = new List<GridTrack>();

            var value = node.TryGetValue(definitionsKey, out var definitions)
                ? definitions
                : node.TryGetValue(countKey, out var count) ? count : null;

            if (value is not JsonArray array)
            {
                if (value is not null)
                {
                    var text = value.ToString().Trim('"').Trim();

                    if (text.StartsWith("auto", StringComparison.OrdinalIgnoreCase))
                    {
                        var repeat = text.Length == 4
                            ? 1
                            : int.TryParse(text[4..], NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
                                ? Math.Max(0, parsed)
                                : 0;

                        for (var i = 0; i < repeat; i++)
                            result.Add(GridTrack.Auto);
                    }
                }

                return result;
            }

            foreach (var entry in array)
                result.Add(ParseGridTrack(entry));

            return result;
        }

        private static GridTrack ParseGridTrack(JsonValue? value)
        {
            if (value is null)
                return GridTrack.Star();

            if (value is JsonObject obj)
            {
                var size = obj.Str("size", obj.Str("value", obj.Str("height", obj.Str("width", "1*"))));

                return ParseGridTrackString(size);
            }

            return ParseGridTrackString(value.ToString().Trim('"'));
        }

        private static GridTrack ParseGridTrackString(string value)
        {
            value = value.Trim();

            if (value.Equals("auto", StringComparison.OrdinalIgnoreCase))
                return GridTrack.Auto;

            if (value.EndsWith('*'))
            {
                var weight = value[..^1];

                return float.TryParse(weight, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed)
                    ? GridTrack.Star(parsed)
                    : GridTrack.Star();
            }

            return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var pixels)
                ? GridTrack.Pixels(pixels)
                : GridTrack.Star();
        }

        private static Component CreatePaperBackground(JsonObject node)
        {
            var paper = new PaperBackground();

            var settings = node.Str("preset").Trim().ToLowerInvariant() switch
            {
                "clean" => PaperSettings.Clean,
                "aged" => PaperSettings.Aged,
                _ => paper.Settings
            };

            if (node.TryGetValue("centerColor", out _))
                settings.CenterColor = ColorOf(node.Str("centerColor"));

            if (node.TryGetValue("edgeColor", out _))
                settings.EdgeColor = ColorOf(node.Str("edgeColor"));

            if (node.TryGetValue("edgeStrength", out var edgeStrength))
                settings.EdgeStrength = Float(edgeStrength);

            if (node.TryGetValue("lowNoiseStrength", out var lowNoise))
                settings.LowNoiseStrength = Float(lowNoise);

            if (node.TryGetValue("fiberStrength", out var fiber))
                settings.FiberStrength = Float(fiber);

            paper.Settings = settings;

            return paper;
        }

        private Component CreateCustomComponent(JsonObject node)
        {
            var componentId = node.Str("component");

            if (componentId.Length == 0)
                componentId = node.Str("widget");

            if (componentId.Length == 0)
                throw new InvalidDataException("Custom component node is missing 'component'");

            var properties = node.TryGetValue("properties", out var value) && value is JsonObject json
                ? json
                : [];

            if (!_customComponents.TryCreate(componentId, properties, out var component) || component is null)
                throw new InvalidDataException($"Unknown custom component '{componentId}'");

            return component;
        }

        private void ApplyControlTemplate(Button button, JsonObject node, UIDocument document)
        {
            var templateId = node.Str("controlTemplate");

            if (templateId.Length == 0)
                return;

            if (!document.ControlTemplates.TryGetValue(templateId, out var template))
            {
                Console.ConsoleOverlay.WriteError(
                    $"[UI] Unknown control template '{templateId}' in {document.Source}");

                return;
            }

            var root = BuildTemplateNode(template, document);

            if (root is not null)
                button.ApplyControlTemplate(root);
        }

        private Component? BuildTemplateNode(JsonObject node, UIDocument document)
        {
            node = document.Expand(node);

            var component = Create(node);

            if (component is null)
                return null;

            ApplyLayout(component, node);

            ApplyMaterial(component, node);

            ApplyStateStyle(component, node);

            ApplyDecorationStyle(component, node);

            BindEvents(component, node);

            BuildChildren(component, node, document);

            MakeRelative(component);

            if (component is Grid grid)
                grid.ResizeToFit();

            return component;
        }

        private static void MakeRelative(Component component)
        {
            foreach (var child in component.LayoutData.Children)
            {
                child.LayoutData.LayoutType = LayoutType.Relative;

                MakeRelative(child);
            }
        }

        private static void ApplyStateStyle(Component component, JsonObject node)
        {
            var hasStates = node.TryGetValue("states", out var value) && value is JsonObject;

            var hasThickness = node.TryGetValue("borderThickness", out var thickness);

            var hasBorderColor = node.TryGetValue("borderColor", out _);

            if (!hasStates && !hasThickness && !hasBorderColor)
                return;

            var style = new UIStateStyle();

            if (hasThickness)
            {
                var sides = ReadSides(thickness);

                if (sides is not null)
                {
                    style.NormalBorderSides = sides;
                    style.HoverBorderSides = sides;
                    style.PressedBorderSides = sides;
                    style.DisabledBorderSides = sides;
                    style.SelectedBorderSides = sides;
                }
                else
                {
                    style.BorderThickness = Int(thickness);
                    style.NormalBorderThickness = style.BorderThickness;
                    style.HoverBorderThickness = style.BorderThickness;
                    style.PressedBorderThickness = style.BorderThickness;
                    style.DisabledBorderThickness = style.BorderThickness;
                    style.SelectedBorderThickness = style.BorderThickness;
                }
            }

            if (hasStates)
            {
                var states = (JsonObject)value!;

                ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
                ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
                ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
                ApplyState(states, "disabled", out style.DisabledBackground, out style.DisabledBorder, out style.DisabledText);
                ApplyState(states, "selected", out style.SelectedBackground, out style.SelectedBorder, out style.SelectedText);

                ApplyStateThickness(states, "normal", ref style.NormalBorderThickness);
                ApplyStateThickness(states, "hover", ref style.HoverBorderThickness);
                ApplyStateThickness(states, "pressed", ref style.PressedBorderThickness);
                ApplyStateThickness(states, "disabled", ref style.DisabledBorderThickness);
                ApplyStateThickness(states, "selected", ref style.SelectedBorderThickness);

                style.NormalBorderSides = StateSides(states, "normal");
                style.HoverBorderSides = StateSides(states, "hover");
                style.PressedBorderSides = StateSides(states, "pressed");
                style.DisabledBorderSides = StateSides(states, "disabled");
                style.SelectedBorderSides = StateSides(states, "selected");

                InheritStateFallbacks(states, style);
            }

            if (hasBorderColor)
            {
                var border = ColorOf(node.Str("borderColor"));

                style.NormalBorder = border;

                if (style.HoverBorder == Color.Transparent)
                    style.HoverBorder = border;

                if (style.PressedBorder == Color.Transparent)
                    style.PressedBorder = border;

                if (style.DisabledBorder == Color.Transparent)
                    style.DisabledBorder = border;
            }

            component.StateStyle = style;
        }

        private static void ApplyState(
            JsonObject states,
            string name,
            out Color background,
            out Color border,
            out Color? text)
        {
            background = Color.Transparent;
            border = Color.Transparent;
            text = null;

            if (!states.TryGetValue(name, out var value) || value is not JsonObject state)
                return;

            if (state.TryGetValue("backgroundColor", out _))
                background = ColorOf(state.Str("backgroundColor"));

            if (state.TryGetValue("borderColor", out _))
                border = ColorOf(state.Str("borderColor"));

            if (state.TryGetValue("textColor", out _))
                text = ColorOf(state.Str("textColor"));
        }

        private static void ApplyStateThickness(JsonObject states, string name, ref int thickness)
        {
            if (!states.TryGetValue(name, out var value) || value is not JsonObject state)
                return;

            if (state.TryGetValue("borderThickness", out var stateThickness) && stateThickness is not JsonArray)
                thickness = Int(stateThickness);
        }

        private static BorderSides? ReadSides(JsonValue value)
        {
            if (value is not JsonArray array || array.Count == 0)
                return null;

            var values = new List<int>();

            foreach (var item in array)
                values.Add(Int(item));

            return values.Count switch
            {
                1 => BorderSides.Uniform(values[0]),
                2 => new BorderSides(values[0], values[1], values[0], values[1]),
                3 => new BorderSides(values[1], values[0], values[1], values[2]),
                4 => new BorderSides(values[0], values[1], values[2], values[3]),
                _ => null
            };
        }

        private static BorderSides? StateSides(JsonObject states, string name)
        {
            if (!states.TryGetValue(name, out var value) || value is not JsonObject state ||
                !state.TryGetValue("borderThickness", out var thickness))
                return null;

            return ReadSides(thickness);
        }

        private static UIStateStyle? ReadOptionalStateStyle(JsonObject node, string name, UIStateStyle? fallback)
        {
            if (!node.TryGetValue(name, out var value) || value is not JsonObject states)
                return fallback;

            return BuildStateStyle(states, node.Int("borderThickness", 0));
        }

        private static UIStateStyle BuildPartStateStyle(JsonObject states, string part, int thickness)
        {
            var style = new UIStateStyle { BorderThickness = thickness };

            style.NormalBackground = PartColor(states, "normal", part + "Background") ?? Color.Transparent;
            style.NormalText = PartColor(states, "normal", part + "TextColor");
            style.NormalBorder = PartColor(states, "normal", "borderColor") ?? Color.Transparent;

            style.HoverBackground = PartColor(states, "hover", part + "Background") ?? style.NormalBackground;
            style.HoverText = PartColor(states, "hover", part + "TextColor") ?? style.NormalText;
            style.HoverBorder = PartColor(states, "hover", "borderColor") ?? style.NormalBorder;

            style.PressedBackground = PartColor(states, "pressed", part + "Background") ?? style.NormalBackground;
            style.PressedText = PartColor(states, "pressed", part + "TextColor") ?? style.NormalText;
            style.PressedBorder = PartColor(states, "pressed", "borderColor") ?? style.NormalBorder;

            var normalThickness = PartThickness(states, "normal") ?? thickness;
            style.NormalBorderThickness = normalThickness;
            style.HoverBorderThickness = PartThickness(states, "hover") ?? normalThickness;
            style.PressedBorderThickness = PartThickness(states, "pressed") ?? normalThickness;

            return style;
        }

        private static Color? PartColor(JsonObject states, string stateName, string key)
        {
            if (!states.TryGetValue(stateName, out var value) || value is not JsonObject state)
                return null;

            return state.TryGetValue(key, out _) ? ColorOf(state.Str(key)) : null;
        }

        private static int? PartThickness(JsonObject states, string stateName)
        {
            if (!states.TryGetValue(stateName, out var value) || value is not JsonObject state)
                return null;

            return state.TryGetValue("borderThickness", out var thickness) ? Int(thickness) : null;
        }

        public static UIStateStyle BuildStateStyle(JsonObject states, int thickness)
        {
            var style = new UIStateStyle { BorderThickness = thickness };

            ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
            ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
            ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
            ApplyState(states, "selected", out style.SelectedBackground, out style.SelectedBorder, out style.SelectedText);
            ApplyStateThickness(states, "normal", ref style.NormalBorderThickness);
            ApplyStateThickness(states, "hover", ref style.HoverBorderThickness);
            ApplyStateThickness(states, "pressed", ref style.PressedBorderThickness);
            ApplyStateThickness(states, "selected", ref style.SelectedBorderThickness);

            style.NormalBorderSides = StateSides(states, "normal");
            style.HoverBorderSides = StateSides(states, "hover");
            style.PressedBorderSides = StateSides(states, "pressed");
            style.SelectedBorderSides = StateSides(states, "selected");

            InheritStateFallbacks(states, style);

            return style;
        }

        private static bool HasStateField(JsonObject states, string name, string field) =>
            states.TryGetValue(name, out var value) &&
            value is JsonObject state &&
            state.TryGetValue(field, out _);

        private static void InheritStateFallbacks(JsonObject states, UIStateStyle style)
        {
            if (!HasStateField(states, "hover", "backgroundColor"))
                style.HoverBackground = style.NormalBackground;

            if (!HasStateField(states, "hover", "borderColor"))
                style.HoverBorder = style.NormalBorder;

            if (!HasStateField(states, "hover", "textColor"))
                style.HoverText = style.NormalText;

            if (!HasStateField(states, "hover", "borderThickness"))
            {
                style.HoverBorderThickness = style.NormalBorderThickness;
                style.HoverBorderSides = style.NormalBorderSides;
            }

            if (!HasStateField(states, "pressed", "backgroundColor"))
                style.PressedBackground = style.NormalBackground;

            if (!HasStateField(states, "pressed", "borderColor"))
                style.PressedBorder = style.NormalBorder;

            if (!HasStateField(states, "pressed", "textColor"))
                style.PressedText = style.NormalText;

            if (!HasStateField(states, "pressed", "borderThickness"))
            {
                style.PressedBorderThickness = style.NormalBorderThickness;
                style.PressedBorderSides = style.NormalBorderSides;
            }

            if (!HasStateField(states, "selected", "backgroundColor"))
                style.SelectedBackground = style.NormalBackground;

            if (!HasStateField(states, "selected", "borderColor"))
                style.SelectedBorder = style.NormalBorder;

            if (!HasStateField(states, "selected", "textColor"))
                style.SelectedText = style.NormalText;

            if (!HasStateField(states, "selected", "borderThickness"))
            {
                style.SelectedBorderThickness = style.NormalBorderThickness;
                style.SelectedBorderSides = style.NormalBorderSides;
            }

            if (!HasStateField(states, "disabled", "backgroundColor"))
                style.DisabledBackground = style.NormalBackground;

            if (!HasStateField(states, "disabled", "borderColor"))
                style.DisabledBorder = style.NormalBorder;

            if (!HasStateField(states, "disabled", "textColor"))
                style.DisabledText = style.NormalText;

            if (!HasStateField(states, "disabled", "borderThickness"))
            {
                style.DisabledBorderThickness = style.NormalBorderThickness;
                style.DisabledBorderSides = style.NormalBorderSides;
            }
        }

        private void BindEvents(Component component, JsonObject node)
        {
            var click = EventId(node, "onClick", "command");

            if (click.Length > 0)
            {
                if (component is Button button)
                    button.OnClick = () => _events.Raise(click);
                else
                    component.OnClick = () => _events.Raise(click);
            }

            if (component is Button mouseButton)
            {
                var mouseDown = EventId(node, "onMouseDown");

                if (mouseDown.Length > 0)
                    mouseButton.OnMouseDown = () => _events.Raise(mouseDown);

                var mouseUp = EventId(node, "onMouseUp");

                if (mouseUp.Length > 0)
                    mouseButton.OnMouseUp = _ => _events.Raise(mouseUp);

                var mouseMove = EventId(node, "onMouseMove");

                if (mouseMove.Length > 0)
                    mouseButton.OnMouseMove = _ => _events.Raise(mouseMove);
            }

            var mouseEnter = EventId(node, "onMouseEnter");

            if (mouseEnter.Length > 0)
                component.OnMouseEnterAction = () => _events.Raise(mouseEnter);

            var mouseLeave = EventId(node, "onMouseLeave");

            if (mouseLeave.Length > 0)
                component.OnMouseLeaveAction = () => _events.Raise(mouseLeave);
        }

        private static string EventId(JsonObject node, string primary, string? fallback = null)
        {
            var command = node.Str(primary);

            if (command.Length == 0 && fallback is not null)
                command = node.Str(fallback);

            return command;
        }

        private static void ApplyMaterial(Component component, JsonObject node)
        {
            var materialId = node.Str("material");

            if (materialId.Length == 0)
                return;

            var config = node.TryGetValue("materialConfig", out var value) && value is JsonObject json
                ? json
                : null;

            if (MaterialRegistry.Instance.TryCreate(materialId, config, out var material) && material is not null)
                component.Material = material;
        }

        private static void ApplyLayout(Component component, JsonObject node)
        {
            if (node.TryGetValue("x", out var x))
                component.LayoutData.Position.X = Int(x);

            if (node.TryGetValue("y", out var y))
                component.LayoutData.Position.Y = Int(y);

            if (node.TryGetValue("width", out var width))
                component.LayoutData.Size.X = Int(width);

            if (node.TryGetValue("height", out var height))
                component.LayoutData.Size.Y = Int(height);

            if (node.TryGetValue("padding", out var padding))
                component.LayoutData.Padding = ParsePadding(padding);

            if (node.TryGetValue("alpha", out var alpha))
                component.LayoutData.Alpha = Float(alpha);

            if (node.TryGetValue("alphaType", out _))
                component.LayoutData.AlphaType = ParseAlphaType(node.Str("alphaType"));

            if (node.TryGetValue("sizeType", out _))
                component.LayoutData.SizeType = ParseSizeType(node.Str("sizeType"));

            if (node.TryGetValue("layout", out _))
                component.LayoutData.LayoutType = ParseLayoutType(node.Str("layout"));

            if (node.TryGetValue("gridRow", out var gridRow))
                component.LayoutData.GridRow = Int(gridRow);
            else if (node.TryGetValue("row", out var row))
                component.LayoutData.GridRow = Int(row);

            if (node.TryGetValue("gridColumn", out var gridColumn))
                component.LayoutData.GridColumn = Int(gridColumn);
            else if (node.TryGetValue("column", out var column))
                component.LayoutData.GridColumn = Int(column);

            if (node.TryGetValue("gridRowSpan", out var gridRowSpan))
                component.LayoutData.GridRowSpan = Math.Max(1, Int(gridRowSpan));
            else if (node.TryGetValue("rowSpan", out var rowSpan))
                component.LayoutData.GridRowSpan = Math.Max(1, Int(rowSpan));

            if (node.TryGetValue("gridColumnSpan", out var gridColumnSpan))
                component.LayoutData.GridColumnSpan = Math.Max(1, Int(gridColumnSpan));
            else if (node.TryGetValue("columnSpan", out var columnSpan))
                component.LayoutData.GridColumnSpan = Math.Max(1, Int(columnSpan));

            ApplyAlignment(component, node);

            if (node.TryGetValue("visible", out _))
                component.Visible = node.Bool("visible", true);

            if (node.TryGetValue("enabled", out _))
                component.Enabled = node.Bool("enabled", true);

            if (node.TryGetValue("scissor", out _))
                component.Scissor = node.Bool("scissor", false);

            if (node.TryGetValue("canHit", out _))
                component.CanHit = node.Bool("canHit", true);

            if (node.TryGetValue("backgroundColor", out _))
                component.BackgroundColor = ColorOf(node.Str("backgroundColor", "#00000000"));

            if (component is TextArea textArea)
            {
                if (node.TryGetValue("padding", out var textPadding))
                    textArea.Padding = ParsePadding(textPadding);

                if (node.TryGetValue("autoScroll", out _))
                    textArea.AutoScroll = node.Bool("autoScroll", textArea.AutoScroll);

                if (node.TryGetValue("scrollStep", out var scrollStep))
                    textArea.ScrollStep = Int(scrollStep);

                if (node.TryGetValue("textHorizontalAlign", out var textAlignValue))
                {
                    var textAlign = textAlignValue.ToString().Trim('"').Trim().ToLowerInvariant();

                    textArea.Text.HorizontalAlignment = textAlign switch
                    {
                        "center" => TextHorizontalAlignment.Center,
                        "right" => TextHorizontalAlignment.Right,
                        _ => TextHorizontalAlignment.Left
                    };
                }
            }

            var part = node.Str("part");

            if (part.Length > 0)
                component.PartName = part;
        }

        private static UIVec4 ParsePadding(object value)
        {
            if (value is JsonArray array && array.Count >= 4)
                return new UIVec4(Int(array[0]), Int(array[1]), Int(array[2]), Int(array[3]));

            if (value is JsonObject obj)
                return new UIVec4(
                    Int(obj.Str("top")),
                    Int(obj.Str("left")),
                    Int(obj.Str("right")),
                    Int(obj.Str("bottom")));

            return new UIVec4(Int(value));
        }

        private static void ApplyAlignment(Component component, JsonObject node)
        {
            var horizontal = node.Str("horizontalAlign", node.Str("hAlign"));

            var vertical = node.Str("verticalAlign", node.Str("vAlign"));

            var both = node.Str("alignment");

            if (both.Length > 0)
            {
                foreach (var part in both.Split('-', StringSplitOptions.RemoveEmptyEntries))
                {
                    var value = part.Trim().ToLowerInvariant();

                    if (value == "center")
                    {
                        horizontal = "center";
                        vertical = "center";
                    }
                    else if (value is "left" or "right" or "stretch")
                        horizontal = value;
                    else if (value is "top" or "bottom")
                        vertical = value;
                    else
                        ReportAlignmentError("alignment", value);
                }
            }

            if (horizontal.Length > 0)
                component.LayoutData.HorizontalAlign = ParseHorizontalAlign(horizontal);

            if (vertical.Length > 0)
                component.LayoutData.VerticalAlign = ParseVerticalAlign(vertical);
        }

        private static AlignHorizontal ParseHorizontalAlign(string value) =>
            value.Trim().ToLowerInvariant() switch
            {
                "center" => AlignHorizontal.Center,
                "right" => AlignHorizontal.Right,
                "stretch" => AlignHorizontal.Stretch,
                "left" => AlignHorizontal.Left,
                _ => ReportInvalidHorizontal(value)
            };

        private static AlignVertical ParseVerticalAlign(string value) =>
            value.Trim().ToLowerInvariant() switch
            {
                "center" => AlignVertical.Center,
                "bottom" => AlignVertical.Bottom,
                "stretch" => AlignVertical.Stretch,
                "top" => AlignVertical.Top,
                _ => ReportInvalidVertical(value)
            };

        private static AlignHorizontal ReportInvalidHorizontal(string value)
        {
            ReportAlignmentError("horizontalAlign", value);

            return AlignHorizontal.None;
        }

        private static AlignVertical ReportInvalidVertical(string value)
        {
            ReportAlignmentError("verticalAlign", value);

            return AlignVertical.None;
        }

        private static void ReportAlignmentError(string field, string value) =>
            MonoCoC.UI.Console.ConsoleOverlay.WriteError(
                $"[UI] Invalid {field} '{value}', using default.");

        private static TextHorizontalAlignment ReportInvalidTextHorizontal(string value)
        {
            ReportAlignmentError("textHorizontalAlign", value);

            return TextHorizontalAlignment.Left;
        }

        private static TextVerticalAlignment ReportInvalidTextVertical(string value)
        {
            ReportAlignmentError("textVerticalAlign", value);

            return TextVerticalAlignment.Top;
        }

        private static LayoutType ParseLayoutType(string value) =>
            string.Equals(value, "relative", StringComparison.OrdinalIgnoreCase)
                ? LayoutType.Relative
                : LayoutType.Absolute;

        private static SizeType ParseSizeType(string value) =>
            string.Equals(value, "fill", StringComparison.OrdinalIgnoreCase)
                ? SizeType.Fill
                : SizeType.Common;

        private static CommonDataType ParseAlphaType(string value) =>
            string.Equals(value, "inherit", StringComparison.OrdinalIgnoreCase)
                ? CommonDataType.Inherit
                : CommonDataType.Independent;

        private static int Int(object value) =>
            int.TryParse(value.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : 0;

        private static float Float(object value) =>
            float.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : 0f;

                private static Color ColorOf(string value)
        {
            value = value.Trim();

            if (value.Length == 0 || value[0] != '#')
                return Color.Transparent;

            var hex = value[1..];

            if (hex.Length == 6)
                hex += "FF";

            if (hex.Length != 8 ||
                !uint.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var rgba))
                return Color.Transparent;

            return new Color(
                (byte)((rgba >> 24) & 0xFF),
                (byte)((rgba >> 16) & 0xFF),
                (byte)((rgba >> 8) & 0xFF),
                (byte)(rgba & 0xFF));
        }

        // 标签页列表：条目由 OptionStore.Panes 动态生成，Controls 页固定插在第 3 项之后。
        // 用 Container + 显式坐标而不是 Grid：条目数量在构建期才确定，Grid 的轨道在 ApplyLayout 时
        // 已经按空 children 求过值，后续 AddChild 不会重算，会把所有按钮叠在同一位置。
        private Component CreateTabList(JsonObject node)
        {
            var template = ResolveNamedTemplate(node, "buttonTemplate");

            var width = node.Int("width", 300);

            var buttonWidth = node.Int("buttonWidth", 200);

            var buttonHeight = node.Int("buttonHeight", 60);

            var spacing = node.Int("spacing", 12);

            var container = new Container
            {
                LayoutData = { LayoutType = LayoutType.Relative }
            };

            var index = 0;

            void AddTab(string labelKey, Action clicked)
            {
                var buttonNode = template ?? new JsonObject();

                var button = (Button)CreateButton(buttonNode);

                // CreateButton 只建对象；states/字体等样式是 BuildChildren 里单独应用的，
                // 代码构造的条目必须手动补 ApplyStateStyle，否则选中态没有背景。
                ApplyStateStyle(button, buttonNode);

                button.SetLabel(labelKey.Localized());

                button.Click(clicked);

                button.LayoutData.LayoutType = LayoutType.Relative;

                button.LayoutData.Size = new Vector2(buttonWidth, buttonHeight);

                button.LayoutData.Position = new Vector2((width - buttonWidth) / 2f, index * (buttonHeight + spacing));

                container.AddChild(button);

                index++;
            }

            var panes = MonoCoC.Configuration.OptionStore.Instance.Panes;

            for (var paneIndex = 0; paneIndex < panes.Count; paneIndex++)
            {
                var pane = panes[paneIndex];

                // 必须复制到局部变量再进 lambda：for 的循环变量只声明一次，
                // 直接捕获会让所有标签都指向最后一个索引。
                var targetPane = paneIndex;

                AddTab(pane.ButtonKey, () => MonoCoC.UI.Components.Views.OptionsView.Current?.SelectPage(targetPane));

                if (paneIndex == 2)
                    AddTab("View.Options.Tab.Controls", () => MonoCoC.UI.Components.Views.OptionsView.Current?.OpenControlsPage());
            }

            return container;
        }    }
}

