using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Registry
{
    // 统一的注册扫描器（订阅式，自动扫自己的程序集）。
    //
    // 注册表只在静态构造里订阅，不写 Scan：
    //   EnemyRegistry                 → RegistrationScanner.OnTypes(RegisterTypes)
    //   CombatAbilityHandlerRegistry  → RegistrationScanner.OnMethods(RegisterMethod)
    //   CombatConditionRegistry       → RegistrationScanner.OnMethods(RegisterMethod)
    //
    // 第一次有人订阅时，扫描器自己把「它所在的程序集」扫一遍（全进程只扫一次）；
    // 之后订阅的注册表直接从缓存补发，顺序无关。mod 加载后调 Scan(modAssembly) 即可。
    public static class RegistrationScanner
    {
        public const BindingFlags StaticAnywhere =
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        private static readonly object Gate = new();

        private static readonly Dictionary<Assembly, Type[]> TypeCache = [];

        // 只缓存「带 [Register] 的静态方法」，避免重复 GetMethods()
        private static readonly Dictionary<Assembly, List<(MethodInfo Method, object[] Attributes)>> MethodCache = [];

        private static readonly List<Func<Type[], int>> TypeConsumers = [];

        private static readonly List<Func<MethodInfo, object[], bool>> MethodConsumers = [];

        private static bool _ownScanned;

        // 订阅「类级 [Register]」
        public static void OnTypes(Func<Type[], int> consume)
        {
            lock (Gate)
            {
                EnsureOwnScanned();

                TypeConsumers.Add(consume);

                foreach (var types in TypeCache.Values)
                    consume(types);
            }
        }

        // 订阅「方法级 [Register]」
        public static void OnMethods(Func<MethodInfo, object[], bool> consume)
        {
            lock (Gate)
            {
                EnsureOwnScanned();

                MethodConsumers.Add(consume);

                foreach (var methods in MethodCache.Values)
                    foreach (var (method, attributes) in methods)
                        consume(method, attributes);
            }
        }

        // 扫「其它程序集」（mod）。传自己的程序集等于没事发生。
        public static void Scan(Assembly assembly)
        {
            lock (Gate)
            {
                if (assembly == typeof(RegistrationScanner).Assembly)
                {
                    EnsureOwnScanned();

                    return;
                }

                ScanCore(assembly);
            }
        }

        // 第一次订阅时扫自己所在的程序集
        private static void EnsureOwnScanned()
        {
            if (_ownScanned)
                return;

            // 先置位再扫：注册表的静态构造可能在扫描过程中回调 OnTypes/OnMethods
            _ownScanned = true;

            ScanCore(typeof(RegistrationScanner).Assembly);
        }

        private static void ScanCore(Assembly assembly)
        {
            if (TypeCache.ContainsKey(assembly))
                return;

            var types = assembly.GetTypes();
            var methods = new List<(MethodInfo Method, object[] Attributes)>();

            foreach (var type in types)
            {
                if (!type.IsClass)
                    continue;

                foreach (var method in type.GetMethods(StaticAnywhere))
                {
                    var attributes = method.GetCustomAttributes(typeof(Attributes.RegisterAttribute), false);

                    if (attributes.Length > 0)
                        methods.Add((method, attributes));
                }
            }

            // 先放缓存，再通知：消费方在自己的静态构造里回调订阅时能立刻拿到数据
            TypeCache[assembly] = types;
            MethodCache[assembly] = methods;

            foreach (var consume in TypeConsumers.ToArray())
                consume(types);

            foreach (var consume in MethodConsumers.ToArray())
                foreach (var (method, attributes) in methods)
                    consume(method, attributes);
        }
    }
}
