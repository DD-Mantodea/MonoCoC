using MonoCoC.Attributes;
using MonoCoC.Extensions;
using MonoCoC.Contents.Enemies;

namespace MonoCoC.Contents.Combat.Handlers
{

    public static class GnollCombatHandlers
    {
        public static readonly string[] AttackTexts =

        [
            "Combat.Gnoll.Attack.JawsUpperArm",

            "Combat.Gnoll.Attack.GutBarge",

            "Combat.Gnoll.Attack.SandAndClub",

            "Combat.Gnoll.Attack.TeethScratch",

            "Combat.Gnoll.Attack.ClubTemple",

            "Combat.Gnoll.Attack.FootKick"
        ];

        public static readonly string[] TeaseTexts =

        [
            "Combat.Gnoll.Tease.Stretch",

            "Combat.Gnoll.Tease.ClitStroke",

            "Combat.Gnoll.Tease.BodySlide",

            "Combat.Gnoll.Tease.MuskWave",

            "Combat.Gnoll.Tease.Taunt",

            "Combat.Gnoll.Tease.HipShake"
        ];

        public static readonly float[] AttackBonus = [10f, 3f, 13f, 8f, 25f, 0f];

        public static readonly float[] TeaseBonus = [5f, 5f, 10f, 12f, 0f, 6f];

        [Register("coc:gnoll_attack")]
        public static void Attack(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var avoid = CombatCalculator.Avoid(context, new AvoidDamageParameters
            {
                Attacker = context.Enemy,

                Defender = player,

                DoDodge = true,

                DoParry = true,

                DoBlock = true,

                DoFatigue = true
            });

            if (avoid.Outcome != AvoidOutcome.Hit)
            {
                context.WriteAction("Combat.Gnoll.Attack.Dodged".Localized());

                return;
            }

            var roll = context.Random.Next(6);

            context.WriteAction(AttackTexts[roll].Localized());

            var damage = AttackBonus[roll] + context.Enemy.Strength + context.Enemy.WeaponAttack;

            damage = CombatCalculator.ReduceDamage(context, player, damage);

            if (damage <= 0)
            {
                context.WriteAction("Combat.Gnoll.Attack.Blocked".Localized());

                return;
            }

            player.HealthChange(-damage);
        }

        [Register("coc:gnoll_tease")]
        public static void Tease(CombatContext context, CombatAbilityDefinition ability)
        {
            var player = context.Player;

            var roll = context.Random.Next(6);

            context.WriteAction(TeaseTexts[roll].Localized());

            var lust = TeaseBonus[roll] + 10f + player.Libido / 20f + context.Random.Next((int)(player.Corruption / 20));

            player.TakeLustDamage(lust);
        }
    }
}