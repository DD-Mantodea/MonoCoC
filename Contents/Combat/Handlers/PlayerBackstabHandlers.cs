using MonoCoC.Attributes;
using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.StatusEffects;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 CombatAbilities.hx:5571-5597 / BackstabBuff.hx
    public static class PlayerBackstabHandlers
    {
        private static readonly Identifier KnifeMastery = new("coc", "Knife");

        [Register("coc:can_backstab")]
        public static bool CanBackstab(CombatContext context, CombatState state)
        {
            var player = context.Player;

            return player.Weapon.IsKnife() && player.MasteryLevel(KnifeMastery) >= 2;
        }

        [Register("coc:special_backstab")]
        public static void Backstab(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Combat.Special.Backstab.Start".Localized());

            // Values[1] 记录施放时的 HP，用于判断本回合是否被命中（近似原版 BackstabBuff 的 value1 标记）
            player.CreateStatusEffect(StatusEffectIds.Backstab, 0, player.Health, 0, 0, 1);
        }

        [Register("coc:effect_backstab_expire")]
        public static void Expire(CombatContext context, Creature host, StatusEffect status)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            if (status.Values[1] < player.Health)
            {
                context.Append("Combat.Special.Backstab.Fail".Localized());

                return;
            }

            context.Append("Combat.Special.Backstab.Success".Localized());

            var damage = CombatCalculator.PhysicalDamage(context, player, enemy) * 1.5f;

            CombatCalculator.DamageEnemy(context, damage, CombatDamageKind.Melee);

            player.MasteryXP(KnifeMastery, 10 + context.Random.Next(21));
        }
    }
}