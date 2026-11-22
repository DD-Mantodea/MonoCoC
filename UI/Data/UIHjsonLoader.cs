using FontStashSharp;
using FontStashSharp.RichText;
using Hjson;
using Microsoft.Xna.Framework;
using MonoCoC.Assets;
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
    public class UIHjsonLoader
    {
        private readonly UIEventRegistry _events;

        private UIDocument? _document;

        private readonly UIBuildContext _context;

        public UIHjsonLoader(UIEventRegistry events)
        {
            _events = events;

            _context = new UIBuildContext(_events);

        }

        public View Build(UIDocument document)
        {
            _document = document;

            _context.Document = document;

            _context.TemplateBuilder = BuildTemplateNode;

            var rootNode = document.Root.TryGetValue("root", out var rootValue) && rootValue is JsonObject rootObject
                ? rootObject
                : document.Root;

            rootNode = document.Expand(rootNode);

            var root = new View();

            root.ApplyLayout(rootNode);

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
                    component.LayoutData.GridIndex = UIParseUtils.Int(gridIndex);

                sequentialIndex++;

                component.ApplyStyle(child);

                component.ApplyTemplate(child, _context);

                component.BindEvents(child, _context.Events);

                ApplyDecorationStyle(component, child);

                parent.AddChild(component);

                BuildChildren(component, child, document);

                if (component is Grid grid)
                    grid.ResizeToFit();
            }
        }

        public Component BuildTemplate(JsonObject template)
        {
            var component = Create(template) ?? new Component();

            component.ApplyStyle(template);

            component.ApplyTemplate(template, _context);

            component.BindEvents(template, _context.Events);

            return component;
        }
        private Component? Create(JsonObject node)
        {
            var type = node.Str("type", "Panel");

            if (!type.Contains(':', StringComparison.Ordinal))
                type = "coc:" + type.ToLowerInvariant();

            if (!ComponentFactory.TryCreate(type, node, _context, out var component))
                throw new InvalidDataException($"Unknown UI component type '{type}'");

            return component;
        }

        private JsonObject? ResolveNamedTemplate(JsonObject node, string key)
        {
            if (!node.TryGetValue(key, out var value))
                return null;

            if (value is JsonObject inline)
                return inline;

            var name = value.ToString().Trim('"');

            return _document is not null && _document.Styles.TryGetValue(name, out var named) ? named : null;
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
                primary = UIParseUtils.ColorOf(state.Str("color"));

            if (state.TryGetValue("backgroundColor", out _))
                primary = UIParseUtils.ColorOf(state.Str("backgroundColor"));

            if (state.TryGetValue("secondaryColor", out _))
                secondary = UIParseUtils.ColorOf(state.Str("secondaryColor"));

            if (state.TryGetValue("borderColor", out _))
                secondary = UIParseUtils.ColorOf(state.Str("borderColor"));
        }



        private static UIBinding? ReadBinding(JsonObject node, string name)
        {
            if (!node.TryGetValue(name, out var value) || value is not JsonObject binding)
                return null;

            if (!binding.TryGetValue("bind", out var path))
                return null;

            return new UIBinding(path.ToString().Trim('"'));
        }
        private static ItemSlotStyle? ResolveItemSlotStyle(JsonObject node, UIDocument document)
        {
            if (!node.TryGetValue("itemStyle", out var styleValue))
                return null;

            JsonObject? template = null;

            if (styleValue is JsonObject inline)
                template = inline;
            else if (styleValue is not JsonObject &&
                     document.Styles.TryGetValue(styleValue.ToString().Trim('"'), out var named))
                template = named;

            if (template is null)
                return null;

            var style = new ItemSlotStyle
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
                style.States = Component.BuildStateStyle(states, template.Int("borderThickness", 0));

            return style;
        }

        private Component? BuildTemplateNode(JsonObject node, UIDocument document)
        {
            node = document.Expand(node);

            var component = Create(node);

            if (component is null)
                return null;

            component.ApplyLayout(node);

            component.ApplyMaterial(node);

            component.ApplyStateStyle(node);

            ApplyDecorationStyle(component, node);

            component.BindEvents(node, _context.Events);

            BuildChildren(component, node, document);

            component.MakeRelative();

            if (component is Grid grid)
                grid.ResizeToFit();

            return component;
        }



    }
}