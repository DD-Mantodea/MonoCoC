using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<ArianLover>("coc:arian")]
    public class ArianLover : FollowerDefinition
    {
        public ArianLover() : base(
                FollowerIds.ArianLover,

                FollowerIds.Roles.Lover,

                "Arian",

                () => CoC.Instance.Flags.Get(GameFlagIds.ArianFollower) > 0,

                null,

                "Arian has moved into your camp.",

                10)
        {
        }
    }
}