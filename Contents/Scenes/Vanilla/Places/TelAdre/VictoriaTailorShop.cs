using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Shops;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<VictoriaTailorShop>("coc:victoria_tailor")]
    public class VictoriaTailorShop : ShopScene
    {
        public Action? backOption;

        protected override Identifier ShopId => SceneIds.TelAdre.VictoriaTailorShop;

        public override void Entry()
        {
            Inside();
        }

        public void Inside()
        {
            ClearOutput();

            OutputText("Scenes.VictoriaTailorShop.Inside.ClutteredShop".Localized());

            if (!Player.HasStatusEffect(StatusEffectIds.Victoria))
            {
                OutputText("Scenes.VictoriaTailorShop.Inside.TurnsAround".Localized());

                if (Player.Tallness > 60)
                    OutputText("Scenes.VictoriaTailorShop.Inside.LooksForSource".Localized());
                else
                    OutputText("Scenes.VictoriaTailorShop.Inside.FaceToFace".Localized());

                OutputText("Scenes.VictoriaTailorShop.Inside.ClearlyTailor".Localized());

                Player.CreateStatusEffect(StatusEffectIds.Victoria, 0, 0, 0, 0);
            }
            else
            {
                OutputText("Scenes.VictoriaTailorShop.Inside.TurnsToLook".Localized());

                if (Player.Tallness > 60)
                    OutputText("Scenes.VictoriaTailorShop.Inside.DownWord".Localized());

                OutputText("Scenes.VictoriaTailorShop.Inside.VictoriaNoted".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.FullClothes".Localized(), ClothesSection);

            AddButton(1, "Common.Button.Underwears".Localized(), UndergarmentSection);

            AddButton(14, "Common.Button.Leave".Localized(), ReturnToPrevious);
        }

        public void ConfirmBuy(Identifier itype = default, int priceOverride = -1, string keyItem = "")
        {
            ClearOutput();

            OutputText("Scenes.VictoriaTailorShop.ConfirmBuy.MeasuringTape".Localized());

            if (Player.BiggestCockArea() > 30 || Player.Cocks.Count > 1)
                OutputText("Scenes.VictoriaTailorShop.ConfirmBuy.FondlePackage".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.VictoriaTailorShop.ConfirmBuy.RubOuterLips".Localized());
                else
                    OutputText("Scenes.VictoriaTailorShop.ConfirmBuy.SlipFinger".Localized());

            OutputText("Scenes.VictoriaTailorShop.ConfirmBuy.CouldSwear".Localized());

            OutputText("Scenes.VictoriaTailorShop.ConfirmBuy.DisappearsBack".Localized(
                 ItemRegistry.Create(itype).LongName
            ));

            OutputText("Scenes.VictoriaTailorShop.ConfirmBuy.GemsAndYours".Localized(
                 itype.Path
            ));

            Purchase(itype, priceOverride);

            if (Player.HasCock() && Player.Lust >= 33)
                AddButton(4, "Common.Button.Flirt".Localized(), () => FlirtWithVictoria(itype));
        }

        public void ClothesSection()
        {
            ClearOutput();

            OutputText("Scenes.VictoriaTailorShop.ClothesSection.BuyClothesPrompt".Localized());

            backOption = ClothesSection;

            ClearButtons();

            RenderShop();

            AddButton(14, "Common.Button.Back".Localized(), Inside);
        }

        public void UndergarmentSection()
        {
            ClearOutput();

            OutputText("Scenes.VictoriaTailorShop.UndergarmentSection.BuyUndergarmentsPrompt".Localized());

            backOption = UndergarmentSection;

            ClearButtons();

            AddNextButton(ItemRegistry.Create(ItemIds.ComfortableBra).Name, () => ConfirmBuy(ItemIds.ComfortableBra));

            AddNextButton(ItemRegistry.Create(ItemIds.ComfyPanties).Name, () => ConfirmBuy(ItemIds.ComfyPanties));

            AddNextButton(ItemRegistry.Create(ItemIds.ComfyLoincloth).Name, () => ConfirmBuy(ItemIds.ComfyLoincloth));

            AddNextButton(ItemRegistry.Create(ItemIds.ChildPanties).Name, () => ConfirmBuy(ItemIds.ChildPanties));

            AddNextButton(ItemRegistry.Create(ItemIds.TrainingBra).Name, () => ConfirmBuy(ItemIds.TrainingBra));

            AddNextButton(ItemRegistry.Create(ItemIds.Fundosh).Name, () => ConfirmBuy(ItemIds.Fundosh));

            AddNextButton(ItemRegistry.Create(ItemIds.FurLoincloth).Name, () => ConfirmBuy(ItemIds.FurLoincloth));

            AddNextButton(ItemRegistry.Create(ItemIds.Garters).Name, () => ConfirmBuy(ItemIds.Garters));

            AddNextButton(ItemRegistry.Create(ItemIds.LatexBra).Name, () => ConfirmBuy(ItemIds.LatexBra));

            AddNextButton(ItemRegistry.Create(ItemIds.LatexShorts).Name, () => ConfirmBuy(ItemIds.LatexShorts));

            AddNextButton(ItemRegistry.Create(ItemIds.LatexThong).Name, () => ConfirmBuy(ItemIds.LatexThong));

            AddButton(14, "Common.Button.Back".Localized(), Inside);
        }

        public void NoBuyOption(Identifier itype = default, string keyItem = "")
        {

            backOption?.Invoke();
        }

        public void Purchase(Identifier itype, int priceOverride)
        {
            var item = ItemRegistry.Create(itype);

            var price = priceOverride >= 0 ? priceOverride : (int)Math.Max(1, Math.Round(item.Value));

            if (Player.Gems < price)
            {
                OutputText("Scenes.JewelryShop.ConfirmBuy.NotEnoughGems".Localized());

                Inside();

                return;
            }

            Player.Gems -= price;

            Player.Inventory.Add(item, 1);

            if (Player.HasCock() && Player.Lust >= 33)
                AddButton(4, "Common.Button.Flirt".Localized(), () => FlirtWithVictoria(itype));
        }

        public void FlirtWithVictoria(Identifier itype)
        {
            ClearOutput();

            int x = Player.CockThatFits(70);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.TakesClothes".Localized());

            if (x < 0)
            {
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.SmirksAnswers".Localized(
                     Player.Mf("studmuffin", "sweet thing")
                ));

                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.StillWantBuy".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), () => Debit(itype)); AddButton(1, "No", () => Inside());

                return ;
            }

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.MomentRealize".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.SqueezesButt".Localized());

            if (Player.Tallness >= 65)
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.YourStomach".Localized());
            else
                if (Player.Tallness >= 55)
                    OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.YourChest".Localized());
                else
                    OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.YourFace".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.PracticedEase".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.BustySmirk".Localized());

            if (Player.Cocks[x].Length >= 5)
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.TipPoking".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.SentencePeriod".Localized());

            if (Player.Cocks[x].Thickness > 3)
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.FaceScrunches".Localized());
            else
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.PressesArms".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.MovesSlowly".Localized());

            if (Player.Cocks[x].Length > 5)
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.QuickSuck".Localized(
                     Player.CockHead(x)
                ));
            else
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.PressesMouth".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.HvvngFnn".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.MarshmallowAssault".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.WrenchesFree".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.SquealsPleasure".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.MiraculouslyAble".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.SuddenResistance".Localized());

            if (silly)
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.GroansExtreme".Localized());
            else
                OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.GroansExtremeTwo".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.StrengthGivesOut".Localized());

            OutputText("Scenes.VictoriaTailorShop.FlirtWithVictoria.BodyFinally".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

    }
}