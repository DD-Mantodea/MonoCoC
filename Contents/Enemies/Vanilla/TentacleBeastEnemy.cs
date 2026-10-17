using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;
using MonoCoC.Attributes;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<TentacleBeastEnemy>("coc:tentacle_beast")]
    public class TentacleBeastEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(40f, 1.5f, new Identifier("coc", "tentacle"));

            CreateCock(60f, 1.5f, new Identifier("coc", "tentacle"));

            CreateCock(50f, 1.5f, new Identifier("coc", "tentacle"));

            CreateCock(20f, 1.5f, new Identifier("coc", "tentacle"));

            Balls = 0;

            BallSize = 0;

            CreateBreastRow(0f);

            Tallness = Random.Shared.Next(9) + 70;

            var skin = Body.Get<SkinPart>();

            skin.Tone = "green";

            var hair = Body.Get<HairPart>();

            hair.Color = "green";

            hair.Length = 1;
        }

        public override bool ShouldMove(Combat.CombatDistance newPos, bool forceAction = false) => false;

        public override bool React(Combat.CombatContext context, Combat.CombatReactionContext reaction)
        {
            if (reaction != Combat.CombatReactionContext.PlayerWaited)
                return true;

            if (!context.Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.TentacleBind))
                return true;

            context.Clear();

            if (context.Player.HasCock())
                context.WriteAction("Combat.TentacleBeast.React.TheCreatureContinuesSpiralingAroundYour".Localized());
            else if (context.Player.HasVagina())
                context.WriteAction("Combat.TentacleBeast.React.TheCreatureContinuesSuckingYourClit".Localized());
            else
                context.WriteAction("Combat.TentacleBeast.React.TheCreatureContinuesProbingAtYour".Localized((int)context.Player.TotalNipples()));

            var lust = 8 + context.Player.Sensitivity / 10f;

            context.Player.Teased(lust);

            TookAction = true;

            return false;
        }

        public override void Struggle(Combat.CombatContext context)
        {
            if (!context.Player.HasStatusEffect(Datas.Vanilla.StatusEffectIds.TentacleBind))
                return;

            context.WriteAction("Combat.TentacleBeast.Struggle.YouStruggleWithAllOfYour".Localized());

            var escaped = context.Random.Next(3) == 0 ||

                context.Random.Next(80) < context.Player.Strength / 2;

            if (escaped)
            {
                context.WriteAction("Combat.TentacleBeast.Struggle.AsTheCreatureAttemptsToAdjust".Localized());

                context.Player.RemoveStatusEffect(Datas.Vanilla.StatusEffectIds.TentacleBind);

                CreateStatusEffect(Datas.Vanilla.StatusEffectIds.TentacleCoolDown, 3, 0, 0, 0, 3);

                return;
            }

            context.WriteAction("Combat.TentacleBeast.Struggle.DespiteTryingToEscapeTheCreature".Localized());

            context.Player.HealthChange(-5);

            if (context.Player.HasCock())
                context.WriteAction("Combat.TentacleBeast.Struggle.TheCreatureContinuesSpiralingAroundYour".Localized());
            else if (context.Player.HasVagina())
                context.WriteAction("Combat.TentacleBeast.Struggle.TheCreatureContinuesSuckingYourClit".Localized());
            else
                context.WriteAction("Combat.TentacleBeast.Struggle.TheCreatureContinuesProbingAtYour".Localized((int)context.Player.TotalNipples()));

            var lust = 3 + context.Player.Sensitivity / 10f + context.Player.Libido / 20f;

            context.Player.Teased(lust);

            TookAction = true;
        }
    }
}