using MonoCoC.Contents.Creatures;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Pregnancy
{

    public class PregnancyContext
    {
        public Player Player = null!;

        public PregnancyState State = null!;

        public PregnancyType? Definition;

        public bool Anal;

        public Action<string> Write = _ => { };

        public Identifier Father => State.Type;

        public int Incubation => State.Incubation;
    }

    public interface IPregnancyDelivery
    {
        bool Update(PregnancyContext context);

        void Deliver(PregnancyContext context);
    }

    public class NativePregnancyDelivery : IPregnancyDelivery
    {
        public virtual bool Update(PregnancyContext context) => false;

        public virtual void Deliver(PregnancyContext context)
        {
        }
    }

    public static class PregnancyDeliveryRegistry
    {
        static PregnancyDeliveryRegistry() => RegistrationScanner.OnTypes(RegisterTypes);

        public static readonly Dictionary<Identifier, Func<IPregnancyDelivery>> Factories = [];

        public static readonly Dictionary<Identifier, IPregnancyDelivery> Instances = [];

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

        public static int RegisterTypes(Type[] types)
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