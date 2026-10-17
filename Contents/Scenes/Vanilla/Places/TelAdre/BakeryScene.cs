using MonoCoC.Attributes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Plains;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Internals;
using MonoCoC.Utils;
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<BakeryScene>("coc:bakery")]
    public class BakeryScene : Scene
    {

        public static SaveContent Save => SaveContent(SceneIds.TelAdre.Bakery);

        public override void Entry()
        {
            Bakeryuuuuuu();
        }

        public void Bakeryuuuuuu()
        {
            ClearOutput();

            if (Holidays.IsEaster() && Player.HasCock() && (CoC.Instance.Flags.Get(GameFlagIds.LastEasterYear) < DateTime.Now.Year || Random.Shared.Next(20) == 0))
            {
                CoC.Instance.Flags.Set(GameFlagIds.LastEasterYear, DateTime.Now.Year);

                EasterBakeSale();

                return ;
            }

            if (Random.Shared.Next(10) <= 1 && false  && (int)Player.Gender > 0 && CoC.Instance.Flags.Get(GameFlagIds.MaddieStatus) == 4)

                return ;

            CoC.Instance.Flags.Set(GameFlagIds.MinoChefBakeryProcCounter, CoC.Instance.Flags.Get(GameFlagIds.MinoChefBakeryProcCounter) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.MaddieStatus) == 0 && CoC.Instance.Flags.Get(GameFlagIds.MinoChefBakeryProcCounter) % 8 == 0)
            {
                SceneInstance<MaddieScene>.Get().ProcMaddieOneIntro();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MaddieStatus) == 3)
            {
                SceneInstance<MaddieScene>.Get().BakeryEpilogue();

                return ;
            }

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesVisitedBakery) == 0)
            {
                OutputText("Scenes.Bakeryscene.Bakeryuuuuuu.BakerySunk".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaCovenQueenCrowned) == 1 && Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                    if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 1)
                        OutputText("Scenes.Bakeryscene.Bakeryuuuuuu.JulienneAgain".Localized());
                    else
                        OutputText("Scenes.Bakeryscene.Bakeryuuuuuu.NoticesMouseGirl".Localized());
            }
            else
            {
                if (Holidays.IsWinter())
                {
                    SceneInstance<XmasMiscScene>.Get().EncounterKamiTheChristmasRoo();

                    if (CoC.Instance.Flags.Get(GameFlagIds.KamiEncounter) == 1)
                        AddButton(3, "Common.Button.Pudding".Localized(), () => SceneInstance<XmasMiscScene>.Get().GetWinterPudding());
                }
                else
                    OutputText("Scenes.Bakeryscene.Bakeryuuuuuu.DomedInterior".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) < 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaCovenQueenCrowned) == 1 && Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                    if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 1)
                        OutputText("Scenes.Bakeryscene.Bakeryuuuuuu.GazingLongingly".Localized());
                    else
                        OutputText("Scenes.Bakeryscene.Bakeryuuuuuu.NoticesMouseGirlAgain".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesVisitedBakery, CoC.Instance.Flags.Get(GameFlagIds.TimesVisitedBakery) + 1);

            OutputText("Scenes.Bakeryscene.Bakeryuuuuuu.WhatDoYouDo".Localized());

            AddButton(0, "Common.Button.CheckMenu".Localized(), CheckBakeryMenu);

            AddButton(1, "Common.Button.Talk".Localized(), TalkBakeryMenu);

            if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseOwned) < 1 && CoC.Instance.Flags.Get(GameFlagIds.NephilaCovenQueenCrowned) == 1 && Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10)
                if (CoC.Instance.Flags.Get(GameFlagIds.NephilaMouseMet) == 1)
                    AddButton(2, "Common.Button.Approachjulienne".Localized(), NephilaMouseAgain);
                else
                    AddButton(2, "Common.Button.Approachmouse".Localized(), NephilaMouse);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void NephilaMouse()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Bakeryscene.NephilaMouse.BabiesCart".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 20)
                OutputText("Scenes.Bakeryscene.NephilaMouse.SimulacrumBulk".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 15)
                    OutputText("Scenes.Bakeryscene.NephilaMouse.RoomFillingBulk".Localized());
                else
                    OutputText("Scenes.Bakeryscene.NephilaMouse.CartSizedBelly".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouse.AlreadyBlushing".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouse.MouseSputters".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouse.AssureAlright".Localized());

            if (Player.Thickness <= 33)
                OutputText("Scenes.Bakeryscene.NephilaMouse.TightlyPacked".Localized());
            else
                if (Player.Thickness <= 66)
                    OutputText("Scenes.Bakeryscene.NephilaMouse.StretchedStomach".Localized());
                else
                    OutputText("Scenes.Bakeryscene.NephilaMouse.PlushMotherly".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouse.ARub".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouse.ReallyAsks".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouse.LooksHerOver".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouse.SoBeautiful".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Politelydecline".Localized(), NephilaMouseFirstDecline);

            AddButton(2, "Common.Button.Accept".Localized(), NephilaMouseFirstAccept);
        }

        public void NephilaMouseFirstDecline()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstDecline.BusyCannot".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstDecline.SorryJust".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstDecline.NodsNoncommittally".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CheckBakeryMenu);

            return ;
        }

        public void NephilaMouseFirstAccept()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.GraciouslyAccept".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.SituatedSideways".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.CutsBrownies".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.LookHowHungry".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.IntroducesSelf".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.DontFindForward".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.MeAsks".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.MovedToFront".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.AlmostPerfect".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.WhinesBlubbers".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.ListeningMinutes".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.ShakingKnees".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.YourBabies".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.NuzzlesNeck".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseFirstAccept.RegainedStrength".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 3f));

            Player.Thickness += 1 + Random.Shared.Next(2);

            CoC.Instance.Flags.Set(GameFlagIds.NephilaMouseMet, 1);

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void NephilaMouseAgain()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Bakeryscene.NephilaMouseAgain.BabiesCart".Localized());

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 20)
                OutputText("Scenes.Bakeryscene.NephilaMouseAgain.SimulacrumBulk".Localized());
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 15)
                    OutputText("Scenes.Bakeryscene.NephilaMouseAgain.RoomFillingBulk".Localized());
                else
                    OutputText("Scenes.Bakeryscene.NephilaMouseAgain.CartSizedBelly".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseAgain.SmilesApproach".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseAgain.ForLuck".Localized());

            ClearButtons();

            AddButton(1, "Common.Button.Politelydecline".Localized(), NephilaMouseRepeatDecline);

            AddButton(2, "Common.Button.Accept".Localized(), NephilaMouseRepeatAccept);

            if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 15 && CoC.Instance.Flags.Get(GameFlagIds.NephilaSecondAscension) == 1)
                AddButton(3, "Common.Button.Cureinfertility".Localized(), null );
            else
                AddButton(3, "Common.Button.Cureinfertility".Localized(), null);
        }

        public void NephilaMouseRepeatDecline()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatDecline.BusyMoment".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CheckBakeryMenu);

            return ;
        }

        public void NephilaMouseRepeatAccept()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.GraciouslyAccept".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.TrioAssistants".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.EyesDeceiving".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.TakesSignal".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.BreathyRagged".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.StopsTorturing".Localized());

            if (Player.Thickness <= 33)
                OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.StretchedThin".Localized());
            else
                if (Player.Thickness <= 66)
                    OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.TentaclePacked".Localized());
                else
                    OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.SoftTentacleGut".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.CrossesCrest".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.WatchesEat".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.LookAtYouEat".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.VoiceTurnedSoft".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.TentaclesLolling".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.GivesWink".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.TentaclesSwarm".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.EventuallyOrgasm".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.ShimmiesForward".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.EyesShiny".Localized());

            OutputText("Scenes.Bakeryscene.NephilaMouseRepeatAccept.JulienneDisappears".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, 3f), new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Corruption, 3f));

            Player.Thickness += 1 + Random.Shared.Next(2);

            Player.Orgasm("Vaginal");

            ReturnToCamp(1);
        }

        public void CheckBakeryMenu()
        {
            ClearOutput();

            if (Player.HasPerk(PerkIds.MinotaurCumAddict) && CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumEclairUnlocked) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.MinotaurCumEclairUnlocked, CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumEclairUnlocked) + 1);

                OutputText("Scenes.Bakeryscene.CheckBakeryMenu.InLineCentauress".Localized());
            }

            OutputText("Scenes.Bakeryscene.CheckBakeryMenu.ChocolateBrownies".Localized());

            OutputText("Scenes.Bakeryscene.CheckBakeryMenu.FigCookies".Localized());

            OutputText("Scenes.Bakeryscene.CheckBakeryMenu.BerryCupcakes".Localized());

            OutputText("Scenes.Bakeryscene.CheckBakeryMenu.DoughnutsItem".Localized());

            OutputText("Scenes.Bakeryscene.CheckBakeryMenu.PoundCake".Localized());

            AddButton(0, "Common.Button.Brownies".Localized(), () => Nomnomnom("brownies", 3));

            AddButton(1, "Common.Button.Cookies".Localized(), () => Nomnomnom("cookies", 4));

            AddButton(2, "Common.Button.Cupcakes".Localized(), () => Nomnomnom("cupcakes", 3));

            AddButton(3, "Common.Button.Doughnuts".Localized(), () => Nomnomnom("doughnuts", 5));

            AddButton(4, "Common.Button.PoundCake".Localized(), () => Nomnomnom("pound cake", 4));

            if (survival)
            {
                OutputText("Scenes.Bakeryscene.CheckBakeryMenu.HardBiscuits".Localized());

                OutputText("Scenes.Bakeryscene.CheckBakeryMenu.TrailMix".Localized());

                AddButton(5, "Items.HardBiscuits.Short".Localized(), BuyHardBiscuits);

                AddButton(6, "Items.TrailMix.Short".Localized(), BuyTrailMix);
            }

            if (View.Time.Days % 7 == 0)
            {
                OutputText("Scenes.Bakeryscene.CheckBakeryMenu.HummusSpecial".Localized());

                AddButton(7, "Common.Button.Hummus".Localized(), BuyHummus);
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumEclairUnlocked) > 0)
            {
                OutputText("Scenes.Bakeryscene.CheckBakeryMenu.SpecialEclair".Localized());

                AddButton(8, "Common.Button.Specialeclair".Localized(), () => Nomnomnom("eclair", 10));
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MaddieStatus) >= 4)
            {
                OutputText("Scenes.Bakeryscene.CheckBakeryMenu.GiantCupcake".Localized());

                AddButton(9, "Common.Button.Giantcupcake".Localized(), BuySlutCake);
            }

            OutputText("Scenes.Bakeryscene.CheckBakeryMenu.LineBreak".Localized());

            DisplayIngredients();

            OutputText("Scenes.Bakeryscene.CheckBakeryMenu.WhatOrder".Localized());

            AddButton(10, "Common.Button.Ingredients".Localized(), IngredientsMenu);

            AddButton(14, "Common.Button.Leave".Localized(), Bakeryuuuuuu);
        }

        public void DisplayIngredients()
        {

            OutputText("Scenes.Bakeryscene.DisplayIngredients.SpecialIngredients".Localized());

            OutputText("Scenes.Bakeryscene.DisplayIngredients.FoxBerryItem".Localized());

            OutputText("Scenes.Bakeryscene.DisplayIngredients.RingtailFig".Localized());

            OutputText("Scenes.Bakeryscene.DisplayIngredients.MouseCocoaItem".Localized());

            OutputText("Scenes.Bakeryscene.DisplayIngredients.RedRiverRootItem".Localized());

            OutputText("Scenes.Bakeryscene.DisplayIngredients.FerretFruitItem".Localized());
        }

        public void IngredientsMenu()
        {
            ClearOutput();

            DisplayIngredients();

            ClearButtons();

            AddButton(0, "Items.FoxBerry.Short".Localized(), BuyFoxBerry);

            AddButton(1, "Items.RingtailFig.Short".Localized(), BuyFig);

            AddButton(2, "Items.MouseCocoa.Short".Localized(), BuyCocoa);

            AddButton(3, "Common.Button.RRvrRoot".Localized(), BuyRoot);

            AddButton(4, "Items.FerretFruit.Short".Localized(), BuyFerretFruit);

            AddButton(14, "Common.Button.Back".Localized(), CheckBakeryMenu);
        }

        public void TalkToBaker()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkToBaker.MinotaurSnorts".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MaddieStatus) >= 4)
                OutputText("Scenes.Bakeryscene.TalkToBaker.SpecialOrder".Localized());

            OutputText("Scenes.Bakeryscene.TalkToBaker.SayEnd".Localized());

            OutputText("Scenes.Bakeryscene.TalkToBaker.UnrefinedAppearance".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Brownie".Localized(), TalkAboutBrownies);

            AddButton(1, "Common.Button.Cookie".Localized(), TalkAboutCookies);

            AddButton(2, "Common.Button.Cupcake".Localized(), TalkAboutCupcakes);

            AddButton(3, "Common.Button.Doughnut".Localized(), TalkAboutDoughnuts);

            AddButton(4, "Common.Button.PoundCake".Localized(), TalkToBakerAboutPoundCake);

            AddButton(5, "Items.FoxBerry.Short".Localized(), TalkAboutFoxBerry);

            AddButton(6, "Items.RingtailFig.Short".Localized(), TalkAFig);

            AddButton(7, "Items.MouseCocoa.Short".Localized(), TalkAboutMouseCocoa);

            AddButton(8, "Common.Button.RRvrRoot".Localized(), TalkAboutRoot);

            AddButton(14, "Scenes.CorruptedGlade.NeverMind".Localized(), TalkBakeryMenu);
        }

        public void TalkAboutBrownies()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAboutBrownies.LikeOurBrownies".Localized());

            OutputText("Scenes.Bakeryscene.TalkAboutBrownies.HeContinues".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => Nomnomnom("brownies", 3));

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void TalkAboutCookies()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAboutCookies.NodsAtYou".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => Nomnomnom("cookies", 4));

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void TalkAboutCupcakes()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAboutCupcakes.CupcakesTakeWork".Localized());

            OutputText("Scenes.Bakeryscene.TalkAboutCupcakes.HeSighs".Localized());

            OutputText("Scenes.Bakeryscene.TalkAboutCupcakes.TooPopular".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => Nomnomnom("cupcakes", 3));

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void TalkAboutDoughnuts()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAboutDoughnuts.DoughnutsAreFun".Localized());

            OutputText("Scenes.Bakeryscene.TalkAboutDoughnuts.HeFrowns".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => Nomnomnom("doughnuts", 5));

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void TalkToBakerAboutPoundCake()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkToBakerAboutPoundCake.SnortsAgain".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => Nomnomnom("pound cake", 4));

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void TalkAboutFoxBerry()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAboutFoxBerry.FromWhorehouse".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), BuyFoxBerry);

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void TalkAFig()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAFig.BorderSwamp".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), BuyFig);

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void TalkAboutRoot()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAboutRoot.NotRed".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), BuyRoot);

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);

            if (CoC.Instance.Flags.Get(GameFlagIds.MinoChefTalkedRedRiverRoot) < 0)
                CoC.Instance.Flags.Set(GameFlagIds.MinoChefTalkedRedRiverRoot, 0);

            CoC.Instance.Flags.Set(GameFlagIds.MinoChefTalkedRedRiverRoot, CoC.Instance.Flags.Get(GameFlagIds.MinoChefTalkedRedRiverRoot) + 1);
        }

        public void TalkAboutMouseCocoa()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkAboutMouseCocoa.WarmSideLake".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), BuyCocoa);

            AddButton(1, "Common.Button.No".Localized(), TalkToBaker);
        }

        public void BuyCocoa()
        {
            ClearOutput();

            if (Player.Gems < 10)
            {
                OutputText("Scenes.Bakeryscene.BuyCocoa.CantAfford".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), IngredientsMenu);

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyCocoa.PayTenGems".Localized());

            Player.Gems -= 10;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.MouseCocoa), text => OutputText(text), Random.Shared);

                IngredientsMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.MouseCocoa), 1);

                IngredientsMenu();
            }
        }

        public void BuyFerretFruit()
        {
            ClearOutput();

            if (Player.Gems < 20)
            {
                OutputText("Scenes.Bakeryscene.BuyFerretFruit.CantAfford".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), IngredientsMenu);

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyFerretFruit.PayTwentyGems".Localized());

            Player.Gems -= 20;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.Frrtfrt), text => OutputText(text), Random.Shared);

                IngredientsMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Frrtfrt), 1);

