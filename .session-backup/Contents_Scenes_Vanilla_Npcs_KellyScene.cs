// 原版 classes.scenes.places.farm.Kelly（Kelly.hx，2413 行；类名 Kelly，C# 侧按场景命名 KellyScene）
// 由 Tools/PortKit/scene-port.mjs 生成骨架后人工补齐：妊娠改成 Kelly 自己的 flag 存储、
// 补 TimeChange / KellySprite、把 farm.* 依赖换成本地等价实现。

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
using MonoCoC.Contents.Scenes.Vanilla.Places;
using MonoCoC.Contents.Scenes.Vanilla.Places.Farm;

namespace MonoCoC.Contents.Scenes.Vanilla.Npcs
{
    // 原版 classes.scenes.places.farm.Kelly extends AbstractFarmContent（Kelly.hx）
    // 由 Tools/PortKit/scene-port.mjs 生成骨架后人工补齐。
    [Register("coc:kelly_scene")]
    public class KellyScene : Scene, ITimeAware
    {



        private static bool noFur => !Player.HasFur();

        private static bool silly => OptionStore.Instance.Get("silly", 0) > 0;

        private static bool metric => OptionStore.Instance.Get("metric", 0) > 0;

        private static bool realistic => OptionStore.Instance.Get("realistic", 0) > 0;

        private static bool survival => OptionStore.Instance.Get("survival", 0) > 0;

        private static bool hasKnot => Player.Cocks.Count > 0 && Player.Cocks.Any(c => c.KnotMultiplier > 1);

        private static void ReturnToCamp(int hours) => Go(SceneRegistry.CreateOrDefault("coc:camp"));

        // ── 原版依赖的查询（Haxe 里在别的场景/系统上，C# 侧还没移植对应 API）──
        // 全部直接读原版用的同一批 flag，语义照抄：
        // FarmCorruption.hasTattoo/numTattoos/kellyFullTribalTats（FarmCorruption.hx:3254-3263、5326-5331）
        private static bool KellyTattooFlagSet(Flag<string> flag) => !string.IsNullOrEmpty(CoC.Instance.Flags.Get(flag));

        private static int NumKellyTattoos()
        {
            var count = 0;

            if (KellyTattooFlagSet(VanillaFlags.KellyTattooCollarbone)) count++;
            if (KellyTattooFlagSet(VanillaFlags.KellyTattooShoulders)) count++;
            if (KellyTattooFlagSet(VanillaFlags.KellyTattooLowerback)) count++;
            if (KellyTattooFlagSet(VanillaFlags.KellyTattooButt)) count++;

            return count;
        }

