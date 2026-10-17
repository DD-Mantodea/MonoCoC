using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Combat;
using System.Linq;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    // 招式效果见 Contents/Combat/Handlers/PhoenixPyroCombatHandlers.cs（已实现，34 行）
    [Register<PhoenixPyroEnemy>("coc:phoenix_pyro")]
    public class PhoenixPyroEnemy : Enemy
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

        public bool Ordered;

        public static readonly Identifier FireBreathId = new("coc", "phoenix_pyro_phoenix_platoon_fire_breath");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var leader = context.Enemies.OfType<PhoenixCommanderEnemy>().FirstOrDefault(e => e.Health > 0);

            if (leader != null && Health < MaxHealth * 0.5f)
            {
                if (leader.HasStatusEffect(StatusEffectIds.Stunned) || leader.HasStatusEffect(StatusEffectIds.Fear))
                {
                    context.Append("Combat.Hel.PhoenixPyroLeaderless".Localized());

                    return null;
                }

                leader.FriendlyDanger = true;
            }
            else if (leader == null && Random.Shared.Next(4) == 0)
            {
                context.Append("Combat.Hel.PhoenixPyroLeaderless".Localized());

                return null;
            }

            if (Ordered)
            {
                Ordered = false;

                return new CombatAiAction { Ability = FireBreathId };
            }

            return Random.Shared.Next(4) == 0

                ? new CombatAiAction { Ability = FireBreathId }

                : new CombatAiAction { Ability = new Identifier("coc", "phoenix_pyro_attack") };
        }

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