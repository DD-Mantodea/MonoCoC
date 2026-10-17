using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Combat.VanillaHandlers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 Clara extends Monster（src/classes/scenes/npcs/Clara.hx）
    // 招式见 Contents/Combat/VanillaHandlers/ClaraCombatHandlers.cs。
    //
    // won/defeated 已接 MarblePurification（原版 Clara.hx:123 / :139）：
    //   defeated → MarblePurification.defeatClaraCuntInAFight()
    //   won      → MarblePurification.loseToClara()
    // 两者都 return true，由场景接管收尾。
    [Register("coc:clara")]
    public class ClaraEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            // 原版 new() 逐项照抄
            CreateVagina(false, 1, 2);

            CreateBreastRow(VanillaBreastCups.F);

            // ass.analLooseness = Ass.LOOSENESS_VIRGIN(0) / analWetness = Ass.WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();
            anal.Looseness = 0;
            anal.Wetness = 0;

            // tallness = 6 * 12 + 4
            Tallness = 76;

            // hips.rating = Hips.RATING_CURVY(10) / butt.rating = Butt.RATING_LARGE(8)
            Body.Hips = 10;
            Body.Butt = 8;

            // lowerBody.type = LowerBody.HOOFED
            Body.Get<LowerBodyPart>().Id = VanillaBodyPartTypes.Hoofed;

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();
            hair.Color = "brown";
            hair.Length = 13;

            // initStrTouSpeInte(37, 55, 35, 60)
            Strength = 37;
            Toughness = 55;
            Speed = 35;
            Intelligence = 60;

            // initLibSensCor(25, 45, 40)
            Libido = 25;
            Sensitivity = 45;
            Corruption = 40;

            WeaponName = "mace";
            WeaponVerb = "smack";
            WeaponAttack = 10;

            ArmorName = "tough hide";
            ArmorDefense = 5;

            // 原版 Clara.hx:170 temperment = Monster.TEMPERMENT_RANDOM_GRAPPLES：死字段，
            // AI 权重由 Assets/Data/Combat/Ai/Clara.hjson 表达。

            Level = 5;

            // bonusHP = 30：走数据表 bonusHealth（base.Configure 已经加进最大生命）

            Gems = Random.Shared.Next(5) + 25;

            // 原版 Clara.hx:174 drop = NO_DROP → 数据表不配 loot

            // 原版 tail.type = Tail.COW
            Body.Get<TailPart>().Id = VanillaBodyPartTypes.Cow;
        }

        // 原版 performCombatAction（Clara.hx:95）的回合前后逻辑：
        //   · 营地战（ClaraFoughtInCamp）满 10 回合直接判负
        //   · 每回合累加 ClaraCombatRounds
        //   · 非营地战时每回合额外涨欲
        // C# 没有「招式执行完之后」的回合末钩子，所以整段挂在敌人回合开始（回合计数语义等价：
        // 两边都是「先看计数、再 +1、下一次行动前用新值判断」）。
        public override void CombatRoundUpdate(CombatContext context)
        {
            base.CombatRoundUpdate(context);

            var player = context.Player;

            if (player.HasStatusEffect(VanillaStatusEffects.ClaraFoughtInCamp)
                && player.StatusEffectValue(VanillaStatusEffects.ClaraCombatRounds, 0) >= 10)
                Health = 0;

            if (!player.HasStatusEffect(VanillaStatusEffects.ClaraCombatRounds))
                player.CreateStatusEffect(VanillaStatusEffects.ClaraCombatRounds, 1, 0, 0, 0);
            else
                player.ChangeStatusValue(VanillaStatusEffects.ClaraCombatRounds, 0, 1);

            if (Health > 0 && Lust < MaxLust && !player.HasStatusEffect(VanillaStatusEffects.ClaraFoughtInCamp))
                ClaraCombatHandlers.BonusBaseLustDamage(context);
        }

        // 原版 performCombatAction 里的两个「先手覆盖」分支：
        //   HP < 50 且 rand(2) == 0 → 喝自己的奶回血
        //   玩家失明 → 摸玩家
        // 两者都会占掉本次行动，其余情况返回 false 交给数据驱动 AI 抽四招之一。
        public override bool BeforeCombatAction(CombatContext context)
        {
            // 血量被「回合数」判定清零后，让 CombatController 的 CheckEnd 收尾（不再出手）
            if (Health <= 0)
                return true;

            if (Health < 50 && context.Random.Next(2) == 0)
            {
                ClaraCombatHandlers.LacticAcid(context);

                return true;
            }

            if (context.Player.HasStatusEffect(VanillaStatusEffects.Blind))
            {
                ClaraCombatHandlers.GropesBlind(context);

                return true;
            }

            return false;
        }

        // 原版 Clara.hx:123-137 defeated()：无论 HP 胜还是欲望胜，最后都交给
        // MarblePurification.defeatClaraCuntInAFight() 收尾。
        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<MarblePurification>.Get().DefeatClaraCuntInAFight();

            return true;
        }

        // 原版 Clara.hx:139-141 won()：loseToClara()
        public override bool Won(bool hpVictory)
        {
            SceneInstance<MarblePurification>.Get().LoseToClara();

            return true;
        }
    }
}
