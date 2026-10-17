using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Items;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/SkinOil.hx:24 useItem() 入口：
    /// 记住油的颜色后打开部位选择界面（原版是选部位涂，不是一次性全身涂）。
    /// </summary>
    public class SkinOilEffect(string color) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            SkinOilScene.OilColor = color;

            SkinOilScene.SourceItem = context.Item;

            Scene.Go<SkinOilScene>();
        }
    }
}