using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/PhoenixCombatHandlers.cs（已实现，146 行）
    [Register<PhoenixEnemy>("coc:phoenix")]
    public class PhoenixEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(8, 1.2f, BodyPartTypes.Lizard);

            CreateVagina(false, 3, 3);

            Tallness = 6 * 12 + 6;

            Body.Get<SkinPart>().Tone = "light";

            var hair = Body.Get<HairPart>();

            hair.Color = "red";

            hair.Length = 16;

            WeaponName = "scimitar and shield";

            ArmorName = "chainmail and loincloth";
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.PhoenixScene>.Get().WinAgainstPhoenix();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.Phoenix.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.PhoenixScene>.Get().LoseToPhoenix();

            return true;
        }

        public override MonoCoC.Registry.Identifier? ForcedAbility(Combat.CombatContext context)
        {
            if (HasStatusEffect(StatusEffectIds.Uber))
                return new MonoCoC.Registry.Identifier("coc", "phoenix_fire_breath");

            return null;
        }
    }
}