using MonoCoC.Attributes;
using MonoCoC.Contents.BodyParts.Vanilla;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Datas.Vanilla;
using MonoCoC.Contents.Flags;
using MonoCoC.Extensions;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Enemies.Vanilla
{

    [Register<WormMassEnemy>("coc:worm_mass")]
    public class WormMassEnemy : Enemy
    {
        public static readonly Identifier WormAttackId = new("coc", "worm_mass_worm_attack");

        public static readonly Identifier WormsEnticeId = new("coc", "worm_mass_worms_entice");

        public override CombatAiAction? ChooseCombatAction(CombatContext context, CombatAiInterpreter interpreter) =>

            new() { Ability = context.Random.Next(2) == 0 ? WormAttackId : WormsEnticeId };

        public override bool React(CombatContext context, CombatReactionContext reaction)
        {
            var player = context.Player;

            if (reaction == CombatReactionContext.BeforeAttacked)
            {
                if (context.Random.Next(100) + player.Intelligence / 3f >= 50)
                {
                    var temp = (int)(player.Strength / 5f - context.Random.Next(5));

                    if (temp == 0)
                        temp = 1;

                    context.Append("Combat.WormMass.React.YouStrikeAt".Localized("#ff0000", temp));

                    HealthChange(-temp);

                    if (Health <= 0)
                        return false;
                }
                else
                    context.Append("Combat.WormMass.React.YouAttemptTo".Localized());

                return false;
            }

            if (reaction == CombatReactionContext.BowHit)
            {
                context.Append("Combat.WormMass.React.TheArrowSlips".Localized());

                return false;
            }

            return true;
        }

        public override bool Defeated(bool hpVictory)
        {
            if (hpVictory)
                CoC.Instance.Flags.Set(GameFlagIds.WormsMassKilled, CoC.Instance.Flags.Get(GameFlagIds.WormsMassKilled) + 1);

            return false;
        }

        public override bool Won(bool hpVictory)
        {
            CombatController.Instance?.Context.Append(
                "Combat.WormMass.Won.OvercomeByYour".Localized(hpVictory ? "wounds" : "lust"));

            return false;
        }

        public override void Configure(EnemyDefinition definition)
        {

            var player = CoC.Instance.Player;

            Stats.SetProvider(GameStats.MaxHealth, () => 40 + player.NewGamePlusMod() * 20);

            base.Configure(definition);

            CreateBreastRow(0f, 0f);

            var anal = Body.Get<AnalPart>();

            anal.Looseness = 0;

            anal.Wetness = 0;

            Tallness = 1;

            Body.SetHipsRating("slender");

            Body.SetButtRating("buttless");

            Body.Get<SkinPart>().Tone = "white";

            WeaponName = "worm";

            WeaponVerb = "slap";

            NormalAttackAvoid.DoParry = false;

            NormalAttackAvoid.DoCounter = false;

            ArmorName = "skin";

            LustVulnerability = 0;

        }
    }
}