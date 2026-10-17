using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/CorruptedWitchCombatHandlers.cs（已实现，302 行）
    [Register<CorruptedWitchEnemy>("coc:corrupted_witch")]
    public class CorruptedWitchEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            Balls = 0;

            BallSize = 0;

            CreateVagina(false, 5, 3);

            Tallness = Random.Shared.Next(12) + 55;

            Body.Get<SkinPart>().Tone = "ashen";

            var hair = Body.Get<HairPart>();

            hair.Color = "red";

            hair.Length = 15;

            WeaponName = "fists";

            ArmorName = "robes";
        }

        public override MonoCoC.Registry.Identifier? ForcedAbility(Combat.CombatContext context)
        {
            if (Health < MaxHealth * 0.6f && Fatigue <= 90 && context.Player.HasCock())
                return new MonoCoC.Registry.Identifier("coc", "corrupted_witch_heals");

            return null;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.CorruptedWitchScene>.Get().DefeatWitch();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            if (HasStatusEffect(StatusEffectIds.Infested))
            {
                Scenes.Scene.OutputText("Combat.CorruptedWitch.CameWorms".Localized());

                Combat.CombatScene.CleanupAfterCombat();

                return true;
            }

            Scenes.SceneInstance<Scenes.Vanilla.Areas.VolcanicCrag.CorruptedWitchScene>.Get().LoseToWitch();

            return true;
        }

        public override void Struggle(Combat.CombatContext context) =>

            Combat.Handlers.CorruptedWitchCombatHandlers.GrabStruggle(context);

        public override bool React(Combat.CombatContext context, Combat.CombatReactionContext reaction)
        {
            if (reaction == Combat.CombatReactionContext.PlayerWaited
                && context.Player.HasStatusEffect(StatusEffectIds.CorrWitchBind))
            {
                Combat.Handlers.CorruptedWitchCombatHandlers.GrabFail(context, false);

                return false;
            }

            return true;
        }
    }
}