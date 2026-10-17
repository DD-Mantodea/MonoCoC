using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Registry
{
    public class Registry<T> where T : IIdentifiable
    {
        private readonly Dictionary<Identifier, Func<T>> _factories = [];

        private readonly Dictionary<Identifier, string> _owners = [];

        private readonly Func<T> _factory;

        private Identifier _defaultId;

        public Registry(Func<T> factory = null) => _factory = factory;

        public IReadOnlyCollection<Identifier> Ids => _factories.Keys;

        public Identifier DefaultId => _defaultId;

        public Registry<T> Register(Identifier id, Func<T> factory = null, string? owner = null)
        {
            _factories[id] = factory;

            if (owner is null)
                _owners.Remove(id);
            else
                _owners[id] = owner;

            return this;
        }

        public int RemoveAll(string owner)
        {
            var removed = 0;

            foreach (var id in _owners.Where(pair => pair.Value == owner).Select(pair => pair.Key).ToArray())
            {
                _factories.Remove(id);

                _owners.Remove(id);

                removed++;
            }

            if (removed > 0 && (_defaultId.Namespace.Length == 0 || !_factories.ContainsKey(_defaultId)))
                _defaultId = default;

            return removed;
        }

        public Registry<T> SetDefault(Identifier id)
        {
            _defaultId = id;

            return this;
        }

        public bool Has(Identifier id) => _factories.ContainsKey(id);

        public T Get(Identifier id) => Get(id, out _);

        public T Get(string id) => Get(Identifier.Parse(id), out _);

        public T Get(Identifier id, out Identifier resolved)
        {
            if (!_factories.ContainsKey(id))
                throw new KeyNotFoundException($"Unregistered id '{id}'");

            resolved = id;

            return Build(id);
        }

        public bool TryGet(Identifier id, out T value)
        {
            if (!_factories.ContainsKey(id))
            {
                value = default;

                return false;
            }

            value = Build(id);

            return true;
        }

        public T CreateOrDefault(string id) => CreateOrDefault(Identifier.Parse(id), out _);

        public T CreateOrDefault(Identifier id) => CreateOrDefault(id, out _);

        public T CreateOrDefault(Identifier id, out Identifier resolved)
        {
            if (!_factories.ContainsKey(id))
            {
                if (string.IsNullOrEmpty(_defaultId.Namespace))
                    throw new InvalidOperationException($"Registry has no default entry and '{id}' is not registered");

                id = _defaultId;
            }

            resolved = id;

            return Build(id);
        }

        private T Build(Identifier id)
        {
            _factories.TryGetValue(id, out var custom);

            var value = custom != null ? custom() : _factory != null ? _factory() : throw new InvalidOperationException($"No factory registered for '{id}'");

            value.Id = id;

            return value;
        }
    }
}
