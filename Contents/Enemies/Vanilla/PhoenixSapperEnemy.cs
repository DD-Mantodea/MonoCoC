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

    // 招式效果见 Contents/Combat/Handlers/PhoenixSapperCombatHandlers.cs（已实现，74 行）
    [Register<PhoenixSapperEnemy>("coc:phoenix_sapper")]
    public class PhoenixSapperEnemy : Enemy
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

        public static readonly Identifier FlashBangId = new("coc", "phoenix_sapper_flash_bang");

        public static readonly Identifier ArmorRendId = new("coc", "phoenix_sapper_armor_rend");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            var leader = context.Enemies.OfType<PhoenixCommanderEnemy>().FirstOrDefault(e => e.Health > 0);

            if (leader != null && Health < MaxHealth * 0.5f)
            {
                if (leader.HasStatusEffect(StatusEffectIds.Stunned) || leader.HasStatusEffect(StatusEffectIds.Fear))
                {
                    context.Append("Combat.Hel.PhoenixSapperLeaderless".Localized());

                    return null;
                }

                leader.FriendlyDanger = true;
            }
            else if (leader == null && Random.Shared.Next(4) == 0)
            {
                context.Append("Combat.Hel.PhoenixSapperLeaderless".Localized());

                return null;
            }

            var blind = context.Player.HasStatusEffect(StatusEffectIds.Blind);

            if (Ordered)
            {
                Ordered = false;

                return new CombatAiAction { Ability = blind ? ArmorRendId : FlashBangId };
            }

            var roll = Random.Shared.Next(5);

            return roll switch
            {
                3 => new CombatAiAction { Ability = FlashBangId },

                4 => new CombatAiAction { Ability = ArmorRendId },

                _ => new CombatAiAction { Ability = new Identifier("coc", "phoenix_sapper_attack") }
            };
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