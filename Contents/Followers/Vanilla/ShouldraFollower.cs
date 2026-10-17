using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<ShouldraFollower>("coc:shouldra")]
    public class ShouldraFollower : FollowerDefinition
    {
        public ShouldraFollower() : base(
                FollowerIds.ShouldraFollower,

                FollowerIds.Roles.Follower,

                "Shouldra",

                () => CoC.Instance.Flags.Get(GameFlagIds.ShouldraFollowerState) == 1,

                null,

                "The ghostly fox lives inside your body.",

                40)
        {
        }
    }
}