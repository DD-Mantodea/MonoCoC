using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.Datas.CharacterCreation
{
    public class AgeStats
    {
        public float Strength;

        public float Toughness;

        public float Speed;

        public float Intelligence;

        public float Sensitivity;

        public float Libido;

        public float Lust;

        public int Bonus;

        public float NippleLength;

        public float ClitLength;

        public float CockLength;

        public float CockThickness;

        public float BallSize;

        public float TallnessMale;

        public float TallnessFemale;

        public float TallnessHerm;

        public float FertilityMale;

        public float FertilityFemale;

        public float FertilityHerm;

        public float ToneMale;

        public float ToneFemale;

        public float ToneHerm;

        public float Tallness(Gender gender) => gender switch
        {
            Gender.Male => TallnessMale,

            Gender.Female => TallnessFemale,

            _ => TallnessHerm
        };

        public float Fertility(Gender gender) => gender switch
        {
            Gender.Male => FertilityMale,

            Gender.Female => FertilityFemale,

            _ => FertilityHerm
        };

        public float Tone(Gender gender) => gender switch
        {
            Gender.Male => ToneMale,

            Gender.Female => ToneFemale,

            _ => ToneHerm
        };
    }
}