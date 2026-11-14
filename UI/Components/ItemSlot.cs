using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Styles;
using System;

namespace MonoCoC.UI.Components
{
    public class ItemSlot : Component
    {
        public ItemStack? Stack;

        public int Index;

        public ItemSlotStyle Style = new();

        public Action<ItemStack>? Clicked;

        public override void Update(GameTime gameTime)
        {
            if (UserInput.MouseLeftPressed && Rectangle.Contains(UserInput.GetMousePos()))
            {
                Selected = true;

                if (Stack is not null && !Stack.IsEmpty)
                    Clicked?.Invoke(Stack);
            }

            base.Update(gameTime);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var style = Style.States;

            var background = style?.BackgroundFor(true, IsHovering, false, Selected) ?? new Color(0xF7, 0xEF, 0xE0, 0xCC);

            var border = style?.BorderFor(true, IsHovering, false, Selected) ?? new Color(0xB7, 0x9A, 0x70, 0xAA);

            var text = style?.TextFor(true, IsHovering, false, Selected) ?? new Color(0x35, 0x29, 0x1F, 0xFF);

            var thickness = style?.ThicknessFor(true, IsHovering, false, Selected) ?? 1;

            spriteBatch.DrawRectangle(Rectangle, background * LayoutData.Alpha);

            if (thickness > 0 && border != Color.Transparent)
                spriteBatch.DrawBorder(Rectangle, thickness, border * LayoutData.Alpha);

            if (Stack is null || Stack.IsEmpty)
            {
                var emptyIcon = SvgTextureCache.GetFile(CoC.Instance.GraphicsDevice,
                    "Icons/inventory_empty.svg", Style.IconSize,
                    new SkiaSharp.SKColor(text.R, text.G, text.B, text.A));

                if (emptyIcon is not null)
                    spriteBatch.Draw(emptyIcon, new Rectangle(Rectangle.X + Style.Padding, Rectangle.Y + (Rectangle.Height - Style.IconSize) / 2, Style.IconSize, Style.IconSize), Color.White * LayoutData.Alpha);

                UIFonts.Get("coc:menu").DrawText(spriteBatch, "Items.Type.Empty".LocalizedOr("Empty"),
                    new Vector2(Rectangle.X + Style.Padding + Style.IconSize + Style.Spacing, Rectangle.Y + 24),
                    text * LayoutData.Alpha);

                return;
            }

            var category = ItemCategories.Resolve(Stack.Item);

            var icon = SvgTextureCache.GetFile(CoC.Instance.GraphicsDevice,
                category.Icon, Style.IconSize,
                new SkiaSharp.SKColor(text.R, text.G, text.B, text.A));

            if (icon is not null)
                spriteBatch.Draw(icon, new Rectangle(Rectangle.X + Style.Padding, Rectangle.Y + (Rectangle.Height - Style.IconSize) / 2, Style.IconSize, Style.IconSize), Color.White * LayoutData.Alpha);

            var nameFont = UIFonts.Get("coc:menu");

            var small = UIFonts.GetAt(UIFonts.Family("coc:menu"), 14);

            var textX = Rectangle.X + Style.Padding + Style.IconSize + Style.Spacing;

            if (Style.ShowName)
                nameFont.DrawText(spriteBatch, Stack.InvLabel, new Vector2(textX, Rectangle.Y + 14), text * LayoutData.Alpha);

            if (Style.ShowType)
                small.DrawText(spriteBatch, category.LabelKey.LocalizedOr(category.Id), new Vector2(textX, Rectangle.Y + 42), text * 0.75f * LayoutData.Alpha);

            if (Style.ShowQuantity && Stack.Quantity > 1)
            {
                var qty = "×" + Stack.Quantity;

                var size = nameFont.MeasureString(qty);

                nameFont.DrawText(spriteBatch, qty, new Vector2(Rectangle.Right - 14 - size.X, Rectangle.Y + 14), text * LayoutData.Alpha);
            }
        }
    }
}
