using System;

namespace MonoCoC.UI.Data
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class UIEventAttribute(string id) : Attribute
    {
        public string Id { get; } = id;
    }
}
