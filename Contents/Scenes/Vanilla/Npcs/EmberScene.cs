using MonoCoC.Contents.World;
using System.Collections.Generic;
using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Configuration;
using System;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Contents.Internals;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<EmberScene>("coc:ember")]
    public class EmberScene : Scene
    {

        public int parasiteRating = 0;

        public static double Floor(double value) => System.Math.Floor(value);

        public static bool debug => OptionStore.Instance.Get("debug", 0) > 0;

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Ember);

        public string debugName => "Ember";

        public string debugHint => "";

        public override void Entry()
        {

        }

        public void Reset()
        {

            Save.SetBool("flowerExplained", false);

            Save.SetInt("newbornGender", 0);

            Save.SetInt("birthTime", 0);

            Save.SetBool("learnedFeeding", false);

            Save.SetIntList("eggArray", new List<int>());

            Save.SetInt("hatchedToday", 0);

            Save.SetInt("tuckedToday", 0);
        }

        public int EmberAffection(int changes = 0)
        {

            CoC.Instance.Flags.Set(GameFlagIds.EmberAffection, CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) + changes);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.EmberAffection, 100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberAffection, 0);

            return CoC.Instance.Flags.Get(GameFlagIds.EmberAffection);
        }

        public int EmberCorruption(int changes = 0)
        {

            CoC.Instance.Flags.Set(GameFlagIds.EmberCor, CoC.Instance.Flags.Get(GameFlagIds.EmberCor) + changes);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberCor) > 100)
                CoC.Instance.Flags.Set(GameFlagIds.EmberCor, 100);
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberCor) < 0)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberCor, 0);

            return CoC.Instance.Flags.Get(GameFlagIds.EmberCor);
        }

        public void EmberReactsToLustiness()
        {
            ClearOutput();

            OutputText("The dragon " + (LittleEmber() ? "Common.Text.GivesACuteWhimper".Localized() : "Common.Text.Moans".Localized()) + ", weaving softly from side to side, eyes glazed at the intimate prospect of sex... but then, to your surprise, Ember visibly shakes it off and recomposes herself, frowning at you.");

            OutputText("Scenes.Ember.EmberReactsToLustiness.NotOrdinaryMonster".Localized());

            OutputText("Common.Text.PgDespiteEmbersInitialDisplayYou".Localized());

            if (Player.Libido >= 50)
                OutputText("Scenes.Ember.EmberReactsToLustiness.HowBoring".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public int EmberSparIntensity()
        {

            var amount = 0;

            amount += (int)Floor(CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) / 5);

            amount += CoC.Instance.Flags.Get(GameFlagIds.EmberSparVictories);

            if (amount > 100)
                amount = 100;

            return amount;
        }

        public bool FollowerEmber()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) > 0;
        }

        public string EmberMF(string man, string woman)
        {

            if (EmberIsMasculine())
                return man;

            return woman;
        }

        public string EmberGroinDesc(string cock, string pussy, string herm = " and ")
        {

            var strText = "";

            if (EmberHasCock())
                strText += cock;

            if (EmberIsHerm())
                strText += herm;

            if (EmberHasVagina())
                strText += pussy;

            return strText;
        }

        public int EmberVaginalCapacity()
        {

            return LittleEmber() ? 15 : 60;
        }

        public int EmberAnalCapacity()
        {

            return LittleEmber() ? 15 : 60;
        }

        public bool EmberHasCock()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male || CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite;
        }

        public bool EmberHasVagina()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female || CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite;
        }

        public bool EmberIsFeminine()
        {

            return EmberHasVagina();
        }

        public bool EmberIsMasculine()
        {

            return !EmberIsFeminine();
        }

        public bool EmberIsHerm()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite;
        }

        public bool LittleEmber()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberAge) == 1;
        }

        public int EmberChildren()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenMales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms);
        }

        public bool EmberInternalDick()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0;
        }

        public bool EmberHasHair()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.EmberHair) != 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) != 0;
        }

        public void EmberSprite()
        {

        }

        public void EmberCampMenu(bool output = true)
        {
            ClearOutput();

            if (output)
            {
                if ((View.Time.Days - Save.GetInt("birthTime", 0)) < 14 && !(LittleEmber() && !Save.GetBool("learnedFeeding")) && (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) != (int)Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) == 0) && Random.Shared.Next(3) == 0)
                {
                    EmberBreastfeeding();

                    return ;
                }

                ShowImage("ember-visit-at-camp");

                if (EmberAffection() <= 25)
                    OutputText("Scenes.Emberscene.EmberCampMenu.SighsWhatWant".Localized());
                else
                    if (EmberAffection() <= 75)
                        OutputText("Scenes.Emberscene.EmberCampMenu.FidgetsTailSways".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberCampMenu.EyesLightUpNervous".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.Appearance".Localized(), EmbersAppearance);

            AddButton(1, "Common.Button.Talk".Localized(), TalkToEmber);

            AddButton(2, "Common.Button.Sex".Localized(), () => EmberSexMenu());

            AddButton(3, "Common.Button.Spar".Localized(), DecideToSparEmbra);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) + EmberChildren() > 0)
                AddButton(5, "Common.Button.Children".Localized(), EmberKidsMenu);

            AddRowButton(1, "Drink Blood", BloodForTheBloodGod);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || (Player.HasItem(ItemRegistry.Create(ConsumableIds.Lactaid), 1) && CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) >= 75))
                AddRowButton(1, "Drink Milk", GetMilkFromEmber);

            if ((CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0 || (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir), 1) && CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) >= 75)) && EmberHasVagina() && !Player.IsPregnant())
                AddRowButton(1, "Get Egg", EmberIsAnEggFactory);

            if (View.Time.Hours >= 21 || View.Time.Hours < 5)
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) < 75)
                    AddButton(9, "Common.Button.SleepWith".Localized(), SleepWithEmber);
                else
                    AddButton(9, "Common.Button.SleepWith".Localized(), SleepWithEmber);

            if (EmberAffection() > 0 && Player.HasItem(ItemRegistry.Create(ConsumableIds.DrakeHeart), 1) && CoC.Instance.Flags.Get(GameFlagIds.GiftedFlower) == 0)
                AddButton(10, "Common.Button.GiftFlower".Localized(), GiftDrakeFlower);

            AddButton(14, "Common.Button.Back".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
        }

        public void EmberSexMenu(bool output = true)
        {
            ClearOutput();

            if (output)
            {
                OutputText("Scenes.Emberscene.EmberSexMenu.OglesBodyNuances".Localized(EmberMF("his", "her"), (LittleEmber() ? " preteen" : "")));

                if (EmberAffection() <= 25)
                    OutputText("Scenes.Emberscene.EmberSexMenu.AsksWhyLooking".Localized());
                else
                    if (EmberAffection() < 75)
                        OutputText("Scenes.Emberscene.EmberSexMenu.AsksSelfBodyWrong".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberSexMenu.ProtestsBitingLip".Localized());

                OutputText("Scenes.Emberscene.EmberSexMenu.AdmiresDragonShape".Localized());

                if (EmberAffection() <= 25)
                    OutputText("Scenes.Emberscene.EmberSexMenu.FlatteryNoPoints".Localized((LittleEmber() ? ", you pedophile" : "")));
                else
                    if (EmberAffection() < 75)
                        OutputText("Scenes.Emberscene.EmberSexMenu.DontBuyIt".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberSexMenu.StopMakingMe".Localized((LittleEmber() ? "" : " a mixture of arousal and")));

                if (EmberHasCock())
                {
                    OutputText("Scenes.Emberscene.EmberSexMenu.PlaceholderCockSpace".Localized(
                        EmberMF("His", "Her")
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1 || CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                        OutputText("Scenes.Emberscene.EmberSexMenu.PokingOutSlit".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberSexMenu.SwellingWithBlood".Localized());

                    OutputText("Scenes.Emberscene.EmberSexMenu.PeriodSpace".Localized());
                }

                if (EmberHasVagina())
                    OutputText("Scenes.Emberscene.EmberSexMenu.RivuletsInnerThighs".Localized());

                OutputText("Scenes.Emberscene.EmberSexMenu.SexyBeastAsk".Localized());

                if (EmberAffection() <= 25)
                    OutputText("Scenes.Emberscene.EmberSexMenu.ChucklesFailedJustify".Localized());
                else
                    if (EmberAffection() < 75)
                        OutputText("Scenes.Emberscene.EmberSexMenu.OffersHelp".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberSexMenu.DependsWhatInMind".Localized());
            }

            ClearButtons();

            AddButton(0, "Common.Button.CatchAnal".Localized(), null);

            AddButton(1, "Common.Button.PitchAnal".Localized(), null);

            AddButton(2, "Common.Button.BlowEmber".Localized(), null);

            AddButton(3, "Common.Button.GetBlown".Localized(), null);

            AddButton(4, "Eat " + EmberMF("Him", "Her") + "Common.Text.Out".Localized(), null);

            AddButton(5, "Common.Button.GetEatenOut".Localized(), null);

            AddButton(6, "Common.Text.Penetrate".Localized() + EmberMF("Him", "Her"), null);

            AddButton(7, "Common.Button.GetPenetrated".Localized(), null);

            AddButton(8, "Common.Button.Lustyfuck".Localized(), null);

            if (EmberHasCock())
                AddButton(0, "Common.Button.CatchAnal".Localized(), CatchAnal);

            if (Player.HasCock() && Player.Lust >= 33)
                AddButton(1, "Common.Button.PitchAnal".Localized(), StickItInEmbersButt);

            if (EmberHasCock())
                AddButton(2, "Common.Button.BlowEmber".Localized(), SuckEmberCock);

            if (Player.HasCock() && Player.Lust >= 33)
                AddButton(3, "Common.Button.GetBlown".Localized(), StickDickInKnifeDrawer);

            if (EmberHasVagina())
                AddButton(4, "Common.Button.EatHerOut".Localized(), SlurpDraggieCunnies);

            if (Player.HasVagina() && Player.Lust >= 33)
                AddButton(5, "Common.Button.GetEatenOut".Localized(), GetEatenOutByEmbra);

            if (EmberHasVagina() && Player.HasCock() && Player.Lust >= 33)
                AddButton(6, "Common.Text.Penetrate".Localized() + EmberMF("Him", "Her"), PenetrateEmbrah);

            if (EmberHasCock() && Player.Lust >= 33 && Player.HasVagina())
                AddButton(7, "Common.Button.GetPenetrated".Localized(), GetPenetratedByEmberLastSexSceneWoooo);

            if (EmberAffection() >= 95 && Player.HasCockThatFits(EmberVaginalCapacity()) && (Player.HasItem(ItemRegistry.Create(ConsumableIds.LustDraft)) || Player.Libido >= 50 || Player.Libido >= 40))
                AddButton(8, "Common.Button.Lustyfuck".Localized(), HighAffectionEmberLustFuck);

            AddNextButton("Scenes.Kitsune.DefeatTheKitsunes.ButtonTribbing".Localized(), EmberTribbing);

            AddButton(14, "Common.Button.Back".Localized(), () => EmberCampMenu());
        }

        public void FindEmbersEgg()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFoundEmbersEgg) == 0)
            {
                OutputText("Scenes.Emberscene.FindEmbersEgg.SpotsCaveEntrance".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.CaveFloorDamp".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.FortunatelyNotInjured".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.PressOnChamber".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.DearReaderLetter".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.FellowDragonWard".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.DragonsExist".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.ObliteratedStrangeMagic".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.MultiplyNumbers".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.SmallGroupEscaped".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.FollowingLetterNotes".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.NotesOnHatching".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.ShouldTakeEgg".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.FindEmbersEgg.FamiliarCaveConfirm".Localized());

                OutputText("Scenes.Emberscene.FindEmbersEgg.OnceAgainChamber".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesFoundEmbersEgg, CoC.Instance.Flags.Get(GameFlagIds.TimesFoundEmbersEgg) + 1);

            ClearButtons();

            AddButton(0, "Scenes.Kitsune.StealAStatue.ButtonTakeIt".Localized(), TakeEmbersEggHomeInADoggieBag);

            AddButton(1, "Common.Button.DestroyIt".Localized(), DestroyBabyEmberYouMonster);

            AddButton(14, "Common.Button.Leave".Localized(), LeaveEmbersAssOutToDry);
        }

        public void LeaveEmbersAssOutToDry()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.LeaveEmbersAssOutToDry.LeaveEggUndecided".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void DestroyBabyEmberYouMonster()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.DestroyBabyEmberYouMonster.RainBlowsOnEgg".Localized());

            OutputText("Scenes.Emberscene.DestroyBabyEmberYouMonster.SealedSpeciesFate".Localized());

            OutputText("Scenes.Emberscene.DestroyBabyEmberYouMonster.StoppedByThought".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EggBroken, 1);

            ClearButtons();

            AddButton(0, "Areas.Forest.FruitEat".Localized(), EatEmbersYolkLikeAnEvenBiggerDick);

            SetExitButton("Common.Button.Back".Localized());
        }

        public void EatEmbersYolkLikeAnEvenBiggerDick()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EatEmbersYolkLikeAnEvenBiggerDick.ImpulseEatYolk".Localized());

            OutputText("Scenes.Emberscene.EatEmbersYolkLikeAnEvenBiggerDick.TasteIncredible".Localized());

            Player.RefillHunger(100);

            OutputText("Scenes.Emberscene.EatEmbersYolkLikeAnEvenBiggerDick.AteAllPossible".Localized());

            OutputText("Scenes.Emberscene.EatEmbersYolkLikeAnEvenBiggerDick.SatedTakeShell".Localized());

            OutputText("Scenes.Emberscene.EatEmbersYolkLikeAnEvenBiggerDick.GainedDragonEggshell".Localized());

            Player.CreateKeyItem("Dragon Eggshell", 0, 0, 0, 0);

            Player.ApplyStats(new DynStat(GameStats.Strength, 5 + Random.Shared.Next(5)), new DynStat(GameStats.Toughness, 5 + Random.Shared.Next(5)), new DynStat(GameStats.Intelligence, 5 + Random.Shared.Next(5)), new DynStat(GameStats.Corruption, 20f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void GetSomeStuff()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.GetSomeStuff.ShowsShellArmorsmith".Localized());

            OutputText("Scenes.Emberscene.GetSomeStuff.EggsNoGoodArmor".Localized());

            OutputText("Scenes.Emberscene.GetSomeStuff.TurnsShellInFire".Localized());

            OutputText("Scenes.Emberscene.GetSomeStuff.ShieldFine".Localized());

            OutputText("Scenes.Emberscene.GetSomeStuff.GemShieldMade".Localized());

            Player.Gems -= 200;

            Player.RemoveKeyItem("Dragon Eggshell");

            InventoryScene.TakeItem(ItemIds.DragonShield, () => Go<Places.TelAdre.YvonneArmorShop>());
        }

        public void TakeEmbersEggHomeInADoggieBag()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.DecidesTakeEgg".Localized());

            if (Player.Tallness < 84)
                OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.SurprisinglyLight".Localized());
            else
                OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.CarryEasy".Localized());

            OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.BackCorridorBlocked".Localized());

            OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.IntactRollPast".Localized());

            OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.LooksForWayOut".Localized());

            if (Player.Intelligence >= 50)
            {
                OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.ElectricityRunSpine".Localized());

                OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.PulseIntoCorridor".Localized());
            }
            else
                if (Player.Strength >= 90)
                {
                    OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.LooksOverAndOver".Localized());

                    OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.MakeWayStrength".Localized());

                    OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.LongerThanAnticipated".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.LooksOverAgain".Localized());

                    OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.LeaveEggForNow".Localized());

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

            OutputText("Scenes.Emberscene.TakeEmbersEggHomeInADoggieBag.QuestBegun".Localized());

            Player.CreateKeyItem("Dragon Egg", 0, 0, 0, 0);

            CoC.Instance.Flags.Set(GameFlagIds.TookEmberEgg, 1);

            CoC.Instance.Flags.Set(GameFlagIds.EmberCor, 50);

            DoNext(() => ReturnToCamp(1));
        }

        public void EmberCampDesc()
        {

            OutputText("Scenes.Emberscene.EmberCampDesc.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) == 0)
                OutputText("Scenes.Emberscene.EmberCampDesc.EggInNest".Localized());
            else
                if (CampSleep.IsForceSleepTime())
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Ember")
                        OutputText("Scenes.Emberscene.EmberCampDesc.HoversAwaitingEntry".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberCampDesc.EmberIsTuckedSnugly".Localized(
                            RandomUtils.Choice(["Ember is tucked snugly on [ember eir] bedding in [ember eir] cave, wings folded over [ember em] as [ember ey] lays on all fours.", "Viewing from afar, you see Ember is asleep within [ember eir] cave."])
                        ));
                }
                else
                {
                    float choice = Random.Shared.Next(3);

                    if (choice == 0)
                        OutputText("Scenes.Emberscene.EmberCampDesc.LiesInFrontDen".Localized());
                    else
                        if (choice == 1)
                            OutputText("Scenes.Emberscene.EmberCampDesc.NotAroundDen".Localized());
                        else
                            OutputText("Scenes.Emberscene.EmberCampDesc.FightingSleep".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.GiftedFlower) == 1)
                        OutputText("Scenes.Emberscene.EmberCampDesc.DrakeHeartPlaced".Localized());
                }

            OutputText("Scenes.Emberscene.EmberCampDesc.ParagraphBreakAgain".Localized());
        }

        public void EmberEggInteraction()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberEggInteraction.ApproachEggPulsing".Localized());

            OutputText("Scenes.Emberscene.EmberEggInteraction.PulsesArrogance".Localized());

            if (Player.Lust >= 33 && (CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) < 5 || CoC.Instance.Flags.Get(GameFlagIds.EmberJackedOn) == 0))
            {
                OutputText("Scenes.Emberscene.EmberEggInteraction.PulsationsPattern".Localized());

                OutputText("Scenes.Emberscene.EmberEggInteraction.ImpressionPanting".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) == 4)
                    OutputText("Scenes.Emberscene.EmberEggInteraction.ExasperationUnfinished".Localized());

                AddButton(0, "Common.Button.Yes".Localized(), MasturbateOntoAnEgg);

                AddButton(1, "Common.Button.No".Localized(), DontEggFap);

                return ;
            }
            else
                OutputText("Scenes.Emberscene.EmberEggInteraction.PulsationsRhythmShifts".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.EmberJackedOn) > 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) > 0)
            {
                OutputText("Scenes.Emberscene.EmberEggInteraction.EggReadyHatch".Localized());

                AddButton(0, "Common.Button.Hatch".Localized(), HatchZeMuzzles);
            }
            else
                if (Player.Lust >= 33 && !Player.IsGenderless())
                    AddButton(0, "Common.Button.Masturbate".Localized(), MasturbateOntoAnEgg);
                else
                    AddButton(0, "Common.Button.Masturbate".Localized(), null);

            AddButton(1, "Common.Button.Blood".Localized(), GiveEmberBludSausages);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft)))
                AddButton(2, "Common.Button.Incubidraft".Localized(), () => UseIncubusDraftOnEmber(false));
            else
                AddButton(2, "Common.Button.Incubidraft".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                AddButton(3, "Common.Button.PureDraft".Localized(), () => UseIncubusDraftOnEmber(true));
            else
                AddButton(3, "Common.Button.PureDraft".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                AddButton(4, "Items.SuccubiMilk.Short".Localized(), () => UseSuccubiMilkOnEmber(false));
            else
                AddButton(4, "Items.SuccubiMilk.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)))
                AddButton(5, "Common.Button.PureMilk".Localized(), () => UseSuccubiMilkOnEmber(true));
            else
                AddButton(5, "Common.Button.PureMilk".Localized(), null);

            if (Player.HasItem(ConsumableIds.HairSerum))
                AddButton(6, "Items.HairSerum.Short".Localized(), HairExtensionSerum);
            else
                AddButton(6, "Items.HairSerum.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)))
                AddButton(7, "Items.OviElixir.Short".Localized(), UseOviElixerOnEmber);
            else
                AddButton(7, "Items.OviElixir.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Lactaid)))
                AddButton(8, "Items.Lactaid.Short".Localized(), UseLactaidOnEmber);
            else
                AddButton(8, "Items.Lactaid.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Lollipop)))
                AddButton(9, "Items.Lolipop.Short".Localized(), UseLolipopOnEmber);
            else
                AddButton(9, "Items.Lolipop.Short".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), LeaveWithoutUsingAnEmberItem);
        }

        public void DontEggFap()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.DontEggFap.StepsAwayImpulses".Localized());

            OutputText("Scenes.Emberscene.DontEggFap.RhythmExcited".Localized());

            OutputText("Scenes.Emberscene.DontEggFap.ItemsNoReaction".Localized());

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) >= 5 && CoC.Instance.Flags.Get(GameFlagIds.EmberJackedOn) > 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) > 0)
            {
                OutputText("Scenes.Emberscene.DontEggFap.EggReadyHatch".Localized());

                AddButton(0, "Common.Button.Hatch".Localized(), HatchZeMuzzles);
            }
            else
                if (Player.Lust >= 33 && !Player.IsGenderless())
                    AddButton(0, "Common.Button.Masturbate".Localized(), MasturbateOntoAnEgg);
                else
                    AddButton(0, "Common.Button.Masturbate".Localized(), null);

            AddButton(1, "Common.Button.Blood".Localized(), GiveEmberBludSausages);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.IncubiDraft)))
                AddButton(2, "Common.Button.Incubidraft".Localized(), () => UseIncubusDraftOnEmber(false));
            else
                AddButton(2, "Common.Button.Incubidraft".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft)))
                AddButton(3, "Common.Button.PureDraft".Localized(), () => UseIncubusDraftOnEmber(true));
            else
                AddButton(3, "Common.Button.PureDraft".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk)))
                AddButton(4, "Items.SuccubiMilk.Short".Localized(), () => UseSuccubiMilkOnEmber(false));
            else
                AddButton(4, "Items.SuccubiMilk.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk)))
                AddButton(5, "Common.Button.PureMilk".Localized(), () => UseSuccubiMilkOnEmber(true));
            else
                AddButton(5, "Common.Button.PureMilk".Localized(), null);

            if (Player.HasItem(ConsumableIds.HairSerum))
                AddButton(6, "Items.HairSerum.Short".Localized(), HairExtensionSerum);
            else
                AddButton(6, "Items.HairSerum.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.OviElixir)))
                AddButton(7, "Items.OviElixir.Short".Localized(), UseOviElixerOnEmber);
            else
                AddButton(7, "Items.OviElixir.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Lactaid)))
                AddButton(8, "Items.Lactaid.Short".Localized(), UseLactaidOnEmber);
            else
                AddButton(8, "Items.Lactaid.Short".Localized(), null);

            if (Player.HasItem(ItemRegistry.Create(ConsumableIds.Lollipop)))
                AddButton(9, "Items.Lolipop.Short".Localized(), UseLolipopOnEmber);
            else
                AddButton(9, "Items.Lolipop.Short".Localized(), null);

            AddButton(14, "Common.Button.Back".Localized(), LeaveWithoutUsingAnEmberItem);
        }

        public void LeaveWithoutUsingAnEmberItem()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.LeaveWithoutUsingAnEmberItem.ShakeHeadDontTamper".Localized());

            DoNext(PlayerMenu);
        }

        public void UseIncubusDraftOnEmber(bool purified = false)
        {
            ClearOutput();

            if (purified)
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedIncubiDraft));

                EmberCorruption(-10);
            }
            else
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.IncubiDraft));

                EmberCorruption(10);
            }

            OutputText("Scenes.Emberscene.UseIncubusDraftOnEmber.DrizzleIncubusDraft".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite || CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Genderless)
            {
                OutputText("Scenes.Emberscene.UseIncubusDraftOnEmber.ShellPastelBlue".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.EmberGender, (int)Gender.Male);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female)
                {
                    OutputText("Scenes.Emberscene.UseIncubusDraftOnEmber.ShellLavender".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.EmberGender, (int)Gender.Hermaphrodite);
                }

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(PlayerMenu);
        }

        public void UseSuccubiMilkOnEmber(bool purified = false)
        {
            ClearOutput();

            if (purified)
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.PurifiedSuccubiMilk));

                EmberCorruption(-10);
            }
            else
            {
                Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.SuccubiMilk));

                EmberCorruption(10);
            }

            OutputText("Scenes.Emberscene.UseSuccubiMilkOnEmber.PourSuccubiMilk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite || CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Genderless)
            {
                OutputText("Scenes.Emberscene.UseSuccubiMilkOnEmber.ShellMutedPink".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.EmberGender, (int)Gender.Female);
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                {
                    OutputText("Scenes.Emberscene.UseSuccubiMilkOnEmber.ShellLavender".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.EmberGender, (int)Gender.Hermaphrodite);
                }

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(PlayerMenu);
        }

        public void UseOviElixerOnEmber()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.OviElixir));

            OutputText("Scenes.Emberscene.UseOviElixerOnEmber.PourGreenElixer".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberOviposition, 1);

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(PlayerMenu);
        }

        public void UseLactaidOnEmber()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lactaid));

            OutputText("Scenes.Emberscene.UseLactaidOnEmber.PourCreamyLactaid".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberMilk, 1);

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(PlayerMenu);
        }

        public void UseLolipopOnEmber()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.Lollipop));

            OutputText("Scenes.Emberscene.UseLolipopOnEmber.GrindCandyPaste".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberAge, 1);

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(PlayerMenu);
        }

        public void HairExtensionSerum()
        {
            ClearOutput();

            Player.ConsumeItem(ConsumableIds.HairSerum);

            OutputText("Scenes.Emberscene.HairExtensionSerum.PourGoblinGunk".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) == 0)
            {
                OutputText("Scenes.Emberscene.HairExtensionSerum.TigerStripeDevelops".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.EmberHair, 1);
            }
            else
            {
                OutputText("Scenes.Emberscene.HairExtensionSerum.StripesMultiply".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.EmberHair, 2);
            }

            OutputText("Scenes.Emberscene.HairExtensionSerum.OnTheEggFragment".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(PlayerMenu);
        }

        public void GiveEmberBludSausages()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.GiveEmberBludSausages.SharingEssence".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
            {
                OutputText("Scenes.Emberscene.GiveEmberBludSausages.GlowsWantsMore".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.EmberInternalDick, 1);
            }
            else
            {
                OutputText("Scenes.Emberscene.GiveEmberBludSausages.NothingHappens".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.EmberInternalDick, 0);
            }

            Player.HealthChange(-1);

            CoC.Instance.Flags.Set(GameFlagIds.EmberRoundface, 1);

            Player.HealthChange(-10);

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(PlayerMenu);
        }

        public void MasturbateOntoAnEgg()
        {
            ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.PulsesSlowDisrobe".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (Player.Gender == Gender.Male)
            {
                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.PulsesRapidUndress".Localized());

                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.GrabsCockStands".Localized());

                if (Player.CockTotal() <= 2)
                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.PumpingVigorously".Localized());
                else
                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.WrangleAllShafts".Localized());

                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.PulsationsLureStroking".Localized());

                if (Player.CumQ() < 100)
                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.TrickleWord".Localized());
                else
                    if (Player.CumQ() <= 500)
                        OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.StreamWord".Localized());
                    else
                        if (Player.CumQ() <= 1000)
                            OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.GoutWord".Localized());
                        else
                            OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.WaveWord".Localized());

                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.OfCumOntoEgg".Localized());

                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.PantingUnleashed".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Genderless)
                {
                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.ShellMutedPink".Localized());

                    CoC.Instance.Flags.Set(GameFlagIds.EmberGender, (int)Gender.Female);
                }

                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.ExaminesForCum".Localized());
            }
            else
                if (Player.Gender == Gender.Female)
                {
                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.PulsesRapidUndressAgain".Localized());

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.WalksUpEggVagina".Localized());

                    if (Player.Wetness() <= 3)
                        OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.FewDropsWord".Localized());
                    else
                        if (Player.Wetness() < 5)
                            OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.SquirtWord".Localized());
                        else
                            OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.TorrentWord".Localized());

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.GirlcumJumpsPussy".Localized());

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.ReleasesPanting".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Genderless)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.EmberGender, (int)Gender.Male);

                        OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.ShellPaleBlue".Localized());
                    }

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.ExaminesForCumAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.PulsesRapidThird".Localized());

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.TormentedBothNeed".Localized());

                    if (Player.CockTotal() == 1)
                        OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.YourCockFragment".Localized());
                    else
                        OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.AsManyCocksGrab".Localized());

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.CumsSpraysSemen".Localized());

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.NoWonderSlipsOff".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Genderless)
                    {
                        CoC.Instance.Flags.Set(GameFlagIds.EmberGender, (int)Gender.Hermaphrodite);

                        OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.ShellLavender".Localized());
                    }

                    OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.ExaminesForCumThird".Localized());
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) < 5)
                OutputText("Scenes.Emberscene.MasturbateOntoAnEgg.GreaterSatisfaction".Localized());

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            CoC.Instance.Flags.Set(GameFlagIds.EmberJackedOn, 1);

            CoC.Instance.Flags.Set(GameFlagIds.EmberEggFluidCount, CoC.Instance.Flags.Get(GameFlagIds.EmberEggFluidCount) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void HatchZeMuzzles()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.HatchZeMuzzles.RestBonelessReexamine".Localized());

            OutputText("Scenes.Emberscene.HatchZeMuzzles.EggGoneFigure".Localized());

            if (LittleEmber())
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Genderless)
                    OutputText("Scenes.Emberscene.HatchZeMuzzles.BrightWord".Localized());
                else
                    OutputText("Scenes.Emberscene.HatchZeMuzzles.LightWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Genderless)
                OutputText("Scenes.Emberscene.EmberEggInteraction.WhiteWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.HatchZeMuzzles.BlueWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female)
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.PinkWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.PurpleWord".Localized());

            OutputText("Scenes.Emberscene.HatchZeMuzzles.DustFragment".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                {
                    OutputText("Scenes.Emberscene.HatchZeMuzzles.ItsBuildLean".Localized((LittleEmber() ? "Common.Text.ShorterThanExpectedStandingALittle".Localized() : "Common.Text.HugeStandingFeetTallAtThe".Localized())));

                    OutputText("Scenes.Emberscene.HatchZeMuzzles.WingsGrowReptilian".Localized((LittleEmber() ? " They seem a little out of place on such a small body." : ""), (LittleEmber() ? "Common.Text.Stubby".Localized() : "Common.Text.Large".Localized())));

                    OutputText("Scenes.Emberscene.HatchZeMuzzles.LongTongueLicks".Localized());

                    OutputText("Scenes.Emberscene.HatchZeMuzzles.ShieldScalesCovered".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.EyesOnChestBreasts".Localized((LittleEmber() ? "A" : "F")));
                    else
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.EyesOnChestScales".Localized());

                    if (Player.Libido >= 50)
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.EyesWanderNext".Localized());
                    else
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.SneakPeekCrotch".Localized());

                    OutputText("Scenes.Emberscene.HatchZeMuzzles.CrotchSlitPenis".Localized((LittleEmber() ? "5" : "16"), (LittleEmber() ? "Common.Text.Small".Localized() : "Appearance.Balls.Size.Apple".Localized())));

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.DroolingNipplesFragment".Localized());

                    OutputText("Scenes.Emberscene.HatchZeMuzzles.SentencePeriod".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female)
                    {
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.ItsBuildLeanAgain".Localized((LittleEmber() ? "Common.Text.ShorterThanExpectedStandingALittle".Localized() : "Common.Text.HugeStandingFeetTallAtThe".Localized())));

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.WingsGrowReptilianAgain".Localized((LittleEmber() ? " They seem a little out of place on such a small body." : ""), (LittleEmber() ? "Common.Text.Stubby".Localized() : "Common.Text.Large".Localized())));

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.LongTongueLicksAgain".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.ShieldScalesCoveredAgain".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.ChestBreastsAgain".Localized((LittleEmber() ? "A" : "F")));

                        if (Player.Libido >= 50)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.EyesWanderTo".Localized());
                        else
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SneakPeekCrotchAgain".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.CrotchSlitPussy".Localized((LittleEmber() ? " tiny," : "")));
                    }
                    else
                    {
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.ItsBuildLeanThird".Localized((LittleEmber() ? "Common.Text.ShorterThanExpectedStandingALittle".Localized() : "Common.Text.HugeStandingFeetTallAtThe".Localized())));

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.WingsGrowReptilianThird".Localized((LittleEmber() ? " They seem a little out of place on such a small body." : ""), (LittleEmber() ? "Common.Text.Stubby".Localized() : "Common.Text.Large".Localized())));

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.LongTongueLicksThird".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.ShieldScalesCoveredThird".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.ChestBreastsThird".Localized((LittleEmber() ? "A" : "F")));

                        if (Player.Libido >= 50)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.EyesWanderToAgain".Localized());
                        else
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SneakPeekCrotchThird".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.CrotchScalesPartTwo".Localized((LittleEmber() ? " tiny" : ""), (LittleEmber() ? " 5" : " 16"), (LittleEmber() ? "Common.Text.Small".Localized() : "Appearance.Balls.Size.Apple".Localized())));
                    }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                {
                    if (LittleEmber())
                    {
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.FirstImpressionChild".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.SlightlyMasculine".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberHair) > 0)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.CompleteWithFragment".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                        {
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SmallBreastsACups".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) > 0)
                                OutputText("Scenes.Emberscene.HatchZeMuzzles.AndFragment".Localized());
                        }

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) > 0)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.LongFeminineLocks".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.MalenessUndeniable".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SlitDisgorgesPenis".Localized());
                        else
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.FiveInchHumanPenis".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.SmallNutsSack".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Emberscene.HatchZeMuzzles.FirstImpressionHumanoid".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.MasculineAppearance".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberHair) > 0)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.CompleteWithAgain".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                        {
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.HugeBreastsFCups".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) > 0)
                                OutputText("Scenes.Emberscene.HatchZeMuzzles.AndFragmentAgain".Localized());
                        }

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) > 0)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.LongFeminineLocksAgain".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.MalenessUndeniableAgain".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SlitDisgorgesLongPenis".Localized());
                        else
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.FootAndHalfPenis".Localized());

                        OutputText("Scenes.Emberscene.HatchZeMuzzles.AppleNutsSack".Localized());
                    }
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female)
                    {
                        if (LittleEmber())
                        {
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.FirstImpressionChildThin".Localized());

                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SlightlyFeminine".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.FirstImpressionHumanoidSlender".Localized());

                            OutputText("Scenes.Emberscene.HatchZeMuzzles.FeminineAppearance".Localized());
                        }
                    }
                    else
                        if (LittleEmber())
                        {
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.FirstImpressionAndrogyne".Localized());

                            OutputText("Scenes.Emberscene.HatchZeMuzzles.TooAndrogynous".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 0)
                                OutputText("Scenes.Emberscene.HatchZeMuzzles.FiveInchHumanPrick".Localized());
                            else
                                OutputText("Scenes.Emberscene.HatchZeMuzzles.FiveInchInhumanCock".Localized());

                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SmallNutsHerm".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Emberscene.HatchZeMuzzles.FirstImpressionHumanoidHerm".Localized());

                            OutputText("Scenes.Emberscene.HatchZeMuzzles.SeemsFeminineFirst".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 0)
                                OutputText("Scenes.Emberscene.HatchZeMuzzles.HugeHumanPrick".Localized());
                            else
                                OutputText("Scenes.Emberscene.HatchZeMuzzles.HugeInhumanCock".Localized());

                            OutputText("Scenes.Emberscene.HatchZeMuzzles.AppleNutsHerm".Localized());
                        }

            DoNext(() => MeetEmberAftermath());
        }

        public void MeetEmberAftermath()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.MeetEmberAftermath.StandsStaring".Localized());

            OutputText("Scenes.Emberscene.MeetEmberAftermath.BreaksSilenceWho".Localized());

            if (LittleEmber())
                OutputText("Scenes.Emberscene.MeetEmberAftermath.IntimidationDestroyed".Localized());

            OutputText("Scenes.Emberscene.MeetEmberAftermath.SpeaksLanguageIntro".Localized());

            OutputText("Scenes.Emberscene.MeetEmberAftermath.RelaxesIntroduces".Localized((LittleEmber() ? "Common.Text.Cute".Localized() : "Common.Text.Showy".Localized())));

            OutputText("Scenes.Emberscene.MeetEmberAftermath.WatchesHatchling".Localized());

            OutputText("Scenes.Emberscene.MeetEmberAftermath.EmberAddedFollowers".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberHatched, 1);

            Player.RemoveKeyItem("Dragon Egg");

            DoNext(() => ReturnToCamp(1));
        }

        public void EmbersAppearance()
        {
            ClearOutput();

            ShowImage("ember-examine-appearance");

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
            {
                OutputText("Scenes.Emberscene.EmbersAppearance.TallHumanoidDragon".Localized((LittleEmber() ? "4'4\"" : "7' 3\""), EmberMF("He", "She"), (LittleEmber() ? "Common.Text.DespiteEmberEirSize".Localized() : "Common.Text.AndImposing".Localized())));

                if (EmberIsMasculine())
                    OutputText("Scenes.Emberscene.EmbersAppearance.NotOverlyMuscled".Localized());
                else
                    OutputText("Scenes.Emberscene.EmbersAppearance.FeminineFragment".Localized());

                if (EmberIsMasculine())
                {
                    OutputText("Scenes.Emberscene.EmbersAppearance.HealthyMaleBody".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                        OutputText("Scenes.Emberscene.EmbersAppearance.DribblingBreasts".Localized());

                    OutputText("Scenes.Emberscene.EmbersAppearance.PrinceSilhouette".Localized());
                }
                else
                    OutputText("Scenes.Emberscene.EmbersAppearance.RoundedSuppleThing".Localized((LittleEmber() ? "Common.Text.Slender".Localized() : "Common.Text.Curvy".Localized()), (LittleEmber() ? "Common.Text.Boyish".Localized() : "Common.Text.PerfectHourglass".Localized()), (LittleEmber() ? "Common.Text.DespiteEmberEirSmallSizeThough".Localized() : "Common.Text.GivingEmberEmTheSilhouetteOf".Localized())));

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.EmbersAppearance.LongNeck".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.ScornsClothing".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.ReptilianHead".Localized(
                    EmberMF("His", "Her")
                ));

                OutputText("Scenes.Emberscene.EmbersAppearance.SecondEyelids".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) == 1)
                {
                    if (EmberIsMasculine())
                        OutputText("Scenes.Emberscene.EmbersAppearance.ShortWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmbersAppearance.ShoulderLengthWord".Localized());

                    OutputText("Scenes.Emberscene.EmbersAppearance.SteelGrayHair".Localized());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) == 2)
                        OutputText("Scenes.Emberscene.EmbersAppearance.ManeDownSpine".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) != 2)
                    OutputText("Scenes.Emberscene.EmbersAppearance.SpineSpikes".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.StrongDragonWings".Localized(
                    EmberMF("His", "Her")
                ));

                if (EmberIsMasculine())
                    OutputText("Scenes.Emberscene.EmbersAppearance.NormalHips".Localized());
                else
                    OutputText("Scenes.Emberscene.EmbersAppearance.DelightfulButt".Localized((LittleEmber() ? "Common.Text.BoyishHipsAreStrangelyEyeCatching".Localized() : "Common.Text.GirlyHipsAreAsEyeCatching".Localized())));

                OutputText("Scenes.Emberscene.EmbersAppearance.ScalyTailLashes".Localized(
                    EmberMF("His", "Her")
                ));

                OutputText("Scenes.Emberscene.EmbersAppearance.ShieldScalesCover".Localized(
                    EmberMF("His", "Her"),

                    EmberMF("His", "Her")
                ));

                OutputText("Scenes.Emberscene.EmbersAppearance.TheWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.EmbersAppearance.LittleWord".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.PinkExposedFlesh".Localized());

                var __sw0_1 = Player.Pregnancy.Event;

                if (__sw0_1 == 2)
                    OutputText("Scenes.Emberscene.EmbersAppearance.SlightBellyPudge".Localized());

                if (__sw0_1 == 3)
                    OutputText("Scenes.Emberscene.EmbersAppearance.BellyLittleLarger".Localized());

                if (__sw0_1 == 4 || __sw0_1 == 5)
                    OutputText("Scenes.Emberscene.EmbersAppearance.BellyGrownQuite".Localized());

                if (__sw0_1 == 6)
                    OutputText("Scenes.Emberscene.EmbersAppearance.PregnantBellyPosture".Localized());

                if (__sw0_1 == 7)
                    OutputText("Scenes.Emberscene.EmbersAppearance.SwollenBellyLarge".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0 ? "Common.Text.CanEasilyMakeOutTheEggs".Localized() : "Common.Text.OccasionallySeeMovementAsTheBaby".Localized())));

                if (__sw0_1 == 8 || __sw0_1 == 9)
                    OutputText("Scenes.Emberscene.EmbersAppearance.BellyStretchedTaut".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0 ? "Common.Text.Egg".Localized() : "Common.Text.Baby".Localized()), (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0 ? "Common.Text.Egg".Localized() : "Common.Text.Baby".Localized())));

                OutputText("Scenes.Emberscene.EmbersAppearance.ChestPairBreasts".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || EmberIsFeminine())
                    OutputText("Scenes.Emberscene.EmbersAppearance.CupBreasts".Localized((LittleEmber() ? "A" : "F"), (LittleEmber() ? "Common.Text.Perky".Localized() : "Scenes.Sheilascene.ForcedSheilaOral.SoftPillowyWord".Localized())));
                else
                    OutputText("Scenes.Emberscene.EmbersAppearance.FlatManlyPecs".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.AreolaScales".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || EmberIsFeminine())
                    OutputText("Scenes.Emberscene.EmbersAppearance.GenerousMounds".Localized());
                else
                    OutputText("Scenes.Emberscene.EmbersAppearance.MasculinePectorals".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.SentencePeriod".Localized());

                if (EmberHasCock())
                {
                    OutputText("Scenes.Emberscene.EmbersAppearance.CrotchCockRidges".Localized((LittleEmber() ? " 5" : " 16"), (LittleEmber() ? "Common.Text.One".Localized() : "Common.Text.Two".Localized())));

                    OutputText("Scenes.Emberscene.EmbersAppearance.RoundedGlansRidges".Localized((LittleEmber() ? "Common.Text.Small".Localized() : "Appearance.Balls.Size.Apple".Localized())));
                }

                if (EmberHasVagina())
                    OutputText("Scenes.Emberscene.EmbersAppearance.SmoothInnerScales".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.PuffsChestPride".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.CuteReactionFidgets".Localized(
                    EmberMF("His", "Her"),

                    EmberMF("His", "Her")
                ));
            }
            else
            {
                OutputText("Scenes.Emberscene.EmbersAppearance.EmberTallPrefix".Localized((LittleEmber() ? "4'4\"" : "7'3\"")));

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.EmbersAppearance.MaleWord".Localized());
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female)
                        OutputText("Scenes.Emberscene.EmbersAppearance.FemaleWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmbersAppearance.HermaphroditeWord".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.SlenderDragon".Localized(
                    EmberMF("boy", "girl"),

                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.EmbersAppearance.ScalePatternFace".Localized((LittleEmber() ? "Common.Text.IsGrowing".Localized() : "Common.Text.Sports".Localized()), (LittleEmber() ? "Common.Text.ThoughTheyreStillBarelyAnInch".Localized() : "Common.Text.BoldlyCurvedBackwardsPastEmberEir".Localized()), (LittleEmber() ? "Common.Text.SurviveAny".Localized() : "Common.Text.DeflectMostOverhead".Localized())));

                OutputText("Scenes.Emberscene.EmbersAppearance.SilverHairCoiffure".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) < 1)
                    OutputText("Scenes.Emberscene.EmbersAppearance.ShortDefinitiveLook".Localized(
                        EmberMF((LittleEmber() ? "Common.Text.Boyish".Localized() : "Common.Text.Masculine".Localized()), "tomboy")
                    ));
                else
                    OutputText("Scenes.Emberscene.EmbersAppearance.ShoulderLengthWarrior".Localized(
                        EmberMF("handsome", "beautiful")
                    ));

                if (LittleEmber())
                {
                    OutputText("Scenes.Emberscene.EmbersAppearance.FlatChestHuman".Localized(
                        EmberMF("His", "Her"),

                        EmberMF("", "the outline of what will someday become breasts just barely visible in the form of tiny pads of babyfat")
                    ));

                    OutputText("Scenes.Emberscene.EmbersAppearance.GoldPendantChest".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.EmbersAppearance.HumanChestBreasts".Localized(
                        EmberMF("His", "Her")
                    ));

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || EmberIsFeminine())
                        OutputText("Scenes.Emberscene.EmbersAppearance.FCupBreasts".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmbersAppearance.ManlyPectoralsNipples".Localized());

                    OutputText("Scenes.Emberscene.EmbersAppearance.GoldPendantChestAgain".Localized());
                }

                OutputText("Scenes.Emberscene.EmbersAppearance.ScalesMeetFlesh".Localized(
                    LittleEmber() ? "childish" : EmberMF("masculine", "feminine")
                ));

                OutputText("Scenes.Emberscene.EmbersAppearance.ScalingExtendsBack".Localized((LittleEmber() ? "oversized " : ""), EmberMF("his", "her"), (LittleEmber() ? " little" : "")));

                OutputText("Scenes.Emberscene.EmbersAppearance.HumanPelvis".Localized());

                if (EmberHasCock())
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 0)
                        OutputText("Scenes.Emberscene.EmbersAppearance.FlaccidPenisBalls".Localized(EmberMF("He", "She"), (LittleEmber() ? "Common.Text.Undersized".Localized() : "Appearance.Balls.Size.Apple".Localized()), (LittleEmber() ? "5" : "16"), (LittleEmber() ? "Common.Text.AnInch".Localized() : "Common.Text.Inches".Localized())));
                    else
                        OutputText("Scenes.Emberscene.EmbersAppearance.ProtectiveSlit".Localized(
                            EmberMF("He", "She"),

                            (LittleEmber() ? "Common.Text.InchesLongAndAnInchThick".Localized() : "Common.Text.InchesLongAndInchesThick".Localized())
                        ));

                if (EmberHasVagina())
                    if (LittleEmber())
                        OutputText("Scenes.Emberscene.EmbersAppearance.SoftPussyLips".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmbersAppearance.InvitingPussyLips".Localized());

                OutputText("Scenes.Emberscene.EmbersAppearance.HumanLikeLegs".Localized());

                if (LittleEmber())
                {
                    OutputText("Scenes.Emberscene.EmbersAppearance.PuffsFlatChestPride".Localized());

                    OutputText("Scenes.Emberscene.EmbersAppearance.CuteReactionFidgetsAgain".Localized(
                        EmberMF("His", "Her"),

                        EmberMF("His", "Her")
                    ));
                }
                else
                    OutputText("Scenes.Emberscene.EmbersAppearance.DragonAttentionDrawn".Localized());
            }

            DoNext(() => EmberCampMenu());
        }

        public void TalkToEmber()
        {
            ClearOutput();

            if (Player.IsPregnant() && EmberHasCock())
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviBitchedYet) == 0 && Player.Pregnancy.Type == PregnancyIds.OvielixirEggs)
                {
                    EmberBitchesAboutPCBeingFullOfEggs();

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

                if (Player.Pregnancy.Incubation < 200 && Player.Pregnancy.Type != PregnancyIds.Ember && CoC.Instance.Flags.Get(GameFlagIds.EmberBitchesAboutPregnantPc) == 0)
                {
                    ManEmberBitchesAboutPCPregnancy();

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }

                if (Player.Pregnancy.Type == PregnancyIds.Ember && Player.Pregnancy.Incubation < 300 && CoC.Instance.Flags.Get(GameFlagIds.EmberTalksToPcAboutPcMotheringDragons) == 0)
                {
                    EmberTalksToPCAboutPCDragoNPregnancy();

                    DoNext(() => ReturnToCamp(1));

                    return ;
                }
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberPregnantTalk) == 0 && Player.Pregnancy.Event > 1)
            {
                EmberIsPregnantFirstTimeTalkScene();

                DoNext(() => ReturnToCamp(1));
            }

            OutputText("Scenes.Emberscene.TalkToEmber.TalkTopicPrompt".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Dragons".Localized(), TalkToEmberAboutDragonzzz);

            AddButton(1, "Common.Button.Exploring".Localized(), DiscussExplorationWithEmber);

            AddButton(2, "Scenes.PlayerAppearance.Name".Localized(), TalkToEmberAboutYourself);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) >= 100 && CoC.Instance.Flags.Get(GameFlagIds.EggBroken) < 1)
                AddButton(3, "Common.Button.Eggshell".Localized(), AskEmberForEggshell);

            AddButton(14, "Common.Button.Back".Localized(), () => EmberCampMenu());
        }

        public void TalkToEmberAboutDragonzzz()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.AskAboutSpecies".Localized());

            float choice = Random.Shared.Next(5);

            if (choice == 0)
            {
                OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.CrossesArmsPride".Localized(EmberMF("his", "her"), (LittleEmber() ? " slender" : "")));

                EmberAffection(2 + Random.Shared.Next(3));
            }
            else
                if (choice == 1)
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.ThinksThenAnatomy".Localized((LittleEmber() ? " flat" : "")));

                    EmberAffection(2 + Random.Shared.Next(3));
                }
                else
                    if (choice == 2)
                    {
                        OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.MatingRituals".Localized());

                        OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.FaceTurnsSerious".Localized((LittleEmber() ? "Common.Text.Right".Localized() : "Common.Text.DammitThenWhyDoIFeel".Localized())));

                        OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.AskSpeakUp".Localized());

                        EmberAffection(5);
                    }
                    else
                        if (choice == 3)
                        {
                            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.DrakesHeartFlower".Localized());

                            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.FlowerMysteryAsk".Localized());

                            if (EmberAffection() <= 25)
                                OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.SnortsAmusedSmile".Localized((LittleEmber() ? "Common.Text.Giggles".Localized() : "Common.Text.Chuckles".Localized()), EmberMF("He", "She")));
                            else
                                if (EmberAffection() <= 75)
                                    OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.DragonDaydreaming".Localized((LittleEmber() ? "" : "Look, "), EmberMF("His", "Her")));
                                else
                                    OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.EmbarrassedReaction".Localized((LittleEmber() ? "little " : ""), (LittleEmber() ? "" : "Look, "), (LittleEmber() ? "!" : ".")));

                            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.ThankConversationDone".Localized());

                            EmberAffection(5);

                            Save.SetBool("flowerExplained", true);
                        }
                        else
                        {
                            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.DragonHabitsTalk".Localized());

                            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.StopsAbruptly".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                                OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.BlushCreeping".Localized());

                            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.SentencePeriod".Localized());

                            OutputText("Scenes.Emberscene.TalkToEmberAboutDragonzzz.AskIfAllRight".Localized());

                            EmberAffection(5);
                        }

            DoNext(PlayerMenu);

            View.Time.PassMinutes((int)Math.Round((double)((1 / 6) * 60)));
        }

        public void DiscussExplorationWithEmber()
        {
            ClearOutput();

            var choice = Random.Shared.Next(4);

            var subChoice = 0;

            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.AskForNews".Localized());

            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.NodsScratchesChin".Localized());

            if (choice == 0)
            {
                subChoice = Random.Shared.Next(6);

                OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.FoundMountainRange".Localized((LittleEmber() ? "Common.Text.LongTime".Localized() : "Common.Text.GoodWhile".Localized())));

                if (subChoice == 0)
                    OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.FewNervousImps".Localized((LittleEmber() ? "Common.Text.Really".Localized() : "Common.Text.Very".Localized()), (LittleEmber() ? "" : "naturally ")));
                else
                    if (subChoice == 1)
                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.GoblinsArguing".Localized((LittleEmber() ? "Common.Text.Because".Localized() : "Common.Text.AsApparently".Localized()), (LittleEmber() ? "Common.Text.TheyWereReallyLewd".Localized() : "Common.Text.UghDisgustingLittleCreatures".Localized())));
                    else
                        if (subChoice == 2)
                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.BullMenBrawling".Localized((LittleEmber() ? "Common.Text.StartedDoingLewdThingsToThe".Localized() : "Common.Text.PromptlyStartedFuckingTheLoserUp".Localized()), (LittleEmber() ? "Common.Text.DidntWantToKeepWatchingThat".Localized() : "Common.Text.HadSeenMoreThanEnoughBy".Localized())));
                        else
                            if (subChoice == 3)
                                OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.HermWomanBigDick".Localized((LittleEmber() ? "Scenes.Milkwaifu.NyanCatMilkTime.ThingWord".Localized() : "Common.Text.Creature".Localized()), (LittleEmber() ? "" : "stark ")));
                            else
                                if (subChoice == 4)
                                    OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.GoblinsSharpeningScissors".Localized((LittleEmber() ? "I w" : "W")));
                                else
                                    if (Random.Shared.Next(100) < (parasiteRating * 25))
                                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.WhiteWormSwarm".Localized((LittleEmber() ? "Common.Text.MessedUpHumanShape".Localized() : "Common.Text.MockeryOfAHumanForm".Localized()), (LittleEmber() ? "Common.Text.Grab".Localized() : "Common.Text.Latch".Localized()), (LittleEmber() ? "Common.Text.Dicks".Localized() : "Common.Text.Cocks".Localized())));
                                    else
                                        if (subChoice == 5)
                                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.TwoHeadedDog".Localized((LittleEmber() ? "Common.Text.Saw".Localized() : "Common.Text.Spotted".Localized()), (LittleEmber() ? "Common.Text.StartedChasingIt".Localized() : "Common.Text.GaveChase".Localized()), (LittleEmber() ? "It m" : "M"), (LittleEmber() ? "Common.Text.AndDidLewdThingsToThe".Localized() : "Common.Text.WhereuponItAssRapedItThen".Localized())));
            }
            else
                if (choice == 1)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.TamaniBadEnded) <= 0)
                        subChoice = Random.Shared.Next(5);
                    else
                    {
                        subChoice = Random.Shared.Next(4);

                        if (subChoice == 3)
                            subChoice += 1;
                    }

                    OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.FoundForest".Localized((LittleEmber() ? "" : "must confess I "), (LittleEmber() ? "Common.Text.Bunch".Localized() : "Common.Text.Host".Localized())));

                    if (subChoice == 0)
                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.TribeOfImps".Localized((LittleEmber() ? "Common.Text.DoingLewdThingsToThemselves".Localized() : "Common.Text.JerkingThemselvesOff".Localized()), (LittleEmber() ? "Common.Text.Fighting".Localized() : "Common.Text.Squabbling".Localized())));
                    else
                        if (subChoice == 1)
                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.PregnantGoblinAle".Localized());
                        else
                            if (subChoice == 2)
                                OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.BeeWomanMusic".Localized());
                            else
                                if (subChoice == 3 && CoC.Instance.Flags.Get(GameFlagIds.TamaniNumberOfDaughters) > 0)
                                {
                                    OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.TamaniNursingDaughters".Localized());

                                    if (EmberIsFeminine())
                                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.ShouldHaveMorePride".Localized((LittleEmber() ? "Common.Text.Slut".Localized() : "Common.Text.Cunt".Localized())));
                                }
                                else
                                    if (subChoice == 3 && true)
                                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.GoblinTeasedPregnancy".Localized());
                                    else
                                        if (Random.Shared.Next(2) == 0 && CoC.Instance.Flags.Get(GameFlagIds.JojoStatus) <= 1 && CoC.Instance.Flags.Get(GameFlagIds.JojoFixedStatus) == 0)
                                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.MouseMonkMeditating".Localized((noFur ? "Common.Text.Eared".Localized() : "Common.Text.Morph".Localized())));
                                        else
                                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.GladeStrangeCreature".Localized());
                }
                else
                    if (choice == 2)
                    {
                        subChoice = Random.Shared.Next(2);

                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.FoundLake".Localized());

                        if (subChoice == 0)
                        {
                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.SharkWomenPair".Localized());

                            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                                OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.LikeMeWord".Localized());

                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.BeachTogether".Localized((LittleEmber() ? "Common.Text.HavingSex".Localized() : "Common.Text.FuckingEachOthersBrainsOut".Localized())));
                        }
                        else
                            if (Random.Shared.Next(3) == 0 && Player.StatusEffectValue(StatusEffectIds.MetWhitney, 0) <= 1)
                                OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.ShorelineFarm".Localized());
                            else
                                if (Random.Shared.Next(3) == 0 && Player.HasStatusEffect(StatusEffectIds.FetishOn))
                                    OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.HumansArguingNaughty".Localized((LittleEmber() ? "Common.Text.LewdThings".Localized() : "Common.Text.Sex".Localized()), (LittleEmber() ? "to do " : "")));
                                else
                                    if (Random.Shared.Next(3) == 0 && !Player.HasStatusEffect(StatusEffectIds.TookBlessedSword) && !Player.HasStatusEffect(StatusEffectIds.BSwordBroken))
                                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.SwordInTree".Localized());
                                    else
                                        if (Random.Shared.Next(3) == 0 && !SceneInstance<RathazulScene>.Get().FollowerRathazul())
                                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.RatManAlchemist".Localized());
                                        else
                                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.GreenGooBlob".Localized());
                    }
                    else
                        if (choice == 3)
                        {
                            subChoice = Random.Shared.Next(4);

                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.FoundDesert".Localized((LittleEmber() ? "" : "far ")));

                            if (subChoice == 0)
                                OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.FourBreastedWoman".Localized((LittleEmber() ? "Appearance.Breast.Noun.Boobs".Localized() : "Common.Text.Tits".Localized())));
                            else
                                if (subChoice == 1)
                                    OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.DemonsAtOasis".Localized());
                                else
                                    if (Random.Shared.Next(2) == 0 && !Player.HasStatusEffect(StatusEffectIds.WandererDemon) && !Player.HasStatusEffect(StatusEffectIds.WandererHuman))
                                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.WheelbarrowBalls".Localized());
                                    else
                                    {
                                        OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.SnakeTailWoman".Localized());

                                        if (Player.IsNaga())
                                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.LookedLikeYou".Localized());
                                        else
                                            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.NeverSeenBefore".Localized());
                                    }
                        }

            OutputText("Scenes.Emberscene.DiscussExplorationWithEmber.SayEnd".Localized());

            DoNext(PlayerMenu);

            View.Time.PassMinutes((int)Math.Round((double)((1 / 6) * 60)));
        }

        public void TalkToEmberAboutYourself()
        {
            ClearOutput();

            float points = 0;

            OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.AskWhatThinks".Localized());

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.WasteOfTime".Localized(
                    EmberMF("He", "She")
                ));

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.LooksYouOver".Localized(
                EmberMF("He", "She")
            ));

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 50)
                OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.NeedMoreTraining".Localized());
            else
                if ((100 * Player.Strength / Player.GetMaxStats("str")) < 90)
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.StrongAsDragon".Localized());

                    points += 1;
                }
                else
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.LiftAnythingEvenMe".Localized((LittleEmber() ? "Common.Text.ADreamyLookInHerEyes".Localized() : "Common.Text.DreamilyAlmostPurring".Localized())));

                    points += 2;
                }

            OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.PokesChestRibs".Localized());

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 50)
                OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.WindBlowAway".Localized());
            else
                if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 90)
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.ToughAsMe".Localized());

                    points += 1;
                }
                else
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.ShowOffBody".Localized());

                    EmberAffection(1);

                    points += 2;
                }

            OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.LooksIntoEyes".Localized());

            if (Player.Intelligence < 50)
                OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.StillVeryNaive".Localized());
            else
                if (Player.Intelligence < 90)
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.SeeWisdomLessons".Localized());

                    points += 1;
                }
                else
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.EyesWidenGreatWisdom".Localized(EmberMF("He", "She"), (LittleEmber() ? "" : " steely")));

                    points += 2;
                }

            OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.ThinkFastPebble".Localized());

            if ((100 * Player.Speed / Player.GetMaxStats("spe")) < 50)
                OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.PebbleHitsBelly".Localized());
            else
                if ((100 * Player.Speed / Player.GetMaxStats("spe")) < 90)
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.DeflectPebblePraise".Localized());

                    points += 1;
                }
                else
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.CatchThrowBack".Localized());

                    points += 2;
                }

            OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.SatisfiedTurns".Localized());

            if (Player.NagaScore() >= 3 || Player.LizardScore() >= 3 || Player.DragonScore() >= 3)
            {
                OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.EyesLingerForm".Localized());

                points += 1;
            }

            OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.HeresWhatIThink".Localized());

            if (points < 2)
                OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.MiracleYouSurvived".Localized());
            else
                if (points < 6)
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.DoingFineKeepTraining".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.TalkToEmberAboutYourself.QuiteACatch".Localized());

                    EmberAffection(5);
                }

            DoNext(PlayerMenu);

            View.Time.PassMinutes((int)Math.Round((double)((1 / 6) * 60)));
        }

        public void AskEmberForEggshell()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.AskEmberForEggshell.AskForEggshell".Localized());

            OutputText("Scenes.Emberscene.AskEmberForEggshell.SinceILikeYou".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.AskEmberForEggshell.TakeToArmorsmith".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EggBroken, 1);

            OutputText("Scenes.Emberscene.AskEmberForEggshell.GainedDragonEggshell".Localized());

            Player.CreateKeyItem("Dragon Eggshell", 0, 0, 0, 0);

            DoNext(PlayerMenu);

            View.Time.PassMinutes((int)Math.Round((double)((1 / 3) * 60)));
        }

        public void ManEmberBitchesAboutPCPregnancy()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.EmberBitchesAboutPregnantPc, 1);

            if (EmberAffection() <= 25)
                OutputText("Scenes.Emberscene.ManEmberBitchesAboutPCPregnancy.LightAiryTalk".Localized());
            else
                if (EmberAffection() < 75)
                    OutputText("Scenes.Emberscene.ManEmberBitchesAboutPCPregnancy.StaresColdlyGravid".Localized());
                else
                    OutputText("Scenes.Emberscene.ManEmberBitchesAboutPCPregnancy.WhatMeaningOfThis".Localized((LittleEmber() ? "Common.Text.OtherPeople".Localized() : "Common.Text.SlutAroundWithTrash".Localized())));
        }

        public void EmberTalksToPCAboutPCDragoNPregnancy()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.EmberTalksToPcAboutPcMotheringDragons, 1);

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.EyesFixatedBelly".Localized());

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.HopeFirstOfMany".Localized());

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.PressWhatMeans".Localized());

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.DidntSayAnything".Localized());

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.FeelingFineHeavy".Localized());

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.ShynessCaves".Localized());

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.YouWord".Localized());

            if (Player.Libido < 40)
                OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.StartAtRevelation".Localized());

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.SultrySmileHardening".Localized((LittleEmber() ? "Common.Text.PullEmberEmIntoYourLap".Localized() : "Common.Text.SeatYourselfInEmberEirLap".Localized()), EmberMF("He", "She"), (LittleEmber() ? "Common.Text.AwayFrom".Localized() : "Common.Text.FromUnder".Localized())));

            OutputText("Scenes.Emberscene.EmberTalksToPCAboutPCDragoNPregnancy.WatchHerGoSmile".Localized());
        }

        public void EmberIsPregnantFirstTimeTalkScene()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.EmberPregnantTalk, 1);

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.StareAtBelly".Localized((LittleEmber() ? ", one who's still a child [Ember emself]" : "")));

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.CatchesYouStaring".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.AndBlushes".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.RubsBellyLovingly".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.BeautifulWhenPregnant".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.CheesyWordsChoke".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.ReallyThinkSo".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.RadiantMaternalGlow".Localized());

            if (Player.Corruption >= 66)
                OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.BuyingThisNonsense".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.CantHideHappiness".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.NotHowRemember".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.WouldntHaveDoneThat".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.YouTeasingHer".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.YouCheated".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.GiveChaseCatch".Localized());

            if (Player.Corruption < 30)
                OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.LuckyToHaveHer".Localized());

            OutputText("Scenes.Emberscene.EmberIsPregnantFirstTimeTalkScene.SmileBroadensPat".Localized());
        }

        public void EmberBitchesAboutPCBeingFullOfEggs()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberBitchesAboutPCBeingFullOfEggs.StaringAtEggBelly".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAboutPCBeingFullOfEggs.WasntStaring".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAboutPCBeingFullOfEggs.WhoSaidUnfertilized".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAboutPCBeingFullOfEggs.NeedBathGross".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAboutPCBeingFullOfEggs.ShakeHeadWonder".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberOviBitchedYet, 1);
        }

        public void EmberBitchesAtYouAboutLustiness()
        {

            OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.MindOnTopic".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.StiffAsIron".Localized());

                if (Player.HasVagina())
                    OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.AndWord".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.SlickWetMoisture".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.WantSexHurts".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.TooHornyThink".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.InsistNothingsWrong".Localized());

            if (EmberAffection() <= 25)
                OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.DoSomethingAboutIt".Localized());
            else
                if (EmberAffection() < 75)
                    OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.MightBePersuaded".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.KissOnCheek".Localized());

            OutputText("Scenes.Emberscene.EmberBitchesAtYouAboutLustiness.ParagraphBreak".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberLustBitchingCounter, 0);
        }

        public void MinotaurJizzFreakout()
        {

            OutputText("Scenes.Emberscene.MinotaurJizzFreakout.HardToConcentrate".Localized());

            OutputText("Scenes.Emberscene.MinotaurJizzFreakout.RoarOfFury".Localized());

            OutputText("Scenes.Emberscene.MinotaurJizzFreakout.WatchYouSuffer".Localized(
                EmberMF("He", "She")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0)
                OutputText("Scenes.Emberscene.MinotaurJizzFreakout.KihaThoughtTemper".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberCurrentlyFreakingAboutMinocum, 1);

            OutputText("Scenes.Emberscene.MinotaurJizzFreakout.ParagraphBreak".Localized());
        }

        public void EmberGetOverFreakingOutAboutMinoJizz()
        {

            CoC.Instance.Flags.Set(GameFlagIds.EmberCurrentlyFreakingAboutMinocum, 0);

            OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.NoLongerPlagued".Localized());

            OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.FaceLightsUp".Localized());

            OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.GreatToHear".Localized());

            OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.NoHuntMinotaurs".Localized());

            OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.ThunderousBelch".Localized(
                EmberMF("He", "She")
            ));

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.NotForYou".Localized(
                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.HuntingForBeef".Localized());
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.TiredRoastedBeef".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.AppreciateIntent".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.CookMeSomething".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.ChronicIndigestion".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.GetMeTea".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.LifeFuckingWeird".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.StayAwayBulls".Localized());

                    if (EmberHasCock())
                        OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.EspeciallySuckingCock".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.ComeToMeInstead".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.HowElseHelp".Localized());

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.JustForgetIt".Localized(
                        EmberMF("He", "She")
                    ));

                    OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.BellyrubPet".Localized());
                }

            OutputText("Scenes.Emberscene.EmberGetOverFreakingOutAboutMinoJizz.SighWalkAway".Localized());
        }

        public void BloodForTheBloodGod()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.BloodForTheBloodGod.AskForBloodTaste".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.DrankEmberBloodToday) == 1 && !debug)
            {
                OutputText("Scenes.Emberscene.BloodForTheBloodGod.NoBloodRecovering".Localized());

                EmberCampMenu(false);

                return ;
            }

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.BloodForTheBloodGod.WhyHurtMyself".Localized());

                DoNext(PlayerMenu);

                return ;
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.MightRubOff".Localized());

                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.ReachOutTake".Localized());

                    if (Player.Corruption < 50)
                        OutputText("Scenes.Emberscene.BloodForTheBloodGod.KissFingersCut".Localized());
                    else
                        OutputText("Scenes.Emberscene.BloodForTheBloodGod.SealLipsCut".Localized());

                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.WincesThenRecovers".Localized());

                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.RushOfEnergy".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Continue".Localized(), DrinkDeeplyOfDagronBlud);

                    AddButton(1, "Common.Button.Stop".Localized(), NoMoDagronBlud);
                }
                else
                {
                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.NiceEnoughFavor".Localized());

                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.BitesTongueFang".Localized());

                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.RudeToKeepWaiting".Localized(
                        LittleEmber() ? "your little " + EmberMF("boy", "girl") + "Common.Text.Friend".Localized() : "[Ember em]"
                    ));

                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.KissesBackBleeding".Localized());

                    OutputText("Scenes.Emberscene.BloodForTheBloodGod.RushOfEnergyAgain".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Continue".Localized(), DrinkDeeplyOfDagronBlud);

                    AddButton(1, "Common.Button.Stop".Localized(), NoMoDagronBlud);
                }

            CoC.Instance.Flags.Set(GameFlagIds.DrankEmberBloodToday, 1);

            var stat = Random.Shared.Next(4);

            if (stat == 0)
                Player.ApplyStats(new DynStat(GameStats.Strength, 1f));
            else
                if (stat == 1)
                    Player.ApplyStats(new DynStat(GameStats.Toughness, 1f));
                else
                    if (stat == 2)
                        Player.ApplyStats(new DynStat(GameStats.Speed, 1f));
                    else
                        Player.ApplyStats(new DynStat(GameStats.Intelligence, 1f));
        }

        public void NoMoDagronBlud()
        {
            ClearOutput();

            if (EmberAffection() < 75)
            {
                OutputText("Scenes.Emberscene.NoMoDagronBlud.StopForNow".Localized());

                OutputText("Scenes.Emberscene.NoMoDagronBlud.DontMentionIt".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.NoMoDagronBlud.StopForNowAgain".Localized());

                OutputText("Scenes.Emberscene.NoMoDagronBlud.WhatStrangeIdeas".Localized());

                OutputText("Scenes.Emberscene.NoMoDagronBlud.TheOnesGetting".Localized());
            }

            DoNext(() => ReturnToCamp(1));
        }

        public void DrinkDeeplyOfDagronBlud()
        {
            ClearOutput();

            var hasDoNext = false;

            if (EmberAffection() < 75)
            {
                OutputText("Scenes.Emberscene.DrinkDeeplyOfDagronBlud.ContinueDrinking".Localized());

                hasDoNext = true;

                OutputText("Scenes.Emberscene.DrinkDeeplyOfDagronBlud.DrankTooMuch".Localized());

                OutputText("Scenes.Emberscene.DrinkDeeplyOfDagronBlud.OfferHelpingHand".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.DrinkDeeplyOfDagronBlud.ContinueDrinkingAgain".Localized());

                hasDoNext = true;

                OutputText("Scenes.Emberscene.DrinkDeeplyOfDagronBlud.BreakKissWoozy".Localized());

                OutputText("Scenes.Emberscene.DrinkDeeplyOfDagronBlud.QuickHelpingHand".Localized());
            }

            if (!hasDoNext)
                EmberCampMenu(false);
        }

        public void EmberIsAnEggFactory()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberIsAnEggFactory.AskEmberLayEgg".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) <= 0)
                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.GiveOvipositionElixir".Localized());

            OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SentencePeriod".Localized());

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberEyesDarken".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.ApologizeNeedBlankEggs".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberPuffsSmokeRing".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.AssureNothingWeirdPlanned".Localized());

                if (Player.Corruption >= 40)
                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SellForQuickCash".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.KeepSellIdeaSecret".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberDucksForPrivacy".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SneakAfterEmber".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberSitsLegsOpen".Localized());

                if (EmberHasCock())
                {
                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.AndHerWord".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.DragonDickJuts".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.VeinyBulbousPenis".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.ErectWord".Localized());
                }

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.HandTeasesClit".Localized((LittleEmber() ? "puffy " : "")));

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.HerPaceQuickens".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.LicksEggClean".Localized());

                if (EmberHasCock())
                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.ThrobbingCockSoftens".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.PresentsEggToYou".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.ReturnToWaitingSpot".Localized(
                    LittleEmber() ? "Common.Text.TheLittle".Localized() + EmberMF("boy's", "girl's") : "Ember's little"
                ));

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberAppearsWithEgg".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberHoldsOutEgg".Localized());

                OutputText("Scenes.Emberscene.EmberIsAnEggFactory.MumblesFertilizeNextTime".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

                InventoryScene.TakeItem(ItemIds.DragonEgg, () => ReturnToCamp(1));
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberGasps".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.CheeksRedWithShame".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.CantFinishSentence".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.ApologizeAssureHelpful".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.CringesAtBarrenEggs".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.BitesLipsAsksWatch".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.BlinkAtOfferSmile".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Watch".Localized(), WatchMediumAffectionEmberEggLay);

                    AddButton(1, "Common.Button.DontWatch".Localized(), DontWatchEmberLayEgg);
                }
                else
                {
                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SmirksDespiteShame".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) <= 0)
                    {
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.UncorksElixirBellySwells".Localized());

                        Player.DestroyItems(ConsumableIds.OviElixir, 1);
                    }

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.AskWhatSheMeans".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.PinchedSnakeFace".Localized());

                    if (EmberHasCock())
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.CockPointsAccusingly".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SentencePeriodAgain".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.LeadsToSecludedSpot".Localized((LittleEmber() ? "Common.Text.TinyUndevelopedCunny".Localized() : "Common.Text.PinkMoistFuckhole".Localized())));

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.TellHerYouDo".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.JustGetStarted".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.NoPointLeavingHanging".Localized());

                    if (Player.Body.Get<TonguePart>().Id.Path != "human")
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.TongueRollsFurther".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.LickUpHerCenter".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SlurpToCockTip".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SavorHerJuices".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.GaspsMoansPleasure".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.DontIntendToStop".Localized());

                    if (Player.Body.Get<TonguePart>().Id.Path != "human")
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.QuiteFarIndeed".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.OralTricksCaress".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.DribblesDownStiffDick".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.DoingRatherWell".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.NoseBumpsHerButton".Localized());

                    if (Player.Body.Get<TonguePart>().Id.Path != "human")
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.FeelRoundSmoothThing".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EmberScreamsOrgasm".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.CockPumpsStreams".Localized());

                    if (Player.Body.Get<TonguePart>().Id.Path != "human")
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.FeelEggWrapTongue".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberIsAnEggFactory.ShellPressesTongue".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.LegsRelaxLetEscape".Localized((LittleEmber() ? "little " : "")));

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.EarnsGlareScampers".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.TeaseConvenientExcuse".Localized());

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.FightingLosingBattle".Localized((LittleEmber() ? "Common.Text.Pouts".Localized() : "Common.Text.Sighs".Localized())));

                    OutputText("Scenes.Emberscene.EmberIsAnEggFactory.SmileUnderstandHer".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

                    InventoryScene.TakeItem(ItemIds.DragonEgg, () => ReturnToCamp(1));
                }
        }

        public void DontWatchEmberLayEgg()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.DontWatchEmberLayEgg.WontIntrudePrivacy".Localized());

            OutputText("Scenes.Emberscene.DontWatchEmberLayEgg.SashaysOffAwaits".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.DontWatchEmberLayEgg.DripsOffWhiteFluid".Localized());

            OutputText("Scenes.Emberscene.DontWatchEmberLayEgg.EyesGlazeAtSuggestion".Localized());

            InventoryScene.TakeItem(ItemIds.DragonEgg, () => ReturnToCamp(1));
        }

        public void WatchMediumAffectionEmberEggLay()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.FailsHideArousal".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.CockPokesStraightOut".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.ItsHidingPlace".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.OnlyAllowedToWatch".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.PromiseWontDoAnything".Localized());

            if (Player.Libido >= 50)
                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.DoesntAskBeamsGrin".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SentencePeriod".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SitsSpreadsLegsView".Localized((LittleEmber() ? "Common.Text.Hairless".Localized() : "Common.Text.Moist".Localized()), (LittleEmber() ? "puffy " : "")));

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.GazeInsidePinkVulva".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.PromiseHarderToKeep".Localized());

            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SemicolonSpace".Localized());

                if (Player.HasCock())
                {
                    OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.BulgeInsideArmor".Localized());

                    if (Player.HasVagina())
                        OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.AndWord".Localized());
                }

                if (Player.HasVagina())
                    OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.WetnessGathersPussy".Localized());

                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.IndicateDesire".Localized());

                if (!Player.HasCock() || !Player.HasVagina())
                    OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SLetter".Localized());

                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.DesireBreakPromise".Localized());
            }

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.ClawedFingerPenetrates".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.PleasureOfExhibiting".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.HopingSheWontRegret".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.CaressThrobbingCock".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SentencePeriodAgain".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.DeepMoanShakesOrgasms".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.JismSpewsFromCock".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.EggPlugsPleasureHole".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SighReliefCollapses".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.AppraiseEggThenReturn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.CockHangingOut".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.HardToSeeSame".Localized((LittleEmber() ? "Common.Text.PreteenGirl".Localized() : "Common.Text.IceQueen".Localized())));

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SitsUpLicksLips".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.CantResistTellingHer".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.TouchedMeStammer".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.SheAskedForHelp".Localized());

            OutputText("Scenes.Emberscene.WatchMediumAffectionEmberEggLay.ShakeHeadAndSigh".Localized());

            InventoryScene.TakeItem(ItemIds.DragonEgg, () => ReturnToCamp(1));

            EmberAffection(5);
        }

        public void GetMilkFromEmber()
        {
            ClearOutput();

            ShowImage("ember-drink-her-milk");

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
            {
                OutputText("Scenes.Emberscene.GetMilkFromEmber.GazeDrawnToEmber".Localized((LittleEmber() ? "Body.BreastCup.Zero".Localized() : "Common.Text.RoundFirm".Localized())));

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.ScalyWord".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.BareNipplesEnticing".Localized((LittleEmber() ? "Common.Text.Chest".Localized() : "Common.Text.Breasts".Localized()), (LittleEmber() ? "Common.Text.Tiny".Localized() : "Common.Text.Perky".Localized())));
            }
            else
            {
                OutputText("Scenes.Emberscene.GetMilkFromEmber.GazeDrawnAgain".Localized((LittleEmber() ? "Body.BreastCup.Zero".Localized() : "Common.Text.RoundFirm".Localized())));

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.ScalyWordAgain".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.BareNipplesEnticingAgain".Localized((LittleEmber() ? "Common.Text.Chest".Localized() : "Common.Text.Breasts".Localized()), (LittleEmber() ? "Common.Text.Tiny".Localized() : "Common.Text.Perky".Localized())));
            }

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.GetMilkFromEmber.TooOldToSuckle".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.HonestTruthThirsty".Localized((LittleEmber() ? "Common.Text.TheyAreWithSoLittleRoom".Localized() : "Common.Text.AndHeavyTheyAreFromAll".Localized())));

                OutputText("Scenes.Emberscene.GetMilkFromEmber.FaceCloudsFeelFull".Localized(EmberMF("His", "Her"), (LittleEmber() ? "Scenes.Rubi.RubiAppearance.FlatChest".Localized() : "Common.Text.Breasts".Localized())));

                OutputText("Scenes.Emberscene.GetMilkFromEmber.AskWhoMeantFor".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) == 0 && EmberChildren() == 0)
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.NoOffspringToFeed".Localized(
                        EmberMF("he", "she")
                    ));
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) > 0)
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.ChildrenNotHatched".Localized(
                            EmberMF("his", "her")
                        ));
                    else
                        if (LittleEmber())
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.SwollenWithMilkAgain".Localized());
                        else
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.ChildrenDontDrinkEnough".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.EyesSinkConcedes".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.PromiseBehaveNurse".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.ReclinesOnLeaves".Localized(
                    EmberMF("His", "Her")
                ));

                OutputText("Scenes.Emberscene.GetMilkFromEmber.SeatLapStrokeBreasts".Localized((LittleEmber() ? "Common.Text.EmberEmInYour".Localized() : "Common.Text.YourselfInEmberEir".Localized()), (LittleEmber() ? "Common.Text.Tiny".Localized() : "Scenes.Lottie.LottieAppearance.Bountiful".Localized())));

                OutputText("Scenes.Emberscene.GetMilkFromEmber.FlinchesThenCalms".Localized((LittleEmber() ? "Common.Text.NearlyFlatChest".Localized() : "Common.Text.Weight".Localized())));

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.ScalesWord".Localized());
                else
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SkinWord".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.CuteGaspEyesDart".Localized(
                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.GetMilkFromEmber.NipplesWeepFirstDrop".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.DontDareStop".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.NoEncouragementNeeded".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) != 2)
                {
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SomethingPokingYou".Localized());

                    if (!Player.IsTaur() || LittleEmber())
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.BellyWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.BackWord".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.LookDownAtShaft".Localized());
                }
                else
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.HumsMoansHandSnakes".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.RealizationDawnsGetsUp".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.ApologizePointOut".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.FlushesNoMoreMilk".Localized());

                OutputText("Scenes.Emberscene.GetMilkFromEmber.ShakeHeadGoodTemper".Localized());

                if (Player.HasStatusEffect(StatusEffectIds.DragonBreathCooldown))
                {
                    Player.RemoveStatusEffect(StatusEffectIds.DragonBreathCooldown);

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.ThroatSoothedSorenessGone".Localized());
                }

                Player.RefillHunger(25);

                if (Random.Shared.Next(2) == 0 && Player.DragonScore() >= 4 && (int)Player.Gender > 0 && ((int)Player.Gender != CoC.Instance.Flags.Get(GameFlagIds.EmberGender) || ((int)Player.Gender == (int)Gender.Hermaphrodite && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)))
                {
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SuddenSwellOfLust".Localized());

                    if (Player.HasCock())
                    {
                        OutputText(Player.CockDescript(0));

                        if (Player.HasVagina())
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.AndWord".Localized());
                    }

                    if (Player.HasVagina())
                        OutputText(Player.VaginaDescript());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.WishEmberHadntLeft".Localized());

                    if (Player.HasCock() && EmberHasVagina())
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.FillWombWithSeed".Localized());

                        if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.EvenWhileWord".Localized());
                    }

                    if (Player.HasVagina() && EmberHasCock())
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.HardCockInsideYou".Localized(
                            Player.VaginaDescript(0)
                        ));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.TooLateMilkRut".Localized());

                    if (Player.HasCock() && EmberHasVagina())
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.RutWord".Localized());

                        Player.GoIntoRut(false);
                    }
                    else
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.HeatWord".Localized());

                        Player.GoIntoHeat(false);
                    }

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.BoldEndTag".Localized());
                }
            }
            else
                if (EmberAffection() < 75)
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) <= 0)
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.GiveLactaidWarning".Localized(EmberMF("He", "She"), EmberMF("His", "Her"), (LittleEmber() ? "Common.Text.FlatChestSwellsSlightly".Localized() : "Common.Text.BreastsSwell".Localized())));

                        Player.DestroyItems(ConsumableIds.Lactaid, 1);
                    }

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.WalksIntoDenTreat".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.PromiseBehaveStrokeBreasts".Localized((LittleEmber() ? "Common.Text.PullEmberEmIntoYour".Localized() : "Common.Text.SeatYourselfInEmberEir".Localized()), (LittleEmber() ? "Common.Text.Tiny".Localized() : "Scenes.Lottie.LottieAppearance.Bountiful".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.EyesWidenNotFast".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.ApologizeThenTease".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.PurrsAtNippleBrush".Localized());

                    if (EmberHasCock())
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.PokeShaftLookUp".Localized());
                    else
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.GroansSquirmsWetness".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.TakeFullResponsibility".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.RumblingPurrVibrates".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.FleshWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.ScalesWordAgain".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.DrinkMoreProduceMore".Localized((LittleEmber() ? "Common.Text.JustBarelySwellingUpFromIts".Localized() : "Common.Text.SloshingWithTheirEverFullLoad".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.ArmsEncircleEmbrace".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.AppetiteDwindlesFull".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.StopsHummingSighs".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.AdmitFullThankHer".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.FaintSmileThenYelp".Localized(
                        LittleEmber() ? "lean back away from the little " + EmberMF("boy", "girl") + "Common.Text.InYourLap".Localized() : "jump out of Ember's lap"
                    ));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.PointOutSeemedEager".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.DrinkDidPlentyGood".Localized());

                    if (Player.HasStatusEffect(StatusEffectIds.DragonBreathCooldown))
                    {
                        Player.RemoveStatusEffect(StatusEffectIds.DragonBreathCooldown);

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.ThroatSoothedAgain".Localized());
                    }

                    Player.RefillHunger(50);

                    if (Random.Shared.Next(2) == 0 && Player.DragonScore() >= 4 && (int)Player.Gender > 0 && ((int)Player.Gender != CoC.Instance.Flags.Get(GameFlagIds.EmberGender) || ((int)Player.Gender == (int)Gender.Hermaphrodite && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)))
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.SuddenSwellAgain".Localized());

                        if (Player.HasCock())
                        {
                            OutputText(Player.CockDescript(0));

                            if (Player.HasVagina())
                                OutputText("Scenes.Emberscene.GetMilkFromEmber.AndWordAgain".Localized());
                        }

                        if (Player.HasVagina())
                            OutputText(Player.VaginaDescript());

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.EmberHadntLeftAgain".Localized());

                        if (Player.HasCock() && EmberHasVagina())
                        {
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.FillWombSeedAgain".Localized());

                            if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                                OutputText("Scenes.Emberscene.GetMilkFromEmber.EvenWhileWordAgain".Localized());
                        }

                        if (Player.HasVagina() && EmberHasCock())
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.HardCockInsideAgain".Localized(
                                Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.TooLateMilkAgain".Localized());

                        if (Player.HasCock() && EmberHasVagina())
                        {
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.RutWordAgain".Localized());

                            Player.GoIntoRut(false);
                        }
                        else
                        {
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.HeatWordAgain".Localized());

                            Player.GoIntoHeat(false);
                        }

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.BoldEndTagAgain".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Emberscene.GetMilkFromEmber.TailWagglesForcedFrown".Localized());

                    if (EmberHasCock())
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.FaintHintOfCock".Localized());

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.EmergingWord".Localized());
                        else
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.HardeningWord".Localized());
                    }
                    else
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.BeadsOnHoneypot".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.HelpDrainingNoMoves".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.PromiseWontTryAnything".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.FrownDeepensFindsSpot".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.WideSmileSeatYourself".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.DontNeedToWonder".Localized((LittleEmber() ? "Common.Text.StrokeHerNearlyFlatChest".Localized() : "Common.Text.SqueezeEmberEirImpressiveBosom".Localized()), (LittleEmber() ? "Common.Text.HappyToHaveSuchAGirlish".Localized() : "Common.Text.SoProudToBeSoBig".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.BlowsPuffOfSmoke".Localized(
                        LittleEmber() ? (silly ? "Common.Text.ImProudAFlatChestIs".Localized() : "Common.Text.BetYouWontFindManyGirls".Localized()) : " I'm proud. Bet you don't see girls with breasts as perfect as mine."
                    ));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.AdmitThenKissTeat".Localized((LittleEmber() ? "Common.Text.ShesRight".Localized() : "Common.Text.YouDont".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.FocusOnPlayingDragon".Localized());

                    if (EmberHasCock())
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.ErectingPrickStroking".Localized(
                            EmberMF("his", "her")
                        ));

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.RidgedAlmostScaly".Localized());

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.SurfaceWord".Localized());
                    }
                    else
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.DrippingSlidesBetweenLips".Localized((LittleEmber() ? "Scenes.Calluscene.OttersForGals.CunnyWord".Localized() : "Common.Text.Cunt".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SentencePeriod".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.GripTightensWhatDoing".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.InnocentLookSuckleEarnest".Localized());

                    if (EmberHasCock())
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.StrokeShaft".Localized());
                    else
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.ProbeSlickDepths".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SquirmsMoansHumming".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.PullsHeadUpKisses".Localized((LittleEmber() ? "Common.Text.LetsYouGo".Localized() : "Common.Text.DropsYou".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.DragonSmilesTriumphant".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SmileAdoptInnocent".Localized((LittleEmber() ? "Common.Text.LookingAtTheMilkDrippingDown".Localized() : "Common.Text.RepositioningYourselfWhereEmberEyDropped".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.FocusOnSimplyDrinking".Localized((LittleEmber() ? "Common.Text.Tiny".Localized() : "Scenes.Lottie.LottieAppearance.Bountiful".Localized())));

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SoftPurrsCaresses".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SwallowsCeaseStomachFills".Localized());

                    if (Player.HasStatusEffect(StatusEffectIds.DragonBreathCooldown))
                    {
                        Player.RemoveStatusEffect(StatusEffectIds.DragonBreathCooldown);

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.ThroatSoothedThird".Localized());
                    }

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.TellDragonFinished".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.SmilesOneBreastLeft".Localized());

                    if (Player.Corruption < 50)
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.CantDisappointHer".Localized(
                            EmberMF("boy", "girl")
                        ));

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.EmbracesTightHug".Localized());

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.MeltsIntoPurringPile".Localized((LittleEmber() ? "Common.Text.SnugglingIntoYourLap".Localized() : "Common.Text.LettingYouSitOnEmberEir".Localized())));

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.WouldntDreamThinking".Localized());

                        if (Player.Toughness > 40)
                            Player.ApplyStats(new DynStat(GameStats.Toughness, -1f));
                    }
                    else
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.StomachGurglesProtest".Localized((LittleEmber() ? "Common.Text.PullingEmberIntoYour".Localized() : "Common.Text.LoweringYourselfIntoEmbers".Localized())));

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.MilkFlowsPopTeat".Localized((LittleEmber() ? "Common.Text.TinyBreast".Localized() : "Common.Text.BreastPointingTheNippleAtThe".Localized()), (LittleEmber() ? ", letting the milk dribble down her childish body" : "")));

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.AdmonishCouldntDrinkAll".Localized());

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.RetortsTearComing".Localized());

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.DrainSecondBreastKiss".Localized((LittleEmber() ? "Scenes.Kihafollowerscene.LovinHugKiha.LeanDown".Localized() : "Common.Text.LiftIt".Localized())));
                    }

                    Player.RefillHunger(50);

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.FlusteredStaresStunned".Localized());

                    OutputText("Scenes.Emberscene.GetMilkFromEmber.WhisperLookingForward".Localized((LittleEmber() ? "Common.Text.SlideEmberEmOutOfYour".Localized() : "Common.Text.RaiseYourselfFromEmberEir".Localized())));

                    if (Random.Shared.Next(2) == 0 && Player.DragonScore() >= 4 && (int)Player.Gender > 0 && ((int)Player.Gender != CoC.Instance.Flags.Get(GameFlagIds.EmberGender) || ((int)Player.Gender == (int)Gender.Hermaphrodite && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)))
                    {
                        OutputText("Scenes.Emberscene.GetMilkFromEmber.SuddenSwellThird".Localized());

                        if (Player.HasCock())
                        {
                            OutputText(Player.CockDescript(0));

                            if (Player.HasVagina())
                                OutputText("Scenes.Emberscene.GetMilkFromEmber.AndWordThird".Localized());
                        }

                        if (Player.HasVagina())
                            OutputText(Player.VaginaDescript());

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.WishEmberHadntThird".Localized());

                        if (Player.HasCock() && EmberHasVagina())
                        {
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.FillWombSeedThird".Localized());

                            if (Player.HasVagina() && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                                OutputText("Scenes.Emberscene.GetMilkFromEmber.EvenWhileThird".Localized());
                        }

                        if (Player.HasVagina() && EmberHasCock())
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.HardCockInsideThird".Localized(
                                Player.VaginaDescript(0)
                            ));

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.TooLateMilkThird".Localized());

                        if (Player.HasCock() && EmberHasVagina())
                        {
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.RutWordThird".Localized());

                            Player.GoIntoRut(false);
                        }
                        else
                        {
                            OutputText("Scenes.Emberscene.GetMilkFromEmber.HeatWordThird".Localized());

                            Player.GoIntoHeat(false);
                        }

                        OutputText("Scenes.Emberscene.GetMilkFromEmber.BoldEndTagThird".Localized());
                    }
                }

            EmberAffection(1);

            if (EmberAffection() < 75)
                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

            Player.ChangeFatigue(-50);

            Player.SlimeFeed();

            Player.HealthChange(Player.MaxHealth * .33f);

            DoNext(() => ReturnToCamp(1));
        }

        public void DecideToSparEmbra()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.DecideToSparEmbra.FeelLikeNeedPractice".Localized());

            if (EmberAffection() <= 25 && EmberSparIntensity() <= 5)
            {
                OutputText("Scenes.Emberscene.DecideToSparEmbra.EmberScoffsJoking".Localized());

                OutputText("Scenes.Emberscene.DecideToSparEmbra.ProveWrongBrandishWeapon".Localized());

                OutputText("Scenes.Emberscene.DecideToSparEmbra.LeadsToIsolatedClearing".Localized());
            }
            else
                if (EmberAffection() < 75 && EmberSparIntensity() <= 15)
                {
                    OutputText("Scenes.Emberscene.DecideToSparEmbra.WarnsWontHoldBack".Localized());

                    OutputText("Scenes.Emberscene.DecideToSparEmbra.NeedStrongBestSpar".Localized());

                    OutputText("Scenes.Emberscene.DecideToSparEmbra.FlatteryWontEarnMercy".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.DecideToSparEmbra.WarnsDontExpectEasy".Localized());

                    OutputText("Scenes.Emberscene.DecideToSparEmbra.GrinBringItOn".Localized());
                }

            var emberEnemy = EnemyRegistry.Create(new Identifier("coc", "ember"));

            emberEnemy.OnDefeated = _ => BeatEmberSpar();

            emberEnemy.OnWon = _ => LoseToEmberSpar();

            CombatScene.Start(emberEnemy, () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void BeatEmberSpar()
        {
            ClearOutput();

            var oldIntensity = EmberSparIntensity();

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.BeatEmberSpar.LiesOnBackDefeated".Localized());

                OutputText("Scenes.Emberscene.BeatEmberSpar.DontNeedHelpCheater".Localized());

                OutputText("Scenes.Emberscene.BeatEmberSpar.WonFairAndSquare".Localized());

                OutputText("Scenes.Emberscene.BeatEmberSpar.WrongStillCheated".Localized());

                OutputText("Scenes.Emberscene.BeatEmberSpar.SoreLoserExpected".Localized());

                OutputText("Scenes.Emberscene.BeatEmberSpar.YellIllWinAgain".Localized());

                OutputText("Scenes.Emberscene.BeatEmberSpar.MaybeNot".Localized());

                EmberAffection(10);
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.BeatEmberSpar.LiesOnBackAgain".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.WonThisTimeLucky".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.HopeBetterFightNext".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.GlareNeverHoldBack".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.StrikePoseGettingBetter".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.SnortsGotLucky".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.PuffsTurnsOnHeels".Localized(
                        EmberMF("He", "She")
                    ));

                    EmberAffection(8);
                }
                else
                {
                    OutputText("Scenes.Emberscene.BeatEmberSpar.LiesOnBackThird".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.AdmitsSomeSkill".Localized());

                    OutputText("Scenes.Emberscene.BeatEmberSpar.PullsCloseWalkBack".Localized());

                    EmberAffection(5);
                }

            CoC.Instance.Flags.Set(GameFlagIds.EmberSparVictories, CoC.Instance.Flags.Get(GameFlagIds.EmberSparVictories) + 1);

            if (EmberSparIntensity() >= 5 && oldIntensity < 5)
                OutputText("Scenes.Emberscene.BeatEmberSpar.DragonBreathUnlock".Localized());

            if (EmberSparIntensity() >= 15 && oldIntensity < 15)
                OutputText("Scenes.Emberscene.BeatEmberSpar.DragonForceUnlock".Localized());

            if (EmberSparIntensity() >= 30 && oldIntensity < 30)
                OutputText("Scenes.Emberscene.BeatEmberSpar.ForceBypassesResolute".Localized());

            if (EmberSparIntensity() >= 45 && oldIntensity < 45)
                OutputText("Scenes.Emberscene.BeatEmberSpar.ForceCooldownReduced".Localized());

            CombatScene.CleanupAfterCombat();
        }

        public void LoseToEmberSpar()
        {
            ClearOutput();

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.LoseToEmberSpar.PantingDropStance".Localized());

                OutputText("Scenes.Emberscene.LoseToEmberSpar.EmberTauntsLoss".Localized());

                OutputText("Scenes.Emberscene.LoseToEmberSpar.QuietVowToWin".Localized());

                OutputText("Scenes.Emberscene.LoseToEmberSpar.DepartureToDen".Localized());

                EmberAffection(-10);
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.LoseToEmberSpar.PantingDropStanceAgain".Localized());

                    OutputText("Scenes.Emberscene.LoseToEmberSpar.BoastsOffersHand".Localized());

                    OutputText("Scenes.Emberscene.LoseToEmberSpar.DefiantConfession".Localized());

                    OutputText("Scenes.Emberscene.LoseToEmberSpar.SmilesPromisesTraining".Localized());

                    EmberAffection(-5);
                }
                else
                {
                    OutputText("Scenes.Emberscene.LoseToEmberSpar.PantingDropStanceThird".Localized());

                    OutputText("Scenes.Emberscene.LoseToEmberSpar.RushesWithConcern".Localized());

                    OutputText("Scenes.Emberscene.LoseToEmberSpar.RefusesHelpFirmly".Localized());

                    OutputText("Scenes.Emberscene.LoseToEmberSpar.SighsHelpsYouUp".Localized((LittleEmber() ? "Common.Text.EasilyDespiteHerSmallBody".Localized() : "Common.Text.Bodily".Localized())));

                    EmberAffection(-5);
                }

            CombatScene.CleanupAfterCombat();

            Player.HealthChange(Player.MaxHealth * .33f);
        }

        public void CatchAnal()
        {
            ClearOutput();

            ShowImage("ember-fucks-your-ass");

            OutputText("Scenes.Emberscene.CatchAnal.SuggestsRiding".Localized());

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.CatchAnal.DisgustedRefusal".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            if (EmberAffection() < 75)
            {
                OutputText("Scenes.Emberscene.CatchAnal.ReluctantAgreement".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1 || CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.CatchAnal.HidingPlaceFragment".Localized());

                OutputText("Scenes.Emberscene.CatchAnal.GrowingSteadilyFragment".Localized());

                OutputText("Scenes.Emberscene.CatchAnal.SmileAtEmber".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.CatchAnal.SwallowsAndAgrees".Localized());

                OutputText("Scenes.Emberscene.CatchAnal.AcceptsObviousLie".Localized());
            }

            OutputText("Scenes.Emberscene.CatchAnal.LeadsToClearing".Localized(
                EmberMF("He", "She")
            ));

            if (Player.Balls > 0)
            {
                OutputText("Scenes.Emberscene.CatchAnal.SwayingTesticles".Localized(
                    EmberMF("him", "her")
                ));

                if (Player.HasVagina())
                    OutputText("Scenes.Emberscene.CatchAnal.WhileWord".Localized());
                else
                    OutputText("Scenes.Emberscene.CatchAnal.SentencePeriod".Localized());
            }

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.CatchAnal.DripsEveryMoment".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.AdmiresNakedBody".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.SettlesBehindYou".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                OutputText("Scenes.Emberscene.CatchAnal.SegmentedWord".Localized());
            else
                OutputText("Scenes.Emberscene.CatchAnal.SmoothWord".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.PrickBetweenCheeks".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.GripsYourButt".Localized(
                EmberMF("His", "Her")
            ));

            OutputText("Scenes.Emberscene.CatchAnal.UnexpectedIntimacy".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.CroonsAfterSlurping".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                OutputText("Scenes.Emberscene.CatchAnal.MouthWord".Localized());
            else
                OutputText("Scenes.Emberscene.CatchAnal.MawWord".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.CockAgainstPucker".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.GrowlsForcesEntry".Localized(
                EmberMF("He", "She")
            ));

            if (Player.Anal.Looseness == 0 || Player.AnalCapacity() < (LittleEmber() ? 7 : 25))
                OutputText("Scenes.Emberscene.CatchAnal.WincingAtInsertion".Localized());

            if (Player.AnalCapacity() < (LittleEmber() ? 24 : 50))
            {
                OutputText("Scenes.Emberscene.CatchAnal.GaspsFromEntry".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                    OutputText("Scenes.Emberscene.CatchAnal.DraconicWord".Localized());

                OutputText("Scenes.Emberscene.CatchAnal.SlippingInsideStretch".Localized());
            }
            else
                OutputText("Scenes.Emberscene.CatchAnal.LoosenessNoResistance".Localized());

            Player.ButtChange(LittleEmber() ? 10 : 32, true, true, false);

            OutputText("Scenes.Emberscene.CatchAnal.SettlesSlowRhythm".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                OutputText("Scenes.Emberscene.CatchAnal.DraconicWordAgain".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.ReachingDeepSpots".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.MoansUnderEmber".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.CatchAnal.ProstatePressure".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Emberscene.CatchAnal.StimulusMakesWet".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Emberscene.CatchAnal.OverBalls".Localized());

                OutputText("Scenes.Emberscene.CatchAnal.PuddleUnderneath".Localized());

                if (Player.HasCock())
                    OutputText("Scenes.Emberscene.CatchAnal.MingledFluidsPool".Localized());
            }

            OutputText("Scenes.Emberscene.CatchAnal.PicksUpPace".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.FurrowsBrowGrowl".Localized(
                EmberMF("He", "She")
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Emberscene.CatchAnal.BallsSlapTogether".Localized(
                    EmberMF("His", "Her")
                ));

            OutputText("Scenes.Emberscene.CatchAnal.ThroatyCryOut".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.ThrustAndGrind".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.EagerDemandsMore".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.DecidesToDominate".Localized());

            if (silly)
                OutputText("Scenes.Emberscene.CatchAnal.SeizesControlBoldly".Localized());
            else
                OutputText("Scenes.Emberscene.CatchAnal.SeizesInitiative".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.TacklesDragonOver".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.SwivelsInLap".Localized());

            if (Player.Pregnancy.Event > 3)
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) == 0)
                    OutputText("Scenes.Emberscene.CatchAnal.SwollenWord".Localized());
                else
                    OutputText("Scenes.Emberscene.CatchAnal.EggLadenWord".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.PistoningUpDown".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.GaspsMidQuestion".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.CheekyRetort".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.GruntsOutPraise".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.GrinsDownSlowly".Localized((LittleEmber() ? "Common.Text.Five".Localized() : "Numbers.Sixteen".Localized())));

            OutputText("Scenes.Emberscene.CatchAnal.AnnouncesClimax".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.KissesDeeply".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.WatchesEmberClimax".Localized(
                EmberMF("His", "Her")
            ));

            OutputText("Scenes.Emberscene.CatchAnal.MoansAsBellySwells".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Emberscene.CatchAnal.CommaSpace".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.CatchAnal.CuntGlazesBelly".Localized());

            if (Player.HasVagina() && Player.HasCock())
                OutputText("Scenes.Emberscene.CatchAnal.AndWord".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Emberscene.CatchAnal.YourCockWord".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Emberscene.CatchAnal.SpurtsOntoTorso".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Emberscene.CatchAnal.SpunkRainsOnDragon".Localized());
                    else
                        OutputText("Scenes.Emberscene.CatchAnal.CreamGlazesLover".Localized());
            }

            OutputText("Scenes.Emberscene.CatchAnal.SentencePeriodAgain".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.HugsCloseAfterglow".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Emberscene.CatchAnal.RubbingOwnCum".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.SinksIntoEmbrace".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.FlusteredOutburst".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.LanguidSmileRecall".Localized());

            OutputText("Scenes.Emberscene.CatchAnal.DemandsABath".Localized());

            EmberAffection(6);

            Player.Orgasm("Anal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f));

            Player.SlimeFeed();

            DoNext(() => ReturnToCamp(1));
        }

        public void SuckEmberCock()
        {
            ClearOutput();

            ShowImage("ember-give-her-a-blowjob");

            OutputText("Scenes.Emberscene.SuckEmberCock.ImaginesCumTaste".Localized());

            if (EmberAffection() <= 25)
                OutputText("Scenes.Emberscene.SuckEmberCock.NotesReluctantYes".Localized());
            else
                if (EmberAffection() < 75)
                    OutputText("Scenes.Emberscene.SuckEmberCock.SuppressesSmile".Localized());
                else
                    OutputText("Scenes.Emberscene.SuckEmberCock.ThrobsBeforeReply".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.FollowsLoverAway".Localized(
                EmberMF("emperor", "empress")
            ));

            OutputText("Scenes.Emberscene.SuckEmberCock.SpreadsLegsWide".Localized(
                EmberMF("He", "She")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                    OutputText("Scenes.Emberscene.SuckEmberCock.GenitalSlitHis".Localized());
                else
                    OutputText("Scenes.Emberscene.SuckEmberCock.GenitalsHis".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                    OutputText("Scenes.Emberscene.SuckEmberCock.TwinSlitsHers".Localized());
                else
                    OutputText("Scenes.Emberscene.SuckEmberCock.GenitalsHers".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.CommaEmberPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                OutputText("Scenes.Emberscene.SuckEmberCock.DraconicWord".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.WaitsForAttention".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.StripsOffArmor".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.SuckEmberCock.HardensAimlessly".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.SuckEmberCock.SeepsWithJuices".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.StammersOpensLegs".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.AffectionateSmileUp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
            {
                OutputText("Scenes.Emberscene.SuckEmberCock.SpringingForthFragment".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                    OutputText("Scenes.Emberscene.SuckEmberCock.FirstWord".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.GenitalSlitWord".Localized());
            }

            OutputText("Scenes.Emberscene.SuckEmberCock.RisesToFullHeight".Localized((LittleEmber() ? "5" : "16")));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                OutputText("Scenes.Emberscene.SuckEmberCock.DragonWord".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.LancingTowardFace".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.LicksGlansFirst".Localized((LittleEmber() ? "Common.Text.TwoOrThreeInchesInside".Localized() : "Common.Text.ThreeOrFourInchesInsideWith".Localized())));

            OutputText("Scenes.Emberscene.SuckEmberCock.ImpatientGoading".Localized());

            if (LittleEmber())
            {
                OutputText("Scenes.Emberscene.SuckEmberCock.ContinuesSwallowing".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.GlanceUpAtEmber".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite && (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0))
                {
                    OutputText("Scenes.Emberscene.SuckEmberCock.PerverseInspiration".Localized());

                    if (Player.Body.Get<TonguePart>().Id.Path != "human")
                        OutputText("Scenes.Emberscene.SuckEmberCock.TongueSlithersDeep".Localized());
                }

                OutputText("Scenes.Emberscene.SuckEmberCock.BegsForCaresses".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.BobsAndSwallows".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                    OutputText("Scenes.Emberscene.SuckEmberCock.TongueWrigglesSlit".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.WantsSpunkInMouth".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.MurmursWithDelight".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.CupsYourCheeks".Localized(
                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.SuckEmberCock.ThrustsWithForce".Localized(
                    EmberMF("He", "She"),

                    EmberMF("He", "She")
                ));

                Player.RefillHunger(50);

                OutputText("Scenes.Emberscene.SuckEmberCock.SpoogeOverflows".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.KneelsAcceptsBath".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.PassionateKissAfter".Localized(
                    EmberMF("He", "She")
                ));
            }
            else
            {
                OutputText("Scenes.Emberscene.SuckEmberCock.SwallowsSixInches".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.WorriesAboutChoking".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.SparksProudDefiance".Localized(
                    EmberMF("His", "Her")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.SuckEmberCock.ScalyWord".Localized());
                else
                    OutputText("Scenes.Emberscene.SuckEmberCock.SoftSkinnedWord".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.LowerBellyWord".Localized());

                if (Player.Pregnancy.Event > 3)
                    OutputText("Scenes.Emberscene.SuckEmberCock.GravidOrbResists".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.ProudOfFullDepth".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.GlanceUpAgain".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.WeakGrinBobbing".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite && (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0))
                {
                    OutputText("Scenes.Emberscene.SuckEmberCock.PerverseInspirationAgain".Localized());

                    if (Player.Body.Get<TonguePart>().Id.Path != "human")
                        OutputText("Scenes.Emberscene.SuckEmberCock.TongueSlithersDeepAgain".Localized());
                }

                OutputText("Scenes.Emberscene.SuckEmberCock.BegsForCaressesAgain".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.BobsAndSwallowsAgain".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                    OutputText("Scenes.Emberscene.SuckEmberCock.TongueWrigglesSlitAgain".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.WantsSpunkInBelly".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.MurmursWithDelightAgain".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.CupsYourCheeksAgain".Localized(
                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.SuckEmberCock.ThrustsWithForceAgain".Localized(
                    EmberMF("He", "She"),

                    EmberMF("He", "She")
                ));

                Player.RefillHunger(50);

                OutputText("Scenes.Emberscene.SuckEmberCock.SpoogeFillsMidriff".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.TooFullToMove".Localized());

                OutputText("Scenes.Emberscene.SuckEmberCock.ShortLivedRelief".Localized(
                    EmberMF("He", "She")
                ));
            }

            OutputText("Scenes.Emberscene.SuckEmberCock.SubmitsToGentleness".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.PuzzledByCuddling".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.ShovesYouAside".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.PointsOutKiss".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.HurriedBathExit".Localized());

            if (Player.Corruption < 80)
                OutputText("Scenes.Emberscene.SuckEmberCock.AmusedWord".Localized());
            else
                OutputText("Scenes.Emberscene.SuckEmberCock.WaryOfBattyWays".Localized());

            OutputText("Scenes.Emberscene.SuckEmberCock.HeadsBackToCamp".Localized());

            Player.SlimeFeed();

            EmberAffection(6);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            DoNext(() => ReturnToCamp(1));
        }

        public void StickDickInKnifeDrawer()
        {
            ClearOutput();

            ShowImage("ember-gives-you-a-blowjob");

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.AloofSuggestion".Localized());

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.HuffsWalksAway".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ArmsCrossedRefusal".Localized());

                    EmberSexMenu(false);

                    return ;
                }
                else
                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ConspiratorialGlance".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.TakesTheLead".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SeatsOnBoulder".Localized());
            else
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.CoilsUpOnTail".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.PeelsOffClothes".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.EyesGlowOnCock".Localized(
                LittleEmber() ? "Common.Text.TheLittle".Localized() + EmberMF("boy", "girl") : "Ember"
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SLetter".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SentencePeriod".Localized());

            var x = Player.BiggestCockIndex();

            var y = x + 1;

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SelectsBiggestCock".Localized(
                    EmberMF("He", "She")
                ));
            else
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.TakesCockInHand".Localized(
                    EmberMF("He", "She"),

                    y
                ));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SniffsYourMusk".Localized());

            if (Player.Cocks[x].Id == BodyPartTypes.Dragon)
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ComplimentsYourCock".Localized());
            else
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.UnsureButWilling".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ShiversAtTouch".Localized((LittleEmber() ? "small " : "")));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ScaledPalmsThrill".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.BlurtsUnderstanding".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.TongueWrapsAround".Localized(
                Player.CockDescript(x),

                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SeesHappyDragon".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.GlowAtFirstTaste".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SucksAndCoos".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.RockHardShaft".Localized(
                    EmberMF("His", "Her")
                ));

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.PoolOfJuices".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SwiftSuckRelease".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.RefusesToStop".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.StrokesHeadSmiling".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) == 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ScalyPateWord".Localized());
            else
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.HairWord".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.HasYourAttention".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.TongueFlicksBalls".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.BallsWord".Localized());
            else
                if (Player.HasVagina())
                    OutputText(Player.VaginaDescript());
                else
                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.CockPlaceholder".Localized(
                        y
                    ));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.LicksBaseToTip".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.LeakingLappedUp".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ClawedHandsWaist".Localized((LittleEmber() ? "Common.Text.TheChildsMouth".Localized() : "Common.Text.EmbersJaws".Localized())));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.UnleashesInMouth".Localized(
                y
            ));

            if (Player.CumQ() < 250)
            {
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.SucksEveryDrop".Localized());

                if (!Player.IsTaur())
                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.FingerWord".Localized());
                else
                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.WingThumbWord".Localized());

                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.TickleProstate".Localized());
            }
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.DrinksAllOffering".Localized(
                        EmberMF("His", "Her")
                    ));
                else
                {
                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.InsistsOnEveryDrop".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                        OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.MouthWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.MawWord".Localized());

                    OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.FearsDrowning".Localized());
                }

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.HappySighHug".Localized());

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.IdleLookDenial".Localized((LittleEmber() ? "cute little " : "")));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ProtestsAndDashes".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.WatchesEmberLeave".Localized());

            if (Player.Corruption < 75)
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.ChucklesAtLove".Localized());
            else
                OutputText("Scenes.Emberscene.StickDickInKnifeDrawer.DementiaWorsening".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void StickItInEmbersButt()
        {
            ClearOutput();

            ShowImage("ember-fuck-her-in-her-buttz");

            OutputText("Scenes.Emberscene.StickItInEmbersButt.EyesOnEmberButt".Localized((LittleEmber() ? "cute little " : "")));

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.StickItInEmbersButt.EmberRefusesNever".Localized(
                    EmberMF("His", "Her")
                ));

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.StickItInEmbersButt.ProtestEwwwDisgust".Localized());

                    EmberSexMenu(false);

                    return ;
                }
                else
                {
                    OutputText("Scenes.Emberscene.StickItInEmbersButt.EyesLockWontFit".Localized());

                    if (EmberHasCock())
                    {
                        OutputText("Scenes.Emberscene.StickItInEmbersButt.StiffeningCock".Localized(
                            EmberMF("his", "her")
                        ));

                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                            OutputText("Scenes.Emberscene.StickItInEmbersButt.AndWord".Localized());
                    }

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female)
                        OutputText("Scenes.Emberscene.StickItInEmbersButt.HerWord".Localized());

                    if (EmberHasVagina())
                        OutputText("Scenes.Emberscene.StickItInEmbersButt.DrippingPussy".Localized());

                    OutputText("Scenes.Emberscene.StickItInEmbersButt.IdeaPartiallyArousing".Localized());
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesButtfuckedEmber) == 0)
                OutputText("Scenes.Emberscene.StickItInEmbersButt.AskIfReallyCertain".Localized());
            else
                OutputText("Scenes.Emberscene.StickItInEmbersButt.AskWhySoScared".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesButtfuckedEmber, CoC.Instance.Flags.Get(GameFlagIds.TimesButtfuckedEmber) + 1);

            OutputText("Scenes.Emberscene.StickItInEmbersButt.MentionOfScaredWord".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.HastenToObeyUndress".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.StickItInEmbersButt.AndVaginaWord".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.UndressedEmberMasturbates".Localized(
                EmberGroinDesc("cock", "pussy")
            ));

            int x = Player.CockThatFits(EmberAnalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Emberscene.StickItInEmbersButt.StrippedPushedToGround".Localized(Player.CockDescript(x), (LittleEmber() ? "Common.Text.LittleHole".Localized() : "Common.Text.Pucker".Localized())));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesButtfuckedEmber) == 1)
            {
                OutputText("Scenes.Emberscene.StickItInEmbersButt.MoansGrowlsNervous".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.NervousGlareDemand".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.SighAdmitsHumiliation".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.SmileReassuringly".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.ReadyExtractsFingers".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.StickItInEmbersButt.MoansGrowlsStretching".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.NoticesSmileStops".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.CuteFirstTimeMemory".Localized(
                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.StickItInEmbersButt.FeelsReadyExtracts".Localized());
            }

            OutputText("Scenes.Emberscene.StickItInEmbersButt.DemandsCompleteControl".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.YouPrefix".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Emberscene.StickItInEmbersButt.PromiseUnderstandInsist".Localized());
            else
                OutputText("Scenes.Emberscene.StickItInEmbersButt.NodWord".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.StepsOverLiftsTail".Localized((LittleEmber() ? "Common.Text.Tiny".Localized() : "Common.Text.Puckered".Localized()), (LittleEmber() ? "childish " : ""), EmberMF("He", "She"), Player.CockDescript(x)));

            OutputText("Scenes.Emberscene.StickItInEmbersButt.MoanAsSwallowsYou".Localized(
                LittleEmber() ? "the little " + EmberMF("boy", "girl") + "Common.Text.SPreteenAsshole".Localized() : "Ember's back passage"
            ));

            OutputText("Scenes.Emberscene.StickItInEmbersButt.ProtestsPullingDown".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.SorryCannotHelpIt".Localized((LittleEmber() ? "Common.Text.CuteAndPetite".Localized() : "Common.Text.FullAndRound".Localized()), (LittleEmber() ? "Common.Text.Firm".Localized() : "Appearance.BreastSize.Luscious".Localized())));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.StickItInEmbersButt.ScalyWord".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.RubbingTailBase".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.AsksMomentToAdjust".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.StickItInEmbersButt.PantingWithLust".Localized());
            else
                OutputText("Scenes.Emberscene.StickItInEmbersButt.BlushAdorningFace".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.ForceSelfRemainStill".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Emberscene.StickItInEmbersButt.MinutesEmberStartsMoving".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.StickItInEmbersButt.PreSlidesDownBalls".Localized());

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.StickItInEmbersButt.PussyDripsRhythmically".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.LustyMoansThrillSpine".Localized(
                EmberMF("His", "Her"),

                Player.CockDescript(x)
            ));

            OutputText("Scenes.Emberscene.StickItInEmbersButt.NotPurringOrMoans".Localized((LittleEmber() ? "" : "wrap around "), (LittleEmber() ? "Common.Text.Young".Localized() : "Common.Text.Inviting".Localized())));

            OutputText("Scenes.Emberscene.StickItInEmbersButt.SurpriseKnocksOnBack".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.AskIfStopNow".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.DontDareStop".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.GiveItEverything".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Emberscene.StickItInEmbersButt.YourBallsWord".Localized());
            else
                OutputText("Scenes.Emberscene.StickItInEmbersButt.SpineBase".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.GroanGoingToCum".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.ScreamsFillWithSeed".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.GiveInToFeeling".Localized((LittleEmber() ? "Common.Text.TheChild".Localized() : "Enemies.Ember.Name".Localized())));

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.StickItInEmbersButt.JetsArcOverHead".Localized());

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.StickItInEmbersButt.GushFromLoveHole".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.OrgasmClenchWaves".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Emberscene.StickItInEmbersButt.SpentButCameMore".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Emberscene.StickItInEmbersButt.OrgasmLastsLonger".Localized());
                else
                    OutputText("Scenes.Emberscene.StickItInEmbersButt.ThunderousOrgasmRips".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.CollapsesBlissedHeap".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.ExtractFromSleeping".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.ScoldsAfterWarning".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.ConfessPartialBlame".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.SnortsPuffSmoke".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.SharesResponsibility".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.PushesWithTail".Localized());

            OutputText("Scenes.Emberscene.StickItInEmbersButt.TossesClothesAround".Localized());

            if (Player.Strength < 70)
            {
                OutputText("Scenes.Emberscene.StickItInEmbersButt.GrabFlyingClothes".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.SentencePeriod".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.ConfidentPuffSmoke".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.SighTemperamentalEmber".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.StickItInEmbersButt.GrabsTailThrowsDown".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.YelpsThrashingStruggling".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.YellsObviouslyMad".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.RefuseGameToday".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.GrowlsButSurprises".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.SighNodAdmission".Localized());

                OutputText("Scenes.Emberscene.StickItInEmbersButt.ShuddersHurriesAway".Localized());
            }

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => ReturnToCamp(1));
        }

        public void SlurpDraggieCunnies()
        {
            ClearOutput();

            ShowImage("ember-eat-out-her-vagoo");

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.OfferKneelPleasure".Localized((LittleEmber() && silly ? "loli " : "")));

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.HidesExcitement".Localized());

                if (!silly)
                    OutputText("Scenes.Emberscene.SlurpDraggieCunnies.WithMeWord".Localized());
                else
                    OutputText("Scenes.Emberscene.SlurpDraggieCunnies.AffectionMeterWord".Localized());

                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.HastilyAddsExcuse".Localized());
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.SlurpDraggieCunnies.ClawsTeaseNetherlips".Localized());

                    if (EmberHasCock())
                        OutputText("Scenes.Emberscene.SlurpDraggieCunnies.SlowlyHardeningShaft".Localized());

                    OutputText("Scenes.Emberscene.SlurpDraggieCunnies.SentencePeriod".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.SlurpDraggieCunnies.SuggestionNearlyJuices".Localized());

                    if (EmberHasCock())
                        OutputText("Scenes.Emberscene.SlurpDraggieCunnies.AndShaftWord".Localized());

                    OutputText("Scenes.Emberscene.SlurpDraggieCunnies.CalmsHerselfDown".Localized());
                }

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.LeadsToSecludedSpot".Localized((LittleEmber() ? "Common.Text.PuffyLittle".Localized() : "Common.Text.BloomingMoist".Localized())));

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.TailSwingingReady".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.SettleBetweenLegs".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.CockJutsTongueSlide".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.GaspsLegsBuckle".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.BlushingWhilePrefix".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.StimulatedPanting".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.LookUpInnocentSmile".Localized((LittleEmber() ? "" : " as best you can around her heaving bosom"), (LittleEmber() ? "" : " This bitch is yours now.")));

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.PreDripsOnHead".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.EnjoymentVeryApparent".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.TongueBendsPrefix".Localized());

            if (Player.Body.Get<TonguePart>().Id.Path != "human")
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.InhumanWord".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.FlexibilityWrigglesDeep".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.GaspsTongueLolling".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.PreRainIntoHair".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.GrabsHeadPushesAway".Localized((LittleEmber() ? "Common.Text.TheLittleDragonGirl".Localized() : "Enemies.Ember.Name".Localized())));

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.SneakUpBehindHer".Localized((LittleEmber() ? "Common.Text.CuteLittle".Localized() : "Common.Text.ShapelyHandfulOf".Localized())));

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.MoanDevolvesToRoar".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.ShaftMarksTreeBark".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.CrumblesLegsGiveWay".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.WaitsThenStrokesCheek".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.LeansIntoKissThen".Localized());

            if (silly)
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.MeterNotThatFull".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.SayEnd".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.AskWhoInitiatesKiss".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.WatchHerGoSmile".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.StiffenedShaft".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.AndWord".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.MoistCunt".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Emberscene.SlurpDraggieCunnies.EmptyCrotch".Localized());

            OutputText("Scenes.Emberscene.SlurpDraggieCunnies.HeadBackToCamp".Localized());

            Player.SlimeFeed();

            EmberAffection(6);

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            DoNext(() => ReturnToCamp(1));
        }

        public void GetEatenOutByEmbra()
        {
            ClearOutput();

            ShowImage("ember-eats-your-vagoo-out");

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.LongTongueIdea".Localized(
                LittleEmber() ? (silly ? "Common.Text.Your".Localized() + EmberMF("shota dragon", "dragon loli") : "Common.Text.TheLittle".Localized() + EmberMF("boy", "girl")) : "Ember"
            ));

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.RefusesGrossDisgust".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (EmberAffection() < 75)
                {
                    OutputText("Scenes.Emberscene.GetEatenOutByEmbra.SuggestsMutualPleasure".Localized());

                    EmberSexMenu(false);

                    return ;
                }
                else
                    OutputText("Scenes.Emberscene.GetEatenOutByEmbra.AgreesIfAskedNicely".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.PolitelyAskPleasure".Localized((Player.IsChild() ? "" : " womanly"), EmberMF("He", "She")));

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.EmberTurnsAppraising".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.ShiversPredatoryGrin".Localized());

            if (!Player.HasTailInsteadOfLegs())
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.LegsSpreadWide".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.DisplayThenInvite".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.CrawlsNoseToSnatch".Localized((LittleEmber() ? "little " : "")));

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.MuskFromDroolingCock".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.SavorsScentLicksLips".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.LicksSlitTopBottom".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.ShudderJuicesDribble".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.PreCumBubblingOut".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.WrigglePraiseBegging".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.NudgesPleasureButton".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.MawWord".Localized());
            else
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.MouthWord".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.KissesContractingBox".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.HowlWritheAsDevours".Localized((LittleEmber() ? "preteen " : "")));

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.ComplianceTongueCervix".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.SpatteringWord".Localized());
            else
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.DrenchingWord".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.FaceWithFeminineHoney".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.CumAllOverDragon".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.SentencePeriod".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.JuicesFlowIntoMouth".Localized());

            if (Player.Wetness() >= 4)
                OutputText("Scenes.Emberscene.GetEatenOutByEmbra.SquirtingSpoilsTask".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.SlumpDownSpent".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.LicksFaceClean".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.HopeNotGetUsed".Localized());

            OutputText("Scenes.Emberscene.GetEatenOutByEmbra.ShakeOffFantasies".Localized(
                LittleEmber() ? "watching the cute little " + EmberMF("boy", "girl") + "Common.Text.SAssAsEmberEyLeaves".Localized() : "hating to see [Ember em] go, but so loving to watch [Ember em] leave"
            ));

            Player.Orgasm("Vaginal");

            DoNext(() => ReturnToCamp(1));
        }

        public void PenetrateEmbrah()
        {
            ClearOutput();

            ShowImage("ember-fuck-her-in-the-vagoo-with-your-penor");

            OutputText("Scenes.Emberscene.PenetrateEmbrah.EyesDrawnToCrotch".Localized());

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrah.LaughsForciblyAway".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (EmberAffection() < 75)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.TurnsAwayLingers".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.SeesEmberMoisten".Localized((LittleEmber() ? "Common.Text.LittleCunny".Localized() : "Common.Text.Vagina".Localized())));

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                        if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                            OutputText("Scenes.Emberscene.PenetrateEmbrah.CockPeekOut".Localized());
                        else
                            OutputText("Scenes.Emberscene.PenetrateEmbrah.CockBeginHardening".Localized());

                    OutputText("Scenes.Emberscene.PenetrateEmbrah.AnticipationWhyAsk".Localized());
                }

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberPussyFuckCount) == 0)
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrah.TailTwistsNervously".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.ReactionSpotOn".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.PromisesGentle".Localized((LittleEmber() ? "Common.Text.TheLittleGirl".Localized() : "Enemies.Ember.Name".Localized())));

                OutputText("Scenes.Emberscene.PenetrateEmbrah.TemperFlipsAgain".Localized());

                if (Player.Corruption > 75)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.SneersAtMaidenClaim".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.ClaimsHonorMaidenhead".Localized());

                if (Player.Corruption <= 75)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.CuteButTroublesome".Localized());
                else
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.IrritatingAsHell".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.WonderIfShouldTease".Localized());
            }

            OutputText("Scenes.Emberscene.PenetrateEmbrah.ScratchesNeckThinking".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.WaitsForInitiative".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.ArriveSmallClearing".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.YouPrefix".Localized());

            if (Player.Corruption < 50)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.HastenUndressImpatient".Localized());
            else
                OutputText("Scenes.Emberscene.PenetrateEmbrah.LanguidlyDenude".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.StandingNakedBeforeHer".Localized());

            int x = 0;

            if (Player.CockThatFits(EmberVaginalCapacity()) == -1)
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrah.GrowlsInExasperation".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.ItDoesntPrefix".Localized());
                else
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.NoneOfThemPrefix".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.FitWord".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.AskIfTooBig".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.KnowMyBodyBetter".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.ThatMonsterInside".Localized((LittleEmber() ? "Common.Text.AnyLittleGirl".Localized() : "Common.Text.Me".Localized())));
                else
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.ThoseMonstersInside".Localized((LittleEmber() ? "Common.Text.One".Localized() : "Common.Text.Any".Localized()), (LittleEmber() ? "Common.Text.AnyLittleGirl".Localized() : "Common.Text.Me".Localized())));

                OutputText("Scenes.Emberscene.PenetrateEmbrah.PuffSmokeFrustration".Localized((LittleEmber() ? "Common.Text.ShedBeSplitInHalf".Localized() : "Common.Text.YoullTearMeApart".Localized())));

                OutputText("Scenes.Emberscene.PenetrateEmbrah.FindWayToShrink".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.ThatWord".Localized());
                else
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.ThoseWord".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.ComeBackThenLeaves".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            x = Player.CockThatFits(EmberVaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            if (Player.HasCockThatFits(EmberVaginalCapacity()) && Player.CockThatFits2(EmberVaginalCapacity()) >= 0)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.ChoosesOneInHand".Localized(
                    Player.CockDescript(x)
                ));

            if (Player.Cocks[x].Id == BodyPartTypes.Dragon)
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrah.AdmiresWonderfulTool".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.NotesFavoriteOne".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.CatchesHerselfSlip".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrah.StrokesToFullMast".Localized(Player.CockDescript(x), (LittleEmber() ? "Common.Text.BothHerTinyHands".Localized() : "Common.Text.HerHand".Localized())));

                OutputText("Scenes.Emberscene.PenetrateEmbrah.AssureKnowsWhatDoing".Localized());
            }

            OutputText("Scenes.Emberscene.PenetrateEmbrah.WaitPatientlyPsyche".Localized(Player.CockDescript(x), (LittleEmber() ? "Common.Text.PuffyMound".Localized() : "Common.Text.MoistenedNetherlips".Localized())));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.DemandsEndForeplay".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.FlamboyantGraceLayDown".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.LicksLipsStraddles".Localized(
                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberPussyFuckCount) == 0)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.TearsHymenHisses".Localized((LittleEmber() ? "Common.Text.ThePreteenGirl".Localized() : "Enemies.Ember.Name".Localized())));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.HiltedProtestsControl".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.CaughtUpThrusting".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.PinsUnderMovesHips".Localized());

            if (EmberHasCock())
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrah.HerWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.DraconicWord".Localized());
                else
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.HumanoidWord".Localized());

                OutputText("Scenes.Emberscene.PenetrateEmbrah.PrickSlapsBelly".Localized());
            }

            OutputText("Scenes.Emberscene.PenetrateEmbrah.SentencePeriod".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.MoansInsertionPurrs".Localized((LittleEmber() ? "Scenes.Kihafollowerscene.BoneTheShitOutofKihaHolesWithHorsecock.FlatAdj".Localized() : "Common.Text.Pillowy".Localized()), (LittleEmber() ? "Common.Text.Little".Localized() : "Common.Text.Ravenous".Localized())));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.StopThrustingGasps".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.PenetrateEmbrah.CockSpurtsPre".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.SentencePeriodAgain".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.InnerWallsMassage".Localized((LittleEmber() && silly ? "Common.Text.LoliHole".Localized() : "Common.Text.InnerWalls".Localized()), (LittleEmber() ? "Common.Text.TinyGirl".Localized() : "Common.Text.Dragon".Localized())));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.OverwhelmedStopsMoving".Localized());

            if (Player.Corruption >= 80)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.NoisyWeakHelpless".Localized());
            else
                OutputText("Scenes.Emberscene.PenetrateEmbrah.WontAcceptThis".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.LustInflamedSurge".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Dragon".Localized())));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.PleadsForHarder".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.MilksYouDry".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.WallsWorkInTandem".Localized((LittleEmber() ? "immature " : "")));
                else
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.ContractionsMilkSperm".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.GroanDeepSatisfaction".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.DribbleWord".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.FlowWord".Localized());
                else
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.GushWord".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.SitOnGroundPanting".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.PenetrateEmbrah.CockEngorgedNotPoured".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.SentencePeriodThird".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.MeetsGazeAllFours".Localized());

            if (Player.Corruption >= 66)
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrah.FoldingArmsBlame".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

                EmberAffection(-5);

                DoNext(() => ReturnToCamp(1));

                CoC.Instance.Flags.Set(GameFlagIds.EmberPussyFuckCount, CoC.Instance.Flags.Get(GameFlagIds.EmberPussyFuckCount) + 1);

                return ;
            }

            OutputText("Scenes.Emberscene.PenetrateEmbrah.SheHasAPoint".Localized((LittleEmber() ? "Scenes.Rubi.RubiAppearance.FlatChest".Localized() : "Scenes.Emberscene.EmbersAppearance.GenerousMounds".Localized())));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.RoarsOverTheEdge".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.CockUnloadsGround".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.PussyViceGrip".Localized((LittleEmber() ? "little " : "")));

            OutputText("Scenes.Emberscene.PenetrateEmbrah.PushDeeperDespite".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Emberscene.PenetrateEmbrah.SeedPastCervix".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.WombInflatesPregnant".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.PenetrateEmbrah.OverworkedPrefix".Localized());

                    if (Player.Balls == 0)
                        OutputText("Scenes.Emberscene.PenetrateEmbrah.PrickIsWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.PenetrateEmbrah.BallsAreWord".Localized());

                    OutputText("Scenes.Emberscene.PenetrateEmbrah.AbsolutelyHugeBelly".Localized());
                }

            OutputText("Scenes.Emberscene.PenetrateEmbrah.SlumpOnBack".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrah.TiredlyAgreesNap".Localized((LittleEmber() ? "Common.Text.YourLittleDragon".Localized() : "Common.Text.Her".Localized())));

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, -2f));

            DoNext(() => PenetrateEmbrahPartII());
        }

        public void PenetrateEmbrahPartII()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.WakesToHoveringFace".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.ScoldsDemandsExcuse".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.YawnDoingAsWanted".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.AddsAnotherComplaint".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.LaughRecallsFinale".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.GrowlsGivesUp".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.RedressWincingBalls".Localized());

            if (Player.CumQ() < 1000)
            {
                OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.MoreCumWhenRecovered".Localized());

                Player.CumMultiplier += 1 + Random.Shared.Next(3);
            }
            else
                OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.CantProduceMore".Localized());

            OutputText("Scenes.Emberscene.PenetrateEmbrahPartII.ReturnToCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.EmberPussyFuckCount, CoC.Instance.Flags.Get(GameFlagIds.EmberPussyFuckCount) + 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void GetPenetratedByEmberLastSexSceneWoooo()
        {
            ClearOutput();

            ShowImage("ember-sticks-her-penor-in-your-vagoo");

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EyesDrawnToLength".Localized((LittleEmber() ? "5" : "16")));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.DragonWord".Localized());
            else
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.Shota.Two".Localized(
                    (LittleEmber() && silly) ? EmberMF("shota", "loli") : "human"
                ));

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.DickBetweenLegs".Localized(
                EmberMF("man", "herm")
            ));

            if (EmberAffection() <= 25)
            {
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberMocksAsLeaves".Localized(
                    LittleEmber() ? " " + EmberMF("He", "She") + "Common.Text.SeemsToBeExaggeratingTheSize".Localized() : ""
                ));

                DoNext(() => ReturnToCamp(1));

                return ;
            }
            else
                if (EmberAffection() < 75)
                    OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberBoastsDragonhood".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberCockJumps".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 0)
                        OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.ToAttention".Localized());
                    else
                        OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.OutOfProtectiveSlit".Localized());

                    OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.MereMentionOfExpectation".Localized());
                }

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YouCooFlirtatiously".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberGraspsArmLeads".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YouStripDownNaked".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Dragon".Localized())));

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberFlinchesTranceBroken".Localized());

            if (Player.Body.Get<TonguePart>().Id == BodyPartTypes.Human)
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YoursWord".Localized());
            else
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YourSinuousMuscle".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.SlitheringIntoThroat".Localized(
                EmberMF("He", "She")
            ));

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.HandsCaressingCock".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberGentlyProbes".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YouGaspThenMoan".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.DraconicWord".Localized());
            else
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.Shota.Two".Localized(
                    (LittleEmber() && silly) ? EmberMF("shota ", "loli ") : "human-like "
                ));

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.CockPastLabia".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
            {
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.TaperedSpearHead".Localized());

                Player.CuntChange(LittleEmber() ? 10 : 32, true, false, true);

                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.ArriveAtCockBase".Localized());
            }
            else
            {
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.MemberDrivesInside".Localized());

                Player.CuntChange(LittleEmber() ? 10 : 32, true, true, false);

                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.LowerTemperatureRelieving".Localized());
            }

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberRemarksHappily".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0)
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberPullsOutSlowly".Localized());
            else
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.MoaningInAgonyEmpty".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.DelightInEmberMotions".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberLeansPanting".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberThrustsDeeply".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YouSnarlBackSoftly".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.FrustratedHornyRolls".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.CockExplodesBelchingCum".Localized());

                if (Player.CumQ() < 250)
                    OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.SpatterWord".Localized());
                else
                    if (Player.CumQ() < 1000)
                        OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.DrenchWord".Localized());
                    else
                        if (Player.CumQ() < 2000)
                            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.CompletelyCoverWord".Localized());
                        else
                            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.UtterlySoakWord".Localized());

                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YourDraconicPartner".Localized());
            }

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.YourOrgasmTriggersEmber".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.SighSoftlyPleasure".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.ActionsNotUnnoticed".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.SmileHungrilySecondRound".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberHissesSensitive".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberGroansGrowsHard".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.EmberSaysPantingExhaustion".Localized());

            if (Player.HasPerk(PerkIds.HistoryWhore) || Player.HasPerk(PerkIds.HistorySlut))
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.VaginaMusclesRipple".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.SecondOrgasmComing".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWoooo.ExhaustedBellyStuffed".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            if (Player.Libido > 50)
                Player.ApplyStats(new DynStat(GameStats.Libido, -3f));

            Player.SlimeFeed();

            DoNext(() => GetPenetratedByEmberLastSexSceneWooooPartII());
        }

        public void GetPenetratedByEmberLastSexSceneWooooPartII()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.WakeBeforeLoverWakes".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Dragon".Localized())));

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.EmberLooksAccusingly".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.ProtectiveSlit".Localized());
            else
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.LimpCockAndBalls".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.UsedLikeFucktoy".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.YouAdmitYouDid".Localized());

            if (EmberAffection() < 75)
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.CockUnlikeAnythingElse".Localized());
            else
                OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.HandleTwoOrThree".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.ExpectedMoreResistance".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.AlmostDehydrated".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.HeaveGreatSighPity".Localized());

            OutputText("Scenes.Emberscene.GetPenetratedByEmberLastSexSceneWooooPartII.EmberEyesWidenTerror".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void EmberRapesYourHeatness()
        {

            OutputText("Scenes.Emberscene.EmberRapesYourHeatness.ScalyClawedHands".Localized());

            OutputText("Scenes.Emberscene.EmberRapesYourHeatness.DeepNeedBurning".Localized());

            if (Player.HasVagina() && Player.InHeat && EmberHasCock())
                OutputText("Scenes.Emberscene.EmberRapesYourHeatness.AchingToBeFilled".Localized());
            else
                OutputText("Scenes.Emberscene.EmberRapesYourHeatness.SeedingRipeWombs".Localized());

            OutputText("Scenes.Emberscene.EmberRapesYourHeatness.SentencePeriod".Localized());

            OutputText("Scenes.Emberscene.EmberRapesYourHeatness.EmberRespondsBy".Localized());

            if (Player.HasVagina() && Player.InHeat && EmberHasCock())
                OutputText("Scenes.Emberscene.EmberRapesYourHeatness.HumpingSmearingThigh".Localized());
            else
                OutputText("Scenes.Emberscene.EmberRapesYourHeatness.PressingDroolingPuffy".Localized((LittleEmber() ? "Scenes.Calluscene.OttersForGals.CunnyWord".Localized() : "Common.Text.Netherlips".Localized())));

            OutputText("Scenes.Emberscene.EmberRapesYourHeatness.NeedYouSoBadly".Localized(
                LittleEmber() ? "Common.Text.TheLittle".Localized() + EmberMF("boy", "girl") : "Ember"
            ));

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            OutputText("Scenes.Emberscene.EmberRapesYourHeatness.WhatDoYouSay".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Accept".Localized(), TimeToPuffTheMagicDragon);

            AddButton(1, "Scenes.Akbal.RepeatAkbalPostSubmission.ButtonDeny".Localized(), FuckOffEmberIWantANap);
        }

        public void FuckOffEmberIWantANap()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Lust, (float)(10 + Player.Libido / 10)));

            OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.OhYourPrefix".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.VaginaWord".Localized());

            if (Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.AndWord".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.EachCock".Localized());

            OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.SoYearnWord".Localized());

            if (Player.Gender == Gender.Male || Player.Gender == Gender.Female)
                OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.SLetter".Localized());

            OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.RefuseEmbersOffer".Localized());

            OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.EmberWhimpersAtWords".Localized());

            OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.SorryNoKids".Localized());

            if (LittleEmber())
                OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.EmberStillAChild".Localized());

            OutputText("Scenes.Emberscene.FuckOffEmberIWantANap.EmberGrowlsSpinsYou".Localized(
                EmberGroinDesc("protruding prick", "creaming wet vagina")
            ));

            EmberAffection(-10);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) < 75 && CoC.Instance.Flags.Get(GameFlagIds.SleepWith) == "Ember")
                CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "");

            Player.CreateStatusEffect(StatusEffectIds.EmberFuckCooldown, 12, 0, 0, 0);

            DoNext(PlayerMenu);
        }

        public void TimeToPuffTheMagicDragon()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.TimeToPuffTheMagicDragon.DazedSniffEmber".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.TimeToPuffTheMagicDragon.EmberPurrsStealingLick".Localized());

            OutputText("Scenes.Emberscene.TimeToPuffTheMagicDragon.BarelyResistInsinuations".Localized(
                LittleEmber() ? "Common.Text.OfTheLittle".Localized() + EmberMF("boy", "girl") : "Ember is"
            ));

            OutputText("Scenes.Emberscene.TimeToPuffTheMagicDragon.AvoidClingingClaws".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.TimeToPuffTheMagicDragon.CantTakeItAnymore".Localized());

            if (Player.Pregnancy.Incubation == 0 && !Player.IsPregnant() && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite && Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Emberscene.TimeToPuffTheMagicDragon.WhoShouldBearKids".Localized());

                ClearButtons();

                AddButton(0, "Enemies.Ember.Name".Localized(), BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding);

                AddButton(1, "Common.Button.You".Localized(), GetKnockedUpByEmbrahBroBaby);
            }
            else
                if (Player.Pregnancy.Incubation == 0 && EmberHasCock() && Player.InHeat)
                    GetKnockedUpByEmbrahBroBaby();
                else
                    BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding();
        }

        public void GetKnockedUpByEmbrahBroBaby()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberGrabsAndRolls".Localized((LittleEmber() ? "Common.Text.SmallBody".Localized() : "Common.Text.Weight".Localized())));

            if (LittleEmber())
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.SmoothFlatChest".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) == 0)
                    OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.HisMuscularChest".Localized());
                else
                    OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.SoftSqueezableBoobs".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.SavoringKissEmboldened".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberNoResistance".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) == 0)
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.LittleDragons".Localized());
            else
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.DragonEggsAgain".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.BeautifulEmberResumesKiss".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.HoldOwnAffectionateLover".Localized((LittleEmber() ? "Common.Text.Preteen".Localized() : "Scenes.Emberscene.PenetrateEmbrah.DraconicWord".Localized())));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) == 0)
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.Dragonlings".Localized());
            else
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.DragonEggsAgain".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.YouWantToBreed".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberHandStrokesCock".Localized());
            else
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberHandsOnThighs".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.GroanThroatilyGrinding".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.DragonEquallyTurnedOn".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.FirstThrustAwkward".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Dragon".Localized())));

            Player.CuntChange(LittleEmber() ? 10 : 20, true, true, false);

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.BedSlickFromLubricant".Localized());
            else
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.ParagraphBreak".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.GrowlThroatilySameNeed".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.NeglectedCockDrooling".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.GodsFeelFillingYou".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberHuffsEachThrust".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.HappyToReceive".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberGrinsFewMinutes".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.SnarlingInFrustration".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.StartledStraddlingDragon".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberLaysBackThrusting".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.DontDignifyWithResponse".Localized((LittleEmber() ? "" : "long, "), Player.Mf("herm", "woman")));

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.PrimalInstinctsCoax".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.WordsHaveDesiredEffect".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.BrutalThrustsRockYou".Localized(
                EmberMF("He", "She")
            ));

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.WetSplatteringFemcum".Localized());
            else
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.ParagraphBreakAgain".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.FreshBatchDragonCum".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.SoCloseNowOrgasm".Localized(
                EmberMF("male", "herm")
            ));

            if (Player.HasCock())
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.CumIntoChestsBellies".Localized());
            else
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.ParagraphBreakThird".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.ContractingWallsMilk".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.KnotSwellingPressure".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.FeelItSteadySeed".Localized((LittleEmber() ? "Common.Text.SmallChild".Localized() : "Common.Text.Dragon".Localized())));

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.EmberSighsRubbingBack".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.MurmursOfficialMates".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.BlushingWord".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.FinalSighLoveConfession".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.KnotAnchorsYouTogether".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) == 0)
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.BabyDragon".Localized());
            else
                OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.DragonEgg".Localized());

            OutputText("Scenes.Emberscene.GetKnockedUpByEmbrahBroBaby.YawningCurlUpNap".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            Player.KnockUp(PregnancyIds.Ember, PregnancyIds.EmberIncubation, 0);

            Player.CreateStatusEffect(StatusEffectIds.EmberFuckCooldown, 36, 0, 0, 0);

            EmberBreedingAfterMathWatchOutForRadioactiveFallout(false);
        }

        public void BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.EmberPussyFuckCount, CoC.Instance.Flags.Get(GameFlagIds.EmberPussyFuckCount) + 1);

            var x = Player.CockThatFits(EmberVaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.EmberCatchesAndRolls".Localized((LittleEmber() ? "Common.Text.TheLittleGirl".Localized() : "Common.Text.She".Localized()), Player.CockDescript(x)));

            if (Player.Balls > 0)
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.ReachesCumFilledOrbs".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.TailTipTeasesVagina".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.ShudderInstinctivelyThrust".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.PurrsDreamilySoSexy".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.JumpsOffInhaleMusk".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.StraddlesPulsingCock".Localized(Player.CockDescript(x), (LittleEmber() ? "Common.Text.TinyCunny".Localized() : "Common.Text.DroolingFuckhole".Localized())));

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.HerCockSpringsUp".Localized());
            else
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.SentencePeriod".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.HissBackSensations".Localized(((EmberChildren() < 1 && LittleEmber()) ? "Common.Text.WhatYouHopeIsARipe".Localized() : "Common.Text.ARipeReadyBreedingHole".Localized())));

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.TakeHoldOfShoulders".Localized((LittleEmber() ? "Common.Text.ThisTinyChild".Localized() : "Common.Text.Her".Localized())));

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.EmberBegsBreasts".Localized((LittleEmber() ? "Common.Text.CuteFlatChest".Localized() : "Common.Text.GreatHeavyBosom".Localized())));

            if (LittleEmber())
            {
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.TinyNipplesStiffening".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                    OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.AndLeakingMilk".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.ScaleCovered".Localized());

                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.OrbsGentlySquish".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                    OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.AndLeakMilk".Localized());
            }

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.UnderYourGrasp".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.EmberMoansChestOut".Localized((LittleEmber() ? "" : " breasts and")));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.TricklingRivuletsMilk".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.PleasedMoansEnough".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.YouHungrilyKissHer".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.DraconicWord".Localized());
            else
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.OhSoHuman".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.NippingAtHerLip".Localized((LittleEmber() ? "" : " Her vibrating breasts massage your hands as you knead, lift, and then roll them about.")));

            if (EmberHasCock())
            {
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.SomethingPokesYourChest".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 || CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 1)
                    OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.DragonWord".Localized());

                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.MischievousCurlOfLips".Localized());
            }

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.EmberSquealsIntoKiss".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.CockSpurtsJetsCum".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.EyesRollBackHead".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.CockWetsCabinSide".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.BreaksKissAndPants".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.InstinctivelyBiteNeck".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.TongueLollsSubmission".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.SpentCockThrob".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.EyesShutSecondOrgasm".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.LittleGobsOfCum".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.TailCoilsAroundYou".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.WhispersSlumpingAtop".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.CloseJustLittleMore".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.RepliesTiredlyBounce".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.NoProtestJustMoans".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.FinallyDriveOverEdge".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.CuntSplashingDownLegs".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.GushingIntoReadyWomb".Localized(Player.CockDescript(x), (LittleEmber() ? "Scenes.ErlKing.HowDareYouII.LittleGirl".Localized() : "Common.Text.Dragon".Localized())));

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.GrowlsPurrsHotSeed".Localized((LittleEmber() ? "" : " her")));

            if (Player.CumQ() < 250)
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.WallsSuckCumOut".Localized());
            else
                if (Player.CumQ() < 1000)
                    OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.HelpingCumReachesWomb".Localized());
                else
                    OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.DumpHugeLoadJism".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.GaspSighUtterRelief".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.CumSlickWord".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.BellyRegainStrength".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.DismissiveOfSpentCock".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.HeatInHerGut".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.PurrsPullsIntoKiss".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.BreakingKissWhispers".Localized());

            OutputText("Scenes.Emberscene.BreedEmberPregnantAsIfThereWasAnyOtherKindOfBreeding.FiguringStillHaveTime".Localized());

            Player.KnockUp(PregnancyIds.PlayerFather, PregnancyIds.EmberIncubation, 0);

            Player.CreateStatusEffect(StatusEffectIds.EmberFuckCooldown, 36, 0, 0, 0);

            Player.RemoveStatusEffect(StatusEffectIds.Rut);

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f));

            EmberBreedingAfterMathWatchOutForRadioactiveFallout(true);
        }

        public void EmberBreedingAfterMathWatchOutForRadioactiveFallout(bool emberPregged = true)
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.WakeFeelingReplenished".Localized(
                LittleEmber() ? "Common.Text.ATinyPreteen".Localized() + EmberMF("boy", "girl") : "your draconic lover"
            ));

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.SpotEmberGuarding".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.HappyGlanceNoticesAwake".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.ThankForCleaning".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.EmberCheeks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.BlushingUnderScales".Localized());
            else
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.CheeksBlush".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.NotYourPersonalMaid".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.WonderIfPointOut".Localized());

            if (!emberPregged)
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.TouchYourBelly".Localized());
            else
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.PointAtEmberBelly".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.AskIfItTook".Localized(((LittleEmber() && EmberChildren() == 0) ? " despite [Ember eir] age" : "")));

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.PuffsChestBoasts".Localized());

            if (emberPregged)
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.PumpedIntoMe".Localized());
            else
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.PumpedIntoYou".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.TrailsOffTouchesCrotch".Localized());

            if (emberPregged)
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.SlitWord".Localized());
            else
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.CockWord".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.RememberingIntenseSex".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.YouWoreOutFirst".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.EmberPossessivePrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.BlushDeepens".Localized());
            else
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.DiscomfortDeepens".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.DeepensAndFrowns".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.StifleLaughWantKnow".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.IndignantPuffSmoke".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.NodUnderstandingComment".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.EmberWord".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.BlushesWord".Localized());
            else
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.FrownsWord".Localized());

            OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.YellsIndignantlyRushesAway".Localized());

            if (!emberPregged)
                OutputText("Scenes.Emberscene.EmberBreedingAfterMathWatchOutForRadioactiveFallout.TouchBellyWithSmirk".Localized());

            DoNext(PlayerMenu);
        }

        public bool EmberPregUpdate()
        {

            switch (Player.Pregnancy.Event)
            {
                case 1:
                {
                OutputText("Scenes.Emberscene.EmberPregUpdate.BellySwellingSeedTook".Localized((LittleEmber() ? "Scenes.ErlKing.HowDareYouII.LittleGirl".Localized() : "Common.Text.Dragon".Localized())));

                return true;
                }

                case 2:
                {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0)
                    OutputText("Scenes.Emberscene.EmberPregUpdate.BellyGrowsEverBigger".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberPregUpdate.SwollenMidriffSuitsHer".Localized());

                Player.ApplyStats(new DynStat(GameStats.Lust, (float)(5 + Player.Libido / 20)));

                return true;
                }

                case 4:
                {
                OutputText("Scenes.Emberscene.EmberPregUpdate.BellyGrownQuiteBit".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0)
                    OutputText("Scenes.Emberscene.EmberPregUpdate.NoticesLookingAsks".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Woman".Localized())));
                else
                    OutputText("Scenes.Emberscene.EmberPregUpdate.CatchesYouLooking".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1 ? " and blushes" : "")));

                return true;
                }

                case 5:
                {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0)
                    OutputText("Scenes.Emberscene.EmberPregUpdate.GroanSitFeelEgg".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.EmberPregUpdate.SittingDownRubbingBelly".Localized());

                    OutputText("Scenes.Emberscene.EmberPregUpdate.RespondYesFeelBaby".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 ? "Common.Text.Scales".Localized() : "Common.Text.Skin".Localized())));

                    OutputText("Scenes.Emberscene.EmberPregUpdate.FeelSmallKick".Localized());
                }

                return true;
                }

                case 6:
                {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0)
                    OutputText("Scenes.Emberscene.EmberPregUpdate.NeverStopsGrowingEgg".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.EmberPregUpdate.MoodyAsBellyBig".Localized());

                    OutputText("Scenes.Emberscene.EmberPregUpdate.QuestionsYouGlaring".Localized());

                    OutputText("Scenes.Emberscene.EmberPregUpdate.HastenToHelpTasks".Localized());
                }

                return true;
                }

                case 7:
                {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0)
                    OutputText("Scenes.Emberscene.EmberPregUpdate.TiredAsksBellyRub".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.EmberPregUpdate.LessActiveHeavyBelly".Localized());

                    OutputText("Scenes.Emberscene.EmberPregUpdate.FetchWaterRequest".Localized());
                }

                return true;
                }

                case 8:
                {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0)
                {
                    OutputText("Scenes.Emberscene.EmberPregUpdate.MilkLeakingNub".Localized(
                        (LittleEmber() ? "Common.Text.FlatChestHasSwollenUpInto".Localized() : "Common.Text.BreastsLookBloated".Localized()),

                        (LittleEmber() ? "Common.Text.RubbingTheMilkyLumps".Localized() : "Common.Text.LiftingHerMilkyJugsAndLetting".Localized()),

                        (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? ", a blush forming on her cheeks at her request" : "")
                    ));

                    OutputText("Scenes.Emberscene.EmberPregUpdate.HappyToObligeDrinking".Localized());

                    Player.RefillHunger(40, true);

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                    {
                        OutputText("Scenes.Emberscene.EmberPregUpdate.BreastsAlwaysFullMilk".Localized());

                        OutputText("Scenes.Emberscene.EmberPregUpdate.MoveToOtherBreast".Localized());

                        OutputText("Scenes.Emberscene.EmberPregUpdate.AdmitsThenStifleBurp".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? ", blushing softly" : "")));
                    }
                    else
                    {
                        OutputText("Scenes.Emberscene.EmberPregUpdate.SoonExhaustedOneBreast".Localized());

                        OutputText("Scenes.Emberscene.EmberPregUpdate.CommentsYouSmileBack".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Emberscene.EmberPregUpdate.DecideToCheckUp".Localized((LittleEmber() ? "Common.Text.Swollen".Localized() : "Common.Text.Heavy".Localized()), (LittleEmber() ? "Common.Text.WhatWasOnceAFlatPlane".Localized() : "Common.Text.TheyreSoSwollenTheyreAtLeast".Localized()), (LittleEmber() ? " milk" : ""), (LittleEmber() ? "" : "gently lift up one of her milk-bloated breasts, ")));

                    OutputText("Scenes.Emberscene.EmberPregUpdate.SighWatchSheSleeps".Localized());

                    Player.RefillHunger(40, false);
                }

                Player.ChangeFatigue(-25);

                return true;
                }
            }

            return false;
        }

        public void EmberGivesBirth()
        {

            int childGender = 0;

            if (Player.Pregnancy.allowHerm && Random.Shared.Next(100) < 20)
                childGender = (int)Gender.Hermaphrodite;
            else
                childGender = 1 + Random.Shared.Next(2);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) == 0)
            {
                OutputText("Scenes.Emberscene.EmberGivesBirth.RoarInterruptsRoutine".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.BabyComing".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.InstinctivelyGrabHand".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.YellsInPain".Localized((LittleEmber() ? "Common.Text.TheLittleGirl".Localized() : "Enemies.Ember.Name".Localized())));

                OutputText("Scenes.Emberscene.EmberGivesBirth.WrenchHandFree".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.ScalesWord".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.SkinWord".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.WombWorksToExpel".Localized());

                if (EmberHasCock())
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 0)
                        OutputText("Scenes.Emberscene.EmberGivesBirth.HumanCockDangles".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberGivesBirth.DraconicCockPushed".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.LickWithTongue".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.TheoryWasCorrect".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.TowelReadyImminent".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.EmberYellsOblivious".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.StruggleAndWrestle".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HeadCrowning".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.MuzzleTipJutting".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.KnowNoTime".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.EmberCollapsesPanting".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.AnthroDragonWord".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HybridOfHumanDragon".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.BeautifulStrongHealthy".Localized());

                var __sw0_1 = childGender;

                if (__sw0_1 == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.BoyWord".Localized());

                if (__sw0_1 == (int)Gender.Female)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.GirlWord".Localized());

                if (__sw0_1 == (int)Gender.Hermaphrodite)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HermWord".Localized());

                if (true )
                    OutputText("Scenes.Emberscene.EmberGivesBirth.ErrorMessage".Localized(
                        childGender
                    ));

                OutputText("Scenes.Emberscene.EmberGivesBirth.SentencePeriod".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.CradlePrefix".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HimWord".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HerWord".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.InYourArmsUncaring".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HimWordAgain".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HerWordAgain".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.GrinProudly".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.SonWord".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.DaughterWord".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.HeldOutForHer".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.GazesAtNewborn".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.PassNewbornToEmber".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HisWord".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.HerWordThird".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.MothersLovingArms".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.StrokeHerCheek".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.HelpHerDown".Localized());

                if (EmberChildren() > 1)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.OffspringPeekIn".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.ContentedExpression".Localized());

                if (EmberChildren() <= 1)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.NewChildWord".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.ExpandedDraconicBrood".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.RestLeavingDen".Localized());

                var __sw1_1 = childGender;

                if (__sw1_1 == (int)Gender.Male)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenMales, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenMales) + 1);

                if (__sw1_1 == (int)Gender.Female)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenFemales, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + 1);

                if (__sw1_1 == (int)Gender.Hermaphrodite)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenHerms, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) + 1);

                Save.SetInt("newbornGender", childGender);

                Save.SetInt("birthTime", View.Time.Days);
            }
            else
            {
                OutputText("Scenes.Emberscene.EmberGivesBirth.EggRoarInterrupts".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.EggLayingTime".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.InstinctivelyGrabHandEgg".Localized((LittleEmber() ? "Common.Text.TheLittleGirl".Localized() : "Common.Text.Her".Localized())));

                OutputText("Scenes.Emberscene.EmberGivesBirth.EmberYellsInPain".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.WrenchHandFreeEgg".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.EmberGivesBirth.ScalesWordAgain".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberGivesBirth.SkinWordAgain".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.WombWorksEgg".Localized());

                if (EmberHasCock())
                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) == 0)
                        OutputText("Scenes.Emberscene.EmberGivesBirth.HumanCockDanglesEgg".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberGivesBirth.DraconicCockPushedEgg".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.LickWithTongueEgg".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.TheoryWasCorrectEgg".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.TowelReadyForEgg".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.EmberYellsObliviousEgg".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.StruggleWrestleEgg".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.CantPayAttention".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.EmberPantsTired".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) > 0)
                {
                    OutputText("Scenes.Emberscene.EmberGivesBirth.AlongWithThePrefix".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) == 1)
                        OutputText("Scenes.Emberscene.EmberGivesBirth.OtherWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.EmberGivesBirth.OthersWord".Localized(
                            NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.EmberEggs))
                        ));
                }

                OutputText("Scenes.Emberscene.EmberGivesBirth.TellHerBeautiful".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.EmberSmilesHooksTail".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.AskWhatThatIs".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.KnowingLookSuckle".Localized());

                OutputText("Scenes.Emberscene.EmberGivesBirth.AllowHerToDecide".Localized((LittleEmber() ? "Common.Text.Underage".Localized() : "Common.Text.Dragon".Localized())));

                OutputText("Scenes.Emberscene.EmberGivesBirth.DrainBreastsDrinking".Localized());

                Player.RefillHunger(40);

                OutputText("Scenes.Emberscene.EmberGivesBirth.WakeUpLater".Localized());

                if (Player.Pregnancy.allowHerm)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberHermEggs, CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) + 1);
                else
                    CoC.Instance.Flags.Set(GameFlagIds.EmberEggs, CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + 1);

                var eggList = Save.GetIntList("eggArray");

                eggList.Add(336);

                Save.SetIntList("eggArray", eggList);
            }

            Player.CreateStatusEffect(StatusEffectIds.EmberNapping, 12, 0, 0, 0);
        }

        public bool UpdateVaginalPregnancy()
        {

            var displayedUpdate = false;

            var pregText = "";

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) > 0)
            {
                if (Player.Pregnancy.Incubation == 330)
                    pregText = "Your belly has swollen, becoming larger - proof that Ember's seed did its work. The dragon seems to be constantly checking you out, as if looking for the signs of weight gain.";

                if (Player.Pregnancy.Incubation == 250)
                    pregText = "Your belly grows ever bigger, making your pregnancy noticeable; your belly also feels somewhat solid. Ember casts pleased glances in your direction, whenever [Ember ey] thinks you're not looking.";

                if (Player.Pregnancy.Incubation == 170)
                {
                    pregText = "You've grown a lot. Anyone is able to tell that you're pregnant with a single glance; and by the shape, you have no doubt that there's an egg in your womb; a big one.";

                    if (Player.Corruption < 40)
                        pregText += " Part of you didn't really want to get knocked up, but it's for a good cause. Besides, Ember looks very cute, trying to hide [Ember eir] happiness whenever [Ember ey] glances at your belly...";
                    else
                        if (Player.Corruption < 75)
                            pregText += " Considering the size of the egg, you hope it doesn't hurt when your child comes out. You hope Ember will help you through this.";
                        else
                            pregText += " You think dreamily about the wild sex that helped conceive this little one. Ember is such a great fuck" + (LittleEmber() ? " at such a young age" : "") + "Common.Text.ReallyYoureDoingThisWorldA".Localized();
                }

                if (Player.Pregnancy.Incubation == 120)
                    pregText = "Though you're sure that this is the time when a regular baby would start moving about, your own belly simply sits there, heavy and full. You'd be worried if you didn't remember that Ember hatched from an egg. Sometimes; a delightful, refreshing, chill spreads from your belly throughout your body; making you feel invigorated, ready for anything.";

                if (Player.Pregnancy.Incubation == 90)
                    pregText = "You've somehow grown even larger, the egg's outline appearing through your tummy. By now, you're quite bothered with how difficult it's getting to move. Ember constantly shadows you around the camp, making sure you're all right, although if you ever question [Ember em] [Ember ey]'ll just say you're both going in the same direction.";

                if (Player.Pregnancy.Incubation == 60)
                {
                    pregText = "The egg inside your belly seems to grow heavier each day that passes. ";

                    if (Player.Corruption < 40)
                        pregText += "It's quite a burden that you're carrying. Still, it's a worthwhile sacrifice to make in order to restore Ember's race.";
                    else
                        if (Player.Corruption < 75)
                            pregText += "You wonder how much longer you have to wait. This egg is quite burdensome. Part of you is scared of its size, the other part is delighted to have produced such a big egg.";
                        else
                            pregText += "You're eager to give birth, just so you can get impregnated again. Particularly because that means more wild sex with " + (LittleEmber() ? "Common.Text.YourUnderageDragon".Localized() : "Enemies.Ember.Name".Localized()) + ".";
                }

                if (Player.Pregnancy.Incubation == 30)
                {
                    pregText = "You rub your hands over your ripe belly, lost in the sensations of motherhood. ";

                    Player.ApplyStats(new DynStat(GameStats.Sensitivity, 5f), new DynStat(GameStats.Lust, (float)(5 + Player.Sensitivity / 20)));

                    if (Player.Corruption < 40)
                        pregText += "Despite your initial reluctance, you've come to find a very real pleasure in being pregnant. You hope Ember will want to have more children in the future...";
                    else
                        if (Player.Corruption < 75)
                            pregText += "You smile, knowing you'll have your egg in your hands the next few days. A part of you is almost sad that you'll be empty, but you can always entice Ember into getting you pregnant again.";
                        else
                        {
                            pregText += "You find yourself daydreaming about giving birth, your belly swollen huge - bigger than it currently is - and the orgasmic sensation of many large, round eggs sliding out of your [vagina].\n\nYou start to absently rub yourself as you envision eggs by the dozens coming from within you; you shall be mothergod for a whole new race of dragons...";

                            Player.ApplyStats(new DynStat(GameStats.Lust, 35f));
                        }

                    pregText += "\n\nEmber interrupts your musings with a question. [say: How are you feeling? Do you need me to get you anything?]";

                    pregText += "\n\nThe dragon's question is uncharacteristic of [Ember em]. Still, you do appreciate the attention you're getting, and so you ask Ember to fetch you some food and water. The speed with which Ember dashes off to fulfill your requests is truly impressive! In short moments Ember is back with a piece of roasted meat and a skin of water.";

                    pregText += "\n\nAs you eat and drink your fill, Ember uses one wing to shield you off the sun. You're starting to really enjoy all the attention, but seeing Ember give up on [Ember eir] usual antics is still very weird.";
                }
            }
            else
            {
                if (Player.Pregnancy.Incubation == 330)
                    pregText = "Your belly is a bit swollen - either you're eating too much or Ember's seed really did the job.";

                if (Player.Pregnancy.Incubation == 250)
                    pregText = "Your belly grows ever bigger, making your pregnancy noticeable. Ember shoots you quick looks, trying to hide [Ember eir] smirk of success every time [Ember ey] does. You smirk right back at [Ember em], and occasionally make a subtle show of your gravid form, just to see [Ember em] get turned on by the sight.";

                if (Player.Pregnancy.Incubation == 170)
                {
                    pregText = "You've grown a lot, anyone is able to tell that you're pregnant with a single glance. ";

                    if (Player.Corruption < 40)
                        pregText += "Part of you didn't really want to get knocked up. However, Ember's look of satisfaction whenever [Ember ey] gazes your way is rewarding despite that. Plus, it is for a good cause. You smirk in satisfaction - with a couple of dragons at your beck and call, things will look very different indeed.";
                    else
                        if (Player.Corruption < 75)
                            pregText += "You grin, savoring the strange, erotic sensations from the life inside your burgeoning womb and the promise of motherhood. Mmm, if it feels this good, maybe you should [say: encourage] Ember to get you pregnant again.";
                        else
                            pregText += "You think dreamily about the wild sex that helped conceive this little one. Ember is such a great fuck" + (LittleEmber() ? " at such a young age" : "") + "Common.Text.ReallyYoureDoingThisWorldA".Localized();
                }

                if (Player.Pregnancy.Incubation == 120)
                {
                    pregText = "Every once in awhile, you feel a kick from inside your bulging belly. Right now, it's really kicking up a storm, and so you decide to sit down and take it easy. You keep rubbing your belly, hoping to calm your child down and make it stop battering your innards.";

                    pregText += "[pg]Ember approaches you, and casually asks, [say: So... is it kicking already?]";

                    pregText += "[pg]You admit that it is, stroking your stomach. Casually, you ask if Ember would maybe like to touch your belly, wondering if [Ember ey] will be able to bring [Ember em]self to do it.";

                    pregText += "[pg][say: Yes! Of course!] Ember replies";

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1)
                        pregText += ", blushing at [Ember eir] own over-enthusiastic reply";

                    pregText += ". You just smile encouragingly at the dragon " + EmberMF("boy", "herm") + "Common.Text.AndLeanBackSlightlyStickingOut".Localized() + EmberMF("father", "mother") + "Common.Text.ToTryAndConnectWithEmber".Localized();

                    pregText += "[pg]Ember sets a clawed hand on your belly, careful not to hurt you with [Ember eir] claws. Slowly [Ember ey] rubs your belly, until [Ember ey] feels a small kick and smiles in glee. You smile at the look of joy on [Ember eir] face, even as [Ember ey] realizes what [Ember ey]'s doing and embarrassedly mumbles an excuse and walks away.";
                }

                if (Player.Pregnancy.Incubation == 90)
                {
                    pregText = "You stop for a moment and sit down on a nearby rock. Your belly feels much heavier than usual, and just walking about has become a chore. Ember takes notice of your tiredness and quickly closes the distance between you two. [say: [name], are you feeling all right?]";

                    pregText += "[pg]You tell [Ember em] that you are, just worn out. It's not easy carrying [Ember eir] child, after all.";

                    pregText += "[pg]Ember sighs in relief. [say: Good, is there anything I can do for you?]";

                    pregText += "[pg]You tap your lips thoughtfully, mulling it over. ";

                    if (Player.Corruption <= 33)
                        pregText += "There really isn't anything you feel like you need right now... maybe some water? Or maybe you could have Ember help you to your " + CampSleep.HomeDesc() + "Common.Text.ForAQuickRest".Localized();
                    else
                        if (Player.Corruption <= 66)
                            pregText += "You wonder if you should take advantage of Ember - you've certainly been feeling a little on edge lately, and besides [Ember ey] did say 'anything'. You ponder this for a while longer.";
                        else
                            pregText += "You already thought up a perfect way for this sexy dragon" + (LittleEmber() ? " child" : "") + "Common.Text.ToHelpYouButItsBest".Localized();

                    pregText += "[pg]Finally, you decide there really isn't anything Ember can help you with, and tell [Ember em] so. Though [Ember ey] had better be ready to do [Ember eir] part when the baby is born and needs caring.";

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                        pregText += " You can't resist smirking and patting one of your shemale dragon's " + (LittleEmber() ? "Common.Text.Little".Localized() : "Scenes.Lottie.LottieAppearance.Bountiful".Localized()) + " breasts, noting that maybe you should let him do all the breast-feeding.";

                    pregText += "[pg]";

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                        pregText += "Ember blushes. ";

                    pregText += "[say: O-of course I'll do my part. If you don't need me for anything, I'll be going then.] " + EmberMF("He", "She") + "Common.Text.TurnsOnEmberEirHeelsAnd".Localized();
                }

                if (Player.Pregnancy.Incubation == 60)
                {
                    pregText = "Besides being so huge you'd probably be asked if you were having twins back in Ingnam, your belly has grown stupidly heavy, ";

                    if (Player.Corruption <= 33)
                        pregText += "making you wonder more than ever if it really was a good idea to get pregnant with a dragon. True, Ember looks ready to burst with pride at your fruitful bounty, but you feel ready to just plain burst yourself.";
                    else
                        if (Player.Corruption <= 66)
                            pregText += "and you wonder how much longer you have to wait. Despite being a bit bothersome, you're pleased your child is growing into a healthy, hopefully sexy, dragon; like its father.";
                        else
                            pregText += "and you're eager to give birth, so you can get impregnated again. Particularly because that means more rowdy fucking from Ember.";
                }

                if (Player.Pregnancy.Incubation == 30)
                {
                    pregText = "You rub your hands over your gloriously full, ripe belly, lost in the sensations of motherhood. ";

                    if (Player.Corruption <= 33)
                        pregText += "Despite your initial reluctance, you've come to find a very real pleasure in being pregnant. You hope Ember will want to have more children in the future.";
                    else
                        if (Player.Corruption <= 66)
                            pregText += "You smile, knowing you'll meet your child in the next few days. A part of you is almost sad that you'll be empty, but you can always entice Ember into getting you pregnant again.";
                        else
                            pregText += "You find yourself daydreaming about being the revered mother-queen of a huge army of dragons, visions of magnificent, sexy, scaly beasts sweeping across the land conquering it in your honor, offering up tribute to the ever-ripe womb that brought them forth; rolling around, as the musk of their fucking fills the air. The image is so delicious you don't want to wake up from your fantasy.";
                }
            }

            if (pregText != "")
            {
                OutputText("Scenes.Emberscene.UpdateVaginalPregnancy.ParagraphBreakPlaceholder".Localized(
                    pregText
                ));

                displayedUpdate = true;
            }

            return displayedUpdate;
        }

        public void VaginalBirth()
        {

            int childGender = 0;

            if (Player.Pregnancy.allowHerm && Random.Shared.Next(100) < 20)
                childGender = (int)Gender.Hermaphrodite;
            else
                childGender = 1 + Random.Shared.Next(2);

            OutputText("Scenes.Emberscene.VaginalBirth.ParagraphBreak".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberOviposition) == 0)
            {
                ShowImage("birth-ember-live");

                if (!Player.HasVagina())
                {
                    OutputText("Scenes.Emberscene.VaginalBirth.PainfulRippingFeeling".Localized());

                    Player.CreateVagina();
                }

                OutputText("Scenes.Emberscene.VaginalBirth.DoublingOverHowl".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberHeftsToDen".Localized(
                    EmberMF("-boy", "-girl")
                ));

                OutputText("Scenes.Emberscene.VaginalBirth.GritTeethSore".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.FetchCleanCloths".Localized(
                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.VaginalBirth.LowersHeadToVagina".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Emberscene.VaginalBirth.SlowPushBallsPrefix".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Emberscene.VaginalBirth.SlowPushCocksPrefix".Localized());

                if (Player.Balls > 0 || Player.HasCock())
                    OutputText("Scenes.Emberscene.VaginalBirth.OutOfWaySuffix".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.BlowsOnLoveHole".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.MoanPleasureAndPain".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberDigsDeeper".Localized());

                Player.CuntChange(80, true, true, false);

                OutputText("Scenes.Emberscene.VaginalBirth.StrainWithAllMight".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Emberscene.VaginalBirth.DeliciouslyWord".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.BoundaryPleasurePain".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.WithdrawTongueScream".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.LastHollowGroan".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.FaintlyHearEmber".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.CloseEyesPassOut".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberPointsToChest".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.HeWord".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.SheWord".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.HungryDidntKnow".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                {
                    OutputText("Scenes.Emberscene.VaginalBirth.TriedToFeedPrefix".Localized());

                    if (childGender == (int)Gender.Male)
                        OutputText("Scenes.Emberscene.VaginalBirth.HimWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.VaginalBirth.HerWord".Localized());

                    OutputText("Scenes.Emberscene.VaginalBirth.MyselfButSuffix".Localized());

                    if (childGender == (int)Gender.Male)
                        OutputText("Scenes.Emberscene.VaginalBirth.HeWordAgain".Localized());
                    else
                        OutputText("Scenes.Emberscene.VaginalBirth.SheWordAgain".Localized());

                    OutputText("Scenes.Emberscene.VaginalBirth.WantedYoursSuffix".Localized());
                }

                OutputText("Scenes.Emberscene.VaginalBirth.SighStrokeNewborn".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberSighsSmiles".Localized());

                var __sw0_1 = childGender;

                if (__sw0_1 == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.BoyWord".Localized());

                if (__sw0_1 == (int)Gender.Female)
                    OutputText("Scenes.Emberscene.VaginalBirth.GirlWord".Localized());

                if (__sw0_1 == (int)Gender.Hermaphrodite)
                    OutputText("Scenes.Emberscene.VaginalBirth.HermWord".Localized());

                if (true )
                    OutputText("Scenes.Emberscene.VaginalBirth.ErrorMessage".Localized(
                        childGender
                    ));

                OutputText("Scenes.Emberscene.VaginalBirth.SayEndAfterPeriod".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.SmileAtBaby".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.HisWord".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.HerWordAgain".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.FaceCriesSoftly".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.HimWordAgain".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.HerWordThird".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.OntoShoulderPat".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.HimWordThird".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.HerWordFourth".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.BackBetweenSuffix".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.HisWordAgain".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.HerWordFifth".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.LittleWingsBurp".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.HimWordFourth".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.HerWordSixth".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.FeelingBetterSuffix".Localized());

                if (childGender == (int)Gender.Male)
                    OutputText("Scenes.Emberscene.VaginalBirth.HeWordThird".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.SheWordThird".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.CoosGigglesNuzzles".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberOffersToTend".Localized());

                var __sw1_1 = childGender;

                if (__sw1_1 == (int)Gender.Male)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenMales, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenMales) + 1);

                if (__sw1_1 == (int)Gender.Female)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenFemales, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + 1);

                if (__sw1_1 == (int)Gender.Hermaphrodite)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenHerms, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) + 1);

                Save.SetInt("newbornGender", childGender);

                Save.SetInt("birthTime", View.Time.Days);
            }
            else
            {
                ShowImage("birth-ember-egg");

                if (!Player.HasVagina())
                {
                    OutputText("Scenes.Emberscene.VaginalBirth.PainfulRippingEgg".Localized());

                    Player.CreateVagina();
                }

                OutputText("Scenes.Emberscene.VaginalBirth.DoublingOverHowlEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberEggToDen".Localized(
                    EmberMF("-boy", "-girl")
                ));

                OutputText("Scenes.Emberscene.VaginalBirth.GritTeethSoreEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.FetchCleanClothsEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.LowersHeadEgg".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Emberscene.VaginalBirth.SlowPushBallsEgg".Localized());
                else
                    if (Player.HasCock())
                        OutputText("Scenes.Emberscene.VaginalBirth.SlowPushCocksEgg".Localized());

                if (Player.Balls > 0 || Player.HasCock())
                    OutputText("Scenes.Emberscene.VaginalBirth.OutOfWayEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.BlowsOnLoveEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.MoanPleasurePainEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberDigsDeeperEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.StrainForEgg".Localized());

                if (Player.HasPerk(PerkIds.Masochist))
                    OutputText("Scenes.Emberscene.VaginalBirth.DeliciouslyWordEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.BoundaryPleasureEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.WithdrawTongueEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.LastHollowGroanEgg".Localized());

                Player.CuntChange(80, true, true, false);

                OutputText("Scenes.Emberscene.VaginalBirth.FaintlyHearEmberEgg".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.WatchEmberTend".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.NodWearilyLieBack".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.UncertainSleepLength".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.EmberWord".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                    OutputText("Scenes.Emberscene.VaginalBirth.BlushesAndPrefix".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.WithdrawsLickingLips".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.OnlyPoliteToAsk".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.LayBackEnjoyIt".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.TellItsFine".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) > 0)
                    OutputText("Scenes.Emberscene.VaginalBirth.SHatchSuffix".Localized());
                else
                    OutputText("Scenes.Emberscene.VaginalBirth.HatchesWord".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.AnywayTheDragon".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                    OutputText("Scenes.Emberscene.VaginalBirth.BlushesAndThenPrefix".Localized());

                OutputText("Scenes.Emberscene.VaginalBirth.ScurriesAway".Localized());

                if (Player.Pregnancy.allowHerm)
                    CoC.Instance.Flags.Set(GameFlagIds.EmberHermEggs, CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) + 1);
                else
                    CoC.Instance.Flags.Set(GameFlagIds.EmberEggs, CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + 1);

                var eggList = Save.GetIntList("eggArray");

                eggList.Add(336);

                Save.SetIntList("eggArray", eggList);
            }

            Player.CreateStatusEffect(StatusEffectIds.EmberNapping, 5, 0, 0, 0);
        }

        public void EmberHatchEggs(int egg)
        {
            ClearOutput();

            RegisterMacro("children", EmberChildren() > 1 ? "children" : (CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenMales) > 0 ? "Common.Text.Son".Localized() : "Common.Text.Daughter".Localized()));

            int childGender = 0;

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) > 0 && Random.Shared.Next(100) < 20)
                childGender = (int)Gender.Hermaphrodite;
            else
                childGender = 1 + Random.Shared.Next(2);

            RegisterMacro("kid.he", childGender == 1 ? "Common.Text.He".Localized() : "Common.Text.She".Localized());

            RegisterMacro("kid.him", childGender == 1 ? "Common.Text.Him".Localized() : "Common.Text.Her".Localized());

            RegisterMacro("kid.his", childGender == 1 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized());

            RegisterMacro("dragonkid", childGender == 1 ? (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? "Common.Text.DragonBoy".Localized() : "Common.Text.Dragon".Localized()) : (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? "Common.Text.DragonGirl".Localized() : "Common.Text.Dragoness".Localized()));

            int currentKid = 0;

            if (EmberChildren() <= CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenMales))
                currentKid = 1;
            else
                currentKid = Random.Shared.Next(CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms)) <= CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) ? 2 : 3;

            RegisterMacro("rand.he", currentKid == 1 ? "Common.Text.He".Localized() : "Common.Text.She".Localized());

            RegisterMacro("rand.him", currentKid == 1 ? "Common.Text.Him".Localized() : "Common.Text.Her".Localized());

            RegisterMacro("rand.his", currentKid == 1 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized());

            if (EmberChildren() > 0)
                OutputText("Scenes.Emberscene.EmberHatchEggs.ChildWaitsImpatiently".Localized((EmberChildren() > 1 ? "One of y" : "Y"), EmberMF("father", "mother")));
            else
                OutputText("Scenes.Emberscene.EmberHatchEggs.EmberCriesYourName".Localized());

            OutputText("Scenes.Emberscene.EmberHatchEggs.EyesAdjustToDark".Localized((EmberChildren() > 0 ? "both of " : "")));

            OutputText("Scenes.Emberscene.EmberHatchEggs.SquealEchoesWalls".Localized((EmberChildren() > 0 ? "Common.Text.ToTakeCareOfAnotherLittle".Localized() : "Common.Text.ToBeAFather".Localized())));

            OutputText("Scenes.Emberscene.EmberHatchEggs.MustBeYou".Localized(
                EmberChildren() > 1 ? "Common.Text.ButItsHardToSeeAnything".Localized() : "Common.Text.ThoughYoudScarcelyNoticeAnyChange".Localized() + (EmberChildren() > 0 ? "Common.Text.YourChildrenSittingBesideItEagerly".Localized() : "Common.Text.EmberPointingItOut".Localized()),

                Player.Mf("a mother's instinct", Player.DragonScore() < 4 ? "Common.Text.ADragonsInstinct".Localized() : "Common.Text.Instinctual".Localized())
            ));

            OutputText("Scenes.Emberscene.EmberHatchEggs.WaitWithWigglingEmber".Localized(
                EmberChildren() > 0 ? "Even your [children] grow" + (EmberChildren() > 1 ? "" : "s") + " still and can only watch, enraptured by the birth of " + (EmberChildren() > 1 ? "Common.Text.Their".Localized() : "Common.Text.RandHis".Localized()) + " new sibling." : ""
            ));

            OutputText("Scenes.Emberscene.EmberHatchEggs.HairlineCracksSpread".Localized());

            OutputText("Scenes.Emberscene.EmberHatchEggs.FreedFromEgg".Localized(
                childGender == 3 ? ", though she clearly has her " + (Player.IsHerm() && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite ? "parents'" : EmberMF("mother's", "father's")) + "Common.Text.ExtraEndowmentAsWell".Localized() : "",

                EmberChildren() > 0 ? "Common.Text.LovingFamily".Localized() : "Common.Text.Parents".Localized(),

                Player.DragonScore() < 4 ? "apparently unconcerned that you look " + (Player.DragonScore() == 0 ? "Common.Text.Nothing".Localized() : "Common.Text.Little".Localized()) + " like [kid.him]" : "and the resemblance to [kid.his] draconic kin is obvious",

                childGender == 1 ? "Common.Text.Son".Localized() : "Common.Text.Daughter".Localized()
            ));

            OutputText("Scenes.Emberscene.EmberHatchEggs.PerfectSaysYouAgree".Localized((EmberChildren() > 0 ? "Your " + (EmberChildren() > 1 ? "other " : "") + "[children] scramble" + (EmberChildren() > 1 ? "" : "s") + " around the two of you, eager to meet " + (EmberChildren() > 1 ? "Common.Text.Their".Localized() : "Common.Text.RandHis".Localized()) + " new " + (childGender == 1 ? "Common.Text.Brother".Localized() : "Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Sister".Localized()) + ", but" : ""), (EmberChildren() > 0 ? "first" : ""), EmberMF("father", "mother")));

            if (EmberChildren() > 0)
                OutputText("Scenes.Emberscene.EmberHatchEggs.HoldsUpChild".Localized((childGender == 1 ? "Common.Text.Brother".Localized() : "Scenes.Ceraphfollowerscene.BarkOrMeowTwiceToCeraph.Sister".Localized()), (EmberChildren() > 1 ? "s" : ""), (EmberChildren() > 1 ? "Common.Text.TheyCrowd".Localized() : "Common.Text.RandHeCrowds".Localized()), (EmberChildren() > 1 ? "Common.Text.Their".Localized() : "Common.Text.RandHis".Localized()), EmberMF("father", "mother"), (EmberChildren() > 1 ? ", pushing each other aside to be the first to hold your newest " + (childGender == 1 ? "Common.Text.Son".Localized() : "Common.Text.Daughter".Localized()) : ", anxious to touch [rand.his] newest family member for the first time")));

            OutputText("Scenes.Emberscene.EmberHatchEggs.SmileInfectious".Localized(
                EmberChildren() > 0 ? "manages to break away from your " + (EmberChildren() > 1 ? "other " : "") + "Common.Text.ChildrenLongEnoughToStep".Localized() : "steps",

                childGender == 1 ? "Common.Text.Boy".Localized() : "Common.Text.Girl".Localized()
            ));

            OutputText("Scenes.Emberscene.EmberHatchEggs.PromiseToReturn".Localized());

            OutputText("Scenes.Emberscene.EmberHatchEggs.SmileIsEnough".Localized());

            var __sw0_0 = childGender;

            if (__sw0_0 == (int)Gender.Male)
                CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenMales, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenMales) + 1);

            if (__sw0_0 == (int)Gender.Female)
                CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenFemales, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + 1);

            if (__sw0_0 == (int)Gender.Hermaphrodite)
                CoC.Instance.Flags.Set(GameFlagIds.EmberChildrenHerms, CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) + 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) != 0)
                CoC.Instance.Flags.Set(GameFlagIds.EmberHermEggs, CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) - 1);
            else
                CoC.Instance.Flags.Set(GameFlagIds.EmberEggs, CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) - 1);

            var eggList = Save.GetIntList("eggArray");

            if (egg >= 0 && egg < eggList.Count)
                eggList.RemoveAt(egg);

            Save.SetIntList("eggArray", eggList);

            Save.SetInt("newbornGender", childGender);

            Save.SetInt("birthTime", View.Time.Days);

            DoNext(PlayerMenu);
        }

        public void EmberPlayerFeeds()
        {
            ClearOutput();

            RegisterMacro("kid.he", Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.He".Localized() : "Common.Text.She".Localized());

            RegisterMacro("kid.him", Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.Him".Localized() : "Common.Text.Her".Localized());

            RegisterMacro("kid.his", Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized());

            RegisterMacro("dragonkid", Save.GetInt("newbornGender", 0) == 1 ? (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? "Common.Text.DragonBoy".Localized() : "Common.Text.Dragon".Localized()) : (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? "Common.Text.DragonGirl".Localized() : "Common.Text.Dragoness".Localized()));

            OutputText("Scenes.Emberscene.EmberPlayerFeeds.WakingWhisperedName".Localized());

            if (LittleEmber() && !Save.GetBool("learnedFeeding"))
            {
                OutputText("Scenes.Emberscene.EmberPlayerFeeds.OnlyWantsFinger".Localized());

                OutputText("Scenes.Emberscene.EmberPlayerFeeds.KnowWhatKidNeeds".Localized());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) == 0)
                {
                    OutputText("Scenes.Emberscene.EmberPlayerFeeds.KidIsHungry".Localized());

                    OutputText("Scenes.Emberscene.EmberPlayerFeeds.ReliefFlashes".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.EmberPlayerFeeds.SmilesInApology".Localized());

                    OutputText("Scenes.Emberscene.EmberPlayerFeeds.OnlyFair".Localized());
                }

            OutputText("Scenes.Emberscene.EmberPlayerFeeds.TakeKidInArms".Localized());

            if (LittleEmber() && !Save.GetBool("learnedFeeding"))
            {
                OutputText("Scenes.Emberscene.EmberPlayerFeeds.EmberSettlesWatching".Localized());

                OutputText("Scenes.Emberscene.EmberPlayerFeeds.AsksToDoThat".Localized());

                OutputText("Scenes.Emberscene.EmberPlayerFeeds.QuicklyTurnsAway".Localized());

                OutputText("Scenes.Emberscene.EmberPlayerFeeds.ParagraphBreakPlaceholder".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) == 0 ? "Common.Text.UnfortunatelyNotYouHaveToTell".Localized() + Player.Mf(" You're not sure this situation could be any more awkward, considering you're not one yourself, and you're more than relieved when [ember ey] doesn't press you on it.", "") : "It couldn't hurt to try, you tell [ember em], and Ember nods in understanding."
                ));

                Save.SetBool("learnedFeeding", true);
            }
            else
            {
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) == 0)
                    OutputText("Scenes.Emberscene.EmberPlayerFeeds.WatchingChildFeed".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.EmberPlayerFeeds.CuddlesAgainstYou".Localized(
                        Player.DragonScore() >= 4 ? "Common.Text.SayWithSuchAStrongMate".Localized() : "Common.Text.SayImSurprisedKidHeLikes".Localized(),

                        Player.DragonScore() < 4 ? "[say: Must get that from me...]" : ""
                    ));

                    OutputText("Scenes.Emberscene.EmberPlayerFeeds.LoverNestlesWarmth".Localized());
                }

                OutputText("Scenes.Emberscene.EmberPlayerFeeds.TryToExplainThanks".Localized());

                OutputText("Scenes.Emberscene.EmberPlayerFeeds.MightBeTrue".Localized());
            }

            OutputText("Scenes.Emberscene.EmberPlayerFeeds.CompletelyDrained".Localized());

            OutputText("Scenes.Emberscene.EmberPlayerFeeds.FlopDownOnBed".Localized());

            Player.Milked();

            View.Time.PassTime((int)View.Time.LastPassedHours);
        }

        public void EmberBreastfeeding()
        {
            ClearOutput();

            RegisterMacro("kid.he", Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.He".Localized() : "Common.Text.She".Localized());

            RegisterMacro("kid.him", Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.Him".Localized() : "Common.Text.Her".Localized());

            RegisterMacro("kid.his", Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.His".Localized() : "Common.Text.Her".Localized());

            RegisterMacro("dragonkid", Save.GetInt("newbornGender", 0) == 1 ? (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? "Common.Text.DragonBoy".Localized() : "Common.Text.Dragon".Localized()) : (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0 ? "Common.Text.DragonGirl".Localized() : "Common.Text.Dragoness".Localized()));

            OutputText("Scenes.Emberscene.EmberBreastfeeding.EmberNotWaiting".Localized(
                (LittleEmber() || EmberIsMasculine() ? "Common.Text.AMostUnexpectedSightEmber".Localized() : "Common.Text.Her".Localized()),

                (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) > 0 ? "beside [ember eir] newest egg," : ""),

                EmberMF("his distinctly unmasculine chest", "her heavy breasts"),

                (Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.Boy".Localized() : "Common.Text.Girl".Localized()),

                EmberMF("father", "mother")
            ));

            if (LittleEmber())
            {
                OutputText("Scenes.Emberscene.EmberBreastfeeding.CallsOutPanic".Localized(
                    EmberMF("father", "mother")
                ));

                OutputText("Scenes.Emberscene.EmberBreastfeeding.OfferToTakeKid".Localized());
            }
            else
                OutputText("Scenes.Emberscene.EmberBreastfeeding.BeckonsYouCloser".Localized(
                    (LittleEmber() ? "60" : "80"),

                    (LittleEmber() ? "40" : "60"),

                    (Save.GetInt("newbornGender", 0) == 1 ? "Common.Text.Son".Localized() : "Common.Text.Daughter".Localized())
                ));

            OutputText("Scenes.Emberscene.EmberBreastfeeding.KeepsGettingBigger".Localized());

            if (LittleEmber())
                OutputText("Scenes.Emberscene.EmberBreastfeeding.AvoidThatPath".Localized((EmberHasHair() ? "Common.Text.ThroughEmberEirHair".Localized() : "Common.Text.AlongEmberEirSmoothScales".Localized())));

            OutputText("Scenes.Emberscene.EmberBreastfeeding.RememberOldEmber".Localized(
                EmberMF("father", "mother")
            ));

            OutputText("Scenes.Emberscene.EmberBreastfeeding.AttentionDrawnDown".Localized((LittleEmber() ? "52" : "78"), EmberMF("father", "mother")));

            OutputText("Scenes.Emberscene.EmberBreastfeeding.LeaningIntoWarmth".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) > 0 ? ", beside [kid.his] sibling-to-be" : "")));

            OutputText("Scenes.Emberscene.EmberBreastfeeding.GreetingWhatToday".Localized());

            EmberCampMenu(false);
        }

        public void EmberKidsMenu()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberKidsMenu.KidsWithEmber".Localized(
                EmberChildren() != 0 ? NumberUtils.Num2Text(EmberChildren()) + "Common.Text.Child".Localized() + (EmberChildren() > 1 ? "ren" : "") + (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) != 0 ? " and " : "") : "",

                CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) > 0 ? NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs)) + "Common.Text.Eggs".Localized() : ""
            ));

            ClearButtons();

            if (EmberChildren() != 0)
                AddNextButton("Common.Button.TuckIn".Localized(), EmberTuckIn);

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberEggs) + CoC.Instance.Flags.Get(GameFlagIds.EmberHermEggs) != 0)
                AddNextButton("Common.Button.Egg".Localized(), EmberEggAppearance);

            AddButton(14, "Common.Button.Back".Localized(), () => EmberCampMenu());
        }

        public void EmberTuckIn()
        {
            ClearOutput();

            RegisterMacro("kids", EmberChildren() > 1);

            RegisterMacro("dson", CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) > 0 ? "Common.Text.Daughter".Localized() : "Common.Text.Son".Localized());

            RegisterMacro("dboy", CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) > 0 ? "Common.Text.Girl".Localized() : "Common.Text.Boy".Localized());

            RegisterMacro("dhe",  CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) > 0 ? "Common.Text.She".Localized() : "Common.Text.He".Localized());

            RegisterMacro("dhim", CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) > 0 ? "Common.Text.Her".Localized() : "Common.Text.Him".Localized());

            RegisterMacro("dhis", CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenFemales) + CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenHerms) > 0 ? "Common.Text.Her".Localized() : "Common.Text.His".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.YoungDragonYawning".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.FiercelyShakesHead".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.AskWhatFirst".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.ScalpStrokedRelaxing".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.FoundAnyBugs".Localized(
                EmberMF("Daddy", "Mommy")
            ));

            OutputText("Scenes.Emberscene.EmberTuckIn.GetsIntoStory".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.InnocentlyRecline".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.FinalForeheadKiss".Localized());

            OutputText("Scenes.Emberscene.EmberTuckIn.BreathingDeepEnough".Localized());

            Player.ChangeFatigue(10);

            Save.SetInt("tuckedToday", View.Time.Days);

            DoNext(() => ReturnToCamp(2));
        }

        public void EmberEggAppearance()
        {
            ClearOutput();

            RegisterMacro("children", EmberChildren() > 1 ? "children" : (CoC.Instance.Flags.Get(GameFlagIds.EmberChildrenMales) > 0 ? "Common.Text.Son".Localized() : "Common.Text.Daughter".Localized()));

            OutputText("Scenes.Emberscene.EmberEggAppearance.EmberWatchesCurious".Localized((EmberChildren() > 0 ? ", your [children] in tow" : ""), (EmberChildren() > 0 ? "All" + (EmberChildren() == 1 ? " three" : "") : "Both")));

            OutputText("Scenes.Emberscene.EmberEggAppearance.PeacefulNestledEgg".Localized());

            OutputText("Scenes.Emberscene.EmberEggAppearance.PullsYouClose".Localized(
                Player.IsPregnant() || Player.IsPregnant() ? "[say: And then we're going to have another...]" : ((int)Player.Gender != CoC.Instance.Flags.Get(GameFlagIds.EmberGender) && (int)Player.Gender != 3 ? "Common.Text.SayButWeCouldGetStarted".Localized() : "Common.Text.SayIHopeOneDayWell".Localized())
            ));

            DoNext(() => EmberKidsMenu());
        }

        public void HighAffectionEmberLustFuck()
        {
            ClearOutput();

            var x = Player.CockThatFits(EmberVaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StripAndWatchEmber".Localized(
                EmberMF("his", "her")
            ));

            if (EmberHasCock())
            {
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.DragonCockWord".Localized());

                if (EmberInternalDick())
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PokingOutOfSlit".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.GrowingErect".Localized());
            }

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AndWord".Localized());

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HerPussyDripping".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SentencePeriod".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.NowThatsAView".Localized(
                Player.Mf("man", "girl")
            ));

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.WonderfulCockSingular".Localized());
            else
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.WonderfulCocksPlural".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.YoureAllMine".Localized(
                EmberMF("guy", "girl")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ThinkingOfProperFucking".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StaresClearlyDumbstruck".Localized(
                EmberMF("He", "She"),

                EmberMF("he'd", "she'd"),

                EmberGroinDesc("stiffness of [Ember eir] cock", "the slickness of her pussy"),

                LittleEmber() ? (silly ? EmberMF("shota ", "loli ") : "little ") : ""
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesEmberLustyFucked) == 0)
            {
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SmileTakeOfferedBook".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AsksMoreSpecific".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StateAllOfThem".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.IncredulousStareGreets".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SureYouCanPerform".Localized());

                if (Player.Libido >= 50 || Player.Libido >= 40)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ConfidentInLibido".Localized());
                else
                {
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StaminaWontProblem".Localized());

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));
                }
            }
            else
            {
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LaughsAtAllPoses".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.NodInAgreement".Localized());

                if (Player.Libido < 50 && Player.Libido < 40)
                {
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.GrabLustDraftVial".Localized());

                    Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.LustDraft));
                }

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SentencePeriodAgain".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.TimesEmberLustyFucked, CoC.Instance.Flags.Get(GameFlagIds.TimesEmberLustyFucked) + 1);

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.DragonsDutySateMate".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.FollowFeelingHarder".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.EmberLaysComfortable".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
            {
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PosturesForFemaleDragon".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LeanBesideEmbarrassedDragon".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SmilesUpAtYou".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PositionAlignShaft".Localized(
                    Player.CockHead(x)
                ));

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PleasesStopTeasing".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LookIntoEyesSmile".Localized(
                    Player.CockDescript(x)
                ));

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.MoansFeelsSoFull".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HopeNotTooSore".Localized());
            }
            else
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HopesBodyPleasing".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.RunHandOverAss".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ThumbDrippingWet".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ImpatientWantsCock".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ChuckleAlignEntrance".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SlideInchByInch".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.YourPrefix".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SheDragonWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.DragonGirlWord".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.MoansThroatilyRapturous".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HappyToHearFeelings".Localized());
                }
                else
                {
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.EyesFixatedOnCocks".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.BothWord".Localized());
                    else
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.TwoWord".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.NotExactlyPartPose".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.BlinkIsSheImplying".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.GigglesDoublyStuffed".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PleadingEyesCute".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PecksLipsLookDown".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SlickFingersPushInside".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StiffnessOfHerDick".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ObviousSheIsReady".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.UrgeToSavorIt".Localized(
                        Player.CockDescript(x),

                        Player.CockDescript(x)
                    ));

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CriesBodyQuivering".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CockDribblingPrecum".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SentencePeriodThird".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.FeelingOfHavingPrefix".Localized());

                    if (Player.CockTotal() == 2)
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.BothWordAgain".Localized());
                    else
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.TwoOfPrefix".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.EnvelopedSlickTightness".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.YourPrefixAgain".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SheDragonWordAgain".Localized());
                    else
                        OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.DragonGirlWordAgain".Localized());

                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.MoansThroatilyAgain".Localized());
                }

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LegsOverShoulders".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HumpingSlowThenFast".Localized(
                EmberMF("his butt", "her soft folds"),

                EmberMF("his ass", "her cunt")
            ));

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AndAssWord".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.RipplingSqueezingMember".Localized());

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetter".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SentencePeriodFourth".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.RedoubleEffortsPistoning".Localized());

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.VaginaWord".Localized());
            else
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AssWord".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PoundingItDeserves".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.GaspsPleadingForCum".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PenetrateOnceTwiceHilt".Localized(
                Player.CockDescript(x),

                EmberMF("his", "her"),

                EmberGroinDesc("tight ass", "slick pussy")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HowlsSteamingLoad".Localized(
                EmberMF("His", "Her")
            ));

            if (EmberHasVagina())
            {
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CuntShuddersFemcum".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AndHerPrefix".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CommaSpace".Localized());
            }

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CockEruptsDraconicCum".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.GaspingClimaxEbbs".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PantInUnison".Localized());

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterAgain".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StreamOfWhiteMat".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterThird".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StillFlaggingAbove".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CroonsBrushingCheek".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CaressTailWrapsIt".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AssWordAgain".Localized());
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PussyWord".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HolesWord".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SquelchFeelsSoGood".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.InstructsGrabBelly".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SteadyRhythmMoans".Localized(
                EmberMF("He", "She")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AssWordThird".Localized());
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PussyWordAgain".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HolesWordAgain".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.WetSquelchingHips".Localized(
                EmberMF("guy", "princess")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.DrivingMoreIntensity".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SomethingPopsInside".Localized());

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterFourth".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LetsOutExultantCry".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AssSqueezesWord".Localized());
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PussySqueezesWord".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HolesSqueezeWord".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.MilkingYourPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ShaftAsCockSpurts".Localized());
            else
            {
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CuntSpasmsWetly".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.TwinShaftsCuntSpasms".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HerCockSpurtsSeed".Localized());

                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SentencePeriodFifth".Localized());
            }

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.GroansMoansLikeWhore".Localized());

            if (Player.Libido >= 50 || Player.Libido >= 40)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.NotQuiteSatisfiedYet".Localized(
                    EmberGroinDesc("[ember Eir] cock growing hard again", "her cunt starting to drip")
                ));
            else
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.NotGottenFuckingDeserves".Localized(
                    EmberGroinDesc("[ember eir] cock begins to grow erect again", "her cunt starts to drip with moisture"),

                    EmberMF("He", "She")
                ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CrawlsGrippingShaft".Localized());

            if (Player.CockTotal() > 1 && EmberHasVagina())
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterFifth".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.StrokesTenderlyFirmly".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ThisWord".Localized());
            else
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.TheseWord".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.IntoxicatingYourScent".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 1 || CoC.Instance.Flags.Get(GameFlagIds.EmberHair) > 0)
            {
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.RufflingPrefix".Localized(
                    EmberMF("his ", "her ")
                ));

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) >= 2)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ManeWord".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HairWord".Localized());
            }

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LeansHandHuggingMidriff".Localized(
                EmberMF("He", "She")
            ));

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterSixth".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.BrushAgainstCheek".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SmilesOpensMouth".Localized());

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.MingledPrefix".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CoilingAroundLikeSnake".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.DragonBeginsSuckingOff".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SlurpsSucksLoudly".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AsksIfLikesBlowjob".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.GripsButtSpreadCheeks".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.DepthsWord".Localized());
            else
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.NethersWord".Localized());

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AndDepthsWord".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.HookArmUnderKnee".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AssWordFourth".Localized());
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PussyWordThird".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LoveHolesWord".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.TongueLollsPants".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.MoansMakeWhoreBlush".Localized(
                EmberMF("His", "Her")
            ));

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.BothYourDicksWord".Localized());
            else
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.YourDickWord".Localized());

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.MoistureDrenchingCock".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CockThrobsPulsates".Localized(
                    EmberMF("His", "Her")
                ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.WobblingBecomesDependent".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ReleaseLegGrabMidriff".Localized(
                EmberGroinDesc("[ember eir] cock fountaining seed across the wall, practically whitewashing it in steaming hot dragon-spunk", "her cunt gushing female ejaculate, almost soaking you with the cascade of feminine fluids and puddling wetly against your [feet]")
            ));

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.OrgasmForcesYours".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.AssWordFifth".Localized());
            else
                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.PussyWordFourth".Localized());
                else
                    OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.LoveHolesWordAgain".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.ExcessSlidesOut".Localized());

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterSeventh".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.WhispersNearlyPassingOut".Localized());

            if (EmberHasVagina() && Player.CockTotal() > 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterEighth".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SlideWord".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.SLetterNinth".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.WetSlurpDepositLeaks".Localized());

            OutputText("Scenes.Emberscene.HighAffectionEmberLustFuck.CatchBreathRollOver".Localized(
                EmberMF("He", "She")
            ));

            Player.Orgasm("Generic");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -5f));

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), StayWithEmberAfterLustFuck);

            AddButton(1, "Common.Button.No".Localized(), NoStayingForCuddlesPostLustFuck);
        }

        public void NoStayingForCuddlesPostLustFuck()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.NoStayingForCuddlesPostLustFuck.CantStayMustGo".Localized(
                EmberMF("He", "She")
            ));

            if (Player.Corruption < 50)
                OutputText("Scenes.Emberscene.NoStayingForCuddlesPostLustFuck.ThanksUnderstanding".Localized());
            else
                OutputText("Scenes.Emberscene.NoStayingForCuddlesPostLustFuck.GruntAcknowledgement".Localized());

            OutputText("Scenes.Emberscene.NoStayingForCuddlesPostLustFuck.GatherThingsWashOff".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void StayWithEmberAfterLustFuck()
        {
            ClearOutput();

            var x = Player.CockThatFits(EmberVaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.AgreesWithSmile".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
            {
                OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.EmbraceFeelStirring".Localized());

                OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.TremblesElectricalWaves".Localized());

                OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.NoReasonNotTo".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), () => FrottingWithEmber());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Female)
                {
                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.HugsPressesAgainst".Localized((LittleEmber() ? "Scenes.Rubi.RubiAppearance.FlatChest".Localized() : "Common.Text.Bosom".Localized())));

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                        OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.MilkFilledWord".Localized());

                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.SoftSmoothCooler".Localized((LittleEmber() ? "Common.Text.Chest".Localized() : "Common.Text.Breasts".Localized())));

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                        OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.GirlSuffix".Localized());

                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.PressesHandToQuim".Localized());

                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.GripsBaseTeasing".Localized());

                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.BreaksKissAlignsTip".Localized(
                        Player.CockDescript(x)
                    ));

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), () => PenetrateWithEmber());
                }
                else
                {
                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.EmbraceFeelStirringAgain".Localized());

                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.TremblesWavesHerBody".Localized());

                    if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                        OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.GirlSuffixAgain".Localized());

                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.RubsCocksTogether".Localized());

                    OutputText("Scenes.Emberscene.StayWithEmberAfterLustFuck.TryingSomethingDifferent".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.Frotting".Localized(), FrottingWithFrottingEmberHerm);

                    AddButton(1, "Common.Button.Penetrate".Localized(), PenetrateEmberHerm);
                }
        }

        public void FrottingWithFrottingEmberHerm()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.FrottingWithFrottingEmberHerm.ThinksThenRubs".Localized());

            FrottingWithEmber(false);
        }

        public void PenetrateEmberHerm()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.PenetrateEmberHerm.FingersHerOnceMore".Localized());

            PenetrateWithEmber(false);
        }

        public void FrottingWithEmber(bool clear = true)
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            if (!(clear))
                OutputText("Scenes.Emberscene.FrottingWithEmber.ParagraphBreak".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.ThrustsRidgesStimulate".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Emberscene.FrottingWithEmber.AlwaysReadyPleasure".Localized());

            if (Player.Gender == Gender.Male && CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Male)
                OutputText("Scenes.Emberscene.FrottingWithEmber.WondersOtherDragonsSay".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.ShaftPressedTightly".Localized(
                EmberMF("His", "Her")
            ));

            OutputText("Scenes.Emberscene.FrottingWithEmber.UseShaftGetOff".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                OutputText("Scenes.Emberscene.FrottingWithEmber.Boy.Two".Localized(
                    EmberMF("-boy", "-girl")
                ));

            OutputText("Scenes.Emberscene.FrottingWithEmber.TongueSticksOutPleasure".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.ReadsMindEnjoyYourself".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.NoIdleChatterEmbrace".Localized());

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.FrottingWithEmber.MoistPussyNotHelping".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.MatchThrustHump".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.MoanGaspSpillsSeed".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.BreaksHugScootsBack".Localized(
                EmberMF("his", "she")
            ));

            if (EmberHasVagina())
                OutputText("Scenes.Emberscene.FrottingWithEmber.OtherHandFrigsPussy".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.TheDragonPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                OutputText("Scenes.Emberscene.FrottingWithEmber.Boy.Two".Localized(
                    EmberMF("-boy", "-girl")
                ));

            OutputText("Scenes.Emberscene.FrottingWithEmber.CrawlsBackBetweenArms".Localized(
                EmberMF("He", "She"),

                EmberMF("He", "She"),

                EmberMF("his", "her")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) == 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.FrottingWithEmber.HeadWord".Localized());
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberHair) == 2)
                    OutputText("Scenes.Emberscene.FrottingWithEmber.ManeWord".Localized());
                else
                    OutputText("Scenes.Emberscene.FrottingWithEmber.HairWord".Localized());

            OutputText("Scenes.Emberscene.FrottingWithEmber.FallAsleepTogether".Localized());

            Player.Orgasm("Dick");

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                Player.RefillHunger(25);

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), EmberJizzbangbangEnding);
        }

        public void PenetrateWithEmber(bool clear = true)
        {
            ClearOutput();

            if (!(clear))
                OutputText("Scenes.Emberscene.PenetrateWithEmber.ParagraphBreak".Localized());

            var x = Player.CockThatFits(EmberVaginalCapacity());

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Emberscene.PenetrateWithEmber.ArmsAroundHug".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.EmberEmbracesTightly".Localized((LittleEmber() ? "Common.Text.Nipples".Localized() : "Common.Text.Breasts".Localized())));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.MilkSeepingFingers".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.PromiseSlidingSlowly".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.TheDragonPrefix".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.GirlSuffix".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.MoansAtMinistrations".Localized());

            if (EmberHasCock())
            {
                OutputText("Scenes.Emberscene.PenetrateWithEmber.DraconicPrickPressing".Localized());

                if (Player.BiggestBreastSize() > 1)
                    OutputText("Scenes.Emberscene.PenetrateWithEmber.YourChestPrefix".Localized());

                OutputText("Scenes.Emberscene.PenetrateWithEmber.YourBelly".Localized());

                if (Player.BiggestBreastSize() > 1)
                    OutputText("Scenes.Emberscene.PenetrateWithEmber.Comma".Localized());

                OutputText("Scenes.Emberscene.PenetrateWithEmber.AndEmbersOwn".Localized());
            }

            OutputText("Scenes.Emberscene.PenetrateWithEmber.EmberAsksGazing".Localized((LittleEmber() ? " undeveloped" : "")));

            OutputText("Scenes.Emberscene.PenetrateWithEmber.WhispersIntoEar".Localized());

            if (Player.Body.Get<TailPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.TailBaseTugging".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.ButtGrabbingPrefix".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.CheeksWriggleAppreciatively".Localized());

            if (Player.Body.Get<WingsPart>().Id != BodyPartTypes.None)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.StrokesAlongWings".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.BackOfHead".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.GuidesTowardsNipple".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.DrinkMyMilk".Localized());
            else
                OutputText("Scenes.Emberscene.PenetrateWithEmber.NoMilkYet".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.EmberTeasesSmile".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.SavorMilkGush".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.BucksAgainstCock".Localized());

            if (EmberHasCock())
                OutputText("Scenes.Emberscene.PenetrateWithEmber.DollopOfPre".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.TakesHandGuides".Localized((LittleEmber() ? "Common.Text.TheOtherSide".Localized() : "Common.Text.MyOtherBreast".Localized())));

            OutputText("Scenes.Emberscene.PenetrateWithEmber.SqueezeTitCaress".Localized((LittleEmber() ? "Common.Text.TinyNipple".Localized() : "Common.Text.FirmYetSoftFlesh".Localized())));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.MilkSpillsMouth".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.ListeningToMoan".Localized(
                Player.CockDescript(x)
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.TastingOwnMilk".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.KissHerBack".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.CumForMe".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) > 0)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.GirlSuffixAgain".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.RoaringAtCeiling".Localized());

            if (Player.CumQ() < 250)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.MereTrickleLeft".Localized());
            else
                if (Player.CumQ() < 500)
                    OutputText("Scenes.Emberscene.PenetrateWithEmber.AlreadyWellMilked".Localized());
                else
                    OutputText("Scenes.Emberscene.PenetrateWithEmber.ProdigiousOutputSmaller".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.SentencePeriod".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite)
                OutputText("Scenes.Emberscene.PenetrateWithEmber.CockThrobsBlanks".Localized());

            OutputText("Scenes.Emberscene.PenetrateWithEmber.CollapseIntoArms".Localized());

            Player.Orgasm("Generic");

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), EmberJizzbangbangEnding);
        }

        public void EmberJizzbangbangEnding()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.ConsciousnessReturns".Localized());

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.LooksUpSmiles".Localized(
                EmberMF("He", "She"),

                EmberMF("His", "Her"),

                EmberMF("He", "She")
            ));

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.MawWord".Localized());
            else
                OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.MouthWord".Localized());

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.WhitenessOnTongue".Localized(
                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.EnjoySessionsWithYou".Localized(
                EmberMF("He", "She"),

                EmberMF("He", "She"),

                EmberMF("He", "She")
            ));

            if (EmberHasCock())
            {
                OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.WhatBobbingBetween".Localized());

                OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.LookAtDragon".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0)
                    OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.AnthroWord".Localized());
                else
                    OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.MonsterPrefix".Localized(
                        EmberMF("boy", "girl")
                    ));

                OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.ComesCleanCrossesArms".Localized(
                    EmberMF("He", "She")
                ));

                OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.CantHelpButLaugh".Localized(
                    EmberMF("he's", "she's"),

                    EmberMF("He", "She")
                ));
            }

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.LoverStillPlastered".Localized());

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.CleanUpLater".Localized());

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.GathersYourClothes".Localized());

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.DragonMatePeck".Localized(
                EmberMF("He", "She"),

                EmberMF("He", "She")
            ));

            OutputText("Scenes.Emberscene.EmberJizzbangbangEnding.HeadsOffForDay".Localized());

            Player.ChangeFatigue(-20);

            Player.ApplyStats(new DynStat(GameStats.Libido, -1f));

            DoNext(() => ReturnToCamp(2));
        }

        public void EmberTribbing()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberTribbing.GazeTravelsBody".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.BristlesUnderGaze".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.WhatAreYouWaiting".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.ClaimsHerLips".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.BreakTheKiss".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.PatGroundBeside".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.SittingCloseReserved".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.MustFaceYou".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.TremorInVoice".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.GladlyTakeLead".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.BreathHeavyLidded".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.FinalPlungeLips".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.PaceFirmMeasured".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.CompensateFalseConfidence".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.ClitPressHips".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.ArmsAroundHer".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing.CrestOverPeace".Localized());

            Player.Orgasm("Vaginal");

            DoNext(() => EmberTribbing2());
        }

        public void EmberTribbing2()
        {
            ClearOutput();

            OutputText("Scenes.Emberscene.EmberTribbing2.BodyEbbsPulse".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing2.TurnsToLook".Localized());

            OutputText("Scenes.Emberscene.EmberTribbing2.SmileAndStretch".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void SleepWithEmber()
        {
            ClearOutput();

            if (View.Time.Hours >= 21)
            View.Time.LastPassedHours = 24 - View.Time.Hours;
            else
            View.Time.LastPassedHours = 0;

            View.Time.Hours += CampScene.WakeupTime();

            var chooser = Random.Shared.Next(3);

            OutputText("Scenes.Emberscene.SleepWithEmber.BeforeTurningIn".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) < 25)
            {
                if (chooser == 0)
                {
                    OutputText("Scenes.Emberscene.SleepWithEmber.ApproachingDenYawn".Localized());

                    OutputText("Scenes.Emberscene.SleepWithEmber.ChidesThenSoftens".Localized());

                    OutputText("Scenes.Emberscene.SleepWithEmber.ApologizeExhausted".Localized());

                    OutputText("Scenes.Emberscene.SleepWithEmber.ReadyForBed".Localized());

                    OutputText("Scenes.Emberscene.SleepWithEmber.WantedToSeeEmber".Localized());

                    OutputText("Scenes.Emberscene.SleepWithEmber.TakesAnotherGlance".Localized((LittleEmber() ? "You refrain from pointing out that [Ember ey] is, in fact, a child. " : "")));

                    OutputText("Scenes.Emberscene.SleepWithEmber.CampWatchSleep".Localized());

                    OutputText("Scenes.Emberscene.SleepWithEmber.QuizzicalLookUndress".Localized());
                }
                else
                    if (chooser == 1)
                    {
                        OutputText("Scenes.Emberscene.SleepWithEmber.DenDragonHome".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.LovesMeRhyme".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.CantBelieveRhyme".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.FreezesDropsFlower".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.WhoInspiringEmber".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Emberscene.SleepWithEmber.FollowsEmberNight".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.WanderingThenStops".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Dragon".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.CurseYourName".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.WatchesCaressingBody".Localized((LittleEmber() ? "little " : "")));

                        if (EmberHasCock())
                            OutputText("Scenes.Emberscene.SleepWithEmber.PokesOwnGenitals".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberInternalDick) > 0 ? "Common.Text.Out".Localized() : "Common.Text.ToFullMast".Localized())));

                        if (EmberHasVagina())
                            OutputText("Scenes.Emberscene.SleepWithEmber.FingersFindPussy".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.WondersWhyYes".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.BreathingErratic".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.ReachesClimax".Localized((EmberHasCock() ? "" + EmberMF("His", "Her") + " cum splatters on the ground ahead of [Ember em]" : ""), (CoC.Instance.Flags.Get(GameFlagIds.EmberGender) == (int)Gender.Hermaphrodite ? "while" : ""), (EmberHasVagina() ? "her juices cascade down [Ember eir] legs, pooling below" : "")));

                        OutputText("Scenes.Emberscene.SleepWithEmber.WonderWhatTalking".Localized());
                    }
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) >= 25 && CoC.Instance.Flags.Get(GameFlagIds.EmberAffection) < 75)
                {
                    if (chooser == 0 && Player.Health < Player.MaxHealth * 0.8f)
                    {
                        OutputText("Scenes.Emberscene.SleepWithEmber.DenLickingClaws".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.CallOutGreeting".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.GlanceSpotsBruise".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.PointsAtBruise".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.RetreatsIntoDen".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.FollowsIntoDark".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.ComfortableGrassBed".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.OpensJugWater".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 ? "Common.Text.Maw".Localized() : "Common.Text.Mouth".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.ResumesLickingArm".Localized(
                            LittleEmber() ? "Common.Text.TheLittle".Localized() + EmberMF("boy", "girl") : "Ember"
                        ));

                        OutputText("Scenes.Emberscene.SleepWithEmber.WhyLickNotPotion".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.SalivaJustAsGood".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.WinceNoPain".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.SighStillWeirdedOut".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.ArmBarelyHurts".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.PromiseTryBest".Localized());
                    }
                    else
                        if (chooser == 1)
                        {
                            OutputText("Scenes.Emberscene.SleepWithEmber.DenSomethingAmiss".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.CanHandleHerself".Localized(
                                EmberMF("boy", "girl")
                            ));

                            OutputText("Scenes.Emberscene.SleepWithEmber.CabinOccupiedSleeper".Localized(
                                CampSleep.BedDesc(),

                                CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 ? "dragon" : EmberMF("dragon-boy", "dragon-girl")
                            ));

                            OutputText("Scenes.Emberscene.SleepWithEmber.MuttersBackToBed".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.ComplainsRubbingEyes".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.ReasonForBed".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.WantedToTrySleeping".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.NowKnowsLeave".Localized((CabinInterior.HasBed ? "Common.Text.Bed".Localized() : "Scenes.Camp.CampFollowers.BedrollWord".Localized())));

                            OutputText("Scenes.Emberscene.SleepWithEmber.MightKeepBed".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.WantsToKeepBed".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.CheeksGlowYuck".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.TookTheBait".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Emberscene.SleepWithEmber.DenAmissAgain".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.CanHandleHerselfAgain".Localized(
                                EmberMF("boy", "girl")
                            ));

                            OutputText("Scenes.Emberscene.SleepWithEmber.UnusualBloomBed".Localized(
                                CampSleep.HomeDesc()
                            ));
                        }
                }
                else
                {
                    chooser = Random.Shared.Next(3);

                    if (CoC.Instance.Flags.Get(GameFlagIds.SleepWith) != "Ember" && CoC.Instance.Flags.Get(GameFlagIds.BenoitClockBought) > 0)
                        OutputText("Scenes.Emberscene.SleepWithEmber.TakesAlarmClock".Localized());

                    if (chooser == 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesSleptWithEmber) > 0)
                    {
                        OutputText("Scenes.Emberscene.SleepWithEmber.TakesUpOfferDen".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.EmberAlreadyNest".Localized((LittleEmber() ? "Common.Text.Chest".Localized() : "Common.Text.Breasts".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.PointsAtSelf".Localized((LittleEmber() ? "Common.Text.ThisIs".Localized() : "Common.Text.TheseAre".Localized()), (LittleEmber() ? "Common.Text.Breast".Localized() : "Common.Text.Breasts".Localized()), (LittleEmber() ? "Common.Text.SmoothFlatChestItLooks".Localized() : "Common.Text.BountifulFCupsTheyLook".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.BreastsBloatedFull".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.OffersToMilk".Localized((LittleEmber() ? "Common.Text.EmberEm".Localized() : "Common.Text.EmberEirBreasts".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.RepliesEmbarrassed".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.RubsSqueezesBreasts".Localized((LittleEmber() ? "Common.Text.UndevelopedChest".Localized() : "Common.Text.BountifulBreasts".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.ProtestsMorningDrained".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.HandStimulationFails".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.RubsChinConcedes".Localized(
                            EmberMF("He", "She")
                        ));

                        OutputText("Scenes.Emberscene.SleepWithEmber.NodsStrokesGently".Localized((LittleEmber() ? "Common.Text.Swollen".Localized() : "Common.Text.WeightOfEmberEir".Localized()), (LittleEmber() ? "Common.Text.ComparedToTheUsualPerfectFlatness".Localized() : "Common.Text.TheyActuallyFeelDramaticallyHeavierThan".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.ScoldsTheyreSensitive".Localized());

                        Player.RefillHunger(25);

                        OutputText("Scenes.Emberscene.SleepWithEmber.SighsInRelief".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.SuckleAndKnead".Localized((LittleEmber() ? "Common.Text.Chest".Localized() : "Common.Text.Breasts".Localized())));

                        OutputText("Scenes.Emberscene.SleepWithEmber.PurringRumblesLouder".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.DampnessOnCheek".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.AsksHighMilk".Localized());

                        Player.RefillHunger(25);

                        OutputText("Scenes.Emberscene.SleepWithEmber.SuckleInterruptsFlow".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.SometimeLaterBreak".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.ManyTimesDrained".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.EvenLaterBreak".Localized());

                        OutputText("Scenes.Emberscene.SleepWithEmber.FinalSuckleEighth".Localized(
                            LittleEmber() ? "Common.Text.OnTheChildsBareChestAnd".Localized() : "Common.Text.BetweenEmbersSoftMilkyMoundsAnd".Localized() + (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 ? "Common.Text.Scales".Localized() : "Common.Text.Flesh".Localized()) + ","
                        ));

                        Player.RemoveStatusEffect(StatusEffectIds.DragonBreathCooldown);

                        Player.SlimeFeed();

                        Player.ChangeFatigue(-40);
                    }
                    else
                        if (chooser == 1 && ((Player.Libido + (Player.Corruption / 2)) >= 50 || Player.LustPercent >= 70) && CoC.Instance.Flags.Get(GameFlagIds.TimesSleptWithEmber) > 0)
                        {
                            OutputText("Scenes.Emberscene.SleepWithEmber.WantsCabinSleep".Localized());

                            if (Player.Libido >= 66 || Player.LustPercent >= 90)
                                OutputText("Scenes.Emberscene.SleepWithEmber.MaybeGetLucky".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.CatchesEmberEating".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.FinishesBitesLicks".Localized(
                                EmberMF("He", "She")
                            ));

                            OutputText("Scenes.Emberscene.SleepWithEmber.SuddenStirringArousal".Localized((Player.Gender > 0 ? "Common.Text.InYourGroin".Localized() : "Common.Text.WithinYou".Localized())));

                            OutputText("Scenes.Emberscene.SleepWithEmber.EmberStandsUp".Localized(EmberMF("He", "She"), (LittleEmber() ? "flat chest" : (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || EmberIsFeminine() ? "Common.Text.GenerousBosom".Localized() : "Common.Text.TonedChest".Localized())), (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 ? "Common.Text.Scales".Localized() : "Common.Text.Skin".Localized())));

                            OutputText("Scenes.Emberscene.SleepWithEmber.ForgottenTiredness".Localized((LittleEmber() ? "Common.Text.Tiny".Localized() : "Common.Text.HandfulOf".Localized()), (EmberHasVagina() ? " and the soft lips of [Ember eir] " + (LittleEmber() ? "Common.Text.LittleCunny".Localized() : "Common.Text.PleasureHole".Localized()) : "")));

                            OutputText("Scenes.Emberscene.SleepWithEmber.GoingToBedSoon".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.NodSwallowHard".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.FetchesWaterSkin".Localized((CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || EmberIsFeminine() ? "Common.Text.SoftMounds".Localized() : "Common.Text.TonedChest".Localized())));

                            OutputText("Scenes.Emberscene.SleepWithEmber.UrgeLickDry".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.LooksQuestioningly".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.DeliberateOrNot".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.JoinMeInBed".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.CabinFeelsFar".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.WelcomeWhenever".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.CrawlsToNest".Localized(EmberMF("He", "She"), ((!LittleEmber() && (EmberIsFeminine() || CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0)) ? "Common.Text.PillowyOrbs".Localized() : "Common.Text.Chest".Localized())));

                            OutputText("Scenes.Emberscene.SleepWithEmber.HalfLiddedInvite".Localized(
                                LittleEmber() ? "Common.Text.Little".Localized() + EmberMF("boy", "girl") : "dragon"
                            ));

                            OutputText("Scenes.Emberscene.SleepWithEmber.WrapsEmbraceWhisper".Localized());

                            OutputText("Scenes.Emberscene.SleepWithEmber.HardTimeSleeping".Localized());

                            Player.ApplyStats(new DynStat(GameStats.Lust, 30f));
                        }
                        else
                            if (chooser == 2 && Random.Shared.Next(2) == 0 && CoC.Instance.Flags.Get(GameFlagIds.TimesSleptWithEmber) > 0)
                            {
                                OutputText("Scenes.Emberscene.SleepWithEmber.WantsCompanyTonight".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.GreetsRubbingEyes".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.CuriousAboutCompany".Localized(
                                    LittleEmber() ? "Common.Text.Little".Localized() + EmberMF("boy", "girl") : "dragon",

                                    EmberMF("He", "She")
                                ));

                                OutputText("Scenes.Emberscene.SleepWithEmber.WhiffSmellySelf".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.StreamStripsFolds".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.SplashWaterWave".Localized((LittleEmber() ? "" : " how can something so big be so sneaky!?")));

                                OutputText("Scenes.Emberscene.SleepWithEmber.CoilsWaistScrubs".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.SillyQuestionDip".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.AvoidsAnswering".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.FeelsNiceSmellsGood".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.TurnAroundFront".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.TurnsAroundFacing".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Scenes.Emberscene.PenetrateEmbrah.DraconicWord".Localized())));

                                OutputText("Scenes.Emberscene.SleepWithEmber.StartsConversation".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.NodCloseEyes".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Dragon".Localized())));

                                OutputText("Scenes.Emberscene.SleepWithEmber.SmilesMakesWay".Localized((Player.HasCock() ? Player.MultiCockDescriptLight() : ""), (Player.HasCock() && Player.HasVagina() ? " and " : ""), (Player.HasVagina() ? Player.VaginaDescript() : ""), (Player.Gender == Gender.Genderless ? "groin" : "")));

                                OutputText("Scenes.Emberscene.SleepWithEmber.ShiversAttention".Localized(
                                    (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 && CoC.Instance.Flags.Get(GameFlagIds.EmberHair) == 0) ? "You note that Ember doesn't have hair [Ember emself], but this doesn't stop you reaching out and starting to gently stroke " + EmberMF("his", "her") + "Common.Text.ScalpInstead".Localized() : "Deciding that fair is fair, you reach out and start to gently stroke through Ember's hair, gathering some of the same goo that [Ember ey] was using in yours to wash it with."
                                ));

                                OutputText("Scenes.Emberscene.SleepWithEmber.NotMeNeedsBath".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.SmileDoTogether".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.ConcedesReachesBack".Localized((Player.HasCock() ? Player.MultiCockDescriptLight() : ""), (Player.HasCock() && Player.HasVagina() ? " and " : ""), (Player.HasVagina() ? Player.VaginaDescript() : ""), (Player.Gender == Gender.Genderless ? "genderless crotch" : ""), (CoC.Instance.Flags.Get(GameFlagIds.EmberRoundface) == 0 ? "Common.Text.Scalp".Localized() : "Common.Text.Hair".Localized()), (!LittleEmber() && (EmberIsFeminine() || CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0) ? "Common.Text.SizableBreasts".Localized() : "Scenes.Rubi.RubiAppearance.FlatChest".Localized())));

                                OutputText("Scenes.Emberscene.SleepWithEmber.ShuddersFaintSigh".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.InnocentCleaning".Localized(
                                    (CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 || EmberIsFeminine() ? "Common.Text.OneNipple".Localized() : "Common.Text.TheHeartShapedMarkOnEmber".Localized()),

                                    (CoC.Instance.Flags.Get(GameFlagIds.EmberPregnancyType) > 0 ? "Common.Text.Swollen".Localized() : "Common.Text.FlatAndMuscular".Localized())
                                ));

                                OutputText("Scenes.Emberscene.SleepWithEmber.StopsBracesBank".Localized(
                                    (CoC.Instance.Flags.Get(GameFlagIds.EmberPregnancyType) > 0 ? "pregnant " : ""),

                                    EmberMF("His", "Her"),

                                    EmberGroinDesc("cock begins to " + (EmberInternalDick() ? "peek from its slit and" : "") + "Common.Text.GrowErect".Localized(), "vagina starts to grow moist, dribbling lubricants down [Ember eir] legs to drip into the water")
                                ));

                                OutputText("Scenes.Emberscene.SleepWithEmber.WeakProtestFinish".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.LetsGoTurnsAway".Localized((Player.HasTailInsteadOfLegs() || Player.IsGoo() ? "Scenes.Ceraphfollowerscene.BarkToCeraphOnce.Sliding".Localized() : "Common.Text.Walking".Localized())));

                                OutputText("Scenes.Emberscene.SleepWithEmber.BathNotOver".Localized((Player.Gender > 0 ? ", and over your genitals as well" : "")));

                                OutputText("Scenes.Emberscene.SleepWithEmber.ReachesForBowl".Localized((LittleEmber() ? "Common.Text.TheChilds".Localized() : "Common.Text.EmberEir".Localized()), EmberGroinDesc("erect cock", "dripping pussy")));

                                OutputText("Scenes.Emberscene.SleepWithEmber.RubsSalveGasp".Localized(
                                    EmberGroinDesc("cock", "pussy"),

                                    EmberGroinDesc((LittleEmber() ? "" : "long ") + "Common.Text.Shaft".Localized(), "the soft, wet walls of [Ember eir] feminine sex")
                                ));

                                OutputText("Scenes.Emberscene.SleepWithEmber.BendsShowSeductively".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.EyesGluedForm".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.WaitInDen".Localized());

                                OutputText("Scenes.Emberscene.SleepWithEmber.InnocentSmileKnows".Localized(
                                    EmberMF("He", "She")
                                ));

                                Player.ApplyStats(new DynStat(GameStats.Lust, 20f));

                                Player.HasGottenWashed();
                            }
                            else
                                if (CoC.Instance.Flags.Get(GameFlagIds.TimesSleptWithEmber) <= 0)
                                {
                                    OutputText("Scenes.Emberscene.SleepWithEmber.DragonNotThere".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.EmberLandsBehind".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.WantedCheckAlright".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.HmphNotChild".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.BidsGoodnightReturns".Localized((LittleEmber() ? "Common.Text.DecidingNotToPointOutThat".Localized() : "Common.Text.SeeingThatsClearlyTheCase".Localized())));

                                    OutputText("Scenes.Emberscene.SleepWithEmber.SinceSoWorried".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.BegPardonHeard".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.NotAnInvitation".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.OldStoriesFoolishness".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.SmilesComeIn".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.SeeksGloomFlame".Localized(((CoC.Instance.Flags.Get(GameFlagIds.EmberMilk) > 0 && !LittleEmber()) ? "bountiful," : "")));

                                    OutputText("Scenes.Emberscene.SleepWithEmber.DrapesWingTail".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.ComfyThanExpected".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.MindNotSmelly".Localized(
                                        CampSleep.HomeDesc()
                                    ));

                                    OutputText("Scenes.Emberscene.SleepWithEmber.YawnsBidsGoodnight".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Emberscene.SleepWithEmber.WantsCompanyAgain".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.GreetsSleepyAgain".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.CuriousCompanyAgain".Localized((LittleEmber() ? "Common.Text.Child".Localized() : "Common.Text.Dragon".Localized())));

                                    OutputText("Scenes.Emberscene.SleepWithEmber.FollowsIntoBed".Localized());

                                    OutputText("Scenes.Emberscene.SleepWithEmber.FollowsEmbracingYou".Localized(
                                        EmberMF("He", "She")
                                    ));

                                    OutputText("Scenes.Emberscene.SleepWithEmber.ReturnsSentimentHug".Localized());
                                }

                    OutputText("Scenes.Emberscene.SleepWithEmber.PlansSleepHours".Localized(
                        NumberUtils.Num2Text(24 - View.Time.Hours)
                    ));

                    CoC.Instance.Flags.Set(GameFlagIds.SleepWith, "Ember");

                    CoC.Instance.Flags.Set(GameFlagIds.TimesSleptWithEmber, CoC.Instance.Flags.Get(GameFlagIds.TimesSleptWithEmber) + 1);

                    CoC.Instance.Flags.Set(GameFlagIds.EmberMorning, 1);
                }

            EmberAffection(3);

            CampSleep.SleepWrapper();
        }

        public void PostEmberSleep()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.EmberMorning, 0);

            OutputText("Scenes.Emberscene.PostEmberSleep.YawnStretchKinks".Localized());

            OutputText("Scenes.Emberscene.PostEmberSleep.NoSignDragon".Localized(
                Player.ClothedOrNaked(".. looking to the side, you spot your [armorName] neatly folded beside the nest."),

                Player.ClothedOrNaked("put on your [armorName] and ")
            ));

            DoNext(PlayerMenu);
        }

        public void GiftDrakeFlower()
        {
            ClearOutput();

            Player.ConsumeItem(ItemRegistry.Create(ConsumableIds.DrakeHeart), 1);

            OutputText("Scenes.Emberscene.GiftDrakeFlower.GiftBehindBack".Localized(
                EmberMF("dragon", "dragon girl")
            ));

            OutputText("Scenes.Emberscene.GiftDrakeFlower.HidesExcitement".Localized());

            OutputText("Scenes.Emberscene.GiftDrakeFlower.DragonsForagePride".Localized(
                LittleEmber() ? "Common.Text.Little".Localized() + EmberMF("boy", "girl") : "[Ember short]"
            ));

            OutputText("Scenes.Emberscene.GiftDrakeFlower.EyesLightUp".Localized());

            OutputText("Scenes.Emberscene.GiftDrakeFlower.SentimentalThankYou".Localized());

            OutputText("Scenes.Emberscene.GiftDrakeFlower.BlushingTakesHeart".Localized());

            DoNext(PlayerMenu);

            CoC.Instance.Flags.Set(GameFlagIds.GiftedFlower, 1);

            View.Time.Hours += 1;
        }

    }
}