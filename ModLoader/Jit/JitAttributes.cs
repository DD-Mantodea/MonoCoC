using System;

namespace MonoCoC.ModLoader
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public class NoJitAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, Inherited = true)]
    public class JitWhenModsEnabledAttribute : Attribute
    {
        public JitWhenModsEnabledAttribute(params string[] modIds)
        {
            ModIds = modIds ?? throw new ArgumentNullException(nameof(modIds));
        }

        public string[] ModIds { get; }
    }

    public class ModJitException : Exception
    {
        public ModJitException(string modId, string member, Exception innerException)
            : base($"{modId}: failed to JIT {member}: {innerException.Message}", innerException)
        {
        }
    }
}
