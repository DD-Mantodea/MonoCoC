using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/GreenSlimeCombatHandlers.cs（已实现，62 行）
    [Register<GreenSlimeEnemy>("coc:green_slime")]
    public class GreenSlimeEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(18, 2, BodyPartTypes.Human);

            Tallness = Random.Shared.Next(8) + 80;

            Body.Get<SkinPart>().Tone = "green";

            WeaponName = "hands";

            ArmorName = "gelatinous skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.Lake.GreenSlimeScene>.Get().SlimeVictory();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
                Scenes.Scene.OutputText("Combat.GreenSlime.CameWorms".Localized());

            Scenes.SceneInstance<Scenes.Vanilla.Areas.Lake.GreenSlimeScene>.Get().SlimeLoss();

            return true;
        }
    }
}