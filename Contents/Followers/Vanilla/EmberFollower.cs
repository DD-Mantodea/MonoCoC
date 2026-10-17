using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<EmberFollower>("coc:ember")]
    public class EmberFollower : FollowerDefinition
    {
        public EmberFollower() : base(
                FollowerIds.EmberFollower,

                FollowerIds.Roles.Follower,

                "Ember",

                () => CoC.Instance.Flags.Get(GameFlagIds.EmberHatched) > 0,

                null,

                "The dragon-morph you hatched is resting at camp.",

                10)
        {
        }
    }
}