using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<CeraphSlave>("coc:ceraph")]
    public class CeraphSlave : FollowerDefinition
    {
        public CeraphSlave() : base(
                FollowerIds.CeraphSlave,

                FollowerIds.Roles.Slave,

                "Ceraph",

                () => Scenes.SceneInstance<Scenes.Vanilla.Npcs.CeraphFollowerScene>.Get().CeraphIsFollower(),

                () => Scenes.SceneInstance<Scenes.Vanilla.Npcs.CeraphFollowerScene>.Get().CeraphFollowerEncounter(),

                "Ceraph serves at camp.",

                50)
        {
        }
    }
}