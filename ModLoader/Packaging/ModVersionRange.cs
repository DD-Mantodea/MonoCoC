using System;
using System.Collections.Generic;
using System.Globalization;

namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Simple NuGet-style version range used by mod dependencies.
    /// Supported forms: "1.2.3", "&gt;=1.2.0 &lt;2.0.0", "[1.0,2.0)", "1.2.*".
    /// </summary>
    public sealed class ModVersionRange
    {
        private readonly List<Func<Version, bool>> _constraints = [];

        public string Text { get; }

        private ModVersionRange(string text)
        {
            Text = text;
        }

        public static ModVersionRange Any { get; } = new("*");

        public static bool TryParse(string? text, out ModVersionRange range)
        {
            range = Any;

            if (string.IsNullOrWhiteSpace(text) || text.Trim() == "*")
                return true;

            text = text.Trim();

            var parsed = new ModVersionRange(text);

            if (text[0] is '[' or '(')
                return TryParseInterval(text, parsed);

            foreach (var token in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                if (!TryParseToken(token, parsed))
                    return false;

            range = parsed;
            return true;
        }

        public bool IsSatisfiedBy(Version version)
        {
            var normalized = Normalize(version);

            foreach (var constraint in _constraints)
                if (!constraint(normalized))
                    return false;

            return true;
        }

        private static bool TryParseInterval(string text, ModVersionRange parsed)
        {
            var close = text.IndexOfAny([']', ')']);

            if (close < 0 || close != text.Length - 1)
                return false;

            var body = text[1..close];

            var parts = body.Split(',');

            if (parts.Length != 2)
                return false;

            if (!TryVersion(parts[0].Trim(), out var lower) || !TryVersion(parts[1].Trim(), out var upper))
                return false;

            var includeLower = text[0] == '[';

            var includeUpper = text[^1] == ']';

            parsed._constraints.Add(value => includeLower ? value >= lower : value > lower);

            parsed._constraints.Add(value => includeUpper ? value <= upper : value < upper);

            return true;
        }

        private static bool TryParseToken(string token, ModVersionRange parsed)
        {
            if (token.StartsWith(">=", StringComparison.Ordinal))
                return AddComparison(parsed, token[2..], value => value >= 0, false);

            if (token.StartsWith("<=", StringComparison.Ordinal))
                return AddComparison(parsed, token[2..], value => value <= 0, false);

            if (token.StartsWith(">", StringComparison.Ordinal))
                return AddComparison(parsed, token[1..], value => value > 0, false);

            if (token.StartsWith("<", StringComparison.Ordinal))
                return AddComparison(parsed, token[1..], value => value < 0, false);

            if (token.StartsWith("==", StringComparison.Ordinal))
                return AddComparison(parsed, token[2..], value => value == 0, false);

            if (token.StartsWith("=", StringComparison.Ordinal))
                return AddComparison(parsed, token[1..], value => value == 0, false);

            if (token.EndsWith(".*", StringComparison.Ordinal))
            {
                var prefix = token[..^2];

                if (!TryVersion(prefix, out var lower))
                    return false;

                var parts = prefix.Split('.');

                if (parts.Length > 3)
                    return false;

                var upper = parts.Length switch
                {
                    1 => new Version(int.Parse(parts[0], CultureInfo.InvariantCulture) + 1, 0),
                    2 => new Version(int.Parse(parts[0], CultureInfo.InvariantCulture), int.Parse(parts[1], CultureInfo.InvariantCulture) + 1),
                    _ => new Version(int.Parse(parts[0], CultureInfo.InvariantCulture), int.Parse(parts[1], CultureInfo.InvariantCulture), int.Parse(parts[2], CultureInfo.InvariantCulture) + 1)
                };

                parsed._constraints.Add(value => value >= lower);
                parsed._constraints.Add(value => value < upper);

                return true;
            }

            if (!TryVersion(token, out var exact))
                return false;

            parsed._constraints.Add(value => value == exact);

            return true;
        }

        private static bool AddComparison(ModVersionRange parsed, string text, Func<int, bool> comparison, bool unused)
        {
            if (!TryVersion(text, out var version))
                return false;

            parsed._constraints.Add(value => comparison(value.CompareTo(version)));

            return true;
        }

        private static bool TryVersion(string text, out Version version)
        {
            if (!Version.TryParse(text, out var parsed))
            {
                version = new Version();
                return false;
            }

            version = Normalize(parsed);
            return true;
        }

        private static Version Normalize(Version version) =>
            new(version.Major, Math.Max(version.Minor, 0), Math.Max(version.Build, 0), Math.Max(version.Revision, 0));

        public override string ToString() => Text;
    }
}
