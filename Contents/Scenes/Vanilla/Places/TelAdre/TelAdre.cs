using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.DeepCave;
using MonoCoC.Contents.Scenes.Vanilla.Places.Bazaar;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Seasonal;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Utils;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<TelAdre>("coc:teladre")]
    public class TelAdre : Scene
    {
        public override void Entry()
        {
            if (!IsDiscovered())
            {
                DiscoverTelAdre();

                return;
            }

            if (!IsAllowedInto())
            {
                EncounterTelAdre();

                return;
            }

            // 原版 TelAdre.hx:235：Raphael 背叛结局且 Urta 被停职 → Ranis 在城门口发赏金
            var raphael = SceneInstance<RaphaelScene>.Get();

            if (raphael.FinaleBetrayed() && !raphael.bountyReceived && GameSettings.UrtaDisabled)
            {
                RanisPaysBounty();

                return;
            }

            // 原版 TelAdre.hx:238：情人节 Venithil 事件
            if (Flags.Get(GameFlagIds.ValentinesEventYear) < DateTime.Now.Year
                && Player.Balls > 0 && Player.HasCock()

                && Flags.Get(GameFlagIds.NumberOfTimesMetScylla) >= 4

                && Flags.Get(GameFlagIds.TimesMetScyllaInAddictionGroup) > 0

                && World.Holidays.IsValentine())
            {
                SceneInstance<ValentinesScene>.Get().CrazyVDayShenanigansByVenithil();

                return;
            }

            // 原版 TelAdre.hx:242：第一次看到 Urta/Ranis 打架
            if (!SceneInstance<UrtaQuestScene>.Get().UrtaBusy()
                && Flags.Get(GameFlagIds.PcSeenUrtaBadassFight) == 0

                && Random.Shared.Next(15) == 0 && View.Time.Hours > 15)
            {
                UrtaIsABadass();

                return;
            }

            // 原版 TelAdre.hx:246：Urta 怀孕值班事件
            if (!SceneInstance<UrtaQuestScene>.Get().UrtaBusy()
                && SceneInstance<UrtaScene>.Get().Pregnancy.Event > 5

                && Random.Shared.Next(30) == 0)
            {
                SceneInstance<UrtaPregsScene>.Get().UrtaIsAPregnantCopScene();

                return;
            }

            // 原版 TelAdre.hx:250：Katherine 各阶段事件
            switch (Flags.Get(GameFlagIds.KatherineUnlocked))
            {
                case -1:
                case 0:
                    if (Flags.Get(GameFlagIds.KatherineRandomRecruitmentDisabled) == 0
                        && Player.Gems > 34 && Random.Shared.Next(25) == 0)
                    {
                        if (Flags.Get(GameFlagIds.KatherineUnlocked) == 0)
                            SceneInstance<KatherineScene>.Get().AmbushByVagrantKittyKats();
                        else
                            SceneInstance<KatherineScene>.Get().RepeatAmbushKatherineRecruitMent();

                        return;
                    }

                    break;

                case 1:
                case 2:
                case 3:
                case 4:
                    if (!SceneInstance<KatherineScene>.Get().IsAt(KatherineScene.KLOC_KATHS_APT)
                        && Flags.Get(GameFlagIds.KatherineTraining) >= 100)
                    {
                        SceneInstance<KatherineEmploymentScene>.Get().KatherineGetsEmployed();

                        return;
                    }

                    break;

                default:
                    if (View.Time.Hours < 10 && Random.Shared.Next(12) == 0)
                    {
                        SceneInstance<KatherineScene>.Get().KatherineOnDuty();

                        return;
                    }

                    break;
            }

            // 原版 TelAdre.hx:271：Arian 公园初遇
            if (Flags.Get(GameFlagIds.ArianPark) == 0 && Random.Shared.Next(10) == 0
                && Flags.Get(GameFlagIds.NotHelpedArianToday) == 0)
            {
                SceneInstance<ArianScene>.Get().MeetArian();

                return;
            }

            // 原版 TelAdre.hx:278：Kittens 初遇
            if (!SceneInstance<Kittens>.Get().metKittens
                && View.Time.Hours > 16 && View.Time.Hours < 21

                && Random.Shared.Next(20) == 0)
            {
                SceneInstance<Kittens>.Get().KittensMeet();

                return;
            }

            // 原版 TelAdre.hx:282：Edryn 孩子遭遇
            if (!EdrynScene.Save.GetBool("kidMet")
                && View.Time.Hours >= 10 && View.Time.Hours <= 16

                && Flags.Get(GameFlagIds.EdrynNumberOfKids) > 0 && Random.Shared.Next(10) == 0)
            {
                SceneInstance<EdrynScene>.Get().EdrynKidEncounter();

                return;
            }

            // 原版 TelAdre.hx:290：小孩、11-16 点、1/3 概率、当天未触发
            if (Player.IsChild() && View.Time.Hours >= 11 && View.Time.Hours <= 16
                && Random.Shared.Next(3) == 0 && Flags.Get(GameFlagIds.TelAdreIceCreamed) == 0)
            {
                IceCream();

                return;
            }

            // 原版 TelAdre.hx:300：Urta 特殊生产
            if (SceneInstance<UrtaScene>.Get().Pregnancy.Event > 2 && Random.Shared.Next(4) == 0
                && Flags.Get(GameFlagIds.UrtaPregnantDeliveryScene) == 0

                && Player.HasKeyItem("Spare Key to Urta's House"))
            {
                SceneInstance<UrtaPregsScene>.Get().UrtaSpecialDeliveries();

                return;
            }

            // 原版 TelAdre.hx:315：Maddie 离家后续
            if (Flags.Get(GameFlagIds.MaddieStatus) == -1)
            {
                SceneInstance<MaddieScene>.Get().RunAwayMaddieFollowup();

                return;
            }

            ClearOutput();

            OutputText("Scenes.Teladre.TelAdreMenu.MassiveCity".Localized());
            // 原版 TelAdre.hx:312：(urtaDisabled ? "the bar, 'The Wet Bitch'. Y" : "Urta's favorite bar, ...")
            OutputText("Scenes.Teladre.TelAdreMenu.SignCentaur".Localized(GameSettings.UrtaDisabled

                ? "the bar, 'The Wet Bitch'. Y"

                : "Urta's favorite bar, 'The Wet Bitch'. How strange that those would be what she talks about during a tour. In any event y"));

            ClearButtons();

            AddButton(0, "Common.Button.Shops".Localized(), Shops);

            AddButton(1, "Common.Button.Bakery".Localized(), () => SceneInstance<BakeryScene>.Get().Bakeryuuuuuu());

            AddButton(2, "Common.Button.Bar".Localized(), EnterBarTelAdre);

            AddButton(3, "Common.Button.Gym".Localized(), () => SceneInstance<GymScene>.Get().GymDesc());

            var raphaelScene = SceneInstance<RaphaelScene>.Get();

            var homes = (raphaelScene.FinaleCovered() && raphaelScene.RaphaelLikes())

                || Player.HasKeyItem("Spare Key to Urta's House")

                || Flags.Get(GameFlagIds.KatherineUnlocked) >= 5

                || (Flags.Get(GameFlagIds.ArianPark) >= 4 && !SceneInstance<ArianScene>.Get().ArianFollower());

            if (homes)
                AddButton(4, "Common.Button.Homes".Localized(), Houses);

            if (Flags.Get(GameFlagIds.ArianPark) > 0 && Flags.Get(GameFlagIds.ArianPark) < 4)
                AddButton(5, "Common.Button.Park".Localized(), () => SceneInstance<ArianScene>.Get().VisitThePark());

            AddButton(6, "Scenes.Teladre.TelAdreMenu.Pawn".Localized(), OswaldPawn);

            AddButton(7, "Common.Button.Tower".Localized(), () => SceneInstance<Library>.Get().VisitZeMagesTower());

            AddButton(8, "Common.Button.Jasun".Localized(), () => Go<Jasun>());

            AddButton(9, "Common.Button.SexMachine".Localized(), () => Go<SexMachine>());

            AddButton(10, "Common.Button.AuntNancy".Localized(), () => Go<AuntNancy>());

            AddButton(11, "Common.Button.Kittens".Localized(), () => Go<Kittens>());

            AddButton(12, "Common.Button.Frosty".Localized(), () => Go<Frosty>());

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.DesertScene>.Get()));
        }

        /// <summary>原版 TelAdre.hx isDiscovered()：只要拥有 Tel'Adre 状态效果即算已发现。</summary>
        public virtual bool IsDiscovered() => Player.HasStatusEffect(StatusEffectIds.TelAdre);

        public bool IsAllowedInto() => Player.StatusEffectValue(StatusEffectIds.TelAdre, 0) >= 1;

        public void EnterBarTelAdre()
        {
            // TelAdre.hx:551 enterBarTelAdre
            if (SceneInstance<ThanksgivingScene>.Get().PigSlutAllowed())
                SceneInstance<ThanksgivingScene>.Get().PigSlutRoastingGreet();
            else
                BarTelAdre();
        }

        public void BarTelAdre()
        {
            // TelAdre.hx:559 barTelAdre
            ClearOutput();

            if (Flags.Get(GameFlagIds.LoppeDisabled) == 0
                && Flags.Get(GameFlagIds.LoppeMet) == 0

                && Random.Shared.Next(10) == 0)
            {
                SceneInstance<LoppeScene>.Get().LoppeFirstMeeting();

                return;
            }

            var noFur = !Player.HasFur();

            OutputText("Scenes.Teladre.BarTelAdre.WetBitchInterior".Localized(noFur ? "eared people" : "morphs"));

            if (Player.HumanScore() <= 3)
                OutputText("Scenes.Teladre.BarTelAdre.AlteredAppearance".Localized());

            OutputText("Scenes.Teladre.BarTelAdre.HardlyStares".Localized());

            // Done before anything else so that other NPCs can check Scylla's action (TelAdre.hx:579)
            SceneInstance<ScyllaScene>.Get().ScyllaBarSelectAction();

            if (SceneInstance<ValaScene>.Get().PurifiedFaerieBitchBar())
                AddButton(8, "Npcs.Vala.Name".Localized(), () => SceneInstance<ValaScene>.Get().ChooseValaInBar());

            ClearButtons();

            if (Flags.Get(GameFlagIds.AmilyVisitingUrta) == 1)
                AddButton(0, "Common.Button.Ask4amily".Localized(), () => SceneInstance<FollowerInteractionsScene>.Get().AskAboutAmily());

            AddButton(1, "Common.Button.Edryn".Localized(), () => SceneInstance<EdrynScene>.Get().EdrynBarTalk());

            AddButton(2, "Common.Button.Katherine".Localized(), () => SceneInstance<KatherineScene>.Get().BarApproach());

            AddButton(3, "Common.Button.Helia".Localized(), () => SceneInstance<EdrynScene>.Get().ApproachHelAtZeBitch());

            AddButton(4, "Common.Button.Nancy".Localized(), () => SceneInstance<AuntNancy>.Get().InteractWithAuntNancy());

            AddButton(5, "Common.Button.Niamh".Localized(), () => SceneInstance<NiamhScene>.Get().ApproachNiamh());

            AddButton(6, "Common.Button.Rogar".Localized(), () => SceneInstance<RogarScene>.Get().RogarPhaseFour());

            AddButton(7, "Common.Button.Scylla".Localized(), () => SceneInstance<ScyllaScene>.Get().TalkToScylla());

            AddButton(14, "Common.Button.Leave".Localized(), () => Go<TelAdre>());
        }

        // ===== 原版 TelAdre.hx:385 oswaldPawn / :434 buyCarrotFromOswald / :455 oswaldBuyMenu =====

        public void OswaldPawn()
        {
            SpriteSelect("oswald");

            ClearOutput();

            var noFur = !Player.HasFur();

            if (!Player.HasStatusEffect(StatusEffectIds.Oswald))
            {
                OutputText("Scenes.Teladre.OswaldPawn.CloserInspection".Localized(noFur

                    ? "'s ears resemble those of a golden retriever"

                    : " appears to be some kind of golden retriever"));

                OutputText(Player.Corruption < 75

                    ? "Scenes.Teladre.OswaldPawn.WhoWouldThought".Localized()

                    : "Scenes.Teladre.OswaldPawn.WhatAShame".Localized());

                OutputText("Scenes.Teladre.OswaldPawn.StandMess".Localized());

                OutputText("Scenes.Teladre.OswaldPawn.IntroducesOswald".Localized(noFur ? "-man" : ""));

                OutputText("Scenes.Teladre.OswaldPawn.CanSellItem".Localized());

                Player.CreateStatusEffect(StatusEffectIds.Oswald, 0, 0, 0, 0);
            }
            else
                OutputText("Scenes.Teladre.OswaldPawn.FiddlingTopHat".Localized(noFur

                    ? "walking up to you and rubbing his"

                    : "padding up to you and rubbing his furry", Player.Mf("sir", "dear")));

            ClearButtons();

            AddButton(0, "Scenes.Teladre.TelAdreMenu.OswaldBuy".Localized(), OswaldBuyMenu);

            AddButton(1, "Scenes.Teladre.TelAdreMenu.OswaldSell".Localized(), () => OswaldPawnMenu());

            switch (Flags.Get(GameFlagIds.KatherineUnlocked))
            {
                case 1:
                case 2:
                    AddButton(2, "Scenes.Teladre.TelAdreMenu.OswaldKathAlley".Localized(), () => SceneInstance<KatherineScene>.Get().VisitKatherine());

                    break;

                case 3:
                    AddButton(2, "Scenes.Teladre.TelAdreMenu.OswaldSafehouse".Localized(), () => SceneInstance<KatherineEmploymentScene>.Get().KatherineTrainingWithUrta());

                    break;

                case 4:
                    AddButton(2, "Scenes.Teladre.TelAdreMenu.OswaldKathAlley".Localized(), () => SceneInstance<KatherineEmploymentScene>.Get().PostTrainingAlleyDescription());

                    break;
            }

            if (!Player.HasKeyItem("Carrot") && Flags.Get(GameFlagIds.NieveStage) > 0 && Flags.Get(GameFlagIds.NieveStage) < 4)
            {
                OutputText("Scenes.Teladre.OswaldPawn.MentionCarrot".Localized());

                OutputText("Scenes.Teladre.OswaldPawn.TopHatTips".Localized());

                if (Player.Gems < 500)
                    OutputText("Scenes.Teladre.OswaldPawn.CantAfford".Localized());
                else
                    AddButton(3, "Scenes.Teladre.TelAdreMenu.OswaldBuyCarrot".Localized(), BuyCarrotFromOswald);
            }

            AddButton(14, "Common.Button.Back".Localized(), () => Go<TelAdre>());
        }

        public void BuyCarrotFromOswald()
        {
            Player.Gems -= 500;

            Player.CreateKeyItem("Carrot", 0, 0, 0, 0);

            ClearOutput();

            OutputText("Scenes.Teladre.BuyCarrotFromOswald.GemsChangeHands".Localized());

            OutputText("Scenes.Teladre.BuyCarrotFromOswald.AcquiredCarrot".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), OswaldPawn);
        }

        private Item[] OswaldStock()
        {
            if (string.IsNullOrEmpty(Flags.Get(GameFlagIds.Benoit1)))
                SceneInstance<BenoitScene>.Get().UpdateBenoitInventory();

            return

            [
                ItemRegistry.Create(Identifier.Parse(Flags.Get(GameFlagIds.Benoit1))),

                ItemRegistry.Create(Identifier.Parse(Flags.Get(GameFlagIds.Benoit2))),

                ItemRegistry.Create(Identifier.Parse(Flags.Get(GameFlagIds.Benoit3))),
            ];
        }

        public void OswaldBuyMenu()
        {
            ClearOutput();

            var itypes = OswaldStock();

            const float buyMod = 2;

            OutputText("Scenes.Teladre.OswaldBuyMenu.AsksAnything".Localized());

            OutputText("Scenes.Teladre.OswaldBuyMenu.PricesBanner".Localized());

            OutputText("Scenes.Teladre.OswaldBuyMenu.FirstItemLine".Localized(itypes[0].LongName, buyMod * itypes[0].Value));

            OutputText("Scenes.Teladre.OswaldBuyMenu.SecondItemLine".Localized(itypes[1].LongName, buyMod * itypes[1].Value));

            OutputText("Scenes.Teladre.OswaldBuyMenu.ThirdItemLine".Localized(itypes[2].LongName, buyMod * itypes[2].Value));

            ClearButtons();

            AddButton(0, itypes[0].Name, () => OswaldTransactBuy(1), itypes[0].TooltipText);

            AddButton(1, itypes[1].Name, () => OswaldTransactBuy(2), itypes[1].TooltipText);

            AddButton(2, itypes[2].Name, () => OswaldTransactBuy(3), itypes[2].TooltipText);

            AddButton(14, "Common.Button.Back".Localized(), OswaldPawn);
        }

        public void OswaldTransactBuy(int slot = 1)
        {
            ClearOutput();

            var itypes = OswaldStock();

            var itype = itypes[slot - 1];

            const float buyMod = 2;

            if (Player.Gems < (float)(buyMod * itype.Value))
            {
                OutputText("Scenes.Teladre.OswaldTransactBuy.ConsiderPurchase".Localized());

                DoNext(OswaldBuyMenu);

                return;
            }

            OutputText("Scenes.Teladre.OswaldTransactBuy.AfterExamining".Localized());

            Player.Gems -= (int)Math.Round(buyMod * itype.Value);

            InventoryScene.TakeItem(itype, OswaldBuyMenu);
        }

        public void OswaldPawnMenu()
        {
            ClearOutput();

            SpriteSelect("oswald");

            var noFur = !Player.HasFur();

            OutputText("Scenes.Teladre.OswaldPawnMenu.FiddlingTopHat".Localized(noFur

                ? "walking up to you and rubbing his"

                : "padding up to you and rubbing his furry", Player.Mf("sir", "dear")));

            OutputText("Scenes.Teladre.OswaldPawnMenu.CanSellItem".Localized());

            OutputText("Scenes.Teladre.OswaldPawnMenu.EstimatesBanner".Localized());

            ClearButtons();

            var totalItems = 0;

            for (var slot = 0; slot < 10; slot++)
            {
                var stack = Player.Inventory.Slots[slot];

                if (stack.Quantity > 0 && (int)(stack.Item.Value / 2) >= 1)
                {
                    OutputText("Scenes.Teladre.OswaldPawnMenu.GemsFor".Localized(
                        (int)(stack.Item.Value / 2),

                        stack.Item.LongName
                    ));

                    AddButton(slot, stack.InvLabel, () => OswaldPawnSell(slot), stack.TooltipText);

                    totalItems += stack.Quantity;
                }
            }

            if (totalItems > 1)
                AddButton(12, "Common.Button.SellAll".Localized(), OswaldPawnSellAll);

            AddButton(14, "Common.Button.Back".Localized(), OswaldPawn);
        }

        public void OswaldPawnSell(int slot)
        {
            SpriteSelect("oswald");

            var stack = Player.Inventory.Slots[slot];

            var itemValue = (int)(stack.Item.Value / 2);

            var shift = Flags.Get(GameFlagIds.ShiftKeyDown) == 1;

            var count = shift ? stack.Quantity : 1;

            var total = itemValue * count;

            var itemName = NumberUtils.Num2Text(count) + " " + stack.Item.Name;

            ClearOutput();

            if (shift)
                OutputText(itemValue == 0

                    ? "Scenes.Teladre.OswaldPawnSell.HandsOverQuantity".Localized(NumberUtils.Num2Text(count), stack.Item.Name)

                    : "Scenes.Teladre.OswaldPawnSell.HandsOverQuantityAgain".Localized(NumberUtils.Num2Text(count), stack.Item.Name, NumberUtils.Num2Text(total)));
            else
                OutputText(itemValue == 0

                    ? "Scenes.Teladre.OswaldPawnSell.HandsOverItem".Localized(itemName)

                    : "Scenes.Teladre.OswaldPawnSell.HandsOverItemAgain".Localized(itemName, NumberUtils.Num2Text(total)));

            for (var index = 0; index < count; index++)
                stack.RemoveOneItem();

            Player.Gems += total;

            DoNext(OswaldPawn);
        }

        public void OswaldPawnSellAll()
        {
            SpriteSelect("oswald");

            var total = 0;

            for (var slot = 0; slot < 10; slot++)
            {
                var stack = Player.Inventory.Slots[slot];

                if (stack.Quantity > 0 && (int)(stack.Item.Value / 2) >= 1)
                {
                    total += stack.Quantity * (int)(stack.Item.Value / 2);

                    stack.Quantity = 0;
                }
            }

            ClearOutput();

            OutputText("Scenes.Teladre.OswaldPawnSellAll.LaysOutItems".Localized(NumberUtils.Num2Text(total)));

            Player.Gems += total;

            DoNext(OswaldPawn);
        }

        // ===== 原版 TelAdre.hx:768 iceCream / :785 iceCreamYes / :802 iceCreamNo / :812 iceCreamNoFinal / :821 iceCreamRun =====

        public void IceCream()
        {
            Flags.Set(GameFlagIds.TelAdreIceCreamed, 1);

            ClearOutput();

            OutputText("Scenes.Teladre.IceCream.HotDustyRoads".Localized());

            OutputText("Scenes.Teladre.IceCream.NearbyAlley".Localized());

            OutputText("Scenes.Teladre.IceCream.HearsVoice".Localized());

            OutputText("Scenes.Teladre.IceCream.AsksGentle".Localized());

            OutputText("Scenes.Teladre.IceCream.ShakesHeadNo".Localized());

            OutputText("Scenes.Teladre.IceCream.NodContinues".Localized());

            OutputText("Scenes.Teladre.IceCream.CrouchedEntrance".Localized());

            OutputText("Scenes.Teladre.IceCream.LooksExpectantly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), IceCreamYes);

            AddButton(1, "Common.Button.No".Localized(), IceCreamNo);

            AddButton(2, "Common.Button.Run".Localized(), IceCreamRun);
        }

        public void IceCreamYes()
        {
            ClearOutput();

            OutputText("Scenes.Teladre.IceCreamYes.SaysGrinning".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.WaitsUncertain".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.ArrivesHolding".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.NotTooPicky".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.PutsForth".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.HeadInclines".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.TastesGood".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.GlanceUp".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.LickAfterLick".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.GivesAffirmation".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.ParticularlyPleased".Localized());

            OutputText("Scenes.Teladre.IceCreamYes.StrangeEncounter".Localized());

            DoNext(() => Go<TelAdre>());
        }

        public void IceCreamNo()
        {
            ClearOutput();

            OutputText("Scenes.Teladre.IceCreamNo.NotReallySure".Localized());

            OutputText("Scenes.Teladre.IceCreamNo.AsksSmilesWarmly".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), IceCreamYes);

            AddButton(1, "Common.Button.No".Localized(), IceCreamNoFinal);

            AddButton(2, "Common.Button.Run".Localized(), IceCreamRun);
        }

        public void IceCreamNoFinal()
        {
            ClearOutput();

            OutputText("Scenes.Teladre.IceCreamNoFinal.ShakesHeadFirmly".Localized());

            OutputText("Scenes.Teladre.IceCreamNoFinal.TakeCareYourself".Localized());

            OutputText("Scenes.Teladre.IceCreamNoFinal.LeftDisconcerted".Localized());

            OutputText("Scenes.Teladre.IceCreamNoFinal.NothingLeft".Localized());

            DoNext(() => Go<TelAdre>());
        }

        public void IceCreamRun()
        {
            ClearOutput();

            OutputText("Scenes.Teladre.IceCreamRun.DontKnowWhat".Localized());

            OutputText("Scenes.Teladre.IceCreamRun.RiskIt".Localized());

            OutputText("Scenes.Teladre.IceCreamRun.FeelsSafety".Localized());

            OutputText("Scenes.Teladre.IceCreamRun.SenseUnease".Localized());

            OutputText("Scenes.Teladre.IceCreamRun.NothingMoreToDo".Localized());

            DoNext(() => Go<TelAdre>());
        }

        // ===== 原版 TelAdre.hx:831 ranisPaysBounty =====

        public void RanisPaysBounty()
        {
            ClearOutput();

            OutputText("Scenes.Teladre.RanisPaysBounty.RanisStopsGate".Localized());

            OutputText("Scenes.Teladre.RanisPaysBounty.GivesNod".Localized());

            SceneInstance<RaphaelScene>.Get().bountyReceived = true;

            Player.Gems += 1000;

            DoNext(() => Go<TelAdre>());
        }

        // ===== 原版 TelAdre.hx:738 urtaIsABadass / :748 watchUrtaBeABadass =====

        public void UrtaIsABadass()
        {
            Flags.Set(GameFlagIds.PcSeenUrtaBadassFight, 1);

            ClearOutput();

            OutputText("Scenes.Teladre.UrtaBadass.Commotion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Investigate".Localized(), WatchUrtaBeABadass);

            AddButton(1, "Common.Button.WhoCares".Localized(), () => Go<TelAdre>());
        }

        public void WatchUrtaBeABadass()
        {
            ClearOutput();

            var noFur = !Player.HasFur();

            var urtaDisabled = GameSettings.UrtaDisabled;

            OutputText("Scenes.Teladre.UrtaBadass.PressThrough".Localized(
                noFur ? "fluffy tails" : "rough fur",

                noFur ? "they brush" : "it brushes",

                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled ? "bulky dog-man" : "comparatively diminutive fox-woman",

                noFur ? "clothes are soaked" : "dark brown fur is matted"
            ));

            OutputText("Scenes.Teladre.UrtaBadass.Fight".Localized(
                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled ? "his" : "her",

                urtaDisabled ? "his" : "her",

                urtaDisabled ? "His" : "Her",

                urtaDisabled ? "Ranis" : "Urta"
            ));

            OutputText("Scenes.Teladre.UrtaBadass.Beast".Localized(
                Player.HasStatusEffect(StatusEffectIds.Infested) ? "aroused"

                    : Player.Corruption < 50 ? "horrified" : "confused"
            ));

            OutputText("Scenes.Teladre.UrtaBadass.Kneel".Localized(
                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled ? "him" : "her",

                urtaDisabled ? "he" : "she",

                urtaDisabled ? "him" : "her",

                urtaDisabled ? "his" : "her",

                urtaDisabled ? "his" : "her",

                urtaDisabled ? "Ranis" : "Urta"
            ));

            OutputText("Scenes.Teladre.UrtaBadass.Barks".Localized(urtaDisabled ? "He" : "She"));

            OutputText("Scenes.Teladre.UrtaBadass.DragAway".Localized(
                noFur ? "men" : "morphs",

                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled

                    ? "people nod to Ranis respectfully. He keeps his expression neutral and excuses himself to resume his rounds, wiping his hands off on his armor-studded pants as he"

                    : "males nod to Urta respectfully. She keeps her expression neutral and excuses herself to resume her rounds, wiping her hands off on her armor-studded skirt as she"
            ));

            DoNext(() => Go<TelAdre>());
        }

        // ===== 原版 TelAdre.hx:110 setStatus / :120 discoverTelAdre / :126 encounterTelAdre =====

        public void SetDiscovered(bool discovered, bool allowed)
        {
            if (!discovered)
            {
                Player.RemoveStatusEffect(StatusEffectIds.TelAdre);

                return;
            }

            if (!Player.HasStatusEffect(StatusEffectIds.TelAdre))
                Player.CreateStatusEffect(StatusEffectIds.TelAdre, allowed ? 1 : 0, 0, 0, 0);
            else
                Player.ChangeStatusValue(StatusEffectIds.TelAdre, 1, allowed ? 1 : 0);
        }

        public void DiscoverTelAdre()
        {
            ClearOutput();

            if (!IsDiscovered())
            {
                OutputText("Scenes.Teladre.DiscoverTelAdre.1".Localized());

                OutputText("Scenes.Teladre.DiscoverTelAdre.2".Localized());
            }
            else
                OutputText("Scenes.Teladre.DiscoverTelAdre.3".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), EncounterTelAdre);

            AddButton(1, "Common.Button.No".Localized(), () => ReturnToCamp(1));
        }

        public void EncounterTelAdre()
        {
            ClearOutput();

            var urtaDisabled = GameSettings.UrtaDisabled;

            var noFur = !Player.HasFur();

            if (!IsDiscovered())
            {
                OutputText("Scenes.Teladre.EncounterTelAdre.1".Localized(urtaDisabled

                    ? "burly, dog-eared man"

                    : noFur ? "fox-eared girl with gray hair" : "female gray fox"));

                OutputText("Scenes.Teladre.EncounterTelAdre.2".Localized(
                    urtaDisabled ? "he" : "she",

                    urtaDisabled ? "he" : "she"
                ));

                OutputText("Scenes.Teladre.EncounterTelAdre.3".Localized(urtaDisabled

                    ? "dog-man, his tail"

                    : noFur ? "fox-girl, her dark gray tail" : "fox, her dark gray fur"));

                OutputText("Scenes.Teladre.EncounterTelAdre.4".Localized(
                    urtaDisabled ? "man" : "girl",

                    urtaDisabled ? "He" : "She"
                ));

                OutputText("Scenes.Teladre.EncounterTelAdre.5".Localized(urtaDisabled

                    ? "guard"

                    : "cute vulpine-girl"));

                OutputText("Scenes.Teladre.EncounterTelAdre.6".Localized(
                    urtaDisabled ? "Captain" : "Urta",

                    urtaDisabled ? "The dog-eared guard captain shakes his" : "Urta the fox shakes her",

                    Player.Mf("He's", "She's")
                ));

                OutputText("Scenes.Teladre.EncounterTelAdre.7".Localized());

                OutputText("Scenes.Teladre.EncounterTelAdre.8".Localized());

                TelAdreCrystal();

                return;
            }

            OutputText("Scenes.Teladre.EncounterTelAdre.9".Localized(
                urtaDisabled ? "muscular guard captain" : noFur ? "gray-haired fox, Urta," : "gray fox, Urta,",

                urtaDisabled ? "He" : "She",

                urtaDisabled ? "his" : "her"
            ));

            TelAdreCrystal();
        }

        public void TelAdreCrystal()
        {
            if (!IsDiscovered())
                SetDiscovered(true, false);

            var urtaDisabled = GameSettings.UrtaDisabled;

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan) || !Player.IsPureEnough(70))
            {
                OutputText("Scenes.Teladre.Crystal.1".Localized());

                OutputText("Scenes.Teladre.Crystal.2".Localized());

                DoNext(() => ReturnToCamp(1));

                return;
            }

            if (!Player.IsPureEnough(50) || SceneInstance<JojoScene>.Get().IsJojoCorrupted())
                OutputText("Scenes.Teladre.Crystal.3".Localized());
            else if (!Player.IsPureEnough(25) || Flags.Get(GameFlagIds.FactoryShutdown) == 2)
                OutputText("Scenes.Teladre.Crystal.4".Localized());
            else
                OutputText("Scenes.Teladre.Crystal.5".Localized());

            OutputText("Scenes.Teladre.Crystal.6".Localized(urtaDisabled

                ? "guard captain introduces himself as Ranis, then"

                : "vixen Urta"));

            if (Player.IsChild())
            {
                OutputText("Scenes.Teladre.Crystal.7".Localized(
                    urtaDisabled ? "the dog-man" : "Urta",

                    urtaDisabled ? "Captain" : "Urta",

                    urtaDisabled ? "Ranis" : "Urta"
                ));

                OutputText("Scenes.Teladre.Crystal.8".Localized(urtaDisabled ? "dog" : "fox"));

                OutputText("Scenes.Teladre.Crystal.9".Localized());

                OutputText("Scenes.Teladre.Crystal.10".Localized());

                DoNext(TelAdreTourLoli);

                return;
            }

            OutputText("Scenes.Teladre.Crystal.11".Localized(
                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled ? "" : " You do notice her gait is a bit odd, and her fluffy fox-tail seems to be permanently wrapped around her right leg"
            ));

            DoNext(TelAdreTour);
        }

        public void TelAdreTourLoli()
        {
            SetDiscovered(true, true);

            ClearOutput();

            SpriteSelect(null);

            OutputText("Scenes.Teladre.TourLoli.1".Localized(Player.Mf("boy", "girl")));

            OutputText("Scenes.Teladre.TourLoli.2".Localized());

            OutputText("Scenes.Teladre.TourLoli.3".Localized());

            OutputText("Scenes.Teladre.TourLoli.4".Localized());

            OutputText("Scenes.Teladre.TourLoli.5".Localized());

            OutputText("Scenes.Teladre.TourLoli.6".Localized());

            OutputText("Scenes.Teladre.TourLoli.7".Localized());

            if (Player.Gems < 40)
                OutputText("Scenes.Teladre.TourLoli.8".Localized());
            else
                OutputText("Scenes.Teladre.TourLoli.9".Localized());

            OutputText("Scenes.Teladre.TourLoli.10".Localized());

            OutputText("Scenes.Teladre.TourLoli.11".Localized());

            OutputText("Scenes.Teladre.TourLoli.12".Localized(GameSettings.UrtaDisabled ? "Ranis" : "Urta"));

            OutputText("Scenes.Teladre.TourLoli.13".Localized());

            OutputText("Scenes.Teladre.TourLoli.14".Localized());

            OutputText("Scenes.Teladre.TourLoli.15".Localized());

            Player.RefillHunger(30);

            DoNext(() => Go<TelAdre>());
        }

        public void TelAdreTour()
        {
            SetDiscovered(true, true);

            ClearOutput();

            SceneInstance<UrtaScene>.Get().UrtaSprite();

            var urtaDisabled = GameSettings.UrtaDisabled;

            OutputText("Scenes.Teladre.Tour.1".Localized(
                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled ? "the" : "her and her"
            ));

            OutputText("Scenes.Teladre.Tour.2".Localized(
                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled ? "He" : "She",

                urtaDisabled ? "his" : "her",

                urtaDisabled ? "his" : "her"
            ));

            OutputText(Player.Corruption < 25

                ? "Scenes.Teladre.Tour.3".Localized()

                : "Scenes.Teladre.Tour.4".Localized());

            OutputText("Scenes.Teladre.Tour.5".Localized(urtaDisabled ? "Ranis" : "Urta"));

            OutputText("Scenes.Teladre.Tour.6".Localized(
                urtaDisabled ? "his" : "Urta's",

                urtaDisabled ? "Ranis" : "Urta",

                urtaDisabled

                    ? " as he"

                    : ", though the tail she keeps wrapped around her leg twitches as she"
            ));

            OutputText("Scenes.Teladre.Tour.7".Localized(urtaDisabled

                ? "He claps you roughly"

                : "She gives you a friendly punch"));

            OutputText("Scenes.Teladre.Tour.8".Localized(urtaDisabled

                ? "he's taken off back towards the city gates, leaving you free to explore the city on your own."

                : "she's taken off back down the street, probably stopping off at 'The Wet Bitch' for a drink. Strange, her departure was rather sudden..."));

            DoNext(() => Go<TelAdre>());
        }

        // ===== 原版 TelAdre.hx:357 houses() =====

        public void Houses()
        {
            ClearOutput();

            OutputText("Scenes.Teladre.Houses.WhoseHome".Localized());

            ClearButtons();

            if (Flags.Get(GameFlagIds.KatherineUnlocked) >= 5)
                AddButton(0, "Scenes.Teladre.TelAdreMenu.OswaldKathAlley".Localized(), () => SceneInstance<KatherineScene>.Get().VisitAtHome());

            if (SceneInstance<UrtaPregsScene>.Get().UrtaKids() > 0 && Player.HasKeyItem("Spare Key to Urta's House"))
            {
                var katherine = SceneInstance<KatherineScene>.Get();

                AddButton(1, "Scenes.Teladre.Houses.UrtasHouse".Localized(), () =>
                {
                    if (katherine.IsAt(KatherineScene.KLOC_URTAS_HOME))
                        katherine.KatherineAtUrtas();
                    else
                        SceneInstance<UrtaPregsScene>.Get().VisitTheHouse();
                });
            }

            if (Flags.Get(GameFlagIds.ArianPark) >= 4 && !SceneInstance<ArianScene>.Get().ArianFollower())
                AddButton(2, "Scenes.Teladre.Houses.Arians".Localized(), () => SceneInstance<ArianScene>.Get().VisitAriansHouse());

            var raphael = SceneInstance<RaphaelScene>.Get();

            if (raphael.FinaleCovered() && raphael.RaphaelLikes())
                AddButton(3, "Scenes.Teladre.Houses.Orphanage".Localized(), raphael.OrphanageIntro);

            AddButton(14, "Common.Button.Back".Localized(), () => Go<TelAdre>());
        }

        // ===== 原版 TelAdre.hx:343 armorShops() =====

        public void Shops()
        {
            ClearOutput();

            OutputText("Scenes.Teladre.Shops.Street".Localized());

            OutputText("Scenes.Teladre.Shops.Second".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Armorsmith".Localized(), () => Go<YvonneArmorShop>());

            AddButton(1, "Common.Button.Weaponsmith".Localized(), () => Go<WeaponShop>());

            AddButton(2, "Common.Button.Tailor".Localized(), () => Go<VictoriaTailorShop>());

            AddButton(3, "Common.Button.Jewelry".Localized(), () => Go<JewelryShop>());

            AddButton(4, "Common.Button.Piercing".Localized(), () => Go<YaraPiercingStudio>());

            AddButton(5, "Common.Button.Clinic".Localized(), () => Go<UmasShopScene>());

            AddButton(6, "Common.Button.Carpenter".Localized(), () => Go<CarpentryShop>());

            AddButton(14, "Common.Button.Back".Localized(), () => Go<TelAdre>());
        }
    }
}