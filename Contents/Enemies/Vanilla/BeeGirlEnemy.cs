using MonoCoC.Contents.BodyParts;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Registry;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<BeeGirlEnemy>("coc:bee_girl")]
    public class BeeGirlEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 4);

            CreateBreastRow(5f);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 3;

            anal.Wetness = 2;

            Tallness = Random.Shared.Next(14) + 59;

            Body.SetHipsRating("curvy", 3);

            Body.SetButtRating("expansive");

            Body.Get<LowerBodyPart>().Id = new Identifier("coc", "bee");

            Body.Get<SkinPart>().Tone = "yellow";

            var hair = Body.Get<HairPart>();

            hair.Color = Random.Shared.Next(2) == 0 ? "black" : "black and yellow";

            hair.Length = 6;

            Body.Get<AntennaePart>().Id = new Identifier("coc", "bee");

            Body.Get<WingsPart>().Id = new Identifier("coc", "bee_like_small");

            Body.Get<TailPart>().Id = new Identifier("coc", "bee_abdomen");

            Lust = 20 + Random.Shared.Next(40);

            Gems = Random.Shared.Next(15) + 1;

            ArmorName = "chitin";

            ArmorDefense = 18;

            WeaponName = "chitin-plated fist";

            WeaponVerb = "armored punch";

            LustVulnerability = 0.9f;
        }
    }
}