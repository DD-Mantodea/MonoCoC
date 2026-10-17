using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Items.Behaviors.Vanilla
{
    [Register<AkbalShieldBehavior>("coc:akbal_shield")]
    public class AkbalShieldBehavior : ItemBehavior, IItemDescriptionBehavior
    {
        public string GetDescription(Item item, Player player, string baseDescription) =>

            baseDescription + "\n" + "Items.AkbalShield.Special".Localized();
    }
}