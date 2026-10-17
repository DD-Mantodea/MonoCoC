using MonoCoC.Registry;

namespace MonoCoC.Contents.Items.Effects
{
    public interface IConsumableEffect : IIdentifiable
    {
        void Apply(ItemUseContext context);
    }
}