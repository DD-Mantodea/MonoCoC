using MonoCoC.Assets;
using MonoCoC.Assets.AssetManagers;
using System;
using System.Linq;

namespace MonoCoC.Extensions
{
    public static class StringExt
    {
        extension(string str)
        {
            // 查表 + 格式化一步完成：Localized(args) 等价于「先翻译再用 args 做 string.Format」。
            // 没有单独的 Format 扩展：一律走 Localized（避免在译文上二次查表）。
            public string Localized(params object[] args) =>
                ManagerInstance<LocalizationManager>.Instance.Localize(str, args);

            public string LocalizedOr(string fallback)
            {
                var value = str.Localized();

                return value == str ? fallback : value;
            }

            public string PascalKey() =>
                string.Concat(str
                    .Split(['_', '-', ' '], StringSplitOptions.RemoveEmptyEntries)
                    .Select(part => char.ToUpperInvariant(part[0]) + part[1..]));
        }
    }
}
