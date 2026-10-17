using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/UrtaCum.hx:9-21 —— 喝下 Urta 的精液。
    /// 敏感 +1、性欲 +5 且随腐化提高、治疗 25% 最大生命、恢复 25 饱食度、嘴唇高潮。
    /// </summary>
    public class UrtaCumEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            context.Write("Items.UrtaCum.Use".Localized());

            player.Sensitivity = Math.Max(0f, player.Sensitivity + 1f);

            player.Lust = Math.Max(0f, player.Lust + 5f + player.Corruption / 5f);

            player.HealthChange(player.MaxHealth * 0.25f);

            player.SlimeFeed();

            player.RefillHunger(25);

            player.Orgasm("Lips");
        }
    }
}