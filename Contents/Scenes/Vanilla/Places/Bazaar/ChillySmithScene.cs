using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<ChillySmithScene>("coc:chilly_smith")]
    public class ChillySmithScene : Scene
    {

        public override void Entry()
        {
            SmithShop();
        }

        public void AddShopItem(float button, Identifier item, int price, int shop)
        {

            OutputText("Scenes.Chillysmith.AddShopItem.ShopItemLine".Localized(
                 Utils.Utils.CapitalizeFirstLetter(ItemRegistry.Create(item).LongName),

                 Utils.Utils.AddComma(price)
            ));

            AddButton((int)(button), ItemRegistry.Create(item).Name, () => TransactionItemConfirmation(item, price, shop));
        }

        public void TransactionItemConfirmation(Identifier item, int price, int shop)
        {
            ClearOutput();

            OutputText("Scenes.Chillysmith.TransactionItemConfirmation.SunInspectsItem".Localized(
                 ItemRegistry.Create(item).LongName,

                 NumberUtils.Num2Text(price)
            ));

            if (Player.Gems >= price)
            {
                OutputText("Scenes.Chillysmith.TransactionItemConfirmation.DoYouBuyIt".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), () => TransactionYes(item, price, shop));

                AddButton(1, "Common.Button.No".Localized(), () => TransactionNo(shop));
            }
            else
            {
                OutputText("Scenes.Chillysmith.TransactionItemConfirmation.CannotAfford".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => TransactionNo(shop));
            }
        }

        public void TransactionYes(Identifier item, int price, int shop)
        {

            Action shopToGo = shop == 1 ? BuySomeWeapons : BuySomeArmor;

        ClearOutput();

            if (Player.Gems >= price)
            {
                OutputText("Scenes.Chillysmith.TransactionYes.HandsItemOver".Localized(
                     ItemRegistry.Create(item).LongName
                ));

                Player.Gems -= price;

                ClearButtons();

                InventoryScene.TakeItem(item, shopToGo);
            }
            else
            {
                OutputText("Scenes.Chillysmith.TransactionYes.CannotAffordTrade".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => TransactionNo(shop));
            }
        }

        public void TransactionNo(int shop)
        {

            if (shop == 1)
                BuySomeWeapons();
            else
                BuySomeArmor();
        }

        public void SmithButton(bool setButtonOnly = false)
        {

            if (!setButtonOnly)
            {
                OutputText("Scenes.Chillysmith.SmithButton.WagonBlurbLead".Localized());

                if (silly)
                    OutputText("Scenes.Chillysmith.SmithButton.ChilisName".Localized());
                else
                    OutputText("Scenes.Chillysmith.SmithButton.ChillySmithName".Localized());

                OutputText("Scenes.Chillysmith.SmithButton.MutedColors".Localized());
            }
            else
                AddButton(3, "Common.Button.CSmith".Localized(), SmithShop);
        }

        public void SmithShop()
        {
            ClearOutput();

            OutputText("Scenes.Chillysmith.SmithShop.EnterLead".Localized());

            if (silly)
                OutputText("Scenes.Chillysmith.SmithShop.TheChilisName".Localized());
            else
                OutputText("Scenes.Chillysmith.SmithShop.ChillySmithName".Localized());

            OutputText("Scenes.Chillysmith.SmithShop.GlanceAround".Localized((!metric ? "6'4\"" : "193 cm")));

            ClearButtons();

            AddButton(0, "Common.Button.Buy".Localized(), BuySomeShit);

            AddButton(1, "Common.Button.Talk".Localized(), TheFuckIsYouWho);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());
        }

        public void BuySomeShit()
        {
            ClearOutput();

            OutputText("Scenes.Chillysmith.BuySomeShit.AnnouncesBuying".Localized());

            ClearButtons();

            AddButton(0, "Scenes.Storage.WeaponsName".Localized(), BuySomeWeapons);

            AddButton(1, "Common.Button.Armor".Localized(), BuySomeArmor);

            AddButton(14, "Common.Button.Back".Localized(), SmithShop);
        }

        public void BuySomeWeapons()
        {
            ClearOutput();

            OutputText("Scenes.Chillysmith.BuySomeWeapons.LooksAtWeapons".Localized());

            ClearButtons();

            AddShopItem(0, ItemIds.Pitchfork, 200, 1);

            AddShopItem(1, ItemIds.Dagger, 150, 1);

            AddShopItem(2, ItemIds.RidingCrop, 50, 1);

            AddShopItem(3, ItemIds.SuccubusWhip, 400, 1);

            AddShopItem(4, ItemIds.Scimitar, 500, 1);

            AddShopItem(5, ItemIds.Spear, 450, 1);

            AddShopItem(6, WeaponIds.BeautifulSword, 800, 1);

            AddShopItem(7, ItemIds.MidnightRapier, 25000, 1);

            AddButton(14, "Common.Button.Back".Localized(), BuySomeShit);
        }

        public void BuySomeArmor()
        {
            ClearOutput();

            OutputText("Scenes.Chillysmith.BuySomeArmor.LooksAtArmor".Localized());

            ClearButtons();

            AddShopItem(0, ArmorIds.DragonScaleArmor, 1000, 2);

            AddShopItem(1, ArmorIds.SilkArmor, 2000, 2);

            AddShopItem(2, ArmorIds.PlateArmor, 700, 2);

            AddShopItem(3, ArmorIds.ScaleArmor, 500, 2);

            AddShopItem(4, ArmorIds.EBNJACK, 6000, 2);

            AddShopItem(5, ArmorIds.SilkRobes, 2500, 2);

            AddShopItem(6, ArmorIds.BimboOutfit, 250, 2);

            AddButton(14, "Common.Button.Back".Localized(), BuySomeShit);
        }

        public void TheFuckIsYouWho()
        {
            ClearOutput();

            OutputText("Scenes.Chillysmith.TheFuckIsYouWho.GreetingRejected".Localized());

            if (Random.Shared.Next(4) == 0)
            {
                OutputText("Scenes.Chillysmith.TheFuckIsYouWho.GemsCrammed".Localized());

                if (silly)
                    OutputText("Scenes.Chillysmith.TheFuckIsYouWho.DollarsWord".Localized());
                else
                    OutputText("Scenes.Chillysmith.TheFuckIsYouWho.GemsWord".Localized());

                OutputText("Scenes.Chillysmith.TheFuckIsYouWho.FuckOffSuffix".Localized());

                Player.Gems += 5;
            }
            else
                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Chillysmith.TheFuckIsYouWho.HardGlare".Localized());
                else
                    OutputText("Scenes.Chillysmith.TheFuckIsYouWho.HasBoyfriend".Localized());

            DoNext(() => SmithShop());
        }

    }
}