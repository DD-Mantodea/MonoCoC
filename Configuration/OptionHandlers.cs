using System;
using System.Collections.Generic;

namespace MonoCoC.Configuration
{
    /// <summary>
    /// 选项系统用到的三类回调注册表：动作、应用器、候选值来源。
    /// 合并了原来的 OptionActions / OptionAppliers / OptionSources。
    /// </summary>
    public static class OptionHandlers
    {
        private static readonly Dictionary<string, Action> Actions = [];

        private static readonly Dictionary<string, Action<string>> Appliers = [];

        private static readonly Dictionary<string, Func<List<OptionValueDefinition>>> Sources = [];

        public static void RegisterAction(string id, Action action)
        {
            if (!string.IsNullOrEmpty(id))
                Actions[id] = action;
        }

        public static void InvokeAction(string id)
        {
            if (Actions.TryGetValue(id, out var action))
                action();
        }

        public static void RegisterApplier(string id, Action<string> applier)
        {
            if (!string.IsNullOrEmpty(id))
                Appliers[id] = applier;
        }

        public static void InvokeApplier(string id, string data)
        {
            if (Appliers.TryGetValue(id, out var applier))
                applier(data);
        }

        public static void RegisterSource(string id, Func<List<OptionValueDefinition>> source)
        {
            if (!string.IsNullOrEmpty(id))
                Sources[id] = source;
        }

        public static List<OptionValueDefinition> ResolveSource(string id)
        {
            if (string.IsNullOrEmpty(id))
                return [];

            return Sources.TryGetValue(id, out var source) ? source() : [];
        }
    }
}