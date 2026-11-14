using MonoCoC.Extensions;
using System;
using System.Text;

namespace MonoCoC.UI.Components
{
    public static class ChoiceTextResolver
    {
        public static string Resolve(string text, string age, string gender)
        {
            return Resolve(text, age, gender, null);
        }

        private static string Resolve(string text, string age, string gender, Func<string, bool?>? conditionOverride)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var result = new StringBuilder();

            for (var i = 0; i < text.Length;)
            {
                if (text[i] == '[' && i + 4 < text.Length && text.Substring(i, 4) == "[if ")
                {
                    var openParen = text.IndexOf('(', i);
                    var closeParen = text.IndexOf(')', openParen + 1);

                    if (openParen < 0 || closeParen < 0)
                    {
                        result.Append(text[i]);
                        i++;
                        continue;
                    }

                    var condition = text[(openParen + 1)..closeParen].Trim();

                    var openBrace = text.IndexOf('{', closeParen);
                    var closeBrace = FindMatchingBrace(text, openBrace);

                    if (openBrace < 0 || closeBrace < 0)
                    {
                        result.Append(text[i]);
                        i++;
                        continue;
                    }

                    var body = text[(openBrace + 1)..closeBrace];
                    var separator = body.IndexOf('|');

                    var value = conditionOverride?.Invoke(condition) ?? Evaluate(condition, age, gender);

                    var chosen = value
                        ? separator >= 0 ? body[..separator] : body
                        : separator >= 0 ? body[(separator + 1)..] : "";

                    result.Append(Resolve(chosen, age, gender, conditionOverride));

                    i = closeBrace + 1;

                    if (i < text.Length && text[i] == ']')
                        i++;

                    continue;
                }

                result.Append(text[i]);
                i++;
            }

            return result.ToString();
        }

        public static string Explain(string text, string age, string gender)
        {
            if (string.IsNullOrEmpty(text) || !text.Contains("[if ", StringComparison.Ordinal))
                return text;

            string[] conditions = ["ischild", "isteen", "isteenager", "isadult", "isolder", "iselder", "ismale", "isfemale", "isherm"];

            foreach (var condition in conditions)
            {
                if (!text.Contains(condition, StringComparison.OrdinalIgnoreCase))
                    continue;

                var trueText = Resolve(text, AgeFor(condition, true), GenderFor(condition, true), name => ConditionValue(name, condition, true));
                var falseText = Resolve(text, AgeFor(condition, false), GenderFor(condition, false), name => ConditionValue(name, condition, false));

                var label = ("Views.CharCreation.Condition." + condition).LocalizedOr(condition);

                var prefixLength = 0;

                while (prefixLength < trueText.Length &&
                       prefixLength < falseText.Length &&
                       trueText[prefixLength] == falseText[prefixLength])
                {
                    prefixLength++;
                }

                var suffixLength = 0;

                while (suffixLength < trueText.Length - prefixLength &&
                       suffixLength < falseText.Length - prefixLength &&
                       trueText[^(suffixLength + 1)] == falseText[^(suffixLength + 1)])
                {
                    suffixLength++;
                }

                var prefix = trueText[..prefixLength].TrimEnd();
                var suffix = trueText[^suffixLength..];

                var trueMiddle = trueText[prefixLength..^suffixLength];
                var falseMiddle = falseText[prefixLength..^suffixLength];

                if (prefix.EndsWith("(+", StringComparison.Ordinal))
                {
                    prefix = prefix[..^2].TrimEnd();
                    trueMiddle = "+" + trueMiddle;
                    falseMiddle = "+" + falseMiddle;
                }

                if (suffix.EndsWith(")", StringComparison.Ordinal))
                    suffix = suffix[..^1];

                trueMiddle += suffix;
                falseMiddle += suffix;

                return $"{prefix}\nIf {label}: {trueMiddle}\nOtherwise: {falseMiddle}";
            }

            return text;
        }

        private static bool? ConditionValue(string name, string target, bool value) =>
            name.Equals(target, StringComparison.OrdinalIgnoreCase) ? value : null;

        private static string AgeFor(string condition, bool trueValue) => condition.ToLowerInvariant() switch
        {
            "ischild" => trueValue ? "Child" : "Adult",
            "isteen" or "isteenager" => trueValue ? "Teen" : "Adult",
            "isadult" => trueValue ? "Adult" : "Child",
            "isolder" or "iselder" => trueValue ? "Elder" : "Adult",
            _ => "Adult"
        };

        private static string GenderFor(string condition, bool trueValue) => condition.ToLowerInvariant() switch
        {
            "ismale" => trueValue ? "Male" : "Female",
            "isfemale" => trueValue ? "Female" : "Male",
            "isherm" => trueValue ? "Hermaphrodite" : "Female",
            _ => "Female"
        };

        private static int FindMatchingBrace(string text, int open)
        {
            if (open < 0)
                return -1;

            var depth = 0;

            for (var i = open; i < text.Length; i++)
            {
                if (text[i] == '{')
                    depth++;
                else if (text[i] == '}' && --depth == 0)
                    return i;
            }

            return -1;
        }

        private static bool Evaluate(string condition, string age, string gender)
        {
            condition = condition.Trim().ToLowerInvariant();

            return condition switch
            {
                "ischild" => age.Equals("Child", StringComparison.OrdinalIgnoreCase),
                "isteen" or "isteenager" => age.Equals("Teen", StringComparison.OrdinalIgnoreCase) || age.Equals("Teenager", StringComparison.OrdinalIgnoreCase),
                "isadult" => age.Equals("Adult", StringComparison.OrdinalIgnoreCase),
                "isolder" or "iselder" => age.Equals("Elder", StringComparison.OrdinalIgnoreCase),
                "ismale" => gender.Equals("Male", StringComparison.OrdinalIgnoreCase),
                "isfemale" => gender.Equals("Female", StringComparison.OrdinalIgnoreCase),
                "isandrogynous" or "isherm" => gender.Equals("Hermaphrodite", StringComparison.OrdinalIgnoreCase),
                _ => false
            };
        }
    }
}
