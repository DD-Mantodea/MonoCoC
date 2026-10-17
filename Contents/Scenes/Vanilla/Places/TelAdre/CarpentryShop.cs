using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Contents.Achievements;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<CarpentryShop>("coc:carpentry")]
public class CarpentryShop : Scene
    {

        public int nails = 0;

        public int wood = 0;

        public int stone = 0;

        public override void Entry()
        {
            Enter();
        }

        public void Enter()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.Enter.EntersShop".Localized((noFur ? "man with zebra-like stripes on his skin and equine ears and tail" : "zebra-morph stallion")));

            OutputText("Scenes.Carpentryshop.Enter.WelcomeGreeting".Localized());

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryZebras);

            if (Player.HasKeyItem("Carpenter's Toolbox"))
                CampScene.CheckCabinMaterials(0);

            ClearButtons();

            if (Player.HasKeyItem("Carpenter's Toolbox"))
                AddButton(0, "Common.Button.BuyNails".Localized(), CarpentryShopBuyNails);
            else
                AddButton(0, "Common.Button.BuyNails".Localized(), null);

            AddButton(1, "Common.Button.BuyWood".Localized(), CarpentryShopBuyWood);

            AddButton(2, "Common.Button.BuyStones".Localized(), CarpentryShopBuyStone);

            if (Player.KeyItemV1("Carpenter's Toolbox") > CampScene.MaxNailSupply())
                AddButton(5, "Common.Button.SellNails".Localized(), CarpentryShopSellNails);
            else
                AddButton(5, "Common.Button.SellNails".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) > CampScene.MaxWoodSupply())
                AddButton(6, "Common.Button.SellWood".Localized(), CarpentryShopSellWood);
            else
                AddButton(6, "Common.Button.SellWood".Localized(), null);

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) > CampScene.MaxStoneSupply())
                AddButton(7, "Common.Button.SellStones".Localized(), CarpentryShopSellStone);
            else
                AddButton(7, "Common.Button.SellStones".Localized(), null);

            if (!Player.HasKeyItem("Carpenter's Toolbox"))
            {
                AddButton(10, "Common.Button.Toolbox".Localized(), CarpentryShopBuySet);

                AddButton(11, "Common.Button.NailBox".Localized(), null);
            }
            else
            {
                AddButton(10, "Common.Button.Toolbox".Localized(), null);

                if (!Player.HasKeyItem("Carpenter's Nail Box"))
                    AddButton(11, "Common.Button.NailBox".Localized(), CarpentryShopBuyNailbox);
                else
                    AddButton(11, "Common.Button.NailBox".Localized(), null);
            }

            AddButton(14, "Common.Button.Leave".Localized(), ReturnToPrevious);
        }

        public void CarpentryShopBuyNails()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNails.AsksNails".Localized(
                 0
            ));

            CampScene.CheckCabinMaterials(1);

            ClearButtons();

            AddButton(0, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyNailsAmount(10));

            AddButton(1, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyNailsAmount(25));

            AddButton(2, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyNailsAmount(50));

            AddButton(3, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyNailsAmount(75));

            AddButton(4, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyNailsAmount(100));

            AddButton(14, "Common.Button.Back".Localized(), Enter);
        }

        public void CarpentryShopBuyNailsAmount(int amount)
        {
            ClearOutput();

            nails = amount;

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailsAmount.NailAmount".Localized(
                 amount,

                 amount * 2
            ));

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailsAmount.ConfirmNailBuy".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopBuyNailsYes()); AddButton(1, "No", () => CarpentryShopBuyNails());
        }

        public void CarpentryShopBuyNailsYes()
        {
            ClearOutput();

            if (Player.Gems >= (nails * 2))
            {
                Player.Gems -= nails * 2;

                CoC.Instance.Flags.Set(GameFlagIds.AchievementProgressHammerTime, CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressHammerTime) + nails);

                if (CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressHammerTime) >= 300)
                    AwardAchievement(AchievementIds.HammerTime);

                Player.AddKeyValue("Carpenter's Toolbox", 1, nails);

                OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailsYes.PaysNails".Localized(
                     nails * 2,

                     nails
                ));

                if (Player.KeyItemV1("Carpenter's Toolbox") > CampScene.MaxNailSupply())
                {
                    OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailsYes.ToolboxFull".Localized());

                    Player.Gems += (int)((Player.KeyItemV1("Carpenter's Toolbox") - CampScene.MaxNailSupply()) * 2);

                    Player.AddKeyValue("Carpenter's Toolbox", 1, -(Player.KeyItemV1("Carpenter's Toolbox") - CampScene.MaxNailSupply()));
                }

                CampScene.CheckCabinMaterials(1);
            }
            else
                OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailsYes.NailDecline".Localized());

            DoNext(() => CarpentryShopBuyNails());
        }

        public void CarpentryShopBuyWood()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyWood.AsksWood".Localized());

            CampScene.CheckCabinMaterials(2);

            ClearButtons();

            AddButton(0, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyWoodAmount(10));

            AddButton(1, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyWoodAmount(20));

            AddButton(2, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyWoodAmount(30));

            AddButton(3, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyWoodAmount(40));

            AddButton(4, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyWoodAmount(50));

            AddButton(14, "Common.Button.Back".Localized(), Enter);
        }

        public void CarpentryShopBuyWoodAmount(int amount)
        {
            ClearOutput();

            wood = amount;

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyWoodAmount.WoodAmount".Localized(
                 amount,

                 amount * 10
            ));

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyWoodAmount.ConfirmWoodBuy".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopBuyWoodYes()); AddButton(1, "No", () => CarpentryShopBuyWood());
        }

        public void CarpentryShopBuyWoodYes()
        {
            ClearOutput();

            if (Player.Gems >= (wood * 10))
            {
                Player.Gems -= wood * 10;

                CoC.Instance.Flags.Set(GameFlagIds.AchievementProgressImNoLumberjack, CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressImNoLumberjack) + wood);

                if (CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressImNoLumberjack) >= 100)
                    AwardAchievement(AchievementIds.ImNoLumberjack);

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinWoodResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) + wood);

                OutputText("Scenes.Carpentryshop.CarpentryShopBuyWoodYes.PaysWood".Localized(
                     wood * 10
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) > CampScene.MaxWoodSupply())
                {
                    OutputText("Scenes.Carpentryshop.CarpentryShopBuyWoodYes.WoodSupplyFull".Localized());

                    Player.Gems += ((CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) - CampScene.MaxWoodSupply()) * 10);

                    CoC.Instance.Flags.Set(GameFlagIds.CampCabinWoodResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) - (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) - CampScene.MaxWoodSupply()));
                }

                CampScene.CheckCabinMaterials(2);
            }
            else
                OutputText("Scenes.Carpentryshop.CarpentryShopBuyWoodYes.WoodDecline".Localized());

            DoNext(() => CarpentryShopBuyWood());
        }

        public void CarpentryShopBuyStone()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyStone.AsksStones".Localized());

            CampScene.CheckCabinMaterials(3);

            ClearButtons();

            AddButton(0, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyStoneAmount(10));

            AddButton(1, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyStoneAmount(20));

            AddButton(2, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyStoneAmount(30));

            AddButton(3, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyStoneAmount(40));

            AddButton(4, "Common.Button.Buy2".Localized(), () => CarpentryShopBuyStoneAmount(50));

            AddButton(14, "Common.Button.Back".Localized(), Enter);
        }

        public void CarpentryShopBuyStoneAmount(int amount)
        {
            ClearOutput();

            stone = amount;

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyStoneAmount.StoneAmount".Localized(
                 amount,

                 amount * 20
            ));

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyStoneAmount.ConfirmStoneBuy".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopBuyStoneYes()); AddButton(1, "No", () => CarpentryShopBuyStone());
        }

        public void CarpentryShopBuyStoneYes()
        {

            if (Player.Gems >= (stone * 20))
            {
                Player.Gems -= stone * 20;

                CoC.Instance.Flags.Set(GameFlagIds.AchievementProgressYabbaDabbaDoo, CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressYabbaDabbaDoo) + stone);

                if (CoC.Instance.Flags.Get(GameFlagIds.AchievementProgressYabbaDabbaDoo) >= 100)
                    AwardAchievement(AchievementIds.YabbaDabbaDoo);

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinStoneResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) + stone);

                OutputText("Scenes.Carpentryshop.CarpentryShopBuyStoneYes.PaysStones".Localized(
                     stone * 20
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) > CampScene.MaxStoneSupply())
                {
                    OutputText("Scenes.Carpentryshop.CarpentryShopBuyStoneYes.StoneSupplyFull".Localized());

                    Player.Gems += ((CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) - CampScene.MaxStoneSupply()) * 20);

                    CoC.Instance.Flags.Set(GameFlagIds.CampCabinStoneResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) - (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) - CampScene.MaxStoneSupply()));
                }

                CampScene.CheckCabinMaterials(3);
            }
            else
                OutputText("Scenes.Carpentryshop.CarpentryShopBuyStoneYes.StoneDecline".Localized());

            DoNext(() => CarpentryShopBuyStone());
        }

        public void CarpentryShopSellNails()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopSellNails.OffersNails".Localized());

            CampScene.CheckCabinMaterials(1);

            ClearButtons();

            if (Player.KeyItemV1("Carpenter's Toolbox") >= 10)
                AddButton(0, "Common.Button.Sell".Localized(), () => CarpentryShopSellNailsAmount(10));

            if (Player.KeyItemV1("Carpenter's Toolbox") >= 25)
                AddButton(1, "Common.Button.Sell".Localized(), () => CarpentryShopSellNailsAmount(25));

            if (Player.KeyItemV1("Carpenter's Toolbox") >= 50)
                AddButton(2, "Common.Button.Sell".Localized(), () => CarpentryShopSellNailsAmount(50));

            if (Player.KeyItemV1("Carpenter's Toolbox") >= 75)
                AddButton(3, "Common.Button.Sell".Localized(), () => CarpentryShopSellNailsAmount(75));

            if (Player.KeyItemV1("Carpenter's Toolbox") >= 100)
                AddButton(3, "Common.Button.Sell".Localized(), () => CarpentryShopSellNailsAmount(100));

            if (Player.KeyItemV1("Carpenter's Toolbox") > CampScene.MaxNailSupply())
                AddButton(4, "Common.Button.SellAll".Localized(), () => CarpentryShopSellNailsAmount((int)(Player.KeyItemV1("Carpenter's Toolbox"))));

            AddButton(14, "Common.Button.Back".Localized(), Enter);
        }

        public void CarpentryShopSellNailsAmount(int amount)
        {
            ClearOutput();

            nails = amount;

            OutputText("Scenes.Carpentryshop.CarpentryShopSellNailsAmount.NailOffer".Localized(NumberUtils.Num2Text(amount), (Player.KeyItemV1("Carpenter's Toolbox") == 1 ? "piece" : "pieces"), amount));

            OutputText("Scenes.Carpentryshop.CarpentryShopSellNailsAmount.ConfirmNailSell".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopSellNailsYes()); AddButton(1, "No", () => CarpentryShopSellNails());
        }

        public void CarpentryShopSellNailsYes()
        {
            ClearOutput();

            if (Player.KeyItemV1("Carpenter's Toolbox") >= nails)
            {
                Player.Gems += nails;

                Player.AddKeyValue("Carpenter's Toolbox", 1, -nails);

                OutputText("Scenes.Carpentryshop.CarpentryShopSellNailsYes.SellsNails".Localized(NumberUtils.Num2Text(nails), (Player.KeyItemV1("Carpenter's Toolbox") == 1 ? "piece" : "pieces"), nails));

                CampScene.CheckCabinMaterials(1);
            }
            else
                OutputText("Scenes.Carpentryshop.CarpentryShopSellNailsYes.NailSellDecline".Localized());

            DoNext(() => CarpentryShopSellNails());
        }

        public void CarpentryShopSellWood()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopSellWood.OffersWood".Localized());

            CampScene.CheckCabinMaterials(2);

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) >= 1)
                AddButton(0, "Common.Button.Sell".Localized(), () => CarpentryShopSellWoodAmount(1));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) >= 5)
                AddButton(1, "Common.Button.Sell".Localized(), () => CarpentryShopSellWoodAmount(5));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) >= 10)
                AddButton(2, "Common.Button.Sell".Localized(), () => CarpentryShopSellWoodAmount(10));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) >= 25)
                AddButton(3, "Common.Button.Sell".Localized(), () => CarpentryShopSellWoodAmount(25));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) > CampScene.MaxWoodSupply())
                AddButton(4, "Common.Button.SellAll".Localized(), () => CarpentryShopSellWoodAmount(CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources)));

            AddButton(14, "Common.Button.Back".Localized(), Enter);
        }

        public void CarpentryShopSellWoodAmount(int amount)
        {
            ClearOutput();

            wood = amount;

            OutputText("Scenes.Carpentryshop.CarpentryShopSellWoodAmount.WoodOffer".Localized(NumberUtils.Num2Text(amount), (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) == 1 ? "piece" : "pieces"), (amount * 5)));

            OutputText("Scenes.Carpentryshop.CarpentryShopSellWoodAmount.ConfirmWoodSell".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopSellWoodYes()); AddButton(1, "No", () => CarpentryShopSellWood());
        }

        public void CarpentryShopSellWoodYes()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) >= wood)
            {
                Player.Gems += wood * 5;

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinWoodResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) - wood);

                OutputText("Scenes.Carpentryshop.CarpentryShopSellWoodYes.SellsWood".Localized(NumberUtils.Num2Text(wood), (CoC.Instance.Flags.Get(GameFlagIds.CampCabinWoodResources) == 1 ? "piece" : "pieces"), (wood * 5)));

                CampScene.CheckCabinMaterials(2);
            }
            else
                OutputText("Scenes.Carpentryshop.CarpentryShopSellWoodYes.WoodSellDecline".Localized());

            DoNext(() => CarpentryShopSellWood());
        }

        public void CarpentryShopSellStone()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopSellStone.OffersStones".Localized());

            CampScene.CheckCabinMaterials(3);

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) >= 1)
                AddButton(0, "Common.Button.Sell".Localized(), () => CarpentryShopSellStoneAmount(1));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) >= 5)
                AddButton(1, "Common.Button.Sell".Localized(), () => CarpentryShopSellStoneAmount(5));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) >= 10)
                AddButton(2, "Common.Button.Sell".Localized(), () => CarpentryShopSellStoneAmount(10));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) >= 25)
                AddButton(3, "Common.Button.Sell".Localized(), () => CarpentryShopSellStoneAmount(25));

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) > CampScene.MaxStoneSupply())
                AddButton(4, "Common.Button.SellAll".Localized(), () => CarpentryShopSellStoneAmount(CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources)));

            AddButton(14, "Common.Button.Back".Localized(), Enter);
        }

        public void CarpentryShopSellStoneAmount(int amount)
        {
            ClearOutput();

            stone = amount;

            OutputText("Scenes.Carpentryshop.CarpentryShopSellStoneAmount.StoneOffer".Localized(NumberUtils.Num2Text(amount), (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) == 1 ? "piece" : "pieces"), (amount * 10)));

            OutputText("Scenes.Carpentryshop.CarpentryShopSellStoneAmount.ConfirmStoneSell".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopSellStoneYes()); AddButton(1, "No", () => CarpentryShopSellStone());
        }

        public void CarpentryShopSellStoneYes()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) >= stone)
            {
                Player.Gems += stone * 10;

                CoC.Instance.Flags.Set(GameFlagIds.CampCabinStoneResources, CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) - stone);

                OutputText("Scenes.Carpentryshop.CarpentryShopSellStoneYes.SellsStones".Localized(NumberUtils.Num2Text(stone), (CoC.Instance.Flags.Get(GameFlagIds.CampCabinStoneResources) == 1 ? "piece" : "pieces"), (stone * 10)));

                CampScene.CheckCabinMaterials(3);
            }
            else
                OutputText("Scenes.Carpentryshop.CarpentryShopSellStoneYes.StoneSellDecline".Localized());

            DoNext(() => CarpentryShopSellStone());
        }

        public void CarpentryShopBuySet()
        {
            ClearOutput();

            if (Player.HasKeyItem("Carpenter's Toolbox"))
            {
                OutputText("Scenes.Carpentryshop.CarpentryShopBuySet.AlreadyOwnSet".Localized());

                DoNext(() => Enter());

                return ;
            }

            OutputText("Scenes.Carpentryshop.CarpentryShopBuySet.WalksAround".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CampCabinProgress) >= 4)
                OutputText("Scenes.Carpentryshop.CarpentryShopBuySet.JustWhatNeed".Localized());
            else
                OutputText("Scenes.Carpentryshop.CarpentryShopBuySet.HandyForBuilding".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuySet.TwoHundredGems".Localized());

            if (Player.Gems >= 200)
            {
                OutputText("Scenes.Carpentryshop.CarpentryShopBuySet.ConfirmSetBuy".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopBuySetYes()); AddButton(1, "No", () => CarpentryShopBuySetNo());
            }
            else
            {
                OutputText("Scenes.Carpentryshop.CarpentryShopBuySet.CountsGems".Localized());

                DoNext(() => Enter());
            }
        }

        public void CarpentryShopBuySetYes()
        {

            Player.Gems -= 200;

            OutputText("Scenes.Carpentryshop.CarpentryShopBuySetYes.PaysSet".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuySetYes.FeelsProud".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuySetYes.GainsSet".Localized());

            Player.CreateKeyItem("Carpenter's Toolbox", 0, 0, 0, 0);

            DoNext(() => Enter());
        }

        public void CarpentryShopBuySetNo()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopBuySetNo.NoThanks".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuySetNo.SuitYourself".Localized());

            DoNext(() => Enter());
        }

        public void CarpentryShopBuyNailbox()
        {

            if (Player.HasKeyItem("Carpenter's Nail Box"))
            {
                OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailbox.AlreadyOwnBox".Localized());

                DoNext(() => Enter());

                return ;
            }

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailbox.WalksBack".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailbox.BoxPrice".Localized());

            if (Player.Gems >= 200)
            {
                OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailbox.ConfirmBoxBuy".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => CarpentryShopBuyNailboxYes()); AddButton(1, "No", () => CarpentryShopBuyNailboxNo());
            }
            else
            {
                OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailbox.CountsBoxGems".Localized());

                DoNext(() => Enter());
            }
        }

        public void CarpentryShopBuyNailboxYes()
        {

            Player.Gems -= 200;

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailboxYes.PaysBox".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailboxYes.HoldsMoreNails".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailboxYes.GainsBox".Localized());

            Player.CreateKeyItem("Carpenter's Nail Box", 400, 0, 0, 0);

            DoNext(() => Enter());
        }

        public void CarpentryShopBuyNailboxNo()
        {
            ClearOutput();

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailboxNo.NoThanks".Localized());

            OutputText("Scenes.Carpentryshop.CarpentryShopBuyNailboxNo.SuitYourself".Localized());

            DoNext(() => Enter());
        }

    }
}