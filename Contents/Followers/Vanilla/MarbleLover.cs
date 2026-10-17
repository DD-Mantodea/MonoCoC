using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<MarbleLover>("coc:marble")]
    public class MarbleLover : FollowerDefinition
    {
        public MarbleLover() : base(
                FollowerIds.MarbleLover,

                FollowerIds.Roles.Lover,

                "Marble",

                () => CoC.Instance.Player.HasStatusEffect(StatusEffectIds.CampMarble) && CoC.Instance.Flags.Get(GameFlagIds.FollowerAtFarmMarble) == 0,

                null,

                "Marble is at camp.",

                50)
        {
        }
    }
}