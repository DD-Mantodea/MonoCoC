using Hjson;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Data
{
    /// <summary>
    /// Custom widgets are C# components registered under a logical ID.
    /// A Hjson node can use:
    /// { type: "CustomWidget", widget: "mod:my_widget", properties: { ... } }
    /// </summary>
    public sealed class UIWidgetRegistry
    {
        private readonly Dictionary<string, Func<JsonObject, Component>> _widgets = [];

        public void Register(string id, Func<JsonObject, Component> factory)
        {
            _widgets[id] = factory;
        }

        public bool TryCreate(string id, JsonObject node, out Component? component)
        {
            if (_widgets.TryGetValue(id, out var factory))
            {
                component = factory(node);
                return true;
            }

            component = null;
            return false;
        }
    }
}
