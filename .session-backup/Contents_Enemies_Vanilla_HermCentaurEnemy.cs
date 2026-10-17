using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes.scenes.dungeons.lethicesKeep.HermCentaur extends Monster（HermCentaur.hx）
    // 招式见 Contents/Combat/VanillaHandlers/HermCentaurCombatHandlers.cs。
    // 原版 performCombatAction 是手写的、还带 6 个实例状态字段，所以选招在下面的 ChooseCombatAction，
    // 那些字段也挂在实例上供 handler 读写（对应原版 _usedGottaCum / _usedHeal / ...）。
    [Register("coc:herm_centaur")]
    public class HermCentaurEnemy : Enemy
    {
        // 原版实例字段（HermCentaur.hx:70-77）
        public bool UsedGottaCum;

        public bool UsedHeal;

        public bool ChargingArouse;

        public float LustAtChargeStart = -1;

        public int ArouseCooldown;

        public int HypnoCockUses;

        private static readonly Identifier ArouseSpellCastId = new("coc", "herm_centaur_arouse_spell_cast");

        private static readonly Identifier GottaCumId = new("coc", "herm_centaur_gotta_cum");

        private static readonly Identifier HealUpId = new("coc", "herm_centaur_heal_up");

        private static readonly Identifier HypnoCockId = new("coc", "herm_centaur_hypno_cock");

        private static readonly Identifier FeminineMuskId = new("coc", "herm_centaur_feminine_musk");

        private static readonly Identifier AphrodisiacSquirtId = new("coc", "herm_centaur_aphrodisiac_squirt");

        private static readonly Identifier ArouseSpellChargeId = new("coc", "herm_centaur_arouse_spell_charge");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（HermCentaur.hx:14-54）逐项照抄
            Race = "Demon";

            // 原版 tallness = 12 * 9
            Tallness = 108;

            // 原版 createBreastRow(Appearance.breastCupInverse("E")) = 5
            CreateBreastRow(5f);

            CreateCock(36, 5, VanillaBodyPartTypes.Horse);
            CreateVagina(false, 4, 5);

            Balls = 2;
            BallSize = 3;

            // hips.rating = Hips.RATING_FERTILE(15) / butt.rating = Butt.RATING_EXPANSIVE(13)
            Body.Hips = 15;
            Body.Butt = 13;

            // initStrTouSpeInte(100, 100, 65, 65) / initLibSensCor(85, 40, 100)
            Strength = 100;
            Toughness = 100;
            Speed = 65;
            Intelligence = 65;
            Libido = 85;
            Sensitivity = 40;
            Corruption = 100;

            WeaponName = "fists";
            WeaponAttack = 1;
            WeaponVerb = "punch";

            ArmorName = "wraps";

            // 原版 bonusHP = 800（HermCentaur.hx:44）：走最大生命修饰层（数据表 bonusHealth 留 0）
            var bonusHealth = 800f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            // 原版 gems = 75 + Utils.rand(50)（HermCentaur.hx:46）
            Gems = Random.Shared.Next(50) + 75;

            Level = 22;

            LustVulnerability = 0.6f;

            // 原版 drop = NO_DROP（HermCentaur.hx:51）：数据表 loot 留空即可

            // 原版 createVagina 在 createCock 之后调用，所以最终代词是 she/her/her（HermCentaur.hx:25-28 的吐槽）。
            // C# 的 Pronoun1-3 只按「有屌就是 he」推断，会得到 he，所以用数据表的 pronoun1-3 显式覆盖。
        }

        // 原版 performCombatAction()（HermCentaur.hx:79-100）：
        //   回合开始先 _arouseCooldown--；
        //   蓄力中 → arouseSpellCast；lust100 >= 60 且没用过 → gottaCum；
        //   血量 <= 50% 且没用过 → healUp；rand(lust) >= 40 → hypnoCock；
        //   否则在 [feminineMusk, aphrodisiacSquirt, (冷却好了才加 arouseSpellCharge)] 里等权重抽。
        // 前四个分支每次都只放一条 action，所以等于是强制出那一招。
        // 每条的消耗照抄原版 add(action, weight, when, cost, fatigueType, range) 的后三个参数。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            ArouseCooldown--;

            if (ChargingArouse)
                return new CombatAiAction { Ability = ArouseSpellCastId, Cost = 10, Fatigue = FatigueType.Magical };

            if (Lust100 >= 60 && !UsedGottaCum)
                return new CombatAiAction { Ability = GottaCumId, Cost = 10, Fatigue = FatigueType.None };

            if (Health <= MaxHealth * 0.5f && !UsedHeal)
                return new CombatAiAction { Ability = HealUpId, Cost = 0, Fatigue = FatigueType.MagicalHeal };

            // 原版 Utils.rand(lust) >= 40（rand 是 Std.int(random * max)）
            if ((int)(context.Random.NextDouble() * Lust) >= 40)
                return new CombatAiAction { Ability = HypnoCockId, Cost = 10, Fatigue = FatigueType.Magical };

            var options = new List<CombatAiAction>
            {
                new() { Ability = FeminineMuskId, Cost = 10, Fatigue = FatigueType.Physical },
                new() { Ability = AphrodisiacSquirtId, Cost = 10, Fatigue = FatigueType.Physical }
            };

            if (!ChargingArouse && ArouseCooldown <= 0)
                options.Add(new CombatAiAction { Ability = ArouseSpellChargeId, Cost = 0, Fatigue = FatigueType.Magical });

            return options[context.Random.Next(options.Count)];
        }

        // 原版 react / DescribeAttacked 核对（HermCentaur.hx 全文）：
        //   - react：0 处（没有覆写任何反应时机）
        //   - replacesDescribeAttacked / DescribeAttacked：0 处
        // 所以 AfterAttacked 等派发点对本类没有额外行为，不需要覆写。

        // 原版 defeated()（HermCentaur.hx:56-58）：
        //   game.lethicesKeep.hermCentaur.beatThePony(hpVictory)
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<HermCentaurScenes>.Get().BeatThePony(hpVictory);

            return true;
        }

        // 原版 won()（HermCentaur.hx:60-62）：
        //   game.lethicesKeep.hermCentaur.inSovietCoCPonyRidesYou(hpVictory, pcCameWorms)
        public override bool Won(bool hpVictory)
        {
            SceneInstance<HermCentaurScenes>.Get().InSovietCoCPonyRidesYou(hpVictory);

            return true;
        }
    }
}
