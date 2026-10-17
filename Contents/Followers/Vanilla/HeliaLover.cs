using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<HeliaLover>("coc:helia")]
    public class HeliaLover : FollowerDefinition
    {
        public HeliaLover() : base(
                FollowerIds.HeliaLover,

                FollowerIds.Roles.Lover,

                "Helia",

                () => CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) == 2 || (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) > 0 && CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0),

                null,

                "Helia shares your camp.",

                20,

                () => CoC.Instance.Flags.Get(GameFlagIds.HelGuarding) > 0 && (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) == 2 || (CoC.Instance.Flags.Get(GameFlagIds.HelFollowerLevel) > 0 && CoC.Instance.Flags.Get(GameFlagIds.HelHarpyQueenDefeated) == 0)))
        {
        }
    }
}