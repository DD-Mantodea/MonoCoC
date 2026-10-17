using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MonoCoC.Scripting
{
    // [if] 里可以直接写 C# 表达式，条件自己带上下文：
    //   [if (CoC.Instance.Player.Corruption > 20) {高腐化|低腐化}]
    //   [if (CoC.Instance.Flags.Get(GameFlagIds.AikoAffection) >= 50) {…|…}]
    //   [if (MyMod.Instance.SomeFlag) {…|…}]        ← mod 的类型，只要它的程序集在同一进程里加载了就能解析
    //
    // 不需要 globals：脚本直接引用 MonoCoC 程序集 + 进程里已加载的其它程序集，
    // 所以 mod 加载完调一下 ConditionEvaluator.Refresh() 就能用自己类型里的条件。
    public static class ConditionEvaluator
    {
        private static readonly Dictionary<string, Script<bool>> Cache = new(StringComparer.Ordinal);

        private static readonly HashSet<string> Broken = new(StringComparer.Ordinal);

        private static readonly object Gate = new();

        private static ScriptOptions? _options;

        private static readonly string[] Imports =

        [
            "System",

            "MonoCoC",

            "MonoCoC.Contents",

            "MonoCoC.Contents.BodyParts.Vanilla",

            "MonoCoC.Contents.Creatures",

            "MonoCoC.Contents.Datas.Vanilla",

            "MonoCoC.Contents.Flags",

            "MonoCoC.Contents.Npcs",

            "MonoCoC.Contents.Enemies",

            "MonoCoC.Contents.Enemies.Vanilla",

            "MonoCoC.Contents.Scenes",

            "MonoCoC.Contents.World",

            "MonoCoC.Registry",

            "MonoCoC.UI.Components.Views",
        ];

        // mod 加载完可以调一次，让新程序集进入引用列表
        public static void Refresh()
        {
            lock (Gate)
            {
                _options = null;

                Cache.Clear();

                Broken.Clear();
            }
        }

        private static ScriptOptions BuildOptions()
        {
            var references = new List<MetadataReference>();

            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void Add(Assembly assembly)
            {
                if (assembly.IsDynamic)
                    return;

                string? location;

                try { location = assembly.Location; }
                catch { return; }

                if (string.IsNullOrEmpty(location) || !File.Exists(location) || !seen.Add(location))
                    return;

                try { references.Add(MetadataReference.CreateFromFile(location)); }
                catch { /* 个别程序集不能当引用 */ }
            }

            // 运行时 / 框架
            var tpa = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES") as string;

            if (!string.IsNullOrEmpty(tpa))
                foreach (var path in tpa.Split(Path.PathSeparator))
                {
                    if (!File.Exists(path) || !seen.Add(path))
                        continue;

                    try { references.Add(MetadataReference.CreateFromFile(path)); }
                    catch { }
                }

            // 本项目 + 进程里加载的其它程序集（含 mod）
            Add(typeof(ConditionEvaluator).Assembly);

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                Add(assembly);

            try { Add(typeof(Microsoft.CSharp.RuntimeBinder.Binder).Assembly); }
            catch { }

            return ScriptOptions.Default.WithReferences(references).WithImports(Imports);
        }

        public static bool TryEvaluate(string expression, out bool value, out string error)
        {
            value = false;

            error = "";

            if (string.IsNullOrWhiteSpace(expression))
                return false;

            lock (Gate)
            {
                if (Broken.Contains(expression))
                {
                    error = "上次编译就失败了";

                    return false;
                }

                _options ??= BuildOptions();

                if (!Cache.TryGetValue(expression, out var script))
                {
                    if (!TryCompile(expression, out script, out error))
                        return false;

                    Cache[expression] = script!;
                }

                try
                {
                    value = script!.RunAsync().GetAwaiter().GetResult().ReturnValue;

                    return true;
                }
                catch (Exception ex)
                {
                    error = ex.Message.Split('\n')[0];

                    return false;
                }
            }
        }

        private static bool TryCompile(string expression, out Script<bool>? script, out string error)
        {
            error = "";

            for (var attempt = 0; attempt < 2; attempt++)
                try
                {
                    script = CSharpScript.Create<bool>(expression, _options!);

                    script.Compile();

                    return true;
                }
                catch (Exception ex)
                {
                    var message = ex.Message.Split('\n')[0];

                    // 可能是「引用的程序集里没这个类型」（比如 mod 刚加载）——刷新引用再试一次
                    if (attempt == 0 && (message.Contains("CS0246") || message.Contains("CS0103") || message.Contains("CS0234")))
                    {
                        Refresh();

                        _options ??= BuildOptions();

                        continue;
                    }

                    script = null;

                    Broken.Add(expression);

                    error = message;

                    return false;
                }

            script = null;

            Broken.Add(expression);

            error = "编译失败";

            return false;
        }
    }
}