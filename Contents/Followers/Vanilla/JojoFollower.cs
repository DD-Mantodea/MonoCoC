using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<JojoFollower>("coc:jojo")]
    public class JojoFollower : FollowerDefinition
    {
        public JojoFollower() : base(
                FollowerIds.JojoFollower,

                FollowerIds.Roles.Follower,

                "Jojo",

                () => CoC.Instance.Player.HasStatusEffect(StatusEffectIds.PureCampJojo) && CoC.Instance.Flags.Get(GameFlagIds.JojoDeadOrGone) == 0,

                null,

                "The mouse-monk keeps watch over the camp.",

                30,

                () => CoC.Instance.Player.HasStatusEffect(StatusEffectIds.JojoNightWatch) && CoC.Instance.Player.HasStatusEffect(StatusEffectIds.PureCampJojo) && CoC.Instance.Flags.Get(GameFlagIds.JojoDeadOrGone) == 0)
        {
        }
    }
}