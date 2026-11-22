using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class ParseResults
    {
        public required StringReader Reader { get; init; }

        public required CommandContext Context { get; init; }

        public Dictionary<string, object> Values { get; init; } = [];

        public CommandContextBuilder ContextBuilder { get; init; } = new(new CommandContext());

        public CommandNode? Node { get; set; }

        public CommandSyntaxException? Exception { get; set; }

        public SuggestionContext? Suggestion { get; set; }

        public bool IsSuccess => Exception is null && Node?.Executor is not null;
    }
}
