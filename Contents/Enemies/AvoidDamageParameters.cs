using MonoCoC.Contents.Creatures;

namespace MonoCoC.Contents.Enemies
{

    public class AvoidDamageParameters
    {
        public bool DoDodge = true;

        public bool DoParry = true;

        public bool DoBlock = true;

        public bool DoCounter;

        public bool DoFatigue;

        public float ToHitChance = float.NaN;

        public Creature? Attacker;

        public Creature? Defender;

        public AvoidDamageParameters Clone() => new()
        {
            DoDodge = DoDodge,

            DoParry = DoParry,

            DoBlock = DoBlock,

            DoCounter = DoCounter,

            DoFatigue = DoFatigue,

            ToHitChance = ToHitChance,

            Attacker = Attacker,

            Defender = Defender
        };

        public AvoidDamageParameters For(Creature attacker, Creature defender)
        {
            var copy = Clone();

            copy.Attacker = attacker;

            copy.Defender = defender;

            return copy;
        }
    }

    public enum AvoidOutcome
    {
        Hit,

        Dodge,

        Parry,

        Block,

        Counter
    }

    public readonly struct AvoidResult
    {

        public string? DodgeReason { get; init; }

        public AvoidOutcome Outcome { get; init; }

        public bool AttackHit => Outcome == AvoidOutcome.Hit;

        public bool AttackFailed => !AttackHit;
    }
}