using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Seasonal
{
    [Register<AprilFoolsScene>("coc:april_fools")]
    public class AprilFoolsScene : Scene
    {

        public override void Entry()
        {
            PoniesFn();
        }

        public void PoniesFn()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.PoniesFn.LakePonies".Localized());

            ClearButtons();

            AddButton(0, "Combat.Button.Approach".Localized(), ApproachPonies);

            AddButton(14, "Common.Button.Leave".Localized(), LeavePonies);

            CoC.Instance.Flags.Set(GameFlagIds.SillyModePonies, CoC.Instance.Flags.Get(GameFlagIds.SillyModePonies) + 1);
        }

        public void LeavePonies()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.LeavePonies.DemonicTrick".Localized());

            ReturnToCamp(1);
        }

        public void ApproachPonies()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.ApproachPonies.SlowApproach".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.StepsOut".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.PurpleApologizes".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.NoProblem".Localized());

            if (!Player.Weapon.IsUnarmed())
                OutputText("Scenes.Aprilfools.ApproachPonies.ArmedStrangers".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.PeriodParagraph".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.BluePuzzled".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.ToBeFair".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.RambunctiousHorses".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.WhatShouldYouDo".Localized());

            OutputText("Scenes.Aprilfools.ApproachPonies.WhicheverYouChoose".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.TooCreepy".Localized(), DerpCreepy);

            AddButton(1, "Common.Button.YayParty".Localized(), DerpyParty);

            AddButton(14, "Common.Button.Leave".Localized(), DerpPolitely);
        }

        public void DerpPolitely()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.DerpPolitely.HoldsOutArms".Localized());

            ReturnToCamp(1);
        }

        public void DerpCreepy()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.DerpCreepy.CocksHornsVaginas".Localized());

            OutputText("Scenes.Aprilfools.DerpCreepy.XpGemsReward".Localized());

            Player.Experience += 10;

            Player.Gems += 5;

            ReturnToCamp(1);
        }

        public void DerpyParty()
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.DerpyParty.FlyingHorsesAmaze".Localized());

            OutputText("Scenes.Aprilfools.DerpyParty.DayPassed".Localized());

            OutputText("Scenes.Aprilfools.DerpyParty.LowerBodyShrinks".Localized());

            OutputText("Scenes.Aprilfools.DerpyParty.CheckChangelogNote".Localized());

            Player.Body.Get<LowerBodyPart>().Id = BodyPartTypes.Pony;

            Player.Body.Get<LowerBodyPart>().LegCount = 4;

            ReturnToCamp(1);
        }

        public void DLCPrompt(string dlcName, string dlcPitch, string dlcPrice, Action nextFunc = null)
        {
            ClearOutput();

            OutputText(dlcPitch);

            OutputText("Scenes.Aprilfools.DLCPrompt.PurchasePrompt".Localized(
                dlcName,

                dlcPrice
            ));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => BuyDLCPrompt(dlcName, dlcPrice, nextFunc));

            if (nextFunc != null)
                AddButton(1, "Common.Button.No".Localized(), nextFunc);
            else
                AddButton(1, "Common.Button.No".Localized(), () => Go<CampScene>());
        }

        public void BuyDLCPrompt(string dlcName, string dlcPrice, Action nextFunc = null)
        {
            ClearOutput();

            OutputText("Scenes.Aprilfools.BuyDLCPrompt.ItemLabel".Localized(
                dlcName
            ));

            OutputText("Scenes.Aprilfools.BuyDLCPrompt.PriceLabel".Localized(
                dlcPrice
            ));

            OutputText("Scenes.Aprilfools.BuyDLCPrompt.SelectMethod".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.CreditDebit".Localized(), () => ProceedToCheckout("Credit/Debit Card", nextFunc));

            AddButton(1, "Common.Button.Paypal".Localized(), () => ProceedToCheckout("Paypal", nextFunc));

            AddButton(2, "Common.Button.Interac".Localized(), () => ProceedToCheckout("Interac", nextFunc));

            AddButton(3, "Common.Button.Gems".Localized(), () => ProceedToCheckout("Gems", nextFunc));

            AddButton(14, "Scenes.Options.Cancel".Localized(), nextFunc);
        }

        public void ProceedToCheckout(string method, Action nextFunc = null)
        {
            ClearOutput();

            if (method != "Gems")
                OutputText("Scenes.Aprilfools.ProceedToCheckout.ExternalSite".Localized());
            else
                OutputText("Scenes.Aprilfools.ProceedToCheckout.PayWithGems".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), ReallyCheckout);

            AddButton(1, "Common.Button.No".Localized(), nextFunc);
        }

        public void ReallyCheckout()
        {
            ClearOutput();

            ShowImage("monster-troll");

            OutputText("Scenes.Aprilfools.ReallyCheckout.AprilFoolsJoke".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DlcAprilFools, 1);

            Go<CampScene>();
        }

        public void Pay2WinSelection()
        {
            ClearOutput();

            DisplayHeader("Premium Shop");

            OutputText("Scenes.Aprilfools.Pay2WinSelection.DontFeelLikeEarning".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.GemsHeader".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.GemsOneK".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.GemsTwoHalfK".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.GemsFiveK".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.GemsTenK".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.GemsTwentyFiveK".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.SpecialItemsHeader".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.LethiciteArmor".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.DivineBarkArmor".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.LethiciteStaff".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.LethicesWhip".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.FiveLethicite".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.FivePremiumMead".Localized());

            OutputText("Scenes.Aprilfools.Pay2WinSelection.FivePeppermintWhite".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.GemsAmount".Localized(1000.ToString("N0")), () => BuyDLCPrompt("Common.Button.GemsAmount".Localized(1000.ToString("N0")), "$0.99f", Pay2WinSelection));

            AddButton(1, "Common.Button.GemsAmount".Localized(2500.ToString("N0")), () => BuyDLCPrompt("Common.Button.GemsAmount".Localized(2500.ToString("N0")), "$1.99f", Pay2WinSelection));

            AddButton(2, "Common.Button.GemsAmount".Localized(5000.ToString("N0")), () => BuyDLCPrompt("Common.Button.GemsAmount".Localized(5000.ToString("N0")), "$2.99f", Pay2WinSelection));

            AddButton(3, "Common.Button.GemsAmount".Localized(10000.ToString("N0")), () => BuyDLCPrompt("Common.Button.GemsAmount".Localized(10000.ToString("N0")), "$4.99f", Pay2WinSelection));

            AddButton(4, "Common.Button.GemsAmount".Localized(25000.ToString("N0")), () => BuyDLCPrompt("Common.Button.GemsAmount".Localized(25000.ToString("N0")), "$7.99f", Pay2WinSelection));

            AddButton(5, "Common.Button.ChroniclersTears".Localized(), () => BuyDLCPrompt("Chronicler's Tears", "$2.99f", Pay2WinSelection));

            AddButton(6, ItemRegistry.Create(ArmorIds.DivineArmor).Name, () => BuyDLCPrompt(ItemRegistry.Create(ArmorIds.DivineArmor).LongName, "$0.99f", Pay2WinSelection));

            AddButton(7, ItemRegistry.Create(WeaponIds.LethiciteStaff).Name, () => BuyDLCPrompt(ItemRegistry.Create(WeaponIds.LethiciteStaff).LongName, "$0.99f", Pay2WinSelection));

            AddButton(8, ItemRegistry.Create(ItemIds.LethicesWhip).Name, () => BuyDLCPrompt(ItemRegistry.Create(ItemIds.LethicesWhip).LongName, "$0.99f", Pay2WinSelection));

            AddButton(9, ItemRegistry.Create(ItemIds.Lethicite).Name, () => BuyDLCPrompt("5x " + ItemRegistry.Create(ItemIds.Lethicite).LongName, "$0.99f", Pay2WinSelection));

            AddButton(10, ItemRegistry.Create(ConsumableIds.PremiumMead).Name, () => BuyDLCPrompt("5x " + ItemRegistry.Create(ConsumableIds.PremiumMead).LongName, "$1.99f", Pay2WinSelection));

            AddButton(11, ItemRegistry.Create(ItemIds.MintWhite).Name, () => BuyDLCPrompt("5x " + ItemRegistry.Create(ItemIds.MintWhite).LongName, "$2.99f", Pay2WinSelection));

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

    }
}