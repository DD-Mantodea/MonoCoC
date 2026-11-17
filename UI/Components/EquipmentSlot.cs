using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoCoC.Assets;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.UI.Framework;
using System;

namespace MonoCoC.UI.Components
{
    public class EquipmentSlot : Component
    {
        public string Kind = "weapon";

        public string SlotId = "";

        public bool IsExtra;

        public string LabelKey = "";

        public string SlotIcon = "";

        public Color SlotColor = UIComponentStyles.Slot.Background;

        public Color BorderColor = UIComponentStyles.Slot.Border;

        public Color TextColor = UIComponentStyles.Slot.Text;

        public Color SelectedBackground = UIComponentStyles.Slot.SelectedBackground;

        public Color SelectedBorder = UIComponentStyles.Slot.SelectedBorder;

        public ItemSlotStyle? Style;

        public int IconSize = 36;

        public int Padding = 12;

        public int Spacing = 10;

        public bool ShowLabel = true;

        public Action<Item?>? Clicked;

        public Action<string, string>? Activated;

        public Action<Item?, string, string>? RightClicked;

        // 供装备网格禁用某个槽位（例如双手武器占用后，另一只手禁用）。
        public bool Disabled;

        public override void Update(GameTime gameTime)
        {
            if (Disabled)
            {
                base.Update(gameTime);

                return;
            }

            var player = CoC.Instance.Player;

            if (Visible && UserInput.MouseLeftPressed && Rectangle.Contains(UserInput.GetMousePos()))
            {
                Item? item;

                if (IsExtra)
                    item = player.ExtraEquipment.TryGetValue(SlotId, out var extra) ? extra : null;
                else
                    item = player.GetEquipped(Kind);

                if (item is not null)
                {
                    Activated?.Invoke(SlotId, Kind);

                    Clicked?.Invoke(item);
                }
            }

            if (Visible && UserInput.MouseRightPressed && Rectangle.Contains(UserInput.GetMousePos()))
            {
                Item? item;

                if (IsExtra)
                    item = player.ExtraEquipment.TryGetValue(SlotId, out var extra) ? extra : null;
                else
                    item = player.GetEquipped(Kind);

                // 右键同样先选中（与左键一致），再弹菜单。
                if (item is not null)
                    Activated?.Invoke(SlotId, Kind);

                RightClicked?.Invoke(item, SlotId, Kind);
            }

            base.Update(gameTime);
        }

        public override void DrawSelf(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawSelf(spriteBatch, gameTime);

            var player = CoC.Instance.Player;

            Item? item;

            if (IsExtra)
                item = player.ExtraEquipment.TryGetValue(SlotId, out var extra) ? extra : null;
            else
                item = player.GetEquipped(Kind);

            var empty = item is null;

            // 被禁用的槽位（例如双手武器占满后另一只手）整体画淡，并且不接受点击。
            var alpha = Disabled ? LayoutData.Alpha * 0.35f : LayoutData.Alpha;

            var font = UIFonts.Get("coc:menu");

            var small = UIFonts.GetAt(UIFonts.Family("coc:menu"), 14);

            var style = Style?.States;

            var background = style?.BackgroundFor(true, IsHovering, false, Selected) ?? (Selected ? SelectedBackground : SlotColor);

            var border = style?.BorderFor(true, IsHovering, false, Selected) ?? (Selected ? SelectedBorder : BorderColor);

            var thickness = style?.ThicknessFor(true, IsHovering, false, Selected) ?? (Selected ? 2 : 1);

            spriteBatch.DrawRectangle(Rectangle, background * alpha);

            if (thickness > 0 && border != Color.Transparent)
                spriteBatch.DrawBorder(Rectangle, thickness, border * alpha);

            if (ShowLabel)
                small.DrawText(spriteBatch, LabelKey.LocalizedOr(Kind),
                    new Vector2(Rectangle.X + 10, Rectangle.Y + 6), TextColor * 0.75f * alpha);

            var name = empty ? "Items.Type.Empty".Localized() : item!.Name;

            if (empty)
            {
                var emptyIcon = string.IsNullOrWhiteSpace(SlotIcon)
                    ? null
                    : SvgTextureCache.GetFile(CoC.Instance.GraphicsDevice, SlotIcon, IconSize,
                        new SkiaSharp.SKColor(TextColor.R, TextColor.G, TextColor.B, 120));

                if (emptyIcon is not null)
                    spriteBatch.Draw(emptyIcon, new Rectangle(Rectangle.X + Padding, Rectangle.Y + (Rectangle.Height - IconSize) / 2, IconSize, IconSize), Color.White * alpha);

                small.DrawText(spriteBatch, name, new Vector2(Rectangle.X + Padding + IconSize + Spacing, Rectangle.Y + 24), TextColor * 0.75f * alpha);

                return;
            }

            var category = ItemCategories.Resolve(item!);

            var iconPath = string.IsNullOrWhiteSpace(item!.Icon) ? category.Icon : item.Icon;

            var icon = SvgTextureCache.GetFile(CoC.Instance.GraphicsDevice, iconPath, IconSize,
                new SkiaSharp.SKColor(TextColor.R, TextColor.G, TextColor.B, TextColor.A));

            if (icon is not null)
                spriteBatch.Draw(icon, new Rectangle(Rectangle.X + Padding, Rectangle.Y + (Rectangle.Height - IconSize) / 2, IconSize, IconSize), Color.White * alpha);

            var textX = Rectangle.X + Padding + IconSize + Spacing;

            font.DrawText(spriteBatch, name, new Vector2(textX, Rectangle.Y + 14), TextColor * alpha);

            small.DrawText(spriteBatch, category.LabelKey.LocalizedOr(category.Id),
                new Vector2(textX, Rectangle.Y + 42), TextColor * 0.75f * alpha);
        }
    }
}
