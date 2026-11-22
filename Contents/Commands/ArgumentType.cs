using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public abstract class ArgumentType
    {
        public abstract string UsageText { get; }

        public abstract object Parse(StringReader reader);

        public virtual IEnumerable<string> ListSuggestions(CommandContext context, SuggestionsBuilder builder) => [];

        public virtual string SuggestTokenPrefix => "";
    }

    public abstract class ArgumentType<T> : ArgumentType
    {
        public sealed override object Parse(StringReader reader) => ParseValue(reader);

        public abstract T ParseValue(StringReader reader);
    }
}
