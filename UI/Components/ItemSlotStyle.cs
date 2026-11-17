using MonoCoC.UI.Styles;

namespace MonoCoC.UI.Components
{
    public class ItemSlotStyle
    {
        public int Height = 84;

        public int IconSize = 36;

        public string EmptyIcon = "Icons/inventory_empty.svg";

        public int Padding = 12;

        public int Spacing = 10;

        public bool ShowName = true;

        public bool ShowType = true;

        public bool ShowQuantity = true;

        public UIStateStyle? States;
    }
}
