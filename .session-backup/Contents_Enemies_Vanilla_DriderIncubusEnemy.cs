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

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes.scenes.dungeons.lethicesKeep.DriderIncubus extends AbstractSpiderMorph（DriderIncubus.hx）
    // C# 没有蜘蛛娘公共基类，AbstractSpiderMorph 的 spiderMorphWebAttack 一并实现在 handler 里
    // （与 CorruptedDrider 的移植方式一致）。
    // 原版 performCombatAction 是手写的多段回合（每回合「刺击 + 一次子招式」，必要时再加哥布林行动），
    // 所以整回合由一个 handler 执行：Contents/Combat/VanillaHandlers/DriderIncubusCombatHandlers.cs。
    [Register("coc:drider_incubus")]
    public class DriderIncubusEnemy : Enemy
    {
        // 原版实例字段（DriderIncubus.hx:12-22）
        public bool GoblinFree;

        public int CombatRound;

        public int HpGains;

        public bool SeenResolute;

        public bool GoblinWebChain;

        public bool GoblinOiled;

        // 原版的三个状态只有 DriderIncubus 自己用（Haxe src 里没有别的引用），
        // 所以不进共享的 VanillaStatusEffects，本地声明。
        public static readonly Identifier DriderIncubusVenomId = new("coc", "drider_incubus_venom");

        public static readonly Identifier TaintedMindId = new("coc", "tainted_mind");

        public static readonly Identifier PurpleHazeId = new("coc", "purple_haze");

        private static readonly Identifier RoundId = new("coc", "drider_incubus_round");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（DriderIncubus.hx:24-52）逐项照抄
            Race = "Demon";

            Tallness = 108;

            CreateCock(24, 5, VanillaBodyPartTypes.Demon);
            CreateBreastRow(0);
            Balls = 2;
            BallSize = 4;
            HoursSinceCum = 9999;

            // hips.rating = Hips.RATING_SLENDER(2) / butt.rating = Butt.RATING_TIGHT(2)
            Body.Hips = 2;
            Body.Butt = 2;

            // initStrTouSpeInte(65, 80, 90, 70) / initLibSensCor(66, 40, 100)
            Strength = 65;
            Toughness = 80;
            Speed = 90;
            Intelligence = 70;
            Libido = 66;
            Sensitivity = 40;
            Corruption = 100;

            WeaponName = "spear";
            WeaponAttack = 19;
            WeaponVerb = "lunge";

            ArmorName = "chitin";
            ArmorDefense = 40;

            // 原版 bonusHP = 500：走最大生命修饰层（数据表 bonusHealth 留 0）
            var bonusHealth = 500f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            // 原版 gems = 75 + Utils.rand(50)
            Gems = Random.Shared.Next(50) + 75;

            Level = 22;

            // 原版 additionalXP = 200
            AdditionalExperience = 200;

            LustVulnerability = 0.45f;

            // 原版 drop = NO_DROP：数据表 loot 留空即可
        }

        // 原版 get_long() / set_goblinFree()（DriderIncubus.hx:63-78）：
        // 主描述固定，哥布林那一段按 goblinFree 二选一。C# 用 Description 现拼。
        public override string Description
        {
            get
            {
                var text = "Npcs.DriderIncubus.Desc".Localized();

                text += GoblinFree
                    ? "Npcs.DriderIncubus.Desc.Freed".Localized()
                    : "Npcs.DriderIncubus.Desc.Harness".Localized();

                return text;
            }
        }

        // 原版 performCombatAction（DriderIncubus.hx:80-98）：整回合交给 Round handler。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>
            new CombatAiAction { Ability = RoundId };

        // 原版 react(BeforeAttacked)（DriderIncubus.hx:396-406）：
        //   玩家中了 TaintedMind 还想攻击 → 手不听使唤去摸自己，攻击被整个取消。
        // （Combat.hx:927 在原版就是攻击流程最前面的这个钩子；C# 的 CombatResolver.PlayerAttack 已派发。）
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.BeforeAttacked)
                return true;

            if (!context.Player.HasStatusEffect(TaintedMindId))
                return true;

            DriderIncubusCombatHandlers.TaintedMindAttackAttempt(context);

            return false;
        }

        // 原版每回合的玩家侧状态结算（Combat.hx:2067-2088）在 C# 没有对应位置，
        // 放到敌人回合开始的 CombatRoundUpdate 里结算一次（频率相同：每回合一次）：
        //   PurpleHaze 倒计时，归零时连 Blind 一起摘掉；期间每回合 +3 欲望。
        //   TaintedMind 倒计时，归零时移除。
        //   DriderIncubusVenom 的「医学」perk 每回合 14% 净化（DriderIncubusVenomDebuff.hx:22-30）。
        public override void CombatRoundUpdate(CombatContext context)
        {
            var player = context.Player;

            if (player.HasStatusEffect(PurpleHazeId))
            {
                player.ChangeStatusValue(PurpleHazeId, 0, -1);

                if (player.StatusEffectValue(PurpleHazeId, 0) <= 0)
                {
                    player.RemoveStatusEffect(PurpleHazeId);
                    player.RemoveStatusEffect(VanillaStatusEffects.Blind);

                    context.Append("Combat.DriderIncubus.CombatRound.PurpleHazeEnds".Localized());
                }
                else
                {
                    context.Append("Combat.DriderIncubus.CombatRound.PurpleHazeTick".Localized());

                    player.TakeLustDamage(3, true);
                }
            }

            if (player.HasStatusEffect(TaintedMindId))
            {
                player.ChangeStatusValue(TaintedMindId, 0, -1);

                if (player.StatusEffectValue(TaintedMindId, 0) <= 0)
                {
                    player.RemoveStatusEffect(TaintedMindId);

                    context.Append("Combat.DriderIncubus.CombatRound.TaintedMindEnds".Localized());
                }
            }

            if (player.HasStatusEffect(DriderIncubusVenomId) &&
                player.HasPerk(VanillaPerks.Medicine) &&
                context.Random.Next(100) <= 14)
            {
                player.RemoveStatusEffect(DriderIncubusVenomId);

                context.Append("Combat.DriderIncubus.CombatRound.VenomCleansed".Localized());
            }
        }

        // 原版 react / DescribeAttacked 核对（DriderIncubus.hx 全文）：
        //   - AfterAttacked：0 处（没有这个分支）
        //   - replacesDescribeAttacked / DescribeAttacked：0 处
        //   - 只有 react(BeforeAttacked)（DriderIncubus.hx:396-406，已接本类的 React）。
        // 所以 AfterAttacked 派发点对本类没有额外行为，不需要覆写。

        // 原版 defeated()（DriderIncubus.hx:54-56）：
        //   game.lethicesKeep.driderIncubus.beatTheSpooderbutt(hpVictory);
        // 场景已落地：Contents/Scenes/Vanilla/Dungeons/LethicesKeep/DriderIncubusScenes.cs。
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DriderIncubusScenesScene>.Get().BeatTheSpooderbutt(hpVictory);

            return true;
        }

        // 原版 won()（DriderIncubus.hx:58-60）：
        //   game.lethicesKeep.driderIncubus.spooderbuttGetsANewCockSleeve(hpVictory, pcCameWorms);
        // C# 的 Won(bool) 没有 pcCameWorms 参数，按默认 false 处理（原版默认值即 false）。
        public override bool Won(bool hpVictory)
        {
            SceneInstance<DriderIncubusScenesScene>.Get().SpooderbuttGetsANewCockSleeve(hpVictory);

            return true;
        }
    }
}
