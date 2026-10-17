using MonoCoC.Contents.Creatures;
using MonoCoC.ModLoader;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Pregnancy
{
    // 原版 PregnancyStore.hx 的 C# 等价物：type / incubation / event / eventTriggered。
    // 所有语义照 PregnancyStore.hx:101-224；唯一差异是 C# 用 Identifier 表示 type，
    // 并把「已注意阶段」从 type 的高位拆成 backing.Notice（旧 flag backing 里仍然原位存取）。
    public sealed class PregnancyStore(IPregnancyBacking backing)
    {
        // 每个 father 类型一套事件阈值（降序），对应 addPregnancyEventSet（PregnancyStore.hx:144-146）。
        private readonly Dictionary<Identifier, int[]> _eventSets = [];

        public bool AllowHerm;

        public IPregnancyBacking Backing => backing;

        public Identifier Type => backing.Type;

        public int Incubation => backing.Incubation;

        // PregnancyStore.hx:133-137：birth 时 incubation 可以是 0，所以用 type 判断。
        public bool IsPregnant => !PregnancyLegacyTypes.IsNone(backing.Type);

        public bool IsKnockedUp => backing.Incubation > 0;

        public void AddEventSet(Identifier father, params int[] descendingThresholds) =>
            _eventSets[father] = descendingThresholds;

        // PregnancyStore.hx:149-153：已怀孕时 no-op。
        public void KnockUp(Identifier father, int incubation, bool forceAllowHerm = false)
        {
            if (!IsPregnant)
                KnockUpForce(father, incubation, forceAllowHerm);
        }

        // PregnancyStore.hx:155-166。
        public void KnockUpForce(Identifier father = default, int incubation = 0, bool forceAllowHerm = false)
        {
            var empty = PregnancyLegacyTypes.IsNone(father);

            backing.Type = empty ? PregnancyLegacyTypes.None : father;
            backing.Incubation = empty ? 0 : incubation;

            if (forceAllowHerm || Player?.IsHerm() == true)
                AllowHerm = true;
        }

        // PregnancyStore.hx:169-177：father 默认是玩家；成功率 = boundInt(floor, floor+virilityQ, ceiling) 百分比。
        public bool KnockUpChance(int floor = 20, int ceiling = 60, Creature? father = null)
        {
            if (IsPregnant)
                return false;

            father ??= Player;

            var chance = Math.Clamp(floor + (father?.VirilityQ() ?? 0), floor, ceiling);

            return Random.Shared.NextDouble() * 100 < chance;
        }

        // PregnancyStore.hx:181-186：只在 next >= 0 时写回。
        public void PregnancyAdvance()
        {
            var next = backing.Incubation - 1;

            if (next >= 0)
                backing.Incubation = next;
        }

        // PregnancyStore.hx:191-207：无事件集时返回 1；否则返回「剩余小时跨过的阈值个数 + 1」。
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

        // PregnancyStore.hx:211-224：同一阶段只触发一次；写入当前阶段并返回它。
        public int EventTriggered()
        {
            var stage = Event;

            if (stage == backing.Notice)
                return 0;

            backing.Notice = stage;

            return stage;
        }

        public void Clear() => KnockUpForce();

        private static Player Player => CoC.Instance.Player;
    }
}
