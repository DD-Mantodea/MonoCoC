using MonoCoC.Contents.Stats.ModifierSources;
using System;

namespace MonoCoC.Contents.Stats
{
    public class StatModifier
    {
        public Stat Stat;

        public ModifierOperation Operation;

        public Func<float> Value;

        public IModifierSource Source;

        public bool Visible = true;

        public float Resolve() => Value?.Invoke() ?? 0f;

        public static StatModifier Add(Stat stat, Func<float> value, IModifierSource source, bool visible = true) => new()
        {
            Stat = stat,

            Operation = ModifierOperation.Addition,

            Value = value,

            Source = source,

            Visible = visible
        };

        public static StatModifier Multiply(Stat stat, Func<float> value, IModifierSource source, bool visible = true) => new()
        {
            Stat = stat,

            Operation = ModifierOperation.Multiplier,

            Value = value,

            Source = source,

            Visible = visible
        };
    }

    public enum ModifierOperation
    {
        Addition,

        Multiplier,

        Factor
    }
}