using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Contents.Combat
{

    public static class DamageTypeHandlerRegistry
    {
        static DamageTypeHandlerRegistry() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, Action<CombatContext, float>> Handlers = [];

        public static readonly Type Signature = typeof(Action<CombatContext, float>);

        public static void Invoke(Identifier id, CombatContext context, float damage)
        {
            if (id.Path != null && Handlers.TryGetValue(id, out var handler))
                handler(context, damage);
        }

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(void))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 2 || parameters[0].ParameterType != typeof(CombatContext) || parameters[1].ParameterType != typeof(float))
                return false;

            var handler = (Action<CombatContext, float>)method.CreateDelegate(Signature);

            foreach (RegisterAttribute attribute in attributes)
                Handlers[Identifier.Parse(attribute.Id)] = handler;

            return true;
        }
    }
}