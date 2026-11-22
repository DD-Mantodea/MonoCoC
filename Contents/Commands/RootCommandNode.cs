using System;
using System.Collections.Generic;

namespace MonoCoC.Contents.Commands
{
    public sealed class RootCommandNode : CommandNode
    {
        public override string UsageText => "";

        public override ParsedArgument? Consume(StringReader reader, CommandContext context,
            Dictionary<string, object> values) =>
            throw new CommandSyntaxException("Commands.Error.Unknown", [""], reader.Cursor);
    }
}
