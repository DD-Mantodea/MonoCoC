using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<MarbleEnemy>("coc:marble")]
    public class MarbleEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 2);

            CreateBreastRow(BreastCups.F);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            Tallness = 76;

            Body.Hips = 10;

            Body.Butt = 8;

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Hoofed;

            Body.Get<SkinPart>().Tone = "pale";

            var hair = Body.Get<HairPart>();

            hair.Color = "brown";

            hair.Length = 13;

            Strength = 75;

            Toughness = 70;

            Speed = 35;

            Intelligence = 40;

            Libido = 25;

            Sensitivity = 45;

            Corruption = 40;

            WeaponName = "large hammer";

            WeaponVerb = "hammer-blow";

            WeaponAttack = 10;

            ArmorName = "tough hide";

            ArmorDefense = 5;

            Level = 7;

            Gems = Random.Shared.Next(5) + 25;

            Body.Get<TailPart>().Id = BodyPartTypes.Cow;

        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<MarbleScene>.Get().MarbleFightWin();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<MarbleScene>.Get().MarbleFightLose();

            return true;
        }
    }
}