using MonoCoC.Attributes;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;

namespace MonoCoC.Contents.Followers.Vanilla
{
    [Register<HelspawnLover>("coc:helspawn_lover")]
    public class HelspawnLover : FollowerDefinition
    {
        public HelspawnLover() : base(
                FollowerIds.HelspawnLover,

                FollowerIds.Roles.Lover,

                "Helspawn",

                () => CoC.Instance.Flags.Get(GameFlagIds.HelspawnAge) == 3 && CoC.Instance.Flags.Get(GameFlagIds.HelspawnIncest) > 0,

                null,

                "Your daughter shares your bedroll.",

                100)
        {
        }
    }
}