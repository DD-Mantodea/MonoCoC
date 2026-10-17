using System;

namespace MonoCoC.Contents.Stats.ModifierSources
{
    public record TemporarySource(Guid Id) : IModifierSource
    {
        public string DisplayName => "Temporary";
    }
}