using MonoCoC.Extensions;

namespace MonoCoC.Utils
{
    public static class NumberUtils
    {
        public static string Num2Text(int value) =>
            value is >= 0 and <= 20 ? ("Numbers." + value).LocalizedOr(value.ToString()) : value.ToString();

        // 原版 num2Text(number:Float)：Haxe 侧参数常是 Float，这里按整数截断
        public static string Num2Text(float value) => Num2Text((int)value);

        // 原版 Utils.countSetBits()：二进制里 1 的个数
        public static int CountSetBits(int value)
        {
            var count = 0;

            for (var v = value; v != 0; v &= v - 1)
                count++;

            return count;
        }
    }
}
