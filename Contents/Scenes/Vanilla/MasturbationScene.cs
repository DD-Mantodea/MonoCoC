using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using System.Linq;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Utils;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    [Register<MasturbationScene>("coc:masturbation")]
    public class MasturbationScene : Scene
    {

        public override void Entry()
        {
            SetMasturbateButton();
        }

        public void SetMasturbateButton(bool inDungeon = false, int index = 8)
        {

            if (PaladinCheck())
            {
                if (Player.Lust == Player.MaxLust)
                    AddButton(index, "Common.Button.Flagellate".Localized(), Flagellate,

                    "Scenes.Camp.Hint.Flagellate".Localized());
                else
                    AddButtonDisabled(index, CanMeditate() ? "Meditate" : "Masturbate",

                    CanMeditate()

                        ? "Scenes.Camp.Hint.NoTimeToMeditate".Localized()

                        : "Scenes.Camp.Hint.NoTimeToMasturbate".Localized());
            }
            else
                if (CanMeditate())
                    AddButton(index, "Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(),

                    inDungeon ? () => Meditate("place") : MasturbateMenu,

                    "Scenes.Camp.Hint.Meditate".Localized());
                else
                    AddButton(index, "Common.Button.Masturbate".Localized(),

                    inDungeon ? MasturbateGo : MasturbateMenu,

                    "Scenes.Camp.Hint.Masturbate".Localized());
        }

        public bool ReligionCheck()
        {

            return Player.IsReligious() && Player.IsPureEnough(66);
        }

        public bool EnlightenedCheck()
        {

            return Player.HasPerk(PerkIds.Enlightened) && Player.IsPureEnough(10);
        }

        public bool PaladinCheck()
        {

            return Player.HasPerk(PerkIds.HistoryDEUSVULT) && Player.IsPureEnough(25);
        }

        public bool ExgartuanCheck()
        {

            return Player.HasStatusEffect(StatusEffectIds.Exgartuan) && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 1) == 0;
        }

        public bool CanMeditate()
        {

            return ReligionCheck() || EnlightenedCheck();
        }

        public void UpdateMasturbation()
        {

            CoC.Instance.Flags.Set(GameFlagIds.TimesMasturbated, CoC.Instance.Flags.Get(GameFlagIds.TimesMasturbated) + 1);
        }

        public void MasturbateMenu()
        {
            ClearOutput();

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.Ceraphchasity) > 0)
            {
                OutputText("Scenes.Masturbation.MasturbateMenu.OrgasmUnreachable".Localized());

                AddNextButton("Common.Button.Next".Localized(), PlayerMenu);

                if (CanMeditate())
                    AddNextButton("Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), () => Meditate());

                return ;
            }

            if (Player.HasStatusEffect(StatusEffectIds.ParasiteSlugReproduction))
            {
                OutputText("Scenes.Masturbation.MasturbateMenu.ProstateParasiteStirring".Localized());

                OutputText("Scenes.Masturbation.MasturbateMenu.OptionsWeighed".Localized());

                if (CanMeditate())
                    AddNextButton("Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), () => Meditate());

                AddNextButton("Common.Button.FistYourself".Localized(), FistForFullFeelings);

                if (Player.CountCocksOfType("TENTACLE") > 0)
                    AddNextButton("Common.Button.Tentabutt".Localized(), TentacleGoesUpYerPooperNewsAtEleven);

                if (Player.HasKeyItem("Dildo"))
                    AddNextButton("Common.Button.AnalDildo".Localized(), DildoButts);

                if (Player.CanOvipositBee() && Player.Lust >= 33 && Player.BiggestCockArea() > 100)
                    AddNextButton("Common.Button.Layincock".Localized(), GetHugeEggsInCawk);

                if (Player.HasKeyItem("All-Natural Self-Stimulation Belt") && Player.Vaginas.Count > 0 && !Player.IsTaur())
                    AddNextButton("Common.Button.AnStimBelt".Localized(), AllNaturalStimBeltUse);

                AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());

                return ;
            }

            if (Player.HasCock() && (Player.Cocks[0].Id == BodyPartTypes.Bee) && !FappingItems(false))
            {
                OutputText("Scenes.Masturbation.MasturbateMenu.SelfReliefBlocked".Localized());

                if (CanMeditate())
                {
                    OutputText("Scenes.Masturbation.MasturbateMenu.MeditationCleanses".Localized());

                    AddNextButton("Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), () => Meditate());
                }

                if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.SpecialHoney)))
                {
                    OutputText("Scenes.Masturbation.MasturbateMenu.HoneySmearOption".Localized(
                        Player.CockDescript()
                    ));

                    AddNextButton("Common.Button.UseHoney".Localized(), MasturbateGo);
                }

                AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());

                return ;
            }

            if (CanMeditate())
            {
                AddNextButton("Scenes.Kitsune.KitsuneShrine.ButtonMeditate".Localized(), () => Meditate());

                if (!ReligionCheck())
                    AddNextButton("Common.Button.Masturbate".Localized(), MasturbateGo);
            }
            else
                if (Player.StatusEffectValue(StatusEffectIds.ParasiteNephila, 0) >= 10 && Player.Lust >= 33 && Player.StatusEffectValue(StatusEffectIds.ParasiteNephilaNeedCum, 1) >= 1)
                {
                    OutputText("Scenes.Masturbation.MasturbateMenu.WombParasiteStirring".Localized());

                    OutputText("Scenes.Masturbation.MasturbateMenu.OptionsWeighedBold".Localized());

                    AddNextButton("Common.Button.Hunt".Localized(), NephilaHunt);
                }
                else
                    AddNextButton("Common.Button.Masturbate".Localized(), MasturbateGo);

            if (Player.HasCock() && (Player.HasPerk(PerkIds.Flexibility) || CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) > 0))
                AddNextButton("Common.Button.LickCock".Localized(), CatAutoLick);

            if (Player.HasVagina() && (Player.HasPerk(PerkIds.Flexibility) || CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) > 0))
                AddNextButton("Common.Button.LickGina".Localized(), LickYerGirlParts);

            if (Player.CountCocksOfType("TENTACLE") > 0 && Player.HasVagina())
                AddNextButton("Common.Button.Tentapussy".Localized(), TentacleSelfFuck);

            if (Player.CountCocksOfType("TENTACLE") > 0)
                AddNextButton("Common.Button.Tentabutt".Localized(), TentacleGoesUpYerPooperNewsAtEleven);

            if (Player.CanOvipositBee() && Player.Lust >= 33 && Player.BiggestCockArea() > 100)
                AddNextButton("Common.Button.Layincock".Localized(), GetHugeEggsInCawk);

            if (Player.CanOviposit() && Player.HasFuckableNipples() && Player.Lust >= 33 && Player.BiggestBreastSize() >= 21)
                AddNextButton("Common.Button.Layintits".Localized(), LayEggsInYerTits);

            if (Player.HasVagina() && Player.IsNaga() && Player.Lust >= 33)
                AddNextButton("Common.Button.NagaTail".Localized(), NagaTailsturbation);
            // 原版 Masturbation.hx:151 — 穿着修女服才出现「Habit」自慰选项
            if (Player.Armor.Id == ArmorIds.NaughtyNunsHabit)
            {
                var habitButton = AddNextButton("Common.Button.Habit".Localized(),

                    Contents.Items.Behaviors.Vanilla.NaughtyNunsHabitBehavior.MasturbationMenu);

                habitButton?.Hint = "Scenes.Masturbation.MasturbateMenu.HabitHint".Localized();
            }

            if (Player.HasCock() && OptionStore.Instance.Get("watersports", 0) > 0 && (Player.HasPerk(PerkIds.Flexibility) || Player.BiggestCockLength() >= Player.Tallness * 0.33f))
                AddNextButton("Scenes.Lumberjack.Button.Drink".Localized(), DrinkPiss);

            if (!Player.IsGenderless() && Player.Hunger < 5 && Player.Lust >= 33)
                AddNextButton("Common.Button.Starving".Localized(), HungerRub);

            if (!Player.IsGenderless() && (Player.HasItem(UseableIds.TeddyBear) || CoC.Instance.Flags.Get(GameFlagIds.BedBears) > 0) && Player.Lust >= 33)
                AddNextButton("Common.Button.UseBear".Localized(), FuckBear);

            if (FappingItems(false))
                AddNextButton("Common.Button.Items".Localized(), () => FappingItems());

            var buttons = Buttons.Where(b => b != null).ToList();

            if (buttons.Count == 1)
            {
                ClickButton(buttons[0]);

                return ;
            }

            AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
        }

        public bool FappingItems(bool menus = true)
        {

            if (menus)
                ClearButtons();

            var canReachCock = Player.Cocks.Count > 0 && (!Player.IsTaur() || Player.Cocks[Player.BiggestCockIndex()].Length >= Player.Tallness * (5 / 6));

            var hasItem = false;

            if (Player.HasKeyItem("Deluxe Dildo") && Player.HasVagina() && !Player.IsTaur())
            {
                if (menus)
                    AddNextButton("Common.Button.DDildo".Localized(), DeluxeDildo);

                hasItem = true;
            }

            if (Player.HasKeyItem("All-Natural Onahole") && canReachCock)
            {
                if (menus)
                    AddNextButton("Common.Button.AnOnahole".Localized(), AllNaturalOnaholeUse);

                hasItem = true;
            }

            if (Player.HasKeyItem("Deluxe Onahole") && canReachCock)
            {
                if (menus)
                    AddNextButton("Common.Button.DOnahole".Localized(), DeluxeOnaholeUse);

                hasItem = true;
            }

            if (Player.HasKeyItem("Plain Onahole") && canReachCock)
            {
                if (menus)
                    AddNextButton("Common.Button.Onahole".Localized(), OnaholeUse);

                hasItem = true;
            }

            if (Player.HasKeyItem("Self-Stimulation Belt") && Player.Vaginas.Count > 0 && !Player.IsTaur())
            {
                if (menus)
                    AddNextButton("Common.Button.StimBelt".Localized(), StimBeltUse);

                hasItem = true;
            }

            if (Player.HasKeyItem("All-Natural Self-Stimulation Belt") && Player.Vaginas.Count > 0 && !Player.IsTaur())
            {
                if (menus)
                    AddNextButton("Common.Button.AnStimBelt".Localized(), AllNaturalStimBeltUse);

                hasItem = true;
            }

            if (Player.HasKeyItem("Dual Belt") && Player.Gender == Gender.Hermaphrodite && !Player.IsTaur())
            {
                if (menus)
                    AddNextButton("Common.Button.DualBelt".Localized(), DualBeltMasturbation);

                hasItem = true;
            }

            if (Player.HasKeyItem("Fake Mare") && Player.HasCock() && Player.IsTaur())
            {
                if (menus)
                    AddNextButton("Common.Button.FakeMare".Localized(), CentaurDudesGetHorseAids);

                hasItem = true;
            }

            if (Player.HasKeyItem("Centaur Pole") && Player.HasVagina() && Player.IsTaur())
            {
                if (menus)
                    AddNextButton("Common.Button.CPole".Localized(), CentaurGirlsGetHorseAids);

                hasItem = true;
            }

            if (Player.HasKeyItem("Dildo"))
            {
                if (menus)
                    AddNextButton("Common.Button.AnalDildo".Localized(), DildoButts);

                hasItem = true;

                if (menus && Player.HasVagina())
                    AddNextButton("Common.Button.Dildo".Localized(), StickADildoInYourVagooSlut);
            }

            if (menus)
                AddButton(14, "Common.Button.Back".Localized(), () => MasturbateMenu());

            return hasItem;
        }

        public void Flagellate()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.Flagellate.KneelRepentant".Localized());

            OutputText("Scenes.Masturbation.Flagellate.TrialPreparation".Localized());

            OutputText("Scenes.Masturbation.Flagellate.FirstLashPain".Localized());

            OutputText("Scenes.Masturbation.Flagellate.SecondLashBlood".Localized());

            OutputText("Scenes.Masturbation.Flagellate.ThirdLashScream".Localized());

            OutputText("Scenes.Masturbation.Flagellate.SustainedScarring".Localized());

            OutputText("Scenes.Masturbation.Flagellate.WakingFocused".Localized());

            Player.ApplyStats(new DynStat(GameStats.Strength, -0.25f), new DynStat(GameStats.Speed, -0.25f), new DynStat(GameStats.Intelligence, -0.25f), new DynStat(GameStats.Toughness, -0.5f));

            Player.Lust = 0;

            ReturnToCamp(1);
        }

        public void DoStripCheck()
        {

            if (CoC.Instance.Flags.Get(GameFlagIds.CampBuiltCabin) <= 0 || RoomDungeon.InRoomedDungeon)
            {
                if (Player.Corruption < 15)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.HidingInRocks".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GooGarmentRemoval".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReach".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.ArmorRemoval".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.ParagraphBreak".Localized());
                }

                if (Player.Corruption >= 15 && Player.Corruption < 30)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.AloneCheck".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GooGarmentRemovalAlone".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachAlone".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.StripNaked".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.ParagraphBreakAgain".Localized());
                }

                if (Player.Corruption >= 30 && Player.Corruption < 60)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.HappilyPrefix".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalAnd".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachEager".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.ArmorRemovalEager".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.EagerPleasureSuffix".Localized());
                }

                if (Player.Corruption >= 60 && Player.Corruption < 80)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.YouPrefixWatched".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalWatched".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachWatched".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.StripNakedExaggerated".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.WatchingHopeSuffix".Localized());
                }

                if (Player.Corruption >= 80)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.YouPrefixSeductive".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalSeductive".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachSeductive".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.StripNakedComma".Localized());

                    if (Player.HasCock() || Player.HasVagina())
                        OutputText("Scenes.Masturbation.DoStripCheck.NaughtyBitsFondling".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.SeductiveLooksCast".Localized());
                }
            }
            else
            {
                if (Player.Corruption < 15)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.CabinEntryPrivate".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        OutputText("Scenes.Masturbation.DoStripCheck.YouPrefixShy".Localized());

                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalShy".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachShy".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.ArmorRemovalShy".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.ParagraphBreakThird".Localized());
                }

                if (Player.Corruption >= 15 && Player.Corruption < 30)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.CabinEntryForgotShutters".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        OutputText("Scenes.Masturbation.DoStripCheck.YouPrefixForgot".Localized());

                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalForgotShutters".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachForgotShutters".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.ArmorRemovalForgotShutters".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.ParagraphBreakFourth".Localized());
                }

                if (Player.Corruption >= 30 && Player.Corruption < 60)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.CabinEntryOpenShutters".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        OutputText("Scenes.Masturbation.DoStripCheck.YouPrefixOpen".Localized());

                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalOpenShutters".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachOpenShutters".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.ArmorRemovalHappy".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.ParagraphBreakFifth".Localized());
                }

                if (Player.Corruption >= 60 && Player.Corruption < 80)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.CabinEntryDoorOpen".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        OutputText("Scenes.Masturbation.DoStripCheck.YouPrefixDoorOpen".Localized());

                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalDoorOpen".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachDoorOpen".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.ArmorRemovalDoorOpen".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.ParagraphBreakSixth".Localized());
                }

                if (Player.Corruption >= 80)
                {
                    OutputText("Scenes.Masturbation.DoStripCheck.BedMovedWindowVisible".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                    {
                        OutputText("Scenes.Masturbation.DoStripCheck.YouPrefixWindow".Localized());

                        if (Player.LowerGarment != Undergarment.Nothing)
                            OutputText("Scenes.Masturbation.DoStripCheck.GarmentRemovalWindowView".Localized());

                        OutputText("Scenes.Masturbation.DoStripCheck.GroinReachWindowView".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DoStripCheck.ArmorRemovalExaggerated".Localized());

                    OutputText("Scenes.Masturbation.DoStripCheck.ParagraphBreakSeventh".Localized());
                }
            }
        }

        public void MasturbateGo()
        {
            ClearOutput();

            if (Player.HasStatusEffect(StatusEffectIds.Dysfunction))
            {
                OutputText("Scenes.Masturbation.MasturbateGo.NumbnessBlocksRelief".Localized());

                Go<CampScene>();

                return ;
            }

            if (Player.HasCock() && (Player.Cocks[0].Id == BodyPartTypes.Bee) && !(Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.SpecialHoney))))
            {
                OutputText("Scenes.Masturbation.MasturbateGo.BeeCockAchesHelplessly".Localized());

                Go<CampScene>();

                return ;
            }

            if (Player.IsTaur())
            {
                if (CentaurMasturbation())
                {
                    UpdateMasturbation();

                    ReturnToCamp(1);
                }
                else
                    Go<CampScene>();

                return ;
            }

            if (Player.Gender == Gender.Genderless)
            {
                GenderlessMasturbate();

                UpdateMasturbation();

                Player.ApplyStats(new DynStat(GameStats.Lust, -50f));

                ReturnToCamp(1);

                return ;
            }

            if (Player.HasStatusEffect(StatusEffectIds.Exgartuan) && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 1) == 0)
            {
                UpdateMasturbation();

                if (Player.IsNaga() && Random.Shared.Next(2) == 0 && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1)
                    SceneInstance<ExgartuanScene>.Get().ExgartuanNagaStoleMyMasturbation();
                else
                    SceneInstance<ExgartuanScene>.Get().ExgartuanMasturbation();

                return ;
            }

            if (Player.CountCockSocks("gilded") > 0 && CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) < Player.CountCockSocks("gilded"))
            {
                UpdateMasturbation();

                GildedCockTurbate();

                return ;
            }

            var autofellatio = false;

            var nippleFuck = false;

            DoStripCheck();

            TitForeplay();

            if (Player.Cocks.Count > 0)
            {
                if (Player.Cocks[0].Id == BodyPartTypes.Bee && (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)) || Player.HasItem(ItemRegistry.Create(ConsumableIds.SpecialHoney))))
                {
                    if (Player.HasItem(ItemRegistry.Create(ConsumableIds.BeeHoney)))
                        Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.BeeHoney), 1);
                    else
                        if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PureHoney)))
                            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PureHoney), 1);
                        else
                            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SpecialHoney)))
                                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SpecialHoney), 1);

                    OutputText("Scenes.Masturbation.MasturbateGo.HoneySoothesBeeCock".Localized());
                }

                if (Player.Cocks.Count == 1)
                {
                    OutputText("Scenes.Masturbation.MasturbateGo.StrokingSingleCock".Localized(
                        Player.CockDescript()
                    ));

                    if (Player.LibidoPercent < 45)
                        OutputText("Scenes.Masturbation.MasturbateGo.EagerSingleThrob".Localized());
                    else
                        if (Player.LibidoPercent < 70)
                            OutputText("Scenes.Masturbation.MasturbateGo.LanguidSingleHardness".Localized());
                        else
                            OutputText("Scenes.Masturbation.MasturbateGo.TeasingSinglePreDrip".Localized());
                }
                else
                {
                    OutputText("Scenes.Masturbation.MasturbateGo.StrokingTwinCocks".Localized(
                        Player.CockDescript()
                    ));

                    if (Player.LibidoPercent < 45)
                        OutputText("Scenes.Masturbation.MasturbateGo.EagerTwinThrob".Localized());
                    else
                        if (Player.LibidoPercent < 70)
                            OutputText("Scenes.Masturbation.MasturbateGo.LanguidTwinHardness".Localized());
                        else
                            OutputText("Scenes.Masturbation.MasturbateGo.TeasingTwinPrePool".Localized());
                }
            }

            if (Player.Gender == Gender.Hermaphrodite)
                ShowImage("masti-herm");
            else
                if (Player.Gender == Gender.Female)
                    ShowImage("masti-female");
                else
                    ShowImage("masti-male");

            if (Player.Vaginas.Count > 0)
            {
                if (Player.Vaginas.Count == 1)
                {
                    if (Player.LibidoPercent < 45)
                        OutputText("Scenes.Masturbation.MasturbateGo.PlayWithVagina".Localized());
                    else
                        if (Player.LibidoPercent < 70)
                            OutputText("Scenes.Masturbation.MasturbateGo.SlapPussySoftly".Localized());
                        else
                            OutputText("Scenes.Masturbation.MasturbateGo.RubArousedVagina".Localized());

                    switch (Player.Vaginas[0].Wetness)
                    {
                        case 0:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.ExpertArousePartsSingle".Localized());
                        }

                        break;

                        case 1:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.SighItBecomesMoist".Localized());
                        }

                        break;

                        case 2:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.GiggleDampFingers".Localized());
                        }

                        break;

                        case 3:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.SmileSlickFingers".Localized());
                        }

                        break;

                        case 4:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.SlickFingersInJuice".Localized());
                        }

                        break;
                    }

                    if (true )
                        OutputText("Scenes.Masturbation.MasturbateGo.LickLipsSmallSpurt".Localized());
                }

                if (Player.Vaginas.Count > 1)
                {
                    if (Player.LibidoPercent < 45)
                        OutputText("Scenes.Masturbation.MasturbateGo.PlayWithManyFolds".Localized());
                    else
                        if (Player.LibidoPercent < 70)
                            OutputText("Scenes.Masturbation.MasturbateGo.SlapPussiesSoftly".Localized());
                        else
                            OutputText("Scenes.Masturbation.MasturbateGo.RubArousedVaginas".Localized());

                    var cockId = Player.Vaginas[0].Wetness;

                    if (cockId == 0)
                        OutputText("Scenes.Masturbation.MasturbateGo.ExpertArousePartsPlural".Localized());

                    if (cockId == 1)
                        OutputText("Scenes.Masturbation.MasturbateGo.SighTheyBecomeMoist".Localized());

                    if (cockId == 2)
                        OutputText("Scenes.Masturbation.MasturbateGo.GiggleDampFingersPlural".Localized());

                    if (cockId == 3)
                        OutputText("Scenes.Masturbation.MasturbateGo.SmileSlickFingersPlural".Localized());

                    if (cockId == 4)
                        OutputText("Scenes.Masturbation.MasturbateGo.SlickFingersManyJuices".Localized());

                    if (true )
                        OutputText("Scenes.Masturbation.MasturbateGo.LickLipsManySpurts".Localized());
                }
            }

            if (Player.Cocks.Count == 1)
            {
                OutputText("Scenes.Masturbation.MasturbateGo.ParagraphBreak".Localized());

                if (Player.Cocks[0].Thickness < 1.8f)
                    OutputText("Scenes.Masturbation.MasturbateGo.EasilyWrapHandAround".Localized(
                        Player.CockDescript()
                    ));
                else
                    if (Player.Cocks[0].Thickness < 3)
                    {
                        OutputText("Scenes.Masturbation.MasturbateGo.StruggleFitHand".Localized(
                            Player.CockDescript()
                        ));

                        if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                            OutputText("Scenes.Masturbation.MasturbateGo.AnimalisticWord".Localized());
                        else
                            if (Player.HasKnot())
                                OutputText("Scenes.Masturbation.MasturbateGo.BulbousBeastEpithet".Localized());
                            else
                                OutputText("Scenes.Masturbation.MasturbateGo.LargeWord".Localized());

                        OutputText("Scenes.Masturbation.MasturbateGo.EndowmentSuffix".Localized());
                    }
                    else
                        if (Player.Cocks[0].Thickness < 5)
                            OutputText("Scenes.Masturbation.MasturbateGo.GripWithBothHands".Localized(
                                Player.CockDescript()
                            ));
                        else
                            OutputText("Scenes.Masturbation.MasturbateGo.FailsToEncircle".Localized(
                                Player.CockDescript()
                            ));

                if (Player.Cocks[0].Length < 12)
                {
                    if (Player.CountCocksOfType("Human") >= 1)
                        OutputText("Scenes.Masturbation.MasturbateGo.PleasureSensitiveDick".Localized());
                    else
                        if (Player.CountCocksOfType("Horse") >= 1)
                            OutputText("Scenes.Masturbation.MasturbateGo.PleasureHorsefleshSheath".Localized());
                        else
                            if (Player.DogCocks() >= 1)
                                OutputText("Scenes.Masturbation.MasturbateGo.PleasureCanineErection".Localized());
                            else
                                if (Player.CountCocksOfType("TENTACLE") >= 1)
                                    OutputText("Scenes.Masturbation.MasturbateGo.PleasureTentacleLength".Localized());
                                else
                                    if (Player.CountCocksOfType("DEMON") >= 1)
                                        OutputText("Scenes.Masturbation.MasturbateGo.PleasureDemonicRidges".Localized());
                                    else
                                        if (Player.CountCocksOfType("Cat") >= 1)
                                            OutputText("Scenes.Masturbation.MasturbateGo.FeelTinyBarbs".Localized(
                                                Player.CockDescript()
                                            ));
                                        else
                                            if (Player.CountCocksOfType("LIZARD") >= 1)
                                                OutputText("Scenes.Masturbation.MasturbateGo.PleasureRidgedShaft".Localized(
                                                    Player.CockDescript()
                                                ));
                                            else
                                                if (Player.CountCocksOfType("ANEMONE") >= 1)
                                                    OutputText("Scenes.Masturbation.MasturbateGo.GaspStingingTentacles".Localized(
                                                        Player.CockDescript()
                                                    ));
                                                else
                                                    if (Player.CountCocksOfType("DISPLACER") >= 1)
                                                        OutputText("Scenes.Masturbation.MasturbateGo.PleasureAlienEndowment".Localized());
                                                    else
                                                        OutputText("Scenes.Masturbation.MasturbateGo.PleasureSensitiveDickAgain".Localized());
                }
                else
                    if (Player.Cocks[0].Length < 20)
                    {
                        if (Player.CountCocksOfType("Human") >= 1)
                            OutputText("Scenes.Masturbation.MasturbateGo.TeaseCrownDollops".Localized(
                                Player.CockDescript()
                            ));
                        else
                            if (Player.CountCocksOfType("Horse") >= 1)
                                OutputText("Scenes.Masturbation.MasturbateGo.TeaseFlaredTip".Localized(
                                    Player.CockDescript()
                                ));
                            else
                                if (Player.DogCocks() >= 1)
                                    OutputText("Scenes.Masturbation.MasturbateGo.TeasePointedTip".Localized(
                                        Player.CockDescript()
                                    ));
                                else
                                    if (Player.CountCocksOfType("TENTACLE") >= 1)
                                        OutputText("Scenes.Masturbation.MasturbateGo.TeaseMushroomTip".Localized(
                                            Player.CockDescript()
                                        ));
                                    else
                                        if (Player.CountCocksOfType("DEMON") >= 1)
                                            OutputText("Scenes.Masturbation.MasturbateGo.TeaseCrownRingBumps".Localized(
                                                Player.CockDescript()
                                            ));
                                        else
                                            if (Player.CountCocksOfType("Cat") >= 1)
                                                OutputText("Scenes.Masturbation.MasturbateGo.TeaseSensitiveNubs".Localized(
                                                    Player.CockDescript()
                                                ));
                                            else
                                                if (Player.CountCocksOfType("LIZARD") >= 1)
                                                    OutputText("Scenes.Masturbation.MasturbateGo.TeaseRoundedBulbs".Localized(
                                                        Player.CockDescript()
                                                    ));
                                                else
                                                    if (Player.CountCocksOfType("ANEMONE") >= 1)
                                                        OutputText("Scenes.Masturbation.MasturbateGo.SqueezeStingingTentacles".Localized(
                                                            Player.CockDescript()
                                                        ));
                                                    else
                                                        if (Player.CountCocksOfType("DISPLACER") >= 1)
                                                            OutputText("Scenes.Masturbation.MasturbateGo.TeaseOpenedTip".Localized(
                                                                Player.CockDescript()
                                                            ));
                                                        else
                                                            OutputText("Scenes.Masturbation.MasturbateGo.TeaseCrownDollopsAgain".Localized(
                                                                Player.CockDescript()
                                                            ));
                    }
                    else
                        if (Player.Cocks[0].Length < 26)
                        {
                            if (Player.CountCocksOfType("Human") >= 1)
                                OutputText("Scenes.Masturbation.MasturbateGo.HeadWobblesPreBead".Localized(
                                    Player.CockDescript()
                                ));
                            else
                                if (Player.CountCocksOfType("Horse") >= 1)
                                    OutputText("Scenes.Masturbation.MasturbateGo.FlaredTipWobbles".Localized(
                                        Player.CockDescript()
                                    ));
                                else
                                    if (Player.DogCocks() >= 1)
                                        OutputText("Scenes.Masturbation.MasturbateGo.PointedTipLeaksPre".Localized(
                                            Player.CockDescript()
                                        ));
                                    else
                                        if (Player.CountCocksOfType("TENTACLE") >= 1)
                                            OutputText("Scenes.Masturbation.MasturbateGo.WideHeadBumpsLips".Localized(
                                                Player.CockDescript()
                                            ));
                                        else
                                            if (Player.CountCocksOfType("DEMON") >= 1)
                                                OutputText("Scenes.Masturbation.MasturbateGo.PurpleHeadFlushes".Localized(
                                                    Player.CockDescript()
                                                ));
                                            else
                                                if (Player.CountCocksOfType("Cat") >= 1)
                                                    OutputText("Scenes.Masturbation.MasturbateGo.PointedTipBloodFlush".Localized(
                                                        Player.CockDescript()
                                                    ));
                                                else
                                                    if (Player.CountCocksOfType("LIZARD") >= 1)
                                                        OutputText("Scenes.Masturbation.MasturbateGo.KnottedPurpleTip".Localized(
                                                            Player.CockDescript()
                                                        ));
                                                    else
                                                        if (Player.CountCocksOfType("ANEMONE") >= 1)
                                                            OutputText("Scenes.Masturbation.MasturbateGo.TentacleRingTingles".Localized(
                                                                Player.CockDescript()
                                                            ));
                                                        else
                                                            if (Player.CountCocksOfType("DISPLACER") >= 1)
                                                                OutputText("Scenes.Masturbation.MasturbateGo.BloomingTipLeaks".Localized(
                                                                    Player.CockDescript()
                                                                ));
                                                            else
                                                                OutputText("Scenes.Masturbation.MasturbateGo.HeadWobblesBeadAgain".Localized(
                                                                    Player.CockDescript()
                                                                ));

                            if (Player.HasFuckableNipples() && Player.BiggestBreastSize() >= 3)
                            {
                                nippleFuck = true;

                                TitFuckSingle();
                            }
                            else
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SwallowTipMilkPre".Localized(
                                    Player.CockDescript()
                                ));

                                autofellatio = true;
                            }

                            if (Player.CanTitFuck() && Player.BiggestBreastSize() > 3 && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SelfTitfuckSingle".Localized(
                                    Player.CockDescript()
                                ));

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.MilkJetsNipples".Localized());
                            }
                        }
                        else
                        {
                            if (Player.CountCocksOfType("Human") >= 1)
                                OutputText("Scenes.Masturbation.MasturbateGo.HeadSmearsPre".Localized(
                                    Player.CockDescript()
                                ));
                            else
                                if (Player.CountCocksOfType("Horse") >= 1)
                                    OutputText("Scenes.Masturbation.MasturbateGo.FlaredTipSmearsPre".Localized(
                                        Player.CockDescript()
                                    ));
                                else
                                    if (Player.DogCocks() >= 1)
                                        OutputText("Scenes.Masturbation.MasturbateGo.HeadSmearsPreAgain".Localized(
                                            Player.CockDescript(),

                                            Player.CockDescript()
                                        ));
                                    else
                                        if (Player.CountCocksOfType("TENTACLE") >= 1)
                                            OutputText("Scenes.Masturbation.MasturbateGo.MushroomHeadPressesFace".Localized(
                                                Player.CockDescript()
                                            ));
                                        else
                                            if (Player.CountCocksOfType("DEMON") >= 1)
                                                OutputText("Scenes.Masturbation.MasturbateGo.TaintedHeadPressesFace".Localized(
                                                    Player.CockDescript()
                                                ));
                                            else
                                                if (Player.CountCocksOfType("Cat") >= 1)
                                                    OutputText("Scenes.Masturbation.MasturbateGo.PointedTipTingles".Localized(
                                                        Player.CockDescript(),

                                                        Player.CockDescript()
                                                    ));
                                                else
                                                    if (Player.CountCocksOfType("LIZARD") >= 1)
                                                        OutputText("Scenes.Masturbation.MasturbateGo.PointedTipEagerLips".Localized(
                                                            Player.CockDescript()
                                                        ));
                                                    else
                                                        if (Player.CountCocksOfType("ANEMONE") >= 1)
                                                            OutputText("Scenes.Masturbation.MasturbateGo.TentacleRingSlidesFace".Localized(
                                                                Player.CockDescript()
                                                            ));
                                                        else
                                                            if (Player.CountCocksOfType("DISPLACER") >= 1)
                                                                OutputText("Scenes.Masturbation.MasturbateGo.HeadSmearsPreThird".Localized(
                                                                    Player.CockDescript(),

                                                                    Player.CockDescript()
                                                                ));
                                                            else
                                                                OutputText("Scenes.Masturbation.MasturbateGo.HeadSmearsPreFourth".Localized(
                                                                    Player.CockDescript()
                                                                ));

                            if (Player.HasFuckableNipples() && Player.BiggestBreastSize() >= 3)
                            {
                                nippleFuck = true;

                                TitFuckSingle();
                            }
                            else
                            {
                                if (Player.Corruption > 60)
                                    if (Player.CountCocksOfType("Human") >= 1)
                                        OutputText("Scenes.Masturbation.MasturbateGo.CockAromaDeepthroat".Localized());
                                    else
                                        if (Player.CountCocksOfType("Horse") >= 1)
                                            OutputText("Scenes.Masturbation.MasturbateGo.ThickAnimalScent".Localized());
                                        else
                                            if (Player.DogCocks() >= 1)
                                                OutputText("Scenes.Masturbation.MasturbateGo.StrongAnimalScent".Localized());
                                            else
                                                if (Player.CountCocksOfType("TENTACLE") >= 1)
                                                    OutputText("Scenes.Masturbation.MasturbateGo.SweetScentTentacleThroat".Localized(
                                                        Player.CockDescript()
                                                    ));
                                                else
                                                    if (Player.CountCocksOfType("DEMON") >= 1)
                                                        OutputText("Scenes.Masturbation.MasturbateGo.SpicyDemonicOdor".Localized(
                                                            Player.CockDescript(),

                                                            Player.CockDescript()
                                                        ));
                                                    else
                                                        if (Player.CountCocksOfType("Cat") >= 1)
                                                            OutputText("Scenes.Masturbation.MasturbateGo.TangyCockScent".Localized());
                                                        else
                                                            if (Player.CountCocksOfType("LIZARD") >= 1)
                                                                OutputText("Scenes.Masturbation.MasturbateGo.SaltyDryOdor".Localized(
                                                                    Player.CockDescript()
                                                                ));
                                                            else
                                                                if (Player.CountCocksOfType("ANEMONE") >= 1)
                                                                    OutputText("Scenes.Masturbation.MasturbateGo.StingingTentacleThroat".Localized(
                                                                        Player.CockDescript()
                                                                    ));
                                                                else
                                                                    if (Player.CountCocksOfType("DISPLACER") >= 1)
                                                                        OutputText("Scenes.Masturbation.MasturbateGo.StrongAnimalScentAgain".Localized());
                                                                    else
                                                                        OutputText("Scenes.Masturbation.MasturbateGo.CockAromaDeepthroatAgain".Localized());

                                autofellatio = true;
                            }

                            if (Player.CanTitFuck() && Player.BiggestBreastSize() > 3 && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.BreastPillowsWrapShaft".Localized(
                                    Player.CockDescript()
                                ));

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.MilkJetsNipplesAgain".Localized());
                            }
                        }
            }
            else
                if (Player.Cocks.Count == 2)
                {
                    if (Player.Cocks.Exists(c => c.SheathOn))
                        OutputText("Scenes.Masturbation.MasturbateGo.TraceSheathTwinSpasms".Localized());

                    OutputText("Scenes.Masturbation.MasturbateGo.GraspCockEachHand".Localized());

                    if (Player.AverageCockThickness() <= 1.8f)
                        OutputText("Scenes.Masturbation.MasturbateGo.HoldBothTools".Localized());
                    else
                        OutputText("Scenes.Masturbation.MasturbateGo.SqueezeTwinTools".Localized());

                    if (Player.AverageCockLength() <= 10)
                        OutputText("Scenes.Masturbation.MasturbateGo.HipsForceFuckHands".Localized());
                    else
                        if (Player.AverageCockLength() < 20)
                        {
                            OutputText("Scenes.Masturbation.MasturbateGo.LickTremblingCocktip".Localized());

                            if (Player.HasFuckableNipples())
                                if (Player.DogCocks() > 0)
                                {
                                    if (Player.HasFuckableNipples())
                                    {
                                        nippleFuck = true;

                                        if (MultiTitFuck())
                                            autofellatio = true;
                                    }
                                    else
                                        if (Player.DogCocks() != Player.Cocks.Count)
                                        {
                                            nippleFuck = true;

                                            autofellatio = true;

                                            if (Player.CountCocksOfType("Human") > 0)
                                                TitFuckSingle();
                                        }
                                }
                                else
                                {
                                    nippleFuck = true;

                                    if (MultiTitFuck())
                                        autofellatio = true;
                                }

                            if (Player.CanTitFuck() && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SandwichTongueTips".Localized());

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.MilkJetsNipplesThird".Localized());
                            }

                            if (Player.CanTitFuck() && Player.MostBreastsPerRow() > 2 && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SqueezeBreastsLickSuck".Localized(
                                    Player.BreastRows.Count()
                                ));

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.ManyNipplesJets".Localized());
                            }

                            if (Player.BiggestBreastSize() <= 3 && !nippleFuck)
                                OutputText("Scenes.Masturbation.MasturbateGo.NoTitsSmear".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Masturbation.MasturbateGo.CocksBrushFaceSuck".Localized());

                            if (Player.HasFuckableNipples())
                                if (Player.DogCocks() > 0)
                                {
                                    if (Player.HasFuckableNipples())
                                    {
                                        nippleFuck = true;

                                        if (MultiTitFuck())
                                            autofellatio = true;
                                    }
                                    else
                                        if (Player.DogCocks() != Player.Cocks.Count)
                                        {
                                            nippleFuck = true;

                                            autofellatio = true;

                                            if (Player.CountCocksOfType("Human") > 0 || Player.CountCocksOfType("Horse") > 0)
                                                TitFuckSingle();
                                        }
                                }
                                else
                                {
                                    nippleFuck = true;

                                    if (MultiTitFuck())
                                        autofellatio = true;
                                }

                            if (Player.CanTitFuck() && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SandwichMouthfuck".Localized());

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.MilkJetsNipplesFourth".Localized());
                            }

                            if (Player.MostBreastsPerRow() > 2 && Player.BiggestBreastSize() > 3 && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SqueezeBreastsMouthfuck".Localized(
                                    Player.BreastRows.Count()
                                ));

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.ManyNipplesJetsAgain".Localized());
                            }

                            if (Player.BiggestBreastSize() <= 3 && !nippleFuck)
                                OutputText("Scenes.Masturbation.MasturbateGo.NoTitsSmearAgain".Localized());
                        }
                }
                else
                    if (Player.CockTotal() >= 3)
                    {
                        if (Player.Cocks.Exists(c => c.SheathOn))
                            OutputText("Scenes.Masturbation.MasturbateGo.TraceSheathCockSpasms".Localized());

                        OutputText("Scenes.Masturbation.MasturbateGo.GraspEachCockAgain".Localized());

                        if (Player.AverageCockThickness() <= 1.8f)
                            OutputText("Scenes.Masturbation.MasturbateGo.HoldManyTools".Localized(
                                NumberUtils.Num2Text(Player.Cocks.Count)
                            ));
                        else
                            OutputText("Scenes.Masturbation.MasturbateGo.SqueezeCockBundle".Localized());

                        if (Player.AverageCockLength() <= 10)
                            OutputText("Scenes.Masturbation.MasturbateGo.HipsFuckInvoluntary".Localized());
                        else
                            if (Player.AverageCockLength() < 20)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.LickTremblingCocktipAgain".Localized());

                                if (Player.HasFuckableNipples())
                                    if (Player.DogCocks() > 0)
                                    {
                                        if (Player.HasFuckableNipples())
                                        {
                                            nippleFuck = true;

                                            if (MultiTitFuck())
                                                autofellatio = true;
                                        }
                                        else
                                            if (Player.DogCocks() != Player.Cocks.Count)
                                            {
                                                nippleFuck = true;

                                                autofellatio = true;

                                                if ((Player.CountCocksOfType("Human") > 0 || Player.CountCocksOfType("Horse") > 0) && Player.Cocks.Count - Player.DogCocks() == 1)
                                                    TitFuckSingle();

                                                if (Player.Cocks.Count - Player.DogCocks() > 1)
                                                    MultiTitFuck();
                                            }
                                    }
                                    else
                                    {
                                        nippleFuck = true;

                                        if (MultiTitFuck())
                                            autofellatio = true;
                                    }

                                if (Player.CanTitFuck() && !nippleFuck)
                                {
                                    OutputText("Scenes.Masturbation.MasturbateGo.SandwichTongueAgain".Localized());

                                    if (Player.BiggestLactation() > 0)
                                        OutputText("Scenes.Masturbation.MasturbateGo.MilkJetsNipplesFifth".Localized());
                                }

                                if (Player.MostBreastsPerRow() > 2 && Player.BiggestBreastSize() > 3 && !nippleFuck)
                                {
                                    OutputText("Scenes.Masturbation.MasturbateGo.SqueezeBreastsLickAgain".Localized(
                                        Player.BreastRows.Count()
                                    ));

                                    if (Player.BiggestLactation() > 0)
                                        OutputText("Scenes.Masturbation.MasturbateGo.ManyNipplesJetsThird".Localized());
                                }

                                if (Player.BiggestBreastSize() <= 3 && !nippleFuck)
                                    OutputText("Scenes.Masturbation.MasturbateGo.NoTitsSmearThird".Localized());
                            }

                        if (Player.AverageCockLength() >= 20)
                        {
                            OutputText("Scenes.Masturbation.MasturbateGo.CocksBrushSuckAgain".Localized());

                            if (Player.HasFuckableNipples())
                                if (Player.DogCocks() > 0)
                                {
                                    if (Player.HasFuckableNipples())
                                    {
                                        nippleFuck = true;

                                        if (MultiTitFuck())
                                            autofellatio = true;
                                    }
                                    else
                                        if (Player.DogCocks() != Player.Cocks.Count)
                                        {
                                            nippleFuck = true;

                                            autofellatio = true;

                                            if ((Player.CountCocksOfType("Human") > 0 || Player.CountCocksOfType("Horse") > 0) && Player.Cocks.Count - Player.DogCocks() == 1)
                                                TitFuckSingle();

                                            if (Player.Cocks.Count - Player.DogCocks() > 1)
                                                MultiTitFuck();
                                        }
                                }
                                else
                                {
                                    nippleFuck = true;

                                    if (MultiTitFuck())
                                        autofellatio = true;
                                }

                            if (Player.CanTitFuck() && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SandwichMouthfuckAgain".Localized());

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.MilkJetsNipplesSixth".Localized());
                            }

                            if (Player.MostBreastsPerRow() > 2 && Player.BiggestBreastSize() > 3 && !nippleFuck)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.SqueezeBreastsMouthfuckAgain".Localized(
                                    Player.BreastRows.Count()
                                ));

                                if (Player.BiggestLactation() > 0)
                                    OutputText("Scenes.Masturbation.MasturbateGo.ManyNipplesJetsFourth".Localized());
                            }

                            if (Player.BiggestBreastSize() <= 3 && !nippleFuck)
                                OutputText("Scenes.Masturbation.MasturbateGo.NoTitsSmearFourth".Localized());
                        }
                    }

            if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Masturbation.MasturbateGo.ParagraphBreakAgain".Localized());

                if (Player.Vaginas.Count == 1)
                {
                    OutputText("Scenes.Masturbation.MasturbateGo.RoamOverPussyLips".Localized());

                    if (Player.GetClitLength() < 1.5f)
                        OutputText("Scenes.Masturbation.MasturbateGo.TeasePleasureBud".Localized());
                    else
                        if (Player.GetClitLength() < 4.5f)
                        {
                            OutputText("Scenes.Masturbation.MasturbateGo.LargeClitPokingOut".Localized());

                            if (Player.Vaginas[0].Wetness > 0)
                                OutputText("Scenes.Masturbation.MasturbateGo.GlisteningWord".Localized());

                            OutputText("Scenes.Masturbation.MasturbateGo.GrowClitToCock".Localized());
                        }
                        else
                            OutputText("Scenes.Masturbation.MasturbateGo.EngorgedCockSizedClit".Localized());

                    switch (Player.Vaginas[0].Wetness)
                    {
                        case 0:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.DryPussyStruggle".Localized());
                        }

                        break;

                        case 1:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.HornyPussHoneypot".Localized());
                        }

                        break;

                        case 2:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.MoistCleftDemandsFingers".Localized());
                        }

                        break;

                        case 3:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.SlickCuntAchesFilling".Localized());
                        }

                        break;

                        case 4:
                        {
                        OutputText("Scenes.Masturbation.MasturbateGo.WetnessRunsDownLegs".Localized());
                        }

                        break;
                    }

                    if (true )
                        OutputText("Scenes.Masturbation.MasturbateGo.HeavyArousalGirlLube".Localized());
                }
            }

            Orgazmo(autofellatio, nippleFuck);

            OutputText("Scenes.Masturbation.MasturbateGo.ParagraphBreakThird".Localized());

            if (Player.Cocks.Count > 0)
            {
                if (Player.Cocks.Count == 1)
                {
                    if (Player.LibidoPercent < 30)
                        OutputText("Scenes.Masturbation.MasturbateGo.FallAsleepSpent".Localized());
                    else
                        if (Player.LibidoPercent < 55)
                            OutputText("Scenes.Masturbation.MasturbateGo.RollDozeSemiErect".Localized(
                                Player.CockDescript()
                            ));
                        else
                            if (Player.LibidoPercent <= 80)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.CloseEyesSurgeErect".Localized(
                                    Player.CockDescript()
                                ));

                                if (Player.Corruption < 50)
                                    OutputText("Scenes.Masturbation.MasturbateGo.DreamsFilledWithSex".Localized());
                                else
                                    OutputText("Scenes.Masturbation.MasturbateGo.DreamDepravedFantasia".Localized());
                            }
                            else
                                OutputText("Scenes.Masturbation.MasturbateGo.GroanSleepRigidPulse".Localized(
                                    Player.CockDescript()
                                ));
                }
                else
                    if (Player.LibidoPercent < 30)
                        OutputText("Scenes.Masturbation.MasturbateGo.FallAsleepSpentAgain".Localized());
                    else
                        if (Player.LibidoPercent < 55)
                            OutputText("Scenes.Masturbation.MasturbateGo.DozeHourCocksFlop".Localized());
                        else
                            if (Player.LibidoPercent <= 80)
                            {
                                OutputText("Scenes.Masturbation.MasturbateGo.CloseEyesDicksSurge".Localized());

                                if (Player.Corruption < 50)
                                    OutputText("Scenes.Masturbation.MasturbateGo.DreamsFilledSexAgain".Localized());
                                else
                                    OutputText("Scenes.Masturbation.MasturbateGo.DreamDepravedFantasiaAgain".Localized());
                            }
                            else
                                OutputText("Scenes.Masturbation.MasturbateGo.CatnapRigidErections".Localized());
            }
            else
                if (Player.HasVagina())
                    OutputText("Scenes.Masturbation.MasturbateGo.SighQuickNap".Localized());
                else
                    OutputText("Scenes.Masturbation.MasturbateGo.SighDriftOffSleep".Localized());

            ReturnToCamp(1);
        }

        public void GenderlessMasturbate()
        {
            ClearOutput();

            ShowImage("masti-genderless-fuck");

            DoStripCheck();

            TitForeplay();

            if (!Player.HasStatusEffect(StatusEffectIds.FappedGenderless))
            {
                OutputText("Scenes.Masturbation.GenderlessMasturbate.UnsureHowToMasturbate".Localized());

                Player.CreateStatusEffect(StatusEffectIds.FappedGenderless, 0, 0, 0, 0);
            }

            OutputText("Scenes.Masturbation.GenderlessMasturbate.AssholeBeginsToTwitch".Localized());

            OutputText("Scenes.Masturbation.GenderlessMasturbate.LaySidewaysReachBack".Localized());

            if (Player.Anal.Wetness < 2)
            {
                OutputText("Scenes.Masturbation.GenderlessMasturbate.HandToMouth".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.MiddleFinger".Localized());
                else
                    if (Player.Anal.Looseness == 3)
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.FirstTwoFingers".Localized());
                    else
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.FirstHandWord".Localized());

                OutputText("Scenes.Masturbation.GenderlessMasturbate.CoatedInSaliva".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.GenderlessMasturbate.LubricationEasesEntry".Localized());

                if (Player.Anal.Looseness >= 3)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.PluralSLetter".Localized());

                OutputText("Scenes.Masturbation.GenderlessMasturbate.ShiverUpYourSpine".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.ToYourKnuckle".Localized());
                else
                    if (Player.Anal.Looseness == 3)
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.ToFirstTwoKnuckles".Localized());
                    else
                        if (Player.Anal.Looseness == 4)
                            OutputText("Scenes.Masturbation.GenderlessMasturbate.ThreeKnuckles".Localized());
                        else
                            OutputText("Scenes.Masturbation.GenderlessMasturbate.FourFingersDeep".Localized());

                OutputText("Scenes.Masturbation.GenderlessMasturbate.SlowlyPushAndPull".Localized());

                if (Player.Anal.Looseness >= 3)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.RepeatedPluralSLetter".Localized());

                OutputText("Scenes.Masturbation.GenderlessMasturbate.PickingUpThePace".Localized());
            }

            if (Player.Anal.Looseness == 5)
                OutputText("Scenes.Masturbation.GenderlessMasturbate.DevilishThought".Localized());

            OutputText("Scenes.Masturbation.GenderlessMasturbate.PleasureFillsYourBody".Localized());

            if (Player.Anal.Looseness <= 2)
                OutputText("Scenes.Masturbation.GenderlessMasturbate.DigitSingular".Localized());
            else
                if (Player.Anal.Looseness < 5)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.DigitPlural".Localized());
                else
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.SecondHandWord".Localized());

            OutputText("Scenes.Masturbation.GenderlessMasturbate.ToesCurlOrgasm".Localized());

            if (Player.Anal.Wetness >= 2)
            {
                OutputText("Scenes.Masturbation.GenderlessMasturbate.PullingOutYourHand".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.FingerSingular".Localized());
                else
                    if (Player.Anal.Looseness < 5)
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.FingerPlural".Localized());
                    else
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.ThirdHandWord".Localized());

                OutputText("Scenes.Masturbation.GenderlessMasturbate.CoatedInWarmLube".Localized());

                if (Player.LibidoPercent >= 75)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.SleepWithWetDreams".Localized());
                else
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.CompletelySpentSleep".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.GenderlessMasturbate.WithdrawHandToDry".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.SecondFingerSingular".Localized());
                else
                    if (Player.Anal.Looseness < 5)
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.SecondFingerPlural".Localized());
                    else
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.FourthHandWord".Localized());

                OutputText("Scenes.Masturbation.GenderlessMasturbate.WipeItDry".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.PronounIt".Localized());
                else
                    if (Player.Anal.Looseness < 5)
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.PronounThey".Localized());
                    else
                        OutputText("Scenes.Masturbation.GenderlessMasturbate.PronounItAgain".Localized());

                OutputText("Scenes.Masturbation.GenderlessMasturbate.OffWord".Localized());

                if (Player.LibidoPercent > 75)
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.HoleReadyForMore".Localized());
                else
                    OutputText("Scenes.Masturbation.GenderlessMasturbate.SatisfiedDriftOff".Localized());
            }
        }

        public void TitForeplay()
        {

            if (Player.BreastRows.Count == 1 && Player.BiggestBreastSize() > 3)
            {
                if (Player.LibidoPercent < 45)
                    OutputText("Scenes.Masturbation.TitForeplay.CaressBreastsGently".Localized());
                else
                    if (Player.LibidoPercent < 70)
                        OutputText("Scenes.Masturbation.TitForeplay.GropeBreastsBothHands".Localized());
                    else
                        OutputText("Scenes.Masturbation.TitForeplay.SqueezeBreastsBrutally".Localized());
            }
            else
                if (Player.BreastRows.Count > 1 && Player.BiggestBreastSize() > 3)
                {
                    if (Player.LibidoPercent < 45)
                        OutputText("Scenes.Masturbation.TitForeplay.HesitantTracingCaress".Localized(
                            NumberUtils.Num2Text(Player.BreastRows.Count())
                        ));
                    else
                        if (Player.LibidoPercent < 70)
                            OutputText("Scenes.Masturbation.TitForeplay.SqueezeEachBreast".Localized(
                                NumberUtils.Num2Text(Player.BreastRows.Count())
                            ));
                        else
                            OutputText("Scenes.Masturbation.TitForeplay.GropeBreastsInTurn".Localized(
                                NumberUtils.Num2Text(Player.BreastRows.Count())
                            ));

                    if (Player.Corruption <= 25)
                        OutputText("Scenes.Masturbation.TitForeplay.ShamefulTactileProof".Localized(
                            Appearance.AllBreastsDescript(Player)
                        ));
                    else
                        if (Player.Corruption < 75)
                            OutputText("Scenes.Masturbation.TitForeplay.HorrifiedYetMashing".Localized(
                                Appearance.AllBreastsDescript(Player)
                            ));
                        else
                            OutputText("Scenes.Masturbation.TitForeplay.ThrillAtAbnormalTits".Localized());
                }

            if (Player.HasFuckableNipples() && Player.BiggestBreastSize() > 3)
            {
                if (Player.BreastRows.Count >= 1)
                    OutputText("Scenes.Masturbation.TitForeplay.AttentionToNipples".Localized());

                if (Player.LibidoPercent < 60)
                    OutputText("Scenes.Masturbation.TitForeplay.HesitantFingerRub".Localized());
                else
                    OutputText("Scenes.Masturbation.TitForeplay.EagerRoughStroking".Localized());

                if (Player.AverageVaginalLooseness() < 2)
                    OutputText("Scenes.Masturbation.TitForeplay.DelicateNippleOpenings".Localized());
                else
                    if (Player.AverageVaginalLooseness() < 4)
                        OutputText("Scenes.Masturbation.TitForeplay.PuckeredNippleHoles".Localized());
                    else
                        if (Player.AverageNipplesPerBreast() > 1)
                            OutputText("Scenes.Masturbation.TitForeplay.ManyWord".Localized());
                        else
                            OutputText("Scenes.Masturbation.TitForeplay.SwollenCuntLips".Localized());

                if (Player.SensitivityPercent < 45)
                    OutputText("Scenes.Masturbation.TitForeplay.SighContentedly".Localized());
                else
                    if (Player.SensitivityPercent < 70)
                        OutputText("Scenes.Masturbation.TitForeplay.MoanLewdly".Localized());
                    else
                        OutputText("Scenes.Masturbation.TitForeplay.SquealWithPleasure".Localized());

                if (Player.AverageVaginalWetness() < 2)
                    OutputText("Scenes.Masturbation.TitForeplay.NipplesLoosenUp".Localized());
                else
                    if (Player.AverageVaginalWetness() < 4)
                        OutputText("Scenes.Masturbation.TitForeplay.SpreadNippleSecretions".Localized());
                    else
                    {
                        OutputText("Scenes.Masturbation.TitForeplay.LittleSpurts".Localized());

                        if (Player.AverageLactation() > 0)
                            OutputText("Scenes.Masturbation.TitForeplay.MilkyWord".Localized());

                        OutputText("Scenes.Masturbation.TitForeplay.GirlcumSquirtsOut".Localized());
                    }

                if (Player.AverageLactation() > 0 && Player.AverageLactation() < 2)
                    OutputText("Scenes.Masturbation.TitForeplay.MammariesDripSticky".Localized());
                else
                    if (Player.AverageLactation() < 3)
                        OutputText("Scenes.Masturbation.TitForeplay.MammariesLeakStreams".Localized());
                    else
                        OutputText("Scenes.Masturbation.TitForeplay.MammariesConstantlyDrool".Localized());
            }
            else
                if (Player.BreastRows.Count > 0 && Player.BiggestBreastSize() > 3)
                {
                    OutputText("Scenes.Masturbation.TitForeplay.EmittingWord".Localized());

                    if (Player.SensitivityPercent < 45)
                        OutputText("Scenes.Masturbation.TitForeplay.SoftGaspsOfPleasure".Localized());
                    else
                        if (Player.SensitivityPercent < 70)
                            OutputText("Scenes.Masturbation.TitForeplay.LoudMoansOfPleasure".Localized());
                        else
                            OutputText("Scenes.Masturbation.TitForeplay.SquealsOfPleasure".Localized());

                    if (Player.TotalNipples() > 2)
                        OutputText("Scenes.Masturbation.TitForeplay.ManyWordAgain".Localized());

                    if (Player.LustPercent >= 50)
                    {
                        if (Player.BiggestLactation() > 2)
                            OutputText("Scenes.Masturbation.TitForeplay.HugeSwollenNipples".Localized());
                        else
                            if (Player.BiggestLactation() > 1)
                                OutputText("Scenes.Masturbation.TitForeplay.FatPuckeredNipples".Localized());
                            else
                                OutputText("Scenes.Masturbation.TitForeplay.ErectNipples".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.TitForeplay.NipplesWord".Localized());

                    if (Player.BiggestLactation() > 1 && Player.BiggestLactation() < 2)
                        OutputText("Scenes.Masturbation.TitForeplay.MilkDropletsDribble".Localized());
                    else
                        if (Player.BiggestLactation() >= 2 && Player.BiggestLactation() < 3)
                            OutputText("Scenes.Masturbation.TitForeplay.MilkSquirtsSpray".Localized());
                        else
                            if (Player.BiggestLactation() >= 3)
                                OutputText("Scenes.Masturbation.TitForeplay.MilkStreamDrizzles".Localized());
                }
        }

        public void TitFuckSingle()
        {

            if (Player.LibidoPercent < 45)
                OutputText("Scenes.Masturbation.TitFuckSingle.GripMemberGuideTowards".Localized(
                    Player.CockDescript()
                ));
            else
                if (Player.LibidoPercent < 70)
                {
                    OutputText("Scenes.Masturbation.TitFuckSingle.AnticipationPlaceFragment".Localized());

                    var cockId = Player.Cocks[0].Id;

                    if (cockId == BodyPartTypes.Anemone || cockId == BodyPartTypes.Displacer)
                        OutputText("Scenes.Masturbation.TitFuckSingle.WrigglingWord".Localized());
                    else if (cockId == BodyPartTypes.Demon)
                        OutputText("Scenes.Masturbation.TitFuckSingle.BumpEncircledWord".Localized());
                    else if (cockId == BodyPartTypes.Dog || cockId == BodyPartTypes.Fox || cockId == BodyPartTypes.Wolf)
                        OutputText("Scenes.Masturbation.TitFuckSingle.PointedWord".Localized());
                    else if (cockId == BodyPartTypes.Horse)
                        OutputText("Scenes.Masturbation.TitFuckSingle.FlaredWord".Localized());
                    else if (cockId == new Identifier("coc", "tentacle"))
                        OutputText("Scenes.Masturbation.TitFuckSingle.BulbousWord".Localized());

                    OutputText("Scenes.Masturbation.TitFuckSingle.TipAgainstOpening".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                {
                    OutputText("Scenes.Masturbation.TitFuckSingle.ThrustShaftStraightIn".Localized());

                    var __sw1_2 = Player.Cocks[0].Id;

                    if (__sw1_2 == BodyPartTypes.Anemone)
                        OutputText("Scenes.Masturbation.TitFuckSingle.TentacleLadenMushroom".Localized());

                    if (__sw1_2 == BodyPartTypes.Demon)
                        OutputText("Scenes.Masturbation.TitFuckSingle.BumpRingedMonstrosity".Localized());

                    if (__sw1_2 == BodyPartTypes.Displacer)
                        OutputText("Scenes.Masturbation.TitFuckSingle.OpenedTopFragment".Localized());

                    if (__sw1_2 == BodyPartTypes.Dog || __sw1_2 == BodyPartTypes.Fox || __sw1_2 == BodyPartTypes.Wolf)
                        OutputText("Scenes.Masturbation.TitFuckSingle.PointedTipFragment".Localized());

                    if (__sw1_2 == BodyPartTypes.Horse)
                        OutputText("Scenes.Masturbation.TitFuckSingle.EngorgedFlareFragment".Localized());

                    if (__sw1_2 == BodyPartTypes.Human)
                        OutputText("Scenes.Masturbation.TitFuckSingle.TipOfFragment".Localized());

                    if (__sw1_2 == new Identifier("coc", "tentacle"))
                        OutputText("Scenes.Masturbation.TitFuckSingle.OversizedMushroom".Localized());

                    if (true )
                        OutputText("Scenes.Masturbation.TitFuckSingle.OfWord".Localized(
                            Player.CockHead()
                        ));

                    OutputText("Scenes.Masturbation.TitFuckSingle.YourIntoFragment".Localized(
                        Player.CockDescript()
                    ));
                }

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Masturbation.TitFuckSingle.OneOfTheFragment".Localized());
            else
                OutputText("Scenes.Masturbation.TitFuckSingle.OneOfYourFragment".Localized());

            if (Player.AverageNipplesPerBreast() > 1)
                OutputText("Scenes.Masturbation.TitFuckSingle.ManyWord".Localized());

            if (Player.AverageVaginalLooseness() < 2)
                OutputText("Scenes.Masturbation.TitFuckSingle.PainfullyStretchedNipples".Localized());
            else
                if (Player.AverageVaginalLooseness() < 4)
                    OutputText("Scenes.Masturbation.TitFuckSingle.FreakishlySwollenNipples".Localized());
                else
                    OutputText("Scenes.Masturbation.TitFuckSingle.HugeBloatedCuntNipples".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Masturbation.TitFuckSingle.OnLowerBreastFragment".Localized());

            OutputText("Scenes.Masturbation.TitFuckSingle.PeriodSpace".Localized());

            if (Player.AverageLactation() == 0)
            {
                if (Player.AverageVaginalWetness() < 2)
                    OutputText("Scenes.Masturbation.TitFuckSingle.YLetter".Localized());
                else
                    if (Player.AverageVaginalWetness() < 4)
                    {
                        OutputText("Scenes.Masturbation.TitFuckSingle.SlickJuicesDribble".Localized());

                        var __sw2_3 = Player.Cocks[0].Id;

                        if (__sw2_3 == BodyPartTypes.Anemone)
                            OutputText("Scenes.Masturbation.TitFuckSingle.OverTransparentSkin".Localized());

                        if (__sw2_3 == BodyPartTypes.Cat)
                            OutputText("Scenes.Masturbation.TitFuckSingle.OverPinkSpinyProtrusions".Localized());

                        if (__sw2_3 == BodyPartTypes.Demon)
                            OutputText("Scenes.Masturbation.TitFuckSingle.ShinyPurplishSkinNodules".Localized());

                        if (__sw2_3 == BodyPartTypes.Displacer)
                            OutputText("Scenes.Masturbation.TitFuckSingle.OverDuskyPurple".Localized());

                        if (__sw2_3 == BodyPartTypes.Dog || __sw2_3 == BodyPartTypes.Fox || __sw2_3 == BodyPartTypes.Wolf)
                            OutputText("Scenes.Masturbation.TitFuckSingle.DownRedShinySkin".Localized());

                        if (__sw2_3 == BodyPartTypes.Horse)
                            OutputText("Scenes.Masturbation.TitFuckSingle.DownMottledSkin".Localized());

                        if (__sw2_3 == BodyPartTypes.Lizard)
                            OutputText("Scenes.Masturbation.TitFuckSingle.DownBumpyPurpleSkin".Localized());

                        if (__sw2_3 == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Masturbation.TitFuckSingle.DownRubberySkin".Localized());

                        if (true )
                            OutputText("Scenes.Masturbation.TitFuckSingle.DownTheSkin".Localized());

                        OutputText("Scenes.Masturbation.TitFuckSingle.YourAndYFragment".Localized(
                            Player.CockDescript()
                        ));
                    }
                    else
                        OutputText("Scenes.Masturbation.TitFuckSingle.JuicesSquirtFromAround".Localized(
                            Player.CockDescript()
                        ));
            }
            else
                if (Player.AverageLactation() < 2)
                {
                    OutputText("Scenes.Masturbation.TitFuckSingle.RivuletsMilkyDrizzle".Localized());

                    var __sw3_2 = Player.Cocks[0].Id;

                    if (__sw3_2 == BodyPartTypes.Anemone)
                        OutputText("Scenes.Masturbation.TitFuckSingle.DripFromTinyTentacles".Localized());

                    if (__sw3_2 == BodyPartTypes.Cat)
                        OutputText("Scenes.Masturbation.TitFuckSingle.DownSpinyPinkFlesh".Localized());

                    if (__sw3_2 == BodyPartTypes.Demon)
                        OutputText("Scenes.Masturbation.TitFuckSingle.DownPurplishNodules".Localized());

                    if (__sw3_2 == BodyPartTypes.Displacer)
                        OutputText("Scenes.Masturbation.TitFuckSingle.OverPurplishKnottedFlesh".Localized());

                    if (__sw3_2 == BodyPartTypes.Dog || __sw3_2 == BodyPartTypes.Fox || __sw3_2 == BodyPartTypes.Wolf)
                        OutputText("Scenes.Masturbation.TitFuckSingle.DownRedGlossySkin".Localized());

                    if (__sw3_2 == BodyPartTypes.Horse)
                        OutputText("Scenes.Masturbation.TitFuckSingle.DownMottledSkinAgain".Localized());

                    if (__sw3_2 == BodyPartTypes.Human)
                        OutputText("Scenes.Masturbation.TitFuckSingle.DownTheSkinAgain".Localized());

                    if (__sw3_2 == BodyPartTypes.Lizard)
                        OutputText("Scenes.Masturbation.TitFuckSingle.OverPurplishBumpyFlesh".Localized());

                    if (__sw3_2 == new Identifier("coc", "tentacle"))
                        OutputText("Scenes.Masturbation.TitFuckSingle.DownRubberySkinAgain".Localized());

                    if (true )
                        OutputText("Scenes.Masturbation.TitFuckSingle.OverSensitiveSkin".Localized());

                    OutputText("Scenes.Masturbation.TitFuckSingle.YourAndYPrefix".Localized(
                        Player.CockDescript()
                    ));
                }
                else
                {
                    OutputText("Scenes.Masturbation.TitFuckSingle.GirlcumSquirtStaining".Localized(
                        Player.CockDescript()
                    ));

                    var __sw4_2 = Player.Cocks[0].Id;

                    if (__sw4_2 == BodyPartTypes.Anemone)
                        OutputText("Scenes.Masturbation.TitFuckSingle.OddAquaticShaftWhite".Localized());

                    if (__sw4_2 == BodyPartTypes.Cat)
                        OutputText("Scenes.Masturbation.TitFuckSingle.PinkKittySkinWhite".Localized());

                    if (__sw4_2 == BodyPartTypes.Demon)
                        OutputText("Scenes.Masturbation.TitFuckSingle.PurplishSkinWhite".Localized());

                    if (__sw4_2 == BodyPartTypes.Displacer)
                        OutputText("Scenes.Masturbation.TitFuckSingle.PurpleKnottyFleshWhite".Localized());

                    if (__sw4_2 == BodyPartTypes.Dog || __sw4_2 == BodyPartTypes.Fox || __sw4_2 == BodyPartTypes.Wolf)
                        OutputText("Scenes.Masturbation.TitFuckSingle.ShinySkinWhite".Localized());

                    if (__sw4_2 == BodyPartTypes.Horse)
                        OutputText("Scenes.Masturbation.TitFuckSingle.MottledSkinWhite".Localized());

                    if (__sw4_2 == BodyPartTypes.Human)
                        OutputText("Scenes.Masturbation.TitFuckSingle.ItWhiteFragment".Localized());

                    if (__sw4_2 == BodyPartTypes.Lizard)
                        OutputText("Scenes.Masturbation.TitFuckSingle.PurpleBumpyFleshWhite".Localized());

                    if (__sw4_2 == new Identifier("coc", "tentacle"))
                        OutputText("Scenes.Masturbation.TitFuckSingle.RubberySkinWhite".Localized());

                    if (true )
                        OutputText("Scenes.Masturbation.TitFuckSingle.LengthWhite".Localized());
                }

            if ((float)(Player.CockArea(0)) >= Player.VaginalCapacity())
            {
                if (Player.SensitivityPercent < 45)
                    OutputText("Scenes.Masturbation.TitFuckSingle.GruntWithExertion".Localized(
                        Player.CockDescript()
                    ));
                else
                    if (Player.SensitivityPercent < 70)
                        OutputText("Scenes.Masturbation.TitFuckSingle.BlinkBackTears".Localized(
                            Player.CockDescript()
                        ));
                    else
                        OutputText("Scenes.Masturbation.TitFuckSingle.ScreamPainPleasure".Localized(
                            Player.CockDescript()
                        ));

                if (Player.AverageVaginalLooseness() < 2)
                    OutputText("Scenes.Masturbation.TitFuckSingle.SmallOverstretchedOpening".Localized());
                else
                    if (Player.AverageVaginalLooseness() < 4)
                        OutputText("Scenes.Masturbation.TitFuckSingle.EngorgedDistendedOpening".Localized());
                    else
                        OutputText("Scenes.Masturbation.TitFuckSingle.GapingFuckMouth".Localized());

                if ((float)(Player.CockArea(0)) == Player.VaginalCapacity())
                {
                    if (Player.AverageVaginalLooseness() < 2)
                        OutputText("Scenes.Masturbation.TitFuckSingle.PoorTorturedNippleBarely".Localized(
                            Player.CockDescript()
                        ));
                    else
                        if (Player.AverageVaginalLooseness() < 4)
                            OutputText("Scenes.Masturbation.TitFuckSingle.EngorgedOpeningAtEnd".Localized(
                                Player.CockDescript()
                            ));
                        else
                        {
                            var __sw5_4 = Player.Cocks[0].Id;

                            if (__sw5_4 == BodyPartTypes.Anemone)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenTipsWrapTentacles".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == BodyPartTypes.Cat)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenTipsStretchBarbed".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == BodyPartTypes.Demon)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsNoduleRinged".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == BodyPartTypes.Displacer)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenTipsOutstretchedHead".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == BodyPartTypes.Dog || __sw5_4 == BodyPartTypes.Fox || __sw5_4 == BodyPartTypes.Wolf)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsPointedTip".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == BodyPartTypes.Horse)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsFlaredTip".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == BodyPartTypes.Human)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsPlainTip".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == BodyPartTypes.Lizard)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenTipsPointedTwist".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw5_4 == new Identifier("coc", "tentacle"))
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsRoundedTip".Localized(
                                    Player.CockDescript()
                                ));

                            if (true )
                                OutputText("Scenes.Masturbation.TitFuckSingle.TipsWrapEnormousMouth".Localized(
                                    Player.CockHead(),

                                    Player.CockDescript()
                                ));
                        }

                    OutputText("Scenes.Masturbation.TitFuckSingle.EachThrustBuryDeeper".Localized(
                        Player.CockDescript()
                    ));
                }

                if ((float)(Player.CockArea(0)) > Player.VaginalCapacity())
                {
                    if (Player.AverageVaginalLooseness() < 2)
                        OutputText("Scenes.Masturbation.TitFuckSingle.NippleWoefullyInsufficient".Localized(
                            Player.CockDescript()
                        ));
                    else
                        if (Player.AverageVaginalLooseness() < 4)
                            OutputText("Scenes.Masturbation.TitFuckSingle.EngorgedOpeningBeyondLimit".Localized(
                                Player.CockDescript()
                            ));
                        else
                        {
                            var __sw6_4 = Player.Cocks[0].Id;

                            if (__sw6_4 == BodyPartTypes.Anemone)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsStingingTip".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == BodyPartTypes.Cat)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsBarbedTip".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == BodyPartTypes.Demon)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsTipWidens".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == BodyPartTypes.Displacer)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsWideHead".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == BodyPartTypes.Dog || __sw6_4 == BodyPartTypes.Fox || __sw6_4 == BodyPartTypes.Wolf)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsMassiveGirth".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == BodyPartTypes.Horse)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsFlaredSpreads".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == BodyPartTypes.Human)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsWidenAgain".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == BodyPartTypes.Lizard)
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsPointedSlow".Localized(
                                    Player.CockDescript()
                                ));

                            if (__sw6_4 == new Identifier("coc", "tentacle"))
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsWidenThird".Localized(
                                    Player.CockDescript()
                                ));

                            if (true )
                                OutputText("Scenes.Masturbation.TitFuckSingle.SwollenLipsGenericSpreads".Localized(
                                    Player.CockHead(),

                                    Player.CockDescript()
                                ));
                        }

                    OutputText("Scenes.Masturbation.TitFuckSingle.GruntingSweatingStuff".Localized(
                        Player.CockDescript(),

                        Player.CockDescript()
                    ));
                }

                if (Player.AverageLactation() > 0)
                    OutputText("Scenes.Masturbation.TitFuckSingle.MilkyWord".Localized());
                else
                    OutputText("Scenes.Masturbation.TitFuckSingle.SlickWord".Localized());

                if (Player.AverageVaginalWetness() < 2)
                    OutputText("Scenes.Masturbation.TitFuckSingle.LubeCoatsWholeLength".Localized(
                        Player.CockDescript()
                    ));
                else
                    if (Player.AverageVaginalWetness() < 4)
                    {
                        OutputText("Scenes.Masturbation.TitFuckSingle.LubeDrizzlesDownLength".Localized(
                            Player.CockDescript()
                        ));

                        if (Player.Cocks[0].HasKnot)
                            OutputText("Scenes.Masturbation.TitFuckSingle.PoolingAroundKnot".Localized());
                        else
                        {
                            var __sw7_4 = Player.Cocks[0].Id;

                            if (__sw7_4 == BodyPartTypes.Anemone)
                                OutputText("Scenes.Masturbation.TitFuckSingle.PoolingWrigglingTentacles".Localized());

                            if (__sw7_4 == BodyPartTypes.Cat)
                                OutputText("Scenes.Masturbation.TitFuckSingle.PoolingAroundSpines".Localized());

                            if (__sw7_4 == BodyPartTypes.Demon)
                                OutputText("Scenes.Masturbation.TitFuckSingle.PoolingAroundMemberBase".Localized());

                            if (__sw7_4 == BodyPartTypes.Displacer || __sw7_4 == BodyPartTypes.Dog || __sw7_4 == BodyPartTypes.Fox || __sw7_4 == BodyPartTypes.Wolf)
                                OutputText("Scenes.Masturbation.TitFuckSingle.PoolingAroundBulb".Localized());

                            if (__sw7_4 == BodyPartTypes.Horse)
                                OutputText("Scenes.Masturbation.TitFuckSingle.PoolingInsideSheath".Localized());

                            if (__sw7_4 == BodyPartTypes.Lizard)
                                OutputText("Scenes.Masturbation.TitFuckSingle.PoolingAroundBulbs".Localized());

                            if (__sw7_4 == new Identifier("coc", "tentacle"))
                                OutputText("Scenes.Masturbation.TitFuckSingle.MixingTentacleLubricants".Localized());

                            if (true )
                                OutputText("Scenes.Masturbation.TitFuckSingle.PoolingAtCrotch".Localized());
                        }
                    }
                    else
                        OutputText("Scenes.Masturbation.TitFuckSingle.LubeSpurtsFromNipple".Localized(
                            Player.CockDescript()
                        ));
            }
            else
            {
                if (Player.SensitivityPercent < 45)
                    OutputText("Scenes.Masturbation.TitFuckSingle.SighWithPleasure".Localized());
                else
                    if (Player.SensitivityPercent < 70)
                        OutputText("Scenes.Masturbation.TitFuckSingle.MoanWithPleasure".Localized());
                    else
                        OutputText("Scenes.Masturbation.TitFuckSingle.ScreamWithDelight".Localized());

                OutputText("Scenes.Masturbation.TitFuckSingle.SlidesIntoNipple".Localized(
                    Player.CockDescript()
                ));

                if (Player.AverageVaginalLooseness() < 2)
                    OutputText("Scenes.Masturbation.TitFuckSingle.SmallOpeningPenetration".Localized(
                        Player.CockDescript()
                    ));
                else
                    if (Player.AverageVaginalLooseness() < 4)
                        OutputText("Scenes.Masturbation.TitFuckSingle.EngorgedOpeningDeepPlunge".Localized(
                            Player.CockDescript()
                        ));
                    else
                    {
                        OutputText("Scenes.Masturbation.TitFuckSingle.GapingMouthEngulfsPrefix".Localized());

                        if (Player.Cocks[0].Id == BodyPartTypes.Horse)
                            OutputText("Scenes.Masturbation.TitFuckSingle.FlaredWordAgain".Localized());

                        OutputText("Scenes.Masturbation.TitFuckSingle.TipBeginsSliding".Localized(
                            Player.CockDescript()
                        ));

                        var __sw8_3 = Player.Cocks[0].Id;

                        if (__sw8_3 == BodyPartTypes.Anemone)
                            OutputText("Scenes.Masturbation.TitFuckSingle.SwallowsLeavingDesire".Localized());

                        if (__sw8_3 == BodyPartTypes.Cat)
                            OutputText("Scenes.Masturbation.TitFuckSingle.SwallowsSpringyBarb".Localized());

                        if (__sw8_3 == BodyPartTypes.Demon)
                            OutputText("Scenes.Masturbation.TitFuckSingle.SwallowsBumpsNodules".Localized());

                        if (__sw8_3 == BodyPartTypes.Displacer || __sw8_3 == BodyPartTypes.Dog || __sw8_3 == BodyPartTypes.Fox || __sw8_3 == BodyPartTypes.Wolf)
                            OutputText("Scenes.Masturbation.TitFuckSingle.SwallowsKnotEasily".Localized());

                        if (__sw8_3 == BodyPartTypes.Lizard)
                            OutputText("Scenes.Masturbation.TitFuckSingle.SwallowsBulgyKnots".Localized());

                        if (__sw8_3 == new Identifier("coc", "tentacle"))
                            OutputText("Scenes.Masturbation.TitFuckSingle.SwallowsTwistingShaft".Localized());

                        if (true )
                            OutputText("Scenes.Masturbation.TitFuckSingle.SwallowsCompletely".Localized());
                    }

                OutputText("Scenes.Masturbation.TitFuckSingle.RevelStrokeInOut".Localized(
                    Player.CockDescript()
                ));

                if (Player.AverageLactation() > 0)
                    OutputText("Scenes.Masturbation.TitFuckSingle.MilkyWordAgain".Localized());
                else
                    OutputText("Scenes.Masturbation.TitFuckSingle.SlipperyWord".Localized());

                if (Player.AverageVaginalWetness() < 2)
                    OutputText("Scenes.Masturbation.TitFuckSingle.LubeCoatsLengthAgain".Localized(
                        Player.CockDescript()
                    ));
                else
                    if (Player.AverageVaginalWetness() < 4)
                        OutputText("Scenes.Masturbation.TitFuckSingle.LubeDrizzlesAtCrotch".Localized(
                            Player.CockDescript()
                        ));
                    else
                        if (Player.CountCocksOfType("Human") > 0)
                            OutputText("Scenes.Masturbation.TitFuckSingle.LubePoursOverHands".Localized(
                                Player.CockDescript()
                            ));
                        else
                            if (Player.Cocks.Exists(c => c.SheathOn))
                                OutputText("Scenes.Masturbation.TitFuckSingle.LubeDrizzlesIntoSheath".Localized(
                                    Player.CockDescript()
                                ));
                            else
                                if (Player.CountCocksOfType("TENTACLE") > 0)
                                    OutputText("Scenes.Masturbation.TitFuckSingle.LubePoursHandsAgain".Localized(
                                        Player.CockDescript()
                                    ));
                                else
                                    if (Player.CountCocksOfType("DEMON") > 0)
                                        OutputText("Scenes.Masturbation.TitFuckSingle.LubePoursHandsThird".Localized(
                                            Player.CockDescript()
                                        ));
                                    else
                                        OutputText("Scenes.Masturbation.TitFuckSingle.LubePoursHandsFourth".Localized(
                                            Player.CockDescript()
                                        ));
            }
        }

        public bool MultiTitFuck()
        {

            var holeTotal = (int)(Player.TotalNipples());

            var fittableCocks = Player.Cocks.Count;

            var doubleUp = false;

            var randomCock = Random.Shared.Next(fittableCocks);

            MultiNippleFuckPrep(randomCock);

            if (holeTotal >= fittableCocks)
            {
                if (fittableCocks == 2)
                    OutputText("Scenes.Masturbation.MultiTitFuck.MoveOtherCock".Localized());
                else
                    if (fittableCocks == 2 || holeTotal == 2)
                        OutputText("Scenes.Masturbation.MultiTitFuck.MoveAnotherCock".Localized());
                    else
                        if (holeTotal >= fittableCocks)
                            OutputText("Scenes.Masturbation.MultiTitFuck.MoveRemainingShafts".Localized());

                if (holeTotal == 2)
                    OutputText("Scenes.Masturbation.MultiTitFuck.OtherNipple".Localized());
                else
                    if (fittableCocks <= 2)
                        OutputText("Scenes.Masturbation.MultiTitFuck.OneOfOtherNipples".Localized());
            }

            if (fittableCocks > 2 && holeTotal < fittableCocks)
            {
                OutputText("Scenes.Masturbation.MultiTitFuck.StumpedByNippleLimit".Localized());

                if (Player.VaginalCapacity() >= 2 * Player.CockArea(randomCock))
                {
                    doubleUp = true;

                    OutputText("Scenes.Masturbation.MultiTitFuck.MultipleInsertionsIdea".Localized());

                    if (2 * holeTotal >= fittableCocks)
                        OutputText("Scenes.Masturbation.MultiTitFuck.GleeAtNippleCapacity".Localized());
                    else
                        OutputText("Scenes.Masturbation.MultiTitFuck.TwoCocksEachHole".Localized(
                            NumberUtils.Num2Text(Player.Cocks.Count)
                        ));
                }
                else
                    OutputText("Scenes.Masturbation.MultiTitFuck.AcceptsAndPushes".Localized());
            }

            if (Player.AverageLactation() == 0)
            {
                if (Player.AverageVaginalWetness() < 2)
                    OutputText("Scenes.Masturbation.MultiTitFuck.YLetter".Localized());
                else
                    if (Player.AverageVaginalWetness() < 4)
                        OutputText("Scenes.Masturbation.MultiTitFuck.SlickJuicesDribble".Localized());
                    else
                        OutputText("Scenes.Masturbation.MultiTitFuck.SlickJuicesSquirt".Localized());
            }
            else
                if (Player.AverageLactation() < 2)
                    OutputText("Scenes.Masturbation.MultiTitFuck.RivuletsOfGirlcum".Localized());
                else
                    OutputText("Scenes.Masturbation.MultiTitFuck.MilkyGirlcumSquirts".Localized());

            if ((float)(Player.CockArea(randomCock)) >= Player.VaginalCapacity())
            {
                if (Player.SensitivityPercent < 45)
                    OutputText("Scenes.Masturbation.MultiTitFuck.GruntWithExertion".Localized());
                else
                    if (Player.SensitivityPercent < 70)
                        OutputText("Scenes.Masturbation.MultiTitFuck.BlinkBackTears".Localized());
                    else
                        OutputText("Scenes.Masturbation.MultiTitFuck.ScreamPainPleasure".Localized());

                if (Player.AverageVaginalLooseness() < 2)
                    OutputText("Scenes.Masturbation.MultiTitFuck.SmallOverstretchedOpenings".Localized());
                else
                    if (Player.AverageVaginalLooseness() < 4)
                        OutputText("Scenes.Masturbation.MultiTitFuck.EngorgedDistendedOpenings".Localized());
                    else
                        OutputText("Scenes.Masturbation.MultiTitFuck.GapingFuckMouths".Localized());

                if ((float)(Player.CockArea(randomCock)) == Player.VaginalCapacity())
                {
                    if (Player.AverageVaginalLooseness() < 2)
                        OutputText("Scenes.Masturbation.MultiTitFuck.TorturedNipplesBarelyCope".Localized());
                    else
                        if (Player.AverageVaginalLooseness() < 4)
                            OutputText("Scenes.Masturbation.MultiTitFuck.PuckeredOpeningsStretched".Localized());
                        else
                            OutputText("Scenes.Masturbation.MultiTitFuck.SwollenLipsStretch".Localized());

                    OutputText("Scenes.Masturbation.MultiTitFuck.BuryShaftsDeeper".Localized());
                }

                if ((float)(Player.CockArea(randomCock)) > Player.VaginalCapacity())
                {
                    if (Player.AverageVaginalLooseness() < 2)
                        OutputText("Scenes.Masturbation.MultiTitFuck.WoefullyInsufficient".Localized());
                    else
                        if (Player.AverageVaginalLooseness() < 4)
                            OutputText("Scenes.Masturbation.MultiTitFuck.PuckeredOpeningsBeyondLimits".Localized());
                        else
                            OutputText("Scenes.Masturbation.MultiTitFuck.SwollenLipsGapeWide".Localized());

                    OutputText("Scenes.Masturbation.MultiTitFuck.GruntingStuffingEffort".Localized());
                }

                if (Player.AverageLactation() > 0)
                    OutputText("Scenes.Masturbation.MultiTitFuck.MilkyWord".Localized());
                else
                    OutputText("Scenes.Masturbation.MultiTitFuck.SlickWord".Localized());

                if (Player.AverageVaginalWetness() < 2)
                    OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubeDrizzles".Localized());
                else
                    if (Player.AverageVaginalWetness() < 4)
                        OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubeDroolsTortured".Localized());
                    else
                        OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubeSpurts".Localized());
            }
            else
                if (!doubleUp)
                {
                    if (Player.SensitivityPercent < 45)
                        OutputText("Scenes.Masturbation.MultiTitFuck.SighWithPleasure".Localized());
                    else
                        if (Player.SensitivityPercent < 70)
                            OutputText("Scenes.Masturbation.MultiTitFuck.MoanWithPleasure".Localized());
                        else
                            OutputText("Scenes.Masturbation.MultiTitFuck.ScreamWithDelight".Localized());

                    if (Player.AverageVaginalLooseness() < 2)
                        OutputText("Scenes.Masturbation.MultiTitFuck.HardRodsPenetrateEasily".Localized());
                    else
                        if (Player.AverageVaginalLooseness() < 4)
                            OutputText("Scenes.Masturbation.MultiTitFuck.HardRodsPlungeDeeply".Localized());
                        else
                            OutputText("Scenes.Masturbation.MultiTitFuck.LipsEngulfHardRods".Localized());

                    OutputText("Scenes.Masturbation.MultiTitFuck.RevelInSlowStroke".Localized());

                    if (Player.AverageLactation() > 0)
                        OutputText("Scenes.Masturbation.MultiTitFuck.MilkyLowerWord".Localized());
                    else
                        OutputText("Scenes.Masturbation.MultiTitFuck.SlipperyWord".Localized());

                    if (Player.AverageVaginalWetness() < 2)
                        OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubeDrizzlesAgain".Localized());
                    else
                        if (Player.AverageVaginalWetness() < 4)
                            OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubeDroolsFat".Localized());
                        else
                            OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubePoursDeluge".Localized());
                }
                else
                {
                    if (Player.SensitivityPercent < 45)
                        OutputText("Scenes.Masturbation.MultiTitFuck.SighWithPleasureAgain".Localized());
                    else
                        if (Player.SensitivityPercent < 70)
                            OutputText("Scenes.Masturbation.MultiTitFuck.MoanWithPleasureAgain".Localized());
                        else
                            OutputText("Scenes.Masturbation.MultiTitFuck.ScreamWithDelightAgain".Localized());

                    if (4 * (float)(Player.AverageCockThickness()) < Player.AverageVaginalLooseness())
                    {
                        if (Player.AverageVaginalLooseness() < 2)
                            OutputText("Scenes.Masturbation.MultiTitFuck.SmallDelicateOpenings".Localized());
                        else
                            if (Player.AverageVaginalLooseness() < 4)
                                OutputText("Scenes.Masturbation.MultiTitFuck.StretchEasilyEngulfTwo".Localized());
                            else
                                OutputText("Scenes.Masturbation.MultiTitFuck.LipsSquishOpen".Localized());

                        OutputText("Scenes.Masturbation.MultiTitFuck.CocksRubbingTogether".Localized());
                    }
                    else
                    {
                        if (Player.AverageVaginalLooseness() < 2)
                            OutputText("Scenes.Masturbation.MultiTitFuck.OverstretchedPainfully".Localized());
                        else
                            if (Player.AverageVaginalLooseness() < 4)
                                OutputText("Scenes.Masturbation.MultiTitFuck.EngorgedStretchedToLimits".Localized());
                            else
                                OutputText("Scenes.Masturbation.MultiTitFuck.LipsStretchedToLimit".Localized());

                        OutputText("Scenes.Masturbation.MultiTitFuck.StrainingStuffingEffort".Localized());
                    }

                    if (Player.AverageLactation() > 0)
                        OutputText("Scenes.Masturbation.MultiTitFuck.MilkyWordAgain".Localized());
                    else
                        OutputText("Scenes.Masturbation.MultiTitFuck.SlickWordAgain".Localized());

                    if (Player.AverageVaginalWetness() < 2)
                        OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubeDrizzlesThird".Localized());
                    else
                        if (Player.AverageVaginalWetness() < 4)
                            OutputText("Scenes.Masturbation.MultiTitFuck.TorturedNipplesDroolAgain".Localized());
                        else
                            OutputText("Scenes.Masturbation.MultiTitFuck.GirlLubeSpurtsAgain".Localized());
                }

            if ((!doubleUp && fittableCocks <= holeTotal) || (doubleUp && fittableCocks <= holeTotal * 2))
                return false;

            return true;
        }

        public void MultiNippleFuckPrep(int randomCock)
        {

            if (Player.LibidoPercent < 70)
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.PlaceWithAnticipation".Localized());
            else
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.ShoveWithoutHesitation".Localized());

            if (Player.Cocks[randomCock].Id == BodyPartTypes.Human)
            {
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.TipOfPrefix".Localized());

                if (Player.CountCocksOfType("Human") > 1)
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.OneOfYourPrefix".Localized(
                        NumberUtils.Num2Text(Player.CountCocksOfType("Human")),

                        Player.CockDescript(randomCock)
                    ));
                else
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.YourPossessivePrefix".Localized(
                        Player.CockDescript(randomCock)
                    ));
            }

            if (Player.Cocks[randomCock].Id == BodyPartTypes.Horse)
            {
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.FlaredTipPrefix".Localized());

                if (Player.CountCocksOfType("Horse") > 1)
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.OneOfYourAgain".Localized(
                        NumberUtils.Num2Text(Player.CountCocksOfType("Horse")),

                        Player.CockDescript(randomCock)
                    ));
                else
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.YourPossessiveAgain".Localized(
                        Player.CockDescript(randomCock)
                    ));
            }

            if (Player.Cocks[randomCock].HasKnot)
            {
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.PointedTipPrefix".Localized());

                if (Player.CountKnottedCocks() > 1)
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.OneOfYourThird".Localized(
                        NumberUtils.Num2Text(Player.CountKnottedCocks()),

                        Player.CockDescript(randomCock)
                    ));
                else
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.YourPossessiveThird".Localized(
                        Player.CockDescript(randomCock)
                    ));
            }

            if (Player.Cocks[randomCock].TypeIndex >= 3)
            {
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.TipOfPrefixAgain".Localized());

                if (Player.CountCocksOfType("Human") > 1)
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.OneOfYourFourth".Localized(
                        NumberUtils.Num2Text(Player.CountCocksOfType("Human")),

                        Player.CockDescript(randomCock)
                    ));
                else
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.YourPossessiveFourth".Localized(
                        Player.CockDescript(randomCock)
                    ));
            }

            if (Player.LibidoPercent < 70)
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.AgainstOpeningOfPrefix".Localized());
            else
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.IntoPrefix".Localized());

            if (Player.BreastRows.Count > 1)
            {
                if (Player.AverageNipplesPerBreast() > 1)
                    OutputText("Scenes.Masturbation.TitFuckSingle.OneOfThePrefix".Localized());
                else
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.TheArticlePrefix".Localized());
            }
            else
                OutputText("Scenes.Masturbation.TitFuckSingle.OneOfYourPrefix".Localized());

            if (Player.AverageNipplesPerBreast() > 1)
                OutputText("Scenes.Masturbation.TitFuckSingle.ManyWordAgain".Localized());

            if (Player.AverageVaginalLooseness() < 2)
                OutputText("Scenes.Masturbation.MultiNippleFuckPrep.PainfullyStretchedNipple".Localized());
            else
                if (Player.AverageVaginalLooseness() < 4)
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.FreakishlySwollenNipple".Localized());
                else
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.HugeBloatedCuntNipple".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Masturbation.TitFuckSingle.LowerBreastFragmentAgain".Localized());
            else
                if (Player.AverageNipplesPerBreast() > 1)
                    OutputText("Scenes.Masturbation.MultiNippleFuckPrep.SLetter".Localized());

            OutputText("Scenes.Masturbation.TitFuckSingle.PeriodSpaceAgain".Localized());
        }

        public void Orgazmo(bool selfSucking, bool nippleFuck)
        {

            OutputText("Scenes.Masturbation.Orgazmo.ParagraphBreak".Localized());

            var cumType = 0;

            if (Player.Cocks.Count > 0)
            {
                cumType = 1;

                if (Player.Cocks.Count == 1)
                {
                    OutputText("Scenes.Masturbation.Orgazmo.TightnessInLoins".Localized());

                    if (Player.CountCocksOfType("Human") == 1)
                        OutputText("Scenes.Masturbation.Orgazmo.ManhoodNearRelease".Localized());
                    else
                        if (Player.HasKnot())
                            OutputText("Scenes.Masturbation.Orgazmo.KnotSwellingTwiceWidth".Localized(
                                Player.CockDescript()
                            ));
                        else
                            if (Player.CountCocksOfType("Horse") == 1)
                                OutputText("Scenes.Masturbation.Orgazmo.EquineSheathPreCum".Localized(
                                    Player.CockDescript(),

                                    Player.CockDescript()
                                ));
                            else
                                if (Player.CountCocksOfType("TENTACLE") == 1)
                                    OutputText("Scenes.Masturbation.Orgazmo.VineCockContracting".Localized(
                                        Player.CockDescript()
                                    ));
                                else
                                    if (Player.CountCocksOfType("DEMON") == 1)
                                        OutputText("Scenes.Masturbation.Orgazmo.NodulesPulsingRipple".Localized(
                                            Player.CockDescript()
                                        ));
                                    else
                                        if (Player.CountCocksOfType("Cat") == 1)
                                            OutputText("Scenes.Masturbation.Orgazmo.CatSheathSpinesPulse".Localized(
                                                Player.CockDescript()
                                            ));
                                        else
                                            if (Player.CountCocksOfType("LIZARD") == 1)
                                                OutputText("Scenes.Masturbation.Orgazmo.MemberContractingPreCum".Localized(
                                                    Player.CockDescript()
                                                ));
                                            else
                                                if (Player.CountCocksOfType("ANEMONE") == 1)
                                                    OutputText("Scenes.Masturbation.Orgazmo.TentaclesFireAphrodisiac".Localized(
                                                        Player.CockDescript()
                                                    ));
                                                else
                                                    OutputText("Scenes.Masturbation.Orgazmo.ManhoodNearReleaseAgain".Localized());

                    OutputText("Scenes.Masturbation.Orgazmo.SpasmsAsCumErupts".Localized(
                        Player.CockDescript()
                    ));

                    OutputText("Scenes.Masturbation.Orgazmo.HipsJerkingEruptions".Localized());

                    if (selfSucking)
                        OutputText("Scenes.Masturbation.Orgazmo.IntoEagerMouth".Localized());

                    if (nippleFuck)
                        OutputText("Scenes.Masturbation.Orgazmo.InsideUnnaturalBreast".Localized());
                    else
                        if (!selfSucking)
                            OutputText("Scenes.Masturbation.Orgazmo.InTheAir".Localized());

                    if (selfSucking)
                    {
                        if (Player.CumQ() < 25)
                            OutputText("Scenes.Masturbation.Orgazmo.GulpDownMostCum".Localized());
                        else
                            if (Player.CumQ() < 250)
                                OutputText("Scenes.Masturbation.Orgazmo.SputterJismOverflow".Localized());
                            else
                                if (Player.CumQ() < 500)
                                    OutputText("Scenes.Masturbation.Orgazmo.PulseIntoMouthOverflow".Localized(
                                        Player.CockDescript()
                                    ));
                                else
                                    OutputText("Scenes.Masturbation.Orgazmo.EndlessOrgasmJets".Localized(
                                        Player.CockDescript()
                                    ));

                        if (Player.CumQ() < 1000)
                            Player.RefillHunger(Player.CumQ() / 20);
                        else
                            if (Player.CumQ() < 3000)
                                Player.RefillHunger(50 + ((Player.CumQ() - 1000) / 40));
                            else
                                Player.RefillHunger(100);
                    }
                    else
                        if (nippleFuck)
                        {
                            if (Player.CumQ() < 25)
                                OutputText("Scenes.Masturbation.Orgazmo.SpurtsInsideBreast".Localized());
                            else
                                if (Player.CumQ() < 250)
                                    OutputText("Scenes.Masturbation.Orgazmo.CumDribblesBreastOverflow".Localized());
                                else
                                    if (Player.CumQ() < 500)
                                        OutputText("Scenes.Masturbation.Orgazmo.BreastSwellingWithCum".Localized());
                                    else
                                        OutputText("Scenes.Masturbation.Orgazmo.EndlessOrgasmJetsAgain".Localized(
                                            Player.CockDescript()
                                        ));
                        }
                        else
                            if (Player.CumQ() < 25)
                                OutputText("Scenes.Masturbation.Orgazmo.FewSpurtsSplatterYou".Localized());
                            else
                                if (Player.CumQ() < 250)
                                    OutputText("Scenes.Masturbation.Orgazmo.OrgasmDragsOnCoating".Localized());
                                else
                                    if (Player.CumQ() < 500)
                                        OutputText("Scenes.Masturbation.Orgazmo.BodySpasmsRopeAfter".Localized());
                                    else
                                        OutputText("Scenes.Masturbation.Orgazmo.EndlessOrgasmEveryJet".Localized());

                    if (Player.Armor.Id == ArmorIds.GooArmor)
                        OutputText("Scenes.Masturbation.Orgazmo.SeedAbsorbedByGoo".Localized());
                }

                if (Player.Cocks.Count > 1)
                {
                    OutputText("Scenes.Masturbation.Orgazmo.LoinsTighteningBeforeRelease".Localized());

                    if (Player.CountKnottedCocks() > Player.CountCocksOfType("Human") && Player.CountKnottedCocks() > Player.CountCocksOfType("Horse"))
                    {
                        int dogIndex = 0;

                        dogIndex = 0;

                        while (dogIndex < Player.Cocks.Count)
                        {
                            if (Player.Cocks[dogIndex].HasKnot)
                                break;

                            dogIndex += 1;
                        }

                        OutputText("Scenes.Masturbation.Orgazmo.KnotsBulgingDouble".Localized(
                            Player.CockDescript(dogIndex)
                        ));
                    }
                    else
                        if (Player.CountCocksOfType("Horse") > Player.CountCocksOfType("Human"))
                        {
                            int horseIndex = 0;

                            horseIndex = 0;

                            while (horseIndex < Player.Cocks.Count)
                            {
                                if (Player.Cocks[horseIndex].Id == BodyPartTypes.Horse)
                                    break;

                                horseIndex += 1;
                            }

                            OutputText("Scenes.Masturbation.Orgazmo.SheathPulsePreCum".Localized(
                                Player.CockDescript(horseIndex),

                                Player.CockDescript(horseIndex)
                            ));
                        }
                        else
                            OutputText("Scenes.Masturbation.Orgazmo.ManyManhoodsFullSize".Localized());

                    if (nippleFuck)
                        if (Player.CumQ() < 25)
                            OutputText("Scenes.Masturbation.Orgazmo.SpurtsInsideBreasts".Localized());
                        else
                            if (Player.CumQ() < 250)
                                OutputText("Scenes.Masturbation.Orgazmo.CumDribblesShaftsTits".Localized());
                            else
                                if (Player.CumQ() < 500)
                                    OutputText("Scenes.Masturbation.Orgazmo.BreastsSwellingTorrents".Localized());
                                else
                                    OutputText("Scenes.Masturbation.Orgazmo.ExplosiveReleaseEndless".Localized());

                    OutputText("Scenes.Masturbation.Orgazmo.PleasureOverwhelmsMind".Localized());

                    if (selfSucking)
                    {
                        if (Player.CumQ() < 25)
                            OutputText("Scenes.Masturbation.Orgazmo.SwallowMostCum".Localized());
                        else
                            if (Player.CumQ() < 250)
                                OutputText("Scenes.Masturbation.Orgazmo.SputterJismTooMuch".Localized());
                            else
                                if (Player.CumQ() < 500)
                                    OutputText("Scenes.Masturbation.Orgazmo.MouthFilledByPulses".Localized(
                                        Player.CockDescript(),

                                        Player.CockDescript()
                                    ));
                                else
                                    OutputText("Scenes.Masturbation.Orgazmo.EndlessOrgasmJetsThird".Localized(
                                        Player.CockDescript()
                                    ));

                        if (Player.CumQ() < 1000)
                            Player.RefillHunger(Player.CumQ() / 20);
                        else
                            if (Player.CumQ() < 3000)
                                Player.RefillHunger(50 + ((Player.CumQ() - 1000) / 40));
                            else
                                Player.RefillHunger(100);
                    }

                    if (Player.CumQ() < 25)
                        OutputText("Scenes.Masturbation.Orgazmo.FewSpurtsFromCocks".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Masturbation.Orgazmo.DragsOnEachCock".Localized());
                        else
                            if (Player.CumQ() < 500)
                                OutputText("Scenes.Masturbation.Orgazmo.BodySpasmsEachSpurt".Localized());
                            else
                                OutputText("Scenes.Masturbation.Orgazmo.EndlessOrgasmMultipleJets".Localized());
                }
            }

            if (Player.Vaginas.Count > 0)
            {
                cumType = 2;

                if (Player.Vaginas.Count == 1)
                {
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Masturbation.Orgazmo.FemWord".Localized());
                    else
                        OutputText("Scenes.Masturbation.Orgazmo.YourWord".Localized());

                    OutputText("Scenes.Masturbation.Orgazmo.SexQuiversMuscleLoss".Localized());

                    if (Player.GetClitLength() >= 4.5f)
                        OutputText("Scenes.Masturbation.Orgazmo.HandSqueezesCockClit".Localized());
                    else
                        if (Player.GetClitLength() > 1.5f)
                            OutputText("Scenes.Masturbation.Orgazmo.OversensitiveClittyStretches".Localized());

                    if (Player.Vaginas[0].Wetness == 5)
                        OutputText("Scenes.Masturbation.Orgazmo.GushExplodesNethers".Localized());
                }
            }

            if (Player.AverageLactation() >= 2 || Player.HasFuckableNipples())
            {
                cumType = 3;

                TitCum(Player.CumQ());

                TitDrink();
            }

            if (Player.Armor.Id == ArmorIds.GooArmor)
            {
                var valeriaFluids = 0;

                if ((Player.HasVagina()) && (Player.Vaginas[0].Wetness >= 3))
                        valeriaFluids += (int)(Player.Vaginas[0].Wetness * 3);

                if (Player.HasCock())
                    if (Player.CumQ() < 100)
                        valeriaFluids += (int)(Player.CumQ() / 5);
                    else
                        if (Player.CumQ() >= 100 && Player.CumQ() < 500)
                            valeriaFluids += (int)(20 + (Player.CumQ() / 20));
                        else
                            if (Player.CumQ() >= 500 && Player.CumQ() < 3500)
                                valeriaFluids += (int)(40 + (Player.CumQ() / 50));
                            else
                                if (Player.CumQ() >= 3500)
                                    valeriaFluids += 100;

                valeriaFluids += (int)(Math.Sqrt(Player.LactationQ()));

                if (valeriaFluids > 0)
                    OutputText("Scenes.Masturbation.Orgazmo.ValeriaThanksFluids".Localized());
            }

            if (cumType == 1)
                Player.Orgasm("Dick");
            else
                if (cumType == 2)
                    Player.Orgasm("Vaginal");
                else
                    if (cumType == 3)
                        Player.Orgasm("Tits");

            UpdateMasturbation();

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -0.5f));
        }

        public void TitCum(float cumQuantity = 3)
        {

            if (Player.AverageLactation() >= 2 && !Player.HasFuckableNipples())
            {
                OutputText("Scenes.Masturbation.TitCum.PressureBuildsInBreasts".Localized());

                if (Player.AverageLactation() <= 2.6f)
                    OutputText("Scenes.Masturbation.TitCum.ThinStreamsMilk".Localized());

                if (Player.AverageLactation() > 2.6f && Player.AverageLactation() < 3)
                    OutputText("Scenes.Masturbation.TitCum.ThickGoutsMilk".Localized());

                if (Player.AverageLactation() >= 3)
                    OutputText("Scenes.Masturbation.TitCum.HugeTorrentsMilk".Localized());

                if (Player.NippleLength <= 1)
                    OutputText("Scenes.Masturbation.TitCum.ErectNipples".Localized());

                if (Player.NippleLength > 1 && Player.NippleLength < 4)
                    OutputText("Scenes.Masturbation.TitCum.FatPuckeredNipples".Localized());

                if (Player.NippleLength >= 4)
                    OutputText("Scenes.Masturbation.TitCum.HugeSwollenTeats".Localized());

                if (Player.Armor.Id == ArmorIds.GooArmor)
                {
                    if (Player.AverageLactation() < 2.6f)
                        OutputText("Scenes.Masturbation.TitCum.MilkSpatteringGoo".Localized());
                    else
                        OutputText("Scenes.Masturbation.TitCum.StuffingGooMilk".Localized());
                }
                else
                {
                    if (Player.AverageLactation() <= 2.6f)
                        OutputText("Scenes.Masturbation.TitCum.MilkEverywhere".Localized());

                    if (Player.AverageLactation() > 2.6f && Player.AverageLactation() < 3)
                        OutputText("Scenes.Masturbation.TitCum.CoveringEverythingNearby".Localized());

                    if (Player.AverageLactation() >= 3)
                        OutputText("Scenes.Masturbation.TitCum.DrenchingEntireArea".Localized());
                }
            }

            if (Player.HasFuckableNipples())
            {
                OutputText("Scenes.Masturbation.TitCum.StrangeFeelingInBreasts".Localized());

                if (Player.SensitivityPercent < 70)
                    OutputText("Scenes.Masturbation.TitCum.MoanFreakishlySwollen".Localized());
                else
                    OutputText("Scenes.Masturbation.TitCum.ScreamFreakishlySwollen".Localized());

                if (Player.AverageVaginalWetness() < 2)
                    OutputText("Scenes.Masturbation.TitCum.DribbleWord".Localized());

                if (Player.AverageVaginalWetness() >= 2 && Player.AverageVaginalWetness() < 4)
                    OutputText("Scenes.Masturbation.TitCum.SprayWord".Localized());

                if (Player.AverageVaginalWetness() >= 4)
                    OutputText("Scenes.Masturbation.TitCum.GushWord".Localized());

                if (Player.AverageLactation() > 0)
                    OutputText("Scenes.Masturbation.TitCum.MilkAndWord".Localized());

                OutputText("Scenes.Masturbation.TitCum.PussyJuiceWord".Localized());

                if (Player.Armor.Id == ArmorIds.GooArmor)
                    OutputText("Scenes.Masturbation.TitCum.IntoBlueGoo".Localized());
                else
                    OutputText("Scenes.Masturbation.TitCum.EverywhereWord".Localized());
            }
        }

        public void TitDrink()
        {

            if (Player.BiggestBreastSize() > 5 && Player.AverageLactation() > 1)
            {
                ShowImage("masti-tit");

                if (Player.BiggestLactation() > 3)
                {
                    OutputText("Scenes.Masturbation.TitDrink.GulpGreedilyMilk".Localized());

                    Player.RefillHunger(50);

                    Player.BoostLactation(.05f);

                    Player.Milked();
                }
                else
                    if (Player.BiggestLactation() > 2)
                    {
                        OutputText("Scenes.Masturbation.TitDrink.DrinkDeeplyBounty".Localized());

                        Player.RefillHunger((Player.BiggestLactation() - 2) * 30 + 20);

                        Player.BoostLactation(.05f);

                        Player.Milked();
                    }
                    else
                        if (Player.BiggestLactation() > 1)
                        {
                            OutputText("Scenes.Masturbation.TitDrink.SuckBountyGulping".Localized());

                            Player.RefillHunger((Player.BiggestLactation() - 1) * 20);

                            Player.BoostLactation(.05f);

                            Player.Milked();
                        }
            }
        }

        public void DeluxeDildo()
        {
            ClearOutput();

            Player.SlimeFeed();

            if (Player.HasVagina())
            {
                ShowImage("masti-dildo-vag");

                if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
                {
                    OutputText("Scenes.Masturbation.DeluxeDildo.BlushGrabPinkDildo".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.SitRubPinkCock".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.SweetFluidWarmth".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.ClitBurnsSlickToy".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.ResistChastityUrge".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.HymenTornVirginity".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.PistonShaftDepths".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.HoldCockInPlace".Localized());

                    OutputText("Scenes.Masturbation.DeluxeDildo.DriftOffPinkSemen".Localized());

                    Player.Orgasm("Vaginal");

                    UpdateMasturbation();

                    OutputText("Scenes.Masturbation.DeluxeDildo.ParagraphBreak".Localized());

                    Player.CuntChange(Player.VaginalCapacity() * 0.9f, true);

                    ReturnToCamp(1);

                    return ;
                }

                if (Player.Corruption > 66)
                    OutputText("Scenes.Masturbation.DeluxeDildo.RetrieveDildoStripCamp".Localized());
                else
                    if (Player.Corruption > 33)
                        OutputText("Scenes.Masturbation.DeluxeDildo.SqueezeSpongyDildo".Localized());
                    else
                        OutputText("Scenes.Masturbation.DeluxeDildo.BlushGrabDildoAgain".Localized());

                OutputText("Scenes.Masturbation.DeluxeDildo.ParagraphBreakAgain".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Masturbation.DeluxeDildo.HoldDongDisgusted".Localized());
                else
                    OutputText("Scenes.Masturbation.DeluxeDildo.SqueezeDongGiggle".Localized());

                OutputText("Scenes.Masturbation.DeluxeDildo.AphrodisiacDropHint".Localized());

                if (Player.GetClitLength() < .5f)
                    OutputText("Scenes.Masturbation.DeluxeDildo.BecomesHardSensitive".Localized());
                else
                    if (Player.GetClitLength() < 3)
                        OutputText("Scenes.Masturbation.DeluxeDildo.PeeksThroughFolds".Localized());
                    else
                        if (Player.GetClitLength() < 6)
                            OutputText("Scenes.Masturbation.DeluxeDildo.BloodErectTinyCock".Localized());
                        else
                            OutputText("Scenes.Masturbation.DeluxeDildo.BloodTwitchingCock".Localized());

                OutputText("Scenes.Masturbation.DeluxeDildo.YouWord".Localized());

                if (Player.Corruption > 50)
                    OutputText("Scenes.Masturbation.DeluxeDildo.DontWord".Localized());

                OutputText("Scenes.Masturbation.DeluxeDildo.HesitateWord".Localized());

                if (Player.Corruption < 50)
                    OutputText("Scenes.Masturbation.DeluxeDildo.SlowInsertionGasp".Localized());
                else
                    OutputText("Scenes.Masturbation.DeluxeDildo.RammingDeepMoaning".Localized());

                OutputText("Scenes.Masturbation.DeluxeDildo.SplayLegsLieResponding".Localized());

                if (Player.Vaginas[0].Wetness < 3)
                    OutputText("Scenes.Masturbation.DeluxeDildo.ToyThicknessIncreases".Localized());
                else
                    if (Player.Vaginas[0].Wetness < 5)
                    {
                        OutputText("Scenes.Masturbation.DeluxeDildo.GirthPulseSwell".Localized());

                        if (Player.Cocks.Count > 0)
                        {
                            OutputText("Scenes.Masturbation.DeluxeDildo.EvenYourAche".Localized(
                                Player.MultiCockDescript()
                            ));

                            if (Player.Cocks.Count == 1)
                                OutputText("Scenes.Masturbation.DeluxeDildo.SLetter".Localized());

                            OutputText("Scenes.Masturbation.DeluxeDildo.AndPulseWord".Localized());

                            if (Player.Cocks.Count == 1)
                                OutputText("Scenes.Masturbation.DeluxeDildo.SLetterAgain".Localized());

                            OutputText("Scenes.Masturbation.DeluxeDildo.BouncingOnBelly".Localized());
                        }

                        OutputText("Scenes.Masturbation.DeluxeDildo.PistonFatDickHarder".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DeluxeDildo.DildoGrowingInside".Localized());

                if (Player.SensitivityPercent < 80)
                {
                    OutputText("Scenes.Masturbation.DeluxeDildo.BrutalizeCuntOrgasm".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));
                }
                else
                    OutputText("Scenes.Masturbation.DeluxeDildo.BrutalizeFuckHoleGetoff".Localized());

                if (SceneInstance<JojoScene>.Get().CampCorruptJojo())
                {
                    OutputText("Scenes.Masturbation.DeluxeDildo.StandCleanSpotJojo".Localized());

                    if (Player.HasStatusEffect(StatusEffectIds.TentacleJojo))
                        OutputText("Scenes.Masturbation.DeluxeDildo.TentaclesSplatterMouseJizz".Localized());
                    else
                        OutputText("Scenes.Masturbation.DeluxeDildo.SplatteringMouseSpunkFinishes".Localized());
                }

                Player.Orgasm("Vaginal");

                UpdateMasturbation();

                OutputText("Scenes.Masturbation.DeluxeDildo.ParagraphBreakThird".Localized());

                Player.CuntChange(Player.VaginalCapacity() * 0.9f, true);

                ReturnToCamp(1);
            }
        }

        public void DeluxeDildoAnal()
        {
            ClearOutput();

            Player.SlimeFeed();

            ShowImage("masti-dildo-anal");

            if (Player.Corruption > 66)
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.RetrievePinkDildo".Localized());
            else
                if (Player.Corruption > 33)
                    OutputText("Scenes.Masturbation.DeluxeDildoAnal.RetrieveFloppyDildo".Localized());
                else
                    OutputText("Scenes.Masturbation.DeluxeDildoAnal.BlushGrabPinkDildo".Localized());

            OutputText("Scenes.Masturbation.DeluxeDildoAnal.ParagraphBreak".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.HoldFakeDongAway".Localized());
            else
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.SqueezeFakeDong".Localized());

            OutputText("Scenes.Masturbation.DeluxeDildoAnal.AphrodisiacDropLeaks".Localized());

            OutputText("Scenes.Masturbation.DeluxeDildoAnal.PeriodYouPrefix".Localized());

            if (Player.Corruption > 50)
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.DontWord".Localized());

            OutputText("Scenes.Masturbation.DeluxeDildoAnal.HesitateWord".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.SlowlyWorkInside".Localized());
            else
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.RamDeepMoaning".Localized());

            OutputText("Scenes.Masturbation.DeluxeDildoAnal.SplayLegsLieThere".Localized());

            if (Player.Anal.Wetness < AnalPart.WetnessSlimy)
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.ToyThicknessGrows".Localized());
            else
                if (Player.Anal.Wetness < AnalPart.WetnessSlimeDrooling)
                {
                    OutputText("Scenes.Masturbation.DeluxeDildoAnal.ToyGirthPulses".Localized());

                    if (Player.Cocks.Count > 0)
                    {
                        OutputText("Scenes.Masturbation.DeluxeDildoAnal.EvenAchePrefix".Localized(
                            Player.MultiCockDescript()
                        ));

                        if (Player.Cocks.Count == 1)
                            OutputText("Scenes.Masturbation.DeluxeDildoAnal.PluralSLetter".Localized());

                        OutputText("Scenes.Masturbation.DeluxeDildoAnal.AndPulseSuffix".Localized());

                        if (Player.Cocks.Count == 1)
                            OutputText("Scenes.Masturbation.DeluxeDildoAnal.PluralSLetterSecond".Localized());

                        OutputText("Scenes.Masturbation.DeluxeDildoAnal.BouncingOnBelly".Localized());
                    }

                    OutputText("Scenes.Masturbation.DeluxeDildoAnal.PistonFatDickHarder".Localized());
                }
                else
                    OutputText("Scenes.Masturbation.DeluxeDildoAnal.FeelDildoGrowing".Localized());

            if (Player.SensitivityPercent < 80)
            {
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.BrutalizeWithSwollenPlug".Localized());

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f));
            }
            else
                OutputText("Scenes.Masturbation.DeluxeDildoAnal.BrutalizeBloatedToy".Localized());

            Player.Orgasm("Anal");

            UpdateMasturbation();

            OutputText("Scenes.Masturbation.DeluxeDildoAnal.ParagraphBreakAgain".Localized());

            Player.ButtChange(Player.AnalCapacity() * 0.9f, true);

            ReturnToCamp(1);
        }

        public void OnaholeUse()
        {
            ClearOutput();

            ShowImage("masti-Onahole");

            if (!Player.HasStatusEffect(StatusEffectIds.PlainOnaholeUsed))
            {
                Player.CreateStatusEffect(StatusEffectIds.PlainOnaholeUsed, 0, 0, 0, 0);

                OutputText("Scenes.Masturbation.OnaholeUse.GetNakedSettleDown".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Masturbation.OnaholeUse.SlapCocksAttention".Localized());
                else
                    OutputText("Scenes.Masturbation.OnaholeUse.SlapCockAttentionSingular".Localized());

                OutputText("Scenes.Masturbation.OnaholeUse.GiacomoFailedToWarn".Localized());

                OutputText("Scenes.Masturbation.OnaholeUse.BodyQuickRespond".Localized());
            }
            else
                if (Player.Corruption > 66)
                    OnaholeRepeatUse(true);
                else
                    OnaholeRepeatUse(false);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -.75f));

            OnaholeContinuation();
        }

        public void DeluxeOnaholeUse()
        {
            ClearOutput();

            ShowImage("masti-Onahole");

            if (!Player.HasStatusEffect(StatusEffectIds.DeluxeOnaholeUsed))
            {
                Player.CreateStatusEffect(StatusEffectIds.DeluxeOnaholeUsed, 0, 0, 0, 0);

                OutputText("Scenes.Masturbation.DeluxeOnaholeUse.GetNakedSettleDown".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Masturbation.DeluxeOnaholeUse.SlapCocksAttention".Localized());
                else
                    OutputText("Scenes.Masturbation.DeluxeOnaholeUse.SlapCockAttentionSingular".Localized());

                OutputText("Scenes.Masturbation.DeluxeOnaholeUse.DeflowerRealisticToy".Localized());

                OutputText("Scenes.Masturbation.DeluxeOnaholeUse.WorkCockRelentlessly".Localized());

                OutputText("Scenes.Masturbation.DeluxeOnaholeUse.BodyQuickRespond".Localized());
            }
            else
                if (Player.Corruption > 66)
                    OnaholeRepeatUse(true);
                else
                    OnaholeRepeatUse(false);

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1.5f));

            OnaholeContinuation();
        }

        public void OnaholeContinuation()
        {

            UpdateMasturbation();

            if (Player.Cocks.Count > 1)
            {
                if (Player.Gender == Gender.Hermaphrodite && Random.Shared.Next(2) == 0)
                    OnaholeFutaContinuation();
                else
                    OnaholeMulticockContinuation();
            }
            else
                if (Player.Gender == Gender.Hermaphrodite)
                    OnaholeFutaContinuation();
                else
                {
                    Player.Orgasm("Dick");

                    ReturnToCamp(1);
                }
        }

        public void OnaholeMulticockContinuation()
        {

            OutputText("Scenes.Masturbation.OnaholeMulticockContinuation.PullSloppyToyAway".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Masturbation.OnaholeMulticockContinuation.PassOutAchingBalls".Localized());
            else
                OutputText("Scenes.Masturbation.OnaholeMulticockContinuation.PassOutSoreCocks".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

        public void OnaholeFutaContinuation()
        {

            OutputText("Scenes.Masturbation.OnaholeFutaContinuation.GenderBlessingCurse".Localized());

            OutputText(((int)((((Random.Shared.NextDouble() * Player.Strength / 12) + Player.Strength / 6) * 10) / 10)).ToString());

            OutputText("Scenes.Masturbation.OnaholeFutaContinuation.SemenVolcanoDelirium".Localized());

            OutputText("Scenes.Masturbation.OnaholeFutaContinuation.OrgasmsFadePassOut".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

        public void AllNaturalOnaholeUse()
        {
            ClearOutput();

            ShowImage("masti-Onahole-allNatural");

            if (!Player.HasStatusEffect(StatusEffectIds.AllNaturalOnaholeUsed))
            {
                Player.CreateStatusEffect(StatusEffectIds.AllNaturalOnaholeUsed, 0, 0, 0, 0);

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.WonderAtGoofyToy".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.StrokeGentlyLeisurely".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.PressureClampsCock".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.ImpressionShiftsAdjusts".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.PlightSinksIn".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.TendrilExploresUrethra".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.CumBuildSharpSuction".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.MoreSemenHarderSucks".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.AwakeOutBlur".Localized());

                OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.TimePassedSoreOrgans".Localized());
            }
            else
                if (Player.Corruption > 66)
                    OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.GrinBuryDickMaw".Localized());
                else
                    OutputText("Scenes.Masturbation.AllNaturalOnaholeUse.RegretPurchaseLonging".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, -1.5f), new DynStat(GameStats.Sensitivity, .75f), new DynStat(GameStats.Corruption, .5f));

            UpdateMasturbation();

            Player.Orgasm("Dick");

            ReturnToCamp(1);
        }

        public void StimBeltUse()
        {
            ClearOutput();

            if (Player.HasKeyItem("Self-Stimulation Belt"))
            {
                ShowImage("masti-stimBelt");

                if (!Player.HasStatusEffect(StatusEffectIds.used_self_dash_stim))
                {
                    Player.CreateStatusEffect(StatusEffectIds.used_self_dash_stim, 0, 0, 0, 0);

                    OutputText("Scenes.Masturbation.StimBeltUse.WindUpGearbox".Localized());

                    OutputText(Player.BreastCup(0));

                    OutputText("Scenes.Masturbation.StimBeltUse.TitsConvulseOrgasms".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                    ReturnToCamp(1);
                }
                else
                {
                    OutputText("Scenes.Masturbation.StimBeltUse.NeedReleaseFumble".Localized());

                    OutputText("Scenes.Masturbation.StimBeltUse.BeltVibratesVagina".Localized());

                    OutputText("Scenes.Masturbation.StimBeltUse.MachineTweaksNerves".Localized());

                    OutputText("Scenes.Masturbation.StimBeltUse.LocateKeyNearFeet".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

                    ReturnToCamp(1);
                }

                UpdateMasturbation();

                Player.CuntChange(1, true, true, false);
            }
        }

        public void AllNaturalStimBeltUse()
        {
            ClearOutput();

            if (Player.HasKeyItem("All-Natural Self-Stimulation Belt"))
            {
                ShowImage("masti-stimBelt-allNatural");

                if (!Player.HasStatusEffect(StatusEffectIds.UsedNaturalSelfStim))
                {
                    Player.CreateStatusEffect(StatusEffectIds.UsedNaturalSelfStim, 0, 0, 0, 0);

                    OutputText("Scenes.Masturbation.AllNaturalStimBeltUse.PutOnGlovesBelt".Localized());

                    OutputText("Scenes.Masturbation.AllNaturalStimBeltUse.SquishingBeltTightens".Localized());

                    AllNaturalSelfStimulationBeltContinuation();
                }
                else
                    if (Player.Corruption < 50)
                    {
                        OutputText("Scenes.Masturbation.AllNaturalStimBeltUse.ShamedBySexualNeeds".Localized());

                        OutputText("Scenes.Masturbation.AllNaturalStimBeltUse.CreaturePenetratesCervix".Localized());

                        Player.Orgasm("Vaginal");

                        Player.ApplyStats(new DynStat(GameStats.Libido, -1f), new DynStat(GameStats.Sensitivity, .75f), new DynStat(GameStats.Corruption, 1f));

                        ReturnToCamp(1);
                    }
                    else
                    {
                        OutputText("Scenes.Masturbation.AllNaturalStimBeltUse.StripSlideBeltOn".Localized());

                        OutputText("Scenes.Masturbation.AllNaturalStimBeltUse.CreatureCumTorrent".Localized());

                        Player.Orgasm("Vaginal");

                        Player.ApplyStats(new DynStat(GameStats.Libido, -.5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Corruption, 1.5f));

                        if (!Player.IsPureEnough(100, false))
                            AllNaturalSelfStimulationBeltBadEnd();
                        else
                            ReturnToCamp(4);
                    }
            }

            UpdateMasturbation();

            Player.CuntChange(1, true);

            Player.SlimeFeed();
        }

        public void AllNaturalSelfStimulationBeltContinuation()
        {
            ClearOutput();

            ShowImage("masti-stimBelt-allNatural");

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltContinuation.ScreamOrganicDildoGrows".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltContinuation.SuddenPhaseShift".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltContinuation.FiendishBeltShiftsAgain".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltContinuation.EternityPussySore".Localized());

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -0.5f));

            ReturnToCamp(1);
        }

        public void AllNaturalSelfStimulationBeltBadEnd()
        {
            ClearOutput();

            ShowImage("badend-stimBelt");

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltBadEnd.BeltNoLongerMatters".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltBadEnd.AwarenessCompromisedFamiliar".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltBadEnd.GiacomoSpeaksAddictive".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltBadEnd.GiacomoContemplatesWrithe".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltBadEnd.PurveyorNewProduct".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltBadEnd.GiacomoCacklesIdea".Localized());

            OutputText("Scenes.Masturbation.AllNaturalSelfStimulationBeltBadEnd.GiacomoLoadsCart".Localized());

            GameOver();
        }

        public void LickYerGirlParts()
        {
            ClearOutput();

            ShowImage("masti-lick-girl");

            if (!Player.HasPerk(PerkIds.Flexibility))
            {
                OutputText("Scenes.Masturbation.LickYerGirlParts.UndressArmorSpreadLegs".Localized(
                    Player.VaginaDescript()
                ));

                Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

                ReturnToCamp(1);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesAutofellatioDueToCatFlexability, CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) + 1);

                OutputText("Scenes.Masturbation.LickYerGirlParts.TakeOffArmorLook".Localized(
                    Player.VaginaDescript()
                ));

                OutputText("Scenes.Masturbation.LickYerGirlParts.FlickTongueAmazing".Localized(
                    Player.ClitDescript()
                ));

                OutputText("Scenes.Masturbation.LickYerGirlParts.CoverPussyHotAir".Localized());
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesAutofellatioDueToCatFlexability, CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) + 1);

                OutputText("Scenes.Masturbation.LickYerGirlParts.QuickUndressBothMouths".Localized(
                    Appearance.AllBreastsDescript(Player),

                    Player.AssDescript(),

                    Appearance.AllBreastsDescript(Player)
                ));

                OutputText("Scenes.Masturbation.LickYerGirlParts.FaceRightBeforeYou".Localized(
                    Player.VaginaDescript(),

                    Player.ClitDescript()
                ));

                OutputText("Scenes.Masturbation.LickYerGirlParts.LapHarderFaster".Localized());
            }

            UpdateMasturbation();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -0.5f));

            ReturnToCamp(1);
        }

        public void CatAutoLick()
        {
            ClearOutput();

            ShowImage("masti-lick-boy");

            if (!Player.HasPerk(PerkIds.Flexibility))
            {
                OutputText("Scenes.Masturbation.CatAutoLick.UndressArmorPointing".Localized(
                    Player.CockDescript(),

                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

                ReturnToCamp(1);

                return ;
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) == 0)
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesAutofellatioDueToCatFlexability, CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) + 1);

                OutputText("Scenes.Masturbation.CatAutoLick.UndressInspectBody".Localized(
                    Player.CockDescript()
                ));

                OutputText("Scenes.Masturbation.CatAutoLick.LickManMeatHead".Localized(
                    Player.CockDescript()
                ));

                OutputText("Scenes.Masturbation.CatAutoLick.FamiliarPleasureShudder".Localized(
                    Player.CockDescript()
                ));

                if (Player.CountCockSocks("gilded") > 0 && CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) < Player.CountCockSocks("gilded"))
                {
                    CoC.Instance.Flags.Set(GameFlagIds.GildedJerked, CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) + 1);

                    var gems = MidasCockJackingGemsRoll();

                    OutputText("Scenes.Masturbation.CatAutoLick.ChillGildedSock".Localized(
                        Player.CockDescript()
                    ));

                    if (Player.CumQ() < 25)
                        OutputText("Scenes.Masturbation.CatAutoLick.SprinkleOfPrefix".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Masturbation.CatAutoLick.RainOfPrefix".Localized());
                        else
                            OutputText("Scenes.Masturbation.CatAutoLick.TorrentOfPrefix".Localized());

                    OutputText("Scenes.Masturbation.CatAutoLick.GemsFallUponBody".Localized());

                    OutputText("Scenes.Masturbation.CatAutoLick.CollectGlitteringGems".Localized(
                        gems
                    ));

                    Player.Gems += gems;
                }
                else
                {
                    OutputText("Scenes.Masturbation.CatAutoLick.CompleteBlissSquirm".Localized());

                    if (Player.CumQ() < 1000)
                        Player.RefillHunger(Player.CumQ() / 20);
                    else
                        if (Player.CumQ() < 3000)
                            Player.RefillHunger(50 + ((Player.CumQ() - 1000) / 40));
                        else
                            Player.RefillHunger(100);
                }
            }
            else
            {
                CoC.Instance.Flags.Set(GameFlagIds.TimesAutofellatioDueToCatFlexability, CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) + 1);

                OutputText("Scenes.Masturbation.CatAutoLick.QuickUndressDroolingCock".Localized(
                    Player.CockDescript(),

                    Player.AssDescript(),

                    Appearance.AllBreastsDescript(Player)
                ));

                OutputText("Scenes.Masturbation.CatAutoLick.CockPokesFaceCheek".Localized(
                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                OutputText("Scenes.Masturbation.CatAutoLick.PreCumDownThroat".Localized(
                    Player.CockDescript()
                ));

                if (Player.CountCockSocks("gilded") > 0 && CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) < Player.CountCockSocks("gilded"))
                {
                    CoC.Instance.Flags.Set(GameFlagIds.GildedJerked, CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) + 1);

                    var gemsCreated = MidasCockJackingGemsRoll();

                    OutputText("Scenes.Masturbation.CatAutoLick.SlightChillPopFree".Localized(
                        Player.CockDescript()
                    ));

                    if (Player.CumQ() < 25)
                        OutputText("Scenes.Masturbation.CatAutoLick.SprinkleOfPrefixAgain".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Masturbation.CatAutoLick.RainOfPrefixAgain".Localized());
                        else
                            OutputText("Scenes.Masturbation.CatAutoLick.TorrentOfPrefixAgain".Localized());

                    OutputText("Scenes.Masturbation.CatAutoLick.GemsFallBodyAgain".Localized());

                    OutputText("Scenes.Masturbation.CatAutoLick.CollectGlitteringGemsAgain".Localized(
                        gemsCreated
                    ));

                    Player.Gems += gemsCreated;
                }
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesAutofellatioDueToCatFlexability) > 10 && Player.Balls > 1 && Random.Shared.Next(5) == 0)
                    {
                        OutputText("Scenes.Masturbation.CatAutoLick.ComeUpForAir".Localized());

                        OutputText("Scenes.Masturbation.CatAutoLick.GaspScowlSack".Localized(
                            Player.CockDescript()
                        ));

                        OutputText("Scenes.Masturbation.CatAutoLick.LegOverHead".Localized(
                            Player.BallsDescriptLight()
                        ));

                        OutputText("Scenes.Masturbation.CatAutoLick.RealizeTorsoSoaked".Localized(
                            Player.CockDescript()
                        ));

                        OutputText("Scenes.Masturbation.CatAutoLick.ReleaseImmediateContractions".Localized(
                            Player.CockDescript()
                        ));

                        OutputText("Scenes.Masturbation.CatAutoLick.FeetHandsMassaging".Localized(
                            Player.CockDescript()
                        ));

                        OutputText("Scenes.Masturbation.CatAutoLick.SatiatedUntangleSweaty".Localized());

                        Player.ModCumMultiplier(0.3f);
                    }
                    else
                        OutputText("Scenes.Masturbation.CatAutoLick.PentUpPressureRelease".Localized());

                    if (Player.CumQ() < 1000)
                        Player.RefillHunger(Player.CumQ() / 20);
                    else
                        if (Player.CumQ() < 3000)
                            Player.RefillHunger(50 + ((Player.CumQ() - 1000) / 40));
                        else
                            Player.RefillHunger(100);
                }
            }

            UpdateMasturbation();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -0.5f));

            ReturnToCamp(1);
        }

        public void Meditate(string description = "rock")
        {
            ClearOutput();

            if (Player.Gender == Gender.Female)
                ShowImage("masti-Meditate-female");
            else
                ShowImage("masti-Meditate-male");

            OutputText("Scenes.Masturbation.Meditate.FlatSpotMeditate".Localized(
                description
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, -50f), new DynStat(GameStats.Corruption, (float)(-.3f - 0.3f * Player.CountCockSocks("alabaster"))));

            if (Player.HasPerk(PerkIds.Enlightened) && Player.IsPureEnough(10))
                Player.HealthChange(50);

            Player.ChangeFatigue(-10);

            ReturnToCamp(2);
        }

        public void DualBeltMasturbation()
        {
            ClearOutput();

            ShowImage("masti-stimBelt-play");

            OutputText("Scenes.Masturbation.DualBeltMasturbation.SighAtDrippingFluids".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Masturbation.DualBeltMasturbation.BeltWhirsToLife".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Masturbation.DualBeltMasturbation.LatexCoveringTail".Localized());

            OutputText("Scenes.Masturbation.DualBeltMasturbation.SuitRisesUpBody".Localized(
                Appearance.AllBreastsDescript(Player)
            ));

            OutputText("Scenes.Masturbation.DualBeltMasturbation.BeltReactivatesTightening".Localized(
                Player.CockDescript()
            ));

            OutputText("Scenes.Masturbation.DualBeltMasturbation.LatexPulsesFaster".Localized(
                Player.CockDescript(),

                Appearance.AllBreastsDescript(Player)
            ));

            if (Player.CumQ() > 200)
                OutputText("Scenes.Masturbation.DualBeltMasturbation.MassiveWord".Localized());

            OutputText("Scenes.Masturbation.DualBeltMasturbation.BalloonWord".Localized());

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Masturbation.DualBeltMasturbation.GrowsUntilItPops".Localized());

            OutputText("Scenes.Masturbation.DualBeltMasturbation.EyesCloseShiveredEcstasy".Localized());

            if (Player.BiggestLactation() >= 2)
                OutputText("Scenes.Masturbation.DualBeltMasturbation.MilkGushesDuringOrgasm".Localized());

            OutputText("Scenes.Masturbation.DualBeltMasturbation.SuitKeepsOrgasmicPlateau".Localized());

            OutputText("Scenes.Masturbation.DualBeltMasturbation.WakingWithoutLatex".Localized());

            if (Player.BiggestLactation() >= 2)
                OutputText("Scenes.Masturbation.DualBeltMasturbation.CreamyMilkStreamsFlow".Localized(
                    Appearance.AllBreastsDescript(Player)
                ));

            OutputText("Scenes.Masturbation.DualBeltMasturbation.SatedBeltRemoval".Localized());

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            UpdateMasturbation();

            Player.Orgasm("Generic");

            if (Player.LibidoPercent < 30)
                Player.ApplyStats(new DynStat(GameStats.Libido, .5f));

            if (Player.LibidoPercent < 50)
                Player.ApplyStats(new DynStat(GameStats.Libido, .5f));

            if (Player.LibidoPercent < 60)
                Player.ApplyStats(new DynStat(GameStats.Libido, .5f));

            if (Player.SensitivityPercent > 40)
                Player.Sensitivity -= 1;

            if (Player.SensitivityPercent > 60)
                Player.Sensitivity -= 1;

            if (Player.SensitivityPercent > 80)
                Player.Sensitivity -= 1;

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) > 50)
                Player.ApplyStats(new DynStat(GameStats.Toughness, -1f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) > 75)
                Player.ApplyStats(new DynStat(GameStats.Toughness, -1f));

            ReturnToCamp(1);
        }

        public bool CentaurMasturbation()
        {
            ClearOutput();

            var canMasturbateHugeCock = Player.HasCock() && (Player.Tallness * (5 / 6) < Player.Cocks[Player.BiggestCockIndex()].Length);

            if (Player.HasFuckableNipples())
            {
                if (canMasturbateHugeCock && Random.Shared.Next(2) == 0)
                {
                    CentaurHugeCock();

                    return true;
                }

                CentaurNippleCunt();

                return true;
            }
            else
                if (canMasturbateHugeCock)
                {
                    CentaurHugeCock();

                    return true;
                }

            CentaurCantMasturbate();

            return false;
        }

        public void CentaurNippleCunt()
        {

            OutputText("Scenes.Masturbation.CentaurNippleCunt.ShrugOutOfArmor".Localized());

            if (SceneInstance<CeraphScene>.Get().HasExhibition())
                OutputText("Scenes.Masturbation.CentaurNippleCunt.PantingEnvisionBeingCaught".Localized());
            else
                if (Player.Corruption < 33)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.BlushingLookingDown".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Masturbation.CentaurNippleCunt.AirHitsExposedNipples".Localized());
                    else
                        OutputText("Scenes.Masturbation.CentaurNippleCunt.SmilingOglingNipples".Localized());

            OutputText("Scenes.Masturbation.CentaurNippleCunt.OpeningsSoftAndSlick".Localized());

            if (Player.TotalNipples() > 2)
                OutputText("Scenes.Masturbation.CentaurNippleCunt.TwoOfPrefix".Localized());

            OutputText("Scenes.Masturbation.CentaurNippleCunt.ThemWord".Localized());

            if (Player.BiggestBreastSize() > 2)
                OutputText("Scenes.Masturbation.CentaurNippleCunt.DisplacementMakesJiggle".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurNippleCunt.ShallowOpeningsOnlyFingertip".Localized());

            OutputText("Scenes.Masturbation.CentaurNippleCunt.FreeFingersStrokeLips".Localized());

            ShowImage("masti-centaur-nipple");

            OutputText("Scenes.Masturbation.CentaurNippleCunt.KneelingShiveringPleasure".Localized());

            if (Player.BiggestLactation() > 1)
                OutputText("Scenes.Masturbation.CentaurNippleCunt.MilkyLubricantRacesDown".Localized());
            else
                if (Player.Wetness() >= 3)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.RivuletsRaceDownBreasts".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.SlickSquishFingerFuck".Localized());

            OutputText("Scenes.Masturbation.CentaurNippleCunt.MoanLeanSecondFinger".Localized());

            if (Player.HasVagina() || Player.HasCock())
            {
                var plural = false;

                OutputText("Scenes.Masturbation.CentaurNippleCunt.DeniedSingleTouch".Localized());

                if (Player.HasCock())
                {
                    if (Player.HasVagina())
                        OutputText("Scenes.Masturbation.CentaurNippleCunt.AndWord".Localized());
                    else
                        OutputText(Player.CockDescript());

                    if (Player.HasVagina() || Player.Cocks.Count > 1)
                        plural = true;
                }
                else
                    OutputText(Player.VaginaDescript());

                if (plural)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.LeakFluidQuiveringPlural".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.LeakFluidQuiveringSingular".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.FeelEachCockTapping".Localized());
                else
                    if (Player.Cocks.Count == 1)
                        OutputText("Scenes.Masturbation.CentaurNippleCunt.FeelTappingCocks".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.RockingInsideSackBalls".Localized());

                OutputText("Scenes.Masturbation.CentaurNippleCunt.ParagraphBreak".Localized());
            }

            OutputText("Scenes.Masturbation.CentaurNippleCunt.SlowWaveContractionsStart".Localized());

            if (Player.HasCock())
            {
                if (Player.CumQ() < 50)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.SmallPuddleOfSemen".Localized());
                else
                    if (Player.CumQ() < 200)
                        OutputText("Scenes.Masturbation.CentaurNippleCunt.DecentPuddleWhiteSeed".Localized());
                    else
                        if (Player.CumQ() < 1000)
                            OutputText("Scenes.Masturbation.CentaurNippleCunt.LargePuddleWhiteSeed".Localized());
                        else
                            if (Player.CumQ() < 2000)
                                OutputText("Scenes.Masturbation.CentaurNippleCunt.FrothingPuddleThickCum".Localized());
                            else
                                OutputText("Scenes.Masturbation.CentaurNippleCunt.SmallLakeOfSemen".Localized());

                OutputText("Scenes.Masturbation.CentaurNippleCunt.ReleasedByIntenseClimax".Localized());
            }

            if (Player.HasVagina())
                if (Player.Wetness() < 2)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.PussyScentHangs".Localized());
                else
                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Masturbation.CentaurNippleCunt.HorseScentFillsAir".Localized());
                    else
                        if (Player.Wetness() < 5)
                            OutputText("Scenes.Masturbation.CentaurNippleCunt.SaturatedArousedHorseScent".Localized());
                        else
                            OutputText("Scenes.Masturbation.CentaurNippleCunt.ThickBestialMusk".Localized());

            if (Player.HasCock() || Player.HasVagina())
                OutputText("Scenes.Masturbation.CentaurNippleCunt.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Masturbation.CentaurNippleCunt.TimePassedAnHour".Localized());

            if (Player.Corruption > 66)
                OutputText("Scenes.Masturbation.CentaurNippleCunt.EagerToRepeat".Localized());
            else
                if (Player.Corruption > 33)
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.ConfusedByEnjoyment".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurNippleCunt.HorrifiedAtPerversion".Localized());

            Player.Orgasm("Nipples");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -0.5f));
        }

        public void CentaurHugeCock()
        {

            var primary = Player.BiggestCockIndex();

            var plural = (Player.Cocks.Count > 1);

            OutputText("Scenes.Masturbation.CentaurHugeCock.YouShrugOutArmor".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Masturbation.CentaurHugeCock.SighDisappointmentAndShame".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.SighInResignation".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurHugeCock.SmileWithExcitement".Localized());

            OutputText("Scenes.Masturbation.CentaurHugeCock.YouBlushAs".Localized());

            if (plural)
                OutputText("Scenes.Masturbation.CentaurHugeCock.EachOf".Localized());

            OutputText("Scenes.Masturbation.CentaurHugeCock.CocksSwellUnderneath".Localized());

            if (!plural)
                OutputText("Scenes.Masturbation.CentaurHugeCock.ItGrowsFullHard".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurHugeCock.TheyGrowFullHard".Localized());

            if (plural)
                OutputText("Scenes.Masturbation.CentaurHugeCock.AfterMomentTheyPeek".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurHugeCock.AfterMomentItPeeks".Localized(
                    primary + 1
                ));

            if (Player.Tallness * 1.5f < Player.Cocks[Player.BiggestCockIndex()].Length)
            {
                OutputText("Scenes.Masturbation.CentaurHugeCock.ImmobilizedByLength".Localized());

                if (plural)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.AllWord".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.YourWord".Localized());
            }

            OutputText("Scenes.Masturbation.CentaurHugeCock.ParagraphBreak".Localized());

            ShowImage("masti-centaur-huge");

            if (Player.Tallness * 1.5f >= Player.Cocks[Player.BiggestCockIndex()].Length)
            {
                OutputText("Scenes.Masturbation.CentaurHugeCock.BendBothHands".Localized());

                if (plural)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.AWord".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurHugeCock.YourWordAgain".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.LiftAndPressBelly".Localized(
                    Player.CockDescript(primary)
                ));

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.ThickPrecumStreams".Localized());
                else
                    if (Player.CumQ() > 100)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.PrecumDribbleStarts".Localized());
                    else
                        OutputText("Scenes.Masturbation.CentaurHugeCock.PrecumTinyDollop".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Masturbation.CentaurHugeCock.NeglectedVaginaAches".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.SighDelightedCaress".Localized());

                if (Player.Corruption < 40)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.WorriedAboutPerversion".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.FeelsGoodWhinny".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.SmearPrecumTip".Localized(
                    Player.CockDescript(primary)
                ));

                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.OtherDickMimics".Localized());
                else
                    if (Player.Cocks.Count > 2)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.CocksMimicBrothers".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.HindLegsSlump".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.CentaurHugeCock.BendBothHandsAgain".Localized());

                if (plural)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.AWordAgain".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurHugeCock.YourWordThird".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.LiftCurvingUnderside".Localized(
                    Player.CockDescript(primary)
                ));

                if (Player.CumQ() > 500)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.ThickPrecumStreamsAgain".Localized());
                else
                    if (Player.CumQ() > 100)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.PrecumDribbleStartsAgain".Localized());
                    else
                        OutputText("Scenes.Masturbation.CentaurHugeCock.PrecumTinyDollopAgain".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Masturbation.CentaurHugeCock.NeglectedVaginaAchesAgain".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.SighDelightedOrgans".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.WorriedAboutPerversionAgain".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.FeelsGoodFondleLength".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.SmearPrecumTipAgain".Localized(
                    Player.CockDescript()
                ));

                if (Player.Cocks.Count == 2)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.OtherDickMimicsAgain".Localized());
                else
                    if (Player.Cocks.Count > 2)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.CocksMimicBrothersAgain".Localized());

                OutputText("Scenes.Masturbation.CentaurHugeCock.HindLegsSlumpAgain".Localized());
            }

            if (Player.Tallness * 1.5f >= Player.Cocks[Player.BiggestCockIndex()].Length)
                OutputText("Scenes.Masturbation.CentaurHugeCock.SqueezeTightlyAround".Localized(
                    Player.CockHead()
                ));
            else
                OutputText("Scenes.Masturbation.CentaurHugeCock.PumpingHard".Localized());

            OutputText("Scenes.Masturbation.CentaurHugeCock.OrgasmReliefFlow".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Masturbation.CentaurHugeCock.YourWordFourth".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurHugeCock.ReleaseWord".Localized());

            OutputText("Scenes.Masturbation.CentaurHugeCock.ArchBackBlastSeed".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Masturbation.CentaurHugeCock.FinishUnloadingAgainstIt".Localized());
            else
                if (Player.CumQ() < 200)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.ContinuePumpMoreSeed".Localized());
                else
                    if (Player.CumQ() < 500)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.ContinueFountainSpooge".Localized());
                    else
                        if (Player.CumQ() < 1500)
                            OutputText("Scenes.Masturbation.CentaurHugeCock.ContinueBlastCum".Localized());
                        else
                        {
                            OutputText("Scenes.Masturbation.CentaurHugeCock.ContinueEruptCum".Localized());

                            if (Player.CumQ() > 3000)
                                OutputText("Scenes.Masturbation.CentaurHugeCock.FinishCummingPuddle".Localized());
                        }

            OutputText("Scenes.Masturbation.CentaurHugeCock.SlumpBackShuddering".Localized());

            if (!plural)
                OutputText("Scenes.Masturbation.CentaurHugeCock.MemberWord".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurHugeCock.MemberWordPlural".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
            {
                if (plural)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.GrowLimpSlideBack".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurHugeCock.GrowingLimpSlidingBack".Localized());
            }
            else
                if (Player.MinLibido > 40 || Player.LibidoPercent > 50)
                {
                    if (plural)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.LessHardUnsurprised".Localized());
                    else
                        OutputText("Scenes.Masturbation.CentaurHugeCock.GrowingLessHardUnsurprised".Localized());
                }
                else
                    if (plural)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.GrowLimpShrinking".Localized());
                    else
                        OutputText("Scenes.Masturbation.CentaurHugeCock.GrowingLimpShrinking".Localized());

            OutputText("Scenes.Masturbation.CentaurHugeCock.ParagraphBreakAgain".Localized());

            if (Player.HasVagina())
            {
                if (Player.Wetness() <= 1)
                    OutputText("Scenes.Masturbation.CentaurHugeCock.WetnessTrickleThighs".Localized());
                else
                    if (Player.Wetness() == 2)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.FluidsLeakThighs".Localized());
                    else
                        if (Player.Wetness() == 3)
                            OutputText("Scenes.Masturbation.CentaurHugeCock.ClearFluidSoaksThighs".Localized());
                        else
                            if (Player.Wetness() == 4)
                                OutputText("Scenes.Masturbation.CentaurHugeCock.ThickFluidCoatsThighs".Localized());
                            else
                                OutputText("Scenes.Masturbation.CentaurHugeCock.FluidsGushBetweenThighs".Localized());

                if (Player.HasFuckableNipples())
                {
                    if (Player.TotalNipples() > 2)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.AllWordAgain".Localized());
                    else
                        OutputText("Scenes.Masturbation.CentaurHugeCock.BothWord".Localized());

                    OutputText("Scenes.Masturbation.CentaurHugeCock.NipplesQuiver".Localized(
                        NumberUtils.Num2Text(Player.TotalNipples())
                    ));

                    if (Player.Wetness() < 4 && Player.BiggestLactation() < 2)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.LeakingWord".Localized());
                    else
                        if (Player.Wetness() < 5 && Player.BiggestLactation() < 3)
                            OutputText("Scenes.Masturbation.CentaurHugeCock.DrippingWord".Localized());
                        else
                            OutputText("Scenes.Masturbation.CentaurHugeCock.SquirtingWord".Localized());

                    OutputText("Scenes.Masturbation.CentaurHugeCock.EqualQuantities".Localized());

                    if (Player.BiggestLactation() < 1)
                        OutputText("Scenes.Masturbation.CentaurHugeCock.ClearWord".Localized());
                    else
                        OutputText("Scenes.Masturbation.CentaurHugeCock.MilkLacedWord".Localized());

                    OutputText("Scenes.Masturbation.CentaurHugeCock.FingerEntrancesPause".Localized());
                }

                OutputText("Scenes.Masturbation.CentaurHugeCock.ParagraphBreakThird".Localized());
            }

            OutputText("Scenes.Masturbation.CentaurHugeCock.SatedRestHour".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -0.5f));
        }

        public void CentaurCantMasturbate()
        {

            ShowImage("masti-centaur-fail");

            if (CoC.Instance.Flags.Get(GameFlagIds.UnableToMasturbateBecauseCentaur) == 0)
            {
                OutputText("Scenes.Masturbation.CentaurCantMasturbate.TwistTurnCannotReach".Localized());

                if (Player.HasCock() || Player.HasVagina())
                    OutputText("Scenes.Masturbation.CentaurCantMasturbate.GenitaliaWord".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurCantMasturbate.AnythingRemotelySexual".Localized());

                OutputText("Scenes.Masturbation.CentaurCantMasturbate.CentaurCripplingDownside".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.UnableToMasturbateBecauseCentaur, CoC.Instance.Flags.Get(GameFlagIds.UnableToMasturbateBecauseCentaur) + 1);
            }
            else
                OutputText("Scenes.Masturbation.CentaurCantMasturbate.StillCannotReachAround".Localized());

            if (Player.HasKeyItem("Cock Milker - Installed At Whitney's Farm") && Player.Cocks.Count > 0)
                OutputText("Scenes.Masturbation.CentaurCantMasturbate.VisitCockMilkerFarm".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurCantMasturbate.FindPartnerRelieveNeed".Localized());
        }

        public void CentaurDudesGetHorseAids()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            if (Player.KeyItemV1("Fake Mare") == 0)
                if (Player.Corruption < 50)
                    OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.MareCocksleeveTrial".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.PlayMareShapedToy".Localized());

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.ToyGapingGenitals".Localized());

            if (Player.CockArea(x) < 30)
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.HandSlipsTooBig".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.FistSlipsPreparatoryThrusts".Localized());

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.ParagraphBreak".Localized());

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.MountToyReady".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) < 30)
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.PressTightAnusRing".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.LineUpGapingCunt".Localized());

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.RearHipsSlam".Localized());

            ShowImage("masti-centaur-cPole-male");

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.ToyPassageContract".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.AndWord".Localized());

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.HardAgainstRump".Localized());

            if (Player.CockArea(x) >= 30)
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.HorsecuntWord".Localized());
            else
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.AssholeWord".Localized());

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.PeriodParagraphBreak".Localized());

            OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.FinallyGetRelease".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CountCockSocks("gilded") > 0 && CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) < Player.CountCockSocks("gilded"))
            {
                CoC.Instance.Flags.Set(GameFlagIds.GildedJerked, CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) + 1);

                var gems = MidasCockJackingGemsRoll();

                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.SatedBlissfulWarmth".Localized(
                    Player.CockDescript(x)
                ));

                if (Player.CockArea(x) >= 30)
                    OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.HorsecuntWordAgain".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.AssholeWordAgain".Localized());

                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.ScrambleGatherShinies".Localized(
                    gems
                ));

                Player.Gems += gems;
            }
            else
            {
                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.UntilFullLeaking".Localized());

                OutputText("Scenes.Masturbation.CentaurDudesGetHorseAids.SatedBlissfulWarmthAgain".Localized(
                    Player.CockDescript(x)
                ));
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);

            Player.AddKeyValue("Fake Mare", 1, 1);
        }

        public void CentaurGirlsGetHorseAids()
        {
            ClearOutput();

            if (Player.KeyItemV1("Centaur Pole") == 0)
                if (Player.Corruption < 50)
                    OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.AntsyPoleExperiment".Localized());
                else
                    OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.CentaurLustUnsated".Localized());

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.StatuePoleAdmiration".Localized());

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.RubberyHeftInside".Localized());

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.HipsWiggleAnticipation".Localized(
                Player.VaginaDescript()
            ));

            Player.CuntChange(Player.VaginalCapacity() - 3, true, true, false);

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.ParagraphBreak".Localized());

            ShowImage("masti-centaur-cPole-female");

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.ToyStallsInside".Localized());

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.DesperateCurseRamming".Localized());

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.HotFluidOrgasm".Localized());

            OutputText("Scenes.Masturbation.CentaurGirlsGetHorseAids.ClimaxCollapseSwaying".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);

            Player.AddKeyValue("Centaur Pole", 1, 1);
        }

        public void GetHugeEggsInCawk()
        {
            ClearOutput();

            ShowImage("masti-cock-eggs");

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 1 && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 1) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesEggedInCock) == 0)
            {
                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.TimeForLittleFun".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.ArmorOffSettleDown".Localized());

                if (Player.Corruption < 33)
                {
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BedrollNearby".Localized());

                    if (CampScene.HasCompanions())
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.CompanionsMightSee".Localized());
                }
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.RockConcealedSpot".Localized());
                    else
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.WastesOutsideCamp".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.TendingEndowments".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.LayingBackStroking".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.StartledChampion".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.FlexingOvipositorOut".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.DemonFightsBack".Localized());
                // 原版 Masturbation.hx:3489 — ShouldraFollower.hx：Shouldra 是跟随者
                if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.ShouldraAssists".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.WrestlingDemonControl".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SlitStretchingSensation".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.DenialProtest".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.InvoluntaryEggPush".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BlackoutSensation".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.MoreEggsRising".Localized(
                    NumberUtils.Num2Text(Player.Cocks[Player.BiggestCockIndex()].Length)
                ));

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.StruggleEffort".Localized());
                // 原版 Masturbation.hx:3499 — ShouldraFollower.hx：Shouldra 是跟随者
                if (SceneInstance<ShouldraFollowerScene>.Get().FollowerShouldra())
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.GhostAssisted".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.WillVersusOrgasmEggs".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.FlexClenchStinger".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.OvipositorOverdrive".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BallsPlaceholder".Localized());
                else
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.HipsPlaceholder".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SentencePeriod".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.EggsPushTesticles".Localized());

                    if (Player.FertilizedEggs() > 0)
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.FertilizationCertainty".Localized());

                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SentencePeriodAgain".Localized());
                }
                else
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.AbdominalPressure".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SackBulgesKnobbly".Localized());
                else
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BellyBulgesPregnant".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.EggAfterEgg".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.NappingExgartuanVoice".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.TiredLaughterSleep".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.RandyRetreat".Localized());

                if (Player.Corruption < 33)
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SlipIntoBedroll".Localized());
                else
                    if (Player.Corruption < 66)
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.OffBehindRock".Localized());
                    else
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.TreesOutsideCamp".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.StripArmorDown".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.ComfortableStroking".Localized());

                if (Player.Gender == Gender.Hermaphrodite)
                {
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.CockExamination".Localized());

                    if (Player.Cocks.Count > 1)
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SLetter".Localized());

                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.PussyAlternative".Localized());

                    if (Player.Strength > 50)
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.StrengthInsufficient".Localized());

                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BendingFailsPain".Localized());
                }
                else
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.ConsideringOnlyHole".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.CockOnlyOption".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.OvipositorCurlsInward".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.GoingForBroke".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BaskingMomentRest".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.PreFlowsFreely".Localized());

                if (Player.Corruption >= 75)
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.CorruptedWord".Localized());
                else
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.TransformedWord".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.DeeperThrustEggFeel".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.CockHeadPrick".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.ForgottenStinger".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.LittleMorePushing".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BallsPlaceholderAgain".Localized());
                else
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.HipsPlaceholderAgain".Localized());

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SentencePeriodThird".Localized());

                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SackFillAnticipation".Localized());

                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.ForceEggProstate".Localized());

                    if (!Player.IsTaur())
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.MassagingBloatingBalls".Localized());

                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.HoneyVenomNumbing".Localized());
                }
                else
                {
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.EggDestinationWonder".Localized());

                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.EggSlipsPast".Localized());
                }

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BaskingEggHoney".Localized());

                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Masturbation.GetHugeEggsInCawk.IdlyFondling".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.SwollenBelly".Localized());
                    else
                        OutputText("Scenes.Masturbation.GetHugeEggsInCawk.BulgingSack".Localized());
                }

                OutputText("Scenes.Masturbation.GetHugeEggsInCawk.DreamingLayingEggs".Localized());
            }

            if (Player.FertilizedEggs() > 0 && CoC.Instance.Flags.Get(GameFlagIds.DickEggIncubation) == 0)
                CoC.Instance.Flags.Set(GameFlagIds.DickEggIncubation, 48);

            Player.DumpEggs();

            Player.Orgasm("Generic");

            CoC.Instance.Flags.Set(GameFlagIds.TimesEggedInCock, CoC.Instance.Flags.Get(GameFlagIds.TimesEggedInCock) + 1);

            ReturnToCamp(1);
        }

        public void BirthBeeEggsOutYourWang()
        {

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.HeightenedLust".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.QuickShedSitting".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.BallsFeelVariant".Localized());
            else
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.StomachFeelsVariant".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.HeavyAndConnector".Localized());

            if (Player.IsTaur())
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.YLetter".Localized());
            else
            {
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.CaressAction".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.CaressThey".Localized());
                else
                    OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.CaressIt".Localized());

                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.FreeHandStroking".Localized());
            }

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.EggsShiftingInside".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.LyingBackErection".Localized());

            if (Player.Balls == 0)
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.StomachWord".Localized());
            else
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.BallsWord".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.EggsShiftOutward".Localized());

            ShowImage("birth-cock-eggs");

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.PushingCockBase".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.PantingFuriousPace".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.SingleBulgeBloats".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.OverwhelmingPileup".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.CockBaseBloating".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.SpasmCurlingIn".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.PleasureExplosion".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.LostEggCount".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.AwakeningCovered".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.SmilingAtChildren".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.WatchingHatchlings".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.AmusedWord".Localized());
            else
                OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.MildAnnoyance".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.LandingOnNose".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.OthersHatching".Localized());

            OutputText("Scenes.Masturbation.BirthBeeEggsOutYourWang.LighterHappierMood".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.DickEggIncubation, 0);

            Player.Orgasm("Dick");
        }

        public void LayEggsInYerTits()
        {
            ClearOutput();

            ShowImage("masti-eggs-in-tits");

            if (Player.StatusEffectValue(StatusEffectIds.Exgartuan, 0) == 2 && Player.StatusEffectValue(StatusEffectIds.Exgartuan, 1) == 0)
            {
                OutputText("Scenes.Masturbation.LayEggsInYerTits.MischievousLookDown".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.ExgartuanTauntsAboutEggs".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.GrinTurnsConfused".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.ExgartuanJustJoking".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.BoundlessEnthusiasm".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.PropUpOnElbow".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.ExgartuanPipesUp".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.OvipositorDrawnUnbidden".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.NoticeHandsFree".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.MakeMeAMomma".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.PregnantDemonQuestion".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.AmazingPushingFeeling".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.FirstEggDownTube".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.ExertIntoNipple".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.KeeningSharedOrgasm".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.StifledMovementProtest".Localized());

                if (Player.Cocks.Count > 1)
                    OutputText("Scenes.Masturbation.LayEggsInYerTits.CocksDribblingCum".Localized());
                else
                    if (Player.Cocks.Count == 1)
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.CockDribblesCumSingular".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Masturbation.LayEggsInYerTits.StickyFluidsSlowly".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.EggsComeToStop".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.SubduedMuffledRemark".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.OvipositorsPullBack".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.GetAlongThought".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.AgreeThenPassOut".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.LayEggsInYerTits.BaserUrgesDecision".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.RemoveArmorLieSide".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.MountingAnticipation".Localized());

                if (Player.HasVagina())
                {
                    if (Player.Balls > 0)
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.BehindBallsYPrefix".Localized());
                    else
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.YLetter".Localized());

                    OutputText("Scenes.Masturbation.LayEggsInYerTits.VaginaDripsOpen".Localized());
                }

                OutputText("Scenes.Masturbation.LayEggsInYerTits.OvipositorPeeksOut".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.NipplePulledWide".Localized());

                if (Player.CanOvipositBee())
                    OutputText("Scenes.Masturbation.LayEggsInYerTits.HoneyWord".Localized());
                else
                    OutputText("Scenes.Masturbation.LayEggsInYerTits.GreenFluidWord".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.SquirtsFromPressure".Localized());

                OutputText("Scenes.Masturbation.LayEggsInYerTits.SexualGleeJackknife".Localized());

                if (Player.HasVagina())
                {
                    OutputText("Scenes.Masturbation.LayEggsInYerTits.VaginaFullyParted".Localized());

                    if (Player.Wetness() >= 4)
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.LiberallyWord".Localized());

                    OutputText("Scenes.Masturbation.LayEggsInYerTits.OntoLegsAbdomen".Localized());
                }

                OutputText("Scenes.Masturbation.LayEggsInYerTits.OvipositorBeginsTask".Localized());

                if (Player.Eggs() < 20)
                {
                    OutputText("Scenes.Masturbation.LayEggsInYerTits.ContentedGlowEggs".Localized());

                    if (Player.CanOvipositBee())
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.HoneyWordAgain".Localized());
                    else
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.GooWord".Localized());

                    OutputText("Scenes.Masturbation.LayEggsInYerTits.ThrustIntoOtherBreast".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.CoatedInWhite".Localized());

                    OutputText("Scenes.Masturbation.LayEggsInYerTits.NoMoreEggs".Localized());
                }
                else
                    if (Player.Eggs() < 40)
                    {
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.ChestTingleHeartRace".Localized());

                        OutputText("Scenes.Masturbation.LayEggsInYerTits.DrapingArmOverTit".Localized());

                        OutputText("Scenes.Masturbation.LayEggsInYerTits.EggSacEmptied".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Masturbation.LayEggsInYerTits.HeartRacesEggWaves".Localized());

                        OutputText("Scenes.Masturbation.LayEggsInYerTits.DrapedArmOtherNipple".Localized());

                        if (Player.BreastRows.Count > 1)
                            OutputText("Scenes.Masturbation.LayEggsInYerTits.SwitchToAnotherNipple".Localized());
                        else
                            OutputText("Scenes.Masturbation.LayEggsInYerTits.HalfOrgasmicMoan".Localized());

                        if (Player.BreastRows.Count < 2)
                        {
                            OutputText("Scenes.Masturbation.LayEggsInYerTits.ContemplatePulsatingOrgan".Localized());

                            OutputText("Scenes.Masturbation.LayEggsInYerTits.EggsResumeAdvance".Localized());

                            OutputText("Scenes.Masturbation.LayEggsInYerTits.StomachUncomfortablyFull".Localized());
                        }
                    }
            }

            if (!Player.HasStatusEffect(StatusEffectIds.Eggchest))
                Player.CreateStatusEffect(StatusEffectIds.Eggchest, 3 + Random.Shared.Next(10), 1 + Random.Shared.Next(4), 0, 0);

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 1f));

            Player.DumpEggs();

            ReturnToCamp(1);
        }

        public void TentacleSelfFuck()
        {
            ClearOutput();

            var x = -1;

            var y = -1;

            var temp = 0;

            while (temp < Player.Cocks.Count)
            {
                if ((Player.Cocks[temp].Id == new Identifier("coc", "tentacle")) && (x == -1))
                        x = temp;

                temp += 1;
            }

            temp = 0;

            while (temp < Player.Cocks.Count)
            {
                if (temp != x)
                    if (y < 0)
                        y = temp;
                    else
                        if (Random.Shared.Next(2) == 0 && Player.Cocks[y].Id != new Identifier("coc", "tentacle"))
                            y = temp;
                        else
                            if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle"))
                                y = temp;

                temp += 1;
            }

            DoStripCheck();

            OutputText("Scenes.Masturbation.TentacleSelfFuck.EagerCockPerksUp".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Wetness() <= 2)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.MoistWord".Localized());
            else
                if (Player.Wetness() <= 3)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.WetWord".Localized());
                else
                    if (Player.Wetness() <= 4)
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.SloppyAndWet".Localized());
                    else
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.BeyondSoaked".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.SlickSlitPartsSlowly".Localized());

            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Masturbation.TentacleSelfFuck.BrushHandAgainstOther".Localized());

                if (Player.LustPercent <= 70)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.HalfHard".Localized());
                else
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.ErectWord".Localized());

                OutputText("Scenes.Masturbation.TentacleSelfFuck.PenisWord".Localized());

                if (Player.Cocks.Count > 2)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.EsLetter".Localized());

                OutputText("Scenes.Masturbation.TentacleSelfFuck.OnTheWay".Localized());
            }

            OutputText("Scenes.Masturbation.TentacleSelfFuck.HandleAlienFlood".Localized());

            if (y >= 0)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.TendrilLoopsYourCock".Localized(
                    Player.CockDescript(y)
                ));

            OutputText("Scenes.Masturbation.TentacleSelfFuck.ParagraphBreak".Localized());

            ShowImage("masti-tentacle-vag");

            OutputText("Scenes.Masturbation.TentacleSelfFuck.ArchAgainstDesire".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.LiftBallsAside".Localized());
            else
                OutputText("Scenes.Masturbation.TentacleSelfFuck.PronounYou".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.TwistWristDownEases".Localized(
                Player.CockDescript(x)
            ));

            Player.CuntChange(Player.CockArea(x), true, true, false);

            OutputText("Scenes.Masturbation.TentacleSelfFuck.RipplesAlongYourCock".Localized(
                Player.CockDescript(x)
            ));

            if (y >= 0)
            {
                OutputText("Scenes.Masturbation.TentacleSelfFuck.JerkedOffByCoil".Localized(
                    Player.CockDescript(y)
                ));

                if (Player.Cocks[y].Id == new Identifier("coc", "tentacle"))
                {
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.SecondTendrilReachesAss".Localized());

                    Player.ButtChange(Player.CockArea(y), true, true, false);
                }
            }

            OutputText("Scenes.Masturbation.TentacleSelfFuck.GrabChestDelirious".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.KneadSoftMammary".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.TwistNippleWhiteHot".Localized());

            if (Player.HasFuckableNipples())
                OutputText("Scenes.Masturbation.TentacleSelfFuck.FingerInsideNipplecunt".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.SentencePeriod".Localized());

            if (Player.LactationQ() >= 250)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.MilkSquirtsFromTeat".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 3)
            {
                OutputText("Scenes.Masturbation.TentacleSelfFuck.MoansCutOffSuddenly".Localized());

                if (Player.CountCocksOfType("TENTACLE") < Player.Cocks.Count)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.AndTheOpenAir".Localized());

                OutputText("Scenes.Masturbation.TentacleSelfFuck.SentencePeriodAgain".Localized());

                if (Player.BiggestBreastSize() >= 4)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.ShaftBetweenBoobs".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.TentacleSelfFuck.MoansRiseInPitch".Localized());

                if (y >= 0 && Player.Cocks[y].Id == new Identifier("coc", "tentacle") && Player.Cocks[y].Length >= 20)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.BendToSlurpCock".Localized());
            }

            if (Player.CountCocksOfType("TENTACLE") >= 6)
            {
                OutputText("Scenes.Masturbation.TentacleSelfFuck.GreenToolsRiseUp".Localized());

                if (Player.CountCocksOfType("TENTACLE") >= 7)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.ToolsPileInSquirming".Localized());

                OutputText("Scenes.Masturbation.TentacleSelfFuck.FuckingArmpitsDivine".Localized());
            }
            else
                if (Player.CountCocksOfType("TENTACLE") >= 4)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.LastGreenToolRises".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.FamiliarTwingesStart".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.BallsWord".Localized());
            else
                OutputText("Scenes.Masturbation.TentacleSelfFuck.GroinWord".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.OrgasmClosingInRapidly".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 3)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.GurgleWord".Localized());
            else
                OutputText("Scenes.Masturbation.TentacleSelfFuck.MoanWord".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.InnerMusclesContract".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 3)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.MultitudeOfPenises".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.VolcanoEruptsDeep".Localized());

            if (Player.CountCocksOfType("TENTACLE") == 2)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.AndAssAgain".Localized());
            else
                if (Player.CountCocksOfType("TENTACLE") > 2)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.AssAndMouthAgain".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.SentencePeriodThird".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 3)
            {
                OutputText("Scenes.Masturbation.TentacleSelfFuck.GulpSaltyFlowDown".Localized());

                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.DoesntCountForMuch".Localized());

                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.CheeksBulgeEachSquirt".Localized());
                    else
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.CumSquirtsFromCorners".Localized());
                }

                OutputText("Scenes.Masturbation.TentacleSelfFuck.SentencePeriodFourth".Localized());
            }

            OutputText("Scenes.Masturbation.TentacleSelfFuck.BirthCanalFlooded".Localized());

            if (Player.CountCocksOfType("TENTACLE") >= 2)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.AssInteriorPaintedWhite".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.SentencePeriodFifth".Localized());

            if (Player.CountCocksOfType("TENTACLE") == 2)
            {
                OutputText("Scenes.Masturbation.TentacleSelfFuck.UntendedCockBlowsSpunk".Localized(
                    Player.CockDescript(y)
                ));

                if (Player.CumQ() < 50)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.InSmallSpurts".Localized());
                else
                    if (Player.CumQ() < 200)
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.InThickRopes".Localized());
                    else
                        if (Player.CumQ() < 500)
                            OutputText("Scenes.Masturbation.TentacleSelfFuck.InHugeBlobs".Localized());
                        else
                            OutputText("Scenes.Masturbation.TentacleSelfFuck.LikeVirileSprinkler".Localized());

                OutputText("Scenes.Masturbation.TentacleSelfFuck.SentencePeriodSeventh".Localized());

                if (Player.Cocks.Count > 2)
                {
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.TheOtherOne".Localized());

                    if (Player.Cocks.Count > 3)
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.SMatchSuffix".Localized());
                    else
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.MatchesWord".Localized());

                    OutputText("Scenes.Masturbation.TentacleSelfFuck.CumsWithoutStimulus".Localized());

                    if (Player.Cocks.Count == 3)
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.PronounIt".Localized());
                    else
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.PronounThey".Localized());

                    OutputText("Scenes.Masturbation.TentacleSelfFuck.PrehensileSensationsOverwhelm".Localized());
                }
            }
            else
                if (Player.CountCocksOfType("TENTACLE") >= 4)
                {
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.MeanwhilePurpleTip".Localized());

                    if (Player.CountCocksOfType("TENTACLE") == 4)
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.SSwellSuffix".Localized());
                    else
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.SwellsWord".Localized());

                    OutputText("Scenes.Masturbation.TentacleSelfFuck.InYourArmpit".Localized());

                    if (Player.CountCocksOfType("TENTACLE") >= 5)
                        OutputText("Scenes.Masturbation.TentacleSelfFuck.SLetter".Localized());

                    OutputText("Scenes.Masturbation.TentacleSelfFuck.EngorgeIntoSoftFlesh".Localized());
                }

            OutputText("Scenes.Masturbation.TentacleSelfFuck.BodySagsBackLimp".Localized());

            if (Player.CountCocksOfType("TENTACLE") > 1)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.SRetractSuffix".Localized());
            else
                OutputText("Scenes.Masturbation.TentacleSelfFuck.RetractsWord".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.BackTo".Localized());

            if (Player.CountCocksOfType("TENTACLE") == 1)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.PronounIts".Localized());
            else
                OutputText("Scenes.Masturbation.TentacleSelfFuck.PronounTheir".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.PoppingOutNormalLength".Localized());

            if (Player.CountCocksOfType("TENTACLE") == 2)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.AndAssAgain".Localized());
            else
                if (Player.CountCocksOfType("TENTACLE") > 2)
                    OutputText("Scenes.Masturbation.TentacleSelfFuck.AssAndMouthAgain".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.LeavingYourOrifice".Localized());

            if (Player.CountCocksOfType("TENTACLE") > 1)
                OutputText("Scenes.Masturbation.TentacleSelfFuck.SLetterAgain".Localized());

            OutputText("Scenes.Masturbation.TentacleSelfFuck.DribbleSpentPassion".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, (-1 * (1 + Player.CountCocksOfType("TENTACLE")))));

            Player.KnockUp(PregnancyIds.TentacleBeastSeed, PregnancyIds.TentacleBeastSeedIncubation);

            ReturnToCamp(1);
        }

        public void TentacleGoesUpYerPooperNewsAtEleven()
        {
            ClearOutput();

            int tentacle = 0;

            tentacle = 0;

            while (tentacle < Player.Cocks.Count)
            {
                if (Player.Cocks[tentacle].Id == new Identifier("coc", "tentacle"))
                    break;

                tentacle += 1;
            }

            var x = tentacle + 1;

            while (x < Player.Cocks.Count)
            {
                if (Player.Cocks[x].Id == new Identifier("coc", "tentacle") && Player.CockArea(x) > Player.CockArea(tentacle))
                    tentacle = x;

                x += 1;
            }

            DoStripCheck();

            ShowImage("masti-tentacle-butt");

            OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.RevealFloraPecker".Localized(
                Player.CockDescript(tentacle),

                Player.CockDescript(tentacle),

                Player.CockDescript(tentacle)
            ));

            Player.ButtChange(Player.CockArea(tentacle), true, true, false);

            OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.ImpetuousnessMakesYouCry".Localized(
                Player.CockDescript(tentacle),

                Player.CockDescript(tentacle)
            ));

            OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.ConjugatedFrictionInside".Localized(
                Player.CockDescript(tentacle)
            ));

            if (Player.CumQ() >= 1000)
                OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.ExtraFeelingOverEdge".Localized());

            OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.SentencePeriod".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.BellySwellsFromSemen".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.InsidesCannotHandleLoad".Localized());

            OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.GroanRemoveCockLazily".Localized(
                Player.CockDescript(tentacle)
            ));

            if (Player.Cocks.Count > 0)
            {
                if (Player.Cocks.Count == 1)
                    if (Player.LibidoPercent < 30)
                        OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.FallAsleepSpent".Localized());
                    else
                        if (Player.LibidoPercent < 55)
                            OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.RollAndBeginDoze".Localized(
                                Player.CockDescript()
                            ));
                        else
                            if (Player.LibidoPercent <= 80)
                            {
                                OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.CloseEyesAndRelax".Localized(
                                    Player.CockDescript()
                                ));

                                if (Player.Corruption < 50)
                                    OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.DreamsFilledWithSex".Localized());
                                else
                                    OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.DreamDepravedFantasia".Localized());
                            }
                            else
                                OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.GroanDriftToSleep".Localized(
                                    Player.CockDescript()
                                ));

                if (Player.Cocks.Count > 1)
                    if (Player.LibidoPercent < 30)
                        OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.FallAsleepSpentAgain".Localized());
                    else
                        if (Player.LibidoPercent < 55)
                            OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.RollBeginDozeAgain".Localized());
                        else
                            if (Player.LibidoPercent <= 80)
                            {
                                OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.CloseEyesRelaxAgain".Localized());

                                if (Player.Corruption < 50)
                                    OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.DreamsFilledSexAgain".Localized());
                                else
                                    OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.DreamDepravedFantasiaAgain".Localized());
                            }
                            else
                                OutputText("Scenes.Masturbation.TentacleGoesUpYerPooperNewsAtEleven.GroanDriftIntoCatnap".Localized());
            }

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            ReturnToCamp(1);
        }

        public void GildedCockTurbate()
        {
            ClearOutput();

            int gildedCock = 0;

            gildedCock = 0;

            while (gildedCock < Player.Cocks.Count)
            {
                if (Player.Cocks[gildedCock].sock == "gilded")
                    break;

                gildedCock += 1;
            }

            var x = gildedCock + 1;

            while (x < Player.Cocks.Count)
            {
                if (Player.Cocks[x].sock == "gilded" && Player.CockArea(x) > Player.CockArea(gildedCock))
                    gildedCock = x;

                x += 1;
            }

            OutputText("Scenes.Masturbation.GildedCockTurbate.DisrobeShiveringSoftly".Localized());

            OutputText("Scenes.Masturbation.GildedCockTurbate.LightCatchesGoldenCock".Localized());

            ShowImage("masti-cocksock");

            OutputText("Scenes.Masturbation.GildedCockTurbate.FingersSlideOverFabric".Localized());

            if (Player.CockArea(gildedCock) < 6)
                OutputText("Scenes.Masturbation.GildedCockTurbate.EnvelopedInCocksock".Localized());
            else
                if (Player.CockArea(gildedCock) < 20)
                    OutputText("Scenes.Masturbation.GildedCockTurbate.CockHeadAbutsEdge".Localized());
                else
                    OutputText("Scenes.Masturbation.GildedCockTurbate.MetalCasingClampsDick".Localized());

            OutputText("Scenes.Masturbation.GildedCockTurbate.PressureBuildsTremors".Localized(
                Player.CockDescript(gildedCock),

                Player.CumQ()
            ));

            OutputText("Scenes.Masturbation.GildedCockTurbate.PantingWatchCumJet".Localized());

            if (Player.CumQ() < 25)
                OutputText("Scenes.Masturbation.GildedCockTurbate.ASprinkleOf".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Masturbation.GildedCockTurbate.ARainOf".Localized());
                else
                    OutputText("Scenes.Masturbation.GildedCockTurbate.ATorrentOf".Localized());

            OutputText("Scenes.Masturbation.GildedCockTurbate.GemsFallOnBody".Localized());

            var gems = MidasCockJackingGemsRoll();

            OutputText("Scenes.Masturbation.GildedCockTurbate.GatherGemsThenSleep".Localized(
                NumberUtils.Num2Text(gems)
            ));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            Player.Gems += gems;

            CoC.Instance.Flags.Set(GameFlagIds.GildedJerked, CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) + 1);

            ReturnToCamp(1);
        }

        public void StickADildoInYourVagooSlut()
        {
            ClearOutput();

            if (Player.Vaginas.Count > 0 && Player.Vaginas[0].Virgin)
            {
                if (Player.Corruption <= 50)
                {
                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.BlushNervouslyGraspToy".Localized());

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.SittingRubsSlenderTube".Localized());

                    if (Player.Wetness() <= 2)
                        OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.ToySlidesEasierJuices".Localized());
                    else
                        OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.ToySlidesEffortlessly".Localized());

                    ShowImage("masti-dildo-vag");

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.LeanBackPositionDildo".Localized());

                    Player.CuntChange(8, true, true, false);

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.SlowlyPullFakePhallus".Localized());

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.RestBackOnGround".Localized());
                }
                else
                {
                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.BlushPerverselyGrasp".Localized());

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.PranceNudeToSpot".Localized());

                    ShowImage("masti-dildo-vag");

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.PhallusSlickWithSaliva".Localized());

                    Player.CuntChange(8, true, true, false);

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.RegainComposurePump".Localized());

                    OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.AfterglowCleanRedress".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.SitBehindRock".Localized());

                ShowImage("masti-dildo-vag");

                OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.SpreadLegsRubFingers".Localized());

                Player.CuntChange(8, true, true, false);

                OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.EroticPumpingQuickens".Localized());

                OutputText("Scenes.Masturbation.StickADildoInYourVagooSlut.OrgasmSupremeRelief".Localized());
            }

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1.5f));

            ReturnToCamp(1);
        }

        public void DildoButts()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.DildoButts.KinkyIdeaCrossesMind".Localized());

            OutputText("Scenes.Masturbation.DildoButts.AdjustSpreadLegs".Localized());

            if (Player.AnalCapacity() <= 8)
                OutputText("Scenes.Masturbation.DildoButts.TightHoleResistsFirst".Localized());
            else
                OutputText("Scenes.Masturbation.DildoButts.ExperiencedHoleAcceptsFinger".Localized());

            OutputText("Scenes.Masturbation.DildoButts.RubInsidesPinchNipples".Localized());

            ShowImage("masti-dildo-anal");

            OutputText("Scenes.Masturbation.DildoButts.LickToyThenInsert".Localized());

            if (silly)
                OutputText("Scenes.Masturbation.DildoButts.AlliterationArousesYou".Localized());

            OutputText("Scenes.Masturbation.DildoButts.ToesCurlSmoothPumping".Localized());

            Player.ButtChange(8, true, true, false);

            if (Player.HasCock() && Player.HasVagina())
                OutputText("Scenes.Masturbation.DildoButts.MoanEcstasyBothErupt".Localized());
            else
                if (Player.HasCock())
                    OutputText("Scenes.Masturbation.DildoButts.MoanEcstasyCocksCum".Localized());
                else
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Masturbation.DildoButts.MoanEcstasyJuices".Localized());

                        if (Player.Wetness() >= 4)
                            OutputText("Scenes.Masturbation.DildoButts.SquirtWord".Localized());
                        else
                            OutputText("Scenes.Masturbation.DildoButts.TrickleWord".Localized());

                        OutputText("Scenes.Masturbation.DildoButts.FromYourVagina".Localized());
                    }
                    else
                        OutputText("Scenes.Masturbation.DildoButts.ParagraphBreak".Localized());

            OutputText("Scenes.Masturbation.DildoButts.BodyShakesAnalOrgasm".Localized());

            OutputText("Scenes.Masturbation.DildoButts.GatherThingsReturnCamp".Localized());

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 0.5f));

            ReturnToCamp(1);
        }

        public int MidasCockJackingGemsRoll()
        {

            var cockSocks = Player.CountCockSocks("gilded");

            var gems = 10 + Random.Shared.Next(20);

            if (Player.CumQ() < 1000 * cockSocks)
                gems += (int)(Player.CumQ() / 10);
            else
                if (Player.CumQ() < 2500 * cockSocks)
                    gems += (int)(100 + Player.CumQ() / 50);
                else
                    if (Player.CumQ() < 5000 * cockSocks)
                        gems += (int)(150 + Player.CumQ() / 150);
                    else
                        if (Player.CumQ() < 10000 * cockSocks)
                            gems += (int)(200 + Player.CumQ() / 450);
                        else
                            gems += (int)(250 + Player.CumQ() / 1350);

            if (gems > 200 * cockSocks)
                gems = 200 * cockSocks + Random.Shared.Next(20);

            return gems;
        }

        public void OnaholeRepeatUse(bool corrupted)
        {

            var gemsCreated = 0;

            if (Player.CountCockSocks("gilded") > 0 && CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) < Player.CountCockSocks("gilded"))
            {
                CoC.Instance.Flags.Set(GameFlagIds.GildedJerked, CoC.Instance.Flags.Get(GameFlagIds.GildedJerked) + 1);

                gemsCreated = MidasCockJackingGemsRoll();

                Player.Gems += gemsCreated;
            }

            if (corrupted)
            {
                OutputText("Scenes.Masturbation.OnaholeRepeatUse.AmusedAnnoyedAchingLoins".Localized(
                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                if (gemsCreated > 0)
                    OutputText("Scenes.Masturbation.OnaholeRepeatUse.PuddleShimmersStrangely".Localized(
                        gemsCreated
                    ));
                else
                    OutputText("Scenes.Masturbation.OnaholeRepeatUse.LayBackRecuperate".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.OnaholeRepeatUse.AnnoyanceAndEmbarrassment".Localized(
                    Player.CockDescript(),

                    Player.CockDescript()
                ));

                if (gemsCreated > 0)
                {
                    OutputText("Scenes.Masturbation.OnaholeRepeatUse.PopToyHumpSky".Localized());

                    OutputText("Scenes.Masturbation.OnaholeRepeatUse.PantingHumpWatchJet".Localized());

                    if (Player.CumQ() < 25)
                        OutputText("Scenes.Masturbation.OnaholeRepeatUse.SprinkleWord".Localized());
                    else
                        if (Player.CumQ() < 250)
                            OutputText("Scenes.Masturbation.OnaholeRepeatUse.RainWord".Localized());
                        else
                            OutputText("Scenes.Masturbation.OnaholeRepeatUse.TorrentWord".Localized());

                    OutputText("Scenes.Masturbation.OnaholeRepeatUse.GemsInsteadOfCum".Localized());

                    OutputText("Scenes.Masturbation.OnaholeRepeatUse.ForceYourselfPickGems".Localized(
                        gemsCreated
                    ));
                }
                else
                    OutputText("Scenes.Masturbation.OnaholeRepeatUse.MultipleStreamsErupt".Localized());
            }
        }

        public void FingerFuck()
        {

            ShowImage("masti-genderless-fuck");

            OutputText("Scenes.Masturbation.FingerFuck.HoleTwitchesForAttention".Localized());

            OutputText("Scenes.Masturbation.FingerFuck.LieOnSideReach".Localized());

            if (Player.Anal.Wetness < 2)
            {
                OutputText("Scenes.Masturbation.FingerFuck.HandUpToMouth".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.FingerFuck.MiddleFinger".Localized());
                else
                    if (Player.Anal.Looseness == 3)
                        OutputText("Scenes.Masturbation.FingerFuck.FirstTwoFingers".Localized());
                    else
                        OutputText("Scenes.Masturbation.FingerFuck.HandWord".Localized());

                OutputText("Scenes.Masturbation.FingerFuck.GenerousHelpingSaliva".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.FingerFuck.LubricationAllowsEasy".Localized());

                if (Player.Anal.Looseness >= 3)
                    OutputText("Scenes.Masturbation.FingerFuck.SLetter".Localized());

                OutputText("Scenes.Masturbation.FingerFuck.ShiverRunsUpSpine".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.FingerFuck.AllWayToKnuckle".Localized());
                else
                    if (Player.Anal.Looseness == 3)
                        OutputText("Scenes.Masturbation.FingerFuck.ToFirstTwoKnuckles".Localized());
                    else
                        if (Player.Anal.Looseness == 4)
                            OutputText("Scenes.Masturbation.FingerFuck.YourThreeKnuckles".Localized());
                        else
                            OutputText("Scenes.Masturbation.FingerFuck.FourFingersInDeep".Localized());

                OutputText("Scenes.Masturbation.FingerFuck.SlowlyPushAndPull".Localized());

                if (Player.Anal.Looseness >= 3)
                    OutputText("Scenes.Masturbation.FingerFuck.SLetterAgain".Localized());

                OutputText("Scenes.Masturbation.FingerFuck.PumpingAnusMotion".Localized());
            }

            if (Player.Anal.Looseness == 5)
                OutputText("Scenes.Masturbation.FingerFuck.DevilishThoughtCrossesMind".Localized());

            OutputText("Scenes.Masturbation.FingerFuck.PleasureFillsWithWarmth".Localized());

            if (Player.Anal.Looseness <= 2)
                OutputText("Scenes.Masturbation.FingerFuck.DigitSingular".Localized());
            else
                if (Player.Anal.Looseness < 5)
                    OutputText("Scenes.Masturbation.FingerFuck.DigitPlural".Localized());
                else
                    OutputText("Scenes.Masturbation.FingerFuck.HandWordAgain".Localized());

            OutputText("Scenes.Masturbation.FingerFuck.ToesCurlOrgasmWracks".Localized());

            if (Player.Anal.Wetness >= 2)
            {
                OutputText("Scenes.Masturbation.FingerFuck.WithdrawYourPrefix".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.FingerFuck.FingerSingular".Localized());
                else
                    if (Player.Anal.Looseness < 5)
                        OutputText("Scenes.Masturbation.FingerFuck.FingerPlural".Localized());
                    else
                        OutputText("Scenes.Masturbation.FingerFuck.HandWordThird".Localized());

                OutputText("Scenes.Masturbation.FingerFuck.CoatedInWarmLube".Localized());

                if (Player.LibidoPercent >= 75)
                    OutputText("Scenes.Masturbation.FingerFuck.RollOverSleepDripping".Localized());
                else
                    OutputText("Scenes.Masturbation.FingerFuck.RollOverCompletelySpent".Localized());
            }
            else
            {
                OutputText("Scenes.Masturbation.FingerFuck.WithdrawYourPrefixAgain".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.FingerFuck.FingerSingularAgain".Localized());
                else
                    if (Player.Anal.Looseness < 5)
                        OutputText("Scenes.Masturbation.FingerFuck.FingerPluralAgain".Localized());
                    else
                        OutputText("Scenes.Masturbation.FingerFuck.HandWordFourth".Localized());

                OutputText("Scenes.Masturbation.FingerFuck.AndDry".Localized());

                if (Player.Anal.Looseness <= 2)
                    OutputText("Scenes.Masturbation.FingerFuck.PronounIt".Localized());
                else
                    if (Player.Anal.Looseness < 5)
                        OutputText("Scenes.Masturbation.FingerFuck.PronounThey".Localized());
                    else
                        OutputText("Scenes.Masturbation.FingerFuck.PronounItAgain".Localized());

                OutputText("Scenes.Masturbation.FingerFuck.OffWord".Localized());

                if (Player.LibidoPercent > 75)
                    OutputText("Scenes.Masturbation.FingerFuck.SatisfiedSleepWarmHole".Localized());
                else
                    OutputText("Scenes.Masturbation.FingerFuck.SatisfiedSleepSpent".Localized());
            }

            Player.ButtChange(4 + Player.Anal.Looseness * 2, true);

            Player.Orgasm("Anal");

            UpdateMasturbation();
        }

        public void NagaTailsturbation()
        {
            ClearOutput();

            ShowImage("masti-naga-tailsex");

            OutputText("Scenes.Masturbation.NagaTailsturbation.PerverseGrinDecide".Localized());

            OutputText("Scenes.Masturbation.NagaTailsturbation.SlitherToSafeSpot".Localized());

            Player.CuntChange(Player.Tallness + 12, true);

            OutputText("Scenes.Masturbation.NagaTailsturbation.PositionAwkwardAtFirst".Localized());

            OutputText("Scenes.Masturbation.NagaTailsturbation.BriefNapCleanUp".Localized());

            UpdateMasturbation();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -0.5f));

            ReturnToCamp(1);
        }

        public void FistForFullFeelings()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.FistForFullFeelings.NothingFancyNeeded".Localized());

            if (Player.Anal.Looseness < 3)
                OutputText("Scenes.Masturbation.FistForFullFeelings.DespiteTightness".Localized());
            else
                OutputText("Scenes.Masturbation.FistForFullFeelings.VeryUsedToIt".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.StripArmorOntoBed".Localized());

            if (Player.Libido < 50)
                OutputText("Scenes.Masturbation.FistForFullFeelings.PantingAnticipationFondle".Localized());
            else
                OutputText("Scenes.Masturbation.FistForFullFeelings.ShiveringWithNeed".Localized());

            if (Player.Anal.Wetness >= 3)
                OutputText("Scenes.Masturbation.FistForFullFeelings.NaturalLubricationEnough".Localized());
            else
                OutputText("Scenes.Masturbation.FistForFullFeelings.CoverHandWithPrecum".Localized(
                    Player.MultiCockDescript()
                ));

            OutputText("Scenes.Masturbation.FistForFullFeelings.TeaseHoleFingerDeeper".Localized());

            if (Player.Sensitivity < 33)
                OutputText("Scenes.Masturbation.FistForFullFeelings.HeartRacesBreathQuickens".Localized());

            if (Player.Sensitivity >= 33 && Player.Sensitivity < 66)
                OutputText("Scenes.Masturbation.FistForFullFeelings.MoanAsHeartRaces".Localized());

            if (Player.Sensitivity >= 66)
                OutputText("Scenes.Masturbation.FistForFullFeelings.SquirmMoanAndSweat".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.TeasingDeeperParasiteFaster".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.StartedOnlyMomentsAgo".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Masturbation.FistForFullFeelings.PrecumPuddleForms".Localized(
                    Player.MultiCockDescript()
                ));

            if (Player.CumQ() < 250 && Player.CumQ() > 50)
                OutputText("Scenes.Masturbation.FistForFullFeelings.LegsCoveredStickyPre".Localized());

            if (Player.CumQ() >= 250)
                OutputText("Scenes.Masturbation.FistForFullFeelings.AbsolutelyCoveredPrecum".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.FistLodgedInAss".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.MoveFistTowardProstate".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.PointOfNoReturn".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.OrgasmsArrestBody".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.FistExitsWithPop".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.TwistTurnWildly".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Masturbation.FistForFullFeelings.DribbleSemenOverSelf".Localized());

            if (Player.CumQ() < 250 && Player.CumQ() > 50)
                OutputText("Scenes.Masturbation.FistForFullFeelings.LeakPuddlesOfSemen".Localized());

            if (Player.CumQ() >= 250)
                OutputText("Scenes.Masturbation.FistForFullFeelings.CoverBedInSemen".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.NoThoughtToMess".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.OrgasmWavesSubside".Localized());

            OutputText("Scenes.Masturbation.FistForFullFeelings.WakeMinutesLater".Localized());

            Player.Orgasm("Anal");

            UpdateMasturbation();

            ReturnToCamp(1);
        }

        public void NephilaHunt()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.NephilaHunt.WanderIntoWoods".Localized());

            OutputText("Scenes.Masturbation.NephilaHunt.CryOutToChildren".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Masturbation.NephilaHunt.TeasingDeeperSlimesFaster".Localized(
                Player.VaginaDescript(0)
            ));

            OutputText("Scenes.Masturbation.NephilaHunt.FistLodgedInPussy".Localized());

            OutputText("Scenes.Masturbation.NephilaHunt.DeerInterruptsMasturbation".Localized());

            OutputText("Scenes.Masturbation.NephilaHunt.SayPrettyGirl".Localized());

            OutputText("Scenes.Masturbation.NephilaHunt.BeastLooksUpFreezes".Localized());

            OutputText("Scenes.Masturbation.NephilaHunt.IrisesOpenSlimeStream".Localized());

            OutputText("Scenes.Masturbation.NephilaHunt.GiggleSpreadLabia".Localized());

            OutputText("Scenes.Masturbation.NephilaHunt.SatisfiedReturnToCamp".Localized());

            Player.Orgasm("Vaginal");

            UpdateMasturbation();

            ReturnToCamp(1);
        }

        public void DrinkPiss()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.DrinkPiss.MasturbationThoughtArouses".Localized());

            OutputText("Scenes.Masturbation.DrinkPiss.GripCockBothHands".Localized());

            OutputText("Scenes.Masturbation.DrinkPiss.PeeStreamOnFace".Localized());

            OutputText("Scenes.Masturbation.DrinkPiss.ArchBackGruntingCum".Localized());

            Player.Orgasm("Dick");

            UpdateMasturbation();

            ReturnToCamp(1);
        }

        public void HungerRub()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.HungerRub.NeedGnawsAtYou".Localized());

            OutputText("Scenes.Masturbation.HungerRub.WoozilySlipToCabin".Localized());

            OutputText("Scenes.Masturbation.HungerRub.PangsInAbdomen".Localized());

            OutputText("Scenes.Masturbation.HungerRub.TentativeHandReachesDown".Localized());

            OutputText("Scenes.Masturbation.HungerRub.NoMatterHowHard".Localized());

            OutputText("Scenes.Masturbation.HungerRub.DeliriumTakesHold".Localized());

            OutputText("Scenes.Masturbation.HungerRub.HoarseCryClimaxes".Localized());

            Player.Orgasm(Player.HasCock() ? "Dick" : "Vaginal");

            UpdateMasturbation();

            ReturnToCamp(1);
        }

        public void FuckBear()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.FuckBear.SettleOntoBed".Localized());

            OutputText("Scenes.Masturbation.FuckBear.MarvelAtFluffyBear".Localized());

            OutputText("Scenes.Masturbation.FuckBear.WantMoreThanCuddles".Localized());

            OutputText("Scenes.Masturbation.FuckBear.PullBackToLook".Localized());

            OutputText("Scenes.Masturbation.FuckBear.SomethingNeedsDoingFirst".Localized());

            OutputText("Scenes.Masturbation.FuckBear.HesitanceGoneThrust".Localized());

            OutputText("Scenes.Masturbation.FuckBear.BearOnlyStares".Localized());

            OutputText("Scenes.Masturbation.FuckBear.FluffRubsAgainstSkin".Localized());

            OutputText("Scenes.Masturbation.FuckBear.UnloadOverBear".Localized());

            OutputText("Scenes.Masturbation.FuckBear.OneFinalRelease".Localized());

            Player.Orgasm("Generic");

            ClearButtons();

            AddNextButton("Common.Button.FixIt".Localized(), FuckBearFix);

            AddNextButton("Common.Button.LeaveIt".Localized(), FuckBearLeave);
        }

        public void FuckBearFix()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.FuckBearFix.BearDeservesProperCare".Localized());

            OutputText("Scenes.Masturbation.FuckBearFix.WorthItWhenFixed".Localized());

            ReturnToCamp(1);
        }

        public void FuckBearLeave()
        {
            ClearOutput();

            OutputText("Scenes.Masturbation.FuckBearLeave.CannotServeAsBear".Localized());

            if (Player.HasItem(UseableIds.TeddyBear))
                Player.ConsumeItem(UseableIds.TeddyBear);
            else
                CoC.Instance.Flags.Set(GameFlagIds.BedBears, CoC.Instance.Flags.Get(GameFlagIds.BedBears) - 1);

            ReturnToCamp(1);
        }

    }
}