using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Scenes.Vanilla.Areas.GlacialRift;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;

using CreatureGender = MonoCoC.Contents.Creatures.Gender;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<AmarokEnemy>("coc:amarok")]
    public class AmarokEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(8, 1.5f, BodyPartTypes.Wolf);

            Balls = 2;

            BallSize = 2;

            Tallness = 8 * 12;

            Body.Get<SkinPart>().Tone = "black";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 3;

            WeaponName = "teeth";

            ArmorName = "thick fur";
        }

        public override bool Won(bool hpVictory)
        {
            ApplyLoss(hpVictory);

            return false;
        }

        public void ApplyLoss(bool hpVictory)
        {
            var scene = SceneInstance<AmarokScene>.Get();

            if (CoC.Instance.Flags.Get(GameFlagIds.CameWormsAfterCombat) > 0)
            {

                Scene.OutputText("Combat.Amarok.Won.CameWorms".Localized());

                return;
            }

            if (Lust >= 33)
            {
                if (OptionStore.Instance.Get("watersports", 0) > 0 && Random.Shared.Next(2) == 0)
                {
                    scene.AmarokFacefuck();

                    return;
                }

                if (CoC.Instance.Player.IsChild())
                    switch (CoC.Instance.Player.Gender)
                    {
                        case CreatureGender.Male:
                        case CreatureGender.Hermaphrodite:
                            scene.AmarokLustDefeatMaleHermChild();

                            return;

                        case CreatureGender.Female:
                            scene.AmarokLustDefeatFemaleChild();

                            return;

                        default:
                            scene.AmarokChowTime();

                            return;
                    }

                switch (CoC.Instance.Player.Gender)
                {
                    case CreatureGender.Male:
                    case CreatureGender.Hermaphrodite:
                        scene.AmarokLustDefeatMaleAdult();

                        return;

                    case CreatureGender.Female:
                        scene.AmarokLustDefeatFemaleAdult();

                        return;

                    default:
                        scene.AmarokChowTime();

                        return;
                }
            }

            scene.AmarokChowTime();
        }
    }
}