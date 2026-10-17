using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects
{
    public abstract class ConsumableEffect : IConsumableEffect
    {
        public Identifier Id { get; set; }

        public abstract void Apply(ItemUseContext context);
    }
}