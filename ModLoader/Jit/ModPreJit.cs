using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Forces load-time JIT so missing weak dependencies are found during mod loading,
    /// not in the middle of gameplay.
    /// </summary>
    public static class ModPreJit
    {
        public static bool Enabled { get; set; } = true;

        private const BindingFlags All = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public static void Prepare(Assembly assembly, string modId, Func<string, bool> isModLoaded)
        {
            if (!Enabled)
                return;

            foreach (var type in assembly.GetTypes())
            {
                if (!ShouldJit(type, isModLoaded))
                    continue;

                if (type.IsGenericTypeDefinition)
                    continue;

                foreach (var method in GetMethods(type, isModLoaded))
                {
                    if (method.IsAbstract || method.ContainsGenericParameters || method.DeclaringType != type)
                        continue;

                    if (!ShouldJit(method, isModLoaded))
                        continue;

                    try
                    {
                        RuntimeHelpers.PrepareMethod(method.MethodHandle);
                    }
                    catch (Exception exception)
                    {
                        throw new ModJitException(modId, $"{type.FullName}.{method.Name}", exception);
                    }
                }
            }
        }

        private static IEnumerable<MethodBase> GetMethods(Type type, Func<string, bool> isModLoaded)
        {
            foreach (var method in type.GetMethods(All))
                if (!method.IsSpecialName)
                    yield return method;

            foreach (var constructor in type.GetConstructors(All))
                yield return constructor;

            foreach (var property in type.GetProperties(All))
                if (ShouldJit(property, isModLoaded))
                    foreach (var accessor in property.GetAccessors())
                        yield return accessor;
        }

        private static bool ShouldJit(Type type, Func<string, bool> isModLoaded) =>
            ShouldJit((MemberInfo)type, isModLoaded) &&
            (type.DeclaringType is null || ShouldJit(type.DeclaringType, isModLoaded));

        private static bool ShouldJit(MemberInfo member, Func<string, bool> isModLoaded) =>
            ShouldJitRaw(member) &&
            member.GetCustomAttributes<JitWhenModsEnabledAttribute>(inherit: true)
                .All(attribute => attribute.ModIds.All(isModLoaded));

        private static bool ShouldJitRaw(MemberInfo member) =>
            member.GetCustomAttribute<NoJitAttribute>(inherit: true) is null;
    }
}
