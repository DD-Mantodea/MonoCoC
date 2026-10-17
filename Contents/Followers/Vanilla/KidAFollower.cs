using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<KidAFollower>("coc:kid_a")]
    public class KidAFollower : FollowerDefinition
    {
        public KidAFollower() : base(
                FollowerIds.KidAFollower,

                FollowerIds.Roles.Follower,

                "Kid A",

                () => CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) > 0,

                null,

                "The child from the lake visits your camp.",

                80,

                () => CoC.Instance.Flags.Get(GameFlagIds.AnemoneWatch) > 0 && CoC.Instance.Flags.Get(GameFlagIds.AnemoneKid) > 0)
        {
        }
    }
}