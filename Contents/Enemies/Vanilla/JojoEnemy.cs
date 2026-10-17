using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Npcs;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<JojoEnemy>("coc:jojo")]
    public class JojoEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            var noFur = !CoC.Instance.Player.HasFur();

            CreateCock(7.5f, 1.8f, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 1;

            CumMultiplier = 1;

            CreateBreastRow(0);

            Tallness = 48;

            Body.Get<SkinPart>().Tone = "white";

            Body.Get<SkinPart>().Id = noFur ? BodyPartTypes.Plain : BodyPartTypes.Fur;

            Body.Get<HairPart>().Color = "white";

            Body.Get<HairPart>().Length = 2;

            WeaponName = noFur ? "fist" : "paw";

            ArmorName = "robes";
        }

        public override void PerformCombatAction()
        {
            var controller = CombatController.Instance!;

            var context = controller.Context;

            var status = CoC.Instance.Flags.Get(GameFlagIds.JojoStatus);

            if (status > 1 && Random.Shared.NextDouble() < 0.33)
            {
                switch (status)
                {
                    case 2:
                        context.Append("Scenes.Jojo.SelfCorruption.LostInThought".Localized());

                        Lust += 4;

                        break;

                    case 3:
                        context.Append("Scenes.Jojo.SelfCorruption.BlushDistractedBulge".Localized());

                        Lust += 8;

                        break;

                    case 4:
                        context.Append("Scenes.Jojo.SelfCorruption.StumblePullsHand".Localized());

                        Lust += 10;

                        break;

                    default:
                        context.Append("Scenes.Jojo.SelfCorruption.FranticallyJerksOff".Localized(CockDescriptShort(0), CockDescriptShort(0)));

                        Lust += 15;

                        break;
                }

                return;
            }

            controller.Resolver.EnemyBasicAttack();
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<JojoScene>.Get().DefeatedJojo(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            SceneInstance<JojoScene>.Get().LoseToJojo();

            return true;
        }
    }
}