using Hjson;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;

namespace MonoCoC.UI.Components
{
    public static class ItemSlotStyleResolver
    {
        public static ItemSlotStyle? Resolve(JsonObject node, UIDocument document)
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

    }
}

