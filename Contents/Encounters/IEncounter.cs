using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.World;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Encounters
{
    public interface IEncounter
    {
        string Name { get; }

        float Chance(Player player);

        bool CanRun(Player player);

        void Execute(EncounterContext context);

        public const float Always = float.PositiveInfinity;

        public static IEncounter Create(
            string name,

            Func<Player, float> chance,

            Func<Player, bool> when,

            Action<EncounterContext> execute) =>

            new DelegateEncounter(name, chance, when, execute);

        public static IEncounter Group(
            string name,

            params IEncounter[] children) =>

            new CompositeEncounter(name, null, children);

        public static IEncounter Complex(
            string name,

            Func<Player, float> chance,

            params IEncounter[] children) =>

            new CompositeEncounter(name, chance, children);

        public static IEncounter Wrap(
            IEncounter encounter,

            Func<Player, float> modifier) =>

            new DelegateEncounter(
                encounter.Name,

                player => encounter.Chance(player) * modifier(player),

                encounter.CanRun,

                encounter.Execute);

        public static IEncounter Disabled(string name, string reason) =>

            new DelegateEncounter(
                "disabled:" + name + "  [" + reason + "]",

                _ => 0f,

                _ => false,

                _ => { });
    }

    public class DelegateEncounter(
        string name,

        Func<Player, float> chance,

        Func<Player, bool> when,

        Action<EncounterContext> execute) : IEncounter
    {
        public string Name => name;

        public virtual float Chance(Player player) => EncounterProbabilityConfig.Apply(name, chance(player));

        public virtual bool CanRun(Player player) => when(player);

        public virtual void Execute(EncounterContext context) => execute(context);
    }

    public class CompositeEncounter(
        string name,

        Func<Player, float>? ownChance,

        IReadOnlyList<IEncounter> children) : IEncounter
    {
        public string Name => name;

        public virtual float Chance(Player player)
        {
            if (ownChance != null)
                return EncounterProbabilityConfig.Apply(name, ownChance(player));

            return children.Sum(child => child.CanRun(player) ? child.Chance(player) : 0);
        }

        public virtual bool CanRun(Player player) => children.Any(child => child.CanRun(player));

        public virtual void Execute(EncounterContext context)
        {
            var eligible = children.Where(child => child.CanRun(context.Player)).ToList();

            if (eligible.Count == 0)
                return;

            var always = eligible.FirstOrDefault(child => child.Chance(context.Player) == IEncounter.Always);

            if (always != null)
            {
                always.Execute(context);

                return;
            }

            var total = eligible.Sum(child => child.Chance(context.Player));

            if (total <= 0)
                return;

            var roll = context.Random.NextDouble() * total;

            foreach (var child in eligible)
            {
                roll -= child.Chance(context.Player);

                if (roll <= 0)
                {
                    child.Execute(context);

                    return;
                }
            }
        }
    }

    public class EncounterContext
    {
        public Player Player = null!;

        public GameFlags Flags = null!;

        public Time Time = null!;

        public Random Random = Random.Shared;

        public Action<string> Write = _ => { };

        public Action ReturnToCamp = () => { };

        public Action<int>? ReturnToCampAfter;

        public Action StartCombat = () => { };
    }
}