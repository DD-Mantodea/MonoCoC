using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Utils;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<LumiScene>("coc:lumi")]
    public class LumiScene : Scene
    {

        public override void Entry()
        {
            LumiEncounter();
        }

        public string InchesOrCentimetres(float inches)
        {

            return MeasurementsUtils.InchesOrCentimetres(inches);
        }

        public void LumiEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LumiMet) == 0)
            {
                ShowImage("event-lumi");

                OutputText("Scenes.Lumi.LumiEncounter.SpotsAnomaly".Localized());

                OutputText("Scenes.Lumi.LumiEncounter.OpenPrompt".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), LumiLabChoices);

                AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Lumi.LumiEncounter.SpotsDoor".Localized());

                LumiLabChoices();
            }
        }

        public void LumiLabChoices()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.LumiMet) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.LumiMet, CoC.Instance.Flags.Get(GameFlagIds.LumiMet) + 1);

                ShowImage("encounter-lumi");

                OutputText("Scenes.Lumi.LumiLabChoices.OpensDoor".Localized());

                OutputText("Scenes.Lumi.LumiLabChoices.GoblinHeight".Localized());

                OutputText("Scenes.Lumi.LumiLabChoices.LumiGreeting".Localized());

                OutputText("Scenes.Lumi.LumiLabChoices.HardToExplain".Localized());

                OutputText("Scenes.Lumi.LumiLabChoices.IntricateMachine".Localized());

                if (Player.HasPerk(PerkIds.HistoryAlchemist))
                    OutputText("Scenes.Lumi.LumiLabChoices.ApproachesMachine".Localized());
            }
            else
            {
                ShowImage("encounter-lumi-repeat");

                OutputText("Scenes.Lumi.LumiLabChoices.EntersLabAgain".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Shop".Localized(), LumiShop);

            AddButton(1, "Common.Button.Enhance".Localized(), LumiEnhance);

            SetExitButton("Common.Button.Back".Localized());

            if ((Player.Gems >= 15 || Player.HasPerk(PerkIds.HistoryAlchemist)) && Player.HasCock())
                AddButton(3, "Common.Button.Machine".Localized(), LumiMachine);
            else
                if (Player.Gems < 15)
                    AddButton(3, "Common.Button.Machine".Localized(), null);
                else
                    if (!Player.HasCock())
                        AddButton(3, "Common.Button.Machine".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Liddellium)) && (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) >= 0) && (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumLumiFlag) == 0))
                AddButton(4, "Items.Liddellium.Short".Localized(), IdLiddellium);

            if (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) < 0 && CoC.Instance.Flags.Get(GameFlagIds.LiddelliumLumiFlag) == 0)
                AddButton(4, "Common.Button.Liddellium".Localized(), AskLiddellium);
        }

        public void LumiShop()
        {
            ClearOutput();

            OutputText("Scenes.Lumi.LumiShop.AsksPotions".Localized());

            OutputText("Scenes.Lumi.LumiShop.GawantiedQwality".Localized());

            OutputText("Scenes.Lumi.LumiShop.PriceList".Localized());

            ClearButtons();

            if (Player.Gems >= 15)
                AddButton(0, ItemRegistry.Create(ConsumableIds.LustDraft).Name, LumiLustDraftPitch);
            else
                AddButton(0, ItemRegistry.Create(ConsumableIds.LustDraft).Name, null);

            if (Player.Gems >= 20)
                AddButton(1, ItemRegistry.Create(ConsumableIds.GoblinAle).Name, LumiPitchGobboAle);
            else
                AddButton(1, ItemRegistry.Create(ConsumableIds.GoblinAle).Name, null);

            if (Player.Gems >= 45)
                AddButton(2, ItemRegistry.Create(ConsumableIds.OviElixir).Name, LumiPitchOviElixer);
            else
                AddButton(2, ItemRegistry.Create(ConsumableIds.OviElixir).Name, null);

            if (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumLumiFlag) > 0 && Player.Gems >= 500 && Player.HasItem(ItemRegistry.Create(ConsumableIds.Lollipop), 5))
                AddButton(3, "Common.Button.Liddellium".Localized(), BuyLiddellium);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LiddelliumLumiFlag) > 0)
                    AddButton(3, "Common.Button.Liddellium".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), LumiLabChoices);
        }

        public void IdLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.Lumi.IdLiddellium.SetsPhial".Localized());

            OutputText("Scenes.Lumi.IdLiddellium.ReadsLabel".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LiddelliumFlag, -1);

            LumiLabChoices();
        }

        public void AskLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.Lumi.AskLiddellium.SternRefusal".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.LiddelliumLumiFlag, 1);

            LumiLabChoices();
        }

        public void BuyLiddellium()
        {
            ClearOutput();

            OutputText("Scenes.Lumi.BuyLiddellium.HandsPayment".Localized());

            OutputText("Scenes.Lumi.BuyLiddellium.RubyRed".Localized());

            OutputText("Scenes.Lumi.BuyLiddellium.DontOveruse".Localized());

            Player.Gems -= 500;

            Player.DestroyItems(ConsumableIds.Lollipop, 5);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Liddellium), 1);

