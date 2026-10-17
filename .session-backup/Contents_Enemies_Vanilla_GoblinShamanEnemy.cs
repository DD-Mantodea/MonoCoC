using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/GoblinShaman.hx（extends Goblin）
    // 原版构造函数调用 super(true) 跳过 Goblin 的默认身体（GoblinShaman.hx:93），
    // 所以这里继承 GoblinEnemy 并整体覆写 ConfigureBody，避免叠两层哥布林身体。
    [Register("coc:goblin_shaman")]
    public class GoblinShamanEnemy : GoblinEnemy
    {
        protected override void ConfigureBody()
        {
            // ── 原版 new() 逐项照抄（GoblinShaman.hx:92-133）──
            // createVagina(false, WETNESS_DROOLING(4), LOOSENESS_NORMAL(2))
            CreateVagina(false, 4, 2);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 40);

            // createBreastRow(Appearance.breastCupInverse("E"))
            CreateBreastRow(VanillaBreastCups.E);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 0;

            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 30);

            // tallness = 44 + Utils.rand(7)
            Tallness = 44 + Random.Shared.Next(7);

            // hips.rating = RATING_AMPLE(6) + 2 / butt.rating = RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();
            hair.Color = "indigo";
            hair.Length = 4;

            WeaponName = "wizard staff";
            WeaponVerb = "bludgeon";
            WeaponAttack = 14;

            ArmorName = "fur loincloth";
            ArmorDefense = 6;

            Fatigue = 0;

            // 原版 lustVuln = 0.4
            LustVulnerability = 0.4f;

            // 原版 bonusHP = 275 由数据表 bonusHealth 走基类加成；level = 14 / lust = 35 / 属性都在数据表。

            // 原版 gems = Utils.rand(15) + 15
            Gems = Random.Shared.Next(15) + 15;

            // 原版 drop 见 GoblinShaman.hjson loot。
        }

        // 原版 new() 末尾（GoblinShaman.hx:100-102）：玩家有屌时补充一句独占台词。
        public override string Description =>
            base.Description
            + (CoC.Instance?.Player?.HasCock() == true
                ? "Npcs.GoblinShaman.DescCockSuffix".Localized()
                : "");

        // 原版 blind() 的玩家 Blind 递减在全局玩家状态块（Combat.hx:1776-1799），
        // 已统一迁到 CombatStatusTicker.Tick（不再由施加者敌人 tick，避免多敌双倍递减）。

        // 原版 defeated() → game.goblinShamanScene.goblinShamanRapeIntro()（GoblinShaman.hx:79-81）
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.GoblinShamanScene>
                .Get().GoblinShamanRapeIntro();

            return true;
        }

        // 原版 won()（GoblinShaman.hx:83-90）：
        //   无性别玩家 → 专属台词 + cleanupAfterCombat；否则 → goblinShamanBeatYaUp()。
        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Player.Gender == MonoCoC.Contents.Creatures.Gender.Genderless)
            {
                CombatController.Instance?.Context.WriteAction(
                    "Combat.GoblinShaman.Won.YouCollapseIn".Localized());

                MonoCoC.Contents.Combat.CombatScene.CleanupAfterCombat();

                return true;
            }

            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.GoblinShamanScene>
                .Get().GoblinShamanBeatYaUp();

            return true;
        }
    }
}
