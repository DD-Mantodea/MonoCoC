using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/FrostGiantCombatHandlers.cs（已实现，324 行）
    [Register<FrostGiantEnemy>("coc:frost_giant")]
    public class FrostGiantEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(36, 4, BodyPartTypes.Human);

            Balls = 2;

            BallSize = 2;

            Tallness = 20 * 12;

            Body.Get<SkinPart>().Tone = "deep blue";

            var hair = Body.Get<HairPart>();

            hair.Color = "white";

            hair.Length = 8;

            WeaponName = "fists";

            ArmorName = "ice";
        }

        public override void Struggle(Combat.CombatContext context) =>

            Combat.Handlers.FrostGiantCombatHandlers.GiantGrabStruggle(context);

        public override bool React(Combat.CombatContext context, Combat.CombatReactionContext reaction)
        {
            switch (reaction)
            {
                case Combat.CombatReactionContext.BeforeAttacked:
                    if (context.Player.HasStatusEffect(StatusEffectIds.GiantBoulder))
                    {
                        Combat.Handlers.FrostGiantCombatHandlers.GiantBoulderHit(context, 0);

                        return false;
                    }

                    break;

                case Combat.CombatReactionContext.PlayerWaited:
                    if (context.Player.HasStatusEffect(StatusEffectIds.GiantGrabbed))
                    {
                        Combat.Handlers.FrostGiantCombatHandlers.GiantGrabFail(context, false);

                        return false;
                    }

                    if (context.Player.HasStatusEffect(StatusEffectIds.GiantBoulder))
                    {
                        Combat.Handlers.FrostGiantCombatHandlers.GiantBoulderMiss(context);

                        return false;
                    }

                    break;
            }

            return true;
        }

        // 原版 FrostGiant.hx:231 defeated() -> winAgainstGiant()；won() 仅调用默认战后清理，无需覆写
        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.GlacialRift.FrostGiantScene>.Get().WinAgainstGiant();

            return true;
        }
    }
}