namespace MonoCoC.ModLoader
{
    public sealed record CocModEntry(string Name, long Offset, long Length, byte[] Hash);
}
