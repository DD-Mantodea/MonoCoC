using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public readonly record struct StringRange(int Start, int End)
    {
        public int Length => End - Start;

        public static StringRange Between(int start, int end) => new(start, end);

        public static StringRange At(int position) => new(position, position);
    }

    public sealed record Suggestion(StringRange Range, string Text, string? Tooltip = null)
    {
        public int Start => Range.Start;

        public int Length => Range.Length;
    }

    public sealed class SuggestionsBuilder(string input, int start)
    {
        private readonly List<Suggestion> _suggestions = [];

        public string Input { get; } = input;

        public int Start { get; } = start;

        public string Remaining => Input[Start..];

        public SuggestionsBuilder Suggest(string text, string? tooltip = null)
        {
            if (!string.Equals(text, Remaining, StringComparison.Ordinal))
                _suggestions.Add(new Suggestion(StringRange.Between(Start, Input.Length), text, tooltip));

            return this;
        }

        public SuggestionsBuilder Add(SuggestionsBuilder other)
        {
            _suggestions.AddRange(other._suggestions);

            return this;
        }

        public SuggestionsBuilder AddRange(IEnumerable<Suggestion> suggestions)
        {
            _suggestions.AddRange(suggestions);

            return this;
        }

        public SuggestionsBuilder CreateOffset(int offset) => new(Input, offset);

        public SuggestionsBuilder Restart() => CreateOffset(Start);

        public IReadOnlyList<Suggestion> Build() => _suggestions;

        public System.Threading.Tasks.Task<Suggestions> BuildFuture() =>
            System.Threading.Tasks.Task.FromResult(new Suggestions(Input, _suggestions));
    }

    public sealed record Suggestions(string Input, IReadOnlyList<Suggestion> List)
    {
        public static Suggestions Empty(string input) => new(input, []);

        public static Suggestions Merge(string input, params Suggestions[] values)
        {
            var merged = new List<Suggestion>();

            foreach (var value in values)
                foreach (var suggestion in value.List)
                    if (!merged.Exists(existing => existing.Text == suggestion.Text &&
                                                   existing.Range == suggestion.Range))
                        merged.Add(suggestion);

            return new Suggestions(input, merged);
        }
    }
}
