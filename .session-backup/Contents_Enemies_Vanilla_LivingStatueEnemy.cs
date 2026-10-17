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
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes.scenes.dungeons.lethicesKeep.LivingStatue extends Monster（LivingStatue.hx）
    // 招式见 Contents/Combat/VanillaHandlers/LivingStatueCombatHandlers.cs；选招在下面的 ChooseCombatAction
    // （原版 performCombatAction 是手写的，不走 MonsterAI）。
    [Register("coc:living_statue")]
    public class LivingStatueEnemy : Enemy
    {
        private static readonly Identifier ConcussiveBlowId = new("coc", "living_statue_concussive_blow");

        private static readonly Identifier DirtKickId = new("coc", "living_statue_dirt_kick");

        private static readonly Identifier BackhandId = new("coc", "living_statue_backhand");

        private static readonly Identifier OverhandSmashId = new("coc", "living_statue_overhand_smash");

        private static readonly Identifier DisarmId = new("coc", "living_statue_disarm");

        private static readonly Identifier CycloneStrikeId = new("coc", "living_statue_cyclone_strike");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new()（LivingStatue.hx:23-55）逐项照抄
            Race = "Statue";

            // initStrTouSpeInte(100, 80, 25, 50) / initLibSensCor(10, 10, 0)
            Strength = 100;
            Toughness = 80;
            Speed = 25;
            Intelligence = 50;
            Libido = 10;
            Sensitivity = 10;
            Corruption = 0;

            // 原版 lustVuln = 0（完全不吃挑逗）
            LustVulnerability = 0f;

            // 原版 tallness = 16 * 12
            Tallness = 192;

            // 原版 createBreastRow(0, 1)
            CreateBreastRow(0, 1);

            // 原版 initGenderless()（LivingStatue.hx:38）：没有任何生殖器，
            // 代词由 Enemy.Pronoun1-3 按 HasCock/HasVagina 推断成 it/it/its，正好一致。

            // 原版 fireRes = 0：火焰伤害为 0
            FireResistance = 0f;

            Level = 22;

            // 原版 bonusHP = 1000（LivingStatue.hx:43）：走最大生命修饰层（数据表 bonusHealth 留 0）
            var bonusHealth = 1000f;

            Stats.AddModifier(VanillaStats.MaxHealth, ModifierOperation.Addition,
                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            WeaponName = "stone greathammer";
            WeaponVerb = "smash";
            WeaponAttack = 25;

            ArmorName = "cracked stone";

            // 原版 drop = NO_DROP（LivingStatue.hx:40）：数据表 loot 留空即可

            CreatePerk(VanillaPerks.StunImmune);
            CreatePerk(VanillaPerks.PoisonImmune);
            CreatePerk(VanillaPerks.BleedImmune);
            CreatePerk(VanillaPerks.BlindImmune);
        }

        // 原版 handleStun()（LivingStatue.hx:57-60）：无视眩晕，照常出手。
        // 原版只 return true、不删状态；这里按项目既有写法（YamataEnemy.HandleFear 等）把状态删掉，
        // 避免战斗界面永久挂着「眩晕」字样。
        public override bool HandleStun(CombatContext context)
        {
            context.WriteAction("Combat.LivingStatue.HandleStun.TheStoneGiant".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Stunned);

            return true;
        }

        // 原版 handleFear()（LivingStatue.hx:62-65）：无视恐惧，照常出手（状态删除理由同上）。
        public override bool HandleFear(CombatContext context)
        {
            context.WriteAction("Combat.LivingStatue.HandleFear.TheStoneGiant".Localized());

            RemoveStatusEffect(VanillaStatusEffects.Fear);

            return true;
        }

        // 原版 handleBlind()（LivingStatue.hx:67-69）：直接 return true。
        // 注意：Haxe 侧 handleBlind() 在整个 src 里只有定义、没有调用点（Monster.hx:1085 起各覆写都是死代码），
        // 所以这里不再实现；雕像真正的「免疫致盲」在下面的 react(Blinded)。
        // C# 没有 handleBlind 钩子；Blinded 反应已有派发点（CombatResolver.PlayerSpellHandlers 的致盲法术），
        // 所以下面 react(Blinded) 会生效。

        // 原版 react(Blinded)（LivingStatue.hx:71-79）：致盲法术对它无效，输出一句并吃掉这次反应（return false）。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.Blinded)
                return true;

            context.Append("Combat.LivingStatue.React.TheGolemDoesn".Localized());

            return false;
        }

        // 原版 performCombatAction()（LivingStatue.hx:191-211）：
        //   血量 < 70% 且不在远距离 → 1/2 概率 backhand；
        //   血量 < 40% 且自己没挂 Disarmed 标记、玩家还能被缴械 → 1/2 概率 disarm；
        //   否则在 [dirtKick, concussiveBlow, cycloneStrike, cycloneStrike, overhandSmash] 里等概率抽一招，
        //   其中前两招只在玩家没有失明/眩晕时进入选项。
        // 原版这里是手写的，没有 MonsterAI；所以整段覆写 ChooseCombatAction（不走数据 AI 表）。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var player = context.Player;

            if (Health < MaxHealth * 0.7f &&
                Distance != CombatDistance.Distant &&
                context.Random.Next(2) == 0)
            {
                return new CombatAiAction { Ability = BackhandId };
            }

            if (Health < MaxHealth * 0.4f &&
                !HasStatusEffect(VanillaStatusEffects.Disarmed) &&
                player.CanDisarm() &&
                context.Random.Next(2) == 0)
            {
                return new CombatAiAction { Ability = DisarmId };
            }

            var options = new List<Identifier>();

            if (!player.HasStatusEffect(VanillaStatusEffects.Blind) &&
                !player.HasStatusEffect(VanillaStatusEffects.Stunned))
            {
                options.Add(DirtKickId);
                options.Add(ConcussiveBlowId);
            }

            // 原版 opts 里 cycloneStrike 塞了两次，所以它的权重是 2/5
            options.Add(CycloneStrikeId);
            options.Add(CycloneStrikeId);
            options.Add(OverhandSmashId);

            return new CombatAiAction { Ability = options[context.Random.Next(options.Count)] };
        }

        // 原版 replacesDescribeAttacked()（LivingStatue.hx:213-236）：玩家打中雕像时的专属战报
        // （按伤害分档换措辞，武器带 channeling 时不接管）。
        // C# 的派发点在 CombatResolver.PlayerAttack（Enemy.DescribeAttacked），返回 true 时不再输出通用战报。
        // 差异：原版第三个参数 crit 在 C# 签名里没有，所以「Critical hit!」那句没接；
        // 原版 weapon.attackNoun 在 C# 没有对应字段，用武器名（Item.Name）代替（注释见 loc 里的键）。
        public override bool DescribeAttacked(CombatContext context, float damage)
        {
            var player = context.Player;
            var weapon = player.Weapon;

            // 原版：通道法杖（channeling）不接管这段战报
            if (weapon.IsChanneling(player))
                return false;

            var monster = Article + ShortName;

            var noun = weapon.Name;
            var verb = string.IsNullOrEmpty(weapon.Verb) ? "attack" : weapon.Verb;

            // 原版这三档（LivingStatue.hx:215-217）是「或」关系，顺序照抄
            var damageLow = damage < 15 || damage < MaxHealth * 0.05f;
            var damageMed = damage < 50 || damage < MaxHealth * 0.20f;
            var damageHigh = damage < 100 || damage < MaxHealth * 0.33f;

            if (damage <= 0)
            {
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Bounce".Localized(noun, monster));
            }
            else if (damageLow)
            {
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Glancing".Localized(monster));
            }
            else if (damageMed)
            {
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Medium".Localized(verb, monster, Pronoun2));
            }
            else if (damageHigh)
            {
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.High".Localized(monster, noun));
            }
            else
            {
                context.WriteAction("Combat.LivingStatue.DescribeAttacked.Shatter".Localized(monster, noun));
            }

            // 原版 getDamageText(damage)：C# 复用共享键 Combat.Monster.Attack.Damage（"<b>({0})</b>"）
            context.Append("Combat.Monster.Attack.Damage".Localized((int)damage));

            return true;
        }

        // 原版 defeated()（LivingStatue.hx:14-17）：
        //   flags[KFLAGS.D3_STATUE_DEFEATED] = 1;
        //   game.lethicesKeep.livingStatue.beatUpDaStatue(hpVictory);
        public override bool Defeated(bool hpVictory)
        {
            MonoCoC.CoC.Instance.Flags.Set(VanillaFlags.D3StatueDefeated, 1);

            SceneInstance<LivingStatueScenesScene>.Get().BeatUpDaStatue(hpVictory);

            return true;
        }

        // 原版 won()（LivingStatue.hx:19-21）：
        //   game.lethicesKeep.livingStatue.fuckinMarbleOP(hpVictory, pcCameWorms);
        // C# 的 Won(bool) 没有 pcCameWorms 参数，按默认 false 处理。
        public override bool Won(bool hpVictory)
        {
            SceneInstance<LivingStatueScenesScene>.Get().FuckinMarbleOP(hpVictory);

            return true;
        }
    }
}
