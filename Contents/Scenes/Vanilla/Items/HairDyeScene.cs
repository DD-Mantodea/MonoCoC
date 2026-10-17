using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Items
{
    /// <summary>
    /// 原版 items/consumables/HairDye.hx:25-205 —— 染发剂的部位选择界面。
    /// 原版每个部位都单独判定（能不能染、是不是已经这个颜色），不能染的按钮灰掉并给出原因；
    /// 染头发有橡胶/乳胶材质免疫，其余部位染完统一走 finalize（降温 + 消耗物品回到背包）。
    /// </summary>
    public class HairDyeScene : Scene
    {
        public static string DyeColor = "black";

        public static Item? SourceItem;

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            var player = CoC.Instance.Player;

            var hair = player.Body.Get<HairPart>();

            var skin = player.Body.Get<SkinPart>();

            var underBody = player.Body.Get<UnderBodyPart>();

            var wings = player.Body.Get<WingsPart>();

            var neck = player.Body.Get<NeckPart>();

            var rearBody = player.Body.Get<RearBodyPart>();

            // 头发
            if (hair.Length > 0)
            {
                OutputText("Scenes.Items.HairDye.HairHas".Localized());

                if (hair.Color != DyeColor)
                    AddButton(0, "Scenes.Items.HairDye.Hair".Localized(), DyeHair);
                else
                    AddButtonDisabled(0, "Scenes.Items.HairDye.Hair".Localized(), "Scenes.Items.HairDye.HairAlready".Localized(DyeColor));
            }
            else
            {
                OutputText("Scenes.Items.HairDye.HairNone".Localized());

                AddButtonDisabled(0, "Scenes.Items.HairDye.Hair".Localized(), "Scenes.Items.HairDye.HairBald".Localized());
            }

            // 毛发 / 羽毛
            if (player.HasFur())
            {
                OutputText("Scenes.Items.HairDye.FurHas".Localized());

                if (skin.FurColor != DyeColor)
                    AddButton(1, "Scenes.Items.HairDye.Fur".Localized(), DyeFur);
                else
                    AddButtonDisabled(1, "Scenes.Items.HairDye.Fur".Localized(), "Scenes.Items.HairDye.FurAlready".Localized(DyeColor));
            }
            else if (player.HasFeathers() || player.HasCockatriceSkin())
            {
                OutputText("Scenes.Items.HairDye.FeathersHas".Localized());

                if (skin.FurColor != DyeColor)
                    AddButton(1, "Scenes.Items.HairDye.Feathers".Localized(), DyeFeathers);
                else
                    AddButtonDisabled(1, "Scenes.Items.HairDye.Feathers".Localized(), "Scenes.Items.HairDye.FeathersAlready".Localized(DyeColor));
            }
            else
            {
                OutputText("Scenes.Items.HairDye.FurNone".Localized());

                AddButtonDisabled(1, "Scenes.Items.HairDye.Fur".Localized(), "Scenes.Items.HairDye.FurMissing".Localized());
            }

            // 下体毛 / 羽毛
            if (player.HasFurryUnderBody())
            {
                OutputText("Scenes.Items.HairDye.UnderFurHas".Localized());

                if (underBody.Skin.FurColor != DyeColor)
                    AddButton(2, "Scenes.Items.HairDye.UnderFur".Localized(), DyeUnderBodyFur);
                else
                    AddButtonDisabled(2, "Scenes.Items.HairDye.UnderFur".Localized(), "Scenes.Items.HairDye.UnderFurAlready".Localized(DyeColor));
            }
            else if (player.HasFeatheredUnderBody())
            {
                OutputText("Scenes.Items.HairDye.UnderFeathersHas".Localized());

                if (underBody.Skin.FurColor != DyeColor)
                    AddButton(2, "Scenes.Items.HairDye.UnderFeathers".Localized(), DyeUnderBodyFeathers);
                else
                    AddButtonDisabled(2, "Scenes.Items.HairDye.UnderFeathers".Localized(), "Scenes.Items.HairDye.UnderFeathersAlready".Localized(DyeColor));
            }
            else
            {
                OutputText("Scenes.Items.HairDye.UnderNone".Localized());

                AddButtonDisabled(2, "Scenes.Items.HairDye.UnderFur".Localized(), "Scenes.Items.HairDye.UnderMissing".Localized());
            }

            // 翅膀
            if (wings.CanDye())
            {
                OutputText("Scenes.Items.HairDye.WingsHas".Localized());

                if (!wings.HasDyeColor(DyeColor))
                    AddButton(3, "Scenes.Items.HairDye.Wings".Localized(), DyeWings);
                else
                    AddButtonDisabled(3, "Scenes.Items.HairDye.Wings".Localized(), "Scenes.Items.HairDye.WingsAlready".Localized(DyeColor));
            }
            else
            {
                OutputText("Scenes.Items.HairDye.WingsNone".Localized());

                AddButtonDisabled(3, "Scenes.Items.HairDye.Wings".Localized(), "Scenes.Items.HairDye.WingsCant".Localized());
            }

            // 脖子
            if (neck.CanDye())
            {
                OutputText("Scenes.Items.HairDye.NeckHas".Localized());

                if (!neck.HasDyeColor(DyeColor))
                    AddButton(5, "Scenes.Items.HairDye.Neck".Localized(), DyeNeck);
                else
                    AddButtonDisabled(5, "Scenes.Items.HairDye.Neck".Localized(), "Scenes.Items.HairDye.NeckAlready".Localized(DyeColor));
            }
            else
            {
                OutputText("Scenes.Items.HairDye.NeckNone".Localized());

                AddButtonDisabled(5, "Scenes.Items.HairDye.Neck".Localized(), "Scenes.Items.HairDye.NeckCant".Localized());
            }

            // 后体
            if (rearBody.CanDye())
            {
                OutputText("Scenes.Items.HairDye.RearHas".Localized());

                if (!rearBody.HasDyeColor(DyeColor))
                    AddButton(6, "Scenes.Items.HairDye.RearBody".Localized(), DyeRearBody);
                else
                    AddButtonDisabled(6, "Scenes.Items.HairDye.RearBody".Localized(), "Scenes.Items.HairDye.RearAlready".Localized(DyeColor));
            }
            else
            {
                OutputText("Scenes.Items.HairDye.RearNone".Localized());

                AddButtonDisabled(6, "Scenes.Items.HairDye.RearBody".Localized(), "Scenes.Items.HairDye.RearCant".Localized());
            }

            AddButton(4, "Common.Button.NeverMind".Localized(), Cancel);
        }

        // 原版 :118 dyeHair()
        private void DyeHair()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            var hair = player.Body.Get<HairPart>();

            if (hair.Length == 0)
                OutputText("Scenes.Items.HairDye.DyeBald".Localized());
            else if (hair.Color.Contains("rubbery") || hair.Color.Contains("latex-textured"))
                OutputText("Scenes.Items.HairDye.DyeRubbery".Localized());
            else
            {
                OutputText("Scenes.Items.HairDye.RubHair".Localized());

                hair.Color = DyeColor;

                OutputText("Scenes.Items.HairDye.NowHair".Localized());

                if (player.Lust > 50)
                {
                    OutputText("Scenes.Items.HairDye.Calm".Localized());

                    player.Lust = Math.Max(0, player.Lust - 15);
                }
            }

            FinishUse();
        }

        // 原版 :136 dyeFur()
        private void DyeFur()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.RubFur".Localized());

            CoC.Instance.Player.Body.Get<SkinPart>().FurColor = DyeColor;

            OutputText("Scenes.Items.HairDye.NowFur".Localized());

            FinalizeDye();
        }

        // 原版 :144 dyeUnderBodyFur()
        private void DyeUnderBodyFur()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.RubUnderFur".Localized());

            CoC.Instance.Player.Body.Get<UnderBodyPart>().Skin.FurColor = DyeColor;

            OutputText("Scenes.Items.HairDye.NowUnderFur".Localized());

            FinalizeDye();
        }

        // 原版 :152 dyeFeathers()
        private void DyeFeathers()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.RubFeathers".Localized());

            CoC.Instance.Player.Body.Get<SkinPart>().FurColor = DyeColor;

            OutputText("Scenes.Items.HairDye.NowFeathers".Localized());

            FinalizeDye();
        }

        // 原版 :160 dyeUnderBodyFeathers()
        private void DyeUnderBodyFeathers()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.RubUnderFeathers".Localized());

            CoC.Instance.Player.Body.Get<UnderBodyPart>().Skin.FurColor = DyeColor;

            OutputText("Scenes.Items.HairDye.NowUnderFeathers".Localized());

            FinalizeDye();
        }

        // 原版 :168 dyeWings()
        private void DyeWings()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.RubWings".Localized());

            CoC.Instance.Player.Body.Get<WingsPart>().ApplyDye(DyeColor);

            OutputText("Scenes.Items.HairDye.NowWings".Localized());

            FinalizeDye();
        }

        // 原版 :176 dyeNeck()
        private void DyeNeck()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.RubNeck".Localized());

            CoC.Instance.Player.Body.Get<NeckPart>().ApplyDye(DyeColor);

            OutputText("Scenes.Items.HairDye.NowNeck".Localized());

            FinalizeDye();
        }

        // 原版 :184 dyeRearBody()
        private void DyeRearBody()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.RubRear".Localized());

            CoC.Instance.Player.Body.Get<RearBodyPart>().ApplyDye(DyeColor);

            OutputText("Scenes.Items.HairDye.NowRear".Localized());

            FinalizeDye();
        }

        // 原版 :192 dyeCancel()：把染料退回背包
        private void Cancel()
        {
            ClearOutput();

            OutputText("Scenes.Items.HairDye.Cancel".Localized());

            if (SourceItem != null)
                CoC.Instance.Player.Inventory.Add(SourceItem, 1);

            FinishUse();
        }

        // 原版 :198 finalize()
        private void FinalizeDye()
        {
            var player = CoC.Instance.Player;

            if (player.Lust > 50)
            {
                OutputText("Scenes.Items.HairDye.Calm".Localized());

                player.Lust = Math.Max(0, player.Lust - 15);
            }

            FinishUse();
        }

        // 原版 inventory.itemGoNext()：物品已消耗，回背包
        private static void FinishUse() => Go<InventoryScene>();
    }
}