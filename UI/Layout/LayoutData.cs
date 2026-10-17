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

        public LayoutType LayoutType = LayoutType.Absolute;

        public UIVec4 Padding = 0;

        public Vector2 Position = Vector2.Zero;

        public SizeType SizeType = SizeType.Common;

        public Vector2 Size = Vector2.Zero;

        public CommonDataType AlphaType = CommonDataType.Independent;

        public float Alpha = 1;

        public readonly Vector2 ComputePosition()
        {
            switch (LayoutType)
            {
                case LayoutType.Absolute:
                    return Position;

                case LayoutType.Relative:
                    var origin = Parent?.Position ?? Vector2.Zero;

                    origin += Parent?.LayoutData.Padding.TopLeft() ?? Vector2.Zero;

                    origin += Parent?.SlotOf(GridIndex) ?? Vector2.Zero;

                    return origin + Position;

                default:
                    return Position;
            }
        }

        public readonly Vector2 ComputeSize()
        {
            return SizeType switch
            {
                SizeType.Common => Size,

                SizeType.Fill => Parent?.Size ?? Vector2.Zero,

                _ => Size,
            };
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
}