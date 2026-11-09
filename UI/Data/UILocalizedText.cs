using Hjson;
using MonoCoC.Extensions;
using System;
using System.Collections.Generic;

namespace MonoCoC.UI.Data
{
    /// <summary>
    /// Hjson localized string reference:
    /// { key: "Name", parameter: [] }.
    /// Parameters may be strings, numbers, or nested references. Everything is
    /// resolved once while building the screen, never during Draw/Update.
    /// </summary>
    public sealed class UILocalizedText
    {
        private const int MaxDepth = 8;

        public string Key = "";

        public List<object> Parameters = [];

        public string Resolve() => Resolve(0);

        private string Resolve(int depth)
        {
            if (Key.Length == 0 || depth > MaxDepth)
                return Key;

            var arguments = new object[Parameters.Count];

            for (var index = 0; index < Parameters.Count; index++)
                arguments[index] = Parameters[index] is UILocalizedText nested
                    ? nested.Resolve(depth + 1)
                    : Parameters[index];

            return Key.Localized(arguments);
        }

        public static string Resolve(JsonObject parent, string property)
        {
            if (!parent.TryGetValue(property, out var value) || value is null)
                return "";

            if (value is JsonObject reference)
                return Parse(reference)?.Resolve() ?? "";

            return value.ToString().Trim('"');
        }

        public static UILocalizedText? Parse(JsonObject node)
        {
            var key = node.Str("key");

            if (key.Length == 0)
                return null;

            var result = new UILocalizedText { Key = key };

            var parameters = node.Array("parameter") ?? node.Array("parameters");

            if (parameters is not null)
                foreach (var parameter in parameters)
                {
                    if (parameter is JsonObject nested)
                    {
                        var gameValue = nested.Str("game");

                        if (gameValue.Length > 0)
                        {
                            result.Parameters.Add(ReadGameValue(gameValue));

                            continue;
                        }

                        var child = Parse(nested);

                        if (child is not null)
                            result.Parameters.Add(child);

                        continue;
                    }

                    if (parameter is not null)
                        result.Parameters.Add(parameter.ToString().Trim('"'));
                }

            return result;
        }

        private static string ReadGameValue(string expression)
        {
            var name = expression.StartsWith("GameInfo.", StringComparison.Ordinal)
                ? expression["GameInfo.".Length..]
                : expression;

            var field = typeof(GameInfo).GetField(name);

            if (field is not null)
                return field.GetValue(null)?.ToString() ?? "";

            var property = typeof(GameInfo).GetProperty(name);

            return property?.GetValue(null)?.ToString() ?? "";
        }
    }
}
