using Hjson;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.UI.Data;
using MonoCoC.UI.Framework;
using MonoCoC.UI.Styles;
using System;
using System.Runtime.CompilerServices;

namespace MonoCoC.UI.Components
{
    public class ItemSlot : Component, IFromHjson<ItemSlot>
    {
        public ItemStack? Stack;

        public int Index;

        public ItemSlotStyle Style = new();

        public Action<ItemStack>? Clicked;

        public Action<ItemStack>? RightClicked;

        public override void Update(GameTime gameTime)
        {
            if (UserInput.MouseLeftPressed && Rectangle.Contains(UserInput.GetMousePos()) &&
                Stack is not null && !Stack.IsEmpty)
            {
                Selected = true;

                Clicked?.Invoke(Stack);
            }

            if (UserInput.MouseRightPressed && Rectangle.Contains(UserInput.GetMousePos()) &&
                Stack is not null && !Stack.IsEmpty)
            {
                // 右键也选中（与左键一致：高亮 + 详情面板），再弹交互菜单。
                Selected = true;

                Clicked?.Invoke(Stack);

                RightClicked?.Invoke(Stack);
            }

            base.Update(gameTime);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            var style = Style.States;

            var background = style?.BackgroundFor(true, IsHovering, false, Selected) ?? UIParseUtils.ColorOf("#F7EFE0CC");

            var border = style?.BorderFor(true, IsHovering, false, Selected) ?? UIParseUtils.ColorOf("#B79A70AA");

            var text = style?.TextFor(true, IsHovering, false, Selected) ?? UIParseUtils.ColorOf("#35291FFF");

            var thickness = style?.ThicknessFor(true, IsHovering, false, Selected) ?? 1;

            spriteBatch.DrawRectangle(Rectangle, background * LayoutData.Alpha);

            if (thickness > 0 && border != Color.Transparent)
                spriteBatch.DrawBorder(Rectangle, thickness, border * LayoutData.Alpha);

            if (Stack is null || Stack.IsEmpty)
            {
                var emptyIcon = SvgTextureCache.GetFile(CoC.Instance.GraphicsDevice,
                    Style.EmptyIcon, Style.IconSize,
                    new SkiaSharp.SKColor(text.R, text.G, text.B, text.A));

                if (emptyIcon is not null)
                    spriteBatch.Draw(emptyIcon, new Rectangle(Rectangle.X + Style.Padding, Rectangle.Y + (Rectangle.Height - Style.IconSize) / 2, Style.IconSize, Style.IconSize), Color.White * LayoutData.Alpha);

                UIFonts.Get("coc:menu").DrawText(spriteBatch, "Items.Type.Empty".LocalizedOr("Empty"),
                    new Vector2(Rectangle.X + Style.Padding + Style.IconSize + Style.Spacing, Rectangle.Y + 24),
                    text * LayoutData.Alpha);

                return;
            }

            var category = ItemCategories.Resolve(Stack.Item);

            var iconPath = string.IsNullOrWhiteSpace(Stack.Item.Icon) ? category.Icon : Stack.Item.Icon;

            var icon = SvgTextureCache.GetFile(CoC.Instance.GraphicsDevice,
                iconPath, Style.IconSize,
                new SkiaSharp.SKColor(text.R, text.G, text.B, text.A));

            if (icon is not null)
                spriteBatch.Draw(icon, new Rectangle(Rectangle.X + Style.Padding, Rectangle.Y + (Rectangle.Height - Style.IconSize) / 2, Style.IconSize, Style.IconSize), Color.White * LayoutData.Alpha);

            var nameFont = UIFonts.Get("coc:menu");

            var small = UIFonts.GetAt(UIFonts.Family("coc:menu"), 14);

            var textX = Rectangle.X + Style.Padding + Style.IconSize + Style.Spacing;

            if (Style.ShowName)
                nameFont.DrawText(spriteBatch, Stack.Item.Name, new Vector2(textX, Rectangle.Y + 14), text * LayoutData.Alpha);

            if (Style.ShowType)
                small.DrawText(spriteBatch, category.LabelKey.LocalizedOr(category.Id), new Vector2(textX, Rectangle.Y + 42), text * 0.75f * LayoutData.Alpha);

            if (Style.ShowQuantity && Stack.Quantity > 1)
            {
                var qty = "×" + Stack.Quantity;

                var size = nameFont.MeasureString(qty);

                nameFont.DrawText(spriteBatch, qty, new Vector2(Rectangle.Right - 14 - size.X, Rectangle.Y + 14), text * LayoutData.Alpha);
            }
        }
        public static string TypeId => "coc:item_slot";

        public static Component FromHjson(JsonObject node, UIBuildContext context) => new ItemSlot
        {
            Index = node.Int("index", 0)
        };

        [ModuleInitializer]
        internal static void Register() => ComponentFactory.Register<ItemSlot>();
    }
}
