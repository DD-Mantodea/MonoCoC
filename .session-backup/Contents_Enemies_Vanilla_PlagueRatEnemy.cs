using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 PlagueRat extends Monster（src/classes/scenes/monsters/PlagueRat.hx）
    // 招式见 Contents/Combat/VanillaHandlers/PlagueRatCombatHandlers.cs，
    // 剧情分支在 Contents/Scenes/Vanilla/Areas/Swamp/PlagueRatScene.cs。
    [Register("coc:plague_rat")]
    public class PlagueRatEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄
            // createCock(Utils.rand(2) + 5, 1.5, CockTypesEnum.DEMON)
            CreateCock(Random.Shared.Next(2) + 5, 1.5f, VanillaBodyPartTypes.Demon);

            Balls = 2;
            BallSize = 1;

            // 原版 cumMultiplier = 3 / hoursSinceCum = 20：C# 没有对应字段（同 Minotaur 的说明）

            CreateBreastRow(0);

            // 原版 pronoun1/2/3 = "he"/"him"/"his"：C# 用数据表里的 pronoun1-3 表达

            // ass.analLooseness = LOOSENESS_STRETCHED(4) / analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 4;
            anal.Wetness = 1;

            // tallness = Utils.rand(6) + 57
            Tallness = Random.Shared.Next(6) + 57;

            // hips.rating = RATING_BOYISH(0) / butt.rating = RATING_TIGHT(2)
            Body.Hips = 0;
            Body.Butt = 2;

            // horns.type = Horns.DEMON; horns.value = 2
            var horns = Body.Get<HornsPart>();
            horns.Id = VanillaBodyPartTypes.Demon;
            horns.Value = 2;

            // face.type = Face.MOUSE
            Body.Get<FacePart>().Id = VanillaBodyPartTypes.Mouse;

            // skin.type = FUR / adj = "patchy" / furColor = "burgundy" / desc = "fur" / tone = "red"
            var skin = Body.Get<SkinPart>();
            skin.Id = VanillaBodyPartTypes.Fur;
            skin.Adj = "patchy";
            skin.FurColor = "burgundy";
            skin.DescKey = "Skin.Desc.fur";
            skin.Tone = "red";

            // initStrTouSpeInte(65, 30, 80, 20)
            Strength = 65;
            Toughness = 30;
            Speed = 80;
            Intelligence = 20;

            // initLibSensCor(55, 35, 100)
            Libido = 55;
            Sensitivity = 35;
            Corruption = 100;

            WeaponName = "claw";
            WeaponVerb = "swipe";
            WeaponAttack = 10;

            ArmorName = "leathery skin";
            ArmorDefense = 5;

            // bonusHP = 100：走最大生命修饰层（数据表里 bonusHealth 留 0，避免重复叠加）
            var bonusHealth = 100f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Lust = 30;
            LustVulnerability = .65f;
            // 原版 temperment = TEMPERMENT_LUSTY_GRAPPLES：AI 权重由 PlagueRat.hjson 的 ai 表达
            Level = 7;
            Gems = Random.Shared.Next(15) + 25;

            // 原版 drop = new WeightedDrop().add(MOUSECO, 3).add(INCUBID, 1)
            // 掉落在 Assets/Data/Enemies/PlagueRat.hjson 的 loot 里配置
        }

        // 原版 this.long（PlagueRat.hx:97）里有一处 [if (tallness < 60)]：
        // Haxe 的 tallness 在这里指「怪物自己」的身高，而 C# 的 tallness 标签是玩家身高，
        // 语义不同，所以拆成两条 key，在这里按自己的 Tallness 选。
        public override string Description => Tallness < 60
            ? "Npcs.Plaguerat.Desc.NearPlayerHeight".Localized()
            : "Npcs.Plaguerat.Desc".Localized();

        // 原版 defeated()：game.plagueRatScene.plagueDefeat(hpVictory)
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp.PlagueRatScene>.Get().PlagueDefeat(hpVictory);

            return true;
        }

        // 原版 won()：game.plagueRatScene.plagueVictory()
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Areas.Swamp.PlagueRatScene>.Get().PlagueVictory();

            return true;
        }
    }
}
