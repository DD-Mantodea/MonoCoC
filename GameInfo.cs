namespace MonoCoC
{
    /// <summary>
    /// Build and save-format metadata. Not gameplay state.
    /// </summary>
    public static class GameInfo
    {
        public const string SaveVersion = "hgg 1.7.0";

        public const string Version = "/hgg/ mod (Hollow Hearts, Hollow Heads, Halloween)";

#if DEBUG
        public const string BuildKind = "Debug";
#else
        public const string BuildKind = "Release";
#endif

        public const string Target = "WINDOWS";
    }
}
