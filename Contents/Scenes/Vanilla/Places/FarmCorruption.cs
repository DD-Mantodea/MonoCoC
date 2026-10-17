using MonoCoC.Contents.Stats;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Scenes.Vanilla.Places
{
    // 原版 classes/scenes/places/farm/FarmCorruption.hx 的移植（腐化农场子系统）。
    // 已移植：Whitney 腐化计数器与分段钳制、农场腐化完成判定、Whitney 支配态判定。
    // 原版 FarmCorruption.hx:538 接管事件与 FarmCorruption.hx:1047 腐化菜单仍待移植。
    public static class FarmCorruption
    {
        private static GameFlags F => CoC.Instance.Flags;

        // 原版 FarmCorruption.hx:50 whitneyCorruption(mod)：计数 + 分段上限钳制
        public static int whitneyCorruption(int mod = 0)
        {
            if (mod != 0)
            {
                F.Set(GameFlagIds.WhitneyCorruption, F.Get(GameFlagIds.WhitneyCorruption) + mod);

                if (F.Get(GameFlagIds.WhitneyCorruption) > F.Get(GameFlagIds.WhitneyCorruptionHighest))
                    F.Set(GameFlagIds.WhitneyCorruptionHighest, F.Get(GameFlagIds.WhitneyCorruption));
            }

            if (F.Get(GameFlagIds.FarmCorruptionApproachedWhitney) == 0 && F.Get(GameFlagIds.WhitneyCorruption) > 30)
                F.Set(GameFlagIds.WhitneyCorruption, 30);
            else if (F.Get(GameFlagIds.WhitneyLeave060) == 0 && F.Get(GameFlagIds.WhitneyCorruption) > 60)
                F.Set(GameFlagIds.WhitneyCorruption, 60);
            else if (F.Get(GameFlagIds.WhitneyLeave6190) == 0 && F.Get(GameFlagIds.WhitneyCorruption) > 90)
                F.Set(GameFlagIds.WhitneyCorruption, 90);
            else if (F.Get(GameFlagIds.WhitneyMenu91119) == 0 && F.Get(GameFlagIds.WhitneyCorruption) > 119)
                F.Set(GameFlagIds.WhitneyCorruption, 119);

            if (F.Get(GameFlagIds.WhitneyCorruption) < 0)
                F.Set(GameFlagIds.WhitneyCorruption, 0);

            if (F.Get(GameFlagIds.WhitneyCorruption) >= 120)
                F.Set(GameFlagIds.WhitneyCorruption, 120);

            return F.Get(GameFlagIds.WhitneyCorruption);
        }

        // 原版 FarmCorruption.hx:85 whitneyCorrupt()：农场已完全腐化
        public static bool whitneyCorrupt() => F.Get(GameFlagIds.WhitneyCorruptionComplete) > 0;

        // 原版 FarmCorruption.hx:whitneyDom()：Whitney 处于支配态
        public static bool whitneyDom() => F.Get(GameFlagIds.WhitneyDom) == 1;

        // 原版 FarmCorruption.hx:538 takeoverPrompt()：农场接管事件
        // 原版 FarmCorruption.hx:22 corruptFollowers()：腐化阵营的跟随者数量
        public static int corruptFollowers()
        {
            var count = 0;

            if (SceneInstance<Npcs.JojoScene>.Get().CampCorruptJojo()) count++;
            if (SceneInstance<Npcs.AmilyScene>.Get().AmilyCorrupt()) count++;
            if (SceneInstance<Npcs.IzmaScene>.Get().IzmaFollower()) count++;
            if (SceneInstance<Npcs.SophieBimboScene>.Get().BimboSophie()) count++;
            if (SceneInstance<Npcs.VapulaScene>.Get().VapulaSlave()) count++;
            if (SceneInstance<Npcs.CeraphFollowerScene>.Get().CeraphIsFollower()) count++;
            if (SceneInstance<Npcs.LatexGirlScene>.Get().LatexGooFollower()) count++;

            return count;
        }

        // 原版 CoC.hx:379 softLevelMin()：等级或天数任一到线即可
        public static bool softLevelMin(int level, int daysPerLevel = 6) =>

            CoC.Instance.Player.Level >= level || Scene.View.Time.Days >= level * daysPerLevel;

        // 原版 FarmCorruption.hx:538 takeoverPrompt()：农场接管事件
        public static bool takeoverPrompt()
        {
            if (F.Get(GameFlagIds.FarmCorruptionDisabled) == 1)
                return false;

            if (F.Get(GameFlagIds.FarmCorruptionStarted) == 1)
            {
                FarmMenu();

                return true;
            }

            if (!CoC.Instance.Player.IsCorruptEnough(70) || corruptFollowers() < 2 || !softLevelMin(12))
                return false;

            var today = Scene.View.Time.Days;

            if (F.Get(GameFlagIds.FarmCorruptPromptDisplay) != today)
            {
                Scene.ClearOutput();

                if (F.Get(GameFlagIds.KeltBreakLevel) >= 4)
                    TakeoverPromptKelly();
                else if (CoC.Instance.Player.HasStatusEffect(StatusEffectIds.MarbleRapeAttempted))
                    TakeoverPromptMarbleRape();
                else
                    TakeoverPromptGeneric();

                TakeoverPromptMerge(true);

                return true;
            }

            if (F.Get(GameFlagIds.FarmCorruptPromptDisplay) != today || F.Get(GameFlagIds.FarmDisabled) == 1)
            {
                Scene.ClearOutput();

                TakeoverPromptMerge();

                return true;
            }

            return false;
        }

        // 原版 FarmCorruption.hx:569 takeoverPromptKelly()
        public static void TakeoverPromptKelly()
        {
            Scene.OutputText("Scenes.FarmCorruption.Takeover.YouStandTheTop".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.NowYouFeelNothing".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.OnceAffectsHerWell".Localized());

            var amily = SceneInstance<Npcs.AmilyScene>.Get().AmilyCorrupt();

            var jojo = SceneInstance<Npcs.JojoScene>.Get().CampCorruptJojo();

            if (amily || jojo)
            {
                Scene.OutputText("Scenes.FarmCorruption.Takeover.AndTheRodentSlut".Localized());

                if (amily && jojo)
                    Scene.OutputText("Scenes.FarmCorruption.Takeover.Text".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Takeover.BeforeHim".Localized());
            }

            Scene.OutputText("Scenes.FarmCorruption.Takeover.2".Localized());
        }

        // 原版 FarmCorruption.hx:581 takeoverPromptMarbleRape()
        public static void TakeoverPromptMarbleRape()
        {
            Scene.OutputText("Scenes.FarmCorruption.Takeover.YouStandTheTopSmall".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.NowYouFeelNothingBut".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.DoesSheHaveAny".Localized());

            var amily = SceneInstance<Npcs.AmilyScene>.Get().AmilyCorrupt();

            var jojo = SceneInstance<Npcs.JojoScene>.Get().CampCorruptJojo();

            if (amily || jojo)
            {
                Scene.OutputText("Scenes.FarmCorruption.Takeover.AndTheRodentSlut".Localized());

                if (amily && jojo)
                    Scene.OutputText("Scenes.FarmCorruption.Takeover.3".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Takeover.BeforeHer".Localized());
            }

            Scene.OutputText("Scenes.FarmCorruption.Takeover.2".Localized());
        }

        // 原版 FarmCorruption.hx:590 takeoverPromptGeneric()
        public static void TakeoverPromptGeneric()
        {
            Scene.OutputText("Scenes.FarmCorruption.Takeover.Generic01".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.Generic02".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.Generic03".Localized());
        }

        // 原版 FarmCorruption.hx:597 takeoverPromptMerge(firstTime)
        public static void TakeoverPromptMerge(bool firstTime = false)
        {
            F.Set(GameFlagIds.FarmCorruptPromptDisplay, Scene.View.Time.Days);

            Scene.OutputText(firstTime

                ? "Scenes.FarmCorruption.Takeover.MergeFirstTime".Localized()

                : "Scenes.FarmCorruption.Takeover.MergeAgain".Localized());

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Now".Localized(), TakeoverPromptNow);

            Scene.AddButton(1, "Common.Button.Later".Localized(), TakeoverPromptLater);

            Scene.AddButton(2, "Scenes.Faerie.SexMenu.ButtonNever".Localized(), TakeoverPromptNever);
        }

        // 原版 FarmCorruption.hx:622 takeoverPromptNow()：当面摊牌，接管农场
        public static void TakeoverPromptNow()
        {
            var player = CoC.Instance.Player;

            var kelly = F.Get(GameFlagIds.KeltBreakLevel) >= 4;

            var marbleRaped = player.HasStatusEffect(StatusEffectIds.MarbleRapeAttempted);

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Takeover.YouStrideDownThe".Localized());

            // 原版 625：武器是否附着决定「卸下武器」的描述
            Scene.OutputText(player.Weapon.IsAttached()

                ? "Scenes.FarmCorruption.Takeover.AndDemonstrateYourPeacefulness".Localized()

                : "Scenes.FarmCorruption.Takeover.Continuation3".Localized());

            // 原版 631：Whitney 对 PC 的态度取决于是否已拿下 Kelly 或强暴过 Marble
            Scene.OutputText(kelly || marbleRaped

                ? "Scenes.FarmCorruption.Takeover.SheDoesntMoveShoo".Localized()

                : "Scenes.FarmCorruption.Takeover.SheLooksYouWith".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.AllWantTalkIve".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.ThereNothingDiscussShe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.ButThinkThereYou".Localized());

            // 原版 640：她抬出各自的「保镖」来压你
            if (kelly)
                Scene.OutputText("Scenes.FarmCorruption.Takeover.CuteButYouKnow".Localized());
            else if (marbleRaped)
                Scene.OutputText("Scenes.FarmCorruption.Takeover.CuteButYouKnowMarble".Localized());
            else
                Scene.OutputText("Scenes.FarmCorruption.Takeover.CuteButIveFought".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.WhatYouWantShe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.AllWantMaximizeThis".Localized());

            // 原版 651：她已被震慑，服软
            if (kelly || marbleRaped)
            {
                Scene.OutputText("Scenes.FarmCorruption.Takeover.YouHaveAdmitIts".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Takeover.AlrightAlrightMaybeYou".Localized());

                Scene.OutputText(kelly

                    ? "Scenes.FarmCorruption.Takeover.Kelt".Localized()

                    : "Scenes.FarmCorruption.Takeover.Marble".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Takeover.CannotProperlyProtectThe".Localized());
            }
            else
            {
                // 原版 662：Marble 还在农场时她会搬出 Marble
                if (!marbleRaped && !player.HasStatusEffect(StatusEffectIds.NoMoreMarble))
                {
                    Scene.OutputText("Scenes.FarmCorruption.Takeover.MarbleWillNotStand".Localized());

                    Scene.OutputText("Scenes.FarmCorruption.Takeover.MarbleCanFuckingSwivel".Localized());
                }

                // 原版 668：Kelt 还在时她会搬出 Kelt
                if (!kelly)
                {
                    Scene.OutputText("Scenes.FarmCorruption.Takeover.YouThinkKeltWill".Localized());

                    Scene.OutputText("Scenes.FarmCorruption.Takeover.KeltWillingTakeDirection".Localized());
                }

                Scene.OutputText("Scenes.FarmCorruption.Takeover.YouHaveAdmitItsGoing".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Takeover.AlrightAlrightMaybeYouGot".Localized());
            }

            Scene.OutputText("Scenes.FarmCorruption.Takeover.SmartDecisionIllSend".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.JustOneMoreThing".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Takeover.SheLooksYouShes".Localized());

            F.Set(GameFlagIds.FarmCorruptionStarted, 1);

            if (!player.HasStatusEffect(StatusEffectIds.NoMoreMarble))
                F.Set(GameFlagIds.FollowerAtFarmMarble, 0);

            Scene.ReturnToCamp(1);
        }

        // 原版 FarmCorruption.hx:685 takeoverPromptLater()
        public static void TakeoverPromptLater()
        {
            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Takeover.YouStareForMoment".Localized());

            FarmMenu();
        }

        // 原版 FarmCorruption.hx:691 takeoverPromptNever()
        public static void TakeoverPromptNever()
        {
            Scene.ClearOutput();

            F.Set(GameFlagIds.FarmCorruptionDisabled, 1);

            Scene.OutputText("Scenes.FarmCorruption.Takeover.Never01".Localized());

            CoC.Instance.Player.ApplyStats(new DynStatsOptions(Scale: false), new DynStat(GameStats.Corruption, -5f));

            Scene.ReturnToCamp(1);
        }

        // 兼容旧名（早期占位方法）
        public static bool TakeoverPrompt() => takeoverPrompt();

        // 原版 FarmCorruption.hx:rootScene()：腐化农场主菜单
        // 原版 FarmCorruption.hx:700 rootScene()：站在高坡上俯瞰农场 + 跟随者状态描述
        public static void RootScene()
        {
            Scene.ClearOutput();

            // 原版 FarmCorruption.hx:704：Whitney 被罚一天的次日，她会主动迎接
            if (F.Get(GameFlagIds.WhitneyDisabledForDay) == 2)
            {
                F.Set(GameFlagIds.WhitneyDisabledForDay, 3);

                Scene.OutputText("Scenes.FarmCorruption.RootScene.YouTurnAndWatch".Localized());

                Scene.OutputText("Scenes.FarmCorruption.RootScene.VeryGoodGirlYou".Localized());

                Scene.DoNext(DontDeFurDoge);

                return;
            }

            if (!whitneyCorrupt())
            {
                Scene.OutputText("Scenes.FarmCorruption.RootScene.YouStandTheRise".Localized());

                if (F.Get(GameFlagIds.FarmUpgradesRefinery) == 1)
                    Scene.OutputText("Scenes.FarmCorruption.RootScene.LargeMachineBulkyAnd".Localized());

                if (F.Get(GameFlagIds.FarmUpgradesContraceptive) == 1)
                    Scene.OutputText("Scenes.FarmCorruption.RootScene.NextThePepperPatch".Localized());

                if (F.Get(GameFlagIds.QueueOrgyroomUpgrade) > 1)
                    Scene.OutputText("Scenes.FarmCorruption.RootScene.LooksWhitneyHasBeen".Localized());
            }
            else
            {
                Scene.OutputText("Scenes.FarmCorruption.RootScene.YouStandYourRise".Localized());

                if (F.Get(GameFlagIds.FarmUpgradesRefinery) == 1)
                    Scene.OutputText("Scenes.FarmCorruption.RootScene.LargeMachineBulkyAndRotund".Localized());

                if (F.Get(GameFlagIds.FarmUpgradesContraceptive) == 1)
                    Scene.OutputText("Scenes.FarmCorruption.RootScene.NextThePepperPatchAnother".Localized());
            }

            // 原版 FarmCorruption.hx:744 起：在农场的跟随者 / 恋人
            if (F.Get(GameFlagIds.FollowerAtFarmCeraph) > 0)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.ThereIndefinableAuraCorruption".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmHolli) > 0)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.TheCropsAndGrass".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.AmilyThePepperPatch".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.YouCannotSeeJojo".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1
                && SceneInstance<Npcs.SophieBimboScene>.Get().BimboSophie())

                Scene.OutputText("Scenes.FarmCorruption.RootScene.YouCannotSeeSophie".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1
                && !SceneInstance<Npcs.SophieBimboScene>.Get().BimboSophie())

                Scene.OutputText("Scenes.FarmCorruption.RootScene.SophieHasPutTogether".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmIzma) == 1)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.IzmaSittingWhitneyOld".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmIsabella) == 1)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.IsabellaHaulingSteelCanisters".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.VapulaSlouchedAgainstBarn".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmLatexy) == 1)
                Scene.OutputText("Scenes.FarmCorruption.RootScene.YouCanSeeSomething".Localized());

            if (F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1)
                Scene.OutputText(F.Get(GameFlagIds.MilkSize) > 0

                    ? "Scenes.FarmCorruption.RootScene.RatherPredictablyTheCow".Localized()

                    : "Scenes.FarmCorruption.RootScene.SatTheEdgeHer".Localized());

            FarmMenu();
        }

        // 原版 FarmCorruption.hx:806 farmMenu()：农场主菜单
        public static void FarmMenu()
        {
            Scene.ClearButtons();

            var player = CoC.Instance.Player;

            var farm = SceneInstance<FarmScene>.Get();

            if (F.Get(GameFlagIds.WhitneyDisabledForDay) != 1)
                Scene.AddButton(0, "Common.Button.Whitney".Localized(), whiteneyMenuEntry);

            // 原版：Marble 尚未被拒绝过才出现
            if (!player.HasStatusEffect(StatusEffectIds.MarbleRapeAttempted)
                && !player.HasStatusEffect(StatusEffectIds.NoMoreMarble))

                Scene.AddButton(1, "Common.Button.Marble".Localized(), farm.MeetMarble);

            if (player.HasKeyItem("Breast Milker - Installed At Whitney's Farm"))
            {
                if (player.HasStatusEffect(StatusEffectIds.Milked))
                    Scene.OutputText("Scenes.FarmCorruption.FarmMenu.NipplesTooSore".Localized());

                Scene.AddButton(3, "Common.Button.GetMilked".Localized(), farm.GetMilked);
            }

            if (player.HasKeyItem("Cock Milker - Installed At Whitney's Farm") && player.CockTotal() > 0)
                Scene.AddButton(4, "Common.Button.MilkCock".Localized(), farm.CockPumping);

            Scene.AddButton(5, "Common.Button.Farm".Localized(), CorruptingTheFarmExplore);
            // 原版 FarmCorruption.hx:876-880：奴隶 / 恋人 / 普通跟随者三个子菜单
            if (SlavesAtFarm())
                Scene.AddButton(6, "Common.Button.Slaves".Localized(), SlavesAtFarmMenu);

            if (LoversAtFarm())
                Scene.AddButton(7, "Common.Button.Lovers".Localized(), LoversAtFarmMenu);

            if (FollowersAtFarm())
                Scene.AddButton(8, "Common.Button.Followers".Localized(), FollowersAtFarmMenu);

            Scene.AddButton(14, "Common.Button.Leave".Localized(), () => Scene.ReturnToCamp(1));
        }

        private static void whiteneyMenuEntry()
        {
            if (!whitneyCorrupt())
                Scene.Go<FarmScene>(); // 未腐化时由农场场景承载 Whitney 对话（原版 dogeNotCorruptYet）
            else
                Scene.Go<FarmScene>();
        }

        // 原版 FarmCorruption.hx:869 corruptingTheFarmExplore()
        public static void CorruptingTheFarmExplore()
        {
            Scene.ClearButtons();

            var farm = SceneInstance<FarmScene>.Get();

            Scene.AddButton(0, "Common.Button.Explore".Localized(), farm.ExploreFarm);

            Scene.AddButton(1, "Common.Button.Work".Localized(), farm.WorkFarm);

            if (CollectionAvailable())
                Scene.AddButton(2, "Common.Button.Collect".Localized(), CollectTheGoodies);

            if (F.Get(GameFlagIds.FarmStables) > 0)
                Scene.AddButton(3, "Common.Button.Stables".Localized(), farm.StablesMenu);

            Scene.AddButton(14, "Common.Button.Back".Localized(), FarmMenu);
        }

        // 原版 FarmCorruption.hx:333 collectTheGoodies()：结算农场收益与存放的货物
        public static void CollectTheGoodies()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            if (F.Get(GameFlagIds.FarmCorruptionDaysSinceLastPayout) >= 7)
            {
                Scene.OutputText("Scenes.FarmCorruption.Collect.YouStrollOverThe".Localized());

                // 原版 FarmCorruption.hx:341：农场估值低于 25 时提示「这点收成太少」
                if (FarmValue() < 25)
                    Scene.OutputText("Scenes.FarmCorruption.Collect.Gems".Localized());

                player.Gems += F.Get(GameFlagIds.FarmCorruptionGemsWaiting);

                F.Set(GameFlagIds.FarmCorruptionGemsWaiting, 0);

                F.Set(GameFlagIds.FarmCorruptionDaysSinceLastPayout, 0);
            }

            var succu = F.Get(GameFlagIds.FarmSuccumilkStored);

            var incu = F.Get(GameFlagIds.FarmIncudraftStored);

            var eggs = F.Get(GameFlagIds.FarmEggStored);

            var contra = F.Get(GameFlagIds.FarmContraceptiveStored);

            if (succu > 0 || incu > 0 || eggs > 0 || contra > 0)
                Scene.OutputText("Scenes.FarmCorruption.Collect.YouFrownSeemsLike".Localized());

            Scene.ClearButtons();

            if (succu > 0)
            {
                Scene.OutputText("Common.Text.BX".Localized(succu) +

                    (succu == 1 ? "Scenes.FarmCorruption.Collect.BoldOpen".Localized() : "Scenes.FarmCorruption.Collect.CountSuffix".Localized()) +

                    "</b>");

                Scene.AddButton(0, ItemShortName(ConsumableIds.SuccubiMilk), () => TakeStoredItems(GameFlagIds.FarmSuccumilkStored));
            }

            if (incu > 0)
            {
                Scene.OutputText("Common.Text.BX".Localized(incu) +

                    (incu == 1 ? "Scenes.FarmCorruption.Collect.BoldClose".Localized() : "Scenes.FarmCorruption.Collect.BoldOpen".Localized()) +

                    "</b>");

                Scene.AddButton(1, ItemShortName(ConsumableIds.IncubiDraft), () => TakeStoredItems(GameFlagIds.FarmIncudraftStored));
            }

            if (eggs > 0)
            {
                Scene.OutputText("Common.Text.BX".Localized(eggs) + EggIdentifier().ToString() + (eggs > 1 ? "Scenes.FarmCorruption.Collect.BoldClose".Localized() : "") + "Scenes.FarmCorruption.Collect.BoldOpen".Localized() + "</b>");

                Scene.AddButton(2, ItemShortName(EggIdentifier()), () => TakeStoredItems(GameFlagIds.FarmEggStored));
            }

            if (contra > 0)
            {
                Scene.OutputText("Common.Text.BX".Localized(contra) + "Scenes.FarmCorruption.Collect.Egg".Localized() + "</b>");

                Scene.AddButton(3, ItemShortName(ContraceptiveIdentifier), () => TakeStoredItems(GameFlagIds.FarmContraceptiveStored));
            }

            Scene.AddButton(14, "Common.Button.Back".Localized(), RootScene);
        }

        // ===== 原版 FarmCorruption.hx:395 getItemObj / takeItems / afterTakeItems =====

        /// <summary>原版 FARM_CONTRACEPTIVE_STORED 对应的物品。</summary>
        public static readonly MonoCoC.Registry.Identifier ContraceptiveIdentifier = new("coc", "herbal_contraceptive");

        /// <summary>原版 FarmCorruption.hx:395 getItemObj()：按存放 flag 取物品。</summary>
        public static MonoCoC.Registry.Identifier StoredItemIdentifier(Flag<int> flag)
        {
            if (flag.Id.Equals(GameFlagIds.FarmSuccumilkStored.Id)) return ConsumableIds.SuccubiMilk;
            if (flag.Id.Equals(GameFlagIds.FarmIncudraftStored.Id)) return ConsumableIds.IncubiDraft;
            if (flag.Id.Equals(GameFlagIds.FarmEggStored.Id)) return EggIdentifier();
            if (flag.Id.Equals(GameFlagIds.FarmContraceptiveStored.Id)) return ContraceptiveIdentifier;

            throw new ArgumentOutOfRangeException(nameof(flag));
        }

        /// <summary>原版 FarmCorruption.hx:409 takeItems()：取走一个并刷新界面。</summary>
        public static void TakeStoredItems(Flag<int> flag)
        {
            // 鸡蛋取走后重置产蛋冷却（原版 :411）
            if (flag.Id.Equals(GameFlagIds.FarmEggStored.Id))
                F.Set(GameFlagIds.FarmEggCountdown, 7);

            F.Set(flag, F.Get(flag) - 1);

            InventoryScene.TakeItem(
                Contents.Items.ItemRegistry.Create(StoredItemIdentifier(flag)),

                AfterTakeItems);
        }

        /// <summary>原版 FarmCorruption.hx:419 afterTakeItems()</summary>
        public static void AfterTakeItems()
        {
            if (CollectionAvailable())
                CollectTheGoodies();
            else
                RootScene();
        }

        /// <summary>原版 Sophie 产蛋颜色 → 对应蛋物品。</summary>
        public static MonoCoC.Registry.Identifier EggIdentifier() =>

            F.Get(GameFlagIds.FollowerProductionSophieColorchoice) switch
            {
                "black" => ConsumableIds.EggBlack,

                "blue" => ConsumableIds.EggBlue,

                "pink" => ConsumableIds.EggPink,

                "purple" => ConsumableIds.EggPurple,

                _ => ConsumableIds.EggBrown,
            };

        /// <summary>物品短名（用于按钮文字）。</summary>
        public static string ItemShortName(MonoCoC.Registry.Identifier id) =>

            Contents.Items.ItemRegistry.Create(id).Name;

        // 原版 FarmCorruption.hx:891 collectionAvailable()
        public static bool CollectionAvailable() =>

            F.Get(GameFlagIds.FarmCorruptionDaysSinceLastPayout) >= 7

            || F.Get(GameFlagIds.FarmSuccumilkStored) > 0

            || F.Get(GameFlagIds.FarmIncudraftStored) > 0

            || F.Get(GameFlagIds.FarmEggStored) > 0

            || F.Get(GameFlagIds.FarmContraceptiveStored) > 0;

        // 原版 FarmCorruption.hx:480 farmValue()：按在场跟随者给农场估值
        public static int FarmValue()
        {
            var value = 0;

            if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1) value += 3;
            if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1) value += 2;

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1)
                value += SceneInstance<Npcs.SophieBimboScene>.Get().BimboSophie() ? 4 : 3;

            if (F.Get(GameFlagIds.FollowerAtFarmIzma) == 1) value += 1;
            if (F.Get(GameFlagIds.FollowerAtFarmIsabella) == 1) value += 3;
            if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1) value += 1;
            if (F.Get(GameFlagIds.FollowerAtFarmLatexy) == 1) value += 2;

            if (F.Get(GameFlagIds.FollowerAtFarmKelly) == 1 && F.Get(GameFlagIds.KeltBreakLevel) >= 4)
                value += 1;

            if (F.Get(GameFlagIds.FollowerAtFarmMarble) == 1) value += 2;

            if (F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1)
                value += F.Get(GameFlagIds.MilkSize) > 0 ? 3 : 7;

            // 原版 :531：惠特尼腐化完成后再乘 1.25
            if (F.Get(GameFlagIds.WhitneyCorruptionComplete) == 1)
                value = (int)(value * 1.25f);

            return value;
        }

        // 原版 FarmCorruption.hx:707 之后的分支（Whitney 主动迎接）
        public static void DontDeFurDoge() => Scene.Go<FarmScene>();

        // ===== 原版 FarmCorruption.hx:3234 tattooFlagMap =====
        // 每个 NPC 的 4 个纹身槽位：collarbone / lowerback / shoulders / butt
        private static Flag<string>[] TattooSlots(string name) => name switch
        {
            "whitney" => [GameFlagIds.WhitneyTattooCollarbone, GameFlagIds.WhitneyTattooLowerback, GameFlagIds.WhitneyTattooShoulders, GameFlagIds.WhitneyTattooButt],

            "amily" => [GameFlagIds.AmilyTattooCollarbone, GameFlagIds.AmilyTattooLowerback, GameFlagIds.AmilyTattooShoulders, GameFlagIds.AmilyTattooButt],

            "jojo" => [GameFlagIds.JojoTattooCollarbone, GameFlagIds.JojoTattooLowerback, GameFlagIds.JojoTattooShoulders, GameFlagIds.JojoTattooButt],

            "sophie" => [GameFlagIds.SophieTattooCollarbone, GameFlagIds.SophieTattooLowerback, GameFlagIds.SophieTattooShoulders, GameFlagIds.SophieTattooButt],

            "vapula" => [GameFlagIds.VapulaTattooCollarbone, GameFlagIds.VapulaTattooLowerback, GameFlagIds.VapulaTattooShoulders, GameFlagIds.VapulaTattooButt],

            "kelly" => [GameFlagIds.KellyTattooCollarbone, GameFlagIds.KellyTattooLowerback, GameFlagIds.KellyTattooShoulders, GameFlagIds.KellyTattooButt],

            "milky" => [GameFlagIds.MilkyTattooCollarbone, GameFlagIds.MilkyTattooLowerback, GameFlagIds.MilkyTattooShoulders, GameFlagIds.MilkyTattooButt],

            _ => throw new ArgumentOutOfRangeException(nameof(name), name, "Unable to determine correct NPC flags."),
        };

        /// <summary>原版 FarmCorruption.hx:3254 hasTattoo(name)：该 NPC 是否有任意一处纹身。</summary>
        public static bool hasTattoo(string name) => TattooSlots(name).Any(F.Has);

        /// <summary>原版 FarmCorruption.hx:3265 numTattoos(name)：该 NPC 已纹的身位数量。</summary>
        public static int numTattoos(string name) => TattooSlots(name).Count(F.Has);

        /// <summary>原版 FarmCorruption.hx:3241 hasFreeTattooSlot(name)：是否还有空槽位。</summary>
        public static bool hasFreeTattooSlot(string name) => TattooSlots(name).Any(f => !F.Has(f));

        // 原版 FarmCorruption.hx:5141 等：按纹身文本内容计数（如 "A tribal tattoo" / "A butterfly"）
        private static int numTattoosMatching(string name, string pattern) =>

            TattooSlots(name).Count(f => F.Has(f) && (F.Get(f) ?? string.Empty).Contains(pattern));

        public static int numAmilyTribalTats() => numTattoosMatching("amily", "A tribal tattoo");

        public static int numSophieTribalTats() => numTattoosMatching("sophie", "A tribal tattoo");

        public static int numVapulaTribalTats() => numTattoosMatching("vapula", "A tribal tattoo");

        public static int numMilkyTribalTats() => numTattoosMatching("milky", "A tribal tattoo");

        public static int numMilkyButterflyTats() => numTattoosMatching("milky", "A butterfly");

        // 原版 FarmCorruption.hx:5198/5262/5358 等：4 个槽位全是同款纹身即为「全套」
        public static bool amilyFullTribalTats() => numAmilyTribalTats() == 4;

        public static bool milkyFullTribalTats() => numMilkyTribalTats() == 4;

        public static bool sophieFullTribalTats() => numSophieTribalTats() == 4;

        public static bool vapulaFullTribalTats() => numVapulaTribalTats() == 4;

        // 原版 :5166/:5205/:5301：其余三个角色的同款纹身统计
        public static int numWhitneyTribalTats() => numTattoosMatching("whitney", "A tribal tattoo");

        public static bool whitneyFullTribalTats() => numWhitneyTribalTats() == 4;

        public static int numJojoTribalTats() => numTattoosMatching("jojo", "A tribal tattoo");

        public static bool jojoFullTribalTats() => numJojoTribalTats() == 4;

        public static int numKellyTribalTats() => numTattoosMatching("kelly", "A tribal tattoo");

        public static bool kellyFullTribalTats() => numKellyTribalTats() == 4;

        // ===== 原版 FarmCorruption.hx:914-1046：农场的三类成员子菜单 =====

        /// <summary>原版 FarmCorruption.hx:914 numSlavesAtFarm()</summary>
        public static int NumSlavesAtFarm()
        {
            var count = 0;

            if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1) count++;
            if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1) count++;
            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && BimboSophie()) count++;
            if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1) count++;
            if (F.Get(GameFlagIds.FollowerAtFarmLatexy) == 1) count++;
            if (F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1) count++;

            return count;
        }

        /// <summary>原版 FarmCorruption.hx:966 numFollowersAtFarm()</summary>
        public static int NumFollowersAtFarm()
        {
            var count = 0;

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && !BimboSophie()) count++;

            return count;
        }

        /// <summary>原版 FarmCorruption.hx:1003 numLoversAtFarm()</summary>
        public static int NumLoversAtFarm()
        {
            var count = 0;

            if (F.Get(GameFlagIds.FollowerAtFarmIzma) > 0) count++;
            if (F.Get(GameFlagIds.FollowerAtFarmIsabella) > 0) count++;

            return count;
        }

        /// <summary>原版 FarmCorruption.hx:971 slavesAtFarm()</summary>
        public static bool SlavesAtFarm() => NumSlavesAtFarm() > 0;

        /// <summary>原版 FarmCorruption.hx:992 followersAtFarm()</summary>
        public static bool FollowersAtFarm() => NumFollowersAtFarm() > 0;

        /// <summary>原版 FarmCorruption.hx:1020 loversAtFarm()</summary>
        public static bool LoversAtFarm() => NumLoversAtFarm() > 0;

        private static bool BimboSophie() =>

            SceneInstance<Npcs.SophieBimboScene>.Get().BimboSophie();

        /// <summary>原版 FarmCorruption.hx:946 slavesAtFarmMenu()</summary>
        public static void SlavesAtFarmMenu()
        {
            Scene.ClearButtons();

            if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1)
                Scene.AddButton(0, "Common.Button.Amily".Localized(), () => SceneInstance<Npcs.AmilyScene>.Get().AmilyFollowerEncounter());

            if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1)
                Scene.AddButton(1, "Common.Button.Jojo".Localized(), () => SceneInstance<Npcs.JojoScene>.Get().CorruptCampJojo());

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && BimboSophie())
                Scene.AddButton(2, "Common.Button.Sophie".Localized(), () => SceneInstance<Npcs.SophieBimboScene>.Get().ApproachBimboSophieInCamp());

            if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1)
                Scene.AddButton(3, "Enemies.VapulaFight.Name".Localized(), () => SceneInstance<Npcs.VapulaScene>.Get().CallSlaveVapula());

            if (F.Get(GameFlagIds.FollowerAtFarmLatexy) == 1)
                Scene.AddButton(4,

                    SceneInstance<Npcs.LatexGirlScene>.Get().GooName(),

                    () => SceneInstance<Npcs.LatexGirlScene>.Get().ApproachLatexy());

            if (F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1)
                Scene.AddButton(5,

                    SceneInstance<Npcs.MilkWaifuScene>.Get().milkName,

                    () => SceneInstance<Npcs.MilkWaifuScene>.Get().MilkyMenu());

            Scene.AddButton(14, "Common.Button.Back".Localized(), FarmMenu);
        }

        /// <summary>原版 FarmCorruption.hx:993 followersAtFarmMenu()</summary>
        public static void FollowersAtFarmMenu()
        {
            Scene.ClearButtons();

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && !BimboSophie())
                Scene.AddButton(0, "Common.Button.Sophie".Localized(), () => SceneInstance<Npcs.SophieFollowerScene>.Get().FollowerSophieMainScreen());

            Scene.AddButton(14, "Common.Button.Back".Localized(), FarmMenu);
        }

        /// <summary>原版 FarmCorruption.hx:1023 loversAtFarmMenu()</summary>
        public static void LoversAtFarmMenu()
        {
            Scene.ClearButtons();

            switch (F.Get(GameFlagIds.FollowerAtFarmIzma))
            {
                case 1:
                    Scene.AddButton(0, "Common.Button.Izma".Localized(), () => SceneInstance<Npcs.IzmaScene>.Get().IzmaFollowerMenu());

                    break;

                case 2:
                    // 原版 :1030：Izmael（被腐化成魔）直接触发游戏结束
                    Scene.AddButton(0, "Common.Button.Izmael".Localized(), () => Scene.GameOver());

                    break;
            }

            if (F.Get(GameFlagIds.FollowerAtFarmIsabella) == 1)
                Scene.AddButton(1, "Common.Button.Isabella".Localized(), () => SceneInstance<Npcs.IsabellaFollowerScene>.Get().CallForFollowerIsabella());

            Scene.AddButton(14, "Common.Button.Back".Localized(), FarmMenu);
        }

        // ===== 原版 FarmCorruption.hx:1207-1574：农场投资系统（5 项升级 + 拒绝分支）=====

        /// <summary>原版 :1207 availableInvestments()</summary>
        public static bool AvailableInvestments()
        {
            var player = CoC.Instance.Player;

            if (!player.HasKeyItem("Breast Milker - Installed At Whitney's Farm")
                && F.Get(GameFlagIds.QueueBreastmilkerUpgrade) == 0)

                return true;

            if (!player.HasKeyItem("Cock Milker - Installed At Whitney's Farm")
                && F.Get(GameFlagIds.QueueCockmilkerUpgrade) == 0)

                return true;

            if (F.Get(GameFlagIds.FarmUpgradesRefinery) == 0 && F.Get(GameFlagIds.QueueRefineryUpgrade) == 0)
                return true;

            if (F.Get(GameFlagIds.FarmUpgradesContraceptive) == 0 && F.Get(GameFlagIds.QueueContraceptiveUpgrade) == 0)
                return true;

            if (F.Get(GameFlagIds.FarmUpgradesMilktank) == 0
                && SceneInstance<Npcs.MilkWaifuScene>.Get().MilkSlave()

                && F.Get(GameFlagIds.QueueMilktankUpgrade) == 0)

                return true;

            return false;
        }

        /// <summary>原版 :1323 investmentMenu()</summary>
        public static void InvestmentMenu()
        {
            var player = CoC.Instance.Player;

            Scene.ClearButtons();

            if (!player.HasKeyItem("Breast Milker - Installed At Whitney's Farm")
                && F.Get(GameFlagIds.QueueBreastmilkerUpgrade) == 0)

                Scene.AddButton(0, "Common.Button.BreastMilker".Localized(), InvestmentBreastMilker);

            if (!player.HasKeyItem("Cock Milker - Installed At Whitney's Farm")
                && F.Get(GameFlagIds.QueueCockmilkerUpgrade) == 0)

                Scene.AddButton(1, "Common.Button.CockMilker".Localized(), InvestmentCockMilker);

            if (F.Get(GameFlagIds.FarmUpgradesRefinery) == 0 && F.Get(GameFlagIds.QueueRefineryUpgrade) == 0)
                Scene.AddButton(2, "Common.Button.Refinery".Localized(), InvestmentRefinery);

            if (F.Get(GameFlagIds.FarmUpgradesContraceptive) == 0 && F.Get(GameFlagIds.QueueContraceptiveUpgrade) == 0)
                Scene.AddButton(3, "Common.Button.Contraceptive".Localized(), InvestmentContraceptive);

            if (F.Get(GameFlagIds.FarmUpgradesMilktank) == 0
                && SceneInstance<Npcs.MilkWaifuScene>.Get().MilkSlave()

                && F.Get(GameFlagIds.QueueMilktankUpgrade) == 0)

                Scene.AddButton(4, "Common.Button.Milktank".Localized(), InvestmentMilktank);

            if (!whitneyCorrupt())
                // 原版 :1200 回到「惠特尼尚未腐化」菜单
                Scene.AddButton(14, "Common.Button.Back".Localized(), DogeNotCorruptYetMenu);
            else
                // 原版 :1202 回到腐化完成菜单
                Scene.AddButton(14, "Common.Button.Back".Localized(), () => DogeCorruptedMissionComplete());
        }

        private static void InvestmentOffer(int price, Action confirm)
        {
            if (CoC.Instance.Player.Gems >= price)
                Scene.AddButton(0, "Common.Button.DoIt".Localized(), confirm);
            else
                Scene.AddButton(0, "Common.Button.DoIt".Localized(), () => TurnDownInvestment(true));

            Scene.AddButton(1, "Common.Button.No".Localized(), () => TurnDownInvestment());
        }

        /// <summary>原版 :1348 investmentBreastMilker()</summary>
        public static void InvestmentBreastMilker()
        {
            Scene.ClearOutput();

            Scene.OutputText(whitneyCorrupt()

                ? "Scenes.FarmCorruption.Invest.BreastMilker.Corrupt".Localized()

                : "Scenes.FarmCorruption.Invest.BreastMilker.Pure".Localized());

            InvestmentOffer(1000, DoBreastMilkerInvestment);
        }

        /// <summary>原版 :1370 doBreastMilkerInvestment()</summary>
        public static void DoBreastMilkerInvestment()
        {
            Scene.ClearOutput();

            CoC.Instance.Player.Gems -= 1000;

            Scene.OutputText("Scenes.FarmCorruption.Invest.Pay".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Invest.BreastMilker.Paid".Localized());

            F.Set(GameFlagIds.QueueBreastmilkerUpgrade, 1);

            Scene.DoNext(RootScene);
        }

        /// <summary>原版 :1385 investmentCockMilker()</summary>
        public static void InvestmentCockMilker()
        {
            Scene.ClearOutput();

            Scene.OutputText(whitneyCorrupt()

                ? "Scenes.FarmCorruption.Invest.CockMilker.Corrupt".Localized()

                : "Scenes.FarmCorruption.Invest.CockMilker.Pure".Localized());

            InvestmentOffer(1000, DoCockMilkerInvestment);
        }

        /// <summary>原版 :1406 doCockMilkerInvestment()</summary>
        public static void DoCockMilkerInvestment()
        {
            Scene.ClearOutput();

            CoC.Instance.Player.Gems -= 1000;

            Scene.OutputText("Scenes.FarmCorruption.Invest.Pay".Localized());

            Scene.OutputText(whitneyCorrupt()

                ? "Scenes.FarmCorruption.Invest.CockMilker.PaidCorrupt".Localized()

                : "Scenes.FarmCorruption.Invest.CockMilker.PaidPure".Localized());

            F.Set(GameFlagIds.QueueCockmilkerUpgrade, 1);

            Scene.DoNext(RootScene);
        }

        /// <summary>原版 :1426 investmentRefinery()</summary>
        public static void InvestmentRefinery()
        {
            Scene.ClearOutput();

            Scene.OutputText(whitneyCorrupt()

                ? "Scenes.FarmCorruption.Invest.Refinery.Corrupt".Localized()

                : "Scenes.FarmCorruption.Invest.Refinery.Pure".Localized());

            InvestmentOffer(1500, DoRefineryInvestment);
        }

        /// <summary>原版 :1447 doRefineryInvestment()</summary>
        public static void DoRefineryInvestment()
        {
            Scene.ClearOutput();

            CoC.Instance.Player.Gems -= 1500;

            Scene.OutputText("Scenes.FarmCorruption.Invest.Pay".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Invest.Refinery.Paid".Localized());

            F.Set(GameFlagIds.QueueRefineryUpgrade, 1);

            Scene.DoNext(RootScene);
        }

        /// <summary>原版 :1462 investmentContraceptive()</summary>
        public static void InvestmentContraceptive()
        {
            Scene.ClearOutput();

            Scene.OutputText(whitneyCorrupt()

                ? "Scenes.FarmCorruption.Invest.Contraceptive.Corrupt".Localized()

                : "Scenes.FarmCorruption.Invest.Contraceptive.Pure".Localized());

            InvestmentOffer(750, DoContraceptiveInvestment);
        }

        /// <summary>原版 :1483 doContraceptiveInvestment()</summary>
        public static void DoContraceptiveInvestment()
        {
            Scene.ClearOutput();

            CoC.Instance.Player.Gems -= 750;

            Scene.OutputText(whitneyCorrupt()

                ? "Scenes.FarmCorruption.Invest.Contraceptive.PaidCorrupt".Localized()

                : "Scenes.FarmCorruption.Invest.Contraceptive.PaidPure".Localized());

            F.Set(GameFlagIds.QueueContraceptiveUpgrade, 1);

            Scene.DoNext(RootScene);
        }

        /// <summary>原版 :1503 investmentMilktank()</summary>
        public static void InvestmentMilktank()
        {
            Scene.ClearOutput();

            var boobedBathGirl = F.Get(GameFlagIds.MilkSize) > 0

                && F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1;

            var key = whitneyCorrupt()

                ? (boobedBathGirl ? "Common.Text.ScenesFarmcorruptionInvestMilktankCorruptboobed".Localized() : "Common.Text.ScenesFarmcorruptionInvestMilktankCorruptflat".Localized())

                : (boobedBathGirl ? "Common.Text.ScenesFarmcorruptionInvestMilktankPureboobed".Localized() : "Common.Text.ScenesFarmcorruptionInvestMilktankPureflat".Localized());

            Scene.OutputText(key.Localized());

            InvestmentOffer(400, DoMilktankInvestment);
        }

        /// <summary>原版 :1538 doMilktankInvestment()</summary>
        public static void DoMilktankInvestment()
        {
            Scene.ClearOutput();

            Scene.OutputText(whitneyCorrupt()

                ? "Scenes.FarmCorruption.Invest.Milktank.PaidCorrupt".Localized()

                : "Scenes.FarmCorruption.Invest.Milktank.PaidPure".Localized());

            F.Set(GameFlagIds.QueueMilktankUpgrade, 1);

            CoC.Instance.Player.Gems -= 400;

            Scene.DoNext(RootScene);
        }

        /// <summary>原版 :1555 turnDownInvestment()</summary>
        public static void TurnDownInvestment(bool money = false)
        {
            Scene.ClearOutput();

            if (!whitneyCorrupt())
            {
                Scene.OutputText("Scenes.FarmCorruption.Invest.No.Pure".Localized());

                Scene.OutputText(money

                    ? "Scenes.FarmCorruption.Invest.No.PureNoMoney".Localized()

                    : "Scenes.FarmCorruption.Invest.No.PureChangeMind".Localized());
            }
            else
            {
                Scene.OutputText("Scenes.FarmCorruption.Invest.No.Corrupt".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Invest.No.CorruptMoney".Localized());
            }

            InvestmentMenu();
        }

        // ===== 原版 FarmCorruption.hx:1040-1235：惠特尼腐化流程（尚未完全腐化阶段）=====

        /// <summary>原版 :1040 dogeNotCorruptYet()：按腐化度分段的登场对白。</summary>
        public static void DogeNotCorruptYet()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            F.Set(GameFlagIds.FarmCorruptionApproachedWhitney, 1);

            var corruption = whitneyCorruption();

            // 原版 :1044 从高位跌回 30 以下的专属播报（只播一次）
            if (corruption < 30
                && F.Get(GameFlagIds.WhitneyCorruptionHighest) > 30

                && F.Get(GameFlagIds.WhitneyCorruption030DropMessage) == 0)
            {
                Scene.OutputText("Scenes.FarmCorruption.Doge.DropBelow30".Localized());

                F.Set(GameFlagIds.WhitneyCorruption030DropMessage, 1);

                if (F.Get(GameFlagIds.WhitneyLeave060) == 0)
                    Scene.DoNext(DogeNotCorruptLeaveFirstTime);
                else
                    Scene.DoNext(() => Scene.ReturnToCamp(1));

                return;
            }

            // 原版 :1057 0-30
            if (corruption <= 30)
            {
                Scene.OutputText("Scenes.FarmCorruption.Doge.C030".Localized());

                if (F.Get(GameFlagIds.WhitneyLeave060) == 0)
                    Scene.DoNext(DogeNotCorruptLeaveFirstTime);
                else
                    Scene.DoNext(() => Scene.ReturnToCamp(1));

                return;
            }

            // 原版 :1074 31-60
            if (corruption is >= 31 and <= 60)
            {
                if (F.Get(GameFlagIds.WhitneyMenu3160) == 0)
                {
                    F.Set(GameFlagIds.WhitneyMenu3160, 1);

                    Scene.OutputText("Scenes.FarmCorruption.Doge.C3160First".Localized());
                }
                else
                    Scene.OutputText("Scenes.FarmCorruption.Doge.C3160Later".Localized());

                DogeNotCorruptYetMenu();

                return;
            }

            // 原版 :1096 61-90
            if (corruption is >= 61 and <= 90)
            {
                if (F.Get(GameFlagIds.WhitneyMenu6190) == 0)
                {
                    F.Set(GameFlagIds.WhitneyMenu6190, 1);

                    Scene.OutputText("Scenes.FarmCorruption.Doge.C6190First".Localized());
                }
                else
                    Scene.OutputText("Scenes.FarmCorruption.Doge.C6190Later".Localized());

                DogeNotCorruptYetMenu();

                return;
            }

            // 原版 :1119 91-119
            if (corruption is >= 91 and <= 119)
            {
                if (F.Get(GameFlagIds.WhitneyMenu91119) == 0)
                {
                    F.Set(GameFlagIds.WhitneyMenu91119, 1);

                    Scene.OutputText("Scenes.FarmCorruption.Doge.C91119First".Localized());

                    Scene.OutputText("Scenes.FarmCorruption.Doge.C91119Speech".Localized());

                    Scene.OutputText("Scenes.FarmCorruption.Doge.C91119Gem".Localized());

                    Scene.DoNext(RootScene);

                    return;
                }

                Scene.OutputText("Scenes.FarmCorruption.Doge.C91119Later".Localized());

                DogeNotCorruptYetMenu();

                return;
            }

            // 原版 :1145 腐化度满 120
            if (corruption == 120 && F.Get(GameFlagIds.WhitneyCorruptionComplete) == 0)
            {
                F.Set(GameFlagIds.WhitneyCorruptionComplete, 1);

                Scene.OutputText(player.HasFur()

                    ? "Scenes.FarmCorruption.Doge.CompleteFur".Localized()

                    : "Scenes.FarmCorruption.Doge.CompleteNoFur".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Doge.CompleteTalk1".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Doge.CompleteTalk2".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Doge.CompleteTalk3".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Doge.CompleteMaster".Localized());

                Scene.ClearButtons();

                if (!player.HasFur())
                {
                    Scene.AddButton(0, "Common.Button.ChangeHer".Localized(), DeFurDoge);

                    Scene.AddButton(1, "Common.Button.DontChange".Localized(), DontDeFurDoge);
                }
                else
                    Scene.AddButton(0, "Common.Button.Continue".Localized(), DontDeFurDoge);
            }
        }

        /// <summary>原版 :1188 dogeNotCorruptYetMenu()</summary>
        public static void DogeNotCorruptYetMenu()
        {
            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Appearance".Localized(), WhitneyAppearanceNotCorrupt);

            Scene.AddButton(1, "Common.Button.Prosperity".Localized(), ProsperityGoNotCorrupt);

            if (AvailableInvestments())
                Scene.AddButton(2, "Common.Button.Investment".Localized(), InvestmentMenu);

            // 原版 :1194：首次 / 61-90 阶段用专属离开对白覆盖默认返回
            if (whitneyCorruption() <= 60 && F.Get(GameFlagIds.WhitneyLeave060) == 0)
                Scene.AddButton(14, "Common.Button.Back".Localized(), DogeNotCorruptLeaveFirstTime);
            else if (whitneyCorruption() <= 90 && F.Get(GameFlagIds.WhitneyLeave6190) == 0)
                Scene.AddButton(14, "Common.Button.Back".Localized(), DogeNotCorruptLeave6190);
            else
                Scene.AddButton(14, "Common.Button.Back".Localized(), RootScene);
        }

        /// <summary>原版 :1162 dogeNotCorruptLeaveFirstTime()</summary>
        public static void DogeNotCorruptLeaveFirstTime()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            F.Set(GameFlagIds.WhitneyLeave060, 1);

            Scene.OutputText("Scenes.FarmCorruption.Doge.LeaveFirstTime".Localized());

            Scene.DoNext(RootScene);
        }

        /// <summary>原版 :1175 dogeNotCorruptLeave6190()</summary>
        public static void DogeNotCorruptLeave6190()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            F.Set(GameFlagIds.WhitneyLeave6190, 1);

            Scene.OutputText("Scenes.FarmCorruption.Doge.Leave6190".Localized());

            Scene.DoNext(RootScene);
        }

        /// <summary>原版 :1226 whitneyAppearanceNotCorrupt()：惠特尼（未腐化）外观描写。</summary>
        public static void WhitneyAppearanceNotCorrupt()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText(player.HasFur()

                ? "Scenes.FarmCorruption.WhitneyAppearance.NotCorruptFur".Localized()

                : "Scenes.FarmCorruption.WhitneyAppearance.NotCorruptNoFur".Localized());

            DogeNotCorruptYetMenu();
        }

        // ===== 原版 FarmCorruption.hx:422 farmProtection()：农场防护力（用于繁荣对白分段）=====

        public static int FarmProtection()
        {
            var protection = 0;

            if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1) protection += 1;
            if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1) protection += 2;

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1)
                protection += BimboSophie() ? 1 : 2;

            if (F.Get(GameFlagIds.FollowerAtFarmIzma) == 1) protection += 4;
            if (F.Get(GameFlagIds.FollowerAtFarmIsabella) == 1) protection += 3;
            if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1) protection += 2;

            if (F.Get(GameFlagIds.FollowerAtFarmKelly) == 1)
                protection += F.Get(GameFlagIds.KeltBreakLevel) >= 4 ? 1 : 2;

            if (F.Get(GameFlagIds.FollowerAtFarmMarble) == 1) protection += 2;

            // 原版 :464：BathGirl 只有已催乳时才提供 1 点
            if (F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1 && F.Get(GameFlagIds.MilkSize) > 0)
                protection += 1;

            if (F.Get(GameFlagIds.FollowerAtFarmCeraph) > 0) protection += 7;

            return protection;
        }

        /// <summary>原版 :1282 prosperityGoNotCorrupt()：按「产出 / 防护」四象限播报。</summary>
        public static void ProsperityGoNotCorrupt()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Prosperity.Intro".Localized());

            var lowProtection = FarmProtection() < 12;

            var lowValue = FarmValue() < 12;

            if (lowValue && lowProtection)
            {
                Scene.OutputText("Scenes.FarmCorruption.Prosperity.LowBoth".Localized());

                Scene.OutputText(NumFollowersAtFarm() + NumSlavesAtFarm() + NumLoversAtFarm() == 0

                    ? "Scenes.FarmCorruption.Prosperity.LowBothNoWorkers".Localized()

                    : "Scenes.FarmCorruption.Prosperity.LowBothSomeWorkers".Localized());
            }
            else if (!lowValue && lowProtection)
                Scene.OutputText("Scenes.FarmCorruption.Prosperity.HighValueLowProtection".Localized());
            else if (lowValue && !lowProtection)
                Scene.OutputText("Scenes.FarmCorruption.Prosperity.LowValueHighProtection".Localized());
            else
                Scene.OutputText("Scenes.FarmCorruption.Prosperity.HighBoth".Localized());

            DogeNotCorruptYetMenu();
        }

        /// <summary>原版 :1576 deFurDoge()：把惠特尼变成人形（去掉毛皮）。</summary>
        public static void DeFurDoge()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.DeFur.Action".Localized());

            Scene.OutputText("Scenes.FarmCorruption.DeFur.Obedience".Localized());

            Scene.OutputText("Scenes.FarmCorruption.DeFur.Acquiesce".Localized());

            Scene.OutputText("Scenes.FarmCorruption.DeFur.GoodGirl".Localized());

            // 原版 :1588：当天从农场菜单移除，并记为已去毛
            F.Set(GameFlagIds.WhitneyDisabledForDay, 1);

            F.Set(GameFlagIds.WhitneyDefurred, 1);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:99-131 惠特尼状态辅助 + :1236 腐化外观 + :1677 腐化后主菜单 =====

        /// <summary>原版 :99 WhitneyDefurred()</summary>
        public static bool WhitneyDefurred() => F.Get(GameFlagIds.WhitneyDefurred) == 1;

        /// <summary>原版 :106 whitneyHasTattoo()：四个纹身槽任一有记录。</summary>
        public static bool WhitneyHasTattoo() =>

            F.Has(GameFlagIds.WhitneyTattooCollarbone)

            || F.Has(GameFlagIds.WhitneyTattooShoulders)

            || F.Has(GameFlagIds.WhitneyTattooLowerback)

            || F.Has(GameFlagIds.WhitneyTattooButt);

        /// <summary>原版 :118 whitneyMaxedOralTraining()：口交训练满 100。</summary>
        public static bool WhitneyMaxedOralTraining() => F.Get(GameFlagIds.WhitneyOralTraining) == 100;

        /// <summary>原版 :1236 whitneyAppearanceCorrupt()：腐化后外观描写（含纹身清单）。</summary>
        public static void WhitneyAppearanceCorrupt()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var defurred = WhitneyDefurred();

            var noFur = defurred || !player.HasFur();

            if (!player.HasFur())
            {
                Scene.OutputText("Scenes.FarmCorruption.WhitneyAppearance.CorruptGirl".Localized());

                if (!defurred)
                    Scene.OutputText("Scenes.FarmCorruption.WhitneyAppearance.CorruptGirlFresh".Localized());
            }
            else
                Scene.OutputText("Scenes.FarmCorruption.WhitneyAppearance.CorruptMorph".Localized());

            if (WhitneyMaxedOralTraining())
                Scene.OutputText("Scenes.FarmCorruption.WhitneyAppearance.CorruptLips".Localized());

            Scene.OutputText("Scenes.FarmCorruption.WhitneyAppearance.CorruptDress".Localized());

            if (WhitneyHasTattoo())
            {
                Scene.OutputText(numTattoos("whitney") > 1

                    ? "Scenes.FarmCorruption.WhitneyAppearance.TattoosPlural".Localized()

                    : "Scenes.FarmCorruption.WhitneyAppearance.TattooSingle".Localized());

                if (F.Has(GameFlagIds.WhitneyTattooCollarbone))
                    Scene.OutputText(F.Get(GameFlagIds.WhitneyTattooCollarbone) + "Common.Text.N".Localized());

                if (F.Has(GameFlagIds.WhitneyTattooShoulders))
                    Scene.OutputText(F.Get(GameFlagIds.WhitneyTattooShoulders) + "Common.Text.N".Localized());

                if (F.Has(GameFlagIds.WhitneyTattooLowerback))
                    Scene.OutputText(F.Get(GameFlagIds.WhitneyTattooLowerback) + "Common.Text.N".Localized());

                if (F.Has(GameFlagIds.WhitneyTattooButt))
                    Scene.OutputText(F.Get(GameFlagIds.WhitneyTattooButt) + "Common.Text.N".Localized());
            }

            Scene.OutputText("Common.Text.N".Localized());

            Scene.OutputText(noFur

                ? "Scenes.FarmCorruption.WhitneyAppearance.SkinLine".Localized()

                : "Scenes.FarmCorruption.WhitneyAppearance.FurLine".Localized());

            Scene.OutputText("Scenes.FarmCorruption.WhitneyAppearance.CommonDetails".Localized());

            DogeCorruptedMissionComplete(false);
        }

        /// <summary>
        /// 原版 :1677 dogeCorruptedMissionComplete(output)：腐化完成后的主菜单。
        /// Branding（:1690）与 Pleasure（:1697）两个按钮均已接通对应子系统。
        /// </summary>
        public static void DogeCorruptedMissionComplete(bool output = true)
        {
            if (output)
            {
                Scene.ClearOutput();

                Scene.SpriteSelect("whitney");

                Scene.OutputText(whitneyDom()

                    ? "Scenes.FarmCorruption.CorruptMenu.DomGreeting".Localized()

                    : "Scenes.FarmCorruption.CorruptMenu.SubGreeting".Localized());
            }

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Appearance".Localized(), WhitneyAppearanceCorrupt);

            if (AvailableInvestments())
                Scene.AddButton(1, "Common.Button.Investment".Localized(), InvestmentMenu);

            // 原版 :1690：品牌烙印菜单（解锁后或尚未排队升级时显示）
            if (F.Get(GameFlagIds.FarmCorruptionBrandingMenuUnlocked) == 1 || F.Get(GameFlagIds.QueueBrandingUpgrade) < 1)
                Scene.AddButton(2, "Common.Button.Branding".Localized(), BrandingMenu);

            // 原版 :1697-1701：支配状态下进支配向 Pleasure，否则走服从向口交训练
            if (whitneyDom())
                Scene.AddButton(3, "Common.Button.Pleasure".Localized(), WhitneyDomPleasure);
            else
                Scene.AddButton(3, "Common.Button.Pleasure".Localized(), WhitneySubPleasure);

            Scene.AddButton(14, "Common.Button.Back".Localized(), RootScene);
        }

        // ===== 原版 FarmCorruption.hx:1781 firstOralTraining()：口交训练的开场（性别中立）=====

        /// <summary>原版 :1781 firstOralTraining()：初次训练的引导对白，含傻化模式的台词分支。</summary>
        public static void FirstOralTraining()
        {
            var player = CoC.Instance.Player;

            var hasCock = player.HasCock();

            var hasVagina = player.HasVagina();

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstIntro".Localized());

            if (hasCock)
                Scene.OutputText(" " + "Common.Text.Eachcock".Localized());

            if (hasCock && hasVagina)
                Scene.OutputText("Common.Text.And".Localized());

            if (hasVagina)
                Scene.OutputText("Common.Text.Vagina".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstIntroEnd".Localized());

            Scene.OutputText(hasCock

                ? "Scenes.FarmCorruption.Oral.FirstReplyCock".Localized()

                : "Scenes.FarmCorruption.Oral.FirstReplyVagina".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstTeach".Localized());

            Scene.OutputText(hasCock

                ? "Scenes.FarmCorruption.Oral.FirstCocksucker".Localized()

                : "Scenes.FarmCorruption.Oral.FirstPussyEater".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstDoubt".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstWillTry".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstNo".Localized());

            // 原版 :1806：傻化模式下换成尤达台词
            Scene.OutputText(Settings.ModeSettings.Instance.Silly

                ? "Scenes.FarmCorruption.Oral.FirstSillyDo".Localized()

                : "Scenes.FarmCorruption.Oral.FirstYouWill".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstDetermination".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstYes".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Oral.FirstInstruction".Localized());
        }

        // 由 Tools/PortKit/gen-scene-emit.mjs 从原版 FarmCorruption.hx:1995 生成
        public static void cockOralTrainingStageTwo()
        {
            var player = CoC.Instance.Player;

            // 原版 :1999
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.One".Localized());
            // 原版 :2003
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Two".Localized());
            // 原版 :2005
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Three".Localized());
            // 原版 :2008
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Four".Localized());
            // 原版 :2010
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Five".Localized());
            // 原版 :2012
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Six".Localized());
            // 原版 :2014
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Seven".Localized());
            // 原版 :2016
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Eight".Localized());
            // 原版 :2017
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Nine".Localized());
            // 原版 :2018
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Ten".Localized());
            // 原版 :2020
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Eleven".Localized());
            // 原版 :2022
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Twelve".Localized());
            // 原版 :2024
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Thirteen".Localized());
            // 原版 :2026
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Fourteen".Localized());
            // 原版 :2028
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Fifteen".Localized());
            // 原版 :2030
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Sixteen".Localized());
            // 原版 :2032
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Seventeen".Localized());
            // 原版 :2033
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Eighteen".Localized());
            // 原版 :2038
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Nineteen".Localized());
            // 原版 :2040
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.Twenty".Localized());
            // 原版 :2041
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.TwentyOne".Localized());
            // 原版 :2042
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.TwentyTwo".Localized());
            // 原版 :2043
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.TwentyThree".Localized());
            // 原版 :2044
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.TwentyFour".Localized());
            // 原版 :2045
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.25".Localized());
            // 原版 :2046
            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage2.26".Localized());
        }

        // ===== 原版 FarmCorruption.hx:1816 firstCockOralTraining()：第一次口交训练（有阴茎）=====

        public static void FirstCockOralTraining()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.One".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.Two".Localized());

            // 原版 :1822：阴道分支插在句子中间
            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.3a".Localized());

            Scene.OutputText(player.HasVagina()

                ? "Scenes.FarmCorruption.CockOral.First.3bVagina".Localized()

                : "Scenes.FarmCorruption.CockOral.First.3bNoVagina".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.3c".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.Four".Localized());

            // 原版 :1824：大屌（>12）专属一段
            if (player.BiggestCockLength() > 12)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First.5BigCock".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.Six".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.Seven".Localized());

            // 原版 :1831：高射精量（>=1500）分支
            Scene.OutputText(player.CumQ() >= 1500

                ? "Scenes.FarmCorruption.CockOral.First.8HighCum".Localized()

                : "Scenes.FarmCorruption.CockOral.First.8NormalCum".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.Nine".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First.Ten".Localized());

            // 原版 :1841：结算
            player.Orgasm("Dick");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:1845 cockOralTrainingStageOne()：训练阶段一 =====

        public static void CockOralTrainingStageOne()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.One".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.Two".Localized());

            // 原版 :1849：有阴道或多根阴茎时加训「同时兼顾」
            if (player.HasVagina() || player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.3Multi".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.Four".Localized());

            // 原版 :1855：大屌（>12）专属一段
            if (player.BiggestCockLength() > 12)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.5BigCock".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.Six".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.Seven".Localized());

            // 原版 :1862：高射精量分支
            Scene.OutputText(player.CumQ() >= 1500

                ? "Scenes.FarmCorruption.CockOral.Stage1.8HighCum".Localized()

                : "Scenes.FarmCorruption.CockOral.Stage1.8NormalCum".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.Nine".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage1.Ten".Localized());

            player.Orgasm("Dick");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2380 cockOralTrainingMaxed()：训练满级后的日常（77 段对白）=====

        public static void CockOralTrainingMaxed()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouSettleYourselfDown".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.PleaseMaySuckYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.VeryWellYouSay".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheSpasmsForwardHer".Localized());

            if (!player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouSighBlissfullyHer".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheRisesUntilHer".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheBathesYourCock".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.FinallySheSnakesHer".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouOpenYourMouth".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.WhenSheFinallyEnvelopes".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SuchHerMasteryNow".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.TheLongDeliciousDaze".Localized());

            if (!player.HasVagina() && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SwitchingHerAttentionYour".Localized());

            if (!player.HasVagina() && player.Cocks.Count >= 3)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SwitchingHerAttentionFirst".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.ThatEachYourFiercely".Localized());
            // 原版 :2415（按 BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Maxed.PushingHerAllThe".Localized()

                : "Scenes.FarmCorruption.CockOral.Maxed.Mouth".Localized());

            if (!player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.AndHerTongueOnce".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.BeforeWrappingYourHand".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouLetHerAnd".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.HerHandsWrappingBack".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Punctuation".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.TheyNeverLeftThem".Localized());
            // 原版 :2430（按 BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Maxed.PullingAndKneadingYou".Localized()

                : "Scenes.FarmCorruption.CockOral.Maxed.Mouth".Localized());
            // 原版 :2431（按 BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Maxed.AlwaysWhenYouFace".Localized()

                : "Scenes.FarmCorruption.CockOral.Maxed.TheBackHerMouth".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouCanFindThe".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheDoesnForgetThe".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Punctuation".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.TrappedHerGraspFor".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Punctuation3".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Them".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.HardYouFuckingHer".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.TheyFlex".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Flexes".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.OutCumJustEagerly".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Twin".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count == 3)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Triple".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Transcendent".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.High".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Paints".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.TheyPaint".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.HerSkirtWith".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Its".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.Their".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.GenerousDischarge".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.EventuallyYouCanGive".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouSighBeatificallyAnd".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.OnceYourWholeGroin".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.WhatYouSayBitch".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.ThankYouYourTaskmistress".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheSpasmsForwardHerTongue".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.GivenTheWayHer".Localized());

            if (player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouSighBlissfullyHerPlump".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheRisesUntilHerHead".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheBathesYourCockBiggest".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouOpenYourMouthThe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.WhenSheFinallyEnvelopesYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SuchHerMasteryNowWhitney".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.TheLongDeliciousDazeThat".Localized());

            if (player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SwitchingHerAttentionYourCock".Localized());

            if (player.Cocks.Count >= 3)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SwitchingHerFirstOne".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.ThatEachYourFiercelyErect".Localized());
            // 原版 :2505（按 BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Maxed.PushingHerAllTheWay".Localized()

                : "Scenes.FarmCorruption.CockOral.Maxed.Mouth".Localized());

            if (player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.AndHerTongueOnceAgain".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.BeforeWrappingYourHandThrough".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.YouLetHerAndShe".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.AndHerOtherHand".Localized());
            // 原版 :2514（按 BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Maxed.TheyNeverLeftThem".Localized()

                : "Scenes.FarmCorruption.CockOral.Maxed.Mouth".Localized());
            // 原版 :2515（按 BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Maxed.EvenWhenYouFace".Localized()

                : "Scenes.FarmCorruption.CockOral.Maxed.TheBackHerMouth".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.ThereSeriesShrillStrangled".Localized());

            if (player.Wetness() > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SoakingHerArmGutters".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.HeighteningTheEcstasyYou".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.SheDoesnForgetTheCock".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.EventuallyYouCanGiveHer".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.MomentLaterYouFeel".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.OnceYourWholeGroinHas".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.WhatYouSayBitch".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Maxed.ThankYouYourTask".Localized());

            // 原版 :2520 附近的结算（orgasm + 回营）
            player.Orgasm("Dick");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2205 cockOralTrainingStageThree() =====

        public static void CockOralTrainingStageThree()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YouSmirkTheDog".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.WhenYouFinallySettle".Localized());

            if (player.Cocks.Count >= 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.AllowingYourOtherCock".Localized());

            if (player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Punctuation".Localized());

            if (player.Cocks.Count >= 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.UnfurlAndBobBehind".Localized());

            if (player.HasVagina() && player.Cocks.Count >= 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.And".Localized());

            if (player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.LeisurelyExposingYourMoist".Localized());

            if (player.Cocks.Count >= 2 && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.TheSameTime".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.PerfumingTheAirAround".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.GodsSheMoansUnable".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.TimeForTheLast".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YesTheTailWags".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.AllRightPutYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.PleaseMaySuckYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.VeryWellYouSay".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.SheSpasmsForwardHer".Localized());

            if (!player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YouSighBlissfullyThe".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.SheRisesUntilHer".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.SheBathesYourCock".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.FinallySheSnakesHer".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.WhenSheFinallyEnvelopes".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YouKeepCloseEye".Localized());
            // 原版 :2253（按 player.BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Stage3.YouGrabHoldThose".Localized()

                : "Scenes.FarmCorruption.CockOral.Stage3.Mouth".Localized());

            if (player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YourSalivaGlossedBeating".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.ThereOneDeficiencyHer".Localized());
            // 原版 :2258（按 player.BiggestCockLength() < 8 拆分）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.Stage3.EvenWhenYouFace".Localized()

                : "Scenes.FarmCorruption.CockOral.Stage3.TheBackHerMouth".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.ThereSeriesShrillStrangled".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.SheDoesnForgetThe".Localized());

            if (player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Punctuation".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.TrappedHerGraspFor".Localized());

            if (player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Punctuation3".Localized());

            if (player.Cocks.Count > 1 && !(player.Cocks.Count == 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Them".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.HardYouFuckingHer".Localized());

            if (player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Flexes".Localized());

            if (player.Cocks.Count > 1 && !(player.Cocks.Count == 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.TheyFlex".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.OutCumJustEagerly".Localized());

            if (player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Twin".Localized());

            if (player.Cocks.Count > 1 && !(player.Cocks.Count == 2) && (player.Cocks.Count == 3))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Triple".Localized());

            if (player.Cocks.Count > 1 && !(!(player.Cocks.Count == 2) && (player.Cocks.Count == 3)))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Transcendent".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.High".Localized());

            if (player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Paints".Localized());

            if (player.Cocks.Count > 1 && !(player.Cocks.Count == 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.TheyPaint".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.HerSkirtWith".Localized());

            if (player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Its".Localized());

            if (player.Cocks.Count > 1 && !(player.Cocks.Count == 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Their".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.GenerousDischarge".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.SheSuddenlyGaspsShock".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.GruntingExultationYouKeep".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YouSighBeatificallyAnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.MasterWhatHaveYou".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YouMayNotHave".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.All".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YourCock".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Punctuation".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.FirstKissingYourHead".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.Punctuation".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.MakeTheLastDrool".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YouCloseYourEyes".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.YouPullHerOff".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.Stage3.SheWhispersLickingHer".Localized());
        }

        // ===== 原版 FarmCorruption.hx:1877 firstCockOralTrainingStageTwo() =====

        public static void FirstCockOralTrainingStageTwo()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.UndoingTheBottomHalf".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YourFaithImmediatelyRepaid".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GodsdamnSheEvenLearned".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.WithSomeEffortYou".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.SorryDoingSomethingWrong".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.NotAllYouGroan".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.FirstAllBeforeYou".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.AfterSlightPauseWhitney".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.OohYouNaturalNow".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.CarefullyWhitneyDelvesBetween".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.SheDoesLeastHave".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.EventuallyThoughSheStarts".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GoodYouHuskWith".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.OnceAgainThereAre".Localized());
            // 原版 :1898（按 player.BiggestCockLength() < 8 拆分，共 1 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First2.EventuallyYouCannotHold".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.Mouth".Localized());
            // 原版 :1899（按 player.Wetness() <= 2 拆分，共 1 处拼接）
            Scene.OutputText(player.Wetness() <= 2

                ? "Scenes.FarmCorruption.CockOral.First2.CumBitchYouGrit".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.WettingYourSlaveHand".Localized());

            if (player.HasVagina() && player.Balls > 0 && player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.WhichMeanFeatGiven".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.Punctuation".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouExhaleLongAnd".Localized());

            if (player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GodsThatWasSomething".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.ThisBitMoreDelicate".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.CarefullyWhitneyDelvesBetweenYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.SheDoesLeastHaveSome".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.EventuallyThoughSheStartsGet".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GoodYouHuskWithSome".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.OnceAgainThereAreDifficulties".Localized());
            // 原版 :1915（按 player.BiggestCockLength() < 8 拆分，共 1 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First2.EventuallyYouCannotHoldYour".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.Mouth".Localized());
            // 原版 :1916（按 player.Wetness() <= 2 拆分，共 1 处拼接）
            Scene.OutputText(player.Wetness() <= 2

                ? "Scenes.FarmCorruption.CockOral.First2.CumBitchYouGritYou".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.WettingYourSlaveHand".Localized());

            if (player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.WhichMeanFeatGivenFeels".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.Punctuation".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouExhaleLongAndLow".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GodsThatWasSomethingMistress".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.FirstAllBeforeYouStart".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.AfterSlightPauseWhitneySinks".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.OohYouNatural".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouLeanBackAnd".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouKeepEyeHer".Localized());
            // 原版 :1933（按 player.BiggestCockLength() < 8 拆分，共 1 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First2.EventuallyYouCannotHoldYourLust".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.Mouth".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.CumBitchYouGrit".Localized());

            if (player.Cocks.Count == 1 && player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.WhichMeanFeatGivenFeelsLike".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.Punctuation".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouLeanBackAndConsider".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouSighAfterShort".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GoodYouHuskWithSomeEffort".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.OnceAgainThereAreDifficultiesYou".Localized());
            // 原版 :1946（按 player.BiggestCockLength() < 8 拆分，共 1 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First2.EventuallyYouCannotHoldYourLustBack".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.Mouth".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.CumBitchYouGritYou".Localized());

            if (player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.WhichMeanFeatGivenFeelsLikeYou".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.Punctuation".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.TheSecondCockTrapped".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.ThisBitMoreTechnical".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouSighAfterShortPause".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GoodYouHuskWithSomeEffortKeeping".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.OnceAgainThereAreDifficultiesYouHave".Localized());
            // 原版 :1965（按 player.BiggestCockLength() < 8 拆分，共 1 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First2.EventuallyYouCannotHoldYourLustBackYou".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.Mouth".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.PutYourOtherHand".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouKeepEyeHerAnd".Localized());
            // 原版 :1971（按 player.BiggestCockLength() < 8 拆分，共 1 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First2.EventuallyYouCannotHoldYourLustBackYouGrab".Localized()

                : "Scenes.FarmCorruption.CockOral.First2.Mouth".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.CumBitchYouGritYouSurge".Localized());

            if (player.Cocks.Count == 1 && player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.WhichMeanFeatGivenFeelsLikeYouFucked".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.Punctuation".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.YouExhaleLongAndLowWhen".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.GodsThatWasSomethingMaster".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.AndCockBiggest".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First2.ThereEagernessDisplayHere".Localized());

            player.Orgasm("Generic");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2054 firstCockOralTrainingStageThree() =====

        public static void FirstCockOralTrainingStageThree()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouSmileWhitneyYou".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.OnceYouHaveSettled".Localized());

            if (!player.HasVagina() && player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouSighBlissfullyThe".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SheRisesUntilHer".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SheBathesYourCock".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.WhenSheFinallyEnvelopes".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouCloseYourEyes".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.WithOneHandSchlicking".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.StopYouMurmurThere".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SorrySheMumblesActually".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.PutYourOtherHand".Localized());

            if (!player.HasVagina() && !(player.Cocks.Count > 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.PutYourHandHere".Localized());

            if (!player.HasVagina() && !(player.Cocks.Count > 2) && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.JerkingBothYourCocks".Localized());

            if (!player.HasVagina() && !(player.Cocks.Count > 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Punctuation".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.NowGetBackYour".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.AllTheWhileYour".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.AndCockBiggest".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.BulgeAndVibrateWith".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Punctuation2".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.ThoughWouldEasyLose".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouDonEvenNeed".Localized());
            // 原版 :2098（按 player.BiggestCockLength() < 8 拆分，共 2 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First3.TheSightAndSound".Localized()

                : "Scenes.FarmCorruption.CockOral.First3.Mouth".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouCanFindThe".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 || player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.ParagraphBreak".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SheDoesnForgetThe".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Punctuation2".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.TrappedHerGraspFor".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Them".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && !(player.Cocks.Count > 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Punctuation4".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.HardYouFuckingHer".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.TheyFlex".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && !(player.Cocks.Count > 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Flexes".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.OutCumJustEagerly".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count == 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Twin".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && !(player.Cocks.Count == 2) && (player.Cocks.Count == 3))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Triple".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && !(!(player.Cocks.Count == 2) && (player.Cocks.Count == 3)))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Transcendent".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.High".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.TheyPaint".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && !(player.Cocks.Count > 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Paints".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.HerSkirtWith".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && player.Cocks.Count > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Their".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1 && !(player.Cocks.Count > 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.Its".Localized());

            if (!player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.GenerousDischarge".Localized());

            if (!player.HasVagina() && player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.OnceYouAlmostDone".Localized());
            // 原版 :2146（按 player.Cocks.Count == 1 拆分，共 1 处拼接）
            Scene.OutputText(player.Cocks.Count == 1

                ? "Scenes.FarmCorruption.CockOral.First3.OnceYouHaveTensed".Localized()

                : "Scenes.FarmCorruption.CockOral.First3.WhitneyCleansYourCock".Localized());

            if (!player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.ThinkGotSheWhispers".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.OnceYouHaveSettledYourself".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouSighSheLaps".Localized());

            if (player.Balls > 0)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouSighBlissfullyTheFlat".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SheRisesUntilHerHead".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.WhenSheFinallyEnvelopesYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouCloseYourEyesHappy".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.WithOneHandSchlickingAway".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.StopYouMurmurThereReluctant".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SorrySheMumblesActuallyLooking".Localized());

            if (player.Cocks.Count >= 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.PutYourOtherHandThis".Localized());

            if (!(player.Cocks.Count >= 2))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.PutYourHandHereSlowly".Localized());
            // 原版 :2169（按 player.Cocks.Count >= 2 拆分，共 1 处拼接）
            Scene.OutputText(player.Cocks.Count >= 2

                ? "Scenes.FarmCorruption.CockOral.First3.NowGetBackYour".Localized()

                : "Scenes.FarmCorruption.CockOral.First3.AndCockBiggestThrum".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouDonEvenNeedLook".Localized());
            // 原版 :2171（按 player.BiggestCockLength() < 8 拆分，共 1 处拼接）
            Scene.OutputText(player.BiggestCockLength() < 8

                ? "Scenes.FarmCorruption.CockOral.First3.TheSightAndSoundHer".Localized()

                : "Scenes.FarmCorruption.CockOral.First3.Mouth".Localized());

            if (player.BiggestCockLength() < 8)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.TheBackHerMouth".Localized());

            if (!(player.BiggestCockLength() < 8))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.HerGullet".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouSpearDecisivelyInwards".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.YouCanFindTheWords".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.HerFingersSlipAnd".Localized());

            if (player.Wetness() > 2)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SoakingHerArmGutters".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.HeighteningTheEcstasyYou".Localized());

            if (player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SheDoesnForgetTheCock".Localized());

            if (player.CumQ() >= 1500)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.OnceYouAlmostDoneEver".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.OnceYouHaveTensed".Localized());

            if (player.Cocks.Count == 1)
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.WhitneyFirstCleansYour".Localized());

            if (!(player.Cocks.Count == 1))
                Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.WhitneyUsesHerFlat".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.SheMoansTheLow".Localized());

            Scene.OutputText("Scenes.FarmCorruption.CockOral.First3.ThinkGotSheWhispersGrinning".Localized());

            player.Orgasm("Dick");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2536 firstVaginaOralTraining() =====

        public static void firstVaginaOralTraining()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2540".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2541".Localized());

            if (player.GetClitLength() > 3)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2543".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2545".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2546".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2547".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2548".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2549".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2550".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2551".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2553".Localized());

            if (!(player.Wetness() >= 5))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2555".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2557".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2558".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First.L2559".Localized());

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2566 vaginaOralTrainingStageOne() =====

        public static void vaginaOralTrainingStageOne()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            // 原版 :2570（按 WhitneyDefurred() || !player.HasFur() 拆分，共 1 处拼接）
            Scene.OutputText(WhitneyDefurred() || !player.HasFur()

                ? "Scenes.FarmCorruption.VagOral.Stage1.SighingYouSlideOut".Localized()

                : "Scenes.FarmCorruption.VagOral.Stage1.Hands".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.WhatFollowsOccasionallyFrustrating".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.YouKnowThatAhead".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.SheStillDoesnSeem".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.YouMayCumNow".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.TheDogWomanCoughs".Localized());

            if (!(player.Wetness() >= 5))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.TheDogWomanCoughsAnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.AfterYouDoneAnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.ThatWasPrettyFun".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.YouTellHerWith".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage1.AlrightMistressWill".Localized());

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2590 firstVaginaOralTrainingStageTwo() =====

        public static void firstVaginaOralTrainingStageTwo()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2594".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2595".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2596".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2597".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2598".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2599".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2601".Localized());

            if (!(player.LactationQ() > 0))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2603".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2605".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2606".Localized());

            if (player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2608".Localized());

            if (!(player.HasFuckableNipples()))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2610".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2612".Localized());

            if (player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2614".Localized());

            if (!(player.HasFuckableNipples()))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2616".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2619".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2620".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2621".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2623".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2624".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2625".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2626".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2627".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2628".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2629".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2630".Localized());

            if (player.Wetness() <= 2)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2632".Localized());

            if (!(player.Wetness() <= 2))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2634".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2636".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2638".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2640".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2641".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2642".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First2.L2643".Localized());

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2650 vaginaOralTrainingStageTwo() =====

        public static void vaginaOralTrainingStageTwo()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2654".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2655".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2657".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2659".Localized());

            if (!player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2661".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2663".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2664".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2665".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2666".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2667".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2669".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2671".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2673".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2675".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2676".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2677".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2678".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage2.L2679".Localized());

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2686 firstVaginaOralTrainingStageThree() =====

        public static void firstVaginaOralTrainingStageThree()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2690".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2691".Localized());

            if (!player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2693".Localized());

            if (!(!player.HasFuckableNipples()))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2695".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2697".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2699".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2701".Localized());

            if (!player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2703".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2705".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2706".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2707".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2708".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2709".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2710".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2711".Localized());

            if (player.GetClitLength() >= 4)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2713".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2715".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2716".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2717".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2719".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2721".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2723".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2725".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2726".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2727".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.First3.L2728".Localized());

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2735 vaginaOralTrainingStageThree() =====

        public static void vaginaOralTrainingStageThree()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2739".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2740".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2741".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2742".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2743".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2744".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2745".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2746".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2747".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2748".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2749".Localized());

            if (!player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2751".Localized());

            if (!(!player.HasFuckableNipples()))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2753".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2755".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2757".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2759".Localized());

            if (player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2761".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2763".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2764".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2765".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2766".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2767".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2768".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2769".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2770".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2771".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2773".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2775".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2776".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2777".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2778".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2779".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2780".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Stage3.L2781".Localized());

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2788 vaginaOralTrainingMaxed() =====

        public static void vaginaOralTrainingMaxed()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2792".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2793".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2794".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2795".Localized());

            if (!player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2797".Localized());

            if (!(!player.HasFuckableNipples()))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2799".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2801".Localized());

            if (player.LactationQ() > 0)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2803".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2805".Localized());

            if (!player.HasFuckableNipples())
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2807".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2809".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2810".Localized());

            if (player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating < 4)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2812".Localized());

            if (player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating < 8)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2815".Localized());

            if (!(player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating < 8))
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2817".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2819".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2820".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2821".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2822".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2823".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2824".Localized());

            if (player.GetClitLength() >= 4)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2827".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2829".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2830".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2832".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2834".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2835".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2836".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2837".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2838".Localized());

            Scene.OutputText("Scenes.FarmCorruption.VagOral.Maxed.L2839".Localized());

            player.Orgasm("Vaginal");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:1719-1775：口交训练的分发层 =====

        /// <summary>原版 :1719 whitneySubPleasure()：按训练计数推进并调用对应阶段。</summary>
        public static void WhitneySubPleasure()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var functorOnNext = false;

            // 原版 :1724：计数为 0 时先播开场（要等玩家点下一步）
            if (F.Get(GameFlagIds.WhitneyOralTraining) == 0)
            {
                functorOnNext = true;

                FirstOralTraining();
            }

            var doFunctor = player.HasCock() ? CockOralTraining() : VaginaOralTraining();

            F.Set(GameFlagIds.WhitneyOralTraining, F.Get(GameFlagIds.WhitneyOralTraining) + 1);

            if (functorOnNext)
                Scene.DoNext(doFunctor);
            else
                doFunctor();
        }

        /// <summary>原版 :1738 cockOralTraining()：按计数选阴茎侧阶段（0 / ≤3 / 4 / ≤9 / 10 / 11 / 其他）。</summary>
        public static Action CockOralTraining()
        {
            var t = F.Get(GameFlagIds.WhitneyOralTraining);

            if (t == 0) return FirstCockOralTraining;
            if (t <= 3) return CockOralTrainingStageOne;
            if (t == 4) return FirstCockOralTrainingStageTwo;
            if (t <= 9) return cockOralTrainingStageTwo;
            if (t == 10) return FirstCockOralTrainingStageThree;
            if (t == 11) return CockOralTrainingStageThree;

            return CockOralTrainingMaxed;
        }

        /// <summary>原版 :1756 vaginaOralTraining()：按计数选阴道侧阶段。</summary>
        public static Action VaginaOralTraining()
        {
            var t = F.Get(GameFlagIds.WhitneyOralTraining);

            if (t == 0) return firstVaginaOralTraining;
            if (t <= 3) return vaginaOralTrainingStageOne;
            if (t == 4) return firstVaginaOralTrainingStageTwo;
            if (t <= 9) return vaginaOralTrainingStageTwo;
            if (t == 10) return firstVaginaOralTrainingStageThree;
            if (t == 11) return vaginaOralTrainingStageThree;

            return vaginaOralTrainingMaxed;
        }

        // ===== 原版 FarmCorruption.hx:2876 firstWhitneyDomPleasure() =====

        public static void firstWhitneyDomPleasure()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Dom.First.L2877".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.First.L2878".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.First.L2879".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.First.L2880".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.First.L2881".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.First.L2882".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.First.L2883".Localized());
        }

        // ===== 原版 FarmCorruption.hx:2886 repeatWhitneyDomPleasure() =====

        public static void repeatWhitneyDomPleasure()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Dom.Repeat.L2887".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Repeat.L2888".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Repeat.L2889".Localized());
        }

        // ===== 原版 FarmCorruption.hx:2893 whitneyDomBondageOral() =====

        public static void whitneyDomBondageOral()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.WhitneyClimbsTopYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YesSheSaysAfter".Localized());

            if (player.IsBiped())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.SheWorksFastShe".Localized());

            if (!(player.IsBiped()) && (player.HasTailInsteadOfLegs()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.ObviousShePutSome".Localized());

            if (!(!(player.IsBiped()) && (player.HasTailInsteadOfLegs())) && (player.IsDrider()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.ObviousShePutSomeThought".Localized());

            if (!(!(!(player.IsBiped()) && (player.HasTailInsteadOfLegs())) && (player.IsDrider())) && (player.IsGoo()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.CanSecureYouProperly".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouHaveMomentSee".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.SheMakesWordlessBestial".Localized());
            // 原版 :2913（按 WhitneyDefurred() || !player.HasFur() 拆分，共 1 处拼接）
            Scene.OutputText(WhitneyDefurred() || !player.HasFur()

                ? "Scenes.FarmCorruption.Dom.BondageOral.ComesHerVoiceWhitney".Localized()

                : "Scenes.FarmCorruption.Dom.BondageOral.Skin".Localized());
            // 原版 :2914（按 WhitneyDefurred() || !player.HasFur() 拆分，共 1 处拼接）
            Scene.OutputText(WhitneyDefurred() || !player.HasFur()

                ? "Scenes.FarmCorruption.Dom.BondageOral.HerThighsRelaxAnd".Localized()

                : "Scenes.FarmCorruption.Dom.BondageOral.Smooth".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.GladGotTheEdge".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HerDrySmoothDigits".Localized());

            if (player.HasCock() && !player.HasVagina() && player.Cocks.Count > 1)
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.ManyThingsPlayWith".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.TheRoughOralYou".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.SheLiftsHerThighs".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.SheSaysTheSame".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouTrappedHereBeing".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YourMasterfulMinistrationsElicit".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HerThighsClenchAlmost".Localized());

            if (player.HasCock() && !player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouPushHerUpwards".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HerDrySmoothDigitsTouch".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.GotManyThingsPlay".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouContinueLapAway".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HerOtherHandCurls".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.SheSaysTheSameLight".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouTrappedHereBeingEdged".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YourMasterfulMinistrationsElicitCoo".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HerThighsClenchAlmostPainfully".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouPushHerUpwardsShe".Localized());

            if (!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.WithTheFierceAction".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HerDrySmoothDigitsFlow".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouContinueLapAwayHer".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.WhilstHerHipsSmother".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.SheSaysTheSameLightTeasing".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouTrappedHereBeingEdgedWith".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YourMasterfulMinistrationsElicitCooFrom".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HerThighsClenchAlmostPainfullyTight".Localized());

            if (!(!(player.HasCock() && !player.HasVagina()) && (player.HasCock() && player.HasVagina())) && (player.HasVagina() && !player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.YouPushHerUpwardsSheFlicks".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.SheThrashesIntoYou".Localized());

            if (player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.Cum".Localized());

            if (!(player.HasCock()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.Femcum".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.AndOnlyAfterRelaxing".Localized());

            if (player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.CockBiggest".Localized());

            if (player.HasCock() && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.And".Localized());

            if (player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.Punctuation".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.AchingOozingDeepPost".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.OnlyAfterTheseEnforced".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.BondageOral.HopeYouFeelSuitably".Localized());

            player.Orgasm("Generic");

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:3083 whitneyDomRide()：被支配方骑乘（含三档尺寸分支）=====

        /// <summary>原版 :113 WhitneyVagCapacity()：Whitney 的阴道容量常量。</summary>
        public static float WhitneyVagCapacity() => 44f;

        /// <summary>原版 :3083 whitneyDomRide()</summary>
        public static void WhitneyDomRide()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.FlickingOffHerUnderwear".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.LieStillSheCommands".Localized());

            // 原版 :3089-3097：按下体形态描述绑缚方式
            if (player.IsBiped())
                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SheDoesTheSame".Localized());
            else if (player.IsDrider())
                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SheHasLongLeather".Localized());
            else if (player.IsGoo())
                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.CanSecureYouProperly".Localized());
            else if (player.HasTailInsteadOfLegs())
                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YourLongHeavyTail".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.OnceSheDoneTying".Localized());

            // 原版 :3099：多眼系的蒙眼描写不同
            Scene.OutputText((player.Body.Get<BodyParts.Vanilla.EyesPart>().Count > 2)

                ? "Scenes.FarmCorruption.Dom.Ride.StopFussingNowSweetheart".Localized()

                : "Scenes.FarmCorruption.Dom.Ride.OnceSheFinishedWith".Localized());

            // 原版 :3100：有睾丸时会额外摩擦
            Scene.OutputText(player.Balls > 0

                ? "Scenes.FarmCorruption.Dom.Ride.SheCrawlsDownYour".Localized()

                : "Scenes.FarmCorruption.Dom.Ride.MakingYouTwitchShe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.HerWarmHandCurls".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YeahHissesWhitneyShe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.ThereYourSlaveMistress".Localized());

            // 原版 :3108-3116：挑一根能进得去的阴茎，挑不到就是「太大」档
            var cockI = player.CockThatFits(WhitneyVagCapacity() * 1.33f) + 1;

            var tooBig = false;

            if (cockI == 0)
            {
                tooBig = true;

                cockI = player.SmallestCockIndex() + 1;
            }

            if (tooBig)
            {
                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YourMouthOpensShe".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YouGroanWithLaugh".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.WhyYouHaveSuch".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.MeanYouWouldnAble".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SomethingSuddenlyPushesAgainst".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YouGritYourTeeth".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YouFeelFullEngorged".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SayingSheBeginsRub".Localized());

                // 原版 :3126：手/爪取决于是否去毛
                Scene.OutputText((WhitneyDefurred() || !player.HasFur())

                    ? "Scenes.FarmCorruption.Dom.Ride.SheQuicklyPicksThe".Localized()

                    : "Scenes.FarmCorruption.Dom.Ride.Feet".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SheFlexesIntoYou".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.WhitneySoppingSexSlides".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.EventuallyYourPetDominatrix".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.TheLightHurtsYour".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.ThatHowWantYou".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SilentlyYouGetAnd".Localized());

                // 原版 :3134：射精计时清零，淫欲 +1
                player.HoursSinceCum = 0;

                player.ApplyStats(new DynStat(GameStats.Libido, 1));
            }
            // 原版 :3138：中等尺寸（超过容量 66% 但塞得进）
            else if (player.CockArea(cockI - 1) > WhitneyVagCapacity() * 0.66f)
            {
                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YourMouthOpensSheBegins".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YouPragmatistArenShe".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.EveryHoleCanUse".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.WithEffortYouElect".Localized());

                // 原版 :3144：泌乳状态下的台词不同
                Scene.OutputText(player.IsLactating()

                    ? "Scenes.FarmCorruption.Dom.Ride.MassiveCentaurCockLil".Localized()

                    : "Scenes.FarmCorruption.Dom.Ride.Moistened".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SheLapsesIntoHarsh".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SheUsesYouPush".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.FinallyAfterEnoughHer".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.ThenToySheWhispers".Localized());
            }
            else
            {
                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.YourMouthOpensSheBeginsRevolve".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.WhyWouldWhoOwns".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SadismYouGetBathe".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SmallWhiteDropPain".Localized());

                // 原版 :3157：泌乳状态下的台词不同
                Scene.OutputText(player.IsLactating()

                    ? "Scenes.FarmCorruption.Dom.Ride.CryForSissyShe".Localized()

                    : "Scenes.FarmCorruption.Dom.Ride.Moistened".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SheLapsesIntoHarshSighs".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.WhateverHerVocalMisgivings".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.FinallyAfterEnoughHerOwn".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.ThenSissySheWhispers".Localized());
            }

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.OnceFinallyOverAnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.ThisDeepPostCoital".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.SheWatchesYouDress".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.Ride.HopeYouFeelSuitably".Localized());
        }

        // ===== 原版 FarmCorruption.hx:2980 whitneyDomStraponDoggy() =====

        public static void WhitneyDomStraponDoggy()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            // 原版 :3037：按有无阴道决定容量口径
            var capacity = player.HasVagina() ? player.VaginalCapacity() : player.AnalCapacity();

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.OnceHerClothesAre".Localized());
            // 原版 :2985（按 !player.IsBiped() 拆分，共 1 处拼接）
            Scene.OutputText(!player.IsBiped()

                ? "Scenes.FarmCorruption.Dom.StraponDoggy.GladYouEnjoyingThe".Localized()

                : "Scenes.FarmCorruption.Dom.StraponDoggy.CloseYouCanWhat".Localized());
            // 原版 :2986（按 player.Body.Get<MonoCoC.Contents.BodyParts.Vanilla.ButtPart>().Rating <= 4 拆分，共 1 处拼接）
            Scene.OutputText(player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating <= 4

                ? "Scenes.FarmCorruption.Dom.StraponDoggy.YouKnowHowHard".Localized()

                : "Scenes.FarmCorruption.Dom.StraponDoggy.TightAss".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.HerHandWithdrawsAnd".Localized());

            if (player.IsBiped() || player.IsTaur())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.NextShePushesYour".Localized());

            if (!(player.IsBiped() || player.IsTaur()) && (player.IsDrider()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YouDonStruggleShe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.SheThenTiesYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.SomethingDerped".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.PositionCompleteSupplicationYour".Localized());

            if (player.HasVagina() && !player.HasCock() && SceneInstance<Npcs.VapulaScene>.Get().VapulaSlave())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.TheWoodenPaddleOne".Localized());

            if (player.HasVagina() && !player.HasCock() && SceneInstance<Npcs.VapulaScene>.Get().VapulaSlave())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YesYourSuccubusWas".Localized());

            if (!(player.HasVagina() && !player.HasCock() && SceneInstance<Npcs.VapulaScene>.Get().VapulaSlave()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.TheWoodenPaddleOneHand".Localized());

            if (!(player.HasVagina() && !player.HasCock() && SceneInstance<Npcs.VapulaScene>.Get().VapulaSlave()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.CanYouGuessWhich".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YouWatchWhitneySlowly".Localized());
            // 原版 :3008（按 player.HasCock() 拆分，共 1 处拼接）
            Scene.OutputText(player.HasCock()

                ? "Scenes.FarmCorruption.Dom.StraponDoggy.OnceSheDoneYou".Localized()

                : "Scenes.FarmCorruption.Dom.StraponDoggy.YourSemiErect".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YourEyesBulgeOpen".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.ShiverRunsDownYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.WhitneyRestsHerThick".Localized());

            if (player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.SlowlyLeadingDownUntil".Localized());

            if (!(player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.TeasingYouWithGently".Localized());

            if (capacity >= 36)
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.TheDogWomanHuffs".Localized());

            if (capacity >= 36 && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.DeepEagerVagina".Localized());

            if (capacity >= 36 && !(player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.OilyWellUsedPassage".Localized());

            if (capacity >= 36)
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.EagerlySwallowingUntilHer".Localized());

            if (capacity >= 36)
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YouStrangerCockAre".Localized());

            if (capacity >= 36 && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.SitAndSpreadFor".Localized());

            if (capacity >= 36 && !(player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.BetYouGetYour".Localized());

            if (capacity >= 36)
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.WhatWouldYourOther".Localized());

            if (capacity >= 36)
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YouLikeRetortWith".Localized());

            if (!(capacity >= 36))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.TheDogWomanWorks".Localized());

            if (!(capacity >= 36) && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.TakeYourTightCunt".Localized());

            if (!(capacity >= 36) && !(player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.GiveYourTightAss".Localized());

            if (!(capacity >= 36))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.PushingTheOtherEnd".Localized());

            if (!(capacity >= 36))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.WhitneyBreathesExultantlyHer".Localized());

            if (!(capacity >= 36) && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.ServantsYouTightBossy".Localized());

            if (!(capacity >= 36) && !(player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.CattleYouTightBossy".Localized());

            if (!(capacity >= 36))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.Punctuation".Localized());

            if (!(capacity >= 36))
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YouJerkShockThe".Localized());
            // 原版 :3066（按 player.HasVagina() 拆分，共 1 处拼接）
            Scene.OutputText(player.HasVagina()

                ? "Scenes.FarmCorruption.Dom.StraponDoggy.SheWithdrawsAlmostAll".Localized()

                : "Scenes.FarmCorruption.Dom.StraponDoggy.EntranceBeforeThrustingBack".Localized());
            // 原版 :3067（按 player.HasCock() 拆分，共 1 处拼接）
            Scene.OutputText(player.HasCock()

                ? "Scenes.FarmCorruption.Dom.StraponDoggy.ThePantiesYourMouth".Localized()

                : "Scenes.FarmCorruption.Dom.StraponDoggy.BumpingDelightfullyAgainstYour".Localized());

            // 原版 :3068（三处不同条件拼接，工具无法合并，按段拼装）
            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.L3068.a".Localized()

                + (player.HasVagina()

                    ? "Scenes.FarmCorruption.Dom.StraponDoggy.L3068.bV".Localized()

                    : "Scenes.FarmCorruption.Dom.StraponDoggy.L3068.bC".Localized())

                + "Scenes.FarmCorruption.Dom.StraponDoggy.L3068.c".Localized()

                + (player.HasVagina()

                    ? "Scenes.FarmCorruption.Dom.StraponDoggy.L3068.dV".Localized()

                    : "")

                + (player.HasCock()

                    ? "Scenes.FarmCorruption.Dom.StraponDoggy.L3068.eC".Localized()

                    : "")

                + "Scenes.FarmCorruption.Dom.StraponDoggy.YourPussyQuiveringAnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.GoesAndTheCreak".Localized());
            // 原版 :3070（按 player.HasVagina() 拆分，共 3 处拼接）
            Scene.OutputText(player.HasVagina()

                ? "Scenes.FarmCorruption.Dom.StraponDoggy.ParagraphBreak".Localized()

                : "Scenes.FarmCorruption.Dom.StraponDoggy.Hand".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.TheRelentlessPoundingForces".Localized());

            if (player.HasVagina() && !player.HasCock() && SceneInstance<Npcs.VapulaScene>.Get().VapulaSlave())
                Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YouFeelTheEnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.LongLastSheComes".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.YouLikeRestLonger".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Dom.StraponDoggy.HopeYouFeelSuitably".Localized());

            player.Orgasm("VaginalAnal");

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:2850 whitneyDomPleasure()：支配向 Pleasure 入口 =====

        /// <summary>原版 :2850 whitneyDomPleasure()：首次播开场，之后随机从可用场景里挑一个。</summary>
        public static void WhitneyDomPleasure()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            // 原版 :2854：首次进入播专属开场
            if (F.Get(GameFlagIds.WhitneyDomFirstPleasure) == 0)
            {
                firstWhitneyDomPleasure();

                F.Set(GameFlagIds.WhitneyDomFirstPleasure, 1);
            }
            else
                repeatWhitneyDomPleasure();

            // 原版 :2861：按体型与性别筛出可用场景
            var scenes = new List<Action>();

            if (!player.IsTaur())
                scenes.Add(() => whitneyDomBondageOral());

            if (player.HasCock() || player.HasVagina())
                scenes.Add(() => WhitneyDomStraponDoggy());

            if (player.HasCock())
                scenes.Add(() => WhitneyDomRide());

            Scene.DoNext(scenes[Random.Shared.Next(scenes.Count)]);
        }

        // ===== 原版 FarmCorruption.hx:3383 getBrandingStuff() =====

        public static void getBrandingStuff()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            // 原版 :3390（按 WhitneyDefurred() || !player.HasFur() 拆分，共 1 处拼接）
            Scene.OutputText(WhitneyDefurred() || !player.HasFur()

                ? "Scenes.FarmCorruption.Branding.GetStuff.L3390.t".Localized()

                : "Scenes.FarmCorruption.Branding.GetStuff.L3390b.f".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.GetStuff.MakeHappenYouMurmur".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:3398 dontGetBrandingStuff() =====

        public static void dontGetBrandingStuff()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Branding.DontGetStuff.L3402".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.DontGetStuff.L3403".Localized());

            if (whitneyDom())
                Scene.OutputText("Scenes.FarmCorruption.Branding.DontGetStuff.L3405".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.DontGetStuff.L3407".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3412 brandingAvailableTalk() =====

        public static void brandingAvailableTalk()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Branding.AvailableTalk.L3416".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.AvailableTalk.L3417".Localized());

            if (WhitneyDefurred() || !player.HasFur())
                Scene.OutputText("Scenes.FarmCorruption.Branding.AvailableTalk.L3419".Localized());

            if (!(WhitneyDefurred() || !player.HasFur()))
                Scene.OutputText("Scenes.FarmCorruption.Branding.AvailableTalk.L3421".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.AvailableTalk.L3423".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.AvailableTalk.L3424".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.AvailableTalk.L3425".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3434 testBranding() =====

        public static void testBranding()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Branding.Test.L3438".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.Test.L3439".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.Test.L3440".Localized());
        }

        // ===== 原版 FarmCorruption.hx:5130 dontTestBranding() =====

        public static void dontTestBranding()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Branding.DontTest.L5134".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.DontTest.L5135".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Branding.DontTest.L5136".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:3777 amilyButtIntro() =====

        public static void amilyButtIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.amilyButtIntro.L3778".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.amilyButtIntro.L3779".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3658 amilyCollarboneIntro() =====

        public static void amilyCollarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.amilyCollarboneIntro.L3659".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3740 amilyLowerBackIntro() =====

        public static void amilyLowerBackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.amilyLowerBackIntro.L3741".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.amilyLowerBackIntro.L3742".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3699 amilyShouldersIntro() =====

        public static void amilyShouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.amilyShouldersIntro.L3700".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.amilyShouldersIntro.L3701".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3809 bigMilkyButtIntro() =====

        public static void bigMilkyButtIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyButtIntro.L3810".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyButtIntro.L3811".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyButtIntro.L3812".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3683 bigMilkyCollarboneIntro() =====

        public static void bigMilkyCollarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyCollarboneIntro.L3684".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyCollarboneIntro.L3685".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyCollarboneIntro.L3686".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3766 bigMilkyLowerBackIntro() =====

        public static void bigMilkyLowerBackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyLowerBackIntro.L3767".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyLowerBackIntro.L3768".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyLowerBackIntro.L3769".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3724 bigMilkyShouldersIntro() =====

        public static void bigMilkyShouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyShouldersIntro.L3725".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyShouldersIntro.L3726".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bigMilkyShouldersIntro.L3727".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3787 bimboSophieButtIntro() =====

        public static void bimboSophieButtIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieButtIntro.L3788".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieButtIntro.L3789".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieButtIntro.L3790".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3667 bimboSophieCollarboneIntro() =====

        public static void bimboSophieCollarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieCollarboneIntro.L3668".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3745 bimboSophieLowerBackIntro() =====

        public static void bimboSophieLowerBackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieLowerBackIntro.L3746".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieLowerBackIntro.L3747".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieLowerBackIntro.L3748".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3704 bimboSophieShouldersIntro() =====

        public static void bimboSophieShouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieShouldersIntro.L3705".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieShouldersIntro.L3706".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieShouldersIntro.L3707".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3772 buttIntro() =====

        public static void buttIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.buttIntro.L3773".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.buttIntro.L3774".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3654 collarboneIntro() =====

        public static void collarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.collarboneIntro.L3655".Localized());
        }

        // ===== 原版 FarmCorruption.hx:5697 happyEndingMerge() =====

        public static void happyEndingMerge()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.happyEndingMerge.L5698".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.happyEndingMerge.L5699".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.happyEndingMerge.L5700".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.happyEndingMerge.L5701".Localized());

            player.Orgasm("Generic");

            player.ApplyStats(new DynStat(GameStats.Sensitivity, -1));

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:3782 jojoButtIntro() =====

        public static void jojoButtIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoButtIntro.L3783".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoButtIntro.L3784".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3662 jojoCollarboneIntro() =====

        public static void jojoCollarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoCollarboneIntro.L3663".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoCollarboneIntro.L3664".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3735 jojoLowerbackIntro() =====

        public static void jojoLowerbackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoLowerbackIntro.L3736".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoLowerbackIntro.L3737".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3694 jojoShouldersIntro() =====

        public static void jojoShouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoShouldersIntro.L3695".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.jojoShouldersIntro.L3696".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3799 kellyButtIntro() =====

        public static void kellyButtIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyButtIntro.L3800".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyButtIntro.L3801".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyButtIntro.L3802".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3675 kellyCollarboneIntro() =====

        public static void kellyCollarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyCollarboneIntro.L3676".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3757 kellyLowerBackIntro() =====

        public static void kellyLowerBackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyLowerBackIntro.L3758".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyLowerBackIntro.L3759".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3715 kellyShouldersIntro() =====

        public static void kellyShouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyShouldersIntro.L3716".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.kellyShouldersIntro.L3717".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3730 lowerbackIntro() =====

        public static void lowerbackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.lowerbackIntro.L3731".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.lowerbackIntro.L3732".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3689 shouldersIntro() =====

        public static void shouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.shouldersIntro.L3690".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.shouldersIntro.L3691".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3805 smallMilkyButtIntro() =====

        public static void smallMilkyButtIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButtIntro.L3806".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3679 smallMilkyCollarboneIntro() =====

        public static void smallMilkyCollarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyCollarboneIntro.L3680".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3762 smallMilkyLowerBackIntro() =====

        public static void smallMilkyLowerBackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyLowerBackIntro.L3763".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3720 smallMilkyShouldersIntro() =====

        public static void smallMilkyShouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyShouldersIntro.L3721".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3815 tattooMerge() =====

        public static void tattooMerge()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.tattooMerge.L3816".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.tattooMerge.L3817".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.tattooMerge.L3818".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:3793 vapulaButtIntro() =====

        public static void vapulaButtIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaButtIntro.L3794".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaButtIntro.L3795".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaButtIntro.L3796".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3671 vapulaCollarboneIntro() =====

        public static void vapulaCollarboneIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaCollarboneIntro.L3672".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3751 vapulaLowerBackIntro() =====

        public static void vapulaLowerBackIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaLowerBackIntro.L3752".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaLowerBackIntro.L3753".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaLowerBackIntro.L3754".Localized());
        }

        // ===== 原版 FarmCorruption.hx:3710 vapulaShouldersIntro() =====

        public static void vapulaShouldersIntro()
        {
            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaShouldersIntro.L3711".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaShouldersIntro.L3712".Localized());
        }

        // ===== 原版 FarmCorruption.hx:4941 amilyBreedingBitchTattoo(slot) =====

        public static void AmilyBreedingBitchTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Breeding Bitch\" tattooed across her ";

            if (slot == 0)
            {
                amilyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.amilyBreedingBitchTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.AmilyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                amilyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.AmilyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                amilyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.AmilyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                amilyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.AmilyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4996 amilyCockGoesHereTattoo(slot) =====

        public static void AmilyCockGoesHereTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Cock Goes Here\" tattooed across her ";

            if (slot == 0)
            {
                amilyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.amilyCockGoesHereTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.AmilyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                amilyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.AmilyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                amilyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.AmilyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                amilyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.AmilyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4171 amilyHeartTattoo(slot) =====

        public static void AmilyHeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on her ";

            if (slot == 0)
            {
                amilyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.amilyHeartTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.AmilyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                amilyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.AmilyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                amilyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.AmilyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                amilyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.AmilyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:5049 amilyMommysGirlTattoo(slot) =====

        public static void AmilyMommysGirlTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Mommy's Girl\" tattooed across her ";

            if (slot == 0)
            {
                amilyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.amilyMommysGirlTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.AmilyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                amilyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.AmilyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                amilyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.AmilyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                amilyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.AmilyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4636 amilyPropertyTattoo(slot) =====

        public static void AmilyPropertyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across her ";

            if (slot == 0)
            {
                amilyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.amilyPropertyTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.AmilyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                amilyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.AmilyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                amilyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.AmilyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                amilyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.AmilyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3851 amilyTribalTattoo(slot) =====

        public static void AmilyTribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across her ";

            if (slot == 0)
            {
                amilyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.amilyTribalTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.AmilyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                amilyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.AmilyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                amilyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.AmilyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                amilyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.AmilyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4097 bigMilkyBathToyTattoo(slot) =====

        public static void BigMilkyBathToyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Bath Toy\" tattooed across her ";

            if (slot == 0)
            {
                bigMilkyCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bigMilkyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bigMilkyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bigMilkyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4133 bigMilkyCockCozyTattoo(slot) =====

        public static void BigMilkyCockCozyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Cock Cozy\" tattooed across her collarbone.";

            bigMilkyCollarboneIntro();

            F.Set(GameFlagIds.MilkyTattooCollarbone, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4045 bigMilkyHeartTattoo(slot) =====

        public static void BigMilkyHeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on her ";

            if (slot == 0)
            {
                bigMilkyCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bigMilkyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bigMilkyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bigMilkyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4123 bigMilkyMegaMilkTattoo(slot) =====

        public static void BigMilkyMegaMilkTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Mega Milk\" tattooed across her collarbone.";

            bigMilkyCollarboneIntro();

            F.Set(GameFlagIds.MilkyTattooCollarbone, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4071 bigMilkyPropertyOfTattoo(slot) =====

        public static void BigMilkyPropertyOfTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across her ";

            if (slot == 0)
            {
                bigMilkyCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bigMilkyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bigMilkyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bigMilkyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4019 bigMilkyTribalTattoo(slot) =====

        public static void BigMilkyTribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across her ";

            if (slot == 0)
            {
                bigMilkyCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bigMilkyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bigMilkyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bigMilkyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4863 bimboSophieBreedingBitchTattoo(slot) =====

        public static void BimboSophieBreedingBitchTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Breeding Bitch\" tattooed across her ";

            if (slot == 0)
            {
                bimboSophieCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieBreedingBitchTattoo.s0.One".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieBreedingBitchTattoo.s0.Two".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.SophieTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bimboSophieShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.SophieTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bimboSophieLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.SophieTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bimboSophieButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.SophieTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4891 bimboSophieCockGoesHereTattoo(slot) =====

        public static void BimboSophieCockGoesHereTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Cock Goes Here\" tattooed across her lower back.";

            bimboSophieLowerBackIntro();

            F.Set(GameFlagIds.SophieTattooLowerback, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4224 bimboSophieHeartTattoo(slot) =====

        public static void BimboSophieHeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on her ";

            if (slot == 0)
            {
                bimboSophieCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieHeartTattoo.s0.One".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieHeartTattoo.s0.Two".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.SophieTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bimboSophieShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.SophieTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bimboSophieLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.SophieTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bimboSophieButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.SophieTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4715 bimboSophiePropertyOfTattoo(slot) =====

        public static void BimboSophiePropertyOfTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across her ";

            if (slot == 0)
            {
                bimboSophieCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophiePropertyOfTattoo.s0.One".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophiePropertyOfTattoo.s0.Two".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.SophieTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bimboSophieShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.SophieTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bimboSophieLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.SophieTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bimboSophieButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.SophieTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4835 bimboSophieSwallowTattoo(slot) =====

        public static void BimboSophieSwallowTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A swallow with its tapering wings in flight across her ";

            if (slot == 0)
            {
                bimboSophieCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieSwallowTattoo.s0.One".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieSwallowTattoo.s0.Two".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.SophieTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bimboSophieShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.SophieTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bimboSophieLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.SophieTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bimboSophieButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.SophieTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3904 bimboSophieTribalTattoo(slot) =====

        public static void BimboSophieTribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across her ";

            if (slot == 0)
            {
                bimboSophieCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieTribalTattoo.s0.One".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.bimboSophieTribalTattoo.s0.Two".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.SophieTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                bimboSophieShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.SophieTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                bimboSophieLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.SophieTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                bimboSophieButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.SophieTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4902 bimboSophieWideLoadTattoo(slot) =====

        public static void BimboSophieWideLoadTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Wide\" tattooed across one butt cheek and \"Load\" tattooed on the other.";

            bimboSophieButtIntro();

            F.Set(GameFlagIds.SophieTattooButt, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4968 champCocksuckerTattoo(slot) =====

        public static void ChampCocksuckerTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Champion Cocksucker\" tattooed across her ";

            if (slot == 0)
            {
                collarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.champCocksuckerTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.WhitneyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                shouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.WhitneyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                lowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.WhitneyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                buttIntro();

                tText += "butt.";

                F.Set(GameFlagIds.WhitneyTattooButt, tText);
            }

            tattooMerge();
        }

        // ===== 原版 FarmCorruption.hx:5102 champPussylickerTattoo(slot) =====

        public static void ChampPussylickerTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Champion Pussylicker\" tattooed across her ";

            if (slot == 0)
            {
                collarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.champPussylickerTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.WhitneyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                shouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.WhitneyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                lowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.WhitneyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                buttIntro();

                tText += "butt.";

                F.Set(GameFlagIds.WhitneyTattooButt, tText);
            }

            tattooMerge();
        }

        // ===== 原版 FarmCorruption.hx:4143 heartTattoo(slot) =====

        public static void HeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on her ";

            if (slot == 0)
            {
                collarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.heartTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.WhitneyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                shouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.WhitneyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                lowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.WhitneyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                buttIntro();

                tText += "butt.";

                F.Set(GameFlagIds.WhitneyTattooButt, tText);
            }

            tattooMerge();
        }

        // ===== 原版 FarmCorruption.hx:5023 jojoCockGoesHereTattoo(slot) =====

        public static void JojoCockGoesHereTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Cock Goes Here\" tattooed across his ";

            if (slot == 0)
            {
                jojoCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.JojoTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                jojoShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.JojoTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                jojoLowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.JojoTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                jojoButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.JojoTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4198 jojoHeartTattoo(slot) =====

        public static void JojoHeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on his ";

            if (slot == 0)
            {
                jojoCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.JojoTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                jojoShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.JojoTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                jojoLowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.JojoTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                jojoButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.JojoTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:5076 jojoMommysBoyTattoo(slot) =====

        public static void JojoMommysBoyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Mommy's Boy\" tattooed across his ";

            if (slot == 0)
            {
                jojoCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.JojoTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                jojoShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.JojoTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                jojoLowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.JojoTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                jojoButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.JojoTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4663 jojoPropertyTattoo(slot) =====

        public static void JojoPropertyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across his ";

            if (slot == 0)
            {
                jojoCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.JojoTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                jojoShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.JojoTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                jojoLowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.JojoTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                jojoButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.JojoTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4689 jojoSissySlutTattoo(slot) =====

        public static void JojoSissySlutTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Sissy Slut\" tattooed across his ";

            if (slot == 0)
            {
                jojoCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.JojoTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                jojoShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.JojoTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                jojoLowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.JojoTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                jojoButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.JojoTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3878 jojoTribalTattoo(slot) =====

        public static void JojoTribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across his ";

            if (slot == 0)
            {
                jojoCollarboneIntro();

                tText += "collarbone.";

                F.Set(GameFlagIds.JojoTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                jojoShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.JojoTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                jojoLowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.JojoTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                jojoButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.JojoTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4581 kellyDickWonTattoo(slot) =====

        public static void KellyDickWonTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"I Fought the Dick And the Dick Won\" tattooed in fine text across her ";

            if (slot == 0)
            {
                kellyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.kellyDickWonTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.KellyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                kellyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.KellyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                kellyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.KellyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                kellyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.KellyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4279 kellyHeartTattoo(slot) =====

        public static void KellyHeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on her ";

            if (slot == 0)
            {
                kellyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.kellyHeartTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.KellyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                kellyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.KellyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                kellyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.KellyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                kellyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.KellyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4516 kellyHorseshoeTattoo(slot) =====

        public static void KellyHorseshoeTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A horseshoe imprinted firmly on each shoulder.";

            kellyShouldersIntro();

            F.Set(GameFlagIds.KellyTattooShoulders, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4554 kellyNo1FillyTattoo(slot) =====

        public static void KellyNo1FillyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"#1 Filly\" tattooed across her ";

            if (slot == 0)
            {
                kellyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.kellyNo1FillyTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.KellyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                kellyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.KellyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                kellyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.KellyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                kellyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.KellyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4527 kellyPropertyOfTattoo(slot) =====

        public static void KellyPropertyOfTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across her ";

            if (slot == 0)
            {
                kellyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.kellyPropertyOfTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.KellyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                kellyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.KellyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                kellyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.KellyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                kellyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.KellyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3959 kellyTribalTattoo(slot) =====

        public static void KellyTribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across her ";

            if (slot == 0)
            {
                kellyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.kellyTribalTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.KellyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                kellyShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.KellyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                kellyLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.KellyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                kellyButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.KellyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4913 no1Tattoo(slot) =====

        public static void No1Tattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"No. 1 Bitch\" tattooed across her ";

            if (slot == 0)
            {
                collarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.no1Tattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.WhitneyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                shouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.WhitneyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                lowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.WhitneyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                buttIntro();

                tText += "butt.";

                F.Set(GameFlagIds.WhitneyTattooButt, tText);
            }

            tattooMerge();
        }

        // ===== 原版 FarmCorruption.hx:4608 propertyTattoo(slot) =====

        public static void PropertyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across her ";

            if (slot == 0)
            {
                collarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.propertyTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.WhitneyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                shouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.WhitneyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                lowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.WhitneyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                buttIntro();

                tText += "butt.";

                F.Set(GameFlagIds.WhitneyTattooButt, tText);
            }

            tattooMerge();
        }

        // ===== 原版 FarmCorruption.hx:4372 smallMilkyBathToyTattoo(slot) =====

        public static void SmallMilkyBathToyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Bath Toy\" tattooed across her ";

            if (slot == 0)
            {
                smallMilkyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyBathToyTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                smallMilkyShouldersIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyBathToyTattoo.s1.Two".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyBathToyTattoo.s1.Three".Localized());

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                smallMilkyLowerBackIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyBathToyTattoo.s2.Four".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyBathToyTattoo.s2.Five".Localized());

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                smallMilkyButtIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyBathToyTattoo.s3.Six".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyBathToyTattoo.s3.Seven".Localized());

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4454 smallMilkyButterflyTattoo(slot) =====

        public static void SmallMilkyButterflyTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A butterfly, its four leaf-like wings in flight, tattooed across her ";

            if (slot == 0)
            {
                smallMilkyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s0.One".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s0.Two".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s0.Three".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s0.Four".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                smallMilkyShouldersIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s1.Five".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s1.Six".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s1.Seven".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s1.Eight".Localized());

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                smallMilkyLowerBackIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s2.Nine".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s2.Ten".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s2.Eleven".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s2.Twelve".Localized());

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                smallMilkyButtIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s3.Thirteen".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s3.Fourteen".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s3.Fifteen".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyButterflyTattoo.s3.Sixteen".Localized());

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4306 smallMilkyHeartTattoo(slot) =====

        public static void SmallMilkyHeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on her ";

            if (slot == 0)
            {
                smallMilkyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyHeartTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                smallMilkyShouldersIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyHeartTattoo.s1.Two".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyHeartTattoo.s1.Three".Localized());

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                smallMilkyLowerBackIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyHeartTattoo.s2.Four".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyHeartTattoo.s2.Five".Localized());

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                smallMilkyButtIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyHeartTattoo.s3.Six".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyHeartTattoo.s3.Seven".Localized());

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4405 smallMilkyMegaMilkTattoo(slot) =====

        public static void SmallMilkyMegaMilkTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Mega Milk\" tattooed across her collarbone.";

            smallMilkyCollarboneIntro();

            Scene.OutputText("Scenes.FarmCorruption.SmallMilkyBathToyTattoo.LaughsInEmbarrassment".Localized());

            F.Set(GameFlagIds.MilkyTattooCollarbone, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4339 smallMilkyPropertyOfTattoo(slot) =====

        public static void SmallMilkyPropertyOfTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across her ";

            if (slot == 0)
            {
                smallMilkyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyPropertyOfTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                smallMilkyShouldersIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyPropertyOfTattoo.s1.Two".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyPropertyOfTattoo.s1.Three".Localized());

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                smallMilkyLowerBackIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyPropertyOfTattoo.s2.Four".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyPropertyOfTattoo.s2.Five".Localized());

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                smallMilkyButtIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyPropertyOfTattoo.s3.Six".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyPropertyOfTattoo.s3.Seven".Localized());

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3986 smallMilkyTribalTattoo(slot) =====

        public static void SmallMilkyTribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across her ";

            if (slot == 0)
            {
                smallMilkyCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyTribalTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.MilkyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                smallMilkyShouldersIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyTribalTattoo.s1.Two".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyTribalTattoo.s1.Three".Localized());

                tText += "shoulders.";

                F.Set(GameFlagIds.MilkyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                smallMilkyLowerBackIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyTribalTattoo.s2.Four".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyTribalTattoo.s2.Five".Localized());

                tText += "lower back.";

                F.Set(GameFlagIds.MilkyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                smallMilkyButtIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyTribalTattoo.s3.Six".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyTribalTattoo.s3.Seven".Localized());

                tText += "butt.";

                F.Set(GameFlagIds.MilkyTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3823 tribalTattoo(slot) =====

        public static void TribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across her ";

            if (slot == 0)
            {
                collarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.tribalTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.WhitneyTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                shouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.WhitneyTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                lowerbackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.WhitneyTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                buttIntro();

                tText += "butt.";

                F.Set(GameFlagIds.WhitneyTattooButt, tText);
            }

            tattooMerge();
        }

        // ===== 原版 FarmCorruption.hx:4797 vapulaButtslutTattoo(slot) =====

        public static void VapulaButtslutTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Buttslut\" tattooed in a red love heart across her lower back.";

            vapulaLowerBackIntro();

            F.Set(GameFlagIds.VapulaTattooLowerback, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:4770 vapulaCumAddictTattoo(slot) =====

        public static void VapulaCumAddictTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Cum Addict\" tattooed across her ";

            if (slot == 0)
            {
                vapulaCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaCumAddictTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.VapulaTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                vapulaShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.VapulaTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                vapulaLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.VapulaTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                vapulaButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.VapulaTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4808 vapulaDildoPolisherTattoo(slot) =====

        public static void VapulaDildoPolisherTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Dildo Polisher\" tattooed across her ";

            if (slot == 0)
            {
                vapulaCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaDildoPolisherTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.VapulaTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                vapulaShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.VapulaTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                vapulaLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.VapulaTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                vapulaButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.VapulaTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4252 vapulaHeartTattoo(slot) =====

        public static void VapulaHeartTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A plump, red love heart tattoo on her ";

            if (slot == 0)
            {
                vapulaCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaHeartTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.VapulaTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                vapulaShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.VapulaTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                vapulaLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.VapulaTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                vapulaButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.VapulaTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:4743 vapulaPropertyOfTattoo(slot) =====

        public static void VapulaPropertyOfTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "\"Property of [Name]\" tattooed across her ";

            if (slot == 0)
            {
                vapulaCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaPropertyOfTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.VapulaTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                vapulaShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.VapulaTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                vapulaLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.VapulaTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                vapulaButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.VapulaTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3932 vapulaTribalTattoo(slot) =====

        public static void VapulaTribalTattoo(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            var tText = "A tribal tattoo, all snaking, erotic lines, across her ";

            if (slot == 0)
            {
                vapulaCollarboneIntro();

                Scene.OutputText("Scenes.FarmCorruption.Tat.vapulaTribalTattoo.s0.One".Localized());

                tText += "collarbone.";

                F.Set(GameFlagIds.VapulaTattooCollarbone, tText);
            }
            else if (slot == 1)
            {
                vapulaShouldersIntro();

                tText += "shoulders.";

                F.Set(GameFlagIds.VapulaTattooShoulders, tText);
            }
            else if (slot == 2)
            {
                vapulaLowerBackIntro();

                tText += "lower back.";

                F.Set(GameFlagIds.VapulaTattooLowerback, tText);
            }
            else if (slot == 3)
            {
                vapulaButtIntro();

                tText += "butt.";

                F.Set(GameFlagIds.VapulaTattooButt, tText);
            }
        }

        // ===== 原版 FarmCorruption.hx:3447-3490：纹身部位选择菜单 =====

        private static readonly string[] TattooSlotNames = ["Collarbone", "Shoulders", "Lower Back", "Butt"];

        /// <summary>原版 :3447 slotSelectMenu()：只列出尚未使用的部位。</summary>
        public static void SlotSelectMenu(string character, Action<int> brandFun)
        {
            var slots = TattooSlots(character);

            Scene.ClearButtons();

            for (var i = 0; i < TattooSlotNames.Length; i++)
                if (!F.Has(slots[i]))
                {
                    var index = i;

                    Scene.AddButton(i, TattooSlotNames[i], () => brandFun(index));
                }
        }

        /// <summary>原版 :3459 brandSlotSelect()</summary>
        public static void BrandSlotSelect() => SlotSelectMenu("whitney", BrandSelect);

        /// <summary>原版 :3463 amilyBrandSlotSelect()</summary>
        public static void AmilyBrandSlotSelect() => SlotSelectMenu("amily", AmilyBrandSelect);

        /// <summary>原版 :3467 jojoBrandSlotSelect()</summary>
        public static void JojoBrandSlotSelect() => SlotSelectMenu("jojo", JojoBrandSelect);

        /// <summary>原版 :3471 bimboSophieSlotSelect()</summary>
        public static void BimboSophieSlotSelect() => SlotSelectMenu("sophie", BimboSophieBrandSelect);

        /// <summary>原版 :3475 vapulaSlotSelect()</summary>
        public static void VapulaSlotSelect() => SlotSelectMenu("vapula", VapulaBrandSelect);

        /// <summary>原版 :3479 kellySlotSelect()</summary>
        public static void KellySlotSelect() => SlotSelectMenu("kelly", KellyBrandSelect);

        /// <summary>原版 :3483 smallMilkySlotSelect()</summary>
        public static void SmallMilkySlotSelect() => SlotSelectMenu("milky", SmallMilkyBrandSelect);

        /// <summary>原版 :3487 bigMilkySlotSelect()</summary>
        public static void BigMilkySlotSelect() => SlotSelectMenu("milky", BigMilkyBrandSelect);

        // ===== 原版 FarmCorruption.hx:3491 brandSelect(slot) =====

        public static void BrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.brandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => TribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => HeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.PropertyOf".Localized(), () => PropertyTattoo(slot));

            Scene.AddButton(3, "Common.Button.NoBitch".Localized(), () => No1Tattoo(slot));

            if (player.HasCock() && WhitneyMaxedOralTraining())
                Scene.AddButton(4, "Common.Button.Cocksucker".Localized(), () => ChampCocksuckerTattoo(slot));

            if (player.HasVagina() && WhitneyMaxedOralTraining())
                Scene.AddButton(5, "Common.Button.Pussylicker".Localized(), () => ChampPussylickerTattoo(slot));

            Scene.AddButton(14, "Common.Button.Back".Localized(), BrandSlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:3512 amilyBrandSelect(slot) =====

        public static void AmilyBrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.amilyBrandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => AmilyTribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => AmilyHeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.PropertyOf".Localized(), () => AmilyPropertyTattoo(slot));

            Scene.AddButton(3, "Common.Button.BreedingBitch".Localized(), () => AmilyBreedingBitchTattoo(slot));

            if (player.HasCock() && slot == 2)
                Scene.AddButton(4, "Common.Button.CockHere".Localized(), () => AmilyCockGoesHereTattoo(slot));

            if (player.HasVagina())
                Scene.AddButton(5, "Common.Button.MommysGirl".Localized(), () => AmilyMommysGirlTattoo(slot));

            Scene.AddButton(14, "Common.Button.Back".Localized(), AmilyBrandSlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:3634 bigMilkyBrandSelect(slot) =====

        public static void BigMilkyBrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.bigMilkyBrandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => BigMilkyTribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => BigMilkyHeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.PropertyOf".Localized(), () => BigMilkyPropertyOfTattoo(slot));

            Scene.AddButton(3, "Common.Button.BathToy".Localized(), () => BigMilkyBathToyTattoo(slot));

            if (slot == 0)
                Scene.AddButton(4, "Common.Button.MegaMilk".Localized(), () => BigMilkyMegaMilkTattoo(slot));

            if (slot == 0 && player.HasCock())
                Scene.AddButton(5, "Common.Button.CockCozy".Localized(), () => BigMilkyCockCozyTattoo(slot));

            Scene.AddButton(14, "Common.Button.Back".Localized(), BigMilkySlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:3552 bimboSophieBrandSelect(slot) =====

        public static void BimboSophieBrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.bimboSophieBrandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => BimboSophieTribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => BimboSophieHeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.Swallow".Localized(), () => BimboSophieSwallowTattoo(slot));

            Scene.AddButton(3, "Common.Button.PropertyOf".Localized(), () => BimboSophiePropertyOfTattoo(slot));

            Scene.AddButton(4, "Common.Button.BreedingBitch".Localized(), () => BimboSophieBreedingBitchTattoo(slot));

            if (slot == 3)
                Scene.AddButton(5, "Common.Button.WideLoad".Localized(), () => BimboSophieWideLoadTattoo(slot));

            if (player.HasCock() && slot == 2)
                Scene.AddButton(6, "Common.Button.CockGoesHere".Localized(), () => BimboSophieCockGoesHereTattoo(slot));

            Scene.AddButton(14, "Common.Button.Back".Localized(), BimboSophieSlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:3532 jojoBrandSelect(slot) =====

        public static void JojoBrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.jojoBrandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => JojoTribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => JojoHeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.PropertyOf".Localized(), () => JojoPropertyTattoo(slot));

            Scene.AddButton(3, "Common.Button.SissySlut".Localized(), () => JojoSissySlutTattoo(slot));

            if (player.HasCock() && slot == 2)
                Scene.AddButton(4, "Common.Button.CockHere".Localized(), () => JojoCockGoesHereTattoo(slot));

            if (player.HasVagina())
                Scene.AddButton(5, "Common.Button.MommysBoy".Localized(), () => JojoMommysBoyTattoo(slot));

            Scene.AddButton(14, "Common.Button.Back".Localized(), JojoBrandSlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:3593 kellyBrandSelect(slot) =====

        public static void KellyBrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.kellyBrandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => KellyTribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => KellyHeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.PropertyOf".Localized(), () => KellyPropertyOfTattoo(slot));

            Scene.AddButton(3, "Common.Button.NoFilly".Localized(), () => KellyNo1FillyTattoo(slot));

            if (Settings.ModeSettings.Instance.Silly)
                Scene.AddButton(4, "Common.Button.DickWon".Localized(), () => KellyDickWonTattoo(slot));

            if (slot == 1)
                Scene.AddButton(5, "Common.Button.Horseshoe".Localized(), () => KellyHorseshoeTattoo(slot));

            Scene.AddButton(14, "Common.Button.Back".Localized(), KellySlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:3613 smallMilkyBrandSelect(slot) =====

        public static void SmallMilkyBrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.smallMilkyBrandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => SmallMilkyTribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => SmallMilkyHeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.Butterfly".Localized(), () => SmallMilkyButterflyTattoo(slot));

            Scene.AddButton(3, "Common.Button.PropertyOf".Localized(), () => SmallMilkyPropertyOfTattoo(slot));

            Scene.AddButton(4, "Common.Button.BathToy".Localized(), () => SmallMilkyBathToyTattoo(slot));

            if (slot == 0)
                Scene.AddButton(5, "Common.Button.MegaMilk".Localized(), () => SmallMilkyMegaMilkTattoo(slot));

            if (slot == 0 && player.HasCock())
                Scene.AddButton(6, "Common.Button.CockCozy".Localized(), SmallMilkyCockCozyTattoo);

            Scene.AddButton(14, "Common.Button.Back".Localized(), SmallMilkySlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:3573 vapulaBrandSelect(slot) =====

        public static void VapulaBrandSelect(int slot)
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.BrandSelect.vapulaBrandSelect.Title".Localized(TattooSlotNames[slot]));

            Scene.ClearButtons();

            Scene.AddButton(0, "Common.Button.Tribal".Localized(), () => VapulaTribalTattoo(slot));

            Scene.AddButton(1, "Common.Button.Heart".Localized(), () => VapulaHeartTattoo(slot));

            Scene.AddButton(2, "Common.Button.PropertyOf".Localized(), () => VapulaPropertyOfTattoo(slot));

            Scene.AddButton(3, "Common.Button.CumAddict".Localized(), () => VapulaCumAddictTattoo(slot));

            if (slot == 2 || slot == 3)
                Scene.AddButton(4, "Perks.Buttslut.Name".Localized(), () => VapulaButtslutTattoo(slot));

            if (!player.HasCock())
                Scene.AddButton(5, "Common.Button.DildoPolisher".Localized(), () => VapulaDildoPolisherTattoo(slot));

            Scene.AddButton(14, "Common.Button.Back".Localized(), VapulaSlotSelect);
        }

        // ===== 原版 FarmCorruption.hx:4417 smallMilkyCockCozyTattoo()：乳奴锁骨纹身（无 slot 参数）=====

        public static void SmallMilkyCockCozyTattoo()
        {
            Scene.ClearOutput();

            var tText = "\"Cock Cozy\" tattooed across her collarbone.";

            smallMilkyCollarboneIntro();

            Scene.OutputText("Scenes.FarmCorruption.Tat.smallMilkyCockCozyTattoo.One".Localized());

            F.Set(GameFlagIds.MilkyTattooCollarbone, tText);

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:3277 brandWhitney()：品牌烙印入口 =====

        public static void BrandWhitney()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandWhitney.One".Localized());

            player.Gems -= 50;

            BrandSlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3289 brandAmily()：品牌烙印入口 =====

        public static void BrandAmily()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandAmily.Two".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandAmily.Three".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandAmily.Four".Localized());

            Scene.OutputText(player.HasCock()

                ? "Scenes.FarmCorruption.Brand.BrandAmily.Five".Localized()

                : player.HasVagina() ? "Scenes.FarmCorruption.Brand.BrandAmily.Six".Localized() : "Scenes.FarmCorruption.Brand.BrandAmily.Seven".Localized());

            player.Gems -= 50;

            AmilyBrandSlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3309 brandJojo()：品牌烙印入口 =====

        public static void BrandJojo()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandJojo.Eight".Localized());

            player.Gems -= 50;

            JojoBrandSlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3320 brandBimboSophie()：品牌烙印入口 =====

        public static void BrandBimboSophie()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBimboSophie.Nine".Localized());

            if (!hasTattoo("sophie"))
            {
                Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBimboSophie.Ten".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBimboSophie.Eleven".Localized());
            }
            else
            {
                Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBimboSophie.Twelve".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBimboSophie.Thirteen".Localized());
            }

            player.Gems -= 50;

            BimboSophieSlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3339 brandVapula()：品牌烙印入口 =====

        public static void BrandVapula()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandVapula.Fourteen".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandVapula.Fifteen".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandVapula.Sixteen".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandVapula.Seventeen".Localized());

            player.Gems -= 50;

            VapulaSlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3351 brandKelly()：品牌烙印入口 =====

        public static void BrandKelly()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandKelly.Eighteen".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandKelly.Nineteen".Localized());

            player.Gems -= 50;

            KellySlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3361 brandSmallMilky()：品牌烙印入口 =====

        public static void BrandSmallMilky()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandSmallMilky.Twenty".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandSmallMilky.TwentyOne".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandSmallMilky.TwentyTwo".Localized());

            player.Gems -= 50;

            SmallMilkySlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3372 brandBigMilky()：品牌烙印入口 =====

        public static void BrandBigMilky()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBigMilky.TwentyThree".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBigMilky.TwentyFour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Brand.BrandBigMilky.BathTimeReply".Localized());

            player.Gems -= 50;

            BigMilkySlotSelect();
        }

        // ===== 原版 FarmCorruption.hx:3177 brandingMenu()：品牌烙印总菜单 =====

        public static void BrandingMenu()
        {
            var player = CoC.Instance.Player;

            // 原版 :3179：尚未解锁时先做一次解锁对话
            if (F.Get(GameFlagIds.FarmCorruptionBrandingMenuUnlocked) == 0)
            {
                Scene.ClearOutput();

                Scene.SpriteSelect("whitney");

                Scene.OutputText("Scenes.FarmCorruption.Branding.Unlock1".Localized());

                Scene.OutputText("Scenes.FarmCorruption.Branding.Unlock2".Localized());

                Scene.ClearButtons();

                if (player.Gems >= 500)
                    Scene.AddButton(0, "Common.Button.DoIt".Localized(), getBrandingStuff);
                else
                    Scene.AddButton(0, "Common.Button.DoIt".Localized(), dontGetBrandingStuff);

                Scene.AddButton(1, "Common.Button.No".Localized(), dontGetBrandingStuff);

                return;
            }

            Scene.ClearOutput();

            Scene.OutputText("Scenes.FarmCorruption.Branding.MenuIntro".Localized());

            if (player.Gems < 50)
                Scene.OutputText("Scenes.FarmCorruption.Branding.NotEnoughGems".Localized());

            if (player.Gems >= 50)
            {
                if (hasFreeTattooSlot("whitney"))
                    Scene.AddButton(0, "Common.Button.Whitney".Localized(), BrandWhitney);

                if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1 && hasFreeTattooSlot("amily"))
                    Scene.AddButton(1, "Common.Button.Amily".Localized(), BrandAmily);

                if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1 && hasFreeTattooSlot("jojo"))
                    Scene.AddButton(2, "Common.Button.Jojo".Localized(), BrandJojo);

                if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && BimboSophie() && hasFreeTattooSlot("sophie"))
                    Scene.AddButton(3, "Common.Button.Sophie".Localized(), BrandBimboSophie);

                if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1 && hasFreeTattooSlot("vapula"))
                    Scene.AddButton(4, "Enemies.VapulaFight.Name".Localized(), BrandVapula);

                if (F.Get(GameFlagIds.KeltBreakLevel) >= 4 && hasFreeTattooSlot("kelly"))
                    Scene.AddButton(5, "Common.Button.Kelly".Localized(), BrandKelly);

                if (F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1 && F.Get(GameFlagIds.MilkSize) > 0
                    && hasFreeTattooSlot("milky"))

                    Scene.AddButton(6, SceneInstance<Npcs.MilkWaifuScene>.Get().milkName, BrandSmallMilky);

                if (F.Get(GameFlagIds.FollowerAtFarmBathGirl) == 1 && F.Get(GameFlagIds.MilkSize) == 0
                    && hasFreeTattooSlot("milky"))

                    Scene.AddButton(6, SceneInstance<Npcs.MilkWaifuScene>.Get().milkName, BrandBigMilky);
            }

            Scene.AddButton(14, "Common.Button.Back".Localized(), () => DogeCorruptedMissionComplete());
        }

        // ===== 原版 FarmCorruption.hx:1622 makeDogeSubby() =====

        public static void makeDogeSubby()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1626".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1627".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1628".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1629".Localized());

            if (player.BiggestBreastSize() >= 2)
                Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1632".Localized());

            if (player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1636".Localized());

            if (player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1640".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1642".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1643".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1644".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1645".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Subby.L1646".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:1651 makeDogeDommy() =====

        public static void makeDogeDommy()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YouAgainRetrieveYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YouLikedDoingThat".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YouTalkYouStop".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.HerBreathComesHarder".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.GoodThatCameAlong".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YesSheBreathesHer".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.GoodBecauseHaveUse".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.SheSaysGrittingHer".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.WillLetYouBecome".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YesYouSayNothing".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YouWhisperYourMaster".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YesShePantsThe".Localized());
            // 原版 :1667 —— 未能自动展开，需人工："Common.Text.PgSayGoodYouSayQuietly".Localized() + player.mf("masters", "mistresses") + " need some R and R from t
            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.YouBrokenSomethingOpen".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.LaterThereWillEndless".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.Dommy.OnceTheDogGirl".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:895 keltAChangeInManagement() =====

        public static void keltAChangeInManagement()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Doge.KeltChange.L898".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.KeltChange.L899".Localized());

            if (player.Balls == 0)
                Scene.OutputText("Scenes.FarmCorruption.Doge.KeltChange.L901".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.KeltChange.L903".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.KeltChange.L904".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.KeltChange.L905".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Doge.KeltChange.L906".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:5382 wantOrgyRoom() =====

        public static void wantOrgyRoom()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Want.SaysWhitneyHaltinglyAfter".Localized());
            // 原版 :5387 —— 未能自动展开，需人工："[pg]You move your [legs] restlessly when the silence drags out a bit. [say: Well, understand I 'kin only talk as a farmer. But if I got a p
            Scene.OutputText("Scenes.FarmCorruption.Orgy.Want.YouLookHerShrewdly".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Want.WantMakeReallyGood".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Want.Gems".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Want.CouldConvertFrontRoom".Localized());
        }

        // ===== 原版 FarmCorruption.hx:5405 getOrgyRoom() =====

        public static void getOrgyRoom()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Get.L5409".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Get.L5410".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:5420 noOrgyRoomPlz() =====

        public static void noOrgyRoomPlz()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Orgy.No.L5424".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.No.L5425".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:5432 orgyRoomTalk() =====

        public static void orgyRoomTalk()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5436".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5437".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5438".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5439".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5440".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5441".Localized());

            if (whitneyDom())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5443".Localized());

            if (!(whitneyDom()))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5445".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5447".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5448".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5449".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5450".Localized());

            if (!whitneyDom())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5452".Localized());

            if (!(!whitneyDom()))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5454".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5456".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5457".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5458".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Talk.L5459".Localized());

            Scene.DoNext(() => Scene.ReturnToCamp(1));
        }

        // ===== 原版 FarmCorruption.hx:5601 subHappyEndingMale() =====

        public static void subHappyEndingMale()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            // 原版 :5610（按 WhitneyDefurred() || !player.HasFur() 拆分，共 1 处拼接）
            Scene.OutputText(WhitneyDefurred() || !player.HasFur()

                ? "Scenes.FarmCorruption.Orgy.HappyMale.YouDriftAcrossAnd".Localized()

                : "Scenes.FarmCorruption.Orgy.HappyMale.Smooth".Localized());
            // 原版 :5611（标识符插值：cockThatFits）
            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyMale.YouSlideYourCock".Localized(player.CockThatFits(WhitneyVagCapacity() * 1.33f) + 1));
            // 原版 :5613（标识符插值：cockThatFits）
            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyMale.YourCockBiggestUsed".Localized(player.CockThatFits(WhitneyVagCapacity() * 1.33f) + 1));
        }

        // ===== 原版 FarmCorruption.hx:5654 subHappyEndingFemale() =====

        public static void subHappyEndingFemale()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            // 原版 :5658（按 WhitneyDefurred() || !player.HasFur() 拆分，共 1 处拼接）
            Scene.OutputText(WhitneyDefurred() || !player.HasFur()

                ? "Scenes.FarmCorruption.Orgy.HappyFemale.YouDriftAcrossAnd".Localized()

                : "Scenes.FarmCorruption.Orgy.HappyFemale.Smooth".Localized());

            if (player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.YourCockBiggestUsed".Localized());

            if (F.Get(GameFlagIds.MassageHappyEndings) == 0)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.BeenWhileTakeYou".Localized());

            if (F.Get(GameFlagIds.MassageHappyEndings) == 0)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.NeverNeverWithAnother".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.SheGaspsYouClench".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.BreatheTimeWithYou".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.TheSensationSlidingAgainst".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.WhitneyBreathCatchesHer".Localized());

            if (player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.CockBiggest".Localized());

            if (!(player.HasCock()) && (player.IsBiped()))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.AgainstYour".Localized());

            if (!(!(player.HasCock()) && (player.IsBiped())))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.AroundYourBack".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.MomentarilyYouGrinAnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.WhitneyDoesHerBest".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.TheBreathingTeaseQuickly".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.YouHoldHerTight".Localized());

            if (player.Wetness() >= 5)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.YourPussySpurtsExcitement".Localized());

            if (player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.HappyFemale.WhitneyHasStrokedAnd".Localized());
        }

        // ===== 原版 FarmCorruption.hx:14 / :113 / :5365：零散助手 =====

        /// <summary>原版 :14 whitneySprite()：委托给 FarmScene 选立绘。</summary>
        public static void whitneySprite() => Scene.SpriteSelect("whitney");

        /// <summary>原版 :113 whitneyCockArea()：常量 20（原版注释说明来自 Vapula 的假阳具参数）。</summary>
        public static float whitneyCockArea() => 10 * 2;

        /// <summary>原版 :5365 orgyRoomRouter()：按兽交房状态往当前按钮列表追加 Massage 入口。</summary>
        public static void OrgyRoomRouter()
        {
            if (whitneyDom())
                return;

            var player = CoC.Instance.Player;

            var orgyRoom = F.Get(GameFlagIds.FarmUpgradesOrgyroom);

            // 原版 :5369：已建成时可按摩（可变形体型除外）
            // 原版 :5369：建成后可按摩（可变形体型除外）
            if (orgyRoom == 1 && !player.IsGoo() && !player.IsTaur() && !player.IsGenderless())
                Scene.AddButton(4, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), orgyRoomSubMassage);

            // 原版 :5374：未排队升级时显示「想建一个」
            if (orgyRoom == 0 && F.Get(GameFlagIds.QueueOrgyroomUpgrade) == 0)
                Scene.AddButton(4, "Scenes.Kitsune.KitsuneDrinking.ButtonMassage".Localized(), wantOrgyRoom);
        }

        // ===== 原版 FarmCorruption.hx:5464 orgyRoomSubMassage() =====

        public static void orgyRoomSubMassage()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouSayGrandWearied".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.OnceWhitneyEyesFlick".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.WhitneyHasGotThe".Localized());

            if (player.IsBiped())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouClamberOntoThe".Localized());

            if (!(player.IsBiped()) && (player.IsDrider()))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouTickTackOver".Localized());

            if (!(!(player.IsBiped()) && (player.IsDrider())) && (player.HasTailInsteadOfLegs()))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouSlitherOverAnd".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.Punctuation".Localized());
            // 原版 :5479（按 WhitneyDefurred() || !player.HasFur() 拆分，共 1 处拼接）
            Scene.OutputText(WhitneyDefurred() || !player.HasFur()

                ? "Scenes.FarmCorruption.Orgy.Massage.ThereTheSoundCloth".Localized()

                : "Scenes.FarmCorruption.Orgy.Massage.Smooth".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.SheSighsTheSound".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouBreatheAllThe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.HerHandsMoveYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.OnceYourUpperBody".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouAreAllBut".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.TheEroticTensionBuilds".Localized());

            if (player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating >= 6)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.Soft".Localized());

            if (!(player.Body.Get<BodyParts.Vanilla.ButtPart>().Rating >= 6))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.Brawny".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.RearAfterSheFinished".Localized());

            if (player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.GripsYour".Localized());

            if (!(player.HasCock()) && (player.HasVagina()))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.TracesTheEntranceYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.ForMomentBeforeSliding".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.TheThickerFleshHere".Localized());

            if (player.IsBiped())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.SheSpendsShortTime".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YourMouthOpensTwinge".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony) && player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.CockThicken".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony) && player.HasCock() && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.AndYour".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony) && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.PussyMoisten".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.Punctuation".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.ThereMustDirectNerve".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony) && player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.DemonicClaws || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Lizard || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Harpy || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Dragon)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.Claws".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony) && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.DemonicClaws || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Lizard || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Harpy || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Dragon))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.Toes".Localized());

            if (player.IsBiped() && !(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.BeforeReturningDeliberatelyStroke".Localized());

            if (player.IsBiped() && !(!(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony)))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.SheSpendsShortTimeWith".Localized());

            if (player.IsBiped() && !(!(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony)) && player.HasCock())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.CockThicken".Localized());

            if (player.IsBiped() && !(!(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony)) && player.HasCock() && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.AndYour".Localized());

            if (player.IsBiped() && !(!(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony)) && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.PussyMoisten".Localized());

            if (player.IsBiped() && !(!(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony)))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.Punctuation".Localized());

            if (player.IsBiped() && !(!(player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Hoofed || player.HasTailInsteadOfLegs() || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Goo || player.Body.Get<BodyParts.Vanilla.LowerBodyPart>().Id == BodyPartTypes.Pony)))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.ThereMustDirectNerveLink".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouDidnReallyExpect".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.JustFromWatchingYou".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouWatchLotThen".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.SomethingLikeThatWhitney".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouDidnReallyExpectEven".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.JustFromWatchingYouShe".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouWatchLotThenSilence".Localized());

            if (player.Body.Get<BodyParts.Vanilla.TonguePart>().Id == BodyPartTypes.Snake)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouGrinFlickingYour".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.ImagineWatchingYourMove".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.SomethingLikeThatWhitneyWhispers".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.WithOneLastGlide".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.WasThatGoodFor".Localized());

            if (player.Lust >= 33)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.TheHotTubBubbles".Localized());

            if (!(player.Lust >= 33))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.Massage.YouPlayWithThe".Localized());
        }

        // ===== 原版 FarmCorruption.hx:5572 orgyRoomSubMassageHappyEnding() =====

        public static void orgyRoomSubMassageHappyEnding()
        {
            var player = CoC.Instance.Player;

            Scene.ClearOutput();

            Scene.SpriteSelect("whitney");

            Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5576".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5577".Localized());

            if (player.Tallness >= 67)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5579".Localized());

            if (player.Tallness >= 67)
                Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5580".Localized());

            if (!(player.Tallness >= 67))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5582".Localized());

            if (!(player.Tallness >= 67))
                Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5583".Localized());

            Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5585".Localized());

            if (player.HasCock() && player.CockThatFits(WhitneyVagCapacity() * 1.33f) != -1 && player.HasVagina())
                Scene.OutputText("Scenes.FarmCorruption.Orgy.MassageHappy.L5588".Localized());

            // 原版 :5591-5598：按可用器官给出 Male / Female 两个结算入口
            Scene.ClearButtons();

            if (player.CockThatFits(WhitneyVagCapacity() * 1.33f) != -1)
                Scene.AddButton(0, "Scenes.Aiko.AikoE1SexPart.ButtonMale".Localized(), subHappyEndingMale);

            if (player.HasVagina())
                Scene.AddButton(1, "Scenes.Aiko.AikoE1SexPart.ButtonFemale".Localized(), subHappyEndingFemale);
        }

        // ===== 原版 FarmCorruption.hx:132 updateFarmCorruption()：农场每日结算（由生成器转换）=====

        public static int UpdateFarmCorruption()
        {
            var player = CoC.Instance.Player;

            if (F.Get(GameFlagIds.FarmCorruptionStarted) <= 0)
            return F.Get(GameFlagIds.WhitneyCorruption);

            if (F.Get(GameFlagIds.WhitneyDisabledForDay) == 1)
            F.Set(GameFlagIds.WhitneyDisabledForDay, 2);

            if (F.Get(GameFlagIds.QueueBreastmilkerUpgrade) != 0 && !player.HasKeyItem("Breast Milker - Installed At Whitney's Farm"))
            {
            F.Set(GameFlagIds.QueueBreastmilkerUpgrade, 0);

            player.CreateKeyItem("Breast Milker - Installed At Whitney's Farm");
            }

            if (F.Get(GameFlagIds.QueueCockmilkerUpgrade) != 0 && !player.HasKeyItem("Cock Milker - Installed At Whitney's Farm"))
            {
            F.Set(GameFlagIds.QueueCockmilkerUpgrade, 0);

            player.CreateKeyItem("Cock Milker - Installed At Whitney's Farm");
            }

            if (F.Get(GameFlagIds.QueueContraceptiveUpgrade) != 0)
            {
            F.Set(GameFlagIds.QueueContraceptiveUpgrade, F.Get(GameFlagIds.QueueContraceptiveUpgrade) + (1));

            if (F.Get(GameFlagIds.QueueContraceptiveUpgrade) > 7)
            {
            F.Set(GameFlagIds.FarmUpgradesContraceptive, 1);

            F.Set(GameFlagIds.QueueContraceptiveUpgrade, 0);
            }
            }

            if (F.Get(GameFlagIds.QueueRefineryUpgrade) != 0)
            {
            F.Set(GameFlagIds.QueueRefineryUpgrade, F.Get(GameFlagIds.QueueRefineryUpgrade) + (1));

            if (F.Get(GameFlagIds.QueueRefineryUpgrade) > 3)
            {
            F.Set(GameFlagIds.FarmUpgradesRefinery, 1);

            F.Set(GameFlagIds.QueueRefineryUpgrade, 0);
            }
            }

            if (F.Get(GameFlagIds.QueueMilktankUpgrade) != 0)
            {
            F.Set(GameFlagIds.QueueMilktankUpgrade, F.Get(GameFlagIds.QueueMilktankUpgrade) + (1));

            if (F.Get(GameFlagIds.QueueMilktankUpgrade) > 3)
            {
            F.Set(GameFlagIds.QueueMilktankUpgrade, 0);

            F.Set(GameFlagIds.FarmUpgradesMilktank, 1);

            F.Set(GameFlagIds.FollowerAtFarmBathGirl, 1);

            F.Set(GameFlagIds.MilkSize, 0);
            }
            }

            if (F.Get(GameFlagIds.QueueBrandingUpgrade) != 0)
            {
            F.Set(GameFlagIds.QueueBrandingUpgrade, F.Get(GameFlagIds.QueueBrandingUpgrade) + (1));

            if (F.Get(GameFlagIds.QueueBrandingUpgrade) > 2)
            {
            F.Set(GameFlagIds.QueueBrandingUpgrade, 0);

            F.Set(GameFlagIds.FarmCorruptionBrandingMenuUnlocked, 1);

            F.Set(GameFlagIds.QueueBrandingAvailableTalk, 1);
            }
            }

            if (F.Get(GameFlagIds.QueueOrgyroomUpgrade) != 0)
            {
            F.Set(GameFlagIds.QueueOrgyroomUpgrade, F.Get(GameFlagIds.QueueOrgyroomUpgrade) + (1));

            if (F.Get(GameFlagIds.QueueOrgyroomUpgrade) > 7)
            {
            F.Set(GameFlagIds.QueueOrgyroomUpgrade, 0);

            F.Set(GameFlagIds.FarmUpgradesOrgyroom, 2);
            }
            }

            var modValue = -1;

            if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1)
            modValue += 2;

            if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1)
            modValue += 2;

            if (F.Get(GameFlagIds.FollowerAtFarmSophie) == 1 && BimboSophie())
            modValue += 1;

            if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1)
            modValue += 4;

            if (F.Get(GameFlagIds.FollowerAtFarmLatexy) == 1)
            modValue += 1;

            if (F.Get(GameFlagIds.FollowerAtFarmHolli) > 0)
            modValue += 4;

            if (F.Get(GameFlagIds.FollowerAtFarmKelly) == 1)
            modValue += 2;

            whitneyCorruption(modValue);

            F.Set(GameFlagIds.FarmCorruptionGemsWaiting, F.Get(GameFlagIds.FarmCorruptionGemsWaiting) + (FarmValue()));

            if (F.Get(GameFlagIds.FollowerAtFarmAmily) == 1 && F.Get(GameFlagIds.FollowerProductionAmily) == 1 && F.Get(GameFlagIds.FarmUpgradesRefinery) == 1)
            {
            if (F.Get(GameFlagIds.FollowerAtFarmAmilyGibsMilk) == 0)
            F.Set(GameFlagIds.FollowerAtFarmAmilyGibsMilk, 1);

            F.Set(GameFlagIds.FarmSuccumilkStored, F.Get(GameFlagIds.FarmSuccumilkStored) + (1));
            }

            if (F.Get(GameFlagIds.FollowerAtFarmJojo) == 1 && F.Get(GameFlagIds.FollowerProductionJojo) == 1 && F.Get(GameFlagIds.FarmUpgradesRefinery) == 1)
            {
            if (F.Get(GameFlagIds.FollowerAtFarmJojoGibsDraft) == 0)
            F.Set(GameFlagIds.FollowerAtFarmJojoGibsDraft, 1);

            F.Set(GameFlagIds.FarmIncudraftStored, F.Get(GameFlagIds.FarmIncudraftStored) + (1));

            if (SceneInstance<Npcs.JojoScene>.Get().TentacleJojo())
            F.Set(GameFlagIds.FarmIncudraftStored, F.Get(GameFlagIds.FarmIncudraftStored) + (1));
            }

            if ((F.Get(GameFlagIds.FollowerAtFarmSophie) > 0 && F.Get(GameFlagIds.FollowerProductionSophie) == 1) && (F.Get(GameFlagIds.FarmEggStored) == 0))
            {
            F.Set(GameFlagIds.FarmEggCountdown, F.Get(GameFlagIds.FarmEggCountdown) - 1);

            if (F.Get(GameFlagIds.FarmEggCountdown) == 0)
            {
            F.Set(GameFlagIds.FarmEggStored, 1);

            F.Set(GameFlagIds.FarmEggCountdown, 7);
            }
            }

            if (F.Get(GameFlagIds.FollowerAtFarmVapula) == 1 && F.Get(GameFlagIds.FollowerProductionVapula) == 1)
            {
            F.Set(GameFlagIds.FarmSuccumilkStored, F.Get(GameFlagIds.FarmSuccumilkStored) + (1));

            if (F.Get(GameFlagIds.FarmUpgradesRefinery) == 1)
            F.Set(GameFlagIds.FarmSuccumilkStored, F.Get(GameFlagIds.FarmSuccumilkStored) + (1));

            if (F.Get(GameFlagIds.FollowerAtFarmVapulaGibsMilk) == 0)
            F.Set(GameFlagIds.FollowerAtFarmVapulaGibsMilk, 1);
            }

            if (F.Get(GameFlagIds.FarmSuccumilkStored) > 5)
            {
            F.Set(GameFlagIds.FarmCorruptionGemsWaiting, F.Get(GameFlagIds.FarmCorruptionGemsWaiting) + (F.Get(GameFlagIds.FarmSuccumilkStored) - 5) * 3); // 3 extra gems per succumilk

            F.Set(GameFlagIds.FarmSuccumilkStored, 5);
            }

            if (F.Get(GameFlagIds.FarmIncudraftStored) > 5)
            {
            F.Set(GameFlagIds.FarmCorruptionGemsWaiting, F.Get(GameFlagIds.FarmCorruptionGemsWaiting) + (F.Get(GameFlagIds.FarmIncudraftStored) - 5) * 5); // 5 extra gems per incudraft (less of them produced etc.)

            F.Set(GameFlagIds.FarmIncudraftStored, 5);
            }

            if (F.Get(GameFlagIds.FollowerAtFarmLatexy) == 1)
            {
                SceneInstance<Npcs.LatexGirlScene>.Get().GooHappiness(-0.5f, false);

                SceneInstance<Npcs.LatexGirlScene>.Get().GooObedience(0.5f, false);
            }

            if (F.Get(GameFlagIds.FollowerAtFarmCeraph) > 0)
            {
            F.Set(GameFlagIds.FollowerAtFarmCeraph, F.Get(GameFlagIds.FollowerAtFarmCeraph) + (1));

            if (F.Get(GameFlagIds.FollowerAtFarmCeraph) > 7)
            F.Set(GameFlagIds.FollowerAtFarmCeraph, -1);
            }

            if (F.Get(GameFlagIds.FollowerAtFarmHolli) > 0)
            {
            F.Set(GameFlagIds.FollowerAtFarmHolli, F.Get(GameFlagIds.FollowerAtFarmHolli) + (1));

            if (F.Get(GameFlagIds.FollowerAtFarmHolli) > 20)
            F.Set(GameFlagIds.FollowerAtFarmHolli, -1);
            }

            if ((F.Get(GameFlagIds.FarmUpgradesContraceptive) == 1) && (F.Get(GameFlagIds.FarmContraceptiveStored) == 0))
            F.Set(GameFlagIds.FarmContraceptiveStored, 1);

            F.Set(GameFlagIds.FarmCorruptionDaysSinceLastPayout, F.Get(GameFlagIds.FarmCorruptionDaysSinceLastPayout) + (1));

            return modValue;
        }
    }
}