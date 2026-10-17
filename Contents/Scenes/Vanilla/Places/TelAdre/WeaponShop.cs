using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Shops;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
[Register<WeaponShop>("coc:weapon_shop")]
    public class WeaponShop : ShopScene
    {

        protected override Identifier ShopId => SceneIds.TelAdre.WeaponShop;

        public override void Entry()
        {
            Inside();
        }

        public void Inside()
        {
            ClearOutput();

            OutputText("Scenes.Weaponshop.Inside.ForgeAmbience".Localized((noFur ? "A short gray and white tail" : "Thick gray and white fur"), (noFur ? "tail" : "fur")));

            OutputText("Scenes.Weaponshop.Inside.GruffGreeting".Localized());

            OutputText("Scenes.Weaponshop.Inside.BuyPrompt".Localized());

            ClearButtons();

            RenderShop();

            if (Player.HasKeyItem("Sheila's Lethicite") || CoC.Instance.Flags.Get(GameFlagIds.SheilaLethiciteForgeDay) > 0)
                AddButton(13, "Common.Button.Scarblade".Localized(), ForgeScarredBlade);

            AddButton(14, "Common.Button.Leave".Localized(), ReturnToPrevious);
        }

        public void ConfirmBuy(Identifier itype = default, int priceOverride = -1, string keyItem = "")
        {
            ClearOutput();

            OutputText("Scenes.Weaponshop.ConfirmBuy.GruffConfirmation".Localized(
                 itype.Path
            ));

            Purchase(itype);
        }

        public void Purchase(Identifier itype)
        {
            var item = ItemRegistry.Create(itype);

            var price = (int)Math.Max(1, Math.Round(item.Value));

            if (Player.Gems < price)
            {
                OutputText("Scenes.JewelryShop.ConfirmBuy.NotEnoughGems".Localized());

                Inside();

                return;
            }

            Player.Gems -= price;

            Player.Inventory.Add(item, 1);
        }

        public void ForgeScarredBlade()
        {

            if (Player.HasKeyItem("Sheila's Lethicite"))
                ForgeScarredBladeStart();

            if (View.Time.Days - CoC.Instance.Flags.Get(GameFlagIds.SheilaLethiciteForgeDay) < 14)
                ForgeScarredBladeMiddle();
            else
                ForgeScarredBladeEnd();
        }

        public void ForgeScarredBladeStart()
        {
            ClearOutput();

            OutputText("Scenes.Weaponshop.ForgeScarredBladeStart.AppraisingEye".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeStart.GruffDemand".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeStart.CrystalProposal".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeStart.StoleItLie".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeStart.AgreesToForge".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.SheilaLethiciteForgeDay, View.Time.Days);

            Player.RemoveKeyItem("Sheila's Lethicite");

            DoNext(() => Inside());
        }

        public void ForgeScarredBladeMiddle()
        {
            ClearOutput();

            OutputText("Scenes.Weaponshop.ForgeScarredBladeMiddle.NotDoneYet".Localized());

            DoNext(() => Inside());
        }

        public void ForgeScarredBladeEnd()
        {
            ClearOutput();

            OutputText("Scenes.Weaponshop.ForgeScarredBladeEnd.TighterGrimace".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeEnd.FinishedBlade".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeEnd.DamnedestThing".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeEnd.ScratchesExamined".Localized());

            OutputText("Scenes.Weaponshop.ForgeScarredBladeEnd.RepeatsDamnedest".Localized());

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.ScarredBlade), FinishTakingScarredBlade, Inside);
        }

        public void FinishTakingScarredBlade()
        {

            CoC.Instance.Flags.Set(GameFlagIds.SheilaLethiciteForgeDay, -1);

            Inside();
        }

    }
}