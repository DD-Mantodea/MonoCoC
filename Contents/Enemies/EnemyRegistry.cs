using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Enemies
{
    public static class EnemyRegistry
    {

        static EnemyRegistry() => Registry.RegistrationScanner.OnTypes(RegisterTypes);

        public static readonly Dictionary<string, Func<EnemyDefinition, Enemy>> Types = new(StringComparer.OrdinalIgnoreCase);

        public static Registry<EnemyDefinition> Definitions => ManagerInstance<EnemyManager>.Instance.Registry;

        public static void RegisterType(string type, Func<EnemyDefinition, Enemy> factory) => Types[type] = factory;

        public static readonly Dictionary<string, Type> TypeOfId = new(StringComparer.OrdinalIgnoreCase);

        internal static int RegisterTypes(Type[] types)
        {
            var count = 0;

            foreach (var type in types)
            {
                if (type.IsAbstract || !typeof(Enemy).IsAssignableFrom(type))
                    continue;

                foreach (var attribute in type.GetCustomAttributes(typeof(Attributes.RegisterAttribute), false))
                {
                    Register(type, (Attributes.RegisterAttribute)attribute);

                    count++;
                }
            }

            return count;
        }

        public static void Register(Type annotated, Attributes.RegisterAttribute attribute)
        {

            var factoryType = attribute.Factory ?? annotated;

            if (!typeof(Enemy).IsAssignableFrom(factoryType) || factoryType.IsAbstract)
                throw new InvalidOperationException(
                    $"[Register(\"{attribute.Id}\")] 的工厂类型 {factoryType.Name} 不是具体的 Enemy 子类");

            if (TypeOfId.TryGetValue(attribute.Id, out var existing) && !attribute.Replace)
                throw new InvalidOperationException(
                    $"敌人 id 重复: \"{attribute.Id}\" 已被 {existing.Name} 注册，{factoryType.Name} 想注册同一个 id（要覆盖请用 Replace = true）");

            TypeOfId[attribute.Id] = factoryType;

            var factory = CompileFactory(factoryType);

            Types[attribute.Id] = _ => factory();
        }

        public static Func<Enemy> CompileFactory(Type factoryType) =>

            (Func<Enemy>)typeof(EnemyRegistry)
                .GetMethod(nameof(NewFactory), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                .MakeGenericMethod(factoryType)
                .Invoke(null, null)!;

        public static Func<Enemy> NewFactory<T>() where T : Enemy, new() => static () => new T();

        public static string Dump()
        {
            var lines = new List<string>();

            foreach (var pair in TypeOfId.OrderBy(p => p.Key, StringComparer.Ordinal))
                lines.Add($"  {pair.Key,-24} {pair.Value.Name}");

            try
            {
                var implemented = new HashSet<string>(TypeOfId.Keys, StringComparer.OrdinalIgnoreCase);

                foreach (var id in Definitions.Ids)
                {
                    var definition = Definitions.Get(id);

                    if (!implemented.Contains(definition.Type))
                        lines.Add($"  {definition.Type,-24} (数据里有定义，但没有 [Register] 的敌人类 → 会用 GenericEnemy)");
                }
            }
            catch
            {

            }

            return string.Join(Environment.NewLine, lines);
        }

        public static Enemy Create(Identifier id)
        {
            if (!Definitions.TryGet(id, out var definition))
                throw new KeyNotFoundException($"Unregistered enemy '{id}'");

            return Create(definition);
        }

        public static Enemy Create(EnemyDefinition definition)
        {
            if (!Types.TryGetValue(definition.Type, out var factory))
                factory = _ => new GenericEnemy();

            var enemy = factory(definition);

            enemy.Id = definition.Id;

            enemy.Configure(definition);

            // 原版 Monster.hx:893：开启「等级缩放」时按玩家等级提升敌人
            if (Settings.ModeSettings.Instance.Scaling)
                enemy.ScaleToLevel();

            enemy.ApplyDefinitionEquipment(definition);

            return enemy;
        }
    }
}
