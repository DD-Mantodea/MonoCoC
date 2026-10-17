using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Enemies.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class HolliCombatHandlers
    {

        [Register("coc:holli_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability) =>

            CombatController.Instance!.Resolver.EnemyBasicAttack();

        [Register("coc:holli_heal_holli")]
        public static void HealHolli(CombatContext context, CombatAbilityDefinition ability)
        {
            context.Clear();

            context.Append("Combat.Holli.HealHolli.TheBarkSplits".Localized());

            context.Enemy.HealthChange(200);
        }

        [Register("coc:holli_constrict_attack")]
        public static void ConstrictAttack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Holli.HolliConstrictAttack.AForestOf".Localized());

            if (enemy.HasStatusEffect(StatusEffectIds.Blind) && context.Random.Next(6) == 0)
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

        [Register("coc:holli_fuckin_jamanji_flower_darts")]
        public static void FlowerDarts(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Combat.Holli.FuckinJamanjiFlowerDarts.ABlossomOpens".Localized(enemy.ShortName));

            if (enemy.HasStatusEffect(StatusEffectIds.Blind))
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

        [Register("coc:holli_can_heal")]
        public static bool CanHeal(CombatContext context, CombatState state) =>

            context.Enemy.HealthRatio() < 0.5f;

        [Register("coc:holli_can_constrict")]
        public static bool CanConstrict(CombatContext context, CombatState state) =>

            !context.Player.HasStatusEffect(HolliEnemy.ConstrictStatus);

        public static AvoidResult Avoid(CombatContext context) =>

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