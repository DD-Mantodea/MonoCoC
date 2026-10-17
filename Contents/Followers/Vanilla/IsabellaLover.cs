using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<IsabellaLover>("coc:isabella")]
    public class IsabellaLover : FollowerDefinition
    {
        public IsabellaLover() : base(
                FollowerIds.IsabellaLover,

                FollowerIds.Roles.Lover,

                "Isabella",

                () => CoC.Instance.Flags.Get(GameFlagIds.IsabellaFollowerAccepted) == 1 && CoC.Instance.Flags.Get(GameFlagIds.IsabellaCampDisabled) == 0,

                null,

                "Isabella lives at camp.",

                40)
        {
        }
    }
}