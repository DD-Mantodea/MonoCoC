using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/SatyrCombatHandlers.cs（已实现，176 行）
    [Register<SatyrEnemy>("coc:satyr")]
    public class SatyrEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(13) + 14, 1.5f + Random.Shared.Next(20) / 2, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 2;

            Tallness = Random.Shared.Next(37) + 64;

            Body.Get<SkinPart>().Tone = "tan";

            var hair = Body.Get<HairPart>();

            hair.Length = 3;

            WeaponName = "fist";

            ArmorName = "thick fur";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Plains.SatyrScene>.Get().DefeatASatyr();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.Satyr.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Plains.SatyrScene>.Get().LoseToSatyr();

            return true;
        }
    }
}