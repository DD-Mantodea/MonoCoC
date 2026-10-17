using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;

namespace MonoCoC.Contents.Followers
{
    [Register<RathazulFollower>("coc:rathazul_follower")]
    public class RathazulFollower : FollowerDefinition
    {
        public RathazulFollower() : base(
            new MonoCoC.Registry.Identifier("coc", "rathazul_follower"),

            FollowerIds.Roles.Follower,

            "Rathazul",

            () => SceneInstance<RathazulScene>.Get().FollowerRathazul(),

            () => SceneInstance<RathazulScene>.Get().ReturnToRathazulMenu(),

            "The alchemist Rathazul.",

            50)
        {
        }
    }
}