namespace LATravelManager.UI.Message
{
    public class IsBusyChangedMessage
    {
        public IsBusyChangedMessage(bool isBusy)
        {
            IsBusy = isBusy;
        }

        public bool IsBusy { get; }
    }
}