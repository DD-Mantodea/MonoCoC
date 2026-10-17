using Hjson;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts
{
    public abstract class BodyPart : IIdentifiable
    {
        public Identifier _id;

        public Identifier Id
        {
            get => _id;

            set
            {
                _id = value;

                OnTypeChanged();
            }
        }

        public Body Body;

        public abstract Identifier SlotId { get; }

        protected virtual void OnTypeChanged()
        {
        }

        public virtual void Restore() => Id = BodyPartSlotRegistry.Get(SlotId).DefaultType;

        public virtual BodyPartType Type => BodyPartSlotRegistry.Get(SlotId).Types.Get(Id);

        public string TypeName
        {
            get
            {
                var type = BodyPartSlotRegistry.Get(SlotId).Types.Get(Id);

                var name = type.Name;

                return name == type.Id.Path ? name.Replace('_', ' ') : name;
            }
        }

        public virtual string Descript(bool noAdj = false, bool noTone = false) => TypeName;

        public abstract void Save(JsonObject json);

        public abstract void Load(JsonObject json);
    }
}