IngredientsMenu();
            }
        }

        public void BuyFig()
        {
            ClearOutput();

            if (Player.Gems < 5)
            {
                OutputText("Scenes.Bakeryscene.BuyFig.CantAfford".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), IngredientsMenu);

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyFig.PayFiveGems".Localized());

            Player.Gems -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.RingtailFig), text => OutputText(text), Random.Shared);

                IngredientsMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.RingtailFig), 1);

                IngredientsMenu();
            }
        }

        public void BuyRoot()
        {
            ClearOutput();

            if (Player.Gems < 14)
            {
                OutputText("Scenes.Bakeryscene.BuyRoot.CantAfford".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), IngredientsMenu);

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyRoot.PayFourteenGems".Localized());

            Player.Gems -= 14;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.RiverRoot), text => OutputText(text), Random.Shared);

                IngredientsMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.RiverRoot), 1);

                IngredientsMenu();
            }
        }

        public void TalkBakeryMenu()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.TalkBakeryMenu.WhoTalk".Localized());

            var rubiT = "Waitress";

            if (CoC.Instance.Flags.Get(GameFlagIds.RubiIntroduced) > 0)
                rubiT = "Rubi";

            ClearButtons();

            AddButton(0, "Common.Button.Baker".Localized(), TalkToBaker);

            var rubiB = SceneInstance<RubiScene>.Get().RubiIntros();

            if (rubiB != null)
                AddButton(1, rubiT, rubiB);

            if (Holidays.IsWinter())
                if (CoC.Instance.Flags.Get(GameFlagIds.KamiEncounter) > 0)
                {
                    OutputText("Scenes.Bakeryscene.TalkBakeryMenu.BurnOffSteam".Localized());

                    AddButton(2, "Common.Button.Kami".Localized(), null );
                }
                else
                {
                    OutputText("Scenes.Bakeryscene.TalkBakeryMenu.SummonKangaroo".Localized());

                    AddButton(2, "Common.Button.Kangaroo".Localized(), null );
                }

            OutputText("Scenes.Bakeryscene.TalkBakeryMenu.BubblegumPink".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetFrosty) != 0)
                AddButton(3, "Common.Button.Frosty".Localized(), null );
            else
                AddButton(3, "Common.Button.Pinkgirl".Localized(), null );

            AddButton(14, "Common.Button.Leave".Localized(), Bakeryuuuuuu);
        }

        public void Nomnomnom(string name, float price)
        {
            ClearOutput();

            if (Player.Gems < price)
            {
                if (Player.IsChild() && SceneInstance<RubiScene>.Get().RubiAffection() >= 30)
                    RubiTreat();
                else
                {
                    OutputText("Scenes.Bakeryscene.Nomnomnom.NotEnoughGems".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), CheckBakeryMenu);
                }

                return ;
            }

            Player.Gems -= (int)(price);

            if (name == "eclair")
            {
                OutputText("Scenes.Bakeryscene.Nomnomnom.HandsTenGems".Localized());

                if (Player.Tallness <= 52)
                    OutputText("Scenes.Bakeryscene.Nomnomnom.DownWord".Localized());
                else
                    if (Player.Tallness >= 84)
                        OutputText("Scenes.Bakeryscene.Nomnomnom.UpWord".Localized());

                OutputText("Scenes.Bakeryscene.Nomnomnom.PullsPastry".Localized());

                if (Player.Femininity >= 75)
                    OutputText("Scenes.Bakeryscene.Nomnomnom.MalesFillPants".Localized());
                else
                    if (Player.Femininity <= 25)
                        OutputText("Scenes.Bakeryscene.Nomnomnom.FemalesSquirm".Localized());
                    else
                        OutputText("Scenes.Bakeryscene.Nomnomnom.OtherPatrons".Localized());

                OutputText("Scenes.Bakeryscene.Nomnomnom.MakeAMove".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(20 + Player.Libido / 10)));

                Player.MinoCumAddiction(10);

                Player.RefillHunger(20);
            }
            else
            {
                OutputText("Scenes.Bakeryscene.Nomnomnom.HandsGems".Localized(
                    NumberUtils.Num2Text(price),

                    name
                ));

                if (name == "doughnuts")
                {

                    OutputText(Player.ModThickness(100, 1));

                    if (Random.Shared.Next(3) == 0 && Player.Body.Butt < 15 && (Player.Hunger > 25 || !survival))
                    {
                        OutputText("Scenes.Bakeryscene.Nomnomnom.AssJiggles".Localized());

                        Player.Body.Butt += 1;
                    }

                    if (Random.Shared.Next(3) == 0 && Player.Body.Hips < 15 && (Player.Hunger > 25 || !survival))
                    {
                        OutputText("Scenes.Bakeryscene.Nomnomnom.GaitChanged".Localized());

                        Player.Body.Hips += 1;
                    }

                    Player.RefillHunger(25);
                }
                else
                    if (name == "cookies")
                    {

                        OutputText(Player.ModThickness(100, 2));

                        if (Random.Shared.Next(3) == 0 && Player.Body.Hips < 20 && (Player.Hunger > 25 || !survival))
                        {
                            OutputText("Scenes.Bakeryscene.Nomnomnom.GaitChangedAgain".Localized());

                            Player.Body.Hips += 1;
                        }

                        Player.RefillHunger(20);
                    }
                    else
                        if (name == "brownies")
                        {
                            OutputText(Player.ModThickness(100, 4));

                            if (Random.Shared.Next(2) == 0 && Player.Body.Hips < 30 && (Player.Hunger > 25 || !survival))
                            {
                                OutputText("Scenes.Bakeryscene.Nomnomnom.GaitChangedThird".Localized());

                                Player.Body.Hips += 2;
                            }

                            Player.RefillHunger(20);
                        }
                        else
                            if (name == "cupcakes")
                            {

                                if (Random.Shared.Next(2) == 0 && Player.Body.Butt < 30 && (Player.Hunger > 25 || !survival))
                                {
                                    OutputText("Scenes.Bakeryscene.Nomnomnom.AssJigglesAgain".Localized());

                                    Player.Body.Butt += 2;
                                }

                                Player.RefillHunger(20);
                            }
                            else
                                if (name == "pound cake")
                                {

                                    OutputText(Player.ModThickness(100, 2));

                                    if (Random.Shared.Next(3) == 0 && Player.Body.Butt < 25 && (Player.Hunger > 25 || !survival))
                                    {
                                        OutputText("Scenes.Bakeryscene.Nomnomnom.AssJigglesThird".Localized());

                                        Player.Body.Butt += 1;
                                    }

                                    if (Random.Shared.Next(3) == 0 && Player.Body.Hips < 25 && (Player.Hunger > 25 || !survival))
                                    {
                                        OutputText("Scenes.Bakeryscene.Nomnomnom.GaitChangedFourth".Localized());

                                        Player.Body.Hips += 1;
                                    }

                                    Player.RefillHunger(50);
                                }
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), CheckBakeryMenu);
        }

        public void BuySlutCake()
        {
            ClearOutput();

            if (Player.Gems < 500)
            {
                OutputText("Scenes.Bakeryscene.BuySlutCake.NotEnoughGems".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), CheckBakeryMenu);

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuySlutCake.MinotaurChef".Localized());

            Player.Gems -= 500;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Cupcake), 1);

