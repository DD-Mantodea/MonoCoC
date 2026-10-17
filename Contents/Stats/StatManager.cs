using Hjson;
using MonoCoC.Contents.Stats.ModifierSources;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Stats
{
    public class StatManager
    {
        public readonly Dictionary<Stat, float> _base = [];

        public readonly Dictionary<Stat, Func<float>> _providers = [];

        public readonly List<StatModifier> _modifiers = [];

        public IReadOnlyList<StatModifier> Modifiers => _modifiers;

        public float GetBase(Stat stat) => _providers.TryGetValue(stat, out var provider) ? provider() : _base.GetValueOrDefault(stat);

        public void SetBase(Stat stat, float value) => _base[stat] = value;

        public void SetProvider(Stat stat, Func<float> provider) => _providers[stat] = provider;

        public float Addition(Stat stat)
        {
            var sum = 0f;

            foreach (var modifier in _modifiers)
                if (modifier.Stat == stat && modifier.Operation == ModifierOperation.Addition)
                    sum += modifier.Resolve();

            return sum;
        }

        public float Multiplier(Stat stat)
        {
            var sum = 0f;

            foreach (var modifier in _modifiers)
                if (modifier.Stat == stat && modifier.Operation == ModifierOperation.Multiplier)
                    sum += modifier.Resolve();

            return 1f + sum;
        }

        public float Factor(Stat stat)
        {
            var product = 1f;

            foreach (var modifier in _modifiers)
                if (modifier.Stat == stat && modifier.Operation == ModifierOperation.Factor)
                    product *= modifier.Resolve();

            return product;
        }

        public float Get(Stat stat) => (GetBase(stat) + Addition(stat)) * Multiplier(stat);

        public void AddBase(Stat stat, float amount) => SetBase(stat, GetBase(stat) + amount);

        public StatModifier AddTemporaryAddition(Stat stat, float amount, IModifierSource source, bool visible = true)
        {
            var modifier = new StatModifier
            {
                Stat = stat,

                Operation = ModifierOperation.Addition,

                Value = () => amount,

                Source = source,

                Visible = visible
            };

            _modifiers.Add(modifier);

            return modifier;
        }

        public void RemoveModifier(StatModifier modifier) => _modifiers.Remove(modifier);

        public void AddModifier(StatModifier modifier) => _modifiers.Add(modifier);

        public void AddModifier(Stat stat, ModifierOperation operation, Func<float> value, IModifierSource source, bool visible = true) => _modifiers.Add(new StatModifier
        {
            Stat = stat,

            Operation = operation,

            Value = value,

            Source = source,

            Visible = visible
        });

        public void ClearModifiers() => _modifiers.Clear();

        public void RemoveFrom(IModifierSource source) => _modifiers.RemoveAll(modifier => Equals(modifier.Source, source));

        public void RemoveAll<T>() where T : IModifierSource => _modifiers.RemoveAll(modifier => modifier.Source is T);

        public IEnumerable<StatModifier> From(IModifierSource source)
        {
            foreach (var modifier in _modifiers)
                if (Equals(modifier.Source, source))
                    yield return modifier;
        }

        public void Save(JsonObject json)
        {
            foreach (var pair in _base)
                json[pair.Key.ToString()] = pair.Value;
        }

        public void Load(JsonObject json)
        {
            _base.Clear();

            foreach (var pair in json)
                _base[Identifier.Parse(pair.Key)] = (float)pair.Value;
        }
    }
}