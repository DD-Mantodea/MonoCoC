using MonoCoC.Extensions;
using System;

namespace MonoCoC.Contents.Commands
{
    public sealed class CommandSyntaxException : Exception
    {
        public string Key { get; }

        public object[] Args { get; }

        public int Cursor { get; }

        public CommandSyntaxException(string key, object[] args, int cursor) : base(key)
        {
            Key = key;

            Args = args;

            Cursor = cursor;
        }

        public string LocalizedMessage => Key.Localized(Args);
    }
}
