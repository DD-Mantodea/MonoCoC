using Hjson;
using MonoCoC.Registry;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MonoCoC.Contents.BodyParts
{
    public class Body
    {
        public readonly Dictionary<Identifier, BodyPart> _parts = [];

        public IReadOnlyDictionary<Identifier, BodyPart> Parts => _parts;

        public readonly Dictionary<Identifier, IList> _collections = [];

        public readonly Dictionary<Type, Identifier> _collectionTypes = [];

        public List<T> GetList<T>() where T : BodyPart
        {
            if (!_collectionTypes.TryGetValue(typeof(T), out var slotId))
                throw new InvalidOperationException($"No collection registered for {typeof(T).Name}");

            if (_collections.TryGetValue(slotId, out var existing))
                return (List<T>)existing;

            var list = new List<T>();

            _collections[slotId] = list;

            return list;
        }

        public float Butt
        {
            get => Get<Vanilla.ButtPart>().Rating;

            set => Get<Vanilla.ButtPart>().Rating = value;
        }

        public float Hips
        {
            get => Get<Vanilla.HipsPart>().Rating;

            set => Get<Vanilla.HipsPart>().Rating = value;
        }

        public void SetButtRating(string anchor, float delta = 0f) =>

            Butt = Get<Vanilla.ButtPart>().Type.RatingAnchor(anchor) + delta;

        public void SetHipsRating(string anchor, float delta = 0f) =>

            Hips = Get<Vanilla.HipsPart>().Type.RatingAnchor(anchor) + delta;

        public Body()
        {
            foreach (var slotId in BodyPartSlotRegistry.Registry.Ids)
                if (BodyPartSlotRegistry.Get(slotId).Collection)
                    _collectionTypes[BodyPartSlotRegistry.Get(slotId).Factory().GetType()] = slotId;
        }

        public static Body Create()
        {
            var body = new Body();

            foreach (var slotId in BodyPartSlotRegistry.Registry.Ids)
            {
                if (BodyPartSlotRegistry.Get(slotId).Collection)
                    continue;

                body.Add(BodyPartSlotRegistry.New(slotId));
            }

            body.Restore();

            return body;
        }

        public Body Add(BodyPart part)
        {
            part.Body = this;

            _parts[part.SlotId] = part;

            return this;
        }

        public BodyPart Get(string kind) => Get(new Identifier("coc", kind));

        public BodyPart Get(Identifier slotId) => !string.IsNullOrEmpty(slotId.Namespace) && _parts.TryGetValue(slotId, out var part) ? part : null;

        public T Get<T>() where T : BodyPart
        {
            foreach (var part in _parts.Values)
                if (part is T typed)
                    return typed;

            return null;
        }

        public void Restore()
        {
            SetHipsRating("boyish");

            SetButtRating("buttless");

            foreach (var part in _parts.Values)
                part.Restore();
        }

        public void Save(JsonObject json)
        {
            json["hips"] = Hips;

            json["butt"] = Butt;

            foreach (var pair in _parts)
            {
                var partJson = new JsonObject();

                pair.Value.Save(partJson);

                json[pair.Key.ToString()] = partJson;
            }
        }

        public void Load(JsonObject json)
        {
            Hips = json.TryGetValue("hips", out var hips) ? (float)(double)hips : Get<Vanilla.HipsPart>().Type.RatingAnchor("boyish");

            Butt = json.TryGetValue("butt", out var butt) ? (float)(double)butt : Get<Vanilla.ButtPart>().Type.RatingAnchor("buttless");

            foreach (var pair in _parts)
            {
                if (!json.TryGetValue(pair.Key.ToString(), out var value) || value is not JsonObject partJson)
                {
                    pair.Value.Restore();

                    continue;
                }

                pair.Value.Load(partJson);
            }
        }
    }
}