using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Contents.Dungeons
{

    public static class DungeonConditionRegistry
    {
        static DungeonConditionRegistry() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, Func<DungeonContext, bool>> Handlers = [];

        public static readonly Type Signature = typeof(Func<DungeonContext, bool>);

        public static bool Evaluate(Identifier id, DungeonContext context) =>

            Handlers.TryGetValue(id, out var handler) && handler(context);

        public static bool Has(Identifier id) => Handlers.ContainsKey(id);

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(bool))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 1 || parameters[0].ParameterType != typeof(DungeonContext))
                return false;

            var handler = (Func<DungeonContext, bool>)method.CreateDelegate(Signature);

            foreach (RegisterAttribute attribute in attributes)
                Handlers[Identifier.Parse(attribute.Id)] = handler;

            return true;
        }
    }
}