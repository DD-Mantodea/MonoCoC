using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Configuration;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/ChameleonGirlCombatHandlers.cs（已实现，141 行）
    [Register<ChameleonGirlEnemy>("coc:chameleon_girl")]
    public class ChameleonGirlEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 5, 3);

            Tallness = Random.Shared.Next(2) + 68;

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "tongue";

            ArmorName = "skin";
        }

        public override bool Defeated(bool hpVictory)
        {
            SceneInstance<Scenes.Vanilla.Areas.Bog.ChameleonGirlScene>.Get().DefeatChameleonGirl();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            var player = CoC.Instance.Player;

            if (player.HasCock()
                && !player.HasStatusEffect(StatusEffectIds.ParasiteSlug)

                && !player.HasPerk(PerkIds.ParasiteMusk)

                && !player.HasStatusEffect(StatusEffectIds.Infested)

                && Random.Shared.Next(100) < OptionStore.Instance.Get("parasites", 0) * 20)

                player.CreateStatusEffect(StatusEffectIds.ParasiteSlug, value1: 72);

            if (player.HasStatusEffect(StatusEffectIds.Infested))
            {
                Scene.OutputText("Combat.ChameleonGirl.CameWorms".Localized());

                Combat.CombatScene.CleanupAfterCombat();

                return true;
            }

            SceneInstance<Scenes.Vanilla.Areas.Bog.ChameleonGirlScene>.Get().LoseToChameleonGirl();

            return true;
        }
    }
}