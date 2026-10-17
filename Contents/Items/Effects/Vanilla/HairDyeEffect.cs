using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Items;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/HairDye.hx:25 useItem() 入口：
    /// 记住染料颜色后打开部位选择界面（原版是一次选一个部位染，不是全身一起染）。
    /// </summary>
    public class HairDyeEffect(string color) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            HairDyeScene.DyeColor = color;

            HairDyeScene.SourceItem = context.Item;

            Scene.Go<HairDyeScene>();
        }
    }
}