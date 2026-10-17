using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Npcs;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Pregnancy
{
    // 原版 PregnancyStore.hx 的三个字段在 C# 的存储面。
    // 不同母体用不同 backing：
    //   FlagPregnancyBacking   —— 旧存档兼容（直接读写原版 KFLAG 的 int 旗标）
    //   NpcStatePregnancyBacking —— 新 NPC 状态（随 npcStates 存档）
    //   PlayerPregnancyBacking —— 玩家自己的 PregnancyState
    public interface IPregnancyBacking
    {
        Identifier Type { get; set; }

        int Incubation { get; set; }

        // 原版把「已注意到的阶段」塞在 type 旗标高 16 位（PregnancyStore.hx:101-102）。
        int Notice { get; set; }
    }

    // 原版 PREGNANCY_* 是 int；C# 的类型是 Identifier。
    // 数据里的 legacyId 提供双向映射，旧存档里的裸 int 才能无损读写。
    public static class PregnancyLegacyTypes
    {
        public static readonly Identifier None = new("coc", "none");

        private static Dictionary<int, Identifier>? _byNumeric;

        private static Dictionary<Identifier, int>? _byId;

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

        private static void Build()
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

    // 旧存档兼容：直接操作两个 int 旗标，type 低 16 位 = 类型、高 16 位 = notice。
    // 完全复刻 PregnancyStore.hx:101-102、118-131、155-166、211-224 的位布局。
    public sealed class FlagPregnancyBacking(Flag<int> typeFlag, Flag<int> incubationFlag) : IPregnancyBacking
    {
        private const int TypeMask = 0x0000FFFF;

        private const int NoticeMask = 0x7FFF0000;

        private static GameFlags Flags => CoC.Instance.Flags;

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

    // 新 NPC：NpcState 已有 pregnancyType / pregnancyIncubation 存档字段，这里只补 notice。
    public sealed class NpcStatePregnancyBacking(NpcState state) : IPregnancyBacking
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

    // 玩家：PregnancyState 已有 type / incubation / event 存档字段。
    public sealed class PlayerPregnancyBacking(PregnancyState state) : IPregnancyBacking
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
