using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Shops;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<JewelryShop>("coc:jewelry")]
    public class JewelryShop : ShopScene
    {

        public bool firstEntry = true;

        protected override Identifier ShopId => SceneIds.TelAdre.JewelryShop;

        public override void Entry()
        {
            Enter();
        }

        public void Enter()
        {
            ClearOutput();

            if (firstEntry)
            {
                OutputText("Scenes.JewelryShop.Enter.StallNoticed".Localized());

                OutputText("Scenes.JewelryShop.Enter.HoodedPeddler".Localized());

                OutputText("Scenes.JewelryShop.Enter.PeddlerAppears".Localized());

                firstEntry = false;
            }
            else
                OutputText("Scenes.JewelryShop.Enter.MerchantGreets".Localized());

            OutputText("Scenes.JewelryShop.Enter.ParagraphBreak".Localized());

            DoNext(() => Inside());
        }

        public void Inside()
        {
            ClearOutput();

            OutputText("Scenes.JewelryShop.Inside.WhatWillItBe".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.NormalRings".Localized(), NormalRings);

            AddButton(1, "Common.Button.SpecialRings".Localized(), SpecialRings);

            AddButton(2, "Common.Button.EnchantedRings".Localized(), EnchantedRings);

            AddButton(5, "Common.Button.JewelryBox".Localized(), () => ConfirmBuy(null, 500, KeyItemIds.EquipmentStorageJewelryBox));

            AddButton(14, "Common.Button.Leave".Localized(), ReturnToPrevious);
        }

        public void ConfirmBuy(Identifier itype = default, int priceOverride = -1, string keyItem = "")
        {
            ClearOutput();

            if (IsJewelryBox(keyItem))
            {
                if (Player.HasKeyItem(KeyItemIds.EquipmentStorageJewelryBox))
                {
                    OutputText("Scenes.JewelryShop.ConfirmBuy.BoxAlreadyOwned".Localized());

                    DoNext(() => Inside());

                    return ;
                }

                OutputText("Scenes.JewelryShop.ConfirmBuy.BoxShelfNotice".Localized());
            }
            else
                OutputText("Scenes.JewelryShop.ConfirmBuy.MerchantExcited".Localized(
                     itype.Path
                ));

            Buy(itype, priceOverride, keyItem);
        }

        public void NoBuyOption(Identifier itype = default, string keyItem = "")
        {
            ClearOutput();

            if (IsJewelryBox(keyItem))
                OutputText("Scenes.JewelryShop.NoBuyOption.DeclineFirm".Localized());
            else
                OutputText("Scenes.JewelryShop.NoBuyOption.DeclineScoff".Localized());

            DoNext(() => Inside());
        }

        public void Debit(Identifier itype = default, int priceOverride = -1, string keyItem = "")
        {

            if (IsJewelryBox(keyItem))
            {
                OutputText("Scenes.JewelryShop.Debit.PaysFiveHundred".Localized());

                OutputText("Scenes.JewelryShop.Debit.BoxReceived".Localized());
            }
            else
                OutputText("Scenes.JewelryShop.Debit.GemsSnatched".Localized());

            Buy(itype, priceOverride, keyItem);
        }

        public void NormalRings()
        {
            ClearOutput();

            OutputText("Scenes.JewelryShop.NormalRings.StandardRings".Localized());

            ClearButtons();

            AddNextButton(ItemRegistry.Create(ItemIds.SilverRing).Name, () => ConfirmBuy(ItemIds.SilverRing));

            AddNextButton(ItemRegistry.Create(ItemIds.GoldRing).Name, () => ConfirmBuy(ItemIds.GoldRing));

            AddNextButton(ItemRegistry.Create(ItemIds.PlatinumRing).Name, () => ConfirmBuy(ItemIds.PlatinumRing));

            AddNextButton(ItemRegistry.Create(ItemIds.DiamondRing).Name, () => ConfirmBuy(ItemIds.DiamondRing));

            AddButton(14, "Common.Button.Back".Localized(), Inside);
        }

        public void SpecialRings()
        {
            ClearOutput();

            OutputText("Scenes.JewelryShop.SpecialRings.HiddenRingShelf".Localized());

            OutputText("Scenes.JewelryShop.SpecialRings.CacklesDisplay".Localized());

            ClearButtons();

            AddNextButton(ItemRegistry.Create(ItemIds.LethiciteRing).Name, () => ConfirmBuy(ItemIds.LethiciteRing));

            AddNextButton(ItemRegistry.Create(ItemIds.PurityRing).Name, () => ConfirmBuy(ItemIds.PurityRing));

            AddButton(14, "Common.Button.Back".Localized(), Inside);
        }

        public void EnchantedRings()
        {
            ClearOutput();

            OutputText("Scenes.JewelryShop.EnchantedRings.TarnishedJewelry".Localized((noFur ? "face" : "snout")));

            OutputText("Scenes.JewelryShop.EnchantedRings.TierPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TierRings".Localized(), () => TieredRings(1));

            if (Player.Level >= 10 || View.Time.Days >= 100)
                AddButton(1, "Common.Button.TierRings2".Localized(), () => TieredRings(2));

            if (Player.Level >= 20 || View.Time.Days >= 200)
                AddButton(2, "Common.Button.TierRings3".Localized(), () => TieredRings(3));

            AddButton(14, "Common.Button.Back".Localized(), Inside);
        }

        public void TieredRings(int tier)
        {
            ClearButtons();

            var ids = tier switch
            {
                1 => ["lesser_crimstone_ring", "lesser_fertite_ring", "lesser_icestone_ring", "lesser_critical_ring", "lesser_regen_ring", "lesser_life_ring", "lesser_mystic_ring", "lesser_power_ring", "lesser_focus_ring"],

                2 => ["crimstone_ring", "fertite_ring", "icestone_ring", "critical_ring", "regen_ring", "life_ring", "mystic_ring", "power_ring", "focus_ring"],

                3 => ["greater_crimstone_ring", "greater_fertite_ring", "greater_icestone_ring", "greater_critical_ring", "greater_regen_ring", "greater_life_ring", "greater_mystic_ring", "greater_power_ring", "greater_focus_ring"],

                _ => Array.Empty<string>()
            };

            for (var i = 0; i < ids.Length; i++)
            {
                var item = ItemRegistry.Create(Identifier.Parse("coc:" + ids[i]));

                var id = Identifier.Parse("coc:" + ids[i]);

                AddNextButton(item.Name, () => ConfirmBuy(id));
            }

            AddButton(14, "Common.Button.Back".Localized(), EnchantedRings);
        }

        public void Buy(Identifier itype, int priceOverride, string keyItem)
        {
            var price = IsJewelryBox(keyItem)

                ? 500

                : priceOverride >= 0

                    ? priceOverride

                    : (int)Math.Max(1, Math.Round(ItemRegistry.Create(itype).Value));

            if (Player.Gems < price)
            {
                OutputText("Scenes.JewelryShop.ConfirmBuy.NotEnoughGems".Localized());

                Inside();

                return;
            }

            Player.Gems -= price;

            if (IsJewelryBox(keyItem))
                Player.CreateKeyItem(keyItem);
            else
                Player.Inventory.Add(ItemRegistry.Create(itype), 1);

            Inside();
        }

        public bool IsJewelryBox(string keyItem)
        {

            return keyItem == KeyItemIds.EquipmentStorageJewelryBox;
        }

    }
}