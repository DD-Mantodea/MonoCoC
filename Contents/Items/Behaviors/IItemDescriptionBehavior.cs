using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.Items.Behaviors
{
    public interface IItemDescriptionBehavior : IItemBehavior
    {
        string GetDescription(Item item, Player player, string baseDescription);
    }
}