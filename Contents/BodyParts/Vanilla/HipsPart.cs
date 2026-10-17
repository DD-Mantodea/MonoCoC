using Hjson;
using MonoCoC.Registry;

namespace MonoCoC.Contents.BodyParts.Vanilla
{

    public class HipsPart : BodyPart
    {
        public const float RatingBoyish = 0f;

        public const float RatingSlender = 2f;

        public const float RatingAverage = 4f;

        public const float RatingAmple = 6f;

        public const float RatingCurvy = 10f;

        public const float RatingFertile = 15f;

        public const float RatingInhumanlyWide = 20f;

        public override Identifier SlotId => Datas.Vanilla.BodyPartSlots.Hips;

        public float Rating;

        public override void Save(JsonObject json)
        {
            json["rating"] = Rating;
        }

        public override void Load(JsonObject json)
        {
            if (json.TryGetValue("rating", out var rating) && rating is not null)
                Rating = (float)(double)rating;
        }
    }
}