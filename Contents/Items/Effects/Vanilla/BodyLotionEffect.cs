using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Items;

namespace MonoCoC.Contents.Items.Effects.Vanilla
{
    /// <summary>
    /// 原版 items/consumables/BodyLotion.hx:23 useItem() 入口：
    /// 记住乳液类型后打开部位选择界面（原版是选身体/下体，不是一次抹全身）。
    /// </summary>
    public class BodyLotionEffect(string adj) : ConsumableEffect
    {
        public override void Apply(ItemUseContext context)
        {
            BodyLotionScene.LotionAdj = adj;

            BodyLotionScene.SourceItem = context.Item;

            Scene.Go<BodyLotionScene>();
        }
    }
}