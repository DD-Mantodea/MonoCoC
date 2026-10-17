using System;
using System.Collections.Generic;

namespace MonoCoC.ModLoader
{
    public sealed class ModResolveException : Exception
    {
        public ModResolveException(IReadOnlyList<string> errors) : base(string.Join(Environment.NewLine, errors))
        {
            Errors = errors;
        }

        public IReadOnlyList<string> Errors { get; }
    }
}
