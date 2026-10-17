using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<AmilyLover>("coc:amily_lover")]
    public class AmilyLover : FollowerDefinition
    {
        public AmilyLover() : base(
                FollowerIds.AmilyLover,

                FollowerIds.Roles.Lover,

                "Amily",

                () => CoC.Instance.Flags.Get(GameFlagIds.AmilyFollower) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmAmily) == 0 && (CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) == 0 || CoC.Instance.Flags.Get(GameFlagIds.AmilyVisitingUrta) > 2),

                null,

                "Amily tends her nest at camp.",

                60)
        {
        }
    }
}