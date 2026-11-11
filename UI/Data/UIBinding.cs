namespace MonoCoC.UI.Data
{
    public class UIBinding(string path)
    {
        public string Path { get; } = path;

        public object? Resolve(IUIDataContext? context) => context?.Resolve(Path);
    }
}
