using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Data
{
    /// <summary>
    /// Event handlers used by Hjson event properties such as onClick.
    /// Handlers are owned so a mod can remove only its own events.
    /// </summary>
    public sealed class UIEventRegistry
    {
        private readonly Dictionary<string, List<(string? Owner, Action Action)>> _events = [];

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
