using System;

namespace MonoCoC.UI.Data
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class UIRequiredAttribute(string part) : Attribute
    {
        public string Part { get; } = part;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public sealed class UIOptionalAttribute(string part) : Attribute
    {
        public string Part { get; } = part;
    }
}
