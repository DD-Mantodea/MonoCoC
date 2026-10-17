using MonoMod.Cil;
using System;
using System.Reflection;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// tML-style hook API. The owning mod is taken from ModContext.Current,
    /// so DynamicMethod delegates do not need assembly-owner inference.
    /// </summary>
    public static class MonoModHooks
    {
        public static void Add(MethodBase target, Delegate hook) => Current().Add(target, hook);

        public static void Modify(MethodBase target, ILContext.Manipulator manipulator) => Current().Modify(target, manipulator);

        public static IDisposable AddFor(ModContext context, MethodBase target, Delegate hook) =>
            context.Hooks.Add(target, hook);

        public static IDisposable ModifyFor(ModContext context, MethodBase target, ILContext.Manipulator manipulator) =>
            context.Hooks.Modify(target, manipulator);

        private static ModHookScope Current() =>
            ModContext.Current?.Hooks
            ?? throw new InvalidOperationException("MonoModHooks must be called while a mod is loading or reloading");
    }
}
