using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<ValeriaFollower>("coc:valeria")]
    public class ValeriaFollower : FollowerDefinition
    {
        public ValeriaFollower() : base(
                FollowerIds.ValeriaFollower,

                FollowerIds.Roles.Follower,

                "Valeria",

                () => CoC.Instance.Flags.Get(GameFlagIds.ValariaAtCamp) == 1 || CoC.Instance.Player.Armor?.Id == ArmorIds.GooArmor,

                null,

                "Valeria is staying at your camp.",

                20)
        {
        }
    }
}