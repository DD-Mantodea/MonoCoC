using Microsoft.Xna.Framework;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Structures;
using System.Collections.Generic;

namespace MonoCoC.UI.Layout
{
    public struct LayoutData()
    {
        public Component? Parent;

        public List<Component> Children = [];

        public int GridIndex = -1;

        public int GridRow = -1;

        public int GridColumn = -1;

        public int GridRowSpan = 1;

        public int GridColumnSpan = 1;

        public LayoutType LayoutType = LayoutType.Absolute;

        public UIVec4 Padding = 0;

        public Vector2 Position = Vector2.Zero;

        public SizeType SizeType = SizeType.Common;

        public Vector2 Size = Vector2.Zero;

        public CommonDataType AlphaType = CommonDataType.Independent;

        public float Alpha = 1;

        public AlignHorizontal HorizontalAlign = AlignHorizontal.None;

        public AlignVertical VerticalAlign = AlignVertical.None;

        public readonly Vector2 ComputePosition()
        {
            if (Parent is null)
                return Position;

            if (LayoutType == LayoutType.Absolute &&
                HorizontalAlign == AlignHorizontal.None &&
                VerticalAlign == AlignVertical.None)
                return Position;

            var parentPadding = Parent.LayoutData.Padding;

            var origin = Parent.Position + parentPadding.TopLeft() + Parent.SlotOf(GridIndex);

            var area = Parent.SlotSize(GridIndex);

            if (area.X <= 0 || area.Y <= 0)
                area = Parent.Size - parentPadding.TopLeft() - parentPadding.BottomRight();

            var size = ComputeSize();

            var x = HorizontalAlign switch
            {
                AlignHorizontal.Center => origin.X + (area.X - size.X) * 0.5f,
                AlignHorizontal.Right => origin.X + area.X - size.X,
                AlignHorizontal.Stretch => origin.X,
                _ => origin.X
            };

            var y = VerticalAlign switch
            {
                AlignVertical.Center => origin.Y + (area.Y - size.Y) * 0.5f,
                AlignVertical.Bottom => origin.Y + area.Y - size.Y,
                AlignVertical.Stretch => origin.Y,
                _ => origin.Y
            };

            return new Vector2(x, y) + Position;
        }

        public readonly Vector2 ComputeSize()
        {
            switch (SizeType)
            {
                case SizeType.Common:
                    var size = Size;

                    if (Parent is null)
                        return size;

                    var area = Parent.SlotSize(GridIndex);

                    if (area.X <= 0 || area.Y <= 0)
                        area = Parent.Size - Parent.LayoutData.Padding.TopLeft() - Parent.LayoutData.Padding.BottomRight();

                    if (HorizontalAlign == AlignHorizontal.Stretch)
                        size.X = area.X;

                    if (VerticalAlign == AlignVertical.Stretch)
                        size.Y = area.Y;

                    return size;

                case SizeType.Fill:
                    return Parent?.Size ?? Vector2.Zero;

                default:
                    return Size;
            }
        }

        public readonly int ComputeWidth() => (int)ComputeSize().X;

        public readonly int ComputeHeight() => (int)ComputeSize().Y;

        private static T ComputeCommon<T>(T self, T parent, CommonDataType type)
        {
            return type switch
            {
                CommonDataType.Independent => self,

                CommonDataType.Inherit => parent,

                _ => self
            };
        }

        public readonly float ComputeAlpha() => ComputeCommon(Alpha, Parent?.Alpha ?? 1, AlphaType);
    }

    public enum LayoutType
    {
        Absolute,

        Relative
    }

    public enum SizeType
    {
        Common,

        Fill
    }

    public enum CommonDataType
    {
        Independent,

        Inherit
    }

    public enum AlignHorizontal
    {
        None,

        Left,

        Center,

        Right,

        Stretch
    }

    public enum AlignVertical
    {
        None,

        Top,

        Center,

        Bottom,

        Stretch
    }
}