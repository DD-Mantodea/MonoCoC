using System.Collections.Generic;

namespace MonoCoC.Contents.Stats
{
    public enum DynStatOp
    {
        Add,

        Sub,

        Mul,

        Div,

        Eq
    }

    public readonly record struct DynStat(
        Stat Stat,

        float Amount,

        DynStatOp Op = DynStatOp.Add,

        bool Permanent = true);

    public readonly record struct DynStatsOptions(bool Scale = true, bool EnforceMax = true);

    public class StatTicket(StatManager manager, StatModifier modifier)
    {
        public void Revert() => manager.RemoveModifier(modifier);
    }

    public class DynResult
    {
        public Dictionary<Stat, float> Applied { get; } = [];

        public List<StatTicket> Tickets { get; } = [];

        public void RevertTemporary()
        {
            foreach (var ticket in Tickets)
                ticket.Revert();

            Tickets.Clear();
        }
    }
}