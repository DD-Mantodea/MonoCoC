using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using System.Linq;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/PhoenixCommanderCombatHandlers.cs（已实现，59 行）
    [Register<PhoenixCommanderEnemy>("coc:phoenix_commander")]
    public class PhoenixCommanderEnemy : Enemy
    {
        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock();

            Balls = 2;

            BallSize = 1;

            CreateVagina(false, 5, 3);

            Tallness = Random.Shared.Next(8) + 70;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 15;

            WeaponName = "spears";

            ArmorName = "armor";
        }

        public bool ShieldWall;

        public bool FriendlyDanger;

        public static readonly Identifier ProtectId = new("coc", "phoenix_commander_protect");

        public static readonly Identifier InspireId = new("coc", "phoenix_commander_inspire");

        public static readonly Identifier AttackId = new("coc", "phoenix_commander_attack");

        public static readonly Identifier OrderGrenadierId = new("coc", "phoenix_commander_order_grenadier");

        public static readonly Identifier OrderPyroId = new("coc", "phoenix_commander_order_pyro");

        public static readonly Identifier OrderSapperId = new("coc", "phoenix_commander_order_sapper");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var ordered = TryOrder(context);

            if (ordered != null)
                return ordered;

            if (Health < MaxHealth * 0.4f && !ShieldWall)
                return new CombatAiAction { Ability = ProtectId };

            if (context.Enemies.Count(e => e.Health > 0) > 1 && Random.Shared.Next(3) == 0 && Fatigue + 15 <= MaxFatigue() && !GoingSolo(context))
                return new CombatAiAction { Ability = InspireId };

            return new CombatAiAction { Ability = AttackId };
        }

        public CombatAiAction? TryOrder(CombatContext context)
        {
            foreach (var ally in context.Enemies.Where(e => e.Health > 0))
            {
                if (ally is PhoenixGrenadierEnemy && ally.Fatigue <= 85 && context.Player.LustPercent > 50 && Random.Shared.Next(2) == 1)
                {
                    ((PhoenixGrenadierEnemy)ally).Ordered = true;

                    return new CombatAiAction { Ability = OrderGrenadierId };
                }

                if (ally is PhoenixPyroEnemy && ally.Fatigue <= 85 && context.Player.Health / context.Player.MaxHealth < 0.4f && Random.Shared.Next(2) == 1)
                {
                    ((PhoenixPyroEnemy)ally).Ordered = true;

                    return new CombatAiAction { Ability = OrderPyroId };
                }

                if (ally is PhoenixSapperEnemy && ally.Fatigue <= 85 && FriendlyDanger && Random.Shared.Next(2) == 1)
                {
                    ((PhoenixSapperEnemy)ally).Ordered = true;

                    FriendlyDanger = false;

                    return new CombatAiAction { Ability = OrderSapperId };
                }
            }

            return null;
        }

        public static bool GoingSolo(CombatContext context) =>

            !context.Enemies.Any(e => e != null && e.Health > 0 && e is not PhoenixCommanderEnemy);

        public override bool Defeated(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Hel.HelDungeonScene>.Get().PhoenixPlatoonLosesToPC();

            return true;
        }

        public override bool Won(bool hpVictory)
        {
            Scenes.SceneInstance<Scenes.Vanilla.Hel.HelDungeonScene>.Get().PhoenixPlatoonMurdersPC();

            return true;
        }
    }
}