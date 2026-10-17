using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<KihaLover>("coc:kiha")]
    public class KihaLover : FollowerDefinition
    {
        public KihaLover() : base(
                FollowerIds.KihaLover,

                FollowerIds.Roles.Lover,

                "Kiha",

                () => CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) != 1,

                null,

                "Kiha stays near the camp.",

                70,

                () => CoC.Instance.Flags.Get(GameFlagIds.KihaCampWatch) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaFollower) > 0 && CoC.Instance.Flags.Get(GameFlagIds.KihaCorruptionBitch) != 1)
        {
        }
    }
}