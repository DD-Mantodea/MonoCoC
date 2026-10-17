using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes.scenes.dungeons.lethicesKeep.JeanClaude extends Monster（JeanClaude.hx）
    // 招式见 Contents/Combat/VanillaHandlers/JeanClaudeCombatHandlers.cs。
    // 数据表：Assets/Data/Enemies/JeanClaude.hjson + Assets/Data/Combat/Ai/JeanClaude.hjson。
    [Register("coc:jean_claude")]
    public class JeanClaudeEnemy : Enemy
    {
        // 原版 StatusEffects.RemovedArmor / StatusEffects.JCLustLevel 只有 JeanClaude（JeanClaude.hx:41-58）
        // 和 JeanClaudeScenes.hx:234/303 在用，所以不进共享的 VanillaStatusEffects，这里本地声明。
        // RemovedArmorId 由 JeanClaudeScenes 的 bad-end 分支读取，故为 public。
        public static readonly Identifier RemovedArmorId = new("coc", "removed_armor");

        private static readonly Identifier JcLustLevelId = new("coc", "jc_lust_level");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（JeanClaude.hx:141-189）逐项照抄
            Race = "Basilisk";

            // 原版 imageName = "jeanclaude"（JeanClaude.hx:145）：C# 目前没有敌人立绘字段，忽略（同其它敌人）

            CreateCock(12, 2, VanillaBodyPartTypes.Lizard);
            Balls = 2;
            BallSize = 6;
            HoursSinceCum = 9999;
            CreateBreastRow(0);
            Tallness = 86;

            // ass.analLooseness = 0 / analWetness = 0（JeanClaude.hx:157-158）
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 0;
            anal.Wetness = 0;

            // hips.rating = Hips.RATING_AVERAGE(4) / butt.rating = Butt.RATING_AVERAGE(4)（JeanClaude.hx:160-161）
            Body.Hips = 4;
            Body.Butt = 4;

            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.Lizard;
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Lizard;

            // 原版 skin.desc = "green-purple mottled hide"（JeanClaude.hx:164）
            // C# 的皮肤描述走本地化 key（SkinPart.DescKey），所以给它一条专属 key。
            Body.Get<SkinPart>().DescKey = "Npcs.JeanClaude.SkinDesc";

            // initStrTouSpeInte(80, 100, 80, 60) / initLibSensCor(40, 40, 80)（JeanClaude.hx:165-166）
            Strength = 80;
            Toughness = 100;
            Speed = 80;
            Intelligence = 60;
            Libido = 40;
            Sensitivity = 40;
            Corruption = 80;

            // 原版 face.type = (game.noFur ? Face.HUMAN : Face.LIZARD)（JeanClaude.hx:167）
            // C# 里 game.noFur 的既有等价写法是 !Player.HasFur()（见 JojoEnemy）
            Body.Get<FacePart>().Id = CoC.Instance.Player.HasFur()
                ? VanillaBodyPartTypes.Lizard
                : VanillaBodyPartTypes.Human;

            Body.Get<EarsPart>().Id = VanillaBodyPartTypes.Lizard;
            Body.Get<EyesPart>().Id = VanillaBodyPartTypes.Basilisk;

            WeaponName = "cutlass";
            WeaponVerb = "slash";
            WeaponAttack = 20;

            ArmorName = "leather tunic";
            ArmorDefense = 20;

            // 原版 bonusHP = 400（JeanClaude.hx:175）：走最大生命修饰层（数据表里 bonusHealth 留 0，避免重复叠加）
            var bonusHealth = 400f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Lust = 5;
            LustVulnerability = 0.6f;
            Level = 20;

            // 原版 gems = 300 + Utils.rand(55)（JeanClaude.hx:182）
            Gems = Random.Shared.Next(55) + 300;

            // 原版 drop = NO_DROP（JeanClaude.hx:184）：数据表 loot 留空即可

            CreatePerk(VanillaPerks.BasiliskResistance);
            CreatePerk(VanillaPerks.BlindImmune);
        }

        // 原版 shouldMove()（JeanClaude.hx:27-30）：他只有冲锋近战，所以永远不移动。
        public override bool ShouldMove(CombatDistance newPos, bool forceAction = false) => false;

        // 原版 handleTease(lustDelta, successful)（JeanClaude.hx:40-72）。
        // 调用点在原版 CombatTeases.hx:1762（命中，successful = true）与 CombatTeases.hx:1792（未命中，false）。
        // C# 的挑逗没有命中判定（CombatResolver.PlayerTease 必定结算伤害），所以这里等价于
        // 原版 successful = true 的分支；false 分支（「你咬牙忍住」那段）在 C# 不可达，保留说明。
        // 基础欲望由基类 Creature.ApplyTease 先加（对应原版 applyTease），本覆写只做原版
        // 叙事、脱衣、欲望易感度 +0.1 与玩家 +20 欲望。
        public override void OnTeased(CombatContext context, float lustDelta)
        {
            var player = context.Player;

            if (!player.HasStatusEffect(RemovedArmorId) && player.Armor != Armor.Nothing)
            {
                context.Append("Combat.JeanClaude.HandleTease.JeanClaudeStops".Localized());
                context.Append("Combat.JeanClaude.HandleTease.HeHusksHis".Localized());

                // successful = true：原版在这里输出「你被催眠着脱下全部衣服」的两段（JeanClaude.hx:48-49）
                context.Append("Combat.JeanClaude.HandleTease.YourIntentIs".Localized());
                context.Append("Combat.JeanClaude.HandleTease.JeanClaudeBreathes".Localized());

                player.CreateStatusEffect(RemovedArmorId);

                if (!HasStatusEffect(JcLustLevelId))
                {
                    CreateStatusEffect(JcLustLevelId, 1, 0, 0, 0);
                    LustVulnerability += 0.1f;
                }
                else
                {
                    ChangeStatusValue(JcLustLevelId, 0, 1);
                }

                player.TakeLustDamage(20, true);
            }
            else
            {
                context.Append("Combat.JeanClaude.HandleTease.JeanClaudeSighs".Localized());

                // 原版（JeanClaude.hx:67-69）：successful 时 applyTease(lustDelta)（基类已经加过，这里不重复加）
                player.TakeLustDamage(20, true);
            }
        }

        // 原版 react()（JeanClaude.hx:74-113）：
        //   AfterAttacked —— 玩家打他之后用催眠金眼反击（欲望按玩家 lust 分档递增，再 +25）
        //   Blinded       —— 玩家用致盲法术时，他有概率反制：成功则反噬伤害，失败则自己被晃瞎
        // 两个派发点现在都在：
        //   AfterAttacked → CombatResolver.PlayerAttack 末尾（对应原版 Combat.hx:1019）
        //   Blinded       → 玩家致盲法术结算处（对应原版 CombatAbilities.hx:297,6587）
        // 本类没有 replacesDescribeAttacked 覆写（JeanClaude.hx 全文 0 处），所以不接 DescribeAttacked。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            var player = context.Player;

            switch (reaction)
            {
                case CombatReactionContext.AfterAttacked:
                    if (!player.HasStatusEffect(VanillaStatusEffects.FirstAttack) &&
                        player.StatusEffectValue(VanillaStatusEffects.CounterAB, 0) != 1)
                    {
                        if (Health < 1 || Lust >= MaxLust)
                            return true;

                        if (player.Lust <= 30)
                        {
                            context.Append("Combat.JeanClaude.React.JeanClaudeDoesn".Localized());
                            context.Append("Combat.JeanClaude.React.HeSaysThe".Localized());
                        }
                        else if (player.Lust <= 50)
                        {
                            context.Append("Combat.JeanClaude.React.AgainYourThumps".Localized());
                            context.Append("Combat.JeanClaude.React.HeSaysHe".Localized());
                        }
                        else if (player.Lust <= 80)
                        {
                            context.Append("Combat.JeanClaude.React.GrowlsJeanClaude".Localized());
                        }
                        else
                        {
                            context.Append("Combat.JeanClaude.React.YouCanT".Localized());
                        }

                        player.TakeLustDamage(25, true);

                        return true;
                    }

                    return true;

                case CombatReactionContext.Blinded:
                    context.Append("Combat.JeanClaude.React.JeanClaudeHowls".Localized());
                    context.Append("Combat.JeanClaude.React.HeSnarlsThe".Localized());

                    // 原版（JeanClaude.hx:100）：Utils.rand(player.spe) >= 50 || Utils.rand(player.inte) >= 50
                    if (context.Random.Next(Math.Max(1, (int)player.Speed)) >= 50 ||
                        context.Random.Next(Math.Max(1, (int)player.Intelligence)) >= 50)
                    {
                        context.Append("Combat.JeanClaude.React.TheLightSears".Localized());
                        context.Append("Combat.JeanClaude.React.TheMonsterSnaps".Localized());

                        // 原版 game.combat.doDamage(Std.int(10 + (player.inte / 3 + rand(player.inte / 2)) * player.spellMod()), true, true)（JeanClaude.hx:103）
                        var damage = (int)(10 +
                            (player.Intelligence / 3f +
                             context.Random.Next(Math.Max(1, (int)(player.Intelligence / 2)))) *
                            player.SpellMod());

                        context.DealDamageToPlayer(damage);
                    }
                    else
                    {
                        context.Append("Combat.JeanClaude.React.TheLightSears2".Localized());
                        context.Append("Combat.JeanClaude.React.Text".Localized());

                        // 原版 player.createStatusEffect(StatusEffects.Blind, Utils.rand(4) + 1, 0, 0, 0)（JeanClaude.hx:107）
                        player.CreateStatusEffect(VanillaStatusEffects.Blind,
                            context.Random.Next(4) + 1, 0, 0, 0);
                    }

                    return false;

                default:
                    return true;
            }
        }

        // 原版 defeated()（JeanClaude.hx:32-34）：game.lethicesKeep.jeanClaude.beatDaFuckhugeBasilisk(hpVictory)
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep.JeanClaudeScenes>.Get()
                .BeatDaFuckhugeBasilisk(hpVictory);

            return true;
        }

        // 原版 won()（JeanClaude.hx:36-38）：game.lethicesKeep.jeanClaude.fuckhugeBasiliskFuckedYouUp(hpVictory)
        public override bool Won(bool hpVictory)
        {
            MonoCoC.Contents.Scenes.SceneInstance<MonoCoC.Contents.Scenes.Vanilla.Dungeons.LethicesKeep.JeanClaudeScenes>.Get()
                .FuckhugeBasiliskFuckedYouUp(hpVictory);

            return true;
        }
    }
}
