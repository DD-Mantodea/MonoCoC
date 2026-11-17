using MonoCoC.Contents.Items;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Combat
{
    public sealed class NaturalAttack : Weapon
    {
        public NaturalAttack(
            string id,
            string nameKey,
            float attack,
            string verb,
            IReadOnlyList<string> requiresTags,
            int priority)
        {
            Id = Identifier.Parse(id);

            Name = id[(id.IndexOf(':') + 1)..];

            LongName = Name;

            Attack = attack;

            Verb = verb;

            Tags = ["coc:unarmed", .. requiresTags];

            RequiresTags = requiresTags;

            Priority = priority;

            Accuracy = 1;
        }

        public IReadOnlyList<string> RequiresTags { get; }

        public int Priority { get; }

        public bool Available(IReadOnlySet<string> tags)
        {
            foreach (var tag in RequiresTags)
                if (!tags.Contains(tag))
                    return false;

            return true;
        }
    }

    public static class NaturalAttacks
    {
        private static readonly List<NaturalAttack> Attacks = [];

        public static readonly NaturalAttack Fists =
            new("coc:fists", "Combat.Natural.Fists", 1, "punch", [], 0);

        public static readonly NaturalAttack Claws =
            new("coc:claws", "Combat.Natural.Claws", 5, "slash", ["claws"], 10);

        public static readonly NaturalAttack TailWhip =
            new("coc:tail_whip", "Combat.Natural.TailWhip", 3, "whip", ["tail"], 10);

        static NaturalAttacks()
        {
            Register(Fists);
            Register(Claws);
            Register(TailWhip);
        }

        public static void Register(NaturalAttack attack) => Attacks.Add(attack);

        public static NaturalAttack Resolve(IReadOnlySet<string> tags)
        {
            NaturalAttack best = Fists;

            foreach (var attack in Attacks)
            {
                if (!attack.Available(tags))
                    continue;

                if (attack.Priority > best.Priority)
                    best = attack;
            }

            return best;
        }
    }
}
