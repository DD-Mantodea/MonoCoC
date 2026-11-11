namespace MonoCoC.UI.Data
{
    public interface IUIDataContext
    {
        object? Resolve(string path);
    }
}
