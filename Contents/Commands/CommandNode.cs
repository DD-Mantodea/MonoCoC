using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoCoC.Contents.Commands
{
    public abstract class CommandNode
    {
        public string Name = "";

        public Action<CommandContext, IReadOnlyDictionary<string, object>>? Executor;

        public CommandDispatcher? RedirectTarget { get; private set; }

        public bool IsFork { get; private set; }

        public readonly List<CommandNode> Children = [];

        public abstract string UsageText { get; }

        public CommandNode Executes(Action<CommandContext, IReadOnlyDictionary<string, object>> action)
        {
            Executor = action;

            return this;
        }

        public CommandNode Then(CommandNode child)
        {
            Children.Add(child);

            return this;
        }

        public CommandNode RedirectsTo(CommandDispatcher dispatcher)
        {
            RedirectTarget = dispatcher;

            return this;
        }

        public CommandNode Forks()
        {
            IsFork = true;

            return this;
        }

        public static CommandNode LiteralNode(string text) => new LiteralCommandNode(text);

        public static CommandNode ArgumentNode(string name, ArgumentType type) =>
            new ArgumentCommandNode(name, type);

        public abstract ParsedArgument? Consume(StringReader reader, CommandContext context,
            Dictionary<string, object> values);

        public CommandNode? Parse(StringReader reader, CommandContext context,
            Dictionary<string, object> values, CommandContextBuilder? builder = null)
        {
            reader.SkipWhitespace();

            if (!reader.CanRead)
            {
                builder?.Add(this);

                return Executor is not null ? this : null;
            }

            foreach (var child in Children)
            {
                var clone = reader.Clone();

                ParsedArgument? parsed;

                try
                {
                    parsed = child.Consume(clone, context, values);
                }
                catch (CommandSyntaxException)
                {
                    continue;
                }

                var childBuilder = builder is null ? null : new CommandContextBuilder(context);

                var result = child.Parse(clone, context, values, childBuilder);

                if (result is not null)
                {
                    reader.Cursor = clone.Cursor;

                    builder?.Add(child, parsed);

                    if (childBuilder is not null)
                        foreach (var argument in childBuilder.Arguments)
                            builder!.Arguments.Add(argument);

                    return result;
                }
            }

            builder?.Add(this);

            return Executor is not null ? this : null;
        }

        public string RenderUsage()
        {
            var parts = new List<string> { UsageText };

            if (Children.Count == 1)
            {
                var child = Children[0].RenderUsage();

                if (Executor is not null)
                    child = "[" + child + "]";

                parts.Add(child);
            }
            else if (Children.Count > 1)
            {
                parts.Add("(" + string.Join("|", Children.Select(child => child.RenderUsage())) + ")");
            }

            return string.Join(" ", parts.Where(part => part.Length > 0));
        }
    }
}
