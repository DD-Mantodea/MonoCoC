using MonoCoC.Registry;
using System;

namespace MonoCoC.Contents.Stats
{
    public readonly struct Stat : IEquatable<Stat>
    {
        public readonly Identifier Id;

        public Stat(Identifier id) => Id = id;

        public Stat(string id) => Id = Identifier.Parse(id);

        public static implicit operator Stat(Identifier id) => new(id);

        public static implicit operator Stat(string id) => new(id);

        public static implicit operator Identifier(Stat stat) => stat.Id;

        public bool Equals(Stat other) => Id == other.Id;

        public static bool operator ==(Stat left, Stat right) => left.Id == right.Id;

        public static bool operator !=(Stat left, Stat right) => left.Id != right.Id;

        public override bool Equals(object obj) => obj is Stat other && Equals(other);

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Id.ToString();
    }
}