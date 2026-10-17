using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 BeautifulSwordFight extends Monster（src/classes/scenes/camp/BeautifulSwordFight.hx）
    // 只移植类本体；defeated/won 依赖 BeautifulSwordScene（未移植，原版 BeautifulSwordFight.hx:8-17）。
    [Register("coc:beautiful_sword_fight")]
    public class BeautifulSwordFightEnemy : Enemy
    {
        // 原版 BEAUTIFUL_SWORD_LEVEL 影响武器攻击、护甲、等级、AI 招式解锁。
        private int _swordLevel;

        private static readonly Identifier AttackId = new("coc", "beautiful_sword_fight_attack");
        private static readonly Identifier EviscerateId = new("coc", "beautiful_sword_fight_eviscerate");
        private static readonly Identifier HolyLightId = new("coc", "beautiful_sword_fight_holy_light");
        private static readonly Identifier PurgeId = new("coc", "beautiful_sword_fight_purge");

        // 原版 performCombatAction（BeautifulSwordFight.hx:69-76）：
        //   eAttack 2 / eviscerate 1（level>=2）/ holyLight 1（level>=3）/ purge 1（HP 比例 < .6）
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)> { (AttackId, 2f) };

            if (_swordLevel >= 2)
                choices.Add((EviscerateId, 1f));

            if (_swordLevel >= 3)
                choices.Add((HolyLightId, 1f));

            if (HealthRatio() < 0.6f)
                choices.Add((PurgeId, 1f));

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

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            _swordLevel = CoC.Instance.Flags.Get(VanillaFlags.BeautifulSwordLevel);

            // 原版 createBreastRow()（默认 0 号）；balls = 0; ballSize = 0
            CreateBreastRow();
            Balls = 0;
            BallSize = 0;

            // 原版 tallness = 36
            Tallness = 36;

            // 原版 skin.tone = "metallic"
            Body.Get<SkinPart>().Tone = "metallic";

            // 原版 weaponAttack = 20 + BEAUTIFUL_SWORD_LEVEL * 5
            WeaponName = "blessed blade";
            WeaponVerb = "slash";
            WeaponAttack = 20 + _swordLevel * 5;

            // 原版 armorDef = 15 + BEAUTIFUL_SWORD_LEVEL
            ArmorName = "holy steel";
            ArmorDefense = 15 + _swordLevel;

            // 原版 lustVuln = 0（完全不吃挑逗）
            LustVulnerability = 0;

            // 原版 level = 17 + BEAUTIFUL_SWORD_LEVEL * 2
            Level = 17 + _swordLevel * 2;

            // 原版 gems = 0
            Gems = 0;

            // 原版 temperment = TEMPERMENT_LUSTY_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop = new WeightedDrop()（空表，BeautifulSwordFight.hx:131），所以数据表 loot 留空。
            // 原版 replacesDescribeAttacked（BeautifulSwordFight.hx:78-101）在 C# 没有对应派发点，未接。
        }

        // 原版 BeautifulSwordFight.hx:12-17 defeated(hpVictory)：输出击碎文本 + beautifulSwordScene.destroyBeautifulSword()
        public override bool Defeated(bool hpVictory)
        {
            Scene.OutputText("Combat.BeautifulSwordFight.Defeated.WithAFinal".Localized());

            SceneInstance<BeautifulSwordScene>.Get().DestroyBeautifulSword();

            return true;
        }

        // 原版 BeautifulSwordFight.hx:8-10 won(hpVictory, pcCameWorms) → beautifulSwordScene.defeatedBySword()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<BeautifulSwordScene>.Get().DefeatedBySword();

            return true;
        }
    }
}
