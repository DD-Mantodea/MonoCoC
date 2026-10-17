using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Stats;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<PlagueRatEnemy>("coc:plague_rat")]
    public class PlagueRatEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(2) + 5, 1.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 1;

            CreateBreastRow(0);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 4;

            anal.Wetness = 1;

            Tallness = Random.Shared.Next(6) + 57;

            Body.Hips = 0;

            Body.Butt = 2;

            var horns = Body.Get<HornsPart>();

            horns.Id = BodyPartTypes.Demon;

            horns.Value = 2;

            Body.Get<FacePart>().Id = BodyPartTypes.Mouse;

            var skin = Body.Get<SkinPart>();

            skin.Id = BodyPartTypes.Fur;

            skin.Adj = "patchy";

            skin.FurColor = "burgundy";

            skin.DescKey = "Skin.Desc.fur";

            skin.Tone = "red";

            Strength = 65;

            Toughness = 30;

            Speed = 80;

            Intelligence = 20;

            Libido = 55;

            Sensitivity = 35;

            Corruption = 100;

            WeaponName = "claw";

            WeaponVerb = "swipe";

            WeaponAttack = 10;

            ArmorName = "leathery skin";

            ArmorDefense = 5;

            var bonusHealth = 100f;

            Stats.AddModifier(GameStats.MaxHealth, ModifierOperation.Addition,

                () => bonusHealth, new TemporarySource(Guid.NewGuid()));

            Health = MaxHealth;

            Lust = 30;

            LustVulnerability = .65f;

            Level = 7;

            Gems = Random.Shared.Next(15) + 25;

        }

        public override string Description => Tallness < 60

            ? "Npcs.PlagueRat.Desc.NearPlayerHeight".Localized()

            : "Npcs.PlagueRat.Desc".Localized();

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Swamp.PlagueRatScene>.Get().PlagueDefeat(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Swamp.PlagueRatScene>.Get().PlagueVictory();

            return true;
        }
    }
}