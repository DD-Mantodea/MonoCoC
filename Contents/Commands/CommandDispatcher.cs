using MonoCoC.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonoCoC.Contents.Commands
{
    public sealed class CommandDispatcher
    {
        public static readonly CommandDispatcher Instance = new();

        public RootCommandNode Root { get; } = new();

        private readonly Dictionary<string, CommandNode> _commands = new(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<string> Names => _commands.Keys;

        public CommandNode Register(string name, CommandNode command)
        {
            _commands[name] = command;

            Root.Then(command);

            return command;
        }

        public CommandNode? Find(string name) =>
            _commands.TryGetValue(name, out var command) ? command : null;

        public ParseResults Parse(string input, CommandContext context)
        {
            var reader = new StringReader(input.TrimStart('/'));

            var contextBuilder = new CommandContextBuilder(context);

            var result = new ParseResults
            {
                Reader = reader,
                Context = context,
                ContextBuilder = contextBuilder,
                Values = contextBuilder.Values
            };

            try
            {
                reader.SkipWhitespace();

                if (!reader.CanRead)
                    return result;

                var name = reader.ReadString();

                if (!_commands.TryGetValue(name, out var root))
                    throw new CommandSyntaxException("Commands.Error.Unknown", [name], reader.Cursor);

                result.Node = root.Parse(reader, context, result.Values, result.ContextBuilder);

                if (result.Node is null && root.RedirectTarget is not null)
                    result.Node = root;

                if (result.Node?.RedirectTarget is { } redirect && reader.CanRead)
                {
                    var redirected = redirect.Parse(reader.Remaining, context);

                    foreach (var pair in redirected.Values)
                        result.Values[pair.Key] = pair.Value;

                    result.Node = redirected.Node;

                    result.Exception = redirected.Exception;

                    reader.Cursor = input.Length;
                }

                reader.SkipWhitespace();

                if (result.Node?.Executor is null)
                {
                    result.Exception = FindFailure(root, reader.Clone(), context) ??
                        (reader.CanRead
                            ? new CommandSyntaxException("Commands.Error.ExpectedArgument",
                                [root.RenderUsage()], reader.Cursor)
                            : new CommandSyntaxException("Commands.Error.Usage",
                                ["/" + root.RenderUsage()], reader.Cursor));
                }
                else if (reader.CanRead)
                    result.Exception = new CommandSyntaxException("Commands.Error.Usage",
                        ["/" + root.RenderUsage()], reader.Cursor);
            }
            catch (CommandSyntaxException exception)
            {
                result.Exception = exception;
            }

            return result;
        }

        public bool Execute(ParseResults result)
        {
            if (result.Exception is not null || result.Node?.Executor is null)
                return false;

            if (result.Node.IsFork)
            {
                var sources = result.Context.Sources.Count == 0
                    ? [result.Context]
                    : result.Context.Sources;

                var executed = 0;

                foreach (var source in sources)
                {
                    var builder = ReferenceEquals(source, result.Context)
                        ? result.ContextBuilder
                        : new CommandContextBuilder(source);

                    if (!ReferenceEquals(source, result.Context))
                        foreach (var pair in result.ContextBuilder.Values)
                            builder.Values[pair.Key] = pair.Value;

                    result.Node.Executor(source, builder);

                    executed++;
                }

                return executed > 0;
            }

            result.Node.Executor(result.Context, result.ContextBuilder);

            return true;
        }

        public IReadOnlyList<Suggestion> GetSuggestions(string input, int cursor)
        {
            var safe = Math.Clamp(cursor, 0, input.Length);

            var source = input[..safe];

            var tokenStart = safe;

            while (tokenStart > 0 && !char.IsWhiteSpace(source[tokenStart - 1]))
                tokenStart--;

            var token = source[tokenStart..safe];

            var reader = new StringReader(source);

            reader.SkipWhitespace();

            if (tokenStart == 0)
                return _commands.Keys
                    .Where(name => name.StartsWith(token, StringComparison.OrdinalIgnoreCase))
                    .Select(name => new Suggestion(StringRange.Between(tokenStart, safe), name))
                    .OrderBy(suggestion => suggestion.Text)
                    .Take(20)
                    .ToList();

            var name = reader.ReadString();

            if (!_commands.TryGetValue(name, out var root))
                return [];

            var values = new Dictionary<string, object>();

            var current = ParsePrefix(root, reader, new CommandContext(), values);

            var builder = new SuggestionsBuilder(source, tokenStart);

            SuggestChildren(current, tokenStart, source, builder);

            var merged = Suggestions.Merge(source, new Suggestions(source, builder.Build()));

            return merged.List.Take(20).ToList();
        }

        public Task<Suggestions> GetSuggestionsAsync(string input, int cursor) =>
            Task.FromResult(new Suggestions(input, GetSuggestions(input, cursor)));

        private static CommandNode ParsePrefix(CommandNode node, StringReader reader,
            CommandContext context, Dictionary<string, object> values)
        {
            reader.SkipWhitespace();

            if (!reader.CanRead)
                return node;

            foreach (var child in node.Children)
            {
                var clone = reader.Clone();

                try
                {
                    child.Consume(clone, context, values);
                }
                catch (CommandSyntaxException)
                {
                    continue;
                }

                return ParsePrefix(child, clone, context, values);
            }

            return node;
        }

        private static void SuggestChildren(CommandNode node, int tokenStart, string source,
            SuggestionsBuilder builder)
        {
            var token = source[tokenStart..];

            foreach (var child in node.Children)
            {
                if (child is LiteralCommandNode literal)
                {
                    if (literal.Literal.StartsWith(token, StringComparison.OrdinalIgnoreCase))
                        builder.Suggest(literal.Literal);
                }
                else if (child is ArgumentCommandNode argument)
                    foreach (var value in argument.Argument.ListSuggestions(
                                 new CommandContext(), builder))
                        builder.Suggest(value);
            }
        }

        private static CommandSyntaxException? FindFailure(CommandNode node, StringReader reader,
            CommandContext context)
        {
            reader.SkipWhitespace();

            if (!reader.CanRead)
                return null;

            CommandSyntaxException? best = null;

            foreach (var child in node.Children)
            {
                var clone = reader.Clone();

                try
                {
                    child.Consume(clone, context, new Dictionary<string, object>());
                }
                catch (CommandSyntaxException exception)
                {
                    if (best is null || exception.Cursor > best.Cursor)
                        best = exception;

                    continue;
                }

                var nested = FindFailure(child, clone, context);

                if (nested is not null && (best is null || nested.Cursor > best.Cursor))
                    best = nested;
            }

            return best;
        }

        public string GetSmartUsage(string name)
        {
            if (!_commands.TryGetValue(name, out var node))
                return "";

            var parts = node.Children
                .Select(child => SmartUsage(child, node.Executor is not null))
                .Distinct()
                .ToList();

            return string.Join(" ", parts);
        }

        private static string SmartUsage(CommandNode node, bool optional)
        {
            var self = optional ? "[" + node.UsageText + "]" : node.UsageText;

            if (node.Children.Count == 0)
                return self;

            var childOptional = node.Executor is not null;

            if (node.Children.Count == 1)
                return self + " " + SmartUsage(node.Children[0], childOptional);

            var children = node.Children
                .Select(child => SmartUsage(child, false))
                .Distinct()
                .ToList();

            if (children.Count == 1)
                return self + " " + (childOptional ? "[" + children[0] + "]" : children[0]);

            var open = childOptional ? "[" : "(";

            var close = childOptional ? "]" : ")";

            return self + " " + open + string.Join("|", children) + close;
        }

        public string GetError(ParseResults result) =>
            result.Exception?.LocalizedMessage ?? "";
    }
}
