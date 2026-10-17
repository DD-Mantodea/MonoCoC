using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/InfestedChameleonGirlCombatHandlers.cs（已实现，144 行）
    [Register<InfestedChameleonGirlEnemy>("coc:infested_chameleon_girl")]
    public class InfestedChameleonGirlEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 3);

            Tallness = Random.Shared.Next(2) + 68;

            Body.Get<SkinPart>().Tone = "dark";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "claws";

            ArmorName = "skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<Scenes.Vanilla.Areas.Bog.InfestedChameleonGirlScene>
                .Get().DefeatChameleonGirl();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var p = CoC.Instance.Player;

            if (p.HasCock()
                && !p.HasStatusEffect(StatusEffectIds.ParasiteSlug)

                && !p.HasPerk(PerkIds.ParasiteMusk)

                && !p.HasStatusEffect(StatusEffectIds.Infested)

                && Random.Shared.Next(100) < CoC.Instance.Flags.Get(GameFlagIds.FetishParasites) * 20)

                p.CreateStatusEffect(StatusEffectIds.ParasiteSlug, 72, 0, 0, 0);

            SceneInstance<Scenes.Vanilla.Areas.Bog.InfestedChameleonGirlScene>
                .Get().LoseToChameleonGirl();

            return true;
        }
    }
}
