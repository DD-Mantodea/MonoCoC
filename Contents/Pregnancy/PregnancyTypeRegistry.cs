using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Pregnancy
{
    public static class PregnancyTypeRegistry
    {
        public static Registry<PregnancyType> Types => ManagerInstance<PregnancyTypeManager>.Instance.Registry;

        public static PregnancyType? Get(Identifier id) =>

            Types.TryGet(id, out var type) ? type : null;
    }
}