using System;

namespace MonoCoC.ModLoader
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public sealed class NoJitAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public sealed class JitWhenModsEnabledAttribute : Attribute
    {
        public JitWhenModsEnabledAttribute(params string[] modIds)
        {
            ModIds = modIds ?? throw new ArgumentNullException(nameof(modIds));
        }

        public string[] ModIds { get; }
    }

    public sealed class ModJitException : Exception
    {
        public ModJitException(string modId, string member, Exception innerException)
            : base($"{modId}: failed to JIT {member}: {innerException.Message}", innerException)
        {
        }
    }
}
