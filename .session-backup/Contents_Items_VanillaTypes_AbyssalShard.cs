using MonoCoC.Attributes;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;
using MonoCoC.ModLoader;

namespace MonoCoC.Contents.Items
{
    // 原版 classes/items/useables/AbyssalShard.hx：只能从物品栏使用的黑晶体。
    // 在死灵法师密室（Manor 第 0 层 loc 30）、晚上 19 点后使用会打开虚空（Manor.useCrystal）。
    [Register("coc:abyssal_shard")]
    public class AbyssalShard : Useable
    {
        public AbyssalShard()
        {
            UseAction = UseCrystal;
        }

        private static void UseCrystal(Item item)
        {
            var scene = SceneInstance<ManorScene>.Get();

            if (scene.UseCrystal())
            {
                // 原版 AbyssalShard.hx:11-12：doNext(inventory.callNext)，而 useCrystal 里 callNext = runFunc
                Scene.DoNext(scene.RunFunc);

                return;
            }

            // 原版 AbyssalShard.hx:14 inventory.returnItemToInventory(this)：没进虚空就退回物品栏
            Scene.Player.Inventory.Add(item);
        }
    }
}