Bakeryuuuuuu();
        }

        public void BuyFoxBerry()
        {
            ClearOutput();

            if (Player.Gems < 5)
            {
                OutputText("Scenes.Bakeryscene.BuyFoxBerry.CantAfford".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), IngredientsMenu);

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyFoxBerry.PayFiveGems".Localized());

            Player.Gems -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.FoxBerry), text => OutputText(text), Random.Shared);

                IngredientsMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.FoxBerry), 1);

IngredientsMenu();
            }
        }

        public void BuyHardBiscuits()
        {
            ClearOutput();

            if (Player.Gems < 5)
            {
                OutputText("Scenes.Bakeryscene.BuyHardBiscuits.CantAfford".Localized());

                CheckBakeryMenu();

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyHardBiscuits.PayFiveGems".Localized());

            Player.Gems -= 5;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.Biscuit), text => OutputText(text), Random.Shared);

                CheckBakeryMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.Biscuit), 1);

CheckBakeryMenu();
            }
        }

        public void BuyTrailMix()
        {
            ClearOutput();

            if (Player.Gems < 20)
            {
                OutputText("Scenes.Bakeryscene.BuyTrailMix.CantAfford".Localized());

                CheckBakeryMenu();

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyTrailMix.PayTwentyGems".Localized());

            Player.Gems -= 20;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.TrailMix), text => OutputText(text), Random.Shared);

                CheckBakeryMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.TrailMix), 1);

