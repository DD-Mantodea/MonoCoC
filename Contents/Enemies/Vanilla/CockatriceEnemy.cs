using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/CockatriceCombatHandlers.cs（已实现，177 行）
    [Register<CockatriceEnemy>("coc:cockatrice")]
    public class CockatriceEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(8, 2, BodyPartTypes.Lizard);

            Balls = 2;

            BallSize = 2;

            Tallness = 6 * 12 + 2;

            Body.Get<SkinPart>().Tone = "midnight blue";

            var hair = Body.Get<HairPart>();

            hair.Color = "blue";

            hair.Length = 2;

            WeaponName = "talons";

            ArmorName = "scales and feathers";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.CockatriceScene>.Get().DefeatCockatrice();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.Scene.ClearOutput();

            Scenes.Scene.ClearButtons();

            if (hpVictory)
            {
                CoC.Instance.Player.Health = 1;

                Scenes.Scene.OutputText("Combat.Cockatrice.WoundsTooGreat".Localized());
            }

            if (HasStatusEffect(StatusEffectIds.Infested))
                Scenes.Scene.OutputText("Combat.Cockatrice.CameWorms".Localized());

            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.CockatriceScene>.Get().LoseToCockatrice();

            return true;
        }
    }
}