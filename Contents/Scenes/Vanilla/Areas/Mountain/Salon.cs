using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Scenes.Vanilla.Areas.Mountain
{
    [Register<Salon>("coc:salon")]
    public class Salon : Scene
    {

        public override void Entry()
        {
            HairDresser();
        }

        public void HairDresser()
        {

            OutputText("Scenes.Salon.HairDresser.WhileExploringMountain".Localized());

            AddButton(0, "Common.Button.Yes".Localized(), () => SalonGreeting()); AddButton(1, "No", () => ReturnToCamp(1));
        }

        public bool IsDiscovered()
        {

            return Player.HasStatusEffect(StatusEffectIds.HairdresserMeeting);
        }

        public void SalonGreeting()
        {

            if (Player.HasStatusEffect(StatusEffectIds.HairdresserMeeting))
                HairDresserRepeatGreeting();
            else
            {
                Player.CreateStatusEffect(StatusEffectIds.HairdresserMeeting, 0, 0, 0, 0);

                HairDresserGreeting();
            }
        }

        public void FavoriteSalonMenu()
        {

            SalonPurchaseMenu();

            AddNextButton("Common.Button.Payments".Localized(), () => SalonPaymentMenu(true));
        }

        public void SalonPaymentMenu(bool favorite = false)
        {

            ClearButtons();

            AddNextButton("Common.Button.GoblinBlow".Localized(), GoblinHairDresserFacefuck);

            AddNextButton("Common.Button.Canine".Localized(), GloryholeDoggie);

            AddNextButton("Common.Button.Imp".Localized(), GloryholeImp);

            AddNextButton("Codex.Entry.Minotaurs.Name".Localized(), GloryholeMinotaur);

            AddNextButton("Common.Button.Incubus".Localized(), GloryholeIncubus);

            AddNextButton("Common.Button.FuckGoblin".Localized(), FuckLynnette);

            AddNextButton("Common.Button.GiveCum".Localized(), GiveCumOptions);

            if (favorite)
                AddButton(14, "Common.Button.Back".Localized(), FavoriteSalonMenu);
            else
                SetExitButton("Common.Button.Back".Localized());
        }

        public void SalonPurchaseMenu(bool paid = false)
        {

            if (paid)
                CoC.Instance.Flags.Set(GameFlagIds.SalonPaid, 1);

            var mudFacialEnabled = false;

            var sandFacialEnabled = false;

            if (Player.Femininity < 100 && Player.Gender == Gender.Female)
                mudFacialEnabled = true;
            else
                if (Player.Femininity < 85 && (Player.Gender == Gender.Genderless || Player.Gender == Gender.Hermaphrodite))
                    mudFacialEnabled = true;
                else
                    if (Player.Femininity < 70 && Player.Gender == Gender.Male)
                        mudFacialEnabled = true;
                    else
                        if (Player.Femininity < 100 && Player.HasPerk(PerkIds.Androgyny))
                            mudFacialEnabled = true;

            if (Player.Femininity > 0 && Player.Gender == Gender.Male)
                sandFacialEnabled = true;
            else
                if (Player.Femininity > 30 && Player.Gender == Gender.Female)
                    sandFacialEnabled = true;
                else
                    if (Player.Femininity > 20 && (Player.Gender == Gender.Genderless || Player.Gender == Gender.Hermaphrodite))
                        sandFacialEnabled = true;
                    else
                        if (Player.Femininity > 0 && Player.HasPerk(PerkIds.Androgyny))
                            sandFacialEnabled = true;

            ClearButtons();

            AddNextButton("Common.Button.CutShort".Localized(), CutShort);

            AddNextButton("Common.Button.CutMedium".Localized(), CutMedium);

            AddNextButton("Common.Button.CutLong".Localized(), CutLong);

            AddNextButton("Common.Button.Lengthen".Localized(), HairGrow);

            AddNextButton("Common.Button.RemoveHair".Localized(), RemoveHair);

            AddNextButton("Common.Button.BuyProducts".Localized(), ShopMenu);

            AddNextButton("Common.Button.BeardOptions".Localized(), BeardMenu);

            var mudFacialButton = AddNextButton("Common.Button.MudFacial".Localized(), MudFacial);

            mudFacialButton?.Hint("Scenes.Salon.Facial.MudHint".Localized());

            mudFacialButton?.DisableIf(!mudFacialEnabled, "Scenes.Salon.Facial.MudDisabled".Localized());

            var sandFacialButton = AddNextButton("Common.Button.SandFacial".Localized(), SandFacial);

            sandFacialButton?.Hint("Scenes.Salon.Facial.SandHint".Localized());

            sandFacialButton?.DisableIf(!sandFacialEnabled, "Scenes.Salon.Facial.SandDisabled".Localized());

            SetExitButton("Common.Button.Back".Localized());
        }

        public void HairDresserGreeting()
        {
            ClearOutput();

            ShowImage("location-salon");

            OutputText("Scenes.Salon.HairDresserGreeting.StepInsideCave".Localized());

            OutputText("Scenes.Salon.HairDresserGreeting.FewChairsAlong".Localized());

            OutputText("Scenes.Salon.HairDresserGreeting.PressesHerself".Localized());

            if (Player.CockTotal() == 0)
            {
                OutputText("Scenes.Salon.HairDresserGreeting.LookLikeDont".Localized());

                OutputText("Scenes.Salon.HairDresserGreeting.ShowsYouToBack".Localized());
            }
            else
            {
                OutputText("Scenes.Salon.HairDresserGreeting.IllEvenDoYou".Localized(
                     Player.Mf("kinky boy", "naughty girl")
                ));

                OutputText("Scenes.Salon.HairDresserGreeting.InVeryBack".Localized());
            }

            SalonPaymentMenu();

            OutputText("Scenes.Salon.HairDresserGreeting.SalonAddedToPlaces".Localized());
        }

        public void HairDresserRepeatGreeting()
        {
            ClearOutput();

            ShowImage("location-salon");

            if (Random.Shared.Next(5) == 0 && (Player.HasPerk(PerkIds.MinotaurCumAddict) || CoC.Instance.Flags.Get(GameFlagIds.MinotaurCumAddictionState) > 0))
            {
                MinotaurCumBukkakeInSalon();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.LynnettePregnancyCycle) == 4 && CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) > 0)
            {
                OutputText("Scenes.Salon.HairDresserRepeatGreeting.AsSoonAsEnter".Localized());

                if (Player.Tallness >= 48)
                    OutputText("Scenes.Salon.HairDresserRepeatGreeting.UpWord".Localized());

                OutputText("Scenes.Salon.HairDresserRepeatGreeting.ShePatsHerNow".Localized(
                     NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount))
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteBabyCount) > 0)
                    OutputText("Scenes.Salon.HairDresserRepeatGreeting.FewNewGreenFaces".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.LynnetteBabyCount, CoC.Instance.Flags.Get(GameFlagIds.LynnetteBabyCount) + CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount));

                CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, 0);

                if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteAnnouncedApproval) == 1)
                {
                    FavoriteSalonMenu();

                    return ;
                }
                else
                    if (Player.CockTotal() == 0)
                    {
                        OutputText("Scenes.Salon.HairDresserRepeatGreeting.TapsChinThought".Localized());

                        OutputText("Scenes.Salon.HairDresserRepeatGreeting.NumberOfDicks".Localized());
                    }
                    else
                        OutputText("Scenes.Salon.HairDresserRepeatGreeting.TapsChinThoughtAgain".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteApproval) >= 100 && CoC.Instance.Flags.Get(GameFlagIds.LynnetteAnnouncedApproval) == 0)
                {
                    OutputText("Scenes.Salon.HairDresserRepeatGreeting.LynnettePracticallyBeams".Localized());

                    OutputText("Scenes.Salon.HairDresserRepeatGreeting.QuiteTheDeal".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.LynnetteAnnouncedApproval, 1);

                    FavoriteSalonMenu();

                    return ;
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteAnnouncedApproval) == 1)
                    {
                        if (CoC.Instance.Flags.Get(GameFlagIds.LynnettePregnancyCycle) < 4 && CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) > 0)
                            OutputText("Scenes.Salon.HairDresserRepeatGreeting.DifficultyUshering".Localized());
                        else
                            if (CoC.Instance.Flags.Get(GameFlagIds.LynnettePregnancyCycle) < 4)
                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.GoblinAnswersDoor".Localized());
                            else
                            {
                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.PullingDoorRest".Localized());

                                if (Player.Tallness <= 48)
                                    OutputText("Scenes.Salon.HairDresserRepeatGreeting.SideWord".Localized());
                                else
                                    if (Player.Tallness <= 68)
                                        OutputText("Scenes.Salon.HairDresserRepeatGreeting.HipsWord".Localized());
                                    else
                                        OutputText("Scenes.Salon.HairDresserRepeatGreeting.LegWord".Localized());

                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.CompressingMammaries".Localized());

                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.HoneyedWordsFrom".Localized());
                            }

                        if (Player.CockTotal() == 0)
                            OutputText("Scenes.Salon.HairDresserRepeatGreeting.ExpressionDropsRealizes".Localized());

                        FavoriteSalonMenu();

                        return ;
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.LynnettePregnancyCycle) >= 4)
                        {
                            if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteMetUnpregnant) == 0)
                            {
                                CoC.Instance.Flags.Set(GameFlagIds.LynnetteMetUnpregnant, 1);

                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.FamiliarFaceGreets".Localized());

                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.GoblinCoosSashay".Localized());
                            }
                            else
                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.OpensDoorForYou".Localized());

                            if (Player.CockTotal() == 0)
                            {
                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.TapsChinThoughtThird".Localized());

                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.NumberOfDicksAgain".Localized());
                            }
                            else
                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.TapsChinThoughtFourth".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Salon.HairDresserRepeatGreeting.GoblinAnswersDoorAgain".Localized());

                            if (Player.CockTotal() == 0)
                            {
                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.StillCantPay".Localized());

                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.NumberOfDicksThird".Localized());
                            }
                            else
                                OutputText("Scenes.Salon.HairDresserRepeatGreeting.NormalOrFull".Localized());
                        }

            SalonPaymentMenu();
        }

        public void GloryholeImp()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Salon.GloryholeImp.WalkOverToHole".Localized());

            if (Math.Max(Player.Corruption, Math.Max(Player.Libido, Player.Lust)) < 33)
            {
                OutputText("Scenes.Salon.GloryholeImp.StruggleForceMouth".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 15f), new DynStat(GameStats.Corruption, .5f));
            }
            else
                if (Math.Max(Player.Corruption, Math.Max(Player.Libido, Player.Lust)) < 66)
                {
                    OutputText("Scenes.Salon.GloryholeImp.OpenWideBobbing".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 35f), new DynStat(GameStats.Corruption, 1f));
                }
                else
                {
                    OutputText("Scenes.Salon.GloryholeImp.DiveForwardsRelishing".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 45f), new DynStat(GameStats.Corruption, 2f));
                }

            Player.Orgasm("Lips", false);

            Player.RefillHunger(25);

            OutputText("Scenes.Salon.GloryholeImp.DemonDongSlips".Localized());

            HairDressingMainMenu(true);
        }

        public void GloryholeDoggie()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Salon.GloryholeDoggie.SighKneelDown".Localized());

            if (Math.Max(Player.Corruption, Math.Max(Player.Libido, Player.Lust)) < 33)
            {
                OutputText("Scenes.Salon.GloryholeDoggie.StruggleForceMouth".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 15f), new DynStat(GameStats.Corruption, .25f));
            }
            else
                if (Math.Max(Player.Corruption, Math.Max(Player.Libido, Player.Lust)) < 66)
                {
                    OutputText("Scenes.Salon.GloryholeDoggie.OpenWideTakeAs".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 25f), new DynStat(GameStats.Corruption, .25f));
                }
                else
                {
                    OutputText("Scenes.Salon.GloryholeDoggie.ThrowYourselfSwollen".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, 30f), new DynStat(GameStats.Corruption, .3f));
                }

            Player.Orgasm("Lips", false);

            Player.RefillHunger(10);

            OutputText("Scenes.Salon.GloryholeDoggie.YoungGoblinComes".Localized());

            HairDressingMainMenu(true);
        }

        public void GloryholeIncubus()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Salon.GloryholeIncubus.KneelDownFront".Localized());

            if (Math.Max(Player.Corruption, Math.Max(Player.Libido, Player.Lust)) < 33)
            {
                OutputText("Scenes.Salon.GloryholeIncubus.PickedThisDick".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 1f), new DynStat(GameStats.Corruption, 1.5f));
            }
            else
            {
                OutputText("Scenes.Salon.GloryholeIncubus.TakeCorruptedCock".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, 1f), new DynStat(GameStats.Corruption, 1f));
            }

            if (Player.Lust < 99)
                Player.Lust = 99;

            Player.Orgasm("Lips", false);

            Player.RefillHunger(35);

            HairDressingMainMenu(true);
        }

        public void GloryholeMinotaur()
        {
            ClearOutput();

            Player.SlimeFeed();

            OutputText("Scenes.Salon.GloryholeMinotaur.EyesDrawnToThe".Localized());

            OutputText("Scenes.Salon.GloryholeMinotaur.LargestAvailableMember".Localized());

            OutputText("Scenes.Salon.GloryholeMinotaur.DrivenOnByMinotaur".Localized(
                 Appearance.AllBreastsDescript(Player),

                 Appearance.BiggestBreastSizeDescript(Player)
            ));

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Salon.GloryholeMinotaur.HoldUpWord".Localized());
            else
                OutputText("Scenes.Salon.GloryholeMinotaur.ArchBackExpose".Localized());

            OutputText("Scenes.Salon.GloryholeMinotaur.AsCumRainsDown".Localized());

            OutputText("Scenes.Salon.GloryholeMinotaur.HeadSpinsMinotaur".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f), new DynStat(GameStats.Corruption, 1f));

            Player.Orgasm("Lips", false);

            Player.RefillHunger(30);

            Player.MinoCumAddiction(10);

            HairDressingMainMenu(true);
        }

        public void GiveCumOptions()
        {

            ClearButtons();

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.MinotaurCum)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedMinotaurCum)))
                AddButton(0, "Codex.Entry.Minotaurs.Name".Localized(), () => GiveCumToLynnette(0));

            if (Player.HasItem(ConsumableIds.UrtaCum))
                AddButton(1, "Common.Button.Urtas".Localized(), () => GiveCumToLynnette(1));

            AddButton(14, "Common.Button.Back".Localized(), () => SalonPaymentMenu());
        }

        public void GiveCumToLynnette(int type)
        {
            ClearOutput();

            OutputText("Scenes.Salon.GiveCumToLynnette.ShowBottleToLynnette".Localized((type == 0 ? "minotaur" : "Urta's")));

            if (type == 0)
            {
                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.MinotaurCum)))
                    Player.DestroyItems(ConsumableIds.MinotaurCum, 1);
                else
                    Player.DestroyItems(ConsumableIds.PurifiedMinotaurCum, 1);
            }
            else
                if (type == 1)
                    Player.DestroyItems(ConsumableIds.UrtaCum, 1);

            SalonPurchaseMenu(true);
        }

        public void GoblinHairDresserFacefuck()
        {
            ClearOutput();

            ShowImage("lynnette-blowjob");

            OutputText("Scenes.Salon.GoblinHairDresserFacefuck.LicksLipsPractically".Localized());

            if (Player.Cocks[0].Thickness >= 4)
                OutputText("Scenes.Salon.GoblinHairDresserFacefuck.AroundAsMuch".Localized());
            else
                OutputText("Scenes.Salon.GoblinHairDresserFacefuck.AroundFirstFewInches".Localized());

            OutputText("Scenes.Salon.GoblinHairDresserFacefuck.SlipsTongueDown".Localized());

            OutputText("Scenes.Salon.GoblinHairDresserFacefuck.ImpossibleToHoldBack".Localized());

            if (Player.CumQ() >= 250)
                OutputText("Scenes.Salon.GoblinHairDresserFacefuck.CantHoldItAll".Localized());

            OutputText("Scenes.Salon.GoblinHairDresserFacefuck.InTimeItEnds".Localized());

            OutputText("Scenes.Salon.GoblinHairDresserFacefuck.MatronSputters".Localized());

            Player.Orgasm("Dick");

            HairDressingMainMenu(true);
        }

        public void HairDressingMainMenu(bool paid = false)
        {
            ClearOutput();

            if (paid)
                CoC.Instance.Flags.Set(GameFlagIds.SalonPaid, 1);

            OutputText("Scenes.Salon.HairDressingMainMenu.OffersAndExplains".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.SalonMinocumUnlocked) > 0)
                OutputText("Scenes.Salon.HairDressingMainMenu.CouldAlwaysSpend".Localized());

            SalonPurchaseMenu();
        }

        public void SalonPay(int cost = 30, bool alwaysPay = false)
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.SalonPaid) != 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.SalonPaid, 0);

                if (alwaysPay)
                    Player.Gems -= cost;
            }
            else
                Player.Gems -= cost;

        }

        public void CutShort()
        {
            ClearOutput();

            if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Anemone)
            {
                OutputText("Scenes.Salon.CutShort.LynnetteStares".Localized());

                SalonPurchaseMenu();

                return ;
            }

            OutputText("Scenes.Salon.CutShort.DaughtersCrowdAround".Localized());

            Player.Body.Get<HairPart>().Length = 1;

            OutputText("Scenes.Salon.CutShort.HairPeriod".Localized());

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void CutMedium()
        {
            ClearOutput();

            if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Anemone)
            {
                OutputText("Scenes.Salon.CutMedium.LynnetteStares".Localized());

                SalonPurchaseMenu();

                return ;
            }

            OutputText("Scenes.Salon.CutMedium.DaughtersCrowdAround".Localized());

            Player.Body.Get<HairPart>().Length = 10;

            OutputText("Scenes.Salon.CutMedium.HairPeriod".Localized());

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void CutLong()
        {
            ClearOutput();

            if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Anemone)
            {
                OutputText("Scenes.Salon.CutLong.LynnetteStares".Localized());

                SalonPurchaseMenu();

                return ;
            }

            OutputText("Scenes.Salon.CutLong.DaughtersCrowdAround".Localized());

            Player.Body.Get<HairPart>().Length = 25;

            OutputText("Scenes.Salon.CutLong.HairPeriod".Localized());

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void HairGrow()
        {
            ClearOutput();

            if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Anemone)
            {
                OutputText("Scenes.Salon.HairGrow.LooksDubiously".Localized());

                SalonPurchaseMenu();

                return ;
            }

            OutputText("Scenes.Salon.HairGrow.GrabsBottleSquirts".Localized());

            var temp = Random.Shared.Next(3) + 3;

            CoC.Instance.Flags.Set(GameFlagIds.HairGrowthStoppedBecauseLizard, 0);

            Player.Body.Get<HairPart>().Length += temp;

            OutputText("Scenes.Salon.HairGrow.MoreInchesOfHair".Localized(
                 NumberUtils.Num2Text(temp)
            ));

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void RemoveHair()
        {
            ClearOutput();

            OutputText("Scenes.Salon.RemoveHair.TellThatYoud".Localized());

            OutputText("Scenes.Salon.RemoveHair.InstructsTakeSeat".Localized());

            OutputText("Scenes.Salon.RemoveHair.NoLongerHaveHair".Localized());

            Player.Body.Get<HairPart>().Length = 0;

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void BuyDye(Identifier itype, int cost = 30, bool alwaysPay = false)
        {
            ClearOutput();

            SalonPay(cost, alwaysPay);

            InventoryScene.TakeItem(ItemRegistry.Create(itype), ShopMenu);
        }

        public void ShopButton(string name, Identifier itype, int cost = 30, bool alwaysPay = false)
        {

            AddNextButton(name, () => BuyDye(itype, cost, alwaysPay));
        }

        public void ShopMenu()
        {
            ClearOutput();

            var paid = (CoC.Instance.Flags.Get(GameFlagIds.SalonPaid) == 1);

            var mino = (CoC.Instance.Flags.Get(GameFlagIds.SalonMinocumUnlocked) > 0);

            OutputText("Scenes.Salon.ShopMenu.PullsOpenCabinet".Localized());

            if (!paid)
                OutputText("Scenes.Salon.ShopMenu.PayThirtyGems".Localized());

            if (mino)
                OutputText("Scenes.Salon.ShopMenu.BuyMinotaurCum".Localized((paid ? "60" : "120")));

            ClearButtons();

            ShopButton(ItemRegistry.Create(ConsumableIds.HairDyeBlue).Name, ConsumableIds.HairDyeBlue);

            ShopButton(ItemRegistry.Create(ConsumableIds.HairDyeOrange).Name, ConsumableIds.HairDyeOrange);

            ShopButton(ItemRegistry.Create(ConsumableIds.HairDyePink).Name, ConsumableIds.HairDyePink);

            ShopButton(ItemRegistry.Create(ConsumableIds.HairDyePurple).Name, ConsumableIds.HairDyePurple);

            ShopButton(ItemRegistry.Create(ConsumableIds.HairDyeGreen).Name, ConsumableIds.HairDyeGreen);

            ShopButton(ItemRegistry.Create(ConsumableIds.HairSerum).Name, ConsumableIds.HairSerum);

            if (mino)
                ShopButton("Mino Cum", ConsumableIds.MinotaurCum, paid ? 60 : 120, true);

            if (paid)
                AddButton(14, "Common.Button.Back".Localized(), () => HairDressingMainMenu());
            else
                SetExitButton("Common.Button.Back".Localized());
        }

        public void BeardMenu()
        {

            ClearButtons();

            if (Player.IsChild() && !Player.HasBeard())
            {
                OutputText("Scenes.Salon.BeardMenu.LooksAtYourFace".Localized());

                OutputText("Scenes.Salon.BeardMenu.BitDisappointing".Localized());

                OutputText("Scenes.Salon.BeardMenu.BeardInsideCounts".Localized());

                OutputText("Scenes.Salon.BeardMenu.WorthKeepingInMind".Localized());

                AddButton(14, "Common.Button.Back".Localized(), () => HairDressingMainMenu());

                return ;
            }

            OutputText("Scenes.Salon.BeardMenu.LynnetteSays".Localized());

            AddButton(0, "Common.Button.CutBeard".Localized(), CutBeard);

            if (!Player.HasBeard())
                AddButton(1, "Common.Button.GrowBeard".Localized(), () => GrowBeard(1));
            else
                AddButton(1, "Common.Button.LengthenBeard".Localized(), () => GrowBeard(0));

            AddButton(2, "CharCreation.GenericStyleCustomizeMenu.Button3".Localized(), () => ChangeBeardStyle());

            AddButton(3, "Common.Button.RemoveBeard".Localized(), RemoveBeard);

            AddButton(14, "Common.Button.Back".Localized(), () => HairDressingMainMenu());
        }

        public void CutBeard()
        {
            ClearOutput();

            if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Anemone)
            {
                OutputText("Scenes.Salon.CutBeard.LynnetteStares".Localized());

                BeardMenu();

                return ;
            }

            OutputText("Scenes.Salon.CutBeard.DaughtersCrowdAround".Localized(
                 Player.BeardDescript()
            ));

            Player.Beard.Length = 0.01f;

            OutputText("Scenes.Salon.CutBeard.BeardPeriod".Localized(
                 Player.BeardDescript()
            ));

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void GrowBeard(int mode = 0)
        {
            ClearOutput();

            if (mode == 1)
            {
                if (Player.Mf("m", "f") == "f")
                {
                    OutputText("Scenes.Salon.GrowBeard.StaresSheAsks".Localized());

                    OutputText("Scenes.Salon.GrowBeard.InsistReally".Localized());

                    OutputText("Scenes.Salon.GrowBeard.GetStartedNow".Localized());
                }

                OutputText("Scenes.Salon.GrowBeard.GrabsBottleSquirts".Localized());

                Player.Beard.Length = 0.2f;

                OutputText("Scenes.Salon.GrowBeard.ChooseStyleBefore".Localized());

                ChangeBeardStyle(true);

                return ;
            }
            else
            {
                if (Player.Body.Get<HairPart>().Id == BodyPartTypes.Anemone)
                {
                    OutputText("Scenes.Salon.GrowBeard.LooksDubiously".Localized());

                    BeardMenu();

                    return ;
                }

                OutputText("Scenes.Salon.GrowBeard.GrabsBottleSquirtsAgain".Localized());

                var temp = (5 + Random.Shared.Next(5)) / 5;

                Player.Beard.Length += temp;

                OutputText("Scenes.Salon.GrowBeard.MoreInch".Localized(
                     NumberUtils.Num2Text(temp)
                ));

                if (temp >= 2)
                    OutputText("Scenes.Salon.GrowBeard.PluralEs".Localized());

                OutputText("Scenes.Salon.GrowBeard.OfBeard".Localized());
            }

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void ChangeBeardStyle(bool leave = false)
        {

            OutputText("Scenes.Salon.ChangeBeardStyle.WhatBeardStyle".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Normal".Localized(), () => ChooseBeardStyleFinalize(0));

            AddButton(1, "CharCreation.MenuBeardStyle.Button2".Localized(), () => ChooseBeardStyleFinalize(1));

            AddButton(2, "CharCreation.MenuBeardStyle.Button3".Localized(), () => ChooseBeardStyleFinalize(2));

            AddButton(3, "CharCreation.MenuBeardStyle.Button4".Localized(), () => ChooseBeardStyleFinalize(3));

            if (leave)
                SetExitButton("Common.Button.Back".Localized());
            else
                AddButton(14, "Common.Button.Back".Localized(), BeardMenu);
        }

        public void ChooseBeardStyleFinalize(int choiceStyle = 0)
        {
            ClearOutput();

            var beardTypes = new[] { "simple beard", "goatee", "clean shave", "mountainman cut" };

            OutputText("Scenes.Salon.ChooseBeardStyleFinalize.TellThatYoud".Localized(
                 beardTypes[choiceStyle]
            ));

            OutputText("Scenes.Salon.ChooseBeardStyleFinalize.DaughtersBeginTo".Localized());

            Player.Beard.Style = choiceStyle;

            OutputText("Scenes.Salon.ChooseBeardStyleFinalize.NowYouHave".Localized(
                 Player.BeardDescript()
            ));

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void RemoveBeard()
        {
            ClearOutput();

            OutputText("Scenes.Salon.RemoveBeard.TellThatYoud".Localized());

            OutputText("Scenes.Salon.RemoveBeard.InstructsTakeSeat".Localized(
                 Player.BeardDescript()
            ));

            OutputText("Scenes.Salon.RemoveBeard.NoLongerHaveBeard".Localized());

            Player.Beard.Style = BeardPart.Normal;

            Player.Beard.Length = 0;

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void MinotaurCumBukkakeInSalon()
        {
            ClearOutput();

            Player.MinoCumAddiction(10);

            Player.SlimeFeed();

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.SalonDoorSwingsClosed".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PuffingUpTwat".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.TurgidPulsating".Localized());
                else
                    OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.ParagraphBreak".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PushPastCrowd".Localized());

            if (Player.HasVagina() || Player.HasCock())
            {
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.TrailOfWord".Localized());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.FemaleSlimeWord".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.AndWord".Localized());
                }

                if (Player.HasCock())
                    if (Player.CumQ() < 100)
                        OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PreCumWord".Localized());
                    else
                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PreCumInchesWide".Localized());
                        else
                            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PreCumFootWide".Localized());

                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.WindsOverFloor".Localized());
            }

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.TimePassesSlow".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.DeepStrangledSigh".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.IncreasingSizeOf".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.BetweenGulpsFragrant".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.BeastPullsBack".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.EyesDriftClosed".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.TiedDownByCum".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.MoaningDrugInduced".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.BeastKeptLeft".Localized());

            if (Player.BiggestBreastSize() >= 2)
            {
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.WrapPillowyFlesh".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

                if (Player.BiggestBreastSize() < 6)
                    OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.EvenSoCantQuite".Localized());

                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.RunnelsOfPre".Localized());
            }
            else
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.CrushMemberTightly".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.ParagraphBreakThird".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.LowerRaiseBouncing".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.TitFuckingWord".Localized());
            else
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.FullBodyHandjob".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.GivingOutInSuch".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.GirthChangeSends".Localized(
                     Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PullOffBounceFaster".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.MinotaurNotDisappoint".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.AnimalPartOfYour".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PeriodBreak".Localized());

            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation, 70);

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.StaggerOverToThe".Localized());

            if (Player.AnalCapacity() < 80)
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.DelightingDrugNumbed".Localized());
            else
                if (Player.AnalCapacity() < 140)
                    OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.DelightingPerfectFullness".Localized());
                else
                    OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.DelightingRealizingCould".Localized());

            Player.ButtChange(90, true, false);

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.SlideDownTwitchingBull".Localized());

            if (!Player.HasCock())
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.DragsThroughBody".Localized());
            else
                OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.PressesOnProstate".Localized(
                     Player.SMultiCockDesc()
                ));

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.ParagraphBreakFourth".Localized());

            OutputText("Scenes.Salon.MinotaurCumBukkakeInSalon.StrengthGoesCompletely".Localized());

            Player.Orgasm("Lips", false);

            Player.Orgasm();

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
                Player.ChangeStatusValue(StatusEffectIds.ParasiteSlugReproduction, 1, 1);

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 2f));

            DoNext(() => MinotaurSalonFollowUp());
        }

        public void MinotaurSalonFollowUp()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.SalonMinocumUnlocked) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.SalonMinocumUnlocked, CoC.Instance.Flags.Get(GameFlagIds.SalonMinocumUnlocked) + 1);

                OutputText("Scenes.Salon.MinotaurSalonFollowUp.SlapsFaceWaking".Localized());

                OutputText("Scenes.Salon.MinotaurSalonFollowUp.GoblinAnswers".Localized());

                OutputText("Scenes.Salon.MinotaurSalonFollowUp.NodLittleShocked".Localized());
            }
            else
                OutputText("Scenes.Salon.MinotaurSalonFollowUp.WokenWithA".Localized());

            OutputText("Scenes.Salon.MinotaurSalonFollowUp.OffersAndExplains".Localized());

            SalonPurchaseMenu(true);
        }

        public void MudFacial()
        {
            ClearOutput();

            OutputText("Scenes.Salon.MudFacial.SitBackComfortable".Localized());

            OutputText("Scenes.Salon.MudFacial.CrowdOfGoblins".Localized());

            Player.ModFem(100, 10);

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void SandFacial()
        {
            ClearOutput();

            OutputText("Scenes.Salon.SandFacial.SitBackComfortable".Localized());

            OutputText("Scenes.Salon.SandFacial.GoblinGirls".Localized());

            Player.ModFem(0, 10);

            SalonPay();

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckLynnette()
        {
            ClearOutput();

            ShowImage("lynnette-fuck");

            if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteFuckCounter) == 0)
            {
                OutputText("Scenes.Salon.FuckLynnette.AtYourSuggestion".Localized());

                if (Player.Tallness >= 72)
                    OutputText("Scenes.Salon.FuckLynnette.LegWord".Localized());
                else
                    if (Player.Tallness >= 55)
                        OutputText("Scenes.Salon.FuckLynnette.HipsWord".Localized());
                    else
                        OutputText("Scenes.Salon.FuckLynnette.SideWord".Localized());

                OutputText("Scenes.Salon.FuckLynnette.SoSheCanSlip".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Salon.FuckLynnette.InTurnWord".Localized());

                OutputText("Scenes.Salon.FuckLynnette.SkillOfPracticed".Localized());

                if (Player.Balls == 0)
                    OutputText("Scenes.Salon.FuckLynnette.PressingSheath".Localized());
                else
                    OutputText("Scenes.Salon.FuckLynnette.CurlingPetiteFingers".Localized());

                OutputText("Scenes.Salon.FuckLynnette.SentencePeriod".Localized());

                if (Player.HoursSinceCum < 24)
                {
                    OutputText("Scenes.Salon.FuckLynnette.FeminineGiggle".Localized());

                    OutputText("Scenes.Salon.FuckLynnette.LicksScentFrom".Localized());

                    SalonPaymentMenu();

                    return ;
                }
            }
            else
            {
                OutputText("Scenes.Salon.FuckLynnette.LicksLipsAtYour".Localized());

                OutputText("Scenes.Salon.FuckLynnette.AppreciativeGroan".Localized());

                if (Player.LustPercent <= 75)
                    OutputText("Scenes.Salon.FuckLynnette.FillsFullSize".Localized());
                else
                    OutputText("Scenes.Salon.FuckLynnette.FullyErectState".Localized());

                OutputText("Scenes.Salon.FuckLynnette.StrokesFewTimes".Localized());

                if (Player.Balls == 0)
                    OutputText("Scenes.Salon.FuckLynnette.AroundToFeel".Localized());
                else
                    OutputText("Scenes.Salon.FuckLynnette.WeighingThemIn".Localized());

                OutputText("Scenes.Salon.FuckLynnette.SentencePeriodAgain".Localized());

                if (Player.HoursSinceCum < 24)
                {
                    OutputText("Scenes.Salon.FuckLynnette.FrownSpreads".Localized());

                    SalonPaymentMenu();

                    return ;
                }
            }

            OutputText("Scenes.Salon.FuckLynnette.CoosShivers".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Salon.FuckLynnette.BallsWord".Localized());
            else
                OutputText("Scenes.Salon.FuckLynnette.SheathWord".Localized());

            OutputText("Scenes.Salon.FuckLynnette.HungerInHerEyes".Localized());

            OutputText("Scenes.Salon.FuckLynnette.TossingDiscarded".Localized());

            OutputText("Scenes.Salon.FuckLynnette.NodKickDoor".Localized());

            OutputText("Scenes.Salon.FuckLynnette.LooseShoulderStrap".Localized());

            OutputText("Scenes.Salon.FuckLynnette.LikeABalloon".Localized());

            OutputText("Scenes.Salon.FuckLynnette.BedBounces".Localized());

            OutputText("Scenes.Salon.FuckLynnette.GoblinGrowlsDisplays".Localized());

            OutputText("Scenes.Salon.FuckLynnette.BeforeCrawlIntoBed".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Salon.FuckLynnette.EsHaveWord".Localized());
            else
                OutputText("Scenes.Salon.FuckLynnette.HasSpaceWord".Localized());

            OutputText("Scenes.Salon.FuckLynnette.BeadClearDroplets".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Salon.FuckLynnette.PluralS".Localized());

            OutputText("Scenes.Salon.FuckLynnette.RaceDownTowards".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Salon.FuckLynnette.BallsWordAgain".Localized());
            else
                OutputText("Scenes.Salon.FuckLynnette.SheathWordAgain".Localized());

            OutputText("Scenes.Salon.FuckLynnette.LeavingGlossyShine".Localized());

            OutputText("Scenes.Salon.FuckLynnette.PregnancyObsessedSlut".Localized());

            OutputText("Scenes.Salon.FuckLynnette.ClimbUpOntoBed".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Salon.FuckLynnette.PluralSAgain".Localized());

            OutputText("Scenes.Salon.FuckLynnette.WontTakeNo".Localized());

            var x = Player.CockThatFits(80);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Salon.FuckLynnette.GruntingAggressively".Localized(
                 Player.CockHead(x)
            ));

            if (Player.CockArea(x) > 50)
            {
                OutputText("Scenes.Salon.FuckLynnette.TummyBulgesOutline".Localized());

                if (Player.CockArea(x) <= 80)
                    OutputText("Scenes.Salon.FuckLynnette.HerBellyWord".Localized());
                else
                    OutputText("Scenes.Salon.FuckLynnette.IntoRibcage".Localized());
            }

            OutputText("Scenes.Salon.FuckLynnette.BreederGrabsHold".Localized());

            if (Player.CockArea(x) <= 90)
            {
                OutputText("Scenes.Salon.FuckLynnette.SlappingAgainst".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Salon.FuckLynnette.RestingOnSoakedAss".Localized());
            }
            else
            {
                OutputText("Scenes.Salon.FuckLynnette.EventuallyStopping".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Salon.FuckLynnette.FeetWrapAround".Localized());
            }

            OutputText("Scenes.Salon.FuckLynnette.SoGoddamnedWet".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Salon.FuckLynnette.LosingControl".Localized(
                 Player.CockDescript(x)
            ));

            OutputText("Scenes.Salon.FuckLynnette.BlissedOutEyelids".Localized());

            OutputText("Scenes.Salon.FuckLynnette.DontBlowYour".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Salon.FuckLynnette.SackTightAnd".Localized());
            else
                OutputText("Scenes.Salon.FuckLynnette.GutClenchSpasm".Localized());

            OutputText("Scenes.Salon.FuckLynnette.TsunamiOfSperm".Localized());

            OutputText("Scenes.Salon.FuckLynnette.MilkDrippingNipples".Localized());

            OutputText("Scenes.Salon.FuckLynnette.ThrustsTransform".Localized(
                 Player.CockHead(x)
            ));

            if (Player.CockArea(x) > 24)
                OutputText("Scenes.Salon.FuckLynnette.YieldingCervix".Localized());
            else
                OutputText("Scenes.Salon.FuckLynnette.CloseToCervix".Localized());

            OutputText("Scenes.Salon.FuckLynnette.GruntingGrowlingLose".Localized());

            OutputText("Scenes.Salon.FuckLynnette.LookingUpBetween".Localized());

            OutputText("Scenes.Salon.FuckLynnette.BodyReadyTo".Localized(
                 Player.CockDescript(x)
            ));

            if (Player.CockArea(x) <= 90)
                OutputText("Scenes.Salon.FuckLynnette.CompletelyInside".Localized());
            else
                OutputText("Scenes.Salon.FuckLynnette.DeeplyInsideAsYou".Localized());

            OutputText("Scenes.Salon.FuckLynnette.HoldItThere".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Salon.FuckLynnette.YourOtherMember".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Salon.FuckLynnette.SAreWord".Localized());
                else
                    OutputText("Scenes.Salon.FuckLynnette.IsSpaceWord".Localized());

                OutputText("Scenes.Salon.FuckLynnette.SpasmingWord".Localized());

                if (Player.BiggestCockLength() <= 12)
                    OutputText("Scenes.Salon.FuckLynnette.GreedyGreenSlutBelly".Localized());
                else
                    OutputText("Scenes.Salon.FuckLynnette.MilkStainedValley".Localized());

                OutputText("Scenes.Salon.FuckLynnette.JizzingWildSpurts".Localized());
            }

            OutputText("Scenes.Salon.FuckLynnette.ScreamingWithRelief".Localized());

            OutputText("Scenes.Salon.FuckLynnette.TinyHandsLatch".Localized());

            if (Player.CumQ() < 1000)
                OutputText("Scenes.Salon.FuckLynnette.InNoTimeFlat".Localized());
            else
                if (Player.CumQ() < 3000)
                    OutputText("Scenes.Salon.FuckLynnette.EnoughCumPouring".Localized());
                else
                    if (Player.CumQ() < 6000)
                        OutputText("Scenes.Salon.FuckLynnette.SoMuchCumFlowing".Localized(
                             Player.CockHead(x)
                        ));
                    else
                        if (Player.CumQ() < 10000)
                            OutputText("Scenes.Salon.FuckLynnette.OhGodsSoMuch".Localized());
                        else
                            OutputText("Scenes.Salon.FuckLynnette.EveryTorrentialOutflow".Localized(
                                 Player.CockDescript(x)
                            ));

            OutputText("Scenes.Salon.FuckLynnette.HoldsYouTight".Localized());

            if (Player.CumQ() >= 3000)
            {
                OutputText("Scenes.Salon.FuckLynnette.EvenIfWord".Localized());

                if (Player.CumQ() >= 10000)
                    OutputText("Scenes.Salon.FuckLynnette.MostWord".Localized());
                else
                    OutputText("Scenes.Salon.FuckLynnette.SomeWord".Localized());

                OutputText("Scenes.Salon.FuckLynnette.OfItOnBed".Localized());
            }

            OutputText("Scenes.Salon.FuckLynnette.OnlyThenRelease".Localized());

            if (Player.HasKnot(x))
                OutputText("Scenes.Salon.FuckLynnette.WetSoundingPop".Localized());
            else
                OutputText("Scenes.Salon.FuckLynnette.GushOfGirlspunk".Localized());

            OutputText("Scenes.Salon.FuckLynnette.SheGroansWord".Localized());

            if (Player.CumQ() < 1000)
            {
                OutputText("Scenes.Salon.FuckLynnette.ThatWasFun".Localized());

                OutputText("Scenes.Salon.FuckLynnette.LeaveHerFeeling".Localized());

                LynnetteApproval(-2);
            }
            else
                if (Player.CumQ() <= 10000)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteAnnouncedApproval) == 1)
                        OutputText("Scenes.Salon.FuckLynnette.CumLikeMinotaur".Localized());
                    else
                        OutputText("Scenes.Salon.FuckLynnette.LoveTheWayPack".Localized());

                    LynnetteApproval(10);
                }
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteAnnouncedApproval) == 0)
                        OutputText("Scenes.Salon.FuckLynnette.FeelItLikeI".Localized());
                    else
                        OutputText("Scenes.Salon.FuckLynnette.ColorsDeepGreen".Localized());

                    LynnetteApproval(25);
                }

            CoC.Instance.Flags.Set(GameFlagIds.LynnetteFuckCounter, CoC.Instance.Flags.Get(GameFlagIds.LynnetteFuckCounter) + 1);

            Player.Orgasm("Dick");

            CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, 3 + Random.Shared.Next(3));

            if (Player.CumQ() >= 1000)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) + 1 + Random.Shared.Next(3));

            if (Player.CumQ() >= 2000)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) + 1 + Random.Shared.Next(3));

            if (Player.CumQ() >= 3000)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) + 2 + Random.Shared.Next(3));

            if (Player.CumQ() >= 4000)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) + 2 + Random.Shared.Next(3));

            if (Player.CumQ() >= 6000)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) + 2 + Random.Shared.Next(3));

            if (Player.CumQ() >= 10000)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteCarryingCount, CoC.Instance.Flags.Get(GameFlagIds.LynnetteCarryingCount) + 2 + Random.Shared.Next(3));

            CoC.Instance.Flags.Set(GameFlagIds.LynnettePregnancyCycle, 0);

            SalonPurchaseMenu(true);
        }

        public float LynnetteApproval(int arg = 0)
        {

            if (arg != 0)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteApproval, CoC.Instance.Flags.Get(GameFlagIds.LynnetteApproval) + arg);

            if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteApproval) < -100)
                CoC.Instance.Flags.Set(GameFlagIds.LynnetteApproval, -100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.LynnetteApproval) > 100)
                    CoC.Instance.Flags.Set(GameFlagIds.LynnetteApproval, 100);

            return CoC.Instance.Flags.Get(GameFlagIds.LynnetteApproval);
        }

    }
}