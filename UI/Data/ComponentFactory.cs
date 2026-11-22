using Hjson;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Data
{
    public static class ComponentFactory
    {
        private static readonly Dictionary<string, Func<JsonObject, UIBuildContext, Component?>> Factories =
            new(StringComparer.OrdinalIgnoreCase);

        public static void Register<T>() where T : Component, IFromHjson<T>
        {
            Factories[T.TypeId] = (node, services) => T.FromHjson(node, services);
        }

        public static void Register(string typeId, Func<JsonObject, Component?> factory)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(typeId);

            Factories[typeId] = (node, _) => factory(node);
        }

        public static void Register(string typeId, Func<JsonObject, UIBuildContext, Component?> factory)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(typeId);

            Factories[typeId] = factory;
        }

        public static bool TryCreate(string typeId, JsonObject node, UIBuildContext services, out Component? component)
        {
            if (Factories.TryGetValue(typeId, out var factory))
            {
                component = factory(node, services);

                return true;
            }

            component = null;

            return false;
        }
    }
}
