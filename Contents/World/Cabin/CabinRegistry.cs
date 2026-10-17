using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.World.Cabin
{
    public static class CabinHandlerRegistry
    {
        public static readonly Dictionary<Identifier, Action<CabinBuildContext>> Handlers = [];

        static CabinHandlerRegistry()
        {
            Register(new Identifier("coc", "cabin_mark_built"), context => context.Flags.Set(GameFlagIds.CampBuiltCabin, 1));

            Register(new Identifier("coc", "cabin_study_guide"), context => context.Player.Stats.SetBase(
                GameStats.Intelligence,

                context.Player.Stats.GetBase(GameStats.Intelligence) + 1));
        }

        public static void Register(Identifier id, Action<CabinBuildContext> handler) => Handlers[id] = handler;

        public static bool Has(Identifier id) => Handlers.ContainsKey(id);

        public static void Invoke(Identifier id, CabinBuildContext context)
        {
            if (Handlers.TryGetValue(id, out var handler))
                handler(context);
        }
    }

    public static class CabinStageRegistry
    {
        public static Registry<CabinStage> Stages => ManagerInstance<CabinStageManager>.Instance.Registry;

        public static IReadOnlyList<CabinStage> Ordered() =>

            Stages.Ids
                .Select(id => Stages.Get(id))
                .OrderBy(stage => stage.Order)
                .ThenBy(stage => stage.Id.ToString())
                .ToList();

        public static CabinStage? First() => Ordered().FirstOrDefault();

        public static CabinStage? Next(Identifier current)
        {
            var ordered = Ordered();

            for (var index = 0; index < ordered.Count; index++)
            {
                if (ordered[index].Id != current)
                    continue;

                return index + 1 < ordered.Count ? ordered[index + 1] : null;
            }

            return null;
        }

        public static void Register(CabinStage stage) => Stages.Register(stage.Id, () => stage);
    }

    public static class CabinFurnitureRegistry
    {
        public static Registry<CabinFurniture> Furniture => ManagerInstance<CabinFurnitureManager>.Instance.Registry;

        public static void Register(CabinFurniture furniture) => Furniture.Register(furniture.Id, () => furniture);

        public static IReadOnlyList<CabinFurniture> All() => Furniture.Ids.Select(id => Furniture.Get(id)).ToList();
    }

    public static class CabinResourceRegistry
    {
        public static Registry<CabinResource> Resources => ManagerInstance<CabinResourceManager>.Instance.Registry;

        public static void Register(CabinResource resource) => Resources.Register(resource.Id, () => resource);
    }
}
