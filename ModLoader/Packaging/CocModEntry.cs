namespace MonoCoC.ModLoader
{
    public record CocModEntry(string Name, long Offset, long Length, byte[] Hash);
}
