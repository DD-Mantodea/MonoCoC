using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.Forest;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/GoblinAssassin.hx（extends Monster）
    // 10 级哥布林刺客：药剂、欲望针、双针、爆针。
    [Register("coc:goblin_assassin")]
    public class GoblinAssassinEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new() 逐项照抄（GoblinAssassin.hx:137-175）──
            // createVagina(false, WETNESS_DROOLING(4), LOOSENESS_NORMAL(2))
            CreateVagina(false, 4, 2);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 90);

            // createBreastRow(Appearance.breastCupInverse("E"))
            CreateBreastRow(VanillaBreastCups.E);

            // ass.analLooseness = LOOSENESS_NORMAL(2) / analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 2;
            anal.Wetness = 0;

            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 50);

            // tallness = 35 + Utils.rand(4)
            Tallness = 35 + Random.Shared.Next(4);

            // hips.rating = RATING_AMPLE(6) + 2 / butt.rating = RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "dark green";

            var hair = Body.Get<HairPart>();
            hair.Color = "blue";
            hair.Length = 7;

            // 原版 level = 10 / lust = 50 / bonusHP = 70 / 属性都在数据表里。
            WeaponName = "needles";
            WeaponVerb = "stabbing needles";
            ArmorName = "leather straps";

            // 原版 gems = Utils.rand(50) + 25
            Gems = Random.Shared.Next(50) + 25;

            // createPerk(PerkLib.Evade, 0, 0, 0, 0)
            CreatePerk(VanillaPerks.Evade);

            // 原版 additionalXP = 150
            AdditionalExperience = 150;

            // 原版 drop 见 GoblinAssassin.hjson loot。
        }

        // 原版 GoblinAssassin.hx:124-126 defeated(hpVictory) → game.goblinAssassinScene.gobboAssassinRapeIntro()
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<GoblinAssassinScene>.Get().GobboAssassinRapeIntro();

            return true;
        }

        // 原版 GoblinAssassin.hx:128-135 won(hpVictory, pcCameWorms) → game.goblinAssassinScene.gobboAssassinBeatYaUp()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<GoblinAssassinScene>.Get().GobboAssassinBeatYaUp();

            return true;
        }
    }
}
