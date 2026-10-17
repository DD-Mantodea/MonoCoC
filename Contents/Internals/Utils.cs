using System;
using System.Collections.Generic;
using System.Linq;
using MonoCoC.Registry;
using MonoCoC.Contents.Flags;
using MonoCoC.Contents.Scenes;
using MonoCoC.Contents.Internals;

namespace MonoCoC.Utils
{

    public static class Utils
    {
        public static int boundInt(int min, int value, int max) => Math.Clamp(value, min, max);

        public static int RandBetween(float n1, float n2)
        {
            var min = Math.Min(n1, n2);

            var max = Math.Max(n1, n2);

            return (int)((Random.Shared.NextDouble() * (max - min + 1)) + min);
        }

        public static string Num2Text(float number) => CapitalizeFirstLetter(NumToWordsInternal(number));

        private static string NumToWordsInternal(float n, bool ordinal = false)
        {
            var numerals = new (float Value, string Str, string Ordinal)[]
            {
                (1E21f, "sextillion", "sextillionth"),

                (1E18f, "quintillion", "quintillionth"),

                (1E15f, "quadrillion", "quadrillionth"),

                (1E12f, "trillion", "trillionth"),

                (1E09f, "billion", "billionth"),

                (1E06f, "million", "millionth"),

                (1000f, "thousand", "thousandth"),

                (100f, "hundred", "hundredth"),

                (90f, "ninety", "ninetieth"),

                (80f, "eighty", "eightieth"),

                (70f, "seventy", "seventieth"),

                (60f, "sixty", "sixtieth"),

                (50f, "fifty", "fiftieth"),

                (40f, "forty", "fortieth"),

                (30f, "thirty", "thirtieth"),

                (20f, "twenty", "twentieth"),

                (19f, "nineteen", "nineteenth"),

                (18f, "eighteen", "eighteenth"),

                (17f, "seventeen", "seventeenth"),

                (16f, "sixteen", "sixteenth"),

                (15f, "fifteen", "fifteenth"),

                (14f, "fourteen", "fourteenth"),

                (13f, "thirteen", "thirteenth"),

                (12f, "twelve", "twelfth"),

                (11f, "eleven", "eleventh"),

                (10f, "ten", "tenth"),

                (9f, "nine", "ninth"),

                (8f, "eight", "eighth"),

                (7f, "seven", "seventh"),

                (6f, "six", "sixth"),

                (5f, "five", "fifth"),

                (4f, "four", "fourth"),

                (3f, "three", "third"),

                (2f, "two", "second"),

                (1f, "one", "first"),
            };

            n = MathF.Floor(n);

            if (n < 0)
                return "negative " + NumToWordsInternal(-n, ordinal);

            if (n == 0)
                return ordinal ? "zeroth" : "zero";

            var result = "";

            foreach (var (Value, Str, Ordinal) in numerals)
            {
                if (n < Value)
                    continue;

                var standaloneWord = ordinal ? Ordinal : Str;

                if (n < 100)
                {
                    n -= Value;

                    result += n > 0 ? Str + " " : standaloneWord;

                    continue;
                }

                var times = (int)(n / Value);

                n -= Value * times;

                if (n > 0)
                {
                    result += NumToWordsInternal(times) + " " + Str;

                    result += n < 100 ? " and " : ", ";
                }
                else
                    result += NumToWordsInternal(times) + " " + standaloneWord;
            }

            return result;
        }

        public static string NumberOfThings(int n, string name, string? pluralForm = null)
        {
            pluralForm ??= name + "s";

            return n switch
            {
                0 => "no " + pluralForm,

                1 => "one " + name,

                _ => Num2Text(n) + " " + pluralForm,
            };
        }

        public static string FormatStringArray(IReadOnlyList<string> list)
        {
            if (list.Count == 0)
                return "";

            if (list.Count == 1)
                return list[0];

            if (list.Count == 2)
                return list[0] + " and " + list[1];

            return string.Join(", ", list.Take(list.Count - 1)) + ", and " + list[list.Count - 1];
        }

        public static T[] RandChoices<T>(int count, params T[] args)
        {
            if (count > args.Length)
                throw new ArgumentException("randChoices requested a count larger than provided options.");

            if (count < 0)
                throw new ArgumentException("randChoices requested a negative count");

            var target = args.ToList();

            var result = new List<T>();

            if (count == target.Count)
                return target.ToArray();

            while (count > 0)
            {
                var idx = Random.Shared.Next(target.Count);

                result.Add(target[idx]);

                target.RemoveAt(idx);

                count--;
            }

            return result.ToArray();
        }

        public static bool IsBetween(float min, float value, float max) =>

            max > min ? value >= min && value <= max : value >= max && value <= min;

        public static string LowercaseFirstLetter(string value) =>

            string.IsNullOrEmpty(value) ? value : char.ToLowerInvariant(value[0]) + value[1..];

        public static string CapitalizeFirstLetter(string value) =>

            string.IsNullOrEmpty(value) ? value : char.ToUpperInvariant(value[0]) + value[1..];

