using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/HarpyCombatHandlers.cs（已实现，94 行）
    [Register<HarpyEnemy>("coc:harpy")]
    public class HarpyEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 3, 0);

            Tallness = 6 * 12 + 6;

            Body.Get<SkinPart>().Tone = "pink";

            var hair = Body.Get<HairPart>();

            hair.Color = "blue";

            hair.Length = 16;

            WeaponName = "talons";

            ArmorName = "feathers";
        }

        public override MonoCoC.Registry.Identifier? ForcedAbility(Combat.CombatContext context)
        {
            if (HasStatusEffect(Datas.Vanilla.StatusEffectIds.Uber))
                return new MonoCoC.Registry.Identifier("coc", "harpy_uber_charge");

            return null;
        }

        public override void OutputAttack(Combat.CombatContext context, float damage)
        {
            if (damage <= 0)
            {
                context.Append("Combat.Harpy.Attack.Deflected".Localized());

                return;
            }

            context.Append("Combat.Harpy.Attack.Hit".Localized((int)damage));
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.HarpyScene>.Get().HarpyVictoryuuuuu();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(Datas.Vanilla.StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.Harpy.CameWorms".Localized());

                return false;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.HighMountains.HarpyScene>.Get().HarpyLossU();

            return true;
        }
    }
}