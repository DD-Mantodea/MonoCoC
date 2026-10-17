using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Scenes
{
    /// <summary>
    /// 场景注册表：**被动发现**。
    ///
    /// 装配时由 <see cref="RegistrationScanner"/> 扫描程序集里所有带 [Register&lt;T&gt;] 的 Scene 子类，
    /// 把 id → 无参工厂登记到 <see cref="Types"/>；运行时注册表直接由这张表构建。
    /// 不再读取任何数据文件来决定「有哪些场景」——数据文件只承载内容（文本 / 数值），不承载身份。
    /// </summary>
    public static class SceneRegistry
    {
        static SceneRegistry() => RegistrationScanner.OnTypes(RegisterTypes);

        /// <summary>id → 工厂（工厂一律是 public 无参构造）。</summary>
        public static readonly Dictionary<string, Func<Scene>> Types = new(StringComparer.OrdinalIgnoreCase);

        private static Registry<Scene>? _scenes;

        public static Registry<Scene> Scenes => _scenes ??= Build();

        /// <summary>供极少数需要手工登记的扩展点（mod 也可用）。</summary>
        public static void RegisterType(string type, Func<Scene> factory) => Types[type] = factory;

        internal static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || !typeof(Scene).IsAssignableFrom(type))
                    continue;

                var attributes = type.GetCustomAttributes(typeof(RegisterAttribute), false);

                if (attributes.Length == 0)
                    continue;

                var factory = CompileFactory(type);

                foreach (RegisterAttribute attribute in attributes)
                {
                    Types[attribute.Id] = factory;

                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 场景工厂：要求 public 无参构造。
        /// 区域场景（AreaScene 子类）的 AreaDefinition 由场景自身通过 AreaIds 常量确定，
        /// 不接受 (AreaDefinition) 构造，也不从数据文件注入。
        /// </summary>
        public static Func<Scene> CompileFactory(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes)

                ?? throw new InvalidOperationException(
                    $"[Register] 的 {type.Name} 缺少 public 无参构造（注册表按无参构造实例化）");

            return () => (Scene)ctor.Invoke(null);
        }

        public static Scene CreateOrDefault(string id) => Scenes.CreateOrDefault(id);

        public static Scene CreateOrDefault(Identifier id) => Scenes.CreateOrDefault(id);

        /// <summary>由被动发现的 <see cref="Types"/> 构建运行时注册表。</summary>
        public static Registry<Scene> Build()
        {
            var registry = new Registry<Scene>(() => throw new InvalidOperationException(
                "场景 id 未注册：代码里没有对应的 [Register<XxxScene>(\"...\")]"));

            foreach (var (id, factory) in Types)
                registry.Register(Identifier.Parse(id), factory);

            return registry;
        }
    }
}