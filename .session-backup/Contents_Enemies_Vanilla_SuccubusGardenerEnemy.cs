using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes.scenes.dungeons.lethicesKeep.SuccubusGardener extends Monster（SuccubusGardener.hx）
    // 招式见 Contents/Combat/VanillaHandlers/SuccubusGardenerCombatHandlers.cs。
    // 原版 performCombatAction 是手写的阶段战（冷却藤蔓 / 植物精液 DoT / 低血回血），
    // 所以选招在下面的 BeforeCombatAction + ChooseCombatAction。
    [Register("coc:succubus_gardener")]
    public class SuccubusGardenerEnemy : Enemy
    {
        // 原版的这 5 个状态只有 SuccubusGardener 自己用（Haxe src 里没有别的引用），
        // 所以不进共享的 VanillaStatusEffects，本地声明。
        // （StatusEffects.LustAura 在 C# 已有同名定义，直接复用。）
        public static readonly Identifier TentagrappleCooldownId = new("coc", "tentagrapple_cooldown");

        public static readonly Identifier TentagrappledId = new("coc", "tentagrappled");

        public static readonly Identifier ShowerDotEffectId = new("coc", "shower_dot_effect");

        public static readonly Identifier GardenerSapSpeedId = new("coc", "gardener_sap_speed");

        public static readonly Identifier VineHealUsedId = new("coc", "vine_heal_used");

        private static readonly Identifier VineHealId = new("coc", "succubus_gardener_vine_heal");

        private static readonly Identifier TentagrappleId = new("coc", "succubus_gardener_tentagrapple");

        private static readonly Identifier SapSpeedId = new("coc", "succubus_gardener_sap_speed");

        private static readonly Identifier SicemId = new("coc", "succubus_gardener_sicem");

        private static readonly Identifier CorruptiveShowerId = new("coc", "succubus_gardener_corruptive_shower");

        private static readonly Identifier LustAuraCastId = new("coc", "succubus_gardener_lust_aura");

        private static readonly Identifier TauntId = new("coc", "succubus_gardener_taunt");

        private static readonly Identifier MotorboatId = new("coc", "succubus_gardener_motorboat");

        private static readonly Identifier TasteTheEcstasyId = new("coc", "succubus_gardener_taste_the_ecstasy");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（SuccubusGardener.hx:16-57）逐项照抄
            Race = "Demon";

            CreateVagina(false, 3, 3);

            // 原版 createBreastRow(Appearance.breastCupInverse("FF"))：BREAST_CUP_NAMES 里 "FF-cup" 的下标是 13
            CreateBreastRow(13f);

            // ass.analLooseness = Ass.LOOSENESS_LOOSE(3) / analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 3;
            anal.Wetness = 0;

            Tallness = 96;

            // hips.rating = Hips.RATING_AVERAGE(4) / butt.rating = Butt.RATING_TIGHT(2)
            Body.Hips = 4;
            Body.Butt = 2;

            WeaponName = "tentacles";
            WeaponVerb = "lash";
            WeaponAttack = 22;

            // 原版 normalAttackAvoid.doParry = false; doCounter = false（SuccubusGardener.hx:37-38）
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            ArmorName = "tentaclothes";

            // initStrTouSpeInte(85, 60, 85, 100) / initLibSensCor(85, 60, 100)
            Strength = 85;
            Toughness = 60;
            Speed = 85;
            Intelligence = 100;
            Libido = 85;
            Sensitivity = 60;
            Corruption = 100;

            // 原版 bonusHP = 600：走最大生命修饰层（数据表 bonusHealth 留 0）
            var bonusHealth = 600f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            // 原版 fatigue = 0（vineHeal 每次 +5，到 100 就用不出回血了）
            Fatigue = 0;

            // 原版 gems = 50 + Utils.rand(33)
            Gems = Random.Shared.Next(33) + 50;

            Level = 20;
            LustVulnerability = 0;

            // 原版 drop = NO_DROP
            // 构造末尾 createStatusEffect(StatusEffects.TentagrappleCooldown, 10)
            CreateStatusEffect(TentagrappleCooldownId, 10);
        }

        // 原版 handleStun()（SuccubusGardener.hx:124-130）：
        //   血量 <= 60% 时无视眩晕（return true），否则走基类。
        // 注意：这个分支故意**不删** Stunned 状态——原版 vineHeal 就是靠 hasStatusEffect(Stunned)
        // 来决定用「被藤蔓强行灌」那套文本；状态会在回满血后的下一回合由 base.HandleStun 清掉。
        public override bool HandleStun(CombatContext context)
        {
            if (Health <= MaxHealth * 0.6f)
                return true;

            return base.HandleStun(context);
        }

        // 原版 performCombatAction 的前半段（SuccubusGardener.hx:85-98）：
        //   1) TentagrappleCooldown 每回合减 1，归零移除；
        //   2) 玩家身上有植物精液 DoT → 立刻结算一次；若这个 DoT 把玩家欲望顶满，本回合她不再行动。
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (HasStatusEffect(TentagrappleCooldownId))
            {
                ChangeStatusValue(TentagrappleCooldownId, 0, -1);

                if (StatusEffectValue(TentagrappleCooldownId, 0) <= 0)
                    RemoveStatusEffect(TentagrappleCooldownId);
            }

            if (context.Player.HasStatusEffect(ShowerDotEffectId))
            {
                SuccubusGardenerCombatHandlers.ShowerDotEffect(context);

                if (context.Player.Lust >= context.Player.MaxLust)
                    return true;
            }

            return false;
        }

        // 原版 performCombatAction 的后半段（SuccubusGardener.hx:100-121）：
        //   血量 <= 60% 且没力竭 → vineHeal；
        //   藤蔓抓取冷却好了 → tentagrapple；
        //   玩家没被减速且自己用过 vineHeal → sapSpeed；
        //   否则在 [sicem, corruptiveShower, lustAuraCast] +（lust<40 加 taunt）+（lust>=40 加 motorboat、tasteTheEcstasy）里等权重抽。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (Health <= MaxHealth * 0.6f && Fatigue < 100)
                return new CombatAiAction { Ability = VineHealId };

            if (!HasStatusEffect(TentagrappleCooldownId))
                return new CombatAiAction { Ability = TentagrappleId };

            if (!context.Player.HasStatusEffect(GardenerSapSpeedId) && HasStatusEffect(VineHealUsedId))
                return new CombatAiAction { Ability = SapSpeedId };

            var options = new List<Identifier> { SicemId, CorruptiveShowerId, LustAuraCastId };

            if (Lust < 40)
                options.Add(TauntId);

            if (Lust >= 40)
            {
                options.Add(MotorboatId);
                options.Add(TasteTheEcstasyId);
            }

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        // 原版 struggle()（SuccubusGardener.hx:192-220）：被藤蔓抓住时玩家的挣扎。
        public override void Struggle(CombatContext context) =>
            SuccubusGardenerCombatHandlers.Struggle(context);

        // 原版 react(PlayerWaited)（SuccubusGardener.hx:222-234）：被抓住时玩家选择「等待」→ 藤蔓收紧。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            if (!context.Player.HasStatusEffect(TentagrappledId))
                return true;

            SuccubusGardenerCombatHandlers.Squeeze(context);

            TookAction = true;

            return false;
        }

        // 原版 cleanupEffects()（SuccubusGardener.hx:59-66）：战斗结束时摘掉藤蔓束缚与植物精液 DoT。
        // 场景接线在下面的 Defeated/Won：先 CleanupEffects()，再调 SuccubusGardenerScenes 的对应方法。
        private void CleanupEffects()
        {
            var player = CoC.Instance.Player;

            if (player.HasStatusEffect(TentagrappledId))
                player.RemoveStatusEffect(TentagrappledId);

            if (player.HasStatusEffect(ShowerDotEffectId))
                player.RemoveStatusEffect(ShowerDotEffectId);

            // GardenerSapSpeed 在原版是 CombatBuff，脱战自动失效；C# 这里连状态一起清掉
            if (player.HasStatusEffect(GardenerSapSpeedId))
                player.RemoveStatusEffect(GardenerSapSpeedId);
        }

        // 原版 react / DescribeAttacked 核对（SuccubusGardener.hx 全文）：
        //   - AfterAttacked：0 处
        //   - replacesDescribeAttacked / DescribeAttacked：0 处
        //   - 只有 react(PlayerWaited)（SuccubusGardener.hx:222-234，已接本类的 React）。
        // 所以 AfterAttacked 派发点对本类没有额外行为，不需要覆写。

        // 原版 defeated()（SuccubusGardener.hx:68-71）：cleanupEffects + game.lethicesKeep.succubusGardener.fuckUpTheGardener
        public override bool Defeated(bool hpVictory)
        {
            CleanupEffects();
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep.SuccubusGardenerScenes>.Get()
                .FuckUpTheGardener(hpVictory);

            return true;
        }

        // 原版 won()（SuccubusGardener.hx:73-76）：cleanupEffects + game.lethicesKeep.succubusGardener.surrenderToTheGardener
        public override bool Won(bool hpVictory)
        {
            CleanupEffects();
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep.SuccubusGardenerScenes>.Get()
                .SurrenderToTheGardener(hpVictory);

            return true;
        }
    }
}
