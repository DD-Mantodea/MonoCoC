using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Shops;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<YvonneArmorShop>("coc:yvonne_armor")]
    public class YvonneArmorShop : ShopScene
    {

        protected override Identifier ShopId => SceneIds.TelAdre.YvonneArmorShop;

        public override void Entry()
        {
            Inside();
        }

        public void Inside()
        {
            ClearOutput();

            OutputText("Scenes.YvonneArmorShop.Inside.InteriorHot".Localized((noFur ? "dog-eared" : "german-shepherd")));

            OutputText("Scenes.YvonneArmorShop.Inside.WhatToMake".Localized());

            ClearButtons();

            RenderShop();

            if (Player.HasKeyItem("Dragon Eggshell"))
            {
                OutputText("Scenes.YvonneArmorShop.Inside.DisplayPieces".Localized());

                if (Player.Gems < 200)
                    OutputText("Scenes.YvonneArmorShop.Inside.CantAfford".Localized());
                else
                    AddButton(12, "Common.Button.Eggshell".Localized(), null );
            }
            else
                AddButton(12, "Common.Button.Combine".Localized(), YvonneCombine);

            AddButton(13, "Common.Button.Flirt".Localized(), YvonneFlirt);

            AddButton(14, "Common.Button.Leave".Localized(), ReturnToPrevious);
        }

        public void ConfirmBuy(Identifier itype = default, int priceOverride = -1, string keyItem = "")
        {
            ClearOutput();

            // 原版 YvonneArmorShop.hx:50 Std.isOfType(itype, Armor)：买护甲与买盾牌台词不同
            if (ItemRegistry.Definitions.TryGet(itype, out var itypeDefinition)
                && itypeDefinition.Type == "coc:armor")
            {
                OutputText("Scenes.YvonneArmorShop.ConfirmBuy.SeriousLook".Localized());

                OutputText("Scenes.YvonneArmorShop.ConfirmBuy.SmilesBriefly".Localized(
                     itype.Path
                ));
            }
            else
            {
                OutputText("Scenes.YvonneArmorShop.ConfirmBuy.SeriousLookAgain".Localized());

                OutputText("Scenes.YvonneArmorShop.ConfirmBuy.WillServeWell".Localized(
                     itype.Path
                ));
            }

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

        public void YvonneFlirt()
        {
            ClearOutput();

            OutputText("Scenes.YvonneArmorShop.YvonneFlirt.StepsCloser".Localized());

            OutputText("Scenes.YvonneArmorShop.YvonneFlirt.StepsBack".Localized((noFur ? "on her cheeks" : "through her fur")));

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            if (Player.CockTotal() == 0)
            {
                OutputText("Scenes.YvonneArmorShop.YvonneFlirt.NotMyType".Localized());

                DoNext(() => Inside());

                return ;
            }
            else
                if (Player.Tallness > 65 && !(OptionStore.Instance.Get("lowStandards", 0) > 0))
                {
                    OutputText("Scenes.YvonneArmorShop.YvonneFlirt.NotMyTypeTwo".Localized());

                    DoNext(() => Inside());

                    return ;
                }
                else
                    if (Player.CockThatFits(75) == -1 && !(OptionStore.Instance.Get("lowStandards", 0) > 0))
                    {
                        OutputText("Scenes.YvonneArmorShop.YvonneFlirt.NotMyTypeThree".Localized());

                        DoNext(() => Inside());

                        return ;
                    }

            if (CoC.Instance.Flags.Get(GameFlagIds.YvonneFuckCounter) == 0)
                OutputText("Scenes.YvonneArmorShop.YvonneFlirt.QuickFuckOffer".Localized());
            else
                OutputText("Scenes.YvonneArmorShop.YvonneFlirt.GoAgain".Localized());

            OutputText("Scenes.YvonneArmorShop.YvonneFlirt.SayEndTag".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.FuckHer".Localized(), FuckYvonneInZeBlacksmith);

            AddButton(1, "Scenes.CorruptedGlade.NeverMind".Localized(), BackOutOfYvonneFuck);
        }

        public void BackOutOfYvonneFuck()
        {
            ClearOutput();

            OutputText("Scenes.YvonneArmorShop.BackOutOfYvonneFuck.PolitelyDeclines".Localized());

            DoNext(() => Inside());
        }

        public void FuckYvonneInZeBlacksmith()
        {
            ClearOutput();

            int x = Player.CockThatFits(75);

            if (x < 0)
                x = 0;

            var y = x + 1;

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.WalksToDoor".Localized());

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.HandGrabs".Localized((noFur ? "" : " fuzzy,"), (noFur ? "" : ", Yvonne's wet, canine nose bumping against your ear")));

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.FeminineScent".Localized());

            if (Player.CockArea(x) < 20)
                OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.RuntWord".Localized());
            else
                if (Player.CockArea(x) < 40)
                    OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.Boy".Localized(
                         Player.Mf("boy", "girl")
                    ));
                else
                    if (Player.CockArea(x) < 60)
                        OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.BigBoy".Localized());
                    else
                        OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.Stud".Localized(
                             Player.Mf("stud", "breeder")
                        ));

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.PeriodSayEnd".Localized());

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.ReleasesYou".Localized());

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.OnAllFours".Localized());

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.SidlesUp".Localized(
                 y,

                 y
            ));

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.HandholdChest".Localized(
                 y
            ));

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.ShiverDog".Localized((noFur ? "girl" : "morph"), (noFur ? "hair" : "fur")));

            if (Player.HasKnot(x))
                OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.KnotLocks".Localized());
            else
                OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.JizzFills".Localized());

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.CreamStuffed".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.PopWord".Localized());
            else
                OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.PullWord".Localized());

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.PullsOutSuffix".Localized());

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.StaggersUp".Localized((noFur ? "feet" : "footpaws")));

            OutputText("Scenes.YvonneArmorShop.FuckYvonneInZeBlacksmith.TossesGear".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.YvonneFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.YvonneFuckCounter) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void YvonneCombine()
        {
            ClearOutput();

            OutputText("Scenes.YvonneArmorShop.YvonneCombine.AsksCombine".Localized());

            OutputText("Scenes.YvonneArmorShop.YvonneCombine.SureCanDo".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Normal".Localized(), YvonneCombineNorm);

            AddButton(1, "Common.Button.Lewd".Localized(), YvonneCombineLewd);
        }

        public void YvonneCombineNorm()
        {

            OutputText("Scenes.YvonneArmorShop.YvonneCombineNorm.SnortsRecognition".Localized());

            OutputText("Scenes.YvonneArmorShop.YvonneCombineNorm.ExaminesTunic".Localized());

            OutputText("Scenes.YvonneArmorShop.YvonneCombineNorm.AdventurersChain".Localized());

            Player.DestroyItems(ArmorIds.Chainmail,1);

            Player.DestroyItems(ArmorIds.KokiriClothes,1);

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.KokiriChain), Inside);
        }

        public void YvonneCombineLewd()
        {

            OutputText("Scenes.YvonneArmorShop.YvonneCombineLewd.SmirksRecognition".Localized());

            OutputText("Scenes.YvonneArmorShop.YvonneCombineLewd.ExaminesChain".Localized());

            OutputText("Scenes.YvonneArmorShop.YvonneCombineLewd.LewdChain".Localized());

            Player.DestroyItems(ArmorIds.Chainmail,1);

            Player.DestroyItems(ArmorIds.KokiriClothes,1);

            InventoryScene.TakeItem(ItemRegistry.Create(ItemIds.KokiriLewdChain), Inside);
        }

    }
}