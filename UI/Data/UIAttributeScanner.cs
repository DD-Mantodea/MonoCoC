using Hjson;
using MonoCoC.UI.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.UI.Data
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class UIEventAttribute(string id) : Attribute
    {
        public string Id { get; } = id;
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class UIComponentAttribute(string id) : Attribute
    {
        public string Id { get; } = id;
    }

    public static class UIAttributeScanner
    {
        public static void RegisterEvents(UIEventRegistry registry, object source)
        {
            foreach (var method in MethodsOf(source))
                foreach (var attribute in method.GetCustomAttributes<UIEventAttribute>())
                    registry.RegisterEvent(attribute.Id, EventFor(method, source));
        }

        public static void RegisterComponents(UICustomComponentRegistry registry, object source)
        {
            foreach (var method in MethodsOf(source))
                foreach (var attribute in method.GetCustomAttributes<UIComponentAttribute>())
                    registry.Register(attribute.Id, FactoryFor(method, source));
        }

        private static IEnumerable<MethodInfo> MethodsOf(object source)
        {
            for (var current = source.GetType(); current is not null && current != typeof(object); current = current.BaseType)
                foreach (var method in current.GetMethods(
                             BindingFlags.DeclaredOnly |
                             BindingFlags.Public |
                             BindingFlags.NonPublic |
                             BindingFlags.Static |
                             BindingFlags.Instance))
                    yield return method;
        }

        private static Action EventFor(MethodInfo method, object source)
        {
            if (method.ReturnType != typeof(void) || method.GetParameters().Length != 0)
                throw new InvalidOperationException(
                    $"UIEvent method '{method.DeclaringType?.Name}.{method.Name}' must return void and take no arguments");

            return (Action)method.CreateDelegate(typeof(Action), method.IsStatic ? null : source);
        }

        private static Func<JsonObject, Component> FactoryFor(MethodInfo method, object source)
        {
            var parameters = method.GetParameters();

            if (method.ReturnType != typeof(Component) || parameters.Length > 1 ||
                (parameters.Length == 1 && parameters[0].ParameterType != typeof(JsonObject)))
                throw new InvalidOperationException(
                    $"UIComponent method '{method.DeclaringType?.Name}.{method.Name}' must return Component and take no arguments or one JsonObject");

            var target = method.IsStatic ? null : source;

            if (parameters.Length == 0)
                return _ => (Component)method.Invoke(target, null)!;

            return properties => (Component)method.Invoke(target, [properties])!;
        }
    }
}
