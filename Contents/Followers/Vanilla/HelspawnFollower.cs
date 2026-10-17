using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<HelspawnFollower>("coc:helspawn_follower")]
    public class HelspawnFollower : FollowerDefinition
    {
        public HelspawnFollower() : base(
                FollowerIds.HelspawnFollower,

                FollowerIds.Roles.Follower,

                "Helspawn",

                () => CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) == 3 && CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) == 0,

                null,

                "Your daughter is staying at camp.",

                60)
        {
        }
    }
}