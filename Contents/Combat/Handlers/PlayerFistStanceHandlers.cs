using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:6925-6949 monkStance / brawlerStance
    public static class PlayerFistStanceHandlers
    {
        private static readonly Identifier FistMastery = new("coc", "Fist");

        [Register("coc:can_fist_stance")]
        public static bool CanFistStance(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.HasPerk(PerkIds.FistsOfFury)

                && player.Weapon.IsBareFist()

                && player.Shield.Id.Path == "nothing";
        }

        private static int Mastery(Player player) => player.MasteryLevel(FistMastery);

        private static void StanceChange(Player player, Identifier stance)
        {
            player.RemoveStatusEffect(StatusEffectIds.MonkStance);

            player.RemoveStatusEffect(StatusEffectIds.BrawlerStance);

            player.RemoveStatusEffect(StatusEffectIds.ClawStance);

            player.CreateStatusEffect(stance);
        }

        [Register("coc:monk_stance")]
        public static void MonkStance(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var mastery = Mastery(player);

            context.Append("Combat.Ability.MonkStance.Text".Localized());

            StanceChange(player, StatusEffectIds.MonkStance);

            CombatCalculator.AddCombatBuff(player, GameStats.Dodge, 10, "MonkStance", StatusEffectIds.MonkStance);

            CombatCalculator.AddCombatBuff(player, GameStats.Parry, 10 * mastery, "MonkStance", StatusEffectIds.MonkStance);

            CombatCalculator.AddCombatBuff(player, GameStats.Accuracy, 3 * mastery, "MonkStance", StatusEffectIds.MonkStance);

            if (mastery >= 5)
                CombatCalculator.AddCombatBuff(player, GameStats.MovementChance, 25, "MonkStance", StatusEffectIds.MonkStance);

            if (mastery >= 5)
            {
                context.Append("Combat.Ability.MonkStance.Activation".Localized());

                player.CreateStatusEffect(StatusEffectIds.TrueParry, 0, 0, 0, 0, 1);

                CombatCalculator.AddCombatBuff(player, GameStats.Parry, 100, "TrueParry", StatusEffectIds.TrueParry);
            }
        }

        [Register("coc:brawler_stance")]
        public static void BrawlerStance(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            var mastery = Mastery(player);

            context.Append("Combat.Ability.BrawlerStance.Text".Localized());

            StanceChange(player, StatusEffectIds.BrawlerStance);

            CombatCalculator.AddCombatBuff(player, GameStats.PhysDamage, 0.03f * mastery, "BrawlerStance", StatusEffectIds.BrawlerStance);

            if (mastery >= 5)
            {
                context.Append("Combat.Ability.BrawlerStance.Activation".Localized());

                CombatRangeData.CloseDistance(enemy);

                var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * 0.5f;

                CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);
            }
        }

        [Register("coc:effect_monkstance_round")]
        public static void MonkRound(CombatContext context, Creature host, StatusEffect status) =>

            CheckEquipment(context, (Player)host, StatusEffectIds.MonkStance);

        [Register("coc:effect_brawlerstance_round")]
        public static void BrawlerRound(CombatContext context, Creature host, StatusEffect status) =>

            CheckEquipment(context, (Player)host, StatusEffectIds.BrawlerStance);

        private static void CheckEquipment(CombatContext context, Player player, Identifier stance)
        {
            if (player.Weapon.IsBareFist() && player.Shield.Id.Path == "nothing")
                return;

            context.Append("Combat.Ability.Stance.Broken".Localized());

            player.RemoveStatusEffect(stance);
        }
    }
}