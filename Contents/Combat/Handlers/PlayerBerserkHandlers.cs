using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:2766-2783 / BerserkBuff.hx / LustserkBuff.hx
    public static class PlayerBerserkHandlers
    {
        [Register("coc:can_berserk")]
        public static bool CanBerserk(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.Berserker);

        [Register("coc:can_lustserk")]
        public static bool CanLustserk(CombatContext context, CombatState state) =>

            context.Player.HasPerk(PerkIds.Lustserker);

        [Register("coc:is_berserking")]
        public static bool IsBerserking(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.Berserking);

        [Register("coc:is_lustserking")]
        public static bool IsLustserking(CombatContext context, CombatState state) =>

            context.Player.HasStatusEffect(StatusEffectIds.Lustserking);

        private static void ChargeAndAttack(CombatContext context, Identifier status, string textKey, float armorMultiplier)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            CombatRangeData.CloseDistance(enemy);

            context.Append(textKey.Localized());

            var originalArmor = player.ArmorDefense;

            player.CreateStatusEffect(status, originalArmor, 0, 0, 0, 99);

            player.ArmorDefense = MathF.Max(0, originalArmor * armorMultiplier);

            CombatCalculator.AddCombatBuff(player, GameStats.BodyDamage, 30, "Berserk", status);

            var damage = CombatCalculator.PhysicalDamage(context, player, enemy);

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);
        }

        [Register("coc:special_berserk")]
        public static void Berserk(CombatContext context, CombatAbilityDefinition ability)
        {
            ChargeAndAttack(context, StatusEffectIds.Berserking,

                "Combat.Special.Berserk.Text", context.Player.HasPerk(PerkIds.ColdFury) ? 0.5f : 0f);

            // 原版 BerserkBuff：欲望抗性 ×1.6
            CombatCalculator.AddCombatBuffMultiplier(context.Player, GameStats.LustResistance, 1.6f,

                "Berserk", StatusEffectIds.Berserking);
        }

        [Register("coc:special_lustserk")]
        public static void Lustserk(CombatContext context, CombatAbilityDefinition ability)
        {
            ChargeAndAttack(context, StatusEffectIds.Lustserking,

                "Combat.Special.Lustserk.Text", 1.5f);

            // 原版 LustserkBuff：欲望抗性减半
            CombatCalculator.AddCombatBuffMultiplier(context.Player, GameStats.LustResistance, 0.5f,

                "Lustserk", StatusEffectIds.Lustserking);
        }

        [Register("coc:effect_berserking_expire")]
        public static void BerserkingExpire(CombatContext context, Creature host, StatusEffect status) =>

            host.ArmorDefense = status.Values[0];

        [Register("coc:effect_lustserking_expire")]
        public static void LustserkingExpire(CombatContext context, Creature host, StatusEffect status) =>

            host.ArmorDefense = status.Values[0];
    }
}