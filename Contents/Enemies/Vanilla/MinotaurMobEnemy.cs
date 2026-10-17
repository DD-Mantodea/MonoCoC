using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/MinotaurMobCombatHandlers.cs（已实现，214 行）
    [Register<MinotaurMobEnemy>("coc:minotaur_mob")]
    public class MinotaurMobEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(13) + 24, 2 + Random.Shared.Next(3), BodyPartTypes.Horse);

            Balls = 2;

            BallSize = 2;

            Tallness = Random.Shared.Next(37) + 84;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Length = 3;

            WeaponName = "fists";

            ArmorName = "thick fur";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.MinotaurMobScene>.Get().VictoryMinotaurGang();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.MinotaurMob.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.MinotaurMobScene>.Get().MinotaurDeFeet();

            return true;
        }
    }
}