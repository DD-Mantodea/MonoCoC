using MonoCoC.Extensions;
using System;
using System.Linq;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/Lactaid.hx:14-37 —— 促乳剂。
    /// 提升胸围（平均 &lt; 8 时）并提高各排乳腺的泌乳倍率。
    /// </summary>
    public class LactaidEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            var player = context.Player;

            player.SlimeFeed();

            context.Write("Items.Lactaid.Use".Localized());

            var averageSize = player.BreastRows.Count == 0

                ? 0f

                : player.BreastRows.Average(row => row.BreastRating);

            if (averageSize < 8)
            {
                context.Write("[pg]");

                if (player.BreastRows.Count == 1)
                    player.GrowTits(1 + context.Random.Next(5), 1, true, 1);
                else
                    player.GrowTits(1 + context.Random.Next(2), player.BreastRows.Count, true, 1);
            }

            if (player.BiggestLactation() < 1)
            {
                // 原版 :28 原本不泌乳 → 每排 +2
                context.Write("Items.Lactaid.Start".Localized());

                foreach (var row in player.BreastRows)
                    row.Lactation += 2f;
            }
            else
            {
                // 原版 :33 已泌乳 → 每排 +1 + rand(10)/10
                context.Write("Items.Lactaid.Boost".Localized());

                foreach (var row in player.BreastRows)
                    row.Lactation += 1f + context.Random.Next(10) / 10f;
            }

            player.Lust = Math.Max(0f, player.Lust + 10);

            if (context.Random.Next(3) == 0)
                // 原版 Character.hx:109 modFem(goal=95, strength=1)：向 95 逐步靠近 1 点。
                // C# 的 ModFem(amount, max) 语义不同（加值并夹到 max），这里按原版语义实现。
                player.Femininity = Math.Min(95f, player.Femininity + 1f);
        }
    }
}