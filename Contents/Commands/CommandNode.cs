using MonoCoC.Contents.Commands.Arguments;
using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public class CommandExecution
    {
        public Dictionary<string, object> Values = [];

        public Dictionary<CommandNode, object> NodeValues = [];

        public object? Last;

        public Action<CommandExecution, CommandContext>? Action;

        public CommandNode? ActionNode;

        public void Record(CommandNode node, object value)
        {
            NodeValues[node] = value;

            if (node.Name != "")
                Values[node.Name] = value;

            if (node.Action == null)
                return;

            Action = node.Action;

            ActionNode = node;
        }

        public CommandExecution Clone() => new()
        {
            Values = new Dictionary<string, object>(Values),

            NodeValues = new Dictionary<CommandNode, object>(NodeValues),

            Last = Last,

            Action = Action,

            ActionNode = ActionNode
        };

        public T Get<T>(string name) => Values.TryGetValue(name, out var value) && value is T typed ? typed : default!;

        public bool TryGet<T>(string name, out T value)
        {
            if (Values.TryGetValue(name, out var stored) && stored is T typed)
            {
                value = typed;

                return true;
            }

            value = default!;

            return false;
        }
    }

    public abstract class CommandNode
    {
        public string Name = "";

        public bool IsOptional;

        public List<CommandNode> Children = [];

        public Action<CommandExecution, CommandContext>? Action;

        public virtual bool AcceptsToken => false;

        public virtual string UsageText => "";

        public virtual bool CanEnd
        {
            get
            {
                if (Action == null)
                    return false;

                if (Children.Count == 0)
                    return true;

                return Children.Exists(child => child.IsOptional && child.CanEnd);
            }
        }

        public virtual bool TryConsume(string token, CommandExecution execution, out string expected)
        {
            expected = "";

            return false;
        }

        public CommandNode Next(CommandNode child)
        {
            if (child != null)
                Children.Add(child);

            return this;
        }

        public CommandNode Execute(Action<CommandExecution, CommandContext> action)
        {
            Action = action;

            return this;
        }

        public IEnumerable<CommandNode> Consumers()
        {
            foreach (var child in Children)
            {
                if (child.AcceptsToken)
                    yield return child;

                if (child.IsOptional || !child.AcceptsToken)
                    foreach (var nested in child.Consumers())
                        yield return nested;
            }
        }

        public virtual IEnumerable<string> GetCompletions() => [];

        public void CollectCompletions(IReadOnlyList<string> tokens, int index, string prefix, List<string> results)
        {
            foreach (var child in Children)
            {
                if (!child.AcceptsToken)
                {
                    child.CollectCompletions(tokens, index, prefix, results);

                    continue;
                }

                if (index >= tokens.Count)
                {
                    foreach (var value in child.GetCompletions())
                        if (CommandCompletion.Matches(value, prefix))
                            results.Add(value);

                    continue;
                }

                if (child is LiteralNode literal)
                {
                    if (string.Equals(literal.Text, tokens[index], StringComparison.OrdinalIgnoreCase))
                        literal.CollectCompletions(tokens, index + 1, prefix, results);

                    continue;
                }

                child.CollectCompletions(tokens, index + 1, prefix, results);
            }
        }

        public string RenderUsage() => Render(node => node.UsageText);

        public string Render(Func<CommandNode, string> format)
        {
            if (!AcceptsToken)
                return RenderAlternatives(format, false);

            var text = format(this);

            if (Children.Count == 0)
                return text;

            if (Children.Count == 1)
            {
                var rest = Children[0].Render(format);

                return rest == "" ? text : text + " " + rest;
            }

            var alternatives = RenderAlternatives(format, true);

            return alternatives == "" ? text : text + " " + alternatives;
        }

        public string RenderAlternatives(Func<CommandNode, string> format, bool wrap)
        {
            var parts = new List<string>();

            foreach (var child in Children)
            {
                var part = child.Render(format);

                if (part != "")
                    parts.Add(part);
            }

            if (parts.Count == 0)
                return "";

            if (parts.Count == 1)
                return parts[0];

            var text = string.Join(" | ", parts);

            return wrap ? "(" + text + ")" : text;
        }

        public string MissingUsage
        {
            get
            {
                foreach (var child in Children)
                    if (!child.IsOptional)
                        return child.UsageText != "" ? child.UsageText : child.MissingUsage;

                return "";
            }
        }

        public static LiteralNode Literal(string text, string name = "") => new(text, name);

        public static ArgumentNode Argument(CommandArgument argument) => new(argument);

        public static ArgumentNode Optional(CommandArgument argument) => new(argument) { IsOptional = true };
    }

    public class LiteralNode : CommandNode
    {
        public readonly string Text;

        public LiteralNode(string text, string name = "")
        {
            Text = text;

            Name = name;
        }

        public override bool AcceptsToken => true;

        public override string UsageText => Text;

        public override IEnumerable<string> GetCompletions() => [Text];

        public override bool TryConsume(string token, CommandExecution execution, out string expected)
        {
            expected = UsageText;

            if (!string.Equals(Text, token, StringComparison.OrdinalIgnoreCase))
                return false;

            execution.Record(this, Text);

            return true;
        }
    }

    public class ArgumentNode : CommandNode
    {
        public new readonly CommandArgument Argument;

        public ArgumentNode(CommandArgument argument)
        {
            Argument = argument;

            Name = argument.Name;
        }

        public override bool AcceptsToken => true;

        public override string UsageText => IsOptional ? "[" + Argument.UsageText + "]" : Argument.UsageText;

        public override IEnumerable<string> GetCompletions() => Argument.GetCompletions();

        public override bool TryConsume(string token, CommandExecution execution, out string expected)
        {
            expected = Argument.UsageText;

            if (!Argument.TryParse(token, out var value))
                return false;

            execution.Record(this, value);

            execution.Last = value;

            return true;
        }
    }

    public static class CommandTree
    {
        public static bool TryParse(CommandNode root, IReadOnlyList<string> tokens, out CommandExecution execution, out string expected)
        {
            execution = new CommandExecution();

            if (tokens.Count == 0)
            {
                if (root.CanEnd)
                {
                    execution.Action = root.Action;

                    execution.ActionNode = root;
                }

                expected = root.CanEnd ? "" : root.MissingUsage;

                return root.CanEnd;
            }

            return Parse(root, 0, tokens, execution, out expected);
        }

        public static bool Parse(CommandNode node, int index, IReadOnlyList<string> tokens, CommandExecution execution, out string expected)
        {
            if (index == tokens.Count)
            {
                if (node.CanEnd)
                {
                    execution.Action = node.Action;

                    execution.ActionNode = node;

                    expected = "";

                    return true;
                }

                if (execution.Action != null)
                {
                    expected = "";

                    return true;
                }

                expected = node.MissingUsage;

                return false;
            }

            expected = node.MissingUsage;

            foreach (var candidate in node.Consumers())
            {
                var clone = execution.Clone();

                if (!candidate.TryConsume(tokens[index], clone, out var failure))
                {
                    expected = failure;

                    continue;
                }

                if (!Parse(candidate, index + 1, tokens, clone, out expected))
                    continue;

                Copy(clone, execution);

                return true;
            }

            return false;
        }

        public static void Copy(CommandExecution source, CommandExecution target)
        {
            target.Values = source.Values;

            target.NodeValues = source.NodeValues;

            target.Last = source.Last;

            target.Action = source.Action;

            target.ActionNode = source.ActionNode;
        }
    }
}