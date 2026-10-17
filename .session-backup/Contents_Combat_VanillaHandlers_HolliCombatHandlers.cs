using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.ModLoader;
using System;

namespace MonoCoC.Contents.Combat.VanillaHandlers
{
    // 原版 classes.scenes.npcs.Holli 的招式（Holli.hx:125 / :153 / :215 / :227）。
    public static class HolliCombatHandlers
    {
        // 原版 eAttack()：普攻
        [Register("coc:holli_attack")]
        private static void Attack(CombatContext context, CombatAbilityDefinition ability) =>
            CombatController.Instance!.Resolver.EnemyBasicAttack();

        // 原版 healHolli()（Holli.hx:215）：clearOutput + 回 200 血
        [Register("coc:holli_heal_holli")]
        private static void HealHolli(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Clear();

            context.Append("Combat.Holli.HealHolli.TheBarkSplits".Localized());

            context.Enemy.HealthChange(200);
        }

        // 原版 holliConstrictAttack()（Holli.hx:153）：根须束缚，命中挂 HolliConstrict
        [Register("coc:holli_constrict_attack")]
        private static void ConstrictAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Holli.HolliConstrictAttack.AForestOf".Localized());

            if (enemy.HasStatusEffect(VanillaStatusEffects.Blind) && context.Random.Next(6) == 0)
                context.Append("Combat.Holli.HolliConstrictAttack.LuckilyTheDemon".Localized());

            if (Avoid(context).AttackFailed)
            {
                context.Append("Combat.Holli.HolliConstrictAttack.ItSA".Localized());

                return;
            }

            context.Append("Combat.Holli.HolliConstrictAttack.SheLatchesOnto".Localized());

            player.TakeDamage(10 + context.Random.Next(5));

            player.TakeLustDamage(15, true);

            player.CreateStatusEffect(HolliEnemy.ConstrictStatus, 0, 0, 0, 0);
        }

        // 原版 fuckinJamanjiFlowerDarts()（Holli.hx:125）：20+rand(10) 伤害、10 疲劳、25 欲望
        [Register("coc:holli_fuckin_jamanji_flower_darts")]
        private static void FlowerDarts(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;
            var enemy = context.Enemy;

            context.Append("Combat.Holli.FuckinJamanjiFlowerDarts.ABlossomOpens".Localized(enemy.ShortName));

            // 原版：她被致盲时这句话照出，但新鲜的眼睛仍然瞄得很准（没有命中惩罚）
            if (enemy.HasStatusEffect(VanillaStatusEffects.Blind))
                context.Append("Combat.Holli.FuckinJamanjiFlowerDarts.ThoughTheDemon".Localized());

            if (Avoid(context).AttackFailed)
            {
                context.Append("Combat.Holli.FuckinJamanjiFlowerDarts.NimblyYouStep".Localized());

                return;
            }

            context.Append("Combat.Holli.FuckinJamanjiFlowerDarts.TheDartsFind".Localized());

            context.Append(player.HasCock()
                ? "Combat.Holli.FuckinJamanjiFlowerDarts.ComeInsideMe".Localized()
                : "Combat.Holli.FuckinJamanjiFlowerDarts.HaveMeInside".Localized());

            context.Append("Combat.Holli.FuckinJamanjiFlowerDarts.Forever".Localized());

            player.TakeDamage(20 + context.Random.Next(10));

            player.ChangeFatigue(10);

            player.TakeLustDamage(25, true);
        }

        // ── AI 条件（原版 MonsterAI.add 的 when）──

        // 原版 healHolli 的 when：hp100 < 50
        [Register("coc:holli_can_heal")]
        private static bool CanHeal(CombatContext context, CombatState state) =>
            context.Enemy.HealthRatio() < 0.5f;

        // 原版 holliConstrictAttack 的 when：!player.hasStatusEffect(StatusEffects.HolliConstrict)
        [Register("coc:holli_can_constrict")]
        private static bool CanConstrict(CombatContext context, CombatState state) =>
            !context.Player.HasStatusEffect(HolliEnemy.ConstrictStatus);

        // 原版三处 combatAvoidDamage({doDodge: true, doParry: false, doBlock: false})
        private static AvoidResult Avoid(CombatContext context) =>
            CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,
                Defender = context.Player,
                DoDodge = true,
                DoParry = false,
                DoBlock = false
            });
    }
}
