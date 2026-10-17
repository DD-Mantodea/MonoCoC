using MonoCoC.Attributes;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Combat.Handlers
{
    /// <summary>
    /// 原版 Soulburst 法术（CombatAbilities.hx:1550-1562 条目 / 2792-2807 施法）。
    /// 状态效果见 Creature.hx:4403（受伤按满额）与 4794（最大生命按 2^层数 缩减）。
    /// </summary>
    public static class SoulburstHandlers
    {
        public static readonly Identifier Soulburst = new("coc", "soulburst");

        [Register("coc:spell_soulburst")]
        public static void Cast(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.WriteAction("Combat.Spell.Soulburst.Text".Localized());

            context.Append("Combat.Spell.Soulburst.Burst".Localized());

            // 原版：已有则层数 +1，否则创建（v1 = 1）
            var existing = player.StatusEffectByType(Soulburst);

            if (existing != null) existing.Values[0] += 1f;
            else player.CreateStatusEffect(Soulburst, 1, 0, 0, 0, 0);

            // 原版：先夹到最大生命，再自伤 1 点
            if (player.MaxHealth < player.Health) player.Health = player.MaxHealth;

            player.TakeDamage(1);
        }
    }
}