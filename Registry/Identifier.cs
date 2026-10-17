using System;

namespace MonoCoC.Registry
{
    public readonly struct Identifier : IEquatable<Identifier>
    {
        public const char Separator = ':';

        public Identifier(string space, string path)
        {
            if (string.IsNullOrEmpty(space))
                throw new ArgumentException("Identifier namespace is empty", nameof(space));

            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Identifier path is empty", nameof(path));

            if (space.Contains(Separator) || path.Contains(Separator))
                throw new ArgumentException($"Identifier parts cannot contain '{Separator}': {space}{Separator}{path}");

            Namespace = space;

            Path = path;
        }

        public string Namespace { get; }

        public string Path { get; }

        public static Identifier Parse(string id) => TryParse(id, out var identifier) ? identifier : throw new FormatException($"Invalid identifier '{id}'");

        public static bool TryParse(string id, out Identifier identifier)
        {
            identifier = default;

            if (string.IsNullOrEmpty(id))
                return false;

            var index = id.IndexOf(Separator);

            if (index <= 0 || index == id.Length - 1)
                return false;

            if (id.IndexOf(Separator, index + 1) >= 0)
                return false;

            identifier = new Identifier(id[..index], id[(index + 1)..]);

            return true;
        }

        public static bool IsValid(string id) => TryParse(id, out _);

        public bool Equals(Identifier other) => string.Equals(Namespace, other.Namespace, StringComparison.OrdinalIgnoreCase) && string.Equals(Path, other.Path, StringComparison.OrdinalIgnoreCase);

        public override bool Equals(object obj) => obj is Identifier other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(StringComparer.OrdinalIgnoreCase.GetHashCode(Namespace ?? ""), StringComparer.OrdinalIgnoreCase.GetHashCode(Path ?? ""));

        public override string ToString() => Namespace + Separator + Path;

        public static bool operator ==(Identifier left, Identifier right) => left.Equals(right);

        public static bool operator !=(Identifier left, Identifier right) => !left.Equals(right);

        public static implicit operator string(Identifier id) => id.ToString();

        public static implicit operator Identifier(string? id) =>
            id is null ? default : Parse(id);
    }
}
