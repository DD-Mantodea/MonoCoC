using MonoCoC.Contents.Creatures;
using MonoCoC.Extensions;
using MonoCoC.Contents.Stats;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Perks
{
    public record PerkBonus(Stat Stat, ModifierOperation Operation, Func<Creature, float> Value, bool Visible = true);

    public class PerkType : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string Name
        {
            get
            {
                var value = Lookup("Name");

                return value.Length > 0 ? value : Id.Path;
            }
        }

        public string Description
        {
            get
            {
                return Lookup("Desc");
            }
        }

        /// <summary>
        /// 兼容历史本地化命名：优先 Perks.&lt;snake&gt;.*，再尝试 Perks.&lt;PascalCase&gt;.*（旧专长文案用这种写法）。
        /// </summary>
        private string Lookup(string suffix)
        {
            var snake = $"Perks.{Id.Path}.{suffix}";

            var value = snake.Localized();

            if (value != snake)
                return value;

            var pascal = $"Perks.{ToPascal(Id.Path)}.{suffix}";

            value = pascal.Localized();

            return value == pascal ? "" : value;
        }

        private static string ToPascal(string path)
        {
            var parts = path.Split(['_', '-'], StringSplitOptions.RemoveEmptyEntries);

            for (var index = 0; index < parts.Length; index++)
            {
                if (parts[index].Length == 0)
                    continue;

                parts[index] = char.ToUpperInvariant(parts[index][0]) + parts[index][1..];
            }

            return string.Concat(parts);
        }

        public List<PerkBonus> Bonuses = [];
    }
}