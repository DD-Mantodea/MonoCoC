using MonoCoC.Contents.Flags;
using MonoCoC.Registry;
using System.Collections.Generic;

namespace MonoCoC.Contents.Codex
{

    public class CodexEntry : IIdentifiable
    {
        public Identifier Id { get; set; }

        public string Category = "";

        public string NameKey = "";

        /// <summary>
        /// 原版 Codex.hx 的每个 codexEntryXxx() 会连续输出多行（性别 / 身高 / 皮肤……）。
        /// C# 侧把这些行拆成同前缀的多个本地化键，按本列表顺序渲染。
        /// </summary>
        public List<string> TextKeys = [];

        /// <summary>单键条目用（TextKeys 为空时的回退）。</summary>
        public string TextKey = "";

        public string UnlockFlag = "";

        public Flag<int>? Unlock;

        public int Order;
    }
}