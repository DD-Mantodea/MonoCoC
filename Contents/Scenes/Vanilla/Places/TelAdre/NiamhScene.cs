using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;
using MonoCoC.Contents.World;

namespace MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre
{
    [Register<NiamhScene>("coc:niamh")]
    public class NiamhScene : Scene, ITimeAware
    {
        // 原版 Niamh.hx:22 timeChange()：酿酒/搬走计数器、香槟与黑猫啤酒的每小时倒计时
        public bool TimeChange()
        {
            var flags = CoC.Instance.Flags;

            var player = CoC.Instance.Player;

            var needNext = false;

            if (flags.Get(GameFlagIds.NiamhSeanBrewBimboLiqueurCounter) > 1)
                flags.Set(GameFlagIds.NiamhSeanBrewBimboLiqueurCounter, flags.Get(GameFlagIds.NiamhSeanBrewBimboLiqueurCounter) - 1);

            if (flags.Get(GameFlagIds.NiamhMovedOutCounter) > 1)
                flags.Set(GameFlagIds.NiamhMovedOutCounter, flags.Get(GameFlagIds.NiamhMovedOutCounter) - 1);

            if (player.HasStatusEffect(StatusEffectIds.BimboChampagne))
            {
                player.ChangeStatusValue(StatusEffectIds.BimboChampagne, 0, -1);

                if (player.StatusEffectValue(StatusEffectIds.BimboChampagne, 0) <= 0)
                {
                    Contents.Items.Effects.Vanilla.BimboChampagneEffect.Remove(player, OutputText);

                    needNext = true;
                }
            }

            if (player.StatusEffectValue(StatusEffectIds.BlackCatBeer, 0) > 0)
            {
                player.ChangeStatusValue(StatusEffectIds.BlackCatBeer, 0, -1);

                if (player.StatusEffectValue(StatusEffectIds.BlackCatBeer, 0) <= 0)
                {
                    Contents.Items.Effects.Vanilla.BlackCatBeerEffect.Expire(player, OutputText);

                    needNext = true;
                }
            }

            return needNext;
        }

        public bool TimeChangeLarge() => false;

        public override void Entry()
        {
            NiamhSprite();
        }

        public void NiamhSprite()
        {

        }

