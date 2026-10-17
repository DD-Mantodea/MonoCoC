using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/Mammon.hx（extends Monster）
    // 40 级最终 Boss 级法师。原版这场战斗由场景脚本控制（MammonScene 不在 Contents 中，
    // 数据缺失：原版 Mammon.hx:132-142），类自身只有 eAttack 与「第 3 回合 rewind」两段 performCombatAction 逻辑。
    [Register("coc:mammon")]
    public class MammonEnemy : Enemy
    {
        // 原版 public var hasSwitched:Bool = false（Mammon.hx:172）
        public bool HasSwitched;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new() 逐项照抄（Mammon.hx:11-45）──
            // a = "" / short = "Mammon" / long = "placeholder" 由数据表给。

            // createBreastRow(BreastCup.D) = 4
            CreateBreastRow(VanillaBreastCups.D);

            // createVagina(false, WETNESS_NORMAL(1), LOOSENESS_LOOSE(3))
            CreateVagina(false, 1, 3);

            Tallness = 5 * 12 + 7;

            // 原版 lustVuln = 0.75
            LustVulnerability = 0.75f;

            // 原版 level = 40 / lust = 30 / bonusHP = 2500 / 属性都在数据表里。
            WeaponName = "spectral sword";
            WeaponVerb = "slash";
            WeaponAttack = 0;

            ArmorName = "";
            ArmorDefense = 0;

            // 原版 ignoreLust = true：不吃魅惑胜利。
            IgnoreLust = true;

            // 原版 drop = NO_DROP（数据表 loot 留空）；gems = 2500 固定值在数据表里。

            // 原版 additionalXP = 3200
            AdditionalExperience = 3200;

            // createPerk(Immovable) / Archmage / Mage / Spellpower / MysticLearnings
            CreatePerk(VanillaPerks.Immovable);
            CreatePerk(VanillaPerks.Archmage);
            CreatePerk(VanillaPerks.Mage);
            CreatePerk(VanillaPerks.Spellpower);
            CreatePerk(VanillaPerks.MysticLearnings);

            // 原版 bonusLust = 20：C# 的 Creature.MaxLust 恒为 100，没有对应的最大欲望修饰。
        }

        // 原版 outputDefaultTeaseReaction()（Mammon.hx:47-52）
        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var capitalA = MonoCoC.Utils.Utils.CapitalizeFirstLetter(Article);

            if (lustDelta == 0)
                context.WriteAction("Combat.Mammon.OutputDefaultTeaseReaction.01Doesn".Localized()
                    capitalA, ShortName);

            context.WriteAction("Combat.Mammon.OutputDefaultTeaseReaction.01Shows".Localized()
                capitalA, ShortName);
        }

        // 原版 handleFear()（Mammon.hx:54-57）：完全无视恐惧，并嘲讽一句。
        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.Mammon.HandleFear.Text".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 react(TurnStart)（Mammon.hx:60-74）：每回合开头在怪物槽位间随机换位。
        // 依赖原版 game.monsterArray 的「空槽 + EmptySpace」多怪位置系统；
        // EmptySpace 在 Contents 全量搜索 0 命中，C# 的 CombatContext.Enemies 是紧凑列表，没有空槽语义。
        // 而且 C# 目前也没有 TurnStart 反应调用点，所以这里只记录「已切换」标记。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            switch (reaction)
            {
                case CombatReactionContext.TurnStart:
                    if (!HasSwitched)
                        HasSwitched = true;
                    break;
            }

            return true;
        }

        // 原版 performCombatAction（Mammon.hx:174-181）：
        //   hasSwitched = false；
        //   第 3 回合 → rewind()；否则 eAttack()。
        public override bool BeforeCombatAction(CombatContext context)
        {
            HasSwitched = false;

            if (CombatController.Instance!.State.Round == 3)
            {
                Rewind(context);

                return true;
            }

            CombatController.Instance.Resolver.EnemyBasicAttack();

            return true;
        }

        // 原版 rewind()（Mammon.hx:132-136）：说完整段台词后，把回合末函数换成 rewind2。
        // rewind2（Mammon.hx:138-142）会 clearOutput + startCombatMultiple(new Mammon(), 3×EmptySpace) 重置整场战斗。
        private void Rewind(CombatContext context)
        {
            context.Append("Combat.Mammon.Rewind.MammonBreathesDeeply".Localized());
            context.Append("Combat.Mammon.Rewind.ThenYouAlso".Localized());

            // 原版 game.combat.overrideEndOfRoundFunction = rewind2：
            // C# 同名字段是 Context.EndOfRoundOverride（CombatController.Menu 里会出一枚 Next 按钮来触发它）。
            context.EndOfRoundOverride = () => Rewind2(context);
        }

        // 原版 rewind2()（Mammon.hx:138-142）：真正重开战斗。
        // 原版 startCombatMultiple(new Mammon(), 3×EmptySpace)：一个全新的 Mammon + 3 个空位。
        // C# 没有空槽/EmptySpace 语义（数据缺失：原版 Mammon.hx:138-142 依赖多怪位置系统），
        // 单敌人重置等价于「新实例 + 清空战斗上下文并重新 Start」，空位部分省略。
        private void Rewind2(CombatContext context)
        {
            context.Clear();

            var controller = CombatController.Instance!;

            // 新 Mammon：走 EnemyRegistry.Create，等价原版 new Mammon()（重新跑一遍构造函数/Configure）
            var fresh = EnemyRegistry.Create(new Identifier("coc", "mammon"));

            // 派发层提供的战斗重置入口：对应原版 startCombatMultiple 的上下文重置部分
            context.ResetFor([fresh]);

            // 原版 setUpCombatEnvironment：combatRound = 0（C# 的回合计数在 CombatState 上）
            controller.State.Round = 0;
            controller.State.Outcome = null;
            context.EndOfRoundOverride = null;

            // 重新初始化解释器 / 结算器 / 奖励并回到战斗菜单（对应原版 setUpCombatEnvironment + 显示）
            controller.Start();
        }

        // 原版无遭遇入口（MammonScene 不存在；唯一 new Mammon() 在 Mammon.hx:140 的 rewind2 自重启），
        // 只能调试/脚本生成。原版也没有覆写 defeated()/won()（Mammon.hx 全文）。
    }
}
