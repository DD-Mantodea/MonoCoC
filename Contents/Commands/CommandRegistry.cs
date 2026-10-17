using MonoCoC.Contents.Creatures;
using MonoCoC.Contents.Flags;
using MonoCoC.UI.Components.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public class CommandContext
    {
        public Player Player => CoC.Instance.Player;

        public GameFlags Flags => CoC.Instance.Flags;

        public GameView? View => GameView.Instance;

        public Action<string> Output = _ => { };

        public Action Clear = () => { };

        public Action Close = () => { };
    }

    public class CommandRegistry
    {
        public static readonly Dictionary<string, CommandRegistry> Registered = new(StringComparer.OrdinalIgnoreCase);

        public readonly CommandNode _root = new RootNode();

        public CommandRegistry(string name) => Name = name;

        public string Name { get; }

        public string Usage => _root.RenderUsage();

        public static IReadOnlyCollection<string> Names => Registered.Keys;

        public static CommandRegistry Create(string name) => new(name);

        public static CommandRegistry? Find(string name) => Registered.TryGetValue(name, out var command) ? command : null;

        public CommandRegistry Next(CommandNode node)
        {
            _root.Next(node);

            return this;
        }

        public CommandRegistry Execute(Action<CommandExecution, CommandContext> action)
        {
            _root.Execute(action);

            return this;
        }

        public void Register() => Registered[Name] = this;

        public bool TryParse(IReadOnlyList<string> tokens, out CommandExecution execution, out string expected) =>

            CommandTree.TryParse(_root, tokens, out execution, out expected);

        public static bool TryExecute(string line, CommandContext context, out string error)
        {
            error = "";

            var tokens = CommandTokenizer.Tokenize(line);

            if (tokens.Count == 0)
                return true;

            var name = tokens[0].Text.TrimStart('/');

            if (!Registered.TryGetValue(name, out var command))
            {
                error = "Unknown command: " + name;

                return false;
            }

            var arguments = tokens.Skip(1).Select(token => token.Text).ToArray();

            if (!command.TryParse(arguments, out var execution, out var expected) || execution.Action == null)
            {
                error = "Usage: " + name + (command.Usage == "" ? "" : " " + command.Usage);

                if (expected != "")
                    error += " (expected " + expected + ")";

                return false;
            }

            try
            {
                execution.Action(execution, context);

                return true;
            }
            catch (Exception exception)
            {
                error = exception.Message;

                return false;
            }
        }

        public static List<string> Complete(string input, int cursor)
        {
            var start = Math.Clamp(cursor, 0, input.Length);

            while (start > 0 && !char.IsWhiteSpace(input[start - 1]))

                start--;

            var prefix = input[start..Math.Clamp(cursor, 0, input.Length)].Trim('"', '\'');

            var before = input[..start].Trim();

            if (before == "")
                return CommandCompletion.Filter(Registered.Keys, prefix).ToList();

            var tokens = CommandTokenizer.Tokenize(before);

            if (tokens.Count == 0)
                return [];

            var command = Find(tokens[0].Text);

            if (command == null)
                return [];

            var arguments = tokens.Skip(1).Select(token => token.Text).ToArray();

            var results = new List<string>();

            command._root.CollectCompletions(arguments, 0, prefix, results);

            return results.Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(value => value).ToList();
        }

        public static void ClearRegistry() => Registered.Clear();

        /// <summary>
        /// 框架占位：命令树的根节点（见本文件 :29 的 _root 字段）。
        /// 根节点只承担「容器」职责，所有命令都挂在它的子节点上，因此类体为空是设计如此。
        /// </summary>
        public class RootNode : CommandNode
        {
        }
    }
}