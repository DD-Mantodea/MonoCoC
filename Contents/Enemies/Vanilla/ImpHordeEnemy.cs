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

    [Register<ImpHordeEnemy>("coc:imp_horde")]
    public class ImpHordeEnemy : Enemy
    {
        public static readonly Identifier ClawId = new("coc", "imp_horde_claw");

        public static readonly Identifier BukkakeId = new("coc", "imp_horde_bukkake");

        public static readonly Identifier UberId = new("coc", "imp_horde_uber");

        public bool UberCharged;

        public override void Configure(EnemyDefinition definition)
        {
            base.Configure(definition);

            CreateCock(12, 2, BodyPartTypes.Demon);

            Balls = 2;

            BallSize = 1;

            Tallness = 36;

            Body.Get<SkinPart>().Tone = "red";

            var hair = Body.Get<HairPart>();

            hair.Color = "black";

            hair.Length = 1;

            WeaponName = "fists";

            WeaponVerb = "punches";

            ArmorName = "skin";

            LustVulnerability = 0.5f;
        }

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter)
        {
            if (UberCharged)
                return new CombatAiAction { Ability = UberId };

            if (LustPercent > 50 && Random.Shared.Next(2) == 0)
                return new CombatAiAction { Ability = BukkakeId };

            return Random.Shared.Next(4) < 3

                ? new CombatAiAction { Ability = ClawId }

                : new CombatAiAction { Ability = UberId };
        }

        public override bool Defeated(bool hpVictory)
        {
            CoC.Instance.Flags.Set(GameFlagIds.ZetazImpHordeDefeated, 1);

            CombatController.Instance?.Context.Append("Dungeons.ImpHorde.Victory".Localized());

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append("Dungeons.ImpHorde.Defeat".Localized());

            return false;
        }
    }
}