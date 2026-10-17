using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/Succubus.hx（extends AbstractSuccubus）
    // 属性/掉落见 Assets/Data/Enemies/Succubus.hjson；
    // 招式见 Contents/Combat/VanillaHandlers/SuccubusCombatHandlers.cs
    // （kissAttack / seduceAttack 来自 AbstractSuccubus.hx，succubusBarrage 来自 Succubus.hx）。
    [Register("coc:succubus")]
    public class SuccubusEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄（Succubus.hx:42-75，身体部分来自 AbstractSuccubus 的组合）
            // createVagina(false, Vagina.WETNESS_WET, Vagina.LOOSENESS_NORMAL)
            CreateVagina(false, 2, 2);

            CreateStatusEffect(VanillaStatusEffects.BonusVCapacity, 40);

            // createBreastRow(Appearance.breastCupInverse("G"))
            CreateBreastRow(VanillaBreastCups.G);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 1;

            CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 30);

            // tallness = 35 + Utils.rand(4)
            Tallness = 35 + Random.Shared.Next(4);

            // hips.rating = RATING_AMPLE(6) + 2 / butt.rating = RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            Body.Get<SkinPart>().Tone = "ivory";

            var hair = Body.Get<HairPart>();
            hair.Color = "black";
            hair.Length = 4;

            WeaponName = "fists";
            WeaponVerb = "punch";
            ArmorName = "skin";

            // 原版 lust = 50 / level = 3 在数据表里；gems 是 Utils.rand(5) + 5
            Gems = Random.Shared.Next(5) + 5;

            // 原版 drop = WeightedDrop(INCUBID 1, SUCMILK 1, SDELITE 1)：见 Succubus.hjson loot
        }

        // 原版 defeated() → game.succubusScene.winAgainstSuccubus()（Succubus.hx:38-40）
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.SuccubusScene>
                .Get().WinAgainstSuccubus();

            return true;
        }

        // 原版 won() → game.succubusScene.loseToSuccubus()（Succubus.hx:34-36）
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Forest.SuccubusScene>
                .Get().LoseToSuccubus();

            return true;
        }
    }
}