        public static string AddComma(int value) => value.ToString("N0");

        public static string Num2TextOrdinal(int value)
        {
            var suffix = value % 100 is >= 11 and <= 13 ? "th" : (value % 10) switch
            {
                1 => "st",

                2 => "nd",

                3 => "rd",

                _ => "th"
            };

            return value + suffix;
        }

        public static float boundFloat(float min, float value, float max) => Math.Clamp(value, min, max);

        public static int Rand(int max) => max <= 0 ? 0 : Random.Shared.Next(max);

        public static int rand(int max) => Rand(max);

        public static bool Chance(int percent) => RandomUtils.Chance(percent);

        public static bool randomChance(int percent) => Chance(percent);

        public static T RandChoice<T>(params T[] values) => RandomUtils.Choice(values);

        public static T randomChoice<T>(params T[] values) => RandChoice(values);

        public static int CountSetBits(int value)
        {
            var count = 0;

            while (value != 0)
            {
                value &= value - 1;

                count++;
            }

            return count;
        }

        public static int countSetBits(int value) => CountSetBits(value);

        public static Func<float> Product(params Func<float>[] chances) => () => InnerProduct(chances);

        public static float InnerProduct(Func<float>[] chances)
        {
            var product = 1f;

            foreach (var chanceFun in chances)
            {
                var chance = chanceFun();

                if (chance <= 0f)
                    return 0f;

                product *= chance;
            }

            return product;
        }

        public static Func<float> Not(Func<bool> chance) => () => chance() ? 0f : 1f;

        public static Func<float> All(params Func<float>[] chances) => () => InnerAll(chances);

        public static float InnerAll(Func<float>[] chances)
        {
            foreach (var chanceFun in chances)
                if (chanceFun() <= 0f)
                    return 0f;

            return 1f;
        }

        public static Func<float> None(params Func<float>[] chances) => () => InnerNone(chances);

        public static float InnerNone(Func<float>[] chances)
        {
            foreach (var chanceFun in chances)
                if (chanceFun() > 0f)
                    return 0f;

            return 1f;
        }

        public static Func<float> Any(params Func<float>[] chances) => () => InnerAny(chances);

        public static float InnerAny(Func<float>[] chances)
        {
            foreach (var chanceFun in chances)
                if (chanceFun() > 0f)
                    return 1f;

            return 0f;
        }

        public static Func<float> IfLevelMin(int level, int daysPerLevel = 6, float iftrue = 1f, float iffalse = 0f) =>

            () => CoC.Instance.Player.Level >= level || Scene.View.Time.Days >= level * daysPerLevel ? iftrue : iffalse;

        public static Func<float> LineByLevel(int levelA, int levelB, float valueA, float valueB, bool bound = true,

            float min = float.MinValue, float max = float.MaxValue) =>

            () => Lerp(CoC.Instance.Player.Level, levelA, levelB, valueA, valueB, bound, min, max);

        public static float Lerp(float x, int xA, int xB, float yA, float yB, bool bound = true,

            float min = float.MinValue, float max = float.MaxValue)
        {
            if (bound)
            {
                if (x <= xA)
                    return yA;

                if (x >= xB)
                    return yB;
            }

            if (xA == xB)
                return (yA + yB) / 2f;

            var y = yA + (x - xA) * (yB - yA) / (xB - xA);

            return Math.Min(max, Math.Max(min, y));
        }

        public static (float A, float B, float C) BuildLogScaleABC(float x1, float x2, float x3, float y1, float y3)
        {
            var dy = (y3 - y1) / 2f;

            var c = (x2 * x2 - x1 * x3) / (2 * x2 - x1 - x3);

            var a = dy / (float)Math.Log((x2 - c) / (x1 - c));

            var b = y1 - a * (float)Math.Log(x1 - c);

            return (a, b, c);
        }

        public static float LogScale(float x, (float A, float B, float C) abc, float min = float.MinValue,

            float max = float.MaxValue)
        {
            var y = abc.A * (float)Math.Log(x - abc.C) + abc.B;

            return Math.Min(max, Math.Max(min, y));
        }

        public static Func<float> IfTimeOfDay(int minimum, int maximum, float iftrue = 1f, float iffalse = 0f) =>

            () =>
            {
                var hours = Scene.View.Time.Hours;

                return hours >= minimum && hours < maximum ? iftrue : iffalse;
            };

        public static Func<float> IfNGplusMin(int minimum, float iftrue = 1f, float iffalse = 0f) =>

            () => CoC.Instance.Flags.Get(GameFlagIds.NewGamePlusLevel) >= minimum ? iftrue : iffalse;

        public static Func<float> IfPregnantWith(Identifier pregnancyType, float iftrue = 1f, float iffalse = 0f) =>

            () => CoC.Instance.Player.Pregnancy.Incubation > 1 && CoC.Instance.Player.Pregnancy.Type == pregnancyType

                ? iftrue

                : iffalse;
    }

    public static class CoCSettings
    {
        public static void Error(string message) => System.Diagnostics.Debug.WriteLine($"[CoC] {message}");
    }
}