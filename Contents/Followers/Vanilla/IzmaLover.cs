using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<IzmaLover>("coc:izma")]
    public class IzmaLover : FollowerDefinition
    {
        public IzmaLover() : base(
                FollowerIds.IzmaLover,

                FollowerIds.Roles.Lover,

                "Izma",

                () => CoC.Instance.Flags.Get(GameFlagIds.IzmaFollowerStatus) == 1 && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmIzma) == 0,

                null,

                "Izma relaxes at camp.",

                30)
        {
        }
    }
}