CheckBakeryMenu();
            }
        }

        public void BuyHummus()
        {
            ClearOutput();

            if (Player.Gems < 100)
            {
                OutputText("Scenes.Bakeryscene.BuyHummus.CantAfford".Localized());

                CheckBakeryMenu();

                return ;
            }

            OutputText("Scenes.Bakeryscene.BuyHummus.PayHundredGems".Localized());

            Player.Gems -= 100;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.HUMMUS_), text => OutputText(text), Random.Shared);

                CheckBakeryMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.HUMMUS_), 1);

                CheckBakeryMenu();
            }
        }

        public void EasterBakeSale()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.EasterBakeSale.MakeWayBakery".Localized());

            if (SceneInstance<RubiScene>.Get().RubiAffection() >= 40)
                OutputText("Scenes.Bakeryscene.EasterBakeSale.BusyRubi".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSale.HolidayBustle".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSale.ComeHereOften".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.CheckMenu".Localized(), CheckBakeryMenu);

            AddButton(1, "Common.Button.OfferHelp".Localized(), EasterBakeSaleHelp);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void EasterBakeSaleHelp()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.DeterminedHelp".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.HeadIntoBack".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.ExasperatedHelp".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.StillUnsure".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.EuphoricWave".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.BreastsBounce".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.ElaboratePuzzle".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.EggAlters".Localized());

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.JumpsHardness".Localized());
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.NipplesStiffening".Localized());
                else
                    OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.NotFarBehind".Localized());

            OutputText("Scenes.Bakeryscene.EasterBakeSaleHelp.EuphoriaFades".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), MalesHelpOutWithEaster);
        }

        public void MalesHelpOutWithEaster()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.IdeaCrosses".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.StickyChocolate".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.NotUnpleasant".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.ContainLust".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.MatePeak".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.ChocolateScreaming".Localized());

            if (silly)
                OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.ChocolateRain".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.RodSecondsBehind".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.SpitsStreams".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.ShootsStreams".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.SpewsAmount".Localized());
                    else
                        if (Player.CumQ() < 5000)
                            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.SpewsDeepness".Localized());
                        else
                            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.OpensLikeRiver".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.AllYouCanDo".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.LaterBanner".Localized());

            OutputText("Scenes.Bakeryscene.MalesHelpOutWithEaster.StumblesCamp".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f));

            Player.CumMultiplier += 2;

            ReturnToCamp(1);
        }

        public void RubiTreat()
        {
            ClearOutput();

            OutputText("Scenes.Bakeryscene.RubiTreat.RichScents".Localized());

            OutputText("Scenes.Bakeryscene.RubiTreat.RustleFabric".Localized(
                RandomUtils.Choice(["heavy footsteps", "click of heels", "clatter of hooves"])
            ));

            OutputText("Scenes.Bakeryscene.RubiTreat.BakerLumbers".Localized());

            OutputText("Scenes.Bakeryscene.RubiTreat.MeatyFinger".Localized());

            OutputText("Scenes.Bakeryscene.RubiTreat.DrawsRubi".Localized());

            OutputText("Scenes.Bakeryscene.RubiTreat.AsksBeforeNod".Localized());

            OutputText("Scenes.Bakeryscene.RubiTreat.OneOfHands".Localized());

            Player.Gems += 5;

        }

    }
}