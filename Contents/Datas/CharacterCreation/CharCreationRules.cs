using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Stats;
using System.Collections.Generic;

namespace MonoCoC.Contents.Datas.CharacterCreation
{
    public class CharCreationRules
    {
        public Dictionary<Age, AgeStats> Ages = [];

        public Dictionary<Build, BuildStats> Builds = [];

        public Dictionary<Gender, float> DefaultHairLength = [];

        public Dictionary<Gender, Stat[]> StatBonuses = [];
    }
}