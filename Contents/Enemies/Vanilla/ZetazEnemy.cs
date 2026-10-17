using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<ZetazEnemy>("coc:zetaz")]
    public class ZetazEnemy : Enemy
    {
        public static readonly Identifier PotionId = new("coc", "zetaz_potion");

        public static readonly Identifier GustId = new("coc", "zetaz_gust");

        public static readonly Identifier GigaArouseId = new("coc", "zetaz_giga_arouse");

        public static readonly Identifier AttackId = new("coc", "zetaz_attack");

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(Random.Shared.Next(2) + 11, 2.5f, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 1;

            CumMultiplier = 3;

            HoursSinceCum = 20;

            Tallness = 4 * 12 + 1;

            Body.SetHipsRating("boyish", 1);

            Body.SetButtRating("tight");

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 5;

            WeaponName = "claws";

            WeaponVerb = "claw-slash";

            ArmorName = "leathery skin";

            LustVulnerability = 0.35f;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (Lust > 50 && Health <= MaxHealth * 0.5f)
                return new CombatAiAction { Ability = new Identifier("coc", "zetaz_drain") };

            var choices = new (Identifier Id, int Weight)[]
            {
                (PotionId, 1),

                (GustId, 1),

                (GigaArouseId, 1),

                (AttackId, 1)
            };

            return new CombatAiAction { Ability = choices[Random.Shared.Next(choices.Length)].Id };
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.DefeatedZetaz, 1);

            CoC.Instance.Flags.Set(GameFlagIds.ZetazDoorUnlocked, 1);

            CombatController.Instance?.Context.Append("Dungeons.Zetaz.Victory".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append("Dungeons.Zetaz.Defeat".Localized());

            return false;
        }
    }
}