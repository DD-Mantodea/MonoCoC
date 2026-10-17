using MonoCoC.Contents.Commands.Arguments;
using MonoCoC.Contents.Combat;
using MonoCoC.Contents.Items;
using MonoCoC.Contents.Enemies;
using MonoCoC.Contents.Scenes;
using MonoCoC.Registry;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public static class VanillaCommands
    {
        public static bool _registered;

        public static void Register()
        {
            if (_registered)
                return;

            _registered = true;

            CommandRegistry.Create("help")
                .Execute(Help)
                .Next(CommandNode.Optional(new StringArgument("command")).Execute(Help))
                .Register();

            CommandRegistry.Create("clear")
                .Execute((_, context) => context.Clear())
                .Register();

            CommandRegistry.Create("cls")
                .Execute((_, context) => context.Clear())
                .Register();

            CommandRegistry.Create("echo")
                .Next(CommandNode.Argument(new StringArgument("text")).Execute(Echo))
                .Register();

            CommandRegistry.Create("give")
                .Next(CommandNode.Argument(new ItemArgument("item"))
                    .Execute(Give)
                    .Next(CommandNode.Optional(new IntArgument("count")).Execute(Give)))
                .Register();

            CommandRegistry.Create("items")
                .Execute(Items)
                .Register();

            CommandRegistry.Create("scene")
                .Next(CommandNode.Argument(new SceneArgument("scene")).Execute(ChangeScene))
                .Register();

            CommandRegistry.Create("combat")
                .Next(CommandNode.Argument(new EnemyArgument("enemy")).Execute(StartCombat))
                .Register();

            CommandRegistry.Create("time")
                .Next(CommandNode.Argument(new TimeArgument("hour")).Execute(SetTime))
                .Register();

            CommandRegistry.Create("heal")
                .Execute(Heal)
                .Register();

            CommandRegistry.Create("lust")
                .Next(CommandNode.Argument(new FloatArgument("amount")).Execute(Lust))
                .Register();

            CommandRegistry.Create("cor")
                .Next(CommandNode.Argument(new FloatArgument("amount")).Execute(Corruption))
                .Register();
        }

        public static void Help(CommandExecution execution, CommandContext context)
        {
            if (execution.TryGet<string>("command", out var name))
            {
                var command = CommandRegistry.Find(name);

                if (command == null)
                {
                    context.Output("Unknown command: " + name);

                    return;
                }

                context.Output("/" + command.Name + (command.Usage == "" ? "" : " " + command.Usage));

                return;
            }

            context.Output("Commands:");

            foreach (var command in CommandRegistry.Names.OrderBy(name => name))
                context.Output("  /" + command);
        }

        public static void Echo(CommandExecution execution, CommandContext context)
        {
            context.Output(execution.Get<string>("text"));
        }

        public static void Give(CommandExecution execution, CommandContext context)
        {
            var item = execution.Get<Item>("item");

            var count = execution.TryGet<int>("count", out var amount) ? amount : 1;

            if (count <= 0)
            {
                context.Output("Count must be positive.");

                return;
            }

            if (!context.Player.AddItem(item, count))
                context.Output("No room for " + item.LongName + ".");

            else
                context.Output("Added " + count + "x " + item.Name + ".");
        }

        public static void Items(CommandExecution execution, CommandContext context)
        {
            context.Output("Equipment:");

            context.Output("  Weapon: " + context.Player.Weapon.Name);

            context.Output("  Shield: " + context.Player.Shield.Name);

            context.Output("  Armor: " + context.Player.Armor.Name);

            context.Output("  Upper: " + context.Player.UpperGarment.Name);

            context.Output("  Lower: " + context.Player.LowerGarment.Name);

            context.Output("  Accessory: " + context.Player.Jewelry.Name);

            context.Output("Items:");

            for (var index = 0; index < context.Player.ItemStacks.Length; index++)
            {
                var slot = context.Player.ItemStacks[index];

                if (!slot.Unlocked || slot.IsEmpty)
                    continue;

                context.Output("  " + index + ": " + slot.InvLabel);
            }
        }

        public static void ChangeScene(CommandExecution execution, CommandContext context)
        {
            var id = execution.Get<Identifier>("scene");

            context.Close();

            Scene.Go(SceneRegistry.CreateOrDefault(id));
        }

        public static void StartCombat(CommandExecution execution, CommandContext context)
        {
            var id = execution.Get<Identifier>("enemy");

            var enemy = EnemyRegistry.Create(id);

            var previous = Scene.Current;

            context.Close();

            CombatScene.Start(enemy, () =>
            {
                if (previous != null && previous is not CombatScene)
                    Scene.Go(previous);
            });
        }

        public static void SetTime(CommandExecution execution, CommandContext context)
        {
            var hour = execution.Get<int>("hour");

            if (context.View == null)
            {
                context.Output("No active view.");

                return;
            }

            context.View.Time.Hours = hour;

            context.Output("Time set to " + hour + ":00.");
        }

        public static void Heal(CommandExecution execution, CommandContext context)
        {
            context.Player.Health = context.Player.MaxHealth;

            context.Output("Health restored.");
        }

        public static void Lust(CommandExecution execution, CommandContext context)
        {
            var amount = execution.Get<float>("amount");

            context.Player.Lust += amount;

            context.Output("Lust = " + context.Player.Lust);
        }

        public static void Corruption(CommandExecution execution, CommandContext context)
        {
            var amount = execution.Get<float>("amount");

            context.Player.Corruption += amount;

            context.Output("Corruption = " + context.Player.Corruption);
        }
    }
}
