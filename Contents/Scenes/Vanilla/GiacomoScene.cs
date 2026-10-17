using MonoCoC.Attributes;
using MonoCoC.Registry;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<GiacomoScene>("coc:giacomo")]
    public class GiacomoScene : Scene, ITimeAware
    {

        public static SaveContent Save => SaveContent(new Identifier("coc", "giacomo"));

        public override void Entry()
        {
            GiacomoEncounter();
        }

        public bool TimeChange()
        {
            Save.SetInt("checkedSuccubi", 0);

            if (View.Time.Hours > 23)
            {
                var counter = CoC.Instance.Flags.Get(GameFlagIds.CeruleanPotionBadEndFutaCounter);

                if (counter > 0)
                    CoC.Instance.Flags.Set(GameFlagIds.CeruleanPotionBadEndFutaCounter, counter - 0.5f);
            }

            return false;
        }

        public bool TimeChangeLarge()
        {
            var previous = Save.GetInt("checkedSuccubi", 0);

            Save.SetInt("checkedSuccubi", previous + 1);

            if (previous == 0
                && View.Time.Hours == 4

                && Player.HasStatusEffect(StatusEffectIds.SuccubiNight)

                && (Player.HasCock() || Player.Gender == Gender.Genderless))
            {
                if (Player.HasStatusEffect(StatusEffectIds.RepeatSuccubi))
                {
                    if (SceneInstance<VapulaScene>.Get().VapulaSlave()
                        && Player.HasCock()

                        && CoC.Instance.Flags.Get(GameFlagIds.VapulaThreesomes) > 0

                        && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0)

                        VapulaAssistsCeruleanSuccubus(); // 原版 Vapula.hx:831：满足条件时走三人行场景（不再走重复梦境）
                    else
                        NightSuccubiRepeat();
                }
                else
                {
                    NightSuccubiFirstTime();

                    Player.CreateStatusEffect(StatusEffectIds.RepeatSuccubi, 0, 0, 0, 0);
                }

                if (Player.StatusEffectValue(StatusEffectIds.SuccubiNight, 0) > 1)
                    Player.AddStatusValue(StatusEffectIds.SuccubiNight, 0, -1);
                else
                    Player.RemoveStatusEffect(StatusEffectIds.SuccubiNight);

                return true;
            }

            return false;
        }

        public void GiacomoEncounter()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.GiacomoMet) == 0)
                FirstEncounter();
            else
                if (!Player.HasStatusEffect(StatusEffectIds.WormOffer) && Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasPerk(PerkIds.ParasiteMusk) || (Player.HasStatusEffect(StatusEffectIds.ParasiteSlug) && CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 1))
                {
                    OutputText("Scenes.Giacomo.GiacomoEncounter.WalksUp".Localized());

                    OutputText("Scenes.Giacomo.GiacomoEncounter.ChucklesWorms".Localized());

                    OutputText("Scenes.Giacomo.GiacomoEncounter.HowHeKnows".Localized());

                    OutputText("Scenes.Giacomo.GiacomoEncounter.HecklesLightly".Localized());

                    OutputText("Scenes.Giacomo.GiacomoEncounter.LookingDown".Localized());

                    OutputText("Scenes.Giacomo.GiacomoEncounter.PurveyorJingles".Localized());

                    Player.CreateStatusEffect(StatusEffectIds.WormOffer, 0, 0, 0, 0);

                    if (Player.Gems < 175)
                        OutputText("Scenes.Giacomo.GiacomoEncounter.NotEnoughGems".Localized());
                }
                else
                {
                    OutputText("Scenes.Giacomo.GiacomoEncounter.SpyInDistance".Localized());

                    OutputText("Scenes.Giacomo.GiacomoEncounter.CreepyGrin".Localized());
                }

            ClearButtons();

            AddButton(0, "Common.Button.Potions".Localized(), PotionMenu);

            AddButton(1, "Common.Button.Erotica".Localized(), EroticaMenu);

            AddButton(2, "Common.Button.Books".Localized(), BookMenu);

            if (Player.HasStatusEffect(StatusEffectIds.Infested) || Player.HasPerk(PerkIds.ParasiteMusk) || (Player.HasStatusEffect(StatusEffectIds.ParasiteSlug) && CoC.Instance.Flags.Get(GameFlagIds.Hadslugparasite) == 1))
                AddButton(3, "Common.Button.WormCure".Localized(), WormRemovalOffer);

            SetExitButton("Common.Button.Back".Localized());

        }

        public void FirstEncounter()
        {

            OutputText("Scenes.Giacomo.FirstEncounter.TravellingPerson".Localized());

            OutputText("Scenes.Giacomo.FirstEncounter.GreetingsTraveler".Localized());

            OutputText("Scenes.Giacomo.FirstEncounter.ToothyGrin".Localized());

            OutputText("Scenes.Giacomo.FirstEncounter.InterestedDrafts".Localized());

            OutputText("Scenes.Giacomo.FirstEncounter.PausesTurns".Localized());

            OutputText("Scenes.Giacomo.FirstEncounter.MaybeItems".Localized());

            OutputText("Scenes.Giacomo.FirstEncounter.CreepyGrin".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.GiacomoMet, 1);
        }

        public void PotionMenu()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PotionMenu.WhichPotion".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.VitalityT".Localized(), PitchVitailtyTincture);

            AddButton(1, "Common.Button.ScholarsT".Localized(), PitchScholarsTea);

            if (!Player.IsFemale())
                AddButton(2, "Common.Button.CeruleanP".Localized(), PitchCeruleanPotion);
            else
                AddButton(2, "Common.Button.CeruleanP".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), GiacomoEncounter);

        }

        public void BookMenu()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BookMenu.WhichBook".Localized());

            ClearButtons();

            if (!Player.HasKeyItem("Dangerous Plants"))
                AddButton(0, "Common.Button.DangerPlants".Localized(), PitchDangerousPlantsBook);
            else
                AddButton(0, "Common.Button.DangerPlants".Localized(), null);

            if (!Player.HasKeyItem("Traveler's Guide"))
                AddButton(1, "Common.Button.TravelGuide".Localized(), PitchTravelersGuide);
            else
                AddButton(1, "Common.Button.TravelGuide".Localized(), null);

            if (!Player.HasKeyItem("Hentai Comic"))
                AddButton(2, "Common.Button.HentaiComic".Localized(), PitchHentaiComic);
            else
                AddButton(2, "Common.Button.HentaiComic".Localized(), null);

            if (Player.HasKeyItem("Yoga Guide"))
                AddButton(3, "Common.Button.YogaGuide".Localized(), null);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.CottonUnusualYogaBookTracker) > 0)
                    AddButton(3, "Common.Button.YogaGuide".Localized(), PitchYogaGuide);

            AddButton(14, "Common.Button.Back".Localized(), GiacomoEncounter);

        }

        public void EroticaMenu()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.EroticaMenu.CreepyGrin".Localized());

            ClearButtons();

            if (!Player.HasKeyItem("Dildo"))
                AddButton(0, "Common.Button.Dildo".Localized(), PitchDildo);
            else
                AddButton(0, "Common.Button.Dildo".Localized(), null);

            if (Player.HasVagina())
            {
                if (!Player.HasKeyItem("Self-Stimulation Belt"))
                    AddButton(1, "Common.Button.StimBelt".Localized(), PitchSelfStimulationBelt);
                else
                    AddButton(1, "Common.Button.StimBelt".Localized(), null);

                if (!Player.HasKeyItem("All-Natural Self-Stimulation Belt"))
                    AddButton(2, "Common.Button.AnStimBelt".Localized(), PitchAllNaturalSelfStimulationBelt);
                else
                    AddButton(2, "Common.Button.AnStimBelt".Localized(), null);
            }
            else
            {
                AddButton(1, "Common.Button.StimBelt".Localized(), null);

                AddButton(2, "Common.Button.AnStimBelt".Localized(), null);
            }

            if (Player.HasCock())
            {
                if (!Player.HasKeyItem("Plain Onahole"))
                    AddButton(3, "Common.Button.Onahole".Localized(), PitchOnahole);
                else
                    AddButton(3, "Common.Button.Onahole".Localized(), null);

                if (!Player.HasKeyItem("Deluxe Onahole"))
                    AddButton(4, "Common.Button.DOnahole".Localized(), PitchDeluxeOnahole);
                else
                    AddButton(4, "Common.Button.DOnahole".Localized(), null);
            }
            else
            {
                AddButton(3, "Common.Button.Onahole".Localized(), null);

                AddButton(4, "Common.Button.DOnahole".Localized(), null);
            }

            if (Player.HasCock() && Player.HasVagina())
            {
                if (!Player.HasKeyItem("Dual Belt"))
                    AddButton(5, "Common.Button.DualBelt".Localized(), PitchDualStimulationBelt);
                else
                    AddButton(5, "Common.Button.DualBelt".Localized(), null);

                if (!Player.HasKeyItem("All-Natural Onahole"))
                    AddButton(6, "Common.Button.AnOnahole".Localized(), PitchAllNaturalOnahole);
                else
                    AddButton(6, "Common.Button.AnOnahole".Localized(), null);
            }
            else
            {
                AddButton(5, "Common.Button.DualBelt".Localized(), null);

                AddButton(6, "Common.Button.AnOnahole".Localized(), null);
            }

            AddButton(7, "Items.Condom.Short".Localized(), PitchCondom);

            AddButton(14, "Common.Button.Back".Localized(), GiacomoEncounter);

        }

        public void PitchVitailtyTincture()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchVitailtyTincture.HoldsTincture".Localized());

            ClearButtons();

            if (Player.Gems >= 15)
                AddButton(0, "Common.Button.Yes".Localized(), BuyVitailtyTincture);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), PotionMenu);
        }

        public void BuyVitailtyTincture()
        {
            ClearOutput();

            Player.Gems -= 15;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.VitalityTincture), text => OutputText(text), Random.Shared);

                PotionMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.VitalityTincture), 1);

