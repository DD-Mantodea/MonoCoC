//  Tools/PortKit/emit.mjs  plan.json 
//  Haxe TODO 

using MonoCoC.Attributes;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using MonoCoC.OptionSystem;
using System;
using System.Linq;
using MonoCoC.Contents.Achievements;
using MonoCoC.Contents.Pregnancy;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Npcs;
using MonoCoC.Contents.Internals;
using System.Collections.Generic;
using MonoCoC.Utils;
using MonoCoC.Contents.World;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Enemies.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    [Register("coc:holli_scene")]
    public class HolliScene : Scene
    {



        private static bool noFur => !Player.HasFur();

        // 原版 AmilyScene.hx:277 amilyFurry() = flags[KFLAGS.AMILY_NOT_FURRY] == 0 && !noFur。
        // AmilyScene 本体未移植（C# 只有 AmilyEnemy / AmilyLover / AmilySlave），这里按原版函数体落地，
        // 不造假场景；HolliScene.hx:1223/1233 的两处文本分支靠它选词。
        private static bool AmilyFurry() =>
            CoC.Instance.Flags.Get(VanillaFlags.AmilyNotFurry) == 0 && !noFur;

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        private static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        private static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        private static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1);

        private static void ReturnToCamp(int hours) => Go(SceneRegistry.CreateOrDefault("coc:camp"));

        //  saveContent
        private static SaveContent Save => SaveContent(new Identifier("coc", "HolliScene"));

        //  get_xxx
        private string debugName => "Holli";
        private string debugHint => "";

        // 原版 HolliScene（scenes/npcs/HolliScene.hx）是 NPCAwareContent：没有 entry()，
        // 由营地/森林事件直接调它的方法（同 MarbleScene.cs:20 的处理：Entry 留空）。
        public override void Entry()
        {
        }

        // Reset, Load, OnAscend, SaveToObject, Get_debugName, Get_debugHint, DebugMenu, New

        // Haxe Reset()   outputText 0 
        public void Reset()
        {

            Save.SetBool("askedGlades", false);
            Save.SetBool("growingGlades", false);
            Save.SetInt("fruitsEaten", 0);
        }

        // Haxe HolliFollower()   outputText 0 
        public bool HolliFollower()
        {

            return CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerLevel) >= 4 && CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerKilled) == 0;
        }

        // Haxe HolliSprite()   outputText 0 
        public void HolliSprite()
        {

        }

        // Haxe FightHolli()   outputText 0 
        public void FightHolli()
        {

            CoC.Instance.Flags.Set(VanillaFlags.FoughtHolli, 1);
            HolliSprite();
            // 原版 HolliScene.hx:128 `jojoAssisting = false`：Jojo 助战（Holli.hx:81/115）未移植，
            // C# 该状态恒 false，这里不再赋值。
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "holli")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")), canFlee: false);
        }

        // Haxe TreeMenu()   outputText 18 
        public void TreeMenu(bool output = true)
        {
            ClearOutput();

            HolliSprite();
            if (CoC.Instance.Flags.Get(VanillaFlags.FoughtHolli) > 0)
            {
                OutputText("Scenes.Holliscene.TreeMenu.DemonTreeStillStares".Localized());
                ClearButtons();
                AddButton(0, "Fight", FightHolli);
                if (Player.HasKeyItem("Jojo's Talisman") && !SceneInstance<JojoScene>.Get().CampCorruptJojo())
                {
                    AddButton(1, "Call Jojo", CallDatJojo);
                }
                AddButton(14, "Back", () => Go<Camp>());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerLevel) == 1)
                {
                    if (output)
                    {
                        OutputText("Scenes.Holliscene.TreeMenu.SproutLooksLeafyShoot".Localized());
                    }
                    ClearButtons();
                    AddButton(0, "Burn It", () => HolliDestroyPrompt(DestroyDatFukkinTree, () => TreeMenu()));
                    AddButton(14, "Leave It", () => Go<Camp>());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerLevel) == 2)
                    {
                        if (output)
                        {
                            OutputText("Scenes.Holliscene.TreeMenu.LargePlantVioletBloom".Localized());
                        }
                        ClearButtons();
                        if (Player.HasCockThatFits(100) && Player.Lust >= 33)
                        {
                            AddButton(0, "Fuck It", FuckFuckingFuckFlowerP2);
                        }
                        else
                        {
                            AddButton(0, "Fuck It", null);
                        }
                        if (Player.HasVagina() && Player.Lust >= 33)
                        {
                            AddButton(1, "Ride Stamen", RideDatFuckingFukkFlowerP2);
                        }
                        else
                        {
                            AddButton(1, "Ride Stamen", null);
                        }
                        AddButton(10, "Destroy It", () => HolliDestroyPrompt(DestroyDatFuckingPlantAtP2, () => TreeMenu()));
                        AddButton(14, "Leave It", () => Go<Camp>());
                    }
                    else
                    {
                        if (CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerLevel) == 3)
                        {
                            if (output)
                            {
                                OutputText("Scenes.Holliscene.TreeMenu.FamiliarPlantBlossomedTree".Localized());
                            }
                            ClearButtons();
                            if (Player.HasCock() && Player.Lust >= 33)
                            {
                                AddButton(0, "Fuck Flower", FuckTheFlower);
                            }
                            else
                            {
                                AddButton(0, "Fuck Flower", null);
                            }
                            if (Player.HasVagina() && Player.Lust >= 33)
                            {
                                AddButton(1, "Ride Tentacle", RideTheWalrusP3);
                            }
                            else
                            {
                                AddButton(1, "Ride Tentacle", null);
                            }
                            AddButton(2, "Drink Sap", DrinkThePlantGirlsSap);
                            if (Player.HasPerk(VanillaPerks.Dragonfire) || Player.HasPerk(VanillaPerks.FireLord) || Player.HasPerk(VanillaPerks.Hellfire) || Player.HasStatusEffect(VanillaStatusEffects.KnowsWhitefire)                                                             || Player.HasPerk(VanillaPerks.EnlightenedNinetails) || Player.HasPerk(VanillaPerks.CorruptedNinetails))
                            {
                                if (output)
                                {
                                    OutputText("Scenes.Holliscene.TreeMenu.BurnedDownByFire".Localized());
                                }
                                AddButton(10, "Torch It", () => HolliDestroyPrompt(TorchP3Tree, () => TreeMenu()));
                            }
                            else
                            {
                                AddButton(10, "Torch It", null);
                            }
                            AddButton(14, "Leave It", () => Go<Camp>());
                        }
                        else
                        {
                            if (output)
                            {
                                OutputText("Scenes.Holliscene.TreeMenu.WalkingEdgeCampApproach".Localized());
                                if (CoC.Instance.Flags.Get(VanillaFlags.HolliSubmissive) == 1)
                                {
                                    OutputText("Scenes.Holliscene.TreeMenu.WelcomeBackSayStart".Localized());
                                    if (CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) > 0 && Random.Shared.Next((int)(4)) == 0)
                                    {
                                        OutputText("Scenes.Holliscene.TreeMenu.WantFertilizeAgainReady".Localized());
                                    }
                                    else
                                    {
                                        if (CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) > 0 && Random.Shared.Next((int)(3)) == 0)
                                        {
                                            OutputText("Scenes.Holliscene.TreeMenu.WantUseTentaclesRestless".Localized());
                                        }
                                        else
                                        {
                                            if (Random.Shared.Next((int)(2)) == 0)
                                            {
                                                OutputText("Scenes.Holliscene.TreeMenu.ComeDrinkNectarSip".Localized());
                                            }
                                            else
                                            {
                                                OutputText("Scenes.Holliscene.TreeMenu.HowMayServeCommand".Localized());
                                            }
                                        }
                                    }
                                    OutputText("Scenes.Holliscene.TreeMenu.SayEnd".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Holliscene.TreeMenu.WelcomeBackAgain".Localized());
                                    if (CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) > 0 && Random.Shared.Next((int)(3)) == 0)
                                    {
                                        OutputText("Scenes.Holliscene.TreeMenu.WantGardeningExhausting".Localized());
                                    }
                                    else
                                    {
                                        if (CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) > 0 && Random.Shared.Next((int)(2)) == 0)
                                        {
                                            OutputText("Scenes.Holliscene.TreeMenu.ComePlayTentaclesRestless".Localized());
                                        }
                                        else
                                        {
                                            if (Random.Shared.Next((int)(2)) == 0)
                                            {
                                                OutputText("Scenes.Holliscene.TreeMenu.ComeDrinkHungryAdventurer".Localized());
                                            }
                                            else
                                            {
                                                OutputText("Scenes.Holliscene.TreeMenu.WantMaraeFinestCreation".Localized());
                                            }
                                        }
                                    }
                                    OutputText("Scenes.Holliscene.TreeMenu.SayEndAgain".Localized());
                                }
                                // 未移植：原版 HolliScene.hx:229 `akky.locationDesc("Holli")`（Akky 宠物的位置描述；C# 无 Akky 宠物，同缺口见 IzmaScene/MarbleScene/RathazulScene）。
                            }
                            ClearButtons();
                            AddNextButton("Appearance", HolliAppearance);
                            AddNextButton("Talk", HolliTalkMenu);
                            AddNextButton("Sex", HolliSexMenu);
                            // 原版 HolliScene.hx:238/245 用到 game.swamp.alrauneScene 的 askHolli / holliSolution
                            // （AlrauneScene.cs:1147 / :1201）；ASKEDHO = 1 << 8（AlrauneScene.cs:71）。
                            var alrauneQuestAsked = SaveContent(new Identifier("coc", "AlrauneScene")).GetInt("questAsked", 0);
                            var alrauneScene = SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp.AlrauneScene>.Get();
                            var vinesAsked = (alrauneQuestAsked & (1 << 8)) != 0;

                            if (CoC.Instance.Flags.Get(VanillaFlags.HolliSubmissive) == 0)
                            {
                                AddButton(4, CampGuards.IsGuard("Holli") ? "Stop Guarding" : "Guard Camp", AskHolliToWatch);
                                var milkTotal = Player.ItemCount(VanillaConsumables.SuccubiMilk) + Player.ItemCount(VanillaConsumables.PurifiedSuccubiMilk) + Player.ItemCount(Identifier.Parse("coc:isabella_milk")) + Player.ItemCount(VanillaConsumables.MinotaurMilk) + Player.ItemCount(Identifier.Parse("coc:sheep_milk"));
                                // 原版 HolliScene.hx:239
                                if (Player.Armor.Id == VanillaArmors.VineArmorBehavior && !vinesAsked)
                                {
                                    AddButton(10, "Vines", alrauneScene.AskHolli).Hint("Any idea how to fix this?");
                                }
                                // 原版 HolliScene.hx:241
                                else if (Player.Armor.Id == VanillaArmors.VineArmorBehavior)
                                {
                                    AddButton(10, "Cure Vines", alrauneScene.HolliSolution)
                                        .Hint("At long last, you can get this over with.")
                                        .DisableIf(!(Player.ItemCount(Identifier.Parse("coc:slimy_clothoth")) >= 5
                                            && Player.ItemCount(VanillaConsumables.PureHoney) >= 3
                                            && Player.ItemCount(Identifier.Parse("coc:snake_oil")) >= 3
                                            && milkTotal > 2
                                            && CoC.Instance.Flags.Get(VanillaFlags.HolliFuckedToday) == 1),
                                            "You haven't satisfied her demands: five slimy cloths, three bottles each of pure honey and snake oil, and three doses of milk from any source, in addition to feeding her.");
                                }
                            }
                            else
                            {
                                AddButton(4, "Guarding", AskBrokenHolliToGuard);
                                // 原版 HolliScene.hx:246
                                if (Player.Armor.Id == VanillaArmors.VineArmorBehavior && !vinesAsked)
                                {
                                    AddButton(10, "Vines", alrauneScene.AskHolli).Hint("Any idea how to fix this?");
                                }
                                // 原版 HolliScene.hx:248
                                else if (Player.Armor.Id == VanillaArmors.VineArmorBehavior)
                                {
                                    AddButton(10, "Cure Vines", alrauneScene.HolliSolution)
                                        .Hint("At long last, you can get this over with.")
                                        .DisableIf(Player.ItemCount(Identifier.Parse("coc:slimy_clothoth")) < 1,
                                            "You don't have what she needs.");
                                }
                            }
                            if (CoC.Instance.Flags.Get(VanillaFlags.HolliFruit) != 0 || Save.GetInt("fruitsEaten", 0) != 0)
                            {
                                AddButton(5, "Eat Fruit", EatHolliFruit).DisableIf(CoC.Instance.Flags.Get(VanillaFlags.HolliFruit) == 0, "There is no fruit for you to eat.");
                            }
                            if (CoC.Instance.Flags.Get(VanillaFlags.FollowerAtFarmHolli) == 0 && CoC.Instance.Flags.Get(VanillaFlags.FarmCorruptionStarted) == 1)
                            {
                                AddRowButton(1, "Farm Help", HelpWithFarm);
                            }
                            if (CoC.Instance.Flags.Get(VanillaFlags.CorruptedGladesDestroyed) > 0)
                            {
                                var gladesButton = AddRowButton(1, (Save.GetBool("growingGlades") ? "Less" : "More") + " Glades", HolliAskGlades);

                                if (gladesButton != null)
                                    gladesButton.Hint = "Ask Holli to " + (Save.GetBool("growingGlades") ? "stop" : "start") + " growing more corrupted glades.";
                            }
                            AddButton(14, "Leave", () => Scene.Go(SceneRegistry.CreateOrDefault("coc:camp")));
                        }
                    }
                }
            }
        }

        // Haxe HolliAppearance()   outputText 3 
        public void HolliAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliAppearance.TreeHousesHollicyntheaTowering".Localized());
            OutputText("Scenes.Holliscene.HolliAppearance.AttentionDrawnHolliOoz".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.HolliFruit) > 0)
            {
                OutputText("Scenes.Holliscene.HolliAppearance.ZeroBranchesTwistingBreeze".Localized()
                    CoC.Instance.Flags.Get(VanillaFlags.HolliFruit) == 1 ? "A large, purple fruit hangs" : NumberUtils.Num2Text(CoC.Instance.Flags.Get(VanillaFlags.HolliFruit)) + " large, purple fruits hang"
                );
            }
            TreeMenu();
        }

        // Haxe HolliTalkMenu()   outputText 1 
        public void HolliTalkMenu()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkMenu.WhatSubjectDemonDryad".Localized());
            ClearButtons();
            AddNextButton("Marae", HolliTalkMarae).Hint("Ask her about her progenitor.");
            AddNextButton("Magic", HolliTalkMagic).Hint("How exactly does she do what she does?");
            AddNextButton("Rooted", HolliTalkRooted).Hint("Does she ever wish she wasn't bound to that tree?");
            AddNextButton("Reproduction", HolliTalkReproduction).Hint("Ask about the boughs and the trees.");
            if (Save.GetInt("fruitsEaten", 0) >= 5)
            {
                AddNextButton("Seeds", HolliTalkSeeds).Hint("What happens to the cores you throw away?");
            }
            SetExitButton("Back", () => TreeMenu());
        }

        // Haxe HolliTalkMarae()   outputText 6 
        public void HolliTalkMarae()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkMarae.FirstCameGiftMarae".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMarae.CantGoLike".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMarae.HowKnowGoddessFirst".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMarae.SmilesDaughterLinked".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMarae.JoltSuddenContactTendril".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMarae.JustCloserLovelyStart".Localized());
            HolliTalkMenu();
        }

        // Haxe HolliTalkMagic()   outputText 5 
        public void HolliTalkMagic()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkMagic.IncredibleAbilityGrowLife".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMagic.ConsidersWordsConsternation".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMagic.AnyWayTeachSimilar".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMagic.SmilesSlightMockery".Localized());
            OutputText("Scenes.Holliscene.HolliTalkMagic.AndBesidesRatherFucki".Localized());
            HolliTalkMenu();
        }

        // Haxe HolliTalkRooted()   outputText 4 
        public void HolliTalkRooted()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkRooted.LivedWholeLifeSpot".Localized());
            OutputText("Scenes.Holliscene.HolliTalkRooted.InResponseSimplyGiggles".Localized());
            OutputText("Scenes.Holliscene.HolliTalkRooted.EyebrowsArchDoesntKnow".Localized());
            OutputText("Scenes.Holliscene.HolliTalkRooted.SniffsEarthNotOff".Localized());
            HolliTalkMenu();
        }

        // Haxe HolliTalkReproduction()   outputText 6 
        public void HolliTalkReproduction()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkReproduction.PersistentJuicesNotGrasp".Localized((flags[KFLAGS.HOLLI_FRUIT_EXPLAINED] != 0 ? "she's never produced anything more than the fruit that grows on her boughs" : "it's never proven fruitful")));
            if (CoC.Instance.Flags.Get(VanillaFlags.HolliFruitExplained) != 0)
            {
                OutputText("Scenes.Holliscene.HolliTalkReproduction.SighsCannotBreedWant".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.HolliTalkReproduction.SmirksReachesStrokesBranch".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.HolliFruitExplained, 1);
            }
            OutputText("Scenes.Holliscene.HolliTalkReproduction.LeavesLotQuestionsUnanswered".Localized());
            OutputText("Scenes.Holliscene.HolliTalkReproduction.RespondsLivelyGlint".Localized());
            OutputText("Scenes.Holliscene.HolliTalkReproduction.LaughsSurprisingGaiety".Localized());
            HolliTalkMenu();
        }

        // Haxe HolliTalkSeeds()   outputText 6 
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
            AddNextButton("Embrace", HolliTalkSeedsEmbrace).Hint("All is well.");
            AddNextButton("Reject", HolliTalkSeedsReject).Hint("You can't be here right now.");
        }

        // Haxe HolliTalkSeedsEmbrace()   outputText 3 
        public void HolliTalkSeedsEmbrace()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkSeedsEmbrace.LetBreathPeaceComfort".Localized());
            OutputText("Scenes.Holliscene.HolliTalkSeedsEmbrace.RatherTireWarmthSt".Localized());
            OutputText("Scenes.Holliscene.HolliTalkSeedsEmbrace.SwirlingBlackPitsEnrapture".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Lust, 50f));
            HolliSexMenu();
            // 原版 HolliScene.hx:353 `button(14).hide()`：隐藏退出按钮。
            if (ButtonAt(14) is { } hideExitButton)
                hideExitButton.Visible = false;
        }

        // Haxe HolliTalkSeedsReject()   outputText 3 
        public void HolliTalkSeedsReject()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliTalkSeedsReject.TearAwayStumbleBack".Localized());
            OutputText("Scenes.Holliscene.HolliTalkSeedsReject.RetreatHolliCallOut".Localized());
            OutputText("Scenes.Holliscene.HolliTalkSeedsReject.ShutBestNotThink".Localized());
            ReturnToCamp(1);
        }

        // Haxe HelpWithFarm()   outputText 4 
        public void HelpWithFarm()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HelpWithFarm.AskPlantGoddessFarm".Localized());
            OutputText("Scenes.Holliscene.HelpWithFarm.BreathesVerdantOrderliness".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.HolliFuckedToday) == 0)
            {
                OutputText("Scenes.Holliscene.HelpWithFarm.SmirksLittlePatchBoringness".Localized());
                TreeMenu(false);
            }
            else
            {
                OutputText("Scenes.Holliscene.HelpWithFarm.SaysEventuallyHelpNourishing".Localized());
                ClearButtons();
                AddButton(0, "Yes", DoFarmFuckery);
                AddButton(1, "No", NoPlzDontFuckWithFarm);
            }
        }

        // Haxe DoFarmFuckery()   outputText 2 
        public void DoFarmFuckery()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DoFarmFuckery.ClosesEyesHumsReverberate".Localized());
            OutputText("Scenes.Holliscene.DoFarmFuckery.ItsHappeningDoggieEnjoy".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.FollowerAtFarmHolli, 1);
            ReturnToCamp(1);
        }

        // Haxe NoPlzDontFuckWithFarm()   outputText 1 
        public void NoPlzDontFuckWithFarm()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.NoPlzDontFuckWithFarm.SighsFickleMortalGoddess".Localized());
            TreeMenu(false);
        }

        // Haxe FertilizeHolli()   outputText 0 
        public void FertilizeHolli(bool cock = true)
        {

            float odds = 20;
            if (cock && Player.HasCock())
            {
                odds += Player.CumQ() / 300;
                if (odds > 40)
                {
                    odds = 40;
                }
                if (Player.HasPerk(VanillaPerks.MaraesGiftStud))
                {
                    odds += 10;
                }
                if (Player.HasPerk(VanillaPerks.FerasBoonAlpha))
                {
                    odds += 10;
                }
                if (Player.HasPerk(VanillaPerks.ElvenBounty))
                {
                    odds += 2;
                }
            }
            if (!cock && Player.HasVagina())
            {
                odds += Player.TotalFertility() / 5;
                if (odds > 40)
                {
                    odds = 40;
                }
                if (Player.HasPerk(VanillaPerks.MaraesGiftFertility))
                {
                    odds += 10;
                }
                if (Player.HasPerk(VanillaPerks.FerasBoonBreedingBitch))
                {
                    odds += 10;
                }
                if (Player.HasPerk(VanillaPerks.ElvenBounty))
                {
                    odds += 2;
                }
            }
            if (odds >= Random.Shared.Next((int)(101)))
            {
                CoC.Instance.Flags.Set(VanillaFlags.HolliFruit, CoC.Instance.Flags.Get(VanillaFlags.HolliFruit) + 1 + Random.Shared.Next((int)((float)(int)Math.Floor(odds / 10))));
            }
        }

        // Haxe HolliDestroyPrompt()   outputText 1 
        public void HolliDestroyPrompt(Action destroyFunc, Action backFunc)
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliDestroyPrompt.EvenAfterWonderPlant".Localized());
            // 原版 HolliScene.hx:432-435 holliDestroyPrompt(destroyFunc, backFunc) → doYesNo(destroyFunc, backFunc)。
            // emit 把参数名写成了 DestroyFunc/BackFunc，这里改回小写形参与 DoYesNo。
            DoYesNo(destroyFunc, backFunc);
        }

        // Haxe GetASprout()   outputText 5 
        public void GetASprout()
        {
            ClearOutput();

            HolliSprite();
            OutputText("Scenes.Holliscene.GetASprout.AroundCampPlantLife".Localized());
            OutputText("Scenes.Holliscene.GetASprout.BrieflyWonderRootSoil".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.CorruptMaraeFollowupEncounterState) == 2)
            {
                OutputText("Scenes.Holliscene.GetASprout.PromisedPlantTreeCamp".Localized());
            }
            OutputText("Scenes.Holliscene.GetASprout.LookCloserPurpleLeaves".Localized());
            OutputText("Scenes.Holliscene.GetASprout.DoDestroyBold".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerLevel, 1);
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, 0);
            ClearButtons();
            AddButton(0, "Yes", () => HolliDestroyPrompt(DestroyDatFukkinTree, LetZeFuckingSproutLive));
            AddButton(1, "No", LetZeFuckingSproutLive);
        }

        // Haxe DestroyDatFukkinTree()   outputText 3 
        public void DestroyDatFukkinTree()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DestroyDatFukkinTree.ReachExcavateTaintedSapling".Localized());
            OutputText("Scenes.Holliscene.DestroyDatFukkinTree.SparksSmokePurpleFire".Localized());
            OutputText("Scenes.Holliscene.DestroyDatFukkinTree.HopefulNoMorePlants".Localized());
            Player.UpgradeBeautifulSword();
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, -5f));
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerKilled, 1);
            ReturnToCamp(1);
        }

        // Haxe LetZeFuckingSproutLive()   outputText 2 
        public void LetZeFuckingSproutLive()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.LetZeFuckingSproutLive.StayCorruptHasntDone".Localized());
            OutputText("Scenes.Holliscene.LetZeFuckingSproutLive.PlantAddedItemsBold".Localized());
            Go<Camp>();
        }

        // Haxe FuckPlantGrowsToLevel2()   outputText 9 
        public void FuckPlantGrowsToLevel2()
        {
            ClearOutput();

            HolliSprite();
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerLevel, 2);
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, 0);
            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.SoundCreakingSnappingVines".Localized());
            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.WhereSaplingWaistFlower".Localized());
            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.SkinFlushesSexualEnticements".Localized());
            if (Player.Gender == Gender.Genderless)
            {
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.SucksNoGenitals".Localized());
            }
            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.MaraeIdeaOfGift".Localized());
            if ((int)Player.Gender > 0)
            {
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.CouldWhirlSlowly".Localized());
            }
            OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.DestroyingSafestOption".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Lust, 33f));
            if (silly)
            {
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.WhatDoBold".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckPlantGrowsToLevel2.WhatToDoBold".Localized());
            }
            ClearButtons();
            if (Player.HasCockThatFits(100) && Player.Lust >= 33)
            {
                AddButton(0, "Fuck It", FuckFuckingFuckFlowerP2);
            }
            else
            {
                AddButton(0, "Fuck It", null);
            }
            if (Player.HasVagina() && Player.Lust >= 33)
            {
                AddButton(1, "Ride Stamen", RideDatFuckingFukkFlowerP2);
            }
            else
            {
                AddButton(1, "Ride Stamen", null);
            }
            AddButton(10, "Destroy It", () => HolliDestroyPrompt(DestroyDatFuckingPlantAtP2, PlayerMenu));
            AddButton(14, "Do Nothing", () => Go<Camp>());
        }

        // Haxe DestroyDatFuckingPlantAtP2()   outputText 2 
        public void DestroyDatFuckingPlantAtP2()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DestroyDatFuckingPlantAtP2.LettingGrowThreatenSafety".Localized());
            OutputText("Scenes.Holliscene.DestroyDatFuckingPlantAtP2.FireFluttersSparksBlotting".Localized());
            Player.UpgradeBeautifulSword();
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, -5f));
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerKilled, 1);
            ReturnToCamp(1);
        }

        // Haxe FuckFuckingFuckFlowerP2()   outputText 19 
        public void FuckFuckingFuckFlowerP2()
        {
            ClearOutput();

            var x = Player.CockThatFits(100);
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.UndoingArmorApproachPulsating".Localized());
            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.LaughAmusementCreamyTreat".Localized(
                Player.CockDescript(x),
                Player.CockHead(x)
            ));
            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.EnoughQuakeSlideGround".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.BallsPlaceholder".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.LegsPlaceholder".Localized());
            }
            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.StickyPuddlePumpSheath".Localized());
            if (Player.Corruption < 33)
            {
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.TimidlyLookAroundAudience".Localized());
            }
            else
            {
                if (Player.Corruption < 66)
                {
                    OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.WorriedlyGlanceAround".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.GlanceAroundSexyShow".Localized());
                }
            }
            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.PantingUnbearableGiveUp".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Cocks.Exists(c => c.SheathOn))
            {
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.InsideViolatedSheath".Localized());
            }
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
            {
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.BallsGurgleHappySound".Localized());
            }
            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.InsideLiquidHeatSpurting".Localized());
            if (Player.CumQ() > 500)
            {
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.YouForthProdigiousEssence".Localized());
            }
            if (Player.CumQ() > 1500)
            {
                OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.MeagerCannotSwallowFroths".Localized());
            }
            OutputText("Scenes.Holliscene.FuckFuckingFuckFlowerP2.SatisfiedExhaustedSlumpRelease".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            CoC.Instance.Flags.Set(VanillaFlags.TimesFuckedFlower, 1);
            if (CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) < 1000)
            {
                CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) + 4);
            }
            Player.ChangeFatigue(5);
            ReturnToCamp(1);
        }

        // Haxe RideDatFuckingFukkFlowerP2()   outputText 30 
        public void RideDatFuckingFukkFlowerP2()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.ShuckArmorTowardsViolet".Localized());
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.GrabbingPlantMonsSilky".Localized());
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.DrivenOnslaughtFlopBack".Localized());
            if (Player.GetClitLength() > 4)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.HeavyBuzzerFlexibleCock".Localized());
            }
            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.EachPartnerTwistingSlicked".Localized());
            }
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.NothingOccupyRovingSlide".Localized());
            if (Player.BreastRows.Count > 1)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.LowestTitsCuppingRow".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.ChestPlaceholder".Localized());
            }
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.CaressingThe".Localized());
            if (Player.BiggestBreastSize() > 1)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Ir".Localized());
            }
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SwellSearchTightenedNipples".Localized());
            if (Player.LactationQ() > 50)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.OneAfterOtherBegin".Localized());
                if (Player.LactationQ() < 150)
                {
                    OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Trickle".Localized());
                }
                else
                {
                    if (Player.LactationQ() < 500)
                    {
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Leak".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SprayTorrentsOf".Localized());
                    }
                }
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.MilkPeriod".Localized());
            }
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.MoanUnabashedMindlessO".Localized());
            if (!false && Player.Corruption + Player.Libido < 150)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Embarrassment".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Lust".Localized());
            }
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SentencePeriod".Localized());
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.ThoughtBatteredConsciousness".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.EachExplodesReleasePetals".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.YLetter".Localized());
            }
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.OurQuiveringSoppingLast".Localized());
            if (Player.CumQ() < 50)
            {
                OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Drizzle".Localized());
            }
            else
            {
                if (Player.CumQ() < 250)
                {
                    OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Stream".Localized());
                }
                else
                {
                    if (Player.CumQ() < 750)
                    {
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Spray".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.Torrent".Localized());
                    }
                }
            }
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.TentaclesPullBackCreamed".Localized());
            Player.Orgasm("VaginalAnal");
            OutputText("Scenes.Holliscene.RideDatFuckingFukkFlowerP2.SwoonRelaxingSinkingGround".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            Player.ChangeFatigue(5);
            CoC.Instance.Flags.Set(VanillaFlags.TimesRiddenFlower, 1);
            if (CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) < 1000)
            {
                CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) + 4);
            }
            ReturnToCamp(1);
        }

        // Haxe FlowerGrowsToP3()   outputText 8 
        public void FlowerGrowsToP3()
        {
            ClearOutput();

            HolliSprite();
            OutputText("Scenes.Holliscene.FlowerGrowsToP3.AgainNoisesCornerCamp".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) > 0)
            {
                OutputText("Scenes.Holliscene.FlowerGrowsToP3.FuckingOnlyHelpedFaster".Localized());
            }
            OutputText("Scenes.Holliscene.FlowerGrowsToP3.WoodBowingCreakingWeight".Localized());
            OutputText("Scenes.Holliscene.FlowerGrowsToP3.FirstVaginalTentacleFlower".Localized());
            OutputText("Scenes.Holliscene.FlowerGrowsToP3.DollopMoistLandingHair".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerLevel, 3);
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, 0);
            ClearButtons();
            if (Player.HasCock() && Player.Lust >= 33)
            {
                AddButton(0, "Fuck Flower", FuckTheFlower);
            }
            else
            {
                AddButton(0, "Fuck Flower", null);
            }
            if (Player.HasVagina() && Player.Lust >= 33)
            {
                AddButton(1, "Ride Tentacle", RideTheWalrusP3);
            }
            else
            {
                AddButton(1, "Ride Tentacle", null);
            }
            AddButton(2, "Drink Sap", DrinkThePlantGirlsSap);
            if (Player.HasPerk(VanillaPerks.Dragonfire) || Player.HasPerk(VanillaPerks.FireLord) || Player.HasPerk(VanillaPerks.Hellfire) || Player.HasStatusEffect(VanillaStatusEffects.KnowsWhitefire)                                                             || Player.HasPerk(VanillaPerks.EnlightenedNinetails) || Player.HasPerk(VanillaPerks.CorruptedNinetails))
            {
                OutputText("Scenes.Holliscene.FlowerGrowsToP3.SupposeBurnMagicalFire".Localized());
                AddButton(10, "Torch It", () => HolliDestroyPrompt(TorchP3Tree, PlayerMenu));
            }
            else
            {
                AddButton(10, "Torch It", null);
            }
            OutputText("Scenes.Holliscene.FlowerGrowsToP3.SentencePeriod".Localized());
            OutputText("Scenes.Holliscene.FlowerGrowsToP3.WhatToDoBold".Localized());
            AddButton(14, "Leave It", () => Go<Camp>());
        }

        // Haxe FuckTheFlower()   outputText 32 
        public void FuckTheFlower()
        {
            ClearOutput();

            var x = Player.BiggestCockIndex();
            OutputText("Scenes.Holliscene.FuckTheFlower.SmilingShedArmorSnake".Localized());
            OutputText("Scenes.Holliscene.FuckTheFlower.BendSniffFlowerCunt".Localized());
            if (Player.HasVagina())
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.WetnessBuildsVaginaMaleness".Localized());
            }
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
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.SlidingTrunkGiantPussy".Localized());
            }
            OutputText("Scenes.Holliscene.FuckTheFlower.SentencePeriod".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.YourOtherPenis".Localized());
                if (Player.CockTotal() > 2)
                {
                    OutputText("Scenes.Holliscene.FuckTheFlower.EsRub".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.FuckTheFlower.Rubs".Localized());
                }
                OutputText("Scenes.Holliscene.FuckTheFlower.SensuallyAcrossOuterBlossom".Localized());
            }
            OutputText("Scenes.Holliscene.FuckTheFlower.SchliiickSlideSlorpPop".Localized());
            if (Player.Cocks.Exists(c => c.SheathOn))
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.SheathWord".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.BaseWord".Localized());
            }
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.GurglingBallsSlappingEdge".Localized());
            }
            OutputText("Scenes.Holliscene.FuckTheFlower.SmallSquirtSweetLubricants".Localized());
            OutputText("Scenes.Holliscene.FuckTheFlower.KeptLiquidSoftSnugness".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Holliscene.FuckTheFlower.JizzBlastsUncommonForce".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.MultipleEndowmentsAlabaster".Localized());
            }
            OutputText("Scenes.Holliscene.FuckTheFlower.SentencePeriodAgain".Localized());
            if (Player.CumQ() > 500)
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.PearlyRopesFlaskSlit".Localized());
            }
            if (Player.CumQ() > 1500)
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.WaveThickPumpingCock".Localized());
            }
            if (Player.HasVagina())
            {
                OutputText("Scenes.Holliscene.FuckTheFlower.HungryLongDeniedTrembles".Localized());
                if (Player.Wetness() < 4)
                {
                    OutputText("Scenes.Holliscene.FuckTheFlower.Leaks".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.FuckTheFlower.Sprays".Localized());
                }
                OutputText("Scenes.Holliscene.FuckTheFlower.OwnFemaleFluidsSympathy".Localized());
            }
            OutputText("Scenes.Holliscene.FuckTheFlower.AfterSpurtingNoLonger".Localized());
            OutputText("Scenes.Holliscene.FuckTheFlower.SighUtterlyExhaustedDrained".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            Player.ChangeFatigue(5);
            if (CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) < 1000)
            {
                CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) + 5);
            }
            CoC.Instance.Flags.Set(VanillaFlags.TimesFuckedFlower, CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + 1);
            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            ReturnToCamp(1);
        }

        // Haxe DrinkThePlantGirlsSap()   outputText 5 
        public void DrinkThePlantGirlsSap()
        {
            ClearOutput();

            Player.ApplyStats(new DynStat(VanillaStats.Libido, .5f), new DynStat(VanillaStats.Sensitivity, 1f), new DynStat(VanillaStats.Lust, 15f), new DynStat(VanillaStats.Corruption, 1f));
            Player.ChangeFatigue(-50);
            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.FeelLikePickPrance".Localized());
            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.NipplesLargerHarderOne".Localized());
            if (silly)
            {
                OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.TreeHugger".Localized());
            }
            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.ThickFillsEnergyOut".Localized());
            OutputText("Scenes.Holliscene.DrinkThePlantGirlsSap.WipeStickyMouthAdmiring".Localized());
            ReturnToCamp(1);
        }

        // Haxe RideTheWalrusP3()   outputText 11 
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
            Player.ApplyStats(new DynStat(VanillaStats.Libido, 1f), new DynStat(VanillaStats.Sensitivity, -5f));
            if (CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) < 1000)
            {
                CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, CoC.Instance.Flags.Get(VanillaFlags.FuckFlowerGrowthCounter) + 5);
            }
            CoC.Instance.Flags.Set(VanillaFlags.TimesRiddenFlower, CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) + 1);
            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            ReturnToCamp(1);
        }

        // Haxe TorchP3Tree()   outputText 9 
        public void TorchP3Tree()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.TorchP3Tree.GoneEnoughCannotContinue".Localized());
            if (Player.HasStatusEffect(VanillaStatusEffects.KnowsWhitefire))
            {
                OutputText("Scenes.Holliscene.TorchP3Tree.ClosingFocusGatherEnergies".Localized());
            }
            else
            {
                if (Player.HasPerk(VanillaPerks.FireLord))
                {
                    OutputText("Scenes.Holliscene.TorchP3Tree.SuckingBreathJaguarGift".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.TorchP3Tree.SuckingBreathFieryGift".Localized());
                }
            }
            OutputText("Scenes.Holliscene.TorchP3Tree.GoesTorchFadeSooner".Localized());
            if (Player.HasStatusEffect(VanillaStatusEffects.KnowsBlackfire) || Player.HasStatusEffect(VanillaStatusEffects.KnowsWhitefire))
            {
                OutputText("Scenes.Holliscene.TorchP3Tree.MagicalWill".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.TorchP3Tree.Breath".Localized());
            }
            OutputText("Scenes.Holliscene.TorchP3Tree.SentencePeriod".Localized());
            OutputText("Scenes.Holliscene.TorchP3Tree.ThisStaysSuitablyTorch".Localized());
            Player.ChangeFatigue(100);
            Player.UpgradeBeautifulSword();
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerKilled, 1);
            ReturnToCamp(2);
        }

        // Haxe TreePhaseFourGo()   outputText 13 
        public void TreePhaseFourGo()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.TreePhaseFourGo.UnfamiliarFemaleCallsName".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.WhereYoooouuuCroonsBend".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.VerdantSuccubusCrossingArms".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) > 0)
            {
                OutputText("Scenes.Holliscene.TreePhaseFourGo.EnjoyFeedingSeenExpression".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.TreePhaseFourGo.EnjoyWatchingGrowFed".Localized());
            }
            OutputText("Scenes.Holliscene.TreePhaseFourGo.SayEnd".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.ShakeCorruptGoddessSister".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.ExplainsLiftingFuckHarder".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.SnapOddlyColoredGaze".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.CoosBelowPlayMe".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.OffersSateLustsTogether".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.DemonicWomanDryadLegends".Localized());
            OutputText("Scenes.Holliscene.TreePhaseFourGo.HollicyntheaAddedFollowersBold".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerLevel, 4);
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerGrowthCounter, 0);
            Player.ApplyStats(new DynStat(VanillaStats.Lust, 15f));
            TreeMenu(false);
        }

        // Haxe JojoTransformAndRollOut()   outputText 18 
        public void JojoTransformAndRollOut()
        {
            ClearOutput();

            HolliSprite();
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.ComeHerePlease".Localized());
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.JojoCallingBorderPolitely".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) == 0)
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.GesturesStrangePortalSite".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.GesturesHummingVerdantGrown".Localized());
            }
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.QuietlyInterruptNewsMarae".Localized());
            if (Player.Corruption < 25)
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.NotWillingAdmitChanged".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.ThoughLeaveNeedlessDetails".Localized());
            }
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.MonkWidenSurpriseSumrise".Localized());
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.BlurtsUnbelievableSayStart".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) == 0)
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.ToThinkConsiderEnemy".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.EvenKnowingFornicateName".Localized());
            }
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.SayEndAgain".Localized());
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.LooksArgumentResignedShake".Localized());
            if (Player.Level < 20)
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.NonethelessPositionConfront".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.JojoTransformAndRollOut.MortalResourcesDestroyEscape".Localized());
            }
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.SoSayingPressesTalisman".Localized());
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.MakesPoliteForestAssistance".Localized());
            OutputText("Scenes.Holliscene.JojoTransformAndRollOut.JojoNoCompanionBold".Localized());
            Player.RemoveStatusEffect(VanillaStatusEffects.PureCampJojo);
            Player.CreateKeyItem("Jojo's Talisman", 0, 0, 0, 0);
            Go<Camp>();
        }

        // Haxe AmilyHatesTreeFucking()   outputText 3 
        public void AmilyHatesTreeFucking()
        {
            ClearOutput();

            HolliSprite();
            OutputText("Scenes.Holliscene.AmilyHatesTreeFucking.MouseyCohabitantIrritation".Localized());
            OutputText("Scenes.Holliscene.AmilyHatesTreeFucking.SawWhatTree".Localized());
            OutputText("Scenes.Holliscene.AmilyHatesTreeFucking.NoInterruptChanceAnswer".Localized());
            ClearButtons();
            AddButton(0, "Stay Quiet", StayQuietWhileAmilyBitchesAboutFuckingArborday);
            AddButton(1, "Slap Her", SlapAmilysWhoreFace);
            if (Player.Intelligence >= 70 && Player.IsPureEnough(30))
            {
                AddButton(2, "Make Excuse", MakeUpSomething).Hint("You're smart and you're visibly not corrupted. You're pretty sure you can convince her that Holli has an actual purpose.");
            }
        }

        // Haxe MakeUpSomething()   outputText 5 
        public void MakeUpSomething()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.MakeUpSomething.LetFinishOutburstPanting".Localized());
            OutputText("Scenes.Holliscene.MakeUpSomething.DrawBreathExplanationOff".Localized());
            OutputText("Scenes.Holliscene.MakeUpSomething.UsingSmartsMakeStory".Localized());
            OutputText("Scenes.Holliscene.MakeUpSomething.EyesNarrowLowersJudging".Localized());
            OutputText("Scenes.Holliscene.MakeUpSomething.BreatheReliefTruthGives".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.AmilyTreeMadeupbullshit, 1);
            Go<Camp>();
        }

        // Haxe StayQuietWhileAmilyBitchesAboutFuckingArborday()   outputText 3 
        public void StayQuietWhileAmilyBitchesAboutFuckingArborday()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.StayQuietWhileAmilyBitchesAboutFuckingArborday.BeratesSomeWhileEyeballs".Localized());
            OutputText("Scenes.Holliscene.StayQuietWhileAmilyBitchesAboutFuckingArborday.NotStayingAnymoreClean".Localized());
            OutputText("Scenes.Holliscene.StayQuietWhileAmilyBitchesAboutFuckingArborday.CurtlyTurnsDepartsLashing".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.AmilyFollower, 0);
            CoC.Instance.Flags.Set(VanillaFlags.AmilyTreeFlipout, 1);
            CoC.Instance.Flags.Set(VanillaFlags.AmilyCampCorruptionFreaked, 0);
            CoC.Instance.Flags.Set(VanillaFlags.AmilyVillageEncountersDisabled, 0);
            if (Player.Pregnancy.Type == VanillaPregnancy.Amily)
            {
                Player.KnockUp(VanillaPregnancy.Mouse, Player.Pregnancy.Incubation, 100, 1);
            }
            Go<Camp>();
        }

        // Haxe SlapAmilysWhoreFace()   outputText 8 
        public void SlapAmilysWhoreFace()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.SlapAmilyAcrossThe".Localized());
            if (AmilyFurry())
            {
                OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.Nose".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.Face".Localized());
            }
            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.DeclareNoneBusiness".Localized());
            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.LooksWoundedUnhappySorry".Localized());
            if (AmilyFurry())
            {
                OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.MouseHyphen".Localized());
            }
            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.GirlSaysEyesGoodbye".Localized());
            OutputText("Scenes.Holliscene.SlapAmilysWhoreFace.WithoutAnotherTurnsLeaves".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.AmilyFollower, 0);
            CoC.Instance.Flags.Set(VanillaFlags.AmilyCorruptFlipout, 1);
            CoC.Instance.Flags.Set(VanillaFlags.AmilyVillageEncountersDisabled, 0);
            CoC.Instance.Flags.Set(VanillaFlags.AmilyVillageAccessible, 1);
            Go<Camp>();
        }

        // Haxe HolliSexMenu()   outputText 0 
        public void HolliSexMenu()
        {

            ClearButtons();
            if (CoC.Instance.Flags.Get(VanillaFlags.HolliSubmissive) == 0)
            {
                // 原版 HolliScene.hx:981-982
                SexButton(AddButton(0, "Fuck Holli", FuckHolliInZeFlowerPuss), Gender.Male);
                SexButton(AddButton(1, "Ride Tentacles", Level4RideHollisTentacruels), Gender.Female);
            }
            else
            {
                // 原版 HolliScene.hx:984-986
                SexButton(AddButton(0, "Fuck Holli", HolliGetsDickDommed), Gender.Male);
                SexButton(AddButton(1, "TentacleBone", FullOnTentacleTasticGangBangForHolli), Gender.Male)
                    .DisableIf(Player.CountCocksOfType("TENTACLE") < 10, "You don't have enough tentacle dicks for this.");
                SexButton(AddButton(2, "Ride Holli", VaginalDomHollisTentacruels), Gender.Female);
            }
            AddNextButton("Drink Sap", HaveAMapleSyrupSnack);
            SetExitButton("Back", () => TreeMenu());
        }

        // Haxe FuckHolliInZeFlowerPuss()   outputText 48 
        public void FuckHolliInZeFlowerPuss()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            var x = Player.BiggestCockIndex();
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.KnowingSmileArmor".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.WellComeOver".Localized());
            if (Player.Tallness < 84)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.Little".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.Big".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ArborealSuggestsHuskySugar".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ConfidentlyAdvancePhallusSyrup".Localized());
            if (Player.CockTotal() > 1)
            {
                if (Player.CockTotal() == 2)
                {
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.OtherPenisSplays".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.RestOfPenisesSplay".Localized());
                }
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ToSmearingFurrowsSmooth".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.HandReachesBelowTo".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.CradleGentlyMassagingSack".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GentlyCaressYour".Localized());
                if (Player.Cocks.Exists(c => c.SheathOn))
                {
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SheathWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.LoinsWord".Localized());
                }
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.EncouraginglyUpwardPresenting".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SighingHappilyImmobileBeauty".Localized());
            if (Player.CockArea(x) < 100)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.LeansKissesGlossyPuckers".Localized(
                    Player.CockHead(x)
                ));
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.KissesDownToweringShaft".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GrabEdgesTitsSmushing".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.HungryWhispersGiveTaste".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.BallsWord".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GroinWord".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SlappingWetlyMelonsThrobbing".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GeyserSpunkRocketsOut".Localized());
            if (Player.CockArea(x) >= 100)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.AirHangsMomentSplattering".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MouthNoisilySwallowInject".Localized());
            }
            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.KeepBlastingRopesPainted".Localized());
            }
            if (Player.CumQ() >= 1500)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.LakeAroundRiverDownhill".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MischievousGoopyFacialTongue".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.BreakEmbraceHappyAdmire".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.PaleHandsFondleSoothing".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.RollStrokeWhoahDozed".Localized());
            if (Player.Tallness < 60)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.TitsWord".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.ArmsWord".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.InsteadSlammingSlotsWarm".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MassagingBundleCocksFervor".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MassagingToolUncommonFervor".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GruntIrritationVanishesSex".Localized());
            if (Player.BiggestBreastSize() >= 2)
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.YourOwnWord".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.YourChestPlaceholder".Localized());
            }
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.GiveBitingLowerLip".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.DeprivedChoiceRules".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.MoansClingingTightlyPouts".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.SnarlIrritationPillowTweak".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.BlackRingedRollQuivering".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.OnceStumbleAwayDrained".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.OffersSarcasticallyRaisingBoob".Localized());
            OutputText("Scenes.Holliscene.FuckHolliInZeFlowerPuss.PoutsDressedEnticeCannot".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.TimesFuckedFlower, CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + 1);
            FertilizeHolli();
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -1f), new DynStat(VanillaStats.Corruption, 1f));
            ReturnToCamp(1);
        }

        // Haxe HaveAMapleSyrupSnack()   outputText 10 
        public void HaveAMapleSyrupSnack()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.ApproachHungryLickingChops".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.HolliSubmissive) == 0)
            {
                OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.KnowinglyPickDrinkDeeply".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.EagerlyAsksPickEngorged".Localized());
            }
            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.GrowlingFaintlySnakeCurl".Localized());
            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.GaspsNotSoHard".Localized());
            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.LookingConcernedVisageBite".Localized());
            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.NippleWeakSprayMilk".Localized());
            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.QuicklyFondlingReturnDrinking".Localized());
            Player.RefillHunger(10);
            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.TremblingLubricantsFlushed".Localized());
            OutputText("Scenes.Holliscene.HaveAMapleSyrupSnack.PerhapsLaterWalkWanted".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Libido, .5f), new DynStat(VanillaStats.Sensitivity, 1f), new DynStat(VanillaStats.Lust, 15f), new DynStat(VanillaStats.Corruption, 1f));
            Player.ChangeFatigue(-60);
            ReturnToCamp(1);
        }

        // Haxe Level4RideHollisTentacruels()   outputText 19 
        public void Level4RideHollisTentacruels()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.SmirkKnowinglySuggestWhat".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.PromisesLoveLieBack".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.FoldArmsImpatientlyWaiting".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.GlareFacefulFlowerCunt".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.HandCondescendinglyForeplay".Localized());
            if (silly)
            {
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.ProceedsJackSparrowMinutes".Localized());
            }
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.HotFluidDripsLarger".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.BurstFireworkStrengthIntensity".Localized());
            Player.CuntChange(60, true, true, false);
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.VibrantPurplesPinksBrighter".Localized());
            if (Player.LactationQ() == 0)
            {
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.DropletsDrippingLactating".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.SteadyEruptingLactatingHard".Localized());
            }
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.CheersFuckingBothEnds".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.PitcherRootsMuddyPuddle".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.SpittleSoakedTorrentJuices".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.MilkSprayingRedoublesPitcher".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.AnHourLaterBold".Localized());
            OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.WakeUtterlySoakedJizz".Localized());
            if (!Player.IsBiped())
            {
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.WalkingBowleggedWeekLegs".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.Level4RideHollisTentacruels.WalkingBowleggedExclamation".Localized());
            }
            Player.Orgasm("Vaginal");
            Player.ApplyStats(new DynStat(VanillaStats.Libido, .5f), new DynStat(VanillaStats.Sensitivity, 2f), new DynStat(VanillaStats.Corruption, 1f));
            FertilizeHolli(false);
            Player.SlimeFeed();
            CoC.Instance.Flags.Set(VanillaFlags.TimesRiddenFlower, CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) + 1);
            ReturnToCamp(1);
        }

        // Haxe EatHolliFruit()   outputText 10 
        public void EatHolliFruit()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.EatHolliFruit.ReachUpBranchesPluck".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.HolliFruit) == 1)
            {
                OutputText("Scenes.Holliscene.EatHolliFruit.TheFruit".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.EatHolliFruit.OneOfTheFruits".Localized());
            }
            OutputText("Scenes.Holliscene.EatHolliFruit.PeriodSpace".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.HolliFruitExplained) == 0)
            {
                OutputText("Scenes.Holliscene.EatHolliFruit.SmilesAsksFruitsUnion".Localized());
                OutputText("Scenes.Holliscene.EatHolliFruit.NearlyPurplishPearSurprise".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.HolliFruitExplained, CoC.Instance.Flags.Get(VanillaFlags.HolliFruitExplained) + 1);
            }
            OutputText("Scenes.Holliscene.EatHolliFruit.BitingJuicesExplodeChin".Localized());
            Player.RefillHunger(25);
            if (Random.Shared.Next((int)(2)) == 0 && Player.CockTotal() > Player.CountCocksOfType("TENTACLE"))
            {
                var choices = new List<int>();
                var temp = 0;
                while (temp < Player.CockTotal())
                {
                    if (Player.Cocks[temp].Id != new Identifier("coc", "tentacle"))
                    {
                        choices.Add(temp);
                    }
                    temp += 1;
                }
                temp = choices[Random.Shared.Next((int)(choices.Count))];
                OutputText("Scenes.Holliscene.EatHolliFruit.PenisItchesGrowsLonger".Localized(
                    MonoCoC.Utils.Utils.Num2TextOrdinal(temp + 1),
                    Player.CockDescript(temp)
                ));
                if (Player.CountCocksOfType("TENTACLE") > 0)
                {
                    OutputText("Scenes.Holliscene.EatHolliFruit.Nother".Localized());
                }
                OutputText("Scenes.Holliscene.EatHolliFruit.TentacleCockBold".Localized());
                Player.Cocks[temp].Id = new Identifier("coc", "tentacle");
                Player.Cocks[temp].KnotMultiplier = 1.3f;
                Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, 3f), new DynStat(VanillaStats.Lust, 10f));
            }
            // 原版 HolliScene.hx:1193 `flags[KFLAGS.HOLLI_FRUIT]--`
            CoC.Instance.Flags.Set(VanillaFlags.HolliFruit, CoC.Instance.Flags.Get(VanillaFlags.HolliFruit) - 1);
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            if ((100 * Player.Toughness / Player.GetMaxStats("tou")) < 50)
            {
                Player.ApplyStats(new DynStat(VanillaStats.Toughness, 1f));
            }
            if ((100 * Player.Strength / Player.GetMaxStats("str")) < 50)
            {
                Player.ApplyStats(new DynStat(VanillaStats.Strength, 1f));
            }
            Save.SetInt("fruitsEaten", Save.GetInt("fruitsEaten", 0) + 1);
            ReturnToCamp(1);
        }

        // Haxe AskHolliToWatch()   outputText 4 
        public void AskHolliToWatch()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.HolliDefenseOn) == 0)
            {
                OutputText("Scenes.Holliscene.AskHolliToWatch.AskTentaclesGuardCamp".Localized());
                OutputText("Scenes.Holliscene.AskHolliToWatch.LooksOnlyDebaseYourself".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.ThreatenedHolli) > 0)
                {
                    OutputText("Scenes.Holliscene.AskHolliToWatch.CouldAlwaysCuntBold".Localized());
                }
                ClearButtons();
                AddButton(0, "Beg", BegHolli4Watches);
                if (CoC.Instance.Flags.Get(VanillaFlags.ThreatenedHolli) == 0)
                {
                    AddButton(1, "Threaten", ThreatenHolli);
                }
                else
                {
                    AddButton(1, "Slap Her", SlapDatHo);
                }
                AddButton(2, "Assert Self", DomUpSomeHolli);
                AddButton(14, "Back", () => TreeMenu());
            }
            else
            {
                OutputText("Scenes.Holliscene.AskHolliToWatch.TellStopWatchingLaughs".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.HolliDefenseOn, 0);
                TreeMenu();
            }
        }

        // Haxe BegHolli4Watches()   outputText 6 
        public void BegHolli4Watches()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.BegHolli4Watches.SwallowProstrateGrudgingly".Localized());
            OutputText("Scenes.Holliscene.BegHolli4Watches.DevilishGripsSquirtsSap".Localized());
            OutputText("Scenes.Holliscene.BegHolli4Watches.GrimacingFaceDownRoots".Localized());
            OutputText("Scenes.Holliscene.BegHolli4Watches.NahNotEnoughDetail".Localized());
            OutputText("Scenes.Holliscene.BegHolli4Watches.UtterlyHumiliatingLongerLick".Localized());
            OutputText("Scenes.Holliscene.BegHolli4Watches.HahGuessWatchCamp".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.HolliDefenseOn, 1);
            TreeMenu();
        }

        // Haxe DomUpSomeHolli()   outputText 6 
        public void DomUpSomeHolli()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            OutputText("Scenes.Holliscene.DomUpSomeHolli.SwaggerPutHandBark".Localized());
            OutputText("Scenes.Holliscene.DomUpSomeHolli.SlappingOtherGoldBlack".Localized());
            var domPowah = Player.Level;
            domPowah += Player.Tallness / 12;
            if (Player.Horns.Id.Path != "none")
            {
                domPowah += 3;
            }
            if (Player.Corruption > 66)
            {
                domPowah += 2;
            }
            if (domPowah < 20)
            {
                OutputText("Scenes.Holliscene.DomUpSomeHolli.RollingSinksBarkLips".Localized());
                ReturnToCamp(1);
                return ;
            }
            OutputText("Scenes.Holliscene.DomUpSomeHolli.SlowlyPitchBlackSclera".Localized());
            OutputText("Scenes.Holliscene.DomUpSomeHolli.IntimidatedArborSighsMarae".Localized());
            OutputText("Scenes.Holliscene.DomUpSomeHolli.WentConfidenceBrokenObediently".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.HolliSubmissive, 1);
            TreeMenu(false);
        }

        // Haxe AskBrokenHolliToGuard()   outputText 4 
        public void AskBrokenHolliToGuard()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.BowsMentionGuardingKeep".Localized());
            if (CampGuards.IsGuard("Holli"))
            {
                OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.KeepingWatch".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.SleepingInstead".Localized());
            }
            OutputText("Scenes.Holliscene.AskBrokenHolliToGuard.SayEndPeriod".Localized());
            ClearButtons();
            if (CampGuards.IsGuard("Holli"))
            {
                AddButton(1, "Don't Guard", ToggleBrokenHolliGuard);
            }
            else
            {
                AddButton(0, "Guard", ToggleBrokenHolliGuard);
            }
            AddButton(14, "Back", () => TreeMenu());
        }

        // Haxe ToggleBrokenHolliGuard()   outputText 4 
        public void ToggleBrokenHolliGuard()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.HolliDefenseOn) == 0)
            {
                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.QuipsRubbingPalmsFun".Localized());
                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.SupposeBetterLesserWhelps".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.HolliDefenseOn, 1);
            }
            else
            {
                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.SaysWinkSmitePlay".Localized());
                OutputText("Scenes.Holliscene.ToggleBrokenHolliGuard.WarningShutsThinkBefore".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.HolliDefenseOn, 0);
            }
            ClearButtons();
            AddButton(0, "Next", () => TreeMenu());
        }

        // Haxe HolliGetsDickDommed()   outputText 32 
        public void HolliGetsDickDommed()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliGetsDickDommed.TossingExposeCocksFixate".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.LeansForwardEntrancedThrough".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.HangingReachesStopsShort".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.SoLongGoodLicks".Localized());
            if (Player.BiggestCockArea() > 80)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.JawStretchingObscenely".Localized());
            }
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.SentencePeriod".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.CongratulateSuccessPushBack".Localized());
            if (Player.BiggestCockArea() <= 80)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SwallowedToThe".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Holliscene.HolliGetsDickDommed.BallsPlaceholder".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.HolliGetsDickDommed.SheathPlaceholder".Localized());
                }
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SentencePeriodAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SwallowedDeepDistendedNeck".Localized());
            }
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.GurglingEyelashesTwitching".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.BubblingPleasureBuilds".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SackTautReadyBurst".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.BodyStrungTunedGuitar".Localized());
            }
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.PullsBackSnarlSh".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.GrabHungrySuckerNeck".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.SteppingRigidSlickFlower".Localized());
            if (Player.BiggestCockArea() > 80)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.BendsSlightlyForcedInside".Localized());
            }
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.FlopsChestTwitchingWeakly".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.KissPlantyForcefullyDominance".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.AbruptnessOrgasmStartles".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.ExcessUddersViridianMounds".Localized());
                if (Player.CumQ() >= 500)
                {
                    OutputText("Scenes.Holliscene.HolliGetsDickDommed.ConstantSpraysTurnWhite".Localized());
                }
            }
            if (Player.CumQ() > 1000)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.TrickleSpermWashesEntrance".Localized());
            }
            if (Player.CumQ() > 2000)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.MomentLaterTrickleRiver".Localized());
            }
            if (Player.CumQ() > 3000)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SwearTrunkStuffedSpunk".Localized());
            }
            if (Player.CumQ() > 5000)
            {
                OutputText("Scenes.Holliscene.HolliGetsDickDommed.SpoogeSpoutPressureRoof".Localized());
            }
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.TentaclesCumCanopy".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.RemovingGapingFlowerpotAdmire".Localized());
            OutputText("Scenes.Holliscene.HolliGetsDickDommed.CrookFingerPointNectar".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -2f), new DynStat(VanillaStats.Corruption, 1f));
            CoC.Instance.Flags.Set(VanillaFlags.TimesFuckedFlower, CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + 1);
            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            FertilizeHolli();
            ReturnToCamp(1);
        }

        // Haxe FullOnTentacleTasticGangBangForHolli()   outputText 22 
        public void FullOnTentacleTasticGangBangForHolli()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.RemoveExposeWrithingTendrils".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ExercisingVegetativeExtend".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ReachesUpPioneeringPhallus".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.LookingWaySmolderingMassage".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.StartedFlushThreeWay".Localized());
            if (silly)
            {
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.InflatableFlailingTubeCocks".Localized());
            }
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.BelowArborealAffectionPetals".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.HandsGlistenSixPenises".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.InundatedCockDominatedDick".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.UnspoolSixthUnoccupiedBundle".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ExperienceIntenseMoreManhoods".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AwashOrgyRadiatesGroin".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.TremendousExcitementFrame".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.DrawTightGroinDischarging".Localized());
            }
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.SemenDistendsSpheroidBulges".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.SpoogeDistortionsPushedOut".Localized());
            if (Player.CumQ() > 500)
            {
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AndTheNext".Localized());
            }
            if (Player.CumQ() > 1400)
            {
                OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AndTheNextEllipsis".Localized());
            }
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.AnotherLoadCrownsHai".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ClimaxesAgainPassesVagina".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.ComeLittleLaterRetracted".Localized());
            OutputText("Scenes.Holliscene.FullOnTentacleTasticGangBangForHolli.StretchingShouldersDump".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Libido, .4f), new DynStat(VanillaStats.Sensitivity, -3f), new DynStat(VanillaStats.Corruption, 1f));
            CoC.Instance.Flags.Set(VanillaFlags.TimesFuckedFlower, CoC.Instance.Flags.Get(VanillaFlags.TimesFuckedFlower) + 1);
            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            FertilizeHolli();
            FertilizeHolli();
            ReturnToCamp(1);
        }

        // Haxe VaginalDomHollisTentacruels()   outputText 34 
        public void VaginalDomHollisTentacruels()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
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
            {
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.PayAttentionNipplesTugging".Localized());
            }
            else
            {
                if (Player.LactationQ() >= 50)
                {
                    OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SighAtThe".Localized());
                    if (Player.LactationQ() < 150)
                    {
                        OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.Squirts".Localized());
                    }
                    else
                    {
                        if (Player.LactationQ() < 800)
                        {
                            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.Sprays".Localized());
                        }
                        else
                        {
                            if (Player.LactationQ() < 2000)
                            {
                                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.Outpouring".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.ThickFountains".Localized());
                            }
                        }
                    }
                    OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MilkEscapeEverySluggish".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SinkNipplesFuckingTempo".Localized());
                }
            }
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.LoudGroanEscapesHips".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.PaleGreenBlushesMask".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MovingQuickerPoundsPistoning".Localized());
            if (Player.GetClitLength() >= 3.5f)
            {
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.MomentBeforeWrappingJerking".Localized());
            }
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SentencePeriod".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.CriesHnnng".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.TelltaleFlaringDeepInside".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.WhisperSlutCumStroke".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.TentaclesRigidAbsolutelyHard".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.WarmWashesWombCervix".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.StillSpurtingWithdrawDragging".Localized());
            if (Player.Wetness() < 4)
            {
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.OntoTheGround".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.IntoLeavingsSquirtingOrgasm".Localized());
            }
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.SecondTimeMakeMess".Localized());
            OutputText("Scenes.Holliscene.VaginalDomHollisTentacruels.PeckCheekGoodGirl".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.TimesRiddenFlower, CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenFlower) + 1);
            CoC.Instance.Flags.Set(VanillaFlags.HolliFuckedToday, 1);
            FertilizeHolli(false);
            Player.SlimeFeed();
            Player.Orgasm("Vaginal");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -2f), new DynStat(VanillaStats.Corruption, 1f));
            ReturnToCamp(1);
        }

        // Haxe ThreatenHolli()   outputText 8 
        public void ThreatenHolli()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.ThreatenHolli.SnarlThreatenBarkedBitch".Localized());
            OutputText("Scenes.Holliscene.ThreatenHolli.ThreatenFireUprootedSapling".Localized());
            OutputText("Scenes.Holliscene.ThreatenHolli.CompellingPoint".Localized());
            if (Player.Level < 30)
            {
                OutputText("Scenes.Holliscene.ThreatenHolli.MightyMaraeBeyondPower".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.ThreatenHolli.MaraeBeyondNotPressure".Localized());
            }
            OutputText("Scenes.Holliscene.ThreatenHolli.RestStayFirmlyT".Localized());
            if (Camp.CampGuarded())
            {
                OutputText("Scenes.Holliscene.ThreatenHolli.YeahWhateverWaitingCovered".Localized());
            }
            OutputText("Scenes.Holliscene.ThreatenHolli.MaybeRefusesGuardBold".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.ThreatenedHolli, 1);
            ReturnToCamp(1);
        }

        // Haxe SlapDatHo()   outputText 2 
        public void SlapDatHo()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.SlapDatHo.SlapTreeThighsRipple".Localized());
            OutputText("Scenes.Holliscene.SlapDatHo.WhatMeatComePlay".Localized());
            ClearButtons();
            AddButton(0, "CutHerDown", CutHerDown);
            if (Player.HasKeyItem("Jojo's Talisman"))
            {
                AddButton(1, "Call Jojo", CallDatJojo);
            }
            AddButton(4, "Ignore", () => TreeMenu(true));
        }

        // Haxe CutHerDown()   outputText 4 
        public void CutHerDown()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.CutHerDown.DemonSeriousBrokenToy".Localized());
            if (Player.WeaponName == "large axe")
            {
                OutputText("Scenes.Holliscene.CutHerDown.ThatsTrickTreeTree".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.CutHerDown.SoArmoredTaintedInsects".Localized());
                OutputText("Scenes.Holliscene.CutHerDown.VerdantIncensedWithdrawsSlid".Localized());
            }
            CoC.Instance.Flags.Set(VanillaFlags.FoughtHolli, 1);
            // 原版 HolliScene.hx:1549 `jojoAssisting = false`：Jojo 助战未移植，C# 恒 false。
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "holli")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
        }

        // Haxe CallDatJojo()   outputText 3 
        public void CallDatJojo()
        {
            ClearOutput();

            Player.RemoveKeyItem("Jojo's Talisman");
            OutputText("Scenes.Holliscene.CallDatJojo.TakeOutTalismanSummon".Localized());
            OutputText("Scenes.Holliscene.CallDatJojo.ApologiesSmellOnlyWay".Localized());
            OutputText("Scenes.Holliscene.CallDatJojo.DemonRoarsBreakingConversation".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.FoughtHolli, 1);
            // 原版 HolliScene.hx:1566 `jojoAssisting = true`（Jojo 助战）：Holli.hx:81/115 的纵火分支
            // 依赖 CombatReactionContext.Burned，而 C# 没有该派发点，所以这一状态恒 false、不赋值。
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "holli")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
        }

        // Haxe DefeatHolli()   outputText 7 
        public void DefeatHolli()
        {
            ClearOutput();

            int temp = 0;
            Player.UpgradeBeautifulSword();
            CoC.Instance.Flags.Set(VanillaFlags.FuckFlowerKilled, 1);
            // 原版 HolliScene.hx:1576 `if (!jojoAssisting)`：Jojo 助战未移植（该状态恒 false），只保留这一支。
            {
                OutputText("Scenes.Holliscene.DefeatHolli.WearySuccubusGodlingTree".Localized());
                if (Player.WeaponName != "large axe")
                {
                    OutputText("Scenes.Holliscene.DefeatHolli.FetchAxeAnd".Localized());
                }
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
                // statScreenRefresh()C# 
            }
            // 未移植：原版 HolliScene.hx:1646 起的 Jojo 助战败北支（3 小时 + vapulaSlave() 判断 + 招募 Jojo 两个按钮）
            // 依赖 jojoAssisting / CombatReactionContext.Burned，C# 都没有，整段未接；
            // RecruitJojoToCamp / DontRecruitJojoToCamp 因此暂时没有调用方（保留方法与文本）。
            OutputText("Scenes.Holliscene.DefeatHolli.KeyGainedHollisAshes".Localized());
            Player.CreateKeyItem("Holli's Ashes", 0, 0, 0, 0);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe RecruitJojoToCamp()   outputText 3 
        public void RecruitJojoToCamp()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.RecruitJojoToCamp.NodsEagerlyInviteCamp".Localized());
            OutputText("Scenes.Holliscene.RecruitJojoToCamp.BowingNeatlyTakesLeave".Localized());
            Player.CreateStatusEffect(VanillaStatusEffects.PureCampJojo, 0, 0, 0, 0);
            OutputText("Scenes.Holliscene.RecruitJojoToCamp.KeyGainedHollisAshes".Localized());
            Player.CreateKeyItem("Holli's Ashes", 0, 0, 0, 0);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe DontRecruitJojoToCamp()   outputText 2 
        public void DontRecruitJojoToCamp()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.DontRecruitJojoToCamp.HoldTongueAllowingForest".Localized());
            OutputText("Scenes.Holliscene.DontRecruitJojoToCamp.KeyGainedHollisAshes".Localized());
            Player.CreateKeyItem("Holli's Ashes", 0, 0, 0, 0);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe EnjoyYourBadEndBIYAAAATCH()   outputText 74 
        public void EnjoyYourBadEndBIYAAAATCH()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ScreechingTriumphAloftShake".Localized());
            // 未移植：原版 HolliScene.hx:1646 `if (jojoAssisting)` 的 Jojo 纵火开场句
            // （JojoFirewoodStrikeSwats）依赖 Jojo 助战，该状态在 C# 恒 false。
            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.CoosDangerouslyEnoughBefore".Localized());
            if (Player.Gender == Gender.Male || (Random.Shared.Next((int)(2)) == 0 && Player.HasCock()))
            {
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.StripsHaphazardlyShredding".Localized());
                if (Player.Cocks[Player.BiggestCockIndex()].Thickness > 6)
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.StretchingJawWideWince".Localized());
                }
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.EngulfsTonguePlaysSli".Localized());
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.OnceAttentionDisarmingly".Localized());
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PleasureFoggedGoggleImpending".Localized());
                if (Player.BiggestCockLength() > 24)
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.NeckExtendsGrotesquelyMigrate".Localized(
                        NumberUtils.Num2Text((float)(Player.BiggestCockLength()))
                    ));
                }
                else
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SinksBaseQuickThrust".Localized());
                }
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SensationPenetratedSucked".Localized());
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TooClimaxComesFlood".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BallsDrawCloseUnload".Localized());
                }
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.EyesNothingComesOrg".Localized());
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SmilesSwellingIncreasesSeed".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MigratingDirectionEntangling".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ExploringDepthsCurlingSkin".Localized());
                }
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.StomachSproutsCrawlingCrotch".Localized());
                OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SmilesFoolishMeatMareth".Localized());
                Player.Orgasm("Dick");
                Player.ApplyStats(new DynStat(VanillaStats.Libido, 5f), new DynStat(VanillaStats.Sensitivity, 20f));
                ClearButtons();
                AddButton(0, "Next", HolliPlaysWithPenisesBadEnd);
            }
            else
            {
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PullsImpatientlyFastenings".Localized());
                    if (Player.IsTaur())
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AndTurningYouAround".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BuriesVaginaFlexibleTongue".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PealSurpriseInsertionDally".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.HumsHappilyReachesDeep".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.Something".Localized());
                    if (!Player.IsGoo() && !Player.HasTailInsteadOfLegs())
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SlipsBetweenThighsAnd".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.NosesPhallicTendrilUnnoticed".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.GlobPrecumEachCock".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SentencePeriod".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SmilesThrustingTentacle".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.CornerEyeFinishedAlready".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TentacleClimaxesEjaculating".Localized());
                    if (Player.BiggestBreastSize() >= 1)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SLetter".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ChestWord".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.LooksBreathingHeavilyComes".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MusingsInterruptedTentacle".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.OnePrecumLengthSpasm".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TracesPetalsPrettyBlossom".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.LipsKissPulsesSlippery".Localized());
                    if (Player.BiggestBreastSize() > 1)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.WrappingPermanentlyJutting".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TracingSlenderCrisscrossingIvy".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ContinuingBackNeckThicken".Localized());
                    if (Player.BiggestBreastSize() > 1)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BreastsPressedTogetherVines".Localized());
                    }
                    if (!Player.HasTailInsteadOfLegs() && !Player.IsGoo())
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ThighsSpreadWideOpen".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.NectarDroolingFlowerClit".Localized());
                    if (Player.HasCock())
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AndEachCock".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BobbingPermanentSemiArousal".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.YoungSaplingEncasingGesture".Localized());
                    Player.Orgasm("Vaginal");
                    Player.ApplyStats(new DynStat(VanillaStats.Libido, 5f), new DynStat(VanillaStats.Sensitivity, 20f));
                    ClearButtons();
                    AddButton(0, "Next", GirlsGetANiceManToBadEnd);
                }
                else
                {
                    Player.Orgasm("Anal");
                    Player.ApplyStats(new DynStat(VanillaStats.Libido, 5f), new DynStat(VanillaStats.Sensitivity, 20f));
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PullsArmorBreakingFastenings".Localized());
                    if (Player.Anal.Wetness > 0)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BetrayingDepravityCrackOpens".Localized());
                    }
                    else
                    {
                        if (Player.AnalCapacity() < 30)
                        {
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.DifficultyForcesPuckerAs".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.DespiteClenchingImpediment".Localized());
                        }
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.EyesConcentrationShudderSquirm".Localized());
                    Player.ButtChange(20, true, true, false);
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AgonizingExpansionContorts".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MurmursPullingFreeExtracts".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MoansQuiversPushesGrowth".Localized());
                    if (Player.Anal.Wetness > 0)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.TryCantSeeBack".Localized());
                    }
                    else
                    {
                        if (Player.AnalCapacity() < 30)
                        {
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.JawCollarboneFloridHead".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PressesBackBrushingNipples".Localized());
                        }
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SapOozingMemberPassage".Localized());
                    if (Player.Anal.Wetness > 0)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.MinglingLubricationEasingTempo".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PantsShriekExcitedPu".Localized());
                    if (Player.Anal.Wetness > 0 || Player.AnalCapacity() > 30)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.SpillingDepositedFluidsLegs".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.OncePuckerSqueezesTrapping".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.FreshlyShaftShrivelWithering".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.FluidsMinglePinprickSensation".Localized());
                    // 原版 Creature.hx:3422 isFurryOrScaley() = isFurry() || hasScales()
            if (Player.HasFur() || Player.HasScales())
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.UnderSkinfurscales".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.PanicIntensifiesTendrils".Localized());
                    if (Player.BiggestBreastSize() < 1)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.FoldedAtopChest".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.CrookedHandsRestingChest".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.ContinueFaceReachingCanopy".Localized());
                    if (Player.BiggestBreastSize() >= 1)
                    {
                        OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.BreastsExposedFromSides".Localized());
                    }
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.LegsConcealedButtcheeksTrunk".Localized());
                    OutputText("Scenes.Holliscene.EnjoyYourBadEndBIYAAAATCH.AdmiresLovelyAmbianceSticks".Localized());
                    Player.ApplyStats(new DynStat(VanillaStats.Lust, (float)(Player.MaxLust)));
                    ClearButtons();
                    AddButton(0, "Next", HolliAndGenderlessSittingInATree);
                }
            }
        }

        // Haxe HolliPlaysWithPenisesBadEnd()   outputText 27 
        public void HolliPlaysWithPenisesBadEnd()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.YoungHairedStepsPortal".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.ApproachesOuterBarksHumanoid".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.HelloChampionHollicyntheaDryad".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.RegardsCalmlyHelpMoreso".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.GesturesSecondSilentFigure".Localized());
            if (Player.BiggestBreastSize() < 1)
            {
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.MasculineShapeItBears".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.FeminineShapeNonethelessBears".Localized());
            }
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.StandingErectIronBlushes".Localized(
                Player.MultiCockDescript()
            ));
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.TeasesNotCompanionAnswer".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.GraciouslyAcceptsContinues".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.NewestChampionCountersI".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.GesturesAgainCompanionCocks".Localized());
            if (Player.SmallestCockArea() > 36)
            {
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SilentSmallestShuddersShrink".Localized());
            }
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.WaversAppearsDecisionQuiet".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.MurmursNothingAshamedExtends".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.BeginsGrinCutsPushing".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.CoosPurityHumbleDryads".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.YelpsShaftSlidesPrecum".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.TimeReachesThirdTense".Localized());
            if (Player.Cocks.Count > 1)
            {
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SprayingStomachWasteSeed".Localized());
            }
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.FillingChampionPussyCascades".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.DryadGivesPushesTits".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SLetter".Localized());
            }
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.DroolingAftershocksOntoBreasts".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.PromptsSnapOpenFi".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.DazedStrugglesWobblingPounding".Localized());
            OutputText("Scenes.Holliscene.HolliPlaysWithPenisesBadEnd.SpitsDistantRetreatingFool".Localized());
            GameOver();
        }

        // Haxe GirlsGetANiceManToBadEnd()   outputText 77 
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
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestCuppedVinesOut".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestCrisscrossedFineVines".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LegsElevatedSpreadDisplay".Localized());
            if (Player.Balls > 0 && Player.BallSize > 10 && Player.Gender == Gender.Hermaphrodite)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksAballsSwingingBelow".Localized());
            }
            else
            {
                if (Player.Balls > 0 && Player.BallSize <= 10 && Player.Gender == Gender.Hermaphrodite)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksBallsOverVagina".Localized());
                }
                else
                {
                    if (Player.Gender == Gender.Hermaphrodite)
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksAndVagina".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WetDroolingBegsFilled".Localized());
                    }
                }
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.EyesCoveredBitingLicking".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ApproachesReachesCanopyBranch".Localized());
            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestPlaceholder".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.VinesWord".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.PluckingFruitThrows".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LeafyNearestPlantHungry".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HandlesReverentlyDontThank".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.RetortsSmileToldBefore".Localized());
            if (Player.BallSize > 10 && Player.Balls > 0)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LiftingTesticlesRevealPussy".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SpreadingExposingEagerHole".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.MoansNearlyRippingGratefully".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SaysQuietlyBehindFree".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.BuddingThrustingEagerly".Localized());
            if (Player.BiggestBreastSize() >= 1)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ReachesHandfulsBreastThumb".Localized());
                if (Player.Gender == Gender.Female)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WomanWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HermaphroditeWord".Localized());
                }
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SMouthCloseBliss".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.LeansKissesHermaphroditeChest".Localized());
            }
            if (Player.HasCock())
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.NervousGlanceHandCock".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HandSlidesHoleLub".Localized());
                if (Player.GetClitLength() < 6)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SmearsLittleButtonUnderside".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CoatsSwollenJerksValiantly".Localized());
                }
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.AsksUppingFeelGood".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.AsIfReply".Localized());
            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WomanWordAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HermaphroditeWordAgain".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ArchesVaginaClampsTool".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.TextSemicolonSpace".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.BallsCloserBody".Localized());
                }
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HerTremblingDick".Localized());
                if (Player.CockTotal() == 1)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Throbs".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SThrob".Localized());
                }
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.AsSheNearsClimax".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SentencePeriod".Localized());
            if (Player.HasCock())
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.YouthsWidenSqueezesShut".Localized());
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SLetter".Localized());
                }
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.TowardHerFace".Localized());
                if (Player.BiggestCockArea() < 20)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SemenSpurtsDribblesPooling".Localized());
                    if (Player.CumQ() >= 500)
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SoFinishesPartnerTrunk".Localized());
                    }
                }
                else
                {
                    if (Player.BiggestCockArea() < 80)
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.MouthCockheadGulpsDrooling".Localized());
                        if (Player.CumQ() >= 500)
                        {
                            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WavesDontSwellingBelly".Localized());
                        }
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.DwarfedMaleMassiveSize".Localized());
                        if (Player.CumQ() >= 1000)
                        {
                            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.OverproductivePushesCanopySoo".Localized());
                        }
                    }
                }
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ALetter".Localized());
                if (Player.Wetness() < 3)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Gush".Localized());
                }
                else
                {
                    if (Player.Wetness() < 4)
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Rush".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.Torrent".Localized());
                    }
                }
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.FemaleOrgasmMouthWide".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.PushedLimitDisplayGroans".Localized());
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CockWord".Localized());
            }
            else
            {
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.CocksWord".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ChestWord".Localized());
                }
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.GoesSlackJawedSquirts".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SaysComingSensesWithdrawing".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.RepliesPassingOverSilent".Localized());
            if (Player.Gender == Gender.Female)
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WomanWordThird".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.HermaphroditeWordThird".Localized());
            }
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SStomachEnjoyedBes".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.KneelingTakesCockQui".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SaysPullsWhat".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.SeedMothersPowerCreatures".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.ExclaimsGatheringClothesFruit".Localized());
            OutputText("Scenes.Holliscene.GirlsGetANiceManToBadEnd.WatchesSightGoodT".Localized());
            GameOver();
        }

        // Haxe HolliAndGenderlessSittingInATree()   outputText 20 
        public void HolliAndGenderlessSittingInATree()
        {
            ClearOutput();

            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.TallTravelerColorsDissolve".Localized());
            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.DrawsNotOnlyIdea".Localized());
            // 原版 Creature.hx:3422 isFurryOrScaley() = isFurry() || hasScales()
            if (!(Player.HasFur() || Player.HasScales()))
            {
                OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.ColoredSkintoneHyphen".Localized());
            }
            else
            {
                OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.CoveredInSkinfurscales".Localized());
            }
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
            {
                OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.QuiverArousalShakesScales".Localized());
            }
            else
            {
                if (Player.HasFur())
                {
                    OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.FurPricksUpArousal".Localized());
                }
                else
                {
                    OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.BlushLustColorsSkinfurscales".Localized());
                }
            }
            OutputText("Scenes.Holliscene.HolliAndGenderlessSittingInATree.Ellipsis".Localized());
            GameOver();
        }

        // Haxe AmilyComesBack()   outputText 2 
        public void AmilyComesBack()
        {
            ClearOutput();

            HolliSprite();
            OutputText("Scenes.Holliscene.AmilyComesBack.ArrivesBelongingsShoulderKnew".Localized());
            OutputText("Scenes.Holliscene.AmilyComesBack.AmilyAddedLoversBold".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.AmilyFollower, 1);
            CoC.Instance.Flags.Set(VanillaFlags.AmilyVillageEncountersDisabled, 1);
            Go<Camp>();
        }

        // Haxe HolliAskGlades()   outputText 21 
        public void HolliAskGlades()
        {
            ClearOutput();

            if (!Save.GetBool("askedGlades"))
            {
                Save.SetBool("askedGlades", true);
                OutputText("Scenes.Holliscene.HolliAskGlades.NotingResemblanceCorruptDryads".Localized());
                OutputText("Scenes.Holliscene.HolliAskGlades.SmilesFloralScentArt".Localized());
                OutputText("Scenes.Holliscene.HolliAskGlades.MoreThemMaybe".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.HolliFuckedToday) == 0)
                {
                    OutputText("Scenes.Holliscene.HolliAskGlades.GlancesNervouslySinkingBeckons".Localized());
                    OutputText("Scenes.Holliscene.HolliAskGlades.DoesntSeemBudgeLying".Localized());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.HolliSubmissive) != 0)
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
                RegisterTag("growth", Save.GetBool("growingGlades"));
                OutputText("Scenes.Holliscene.HolliAskGlades.AskGrowingLessMore".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.HolliSubmissive) != 0)
                {
                    OutputText("Scenes.Holliscene.HolliAskGlades.HolliDoesntPleasedDefy".Localized());
                    OutputText("Scenes.Holliscene.HolliAskGlades.GroundTremblesShrinksTree".Localized());
                }
                else
                {
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
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.HolliSubmissive) != 0 || CoC.Instance.Flags.Get(VanillaFlags.HolliFuckedToday) != 0)
                {
                    Save.SetBool("growingGlades", !Save.GetBool("growingGlades"));
                }
            }
            TreeMenu();
        }

    }
}
