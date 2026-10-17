using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<SophieFollower>("coc:sophie_follower")]
    public class SophieFollower : FollowerDefinition
    {
        public SophieFollower() : base(
                FollowerIds.SophieFollower,

                FollowerIds.Roles.Follower,

                "Sophie",

                () => CoC.Instance.Flags.Get(GameFlagIds.SophieDisabledForever) == 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) == 0 && (CoC.Instance.Flags.Get(GameFlagIds.SophieRecruitedPure) > 0 || (CoC.Instance.Flags.Get(GameFlagIds.SophieBimbo) > 0 && CoC.Instance.Flags.Get(GameFlagIds.SophieDebimboed) > 0)),

                null,

                "The harpy matron rests in her nest.",

                50)
        {
        }
    }
}