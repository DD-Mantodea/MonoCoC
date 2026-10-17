using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<HolliFollower>("coc:holli")]
    public class HolliFollower : FollowerDefinition
    {
        public HolliFollower() : base(
                FollowerIds.HolliFollower,

                FollowerIds.Roles.Follower,

                "Holli",

                () => CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerKilled) == 0,

                null,

                "Holli lives in the tree at the edge of camp.",

                70,

                () => CoC.Instance.Flags.Get(GameFlagIds.HolliDefenseOn) > 0 && CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerLevel) >= 4 && CoC.Instance.Flags.Get(GameFlagIds.FuckFlowerKilled) == 0)
        {
        }
    }
}