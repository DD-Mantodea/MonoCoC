using Hjson;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Data
{
    /// <summary>
    /// Registered UI component types. Core types use coc: and mods should use
    /// their own namespace, e.g. my_mod:paper_background.
    /// </summary>
    public sealed class UIComponentRegistry
    {
        private readonly Dictionary<string, Func<JsonObject, Component?>> _types =
            new(StringComparer.OrdinalIgnoreCase);

        public void Register(string typeId, Func<JsonObject, Component?> factory)
        {
            if (string.IsNullOrWhiteSpace(typeId))
                throw new ArgumentException("UI component type id is required", nameof(typeId));

            _types[typeId] = factory;
        }

        public bool TryCreate(string typeId, JsonObject node, out Component? component)
        {
            if (_types.TryGetValue(typeId, out var factory))
            {
                component = factory(node);

                return true;
            }

            component = null;

            return false;
        }
    }
}