        // FarmCorruption.numKellyTribalTats：四个部位里含「A tribal tattoo」的个数（FarmCorruption.hx:5293-5324）
        private static int NumKellyTribalTats()
        {
            static bool Tribal(string text) => text.Contains("A tribal tattoo", StringComparison.Ordinal);

            var count = 0;

            if (Tribal(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooCollarbone))) count++;
            if (Tribal(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooShoulders))) count++;
            if (Tribal(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooLowerback))) count++;
            if (Tribal(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooButt))) count++;

            return count;
        }

        // FarmCorruption.whitneyCorrupt（FarmCorruption.hx:85-93）
        private static bool WhitneyCorrupt() => CoC.Instance.Flags.Get(VanillaFlags.WhitneyCorruptionComplete) > 0;

        // TelAdre.isAllowedInto（TelAdre.hx:106-108）：player.statusEffectv1(StatusEffects.TelAdre) >= 1
        private static bool TelAdreAllowedInto() => Player.StatusEffectValue(VanillaStatusEffects.TelAdre, 0) >= 1;

        // AmilyScene.amilyFollowerCorrupt（AmilyScene.hx:253-275）：amilyFollower() && amilyCorrupt()
        private static bool AmilyFollowerCorrupt()
        {
            if (CoC.Instance.Flags.Get(VanillaFlags.AmilyFollower) <= 0)
                return false;

            if (CoC.Instance.Flags.Get(VanillaFlags.FollowerAtFarmAmily) != 0)
                return false;

            var visitingUrta = CoC.Instance.Flags.Get(VanillaFlags.AmilyVisitingUrta);

            if (visitingUrta == 1 || visitingUrta == 2)
                return false;

            return CoC.Instance.Flags.Get(VanillaFlags.AmilyFollower) == 2;
        }

        // 原版 farm.keltScene.keltEncounter()（Kelly.hx:103、118、212）
        private static void KeltEncounter() => SceneInstance<KeltScene>.Get().KeltEncounter();

        // ── Kelly 自己的妊娠（原版 Kelly.hx:41-49 的 pregnancy = new PregnancyStore(KELLY_PREGNANCY_TYPE, KELLY_INCUBATION)）──
        // C# 还没有 PregnancyStore；Player.Pregnancy 才是玩家自己的，不能借用，所以这里按 PregnancyStore.hx:152-200
        // 的语义，直接用这两个 flag 复刻一份 Kelly 专用的妊娠状态。
        private const int KellyPregnancyPlayerFather = 25; // PregnancyStore.PREGNANCY_PLAYER

        // 原版 addPregnancyEventSet(PREGNANCY_PLAYER, [280, 200, 100])（Kelly.hx:46）
        private static readonly int[] KellyPregnancyEvents = [280, 200, 100];

        private static int KellyPregnancyType => CoC.Instance.Flags.Get(VanillaFlags.KellyPregnancyType) & 0xFFFF;

        private static bool KellyIsPregnant => KellyPregnancyType != 0;

        private static int KellyPregnancyIncubation => CoC.Instance.Flags.Get(VanillaFlags.KellyIncubation);

        // PregnancyStore.pregnancyAdvance（PregnancyStore.hx:179-184）
        private static void KellyPregnancyAdvance()
        {
            var next = KellyPregnancyIncubation - 1;

            if (next >= 0)
                CoC.Instance.Flags.Set(VanillaFlags.KellyIncubation, next);
        }

        // PregnancyStore.event（PregnancyStore.hx:190-204）
        private static int KellyPregnancyEvent
        {
            get
            {
                if (KellyPregnancyType == 0)
                    return 0;

                // 原版事件表只登记了 PREGNANCY_PLAYER；其它父亲类型返回 1
                if (KellyPregnancyType != KellyPregnancyPlayerFather)
                    return 1;

                for (var i = 0; i < KellyPregnancyEvents.Length; i++)
                {
                    if (KellyPregnancyIncubation > KellyPregnancyEvents[i])
                        return i + 1;
                }

                return KellyPregnancyEvents.Length + 1;
            }
        }

        // PregnancyStore.knockUp / knockUpForce（PregnancyStore.hx:152-168）
        private static void KellyKnockUpForce()
        {
            CoC.Instance.Flags.Set(VanillaFlags.KellyPregnancyType, 0);
            CoC.Instance.Flags.Set(VanillaFlags.KellyIncubation, 0);
        }

        private static void KellyKnockUp(int type, int incubation)
        {
            if (KellyIsPregnant)
                return;

            CoC.Instance.Flags.Set(VanillaFlags.KellyPregnancyType, type);
            CoC.Instance.Flags.Set(VanillaFlags.KellyIncubation, type == 0 ? 0 : incubation);
        }

        // PregnancyStore.knockUpChance（PregnancyStore.hx:169-177）：boundInt(floor, floor+virilityQ, ceiling)
        private static bool KellyKnockUpChance(int floor, int ceiling)
        {
            if (KellyIsPregnant)
                return false;

            var chance = Math.Clamp(floor + Player.VirilityQ(), floor, ceiling);

            return Random.Shared.NextDouble() * 100 < chance;
        }

        // 原版 TimeAwareInterface.timeChange（Kelly.hx:52-69）：
        //   推进妊娠；午夜（原版 game.time.hours > 23 —— 因为 timeChange 在 hours 递增到 24、回绕之前调用；
        //   C# 的 Time.PassTime 是回绕后才发 HourPassed，所以这里等价于 Hours == 0）处理 reward 冷却与不听话计数；
        //   临盆（isPregnant && incubation == 0）→ kellyPopsOutARunt + 清妊娠。
        // 注意：C# 目前没有 TimeAware 派发器（TamanisDaughtersScene.TimeChange 也没有调用点），本方法等时间系统接线。
        public bool TimeChange()
        {
            KellyPregnancyAdvance();

            if (View.Time.Hours == 0)
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyRewardCooldown) > 0 && View.Time.Days % 3 == 0)
                    CoC.Instance.Flags.Set(VanillaFlags.KellyRewardCooldown, 0);

                if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) >= 4)
                    CoC.Instance.Flags.Set(VanillaFlags.KellyDisobeyingCounter,
                        CoC.Instance.Flags.Get(VanillaFlags.KellyDisobeyingCounter) + 1);
            }

            if (KellyIsPregnant && KellyPregnancyIncubation == 0)
            {
                KellyPopsOutARunt();
                KellyKnockUpForce();
                return true;
            }

            return false;
        }

        // 原版 TimeAwareInterface.timeChangeLarge（Kelly.hx:71-73）：恒 false
        public bool TimeChangeLarge() => false;

        public override void Entry()
        {
            KellySprite();
        }

        // 原版 kellySprite()（Kelly.hx:77-91）：按「是否怀孕 / 有没有额外胸排」选四张立绘之一。
        // C# 没有对应贴图（TextureManager 里没有 kelly*），SpriteSelect 自带 null 保护 → no-op。
        public void KellySprite(bool nude = false)
        {
            var pregnant = KellyIsPregnant;
            var bigBoobs = CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) != 0;

            SpriteSelect(pregnant
                ? (bigBoobs ? "kelly_brst_preg" : "kelly_preg")
                : (bigBoobs ? "kelly_brst" : "kelly"));

            _ = nude; // 原版参数也未使用
        }

        // Haxe HasPinkEgg()   outputText 0 
        public bool HasPinkEgg()
        {

            return Player.HasItem(ItemRegistry.Create(VanillaConsumables.EggPink)) || Player.HasItem(ItemRegistry.Create(VanillaConsumables.EggLargePink));
        }

        // Haxe BreakingKeltOptions()   outputText 2 
        public void BreakingKeltOptions()
        {
            ClearOutput();

            if ((!Player.HasCock() && CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) == 0) || CoC.Instance.Flags.Get(VanillaFlags.NeverResistKelt) == 1 || Player.StatusEffectValue(VanillaStatusEffects.Kelt, 1) >= 40 || !Player.HasStatusEffect(VanillaStatusEffects.Kelt))
            {
                // 原版 farm.keltScene.keltEncounter()（Kelly.hx:103）：KeltScene 未移植（后续任务），先走空入口
                KeltEncounter();
                return ;
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) > 0)
            {
                if (!Player.HasCock())
                {
                    OutputText("Scenes.Kelly.BreakingKeltOptions.BreakKeltNeedsTool".Localized());
                    ClearButtons();
                    AddButton(0, "Next", () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());
                    return ;
                }
                ResistKeltsBSBreakHimIntro();
                return ;
            }
            OutputText("Scenes.Kelly.BreakingKeltOptions.KeltAbuseForTraining".Localized());
            ClearButtons();
            AddButton(0, "Endure", KeltEncounter);
            AddButton(1, "Resist Kelt", ResistKeltsBSBreakHimIntro);
            AddButton(2, "Never Resist", NeverBreakKeltIntoKelly);
            AddButton(4, "FIGHT!", FightToBeatKelt);
            AddButton(10, "Appearance", KeltAppearance);
        }

        // Haxe KeltAppearance()   outputText 4 
        public void KeltAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KeltAppearance.KeltCentaurDescription".Localized());
            OutputText("Scenes.Kelly.KeltAppearance.SquareChiseledFace".Localized());
            OutputText("Scenes.Kelly.KeltAppearance.HairyDirtyChest".Localized());
            OutputText("Scenes.Kelly.KeltAppearance.ArmedWithBow".Localized());
            ClearButtons();
            AddButton(0, "Next", BreakingKeltOptions);
        }

        // Haxe FightToBeatKelt()   outputText 1 
        public void FightToBeatKelt()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.FightToBeatKelt.ReadyWeaponAgainstKelt".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.KeltKillPlan, 1);
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kelt")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
        }

        // Haxe ResistKeltsBSBreakHimIntro()   outputText 12 
        public void ResistKeltsBSBreakHimIntro()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) == 0)
            {
                OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.AnnoyedByKeltRudeness".Localized());
                if (!(Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 15) || (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 10) && HasPinkEgg()) || (Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 10) && HasPinkEgg()) || Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 15)))
                {
                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.MissingTamingItems".Localized());
                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.FifteenMilkHint".Localized());
                    ClearButtons();
                    AddButton(0, "Next", () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());
                }
                else
                {
                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.TeachHimLessonPrompt".Localized());
                    ClearButtons();
                    AddButton(0, "Yes", BreakKeltGo);
                    AddButton(1, "Not Yet", () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());
                    AddButton(2, "Never", NeverBreakKeltIntoKelly);
                }
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) == 1)
                {
                    OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.EagerToFindKelt".Localized());
                    if (!(Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 10) || Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 10) || (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 5) && HasPinkEgg()) || (Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 5) && HasPinkEgg())))
                    {
                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.KeepAnotherLesson".Localized());
                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.TenMilkHint".Localized());
                        ClearButtons();
                        AddButton(0, "Next", () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());
                        return ;
                    }
                    ClearButtons();
                    AddButton(0, "Next", SecondKeltBreaking);
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) == 2)
                    {
                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.BackOfFarmMeeting".Localized());
                        if (!(Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 5) || Player.HasItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 5)))
                        {
                            OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.AcquireSuccubiMilk".Localized());
                            OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.FiveMilkHint".Localized());
                            ClearButtons();
                            AddButton(0, "Next", () => SceneInstance<FarmScene>.Get().FarmExploreEncounter());
                            return ;
                        }
                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.ShakingMilkVial".Localized());
                        OutputText("Scenes.Kelly.ResistKeltsBSBreakHimIntro.KeltDrawsBow".Localized());
                        CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kelt")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
                    }
                    else
                    {
                        if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) == 3)
                        {
                            KellySprite();
                            FinalKeltBreaking();
                        }
                        else
                        {
                            KellySprite();
                            ApproachKelly();
                        }
                    }
                }
            }
        }

        // Haxe NeverBreakKeltIntoKelly()   outputText 1 
        public void NeverBreakKeltIntoKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.NeverResistKelt, 1);
            OutputText("Scenes.Kelly.NeverBreakKeltIntoKelly.NeverBreakKeltDecision".Localized());
            ClearButtons();
            AddButton(0, "Go To Kelt", KeltEncounter);
            AddButton(1, "Go Home", () => ReturnToCamp(1));
        }

        // Haxe BreakKeltGo()   outputText 62 
        public void BreakKeltGo()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.BreakKeltGo.ApproachUppityCentaur".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltHostileGreeting".Localized(
                Player.Mf("sissy", "bitch")
            ));
            OutputText("Scenes.Kelly.BreakKeltGo.ColdCalmRefusal".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltFuckOffSpeech".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.WhamImpact".Localized());
            if (Player.Tallness < 70)
            {
                OutputText("Scenes.Kelly.BreakKeltGo.JumpUpPrefix".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakKeltGo.YLetter".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.StrikesKeltForehead".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.DragUnconsciousKelt".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.RopesOnBarnFloor".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltWakingThreat".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltTauntsAboutFucking".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.CruelSmilePullsMilk".Localized());
            if (HasPinkEgg())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.EggMentionFragment".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.PourMilkIntoKeg".Localized());
            if (HasPinkEgg())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.ThrowEggIntoKeg".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.KeltPleadingRefusal".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeepStirringFluid".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.PullOutAndStroke".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakKeltGo.SlapHardeningDick".Localized());
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Kelly.BreakKeltGo.SLetter".Localized());
                }
                OutputText("Scenes.Kelly.BreakKeltGo.AgainstYourBelly".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.AddFlavorToMeal".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltProtestsBinding".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.MasturbateHalfHorse".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakKeltGo.TouchYourselfLead".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.PrecumDroplets".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltWarnsAboutJob".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.StopAfterEnoughPre".Localized());
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Kelly.BreakKeltGo.PrecumIt".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakKeltGo.PrecumThey".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.LiftKegToKelt".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.TauntAboutWives".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltShockedDenial".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.ShoveFunnelInMouth".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.SwiftFeminization".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.CheckGenitalsChange".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.DelightfulSobs".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.SoothingForOwnGood".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.DisarmingSmirkLead".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.WhipOutCock".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakKeltGo.GrindBetweenTits".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.SentencePeriod".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.GirlishVoiceLine".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.BreaksEyeContact".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.BackAdverb".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.DownAdverb".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.UnderYouPhrase".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.AtYourCockPressed".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.WrapsTongueCocksucker".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.HindlegsDancePrefix".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.CockslapHer".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.KeltScoldsTechnique".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.WhiningApology".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.SuggestBreastsInstead".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.NodsWrapsBreasts".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakKeltGo.ThrustThroughCleavage".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakKeltGo.GrabNipplesThrust".Localized());
            }
            OutputText("Scenes.Kelly.BreakKeltGo.OccasionalTipKiss".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.ClimaxRagingGrunt".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.MediocreDismissal".Localized());
            OutputText("Scenes.Kelly.BreakKeltGo.WalkAwayAfterUse".Localized());
            if (HasPinkEgg())
            {
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.EggPink)))
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.EggPink));
                }
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.EggLargePink));
                }
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 10))
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 10);
                }
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 10);
                }
            }
            else
            {
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 15))
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 15);
                }
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 15);
                }
            }
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 5f));
            CoC.Instance.Flags.Set(VanillaFlags.KeltBreakLevel, 1);
            ReturnToCamp(1);
        }

        // Haxe SecondKeltBreaking()   outputText 7 
        public void SecondKeltBreaking()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.SecondKeltBreaking.StrollUpToKelt".Localized());
            OutputText("Scenes.Kelly.SecondKeltBreaking.PointAndLaugh".Localized());
            OutputText("Scenes.Kelly.SecondKeltBreaking.MockLittleSlut".Localized());
            OutputText("Scenes.Kelly.SecondKeltBreaking.BastardAccusation".Localized());
            OutputText("Scenes.Kelly.SecondKeltBreaking.RemindOfWhorePast".Localized());
            OutputText("Scenes.Kelly.SecondKeltBreaking.SneakyPrickThreat".Localized());
            OutputText("Scenes.Kelly.SecondKeltBreaking.FightBegins".Localized());
            CombatScene.Start(EnemyRegistry.Create(new Identifier("coc", "kelt")), () => Go(SceneRegistry.CreateOrDefault("coc:forest")));
        }

        // Haxe DefeatKellyNDBREAKHIM()   outputText 42 
        public void DefeatKellyNDBREAKHIM()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Lust >= CombatController.Instance!.Context.Enemy.MaxLust)
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltMoansMantitsLust".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltGroansSlumpingDamage".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.CloseInEasyNow".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.RetreatThenTurnBack".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.WontLetHimSlip".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SpeedNoMatchSuffices".Localized());
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltRearsAndFidgets".Localized());
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.UnmountAndDragBarn".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeepUpAsCentaur".Localized());
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KickThenRopeLoop".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.KeltWalksMeeklyHauled".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.FunnelInMouth".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.ChokingInsteadOfReply".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.GirlBeautyPraise".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SobbingSlapWithCock".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.DemandAnswerKelly".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TearsAndCockslap".Localized());
            if (silly && AmilyFollowerCorrupt())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.AmilyHearsCumbuckets".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.ShakesHeadSlappingStops".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.NameKellyAccepted".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SmileGrabHerHead".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.PushHerUnderneath".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.PullHerClose".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.YourErectCocks".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TasteLessonThenWork".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.FasterThanLastTime".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.FondlesBallsWithCare".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.LipsSuckBetter".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.PullHeadToGroin".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.YouFragmentWord".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.CockHyphenSuffix".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.HalfheartedSlap".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.MinistrationsArouse".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.GenitalsEquineBack".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TitsInsteadOfThroat".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.ArticleAWord".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.QuickStepAnd".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.HeavyThrust".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.LaterFragmentWord".Localized());
            }
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.BreastValleyGrind".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.TitfuckFasterQuiver".Localized());
            OutputText("Scenes.Kelly.DefeatKellyNDBREAKHIM.SilentWalkAway".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 5f));
            if (HasPinkEgg())
            {
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.EggPink)))
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.EggPink));
                }
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.EggLargePink));
                }
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 5))
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 5);
                }
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 5);
                }
            }
            else
            {
                if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 10))
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 10);
                }
                else
                {
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 10);
                }
            }
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 5f));
            CoC.Instance.Flags.Set(VanillaFlags.KeltBreakLevel, 2);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe BreakingKeltNumeroThree()   outputText 46 
        public void BreakingKeltNumeroThree()
        {
            ClearOutput();

            if (CombatController.Instance!.Context.Enemy.Health < 1)
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SlumpingDown".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.MoaningAloud".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SubmitsWithoutFight".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ShameRuinedGirl".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ForceMouthBottlePour".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.GoSomeplacePrivate".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.FollowsMeekly".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.TwoMoreVials".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.AsksHerName".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.RemainsSilentStill".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SometimesWithCock".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.YellNameQuestion".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.GivesInAfterSlaps".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.StammersKellyName".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.WhatAreYouQuestion".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.CentauressAdmission".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.AsksHerPurpose".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SwearToSuckWhole".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LethiceImpatience".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ThinHandsFacefuck".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.TestsKellyOnceMore".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.PushFromBehind".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.PullWord".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.PinHeadThroatShove".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BetterStillNotEnough".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.QuietThenBegs".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.EffortNotGenuine".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.MoveCockHerFace".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BriskReach".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.DownPushingHer".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.GrabOneTit".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LustRisesConvulsively".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SecondBreastNeeded".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BucksAndWhinnies".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.KellyMoansErratic".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.FamiliarWarmthBuilds".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BallsChurn".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ClimaxApproaching".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SqueezeBoobsScream".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LetterY".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.LoudGroanCumJets".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.BreakingKeltNumeroThree.ApostropheBreasts".Localized());
            }
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.HeedlessOfHerPain".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.OrderToDrink".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.SlowObeysKegForced".Localized());
            OutputText("Scenes.Kelly.BreakingKeltNumeroThree.BreastsExpandArousal".Localized());
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 5))
            {
                Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 5);
            }
            else
            {
                Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.PurifiedSuccubiMilk), 5);
            }
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 5f));
            CoC.Instance.Flags.Set(VanillaFlags.KeltBreakLevel, 3);
            CombatScene.CleanupAfterCombat();
        }

        // Haxe FinalKeltBreaking()   outputText 43 
        public void FinalKeltBreaking()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.FinalKeltBreaking.ApproachFinalLesson".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.ReunionGreeting".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.CoyGrinQuestion".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.BegsForCum".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.TriumphantLaughter".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.TauntingBlowjobDemand".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.CriesInExcitement".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.KneelsDown".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.BendsOver".Localized());
            }
            OutputText("Scenes.Kelly.FinalKeltBreaking.EagerCockSucking".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.HandsCaressBalls".Localized());
            }
            OutputText("Scenes.Kelly.FinalKeltBreaking.InsatiableHunger".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.SavoringPleasure".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.HindlegHaunches".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.ButtWord".Localized());
            }
            OutputText("Scenes.Kelly.FinalKeltBreaking.RelentlessThroatfuck".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.BackAway".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.PushHerAway".Localized());
            }
            OutputText("Scenes.Kelly.FinalKeltBreaking.PullOutOfMouth".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.TitfuckBegins".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.GropeBountifulTits".Localized());
            }
            OutputText("Scenes.Kelly.FinalKeltBreaking.BreastValleyTightness".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.CumslutBaptism".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.DeclaresHerName".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.IdentityQuestion".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.DeclaresCumslutRole".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.PurposeQuestion".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.DeclaresPurpose".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.PraiseTakeItAll".Localized());
            if (silly)
            {
                OutputText("Scenes.Kelly.FinalKeltBreaking.CumCannonShout".Localized());
            }
            OutputText("Scenes.Kelly.FinalKeltBreaking.FamiliarPowerGathers".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.SpunkFountainErupts".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.CumflowEbbing".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.AppearanceUnchanged".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.OffersToLickClean".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.AwkwardAgreement".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.InstantObedience".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.FinalLessonSpeech".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.DeniesFormerSelf".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.AffirmsHerPlace".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.NeverReply".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.MasterPraise".Localized());
            OutputText("Scenes.Kelly.FinalKeltBreaking.DepartingButtPat".Localized());
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Corruption, 8f));
            CoC.Instance.Flags.Set(VanillaFlags.KeltBreakLevel, 4);
            ReturnToCamp(1);
        }

        // Haxe KeltFucksShitUp()   outputText 1 
        public void KeltFucksShitUp()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KeltFucksShitUp.DefeatedKeltTaunts".Localized());
            ClearButtons();
            AddButton(0, "Next", KeltFucksShitUpII);
        }

        // Haxe KeltFucksShitUpII()   outputText 2 
        public void KeltFucksShitUpII()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KeltFucksShitUpII.WakingAtFarmEdge".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KeltKillPlan) != 1)
            {
                OutputText("Scenes.Kelly.KeltFucksShitUpII.SuccubiMilkLoss".Localized());
                Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk), 5);
                // 原版 flags[KFLAGS.KELT_BREAK_LEVEL]--（Kelly.hx:622-637 的 keltFucksShitUpII）
                CoC.Instance.Flags.Set(VanillaFlags.KeltBreakLevel,
                    CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) - 1);
                if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) < 1)
                {
                    CoC.Instance.Flags.Set(VanillaFlags.KeltBreakLevel, 1);
                }
            }
            CombatScene.CleanupAfterCombat();
        }

        // Haxe KellyAppearance()   outputText 32 
        public void KellyAppearance()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KellyAppearance.KellyBodyOverview".Localized(
                CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor)
            ));
            OutputText("Scenes.Kelly.KellyAppearance.BraidLeverage".Localized(
                CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor)
            ));
            OutputText("Scenes.Kelly.KellyAppearance.SoftBreastPair".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) > 0)
            {
                OutputText("Scenes.Kelly.KellyAppearance.SecondTitRow".Localized());
            }
            if (KellyIsPregnant)
            {
                OutputText("Scenes.Kelly.KellyAppearance.LeakingMilkDrops".Localized());
            }
            OutputText("Scenes.Kelly.KellyAppearance.GenitalLeadIn".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyVaginallyFuckedCount) == 0)
            {
                OutputText("Scenes.Kelly.KellyAppearance.VirginDescriptor".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 0)
            {
                OutputText("Scenes.Kelly.KellyAppearance.PinkPussy".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyAppearance.BlackMareCunt".Localized());
            }
            OutputText("Scenes.Kelly.KellyAppearance.DrippingMoisture".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyHeatTime) > 0 && !KellyIsPregnant)
            {
                OutputText("Scenes.Kelly.KellyAppearance.SignsOfHeat".Localized());
            }
            OutputText("Scenes.Kelly.KellyAppearance.HumanlikeAsshole".Localized());
            var __sw0_0 = KellyPregnancyEvent;
            if (__sw0_0 == 1)
            {
                OutputText("Scenes.Kelly.KellyAppearance.SlightBellyBulge".Localized());
            }
            if (__sw0_0 == 2)
            {
                OutputText("Scenes.Kelly.KellyAppearance.ObviousPregnancy".Localized());
            }
            if (__sw0_0 == 3)
            {
                OutputText("Scenes.Kelly.KellyAppearance.OffspringStirring".Localized());
            }
            if (__sw0_0 == 4)
            {
                OutputText("Scenes.Kelly.KellyAppearance.BloatedWithLife".Localized());
            }
            if (NumKellyTattoos() > 0)
            {
                OutputText("Scenes.Kelly.KellyAppearance.ParagraphBreak".Localized());
                if (NumKellyTribalTats() == 4)
                {
                    OutputText("Scenes.Kelly.KellyAppearance.TattooCoverage".Localized());
                }
                else
                {
                    if (NumKellyTattoos() > 1)
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.TattooListHeading".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.TattooListLead".Localized());
                    }
                    if (KellyTattooFlagSet(VanillaFlags.KellyTattooCollarbone))
                    {
                        OutputText(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooCollarbone));
                    }
                    if (KellyTattooFlagSet(VanillaFlags.KellyTattooShoulders))
                    {
                        OutputText(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooShoulders));
                    }
                    if (KellyTattooFlagSet(VanillaFlags.KellyTattooLowerback))
                    {
                        OutputText(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooLowerback));
                    }
                    if (KellyTattooFlagSet(VanillaFlags.KellyTattooButt))
                    {
                        OutputText(CoC.Instance.Flags.Get(VanillaFlags.KellyTattooButt));
                    }
                }
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 0)
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) == 1)
                {
                    OutputText("Scenes.Kelly.KellyAppearance.OffspringPossessive".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyFirstKidGender) == 1)
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.MaleChild".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.FemaleChild".Localized());
                    }
                    OutputText("Scenes.Kelly.KellyAppearance.DistantChildField".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyFirstKidGender) == 1)
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.SonPlayingDistant".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.DaughterPlayingDistant".Localized());
                    }
                }
                else
                {
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) == 2)
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.ChildrenPlaySounds".Localized(
                            NumberUtils.Num2Text(CoC.Instance.Flags.Get(VanillaFlags.KellyKids))
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyAppearance.ChildrenPlayEchoes".Localized(
                            NumberUtils.Num2Text(CoC.Instance.Flags.Get(VanillaFlags.KellyKids))
                        ));
                    }
                }
            }
            ClearButtons();
            AddButton(0, "Next", ApproachKelly);
        }

        // Haxe ApproachKelly()   outputText 19 
        public void ApproachKelly()
        {
            ClearOutput();

            KellySprite();
            if (!KellyTattooFlagSet(VanillaFlags.KellyHairColor))
            {
                CoC.Instance.Flags.Set(VanillaFlags.KellyHairColor, "chestnut brown");
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyVaginallyFuckedCount) > 0 && CoC.Instance.Flags.Get(VanillaFlags.TimesPunishedKelly) == 0 && Random.Shared.Next((int)(3)) == 0 && Player.HasCock())
            {
                CoC.Instance.Flags.Set(VanillaFlags.KellyDisobeyingCounter, 0);
                PunishKelly();
                return ;
            }
            if (View.Time.Hours >= 9 && View.Time.Hours <= 11 && CoC.Instance.Flags.Get(VanillaFlags.KellyKids) >= 2)
            {
                OutputText("Scenes.Kelly.ApproachKelly.BarnShadowNursing".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 2)
                {
                    OutputText("Scenes.Kelly.ApproachKelly.YoungestChildrenWord".Localized());
                }
                OutputText("Scenes.Kelly.ApproachKelly.ChildrenWithWord".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) > 0)
                {
                    OutputText("Scenes.Kelly.ApproachKelly.TopRowWord".Localized());
                }
                OutputText("Scenes.Kelly.ApproachKelly.MilkLadenTits".Localized());
                OutputText("Scenes.Kelly.ApproachKelly.GrumpyChildComplaints".Localized());
            }
            else
            {
                if (View.Time.Hours >= 15 && View.Time.Hours <= 16 && CoC.Instance.Flags.Get(VanillaFlags.KellyKids) >= 4)
                {
                    OutputText("Scenes.Kelly.ApproachKelly.KellyInFieldCenter".Localized());
                    OutputText("Scenes.Kelly.ApproachKelly.ComeBackLater".Localized());
                    ReturnToCamp(1);
                    return ;
                }
                else
                {
                    OutputText("Scenes.Kelly.ApproachKelly.CanteringGreeting".Localized());
                }
            }
            ClearButtons();
            AddButton(0, "Appearance", KellyAppearance);
            if (Player.Lust >= 33)
            {
                AddButton(1, "Sex", KellySexMenu);
            }
            else
            {
                OutputText("Scenes.Kelly.ApproachKelly.NotArousedEnough".Localized());
                AddButton(1, "Sex", null);
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 0)
            {
                if (Player.HasItem(Identifier.Parse("coc:equinum")))
                {
                    OutputText("Scenes.Kelly.ApproachKelly.EquinumGiftOption".Localized());
                    AddButton(5, "Give Equinum", GiveKellyEquinum);
                }
                else
                {
                    OutputText("Scenes.Kelly.ApproachKelly.EquinumLockedOption".Localized());
                    AddButton(5, "Give Equinum", null);
                }
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
                {
                    if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk)))
                    {
                        OutputText("Scenes.Kelly.ApproachKelly.SuccubiMilkGift".Localized());
                        AddButton(5, "Give SucMilk", GiveKellySuccubiMilk);
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.ApproachKelly.SuccubiMilkLocked".Localized());
                        AddButton(5, "Give SucMilk", null);
                    }
                }
            }
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.CaninePepper)))
            {
                OutputText("Scenes.Kelly.ApproachKelly.CaninePepperGift".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) == 0)
                {
                    OutputText("Scenes.Kelly.ApproachKelly.UncertainTransformation".Localized());
                }
                OutputText("Scenes.Kelly.ApproachKelly.SentencePeriod".Localized());
                AddButton(6, "Give CanineP", GiveKellyAPepper);
            }
            else
            {
                AddButton(6, "Give CanineP", null);
            }
            var beingGood = true;
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyVaginallyFuckedCount) > 0 && CoC.Instance.Flags.Get(VanillaFlags.KellyDisobeyingCounter) >= 3 && Player.HasCock())
            {
                OutputText("Scenes.Kelly.ApproachKelly.PleasuringInYourAbsence".Localized());
                AddButton(7, "Punish", PunishKelly);
                beingGood = false;
            }
            else
            {
                AddButton(7, "Punish", null);
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesPunishedKelly) > 0 && CoC.Instance.Flags.Get(VanillaFlags.KellyRewardCooldown) == 0 && Random.Shared.Next((int)(3)) == 0 && beingGood)
            {
                OutputText("Scenes.Kelly.ApproachKelly.GoodSpiritsReward".Localized());
                AddButton(8, "Reward", RewardKelly);
            }
            else
            {
                AddButton(8, "Reward", null);
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 0)
            {
                AddButton(10, "Race", KellyRace);
            }
            CoC.Instance.Flags.Set(VanillaFlags.KellyDisobeyingCounter, 0);
            if (CoC.Instance.Flags.Get(VanillaFlags.FarmCorruptionStarted) == 0)
            {
                SetExitButton("Back");
            }
            else
            {
                AddButton(14, "Back", FarmCorruption.RootScene);
            }
        }

        // Haxe KellySexMenu()   outputText 3 
        public void KellySexMenu()
        {
            ClearOutput();

            ClearButtons();
            OutputText("Scenes.Kelly.KellySexMenu.SexMenuGreeting".Localized());
            if (Player.HasCock() && Player.Lust >= 33)
            {
                if (Player.HasCockThatFits(300) || CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
                {
                    if (KellyIsPregnant)
                    {
                        AddButton(0, "Preg Fuck", KellyPregSex);
                    }
                    else
                    {
                        if (!Player.IsTaur())
                        {
                            AddButton(0, "Fuck Cunt", FuckKellysCunt);
                        }
                        else
                        {
                            AddButton(0, "Fuck Cunt", TaurOnTaurSexKelly);
                        }
                    }
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyVaginallyFuckedCount) == 0)
                    {
                        AddButton(0, "VirginFuck", TakeKellysVirginity);
                    }
                    if (Player.CountCocksOfType("TENTACLE") >= 2)
                    {
                        AddButton(1, "TentaFuck", TentaFuckKelly);
                    }
                    else
                    {
                        AddButton(1, "TentaFuck", null);
                    }
                }
                else
                {
                    OutputText("Scenes.Kelly.KellySexMenu.TooBigForVagina".Localized());
                }
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) == 0 && Player.CockThatFits(18, "length") < 0 && !Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.KellySexMenu.TooBigForTits".Localized());
                }
                else
                {
                    if (!Player.IsTaur())
                    {
                        AddButton(2, "Titfuck", KellyTitJob);
                    }
                    else
                    {
                        AddButton(2, "Titfuck", null);
                    }
                }
                AddButton(3, "Blowjob", KellyBJsAhoy);
                AddButton(4, "Talk And HJ", TalkNHandToKelly);
            }
            AddButton(14, "Back", ApproachKelly);
        }

        // Haxe FuckKellysCunt()   outputText 38 
        public void FuckKellysCunt()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.KellyVaginallyFuckedCount, CoC.Instance.Flags.Get(VanillaFlags.KellyVaginallyFuckedCount) + 1);
            var x = Player.CockThatFits(300);
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.StripOutOfArmor".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.KellyAsksEagerly".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.GestureTurnAround".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.SmackHindquarters".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.MoanSwishTail".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.PinchNippleWarning".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.DeclareSatisfactory".Localized());
            if (Player.Tallness < 72)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.TurnForStool".Localized());
                if (Player.Tallness < 52)
                {
                    OutputText("Scenes.Kelly.FuckKellysCunt.DiminutiveWord".Localized());
                }
                OutputText("Scenes.Kelly.FuckKellysCunt.HeightHindersReach".Localized());
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.PressAgainstEntrance".Localized(
                Player.CockDescript(x),
                Player.CockHead(x)
            ));
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.OtherErection".Localized());
                if (Player.CockTotal() > 2)
                {
                    OutputText("Scenes.Kelly.FuckKellysCunt.HangSuffix".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.FuckKellysCunt.HangsWord".Localized());
                }
                OutputText("Scenes.Kelly.FuckKellysCunt.DrippingsAcrossBelly".Localized());
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.SlideInsideTunnel".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.FuckKellysCunt.MoanUnrepentant".Localized());
            if (Player.CockArea(x) >= 300)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.BottomOut".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.SlideDeepInside".Localized());
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.SmackAssWarmth".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.ImpaledMoan".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.FuckKellysCunt.LouderMoans".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.FamiliarThrob".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.RoilingBalls".Localized());
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.ClimaxApproaching".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) > 0)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.FragrantWord".Localized());
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.CumSoaksLegs".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.FuckKellysCunt.JismSprays".Localized());
            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.SoggySexToy".Localized());
            }
            if (Player.CumQ() >= 1200)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.SpunkRoundsBelly".Localized());
                if (Player.CumQ() >= 4000)
                {
                    OutputText("Scenes.Kelly.FuckKellysCunt.PregnantWithSpooge".Localized());
                }
                OutputText("Scenes.Kelly.FuckKellysCunt.SentencePeriod".Localized());
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.LayPantingOnBack".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.LustSpentPraise".Localized());
            if (!KellyIsPregnant)
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.BroodmareTransformation".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.FuckKellysCunt.KnitForKids".Localized());
            }
            OutputText("Scenes.Kelly.FuckKellysCunt.SentencePeriodAgain".Localized());
            OutputText("Scenes.Kelly.FuckKellysCunt.DismountWithSpring".Localized());
            Player.Orgasm("Dick");
            KellyPreggers();
            ReturnToCamp(1);
        }

        // Haxe TaurOnTaurSexKelly()   outputText 31 
        public void TaurOnTaurSexKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.KellyVaginallyFuckedCount, CoC.Instance.Flags.Get(VanillaFlags.KellyVaginallyFuckedCount) + 1);
            var x = Player.CockThatFits(300);
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.RemoveLowerArmor".Localized());
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.TeaseFromFront".Localized());
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.CommandLieDown".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SackToken".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.TaintWord".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PressIntoFace".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BallsTingle".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.LewdPopAway".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SLetter".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.GestureRiseAndTurn".Localized());
            if (Player.Tallness < 60)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.TooTallToStand".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.LiftTail".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.MountWithLurch".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PlungeDeepInside".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 0)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PinkPussy".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BlackHorseCunt".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SheathInCanal".Localized());
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.CantSeeHerFace".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SackTokenAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.HindlegsWord".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SplatterFreshWaves".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) > 0)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.MuskyWord".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.AssertControl".Localized());
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BegForFoal".Localized(
                Player.CockDescript(x),
                Player.CockHead(x)
            ));
            if (Player.CumQ() >= 1000)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.BellyGrowsRound".Localized());
                if (Player.CumQ() >= 4000)
                {
                    OutputText("Scenes.Kelly.TaurOnTaurSexKelly.OverfilledSpillage".Localized());
                }
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.PeriodSpace".Localized());
            }
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.DismountCumDump".Localized());
            OutputText("Scenes.Kelly.TaurOnTaurSexKelly.SlipInMud".Localized());
            if (Player.Corruption < 50)
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.HelpHerUp".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TaurOnTaurSexKelly.WalkAwaySnickering".Localized());
            }
            KellyPreggers();
            Player.Orgasm("Dick");
            ReturnToCamp(1);
        }

        // Haxe TentaFuckKelly()   outputText 18 
        public void TentaFuckKelly()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.TentaFuckKelly.RevealTentacleCocks".Localized());
            var one = -1;
            var two = -2;
            var three = -3;
            var four = -4;
            var five = -5;
            var temp = 0;
            while (temp < Player.Cocks.Count)
            {
                if (Player.Cocks[temp].Id == new Identifier("coc", "tentacle"))
                {
                    if (one < 0)
                    {
                        one = temp;
                    }
                    else
                    {
                        if (two < 0)
                        {
                            two = temp;
                        }
                        else
                        {
                            if (three < 0)
                            {
                                three = temp;
                            }
                            else
                            {
                                if (four < 0)
                                {
                                    four = temp;
                                }
                                else
                                {
                                    if (five < 0)
                                    {
                                        five = temp;
                                    }
                                }
                            }
                        }
                    }
                }
                temp += 1;
            }
            OutputText("Scenes.Kelly.TentaFuckKelly.ShoveIntoMouth".Localized(
                Player.CockDescript(one),
                Player.CockDescript(one)
            ));
            OutputText("Scenes.Kelly.TentaFuckKelly.SkilledMouthWork".Localized(
                Player.CockDescript(two),
                Player.CockDescript(two)
            ));
            OutputText("Scenes.Kelly.TentaFuckKelly.ReamBothSides".Localized(
                Player.CockDescript(one)
            ));
            if (three >= 0)
            {
                OutputText("Scenes.Kelly.TentaFuckKelly.TremblingDisplay".Localized(
                    Player.CockDescript(three),
                    Player.CockDescript(three)
                ));
            }
            if (four >= 0)
            {
                OutputText("Scenes.Kelly.TentaFuckKelly.TentacleTrembles".Localized(
                    Player.CockDescript(four),
                    Player.CockDescript(three),
                    Player.CockDescript(four)
                ));
            }
            if (five >= 0)
            {
                OutputText("Scenes.Kelly.TentaFuckKelly.TentacleFindsHand".Localized(
                    Player.CockDescript(five)
                ));
            }
            if (Player.CountCocksOfType("TENTACLE") > 5)
            {
                OutputText("Scenes.Kelly.TentaFuckKelly.WrapAroundTentacle".Localized());
            }
            OutputText("Scenes.Kelly.TentaFuckKelly.PressureBuilds".Localized(
                Player.CockDescript(one),
                Player.CockDescript(two)
            ));
            if (three >= 0)
            {
                OutputText("Scenes.Kelly.TentaFuckKelly.RemainingTentacle".Localized());
                if (four >= 0)
                {
                    OutputText("Scenes.Kelly.TentaFuckKelly.EruptSuffix".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.TentaFuckKelly.EruptWord".Localized());
                }
                OutputText("Scenes.Kelly.TentaFuckKelly.UnholyFountain".Localized());
            }
            OutputText("Scenes.Kelly.TentaFuckKelly.CumShowerEnds".Localized());
            OutputText("Scenes.Kelly.TentaFuckKelly.CumBathPraise".Localized());
            OutputText("Scenes.Kelly.TentaFuckKelly.WallowInPuddle".Localized());
            if (Player.Corruption < 33)
            {
                OutputText("Scenes.Kelly.TentaFuckKelly.DisgustedWatching".Localized());
            }
            OutputText("Scenes.Kelly.TentaFuckKelly.LeaveHerEnjoying".Localized());
            KellyPreggers();
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -3f));
            ReturnToCamp(1);
        }

        // Haxe GiveKellyEquinum()   outputText 7 
        public void GiveKellyEquinum()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.GiveKellyEquinum.WickedVialIdea".Localized());
            OutputText("Scenes.Kelly.GiveKellyEquinum.NervousQuestion".Localized());
            OutputText("Scenes.Kelly.GiveKellyEquinum.HushPetReassurance".Localized());
            OutputText("Scenes.Kelly.GiveKellyEquinum.DemureReply".Localized());
            OutputText("Scenes.Kelly.GiveKellyEquinum.SmackBodaciousRump".Localized());
            OutputText("Scenes.Kelly.GiveKellyEquinum.DisappointedMoan".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.KellyCuntType, 1);
            OutputText("Scenes.Kelly.GiveKellyEquinum.SoakingWetHorsecunt".Localized());
            Player.ConsumeItem(Identifier.Parse("coc:equinum"));
            ClearButtons();
            AddButton(0, "Next", ApproachKelly);
        }

        // Haxe GiveKellySuccubiMilk()   outputText 6 
        public void GiveKellySuccubiMilk()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.DangleSuccubiMilk".Localized());
            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.UncorkAndHand".Localized());
            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.ShrinkToPetite".Localized());
            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.ThankForTreat".Localized());
            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.WithholdPraise".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.KellyCuntType, 0);
            OutputText("Scenes.Kelly.GiveKellySuccubiMilk.HumanlikePussy".Localized());
            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.SuccubiMilk));
            ClearButtons();
            AddButton(0, "Next", ApproachKelly);
        }

        // Haxe PunishKelly()   outputText 15 
        public void PunishKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.TimesPunishedKelly, CoC.Instance.Flags.Get(VanillaFlags.TimesPunishedKelly) + 1);
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesPunishedKelly) == 1)
            {
                OutputText("Scenes.Kelly.PunishKelly.SilentAtHalt".Localized());
                OutputText("Scenes.Kelly.PunishKelly.TraceAndSpeak".Localized());
                OutputText("Scenes.Kelly.PunishKelly.MoanAsYouCircle".Localized());
                OutputText("Scenes.Kelly.PunishKelly.LaughWantonBitch".Localized());
                OutputText("Scenes.Kelly.PunishKelly.GaspingPlea".Localized());
                OutputText("Scenes.Kelly.PunishKelly.GentleGoOn".Localized());
                OutputText("Scenes.Kelly.PunishKelly.HesitantConfession".Localized());
                OutputText("Scenes.Kelly.PunishKelly.ReliefAtTruth".Localized());
                OutputText("Scenes.Kelly.PunishKelly.HumbleRequest".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.PunishKelly.GuiltOnHerFace".Localized());
                OutputText("Scenes.Kelly.PunishKelly.AskBadCumslut".Localized());
                OutputText("Scenes.Kelly.PunishKelly.WhisperedYes".Localized());
                OutputText("Scenes.Kelly.PunishKelly.AskNeedPunishing".Localized());
                OutputText("Scenes.Kelly.PunishKelly.LouderYes".Localized());
            }
            ClearButtons();
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenKellyForPunishment) > 0 && !TelAdreAllowedInto())
            {
                OutputText("Scenes.Kelly.PunishKelly.LackingRideIdea".Localized());
            }
            else
            {
                AddButton(1, "Ride", RideKellyForPunishment);
            }
            AddButton(0, "Rimjob", GetARimjobFromKelly);
            AddButton(14, "Back", ApproachKelly);
        }

        // Haxe GetARimjobFromKelly()   outputText 34 
        public void GetARimjobFromKelly()
        {
            ClearOutput();

            var x = Player.CockThatFits(300);
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            CoC.Instance.Flags.Set(VanillaFlags.TimesRimJobbedByKelly, CoC.Instance.Flags.Get(VanillaFlags.TimesRimJobbedByKelly) + 1);
            OutputText("Scenes.Kelly.GetARimjobFromKelly.PeelOffArmor".Localized());
            if (Player.HasTailInsteadOfLegs())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.SlitherWriggleUp".Localized());
            }
            else
            {
                if (Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.GestureGroundLieDown".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.GestureLieCarefully".Localized(
                        Player.CockDescript(x)
                    ));
                }
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.FlareAndPresent".Localized());
            OutputText("Scenes.Kelly.GetARimjobFromKelly.UnpracticedTeeth".Localized());
            if (Player.HasTailInsteadOfLegs())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.LeanBackSlowly".Localized());
            }
            else
            {
                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.PressHerHead".Localized());
                }
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.LaughAtTongue".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.SmirkGripHair".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.GyrateButt".Localized());
            }
            if (Player.HasVagina())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.ThinFingersBrush".Localized());
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.TongueProbesDeeper".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.GetARimjobFromKelly.BreatheGoodGirl".Localized());
            if (Player.CockTotal() > 1)
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.HardenSuffix".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.HardenWord".Localized());
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.BlissfulBulge".Localized());
            if (Player.IsNaga())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.BoneCrushingClench".Localized());
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.SighPraise".Localized());
            if (Player.Balls > 0)
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.AndBallsToken".Localized());
            }
            if (Player.HasVagina())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.AndPussyToken".Localized());
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.DeservedShamelessSlut".Localized());
            OutputText("Scenes.Kelly.GetARimjobFromKelly.HarshWordsSpur".Localized());
            if (Player.HasVagina())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.FingersFrigYou".Localized());
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.GruntingPeak".Localized(
                Player.CockDescript(x)
            ));
            if (Player.HasVagina())
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.TandemOrgasm".Localized());
            }
            OutputText("Scenes.Kelly.GetARimjobFromKelly.FinalThrusts".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesRimJobbedByKelly) == 1)
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.ShakeAtIgnorance".Localized());
                OutputText("Scenes.Kelly.GetARimjobFromKelly.WaveAtRivulets".Localized());
                if (Player.CumQ() >= 700)
                {
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.OozingLake".Localized());
                }
                OutputText("Scenes.Kelly.GetARimjobFromKelly.KellyPauses".Localized());
                if (Player.CumQ() >= 1000)
                {
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.SheStaysAwhile".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Kelly.GetARimjobFromKelly.KnowsTheDrill".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.CumQ() >= 1000)
                {
                    OutputText("Scenes.Kelly.GetARimjobFromKelly.SheStaysAwhileAgain".Localized());
                }
            }
            Player.Orgasm("Anal");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, 3f), new DynStat(VanillaStats.Corruption, 1f));
            ReturnToCamp(1);
        }

        // Haxe RideKellyForPunishment()   outputText 102 
        public void RideKellyForPunishment()
        {
            ClearOutput();

            var x = Player.CockThatFits(300);
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            CoC.Instance.Flags.Set(VanillaFlags.TimesRiddenKellyForPunishment, CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenKellyForPunishment) + 1);
            if (CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenKellyForPunishment) == 1)
            {
                OutputText("Scenes.Kelly.RideKellyForPunishment.StrokeKellySide".Localized());
                if (Player.Tallness < 78 && !Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.SwingOntoBack".Localized());
                    if (Player.HasTailInsteadOfLegs())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.IdealRidingHalf".Localized());
                    }
                    else
                    {
                        if (Player.IsDrider())
                        {
                            OutputText("Scenes.Kelly.RideKellyForPunishment.SpiderHalfCompromise".Localized());
                        }
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.LeanInClose".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusTitRows) > 0)
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.TopRowOfPrefix".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.BreastsWord".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.OrderRideToFarm".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyLactating) > 0)
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.ThinMilkArcs".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.RaggedBreathGasp".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShortTripSore".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ConsiderBeingRidden".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.UselessSteedWhisper".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyLactating) > 0)
                    {
                        if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusTitRows) > 0)
                        {
                            OutputText("Scenes.Kelly.RideKellyForPunishment.AllFourOfPrefix".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Kelly.RideKellyForPunishment.HLetter".Localized());
                        }
                        OutputText("Scenes.Kelly.RideKellyForPunishment.SwollenTeatsMilk".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ServeAnotherWay".Localized());
                    if (!Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.MarchGait".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.CanterGait".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.FollowsAfterPause".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveBehindBarn".Localized());
                }
                OutputText("Scenes.Kelly.RideKellyForPunishment.FindSmallerBarn".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.WideEyedPensiveness".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.AskRiddenTaboo".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.TabooAnswerSigh".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.SuggestOwnedHarness".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.AssInsultLoud".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.PlaceBitTeeth".Localized());
                if (Player.Tallness < 78 && !Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.AdmireBoundSlut".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.GiddyUpOnceAround".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.SetsOffCanter".Localized());
                    if (Player.IsBiped())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.FeetIntoStirrups".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyBreezeRide".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ReturnToCamp".Localized(
                        Player.CockDescript(x)
                    ));
                    OutputText("Scenes.Kelly.RideKellyForPunishment.FeveredFucking".Localized(
                        Player.CockDescript(x)
                    ));
                }
                else
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.AdmireBoundSlutAgain".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShowPonyOrder".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.LovelyDayBreeze".Localized());
                    if (!Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.ReturnToCampAgain".Localized(
                            Player.CockDescript(x)
                        ));
                        OutputText("Scenes.Kelly.RideKellyForPunishment.FeveredFuckingAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.ReturnToCampThird".Localized(
                            Player.CockDescript(x)
                        ));
                        OutputText("Scenes.Kelly.RideKellyForPunishment.WishLiveForever".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                }
                KellyPreggers();
                OutputText("Scenes.Kelly.RideKellyForPunishment.WithdrawCatchBreath".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.GoodLessonPant".Localized());
                OutputText("Scenes.Kelly.RideKellyForPunishment.LuckyToHaveYou".Localized());
            }
            else
            {
                if (CoC.Instance.Flags.Get(VanillaFlags.TimesRiddenKellyForPunishment) == 2)
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.StrideToStones".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.PleaseNoLesson".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShushAssCalm".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.FixOnMantle".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusTitRows) > 0)
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.JustAsWellFour".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.StepBackAdmire".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.AdorableButHidden".Localized());
                    if (Player.Tallness < 78 && !Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.HeadTowardsTown".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrival".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LearningLessonGuard".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreets".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.AskBeforeOwned".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.CosmeticsRibbonsBox".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLook".Localized(
                            Player.CockDescript(x)
                        ));
                        OutputText("Scenes.Kelly.RideKellyForPunishment.GoodGirlRibbon".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveTelAdreSaddled".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.GrabReignsStride".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrivalAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LearningLessonGuardAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreetsAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.AskBeforeOwnedAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.CosmeticsRibbonsBoxAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLookAgain".Localized(
                            Player.CockDescript(x)
                        ));
                        OutputText("Scenes.Kelly.RideKellyForPunishment.GoodGirlRibbonAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveTelAdreWalking".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.HumiliationWetGag".Localized());
                    if (!Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyBobbingHead".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyEncapsulatedMouth".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.PromptForThanks".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.DevotedThankYou".Localized());
                    KellyPreggers();
                }
                else
                {
                    OutputText("Scenes.Kelly.RideKellyForPunishment.StrideToStonesAgain".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.PleaseNoLessonAgain".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.ShushAssCalmAgain".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.DifficultAboutThis".Localized());
                    if (Player.Tallness < 78 && !Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.KnowWhereGoing".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrivalThird".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreetsThird".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.WipeTearsBlinkers".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.SexShopUnknownBox".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLookThird".Localized(
                            Player.CockDescript(x)
                        ));
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LoosenForFuck".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.BoughtBoxRibbon".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveSaddledAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.JerkReignsOrder".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.TelAdreArrivalFourth".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EveryoneStaresStreetsFourth".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.WipeTearsBlinkersAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.BoughtUnknownBoxAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.AlleyHardLookFourth".Localized(
                            Player.CockDescript(x)
                        ));
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LoosenForFuckAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.BoughtBoxRibbonAgain".Localized());
                        OutputText("Scenes.Kelly.RideKellyForPunishment.LeaveLeadingKelly".Localized());
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.HumiliationWetGagAgain".Localized());
                    if (!Player.IsTaur())
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyBobbingHeadAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.RideKellyForPunishment.EnjoyEncapsulatedMouthAgain".Localized(
                            Player.CockDescript(x)
                        ));
                    }
                    OutputText("Scenes.Kelly.RideKellyForPunishment.PromptForThanksAgain".Localized());
                    OutputText("Scenes.Kelly.RideKellyForPunishment.DevotedThankYouAgain".Localized());
                }
            }
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -1f), new DynStat(VanillaStats.Corruption, .5f));
            ReturnToCamp(1);
        }

        // Haxe TakeKellysVirginity()   outputText 44 
        public void TakeKellysVirginity()
        {
            ClearOutput();

            var x = Player.CockThatFits(300);
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.ClaimPrizePromise".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.HorsePrefix".Localized());
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.CuntWord".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.CentaurFidgetsNervously".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.MareSexToySpeech".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.LieDownCommand".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyLowersMeekly".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.LustSwollenFuckHole".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.BloomingPinkPussy".Localized());
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.GoodQuestion".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.KneelAndTest".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.EquineWord".Localized());
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.HymenVisibleStroke".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.SqueezeOnyxVulva".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.FondleSlickLabia".Localized());
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.MolestingEquestrianToy".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyMoansSensitive".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.FourLeggedSlutCries".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.AnswerWithSmack".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.ClaimMaidenhead".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.LineUpBehindHer".Localized(
                    Player.CockDescript(x)
                ));
            }
            else
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.SlideHindlegsBehind".Localized(
                    Player.CockDescript(x)
                ));
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.FirstContactThrill".Localized(
                Player.CockHead(x)
            ));
            OutputText("Scenes.Kelly.TakeKellysVirginity.SqueezeKelly".Localized());
            if (Player.IsTaur())
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.SnuggleBehindBreast".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.AssWord".Localized());
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.LaughThenSpear".Localized());
            if (Player.CockArea(x) <= 300)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.NestleSheathAgainst".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
                {
                    OutputText("Scenes.Kelly.TakeKellysVirginity.PlumpEquineLips".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.TakeKellysVirginity.SlipperyPinkLips".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.PressCervixBarrier".Localized(
                    Player.CockHead(x)
                ));
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.PinkishFrothOozes".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.PullTailAsk".Localized(
                Player.Mf("man", "breeder")
            ));
            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyEyesCross".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.TakeKellysVirginity.DefloweringMemoryFades".Localized());
            OutputText("Scenes.Kelly.TakeKellysVirginity.KellySquirmsShuddering".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.TakeKellysVirginity.GloryingRelease".Localized());
            if (Player.CumQ() >= 500)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.JismIntoWomb".Localized());
                if (Player.CumQ() >= 1500)
                {
                    OutputText("Scenes.Kelly.TakeKellysVirginity.BellyRoundsWithSpunk".Localized());
                }
                if (Player.CumQ() >= 3500)
                {
                    OutputText("Scenes.Kelly.TakeKellysVirginity.SeedFurther".Localized());
                }
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.PullOutSigh".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 0)
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.WellUsedRedness".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.TakeKellysVirginity.ShiningJuicesDrip".Localized());
            }
            OutputText("Scenes.Kelly.TakeKellysVirginity.KellyPassedOut".Localized());
            KellyPreggers();
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -3f), new DynStat(VanillaStats.Corruption, .5f));
            CoC.Instance.Flags.Set(VanillaFlags.KellyVaginallyFuckedCount, CoC.Instance.Flags.Get(VanillaFlags.KellyVaginallyFuckedCount) + 1);
            ReturnToCamp(1);
        }

        // Haxe GiveKellyAPepper()   outputText 10 
        public void GiveKellyAPepper()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.KellyTimesPeppered) == 0)
            {
                OutputText("Scenes.Kelly.GiveKellyAPepper.OpenWideInstruction".Localized());
                OutputText("Scenes.Kelly.GiveKellyAPepper.SpicyTasteMoan".Localized());
                OutputText("Scenes.Kelly.GiveKellyAPepper.FurtherPepperChanges".Localized());
                OutputText("Scenes.Kelly.GiveKellyAPepper.MasterPleaseMoan".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.KellyBonusBoobRows, 1);
                CoC.Instance.Flags.Set(VanillaFlags.KellyHeatTime, 48);
                KellySprite();
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAPepper.GrinDanglePepper".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) == 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAPepper.FleshBurgeonsMoan".Localized());
                    OutputText("Scenes.Kelly.GiveKellyAPepper.FurtherPepperChangesAgain".Localized());
                    OutputText("Scenes.Kelly.GiveKellyAPepper.MasterPleaseMoanAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAPepper.NoOutwardEffects".Localized());
                    OutputText("Scenes.Kelly.GiveKellyAPepper.FuckMeWhimper".Localized());
                }
                CoC.Instance.Flags.Set(VanillaFlags.KellyHeatTime, CoC.Instance.Flags.Get(VanillaFlags.KellyHeatTime) + 48);
            }
            CoC.Instance.Flags.Set(VanillaFlags.KellyTimesPeppered, CoC.Instance.Flags.Get(VanillaFlags.KellyTimesPeppered) + 1);
            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.CaninePepper));
            ReturnToCamp(1);
        }

        // Haxe KellyTitJob()   outputText 22 
        public void KellyTitJob()
        {
            ClearOutput();

            int x = 0;
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) == 0)
            {
                x = Player.CockThatFits(18, "length");
            }
            else
            {
                x = Player.BiggestCockIndex();
            }
            OutputText("Scenes.Kelly.KellyTitJob.BreastsBounceApproach".Localized());
            if (KellyIsPregnant)
            {
                OutputText("Scenes.Kelly.KellyTitJob.MilkBeadsFlicking".Localized());
            }
            OutputText("Scenes.Kelly.KellyTitJob.WeightReminderBody".Localized(
                Player.CockDescript(x)
            ));
            OutputText("Scenes.Kelly.KellyTitJob.MasterSeesSomething".Localized());
            OutputText("Scenes.Kelly.KellyTitJob.KneelGruntCommand".Localized(
                Player.CockDescript(x)
            ));
            if (Player.Cocks[x].Length <= 18)
            {
                OutputText("Scenes.Kelly.KellyTitJob.HoldShouldersMove".Localized(
                    Player.CockDescript(x)
                ));
                if (KellyIsPregnant)
                {
                    OutputText("Scenes.Kelly.KellyTitJob.MilkDrizzleTaste".Localized(
                        Player.CockDescript(x)
                    ));
                }
                OutputText("Scenes.Kelly.KellyTitJob.DeepConcentration".Localized());
                OutputText("Scenes.Kelly.KellyTitJob.RollsTongueMoan".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) > 0)
                {
                    OutputText("Scenes.Kelly.KellyTitJob.HipsPressLowerTits".Localized());
                }
                OutputText("Scenes.Kelly.KellyTitJob.TongueFlicksOverhead".Localized());
                if (Player.CumQ() <= 250)
                {
                    OutputText("Scenes.Kelly.KellyTitJob.AdmireHandiwork".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyTitJob.CoatWithPotency".Localized());
                }
            }
            else
            {
                OutputText("Scenes.Kelly.KellyTitJob.PrickDownTorso".Localized());
                OutputText("Scenes.Kelly.KellyTitJob.SaySlavesMust".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Kelly.KellyTitJob.SighClaspTopBreasts".Localized(
                    Player.CockDescript(x)
                ));
                OutputText("Scenes.Kelly.KellyTitJob.ExpertWristRoll".Localized());
                if (KellyIsPregnant)
                {
                    OutputText("Scenes.Kelly.KellyTitJob.MilkDrizzleTasteAgain".Localized(
                        Player.CockDescript(x)
                    ));
                }
                OutputText("Scenes.Kelly.KellyTitJob.DeepConcentrationAgain".Localized());
                OutputText("Scenes.Kelly.KellyTitJob.RollsTongueMoanAgain".Localized());
                if (Player.CumQ() < 250)
                {
                    OutputText("Scenes.Kelly.KellyTitJob.AdmireHandiworkAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyTitJob.CoatWithPotencyAgain".Localized());
                }
            }
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -3f));
            ReturnToCamp(1);
        }

        // Haxe KellyPreggers()   outputText 0 
        public void KellyPreggers()
        {

            if (KellyKnockUpChance(CoC.Instance.Flags.Get(VanillaFlags.KellyHeatTime) > 0 ? 15 : 1, 80))
            {
                KellyKnockUp(KellyPregnancyPlayerFather, VanillaPregnancy.CentaurIncubation - 84);
            }
        }

        // Haxe KellyPregSex()   outputText 7 
        public void KellyPregSex()
        {
            ClearOutput();

            int x = 0;
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyCuntType) == 1)
            {
                x = Player.BiggestCockIndex();
            }
            else
            {
                x = Player.CockThatFits(300);
            }
            if (x < 0)
            {
                x = Player.SmallestCockIndex();
            }
            OutputText("Scenes.Kelly.KellyPregSex.PregnancyScentPussy".Localized());
            OutputText("Scenes.Kelly.KellyPregSex.RemoveArmorCocksGrow".Localized());
            OutputText("Scenes.Kelly.KellyPregSex.CentauressScreamsArches".Localized());
            if (Player.Cocks[x].Id != VanillaBodyPartTypes.Horse)
            {
                OutputText("Scenes.Kelly.KellyPregSex.GetsYoursInstead".Localized());
            }
            OutputText("Scenes.Kelly.KellyPregSex.PoundScentedSlut".Localized());
            OutputText("Scenes.Kelly.KellyPregSex.BloatedBellySways".Localized());
            OutputText("Scenes.Kelly.KellyPregSex.GraspHipsContractions".Localized(
                Player.CockDescript(x)
            ));
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -4f));
            ReturnToCamp(1);
        }

        // Haxe KellyPopsOutARunt()   outputText 12 
        public void KellyPopsOutARunt()
        {

            OutputText("Scenes.Kelly.KellyPopsOutARunt.BarnVisitSurprise".Localized());
            OutputText("Scenes.Kelly.KellyPopsOutARunt.KellyHaystackNewborn".Localized());
            OutputText("Scenes.Kelly.KellyPopsOutARunt.SayHiToKid".Localized());
            var gender = Random.Shared.Next((int)(2)) + 1;
            OutputText("Scenes.Kelly.KellyPopsOutARunt.ApproachFoalView".Localized());
            if (gender == (int)Gender.Female)
            {
                OutputText("Scenes.Kelly.KellyPopsOutARunt.HerWord".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyPopsOutARunt.HisWord".Localized());
            }
            OutputText("Scenes.Kelly.KellyPopsOutARunt.MotherMilk".Localized());
            if (gender == (int)Gender.Female)
            {
                OutputText("Scenes.Kelly.KellyPopsOutARunt.CuteProtrusionsDevelop".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyPopsOutARunt.NoticeFoalNub".Localized());
            }
            OutputText("Scenes.Kelly.KellyPopsOutARunt.OffspringGrowStrong".Localized());
            if (Player.Corruption > 80)
            {
                OutputText("Scenes.Kelly.KellyPopsOutARunt.PicturingGrownKids".Localized());
            }
            OutputText("Scenes.Kelly.KellyPopsOutARunt.ParagraphBreak".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) == 0)
            {
                CoC.Instance.Flags.Set(VanillaFlags.KellyFirstKidGender, gender);
            }
            CoC.Instance.Flags.Set(VanillaFlags.KellyKids, CoC.Instance.Flags.Get(VanillaFlags.KellyKids) + 1);
            if (gender == (int)Gender.Male)
            {
                CoC.Instance.Flags.Set(VanillaFlags.KellyKidsMale, CoC.Instance.Flags.Get(VanillaFlags.KellyKidsMale) + 1);
            }
        }

        // Haxe TalkNHandToKelly()   outputText 77 
        public void TalkNHandToKelly()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.KellyTalkNHandTimes) == 0)
            {
                OutputText("Scenes.Kelly.TalkNHandToKelly.PurrsLeastInterestingThing".Localized());
                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.HelpsRemoveLowerArmor".Localized());
                    if (Player.CockTotal() == 1)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.GentlyTugsQuicklyHardens".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.TugsBothCocksHard".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.ClopsRearTowardKelly".Localized());
                    if (Player.CockTotal() == 1)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.WarmHandsAround".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.HandWrapsBoth".Localized());
                    }
                }
            }
            else
            {
                OutputText("Scenes.Kelly.TalkNHandToKelly.TellsHerRatherTalk".Localized());
                OutputText("Scenes.Kelly.TalkNHandToKelly.GrinningCentaurSlave".Localized());
                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.SlidesOffLowerArmor".Localized());
                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.LetterS".Localized());
                    }
                    OutputText("Scenes.Kelly.TalkNHandToKelly.SighsAsShePumps".Localized());
                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.CocksThey".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.CockIt".Localized());
                    }
                    OutputText("Scenes.Kelly.TalkNHandToKelly.RapidStiffening".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.TrotsBehindYou".Localized());
                    if (Player.CockTotal() == 1)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.WarmHandsAroundAgain".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.HandWrapsBothAgain".Localized());
                    }
                }
            }
            CoC.Instance.Flags.Set(VanillaFlags.KellyTalkNHandTimes, CoC.Instance.Flags.Get(VanillaFlags.KellyTalkNHandTimes) + 1);
            var temp = Random.Shared.Next((int)(5));
            if (temp == 0)
            {
                OutputText("Scenes.Kelly.TalkNHandToKelly.AsksAboutWives".Localized());
                OutputText("Scenes.Kelly.TalkNHandToKelly.KellyStopsPumping".Localized());
                if (!Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.NowWorksForMan".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.NowWorksForWoman".Localized());
                }
                OutputText("Scenes.Kelly.TalkNHandToKelly.LaughsMusically".Localized());
                if (Player.CockTotal() > 1)
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.ImaginesEveryCockHole".Localized());
                }
                else
                {
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.ImaginesTongueFucking".Localized());
                    }
                }
                OutputText("Scenes.Kelly.TalkNHandToKelly.SymphonyOfSqueals".Localized());
                OutputText("Scenes.Kelly.TalkNHandToKelly.ClearerBloodSettled".Localized());
                if (Player.Corruption < 70)
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.KeltVictimsUnsettle".Localized());
                }
                OutputText("Scenes.Kelly.TalkNHandToKelly.ThanksKellyButLeaves".Localized());
                OutputText("Scenes.Kelly.TalkNHandToKelly.ThanksMasterSoMuch".Localized());
            }
            else
            {
                if (temp == 1)
                {
                    OutputText("Scenes.Kelly.TalkNHandToKelly.AsksToMoveIn".Localized());
                    OutputText("Scenes.Kelly.TalkNHandToKelly.MoveInOnceRepaid".Localized());
                    OutputText("Scenes.Kelly.TalkNHandToKelly.AsksWhatPaidMeans".Localized());
                    OutputText("Scenes.Kelly.TalkNHandToKelly.TroubleBringingTrueSelf".Localized());
                    OutputText("Scenes.Kelly.TalkNHandToKelly.WhispersHandsSliding".Localized());
                    if (Player.CockTotal() > 1)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.SecondCockWord".Localized());
                    }
                    OutputText("Scenes.Kelly.TalkNHandToKelly.CannotRepayThoughtless".Localized());
                    OutputText("Scenes.Kelly.TalkNHandToKelly.EyesClosedEnjoyingWaves".Localized());
                    OutputText("Scenes.Kelly.TalkNHandToKelly.AdoringGreenEyes".Localized());
                }
                else
                {
                    if (temp == 2)
                    {
                        OutputText("Scenes.Kelly.TalkNHandToKelly.AsksCentaursSubDom".Localized());
                        OutputText("Scenes.Kelly.TalkNHandToKelly.NotLikeTheDemons".Localized());
                        if (!Player.IsTaur())
                        {
                            OutputText("Scenes.Kelly.TalkNHandToKelly.CumsIntoAir".Localized());
                        }
                        else
                        {
                            OutputText("Scenes.Kelly.TalkNHandToKelly.CumsOntoGround".Localized());
                        }
                        OutputText("Scenes.Kelly.TalkNHandToKelly.NuzzlesCocksWhenDone".Localized());
                        if (Player.CockTotal() > 1)
                        {
                            OutputText("Scenes.Kelly.TalkNHandToKelly.LetterSAgain".Localized());
                        }
                        OutputText("Scenes.Kelly.TalkNHandToKelly.SlapsAssAndLeaves".Localized());
                    }
                    else
                    {
                        if (temp == 3)
                        {
                            OutputText("Scenes.Kelly.TalkNHandToKelly.AsksAboutAura".Localized());
                            OutputText("Scenes.Kelly.TalkNHandToKelly.LongStoryMoveHands".Localized());
                            OutputText("Scenes.Kelly.TalkNHandToKelly.TracesBulgingVein".Localized());
                            OutputText("Scenes.Kelly.TalkNHandToKelly.PausesBeforeBending".Localized());
                            if (Player.Balls == 0)
                            {
                                OutputText("Scenes.Kelly.TalkNHandToKelly.SucklesBaseOfCock".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.TalkNHandToKelly.SucklesAtBalls".Localized());
                            }
                            OutputText("Scenes.Kelly.TalkNHandToKelly.BeautifulCoolingTactic".Localized());
                            OutputText("Scenes.Kelly.TalkNHandToKelly.ContinuesLongStory".Localized());
                            OutputText("Scenes.Kelly.TalkNHandToKelly.StrugglesToFocus".Localized());
                            OutputText("Scenes.Kelly.TalkNHandToKelly.SmackingLipsFedTwo".Localized());
                            if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 0)
                            {
                                OutputText("Scenes.Kelly.TalkNHandToKelly.BearingRaisingChildren".Localized());
                            }
                            OutputText("Scenes.Kelly.TalkNHandToKelly.HundredTimesHappier".Localized());
                            OutputText("Scenes.Kelly.TalkNHandToKelly.GlowOfGratitude".Localized());
                        }
                        else
                        {
                            if (!WhitneyCorrupt())
                            {
                                OutputText("Scenes.Kelly.TalkNHandToKelly.AsksAboutWhitney".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.HardNoteInVoice".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.ArchesNeckSensation".Localized());
                                if (!Player.IsTaur())
                                {
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsIntoAirAgain".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsOntoGroundAgain".Localized());
                                }
                                OutputText("Scenes.Kelly.TalkNHandToKelly.SentencePeriod".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.WantsHerGroveling".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.ExcusesHerselfBendsIn".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.TalkNHandToKelly.TrillsDrawingSyllable".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.RepliesStifflyDenial".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.CallsHimMeanImagines".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.KnowsWhatSheDoing".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.TensesAndCums".Localized());
                                if (!Player.IsTaur())
                                {
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsIntoAirThird".Localized());
                                }
                                else
                                {
                                    OutputText("Scenes.Kelly.TalkNHandToKelly.CumsOntoGroundThird".Localized());
                                }
                                OutputText("Scenes.Kelly.TalkNHandToKelly.WoggleCoilsThroughOrgasm".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.WhitneyIsRightHand".Localized());
                                OutputText("Scenes.Kelly.TalkNHandToKelly.InnocentlyAgreesRightHand".Localized());
                            }
                        }
                    }
                }
            }
            Player.Orgasm("Generic");
            ReturnToCamp(1);
        }

        // Haxe RewardKelly()   outputText 5 
        public void RewardKelly()
        {
            ClearOutput();

            CoC.Instance.Flags.Set(VanillaFlags.KellyRewardCooldown, 1);
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyTimesRewarded) == 0)
            {
                OutputText("Scenes.Kelly.RewardKelly.AddedBounceToCanter".Localized());
                OutputText("Scenes.Kelly.RewardKelly.ColorHighInCheeks".Localized());
                OutputText("Scenes.Kelly.RewardKelly.DifficultNotToLaugh".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.RewardKelly.HoldsHeadUpProudly".Localized());
                OutputText("Scenes.Kelly.RewardKelly.CheerfulHurray".Localized());
            }
            CoC.Instance.Flags.Set(VanillaFlags.KellyTimesRewarded, CoC.Instance.Flags.Get(VanillaFlags.KellyTimesRewarded) + 1);
            ClearButtons();
            if (Player.HasCockThatFits(300))
            {
                AddButton(0, "Applesauce", GiveKellyAppleSauce);
            }
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.HairDyeBlack)) && CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor) != "sable black")
            {
                AddButton(1, "Black Dye", () => DyeKellysBitchAssHair(VanillaConsumables.HairDyeBlack));
            }
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.HairDyeBlond)) && CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor) != "slutty blonde")
            {
                AddButton(2, "Blond Dye", () => DyeKellysBitchAssHair(VanillaConsumables.HairDyeBlond));
            }
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.HairDyePurple)) && CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor) != "garish purple")
            {
                AddButton(3, "Purple Dye", () => DyeKellysBitchAssHair(VanillaConsumables.HairDyePurple));
            }
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.HairDyePink)) && CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor) != "bright pink")
            {
                AddButton(4, "Pink Dye", () => DyeKellysBitchAssHair(VanillaConsumables.HairDyePink));
            }
            if (Player.HasItem(ItemRegistry.Create(VanillaConsumables.HairDyeBrown)) && CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor) != "chestnut brown")
            {
                AddButton(5, "Brown Dye", () => DyeKellysBitchAssHair(VanillaConsumables.HairDyeBrown));
            }
            AddButton(14, "Back", ApproachKelly);
        }

        // Haxe DyeKellysBitchAssHair()   outputText 18 
        public void DyeKellysBitchAssHair(Identifier color)
        {
            ClearOutput();

            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.BringsHairDyeGift".Localized());
            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.AgesSinceDidHair".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyTimesDiedHair) == 0)
            {
                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.NeverDoneHairBefore".Localized());
            }
            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.FunWalkToBarn".Localized());
            CoC.Instance.Flags.Set(VanillaFlags.KellyTimesDiedHair, CoC.Instance.Flags.Get(VanillaFlags.KellyTimesDiedHair) + 1);
            if (color == VanillaConsumables.HairDyeBlack)
            {
                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.WhipsJetBlackHair".Localized());
                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.SaysLooksSevere".Localized());
                CoC.Instance.Flags.Set(VanillaFlags.KellyHairColor, "sable black");
                Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.HairDyeBlack));
            }
            else
            {
                if (color == VanillaConsumables.HairDyeBlond)
                {
                    OutputText("Scenes.Kelly.DyeKellysBitchAssHair.WhipsFlaxenHair".Localized());
                    OutputText("Scenes.Kelly.DyeKellysBitchAssHair.FeelsVeryRight".Localized());
                    OutputText("Scenes.Kelly.DyeKellysBitchAssHair.PurrsRetyingBraid".Localized());
                    Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.HairDyeBlond));
                    CoC.Instance.Flags.Set(VanillaFlags.KellyHairColor, "slutty blonde");
                }
                else
                {
                    if (color == VanillaConsumables.HairDyePurple)
                    {
                        OutputText("Scenes.Kelly.DyeKellysBitchAssHair.WhipsPurpleHair".Localized());
                        OutputText("Scenes.Kelly.DyeKellysBitchAssHair.ServesAsReminder".Localized());
                        OutputText("Scenes.Kelly.DyeKellysBitchAssHair.HateToForgetReward".Localized());
                        CoC.Instance.Flags.Set(VanillaFlags.KellyHairColor, "garish purple");
                        Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.HairDyePurple));
                    }
                    else
                    {
                        if (color == VanillaConsumables.HairDyePink)
                        {
                            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.SmilesAtPinkHair".Localized());
                            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.AgreesFeelsGirly".Localized());
                            OutputText("Scenes.Kelly.DyeKellysBitchAssHair.ThanksForReward".Localized());
                            Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.HairDyePink));
                            CoC.Instance.Flags.Set(VanillaFlags.KellyHairColor, "bright pink");
                        }
                        else
                        {
                            if (color == VanillaConsumables.HairDyeBrown)
                            {
                                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.SighsTwistingLock".Localized());
                                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.OfCourseThankYou".Localized());
                                CoC.Instance.Flags.Set(VanillaFlags.KellyHairColor, "chestnut brown");
                                Player.ConsumeItem(ItemRegistry.Create(VanillaConsumables.HairDyeBrown));
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.DyeKellysBitchAssHair.BugReportPlaceholder".Localized()
                                    color
                                );
                            }
                        }
                    }
                }
            }
            ClearButtons();
            AddButton(0, "Next", ApproachKelly);
        }

        // Haxe GiveKellyAppleSauce()   outputText 74 
        public void GiveKellyAppleSauce()
        {
            ClearOutput();

            if (CoC.Instance.Flags.Get(VanillaFlags.KellyTimesApplesauced) == 0)
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.TapChinAskLikes".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.FurrowsBrowRecalling".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.JerkOffWellSpent".Localized());
                if (!WhitneyCorrupt())
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.WantsCheckOrchard".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.WantsCheckOrchardFretfully".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.DismissWhitneyAirily".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.CarefulPromisePlea".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WildAppleTrees".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.StoneMashApples".Localized());
                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.StripArmorStroke".Localized());
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasm".Localized());
                    }
                    if (Player.CumQ() >= 750)
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.BowlBrimsOver".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.PositionOverBowl".Localized());
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasmAgain".Localized());
                    }
                    if (Player.CumQ() >= 750)
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.CockTorrentsOut".Localized());
                    }
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.EnjoyAfterglowGlade".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.LaughAtHerWorry".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.BlowThousandTimes".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.FetchMetalBowl".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.GroveTransformed".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 1)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ClearedOblongPatch".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PickBestApples".Localized());
                if (!Player.IsTaur())
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.GraspCockReverie".Localized());
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasmOnce".Localized());
                    }
                    if (Player.CumQ() >= 750)
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.BowlBrimsOverAgain".Localized());
                    }
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.PositionOverBowlReverie".Localized());
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.VaginaDualOrgasmTwice".Localized());
                    }
                    if (Player.CumQ() >= 750)
                    {
                        OutputText("Scenes.Kelly.GiveKellyAppleSauce.CockTorrentsOutAgain".Localized());
                    }
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.EnjoyAfterglowGladeAgain".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.AsksFairGift".Localized());
                if (silly)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ParsleySageRosemary".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SlapAssPassing".Localized());
            }
            var x = Player.CockThatFits(300);
            var y = Player.CockThatFits2(300);
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SmellGetsHerWet".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SetHandsOnBowl".Localized(
                    Player.CockDescript(x),
                    CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor)
                ));
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.RadiatingHeatInside".Localized(
                    Player.CockHead(x),
                    Player.CockDescript(x)
                ));
                if (Player.Cocks[x].Thickness >= 4)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.GirthSpreadsTunnel".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.KellyGroansArching".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsToken".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ThighsWord".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WondersIfSheCame".Localized());
                if (y >= 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.EyesOnHerAsshole".Localized(
                        Player.CockDescript(x),
                        Player.CockDescript(y)
                    ));
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.WaitHeadThenThrust".Localized(
                        Player.CockDescript(x),
                        Player.CockDescript(y)
                    ));
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.MeasuredDriving".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.BraidLeverage".Localized(
                    CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor)
                ));
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsSlappingThighs".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PushTowardsReward".Localized(
                    Player.CockDescript(x)
                ));
                if (y >= 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.AnusClenchesAround".Localized(
                        Player.CockDescript(x)
                    ));
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WordlessShoutOrgasm".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.SmellWetsHerAgain".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.TutAndWithhold".Localized(
                    Player.CockDescript(x),
                    Player.CockHead(x)
                ));
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.HoldBackAtEntrance".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.Cocks[x].Thickness >= 4)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.GirthSpreadsTunnelAgain".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.KellyGroansArchingAgain".Localized(
                    Player.CockDescript(x)
                ));
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsTokenAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ThighsWordAgain".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WondersIfCameAgain".Localized());
                if (y >= 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.ThinkOfHerAsshole".Localized(
                        Player.CockDescript(x),
                        Player.CockDescript(y),
                        Player.CockDescript(x),
                        Player.CockDescript(y)
                    ));
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.MeasuredDrivingAgain".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.BraidForceBowl".Localized(
                    CoC.Instance.Flags.Get(VanillaFlags.KellyHairColor)
                ));
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.BallsSlappingThighsAgain".Localized());
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PushTowardsRewardAgain".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PantingKellyThrustsBack".Localized(
                    Player.CockDescript(x)
                ));
                if (y >= 0)
                {
                    OutputText("Scenes.Kelly.GiveKellyAppleSauce.AnusClenchesAroundAgain".Localized(
                        Player.CockDescript(y)
                    ));
                }
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WordlessShoutOrgasmAgain".Localized());
            }
            OutputText("Scenes.Kelly.GiveKellyAppleSauce.PoolOntoFloor".Localized());
            if (Player.IsBiped())
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.KneesGiving".Localized());
            }
            OutputText("Scenes.Kelly.GiveKellyAppleSauce.AchingCockClenching".Localized());
            OutputText("Scenes.Kelly.GiveKellyAppleSauce.BendHeadKiss".Localized());
            if (Player.Corruption < 60)
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.CumGlazedBreath".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.TastesPrettyGood".Localized());
            }
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyTimesApplesauced) == 0)
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.MurmursBestTreat".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.WatchAndDress".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.PurrsTastesBetter".Localized());
                OutputText("Scenes.Kelly.GiveKellyAppleSauce.DressAndAmbleBack".Localized());
            }
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -2f));
            CoC.Instance.Flags.Set(VanillaFlags.KellyTimesApplesauced, CoC.Instance.Flags.Get(VanillaFlags.KellyTimesApplesauced) + 1);
            ReturnToCamp(1);
        }

        // Haxe KellyBJsAhoy()   outputText 131 
        public void KellyBJsAhoy()
        {
            ClearOutput();

            OutputText("Scenes.Kelly.KellyBJsAhoy.StepInKissAnswer".Localized());
            OutputText("Scenes.Kelly.KellyBJsAhoy.AlreadyRagingHard".Localized());
            if (Player.CockTotal() == 1)
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.ItPronoun".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.ThemPronoun".Localized());
            }
            OutputText("Scenes.Kelly.KellyBJsAhoy.PullAwayTeaseLips".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrainOutOfArmor".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.TeaseHerFace".Localized());
                if (Player.Cocks[Player.BiggestCockIndex()].Id == VanillaBodyPartTypes.Horse)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.HorseCockSheath".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.TenderTortureGroan".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.BegToSwallow".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusedToneGoOn".Localized());
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SlowRollBackward".Localized());
                }
                if (Player.Balls > 0 && Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BrushBallsAside".Localized());
                    OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrust".Localized());
                    if (Player.BallSize <= 8)
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlEnvelop".Localized());
                        if (Player.Balls > 2)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourth".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.ThenBackFirst".Localized());
                        if (KellyIsPregnant)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenToken".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsBitch".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlAttempt".Localized());
                        if (Player.Balls > 2)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthAgain".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirBackFirst".Localized());
                        if (KellyIsPregnant)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLaden".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsBitchSpaced".Localized());
                        if (CoC.Instance.Flags.Get(VanillaFlags.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(VanillaFlags.DominikaStage) > 0)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuGrip".Localized());
                        }
                    }
                }
                else
                {
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TonguePressVagina".Localized());
                        OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrustAgain".Localized());
                    }
                    else
                    {
                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.TonguePressBalls".Localized());
                            if (Player.BallSize <= 8)
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlEnvelopBare".Localized());
                                if (Player.Balls >= 4)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthThird".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.ThenBackFirstAgain".Localized());
                                if (KellyIsPregnant)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenTokenAgain".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsBitchComma".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlAttemptBare".Localized());
                                if (Player.Balls >= 4)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthFourth".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirBackAgain".Localized());
                                if (KellyIsPregnant)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLadenAgain".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.PolishBallsSpaceBitch".Localized());
                                if (CoC.Instance.Flags.Get(VanillaFlags.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(VanillaFlags.DominikaStage) > 0)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuGripComma".Localized());
                                }
                            }
                        }
                    }
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.HeavyBreathLips".Localized());
                if (Player.BiggestCockLength() > 15)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.LongWayUpLift".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.ReachCockHeadCrown".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.LookDownBemused".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.GrinAtCumslut".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusementSoakPraise".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.BitByBitDeeper".Localized());
                if (Player.BiggestCockLength() > 12)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.CantTakeAllDick".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.MouthFitsThroat".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueUnderbellyStroke".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.BoobsHeldOut".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BallsClenchComma".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.PullOutGripBase".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.OntoAllFourTits".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.DeepReleaseEcstasy".Localized());
                if (Player.CumQ() >= 750)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BarelyControlCum".Localized());
                }
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.FemaleSexOrgasms".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.FinallySpent".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrainDownwardsFree".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.AdmireCentaurCock".Localized());
                if (Player.Cocks[Player.BiggestCockIndex()].Id == VanillaBodyPartTypes.Horse)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.WonderCentaurHorseCock".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.TenderTortureHear".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.BegSwallowInline".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.LetSecondPass".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusedToneGoAgain".Localized());
                if (Player.CockTotal() > 1 || Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SlowRollForward".Localized());
                }
                if (Player.HasVagina() && Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BrushBallsAsideAgain".Localized());
                    OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrustThird".Localized());
                    if (Player.BallSize <= 8)
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlEnvelopAgain".Localized());
                        if (Player.Balls >= 4)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthFifth".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.ThenBackFirstThird".Localized());
                        if (KellyIsPregnant)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLadenHyphen".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishCentaurBitchSpaced".Localized());
                    }
                    else
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TongueCurlAttemptAgain".Localized());
                        if (Player.Balls >= 4)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdFourthSixth".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirBackThird".Localized());
                        if (KellyIsPregnant)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.CommaMilkLadenThird".Localized());
                        }
                        OutputText("Scenes.Kelly.KellyBJsAhoy.PolishCentaurBitch".Localized());
                        if (CoC.Instance.Flags.Get(VanillaFlags.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(VanillaFlags.DominikaStage) > 0)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuGripThird".Localized());
                        }
                    }
                }
                else
                {
                    if (Player.HasVagina())
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.TonguePressVaginaAgain".Localized());
                        OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedThrustFourth".Localized());
                    }
                    else
                    {
                        if (Player.Balls > 0)
                        {
                            OutputText("Scenes.Kelly.KellyBJsAhoy.KellyHotTonguePresses".Localized());
                            if (Player.BallSize <= 8)
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.AfterCurlingTongueOrbs".Localized());
                                if (Player.Balls >= 4)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThenThirdThenFourth".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.BackToFirstOrb".Localized());
                                if (KellyIsPregnant)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenWord".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.BreastsPolishingContinues".Localized());
                            }
                            else
                            {
                                OutputText("Scenes.Kelly.KellyBJsAhoy.AfterCurlingTongueAttempts".Localized());
                                if (Player.Balls >= 4)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.ThirdThenFourthAgain".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.WarmAirPunctuated".Localized());
                                if (KellyIsPregnant)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.MilkLadenWordAgain".Localized());
                                }
                                OutputText("Scenes.Kelly.KellyBJsAhoy.BreastsPolishingConcludes".Localized());
                                if (CoC.Instance.Flags.Get(VanillaFlags.NumberOfTimesMetScylla) > 0 && CoC.Instance.Flags.Get(VanillaFlags.DominikaStage) > 0)
                                {
                                    OutputText("Scenes.Kelly.KellyBJsAhoy.DejaVuThrilling".Localized());
                                }
                            }
                        }
                    }
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.BreathingHeavilyLipsBase".Localized());
                if (Player.BiggestCockLength() > 15)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.LongWayDownDescent".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.ReachesCockHeadCrown".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.EyesClosedWorshipRhythm".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.EnoughKellySipsStream".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.AmusedTeaseLovelyBitch".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.MilkingRednessDeeper".Localized());
                if (Player.BiggestCockLength() > 12)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.CantTakeAllAgain".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.MouthGoodFitThroat".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.TongueForwardUnderbelly".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.HoldingBoobsOffered".Localized());
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BallsClenchWord".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.PullOutPointDown".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.GritCumAnnouncement".Localized());
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.PussyLickingWord".Localized());
                }
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.BallSuckingWord".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.CockAddictCatchLoad".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.CockSlitDilatesRopes".Localized());
                if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.OntoFourTitsAgain".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.DeepReleaseEcstatic".Localized());
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.FemaleSexClenchesSympathy".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.SpentClopAdmire".Localized());
                if (Player.CumQ() < 750)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SterlingJobFrostedBoobs".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.VirilityEcstaticTrance".Localized());
                    if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) > 0)
                    {
                        OutputText("Scenes.Kelly.KellyBJsAhoy.AllFourOfWord".Localized());
                    }
                    OutputText("Scenes.Kelly.KellyBJsAhoy.AlabasterOozeReservoir".Localized());
                }
            }
            OutputText("Scenes.Kelly.KellyBJsAhoy.ThanksForUsingMe".Localized());
            OutputText("Scenes.Kelly.KellyBJsAhoy.LaughSettleFacing".Localized());
            if (!Player.IsTaur())
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.ClutchingAchingPrick".Localized());
            }
            OutputText("Scenes.Kelly.KellyBJsAhoy.KnowsButPleasingFix".Localized());
            OutputText("Scenes.Kelly.KellyBJsAhoy.WatchKellyLicksClean".Localized());
            if (CoC.Instance.Flags.Get(VanillaFlags.KellyBonusBoobRows) > 0)
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.CantReachLowerTits".Localized());
            }
            OutputText("Scenes.Kelly.KellyBJsAhoy.TrailOfCumNipple".Localized());
            if (Player.Libido < 50)
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrokeLanguidHardAgain".Localized());
            }
            else
            {
                OutputText("Scenes.Kelly.KellyBJsAhoy.StrokeLanguidStraining".Localized());
                if (Player.HasVagina())
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.SensualLickVaginaGleam".Localized());
                }
                if (Player.Balls > 0)
                {
                    OutputText("Scenes.Kelly.KellyBJsAhoy.NeedilySuckleBalls".Localized());
                }
                OutputText("Scenes.Kelly.KellyBJsAhoy.EngulfCockPlumpLips".Localized());
                OutputText("Scenes.Kelly.KellyBJsAhoy.NeverDoubtGoodGirl".Localized());
            }
            Player.Orgasm("Dick");
            Player.ApplyStats(new DynStat(VanillaStats.Sensitivity, -2f));
            ReturnToCamp(1);
        }

        // Haxe KellyRace()   outputText 26 
        public void KellyRace()
        {
            ClearOutput();

            RegisterTag("kids", CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 1);
            RegisterTag("kidMale", CoC.Instance.Flags.Get(VanillaFlags.KellyFirstKidGender) == 1);
            RegisterTag("children", CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 1 ? "children" : (CoC.Instance.Flags.Get(VanillaFlags.KellyFirstKidGender) == 1 ? "son" : "daughter"));
            RegisterTag("kid ey", CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 1 ? "they" : (CoC.Instance.Flags.Get(VanillaFlags.KellyFirstKidGender) == 1 ? "he" : "she"));
            RegisterTag("kid em", CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 1 ? "them" : (CoC.Instance.Flags.Get(VanillaFlags.KellyFirstKidGender) == 1 ? "him" : "her"));
            RegisterTag("kid eir", CoC.Instance.Flags.Get(VanillaFlags.KellyKids) > 1 ? "their" : (CoC.Instance.Flags.Get(VanillaFlags.KellyFirstKidGender) == 1 ? "his" : "her"));
            OutputText("Scenes.Kelly.KellyRace.WalkOverToField".Localized(
                CoC.Instance.Flags.Get(VanillaFlags.KellyKids) == 2 ? "both" : "all " + NumberUtils.Num2Text(CoC.Instance.Flags.Get(VanillaFlags.KellyKids))
            ));
            OutputText("Scenes.Kelly.KellyRace.ChildrenCrowdGreeting".Localized());
            OutputText("Scenes.Kelly.KellyRace.SuggestARace".Localized());
            if (Player.Body.Get<LowerBodyPart>().LegCount == 1)
            {
                OutputText("Scenes.Kelly.KellyRace.LittleCentaurGiggles".Localized());
                OutputText("Scenes.Kelly.KellyRace.NeverUnderestimateSomeone".Localized());
            }
            else
            {
                if (Player.IsDrider())
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenNervouslyLook".Localized());
                    OutputText("Scenes.Kelly.KellyRace.DoYourBestRegardless".Localized());
                    OutputText("Scenes.Kelly.KellyRace.ChildrenNotConvinced".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenJumpExcitement".Localized());
                    OutputText("Scenes.Kelly.KellyRace.DragYouOffAnxious".Localized());
                }
            }
            OutputText("Scenes.Kelly.KellyRace.ExplainRulesWalking".Localized());
            OutputText("Scenes.Kelly.KellyRace.KidsClaimWinner".Localized());
            OutputText("Scenes.Kelly.KellyRace.KellyStartsRace".Localized());
            OutputText("Scenes.Kelly.KellyRace.CountdownGo".Localized());
            if (Player.Speed < 60)
            {
                OutputText("Scenes.Kelly.KellyRace.EarlyLeadPursuit".Localized());
                OutputText("Scenes.Kelly.KellyRace.SecondCornerAccept".Localized());
                OutputText("Scenes.Kelly.KellyRace.DaddySoSlowTaunt".Localized());
                OutputText("Scenes.Kelly.KellyRace.SufferingCeasesMerciful".Localized());
            }
            else
            {
                if (Player.Speed < 80)
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenBoundAhead".Localized());
                    OutputText("Scenes.Kelly.KellyRace.FocusOnGrassRhythm".Localized());
                    OutputText("Scenes.Kelly.KellyRace.AchingPropelsAcrossGrass".Localized());
                }
                else
                {
                    OutputText("Scenes.Kelly.KellyRace.ChildrenBoundBehind".Localized());
                    OutputText("Scenes.Kelly.KellyRace.FocusGrassInstead".Localized());
                    OutputText("Scenes.Kelly.KellyRace.EnjoyAirChase".Localized());
                }
            }
            OutputText("Scenes.Kelly.KellyRace.ChildrenPantingExhausted".Localized());
            OutputText("Scenes.Kelly.KellyRace.SayGoodbyesReturn".Localized());
            Player.ChangeFatigue(20);
            ReturnToCamp(1);
        }

    }
}