PotionMenu();
            }

        }

        public void PitchScholarsTea()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchScholarsTea.HoldsPouch".Localized());

            ClearButtons();

            if (Player.Gems >= 15)
                AddButton(0, "Common.Button.Yes".Localized(), BuyScholarsTea);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), PotionMenu);
        }

        public void BuyScholarsTea()
        {
            ClearOutput();

            Player.Gems -= 15;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.ScholarTea), text => OutputText(text), Random.Shared);

                PotionMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.ScholarTea), 1);

PotionMenu();
            }

        }

        public void PitchCeruleanPotion()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchCeruleanPotion.ShoulderSearch".Localized());

            ClearButtons();

            if (Player.Gems >= 75)
                AddButton(0, "Common.Button.Yes".Localized(), BuyCeruleanPotion);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), PotionMenu);
        }

        public void BuyCeruleanPotion()
        {
            ClearOutput();

            Player.Gems -= 75;

            if (CoC.Instance.Flags.Get(GameFlagIds.ShiftKeyDown) == 1)
            {
                Player.Inventory.Use(ItemRegistry.Create(ConsumableIds.CeruleanPotion), text => OutputText(text), Random.Shared);

                PotionMenu();
            }
            else
            {
                Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CeruleanPotion), 1);

PotionMenu();
            }

        }

        public void PitchCondom()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchCondom.HoldsPacket".Localized());

            ClearButtons();

            if (Player.Gems >= 10)
                AddButton(0, "Common.Button.Yes".Localized(), BuyCondom);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuyCondom()
        {

            Player.Gems -= 10;

            Player.Inventory.Add(ItemRegistry.Create(ItemIds.Condom), 1);

EroticaMenu();

        }

        public void PitchDangerousPlantsBook()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchDangerousPlantsBook.HoldsText".Localized());

            OutputText("Scenes.Giacomo.PitchDangerousPlantsBook.AccordingScholars".Localized());

            ClearButtons();

            if (Player.Gems >= 10)
                AddButton(0, "Common.Button.Yes".Localized(), BuyDangerousPlantsBook);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), BookMenu);
        }

        public void BuyDangerousPlantsBook()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyDangerousPlantsBook.QuiteLiterate".Localized());

            OutputText("Scenes.Giacomo.BuyDangerousPlantsBook.ViolationChapter".Localized());

            OutputText("Scenes.Giacomo.BuyDangerousPlantsBook.ShiverAtEnd".Localized());

            BookMenu();

            Player.Gems -= 10;

            Player.CreateKeyItem("Dangerous Plants", 0, 0, 0, 0);

        }

        public void PitchTravelersGuide()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchTravelersGuide.HumblePamphlet".Localized());

            ClearButtons();

            if (Player.Gems >= 1)
                AddButton(0, "Common.Button.Yes".Localized(), BuyTravelersGuide);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), BookMenu);
        }

        public void BuyTravelersGuide()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyTravelersGuide.DidntNeedIt".Localized());

            BookMenu();

            Player.Gems -= 1;

            Player.CreateKeyItem("Traveler's Guide", 0, 0, 0, 0);

        }

        public void PitchHentaiComic()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchHentaiComic.TakesMagazine".Localized());

            OutputText("Scenes.Giacomo.PitchHentaiComic.PrimalTaste".Localized(
                Player.Mfn("man", "lady", "err, whatever you are")
            ));

            ClearButtons();

            if (Player.Gems >= 10)
                AddButton(0, "Common.Button.Yes".Localized(), BuyHentaiComic);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), BookMenu);
        }

        public void BuyHentaiComic()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyHentaiComic.PerusesBook".Localized());

            OutputText("Scenes.Giacomo.BuyHentaiComic.ComicEntertaining".Localized());

            BookMenu();

            Player.Gems -= 10;

            Player.ApplyStats(new DynStat(GameStats.Libido, 2f), new DynStat(GameStats.Lust, 20f));

            Player.CreateKeyItem("Hentai Comic", 0, 0, 0, 0);

        }

        public void PitchYogaGuide()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchYogaGuide.HoldsReverently".Localized());

            OutputText("Scenes.Giacomo.PitchYogaGuide.StrangeBook".Localized());

            ClearButtons();

            if (Player.Gems >= 100)
                AddButton(0, "Common.Button.Yes".Localized(), BuyYogaGuide);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), BookMenu);
        }

        public void BuyYogaGuide()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyYogaGuide.HundredGems".Localized());

            Player.CreateKeyItem("Yoga Guide", 0, 0, 0, 0);

            Player.Gems -= 100;

            BookMenu();
        }

        public void PitchDildo()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchDildo.SlenderTube".Localized());

            ClearButtons();

            if (Player.Gems >= 20)
                AddButton(0, "Common.Button.Yes".Localized(), BuyDildo);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuyDildo()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyDildo.HandsDildo".Localized());

            EroticaMenu();

            Player.Gems -= 20;

            Player.CreateKeyItem("Dildo", 0, 0, 0, 0);

        }

        public void PitchSelfStimulationBelt()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchSelfStimulationBelt.ChastityBelt".Localized());

            OutputText("Scenes.Giacomo.PitchSelfStimulationBelt.CrazedMerchant".Localized());

            ClearButtons();

            if (Player.Gems >= 30)
                AddButton(0, "Common.Button.Yes".Localized(), BuySelfStimulationBelt);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuySelfStimulationBelt()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuySelfStimulationBelt.HandsBelt".Localized());

            Player.CreateKeyItem("Self-Stimulation Belt", 0, 0, 0, 0);

            EroticaMenu();

            Player.Gems -= 30;

        }

        public void PitchAllNaturalSelfStimulationBelt()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchAllNaturalSelfStimulationBelt.BagOnGround".Localized());

            OutputText("Scenes.Giacomo.PitchAllNaturalSelfStimulationBelt.QuiteIntriguing".Localized());

            if (Player.HasKeyItem("Dangerous Plants") && Player.Intelligence > 39)
                OutputText("Scenes.Giacomo.PitchAllNaturalSelfStimulationBelt.NoduleBase".Localized());
            else
                if (Player.Intelligence > 29)
                    OutputText("Scenes.Giacomo.PitchAllNaturalSelfStimulationBelt.ChangeAttitude".Localized());

            OutputText("Scenes.Giacomo.PitchAllNaturalSelfStimulationBelt.BuyAllNatural".Localized());

            ClearButtons();

            if (Player.Gems >= 40)
                AddButton(0, "Common.Button.Yes".Localized(), BuyAllNaturalSelfStimulationBelt);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuyAllNaturalSelfStimulationBelt()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyAllNaturalSelfStimulationBelt.HandsNaturalBelt".Localized());

            Player.CreateKeyItem("All-Natural Self-Stimulation Belt", 0, 0, 0, 0);

            EroticaMenu();

            Player.Gems -= 40;

        }

        public void PitchOnahole()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchOnahole.PinkCylinder".Localized());

            OutputText("Scenes.Giacomo.PitchOnahole.WhenYouDoNot".Localized());

            ClearButtons();

            if (Player.Gems >= 20)
                AddButton(0, "Common.Button.Yes".Localized(), BuyOnahole);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuyOnahole()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyOnahole.HandsOnahole".Localized());

            Player.CreateKeyItem("Plain Onahole", 0, 0, 0, 0);

            EroticaMenu();

            Player.Gems -= 20;

        }

        public void PitchDeluxeOnahole()
        {
            ClearOutput();

            if (Player.HasKeyItem("Deluxe Onahole"))
            {
                OutputText("Scenes.Giacomo.PitchDeluxeOnahole.AlreadyOwn".Localized());

                EroticaMenu();

                return ;
            }

            OutputText("Scenes.Giacomo.PitchDeluxeOnahole.WeirdlyShaped".Localized());

            OutputText("Scenes.Giacomo.PitchDeluxeOnahole.ExcellentThing".Localized());

            ClearButtons();

            if (Player.Gems >= 50)
                AddButton(0, "Common.Button.Yes".Localized(), BuyDeluxeOnahole);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuyDeluxeOnahole()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyDeluxeOnahole.HandsDeluxe".Localized());

            Player.CreateKeyItem("Deluxe Onahole", 0, 0, 0, 0);

            EroticaMenu();

            Player.Gems -= 50;

        }

        public void PitchAllNaturalOnahole()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchAllNaturalOnahole.ReachesBag".Localized());

            OutputText("Scenes.Giacomo.PitchAllNaturalOnahole.MisterStates".Localized());

            if (Player.HasKeyItem("Dangerous Plants") && Player.Intelligence > 35)
            {
                OutputText("Scenes.Giacomo.PitchAllNaturalOnahole.AvoidingSuspicion".Localized());

                if (Player.Intelligence > 65)
                {
                    OutputText("Scenes.Giacomo.PitchAllNaturalOnahole.DoRecall".Localized());

                    OutputText("Scenes.Giacomo.PitchAllNaturalOnahole.NoWayToTell".Localized());
                }
            }

            ClearButtons();

            if (Player.Gems >= 150)
                AddButton(0, "Common.Button.Yes".Localized(), BuyAllNaturalOnahole);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuyAllNaturalOnahole()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyAllNaturalOnahole.HandsNaturalOnahole".Localized());

            Player.CreateKeyItem("All-Natural Onahole", 0, 0, 0, 0);

            EroticaMenu();

            Player.Gems -= 150;

        }

        public void PitchDualStimulationBelt()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.PitchDualStimulationBelt.SmilesWidely".Localized());

            OutputText("Scenes.Giacomo.PitchDualStimulationBelt.LatestPiece".Localized());

            ClearButtons();

            if (Player.Gems >= 50)
                AddButton(0, "Common.Button.Yes".Localized(), BuyDualStimulationBelt);
            else
                AddButton(0, "Common.Button.Yes".Localized(), null);

            AddButton(1, "Common.Button.No".Localized(), EroticaMenu);
        }

        public void BuyDualStimulationBelt()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.BuyDualStimulationBelt.HandsDualBelt".Localized());

            Player.CreateKeyItem("Dual Belt", 0, 0, 0, 0);

            Player.Gems -= 50;

            EroticaMenu();
        }

        public void WormRemoval()
        {
            ClearOutput();

            OutputText("Scenes.Giacomo.WormRemoval.TossGems".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.Infested))
                OutputText("Scenes.Giacomo.WormRemoval.PainFocuses".Localized());

            OutputText("Scenes.Giacomo.WormRemoval.FreakishCel".Localized());

            OutputText("Scenes.Giacomo.WormRemoval.IAmMerchant".Localized());

            OutputText("Scenes.Giacomo.WormRemoval.PutsAwayPrize".Localized());

            if (Player.Health > (int)(Player.MaxHealth * .15f))
                Player.Health = (int)(Player.MaxHealth * .15f);

            Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlug);

            Player.RemoveStatusEffect(StatusEffectIds.Infested);

            Player.RemovePerk(PerkIds.ParasiteMusk);

            Player.RemoveStatusEffect(StatusEffectIds.ParasiteSlugReproduction);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Lust, -99f), new DynStat(GameStats.Corruption, -4f));

            Player.Gems -= 175;

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.VitalityTincture), 1);

