using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 DemonFistFighter extends Monster（src/classes/scenes/places/bazaar/DemonFistFighter.hx）
    // 只移植类本体；defeated/won 依赖 DemonFistFighterScene（未移植，原版 DemonFistFighter.hx:61-79）。
    [Register("coc:demon_fist_fighter")]
    public class DemonFistFighterEnemy : Enemy
    {
        // 原版 statusEffects.combat.AngeredPugilist：C# 未移植，按原版 key 直接建。
        private static readonly Identifier AngeredPugilist = new("coc", "angered_pugilist");

        public bool AngryDemonfist;

        public bool HasAttemptedBSwordDisarm;

        private static readonly Identifier QuickJabId = new("coc", "demon_fist_fighter_quick_jab");
        private static readonly Identifier TwoFerId = new("coc", "demon_fist_fighter_two_fer");
        private static readonly Identifier AttackId = new("coc", "demon_fist_fighter_attack");
        private static readonly Identifier RoundhouseId = new("coc", "demon_fist_fighter_roundhouse");
        private static readonly Identifier CrossPunchId = new("coc", "demon_fist_fighter_cross_punch");

        // 原版 statusEffects.combat.DazedDebuff（DazedDebuff.hx:9-25）：
        //   duration 2；value1 = accuracy 减量（默认 -15，反向脚跟踢 -20）；value2 = 物理伤害倍率（0.8 / 0.7）。
        //   onAttach 时 boostsAccuracy(value1) + boostsPhysDamage(value2, true)；TimedStatusEffect 每回合递减。
        public static readonly Identifier DazedStatus = new("coc", "dazed");

        public override void TickStatuses(CombatContext context)
        {
            var player = context.Player;
            var dazed = player.StatusEffectByType(DazedStatus);

            if (dazed == null)
                return;

            if (dazed.Values[0] <= 0)
            {
                RemoveDazed(player);

                return;
            }

            dazed.Values[0] -= 1;

            if (dazed.Values[0] <= 0)
                RemoveDazed(player);
        }

        // 原版 player.addStatusEffect(new DazedDebuff(...))（DemonFistFighter.hx:162/171）。
        // 两个修饰共用同一个 source id（coc:combat_buff_dazed），这样：
        //   - 正常到期：RemoveStatusEffect(Dazed) → OnStatusRemoved 按 LinkedBuffs 记录一次撤掉两个修饰；
        //   - 战斗结束：ClearCombatStatuses 移除 combatOnly 的 Dazed，ClearLinkedBuffs 同样撤掉两个修饰。
        private static readonly Identifier DazedBuffSourceId = new("coc", "combat_buff_dazed");

        public static void ApplyDazed(Player player, int duration, float accuracyDecrease, float damageFactor)
        {
            var existing = player.StatusEffectByType(DazedStatus);

            if (existing == null)
                player.CreateStatusEffect(DazedStatus, duration, accuracyDecrease, damageFactor);
            else
            {
                existing.Values[0] = duration;
                existing.Values[1] = accuracyDecrease;
                existing.Values[2] = damageFactor;
            }

            var source = new StatusSource(DazedBuffSourceId, "Dazed");

            player.Stats.RemoveFrom(source);

            // Accuracy 走 AddCombatBuff（会登记 LinkedBuffs，供 OnStatusRemoved / ClearLinkedBuffs 撤掉同 source 的修饰）
            CombatCalculator.AddCombatBuff(player, VanillaStats.Accuracy, accuracyDecrease, "Dazed", DazedStatus);

            // 物理伤害倍率（原版 boostsPhysDamage，如 ×0.8）只能走 Factor 修饰
            player.Stats.AddModifier(VanillaStats.PhysDamage, ModifierOperation.Factor,
                () => damageFactor, source);
        }

        private static void RemoveDazed(Player player)
        {
            player.RemoveStatusEffect(DazedStatus);
            player.Stats.RemoveFrom(new StatusSource(DazedBuffSourceId, "Dazed"));
        }

        // 原版 performCombatAction（DemonFistFighter.hx:81-97）：
        //   疲劳满且没生气 → faint；玩家上一招是魔法且没生气 → getAngry；
        //   否则按权重抽 quickJab/twoFer/eAttack/roundhouse/crossPunch。
        public override bool BeforeCombatAction(CombatContext context)
        {
            if (Fatigue >= MaxFatigue() && !AngryDemonfist)
            {
                context.Append("Combat.DemonFistFighter.Faint.TheDemonS".Localized());

                Health = 0;

                return true;
            }

            var last = CombatController.Instance?.CurrAbilityUsed;

            if (last != null && !AngryDemonfist &&
                CombatAbilityRegistry.Get(last.Value)?.Type == CombatAbilityType.Magical)
            {
                context.Append("Combat.DemonFistFighter.GetAngry.TheDemonScowls".Localized());

                if (!HasStatusEffect(AngeredPugilist))
                    CreateStatusEffect(AngeredPugilist, 99);

                AngryDemonfist = true;

                return true;
            }

            return false;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var choices = new List<(Identifier Id, float Weight)>
            {
                (QuickJabId, 1f),
                (TwoFerId, 1f),
                (AttackId, 1f),
                (RoundhouseId, context.Player.ShieldName is "" or "nothing" ? 0.5f : 0.75f),
                (CrossPunchId, 1f)
            };

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

        // 原版 react(PlayerWaited)（DemonFistFighter.hx:99-115）：玩家等待时嘲讽、自己 fatigue -5、占用行动。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            if (reaction != CombatReactionContext.PlayerWaited)
                return true;

            context.Clear();
            context.Append("Combat.DemonFistFighter.React.YouDecideTo".Localized());

            var silly = MonoCoC.OptionSystem.OptionStore.Instance.Get("silly", 0) > 0;
            var taunt = context.Random.Next(silly ? 4 : 3);

            context.Append(("Combat.DemonFistFighter.React.Taunt" + taunt).Localized());

            ChangeFatigue(-5);
            TookAction = true;

            return false;
        }

        // 原版 defeated（DemonFistFighter.hx:61-69）：DEMONS_DEFEATED += 1，然后交给场景结算。
        // DemonFistFighterScene 未移植（原版 DemonFistFighter.hx:61-79），这里只做 flag 记账并放行通用胜利流程。
        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(VanillaFlags.DemonsDefeated, CoC.Instance.Flags.Get(VanillaFlags.DemonsDefeated) + 1);

            return false;
        }

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 createCock(rand(2) + 11, 2.5, DEMON); balls = 2; ballSize = 1
            CreateCock(Random.Shared.Next(2) + 11, 2.5f, VanillaBodyPartTypes.Demon);
            Balls = 2;
            BallSize = 1;

            // 原版 createBreastRow(0)
            CreateBreastRow(0);

            // 原版 ass.analLooseness = LOOSENESS_NORMAL(2); ass.analWetness = WETNESS_NORMAL(1)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 2;
            anal.Wetness = 1;

            // 原版 tallness = Utils.rand(24) + 25
            Tallness = Random.Shared.Next(24) + 25;

            // 原版 hips.rating = RATING_BOYISH(0); butt.rating = RATING_TIGHT(2)
            Body.SetHipsRating("boyish");
            Body.SetButtRating("tight");

            // 原版 skin.tone = "indigo"; hair.color = "black"; hair.length = 5
            Body.Get<SkinPart>().Tone = "indigo";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";
            hair.Length = 5;

            // 原版 weaponName = "fists"; weaponVerb = "jab"
            WeaponName = "fists";
            WeaponVerb = "jab";

            // 原版 armorName = "leathery skin"
            ArmorName = "leathery skin";

            // 原版 gems = Utils.rand(5) + 5：数据表里的 gems 只当占位，实际在这里摇
            Gems = Random.Shared.Next(5) + 5;

            // 原版 createPerk(ExtraDodge, 75)
            if (!HasPerk(VanillaPerks.ExtraDodge))
                CreatePerk(VanillaPerks.ExtraDodge, 75);

            // 原版 lustVuln = 0.1
            LustVulnerability = 0.1f;

            // 原版 temperment = TEMPERMENT_LUSTY_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            //
            // 原版 defeated()：DemonFistFighter.hx:61-69（scene.playerWins / scene.lustKO）
            // 原版 won()：DemonFistFighter.hx:71-79（scene.angryDemonfistPlayerLoss / regularPlayerLoss）
            // C# 的 DemonFistFighterScene 未移植（原版 DemonFistFighter.hx:61-79；Contents/Scenes 全量无此类），
            // 所以 won 不覆写；defeated 只保留 DEMONS_DEFEATED 记账。
        }
    }
}
