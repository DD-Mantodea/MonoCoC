using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:3868-3943 nagaCalc / nagaChance / nagaBiteAttack
    public static class PlayerNagaBiteHandlers
    {
        [Register("coc:has_snake_fangs")]
        public static bool HasSnakeFangs(CombatContext context, CombatState state) =>

            context.Player.Body.Get<FacePart>().Id.Path == "snake_fangs";

        [Register("coc:effect_naga_bite_round")]
        public static void NagaBiteRound(CombatContext context, Creature host, StatusEffect status)
        {
            var stacks = (int)status.Values[0];

            var amount = 2 * stacks;

            if (amount <= 0)
                return;

            CombatCalculator.AddCombatBuff(host, GameStats.Strength, -amount, "NagaBite", StatusEffectIds.NagaBite);

            CombatCalculator.AddCombatBuff(host, GameStats.Speed, -amount, "NagaBite", StatusEffectIds.NagaBite);

            context.Append("Combat.Effect.NagaBite.Round".Localized());
        }

        private static int NagaChance(CombatContext context)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasPerk(PerkIds.PoisonImmune))
                return 0;

            if (enemy.HasStatusEffect(StatusEffectIds.Constricted))
                return 100;

            var penalty = player.HasPerk(PerkIds.NaturalWeapons) ? 0 : -20;

            return (int)player.StandardDodgeFunc(enemy, penalty);
        }

        [Register("coc:special_naga_bite")]
        public static void NagaBite(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (enemy.HasPerk(PerkIds.PoisonImmune))
            {
                context.Append("Combat.Special.NagaBite.PoisonImmune".Localized());

                return;
            }

            if (enemy.HasPerk(PerkIds.BiteImmune))
            {
                context.Append("Combat.Special.NagaBite.BiteImmune".Localized());

                player.HealthChange(-5);

                return;
            }

            if (NagaChance(context) <= context.Random.Next(100))
            {
                context.Append("Combat.Special.NagaBite.Fail".Localized());

                return;
            }

            context.Append("Combat.Special.NagaBite.Hit".Localized());

            if (!enemy.HasStatusEffect(StatusEffectIds.NagaBite))
                enemy.CreateStatusEffect(StatusEffectIds.NagaBite, 0, 0, 0, 0);

            var stacks = 1;

            if (enemy.HasStatusEffect(StatusEffectIds.Constricted))
                stacks += 1;

            enemy.AddStatusValue(StatusEffectIds.NagaBite, 0, stacks);

            var amount = 5 + context.Random.Next(6);

            CombatCalculator.AddCombatBuff(enemy, GameStats.Strength, -amount * stacks, "NagaBite", StatusEffectIds.NagaBite);

            CombatCalculator.AddCombatBuff(enemy, GameStats.Speed, -amount * stacks, "NagaBite", StatusEffectIds.NagaBite);

            context.Append("Combat.Special.NagaBite.Reduced".Localized());
        }
    }
}