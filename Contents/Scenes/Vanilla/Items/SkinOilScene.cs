using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Items;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Scenes.Vanilla.Items
{
    /// <summary>
    /// 原版 items/consumables/SkinOil.hx:24-154 —— 护肤油的部位选择界面。
    /// 身体 / 下体 / 翅膀主色 / 翅膀第二色；只有龙翼能上油，皮肤类型决定描述，
    /// 黏液皮肤不会被染色但要喂史莱姆，同色再涂只减疲劳。
    /// </summary>
    public class SkinOilScene : Scene
    {
        public static string OilColor = "clear";

        public static Item? SourceItem;

        public override void Entry()
        {
            var player = CoC.Instance.Player;

            var wings = player.Body.Get<WingsPart>();

            // 原版 :25 既没有特殊下体也没有可上油的翅膀 → 直接涂身体
            if (!player.HasUnderBody() && !wings.CanOil())
            {
                OilSkin();

                return;
            }

            ClearOutput();

            ClearButtons();

            if (player.HasUnderBody())
                OutputText("Scenes.Items.SkinOil.UnderbodyDifferent".Localized());

            OutputText("Scenes.Items.SkinOil.WhereToApply".Localized(OilColor));

            AddButton(0, "Scenes.Items.SkinOil.Body".Localized(), OilSkin);

            if (player.HasUnderBody())
                AddButton(1, "Scenes.Items.SkinOil.Underbody".Localized(), OilUnderBodySkin);
            else
                AddButtonDisabled(1, "Scenes.Items.SkinOil.Underbody".Localized(), "Scenes.Items.SkinOil.NoUnderbody".Localized());

            // 翅膀主色
            if (wings.Id == BodyPartTypes.None)
            {
                OutputText("Scenes.Items.SkinOil.NoWings".Localized());

                AddButtonDisabled(2, "Scenes.Items.SkinOil.Wings".Localized(), "Scenes.Items.SkinOil.NoWingsHint".Localized());
            }
            else if (wings.CanOil())
            {
                OutputText("Scenes.Items.SkinOil.WingsHave".Localized());

                if (!wings.HasOilColor(OilColor))
                {
                    var button = AddButton(2, "Scenes.Items.SkinOil.Wings".Localized(), OilWings);

                    button?.Hint = "Scenes.Items.SkinOil.OilWingsHint".Localized(
                            "BodyParts.Wings.ColorDesc.Membranes".Localized());
                }
                else
                    AddButtonDisabled(2, "Scenes.Items.SkinOil.Wings".Localized(),

                        "Scenes.Items.SkinOil.WingsAlready".Localized("BodyParts.Wings.ColorDesc.Membranes".Localized(), OilColor));
            }
            else
            {
                OutputText("Scenes.Items.SkinOil.WingsCantOil".Localized());

                AddButtonDisabled(2, "Scenes.Items.SkinOil.Wings".Localized(), "Scenes.Items.SkinOil.WingsCantOilHint".Localized());
            }

            // 翅膀第二色
            if (wings.Id == BodyPartTypes.None)
            {
                OutputText("Scenes.Items.SkinOil.NoWings".Localized());

                AddButtonDisabled(3, "Scenes.Items.SkinOil.Wings2".Localized(), "Scenes.Items.SkinOil.NoWingsHint".Localized());
            }
            else if (wings.CanOil2())
            {
                OutputText("Scenes.Items.SkinOil.Wings2Have".Localized());

                if (!wings.HasOil2Color(OilColor))
                {
                    var button = AddButton(3, "Scenes.Items.SkinOil.Wings2".Localized(), Oil2Wings);

                    button?.Hint = "Scenes.Items.SkinOil.OilWings2Hint".Localized(
                            "BodyParts.Wings.ColorDesc.Bones".Localized());
                }
                else
                    AddButtonDisabled(3, "Scenes.Items.SkinOil.Wings2".Localized(),

                        "Scenes.Items.SkinOil.WingsAlready".Localized("BodyParts.Wings.ColorDesc.Bones".Localized(), OilColor));
            }
            else
                AddButtonDisabled(3, "Scenes.Items.SkinOil.Wings2".Localized(), "Scenes.Items.SkinOil.Wings2NoSecondary".Localized());

            AddButton(4, "Common.Button.NeverMind".Localized(), Cancel);
        }

        // 原版 :53 oilSkin()
        private void OilSkin()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            var skin = player.Body.Get<SkinPart>();

            if (skin.Tone == OilColor)
            {
                OutputText("Scenes.Items.SkinOil.SameBody".Localized(
                    player.ClothedOrNaked(
                        "take a second to disrobe before uncorking the bottle of oil and rubbing",

                        "uncork the bottle of oil and rub")));

                player.ChangeFatigue(-10);
            }
            else
            {
                // 原版 :62 黏液皮肤不改变肤色，但仍然刷新爪子配色
                if (!player.HasGooSkin())
                {
                    skin.Tone = OilColor;

                    player.Body.Get<ArmsPart>().UpdateClaws(player.Body.Get<ArmsPart>().Claws.Id);
                }

                var clothed = player.ClothedOrNaked(
                    "take a second to disrobe before uncorking the bottle of oil and rubbing",

                    "uncork the bottle of oil and rub");

                switch (skin.Id.Path)
                {
                    case "plain":
                        OutputText("Scenes.Items.SkinOil.PlainBody".Localized(clothed, OilColor));

                        break;

                    case "fur":
                        OutputText("Scenes.Items.SkinOil.FurBody".Localized(
                            player.ClothedOrNaked("Once you've disrobed you take the oil and", "You take the oil and"), OilColor));

                        break;

                    case "lizard_scales":
                    case "dragon_scales":
                    case "fish_scales":
                        OutputText("Scenes.Items.SkinOil.ScalesBody".Localized(clothed, OilColor));

                        break;

                    case "goo":
                        OutputText("Scenes.Items.SkinOil.GooBody".Localized());

                        player.SlimeFeed();

                        break;

                    default:
                        OutputText("Scenes.Items.SkinOil.PlainBody".Localized(clothed, OilColor));

                        break;
                }
            }

            FinishUse();
        }

        // 原版 :93 oilUnderBodySkin()
        private void OilUnderBodySkin()
        {
            ClearOutput();

            var player = CoC.Instance.Player;

            var underBody = player.Body.Get<UnderBodyPart>();

            if (underBody.Skin.Tone == OilColor)
            {
                OutputText("Scenes.Items.SkinOil.SameUnderBody".Localized(
                    player.ClothedOrNaked(
                        "take a second to disrobe before uncorking the bottle of oil and rubbing",

                        "uncork the bottle of oil and rub")));

                player.ChangeFatigue(-10);
            }
            else
            {
                if (!player.HasGooSkin())
                    underBody.Skin.Tone = OilColor;

                var clothed = player.ClothedOrNaked(
                    "take a second to disrobe before uncorking the bottle of oil and rubbing",

                    "uncork the bottle of oil and rub");

                switch (underBody.Skin.Id.Path)
                {
                    case "plain":
                        OutputText("Scenes.Items.SkinOil.PlainUnderBody".Localized(clothed, OilColor));

                        break;

                    case "fur":
                        OutputText("Scenes.Items.SkinOil.FurUnderBody".Localized(
                            player.ClothedOrNaked("Once you've disrobed you take the oil and", "You take the oil and"), OilColor));

                        break;

                    case "lizard_scales":
                    case "dragon_scales":
                    case "fish_scales":
                        OutputText("Scenes.Items.SkinOil.ScalesUnderBody".Localized(clothed, OilColor));

                        break;

                    case "goo":
                        OutputText("Scenes.Items.SkinOil.GooBody".Localized());

                        player.SlimeFeed();

                        break;

                    default:
                        OutputText("Scenes.Items.SkinOil.PlainUnderBody".Localized(clothed, OilColor));

                        break;
                }
            }

            FinishUse();
        }

        // 原版 :127 oilWings()
        private void OilWings()
        {
            ClearOutput();

            OutputText("Scenes.Items.SkinOil.OilWings".Localized());

            CoC.Instance.Player.Body.Get<WingsPart>().ApplyOil(OilColor);

            OutputText("Scenes.Items.SkinOil.WingsOiled".Localized());

            FinishUse();
        }

        // 原版 :134 oil2Wings()
        private void Oil2Wings()
        {
            ClearOutput();

            OutputText("Scenes.Items.SkinOil.OilWings2".Localized());

            CoC.Instance.Player.Body.Get<WingsPart>().ApplyOil2(OilColor);

            OutputText("Scenes.Items.SkinOil.Wings2Oiled".Localized());

            FinishUse();
        }

        // 原版 :150 oilCancel()：把油退回背包
        private void Cancel()
        {
            ClearOutput();

            OutputText("Scenes.Items.SkinOil.Cancel".Localized());

            if (SourceItem != null)
                CoC.Instance.Player.Inventory.Add(SourceItem, 1);

            FinishUse();
        }

        // 原版 inventory.itemGoNext()：物品已消耗，回背包
        private static void FinishUse() => Go<InventoryScene>();
    }
}