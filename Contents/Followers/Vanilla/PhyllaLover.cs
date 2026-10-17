using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<PhyllaLover>("coc:phylla")]
    public class PhyllaLover : FollowerDefinition
    {
        public PhyllaLover() : base(
                FollowerIds.PhyllaLover,

                FollowerIds.Roles.Lover,

                "Phylla",

                () => CoC.Instance.Flags.Get(GameFlagIds.AntWaifu) > 0,

                null,

                "Phylla visits from the ant colony.",

                90)
        {
        }
    }
}