using MonoCoC.Contents.Creatures;
using MonoCoC.ModLoader;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoCoC.Contents.Pregnancy
{
    // 一次妊娠结算的上下文：对应原版 VaginalPregnancy/AnalPregnancy 里
    // 「game.player + game.monster + output」三样东西。
    public sealed class PregnancyContext
    {
        public Player Player = null!;

        public PregnancyState State = null!;

        public PregnancyType? Definition;

        public bool Anal;

        public Action<string> Write = _ => { };

        public Identifier Father => State.Type;

        public int Incubation => State.Incubation;
    }

    // 原版 VaginalPregnancy / AnalPregnancy 两个接口合并成一个：
    //   Update  = updateVaginalPregnancy / updateAnalPregnancy
    //   Deliver = vaginalBirth / analBirth
    public interface IPregnancyDelivery
    {
        bool Update(PregnancyContext context);

        void Deliver(PregnancyContext context);
    }

    // 没有注册分娩场景时的兜底：不输出、不清状态以外的东西，保持旧行为（到点静默清空）。
    public sealed class NativePregnancyDelivery : IPregnancyDelivery
    {
        public bool Update(PregnancyContext context) => false;

        public void Deliver(PregnancyContext context)
        {
        }
    }

    // 分娩场景注册表：类级 [Register("coc:delivery_xxx")]，照 SceneRegistry / EnemyRegistry 的扫描风格。
    // 数据里的 deliveries.* 指向这些 id；懒实例化后缓存（对应 Haxe 启动时 new PlayerXxxPregnancy）。
    public static class PregnancyDeliveryRegistry
    {
        static PregnancyDeliveryRegistry() => RegistrationScanner.OnTypes(RegisterTypes);

        private static readonly Dictionary<Identifier, Func<IPregnancyDelivery>> Factories = [];

        private static readonly Dictionary<Identifier, IPregnancyDelivery> Instances = [];

        public static readonly IPregnancyDelivery Native = new NativePregnancyDelivery();

        public static IPregnancyDelivery Resolve(Identifier? id)
        {
            if (id is not { } deliveryId)
                return Native;

            if (Instances.TryGetValue(deliveryId, out var cached))
                return cached;

            if (!Factories.TryGetValue(deliveryId, out var factory))
                return Native;

            var instance = factory();

            Instances[deliveryId] = instance;

            return instance;
        }

        private static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || !typeof(IPregnancyDelivery).IsAssignableFrom(type))
                    continue;

                var attributes = type.GetCustomAttributes(typeof(Attributes.RegisterAttribute), false);

                if (attributes.Length == 0)
                    continue;

                var ctor = type.GetConstructor(Type.EmptyTypes);

                if (ctor == null)
                    continue;

                foreach (Attributes.RegisterAttribute attribute in attributes)
                {
                    Factories[Identifier.Parse(attribute.Id)] = () => (IPregnancyDelivery)ctor.Invoke(null);

                    count++;
                }
            }

            return count;
        }
    }
}
