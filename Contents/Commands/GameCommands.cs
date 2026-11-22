using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Scenes;
using MonoCoC.Extensions;
using MonoCoC.Registry;
using MonoCoC.Saving.Legacy;
using MonoCoC.UI.Components.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public static class GameCommands
    {
        private static bool _registered;

        public static void Register()
        {
            if (_registered)
                return;

            _registered = true;

            CommandRegistry.Create("clear").Executes((context, _) => context.Clear());

            CommandRegistry.Create("help")
                .Executes(Help)
                .Then(CommandNode.ArgumentNode("command", new CommandNameArgumentType()).Executes(Help));

            CommandRegistry.Create("give")
                .Then(CommandNode.ArgumentNode("item", ItemIdentifier())
                    .Executes(Give)
                    .Then(CommandNode.ArgumentNode("count", new IntArgumentType(1, 999)).Executes(Give)));

            CommandRegistry.Create("scene")
                .Then(CommandNode.ArgumentNode("scene", SceneIdentifier()).Executes(ChangeScene));

            CommandRegistry.Create("time")
                .Then(CommandNode.LiteralNode("set")
                    .Then(CommandNode.ArgumentNode("time", new TimeSetArgumentType()).Executes(SetTime)))
                .Then(CommandNode.LiteralNode("add")
                    .Then(CommandNode.ArgumentNode("duration", new TimeAddArgumentType()).Executes(AddTime)));

            RegisterCombat();

            CommandRegistry.Create("player")
                .Then(CommandNode.LiteralNode("set")
                    .Then(CommandNode.ArgumentNode("attribute", new PlayerAttributeArgumentType())
                        .Then(CommandNode.ArgumentNode("value", new StringArgumentType()).Executes(SetPlayer))));
        }

        private static void RegisterCombat()
        {
            var start = CommandNode.LiteralNode("start")
                .Then(CommandNode.ArgumentNode("enemy", EnemyIdentifier()).Executes(StartCombat))
                .Then(CommandNode.ArgumentNode("enemies", new ListArgumentType<IdentifierArgumentType>(EnemyIdentifier()))
                    .Executes(StartCombatList));

            var add = CommandNode.LiteralNode("add")
                .Then(CommandNode.LiteralNode("enemy")
                    .Then(CommandNode.ArgumentNode("enemy", EnemyIdentifier())
                        .Executes(AddEnemy)
                        .Then(CommandNode.ArgumentNode("options", new HjsonArgumentType()).Executes(AddEnemyWithOptions))));

            var modify = CommandNode.LiteralNode("modify")
                .Then(CommandNode.LiteralNode("enemy")
                    .Then(CommandNode.ArgumentNode("index", new IntArgumentType(1, 999))
                        .Then(CommandNode.ArgumentNode("options", new HjsonArgumentType()).Executes(ModifyEnemy))))
                .Then(CommandNode.LiteralNode("player")
                    .Then(CommandNode.ArgumentNode("options", new HjsonArgumentType()).Executes(ModifyPlayer)));

            CommandRegistry.Create("combat")
                .Then(start)
                .Then(CommandNode.LiteralNode("end").Executes(EndCombat))
                .Then(add)
                .Then(modify);
        }

        private static T Get<T>(IReadOnlyDictionary<string, object> values, string name) =>
            values.TryGetValue(name, out var value) && value is T typed ? typed : default!;

        private static IdentifierArgumentType ItemIdentifier() =>
            new(() => ItemRegistry.Definitions.Ids.Select(id => id.ToString()),
                id => ItemRegistry.Definitions.Has(id));

        private static IdentifierArgumentType EnemyIdentifier() =>
            new(() => EnemyRegistry.Definitions.Ids.Select(id => id.ToString()),
                id => EnemyRegistry.Definitions.Has(id));

        private static IdentifierArgumentType SceneIdentifier() =>
            new(() => SceneRegistry.Scenes.Ids.Select(id => id.ToString()),
                id => SceneRegistry.Scenes.Has(id));

        private static bool InGame(CommandContext context)
        {
            if (CoC.Instance.Player.Loaded && GameView.Current is not null)
                return true;

            context.Output("Commands.Error.NotInGame".Localized());

            return false;
        }

        public static void Help(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (values.TryGetValue("command", out var name) && name is string command)
            {
                var usage = CommandRegistry.GetSmartUsage(command);

                context.Output("/" + command + (usage.Length == 0 ? "" : " " + usage));

                return;
            }

            context.Output("Commands:");

            foreach (var commandName in CommandRegistry.Names.OrderBy(item => item))
                context.Output("  /" + commandName);
        }

        public static void Give(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!InGame(context))
                return;

            var id = Get<Identifier>(values, "item");

            if (!ItemRegistry.Definitions.Has(id))
            {
                context.Output("Commands.Error.UnknownItem".Localized(id.ToString()));

                return;
            }

            var count = values.TryGetValue("count", out var raw) && raw is int value ? value : 1;

            var item = ItemRegistry.Create(id);

            if (!context.Player.AddItem(item, count))
            {
                context.Output("Commands.Error.InventoryFull".Localized());

                return;
            }

            context.Output("Commands.Give.Success".Localized(item.LongName, count));
        }

        public static void ChangeScene(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!InGame(context))
                return;

            var id = Get<Identifier>(values, "scene");

            if (!SceneRegistry.Scenes.Has(id))
            {
                context.Output("Commands.Error.UnknownScene".Localized(id.ToString()));

                return;
            }

            if (GameView.Current is not ISceneHostView host)
            {
                context.Output("Commands.Error.NoSceneHost".Localized());

                return;
            }

            var scene = SceneRegistry.CreateOrDefault(id);

            if (!host.CanHost(scene))
            {
                context.Output("Commands.Error.SceneNotAccepted".Localized(id.ToString()));

                return;
            }

            context.Close();

            Scene.Go(scene);
        }

        public static void SetTime(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!InGame(context))
                return;

            var hour = Get<int>(values, "time");

            context.View!.Time.Hours = hour;

            context.Output("Commands.Time.Set".Localized(hour));
        }

        public static void AddTime(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!InGame(context))
                return;

            var duration = Get<int>(values, "duration");

            context.View!.Time.PassTime(duration);

            context.Output("Commands.Time.Add".Localized(duration));
        }

        public static void StartCombat(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            StartCombat(context, [EnemyRegistry.Create(Get<Identifier>(values, "enemy"))]);
        }

        public static void StartCombatList(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            var ids = Get<List<object>>(values, "enemies").OfType<Identifier>().ToList();

            if (ids.Count == 0)
            {
                context.Output("Commands.Error.EmptyEnemyList".Localized());

                return;
            }

            StartCombat(context, ids.Select(EnemyRegistry.Create).ToList());
        }

        private static void StartCombat(CommandContext context, IReadOnlyList<Enemy> enemies)
        {
            if (!InGame(context))
                return;

            if (CombatController.InCombat)
            {
                context.Output("Commands.Error.AlreadyInCombat".Localized());

                return;
            }

            var previous = Scene.Current;

            context.Close();

            CombatScene.StartMultiple(enemies, () =>
            {
                if (previous is not null && previous is not CombatScene)
                    Scene.Go(previous);
            });
        }

        public static void EndCombat(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!CombatController.InCombat)
            {
                context.Output("Commands.Error.NotInCombat".Localized());

                return;
            }

            context.Close();

            CombatScene.CleanupAfterCombat();
        }

        public static void AddEnemy(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            AddEnemy(context, Get<Identifier>(values, "enemy"), null);
        }

        public static void AddEnemyWithOptions(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            AddEnemy(context, Get<Identifier>(values, "enemy"), Get<Hjson.JsonObject>(values, "options"));
        }

        private static void AddEnemy(CommandContext context, Identifier id, Hjson.JsonObject? options)
        {
            if (!CombatController.InCombat || CombatController.Instance?.Context is not { } combat)
            {
                context.Output("Commands.Error.NotInCombat".Localized());

                return;
            }

            var enemy = EnemyRegistry.Create(id);

            if (!ApplyEnemyOptions(context, enemy, options))
                return;

            combat.AddEnemy(enemy);

            context.Output("Commands.Combat.Added".Localized(enemy.ShortName));
        }

        public static void ModifyEnemy(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!CombatController.InCombat || CombatController.Instance?.Context is not { } combat)
            {
                context.Output("Commands.Error.NotInCombat".Localized());

                return;
            }

            var index = Get<int>(values, "index") - 1;

            if (index < 0 || index >= combat.Enemies.Count)
            {
                context.Output("Commands.Error.InvalidTarget".Localized());

                return;
            }

            ApplyEnemyOptions(context, combat.Enemies[index], Get<Hjson.JsonObject>(values, "options"));
        }

        public static void ModifyPlayer(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!InGame(context))
                return;

            var options = Get<Hjson.JsonObject>(values, "options");

            if (options.TryGetValue("hp", out var hp))
                context.Player.Health = Math.Clamp((float)hp, 0, context.Player.MaxHealth);

            if (options.TryGetValue("lust", out var lust))
                context.Player.Lust = Math.Clamp((float)lust, 0, context.Player.MaxLust);

            if (options.TryGetValue("fatigue", out var fatigue))
                context.Player.Fatigue = Math.Clamp((float)fatigue, 0, 100);

            context.Output("Commands.Combat.PlayerModified".Localized());
        }

        private static bool ApplyEnemyOptions(CommandContext context, Enemy enemy, Hjson.JsonObject? options)
        {
            if (options is null)
                return true;

            var allowed = new[] { "hp", "lust", "fatigue", "level" };

            var unknown = options.Keys.FirstOrDefault(key => !allowed.Contains(key));

            if (unknown is not null)
            {
                context.Output("Commands.Error.UnknownOption".Localized(unknown));

                return false;
            }

            if (options.TryGetValue("hp", out var hp))
                enemy.Health = Math.Clamp((float)hp, 0, enemy.MaxHealth);

            if (options.TryGetValue("lust", out var lust))
                enemy.Lust = Math.Clamp((float)lust, 0, enemy.MaxLust);

            if (options.TryGetValue("fatigue", out var fatigue))
                enemy.Fatigue = Math.Clamp((float)fatigue, 0, 100);

            if (options.TryGetValue("level", out var level))
                enemy.Level = (int)level;

            return true;
        }

        public static void SetPlayer(CommandContext context, IReadOnlyDictionary<string, object> values)
        {
            if (!InGame(context))
                return;

            var attribute = Get<Identifier>(values, "attribute");

            var text = Get<string>(values, "value");

            if (!float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out var number))
            {
                context.Output("Commands.Error.ExpectedNumber".Localized(text));

                return;
            }

            switch (attribute.Path)
            {
                case "hp":
                    context.Player.Health = Math.Clamp(number, 0, context.Player.MaxHealth);

                    break;
                case "lust":
                    context.Player.Lust = Math.Clamp(number, 0, context.Player.MaxLust);

                    break;
                case "fatigue":
                    context.Player.Fatigue = Math.Clamp(number, 0, 100);

                    break;
                case "corruption":
                    context.Player.Corruption = Math.Clamp(number, 0, 100);

                    break;
                case "level":
                    context.Player.Level = (int)number;

                    break;
                case "xp":
                    context.Player.Experience = number;

                    break;
                case "gems":
                    context.Player.Gems = (int)number;

                    break;
                case "day":
                    context.View!.Time.Days = (int)number;

                    break;
                case "hour":
                    context.View!.Time.Hours = Math.Clamp((int)number, 0, 23);

                    break;
                default:
                    context.Output("Commands.Error.UnknownAttribute".Localized(attribute.ToString()));

                    return;
            }

            context.Output("Commands.Player.Set".Localized(attribute.ToString(), text));
        }
    }
}
