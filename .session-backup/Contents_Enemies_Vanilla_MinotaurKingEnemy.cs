using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes.scenes.dungeons.lethicesKeep.MinotaurKing extends Monster（MinotaurKing.hx）
    // 招式见 Contents/Combat/VanillaHandlers/MinotaurKingCombatHandlers.cs。
    // 原版 performCombatAction 是手写的、带 4 个实例计数器，选招在下面的 ChooseCombatAction + BeforeCombatAction。
    [Register("coc:minotaur_king")]
    public class MinotaurKingEnemy : Enemy
    {
        // 原版实例字段（MinotaurKing.hx:12-20）
        public int MilkDrinks;

        public int Orgasms;

        public bool LastRoundStun;

        public int LastSpellCastCount;

        // 原版 excelliaHP：被玩家打晕后不再产奶。C# 的场景（lethicesKeep.minotaurKing）未移植，
        // 所以这个值目前只会停在初始值，描述里的「Excellia 昏迷」分支暂时不会出现。
        public int ExcelliaHp;

        // BeforeCombatAction 判定完 _lastRoundStun 后，把「本回合该出 dickslap」这件事传给 ChooseCombatAction。
        private bool _dickslapQueued;

        private static readonly Identifier BackhandId = new("coc", "minotaur_king_backhand");

        private static readonly Identifier HeadbuttId = new("coc", "minotaur_king_headbutt");

        private static readonly Identifier DickslapId = new("coc", "minotaur_king_dickslap");

        private static readonly Identifier BattleaxeId = new("coc", "minotaur_king_battleaxe");

        private static readonly Identifier MinoPheromonesId = new("coc", "minotaur_king_mino_pheromones");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（MinotaurKing.hx:22-53）逐项照抄
            // excelliaHP = 700 + player.newGamePlusMod() * 500
            ExcelliaHp = 700 + CoC.Instance.Player.NewGamePlusMod() * 500;

            Race = "Minotaur";

            // tallness = 12 * 14
            Tallness = 168;

            CreateCock(24, 5, VanillaBodyPartTypes.Horse);
            CreateBreastRow(0);
            Balls = 2;
            BallSize = 4;
            HoursSinceCum = 9999;

            // hips.rating = Hips.RATING_SLENDER(2) / butt.rating = Butt.RATING_TIGHT(2)
            Body.Hips = 2;
            Body.Butt = 2;

            // initStrTouSpeInte(100, 100, 50, 60) / initLibSensCor(66, 10, 100)
            Strength = 100;
            Toughness = 100;
            Speed = 50;
            Intelligence = 60;
            Libido = 66;
            Sensitivity = 10;
            Corruption = 100;

            WeaponName = "axe";
            WeaponAttack = 50;
            WeaponVerb = "swing";

            ArmorName = "rags";
            ArmorDefense = 60;

            // 原版 bonusHP = 850（MinotaurKing.hx:44）：走最大生命修饰层（数据表 bonusHealth 留 0）
            var bonusHealth = 850f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            // 原版 gems = 75 + Utils.rand(50)
            Gems = Random.Shared.Next(50) + 75;

            Level = 22;
            LustVulnerability = 0.05f;

            // 原版 additionalXP = 200
            AdditionalExperience = 200;

            // 原版 drop：先 NO_DROP，随后被 new WeightedDrop(consumables.PROMEAD, 1) 覆盖
            // → 数据表 loot 里配 coc:premium_mead

            // 构造末尾 _lastSpellCastCount = flags[KFLAGS.SPELLS_CAST]
            LastSpellCastCount = CoC.Instance.Flags.Get(VanillaFlags.SpellsCast);
        }

        // 原版 get_long()（MinotaurKing.hx:55-80）：三套主描述（第一次 / 高潮过之后）
        // + 三段 Excellia 状态 + 三段喝奶计数 + Excellia 被打晕。全部按实例状态现拼。
        public override string Description
        {
            get
            {
                var text = Orgasms == 0
                    ? "Npcs.MinotaurKing.Desc".Localized()
                    : "Npcs.MinotaurKing.Desc.AfterOrgasms".Localized();

                if (Orgasms != 0)
                {
                    if (Lust100 < 40)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaCalm".Localized();
                    else if (Lust100 < 80)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaFallen".Localized();
                    else
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaCreamed".Localized();
                }

                if (MilkDrinks != 0)
                {
                    if (MilkDrinks == 1)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.MilkFirst".Localized();
                    else if (MilkDrinks < 10)
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.MilkMore".Localized();
                    else
                        text += "\n\n" + "Npcs.MinotaurKing.Desc.MilkFull".Localized();
                }

                if (ExcelliaHp <= 0)
                    text += "\n\n" + "Npcs.MinotaurKing.Desc.ExcelliaKO".Localized();

                return text;
            }
        }

        // 原版 handleStun()（MinotaurKing.hx:266-270）：删掉眩晕，照常出手。
        public override bool HandleStun(CombatContext context)
        {
            context.WriteAction("Combat.MinotaurKing.HandleStun.ItOnlyTakes".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Stunned);

            return true;
        }

        // 原版 performCombatAction 的第一个分支（MinotaurKing.hx:109-113）：
        //   _lastRoundStun 为真时先清掉；玩家还被眩晕 → 出 dickslap；否则这一回合什么都不做。
        // C# 里「什么都不做」= BeforeCombatAction 返回 true（占掉这次行动）。
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (!LastRoundStun)
                return false;

            LastRoundStun = false;

            if (context.Player.HasStatusEffect(VanillaStatusEffects.Stunned))
            {
                _dickslapQueued = true;

                return false;
            }

            return true;
        }

        // 原版 performCombatAction（MinotaurKing.hx:107-123）：
        //   1) 上回合 headbutt 命中 → 玩家还晕着就 dickslap（由 BeforeCombatAction 排队）；
        //   2) 玩家上回合放过法术（SPELLS_CAST 变了）→ headbutt；
        //   3) 否则在 backhand / battleaxe / minoPheromones 里等权重抽一招。
        // 注意：C# 目前没有任何地方给 VanillaFlags.SpellsCast 自增（只有定义），
        // 所以第 2 条在玩家施法逻辑补上之前不会触发。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (_dickslapQueued)
            {
                _dickslapQueued = false;

                return new CombatAiAction { Ability = DickslapId };
            }

            var spellsCast = CoC.Instance.Flags.Get(VanillaFlags.SpellsCast);

            if (LastSpellCastCount != spellsCast)
            {
                LastSpellCastCount = spellsCast;

                return new CombatAiAction { Ability = HeadbuttId };
            }

            var options = new[]
            {
                BackhandId, BattleaxeId, MinoPheromonesId
            };

            return new CombatAiAction { Ability = options[context.Random.Next(options.Length)] };
        }

        // 原版 hpRestore()（MinotaurKing.hx:194-204）：回满血、lustVuln +0.15、lust +2、喝奶次数 +1。
        // 由下面的 ResolveDefeated 调用（HP 胜利且 _milkDrinks < 10 时续战）。
        public void HpRestore(CombatContext context)
        {
            Health = MaxHealth;
            LustVulnerability += 0.15f;
            Lust += 2;
            MilkDrinks += 1;

            context.Append("Combat.MinotaurKing.HpRestore.StaggeringBackThe".Localized());
            context.Append("Combat.MinotaurKing.HpRestore.HeLooksLike".Localized());

            if (MilkDrinks >= 10)
                context.Append("Combat.MinotaurKing.HpRestore.ItLooksLike".Localized());
        }

        // 原版 lustDump()（MinotaurKing.hx:232-243）：第一次欲望战败时「射一次继续打」。
        public void LustDump(CombatContext context)
        {
            Orgasms += 1;

            context.Append("Combat.MinotaurKing.LustDump.TheIncrediblyAroused".Localized());
            context.Append("Combat.MinotaurKing.LustDump.AMonstrousHand".Localized());
            context.Append("Combat.MinotaurKing.LustDump.TheBruteFucks".Localized());
            context.Append("Combat.MinotaurKing.LustDump.ExcelliaSMaster".Localized());

            if (context.Player.HasPerk(VanillaPerks.MinotaurCumAddict))
                context.Append("Combat.MinotaurKing.LustDump.YouReJealous".Localized());

            context.Append("Combat.MinotaurKing.LustDump.WhateverSpellThis".Localized());

            Lust = 0;
        }

        // 原版 defeated()（MinotaurKing.hx:82-96）：
        //   欲望胜利且 _orgasms == 0 → lustDump() 并续战；
        //   HP 胜利且 _milkDrinks < 10 → hpRestore() 并续战；
        //   否则 SceneInstance<MinotaurKingScenes>.Get().TheKingIsDeadLongLiveTheKing(hpVictory)（MinotaurKing.hx:95；场景已移植 MinotaurKingScenes.cs）。
        // C# 由派发层在发奖励前调用本钩子（CombatController.Finish），返回 Continue 时按新一场战斗重置并进敌人回合；
        // 原版这里还置了 tookAction=true、startMonsterTurn()，C# 用 TookAction=true 消费掉紧接的那次敌人行动。
        public override DefeatResult ResolveDefeated(CombatContext context, bool hpVictory)
        {
            if (!hpVictory && Orgasms == 0)
            {
                LustDump(context);

                TookAction = true;

                return DefeatResult.Continue;
            }

            if (hpVictory && MilkDrinks < 10)
            {
                HpRestore(context);

                TookAction = true;

                return DefeatResult.Continue;
            }

            return DefeatResult.Ended;
        }

        // 最终战败走场景（原版 MinotaurKing.hx:95）：
        //   game.lethicesKeep.minotaurKing.theKingIsDeadLongLiveTheKing(hpVictory)
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<MinotaurKingScenes>.Get().TheKingIsDeadLongLiveTheKing(hpVictory);

            return true;
        }

        // 原版 won()（MinotaurKing.hx:98-100）：
        //   game.lethicesKeep.minotaurKing.hailToTheKingBaby(hpVictory, pcCameWorms)
        public override bool Won(bool hpVictory)
        {
            SceneInstance<MinotaurKingScenes>.Get().HailToTheKingBaby(hpVictory);

            return true;
        }
    }
}