ReturnToCamp(1);
        }

        public void WormRemovalOffer()
        {

            OutputText("Scenes.Giacomo.WormRemovalOffer.HeChuckles".Localized());

            OutputText("Scenes.Giacomo.WormRemovalOffer.HowHeKnows".Localized());

            OutputText("Scenes.Giacomo.WormRemovalOffer.HecklesLightly".Localized());

            OutputText("Scenes.Giacomo.WormRemovalOffer.LookingDown".Localized());

            OutputText("Scenes.Giacomo.WormRemovalOffer.PurveyorJingles".Localized());

            ClearButtons();

            if (Player.Gems >= 175)
            {
                OutputText("Scenes.Giacomo.WormRemovalOffer.PurchaseCure".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), WormRemoval);
            }
            else
            {
                OutputText("Scenes.Giacomo.WormRemovalOffer.NotEnoughGems".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), null);
            }

            AddButton(1, "Common.Button.No".Localized(), GiacomoEncounter);
        }

        public void NightSuccubiFirstTime()
        {

            OutputText("Scenes.Giacomo.NightSuccubiFirstTime.RestDisturbed".Localized());

            OutputText("Scenes.Giacomo.NightSuccubiFirstTime.FeminineVoice".Localized());

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.MovesToCrotch".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.RecordScratch".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.WaitMinute".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.GenderlessNature".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.CrowsNames".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.UtterContempt".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.ThrowsScript".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.WalksPaces".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.KillingMe".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.HandsRobe".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.TellsHerAgent".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.PointsDerision".Localized());

                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.StormyExit".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.CeruleanPotionNeuterAttempted, 1);

                Go<CampScene>();

                return ;
            }

            if (Player.Gender == Gender.Male)
                OutputText("Scenes.Giacomo.NightSuccubiFirstTime.ChildishVoice".Localized());
            else
                if (Player.Gender == Gender.Hermaphrodite)
                {
                    CoC.Instance.Flags.Set(GameFlagIds.CeruleanSuccubusHermCounter, CoC.Instance.Flags.Get(GameFlagIds.CeruleanSuccubusHermCounter) + 1);

                    OutputText("Scenes.Giacomo.NightSuccubiFirstTime.ConfrontedSuccubus".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiFirstTime.WellWellWell".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiFirstTime.LooksAtCock".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiFirstTime.QuitWhimpering".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiFirstTime.TriesStruggle".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiFirstTime.GettingMilked".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiFirstTime.FinishesUltimatum".Localized());
                }

            Player.ApplyStats(new DynStat(GameStats.Lust, 35f));

            CeruleanSuccubusEncounterPart2();
        }

        public void CeruleanSuccubusEncounterPart2()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart2.InstinctsTakeOver".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart2.SatisfiedLook".Localized());
            }
            else
            {
                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart2.SaysTriumphantly".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart2.FierceMilking".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart2.PlayfullyChallenges".Localized());
            }

            Player.ApplyStats(new DynStat(GameStats.Toughness, .3f), new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, .5f), new DynStat(GameStats.Lust, 5f), new DynStat(GameStats.Corruption, 1f));

            CeruleanSuccubusEncounterPart3();
        }

        public void CeruleanSuccubusEncounterPart3()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart3.SixtyNine".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart3.ImpossibleResist".Localized());
            }
            else
            {
                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart3.PinsShoulders".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart3.DickWeakens".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart3.JarNext".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart3.LongTimeSince".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart3.FliesOff".Localized());
            }

            Player.ChangeFatigue(20);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f));

            CeruleanSuccubusEncounterPart4();
        }

        public void CeruleanSuccubusEncounterPart4()
        {
            ClearOutput();

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart4.HelpsToFeet".Localized());

                if (Player.Tallness < 80)
                    OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart4.TowersOver".Localized());

                if (Player.Tallness >= 80 && Player.Tallness < 90)
                    OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart4.AsTallAs".Localized());

                if (Player.Tallness >= 90)
                    OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart4.DefinitelyShorter".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart4.BracesAgainstTree".Localized());

                OutputText("Scenes.Giacomo.CeruleanSuccubusEncounterPart4.GiggleDisappears".Localized());

                Player.ApplyStats(new DynStat(GameStats.Strength, .5f), new DynStat(GameStats.Lust, 4f));
            }

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CeruleanPotion), 1);
        }

        public void NightSuccubiRepeat()
        {

            if (Player.Gender == Gender.Genderless)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.CeruleanPotionNeuterAttempted) == 0)
                {
                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.RestDisturbed".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.FeminineVoice".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.MovesToCrotch".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.RecordScratch".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.WaitMinute".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.GenderlessNature".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.CrowsNames".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.UtterContempt".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.ThrowsScript".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.WalksPaces".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.KillingMe".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.HandsRobe".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.TellsHerAgent".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.PointsDerision".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.StormyExit".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.CeruleanPotionNeuterAttempted, 1);
                }
                else
                {
                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.FootstepsBehind".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.TurnAround".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.SaysMeekly".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.InquiresDocument".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.ScriptProduction".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.TakesScript".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.GenderNeutral".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.ShockOverwhelming".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.CacophonyFades".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.ProblemLine".Localized());
                }

                Go<CampScene>();

                return ;
            }

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 2f));

            if (Player.Gender == Gender.Male)
            {
                if (Player.Corruption < 66)
                {
                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.BetterJudgment".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.CrouchBetween".Localized());
                    else
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.OpensWomb".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.SuperhumanMuscles".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.EngulfsMore".Localized());
                    else
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.CrushingPelvis".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.MusclesChurn".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.ArchesBelly".Localized());
                    else
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.ReleasesGrip".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.ContractionsMilking".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.PassingOut".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.FatigueTakes".Localized());
                }
                else
                {
                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.KnowingSheWillCome".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.ComesAsPredicted".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.SheChuckles".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.BeginBucking".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.FlanksWord".Localized());
                    else
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.HipsWord".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.FamiliarRhythm".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.SensesControl".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.ImpalesDeeper".Localized());
                    else
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.WrapsLegs".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.HeadOfPrick".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.MuscleSpasms".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.MoansInhumanly".Localized());
                    else
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.EmbracesMoaning".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.ForceHarder".Localized());

                    OutputText("Scenes.Giacomo.NightSuccubiRepeat.FirstTimeVisits".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Libido, -1f));
                }
            }
            else
                if (Player.Gender == Gender.Hermaphrodite)
                    if (CoC.Instance.Flags.Get(GameFlagIds.CeruleanPotionBadEndFutaCounter) > 10 && Player.Corruption > 50)
                    {
                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.UncontrollableUrge".Localized());

                        if (Player.Balls > 0)
                            OutputText("Scenes.Giacomo.NightSuccubiRepeat.NutsSore".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.MilkDries".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.WakesSore".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.WellUnexpected".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.FeelsAwkward".Localized());

                        if (Player.HorseScore() + Player.DogScore() + Player.NagaScore() + Player.GoblinScore() + Player.SharkScore() + Player.MinoScore() + Player.CowScore() + Player.MermaidScore() > 5)
                            OutputText("Scenes.Giacomo.NightSuccubiRepeat.OtherCorruptions".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.RealitySoaks".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.TakesUp".Localized());

                        GameOver();

                        return ;
                    }
                    else
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.CeruleanSuccubusHermCounter, CoC.Instance.Flags.Get(GameFlagIds.CeruleanSuccubusHermCounter) + 1);

                        CoC.Instance.Flags.Set(GameFlagIds.CeruleanPotionBadEndFutaCounter, CoC.Instance.Flags.Get(GameFlagIds.CeruleanPotionBadEndFutaCounter) + 1);

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.RelaxFromDay".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.RestrainsYou".Localized());

                        OutputText("Scenes.Giacomo.NightSuccubiRepeat.ConsciousnessFades".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.CeruleanPotionBadEndFutaCounter) > 10)
                            Player.RemoveStatusEffect(StatusEffectIds.SuccubiNight);

                        Player.ChangeFatigue(20);

                        Player.CumMultiplier += 1;
                    }

            OutputText("Scenes.Giacomo.NightSuccubiRepeat.ParagraphBreak".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Strength, Random.Shared.Next(2)), new DynStat(GameStats.Toughness, Random.Shared.Next(2)), new DynStat(GameStats.Speed, Random.Shared.Next(2)), new DynStat(GameStats.Intelligence, Random.Shared.Next(2)), new DynStat(GameStats.Corruption, 1f));

            Player.Inventory.Add(ItemRegistry.Create(ConsumableIds.CeruleanPotion), 1);
        }

        // 原版 Vapula.hx:831 vapulaAssistsCeruleanSuccubus()：Vapula 与蓝色魅魔的三人行
        public virtual void VapulaAssistsCeruleanSuccubus()
        {
            var player = CoC.Instance.Player;

            var flags = CoC.Instance.Flags;

            // 原版 :831 选一根「塞得下」的屌，没有就用最小的
            var x = player.CockThatFits(100);

            if (x < 0)
                x = player.SmallestCockIndex();

            ClearOutput();

            // 原版 :837-844
            OutputText("Scenes.Vapula.CeruleanThreeway.ThatNight".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.GetReadyForGuest".Localized(player.CockDescript(x)));

            OutputText("Scenes.Vapula.CeruleanThreeway.ComeInWaiting".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.WhatAmIGoingToDo".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.TwoGirlsEagerForCum".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.BothCreaturesJumpYou".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.DropsOfFluid".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.SheStartsRiding".Localized());

            // 原版 :846 多屌时换描写
            OutputText(player.CockTotal() > 1

                ? "Scenes.Vapula.CeruleanThreeway.ReachesSecondCock".Localized()

                : "Scenes.Vapula.CeruleanThreeway.LicksVapulaPussy".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.BothRideRelentlessly".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.CockSlapsLovers".Localized());

            // 原版 :854 / :858 第二、第三根屌
            if (player.CockTotal() > 1)
                OutputText("Scenes.Vapula.CeruleanThreeway.InsertSecondPenis".Localized());

            if (player.CockTotal() > 2)
                OutputText("Scenes.Vapula.CeruleanThreeway.ThirdToolThroat".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.KeepPoundingAway".Localized());

            // 原版 :863 / :866
            if (player.CockTotal() > 1)
            {
                OutputText("Scenes.Vapula.CeruleanThreeway.PackTightSnatch".Localized());

                if (player.CockTotal() > 2)
                    OutputText("Scenes.Vapula.CeruleanThreeway.BlueLoversMouth".Localized());
            }

            OutputText("Scenes.Vapula.CeruleanThreeway.Likewise".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.JismSpilled".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.PopOutCock".Localized());

            // 原版 :874 矮个子会被埋进乳沟里
            OutputText(player.Tallness < 60

                ? "Scenes.Vapula.CeruleanThreeway.AskVapulaAgrees".Localized()

                : "Scenes.Vapula.CeruleanThreeway.EnclosingHeadCleavage".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.ThatWasFun".Localized());

            OutputText("Scenes.Vapula.CeruleanThreeway.DefinitelyWill".Localized());

            // 原版 :880 标准的夜间魅魔属性结算
            flags.Set(GameFlagIds.VapulaDaysSinceFed, 0);

            flags.Set(GameFlagIds.VapulaTeaseCount, 0);

            player.Orgasm("Dick");

            player.ApplyStats(
                new DynStatsOptions(Scale: false),

                new DynStat(GameStats.Strength, Random.Shared.Next(2)),

                new DynStat(GameStats.Toughness, Random.Shared.Next(2)),

                new DynStat(GameStats.Speed, Random.Shared.Next(2)),

                new DynStat(GameStats.Intelligence, Random.Shared.Next(2)),

                new DynStat(GameStats.Corruption, 2.5f));

            // 原版 :884 取走一瓶 Cerulean 药水并回到主菜单
            InventoryScene.TakeItem(
                ItemRegistry.Create(ConsumableIds.CeruleanPotion),

                () => ReturnToCamp(1));
        }

    }
}