using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Pregnancy
{

    public class PregnancyStore(IPregnancyBacking backing)
    {
        public const int IncubationDrider = 400;

        public const int IncubationBee = 48;

        public const int IncubationIsabella = 2160;

        public const int IncubationIzma = 300;

        public readonly Dictionary<Identifier, int[]> _eventSets = [];

        public bool AllowHerm;

        public IPregnancyBacking Backing => backing;

        public Identifier Type => backing.Type;

        public int Incubation => backing.Incubation;

        public bool IsPregnant => !PregnancyLegacyTypes.IsNone(backing.Type);

        public bool IsKnockedUp => backing.Incubation > 0;

        public virtual void AddEventSet(Identifier father, params int[] descendingThresholds) =>

            _eventSets[father] = descendingThresholds;

        public virtual void KnockUp(Identifier father, int incubation, bool forceAllowHerm = false)
        {
            if (!IsPregnant)
                KnockUpForce(father, incubation, forceAllowHerm);
        }

        public virtual void KnockUpForce(Identifier father = default, int incubation = 0, bool forceAllowHerm = false)
        {
            var empty = PregnancyLegacyTypes.IsNone(father);

            backing.Type = empty ? PregnancyLegacyTypes.None : father;

            backing.Incubation = empty ? 0 : incubation;

            if (forceAllowHerm || Player?.IsHerm() == true)
                AllowHerm = true;
        }

        public virtual bool KnockUpChance(int floor = 20, int ceiling = 60, Creature? father = null)
        {
            if (IsPregnant)
                return false;

            father ??= Player;

            var chance = Math.Clamp(floor + (father?.VirilityQ() ?? 0), floor, ceiling);

            return Random.Shared.NextDouble() * 100 < chance;
        }

        public virtual void PregnancyAdvance()
        {
            var next = backing.Incubation - 1;

            if (next >= 0)
                backing.Incubation = next;
        }

        public int Event
        {
            get
            {
                if (!IsPregnant)
                    return 0;

                if (!_eventSets.TryGetValue(backing.Type, out var events) || events.Length == 0)
                    return 1;

                for (var index = 0; index < events.Length; index++)
                    if (backing.Incubation > events[index])
                        return index + 1;

                return events.Length + 1;
            }
        }

        public virtual int EventTriggered()
        {
            var stage = Event;

            if (stage == backing.Notice)
                return 0;

            backing.Notice = stage;

            return stage;
        }

        public virtual void Clear() => KnockUpForce();

        public static Player Player => CoC.Instance.Player;
    }
}