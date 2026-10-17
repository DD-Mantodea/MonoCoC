using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Marble extends Monster（src/classes/scenes/npcs/Marble.hx）
    // 招式见 Contents/Combat/VanillaHandlers/MarbleCombatHandlers.cs。
    //
    // won/defeated：原版 Marble.hx:67 的 defeated 调用 game.marbleScene.marbleFightWin()、
    // 原版 Marble.hx:71 的 won 调用 game.marbleScene.marbleFightLose()。
    // C# 侧 MarbleScene 已落地战斗收尾段（Contents/Scenes/Vanilla/Npcs/MarbleScene.cs），直接接线；
    // 场景内尚未移植的按钮（MarbleScene.hx:716 rapeMarbleInHerRoom / :793 forceFeedMarble）见该文件注释。
    [Register("coc:marble")]
    public class MarbleEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄
            CreateVagina(false, 1, 2);
            CreateBreastRow(VanillaBreastCups.F);

            // ass.analLooseness = Ass.LOOSENESS_VIRGIN(0) / analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 0;
            anal.Wetness = 0;

            // tallness = 6 * 12 + 4
            Tallness = 76;

            // hips.rating = Hips.RATING_CURVY(10) / butt.rating = Butt.RATING_LARGE(8)
            Body.Hips = 10;
            Body.Butt = 8;

            // lowerBody.type = LowerBody.HOOFED
            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.Hoofed;

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();
            hair.Color = "brown";
            hair.Length = 13;

            // initStrTouSpeInte(75, 70, 35, 40)
            Strength = 75;
            Toughness = 70;
            Speed = 35;
            Intelligence = 40;

            // initLibSensCor(25, 45, 40)
            Libido = 25;
            Sensitivity = 45;
            Corruption = 40;

            WeaponName = "large hammer";
            WeaponVerb = "hammer-blow";
            WeaponAttack = 10;

            ArmorName = "tough hide";
            ArmorDefense = 5;

            // 原版 Marble.hx:110 temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：死字段，
            // AI 权重由 Assets/Data/Combat/Ai/Marble.hjson 表达。

            Level = 7;
            Gems = Random.Shared.Next(5) + 25;

            // 原版 Marble.hx:114 tail.type = Tail.COW
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Cow;

            // 原版 Marble.hx:113 drop = new WeightedDrop(weapons.L_HAMMR, 1)：见 Assets/Data/Enemies/Marble.hjson
            // （weapons.L_HAMMR 的 header 是 "Marble's Hammer"，对应 coc:marbles_hammer）
        }

        // 原版 Marble.hx:67 defeated()：game.marbleScene.marbleFightWin()
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<MarbleScene>.Get().MarbleFightWin();

            return true;
        }

        // 原版 Marble.hx:71 won()：game.marbleScene.marbleFightLose()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<MarbleScene>.Get().MarbleFightLose();

            return true;
        }
    }
}
