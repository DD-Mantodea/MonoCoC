using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Contents.Pregnancy;
using System.Collections.Generic;
using MonoCoC.Utils;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register<HolliScene>("coc:holli_scene")]
    public class HolliScene : Scene
    {

        public static bool AmilyFurry() =>

            CoC.Instance.Flags.Get(GameFlagIds.AmilyNotFurry) == 0 && !noFur;

        public static SaveContent Save => SaveContent(SceneIds.Npcs.Holli);

        public string debugName => "Holli";

        public string debugHint => "";

        public override void Entry()
        {
        }

        public void Reset()
        {

            Save.SetBool("askedGlades", false);

            Save.SetBool("growingGlades", false);

            Save.SetInt("fruitsEaten", 0);
        }

        public bool HolliFollower()
        {

            return CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerKilled) == 0;
        }

        public void HolliSprite()
        {

        }

        public void FightHolli()
        {

            CoC.Instance.Flags.Set(GameFlagIds.FoughtHolli, 1);

            HolliSprite();

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "holli")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()), canFlee: false);
        }

        public void TreeMenu(bool output = true)
        {
            ClearOutput();

            HolliSprite();

            if (CoC.Instance.Flags.Get(GameFlagIds.FoughtHolli) > 0)
            {
                OutputText("Scenes.Holliscene.TreeMenu.DemonTreeStillStares".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Fight".Localized(), FightHolli);

                if (Player.HasKeyItem("Jojo's Talisman") && !SceneInstance<JojoScene>.Get().CampCorruptJojo())
                    AddButton(1, "Common.Button.CallJojo".Localized(), CallDatJojo);

                AddButton(14, "Common.Button.Back".Localized(), () => Go<CampScene>());
            }
            else
                if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) == 1)
                {
                    if (output)
                        OutputText("Scenes.Holliscene.TreeMenu.SproutLooksLeafyShoot".Localized());

                    ClearButtons();

                    AddButton(0, "Common.Button.BurnIt".Localized(), () => HolliDestroyPrompt(DestroyDatFukkinTree, () => TreeMenu()));

                    AddButton(14, "Common.Button.LeaveIt".Localized(), () => Go<CampScene>());
                }
                else
                    if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) == 2)
                    {
                        if (output)
                            OutputText("Scenes.Holliscene.TreeMenu.LargePlantVioletBloom".Localized());

                        ClearButtons();

                        if (Player.HasCockThatFits(100) && Player.Lust >= 33)
                            AddButton(0, "Common.Button.FuckIt".Localized(), FuckFuckingFuckFlowerP2);
                        else
                            AddButton(0, "Common.Button.FuckIt".Localized(), null);

                        if (Player.HasVagina() && Player.Lust >= 33)
                            AddButton(1, "Common.Button.RideStamen".Localized(), RideDatFuckingFukkFlowerP2);
                        else
                            AddButton(1, "Common.Button.RideStamen".Localized(), null);

                        AddButton(10, "Common.Button.DestroyIt".Localized(), () => HolliDestroyPrompt(DestroyDatFuckingPlantAtP2, () => TreeMenu()));

                        AddButton(14, "Common.Button.LeaveIt".Localized(), () => Go<CampScene>());
                    }
                    else
                        if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) == 3)
                        {
                            if (output)
                                OutputText("Scenes.Holliscene.TreeMenu.FamiliarPlantBlossomedTree".Localized());

                            ClearButtons();

                            if (Player.HasCock() && Player.Lust >= 33)
                                AddButton(0, "Common.Button.FuckFlower".Localized(), FuckTheFlower);
                            else
                                AddButton(0, "Common.Button.FuckFlower".Localized(), null);

                            if (Player.HasVagina() && Player.Lust >= 33)
                                AddButton(1, "Common.Button.RideTentacle".Localized(), RideTheWalrusP3);
                            else
                                AddButton(1, "Common.Button.RideTentacle".Localized(), null);

                            AddButton(2, "Common.Button.DrinkSap".Localized(), DrinkThePlantGirlsSap);

                            if (Player.HasPerk(PerkIds.Dragonfire) || Player.HasPerk(PerkIds.FireLord) || Player.HasPerk(PerkIds.Hellfire) || Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire)                                                             || Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.HasPerk(PerkIds.CorruptedNinetails))
                            {
                                if (output)
                                    OutputText("Scenes.Holliscene.TreeMenu.BurnedDownByFire".Localized());

                                AddButton(10, "Common.Button.TorchIt".Localized(), () => HolliDestroyPrompt(TorchP3Tree, () => TreeMenu()));
                            }
                            else
                                AddButton(10, "Common.Button.TorchIt".Localized(), null);

                            AddButton(14, "Common.Button.LeaveIt".Localized(), () => Go<CampScene>());
                        }
                        else
                        {
                            if (output)
                            {
                                OutputText("Scenes.Holliscene.TreeMenu.WalkingEdgeCampApproach".Localized());

                                if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 1)
                                {
                                    OutputText("Scenes.Holliscene.TreeMenu.WelcomeBackSayStart".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) > 0 && Random.Shared.Next(4) == 0)
                                        OutputText("Scenes.Holliscene.TreeMenu.WantFertilizeAgainReady".Localized());
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) > 0 && Random.Shared.Next(3) == 0)
                                            OutputText("Scenes.Holliscene.TreeMenu.WantUseTentaclesRestless".Localized());
                                        else
                                            if (Random.Shared.Next(2) == 0)
                                                OutputText("Scenes.Holliscene.TreeMenu.ComeDrinkNectarSip".Localized());
                                            else
                                                OutputText("Scenes.Holliscene.TreeMenu.HowMayServeCommand".Localized());

                                    OutputText("Scenes.Holliscene.TreeMenu.SayEnd".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Holliscene.TreeMenu.WelcomeBackAgain".Localized());

                                    if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) > 0 && Random.Shared.Next(3) == 0)
                                        OutputText("Scenes.Holliscene.TreeMenu.WantGardeningExhausting".Localized());
                                    else
                                        if (CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) > 0 && Random.Shared.Next(2) == 0)
                                            OutputText("Scenes.Holliscene.TreeMenu.ComePlayTentaclesRestless".Localized());
                                        else
                                            if (Random.Shared.Next(2) == 0)
                                                OutputText("Scenes.Holliscene.TreeMenu.ComeDrinkHungryAdventurer".Localized());
                                            else
                                                OutputText("Scenes.Holliscene.TreeMenu.WantMaraeFinestCreation".Localized());

                                    OutputText("Scenes.Holliscene.TreeMenu.SayEndAgain".Localized());
                                }

                            }

                            ClearButtons();

                            AddNextButton("Common.Button.Appearance".Localized(), HolliAppearance);

                            AddNextButton("Common.Button.Talk".Localized(), HolliTalkMenu);

                            AddNextButton("Common.Button.Sex".Localized(), HolliSexMenu);

                            var alrauneQuestAsked = SaveContent(SceneIds.Npcs.Holli).GetInt("questAsked", 0);

                            var alrauneScene = SceneInstance<Areas.Swamp.AlrauneScene>.Get();

                            var vinesAsked = (alrauneQuestAsked & (1 << 8)) != 0;

                            if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 0)
                            {
                                AddButton(4, CampGuards.IsGuard("Holli") ? "Common.Button.StopGuarding".Localized() : "Common.Button.GuardCamp".Localized(), AskHolliToWatch);

                                var milkTotal = Player.ItemCount(ConsumableIds.SuccubiMilk) + Player.ItemCount(ConsumableIds.PurifiedSuccubiMilk) + Player.ItemCount(ConsumableIds.IsabellaMilk) + Player.ItemCount(ConsumableIds.MinotaurMilk) + Player.ItemCount(ItemIds.SheepMilk);

                                if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !vinesAsked)
                                    AddButton(10, "Common.Button.Vines".Localized(), alrauneScene.AskHolli).Hint("Any idea how to fix this?");

                                else if (Player.Armor.Id == ArmorIds.VineArmorBehavior)
                                    AddButton(10, "Common.Button.CureVines".Localized(), alrauneScene.HolliSolution)
                                        .Hint("Common.Text.AtLongLastYouCan".Localized())
                                        .DisableIf(!(Player.ItemCount(ConsumableIds.SlimyClothoth) >= 5

                                            && Player.ItemCount(ConsumableIds.PureHoney) >= 3

                                            && Player.ItemCount(ConsumableIds.SnakeOil) >= 3

                                            && milkTotal > 2

                                            && CoC.Instance.Flags.Get(GameFlagIds.HolliFuckedToday) == 1),

                                            "You haven't satisfied her demands: five slimy cloths, three bottles each of pure honey and snake oil, and three doses of milk from any source, in addition to feeding her.");
                            }
                            else
                            {
                                AddButton(4, "Common.Button.Guarding".Localized(), AskBrokenHolliToGuard);

                                if (Player.Armor.Id == ArmorIds.VineArmorBehavior && !vinesAsked)
                                    AddButton(10, "Common.Button.Vines".Localized(), alrauneScene.AskHolli).Hint("Any idea how to fix this?");

                                else if (Player.Armor.Id == ArmorIds.VineArmorBehavior)
                                    AddButton(10, "Common.Button.CureVines".Localized(), alrauneScene.HolliSolution)
                                        .Hint("Common.Text.AtLongLastYouCan".Localized())
                                        .DisableIf(Player.ItemCount(ConsumableIds.SlimyClothoth) < 1,

                                            "You don't have what she needs.");
                            }

                            if (CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) != 0 || Save.GetInt("fruitsEaten", 0) != 0)
                                AddButton(5, "Common.Button.EatFruit".Localized(), EatHolliFruit).DisableIf(CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) == 0, "There is no fruit for you to eat.");

                            if (CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmHolli) == 0 && CoC.Instance.Flags.Get(GameFlagIds.FarmCorruptionStarted) == 1)
                                AddRowButton(1, "Farm Help", HelpWithFarm);

                            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptedGladesDestroyed) > 0)
                            {
                                var gladesButton = AddRowButton(1, (Save.GetBool("growingGlades") ? "Common.Text.Less".Localized() : "Common.Button.More".Localized()) + " Glades", HolliAskGlades);

                                gladesButton?.Hint = "Ask Holli to " + (Save.GetBool("growingGlades") ? "Common.Text.Stop".Localized() : "Common.Text.Start".Localized()) + " growing more corrupted glades.";
                            }

                            AddButton(14, "Common.Button.Leave".Localized(), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.CampScene>.Get()));
                        }
        }

        public void HolliAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliAppearance.TreeHousesHollicyntheaTowering".Localized());

            OutputText("Scenes.Holliscene.HolliAppearance.AttentionDrawnHolliOoz".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) > 0)
                OutputText("Scenes.Holliscene.HolliAppearance.ZeroBranchesTwistingBreeze".Localized(
                    CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) == 1 ? "A large, purple fruit hangs" : NumberUtils.Num2Text(CoC.Instance.Flags.Get(GameFlagIds.HolliFruit)) + "Common.Text.LargePurpleFruitsHang".Localized(),

                    CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) == 1 ? "Common.Text.ItLooks".Localized() : "Common.Text.TheyLook".Localized(),

                    CoC.Instance.Flags.Get(GameFlagIds.HolliFruitExplained) > 0 ? " A very small, strange part of you actually feels proud to have made something like that with the tree-woman." : ""
                ));

            DoNext(() => TreeMenu());
        }

        public void HolliTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkMenu.WhatSubjectDemonDryad".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Marae".Localized(), HolliTalkMarae).Hint("Ask her about her progenitor.");

            AddNextButton("Codex.Category.Magic".Localized(), HolliTalkMagic).Hint("How exactly does she do what she does?");

            AddNextButton("Common.Button.Rooted".Localized(), HolliTalkRooted).Hint("Does she ever wish she wasn't bound to that tree?");

            AddNextButton("Common.Button.Reproduction".Localized(), HolliTalkReproduction).Hint("Ask about the boughs and the trees.");

            if (Save.GetInt("fruitsEaten", 0) >= 5)
                AddNextButton("Common.Button.Seeds".Localized(), HolliTalkSeeds).Hint("What happens to the cores you throw away?");

            SetExitButton("Common.Button.Back".Localized(), () => TreeMenu());
        }

        public void HolliTalkMarae()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkMarae.FirstCameGiftMarae".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMarae.CantGoLike".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMarae.HowKnowGoddessFirst".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMarae.SmilesDaughterLinked".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMarae.JoltSuddenContactTendril".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMarae.JustCloserLovelyStart".Localized());

            DoNext(() => HolliTalkMenu());
        }

        public void HolliTalkMagic()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkMagic.IncredibleAbilityGrowLife".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMagic.ConsidersWordsConsternation".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMagic.AnyWayTeachSimilar".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMagic.SmilesSlightMockery".Localized());

            OutputText("Scenes.Holliscene.HolliTalkMagic.AndBesidesRatherFucki".Localized());

            DoNext(() => HolliTalkMenu());
        }

        public void HolliTalkRooted()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkRooted.LivedWholeLifeSpot".Localized());

            OutputText("Scenes.Holliscene.HolliTalkRooted.InResponseSimplyGiggles".Localized());

            OutputText("Scenes.Holliscene.HolliTalkRooted.EyebrowsArchDoesntKnow".Localized());

            OutputText("Scenes.Holliscene.HolliTalkRooted.SniffsEarthNotOff".Localized());

            DoNext(() => HolliTalkMenu());
        }

        public void HolliTalkReproduction()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkReproduction.PersistentJuicesNotGrasp".Localized((CoC.Instance.Flags.Get(GameFlagIds.HolliFruitExplained) != 0 ? "Common.Text.ShesNeverProducedAnythingMoreThan".Localized() : "Common.Text.ItsNeverProvenFruitful".Localized())));

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliFruitExplained) != 0)
                OutputText("Scenes.Holliscene.HolliTalkReproduction.SighsCannotBreedWant".Localized());
            else
            {
                OutputText("Scenes.Holliscene.HolliTalkReproduction.SmirksReachesStrokesBranch".Localized((CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) > 0 ? "a plump fruit hanging from " : ""), (CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) > 0 ? "" : ", with time, could")));

                CoC.Instance.Flags.Set(GameFlagIds.HolliFruitExplained, 1);
            }

            OutputText("Scenes.Holliscene.HolliTalkReproduction.LeavesLotQuestionsUnanswered".Localized());

            OutputText("Scenes.Holliscene.HolliTalkReproduction.RespondsLivelyGlint".Localized());

            OutputText("Scenes.Holliscene.HolliTalkReproduction.LaughsSurprisingGaiety".Localized());

            DoNext(() => HolliTalkMenu());
        }

        public void HolliTalkSeeds()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkSeeds.AlwaysTossedCoresAway".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeeds.NoticesConcernNotWorry".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeeds.DodgingQuestionFirmAnswer".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeeds.SmilesReturnEarthStrength".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeeds.WhatMeanHeartTightens".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeeds.PoutsEyesDontWorry".Localized());

            ClearButtons();

            AddNextButton("Common.Button.Embrace".Localized(), HolliTalkSeedsEmbrace).Hint("All is well.");

            AddNextButton("Common.Button.Reject".Localized(), HolliTalkSeedsReject).Hint("You can't be here right now.");
        }

        public void HolliTalkSeedsEmbrace()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkSeedsEmbrace.LetBreathPeaceComfort".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeedsEmbrace.RatherTireWarmthSt".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeedsEmbrace.SwirlingBlackPitsEnrapture".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 50f));

            HolliSexMenu();

            if (ButtonAt(14) is { } hideExitButton)
                hideExitButton.Visible = false;
        }

        public void HolliTalkSeedsReject()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkSeedsReject.TearAwayStumbleBack".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeedsReject.RetreatHolliCallOut".Localized());

            OutputText("Scenes.Holliscene.HolliTalkSeedsReject.ShutBestNotThink".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void HelpWithFarm()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HelpWithFarm.AskPlantGoddessFarm".Localized());

            OutputText("Scenes.Holliscene.HelpWithFarm.BreathesVerdantOrderliness".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliFuckedToday) == 0)
            {
                OutputText("Scenes.Holliscene.HelpWithFarm.SmirksLittlePatchBoringness".Localized());

                TreeMenu(false);
            }
            else
            {
                OutputText("Scenes.Holliscene.HelpWithFarm.SaysEventuallyHelpNourishing".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Yes".Localized(), DoFarmFuckery);

                AddButton(1, "Common.Button.No".Localized(), NoPlzDontFuckWithFarm);
            }
        }

        public void DoFarmFuckery()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DoFarmFuckery.ClosesEyesHumsReverberate".Localized());

            OutputText("Scenes.Holliscene.DoFarmFuckery.ItsHappeningDoggieEnjoy".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FollowerAtFarmHolli, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void NoPlzDontFuckWithFarm()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.NoPlzDontFuckWithFarm.SighsFickleMortalGoddess".Localized());

            TreeMenu(false);
        }

        public void FertilizeHolli(bool cock = true)
        {

            float odds = 20;

            if (cock && Player.HasCock())
            {
                odds += Player.CumQ() / 300;

                if (odds > 40)
                    odds = 40;

                if (Player.HasPerk(PerkIds.MaraesGiftStud))
                    odds += 10;

                if (Player.HasPerk(PerkIds.FerasBoonAlpha))
                    odds += 10;

                if (Player.HasPerk(PerkIds.ElvenBounty))
                    odds += 2;
            }

            if (!cock && Player.HasVagina())
            {
                odds += Player.TotalFertility() / 5;

                if (odds > 40)
                    odds = 40;

                if (Player.HasPerk(PerkIds.MaraesGiftFertility))
                    odds += 10;

                if (Player.HasPerk(PerkIds.FerasBoonBreedingBitch))
                    odds += 10;

                if (Player.HasPerk(PerkIds.ElvenBounty))
                    odds += 2;
            }

            if (odds >= Random.Shared.Next(101))
                CoC.Instance.Flags.Set(GameFlagIds.HolliFruit, CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) + 1 + Random.Shared.Next((int)((float)(int)Math.Floor(odds / 10))));
        }

        public void HolliDestroyPrompt(Action destroyFunc, Action backFunc)
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliDestroyPrompt.EvenAfterWonderPlant".Localized());

            DoYesNo(destroyFunc, backFunc);
        }

        public void GetASprout()
        {
            ClearOutput();

            HolliSprite();

            OutputText("Scenes.Holliscene.GetASprout.AroundCampPlantLife".Localized());

            OutputText("Scenes.Holliscene.GetASprout.BrieflyWonderRootSoil".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.CorruptMaraeFollowupEncounterState) == 2)
                OutputText("Scenes.Holliscene.GetASprout.PromisedPlantTreeCamp".Localized());

            OutputText("Scenes.Holliscene.GetASprout.LookCloserPurpleLeaves".Localized());

            OutputText("Scenes.Holliscene.GetASprout.DoDestroyBold".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerLevel, 1);

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, 0);

            ClearButtons();

            AddButton(0, "Common.Button.Yes".Localized(), () => HolliDestroyPrompt(DestroyDatFukkinTree, LetZeFuckingSproutLive));

            AddButton(1, "Common.Button.No".Localized(), LetZeFuckingSproutLive);
        }

        public void DestroyDatFukkinTree()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DestroyDatFukkinTree.ReachExcavateTaintedSapling".Localized());

            OutputText("Scenes.Holliscene.DestroyDatFukkinTree.SparksSmokePurpleFire".Localized());

            OutputText("Scenes.Holliscene.DestroyDatFukkinTree.HopefulNoMorePlants".Localized());

            Player.UpgradeBeautifulSword();

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerKilled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void LetZeFuckingSproutLive()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.LetZeFuckingSproutLive.StayCorruptHasntDone".Localized());

            OutputText("Scenes.Holliscene.LetZeFuckingSproutLive.PlantAddedItemsBold".Localized());

            DoNext(PlayerMenu);
        }

        public void FuckPlantGrowsToLevel2()
        {
            ClearOutput();

            HolliSprite();

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerLevel, 2);

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, 0);

            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.SoundCreakingSnappingVines".Localized());

            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.WhereSaplingWaistFlower".Localized());

            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.SkinFlushesSexualEnticements".Localized());

            if (Player.Gender == Gender.Genderless)
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.SucksNoGenitals".Localized());

            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.MaraeIdeaOfGift".Localized());

            if ((int)Player.Gender > 0)
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.CouldWhirlSlowly".Localized());

            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.DestroyingSafestOption".Localized());

            Player.ApplyStats(new DynStat(GameStats.Lust, 33f));

            if (silly)
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.WhatDoBold".Localized());
            else
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.WhatToDoBold".Localized());

            ClearButtons();

            if (Player.HasCockThatFits(100) && Player.Lust >= 33)
                AddButton(0, "Common.Button.FuckIt".Localized(), FuckFuckingFuckFlowerP2);
            else
                AddButton(0, "Common.Button.FuckIt".Localized(), null);

            if (Player.HasVagina() && Player.Lust >= 33)
                AddButton(1, "Common.Button.RideStamen".Localized(), RideDatFuckingFukkFlowerP2);
            else
                AddButton(1, "Common.Button.RideStamen".Localized(), null);

            AddButton(10, "Common.Button.DestroyIt".Localized(), () => HolliDestroyPrompt(DestroyDatFuckingPlantAtP2, PlayerMenu));

            AddButton(14, "Common.Button.DoNothing".Localized(), () => Go<CampScene>());
        }

        public void DestroyDatFuckingPlantAtP2()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DestroyDatFuckingPlantAtP2.LettingGrowThreatenSafety".Localized());

            OutputText("Scenes.Holliscene.DestroyDatFuckingPlantAtP2.FireFluttersSparksBlotting".Localized());

            Player.UpgradeBeautifulSword();

            Player.ApplyStats(new DynStat(GameStats.Corruption, -5f));

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerKilled, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void FuckFuckingFuckFlowerP2()
        {
            ClearOutput();

            var x = Player.CockThatFits(100);

            if (x < 0)
                x = Player.SmallestCockIndex();

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.UndoingArmorApproachPulsating".Localized());

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.LaughAmusementCreamyTreat".Localized(
                Player.CockDescript(x),

                Player.CockHead(x)
            ));

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.EnoughQuakeSlideGround".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.BallsPlaceholder".Localized());
            else
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.LegsPlaceholder".Localized());

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.StickyPuddlePumpSheath".Localized());

            if (Player.Corruption < 33)
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.TimidlyLookAroundAudience".Localized());
            else
                if (Player.Corruption < 66)
                    OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.WorriedlyGlanceAround".Localized());
                else
                    OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.GlanceAroundSexyShow".Localized());

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.PantingUnbearableGiveUp".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.InsideViolatedSheath".Localized());

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.BloodTrappedBiggerHarder".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.TentacleCirclesPlaceholder".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.InternalSuctionSpikesBloats".Localized(
                Player.CockDescript(x)
            ));

            if (Player.Balls > 0)
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.BallsGurgleHappySound".Localized());

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.InsideLiquidHeatSpurting".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.YouForthProdigiousEssence".Localized());

            if (Player.CumQ() > 1500)
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.MeagerCannotSwallowFroths".Localized());

            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.SatisfiedExhaustedSlumpRelease".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedFlower, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) < 1000)
                CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) + 4);

            Player.ChangeFatigue(5);

            DoNext(() => ReturnToCamp(1));
        }

        public void RideDatFuckingFukkFlowerP2()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.ShuckArmorTowardsViolet".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.GrabbingPlantMonsSilky".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.DrivenOnslaughtFlopBack".Localized());

            if (Player.GetClitLength() > 4)
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.HeavyBuzzerFlexibleCock".Localized());

            if (Player.CockTotal() > 0)
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.EachPartnerTwistingSlicked".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.NothingOccupyRovingSlide".Localized());

            if (Player.BreastRows.Count > 1)
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.LowestTitsCuppingRow".Localized());
            else
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.ChestPlaceholder".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.CaressingThe".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Ir".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SwellSearchTightenedNipples".Localized());

            if (Player.LactationQ() > 50)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.OneAfterOtherBegin".Localized());

                if (Player.LactationQ() < 150)
                    OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Trickle".Localized());
                else
                    if (Player.LactationQ() < 500)
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Leak".Localized());
                    else
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SprayTorrentsOf".Localized());

                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.MilkPeriod".Localized());
            }

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.MoanUnabashedMindlessO".Localized());

            if (!false && Player.Corruption + Player.Libido < 150)
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Embarrassment".Localized());
            else
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Lust".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SentencePeriod".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.ThoughtBatteredConsciousness".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.EachExplodesReleasePetals".Localized());
            else
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.YLetter".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.OurQuiveringSoppingLast".Localized());

            if (Player.CumQ() < 50)
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Drizzle".Localized());
            else
                if (Player.CumQ() < 250)
                    OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Stream".Localized());
                else
                    if (Player.CumQ() < 750)
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Spray".Localized());
                    else
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Torrent".Localized());

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.TentaclesPullBackCreamed".Localized());

            Player.Orgasm("VaginalAnal");

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SwoonRelaxingSinkingGround".Localized());

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.ChangeFatigue(5);

            CoC.Instance.Flags.Set(GameFlagIds.TimesRiddenFlower, 1);

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) < 1000)
                CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) + 4);

            DoNext(() => ReturnToCamp(1));
        }

        public void FlowerGrowsToP3()
        {
            ClearOutput();

            HolliSprite();

            OutputText("Scenes.Holliscene.FlowerGrowsToP3.AgainNoisesCornerCamp".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) > 0)
                OutputText("Scenes.Holliscene.FlowerGrowsToP3.FuckingOnlyHelpedFaster".Localized());

            OutputText("Scenes.Holliscene.FlowerGrowsToP3.WoodBowingCreakingWeight".Localized());

            OutputText("Scenes.Holliscene.FlowerGrowsToP3.FirstVaginalTentacleFlower".Localized());

            OutputText("Scenes.Holliscene.FlowerGrowsToP3.DollopMoistLandingHair".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerLevel, 3);

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, 0);

            ClearButtons();

            if (Player.HasCock() && Player.Lust >= 33)
                AddButton(0, "Common.Button.FuckFlower".Localized(), FuckTheFlower);
            else
                AddButton(0, "Common.Button.FuckFlower".Localized(), null);

            if (Player.HasVagina() && Player.Lust >= 33)
                AddButton(1, "Common.Button.RideTentacle".Localized(), RideTheWalrusP3);
            else
                AddButton(1, "Common.Button.RideTentacle".Localized(), null);

            AddButton(2, "Common.Button.DrinkSap".Localized(), DrinkThePlantGirlsSap);

            if (Player.HasPerk(PerkIds.Dragonfire) || Player.HasPerk(PerkIds.FireLord) || Player.HasPerk(PerkIds.Hellfire) || Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire)                                                             || Player.HasPerk(PerkIds.EnlightenedNinetails) || Player.HasPerk(PerkIds.CorruptedNinetails))
            {
                OutputText("Scenes.Holliscene.FlowerGrowsToP3.SupposeBurnMagicalFire".Localized());

                AddButton(10, "Common.Button.TorchIt".Localized(), () => HolliDestroyPrompt(TorchP3Tree, PlayerMenu));
            }
            else
                AddButton(10, "Common.Button.TorchIt".Localized(), null);

            OutputText("Scenes.Holliscene.FlowerGrowsToP3.SentencePeriod".Localized());

            OutputText("Scenes.Holliscene.FlowerGrowsToP3.WhatToDoBold".Localized());

            AddButton(14, "Common.Button.LeaveIt".Localized(), () => Go<CampScene>());
        }

        public void FuckTheFlower()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Holliscene.FuckTheFlower.SmilingShedArmorSnake".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.BendSniffFlowerCunt".Localized());

            if (Player.HasVagina())
                OutputText("Scenes.Holliscene.FuckTheFlower.WetnessBuildsVaginaMaleness".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.FeverishSweatArousalPlant".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.BeforeHissCuttingAir".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.ClawLikeVinesFrenzy".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.RackingBrainSlumpAcceptance".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Holliscene.FuckTheFlower.SwungFroHarness".Localized(
                Player.CockHead(x)
            ));

            OutputText("Scenes.Holliscene.FuckTheFlower.ShamelessMoanDippingDeeper".Localized(
                Player.CockDescript(x)
            ));

            if (Player.CockArea(x) >= 150)
                OutputText("Scenes.Holliscene.FuckTheFlower.SlidingTrunkGiantPussy".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.SentencePeriod".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.YourOtherPenis".Localized());

                if (Player.CockTotal() > 2)
                    OutputText("Scenes.Holliscene.FuckTheFlower.EsRub".Localized());
                else
                    OutputText("Scenes.Holliscene.FuckTheFlower.Rubs".Localized());

                OutputText("Scenes.Holliscene.FuckTheFlower.SensuallyAcrossOuterBlossom".Localized());
            }

            OutputText("Scenes.Holliscene.FuckTheFlower.SchliiickSlideSlorpPop".Localized());

            if (Player.Cocks.Exists(c => c.SheathOn))
                OutputText("Scenes.Holliscene.FuckTheFlower.SheathWord".Localized());
            else
                OutputText("Scenes.Holliscene.FuckTheFlower.BaseWord".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Holliscene.FuckTheFlower.GurglingBallsSlappingEdge".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.SmallSquirtSweetLubricants".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.KeptLiquidSoftSnugness".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Holliscene.FuckTheFlower.JizzBlastsUncommonForce".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Holliscene.FuckTheFlower.MultipleEndowmentsAlabaster".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.SentencePeriodAgain".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Holliscene.FuckTheFlower.PearlyRopesFlaskSlit".Localized());

            if (Player.CumQ() > 1500)
                OutputText("Scenes.Holliscene.FuckTheFlower.WaveThickPumpingCock".Localized());

            if (Player.HasVagina())
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.HungryLongDeniedTrembles".Localized());

                if (Player.Wetness() < 4)
                    OutputText("Scenes.Holliscene.FuckTheFlower.Leaks".Localized());
                else
                    OutputText("Scenes.Holliscene.FuckTheFlower.Sprays".Localized());

                OutputText("Scenes.Holliscene.FuckTheFlower.OwnFemaleFluidsSympathy".Localized());
            }

            OutputText("Scenes.Holliscene.FuckTheFlower.AfterSpurtingNoLonger".Localized());

            OutputText("Scenes.Holliscene.FuckTheFlower.SighUtterlyExhaustedDrained".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            Player.ChangeFatigue(5);

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) < 1000)
                CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) + 5);

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedFlower, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void DrinkThePlantGirlsSap()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 15f), new DynStat(GameStats.Corruption, 1f));

            Player.ChangeFatigue(-50);

            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.FeelLikePickPrance".Localized());

            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.NipplesLargerHarderOne".Localized());

            if (silly)
                OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.TreeHugger".Localized());

            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.ThickFillsEnergyOut".Localized());

            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.WipeStickyMouthAdmiring".Localized());

            DoNext(() => ReturnToCamp(1));
        }

        public void RideTheWalrusP3()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.RideTheWalrusP3.EyeTentaclesBranchesWrithing".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.EggingPumpWaterfallSlime".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.ThisTendrilsReactCircle".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.SweetSpreadArmsFeelers".Localized());

            Player.CuntChange(15, true, true, false);

            OutputText("Scenes.Holliscene.RideTheWalrusP3.NipplesHardPenetrationReek".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.IndustriousRearsThrustsLips".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.DownSoddenSnatchSquishing".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.HangForeverBreatheDick".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.VisionSwimmingUpsideDown".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.EndureWordlessPleasureSpacy".Localized());

            OutputText("Scenes.Holliscene.RideTheWalrusP3.SetDownSprayedBatch".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, 1f), new DynStat(GameStats.Sensitivity, -5f));

            if (CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) < 1000)
                CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerGrowthCounter) + 5);

            CoC.Instance.Flags.Set(GameFlagIds.TimesRiddenFlower, CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void TorchP3Tree()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.TorchP3Tree.GoneEnoughCannotContinue".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                OutputText("Scenes.Holliscene.TorchP3Tree.ClosingFocusGatherEnergies".Localized());
            else
                if (Player.HasPerk(PerkIds.FireLord))
                    OutputText("Scenes.Holliscene.TorchP3Tree.SuckingBreathJaguarGift".Localized());
                else
                    OutputText("Scenes.Holliscene.TorchP3Tree.SuckingBreathFieryGift".Localized());

            OutputText("Scenes.Holliscene.TorchP3Tree.GoesTorchFadeSooner".Localized());

            if (Player.HasStatusEffect(StatusEffectIds.KnowsBlackfire) || Player.HasStatusEffect(StatusEffectIds.KnowsWhitefire))
                OutputText("Scenes.Holliscene.TorchP3Tree.MagicalWill".Localized());
            else
                OutputText("Scenes.Holliscene.TorchP3Tree.Breath".Localized());

            OutputText("Scenes.Holliscene.TorchP3Tree.SentencePeriod".Localized());

            OutputText("Scenes.Holliscene.TorchP3Tree.ThisStaysSuitablyTorch".Localized());

            Player.ChangeFatigue(100);

            Player.UpgradeBeautifulSword();

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerKilled, 1);

            DoNext(() => ReturnToCamp(2));
        }

        public void TreePhaseFourGo()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.TreePhaseFourGo.UnfamiliarFemaleCallsName".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.WhereYoooouuuCroonsBend".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.VerdantSuccubusCrossingArms".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) > 0)
                OutputText("Scenes.Holliscene.TreePhaseFourGo.EnjoyFeedingSeenExpression".Localized());
            else
                OutputText("Scenes.Holliscene.TreePhaseFourGo.EnjoyWatchingGrowFed".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.SayEnd".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.ShakeCorruptGoddessSister".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.ExplainsLiftingFuckHarder".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.SnapOddlyColoredGaze".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.CoosBelowPlayMe".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.OffersSateLustsTogether".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.DemonicWomanDryadLegends".Localized());

            OutputText("Scenes.Holliscene.TreePhaseFourGo.HollicyntheaAddedFollowersBold".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerLevel, 4);

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerGrowthCounter, 0);

            Player.ApplyStats(new DynStat(GameStats.Lust, 15f));

            TreeMenu(false);
        }

        public void JojoTransformAndRollOut()
        {
            ClearOutput();

            HolliSprite();

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.ComeHerePlease".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.JojoCallingBorderPolitely".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) == 0)
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.GesturesStrangePortalSite".Localized());
            else
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.GesturesHummingVerdantGrown".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.QuietlyInterruptNewsMarae".Localized());

            if (Player.Corruption < 25)
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.NotWillingAdmitChanged".Localized());
            else
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.ThoughLeaveNeedlessDetails".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.MonkWidenSurpriseSumrise".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.BlurtsUnbelievableSayStart".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) == 0)
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.ToThinkConsiderEnemy".Localized());
            else
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.EvenKnowingFornicateName".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.SayEndAgain".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.LooksArgumentResignedShake".Localized());

            if (Player.Level < 20)
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.NonethelessPositionConfront".Localized());
            else
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.MortalResourcesDestroyEscape".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.SoSayingPressesTalisman".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.MakesPoliteForestAssistance".Localized());

            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.JojoNoCompanionBold".Localized());

            Player.RemoveStatusEffect(StatusEffectIds.PureCampJojo);

            Player.CreateKeyItem("Jojo's Talisman", 0, 0, 0, 0);

            DoNext(PlayerMenu);
        }

        public void AmilyHatesTreeFucking()
        {
            ClearOutput();

            HolliSprite();

            OutputText("Scenes.Holliscene.AmilyHatesTreeFucking.MouseyCohabitantIrritation".Localized());

            OutputText("Scenes.Holliscene.AmilyHatesTreeFucking.SawWhatTree".Localized());

            OutputText("Scenes.Holliscene.AmilyHatesTreeFucking.NoInterruptChanceAnswer".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.StayQuiet".Localized(), StayQuietWhileAmilyBitchesAboutFuckingArborday);

            AddButton(1, "Common.Button.SlapHer".Localized(), SlapAmilysWhoreFace);

            if (Player.Intelligence >= 70 && Player.IsPureEnough(30))
                AddButton(2, "Common.Button.MakeExcuse".Localized(), MakeUpSomething).Hint("You're smart and you're visibly not corrupted. You're pretty sure you can convince her that Holli has an actual purpose.");
        }

        public void MakeUpSomething()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.MakeUpSomething.LetFinishOutburstPanting".Localized());

            OutputText("Scenes.Holliscene.MakeUpSomething.DrawBreathExplanationOff".Localized());

            OutputText("Scenes.Holliscene.MakeUpSomething.UsingSmartsMakeStory".Localized());

            OutputText("Scenes.Holliscene.MakeUpSomething.EyesNarrowLowersJudging".Localized());

            OutputText("Scenes.Holliscene.MakeUpSomething.BreatheReliefTruthGives".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyTreeMadeupbullshit, 1);

            DoNext(PlayerMenu);
        }

        public void StayQuietWhileAmilyBitchesAboutFuckingArborday()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.StayQuietWhileAmilyBitchesAboutFuckingArborday.BeratesSomeWhileEyeballs".Localized());

            OutputText("Scenes.Holliscene.StayQuietWhileAmilyBitchesAboutFuckingArborday.NotStayingAnymoreClean".Localized());

            OutputText("Scenes.Holliscene.StayQuietWhileAmilyBitchesAboutFuckingArborday.CurtlyTurnsDepartsLashing".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyTreeFlipout, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCampCorruptionFreaked, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

            if (Player.Pregnancy.Type == PregnancyIds.Amily)
                Player.KnockUp(PregnancyIds.Mouse, Player.Pregnancy.Incubation, 100, 1);

            DoNext(PlayerMenu);
        }

        public void SlapAmilysWhoreFace()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.SlapAmilyAcrossThe".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.Nose".Localized());
            else
                OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.Face".Localized());

            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.DeclareNoneBusiness".Localized());

            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.LooksWoundedUnhappySorry".Localized());

            if (AmilyFurry())
                OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.MouseHyphen".Localized());

            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.GirlSaysEyesGoodbye".Localized());

            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.WithoutAnotherTurnsLeaves".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyCorruptFlipout, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 0);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageAccessible, 1);

            DoNext(PlayerMenu);
        }

        public void HolliSexMenu()
        {

            ClearButtons();

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 0)
            {

                SexButton(AddButton(0, "Common.Button.FuckHolli".Localized(), FuckHolliInZeFlowerPuss), Gender.Male);

                SexButton(AddButton(1, "Common.Button.RideTentacles".Localized(), Level4RideHollisTentacruels), Gender.Female);
            }
            else
            {

                SexButton(AddButton(0, "Common.Button.FuckHolli".Localized(), HolliGetsDickDommed), Gender.Male);

                SexButton(AddButton(1, "Common.Button.Tentaclebone".Localized(), FullOnTentacleTasticGangBangForHolli), Gender.Male)
                    .DisableIf(Player.CountCocksOfType("TENTACLE") < 10, "You don't have enough tentacle dicks for this.");

                SexButton(AddButton(2, "Common.Button.RideHolli".Localized(), VaginalDomHollisTentacruels), Gender.Female);
            }

            AddNextButton("Common.Button.DrinkSap".Localized(), HaveAMapleSyrupSnack);

            SetExitButton("Common.Button.Back".Localized(), () => TreeMenu());
        }

        public void FuckHolliInZeFlowerPuss()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            var x = Player.BiggestCockIndex();

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.KnowingSmileArmor".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.WellComeOver".Localized());

            if (Player.Tallness < 84)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.Little".Localized());
            else
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.Big".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ArborealSuggestsHuskySugar".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ConfidentlyAdvancePhallusSyrup".Localized());

            if (Player.CockTotal() > 1)
            {
                if (Player.CockTotal() == 2)
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.OtherPenisSplays".Localized());
                else
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.RestOfPenisesSplay".Localized());

                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ToSmearingFurrowsSmooth".Localized());
            }

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.HandReachesBelowTo".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.CradleGentlyMassagingSack".Localized());
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GentlyCaressYour".Localized());

                if (Player.Cocks.Exists(c => c.SheathOn))
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SheathWord".Localized());
                else
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.LoinsWord".Localized());
            }

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.EncouraginglyUpwardPresenting".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SighingHappilyImmobileBeauty".Localized());

            if (Player.CockArea(x) < 100)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.LeansKissesGlossyPuckers".Localized(
                    Player.CockHead(x)
                ));
            else
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.KissesDownToweringShaft".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GrabEdgesTitsSmushing".Localized(
                Player.CockDescript(x)
            ));

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.HungryWhispersGiveTaste".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.BallsWord".Localized());
            else
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GroinWord".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SlappingWetlyMelonsThrobbing".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GeyserSpunkRocketsOut".Localized());

            if (Player.CockArea(x) >= 100)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.AirHangsMomentSplattering".Localized());
            else
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MouthNoisilySwallowInject".Localized());

            if (Player.CumQ() >= 500)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.KeepBlastingRopesPainted".Localized());

            if (Player.CumQ() >= 1500)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.LakeAroundRiverDownhill".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MischievousGoopyFacialTongue".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.BreakEmbraceHappyAdmire".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.PaleHandsFondleSoothing".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.RollStrokeWhoahDozed".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.TitsWord".Localized());
            else
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ArmsWord".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.InsteadSlammingSlotsWarm".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MassagingBundleCocksFervor".Localized());
            else
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MassagingToolUncommonFervor".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GruntIrritationVanishesSex".Localized());

            if (Player.BiggestBreastSize() >= 2)
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.YourOwnWord".Localized());
            else
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.YourChestPlaceholder".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GiveBitingLowerLip".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.DeprivedChoiceRules".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MoansClingingTightlyPouts".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SnarlIrritationPillowTweak".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.BlackRingedRollQuivering".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.OnceStumbleAwayDrained".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.OffersSarcasticallyRaisingBoob".Localized());

            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.PoutsDressedEnticeCannot".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedFlower, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + 1);

            FertilizeHolli();

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -1f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void HaveAMapleSyrupSnack()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.ApproachHungryLickingChops".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) == 0)
                OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.KnowinglyPickDrinkDeeply".Localized());
            else
                OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.EagerlyAsksPickEngorged".Localized());

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.GrowlingFaintlySnakeCurl".Localized());

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.GaspsNotSoHard".Localized());

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.LookingConcernedVisageBite".Localized());

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.NippleWeakSprayMilk".Localized());

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.QuicklyFondlingReturnDrinking".Localized());

            Player.RefillHunger(10);

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.TremblingLubricantsFlushed".Localized());

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.PerhapsLaterWalkWanted".Localized());

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 1f), new DynStat(GameStats.Lust, 15f), new DynStat(GameStats.Corruption, 1f));

            Player.ChangeFatigue(-60);

            DoNext(() => ReturnToCamp(1));
        }

        public void Level4RideHollisTentacruels()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.SmirkKnowinglySuggestWhat".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.PromisesLoveLieBack".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.FoldArmsImpatientlyWaiting".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.GlareFacefulFlowerCunt".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.HandCondescendinglyForeplay".Localized());

            if (silly)
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.ProceedsJackSparrowMinutes".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.HotFluidDripsLarger".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.BurstFireworkStrengthIntensity".Localized());

            Player.CuntChange(60, true, true, false);

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.VibrantPurplesPinksBrighter".Localized());

            if (Player.LactationQ() == 0)
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.DropletsDrippingLactating".Localized());
            else
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.SteadyEruptingLactatingHard".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.CheersFuckingBothEnds".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.PitcherRootsMuddyPuddle".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.SpittleSoakedTorrentJuices".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.MilkSprayingRedoublesPitcher".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.AnHourLaterBold".Localized());

            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.WakeUtterlySoakedJizz".Localized());

            if (!Player.IsBiped())
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.WalkingBowleggedWeekLegs".Localized());
            else
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.WalkingBowleggedExclamation".Localized());

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Libido, .5f), new DynStat(GameStats.Sensitivity, 2f), new DynStat(GameStats.Corruption, 1f));

            FertilizeHolli(false);

            Player.SlimeFeed();

            CoC.Instance.Flags.Set(GameFlagIds.TimesRiddenFlower, CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void EatHolliFruit()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.EatHolliFruit.ReachUpBranchesPluck".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) == 1)
                OutputText("Scenes.Holliscene.EatHolliFruit.TheFruit".Localized());
            else
                OutputText("Scenes.Holliscene.EatHolliFruit.OneOfTheFruits".Localized());

            OutputText("Scenes.Holliscene.EatHolliFruit.PeriodSpace".Localized());

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliFruitExplained) == 0)
            {
                OutputText("Scenes.Holliscene.EatHolliFruit.SmilesAsksFruitsUnion".Localized());

                OutputText("Scenes.Holliscene.EatHolliFruit.NearlyPurplishPearSurprise".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HolliFruitExplained, CoC.Instance.Flags.Get(GameFlagIds.HolliFruitExplained) + 1);
            }

            OutputText("Scenes.Holliscene.EatHolliFruit.BitingJuicesExplodeChin".Localized());

            Player.RefillHunger(25);

            if (Random.Shared.Next(2) == 0 && Player.CockTotal() > Player.CountCocksOfType("TENTACLE"))
            {
                var choices = new List<int>();

                var temp = 0;

                while (temp < Player.CockTotal())
                {
                    if (Player.Cocks[temp].Id != new Identifier("coc", "tentacle"))
                        choices.Add(temp);

                    temp += 1;
                }

                temp = choices[Random.Shared.Next(choices.Count)];

                OutputText("Scenes.Holliscene.EatHolliFruit.PenisItchesGrowsLonger".Localized(
                    Utils.Utils.Num2TextOrdinal(temp + 1),

                    Player.CockDescript(temp)
                ));

                if (Player.CountCocksOfType("TENTACLE") > 0)
                    OutputText("Scenes.Holliscene.EatHolliFruit.Nother".Localized());

                OutputText("Scenes.Holliscene.EatHolliFruit.TentacleCockBold".Localized());

                Player.Cocks[temp].Id = new Identifier("coc", "tentacle");

                Player.Cocks[temp].KnotMultiplier = 1.3f;

                Player.ApplyStats(new DynStat(GameStats.Sensitivity, 3f), new DynStat(GameStats.Lust, 10f));
            }

            CoC.Instance.Flags.Set(GameFlagIds.HolliFruit, CoC.Instance.Flags.Get(GameFlagIds.HolliFruit) - 1);

            Player.ApplyStats(new DynStat(GameStats.Corruption, 1f));

            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 50)
                Player.ApplyStats(new DynStat(GameStats.Toughness, 1f));

            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 50)
                Player.ApplyStats(new DynStat(GameStats.Strength, 1f));

            Save.SetInt("fruitsEaten", Save.GetInt("fruitsEaten", 0) + 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void AskHolliToWatch()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliDefenseOn) == 0)
            {
                OutputText("Scenes.Holliscene.AskHolliToWatch.AskTentaclesGuardCamp".Localized());

                OutputText("Scenes.Holliscene.AskHolliToWatch.LooksOnlyDebaseYourself".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.ThreatenedHolli) > 0)
                    OutputText("Scenes.Holliscene.AskHolliToWatch.CouldAlwaysCuntBold".Localized());

                ClearButtons();

                AddButton(0, "Common.Button.Beg".Localized(), BegHolli4Watches);

                if (CoC.Instance.Flags.Get(GameFlagIds.ThreatenedHolli) == 0)
                    AddButton(1, "Common.Button.Threaten".Localized(), ThreatenHolli);
                else
                    AddButton(1, "Common.Button.SlapHer".Localized(), SlapDatHo);

                AddButton(2, "Common.Button.AssertSelf".Localized(), DomUpSomeHolli);

                AddButton(14, "Common.Button.Back".Localized(), () => TreeMenu());
            }
            else
            {
                OutputText("Scenes.Holliscene.AskHolliToWatch.TellStopWatchingLaughs".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HolliDefenseOn, 0);

                DoNext(() => TreeMenu());
            }
        }

        public void BegHolli4Watches()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.BegHolli4Watches.SwallowProstrateGrudgingly".Localized());

            OutputText("Scenes.Holliscene.BegHolli4Watches.DevilishGripsSquirtsSap".Localized());

            OutputText("Scenes.Holliscene.BegHolli4Watches.GrimacingFaceDownRoots".Localized());

            OutputText("Scenes.Holliscene.BegHolli4Watches.NahNotEnoughDetail".Localized());

            OutputText("Scenes.Holliscene.BegHolli4Watches.UtterlyHumiliatingLongerLick".Localized());

            OutputText("Scenes.Holliscene.BegHolli4Watches.HahGuessWatchCamp".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HolliDefenseOn, 1);

            DoNext(() => TreeMenu());
        }

        public void DomUpSomeHolli()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            OutputText("Scenes.Holliscene.DomUpSomeHolli.SwaggerPutHandBark".Localized());

            OutputText("Scenes.Holliscene.DomUpSomeHolli.SlappingOtherGoldBlack".Localized());

            var domPowah = Player.Level;

            domPowah += Player.Tallness / 12;

            if (Player.Horns.Id.Path != "none")
                domPowah += 3;

            if (Player.Corruption > 66)
                domPowah += 2;

            if (domPowah < 20)
            {
                OutputText("Scenes.Holliscene.DomUpSomeHolli.RollingSinksBarkLips".Localized());

                DoNext(() => ReturnToCamp(1));

                return ;
            }

            OutputText("Scenes.Holliscene.DomUpSomeHolli.SlowlyPitchBlackSclera".Localized());

            OutputText("Scenes.Holliscene.DomUpSomeHolli.IntimidatedArborSighsMarae".Localized());

            OutputText("Scenes.Holliscene.DomUpSomeHolli.WentConfidenceBrokenObediently".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.HolliSubmissive, 1);

            TreeMenu(false);
        }

        public void AskBrokenHolliToGuard()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.BowsMentionGuardingKeep".Localized());

            if (CampGuards.IsGuard("Holli"))
                OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.KeepingWatch".Localized());
            else
                OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.SleepingInstead".Localized());

            OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.SayEndPeriod".Localized());

            ClearButtons();

            if (CampGuards.IsGuard("Holli"))
                AddButton(1, "Common.Button.DontGuard".Localized(), ToggleBrokenHolliGuard);
            else
                AddButton(0, "CharCreation.ChooseHistory.ElderButton2".Localized(), ToggleBrokenHolliGuard);

            AddButton(14, "Common.Button.Back".Localized(), () => TreeMenu());
        }

        public void ToggleBrokenHolliGuard()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(GameFlagIds.HolliDefenseOn) == 0)
            {
                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.QuipsRubbingPalmsFun".Localized());

                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.SupposeBetterLesserWhelps".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HolliDefenseOn, 1);
            }
            else
            {
                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.SaysWinkSmitePlay".Localized());

                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.WarningShutsThinkBefore".Localized());

                CoC.Instance.Flags.Set(GameFlagIds.HolliDefenseOn, 0);
            }

            ClearButtons();

            AddButton(0, "Common.Button.Next".Localized(), () => TreeMenu());
        }

        public void HolliGetsDickDommed()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.TossingExposeCocksFixate".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.LeansForwardEntrancedThrough".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.HangingReachesStopsShort".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.SoLongGoodLicks".Localized());

            if (Player.BiggestCockArea() > 80)
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.JawStretchingObscenely".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.SentencePeriod".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.CongratulateSuccessPushBack".Localized());

            if (Player.BiggestCockArea() <= 80)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SwallowedToThe".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Holliscene.HolliGetsDickDommed.BallsPlaceholder".Localized());
                else
                    OutputText("Scenes.Holliscene.HolliGetsDickDommed.SheathPlaceholder".Localized());

                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SentencePeriodAgain".Localized());
            }
            else
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SwallowedDeepDistendedNeck".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.GurglingEyelashesTwitching".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.BubblingPleasureBuilds".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SackTautReadyBurst".Localized());
            else
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.BodyStrungTunedGuitar".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.PullsBackSnarlSh".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.GrabHungrySuckerNeck".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.SteppingRigidSlickFlower".Localized());

            if (Player.BiggestCockArea() > 80)
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.BendsSlightlyForcedInside".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.FlopsChestTwitchingWeakly".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.KissPlantyForcefullyDominance".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.AbruptnessOrgasmStartles".Localized());

            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.ExcessUddersViridianMounds".Localized());

                if (Player.CumQ() >= 500)
                    OutputText("Scenes.Holliscene.HolliGetsDickDommed.ConstantSpraysTurnWhite".Localized());
            }

            if (Player.CumQ() > 1000)
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.TrickleSpermWashesEntrance".Localized());

            if (Player.CumQ() > 2000)
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.MomentLaterTrickleRiver".Localized());

            if (Player.CumQ() > 3000)
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SwearTrunkStuffedSpunk".Localized());

            if (Player.CumQ() > 5000)
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SpoogeSpoutPressureRoof".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.TentaclesCumCanopy".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.RemovingGapingFlowerpotAdmire".Localized());

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.CrookFingerPointNectar".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedFlower, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            FertilizeHolli();

            DoNext(() => ReturnToCamp(1));
        }

        public void FullOnTentacleTasticGangBangForHolli()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.RemoveExposeWrithingTendrils".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ExercisingVegetativeExtend".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ReachesUpPioneeringPhallus".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.LookingWaySmolderingMassage".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.StartedFlushThreeWay".Localized());

            if (silly)
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.InflatableFlailingTubeCocks".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.BelowArborealAffectionPetals".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.HandsGlistenSixPenises".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.InundatedCockDominatedDick".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.UnspoolSixthUnoccupiedBundle".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ExperienceIntenseMoreManhoods".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AwashOrgyRadiatesGroin".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.TremendousExcitementFrame".Localized());

            if (Player.Balls > 0)
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.DrawTightGroinDischarging".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.SemenDistendsSpheroidBulges".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.SpoogeDistortionsPushedOut".Localized());

            if (Player.CumQ() > 500)
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AndTheNext".Localized());

            if (Player.CumQ() > 1400)
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AndTheNextEllipsis".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AnotherLoadCrownsHai".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ClimaxesAgainPassesVagina".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ComeLittleLaterRetracted".Localized());

            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.StretchingShouldersDump".Localized());

            Player.Orgasm("Dick");

            Player.ApplyStats(new DynStat(GameStats.Libido, .4f), new DynStat(GameStats.Sensitivity, -3f), new DynStat(GameStats.Corruption, 1f));

            CoC.Instance.Flags.Set(GameFlagIds.TimesFuckedFlower, CoC.Instance.Flags.Get(GameFlagIds.TimesFuckedFlower) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            FertilizeHolli();

            FertilizeHolli();

            DoNext(() => ReturnToCamp(1));
        }

        public void VaginalDomHollisTentacruels()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SashayFavoriteTentacularCleft".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.CooReleasingTurningSway".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.TiltsHeadUncertainlyUnh".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MuffledSlipsStuffedShudder".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MoansAgainLoudAudible".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.WickedIdeaReleasingPressure".Localized());

            Player.CuntChange(25, true, true, false);

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.ConsiderableRemainUprightInvol".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.NodsAgainVagina".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.TentaclePumpsFasterWetness".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SaySlowerPunctuatingSlap".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.UndulationsResumeLanguidPace".Localized());

            if (!Player.HasFuckableNipples() && Player.LactationQ() < 50)
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.PayAttentionNipplesTugging".Localized());
            else
                if (Player.LactationQ() >= 50)
                {
                    OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SighAtThe".Localized());

                    if (Player.LactationQ() < 150)
                        OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.Squirts".Localized());
                    else
                        if (Player.LactationQ() < 800)
                            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.Sprays".Localized());
                        else
                            if (Player.LactationQ() < 2000)
                                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.Outpouring".Localized());
                            else
                                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.ThickFountains".Localized());

                    OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MilkEscapeEverySluggish".Localized());
                }
                else
                    OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SinkNipplesFuckingTempo".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.LoudGroanEscapesHips".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.PaleGreenBlushesMask".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MovingQuickerPoundsPistoning".Localized());

            if (Player.GetClitLength() >= 3.5f)
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MomentBeforeWrappingJerking".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SentencePeriod".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.CriesHnnng".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.TelltaleFlaringDeepInside".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.WhisperSlutCumStroke".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.TentaclesRigidAbsolutelyHard".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.WarmWashesWombCervix".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.StillSpurtingWithdrawDragging".Localized());

            if (Player.Wetness() < 4)
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.OntoTheGround".Localized());
            else
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.IntoLeavingsSquirtingOrgasm".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SecondTimeMakeMess".Localized());

            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.PeckCheekGoodGirl".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.TimesRiddenFlower, CoC.Instance.Flags.Get(GameFlagIds.TimesRiddenFlower) + 1);

            CoC.Instance.Flags.Set(GameFlagIds.HolliFuckedToday, 1);

            FertilizeHolli(false);

            Player.SlimeFeed();

            Player.Orgasm("Vaginal");

            Player.ApplyStats(new DynStat(GameStats.Sensitivity, -2f), new DynStat(GameStats.Corruption, 1f));

            DoNext(() => ReturnToCamp(1));
        }

        public void ThreatenHolli()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.ThreatenHolli.SnarlThreatenBarkedBitch".Localized());

            OutputText("Scenes.Holliscene.ThreatenHolli.ThreatenFireUprootedSapling".Localized());

            OutputText("Scenes.Holliscene.ThreatenHolli.CompellingPoint".Localized());

            if (Player.Level < 30)
                OutputText("Scenes.Holliscene.ThreatenHolli.MightyMaraeBeyondPower".Localized());
            else
                OutputText("Scenes.Holliscene.ThreatenHolli.MaraeBeyondNotPressure".Localized());

            OutputText("Scenes.Holliscene.ThreatenHolli.RestStayFirmlyT".Localized());

            if (CampScene.CampGuarded())
                OutputText("Scenes.Holliscene.ThreatenHolli.YeahWhateverWaitingCovered".Localized());

            OutputText("Scenes.Holliscene.ThreatenHolli.MaybeRefusesGuardBold".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.ThreatenedHolli, 1);

            DoNext(() => ReturnToCamp(1));
        }

        public void SlapDatHo()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.SlapDatHo.SlapTreeThighsRipple".Localized());

            OutputText("Scenes.Holliscene.SlapDatHo.WhatMeatComePlay".Localized());

            ClearButtons();

            AddButton(0, "Common.Button.Cutherdown".Localized(), CutHerDown);

            if (Player.HasKeyItem("Jojo's Talisman"))
                AddButton(1, "Common.Button.CallJojo".Localized(), CallDatJojo);

            AddButton(4, "Common.Button.Ignore".Localized(), () => TreeMenu(true));
        }

        public void CutHerDown()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.CutHerDown.DemonSeriousBrokenToy".Localized());

            if (Player.WeaponName == "large axe")
                OutputText("Scenes.Holliscene.CutHerDown.ThatsTrickTreeTree".Localized());
            else
            {
                OutputText("Scenes.Holliscene.CutHerDown.SoArmoredTaintedInsects".Localized());

                OutputText("Scenes.Holliscene.CutHerDown.VerdantIncensedWithdrawsSlid".Localized());
            }

            CoC.Instance.Flags.Set(GameFlagIds.FoughtHolli, 1);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "holli")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void CallDatJojo()
        {
            ClearOutput();

            Player.RemoveKeyItem("Jojo's Talisman");

            OutputText("Scenes.Holliscene.CallDatJojo.TakeOutTalismanSummon".Localized());

            OutputText("Scenes.Holliscene.CallDatJojo.ApologiesSmellOnlyWay".Localized());

            OutputText("Scenes.Holliscene.CallDatJojo.DemonRoarsBreakingConversation".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.FoughtHolli, 1);

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "holli")), () => Go(SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.ForestScene>.Get()));
        }

        public void DefeatHolli()
        {
            ClearOutput();

            int temp = 0;

            Player.UpgradeBeautifulSword();

            CoC.Instance.Flags.Set(GameFlagIds.FuckFlowerKilled, 1);

            {
                OutputText("Scenes.Holliscene.DefeatHolli.WearySuccubusGodlingTree".Localized());

                if (Player.WeaponName != "large axe")
                    OutputText("Scenes.Holliscene.DefeatHolli.FetchAxeAnd".Localized());

                OutputText("Scenes.Holliscene.DefeatHolli.HackPeelingWedgeCuts".Localized());

                temp = 7;

                while (temp > 0)
                {
                    temp--;

                    View.Time.Hours += 1;

                    if (View.Time.Hours > 23)
                    {
                        View.Time.Days += 1;

                        View.Time.Hours = 0;
                    }
                }

            }

            OutputText("Scenes.Holliscene.DefeatHolli.KeyGainedHollisAshes".Localized());

            Player.CreateKeyItem("Holli's Ashes", 0, 0, 0, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void RecruitJojoToCamp()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.RecruitJojoToCamp.NodsEagerlyInviteCamp".Localized());

            OutputText("Scenes.Holliscene.RecruitJojoToCamp.BowingNeatlyTakesLeave".Localized());

            Player.CreateStatusEffect(StatusEffectIds.PureCampJojo, 0, 0, 0, 0);

            OutputText("Scenes.Holliscene.RecruitJojoToCamp.KeyGainedHollisAshes".Localized());

            Player.CreateKeyItem("Holli's Ashes", 0, 0, 0, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void DontRecruitJojoToCamp()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DontRecruitJojoToCamp.HoldTongueAllowingForest".Localized());

            OutputText("Scenes.Holliscene.DontRecruitJojoToCamp.KeyGainedHollisAshes".Localized());

            Player.CreateKeyItem("Holli's Ashes", 0, 0, 0, 0);

            CombatScene.CleanupAfterCombat();
        }

        public void EnjoyYourBadEndBIYAAAATCH()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ScreechingTriumphAloftShake".Localized());

            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.CoosDangerouslyEnoughBefore".Localized());

            if (Player.Gender == Gender.Male || (Random.Shared.Next(2) == 0 && Player.HasCock()))
            {
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.StripsHaphazardlyShredding".Localized());

                if (Player.Cocks[Player.BiggestCockIndex()].Thickness > 6)
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.StretchingJawWideWince".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.EngulfsTonguePlaysSli".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.OnceAttentionDisarmingly".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PleasureFoggedGoggleImpending".Localized());

                if (Player.BiggestCockLength() > 24)
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.NeckExtendsGrotesquelyMigrate".Localized(
                        NumberUtils.Num2Text((float)(Player.BiggestCockLength()))
                    ));
                else
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SinksBaseQuickThrust".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SensationPenetratedSucked".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TooClimaxComesFlood".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BallsDrawCloseUnload".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.EyesNothingComesOrg".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SmilesSwellingIncreasesSeed".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MigratingDirectionEntangling".Localized());
                else
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ExploringDepthsCurlingSkin".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.StomachSproutsCrawlingCrotch".Localized());

                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SmilesFoolishMeatMareth".Localized());

                Player.Orgasm("Dick");

                Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 20f));

                ClearButtons();

                AddButton(0, "Common.Button.Next".Localized(), HolliPlaysWithPenisesBadEnd);
            }
            else
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PullsImpatientlyFastenings".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AndTurningYouAround".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BuriesVaginaFlexibleTongue".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PealSurpriseInsertionDally".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.HumsHappilyReachesDeep".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.Something".Localized());

                    if (!Player.IsGoo() && !Player.HasTailInsteadOfLegs())
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SlipsBetweenThighsAnd".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.NosesPhallicTendrilUnnoticed".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.GlobPrecumEachCock".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SentencePeriod".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SmilesThrustingTentacle".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.CornerEyeFinishedAlready".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TentacleClimaxesEjaculating".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SLetter".Localized());
                    else
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ChestWord".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.LooksBreathingHeavilyComes".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MusingsInterruptedTentacle".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.OnePrecumLengthSpasm".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TracesPetalsPrettyBlossom".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.LipsKissPulsesSlippery".Localized());

                    if (Player.BiggestBreastSize() > 1)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.WrappingPermanentlyJutting".Localized());
                    else
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TracingSlenderCrisscrossingIvy".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ContinuingBackNeckThicken".Localized());

                    if (Player.BiggestBreastSize() > 1)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BreastsPressedTogetherVines".Localized());

                    if (!Player.HasTailInsteadOfLegs() && !Player.IsGoo())
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ThighsSpreadWideOpen".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.NectarDroolingFlowerClit".Localized());

                    if (Player.HasCock())
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AndEachCock".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BobbingPermanentSemiArousal".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.YoungSaplingEncasingGesture".Localized());

                    Player.Orgasm("Vaginal");

                    Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 20f));

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), GirlsGetANiceManToBadEnd);
                }
                else
                {
                    Player.Orgasm("Anal");

                    Player.ApplyStats(new DynStat(GameStats.Libido, 5f), new DynStat(GameStats.Sensitivity, 20f));

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PullsArmorBreakingFastenings".Localized());

                    if (Player.Anal.Wetness > 0)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BetrayingDepravityCrackOpens".Localized());
                    else
                        if (Player.AnalCapacity() < 30)
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.DifficultyForcesPuckerAs".Localized());
                        else
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.DespiteClenchingImpediment".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.EyesConcentrationShudderSquirm".Localized());

                    Player.ButtChange(20, true, true, false);

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AgonizingExpansionContorts".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MurmursPullingFreeExtracts".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MoansQuiversPushesGrowth".Localized());

                    if (Player.Anal.Wetness > 0)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TryCantSeeBack".Localized());
                    else
                        if (Player.AnalCapacity() < 30)
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.JawCollarboneFloridHead".Localized());
                        else
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PressesBackBrushingNipples".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SapOozingMemberPassage".Localized());

                    if (Player.Anal.Wetness > 0)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MinglingLubricationEasingTempo".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PantsShriekExcitedPu".Localized());

                    if (Player.Anal.Wetness > 0 || Player.AnalCapacity() > 30)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SpillingDepositedFluidsLegs".Localized());
                    else
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.OncePuckerSqueezesTrapping".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.FreshlyShaftShrivelWithering".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.FluidsMinglePinprickSensation".Localized());

            if (Player.HasFur() || Player.HasScales())
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.UnderSkinfurscales".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PanicIntensifiesTendrils".Localized());

                    if (Player.BiggestBreastSize() < 1)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.FoldedAtopChest".Localized());
                    else
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.CrookedHandsRestingChest".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ContinueFaceReachingCanopy".Localized());

                    if (Player.BiggestBreastSize() >= 1)
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BreastsExposedFromSides".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.LegsConcealedButtcheeksTrunk".Localized());

                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AdmiresLovelyAmbianceSticks".Localized());

                    Player.ApplyStats(new DynStat(GameStats.Lust, (float)(Player.MaxLust)));

                    ClearButtons();

                    AddButton(0, "Common.Button.Next".Localized(), HolliAndGenderlessSittingInATree);
                }
        }

        public void HolliPlaysWithPenisesBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.YoungHairedStepsPortal".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.ApproachesOuterBarksHumanoid".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.HelloChampionHollicyntheaDryad".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.RegardsCalmlyHelpMoreso".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.GesturesSecondSilentFigure".Localized());

            if (Player.BiggestBreastSize() < 1)
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.MasculineShapeItBears".Localized());
            else
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.FeminineShapeNonethelessBears".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.StandingErectIronBlushes".Localized(
                Player.MultiCockDescript()
            ));

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.TeasesNotCompanionAnswer".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.GraciouslyAcceptsContinues".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.NewestChampionCountersI".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.GesturesAgainCompanionCocks".Localized());

            if (Player.SmallestCockArea() > 36)
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SilentSmallestShuddersShrink".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.WaversAppearsDecisionQuiet".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.MurmursNothingAshamedExtends".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.BeginsGrinCutsPushing".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.CoosPurityHumbleDryads".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.YelpsShaftSlidesPrecum".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.TimeReachesThirdTense".Localized());

            if (Player.Cocks.Count > 1)
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SprayingStomachWasteSeed".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.FillingChampionPussyCascades".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.DryadGivesPushesTits".Localized());

            if (Player.CockTotal() > 1)
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SLetter".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.DroolingAftershocksOntoBreasts".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.PromptsSnapOpenFi".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.DazedStrugglesWobblingPounding".Localized());

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SpitsDistantRetreatingFool".Localized());

            GameOver();
        }

        public void GirlsGetANiceManToBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WeedyYoungStepsPortal".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CallsBehindYouthWelcome".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HeStutteringIdentifyYourself".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WomanAnswersNotChallenge".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ThatsWellExplainsNude".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.GlancesErectPenisPrecum".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LeftLacksResidentOlder".Localized());

            if (Player.BiggestBreastSize() > 1)
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestCuppedVinesOut".Localized());
            else
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestCrisscrossedFineVines".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LegsElevatedSpreadDisplay".Localized());

            if (Player.Balls > 0 && Player.BallSize > 10 && Player.Gender == Gender.Hermaphrodite)
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksAballsSwingingBelow".Localized());
            else
                if (Player.Balls > 0 && Player.BallSize <= 10 && Player.Gender == Gender.Hermaphrodite)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksBallsOverVagina".Localized());
                else
                    if (Player.Gender == Gender.Hermaphrodite)
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksAndVagina".Localized());
                    else
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WetDroolingBegsFilled".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.EyesCoveredBitingLicking".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ApproachesReachesCanopyBranch".Localized());

            if (Player.BiggestBreastSize() >= 1)
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestPlaceholder".Localized());
            else
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.VinesWord".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.PluckingFruitThrows".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LeafyNearestPlantHungry".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HandlesReverentlyDontThank".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.RetortsSmileToldBefore".Localized());

            if (Player.BallSize > 10 && Player.Balls > 0)
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LiftingTesticlesRevealPussy".Localized());
            else
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SpreadingExposingEagerHole".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.MoansNearlyRippingGratefully".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SaysQuietlyBehindFree".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.BuddingThrustingEagerly".Localized());

            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ReachesHandfulsBreastThumb".Localized());

                if (Player.Gender == Gender.Female)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WomanWord".Localized());
                else
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HermaphroditeWord".Localized());

                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SMouthCloseBliss".Localized());
            }
            else
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LeansKissesHermaphroditeChest".Localized());

            if (Player.HasCock())
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.NervousGlanceHandCock".Localized());
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HandSlidesHoleLub".Localized());

                if (Player.GetClitLength() < 6)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SmearsLittleButtonUnderside".Localized());
                else
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CoatsSwollenJerksValiantly".Localized());

                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.AsksUppingFeelGood".Localized());
            }

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.AsIfReply".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WomanWordAgain".Localized());
            else
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HermaphroditeWordAgain".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ArchesVaginaClampsTool".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.TextSemicolonSpace".Localized());

                if (Player.Balls > 0)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.BallsCloserBody".Localized());

                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HerTremblingDick".Localized());

                if (Player.CockTotal() == 1)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Throbs".Localized());
                else
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SThrob".Localized());

                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.AsSheNearsClimax".Localized());
            }

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SentencePeriod".Localized());

            if (Player.HasCock())
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.YouthsWidenSqueezesShut".Localized());

                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SLetter".Localized());

                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.TowardHerFace".Localized());

                if (Player.BiggestCockArea() < 20)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SemenSpurtsDribblesPooling".Localized());

                    if (Player.CumQ() >= 500)
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SoFinishesPartnerTrunk".Localized());
                }
                else
                    if (Player.BiggestCockArea() < 80)
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.MouthCockheadGulpsDrooling".Localized());

                        if (Player.CumQ() >= 500)
                            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WavesDontSwellingBelly".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.DwarfedMaleMassiveSize".Localized());

                        if (Player.CumQ() >= 1000)
                            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.OverproductivePushesCanopySoo".Localized());
                    }
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ALetter".Localized());

                if (Player.Wetness() < 3)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Gush".Localized());
                else
                    if (Player.Wetness() < 4)
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Rush".Localized());
                    else
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Torrent".Localized());

                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.FemaleOrgasmMouthWide".Localized());
            }

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.PushedLimitDisplayGroans".Localized());

            if (Player.CockTotal() == 1)
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CockWord".Localized());
            else
                if (Player.CockTotal() > 1)
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksWord".Localized());
                else
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestWord".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.GoesSlackJawedSquirts".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SaysComingSensesWithdrawing".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.RepliesPassingOverSilent".Localized());

            if (Player.Gender == Gender.Female)
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WomanWordThird".Localized());
            else
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HermaphroditeWordThird".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SStomachEnjoyedBes".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.KneelingTakesCockQui".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SaysPullsWhat".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SeedMothersPowerCreatures".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ExclaimsGatheringClothesFruit".Localized());

            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WatchesSightGoodT".Localized());

            GameOver();
        }

        public void HolliAndGenderlessSittingInATree()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.TallTravelerColorsDissolve".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.DrawsNotOnlyIdea".Localized());

            if (!(Player.HasFur() || Player.HasScales()))
                OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.ColoredSkintoneHyphen".Localized());
            else
                OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.CoveredInSkinfurscales".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.WrappingAroundTreeImpossible".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.GreenSkinnedStrangerUnder".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.NodsWarilyCanopy".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.GreenLaughsDryadsGoddess".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.StrangerThinksJungleFound".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.ReachesCanopyWaitingChampion".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.AsksStrangerBlessing".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.LowersSmilesYours".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.NodsSlowlyAboutBuddy".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.BeginsProspectiveWeird".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.MumblesClothingPrincipled".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.AgreesIndeedChoiceAccept".Localized());

            if (Player.HasScales())
                OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.QuiverArousalShakesScales".Localized());
            else
                if (Player.HasFur())
                    OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.FurPricksUpArousal".Localized());
                else
                    OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.BlushLustColorsSkinfurscales".Localized());

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.Ellipsis".Localized());

            GameOver();
        }

        public void AmilyComesBack()
        {
            ClearOutput();

            HolliSprite();

            OutputText("Scenes.Holliscene.AmilyComesBack.ArrivesBelongingsShoulderKnew".Localized());

            OutputText("Scenes.Holliscene.AmilyComesBack.AmilyAddedLoversBold".Localized());

            CoC.Instance.Flags.Set(GameFlagIds.AmilyFollower, 1);

            CoC.Instance.Flags.Set(GameFlagIds.AmilyVillageEncountersDisabled, 1);

            DoNext(PlayerMenu);
        }

        public void HolliAskGlades()
        {
            ClearOutput();

            if (!Save.GetBool("askedGlades"))
            {
                Save.SetBool("askedGlades", true);

                OutputText("Scenes.Holliscene.HolliAskGlades.NotingResemblanceCorruptDryads".Localized());

                OutputText("Scenes.Holliscene.HolliAskGlades.SmilesFloralScentArt".Localized());

                OutputText("Scenes.Holliscene.HolliAskGlades.MoreThemMaybe".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HolliFuckedToday) == 0)
                {
                    OutputText("Scenes.Holliscene.HolliAskGlades.GlancesNervouslySinkingBeckons".Localized());

                    OutputText("Scenes.Holliscene.HolliAskGlades.DoesntSeemBudgeLying".Localized());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) != 0)
                    {
                        OutputText("Scenes.Holliscene.HolliAskGlades.HumsDelightApproval".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.ForSecondOminousTrembling".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.SeeingExpressionSquirmsEnjoy".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.HolliAwaySpreadsPetals".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.GetWhatWantedThink".Localized());

                        Save.SetBool("growingGlades", true);
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.HolliAskGlades.BeckonsCloserAromaTingling".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.SatisfiedLeansRufflingHair".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.SeeingExpressionYoull".Localized());
                    }

                    OutputText("Scenes.Holliscene.HolliAskGlades.GetWantedThinkAgain".Localized());

                    Save.SetBool("growingGlades", true);
                }
            }
            else
            {
                RegisterMacro("growth", Save.GetBool("growingGlades"));

                OutputText("Scenes.Holliscene.HolliAskGlades.AskGrowingLessMore".Localized());

                if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) != 0)
                {
                    OutputText("Scenes.Holliscene.HolliAskGlades.HolliDoesntPleasedDefy".Localized());

                    OutputText("Scenes.Holliscene.HolliAskGlades.GroundTremblesShrinksTree".Localized());
                }
                else
                    if (Save.GetBool("growingGlades"))
                    {
                        OutputText("Scenes.Holliscene.HolliAskGlades.GlancesDownWhySighs".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.HollifedTremblesDisappears".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.HolliAskGlades.GlancesDownSighsMaybe".Localized());

                        OutputText("Scenes.Holliscene.HolliAskGlades.DisappearsTreeConsiderDone".Localized());
                    }

                if (CoC.Instance.Flags.Get(GameFlagIds.HolliSubmissive) != 0 || CoC.Instance.Flags.Get(GameFlagIds.HolliFuckedToday) != 0)
                    Save.SetBool("growingGlades", !Save.GetBool("growingGlades"));
            }

            DoNext(() => TreeMenu());
        }

    }
}
