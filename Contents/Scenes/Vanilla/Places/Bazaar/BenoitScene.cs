using MonoCoC.Contents.World;
using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar
{
    [Register<BenoitScene>("coc:benoit")]
    public class BenoitScene : Scene
    {

        public override void Entry()
        {
            BenoitSprite();
        }

        public void BenoitSprite()
        {

        }

        public string BenoitMF(string stringM, string stringF = "")
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 1 || CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 2 || CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 3)
                return stringF;

            return stringM;
        }

        public bool BenoitLover()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) >= 2;
        }

        public int BenoitAffection(int changes = 0)
        {

            if (changes != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BenoitAffection, CoC.Instance.Flags.Get(GameFlagIds.BenoitAffection) + changes);

                if (CoC.Instance.Flags.Get(GameFlagIds.BenoitAffection) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.BenoitAffection, 100);
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.BenoitAffection) < 0)
                        CoC.Instance.Flags.Set(GameFlagIds.BenoitAffection, 0);
            }

            return CoC.Instance.Flags.Get(GameFlagIds.BenoitAffection);
        }

        public void BenoitKnocksUpPCCheck()
        {

            if (Player.Pregnancy.Type == PregnancyIds.Basilisk && Player.HasPerk(PerkIds.BasiliskWomb))
                Player.KnockUp(PregnancyIds.Benoit, Player.Pregnancy.Incubation, 100, 1);

            if ((Player.Pregnancy.Type == PregnancyIds.OvielixirEggs || Player.HasPerk(PerkIds.HarpyWomb) || Player.HasPerk(PerkIds.Oviposition) || Player.HasPerk(PerkIds.BasiliskWomb)) && (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Type == PregnancyIds.OvielixirEggs))
                if (Player.HasPerk(PerkIds.BasiliskWomb))
                {
                    if (Player.Pregnancy.Type != PregnancyIds.OvielixirEggs || Player.Pregnancy.Incubation == 0)
                        Player.KnockUp(PregnancyIds.Benoit, PregnancyIds.BasiliskIncubation);

                    if (Player.Pregnancy.Incubation > 0)
                        Player.KnockUp(PregnancyIds.Benoit, Player.Pregnancy.Incubation, 100, 1);
                }
                else
                    Player.KnockUp(PregnancyIds.Basilisk, PregnancyIds.BasiliskIncubation);
        }

        public bool BenoitPreggers()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) > 0)
                return true;

            return false;
        }

        public bool BenoitRegularPreggers()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) >= 1 && CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) <= 4)
                return true;

            return false;
        }

        public bool BenoitHeavyPreggers()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) <= 8)
                return true;

            return false;
        }

        public bool BenoitVeryHeavyPreggers()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) >= 9 && CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) <= 12)
                return true;

            return false;
        }

        public bool BenoitExtremePreggers()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) >= 13)
                return true;

            return false;
        }

        public bool BenoitInClutch()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                return false;

            int startDay = CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEvent);

            var currDay = View.Time.Days;

            var diffDays = (currDay - startDay) % 28;

            if (diffDays >= 21)
                return true;

            return false;
        }

        public bool BenoitKnockUp()
        {

            if (BenoitPreggers())
                return false;

            if (!BenoitInClutch())
                return false;

            var eggMod = 0;

            if (Player.HasPerk(PerkIds.ElvenBounty))
                eggMod += 1;

            if (Player.HasPerk(PerkIds.MaraesGiftStud))
                eggMod += 2;

            if (Player.HasPerk(PerkIds.FerasBoonAlpha))
                eggMod += 1;

            var numEggs = (int)(Player.CumQ() / 200);

            var minEggs = 1 + eggMod;

            if (numEggs > 12)
                numEggs = 12;

            if (numEggs < minEggs)
                numEggs = minEggs;

            numEggs += eggMod;

            CoC.Instance.Flags.Set(GameFlagIds.FemoitEggs, numEggs);

            CoC.Instance.Flags.Set(GameFlagIds.FemoitIncubation, 168);

            return true;
        }

        public void ClearBenoitPreggers()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.FemoitEggsLaid, CoC.Instance.Flags.Get(GameFlagIds.FemoitEggsLaid) + CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs));

                CoC.Instance.Flags.Set(GameFlagIds.FemoitEggs, 0);

                CoC.Instance.Flags.Set(GameFlagIds.FemoitIncubation, 0);

                CoC.Instance.Flags.Set(GameFlagIds.FemoitNextdayEvent, View.Time.Days);
            }
        }

        public int BenoitOffspring()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) + CoC.Instance.Flags.Get(GameFlagIds.FemoitEggsLaid);
        }

        public bool BenoitBigFamily()
        {

            if (!Player.HasPerk(PerkIds.BasiliskWomb))
                return false;

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) < 8)
                return false;

            return BenoitOffspring() >= 15;
        }

        public void SetBenoitShop(bool setButtonOnly = false)
        {

            if (View.Time.Hours >= 9 && View.Time.Hours <= 17)
            {
                if ((CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEventDone) == 1 && View.Time.Days >= CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEvent)) || CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEventDone) != 1)
                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBenoits) == 0)
                    {
                        if (!setButtonOnly)
                            OutputText("Scenes.Benoit.SetBenoitShop.FirstVisitNoticeStall".Localized());
                        else
                            AddButton(0, "Common.Button.MarketStall".Localized(), BenoitIntro);
                    }
                    else
                        if (!setButtonOnly)
                            OutputText("Scenes.Benoit.SetBenoitShop.StallLooksOpen".Localized(
                                 BenoitMF("Benoit", "Benoite")
                            ));
                        else
                            AddButton(0, BenoitMF("Benoit", "Benoite") + "Common.Text.SStall".Localized(), BenoitIntro);
            }
            else
                AddButton(0, CoC.Instance.Flags.Get(GameFlagIds.TimesInBenoits) == 0 ? "Market Stall" : BenoitMF("Benoit", "Benoite") + "Common.Text.SStall".Localized(), null, "The shop is currently closed. Come back later.\n(Hours of operation: 9am to 5pm)");
        }

        public void BenoitIntro()
        {
            ClearOutput();

            BenoitSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesInBenoits) == 0)
            {
                OutputText("Scenes.Benoit.BenoitIntro.CautiouslyApproachStallCanopy".Localized());

                OutputText("Scenes.Benoit.BenoitIntro.RaspyVoiceGloom".Localized());

                OutputText("Scenes.Benoit.BenoitIntro.GoAheadStareBasilisk".Localized());

                OutputText("Scenes.Benoit.BenoitIntro.WonderBlindAnythingBazaar".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("him", "her")
                ));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BenoitSuggestUnlocked) == 0 && Player.HasVagina() && (Player.InHeat || Player.Pregnancy.Type == PregnancyIds.OvielixirEggs || Player.HasPerk(PerkIds.HarpyWomb) || Player.HasPerk(PerkIds.Oviposition)) && (Player.Pregnancy.Type == PregnancyIds.OvielixirEggs || Player.Pregnancy.Incubation == 0) && (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0 || CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 3))
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.BenoitSuggestUnlocked) == 0)
                        BenoitAndFemPCTalkAboutEggings();
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEventDone) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEvent) <= View.Time.Days && CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                        FemoitNextDayEvent();
                    else
                        if (BenoitInClutch() && CoC.Instance.Flags.Get(GameFlagIds.FemoitReadyForEggs) == 0 && (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 1 || CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 2))
                        {
                            CoC.Instance.Flags.Set(GameFlagIds.FemoitReadyForEggs, CoC.Instance.Flags.Get(GameFlagIds.FemoitReadyForEggs) + 1);

                            OutputText("Scenes.Benoit.BenoitIntro.EnterStallLooksBlind".Localized());

                            OutputText("Scenes.Benoit.BenoitIntro.StandsNervouslyStomachBulging".Localized());

                            OutputText("Scenes.Benoit.BenoitIntro.AskReadyStiffensSpine".Localized());
                        }
                        else
                            if (BenoitPreggers() && CoC.Instance.Flags.Get(GameFlagIds.FemoitIncubation) < 0)
                            {
                                if (CoC.Instance.Flags.Get(GameFlagIds.FemoitIncubation) < -168)
                                {
                                    OutputText("Scenes.Benoit.BenoitIntro.EnterStomachFlatAgain".Localized(
                                         CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs)
                                    ));

                                    OutputText("Scenes.Benoit.BenoitIntro.TakesMomentComposeGiddy".Localized());

                                    OutputText("Scenes.Benoit.BenoitIntro.WorldContinuesShopRun".Localized());

                                    OutputText("Scenes.Benoit.BenoitIntro.StartsHintExcitedEnergy".Localized());

                                    ClearBenoitPreggers();

                                    CoC.Instance.Flags.Set(GameFlagIds.FemoitNextdayEvent, CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEvent) - 7);
                                }
                                else
                                {
                                    FemoitBirths();

                                    return ;
                                }
                            }
                            else
                                if (!BenoitInClutch() && !BenoitPreggers() && (View.Time.Days - CoC.Instance.Flags.Get(GameFlagIds.FemoitNextdayEvent) >= 30) && (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) > 0))
                                {
                                    if (CoC.Instance.Flags.Get(GameFlagIds.FemoitFirstClutchMissed) == 0)
                                    {
                                        CoC.Instance.Flags.Set(GameFlagIds.FemoitFirstClutchMissed, CoC.Instance.Flags.Get(GameFlagIds.FemoitFirstClutchMissed) + 1);

                                        OutputText("Scenes.Benoit.BenoitIntro.EnterStallSmellCooking".Localized());

                                        OutputText("Scenes.Benoit.BenoitIntro.DontZatAlmostBrained".Localized());

                                        OutputText("Scenes.Benoit.BenoitIntro.AskWhatSheMaking".Localized());

                                        OutputText("Scenes.Benoit.BenoitIntro.LunchOmelette".Localized());

                                        OutputText("Scenes.Benoit.BenoitIntro.EyesDrawnFlatStomach".Localized());

                                        if (Random.Shared.Next(4) == 0)
                                        {
                                            OutputText("Scenes.Benoit.BenoitIntro.ShrugsNeverWasteNot".Localized());

                                            OutputText("Scenes.Benoit.BenoitIntro.TurnOfferSomethingElse".Localized());
                                        }
                                        else
                                        {
                                            OutputText("Scenes.Benoit.BenoitIntro.ChokesOutMonDieu".Localized());

                                            OutputText("Scenes.Benoit.BenoitIntro.PatsMidriffPiecesTogether".Localized());
                                        }
                                    }
                                    else
                                    {
                                        OutputText("Scenes.Benoit.BenoitIntro.FlatBelliedTuckingOmelette".Localized());

                                        OutputText("Scenes.Benoit.BenoitIntro.AzzistYouWiz".Localized());
                                    }
                                }
                                else
                                    if (BenoitAffection() <= 10)
                                        OutputText("Scenes.Benoit.BenoitIntro.OnceAgainEnterGloom".Localized(
                                             BenoitMF("he", "she")
                                        ));
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) == 1 && CoC.Instance.Flags.Get(GameFlagIds.BenoitPostFirstfuckTalk) == 0)
                                            FirstTimeAfterBoningEncounterBenoit();
                                        else
                                            if (BenoitAffection() < 35)
                                                OutputText("Scenes.Benoit.BenoitIntro.EnterGloomAgain".Localized(
                                                     BenoitMF("his", "her")
                                                ));
                                            else
                                            {
                                                OutputText("Scenes.Benoit.BenoitIntro.EnterGloomThird".Localized(
                                                     BenoitMF("Benoit", "Benoite")
                                                ));

                                                if (BenoitInClutch() && !BenoitPreggers())
                                                    OutputText("Scenes.Benoit.BenoitIntro.ObviousBulgeApronClad".Localized());
                                                else
                                                    if (BenoitRegularPreggers())
                                                        OutputText("Scenes.Benoit.BenoitIntro.BellyBulgesBigPregnant".Localized());
                                                    else
                                                        if (BenoitHeavyPreggers())
                                                            OutputText("Scenes.Benoit.BenoitIntro.PregnancyUnmistakableEggs".Localized());
                                                        else
                                                            if (BenoitVeryHeavyPreggers())
                                                                OutputText("Scenes.Benoit.BenoitIntro.BellyHugelySwollenFertilized".Localized());
                                                            else
                                                                if (BenoitExtremePreggers())
                                                                    OutputText("Scenes.Benoit.BenoitIntro.HardlyBelieveHowPregnant".Localized());
                                            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesInBenoits, CoC.Instance.Flags.Get(GameFlagIds.TimesInBenoits) + 1);

            Player.UnlockCodexEntry(GameFlagIds.CodexEntryBasilisks);

            ClearButtons();

            AddButton(0, "Common.Button.Buy".Localized(), BenoitsBuyMenu);

            AddButton(1, "Common.Button.Sell2".Localized(), BenoitSellMenu);

            AddButton(2, "Common.Button.Talk".Localized(), TalkToBenoit);

            if (Player.HasCock() && CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) > 0 && Player.Lust >= 33)
                AddButton(3, "Common.Button.Sex".Localized(), (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFemoit) == 0 ? FemoitFirstTimeYes : FemoitSexIntro));

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<BazaarScene>());

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitUnlocked) == 1 && CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0)
                AddRowButton(1, "Feminize", BenoitFeminise);

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) > 0 && CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) < 3)
                AddRowButton(1, "Herminize", BenoitHerminise);

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitWombTalkUnlocked) == 1 && !Player.HasPerk(PerkIds.BasiliskWomb) && CoC.Instance.Flags.Get(GameFlagIds.BenoitTestedBasiliskWomb) == 0 && Array.IndexOf([0, 3], CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus)) >= 0)
                AddRowButton(1, "Basil. Womb", TryToConvertToBassyWomb);

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitSuggestUnlocked) > 0 && Player.HasVagina() && (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 0 || CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) == 3))
                AddRowButton(1, "Suggest", EggySuggest);

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEyesTalkUnlocked) == 1 && Player.Body.Get<EyesPart>().Id != BodyPartTypes.Basilisk)
                AddRowButton(1, "Basil. Eyes", ConvertToBassyEyes);
        }

        public void BuyOrSellExplanationFirstTime()
        {

            BenoitSprite();

            CoC.Instance.Flags.Set(GameFlagIds.BenoitExplainedShop, CoC.Instance.Flags.Get(GameFlagIds.BenoitExplainedShop) + 1);

            OutputText("Scenes.Benoit.BuyOrSellExplanationFirstTime.SpectacularSalvageBuySell".Localized());
        }

        public void BenoitsBuyMenu()
        {
            ClearOutput();

            if (silly)
            {
            }
            else
                BenoitSprite();

            var itypes = new[] { ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.Benoit1))), ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.Benoit2))), ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.Benoit3))) };

            if (string.IsNullOrEmpty(CoC.Instance.Flags.Get(GameFlagIds.Benoit1)))
                UpdateBenoitInventory();

            float buyMod = 2;

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitExplainedShop) == 0)
                BuyOrSellExplanationFirstTime();
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) != 0)
                {
                    buyMod = 1.66f;

                    OutputText("Scenes.Benoit.BenoitsBuyMenu.SomeCallJunkBuy".Localized());
                }
                else
                    OutputText("Scenes.Benoit.BenoitsBuyMenu.SomeCallZisJunk".Localized());

            OutputText("Scenes.Benoit.BenoitsBuyMenu.PricesHeaderPrices".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BenoitsBuyMenu.NewlineZeroColonOne".Localized(
                 itypes[0].LongName,

                 buyMod * itypes[0].Value
            ));

            OutputText("Scenes.Benoit.BenoitsBuyMenu.NewlineColonOneAgain".Localized(
                 itypes[1].LongName,

                 buyMod * itypes[1].Value
            ));

            OutputText("Scenes.Benoit.BenoitsBuyMenu.NewlineColonOneThird".Localized(
                 itypes[2].LongName,

                 buyMod * itypes[2].Value
            ));

            ClearButtons();

            AddButton(0, itypes[0].Name, () => BenoitTransactBuy(1));

            AddButton(1, itypes[1].Name, () => BenoitTransactBuy(2));

            AddButton(2, itypes[2].Name, () => BenoitTransactBuy(3));

            if (Player.KeyItemV1("Backpack") < 5)
                AddRowButton(2, "Backpack", BuyBackpack);

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitPistolBought) <= 0)
                AddRowButton(2, "Flintlock", BuyFlintlock);

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitClockBought) <= 0 && CabinInterior.HasNightstand)
                AddRowButton(2, "Alarm Clock", BuyAlarmClock);

            if ((Player.Level >= 20 || View.Time.Days >= 20 * 6) && !Player.HasKeyItem("Poorly done map to Volcanic Crag"))
                AddRowButton(2, "Strange Map", BuyMap);

            AddButton(14, "Common.Button.Back".Localized(), BenoitIntro);
        }

        public void BenoitSellMenu()
        {
            ClearOutput();

            BenoitSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitExplainedShop) == 0)
                BuyOrSellExplanationFirstTime();
            else
                OutputText("Scenes.Benoit.BenoitSellMenu.LetUsFeelPalm".Localized(
                     BenoitMF("", "e"),

                     BenoitMF("his", "her")
                ));

            var sellMod = 3;

            OutputText("Scenes.Benoit.BenoitSellMenu.SellAllStackHint".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) > 0 || CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) != 0)
                sellMod = 2;

            OutputText("Scenes.Benoit.BenoitSellMenu.EstimatesHeaderBenoit".Localized(
                 BenoitMF("", "e")
            ));

            ClearButtons();

            var totalItems = 0;

            foreach (var slot in Enumerable.Range(0, 10))
                if (Player.Inventory.Slots[slot].Quantity > 0 && (int)(Player.Inventory.Slots[slot].Item.Value / sellMod) >= 1)
                {
                    OutputText("Scenes.Benoit.BenoitSellMenu.NewlineGemsForCount".Localized(
                         (int)(Player.Inventory.Slots[slot].Item.Value / sellMod),

                         Player.Inventory.Slots[slot].Item.LongName
                    ));

                    AddButton(slot, Player.Inventory.Slots[slot].InvLabel, () => BenoitSellTransact(slot, sellMod));

                    totalItems += Player.Inventory.Slots[slot].Quantity;
                }

            if (totalItems > 1)
                AddButton(12, "Common.Button.SellAll".Localized(), () => BenoitSellAllTransact(totalItems, sellMod));

            AddButton(14, "Common.Button.Back".Localized(), BenoitIntro);
        }

        public void BenoitTransactBuy(int slot = 1)
        {
            ClearOutput();

            if (silly)
            {
            }
            else
                BenoitSprite();

            Item itype = null;

            float buyMod = 2;

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) != 0)
                buyMod = 1.66f;

            if (slot == 1)
                itype = ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.Benoit1)));
            else
                if (slot == 2)
                    itype = ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.Benoit2)));
                else
                    itype = ItemRegistry.Create(Identifier.Parse(CoC.Instance.Flags.Get(GameFlagIds.Benoit3)));

            if (Player.Gems < (float)(buyMod * itype.Value))
            {
                OutputText("Scenes.Benoit.BenoitTransactBuy.LackGemsPurchase".Localized());

                DoNext(() => BenoitsBuyMenu());

                return ;
            }

            if (BenoitLover())
                OutputText("Scenes.Benoit.BenoitTransactBuy.AfterExaminingFingersHands".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite")
                ));
            else
                OutputText("Scenes.Benoit.BenoitTransactBuy.AfterExaminingNamesPrice".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite")
                ));

            BenoitAffection(3);

            Player.Gems -= (int)Math.Round(buyMod * itype.Value);

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1 && false )
            {
                itype.OnUse();

                DoNext(() => BenoitsBuyMenu());
            }
            else
                InventoryScene.TakeItem(itype, BenoitsBuyMenu);
        }

        public void BenoitSellTransact(int slot, int sellMod)
        {
            ClearOutput();

            BenoitSprite();

            if (BenoitLover())
                OutputText("Scenes.Benoit.BenoitSellTransact.BenoitBriefestGoingsOver".Localized(
                     BenoitMF("", "e"),

                     BenoitMF("his", "her")
                ));
            else
                OutputText("Scenes.Benoit.BenoitSellTransact.PainstakingHandsNose".Localized(
                     BenoitMF("him", "her")
                ));

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
                while (Player.Inventory.Slots[slot].Quantity > 0)
                {
                    Player.Gems += (int)(Player.Inventory.Slots[slot].Item.Value / sellMod);

                    Player.Inventory.Slots[slot].RemoveOneItem();

                    BenoitAffection(1);
                }
            else
            {
                Player.Gems += (int)(Player.Inventory.Slots[slot].Item.Value / sellMod);

                Player.Inventory.Slots[slot].RemoveOneItem();

                BenoitAffection(1);
            }

            DoNext(() => BenoitSellMenu());
        }

        public void BenoitSellAllTransact(int totalItems, int sellMod)
        {
            ClearOutput();

            BenoitSprite();

            var itemValue = 0;

            foreach (var slot in Enumerable.Range(0, 10))
                if (Player.Inventory.Slots[slot].Quantity > 0 && (int)(Player.Inventory.Slots[slot].Item.Value / sellMod) >= 1)
                {
                    itemValue += Player.Inventory.Slots[slot].Quantity * (int)(Player.Inventory.Slots[slot].Item.Value / sellMod);

                    Player.Inventory.Slots[slot].Quantity = 0;
                }

            if (BenoitLover())
                OutputText("Scenes.Benoit.BenoitSellAllTransact.BenoitBriefestGoingsObjects".Localized(
                     BenoitMF("", "e"),

                     BenoitMF("his", "her"),

                     NumberUtils.Num2Text(itemValue)
                ));
            else
                OutputText("Scenes.Benoit.BenoitSellAllTransact.PainstakingHandsNose".Localized(
                     NumberUtils.Num2Text(itemValue)
                ));

            Player.Gems += itemValue;

            BenoitAffection(totalItems);

            DoNext(() => BenoitIntro());
        }

        public void UpdateBenoitInventory()
        {

            var benoitSlot1Items = new List<Identifier>();

            var benoitSlot2Items = new List<Identifier>();

            var benoitSlot3Items = new List<Identifier>();

            benoitSlot1Items = [ConsumableIds.IncubiDraft, ConsumableIds.MinotaurBlood, ConsumableIds.MinotaurCum, ItemIds.Equinum, ConsumableIds.BlackCaninePepper, ConsumableIds.ScholarTea, ConsumableIds.VitalityTincture, ConsumableIds.DoubleCaninePepper, ConsumableIds.Reptlum];

            benoitSlot1Items.Add(Random.Shared.Next(3) == 0 ? ConsumableIds.PureHoney : ConsumableIds.BeeHoney);

            CoC.Instance.Flags.Set(GameFlagIds.Benoit1, benoitSlot1Items[Random.Shared.Next(benoitSlot1Items.Count)]);

            if (Random.Shared.Next(10) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.Benoit1, ConsumableIds.GodMead);

            if (Random.Shared.Next(100) >= 4)
                benoitSlot2Items = [ConsumableIds.SuccubiMilk, ConsumableIds.WhiskerFruit, ConsumableIds.SlimyClothoth, ConsumableIds.GoldenSeed, ConsumableIds.LaBova, ConsumableIds.SnakeOil, ConsumableIds.SweetGossamer, ConsumableIds.Hummus, ConsumableIds.PigtailTruffle, ConsumableIds.BlackGossamer];
            else
                benoitSlot2Items = [ItemIds.BimboLiqueur, ConsumableIds.EggLargePink, ConsumableIds.EggLargeBlue, ConsumableIds.BroBrew, ConsumableIds.TentacleTooth, ConsumableIds.PumpkinSlice];

            CoC.Instance.Flags.Set(GameFlagIds.Benoit2, benoitSlot2Items[Random.Shared.Next(benoitSlot2Items.Count)]);

            if (Random.Shared.Next(100) >= 10)
                benoitSlot3Items = [ArmorIds.WizardRobes, ArmorIds.SkimpySwimwear, ItemIds.GreenGel, UseableIds.BeeChitin, ConsumableIds.ToughSilk];
            else
                benoitSlot3Items = [ConsumableIds.D_SCALE, ArmorIds.BondageStraps, ArmorIds.NurseOutfit, ItemIds.WinterPudding];

            CoC.Instance.Flags.Set(GameFlagIds.Benoit3, benoitSlot3Items[Random.Shared.Next(benoitSlot3Items.Count)]);
        }

        public void BuyFlintlock()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.BuyFlintlock.WanderShopSearchingInteresting".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BuyFlintlock.WalkOverPickZis".Localized(
                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.BuyFlintlock.TimeToTestWeapon".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BuyFlintlock.DoYouBuyGun".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), BuyFlintlockConfirmation);

            AddButton(1, "Common.Button.No".Localized(), BenoitsBuyMenu);
        }

        public void BuyFlintlockConfirmation()
        {
            ClearOutput();

            BenoitSprite();

            if (Player.Gems < 200)
            {
                OutputText("Scenes.Benoit.BuyFlintlockConfirmation.CountGemsBeyondRange".Localized());

                DoNext(() => BenoitsBuyMenu());

                return ;
            }

            OutputText("Scenes.Benoit.BuyFlintlockConfirmation.HereNoNeedZis".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            CoC.Instance.Flags.Set(GameFlagIds.BenoitPistolBought, CoC.Instance.Flags.Get(GameFlagIds.BenoitPistolBought) + 1);

            Player.Gems -= 200;

            InventoryScene.TakeItem(ItemIds.FlintlockPistol, BenoitsBuyMenu);
        }

        public void BuyMap()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.BuyMap.InquireAboutMap".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BuyMap.DemonTriedZellIt".Localized(
                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.BuyMap.HardBargainBuyMap".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FoundWizardTower) >= 1)
                OutputText("Scenes.Benoit.BuyMap.SinceFoundTowerUseless".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), BuyMapConfirmation);

            AddButton(1, "Common.Button.No".Localized(), BenoitsBuyMenu);
        }

        public void BuyMapConfirmation()
        {
            ClearOutput();

            BenoitSprite();

            if (Player.Gems < 2500)
            {
                OutputText("Scenes.Benoit.BuyMapConfirmation.CountBeyondRangeAgain".Localized());

                DoNext(() => BenoitsBuyMenu());

                return ;
            }

            OutputText("Scenes.Benoit.BuyMapConfirmation.NoNeedZisAgain".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BuyMapConfirmation.BoughtLeadsVolcanicCrag".Localized());

            Player.CreateKeyItem("Poorly done map to Volcanic Crag", 0, 0, 0, 0);

            Player.Gems -= 2500;

            DoNext(() => BenoitsBuyMenu());
        }

        public void BuyAlarmClock()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.BuyAlarmClock.WanderShopSearchingClock".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BuyAlarmClock.MechanicalClockFlipDisplay".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BuyAlarmClock.WorksFiveHundred".Localized(
                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.BuyAlarmClock.DoYouBuyClock".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), BuyAlarmClockConfirmation);

            AddButton(1, "Common.Button.No".Localized(), BenoitsBuyMenu);
        }

        public void BuyAlarmClockConfirmation()
        {
            ClearOutput();

            BenoitSprite();

            if (Player.Gems < 500)
            {
                OutputText("Scenes.Benoit.BuyAlarmClockConfirmation.CountBeyondRangeThird".Localized());

                DoNext(() => BenoitsBuyMenu());

                return ;
            }

            OutputText("Scenes.Benoit.BuyAlarmClockConfirmation.NoNeedZisThird".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BuyAlarmClockConfirmation.CanSetAlarmCabin".Localized());

            Player.Gems -= 500;

            CoC.Instance.Flags.Set(GameFlagIds.BenoitClockBought, CoC.Instance.Flags.Get(GameFlagIds.BenoitClockBought) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.BenoitClockAlarm, 6);

            DoNext(() => BenoitsBuyMenu());
        }

        public void BuyBackpack()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.BuyBackpack.AskBackpackToSpare".Localized(
                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.BuyBackpack.YesThreeSizesWhich".Localized(
                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.BuyBackpack.BackpackSizePricings".Localized());

            OutputText("Scenes.Benoit.BuyBackpack.SmallTwoThreeSlots".Localized());

            OutputText("Scenes.Benoit.BuyBackpack.MediumHundredFourSlots".Localized());

            OutputText("Scenes.Benoit.BuyBackpack.LargeHundredFiveSlots".Localized());

            ClearButtons();

            if (Player.KeyItemV1("Backpack") < 3)
                AddButton(0, "Common.Button.Small".Localized(), () => BuyBackpackConfirmation(3, "Small", 200));

            if (Player.KeyItemV1("Backpack") < 4)
                AddButton(1, "CharCreation.MenuBeardLength.Button4".Localized(), () => BuyBackpackConfirmation(4, "Medium", 600));

            if (Player.KeyItemV1("Backpack") < 5)
                AddButton(2, "Common.Button.Large".Localized(), () => BuyBackpackConfirmation(5, "Large", 1200));

            AddButton(4, "Scenes.CorruptedGlade.NeverMind".Localized(), BenoitsBuyMenu);
        }

        public void BuyBackpackConfirmation(int size = 3, string sizeDesc = "Small", int price = 200)
        {
            ClearOutput();

            BenoitSprite();

            if (Player.Gems < price)
            {
                OutputText("Scenes.Benoit.BuyBackpackConfirmation.CountBeyondRangeFourth".Localized());

                DoNext(() => BenoitsBuyMenu());

                return ;
            }

            OutputText("Scenes.Benoit.BuyBackpackConfirmation.NoNeedZisFourth".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            if (Player.HasKeyItem("Backpack"))
            {
                OutputText("Scenes.Benoit.BuyBackpackConfirmation.KeyUpgradedBackpackSlots".Localized(
                     sizeDesc,

                     NumberUtils.Num2Text(size - Player.KeyItemV1("Backpack"))
                ));

                Player.AddKeyValue("Backpack", 1, size - Player.KeyItemV1("Backpack"));

                OutputText("Scenes.Benoit.BuyBackpackConfirmation.ForTotalOfSlots".Localized(
                     NumberUtils.Num2Text(Player.Inventory.MaxSlots)
                ));
            }
            else
            {
                OutputText("Scenes.Benoit.BuyBackpackConfirmation.KeyGainedBackpackSlots".Localized(
                     sizeDesc,

                     NumberUtils.Num2Text(size)
                ));

                Player.CreateKeyItem("Backpack", size, 0, 0, 0);

                OutputText("Scenes.Benoit.BuyBackpackConfirmation.ForTotalSlotsAgain".Localized(
                     NumberUtils.Num2Text(Player.Inventory.MaxSlots)
                ));
            }

            var wantedSlots = (int)Player.KeyItemV1("Backpack"); for (var slotIndex = 0; slotIndex < wantedSlots && slotIndex < Player.Inventory.Slots.Length; slotIndex++) Player.Inventory.Slots[slotIndex].Unlocked = true;

            Player.Gems -= price;

            DoNext(() => BenoitsBuyMenu());
        }

        public void TalkToBenoit()
        {
            ClearOutput();

            BenoitSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitTalkedToday) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.BenoitTalkedToday, 1);

                BenoitAffection(5);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitBasilEyesGranted) > 0 && !Player.HasKeyItem("Feathery hair-pin"))
            {
                var hasSolidHair = (Player.Body.Get<HairPart>().Id != BodyPartTypes.Goo && Player.Body.Get<HairPart>().Length > 0);

                OutputText("Scenes.Benoit.TalkToBenoit.AdBeenOpingSpeak".Localized((noFur ? "" : " the scales on "), BenoitMF("Benoit", "Benoite"), BenoitMF("he", "she")));

                OutputText("Scenes.Benoit.TalkToBenoit.HoldSilverHairPin".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.TalkToBenoit.NussingCanPay".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.TalkToBenoit.BlushCreepThankHugging".Localized(
                     BenoitMF("him", "her")
                ));

                if (hasSolidHair && Player.Corruption < 55)
                {
                    OutputText("Scenes.Benoit.TalkToBenoit.SlippingPinIntoHair".Localized());

                    Player.CreateKeyItem("Feathery hair-pin", 1, 1, 0, 0);
                }
                else
                {
                    OutputText("Scenes.Benoit.TalkToBenoit.SentencePeriod".Localized());

                    Player.CreateKeyItem("Feathery hair-pin", 0, 0, 0, 0);
                }

                OutputText("Scenes.Benoit.TalkToBenoit.GainedFeatheryHairPin".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (BenoitBigFamily() && Player.Intelligence >= 60 && CoC.Instance.Flags.Get(GameFlagIds.BenoitEyesTalkUnlocked) == 0)
            {
                OutputText("Scenes.Benoit.TalkToBenoit.AskPetrifyingEffectGaze".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She")
                ));

                OutputText("Scenes.Benoit.TalkToBenoit.DoKnowAmBlind".Localized());

                OutputText("Scenes.Benoit.TalkToBenoit.HurriedlyTellNotCruel".Localized(
                     BenoitMF("him", "her")
                ));

                OutputText("Scenes.Benoit.TalkToBenoit.AfterFewMomentsNods".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.TalkToBenoit.RecklessExperimentOnYourself".Localized());

                OutputText("Scenes.Benoit.TalkToBenoit.SmileAtConcernLeaving".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                CoC.Instance.Flags.Set(GameFlagIds.BenoitEyesTalkUnlocked, 1);

                OutputText("Scenes.Benoit.TalkToBenoit.BasiliskEyesOptionEnabled".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (((CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) > 2 && Player.Intelligence >= 60 && CoC.Instance.Flags.Get(GameFlagIds.BenoitWombTalkUnlocked) == 0) || (CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) == 2 && CoC.Instance.Flags.Get(GameFlagIds.BenoitWombTalkUnlocked) == 0)) && Player.HasVagina())
            {
                OutputText("Scenes.Benoit.TalkToBenoit.AskThoughtHelpPeople".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.TalkToBenoit.SilentClawsAlongCounter".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("he", "she")
                ));

                if (Player.Intelligence >= 60)
                {
                    OutputText("Scenes.Benoit.TalkToBenoit.MindWandersTurnBasilisk".Localized());

                    OutputText("Scenes.Benoit.TalkToBenoit.AnswersWarilyDoubleDose".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.BenoitWombTalkUnlocked, 1);

                    OutputText("Scenes.Benoit.TalkToBenoit.BasiliskWombOptionEnabled".Localized());
                }
                else
                    OutputText("Scenes.Benoit.TalkToBenoit.RackCantThinkAnything".Localized(
                         BenoitMF("him", "her")
                    ));

                DoNext(() => ReturnToCamp(1));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.BenoitTalkedToProperly) == 0)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.BenoitTalkedToProperly, CoC.Instance.Flags.Get(GameFlagIds.BenoitTalkedToProperly) + 1);

                    OutputText("Scenes.Benoit.TalkToBenoit.TakeMomentLookMerchant".Localized(
                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("him", "her")
                    ));

                    OutputText("Scenes.Benoit.TalkToBenoit.ProudRaceOnceNoble".Localized(BenoitMF("he", "she"), BenoitMF("his ", "her "), (noFur ? "Common.Text.Chin".Localized() : "BodyParts.Descript.Face.Noun.Snout".Localized())));

                    OutputText("Scenes.Benoit.TalkToBenoit.ShrugsWhatToDo".Localized(
                         BenoitMF("him", "her")
                    ));
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.FemoitTalkedTo) == 0 && CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) != 0)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.FemoitTalkedTo, CoC.Instance.Flags.Get(GameFlagIds.FemoitTalkedTo) + 1);

                        OutputText("Scenes.Benoit.TalkToBenoit.AskBenoiteIntendsMountains".Localized());

                        OutputText("Scenes.Benoit.TalkToBenoit.SoFondlyHowLong".Localized());

                        if (!Player.HasCock())
                        {
                            OutputText("Scenes.Benoit.TalkToBenoit.AskThoughtsNotReally".Localized());

                            DoNext(() => BenoitIntro());
                        }
                        else
                        {
                            OutputText("Scenes.Benoit.TalkToBenoit.AskThoughtsFrontCustomer".Localized());

                            ClearButtons();

                            AddButton(0, "Common.Button.Yes".Localized(), FemoitFirstTimeYes);

                            AddButton(1, "Common.Button.No".Localized(), FemoitFirstTimeNo);
                        }

                        return ;
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.BenoitTalkedToProperly) != 0 && BenoitAffection() >= 40 && CoC.Instance.Flags.Get(GameFlagIds.FemoitUnlocked) == 0)
                        {
                            FemoitInitialTalk();

                            DoNext(() => ReturnToCamp(1));

                            return ;
                        }
                        else
                        {
                            int choice = 0;

                            var choices = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7 };

                            if (CoC.Instance.Flags.Get(GameFlagIds.CodexEntryHarpies) > 0 && CoC.Instance.Flags.Get(GameFlagIds.CodexEntryBasilisks) > 0)
                            {
                                choices.Add(8);

                                if (CoC.Instance.Flags.Get(GameFlagIds.CockatricesUnlocked) <= 0)
                                    choices.Add(8);
                            }

                            if (!BenoitLover() && BenoitMF("he", "she") == "he")
                                choices.Add(9);

                            if (BenoitLover() && BenoitMF("he", "she") == "he" && Player.HasVagina())
                            {
                                choices.Add(10);

                                choices.Add(11);

                                choices.Add(12);

                                choices.Add(13);
                            }

                            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) != 0)
                            {
                                choices.Add(14);

                                choices.Add(15);

                                if (BenoitLover())
                                    choices.Add(16);
                            }

                            choice = choices[Random.Shared.Next(choices.Count)];

                            if (choice == 0)
                            {
                                OutputText("Scenes.Benoit.TalkToBenoit.AskAllBasilisksTalk".Localized(
                                     BenoitMF("he", "she")
                                ));

                                OutputText("Scenes.Benoit.TalkToBenoit.OnlyZisSideMountain".Localized(
                                     BenoitMF("He", "She")
                                ));
                            }
                            else
                                if (choice == 1)
                                {
                                    OutputText("Scenes.Benoit.TalkToBenoit.AskAboutDog".Localized(
                                         BenoitMF("Benoit", "Benoite")
                                    ));

                                    OutputText("Scenes.Benoit.TalkToBenoit.PierreGivingTroubleMutt".Localized());
                                }
                                else
                                    if (choice == 2)
                                    {
                                        OutputText("Scenes.Benoit.TalkToBenoit.AskAboutAboveShop".Localized(
                                             BenoitMF("him", "her")
                                        ));

                                        OutputText("Scenes.Benoit.TalkToBenoit.TraderSaysProudlyCatguy".Localized(
                                             BenoitMF("He", "She"),

                                             BenoitMF("his", "her"),

                                             BenoitMF("Benoit", "Benoite")
                                        ));

                                        OutputText("Scenes.Benoit.TalkToBenoit.NotToDisillusionBlind".Localized());
                                    }
                                    else
                                        if (choice == 3)
                                        {
                                            OutputText("Scenes.Benoit.TalkToBenoit.AskAlwaysBeenBlind".Localized(
                                                 BenoitMF("he", "she")
                                            ));

                                            OutputText("Scenes.Benoit.TalkToBenoit.DontKnowWhatRed".Localized(
                                                 BenoitMF("he", "she")
                                            ));

                                            OutputText("Scenes.Benoit.TalkToBenoit.AskWhatRedIs".Localized());

                                            OutputText("Scenes.Benoit.TalkToBenoit.HowShouldIKnow".Localized());

                                            OutputText("Scenes.Benoit.TalkToBenoit.NotToPursueSubject".Localized());
                                        }
                                        else
                                            if (choice == 4)
                                            {
                                                OutputText("Scenes.Benoit.TalkToBenoit.AskGotLineWork".Localized(
                                                     BenoitMF("Benoit", "Benoite"),

                                                     BenoitMF("he", "she")
                                                ));

                                                OutputText("Scenes.Benoit.TalkToBenoit.AlwaysWorkedSalvage".Localized(
                                                     BenoitMF("he", "she"),

                                                     BenoitMF("He", "She")
                                                ));

                                                OutputText("Scenes.Benoit.TalkToBenoit.LaughsThroatilyRottenSour".Localized(
                                                     BenoitMF("He", "She"),

                                                     BenoitMF("Benoit", "Benoite"),

                                                     BenoitMF("he", "she")
                                                ));

                                                Player.ApplyStats(new DynStat(GameStats.Corruption, -1f));
                                            }
                                            else
                                                if (choice == 5)
                                                {
                                                    OutputText("Scenes.Benoit.TalkToBenoit.AskAboutBazaar".Localized(
                                                         BenoitMF("he", "she")
                                                    ));

                                                    OutputText("Scenes.Benoit.TalkToBenoit.AskingBlindPersonSniggers".Localized(
                                                         BenoitMF("He", "She"),

                                                         BenoitMF("He", "She"),

                                                         BenoitMF("He", "She")
                                                    ));
                                                }
                                                else
                                                    if (choice == 6)
                                                    {
                                                        OutputText("Scenes.Benoit.TalkToBenoit.AskRumorsGoingAround".Localized(
                                                             BenoitMF("Benoit", "Benoite")
                                                        ));

                                                        if (CoC.Instance.Flags.Get(GameFlagIds.DefeatedZetaz) > 0)
                                                            OutputText("Scenes.Benoit.TalkToBenoit.SomesingStrangeZisserDay".Localized(
                                                                 BenoitMF("he", "she"),

                                                                 BenoitMF("Benoit", "Benoite")
                                                            ));
                                                        else
                                                            if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) <= 0)
                                                                OutputText("Scenes.Benoit.TalkToBenoit.NotAnysingInterestingCustomers".Localized(
                                                                     BenoitMF("he", "she")
                                                                ));
                                                            else
                                                                if (CoC.Instance.Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                                                                    OutputText("Scenes.Benoit.TalkToBenoit.DontKnowWhatAppening".Localized(
                                                                         BenoitMF("he", "she")
                                                                    ));
                                                                else
                                                                    OutputText("Scenes.Benoit.TalkToBenoit.DontKnowAppening".Localized(
                                                                         BenoitMF("he", "she")
                                                                    ));
                                                    }
                                                    else
                                                        if (choice == 7)
                                                        {
                                                            OutputText("Scenes.Benoit.TalkToBenoit.AskTroubleWithDemons".Localized(
                                                                 BenoitMF("he", "she")
                                                            ));

                                                            OutputText("Scenes.Benoit.TalkToBenoit.NotDontLikeZem".Localized(
                                                                 BenoitMF("he", "she")
                                                            ));

                                                            if (silly)
                                                                OutputText("Scenes.Benoit.TalkToBenoit.RubsPierreBehindEar".Localized(
                                                                     BenoitMF("he", "she"),

                                                                     BenoitMF("He", "She")
                                                                ));
                                                        }
                                                        else
                                                            if (choice == 8)
                                                            {
                                                                OutputText("Scenes.Benoit.TalkToBenoit.AskBasilisksMateHarpies".Localized());

                                                                OutputText("Scenes.Benoit.TalkToBenoit.MostZeTimeBasilisks".Localized());

                                                                if (CoC.Instance.Flags.Get(GameFlagIds.CockatricesUnlocked) <= 0)
                                                                {
                                                                    OutputText("Scenes.Benoit.TalkToBenoit.FindHybridsHighMountains".Localized());

                                                                    CoC.Instance.Flags.Set(GameFlagIds.CockatricesUnlocked, 1);
                                                                }
                                                            }
                                                            else
                                                                if (choice == 9)
                                                                {
                                                                    OutputText("Scenes.Benoit.TalkToBenoit.AskTellBySmell".Localized(
                                                                         BenoitMF("Benoit", "Benoite")
                                                                    ));

                                                                    if (Player.Race == "human")
                                                                        OutputText("Scenes.Benoit.TalkToBenoit.CertainlySmellShavedMonkey".Localized(
                                                                             BenoitMF("he", "she"),

                                                                             BenoitMF("his", "her")
                                                                        ));
                                                                    else
                                                                        OutputText("Scenes.Benoit.TalkToBenoit.CertainlySmellShavedAgain".Localized(
                                                                             BenoitMF("he", "she")
                                                                        ));

                                                                    if (BenoitMF("he", "she") == "he")
                                                                        OutputText("Scenes.Benoit.TalkToBenoit.HintLongingJealousy".Localized(
                                                                             BenoitMF("he", "she")
                                                                        ));
                                                                }
                                                                else
                                                                    if (choice == 10)
                                                                    {
                                                                        OutputText("Scenes.Benoit.TalkToBenoit.AskUsefulDemonStrongholds".Localized(
                                                                             BenoitMF("he", "she")
                                                                        ));

                                                                        OutputText("Scenes.Benoit.TalkToBenoit.AfraidCannotBeElpful".Localized(
                                                                             BenoitMF("he", "she")
                                                                        ));
                                                                    }
                                                                    else
                                                                        if (choice == 11)
                                                                        {
                                                                            OutputText("Scenes.Benoit.TalkToBenoit.AskSuggestFightBrethren".Localized(
                                                                                 BenoitMF("he", "she")
                                                                            ));

                                                                            OutputText("Scenes.Benoit.TalkToBenoit.CarryMirrorPointing".Localized(
                                                                                 BenoitMF("he", "she"),

                                                                                 BenoitMF("he", "she")
                                                                            ));
                                                                        }
                                                                        else
                                                                            if (choice == 12)
                                                                            {
                                                                                OutputText("Scenes.Benoit.TalkToBenoit.GnollWord".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.GentleHummingNoise".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.BeeMaidenAfterMoment".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.StampFeetSnuffleSnort".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.MinotaurImmediatelyTooGood".Localized(
                                                                                     BenoitMF("he", "she")
                                                                                ));

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.WhatZeEllSupposed".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.UnitaurWord".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.AWhatWord".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.ExplainUnitaurWhiteCentaur".Localized());

                                                                                OutputText("Scenes.Benoit.TalkToBenoit.OhSeePullingTail".Localized());
                                                                            }
                                                                            else
                                                                                if (choice == 13)
                                                                                {
                                                                                    OutputText("Scenes.Benoit.TalkToBenoit.AskReallyCanTell".Localized(
                                                                                         BenoitMF("he", "she")
                                                                                    ));

                                                                                    OutputText("Scenes.Benoit.TalkToBenoit.WellOfCourseTeasingly".Localized(
                                                                                         BenoitMF("he", "she")
                                                                                    ));
                                                                                }
                                                                                else
                                                                                    if (choice == 14)
                                                                                    {
                                                                                        OutputText("Scenes.Benoit.TalkToBenoit.AskGettingOppositeSex".Localized());

                                                                                        OutputText("Scenes.Benoit.TalkToBenoit.DifferentEventuallyLaughing".Localized());

                                                                                        if (BenoitLover() && Player.HasCock() && Player.HasVagina())
                                                                                            OutputText("Scenes.Benoit.TalkToBenoit.SmilesLuckyOneRespect".Localized());
                                                                                        else
                                                                                            if (BenoitLover() && Player.HasCock() && !Player.HasVagina())
                                                                                            {
                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.SmilesShylyHadResink".Localized());

                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.CharmingYouSay".Localized());

                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.GrinsWiderAffectedHurt".Localized());
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.SmilesShylyListenRiveting".Localized());

                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.PointOutMutabilityAdvantage".Localized());

                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.NodsThoughtfullyNeverSought".Localized());
                                                                                            }
                                                                                    }
                                                                                    else
                                                                                        if (choice == 15)
                                                                                        {
                                                                                            OutputText("Scenes.Benoit.TalkToBenoit.AskWorriedCustomersNotice".Localized());

                                                                                            OutputText("Scenes.Benoit.TalkToBenoit.CunningDisguiseLargeBeret".Localized());
                                                                                        }
                                                                                        else
                                                                                            if (choice == 16)
                                                                                            {
                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.AskBenoiteTellSmell".Localized());

                                                                                                OutputText("Scenes.Benoit.TalkToBenoit.WellOfCourseZilly".Localized());
                                                                                            }

                            if (Player.HasKeyItem("Feathery hair-pin") && (false || CoC.Instance.Flags.Get(GameFlagIds.BenoitHairpinTalkedToday) == 0))
                            {
                                DoNext(() => BenoitHairPinTalk());

                                return ;
                            }
                        }

            DoNext(() => BenoitIntro());
        }

        public void BenoitHairPinTalk()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.BenoitHairPinTalk.AskFeatheryHairPin".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.BenoitHairPinTalk.AhZePinEirloom".Localized());

            BenoitHairPinTFCheck();
        }

        public void BenoitHairPinTFCheck()
        {

            BenoitSprite();

            if (Player.Corruption < 30 && (Player.IsFemale() || Player.IsHerm()) && Player.FeatheryHairPinEquipped() && Array.IndexOf([BodyPartTypes.BasiliskPlume, BodyPartTypes.Goo], Player.Body.Get<HairPart>().Id) == -1)
            {
                OutputText("Scenes.Benoit.BenoitHairPinTFCheck.FeelHairPinHeat".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.BenoitHairPinTFCheck.DoYouLetIt".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), BenoitHairPinTFYes);

                AddButton(1, "Common.Button.No".Localized(), BenoitHairPinTFNo);

                return ;
            }

            BenoitHairPinTalkFinal();
        }

        public void BenoitHairPinTFNo()
        {

            BenoitSprite();

            OutputText("Scenes.Benoit.BenoitHairPinTFNo.TakeOutHairPin".Localized());

            BenoitHairPinTalkFinal();
        }

        public void BenoitHairPinTFYes()
        {

            BenoitSprite();

            OutputText("Scenes.Benoit.BenoitHairPinTFYes.LetFeelingHeadTickle".Localized(
                 BenoitMF(" that your hair has changed into a plume of feathers that, like legend is told, belongs to a female basilisktrue that you have a plume, like a female basilisk!")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.HairGrowthStoppedBecauseLizard) != 0)
                OutputText("Scenes.Benoit.BenoitHairPinTFYes.HairGrowingRedFeathers".Localized());
            else
                OutputText("Scenes.Benoit.BenoitHairPinTFYes.HairPlumeRedFeathers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

            Player.Body.Get<HairPart>().Length = 2;

            Player.Body.Get<HairPart>().Color = "red";

            Player.Body.Get<HairPart>().Id = BodyPartTypes.BasiliskPlume;

            BenoitHairPinTalkFinal();
        }

        public void BenoitHairPinTalkFinal()
        {

            BenoitSprite();

            Player.ApplyStats(new DynStat(GameStats.Corruption, -(Player.FeatheryHairPinEquipped() ? 10 : 5)));

            CoC.Instance.Flags.Set(GameFlagIds.BenoitHairpinTalkedToday, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BenoitAndFemPCTalkAboutEggings()
        {
            ClearOutput();

            BenoitSprite();

            CoC.Instance.Flags.Set(GameFlagIds.BenoitSuggestUnlocked, 1);

            OutputText("Scenes.Benoit.BenoitAndFemPCTalkAboutEggings.SeemsOnEdgePaces".Localized(
                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("He", "She"),

                 BenoitMF("his", "her")
            ));

            OutputText("Scenes.Benoit.BenoitAndFemPCTalkAboutEggings.NuzzingBarksAngrilyCoughs".Localized(
                 BenoitMF("he", "she"),

                 BenoitMF("He", "She"),

                 BenoitMF("he", "she"),

                 BenoitMF("he", "she"),

                 BenoitMF("he", "she"),

                 BenoitMF("his", "her"),

                 BenoitMF("his", "her"),

                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.BenoitAndFemPCTalkAboutEggings.SuggestOptionAddedMenu".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));
        }

        public void EggySuggest()
        {
            ClearOutput();

            BenoitSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitTestedBasiliskWomb) == .5f)
            {
                SuggestSexAfterBasiWombed(true);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) > 0)
            {
                if (Player.IsTaur())
                {
                    OutputText("Scenes.Benoit.EggySuggest.SilentlyReachSqueezeHands".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("Benoit", "Benoite")
                    ));

                    OutputText("Scenes.Benoit.EggySuggest.OnceAgainBlindCharge".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("His", "Her"),

                         BenoitMF("his", "her")
                    ));
                }
                else
                {
                    OutputText("Scenes.Benoit.EggySuggest.SilentlySqueezeHandsAgain".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("Benoit", "Benoite")
                    ));

                    OutputText("Scenes.Benoit.EggySuggest.InchBlindCharge".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("His", "Her"),

                         BenoitMF("his", "her")
                    ));
                }

                ClearButtons();

                AddButton(0, "Common.Text.Let".Localized() + BenoitMF("Him", "Her") + "", RepeatSexWithBenoitLetHim);

                AddButton(1, "Common.Button.TakeCharge".Localized(), RepeatBenoitFuckTakeCharge);

                return ;
            }

            CoC.Instance.Flags.Set(GameFlagIds.BenoitTimesSexedFempcs, CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) + 1);

            if (Player.IsTaur())
            {
                OutputText("Scenes.Benoit.EggySuggest.ReachFingersHandsDont".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.EggySuggest.YouWhatSlightlyStunned".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her")
                ));

                OutputText("Scenes.Benoit.EggySuggest.BackOfficePerfectPurposes".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("His", "Her")
                ));

                OutputText("Scenes.Benoit.EggySuggest.ZisStrangeThickVoice".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.EggySuggest.WarmFingersTravelBody".Localized(
                     BenoitMF("His", "Her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She")
                ));

                if (Player.Gender == Gender.Hermaphrodite)
                {
                    OutputText("Scenes.Benoit.EggySuggest.HandsTravelBehindTouches".Localized(
                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));

                    if (Player.BiggestCockLength() <= 10)
                        OutputText("Scenes.Benoit.EggySuggest.AlthoughUncannyCockySmile".Localized(
                             BenoitMF("him", "her"),

                             BenoitMF("he", "she"),

                             BenoitMF("his", "her"),

                             BenoitMF("he", "she"),

                             BenoitMF("his", "her"),

                             BenoitMF("he", "she")
                        ));
                    else
                        OutputText("Scenes.Benoit.EggySuggest.EvidentlyExperienceAlien".Localized(
                             BenoitMF("him", "her"),

                             BenoitMF("his", "her"),

                             BenoitMF("he", "she")
                        ));
                }

                OutputText("Scenes.Benoit.EggySuggest.HandsTrailUpwardsBestial".Localized(
                     BenoitMF("His", "Her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she")
                ));

                if (Player.InHeat)
                    OutputText("Scenes.Benoit.EggySuggest.VaginaGushingPheromoneNeed".Localized(
                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.EggySuggest.ScentArousalBreathComes".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                Player.CuntChange(12, true, true, false);

                OutputText("Scenes.Benoit.EggySuggest.DickIncrediblyIncredibleEase".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her")
                ));

                if (Player.VaginalCapacity() <= 30)
                    OutputText("Scenes.Benoit.EggySuggest.BottomOutCooing".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));
                else
                    OutputText("Scenes.Benoit.EggySuggest.GroinMuscledThighs".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.EggySuggest.KeepSlowRideHours".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her")
                ));

                if (Player.LactationQ() >= 50)
                    OutputText("Scenes.Benoit.EggySuggest.SlowSensualBeadMilk".Localized(
                         BenoitMF("Benoit", "Benoite"),

                         BenoitMF("he", "she")
                    ));

                OutputText("Scenes.Benoit.EggySuggest.LustRatchetsHandsBehind".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her")
                ));

                if (Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Benoit.EggySuggest.EachThickensSympathyHigh".Localized());
            }
            else
            {
                OutputText("Scenes.Benoit.EggySuggest.ReachFingersHandsYet".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.EggySuggest.YouWhatStunnedAgain".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her")
                ));

                OutputText("Scenes.Benoit.EggySuggest.BackPerfectPurposesAgain".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("His", "Her")
                ));

                OutputText("Scenes.Benoit.EggySuggest.ZisStrangeLowVoice".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                if (Player.Horns.Id.Path != "none" && Player.Horns.Id == BodyPartTypes.Demon && Player.Body.Get<TailPart>().Id == BodyPartTypes.Demonic && Player.DemonScore() >= 3)
                    OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodySecond".Localized(
                         BenoitMF("His", "Her"),

                         BenoitMF("He", "She"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her"),

                         BenoitMF("he", "she"),

                         BenoitMF("He", "She")
                    ));
                else
                    if (Player.Ears.Id == BodyPartTypes.Dog && Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog && Player.DogScore() >= 3)
                        OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyThird".Localized(
                             BenoitMF("His", "Her"),

                             BenoitMF("He", "She"),

                             BenoitMF("he", "she"),

                             BenoitMF("he", "she"),

                             BenoitMF("he", "she")
                        ));
                    else
                        if (Player.CatScore() >= 3 && Player.Body.Get<TailPart>().Id == BodyPartTypes.Cat && Player.Ears.Id == BodyPartTypes.Cat)
                            OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyFourth".Localized(
                                 BenoitMF("His", "Her"),

                                 BenoitMF("He", "She"),

                                 BenoitMF("he", "she"),

                                 BenoitMF("he", "she"),

                                 BenoitMF("he", "she")
                            ));
                        else
                            if (Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Harpy && Player.Body.Get<WingsPart>().Id == BodyPartTypes.FeatheredLarge)
                                OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyFifth".Localized(
                                     BenoitMF("His", "Her"),

                                     BenoitMF("He", "She"),

                                     BenoitMF("he", "she"),

                                     BenoitMF("he", "she"),

                                     BenoitMF("he", "she"),

                                     BenoitMF("his", "her"),

                                     BenoitMF("he", "she"),

                                     BenoitMF("his", "her"),

                                     BenoitMF("he", "she")
                                ));
                            else
                                if (Player.HasReptileScales() && (Player.LizardScore() >= 3 || Player.NagaScore() >= 3) || Player.DragonScore() >= 3 || Player.MermaidScore() >= 3)
                                    OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodySixth".Localized(
                                         BenoitMF("His", "Her"),

                                         BenoitMF("He", "She"),

                                         BenoitMF("he", "she"),

                                         BenoitMF("he", "she"),

                                         BenoitMF("his", "her")
                                    ));
                                else
                                    if ((Player.Body.Get<WingsPart>().Id == BodyPartTypes.BeeLikeSmall || Player.Body.Get<WingsPart>().Id == BodyPartTypes.BeeLikeLarge) && Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Bee)
                                        OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodySeventh".Localized(
                                             BenoitMF("His", "Her"),

                                             BenoitMF("He", "She"),

                                             BenoitMF("he", "she"),

                                             BenoitMF("his", "her"),

                                             BenoitMF("he", "she"),

                                             BenoitMF("his", "her"),

                                             BenoitMF("he", "she")
                                        ));
                                    else
                                        if (Player.IsTaur())
                                            OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyEighth".Localized(
                                                 BenoitMF("His", "Her"),

                                                 BenoitMF("he", "she"),

                                                 BenoitMF("his", "her"),

                                                 BenoitMF("he", "she"),

                                                 BenoitMF("he", "she"),

                                                 BenoitMF("he", "she"),

                                                 BenoitMF("He", "She")
                                            ));
                                        else
                                            if (Player.IsDrider())
                                                OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyNinth".Localized(
                                                     BenoitMF("His", "Her"),

                                                     BenoitMF("he", "she"),

                                                     BenoitMF("his", "her"),

                                                     BenoitMF("he", "she"),

                                                     BenoitMF("he", "she"),

                                                     BenoitMF("he", "she"),

                                                     BenoitMF("his", "her")
                                                ));
                                            else
                                                if (Player.IsGoo())
                                                    OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyTenth".Localized(
                                                         BenoitMF("His", "Her"),

                                                         BenoitMF("he", "she"),

                                                         BenoitMF("his", "her"),

                                                         BenoitMF("he", "she"),

                                                         BenoitMF("he", "she"),

                                                         BenoitMF("his", "her"),

                                                         BenoitMF("he", "she"),

                                                         BenoitMF("his", "her"),

                                                         BenoitMF("his", "her"),

                                                         BenoitMF("him", "her"),

                                                         BenoitMF("he", "she")
                                                    ));
                                                else
                                                    if ((Player.FoxScore() >= 4 || Player.KitsuneScore() >= 4) && Player.Ears.Id == BodyPartTypes.Fox && Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox)
                                                    {
                                                        if (Player.Body.Get<TailPart>().Venom <= 1)
                                                            OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyEleventh".Localized(
                                                                 BenoitMF("His", "Her"),

                                                                 BenoitMF("He", "She"),

                                                                 BenoitMF("he", "she"),

                                                                 BenoitMF("he", "she"),

                                                                 BenoitMF("he laughs", "she giggles")
                                                            ));
                                                        else
                                                            OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyTwelfth".Localized(
                                                                 BenoitMF("His", "Her"),

                                                                 BenoitMF("He", "She"),

                                                                 BenoitMF("he", "she"),

                                                                 BenoitMF("he", "she"),

                                                                 BenoitMF("he laughs", "she giggles")
                                                            ));
                                                    }
                                                    else
                                                        OutputText("Scenes.Benoit.EggySuggest.WarmFingersBodyThirteenth".Localized(
                                                             BenoitMF("His", "Her"),

                                                             BenoitMF("he", "she")
                                                        ));

                if (Player.HasCock())
                {
                    OutputText("Scenes.Benoit.EggySuggest.HandsDownwardsOneCock".Localized(
                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));

                    if (Player.Cocks[Player.BiggestCockIndex()].Length < 10)
                        OutputText("Scenes.Benoit.EggySuggest.AlthoughUncannySmileAgain".Localized(
                             BenoitMF("him", "her"),

                             BenoitMF("he", "she"),

                             BenoitMF("his", "her"),

                             BenoitMF("he", "she"),

                             BenoitMF("his", "her"),

                             BenoitMF("his", "her"),

                             BenoitMF("his", "her"),

                             BenoitMF("him", "her"),

                             BenoitMF("him", "her")
                        ));
                    else
                        OutputText("Scenes.Benoit.EggySuggest.EvidentlyExperienceAgain".Localized(
                             BenoitMF("him", "her"),

                             BenoitMF("his", "her"),

                             BenoitMF("he", "she"),

                             BenoitMF("his", "her"),

                             BenoitMF("his", "her"),

                             BenoitMF("him", "her"),

                             BenoitMF("him", "her")
                        ));
                }

                OutputText("Scenes.Benoit.EggySuggest.HandsFurtherInnerThighs".Localized(
                     BenoitMF("His", "Her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she")
                ));

                if (Player.InHeat)
                    OutputText("Scenes.Benoit.EggySuggest.VaginaGushingBodyPheromone".Localized(
                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.EggySuggest.ScentArousalInhalesBreath".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                Player.CuntChange(14, true, true, false);

                OutputText("Scenes.Benoit.EggySuggest.DickIncrediblySlickEase".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her")
                ));

                if (Player.VaginalCapacity() < 15)
                    OutputText("Scenes.Benoit.EggySuggest.SlideBackCooingSeveral".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));
                else
                    OutputText("Scenes.Benoit.EggySuggest.SlideGroinThighsAgain".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.EggySuggest.KeepHoursStoppingStarting".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                if (Player.LactationQ() > 100)
                {
                    OutputText("Scenes.Benoit.EggySuggest.MoanBreastsSpurtsMilk".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("he", "she")
                    ));

                    if (!Player.IsTaur())
                        OutputText("Scenes.Benoit.EggySuggest.HitInTheFace".Localized(
                             BenoitMF("him", "her")
                        ));
                    else
                        OutputText("Scenes.Benoit.EggySuggest.TrickleOverFingers".Localized(
                             BenoitMF("his", "her")
                        ));

                    OutputText("Scenes.Benoit.EggySuggest.MammalsSoDamnWeird".Localized(
                         BenoitMF("he", "she")
                    ));
                }

                OutputText("Scenes.Benoit.EggySuggest.LustRatchetsHandsLosing".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her")
                ));

                if (Player.HasCock())
                {
                    if (!Player.IsTaur())
                        OutputText("Scenes.Benoit.EggySuggest.StuckBetweenBurningBodies".Localized());
                    else
                        OutputText("Scenes.Benoit.EggySuggest.YWord".Localized());

                    OutputText("Scenes.Benoit.EggySuggest.OurSpurtsSympathyFemale".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Benoit.EggySuggest.BothOfYouWord".Localized());
                    else
                        OutputText("Scenes.Benoit.EggySuggest.DryDirtWord".Localized());

                    OutputText("Scenes.Benoit.EggySuggest.WithWhiteWord".Localized());
                }
            }

            OutputText("Scenes.Benoit.EggySuggest.EventuallyPartDrippingFluids".Localized(
                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("he", "she"),

                 BenoitMF("his", "her"),

                 BenoitMF("his", "her"),

                 BenoitMF("him", "her"),

                 BenoitMF("He", "She"),

                 BenoitMF("he", "she"),

                 BenoitMF("he", "she"),

                 BenoitMF("He", "She")
            ));

            OutputText("Scenes.Benoit.EggySuggest.SharkLadiesMeZis".Localized(
                 BenoitMF("he", "she")
            ));

            Player.Orgasm("Generic");

            if ((Player.Pregnancy.Type == PregnancyIds.OvielixirEggs || Player.HasPerk(PerkIds.HarpyWomb) || Player.HasPerk(PerkIds.Oviposition)) && (Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Type == PregnancyIds.OvielixirEggs))
            {
                OutputText("Scenes.Benoit.EggySuggest.NotChildrenTakeMuch".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), TakeBenoitsContraceptives);

                AddButton(14, "Common.Button.Leave".Localized(), DontTakeEggtraceptives);
            }
            else
            {
                OutputText("Scenes.Benoit.EggySuggest.CannotBabiesUnlessEggs".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void TakeBenoitsContraceptives()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.TakeBenoitsContraceptives.GladlyHerbalContraceptive".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DontTakeEggtraceptives()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.DontTakeEggtraceptives.SmileDontLayingEggs".Localized(
                 BenoitMF("he", "she"),

                 BenoitMF("He", "She"),

                 BenoitMF("he", "she"),

                 BenoitMF("he", "she"),

                 BenoitMF("his", "her")
            ));

            OutputText("Scenes.Benoit.DontTakeEggtraceptives.WarmAfterglowRedressLeads".Localized(
                 BenoitMF("him", "her"),

                 BenoitMF("his", "her")
            ));

            BenoitKnocksUpPCCheck();

            DoNext(() => ReturnToCamp(1));
        }

        public void FirstTimeAfterBoningEncounterBenoit()
        {
            ClearOutput();

            BenoitSprite();

            CoC.Instance.Flags.Set(GameFlagIds.BenoitPostFirstfuckTalk, 1);

            OutputText("Scenes.Benoit.FirstTimeAfterBoningEncounterBenoit.StrangeFaintNoiseShop".Localized(
                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.FirstTimeAfterBoningEncounterBenoit.AlloAgainPleasant".Localized(
                 BenoitMF("he", "she"),

                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("He", "She"),

                 BenoitMF("he", "she"),

                 BenoitMF("his", "her"),

                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.FirstTimeAfterBoningEncounterBenoit.ZisNoNavyTie".Localized(
                 BenoitMF("he", "she"),

                 BenoitMF("he", "she")
            ));
        }

        public void RepeatSexWithBenoitLetHim()
        {
            ClearOutput();

            BenoitSprite();

            if (Player.IsTaur())
            {
                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.ForMomentDontAnything".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.SlowGentlePressure".Localized(
                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.SurgesForwardsGraspsButt".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her")
                ));

                if (Player.BiggestBreastSize() >= 1)
                    OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.AndFullChestWord".Localized());

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.AugmentsRoughnessCarriedAlong".Localized(
                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                Player.CuntChange(14, true, true, false);

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.RoughSexKnocksBreath".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.PullsOutTrailingThreads".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.SlidesIntoNoResistance".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("he", "she")
                ));

                if (Player.VaginalCapacity() < 30)
                    OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.DontTightSexFit".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her"),

                         BenoitMF("him", "her")
                    ));
                else
                    OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.AlthoughLongBarelyTouches".Localized(
                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she")
                    ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.DifferencePositionBendAngle".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.FinallyCumsSoundsPained".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.ZatWasWowManages".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her")
                ));
            }
            else
            {
                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.DontDoAnything".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.SlowGentlePressureAgain".Localized(
                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.SurgesForwardGraspsButt".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.RoughKnocksBreathAgain".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.PullsTrailingThreadsAgain".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("his", "her")
                ));

                if (Player.IsBiped())
                    OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.KneesWord".Localized());
                else
                    OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.LowerBodyWord".Localized());

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.HandsDescendPokeOutwards".Localized(
                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.SlidesNoResistanceAgain".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("he", "she")
                ));

                if (Player.VaginalCapacity() < 30)
                    OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.TightSexFit".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her"),

                         BenoitMF("him", "her")
                    ));
                else
                    OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.AlthoughBarelyTouchesAgain".Localized(
                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));

                Player.CuntChange(10, true, true, false);

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.DifferencePositionAngleNeglect".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.FinallySoundsPainedAgain".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatSexWithBenoitLetHim.ZatWowManagesAgain".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her")
                ));
            }

            CoC.Instance.Flags.Set(GameFlagIds.BenoitTimesSexedFempcs, CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) + 1);

            BenoitKnocksUpPCCheck();

            BenoitAffection(2);

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void RepeatBenoitFuckTakeCharge()
        {
            ClearOutput();

            BenoitSprite();

            if (Player.IsTaur())
            {
                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.DontDoAnythingThird".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her")
                ));

                if (Player.IsTaur())
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.HandsDownwardsSmallGrin".Localized(
                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("He", "She"),

                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.HandsUpwardsBestialBehind".Localized(
                     BenoitMF("His", "Her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she")
                ));

                if (Player.InHeat)
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.VaginaGushingPheromoneNeed".Localized(
                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.ScentArousalBreathComes".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.DickIncrediblyIncredibleEase".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her")
                ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.ParagraphBreak".Localized());

                if (Player.VaginalCapacity() < 30)
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.BottomOutCooing".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));
                else
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.GroinMuscledThighs".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.KeepAtSlowRide".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her")
                ));

                Player.CuntChange(14, true, true, false);

                if (Player.LactationQ() >= 50)
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.SlowSensualBeadMilk".Localized(
                         BenoitMF("Benoit", "Benoite"),

                         BenoitMF("he", "she")
                    ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.LustRatchetsWarmHands".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her")
                ));

                if (Player.HasCock())
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.EachThickensSympathyHigh".Localized());

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.PuddlePackedEarthCuddle".Localized());

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.SaysEventuallyBarelyAbove".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She")
                ));
            }
            else
            {
                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.DontDoAnythingFourth".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her")
                ));

                if (Player.HasCock())
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.HandsTravelDownwards".Localized(
                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("He", "She"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("him", "her")
                    ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.HandsFurtherInnerThighs".Localized(
                     BenoitMF("His", "Her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she")
                ));

                if (Player.InHeat)
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.VaginaGushingBodyPheromone".Localized(
                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.ScentArousalInhalesBreath".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her")
                ));

                Player.CuntChange(10, true, true, false);

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.DickIncrediblySlickEase".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her")
                ));

                if (Player.VaginalCapacity() < 30)
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.BottomOutCooingAgain".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("His", "Her"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));
                else
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.GroinThighsAgain".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.KeepAtThisHours".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                if (Player.LactationQ() > 100)
                {
                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.MoanBreastsSpurtsMilk".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("he", "she")
                    ));

                    if (!Player.IsTaur())
                        OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.HitInTheFace".Localized(
                             BenoitMF("him", "her")
                        ));
                    else
                        OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.TrickleOverFingers".Localized(
                             BenoitMF("his", "her")
                        ));

                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.MammalsSoDamnWeird".Localized(
                         BenoitMF("he", "she")
                    ));
                }

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.LustRatchetsHandsAgain".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her")
                ));

                if (Player.HasCock())
                {
                    if (!Player.IsTaur())
                        OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.StuckBetweenBurningBodies".Localized());
                    else
                        OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.YWord".Localized());

                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.OurSpurtsSympathyFemale".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.BothOfYouWord".Localized());
                    else
                        OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.DryDirtWord".Localized());

                    OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.WithWhiteWord".Localized());
                }

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.PuddlePackedCuddleAgain".Localized());

                OutputText("Scenes.Benoit.RepeatBenoitFuckTakeCharge.VoiceBarelyAbove".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She")
                ));
            }

            BenoitKnocksUpPCCheck();

            BenoitAffection(2);

            CoC.Instance.Flags.Set(GameFlagIds.BenoitTimesSexedFempcs, CoC.Instance.Flags.Get(GameFlagIds.BenoitTimesSexedFempcs) + 1);

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void EquipUnequipHairPin()
        {
            ClearOutput();

            var keyItemNum = Player.FindKeyItem("Feathery hair-pin");

            if (keyItemNum < 0)
                return ;

            if (Player.KeyItemV1("Feathery hair-pin") > 0)
            {
                if (Player.Body.Get<HairPart>().Length > 0)
                    OutputText("Scenes.Benoit.EquipUnequipHairPin.TakeFeatheryPinInventory".Localized(
                         BenoitMF("Benoit", "Benoite"),

                         Player.HairDescript()
                    ));
                else
                    OutputText("Scenes.Benoit.EquipUnequipHairPin.RemoveFeatheryBaldHead".Localized(
                         BenoitMF("Benoit", "Benoite")
                    ));

                Player.GetKeyItem("Feathery hair-pin").Values[0] = 0;

                Player.GetKeyItem("Feathery hair-pin").Values[1] = 0;
            }
            else
                if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Goo)
                    OutputText("Scenes.Benoit.EquipUnequipHairPin.TrySlideSemiLiquid".Localized());
                else
                    if (Player.Corruption >= 55)
                        OutputText("Scenes.Benoit.EquipUnequipHairPin.GoSlideHeatsScalp".Localized());
                    else
                    {
                        if (Player.Body.Get<HairPart>().Length > 0)
                            OutputText("Scenes.Benoit.EquipUnequipHairPin.SlidePinAdmiringMirror".Localized(
                                 BenoitMF("Benoit", "Benoite"),

                                 Player.HairDescript()
                            ));
                        else
                            OutputText("Scenes.Benoit.EquipUnequipHairPin.TakeFeatheryPin".Localized(
                                 BenoitMF("Benoit", "Benoite")
                            ));

                        Player.GetKeyItem("Feathery hair-pin").Values[0] = 1;

                        Player.GetKeyItem("Feathery hair-pin").Values[1] = 1;
                    }

            OutputText("Scenes.Benoit.EquipUnequipHairPin.ParagraphBreak".Localized());

        }

        public void ConvertToBassyEyes()
        {
            ClearOutput();

            int eyesGranted = CoC.Instance.Flags.Get(GameFlagIds.BenoitBasilEyesGranted) + 1;

            BenoitSprite();

            if (eyesGranted <= 1)
            {
                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.TellWeighedProsCons".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.FoundWayTransformEyes".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.BlindBasiliskAfterBottle".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.OpefullyBirthedSoMany".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.FetchClothCoverCut".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.GoingToDrink".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.SwiftNodGrabDrain".Localized());

                DoNext(() => ConvertToBassyEyesPageTwo());

                return ;
            }
            else
            {
                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.TellYouveLostEyes".Localized());

                if (eyesGranted == 2)
                    OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.ShallMakeZePotion".Localized());
                else
                    if (eyesGranted >= 3 && eyesGranted < 6)
                        OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.ShallMakePotionAgain".Localized());
                    else
                        OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.LostZemAgainCareful".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.PadsOverSectionBottles".Localized());

                if (eyesGranted >= 6)
                    OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.GrumblingAsDoes".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.SentencePeriod".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.HereWeGoTime".Localized(
                     Utils.Utils.Num2TextOrdinal(eyesGranted)
                ));

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.Text.Three".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.WithinMinutesBowlSlid".Localized());

                OutputText("Scenes.Benoit.ConvertToBassyVanillaBodyPartTypes.WakeOpenEyesSlowly".Localized());
            }

            ConvertToBassyEyesFinal();
        }

        public void ConvertToBassyEyesPageTwo()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.ConvertToBassyEyesPageTwo.WakeCryOutBright".Localized());

            OutputText("Scenes.Benoit.ConvertToBassyEyesPageTwo.WhatIzItHurt".Localized());

            OutputText("Scenes.Benoit.ConvertToBassyEyesPageTwo.WellWord".Localized());

            OutputText("Scenes.Benoit.ConvertToBassyEyesPageTwo.EyesWidenMirrorReptilian".Localized());

            OutputText("Scenes.Benoit.ConvertToBassyEyesPageTwo.SmilePutMirrorWorked".Localized((noFur ? "Common.Text.Nose".Localized() : "Common.Text.ScaledSnout".Localized()), (noFur ? "Common.Text.Cheeks".Localized() : "Common.Text.Scales".Localized())));

            OutputText("Scenes.Benoit.ConvertToBassyEyesPageTwo.NussingGiftForYou".Localized());

            ConvertToBassyEyesFinal();
        }

        public void ConvertToBassyEyesFinal()
        {

            BenoitSprite();

            Player.Body.Get<EyesPart>().Id = BodyPartTypes.Basilisk;

            OutputText("Scenes.Benoit.ConvertToBassyEyesFinal.EyesNowBasiliskEyes".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.BenoitBasilEyesGranted, CoC.Instance.Flags.Get(GameFlagIds.BenoitBasilEyesGranted) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void TryToConvertToBassyWomb()
        {
            ClearOutput();

            BenoitSprite();

            if (!(Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir), 2) && Player.HasItem(ConsumableIds.Reptlum) && Player.HasItem(ItemRegistry.Create(ConsumableIds.GoblinAle))))
            {
                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.DontHaveNecessaryIngredients".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                BenoitIntro();
            }
            else
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.OviElixir), 2);

                Player.ConsumeItem(ConsumableIds.Reptlum);

                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.GoblinAle));

                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.FerretIngredientsBangCounter".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.WhatIsZatBewildered".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.SunTooLong".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.MixtureLingeringBiteMint".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                if (!Player.HasPerk(PerkIds.Oviposition))
                    OutputText("Scenes.Benoit.TryToConvertToBassyWomb.SlightlyBloatedOtherwiseFine".Localized());
                else
                    OutputText("Scenes.Benoit.TryToConvertToBassyWomb.SlightlyBloatedWereImpregnated".Localized());

                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.SaysNervouslyRightSawbones".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her")
                ));

                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.OffFeetAroundCounter".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("He", "She")
                ));

                CoC.Instance.Flags.Set(GameFlagIds.BenoitTestedBasiliskWomb, .5f);

                if (Player.Lust >= 33)
                {
                    OutputText("Scenes.Benoit.TryToConvertToBassyWomb.GrinNotSureWorked".Localized());

                    if (Player.Tallness <= 78 && !Player.IsTaur())
                        OutputText("Scenes.Benoit.TryToConvertToBassyWomb.BasiliskStillSuddenSurge".Localized(
                             BenoitMF("his", "her"),

                             BenoitMF("he", "she"),

                             BenoitMF("he", "she"),

                             BenoitMF("his", "her"),

                             BenoitMF("he", "she")
                        ));
                    else
                        OutputText("Scenes.Benoit.TryToConvertToBassyWomb.BasiliskStillSurgeAgain".Localized(
                             BenoitMF("his", "her"),

                             BenoitMF("him", "her"),

                             BenoitMF("he", "she"),

                             BenoitMF("him", "her")
                        ));

                    SuggestSexAfterBasiWombed(false);
                }
                else
                {
                    OutputText("Scenes.Benoit.TryToConvertToBassyWomb.GrinNotSureAgain".Localized(
                         BenoitMF("he", "she"),

                         BenoitMF("his", "her")
                    ));

                    DoNext(() => ReturnToCamp(1));
                }

                Player.CreatePerk(PerkIds.BasiliskWomb, 0, 0, 0, 0);

                OutputText("Scenes.Benoit.TryToConvertToBassyWomb.PerkUnlockedBasiliskWomb".Localized());

                if (!Player.HasPerk(PerkIds.Oviposition))
                {
                    Player.CreatePerk(PerkIds.Oviposition, 0, 0, 0, 0);

                    OutputText("Scenes.Benoit.TryToConvertToBassyWomb.PerkUnlockedOviposition".Localized());
                }
            }
        }

        public void SuggestSexAfterBasiWombed(bool later = true)
        {
            ClearOutput();

            BenoitSprite();

            if (later)
            {
                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.SmileCoquettishlyTestDrive".Localized(
                     BenoitMF("he", "she")
                ));

                if (Player.Tallness <= 78 && !Player.IsTaur())
                    OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.BasiliskStillSuddenSurge".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("he", "she"),

                         BenoitMF("he", "she"),

                         BenoitMF("his", "her"),

                         BenoitMF("he", "she")
                    ));
                else
                    OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.BasiliskStillSurgeAgain".Localized(
                         BenoitMF("his", "her"),

                         BenoitMF("him", "her"),

                         BenoitMF("he", "she"),

                         BenoitMF("him", "her")
                    ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.ParagraphBreak".Localized());
            }

            if (!Player.IsTaur())
            {
                if (Player.Tallness > 78)
                    OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.HaulLizardBoyBackroom".Localized(
                         BenoitMF("him", "her"),

                         BenoitMF("Benoit", "Benoite"),

                         BenoitMF("his", "her"),

                         BenoitMF("he", "she")
                    ));
                else
                    OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.StaggersBackRoomBumping".Localized(
                         BenoitMF("Benoit", "Benoite"),

                         BenoitMF("he", "she"),

                         BenoitMF("He", "She"),

                         BenoitMF("his", "her"),

                         BenoitMF("he", "she"),

                         BenoitMF("him", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.NeedsNoFurtherInvitation".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                if (Player.HasCock())
                    OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.RubsUpAndDown".Localized(
                         BenoitMF("He", "She")
                    ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.DespiteBlindnessSlidesDepths".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                Player.CuntChange(14, true, true, false);

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.CannotStopScreamingOrgasm".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.AfterMinutesLockedOrgasm".Localized());

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.SaysEventuallyVoiceLow".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.HavingMilkedEntirelyDry".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.BringZeEggsHuskily".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));
            }
            else
            {
                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.HaulLizardBackroomGently".Localized(
                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.NeedsNoInvitationAgain".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                if (Player.HasCock())
                    OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.DeliberatelyMovesOneCock".Localized(
                         BenoitMF("He", "She"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her"),

                         BenoitMF("his", "her")
                    ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.SlidesIntoDepths".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("he", "she")
                ));

                Player.CuntChange(14, true, true, false);

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.CannotStopScreamingAgain".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.AfterSeemsHoursOrgasm".Localized());

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.VoiceSoLowAgain".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.HavingEntirelyDryAgain".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.SuggestSexAfterBasiWombed.BringZeEggsAgain".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her")
                ));
            }

            CoC.Instance.Flags.Set(GameFlagIds.BenoitTestedBasiliskWomb, 1);

            BenoitKnocksUpPCCheck();

            if (Player.Pregnancy.Type == PregnancyIds.Basilisk)
                Player.KnockUp(PregnancyIds.Benoit, Player.Pregnancy.Incubation, 100, 1);

            DoNext(() => ReturnToCamp(1));

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));
        }

        public void PopOutBenoitEggs()
        {

            if (Player.Vaginas.Count == 0)
            {
                OutputText("Scenes.Benoit.PopOutBenoitEggs.TerriblePressureGroinPain".Localized());

                Player.CreateVagina();
            }

            OutputText("Scenes.Benoit.PopOutBenoitEggs.SuddenPressureBellyRouses".Localized());

            OutputText("Scenes.Benoit.PopOutBenoitEggs.ThickGreenSlimeNetherlips".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Benoit.PopOutBenoitEggs.EachCockPreCum".Localized());

            OutputText("Scenes.Benoit.PopOutBenoitEggs.BeingMovedClimaxBirthing".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Benoit.PopOutBenoitEggs.AndDownpourOfSemen".Localized());

            OutputText("Scenes.Benoit.PopOutBenoitEggs.SentencePeriod".Localized());

            OutputText("Scenes.Benoit.PopOutBenoitEggs.AbleStandExamineBirthed".Localized(
                 NumberUtils.Num2Text((float)(int)Math.Floor(Player.TotalFertility() / 10)),

                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.PopOutBenoitEggs.LightDayBreaksClutch".Localized(
                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("his", "her"),

                 BenoitMF("him", "her")
            ));

            OutputText("Scenes.Benoit.PopOutBenoitEggs.SnarlsDisplayingFangs".Localized(
                 BenoitMF("he", "she"),

                 BenoitMF("his", "her"),

                 BenoitMF("he", "she"),

                 BenoitMF("his", "her"),

                 BenoitMF("He", "She"),

                 BenoitMF("his", "her"),

                 BenoitMF("he", "she")
            ));

            OutputText("Scenes.Benoit.PopOutBenoitEggs.SmileBlindLoverTell".Localized(
                 BenoitMF("him", "her"),

                 BenoitMF("he", "she"),

                 BenoitMF("father", "mother")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) > 0)
                OutputText("Scenes.Benoit.PopOutBenoitEggs.OnceMoreWord".Localized());

            OutputText("Scenes.Benoit.PopOutBenoitEggs.WellWillBeWhen".Localized(
                 BenoitMF("he", "she")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) > 0)
                OutputText("Scenes.Benoit.PopOutBenoitEggs.LatestWord".Localized());

            OutputText("Scenes.Benoit.PopOutBenoitEggs.ClutchHatchesAnyway".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) == 0)
            {
                OutputText("Scenes.Benoit.PopOutBenoitEggs.BeamsJoyThenConfused".Localized(
                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.PopOutBenoitEggs.ExplainCampNotSafe".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.PopOutBenoitEggs.OpensDoorOffersHand".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.PopOutBenoitEggs.SpendTimeArrangingSays".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.PopOutBenoitEggs.SaysDifferentDaughters".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.PopOutBenoitEggs.DoneMyPeopleService".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.PopOutBenoitEggs.LingerBreakfastReturnCamp".Localized(
                     BenoitMF("him", "her")
                ));
            }
            else
                OutputText("Scenes.Benoit.PopOutBenoitEggs.SmilesProudlyCannotZank".Localized());

            Player.Orgasm("Vaginal");

            Player.ClearPregnancy();

            CoC.Instance.Flags.Set(GameFlagIds.BenoitEggs, CoC.Instance.Flags.Get(GameFlagIds.BenoitEggs) + (int)Math.Floor(Player.TotalFertility() / 10));
        }

        public void FemoitInitialTalk()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.FemoitInitialTalk.AskThoughtHelpPeople".Localized(
                 BenoitMF("Benoit", "Benoite"),

                 BenoitMF("he", "she"),

                 BenoitMF("his", "her")
            ));

            OutputText("Scenes.Benoit.FemoitInitialTalk.SilentClawsCounterFemale".Localized(
                 BenoitMF("his", "her"),

                 BenoitMF("he", "she"),

                 BenoitMF("he", "she")
            ));

            if (Player.Intelligence >= 60)
            {
                OutputText("Scenes.Benoit.FemoitInitialTalk.ReckonResistantOppositeSex".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.FemoitInitialTalk.WellDoubleDosePurified".Localized());

                OutputText("Scenes.Benoit.FemoitInitialTalk.FeminizeOptionAddedMenu".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                CoC.Instance.Flags.Set(GameFlagIds.FemoitUnlocked, 1);
            }
            else
                OutputText("Scenes.Benoit.FemoitInitialTalk.RackCantThinkHelp".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her")
                ));
        }

        public void BenoitFeminise()
        {
            ClearOutput();

            BenoitSprite();

            if (!Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk), 2) || !Player.HasItem(ItemRegistry.Create(ConsumableIds.EggLargePink)) || !Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)) || !Player.HasItem(ConsumableIds.Reptlum))
            {
                OutputText("Scenes.Benoit.BenoitFeminise.DontNecessaryIngredientsFemale".Localized());

                OutputText("Scenes.Benoit.BenoitFeminise.RequiresPurifiedSuccubusMilk".Localized());
            }
            else
            {
                Player.DestroyItems(ConsumableIds.PurifiedSuccubiMilk, 2);

                Player.DestroyItems(ConsumableIds.EggLargePink, 1);

                Player.DestroyItems(ConsumableIds.OviElixir, 1);

                Player.DestroyItems(ConsumableIds.Reptlum, 1);

                OutputText("Scenes.Benoit.BenoitFeminise.FerretIngredientsCounterFemale".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.BenoitFeminise.IWhatJustFantastic".Localized(
                     BenoitMF("He", "She"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.BenoitFeminise.AndSinkActuallyWork".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("him", "her"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she")
                ));

                OutputText("Scenes.Benoit.BenoitFeminise.WatchPotionSlidesDown".Localized(
                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.BenoitFeminise.NotWorstEverTasted".Localized(
                     BenoitMF("he", "she"),

                     BenoitMF("He", "She"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("he", "she"),

                     BenoitMF("his", "her"),

                     BenoitMF("His", "Her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("his", "her"),

                     BenoitMF("he", "she"),

                     BenoitMF("Benoit", "Benoite")
                ));

                OutputText("Scenes.Benoit.BenoitFeminise.ZutFuckingVoiceGone".Localized(
                     BenoitMF("Benoit", "Benoite"),

                     BenoitMF("his", "her")
                ));

                OutputText("Scenes.Benoit.BenoitFeminise.CouldBackTomorrowUnevenly".Localized(
                     BenoitMF("Benoit", "Benoite")
                ));

                CoC.Instance.Flags.Set(GameFlagIds.FemoitNextdayEvent, View.Time.Days + 1);

                CoC.Instance.Flags.Set(GameFlagIds.FemoitNextdayEventDone, 1);

                ClearButtons();

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void FemoitNextDayEvent()
        {
            ClearOutput();

            BenoitSprite();

            CoC.Instance.Flags.Set(GameFlagIds.BenoitStatus, 1);

            CoC.Instance.Flags.Set(GameFlagIds.FemoitNextdayEventDone, 2);

            OutputText("Scenes.Benoit.FemoitNextDayEvent.StrangeFaintSoundShop".Localized(
                 BenoitMF("Benoit", "Benoite")
            ));

            OutputText("Scenes.Benoit.FemoitNextDayEvent.DoNotStandingAround".Localized());

            OutputText("Scenes.Benoit.FemoitNextDayEvent.CallBenoiteEasyAdapt".Localized());

            OutputText("Scenes.Benoit.FemoitNextDayEvent.LeansAcrossCounterFading".Localized());

            OutputText("Scenes.Benoit.FemoitNextDayEvent.SoundsChokedStopsMoment".Localized());
        }

        public void FemoitFirstTimeNo()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.FemoitFirstTimeNo.LetHerDownKindly".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeNo.NoYouRightCasual".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeNo.CatchingDisappointmentFarewell".Localized());

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void FemoitFirstTimeYes()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedFemoit, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFemoit) + 1);

            BenoitSprite();

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.SmilingReachSqueezeHands".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.BackOfficePerfectPurposes".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.ZisWeirdDropGarment".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.ZisSoundStrangeShivery".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersTravelBody".Localized());
            else
                if (Player.IsDrider())
                    OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodyAgain".Localized());
                else
                    if (Player.DemonScore() > 4)
                        OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodyThird".Localized());
                    else
                        if (Player.DogScore() >= 4 && Player.Ears.Id == BodyPartTypes.Dog && Player.Body.Get<TailPart>().Id == BodyPartTypes.Dog)
                            OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodyFourth".Localized());
                        else
                            if ((Player.BunnyScore() >= 4 && Player.Ears.Id == BodyPartTypes.Bunny && Player.Body.Get<TailPart>().Id == BodyPartTypes.Rabbit) || (Player.CatScore() >= 4 && Player.Ears.Id == BodyPartTypes.Cat && Player.Body.Get<TailPart>().Id == BodyPartTypes.Cat))
                                OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodyFifth".Localized());
                            else
                                if (Player.HarpyScore() >= 4 && Player.Body.Get<WingsPart>().Id != BodyPartTypes.None && Player.Body.Get<ArmsPart>().Id == BodyPartTypes.Harpy)
                                    OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodySixth".Localized());
                                else
                                    if (Player.BeeScore() >= 4 && Player.Body.Get<WingsPart>().Id != BodyPartTypes.None && Player.Body.Get<LowerBodyPart>().Id == BodyPartTypes.Bee)
                                        OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodySeventh".Localized());
                                    else
                                        if (Player.GooScore() >= 4 && Player.HasGooSkin())
                                            OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodyEighth".Localized());
                                        else
                                            if (Player.HasScales())
                                                OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodyNinth".Localized());
                                            else
                                                if ((Player.FoxScore() >= 4 || Player.KitsuneScore() >= 4) && Player.Ears.Id == BodyPartTypes.Fox && Player.Body.Get<TailPart>().Id == BodyPartTypes.Fox)
                                                {
                                                    if (Player.Body.Get<TailPart>().Venom <= 1)
                                                        OutputText("Scenes.Benoit.FemoitFirstTimeYes.WarmFingersTravelBody".Localized(
                                                             BenoitMF("His", "Her"),

                                                             BenoitMF("He", "She"),

                                                             BenoitMF("he", "she"),

                                                             BenoitMF("he", "she"),

                                                             BenoitMF("he laughs", "she giggles")
                                                        ));
                                                    else
                                                        OutputText("Scenes.Benoit.FemoitFirstTimeYes.WarmFingersBodyAgain".Localized(
                                                             BenoitMF("His", "Her"),

                                                             BenoitMF("He", "She"),

                                                             BenoitMF("he", "she"),

                                                             BenoitMF("he", "she"),

                                                             BenoitMF("he laughs", "she giggles")
                                                        ));
                                                }
                                                else
                                                    OutputText("Scenes.Benoit.FemoitFirstTimeYes.HerFingersBodyTenth".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.HandsTravelTorso".Localized());

            if (Player.BiggestCockLength() <= 10)
                OutputText("Scenes.Benoit.FemoitFirstTimeYes.AlthoughUncannySmileHer".Localized((CoC.Instance.Flags.Get(GameFlagIds.BenoitStatus) < 3 ? "Common.Text.Was".Localized() : "Common.Text.Is".Localized())));
            else
                OutputText("Scenes.Benoit.FemoitFirstTimeYes.EvidentlyUncannyHerAlien".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.ScentArousalInhalesHer".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.PussyVirginallyTightSlowly".Localized());

            if (Player.BiggestCockLength() < 15 && Player.Balls == 0)
                OutputText("Scenes.Benoit.FemoitFirstTimeYes.EventuallyHiltEntirelyDepths".Localized());
            else
                if (Player.BiggestCockLength() < 15 && Player.Balls > 0)
                    OutputText("Scenes.Benoit.FemoitFirstTimeYes.EventuallyHiltDepthsBalls".Localized());
                else
                    if (Player.BiggestCockLength() >= 15 && Player.Balls == 0)
                        OutputText("Scenes.Benoit.FemoitFirstTimeYes.EventuallyBottomOutCervix".Localized());
                    else
                        if (Player.BiggestCockLength() >= 15 && Player.Balls > 0)
                            OutputText("Scenes.Benoit.FemoitFirstTimeYes.EventuallyBottomCervixBalls".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.StayingLikeThatWithdraw".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.SlowlyGetsItThawing".Localized());

            if (Player.IsLactating())
                OutputText("Scenes.Benoit.FemoitFirstTimeYes.PressureArousalNipplesDribble".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.SoonThrashingBatter".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.HowlsOrgasmsClutching".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.PulseDropLowerBenoite".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.PhewManagedCatchBreath".Localized());

            OutputText("Scenes.Benoit.FemoitFirstTimeYes.SankYouHuskily".Localized());

            Player.Orgasm("Dick");

            BenoitKnockUp();

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void FemoitSexIntro()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.FemoitSexIntro.OnceAgainHandLead".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedFemoit, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFemoit) + 1);

            if (!BenoitPreggers())
            {
                OutputText("Scenes.Benoit.FemoitSexIntro.FingersTravelSidesApron".Localized());

                if (BenoitInClutch())
                    OutputText("Scenes.Benoit.FemoitSexIntro.StomachBulgesClutchMusk".Localized());

                OutputText("Scenes.Benoit.FemoitSexIntro.CupBehindWetOpening".Localized());

                OutputText("Scenes.Benoit.FemoitSexIntro.ParagraphBreak".Localized());

                if (Player.BiggestCockLength() < 15)
                    OutputText("Scenes.Benoit.FemoitSexIntro.SoonHiltingDepthsSlap".Localized());
                else
                    OutputText("Scenes.Benoit.FemoitSexIntro.SoonBottomingOutHulking".Localized());

                OutputText("Scenes.Benoit.FemoitSexIntro.QuicklyPickPaceRut".Localized());

                OutputText("Scenes.Benoit.FemoitSexIntro.ParagraphBreakAgain".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Benoit.FemoitSexIntro.SweatDrippingGrabThighs".Localized());

                    if (!BenoitInClutch())
                        OutputText("Scenes.Benoit.FemoitSexIntro.StomachBeatingSlappingFlat".Localized());
                    else
                        OutputText("Scenes.Benoit.FemoitSexIntro.StomachBeatingSlappingGravid".Localized());
                }

                OutputText("Scenes.Benoit.FemoitSexIntro.MoansSquealsScreamsFluids".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Benoit.FemoitSexIntro.TumbleOverPeakTightens".Localized());

                if (Player.CumQ() >= 2500)
                    OutputText("Scenes.Benoit.FemoitSexIntro.QuantityDribblesBackOut".Localized());

                OutputText("Scenes.Benoit.FemoitSexIntro.BothRodeOutLie".Localized());

                OutputText("Scenes.Benoit.FemoitSexIntro.BigZillyStudFondly".Localized());

                BenoitKnockUp();

                Player.Orgasm("Dick");
            }
            else
                if (BenoitRegularPreggers() && (!Player.IsTaur() || (Player.IsTaur() && (Player.Tallness * (5 / 6) < Player.Cocks[Player.BiggestCockIndex()].Length))))
                {
                    OutputText("Scenes.Benoit.FemoitSexIntro.BothUsualSpotUndressing".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.UseOpportunityExamineClosely".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.EnoughForeplaySoughtFuck".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.ZatInMindKinky".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.CupButtocksSqueezingGlobes".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.PromisesPromisesCheeky".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.EnoughZeTeasingBarks".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.HadEnoughForeplayPosition".Localized());

                    if (Player.BiggestCockLength() < 15)
                        OutputText("Scenes.Benoit.FemoitSexIntro.SoonHiltingSlapAgain".Localized());
                    else
                        OutputText("Scenes.Benoit.FemoitSexIntro.SoonBottomingHulkingAgain".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.MomentaryPauseReposition".Localized((noFur ? "Common.Text.Skin".Localized() : "Common.Text.Scales".Localized())));

                    OutputText("Scenes.Benoit.FemoitSexIntro.YezYezGoodSink".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.NotDoneYetEnticing".Localized());

                    if (Player.CumQ() >= 2500)
                        OutputText("Scenes.Benoit.FemoitSexIntro.WombPackedEggsOozes".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.NowTurnSlumpSpent".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.StirsFirstMmmPregnant".Localized());

                    OutputText("Scenes.Benoit.FemoitSexIntro.RedressHandFrontShop".Localized());

                    Player.Orgasm("Dick");
                }
                else
                    if (BenoitVeryHeavyPreggers() || BenoitExtremePreggers())
                    {
                        BenoitSprite();

                        if (Player.IsTaur())
                        {
                            OutputText("Scenes.Benoit.FemoitSexIntro.NoNonAbsolutelyNon".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.PlacesHandWorkingFace".Localized());
                        }
                        else
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitSpooned) == 0)
                            {
                                CoC.Instance.Flags.Set(GameFlagIds.FemoitSpooned, CoC.Instance.Flags.Get(GameFlagIds.FemoitSpooned) + 1);

                                OutputText("Scenes.Benoit.FemoitSexIntro.SeemsPeerClawedBelly".Localized());

                                OutputText("Scenes.Benoit.FemoitSexIntro.AssurePositionWillWork".Localized());

                                OutputText("Scenes.Benoit.FemoitSexIntro.EggLadenThinks".Localized());
                            }
                            else
                                OutputText("Scenes.Benoit.FemoitSexIntro.SmirksGuessLastTime".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.TurnsAroundWaddlesPrivate".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.FortunatelySleepsInShop".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.StartsShockArmsWrap".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.HugeSolidRockSkin".Localized((noFur ? "Common.Text.Smooth".Localized() : "Common.Text.Scaly".Localized())));

                            OutputText("Scenes.Benoit.FemoitSexIntro.FeignOffencePride".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.HandCirclesMoveOther".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.CockUnderAss".Localized());

                            if (Player.BiggestCockLength() < 15)
                                OutputText("Scenes.Benoit.FemoitSexIntro.SoonHiltingSlapThird".Localized());
                            else
                                OutputText("Scenes.Benoit.FemoitSexIntro.SoonBottomingHulkingThird".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.SurprisingLanguidnessRockBack".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.SeeingNotGettingAnywhere".Localized());

                            if (Player.Balls == 0)
                                OutputText("Scenes.Benoit.FemoitSexIntro.WithKnowingDamnWell".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.SentencePeriod".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.CantAnymoreFinalThrust".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.ZatWhatTalkingAbout".Localized());

                            OutputText("Scenes.Benoit.FemoitSexIntro.DoesntComeToThat".Localized());

                            Player.Orgasm("Dick");
                        }
                    }

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void FemoitBirths()
        {
            ClearOutput();

            BenoitSprite();

            OutputText("Scenes.Benoit.FemoitBirths.EnterStallGaspsPain".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.HowlPainClawsDigging".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitHelpedLay) == 0)
            {
                OutputText("Scenes.Benoit.FemoitBirths.AskThinkAnyWay".Localized());

                OutputText("Scenes.Benoit.FemoitBirths.CatchEggsStopHurting".Localized());
            }
            else
            {
                OutputText("Scenes.Benoit.FemoitBirths.RememberingWhatYouBefore".Localized());

                OutputText("Scenes.Benoit.FemoitBirths.LookingAroundScrapClothing".Localized());

                if (BenoitVeryHeavyPreggers() || BenoitExtremePreggers())
                    OutputText("Scenes.Benoit.FemoitBirths.GoodLotOfThem".Localized());

                OutputText("Scenes.Benoit.FemoitBirths.ThatDoneSquatBehind".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.FemoitHelpedLay) == 1)
                {
                    OutputText("Scenes.Benoit.FemoitBirths.BlurtAlarmed".Localized());

                    OutputText("Scenes.Benoit.FemoitBirths.ColdAndsSquealsDispleasure".Localized());
                }
                else
                    OutputText("Scenes.Benoit.FemoitBirths.WhatWarmingZoseHands".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.FemoitHelpedLay, CoC.Instance.Flags.Get(GameFlagIds.FemoitHelpedLay) + 1);

            OutputText("Scenes.Benoit.FemoitBirths.ApologizeDontHandsAway".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.AtBasilisksLayEggs".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.EncourageBreatheDeeply".Localized());

            if (silly)
                OutputText("Scenes.Benoit.FemoitBirths.ShapedLikeRhomboidalPolygon".Localized());
            else
                OutputText("Scenes.Benoit.FemoitBirths.SmoothlyCurvedJadeEgg".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.StrangledCryOrgasmRelief".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) >= 2)
            {
                OutputText("Scenes.Benoit.FemoitBirths.ContinuedDistensionClutch".Localized());

                if (BenoitRegularPreggers())
                    OutputText("Scenes.Benoit.FemoitBirths.LaborsOverQuicklyClutch".Localized(
                         NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs))
                    ));
                else
                    if (BenoitHeavyPreggers())
                        OutputText("Scenes.Benoit.FemoitBirths.ThanksShapeEggsDilated".Localized(
                             NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs))
                        ));
                    else
                        if (BenoitVeryHeavyPreggers())
                            OutputText("Scenes.Benoit.FemoitBirths.GladGivingBirthEasier".Localized(
                                 NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs))
                            ));
                        else
                            if (BenoitExtremePreggers())
                                OutputText("Scenes.Benoit.FemoitBirths.GroansMoansDyingSoldier".Localized(
                                     NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs))
                                ));
            }

            OutputText("Scenes.Benoit.FemoitBirths.LayingDoneHeavesSigh".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) > 1)
                OutputText("Scenes.Benoit.FemoitBirths.PluralS".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.JustLaidGiveHug".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.FlatteryHuskyReptilianBlush".Localized());

            if (BenoitRegularPreggers())
                OutputText("Scenes.Benoit.FemoitBirths.SighsSoftlyClutchNormal".Localized());
            else
                if (BenoitHeavyPreggers())
                    OutputText("Scenes.Benoit.FemoitBirths.SmilesProudlyBigClutch".Localized());
                else
                    if (BenoitVeryHeavyPreggers())
                        OutputText("Scenes.Benoit.FemoitBirths.WowQuiteStudThroaty".Localized());
                    else
                        if (BenoitExtremePreggers())
                            OutputText("Scenes.Benoit.FemoitBirths.StopsAfterCountingStunned".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.AskWillAlrightNow".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.NotFragileMoveLike".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.InsistHelpingPutEgg".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.FemoitEggs) > 1)
                OutputText("Scenes.Benoit.FemoitBirths.PluralSAgain".Localized());

            OutputText("Scenes.Benoit.FemoitBirths.AwaySafelyAppreciatesHelp".Localized());

            ClearBenoitPreggers();

            ClearButtons();

            DoNext(() => ReturnToCamp(1));
        }

        public void BenoitHerminise()
        {
            ClearOutput();

            BenoitSprite();

            if (!Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft), 2) || !Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubusDelight)) || !Player.HasItem(ConsumableIds.Reptlum))
            {
                OutputText("Scenes.Benoit.BenoitHerminise.DontNecessaryIngredientsHerm".Localized());

                OutputText("Scenes.Benoit.BenoitHerminise.RequiresPurifiedIncubiDraft".Localized());
            }
            else
            {
                Player.DestroyItems(ConsumableIds.PurifiedIncubiDraft, 2);

                Player.DestroyItems(ConsumableIds.PurifiedSuccubusDelight, 1);

                Player.DestroyItems(ConsumableIds.Reptlum, 1);

                OutputText("Scenes.Benoit.BenoitHerminise.TellCouldNewPenis".Localized());

                OutputText("Scenes.Benoit.BenoitHerminise.FerretIngredientsCounterHerm".Localized());

                OutputText("Scenes.Benoit.BenoitHerminise.GoodIdeaLayEggs".Localized());

                OutputText("Scenes.Benoit.BenoitHerminise.InstructDrinkConcoction".Localized());

                OutputText("Scenes.Benoit.BenoitHerminise.AssureCanDoSomeday".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.BenoitStatus, 3);

                ClearButtons();

                DoNext(() => ReturnToCamp(1));
            }
        }

    }
}