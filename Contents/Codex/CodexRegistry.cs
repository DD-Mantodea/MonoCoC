using MonoCoC.Assets;
using MonoCoC.Assets.ContentManagers;
using MonoCoC.Registry;

namespace MonoCoC.Contents.Codex
{
    public static class CodexRegistry
    {
        public static Registry<CodexEntry> Entries => ManagerInstance<CodexManager>.Instance.Registry;

        public static CodexEntry? Get(Identifier id) =>

            Entries.TryGet(id, out var entry) ? entry : null;
    }
}