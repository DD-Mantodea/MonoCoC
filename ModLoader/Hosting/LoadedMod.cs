namespace MonoCoC.ModLoader
{
    public class LoadedMod
    {
        public required Mod Mod { get; init; }

        public required ModContext Context { get; init; }
    }
}
