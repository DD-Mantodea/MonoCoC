using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts
{
    public static class BodyPartSlotRegistry
    {
        public static readonly Registry<BodyPartSlot> Registry = new();

        public static BodyPartSlot Get(Identifier slotId) => Registry.Get(slotId);

        public static BodyPartSlot Get(string kind) => Registry.Get(new Identifier("coc", kind));

        public static BodyPart New(Identifier slotId, Identifier type = default)
        {
            var kind = Registry.Get(slotId);

            var part = kind.Factory();

            part.Id = string.IsNullOrEmpty(type.Namespace) ? kind.DefaultType : type;

            return part;
        }

        public static T New<T>(Identifier slotId, Identifier type = default) where T : BodyPart => (T)New(slotId, type);
    }
}