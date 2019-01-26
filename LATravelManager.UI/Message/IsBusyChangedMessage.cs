namespace LATravelManager.UI.Message
{
    public class IsBusyChangedMessage
    {
        public IsBusyChangedMessage(bool visible)
        {
            IsVisible = visible;
        }

        public bool IsVisible { get; }
    }
}