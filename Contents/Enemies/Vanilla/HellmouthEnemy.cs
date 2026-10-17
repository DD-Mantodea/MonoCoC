using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/HellmouthCombatHandlers.cs（四招均已实现）。
    [Register<HellmouthEnemy>("coc:hellmouth")]
    public class HellmouthEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 2);

            Tallness = 35 + Random.Shared.Next(4);

            Body.Get<SkinPart>().Tone = "pale gray";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 10;

            WeaponName = "teeth";

            ArmorName = "nothing";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.HellmouthScene>.Get().BeatHellmouth();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var scene = Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.HellmouthScene>.Get();

            if (HasStatusEffect(StatusEffectIds.Infested))
                Scenes.Scene.OutputText("Combat.Monster.CameWormsIndifferent".Localized());

            scene.LoseToHellmouth();

            return true;
        }
    }
}