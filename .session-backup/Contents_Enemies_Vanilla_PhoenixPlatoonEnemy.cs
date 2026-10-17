using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 PhoenixPlatoon extends Monster（scenes/dungeons/helDungeon/PhoenixPlatoon.hx:6）
    // 由 Tools/PortKit/monster.mjs 生成骨架，Configure / AI / 胜负分支按 Haxe 原文人工补齐。
    // 招式效果见 Contents/Combat/VanillaHandlers/PhoenixPlatoonCombatHandlers.cs。
    [Register("coc:phoenix_platoon")]
    public class PhoenixPlatoonEnemy : Enemy
    {
        // 原版 PhoenixPlatoon.hx:31 `var platoonStage:Int = 0`：
        // 招式按 连击 → 火焰吐息 → 催情弹 循环（Haxe 用 switch 顺序推进）。
        private int _platoonStage;

        private static readonly Identifier RushId = new("coc", "phoenix_platoon_rush");
        private static readonly Identifier FireBreathId = new("coc", "phoenix_platoon_fire_breath");
        private static readonly Identifier LustbangId = new("coc", "phoenix_platoon_lustbang");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new()（PhoenixPlatoon.hx:53）的赋值顺序照抄 ──
            // a = "the " / short = "phoenix platoon" / long 由数据表的 article / nameKey / descriptionKey 表达。
            // plural = true / pronoun1-3 = they/them/their 也来自数据表。

            CreateCock();
            Balls = 2;
            BallSize = 1;
            CumMultiplier = 3;

            // createVagina(false, Vagina.WETNESS_SLAVERING(5), Vagina.LOOSENESS_LOOSE(2))
            CreateVagina(false, 5, 2);

            // createBreastRow(Appearance.breastCupInverse("D")) = 4
            CreateBreastRow(VanillaBreastCups.D);

            // ass.analLooseness = Ass.LOOSENESS_STRETCHED(4) / ass.analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 4;
            anal.Wetness = 0;

            Tallness = Random.Shared.Next(8) + 70;

            // hips.rating = Hips.RATING_AMPLE(6) + 2 / butt.rating = Butt.RATING_LARGE(8)
            Body.SetHipsRating("ample", 2);
            Body.SetButtRating("large");

            // lowerBody.type = LowerBody.LIZARD
            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.Lizard;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();
            hair.Color = "black";
            hair.Length = 15;

            // initStrTouSpeInte(60, 60, 120, 40) / initLibSensCor(40, 45, 50) 由数据表给。

            WeaponName = "spears";
            WeaponVerb = "stab";
            WeaponAttack = 20;

            ArmorName = "armor";
            ArmorDefense = 50;

            // bonusHP = 400 / lust = 20 / level = 20 由数据表给。
            LustVulnerability = .15f;

            // temperment = Monster.TEMPERMENT_LOVE_GRAPPLES：原版死字段，无实际作用。

            Gems = Random.Shared.Next(25) + 160;
            AdditionalExperience = 50;

            // horns.type = Horns.DRACONIC_X2; horns.value = 2
            var horns = Body.Get<HornsPart>();
            horns.Id = VanillaBodyPartTypes.DraconicX2;
            horns.Value = 2;

            // tail.type = Tail.HARPY / wings.type = Wings.FEATHERED_LARGE
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Harpy;
            Body.Get<WingsPart>().Id = VanillaBodyPartTypes.FeatheredLarge;

            // drop = NO_DROP：数据表 loot 留空（不掉落）。
        }

        // 原版 PhoenixPlatoon.hx:32 phoenixPlatoonAI：
        //   switch platoonStage { case 0: rush; case 1: fireBreath; default: lustbang; }
        //   platoonStage = (platoonStage + 1) % 3;
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var ability = _platoonStage switch
            {
                0 => RushId,
                1 => FireBreathId,
                _ => LustbangId
            };

            _platoonStage = (_platoonStage + 1) % 3;

            return new CombatAiAction { Ability = ability };
        }

        // 原版 PhoenixPlatoon.hx:45 defeated → game.dungeons.heltower.phoenixPlatoonLosesToPC()
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Hel.HelDungeonScene>.Get().PhoenixPlatoonLosesToPC();

            return true;
        }

        // 原版 PhoenixPlatoon.hx:49 won → game.dungeons.heltower.phoenixPlatoonMurdersPC()
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Hel.HelDungeonScene>.Get().PhoenixPlatoonMurdersPC();

            return true;
        }
    }
}
