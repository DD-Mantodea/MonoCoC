using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Contents.Dungeons
{

    public static class DungeonRoomRegistry
    {
        static DungeonRoomRegistry() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, Action<DungeonContext>> Handlers = [];

        public static readonly Type Signature = typeof(Action<DungeonContext>);

        public static bool Has(Identifier id) => Handlers.ContainsKey(id);

        public static void Invoke(Identifier id, DungeonContext context)
        {
            if (Handlers.TryGetValue(id, out var handler))
                handler(context);
        }

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(void))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(DungeonContext))
                return false;

            var handler = (Action<DungeonContext>)method.CreateDelegate(Signature);

            foreach (RegisterAttribute attribute in attributes)
                Handlers[Identifier.Parse(attribute.Id)] = handler;

            return true;
        }
    }
}