using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    // 原版 MarbleScene.hx 的「营地互动 / 孩子线 / 物品交换 / 分手线」段。
    // 对应 Haxe 行：1848-3004（postAddictionCampMornings 至 giveMarbleLactaid）。
    //
    // 分工说明（与 MarbleScene.Addiction.cs 的块 B 协调）：
    //   standardSex / marbleSex2Continued / marbleMilkSex / marbleGenderlessNoFuck / applyMarblesMilk
    //   已由 Block B 的 MarbleScene.Addiction.cs 实现（原版 MarbleScene.hx:1881-2232），
    //   本文件不再重复定义，避免 partial 同名冲突。
    //
    // 原版 spriteSelect(SpriteDb.s_marble)：引擎贴图选择，C# 无对应运行时（同主文件 MarbleFightWin 的处理），
    // 跳过并保留原版行号；MarbleScene.Core.cs 的 MarbleSprite 是立绘选择，不是同一函数。
    //
    // 未移植依赖（留 seam 并标注原版行号）：
    //   marbleAppearance（MarbleScene.hx:4539）、
    //   marbleCampSexNew（Haxe:3005-4330，属后续 block）、marblePreggoChance（Haxe:4331）。
    // MarblePurification 场景已移植（Contents/Scenes/Vanilla/Npcs/MarblePurification.cs），
    // 原版 MarbleScene.hx:2297-2299、2312、2969-2973、3009-3015 的按钮已接真实方法。
    // 物品 handler（原版 MarbleScene.hx:4393-4510）由 MarbleScene.Appearance.cs 实现，GiveItem 已直接调用。
    public partial class MarbleScene
    {
        // 原版 AbstractFarmContent / BaseContent 的 convenience 属性。
        private static bool Silly => MonoCoC.OptionSystem.OptionStore.Instance.Get("silly", 0) > 0;

        // TelAdre.isAllowedInto（原版 TelAdre.hx:106-108）：player.statusEffectv1(StatusEffects.TelAdre) >= 1
        // 与 KellyScene.cs:94 使用同一判定。
        private static bool TelAdreAllowed() => Player.StatusEffectValue(VanillaStatusEffects.TelAdre, 0) >= 1;

        private static bool FollowerRathazul() =>
            SceneInstance<RathazulScene>.Get().FollowerRathazul();

        // ── 原版 postAddictionCampMornings（MarbleScene.hx:1848-1877）──
        public void PostAddictionCampMornings(bool extra = true)
        {
            // 原版 spriteSelect(SpriteDb.s_marble)（Haxe:1849）：引擎贴图选择，无运行时，跳过。
            OutputText("Scenes.Marblescene.PostAddictionCampMornings.WakesToFreshMilk".Localized());

            if (Player.Corruption < 40)
            {
                if (Player.Corruption < 30)
                    Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));

                Player.ApplyStats(new DynStat(VanillaStats.Corruption, 1f));
            }

            // 原版 player.lib100 < 40：C# 的 Libido 上限就是 100，lib100 等价于 Libido。
            if (Player.Libido < 40)
                Player.ApplyStats(new DynStat(VanillaStats.Libido, 0.1f));

            Player.RefillHunger(20f);
            Player.SlimeFeed();

            if (!extra)
                return;

            // 原版 hasItem(M__MILK, 5) 会算上仓库；C# 这里用背包count（仓库版本见 HasItemAnywhere，但无数量接口）。
            if (Player.ItemCount(VanillaConsumables.MinotaurMilk) < 5)
            {
                OutputText("Scenes.Marblescene.PostAddictionCampMornings.HandedMilkBottle".Localized());

                if (Player.HasPerk(VanillaPerks.MarbleResistant))
                    OutputText("Scenes.Marblescene.PostAddictionCampMornings.AssuresSafeBottle".Localized());

                InventoryScene.TakeItem(ItemRegistry.Create(VanillaConsumables.MinotaurMilk),
                    () => Camp.ReturnToCamp(1));
            }
        }

        // 原版 marbleAddictionSex（MarbleScene.hx:2130-2185）
        public void MarbleAddictionSex(bool newpage = true)
        {
            // 原版 spriteSelect(SpriteDb.s_marble)（Haxe:2131）：引擎贴图选择，无运行时，跳过。
            if (newpage)
                ClearOutput();

            if (Player.Gender == Gender.Genderless)
            {
                MarbleGenderlessNoFuck();

                if (newpage)
                    Camp.ReturnToCamp(1);

                return;
            }

            OutputText("Scenes.Marblescene.MarbleAddictionSex.RockTogetherUndressing".Localized());

            if (Player.Tallness < 66)
                OutputText("Scenes.Marblescene.MarbleAddictionSex.MarbleBendsAndKisses".Localized());
            else if (Player.Tallness < 78)
                OutputText("Scenes.Marblescene.MarbleAddictionSex.MarbleLowersHeadKiss".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleAddictionSex.MarbleGrabsYourHead".Localized());

            if (Player.CockTotal() > 0)
            {
                OutputText("Scenes.Marblescene.MarbleAddictionSex.NeedAlmostPalpable".Localized());

                if (Player.Tallness >= 48 && Player.Tallness <= 60)
                    OutputText("Scenes.Marblescene.MarbleAddictionSex.HeadClampedBreasts".Localized());

                OutputText("Scenes.Marblescene.MarbleAddictionSex.WarmFoldsAroundCock".Localized());
            }
            else if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Marblescene.MarbleAddictionSex.NeedPalpableMarbleGiggles".Localized()
                    Player.VaginaDescript(0));
                Player.CuntChange(8, true);
                OutputText("Scenes.Marblescene.MarbleAddictionSex.TwoShudderAndBask".Localized());

                if (Player.Tallness >= 48 && Player.Tallness <= 60)
                    OutputText("Scenes.Marblescene.MarbleAddictionSex.HeadBetweenBreasts".Localized());

                OutputText("Scenes.Marblescene.MarbleAddictionSex.ThinkMarbleWonderful".Localized());
                OutputText("Scenes.Marblescene.MarbleAddictionSex.ParagraphBreak".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleAddictionSex.LongAfterLovemaking".Localized());

            if (Player.Tallness < 66)
                OutputText("Scenes.Marblescene.MarbleAddictionSex.SetsYouDown".Localized());

            OutputText("Scenes.Marblescene.MarbleAddictionSex.PushesArmsLengthAsks".Localized());
            Player.Orgasm("Generic");

            // 原版 MarbleScene.hx:2182-2184：hasCock 时 marblePreggoChance()（原版 MarbleScene.hx:4331，
            // 属后续 block，依赖 Marble 自己的 PregnancyStore，C# 未移植），这里不掷怀孕（原版 MarbleScene.hx:4331）。
            if (newpage)
                Camp.ReturnToCamp(1);
        }

        // 原版 marbleCampFuckFUCKFUCKFUCK（MarbleScene.hx:2189-2192）
        public void MarbleCampFuckFUCKFUCKFUCK()
        {
            // 原版 marbleSprite(true) + marbleCampSexNew()（原版 MarbleScene.hx:2189-2192 → Haxe:3005-4330，属后续 block，C# 未移植）。
            // 保留可达 seam，避免在 block C 里硬塞另一块的 300+ 行内容。
            MarbleSprite(true);
            ClearOutput();
            OutputText("Scenes.Camp.NotYet".Localized());
            ClearButtons();
            AddButton(14, "Back", InteractWithMarbleAtCamp);
        }

        // 原版 interactWithMarbleAtCamp（MarbleScene.hx:2262-2302）
        public void InteractWithMarbleAtCamp()
        {
            ClearOutput();
            MarbleSprite();

            // 原版 akky.locationDesc("Marble")（Haxe:2266）：C# 无 Akky 宠物系统，跳过
            // （同 HolliScene.cs:264 的缺口；原版 MarbleScene.hx:2266）。
            Action? playtime = null;

            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) != 2)
                OutputText("Scenes.Marblescene.InteractWithMarbleAtCamp.HowInteractWithMarble".Localized());
            else
                OutputText("Scenes.Marblescene.InteractWithMarbleAtCamp.AlchemistPurityQuestion".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 0)
                playtime = MarbleKidsPlaytime;

            ClearButtons();

            // 原版 marbleAppearance（MarbleScene.hx:4539 函数 / :2284 按钮 + hint）
            AddButton(0, "Appearance", MarbleAppearance, "Take a closer look at the bovine girl.");
            AddButton(1, "Talk", TalkWithMarbleAtCamp);
            AddButton(2, "Sex", MarbleSexMenu);
            AddButton(5, "Children", playtime);

            AddRowButton(1, "Give Item", GiveItem)?.DisableIf(!CanGiveItem(), "You have nothing to give her.");

            AddRowButton(1, "Get Present", MarbleGathered)?.DisableIf(
                !Player.HasStatusEffect(VanillaStatusEffects.MarbleHasItem),
                "Marble might find something for you again at a later time.");

            AddRowButton(1, "Get Milk", GotMilk)?.DisableIf(
                Player.ItemCount(VanillaConsumables.MinotaurMilk) >= 5,
                "Marble can't give you any more milk right now.");

            if (CoC.Instance.Flags.Get(VanillaFlags.SleepWith) != "Marble")
                AddButton(9, "Sleep With", MarbleSleepToggle);
            else
                AddButton(9, "Sleep Alone", MarbleSleepToggle);

            AddButton(10, "Break Up", BreakUpWithMarble);

            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) == 3 &&
                CoC.Instance.Flags.Get(VanillaFlags.MarbleRathazulCounter1) == 0)
            {
                // 原版 MarbleScene.hx:2297-2299 → marblePurification.murbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether()。
                AddRowButton(2, "Go",
                    () => SceneInstance<MarblePurification>.Get()
                        .MurbleShouldGoRunAlongAndGetHerCuntySisterSoTheyCanBeCuntsTogether());
            }

            AddButton(14, "Back", () => FollowersScene.Open(VanillaFollowerRoles.Lover));
        }

        // 原版 marbleSexMenu（MarbleScene.hx:2304-2315）
        public void MarbleSexMenu()
        {
            ClearButtons();

            AddNextButton("Release", MarbleSexExpanded)?.DisableIf(Player.Lust < 33,
                "You're not horny enough to have sex.");

            if ((CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) < 5 &&
                 CoC.Instance.Flags.Get(VanillaFlags.MarbleLust) >= 20) ||
                (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) >= 5 &&
                 CoC.Instance.Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) >= 4))
            {
                OutputText("Scenes.Marblescene.MarbleSexMenu.MarbleFidgetsUncomfortably".Localized());

                // 原版 milkMarble（MarbleScene.hx:2309 → 函数 Haxe:4675）
                AddNextButton("Milking", MilkMarble);
            }

            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) >= 5)
            {
                // 原版 MarbleScene.hx:2312 → marblePurification.nursingFromPureMarble()
                AddNextButton("Nurse", () => SceneInstance<MarblePurification>.Get().NursingFromPureMarble());
            }

            SetExitButton("Back", InteractWithMarbleAtCamp);
        }

        // 原版 marbleSleepToggle（MarbleScene.hx:2317-2328）
        public void MarbleSleepToggle()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.SleepWith) != "Marble")
            {
                OutputText("Scenes.Marblescene.MarbleSleepToggle.MarbleSaysLovely".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.SleepWith, "Marble");
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleSleepToggle.MarbleFoldsArmsSighs".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.SleepWith, "");
            }

            ClearButtons();
            AddButton(0, "Next", InteractWithMarbleAtCamp);
        }

        // 原版 marbleKidsPlaytime（MarbleScene.hx:2331-2460）
        public void MarbleKidsPlaytime()
        {
            RegisterTag("marbleMultiKids", CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1);
            RegisterTag("marbleSoleBoy", CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 1);

            ClearOutput();

            var choices = new List<int> { 1 };

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
            {
                choices.Add(2);
                choices.Add(3);
                choices.Add(4);
                choices.Add(5);
            }

            // 原版 Haxe:2348-2350 的 select == 6 → anemoneScene.repeatCowSitting()：
            // AnemoneScene 未移植（全仓库 0 个该类；原版 MarbleScene.hx:2348-2350），
            // 不把无法执行的分支塞进随机池。原版 Haxe:2351-2353 followerRathazul() → select 7。

            if (FollowerRathazul())
                choices.Add(7);

            choices.Add(8);

            var select = choices[Random.Shared.Next(choices.Count)];

            if (select == 1)
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1 &&
                    CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 1)
                {
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.SonClopsIntoArms".Localized()
                        Player.Mf("Daddy, daddy", "Mommy, mommy"));
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.GameEndsBovineVictory".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.DaughterClopsIntoArms".Localized()
                        Player.Mf("Daddy, daddy", "Mommy, mommy"));
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.GameEndsBovineTriumph".Localized());
                }

                Player.ChangeFatigue(10);
            }

            if (select == 2)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.KidsGreetAtNursery".Localized()
                    Player.Mf("Dad", "Mom"));
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.TaleOfExploits".Localized());

                if (Player.Corruption < 66)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.SkipsExplicitParts".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.StoryEndsFarewell".Localized());
                Player.ChangeFatigue(10);
            }

            if (select == 3)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleAsksWhitneyVisit".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.QuickTravelToWhitney".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.JoinHerWithKids".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.HourNearlyUpGoodbye".Localized());
                Player.ChangeFatigue(10);
            }

            if (select == 4)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleSitsOnLogs".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.BrokenStructureCry".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleTeachesBuilding".Localized());

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 2)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.BothWord".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.HalfOfYourChildren".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.SomeKnackForIt".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarblePatienceLessons".Localized());
            }

            if (select == 5)
            {
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleWearySmile".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.YouSmileAndSuggest".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.CanManageProtest".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.YouSilenceHerExplain".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.ThankfulForRest".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.YouCallTheBrood".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.EagerKidQuestion".Localized()
                    Player.Mf("Daddy", "Mommy"));
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.EnergeticGrabbingKids".Localized());

                if (Player.Intelligence < 30)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.GatherReturnToCamp".Localized());
                else if (Player.Intelligence < 70)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.AwareOfSpiceHerbs".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.KnowsSpicesByHeart".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.TiredKidsAfterWork".Localized()
                    Player.Mf("Daddy", "Mommy"));
                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.MarbleWaitsRested".Localized());

                if (Player.Tallness < 60)
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.LiftsYouBigHug".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsPlaytime.StrongWarmEmbrace".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsPlaytime.LoveYouThanks".Localized());
                Player.ChangeFatigue(15);

                InventoryScene.TakeItem(ItemRegistry.Create(VanillaConsumables.W_FRUIT),
                    () => Camp.ReturnToCamp(1));
                return;
            }

            if (select == 7)
                KidsHaveRatPlaytime();

            if (select == 8)
                KidsPlayingFarmers();

            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePlayedWithKidsToday) > 0)
            {
                Player.ApplyStats(new DynStat(VanillaStats.Lust, -10f));
            }
            else
            {
                Player.ApplyStats(new DynStat(VanillaStats.Libido, -1f),
                    new DynStat(VanillaStats.Lust, -10f),
                    new DynStat(VanillaStats.Corruption, -1f));
            }

            Camp.ReturnToCamp(1);
        }

        // 原版 kidsPlayingFarmers（MarbleScene.hx:2462-2476）
        public void KidsPlayingFarmers()
        {
            OutputText("Scenes.Marblescene.KidsPlayingFarmers.KidRunsUpEagerly".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.KidsPlayingFarmers.KidsUndecidedRole".Localized());
            else
                OutputText("Scenes.Marblescene.KidsPlayingFarmers.WithYourLittleKid".Localized());

            OutputText("Scenes.Marblescene.KidsPlayingFarmers.FarmOwner".Localized());

            // 原版 Haxe:2471-2473：game.telAdre.isAllowedInto() && Utils.trueOnceInN(3)。
            if (TelAdreAllowed() && Random.Shared.Next(3) == 0)
                MarbleKidsAtTelAdre();

            OutputText("Scenes.Marblescene.KidsPlayingFarmers.MarbleJoinsRoleplay".Localized());
            OutputText("Scenes.Marblescene.KidsPlayingFarmers.ContinuePlayingUntilTired".Localized());
        }

        // 原版 marbleKidsAtTelAdre（MarbleScene.hx:2478-2533）
        public void MarbleKidsAtTelAdre()
        {
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.AppleDiscussionInterrupted".Localized());
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.AssureShortErrand".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.StayWithMommyRefusal".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.KidsPlayAloneRefused".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.HesitantThenConvinced".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
            {
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.KidsAccustomOutside".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 1)
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.SonUsedToOutside".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.DaughterOutsideWorld".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.AvoidPerverseTownParts".Localized());
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.QuickTravelTelAdre".Localized());
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.CuriousKidAsksAll".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
            {
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.TooExcitedStayPut".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 0)
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.TirelessGirlRunning".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.TirelessBoyRunning".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.StaysWithinYourRange".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.IngredientsThenShops".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
            {
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.KidsEvenLivelier".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 0)
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.GirlBeamsAtShop".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.BoyJoyAtShop".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.GrowingExcitement".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.EnthusiasmWithEveryVisit".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.EdrynNumberOfKids) > 0)
            {
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.ClearsMisunderstanding".Localized());
                MarbleKidsAtTelAdreEnding();
            }

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.RecognizeEdrynSurprised".Localized());
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdre.EdrynAsksWhoKids".Localized());

            if (Player.StatusEffectValue(VanillaStatusEffects.Edryn, 0) > 0)
                MarbleKidsMeetEdrynFuckbuddy();
            else
                MarbleKidsMeetEdryn();
        }

        // 原版 marbleKidsMeetEdrynFuckbuddy（MarbleScene.hx:2534-2565）
        public void MarbleKidsMeetEdrynFuckbuddy()
        {
            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.HerEyesGoWide".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.ChildrenStareDisbelief".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 0)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.DaughterHardToBelieve".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.SonDisbeliefNervous".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.MommyMarble".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 999 && Silly)
                TooManyCows();

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.BlurtedCongrats".Localized());
            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.EdrynAsksCare".Localized());
            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.RefuseOfferExplainCamp".Localized());

            if (Player.HasPerk(VanillaPerks.HistoryDEUSVULT))
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.NoInterestTown".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 39)
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.TooManyChildrenStrain".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.SentencePeriod".Localized());
            }

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.ComesCloserAdvice".Localized());
            OutputText("Scenes.Marblescene.MarbleKidsMeetEdrynFuckbuddy.GettingLateLeaves".Localized());
            MarbleKidsAtTelAdreEnding();
        }

        // 原版 marbleKidsMeetEdryn（MarbleScene.hx:2567-2669）
        public void MarbleKidsMeetEdryn()
        {
            if (Player.IsChild())
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DisbeliefEars".Localized());

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ChildrenStatement".Localized());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 0)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DaughterStatement".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.SonStatement".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.YoungFatherSurprise".Localized());

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                {
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynLooksCloser".Localized());

                    if (Player.CowScore() < 5)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DifferentSpeciesPrefix".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.YLetter".Localized());

                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.FamilialResemblance".Localized());
                }
                else
                {
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynLittleKidsLook".Localized());

                    if (Player.CowScore() < 5)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.OtherSpeciesPrefix".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.TLetter".Localized());

                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ResembleFather".Localized());
                }

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 999 && Silly)
                    TooManyCows();

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 39)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EyesWideAwe".Localized());
                else if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 9)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.SilenceAmazed".Localized());
                else if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 3)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BigFamilyRemark".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.AsksSoloCare".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BriefRundown".Localized());

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.KidsPraiseHer".Localized());
                else
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.KidsPraiseYou".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynRelievedChange".Localized());
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.FirstVisitPrefix".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.IntroduceChild".Localized());

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 999 && Silly)
                    TooManyCows();

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.CongratsFather".Localized());

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 39)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.HugeFamilyShock".Localized());
                else if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 9)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.HugeFamilySurprise".Localized());
                else if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 3)
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BusyRemark".Localized());

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.YourPrefix".Localized());

                if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                {
                    OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.KidsGreetJoy".Localized());
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBoys) == 0)
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.DaughterGreetJoy".Localized());
                    else
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.SonGreetJoy".Localized());

                    if (Player.IsTaur())
                        OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.CentaurLikeDaddy".Localized());
                }

                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.EdrynSmilesAsks".Localized());
            }

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ChildRidesEdryn".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.ChildrenRideEdryn".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 4)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.TooManyTakeTurns".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) < 9 && Player.IsTaur())
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.OthersRideYourBack".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.TelAdreSuggestion".Localized());

            if (Player.HasPerk(VanillaPerks.HistoryDEUSVULT))
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.RefuseCorruptTown".Localized());
            else if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 39)
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.RefuseStrain".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.CampIsSafe".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsMeetEdryn.BidsFarewell".Localized());
        }

        // 原版 tooManyCows（MarbleScene.hx:2671-2674）
        public void TooManyCows()
        {
            OutputText("Scenes.Marblescene.TooManyCows.GargantuanFamily".Localized());
            MarbleKidsAtTelAdreEnding();
        }

        // 原版 marbleKidsAtTelAdreEnding（MarbleScene.hx:2676-2694）
        public void MarbleKidsAtTelAdreEnding()
        {
            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.ReturnToCamp".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.CampWallGate) > 0)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.CampGate".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.TheEdgeOfThe".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.LiftedEmbracedKiss".Localized());
            else
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.RunsHugsKiss".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.KidsJoinHug".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 4)
                OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.KidsSqueezeHug".Localized());

            OutputText("Scenes.Marblescene.MarbleKidsAtTelAdreEnding.SharedTimeTogether".Localized());
        }

        // 原版 kidsHaveRatPlaytime（MarbleScene.hx:2696-2716）
        public void KidsHaveRatPlaytime()
        {
            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.LeisureWithKids".Localized());
            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.GamesReachRathazul".Localized());
            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.WithoutLookingAwayFrom".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.ChildrenMoreCuriousThey".Localized());
            else
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.ChildAsksHim".Localized());

            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.AlchemyExplained".Localized());
            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.RathazulExperience".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.KidsUnderstandNothingOf".Localized());
            else
                OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.ChildStaysCurious".Localized());

            OutputText("Scenes.Marblescene.KidsHaveRatPlaytime.LessonFinished".Localized());
        }

        // 原版 breakUpWithMarble（MarbleScene.hx:2719-2724）
        public void BreakUpWithMarble()
        {
            ClearOutput();
            OutputText("Scenes.Marblescene.BreakUpWithMarble.ConfirmBreakup".Localized());
            AddButton(0, "Yes", DefinitelyBreakUpWithWithMarble);
            AddButton(1, "No", InteractWithMarbleAtCamp);
        }

        // 原版 definitelyBreakUpWithWithMarble（MarbleScene.hx:2727-2746）
        public void DefinitelyBreakUpWithWithMarble()
        {
            ClearOutput();
            OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.ApproachForSpace".Localized());

            if (Player.HasPerk(VanillaPerks.MarblesMilk))
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.SeeYouForMilk".Localized());
            else
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.SeeYouLater".Localized());

            OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.CollectsThings".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) == 1)
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.HerChildSuffix".Localized());
            else if (CoC.Instance.Flags.Get(VanillaFlags.MarbleKids) > 1)
                OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.HerChildrenSuffix".Localized());

            OutputText("Scenes.Marblescene.DefinitelyBreakUpWithWithMarble.LeavesCamp".Localized());

            CoC.Instance.Flags.Set(VanillaFlags.BrokeUpWithMarble, 1);
            Player.RemoveStatusEffect(VanillaStatusEffects.CampMarble);
            Player.RemoveStatusEffect(VanillaStatusEffects.NoMoreMarble);
            Scene.PlayerMenu();
        }

        // 原版 talkWithMarbleAtCamp（MarbleScene.hx:2754-2886）
        public void TalkWithMarbleAtCamp()
        {
            ClearOutput();

            if (!Player.HasStatusEffect(VanillaStatusEffects.MarbleSpecials))
                Player.CreateStatusEffect(VanillaStatusEffects.MarbleSpecials, 0, 0, 0, 0);

            OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.CallOverToChat".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MetMarae) > 0 &&
                CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 1)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.VisitGoddessMarae".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 1);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.FactoryFound) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 2)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FoundDemonFactory".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 2);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.FactoryShutdown) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 3)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FactoryHorrors".Localized());

                if (Player.HasPerk(VanillaPerks.OmnibusGift))
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LessonGiftDemons".Localized());
                else
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ProbablyForBest".Localized());

                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FactoryShutdownTale".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 3);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.MaraeQuestComplete) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.FactoryShutdown) == 1 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 4)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.HappyHelpedMarae".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 4);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.MaraeQuestComplete) >= 1 &&
                     CoC.Instance.Flags.Get(VanillaFlags.FactoryShutdown) == 2 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 4)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.MarbleShakenStory".Localized());

                if (Player.HasPerk(VanillaPerks.MaraesGiftFertility))
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethicitePregnancyConcern".Localized());
                else if (Player.HasPerk(VanillaPerks.MaraesGiftStud))
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethiciteBreedQuestion".Localized());

                if (Player.Lust100 < 50)
                    Player.ApplyStats(new DynStat(VanillaStats.Lust, 35f));

                if (!Player.HasPerk(VanillaPerks.MaraesGiftFertility) &&
                    !Player.HasPerk(VanillaPerks.MaraesGiftStud))
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.RanAwayTale".Localized());

                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 4);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.DiscoveredDungeon2Zetaz) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 5)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ForestCaveTale".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 5);
            }
            else if (TelAdreAllowed() && CoC.Instance.Flags.Get(VanillaFlags.MarbleTeladreStory) == 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.TelAdreTale".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleTeladreStory, 1);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.DefeatedZetaz) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 6)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.OldFoeCave".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 6);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.D3Discovered) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 7)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethiceStronghold".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 7);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.LethiceDefeated) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MarbleCamptalkLevel) < 8)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.LethiceDefeated".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.MarbleCamptalkLevel, 8);
            }
            else
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.NoNewStories".Localized());
            }

            OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ParagraphBreak".Localized());
            OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.NextStepAdvice".Localized());

            if (CoC.Instance.Flags.Get(VanillaFlags.MetMarae) <= 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FarmLakeAdvice".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.FactoryFound) <= 0)
            {
                if (Player.Level < 3)
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.NotReadyForMountains".Localized());
                else
                    OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ReadyForFactory".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.FactoryFound) >= 1 &&
                     CoC.Instance.Flags.Get(VanillaFlags.FactoryShutdown) <= 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.FinishFactory".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.FactoryShutdown) >= 1 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MaraeQuestComplete) <= 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.MetMaraeCorrupted) <= 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.CorruptedMaraeKilled) <= 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ReturnToMarae".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.DiscoveredDungeon2Zetaz) == 0 || !TelAdreAllowed())
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ExploreWorld".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.DiscoveredDungeon2Zetaz) == 0 && TelAdreAllowed())
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.DeepWoodsSecrets".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.DiscoveredDungeon2Zetaz) > 0 && !TelAdreAllowed())
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.PatrolUniforms".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.DiscoveredDungeon2Zetaz) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.DefeatedZetaz) == 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.ExploreCave".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.DefeatedZetaz) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.D3Discovered) == 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.HighMountains".Localized());
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.D3Discovered) > 0 &&
                     CoC.Instance.Flags.Get(VanillaFlags.LethiceDefeated) == 0)
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.StopLethice".Localized());
            }
            else
            {
                OutputText("Scenes.Marblescene.TalkWithMarbleAtCamp.MainStoryDone".Localized());
            }

            InteractWithMarbleAtCamp();
        }

        // 原版 marbleSexExpanded（MarbleScene.hx:2889-2892）
        public void MarbleSexExpanded()
        {
            MarbleCampFuckFUCKFUCKFUCK();
        }

        // 原版 gotMilk（MarbleScene.hx:2897-2901）
        public void GotMilk()
        {
            ClearOutput();
            OutputText("Scenes.Marblescene.GotMilk.AskMilkBottle".Localized());
            InventoryScene.TakeItem(ItemRegistry.Create(VanillaConsumables.MinotaurMilk),
                InteractWithMarbleAtCamp);
        }

        // 原版 marbleGathered（MarbleScene.hx:2903-2920）
        public void MarbleGathered()
        {
            ClearOutput();
            OutputText("Scenes.Marblescene.MarbleGathered.AskSupplies".Localized());

            var itemStage = Player.StatusEffectValue(VanillaStatusEffects.MarbleHasItem, 0);

            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) != 1 && itemStage <= 4)
            {
                InventoryScene.TakeItem(ItemRegistry.Create(VanillaConsumables.VitalityTincture),
                    InteractWithMarbleAtCamp);
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) != 1 && itemStage <= 7)
            {
                // 原版 armors.LEATHRA（原版 MarbleScene.hx:2914）：C# 装备 id 是 coc:leather_armor。
                InventoryScene.TakeItem(ItemRegistry.Create(Identifier.Parse("coc:leather_armor")),
                    InteractWithMarbleAtCamp);
            }
            else
            {
                InventoryScene.TakeItem(ItemRegistry.Create(VanillaConsumables.LaBova),
                    InteractWithMarbleAtCamp);
            }

            Player.RemoveStatusEffect(VanillaStatusEffects.MarbleHasItem);
        }

        // 原版 canGiveItem（MarbleScene.hx:2922-2956）
        public bool CanGiveItem()
        {
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.OviElixir), 1))
                return true;

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) == 0)
            {
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedIncubiDraft), 1))
                    return true;
            }
            else
            {
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.EggPink), 1))
                    return true;

                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.EggLargePink), 1))
                    return true;
            }

            // 原版 consumables.PROBOVA（原版 MarbleScene.hx:2938、ConsumableLib.hx:182，LaBova ENHANCED）：
            // C# VanillaConsumables 只有 LaBova（labova）与 PurifiedLaBova（purified_labova），
            // 没有 ProBova 变体（数据缺失），暂不可交付。

            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedLaBova), 1) &&
                CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) == 1)
                return true;

            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) >= 5)
            {
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 1))
                    return true;

                if (Player.HasItem(ItemRegistry.Create(VanillaItems.Reducto), 1))
                    return true;

                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.Lactaid), 1))
                    return true;
            }

            return false;
        }

        // 原版 giveItem（MarbleScene.hx:2958-2978）
        public void GiveItem()
        {
            ClearOutput();
            OutputText("Scenes.Marblescene.GiveItem.ChooseItemGift".Localized());
            ClearButtons();

            // 原版 giveItem（MarbleScene.hx:2958-2978）：块 E 的 handler 已落地（Haxe:4393-4510）。
            AddButton(1, ItemRegistry.Create(VanillaConsumables.OviElixir).Name, MarbleOvulatesLikeMadDawg)
                ?.DisableIf(!Player.HasItem(VanillaConsumables.OviElixir), "You don't have that item.");

            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleDickType) == 0)
            {
                AddButton(2, ItemRegistry.Create(VanillaConsumables.PurifiedIncubiDraft).Name, MarbleDigsDraftsYo)
                    ?.DisableIf(!Player.HasItem(VanillaConsumables.PurifiedIncubiDraft), "You don't have that item.");
            }
            else
            {
                AddButton(3, ItemRegistry.Create(VanillaConsumables.EggPink).Name, MarblepinkEgg)
                    ?.DisableIf(!Player.HasItem(VanillaConsumables.EggPink), "You don't have that item.");
                AddButton(4, ItemRegistry.Create(VanillaConsumables.EggLargePink).Name, MarbleLPinkEgg)
                    ?.DisableIf(!Player.HasItem(VanillaConsumables.EggLargePink), "You don't have that item.");
            }

            // 原版 slot 5 用 consumables.PROBOVA；C# 物品表里对应的是 LaBova（见 GiveMarbleTheProBovas4Sho 的 consumeItem）。
            if (CoC.Instance.Flags.Get(VanillaFlags.MarbleBovaLevel) < 2)
                AddButton(5, ItemRegistry.Create(VanillaConsumables.LaBova).Name, GiveMurbleProBova)
                    ?.DisableIf(!Player.HasItem(VanillaConsumables.LaBova), "You don't have that item.");

            // slot 6/7/8 与纯化后的 slot 0：原版 MarbleScene.hx:2969-2973 / 3009-3015 → marblePurification 的 4 个 handler。
            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) == 1)
                AddButton(6, ItemRegistry.Create(VanillaConsumables.PurifiedLaBova).Name,
                    SceneInstance<MarblePurification>.Get().GiveMarblePureQuestLabova)
                    ?.DisableIf(!Player.HasItem(VanillaConsumables.PurifiedLaBova), "You don't have that item.");

            if (CoC.Instance.Flags.Get(VanillaFlags.MarblePurificationStage) >= 5)
            {
                AddButton(7, ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk).Name,
                    SceneInstance<MarblePurification>.Get().PurifiedSuccubusMilkForPureMarbles)
                    ?.DisableIf(!Player.HasItem(VanillaConsumables.PurifiedSuccubiMilk), "You don't have that item.");
                AddButton(8, ItemRegistry.Create(VanillaItems.Reducto).Name,
                    SceneInstance<MarblePurification>.Get().PureMurbleUsesReducto)
                    ?.DisableIf(!Player.HasItem(VanillaItems.Reducto), "You don't have that item.");
                AddButton(0, ItemRegistry.Create(VanillaConsumables.Lactaid).Name,
                    SceneInstance<MarblePurification>.Get().LactaidForPureMurble)
                    ?.DisableIf(!Player.HasItem(VanillaConsumables.Lactaid), "You don't have that item.");
            }
            else if (Player.HasPerk(VanillaPerks.MarblesMilk))
            {
                AddButton(0, ItemRegistry.Create(VanillaConsumables.Lactaid).Name, GiveMarbleLactaid);
            }
            else
            {
                // 原版 MarbleScene.hx:3019-3025：没有 MarblesMilk perk 时 Lactaid 按钮只是置灰（无提示）。
                AddButtonDisabled(0, ItemRegistry.Create(VanillaConsumables.Lactaid).Name);
            }

            AddButton(14, "Back", InteractWithMarbleAtCamp);
        }

        // 原版 giveMarbleLactaid（MarbleScene.hx:2981-2998）
        public void GiveMarbleLactaid()
        {
            ClearOutput();
            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.Lactaid));

            OutputText("Scenes.Marblescene.GiveMarbleLactaid.ShowLactaidBottle".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.GiveMarbleLactaid.LiftsToChest".Localized());
            else if (Player.Tallness > 72)
                OutputText("Scenes.Marblescene.GiveMarbleLactaid.DrinkNowPrompt".Localized());
            else
                OutputText("Scenes.Marblescene.GiveMarbleLactaid.QuickLungeDrink".Localized());

            OutputText("Scenes.Marblescene.GiveMarbleLactaid.TastierMilk".Localized());
            Player.ApplyStats(new DynStat(VanillaStats.Strength, 1f),
                new DynStat(VanillaStats.Toughness, 1f),
                new DynStat(VanillaStats.Intelligence, 1f));
            Camp.ReturnToCamp(1);
        }

        // 原版 BaseContent 里用到的那批衣物名（原版 MarbleScene.hx:1897、2033 等）。
        private static bool IsFetishGear() =>
            Player.ArmorName == "bondage patient clothes" ||
            Player.ArmorName == "crotch-revealing clothes" ||
            Player.ArmorName == "cute servant's clothes" ||
            Player.ArmorName == "maid's clothes" ||
            Player.ArmorName == "servant's clothes";
    }
}
