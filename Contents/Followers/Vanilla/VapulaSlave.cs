using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<VapulaSlave>("coc:vapula")]
    public class VapulaSlave : FollowerDefinition
    {
        public VapulaSlave() : base(
                FollowerIds.VapulaSlave,

                FollowerIds.Roles.Slave,

                "Vapula",

                () => CoC.Instance.Flags.Get(GameFlagIds.VapulaFollower) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmVapula) == 0,

                null,

                "Vapula is kept at camp.",

                20)
        {
        }
    }
}