LumiShop();
        }

        public void LumiLustDraftPitch()
        {
            ClearOutput();

            ShowImage("item-draft-lust");

            OutputText("Scenes.Lumi.LumiLustDraftPitch.PointsBottle".Localized());

            OutputText("Scenes.Lumi.LumiLustDraftPitch.DeLustDwaft".Localized());

            OutputText("Scenes.Lumi.LumiLustDraftPitch.BuyLustDraft".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => LumiPurchase(ConsumableIds.LustDraft, 15));

            AddButton(1, "Common.Button.No".Localized(), LumiShop);
        }

        public void LumiPitchGobboAle()
        {
            ClearOutput();

            ShowImage("item-gAle");

            OutputText("Scenes.Lumi.LumiPitchGobboAle.PointsFlagon".Localized());

            OutputText("Scenes.Lumi.LumiPitchGobboAle.BuyGoblinAle".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => LumiPurchase(ConsumableIds.GoblinAle, 20));

            AddButton(1, "Common.Button.No".Localized(), LumiShop);
        }

        public void LumiPitchOviElixer()
        {
            ClearOutput();

            ShowImage("item-oElixir");

            OutputText("Scenes.Lumi.LumiPitchOviElixer.PointsHexBottle".Localized());

            OutputText("Scenes.Lumi.LumiPitchOviElixer.BuyOviElixir".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => LumiPurchase(ConsumableIds.OviElixir, 45));

            AddButton(1, "Common.Button.No".Localized(), LumiShop);
        }

        public void LumiPurchase(Identifier itype, int cost)
        {
            ClearOutput();

            OutputText("Scenes.Lumi.LumiPurchase.PaysGems".Localized(
                ItemRegistry.Create(itype).LongName
            ));

            Player.Gems -= cost;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1 )
            {
                Player.Inventory.Use(ItemRegistry.Create(itype), text => OutputText(text), Random.Shared);

                LumiLabChoices();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(itype), 1);

                LumiShop();
            }
        }

        public void LumiMachine()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumi.LumiMachine.PaddedSlot".Localized());

            if (Player.HasItem(ItemIds.GroPlus))
                AddButton(0, "Items.GroPlus.Short".Localized(), () => LumiMachine2(true));
            else
                AddButton(0, "Items.GroPlus.Short".Localized(), null);

            if (Player.HasItem(ItemIds.Reducto))
                AddButton(1, "Items.Reducto.Short".Localized(), () => LumiMachine2(false));
            else
                AddButton(1, "Items.Reducto.Short".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), LumiLabChoices);
        }

        public void LumiMachine2(bool enlarge, bool returned = false)
        {
            ClearOutput();

            ClearButtons();

            if (!returned)
                if (enlarge)
                    Player.DestroyItems(ItemIds.GroPlus, 1);
                else
                    Player.DestroyItems(ItemIds.Reducto, 1);

            OutputText("Scenes.Lumi.LumiMachine2.InsertsSyringe".Localized());

            var i = 0;

            while (i < Player.Cocks.Count)
            {
                AddButton(i, "Common.Text.CockNumber".Localized() + (i + 1), () => LumiMachine3(enlarge, i));

                i += 1;
            }

            AddButton(14, "Common.Button.Leave".Localized(), LumiLabChoices);
        }

        public void LumiMachine3(bool enlarge, int cockIndex)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumi.LumiMachine3.InsertsMember".Localized());

            if (Player.Cocks[cockIndex].Thickness >= 6)
                OutputText("Scenes.Lumi.LumiMachine3.DifficultySuffix".Localized());

            OutputText("Scenes.Lumi.LumiMachine3.SentencePeriod".Localized());

            OutputText("Scenes.Lumi.LumiMachine3.FeelsComfortable".Localized());

            OutputText("Scenes.Lumi.LumiMachine3.SizeReadout".Localized(
                InchesOrCentimetres(Player.Cocks[cockIndex].Length),

                InchesOrCentimetres(Player.Cocks[cockIndex].Thickness),

                Player.Cocks[cockIndex].KnotMultiplier > 1 ? "YES, OF WIDTH " + (float)Math.Ceiling(Player.Cocks[cockIndex].KnotMultiplier * Player.Cocks[cockIndex].Thickness) : "NO"
            ));

            OutputText("Scenes.Lumi.LumiMachine3.ButtonsDials".Localized());

            if (!enlarge && Player.Cocks[cockIndex].Length <= 2)
                AddButton(0, "CharCreation.MenuBeardSettings.Button2".Localized(), null);
            else
                AddButton(0, "CharCreation.MenuBeardSettings.Button2".Localized(), () => LumiMachine4(enlarge, cockIndex, 0));

            if (enlarge)
            {
                if (Player.Cocks[cockIndex].Length >= 2 * Player.Cocks[cockIndex].Thickness)
                    AddButton(1, "Common.Button.Width".Localized(), () => LumiMachine4(enlarge, cockIndex, 1));
                else
                    AddButton(1, "Common.Button.Width".Localized(), null);

                if (Player.Cocks[cockIndex].KnotMultiplier > 1)
                    if (Player.Cocks[cockIndex].KnotMultiplier <= 3.9f)
                        AddButton(2, "Common.Button.Knot".Localized(), () => LumiMachine4(enlarge, cockIndex, 2));
                    else
                        AddButton(2, "Common.Button.Knot".Localized(), null);
            }
            else
            {
                if (Player.Cocks[cockIndex].Thickness <= 1.5f)
                    AddButton(1, "Common.Button.Width".Localized(), null);
                else
                    AddButton(1, "Common.Button.Width".Localized(), () => LumiMachine4(enlarge, cockIndex, 1));

                if (Player.Cocks[cockIndex].KnotMultiplier > 1)
                    AddButton(2, "Common.Button.Knot".Localized(), () => LumiMachine4( enlarge, cockIndex, 2));
            }

            AddButton(14, "Common.Button.Back".Localized(), () => LumiMachine2(enlarge, true));
        }

        public void LumiMachine4(bool enlarge, int cockIndex, int type)
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Lumi.LumiMachine4.PressesButton".Localized());

            switch (type)
            {
                case 0:
                {
                if (enlarge)
                {
                    AddButton(0, "1\"", () => MachineChange(type, cockIndex, 1));

                    AddButton(1, "2\"", () => MachineChange(type, cockIndex, 2));

                    AddButton(2, "3\"", () => MachineChange(type, cockIndex, 3));

                    AddButton(3, "4\"", () => MachineChange(type, cockIndex, 4));
                }
                else
                {
                    if (Player.Cocks[cockIndex].Length >= 2)
                        AddButton(0, "1\"", () => MachineChange(type, cockIndex, -1));
                    else
                        AddButton(0, "1\"", null);

                    if (Player.Cocks[cockIndex].Length >= 3)
                        AddButton(1, "2\"", () => MachineChange(type, cockIndex, -2));
                    else
                        AddButton(1, "2\"", null);

                    if (Player.Cocks[cockIndex].Length >= 4)
                        AddButton(2, "3\"", () => MachineChange(type, cockIndex, -3));
                    else
                        AddButton(2, "3\"", null);

                    if (Player.Cocks[cockIndex].Length >= 5)
                        AddButton(3, "4\"", () => MachineChange(type, cockIndex, -4));
                    else
                        AddButton(3, "4\"", null);
                }
                }

                break;

                case 1:
                {
                if (enlarge)
                {
                    if (Player.Cocks[cockIndex].Length >= 2 * (Player.Cocks[cockIndex].Thickness + 0.5f))
                        AddButton(0, "0.5f\"", () => MachineChange(type, cockIndex, 0.5f));
                    else
                        AddButton(0, "0.5f\"", null);

                    if (Player.Cocks[cockIndex].Length >= 2 * (Player.Cocks[cockIndex].Thickness + 1))
                        AddButton(1, "1\"", () => MachineChange(type, cockIndex, 1));
                    else
                        AddButton(1, "1\"", null);

                    if (Player.Cocks[cockIndex].Length >= 2 * (Player.Cocks[cockIndex].Thickness + 1.5f))
                        AddButton(2, "1.5f\"", () => MachineChange(type, cockIndex, 1.5f));
                    else
                        AddButton(2, "1.5f\"", null);

                    if (Player.Cocks[cockIndex].Length >= 2 * (Player.Cocks[cockIndex].Thickness + 1.5f))
                        AddButton(3, "2\"", () => MachineChange(type, cockIndex, 2));
                    else
                        AddButton(3, "2\"", null);
                }
                else
                {
                    if (Player.Cocks[cockIndex].Thickness >= 1.5f)
                        AddButton(0, "0.5f\"", () => MachineChange(type, cockIndex, -0.5f));
                    else
                        AddButton(0, "0.5f\"", null);

                    if (Player.Cocks[cockIndex].Thickness >= 2)
                        AddButton(1, "1\"", () => MachineChange(type, cockIndex, -1));
                    else
                        AddButton(1, "1\"", null);

                    if (Player.Cocks[cockIndex].Thickness >= 2.5f)
                        AddButton(2, "1.5f\"", () => MachineChange(type, cockIndex, -1.5f));
                    else
                        AddButton(2, "1.5f\"", null);

                    if (Player.Cocks[cockIndex].Thickness >= 3)
                        AddButton(3, "2\"", () => MachineChange(type, cockIndex, -2));
                    else
                        AddButton(3, "2\"", null);
                }
                }

                break;

                case 2:
                {
                if (enlarge)
                {
                    if (Player.Cocks[cockIndex].KnotMultiplier <= 3.9f)
                        AddButton(0, "0.1f\"", () => MachineChange(type, cockIndex, 0.1f));
                    else
                        AddButton(0, "0.1f\"", null);

                    if (Player.Cocks[cockIndex].KnotMultiplier <= 3.8f)
                        AddButton(1, "0.2f\"", () => MachineChange(type, cockIndex, 0.2f));
                    else
                        AddButton(1, "0.2f\"", null);

                    if (Player.Cocks[cockIndex].KnotMultiplier <= 3.7f)
                        AddButton(2, "0.3f\"", () => MachineChange(type, cockIndex, 0.3f));
                    else
                        AddButton(2, "0.3f\"", null);

                    if (Player.Cocks[cockIndex].KnotMultiplier <= 3.6f)
                        AddButton(3, "0.4f\"", () => MachineChange(type, cockIndex, 0.4f));
                    else
                        AddButton(3, "0.4f\"", null);

                    if (Player.Cocks[cockIndex].KnotMultiplier <= 3.5f)
                        AddButton(4, "0.5f\"", () => MachineChange(type, cockIndex, 0.5f));
                    else
                        AddButton(0, "0.5f\"", null);
                }
                else
                {
                    AddButton(0, "0.1f\"", () => MachineChange(type, cockIndex, -0.1f));

                    AddButton(1, "0.2f\"", () => MachineChange(type, cockIndex, -0.2f));

                    AddButton(2, "0.3f\"", () => MachineChange(type, cockIndex, -0.3f));

                    AddButton(3, "0.4f\"", () => MachineChange(type, cockIndex, -0.4f));

                    AddButton(4, "0.5f\"", () => MachineChange(type, cockIndex, -0.5f));
                }
                }

                break;
            }

            AddButton(10, "Common.Button.ChangeChoice".Localized(), () => LumiMachine3(enlarge, cockIndex));
        }

        public void MachineChange(int type, int cockIndex, float amount)
        {
            ClearOutput();

            OutputText("Scenes.Lumi.MachineChange.WhirrsShakes".Localized());

            OutputText("Scenes.Lumi.MachineChange.LooksAtVats".Localized());

            bool knotRemoved = false;

            switch (type)
            {
                case 0:
                {
                Player.IncreaseCock(cockIndex, amount);
                }

                break;

                case 1:
                {
                Player.Cocks[cockIndex].ThickenCock(amount, true);
                }

                break;

                case 2:
                {
                Player.Cocks[cockIndex].KnotMultiplier += amount;

                if (Player.Cocks[cockIndex].KnotMultiplier <= 1)
                {
                    Player.Cocks[cockIndex].KnotMultiplier = CockPart.KnotMultiplierNoKnot;

                    knotRemoved = true;
                }
                }

                break;
            }

            ClearButtons();

            AddNextButton("Common.Button.Next".Localized(), () => LumiMachineFinal(knotRemoved));
        }

        public void LumiMachineFinal(bool knotRemoved = false)
        {
            ClearOutput();

            OutputText("Scenes.Lumi.LumiMachineFinal.PowersDown".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f), new DynStat(GameStats.Sensitivity, 2f));

            if (!Player.HasPerk(PerkIds.HistoryAlchemist))
            {
                OutputText("Scenes.Lumi.LumiMachineFinal.GivesGems".Localized());

                Player.Gems -= 15;
            }

            LumiLabChoices();
        }

        public void LumiEnhance()
        {
            ClearOutput();

            OutputText("Scenes.Lumi.LumiEnhance.LovesEnhancement".Localized());

            ClearButtons();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.FoxBerry)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.FoxBerry).Name, () => LumiEnhanceGo(ConsumableIds.FoxBerry));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.FoxBerry).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.FoxJewel)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.FoxJewel).Name, () => LumiEnhanceGo(ConsumableIds.FoxJewel));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.FoxJewel).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.GoldenSeed)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.GoldenSeed).Name, () => LumiEnhanceGo(ConsumableIds.GoldenSeed));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.GoldenSeed).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.KangaFruit)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.KangaFruit).Name, () => LumiEnhanceGo(ConsumableIds.KangaFruit));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.KangaFruit).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.LustDraft).Name, () => LumiEnhanceGo(ConsumableIds.LustDraft));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.LustDraft).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.LaBova)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.LaBova).Name, () => LumiEnhanceGo(ConsumableIds.LaBova));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.LaBova).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.OviElixir).Name, () => LumiEnhanceGo(ConsumableIds.OviElixir));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.OviElixir).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubusDelight)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.SuccubusDelight).Name, () => LumiEnhanceGo(ConsumableIds.SuccubusDelight));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.SuccubusDelight).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PigtailTruffle)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.PigtailTruffle).Name, () => LumiEnhanceGo(ConsumableIds.PigtailTruffle));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.PigtailTruffle).Name, "", "");

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)))
                AddNextButton(ItemRegistry.Create(ConsumableIds.PureHoney).Name, () => LumiEnhanceGo(ConsumableIds.PureHoney));
            else
                AddNextButtonDisabled(ItemRegistry.Create(ConsumableIds.PureHoney).Name, "", "");

            AddButton(14, "Common.Button.Back".Localized(), LumiLabChoices);
        }

        public void LumiEnhanceGo(Identifier itype)
        {
            ClearOutput();

            if (Player.Gems < 100)
            {
                OutputText("Scenes.Lumi.LumiEnhanceGo.HundredGemsAsk".Localized());

                LumiLabChoices();

                return ;
            }

            var nextItem = Item.Nothing.Id;

            if (itype == ConsumableIds.LaBova)
                nextItem = ConsumableIds.LaBova;
            else
                if (itype == ConsumableIds.KangaFruit)
                    nextItem = ConsumableIds.MightyVeggie;
                else
                    if (itype == ConsumableIds.SuccubusDelight)
                        nextItem = ConsumableIds.SuccubusDream;
                    else
                        if (itype == ConsumableIds.OviElixir)
                            nextItem = ConsumableIds.OviMax;
                        else
                            if (itype == ConsumableIds.LustDraft)
                                nextItem = ConsumableIds.FuckDraft;
                            else
                                if (itype == ConsumableIds.GoldenSeed)
                                    nextItem = ConsumableIds.MagicSeed;
                                else
                                    if (itype == ConsumableIds.FoxBerry)
                                        nextItem = ConsumableIds.FoxBerry;
                                    else
                                        if (itype == ConsumableIds.FoxJewel)
                                            nextItem = ConsumableIds.MysticJewel;
                                        else
                                            if (itype == ConsumableIds.PigtailTruffle)
                                                nextItem = ConsumableIds.BoarTruffle;
                                            else
                                                if (itype == ConsumableIds.PureHoney)
                                                    nextItem = ConsumableIds.SpecialHoney;

            Player.Gems -= 100;

            Player.ConsumeItem(itype);

            OutputText("Scenes.Lumi.LumiEnhanceGo.GrabsItem".Localized());

            var temp = Random.Shared.Next(3);

            if (itype == ConsumableIds.GoldenSeed)
                OutputText("Scenes.Lumi.LumiEnhanceGo.FiddlesCoat".Localized());
            else
                if (itype == ConsumableIds.FoxJewel)
                    OutputText("Scenes.Lumi.LumiEnhanceGo.StaresWideEyed".Localized());
                else
                    if (itype == ConsumableIds.KangaFruit)
                        OutputText("Scenes.Lumi.LumiEnhanceGo.FiddlesCoatAgain".Localized());
                    else
                        if (temp == 0)
                            OutputText("Scenes.Lumi.LumiEnhanceGo.GrabsThings".Localized(
                                ItemRegistry.Create(itype).LongName
                            ));
                        else
                            if (temp == 1)
                                OutputText("Scenes.Lumi.LumiEnhanceGo.TakesBottle".Localized());
                            else
                                if (temp == 2)
                                    OutputText("Scenes.Lumi.LumiEnhanceGo.AddsToTray".Localized(
                                        ItemRegistry.Create(itype).LongName,

                                        ItemRegistry.Create(itype).LongName,

                                        ItemRegistry.Create(itype).LongName,

                                        ItemRegistry.Create(itype).LongName
                                    ));

            Player.Inventory.Add(ItemRegistry.Create(nextItem), 1);

            LumiEnhance();
            }

    }
}