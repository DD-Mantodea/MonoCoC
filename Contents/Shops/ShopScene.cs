using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Shops
{

    public abstract class ShopScene : Scene
    {
        protected readonly PagedMenu<ShopEntry> Pager = new();

        protected abstract Identifier ShopId { get; }

        protected override void ReturnToPrevious() =>

            Go<TelAdre>();

        protected ShopDefinition Shop => ShopCatalog.Get(ShopId) ?? new ShopDefinition(ShopId, "", "", 1f, []);

        protected void RenderShop()
        {
            ClearButtons();

            Pager.Items = Shop.Items;

            Pager.PageSize = 10;

            Pager.Render(
                (slot, label, action) =>
                {
                    if (action == null)
                        AddButtonDisabled(slot, label, "");
                    else
                        AddButton(slot, label, action);
                },

                e => "Common.Text.G".Localized(ItemLabel(e), PriceOf(e)),

                e => () => ConfirmBuy(e),

                ReturnToPrevious,

                RenderShop,

                "Prev",

                "Next",

                "Leave",

                0);
        }

        protected virtual void ConfirmBuy(ShopEntry entry)
        {
            var price = PriceOf(entry);

            if (Player.Gems < price)
            {
                ClearOutput();

                OutputText("Common.Text.YouDontHaveEnoughGemsFor".Localized());

                RenderShop();

                return;
            }

            Player.Gems -= price;

            Player.Inventory.Add(ItemRegistry.Create(entry.Item), 1);

            ClearOutput();

            OutputText("Common.Text.PurchasedForGemsNN".Localized(ItemLabel(entry), price));

            RenderShop();
        }

        protected void Debit(Identifier item)
        {
            ConfirmBuy(new ShopEntry(item, 0, -1));
        }

        protected int PriceOf(ShopEntry entry)
        {
            if (entry.PriceOverride >= 0)
                return entry.PriceOverride;

            var item = ItemRegistry.Create(entry.Item);

            return (int)Math.Max(1, Math.Round(item.Value * Shop.Markup));
        }

        protected static string ItemLabel(ShopEntry entry)
        {
            var item = ItemRegistry.Create(entry.Item);

            return item.LongName;
        }
    }
}