using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<LiddelliumBehavior>("coc:liddellium")]
    public class LiddelliumBehavior : ItemBehavior, IItemDescriptionBehavior
    {
        public string GetDescription(Item item, Player player, string baseDescription) =>

            CoC.Instance.Flags.Get(GameFlagIds.LiddelliumFlag) < 0

                ? "Items.Liddellium.KnownDesc".Localized()

                : baseDescription;
    }
}