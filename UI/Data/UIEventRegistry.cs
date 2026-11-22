using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.UI.Data
{
    /// <summary>
    /// Event handlers used by Hjson event properties such as onClick.
    /// Handlers are owned so a mod can remove only its own events.
    /// </summary>
    public class UIEventRegistry
    {
        private readonly Dictionary<string, List<(string? Owner, Action Action)>> _events = [];

        public void RegisterFrom(object source)
        {
            for (var type = source.GetType(); type is not null && type != typeof(object); type = type.BaseType)
                foreach (var method in type.GetMethods(
                             BindingFlags.DeclaredOnly |
                             BindingFlags.Public |
                             BindingFlags.NonPublic |
                             BindingFlags.Static |
                             BindingFlags.Instance))
                    foreach (var attribute in method.GetCustomAttributes<UIEventAttribute>())
                        RegisterEvent(
                            attribute.Id,
                            (Action)method.CreateDelegate(typeof(Action), method.IsStatic ? null : source));
        }

        public void RegisterEvent(string eventId, Action action, string? owner = null)
        {
            if (!_events.TryGetValue(eventId, out var handlers))
                _events[eventId] = handlers = [];

            handlers.Add((owner, action));
        }

        public bool Raise(string eventId)
        {
            if (!_events.TryGetValue(eventId, out var handlers))
                return false;

            foreach (var handler in handlers)
                handler.Action();

            return true;
        }

        public void RemoveAll(string owner)
        {
            foreach (var eventId in _events.Keys)
                _events[eventId].RemoveAll(handler => handler.Owner == owner);
        }
    }
}
