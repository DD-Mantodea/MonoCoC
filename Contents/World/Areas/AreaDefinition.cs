using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Registry;

namespace MonoCoC.Contents.World.Areas
{
    public class AreaDefinition : IIdentifiable
    {
        public Identifier Id { get; set; }

        public Identifier SceneId;

        public string NameKey = "";

        public string DescriptionKey = "";

        public string DiscoveryKey = "";

        public string ExploreKey = "";

        public AreaCondition Condition = AreaCondition.Always();

        public Flag<int> ExplorationFlag;

        public int? Order;

        public bool IsDiscovered(Player player) => Condition.IsSatisfied(player);
    }
}