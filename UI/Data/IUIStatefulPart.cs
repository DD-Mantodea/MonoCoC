namespace MonoCoC.UI.Data
{
    public interface IUIStatefulPart
    {
        object? CaptureState();

        void RestoreState(object state);
    }
}
