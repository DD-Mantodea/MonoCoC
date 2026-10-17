using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.Registry;

namespace MonoCoC.Contents.World.Areas
{
    public enum AreaConditionKind
    {
        Always,

        Flag,

        Compare,

        Code
    }

    public enum AreaComparison
    {
        Equal,

        NotEqual,

        Greater,

        GreaterOrEqual,

        Less,

        LessOrEqual
    }

    public class AreaCondition
    {
        public AreaConditionKind Kind;

        public Flag<bool> BoolFlag;

        public Flag<int> IntFlag;

        public AreaComparison Comparison;

        public int Value;

        public Flag<int> UnlockFlag;

        public static AreaCondition Always() => new()
        {
            Kind = AreaConditionKind.Always
        };

        public static AreaCondition Flag(Flag<bool> flag) => new()
        {
            Kind = AreaConditionKind.Flag,

            BoolFlag = flag
        };

        public static AreaCondition Compare(Flag<int> flag, AreaComparison comparison, int value) => new()
        {
            Kind = AreaConditionKind.Compare,

            IntFlag = flag,

            Comparison = comparison,

            Value = value
        };

        public static AreaCondition Code(Identifier unlockFlag) => new()
        {
            Kind = AreaConditionKind.Code,

            UnlockFlag = new Flag<int>(unlockFlag)
        };

        public bool IsSatisfied(Player player)
        {
            if (player == null)
                return false;

            var flags = CoC.Instance.Flags;

            return Kind switch
            {
                AreaConditionKind.Always => true,

                AreaConditionKind.Flag => flags.Get(BoolFlag),

                AreaConditionKind.Compare => CompareValue(flags.Get(IntFlag)),

                AreaConditionKind.Code => flags.Get(UnlockFlag) > 0,

                _ => false
            };
        }

        public bool CompareValue(int current) => Comparison switch
        {
            AreaComparison.Equal => current == Value,

            AreaComparison.NotEqual => current != Value,

            AreaComparison.Greater => current > Value,

            AreaComparison.GreaterOrEqual => current >= Value,

            AreaComparison.Less => current < Value,

            AreaComparison.LessOrEqual => current <= Value,

            _ => false
        };
    }
}