        public void TelAdreNiamh()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) == 0)
            {
                OutputText("Scenes.Niamh.TelAdreNiamh.TableNearBack".Localized());

                if (View.Time.Hours <= 8)
                    OutputText("Scenes.Niamh.TelAdreNiamh.MassiveNCup".Localized());
                else
                    if (View.Time.Hours <= 9)
                        OutputText("Scenes.Niamh.TelAdreNiamh.HugeMCup".Localized());
                    else
                        if (View.Time.Hours <= 10)
                            OutputText("Scenes.Niamh.TelAdreNiamh.GreatLCup".Localized());
                        else
                            if (View.Time.Hours <= 11)
                                OutputText("Scenes.Niamh.TelAdreNiamh.HeavyKCup".Localized());
                            else
                                if (View.Time.Hours <= 12)
                                    OutputText("Scenes.Niamh.TelAdreNiamh.HeftyJCup".Localized());
                                else
                                    if (View.Time.Hours <= 13)
                                        OutputText("Scenes.Niamh.TelAdreNiamh.WeightyICup".Localized());
                                    else
                                        if (View.Time.Hours <= 14)
                                            OutputText("Scenes.Niamh.TelAdreNiamh.SqueezableHCup".Localized());
                                        else
                                            OutputText("Scenes.Niamh.TelAdreNiamh.ManageableGCup".Localized());

                OutputText("Scenes.Niamh.TelAdreNiamh.SmoothSkinChest".Localized());
            }
            else
            {
                OutputText("Scenes.Niamh.TelAdreNiamh.SeatedUsual".Localized());

                if (View.Time.Hours <= 8)
                    OutputText("Scenes.Niamh.TelAdreNiamh.TitanicNCupKegs".Localized());
                else
                    if (View.Time.Hours <= 14)
                    {
                        OutputText("Scenes.Niamh.TelAdreNiamh.HerWord".Localized());

                        if (View.Time.Hours <= 9)
                            OutputText("Scenes.Niamh.TelAdreNiamh.MCupWord".Localized());
                        else
                            if (View.Time.Hours <= 10)
                                OutputText("Scenes.Niamh.TelAdreNiamh.LCupWord".Localized());
                            else
                                if (View.Time.Hours <= 11)
                                    OutputText("Scenes.Niamh.TelAdreNiamh.KCupWord".Localized());
                                else
                                    if (View.Time.Hours <= 12)
                                        OutputText("Scenes.Niamh.TelAdreNiamh.JCupWord".Localized());
                                    else
                                        if (View.Time.Hours <= 13)
                                            OutputText("Scenes.Niamh.TelAdreNiamh.ICupWord".Localized());
                                        else
                                            OutputText("Scenes.Niamh.TelAdreNiamh.HCupWord".Localized());

                        OutputText("Scenes.Niamh.TelAdreNiamh.BrewBurdenedBreasts".Localized());
                    }
                    else
                        OutputText("Scenes.Niamh.TelAdreNiamh.GCupBeerBoobs".Localized());
            }
        }

        public void ApproachNiamh()
        {
            ClearOutput();

            NiamhSprite();

            ShowImage("niamh-approach-in-bar");

            if (Random.Shared.Next(5) == 0 && CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) > 0 && (Player.HasItem(ItemIds.BimboLiqueur) || Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk))))
            {
                CorruptOrBimboNiamhIntro();

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) > 0)
                OutputText("Scenes.Niamh.ApproachNiamh.GreetsFriendlyBut".Localized());
            else
                OutputText("Scenes.Niamh.ApproachNiamh.ApproachBurdenedWoman".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), TalkToNiamh);

            if (Player.Gems >= 2)
                AddButton(1, "Common.Button.GetBeer".Localized(), GetANiamhBeer);
            else
                AddButton(1, "Common.Button.GetBeer".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveNiamh);
        }

        public void GetANiamhBeer()
        {
            ClearOutput();

            ShowImage("niamh-get-beer");

            OutputText("Scenes.Niamh.GetANiamhBeer.RepliesHandOver".Localized());

            if (Player.HasPerk(PerkIds.Feeder))
                OutputText("Scenes.Niamh.GetANiamhBeer.UnderstandExactly".Localized());
            else
                if (Player.BiggestBreastSize() >= 15)
                {
                    if (Player.LactationQ() == 0)
                        OutputText("Scenes.Niamh.GetANiamhBeer.NotTerriblySurprising".Localized());
                    else
                        OutputText("Scenes.Niamh.GetANiamhBeer.EasilyImagine".Localized());
                }
                else
                    if (Player.LactationQ() > 0)
                        OutputText("Scenes.Niamh.GetANiamhBeer.KnowJustHowDistracting".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.GotNiamhBeer) == 0)
                OutputText("Scenes.Niamh.GetANiamhBeer.BrieflyDeliberate".Localized());
            else
                OutputText("Scenes.Niamh.GetANiamhBeer.RecallHowPleased".Localized());

            OutputText("Scenes.Niamh.GetANiamhBeer.GingerlyHandling".Localized());

            OutputText("Scenes.Niamh.GetANiamhBeer.HandsClampTo".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) > 0)
                OutputText("Scenes.Niamh.GetANiamhBeer.NiamhWord".Localized());
            else
                OutputText("Scenes.Niamh.GetANiamhBeer.CatGirlWord".Localized());

            OutputText("Scenes.Niamh.GetANiamhBeer.SighsInRelief".Localized());

            OutputText("Scenes.Niamh.GetANiamhBeer.ThinkMayHave".Localized());

            Player.Gems -= 2;

            CoC.Instance.Flags.Set(GameFlagIds.GotNiamhBeer, CoC.Instance.Flags.Get(GameFlagIds.GotNiamhBeer) + 1);

            ClearButtons();

            AddButton(0, "Common.Button.Here".Localized(), DrinkNiamhsBeerInTelAdre);

            if (Player.Gems >= 2)
                AddButton(1, "Common.Button.ToGo".Localized(), BlackCatBeerToGo);
            else
                AddButton(1, "Common.Button.ToGo".Localized(), null);
        }

        public void DrinkNiamhsBeerInTelAdre()
        {
            ClearOutput();

            if (Player.LactationQ() >= 300 && Player.BiggestBreastSize() >= 5 && Random.Shared.Next(2) == 0 && CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) > 0)
            {
                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.CheerDownThe".Localized());

                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.StartToStumble".Localized());

                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.CommentsFallsOpen".Localized());

                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.LeanBackAgainstBar".Localized());

                if (Holidays.IsSaturnalia() && DateTime.Now.Day >= 31)
                {
                    OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.GivesOtherNipple".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.PussyToWetten".Localized());

                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.AndWord".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.EachCockToStiffen".Localized());

                    OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.GotAnIdea".Localized());
                }
                else
                    if (DateTime.Now.Day == 17 && DateTime.Now.Month == 2)
                        OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.SmilesLiquidIn".Localized());
                    else
                        if (Holidays.IsSaturnalia())
                            OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.OffColorIndicates".Localized());
                        else
                            if ((DateTime.Now.Month == 2 && DateTime.Now.Day == 14))
                                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.AromaDistinct".Localized());
                            else
                                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.GivesOtherNippleAgain".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Sellyourbooze".Localized(), SellYourBooze);

                AddButton(14, "Common.Button.Leave".Localized(), LeaveWithBeerTits);

                return ;
            }

            OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.YellAndUpend".Localized());

            ItemRegistry.Create(ConsumableIds.BCBeer).OnUse();

            if (View.Time.Hours <= 15)
                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.NodHeadInThanks".Localized(
                     (Player.Gender == Gender.Genderless ? "customer" : Player.Mf("laddie", "lassie"))
                ));
            else
            {
                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.ThanksToEfforts".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) == 0)
                    OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.CatGirlPossessive".Localized());
                else
                    OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.NiamhPossessive".Localized());

                OutputText("Scenes.Niamh.DrinkNiamhsBeerInTelAdre.BreastsLastOf".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.MetNiamh, CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void BlackCatBeerToGo()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.BlackCatBeerToGo.GirlSaysFlirts".Localized());

            Player.Gems -= 2;

            if (View.Time.Hours <= 15)
                OutputText("Scenes.Niamh.BlackCatBeerToGo.NodHeadInThanks".Localized(
                     (Player.Gender == Gender.Genderless ? "customer" : Player.Mf("laddie", "lassie"))
                ));
            else
            {
                OutputText("Scenes.Niamh.BlackCatBeerToGo.ThanksToEfforts".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) == 0)
                    OutputText("Scenes.Niamh.BlackCatBeerToGo.CatGirlPossessive".Localized());
                else
                    OutputText("Scenes.Niamh.BlackCatBeerToGo.NiamhPossessive".Localized());

                OutputText("Scenes.Niamh.BlackCatBeerToGo.BreastsLastOf".Localized());
            }

            OutputText("Scenes.Niamh.BlackCatBeerToGo.ParagraphBreak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.MetNiamh, CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) + 1);

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.BCBeer), 1); Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void TalkToNiamh()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TalkedNiamh) == 0)
            {
                OutputText("Scenes.Niamh.TalkToNiamh.AskCatGirl".Localized());

                OutputText("Scenes.Niamh.TalkToNiamh.NamesNiamh".Localized());

                OutputText("Scenes.Niamh.TalkToNiamh.ShrugsImpressive".Localized());

                OutputText("Scenes.Niamh.TalkToNiamh.TrailsOffShakes".Localized());

                if (View.Time.Hours < 15)
                    OutputText("Scenes.Niamh.TalkToNiamh.WhoUsuallySits".Localized());

                OutputText("Scenes.Niamh.TalkToNiamh.HelpedMeIn".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.MetNiamh, CoC.Instance.Flags.Get(GameFlagIds.MetNiamh) + 1);

                CoC.Instance.Flags.Set(GameFlagIds.TalkedNiamh, CoC.Instance.Flags.Get(GameFlagIds.TalkedNiamh) + 1);
            }
            else
            {
                if (Player.IsTaur())
                    OutputText("Scenes.Niamh.TalkToNiamh.ClopUpStand".Localized());
                else
                    OutputText("Scenes.Niamh.TalkToNiamh.PullUpChair".Localized());

                if (Random.Shared.Next(2) == 0)
                    OutputText("Scenes.Niamh.TalkToNiamh.NextToNiamh".Localized());
                else
                    OutputText("Scenes.Niamh.TalkToNiamh.NextToNiamhAgain".Localized());
            }

            ClearButtons();

            if (Player.Gems >= 2)
                AddButton(0, "Common.Button.Beer".Localized(), GetANiamhBeer);
            else
                AddButton(0, "Common.Button.Beer".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void LeaveNiamh()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.LeaveNiamh.DecideDont".Localized());

            Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get());
        }

        public void GiveNiamphBimboLiquer()
        {

            TaintNiamh(true);
        }

        public void GiveNiamphSuccubiMilk()
        {

            TaintNiamh(false);
        }

        public void CorruptOrBimboNiamhIntro()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.CorruptOrBimboNiamhIntro.BustyKittyGreets".Localized());

            OutputText("Scenes.Niamh.CorruptOrBimboNiamhIntro.PauseContemplating".Localized());

            if (Player.HasItem(ItemIds.BimboLiqueur))
                OutputText("Scenes.Niamh.CorruptOrBimboNiamhIntro.BubblyForHer".Localized());

            OutputText("Scenes.Niamh.CorruptOrBimboNiamhIntro.MaybeSuccubusMilk".Localized());

            OutputText("Scenes.Niamh.CorruptOrBimboNiamhIntro.EditorsNote".Localized());

            ClearButtons();

            if (Player.HasItem(ItemIds.BimboLiqueur))
                AddButton(0, "Common.Button.Bimbo".Localized(), GiveNiamphBimboLiquer);
            else
                AddButton(0, "Common.Button.Bimbo".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                AddButton(1, "Items.SuccubiMilk.Short".Localized(), GiveNiamphSuccubiMilk);
            else
                AddButton(1, "Items.SuccubiMilk.Short".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), MaybeLaterNiamh);
        }

        public void MaybeLaterNiamh()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.MaybeLaterNiamh.ChuckleNervously".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Talk".Localized(), TalkToNiamh);

            if (Player.Gems >= 2)
                AddButton(1, "Common.Button.GetBeer".Localized(), GetANiamhBeer);
            else
                AddButton(1, "Common.Button.GetBeer".Localized(), null);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveNiamh);
        }

        public void TaintNiamh(bool bimbo = false)
        {
            ClearOutput();

            OutputText("Scenes.Niamh.TaintNiamh.GrabGlass".Localized());

            if (!bimbo)
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));

                OutputText("Scenes.Niamh.TaintNiamh.UncorkBottle".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.ReachesForMug".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.RespondRaising".Localized());
            }
            else
            {
                Player.ConsumeItem(ItemIds.BimboLiqueur);

                OutputText("Scenes.Niamh.TaintNiamh.PopSeal".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.ExpressionBrightens".Localized(
                     Player.Mf("lad", "lass")
                ));

                OutputText("Scenes.Niamh.TaintNiamh.ReachesForMugAgain".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.RespondRaisingAgain".Localized());
            }

            if (!bimbo)
            {
                OutputText("Scenes.Niamh.TaintNiamh.SmallGroan".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.CriesOutEcstasy".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.CrowdOfSpectators".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.NiamhStatus, 1);

                Player.ApplyStats(new DynStat(GameStats.Corruption, 5f));
            }
            else
            {
                OutputText("Scenes.Niamh.TaintNiamh.MomentsPassBy".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.BeforeSheCanRecover".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.OnlySightOfHer".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.EnamoredCatgirl".Localized());

                OutputText("Scenes.Niamh.TaintNiamh.CrowdFollowAllowing".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.NiamhStatus, 2);
            }

            CoC.Instance.Flags.Set(GameFlagIds.NiamhMovedOutCounter, 25);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 10f));

            DoNext(() => ReturnToCamp(1));
        }

        public void NiamhPostTelAdreMoveOut()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.NiamhMovedOutCounter, -1);

            OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.RoutineSearches".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 1)
                OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.DemonicLooking".Localized());
            else
                OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.HourglassShaped".Localized());

            OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.NiamhAndObvious".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 1)
                OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.WheyThere".Localized(
                     Player.Mf("boyo", "big girl")
                ));
            else
                OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.WheeeeyHoney".Localized());

            OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.SlursStruggling".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 1)
                OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.DarkWord".Localized());
            else
                OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.LightWord".Localized());

            OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.FluidLeavingBehind".Localized());

            OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.SuddenlyInquires".Localized());

            OutputText("Scenes.Niamh.NiamhPostTelAdreMoveOut.AsksInhibitions".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), NiamhCorruptedMobileSnackDrinkTime);

            AddButton(14, "Common.Button.Leave".Localized(), NiamhCorruptMobileSnackTurnDown);
        }

        public void NiamhCorruptMobileSnackTurnDown()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.NiamhCorruptMobileSnackTurnDown.TurnHerDown".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void NiamhCorruptedMobileSnackDrinkTime()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.OfCourseYouDo".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 2)
            {
                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.DoesBestToJiggle".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.EmptyHeaded".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.PurrsGiggles".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.StillRefusing".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.WayTooMuch".Localized());

                ItemRegistry.Create(ConsumableIds.BimboChampagne).OnUse();

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.GigglingAroundBoob".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.YowlsAndShudders".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.BoobgasmComes".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.GiggleBackPinned".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.LotOfGiggling".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.TellsYouSheHugs".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.HiccuppingWave".Localized());
            }
            else
            {
                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.AtHerBeckoning".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.BeforeYouCanReact".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.ColdBreeze".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.TakeMomentToGet".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.GatheringGear".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.StillPressOn".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.FallingQuestion".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.WakeWithStart".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.GlancingAround".Localized());

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.GivesLittlePunch".Localized(
                     Player.Mf("boy", "gal")
                ));

                OutputText("Scenes.Niamh.NiamhCorruptedMobileSnackDrinkTime.JustBeforeMove".Localized());

                ItemRegistry.Create(ConsumableIds.BCBeer).OnUse();
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void BazaarNiamh()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 2)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.TimesNiamhBazaarMet) == 0)
                {
                    OutputText("Scenes.Niamh.BazaarNiamh.EnterBazaar".Localized());

                    OutputText("Scenes.Niamh.BazaarNiamh.FollowSound".Localized());

                    OutputText("Scenes.Niamh.BazaarNiamh.AlreadyForgotten".Localized());
                }
                else
                    OutputText("Scenes.Niamh.BazaarNiamh.SurroundedBlonde".Localized());
            }
            else
            {
                OutputText("Scenes.Niamh.BazaarNiamh.PileOfSnoozing".Localized());

                OutputText("Scenes.Niamh.BazaarNiamh.PushingPastDrugged".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesNiamhBazaarMet, CoC.Instance.Flags.Get(GameFlagIds.TimesNiamhBazaarMet) + 1);

            ClearButtons();

            if (Player.Gems >= 2)
                AddButton(0, "Common.Button.GetADrink".Localized(), BazaardNiamhDrink);
            else
                AddButton(0, "Common.Button.GetADrink".Localized(), null);

            if (Player.HasCock())
                AddButton(1, "Common.Button.Titfuck".Localized(), BazaarSex);
            else
                AddButton(1, "Common.Button.Titfuck".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), null );
        }

        public void BazaardNiamhDrink()
        {
            ClearOutput();

            Player.Gems -= 2;

            OutputText("Scenes.Niamh.BazaardNiamhDrink.ApproachingFallen".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 2)
            {
                OutputText("Scenes.Niamh.BazaardNiamhDrink.DarkSkinnedBlonde".Localized());

                OutputText("Scenes.Niamh.BazaardNiamhDrink.ApproachLeakingTeats".Localized());

                ClearButtons();

                AddButton(0, "Scenes.Lumberjack.Button.Drink".Localized(), DrinkFromZeTap);

                AddButton(1, "Common.Button.Bottle".Localized(), GetBimboChampFromNiamh);
            }
            else
            {
                OutputText("Scenes.Niamh.BazaardNiamhDrink.SuccubusMilkStub".Localized());

                DoNext(() => ReturnToCamp(1));
            }
        }

        public void DrinkFromZeTap()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.DrinkFromZeTap.EyesLockOnto".Localized());

            OutputText("Scenes.Niamh.DrinkFromZeTap.TasteExplodes".Localized());

            OutputText("Scenes.Niamh.DrinkFromZeTap.SnapBackSenses".Localized());

            OutputText("Scenes.Niamh.DrinkFromZeTap.PushUpright".Localized());

            ItemRegistry.Create(ConsumableIds.BimboChampagne).OnUse();

            DoNext(() => ReturnToCamp(1));
        }

        public void GetBimboChampFromNiamh()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.GetBimboChampFromNiamh.ExplainNeedFor".Localized());

            OutputText("Scenes.Niamh.GetBimboChampFromNiamh.ResignedSigh".Localized());

            OutputText("Scenes.Niamh.GetBimboChampFromNiamh.PushingScaryThought".Localized());

            OutputText("Scenes.Niamh.GetBimboChampFromNiamh.LandsBigPile".Localized());

            InventoryScene.TakeItem(ConsumableIds.BimboChampagne, () => ReturnToCamp(1));
        }

        public void BazaarSex()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            var y = x + 1;

            if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 2)
            {
                OutputText("Scenes.Niamh.BazaarSex.ApproachBimbofiedCat".Localized());

                OutputText("Scenes.Niamh.BazaarSex.GrabHornyBimbo".Localized());

                OutputText("Scenes.Niamh.BazaarSex.PlantHandsFirmly".Localized());

                OutputText("Scenes.Niamh.BazaarSex.TellHerFeel".Localized(
                     y
                ));

                OutputText("Scenes.Niamh.BazaarSex.PinchCatSlut".Localized());

                OutputText("Scenes.Niamh.BazaarSex.MinutesOfTitFucking".Localized());

                OutputText("Scenes.Niamh.BazaarSex.GigglingDrunkenly".Localized());

                OutputText("Scenes.Niamh.BazaarSex.DrunkenKegGirl".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Intelligence, -.5f), new DynStat(GameStats.Sensitivity, -2f));
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.NiamhStatus) == 1)
                    OutputText("Scenes.Niamh.BazaarSex.SuccubiMilkStub".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SeanBimboBrewing()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.SeanBimboBrewing.GentlemanlyIncubus".Localized());

            OutputText("Scenes.Niamh.SeanBimboBrewing.SmirkAndExplain".Localized());

            OutputText("Scenes.Niamh.SeanBimboBrewing.PicksUpOne".Localized());

            ClearButtons();

            if (Player.Gems < 500)
            {
                OutputText("Scenes.Niamh.SeanBimboBrewing.TooPoorForBimbo".Localized());

                AddButton(0, "Common.Button.Yep".Localized(), null);
            }
            else
                AddButton(0, "Common.Button.Yep".Localized(), YeahSeanLetsBimbooze);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Places.TelAdre.TelAdre>.Get()));
        }

        public void YeahSeanLetsBimbooze()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.YeahSeanLetsBimbooze.MovingForwardGather".Localized());

            OutputText("Scenes.Niamh.YeahSeanLetsBimbooze.ScoldsUsheringOut".Localized());

            DoNext(PlayerMenu);

            Player.Gems -= 500;

            Player.ConsumeItem(ConsumableIds.BimboChampagne);

            Player.ConsumeItem(ConsumableIds.BimboChampagne);

            Player.ConsumeItem(ConsumableIds.BimboChampagne);

            Player.ConsumeItem(ConsumableIds.BimboChampagne);

            Player.ConsumeItem(ConsumableIds.BimboChampagne);

            CoC.Instance.Flags.Set(GameFlagIds.NiamhSeanBrewBimboLiqueurCounter, 25);
        }

        public void GetBimboozeFromSean()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.GetBimboozeFromSean.SeanTurnsEnter".Localized());

            OutputText("Scenes.Niamh.GetBimboozeFromSean.ScoopsFamiliarBottle".Localized());

            OutputText("Scenes.Niamh.GetBimboozeFromSean.DropVialInto".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.NiamhSeanBrewBimboLiqueurCounter, 0);

            InventoryScene.TakeItem(ItemIds.BimboLiqueur);
        }

        public void LeaveWithBeerTits()
        {
            ClearOutput();

            OutputText("Scenes.Niamh.LeaveWithBeerTits.OfferTemptingBut".Localized());

            Player.GrowTits(2, Player.BreastRows.Count, false, 2);

            DoNext(() => ReturnToCamp(1));
        }

        public void SellYourBooze()
        {
            ClearOutput();

            Player.Gems += 2;

            var temp = (int)Math.Round(Player.LactationQ() / 100);

            if (temp > 40)
                temp = 40 + Random.Shared.Next(3);

            Player.Gems += temp;

            Player.Gems = (int)Math.Round((double)Player.Gems);

            if (Player.BreastRows.Count == 1)
                OutputText("Scenes.Niamh.SellYourBooze.ChipperGigglePulls".Localized());
            else
                OutputText("Scenes.Niamh.SellYourBooze.GigglesAsYouTry".Localized());

            OutputText("Scenes.Niamh.SellYourBooze.NotLongBeforePatrons".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Niamh.SellYourBooze.ExcitementDrippingDown".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Sensitivity / 5)));

            if (Player.HasCock())
                OutputText("Scenes.Niamh.SellYourBooze.SympatheticSensations".Localized((noFur ? "" : " and paws")));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Niamh.SellYourBooze.PluralS".Localized());

            OutputText("Scenes.Niamh.SellYourBooze.SentencePeriod".Localized());

            if (Player.LustPercent < 50 || Player.Gender == Gender.Genderless)
            {
                Player.GrowTits(2, Player.BreastRows.Count, false, 2);

                OutputText("Scenes.Niamh.SellYourBooze.FlushedFromSensations".Localized());

                DoNext(() => ReturnToCamp(1));
            }
            else
            {
                OutputText("Scenes.Niamh.SellYourBooze.NotSureHowMuch".Localized());

                OutputText("Scenes.Niamh.SellYourBooze.NoticesYourCondition".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Niamh.SellYourBooze.LabiaDripping".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Niamh.SellYourBooze.PreCumDrizzles".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.FuckBar".Localized(), BarBeerOrgyTits);

                AddButton(14, "Common.Button.Leave".Localized(), LeaveWithBeerTits);
            }
        }

        public void BarBeerOrgyTits()
        {
            ClearOutput();

            if (Player.HasVagina())
                OutputText("Scenes.Niamh.BarBeerOrgyTits.DroolingGetsWorse".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Niamh.BarBeerOrgyTits.GrowsToFullLength".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.ItWord".Localized());
                else
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.ThemWord".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.DiscreetButImpossible".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.ItWordAgain".Localized());
                else
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.ThemWordAgain".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.DownOrAside".Localized());
            }

            OutputText("Scenes.Niamh.BarBeerOrgyTits.PurrsPokesBloated".Localized());

            if (Player.Body.Butt >= 20)
                OutputText("Scenes.Niamh.BarBeerOrgyTits.SlapsYourAss".Localized());

            OutputText("Scenes.Niamh.BarBeerOrgyTits.FewMoreHollers".Localized((noFur ? "" : " and paws")));

            if (Player.BiggestBreastSize() >= 20)
                OutputText("Scenes.Niamh.BarBeerOrgyTits.GiantBoobsLargeEnough".Localized());

            OutputText("Scenes.Niamh.BarBeerOrgyTits.HitsTheTableFirst".Localized());

            if ((Player.Pregnancy.Incubation == 0 || Player.Pregnancy.Incubation > 100) && Player.BreastRows.Count > 1)
                OutputText("Scenes.Niamh.BarBeerOrgyTits.PileOf".Localized(
                     BoozeBoobsType()
                ));
            else
                if (Player.BreastRows.Count > 1)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.GiantPregnant".Localized());

            if (View.Time.Hours < 15 && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > 0 && Player.HasVagina())
            {
                OutputText("Scenes.Niamh.BarBeerOrgyTits.PairOfFamiliar".Localized((noFur ? "hands" : "paws")));

                if (Player.Pregnancy.Incubation == 0)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.YelpsAndCheers".Localized());

                Player.CuntChange(60, true, true, false);
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.HandsGripLegs".Localized((noFur ? "men" : "morphs")));

                    if (Player.VaginalCapacity() < 60)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.OneSportingCock".Localized());
                    else
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.BothSportingCanine".Localized());

                    OutputText("Scenes.Niamh.BarBeerOrgyTits.BulbousCockPresses".Localized());
                }

            OutputText("Scenes.Niamh.BarBeerOrgyTits.CrawlsUpAndOn".Localized());

            if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 100)
                OutputText("Scenes.Niamh.BarBeerOrgyTits.PatsBellyChiding".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Niamh.BarBeerOrgyTits.SettlesDownWomanhood".Localized());
            else
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.GrabsHandful".Localized(
                         BoozeBoobsType()
                    ));

            if (Player.GetClitLength() >= 2)
                OutputText("Scenes.Niamh.BarBeerOrgyTits.SensationsInGroin".Localized());

            OutputText("Scenes.Niamh.BarBeerOrgyTits.NipplesSquirt".Localized(
                 BoozeBoobsType()
            ));

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Niamh.BarBeerOrgyTits.RandierDrunks".Localized(
                     NumberUtils.Num2Text(Player.TotalNipples()),

                     BoozeBoobsType()
                ));
            else
                OutputText("Scenes.Niamh.BarBeerOrgyTits.HornierCustomers".Localized());

            OutputText("Scenes.Niamh.BarBeerOrgyTits.BarFucksYouSilly".Localized(
                 BoozeBoobsType()
            ));

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Niamh.BarBeerOrgyTits.CocksFillingNipples".Localized(
                     BoozeBoobsType()
                ));

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Niamh.BarBeerOrgyTits.FeelAsIfBody".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.SHaveWord".Localized());
                else
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.HasSpaceWord".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.BegunSpurting".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.ItsWord".Localized());
                else
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.TheirWord".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.JizzSwallowsDown".Localized());

                if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 200)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.GlobsOfCum".Localized());
            }

            if (Player.StatusEffectValue(StatusEffectIds.Edryn, 0) >= 5 && Player.Gender == Gender.Hermaphrodite && View.Time.Hours == 14 && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > 0)
            {
                OutputText("Scenes.Niamh.BarBeerOrgyTits.SoDizzyOnPleasure".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.EdrynMoans".Localized());
                else
                    if (Player.CockTotal() > 1)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.HeadsOfCocks".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.EdrynsCuntSucks".Localized());

                if (Player.Pregnancy.Incubation > 0 && Player.Pregnancy.Incubation < 200)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.PregnantBellyPresses".Localized());
                else
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.UrtaCockAndEdryn".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.BetweenHorseCunt".Localized(
                     BoozeBoobsType()
                ));

                if (Player.BreastRows.Count == 2)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.SecondRowOfTits".Localized());
                else
                    if (Player.BreastRows.Count == 3)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.LowestRowOfTits".Localized());

                if (Player.Balls > 0 && Player.BallSize > 14)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.HangingTesticles".Localized());

                if (Player.Body.Butt >= 22)
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.FatAssCheeks".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.TwoOfThemFuck".Localized(
                     BoozeBoobsType()
                ));

                OutputText("Scenes.Niamh.BarBeerOrgyTits.FinallyEdrynCalls".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.UrtaCockFeels".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.NextThingAware".Localized());

                OutputText("Scenes.Niamh.BarBeerOrgyTits.TwoOfThemDrop".Localized());

                Player.GrowTits(2, Player.BreastRows.Count, false, 2);
            }
            else
                if (Player.HasVagina() && View.Time.Hours < 15 && CoC.Instance.Flags.Get(GameFlagIds.UrtaComfortableWithOwnBody) > 0)
                {
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.PassionateCries".Localized());

                    if (Player.Pregnancy.Incubation == 0)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.EruptionOfCum".Localized());
                    else
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.UrtaFeelsAsIf".Localized());

                    OutputText("Scenes.Niamh.BarBeerOrgyTits.OrgasmAfterOrgasm".Localized(
                         BoozeBoobsType()
                    ));

                    if (Player.BreastRows.Count > 1)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.RowsOfBreastsShift".Localized());

                    if (Player.Pregnancy.Incubation == 0)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.SmallOceanOfSperm".Localized());

                    OutputText("Scenes.Niamh.BarBeerOrgyTits.FinallyOnYour".Localized());

                    Player.GrowTits(2, Player.BreastRows.Count, false, 2);
                }
                else
                {
                    OutputText("Scenes.Niamh.BarBeerOrgyTits.SeemsLikeForever".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.GiantKnotOfDog".Localized((noFur ? "man" : "morph")));

                    Player.CuntChange(60, true, true, false);

                    OutputText("Scenes.Niamh.BarBeerOrgyTits.FinallyHasHerFill".Localized((noFur ? "man" : "morph")));

                    if (Player.HasVagina())
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.GagOnIt".Localized());

                    if (Player.CockTotal() == 1 && Player.CockArea(0) < 75)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.CatGirlCCup".Localized((noFur ? "" : " six")));
                    else
                        if (Player.CockTotal() == 1)
                            OutputText("Scenes.Niamh.BarBeerOrgyTits.CowGirlLargeTits".Localized((noFur ? "" : " six"), (noFur ? "" : " quad")));

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.CockRemainsEmbedded".Localized((noFur ? "" : " six")));
                    else
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Niamh.BarBeerOrgyTits.NotLongBeforeMouths".Localized());

                    OutputText("Scenes.Niamh.BarBeerOrgyTits.SmellOfSex".Localized());

                    OutputText("Scenes.Niamh.BarBeerOrgyTits.HelpsYouStruggle".Localized());

                    OutputText("Scenes.Niamh.BarBeerOrgyTits.LeaveTheBar".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.TittedGirlSmiles".Localized((noFur ? "big" : "six")));
                    else
                        if (Player.CockTotal() > 1)
                            OutputText("Scenes.Niamh.BarBeerOrgyTits.VariousGirlsAround".Localized());

                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Niamh.BarBeerOrgyTits.WalkBowLegged".Localized());

                        if (Player.Pregnancy.Type.Path == "none")
                            OutputText("Scenes.Niamh.BarBeerOrgyTits.CantHelpButWonder".Localized((noFur ? "men" : "morphs")));

                        var temp = Random.Shared.Next(6);

                        switch (temp)
                        {
                            case 0:
                            {
                            Player.KnockUp(PregnancyIds.Minotaur, PregnancyIds.MinotaurIncubation);
                            }

                            break;

                            case 1:
                            {
                            Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation);
                            }

                            break;

                            case 2:
                            {
                            Player.KnockUp(PregnancyIds.Mouse, PregnancyIds.MouseIncubation);
                            }

                            break;
                        }

                        if (true )
                            Player.KnockUp(PregnancyIds.Centaur, PregnancyIds.CentaurIncubation);
                    }

                    Player.GrowTits(2, Player.BreastRows.Count, false, 2);
                }

            Player.Orgasm("Tits");

            DoNext(() => ReturnToCamp(1));
        }

        public string BoozeBoobsType()
        {

            if (Holidays.IsSaturnalia() && DateTime.Now.Day >= 31)
                return "champagne";
            else
                if (DateTime.Now.Day == 17 && DateTime.Now.Month == 2)
                    return "Saint Patrick's Day lager";
                else
                    if (Holidays.IsSaturnalia())
                        return "egg nog";
                    else
                        if ((DateTime.Now.Month == 2 && DateTime.Now.Day == 14))
                            return "wine";
                        else
                            return "beer";
        }

    }
}