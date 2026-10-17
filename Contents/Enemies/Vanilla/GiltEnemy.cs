using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/GiltCombatHandlers.cs（已实现，101 行）
    [Register<GiltEnemy>("coc:gilt")]
    public class GiltEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 2);

            Tallness = 6 * 12 + 6;

            Body.Get<SkinPart>().Tone = "golden-brown";

            var hair = Body.Get<HairPart>();

            hair.Color = "dark iridescent";

            hair.Length = 10;

            WeaponName = "sickle";

            ArmorName = "discolored metal plates laced together";
        }

        public override void OutputAttack(Combat.CombatContext context, float damage)
        {
            context.Append(IsFlying

                ? "Combat.Gilt.Attack.Flying".Localized()

                : "Combat.Gilt.Attack.Ground".Localized());

            if (damage > 0)
                context.Append("Combat.Gilt.Attack.Damage".Localized((int)damage));
        }

        public override MonoCoC.Registry.Identifier? ForcedAbility(Combat.CombatContext context)
        {
            if (Distance == Combat.CombatDistance.Distant && !IsFlying && Fatigue >= 10)
                return new MonoCoC.Registry.Identifier("coc", "gilt_fly");

            return null;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.GiltScene>.Get().GiltWinChoices(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.GiltScene>.Get().GiltLose(hpVictory);

            return true;
        }
    }
}