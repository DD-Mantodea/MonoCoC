using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Quests;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Sirius extends Naga（src/classes/scenes/quests/urtaQuest/Sirius.hx）
    // 原版 super(true) 跳过 Naga 的默认身体，所以 C# 直接继承 Enemy 并按 Sirius 自己的构造配置。
    // 只移植类本体；defeated/won 依赖 Urta 任务线（未移植，原版 Sirius.hx:77-83）。
    [Register("coc:sirius")]
    public class SiriusEnemy : Enemy
    {
        private static readonly Identifier AttackId = new("coc", "sirius_attack");
        private static readonly Identifier PoisonBiteId = new("coc", "sirius_poison_bite");
        private static readonly Identifier TeaseId = new("coc", "sirius_man_naga_tease");
        private static readonly Identifier SpitId = new("coc", "sirius_naga_spit_attack");

        // 原版 performCombatAction（Sirius.hx:25-32）：eAttack 1 / poisonBite 1 / manNagaTease 1 /
        // nagaSpitAttack 1（when: 玩家失明）。最后一条依赖玩家状态，数据驱动 AI 表达不了，这里覆写。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)>
            {
                (AttackId, 1f),
                (PoisonBiteId, 1f),
                (TeaseId, 1f)
            };

            if (context.Player.HasStatusEffect(VanillaStatusEffects.Blind))
                choices.Add((SpitId, 1f));

            var total = 0f;

            foreach (var (_, weight) in choices)
                total += weight;

            var roll = (float)context.Random.NextDouble() * total;

            foreach (var (id, weight) in choices)
            {
                roll -= weight;

                if (roll < 0)
                    return new CombatAiAction { Ability = id };
            }

            return new CombatAiAction { Ability = choices[^1].Id };
        }

        // 原版 outputAttack（Sirius.hx:17-23）：伤害 > 0 时换成专属措辞（带血量颜色与伤害值）。
        public override void OutputAttack(CombatContext context, float damage)
        {
            if (damage <= 0)
            {
                base.OutputAttack(context, damage);

                return;
            }

            // 固定用深色主题色 #ff0000（原版 colorHpMinus 在 MainViewManager.hx:35-37；C# 未移植主题系统）。
            context.Append("Combat.Sirius.OutputAttack.YouMisjudgeHis".Localized("#ff0000", (int)damage));
        }

        // 原版 nagaSpitAttack 造成的玩家 Blind（Sirius.hx:57，value1=3）在全局玩家状态块递减
        // （Combat.hx:1776-1799）；已统一迁到 CombatStatusTicker.Tick，不再由施加者敌人 tick。

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 createCock(14, 2)（默认 HUMAN）
            CreateCock(14, 2);

            // 原版 createBreastRow(0)
            CreateBreastRow(0);

            // 原版 ass.analLooseness = LOOSENESS_TIGHT(1); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;
            anal.Wetness = 0;

            // 原版 createStatusEffect(BonusACapacity, 10)
            if (!HasStatusEffect(VanillaStatusEffects.BonusACapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 10);

            // 原版 tallness = 5 * 12 + 10
            Tallness = 5 * 12 + 10;

            // 原版 hips.rating = RATING_AMPLE(6) + 2；butt.rating = RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            // 原版 lowerBody.type = LowerBody.NAGA
            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.Naga;

            // 原版 skin.tone = "mediterranean-toned"; hair.color = "orange"; hair.length = 16
            Body.Get<SkinPart>().Tone = "mediterranean-toned";

            var hair = Body.Get<HairPart>();

            hair.Color = "orange";
            hair.Length = 16;

            // 原版 weaponName = "fangs"; weaponVerb = "bite"; weaponAttack = 25
            WeaponName = "fangs";
            WeaponVerb = "bite";
            WeaponAttack = 25;

            // 原版 normalAttackAvoid.doParry = false; doCounter = false
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            // 原版 armorName = "scales"; armorDef = 30
            ArmorName = "scales";
            ArmorDefense = 30;

            // 原版 gems = Utils.rand(5) + 8：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(5) + 8;

            // 原版 temperment = TEMPERMENT_RANDOM_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop = NO_DROP（Sirius.hx:119），所以数据表 loot 留空。
        }

        // 原版 defeated()（Sirius.hx:77-79）
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaBeatsUpSiriusRadio();

            return true;
        }

        // 原版 won()（Sirius.hx:81-83）
        public override bool Won(bool hpVictory)
        {
            SceneInstance<UrtaQuestScene>.Get().UrtaLosesToSirriusSnakeRadio();

            return true;
        }
    }
}
