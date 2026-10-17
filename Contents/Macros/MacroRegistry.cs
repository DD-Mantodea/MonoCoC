using MonoCoC.Attributes;
using MonoCoC.Scripting;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Macros
{

    public static class MacroRegistry
    {

        public static readonly Dictionary<string, Entry> ById = new(StringComparer.Ordinal);

        public static readonly List<Entry> Ordered = [];

        public static readonly List<MacroScope> Scopes = [];

        static MacroRegistry() => RegistrationScanner.OnTypes(RegisterTypes);

        public static MacroScope BeginScope(object owner)
        {
            var scope = new MacroScope(owner);

            Scopes.Add(scope);

            return scope;
        }

        internal static void EndScope(MacroScope scope) => Scopes.Remove(scope);

        public static int ScopeDepth => Scopes.Count;

        public static IReadOnlyList<IMacroProvider> All
        {
            get
            {
                var list = new List<IMacroProvider>(Ordered.Count);

                foreach (var entry in Ordered) list.Add(entry.Provider);

                return list;
            }
        }

        internal static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || type.IsInterface)
                    continue;

                if (!typeof(IMacroProvider).IsAssignableFrom(type))
                    continue;

                var attributes = type.GetCustomAttributes(typeof(RegisterAttribute), false);

                if (attributes.Length == 0)
                    continue;

                if (Activator.CreateInstance(type) is not IMacroProvider provider)
                    continue;

                foreach (RegisterAttribute attribute in attributes)
                {

                    if (ById.TryGetValue(attribute.Id, out var existing))
                    {
                        if (!attribute.Replace)
                            throw new InvalidOperationException(
                                $"标签 provider 的 id 重复：'{attribute.Id}'（已有 {existing.Provider.GetType().Name}，" +

                                $"又来 {type.Name}）。需要覆盖请用 [Register(\"{attribute.Id}\", Replace = true)]。");

                        Ordered.Remove(existing);
                    }

                    var entry = new Entry(attribute.Id, provider);

                    ById[attribute.Id] = entry;

                    count++;
                }
            }

            Rebuild();

            return count;
        }

        public static void Rebuild()
        {
            Ordered.Clear();

            foreach (var entry in ById.Values) Ordered.Add(entry);

            Ordered.Sort((a, b) =>
            {
                var byPriority = b.Provider.Priority.CompareTo(a.Provider.Priority);

                return byPriority != 0 ? byPriority : string.CompareOrdinal(a.Id, b.Id);
            });
        }

        public static bool TryResolve(string name, out ScriptValue value)
        {

            for (var i = Scopes.Count - 1; i >= 0; i--)
                if (Scopes[i].TryResolve(name, out value))
                    return true;

            foreach (var entry in Ordered)
                if (entry.Provider.TryResolve(name, out value))
                    return true;

            value = default;

            return false;
        }

        public readonly record struct Entry(string Id, IMacroProvider Provider);
    }
}