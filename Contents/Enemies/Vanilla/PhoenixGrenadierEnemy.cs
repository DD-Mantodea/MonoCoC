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

    // 招式效果见 Contents/Combat/Handlers/PhoenixGrenadierCombatHandlers.cs（已实现，22 行）
    [Register<PhoenixGrenadierEnemy>("coc:phoenix_grenadier")]
    public class PhoenixGrenadierEnemy : Enemy
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

        public static readonly Identifier LustbangId = new("coc", "phoenix_grenadier_phoenix_platoon_lustbang");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var leader = context.Enemies.OfType<PhoenixCommanderEnemy>().FirstOrDefault(e => e.Health > 0);

            if (leader != null && Health < MaxHealth * 0.5f)
            {
                if (leader.HasStatusEffect(StatusEffectIds.Stunned) || leader.HasStatusEffect(StatusEffectIds.Fear))
                {
                    context.Append("Combat.Hel.PhoenixGrenadierLeaderless".Localized());

                    return null;
                }

                leader.FriendlyDanger = true;
            }
            else if (leader == null && Random.Shared.Next(4) == 0)
            {
                context.Append("Combat.Hel.PhoenixGrenadierLeaderless".Localized());

                return null;
            }

            if (Ordered)
            {
                Ordered = false;

                return new CombatAiAction { Ability = LustbangId };
            }

            return Random.Shared.Next(4) == 0

                ? new CombatAiAction { Ability = LustbangId }

                : new CombatAiAction { Ability = new Identifier("coc", "phoenix_grenadier_attack") };
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