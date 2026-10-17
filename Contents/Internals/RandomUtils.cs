using System;

namespace MonoCoC.Contents.Internals
{

    public static class RandomUtils
    {
        public static T Choice<T>(params T[] choices) =>

            choices.Length == 0 ? default! : choices[Random.Shared.Next(choices.Length)];

        public static bool Chance(int percent) => Random.Shared.Next(100) < percent;

        public static bool Chance(float percent) => Random.Shared.NextDouble() * 100 < percent;
    }
}