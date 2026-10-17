using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.World.Areas
{
    public static class AreaRegistry
    {
        public static Registry<AreaDefinition> Areas => ManagerInstance<AreaManager>.Instance.Registry;

        public static AreaDefinition? Get(Identifier id) =>

            Areas.TryGet(id, out var area) ? area : null;

        public static AreaDefinition? Get(string id) => Get(Identifier.Parse(id));

        public static IReadOnlyList<AreaDefinition> Ordered =>

            [.. Order(Areas.Ids.Select(Get).OfType<AreaDefinition>())];

        public static IReadOnlyList<AreaDefinition> Discovered(Player player) =>

            [.. Order(Areas.Ids
                .Select(Get)
                .OfType<AreaDefinition>()
                .Where(area => area.IsDiscovered(player)))];

        public static IEnumerable<AreaDefinition> Order(IEnumerable<AreaDefinition> areas) =>

            areas
                .OrderBy(area => area.Order.HasValue ? 0 : 1)
                .ThenBy(area => area.Order ?? 0)
                .ThenBy(area => area.Id.Path);

        public static void Discover(AreaDefinition area)
        {
            switch (area.Condition.Kind)
            {
                case AreaConditionKind.Flag:
                    CoC.Instance.Flags.Set(area.Condition.BoolFlag, true);

                    break;

                case AreaConditionKind.Compare:
                    CoC.Instance.Flags.Set(area.Condition.IntFlag, Math.Max(area.Condition.Value, 1));

                    break;

                case AreaConditionKind.Code:
                    Unlock(area.Id);

                    break;
            }
        }

        public static bool Unlock(Identifier id)
        {
            var area = Get(id);

            if (area == null || area.Condition.Kind != AreaConditionKind.Code)
                return false;

            CoC.Instance.Flags.Set(area.Condition.UnlockFlag, 1);

            return true;
        }

        public static bool Unlock(string id) => Unlock(Identifier.Parse(id));
    }
}
