using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla.Items
{
    /// <summary>
    /// 原版 items/consumables/BodyLotion.hx:23-171 —— 身体乳的部位选择界面。
    /// 身体 / 下体；按皮肤类型（普通/毛/鳞/黏液/龙鳞）分别描述，同种效果再涂只回血。
    /// </summary>
    public class BodyLotionScene : Scene
    {
        public static string LotionAdj = "smooth";

        public static Item? SourceItem;

        public override void Entry()
        {
            var player = CoC.Instance.Player;

            // 原版 :30 没有特殊下体 → 直接涂身体
            if (!player.HasUnderBody())
            {
                LotionSkin();

                return;
            }

            ClearOutput();

            ClearButtons();

            OutputText("Scenes.Items.BodyLotion.UnderbodyDifferent".Localized(LotionAdj));

            AddButton(0, "Scenes.Items.BodyLotion.Body".Localized(), LotionSkin);

            AddButton(1, "Scenes.Items.BodyLotion.Underbody".Localized(), LotionUnderBodySkin);

            AddButton(4, "Common.Button.NeverMind".Localized(), Cancel);
        }

        // 原版 :20 liquidDesc()
        private static string LiquidDesc() => LotionAdj switch
        {
            "smooth" => Pick("Scenes.Items.BodyLotion.Liquid.Smooth1", "Scenes.Items.BodyLotion.Liquid.Smooth2"),

            "rough" => Pick("Scenes.Items.BodyLotion.Liquid.Rough1", "Scenes.Items.BodyLotion.Liquid.Rough2"),

            "sexy" => Pick("Scenes.Items.BodyLotion.Liquid.Sexy1", "Scenes.Items.BodyLotion.Liquid.Sexy2", "Scenes.Items.BodyLotion.Liquid.Sexy3"),

            "clear" => Pick("Scenes.Items.BodyLotion.Liquid.Clear1", "Scenes.Items.BodyLotion.Liquid.Clear2"),

            _ => "Scenes.Items.BodyLotion.Liquid.Default".Localized(),
        };

        private static string Pick(params string[] keys) => keys[Random.Shared.Next(keys.Length)].Localized();

        // 原版 :197-260 各 ctx 的后缀（同一个 adj 四选一）
        private static string BodySuffix(string kind)
        {
            var adj = LotionAdj switch
            {
                "smooth" => "Smooth",

                "rough" => "Rough",

                "sexy" => "Sexy",

                "clear" => "Clear",

                _ => "Failsafe",
            };

            return $"Scenes.Items.BodyLotion.{kind}.{adj}".Localized();
        }

        // 原版 :62 lotionSkin()
        private void LotionSkin()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            var skin = player.Body.Get<SkinPart>();

            var clothed = player.ClothedOrNaked(
                "take a second to disrobe before uncorking the flask of lotion and rubbing",

                "uncork the flask of lotion and rub");

            if (skin.Adj == LotionAdj)
            {
                OutputText("Scenes.Items.BodyLotion.SameBody".Localized(clothed, LiquidDesc()));

                player.HealthChange(10);
            }
            else
            {
                // 原版 :67 黏液/龙鳞不改 adj，只是描述不同
                if (skin.Id.Path != "goo" && skin.Id.Path != "dragon_scales")
                    skin.Adj = LotionAdj != "clear" ? LotionAdj : "";

                switch (skin.Id.Path)
                {
                    case "plain":
                        OutputText("Scenes.Items.BodyLotion.PlainBody".Localized(clothed, LiquidDesc()));

                        OutputText(BodySuffix("Plain"));

                        break;

                    case "fur":
                        OutputText("Scenes.Items.BodyLotion.FurBody".Localized(
                            player.ClothedOrNaked("Once you've disrobed you take the lotion and", "You take the lotion and")));

                        OutputText(BodySuffix("Fur"));

                        break;

                    case "lizard_scales":
                    case "fish_scales":
                        OutputText("Scenes.Items.BodyLotion.ScalesBody".Localized(clothed, LiquidDesc()));

                        OutputText(BodySuffix("Scales"));

                        break;

                    case "goo":
                        OutputText("Scenes.Items.BodyLotion.GooBody".Localized(LiquidDesc()));

                        break;

                    case "dragon_scales":
                        OutputText("Scenes.Items.BodyLotion.DragonBody".Localized(LiquidDesc()));

                        break;

                    default:
                        OutputText("Scenes.Items.BodyLotion.DefaultBody".Localized(clothed, LotionAdj));

                        break;
                }
            }

            FinishUse();
        }

        // 原版 :121 lotionUnderBodySkin()
        private void LotionUnderBodySkin()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            var underBody = player.Body.Get<UnderBodyPart>();

            var clothed = player.ClothedOrNaked(
                "take a second to disrobe before uncorking the flask of lotion and rubbing",

                "uncork the flask of lotion and rub");

            if (underBody.Skin.Adj == LotionAdj)
            {
                OutputText("Scenes.Items.BodyLotion.SameUnderBody".Localized(clothed, LiquidDesc()));

                player.HealthChange(10);
            }
            else
            {
                if (underBody.Skin.Id.Path != "goo")
                    underBody.Skin.Adj = LotionAdj != "clear" ? LotionAdj : "";

                switch (underBody.Skin.Id.Path)
                {
                    case "plain":
                        OutputText("Scenes.Items.BodyLotion.PlainUnderBody".Localized(clothed, LiquidDesc()));

                        OutputText(BodySuffix("PlainUnder"));

                        break;

                    case "fur":
                        OutputText("Scenes.Items.BodyLotion.FurUnderBody".Localized(
                            player.ClothedOrNaked("Once you've disrobed you take the lotion and", "You take the lotion and")));

                        OutputText(BodySuffix("FurUnder"));

                        break;

                    case "lizard_scales":
                    case "dragon_scales":
                    case "fish_scales":
                        OutputText("Scenes.Items.BodyLotion.ScalesUnderBody".Localized(clothed, LiquidDesc()));

                        OutputText(BodySuffix("ScalesUnder"));

                        break;

                    case "goo":
                        OutputText("Scenes.Items.BodyLotion.GooBody".Localized(LiquidDesc()));

                        break;

                    default:
                        OutputText("Scenes.Items.BodyLotion.DefaultUnderBody".Localized(clothed, LotionAdj));

                        break;
                }
            }

            FinishUse();
        }

        // 原版 :262 lotionCancel()：把乳液退回背包
        private void Cancel()
        {
            ClearOutput();

            OutputText("Scenes.Items.BodyLotion.Cancel".Localized());

            if (SourceItem != null)
                CoC.Instance.Player.Inventory.Add(SourceItem, 1);

            FinishUse();
        }

        // 原版 inventory.itemGoNext()：物品已消耗，回背包
        private static void FinishUse() => Go<InventoryScene>();
    }
}