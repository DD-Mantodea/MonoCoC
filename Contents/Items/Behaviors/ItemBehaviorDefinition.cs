using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Behaviors
{
    public class ItemBehaviorDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string Type = "";

        public IItemBehavior? Behavior;
    }
}