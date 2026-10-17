using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.World.Areas;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Scenes.Vanilla
{
    public abstract class AreaScene : Scene, IExplorable
    {
        public AreaDefinition Area { get; }

        // 每个 Area 与它的场景一一对应，AreaDefinition 由场景自身（AreaIds 常量）确定，
        // 不再由 Scenes.hjson 的 area 字段动态注入——这样场景的 id 与区域定义不会脱钩。
        protected AreaScene(AreaDefinition area) => Area = area;

        protected AreaScene(Identifier areaId)

            : this(AreaRegistry.Get(areaId)

                   ?? throw new InvalidOperationException($"区域未注册：{areaId}（检查 Assets/Data/Areas 下的 hjson）"))
        {
        }

        public virtual bool IsDiscovered(Player player) => Area.IsDiscovered(player);

        public virtual void Discover(Player player) => AreaRegistry.Discover(Area);

        public virtual void Explore(Player player) => ExploreArea();

        public override void Entry()
        {
            ClearOutput();

            ClearButtons();

            Explore(Player);
        }

        protected virtual void ExploreArea()
        {
            Flags.Set(Area.ExplorationFlag, Flags.Get(Area.ExplorationFlag) + 1);

            ClearOutput();

            ClearButtons();

            OutputText(Area.ExploreKey.Localized());

            View.Time.Minutes = 0;

            View.Time.PassTime(1);

            AddButton(0, "Scenes.Explore.Continue".Localized(), () => Go<CampScene>());
        }
    }
}