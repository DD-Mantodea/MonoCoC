using System.Collections;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class CommandContextBuilder(CommandContext source) : IReadOnlyDictionary<string, object>
    {
        public CommandContext Source { get; } = source;

        public Dictionary<string, object> Values { get; } = [];

        public List<ParsedArgument> Arguments { get; } = [];

        public List<CommandNode> Nodes { get; } = [];

        public void Add(CommandNode node, ParsedArgument? argument = null)
        {
            Nodes.Add(node);

            if (argument is not null)
                Arguments.Add(argument);
        }

        public object this[string key] => Values[key];

        public IEnumerable<string> Keys => Values.Keys;

        IEnumerable<object> IReadOnlyDictionary<string, object>.Values => Values.Values;

        public int Count => Values.Count;

        public bool ContainsKey(string key) => Values.ContainsKey(key);

        public bool TryGetValue(string key, out object value) => Values.TryGetValue(key, out value!);

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}

