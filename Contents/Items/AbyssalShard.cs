using MonoCoC.Attributes;
using MonoCoC.Contents.Dungeons;
using MonoCoC.Contents.Scenes;

namespace MonoCoC.Contents.Items
{

    [Register<AbyssalShard>("coc:abyssal_shard")]
    public class AbyssalShard : Useable
    {
        public AbyssalShard()
        {
            UseAction = UseCrystal;
        }

        public static void UseCrystal(Item item)
        {
            var scene = SceneInstance<ManorScene>.Get();

            if (scene.UseCrystal())
            {

                Scene.DoNext(scene.RunFunc);

                return;
            }

            Scene.Player.Inventory.Add(item);
        }
    }
}