using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<NieveLover>("coc:nieve")]
    public class NieveLover : FollowerDefinition
    {
        public NieveLover() : base(
                FollowerIds.NieveLover,

                FollowerIds.Roles.Lover,

                "Nieve",

                () => CoC.Instance.Flags.Get(GameFlagIds.NieveStage) == 5,

                null,

                "Nieve is at camp.",

                80)
        {
        }
    }
}