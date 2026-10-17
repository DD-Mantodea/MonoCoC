using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<SophieSlave>("coc:sophie_slave")]
    public class SophieSlave : FollowerDefinition
    {
        public SophieSlave() : base(
                FollowerIds.SophieSlave,

                FollowerIds.Roles.Slave,

                "Sophie",

                () => CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) == 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieDebimboed) == 0,

                null,

                "The bimbo Sophie serves at camp.",

                40)
        {
        }
    }
}