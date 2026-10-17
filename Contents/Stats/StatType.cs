using MonoCoC.Extensions;
using MonoCoC.Registry;

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
                var key = $"Stats.{Id.Path}.Name";

                var value = key.Localized();

                return value == key ? Id.Path : value;
            }
        }

        public float Default;

        public bool HigherIsBetter = true;

        public StatFormat Format = StatFormat.Int;

        public bool Visible = true;

        public Stat Stat => new(Id);
    }
}