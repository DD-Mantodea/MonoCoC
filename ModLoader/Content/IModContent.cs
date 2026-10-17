namespace MonoCoC.ModLoader
{
    /// <summary>
    /// Lifecycle implemented by content classes that live in a mod assembly.
    /// Existing MonoCoC base classes can implement this interface directly.
    /// </summary>
    public interface IModContent
    {
        void Load();

        void SetupContent();

        void Unload();
    }
}
