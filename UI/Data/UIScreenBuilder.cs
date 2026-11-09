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
    /// <summary>
    /// Maps Hjson nodes to the existing MonoCoC Component/View tree.
    /// No new layout or rendering system is introduced.
    /// </summary>
    public sealed class UIScreenBuilder
    {
        private readonly UIEventRegistry _events;
        private readonly UIWidgetRegistry _widgets;
        private readonly UIComponentRegistry _types = new();

        private UIDocument? _document;

        public UIScreenBuilder(UIEventRegistry events, UIWidgetRegistry widgets)
        {
            _events = events;
            _widgets = widgets;

            RegisterCoreTypes();
        }

        public UIComponentRegistry Types => _types;

        private void RegisterCoreTypes()
        {
            _types.Register("coc:view", _ => new View());
            _types.Register("coc:image", CreateImage);
            _types.Register("coc:panel", CreatePanel);
            _types.Register("coc:label", CreateLabel);
            _types.Register("coc:button", CreateButton);
            _types.Register("coc:paper_background", CreatePaperBackground);
            _types.Register("coc:border", CreateBorder);
            _types.Register("coc:corners", CreateCorners);
            _types.Register("coc:symbol", CreateSymbol);
            _types.Register("coc:divider", CreateDivider);
            _types.Register("coc:grid", CreateGrid);
            _types.Register("coc:text_area", CreateTextArea);
            _types.Register("coc:text_box", CreateTextBox);
            _types.Register("coc:player_stats", _ => new PlayerStats());
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
            new Component
            {
                CanHit = node.Bool("canHit", false),

                BackgroundColor = ColorOf(node.Str("backgroundColor", "#00000000"))
            };

        private static Component CreateLabel(JsonObject node)
        {
            var label = new Label(UIFonts.Get(node.Str("font", "coc:menu")));

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

            var button = new Button(background, "", node.Str("font", "coc:button"));

            button.SetLabel(UILocalizedText.Resolve(node, "label"));

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
            var style = new HintStyle();

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

        private static Component CreateTextArea(JsonObject node) =>
            new TextArea(UIFonts.Get(node.Str("font", "coc:body")));

        private static Component CreateTextBox(JsonObject node) =>
            new TextBox(UIFonts.Get(node.Str("font", "coc:input")));

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
                return result;

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
            var widgetId = node.Str("widget");

            if (widgetId.Length == 0)
                throw new InvalidDataException("CustomWidget node is missing 'widget'");

            var properties = node.TryGetValue("properties", out var value) && value is JsonObject json
                ? json
                : new JsonObject();

            if (!_widgets.TryCreate(widgetId, properties, out var component) || component is null)
                throw new InvalidDataException($"Unknown custom widget '{widgetId}'");

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
            if (!node.TryGetValue("states", out var value) || value is not JsonObject states)
                return;

            var style = new UIStateStyle();

            if (node.TryGetValue("borderThickness", out var thickness))
                style.BorderThickness = Int(thickness);

            ApplyState(states, "normal", out style.NormalBackground, out style.NormalBorder, out style.NormalText);
            ApplyState(states, "hover", out style.HoverBackground, out style.HoverBorder, out style.HoverText);
            ApplyState(states, "pressed", out style.PressedBackground, out style.PressedBorder, out style.PressedText);
            ApplyState(states, "disabled", out style.DisabledBackground, out style.DisabledBorder, out style.DisabledText);

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

            if (value.Length == 0)
                return Color.Transparent;

            if (value[0] != '#')
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
    }
}

