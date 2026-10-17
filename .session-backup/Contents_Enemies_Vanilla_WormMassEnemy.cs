using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Enemies.Vanilla
{
    // 原版 WormMass extends Monster（src/classes/scenes/areas/mountain/WormMass.hx）
    // WormMass 的场景（WormsScene）已移植，won 由场景的 OnWon 回调接 Infest1（见 WormsScene.cs:152）。
    [Register("coc:worm_mass")]
    public class WormMassEnemy : Enemy
    {
        private static readonly Identifier WormAttackId = new("coc", "worm_mass_worm_attack");
        private static readonly Identifier WormsEnticeId = new("coc", "worm_mass_worms_entice");

        // 原版 performCombatAction（WormMass.hx:62-69）：没有 MonsterAI，纯 50/50 在 special1/special2 之间摇。
        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>
            new() { Ability = context.Random.Next(2) == 0 ? WormAttackId : WormsEnticeId };

        // 原版 react（WormMass.hx:75-106）：
        //   BeforeAttacked：50% 概率（rand(100) + inte/3 >= 50）反伤玩家一次，否则打空；无论哪种都取消这次攻击。
        //   BowHit：箭从缝隙穿过，取消这次攻击。
        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            var player = context.Player;

            if (reaction == CombatReactionContext.BeforeAttacked)
            {
                if (context.Random.Next(100) + player.Intelligence / 3f >= 50)
                {
                    var temp = (int)(player.Strength / 5f - context.Random.Next(5));

                    if (temp == 0)
                        temp = 1;

                    context.Append("Combat.WormMass.React.YouStrikeAt".Localized("#ff0000", temp));

                    HealthChange(-temp);

                    if (Health <= 0)
                        return false;
                }
                else
                {
                    context.Append("Combat.WormMass.React.YouAttemptTo".Localized());
                }

                // 原版 FirstAttack 会追加一次 game.combat.attack()：
                // C# 没有同等的「再打一次」入口（在 BeforeAttacked 里调 PlayerAttack 会递归），未接。
                return false;
            }

            if (reaction == CombatReactionContext.BowHit)
            {
                context.Append("Combat.WormMass.React.TheArrowSlips".Localized());

                return false;
            }

            return true;
        }

        // 原版 defeated（WormMass.hx:130-135）：打死时 WORMS_MASS_KILLED += 1，然后走通用收尾。
        public override bool Defeated(bool hpVictory)
        {
            if (hpVictory)
                CoC.Instance.Flags.Set(VanillaFlags.WormsMassKilled, CoC.Instance.Flags.Get(VanillaFlags.WormsMassKilled) + 1);

            return false;
        }

        // 原版 won（WormMass.hx:137-141）：先输出「Overcome by your …」再 infest1。
        // C# 的 WormsScene 用 OnWon 回调接 Infest1，所以这里只补文本并放行通用战败流程。
        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append(
                "Combat.WormMass.Won.OvercomeByYour".Localized(hpVictory ? "wounds" : "lust"));

            return false;
        }

        public override void Configure(EnemyDefinition definition)
        {
            // 原版 maxHP() = 40 + newGamePlusMod() * 20（WormMass.hx:143-145），覆盖基类血量公式。
            // 数据表 bonusHealth 必须为 0，否则会在这之上再叠加。
            var player = CoC.Instance.Player;

            Stats.SetProvider(VanillaStats.MaxHealth, () => 40 + player.NewGamePlusMod() * 20);

            base.Configure(definition);

            // 原版 initGenderless()：不建任何生殖器；createBreastRow(0, 0)（0 号、0 个乳头）
            CreateBreastRow(0f, 0f);

            // 原版 ass.analLooseness = LOOSENESS_VIRGIN(0); ass.analWetness = WETNESS_DRY(0)
            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;
            anal.Wetness = 0;

            // 原版 tallness = 1
            Tallness = 1;

            // 原版 hips.rating = RATING_SLENDER(2); butt.rating = RATING_BUTTLESS(0)
            Body.SetHipsRating("slender");
            Body.SetButtRating("buttless");

            // 原版 skin.tone = "white"
            Body.Get<SkinPart>().Tone = "white";

            // 原版 weaponName = "worm"; weaponVerb = "slap"
            WeaponName = "worm";
            WeaponVerb = "slap";

            // 原版 normalAttackAvoid.doParry = false; doCounter = false
            NormalAttackAvoid.DoParry = false;
            NormalAttackAvoid.DoCounter = false;

            // 原版 armorName = "skin"
            ArmorName = "skin";

            // 原版 lustVuln = 0（完全不吃挑逗）
            LustVulnerability = 0;

            // 原版 temperment = TEMPERMENT_LOVE_GRAPPLES：Monster.hx 里只在「grappling 待实现」注释中被读，死字段，不移植。
            // 原版 drop = NO_DROP（WormMass.hx:181），所以数据表 loot 留空。
            // 原版 describeDodge（WormMass.hx:71-73）在 C# 没有对应派发点，未接。
        }
    }
}
