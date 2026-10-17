using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Places.MothCave;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 RiddleDemon extends Monster（src/classes/scenes/places/mothCave/RiddleDemon.hx）
    // 只移植类本体；defeated/won 依赖 DoloresScene（未移植，原版 RiddleDemon.hx:12-18）。
    [Register("coc:riddle_demon")]
    public class RiddleDemonEnemy : Enemy
    {
        // 原版 whisperCooldown（RiddleDemon.hx:10）：每回合先自减，<=0 时 whisper 才进抽签池。
        private int _whisperCooldown;

        // 原版 doloresFree = (DoloresScene.saveContent.hikkiQuest & HQFREE) > 0。
        // DoloresScene 未移植（原版 RiddleDemon.hx:89），这里固定 false，TurnStart 里的女儿分支因而不会触发。
        public bool DoloresFree;

        // 供 handler 里 whisper 命中后把冷却设为 4（原版 RiddleDemon.hx:64）
        public void SetWhisperCooldown(int value) => _whisperCooldown = value;

        private static readonly Identifier SlashId = new("coc", "riddle_demon_slash");
        private static readonly Identifier FeintId = new("coc", "riddle_demon_feint");
        private static readonly Identifier WhisperId = new("coc", "riddle_demon_whisper");

        // 原版 performCombatAction（RiddleDemon.hx:20-26）：
        //   slash 3 / feint 1 / whisper 1（when: whisperCooldown-- <= 0）
        // when 依赖实例冷却，数据驱动 AI 表达不了，所以在这里覆写。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var whisperReady = _whisperCooldown-- <= 0;

            var choices = new List<(Identifier Id, float Weight)>
            {
                (SlashId, 3f),
                (FeintId, 1f)
            };

            if (whisperReady)
                choices.Add((WhisperId, 1f));

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

        // 原版 react(TurnStart)（RiddleDemon.hx:67-85）：10% 让女儿打断他（tookAction）；
        // doloresFree 时另有 10% 治疗玩家 / 10% 灼烧他。女儿相关分支目前不可达（原版 RiddleDemon.hx:67-85，DoloresScene 未移植）。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.TurnStart)
                return true;

            if (context.Random.Next(100) < 10)
            {
                context.Append("Combat.RiddleDemon.React.TheDemonMakes".Localized());

                TookAction = true;
            }
            else if (DoloresFree)
            {
                if (context.Random.Next(100) < 10 && context.Player.Health < context.Player.MaxHealth)
                {
                    context.Append("Combat.RiddleDemon.React.ASlightMurmur".Localized());

                    context.Player.HealthChange(context.Player.MaxHealth * 0.1f);
                }
                else if (context.Random.Next(100) < 10)
                {
                    context.Append("Combat.RiddleDemon.React.TheDemonIs".Localized());

                    HealthChange(-(50 + context.Random.Next(50)));
                }
            }

            return true;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 createCock(10, 1.5, CockTypesEnum.DEMON); balls = 2; ballSize = 3
            CreateCock(10, 1.5f, VanillaBodyPartTypes.Demon);
            Balls = 2;
            BallSize = 3;

            // 原版 tallness = 78
            Tallness = 78;

            // 原版 createBreastRow()（默认 0 号）
            CreateBreastRow();

            // 原版 ass.analLooseness = LOOSENESS_TIGHT(1); ass.analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 1;
            anal.Wetness = 1;

            // 原版 skin.tone = "pale"; hair.color = "gray"
            Body.Get<SkinPart>().Tone = "pale";
            Body.Get<HairPart>().Color = "gray";

            // 原版 hips.rating = RATING_SLENDER(2); butt.rating = RATING_AVERAGE(4)
            Body.SetHipsRating("slender");
            Body.SetButtRating("average");

            // 原版 weaponName = "nails"; weaponVerb = "slash"; weaponAttack = 30
            WeaponName = "nails";
            WeaponVerb = "slash";
            WeaponAttack = 30;

            // 原版 armorName = "shabby clothing"; armorDef = 10
            ArmorName = "shabby clothing";
            ArmorDefense = 10;

            // 原版 lustVuln = 0.4
            LustVulnerability = 0.4f;

            // 原版 createStatusEffect(GenericRunDisabled)
            if (!HasStatusEffect(VanillaStatusEffects.GenericRunDisabled))
                CreateStatusEffect(VanillaStatusEffects.GenericRunDisabled);

            // 原版 createPerk(StunImmune) / createPerk(SpeedyRecovery)
            if (!HasPerk(VanillaPerks.StunImmune))
                CreatePerk(VanillaPerks.StunImmune);

            if (!HasPerk(VanillaPerks.SpeedyRecovery))
                CreatePerk(VanillaPerks.SpeedyRecovery);

            // 原版 additionalXP = 200
            AdditionalExperience = 200;

            // 原版 temperment = TEMPERMENT_LUSTY_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 bonusLust = 100（RiddleDemon.hx:114）：C# 的 Creature.MaxLust 固定 100、没有 bonusLust 机制，未建模。
            // 原版 drop = NO_DROP（RiddleDemon.hx:122），所以数据表 loot 留空。
        }

        // 原版 defeated(hpVictory)（RiddleDemon.hx:12-14）：mothCave.doloresScene.hikkiFightWin()
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().HikkiFightWin();

            return true;
        }

        // 原版 won(hpVictory, pcCameWorms)（RiddleDemon.hx:16-18）：mothCave.doloresScene.hikkiFightLoss()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<DoloresScene>.Get().HikkiFightLoss();

            return true;
        }
    }
}
