using Hjson;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Data
{
    public class UIWidgetRegistry
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
