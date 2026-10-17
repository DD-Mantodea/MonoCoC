using MonoCoC.Contents.Items.Effects;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Items
{
    /// <summary>
    /// 原版 items/consumables/GroPlus.hx:19-129 —— Gro+ 注射剂的部位选择界面。
    /// 与原版一致：所有按钮都显示，仅在对应部位不存在时禁用。
    /// </summary>
    public class GroPlusScene : Scene
    {
        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Items.GroPlus.Prompt".Localized());

            var player = CoC.Instance.Player;

            Button(0, "Scenes.Items.GroPlus.BallsButton".Localized(), Balls, player.Balls <= 0);

            Button(1, "Scenes.Items.GroPlus.BreastsButton".Localized(), Breasts, player.BreastRows.Count <= 0);

            Button(2, "Scenes.Items.GroPlus.ClitButton".Localized(), Clit, player.Vaginas.Count <= 0);

            Button(3, "Scenes.Items.GroPlus.CockButton".Localized(), Cock, player.Cocks.Count <= 0);

            Button(4, "Scenes.Items.GroPlus.NipplesButton".Localized(), Nipples, player.TotalNipples() <= 0);

            AddButton(14, "Common.Button.NeverMind".Localized(), Cancel);
        }

        private static void Button(int slot, string text, Action action, bool disabled)
        {
            if (disabled)
                AddButtonDisabled(slot, text, "Scenes.Items.GroPlus.NothingHere".Localized());
            else
                AddButton(slot, text, action);
        }

        /// <summary>原版 GroPlus.hx:31 growPlusBalls()。</summary>
        private void Balls()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            player.SlimeFeed();

            OutputText("Scenes.Items.GroPlus.BallsIntro".Localized());

            if (Random.Shared.Next(4) == 0)
            {
                OutputText("Scenes.Items.GroPlus.BallsBig".Localized());

                player.BallSize += Random.Shared.Next(4) + 2;

                OutputText("Scenes.Items.GroPlus.BallsBigDone".Localized());
            }
            else
            {
                player.BallSize += Random.Shared.Next(2) + 1;

                OutputText("Scenes.Items.GroPlus.BallsDone".Localized());
            }

            if (player.BallSize > 10)
                OutputText("Scenes.Items.GroPlus.BallsTooBig".Localized());

            player.Lust = Math.Max(0f, player.Lust + 10);

            FinishUse();
        }

        /// <summary>原版 GroPlus.hx:51 growPlusBreasts()。</summary>
        private void Breasts()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            player.SlimeFeed();

            OutputText("Scenes.Items.GroPlus.BreastsIntro".Localized(Creatures.Appearance.AllBreastsDescript(player)));

            if (player.BreastRows.Count == 1)
                player.GrowTits(Random.Shared.Next(5) + 1, 1, true, 1);
            else
                player.GrowTits(Random.Shared.Next(2) + 1, player.BreastRows.Count, true, 1);

            player.Lust = Math.Max(0f, player.Lust + 10);

            FinishUse();
        }

        /// <summary>原版 GroPlus.hx:64 growPlusClit()。</summary>
        private void Clit()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            player.SlimeFeed();

            OutputText("Scenes.Items.GroPlus.ClitIntro".Localized());

            player.ChangeClitLength(1);

            OutputText("Scenes.Items.GroPlus.ClitDone".Localized());

            player.Sensitivity = Math.Max(0f, player.Sensitivity + 2);

            player.Lust = Math.Max(0f, player.Lust + 10);

            FinishUse();
        }

        /// <summary>原版 GroPlus.hx:74 growPlusCock()。</summary>
        private void Cock()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            player.SlimeFeed();

            OutputText("Scenes.Items.GroPlus.CockIntro".Localized());

            if (player.Cocks.Count == 1)
            {
                var amount = player.IncreaseCock(0, 4);

                OutputText("Scenes.Items.GroPlus.CockSingle".Localized(
                    amount > 1

                        ? "Scenes.Items.GroPlus.MoreThanInch".Localized()

                        : "Scenes.Items.GroPlus.AboutInch".Localized()));
            }
            else
            {
                OutputText("Scenes.Items.GroPlus.CockMulti".Localized());

                for (var i = 0; i < player.Cocks.Count; i++)
                    player.IncreaseCock(i, 2);
            }

            OutputText(player.Cocks[0].SheathOn

                ? "Scenes.Items.GroPlus.Sheath".Localized()

                : "Scenes.Items.GroPlus.Crotch".Localized());

            player.Sensitivity = Math.Max(0f, player.Sensitivity + 2);

            player.Lust = Math.Max(0f, player.Lust + 10);

            FinishUse();
        }

        /// <summary>原版 GroPlus.hx:99 growPlusNipples()。</summary>
        private void Nipples()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            player.SlimeFeed();

            OutputText("Scenes.Items.GroPlus.NipplesIntro".Localized());

            OutputText("Scenes.Items.GroPlus.NipplesGrow".Localized());

            player.NippleLength += (Random.Shared.Next(2) + 3) / 10f;

            player.Lust = Math.Max(0f, player.Lust + 15);

            // 原版 :108 1/4 概率让乳头变得可用（长度 >= 2 的排）
            if (!player.HasFuckableNipples() && Random.Shared.Next(4) == 0)
            {
                var nowFuckable = false;

                foreach (var row in player.BreastRows)
                    if (!row.Fuckable && player.NippleLength >= 2)
                    {
                        row.Fuckable = true;

                        nowFuckable = true;
                    }

                if (nowFuckable)
                    OutputText("Scenes.Items.GroPlus.NipplesFuckable".Localized(Creatures.Appearance.AllBreastsDescript(player)));
            }

            FinishUse();
        }

        /// <summary>原版 GroPlus.hx:125 growPlusCancel()。</summary>
        private void Cancel()
        {
            ClearOutput();

            OutputText("Scenes.Items.GroPlus.Cancel".Localized());

            FinishUse();
        }

        private static void FinishUse() => Go<InventoryScene>();
    }

    /// <summary>原版 GroPlus.hx:19 useItem()：打开部位选择界面。</summary>
    public class GroPlusEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context) => Scene.Go<GroPlusScene>();
    }
}