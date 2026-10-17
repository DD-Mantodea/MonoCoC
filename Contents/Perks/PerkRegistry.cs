using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Perks
{
    public static class PerkRegistry
    {
        public static Registry<PerkType> Registry => ManagerInstance<PerkManager>.Instance.Registry;

        public static PerkType? Get(Identifier id) => Registry.TryGet(id, out var perk) ? perk : null;

        public static bool TryGet(Identifier id, out PerkType perk) => Registry.TryGet(id, out perk);

        public static string GetName(Identifier id) => Get(id)?.Name ?? id.Path;
    }
}