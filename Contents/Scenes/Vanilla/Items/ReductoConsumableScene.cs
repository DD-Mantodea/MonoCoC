using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items.Effects;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Items
{
    /// <summary>
    /// 原版 items/consumables/Reducto.hx:27-169 —— Reducto 药膏的部位选择界面。
    /// 每个部位都有原版的禁用条件（尺寸下限）与隐藏条件（没有该部位）。
    /// </summary>
    public class ReductoScene : Scene
    {
        private const float MinBalls = 0.5f;

        private const float MinBreast = 0f;

        private const float MinButt = 0f;

        private const float MinClit = 0.1f;

        private const float MinCock = 4f;

        private const float MinHips = 0f;

        private const float MinNipples = 0.1f;

        private const float MinHorns = 2f;

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Items.Reducto.Prompt".Localized());

            var player = CoC.Instance.Player;

            if (player.HasBalls())
                Button(0, "Scenes.Items.Reducto.BallsButton".Localized(), Balls, player.BallSize <= MinBalls);

            if (player.HasBreasts())
                Button(1, "Scenes.Items.Reducto.BreastsButton".Localized(), Breasts, player.BiggestBreastSize() <= MinBreast);

            Button(2, "Scenes.Items.Reducto.ButtButton".Localized(), Butt, player.Body.Get<ButtPart>().Rating <= MinButt);

            if (player.HasVagina())
                Button(3, "Scenes.Items.Reducto.ClitButton".Localized(), Clit, player.GetClitLength() <= MinClit);

            if (player.HasCock())
                Button(4, "Scenes.Items.Reducto.CockButton".Localized(), Cock, player.BiggestCockArea() <= MinCock);

            Button(5, "Scenes.Items.Reducto.HipsButton".Localized(), Hips, player.Body.Get<HipsPart>().Rating <= MinHips);

            Button(6, "Scenes.Items.Reducto.NipplesButton".Localized(), Nipples, player.NippleLength <= MinNipples);

            if (player.Body.Get<HornsPart>().Value > 0)
                Button(7, "Scenes.Items.Reducto.HornsButton".Localized(), Horns, player.Body.Get<HornsPart>().Value <= MinHorns);

            AddButton(14, "Common.Button.NeverMind".Localized(), Cancel);
        }

        /// <summary>尺寸不足时按原版禁用（不可点），否则挂上回调。</summary>
        private static void Button(int slot, string text, Action action, bool disabled)
        {
            if (disabled)
                AddButtonDisabled(slot, text, "Scenes.Items.Reducto.NothingMore".Localized());
            else
                AddButton(slot, text, action);
        }

        /// <summary>原版 Reducto.hx:43 reductoBalls()。</summary>
        private void Balls()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            OutputText("Scenes.Items.Reducto.BallsIntro".Localized());

            player.BallSize *= 2f / 3f;

            if (player.BallSize < MinBalls)
                player.BallSize = MinBalls;

            OutputText("Scenes.Items.Reducto.BallsDone".Localized());

            ApplyStats(lib: -2, lust: -10);

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:55 reductoBreasts()。</summary>
        private void Breasts()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            OutputText("Scenes.Items.Reducto.BreastsIntro".Localized());

            player.ShrinkTits(true);

            if (Random.Shared.Next(2) == 0 && player.BiggestBreastSize() >= MinBreast)
            {
                OutputText("Scenes.Items.Reducto.BreastsAgain".Localized());

                player.ShrinkTits(true);
            }

            OutputText("Scenes.Items.Reducto.BreastsDone".Localized());

            ApplyStats(sens: -2, lust: -5);

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:68 reductoButt()。</summary>
        private void Butt()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            var butt = player.Body.Get<ButtPart>();

            OutputText("Scenes.Items.Reducto.ButtIntro".Localized());

            if (butt.Rating >= 15)
            {
                butt.Rating -= 3 + (int)(butt.Rating / 3);

                OutputText("Scenes.Items.Reducto.ButtHuge".Localized());
            }
            else if (butt.Rating >= 10)
            {
                butt.Rating -= 3;

                OutputText("Scenes.Items.Reducto.ButtBig".Localized());
            }
            else
            {
                butt.Rating -= Random.Shared.Next(1, 4);

                if (butt.Rating < MinButt)
                    butt.Rating = MinButt;

                OutputText("Scenes.Items.Reducto.ButtSmall".Localized());
            }

            ApplyStats(lib: -2, lust: -10);

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:88 reductoClit()。</summary>
        private void Clit()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            OutputText("Scenes.Items.Reducto.ClitIntro".Localized());

            player.SetClitLength(player.GetClitLength() / 1.7f);

            player.SetClitLength((float)Math.Floor(player.GetClitLength() * 100) / 100);

            OutputText("Scenes.Items.Reducto.ClitDone".Localized());

            ApplyStats(sens: -2, lust: -10);

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:99 reductoCock()。</summary>
        private void Cock()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            if (player.Cocks[0].Id == BodyPartTypes.Bee)
            {
                // 原版 :102 蜜蜂型 → 人类
                OutputText("Scenes.Items.Reducto.CockBee".Localized());

                player.Cocks[0].Id = BodyPartTypes.Human;
            }
            else
            {
                OutputText("Scenes.Items.Reducto.CockIntro".Localized());

                var place = player.Cocks[0].SheathOn

                    ? "Scenes.Items.Reducto.Sheath".Localized()

                    : "Scenes.Items.Reducto.Crotch".Localized();

                if (player.Cocks.Count == 1)
                {
                    OutputText("Scenes.Items.Reducto.CockSingle".Localized(place));

                    player.Cocks[0].Length *= 2f / 3f;

                    player.Cocks[0].Thickness *= 2f / 3f;
                }
                else
                    OutputText("Scenes.Items.Reducto.CockMulti".Localized(place));

                foreach (var cock in player.Cocks)
                {
                    cock.Length *= 2f / 3f;

                    cock.Thickness *= 2f / 3f;
                }
            }

            ApplyStats(sens: -2, lust: -10);

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:123 reductoHips()。</summary>
        private void Hips()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            var hips = player.Body.Get<HipsPart>();

            OutputText("Scenes.Items.Reducto.HipsIntro".Localized());

            if (hips.Rating >= 15)
            {
                hips.Rating -= 3 + (int)(hips.Rating / 3);

                OutputText("Scenes.Items.Reducto.HipsHuge".Localized());
            }
            else if (hips.Rating >= 10)
            {
                hips.Rating -= 3;

                OutputText("Scenes.Items.Reducto.HipsBig".Localized());
            }
            else
            {
                hips.Rating -= Random.Shared.Next(1, 4);

                if (hips.Rating < MinHips)
                    hips.Rating = MinHips;

                OutputText("Scenes.Items.Reducto.HipsSmall".Localized());
            }

            ApplyStats(lib: -2, lust: -10);

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:143 reductoNipples()。</summary>
        private void Nipples()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            OutputText("Scenes.Items.Reducto.NipplesIntro".Localized());

            if (player.NippleLength / 2 < MinNipples)
            {
                OutputText("Scenes.Items.Reducto.NipplesTiny".Localized());

                player.NippleLength = MinNipples;
            }
            else
            {
                OutputText("Scenes.Items.Reducto.NipplesHalf".Localized());

                player.NippleLength /= 2;
            }

            ApplyStats(sens: -5, lust: -5);

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:158 shrinkHorns()。</summary>
        private void Horns()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            OutputText("Scenes.Items.Reducto.HornsIntro".Localized());

            OutputText("Scenes.Items.Reducto.HornsDone".Localized());

            player.Body.Get<HornsPart>().Value -= 1;

            FinishUse();
        }

        /// <summary>原版 Reducto.hx:165 reductoCancel()。</summary>
        private void Cancel()
        {
            ClearOutput();

            OutputText("Scenes.Items.Reducto.Cancel".Localized());

            FinishUse();
        }

        /// <summary>原版 dynStats(Lib/Lust/Sens) 的等价实现。</summary>
        private static void ApplyStats(int lib = 0, int lust = 0, int sens = 0)
        {
            var player = CoC.Instance.Player;

            if (lib != 0)
                player.Libido = Math.Max(0f, player.Libido + lib);

            if (lust != 0)
                player.Lust = Math.Max(0f, player.Lust + lust);

            if (sens != 0)
                player.Sensitivity = Math.Max(0f, player.Sensitivity + sens);
        }

        /// <summary>原版 inventory.itemGoNext()：物品在使用时已消耗，这里回到背包。</summary>
        private static void FinishUse() => Go<InventoryScene>();
    }

    /// <summary>
    /// 原版 Reducto.hx:27 useItem() 的入口：打开部位选择界面。
    /// 通过 ConsumableEffectRegistry 的「物品直连效果」注册，避免改动物品数据文件。
    /// </summary>
    public class ReductoEffect : ConsumableEffect
    {
        public override void Apply(ItemUseContext context) => Scene.Go<ReductoScene>();
    }
}