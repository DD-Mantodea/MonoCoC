using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Assets
{
    public abstract class ContentManager<T> : AssetManager<T> where T : class, IIdentifiable
    {
        private readonly Dictionary<string, string> _owners = [];

        private Registry<T> _registry;

        public virtual Registry<T> Registry => _registry ??= new();

        public override bool SupportsDataSource => true;

        protected override void Store(string key, T value, string? owner)
        {
            Dict[value.Id.ToString()] = value;

            Registry.Register(value.Id, () => value, owner);

            if (owner is null)
                _owners.Remove(key);
            else
                _owners[key] = owner;

            Store(key, value);
        }

        public override void RemoveModData(string owner)
        {
            foreach (var key in new List<string>(_owners.Keys))
            {
                if (_owners[key] != owner)
                    continue;

                _owners.Remove(key);

                Dict.Remove(key);
            }

            Registry.RemoveAll(owner);
        }
    }
}
