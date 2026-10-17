using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Extensions;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/MoraCombatHandlers.cs（已实现，172 行）
    [Register<MoraEnemy>("coc:mora")]
    public class MoraEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateVagina(false, 1, 1);

            Tallness = 36;

            Body.Get<SkinPart>().Tone = "ashen";

            var hair = Body.Get<HairPart>();

            hair.Color = "dark";

            hair.Length = 16;

            WeaponName = "claws";

            ArmorName = "linen dress";
        }

        public int Shrouded;

        public override void CombatRoundUpdate(Combat.CombatContext context)
        {
            base.CombatRoundUpdate(context);

            if (Shrouded > 0)
                Shrouded--;
        }

        public override bool ShouldMove(Combat.CombatDistance newPos, bool forceAction = false) =>

            newPos != Combat.CombatDistance.Distant && base.ShouldMove(newPos, forceAction);

        public override float GetEvasionChance() => Lust >= 50 ? 0 : base.GetEvasionChance();

        public override int HandleCombatLossText(Combat.CombatContext context, bool inDungeon, int gemsLost)
        {
            if (inDungeon)
                return base.HandleCombatLossText(context, inDungeon, gemsLost);

            context.Append("Combat.Mora.Loss.Lead".Localized());

            var gems = context.Player.Gems;

            if (gems > 1)
                context.Append("Combat.Mora.Loss.Gems".Localized(gemsLost));
            else if (gems == 1)
                context.Append("Combat.Mora.Loss.LastGem".Localized());
            else
                context.Append("Combat.Mora.Loss.NoGems".Localized());

            return 8;
        }

        public override bool React(Combat.CombatContext context, Combat.CombatReactionContext reaction)
        {
            if (reaction is not (Combat.CombatReactionContext.BeforeAttacked or Combat.CombatReactionContext.BowHit))
                return true;

            var player = context.Player;

            if (reaction == Combat.CombatReactionContext.BeforeAttacked &&
                player.Weapon.GetAttackRange(player) != Combat.CombatRange.Ranged)

                return true;

            if (Shrouded != 0 && context.Random.Next(100) < 60)
            {
                context.Append("Combat.Mora.Shroud.Miss".Localized());

                return false;
            }

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.GlacialRift.MoraScene>.Get().MoraWinChoices(hpVictory);

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Areas.GlacialRift.MoraScene>.Get().MoraLose();

            return true;
        }
    }
}