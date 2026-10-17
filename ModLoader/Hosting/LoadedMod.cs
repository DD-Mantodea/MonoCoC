namespace MonoCoC.ModLoader
{
    public sealed class LoadedMod
    {
        public required Mod Mod { get; init; }

        public required ModContext Context { get; init; }
    }
}
