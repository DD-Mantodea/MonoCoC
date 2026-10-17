using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 classes/scenes/monsters/PlaceholderDodgyMonster.hx（extends Monster）
    // 巴扎 FapArena 的「狗头人替身」占位敌人：会记住玩家用过的招式并针对它提升闪避。
    [Register("coc:placeholder_dodgy_monster")]
    public class PlaceholderDodgyMonsterEnemy : Enemy
    {
        // 原版字段（PlaceholderDodgyMonster.hx:14-16）
        public readonly Dictionary<string, int> AbilityDodgeChance = [];

        public bool DodgeStance;

        public string TossedDagger = "";

        // 原版 StatusEffects.DodgedAttack：注意原版在 Combat.hx:1125-1130 已经把「挂这个状态」的
        // 代码整段注释掉了，所以 dodgeExec 的条件在实战里永远不成立（原版的死分支）。
        // 这里仍保留同样的条件判断，保持语义一致。
        private static readonly Identifier DodgedAttack = new("coc", "dodged_attack");

        // 原版 game.bazaar.demonFistFighterScene.saveContent.shookDogHand
        // （PlaceholderDodgyMonster.hx:29/50/163 读取它）。
        // 依赖场景类 DemonFistFighterScene 在 Contents 全量搜索 0 命中（数据缺失），
        // 这里按「没握过手」的默认分支处理。
        public static bool ShookDogHand => false;

        private static readonly Identifier AttackId = new("coc", "placeholder_dodgy_monster_attack");

        private static readonly Identifier DodgeExecId = new("coc", "placeholder_dodgy_monster_dodge_exec");

        private static readonly Identifier TossDaggerId = new("coc", "placeholder_dodgy_monster_toss_dagger");

        private static readonly Identifier RetrieveDaggerId = new("coc", "placeholder_dodgy_monster_retrieve_dagger");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // ── 原版 new() 逐项照抄（PlaceholderDodgyMonster.hx:125-169）──
            // createCock(6, 2, CockTypesEnum.DOG)
            CreateCock(6, 2, VanillaBodyPartTypes.Dog);

            Balls = 2;
            BallSize = 3;

            CreateBreastRow(0);

            // ass.analLooseness = LOOSENESS_TIGHT(1) / analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 1;
            anal.Wetness = 1;

            // tallness = 5 * 12 + 9
            Tallness = 5 * 12 + 9;

            // hips.rating = RATING_BOYISH(0) / butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "light gray";

            var hair = Body.Get<HairPart>();
            hair.Color = "light gray";
            hair.Length = 0;

            // horns.type = Horns.NONE
            Body.Get<HornsPart>().Id = VanillaBodyPartTypes.None;

            // 原版 level = 20 / lust = 5 / 属性都在数据表里。
            WeaponName = "serrated daggers";
            WeaponVerb = "stab";
            WeaponAttack = 25;

            // if (!game.bazaar.demonFistFighterScene.saveContent.shookDogHand) weaponAttack += 10;
            if (!ShookDogHand)
                WeaponAttack += 10;

            ArmorName = "leather armor";
            ArmorDefense = 5;

            // 原版 gems = Utils.rand(5) + 250
            Gems = Random.Shared.Next(5) + 250;

            // createPerk(PerkLib.Prescience) / Bloodhound / Tactician
            CreatePerk(VanillaPerks.Prescience);
            CreatePerk(VanillaPerks.Bloodhound);
            CreatePerk(VanillaPerks.Tactician);

            // 原版 drop = WeightedDrop(DBLPEPP 2)：见 PlaceholderDodgyMonster.hjson loot。
        }

        // 原版 getEvasionChance()（PlaceholderDodgyMonster.hx:18-24）：
        // 对「玩家上一次使用的招式」累加闪避率（每次 +15 或 +20）。
        public override float GetEvasionChance()
        {
            var chance = base.GetEvasionChance();

            if (CombatController.Instance?.CurrAbilityUsed is { } used
                && AbilityDodgeChance.TryGetValue(used.Path, out var extra))
            {
                chance += extra;
            }

            return chance;
        }

        // 原版 performCombatAction（PlaceholderDodgyMonster.hx:26-44）：
        // 先按玩家上回合用的招式累加闪避，再走 MonsterAI 权重表，最后可能进入闪避姿态。
        // AI 表的四条与权重写死在下面（数据文件里也留了一份等价配置，便于对照）。
        public override bool BeforeCombatAction(CombatContext context)
        {
            // 原版：if (currAbilityUsed != null) 累加 abilityDodgeChance
            if (CombatController.Instance?.CurrAbilityUsed is { } used)
            {
                var amount = ShookDogHand ? 20 : 15;

                AbilityDodgeChance[used.Path] = AbilityDodgeChance.GetValueOrDefault(used.Path) + amount;
            }

            var options = new List<(Identifier Id, int Cost, FatigueType Fatigue, float Weight)>();

            // eAttack：weight 1, cost 10 physical, Ranged
            if (HasFatigue(10, FatigueType.Physical))
                options.Add((AttackId, 10, FatigueType.Physical, 1));

            // dodgeExec：weight 99, cost 10 physical，条件 dodgeStance && hasStatusEffect(DodgedAttack)
            if (DodgeStance && HasStatusEffect(DodgedAttack) && HasFatigue(10, FatigueType.Physical))
                options.Add((DodgeExecId, 10, FatigueType.Physical, 99));

            // tossDagger：weight 1, cost 10 physical，条件 distance == Distant
            if (Distance == CombatDistance.Distant && HasFatigue(10, FatigueType.Physical))
                options.Add((TossDaggerId, 10, FatigueType.Physical, 1));

            // retrieveDagger：weight 3, cost 5 physical，条件 tossedDagger != ''
            if (TossedDagger != "" && HasFatigue(5, FatigueType.Physical))
                options.Add((RetrieveDaggerId, 5, FatigueType.Physical, 3));

            if (options.Count == 0)
            {
                // 原版 MonsterAI.exec：没有可选动作时直接 eAttack。
                Execute(context, new CombatAbilityDefinition { Id = AttackId, Handler = AttackId }, 0, FatigueType.None);
            }
            else
            {
                var total = 0f;

                foreach (var option in options)
                    total += option.Weight;

                var roll = context.Random.NextDouble() * total;

                var chosen = options[^1];

                foreach (var option in options)
                {
                    roll -= option.Weight;

                    if (roll <= 0)
                    {
                        chosen = option;
                        break;
                    }
                }

                var ability = CombatAbilityRegistry.Get(chosen.Id)
                    ?? new CombatAbilityDefinition { Id = chosen.Id, Handler = chosen.Id };

                Execute(context, ability, chosen.Cost, chosen.Fatigue);
            }

            // 原版 exec 之后：if (hasFatigue(10, physical) && rand(4) == 0 && !dodgeStance) dodgePrepare();
            //                  else dodgeStance = false;
            if (HasFatigue(10, FatigueType.Physical) && context.Random.Next(4) == 0 && !DodgeStance)
                DodgePrepare(context);
            else
                DodgeStance = false;

            return true;
        }

        // 对应 MonsterAI.exec 的「扣疲劳 + 执行招式」那一半。
        private void Execute(CombatContext context, CombatAbilityDefinition ability, int cost, FatigueType fatigue)
        {
            if (cost > 0)
                ChangeFatigue(cost, fatigue);

            CombatController.Instance!.Resolver.EnemyAction(ability);
        }

        // 原版 dodgePrepare()（PlaceholderDodgyMonster.hx:107-110）
        public void DodgePrepare(CombatContext context)
        {
            context.Append("Combat.PlaceholderDodgyMonster.DodgePrepare.LowersHisWeapons".Localized());

            DodgeStance = true;
        }

        // 原版 defeated() / won() 都是空实现（PlaceholderDodgyMonster.hx:8-12），不需要接场景。
    }
}
