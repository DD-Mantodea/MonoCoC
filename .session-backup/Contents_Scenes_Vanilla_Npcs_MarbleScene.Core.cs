using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.World;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    // 原版 classes/scenes/npcs/MarbleScene.hx 的 A 块（1-897 行）：
    // 核心状态 / 时间推进 / 初遇 / 警示 / 战斗后续 / 喂食与强奸收尾。
    //
    // 跨块调用（块 C/D 的函数）用 partial 方法声明：
    //   ApplyMarblesMilk → 块 B（MarbleScene.Addiction.cs）；MarbleAddictionSex / PostAddictionCampMornings / PostAddictionFarmMornings → 块 C
    //   RapeDAHMARBLEZ / MarbleBadEndFollowup / MarblePoopsBaybees / MarbleRapeCorruptFantasy            → 块 D
    // 对应块落地时在各自的 partial 文件里写同签名的 partial 实现即可。
    public partial class MarbleScene
    {
        // 原版 MarbleScene.hx:58-60 的实例状态
        public bool HighLibidoTryst;

        private int _checkedMarbleMilk;

        private static Random Rng => Random.Shared;

        // ── 各 block 共用的 Marble 状态访问（原版 MarbleScene.hx:240-279 / :499-501）──
        // 块 B/C/D 的 partial 文件都直接复用这些成员（见 MarbleScene.Addiction.cs 头注释）。

        private static void ReturnToCamp(int hours) => Camp.ReturnToCamp(hours);

        // 原版 CoC.hx:807-810 addictionEnabled = fetishSettings.addiction
        private static bool AddictionEnabled => MonoCoC.OptionSystem.OptionStore.Instance.Get("addiction", 1) > 0;

        // 原版 MarbleScene.hx:499-501 marbleFollower
        public bool MarbleFollower() => Player.HasStatusEffect(VanillaStatusEffects.CampMarble);

        private int MarbleAffection
        {
            get => (int)Player.StatusEffectValue(VanillaStatusEffects.Marble, 0);
            set
            {
                EnsureMarbleStatus();
                Player.StatusEffectByType(VanillaStatusEffects.Marble)!.Values[0] = Math.Clamp(value, 0, 100);
            }
        }

        private int MarbleAddiction
        {
            get => (int)Player.StatusEffectValue(VanillaStatusEffects.Marble, 1);
            set
            {
                EnsureMarbleStatus();
                Player.StatusEffectByType(VanillaStatusEffects.Marble)!.Values[1] = Math.Clamp(value, 0, 100);
            }
        }

        private int KnowAddiction
        {
            get => (int)Player.StatusEffectValue(VanillaStatusEffects.Marble, 2);
            set
            {
                EnsureMarbleStatus();
                Player.StatusEffectByType(VanillaStatusEffects.Marble)!.Values[2] = value;
            }
        }

        // 块 D 的 MarbleScene.Night.cs 沿用原版 Haxe 的小写字段名，这里提供只读别名。
        private int knowAddiction => KnowAddiction;

        // 原版 MarbleScene.hx:44 的 pregnancy 旗标面：type / incubation 的取值见 PregnancyStore.hx:113-124。
        // event / eventTriggered 在 MarbleScene.Night.cs 里基于这两个值实现。
        private static int MarblePregnancyRawType => Flags.Get(VanillaFlags.MarblePregnancyType);

        private static int MarblePregnancyType => MarblePregnancyRawType & 0xFFFF;

        private static int MarblePregnancyIncubation => Flags.Get(VanillaFlags.MarblePregnancyIncubation);

        // 原版 set_marbleAffection：状态不存在时 createStatusEffect(Marble, 0, 0, 0, 40)
        private static void EnsureMarbleStatus()
        {
            if (!Player.HasStatusEffect(VanillaStatusEffects.Marble))
                Player.CreateStatusEffect(VanillaStatusEffects.Marble, 0, 0, 0, 40);
        }

        // 原版 AmilyScene.amilyFollower()（AmilyScene.hx:253-262）的纯旗标公式；
        // AmilyScene 本体缺失，Contents/Scenes/Vanilla/Camp.cs 已内联同一份公式。
        private static bool AmilyFollower()
        {
            if (Flags.Get(VanillaFlags.AmilyFollower) == 0)
                return false;

            if (Flags.Get(VanillaFlags.FollowerAtFarmAmily) != 0)
                return false;

            var visitingUrta = Flags.Get(VanillaFlags.AmilyVisitingUrta);

            return visitingUrta != 1 && visitingUrta != 2;
        }

        // 原版 IsabellaFollowerScene.hx:113 isabellaAtCamp()：
        // isabellaFollower() && FOLLOWER_AT_FARM_ISABELLA == 0。
        // isabellaFollower() 见 IsabellaScene.cs:68（旗标公式）；C# 没有 IsabellaFollowerScene。
        private static bool IsabellaAtCamp() =>
            Flags.Get(VanillaFlags.IsabellaFollowerAccepted) > 0
            && Flags.Get(VanillaFlags.FollowerAtFarmIsabella) == 0;

        // 原版 classes/PregnancyStore.hx：Marble 的孕况存在两个全局旗标里。
        // type / incubation / event / eventTriggered 由块 D 的 MarbleScene.Night.cs 提供；
        // 这里只补 A 块用到的四项：isPregnant（:133）、pregnancyAdvance（:181）、knockUpForce 清空（:155）。
        private static bool MarblePregnancyIsPregnant => MarblePregnancyType != 0;

        private static void MarblePregnancyAdvance()
        {
            var next = MarblePregnancyIncubation - 1;

            if (next >= 0)
                Flags.Set(VanillaFlags.MarblePregnancyIncubation, next);
        }

        private static void MarblePregnancyKnockUpForce()
        {
            Flags.Set(VanillaFlags.MarblePregnancyType, 0);
            Flags.Set(VanillaFlags.MarblePregnancyIncubation, 0);
        }

        // ── 原版 MarbleScene.hx:240-246 marbleSprite ──
        public void MarbleSprite(bool nude = false)
        {
        }

        // 原版 MarbleScene.hx:503-505 marbleFollower 在 MarbleScene.Addiction.cs 里；
        // 这里只补 marbleAtCamp。
        public bool MarbleAtCamp() =>
            MarbleFollower()
            && Flags.Get(VanillaFlags.MarbleRathazulCounter2) <= 0
            && Flags.Get(VanillaFlags.FollowerAtFarmMarble) == 0;

        // 原版 MarbleScene.hx:508-526 marbleBreastSize
        public string MarbleBreastSize()
        {
            if (Flags.Get(VanillaFlags.MarblePurified) > 0)
            {
                return Flags.Get(VanillaFlags.MarbleBreastSize) switch
                {
                    0 => "DD cup",
                    1 => "G cup",
                    2 => "HH cup",
                    _ => "J cup"
                };
            }

            return Player.StatusEffectValue(VanillaStatusEffects.Marble, 3) > 30 ? "HH cup" : "G cup";
        }

        // 原版 MarbleScene.hx:62-237 timeChange。
        // 原版末尾的 `game.time.hours > 23` 在 C# 里写作 `View.Time.Hours == 0`
        //（TimeAware.cs 头注释：本仓库 HourPassed 在回绕之后触发）。
        public bool TimeChange()
        {
            var needNext = false;

            _checkedMarbleMilk = 0;

            MarblePregnancyAdvance();

            if (MarbleFollower())
            {
                if (Flags.Get(VanillaFlags.MarbleRathazulCounter1) > 0)
                {
                    Flags.Set(VanillaFlags.MarbleRathazulCounter1, Flags.Get(VanillaFlags.MarbleRathazulCounter1) - 1);

                    if (Flags.Get(VanillaFlags.MarbleRathazulCounter1) <= 1)
                        Flags.Set(VanillaFlags.MarbleRathazulCounter1, 1);
                }

                if (Flags.Get(VanillaFlags.MarblePurificationStage) == 0)
                {
                    if (Flags.Get(VanillaFlags.MarbleCountupToPurifying) < 200)
                        Flags.Set(VanillaFlags.MarbleCountupToPurifying, Flags.Get(VanillaFlags.MarbleCountupToPurifying) + 1);
                }

                if (Flags.Get(VanillaFlags.MarbleRathazulCounter2) > 0)
                {
                    Flags.Set(VanillaFlags.MarbleRathazulCounter2, Flags.Get(VanillaFlags.MarbleRathazulCounter2) - 1);

                    if (Flags.Get(VanillaFlags.MarbleRathazulCounter2) <= 1)
                        Flags.Set(VanillaFlags.MarbleRathazulCounter2, 1);
                }

                if (Flags.Get(VanillaFlags.MarblePurificationStage) >= 5)
                {
                    Flags.Set(VanillaFlags.MarbleTimeSinceNursedInHours,
                        Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) + 1);

                    if (Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) > 1000)
                        Flags.Set(VanillaFlags.MarbleTimeSinceNursedInHours, 1000);
                }

                if (Flags.Get(VanillaFlags.MarbleWarnedAboutCorruption) == 1 && Player.IsPureEnough(50))
                    Flags.Set(VanillaFlags.MarbleWarnedAboutCorruption, 0);

                if (Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) < 100)
                    Flags.Set(VanillaFlags.MarbleTimeSinceNursedInHours,
                        Flags.Get(VanillaFlags.MarbleTimeSinceNursedInHours) + 1);

                if (Flags.Get(VanillaFlags.MarbleLust) < -100)
                    Flags.Set(VanillaFlags.MarbleLust, -100);

                if (Rng.Next(2) == 0)
                    Flags.Set(VanillaFlags.MarbleLust, Flags.Get(VanillaFlags.MarbleLust) + 1);

                if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 3) > 50)
                    Flags.Set(VanillaFlags.MarbleLust, Flags.Get(VanillaFlags.MarbleLust) + .3f);

                if (Player.StatusEffectValue(VanillaStatusEffects.Marble, 3) > 70)
                    Flags.Set(VanillaFlags.MarbleLust, Flags.Get(VanillaFlags.MarbleLust) + .3f);

                if (Flags.Get(VanillaFlags.MarbleNurseryConstruction) > 0
                    && Flags.Get(VanillaFlags.MarbleNurseryConstruction) < 100
                    && MarbleAtCamp())
                {
                    Flags.Set(VanillaFlags.MarbleNurseryConstruction,
                        Flags.Get(VanillaFlags.MarbleNurseryConstruction) + 1);

                    if (Flags.Get(VanillaFlags.MarbleNurseryConstruction) >= 100)
                    {
                        MarbleSprite();

                        OutputText("Scenes.Marblescene.TimeChange.NurseryBuiltNotice".Localized());

                        needNext = true;

                        Flags.Set(VanillaFlags.MarbleNurseryConstruction, 100);
                    }
                }
                else if (!Player.HasStatusEffect(VanillaStatusEffects.MarbleHasItem) && MarbleAtCamp())
                {
                    if (!Player.HasStatusEffect(VanillaStatusEffects.MarbleItemCooldown) && Rng.Next(10) == 0)
                    {
                        MarbleSprite();

                        OutputText("Scenes.Marblescene.TimeChange.CampNoteFromMarble".Localized());

                        Player.CreateStatusEffect(VanillaStatusEffects.MarbleItemCooldown, 24 + Rng.Next(24), 0, 0, 0);

                        Player.CreateStatusEffect(VanillaStatusEffects.MarbleHasItem, Rng.Next(10), 0, 0, 0);

                        needNext = true;
                    }
                }
            }

            if (Player.HasStatusEffect(VanillaStatusEffects.MarbleItemCooldown))
            {
                Player.ChangeStatusValue(VanillaStatusEffects.MarbleItemCooldown, 0, -1);

                if (Player.StatusEffectValue(VanillaStatusEffects.MarbleItemCooldown, 0) < 1)
                    Player.RemoveStatusEffect(VanillaStatusEffects.MarbleItemCooldown);
            }

            if (!Player.HasStatusEffect(VanillaStatusEffects.Infested))
                Flags.Set(VanillaFlags.MarbleGrossedOutBecauseWormInfestation, 0);

            if (Player.HasStatusEffect(VanillaStatusEffects.MarblesMilk) && !Player.HasPerk(VanillaPerks.MarblesMilk))
            {
                Player.ChangeStatusValue(VanillaStatusEffects.MarblesMilk, 0, -1);

                if (Player.StatusEffectValue(VanillaStatusEffects.MarblesMilk, 0) <= 0)
                {
                    needNext = true;

                    Player.ApplyStats(
                        new DynStat(VanillaStats.Strength, -Player.StatusEffectValue(VanillaStatusEffects.MarblesMilk, 1)),
                        new DynStat(VanillaStats.Toughness, -Player.StatusEffectValue(VanillaStatusEffects.MarblesMilk, 2)));

                    Player.RemoveStatusEffect(VanillaStatusEffects.MarblesMilk);

                    if (MarbleAddiction <= 10)
                        OutputText("Scenes.Marblescene.TimeChange.EuphoriaFadeRealization".Localized());
                    else if (MarbleAddiction <= 30)
                        OutputText("Scenes.Marblescene.TimeChange.MildEuphoriaLoss".Localized());
                    else if (MarbleAddiction <= 50)
                        OutputText("Scenes.Marblescene.TimeChange.ShiverAndSuckleUrge".Localized());

                    if (KnowAddiction > 0)
                    {
                        if (Player.HasStatusEffect(VanillaStatusEffects.BottledMilk))
                        {
                            OutputText("Scenes.Marblescene.TimeChange.TremblingHandsWithdrawal".Localized());
                        }
                        else
                        {
                            if (MarbleAddiction <= 90)
                                OutputText("Scenes.Marblescene.TimeChange.DesperateTremblingNeed".Localized());
                            else
                                OutputText("Scenes.Marblescene.TimeChange.CravingEscapeUrge".Localized());

                            Player.CreateStatusEffect(VanillaStatusEffects.MarbleWithdrawl, 0, 0, 0, 0);

                            Player.ApplyStats(
                                new DynStat(VanillaStats.Toughness, -5f),
                                new DynStat(VanillaStats.Intelligence, -5f));
                        }
                    }
                }
            }

            if (KnowAddiction > 0
                && !Player.HasPerk(VanillaPerks.MarbleResistant)
                && !Player.HasPerk(VanillaPerks.MarblesMilk)
                && MarbleAddiction > 25
                && AddictionEnabled)
            {
                if (!Player.HasStatusEffect(VanillaStatusEffects.MarblesMilk)
                    && !Player.HasStatusEffect(VanillaStatusEffects.BottledMilk))
                {
                    if (!Player.HasStatusEffect(VanillaStatusEffects.MarbleWithdrawl))
                    {
                        OutputText("Scenes.Marblescene.TimeChange.OverwhelmedMilkDesire".Localized());

                        needNext = true;

                        Player.CreateStatusEffect(VanillaStatusEffects.MarbleWithdrawl, 0, 0, 0, 0);

                        Player.ApplyStats(
                            new DynStat(VanillaStats.Toughness, -5f),
                            new DynStat(VanillaStats.Intelligence, -5f));
                    }
                }
            }

            if (KnowAddiction <= 0 && MarbleAddiction > 60)
                MarbleAddiction = 60;

            if (Player.HasStatusEffect(VanillaStatusEffects.MarbleWithdrawl))
            {
                if (MarbleAddiction <= 25)
                {
                    Player.RemoveStatusEffect(VanillaStatusEffects.MarbleWithdrawl);

                    Player.ApplyStats(
                        new DynStat(VanillaStats.Toughness, 5f),
                        new DynStat(VanillaStats.Intelligence, 5f));

                    OutputText("Scenes.Marblescene.TimeChange.WithdrawalSymptomsGone".Localized());

                    needNext = true;
                }
                else if (Player.HasStatusEffect(VanillaStatusEffects.BottledMilk))
                {
                    OutputText("Scenes.Marblescene.TimeChange.WithdrawalSymptomsGoneAgain".Localized());

                    needNext = true;

                    Player.RemoveStatusEffect(VanillaStatusEffects.MarbleWithdrawl);

                    Player.ApplyStats(
                        new DynStat(VanillaStats.Toughness, 5f),
                        new DynStat(VanillaStats.Intelligence, 5f));
                }
            }

            if (Player.HasStatusEffect(VanillaStatusEffects.BottledMilk))
            {
                Player.ChangeStatusValue(VanillaStatusEffects.BottledMilk, 0, -1);

                if (Player.StatusEffectValue(VanillaStatusEffects.BottledMilk, 0) <= 0)
                    Player.RemoveStatusEffect(VanillaStatusEffects.BottledMilk);
            }

            if (View.Time.Hours == 0)
                Flags.Set(VanillaFlags.MarblePlayedWithKidsToday, 0);

            return needNext;
        }

        // 原版 MarbleScene.hx:281-495 timeChangeLarge。
        //
        // 返回 true 的「暂停等 Next」语义：C# 的 TimeAwareRegistry 没有可恢复事件队列
        //（TimeAware.cs 的 Set 注释 + EventParser.hx:212-219 / :268-274），
        // 所以这里统一 `return false`，但保留原版的早退控制流；按钮 / doNext 仍照原版摆好。
        public bool TimeChangeLarge()
        {
            if (MarblePregnancyIsPregnant && MarblePregnancyIncubation == 0 && MarbleAtCamp())
            {
                // 原版 marblePoopsBaybees（MarbleScene.hx:3651，块 D）
                MarblePoopsBaybees();

                MarblePregnancyKnockUpForce();

                // 原版此处 return true（等 Next）；C# 无队列，见方法头注释。
                return false;
            }

            if (KnowAddiction > 0
                && MarbleAddiction >= 100
                && !Player.HasPerk(VanillaPerks.MarblesMilk)
                && !Player.HasPerk(VanillaPerks.MarbleResistant)
                && View.Time.Hours == 6)
            {
                MarbleSprite();

                OutputText("Scenes.Marblescene.TimeChangeLarge.ChillingClarityAwakening".Localized());
                OutputText("Scenes.Marblescene.TimeChangeLarge.MarbleSmilesInRoom".Localized());

                Player.CreatePerk(VanillaPerks.MarblesMilk, 0, 0, 0, 0);

                if (Player.HasStatusEffect(VanillaStatusEffects.MarbleWithdrawl))
                {
                    Player.RemoveStatusEffect(VanillaStatusEffects.MarbleWithdrawl);

                    Player.ApplyStats(
                        new DynStat(VanillaStats.Toughness, 5f),
                        new DynStat(VanillaStats.Intelligence, 5f));
                }

                if (Player.HasStatusEffect(VanillaStatusEffects.MarblesMilk))
                {
                    Player.RemoveStatusEffect(VanillaStatusEffects.MarblesMilk);
                }
                else
                {
                    Player.ApplyStats(
                        new DynStat(VanillaStats.Strength, 5f),
                        new DynStat(VanillaStats.Toughness, 10f));
                }

                if (KnowAddiction == 1)
                {
                    if (MarbleAffection < 49)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.PulledIntoHerChest".Localized());

                        // 原版 doNext(marbleBadEndFollowup)（MarbleScene.hx:3574，块 D）
                        DoNext(() => MarbleBadEndFollowup());

                        return false;
                    }

                    if (MarbleAffection >= 50 && MarbleAffection < 79)
                        OutputText("Scenes.Marblescene.TimeChangeLarge.LeaveThingsAsIs".Localized());

                    if (MarbleAffection >= 80)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.LikingHerForHer".Localized());

                        // 原版 marbleAddictionSex(false)（MarbleScene.hx:2130，块 C）
                        MarbleAddictionSex(false);

                        OutputText("Scenes.Marblescene.TimeChangeLarge.ParagraphBreak".Localized());

                        Player.CreateStatusEffect(VanillaStatusEffects.CampMarble, 0, 0, 0, 0);

                        Flags.Set(VanillaFlags.FollowerAtFarmMarble, 0);

                        if (IsabellaAtCamp())
                            Flags.Set(VanillaFlags.IsabellaMurbleBleh, 1);

                        Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);

                        Flags.Set(VanillaFlags.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

                        if (Flags.Get(VanillaFlags.IzmaFollowerStatus) == 1
                            && Flags.Get(VanillaFlags.FollowerAtFarmIzma) == 0)
                            Flags.Set(VanillaFlags.IzmaMarbleFreakoutStatus, 1);
                    }
                }
                else
                {
                    if (MarbleAffection < 30)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ChestClutchApology".Localized());

                        DoNext(() => MarbleBadEndFollowup());

                        return false;
                    }

                    if (MarbleAffection < 80)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.DailyMilkArrangement".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ForgivenessAndSpecialAsk".Localized());

                        MarbleAddictionSex(false);

                        OutputText("Scenes.Marblescene.TimeChangeLarge.ParagraphBreakAgain".Localized());

                        SetMarbleMovedToCamp();
                    }
                }

                OutputText("Scenes.Marblescene.TimeChangeLarge.MilkPerkGained".Localized());

                PlayerMenu();

                return false;
            }

            if (Player.HasStatusEffect(VanillaStatusEffects.Marble) && View.Time.Hours == 6)
                MarbleAddiction -= 1;

            if (KnowAddiction > 0
                && MarbleAddiction < 25
                && !Player.HasPerk(VanillaPerks.MarblesMilk)
                && !Player.HasPerk(VanillaPerks.MarbleResistant)
                && View.Time.Hours == 6)
            {
                MarbleSprite();

                OutputText("Scenes.Marblescene.TimeChangeLarge.StrangelyRestedAwakening".Localized());
                OutputText("Scenes.Marblescene.TimeChangeLarge.StartledMarbleGreeting".Localized());

                Player.ApplyStats(new DynStat(VanillaStats.Corruption, -5f));

                Player.CreatePerk(VanillaPerks.MarbleResistant, 0, 0, 0, 0);

                if (KnowAddiction == 1)
                {
                    if (MarbleAffection < 30)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.SheStormsOutLeaving".Localized());

                        Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);
                    }

                    if (MarbleAffection >= 30 && MarbleAffection < 90)
                        OutputText("Scenes.Marblescene.TimeChangeLarge.CalmingDownTogether".Localized());

                    if (MarbleAffection >= 90)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.StaysAtCampOffer".Localized());
                        OutputText("Scenes.Marblescene.TimeChangeLarge.LoversMenuNotice".Localized());

                        Player.CreateStatusEffect(VanillaStatusEffects.CampMarble, 0, 0, 0, 0);

                        Flags.Set(VanillaFlags.FollowerAtFarmMarble, 0);

                        if (IsabellaAtCamp())
                            Flags.Set(VanillaFlags.IsabellaMurbleBleh, 1);

                        Flags.Set(VanillaFlags.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

                        if (Flags.Get(VanillaFlags.IzmaFollowerStatus) == 1
                            && Flags.Get(VanillaFlags.FollowerAtFarmIzma) == 0)
                            Flags.Set(VanillaFlags.IzmaMarbleFreakoutStatus, 1);

                        Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);
                    }
                }
                else
                {
                    if (MarbleAffection < 30)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ColdStareAtNews".Localized());
                        OutputText("Scenes.Marblescene.TimeChangeLarge.TornEncyclopediaPage".Localized());

                        // 原版 camp.codex.codexEntryLaBovines()（Codex.hx:394）在本移植里没有条目正文，正文跳过、解锁照常。
                        OutputText("Scenes.Marblescene.TimeChangeLarge.CrudeHandwrittenThreat".Localized());

                        Player.UnlockCodexEntry(VanillaFlags.CodexEntryLabovines);

                        Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);
                    }

                    if (MarbleAffection >= 30 && MarbleAffection < 70)
                        OutputText("Scenes.Marblescene.TimeChangeLarge.ReassuredGoodFriend".Localized());

                    if (MarbleAffection >= 70)
                    {
                        OutputText("Scenes.Marblescene.TimeChangeLarge.TearsAndTightHug".Localized());

                        MarbleAddictionSex(false);

                        OutputText("Scenes.Marblescene.TimeChangeLarge.ParagraphBreakThird".Localized());

                        Player.CreateStatusEffect(VanillaStatusEffects.CampMarble, 0, 0, 0, 0);

                        Flags.Set(VanillaFlags.FollowerAtFarmMarble, 0);

                        if (IsabellaAtCamp())
                            Flags.Set(VanillaFlags.IsabellaMurbleBleh, 1);

                        Flags.Set(VanillaFlags.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

                        if (Flags.Get(VanillaFlags.IzmaFollowerStatus) == 1
                            && Flags.Get(VanillaFlags.FollowerAtFarmIzma) == 0)
                            Flags.Set(VanillaFlags.IzmaMarbleFreakoutStatus, 1);

                        Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);
                    }
                }

                OutputText("Scenes.Marblescene.TimeChangeLarge.ResistancePerkGained".Localized());

                PlayerMenu();

                return false;
            }

            // 原版 `if (!addictionEnabled && false) setMarbleMovedToCamp();` 是死分支（&& false），不移植。

            if (_checkedMarbleMilk++ == 0 && View.Time.Hours == 6 && Player.HasPerk(VanillaPerks.MarblesMilk))
            {
                if (MarbleAtCamp())
                {
                    PostAddictionCampMornings(false);
                }
                else
                {
                    if (!Player.HasStatusEffect(VanillaStatusEffects.NoMoreMarble))
                    {
                        PostAddictionFarmMornings();

                        // 原版 game.timeQ += 1（CoC.hx:407）：C# 没有 timeQ 队列
                        //（同 MarbleScene.Night.cs:1022 的说明），跳过。
                    }
                }

                PlayerMenu();

                return false;
            }

            return false;
        }

        // ── 原版 MarbleScene.hx:528-546 setMarbleMovedToCamp ──
        private void SetMarbleMovedToCamp()
        {
            Player.CreateStatusEffect(VanillaStatusEffects.CampMarble, 0, 0, 0, 0);

            Flags.Set(VanillaFlags.FollowerAtFarmMarble, 0);

            if (IsabellaAtCamp())
                Flags.Set(VanillaFlags.IsabellaMurbleBleh, 1);

            Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);

            Flags.Set(VanillaFlags.MarbleOrAmilyFirstForFreakout, AmilyFollower() ? 2 : 1);

            if (Flags.Get(VanillaFlags.IzmaFollowerStatus) == 1
                && Flags.Get(VanillaFlags.FollowerAtFarmIzma) == 0)
                Flags.Set(VanillaFlags.IzmaMarbleFreakoutStatus, 1);
        }

        // 原版 MarbleScene.hx:549-573 encounterMarbleInitially
        public void EncounterMarbleInitially()
        {
            MarbleSprite();

            Player.CreateStatusEffect(VanillaStatusEffects.Marble, 0, 0, 0, 40);

            OutputText("Scenes.Marblescene.EncounterMarbleInitially.SouthernBelleSpotted".Localized());
            OutputText("Scenes.Marblescene.EncounterMarbleInitially.WhitneyExplainsSoreness".Localized());
            OutputText("Scenes.Marblescene.EncounterMarbleInitially.SearchingBarnRooms".Localized());

            Player.UnlockCodexEntry(VanillaFlags.CodexEntryLabovines, false, true);

            if (Player.Tallness < 60)
            {
                OutputText("Scenes.Marblescene.EncounterMarbleInitially.CuteMilkInvitation".Localized());

                ClearButtons();
                AddButton(0, "Let Her", MarblePicksYouUpInitially);
                AddButton(1, "Don't", ResistMarbleInitially);

                return;
            }

            OutputText("Scenes.Marblescene.EncounterMarbleInitially.NameExchange".Localized());

            ClearButtons();
            AddButton(0, "Caress", CaressMarble, "Give her a gentle massage.");
            AddButton(1, "Suckle", SuckleMarble, "Lighten her load by drinking some milk.");
            AddButton(2, "Rape", () => RapeDAHMARBLEZ(), "Take what you want from her.");
            AddButton(14, "Leave", TurnOffMarbleForever);
        }

        // 原版 MarbleScene.hx:575-581 turnOffMarbleForever
        public void TurnOffMarbleForever()
        {
            ClearOutput();

            Flags.Set(VanillaFlags.MarbleWarning, 1);

            OutputText("Scenes.Marblescene.TurnOffMarbleForever.ConsideringHerBreasts".Localized());

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:584-594 marbleWarningStateMeeting
        public void MarbleWarningStateMeeting()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarbleWarningStateMeeting.FieldEncounterWalk".Localized());

            ClearButtons();
            AddButton(0, "Apologize", ApologizeToWalkingTitsIMEANMARBLE);
            AddButton(1, "Pick Fight", PickAFight);
            AddButton(2, "Leave4Ever", LeaveNonFriendsMarble4EVERRRR);
            AddButton(14, "Leave", LeaveNonFriendsMarble);
        }

        // 原版 MarbleScene.hx:597-602 leaveNonFriendsMarble
        public void LeaveNonFriendsMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.LeaveNonFriendsMarble.InsincerePoliteExit".Localized());

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:605-613 leaveNonFriendsMarble4EVERRRR
        public void LeaveNonFriendsMarble4EVERRRR()
        {
            ClearOutput();

            Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);

            Flags.Set(VanillaFlags.MarbleWarning, 2);

            OutputText("Scenes.Marblescene.LeaveNonFriendsMarble4EVERRRR.BlankLookRefusal".Localized());

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:615-648 apologizeToWalkingTitsIMEANMARBLE
        public void ApologizeToWalkingTitsIMEANMARBLE()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.SeekingReconciliation".Localized());
            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.DucksIntoGrainField".Localized());
            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.ChasingThroughCrops".Localized());

            if (Player.Intelligence < 15)
                OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.CaughtWithHandsUp".Localized());
            else if (Player.Intelligence < 40)
                OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.WaitingHandsRaised".Localized());
            else
                OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.EasyCatchOffGuard".Localized());

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.PulledOntoHerLap".Localized());
            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.OffersBountifulBreasts".Localized());
            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.ExpectedNursingOffer".Localized());
            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.AskedToSwitchBreast".Localized());

            Player.RefillHunger(20);

            OutputText("Scenes.Marblescene.ApologizeToWalkingTitsIMEANMARBLE.FinallyFedPullBack".Localized());

            MarbleAffection = 5;
            MarbleAddiction += 10;

            Flags.Set(VanillaFlags.MarbleWarning, 0);

            ApplyMarblesMilk();

            Player.ApplyStats(
                new DynStat(VanillaStats.Libido, .2f),
                new DynStat(VanillaStats.Lust, 5 + Player.Libido / 10f));

            Player.HealthChange(100);
            Player.ChangeFatigue(-50);

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:651-658 pickAFight
        public void PickAFight()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.PickAFight.VoicingDispleasure".Localized());

            ClearButtons();
            AddButton(0, "Stay", StayForFights);
            AddButton(1, "Fuck That", GetOutOfDodge);
        }

        // 原版 MarbleScene.hx:661-666 stayForFights：startCombat(new Marble(), true)
        public void StayForFights()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.StayForFights.ArmsCrossedDefiance".Localized());

            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "marble")), () => Camp.ReturnToCamp(1));
        }

        // 原版 MarbleScene.hx:669-675 getOutOfDodge
        public void GetOutOfDodge()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.GetOutOfDodge.TruthOverTalkingHammers".Localized());

            Flags.Set(VanillaFlags.MarbleWarning, 3);

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:831-843 resistMarbleInitially
        public void ResistMarbleInitially()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.ResistMarbleInitially.ResistanceCausesApology".Localized());
            OutputText("Scenes.Marblescene.ResistMarbleInitially.NameIntroduction".Localized());

            ClearButtons();
            AddButton(0, "Caress", CaressMarble, "Give her a gentle massage.");
            AddButton(1, "Suckle", SuckleMarble, "Lighten her load by drinking some milk.");
            AddButton(2, "Rape", () => RapeDAHMARBLEZ(), "Take what you want from her.");
            AddButton(14, "Leave", TurnOffMarbleForever);
        }

        // 原版 MarbleScene.hx:845-865 marblePicksYouUpInitially
        public void MarblePicksYouUpInitially()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.LiftedOntoHerLap".Localized());

            Player.RefillHunger(30);

            OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.MilkedEnoughLeaning".Localized());

            if (Player.Body.Get<MonoCoC.Contents.BodyParts.Vanilla.EarsPart>().Id != VanillaBodyPartTypes.Human)
                OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.EarScratchGesture".Localized());
            else
                OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.HeadCaressGesture".Localized());

            OutputText("Scenes.Marblescene.MarblePicksYouUpInitially.ThankfulAsksForName".Localized());

            MarbleAffection += 30;
            MarbleAddiction += 10;

            ApplyMarblesMilk();

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:868-874 caressMarble
        public void CaressMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.CaressMarble.OfferGentleCaress".Localized());

            Player.ApplyStats(new DynStat(VanillaStats.Lust, 15f));

            MarbleAffection += 5;

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:877-894 suckleMarble
        public void SuckleMarble()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.SuckleMarble.SuggestSucklingHelp".Localized());

            if (Player.Tallness < 60)
                OutputText("Scenes.Marblescene.SuckleMarble.ChairToReachBreasts".Localized());

            OutputText("Scenes.Marblescene.SuckleMarble.LeaningToSuckle".Localized());

            Player.RefillHunger(30);

            MarbleAffection += 15;
            MarbleAddiction += 10;

            ApplyMarblesMilk();

            Player.ApplyStats(new DynStat(VanillaStats.Lust, 25f));

            ReturnToCamp(1);
        }

        // 原版 MarbleScene.hx:716-790 rapeMarbleInHerRoom
        public void RapeMarbleInHerRoom()
        {
            ClearOutput();

            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.DraggedFromAudience".Localized());
            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.RoughBreastSqueeze".Localized());
            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SlapDodgedNippleTwist".Localized());

            if (Player.Cocks.Count > 0)
            {
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.ChuckleFreeingCocks".Localized());

                if (Player.Cocks[0].Thickness < 3)
                {
                    if (Player.Cocks[0].Length <= 8)
                        OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.ForcingDeepInsertion".Localized());
                    else
                        OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.HiltDeepInsertion".Localized());

                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.GruntingBreastPlay".Localized());

                    if (Player.Corruption >= 33)
                        MarbleRapeCorruptFantasy();

                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.TauntingInsideRelease".Localized());

                    Player.Orgasm("Dick");
                }
                else
                {
                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.DifficultGirthInsertion".Localized()
                        Player.CockDescript(0));

                    if (Player.Corruption >= 33)
                        MarbleRapeCorruptFantasy();

                    OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.TauntingOutsideRelease".Localized());

                    Player.Orgasm("Dick");
                }
            }
            else if (Player.Vaginas.Count > 0)
            {
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SearchingForDildo".Localized());
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SharedDildoSetup".Localized()
                    Player.VaginaDescript(0));

                if (Player.Corruption >= 33)
                    MarbleRapeCorruptFantasy();

                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.TauntingDildoRelease".Localized());

                Player.Orgasm("Vaginal");
            }
            else
            {
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.GenitalLackWorkaround".Localized());

                if (Player.IsCorruptEnough(33))
                    MarbleRapeCorruptFantasy();

                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.FullFistInsertion".Localized());

                Player.ApplyStats(new DynStat(VanillaStats.Lust, -20f));
            }

            OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.SatisfiedHammerClaim".Localized());

            if (Flags.Get(VanillaFlags.FarmCorruptionStarted) == 0)
                OutputText("Scenes.Marblescene.RapeMarbleInHerRoom.AngryWhitneyConfronts".Localized());

            Flags.Set(VanillaFlags.FarmDisabled, 1);

            CombatScene.CleanupAfterCombat();
        }

        // 原版 MarbleScene.hx:793-829 forceFeedMarble
        public void ForceFeedMarble()
        {
            ClearOutput();

            if (Player.HasPerk(VanillaPerks.Feeder))
                OutputText("Scenes.Marblescene.ForceFeedMarble.BetSheLovesMilking".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.NoticingFarmCrowd".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.BaringChestToCrowd".Localized());

            if (Player.BiggestBreastSize() <= 2)
                OutputText("Scenes.Marblescene.ForceFeedMarble.PressingLipsToNipple".Localized());
            else
                OutputText("Scenes.Marblescene.ForceFeedMarble.LiftingNippleToLips".Localized());

            OutputText("Scenes.Marblescene.ForceFeedMarble.TightlyClosedRefusal".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.TeasingHerToDrink".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.AngryGaspOpening".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.CommandingHerToDrink".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.SqueezeMakesHerGulp".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.OfferingHelpWithMess".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.ExposingBothBreasts".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.SqueezeWithoutMess".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.InevitableMessAccepted".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.PlayfulMilkingHalt".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.CowGirlBlueBalls".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.ThoroughHandMilking".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.RelaxedAwarenessReturn".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.ApologyCuppingTits".Localized());
            OutputText("Scenes.Marblescene.ForceFeedMarble.CrowdDissolvesWarily".Localized());

            Player.CreateStatusEffect(VanillaStatusEffects.NoMoreMarble, 0, 0, 0, 0);

            Player.ChangeStatusValue(VanillaStatusEffects.Feeder, 0, 1);
            Player.ChangeStatusValue(VanillaStatusEffects.Feeder, 2, 0);

            CombatScene.CleanupAfterCombat();
        }

        // 块 D 的 partial 实现（MarbleScene.Night.cs）需要这里的定义声明。
        // 原版 MarbleScene.hx:3442-3529 rapeDAHMARBLEZ
        private partial void RapeDAHMARBLEZ();

        // 原版 MarbleScene.hx:3533-3541 marbleRapeCorruptFantasy
        private partial void MarbleRapeCorruptFantasy();

        // 原版 MarbleScene.hx:3574-3647 marbleBadEndFollowup
        private partial void MarbleBadEndFollowup();

        // 原版 MarbleScene.hx:3651-3769 marblePoopsBaybees
        private partial void MarblePoopsBaybees();

        // 块 C 的 marbleAddictionSex（MarbleScene.hx:2130-2185）与
        // postAddictionCampMornings（MarbleScene.hx:1848-1877）由 MarbleScene.Camp.cs 直接实现，无需声明。
    }
}
