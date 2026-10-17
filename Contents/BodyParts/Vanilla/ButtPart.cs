using Hjson;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{

    public class ButtPart : BodyPart
    {
        public const float RatingButtless = 0f;

        public const float RatingTight = 2f;

        public const float RatingAverage = 4f;

        public const float RatingNoticeable = 6f;

        public const float RatingLarge = 8f;

        public const float RatingJiggly = 10f;

        public const float RatingExpansive = 13f;

        public const float RatingHuge = 16f;

        public override Identifier SlotId => Datas.Vanilla.BodyPartSlots.Butt;

        public float Rating;

        public override void Save(JsonObject json) => json["rating"] = Rating;

        public override void Load(JsonObject json)
        {
            if (json.TryGetValue("rating", out var rating) && rating is not null)
                Rating = (float)(double)rating;
        }
    }
}