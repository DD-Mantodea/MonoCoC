using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Contents.Scenes.Vanilla.Places.Farm;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Kelt extends Monster（src/classes/scenes/places/farm/Kelt.hx）
    // 类本体 + defeated/won 接线都在本文件（胜 → KeltScene/KellyScene 调教收尾，负 → KeltFucksShitUp）。
    [Register("coc:kelt")]
    public class KeltEnemy : Enemy
    {
        // 原版 new() 里的局部量 breakLevel2 = KFLAGS.flags[KELT_BREAK_LEVEL] == 2，
        // 同时影响体型、罩杯、描述与 AI 里的 dayDreamKelly 条件，所以存成字段。
        private bool _breakLevel2;

        // 原版 Kelt.bowCooldown（Kelt.hx:35）：射箭后重置为 3，每回合怪物行动前减 1。
        private int _bowCooldown;

        private static readonly Identifier ShootBowId = new("coc", "kelt_shoot_bow");
        private static readonly Identifier DayDreamId = new("coc", "kelt_day_dream_kelly");
        private static readonly Identifier AttackId = new("coc", "kelt_attack");
        private static readonly Identifier KellyuraId = new("coc", "kelt_kellyura_attack");
        private static readonly Identifier TramplesId = new("coc", "kelt_tramples_joo");

        // 供 KeltCombatHandlers.keltShootBow 重置冷却
        public void SetBowCooldown(int value) => _bowCooldown = value;

        // 原版 performCombatAction（Kelt.hx:96-107）：
        //   bowCooldown > 0 时先减 1；shootBow 仅在冷却为 0 时可抽；
        //   dayDreamKelly 仅在 KELT_BREAK_LEVEL >= 2 时可抽；其余三条常驻。
        // 这些 when 条件依赖实例状态与 flag，数据驱动 AI 表达不了，所以在这里覆写。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (_bowCooldown > 0)
                _bowCooldown -= 1;

            var choices = new List<(Identifier Id, float Weight)>();

            if (_bowCooldown == 0)
                choices.Add((ShootBowId, 2f));

            if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) >= 2)
                choices.Add((DayDreamId, 1f));

            choices.Add((AttackId, 2f));
            choices.Add((KellyuraId, 1.5f));
            choices.Add((TramplesId, 1.5f));

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

        // 原版 long 里 breakLevel2 为真时追加的一段，数据表的 descriptionKey 只放基础段。
        public override string Description =>
            base.Description + (_breakLevel2 ? "Npcs.Kelt.Desc.Broken".Localized() : "");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var player = CoC.Instance.Player;

            _breakLevel2 = CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) == 2;

            // 原版 createCock(breakLevel2 ? 12 : 24, 3.5, CockTypesEnum.HORSE)
            CreateCock(_breakLevel2 ? 12 : 24, 3.5f, VanillaBodyPartTypes.Horse);

            // 原版 balls = 2; ballSize = 2 + Utils.rand(13); cumMultiplier = 1.5
            Balls = 2;
            BallSize = 2 + Random.Shared.Next(13);
            CumMultiplier = 1.5f;

            // 原版 hoursSinceCum = player.ballSize * 10
            HoursSinceCum = player.BallSize * 10;

            // 原版 createBreastRow(Appearance.breastCupInverse(breakLevel2 ? "B" : "A")) → B=2 / A=1
            CreateBreastRow(_breakLevel2 ? VanillaBreastCups.B : VanillaBreastCups.A);

            // 原版 ass.analLooseness = LOOSENESS_NORMAL(2); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 2;
            anal.Wetness = 0;

            // 原版 createStatusEffect(BonusACapacity, 50)
            if (!HasStatusEffect(VanillaStatusEffects.BonusACapacity))
                CreateStatusEffect(VanillaStatusEffects.BonusACapacity, 50);

            // 原版 tallness = 84
            Tallness = 84;

            // 原版 hips.rating = RATING_AVERAGE(4); butt.rating = RATING_AVERAGE(4) + 1
            Body.SetHipsRating("average");
            Body.SetButtRating("average", 1);

            // 原版 lowerBody.type = LowerBody.HOOFED; lowerBody.legCount = 4
            var lowerBody = Body.Get<LowerBodyPart>();

            lowerBody.Id = VanillaBodyPartTypes.Hoofed;
            lowerBody.LegCount = 4;

            // 原版 skin.tone = "tan"
            Body.Get<SkinPart>().Tone = "tan";

            // 原版 hair.color = Utils.randomChoice(["black", "brown"]); hair.length = 3
            var hair = Body.Get<HairPart>();

            hair.Color = Random.Shared.Next(2) == 0 ? "black" : "brown";
            hair.Length = 3;

            // 原版 tail.type = Tail.HORSE
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Horse;

            // 原版 weaponName = "fist"; weaponVerb = "punch"; weaponAttack = 10
            WeaponName = "fist";
            WeaponVerb = "punch";
            WeaponAttack = 10;

            // 原版 armorName = "tough skin"; armorDef = 4
            ArmorName = "tough skin";
            ArmorDefense = 4;

            // 原版 lustVuln = 0.83
            LustVulnerability = 0.83f;

            // 原版 gems = Utils.rand(5) + 5：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(5) + 5;

            // 原版 temperment = TEMPERMENT_LUSTY_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            //
            // 原版 drop = NO_DROP，所以数据表 loot 留空。
        }

        // 原版 defeated(hpVictory)（Kelt.hx:109-123）：
        //   灭口线（KELT_KILL_PLAN == 1）→ KeltScene 的 HP 胜利 / 欲望胜利两种收场；
        //   否则按 KELT_BREAK_LEVEL 分流到 KellyScene 的第二次（== 1）/ 第三次调教收尾。
        // 这些场景方法会读 CombatController.Instance 的战场数据（敌方 HP / 欲望），
        // 所以必须在战斗上下文仍然活着时调用 —— 即 Finish() 里的 Defeated()，不是 OnDefeated 的 Next 回调。
        public override bool Defeated(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(VanillaFlags.KeltKillPlan) == 1)
            {
                if (hpVictory)
                    SceneInstance<KeltScene>.Get().FightToBeatKeltVictoryHP();
                else
                    SceneInstance<KeltScene>.Get().FightToBeatKeltVictoryLust();
            }
            else if (CoC.Instance.Flags.Get(VanillaFlags.KeltBreakLevel) == 1)
            {
                SceneInstance<KellyScene>.Get().DefeatKellyNDBREAKHIM();
            }
            else
            {
                SceneInstance<KellyScene>.Get().BreakingKeltNumeroThree();
            }

            return true;
        }

        // 原版 won(hpVictory, pcCameWorms)（Kelt.hx:125-132）：
        //   被虫群弄射 → 只写一句台词、走通用力竭战败（C# 对应 came_worms_after_combat 旗标，与 Amarok 同做法）；
        //   否则进 kelly.keltFucksShitUp()（战败被踩 + 后续 keltFucksShitUpII）。
        public override bool Won(bool hpVictory)
        {
            if (CoC.Instance.Flags.Get(VanillaFlags.CameWormsAfterCombat) > 0)
            {
                MonoCoC.Contents.Scenes.Scene.OutputText("Combat.Kelt.Won.KeltRecoilsFor".Localized());

                return false;
            }

            SceneInstance<KellyScene>.Get().KeltFucksShitUp();

            return true;
        }
    }
}
