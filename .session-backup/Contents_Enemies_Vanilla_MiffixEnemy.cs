using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/Miffix.hx（extends Monster）
    // 属性/掉落见 Assets/Data/Enemies/Miffix.hjson；原版 performCombatAction 是空 MonsterAI，
    // 没有可用招式，因此走 MonsterAI.exec 的「没得选就普攻」分支。
    [Register("coc:miffix")]
    public class MiffixEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄（Miffix.hx:31-57）
            // createCock(Utils.rand(2) + 10, 2.5, CockTypesEnum.DEMON)
            CreateCock(Random.Shared.Next(2) + 10, 2.5f, VanillaBodyPartTypes.Demon);

            Balls = 2;
            BallSize = 2;

            CreateBreastRow(0);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 1;

            // tallness = Utils.rand(24) + 25
            Tallness = Random.Shared.Next(24) + 25;

            // hips.rating = RATING_BOYISH(0) / butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();
            hair.Color = "black";
            hair.Length = 5;

            // weaponVerb = "claw-slash"
            WeaponName = "claws";
            WeaponVerb = "claw-slash";
            ArmorName = "leathery skin";

            // wings.type = Wings.IMP
            Body.Get<WingsPart>().Id = VanillaBodyPartTypes.Imp;

            // 原版 lust = 20 / level = 3 在数据表里；gems 是 Utils.rand(5) + 5
            Gems = Random.Shared.Next(5) + 5;

            // 原版 drop = WeightedDrop(SUCMILK 3, INCUBID 3, IMPFOOD 4, WOODSHL 1)：见 Miffix.hjson loot

            // 原版 defeated() / won() 都是空实现（Miffix.hx:7-11），不需要接场景。
        }
    }
}
