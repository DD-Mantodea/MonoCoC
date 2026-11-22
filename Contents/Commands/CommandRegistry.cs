using MonoCoC.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public static class CommandRegistry
    {
        private static CommandDispatcher Dispatcher => CommandDispatcher.Instance;

        public static IReadOnlyCollection<string> Names => Dispatcher.Names;

        public static CommandNode Create(string name)
        {
            var root = CommandNode.LiteralNode(name);

            return Dispatcher.Register(name, root);
        }

        public static bool TryExecute(string line, CommandContext context, out string error)
        {
            var result = Dispatcher.Parse(line, context);

            error = Dispatcher.GetError(result);

            if (result.Exception is not null)
                return false;

            return Dispatcher.Execute(result);
        }

        public static IReadOnlyList<Suggestion> GetSuggestions(string input, int cursor) =>
            Dispatcher.GetSuggestions(input, cursor);

        public static string GetHint(string line)
        {
            var result = Dispatcher.Parse(line, new CommandContext());

            return result.Exception?.LocalizedMessage ?? "";
        }

        public static string GetUsage(string line)
        {
            var name = line.TrimStart('/').Split(' ', 2)[0];

            if (!Dispatcher.Names.Contains(name))
                return "";

            var usage = Dispatcher.GetSmartUsage(name);

            return "Commands.Error.Usage".Localized("/" + name + (usage.Length == 0 ? "" : " " + usage));
        }

        public static string GetSmartUsage(string name) =>
            Dispatcher.GetSmartUsage(name);
    }
}
