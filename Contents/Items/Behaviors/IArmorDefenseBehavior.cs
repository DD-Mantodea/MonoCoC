using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.Items.Behaviors
{
    public interface IArmorDefenseBehavior : IItemBehavior
    {
        float GetDefense(Armor armor, Player player);
    }
}