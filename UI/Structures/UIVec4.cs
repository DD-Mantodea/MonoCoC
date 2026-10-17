using System.Numerics;

namespace MonoCoC.UI.Structures
{
    public struct UIVec4(int top, int left, int right, int bottom)
    {
        public UIVec4(int value) : this(value, value, value, value) { }

        public int Top = top;

        public int Left = left;

        public int Right = right;

        public int Bottom = bottom;

        public static implicit operator UIVec4(int value) => new(value);

        public readonly Vector2 TopLeft() => new(Left, Top);
    }
}