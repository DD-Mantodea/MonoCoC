using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Behaviors
{
    public abstract class ItemBehavior : IItemBehavior
    {
        public Identifier Id { get; set; }
    }
}