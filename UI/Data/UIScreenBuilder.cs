using Hjson;
using Microsoft.Xna.Framework;
using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using MonoCoC.Extensions;
using MonoCoC.Graphics.Materials;
using MonoCoC.UI.Components;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;
using MonoCoC.UI.Structures;
using System;
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
            _types.Register("coc:grid", CreateGrid);
            _types.Register("coc:custom_widget", CreateCustomWidget);
        }

        public View Build(UIDocument document)
        {
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

            foreach (var childValue in children)
            {
                if (childValue is not JsonObject child)
                    continue;

                child = document.Expand(child);

                var component = Create(child);

                if (component is null)
                    continue;

                if (child.TryGetValue("gridIndex", out var gridIndex))
                    component.LayoutData.GridIndex = Int(gridIndex);

                ApplyMaterial(component, child);

                ApplyLayout(component, child);

                BindEvents(component, child);

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
            var label = new Label(UIFonts.Get(node.Str("font", "menu")));

            var text = node.Str("textKey");

            label.Text(text.Length > 0 ? text.Localized() : node.Str("text"));

            label.TextColor = ColorOf(node.Str("textColor", "#000000"));

            if (node.TryGetValue("wrapWidth", out var wrap))
                label.WrapWidth(Int(wrap));

            if (node.TryGetValue("lineSpacing", out var spacing))
                label.LineSpacing(Int(spacing));

            return label;
        }

        private Component CreateButton(JsonObject node)
        {
            var background = node.TryGetValue("background", out var backgroundValue)
                ? backgroundValue.ToString().Trim('"')
                : node.Str("texture");

            var button = new Button(background, "", node.Str("font", "button"));

            var label = node.Str("labelKey");

            button.SetLabel(label.Length > 0 ? label.Localized() : node.Str("text"));

            var command = node.Str("command");

            if (command.Length > 0)
                button.Click(() => _events.Raise(command));

            var hint = node.Str("hintKey");

            if (hint.Length > 0)
                button.Hint(hint.Localized());

            if (node.TryGetValue("enabled", out _))
                button.Enabled = node.Bool("enabled", true);

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

        private static Component CreateGrid(JsonObject node)
        {
            var grid = new Grid(node.Int("rows", 1), node.Int("columns", 1));

            grid.AutoSize = node.Bool("autoSize", false);

            if (node.TryGetValue("spacing", out var spacing))
                grid.Spacing = Int(spacing);

            return grid;
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

        private Component CreateCustomWidget(JsonObject node)
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

        private static void ApplyMaterial(Component component, JsonObject node)        {
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
