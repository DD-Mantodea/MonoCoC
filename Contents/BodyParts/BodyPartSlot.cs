using Hjson;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.BodyParts
{
    public class BodyPartSlot : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string Name
        {
            get
            {
                var key = $"BodyParts.Kind.{Id.Path}.Name";

                var value = key.Localized();

                return value == key ? Id.Path : value;
            }
        }

        public Identifier DefaultType;

        public Registry<BodyPartType> Types = new();

        public bool Collection;

        public Func<BodyPart> Factory;

        public JsonObject Config;
    }
}