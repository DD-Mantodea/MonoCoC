using MonoCoC.Attributes;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Followers
{

    public static class FollowerRegistry
    {
        static FollowerRegistry() => RegistrationScanner.OnTypes(RegisterTypes);

        public static readonly List<FollowerDefinition> Definitions = [];

        public static IReadOnlyList<FollowerDefinition> All => Definitions;

        internal static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || !typeof(FollowerDefinition).IsAssignableFrom(type))
                    continue;

                var attributes = type.GetCustomAttributes(typeof(RegisterAttribute), false);

                if (attributes.Length == 0)
                    continue;

                var ctor = type.GetConstructor(Type.EmptyTypes)

                    ?? throw new InvalidOperationException($"[Register] {type.Name} 缺少空构造");

                foreach (RegisterAttribute attribute in attributes)
                {
                    var definition = (FollowerDefinition)ctor.Invoke(null);

                    if (definition.Id.ToString() != Identifier.Parse(attribute.Id).ToString())
                        throw new InvalidOperationException(
                            $"[Register(\"{attribute.Id}\")] 与 {type.Name} 构造里的 id {definition.Id} 不一致");

                    Register(definition);

                    count++;
                }
            }

            return count;
        }

        public static void Register(FollowerDefinition definition)
        {
            Definitions.RemoveAll(d => d.Id == definition.Id);

            Definitions.Add(definition);

            Definitions.Sort((a, b) =>
            {
                var byOrder = a.Order.CompareTo(b.Order);

                return byOrder != 0 ? byOrder : string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase);
            });
        }

        public static void Register(params FollowerDefinition[] definitions)
        {
            foreach (var definition in definitions)
                Register(definition);
        }

        public static IReadOnlyList<FollowerDefinition> Present(Identifier role) =>

            Definitions
                .Where(definition => definition.Role == role && definition.IsPresent())
                .ToList();

        public static int Count(Identifier role) => Present(role).Count;

        public static int TotalCount => Definitions.Count(definition => definition.IsPresent());

        public static bool HasAny(Identifier role) => Count(role) > 0;

        public static bool HasFollower(Identifier followerId) =>

            Definitions.Any(definition => definition.Id == followerId && definition.IsPresent());

        public static bool CampGuarded =>

            Definitions.Any(definition => definition.IsPresent() && definition.IsGuard?.Invoke() == true);

        public static IReadOnlyList<FollowerDefinition> Guards =>

            Definitions
                .Where(definition => definition.IsPresent() && definition.IsGuard?.Invoke() == true)
                .ToList();
    }
}
