using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<AmilySlave>("coc:amily_slave")]
    public class AmilySlave : FollowerDefinition
    {
        public AmilySlave() : base(
                FollowerIds.AmilySlave,

                FollowerIds.Roles.Slave,

                "Amily",

                () => CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 2 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) == 0,

                null,

                "The corrupted Amily serves at camp.",

                30)
        {
        }
    }
}