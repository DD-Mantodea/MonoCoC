using MonoCoC.UI.Framework;
using MonoCoC.UI.Layout;

namespace MonoCoC.Extensions
{
    public static class ComponentExt
    {
        extension<T>(T component) where T : Component
        {
            public T Of(Component? parent)
            {
                parent?.AddChild(component);

                return component;
            }

            public T Enable(bool enable)
            {
                component.Enabled = enable;

                return component;
            }

            public T CanHit(bool canHit)
            {
                component.CanHit = canHit;

                return component;
            }

            public T Size(int width, int Height)
            {
                component.LayoutData.Size.X = width;

                component.LayoutData.Size.Y = Height;

                return component;
            }

            public T Position(int x, int y)
            {
                component.LayoutData.Position.X = x;

                component.LayoutData.Position.Y = y;

                return component;
            }

            public T LayoutType(LayoutType layoutType)
            {
                component.LayoutData.LayoutType = layoutType;

                return component;
            }

            public T SizeType(SizeType sizeType)
            {
                component.LayoutData.SizeType = sizeType;

                return component;
            }

            public T AlphaType(CommonDataType dataType)
            {
                component.LayoutData.AlphaType = dataType;

                return component;
            }

            public T GridIndex(int index)
            {
                component.LayoutData.GridIndex = index;

                return component;
            }
        }
    }
}
