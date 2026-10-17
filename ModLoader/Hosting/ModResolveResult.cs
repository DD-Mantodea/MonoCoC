using System.Collections.Generic;

namespace MonoCoC.ModLoader
{
    public sealed class ModResolveResult
    {
        public List<ModCandidate> Mods { get; } = [];

        public List<string> Errors { get; } = [];

        public List<string> Warnings { get; } = [];

        public List<string> EnabledIds { get; } = [];
    }
}
