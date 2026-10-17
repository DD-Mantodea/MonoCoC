using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Items.Behaviors
{
    public static class ItemBehaviorRegistry
    {

        static ItemBehaviorRegistry() => RegistrationScanner.OnTypes(RegisterTypes);

        public static readonly Dictionary<string, Func<ItemBehaviorDefinition, IItemBehavior>> Types = new(StringComparer.OrdinalIgnoreCase);

        public static Registry<ItemBehaviorDefinition> Definitions => ManagerInstance<ItemBehaviorManager>.Instance.Registry;

        public static void RegisterType(string type, Func<ItemBehaviorDefinition, IItemBehavior> factory) => Types[type] = factory;

        internal static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || !typeof(IItemBehavior).IsAssignableFrom(type))
                    continue;

                var attributes = type.GetCustomAttributes(typeof(RegisterAttribute), false);

                if (attributes.Length == 0)
                    continue;

                var ctor = type.GetConstructor(Type.EmptyTypes)

                    ?? throw new InvalidOperationException($"[Register] {type.Name} 缺少空构造");

                foreach (RegisterAttribute attribute in attributes)
                {
                    Types[attribute.Id] = _ => (IItemBehavior)ctor.Invoke(null);

                    count++;
                }
            }

            return count;
        }

        public static IItemBehavior Create(ItemBehaviorDefinition definition)
        {
            if (!Types.TryGetValue(definition.Type, out var factory))
                throw new InvalidOperationException($"Unknown item behavior type '{definition.Type}' for '{definition.Id}'");

            return factory(definition);
        }

        public static bool Has(Identifier id) => Definitions.Has(id);

        public static IItemBehavior? Resolve(Identifier id) => Definitions.TryGet(id, out var definition) ? definition.Behavior : null;

        public static void Register(Identifier id, IItemBehavior behavior) =>

            Definitions.Register(id, () => new ItemBehaviorDefinition { Id = id, Type = behavior.GetType().Name, Behavior = behavior });
    }
}