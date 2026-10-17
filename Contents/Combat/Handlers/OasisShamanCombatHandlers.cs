using MonoCoC.Attributes;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Combat.Handlers
{
    // 原版 classes/scenes/areas/desert/OasisShaman.hx 的两个挑逗招式
    public static class OasisShamanCombatHandlers
    {
        [Register("coc:oasis_shaman_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            context.Append("Scenes.Oasisshaman.ETease.SeductiveOffering".Localized());

            var lust = 10 + player.Libido / 20f + context.Random.Next(Math.Max(1, (int)(player.Corruption / 20)));

            player.TakeLustDamage(lust, true);
        }

        [Register("coc:oasis_shaman_arouse")]
        public static void Arouse(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var enemy = context.Enemy;

            context.Append("Scenes.Oasisshaman.EArouse.ShimmeryLightGesture".Localized());

            var lust = enemy.Intelligence / 10f + player.Libido / 10f + player.Sensitivity / 5f;

            player.TakeLustDamage(lust, true);
        }
    }
}