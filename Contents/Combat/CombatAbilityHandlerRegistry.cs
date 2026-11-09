using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Contents.Combat
{

    public static class CombatAbilityHandlerRegistry
    {

        static CombatAbilityHandlerRegistry() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Type[] Signature = [typeof(CombatContext), typeof(CombatAbilityDefinition)];

        public static readonly Dictionary<Identifier, Action<CombatContext, CombatAbilityDefinition>> Handlers = [];

        public static void Register(Identifier id, Action<CombatContext, CombatAbilityDefinition> handler) => Handlers[id] = handler;

        public static bool TryGet(Identifier id, out Action<CombatContext, CombatAbilityDefinition> handler) =>

            Handlers.TryGetValue(id, out handler!);

        public static bool IsMetRegistered => true;

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (!Matches(method))
                return false;

            var handler = (Action<CombatContext, CombatAbilityDefinition>)method.CreateDelegate(typeof(Action<CombatContext, CombatAbilityDefinition>));

            foreach (RegisterAttribute attribute in attributes)
                Register(Identifier.Parse(attribute.Id), handler);

            return true;
        }

        public static bool Matches(MethodInfo method)
        {
            if (method.ReturnType != typeof(void))
                return false;

            var parameters = method.GetParameters();

            return parameters.Length == 2

                && parameters[0].ParameterType == Signature[0]

                && parameters[1].ParameterType == Signature[1];
        }
    }
}