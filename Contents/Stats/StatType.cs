using MonoCoC.Extensions;
using MonoCoC.Registry;
using System.Linq;

namespace MonoCoC.Contents.Stats
{
    public enum StatFormat
    {
        Int,

        Decimal,

        Percent
    }

    public class StatType : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string Name
        {
            get
            {
                var pascal = string.Concat(Id.Path.Split('_')
                    .Where(part => part.Length > 0)
                    .Select(part => char.ToUpperInvariant(part[0]) + part[1..]));

                var key = $"Stats.{pascal}.Name";

                var value = key.Localized();

                return value == key ? Id.Path : value;
            }
        }

        public float Default;

        public bool HigherIsBetter = true;

        public StatFormat Format = StatFormat.Int;

        public bool Visible = true;

        public string Group = "";

        public int Order;

        public Stat Stat => new(Id);
    }
}