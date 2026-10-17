using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<PhoenixPlatoonEnemy>("coc:phoenix_platoon")]
    public class PhoenixPlatoonEnemy : Enemy
    {

        public int _platoonStage;

        public static readonly Identifier RushId = new("coc", "phoenix_platoon_rush");

        public static readonly Identifier FireBreathId = new("coc", "phoenix_platoon_fire_breath");

        public static readonly Identifier LustbangId = new("coc", "phoenix_platoon_lustbang");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock();

            Balls = 2;

            BallSize = 1;

            CumMultiplier = 3;

            CreateVagina(false, 5, 2);

            CreateBreastRow(BreastCups.D);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;

            anal.Wetness = 0;

            Tallness = Random.Shared.Next(8) + 70;

            Body.SetHipsRating("ample", 2);

            Body.SetButtRating("large");

            Body.Get<LowerBodyPart>().Id = BodyPartTypes.Lizard;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "spears";

            WeaponVerb = "stab";

            WeaponAttack = 20;

            ArmorName = "armor";

            ArmorDefense = 50;

            LustVulnerability = .15f;

            Gems = Random.Shared.Next(25) + 160;

            AdditionalExperience = 50;

            var horns = Body.Get<HornsPart>();

            horns.Id = BodyPartTypes.DraconicX2;

            horns.Value = 2;

            Body.Get<TailPart>().Id = BodyPartTypes.Harpy;

            Body.Get<WingsPart>().Id = BodyPartTypes.FeatheredLarge;

        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var ability = _platoonStage switch
            {
                0 => RushId,

                1 => FireBreathId,

                _ => LustbangId
            };

            _platoonStage = (_platoonStage + 1) % 3;

            return new CombatAiAction { Ability = ability };
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Hel.HelDungeonScene>.Get().PhoenixPlatoonLosesToPC();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Hel.HelDungeonScene>.Get().PhoenixPlatoonMurdersPC();

            return true;
        }
    }
}