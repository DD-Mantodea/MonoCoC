using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<LatexGirlSlave>("coc:latex_girl")]
    public class LatexGirlSlave : FollowerDefinition
    {
        public LatexGirlSlave() : base(
                FollowerIds.LatexGirlSlave,

                FollowerIds.Roles.Slave,

                "Latex Girl",

                () => CoC.Instance.Flags.Get(GameFlagIds.GooSlaveRecruited) > 0,

                null,

                "The latex goo-girl serves at camp.",

                10)
        {
        }
    }
}