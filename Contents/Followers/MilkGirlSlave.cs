using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Followers
{
    [Register<MilkGirlSlave>("coc:milk_waifu")]
    public class MilkGirlSlave : FollowerDefinition
    {
        public MilkGirlSlave() : base(
            new MonoCoC.Registry.Identifier("coc", "milk_waifu"),

            FollowerIds.Roles.Slave,

            "Milk Girl",

            () => SceneInstance<MilkWaifuScene>.Get().MilkSlaveAtCamp(),

            () => SceneInstance<MilkWaifuScene>.Get().MilkyMenu(),

            "Your well-endowed, dark-skinned milk-girl.",

            20)
        {
        }
    }
}