using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Npcs;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Pregnancy
{

    public interface IPregnancyBacking
    {
        Identifier Type { get; set; }

        int Incubation { get; set; }

        int Notice { get; set; }
    }

    public static class PregnancyLegacyTypes
    {
        public static readonly Identifier None = new("coc", "none");

        public static Dictionary<int, Identifier>? _byNumeric;

        public static Dictionary<Identifier, int>? _byId;

        public static bool IsNone(Identifier id) => id.Path is null or "none";

        public static Identifier FromNumeric(int numeric)
        {
            if (numeric == 0)
                return None;

            Build();

            return _byNumeric!.TryGetValue(numeric, out var id) ? id : new Identifier("coc", $"legacy_{numeric}");
        }

        public static int ToNumeric(Identifier id)
        {
            if (IsNone(id))
                return 0;

            Build();

            return _byId!.TryGetValue(id, out var numeric) ? numeric : 0;
        }

        public static void Build()
        {
            if (_byNumeric != null)
                return;

            _byNumeric = [];

            _byId = [];

            foreach (var id in PregnancyTypeRegistry.Types.Ids)
            {
                var type = PregnancyTypeRegistry.Get(id);

                if (type == null || type.LegacyId == 0)
                    continue;

                _byNumeric[type.LegacyId] = type.Id;

                _byId[type.Id] = type.LegacyId;
            }
        }
    }

    public class FlagPregnancyBacking(Flag<int> typeFlag, Flag<int> incubationFlag) : IPregnancyBacking
    {
        public const int TypeMask = 0x0000FFFF;

        public const int NoticeMask = 0x7FFF0000;

        public static GameFlags Flags => CoC.Instance.Flags;

        public Identifier Type
        {
            get => PregnancyLegacyTypes.FromNumeric(Flags.Get(typeFlag) & TypeMask);

            set
            {
                var raw = Flags.Get(typeFlag);

                var notice = raw & NoticeMask;

                var numeric = PregnancyLegacyTypes.ToNumeric(value) & TypeMask;

                Flags.Set(typeFlag, notice | numeric);

                if (numeric == 0)
                    Flags.Set(incubationFlag, 0);
            }
        }

        public int Incubation
        {
            get => Flags.Get(incubationFlag);

            set => Flags.Set(incubationFlag, value);
        }

        public int Notice
        {
            get => (Flags.Get(typeFlag) & NoticeMask) >> 16;

            set
            {
                var raw = Flags.Get(typeFlag);

                var type = raw & TypeMask;

                Flags.Set(typeFlag, type | ((value & 0x7FFF) << 16));
            }
        }
    }

    public class NpcStatePregnancyBacking(NpcState state) : IPregnancyBacking
    {
        public Identifier Type
        {
            get => state.PregnancyType.Path is null ? PregnancyLegacyTypes.None : state.PregnancyType;

            set
            {
                state.PregnancyType = PregnancyLegacyTypes.IsNone(value) ? default : value;

                if (PregnancyLegacyTypes.IsNone(value))
                    state.PregnancyIncubation = 0;
            }
        }

        public int Incubation
        {
            get => (int)Math.Round(state.PregnancyIncubation);

            set => state.PregnancyIncubation = value;
        }

        public int Notice
        {
            get => state.PregnancyNotice;

            set => state.PregnancyNotice = value;
        }
    }

    public class NpcStateButtPregnancyBacking(NpcState state) : IPregnancyBacking
    {
        public Identifier Type
        {
            get => state.ButtPregnancyType.Path is null ? PregnancyLegacyTypes.None : state.ButtPregnancyType;

            set
            {
                state.ButtPregnancyType = PregnancyLegacyTypes.IsNone(value) ? default : value;

                if (PregnancyLegacyTypes.IsNone(value))
                    state.ButtPregnancyIncubation = 0;
            }
        }

        public int Incubation
        {
            get => (int)Math.Round(state.ButtPregnancyIncubation);

            set => state.ButtPregnancyIncubation = value;
        }

        public int Notice
        {
            get => state.ButtPregnancyNotice;

            set => state.ButtPregnancyNotice = value;
        }
    }

    public class PlayerPregnancyBacking(PregnancyState state) : IPregnancyBacking
    {
        public Identifier Type
        {
            get => state.Type.Path is null ? PregnancyLegacyTypes.None : state.Type;

            set
            {
                state.Type = PregnancyLegacyTypes.IsNone(value) ? PregnancyLegacyTypes.None : value;

                if (PregnancyLegacyTypes.IsNone(value))
                    state.Incubation = 0;
            }
        }

        public int Incubation
        {
            get => state.Incubation;

            set => state.Incubation = value;
        }

        public int Notice
        {
            get => state.Notice;

            set => state.Notice = value;
        }
    }
}