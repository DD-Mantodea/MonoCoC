using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes.scenes.dungeons.lethicesKeep.Lethice extends Monster（Lethice.hx）
    // 招式见 Contents/Combat/VanillaHandlers/LethiceCombatHandlers.cs。
    // 原版是三阶段 Boss（1 本体 → 2 恶魔群 → 3 狂暴 Lethice），阶段内的选招/数值在下面；
    // 阶段切换走 ResolveDefeated → Continue（Lethice.hx:108-120），阶段 2 的三选一菜单由
    // CustomCombatMenu 接管（Lethice.hx:528-535，Button 派发见 CombatController.Menu）。
    [Register("coc:lethice")]
    public class LethiceEnemy : Enemy
    {
        // 原版实例字段（Lethice.hx:9-17）
        public int RoundCount;

        public int FightPhase = 1;

        public bool WingsDestroyed;

        public int DefMode;

        public int HpLastTurn;

        public int LustLastTurn;

        // 阶段 2 战败后的「战斗中途菜单」状态（原版 Lethice.hx:520-536 / 566-573 / 691-697）：
        //   Top         = 三选一（DemonFuck / Heal / Next，Lethice.hx:528-535）
        //   DemonFuckSub= 插曲子菜单（OralFinish / FuckDemon / RideCock，Lethice.hx:566-573）
        //   Phase3Next  = beginPhase3(true) 之后的「Next」（Lethice.hx:691-693）
        public enum Phase2MenuState
        {
            None,
            Top,
            DemonFuckSub,
            Phase3Next
        }

        public Phase2MenuState Phase2Menu;

        // 原版 phase2Ends(hpVictory) 把 hpVictory 绑进 p2DemonFuck（Lethice.hx:530/538），
        // 这里存下来给插曲用。
        public bool P2VictoryWasHp;

        // 原版里只有这一战会用到的状态（Haxe src 未在别处引用），本地声明。
        // 注意 OnFire / PermaFlyStatus 原版是通用状态，但 C# 还没有对应定义，这里同样本地声明。
        public static readonly Identifier LethicesRapeTentaclesId = new("coc", "lethices_rape_tentacles");

        public static readonly Identifier PigbysHandsId = new("coc", "pigbys_hands");

        public static readonly Identifier WhipSilenceId = new("coc", "whip_silence");

        public static readonly Identifier MinotaurKingsTouchId = new("coc", "minotaur_kings_touch");

        public static readonly Identifier OnFireId = new("coc", "on_fire");

        public static readonly Identifier PermaFlyStatusId = new("coc", "perma_fly_status");

        private static readonly Identifier DemonicArouseId = new("coc", "lethice_demonic_arouse");

        private static readonly Identifier DemonfireId = new("coc", "lethice_demonfire");

        private static readonly Identifier RapetaclesId = new("coc", "lethice_rapetacles");

        private static readonly Identifier WingbuffetId = new("coc", "lethice_wingbuffet");

        private static readonly Identifier ClawId = new("coc", "lethice_claw");

        private static readonly Identifier DemonLustMagicId = new("coc", "lethice_demon_lust_magic");

        private static readonly Identifier DirtyDancingId = new("coc", "lethice_dirty_dancing");

        private static readonly Identifier CrushingBodiesId = new("coc", "lethice_crushing_bodies");

        private static readonly Identifier HornyPokeId = new("coc", "lethice_horny_poke");

        private static readonly Identifier BukkakeTimeId = new("coc", "lethice_bukkake_time");

        private static readonly Identifier ParasiteThrowingStarsId = new("coc", "lethice_parasite_throwing_stars");

        private static readonly Identifier WhiptripId = new("coc", "lethice_whiptrip");

        private static readonly Identifier SonicwhipId = new("coc", "lethice_sonicwhip");

        private static readonly Identifier WhipchokeId = new("coc", "lethice_whipchoke");

        private static readonly Identifier GropehandsId = new("coc", "lethice_gropehands");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（Lethice.hx:19-56）逐项照抄
            Race = "Demon";

            Tallness = 108;

            CreateVagina(false, 3, 3);
            CreateBreastRow(3);

            // hips.rating = Hips.RATING_SLENDER(2) / butt.rating = Butt.RATING_TIGHT(2)
            Body.Hips = 2;
            Body.Butt = 2;

            // initStrTouSpeInte(110, 110, 110, 110) / initLibSensCor(100, 40, 100)
            Strength = 110;
            Toughness = 110;
            Speed = 110;
            Intelligence = 110;
            Libido = 100;
            Sensitivity = 40;
            Corruption = 100;

            WeaponName = "whip";
            WeaponAttack = 35;
            WeaponVerb = "whip";

            // 原版 normalAttackAvoid.doParry = false; doCounter = false（Lethice.hx:36-37）
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            ArmorName = "wraps";
            ArmorDefense = 15;

            if (CoC.Instance.Player.NewGamePlusMod() >= 3)
                ArmorDefense += 30;

            // 原版 bonusHP = 650（Lethice.hx:43）：走最大生命修饰层（数据表 bonusHealth 留 0）
            var bonusHealth = 650f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            // 原版 bonusLust = 80（Lethice.hx:44）：C# 的 Creature.MaxLust 是写死的 100（Creature.cs:189），
            // 没有 bonusLust 的等价字段，所以她的欲望上限目前只有 100（已知差异，需共享 Creature 改动）。

            // 原版 additionalXP = 1000（Lethice.hx:45）
            AdditionalExperience = 1000;

            // 原版 gems = 500 + Utils.rand(50)（Lethice.hx:46）
            Gems = Random.Shared.Next(50) + 500;

            Level = 25;
            LustVulnerability = 0.15f;

            // 原版 drop = NO_DROP
            CreatePerk(VanillaPerks.Tank);
            CreatePerk(VanillaPerks.Tank2);
            CreatePerk(VanillaPerks.ImprovedSelfControl);
            CreatePerk(VanillaPerks.SpellcastingAffinity, 30);

            // wings.type = Wings.DRACONIC_LARGE（Lethice.hx:54）
            Body.Get<WingsPart>().Id = VanillaBodyPartTypes.DraconicLarge;
        }

        // 原版 get_long()（Lethice.hx:58-106）：按阶段 / 防御姿态 / 玩家身上的状态现拼。
        // 拆成多条 key，全部在 Assets/Localization/.../Lethice.hjson 里。
        public override string Description
        {
            get
            {
                var player = CoC.Instance.Player;

                string text;

                if (FightPhase == 1)
                {
                    text = "Npcs.Lethice.Desc".Localized();
                }
                else if (FightPhase == 2)
                {
                    text = "Npcs.Lethice.Desc.Phase2".Localized();

                    if (HasStatusEffect(VanillaStatusEffects.Blind))
                        text += "Npcs.Lethice.Desc.Phase2Blind".Localized();
                    else if (HasStatusEffect(OnFireId))
                        text += "Npcs.Lethice.Desc.Phase2OnFire".Localized();
                }
                else
                {
                    text = "Npcs.Lethice.Desc.Phase3Lead".Localized();

                    if (WingsDestroyed)
                        text += "Npcs.Lethice.Desc.Phase3Wings".Localized();

                    text += "Npcs.Lethice.Desc.Phase3Rest".Localized();

                    if (FightPhase == 3 && DefMode != 0)
                    {
                        text += DefMode == 1
                            ? "Npcs.Lethice.Desc.DefMode1".Localized()
                            : "Npcs.Lethice.Desc.DefMode2".Localized();
                    }
                }

                if (player.HasStatusEffect(LethicesRapeTentaclesId))
                {
                    text += "Npcs.Lethice.Desc.Tentacles".Localized();

                    if (player.StatusEffectValue(LethicesRapeTentaclesId, 2) != 0)
                    {
                        text += "Npcs.Lethice.Desc.TentaclesGrabbed".Localized();
                    }
                    else if (player.HasStatusEffect(VanillaStatusEffects.KnowsWhitefire))
                    {
                        text += player.CanFly()
                            ? "Npcs.Lethice.Desc.TentaclesFlying".Localized()
                            : "Npcs.Lethice.Desc.TentaclesQuick".Localized();
                    }
                }

                if (HasStatusEffect(VanillaStatusEffects.Shell))
                    text += "Npcs.Lethice.Desc.Shell".Localized();

                if (player.HasStatusEffect(PigbysHandsId))
                    text += "Npcs.Lethice.Desc.PigbysHands".Localized();

                return text;
            }
        }

        // 原版 performCombatAction 的公共前置（Lethice.hx:131-135）：
        //   回合数 +1；玩家身上有 Lethice 的触手 → 先结算一次被抓；phase3 再按上回合的血/欲望变化切换防御姿态。
        // 这些都在选招之前，所以放 BeforeCombatAction。
        public override bool BeforeCombatAction(CombatContext context)
        {
            RoundCount += 1;

            if (context.Player.HasStatusEffect(LethicesRapeTentaclesId))
                LethiceCombatHandlers.RapetacleTurn(context);

            if (FightPhase == 3)
                LethiceCombatHandlers.UpdatePhase3Stats(context);

            return false;
        }

        // 原版 performCombatAction 的选招部分（Lethice.hx:136-148 + phase1/2/3）。
        // 原版用 MonsterAI 但每个阶段是独立的权重表，且带 `_roundCount % 10 == 3` / 随机门控，
        // 所以按阶段在代码里等权重抽（各招 weight 都是 1）。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>
            FightPhase switch
            {
                2 => Phase2Action(context),
                3 => Phase3Action(context),
                _ => Phase1Action(context)
            };

        // 原版 phase1()（Lethice.hx:151-159）：demonicArouse / demonfire / rapetacles(第 10n+3 回合) /
        // wingbuffet(玩家没失明) / claw，全部 weight 1。
        private CombatAiAction Phase1Action(CombatContext context)
        {
            var options = new List<Identifier> { DemonicArouseId, DemonfireId, ClawId };

            if (!context.Player.HasStatusEffect(VanillaStatusEffects.Blind))
                options.Add(WingbuffetId);

            if (RoundCount % 10 == 3)
                options.Add(RapetaclesId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        // 原版 phase2()（Lethice.hx:406-414）：四招等权重 + bukkakeTime（rand(10)==0 且玩家没失明）。
        private CombatAiAction Phase2Action(CombatContext context)
        {
            var options = new List<Identifier>
            {
                DemonLustMagicId, DirtyDancingId, CrushingBodiesId, HornyPokeId
            };

            if (context.Random.Next(10) == 0 && !context.Player.HasStatusEffect(VanillaStatusEffects.Blind))
                options.Add(BukkakeTimeId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        // 原版 phase3() 的后半段（Lethice.hx:732-741）：
        //   第 5 回合固定 gropehands；否则在四招里等权重抽，whipchoke 只在玩家没被鞭子沉默时进池。
        private CombatAiAction Phase3Action(CombatContext context)
        {
            if (RoundCount == 5)
                return new CombatAiAction { Ability = GropehandsId };

            var options = new List<Identifier>
            {
                ParasiteThrowingStarsId, WhiptripId, SonicwhipId
            };

            if (!context.Player.HasStatusEffect(WhipSilenceId))
                options.Add(WhipchokeId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        // 原版 struggle()（Lethice.hx:326-330）：被触手抓住时玩家挣扎（由原版 Combat 菜单的 Struggle 按钮调用）。
        public override void Struggle(CombatContext context) => LethiceCombatHandlers.Struggle(context);

        // 原版 phase2Ends 的「战斗中途菜单」（Lethice.hx:528-535）与 p2DemonFuck 的子菜单（Lethice.hx:566-573）
        // 以及 beginPhase3(true) 的 Next（Lethice.hx:691-693）。
        // 派发层在 CombatController.Menu() 写完状态行、出标准按钮之前调用本钩子；返回 true 即接管本回合菜单。
        // 按钮槽位照原版：0/1/2。
        public override bool CustomCombatMenu(CombatController controller, CombatContext context)
        {
            switch (Phase2Menu)
            {
                case Phase2MenuState.Top:
                    // 原版：if (player.hasCock() || player.hasVagina()) addButton(0, "DemonFuck", ...)
                    if (context.Player.HasCock() || context.Player.HasVagina())
                    {
                        context.AddButton(0, "DemonFuck", () =>
                        {
                            Phase2Menu = Phase2MenuState.None);
                            LethiceCombatHandlers.P2DemonFuck(context, controller);
                        };
                    }

                    // 原版：if (player.hasStatusEffect(StatusEffects.KnowsHeal)) addButton(1, "Heal", ...)
                    if (context.Player.HasStatusEffect(VanillaStatusEffects.KnowsHeal))
                    {
                        context.AddButton(1, "Heal", () =>
                        {
                            Phase2Menu = Phase2MenuState.None);
                            LethiceCombatHandlers.P2Heal(context, controller);
                        };
                    }

                    // 原版：addButton(2, "Next", p2Next)
                    context.AddButton(2, "Next", () =>
                    {
                        Phase2Menu = Phase2MenuState.None);
                        LethiceCombatHandlers.P2Next(context, controller);
                    };

                    return true;

                case Phase2MenuState.DemonFuckSub:
                    // 原版 p2DemonFuck 的菜单：OralFinish(0) / FuckDemon(1，有屌时) / RideCock(2)（Lethice.hx:566-573）
                    context.AddButton(0, "OralFinish", () =>
                    {
                        Phase2Menu = Phase2MenuState.None);
                        LethiceCombatHandlers.OralFinish(context, controller);
                    };

                    if (context.Player.HasCock())
                    {
                        context.AddButton(1, "FuckDemon", () =>
                        {
                            Phase2Menu = Phase2MenuState.None);
                            LethiceCombatHandlers.FuckDemon(context, controller);
                        };
                    }

                    context.AddButton(2, "RideCock", () =>
                    {
                        Phase2Menu = Phase2MenuState.None);
                        LethiceCombatHandlers.RideCock(context, controller);
                    };

                    return true;

                case Phase2MenuState.Phase3Next:
                    // 原版 beginPhase3(true) 只放一枚 Next（Lethice.hx:691-693）
                    context.AddButton(0, "Next", () =>
                    {
                        Phase2Menu = Phase2MenuState.None);
                        LethiceCombatHandlers.P2Next(context, controller);
                    };

                    return true;

                default:
                    return false;
            }
        }

        // 原版 defeated()（Lethice.hx:108-120）：
        //   phase1/phase2 时分别走 phase1Ends()/phase2Ends()（清屏、长文本、回满血、切阶段、startMonsterTurn 续战）；
        //   phase3 时 SceneInstance<LethiceScenesScene>.Get().Defeated(hpVictory)（Lethice.hx:119；场景已移植 LethiceScenes.cs）。
        // C# 由派发层在发奖励前调用本钩子（CombatController.Finish），返回 Continue 时按新一场战斗重置并进敌人回合。
        // 阶段 2 的 phase2Ends 只置 Phase2Menu=Top 并消费掉这次敌人行动，菜单由 CustomCombatMenu 接管（Lethice.hx:528-535）。
        public override DefeatResult ResolveDefeated(CombatContext context, bool hpVictory)
        {
            if (FightPhase == 1)
            {
                LethiceCombatHandlers.Phase1Ends(context, hpVictory);

                return DefeatResult.Continue;
            }

            if (FightPhase == 2)
            {
                LethiceCombatHandlers.Phase2Ends(context, hpVictory);

                return DefeatResult.Continue;
            }

            return DefeatResult.Ended;
        }

        // 原版 won()（Lethice.hx:122-124）：→ SceneInstance<LethiceScenesScene>.Get().Won(hpVictory)。
        // 场景已移植（LethiceScenes.cs），覆写在下方。

        // 原版 defeated() 的 phase3 分支（Lethice.hx:119）+ defeated_ 的收尾：
        // 阶段 1/2 已在 ResolveDefeated 里 Continue，只有 phase3 会走到这里（ResolveDefeated 返回 Ended 之后）。
        // phase3 场景自己会 cleanupAfterCombat + 出选择菜单，所以这里返回 true 接管，不再补通用胜利流程。
        public override bool Defeated(bool hpVictory)
        {
            if (FightPhase != 3)
                return false;

            SceneInstance<LethiceScenesScene>.Get().Defeated(hpVictory);

            return true;
        }

        // 原版 won(hpVictory, pcCameWorms)（Lethice.hx:122-124）：game.lethicesKeep.lethice.won(hpVictory)。
        // pcCameWorms 在 LethiceScenes.won 的函数体里没被使用（只按 hpVictory 分支），C# 侧无需该参数。
        public override bool Won(bool hpVictory)
        {
            SceneInstance<LethiceScenesScene>.Get().Won(hpVictory);

            return true;
        }
    }
}
