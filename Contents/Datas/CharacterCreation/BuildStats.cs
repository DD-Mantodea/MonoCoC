using MonoCoC.Contents.Creatures;
using System.Collections.Generic;

namespace MonoCoC.Contents.Datas.CharacterCreation
{
    public class BuildStats
    {
        public float Thickness;

        public float Strength;

        public float Toughness;

        public float Speed;

        public float Tone;

        public float? SetToneMale;

        public float? SetToneFemale;

        public Dictionary<Age, BuildMods> Male;

        public Dictionary<Age, BuildMods> Female;

        public Dictionary<Age, BuildMods> Mods(Gender buildGender) => buildGender == Gender.Female ? Female : Male;
    }
}