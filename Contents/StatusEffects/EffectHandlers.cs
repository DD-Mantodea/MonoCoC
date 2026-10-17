using MonoCoC.Attributes;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Contents.StatusEffects
{

    public static class EffectHandlerRegistry
    {
        static EffectHandlerRegistry() => RegistrationScanner.OnMethods(RegisterMethod);

        public static readonly Dictionary<Identifier, Action<CombatContext, Creature, StatusEffect>> Handlers = [];

        public static readonly Type Signature = typeof(Action<CombatContext, Creature, StatusEffect>);

        public static bool Has(Identifier id) => Handlers.ContainsKey(id);

        public static void Invoke(Identifier id, CombatContext context, Creature host, StatusEffect status)
        {
            if (Handlers.TryGetValue(id, out var handler))
                handler(context, host, status);
        }

        internal static bool RegisterMethod(MethodInfo method, object[] attributes)
        {
            if (method.ReturnType != typeof(void))
                return false;

            var parameters = method.GetParameters();

            if (parameters.Length != 3
                || parameters[0].ParameterType != typeof(CombatContext)

                || parameters[1].ParameterType != typeof(Creature)

                || parameters[2].ParameterType != typeof(StatusEffect))

                return false;

            var handler = (Action<CombatContext, Creature, StatusEffect>)method.CreateDelegate(Signature);

            foreach (RegisterAttribute attribute in attributes)
                Handlers[Identifier.Parse(attribute.Id)] = handler;

            return true;
        }
    }

    public static class EffectApplication
    {
        public static StatusEffect Apply(Creature host, Identifier id, float duration = 0, float value1 = 0, float value2 = 0)
        {
            var definition = EffectManager.Get(id);

            var existing = host.StatusEffectByType(id);

            if (existing != null)
                switch (definition?.Stacking ?? EffectStacking.Refresh)
                {
                    case EffectStacking.Ignore:
                        return existing;

                    case EffectStacking.Stack:
                        if (definition == null || definition.MaxStacks <= 0 || existing.Values[0] < definition.MaxStacks)
                            existing.Values[0] += 1;

                        return existing;

                    case EffectStacking.Extend:
                        existing.Duration += duration > 0 ? duration : definition?.Duration ?? 0;

                        return existing;

                    default:
                        if (definition != null && definition.Duration > 0)
                            existing.Duration = definition.Duration;

                        return existing;
                }

            var length = duration > 0 ? duration : definition?.Duration ?? 0;

            return host.CreateStatusEffect(id, value1, value2, duration: length);
        }
    }
}