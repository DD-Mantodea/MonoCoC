using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.World.Areas
{
    public interface IExplorable
    {
        AreaDefinition Area { get; }

        bool IsDiscovered(Player player);

        void Discover(Player player);

        void Explore(Player player);
    }
}