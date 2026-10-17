namespace MonoCoC.ModLoader
{
    public sealed class ModCandidate
    {
        public required string PackagePath { get; init; }

        public ModManifest? Manifest { get; init; }

        public string? Error { get; init; }

        public bool IsValid => Manifest is not null && Error is null;

        public string Id => Manifest?.Id ?? System.IO.Path.GetFileNameWithoutExtension(PackagePath);

        public override string ToString() => Manifest is null ? Id : $"{Manifest.Id} {Manifest.Version}";
    }
}
