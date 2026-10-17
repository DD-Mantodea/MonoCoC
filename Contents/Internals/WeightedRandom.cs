using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Internals
{

    public class WeightedRandom<T>
    {
        public readonly List<(T Value, float Weight)> _choices = [];

        public float _sum;

        public WeightedRandom<T> Add(T choice, float weight = 1)
        {
            _choices.Add((choice, weight));

            _sum += weight;

            return this;
        }

        public WeightedRandom<T> AddMany(float weight, params T[] choices)
        {
            foreach (var choice in choices)
                Add(choice, weight);

            return this;
        }

        public T Choose()
        {
            if (_choices.Count == 0)
                return default!;

            var roll = Random.Shared.NextDouble() * _sum;

            foreach (var (value, weight) in _choices)
            {
                roll -= weight;

                if (roll <= 0)
                    return value;
            }

            return _choices[^1].Value;
        }
    }
}